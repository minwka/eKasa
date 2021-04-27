using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace eKasa.Library.Encryption
{
	public static class String
	{
		private const int Keysize = 256;
		private const int DerivationIterations = 1000;

		static public string Encrypt(string message, string key)
		{
			var saltStringBytes = Generate128BitsOfRandomEntropy();
			var ivStringBytes = Generate128BitsOfRandomEntropy();
			var plainTextBytes = Encoding.Unicode.GetBytes(message);
			using var password = new Rfc2898DeriveBytes(key, saltStringBytes, DerivationIterations); var keyBytes = password.GetBytes(Keysize / 16);
			using var symmetricKey = new RijndaelManaged(); symmetricKey.BlockSize = 128;
			symmetricKey.Mode = CipherMode.CBC;
			symmetricKey.Padding = PaddingMode.PKCS7;
			using var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes); using var memoryStream = new MemoryStream(); using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write); cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();
			var cipherTextBytes = saltStringBytes;
			cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
			cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(cipherTextBytes);
		}

		static public string Decrypt(string message, string key)
		{
			var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(message);
			var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 16).ToArray();
			var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 16).Take(Keysize / 16).ToArray();
			var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 16) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 16) * 2)).ToArray();

			using var password = new Rfc2898DeriveBytes(key, saltStringBytes, DerivationIterations); var keyBytes = password.GetBytes(Keysize / 16);
			using var symmetricKey = new RijndaelManaged(); symmetricKey.BlockSize = 128;
			symmetricKey.Mode = CipherMode.CBC;
			symmetricKey.Padding = PaddingMode.PKCS7;
			using var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes); using var memoryStream = new MemoryStream(cipherTextBytes); using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read); var plainTextBytes = new byte[cipherTextBytes.Length];
			var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.Unicode.GetString(plainTextBytes, 0, decryptedByteCount);
		}

		static private byte[] Generate128BitsOfRandomEntropy()
		{
			var randomBytes = new byte[16];
			using (var rngCsp = new RNGCryptoServiceProvider()) {
				rngCsp.GetBytes(randomBytes);
			}
			return randomBytes;
		}

		static public string Sha256(string input)
		{
			using SHA256 sha256Hash = SHA256.Create(); byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			StringBuilder builder = new();
			for (int i = 0; i < bytes.Length; i++) {
				builder.Append(bytes[i].ToString("x2"));
			}
			return builder.ToString();
		}
	}
}