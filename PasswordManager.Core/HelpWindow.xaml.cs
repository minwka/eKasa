using System.Windows;
using System.Windows.Input;
using PasswordManager.Core.UserControls.HelpControls;

namespace PasswordManager.Core
{
	public partial class HelpWindow : Window
	{
		HomeHelp homeHelp = new();
		CreateHelp createHelp = new();
		EditHelp editHelp = new();
		SettingsHelp settingsHelp = new();
		LoginHelp loginHelp = new();
		NewDbHelp newDbHelp = new();
		PwdGenHelp pwdGenHelp = new();
		AutofillHelp autofillHelp = new();
		EncryptionHelp encryptionHelp = new();

		public HelpWindow()
		{
			InitializeComponent();
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homeHelp);
		}

		private void mainWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left) {
				DragMove();
			}
		}

		private void homeButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(homeHelp);
		}

		private void addButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(createHelp);
		}

		private void editButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(editHelp);
		}

		private void settingsButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(settingsHelp);
		}

		private void loginButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(loginHelp);
		}

		private void newDbButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(newDbHelp);
		}

		private void pwdGenButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(pwdGenHelp);
		}

		private void autofillButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(autofillHelp);
		}

		private void encryptionButton_Click(object sender, RoutedEventArgs e)
		{
			contentCanvas.Children.Clear();
			contentCanvas.Children.Add(encryptionHelp);
		}

		private void terminateButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}