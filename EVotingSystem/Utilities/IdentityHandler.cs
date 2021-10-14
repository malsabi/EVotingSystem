using EVotingSystem.Constants;
using EVotingSystem.Cryptography;
using EVotingSystem.DataBase;
using EVotingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace EVotingSystem.Utilities
{
    public class IdentityHandler 
    {
        #region "Fields"
        private readonly CookieHandler CookieHandler;
        private readonly FireStoreManager FireStore;
        #endregion


        #region "Constructor"
        public IdentityHandler()
        {
            CookieHandler = null;
            FireStore = new FireStoreManager();
        }

        public IdentityHandler(Controller Controller)
        {
            CookieHandler = new CookieHandler(Controller);
            FireStore = new FireStoreManager();
        }
        #endregion

        #region "Handlers"
        private void FireStoreOnStudentFieldsUpdated(StudentModel Student)
        {
            if (CookieHandler.ContainsCookie(Config.IdentityCookieName))
            {
                //Create Cookie value that will contain the login information encrypted by AES and serialized into JSON
                string EncryptedJsonString = AES.Encrypt(JsonSerializer.Serialize(Student), Config.Password);

                //Register the identity cookie that will contain the encrypted json student model.
                CookieHandler.RegisterCookie(Config.IdentityCookieName, EncryptedJsonString, Student.ExpiryDate);
            }
        }
        #endregion


        #region "Private Methods"
        public void HandleStudentListener()
        {
            string Id = GetStudentId();
            if (Id != null)
            {
                FireStore.ListenOnStudentFieldsUpdated(Id);
                FireStore.OnStudentFieldsUpdated += FireStoreOnStudentFieldsUpdated;
            }
        }
        #endregion


        #region "Public Methods"
        public bool IsUserLoggedIn(HttpRequest Request)
        {
            if (Request.Cookies.ContainsKey(Config.IdentityCookieName))
            {
                if (IsStudentSessionValid(Request))
                {
                    return true;
                }
                else
                {
                    LogOut();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsUserLoggedIn()
        {
            if (CookieHandler.ContainsCookie(Config.IdentityCookieName))
            {
                if (IsStudentSessionValid())
                {
                    return true;
                }
                else
                {
                    LogOut();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool IsUserLoggedOut()
        {
            return CookieHandler.ContainsCookie(Config.IdentityCookieName) == false;
        }

        public void LogIn(LoginModel Login)
        {
            DateTime Expires;
            if (Login.StaySignedIn.Equals("true"))
            {
                //Persistant cookie that will stay for 30 days ~ 1 month.
                //The user will remain signed in for 30 days.
                Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                //Session cookie that will stay for only 1 day.
                //The user will logout after 1 day.
                Expires = DateTime.Now.AddDays(1);
            }

            //Get the student model
            StudentModel Student = FireStore.GetStudent(Login.StudentId).Result;

            //Set the expiration date for the account session to log out.
            Student.ExpiryDate = DateTime.SpecifyKind(Expires, DateTimeKind.Utc);

            //Create Cookie value that will contain the login information encrypted by AES and serialized into JSON
            string EncryptedJsonString = AES.Encrypt(JsonSerializer.Serialize(Student), Config.Password);

            //Register the identity cookie that will contain the encrypted json student model.
            CookieHandler.RegisterCookie(Config.IdentityCookieName, EncryptedJsonString, Expires);            
        }

        public void LogOut()
        {
            if (CookieHandler.ContainsCookie(Config.IdentityCookieName))
            {
                CookieHandler.DeleteCookie(Config.IdentityCookieName);
            }
        }

        public StudentModel StudentSession()
        {
            if (CookieHandler.ContainsCookie(Config.IdentityCookieName))
            {
                string EncryptedJsonString = CookieHandler.GetCookieValue(Config.IdentityCookieName);
                StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                return FireStore.GetStudent(Student.StudentId).Result;
            }
            return null;
        }

        public bool IsStudentSessionValid()
        {
            if (CookieHandler.ContainsCookie(Config.IdentityCookieName))
            {
                string EncryptedJsonString = CookieHandler.GetCookieValue(Config.IdentityCookieName);
                StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                return FireStore.IsStudentRegistered(Student.StudentId).Result;
            }
            else
            {
                return false;
            }
        }

        private bool IsStudentSessionValid(HttpRequest request)
        {
            if (request.Cookies.ContainsKey(Config.IdentityCookieName))
            {
                string EncryptedJsonString = request.Cookies[Config.IdentityCookieName];
                StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                return FireStore.IsStudentRegistered(Student.StudentId).Result;
            }
            else
            {
                return false;
            }
        }

        public void SetConfirmationCode(string ConfirmCode)
        {
            //The Confirmation code expires after 1 minute
            DateTime Expires = DateTime.Now.AddMinutes(1);
            //Create Cookie value that will contain the confirmation code encrypted by AES
            string EncryptedConfirmCode = AES.Encrypt(ConfirmCode, Config.Password);
            //Register the confirmation cookie that will contain the encrypted confirmation code.
            CookieHandler.RegisterCookie(Config.ConfirmationCookieName, EncryptedConfirmCode, Expires);
        }

        public string GetConfirmationCode()
        {
            if (CookieHandler.ContainsCookie(Config.ConfirmationCookieName))
            {
                return AES.Decrypt(CookieHandler.GetCookieValue(Config.ConfirmationCookieName), Config.Password);
            }
            return null;
        }

        public void DeleteConfirmationCode()
        {
            if (CookieHandler.ContainsCookie(Config.ConfirmationCookieName))
            {
                CookieHandler.DeleteCookie(Config.ConfirmationCookieName);
            }
        }

        public string GetStudentId()
        {
            StudentModel Student = StudentSession();

            if (Student != null)
            {
                return Student.StudentId;
            }
            return null;
        }
        #endregion

        #region "Static Methods"
        public static StudentModel StudentSession(HttpRequest Request)
        {
            if (Request.Cookies.ContainsKey(Config.IdentityCookieName))
            {
                string EncryptedJsonString = Request.Cookies[Config.IdentityCookieName];
                StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                return new FireStoreManager().GetStudent(Student.StudentId).Result;
            }
            return null;
        }
        #endregion
    }
}