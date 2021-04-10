using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace PasswordManager.Library.Config
{
	public interface ISettings<T>
	{
		public void Save(string path, ref T model);
		public void Restore(string path, ref T model);
	}

	static public class SettingsManager<T>
	{
		static public void Restore(string path, ref T model)
		{
			if (File.Exists(path)) {
				var xs = new XmlSerializer(model.GetType());
				var xr = XmlReader.Create(path);
				model = (T)xs.Deserialize(xr);
				xr.Close();
			}
		}

		static public void Save(string path, ref T model)
		{
			var xs = new XmlSerializer(model.GetType());
			if (Path.GetDirectoryName(path) != "") {
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			var xw = XmlWriter.Create(path);
			xs.Serialize(xw, model);
			xw.Close();
		}
	}
}