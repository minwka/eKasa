using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PasswordManager.Core
{
	public partial class PasswordGenWindow : Window
	{
		public PasswordGenWindow()
		{ InitializeComponent(); }

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void pwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPasswordInput.Text = passwordInput.Password;

				passwordInput.Visibility = Visibility.Collapsed;
				clearPasswordInput.Visibility = Visibility.Visible;
			} else {
				passwordInput.Password = clearPasswordInput.Text;

				passwordInput.Visibility = Visibility.Visible;
				clearPasswordInput.Visibility = Visibility.Collapsed;
			}
		}

		private void pwdLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (pwdLength != null) {
				pwdLength.Content = $"{(int)pwdLengthSlider.Value}";
			}
		}

		string randomString = "";
		private void genPwdButton_Click(object sender, RoutedEventArgs e)
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

				if (pwdToggle.IsChecked == true) {
					clearPasswordInput.Text = randomString;
				} else {
					passwordInput.Password = randomString;
				}
			} catch (Exception) {
				MessageBox.Show("Lütfen en az bir karakter seti seçin!", "Hatalı Karakter Seti!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void savePwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (ManageDbWindow.createuc.pwdToggle.IsChecked == true) {
				ManageDbWindow.createuc.clearPasswordInput.Text = randomString;
			} else {
				ManageDbWindow.createuc.passwordInput.Password = randomString;
			}
			Close();
		}
	}
}