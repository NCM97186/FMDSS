using FMDSS.App_Start;
using FMDSS.Models.SWCSModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.CitizenService.PermissionServices
{

    [Serializable]
    public class PlantFixedPermission : BaseModelSerializable
    {
        public Int64 PlantID { get; set; }
        public string RequestedID { get; set; }
        public string PlantName { get; set; }
        public string PlantCount { get; set; }

    }
    [Serializable]
    public class clsPermission : BaseModelSerializable
    {
        public string FOREST_DIVCODE { get; set; }
        public string ID { get; set; }
        public string DIV_CODE { get; set; }

        public string DIST_CODE { get; set; }
        public string BLK_CODE { get; set; }
        public string GP_CODE { get; set; }
        public string VILL_CODE { get; set; }
        public string KhasraNo { get; set; }
        public string Area { get; set; }
        public string areaName { get; set; }
        public string kioskuserid { get; set; }
        //public string RequestedId { get; set; }

        public string RequestedID { get; set; }

        public string Div_Cd { get; set; }
        public string Dist_Cd { get; set; }
        public string Blk_Cd { get; set; }
        public string Gp_Cd { get; set; }
        public string Vlg_Cd { get; set; }
        public string Div_NM { get; set; }
        public string Dist_NM { get; set; }
        public string Block_NM { get; set; }
        public string Gp_NM { get; set; }
        public string Village_NM { get; set; }
        public string Tehsil_Cd { get; set; }
        public string Tehsil_NM { get; set; }

    }

     [Serializable]
    public class FixedLandUsage : DAL
    {
        public FixedLandUsage()
        {
            _SWCSModel = new SWCSModel.SWCSModel();
        }

        #region Call Single Window Page Developed by Rajveer
        public FMDSS.Models.SWCSModel.SWCSModel _SWCSModel { get; set; }
        #endregion

        #region data members

        public decimal PerposedArea { get; set; }
        public string FOREST_DIVCODE { get; set; }
        public Int64 UserID { get; set; }
        public string SSOID { get; set; }
        public string GISID { get; set; }
        public string UserName { get; set; }
        public Int64 CreatedBy { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public string DIV_CODE { get; set; }
        public string RequestedID { get; set; }
        public string DIST_CODE { get; set; }
        public string BLK_CODE { get; set; }
        public string GP_CODE { get; set; }
        public string VILL_CODE { get; set; }
        public string khasra_no { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        public string KML_Path { get; set; }
        public string Revenue_Record_Path { get; set; }
        public string Revenue_Map_Path { get; set; }
        public string Revenue_Map_Signed { get; set; }
        public int Concerned_Dept { get; set; }

        public string Duration_From { get; set; }
        public string Duration_To { get; set; }
        public string Additional_Document { get; set; }
        public string Citizen_Comment { get; set; }
        //public Nullable<System.DateTime> Duration_From { get; set; }
        //public Nullable<System.DateTime> Duration_To { get; set; }

        public string Industrial_Type { get; set; }
        public string kioskuserid { get; set; }
        [Display(Name = "Amount")]
        public Decimal Amount { get; set; }
        public Decimal Discount { get; set; }
        public Decimal Tax { get; set; }
        public Decimal Final_Amount { get; set; }
        public string Sawmill_Type { get; set; }
        public string Sawmill_Size { get; set; }
        public string Nearest_WaterSource { get; set; }
        public string WaterSource_Distance { get; set; }
        public string Forest_Distance { get; set; }
        public string Wildlife_Distance { get; set; }
        public string Tree_species { get; set; }
        public string AravalliHills { get; set; }
        public string ForestLand { get; set; }
        public string Plantation_Area { get; set; }
        public DateTime Created_Date { get; set; }
        public int Status { get; set; }
        public string PayStatus { get; set; }
        public string ReasonsAttached { get; set; }
        public string GPSLat { get; set; }
        public string GPSLong { get; set; }
        public string Area_Size { get; set; }
        public string OtherPermission { get; set; }
        public int ApplicantType { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string TransactionId { get; set; }
        public Int64 PermissionId { get; set; }
        public string PermissionType { get; set; }
        public int IsActive { get; set; }
        public int Trn_Status_Code { get; set; }
        public string ConditionRevenueMapSigned { get; set; }
        public string IsGTSheetAvailable { get; set; }
        public string ConditionIsGTSheetAvailable { get; set; }
        public bool ConditionFileEditMode { get; set; }
        public bool ConditionGISMode { get; set; }
        public SelectListItem DDlDistrict { get; set; }

        // added by Vandana Gupta on 23-Aug-2016
        public string txtForestDensity { get; set; }
        public string txtplantOthers { get; set; }
        public string txtplantOthersNo { get; set; }

        public SWCSResponse swcsResponse { get; set; }

        #endregion

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable Division()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Division" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable MiningDistrict()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_MDistrict", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "MiningDistrict" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindDistrict(string Division)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Districts", Conn);
                cmd.Parameters.AddWithValue("@divCode", Division);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDistrict" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public Int64 SubmitFixedLandUsage(FixedLandUsage _objmodel, DataTable dt, DataTable dtPlant, int NOCPurpose = 0)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Insert_FixedLandRecords", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@RequestedID", _objmodel.TransactionId);
                cmd.Parameters.AddWithValue("@ApplicantType", _objmodel.ApplicantType);
                cmd.Parameters.AddWithValue("@PermissionTypeID", _objmodel.PermissionId);
                cmd.Parameters.AddWithValue("@DIV_CODE", _objmodel.DIV_CODE);
                cmd.Parameters.AddWithValue("@DIST_CODE", _objmodel.DIST_CODE);
                cmd.Parameters.AddWithValue("@BLK_CODE", _objmodel.BLK_CODE);
                cmd.Parameters.AddWithValue("@GP_CODE", _objmodel.GP_CODE);
                cmd.Parameters.AddWithValue("@VILL_CODE", _objmodel.VILL_CODE);
                cmd.Parameters.AddWithValue("@Location", _objmodel.khasra_no);
                cmd.Parameters.AddWithValue("@Area", _objmodel.Area);
                cmd.Parameters.AddWithValue("@Area_Size", _objmodel.Area_Size);
                cmd.Parameters.AddWithValue("@GPSLat", _objmodel.GPSLat);
                cmd.Parameters.AddWithValue("@GPSLong", _objmodel.GPSLong);
                cmd.Parameters.AddWithValue("@Industrial_Type", _objmodel.Industrial_Type);
                cmd.Parameters.AddWithValue("@IsGTSheetAvailable", _objmodel.IsGTSheetAvailable);
                cmd.Parameters.AddWithValue("@Revenue_Map_Signed", _objmodel.Revenue_Map_Signed);
                cmd.Parameters.AddWithValue("@Duration_From", _objmodel.Duration_From);
                cmd.Parameters.AddWithValue("@Duration_To", _objmodel.Duration_To);
                cmd.Parameters.AddWithValue("@KML_Path", _objmodel.KML_Path);
                cmd.Parameters.AddWithValue("@Revenue_Record_Path", _objmodel.Revenue_Record_Path);
                cmd.Parameters.AddWithValue("@Revenue_Map_Path", _objmodel.Revenue_Map_Path);
                cmd.Parameters.AddWithValue("@Nearest_WaterSource", _objmodel.Nearest_WaterSource);
                cmd.Parameters.AddWithValue("@WaterSource_Distance", _objmodel.WaterSource_Distance);
                cmd.Parameters.AddWithValue("@Forest_Distance", _objmodel.Forest_Distance);
                cmd.Parameters.AddWithValue("@Wildlife_Distance", _objmodel.Wildlife_Distance);
                cmd.Parameters.AddWithValue("@Tree_species", _objmodel.Tree_species);
                cmd.Parameters.AddWithValue("@AravalliHills", _objmodel.AravalliHills);
                cmd.Parameters.AddWithValue("@ForestLand", _objmodel.ForestLand);
                cmd.Parameters.AddWithValue("@Sawmill_Type", _objmodel.Sawmill_Type);
                cmd.Parameters.AddWithValue("@Sawmill_Size", _objmodel.Sawmill_Size);
                cmd.Parameters.AddWithValue("@OtherPermission", _objmodel.OtherPermission);
                cmd.Parameters.AddWithValue("@Plantation_Area", _objmodel.Plantation_Area);
                cmd.Parameters.AddWithValue("@Amount", _objmodel.Amount);
                cmd.Parameters.AddWithValue("@Discount", _objmodel.Discount);
                cmd.Parameters.AddWithValue("@Tax", _objmodel.Tax);
                cmd.Parameters.AddWithValue("@Final_Amount", _objmodel.Final_Amount);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@Additional_Document", _objmodel.Additional_Document);
                cmd.Parameters.AddWithValue("@Citizen_Comment", _objmodel.Citizen_Comment);
                cmd.Parameters.AddWithValue("@MappingData", dt);
                cmd.Parameters.AddWithValue("@PlantMappingData", dtPlant);
                cmd.Parameters.AddWithValue("@GISID", _objmodel.GISID);
                cmd.Parameters.AddWithValue("@KioskUserId", Convert.ToInt64(_objmodel.kioskuserid));
                cmd.Parameters.AddWithValue("@OtherPlantName", _objmodel.txtplantOthers);
                cmd.Parameters.AddWithValue("@OtherPlantNumber", _objmodel.txtplantOthersNo);
                cmd.Parameters.AddWithValue("@NOCPurpose", NOCPurpose);
                cmd.Parameters.AddWithValue("@PurposeArea", PerposedArea);


                Int64 chId = Convert.ToInt64(cmd.ExecuteNonQuery());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "SubmitFixedLandUsage" + "_" + "Citizen", 1, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public int SubmitNocData(NocData _objmodel, DataTable dtGis, DataTable dtPlant, int NOCPurpose = 0)
        {
            try
            {
                int result = 0;
                DALConn();
                SqlCommand cmd = new SqlCommand("KN_FixedLandRecords_I", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@GISID", _objmodel.GISID);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@RequestedID", _objmodel.RequestedID);
                //cmd.Parameters.AddWithValue("@DIV_CODE", _objmodel.DIV_CODE);
                //cmd.Parameters.AddWithValue("@DIST_CODE", _objmodel.DIST_CODE);
                //cmd.Parameters.AddWithValue("@BLK_CODE", _objmodel.BLK_CODE);
                //cmd.Parameters.AddWithValue("@GP_CODE", _objmodel.GP_CODE);
                //cmd.Parameters.AddWithValue("@VILL_CODE", _objmodel.VILL_CODE);
                //cmd.Parameters.AddWithValue("@Area", _objmodel.Area);
                //cmd.Parameters.AddWithValue("@Location", _objmodel.khasra_no);
                cmd.Parameters.AddWithValue("@KML_Path", _objmodel.KML_Path);
                cmd.Parameters.AddWithValue("@Revenue_Record_Path", _objmodel.Revenue_Record_Path);
                cmd.Parameters.AddWithValue("@Revenue_Map_Path", _objmodel.Revenue_Map_Path);
                cmd.Parameters.AddWithValue("@Revenue_Map_Signed", _objmodel.Revenue_Map_Signed);

                cmd.Parameters.AddWithValue("@Duration_From", _objmodel.Duration_From);
                cmd.Parameters.AddWithValue("@Duration_To", _objmodel.Duration_To);
                cmd.Parameters.AddWithValue("@Industrial_Type", _objmodel.Industrial_Type);
                cmd.Parameters.AddWithValue("@IsGTSheetAvailable", _objmodel.IsGTSheetAvailable);

                cmd.Parameters.AddWithValue("@Amount", _objmodel.Amount);
                cmd.Parameters.AddWithValue("@Discount", _objmodel.Discount);
                cmd.Parameters.AddWithValue("@Tax", _objmodel.Tax);
                cmd.Parameters.AddWithValue("@Final_Amount", _objmodel.Final_Amount);

                cmd.Parameters.AddWithValue("@Sawmill_Type", _objmodel.Sawmill_Type);
                cmd.Parameters.AddWithValue("@Sawmill_Size", _objmodel.Sawmill_Size);
                cmd.Parameters.AddWithValue("@Nearest_WaterSource", _objmodel.Nearest_WaterSource);
                cmd.Parameters.AddWithValue("@WaterSource_Distance", _objmodel.WaterSource_Distance);
                cmd.Parameters.AddWithValue("@Forest_Distance", _objmodel.Forest_Distance);
                cmd.Parameters.AddWithValue("@Wildlife_Distance", _objmodel.Wildlife_Distance);
                cmd.Parameters.AddWithValue("@Tree_species", _objmodel.Tree_species);
                cmd.Parameters.AddWithValue("@AravalliHills", _objmodel.AravalliHills);
                cmd.Parameters.AddWithValue("@ForestLand", _objmodel.ForestLand);
                cmd.Parameters.AddWithValue("@Plantation_Area", _objmodel.Plantation_Area);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@GPSLat", _objmodel.GPSLat);
                cmd.Parameters.AddWithValue("@GPSLong", _objmodel.GPSLong);
                cmd.Parameters.AddWithValue("@OtherPermission", _objmodel.OtherPermission);
                cmd.Parameters.AddWithValue("@Area_Size", _objmodel.Area_Size);

                cmd.Parameters.AddWithValue("@ApplicantType", _objmodel.ApplicantType);
                cmd.Parameters.AddWithValue("@PermissionTypeID", _objmodel.NOCType);

                cmd.Parameters.AddWithValue("@Additional_Document", _objmodel.Additional_Document);
                cmd.Parameters.AddWithValue("@Citizen_Comment", _objmodel.Citizen_Comment);
                cmd.Parameters.AddWithValue("@MappingData", dtGis);
                cmd.Parameters.AddWithValue("@PlantMappingData", dtPlant);

                cmd.Parameters.AddWithValue("@KioskUserId", Convert.ToInt64(_objmodel.kioskuserid));
                //cmd.Parameters.AddWithValue("@OtherPlantName", _objmodel.txtplantOthers);
                //cmd.Parameters.AddWithValue("@OtherPlantNumber", _objmodel.txtplantOthersNo);
                cmd.Parameters.AddWithValue("@NOCPurpose", _objmodel.NOCPurpose);
                cmd.Parameters.AddWithValue("@PurposeArea", _objmodel.PerposedArea);
                cmd.Parameters.AddWithValue("@IsSWCS", _objmodel.IsSWCS);
                // cmd.Parameters.AddWithValue("@SWCS_PersonInfo", _objmodel.personStr);                
                cmd.Parameters.AddWithValue("@outPut", result).Direction = ParameterDirection.Output;
                int chId = cmd.ExecuteNonQuery();

                result = Convert.ToInt32(cmd.Parameters["@outPut"].Value);

                return result;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "SubmitFixedLandUsage" + "_" + "Citizen", 1, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 UpdateData(FixedLandUsage _objmodel, string updatetype)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Update_FixedLand", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@RequestedID", _objmodel.RequestedID);

                cmd.Parameters.AddWithValue("@Revenue_Record_Path", _objmodel.Revenue_Record_Path);
                cmd.Parameters.AddWithValue("@Revenue_Map_Path", _objmodel.Revenue_Map_Path);
                cmd.Parameters.AddWithValue("@LastUpdatedOn", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdateType", updatetype);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateData" + "_" + "Citizen", 1, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public DataSet UpdatePaymentStatus(Int64 TransID, int TransStatus, string updatetype, string RequestID, Int64 UserID, decimal finalamount = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Citizen_Update_FixedLand", Conn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@TransactionId", TransID);
                cmd.Parameters.AddWithValue("@Trn_Status_Code", TransStatus);
                cmd.Parameters.AddWithValue("@RequestedID", RequestID);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@UpdateType", updatetype);
                cmd.Parameters.AddWithValue("@finalamount", finalamount);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdatePaymentStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetFixedLandValues(string RequestedID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_GetRecord_FixedLand", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedID", RequestedID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFixedLandValues" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetFinalAmount(Int64 UserID, int PermissionType)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("[SP_Citizen_Select_FixedLandPermissionCharge]", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PermissionType", PermissionType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFinalAmount" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet GetPermissionTypes(int PermissionType)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("[Sp_Citizen_GetFixedPermissionTypes]", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PID", PermissionType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetPermissionTypes" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;

        }


        public void SaveDistrictMapping(List<clsPermission> list)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                Conn.Open();
                for (int i = 0; i < list.Count; i++)
                {

                    SqlCommand cmd = new SqlCommand("[Sp_Citizen_Insert_FixedLandRecords_distMap]", Conn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestedID", list[i].RequestedID);
                    cmd.Parameters.AddWithValue("@DIV_CODE", list[i].DIV_CODE);
                    cmd.Parameters.AddWithValue("@DIST_CODE", list[i].DIST_CODE);
                    cmd.Parameters.AddWithValue("@BLK_CODE", list[i].BLK_CODE);
                    cmd.Parameters.AddWithValue("@GP_CODE", list[i].GP_CODE);
                    cmd.Parameters.AddWithValue("@VILL_CODE", list[i].VILL_CODE);
                    cmd.Parameters.AddWithValue("@Location", list[i].KhasraNo);
                    cmd.Parameters.AddWithValue("@Area", list[i].Area);
                    cmd.Parameters.AddWithValue("@FOREST_DIVCODE", list[i].FOREST_DIVCODE);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveDistrictMapping" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }

        }

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

        public DataSet GetPlantList()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_GetPlantList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
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


    }

}

