namespace EVotingSystem.Constants
{
    public static class Config
    {
        /// <summary>
        /// Represents the session cookie name that will be installed in the client side.
        /// </summary>
        public static readonly string ConfirmationCookieName = "OTP_CODE";

        /// <summary>
        /// Represents the presistent cookie or session cookie for login
        /// </summary>
        public static readonly string IdentityCookieName = "LOGIN_INFO";


        public static readonly string FireStoreKeyPath = "C:\\FirestoreKey\\FireStoreKey.json";


        public static readonly string FireStoreProjectId = "evoting-148ce";


        public static readonly string StudentOnline = "Online";


        public static readonly string StudentOffline = "Offline";


        public static readonly string StudentPath = "STUDENTS";


        public static readonly string CandidatePath = "CANDIDATES";


        public static readonly string Password = "EVOTING2021-6-10";

        public static readonly int ConfirmCodeLength = 6;

        public static readonly string Vote = "Vote";

        public static readonly string UnVote = "UnVote";
    }
}