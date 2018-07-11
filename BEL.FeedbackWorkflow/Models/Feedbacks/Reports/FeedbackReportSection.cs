namespace BEL.FeedbackWorkflow.Models
{
    using BEL.CommonDataContract;
    using BEL.FeedbackWorkflow.Models.Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    ///   Feedback Detail Section
    /// </summary>
    /// <seealso cref="BEL.CommonDataContract.ISection" />
    [DataContract, Serializable]
    public class FeedbackReportSection : ISection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackDetailSection"/> class.
        /// </summary>
        public FeedbackReportSection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackDetailSection"/> class.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        public FeedbackReportSection(bool isSet)
        {
            if (isSet)
            {
////                string query = @"<View>
////                                       <Query>
////                                                  <Where>
////                                                    <And>                                                     
////                                                      <Neq>
////                                                            <FieldRef Name='Status' />
////                                                              <Value Type='Text'>" + FormStatus.COMPLETED + @"</Value>
////                                                       </Neq>                                                      
////                                                       <Neq>
////                                                            <FieldRef Name='Status' />
////                                                              <Value Type='Text'>" + FormStatus.REJECTED + @"</Value>
////                                                       </Neq>
////                                                </And>
////                                        </Where></Query></View>";
                this.ListDetails = new List<ListItemDetail>() { new ListItemDetail(FeedbackListNames.FEEDBACKLIST, true) };
                this.SectionName = FeedbackSectionName.FEEDBACKDETAILSECTION;
            }
        }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        [DataMember]
        public string FeedbackNo { get; set; }

        /// <summary>
        /// Gets or sets the request date.
        /// </summary>
        /// <value>
        /// The request date.
        /// </value>
        [DataMember, DataType(DataType.Date)]
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        [DataMember]
        public string BusinessUnits { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        [DataMember]
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        [DataMember, IsPerson(true, false, true), IsViewer]
        public string ProposedBy { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [DataMember]
        public string WorkflowStatus { get; set; }

        /// <summary>
        /// Gets or sets the tour to date.
        /// </summary>
        /// <value>
        /// The tour to date.
        /// </value>
        [DataMember]
        public string ProductGroup { get; set; }

        /// <summary>
        /// Gets or sets the tour to date.
        /// </summary>
        /// <value>
        /// The tour to date.
        /// </value>
        [DataMember]
        public string ProblemCause { get; set; }

        /// <summary>
        /// Gets or sets the tour to date.
        /// </summary>
        /// <value>
        /// The tour to date.
        /// </value>
        [DataMember]
        public string FeedbackTitle { get; set; }

        /// <summary>
        /// Gets or sets the item code.
        /// </summary>
        /// <value>
        /// The item code.
        /// </value>
        [DataMember]
        public string ItemCode { get; set; }

        /// <summary>
        /// Gets or sets the tour to date.
        /// </summary>
        /// <value>
        /// The tour to date.
        /// </value>
        [DataMember]
        public string DefectDescription { get; set; }

        /// <summary>
        /// Gets or sets the tour to date.
        /// </summary>
        /// <value>
        /// The tour to date.
        /// </value>
        [DataMember]
        public string ProductSerial { get; set; }

        /// <summary>
        /// Gets or sets the tour to date.
        /// </summary>
        /// <value>
        /// The tour to date.
        /// </value>
        [DataMember, IsPerson(true, true, false), IsViewer]
        public string CCActingUser { get; set; }

        /// <summary>
        /// Gets or sets the name of the cc acting user.
        /// </summary>
        /// <value>
        /// The name of the cc acting user.
        /// </value>
        [DataMember, IsPerson(true, true, true), FieldColumnName("CCActingUser"), IsViewer]
        public string CCActingUserName { get; set; }

        /// <summary>
        /// SCM Status
        /// </summary>
        [DataMember, IsListColumn(false), DataType(DataType.Date)]
        public DateTime? CCActingAssignDate
        {
            get
            {
                DateTime? date;
                if (this.ApproversList != null && this.ApproversList.Count != 0)
                {
                    date = this.GetRolewiseAssignDate(FeedbackRoles.CCACTINGUSER); 
                    return date;
                }
                return null;  
            }
        }

        /// <summary>
        /// Design engineer status
        /// </summary>
        [DataMember, IsListColumn(false)]
        public string Comments
        {
            get
            {
                if (this.ApproversList != null && this.ApproversList.Count != 0)
                {
                    string status = this.GetRolewiseComment(FeedbackRoles.CCACTINGUSER); ////ApproversList.All(p => p.Role.Contains(DCRRoles.SCM) && p.Status == "Approved") ? "Completed" : "Pending";
                    return status;
                }
                return "-";
            }
        }

        /// <summary>
        /// Gets or sets the cc quality incharge user.
        /// </summary>
        /// <value>
        /// The cc quality incharge user.
        /// </value>
        [DataMember, Required, IsPerson(true, true, false), IsViewer]
        public string CCQualityInchargeUser { get; set; }

        /// <summary>
        /// Gets or sets the name of the cc quality incharge.
        /// </summary>
        /// <value>
        /// The name of the cc quality incharge.
        /// </value>
        [DataMember, IsPerson(true, true, true), FieldColumnName("CCQualityInchargeUser"), IsViewer]
        public string CCQualityInchargeName { get; set; }

        /// <summary>
        /// SCM Status
        /// </summary>
        [DataMember, IsListColumn(false), DataType(DataType.Date)]
        public DateTime? CCQualityIcAssignDate
        {
            get
            {
                DateTime? date = null;
                if (this.ApproversList != null && this.ApproversList.Count != 0)
                {
                    date = this.GetRolewiseAssignDate(FeedbackRoles.CCQUALITYINCHARGEUSER); 
                    return date;
                }
                return date;
            }
        }

        /// <summary>
        /// Design engineer status
        /// </summary>
        [DataMember, IsListColumn(false)]
        public string QualityComments
        {
            get
            {
                if (this.ApproversList != null && this.ApproversList.Count != 0)
                {
                    string status = this.GetRolewiseComment(FeedbackRoles.CCQUALITYINCHARGEUSER); ////ApproversList.All(p => p.Role.Contains(DCRRoles.SCM) && p.Status == "Approved") ? "Completed" : "Pending";
                    return status;
                }
                return "-";
            }
        }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        [DataMember, IsListColumn(false)]
        public string SectionName { get; set; }

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
        /// Gets the rolewise assign date.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>the string</returns>
        public DateTime? GetRolewiseAssignDate(string role)
        {
            if (this.ApproversList != null && this.ApproversList.Count != 0 && this.ApproversList.Any(p => p.Role.Contains(role)))
            {
                DateTime? assignDate = null;
                List<ApplicationStatus> approvers = this.ApproversList.Where(p => p.Role.Contains(role)).ToList();
                if (approvers != null && approvers.Count != 0)
                {
                    this.ApproversList.ForEach(p =>
                    {
                        if (p.Role == FeedbackRoles.CCACTINGUSER)
                        {
                            assignDate = this.ApproversList.FirstOrDefault(q => q.Role == FeedbackRoles.CCACTINGUSER).AssignDate;
                        }

                        if (p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER)
                        {
                            assignDate = this.ApproversList.FirstOrDefault(q => q.Role == FeedbackRoles.CCACTINGUSER).AssignDate;
                        }
                    });
                }
                return assignDate;
            }
            return null;
        }

        /// <summary>
        /// Gets the rolewise comment.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>the string</returns>
        public string GetRolewiseComment(string role)
        {
            if (this.ApproversList != null && this.ApproversList.Count != 0 && this.ApproversList.Any(p => p.Role.Contains(role)))
            {
                string comments = "-";
                List<ApplicationStatus> approvers = this.ApproversList.Where(p => p.Role.Contains(role)).ToList();
                if (approvers != null && approvers.Count != 0)
                {
                    this.ApproversList.ForEach(p =>
                            {
                                if (p.Role == FeedbackRoles.CCACTINGUSER)
                                {
                                    comments = this.ApproversList.FirstOrDefault(q => q.Role == FeedbackRoles.CCACTINGUSER).Comments;
                                }
                                if (p.Role == FeedbackRoles.CCQUALITYINCHARGEUSER)
                                {
                                    comments = this.ApproversList.FirstOrDefault(q => q.Role == FeedbackRoles.CCACTINGUSER).Comments;
                                }
                            });
                }
                return comments;
            }
            return "-";
        }
    }
}