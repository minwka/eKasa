using System;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core {
	public partial class AppSettingsView : UserControl {
		public AppSettingsView() {
			InitializeComponent();
			UpdateSettingsInfo();
		}

		public void UpdateSettingsInfo() {
			var idb = dbSettings.InternalDb;
			titleLabel.Text = $"Ayarlar - {idb.Name}";
			nameInput.Text = idb.Name;
			ownerInput.Text = idb.Owner;
		}

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e) {
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

		private void ReloadButton_Click(object sender, RoutedEventArgs e) {
			try {
				ref var idb = ref dbSettings.InternalDb;
				Database.Restore(ref idb, dbSettings.FilePath);

				UpdateSettingsInfo();
				pwdInput.Password = "";
				clearPwdInput.Text = "";
				tooltipLabel.Content = "Bilgiler yenilendi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e) {
			try {
				if (pwdToggle.IsChecked == true) pwdInput.Password = clearPwdInput.Text;

				ref var idb = ref dbSettings.InternalDb;
				idb.Name = nameInput.Text;
				idb.Owner = ownerInput.Text;
				idb.PwdHash = pwdInput.Password != "" ? Sha256(pwdInput.Password) : idb.PwdHash;
				dbSettings.Password = pwdInput.Password != "" ? pwdInput.Password : dbSettings.Password;
				Database.Save(idb, dbSettings.FilePath);

				UpdateSettingsInfo();
				pwdInput.Password = "";
				clearPwdInput.Text = "";
				HomeWindow.UpdateHomeView();
				tooltipLabel.Content = "Ayarlar kaydedildi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SettingsControl_Loaded(object sender, RoutedEventArgs e) { nameInput.Focus(); }
	}
}