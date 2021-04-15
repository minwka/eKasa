using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using static eKasa.Core.Settings;
using static eKasa.Library.Encryption.String;

namespace eKasa.Core
{
	static public class Database
	{
		static public DatabaseModel FromJson(string filePath)
		{ return JsonSerializer.Deserialize<DatabaseModel>(File.ReadAllText(filePath)); }

		static public string ToJson(ref DatabaseModel db)
		{ return JsonSerializer.Serialize(db); }

		static public DatabaseModel EncryptDatabase(ref DatabaseModel db, string key)
		{
			var dbm = new DatabaseModel() {
				Name = Encrypt(db.Name, dbSettings.Password),
				Owner = Encrypt(db.Owner, dbSettings.Password),
				PwdHash = db.PwdHash,
				ModifiedDate = Encrypt(db.ModifiedDate, dbSettings.Password),
				Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
				Salt = Convert.ToBase64String(sessionSalt),
				Entries = EncryptEntries(ref db, key)
			};
			return dbm;
		}

		static public DatabaseModel DecryptDatabase(ref DatabaseModel db)
		{
			var dbm = new DatabaseModel() {
				Name = Decrypt(db.Name, dbSettings.Password, dbSettings.InternalDb.Salt),
				Owner = Decrypt(db.Owner, dbSettings.Password, dbSettings.InternalDb.Salt),
				PwdHash = db.PwdHash,
				ModifiedDate = Decrypt(db.ModifiedDate, dbSettings.Password, dbSettings.InternalDb.Salt),
				Version = db.Salt,
				Salt = db.Salt,
				Entries = DecryptEntries(ref db)
			};
			return dbm;
		}

		static public List<EntryModel> EncryptEntries(ref DatabaseModel db, string key)
		{
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

		static public List<EntryModel> DecryptEntries(ref DatabaseModel db)
		{
			var entryList = new List<EntryModel>();
			if (db.Entries != null) {
				foreach (var entry in db.Entries) {
					var newEntry = new EntryModel {
						Id = entry.Id,
						Name = Decrypt(entry.Name, dbSettings.Password, dbSettings.InternalDb.Salt),
						Username = Decrypt(entry.Username, dbSettings.Password, dbSettings.InternalDb.Salt),
						Password = Decrypt(entry.Password, dbSettings.Password, dbSettings.InternalDb.Salt),
						Tag = Decrypt(entry.Tag, dbSettings.Password, dbSettings.InternalDb.Salt)
					};
					entryList.Add(newEntry);
				}
			}
			return entryList;
		}
	}
}