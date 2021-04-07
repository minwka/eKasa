using PasswordManager.Core.Models;

namespace PasswordManager.Core.Classes
{
	// Data Transfer Object
	static public class DTO
	{
		static public DatabaseModel InternalDB { get; set; }
		static public string FilePath { get; set; }
		static public string Password { get; set; }
	}
}