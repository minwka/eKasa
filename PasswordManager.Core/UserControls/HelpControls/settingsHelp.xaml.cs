using System.Windows.Controls;
using PasswordManager.Core.Classes;

namespace PasswordManager.Core.UserControls.HelpControls
{
	public partial class SettingsHelp : UserControl
	{
		public SettingsHelp()
		{
			InitializeComponent();

			var owner = DTO.InternalDB != null ? DTO.InternalDB.owner : "Kullanıcı";
			titleLabel.Text = $"Ayarlar - {owner}";
		}
	}
}