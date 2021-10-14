using EVotingSystem.Constants;
using EVotingSystem.Models;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVotingSystem.DataBase
{
    /// <summary>
    /// A class that is responsible for handling GET/SET/UPDATE/REMOVE in the fire store database.
    /// A class that provides some useful functions for Signing-In/Signing-Out/Registering.
    /// </summary>
    public class FireStoreManager
    {
        #region "Fields"
        private FirestoreDb FireStoreDataBase;
        #endregion

        #region "Constructor"
        public FireStoreManager()
        {
            InitializeFireStoreManager();
        }
        #endregion

        #region "Events"
        public delegate void OnStudentFieldsUpdatedDelegate(StudentModel Student);
        public event OnStudentFieldsUpdatedDelegate OnStudentFieldsUpdated;
        #endregion

        #region "Handlers"
        public void SetOnStudentFieldsUpdated(StudentModel Student)
        {
            OnStudentFieldsUpdated?.Invoke(Student);
        }
        #endregion

        #region "Private Methods"
        /// <summary>
        /// Creates the environmental variable and initialized the FireStoreDataBase instance.
        /// </summary>
        private void InitializeFireStoreManager()
        {
            SetKeyAsEnvironmentVariable();
            FireStoreDataBase = FirestoreDb.Create(Config.FireStoreProjectId);
        }
        /// <summary>
        /// Creates an environmental variable that can be accessed from any where, the value is the path of the credentials
        /// for accessing the fire store.
        /// </summary>
        private void SetKeyAsEnvironmentVariable()
        {
            if (Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS") == null)
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Config.FireStoreKeyPath);
            }
        }
        /// <summary>
        /// Handles the fields updated and raise an event when a student model changes with the new fields.
        /// </summary>
        /// <param name="StudentSnapShot">Represents the document received from the database containing the student model</param>
        private void StudentFieldsUpdatedHandler(DocumentSnapshot StudentSnapShot)
        {
            if (StudentSnapShot.Exists)
            {
                SetOnStudentFieldsUpdated(StudentSnapShot.ConvertTo<StudentModel>());
            }
        }
        #endregion

        #region "Public Methods"
        #region "Student"
        /// <summary>
        /// Listens on new changes on a specific student in the database
        /// </summary>
        /// <param name="StudentId">Represents Student Id</param>
        public void ListenOnStudentFieldsUpdated(string StudentId)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
            Document.Listen(StudentFieldsUpdatedHandler);
        }
        /// <summary>
        /// Registers the student into the database.
        /// </summary>
        /// <param name="Student">Represents the Student Model</param>
        public async void RegisterStudent(StudentModel Student)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Student.StudentId);
            await Document.SetAsync(Student, SetOptions.Overwrite);
        }
        /// <summary>
        /// Gets the Student Information from the database
        /// </summary>
        /// <param name="StudentId">Represents the student Id</param>
        /// <returns>Returns the student if registered in the database otherwise null</returns>
        public async Task<StudentModel> GetStudent(string StudentId)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            if (SnapShot.Exists)
            {
                return SnapShot.ConvertTo<StudentModel>();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Checks if the student is registered in the database or not
        /// </summary>
        /// <param name="StudentId">Represents the student Id</param>
        /// <returns>Returns true if the student is registered in the database otherwise false.</returns>
        public async Task<bool> IsStudentRegistered(string StudentId)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            return SnapShot.Exists;
        }
        /// <summary>
        /// Checks if the student is active and loggedin in the database
        /// </summary>
        /// <param name="StudentId">Represents the student id</param>
        /// <returns>Returns true if the student is online in the database otherwise false</returns>
        public async Task<bool> IsStudentOnline(string StudentId)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            if (SnapShot.Exists)
            {
                return SnapShot.GetValue<string>("Status").Equals(Config.StudentOnline);
            }
            return false;
        }
        /// <summary>
        /// Changes the status of the student to online, and updates the StaySignedIn depending on the user.
        /// </summary>
        /// <param name="Login">Represents the login model</param>
        public async void LoginStudent(LoginModel Login)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Login.StudentId);

            Dictionary<string, object> Fields = new Dictionary<string, object>
            {
                { "Status", Config.StudentOnline },
                { "StaySignedIn", Login.StaySignedIn }
            };

            await Document.UpdateAsync(Fields);
        }
        /// <summary>
        /// Changes the status of the student to Offline, and updates the StaySignedIn to false.
        /// </summary>
        /// <param name="StudentId">Represents the login model</param>
        public async void LogoutStudent(StudentModel Student)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Student.StudentId);

            Dictionary<string, object> Fields = new Dictionary<string, object>
            {
                { "Status", Config.StudentOffline },
                { "StaySignedIn", "false" }
            };

            await Document.UpdateAsync(Fields);
        }
        /// <summary>
        /// Gets the student phone number by providing the student Id
        /// </summary>
        /// <param name="StudentId">Represents the student Id</param>
        /// <returns>Returns the student phone number from the database if registered otherwise empty string</returns>
        public async Task<string> GetStudentPhone(string StudentId)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            if (SnapShot.Exists)
            {
                try
                {
                    return SnapShot.GetValue<string>("Phone");
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
        /// <summary>
        /// Updates the Student model in the database
        /// </summary>
        /// <param name="Student">Represents the student model</param>
        public async Task<WriteResult> UpdateStudent(StudentModel Student)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Student.StudentId);
            return await Document.SetAsync(Student, SetOptions.Overwrite);
        }
        #endregion
        #region "Candidate"
        /// <summary>
        /// Adds the candidate into the database
        /// </summary>
        /// <param name="Candidate">Represents the Candidate Model</param>
        public async void AddCandidate(CandidateModel Candidate)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.CandidatePath).Document(Candidate.Id);
            await Document.SetAsync(Candidate, SetOptions.Overwrite);
        }
        /// <summary>
        /// Gets the Candidate Information from the database
        /// </summary>
        /// <param name="Id">Represents the Candidate Id</param>
        /// <returns>Returns the Candidate if added in the database otherwise null</returns>
        public async Task<CandidateModel> GetCandidate(string Id)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.CandidatePath).Document(Id);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            if (SnapShot.Exists)
            {
                return SnapShot.ConvertTo<CandidateModel>();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Checks if the candidate is added in the database or not
        /// </summary>
        /// <param name="Id">Represents the candidate Id</param>
        /// <returns>Returns true if the candidate is added in the database otherwise false.</returns>
        public async Task<bool> IsCandidateAdded(string Id)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.CandidatePath).Document(Id);
            DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
            return SnapShot.Exists;
        }
        /// <summary>
        /// Gets all of the candidates from the specific path in the database
        /// </summary>
        /// <returns>Returns a list of candidate model</returns>
        public async Task<List<CandidateModel>> GetAllCandidates()
        {
            CollectionReference Collection = FireStoreDataBase.Collection(Config.CandidatePath);
            QuerySnapshot CandidatesQuerySnapShot = await Collection.GetSnapshotAsync();

            List<CandidateModel> Candidates = new List<CandidateModel>();

            foreach (DocumentSnapshot Document in CandidatesQuerySnapShot.Documents)
            {
                if (Document.Exists)
                {
                    Candidates.Add(Document.ConvertTo<CandidateModel>());
                }
            }
            return Candidates;
        }
        /// <summary>
        /// Updates the Candidate model in the database
        /// </summary>
        /// <param name="Candidate">Represents the candidate model</param>
        public async Task<WriteResult> UpdateCandidate(CandidateModel Candidate)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.CandidatePath).Document(Candidate.Id);
            return await Document.SetAsync(Candidate, SetOptions.Overwrite);
        }
        #endregion
        #endregion
    }
}