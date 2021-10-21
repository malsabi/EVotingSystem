namespace EVotingSystem.Constants
{
    public static class Config
    {
        #region "Cookie Constants"
        /// <summary>
        /// Represents the session cookie name that will be installed in the client side.
        /// </summary>
        public static readonly string ConfirmationCookieName = "OTP_CODE";

        /// <summary>
        /// Represents the presistent cookie or session cookie for student login
        /// </summary>
        public static readonly string StudentIdentityCookieName = "STUDENT_LOGIN_INFO";

        /// <summary>
        /// Reprents the persistent cookie or session cookie for admin login
        /// </summary>
        public static readonly string AdminIdentityCookieName = "ADMIN_LOGIN_INFO";
        #endregion

        #region "Firestore Constants"
        /// <summary>
        /// Represents the credentials of the firestore
        /// </summary>
        public static readonly string FireStoreKeyPath = "C:\\FirestoreKey\\FireStoreKey.json";

        /// <summary>
        /// Represents the project id in the firestore
        /// </summary>
        public static readonly string FireStoreProjectId = "evoting-148ce";

        /// <summary>
        /// Represents the status of the student in case online
        /// </summary>
        public static readonly string StudentOnline = "Online";

        /// <summary>
        /// Represents the status of the student in case offline
        /// </summary>
        public static readonly string StudentOffline = "Offline";

        /// <summary>
        /// Represents the path of the students in the firestore
        /// </summary>
        public static readonly string StudentPath = "STUDENTS";

        /// <summary>
        /// Represents the path of the candidates in the firestore
        /// </summary>
        public static readonly string CandidatePath = "CANDIDATES";

        /// <summary>
        /// Represents the path of the candidate vote in the firestore
        /// </summary>
        public static readonly string CandidateVotePath = "CANDIDATE-VOTE";

        /// <summary>
        /// Represents the path of the admins in the firestore
        /// </summary>
        public static readonly string AdminPath = "ADMINS";

        /// <summary>
        /// Represents the status of the admin in case online
        /// </summary>
        public static readonly string AdminOnline = "Online";

        /// <summary>
        /// Represents the status of the admin in case offline
        /// </summary>
        public static readonly string AdminOffline = "Offline";
        #endregion

        #region "AES Constants"
        /// <summary>
        /// Represents the private key for encrypting/decrypting
        /// </summary>
        public static readonly string Password = "EVOTING2021-6-10";
        #endregion

        #region "Confirmation Constants"
        /// <summary>
        /// Represents the length of the confirmation code
        /// </summary>
        public static readonly int ConfirmCodeLength = 6;

        /// <summary>
        /// Represents the timeout of the confirmation code which is 2 minutes
        /// </summary>
        public static readonly int ConfirmationCodeTimeout = 60 * 2;
        #endregion

        #region "Candidate Constants"
        /// <summary>
        /// Represents the maximum number of votes the student can apply
        /// </summary>
        public static readonly int MaximumStudentVotes = 2;
        #endregion
    }
}