using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using IWshRuntimeLibrary;
using PasswordManager.Installer.Classes;

namespace PasswordManager.Installer.Controls
{
	public partial class progress : UserControl
	{
		public progress()
		{ InitializeComponent(); }

		public void CopyInstallParameters()
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
				Process.Start($"cmd.exe", $"/c Binaries\\7z\\7za.exe x -p\"enc765_-_-_inst05\" -o\"{InstallParameters.installPath}\" Binaries\\Install\\package.7z");
				CreateShortcuts();
				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.uclaunch);
				MainWindow.uclaunch.finishInstall();
			} catch (Exception ex) {
				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.ucerror);
				((MainWindow)window).next.IsEnabled = false;
				((MainWindow)window).back.IsEnabled = false;

				System.IO.File.AppendAllText("error.log", $"Error Date: {DateTime.UtcNow.ToLocalTime()}\nError: {ex.Message}\nStack Trace: {ex.StackTrace}\nInner Exception: {ex.InnerException}\n\n");
			}
		}

		public void CreateShortcuts()
		{
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
		}
	}
}