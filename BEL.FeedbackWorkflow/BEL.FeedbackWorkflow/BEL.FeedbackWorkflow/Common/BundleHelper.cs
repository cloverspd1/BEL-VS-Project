namespace NPD.Workflow.Common
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Bundle version Helper
    /// </summary>
    public class BundleHelper
    {
        /// <summary>
        /// return string with version number
        /// </summary>
        /// <value>
        /// none none.
        /// </value>
        public static string StyleVersion
        {
            get
            {
                return "<link href=\"{0}?v=" + ConfigurationManager.AppSettings["version"] + "\" rel=\"stylesheet\"/>";
            }
        }

        /// <summary>
        /// return string with version number
        /// </summary>
        /// <value>
        /// none none.
        /// </value>
        public static string ScriptVersion
        {
            get
            {
                return "<script src=\"{0}?v=" + ConfigurationManager.AppSettings["version"] + "\"></script>";
            }
        }
    }
}