using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eKasa.Core
{
	public partial class HomeWindow : Window
	{
		#region Views
		readonly static public HomeView homeView = new();
		readonly static public AddEntryView addView = new();
		readonly static public EditEntryView editView = new();
		readonly static public AboutAppView aboutView = new();
		readonly static public AppSettingsView appSettingsView = new();
		#endregion

		public HomeWindow()
		{
			InitializeComponent();
			DisplayContent.Content = homeView;
			windowDbTitle.Content = $"{Database.InternalDb.Owner} • {Database.InternalDb.Name}";
		}

		private void Navigate(object sender, RoutedEventArgs e)
		{
			var source = sender as Button;

			switch (source.TabIndex) {
				case 10:
					DisplayContent.Content = homeView;
					break;

				case 11:
					DisplayContent.Content = addView;
					break;

				case 12:
					DisplayContent.Content = editView;
					break;

				case 13:
					HelpWindow hw = new();
					hw.Show();
					break;

				case 14:
					DisplayContent.Content = aboutView;
					break;

				case 15:
					DisplayContent.Content = appSettingsView;
					break;

				default:
					break;
			}
		}

		#region Window Actions
		private void HomeWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				DragMove();
				homeView.tooltipLabel.Content = "";
				appSettingsView.tooltipLabel.Content = "";
			}
		}

		private void HomeWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.System && e.SystemKey == Key.F4) {
				e.Handled = true;
				Terminate(sender, e);
			}
		}

		private void WindowClose_Click(object sender, RoutedEventArgs e)
		{ Terminate(sender, e); }

		private void WindowMinimize_Click(object sender, RoutedEventArgs e)
		{ WindowState = WindowState.Minimized; }

		private void Terminate(object sender, RoutedEventArgs e)
		{
			var r = MessageBox.Show("Uygulamadan çıkmak istediğinize emin misiniz?\nKaydedilmemiş tüm değişiklikleriniz kaybolacaktır!", "Çıkışı onayla!", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (r == MessageBoxResult.Yes) {
				Application.Current.Shutdown();
			}
		}
		#endregion
	}
}