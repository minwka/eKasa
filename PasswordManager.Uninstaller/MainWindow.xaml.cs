using System.Windows;
using System.Windows.Input;
using PasswordManager.Uninstaller.Controls;

namespace PasswordManager.Uninstaller
{
	public partial class MainWindow : Window
	{
		readonly static public Welcome ucwelcome = new();
		readonly static public Options ucoptions = new();
		readonly static public Progress ucprogress = new();
		readonly static public Done ucdone = new();
		readonly static public Error ucerror = new();

		public MainWindow()
		{
			InitializeComponent();
			content.Children.Add(ucwelcome);
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
					content.Children.Clear();
					content.Children.Add(ucoptions);
					next.Content = "KALDIR";
					break;
				case 1:
					content.Children.Clear();
					content.Children.Add(ucprogress);
					next.Content = "KALDIRILIYOR";
					next.IsEnabled = false;
					terminate.Visibility = Visibility.Hidden;
					ucprogress.BeginUninstall();
					terminate.Click -= Terminate_Click;
					break;
			}
			page++;
		}
	}
}