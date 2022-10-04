using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using TestApp;

namespace Jobber
{
    /// <summary>
    /// Summary description for DBImageHandler
    /// </summary>
    public class DBImageHandler : IHttpHandler
    {
        private void returnStatusCode(HttpContext context, int httpStatusCode)
        {
            context.Response.StatusCode = httpStatusCode;
            context.Response.End();
        }


        public void ProcessRequest(HttpContext context)
        {
            string imgQuery = context.Request.QueryString["img"];
            int img = Convert.ToInt32(imgQuery);

            using (MainEntityConnection db = new MainEntityConnection())
            {
                MemoryStream memoryStream = null;

                if (img != 0)
                {
                    Product product = db.Products.FirstOrDefault(u => u.Image.Equals(img));

                    if (product.Image == 0) // no image
                    {
                        returnStatusCode(context, 404);
                        return;
                    }

                    memoryStream = new MemoryStream(product.ImageRef.Image);
                }

                try
                {
                    byte[] imgBytes = memoryStream.ToArray();
                    context.Response.ContentType = "image/png"; // png
                    context.Response.BinaryWrite((byte[])imgBytes);
                    context.Response.Flush();
                }
                catch(IOException iox)
                {
                    // perhaps do something usefull.
                    System.Diagnostics.Debug.Write(iox.Message);
                    returnStatusCode(context, 500);
                }
            }        
        }
        
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}