using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FMDSS.Filters;

namespace FMDSS.Models.CitizenService.ProductionServices
{

    #region Model/Repo For Servey Report and Bhamasha services model developed by Rajveer

    public class BhamashaServiceModel
    {
        public BhamashaServiceModel()
        {
            Data = new object();
        }
        public string errorcode { get; set; }
        public string errorDescription { get; set; }

        public object Data { get; set; }
    }
    public class SurveyReportTP
    {
        public SurveyReportTP()
        {
            ProduceList = new List<ProductDetails>();
        }
        [Required]
        public string PermitID { get; set; }
        [Required]
        [Display(Name = "Division Office")]
        public string DIVISION_OFFICE { get; set; }
        [Required]
        [Display(Name = "District Office")]
        public string District_OFFICE { get; set; }
        [Required]
        public string Village { get; set; }
        [Required]
        public string Tehsil { get; set; }
        [Required]
        [Display(Name = "Survey Date")]
        public string SurveyDate { get; set; }
        [Required]
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }
        [Required]
        [Display(Name = "Area In Km")]
        public string AreaInKm { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }

        public string UploadFile { get; set; }
        public string UploadShapeFile { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "File")]
        [RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.png|.PNG|.jpg|.JPG|.gif|.GIF|.image|.IMAGE)$", ErrorMessage = "Only .jpg  /.png /.gif file formats are allowed")]
        public HttpPostedFileBase fileUpload { get; set; }

        [Required]
        [Display(Name = "Shape File")]
        [RegularExpression(@"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.png|.PNG|.jpg|.JPG|.gif|.GIF|.image|.IMAGE)$", ErrorMessage = "Only .jpg  /.png /.gif file formats are allowed")]
        public HttpPostedFileBase fileShapeFile { get; set; }

        public string ProduceListInString { get; set; }
        public List<ProductDetails> ProduceList { get; set; }

    }

    public class SurveyReportRepository : DAL
    {

        public string SaveServeyReportTP(SurveyReportTP model)
        {
            DataTable dt = new DataTable();
            string Message = string.Empty;
            try
            {
                #region Convert Model Into Datatable
                string JSONString = JsonConvert.SerializeObject(model.ProduceList);
                DataTable ProductDetails = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
                #endregion

                DALConn();
                SqlCommand cmd = new SqlCommand("sp_CTP_InsertServeyReport", Conn);
                cmd.Parameters.AddWithValue("@RequestID", Convert.ToString(model.PermitID));
                cmd.Parameters.AddWithValue("@DIVISION_OFFICE", Convert.ToString(model.DIVISION_OFFICE));
                cmd.Parameters.AddWithValue("@District_OFFICE", Convert.ToString(model.District_OFFICE));
                cmd.Parameters.AddWithValue("@Village", Convert.ToString(model.Village));

                cmd.Parameters.AddWithValue("@SurveyDate", Convert.ToString(model.SurveyDate));
                cmd.Parameters.AddWithValue("@UploadFile", Convert.ToString(model.UploadFile));
                cmd.Parameters.AddWithValue("@AreaName", Convert.ToString(model.AreaName));
                cmd.Parameters.AddWithValue("@AreaInKm", Convert.ToString(model.AreaInKm));

                cmd.Parameters.AddWithValue("@Latitude", Convert.ToString(model.Latitude));
                cmd.Parameters.AddWithValue("@Longitude", Convert.ToString(model.Longitude));
                cmd.Parameters.AddWithValue("@Description", Convert.ToString(model.Description));
                cmd.Parameters.AddWithValue("@SharpUpload", Convert.ToString(model.UploadShapeFile));
                cmd.Parameters.AddWithValue("@ProductDetails", ProductDetails);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(HttpContext.Current.Session["UserID"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    Message = Convert.ToString(dt.Rows[0]["Message"]);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveServeyReportTP" + "_" + "ProductionServices", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return Message;
        }

    }
    #endregion
    public class ProductDetails
    {
        public string Permit_no { get; set; }
        public int PRODUCE_VALUE { get; set; }
        public string PRODUCE_QUANTITY { get; set; }
        public string PRODUCE_DESCRIPTION { get; set; }
    }

    public class TransitPermit : DAL
    {
        public TransitPermit()
        {
            ProductList = new List<ProductDetails>();
        }
        public List<ProductDetails> ProductList { get; set; }
        public string REQUEST_ID { get; set; }
        public Int64 UserId { get; set; }
        public string Option { get; set; }
        public long ID { get; set; }
        public string PERMIT_NO { get; set; }

        [Required(ErrorMessage = "Select division")]
        public string DIVISION_OFFICE { get; set; }

        [Required(ErrorMessage = "Select range")]
        public string RANGE_OFFICE { get; set; }

        [Required(ErrorMessage = "Enter name")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string APPLICANT_NAME { get; set; }

        [Required(ErrorMessage = "Enter address")]
        public string APPLICANT_ADDRESS { get; set; }

        [Required(ErrorMessage = "Enter village")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string APPLICANT_VILLAGE { get; set; }
        [Required(ErrorMessage = "Enter tehsil")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string APPLICANT_TEHSIL { get; set; }
        [Required(ErrorMessage = "Select district")]
        public int APPLICANT_DISTRICT { get; set; }

        public int AadharOrBhamasha { get; set; }
        [Required(ErrorMessage = "Enter Aadhar/Bhamasha No.")]
        public string  AadharOrBhamashaNumber { get; set; }
        public string PRODUCE_ID { get; set; }
        //[Required(ErrorMessage = "Select produce")]
        public string PRODUCE_NAME { get; set; }
        //[Required(ErrorMessage = "Enter quantity")]
        public int PRODUCE_QUANTITY { get; set; }
        //[Required(ErrorMessage = "Enter description")]
        public string PRODUCE_DESCRIPTION { get; set; }
        [Required(ErrorMessage = "Enter name")]
        public string LANDHOLDER_NAME { get; set; }
        [Required(ErrorMessage = "Enter village")]
        public string VILLAGE_NAME { get; set; }
        [Required(ErrorMessage = "Enter khasra")]
        public string KHASRA_NO { get; set; }
        [Required(ErrorMessage = "Enter area")]

        public decimal LANDHOLDING_AREA { get; set; }
        [Required(ErrorMessage = "Select district")]
        public int DISTRICT { get; set; }
        [Required(ErrorMessage = "Enter tehsil")]
        public string TEHSIL { get; set; }
        [Required(ErrorMessage = "Enter place")]
        public string PLACE_NAME { get; set; }
        [Required(ErrorMessage = "Select vehicle No.")]
        public string VEHICLE_NO { get; set; }
        [Required(ErrorMessage = "Select vehicle Type")]
        public string VEHICLE_TYPE { get; set; }
        public int TP_FEES { get; set; }
        public DateTime TP_FEES_PAIDON { get; set; }
        [Required(ErrorMessage = "Select state from")]
        public int FROM_STATE { get; set; }
        [Required(ErrorMessage = "Select state to")]
        public int TO_STATE { get; set; }
        [Required(ErrorMessage = "Select district from")]
        public int FROM_DISTRICT { get; set; }
        [Required(ErrorMessage = "Select district to")]
        public int TO_DISTRICT { get; set; }
        public string ROUTE_PLAN { get; set; }
        public string TP_VALIDITY_DATE { get; set; }

        public short Trn_Status_Code { get; set; }

        public long EmitraTransactionID { get; set; }

        public long ENTEREDBY { get; set; }

        public DateTime ENTEREDON { get; set; }

        public int UPDATEDBY { get; set; }

        public DateTime UPDATEDON { get; set; }

        public bool APPLICATIONSTATUS { get; set; }

        public bool ISACTIVE { get; set; }
        public List<ListInventory> ListInventorys { get; set; }


        public DataTable Fill_Dropdown()
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

        public DataSet Fill_DropdownRangeAndDist()
        {
            DataSet dtdropdown = new DataSet();
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

        public DataTable GetUserDetails(string Option)
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


        /// <summary>
        /// Final insertion of CMS Upload
        /// </summary>
        /// <returns>status</returns>
        public string CREATE_TRANSITPERMIT()
        {
            try
            {
                #region Convert Model Into Datatable

                string JSONString = JsonConvert.SerializeObject(this.ProductList);
                DataTable ProductDetails = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));


                #endregion

                DALConn();
                DataTable dtTag = new DataTable();
                SqlCommand cmd = new SqlCommand("INSERT_Citizen_TransitPermit", Conn);
                cmd.Parameters.AddWithValue("@PERMIT_NO", this.PERMIT_NO);
                cmd.Parameters.AddWithValue("@DIVISION_OFFICE", this.DIVISION_OFFICE);
                cmd.Parameters.AddWithValue("@RANGE_OFFICE", this.RANGE_OFFICE);
                cmd.Parameters.AddWithValue("@APPLICANT_NAME", this.APPLICANT_NAME);
                cmd.Parameters.AddWithValue("@APPLICANT_ADDRESS", this.APPLICANT_ADDRESS);
                cmd.Parameters.AddWithValue("@APPLICANT_VILLAGE", this.APPLICANT_VILLAGE);
                cmd.Parameters.AddWithValue("@APPLICANT_TEHSIL", this.APPLICANT_TEHSIL);
                cmd.Parameters.AddWithValue("@APPLICANT_DISTRICT", this.APPLICANT_DISTRICT);
                cmd.Parameters.AddWithValue("@PRODUCE_NAME", this.PRODUCE_ID);
                cmd.Parameters.AddWithValue("@PRODUCE_QUANTITY", this.PRODUCE_QUANTITY);
                cmd.Parameters.AddWithValue("@PRODUCE_DESCRIPTION", this.PRODUCE_DESCRIPTION);
                cmd.Parameters.AddWithValue("@LANDHOLDER_NAME", this.LANDHOLDER_NAME);
                cmd.Parameters.AddWithValue("@VILLAGE_NAME", this.VILLAGE_NAME);
                cmd.Parameters.AddWithValue("@KHASRA_NO", this.KHASRA_NO);
                cmd.Parameters.AddWithValue("@LANDHOLDING_AREA", this.LANDHOLDING_AREA);
                cmd.Parameters.AddWithValue("@DISTRICT", this.DISTRICT);
                cmd.Parameters.AddWithValue("@TEHSIL", this.TEHSIL);
                cmd.Parameters.AddWithValue("@PLACE_NAME", this.PLACE_NAME);
                cmd.Parameters.AddWithValue("@VEHICLE_NO", this.VEHICLE_NO);
                cmd.Parameters.AddWithValue("@VEHICLE_TYPE", this.VEHICLE_TYPE);
                cmd.Parameters.AddWithValue("@TP_FEES", this.TP_FEES);
                //cmd.Parameters.AddWithValue("@TP_FEES_PAIDON", this.TP_FEES_PAIDON);
                cmd.Parameters.AddWithValue("@FROM_STATE", this.FROM_STATE);
                cmd.Parameters.AddWithValue("@TO_STATE", this.TO_STATE);
                cmd.Parameters.AddWithValue("@FROM_DISTRICT", this.FROM_DISTRICT);
                cmd.Parameters.AddWithValue("@TO_DISTRICT", this.TO_DISTRICT);
                cmd.Parameters.AddWithValue("@ROUTE_PLAN", this.ROUTE_PLAN);
                cmd.Parameters.AddWithValue("@TP_VALIDITY_DATE", this.TP_VALIDITY_DATE);
                cmd.Parameters.AddWithValue("@ENTEREDBY", Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                cmd.Parameters.AddWithValue("@AadharOrBhamashaType", Convert.ToInt64(this.AadharOrBhamasha));
                cmd.Parameters.AddWithValue("@AadharOrBhamashaNumber", Convert.ToString(this.AadharOrBhamashaNumber));

                cmd.Parameters.AddWithValue("@ProductDetails", ProductDetails);
                cmd.CommandType = CommandType.StoredProcedure;
                string status = Convert.ToString(cmd.ExecuteScalar());
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Conn.Close();
            }
        }


    }
    public class ListInventory
    {

        public string SrNo { get; set; }
        public string ProduceName { get; set; }
        public string ProduceQuantity { get; set; }
        public string ProduceDescription { get; set; }
    }
}