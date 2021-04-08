﻿using System.Windows;
using System.Windows.Input;
using PasswordManager.Installer.Controls;

namespace PasswordManager.Installer
{
	public partial class MainWindow : Window
	{
		static public welcome ucwelcome = new();
		static public license uclicense = new();
		static public options ucoptions = new();
		static public progress ucprogress = new();
		static public launch uclaunch = new();
		static public error ucerror = new();

		public MainWindow()
		{
			InitializeComponent();

			content.Children.Add(ucwelcome);
			back.Visibility = Visibility.Hidden;
		}

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				DragMove();
			}
		}

		private void terminate_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("Kaldırma işlemini iptal etmek istediğinize emin misiniz?", "Çıkışı Onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes) {
				Application.Current.Shutdown();
			}
		}

		int page = 0;
		private void next_Click(object sender, RoutedEventArgs e)
		{
			content.Children.Clear();
			switch (page) {
				case 0:
					content.Children.Add(uclicense);
					next.Content = "KABUL EDİYORUM";
					back.Visibility = Visibility.Visible;
					break;
				case 1:
					content.Children.Add(ucoptions);
					next.Content = "YÜKLE";
					break;
				case 2:
					content.Children.Add(ucprogress);
					ucprogress.CopyPreferences();
					next.Content = "YÜKLENİYOR";
					next.IsEnabled = false;
					back.IsEnabled = false;
					terminate.IsEnabled = false;
					ucprogress.BeginInstall();
					break;
			}
			page++;
		}

		private void back_Click(object sender, RoutedEventArgs e)
		{
			content.Children.Clear();
			switch (page) {
				case 2:
					content.Children.Add(uclicense);
					next.Content = "KABUL EDİYORUM";
					next.IsEnabled = true;
					break;
				case 1:
					content.Children.Add(ucwelcome);
					next.Content = "İLERİ";
					back.Visibility = Visibility.Hidden;
					break;
			}
			page--;
		}
	}
}