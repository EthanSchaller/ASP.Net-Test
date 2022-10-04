using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace TestApp
{
    public class BootstrapErrors
    {
        public static void AddError(TextBox txt)
        {
            txt.Attributes.Add("class", txt.Attributes["class"] + " is-invalid");
        }

        public static void RemoveError(TextBox txt)
        {
            if (txt.Attributes["class"] == null)
            {
                throw new ArgumentException("HTML control must have class attribute");
            }

            txt.Attributes.Add("class", txt.Attributes["class"].ToString().Replace("is-invalid", ""));
        }

        public static void AddError(DropDownList ddl)
        {
            ddl.Attributes.Add("class", ddl.Attributes["class"] + " is-invalid");
        }


        public static void RemoveError(DropDownList ddl)
        {
            if (ddl.Attributes["class"] == null)
            {
                throw new ArgumentException("HTML control must have class attribute");
            }

            ddl.Attributes.Add("class", ddl.Attributes["class"].ToString().Replace("is-invalid", "is-valid"));
        }

        public static void AddError(HtmlGenericControl control, HtmlInputText inp, string message)
        {
            control.Attributes.Add("class", control.Attributes["class"] + " has-error");

            inp.Attributes.Add("data-toggle", "tooltip");
            inp.Attributes.Add("data-placement", "right");
            inp.Attributes.Add("title", message);
        }

        public static void RemoveError(HtmlGenericControl control, HtmlInputText inp)
        {
            if (control.Attributes["class"] == null)
            {
                throw new ArgumentException("HTML control must have class attribute");
            }

            control.Attributes.Add("class", control.Attributes["class"].ToString().Replace("has-error", ""));

            inp.Attributes.Add("data-toggle", "");
            inp.Attributes.Add("data-placement", "");
            inp.Attributes.Add("title", "");
        }
    }
}