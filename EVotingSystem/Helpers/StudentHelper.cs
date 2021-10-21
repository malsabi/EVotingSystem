using EVotingSystem.Constants;
using EVotingSystem.Cryptography;

namespace EVotingSystem.Helpers
{
    public class StudentHelper
    {
        /// <summary>
        /// Encryptes the specific student field
        /// </summary>
        /// <param name="Field">Represents the student field</param>
        /// <returns>Returns encrypted AES for the specific student field</returns>
        public static string EncryptField(string Field)
        {
            return FirestoreEncoder.EncodeForFirebaseKey(AES.Encrypt(Field, Config.Password));
        }

        /// <summary>
        /// Decrypts the specific encoded student field
        /// </summary>
        /// <param name="EncodedField">Represents the encoded student field</param>
        /// <returns>Retrusn decrypted AEs for the specific student field</returns>
        public static string DecryptField(string EncodedField)
        {
            return AES.Decrypt(FirestoreEncoder.DecodeFromFirebaseKey(EncodedField), Config.Password);
        }
    }
}