using System.Windows;
using System.Windows.Input;

namespace eKasa.Core
{
	public partial class HelpWindow : Window
	{
		#region UserControls
		readonly HomeHelp homeHelp = new();
		readonly CreateHelp createHelp = new();
		readonly EditHelp editHelp = new();
		readonly SettingsHelp settingsHelp = new();
		readonly UnlockHelp loginHelp = new();
		readonly NewDbHelp newDbHelp = new();
		readonly PwdGenHelp pwdGenHelp = new();
		readonly AutofillHelp autofillHelp = new();
		readonly EncryptionHelp encryptionHelp = new();
		#endregion

		public HelpWindow()
		{
			InitializeComponent();
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homeHelp);
		}

		private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{ if (e.ChangedButton == MouseButton.Left) DragMove(); }

		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homeHelp);
		}

		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(createHelp);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(editHelp);
		}

		private void SettingsButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(settingsHelp);
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(loginHelp);
		}

		private void NewDbButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(newDbHelp);
		}

		private void PwdGenButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(pwdGenHelp);
		}

		private void AutofillButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(autofillHelp);
		}

		private void EncryptionButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(encryptionHelp);
		}

		private void TerminateButton_Click(object sender, RoutedEventArgs e)
		{ Close(); }
	}
}