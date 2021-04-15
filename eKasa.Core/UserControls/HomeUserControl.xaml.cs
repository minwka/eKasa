using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.Settings;

namespace eKasa.Core.UserControls
{
	public partial class HomeUserControl : UserControl
	{
		public HomeUserControl()
		{
			InitializeComponent();

			titleLabel.Text = $"Hoşgeldin, {dbSettings.InternalDb.Owner}!";
			entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
			UpdateDbInfo();
		}

		int[] RSL = { 1, 1, 1 };

		private void UpdateDbInfo()
		{
			ref var idb = ref dbSettings.InternalDb;
			titleLabel.Text = $"Hoşgeldin, {idb.Owner}!";

			var lmsd = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortDateString();
			var lmst = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortTimeString();
			var dbn = idb.Name;
			dbDataLabel.Content = $"DB: {dbn}, Son değişiklik: {lmsd} - {lmst}";

			entriesDataGrid.SelectedIndex = -1;
		}

		private void OptionsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) { entriesDataGrid.SelectedIndex = 0; }

			var canvas = (Canvas)Parent;
			var ouc = new OptionsUserControl();
			canvas.Children.Add(ouc);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) { entriesDataGrid.SelectedIndex = 0; }
			var oldEntry = (EntryModel)entriesDataGrid.SelectedItem;
			if (oldEntry != null) {
				ManageDbWindow.edituc.idPreview.Text = oldEntry.Id.ToString();
				ManageDbWindow.edituc.namePreview.Text = oldEntry.Name;
				ManageDbWindow.edituc.usernamePreview.Text = oldEntry.Username;
				ManageDbWindow.edituc.pwdPreview.Password = oldEntry.Password;
				ManageDbWindow.edituc.tagPreview.Text = oldEntry.Tag;
				ManageDbWindow.edituc.nameInput.Text = oldEntry.Name;
				ManageDbWindow.edituc.usernameInput.Text = oldEntry.Username;
				ManageDbWindow.edituc.tagInput.Text = oldEntry.Tag;

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(ManageDbWindow.edituc);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) { entriesDataGrid.SelectedIndex = 0; }

			var r = MessageBox.Show("Bu kaydı silmek istediğinize emin misiniz?", "Kayıt silme işlemini onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (r == MessageBoxResult.Yes) {
				var entry = (EntryModel)ManageDbWindow.homeuc.entriesDataGrid.SelectedItem;
				dbSettings.InternalDb.Entries.Remove(entry);

				ManageDbWindow.homeuc.entriesDataGrid.ItemsSource = new List<EntryModel>();
				ManageDbWindow.homeuc.entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;

				tooltipLabel.Content = "Kayıt silindi!";
			}
		}

		public void ForceRefresh()
		{
			object s = null;
			RoutedEventArgs e = null;
			RSL[0] = 0;
			ReloadButton_Click(s, e);
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				MessageBoxResult r;
				if (RSL[0] == 1) {
					r = MessageBox.Show("Dahili veritabanını yenilemek istediğinize emin misiniz?\nHarici veritabanına kaydetmediğiniz tüm değişiklikler kaybolacaktır!", "Dahili veritabanını yenile!", MessageBoxButton.YesNo, MessageBoxImage.Question);
				} else {
					r = MessageBoxResult.Yes;
				}

				if (r == MessageBoxResult.Yes) {
					ref var idb = ref dbSettings.InternalDb;
					idb = Database.FromJson(dbSettings.Path);
					idb = Database.DecryptDatabase(ref idb);

					entriesDataGrid.ItemsSource = idb.Entries;
					UpdateDbInfo();
					tooltipLabel.Content = "Veritabanı yenilendi!";
					entriesDataGrid.SelectedIndex = -1;
				}

				int[] def = { 1, 1, 1 };
				RSL = def;
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				MessageBoxResult r;
				if (RSL[1] == 1) {
					r = MessageBox.Show("Veritabanını kaydetmek istediğinize emin misiniz?\nTüm değişiklikler eski veritabanının üstüne yazılacaktır!", "Veritabanını Kaydet!", MessageBoxButton.YesNo, MessageBoxImage.Question);
				} else {
					r = MessageBoxResult.Yes;
				}

				if (r == MessageBoxResult.Yes) {
					RSL[0] = 0;

					var db = Database.EncryptDatabase(ref dbSettings.InternalDb, dbSettings.Password);
					File.WriteAllText(dbSettings.Path, Database.ToJson(ref db));

					ReloadButton_Click(sender, e);
					tooltipLabel.Content = "Veritabanı kaydedildi!";
				} else {
					RSL[0] = 1;
				}
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void LockButton_Click(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Veritabanını kilitlemek istediğinize emin misiniz?\nKaydedilmemiş tüm değişiklikler kaydedilip harici veritabanına yazılacaktır!", "Veritabanını kaydet ve kilitle!", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (r == MessageBoxResult.Yes) {
				RSL[1] = 0;
				SaveButton_Click(sender, e);

				var canvas = (Canvas)Parent;
				var grid = (Grid)(canvas.Parent);
				var window = (Window)(grid.Parent);

				canvas.Children.Clear();
				window.Close();

				MainWindow w = new();
				w.Show();
			} else {
				RSL[1] = 1;
			}
		}
	}
}