using System.Windows.Controls;

namespace eKasa.Core
{
	public partial class HomeHelp : UserControl
	{
		public HomeHelp()
		{
			InitializeComponent();

			var owner = string.IsNullOrEmpty(Database.InternalDb.Owner) ? "Kullanıcı" : Database.InternalDb.Owner;
			titleLabel.Text = $"Hoşgeldin, {owner}!";
		}
	}
}