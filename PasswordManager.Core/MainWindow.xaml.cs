using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PasswordManager.Library.Config;

namespace PasswordManager.Core
{
	public partial class MainWindow : Window
	{
		readonly OpenFileDialog ofd = new();
		public MainWindow()
		{
			InitializeComponent();

			SettingsManager<AppSettingsModel>.Restore(@"Settings\settings.xml", ref Settings.appSettings);
			ofd.FileName = Settings.appSettings.LastDbLocation;
			locationInput.Text = Path.GetFileName(Settings.appSettings.LastDbLocation);
			passwordInput.Focus();
		}

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			CreateDbWindow cdb = new();
			cdb.ShowDialog();
		}

		private void helpButton_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow hw = new();
			hw.Show();
		}

		private void terminateButton_Click(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }

		private void pickerButton_Click(object sender, RoutedEventArgs e)
		{
			ofd.Title = "Bir veritabanı dosyası seçin";
			ofd.Filter = "Veritabanı dosyaları (*fdbx)|*.fdbx|JSON dosyaları (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
			if (ofd.ShowDialog() == true)
				locationInput.Text = ofd.SafeFileName;

			passwordInput.Focus();
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

		private void confirmButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) { passwordInput.Password = clearPwdInput.Text; }

				ref var idb = ref Settings.dbSettings.InternalDb;
				idb = Database.FromJson(ofd.FileName);

				if (idb.PwdHash == AES.Hash(passwordInput.Password)) {
					Settings.dbSettings.Path = ofd.FileName;
					Settings.dbSettings.Password = passwordInput.Password;

					idb.Entries = Database.DecryptEntries(ref idb);
					// idb = Database.DecryptAttributes(ref idb);
					idb.Owner = AES.Decrypt(idb.Owner, Settings.dbSettings.Password);
					idb.Name = AES.Decrypt(idb.Name, Settings.dbSettings.Password);
					idb.ModifiedDate = AES.Decrypt(idb.ModifiedDate, passwordInput.Password);

					if (rememberDb.IsChecked == true) {
						Settings.appSettings.LastDbLocation = ofd.FileName;
						SettingsManager<AppSettingsModel>.Save(@"Settings\settings.xml", ref Settings.appSettings);
					}

					ManageDbWindow mdbw = new();
					mdbw.Show();
					Hide();
				} else {
					MessageBox.Show("Yanlış ya da boş şifre veya geçersiz veritabanı dosyası belirttiniz!", "Hata!", MessageBoxButton.OK, MessageBoxImage.Warning);
				}
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("error.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}
	}
}