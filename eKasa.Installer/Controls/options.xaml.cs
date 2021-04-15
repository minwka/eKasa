﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace eKasa.Installer.Controls
{
	public partial class Options : UserControl
	{
		public Options()
		{ InitializeComponent(); }

		readonly CommonOpenFileDialog cofd = new();
		private void Browse_Click(object sender, RoutedEventArgs e)
		{
			try {
				cofd.IsFolderPicker = true;
				cofd.InitialDirectory = @"C:\Program Files\";
				CommonFileDialogResult res = cofd.ShowDialog();

				path.Text = res != CommonFileDialogResult.Cancel ? cofd.FileName + @"\eKasa" : path.Text;
			} catch (Exception ex) {
				var canvas = (Canvas)Parent;
				var grid = (Grid)canvas.Parent;
				var window = (Window)grid.Parent;
				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.ucerror);
				((MainWindow)window).next.IsEnabled = false;
				((MainWindow)window).back.IsEnabled = false;

				File.AppendAllText("error.log", $"Error Date: {DateTime.UtcNow.ToLocalTime()}\nError: {ex.Message}\nStack Trace: {ex.StackTrace}\nInner Exception: {ex.InnerException}\n\n");
			}
		}
	}
}