namespace BEL.FeedbackWorkflow.Controllers
{
    using BEL.CommonDataContract;
    using BEL.FeedbackWorkflow.BusinessLayer;
    using BEL.FeedbackWorkflow.Common;
    using BEL.FeedbackWorkflow.Models.Common;
    using BEL.FeedbackWorkflow.Models.Feedbacks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using BEL.FeedbackWorkflow.Models.Master;
    using System.Data;
    using BEL.FeedbackWorkflow.Models;

    /// <summary>
    /// Feedbacks Controller
    /// </summary>
    public partial class FeedbacksController : FeedbacksBaseController
    {
        #region "Index"
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Index View
        /// </returns>      
        [SharePointContextFilter]
        public ActionResult Index(int id = 0)
        {
            if (id == 0 && !FeedbacksBusinessLayer.Instance.IsCreator(this.CurrentUser.UserId))
            {
                return this.RedirectToAction("NotAuthorize", "Master");
            }
            FeedbacksContract contract = null;
            Logger.Info("Start Feedback form and ID = " + id);
            Dictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add(Parameter.FROMNAME, FormNameConstants.FeedbacksFORM);
            objDict.Add(Parameter.ITEMID, id.ToString());
            objDict.Add(Parameter.USEREID, this.CurrentUser.UserId);
            objDict.Add("BusinessUnit", "");

            ViewBag.UserID = this.CurrentUser.UserId;
            ViewBag.UserName = this.CurrentUser.FullName;
            contract = this.GetFeedbacksDetails(objDict);
            contract.UserDetails = this.CurrentUser;

            if (contract != null && contract.Forms != null && contract.Forms.Count > 0)
            {
                return this.View(contract);
            }
            else
            {
                return this.RedirectToAction("NotAuthorize", "Master");
            }
        }
        #endregion

        #region "Save Feedbacks Detail Section"
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Content result
        /// </returns>
        ////anti forgery attribute removed
        [HttpPost]
        public ActionResult SaveFeedbacksDetailSection(FeedbacksDetailSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && model.ButtonCaption == " Forward to Quality User")
            {
                //ModelState.Remove("CCQualityInchargeUser");
                model.ActionStatus = model.ActionStatus == ButtonActionStatus.NextApproval ? ButtonActionStatus.SendForward : model.ActionStatus;
                ModelState.Remove("CCQualityInchargeUser");
                ModelState.Remove("CCActingUser");
                model.flag = true;
                model.SampleProvidedDate = DateTime.Now;
                
            }


            if (model != null && this.ValidateModelState(model))
            {
                if (model.ApproversList != null)
                {
                    if (model.ApproversList != null && model.BusinessUnits== "Illumination S" && model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).Approver = model.LUMActingUser;
                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).ApproverName = model.LUMActingUserName;

                        // model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYINCHARGE).Approver = "";
                        //model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Approver = "";

                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).Approver = "";
                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).ApproverName = "";


                        ////model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQualityIncharge).Approver = {assign user from approver master}
                        //// model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Approver = ""
                        //// model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQualityInchargeUser).Approver = ""
                        if (model.ProposedBy == model.LUMActingUser)
                        {
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).Approver = "";
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).ApproverName = "";
                        }
                        else
                        {
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).Approver = model.LUMActingUser;
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).ApproverName = model.LUMActingUserName;
                        }
                        //// if(currentUser == LUM Service manager Id)
                        ////then  model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).Approver = ""
                        //// else model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMSERVICEMANAGERS).Approver = model.LUMActingUser;

                    }

                    else if (model.ApproversList != null && model.BusinessUnits != "Illumination S" && model.ActionStatus == ButtonActionStatus.NextApproval)
                    {
                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Approver = model.CCActingUser;
                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).ApproverName = model.CCActingUserName;

                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYINCHARGE).Approver = "";
                        model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYINCHARGE).ApproverName = "";

                        if (model.ProposedBy == model.CCActingUser)
                        {
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Approver = "";
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).ApproverName = "";
                        }
                        else
                        {
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Approver = model.CCActingUser;
                            model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).ApproverName = model.CCActingUserName;
                        }

                    }
                }
               

                model.ProductGroup = model.ProductType;
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.FileNameList);
                model.FileNameList = string.Join(",", FileListHelper.GetFileNames(model.FileNameList));
               
                ////For Save Attachemennt
                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                //objDict.Add("Flag", Flag);
                //    objDict.Add("CreatorToQA", model.QualityUserCreator);
                //    objDict.Add("CreatorToQAName", model.QualityUserCreatorName);
                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }

        #endregion

        #region "Save CC Acting User Section"
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Content result
        /// </returns>
        ////anti forgery attribute removed
        [HttpPost]
        public ActionResult SaveCCActingUserSection(CCActingUserSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && model.ForwardtoCCQualityIncharge == false)
            {
                ModelState.Remove("CCQualityInchargeUser");
                model.ActionStatus = model.ActionStatus == ButtonActionStatus.NextApproval ? ButtonActionStatus.Complete : model.ActionStatus;
            }

            if (model != null && this.ValidateModelState(model))
            {
                if (model.ApproversList != null && model.ForwardtoCCQualityIncharge && model.ActionStatus == ButtonActionStatus.NextApproval)
                {
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).Approver = model.CCQualityInchargeUser;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).ApproverName = model.CCQualityInchargeName;

                }
                else if (!model.ForwardtoCCQualityIncharge && model.ActionStatus == ButtonActionStatus.Complete)
                {
                    model.CCQualityInchargeUser = model.CCQualityInchargeName = string.Empty;
                    model.QualityInchargeDate = DateTime.Now;
                    model.QualityUserDate = DateTime.Now;
                }
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.CCFileNameList);  ////For Save Attachemennt
                model.CCFileNameList = string.Join(",", FileListHelper.GetFileNames(model.CCFileNameList));

                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }


        [HttpPost]
        public ActionResult SaveLUMActingUserSection(LUMActingUserSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && model.ForwardtoCCQualityIncharge == false)
            {
                ModelState.Remove("LUMQualityInchargeUser");
                model.ActionStatus = model.ActionStatus == ButtonActionStatus.NextApproval ? ButtonActionStatus.Complete : model.ActionStatus;
            }

            if (model != null && this.ValidateModelState(model))
            {
                if (model.ApproversList != null && model.ForwardtoCCQualityIncharge && model.ActionStatus == ButtonActionStatus.NextApproval)
                {
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYINCHARGE).Approver = model.LUMQualityInchargeUser;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYINCHARGE).ApproverName = model.LUMQualityInchargeName;

                }
                else if (!model.ForwardtoCCQualityIncharge && model.ActionStatus == ButtonActionStatus.Complete)
                {
                    model.LUMQualityInchargeUser = model.LUMQualityInchargeName = string.Empty;
                    model.QualityInchargeDate = DateTime.Now;
                    model.QualityUserDate = DateTime.Now;
                }
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.CCFileNameList);  ////For Save Attachemennt
                model.CCFileNameList = string.Join(",", FileListHelper.GetFileNames(model.CCFileNameList));

                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }
        #endregion

        #region "Save QA Incharge Section"
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Content result
        /// </returns>
        ////anti forgery attribute removed
        [HttpPost]
        public ActionResult SaveCCQualityInchargeSection(CCQualityInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && model.ForwardtoQuality == false)
            {
                ModelState.Remove("QAUser");
                model.ActionStatus = model.ActionStatus == ButtonActionStatus.NextApproval ? ButtonActionStatus.Complete : model.ActionStatus;
            }

            if (model != null && this.ValidateModelState(model))
            {
                if (model.ApproversList != null && model.ForwardtoQuality && model.ActionStatus == ButtonActionStatus.NextApproval && model.BUHidden!="Illumination S")
                {
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.QAULITYUSER).Approver = model.QAUser;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.QAULITYUSER).ApproverName = model.QAUserName;
                }
                else if (model.ApproversList != null && model.ForwardtoQuality && model.ActionStatus == ButtonActionStatus.NextApproval && model.BUHidden == "Illumination S")
                {
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYUSER).Approver = model.LUMQAUser;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYUSER).ApproverName = model.LUMQAUserName;
                    model.QAUser = model.LUMQAUser;
                    model.QAUserName = model.LUMQAUserName;
                }
                else if (!model.ForwardtoQuality && model.ActionStatus == ButtonActionStatus.Complete)
                {
                    model.QAUser = model.QAUserName = string.Empty;
                   
                }
                model.QualityInchargeDate = DateTime.Now;
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.CCQAInchargeFileNameList);  ////For Save Attachemennt
                model.CCQAInchargeFileNameList = string.Join(",", FileListHelper.GetFileNames(model.CCQAInchargeFileNameList));

                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                
                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }

        [HttpPost]
        public ActionResult SaveLUMQualityInchargeSection(LUMQualityInchargeSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && model.ForwardtoQuality == false)
            {
                ModelState.Remove("LUMQAUser");
                model.ActionStatus = model.ActionStatus == ButtonActionStatus.NextApproval ? ButtonActionStatus.Complete : model.ActionStatus;
            }

            if (model != null && this.ValidateModelState(model))
            {
               
                 if (model.ApproversList != null && model.ForwardtoQuality && model.ActionStatus == ButtonActionStatus.NextApproval && model.BUHidden == "Illumination S")
                {
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYUSER).Approver = model.LUMQAUser;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.LUMQUALITYUSER).ApproverName = model.LUMQAUserName;
                    //model.QAUser = model.LUMQAUser;
                    //model.QAUserName = model.LUMQAUserName;
                }
                else if (!model.ForwardtoQuality && model.ActionStatus == ButtonActionStatus.Complete)
                {
                    model.LUMQAUser = model.LUMQAUserName = string.Empty;

                }
                model.QualityInchargeDate = DateTime.Now;
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.CCQAInchargeFileNameList);  ////For Save Attachemennt
                model.CCQAInchargeFileNameList = string.Join(",", FileListHelper.GetFileNames(model.CCQAInchargeFileNameList));

                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);

                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }

        #endregion

        #region "Save QA User Section"
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Content result
        /// </returns>
        ////anti forgery attribute removed
        [HttpPost]
        public ActionResult SaveQaulityUserSection(QaulityUserSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && this.ValidateModelState(model))
            {
                if (model.SendBackToCC == true)
                {
                    model.QualityUserDate = DateTime.Now;
                }
                else
                {
                    model.ImplementationDate = DateTime.Now;
                    model.QualityUserDate = DateTime.Now;
                    
                }
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.QUFileNameList);  ////For Save Attachemennt
                model.QUFileNameList = string.Join(",", FileListHelper.GetFileNames(model.QUFileNameList));


                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                objDict.Add("DefectDesc1", model.Implemented);
                objDict.Add("DefectDesc2", model.CurrentApprover.ImplementedRemark);
                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }

        public ActionResult SaveLUMQaulityUserSection(LUMQualityUserSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && this.ValidateModelState(model))
            {
                if (model.SendBackToCC == true)
                {
                    model.QualityUserDate = DateTime.Now;
                }
                else
                {
                    model.ImplementationDate = DateTime.Now;
                    model.QualityUserDate = DateTime.Now;

                }
                model.Files = new List<FileDetails>();
                model.Files = FileListHelper.GenerateFileBytes(model.QUFileNameList);  ////For Save Attachemennt
                model.QUFileNameList = string.Join(",", FileListHelper.GetFileNames(model.QUFileNameList));


                Dictionary<string, string> objDict = this.GetSaveDataDictionary(this.CurrentUser.UserId, model.ActionStatus.ToString(), model.ButtonCaption);
                objDict.Add("DefectDesc1", model.Implemented);
                objDict.Add("DefectDesc2", model.CurrentApprover.ImplementedRemark);
                status = this.SaveSection(model, objDict);
                status = this.GetMessage(status, System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            else
            {
                status.IsSucceed = false;
                status.Messages = this.GetErrorMessage(System.Web.Mvc.Html.ResourceNames.Feedbacks);
            }
            return this.Json(status);
        }
        #endregion

        /// <summary>
        /// Gets the All Items search by Model or Item code.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <returns>json object</returns>
        public JsonResult GetAllItems(string q)
        {
            string data = this.GetAllItemsCode(q);
            if (!string.IsNullOrEmpty(data))
            {
                var master = BEL.DataAccessLayer.JSONHelper.ToObject<ItemMaster>(data);
                return this.Json((from item in master.Items select new { id = item.Title, name = item.Title }).ToList(), JsonRequestBehavior.AllowGet);
            }
            return this.Json("[]", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Artwork Info
        /// </summary>
        /// <param name="itemCode">item code</param>
        /// <returns>json data</returns>
        public JsonResult GetSKUInfo(string itemCode)
        {
            string data = this.GetSKUInfoByItemCode(itemCode);
            if (!string.IsNullOrEmpty(data))
            {
                var master = BEL.DataAccessLayer.JSONHelper.ToObject<ItemMasterListItem>(data);
                return this.Json(master, JsonRequestBehavior.AllowGet);
            }
            return this.Json("[]", JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetVariableDefine(string BU)
        //{
        //    string data = BU;
        //    string result = "";
        //    if (!string.IsNullOrEmpty(data))
        //    {
        //        int id = 0;
        //        FeedbacksContract contract = null;
        //        Logger.Info("Start Feedback form and ID = " + id);
        //        Dictionary<string, string> objDict = new Dictionary<string, string>();
        //        objDict.Add(Parameter.FROMNAME, FormNameConstants.FeedbacksFORM);
        //        objDict.Add(Parameter.ITEMID, id.ToString());
        //        objDict.Add(Parameter.USEREID, this.CurrentUser.UserId);
        //        objDict.Add("BusinessUnit", BU);

        //        ViewBag.UserID = this.CurrentUser.UserId;
        //        ViewBag.UserName = this.CurrentUser.FullName;
        //        contract = this.GetFeedbacksDetails(objDict);
        //        contract.UserDetails = this.CurrentUser;
        //        if (contract.dynmicfield.Count > 0)
        //        {
        //            result = "True";
        //        }
        //        else
        //        {
        //            result = "False";
        //        }
        //        //return this.Json(master, JsonRequestBehavior.AllowGet);
        //    }
        //    return this.Json(result, JsonRequestBehavior.AllowGet);
        //}

        #region "Feedback Report"
        /// <summary>
        /// Feedbacks the report.
        /// </summary>
        /// <returns>Action Result</returns>
        [SharePointContextFilter]
        public ActionResult FeedbackReport()
        {
            Dictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("FormName", FormNameConstants.FeedbacksFORM);
            objDict.Add(Parameter.USEREID, this.CurrentUser.UserId);
            objDict.Add(Parameter.USEREMAIL, this.CurrentUser.UserEmail);
            List<ISection> reportData = FeedbacksBusinessLayer.Instance.RetrieveFeedback(objDict);
            return this.View("Reports/FeedbackReport", reportData);
        }

        /// <summary>
        /// Gets the feedback report export to excel.
        /// </summary>
        public void GetFeedbackReportExportToExcel()
        {
            Logger.Info("GetFeedbackReportExportToExcel starts");
            Dictionary<string, string> objDict = new Dictionary<string, string>();
            objDict.Add("FormName", FormNameConstants.FeedbacksFORM);
            objDict.Add(Parameter.USEREID, this.CurrentUser.UserId);
            objDict.Add(Parameter.USEREMAIL, this.CurrentUser.UserEmail);
            Logger.Info("RetrieveFeedback starts");
            List<ISection> reportData = FeedbacksBusinessLayer.Instance.RetrieveFeedback(objDict);
            Logger.Info("RetrieveFeedback end");
           
            if (reportData != null && reportData.Any())
            {
                Logger.Info("reportData != null");
                List<FeedbackReportSection> result = new List<FeedbackReportSection>();
                foreach (FeedbackReportSection item in reportData)
                {
                    result.Add(item);
                }
                using (DataSet dataSet = new DataSet())
                {
                    DataTable dt = DataTableExtensoins.ToDataTable(result);
                    dt.Columns.Remove("ID");
                    dt.Columns.Remove("CCQualityInchargeUser");
                    dt.Columns.Remove("CCActingUser");
                    dt.Columns.Remove("SectionName");
                    dt.Columns.Remove("IsActive");
                    dt.Columns.Remove("ListDetails");
                    dt.Columns.Remove("ActionStatus");
                    dt.Columns.Remove("CurrentApprover");
                    dt.Columns.Remove("ApproversList");
                    dt.Columns.Remove("ButtonCaption");
                    dt.Columns.Remove("FormBelongTo");
                    dt.Columns.Remove("ApplicationBelongTo");
                    dataSet.Tables.Add(dt);
                    Logger.Info("DataTable created, dt =" + dt);
                    ////ExportToExcelHelper.GetByteArrayExcel(dataSet);

                    byte[] byteArray = ExportToExcelHelper.GetByteArrayExcel(dataSet);
                    Logger.Info("byteArray created");
                    Response.Clear();
                    ////Add a HTTP header to the output stream that specifies the default filename
                    ////for the browser's download dialog
                    Logger.Info("Content-Disposition , filename =FeedbackReport_" + DateTime.Now.ToShortDateString() + ".xls");
                    Response.AddHeader("Content-Disposition", "attachment; filename=FeedbackReport_" + DateTime.Now.ToShortDateString() + ".xls");
                    //// Add a HTTP header to the output stream that contains the 
                    //// content length(File Size). This lets the browser know how much data is being transfered

                    Logger.Info("Content-Length = " + byteArray.Length.ToString());
                    Response.AddHeader("Content-Length", byteArray.Length.ToString());
                    //// Set the HTTP MIME type of the output stream
                    Response.ContentType = "application/octet-stream";
                    //// Write the data out to the client.
                    Response.BinaryWrite(byteArray);
                    Logger.Info(" after Response.BinaryWrite(byteArray)");
                }
               
            }
            Logger.Info("GetFeedbackReportExportToExcel ends");
        }
        #endregion
    }
}