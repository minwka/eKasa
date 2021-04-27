using System;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.GlobalSettings;

namespace eKasa.Core
{
	public partial class EditEntryView : UserControl
	{
		public EditEntryView() { InitializeComponent(); }

		private void GenPwdButton_Click(object sender, RoutedEventArgs e)
		{
			var gpw = new PasswordGenWindow();
			gpw.Show();
		}

		private void PwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
		{
			if (pwdToggle.IsChecked == true) {
				clearPasswordInput.Text = passwordInput.Password;
				clearPwdPreview.Text = pwdPreview.Password;

				passwordInput.Visibility = Visibility.Collapsed;
				clearPasswordInput.Visibility = Visibility.Visible;
				pwdPreview.Visibility = Visibility.Collapsed;
				clearPwdPreview.Visibility = Visibility.Visible;
			}
			else {
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

				var oldEntry = (EntryModel)HomeWindow.homev.entriesDataGrid.SelectedItem;
				if (oldEntry != null) {
					var ei = HomeWindow.homev.entriesDataGrid.SelectedIndex;
					EntryModel newEntry = new() {
						Id = Guid.NewGuid(),
						Name = nameInput.Text,
						Username = usernameInput.Text,
						Tag = tagInput.Text,
						Password = passwordInput.Password == "" ? oldEntry.Password : passwordInput.Password
					};

					Database.InternalDb.Entries.RemoveAt(ei);
					Database.InternalDb.Entries.Insert(ei, newEntry);
					Database.InternalDb.ModifiedDate = DateTime.UtcNow.ToString();
					HomeWindow.UpdateHomeView();
				}

				foreach (var child in mainGrid.Children) {
					if (child.GetType() == typeof(TextBox)) {
						((TextBox)child).Text = "";
					}
					else if (child.GetType() == typeof(PasswordBox)) {
						((PasswordBox)child).Password = "";
					}
				}
				pwdToggle.IsChecked = false;

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(HomeWindow.homev);

				HomeWindow.homev.tooltipLabel.Content = "Kayıt düzenlendi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void EditControl_Loaded(object sender, RoutedEventArgs e)
		{
			nameInput.Focus();

			var entry = (EntryModel)HomeWindow.homev.entriesDataGrid.SelectedItem;
			if (entry != null) {
				idPreview.Text = entry.Id.ToString();
				namePreview.Text = entry.Name;
				usernamePreview.Text = entry.Username;
				pwdPreview.Password = entry.Password;
				tagPreview.Text = entry.Tag;
				nameInput.Text = entry.Name;
				usernameInput.Text = entry.Username;
				tagInput.Text = entry.Tag;
			}
		}
	}
}