﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using eKasa.Library.Config;
using Microsoft.Win32;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core
{
	public partial class LoginWindow : Window
	{
		readonly static public OpenFileDialog ofd = new();
		public LoginWindow()
		{
			InitializeComponent();
			SettingsManager<AppSettingsModel>.Restore(ref appSettings, appSettingsPath);
			if (appSettings.RememberLastDb) {
				ofd.FileName = appSettings.LastDbLocation;
				locationInput.Text = Path.GetFileName(appSettings.LastDbLocation);
				rememberDb.IsChecked = true;
				passwordInput.Focus();
			}
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{ if (e.ChangedButton == MouseButton.Left) DragMove(); }

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			CreateDbWindow cdb = new();
			cdb.ShowDialog();
			if (!string.IsNullOrEmpty(cdb.ofd.FileName)) {
				ofd.FileName = cdb.ofd.FileName;
				locationInput.Text = ofd.SafeFileName;
				rememberDb.IsChecked = true;
				passwordInput.Password = cdb.passwordInput.Password;
				ConfirmButton_Click(sender, e);
			}
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow hw = new();
			hw.Show();
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }

		private void PickerButton_Click(object sender, RoutedEventArgs e)
		{
			ofd.Title = "Bir veritabanı dosyası seçin";
			ofd.Filter = "Veritabanı dosyaları (*fdbx)|*.fdbx|JSON dosyaları (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
			if (ofd.ShowDialog() == true)
				locationInput.Text = ofd.SafeFileName;
			passwordInput.Focus();
		}

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
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

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) { passwordInput.Password = clearPwdInput.Text; }

				dbSettings.FilePath = ofd.FileName;
				dbSettings.Password = passwordInput.Password;

				ref var idb = ref dbSettings.InternalDb;
				idb = Database.FromJson(ofd.FileName);

				if (idb.PwdHash == Sha256(passwordInput.Password)) {
					idb = Database.DecryptDatabase(ref idb);

					if (rememberDb.IsChecked == true) {
						appSettings.RememberLastDb = true;
						appSettings.LastDbLocation = ofd.FileName;
					} else {
						appSettings.RememberLastDb = false;
					}
					SettingsManager<AppSettingsModel>.Save(appSettings, appSettingsPath);

					HomeWindow mdbw = new();
					mdbw.Show();
					Close();
				} else {
					MessageBox.Show("Yanlış ya da boş şifre veya geçersiz veritabanı dosyası belirttiniz!", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
					passwordInput.Password = "";
					clearPwdInput.Text = "";
				}
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (rememberDb.IsChecked == true) appSettings.RememberLastDb = true;
			else appSettings.RememberLastDb = false;
			SettingsManager<AppSettingsModel>.Save(appSettings, appSettingsPath);
		}
	}
}