using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PasswordManager.Core.Models
{
	public class DatabaseModel
	{
		[JsonPropertyName("database_name")]
		public string name { get; set; }
		[JsonPropertyName("owner_name")]
		public string owner { get; set; }
		[JsonPropertyName("password_hash")]
		public string pwd_hash { get; set; }
		[JsonPropertyName("last_modified")]
		public DateTime last_modified { get; set; }
		[JsonPropertyName("entries")]
		public List<EntryModel> entries { get; set; }
	}
}