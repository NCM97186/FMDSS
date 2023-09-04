using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;


namespace FMDSS.Models.Master
{
    public class MasterData:DAL
    {
        public DataTable GetImportExportTables()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select * from tbl_mst_ImportExport", Conn);
                cmd.CommandType = CommandType.Text;
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
        public DataTable GetTableID(string tablename)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Export_Tables", Conn);
                //cmd.Parameters.AddWithValue("@Action", "Export");
                cmd.Parameters.AddWithValue("@Tablename", tablename);
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

        public DataTable GetExportData(string tableID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_ExportMasterData", Conn);
                cmd.Parameters.AddWithValue("@Action", "Export");
                cmd.Parameters.AddWithValue("@TableID", tableID);
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
        //remove
      /*  public DataTable TruncateMasterData(string tableID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_ExportMasterData", Conn);
                cmd.Parameters.AddWithValue("@Action", "Truncate");
                cmd.Parameters.AddWithValue("@TableID", tableID);
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

        }*/
        public DataTable getcount(string Tablename)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("select count(*) from "+ Tablename, Conn);
                cmd.CommandType = CommandType.Text;
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
        public void updatecount(string Tableid, string count)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("update tbl_mst_ImportExport set CurrentRecordCount = " + count + " where ID=" + Tableid, Conn);
                cmd.CommandType = CommandType.Text;
                if (Conn.State == ConnectionState.Closed)
                    Conn.Open();
                cmd.ExecuteScalar();
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
        public void ImportMasterData(string tableID,DataTable dt)
        {
            try
            {
                string parameter=string.Empty;
                  SqlCommand cmd = new SqlCommand("Select ImportQuery,ImportParameters from tbl_mst_ImportExport where ID="+tableID, Conn);
                cmd.CommandType = CommandType.Text;
                   SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtQuery =new DataTable();
                da.Fill(dtQuery);

                string ImportQuery = dtQuery.Rows[0]["ImportQuery"].ToString();

                string[] strparameter = dtQuery.Rows[0]["ImportParameters"].ToString().Split(',');

                foreach(DataRow dr in dt.Rows)
                { 
                    cmd = new SqlCommand(ImportQuery, Conn); 
                    cmd.CommandType = CommandType.Text;
                   for(int i=0;i<strparameter.Length;i++)
                   {
                       cmd.Parameters.AddWithValue("@"+strparameter[i].Trim(), dr[strparameter[i].Trim()]);
                   }
                   if (Conn.State == ConnectionState.Closed)
                       Conn.Open();
               
                    cmd.ExecuteNonQuery();

            }


                cmd = new SqlCommand("Update tbl_mst_ImportExport set LastImportOn=GETDATE() where ID=" + tableID, Conn);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteScalar();
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

    public class AllEmailModule
    {
        public string ID { get; set; }
        [Required]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        [Required]
        [Display(Name = "Email ID")]
        public string EmailID { get; set; }
        [Required]
        [Display(Name = "SSO Id")]
        public string SSOId { get; set; }


        public string AdminTemplate { get; set; }

        public string CitizenTemplate { get; set; }
        public bool Status { get; set; }

        public string AdminTemplateSMS { get; set; }

        public string CitizenTemplateSMS { get; set; }
        [Required]
        [Display(Name = "Admin Mobile Number")]
        public string AdminMobileNumber { get; set; }

        public bool IsSendMailStatusCitizen { get; set; }
    }


    public class AllEmailModuleDetails
    {
        public AllEmailModuleDetails()
        {
            AddEmailmodel = new AllEmailModule();
            AddEmailList = new List<AllEmailModule>();
        }
        public AllEmailModule AddEmailmodel { get; set; }
        public List<AllEmailModule> AddEmailList { get; set; }
    }


    public class AllEmailModuleRepository : DAL
    {
        public DataSet GetAllEmailList(string Action, AllEmailModule model)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetALLEmailModulesListDetails", Conn);
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ModuleName", model.ModuleName);
                cmd.Parameters.AddWithValue("@EmailID", model.EmailID);
                cmd.Parameters.AddWithValue("@SSOId", model.SSOId);
                cmd.Parameters.AddWithValue("@AdminTemplate", model.AdminTemplate);
                cmd.Parameters.AddWithValue("@CitizenTemplate", model.CitizenTemplate);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@AdminTemplateSMS", model.AdminTemplateSMS);
                cmd.Parameters.AddWithValue("@CitizenTemplateSMS", model.CitizenTemplateSMS);
                cmd.Parameters.AddWithValue("@AdminMobileNumber", model.AdminMobileNumber);
                cmd.Parameters.AddWithValue("@IsSendMailStatusCitizen", model.IsSendMailStatusCitizen);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
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

    public class GISInformationRepo : DAL
    {
        public DataSet InsertGISInformation(DataTable GISTable, string RequestID, long userId)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_COMMON_INSERTGIS", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RequestedID", RequestID);
                cmd.Parameters.AddWithValue("@GISModelList", GISTable);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "function name:- InsertGISInformation" + "_" + "Class Name:- EnchorochmentView", 0, DateTime.Now, userId);
            }

            return dt;
        }
    }

    #region Online Booking Pop Up by Rajveer
    public class OnlineBookingPopUp
    {
        public string ID { get; set; }
        [Required]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        [Required]
        public string Content { get; set; }

        public string Title { get; set; }
        public bool Status { get; set; }
    }

    public class OnlineBookingPopUpDetails
    {
        public OnlineBookingPopUpDetails()
        {
            model = new OnlineBookingPopUp();
            ModelList = new List<OnlineBookingPopUp>();
        }
        public OnlineBookingPopUp model { get; set; }
        public List<OnlineBookingPopUp> ModelList { get; set; }
    }

    public class OnlineBookingPopUpRepository : DAL
    {
        public DataSet GetAllOnlineBookingPopUpList(string Action, OnlineBookingPopUp model)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ModuleName", model.ModuleName);
                cmd.Parameters.AddWithValue("@Content", model.Content);
                cmd.Parameters.AddWithValue("@Title", model.Title);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
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
        public DataTable CheckBookingId(string requestId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);

                cmd.Parameters.AddWithValue("@Action", "CheckRequestId");
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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

        public List<Entity.ViewModel.cls_FraDetails> GetFraDetails(string FraRequestId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FRA_GetClaimRequestByReceptNumber", Conn);
                cmd.Parameters.AddWithValue("@FraRequestId", FraRequestId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);

                List<Entity.ViewModel.cls_FraDetails> oList = new List<Entity.ViewModel.cls_FraDetails>();

                if (Globals.Util.isValidDataSet(dt, 0))
                {
                    oList = Globals.Util.GetListFromTable<Entity.ViewModel.cls_FraDetails>(dt, 0);
                }
                return oList;
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


        public DataTable CheckZooBookingId(string requestId, int ZooSectionId, long UserId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);

                cmd.Parameters.AddWithValue("@Action", "CheckZooRequestId");
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@ZooSectionId", ZooSectionId);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
        public DataTable CheckZooRequestIdForOut(string requestId, int ZooSectionId, long UserId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);

                cmd.Parameters.AddWithValue("@Action", "CheckZooRequestIdForOut");
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@ZooSectionId", ZooSectionId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
        public DataTable CheckZooRequestIdByMobileForEnter(string MobileNo, int ZooSectionId, int PlaceId, long UserId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);

                cmd.Parameters.AddWithValue("@Action", "CheckZooRequestIdByMobileForEnter");
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@ZooSectionId", ZooSectionId);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
        public DataTable CheckZooRequestIdByMobileForOut(string MobileNo, int ZooSectionId, int PlaceId, long UserId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);

                cmd.Parameters.AddWithValue("@Action", "CheckZooRequestIdByMobileForOut");
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@ZooSectionId", ZooSectionId);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
        public DataTable GetZooSectionList(int PlaceId)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetZooSections");
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
        public DataSet GetZooTicketDetailByRequestId(string RequestId, int PlaceId, long UserId)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_ZooTicketDetail", Conn);
                cmd.Parameters.AddWithValue("@Action", "TicketDetailByRequestId");
                cmd.Parameters.AddWithValue("@RequestId", RequestId);               
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
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
        public DataSet GetZooTicketDetailByMobileNo(string MobileNo, int PlaceId, long UserId)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_ZooTicketDetail", Conn);
                cmd.Parameters.AddWithValue("@Action", "TicketDetailByMobileNo");
                cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);               
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
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
        public DataTable GetZooTicketDashboardDetail(int PlaceId, string DateOfVisit, long UserId)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_ZooTicketDetail", Conn);
                cmd.Parameters.AddWithValue("@Action", "TicketDashboardDetail");           
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@VisitDate", DateOfVisit);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
        
        public DataTable GetMobileVersion()
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetMobileVersion");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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

        public DataTable GetMobileVersion(string ApkName)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingPopUp", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetMobileVersionAppWise");
                cmd.Parameters.AddWithValue("@ApkName", ApkName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
                da.Fill(dt);
                return dt.Tables[0];
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
    #endregion


    #region Emitra Response Kiosk User
    public class EmitraResponseKioskUser
    {
        public string RequestID { get; set; }
        public string UserID { get; set; }
        public string LogFileData { get; set; }
        public string EnteredOn { get; set; }
        public string Ssoid { get; set; }
        public string Name { get; set; }
    }

    public class EmitraResponseKioskUserDetails : EmitraResponseKioskUser
    {
        public EmitraResponseKioskUserDetails()
        {
            EmitraResponseList = new List<EmitraResponseKioskUser>();
        }

        public List<EmitraResponseKioskUser> EmitraResponseList { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string UserID { get; set; }
    }

    public class EmitraResponseKioskUserRepository : DAL
    {
        public DataSet GetEmitraResponseKioskUserList(string Action, EmitraResponseKioskUserDetails model)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_EmitraTransactionLogsKioskUser", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@FromDate", model.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", model.ToDate);
                cmd.Parameters.AddWithValue("@UserId", model.UserID);
                cmd.Parameters.AddWithValue("@RequestID", model.RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
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
    #endregion


    #region  Restrict Online Zoo Booking Days
    public class RestrictOnlineZooBooking
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string DayID { get; set; }
        public bool IsActive { get; set; }

        public string DayNames { get; set; }
    }

    public class RestrictOnlineZooBookingDetails
    {
        public RestrictOnlineZooBookingDetails()
        {
            RestrictOnlineZooBookingModelList = new List<RestrictOnlineZooBooking>();
            ModelData = new RestrictOnlineZooBooking();
        }
        public List<RestrictOnlineZooBooking> RestrictOnlineZooBookingModelList { get; set; }
        public RestrictOnlineZooBooking ModelData { get; set; }
    }

    public class RestrictOnlineZooBookingRepository : DAL
    {
        public DataSet RestrictOnlineZooBookingList(string Action, RestrictOnlineZooBooking model)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_RestictZooPlaceWiseHolidaysDays", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@PlaceID", model.PlaceId);
                cmd.Parameters.AddWithValue("@DayID", model.DayID);
                cmd.Parameters.AddWithValue("@IsActivity", model.IsActive);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dt = new DataSet();
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
    #endregion
}