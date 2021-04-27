using GregsStack.InputSimulatorStandard;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eKasa.Core
{
	public partial class ActionsView : UserControl
	{
		public ActionsView() { InitializeComponent(); }

		Canvas canvas;
		Grid grid;
		Window window;
		EntryModel entry;
		private void OptionsUserControl_Loaded(object sender, RoutedEventArgs e)
		{
			canvas = (Canvas)Parent;
			grid = (Grid)(canvas.Parent);
			window = (Window)(grid.Parent);
			entry = (EntryModel)HomeWindow.homev.entriesDataGrid.SelectedItem;
		}

		private void CopyUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				Clipboard.SetText(entry.Username);
				HomeWindow.homev.tooltipLabel.Content = "Kullanıcı adı panoya kopyalandı!";
				canvas.Children.Remove(this);
			}
		}

		private void CopyPwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				Clipboard.SetText(entry.Password);
				HomeWindow.homev.tooltipLabel.Content = "Şifre panoya kopyalandı!";
				canvas.Children.Remove(this);
			}
		}

		private void TypeUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				var ips = new InputSimulator();
				ips.Keyboard.TextEntry(entry.Username);
				canvas.Children.Remove(this);
			}
		}

		private void TypePwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				var ips = new InputSimulator();
				ips.Keyboard.TextEntry(entry.Password);
				canvas.Children.Remove(this);
			}
		}

		private void TypeCredsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				var ips = new InputSimulator();
				ips.Keyboard.TextEntry(entry.Username);
				ips.Keyboard.KeyPress(GregsStack.InputSimulatorStandard.Native.VirtualKeyCode.TAB);
				ips.Keyboard.TextEntry(entry.Password);
				canvas.Children.Remove(this);
			}
		}

		private void ReturnButton_Click(object sender, RoutedEventArgs e)
		{
			var huc = (Canvas)Parent;
			huc.Children.Remove(this);
		}

		private void CanvasGrid_MouseDown(object sender, MouseButtonEventArgs e) { ReturnButton_Click(sender, e); }
	}
}