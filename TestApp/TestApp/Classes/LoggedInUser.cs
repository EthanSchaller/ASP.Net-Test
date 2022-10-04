using System;

namespace TestApp
{
    [Serializable]
    public class LoggedInUser
    {
        public enum CurrentRole { Owner = 150, Admin = 100, Guest = 50 };
        private string userID;
        private string email;
        private string name;
        private string username;
        private CurrentRole role;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public CurrentRole Role
        {
            get { return role; }
            set { role = value; }
        }

        #region Constructor
        public LoggedInUser()
        {
        }
        public LoggedInUser(User usr)
        {
            try
            {
                userID = usr.ID.ToString();
                email = usr.Email;
                name = usr.FName + " " + usr.LName;
                username = usr.UsrName;

                if (usr.RoleID == 3) 
                {
                    role = CurrentRole.Owner;
                }
                else if (usr.RoleID == 4)
                {
                    role = CurrentRole.Admin;
                }
                else if (usr.RoleID == 6)
                {
                    role = CurrentRole.Guest;
                }
            }
            catch (Exception ex)
            {
                string error = ex.InnerException.ToString();

            }
        }
        #endregion
    }
}