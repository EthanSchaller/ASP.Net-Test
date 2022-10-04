using System;
using System.Web;
using System.Web.UI.WebControls;

namespace TestApp
{
    public class Utility
    {
        public static string getWebsitePath()
        {
            string urlPath = string.Format("{0}://{1}{2}{3}",
                                    HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Host,
                                    HttpContext.Current.Request.Url.Port == 80
                                        ? string.Empty
                                        : ":" + HttpContext.Current.Request.Url.Port,
                                    HttpContext.Current.Request.ApplicationPath == "/"
                                        ? "/"
                                        : HttpContext.Current.Request.ApplicationPath + "/");

            return urlPath;
        }

        public static int GetColumnIndexByName(GridViewRow row, string columnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                    if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                        break;
                columnIndex++;
            }
            return columnIndex;
        }

        public static DateTime ConvertDateTimeToLocal(DateTime datetime)
        {
            int offset = Convert.ToInt32(HttpContext.Current.Request.Cookies["UTCOffset"].Value);
            return datetime.AddMinutes(-offset);
        }

        public static DateTime? ConvertDateTimeToLocal(DateTime? datetime)
        {
            int offset = Convert.ToInt32(HttpContext.Current.Request.Cookies["UTCOffset"].Value);

            if (datetime != null)
            {
                DateTime castDate = (DateTime)datetime;
                return castDate.AddMinutes(-offset);
            }
            else
            {
                return null;
            }
        }

        public static DateTime ConvertDateTimeToUTC(DateTime datetime)
        {
            int offset = Convert.ToInt32(HttpContext.Current.Request.Cookies["UTCOffset"].Value);
            return datetime.AddMinutes(offset);
        }

        public static string FormatDateTime(DateTime datetime)
        {
            return datetime.ToString("MM/d/yyyy hh:mm tt");
        }

        public static bool validateDate(string date)
        {
            DateTime dateValue;

            if (!DateTime.TryParse(date, out dateValue))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}