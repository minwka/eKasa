using System.Windows;
using System.Windows.Input;
using eKasa.Installer.Controls;

namespace eKasa.Installer
{
	public partial class MainWindow : Window
	{
		#region UserControls
		readonly static public WelcomePage ucwelcome = new();
		readonly static public LicensePage uclicense = new();
		readonly static public OptionsPage ucoptions = new();
		readonly static public ProgressPage ucprogress = new();
		readonly static public LaunchPage uclaunch = new();
		readonly static public ErrorPage ucerror = new();
		#endregion

		public MainWindow()
		{
			InitializeComponent();
			content.Children.Add(ucwelcome);
			back.Visibility = Visibility.Hidden;
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{ if (e.ChangedButton == MouseButton.Left) DragMove(); }

		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{ if (e.Key == Key.System && e.SystemKey == Key.F4) e.Handled = true; }

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