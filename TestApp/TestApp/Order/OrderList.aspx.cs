using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class OrderList : System.Web.UI.Page
    {
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                if (usr.Role != LoggedInUser.CurrentRole.Guest)
                {
                    gvOrder.Columns[0].Visible = false;
                    lblTtlProfit.Visible = true;
                }
                else
                {
                    gvOrder.Columns[0].Visible = true;
                    lblTtlProfit.Visible = false;
                }

                loadProducts();
                getTotals();
                ViewState["sortOrder"] = " ASC";
            }
        }
        #endregion

        #region ButtonEvents
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProdEdit.aspx");
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
        protected void btnTtlProfits_Click(object sender, EventArgs e) 
        {
            Response.Redirect("~/TotalProfits.aspx");
        }
        #endregion

        #region GridEvents
        protected void gvOrder_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    int OrdID = Convert.ToInt32(e.CommandArgument);
                    string sURL = "~/Order/PayPage.aspx?ID=" + OrdID.ToString();
                    Response.Redirect(sURL);
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading the selected Product. Error Details: " + ex.Message);
            }
        }

        protected void gvOrder_Sorting(object sender, GridViewSortEventArgs e)
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

                    gvOrder.DataSource = dvSortedView;
                    gvOrder.DataBind();
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
        private void loadProducts()
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    var OrderList = (from p in db.OrderMades
                                     orderby p.OrderDate descending
                                     select new
                                     {
                                         p.ID,
                                         Username = p.User.UsrName,
                                         Price = Decimal.Round(p.Price, 2),
                                         OrderNum = p.OrdNum,
                                         p.Paid,
                                         p.OrderDate
                                     }).AsEnumerable().Select(x => new
                                     {
                                         x.ID,
                                         x.Username,
                                         x.Price,
                                         x.OrderNum,
                                         x.Paid,
                                         x.OrderDate
                                     });

                    LoggedInUser usr = WebUtility.getCurrentUser();

                    if (usr.Role == LoggedInUser.CurrentRole.Guest) 
                    { 
                        OrderList = OrderList.Where(p => p.Username.ToLower().Contains(usr.Username.ToLower()));
                    }

                    if (txtSrch.Text != "")
                    {
                        OrderList = OrderList.Where(p => p.Username.ToLower().Contains(txtSrch.Text.ToLower()));
                    }

                    if (txtOrdNum.Text != "") 
                    {
                        OrderList = OrderList.Where(p => p.OrderNum.ToString().Contains(txtOrdNum.Text));
                    }

                    DataTable dtProdsDev = DataGridFunctions.DataTableFromIEnumerable(OrderList.ToList());

                    DataSet ds = new DataSet("Products");

                    var ProdList = from t1 in dtProdsDev.AsEnumerable()
                                   select new
                                   {
                                       ID = (int)t1["ID"],
                                       Username = (string)t1["UserName"],
                                       Price = Decimal.Round((decimal)t1["Price"], 2),
                                       OrderNum = (int)t1["OrderNum"],
                                       Paid = (bool)t1["Paid"],
                                       OrderDate = (DateTime)t1["OrderDate"]
                                   };

                    DataTable dt = new DataTable();
                    dt = DataGridFunctions.DataTableFromIEnumerable(ProdList);
                    gvOrder.DataSource = dt;
                    gvOrder.DataBind();

                    if (gvOrder.HeaderRow != null)
                    {
                        gvOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }

                    ViewState["gvDatatable"] = dt;
                    lblGridCount.Text = "Order Count: " + ProdList.Count();
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading Product List. Error Details: " + ex.Message);
            }
        }

        protected void getTotals()
        {
            using (MainEntityConnection db = new MainEntityConnection())
            {
                var OrderList = (from p in db.OrderMades
                                 where p.Paid == true
                                 select new
                                 {
                                     p.ID,
                                     p.Price,
                                 }).AsEnumerable().Select(x => new
                                 {
                                     x.ID,
                                     x.Price,
                                 });

                DataTable orders = DataGridFunctions.DataTableFromIEnumerable(OrderList.ToList());

                decimal ttlPrice = 0;
                foreach (DataRow row in orders.Rows)
                {
                    object price = row.ItemArray[1];
                    ttlPrice += (decimal)price;
                }

                lblTtlProfit.Text = "Total Profit: " + Decimal.Round(ttlPrice, 2);
            }
        }
        #endregion
    }
}