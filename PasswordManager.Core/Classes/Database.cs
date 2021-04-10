using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PasswordManager.Core
{
	static public class Database
	{
		static public DatabaseModel FromJson(string filePath)
		{ return JsonSerializer.Deserialize<DatabaseModel>(File.ReadAllText(filePath)); }

		static public string ToJson(ref DatabaseModel db)
		{ return JsonSerializer.Serialize(db); }

		static public List<EntryModel> EncryptEntries(ref DatabaseModel db)
		{
			var el = new List<EntryModel>();
			if (db.Entries != null) {
				foreach (var entry in db.Entries) {
					var e = new EntryModel {
						Id = entry.Id,
						Name = AES.Encrypt(entry.Name, Settings.dbSettings.Password),
						Username = AES.Encrypt(entry.Username, Settings.dbSettings.Password),
						Password = AES.Encrypt(entry.Password, Settings.dbSettings.Password),
						Tag = AES.Encrypt(entry.Tag, Settings.dbSettings.Password)
					};
					el.Add(e);
				}
			}
			return el;
		}

		static public List<EntryModel> DecryptEntries(ref DatabaseModel db)
		{
			var el = new List<EntryModel>();
			if (db.Entries != null) {
				foreach (var entry in db.Entries) {
					var e = new EntryModel {
						Id = entry.Id,
						Name = AES.Decrypt(entry.Name, Settings.dbSettings.Password),
						Username = AES.Decrypt(entry.Username, Settings.dbSettings.Password),
						Password = AES.Decrypt(entry.Password, Settings.dbSettings.Password),
						Tag = AES.Decrypt(entry.Tag, Settings.dbSettings.Password)
					};
					el.Add(e);
				}
			}
			return el;
		}
	}
}