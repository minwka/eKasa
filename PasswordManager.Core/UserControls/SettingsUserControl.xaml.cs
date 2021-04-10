using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.Core.UserControls
{
	public partial class SettingsUserControl : UserControl
	{
		public SettingsUserControl()
		{
			InitializeComponent();

			ref var idb = ref Settings.dbSettings.InternalDb;
			titleLabel.Text = $"Ayarlar - {idb.Name}";
			nameTextBox.Text = idb.Name;
			usernameTextBox.Text = idb.Owner;
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

		private void reloadButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				ref var idb = ref Settings.dbSettings.InternalDb;
				idb = Database.FromJson(Settings.dbSettings.Path);
				idb.Entries = Database.DecryptEntries(ref idb);
				// idb = Database.DecryptAttributes(ref idb);
				idb.Owner = AES.Decrypt(idb.Owner, Settings.dbSettings.Password);
				idb.Name = AES.Decrypt(idb.Name, Settings.dbSettings.Password);
				idb.ModifiedDate = AES.Decrypt(idb.ModifiedDate, Settings.dbSettings.Password);

				titleLabel.Text = $"Ayarlar - {idb.Name}";
				nameTextBox.Text = idb.Name;
				usernameTextBox.Text = idb.Owner;

				tooltipLabel.Content = "Ayarlar kaydedildi!";
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}

		private void saveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					pwdInput.Password = clearPwdInput.Text;
				}

				ref var idb = ref Settings.dbSettings.InternalDb;
				idb.Name = AES.Encrypt(nameTextBox.Text, Settings.dbSettings.Password);
				idb.Owner = AES.Encrypt(usernameTextBox.Text, Settings.dbSettings.Password);
				idb.PwdHash = pwdInput.Password == "" ? idb.PwdHash : AES.Hash(pwdInput.Password);
				idb.Entries = Database.EncryptEntries(ref idb);
				// idb = Database.EncryptAttributes(ref idb);
				idb.ModifiedDate = AES.Encrypt(DateTime.UtcNow.ToString(), Settings.dbSettings.Password);

				File.WriteAllText(Settings.dbSettings.Path, Database.ToJson(ref idb));

				ManageDbWindow.homeuc.forceRefresh();
				reloadButton_Click(sender, e);
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}
	}
}