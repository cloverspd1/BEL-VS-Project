using BEL.CommonDataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BEL.FeedbackWorkflow.Models.Master
{
    [DataContract, Serializable]
    public class QualityStatusMasterListItem: IMasterItem
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>  
        [FieldColumnName("Title")]
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [FieldColumnName("Quality_x0020_Status_x0020_Code")]
        [DataMember]
        public string Value { get; set; }
    }
}