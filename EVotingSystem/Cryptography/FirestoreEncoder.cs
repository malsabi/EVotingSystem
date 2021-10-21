namespace EVotingSystem.Cryptography
{
    public static class FirestoreEncoder
    {
        public static string EncodeForFirebaseKey(string s)
        {
            return s
            .Replace("_", "__")
            .Replace(".", "_P")
            .Replace("$", "_D")
            .Replace("#", "_H")
            .Replace("[", "_O")
            .Replace("]", "_C")
            .Replace("/", "_S");
        }

        public static string DecodeFromFirebaseKey(string Encoded)
        {
            if (Encoded.Contains("__"))
            {
                Encoded = Encoded.Replace("__", "_");
            }
            else if (Encoded.Contains("_P"))
            {
                Encoded = Encoded.Replace("_P", ".");
            }
            else if (Encoded.Contains("_D"))
            {
                Encoded = Encoded.Replace("_D", "$");
            }
            else if (Encoded.Contains("_H"))
            {
                Encoded = Encoded.Replace("_H", "#");
            }
            else if (Encoded.Contains("_O"))
            {
                Encoded = Encoded.Replace("_O", "[");
            }
            else if (Encoded.Contains("_C"))
            {
                Encoded = Encoded.Replace("_C", "]");
            }
            else if (Encoded.Contains("_S"))
            {
                Encoded = Encoded.Replace("_S", "/");
            }
            return Encoded;
        }
    }
}