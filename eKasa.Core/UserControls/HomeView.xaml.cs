using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core
{
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();

			titleLabel.Text = $"Hoşgeldin, {Database.InternalDb.Owner}!";
			entriesDataGrid.ItemsSource = Database.InternalDb.Entries;
			UpdateDbHint();
		}

		public void UpdateDbHint()
		{
			var idb = Database.InternalDb;
			var modifiedDate = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortDateString();
			var modifiedTime = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortTimeString();

			entriesDataGrid.SelectedIndex = -1;
			titleLabel.Text = $"Hoşgeldin, {idb.Owner}!";
			dbDataLabel.Content = $"DB: {idb.Name}, Son değişiklik: {modifiedDate} - {modifiedTime}";
		}

		private void OptionsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;

			var canvas = (Canvas)Parent;
			var ouc = new ActionsView();
			canvas.Children.Add(ouc);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;

			if ((EntryModel)entriesDataGrid.SelectedItem != null) {
				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(HomeWindow.editv);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;

			Database.InternalDb.Entries.Remove((EntryModel)entriesDataGrid.SelectedItem);
			Database.InternalDb.ModifiedDate = DateTime.UtcNow.ToString();
			HomeWindow.UpdateHomeView();

			tooltipLabel.Content = "Kayıt silindi!";
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Veritabanını yenilemek istediğinize emin misiniz?\nKaydedilmemiş değişiklikleriniz KAYBOLACAKtır!", "Veritabanını yenile?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			try {
				if (r == MessageBoxResult.Yes) {
					Database.Restore();
					HomeWindow.UpdateHomeView();
					tooltipLabel.Content = "Veritabanı yenilendi!";
				}
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				Database.InternalDb.ModifiedDate = DateTime.UtcNow.ToString();

				Database.Save();
				HomeWindow.UpdateHomeView();
				tooltipLabel.Content = "Veritabanı kaydedildi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void LockButton_Click(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Veritabanını kilitlemek istediğinize emin misiniz?\nKaydedilmemiş değişiklikleriniz KAYDEDİLECEKtır!", "Veritabanını kilitle?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if (r == MessageBoxResult.Yes) {
				Database.Save();
				Process.Start(Process.GetCurrentProcess().MainModule.FileName);
				Application.Current.Shutdown();
			}
		}

		private void ExportButton_Click(object sender, RoutedEventArgs e)
		{
			var idb = Database.InternalDb;
			var edb = new DatabaseModel {
				Name = idb.Name,
				Owner = idb.Owner,
				PwdHash = null,
				Entries = idb.Entries,
				ModifiedDate = idb.ModifiedDate,
				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
			};

			var ask = MessageBox.Show("Veritabanını ŞİFRESİZ olarak dışa aktarmak istediğinize emin misiniz?", "Veritabanını dışa aktar!", MessageBoxButton.YesNo, MessageBoxImage.Question);

			try {
				if (ask == MessageBoxResult.Yes) {
				tryAgain:
					OpenFileDialog ofd = new();
					ofd.Title = "Dışa aktarma konumu seçin!";
					ofd.Filter = "JSON dosyası (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
					ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
					ofd.AddExtension = true;
					ofd.CheckPathExists = true;
					ofd.CheckFileExists = false;
					ofd.ShowDialog();

					if (ofd.FileName == "") {
						var retry = MessageBox.Show("Dışa aktarma konumu seçmediniz.\nTekrar denemek ister misiniz?", "Tekrar dene?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

						if (retry == MessageBoxResult.Yes) goto tryAgain;
						else tooltipLabel.Content = "Dışa aktarma iptal edildi!";
					}
					else {
						Database.ToJson(edb, ofd.FileName);
						tooltipLabel.Content = "Dışa aktarma başarılı!";
					}
				}
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void ImportButton_Click(object sender, RoutedEventArgs e)
		{
			var ask = MessageBox.Show("Veritabanı içe aktarmak istediğinize emin misiniz?\nKaydedilmemiş değişiklikleriniz kaybolacaktır!", "Veritabanı içe aktar!", MessageBoxButton.YesNo, MessageBoxImage.Question);

			try {
				if (ask == MessageBoxResult.Yes) {
				tryAgain:
					OpenFileDialog ofd = new();
					ofd.Title = "İçe aktarılacak veritabanını seçin!";
					ofd.Filter = "JSON dosyası (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
					ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
					ofd.AddExtension = true;
					ofd.CheckFileExists = true;
					ofd.ShowDialog();

					if (ofd.FileName == "") {
						var retry = MessageBox.Show("İçe aktarılacak veritabanı seçmediniz.\nTekrar denemek ister misiniz?", "Tekrar dene?", MessageBoxButton.YesNo, MessageBoxImage.Warning);

						if (retry == MessageBoxResult.Yes) goto tryAgain;
						else tooltipLabel.Content = "İçe aktarma iptal edildi!";
					}
					else {
						var db = Database.FromJson(ofd.FileName);
						Database.InternalDb = db;
						Database.Password = "password";
						Database.InternalDb.PwdHash = Sha256("password");
						entriesDataGrid.ItemsSource = Database.InternalDb.Entries;
						entriesDataGrid.Items.Refresh();

						var dbPath = Path.Combine(Path.GetDirectoryName(ofd.FileName), Path.GetFileNameWithoutExtension(ofd.FileName) + ".fdbx");
						Database.FilePath = dbPath;
						Database.Save();

						MessageBox.Show($"İçeri aktarılan veritabanı '{dbPath}' konumuna kaydedildi!\nVarsayılan şifre: 'password'", "Veritabanı kaydedildi!", MessageBoxButton.OK, MessageBoxImage.Information);
						tooltipLabel.Content = $"İçe aktarma başarılı!";
					}
				}

			} catch (Exception ex) { logger.Error(ex); }
		}
	}
}