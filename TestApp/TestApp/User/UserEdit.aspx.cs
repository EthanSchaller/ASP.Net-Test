using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class UserEdit : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userID = Request.QueryString["UserID"];
                hidUserID.Value = userID;

                txtEmail.ReadOnly = true;

                fillDropDownLists();

                if (userID != null)
                {
                    loadUserData();
                    formMode(false);
                }
                else
                {
                    txtEmail.ReadOnly = false;
                    formMode(true);
                    btnCancel.Visible = false;
                    btnDelete.Visible = false;
                }

                checkRole();
            }
        }

        private void checkRole()
        {
            try
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                using (MainEntityConnection db = new MainEntityConnection())
                {
                    if (usr.Role == LoggedInUser.CurrentRole.Admin && usr.UserID != hidUserID.Value) 
                    {
                        btnEdit.Visible = false;
                    }
                    else if (usr.Role < LoggedInUser.CurrentRole.Admin && usr.UserID != hidUserID.Value)
                    {
                        //redirect them
                        Response.Redirect("~/Dashboard.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error validating the user's role. \nError Details: " + ex.Message);
            }
        }
        #endregion

        #region ButtonEvents
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string userID = hidUserID.Value;

            if (validate())
            {
                LoggedInUser usr = WebUtility.getCurrentUser();
                if ((userID == null || userID == ""))
                {
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
                            user.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                            user.IsActive = chkActive.Checked;
                            user.IsDeleted = false;
                            user.CreatedBy = usr.Email;
                            user.CreatedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);
                            user.ModifiedBy = usr.Email;
                            user.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                            UserPasswordRecovery passwordRecovery = new UserPasswordRecovery();
                            passwordRecovery.UserID = user.ID;
                            passwordRecovery.Hash = hash;
                            passwordRecovery.CreatedOn = DateTime.Now;
                            passwordRecovery.HasBeenUpdated = false;

                            db.Users.Add(user);
                            db.UserPasswordRecoveries.Add(passwordRecovery);

                            db.SaveChanges();

                            hidUserID.Value = user.ID.ToString();

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

                            loadUserData();
                            formMode(false);

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
                else
                {
                    try
                    {
                        using (MainEntityConnection db = new MainEntityConnection())
                        {
                            int i = Convert.ToInt32(userID);
                            User user = db.Users.FirstOrDefault(u => u.ID == i);

                            user.UsrName = txtUsrName.Text;
                            user.FName = txtFName.Text;
                            user.MName = txtMName.Text;
                            user.LName = txtLName.Text;
                            user.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                            user.IsActive = chkActive.Checked;
                            user.ModifiedBy = usr.Name;
                            user.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                            db.SaveChanges();
                            loadUserData();

                            formMode(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Site mPage = (Site)Page.Master;
                        mPage.displayAlert(4, "There was an error updating the User. Error Details: " + ex.Message);
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, System.EventArgs e)
        {
            formMode(true);
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            formMode(false);
            loadUserData();
            validate();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                displayDeleteModal();
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error deleting the User. Error Details: " + ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, System.EventArgs e)
        {
            try
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                using (MainEntityConnection db = new MainEntityConnection())
                {
                    int userID = Convert.ToInt32(hidUserID.Value);
                    User user = db.Users.First(u => u.ID == userID);

                    user.IsDeleted = true;
                    user.ModifiedBy = usr.Email;
                    user.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                    db.SaveChanges();
                    Response.Redirect("UserList.aspx");
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error deleting the User. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region Utilities
        private void loadUserData()
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    string userID = hidUserID.Value;
                    int i = Convert.ToInt32(userID);
                    User user = db.Users.FirstOrDefault(u => u.ID == i);

                    if (user == null || user.IsDeleted)
                    {
                        Response.Redirect("~/User/UserList.aspx");
                    }

                    txtEmail.Text = user.Email;
                    txtUsrName.Text = user.UsrName;
                    txtFName.Text = user.FName;
                    txtMName.Text = user.MName;
                    txtLName.Text = user.LName;
                    chkActive.Checked = user.IsActive;

                    txtRole.Text = user.Role.Name;
                    ddlRole.SelectedValue = user.RoleID.ToString();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading the User Details. Error Details: " + ex.Message);
            }
        }

        private void fillDropDownLists()
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    var roleList = (from r in db.Roles where r.IsActive && !r.IsDeleted orderby r.Name select new { r.ID, r.Name }).ToList();

                    DataTable dtRole = new DataTable();
                    dtRole = DataGridFunctions.DataTableFromIEnumerable(roleList);
                    if (dtRole.Rows.Count > 0)
                    {
                        DataRow emptyRowRole = dtRole.NewRow();
                        emptyRowRole["ID"] = 0;
                        emptyRowRole["Name"] = "-SELECT-";
                        dtRole.Rows.InsertAt(emptyRowRole, 0);

                        ddlRole.DataSource = dtRole;
                        ddlRole.DataValueField = "ID";
                        ddlRole.DataTextField = "Name";
                        ddlRole.SelectedIndex = -1;
                        ddlRole.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error populating the drop downs. Error Details: " + ex.Message);
            }
        }

        private void formMode(bool mode)
        {
            string userID = hidUserID.Value;
            if (userID == "")
            {
                txtEmail.ReadOnly = !mode;
            }
            else
            {
                txtEmail.ReadOnly = true;
            }
            txtUsrName.ReadOnly = !mode;
            txtFName.ReadOnly = !mode;
            txtMName.ReadOnly = !mode;
            txtLName.ReadOnly = !mode;

            chkActive.Enabled = mode;

            txtRole.Visible = !mode;
            ddlRole.Visible = mode;

            btnSave.Visible = mode;
            btnCancel.Visible = mode;
            btnDelete.Visible = mode;
            btnEdit.Visible = !mode;

            checkRole();
        }

        private bool validate()
        {
            string userID = hidUserID.Value;

            bool isValid = true;
            try
            {
                if (txtEmail.Text == "")
                {
                    BootstrapErrors.AddError(txtEmail);
                    lblEmailError.Text = "Email Field is required.";
                    isValid = false;
                }
                else
                {
                    if (!RegEx.isValidEmail(txtEmail.Text))
                    {
                        BootstrapErrors.AddError(txtEmail);
                        lblEmailError.Text = "Make sure the email address is valid.";
                        isValid = false;
                    }
                    else
                    {
                        BootstrapErrors.RemoveError(txtEmail);
                    }
                }

                if (userID == "" && isValid)
                {
                    using (MainEntityConnection db = new MainEntityConnection())
                    {
                        User user = db.Users.FirstOrDefault(u => u.Email == txtEmail.Text);

                        if (user != null)
                        {
                            BootstrapErrors.AddError(txtEmail);
                            lblEmailError.Text = "Email address already exists within the system.";
                            isValid = false;
                        }
                        else
                        {
                            BootstrapErrors.RemoveError(txtEmail);
                        }
                    }
                }

                if (txtFName.Text == "")
                {
                    BootstrapErrors.AddError(txtFName);
                    isValid = false;
                }
                else
                {
                    BootstrapErrors.RemoveError(txtFName);
                }

                if (txtMName.Text == "")
                {
                    BootstrapErrors.AddError(txtMName);
                    isValid = false;
                }
                else
                {
                    BootstrapErrors.RemoveError(txtMName);
                }

                if (txtLName.Text == "")
                {
                    BootstrapErrors.AddError(txtLName);
                    isValid = false;
                }
                else
                {
                    BootstrapErrors.RemoveError(txtLName);
                }

                if (ddlRole.SelectedIndex == 0 || ddlRole.SelectedIndex == -1)
                {
                    BootstrapErrors.AddError(ddlRole);
                    isValid = false;
                }
                else
                {
                    BootstrapErrors.RemoveError(ddlRole);
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was problem validating the User. Error Details: " + ex.Message);
            }
            return isValid;
        }

        private void displayDeleteModal()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDeleteModal();", true);
        }
        #endregion
    }
}