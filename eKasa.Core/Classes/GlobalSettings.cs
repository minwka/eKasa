using System;
using System.IO;

namespace eKasa.Core
{
	static public class GlobalSettings
	{
		static public AppSettingsModel appSettings = new();

		readonly static public string appSettingsPath =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"eKasa\app_settings.json");

		readonly static public Logger logger = new("error.log");
	}
}