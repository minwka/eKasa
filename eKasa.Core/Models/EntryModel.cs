using System;
using System.Text.Json.Serialization;

namespace eKasa.Core {
	public record EntryModel {
		[JsonPropertyName("entry_id")]
		public Guid Id { get; set; }

		[JsonPropertyName("entry_name")]
		public string Name { get; set; }

		[JsonPropertyName("entry_tag")]
		public string Tag { get; set; }

		[JsonPropertyName("username")]
		public string Username { get; set; }

		[JsonPropertyName("password")]
		public string Password { get; set; }
	}
}