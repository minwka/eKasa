using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PasswordManager.Core.Models;

namespace PasswordManager.Core.Classes
{
	static public class DbFunctions
	{
		static public void ReadJson(string filepath, ref string json)
		{ json = File.ReadAllText(filepath); }

		static public void WriteJson(ref string json, string filepath)
		{ File.WriteAllText(filepath, json); }

		static public void JsonToDb(ref string json, ref DatabaseModel db)
		{ db = JsonSerializer.Deserialize<DatabaseModel>(json); }

		static public void DbToJson(DatabaseModel db, ref string json)
		{ json = JsonSerializer.Serialize(db); }

		static public void EncryptEntries(ref List<EntryModel> plain, ref List<EntryModel> encrypted)
		{
			encrypted.Clear();
			foreach (var entry in plain) {
				var e = new EntryModel {
					guid = entry.guid,
					name = StringCipher.Encrypt(entry.name, DTO.Password),
					username = StringCipher.Encrypt(entry.username, DTO.Password),
					password = StringCipher.Encrypt(entry.password, DTO.Password),
					tag = StringCipher.Encrypt(entry.tag, DTO.Password)
				};
				encrypted.Add(e);
			}
		}

		static public void DecryptEntries(List<EntryModel> encrypted, ref List<EntryModel> plain)
		{
			plain.Clear();
			foreach (var entry in encrypted) {
				var p = new EntryModel {
					guid = entry.guid,
					name = StringCipher.Decrypt(entry.name, DTO.Password),
					username = StringCipher.Decrypt(entry.username, DTO.Password),
					password = StringCipher.Decrypt(entry.password, DTO.Password),
					tag = StringCipher.Decrypt(entry.tag, DTO.Password)
				};
				plain.Add(p);
			}
		}
	}
}