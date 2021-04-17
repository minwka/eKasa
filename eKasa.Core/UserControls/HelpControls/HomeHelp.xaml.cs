using System.Windows.Controls;

namespace eKasa.Core
{
	public partial class HomeHelp : UserControl
	{
		public HomeHelp()
		{
			InitializeComponent();

			var owner = string.IsNullOrEmpty(GlobalSettings.dbSettings.InternalDb.Owner) ? "Kullanıcı" : GlobalSettings.dbSettings.InternalDb.Owner;
			titleLabel.Text = $"Hoşgeldin, {owner}!";
		}
	}
}