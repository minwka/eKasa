using System.Xml.Serialization;

namespace PasswordManager.Core
{
	[XmlRoot(ElementName = "application_settings")]
	public class AppSettingsModel
	{
		[XmlElement(DataType = "string", ElementName = "last_db_location")]
		public string LastDbLocation { get; set; }
	}
}
