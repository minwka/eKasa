namespace PasswordManager.Core
{
	public class DbSettingsModel
	{
		public DatabaseModel InternalDb = new();
		public string Path { get; set; }
		public string Password { get; set; }
	}
}
