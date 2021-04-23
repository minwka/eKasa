using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core {
	public partial class NewDbWindow : Window {
		public bool dbCreated = false;
		readonly public OpenFileDialog ofd = new();
		public NewDbWindow() { InitializeComponent(); }

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) DragMove(); }

		private void TerminateButton_Click(object sender, RoutedEventArgs e) { Close(); }

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e) {
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

		private void PickerButton_Click(object sender, RoutedEventArgs e) {
			ofd.Title = "Yeni veritabanını kaydedecek konum seçin";
			ofd.Filter = "Veritabanı dosyaları (*fdbx)|*.fdbx|JSON dosyaları (*.json)|*.json|Tüm dosyalar (*.*)|*.*";
			ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			ofd.AddExtension = true;
			ofd.CheckFileExists = false;
			if (ofd.ShowDialog() == true) {
				directoryPreview.Text = ofd.SafeFileName;
				directoryPreview.ToolTip = ofd.FileName;
			}
			nameInput.Focus();
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e) {
			try {
				if (pwdToggle.IsChecked == true) passwordInput.Password = clearPwdInput.Text;
				GlobalSettings.dbSettings.Password = passwordInput.Password;

				DatabaseModel dbm = new() {
					Name = string.IsNullOrEmpty(nameInput.Text) ? "FDBX" : nameInput.Text,
					Owner = string.IsNullOrEmpty(ownerInput.Text) ? "Kullanıcı" : ownerInput.Text,
					PwdHash = Sha256(passwordInput.Password),
					ModifiedDate = DateTime.UtcNow.ToString(),
				};

				Database.Save(dbm, ofd.FileName);

				dbCreated = true;
				MessageBox.Show("Dosya başarıyla oluşturuldu!", "Bildirim!", MessageBoxButton.OK, MessageBoxImage.Information);

				Close();
			} catch (Exception ex) {
				GlobalSettings.logger.Error(ex, "Dosya oluşturulurken bir hata meydana geldi.\nLütfen tekrar deneyin.", "Hata!");
			}
		}
	}
}