using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Core
{
	public static class AES
	{
		readonly static public byte[] sessionSalt = GenerateRandomEntropy();
		public static string Encrypt(string message, string key)
		{
			byte[] clearBytes = Encoding.Unicode.GetBytes(message);
			byte[] salt = sessionSalt;
			using (Aes encryptor = Aes.Create()) {
				Rfc2898DeriveBytes pdb = new(key, salt);
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new()) {
					using (CryptoStream cs = new(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {
						cs.Write(clearBytes, 0, clearBytes.Length);
					}
					message = Convert.ToBase64String(ms.ToArray());
				}
			}
			return message;
		}

		public static string Decrypt(string message, string key)
		{
			message = message.Replace(" ", "+");
			byte[] salt = Convert.FromBase64String(Settings.dbSettings.InternalDb.Salt);
			byte[] cipherBytes = Convert.FromBase64String(message);
			using (Aes encryptor = Aes.Create()) {
				Rfc2898DeriveBytes pdb = new(key, salt);
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new()) {
					using (CryptoStream cs = new(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)) {
						cs.Write(cipherBytes, 0, cipherBytes.Length);
					}
					message = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return message;
		}

		private static byte[] GenerateRandomEntropy()
		{
			var randomBytes = new byte[32];
			using (var rngCsp = new RNGCryptoServiceProvider()) {
				rngCsp.GetBytes(randomBytes);
			}
			return randomBytes;
		}

		public static string Hash(string input)
		{
			using (SHA256 sha256Hash = SHA256.Create()) {
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++) {
					builder.Append(bytes[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
	}
}