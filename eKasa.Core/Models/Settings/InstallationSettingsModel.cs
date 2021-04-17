using System.Xml.Serialization;

namespace eKasa.Core
{
	[XmlRoot(ElementName = "installation_settings")]
	public class InstallationSettingsModel
	{
		[XmlElement(ElementName = "install_path")]
		public string InstallPath { get; set; }
	}
}
