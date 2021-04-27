using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core
{
	static public class Database
	{
		static public DatabaseModel InternalDb = new();
		static public string FilePath { get; set; }
		static public string Password { get; set; }


		static public DatabaseModel FromJson(string filepath)
		{ return JsonConvert.DeserializeObject<DatabaseModel>(File.ReadAllText(filepath)); }

		static public void ToJson(DatabaseModel dbm, string filepath)
		{ File.WriteAllText(filepath, JsonConvert.SerializeObject(dbm, Formatting.Indented)); }


		static public List<EntryModel> EncryptEntries(DatabaseModel dbm, string key)
		{
			var entryList = new List<EntryModel>();
			if (dbm.Entries != null) {
				foreach (var entry in dbm.Entries) {
					var newEntry = new EntryModel {
						Id = entry.Id,
						Name = Encrypt(entry.Name, key),
						Username = Encrypt(entry.Username, key),
						Password = Encrypt(entry.Password, key),
						Tag = Encrypt(entry.Tag, key)
					};
					entryList.Add(newEntry);
				}
			}
			return entryList;
		}

		static public List<EntryModel> DecryptEntries(DatabaseModel dbm, string key)
		{
			var entryList = new List<EntryModel>();
			if (dbm.Entries != null) {
				foreach (var entry in dbm.Entries) {
					var newEntry = new EntryModel {
						Id = entry.Id,
						Name = Decrypt(entry.Name, key),
						Username = Decrypt(entry.Username, key),
						Password = Decrypt(entry.Password, key),
						Tag = Decrypt(entry.Tag, key)
					};
					entryList.Add(newEntry);
				}
			}
			return entryList;
		}


		static public DatabaseModel EncryptDatabase(DatabaseModel dbm, string key)
		{
			var idbm = new DatabaseModel() {
				Name = Encrypt(dbm.Name, key),
				Owner = Encrypt(dbm.Owner, key),
				PwdHash = dbm.PwdHash,
				ModifiedDate = Encrypt(dbm.ModifiedDate, key),
				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
				Entries = EncryptEntries(dbm, key)
			};
			return idbm;
		}

		static public DatabaseModel DecryptDatabase(DatabaseModel dbm, string key)
		{
			var idbm = new DatabaseModel() {
				Name = Decrypt(dbm.Name, key),
				Owner = Decrypt(dbm.Owner, key),
				PwdHash = dbm.PwdHash,
				ModifiedDate = Decrypt(dbm.ModifiedDate, key),
				Version = dbm.Version,
				Entries = DecryptEntries(dbm, key)
			};
			return idbm;
		}


		static public void Save()
		{ ToJson(EncryptDatabase(InternalDb, Password), FilePath); }

		static public void Restore()
		{ InternalDb = DecryptDatabase(FromJson(FilePath), Password); }
	}
}