﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace eKasa.Core
{
	public partial class AddEntryView : UserControl
	{
		public AddEntryView() { InitializeComponent(); }

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
			}
			else {
				passwordInput.Password = clearPasswordInput.Text;

				passwordInput.Visibility = Visibility.Visible;
				clearPasswordInput.Visibility = Visibility.Collapsed;
			}
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) passwordInput.Password = clearPasswordInput.Text;

				var newEntry = new EntryModel() {
					Id = Guid.NewGuid(),
					Name = nameInput.Text,
					Username = usernameInput.Text,
					Password = passwordInput.Password,
					Tag = tagInput.Text
				};

				Database.InternalDb.Entries.Add(newEntry);
				Database.InternalDb.ModifiedDate = DateTime.UtcNow.ToString();
				HomeWindow.homeView.UpdateHomeView();

				foreach (var child in mainGrid.Children) {
					if (child.GetType() == typeof(TextBox)) {
						((TextBox)child).Text = "";
					}
					if (child.GetType() == typeof(PasswordBox)) {
						((PasswordBox)child).Password = "";
					}
				}

				if (stayHereToggle.IsChecked == false) {
					((ContentControl)Parent).Content = HomeWindow.homeView;
				}

				nameInput.Focus();
				HomeWindow.homeView.tooltipLabel.Content = "Kayıt eklendi!";
			} catch (Exception ex) { GlobalSettings.logger.Error(ex); }
		}

		private void CreateControl_Loaded(object sender, RoutedEventArgs e)
		{ nameInput.Focus(); }
	}
}