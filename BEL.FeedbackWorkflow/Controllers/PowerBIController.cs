namespace BEL.FeedbackWorkflow.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    ///    PowerBI Controller
    /// </summary>
    public class PowerBIController : Controller
    {
        /// <summary>
        /// Index Method
        /// </summary>
        /// <returns>Returns View</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}
