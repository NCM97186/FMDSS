
//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : ManageAssetController
//  Description  : File contains calling functions from UI
//  Date Created : 18-Nov-2015
//  History      : 
//  Version      : 1.0
//  Author       : Rajkumar Singh
//  Modified By  : Rajkumar Singh
//  Modified On  : 12-Feb-2016
//  Reviewed By  : Ashok Yadav
//  Reviewed On  : 12-Feb-2016
//********************************************************************************************************


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FMDSS.Filters;
using System.Web.Configuration;

namespace FMDSS.Models.ForesterAction
{
    [MyAuthorization]
    [MyExceptionHandler]
    /// <summary>
    /// Model contain public property and methods
    /// </summary>
    public class ActionRequest : DAL
    {

        #region data members

        public string InpectionDate { get; set; }
        public int SurveyReportCount { get; set; }
        public Int64 ActionRequestId { get; set; }
        public string RequestId { get; set; }
        public string ServiceType { get; set; }
        public string PermissionName { get; set; }
        public string SubPermissionName { get; set; }

        //[DataType(DataType.Date)]
        public DateTime RequestOn { get; set; }
        public decimal PaidAmount { get; set; }
        public int status { get; set; }
        public string Status { get; set; }
        public string TableName { get; set; }
        public string ModuleName { get; set; }
        public string PermissionType { get; set; }
        public string RequestedOn { get; set; }
        public string RequestedBy { get; set; }
        public string Designation { get; set; }
        public string Payment { get; set; }
        public string NumberOfPerson { get; set; }
        public string Action { get; set; }
        public string reason { get; set; }
        public string Comment { get; set; }
        public bool IsReviewer { get; set; }
        public int ApprovedBy { get; set; }
        public string ApproveComment { get; set; }
        public int ActionTakenBy { get; set; }
        public string ReviewedComment { get; set; }
        public int ModuleId { get; set; }
        public int ServiceTypeId { get; set; }
        public int PermissionId { get; set; }
        public int SubPermissionId { get; set; }
        public string TransactionStatus { get; set; }
        public string Durations { get; set; }
        public string DurationsFrom { get; set; }
        public string DurationsTo { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }
        public string ApplicationType { get; set; }
        public string FilmTitle { get; set; }
        public string ShootingPurpose { get; set; }
        public string IdProof { get; set; }
        public string IdProofNo { get; set; }
        public string NumberOfDay { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string Block { get; set; }
        public string GramPanchayat { get; set; }
        public string Qualification { get; set; }
        public string College { get; set; }
        public string ResearchSubject { get; set; }
        public string ResearchProcedure { get; set; }
        public string FixedPermissionName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AnimalCategory { get; set; }
        public string AnimalName { get; set; }
        public string SpeciesCategory { get; set; }
        public string SpeciesName { get; set; }
        public string IsGTSheetAvaliable { get; set; }
        public string Nearest_WaterSource { get; set; }
        public string WaterSource_Distance { get; set; }
        public string Forest_Distance { get; set; }
        public string Wildlife_Distance { get; set; }
        public string Tree_species { get; set; }
        public string AravalliHills { get; set; }
        public string ForestLand { get; set; }
        public string Plantation_Area { get; set; }

        //added by arvind for manage Notice
        public string Notice_No { get; set; }
        public string Region_Name { get; set; }
        public string Circle_Name { get; set; }
        public string Division_Name { get; set; }
        public string Range_Name { get; set; }
        public string Depot_Name { get; set; }
        public string Produce_Name { get; set; }
        public string Product_Name { get; set; }
        public string Produce_Unit { get; set; }
        public string Qty { get; set; }
        public string ReservedPrice { get; set; }
        public string FinancialYear { get; set; }
        public string EstimatedAmount { get; set; }
        public decimal ReviewedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public string Survey_Doc { get; set; }

        public string AppearanceDate { get; set; }
        public string OffenseCode { get; set; }

        //Added by Vandana Gupta to implement SLA CR on 07-Jul-2016
        public string LastDateofAction { get; set; }
        public string PendingSinceDays { get; set; }

        //Added by Ashok Yadav to implement Protaction on 20-10-2016
        public string Time_of_offense { get; set; }
        public string InvestigatingOfficer { get; set; }
        public string Completiondate { get; set; }

        #endregion

        /// <summary>
        /// used for bind forester dash board
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataSet BindAllServiceRequest(int uid, int action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Select_AllServiceRequest_DFO", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", uid);
                cmd.Parameters.AddWithValue("@ActionId", action);
                //Added by Vandana Gupta to implement SLA CR on 01-Jul-2016
                cmd.Parameters.AddWithValue("@SLADays", Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SLAForCitizenRequests"]));
                //End For SLA CR
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Used for Bind RO descision
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>

        public DataSet BindProtectionRODecision(int action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_PendingFOR_RODecision", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SSOID", HttpContext.Current.Session["SSOid"]);
                //cmd.Parameters.AddWithValue("@ActionId", action);
                //Added by Vandana Gupta to implement SLA CR on 01-Jul-2016
                // cmd.Parameters.AddWithValue("@SLADays", Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SLAForCitizenRequests"]));
                //End For SLA CR
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataSet BindAllServiceRequestDFO(int uid, int action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Select_AllServiceRequest_DFO", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", uid);
                cmd.Parameters.AddWithValue("@ActionId", action);
                //Added by Vandana Gupta to implement SLA CR on 01-Jul-2016
                cmd.Parameters.AddWithValue("@SLADays", Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SLAForCitizenRequests"]));
                //End For SLA CR
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        // added by Arvind

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataSet BindAProductionServiceRequest(int uid, int action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FDP_Select_ProductionServiceRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", uid);
                cmd.Parameters.AddWithValue("@ActionId", action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Function for binding dashboard
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataSet BindDevelopmentServiceRequest(int uid, int action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Dashboard_Forest_Development", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", uid);
                cmd.Parameters.AddWithValue("@Action", action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Function for binding dashboard
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataSet BindDevelopmentTransactionRequest(int uid)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Dasboard_My_Transaction", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", uid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Function for binding dashboard
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public DataSet BindProductionTransactionRequest(int uid)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Dasboard_My_Transaction_Production", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", uid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataSet BindProtectionServiceRequest(int uid, int action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Dashboard_Forest_Protection", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", uid);
                cmd.Parameters.AddWithValue("@Action", action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Used for binding reason drop down
        /// </summary>
        /// <param name="ActionId"></param>
        /// <returns></returns>
        public DataTable BindReasonList(int ActionId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_getReason", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionId", ActionId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Used for bind action 
        /// </summary>
        /// <param name="IsReviewer"></param>
        /// <returns></returns>
        public DataTable BindddlActions(bool IsReviewer)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetActions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IsReviewer", IsReviewer);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Get details based on requestid and table name
        /// </summary>
        /// <param name="reqid"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataTable BindActionList(string reqid, string tblName)
        {
            try
            {
                DALConn();
                RequestId = reqid;
                TableName = tblName;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Select_PendingActionResult_V1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestId);
                cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet GetActionList(string reqid, string tblName)
        {
            try
            {
                DALConn();
                RequestId = reqid;
                TableName = tblName;
                DataSet dsData = new DataSet();
                SqlCommand cmd = new SqlCommand("Select_PendingActionResult_V1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestId);
                cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsData);
                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        public DataSet BindActionListWithDataset(string reqid, string tblName)
        {
            try
            {
                DALConn();
                RequestId = reqid;
                TableName = tblName;
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Select_PendingActionResult_V1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", RequestId);
                cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Get details based on requestid and table name
        /// </summary>
        /// <param name="reqid"></param>
        /// <param name="tblName"></param>
        /// <returns></returns>
        public DataTable ReasonList(string CommmaSeperatedReason)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_GetMultiReason", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Reason", CommmaSeperatedReason);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Get multiple district
        /// </summary>
        /// <param name="CommmaSeperatedReason"></param>
        /// <returns></returns>
        public DataTable MultiDistrict(string CommmaSeperatedDist)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_GetMultiDist", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@data", CommmaSeperatedDist);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataSet GetFIRDetails(string RequestId, string Status, string TableName)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetFIRDetail", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@Status", Status);
                // cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            //finally
            //{
            //    Conn.Close();
            //}
        }
        public DataSet GetCompoundDetails(string RequestId, string Status, string TableName)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetPrintForComponding", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@Status", Status);
                // cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            //finally
            //{
            //    Conn.Close();
            //}
        }

        public DataSet GetSeizedDetails(string RequestId, string Status, string TableName)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetPrintForSeizediteam", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", RequestId);

                // cmd.Parameters.AddWithValue("@TableName", TableName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            //finally
            //{
            //    Conn.Close();
            //}
        }
        public DataTable GetOfficerDesignation()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Dashboard_GetFOfficerDesig", Conn);
                cmd.Parameters.AddWithValue("@option", "1");
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"]);
                cmd.Parameters.AddWithValue("@EmpDesig", "");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Used for identify reviewer or approver
        /// </summary>
        /// <returns></returns>
        public bool IdentifyApproveReview(string reqId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_IdentifyReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
                cmd.Parameters.AddWithValue("@ServiceTypeId", ServiceTypeId);
                cmd.Parameters.AddWithValue("@PermissionId", PermissionId);
                cmd.Parameters.AddWithValue("@SubPermissionId", SubPermissionId);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@REQID", reqId);
                bool chId = Convert.ToBoolean(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Final submission on button click
        /// </summary>
        /// <returns></returns>
        public DataSet SubmitActionResult()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_UpdateReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
                cmd.Parameters.AddWithValue("@ServiceTypeId", ServiceTypeId);
                cmd.Parameters.AddWithValue("@PermissionId", PermissionId);
                cmd.Parameters.AddWithValue("@SubPermissionId", SubPermissionId);
                cmd.Parameters.AddWithValue("@DurationFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DurationTo", DateTo);
                cmd.Parameters.AddWithValue("@ReviewedAmount", ReviewedAmount);
                cmd.Parameters.AddWithValue("@ApprovedAmount", ApprovedAmount);
                cmd.Parameters.AddWithValue("@AppreanceDate", AppearanceDate);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ActionTakenBy", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@ActionReason", reason);
                cmd.Parameters.AddWithValue("@Remarks", ReviewedComment);
                cmd.Parameters.AddWithValue("@TableName", TableName);
                cmd.Parameters.AddWithValue("@Survey_Doc", Survey_Doc);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataSet SubmitDFO_Forward(string ssoid)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Dashboard_DFOforwardRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedId", RequestId);
                cmd.Parameters.AddWithValue("@ssoid", ssoid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        public int SubmitTransitPermit(string PERMIT_NO, string DATE, string TIME,string USERID)
        {
            int i = 0;
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_UPDATE_TPC_INSPECTIONDATE", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PERMIT_NO", PERMIT_NO);
                cmd.Parameters.AddWithValue("@DATE", DATE);
                cmd.Parameters.AddWithValue("@TIME", TIME);
                cmd.Parameters.AddWithValue("@USERID", USERID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        SendEmailAfterTransitPermit(Convert.ToString(dr["EmailId"]),PERMIT_NO, Convert.ToString(dr["Name"]), Convert.ToString(dr["InspectionDate"]), Convert.ToString(dr["Mobile"]));
                    }

                }
            }
            catch (Exception ex)
            {
                i = 0;
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return i;
        }
        /// <summary>
        /// Function for getting multiple district,gp,village
        /// </summary>
        /// <param name="RequestedID"></param>
        /// <returns></returns>
        public DataSet GetFixedDistMap(string RequestedID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_GetFixedDistMap", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedID", RequestedID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet GetMultiMicroPlanProj(string RequestedID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FDM_GetMultipleMicroPlanProject", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MicroPlanId", Convert.ToInt64(RequestedID));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }


        /// <summary>
        /// Get Action Name on id 
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string ActionDetail(string val)
        {
            string status = "";
            switch (val)
            {
                case "2":
                    status = "Approve";
                    break;
                case "3":
                    status = "Reject";
                    break;
                case "6":
                    status = "Reassign";
                    break;
                case "7":
                    status = "Review";
                    break;
            }
            return status;
        }

        public string GISPerName(string permissionid)
        {
            string requestfor = "";
            switch (permissionid)
            {
                case "1":
                    requestfor = "Cable";
                    break;
                case "2":
                    requestfor = "Transmission";
                    break;
                case "3":
                    requestfor = "Industry";
                    break;
                case "4":
                    requestfor = "Mines";
                    break;
                case "5":
                    requestfor = "Hospital";
                    break;
                case "6":
                    requestfor = "Power";
                    break;
                case "7":
                    requestfor = "School";
                    break;
                case "8":
                    requestfor = "Roads";
                    break;
                case "9":
                    requestfor = "Sawmill";
                    break;
                case "10":
                    requestfor = "Telephone";
                    break;
                case "11":
                    requestfor = "Other";
                    break;
                case "12":
                    requestfor = "Forest";
                    break;
            }
            return requestfor;


        }


        /// <summary>
        /// Used for identify reviewer or approver
        /// </summary>
        /// <returns></returns>
        public DataSet GetForestDashboardSummaryData(Int64 UserId, string SSOID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetForestDashboardSummaryData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet GetdashboardDetails(long userid)
        {
            DataSet dtdropdown = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_CitizenTransmitpermitDashboard", Conn);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtdropdown);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_All_TransactionReq_ByUserID" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtdropdown;
        }


        public void SendEmailAfterTransitPermit(string AllEmails, string PERMIT_NO,string Name, string InspectionDateTime, string Mobile)
        {
            try
            {
                #region  after SUCCESS flag send SMS and Email to the user // code by Rajveer Sharma

                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

                if (!string.IsNullOrEmpty(AllEmails))
                {
                    #region Send Email Submit Application
                    string UserMailBody = objSMSandEMAILtemplate.ReconciliationMail("Mail", PERMIT_NO, Name, InspectionDateTime, WebConfigurationManager.AppSettings["TransitPermit"].ToString());
                    string subject = "Transit Permit";
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, AllEmails, string.Empty);
                    #endregion
                }

                if (!string.IsNullOrEmpty(Mobile))
                {
                    string UserSmsBody = string.Empty;
                    #region Send SMS Approval Technical Officer
                    // string UserSmsBody = UserMailSMSBody("SMS", ACTION, RequestId, IsApproveAndRejectStatus);
                    UserSmsBody = objSMSandEMAILtemplate.ReconciliationMail("SMS", PERMIT_NO, Name, InspectionDateTime, WebConfigurationManager.AppSettings["TransitPermitSMS"].ToString());
                    SMS_EMail_Services.sendSingleSMS(Mobile, UserSmsBody);
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        public DataTable GetSurveyReportCitizen(string reqid, string Action)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_CTP_GetServeyReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestID", reqid);
                cmd.Parameters.AddWithValue("@Action", Action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        
    }


    public class ForesterDashboardSummary
    {
        public string CitizenMyAction { get; set; }
        public string CitizenPendingRequests { get; set; }
        public string CitizenPendingAtDFOorDCF { get; set; }
        public bool CitizenModule { get; set; }
        public string DevelopmentMyAction { get; set; }
        public string DevelopmentPendingRequests { get; set; }
        public string DevelopmentMyTransactions { get; set; }
        public bool DevelopmentModule { get; set; }
        public string ProductionMyAction { get; set; }
        public string ProductionPendingRequests { get; set; }
        public string ProductionMyTransactions { get; set; }
        public bool ProductionModule { get; set; }
        public string ProtectionMyAction { get; set; }
        public string ProtectionPendingRequests { get; set; }
        public string ProtectionMyTransactions { get; set; }
        public bool ProtectionModule { get; set; }

        public string ResearchMyAction { get; set; }
        public string ResearchPendingRequests { get; set; }
        public string ResearchMyTransactions { get; set; }
        public bool ResearchModule { get; set; }

        public string FilmSuitingMyAction { get; set; }
        public string FilmSuitingPendingRequests { get; set; }
        public string FilmSuitingMyTransactions { get; set; }
        public bool FilmSuitingModule { get; set; }

        public bool FinanceModule { get; set; }
        public USERDETAILS USERDETAILs { get; set; }

        public string CitizenTransitPermitPending { get; set; }
        public string CitizenTransitPermitApprove { get; set; }
        public string CitizenTransitPermitReject { get; set; }


    }

    public class USERDETAILS
    {
        public string Name { get; set; }
        public string Desig_Name { get; set; }
        public string Department { get; set; }

        public string OfficeName { get; set; }
        public string Roles { get; set; }
        public string Mobile { get; set; }

        public string Ssoid { get; set; }
        public string OldSingleRole { get; set; }


    }
    #region add by sunny for multiple office mapping
    public class USERDETAILS_MultipleOffice
    {
        public int SNO { get; set; }
        public long UserID { get; set; }
        public string Name { get; set; }

        public string EmailId { get; set; }
        public string Mobile { get; set; }

        public string Gender { get; set; }
        public string Aadhar_ID { get; set; }
        public string RoleId { get; set; }
        public string Desig_Name { get; set; }
        public string Ssoid { get; set; }
        public string OfficeName { get; set; }


    }
    #endregion
}
public static class Encryption
{

    public static string encrypt(string ToEncrypt)
    {
        try
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(ToEncrypt));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error" + ex);
            return null;
        }
    }

    public static string decrypt(string cypherString)
    {
        try
        {
            return System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(cypherString));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error" + ex);
            return null;
        }
    }


}

