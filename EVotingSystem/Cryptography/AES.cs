using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EVotingSystem.Cryptography
{
    public class AES
    {
        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("0sjufcjbsoyzube6");

        /// <summary>
        /// Encrypt a string using a password
        /// </summary>
        /// <param name="plainText">The plain text which should get encrypted</param>
        /// <param name="passPhrase">The password which should be used for the encryption</param>
        /// <returns>The encrypted <see cref="plainText" /> as a Base64 string</returns>
        public static string Encrypt(string plainText, string passPhrase)
        {
            return Convert.ToBase64String(EncryptToBytes(plainText, passPhrase));
        }

        /// <summary>
        /// Encrypt a string using a password
        /// </summary>
        /// <param name="plainText">he plain text which should get encrypted</param>
        /// <param name="passPhrase">The password which should be used for the encryption</param>
        /// <returns>The encrypted <see cref="plainText" /></returns>
        public static byte[] EncryptToBytes(string plainText, string passPhrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);

            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            var cipherTextBytes = memoryStream.ToArray();
                            return cipherTextBytes;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt a byte array using a password
        /// </summary>
        /// <param name="cipherText">The encrypted data</param>
        /// <param name="passPhrase">The password the <see cref="cipherText" /> was encrypted with</param>
        /// <returns>The decrypted string</returns>
        public static string DecryptBytes(byte[] cipherText, string passPhrase)
        {
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);

            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[cipherText.Length];
                            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypt a string using a password
        /// </summary>
        /// <param name="cipherText">The encrypted string</param>
        /// <param name="passPhrase">The password the <see cref="cipherText" /> was encrypted with</param>
        /// <returns>The decrypted string</returns>
        public static string Decrypt(string cipherText, string passPhrase)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return string.Empty;
            }
            return DecryptBytes(Convert.FromBase64String(cipherText), passPhrase);
        }
    }
}