using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace FMDSS.Models.MIS
{
    public class MISCitizenModel : DAL
    {
        public int index { get; set; }
        public string Duration { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }
        public string BLKCode { get; set; }
        public string PermissionType { get; set; }
        public string SubPermissionType { get; set; }
        public string Total { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ApplicationNo { get; set; }
        public string Remark { get; set; }
        public string StatusName { get; set; }
        public string CategoryStatus { get; set; }
        public string AreaCategory { get; set; }
        public string PlaceForResearch { get; set; }
        public string ResearchType { get; set; }
        public string RequestedId { get; set; }
        public string RequestedType { get; set; }
        public string ApplicationDate { get; set; }
        public string IsGORGOI { get; set; }
        public DataTable GetCSDivisionWise(string FromDate, string ToDate, string Circle, string Division, string Range, string PermissionType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_ReportCitizenService_DivisionWise", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", Circle);
                cmd.Parameters.AddWithValue("@DIVISON_CODE", Division);
                cmd.Parameters.AddWithValue("@RANGE_CODE", Range);
                cmd.Parameters.AddWithValue("@ServicesTATUS", PermissionType);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
        public DataTable GetCSRevenue(string FromDate, string ToDate, string Circle, string Division, string Block)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_ReportCitizenService_RevenueGeneration", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", Circle);
                cmd.Parameters.AddWithValue("@DIVISON_CODE", Division);
                cmd.Parameters.AddWithValue("@BLK_CODE", Block);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
        public DataSet CSPermissionsDrillDown(string ProcName, string FromDate, string ToDate, string Circle, string Division, string Block, string ServiceCategory)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand(ProcName, Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", Circle);
                cmd.Parameters.AddWithValue("@DIVISON_CODE", Division);
                cmd.Parameters.AddWithValue("@BLK_CODE", Block);
                cmd.Parameters.AddWithValue("@ServiceCategory", ServiceCategory);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
        public DataSet CSPermissionsGetData(string ProcName, string FromDate, string ToDate, string ResearchType, string AreaCategory, string PlaceForResearch, string StatusName, string Action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand(ProcName, Conn);

                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", ToDate);
                cmd.Parameters.AddWithValue("@ResearchType", ResearchType == "--Select--" ? null : ResearchType);
                cmd.Parameters.AddWithValue("@AreaCategory", AreaCategory);
                cmd.Parameters.AddWithValue("@PlaceForResearch", PlaceForResearch);
                cmd.Parameters.AddWithValue("@Status", StatusName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
        public DataSet GetApplicationDetails(string ProcName, string RequestID, string Status)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand(ProcName, Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
        public DataTable GetDownLoadList(string ProcName, string RequestIDs)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(ProcName, Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestIDs);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
    public class ApplicationDetails
    {
        public int index { get; set; }
        public string NOCsPermissionName { get; set; }
        public string ApplicationNo { get; set; }
        public string RequestedOn { get; set; }
        public string RequestedBy { get; set; }
        public string DurationFromDate { get; set; }
        public string DurationToDate { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string Block { get; set; }
        public string GramPanchayat { get; set; }
        public string Village { get; set; }
        public string IsPaymentDone { get; set; }
        public string PaidAmount { get; set; }
        public string Approver { get; set; }
        public string ApproverRemarks { get; set; }
        public string Reviwer { get; set; }
        public string ReviwerRemarks { get; set; }
    }
    public class CitizenTransitPermit
    {
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string Status { get; set; }
        public string Counts { get; set; }
        public string Fees { get; set; }
    }
    public class CitizenTransitPermitModel
    {
        public CitizenTransitPermitModel()
        {
            CitizenListTotal = new List<CitizenTransitPermit>();
            CitizenListPending = new List<CitizenTransitPermit>();
            CitizenListReviwed = new List<CitizenTransitPermit>();
            CitizenListApproved = new List<CitizenTransitPermit>();
            CitizenListReject = new List<CitizenTransitPermit>();
        }
        public string Division { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<CitizenTransitPermit> CitizenListTotal { get; set; }
        public List<CitizenTransitPermit> CitizenListPending { get; set; }
        public List<CitizenTransitPermit> CitizenListReviwed { get; set; }
        public List<CitizenTransitPermit> CitizenListApproved { get; set; }
        public List<CitizenTransitPermit> CitizenListReject { get; set; }
    }
    public class CitizenTransitPermitRepo : DAL
    {
        public CitizenTransitPermitModel GetCitizenReports(CitizenTransitPermitModel model)
        {
            DataSet DS = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_CTP_CitizenReport", Conn);
                cmd.Parameters.AddWithValue("@Action", "Details");
                cmd.Parameters.AddWithValue("@DIVISON_CODE", string.IsNullOrEmpty(model.Division) ? "ALL" : model.Division);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", model.FromDate);
                cmd.Parameters.AddWithValue("@DATETIME_TO", model.ToDate);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                #region Bind Data in Model
                if (DS != null && DS.Tables.Count > 0)
                {
                    string str = string.Empty;
                    if (DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[0]);
                        model.CitizenListTotal = new List<CitizenTransitPermit>();
                        model.CitizenListTotal = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CitizenTransitPermit>>(str);
                    }
                    if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        model.CitizenListPending = new List<CitizenTransitPermit>();
                        model.CitizenListPending = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CitizenTransitPermit>>(str);
                    }
                    if (DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[2]);
                        model.CitizenListReviwed = new List<CitizenTransitPermit>();
                        model.CitizenListReviwed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CitizenTransitPermit>>(str);
                    }
                    if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[3]);
                        model.CitizenListApproved = new List<CitizenTransitPermit>();
                        model.CitizenListApproved = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CitizenTransitPermit>>(str);
                    }
                    if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[4]);
                        model.CitizenListReject = new List<CitizenTransitPermit>();
                        model.CitizenListReject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CitizenTransitPermit>>(str);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetCitizenReports" + "_" + "MISCitizenModel", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return model;
        }
        public List<SelectListItem> getDropdown(string option)
        {
            try
            {
                DataTable dtp = Fill_Dropdown(option, string.Empty, 0);
                if (option == "0")
                {
                    List<SelectListItem> blnklist = new List<SelectListItem>();
                    blnklist.Add(new SelectListItem { Text = "--Select--", Value = "" });
                    return blnklist;
                }
                if (option == "1" || option == "13")
                {
                    List<SelectListItem> lstDivision = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstDivision.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    return lstDivision;
                }
                else if (option == "3")
                {
                    List<SelectListItem> lstState = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstState.Add(new SelectListItem { Text = @dr["STATENAME"].ToString(), Value = @dr["STATEID"].ToString() });
                    }
                    return lstState;
                }
                else if (option == "4")
                {
                    List<SelectListItem> lstDistrict = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstDistrict.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }
                    return lstDistrict;
                }
                else if (option == "5")
                {
                    List<SelectListItem> lstVehicle = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["VehicleType"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    return lstVehicle;
                }
                else if (option == "10")
                {
                    List<SelectListItem> lstVehicle = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dtp.Rows)
                    {
                        lstVehicle.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    return lstVehicle;
                }
                else
                {
                    List<SelectListItem> lstVehicle = new List<SelectListItem>();
                    return lstVehicle;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
        public DataTable Fill_Dropdown(string Option, string DIVISION_OFFICE, int VEHICLE_TYPE = 0)
        {
            DataTable dtdropdown = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_SelectTpDDLList", Conn);
                cmd.Parameters.AddWithValue("@DIV_CODE", DIVISION_OFFICE);
                cmd.Parameters.AddWithValue("@STATE_CODE", "");
                cmd.Parameters.AddWithValue("@VEHICLE_ID", Convert.ToInt32(VEHICLE_TYPE));
                cmd.Parameters.AddWithValue("@Option", Option);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserID"]));
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
    }
    public class AmritaDeviModel
    {
        public string RequestId { get; set; }
        public string Name { get; set; }
        public string statusdesc { get; set; }
        public string CategoryName { get; set; }
        public string PersonalLandDETAIL { get; set; }
        public string RevenueLandDETAIL { get; set; }
        public string RevenueLandHectare { get; set; }
    }
    public class AmritaDeviReportModel
    {
        public AmritaDeviReportModel()
        {
            ApprovedDCFmodel = new List<AmritaDeviModel>();
            PendingDCFmodel = new List<AmritaDeviModel>();
            ApprovedStatemodel = new List<AmritaDeviModel>();
            ApprovedTechnicalmodel = new List<AmritaDeviModel>();
            RejectDCFmodel = new List<AmritaDeviModel>();
        }
        public List<AmritaDeviModel> ApprovedDCFmodel { get; set; }
        public List<AmritaDeviModel> PendingDCFmodel { get; set; }
        public List<AmritaDeviModel> ApprovedStatemodel { get; set; }
        public List<AmritaDeviModel> ApprovedTechnicalmodel { get; set; }
        public List<AmritaDeviModel> RejectDCFmodel { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string userid { get; set; }
    }
    public class AmritaDeviReportRepo : DAL
    {
        public AmritaDeviReportModel GetAmritaDeviReports(AmritaDeviReportModel model)
        {
            DataSet DS = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.Parameters.AddWithValue("@Action", "REPORT");
                cmd.Parameters.AddWithValue("@RequestID", "");
                cmd.Parameters.AddWithValue("@UserID", model.userid);
                cmd.Parameters.AddWithValue("@AssignTo", "");
                cmd.Parameters.AddWithValue("@ActionStatus", "");
                cmd.Parameters.AddWithValue("@Comment", model.ToDate);
                cmd.Parameters.AddWithValue("@Reasons", model.FromDate);
                cmd.Parameters.AddWithValue("@SSOID", "");
                cmd.Parameters.AddWithValue("@ReviewApprovalDocument", "");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                #region Bind Data in Model
                if (DS != null && DS.Tables.Count > 0)
                {
                    string str = string.Empty;
                    if (DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[0]);
                        model.ApprovedStatemodel = new List<AmritaDeviModel>();
                        model.ApprovedStatemodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmritaDeviModel>>(str);
                    }
                    if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        model.ApprovedTechnicalmodel = new List<AmritaDeviModel>();
                        model.ApprovedTechnicalmodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmritaDeviModel>>(str);
                    }
                    if (DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[2]);
                        model.ApprovedDCFmodel = new List<AmritaDeviModel>();
                        model.ApprovedDCFmodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmritaDeviModel>>(str);
                    }
                    if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[3]);
                        model.PendingDCFmodel = new List<AmritaDeviModel>();
                        model.PendingDCFmodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmritaDeviModel>>(str);
                    }
                    if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
                    {
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[4]);
                        model.RejectDCFmodel = new List<AmritaDeviModel>();
                        model.RejectDCFmodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmritaDeviModel>>(str);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAmritaDeviReports" + "_" + "AmritaDeviReportRepo", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return model;
        }
    }
}