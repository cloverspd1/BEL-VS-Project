namespace BEL.FeedbackWorkflow.BusinessLayer
{
    using BEL.FeedbackWorkflow.Models.Common;
    using BEL.FeedbackWorkflow.Models.Master;
    using CommonDataContract;
    using DataAccessLayer;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.UserProfiles;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web;

    /// <summary>
    /// Common Business Layer
    /// </summary>
    public sealed class CommonBusinessLayer
    {
        /// <summary>
        ///  lazy Instance
        /// </summary>
        private static readonly Lazy<CommonBusinessLayer> Lazy = new Lazy<CommonBusinessLayer>(() => new CommonBusinessLayer());

        /// <summary>
        /// Comman Instance
        /// </summary>
        public static CommonBusinessLayer Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        /// <summary>
        /// The context
        /// </summary>
        private ClientContext context = null;

        /// <summary>
        /// The web
        /// </summary>
        private Web web = null;

        /// <summary>
        /// Common Business Layer
        /// </summary>
        private CommonBusinessLayer()
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
        /// Download File
        /// </summary>
        /// <param name="url">URL string</param>
        /// <param name="applicationName">application Name</param>
        /// <returns>Byte Array</returns>
        public byte[] DownloadFile(string url, string applicationName)
        {
            BELDataAccessLayer helper = new BELDataAccessLayer();
            ////string siteURL = helper.GetSiteURL(applicationName);
            ////context = helper.CreateClientContext(siteURL);
            return helper.GetFileBytesByUrl(this.context, url);
        }

        /// <summary>
        /// Validates the users.
        /// </summary>
        /// <param name="emailList">The email list.</param>
        /// <returns>list of invalid users</returns>
        public List<string> ValidateUsers(List<string> emailList)
        {
            BELDataAccessLayer helper = new BELDataAccessLayer();
            return helper.GetInvalidUsers(emailList);
        }

        /// <summary>
        /// Removes the cache keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void RemoveCacheKeys(List<string> keys)
        {
            if (keys != null && keys.Count != 0)
            {
                foreach (string key in keys)
                {
                    GlobalCachingProvider.Instance.RemoveItem(key);
                }
            }
        }

        /// <summary>
        /// Gets the cache keys.
        /// </summary>
        /// <returns>list of string</returns>
        public List<string> GetCacheKeys()
        {
            return GlobalCachingProvider.Instance.GetAllKeys();
        }

        /// <summary>
        /// Gets the file name list.
        /// </summary>
        /// <param name="sectionDetails">The section details.</param>
        /// <param name="type">The type.</param>
        /// <returns>ISection Detail</returns>
        public ISection GetFileNameList(ISection sectionDetails, Type type)
        {
            if (sectionDetails == null)
            {
                return null;
            }

            dynamic dysectionDetails = Convert.ChangeType(sectionDetails, type);
            dysectionDetails.FileNameList = string.Empty;

            if (dysectionDetails.Files != null && dysectionDetails.Files.Count > 0)
            {
                dysectionDetails.FileNameList = JsonConvert.SerializeObject(dysectionDetails.Files);
            }
            return dysectionDetails;
        }

        /// <summary>
        /// Gets the file name list from current approver.
        /// </summary>
        /// <param name="sectionDetails">The section details.</param>
        /// <param name="type">The type.</param>
        /// <returns>I Section</returns>
        public ISection GetFileNameListFromCurrentApprover(ISection sectionDetails, Type type)
        {
            if (sectionDetails == null)
            {
                return null;
            }
            dynamic dysectionDetails = Convert.ChangeType(sectionDetails, type);
            dysectionDetails.FileNameList = string.Empty;

            if (dysectionDetails.CurrentApprover != null && dysectionDetails.CurrentApprover.Files != null && dysectionDetails.CurrentApprover.Files.Count > 0)
            {
                dysectionDetails.FileNameList = JsonConvert.SerializeObject(dysectionDetails.CurrentApprover.Files);
            }
            return dysectionDetails;
        }

        /// <summary>
        /// Get Login User Detail
        /// </summary>
        /// <param name="id">id value</param>
        /// <returns>user detail</returns>
        public UserDetails GetLoginUserDetail(string id)
        {
            MasterDataHelper masterHelper = new MasterDataHelper();
            List<UserDetails> userInfoList = masterHelper.GetAllEmployee(this.context, this.web);
            UserDetails detail = userInfoList.FirstOrDefault(p => p.UserId == id);
            return detail;
        }

        /// <summary>
        /// Gets all employee.
        /// </summary>
        /// <returns>List of users</returns>
        public List<UserDetails> GetAllEmployee()
        {
            List<UserDetails> userInfoList = new List<UserDetails>();

            var siteUsers = from user in this.web.SiteUsers
                            where user.PrincipalType == Microsoft.SharePoint.Client.Utilities.PrincipalType.User
                            select user;
            var usersResult = this.context.LoadQuery(siteUsers);
            this.context.ExecuteQuery();

            var peopleManager = new PeopleManager(this.context);
            var userProfilesResult = new List<PersonProperties>();

            foreach (var user in usersResult)
            {
                var userProfile = peopleManager.GetPropertiesFor(user.LoginName);
                this.context.Load(userProfile);
                userProfilesResult.Add(userProfile);
            }
            this.context.ExecuteQuery();

            if (userProfilesResult != null && userProfilesResult.Count != 0)
            {
                var result = from userProfile in userProfilesResult
                             where userProfile.ServerObjectIsNull != null && userProfile.ServerObjectIsNull.Value != true
                             select new UserDetails()
                             {
                                 UserId = usersResult.Where(p => p.LoginName == userProfile.AccountName).FirstOrDefault() != null ? usersResult.Where(p => p.LoginName == userProfile.AccountName).FirstOrDefault().Id.ToString() : string.Empty,
                                 FullName = userProfile.Title,
                                 UserEmail = userProfile.Email,
                                 LoginName = userProfile.AccountName,
                                 Department = userProfile.UserProfileProperties.ContainsKey("Department") ? Convert.ToString(userProfile.UserProfileProperties["Department"]) : string.Empty,
                                 ReportingManager = userProfile.UserProfileProperties.ContainsKey("Manager") ? Convert.ToString(userProfile.UserProfileProperties["Manager"]) : string.Empty
                             };

                userInfoList = result.ToList();
                GlobalCachingProvider.Instance.AddItem(this.web.ServerRelativeUrl + "/" + BEL.CommonDataContract.ListNames.EmployeeMasterList, userInfoList);
            }
            return userInfoList;
        }

        /// <summary>
        /// Get Current User
        /// </summary>
        /// <param name="userid">User ID</param>
        /// <returns>User Object</returns>
        public User GetCurrentUser(string userid)
        {
            return BELDataAccessLayer.EnsureUser(this.context, this.web, userid);
        }

        /// <summary>
        /// cehck user is in Admin group.
        /// </summary>
        /// <param name="userid">User ID</param>
        /// <returns>Yes or No</returns>
        public bool IsAdminUser(string userid)
        {
            return BELDataAccessLayer.Instance.IsGroupMember(this.context, this.web, userid, UserRoles.ADMIN);
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

        /// <summary>
        /// Update SKU Master Data
        /// </summary>
        /// <param name="dt">Sheet Data</param>
        /// <returns>Yes or no</returns>
        public Dictionary<string, string> UpdateBulkData(DataTable dt)
        {
            Dictionary<string, string> returnPara = new Dictionary<string, string>();
            int totalRecords = dt.Rows.Count;
            int creatednew = 0;
            int error = 0;
            string errorids = string.Empty;
            if (dt == null && dt.Rows.Count == 0)
            {
                return returnPara;
            }
            MasterDataHelper masterhelper = new MasterDataHelper();

            List<IMaster> masters = masterhelper.GetMasterData(this.context, this.web, new List<IMaster>() { new ItemMaster(), new BusinessUnitMaster() });
            List<ItemMasterListItem> skuitems = masters.FirstOrDefault(p => p.NameOfMaster == FeedbacksMasters.SKUMASTER).Items.ConvertAll(p => (ItemMasterListItem)p).ToList();
            List<BusinessUnitMasterListItem> buitems = masters.FirstOrDefault(p => p.NameOfMaster == FeedbacksMasters.BUSINESSUNITMASTER).Items.ConvertAll(p => (BusinessUnitMasterListItem)p).ToList();

            List spList = this.web.Lists.GetByTitle(FeedbacksMasters.SKUMASTER);
            ListItem item = null;
            this.context.RequestTimeout = int.MaxValue;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["ItemCode"])))
                    {
                        ItemMasterListItem obj = skuitems.FirstOrDefault(p => p.Title.Trim() == Convert.ToString(dr["ItemCode"]).Trim());

                        if (obj != null && obj.ID != 0)
                        {
                            // item = spList.GetItemById(obj.ID);
                        }
                        else
                        {
                            if (buitems.Any(p => p.Value.Trim().ToUpper() == Convert.ToString(dr["BusinessUnit"]).Trim().ToUpper()))
                            {
                                creatednew++;                                
                                item = spList.AddItem(new ListItemCreationInformation());
                                item["ItemCode"] = Convert.ToString(dr["ItemCode"]);
                                item["Title"] = "View/Edit";
                                item["ItemDescription"] = Convert.ToString(dr["ItemDescription"]);
                                item["BusinessUnit"] = Convert.ToString(dr["BusinessUnit"]).ToUpper();
                                item["ProductGroup"] = Convert.ToString(dr["ProductGroup"]);
                                item.Update();
                                
                                this.context.ExecuteQuery();
                            }
                            else
                            {
                                error++;
                                errorids += "," + Convert.ToString(dr["ItemCode"]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    creatednew--;
                    error++;
                    errorids += "," + Convert.ToString(dr["ItemCode"]);
                    Logger.Error(ex);
                }
            }
            AsyncHelper.Call(obj =>
            {
                GlobalCachingProvider.Instance.RemoveItem(FeedbacksMasters.SKUMASTER);
                masterhelper.GetMasterData(this.context, this.web, new List<IMaster>() { new ItemMaster() });
            });
            returnPara["TotalRecords"] = totalRecords.ToString();
            returnPara["Added"] = creatednew.ToString();
            returnPara["Error"] = error.ToString();
            returnPara["ErrorIDs"] = errorids.ToString().Trim(',');
            return returnPara;
        }
    }
}