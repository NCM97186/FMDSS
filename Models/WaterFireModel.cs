using FMDSS.Entity.ViewModel;
using FMDSS.Models.ForestFire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class WaterFireModelData
    {
        public WaterFireModelData()
        {
            Model = new WaterFireModel();
            List = new List<WaterFireModel>();
        }
        public WaterFireModel Model { get; set; }
        public List<WaterFireModel> List { get; set; }
    }

    public class WaterFireModel : DAL
    {
        public string RegNumber { get; set; }
        public string VendorType { get; set; }
        public string RepresentativeName { get; set; }

        public string VendorName { get; set; }
        public string SourceLatLong { get; set; }
        public string DestinationLatLong { get; set; }

        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        [Required]
        public string Division { get; set; }
        public string DivisionName { get; set; }

        public string CircleName { get; set; }
        [Required]
        public string RangeCode { get; set; }
        public string RangeName { get; set; }

        public string Naka { get; set; }
        public string NakaName { get; set; }
        public string BlockName { get; set; }
        [Required]
        public string StartLatitude { get; set; }
        [Required]
        public string StartLongitude { get; set; }
        public string EndLatitude { get; set; }
        public string EndLongitude { get; set; }
        [Required]
        public string Source_StartLatitude { get; set; }
        [Required]
        public string Source_StartLongitude { get; set; }
        public string Source_EndLatitude { get; set; }
        public string Source_EndLongitude { get; set; }

        public string WaterSourceImage { get; set; }
        public string WaterpointImage { get; set; }
        public string SSOID { get; set; }
        public string WaterPointImagePath { get; set; }
        public string WaterSourceImagePath { get; set; }
        public int Type1 { get; set; }
        public int Type2 { get; set; }
        [Required]
        public string Distance { get; set; }
        public int SNO { get; set; }
        public string EnteredOn { get; set; }
        public string Enteredby { get; set; }
        public int Type { get; set; }
        public string Image { get; set; }
        public string WaterPoint_LatLong { get; set; }
        public string WaterSource_LatLong { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public DataTable GetDivisionOnUser(string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "GetDivisionOnUser");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                //return dsResult;
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataTable GetDivisionOnUser(string SSOID, string ParentId)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "GetDivisionOnUser");
                cmd.Parameters.AddWithValue("@parentID", ParentId);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                //return dsResult;
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataTable SaveWaterResouceData(WaterFireModel model)
        {
            var attachedDoc = new List<CommonDocument>();
            DataTable dsResult = new DataTable();
            try
            {
                string SSOID = string.Empty;
                SSOID = model.SSOID;

                if (SSOID == null || SSOID == "")
                {
                    SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString());
                }

                string id = Convert.ToString(DateTime.Now.Ticks);
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "SaveWaterResouceData");
                cmd.Parameters.AddWithValue("@Water_Fire", "WaterResource");
                cmd.Parameters.AddWithValue("@Division", model.Division);
                cmd.Parameters.AddWithValue("@RangeCode", model.RangeCode);
                cmd.Parameters.AddWithValue("@Naka", model.Naka);
                cmd.Parameters.AddWithValue("@BlockName", model.BlockName);
                cmd.Parameters.AddWithValue("@StartLatitude", model.StartLatitude);
                cmd.Parameters.AddWithValue("@StartLongitude", model.StartLongitude);
                cmd.Parameters.AddWithValue("@EndLatitude", model.EndLatitude);
                cmd.Parameters.AddWithValue("@EndLongitude", model.EndLongitude);
                cmd.Parameters.AddWithValue("@Source_StartLatitude", model.Source_StartLatitude);
                cmd.Parameters.AddWithValue("@Source_StartLongitude", model.Source_StartLongitude);
                cmd.Parameters.AddWithValue("@Source_EndLatitude", model.Source_EndLatitude);
                cmd.Parameters.AddWithValue("@Source_EndLongitude", model.Source_EndLongitude);
                cmd.Parameters.AddWithValue("@Distance", model.Distance);
                cmd.Parameters.AddWithValue("@WaterPointImagePath", Convert.ToString(System.Web.HttpContext.Current.Session["WaterPointImagePath"]));
                cmd.Parameters.AddWithValue("@WaterSourceImagePath", Convert.ToString(System.Web.HttpContext.Current.Session["WaterSourceImagePath"]));
                cmd.Parameters.AddWithValue("@SSOID", SSOID);


                cmd.Parameters.AddWithValue("@SourceName", model.SourceName);
                cmd.Parameters.AddWithValue("@DestinationName", model.DestinationName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                //return dsResult;
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataSet GetWaterSourceList(string SSOID, string ActionName)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_WaterSource", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@SSOId", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public int SaveWaterSourceDetails(WaterSourceDetail model, string ActionName)
        {
            var attachedDoc = new List<CommonDocument>();
            DataTable dsResult = new DataTable();

            int i = 0;
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_WaterSource", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@SSOId", model.SSOID);
                cmd.Parameters.AddWithValue("@SourceLatLong", model.WaterSourceId);
                cmd.Parameters.AddWithValue("@DestinationLatLong", model.WaterDestinationId);
                cmd.Parameters.AddWithValue("@ImagePath", model.SourceImagePath);
                cmd.Parameters.AddWithValue("@DestinationImagePath", model.DestinationImagePath);
                cmd.CommandType = CommandType.StoredProcedure;

                 i = cmd.ExecuteNonQuery();               
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return i;
        }
        public DataTable SaveForestFireLineData(WaterFireModel model)
        {
            var attachedDoc = new List<CommonDocument>();
            DataTable dsResult = new DataTable();
            try
            {
                string id = Convert.ToString(DateTime.Now.Ticks);
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "SaveForestFireLineData");
                cmd.Parameters.AddWithValue("@Water_Fire", "ForestFireLine");
                cmd.Parameters.AddWithValue("@Division", model.Division);
                cmd.Parameters.AddWithValue("@RangeCode", model.RangeCode);
                cmd.Parameters.AddWithValue("@Naka", model.Naka);
                cmd.Parameters.AddWithValue("@BlockName", model.BlockName);
                cmd.Parameters.AddWithValue("@StartLatitude", model.StartLatitude);
                cmd.Parameters.AddWithValue("@StartLongitude", model.StartLongitude);
                cmd.Parameters.AddWithValue("@EndLatitude", model.EndLatitude);
                cmd.Parameters.AddWithValue("@EndLongitude", model.EndLongitude);
                cmd.Parameters.AddWithValue("@Source_StartLatitude", model.Source_StartLatitude);
                cmd.Parameters.AddWithValue("@Source_StartLongitude", model.Source_StartLongitude);
                cmd.Parameters.AddWithValue("@Source_EndLatitude", model.Source_EndLatitude);
                cmd.Parameters.AddWithValue("@Source_EndLongitude", model.Source_EndLongitude);
                cmd.Parameters.AddWithValue("@Distance", model.Distance);
                cmd.Parameters.AddWithValue("@WaterPointImagePath", Convert.ToString(System.Web.HttpContext.Current.Session["FFLinePointImagePath"]));
                cmd.Parameters.AddWithValue("@WaterSourceImagePath", Convert.ToString(System.Web.HttpContext.Current.Session["FFLineSourceImagePath"]));
                cmd.Parameters.AddWithValue("@SSOID", Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                //return dsResult;
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataTable GetRange(string parentID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "GetAppliedRange");
                cmd.Parameters.AddWithValue("@parentID", parentID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                //return dsResult;
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataTable GetDistrict(string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "GetDistrict");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataTable GetFireAlertCircle(string ActionType,string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GISboundaryWiseData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetCircle");
                cmd.Parameters.AddWithValue("@ActionType", ActionType);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataTable GetFireAlertDivision(string ActionType,string Circle_Code,string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GISboundaryWiseData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetDivision");
                cmd.Parameters.AddWithValue("@Circle_Code", Circle_Code);
                cmd.Parameters.AddWithValue("@ActionType", ActionType);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
               
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataTable GetFireAlertRange(string ActionType,string Division_Code, string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GISboundaryWiseData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetRange");
                cmd.Parameters.AddWithValue("@Division_Code", Division_Code);
                cmd.Parameters.AddWithValue("@ActionType", ActionType);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
               
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }


        public DataTable GetGISAlertData(string ActionType,string Circle_Code, string Division_Code,string Range_Code, string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GISboundaryWiseData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetGISAlertData");
                cmd.Parameters.AddWithValue("@ActionType", ActionType);
                cmd.Parameters.AddWithValue("@Circle_Code", Circle_Code);
                cmd.Parameters.AddWithValue("@Division_Code", Division_Code);
                cmd.Parameters.AddWithValue("@Range_Code", Range_Code);
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
               
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }



        public DataTable GetNaka(string parentID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "GetNaka");
                cmd.Parameters.AddWithValue("@parentID", parentID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataSet WaterResFFLineDetails()
        {
            DataSet dsResult = new DataSet();
            try
            {
                //string SSOID = string.Empty;
                //SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"]);

                //if (SSOID != null || SSOID != "")
                //{
                //    SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString());
                //}
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "WaterResFFLineDetails");
               
                //cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataSet WaterResFFLineDetails(string circleCode = "", string divisionCode = "", string rangeCode = "", string nakaCode = "")
        {
            DataSet dsResult = new DataSet();
            try
            {
                //string SSOID = string.Empty;
                //SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"]);

                //if (SSOID != null || SSOID != "")
                //{
                //    SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString());
                //}
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "WaterResFFLineDetails");
                cmd.Parameters.AddWithValue("@CircleCode", circleCode);
                cmd.Parameters.AddWithValue("@Division", divisionCode);
                cmd.Parameters.AddWithValue("@RangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@Naka", nakaCode);
                //cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataSet WaterResFFLineDetails(string WaterPoint_LatLong, string fromDate, string toDate)
        {
            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("RU-ru");
            DataSet dsResult = new DataSet();
            try
            {
                //string SSOID = string.Empty;
                //SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"]);

                //if (SSOID != null || SSOID != "")
                //{
                //    SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString());
                //}
                DALConn();
                DateTime now = DateTime.Now;
                string startDate = Convert.ToString(new DateTime(now.Year, now.Month, 1));
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "WaterResFFLineDetailsOnLatLong");
                cmd.Parameters.AddWithValue("@WaterPoint_LatLong", WaterPoint_LatLong);
                cmd.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(fromDate == null ? startDate : fromDate, cul));
                cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(toDate == null ? DateTime.Now.ToString() : toDate, cul));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        #region new Forest Fire Alert
        public string GetDistrict_User(string SSO_Id)
        {
            string dist = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 5);
                cmd.Parameters.AddWithValue("@SSO_Id", SSO_Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dist = Convert.ToString(dt.Rows[0]["District"]);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dist;
        }


        public DataTable GetSQHECTList()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetSqHectMaster", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }

        public DataTable GetDataFireAlertDistrictWise(string SSOID, string districtName, string CircleCode,string DivisionCode,string RangeCode, string action,int Month,int Year,string asOnDate,int PageSize,int PageStart)
        {
            DataTable dt = new DataTable();
            try
            {
                if (districtName == null)
                    districtName = "";
                //string dis = string.Empty;

                //if (districtName == "-1")
                //    dis = GetDistrict_User(SSOID);

                //else if (!string.IsNullOrEmpty(districtName))
                //    dis = districtName;
                //else
                //    dis = GetDistrict_User(SSOID);


                DateTime? FDate = null;
                if (!string.IsNullOrEmpty(asOnDate))
                {
                    FDate = Convert.ToDateTime(asOnDate);
                }

                //if (dis != "" || dis != null)
                //{
                    DALConn();
                    SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionCode", 6);
                    cmd.Parameters.AddWithValue("@District", districtName);
                    cmd.Parameters.AddWithValue("@Type", action);
                    cmd.Parameters.AddWithValue("@FromDate", FDate);
                    cmd.Parameters.AddWithValue("@Month", Month);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@PageNumber", PageStart); 
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
                    cmd.Parameters.AddWithValue("@DivisionCode", DivisionCode);
                    cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                    cmd.Parameters.AddWithValue("@SSO_Id", SSOID);                   
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                //}
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }


        public DataTable GetWildlifeSpecies()
        {
            DataTable dt = new DataTable();
            try
            { 
                    DALConn();
                    SqlCommand cmd = new SqlCommand("SP_Wildlife_Species", Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetSpecies");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
            }           
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }
        public DataSet GetAllFireStatusList()
        {
            DataSet dsData =new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_AllFireStatusList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);              
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsData;
        }
        public DataTable Checkauthenticity(string SSOId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Checkauthenticity");
                cmd.Parameters.AddWithValue("@SSOID", SSOId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }

        public DataTable Check(string RangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                var prms = new[]{
                            new SqlParameter("Range_Code", RangeCode),
                            new SqlParameter("Action",  "GetDetails")};
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(prms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }

        public DataTable GetWaterHoleVenderDetail(string RangeCode = "")
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Range_Code", RangeCode);
                cmd.Parameters.AddWithValue("@Action", "GetDetails");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }


        public DataTable GETVendorList(string ActionName = "", string UsedFor = "", long UserID = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@UsedFor", UsedFor);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }

        public DataTable SaveWaterHoleVenderDetails(WaterHoleVendor water)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "InsertVenderDetails");
                cmd.Parameters.AddWithValue("@NameofVendor", water.NameofVendor);
                cmd.Parameters.AddWithValue("@District", water.District);
                cmd.Parameters.AddWithValue("@PinCode", water.PinCode);
                cmd.Parameters.AddWithValue("@RepresentativeName", water.RepresentativeName);
                cmd.Parameters.AddWithValue("@MobileNumber", water.MobileNumber);
                cmd.Parameters.AddWithValue("@VendorSSOId", water.VendorSSOId);
                cmd.Parameters.AddWithValue("@PurposeforRegistration", water.PurposeforRegistration);
                cmd.Parameters.AddWithValue("@Circle_Code", water.Circle_Code);                
                cmd.Parameters.AddWithValue("@Division_Code", water.Division_Code);
                cmd.Parameters.AddWithValue("@Range_Code", water.Range_Code);
                cmd.Parameters.AddWithValue("@Village_Code", water.Village_Code);
                cmd.Parameters.AddWithValue("@WaterSource_Code", water.WaterSource_Code);
                cmd.Parameters.AddWithValue("@WaterHoles_Code", water.WaterHoles_Code);
                cmd.Parameters.AddWithValue("@InsertedBy", water.InsertedBy);
                cmd.Parameters.AddWithValue("@isActive", water.isActive);
                cmd.Parameters.AddWithValue("@UsedFor", water.UsedFor);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }

        public DataTable UpdateWaterHoleVenderDetails(WaterHoleVendor water)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "UPDATEVENDORDETAILS");
                cmd.Parameters.AddWithValue("@ObjectID", water.WaterHoleVendorDetailsID);//@REGNUMBERIN
                cmd.Parameters.AddWithValue("@REGNUMBERIN", water.RegNumber);
                cmd.Parameters.AddWithValue("@NameofVendor", water.NameofVendor);
                cmd.Parameters.AddWithValue("@District", water.District);
                cmd.Parameters.AddWithValue("@PinCode", water.PinCode);
                cmd.Parameters.AddWithValue("@RepresentativeName", water.RepresentativeName);
                cmd.Parameters.AddWithValue("@MobileNumber", water.MobileNumber);
                cmd.Parameters.AddWithValue("@VendorSSOId", water.VendorSSOId);
                cmd.Parameters.AddWithValue("@PurposeforRegistration", water.PurposeforRegistration);
                cmd.Parameters.AddWithValue("@Circle_Code", water.Circle_Code);
                cmd.Parameters.AddWithValue("@Division_Code", water.Division_Code);
                cmd.Parameters.AddWithValue("@Range_Code", water.Range_Code);
                cmd.Parameters.AddWithValue("@Village_Code", water.Village_Code);
                cmd.Parameters.AddWithValue("@WaterSource_Code", water.WaterSource_Code);
                cmd.Parameters.AddWithValue("@WaterHoles_Code", water.WaterHoles_Code);
                cmd.Parameters.AddWithValue("@InsertedBy", water.InsertedBy);
                cmd.Parameters.AddWithValue("@isActive", water.isActive);
                cmd.Parameters.AddWithValue("@UsedFor", water.UsedFor);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;
        }



        public DataTable SaveForestFireAlert(ForestFire_AddDetails model,string actionName,string controllerName,string host)
        {
            DataTable dsResult = new DataTable();
            try
            {
               
                #region Convert Model Into Datatable
                string JSONString = JsonConvert.SerializeObject(model.ImageForestFireImage);
                DataTable documentsListTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
                #endregion

                string SSOID = string.Empty;
                SSOID = model.SSOID;

                if (SSOID == null || SSOID == "")
                {
                    SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString());
                }
                string ImPath = string.Empty;
                string ImPathAfter = string.Empty;
                if (System.Web.HttpContext.Current.Session["FFimage"] == null)
                {
                    ImPath = "";
                    if (model.ImageForestFireImage != null)
                    {
                        foreach (DocumentList doc in model.ImageForestFireImage)
                        {
                            if (doc.FilePath.IndexOf(host) != -1)
                            {
                                if (ImPath.Length == 0)
                                    ImPath = doc.FilePath;
                                else
                                    ImPath = ImPath + "," + doc.FilePath;
                            }
                            else
                            {
                                if (ImPath.Length == 0)
                                    ImPath = host + doc.FilePath;
                                else
                                    ImPath = ImPath + "," + host + doc.FilePath;
                            }
                        }
                    }
                }
                else
                {
                    ImPath = "";
                    //ImPath = Convert.ToString(System.Web.HttpContext.Current.Session["FFimage"]);
                    if (model.ImageForestFireImage != null)
                    {
                        foreach (DocumentList doc in model.ImageForestFireImage)
                        {
                            if (doc.FilePath.IndexOf(host) != -1)
                            {
                                if (ImPath.Length == 0)
                                    ImPath = doc.FilePath;
                                else
                                    ImPath = ImPath + "," + doc.FilePath;
                            }
                            else
                            {
                                if (ImPath.Length == 0)
                                    ImPath = host + doc.FilePath;
                                else
                                    ImPath = ImPath + "," + host + doc.FilePath;
                            }
                        }
                    }
                }

                if (System.Web.HttpContext.Current.Session["FFimage"] == null)
                {
                    ImPathAfter = "";
                    if (model.ImageForestFireImageAfter != null)
                    {
                        foreach (DocumentList doc in model.ImageForestFireImageAfter)
                        {
                            if (doc.FilePath.IndexOf(host) != -1)
                            {
                                if (ImPathAfter.Length == 0)
                                    ImPathAfter = doc.FilePath;
                                else
                                    ImPathAfter = ImPathAfter + "," + doc.FilePath;
                            }
                            else
                            {
                                if (ImPathAfter.Length == 0)
                                    ImPathAfter = host + doc.FilePath;
                                else
                                    ImPathAfter = ImPathAfter + "," + host + doc.FilePath;
                            }

                        }
                    }
                }
                else
                {
                    ImPathAfter = "";
                    if (model.ImageForestFireImageAfter != null)
                    {
                        foreach (DocumentList doc in model.ImageForestFireImageAfter)
                        {
                            if (doc.FilePath.IndexOf(host) != -1)
                            {
                                if (ImPathAfter.Length == 0)
                                    ImPathAfter = doc.FilePath;
                                else
                                    ImPathAfter = ImPathAfter + "," + doc.FilePath;
                            }
                            else
                            {
                                if (ImPathAfter.Length == 0)
                                    ImPathAfter = host + doc.FilePath;
                                else
                                    ImPathAfter = ImPathAfter + "," + host + doc.FilePath;
                            }
                        }
                    }                   
                }
               
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_ForestFireAddData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@TotalAreaAffected", model.TotalAreaAffected);
                cmd.Parameters.AddWithValue("@QU", model.QuantificationOfLoss);
                cmd.Parameters.AddWithValue("@CF", model.CauseofFire);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                cmd.Parameters.AddWithValue("@ForestProduceLoss", model.ForestProduceLoss);
                cmd.Parameters.AddWithValue("@WildlifeLoss", model.WildlifeLoss);
                cmd.Parameters.AddWithValue("@AnyotherLoss", model.AnyotherLoss);
                cmd.Parameters.AddWithValue("@NoPepoleInvolved", model.NoPepoleInvolved);
                cmd.Parameters.AddWithValue("@OtherDeptHelped", model.OtherDeptHelped);
                cmd.Parameters.AddWithValue("@Fire_Time", model.Fire_Time);
                cmd.Parameters.AddWithValue("@FireDateTime", model.FireDateTime);
                cmd.Parameters.AddWithValue("@PuttOffTime", model.PuttOffTime);
                cmd.Parameters.AddWithValue("@SSO_ID", SSOID);
                cmd.Parameters.AddWithValue("@ImagePath", ImPath);
                cmd.Parameters.AddWithValue("@SpeciesList", model.SpeciesList);
                cmd.Parameters.AddWithValue("@Documents", documentsListTable);
                ////Below Code is Added on 17-03-2020
                cmd.Parameters.AddWithValue("@FireActionStatusId", model.FireActionStatusId);
                cmd.Parameters.AddWithValue("@MonetaryLoss", model.MonetaryLoss);
                cmd.Parameters.AddWithValue("@CurrentDate", (model.CurrentDate.ToString() == "01/01/0001 00:00:00" ? DateTime.Now.Date : model.CurrentDate));
                //cmd.Parameters.AddWithValue("@CurrentDate", model.CurrentDate);
                cmd.Parameters.AddWithValue("@CurrentTime", model.CurrentTime);
                cmd.Parameters.AddWithValue("@CurrentLat", model.CurrentLat);
                cmd.Parameters.AddWithValue("@CurrentLong", model.CurrentLong);
                cmd.Parameters.AddWithValue("@Latitute", model.Latitude);
                cmd.Parameters.AddWithValue("@Longitute", model.Longitude);
                cmd.Parameters.AddWithValue("@CauseOfFireId", model.CauseOfFireId);
                cmd.Parameters.AddWithValue("@FireIncidentAreaId", model.FireIncidentAreaId);
                cmd.Parameters.AddWithValue("@CircleCode", model.CircleCode);
                cmd.Parameters.AddWithValue("@DivisionCode", model.DivisionCode);
                cmd.Parameters.AddWithValue("@RangeCode", model.RangeCode);
                cmd.Parameters.AddWithValue("@NakaId", model.NakaId);
                cmd.Parameters.AddWithValue("@User_Id", model.User_Id);
                cmd.Parameters.AddWithValue("@Source", model.Source);
                cmd.Parameters.AddWithValue("@StateName", model.StateName);
                cmd.Parameters.AddWithValue("@DistrictName", model.DistrictName);
                cmd.Parameters.AddWithValue("@Block_Name", model.Block_Name);
                cmd.Parameters.AddWithValue("@Beat", model.Beat);
                cmd.Parameters.AddWithValue("@ResponseInitTime", model.ResponseInitTime);
                cmd.Parameters.AddWithValue("@PutOffDate", model.PutOffDate);
                cmd.Parameters.AddWithValue("@PutOffDate1", model.PutOffDate1);// change for saunesh 
                cmd.Parameters.AddWithValue("@ImagePathAfter", ImPathAfter);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                da.Dispose();
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataTable SaveForestFireAlertNew(ForestFire_AddDetails model)
        {
            DataTable dsResult = new DataTable();
            try
            {
                #region Convert Model Into Datatable
                string JSONString = JsonConvert.SerializeObject(model.ImageForestFireImage);
                DataTable documentsListTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
                #endregion

                string SSOID = string.Empty;
                SSOID = model.SSOID;

                if (SSOID == null || SSOID == "")
                {
                    SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"].ToString());
                }
                string ImPath = string.Empty;
                if (System.Web.HttpContext.Current.Session["FFimage"] == null)
                {
                    ImPath = "";
                    foreach (DocumentList doc in model.ImageForestFireImage)
                    {
                        if (ImPath.Length == 0)
                            ImPath = doc.FilePath;
                        else
                            ImPath = ImPath + "|" + doc.FilePath;
                    }
                }
                else
                {
                    ImPath = Convert.ToString(System.Web.HttpContext.Current.Session["FFimage"]);
                }

                DALConn();
                SqlCommand cmd = new SqlCommand("SP_ForestFireAddData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@ID", model.ID);
                cmd.Parameters.AddWithValue("@TotalAreaAffected", model.TotalAreaAffected);
                cmd.Parameters.AddWithValue("@QU", model.QuantificationOfLoss);
                cmd.Parameters.AddWithValue("@CF", model.CauseofFire);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                cmd.Parameters.AddWithValue("@ForestProduceLoss", model.ForestProduceLoss);
                cmd.Parameters.AddWithValue("@WildlifeLoss", model.WildlifeLoss);
                cmd.Parameters.AddWithValue("@AnyotherLoss", model.AnyotherLoss);
                cmd.Parameters.AddWithValue("@NoPepoleInvolved", model.NoPepoleInvolved);
                cmd.Parameters.AddWithValue("@OtherDeptHelped", model.OtherDeptHelped);
                cmd.Parameters.AddWithValue("@Fire_Time", model.Fire_Time);
                cmd.Parameters.AddWithValue("@FireDateTime", model.FireDateTime);
                cmd.Parameters.AddWithValue("@PuttOffTime", model.PuttOffTime);
                cmd.Parameters.AddWithValue("@SSO_ID", SSOID);
                cmd.Parameters.AddWithValue("@ImagePath", ImPath);
                cmd.Parameters.AddWithValue("@SpeciesList", model.SpeciesList);
                cmd.Parameters.AddWithValue("@Documents", documentsListTable);
                ////Below Code is Added on 17-03-2020
                cmd.Parameters.AddWithValue("@FireActionStatusId", model.FireActionStatusId);
                cmd.Parameters.AddWithValue("@MonetaryLoss", model.MonetaryLoss);
                cmd.Parameters.AddWithValue("@CurrentDate", model.CurrentDate);
                cmd.Parameters.AddWithValue("@CurrentTime", model.CurrentTime);
                cmd.Parameters.AddWithValue("@CurrentLat", model.CurrentLat);
                cmd.Parameters.AddWithValue("@CurrentLong", model.CurrentLong);
                cmd.Parameters.AddWithValue("@CauseOfFireId", model.CauseOfFireId);
                cmd.Parameters.AddWithValue("@FireIncidentAreaId", model.FireIncidentAreaId);
                cmd.Parameters.AddWithValue("@CircleCode", model.CircleCode);
                cmd.Parameters.AddWithValue("@DivisionCode", model.DivisionCode);
                cmd.Parameters.AddWithValue("@RangeCode", model.RangeCode);
                cmd.Parameters.AddWithValue("@NakaId", model.NakaId);
                cmd.Parameters.AddWithValue("@User_Id", model.User_Id);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataTable GetCircel(string SSOID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("WaterResourceFireline", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", "GetCircelCode");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }

        public DataTable GetCircelAll()
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_CirclesAll", Conn);
                cmd.CommandType = CommandType.StoredProcedure; 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);

            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }


        public DataTable GetMasterDataListCircleDivisionRangeWise(string Action,string ChildAction,string CircleCode, string DivisionCode, string RangeCode, string NakaID, string DistictCode, long UserID, int FYearId)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetCircleDivisionRangeCountsOffenceFireAlertMasters", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ChildAction", ChildAction);
                cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@DivisionCode", DivisionCode);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@NakaID", NakaID);
                cmd.Parameters.AddWithValue("@DistictCode", DistictCode);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@FYearId", FYearId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                
            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dsResult;
        }
        public DataTable GetWaterRefillList(string Action, string sourceLatLong, string destinationLatLong)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_WaterSource", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@SourceLatLong", sourceLatLong);
                cmd.Parameters.AddWithValue("@DestinationLatLong", destinationLatLong);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;

        }
        public DataTable GetFireAlertList(string Action, string District,int Month=0,int Year=0, long UserID=0)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spFireAlertsCount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionName", Action);
                cmd.Parameters.AddWithValue("@DistrictName", District);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year); 
                cmd.Parameters.AddWithValue("@UserID", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                // Added On 21-04-2020 By Mukesh Kumar Jangid
                //new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, model.User_Id);
                Console.WriteLine(ex.Message.ToString());
                throw ex;
            }
            finally // Added On 21-04-2020 By Mukesh Kumar Jangid
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
            }
            return dt;

        }

        #endregion
    }
    public class WaterSourceDetail
    {
        public string WaterSourceId { get; set; }
        public string WaterDestinationId { get; set; }
        public string SourceImagePath { get; set; }
        public string SourceLatLong { get; set; }
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public string DestinationLatLong { get; set; }
        public string DestinationImagePath { get; set; }
        public string SSOID { get; set; }
        public string Date { get; set; }

    }

}