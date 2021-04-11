using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using PasswordManager.Installer.Classes;
using File = System.IO.File;

namespace PasswordManager.Installer.Controls
{
	public partial class Progress : UserControl
	{
		public Progress()
		{ InitializeComponent(); }

		public static void CopyPreferences()
		{			InstallParameters.InstallPath = MainWindow.ucoptions.path.Text;		}

		public void BeginInstall()
		{
			var canvas = (Canvas)Parent;
			var grid = (Grid)canvas.Parent;
			var window = (Window)grid.Parent;

			try {
				ProcessStartInfo psi = new();
				psi.FileName = "cmd.exe";
				psi.Arguments = $"/c Binaries\\7z\\7za.exe x -p\"enc765_-_-_inst05\" -o\"{InstallParameters.InstallPath}\" Binaries\\Install\\package.7z -y";
				psi.CreateNoWindow = true;
				psi.WindowStyle = ProcessWindowStyle.Hidden;

				Process p = new();
				p.EnableRaisingEvents = true;
				p.Exited += new EventHandler(FinalizeAndLaunch);
				p.StartInfo = psi;
				p.Start();
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
			//iSettings.SetValue("license_type", "Developer");
			//iSettings.SetValue("licensed_by", "NAMI");
			//iSettings.SetValue("license_key", @"owGbwMvMwCH2Lv3x/GtxmgGMa8ST2HMyk1PzilMT8pLiXVzDLCJ0XfxDuTpKWRjEOBhkxRRZzv1z6E6RvRAkxBpbBNPIygRSzsDFKQAT0ZNhZPggst5mvfaB6fyxuUnHz77cc22bl+3Pc11ve/f3pfZuzFjLyLCkRqL/suG74s5K1a2yvE13UqevvlQvJXUtYF9pib3sFl4A");
			iSettings.Close();
			#endregion

			if ((bool)MainWindow.ucoptions.scDesktop.IsChecked) {
				var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				WinShortcut.Create($"{desktopPath}\\eKasa.lnk", $"{InstallParameters.InstallPath}\\eKasa.exe", "", InstallParameters.InstallPath, "eKasa. Şifre yöneticiniz", "", "");
			}

			if ((bool)MainWindow.ucoptions.scStart.IsChecked) {
				var startmenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
				WinShortcut.Create($"{startmenuPath}\\eKasa.lnk", $"{InstallParameters.InstallPath}\\eKasa.exe", "", startmenuPath, "eKasa. Şifre yöneticiniz", "", "");
			}

			if ((bool)MainWindow.ucoptions.launchAtStartup.IsChecked) {
				var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
				WinShortcut.Create($"{startupPath}\\eKasa.lnk", $"{InstallParameters.InstallPath}\\eKasa.exe", "", startupPath, "eKasa. Şifre yöneticiniz", "", "");
			}

			Dispatcher.Invoke(() => {
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

				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.uclaunch);
			});
		}

		void Terminate(object sender, RoutedEventArgs e)
		{
			if (MainWindow.uclaunch.scLaunch.IsChecked == true) 
				Process.Start($"{InstallParameters.InstallPath}\\eKasa.exe");
			Application.Current.Shutdown();
		}
	}
}