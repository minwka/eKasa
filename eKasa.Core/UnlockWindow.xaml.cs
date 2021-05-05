using eKasa.Library.Config;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core
{
	public partial class UnlockWindow : Window
	{
		readonly static public OpenFileDialog ofd = new();
		public UnlockWindow()
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

		private void ConfirmButton_Click(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) passwordInput.Password = clearPwdInput.Text;

			try {
				if (Database.FromJson(ofd.FileName).PwdHash == Sha256(passwordInput.Password)) {
					Database.FilePath = ofd.FileName;
					Database.Password = passwordInput.Password;
					Database.Restore();

					HomeWindow mdbw = new();
					mdbw.Show();
					Close();
				}
				else {
					MessageBox.Show("Yanlış ya da boş şifre veya geçersiz veritabanı dosyası belirttiniz!", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
					passwordInput.Password = "";
					clearPwdInput.Text = "";
				}
			} catch (Exception ex) { logger.Error(ex); }
		}

		#region Window Actions
		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			NewDbWindow cdb = new();
			cdb.ShowDialog();
			if (cdb.dbCreated) {
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

		private void PickerButton_Click(object sender, RoutedEventArgs e)
		{
			ofd.Title = "Bir veritabanı dosyası seçin";
			ofd.Filter = "Veritabanı dosyaları (*fdbx)|*.fdbx|JSON dosyaları (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
			ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

			if (ofd.ShowDialog() == true) locationInput.Text = ofd.SafeFileName;
			passwordInput.Focus();
		}

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPwdInput.Text = passwordInput.Password;

				passwordInput.Visibility = Visibility.Collapsed;
				clearPwdInput.Visibility = Visibility.Visible;
			}
			else {
				passwordInput.Password = clearPwdInput.Text;

				clearPwdInput.Visibility = Visibility.Collapsed;
				passwordInput.Visibility = Visibility.Visible;
			}
		}

		private void UnlockWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{ if (e.ChangedButton == MouseButton.Left) DragMove(); }

		private void UnlockWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (rememberDb.IsChecked == true) { appSettings.RememberLastDb = true; }
			else { appSettings.RememberLastDb = false; }

			appSettings.LastDbLocation = ofd.FileName;

			SettingsManager<AppSettingsModel>.Save(appSettings, appSettingsPath);
		}

		private void Terminate(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }

		#endregion
	}
}