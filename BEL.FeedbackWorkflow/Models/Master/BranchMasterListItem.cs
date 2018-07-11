namespace BEL.FeedbackWorkflow.Models.Master
{
    using CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;

    /// <summary>
    /// Regions Master List Item
    /// </summary>
    [DataContract, Serializable]
    public class BranchMasterListItem : IMasterItem
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
        [FieldColumnName("Code")]
        [DataMember]
        public string Value { get; set; }     
    }
}