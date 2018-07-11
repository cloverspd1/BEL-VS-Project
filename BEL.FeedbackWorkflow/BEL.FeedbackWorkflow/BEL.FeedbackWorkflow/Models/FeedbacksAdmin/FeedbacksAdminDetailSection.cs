namespace BEL.FeedbackWorkflow.Models.Feedbacks
{
    using BEL.CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;
    using BEL.FeedbackWorkflow.Models.Common;
    using System.ComponentModel.DataAnnotations;
    using BEL.FeedbackWorkflow.Models.Master;

    /// <summary>
    /// Feedbacks Detail Section
    /// </summary>
    [DataContract, Serializable]
    public class FeedbacksAdminDetailSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbacksAdminDetailSection"/> class.
        /// </summary>
        public FeedbacksAdminDetailSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbacksAdminDetailSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public FeedbacksAdminDetailSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(FeedbackListNames.FEEDBACKLIST, true) };
                this.SectionName = FeedbackSectionName.FEEDBACKDETAILADMINSECTION;

                this.ApproversList = new List<ApplicationStatus>();
                this.CurrentApprover = new ApplicationStatus();

                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ApproverMaster());
                this.MasterData.Add(new BusinessUnitMaster());
                this.MasterData.Add(new BranchMaster());
                this.MasterData.Add(new ProblemCauseMaster());
            }
        }

        /// <summary>
        /// Gets or sets the master data.
        /// </summary>
        /// <value>
        /// The master data.
        /// </value>
        [DataMember, IsListColumn(false), ContainsMasterData(true)]
        public List<IMaster> MasterData { get; set; }

        /// <summary>
        /// Gets or sets the  Product Serial List.
        /// </summary>
        /// <value>
        /// The Product Serial List
        /// </value>
        [DataMember, Required, MaxLength(1000)]
        public string ProductSerial { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember, IsListColumn(false)]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember, IsPerson(true, false), IsViewer]
        public string ProposedBy { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember, IsPerson(true, false, true), FieldColumnName("ProposedBy"), IsViewer]
        public string ProposedByName { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember]
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the workflow status.
        /// </summary>
        /// <value>
        /// The workflow status.
        /// </value>
        [DataMember]
        public string WorkflowStatus { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets the form belong to.
        /// </summary>
        /// <value>
        /// The form belong to.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string FormBelongTo { get; set; }

        /// <summary>
        /// Gets or sets the application belong to.
        /// </summary>
        /// <value>
        /// The application belong to.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string ApplicationBelongTo { get; set; }

        /// <summary>
        /// Gets or sets the list details.
        /// </summary>
        /// <value>
        /// The list details.
        /// </value>
        [DataMember, IsListColumn(false)]
        public List<ListItemDetail> ListDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [DataMember, IsListColumn(false)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the action status.
        /// </summary>
        /// <value>
        /// The action status.
        /// </value>
        [DataMember, IsListColumn(false)]
        public ButtonActionStatus ActionStatus { get; set; }

        /// <summary>
        /// Gets or sets the current approver.
        /// </summary>
        /// <value>
        /// The current approver.
        /// </value>
        [DataMember, IsListColumn(false), IsApproverDetails(true, FeedbackListNames.FEEDBACKAPPAPPROVALMATRIX)]
        public ApplicationStatus CurrentApprover { get; set; }

        /// <summary>
        /// Gets or sets the approvers list.
        /// </summary>
        /// <value>
        /// The approvers list.
        /// </value>
        [DataMember, IsListColumn(false), IsApproverMatrixField(true, FeedbackListNames.FEEDBACKAPPAPPROVALMATRIX)]
        public List<ApplicationStatus> ApproversList { get; set; }

        /// <summary>
        /// Gets or sets the button caption.
        /// </summary>
        /// <value>
        /// The button caption.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string ButtonCaption { get; set; }

        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        [DataMember, Required, RequiredOnDraft, MaxLength(255)]
        public string FeedbackTitle { get; set; }

        /// <summary>
        /// Gets or sets the Business Unit.
        /// </summary>
        /// <value>
        /// The business unit.
        /// </value>
        [DataMember, Required, RequiredOnDraft]
        public string BusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the Project Type.
        /// </summary>
        /// <value>
        /// The project type.
        /// </value>
        [DataMember, Required, RequiredOnDraft]
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the Product Category.
        /// </summary>
        /// <value>
        /// The prdouct category.
        /// </value>
        [DataMember, Required, RequiredOnDraft]
        public string ProductType { get; set; }

        /////// <summary>
        /////// Gets or sets the reference brand model.
        /////// </summary>
        /////// <value>
        /////// The reference brand model.
        /////// </value>
        ////[DataMember, Required, MaxLength(255)]
        ////public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the Sample Required.
        /// </summary> 
        /// <value>
        /// The reference sample required.
        /// </value>
        [DataMember, Required, MaxLength(1000)]
        public string DefectDescription { get; set; }

        /// <summary>
        /// Gets or sets the Gender.
        /// </summary>
        /// <value>
        /// The reference gender.
        /// </value>
        [DataMember, Required, MaxLength(4)]
        public string SupplierCode { get; set; }

        /// <summary>
        /// Gets or sets the Income Group.
        /// </summary>
        /// <value>
        /// The income group.
        /// </value>
        [DataMember]
        public string ProblemCause { get; set; }

        /// <summary>
        /// Gets or sets the Project No.
        /// </summary>
        /// <value>
        /// The project no.
        /// </value>
        [DataMember,]
        public string FeedbackNo { get; set; }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        [DataMember, IsFile(true)]
        public List<FileDetails> Files { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember,  Required]
        public string FileNameList { get; set; }

        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        [DataMember]
        public bool ForwardtoQuality { get; set; }

        /// <summary>
        /// Gets or sets the Forward to Quality.
        /// </summary>
        /// <value>
        /// The Forward to Quality.
        /// </value>
        [DataMember]
        public bool ForwardtoCCQualityIncharge { get; set; }

        /// <summary>
        /// Gets or sets the Project Type.
        /// </summary>
        /// <value>
        /// The project type.
        /// </value>
        [DataMember, IsPerson(true, true, false), IsViewer]
        public string QAUser { get; set; }

        /// <summary>
        /// Gets or sets the Project Type.
        /// </summary>
        /// <value>
        /// The project type.
        /// </value>
        [DataMember, IsPerson(true, true, true), FieldColumnName("QAUser"), IsViewer]
        public string QAUserName { get; set; }

        /// <summary>
        /// Gets or sets the Project Type.
        /// </summary>
        /// <value>
        /// The project type.
        /// </value>
        [DataMember,  IsPerson(true, true, false), IsViewer]
        public string CCQualityInchargeUser { get; set; }

        /// <summary>
        /// Gets or sets the Project Type.
        /// </summary>
        /// <value>
        /// The project type.
        /// </value>
        [DataMember, IsPerson(true, true, true), FieldColumnName("CCQualityInchargeUser"), IsViewer]
        public string CCQualityInchargeName { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string CCQualityInchargeUserComments { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember]
        public string CCFileNameList { get; set; }

        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        [DataMember, MaxLength(1000)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        [DataMember, MaxLength(1000)]
        public string ActionPlans { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember, IsPerson(true, true), IsViewer]
        public string CCActingUser { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember, IsPerson(true, true, true), FieldColumnName("CCActingUser"), IsViewer]
        public string CCActingUserName { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string CCActingUserComments { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string QualityUserComments { get; set; }

        /// <summary>
        /// Gets or sets the Item Code.
        /// </summary>
        /// <value>
        /// The project no.
        /// </value>
        [DataMember, Required, RequiredOnDraft]
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the Item Code.
        /// </summary>
        /// <value>
        /// The project no.
        /// </value>
        [DataMember]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the file name list.
        /// </summary>
        /// <value>
        /// The file name list.
        /// </value>
        [DataMember]
        public string CCQAInchargeFileNameList { get; set; }
    }
}