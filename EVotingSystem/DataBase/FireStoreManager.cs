using EVotingSystem.Models;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVotingSystem.DataBase
{
    /// <summary>
    /// A class that is responsible for handling GET/SET/UPDATE/REMOVE in the fire store database.
    /// A class that provides some useful functions for Signing-In/Signing-Out/Registering.
    /// </summary>
    public class FireStoreManager
    {
        #region "Constants"
        private const string FireStoreKeyPath = "C:\\FirestoreKey\\FireStoreKey.json";
        private const string FireStoreProjectId = "evoting-148ce";
        #endregion

        #region "Fields"
        private FirestoreDb FireStoreDataBase;
        #endregion

        #region "Properties"
        #endregion

        #region "Constructor"
        public FireStoreManager()
        {
            InitializeFireStoreManager();
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Creates the environmental variable and initialized the FireStoreDataBase instance.
        /// </summary>
        private void InitializeFireStoreManager()
        {
            SetKeyAsEnvironmentVariable();
            FireStoreDataBase = FirestoreDb.Create(FireStoreProjectId);
        }
        /// <summary>
        /// Creates an environmental variable that can be accessed from any where, the value is the path of the credentials
        /// for accessing the fire store.
        /// </summary>
        private void SetKeyAsEnvironmentVariable()
        {
            if (Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") == null)
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", FireStoreKeyPath);
            }
        }
        #endregion

        #region "Public Methods"
        public async void AddRegisteredUser(SignUpModel SignUp)
        {
            DocumentReference Document = FireStoreDataBase.Collection("USERS").Document(SignUp.StudentId);
            await Document.SetAsync(SignUp, SetOptions.Overwrite);
        }

        public async Task<bool> IsUserRegistered(SignUpModel SignUp)
        {
            DocumentReference Document = FireStoreDataBase.Collection("USERS").Document(SignUp.StudentId);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            return SnapShot.Exists;
        }
        #endregion
    }
}