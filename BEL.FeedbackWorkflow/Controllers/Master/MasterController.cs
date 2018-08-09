namespace BEL.FeedbackWorkflow.Controllers
{
    using BEL.CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Http;
    using System.Web.Helpers;
    using BEL.FeedbackWorkflow.Common;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.IO;
    using BEL.FeedbackWorkflow.BusinessLayer;

    /// <summary>
    /// Master Data Controller
    /// </summary>
    public class MasterController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Index View</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Gets the masters.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <param name="d">The d.</param>
        /// <returns>
        /// list of master data
        /// </returns>
        public JsonResult GetUsers(string q, string d = null)
        {
            ////dal
            //using (CommonServiceClient client = new CommonServiceClient())
            //{
            //var newList = client.GetUsers(q, d);
            //JsonResult jResult = this.Json((from n in newList select new { id = n.Key, name = n.Value }).ToList(), JsonRequestBehavior.AllowGet);
            //return jResult;
            //}
            return null;
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="userEmail">The user email.</param>
        /// <returns>
        /// User Information
        /// </returns>
        public JsonResult GetUserInfo(string userEmail)
        {
            ////using (CommonServiceClient client = new CommonServiceClient())
            ////{
            ////    var user = client.GetUserByEmail(userEmail);
            ////    return this.Json(user, JsonRequestBehavior.AllowGet);
            ////}
            ////DAL
            return null;
        }

        /// <summary>
        /// Errors the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns>
        /// error view
        /// </returns>
        public ActionResult Error(string msg)
        {
            return this.View();
        }

        /// <summary>
        /// Nots the authorize.
        /// </summary>
        /// <returns>NotAuthorize View</returns>
        public ActionResult NotAuthorize()
        {
            return this.View();
        }

        /// <summary>
        /// Get Token for CSRF
        /// </summary>
        /// <returns>Json string</returns>
        public JsonResult GetTocken()
        {
            return this.Json("Verified", JsonRequestBehavior.AllowGet);
            ////try
            ////{
            ////    string cookieToken, formToken;
            ////    AntiForgery.GetTokens(null, out cookieToken, out formToken);
            ////    HttpContext.Cache["_formToken"] = formToken;
            ////    return this.Json(cookieToken + ":" + formToken, JsonRequestBehavior.AllowGet);
            ////}
            ////catch (Exception ex)
            ////{
            ////    Logger.Error(ex);
            ////    return this.Json("XYZ123", JsonRequestBehavior.AllowGet);
            ////}
        }

        #region "BulkUpload"
        /// <summary>
        /// Bulk Upload
        /// </summary>
        /// <returns>Action Status</returns>
        [SharePointContextFilter]
        public ActionResult BulkUpload()
        {
            return this.View("BulkUpload");
        }

        /// <summary>
        /// Upload SKU Master Data
        /// </summary>
        /// <param name="qqfile">the sheet</param>
        /// <returns>Json Result</returns>
        public JsonResult UploadSKUMasterData(string qqfile)
        {
            try
            {
                var stream = this.Request.InputStream;
                string extension = System.IO.Path.GetExtension(qqfile);
                if (string.IsNullOrEmpty(this.Request["qqfile"]))
                {
                    //// IE Fix
                    HttpPostedFileBase postedFile = this.Request.Files[0];
                    stream = postedFile.InputStream;
                }

                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(stream))
                {
                    fileData = binaryReader.ReadBytes((int)stream.Length);
                }
                ////DataTable dt = this.ProcessCSV(fileData);
                ExcelHelper excelhelper = new ExcelHelper();
                DataTable dt = excelhelper.UploadExcelFile(fileData, extension);
                if (dt != null && dt.Columns.Count != 4)
                {
                    return this.Json(new { IndividualUpload = true, Status = false, Message = this.GetResourceValue("Text_BulkInValidformat", System.Web.Mvc.Html.ResourceNames.Common) });
                }
                if (dt != null && dt.Rows.Count >= 1)
                {
                    Dictionary<string, string> returnData = CommonBusinessLayer.Instance.UpdateBulkData(dt);
                    if (returnData != null && returnData.Count != 0)
                    {
                        string html = string.Empty;
                        if (returnData.ContainsKey("TotalRecords"))
                        {
                            html += "<b>Total SKU Item Uploaded:</b>" + returnData["TotalRecords"].ToString() + "<br>";
                        }
                        if (returnData.ContainsKey("Added"))
                        {
                            html += "<b>Total New SKU Added:</b>" + returnData["Added"].ToString() + "<br>";
                        }
                        if (returnData.ContainsKey("ErrorIDs") && Convert.ToInt32(returnData["Error"]) != 0)
                        {
                            html += "<b>Total Invalid SKU Data :</b>" + returnData["Error"].ToString() + "<br>";
                        }
                        if (returnData.ContainsKey("ErrorIDs") && !string.IsNullOrEmpty(returnData["ErrorIDs"].ToString()))
                        {
                            html += "<b>Invalid SKU Item Codes:</b>" + returnData["ErrorIDs"].ToString() + "<br>";
                        }
                        Logger.Info("Bulk Upload Result: " + html);
                        return this.Json(new { IndividualUpload = true, Status = true, Message = this.GetResourceValue("Text_BulkUploadSuccess", System.Web.Mvc.Html.ResourceNames.Common) + "<br>" + html, strHTML = html });
                    }
                    else
                    {
                        return this.Json(new { IndividualUpload = true, Status = false, Message = this.GetResourceValue("Text_BulkUploadError", System.Web.Mvc.Html.ResourceNames.Common) });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return this.Json(new { IndividualUpload = true, Status = false, Message = this.GetResourceValue("Text_BulkUploadError", System.Web.Mvc.Html.ResourceNames.Common) });
            }

            return this.Json(new { IndividualUpload = true, Status = false, Message = this.GetResourceValue("Text_BulkUploadError", System.Web.Mvc.Html.ResourceNames.Common) });
        }

        /// <summary>
        /// Processes the CSV.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>DataTable of CSV</returns>
        public DataTable ProcessCSV(byte[] array)
        {
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            using (MemoryStream ms = new MemoryStream(array))
            {
                StreamReader sr = new StreamReader(ms);
                line = sr.ReadLine();
                strArray = r.Split(line);
                Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));
                while ((line = sr.ReadLine()) != null)
                {
                    row = dt.NewRow();
                    row.ItemArray = r.Split(line);
                    dt.Rows.Add(row);
                }
                sr.Dispose();
                return dt;
            }
        }
        #endregion
    }
}