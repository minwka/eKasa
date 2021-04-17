using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using eKasa.Installer.Classes;
using eKasa.Library;
using Microsoft.Win32;
using File = System.IO.File;

namespace eKasa.Installer.Controls
{
	public partial class Progress : UserControl
	{
		public Progress()
		{ InitializeComponent(); }

		public static void CopyPreferences()
		{ InstallParameters.InstallPath = MainWindow.ucoptions.path.Text; }

		public void BeginInstall()
		{
			var canvas = (Canvas)Parent;
			var grid = (Grid)canvas.Parent;
			var window = (Window)grid.Parent;

			try {
				ProcessStartInfo psi = new();
				psi.FileName = "cmd.exe";
				psi.Arguments = $"/c Binaries\\7z\\7za.exe x -o\"{InstallParameters.InstallPath}\\App\" Binaries\\Install\\package.7z -y";
				psi.CreateNoWindow = true;
				psi.WindowStyle = ProcessWindowStyle.Hidden;

				Process p = new();
				p.EnableRaisingEvents = true;
				p.Exited += new EventHandler(FinalizeAndLaunch);
				p.StartInfo = psi;
				p.Start();

				File.Copy("Uninstall.exe", $"{InstallParameters.InstallPath}\\Uninstall.exe");
			} catch (Exception ex) {
				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.ucerror);
				((MainWindow)window).next.Visibility = Visibility.Hidden;
				((MainWindow)window).back.Visibility = Visibility.Hidden;

				File.AppendAllText("error.log", $"Error Date: {DateTime.UtcNow.ToLocalTime()}\nError: {ex.Message}\nStack Trace: {ex.StackTrace}\nInner Exception: {ex.InnerException}\n\n");
			}
		}

		void FinalizeAndLaunch(object sender, EventArgs e)
		{
			#region Replace w/ settings file
			RegistryKey iSettings = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\eKasa");
			iSettings.SetValue("install_path", InstallParameters.InstallPath);
			iSettings.Close();
			#endregion

			Dispatcher.Invoke(() => {
				#region Shortcuts
				if ((bool)MainWindow.ucoptions.scDesktop.IsChecked) {
					var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
					WinShortcut.Create($"{desktopPath}\\eKasa.lnk", $"{InstallParameters.InstallPath}\\App\\eKasa.exe", "", InstallParameters.InstallPath, "eKasa. Şifre yöneticiniz", "", "");
				}

				if ((bool)MainWindow.ucoptions.scStart.IsChecked) {
					var startmenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
					WinShortcut.Create($"{startmenuPath}\\eKasa.lnk", $"{InstallParameters.InstallPath}\\App\\eKasa.exe", "", startmenuPath, "eKasa. Şifre yöneticiniz", "", "");
				}

				if ((bool)MainWindow.ucoptions.launchAtStartup.IsChecked) {
					var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
					WinShortcut.Create($"{startupPath}\\eKasa.lnk", $"{InstallParameters.InstallPath}\\App\\eKasa.exe", "", startupPath, "eKasa. Şifre yöneticiniz", "", "");
				}
				#endregion

				#region Colors and Actions
				var canvas = (Canvas)Parent;
				var grid = (Grid)canvas.Parent;
				var window = (MainWindow)grid.Parent;

				window.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				window.topBar.Fill = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				window.topBar.Stroke = new SolidColorBrush(Color.FromRgb(0, 120, 215));

				window.next.Content = "ÇIK";
				window.next.IsEnabled = true;
				window.next.Foreground = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				window.next.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215));
				window.next.Click += Terminate;
				window.back.Visibility = Visibility.Hidden;
				window.terminate.Visibility = Visibility.Hidden;
				#endregion

				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.uclaunch);
			});
		}

		void Terminate(object sender, RoutedEventArgs e)
		{
			if (MainWindow.uclaunch.scLaunch.IsChecked == true)
				Process.Start($"{InstallParameters.InstallPath}\\App\\eKasa.exe");
			Application.Current.Shutdown();
		}
	}
}