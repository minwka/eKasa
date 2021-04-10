using System.Windows.Controls;

namespace PasswordManager.Core.UserControls.HelpControls
{
	public partial class HomeHelp : UserControl
	{
		public HomeHelp()
		{
			InitializeComponent();

			var owner = string.IsNullOrEmpty(Settings.dbSettings.InternalDb.Owner) ? "Kullanıcı" : Settings.dbSettings.InternalDb.Owner;
			titleLabel.Text = $"Hoşgeldin, {owner}!";
		}
	}
}