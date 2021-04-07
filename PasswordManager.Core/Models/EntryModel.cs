using System;
using System.Text.Json.Serialization;

namespace PasswordManager.Core.Models
{
	public class EntryModel
	{
		[JsonPropertyName("entry_guid")]
		public Guid guid { get; set; }
		[JsonPropertyName("entry_name")]
		public string name { get; set; }
		[JsonPropertyName("entry_username")]
		public string username { get; set; }
		[JsonPropertyName("entry_password")]
		public string password { get; set; }
		[JsonPropertyName("entry_tag")]
		public string tag { get; set; }
	}
}