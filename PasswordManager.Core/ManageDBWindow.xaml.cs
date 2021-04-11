using System.Windows;
using System.Windows.Input;
using PasswordManager.Core.UserControls;

namespace PasswordManager.Core
{
	public partial class ManageDbWindow : Window
	{
		readonly static public HomeUserControl homeuc = new();
		readonly static public NewDbUserControl createuc = new();
		readonly static public EditUserControl edituc = new();
		readonly static public AboutUserControl aboutuc = new();
		readonly static public SettingsUserControl settingsuc = new();

		public ManageDbWindow()
		{
			InitializeComponent();
			contentCanvas.Children.Add(homeuc);
			homeuc.ForceRefresh();
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				DragMove();
				homeuc.tooltipLabel.Content = "";
				homeuc.entriesDataGrid.SelectedIndex = -1;
			}
		}

		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homeuc);
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(createuc);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(edituc);
		}

		private void AboutButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(aboutuc);
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow hw = new();
			hw.Show();
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(settingsuc);
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Uygulamadan çıkmak istediğinize emin misiniz?\nKaydedilmemiş tüm değişiklikleriniz kaybolacaktır!", "Çıkışı onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (r == MessageBoxResult.Yes) {
				Application.Current.Shutdown();
			}
		}
	}
}