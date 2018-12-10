
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
    [DataContract, Serializable]
    public class LUMQualityUserSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QaulityUserSection"/> class.
        /// </summary>
        public LUMQualityUserSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QaulityUserSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public LUMQualityUserSection(bool isSet)
        {
            if (isSet)
            {
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(FeedbackListNames.FEEDBACKLIST, true) };
                this.SectionName = FeedbackSectionName.LUMQAULITYUSERSECTION;
                this.ApproversList = new List<ApplicationStatus>();
                this.CurrentApprover = new ApplicationStatus();
                this.MasterData = new List<IMaster>();
                this.MasterData.Add(new ProblemCauseMaster());
                this.MasterData.Add(new QualityStatusMaster());
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
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember, IsListColumn(false)]
        public int ID { get; set; }

        [DataMember, Required, RequiredOnDraft]
        public bool ForwardtoQuality { get; set; }

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
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public DateTime? QualityUserDate { get; set; }


        [DataMember]
        public string CCQAInchargeFileNameList { get; set; }

        [DataMember]
        public DateTime? QualityInchargeDate { get; set; }

        [DataMember]
        public DateTime? ImplementationDate { get; set; }

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

        [DataMember, MaxLength(1000)]
        //[DataMember, MaxLength(1000)]
        public string ImplementedRemark { get; set; }

        [DataMember]
        public string QUFileNameList { get; set; }

        [DataMember, IsFile(true)]
        public List<FileDetails> Files { get; set; }

        [DataMember]
        public bool SendBackToCC { get; set; }

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

        [DataMember]
        public string QualityStatus { get; set; }

        [DataMember]
        public string Implemented { get; set; }


        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        [DataMember, MaxLength(1000)]
        //[DataMember, MaxLength(1000)]
        public string Observations { get; set; }

        /// <summary>
        /// Gets or sets the Project Name.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        [DataMember, MaxLength(1000)]
        // [DataMember, MaxLength(1000)]
        public string ActionPlans { get; set; }

        /// <summary>
        /// Gets or sets the Income Group.
        /// </summary>
        /// <value>
        /// The income group.
        /// </value>
        [DataMember]
        public string ProblemCause { get; set; }

        /// <summary>
        /// Gets or sets the Income Group.
        /// </summary>
        /// <value>
        /// The income group.
        /// </value>
        [DataMember, IsListColumn(false), IsPerson(true, true, false), IsViewer]
        public string QAHeadUser { get; set; }
    }
}