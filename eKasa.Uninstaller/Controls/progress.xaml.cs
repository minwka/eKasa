using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using static System.Environment;

namespace eKasa.Uninstaller.Controls
{
	public partial class Progress : UserControl
	{
		public Progress()
		{ InitializeComponent(); }

		public void BeginUninstall()
		{
			var canvas = (Canvas)Parent;
			var grid = (Grid)canvas.Parent;
			var window = (Window)grid.Parent;
			string installPath = "";

			try {
				((MainWindow)window).next.Visibility = Visibility.Hidden;
				((MainWindow)window).terminate.Visibility = Visibility.Hidden;
				((MainWindow)window).terminate.Click += Terminate;

				RegistryKey iSettings = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\eKasa");
				if (iSettings != null) {
					installPath = iSettings.GetValue("install_path").ToString();
					if (MainWindow.ucoptions.removeRegistries.IsChecked == true) {
						Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\eKasa");
					}

					if (MainWindow.ucoptions.removeShortcuts.IsChecked == true) {
						var shortcutDesktop = GetFolderPath(SpecialFolder.DesktopDirectory);
						var shortcutStartMenu = GetFolderPath(SpecialFolder.StartMenu);
						var shortcutStartup = GetFolderPath(SpecialFolder.Startup);
						if (MainWindow.ucoptions.removeRegistries.IsChecked == true) {
							File.Delete($"{shortcutDesktop}\\eKasa.lnk");
							File.Delete($"{shortcutStartMenu}\\eKasa.lnk");
							File.Delete($"{shortcutStartup}\\eKasa.lnk");
						}
					}

					Directory.Delete(Path.Combine(installPath,"App"), true);
					iSettings.Close();
				}

				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.ucdone);
				((MainWindow)window).terminate.Visibility = Visibility.Visible;

				#region Set colors
				((MainWindow)window).BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				((MainWindow)window).topBar.Fill = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				((MainWindow)window).topBar.Stroke = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				((MainWindow)window).terminate.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				((MainWindow)window).terminate.Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				#endregion
			} catch (Exception ex) {
				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.ucerror);
				((MainWindow)window).next.Visibility = Visibility.Hidden;
				((MainWindow)window).terminate.Visibility = Visibility.Visible;

				File.AppendAllText("error.log", $"Error Date: {DateTime.UtcNow.ToLocalTime()}\nError: {ex.Message}\nStack Trace: {ex.StackTrace}\nInner Exception: {ex.InnerException}\n\n");
			}
		}

		public void Terminate(object sender, RoutedEventArgs e)
		{
			try {
				if (MainWindow.ucdone.scVisit.IsChecked == true) {
					ProcessStartInfo psi = new();
					psi.FileName = "cmd.exe";
					psi.Arguments = $"/c start https://t.me/anth4";
					psi.CreateNoWindow = true;
					psi.WindowStyle = ProcessWindowStyle.Hidden;

					Process p = new();
					p.EnableRaisingEvents = true;
					p.StartInfo = psi;
					p.Start();
				}
			} catch (Exception) { }
			Application.Current.Shutdown();
		}
	}
}