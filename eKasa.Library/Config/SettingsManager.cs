using System.IO;
using System.Text.Json;

namespace eKasa.Library.Config
{
	static public class SettingsManager<T>
	{
		static public void Save(T settingsModel, string filePath)
		{
			if (Path.GetDirectoryName(filePath) != "") Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			File.WriteAllText(filePath, JsonSerializer.Serialize(settingsModel));
		}

		static public void Restore(ref T settingsModel, string filePath)
		{ if (File.Exists(filePath)) settingsModel = JsonSerializer.Deserialize<T>(File.ReadAllText(filePath)); }
	}
}