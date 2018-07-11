namespace BEL.FeedbackWorkflow.Controllers
{
    using BEL.CommonDataContract;
    using BEL.FeedbackWorkflow.BusinessLayer;
    using BEL.FeedbackWorkflow.Models.Feedbacks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Feedbacks Base Controller
    /// </summary>
    public class FeedbacksBaseController : BaseController
    {
        /// <summary>
        /// Get Feedbacks Details
        /// </summary>
        /// <param name="objDict">dictionary parameter</param>
        /// <returns>contract object</returns>
        public FeedbacksContract GetFeedbacksDetails(IDictionary<string, string> objDict)
        {
            FeedbacksContract contract = FeedbacksBusinessLayer.Instance.GetFeedbacksDetails(objDict);
            return contract;
        }

        /// <summary>
        /// Saves the section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="objDict">The object dictionary.</param>
        /// <returns>return status</returns>
        protected ActionStatus SaveSection(ISection section, Dictionary<string, string> objDict)
        {
            ActionStatus status = new ActionStatus();
            status = FeedbacksBusinessLayer.Instance.SaveBySection(section, objDict);
            return status;
        }

        /// <summary>
        /// Get Feedbacks Admin Form Details
        /// </summary>
        /// <param name="objDict">parameters value</param>
        /// <returns>contract object</returns>
        public FeedbacksAdminContract GetFeedbacksAdminDetails(IDictionary<string, string> objDict)
        {
            FeedbacksAdminContract contract = FeedbacksBusinessLayer.Instance.GetFeedbacksAdminDetails(objDict);
            return contract;
        }

        /// <summary>
        /// Save Feedback Admin Detail Section
        /// </summary>
        /// <param name="section">Section Data</param>
        /// <param name="objDict">parameters value</param>
        /// <returns>Action Status</returns>
        protected ActionStatus SaveFeedbackAdminDetailSection(ISection section, Dictionary<string, string> objDict)
        {
            ActionStatus status = new ActionStatus();
            status = FeedbacksBusinessLayer.Instance.SaveAdminDataBySection(section, objDict);
            return status;
        }

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="q">The q.</param>
        /// <returns>json string</returns>
        protected string GetAllItemsCode(string q)
        {
            string data = FeedbacksBusinessLayer.Instance.GetAllItemsCode(q);
            return data;
        }

        /// <summary>
        /// Gets Artwork Info
        /// </summary>
        /// <param name="itemCode">The item code.</param>
        /// <returns>json string</returns>
        protected string GetSKUInfoByItemCode(string itemCode)
        {
            string data = FeedbacksBusinessLayer.Instance.GetSKUInfoByItemCode(itemCode);
            return data;
        }
    }
}