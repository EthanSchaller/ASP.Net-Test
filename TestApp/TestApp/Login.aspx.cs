using System;
using System.Web;
using System.Web.UI;

namespace TestApp
{
    public partial class Login : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //check if is a previous forms authentication cookie
                if (Request.Cookies["LoginCookie"] != null)
                {
                    User loggedInUser = new User();

                    //pass the forms authentication cookie to the UserAuthentication class
                    UserAuthentication.AuthenticateFromCookie(Request.Cookies["LoginCookie"], ref loggedInUser);

                    //set the logged in user
                    WebUtility wu = new WebUtility();
                    LoggedInUser user = new LoggedInUser();
                    wu.setLoggedInUser(loggedInUser, ref user, false);

                    TimeSpan span = -(TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow));

                    //set the user's offset from UTC time
                    HttpCookie utcOffset = new HttpCookie("UTCOffset");
                    utcOffset.Value = span.TotalMinutes.ToString();
                    Response.Cookies.Add(utcOffset);

                    string commaSeparatedRoles = "";
                    //create forms auth ticket
                    FormsAuthenticationUtil.RedirectFromLoginPage(loggedInUser.Email, commaSeparatedRoles, false);

                    //check if there was a URL passed in, if so rediret back to that page
                    if (Request.QueryString["URL"] != null)
                    {
                        Response.Redirect(EncryptQueryString.DecryptQString(HttpUtility.UrlDecode(Request.QueryString["URL"])), false);
                    }
                }
                else
                {
                    LoggedInUser usr = WebUtility.getCurrentUser();

                    if (usr.Email != null)
                    {
                        Response.Redirect("~/Dashboard.aspx");
                    }
                }
            }

            Page.Master.Page.Form.DefaultButton = btnLogin.UniqueID;
        }
        #endregion

        #region ButtonEvents
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                User loggedInUser = new User();
                bool loginFailed = false;

                if (UserAuthentication.Authenticate(txtUsername.Text, txtPassword.Text, ref loggedInUser) == true)
                {
                    WebUtility wu = new WebUtility();
                    LoggedInUser usr = new LoggedInUser();
                    wu.setLoggedInUser(loggedInUser, ref usr, false);
                }
                else
                {
                    loginFailed = true;
                }

                if (!loginFailed)
                {
                    HttpCookie utcOffset = new HttpCookie("UTCOffset");
                    utcOffset.Value = hidUTCOffset.Value;
                    Response.Cookies.Add(utcOffset);

                    string commaSeparatedRoles = "";
                    FormsAuthenticationUtil.RedirectFromLoginPage(loggedInUser.Email, commaSeparatedRoles, false);

                    if (Request.QueryString["URL"] != null)
                    {
                        Response.Redirect(EncryptQueryString.DecryptQString(HttpUtility.UrlDecode(Request.QueryString["URL"])), false);
                    }
                }
                else
                {
                    Site mPage = (Site)Page.Master;
                    mPage.displayAlert(3, "Username or Password was incorrect");
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error on login. Error Details: " + ex.Message);
            }
        }
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Signup.aspx");
        }
        #endregion        
    }
}