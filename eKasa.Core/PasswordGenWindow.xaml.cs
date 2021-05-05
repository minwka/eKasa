using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace eKasa.Core
{
	public partial class PasswordGenWindow : Window
	{
		public PasswordGenWindow() { InitializeComponent(); }

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) DragMove(); }

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPasswordInput.Text = passwordInput.Password;

				passwordInput.Visibility = Visibility.Collapsed;
				clearPasswordInput.Visibility = Visibility.Visible;
			}
			else {
				passwordInput.Password = clearPasswordInput.Text;

				passwordInput.Visibility = Visibility.Visible;
				clearPasswordInput.Visibility = Visibility.Collapsed;
			}
		}

		private void PwdLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (pwdLength != null) {
				pwdLength.Content = $"{(int)pwdLengthSlider.Value}";
			}
			GenPwdButton_Click(new object(), new RoutedPropertyChangedEventArgs<double>(0, 0));
		}

		string randomString = "";
		private void GenPwdButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
				const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
				const string digitChars = "0123456789";
				const string symbolChars = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
				string charset = "";

				charset += uppercaseLetters.IsChecked == true ? uppercaseChars : "";
				charset += lowercaseLetters.IsChecked == true ? lowercaseChars : "";
				charset += digits.IsChecked == true ? digitChars : "";
				charset += symbols.IsChecked == true ? symbolChars : "";

				var random = new Random();
				randomString = new string(Enumerable.Repeat(charset, (int)pwdLengthSlider.Value).Select(s => s[random.Next(s.Length)]).ToArray());

				if (IsInitialized) {
					clearPasswordInput.Text = randomString;
					passwordInput.Password = randomString;
				}
			} catch (Exception ex) {
				MessageBox.Show($"Lütfen en az bir karakter seti seçin!\n{ex.Message}", "Hatalı Karakter Seti!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void SavePwdButton_Click(object sender, RoutedEventArgs e)
		{
			HomeWindow.addView.clearPasswordInput.Text = randomString;
			HomeWindow.editView.clearPasswordInput.Text = randomString;
			HomeWindow.addView.passwordInput.Password = randomString;
			HomeWindow.editView.passwordInput.Password = randomString;
			Close();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e) { Close(); }
	}
}