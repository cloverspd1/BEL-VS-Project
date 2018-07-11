﻿namespace BEL.FeedbackWorkflow.Models.Master
{
    using CommonDataContract;
    using Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;

    /// <summary>
    /// Regions Master
    /// </summary>
    [DataContract, Serializable]
    public class BranchMaster : IMaster
    {
        /// <summary>
        /// Gets or sets the name of master.
        /// </summary>
        /// <value>
        /// The name of master.
        /// </value>
        [DataMember]
        public string NameOfMaster
        {
            get { return FeedbacksMasters.BRANCHMASTER; }
            set { }
        }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        [DataMember]
        public string Scope
        {
            get { return ListScope.LOCAL; }
            set { }
        }

        /// <summary>
        /// Gets or sets the name of the list.
        /// </summary>
        /// <value>
        /// The name of the list.
        /// </value>
        [DataMember]
        public string ListName
        {
            get { return FeedbacksMasters.BRANCHMASTER; }
            set { }
        }

        /// <summary>
        /// Gets or sets the master items.
        /// </summary>
        /// <value>
        /// The master items.
        /// </value>
        [DataMember]
        public List<IMasterItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the caching interval in HRS.
        /// </summary>
        /// <value>
        /// The caching interval in HRS.
        /// </value>
        [DataMember]
        public int CachingIntervalInHrs
        {
            get { return 24; }
            set { }
        }

        /// <summary>
        /// Gets or sets the type of the item.
        /// </summary>
        /// <value>
        /// The type of the item.
        /// </value>
        [DataMember]
        public Type ItemType
        {
            get { return typeof(BranchMasterListItem); }
            set { }
        }
    }
}