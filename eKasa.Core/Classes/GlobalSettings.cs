namespace eKasa.Core
{
	static public class GlobalSettings
	{
		static public AppSettingsModel appSettings = new();
		readonly static public string appSettingsPath = @"Settings\app_settings.json";
		readonly static public Logger logger = new("error.log");

		readonly static public DbSettingsModel dbSettings = new();
	}
}