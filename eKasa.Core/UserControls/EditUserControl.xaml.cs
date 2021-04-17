using System;
using System.Windows;
using System.Windows.Controls;

namespace eKasa.Core.UserControls
{
	public partial class EditUserControl : UserControl
	{
		public EditUserControl()
		{ InitializeComponent(); }

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPwdInput.Text = pwdInput.Password;
				clearPwdPreview.Text = pwdPreview.Password;

				pwdInput.Visibility = Visibility.Collapsed;
				clearPwdInput.Visibility = Visibility.Visible;
				pwdPreview.Visibility = Visibility.Collapsed;
				clearPwdPreview.Visibility = Visibility.Visible;
			} else {
				pwdInput.Password = clearPwdInput.Text;
				pwdPreview.Password = clearPwdPreview.Text;

				clearPwdInput.Visibility = Visibility.Collapsed;
				pwdInput.Visibility = Visibility.Visible;
				clearPwdPreview.Visibility = Visibility.Collapsed;
				pwdPreview.Visibility = Visibility.Visible;
			}
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) pwdInput.Password = clearPwdInput.Text;

				ref var dg = ref ManageDbWindow.homeuc.entriesDataGrid;
				var entry = (EntryModel)dg.SelectedItem;
				entry.Id = entry.Id;
				entry.Name = nameInput.Text;
				entry.Username = usernameInput.Text;
				entry.Tag = tagInput.Text;
				entry.Password = pwdInput.Password == "" ? entry.Password : pwdInput.Password;
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
				parent.Children.Add(ManageDbWindow.homeuc);

				ManageDbWindow.homeuc.tooltipLabel.Content = "Kayıt düzenlendi!";
			} catch (Exception ex) { Settings.logger.Error(ex); }
		}
	}
}