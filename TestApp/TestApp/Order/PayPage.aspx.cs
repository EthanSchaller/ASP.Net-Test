using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestApp
{
    public partial class PayPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (MainEntityConnection db = new MainEntityConnection())
            {
                string OrdID = Request.QueryString["ID"];
                int id = Convert.ToInt32(OrdID);

                OrderMade order = db.OrderMades.FirstOrDefault(i => i.ID == id);
                order.Paid = true;

                db.SaveChanges();
            }

            Response.Redirect("~/Order/OrderList.aspx");
        }
    }
}