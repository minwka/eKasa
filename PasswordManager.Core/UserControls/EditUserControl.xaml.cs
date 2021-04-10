using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordManager.Core.UserControls
{
	public partial class EditUserControl : UserControl
	{
		public EditUserControl()
		{ InitializeComponent(); }

		private void pwdToggle_CheckedChanged(object sender, RoutedEventArgs e)
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

		private void editButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				if (pwdToggle.IsChecked == true) {
					pwdInput.Password = clearPwdInput.Text;
				}

				var edg = ManageDbWindow.homeuc.entriesDataGrid;
				var oldEntry = (EntryModel)edg.SelectedItem;
				var replaceIndex = edg.SelectedIndex == -1 ? 0 : edg.SelectedIndex;
				var newEntry = new EntryModel() {
					Id = oldEntry.Id,
					Name = nameInput.Text,
					Username = usernameInput.Text,
					Tag = tagInput.Text,
					Password = pwdInput.Password.Length == 0 ? oldEntry.Password : pwdInput.Password
				};

				ref var idb = ref Settings.dbSettings.InternalDb;
				idb.Entries.Remove(oldEntry);
				idb.Entries.Insert(replaceIndex, newEntry);

				edg.ItemsSource = new List<EntryModel>();
				edg.ItemsSource = idb.Entries;
				edg.SelectedIndex = -1;

				idPreview.Text = "";
				namePreview.Text = "";
				usernamePreview.Text = "";
				pwdPreview.Password = "";
				clearPwdPreview.Text = "";
				tagPreview.Text = "";
				nameInput.Text = "";
				usernameInput.Text = "";
				pwdInput.Password = "";
				clearPwdInput.Text = "";
				tagInput.Text = "";

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(ManageDbWindow.homeuc);

				var color = new SolidColorBrush(Color.FromRgb(20, 120, 215));
				ManageDbWindow.homeuc.tooltipLabel.Foreground = color;
				ManageDbWindow.homeuc.tooltipLabel.Content = "Kayıt düzenlendi!";
			} catch (Exception ex) {
				MessageBox.Show("Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.", "Hata!", MessageBoxButton.OK, MessageBoxImage.Error);
				File.AppendAllText("err.log", $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\nError message: {ex.Message}\nError stacktrace: {ex.StackTrace}\nError inner exception: {ex.InnerException}\n\n\n");
			}
		}
	}
}