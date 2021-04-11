using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordManager.Installer.Classes;

namespace PasswordManager.Installer.Controls
{
	public partial class Launch : UserControl
	{
		public Launch()
		{ InitializeComponent(); }

		public void FinishInstall()
		{
			var canvas = (Canvas)Parent;
			var grid = (Grid)canvas.Parent;
			var window = (MainWindow)grid.Parent;

			window.terminate.Visibility = Visibility.Hidden;
			window.next.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0078D7"));
			window.next.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0078D7"));
			window.back.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0078D7"));
			window.back.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0078D7"));
			window.next.Content = "BİTİR";
			window.back.Content = "ÇIK";
			window.next.IsEnabled = true;
			window.back.IsEnabled = true;
			window.back.Visibility = Visibility.Visible;
			window.next.Click += Launcher;
			window.back.Click += Terminate;
		}

		void Launcher(object sender, RoutedEventArgs e)
		{
			if (scLaunch.IsChecked == true)
				Process.Start($"{InstallParameters.InstallPath}\\eKasa.exe");
			Application.Current.Shutdown();
		}

		void Terminate(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }
	}
}