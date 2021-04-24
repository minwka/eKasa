using Newtonsoft.Json;
using System.Collections.Generic;

namespace eKasa.Core {
	public record DatabaseModel {
		[JsonProperty("database_name")]
		public string Name { get; set; }

		[JsonProperty("owner_name")]
		public string Owner { get; set; }

		[JsonProperty("password_hash")]
		public string PwdHash { get; set; }

		[JsonProperty("modified_date")]
		public string ModifiedDate { get; set; }

		[JsonProperty("app_version")]
		public string Version { get; set; }

		[JsonProperty("entry_list")]
		public List<EntryModel> Entries { get; set; }
	}
}