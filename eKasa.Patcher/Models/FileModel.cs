using System.Text.Json.Serialization;

namespace eKasa.Patcher.Models
{
	public class FileModel
	{
		[JsonPropertyName("path")]
		public string Path { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("hash")]
		public string Hash { get; set; }

		[JsonPropertyName("source")]
		public string Source { get; set; }
	}
}