using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PasswordManager.Core.Classes;
using PasswordManager.Core.Models;

namespace PasswordManager.Core
{
	public partial class MainWindow : Window
	{
		readonly OpenFileDialog ofd = new();
		public MainWindow()
		{ InitializeComponent(); }

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			CreateDBWindow cdb = new CreateDBWindow();
			cdb.ShowDialog();
		}

		private void pickerButton_Click(object sender, RoutedEventArgs e)
		{
			ofd.Title = "Bir veritabanı dosyası seçin";
			ofd.Filter = "Veritabanı dosyaları (*fdbx)|*.fdbx|JSON dosyaları (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
			if (ofd.ShowDialog() == true)
				locationInput.Text = ofd.SafeFileName;
		}

		private void confirmButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					passwordInput.Password = clearPwdInput.Text;
				}

				string json = "";
				var dbm = new DatabaseModel();

				DbFunctions.ReadJson(ofd.FileName, ref json);
				DbFunctions.JsonToDb(ref json, ref dbm);
				DTO.InternalDB = dbm;

				if (DTO.InternalDB.pwd_hash == Hash.ComputeSha256(passwordInput.Password)) {
					DTO.FilePath = ofd.FileName;
					DTO.Password = passwordInput.Password;

					if (DTO.InternalDB.entries != null) {
						var ueList = new List<EntryModel>();
						DbFunctions.DecryptEntries(DTO.InternalDB.entries, ref ueList);
						DTO.InternalDB.entries = ueList;
					} else if (DTO.InternalDB.entries == null) {
						var ueList = new List<EntryModel>();
						DTO.InternalDB.entries = ueList;
					}

					ManageDBWindow mdbw = new();
					mdbw.Show();
					Hide();
				} else if (passwordInput.Password == "") {
					MessageBox.Show("Şifre boş olamaz!", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
				} else {
					MessageBox.Show("Yanlış şifre veya geçersiz veritabanı dosyası belirttiniz!", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}

		private void pwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPwdInput.Text = passwordInput.Password;

				passwordInput.Visibility = Visibility.Collapsed;
				clearPwdInput.Visibility = Visibility.Visible;
			} else {
				passwordInput.Password = clearPwdInput.Text;

				clearPwdInput.Visibility = Visibility.Collapsed;
				passwordInput.Visibility = Visibility.Visible;
			}
		}

		private void helpButton_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow hw = new();
			hw.Show();
		}

		private void terminateButton_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}