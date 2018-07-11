namespace BEL.FeedbackWorkflow.Models.Master
{
    using CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;

    /// <summary>
    /// Department Master List Item
    /// </summary>
    [DataContract, Serializable]
    public class ApproverMasterListItem : IMasterItem
    {
        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        [DataMember, FieldColumnName("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DataMember, FieldColumnName("Title")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        /// <value>
        /// The Role.
        /// </value>
        [DataMember, FieldColumnName("Role")]
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        [DataMember, IsPerson(true, true), FieldColumnName("UserName")]
        public string UserID { get; set; }

        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>
        /// The UserName.
        /// </value>
        [DataMember, IsPerson(true, true, true), FieldColumnName("UserName")]
        public string UserName { get; set; }

        ///// <summary>
        ///// Gets or sets the value.
        ///// </summary>
        ///// <value>
        ///// The value.
        ///// </value>
        ////[DataMember, FieldColumnName("BusinessUnit", true, false, "Title")]
        ////public string BusinessUnit { get; set; }     
    }
}