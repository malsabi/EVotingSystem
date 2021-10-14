using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EVotingSystem.Utilities
{
    public class CookieHandler
    {
        #region "Fields"
        private readonly Controller Controller;
        #endregion

        #region "Constructors"
        public CookieHandler(Controller Controller)
        {
            this.Controller = Controller;
        }
        #endregion

        #region "Private Methods"
        private CookieOptions GetCookieOptions(DateTime Expires)
        {
            return new CookieOptions()
            {
                Expires = Expires,
                HttpOnly = false,
                IsEssential = true
            };
        }
        #endregion

        #region "Public Methods"
        public void RegisterCookie(string CookieName, string CookieValue, DateTime Expires)
        {
            if (Controller != null)
            {
                Controller.Response.Cookies.Append(CookieName, CookieValue, GetCookieOptions(Expires));
            }
        }

        public void DeleteCookie(string CookieName)
        {
            if (Controller != null)
            {
                Controller.Response.Cookies.Delete(CookieName);
            }
        }

        public string GetCookieValue(string CookieName)
        {
            if (Controller != null)
            {
                return Controller.Request.Cookies[CookieName];
            }
            return "";
        }

        public bool ContainsCookie(string CookieName)
        {
            if (Controller != null && Controller.Request != null)
            {
                try
                {
                    return Controller.Request.Cookies.ContainsKey(CookieName);
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        #endregion
    }
}