using System;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;


namespace TestApp
{
    public partial class Signup : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region ButtonEvents
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (validate())
            {
                LoggedInUser usr = WebUtility.getCurrentUser();
                try
                {
                    bool emailSent = true;

                    MailMessage insMail = new MailMessage();
                    insMail.From = new MailAddress("devvault@agresearch.ca");
                    insMail.IsBodyHtml = true;

                    using (MainEntityConnection db = new MainEntityConnection())
                    {
                        User user = new User();
                        user.Email = txtEmail.Text;
                        string hash = UserAuthentication.ComputeMd5Hash(txtEmail.Text);
                        user.Password = UserAuthentication.encryptPassword(hash);
                        user.UsrName = txtUsrName.Text;
                        user.FName = txtFName.Text;
                        user.MName = txtMName.Text;
                        user.LName = txtLName.Text;
                        user.RoleID = 6;
                        user.IsActive = true;
                        user.IsDeleted = false;
                        user.CreatedBy = "autogenerated@email.com";
                        user.CreatedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);
                        user.ModifiedBy = "autogenerated@email.com";
                        user.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                        UserPasswordRecovery passwordRecovery = new UserPasswordRecovery();
                        passwordRecovery.UserID = user.ID;
                        passwordRecovery.Hash = hash;
                        passwordRecovery.CreatedOn = DateTime.Now;
                        passwordRecovery.HasBeenUpdated = false;

                        db.Users.Add(user);
                        db.UserPasswordRecoveries.Add(passwordRecovery);

                        db.SaveChanges();

                        insMail.To.Add(txtEmail.Text);
                        insMail.Subject = "ASP.NET TestApp Set Password";
                        insMail.Body = "An account has been created for you on ASP.NET testApp site <br/><br/> To set your password, click this <a href='" + Utility.getWebsitePath() + "/PasswordRecovery.aspx?Hash=" + hash + "&Create=1'>link</a>.";

                        SmtpClient SmtpMail = new SmtpClient();
                        try
                        {
                            SmtpMail.Send(insMail);
                        }
                        catch (Exception)
                        {
                            emailSent = false;
                        }

                        if (emailSent)
                        {
                            Site mPage = (Site)Page.Master;
                            mPage.displayAlert(1, "The user has been created and an email has been sent to them to set their password.");
                        }
                        else
                        {
                            Site mPage = (Site)Page.Master;
                            mPage.displayAlert(3, "The user has been created but the email could not be sent. Verify email is correct.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Site mPage = (Site)Page.Master;
                    mPage.displayAlert(4, "There was an error adding the User. Error Details: " + ex.Message);
                }
            }

            Response.Redirect("Login.aspx");
        }
        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
        #endregion

        #region Utilities
        private bool validate()
        {
            return true;
        }
        #endregion  
    }
}