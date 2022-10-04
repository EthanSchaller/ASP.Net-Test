using System;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Web.Security;

namespace TestApp
{
    public static class UserAuthentication
    {
        public static User FindUser(string username, MainEntityConnection db)
        {
            //get user that matches the username and make sure the user/company is active and not deleted
            User user = db.Users.FirstOrDefault(u => u.Email.Equals(username) && u.IsActive && !u.IsDeleted);
            return user;
        }
        public static void ThrowExceptionIfBadUser(User user)
        {
            if (user == null)
                ThrowUsernameException();
        }
        public static string encryptPassword(string cleartextPassword)
        {
            string salt = Guid.NewGuid().ToString("N");
            string hashedPwd = hashPassword(cleartextPassword, salt);
            return salt + hashedPwd; /// salt is guaranteed to be 32 characters long, so we can implicitly extract the salt from the first 32 chars
        }
        public static string hashPassword(string cleartextPassword, string salt)
        {
            System.Security.Cryptography.SHA256 sha = SHA256Managed.Create();
            byte[] bytesForHashing = Encoding.UTF8.GetBytes(salt + cleartextPassword);
            return CharsToHex(Encoding.UTF8.GetString(sha.ComputeHash(bytesForHashing)));
        }
        public static string CharsToHex(string chars)
        {
            char[] values = chars.ToCharArray();
            string output = string.Empty;
            foreach (char letter in values)
            {
                // Get the integral value of the char
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                output += string.Format("{0:x2}", value);
            }
            return output;
        }
        public static void StorePassword(string username, string hashedPwdWithSaltPrefix)
        {
            using (MainEntityConnection db = new MainEntityConnection())
            {
                User user = FindUser(username, db);
                if (user == null)
                    ThrowUsernameException();
                user.Password = hashedPwdWithSaltPrefix;
                db.SaveChanges();
            }
        }

        public static User RetrieveHashedPassword(string passedUsername)
        {
            using (MainEntityConnection db = new MainEntityConnection())
            {
                User user = FindUser(passedUsername, db);
                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
        }
        public static bool AuthenticateFromCookie(HttpCookie cookie, ref User usr)
        {
            try
            {
                if (cookie != null)
                {
                    //decryt and set the cookie
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    FormsAuthentication.SetAuthCookie(ticket.Name, false, ticket.CookiePath);

                    //get the user based on the username from the decrypted cookie
                    usr = RetrieveHashedPassword(ticket.Name);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool Authenticate(string passedUsername, string passedPassword, ref User usr)
        {
            usr = RetrieveHashedPassword(passedUsername);
            string hashedPwd;
            try
            {
                hashedPwd = usr.Password;
            }
            catch
            {
                return false;
            }
            if (hashedPwd == null)
                return false;
            return ValidatePassword(passedPassword, hashedPwd);
        }

        public static void ThrowUsernameException()
        {
            throw new Exception("No user matches the given username.");
        }

        /// Overloaded static method to determine if a passed cleartext password is a match for the encrypted password when hashed using the encrypted password's 32-character substring prefix as a salt
        public static bool ValidatePassword(string cleartextPassword, string encryptedPassword)
        {
            if (encryptedPassword.Length < 33)
                return false;
            string salt = encryptedPassword.Substring(0, 32);
            return ((salt + hashPassword(cleartextPassword, salt)).Equals(encryptedPassword));
        }

        //used to create hash for url when resetting user passwords
        public static string ComputeMd5Hash(string value)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(value + DateTime.Now.ToString()));

                //initialize string builder, loop through hash and format as hex string, convert to string
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }

    public class WebUtility
    {
        public void setLoggedInUser(User usr)
        {
            LoggedInUser liU = new LoggedInUser(usr);
            HttpContext.Current.Session["DevVault_LoggedIn"] = liU;
        }

        public void setLoggedInUser(User usr, ref LoggedInUser LIU, bool isAD)
        {
            LoggedInUser liU = new LoggedInUser(usr);
            HttpContext.Current.Session["DevVault_LoggedIn"] = liU;
            LIU = liU;
        }

        public static LoggedInUser getCurrentUser()
        {
            LoggedInUser usr;
            if (HttpContext.Current.Request.IsAuthenticated && (HttpContext.Current.Session != null && HttpContext.Current.Session["DevVault_LoggedIn"] != null))
            {
                usr = (LoggedInUser)HttpContext.Current.Session["DevVault_LoggedIn"];
            }
            else
            {
                usr = new LoggedInUser();
            }
            return usr;
        }
    }
}