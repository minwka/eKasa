using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace PasswordManager.Core
{
	public partial class CreateDbWindow : Window
	{
		readonly public OpenFileDialog ofd = new();
		public CreateDbWindow()
		{ InitializeComponent(); }

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{ Close(); }

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

		private void PickerButton_Click(object sender, RoutedEventArgs e)
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

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					passwordInput.Password = clearPwdInput.Text;
				}

				DatabaseModel dbm = new() {
					Name = string.IsNullOrEmpty(nameInput.Text) ? AES.Encrypt("FDBX", passwordInput.Password) : AES.Encrypt(nameInput.Text, passwordInput.Password),
					Owner = string.IsNullOrEmpty(ownerInput.Text) ? AES.Encrypt("Kullanıcı", passwordInput.Password) : AES.Encrypt(ownerInput.Text, passwordInput.Password),
					PwdHash = AES.Hash(passwordInput.Password),
					ModifiedDate = AES.Encrypt(DateTime.UtcNow.ToString(), passwordInput.Password),
					Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
					Salt = Convert.ToBase64String(AES.sessionSalt)
				};

				File.WriteAllText(ofd.FileName, Database.ToJson(ref dbm));

				MessageBox.Show("Dosya başarıyla oluşturuldu!", "Bildirim!", MessageBoxButton.OK, MessageBoxImage.Information);

				Close();
			} catch (Exception ex) {
				MessageBox.Show("Dosya oluşturulurken bir hata meydana geldi.\nLütfen tekrar deneyin.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("error.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}
	}
}