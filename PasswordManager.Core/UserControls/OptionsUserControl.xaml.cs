using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager.Core.UserControls
{
	public partial class OptionsUserControl : UserControl
	{
		public OptionsUserControl()
		{ InitializeComponent(); }

		Canvas canvas;
		Grid grid;
		Window window;
		EntryModel entry;
		private void OptionsUserControl_Loaded(object sender, RoutedEventArgs e)
		{
			canvas = (Canvas)Parent;
			grid = (Grid)(canvas.Parent);
			window = (Window)(grid.Parent);
			entry = (EntryModel)ManageDbWindow.homeuc.entriesDataGrid.SelectedItem;
		}

		private void CopyUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				Clipboard.SetText(entry.Username);
				ManageDbWindow.homeuc.tooltipLabel.Content = "Kullanıcı adı panoya kopyalandı!";
				canvas.Children.Remove(this);
			}
		}

		private void CopyPwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				Clipboard.SetText(entry.Password);
				ManageDbWindow.homeuc.tooltipLabel.Content = "Şifre panoya kopyalandı!";
				canvas.Children.Remove(this);
			}
		}

		private void TypeUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"s {entry.Username}");
				canvas.Children.Remove(this);
			}
		}

		private void TypePwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"s {entry.Password}");
				canvas.Children.Remove(this);
			}
		}

		private void TypeCredsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"m {entry.Username} {entry.Password}");
				canvas.Children.Remove(this);
			}
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			var huc = (Canvas)Parent;
			huc.Children.Remove(this);
		}

		private void CanvasGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{ ReturnButton_Click(sender, e); }
	}
}