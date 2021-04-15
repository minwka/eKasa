using System.Xml.Serialization;

namespace eKasa.Core
{
	[XmlRoot(ElementName = "application_settings")]
	public class AppSettingsModel
	{
		[XmlElement(DataType = "string", ElementName = "last_db_location")]
		public string LastDbLocation { get; set; }
	}
}
