using Newtonsoft.Json;
using System;

namespace eKasa.Core {
	public record EntryModel {
		[JsonProperty("entry_id")]
		public Guid Id { get; set; }

		[JsonProperty("entry_name")]
		public string Name { get; set; }

		[JsonProperty("entry_tag")]
		public string Tag { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("password")]
		public string Password { get; set; }
	}
}