namespace eKasa.Core
{
	public class DbSettingsModel
	{
		public DatabaseModel InternalDb = new();
		public string FilePath { get; set; }
		public string Password { get; set; }
	}
}
