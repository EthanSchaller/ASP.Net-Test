using System;
using System.Linq;
using System.Web.UI;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.Net.Mail;

namespace TestApp
{
    public partial class ProdsEdit : System.Web.UI.Page
    {
        System.Drawing.Bitmap m_bitmap;

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FileUploader.PostedFile != null && FileUploader.PostedFile.ContentLength > 0) UploadImage();

            m_bitmap = (System.Drawing.Bitmap)Session["LogoBitmap"];

            if (!IsPostBack)
            {
                Session["LogoBitmap"] = null;

                string prodID = Request.QueryString["ID"];
                hidProdID.Value = prodID;

                checkRole();

                if (prodID != null)
                {
                    loadProd();
                    formMode(false);
                }
                else
                {
                    formMode(true);
                    btnCancel.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }

        private void checkRole()
        {
            try
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                if (usr.Role < LoggedInUser.CurrentRole.Admin)
                {
                    Response.Redirect("~/Dashboard.aspx");
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error validating the user's role. \nError Details: " + ex.Message);
            }
        }
        #endregion

        #region ButtonEvents
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string ItemID = hidProdID.Value;

            if (validate())
            {
                LoggedInUser usr = WebUtility.getCurrentUser();
                if ((ItemID == null || ItemID == ""))
                {
                    try
                    {
                        using (MainEntityConnection db = new MainEntityConnection())
                        {
                            Product product = new Product();
                            product.Name = txtName.Text;
                            product.Price = Convert.ToDecimal(txtPrice.Text);
                            product.Desc = txtDesc.Text; 
                            
                            if (m_bitmap != null) // a new image was uploaded
                            {
                                byte[] byteArray = new byte[0];
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    // create temp (new) copy of the image, workaround for gdi+ error due to locking streams 
                                    // on certain types of images .. 
                                    // copy m_bitmap to a new bitmap, and then save *it* to the stream... 
                                    Bitmap tmpBmp = new Bitmap(m_bitmap);
                                    tmpBmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                                    stream.Close();

                                    byteArray = stream.ToArray();
                                }

                                //save the new byte array into the database
                                ImageRef image = new ImageRef();
                                image.Image = byteArray;
                                image.IsActive = true;
                                image.IsDeleted = false;
                                image.CreatedBy = usr.Username;
                                image.CreatedOn = DateTime.Now;
                                image.ModifiedBy = usr.Username;
                                image.ModifiedOn = DateTime.Now;

                                db.ImageRefs.Add(image);

                                product.Image = image.ImageID;
                            }

                            product.IsDeleted = false;
                            product.CreatedBy = usr.Email;
                            product.CreatedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);
                            product.ModifiedBy = usr.Email;
                            product.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                            db.Products.Add(product);

                            db.SaveChanges();

                            hidProdID.Value = product.ID.ToString();

                            loadProd();
                            formMode(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Site mPage = (Site)Page.Master;
                        mPage.displayAlert(4, "There was an error adding the Product. Error Details: " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        using (MainEntityConnection db = new MainEntityConnection())
                        {
                            int id = Convert.ToInt32(ItemID);
                            Product product = db.Products.FirstOrDefault(i => i.ID == id);

                            product.Name = txtName.Text;
                            product.Desc = txtDesc.Text;

                            if (m_bitmap != null) // a new image was uploaded
                            {
                                byte[] byteArray = new byte[0];
                                using (MemoryStream stream = new MemoryStream())
                                {
                                    // create temp (new) copy of the image, workaround for gdi+ error due to locking streams 
                                    // on certain types of images .. 
                                    // copy m_bitmap to a new bitmap, and then save *it* to the stream... 
                                    Bitmap tmpBmp = new Bitmap(m_bitmap);
                                    tmpBmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                                    stream.Close();

                                    byteArray = stream.ToArray();
                                }

                                //save the new byte array into the database
                                ImageRef image = new ImageRef();
                                image.Image = byteArray;
                                image.IsActive = true;
                                image.IsDeleted = false;
                                image.CreatedBy = usr.Username;
                                image.CreatedOn = DateTime.Now;
                                image.ModifiedBy = usr.Username;
                                image.ModifiedOn = DateTime.Now;

                                db.ImageRefs.Add(image);

                                product.Image = image.ImageID;
                            }

                            product.ModifiedBy = usr.Email;
                            product.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                            db.SaveChanges();

                            hidProdID.Value = product.ID.ToString();

                            loadProd();

                            formMode(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Site mPage = (Site)Page.Master;
                        mPage.displayAlert(4, "There was an error updating the Product. Error Details: " + ex.Message);
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, System.EventArgs e)
        {
            formMode(true);
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            string ItemID = hidProdID.Value;

            using (MainEntityConnection db = new MainEntityConnection())
            {
                int id = Convert.ToInt32(ItemID);
                Product product = db.Products.FirstOrDefault(i => i.ID == id);
            }

            formMode(false);
            loadProd();
            validate();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                displayDeleteModal();
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error deleting the User. Error Details: " + ex.Message);
            }
        }

        protected void btnDeleteConfirm_Click(object sender, System.EventArgs e)
        {
            try
            {
                LoggedInUser usr = WebUtility.getCurrentUser();

                using (MainEntityConnection db = new MainEntityConnection())
                {
                    int id = Convert.ToInt32(hidProdID.Value);
                    Product product = db.Products.First(i => i.ID == id);

                    product.IsDeleted = true;
                    product.ModifiedBy = usr.Email;
                    product.ModifiedOn = Utility.ConvertDateTimeToUTC(DateTime.Now);

                    db.SaveChanges();
                    Response.Redirect("~/Product/ProdList.aspx");
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error deleting the Product. Error Details: " + ex.Message);
            }
        }
        #endregion

        #region Utilities
        private void loadProd()
        {
            try
            {
                using (MainEntityConnection db = new MainEntityConnection())
                {
                    string itemID = hidProdID.Value;
                    int id = Convert.ToInt32(itemID);
                    Product product = db.Products.FirstOrDefault(i => i.ID == id);

                    if (product == null || product.IsDeleted)
                    {
                        Response.Redirect("~/Product/ProdList.aspx");
                    }

                    txtName.Text = product.Name;
                    txtPrice.Text = Decimal.Round(product.Price, 2).ToString();
                    txtDesc.Text = product.Desc; 
                    
                    if (product.Image != 0)
                    {
                        imgUpload.ImageUrl = "../classes/DBImageHandler.ashx?img=" + product.Image.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error loading the Product Details. Error Details: " + ex.Message);
            }
        }

        private void formMode(bool mode)
        {
            FileUploader.Visible = mode;
            txtName.ReadOnly = !mode;
            txtPrice.ReadOnly = !mode;
            txtDesc.ReadOnly = !mode;

            btnSave.Visible = mode;
            btnCancel.Visible = mode;
            btnDelete.Visible = mode;
            btnEdit.Visible = !mode;
        }

        private bool validate()
        {
            bool isValid = true;
            try
            {
                if (txtName.Text == "")
                {
                    BootstrapErrors.AddError(txtName);
                    isValid = false;
                }
                else
                {
                    BootstrapErrors.RemoveError(txtName);
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was problem validating the Product. Error Details: " + ex.Message);
            }
            return isValid;
        }

        private void displayDeleteModal()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openDeleteModal();", true);
        }
        #endregion

        #region ImageUploading
        private void UploadImage()
        {
            try
            {
                if (FileUploader.HasFile)
                {
                    using (MemoryStream mem = new MemoryStream(FileUploader.FileBytes))
                    {
                        if (!IsFileAnImage(mem))
                            return;

                        System.Drawing.Bitmap bmp = new Bitmap(mem);

                        //if (bmp.Width > 300 || bmp.Height > 100)
                        //{
                        // resize
                        bmp = resizeImage(bmp, 200, 200);
                        //}

                        MemoryStream tmpStream = new MemoryStream();

                        bmp.Save(tmpStream, System.Drawing.Imaging.ImageFormat.Png);

                        Session["LogoBytes"] = tmpStream.ToArray();
                        tmpStream.Close();
                        imgUpload.ImageUrl = "../classes/UploadImageHandler.ashx";
                        //m_uploadImage = true;

                        Session["LogoBitmap"] = bmp;

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an problem uploading the image. Error Details: " + ex.Message);
            }
        }

        public Bitmap resizeImage(Bitmap image, int maxWidth, int maxHeight)
        {
            try
            {
                // Get the image's original width and height
                int originalWidth = image.Width;
                int originalHeight = image.Height;

                // To preserve the aspect ratio
                float ratioX = (float)maxWidth / (float)originalWidth;
                float ratioY = (float)maxHeight / (float)originalHeight;
                float ratio = Math.Min(ratioX, ratioY);

                // New width and height based on aspect ratio
                int newWidth = (int)(originalWidth * ratio);
                int newHeight = (int)(originalHeight * ratio);

                Bitmap newImage = new Bitmap(newWidth, newHeight);

                // Draws the image 
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }

                return newImage;
            }
            catch (Exception ex)
            {
                Site mPage = (Site)Page.Master;
                mPage.displayAlert(4, "There was an error resizing the image. Error Details: " + ex.Message);
                return image;
            }
        }

        private bool IsFileAnImage(MemoryStream mem)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(mem);
                img.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false; // not an image
            }
        }
        #endregion   
    }
}