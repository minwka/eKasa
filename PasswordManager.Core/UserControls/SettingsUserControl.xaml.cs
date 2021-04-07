using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordManager.Core.Classes;
using PasswordManager.Core.Models;

namespace PasswordManager.Core.UserControls
{
	public partial class SettingsUserControl : UserControl
	{
		public SettingsUserControl()
		{
			InitializeComponent();

			titleLabel.Text = $"Ayarlar - {DTO.InternalDB.name}";
			nameTextBox.Text = DTO.InternalDB.name;
			usernameTextBox.Text = DTO.InternalDB.owner;
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					pwdInput.Password = clearPwdInput.Text;
				}

				var idb = DTO.InternalDB;
				idb.name = nameTextBox.Text;
				idb.owner = usernameTextBox.Text;
				idb.pwd_hash = pwdInput.Password == "" ? idb.pwd_hash : Hash.ComputeSha256(pwdInput.Password);

				List<EntryModel> ueList = (List<EntryModel>)ManageDBWindow.homeuc.entriesDataGrid.ItemsSource;
				List<EntryModel> eList = new();

				if (ueList != null) {
					DbFunctions.EncryptEntries(ref ueList, ref eList);
					DTO.InternalDB.entries = eList;
				}

				string json = "";
				DbFunctions.DbToJson(idb, ref json);
				DbFunctions.WriteJson(ref json, DTO.FilePath);

				ManageDBWindow.homeuc.forceRefresh();
				refreshButton_Click(sender, e);
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}

		private void refreshButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				string json = "";
				var jdb = new DatabaseModel();

				DbFunctions.ReadJson(DTO.FilePath, ref json);
				DbFunctions.JsonToDb(ref json, ref jdb);
				DTO.InternalDB = jdb;

				titleLabel.Text = $"Ayarlar - {DTO.InternalDB.name}";
				nameTextBox.Text = DTO.InternalDB.name;
				usernameTextBox.Text = DTO.InternalDB.owner;

				var color = new SolidColorBrush(Color.FromRgb(20, 120, 215));
				tooltipLabel.Foreground = color;
				tooltipLabel.Content = "Ayarlar kaydedildi!";
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}

		private void pwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPwdInput.Text = pwdInput.Password;

				pwdInput.Visibility = Visibility.Collapsed;
				clearPwdInput.Visibility = Visibility.Visible;
			} else {
				pwdInput.Password = clearPwdInput.Text;

				clearPwdInput.Visibility = Visibility.Collapsed;
				pwdInput.Visibility = Visibility.Visible;
			}
		}
	}
}