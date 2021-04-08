using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using PasswordManager.Installer.Classes;
using File = System.IO.File;

namespace PasswordManager.Installer.Controls
{
	public partial class progress : UserControl
	{
		public progress()
		{ InitializeComponent(); }

		public void CopyPreferences()
		{
			InstallParameters.installPath = MainWindow.ucoptions.path.Text;
			InstallParameters.desktopShortcut = (bool)MainWindow.ucoptions.scDesktop.IsChecked;
			InstallParameters.startmenuShortcut = (bool)MainWindow.ucoptions.scStart.IsChecked;
			InstallParameters.launchatStartup = (bool)MainWindow.ucoptions.launchAtStartup.IsChecked;
		}

		public void BeginInstall()
		{
			var canvas = (Canvas)Parent;
			var grid = (Grid)canvas.Parent;
			var window = (Window)grid.Parent;

			try {
				ProcessStartInfo psi = new();
				psi.FileName = "cmd.exe";
				psi.Arguments = $"/c Binaries\\7z\\7za.exe x -p\"enc765_-_-_inst05\" -o\"{InstallParameters.installPath}\" Binaries\\Install\\package.7z -y";
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
			#region Replace w/ app.config
			RegistryKey iSettings = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\eKasa");
			iSettings.SetValue("install_path", InstallParameters.installPath);
			//iSettings.SetValue("license_type", "Developer");
			//iSettings.SetValue("licensed_by", "NAMI");
			//iSettings.SetValue("license_key", @"owGbwMvMwCH2Lv3x/GtxmgGMa8ST2HMyk1PzilMT8pLiXVzDLCJ0XfxDuTpKWRjEOBhkxRRZzv1z6E6RvRAkxBpbBNPIygRSzsDFKQAT0ZNhZPggst5mvfaB6fyxuUnHz77cc22bl+3Pc11ve/f3pfZuzFjLyLCkRqL/suG74s5K1a2yvE13UqevvlQvJXUtYF9pib3sFl4A");
			iSettings.Close();
			#endregion

			if (InstallParameters.desktopShortcut) {
				var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				var shell = new WshShell();
				var shortCutLinkFilePath = $"{startupFolderPath}\\Desktop\\eKasa.lnk";
				var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
				windowsApplicationShortcut.Description = "eKasa. Şifre yöneticisinde tercihiniz.";
				windowsApplicationShortcut.WorkingDirectory = $"{InstallParameters.installPath}";
				windowsApplicationShortcut.TargetPath = $"{InstallParameters.installPath}\\eKasa.exe";
				windowsApplicationShortcut.Save();
			}

			if (InstallParameters.startmenuShortcut) {
				var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
				var shell = new WshShell();
				var shortCutLinkFilePath = $"{startupFolderPath}\\eKasa.lnk";
				var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
				windowsApplicationShortcut.Description = "eKasa. Şifre yöneticisinde tercihiniz.";
				windowsApplicationShortcut.WorkingDirectory = $"{InstallParameters.installPath}";
				windowsApplicationShortcut.TargetPath = $"{InstallParameters.installPath}\\eKasa.exe";
				windowsApplicationShortcut.Save();
			}

			if (InstallParameters.launchatStartup) {
				var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
				var shell = new WshShell();
				var shortCutLinkFilePath = $"{startupFolderPath}\\eKasa.lnk";
				var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
				windowsApplicationShortcut.Description = "eKasa. Şifre yöneticisinde tercihiniz.";
				windowsApplicationShortcut.WorkingDirectory = $"{InstallParameters.installPath}";
				windowsApplicationShortcut.TargetPath = $"{InstallParameters.installPath}\\eKasa.exe";
				windowsApplicationShortcut.Save();
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
				window.next.Click += terminate;
				window.back.Visibility = Visibility.Hidden;
				window.terminate.Visibility = Visibility.Hidden;

				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.uclaunch);
			});
		}

		void terminate(object sender, RoutedEventArgs e)
		{
			if (MainWindow.uclaunch.scLaunch.IsChecked == true) {
				Process.Start($"{InstallParameters.installPath}\\eKasa.exe");
			}
			Application.Current.Shutdown();
		}
	}
}