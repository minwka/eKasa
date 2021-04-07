using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PasswordManager.Core.Models;

namespace PasswordManager.Core.UserControls
{
	public partial class OptionsUserControl : UserControl
	{
		public OptionsUserControl()
		{ InitializeComponent(); }

		private void copyUserButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
			if (entry != null) {
				Clipboard.SetText(entry.username);
				ManageDBWindow.homeuc.tooltipLabel.Content = "Kullanıcı adı panoya kopyalandı!";
				var canvas = (Canvas)Parent;
				canvas.Children.Remove(this);
			}
		}

		private void copyPwdButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
			if (entry != null) {
				Clipboard.SetText(entry.password);
				ManageDBWindow.homeuc.tooltipLabel.Content = "Şifre panoya kopyalandı!";
				var canvas = (Canvas)Parent;
				canvas.Children.Remove(this);
			}
		}

		private void typeUserButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
			if (entry != null) {
				var canvas = (Canvas)Parent;
				var grid = (Grid)(canvas.Parent);
				var window = (Window)(grid.Parent);
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"s {entry.username}");
				canvas.Children.Remove(this);
			}
		}

		private void typePwdButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
			if (entry != null) {
				var canvas = (Canvas)Parent;
				var grid = (Grid)(canvas.Parent);
				var window = (Window)(grid.Parent);
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"s {entry.password}");
				canvas.Children.Remove(this);
			}
		}

		private void typeCredsButton_Click(object sender, RoutedEventArgs e)
		{
			var entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
			if (entry != null) {
				var canvas = (Canvas)Parent;
				var grid = (Grid)(canvas.Parent);
				var window = (Window)(grid.Parent);
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"m {entry.username} {entry.password}");
				canvas.Children.Remove(this);
			}
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			var huc = (Canvas)Parent;
			huc.Children.Remove(this);
		}

		private void canvasGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			returnButton_Click(sender, e);
		}
	}
}