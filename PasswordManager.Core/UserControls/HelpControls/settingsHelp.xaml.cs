using System.Windows.Controls;

namespace PasswordManager.Core.UserControls.HelpControls
{
	public partial class SettingsHelp : UserControl
	{
		public SettingsHelp()
		{
			InitializeComponent();

			var owner = string.IsNullOrEmpty(Settings.dbSettings.InternalDb.Owner) ? "Kullanıcı" : Settings.dbSettings.InternalDb.Owner;
			titleLabel.Text = $"Ayarlar - {owner}";
		}
	}
}