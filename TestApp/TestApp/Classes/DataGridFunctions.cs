using System;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Web.UI.WebControls;

namespace TestApp
{
    public class DataGridFunctions
    {
        public static DataTable DataTableFromIEnumerable(IEnumerable ien)
        {
            DataTable dt = new DataTable();
            foreach (object obj in ien)
            {
                Type t = obj.GetType();
                PropertyInfo[] pis = t.GetProperties();
                if (dt.Columns.Count == 0)
                {
                    foreach (PropertyInfo pi in pis)
                    {
                        dt.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
                    }
                }

                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pi in pis)
                {
                    object value = pi.GetValue(obj, null);
                    if (value != null)
                    {
                        dr[pi.Name] = value;
                    }
                    else
                    {
                        dr[pi.Name] = DBNull.Value;
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
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
    }
}