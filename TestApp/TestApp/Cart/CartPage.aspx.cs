using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class CartPage : System.Web.UI.Page
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
        protected void btnConfirmDelToCart_Click(object sender, EventArgs e)
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    int cartID = Convert.ToInt32(hiCartID.Value);

                    LoggedInUser usr = WebUtility.getCurrentUser();
                    Cart cartItem = db.Carts.FirstOrDefault(c => c.ID == cartID);
                    cartItem.IsDeleted = true;

                    db.SaveChanges(); 
                    loadProducts();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error deleteing the item from the cart. Error Details: " + ex.Message);
            }
        }

        protected void btnConfirmCheckout_Click(object sender, EventArgs e)
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    LoggedInUser usr = WebUtility.getCurrentUser();
                    int usrID = Convert.ToInt32(usr.UserID);

                    DataTable dt = loadOrdersDt();

                    if (dt.Rows.Count > 0) 
                    {
                        decimal price = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            Cart cartItem = db.Carts.FirstOrDefault(c => c.UserID == usrID && c.IsDeleted == false);
                            cartItem.IsDeleted = true;
                            db.SaveChanges();

                            price += (Decimal)row.ItemArray[3];
                        }

                        var Ords = (from p in db.Carts
                                       where p.UserID == usrID
                                       orderby p.Product.Name ascending
                                       select new
                                       {
                                           p.ID,
                                           Prod = p.Product.Name,
                                           User = p.User.FName + " " + p.User.MName + " " + p.User.LName,
                                           Price = p.Product.Price * p.Quantity,
                                           p.Quantity,
                                           p.IsDeleted
                                       }).AsEnumerable();

                        DataTable dtOrds = DataGridFunctions.DataTableFromIEnumerable(Ords.ToList());

                        OrderMade newOrder = new OrderMade();
                        newOrder.UserID = Convert.ToInt32(usr.UserID);
                        newOrder.Price = price;
                        newOrder.Paid = false;
                        newOrder.OrdNum = Convert.ToInt32(usr.UserID + "935" + (dt.Rows.Count).ToString() + "115" +  (dtOrds.Rows.Count).ToString());
                        newOrder.OrderDate = Utility.ConvertDateTimeToUTC(DateTime.Now);
                        
                        db.OrderMades.Add(newOrder);
                        db.SaveChanges();
                    }
                }

                loadProducts();
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error checking-out your cart. Error Details: " + ex.Message);
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
                    delFromCart(Convert.ToInt32(e.CommandArgument));
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
                    int Order = DataGridFunctions.GetColumnIndexByName(e.Row, "Prod");

                    if (e.Row.Cells[Order].Text != "" && e.Row.Cells[Order].Text != "&nbsp;")
                    {
                        bool IsDeleted = Convert.ToBoolean(gvProds.DataKeys[e.Row.RowIndex].Values[1]);

                        if (IsDeleted)
                        {
                            string popover = " <i style=\"color:red;cursor: pointer;\" class=\"fas fa-info-circle\" data-toggle=\"popover\" title=\"TimeVault+\" data-content=\"This Product was deleted\"></i>";
                            e.Row.Cells[Order].Text += popover;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error Validating the product's Status. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region Utitities
        private void loadProducts()
        {
            try
            {
                gvProds.DataSource = loadOrdersDt();
                gvProds.DataBind();

                if (gvProds.HeaderRow != null)
                {
                    gvProds.HeaderRow.TableSection = TableRowSection.TableHeader;
                }

                ViewState["gvDatatable"] = loadOrdersDt();
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading the Product List. Error Details: " + ex.Message);
            }
        }

        private DataTable loadOrdersDt() 
        {
            using (MainEntityConnection db = new MainEntityConnection())
            {
                LoggedInUser usr = WebUtility.getCurrentUser();
                int usrID = Convert.ToInt32(usr.UserID);

                var OrdsDev = (from p in db.Carts
                               where !p.IsDeleted && p.UserID == usrID
                               orderby p.Product.Name ascending
                               select new
                               {
                                   p.ID,
                                   Prod = p.Product.Name,
                                   User = p.User.FName + " " + p.User.MName + " " + p.User.LName,
                                   Price = p.Product.Price * p.Quantity,
                                   p.Quantity,
                                   p.IsDeleted
                               }).AsEnumerable();

                DataTable dtOrdsDev = DataGridFunctions.DataTableFromIEnumerable(OrdsDev.ToList());

                var OrdList = from t1 in dtOrdsDev.AsEnumerable()
                              select new
                              {
                                  ID = (int)t1["ID"],
                                  Prod = (string)t1["Prod"],
                                  User = (string)t1["User"],
                                  Price = Decimal.Round((decimal)t1["Price"], 2),
                                  Quantity = (int)t1["Quantity"],
                                  IsDeleted = t1["IsDeleted"]
                              };

                DataTable dt = new DataTable();
                dt = DataGridFunctions.DataTableFromIEnumerable(OrdList);

                return dt;
            }
        }

        private void delFromCart(int ordId)
        {
            hiCartID.Value = ordId.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "delFromCart();", true);
        }
        #endregion
    }
}