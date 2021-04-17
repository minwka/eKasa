using System;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.Settings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core.UserControls
{
	public partial class SettingsUserControl : UserControl
	{
		public SettingsUserControl()
		{
			InitializeComponent();

			ref var idb = ref dbSettings.InternalDb;
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
			} else {
				pwdInput.Password = clearPwdInput.Text;

				clearPwdInput.Visibility = Visibility.Collapsed;
				pwdInput.Visibility = Visibility.Visible;
			}
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				ref var idb = ref dbSettings.InternalDb;
				Database.Restore(ref idb, dbSettings.FilePath);

				titleLabel.Text = $"Ayarlar - {idb.Name}";
				nameInput.Text = idb.Name;
				ownerInput.Text = idb.Owner;

				pwdInput.Password = "";
				clearPwdInput.Text = "";
				tooltipLabel.Content = "Bilgiler yenilendi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) pwdInput.Password = clearPwdInput.Text;
				ref var idb = ref dbSettings.InternalDb;
				idb.Name = nameInput.Text;
				idb.Owner = ownerInput.Text;
				idb.PwdHash = pwdInput.Password != "" ? Hash(pwdInput.Password) : idb.PwdHash;
				dbSettings.Password = pwdInput.Password != "" ? pwdInput.Password : dbSettings.Password;
				pwdInput.Password = "";
				clearPwdInput.Text = "";
				Database.Save(idb, dbSettings.FilePath);
				tooltipLabel.Content = "Ayarlar kaydedildi!";
			} catch (Exception ex) { logger.Error(ex); }
		}
	}
}