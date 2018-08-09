using BEL.CommonDataContract;
using BEL.FeedbackWorkflow.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BEL.FeedbackWorkflow.Models.Master
{
    /// <summary>
    /// Quality Status Master
    /// </summary>
    [DataContract, Serializable]
    public class QualityStatusMaster : IMaster
    {
        [DataMember]
        public int CachingIntervalInHrs
        {
            get{ return 24;}

            set{}
        }
        [DataMember]
        public List<IMasterItem> Items{get;set;}
        [DataMember]
        public Type ItemType{
            get {return typeof(QualityStatusMasterListItem);}

            set {}
        }
        [DataMember]
        public string ListName
        {
            get
            {
                return FeedbacksMasters.QUALITYSTATUSMASTER;
            }

            set
            {
                
            }
        }
        [DataMember]
        public string NameOfMaster
        {
            get
            {
                return FeedbacksMasters.QUALITYSTATUSMASTER;
            }

            set
            {
                
            }
        }
        [DataMember]
        public string Scope
        {
            get
            {
                 return ListScope.LOCAL; 
            }

            set
            {
               
            }
        }
    }
}