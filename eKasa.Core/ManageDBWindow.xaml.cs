using System;
using System.Windows;
using System.Windows.Input;
using eKasa.Core.UserControls;

namespace eKasa.Core
{
	public partial class HomeWindow : Window
	{
		#region UserControls
		readonly static public HomeView homev = new();
		readonly static public AddEntryView createv= new();
		readonly static public EditEntryView editv = new();
		readonly static public AboutView aboutv = new();
		readonly static public AppSettingsView appSettingsv = new();
		#endregion

		public HomeWindow()
		{
			InitializeComponent();
			contentCanvas.Children.Add(homev);
		}

		static public void UpdateAllViews()
		{
			try {
				Database.Restore(ref GlobalSettings.dbSettings.InternalDb, GlobalSettings.dbSettings.FilePath);
				homev.entriesDataGrid.ItemsSource = GlobalSettings.dbSettings.InternalDb.Entries;
				homev.entriesDataGrid.Items.Refresh();

				homev.UpdateDbHint();
				homev.entriesDataGrid.SelectedIndex = -1;
				homev.tooltipLabel.Content = "Veritabanı yenilendi!";
			} catch (Exception ex) { GlobalSettings.logger.Error(ex); }
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				DragMove();
				homev.tooltipLabel.Content = "";
				appSettingsv.tooltipLabel.Content = "";
				homev.entriesDataGrid.SelectedIndex = -1;
			}
		}

		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homev);
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(createv);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(editv);
		}

		private void AboutButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(aboutv);
		}

		private void HelpButton_Click(object sender, RoutedEventArgs e)
		{
			HelpWindow hw = new();
			hw.Show();
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(appSettingsv);
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