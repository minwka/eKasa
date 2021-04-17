using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace eKasa.Core.UserControls
{
	public partial class AboutView : UserControl
	{
		public AboutView()
		{
			InitializeComponent();
			appname.Text = Assembly.GetExecutingAssembly().GetName().Name.ToString();
			version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			license.Text = "Developer";
		}

		private void VisitButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{ Process.Start("powershell", "start https://t.me/anth4"); }
	}
}