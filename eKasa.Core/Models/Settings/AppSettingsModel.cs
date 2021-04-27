using System.Text.Json.Serialization;

namespace eKasa.Core
{
	public record AppSettingsModel
	{
		[JsonPropertyName("remember_db")]
		public bool RememberLastDb { get; set; }

		[JsonPropertyName("last_db")]
		public string LastDbLocation { get; set; }
	}
}