using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using static eKasa.Core.GlobalSettings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core {
	static public class Database {
		static public DatabaseModel FromJson(string filePath) { return JsonSerializer.Deserialize<DatabaseModel>(File.ReadAllText(filePath)); }

		static public string ToJson(ref DatabaseModel db) { return JsonSerializer.Serialize(db, new JsonSerializerOptions() { WriteIndented = true }); }

		static public string ToJson(ref DatabaseModel db, string filePath) {
			var json = JsonSerializer.Serialize(db, new JsonSerializerOptions() { WriteIndented = true });
			File.WriteAllText(filePath, json);
			return json;
		}

		static public void Save(DatabaseModel db, string filePath) {
			db = EncryptDatabase(ref db, dbSettings.Password);
			ToJson(ref db, filePath);
		}

		static public void Restore(ref DatabaseModel db, string filePath) {
			db = FromJson(filePath);
			db = DecryptDatabase(ref db);
		}

		static public List<EntryModel> EncryptEntries(ref DatabaseModel db, string key) {
			var entryList = new List<EntryModel>();
			if (db.Entries != null) {
				foreach (var entry in db.Entries) {
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

		static public List<EntryModel> DecryptEntries(ref DatabaseModel db) {
			var entryList = new List<EntryModel>();
			if (db.Entries != null) {
				foreach (var entry in db.Entries) {
					var newEntry = new EntryModel {
						Id = entry.Id,
						Name = Decrypt(entry.Name, dbSettings.Password),
						Username = Decrypt(entry.Username, dbSettings.Password),
						Password = Decrypt(entry.Password, dbSettings.Password),
						Tag = Decrypt(entry.Tag, dbSettings.Password)
					};
					entryList.Add(newEntry);
				}
			}
			return entryList;
		}

		static public DatabaseModel EncryptDatabase(ref DatabaseModel db, string key) {
			var dbm = new DatabaseModel() {
				Name = Encrypt(db.Name, dbSettings.Password),
				Owner = Encrypt(db.Owner, dbSettings.Password),
				PwdHash = db.PwdHash,
				ModifiedDate = Encrypt(db.ModifiedDate, dbSettings.Password),
				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
				Entries = EncryptEntries(ref db, key)
			};
			return dbm;
		}

		static public DatabaseModel DecryptDatabase(ref DatabaseModel db) {
			var dbm = new DatabaseModel() {
				Name = Decrypt(db.Name, dbSettings.Password),
				Owner = Decrypt(db.Owner, dbSettings.Password),
				PwdHash = db.PwdHash,
				ModifiedDate = Decrypt(db.ModifiedDate, dbSettings.Password),
				Version = db.Version,
				Entries = DecryptEntries(ref db)
			};
			return dbm;
		}
	}
}