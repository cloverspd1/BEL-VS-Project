namespace BEL.FeedbackWorkflow.Models.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Form Names
    /// </summary>
    public static class FeedbacksFormNames
    {
        /// <summary>
        /// The Feedbackform
        /// </summary>
        public const string FEEDBACKFORM = "Feedback Form";

        /// <summary>
        /// The Feedback Admin form
        /// </summary>
        public const string FEEDBACKADMINFORM = "Feedback Admin Form";
    }

    /// <summary>
    /// List Names
    /// </summary>
    public static class FeedbackListNames
    {
        /// <summary>
        /// The Feedback list name
        /// </summary>
        public const string FEEDBACKLIST = "Feedbacks";

        /// <summary>
        /// The Feedback activity log
        /// </summary>
        public const string FEEDBACKACTIVITYLOG = "FeedbacksActivityLog";

        /// <summary>
        /// The Feedback appapproval matrix
        /// </summary>
        public const string FEEDBACKAPPAPPROVALMATRIX = "FeedbacksApprovalMatrix";

        /// <summary>
        ///  FEEDBACK PRODUCT SERIAL
        /// </summary>
        public const string FEEDBACKPRODUCTSERIAL = "ProductSerial";

        /// <summary>
        /// Feedback Number Count List Name
        /// </summary>
        public const string FEEDBACKNOCOUNT = "FeedbackNoCount";
    }

    /// <summary>
    /// Master Names
    /// </summary>
    public static class FeedbacksMasters
    {
        /// <summary>
        /// the Approver master
        /// </summary>
        public const string APPROVERMASTER = "ApproverMaster";

        /// <summary>
        /// the business unit master
        /// </summary>
        public const string BUSINESSUNITMASTER = "BusinessUnitMaster";

        /// <summary>
        /// the employee master
        /// </summary>
        public const string EMPLOYEEMASTER = "EmployeeMaster";

        /// <summary>
        /// BRANCH MASTER
        /// </summary>
        public const string BRANCHMASTER = "BranchMaster";

        /// <summary>
        /// Problem Cause Master
        /// </summary>
        public const string PROBLEMCAUSEMASTER = "ProblemCauseMaster";

        /// <summary>
        /// Product Type Master
        /// </summary>
        public const string PRODUCTTYPEMASTER = "ProductTypeMaster";

        /// <summary>
        /// Item MASTER
        /// </summary>
        public const string SKUMASTER = "SKUMaster";
    }

    /// <summary>
    /// Feedback Section Names
    /// </summary>
    public static class FeedbackSectionName
    {
        /// <summary>
        /// The Feedback details section
        /// </summary>
        public const string FEEDBACKDETAILSECTION = "Feedback Detail Section";

        /// <summary>
        /// The Feedback approver1 section
        /// </summary>
        public const string CCACTINGUSERSECTION = "CC Acting User Section";

        /// <summary>
        /// The Feedback approver2 section
        /// </summary>
        public const string CCQUALITYINCHARGESECTION = "CC Quality Incharge Section";

        /// <summary>
        /// The Feedback approver3 section
        /// </summary>
        public const string QAULITYUSERSECTION = "Qaulity User Section";

        /// <summary>
        /// The Feedback approver1 section
        /// </summary>
        public const string FEEDBACKDETAILADMINSECTION = "Feedback Admin Detail Section";

        /// <summary>
        /// The activitylog
        /// </summary>
        public const string ACTIVITYLOG = "Activity Log";

        /// <summary>
        /// The activitylog
        /// </summary>
        public const string APPLICATIONSTATUSSECTION = "Application Status Section";
    }

    /// <summary>
    /// Feedback Roles
    /// </summary>
    public static class FeedbackRoles
    {
        /// <summary>
        /// The creator
        /// </summary>
        public const string CREATOR = "Creator";

        /// <summary>
        /// The viewer
        /// </summary>
        public const string VIEWER = "Viewer";

        /// <summary>
        /// The editor
        /// </summary>
        public const string EDITOR = "Editor";

        /// CC Acting User
        /// </summary>
        public const string CCACTINGUSER = "CC Acting User";

        /// CC Acting User
        /// </summary>
        public const string CCQUALITYINCHARGEUSER = "CC Quality Incharge User";

        /// Quality User
        /// </summary>
        public const string QAULITYUSER = "Quality User";

        /// <summary>
        /// QA Head
        /// </summary>
        public const string QAHEADUSER = "QA Head";

        /// <summary>
        /// Admin Role
        /// </summary>
        public const string ADMIN = "Admin";
    }
}