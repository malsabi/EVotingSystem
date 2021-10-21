using EVotingSystem.Constants;
using EVotingSystem.Cryptography;

namespace EVotingSystem.Helpers
{
    public class CandidateHelper
    {
        /// <summary>
        /// Converts Base64 into an Html Img tag embedded with base64 encoded image
        /// </summary>
        /// <param name="EncodedImage">Encoded base64 image</param>
        /// <returns>Returns Html Img tag</returns>
        public static string GetCandidateHtmlImage(string EncodedImage)
        {
            return string.Format("<img class=\"card-img-top rounded img-thumbnail\" src=\"data:image/png;base64, {0}\" />", EncodedImage);
        }
        /// <summary>
        /// Converts Base64 into an Html Img tag embedded with base64 encoded image with the support of width and height
        /// </summary>
        /// <param name="Width">Represents the image width</param>
        /// <param name="Height">Represents the image height</param>
        /// <param name="EncodedImage">Encoded base64 image</param>
        /// <returns>Returns Html Img tag</returns>
        public static string GetCandidateHtmlImage(string Width, string Height, string EncodedImage, bool CenterImage = false) 
        {
            if (CenterImage)
            {
                return string.Format("<img class=\"card-img-top rounded img-thumbnail mx-auto\" style=\"width: {0}; height: {1};\" src=\"data:image/png;base64, {2}\" />", Width, Height, EncodedImage);
            }
            else
            {
                return string.Format("<img class=\"card-img-top rounded img-thumbnail\" style=\"width: {0}; height: {1};\" src=\"data:image/png;base64, {2}\" />", Width, Height, EncodedImage);
            }
        }

        /// <summary>
        /// Encryptes the specific candidate field
        /// </summary>
        /// <param name="Field">Represents the candidate field</param>
        /// <returns>Returns encrypted AES for the specific candidate field</returns>
        public static string EncryptField(string Field)
        {
            return FirestoreEncoder.EncodeForFirebaseKey(AES.Encrypt(Field, Config.Password));
        }

        /// <summary>
        /// Decrypts the specific encoded candidate field
        /// </summary>
        /// <param name="EncodedField">Represents the encoded candidate field</param>
        /// <returns>Retrusn decrypted AEs for the specific candidate field</returns>
        public static string DecryptField(string EncodedField)
        {
            return AES.Decrypt(FirestoreEncoder.DecodeFromFirebaseKey(EncodedField), Config.Password);
        }
    }
}