using System;
using System.IO;
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
				idb = Database.FromJson(dbSettings.Path);
				idb = Database.DecryptDatabase(ref idb);

				titleLabel.Text = $"Ayarlar - {idb.Name}";
				nameInput.Text = idb.Name;
				ownerInput.Text = idb.Owner;
				tooltipLabel.Content = "Ayarlar kaydedildi!";
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

				idb = Database.EncryptDatabase(ref idb, pwdInput.Password);
				File.WriteAllText(dbSettings.Path, Database.ToJson(ref idb));

				ManageDbWindow.homeuc.ForceRefresh();
				ReloadButton_Click(sender, e);
			} catch (Exception ex) { logger.Error(ex); }
		}
	}
}