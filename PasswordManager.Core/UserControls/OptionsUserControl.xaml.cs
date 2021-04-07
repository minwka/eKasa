﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PasswordManager.Core.Models;

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
		private void optionsUserControl_Loaded(object sender, RoutedEventArgs e)
		{
			canvas = (Canvas)Parent;
			grid = (Grid)(canvas.Parent);
			window = (Window)(grid.Parent);
			entry = (EntryModel)ManageDBWindow.homeuc.entriesDataGrid.SelectedItem;
		}

		private void copyUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				Clipboard.SetText(entry.username);
				ManageDBWindow.homeuc.tooltipLabel.Content = "Kullanıcı adı panoya kopyalandı!";
				canvas.Children.Remove(this);
			}
		}

		private void copyPwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				Clipboard.SetText(entry.password);
				ManageDBWindow.homeuc.tooltipLabel.Content = "Şifre panoya kopyalandı!";
				canvas.Children.Remove(this);
			}
		}

		private void typeUserButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"s {entry.username}");
				canvas.Children.Remove(this);
			}
		}

		private void typePwdButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"s {entry.password}");
				canvas.Children.Remove(this);
			}
		}

		private void typeCredsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entry != null) {
				window.WindowState = WindowState.Minimized;
				Process.Start("AutofillHelper.exe", $"m {entry.username} {entry.password}");
				canvas.Children.Remove(this);
			}
		}

		private void returnButton_Click(object sender, RoutedEventArgs e)
		{
			var huc = (Canvas)Parent;
			huc.Children.Remove(this);
		}

		private void canvasGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			returnButton_Click(sender, e);
		}
	}
}