using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace eKasa.Library.Config
{
	public class SettingsManager<T>
	{
		public T Model { get; set; }
		public string Filepath { get; init; }

		public SettingsManager(ref T model, string filepath)
		{
			Model = model;
			Filepath = filepath;
		}

		public void Save()
		{
			var xs = new XmlSerializer(Model.GetType());
			if (Path.GetDirectoryName(Filepath) != "") {
				Directory.CreateDirectory(Path.GetDirectoryName(Filepath));
			}
			var xw = XmlWriter.Create(Filepath);
			xs.Serialize(xw, Model);
			xw.Close();
		}

		public void Restore()
		{
			var xs = new XmlSerializer(Model.GetType());
			if (File.Exists(Filepath)) {
				var xr = XmlReader.Create(Filepath);
				Model = (T)xs.Deserialize(xr);
				xr.Close();
			}
		}
	}
}