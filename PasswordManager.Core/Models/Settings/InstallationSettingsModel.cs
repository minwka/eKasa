using System.Xml.Serialization;

namespace PasswordManager.Core
{
	[XmlRoot(ElementName = "installation_settings")]
	public class InstallationSettingsModel
	{
		[XmlElement(DataType = "string", ElementName = "install_path")]
		public string InstallPath { get; set; }
	}
}
