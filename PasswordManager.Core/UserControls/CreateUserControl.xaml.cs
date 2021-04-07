using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordManager.Core.Classes;
using PasswordManager.Core.Models;

namespace PasswordManager.Core.UserControls
{
	public partial class CreateUserControl : UserControl
	{
		public CreateUserControl()
		{ InitializeComponent(); }

		private void addButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					passwordInput.Password = clearPasswordInput.Text;
				}
				var newEntry = new EntryModel() {
					guid = Guid.NewGuid(),
					name = nameInput.Text,
					username = usernameInput.Text,
					tag = tagInput.Text,
					password = passwordInput.Password
				};

				DTO.InternalDB.entries.Add(newEntry);

				ManageDBWindow.homeuc.entriesDataGrid.ItemsSource = new List<EntryModel>();
				ManageDBWindow.homeuc.entriesDataGrid.ItemsSource = DTO.InternalDB.entries;

				nameInput.Text = "";
				usernameInput.Text = "";
				tagInput.Text = "";
				clearPasswordInput.Text = "";
				passwordInput.Password = "";

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(ManageDBWindow.homeuc);

				var color = new SolidColorBrush(Color.FromRgb(20, 120, 215));
				ManageDBWindow.homeuc.tooltipLabel.Foreground = color;
				ManageDBWindow.homeuc.tooltipLabel.Content = "Kayıt eklendi!";
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
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

		private void genPwdButton_Click(object sender, RoutedEventArgs e)
		{
			var gpw = new PasswordGenWindow();
			gpw.Show();
		}
	}
}