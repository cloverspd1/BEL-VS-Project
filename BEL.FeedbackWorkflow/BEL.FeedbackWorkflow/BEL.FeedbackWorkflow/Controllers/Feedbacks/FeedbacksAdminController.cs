namespace BEL.FeedbackWorkflow.Controllers
{
    using BEL.CommonDataContract;
    using BEL.FeedbackWorkflow.BusinessLayer;
    using BEL.FeedbackWorkflow.Common;
    using BEL.FeedbackWorkflow.Models.Common;
    using BEL.FeedbackWorkflow.Models.Feedbacks;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using BEL.FeedbackWorkflow.Models.Master;

    /// <summary>
    /// Feedback Controller
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
        public ActionResult FeedbackAdminIndex(int id = 0)
        {
            if (id > 0 && CommonBusinessLayer.Instance.IsAdminUser(this.CurrentUser.UserId))
            {
                FeedbacksAdminContract contract = null;
                Logger.Info("Start Feedback Admin form and ID = " + id);
                Dictionary<string, string> objDict = new Dictionary<string, string>();
                objDict.Add(Parameter.FROMNAME, FormNameConstants.FeedbacksADMINFORM);
                objDict.Add(Parameter.ITEMID, id.ToString());
                objDict.Add(Parameter.USEREID, this.CurrentUser.UserId);
                ViewBag.UserID = this.CurrentUser.UserId;
                ViewBag.UserName = this.CurrentUser.FullName;
                contract = this.GetFeedbacksAdminDetails(objDict);
                contract.UserDetails = this.CurrentUser;

                if (contract != null && contract.Forms != null && contract.Forms.Count > 0)
                {
                    FeedbacksAdminDetailSection feedbackDetailSection = contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.FEEDBACKDETAILADMINSECTION) as FeedbacksAdminDetailSection;
                    ApplicationStatusSection feedbackApprovalSection = contract.Forms[0].SectionsList.FirstOrDefault(f => f.SectionName == SectionNameConstant.APPLICATIONSTATUS) as ApplicationStatusSection;

                    if (feedbackApprovalSection != null)
                    {
                        feedbackDetailSection.ApproversList = feedbackApprovalSection.ApplicationStatusList;
                        feedbackDetailSection.CCActingUserComments = feedbackDetailSection.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER) != null ? feedbackDetailSection.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Comments : string.Empty;
                        feedbackDetailSection.QualityUserComments = feedbackDetailSection.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.QAULITYUSER) != null ? feedbackDetailSection.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.QAULITYUSER).Comments : string.Empty;
                        feedbackDetailSection.CCQualityInchargeUserComments = feedbackDetailSection.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER) != null ? feedbackDetailSection.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).Comments : string.Empty;
                    }
                    return this.View("Admin/FeedbackAdminIndex", contract);
                }
                else
                {
                    return this.RedirectToAction("NotAuthorize", "Master");
                }
            }
            else
            {
                return this.RedirectToAction("NotAuthorize", "Master");
            }
        }
        #endregion

        #region "Save Feedbacks Detail Admin Section"
        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Content result
        /// </returns>    
        ////anti forgery attribute removed
        [HttpPost] 
        public ActionResult SaveFeedbacksAdminDetailSection(FeedbacksAdminDetailSection model)
        {
            ActionStatus status = new ActionStatus();
            if (model != null && ModelState.IsValid)
            {
                if (model.ApproversList != null)
                {
                    model.CCActingUser = model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Approver;
                    model.CCActingUserName = model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).ApproverName;
                }
                
                List<FileDetails> fileList = JsonConvert.DeserializeObject<List<FileDetails>>(model.FileNameList);
                if (fileList == null || fileList.Count == 0 || !fileList.Any(m => m.Status != FileStatus.Delete))
                {
                    status.IsSucceed = false;
                    status.Messages = new List<string>() { this.GetResourceValue("Error_FileNameList", System.Web.Mvc.Html.ResourceNames.Feedbacks) };
                    return this.Json(status);
                }
                model.Files = new List<FileDetails>();
                model.Files.AddRange(FileListHelper.GenerateFileBytes(model.FileNameList));
                model.Files.AddRange(FileListHelper.GenerateFileBytes(model.CCFileNameList));
                model.Files.AddRange(FileListHelper.GenerateFileBytes(model.CCQAInchargeFileNameList)); ////For Save Attachemennt
                model.FileNameList = string.Join(",", FileListHelper.GetFileNames(model.FileNameList));
                model.CCFileNameList = string.Join(",", FileListHelper.GetFileNames(model.CCFileNameList));
                model.CCQAInchargeFileNameList = string.Join(",", FileListHelper.GetFileNames(model.CCQAInchargeFileNameList));

                if (model.ApproversList != null)
                {
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCACTINGUSER).Comments = model.CCActingUserComments;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.QAULITYUSER).Comments = model.QualityUserComments;
                    model.ApproversList.FirstOrDefault(p => p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).Comments = model.CCQualityInchargeUserComments;
                }

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
    }
}