using System;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core
{
	public partial class AppSettingsView : UserControl
	{
		public AppSettingsView()
		{
			InitializeComponent();
			UpdateSettingsInfo();
		}

		public void UpdateSettingsInfo()
		{
			var idb = Database.InternalDb;
			titleLabel.Text = $"Ayarlar - {idb.Name}";
			nameInput.Text = idb.Name;
			ownerInput.Text = idb.Owner;
		}

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPwdInput.Text = pwdInput.Password;

				pwdInput.Visibility = Visibility.Collapsed;
				clearPwdInput.Visibility = Visibility.Visible;
			}
			else {
				pwdInput.Password = clearPwdInput.Text;

				clearPwdInput.Visibility = Visibility.Collapsed;
				pwdInput.Visibility = Visibility.Visible;
			}
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				Database.Restore();
				UpdateSettingsInfo();

				pwdInput.Password = "";
				clearPwdInput.Text = "";
				tooltipLabel.Content = "Bilgiler yenilendi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) pwdInput.Password = clearPwdInput.Text;

				ref var idb = ref Database.InternalDb;
				idb.Name = string.IsNullOrEmpty(nameInput.Text) ? idb.Name : nameInput.Text;
				idb.Owner = string.IsNullOrEmpty(ownerInput.Text) ? idb.Owner : ownerInput.Text;
				idb.PwdHash = string.IsNullOrEmpty(pwdInput.Password) ? idb.PwdHash : Sha256(pwdInput.Password);
				Database.Password = string.IsNullOrEmpty(pwdInput.Password) ? Database.Password : pwdInput.Password;
				Database.Save();

				UpdateSettingsInfo();
				HomeWindow.UpdateHomeView();

				pwdInput.Password = "";
				clearPwdInput.Text = "";
				tooltipLabel.Content = "Ayarlar kaydedildi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SettingsControl_Loaded(object sender, RoutedEventArgs e) { nameInput.Focus(); }
	}
}