using FMDSS.Models.CitizenService.PermissionServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CitizenService.PermissionServices
{
    [Serializable]//add by rajveer load balanceing
    public class NocList:BaseModel
    {
        public long ResearchID { get; set; }
        public string RequestId { get; set; }
        public string NocType { get; set; }
        public string ReqDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
        public string ActionTakenUser { get; set; }
    }
    public class NOCApprovalDetails
    {
        public long ResearchID { get; set; }
        public string reqid { get; set; }
        public string cmd { get; set; }
        public string cmdText { get; set; }
        public string Comments { get; set; }
        public string presDate { get; set; }
        public string GOILetterNo { get; set; }
        public string GOIResponseNo { get; set; }
        public string GORLetterNo { get; set; }
    }

    [Serializable]//add by rajveer load balanceing
    public class OtherNocList:BaseModel
    {
        public string RequestId { get; set; }
        public string NocType { get; set; }
        public string ReqDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
    }

    [Serializable]//add by rajveer load balanceing
    public class NocData:BaseModelSerializable
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
        public bool IsSWCS { get; set; }

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
        public string ApplicantTypeStr { get; set; }

        public string personStr { get; set; }
        public IList<clsPermission> gislist { get; set; }
        public IList<PlantFixedPermission> plantList { get; set; }
        public PersonSW personInfo { get; set; }

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

        public int AssignRequest(string ReqId, string ssoid)
        {
            try
            {
                Common cm = new Common();
                SqlParameter[] parameters ={
                            new SqlParameter("@RequestedId", ReqId),
                            new SqlParameter("@ssoid", ssoid) };
                return cm.ExecuteNonQuery("Sp_Dashboard_DFOforwardRequest", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ReviewApprove(string ReqId, string ssoid, int Action, string Reason, string SurveyDoc)
        {
            try
            {
                Common cm = new Common();
                SqlParameter[] parameters ={
                            new SqlParameter("@REQUESTID", ReqId),
                            new SqlParameter("@ssoid", ssoid),

                            new SqlParameter("@ACTION", Action),
                            new SqlParameter("@ACTIONREASON", Reason),
                            new SqlParameter("@REMARKS", ""),
                            new SqlParameter("@SURVEY_DOC", SurveyDoc)
                };
                return cm.ExecuteNonQuery("KN_REQUEST_REVIEWAPPROVE", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetRequestStatus(string ReqId)
        {
            try
            {
                Common cm = new Common();
                SqlParameter[] parameters = { new SqlParameter("@REQUESTID", ReqId) };
                return Convert.ToInt32(cm.ExecuteScalar("KN_GETREQUESTSTATUS", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllRequests(string _ssoId = null, int UserId = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                Common cm = new Common();
                SqlParameter[] parameters = { new SqlParameter("@SSOId", _ssoId), new SqlParameter("@UserId", UserId) };
                cm.Fill(ds, "KN_GetAllRequests", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRequestDetail(string reqId)
        {
            try
            {
                DataSet ds = new DataSet();
                Common cm = new Common();
                SqlParameter[] parameters = { new SqlParameter("@RequestId", reqId) };
                cm.Fill(ds, "KN_GetReqDetail", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReAssignReq(string reqId)
        {
            try
            {
                DataSet ds = new DataSet();
                Common cm = new Common();
                SqlParameter[] parameters = { new SqlParameter("@RequestId", reqId) };
                cm.Fill(ds, "KN_GetReAssignReq", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    [Serializable]//add by rajveer load balanceing
    public class PersonSW:BaseModelSerializable
    {
        public string Establishmentname { get; set; }
        public string Total_Employees { get; set; }
        public string ProposedInvestment { get; set; }
        public string Operational_Date { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CategoryofEstablishment { get; set; }
        public string NatureOfBusiness { get; set; }
        public string PlotNo { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string RuralUrban { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string Village { get; set; }
        public string Tehsil { get; set; }
        public string District { get; set; }
        public string PIN { get; set; }
        public string BusinessDetail { get; set; }
        public string PrimaryGroup { get; set; }

        public string BRN { get; set; }
        public string PAN { get; set; }
        public string TIN { get; set; }
        public string VAT { get; set; }
        public string STDCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string PostalAddress { get; set; }
        public string Est_PlotNo { get; set; }
        public string Est_Street { get; set; }
        public string Est_Area { get; set; }
        public string Est_RuralUrban { get; set; }
        public string Est_District { get; set; }
        public string Est_Tehsil { get; set; }
        public string Est_Ward { get; set; }
        public string Est_Village { get; set; }        
        public string Est_PIN { get; set; }

        public string FatherName { get; set; }
    }
}