namespace EVotingSystem.Models
{
    public class LoginModel
    {
        #region "Properties"
        public string StudentId { get; set; }
        public string Password { get; set; }
        public bool StaySignedIn { get; set; }
        #endregion

        #region "Constructor"
        public LoginModel()
        {
            StudentId = "";
            Password = "";
            StaySignedIn = false;
        }
        #endregion
    }
}