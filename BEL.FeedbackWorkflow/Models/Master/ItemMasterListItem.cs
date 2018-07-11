namespace BEL.FeedbackWorkflow.Models.Master
{
    using CommonDataContract;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;

    /// <summary>
    /// Item Master ListItem
    /// </summary>
    [DataContract, Serializable]
    public class ItemMasterListItem : IMasterItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>  
        [FieldColumnName("ItemCode")]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [FieldColumnName("ItemCode")]
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>  
        [FieldColumnName("ID")]
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the Reference No.
        /// </summary>
        /// <value>
        /// The Reference No.
        /// </value>
        [DataMember, IsListColumn(true), FieldColumnName("ItemDescription")]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Gets or sets the Business Unit.
        /// </summary>
        /// <value>
        /// The Business Unit.
        /// </value>
        [DataMember, IsListColumn(true), FieldColumnName("BusinessUnit")]
        public string BusinessUnits { get; set; }

        /// <summary>
        /// Product Group
        /// </summary>
        [DataMember, IsListColumn(true), FieldColumnName("ProductGroup")]
        public string ProductGroup { get; set; }
    }
}