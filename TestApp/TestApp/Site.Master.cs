using System;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace TestApp
{
    public partial class Site : System.Web.UI.MasterPage
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser usr = WebUtility.getCurrentUser();

            if (usr.Email == null && Path.GetFileName(Request.Url.AbsolutePath) != "Login.aspx" && Path.GetFileName(Request.Url.AbsolutePath) != "Signup.aspx" && Path.GetFileName(Request.Url.AbsolutePath) != "PasswordRecovery.aspx")
            {
                string URL = HttpUtility.UrlEncode(EncryptQueryString.EncryptQString(Request.Url.ToString()));

                if (Path.GetFileName(Request.Url.AbsolutePath) != "Dashboard.aspx")
                {
                    Response.Redirect("~/Login.aspx?URL=" + URL);
                }
                else
                {
                    Response.Redirect("~/Login.aspx");
                }
            }

            if (!IsPostBack)
            {
                LoadMenuByRole(usr);

                if (usr.Name != null)
                {
                    hlUserProfile.NavigateUrl = "~/User/UserEdit.aspx?UserID=" + usr.UserID;
                }
            }
        }
        #endregion

        #region ButtonEvents
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        #endregion

        #region Utilities
        private void LoadMenuByRole(LoggedInUser usr)
        {
            if (usr.Email == null)
            {
                sideMenu.Visible = false;

                userDropdown.Visible = false;
            }
            else
            {
                if (usr.Role == LoggedInUser.CurrentRole.Owner)
                {
                    hlUserList.Visible = true;
                    hlProdList.Visible = true;
                    hlUserProfile.Visible = true;
                    hlViewCart.Visible = false;
                }
                else if (usr.Role == LoggedInUser.CurrentRole.Admin)
                {
                    hlUserList.Visible = true;
                    hlProdList.Visible = true;
                    hlUserProfile.Visible = true;
                    hlViewCart.Visible = true;
                }
                else
                {
                    hlUserList.Visible = false;
                    hlProdList.Visible = false;
                    hlUserProfile.Visible = false;
                    hlViewCart.Visible = true;
                }
            }
        }

        public void displayAlert(int type, string message)
        {
            if (type == 1)
            {
                divAlertSuccess.Visible = true;
                lblAlertSuccess.Text = message;
            }
            if (type == 2)
            {
                divAlertInfo.Visible = true;
                lblAlertInfo.Text = message;
            }
            if (type == 3)
            {
                divAlertWarning.Visible = true;
                lblAlertWarning.Text = message;
            }
            if (type == 4)
            {
                divAlertDanger.Visible = true;
                lblAlertDanger.Text = message;
            }
        }
        #endregion
    }
}