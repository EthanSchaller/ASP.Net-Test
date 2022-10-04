using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBC_Sailing
{
    /// <summary>
    /// Summary description for UploadImageHandler
    /// </summary>
    public class UploadImageHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
           // check whether the session has any image bytes
            if ((context.Session["LogoBytes"]) != null)
            {
                byte[] image = (byte[])(context.Session["LogoBytes"]);
                context.Response.ContentType = "image/PNG";
                context.Response.BinaryWrite(image);              
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}