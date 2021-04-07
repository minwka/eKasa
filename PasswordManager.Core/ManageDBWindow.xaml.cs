using System.Windows;
using System.Windows.Input;
using PasswordManager.Core.UserControls;

namespace PasswordManager.Core
{
	public partial class ManageDBWindow : Window
	{
		static public HomeUserControl homeuc = new();
		static public CreateUserControl createuc = new();
		static public EditUserControl edituc = new();
		static public AboutUserControl aboutuc = new();
		static public SettingsUserControl settingsuc = new();

		public ManageDBWindow()
		{
			InitializeComponent();
			contentCanvas.Children.Add(homeuc);
			homeuc.forceRefresh();
		}

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				DragMove();
				homeuc.tooltipLabel.Content = "";
				homeuc.entriesDataGrid.SelectedIndex = -1;
			}
		}

		private void homeButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homeuc);
		}

		private void createButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(createuc);
		}

		private void editButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(edituc);
		}

		private void aboutButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(aboutuc);
		}

		private void helpButton_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow hw = new();
			hw.Show();
		}

		private void settingsButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(settingsuc);
		}

		private void terminateButton_Click(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Uygulamadan çıkmak istediğinize emin misiniz?\nKaydedilmemiş tüm değişiklikleriniz kaybolacaktır!", "Çıkışı onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (r == MessageBoxResult.Yes) {
				Application.Current.Shutdown();
			}
		}
	}
}