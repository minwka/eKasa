using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordManager.Installer.Classes;

namespace PasswordManager.Installer.Controls
{
	public partial class launch : UserControl
	{
		public launch()
		{ InitializeComponent(); }

		public void finishInstall()
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
			window.next.Click += launcher;
			window.back.Click += terminate;
		}

		void launcher(object sender, RoutedEventArgs e)
		{
			if (scLaunch.IsChecked == true) {
				Process.Start($"{InstallParameters.installPath}\\eKasa.exe");
			}
			Application.Current.Shutdown();
		}

		void terminate(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}
}