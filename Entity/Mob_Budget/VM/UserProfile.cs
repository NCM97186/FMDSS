using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.Mob_BudgetVM
{
    #region Login Request & Response
    public class LoginReqParam
    {
        public string Application { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public bool valid { get; set; }
        public string message { get; set; }
    }

    public class UserDetails
    {
        public string SSOId { get; set; }
        public long UserID { get; set; }
        public string displayName { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string mailPersonal { get; set; }
        public string jpegPhoto { get; set; }
    }

    public class UserDetailsAdditionalInfo
    {
        public int DesignationID { get; set; }
        public long UserID { get; set; }
        public string SSOID { get; set; }
        public string Name { get; set; }
        public string DesignationName { get; set; }
        public string Department { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string DivPrefix { get; set; }
        public string DivCode { get; set; }
        public string Roles { get; set; }
    }

    public class UserDetailsVM
    {
        public UserDetails UserDetails { get; set; }
        public string status { get; set; }
        public UserDetailsAdditionalInfo UserDetailsAdditionalInfo { get; set; }
        public List<DropDownList> RoleList { get; set; }
        public List<DropDownList> FinancialYearList { get; set; }
        public List<DropDownList> BudgetHeadList { get; set; }
        public List<CommonDropDownData> SubBudgetHeadList { get; set; }
        public List<DropDownList> SchemeList { get; set; }
        public List<DropDownList2> CircleList { get; set; }
        public List<DropDownList2> DivList { get; set; }
        public List<CommonDropDownData2> SantatuaryList { get; set; }
        public List<DropDownList> ActivityList { get; set; }
        public List<CommonDropDownData> SubActivityList { get; set; }
        public List<DropDownList2> SubActivityUnit { get; set; }
    }
    #endregion
}