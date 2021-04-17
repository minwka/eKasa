using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core.UserControls
{
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();

			titleLabel.Text = $"Hoşgeldin, {dbSettings.InternalDb.Owner}!";
			entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
			UpdateDbHint();
		}

		public void UpdateDbHint()
		{
			var idb = dbSettings.InternalDb;

			titleLabel.Text = $"Hoşgeldin, {idb.Owner}!";
			var modifiedDate = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortDateString();
			var modifiedTime = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortTimeString();
			dbDataLabel.Content = $"DB: {idb.Name}, Son değişiklik: {modifiedDate} - {modifiedTime}";

			entriesDataGrid.SelectedIndex = -1;
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
			var oldEntry = (EntryModel)entriesDataGrid.SelectedItem;
			if (oldEntry != null) {
				HomeWindow.editv.idPreview.Text = oldEntry.Id.ToString();
				HomeWindow.editv.namePreview.Text = oldEntry.Name;
				HomeWindow.editv.usernamePreview.Text = oldEntry.Username;
				HomeWindow.editv.pwdPreview.Password = oldEntry.Password;
				HomeWindow.editv.tagPreview.Text = oldEntry.Tag;
				HomeWindow.editv.nameInput.Text = oldEntry.Name;
				HomeWindow.editv.usernameInput.Text = oldEntry.Username;
				HomeWindow.editv.tagInput.Text = oldEntry.Tag;

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(HomeWindow.editv);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;

			dbSettings.InternalDb.Entries.Remove((EntryModel)entriesDataGrid.SelectedItem);
			entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
			entriesDataGrid.Items.Refresh();

			tooltipLabel.Content = "Kayıt silindi!";
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				Database.Restore(ref dbSettings.InternalDb, dbSettings.FilePath);
				entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
				entriesDataGrid.Items.Refresh();

				UpdateDbHint();
				entriesDataGrid.SelectedIndex = -1;
				tooltipLabel.Content = "Veritabanı yenilendi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				dbSettings.InternalDb.Entries = (List<EntryModel>)entriesDataGrid.ItemsSource;
				dbSettings.InternalDb.ModifiedDate = DateTime.UtcNow.ToString();
				Database.Save(dbSettings.InternalDb, dbSettings.FilePath);

				HomeWindow.UpdateAllViews();
				tooltipLabel.Content = "Veritabanı kaydedildi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void LockButton_Click(object sender, RoutedEventArgs e)
		{
			Process.Start(Process.GetCurrentProcess().MainModule.FileName);
			Application.Current.Shutdown();
		}

		private void ExportButton_Click(object sender, RoutedEventArgs e)
		{
			DatabaseModel edb = new();
			edb = dbSettings.InternalDb;
			edb.PwdHash = "";
			edb.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			try {
				var ask = MessageBox.Show("Veritabanını ŞİFRESİZ olarak dışa aktarmak istediğinize emin misiniz?", "Veritabanını dışa aktar!", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
						var retry = MessageBox.Show("Dışa aktarmayı iptal ettiniz.\nTekrar denemek ister misiniz?", "Tekrar dene?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
						if (retry == MessageBoxResult.Yes) goto tryAgain;
						else tooltipLabel.Content = "Dışa aktarma iptal edildi!";
					} else {
						Database.ToJson(ref dbSettings.InternalDb, ofd.FileName);
						tooltipLabel.Content = "Dışa aktarma başarılı!";
					}
				}
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void ImportButton_Click(object sender, RoutedEventArgs e)
		{
			try {
			tryAgain:
				OpenFileDialog ofd = new();
				ofd.Title = "İçe aktarılacak veritabanını seçin!";
				ofd.Filter = "JSON dosyası (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
				ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				ofd.AddExtension = true;
				ofd.CheckFileExists = true;
				ofd.ShowDialog();
				if (ofd.FileName == "") {
					var retry = MessageBox.Show("İçe aktarmayı iptal ettiniz.\nTekrar denemek ister misiniz?", "Tekrar dene?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (retry == MessageBoxResult.Yes) goto tryAgain;
					else tooltipLabel.Content = "İçe aktarma iptal edildi!";
				} else {
					var db = Database.FromJson(ofd.FileName);
					dbSettings.InternalDb = db;
					dbSettings.Password = "password";
					dbSettings.InternalDb.PwdHash = Sha256("password");
					entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
					entriesDataGrid.Items.Refresh();

					var dbPath = Path.Combine(Path.GetDirectoryName(ofd.FileName), Path.GetFileNameWithoutExtension(ofd.FileName) + ".fdbx");
					dbSettings.FilePath = dbPath;
					Database.Save(db, dbPath);
					MessageBox.Show($"İçeri aktarılan veritabanı '{dbPath}' konumuna kaydedildi!\nVarsayılan şifre: 'password'", "Veritabanı kaydedildi!", MessageBoxButton.OK, MessageBoxImage.Information);
					tooltipLabel.Content = $"İçe aktarma başarılı!";
				}

			} catch (Exception ex) { logger.Error(ex); }
		}
	}
}