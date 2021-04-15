using eKasa.Library.Config;

namespace eKasa.Core
{
	static public class Settings
	{
		readonly static public DbSettingsModel dbSettings = new();
		readonly static public AppSettingsModel appSettings = new();
		readonly static public SettingsManager<AppSettingsModel> appSettingsManager = new(ref appSettings, @"Settings\app_settings.xml");
		readonly static public Logger logger = new("error.log");
	}
}