using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace PasswordManager.Core.UserControls
{
	public partial class AboutUserControl : UserControl
	{
		public AboutUserControl()
		{
			InitializeComponent();
			appname.Text = Assembly.GetExecutingAssembly().GetName().Name.ToString();
			version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			license.Text = "Developer";
		}

		private void visitButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Process.Start("powershell", "start https://t.me/anth4");
		}
	}
}