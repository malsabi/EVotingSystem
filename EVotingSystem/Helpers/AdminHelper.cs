using EVotingSystem.Constants;
using EVotingSystem.Cryptography;

namespace EVotingSystem.Helpers
{
    public class AdminHelper
    {
        /// <summary>
        /// Encryptes the specific admin field
        /// </summary>
        /// <param name="Field">Represents the admin field</param>
        /// <returns>Returns encrypted AES for the specific admin field</returns>
        public static string EncryptField(string Field)
        {
            return FirestoreEncoder.EncodeForFirebaseKey(AES.Encrypt(Field, Config.Password));
        }

        /// <summary>
        /// Decrypts the specific encoded admin field
        /// </summary>
        /// <param name="EncodedField">Represents the encoded admin field</param>
        /// <returns>Retrusn decrypted AEs for the specific admin field</returns>
        public static string DecryptField(string EncodedField)
        {
            return AES.Decrypt(FirestoreEncoder.DecodeFromFirebaseKey(EncodedField), Config.Password);
        }
    }
}