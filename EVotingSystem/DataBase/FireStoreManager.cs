using EVotingSystem.Constants;
using EVotingSystem.Models.Identity;
using EVotingSystem.Models.Candidate;
using EVotingSystem.Models.Student;
using EVotingSystem.Models.Admin;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EVotingSystem.DataBase
{
    /// <summary>
    /// A class that is responsible for handling GET/SET/UPDATE/REMOVE in the fire store database.
    /// A class that provides some useful functions for Signing-In/Signing-Out/Registering.
    /// A class that provides some useful functions for Candidate Insertion/Update/Remove.
    /// A class that provides some useful functions for Admin to control of the candidates/students,
    ///   and manage to have statistical information of the total votings from each candidate.
    /// FEATURES::
    /// 1. Real time update for any changes on the student fields -> also cookie session is updated.
    /// 2. All of the data is encrypted by AES and Custom Encoder in server side before posting to DB.
    /// 3. Fully Serialized using Google Protobuff.
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
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Student.StudentId);
                await Document.SetAsync(Student, SetOptions.Overwrite);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Gets the Student Information from the database
        /// </summary>
        /// <param name="StudentId">Represents the student Id</param>
        /// <returns>Returns the student if registered in the database otherwise null</returns>
        public async Task<StudentModel> GetStudent(string StudentId)
        {
            try
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
            catch
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
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                return SnapShot.Exists;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if the student is active and loggedin in the database
        /// </summary>
        /// <param name="StudentId">Represents the student id</param>
        /// <returns>Returns true if the student is online in the database otherwise false</returns>
        public async Task<bool> IsStudentOnline(string StudentId)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                if (SnapShot.Exists)
                {
                    return SnapShot.GetValue<string>("Status").Equals(Config.StudentOnline);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// Changes the status of the student to online, and updates the StaySignedIn depending on the user.
        /// </summary>
        /// <param name="Login">Represents the login model</param>
        public async void LoginStudent(LoginModel Login)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Login.StudentId);
                Dictionary<string, object> Fields = new Dictionary<string, object>
                {
                    { "Status", Config.StudentOnline },
                    { "StaySignedIn", Login.StaySignedIn }
                };
                await Document.UpdateAsync(Fields);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Changes the status of the student to Offline, and updates the StaySignedIn to false.
        /// </summary>
        /// <param name="StudentId">Represents the login model</param>
        public async void LogoutStudent(StudentModel Student)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Student.StudentId);
                Dictionary<string, object> Fields = new Dictionary<string, object>
                {
                    { "Status", Config.StudentOffline },
                    { "StaySignedIn", Config.StudentStayOffline }
                };
                await Document.UpdateAsync(Fields);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Gets the student phone number by providing the student Id
        /// </summary>
        /// <param name="StudentId">Represents the student Id</param>
        /// <returns>Returns the student phone number from the database if registered otherwise empty string</returns>
        public async Task<string> GetStudentPhone(string StudentId)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(StudentId);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                if (SnapShot.Exists)
                {
                    return SnapShot.GetValue<string>("Phone");
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// Updates the Student model in the database
        /// </summary>
        /// <param name="Student">Represents the student model</param>
        public async Task<WriteResult> UpdateStudent(StudentModel Student)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.StudentPath).Document(Student.StudentId);
                return await Document.SetAsync(Student, SetOptions.Overwrite);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets all of the students from the specific path in the database
        /// </summary>
        /// <returns>Returns a list of candidate model</returns>
        public async Task<List<StudentModel>> GetAllStudents(bool DecryptProperties = false)
        {
            CollectionReference Collection = FireStoreDataBase.Collection(Config.StudentPath);
            QuerySnapshot CandidatesQuerySnapShot = await Collection.GetSnapshotAsync();

            List<StudentModel> Students = new List<StudentModel>();

            foreach (DocumentSnapshot Document in CandidatesQuerySnapShot.Documents)
            {
                if (Document.Exists)
                {
                    StudentModel Student = Document.ConvertTo<StudentModel>();
                    if (DecryptProperties)
                    {
                        Student.DecryptProperties();
                        Students.Add(Student);
                    }
                    else
                    {
                        Students.Add(Student);
                    }
                }
            }
            return Students;
        }
        #endregion

        #region "Candidate"
        /// <summary>
        /// Adds the candidate into the database
        /// </summary>
        /// <param name="Candidate">Represents the Candidate Model</param>
        public async void AddCandidate(CandidateModel Candidate)
        {
            try
            {
                DocumentReference CandidateDocument = FireStoreDataBase.Collection(Config.CandidatePath).Document(Candidate.Id);
                await CandidateDocument.SetAsync(Candidate, SetOptions.Overwrite);

                CandidateVoteModel CandidateVote = new CandidateVoteModel()
                {
                    Id = Candidate.Id,
                    TotalVotes = "0",
                    StudentVoteCollection = null
                };

                DocumentReference CandidateVoteDocument = FireStoreDataBase.Collection(Config.CandidateVotePath).Document(Candidate.Id);
                await CandidateVoteDocument.SetAsync(CandidateVote, SetOptions.Overwrite);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Removes the candidate from the database
        /// </summary>
        /// <param name="Id">Represents the Id of the candidate</param>
        public async void RemoveCandidate(string Id)
        {
            DocumentReference CandidateDocument = FireStoreDataBase.Collection(Config.CandidatePath).Document(Id);
            await CandidateDocument.DeleteAsync();
            DocumentReference CandidateVoteDocument = FireStoreDataBase.Collection(Config.CandidateVotePath).Document(Id);
            await CandidateVoteDocument.DeleteAsync();
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
        /// <summary>
        /// Gets the Candidate Information from the database
        /// </summary>
        /// <param name="Id">Represents the Candidate Id</param>
        /// <returns>Returns the Candidate if added in the database otherwise null</returns>
        public async Task<CandidateModel> GetCandidate(string Id)
        {
            try
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
            catch
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
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.CandidatePath).Document(Id);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                return SnapShot.Exists;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Gets all of the candidates from the specific path in the database
        /// </summary>
        /// <returns>Returns a list of candidate model</returns>
        public async Task<List<CandidateModel>> GetAllCandidates(bool DecryptProperties = false)
        {
            CollectionReference Collection = FireStoreDataBase.Collection(Config.CandidatePath);
            QuerySnapshot CandidatesQuerySnapShot = await Collection.GetSnapshotAsync();

            List<CandidateModel> Candidates = new List<CandidateModel>();

            foreach (DocumentSnapshot Document in CandidatesQuerySnapShot.Documents)
            {
                CandidateModel Candidate = Document.ConvertTo<CandidateModel>();
                if (DecryptProperties)
                {
                    Candidate.DecryptProperties();
                    Candidates.Add(Candidate);
                }
                else
                {
                    Candidates.Add(Candidate);
                }
            }
            return Candidates;
        }
        /// <summary>
        /// Removes all of the candidates and also removes the candidate vote
        /// </summary>
        public async void RemoveAllCandidates()
        {
            CollectionReference Collection = FireStoreDataBase.Collection(Config.CandidatePath);
            QuerySnapshot CandidatesQuerySnapShot = await Collection.GetSnapshotAsync();
            foreach (DocumentSnapshot Document in CandidatesQuerySnapShot.Documents)
            {
                if (Document.Exists)
                {
                    RemoveCandidate(Document.Id);
                }
            }
        }
 
        public async void UpdateCandidates()
        {
            List<CandidateModel> Candidates = GetAllCandidates().Result;
            foreach (CandidateModel C in Candidates)
            {
                C.DecryptProperties();
                CandidateVoteModel CandidateVote = new CandidateVoteModel()
                {
                    Id = C.Id,
                    TotalVotes = "0",
                    StudentVoteCollection = null
                };
                CandidateVote.EncryptProperties();
                DocumentReference CandidateVoteDocument = FireStoreDataBase.Collection(Config.CandidateVotePath).Document(CandidateVote.Id);
                await CandidateVoteDocument.SetAsync(CandidateVote, SetOptions.Overwrite);
            }
        }
        #endregion

        #region "CandidateVote"
        /// <summary>
        /// Gets the candidate vote model from the CandidateVote Entity by the Id
        /// </summary>
        /// <param name="Id">Represents the Candidate vote Id</param>
        /// <returns>Returns Candidate Vote Model</returns>
        public async Task<CandidateVoteModel> GetCandidateVote(string Id)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.CandidateVotePath).Document(Id);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                if (SnapShot.Exists)
                {
                    return SnapShot.ConvertTo<CandidateVoteModel>();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Updates the specific candidate vote model in the database
        /// </summary>
        /// <param name="CandidateVote">Represents the candidate vote model</param>
        /// <returns>Returns if the task succeeded in updating the candidate vote model in the database</returns>
        public async Task<WriteResult> UpdateCandidateVote(CandidateVoteModel CandidateVote)
        {
            DocumentReference Document = FireStoreDataBase.Collection(Config.CandidateVotePath).Document(CandidateVote.Id);
            return await Document.SetAsync(CandidateVote, SetOptions.Overwrite);
        }
        /// <summary>
        /// Gets all of the votes from the specific path in the database
        /// </summary>
        /// <returns>Returns a list of candidate model</returns>
        public async Task<List<CandidateVoteModel>> GetAllCandidateVotes(bool DecryptProperties = false)
        {
            CollectionReference Collection = FireStoreDataBase.Collection(Config.CandidateVotePath);
            QuerySnapshot CandidatesQuerySnapShot = await Collection.GetSnapshotAsync();

            List<CandidateVoteModel> CandidateVotes = new List<CandidateVoteModel>();

            foreach (DocumentSnapshot Document in CandidatesQuerySnapShot.Documents)
            {
                CandidateVoteModel Vote = Document.ConvertTo<CandidateVoteModel>();
                if (DecryptProperties)
                {
                    Vote.DecryptProperties();
                    CandidateVotes.Add(Vote);
                }
                else
                {
                    CandidateVotes.Add(Vote);
                }
            }
            return CandidateVotes;
        }
        #endregion

        #region "Admin"
        /// <summary>
        /// Registers the admin into the database.
        /// </summary>
        /// <param name="Admin">Represents the Admin Model</param>
        public async void RegisterAdmin(AdminModel Admin)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.AdminPath).Document(Admin.Email);
                await Document.SetAsync(Admin, SetOptions.Overwrite);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Gets the Admin Information from the database
        /// </summary>
        /// <param name="Email">Represents the admin email address</param>
        /// <returns>Returns the admin if registered in the database otherwise null</returns>
        public async Task<AdminModel> GetAdmin(string Email)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.AdminPath).Document(Email);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                if (SnapShot.Exists)
                {
                    return SnapShot.ConvertTo<AdminModel>();
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Checks if the admin is registered in the database or not
        /// </summary>
        /// <param name="Email">Represents the admin email address</param>
        /// <returns>Returns true if the admin is registered in the database otherwise false.</returns>
        public async Task<bool> IsAdminRegistered(string Email)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.AdminPath).Document(Email);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                return SnapShot.Exists;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Checks if the admin is active and loggedin in the database
        /// </summary>
        /// <param name="Email">Represents the admin email address</param>
        /// <returns>Returns true if the admin is online in the database otherwise false</returns>
        public async Task<bool> IsAdminOnline(string Email)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.AdminPath).Document(Email);
                DocumentSnapshot SnapShot = await Document.GetSnapshotAsync();
                if (SnapShot.Exists)
                {
                    return SnapShot.GetValue<string>("Status").Equals(Config.AdminOnline);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
        /// <summary>
        /// Changes the status of the admin to online, and updates the StaySignedIn depending on the user.
        /// </summary>
        /// <param name="AccessPanel">Represents the access panel model</param>
        public async void LoginAdmin(AccessPanelModel AccessPanel)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.AdminPath).Document(AccessPanel.Email);
                Dictionary<string, object> Fields = new Dictionary<string, object>
                {
                    { "Status", Config.AdminOnline },
                    { "StaySignedIn", AccessPanel.StaySignedIn }
                };
                await Document.UpdateAsync(Fields);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Changes the status of the admin to Offline, and updates the StaySignedIn to false.
        /// </summary>
        /// <param name="AccessPanel">Represents the access panel model</param>
        public async void LogoutAdmin(AdminModel Admin)
        {
            try
            {
                DocumentReference Document = FireStoreDataBase.Collection(Config.AdminPath).Document(Admin.Email);
                Dictionary<string, object> Fields = new Dictionary<string, object>
                {
                    { "Status", Config.AdminOffline },
                    { "StaySignedIn", Config.AdminStayOffline }
                };
                await Document.UpdateAsync(Fields);
            }
            catch
            {
            }
        }
        #endregion
        #endregion
    }
}