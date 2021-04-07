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
	public partial class CreateDBWindow : Window
	{
		OpenFileDialog ofd = new OpenFileDialog();
		public CreateDBWindow()
		{ InitializeComponent(); }

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void pickerButton_Click(object sender, RoutedEventArgs e)
		{
			ofd.Title = "Yeni veritabanını kaydedecek konum seçin";
			ofd.Filter = "Veritabanı dosyaları (*fdbx)|*.fdbx|JSON dosyaları (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
			ofd.AddExtension = true;
			ofd.CheckFileExists = false;
			if (ofd.ShowDialog() == true) {
				directoryPreview.Text = ofd.SafeFileName;
				directoryPreview.ToolTip = ofd.FileName;
			}
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					passwordInput.Password = clearPwdInput.Text;
				}

				using (File.Create(ofd.FileName)) { };
				DatabaseModel dbm = new() {
					name = nameInput.Text.Length == 0 ? "FDBX" : nameInput.Text,
					owner = "Kullanıcı",
					pwd_hash = Hash.ComputeSha256(passwordInput.Password),
					entries = new List<EntryModel>(),
					last_modified = DateTime.UtcNow
				};

				string json = "";
				DbFunctions.DbToJson(dbm, ref json);
				DbFunctions.WriteJson(ref json, ofd.FileName);

				MessageBox.Show("Dosya başarıyla oluşturuldu!", "Bildirim!", MessageBoxButton.OK, MessageBoxImage.Information);

				Close();
			} catch (Exception ex) {
				MessageBox.Show("Dosya oluşturulurken bir hata meydana geldi.\nLütfen tekrar deneyin.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
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

		private void terminateButton_Click(object sender, RoutedEventArgs e)
		{ Close(); }
	}
}