using System;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;

namespace TestApp
{
    public partial class PasswordRecovery : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            string hash = Request.QueryString["Hash"];
            hidCreate.Value = Request.QueryString["Create"];

            if (hash != null)
            {
                if (hidCreate.Value == "1")
                {
                    lblHeader.Text = "Create Password";
                    btnReset.Text = "Create Password";
                }

                hidHash.Value = hash;

                using (MainEntityConnection db = new MainEntityConnection())
                {
                    DateTime currentDate = DateTime.Now.Date;
                    
                    UserPasswordRecovery passwordRecovery = db.UserPasswordRecoveries.FirstOrDefault(u => u.Hash == hidHash.Value && u.CreatedOn > currentDate && !u.HasBeenUpdated);
                    if (passwordRecovery != null)
                    {
                        divPasswordRecovery.Visible = false;
                        divResetPassword.Visible = true;
                    }
                    else
                    {
                        Site mPage = (Site)Page.Master;
                        mPage.displayAlert(2, "This reset link is no longer valid. Enter your email if you wish to be sent a new recovery link.");
                    }
                }

            }
        }
        #endregion

        #region ButtonEvents
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                try
                {
                    using (MainEntityConnection db = new MainEntityConnection())
                    {
                        User user = db.Users.FirstOrDefault(u => u.Email == txtEmail.Text && u.IsActive && !u.IsDeleted);

                        if (user != null)
                        {
                            string hash = UserAuthentication.ComputeMd5Hash(txtEmail.Text);

                            UserPasswordRecovery passwordRecovery = new UserPasswordRecovery();
                            passwordRecovery.UserID = user.ID;
                            passwordRecovery.Hash = hash;
                            passwordRecovery.CreatedOn = DateTime.Now;
                            passwordRecovery.HasBeenUpdated = false;
                            db.UserPasswordRecoveries.Add(passwordRecovery);
                            db.SaveChanges();

                            MailMessage insMail = new MailMessage();
                            insMail.From = new MailAddress("AspNetSite@scotialogic.ca");
                            insMail.To.Add(txtEmail.Text);
                            insMail.Subject = "User Password Reset";
                            insMail.Body = "A password reset was requested for your account on the ASP.NET Test Site  <br/><br/> If you did not request this password reset, disregard this email. <br/><br/> To reset your password, click this <a href='" + Utility.getWebsitePath() + "/PasswordRecovery.aspx?Hash=" + hash + "'>link</a>.";
                            insMail.IsBodyHtml = true;

                            SmtpClient SmtpMail = new SmtpClient();
                            SmtpMail.Send(insMail);

                            divPasswordRecovery.Visible = false;
                            divEmailSent.Visible = true;
                        }
                        else
                        {
                            Site mPage = (Site)Page.Master;
                            mPage.displayAlert(3, "The email address entered could not be found in the system.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Site mPage = (Site)Page.Master;
                    mPage.displayAlert(4, "There was an error resetting the password. Error Details: " + ex.Message);
                }
            }//end validate
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                //check new password is valid
                if (validatePassword())
                {
                    using (MainEntityConnection db = new MainEntityConnection())
                    {
                        DateTime currentDate = DateTime.Now.Date;
                        
                        UserPasswordRecovery passwordRecovery = db.UserPasswordRecoveries.FirstOrDefault(u => u.Hash == hidHash.Value && u.CreatedOn > currentDate && !u.HasBeenUpdated);
                        if (passwordRecovery != null)
                        {
                            User userInfo = db.Users.FirstOrDefault(u => u.ID == passwordRecovery.UserID);
                            userInfo.Password = UserAuthentication.encryptPassword(txtNewPassword.Text);
                            passwordRecovery.HasBeenUpdated = true;
                            db.SaveChanges();

                            Response.Redirect("~/Login.aspx");
                        }
                        else
                        {
                            Site mPage = (Site)Page.Master;
                            mPage.displayAlert(3, "This link is no longer valid");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error updating password. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region Utilities
        private bool validate()
        {
            bool isValid = true;
            try
            {
                if (txtEmail.Text == "")
                {
                    BootstrapErrors.AddError(txtEmail);
                    isValid = false;
                }
                else
                {
                    BootstrapErrors.RemoveError(txtEmail);
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was problem validating the email. Error Details: " + ex.Message);
            }
            return isValid;
        }

        private bool validatePassword()
        {
            bool isValid = true;
            Site mPage = (Site)Page.Master;

            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    LoggedInUser usr = WebUtility.getCurrentUser();

                    if (txtNewPassword.Text == "")
                    {
                        BootstrapErrors.AddError(txtNewPassword);
                        isValid = false;
                    }
                    else
                    {
                        BootstrapErrors.RemoveError(txtNewPassword);
                    }

                    if (txtConfirmPassword.Text == "")
                    {
                        BootstrapErrors.AddError(txtConfirmPassword);
                        isValid = false;
                    }
                    else
                    {
                        BootstrapErrors.RemoveError(txtConfirmPassword);
                    }

                    if (isValid)
                    {
                        int i = Convert.ToInt32(usr.UserID);
                        User user = db.Users.FirstOrDefault(u => u.ID == i);

                        if (txtNewPassword.Text != txtConfirmPassword.Text)
                        {
                            isValid = false;
                            mPage.displayAlert(3, "New passwords did not match");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                mPage.displayAlert(4, "There was problem validating the User Password. Error Details: " + ex.Message);
            }

            return isValid;
        }
        #endregion
    }
}