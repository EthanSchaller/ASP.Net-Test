using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class UserList : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                if (usr.Role == LoggedInUser.CurrentRole.Owner)
                {
                    btnAdd.Visible = true;
                }
                else
                {
                    btnAdd.Visible = false;
                }

                loadUsers();
                ViewState["sortOrder"] = " ASC";
            }
        }
        #endregion

        #region ButtonEvents
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserEdit.aspx");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadUsers();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            loadUsers();
        }
        #endregion

        #region GridEvents
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    int UserID = Convert.ToInt32(e.CommandArgument);
                    string sURL = "~/User/UserEdit.aspx?UserID=" + UserID.ToString();
                    Response.Redirect(sURL);
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading the selected User. Error Details: " + ex.Message);
            }
        }

        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataTable dtSortTable = ViewState["gvDatatable"] as DataTable;

                if (dtSortTable != null)
                {
                    DataView dvSortedView = new DataView(dtSortTable);

                    if ((string)ViewState["sortColumn"] != e.SortExpression)
                    {
                        ViewState["sortOrder"] = " ASC";
                    }
                    ViewState["sortColumn"] = e.SortExpression;

                    if ((string)ViewState["sortOrder"] == " ASC")
                    {
                        dvSortedView.Sort = e.SortExpression + ViewState["sortOrder"];
                        ViewState["sortOrder"] = " DESC";
                    }
                    else
                    {
                        dvSortedView.Sort = e.SortExpression + ViewState["sortOrder"];
                        ViewState["sortOrder"] = " ASC";
                    }

                    gvUsers.DataSource = dvSortedView;
                    gvUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error sorting the grid. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region Utitities
        private void loadUsers()
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    var userList = (from u in db.Users
                                    where !u.IsDeleted
                                    orderby (u.FName + " " + u.MName + " " + u.LName) ascending
                                    select new
                                    {
                                        u.ID,
                                        u.Email,
                                        u.UsrName,
                                        Name = u.FName + " " + u.MName + " " + u.LName,
                                        RoleName = u.Role.Name,
                                        u.IsActive,
                                    }).AsEnumerable();

                    if (txtName.Text != "")
                    {
                        userList = userList.Where(u => u.Email.ToLower().Contains(txtName.Text.ToLower()) || u.Name.ToLower().Contains(txtName.Text.ToLower()));
                    }

                    DataTable dt = new DataTable();
                    dt = DataGridFunctions.DataTableFromIEnumerable(userList);
                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();

                    gvUsers.HeaderRow.TableSection = TableRowSection.TableHeader;
                    ViewState["gvDatatable"] = dt;
                    lblGridCount.Text = "User Count: " + userList.Count();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading User List. Error Details: " + ex.Message);
            }
        }
        #endregion
    }
}