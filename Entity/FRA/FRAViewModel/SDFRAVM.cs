using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class SDFRAList
    {
        public string RequestId { get; set; }
        public string NocType { get; set; }
        public string ReqDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
    }

    public class SDFRAData
    {
        #region data members
        public int Id { get; set; }
        public int NOCPurpose { get; set; }
        public int NOCType { get; set; }
        public string NOCPurposeStr { get; set; }
        public string NOCTypeStr { get; set; }

        //SWCS data member
        public string SWSID { get; set; }
        public string IsNew { get; set; }
        public string Userdetails { get; set; }
        public bool IsView { get; set; }

        public string GISID { get; set; }
        public string RequestedID { get; set; }
        public Int64 UserID { get; set; }
        public string SSOID { get; set; }
        public string kioskuserid { get; set; }
        public string UserName { get; set; }

        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Final_Amount { get; set; }
        public string TransactionId { get; set; }
        public int Trn_Status_Code { get; set; }
        public string PayStatus { get; set; }
        public string Duration_From { get; set; }
        public string Duration_To { get; set; }
        public string Industrial_Type { get; set; }
        public string Sawmill_Type { get; set; }
        public string Sawmill_Size { get; set; }
        public bool IsGTSheetAvailable { get; set; }

        public decimal PerposedArea { get; set; }
        public string KML_Path { get; set; }
        public string Revenue_Record_Path { get; set; }
        public string Revenue_Map_Path { get; set; }
        public string Revenue_Map_Signed { get; set; }

        //KML Related Data
        public string Nearest_WaterSource { get; set; }
        public string WaterSource_Distance { get; set; }
        public string Forest_Distance { get; set; }
        public string Wildlife_Distance { get; set; }
        public string Tree_species { get; set; }
        public string AravalliHills { get; set; }
        public string ForestLand { get; set; }
        public string Plantation_Area { get; set; }

        public string Additional_Document { get; set; }
        public string GPSLat { get; set; }
        public string GPSLong { get; set; }

        public string Citizen_Comment { get; set; }
        public string Area_Size { get; set; }
        public int ApplicantType { get; set; }
        public IList<clsPermission> gislist { get; set; }
        public IList<PlantFixedPermission> plantList { get; set; }

        public int Status { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime Created_Date { get; set; }
        public string OtherPermission { get; set; }

        #region Commented Property
        //public string PayStatus { get; set; }
        //public string ReasonsAttached { get; set; }
        //public string OtherPermission { get; set; }
        //public DateTime LastUpdatedOn { get; set; }
        //public string TransactionId { get; set; }
        //public Int64 PermissionId { get; set; }
        //public string PermissionType { get; set; }
        //public int IsActive { get; set; }
        //public string ConditionRevenueMapSigned { get; set; }
        //public string ConditionIsGTSheetAvailable { get; set; }
        //public bool ConditionFileEditMode { get; set; }
        //public bool ConditionGISMode { get; set; }
        //public string txtForestDensity { get; set; }
        //public string txtplantOthers { get; set; }
        //public string txtplantOthersNo { get; set; } 
        #endregion
        #endregion

        //public int AssignRequest(string ReqId, string ssoid)
        //{
        //    try
        //    {
        //        Common cm = new Common();
        //        SqlParameter[] parameters ={
        //                    new SqlParameter("@RequestedId", ReqId),
        //                    new SqlParameter("@ssoid", ssoid) };
        //        return cm.ExecuteNonQuery("Sp_Dashboard_DFOforwardRequest", parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int ReviewApprove(string ReqId, string ssoid, int Action, string Reason, string SurveyDoc)
        //{
        //    try
        //    {
        //        Common cm = new Common();
        //        SqlParameter[] parameters ={
        //                    new SqlParameter("@REQUESTID", ReqId),
        //                    new SqlParameter("@ssoid", ssoid),

        //                    new SqlParameter("@ACTION", Action),
        //                    new SqlParameter("@ACTIONREASON", Reason),
        //                    new SqlParameter("@REMARKS", ""),
        //                    new SqlParameter("@SURVEY_DOC", SurveyDoc)
        //        };
        //        return cm.ExecuteNonQuery("KN_REQUEST_REVIEWAPPROVE", parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int GetRequestStatus(string ReqId)
        //{
        //    try
        //    {
        //        Common cm = new Common();
        //        SqlParameter[] parameters = { new SqlParameter("@REQUESTID", ReqId) };
        //        return Convert.ToInt32(cm.ExecuteScalar("KN_GETREQUESTSTATUS", parameters));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet GetAllRequests(string _ssoId)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        Common cm = new Common();
        //        SqlParameter[] parameters = { new SqlParameter("@SSOId", _ssoId) };
        //        cm.Fill(ds, "KN_GetAllRequests", parameters);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public DataSet GetRequestDetail(string reqId)
        //{
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        Common cm = new Common();
        //        SqlParameter[] parameters = { new SqlParameter("@RequestId", reqId) };
        //        cm.Fill(ds, "KN_GetReqDetail", parameters);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}