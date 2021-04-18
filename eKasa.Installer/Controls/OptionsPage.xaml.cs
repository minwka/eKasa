using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;

namespace eKasa.Installer.Controls
{
	public partial class OptionsPage : UserControl
	{
		public OptionsPage()
		{ InitializeComponent(); }

		readonly VistaFolderBrowserDialog fbd = new();
		private void Browse_Click(object sender, RoutedEventArgs e)
		{
			try {
				fbd.RootFolder = Environment.SpecialFolder.ProgramFiles;
				fbd.Description = "Programın kaydedileceği konumu seçin!";
				fbd.UseDescriptionForTitle = true;
				fbd.ShowDialog();

				path.Text = Path.Combine(fbd.SelectedPath,"eKasa");
			} catch (Exception ex) {
				var canvas = (Canvas)Parent;
				var grid = (Grid)canvas.Parent;
				var window = (Window)grid.Parent;
				canvas.Children.Clear();
				canvas.Children.Add(MainWindow.ucerror);
				((MainWindow)window).next.IsEnabled = false;
				((MainWindow)window).back.IsEnabled = false;

				File.AppendAllText("error.log", $"Error Date: {DateTime.UtcNow.ToLocalTime()}\nError: {ex.Message}\nStack Trace: {ex.StackTrace}\nInner Exception: {ex.InnerException}\n\n");
			}
		}
	}
}