using System.Windows.Controls;
using PasswordManager.Core.Classes;

namespace PasswordManager.Core.UserControls.HelpControls
{
	public partial class HomeHelp : UserControl
	{
		public HomeHelp()
		{
			InitializeComponent();

			var owner = DTO.InternalDB == null ? "Kullanıcı" : DTO.InternalDB.owner;
			titleLabel.Text = $"Hoşgeldin, {owner}!";
		}
	}
}