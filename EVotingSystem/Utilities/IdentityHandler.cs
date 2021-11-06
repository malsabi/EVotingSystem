using EVotingSystem.Constants;
using EVotingSystem.Cryptography;
using EVotingSystem.DataBase;
using EVotingSystem.Models.Identity;
using EVotingSystem.Models.Student;
using EVotingSystem.Models.Admin;
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
            if (CookieHandler.ContainsCookie(Config.StudentIdentityCookieName))
            {
                //Create Cookie value that will contain the login information encrypted by AES and serialized into JSON
                string EncryptedJsonString = AES.Encrypt(JsonSerializer.Serialize(Student), Config.Password);

                //Register the identity cookie that will contain the encrypted json student model.
                CookieHandler.RegisterCookie(Config.StudentIdentityCookieName, EncryptedJsonString, Student.ExpiryDate);
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
        #region "Student Session Logout/Login"
        public bool IsStudentLoggedIn(HttpRequest Request, HttpResponse Response)
        {
            if (Request.Cookies.ContainsKey(Config.StudentIdentityCookieName))
            {
                if (IsStudentSessionValid(Request))
                {
                    return true;
                }
                else
                {
                    LogoutStudent(Request, Response);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsStudentLoggedIn()
        {
            if (CookieHandler.ContainsCookie(Config.StudentIdentityCookieName))
            {
                if (IsStudentSessionValid())
                {
                    return true;
                }
                else
                {
                    LogoutStudent();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsStudentLoggedOut()
        {
            return CookieHandler.ContainsCookie(Config.StudentIdentityCookieName) == false;
        }
        public void LoginStudent(LoginModel Login)
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
            CookieHandler.RegisterCookie(Config.StudentIdentityCookieName, EncryptedJsonString, Expires);
        }
        public void LogoutStudent(HttpRequest Request, HttpResponse Response)
        {
            if (Request.Cookies.ContainsKey(Config.StudentIdentityCookieName))
            {
                Response.Cookies.Delete(Config.StudentIdentityCookieName);
            }
        }
        public void LogoutStudent()
        {
            if (CookieHandler.ContainsCookie(Config.StudentIdentityCookieName))
            {
                CookieHandler.DeleteCookie(Config.StudentIdentityCookieName);
            }
        }
        #endregion

        #region "Student Session"
        public StudentModel StudentSession()
        {
            if (CookieHandler.ContainsCookie(Config.StudentIdentityCookieName))
            {
                string EncryptedJsonString = CookieHandler.GetCookieValue(Config.StudentIdentityCookieName);
                try
                {
                    StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                    return FireStore.GetStudent(Student.StudentId).Result;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        public bool IsStudentSessionValid()
        {
            if (CookieHandler.ContainsCookie(Config.StudentIdentityCookieName))
            {
                string EncryptedJsonString = CookieHandler.GetCookieValue(Config.StudentIdentityCookieName);
                try
                {
                    StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                    return FireStore.IsStudentRegistered(Student.StudentId).Result && FireStore.IsStudentOnline(Student.StudentId).Result;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool IsStudentSessionValid(HttpRequest request)
        {
            if (request.Cookies.ContainsKey(Config.StudentIdentityCookieName))
            {
                string EncryptedJsonString = request.Cookies[Config.StudentIdentityCookieName];
                try
                {
                    StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                    return FireStore.IsStudentRegistered(Student.StudentId).Result && FireStore.IsStudentOnline(Student.StudentId).Result;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
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

        #region "Admin Session Logout/Login"
        public bool IsAdminLoggedIn(HttpRequest Request, HttpResponse Response)
        {
            if (Request.Cookies.ContainsKey(Config.AdminIdentityCookieName))
            {
                if (IsAdminSessionValid(Request))
                {
                    return true;
                }
                else
                {
                    LogoutAdmin(Request, Response);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsAdminLoggedIn()
        {
            if (CookieHandler.ContainsCookie(Config.AdminIdentityCookieName))
            {
                if (IsAdminSessionValid())
                {
                    return true;
                }
                else
                {
                    LogoutAdmin();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool IsAdminLoggedOut()
        {
            return CookieHandler.ContainsCookie(Config.AdminIdentityCookieName) == false;
        }
        public void LoginAdmin(LoginModel Login)
        {
            DateTime Expires;
            if (Login.StaySignedIn.Equals("true"))
            {
                //Persistant cookie that will stay for 30 days ~ 1 month.
                //The admin will remain signed in for 30 days.
                Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                //Session cookie that will stay for only 1 day.
                //The admin will logout after 1 day.
                Expires = DateTime.Now.AddDays(1);
            }

            //Get the Admin model
            AdminModel Admin = FireStore.GetAdmin(Login.Email).Result;

            //Create Cookie value that will contain the login information encrypted by AES and serialized into JSON
            string EncryptedJsonString = AES.Encrypt(JsonSerializer.Serialize(Admin), Config.Password);

            //Register the identity cookie that will contain the encrypted json student model.
            CookieHandler.RegisterCookie(Config.AdminIdentityCookieName, EncryptedJsonString, Expires);
        }
        public void LogoutAdmin(HttpRequest Request, HttpResponse Response)
        {
            if (Request.Cookies.ContainsKey(Config.AdminIdentityCookieName))
            {
                Response.Cookies.Delete(Config.AdminIdentityCookieName);
            }
        }
        public void LogoutAdmin()
        {
            if (CookieHandler.ContainsCookie(Config.AdminIdentityCookieName))
            {
                CookieHandler.DeleteCookie(Config.AdminIdentityCookieName);
            }
        }
        #endregion

        #region "Admin Session"
        public AdminModel AdminSession()
        {
            if (CookieHandler.ContainsCookie(Config.AdminIdentityCookieName))
            {
                string EncryptedJsonString = CookieHandler.GetCookieValue(Config.AdminIdentityCookieName);
                try
                {
                    AdminModel Admin = JsonSerializer.Deserialize<AdminModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                    return FireStore.GetAdmin(Admin.Email).Result;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        public bool IsAdminSessionValid()
        {
            if (CookieHandler.ContainsCookie(Config.AdminIdentityCookieName))
            {
                string EncryptedJsonString = CookieHandler.GetCookieValue(Config.AdminIdentityCookieName);
                try
                {
                    AdminModel Admin = JsonSerializer.Deserialize<AdminModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                    return FireStore.IsAdminRegistered(Admin.Email).Result && FireStore.IsAdminOnline(Admin.Email).Result;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private bool IsAdminSessionValid(HttpRequest request)
        {
            if (request.Cookies.ContainsKey(Config.AdminIdentityCookieName))
            {
                string EncryptedJsonString = request.Cookies[Config.AdminIdentityCookieName];
                try
                {
                    AdminModel Admin = JsonSerializer.Deserialize<AdminModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                    return FireStore.IsAdminRegistered(Admin.Email).Result && FireStore.IsAdminOnline(Admin.Email).Result;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "Confirmation Code / OTP"
        public void SetConfirmationCode(string ConfirmCode)
        {
            //The Confirmation code expires after 1 minute
            DateTime Expires = DateTime.Now.AddMinutes(Config.ConfirmationCodeTimeout);
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
        #endregion
        #endregion

        #region "Static Methods"
        public static StudentModel StudentSession(HttpRequest Request)
        {
            if (Request.Cookies.ContainsKey(Config.StudentIdentityCookieName))
            {
                string EncryptedJsonString = Request.Cookies[Config.StudentIdentityCookieName];
                StudentModel Student = JsonSerializer.Deserialize<StudentModel>(AES.Decrypt(EncryptedJsonString, Config.Password));
                return new FireStoreManager().GetStudent(Student.StudentId).Result;
            }
            return null;
        }
        #endregion
    }
}