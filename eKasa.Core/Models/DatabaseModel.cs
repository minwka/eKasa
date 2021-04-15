using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace eKasa.Core
{
	public class DatabaseModel
	{
		[JsonPropertyName("database_name")]
		public string Name { get; set; }

		[JsonPropertyName("owner_name")]
		public string Owner { get; set; }

		[JsonPropertyName("password_hash")]
		public string PwdHash { get; set; }

		[JsonPropertyName("last_modified_date")]
		public string ModifiedDate { get; set; }

		[JsonPropertyName("database_version")]
		public string Version { get; set; }

		[JsonPropertyName("salt")]
		public string Salt { get; set; }

		[JsonPropertyName("entry_list")]
		public List<EntryModel> Entries { get; set; }
	}
}