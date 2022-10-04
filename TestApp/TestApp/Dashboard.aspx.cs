using System;

namespace TestApp
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoggedInUser usr = WebUtility.getCurrentUser();

            if (usr.Role == LoggedInUser.CurrentRole.Owner) 
            {
                ownerDashboard.Visible = true;
                adminDashboard.Visible = false;
                guestDashboard.Visible = false;
            } 
            else if (usr.Role == LoggedInUser.CurrentRole.Admin)
            {
                ownerDashboard.Visible = false;
                adminDashboard.Visible = true;
                guestDashboard.Visible = false;
            } 
            else 
            {
                ownerDashboard.Visible = false;
                adminDashboard.Visible = false;
                guestDashboard.Visible = true;
            }
        }
    }
}