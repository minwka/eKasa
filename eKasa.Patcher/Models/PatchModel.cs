using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eKasa.Patcher.Models
{
	public class PatchModel
	{
		[JsonPropertyName("branch")]
		public string Branch { get; set; }

		[JsonPropertyName("version")]
		public string Version { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("file_list")]
		public List<FileModel> FileList { get; set; }
	}
}