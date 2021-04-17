using System;
using System.Windows;
using System.Windows.Controls;

namespace eKasa.Core
{
	public partial class EditEntryView : UserControl
	{
		public EditEntryView()
		{ InitializeComponent(); }

		private void GenPwdButton_Click(object sender, RoutedEventArgs e)
		{ var gpw = new PasswordGenWindow(); gpw.Show(); }

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPasswordInput.Text = passwordInput.Password;
				clearPwdPreview.Text = pwdPreview.Password;

				passwordInput.Visibility = Visibility.Collapsed;
				clearPasswordInput.Visibility = Visibility.Visible;
				pwdPreview.Visibility = Visibility.Collapsed;
				clearPwdPreview.Visibility = Visibility.Visible;
			} else {
				passwordInput.Password = clearPasswordInput.Text;
				pwdPreview.Password = clearPwdPreview.Text;

				clearPasswordInput.Visibility = Visibility.Collapsed;
				passwordInput.Visibility = Visibility.Visible;
				clearPwdPreview.Visibility = Visibility.Collapsed;
				pwdPreview.Visibility = Visibility.Visible;
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) passwordInput.Password = clearPasswordInput.Text;

				ref var dg = ref HomeWindow.homev.entriesDataGrid;
				var entry = (EntryModel)dg.SelectedItem;
				entry.Id = entry.Id;
				entry.Name = nameInput.Text;
				entry.Username = usernameInput.Text;
				entry.Tag = tagInput.Text;
				entry.Password = passwordInput.Password == "" ? entry.Password : passwordInput.Password;
				dg.Items.Refresh();

				foreach (var child in mainGrid.Children) {
					if (child.GetType() == typeof(TextBox)) {
						((TextBox)child).Text = "";
					} else if (child.GetType() == typeof(PasswordBox)) {
						((PasswordBox)child).Password = "";
					}
				}
				pwdToggle.IsChecked = false;

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(HomeWindow.homev);

				HomeWindow.homev.tooltipLabel.Content = "Kayıt düzenlendi!";
			} catch (Exception ex) { GlobalSettings.logger.Error(ex); }
		}
	}
}