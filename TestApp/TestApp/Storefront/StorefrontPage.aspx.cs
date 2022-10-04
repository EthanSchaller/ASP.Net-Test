using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class StorefrontPage : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvProds.Visible = true;

                loadProducts();
                ViewState["sortOrder"] = " ASC";
            }
        }
        #endregion

        #region ButtonEvents
        protected void btnConfirmAddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    LoggedInUser usr = WebUtility.getCurrentUser();

                    int userID = Convert.ToInt32(usr.UserID);
                    int prodID = Convert.ToInt32(hidProdID.Value);
                    Cart cart = db.Carts.FirstOrDefault(c => c.UserID == userID && c.ProdID == prodID && c.IsDeleted == false);

                    if (cart == null)
                    {
                        Product product = db.Products.FirstOrDefault(i => i.ID == prodID && i.IsDeleted == false);

                        if (usr.Name != null && product.Name != null)
                        {
                            Cart newCart = new Cart();
                            newCart.ProdID = product.ID;
                            newCart.UserID = Convert.ToInt32(usr.UserID);
                            newCart.Quantity = 1;
                            newCart.IsDeleted = false;

                            db.Carts.Add(newCart);
                        }
                    }
                    else
                    {
                        cart.Quantity++;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error adding the product to the cart. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region GridEvents
        protected void gvProd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    AddToCart(Convert.ToInt32(e.CommandArgument));
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
                    var ProdsDev = (from p in db.Products
                                    where !p.IsDeleted
                                    orderby p.Name ascending
                                    select new
                                    {
                                        p.ID,
                                        p.Image,
                                        p.Name,
                                        p.Price,
                                        p.Desc,
                                        p.IsDeleted
                                    }).AsEnumerable().Select(x => new
                                    {
                                        x.ID,
                                        x.Image,
                                        x.Name,
                                        x.Price,
                                        x.Desc,
                                        x.IsDeleted
                                    });

                    DataTable dtProdsDev = DataGridFunctions.DataTableFromIEnumerable(ProdsDev.ToList());

                    DataSet ds = new DataSet("Products");

                    var ProdList = from t1 in dtProdsDev.AsEnumerable()
                                   select new
                                   {
                                       ID = (int)t1["ID"],
                                       Image = (int)t1["Image"],
                                       Name = (string)t1["Name"],
                                       Price = Decimal.Round((decimal)t1["Price"], 2),
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
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading Product List. Error Details: " + ex.Message);
            }
        }
        private void AddToCart(int prodID)
        {
            hidProdID.Value = prodID.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "addToCart();", true);
        }
        #endregion
    }
}