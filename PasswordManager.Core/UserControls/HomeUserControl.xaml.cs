using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using PasswordManager.Core.Classes;
using PasswordManager.Core.Models;

namespace PasswordManager.Core.UserControls
{
	public partial class HomeUserControl : UserControl
	{
		public HomeUserControl()
		{
			InitializeComponent();

			titleLabel.Text = $"Hoşgeldin, {DTO.InternalDB.owner}!";
			entriesDataGrid.ItemsSource = DTO.InternalDB.entries;
			updateDbInfo();
		}

		int[] RSL = { 1, 1, 1 };

		private void copyPwdButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)entriesDataGrid.SelectedItem;
			if (entry != null) {
				Clipboard.SetText(entry.password);
				tooltipLabel.Content = "Şifre panoya kopyalandı!";
			}
		}

		private void copyUsernameButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)entriesDataGrid.SelectedItem;
			if (entry != null) {
				Clipboard.SetText(entry.name);
				tooltipLabel.Content = "Kullanıcı adı panoya kopyalandı!";
			}
		}

		private void updateDbInfo()
		{
			titleLabel.Text = $"Hoşgeldin, {DTO.InternalDB.owner}!";

			var lmsd = DTO.InternalDB.last_modified.ToLocalTime().ToShortDateString();
			var lmst = DTO.InternalDB.last_modified.ToLocalTime().ToShortTimeString();
			var dbn = DTO.InternalDB.name;
			dbDataLabel.Content = $"DB: {dbn}, Son değişiklik: {lmsd} - {lmst}";

			entriesDataGrid.SelectedIndex = -1;
		}

		private void editButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) { entriesDataGrid.SelectedIndex = 0; }
			var oldEntry = (EntryModel)entriesDataGrid.SelectedItem;
			ManageDBWindow.edituc.idPreview.Text = oldEntry.guid.ToString();
			ManageDBWindow.edituc.namePreview.Text = oldEntry.name;
			ManageDBWindow.edituc.usernamePreview.Text = oldEntry.username;
			ManageDBWindow.edituc.pwdPreview.Password = oldEntry.password;
			ManageDBWindow.edituc.tagPreview.Text = oldEntry.tag;
			ManageDBWindow.edituc.nameInput.Text = oldEntry.name;
			ManageDBWindow.edituc.usernameInput.Text = oldEntry.username;
			ManageDBWindow.edituc.tagInput.Text = oldEntry.tag;

			var parent = (Canvas)Parent;
			parent.Children.Clear();
			parent.Children.Add(ManageDBWindow.edituc);
		}

		private void deleteButton_Click(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Bu kaydı silmek istediğinize emin misiniz?", "Kayıt silme işlemini onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (r == MessageBoxResult.Yes) {
				var entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
				DTO.InternalDB.entries.Remove(entry);

				ManageDBWindow.homeuc.entriesDataGrid.ItemsSource = new List<EntryModel>();
				ManageDBWindow.homeuc.entriesDataGrid.ItemsSource = DTO.InternalDB.entries;

				tooltipLabel.Content = "Kayıt silindi!";
			}
		}

		private void refreshButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult r;
			if (RSL[0] == 1) {
				r = MessageBox.Show("Dahili veritabanını yenilemek istediğinize emin misiniz?\nHarici veritabanına kaydetmediğiniz tüm değişiklikler kaybolacaktır!", "Dahili veritabanını yenile!", MessageBoxButton.YesNo, MessageBoxImage.Question);
			} else {
				r = MessageBoxResult.Yes;
			}

			if (r == MessageBoxResult.Yes) {
				var json = "";
				var dbm = new DatabaseModel();
				DbFunctions.ReadJson(DTO.FilePath, ref json);
				DbFunctions.JsonToDb(ref json, ref dbm);
				DTO.InternalDB = dbm;

				if (DTO.InternalDB.entries != null) {
					var ueList = new List<EntryModel>();
					DbFunctions.DecryptEntries(DTO.InternalDB.entries, ref ueList);
					DTO.InternalDB.entries = ueList;
				}

				entriesDataGrid.ItemsSource = DTO.InternalDB.entries;
				updateDbInfo();
				tooltipLabel.Content = "Veritabanı yenilendi!";

				entriesDataGrid.SelectedIndex = -1;
			}

			int[] def = { 1, 1, 1 };
			RSL = def;
		}

		public void forceRefresh()
		{
			object s = null;
			RoutedEventArgs e = null;
			RSL[0] = 0;
			refreshButton_Click(s, e);
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
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

					List<EntryModel> ueList = (List<EntryModel>)entriesDataGrid.ItemsSource;
					List<EntryModel> eList = new();

					if (ueList != null) {
						DbFunctions.EncryptEntries(ref ueList, ref eList);
						DTO.InternalDB.entries = eList;
					}
					DTO.InternalDB.last_modified = DateTime.UtcNow;

					string json = "";
					DbFunctions.DbToJson(DTO.InternalDB, ref json);
					DbFunctions.WriteJson(ref json, DTO.FilePath);

					refreshButton_Click(sender, e);

					tooltipLabel.Content = "Veritabanı kaydedildi!";
				} else {
					RSL[0] = 1;
				}
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}

		private void lockButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				var r = MessageBox.Show("Veritabanını kilitlemek istediğinize emin misiniz?\nKaydedilmemiş tüm değişiklikler kaydedilip harici veritabanına yazılacaktır!", "Veritabanını kaydet ve kilitle!", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (r == MessageBoxResult.Yes) {
					RSL[1] = 0;
					saveButton_Click(sender, e);

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
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}

		private void optionsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) { entriesDataGrid.SelectedIndex = 0; }

			var canvas = (Canvas)Parent;
			var ouc = new OptionsUserControl();
			canvas.Children.Add(ouc);
		}
	}
}