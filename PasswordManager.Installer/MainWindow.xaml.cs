using System.Windows;
using System.Windows.Input;
using PasswordManager.Installer.Controls;

namespace PasswordManager.Installer
{
	public partial class MainWindow : Window
	{
		readonly static public Welcome ucwelcome = new();
		readonly static public License uclicense = new();
		readonly static public Options ucoptions = new();
		readonly static public Progress ucprogress = new();
		readonly static public Launch uclaunch = new();
		readonly static public Error ucerror = new();

		public MainWindow()
		{
			InitializeComponent();
			content.Children.Add(ucwelcome);
			back.Visibility = Visibility.Hidden;
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) 
				DragMove();
		}

		private void Terminate_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("Kaldırma işlemini iptal etmek istediğinize emin misiniz?", "Çıkışı Onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes) {
				Application.Current.Shutdown();
			}
		}

		int page = 0;
		private void Next_Click(object sender, RoutedEventArgs e)
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
					Progress.CopyPreferences();
					next.Content = "YÜKLENİYOR";
					next.IsEnabled = false;
					back.IsEnabled = false;
					terminate.IsEnabled = false;
					ucprogress.BeginInstall();
					break;
			}
			page++;
		}

		private void Back_Click(object sender, RoutedEventArgs e)
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