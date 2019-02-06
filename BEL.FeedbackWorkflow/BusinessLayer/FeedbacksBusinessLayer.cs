namespace BEL.FeedbackWorkflow.BusinessLayer
{
    using BEL.CommonDataContract;
    using BEL.FeedbackWorkflow.Models.Feedbacks;
    using BEL.FeedbackWorkflow.Models.Common;
    using BEL.DataAccessLayer;
    using Microsoft.SharePoint.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using BEL.FeedbackWorkflow.Models.Master;
    using BEL.FeedbackWorkflow.Models;
    using System.Web.Mvc;

    /// <summary>
    /// Feedbacks Business Layer
    /// </summary>
    public class FeedbacksBusinessLayer
    {
        /// <summary>
        /// Lazy read only object
        /// </summary>
        private static readonly Lazy<FeedbacksBusinessLayer> Lazy =
         new Lazy<FeedbacksBusinessLayer>(() => new FeedbacksBusinessLayer());

        /// <summary>
        /// Instance Object
        /// </summary>
        public static FeedbacksBusinessLayer Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        /// <summary>
        /// The padlock
        /// </summary>
        private static readonly object Padlock = new object();

        /// <summary>
        /// The context
        /// </summary>
        private ClientContext context = null;

        /// <summary>
        /// The web
        /// </summary>
        private Web web = null;

        /// <summary>
        /// Feedbacks Business Layer
        /// </summary>
        private FeedbacksBusinessLayer()
        {
            string siteURL = BELDataAccessLayer.Instance.GetSiteURL(SiteURLs.FEEDBACKSITEURL);
            if (!string.IsNullOrEmpty(siteURL))
            {
                if (this.context == null)
                {
                    this.context = BELDataAccessLayer.Instance.CreateClientContext(siteURL);
                }

                if (this.web == null)
                {
                    this.web = BELDataAccessLayer.Instance.CreateWeb(this.context);
                }
            }
        }

        /// <summary>
        /// cehck user is in Creator group.
        /// </summary>
        /// <param name="userid">User ID</param>
        /// <returns>Yes or No</returns>
        public bool IsCreator(string userid)
        {
            return BELDataAccessLayer.Instance.IsGroupMember(this.context, this.web, userid, UserRoles.CREATOR);
        }

        #region "GET DATA"
        /// <summary>
        /// Gets the Feedbacks details.
        /// </summary>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>byte array</returns>
        public FeedbacksContract GetFeedbacksDetails(IDictionary<string, string> objDict)
        {
            FeedbacksContract contract = new FeedbacksContract();
            if (objDict != null && objDict.ContainsKey(Parameter.FROMNAME) && objDict.ContainsKey(Parameter.ITEMID) && objDict.ContainsKey(Parameter.USEREID))
            {
                string formName = objDict[Parameter.FROMNAME];
                int itemId = Convert.ToInt32(objDict[Parameter.ITEMID]);
                string userID = objDict[Parameter.USEREID];
                string BU = "";

                IForm feedbacksForm = new FeedbacksForm(true);
                List<ApproverMasterListItem> approverList = new List<ApproverMasterListItem>();
                feedbacksForm = BELDataAccessLayer.Instance.GetFormData(this.context, this.web, ApplicationNameConstants.FEEDBACKSAPP, formName, itemId, userID, feedbacksForm);
                var sectionDetails = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.FEEDBACKDETAILSECTION) as FeedbacksDetailSection;

                if (feedbacksForm != null && feedbacksForm.SectionsList != null && feedbacksForm.SectionsList.Count > 0)
                {
                    if (objDict.ContainsKey("BusinessUnit") && objDict["BusinessUnit"] != "")
                    {
                        BU = objDict["BusinessUnit"];
                    }
                    else
                    {
                        BU = sectionDetails.BusinessUnits;
                    }
                    if (sectionDetails != null)
                    {


                        if (itemId == 0)
                        {
                            var spCurrentUser = CommonBusinessLayer.Instance.GetLoginUserDetail(userID);
                            sectionDetails.Branch = spCurrentUser.Location;


                            List<IMasterItem> approvers = sectionDetails.MasterData.FirstOrDefault(p => p.NameOfMaster == FeedbacksMasters.APPROVERMASTER).Items;
                            approverList = approvers.ConvertAll(p => (ApproverMasterListItem)p);

                            sectionDetails.ApproversList.ForEach(p =>
                           {
                               if (p.Role == FeedbackRoles.CCACTINGUSER)
                               {
                                   sectionDetails.CCActingUser = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.CCACTINGUSER).UserID;
                                   sectionDetails.CCActingUserName = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.CCACTINGUSER).UserName;
                               }
                               else if (p.Role == FeedbackRoles.LUMSERVICEMANAGERS)
                               {
                                   sectionDetails.LUMActingUser = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.LUMSERVICEMANAGERS).UserID;
                                   sectionDetails.LUMActingUserName = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.LUMSERVICEMANAGERS).UserName;
                               }
                               else if (p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER)
                               {
                                   p.Approver = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).UserID;
                                   p.ApproverName = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).UserName;
                               }
                               else if (p.Role == FeedbackRoles.LUMQUALITYINCHARGE)
                               {
                                   p.Approver = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.LUMQUALITYINCHARGE).UserID;
                                   p.ApproverName = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.LUMQUALITYINCHARGE).UserName;
                               }

                           });
                        }
                        else
                        {
                            sectionDetails.ApproversList.Remove(sectionDetails.ApproversList.FirstOrDefault(p => p.Role == UserRoles.VIEWER));
                        }
                        sectionDetails.FileNameList = sectionDetails.Files != null && sectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(sectionDetails.Files.Where(x => !string.IsNullOrEmpty(sectionDetails.FileNameList) && sectionDetails.FileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;

                    }
                    contract.Forms.Add(feedbacksForm);
                }



                var ccSectionDetails = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.CCACTINGUSERSECTION) as CCActingUserSection;
                if (ccSectionDetails != null)
                {
                    ccSectionDetails.CCFileNameList = ccSectionDetails.Files != null && ccSectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(ccSectionDetails.Files.Where(x => !string.IsNullOrEmpty(ccSectionDetails.CCFileNameList) && ccSectionDetails.CCFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                    List<IMasterItem> approvers = ccSectionDetails.MasterData.FirstOrDefault(p => p.NameOfMaster == FeedbacksMasters.APPROVERMASTER).Items;
                    approverList = approvers.ConvertAll(p => (ApproverMasterListItem)p);
                    ccSectionDetails.ApproversList.ForEach(p =>
                    {
                        if (BU != "Illumination S" && BU != null)
                        {
                            ccSectionDetails.CCQualityInchargeUser = p.Approver = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).UserID;
                            ccSectionDetails.CCQualityInchargeName = p.ApproverName = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.CCQUALITYINCHARGEUSER).UserName;
                        }
                    });
                }

                var LUMSectionDetails = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.LUMCCACTINGUSERSECTION) as LUMActingUserSection;

                if (LUMSectionDetails != null)
                {
                    LUMSectionDetails.CCFileNameList = LUMSectionDetails.Files != null && LUMSectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(LUMSectionDetails.Files.Where(x => !string.IsNullOrEmpty(LUMSectionDetails.CCFileNameList) && LUMSectionDetails.CCFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                    List<IMasterItem> approvers = LUMSectionDetails.MasterData.FirstOrDefault(p => p.NameOfMaster == FeedbacksMasters.APPROVERMASTER).Items;
                    approverList = approvers.ConvertAll(p => (ApproverMasterListItem)p);

                    LUMSectionDetails.ApproversList.ForEach(p =>
                    {
                        if (BU == "Illumination S")
                        {
                            LUMSectionDetails.LUMQualityInchargeUser = p.Approver = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.LUMQUALITYINCHARGE).UserID;
                            LUMSectionDetails.LUMQualityInchargeName = p.ApproverName = approverList.FirstOrDefault(q => q.Role == FeedbackRoles.LUMQUALITYINCHARGE).UserName;
                        }
                    });

                }

                var ccQASectionDetails = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.CCQUALITYINCHARGESECTION) as CCQualityInchargeSection;
                if (ccQASectionDetails != null)
                {
                    ccQASectionDetails.CCQAInchargeFileNameList = ccQASectionDetails.Files != null && ccQASectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(ccQASectionDetails.Files.Where(x => !string.IsNullOrEmpty(ccQASectionDetails.CCQAInchargeFileNameList) && ccQASectionDetails.CCQAInchargeFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;

                }

                var LUMQASectionDetails = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.LUMQUALITYINCHARGESECTION) as LUMQualityInchargeSection;
                if (LUMQASectionDetails != null)
                {
                    LUMQASectionDetails.CCQAInchargeFileNameList = LUMQASectionDetails.Files != null && LUMQASectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(LUMQASectionDetails.Files.Where(x => !string.IsNullOrEmpty(LUMQASectionDetails.CCQAInchargeFileNameList) && LUMQASectionDetails.CCQAInchargeFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;

                }


                var qaUserSection = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.QAULITYUSERSECTION) as QaulityUserSection;
                if (qaUserSection != null)
                {
                    qaUserSection.QUFileNameList = qaUserSection.Files != null && qaUserSection.Files.Count > 0 ? JsonConvert.SerializeObject(qaUserSection.Files.Where(x => !string.IsNullOrEmpty(qaUserSection.QUFileNameList) && qaUserSection.QUFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                    qaUserSection.QAHeadUser = approverList != null && approverList.FirstOrDefault(q => q.Role == FeedbackRoles.QAHEADUSER) != null ? approverList.FirstOrDefault(q => q.Role == FeedbackRoles.QAHEADUSER).UserID : string.Empty;


                }

                var LUMqaUserSection = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.LUMQAULITYUSERSECTION) as QaulityUserSection;
                if (LUMqaUserSection != null)
                {
                    LUMqaUserSection.QUFileNameList = LUMqaUserSection.Files != null && LUMqaUserSection.Files.Count > 0 ? JsonConvert.SerializeObject(LUMqaUserSection.Files.Where(x => !string.IsNullOrEmpty(LUMqaUserSection.QUFileNameList) && LUMqaUserSection.QUFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                    LUMqaUserSection.QAHeadUser = approverList != null && approverList.FirstOrDefault(q => q.Role == FeedbackRoles.QAHEADUSER) != null ? approverList.FirstOrDefault(q => q.Role == FeedbackRoles.QAHEADUSER).UserID : string.Empty;


                }

                //if (ccSectionDetails != null) { 
                //    string[] ID = ccSectionDetails.CCQualityInchargeUser.Split(',');

                //    contract.dynmicfield = new Dictionary<string, string>();
                //    for (int i = 0; i < ID.Length; i++)
                //    {

                //        if (ID[i] == userID)
                //        {
                //            contract.dynmicfield.Add("Visibility", "visible");
                //        }

                //    }
                //}


            }

            return contract;

        }

        #endregion

        #region "SAVE DATA"
        /// <summary>
        /// Saves the by section.
        /// </summary>
        /// <param name="sectionDetails">The sections.</param>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>return status</returns>
        public ActionStatus SaveBySection(ISection sectionDetails, Dictionary<string, string> objDict)
        {
            lock (Padlock)
            {
                ActionStatus status = new ActionStatus();
                FeedbackNoCount currentFeedbacksNo = null;
                if (sectionDetails != null && objDict != null)
                {
                    objDict[Parameter.ACTIVITYLOG] = FeedbackListNames.FEEDBACKACTIVITYLOG;
                    objDict[Parameter.APPLICATIONNAME] = ApplicationNameConstants.FEEDBACKSAPP;
                    objDict[Parameter.FROMNAME] = FormNameConstants.FeedbacksFORM;

                    FeedbacksDetailSection section = null;
                    if (sectionDetails.SectionName == FeedbackSectionName.FEEDBACKDETAILSECTION)
                    {
                        section = sectionDetails as FeedbacksDetailSection;
                        if (sectionDetails.ActionStatus == ButtonActionStatus.NextApproval && string.IsNullOrEmpty(section.FeedbackNo))
                        {
                            section.RequestDate = DateTime.Now;
                            currentFeedbacksNo = this.GetFeedbacksNo(section.BusinessUnits);
                            if (currentFeedbacksNo != null)
                            {
                                currentFeedbacksNo.CurrentValue = currentFeedbacksNo.CurrentValue + 1;
                                Logger.Info("Feedbacks Current Value + 1 = " + currentFeedbacksNo.CurrentValue);
                                //// section.ProjectNo = string.Format("Feedbacks-{0}-{1}-{2}", currentFeedbacksNo.BusinessUnit, DateTime.Today.Year, string.Format("{0:0000}", currentFeedbacksNo.CurrentValue));
                                section.FeedbackNo = string.Format("{0}-{1}-{2}", section.BusinessUnits, section.RequestDate.Value.ToString("yyyy"), string.Format("{0:00000}", currentFeedbacksNo.CurrentValue));
                                Logger.Info("Feedbacks No is " + section.FeedbackNo);
                                status.ExtraData = section.FeedbackNo;
                            }
                            else
                            {

                            }


                        }

                        if (section.ButtonCaption == " Forward to Quality User")
                        {
                            objDict[Parameter.SENDTOLEVEL] = "3";
                            section.ActionStatus = ButtonActionStatus.SendForward;
                        }
                        else if (section.ButtonCaption == "SendForward")
                        {
                            objDict[Parameter.SENDTOLEVEL] = "2";
                            section.ActionStatus = ButtonActionStatus.SendForward;
                        }


                    }

                    QaulityUserSection sectionQA = null;
                    if (sectionDetails.SectionName == FeedbackSectionName.QAULITYUSERSECTION)
                    {
                        sectionQA = sectionDetails as QaulityUserSection;
                        if (sectionDetails.ActionStatus == ButtonActionStatus.SendBack)
                        {
                            if (sectionQA.ButtonCaption == "Send Back")
                            {
                                objDict[Parameter.SENDTOLEVEL] = "0";
                            }
                        }
                    }
                    LUMQualityUserSection sectionLUMQA = null;
                    if (sectionDetails.SectionName == FeedbackSectionName.LUMQAULITYUSERSECTION)
                    {
                        sectionLUMQA = sectionDetails as LUMQualityUserSection;
                        if (sectionDetails.ActionStatus == ButtonActionStatus.SendBack)
                        {
                            if (sectionLUMQA.ButtonCaption == "Send Back")
                            {
                                objDict[Parameter.SENDTOLEVEL] = "0";
                            }
                        }
                    }
                    List<ListItemDetail> objSaveDetails = BELDataAccessLayer.Instance.SaveData(this.context, this.web, sectionDetails, objDict);
                    ListItemDetail itemDetails = objSaveDetails.Where(p => p.ListName.Equals(FeedbackListNames.FEEDBACKLIST)).FirstOrDefault<ListItemDetail>();
                    if (sectionDetails.SectionName == FeedbackSectionName.FEEDBACKDETAILSECTION)
                    {
                        ////if (!string.IsNullOrEmpty(section.OldProjectNo))
                        ////{
                        ////    Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();
                        ////    values.Add("IsFeedbacksRetrieved", true);
                        ////    BELDataAccessLayer.Instance.SaveFormFields(this.context, this.web, FeedbacksListNames.FeedbacksLIST, section.OldFeedbacksId, values);
                        ////}

                        if (itemDetails.ItemId > 0 && currentFeedbacksNo != null)
                        {
                            this.UpdateFeedbacksNoCount(currentFeedbacksNo);
                            Logger.Info("Update DCR No " + section.FeedbackNo);
                        }

                    }

                    if (itemDetails.ItemId > 0)
                    {
                        status.IsSucceed = true;
                        status.ItemID = itemDetails.ItemId;
                        switch (sectionDetails.ActionStatus)
                        {
                            case ButtonActionStatus.SaveAsDraft:
                            case ButtonActionStatus.SaveAndNoStatusUpdateWithEmail:
                                status.Messages.Add("Text_SaveDraftSuccess");
                                break;
                            case ButtonActionStatus.NextApproval:
                                status.Messages.Add(ApplicationConstants.SUCCESSMESSAGE);
                                break;
                            case ButtonActionStatus.Complete:
                                status.Messages.Add("Text_CompletedSuccess");
                                break;
                            default:
                                status.Messages.Add(ApplicationConstants.SUCCESSMESSAGE);
                                break;
                        }
                    }
                    else
                    {
                        status.IsSucceed = false;
                        status.Messages.Add(ApplicationConstants.ERRORMESSAGE);
                    }
                }
                return status;
            }
        }

        /// <summary>
        /// Get Feedbacks No Logic
        /// </summary>
        /// <returns>Feedbacks No</returns>
        public string GetFeedbacksNo()
        {
            try
            {
                int dcrno = 0;
                List spList = this.web.Lists.GetByTitle(FeedbackListNames.FEEDBACKLIST);
                CamlQuery query = new CamlQuery();
                string startDate = (new DateTime(DateTime.Now.Year, 1, 1)).ToString("yyyy-MM-ddTHH:mm:ssZ");
                string endDate = (new DateTime(DateTime.Now.Year, 12, 31)).ToString("yyyy-MM-ddTHH:mm:ssZ");
                query.ViewXml = @"<View><Query>
                                      <Where>
                                        <And>
                                          <Geq>
                                            <FieldRef Name='Created' />
                                              <Value IncludeTimeValue='FALSE' Type='DateTime'>" + startDate + @"</Value>
                                          </Geq>
                                          <Leq>
                                            <FieldRef Name='Created' />
                                            <Value IncludeTimeValue='FALSE' Type='DateTime'>" + endDate + @"</Value>
                                          </Leq>
                                        </And>
                                      </Where>
                                    </Query>
                                         <ViewFields><FieldRef Name='ID' /></ViewFields>   </View>";
                ////query.ViewXml = @"<View><ViewFields><FieldRef Name='ID' /></ViewFields></View>";
                ListItemCollection items = spList.GetItems(query);
                this.context.Load(items);
                this.context.ExecuteQuery();
                if (items != null && items.Count != 0)
                {
                    dcrno = items.Count;
                }
                return dcrno.ToString();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Feedbacks No Logic
        /// </summary>
        /// <param name="businessunit">Business Unit</param>
        /// <returns>Feedbacks No Count</returns>
        public FeedbackNoCount GetFeedbacksNo(string businessunit)
        {
            try
            {
                List<FeedbackNoCount> lstFeedbacksCount = new List<FeedbackNoCount>();
                List spList = this.web.Lists.GetByTitle(FeedbackListNames.FEEDBACKNOCOUNT);
                CamlQuery query = new CamlQuery();
                query.ViewXml = @"<View><ViewFields><FieldRef Name='Title' /><FieldRef Name='Year' /><FieldRef Name='CurrentValue' /></ViewFields>   </View>";
                ListItemCollection items = spList.GetItems(query);
                this.context.Load(items);
                this.context.ExecuteQuery();
                if (items != null && items.Count != 0)
                {
                    foreach (ListItem item in items)
                    {
                        FeedbackNoCount obj = new FeedbackNoCount();
                        obj.ID = item.Id;
                        obj.BusinessUnit = Convert.ToString(item["Title"]);
                        obj.Year = Convert.ToInt32(item["Year"]);
                        obj.CurrentValue = Convert.ToInt32(item["CurrentValue"]);
                        if (obj.Year != DateTime.Today.Year)
                        {
                            obj.Year = DateTime.Today.Year;
                            obj.CurrentValue = 0;
                        }

                        lstFeedbacksCount.Add(obj);
                    }
                }

                if (lstFeedbacksCount != null)
                {
                    return lstFeedbacksCount.FirstOrDefault(p => businessunit.Contains(p.BusinessUnit) && p.Year == DateTime.Today.Year);
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Update Feedbacks No Count
        /// </summary>
        /// <param name="currentValue">Current Value</param>
        public void UpdateFeedbacksNoCount(FeedbackNoCount currentValue)
        {
            if (currentValue != null && currentValue.ID != 0)
            {
                try
                {
                    Logger.Info("Aync update Feedbacks Current value currentValue : " + currentValue.CurrentValue + " BusinessUnit : " + currentValue.BusinessUnit);
                    List spList = this.web.Lists.GetByTitle(FeedbackListNames.FEEDBACKNOCOUNT);
                    ListItem item = spList.GetItemById(currentValue.ID);
                    item["CurrentValue"] = currentValue.CurrentValue;
                    item["Year"] = currentValue.Year;
                    item.Update();
                    ////context.Load(item);
                    this.context.ExecuteQuery();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while Update Feedbacks no Current Value");
                    Logger.Error(ex);
                }
            }
        }
        #endregion

        #region "GET Admin DATA"
        /// <summary>
        /// Gets the Feedbacks details.
        /// </summary>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>byte array</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "test")]
        public FeedbacksAdminContract GetFeedbacksAdminDetails(IDictionary<string, string> objDict)
        {
            FeedbacksAdminContract contract = new FeedbacksAdminContract();
            if (objDict != null && objDict.ContainsKey(Parameter.FROMNAME) && objDict.ContainsKey(Parameter.ITEMID) && objDict.ContainsKey(Parameter.USEREID))
            {
                string formName = objDict[Parameter.FROMNAME];
                int itemId = Convert.ToInt32(objDict[Parameter.ITEMID]);
                string userID = objDict[Parameter.USEREID];
                IForm feedbacksForm = new FeedbacksAdminForm(true);
                feedbacksForm = BELDataAccessLayer.Instance.GetFormData(this.context, this.web, ApplicationNameConstants.FEEDBACKSAPP, formName, itemId, userID, feedbacksForm);
                if (feedbacksForm != null && feedbacksForm.SectionsList != null && feedbacksForm.SectionsList.Count > 0)
                {
                    var sectionDetails = feedbacksForm.SectionsList.FirstOrDefault(f => f.SectionName == FeedbackSectionName.FEEDBACKDETAILADMINSECTION) as FeedbacksAdminDetailSection;
                    if (sectionDetails != null)
                    {
                        if (itemId > 0)
                        {
                            sectionDetails.ApproversList.Remove(sectionDetails.ApproversList.FirstOrDefault(p => p.Role == UserRoles.VIEWER));
                        }
                        ////if (string.IsNullOrEmpty(sectionDetails.CCFileNameList))
                        ////{
                        ////    sectionDetails.FileNameList = sectionDetails.Files != null && sectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(sectionDetails.Files) : string.Empty;
                        ////}
                        ////else
                        ////{
                        ////    sectionDetails.FileNameList = sectionDetails.Files != null && sectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(sectionDetails.Files.Where(x => !string.IsNullOrEmpty(sectionDetails.CCFileNameList) && !sectionDetails.CCFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                        ////}
                        sectionDetails.FileNameList = sectionDetails.Files != null && sectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(sectionDetails.Files.Where(x => !string.IsNullOrEmpty(sectionDetails.FileNameList) && sectionDetails.FileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                        sectionDetails.CCFileNameList = sectionDetails.Files != null && sectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(sectionDetails.Files.Where(x => !string.IsNullOrEmpty(sectionDetails.CCFileNameList) && sectionDetails.CCFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                        sectionDetails.CCQAInchargeFileNameList = sectionDetails.Files != null && sectionDetails.Files.Count > 0 ? JsonConvert.SerializeObject(sectionDetails.Files.Where(x => !string.IsNullOrEmpty(sectionDetails.CCQAInchargeFileNameList) && sectionDetails.CCQAInchargeFileNameList.Split(',').Contains(x.FileName)).ToList()) : string.Empty;
                    }
                    contract.Forms.Add(feedbacksForm);
                }
            }
            return contract;
        }
        #endregion

        #region "SAVE Admin DATA"
        /// <summary>
        /// Saves the by section.
        /// </summary>
        /// <param name="sectionDetails">The sections.</param>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>return status</returns>
        public ActionStatus SaveAdminDataBySection(ISection sectionDetails, Dictionary<string, string> objDict)
        {
            lock (Padlock)
            {
                ActionStatus status = new ActionStatus();
                if (sectionDetails != null && objDict != null)
                {
                    objDict[Parameter.ACTIVITYLOG] = FeedbackListNames.FEEDBACKACTIVITYLOG;
                    objDict[Parameter.APPLICATIONNAME] = ApplicationNameConstants.FEEDBACKSAPP;
                    objDict[Parameter.FROMNAME] = FormNameConstants.FeedbacksADMINFORM;
                    List<ListItemDetail> objSaveDetails = BELDataAccessLayer.Instance.SaveData(this.context, this.web, sectionDetails, objDict);
                    ListItemDetail itemDetails = objSaveDetails.Where(p => p.ListName.Equals(FeedbackListNames.FEEDBACKLIST)).FirstOrDefault<ListItemDetail>();

                    if (itemDetails.ItemId > 0)
                    {
                        status.IsSucceed = true;
                        switch (sectionDetails.ActionStatus)
                        {
                            case ButtonActionStatus.SaveAsDraft:
                                status.Messages.Add("Text_SaveSuccess");
                                break;
                            case ButtonActionStatus.SaveAndNoStatusUpdate:
                                status.Messages.Add("Text_SaveSuccess");
                                break;
                            case ButtonActionStatus.NextApproval:
                                status.Messages.Add(ApplicationConstants.SUCCESSMESSAGE);
                                break;
                            case ButtonActionStatus.Complete:
                                status.Messages.Add("Text_CompleteSuccess");
                                break;
                            case ButtonActionStatus.Rejected:
                                status.Messages.Add("Text_RejectedSuccess");
                                break;
                            default:
                                status.Messages.Add(ApplicationConstants.SUCCESSMESSAGE);
                                break;
                        }
                    }
                    else
                    {
                        status.IsSucceed = false;
                        status.Messages.Add(ApplicationConstants.ERRORMESSAGE);
                    }
                }
                return status;
            }
        }
        #endregion

        #region GetSKUMaster



        /// <summary>
        /// Get All Items Code
        /// </summary>
        /// <param name="title">search term</param>
        /// <returns>string data</returns>
        public string GetAllItemsCode(string title)
        {
            IMaster objIMaster = GlobalCachingProvider.Instance.GetItem(FeedbacksMasters.SKUMASTER, false) as IMaster;
            ItemMaster itemMaster = new ItemMaster();
            itemMaster.Items = new List<IMasterItem>();
            List<ListItem> itemss = new List<ListItem>();
            if (objIMaster == null)
            {
                List spList = this.web.Lists.GetByTitle(FeedbacksMasters.SKUMASTER);
                ListItemCollectionPosition position = null;
                CamlQuery qry = new CamlQuery();
                qry.ViewXml = @"<View><Query><OrderBy><FieldRef Name='ID' Ascending='False' /></OrderBy></Query><RowLimit>5000</RowLimit></View>";
                ListItemCollection items = null;

                do
                {
                    qry.ListItemCollectionPosition = position;
                    items = spList.GetItems(qry);
                    this.context.Load(items);
                    this.context.ExecuteQuery();

                    position = items.ListItemCollectionPosition;
                    itemss.AddRange(items.ToList());
                }
                while (position != null);

                if (itemss != null && itemss.Count > 0)
                {
                    ItemMasterListItem obj;
                    foreach (ListItem item in itemss)
                    {
                        obj = new ItemMasterListItem();
                        obj.Title = Convert.ToString(item["ItemCode"]);
                        obj.ItemDescription = Convert.ToString(item["ItemDescription"]);
                        obj.BusinessUnits = Convert.ToString(item["BusinessUnit"]);
                        obj.ProductGroup= Convert.ToString(item["ProductGroup"]);
                        itemMaster.Items.Add(obj);
                    }
                    objIMaster = itemMaster;
                }
                GlobalCachingProvider.Instance.AddItem(FeedbacksMasters.SKUMASTER, objIMaster);
            }
            string itemjson = JSONHelper.ToJSON<IMaster>(objIMaster);
            var filterdVendor = JSONHelper.ToObject<IMaster>(itemjson);
            if (filterdVendor != null)
            {
                // filterdVendor.Items = filterdVendor.Items.Where(x => ((x as ItemMasterListItem).ItemCode ?? string.Empty).ToLower().Trim().Contains((title ?? string.Empty).ToLower().Trim()) || ((x as ItemMasterListItem).ModelName ?? string.Empty).ToLower().Trim().Contains((title ?? string.Empty).ToLower().Trim())).ToList();
                filterdVendor.Items = filterdVendor.Items.Where(x => ((x as ItemMasterListItem).Title ?? string.Empty).ToLower().Trim().Contains((title ?? string.Empty).ToLower().Trim())).Take(10).ToList();

                return JSONHelper.ToJSON(filterdVendor);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Artwork Info By Item Code
        /// </summary>
        /// <param name="itemCode">item code</param>
        /// <returns>Artwork Detail</returns>
        public string GetSKUInfoByItemCode(string itemCode)
        {
            MasterDataHelper masterHelper = new MasterDataHelper();
            IMaster itemmaster = masterHelper.GetMasterData(this.context, this.web, new List<IMaster>() { new ItemMaster() }).FirstOrDefault();
            string itemjson = JSONHelper.ToJSON<IMaster>(itemmaster);
            var filterdVendor = JSONHelper.ToObject<IMaster>(itemjson);
            if (filterdVendor != null)
            {
                var item = filterdVendor.Items.Where(x => ((x as ItemMasterListItem).Title ?? string.Empty).ToLower().Trim().Equals((itemCode ?? string.Empty).ToLower().Trim())).FirstOrDefault();
                return JSONHelper.ToJSON<IMasterItem>(item);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region "Report"

        /// <summary>
        /// Retrieves the feedback.
        /// </summary>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>return sections</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "test")]
        public List<ISection> RetrieveFeedback(IDictionary<string, string> objDict)
        {
            Dictionary<string, string> otherParamDictionary = null;
            BELDataAccessLayer helper = new BELDataAccessLayer();
            List<ISection> sections = new List<ISection>();
            if (objDict != null && objDict.ContainsKey("UserEmail"))
            {
                string userEmail = objDict["UserEmail"];
                string formName = string.Empty;
                FeedbackReportSection section = new FeedbackReportSection(true);
                sections = helper.GetItemsForSection(this.context, this.web, ApplicationNameConstants.FEEDBACKSAPP, formName, (ISection)section, userEmail, otherParamDictionary);
            }
            return sections;
        }
        #endregion
    }
}