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
            return string.Format("<img class=\"card-img-top\" src=\"data:image/png;base64, {0}\" />", EncodedImage);
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
                return string.Format("<img class=\"card-img-top mx-auto\" style=\"width: {0}; height: {1};\" src=\"data:image/png;base64, {2}\" />", Width, Height, EncodedImage);
            }
            else
            {
                return string.Format("<img class=\"card-img-top\" style=\"width: {0}; height: {1};\" src=\"data:image/png;base64, {2}\" />", Width, Height, EncodedImage);
            }
        }
    }
}