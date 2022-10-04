using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class ProdsList : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                if (usr.Role == LoggedInUser.CurrentRole.Owner || usr.Role == LoggedInUser.CurrentRole.Admin) {
                    btnAdd.Visible = true;
                }
                else
                {
                    btnAdd.Visible = false;
                }

                loadProducts();
                ViewState["sortOrder"] = " ASC";
            }
        }
        #endregion

        #region ButtonEvents
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProdEdit.aspx");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadProducts();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSrch.Text = "";
            loadProducts();
        }
        #endregion

        #region GridEvents
        protected void gvProd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    int UserID = Convert.ToInt32(e.CommandArgument);
                    string sURL = "~/Product/ProdEdit.aspx?ID=" + UserID.ToString();
                    Response.Redirect(sURL);
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading the selected Product. Error Details: " + ex.Message);
            }
        }

        protected void gvProd_Sorting(object sender, GridViewSortEventArgs e)
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

                    gvProds.DataSource = dvSortedView;
                    gvProds.DataBind();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error sorting the grid. Error Details: " + ex.Message);
            }
        }

        protected void gvProd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int ProdIndex = DataGridFunctions.GetColumnIndexByName(e.Row, "Name");

                    if (e.Row.Cells[ProdIndex].Text != "" && e.Row.Cells[ProdIndex].Text != "&nbsp;")
                    {
                        bool TV_IsDeleted = Convert.ToBoolean(gvProds.DataKeys[e.Row.RowIndex].Values[1]);

                        if (TV_IsDeleted)
                        {
                            string popover = " <i style=\"color:red;cursor: pointer;\" class=\"fas fa-info-circle\" data-toggle=\"popover\" title=\"TimeVault+\" data-content=\"This Product was deleted\"></i>";
                            e.Row.Cells[ProdIndex].Text += popover;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error Validating teh product's Status. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region Utitities
        private void loadProducts()
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    var ProdsDev = (from p in db.Products where !p.IsDeleted orderby p.Name ascending
                                    select new
                                    {
                                        p.ID,
                                        p.Name,
                                        p.Desc,
                                        p.IsDeleted
                                    }).AsEnumerable().Select(x => new
                                    {
                                        x.ID,
                                        x.Name,
                                        x.Desc,
                                        x.IsDeleted
                                    });


                    if (txtSrch.Text != "")
                    {
                        ProdsDev = ProdsDev.Where(u => u.Name.ToLower().Contains(txtSrch.Text.ToLower()) || u.Desc.ToLower().Contains(txtSrch.Text.ToLower()));
                    }

                    DataTable dtProdsDev = DataGridFunctions.DataTableFromIEnumerable(ProdsDev.ToList());

                    DataSet ds = new DataSet("Products");

                    var ProdList = from t1 in dtProdsDev.AsEnumerable()
                                   select new
                                   {
                                       ID = (int)t1["ID"],
                                       Name = (string)t1["Name"],
                                       Desc = (string)t1["Desc"],
                                       TV_IsDeleted = t1["IsDeleted"]
                                   };

                    DataTable dt = new DataTable();
                    dt = DataGridFunctions.DataTableFromIEnumerable(ProdList);
                    gvProds.DataSource = dt;
                    gvProds.DataBind();

                    if (gvProds.HeaderRow != null)
                    {
                        gvProds.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }

                    ViewState["gvDatatable"] = dt;
                    lblGridCount.Text = "Product Count: " + ProdList.Count();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading Product List. Error Details: " + ex.Message);
            }
        }
        private string getAssocUser(string user)
        {
            if (user != null)
            {
                return user;
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}