using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Globals
{
    public class Constant
    {
        public static string SaveMsg { get { return "Data saved successfully."; } }
        public static string UpdateMsg { get { return "Data updated successfully."; } }
        public static string SubmitMsg { get { return "Data submitted successfully."; } }
        public static string ConfirmMsg { get { return "Are you sure, you want to continue?"; } }
        public static string ConfirmMsgDelete { get { return "Are you sure, you want to remove?"; } }
        public static string ServiceServeMsg { get { return "Data served successfully."; } }
    }
    public class ModuleName
    {
        public static string Budget { get { return "Budget"; } }
    }
}