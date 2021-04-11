using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.Core.UserControls
{
	public partial class NewDbUserControl : UserControl
	{
		public NewDbUserControl()
		{ InitializeComponent(); }

		private void GenPwdButton_Click(object sender, RoutedEventArgs e)
		{
			var gpw = new PasswordGenWindow();
			gpw.Show();
		}

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
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

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					passwordInput.Password = clearPasswordInput.Text;
				}
				var newEntry = new EntryModel() {
					Id = Guid.NewGuid(),
					Name = nameInput.Text,
					Username = usernameInput.Text,
					Tag = tagInput.Text,
					Password = passwordInput.Password
				};

				Settings.dbSettings.InternalDb.Entries.Add(newEntry);

				ManageDbWindow.homeuc.entriesDataGrid.ItemsSource = new List<EntryModel>();
				ManageDbWindow.homeuc.entriesDataGrid.ItemsSource = Settings.dbSettings.InternalDb.Entries;

				nameInput.Text = "";
				usernameInput.Text = "";
				tagInput.Text = "";
				clearPasswordInput.Text = "";
				passwordInput.Password = "";

				if (stayHereToggle.IsChecked==false) {
					var parent = (Canvas)Parent;
					parent.Children.Clear();
					parent.Children.Add(ManageDbWindow.homeuc);
				}

				ManageDbWindow.homeuc.tooltipLabel.Content = "Kayıt eklendi!";
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("error.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}
	}
}