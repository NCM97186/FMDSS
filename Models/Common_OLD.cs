//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;


namespace FMDSS.Models
{
    public class Common : DAL
    {

        public static IList<SelectListItem> GetDropOutReason()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "Not Intrested to Buy", Text = "Not Intrested to Buy" });
            _result.Add(new SelectListItem { Value = "Payment Canot be done", Text = "Payment Canot be done" });
          

            return _result;
        }

         public static IList<SelectListItem> GetCategory()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "Conservation Resrve", Text = "Conservation Resrve" });
            _result.Add(new SelectListItem { Value = "National Park", Text = "National Park" });
            _result.Add(new SelectListItem { Value = "Wildlife Santuaries", Text = "Wildlife Santuaries" });
            
            return _result;
        }
        public static IList<SelectListItem> GetUnit()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "Quintal", Text = "Quintal" });
            _result.Add(new SelectListItem { Value = "No.Of Packing", Text = "No.Of Packing" });
            _result.Add(new SelectListItem { Value = "Standard Bar", Text = "Standard Bar" });
            
            return _result;
        }

        public static IList<SelectListItem> GetQualification()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Highter School" });
            _result.Add(new SelectListItem { Value = "2", Text = "Highter Secondary" });
            _result.Add(new SelectListItem { Value = "3", Text = "Graduate" });
            _result.Add(new SelectListItem { Value = "4", Text = "Post Graduate" });

            
           
            return _result;
        }
        public static IList<SelectListItem> GetResearchCategory()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Animal" });
            _result.Add(new SelectListItem { Value = "3", Text = "Animal & Plant" });
            _result.Add(new SelectListItem { Value = "2", Text = "Species" });
            
            return _result;
        }
        public static IList<SelectListItem> GetApplicantType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "1", Text = "Individual" });
            _result.Add(new SelectListItem { Value = "2", Text = "Organization" });
            return _result;
        }

        public static IList<SelectListItem> GetNearestWaterSource()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Well" });
            _result.Add(new SelectListItem { Value = "2", Text = "Nala" });
            _result.Add(new SelectListItem { Value = "3", Text = "River" });
            _result.Add(new SelectListItem { Value = "4", Text = "Lake" });
            return _result;
        }

        public static IList<SelectListItem> GetSawmillType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Cutting/Converting of Timber" });
            _result.Add(new SelectListItem { Value = "2", Text = "Wood Handicraft Swmills" });
            return _result;
        }

        public static IList<SelectListItem> GetIndustrialType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Advertising Industry" });
            _result.Add(new SelectListItem { Value = "2", Text = "Agriculture Industry" });
            return _result;
        }

        public static IList<SelectListItem> GetMeasurementUnits()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Square Foot(sq ft)" });
            _result.Add(new SelectListItem { Value = "2", Text = "Square Meter(sq m)" });
            _result.Add(new SelectListItem { Value = "3", Text = "Acre" });
            _result.Add(new SelectListItem { Value = "4", Text = "Square Kilometer(sq km)" });
            _result.Add(new SelectListItem { Value = "5", Text = "Square Mile(sq mi)" });
            return _result;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string GenerateBody(decimal Amount, string UserName, string TransID, string PermissionDesc)
        {
            //Dummy data for Invoice (Bill).
            StringBuilder sb = new StringBuilder();
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[2] {
		                new DataColumn("Transaction ID", typeof(string)),
		                new DataColumn("Total Amount", typeof(decimal))});

            dt.Rows.Add(TransID, Amount.ToString());


            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {


                    //Generate Invoice (Bill) Header.
                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>Thanks for applying for " + PermissionDesc + ". Your Transaction ID <b>#" + TransID + ".</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Please quote this ID for future communication.");
                    sb.Append("<br />");
                    sb.Append("To view status of your application please click here.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br />");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");

                }
            }
            return sb.ToString();
        }

        public static string GenerateReviwerBody(string UserName, string TransID, string PermissionDesc)
        {
            //Dummy data for Invoice (Bill).
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {

                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>Your request for " + PermissionDesc + " by the Request ID <b>#" + TransID + ".</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Kindly login and take a necessary action.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an automatically generated email, please do not reply ***</b></p>");
                    
                }
            }
            return sb.ToString();
        }

        public static string GenerateSMSBody(string UserName, string TransID, string PermissionDesc)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Dear " + UserName + ",");
            sb.Append("Thanks for applying for " + PermissionDesc + " Your requtest ID is #" + TransID + ".");
            sb.Append("To view status of your application please log-on FMDSS.");
            return sb.ToString();
        }

        public Int64 SaveFavouritelink(Int64 UserID, string PageUrl, string PageName, int IsActive, int ModuleId, string StatementType)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_For_Favourite_link", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@PageName", PageName);
                cmd.Parameters.AddWithValue("@URL", PageUrl);
                cmd.Parameters.AddWithValue("@IsActive", IsActive);
                cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
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

        public static string Citizen_GenerateApprovedBody(string UserName, string TransID, string PermissionDesc)
        {
            StringBuilder sb = new StringBuilder();
            string click = "<a href=" + ConfigurationManager.AppSettings["emitraReturnUrl"].ToString() + ">click</a>";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>Your request for the " + PermissionDesc + " vide no <b>#" + TransID + " has been approved.</b></p>");
                    sb.Append("<br/>");
                    sb.Append("You may find NOC attached here with this email or download");
                    sb.Append("<br/>");
                    sb.Append("the NOC from your FMDSS Dashboard.");
                    sb.Append("<br/>");
                    sb.Append("To view status of your application please " + click + " here.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");
                }
            }
            return sb.ToString();
        }

        public static string Citizen_Reassign_Review_RejEmailBody(string UserName, string TransID, string PermissionDesc, string action)
        {
            StringBuilder sb = new StringBuilder();
            string click = "<a href=" + ConfigurationManager.AppSettings["emitraReturnUrl"].ToString() + ">click</a>";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>Your request for the " + PermissionDesc + " vide no <b>#" + TransID + " has been " + action + ".</b></p>");
                    sb.Append("<br/>");
                    sb.Append("To view status of your application please" + click + "here.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");
                }
            }
            return sb.ToString();
        }

        public static string Forester_Approve_ReviewRequest_EmailBody(string UserName, string TransID, string PermissionDesc, string action)
        {
            StringBuilder sb = new StringBuilder();
            string click = "<a href=" + ConfigurationManager.AppSettings["emitraReturnUrl"].ToString() + ">click</a>";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>An application has been received for " + PermissionDesc + " and the request ID is <b>#" + TransID + ".</b></p>");
                    sb.Append("<br/>");
                    sb.Append("To " + action + " the pending application please " + click + " here or log-on FMDSS.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");
                }
            }
            return sb.ToString();
        }
        public static string Forester_Approve_ReviewRequest_SMSBody(string UserName, string TransID, string PermissionDesc, string action)
        {
            StringBuilder sb = new StringBuilder();
            string click = "<a href=" + ConfigurationManager.AppSettings["emitraReturnUrl"].ToString() + ">click</a>";
            sb.Append("An application has been received for " + PermissionDesc + " and request ID is #" + TransID + ".");
            sb.Append("To " + action + " the pending application check your email or log-on FMDSS.");
            return sb.ToString();
        }
        public static string Citizen_Reasssign_Review_Rej_SMSBody(string UserName, string TransID, string PermissionDesc, string action)
        {
            StringBuilder sb = new StringBuilder();
            string click = "<a href=" + ConfigurationManager.AppSettings["emitraReturnUrl"].ToString() + ">click</a>";

            sb.Append("Your request for " + PermissionDesc + " vide no. #" + TransID + " has been " + action + ".");
            sb.Append("To view status of your application please check your email or Log-on FMDSS");
            return sb.ToString();
        }

        public static string Citizen_Approved_SMSBody(string UserName, string TransID, string PermissionDesc)
        {
            StringBuilder sb = new StringBuilder();
            string click = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
            sb.Append("Your request for " + PermissionDesc + " vide no.#" + TransID + " has been approved.");
            sb.Append("To view and print of NOC please check your email or log-on FMDSS");
            return sb.ToString();
        }

        public DataSet GetAllRecords(Int64 UserID, string StatementType)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_For_Favourite_link", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
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

        public int DeleteFavouritelink(int ID, string StatementType, int IsActive)
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_For_Favourite_link", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", @ID);
                cmd.Parameters.AddWithValue("@IsActive", IsActive);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
                int chId = Convert.ToInt32(cmd.ExecuteScalar());
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





        public void ErrorLog(string ErrorMsg, string FunctionName, int ModuleId, DateTime ErrorDate, Int64 EnteredBy)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Common_Insert_ErrorLog", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                cmd.Parameters.AddWithValue("@FunctionName", FunctionName);
                cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
                cmd.Parameters.AddWithValue("@ErrorDate", ErrorDate);
                cmd.Parameters.AddWithValue("@UserID", EnteredBy);
                cmd.ExecuteNonQuery();
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
        public enum TableName
        {
            tbl_AccomodationBooking = 1,
            tbl_AuctionDetail = 2,
            tbl_Contractor_VehicleDetails = 3,
            tbl_ContractorRegistration = 4,
            tbl_Email_History = 5,
            tbl_Error = 6,
            tbl_Favourite_link = 7,
            tbl_FDM_BudgetEstimation = 8,
            tbl_FDM_MicroPlan = 9,
            tbl_FDM_MicroPlan_ProjectDetail = 10,
            tbl_FDM_MicroPlanEducationDetail = 11,
            tbl_FDM_MicroPlanPopulationDetail = 12,
            tbl_FDM_Project = 13,
            tbl_FDM_ProjectSchemeModel = 14,
            tbl_FDM_Scheme = 15,
            tbl_FDM_SchemeDistrictDetails = 16,
            tbl_FDM_Workorder = 17,
            tbl_FDM_WorkOrder_Activity = 18,
            tbl_FDM_Workorderprogress = 19,
            tbl_FeedbackQuery = 20,
            tbl_FilmShootingCrewMember = 21,
            tbl_FilmShootingPermissions = 22,
            tbl_FixedPermissions = 23,
            tbl_mst_AccomodationFee = 24,
            tbl_mst_Blocks = 25,
            tbl_mst_CampFees = 26,
            tbl_mst_Cast = 27,
            tbl_mst_CategoryAnimal = 27,
            tbl_mst_CoordinatorRegistration = 28,
            tbl_mst_Designations = 29,
            tbl_mst_Districts = 30,
            tbl_mst_Education = 31,
            tbl_mst_Eqpt_SanctuariesFee = 32,
            tbl_mst_FDM_Activity = 33,
            tbl_mst_FDM_Model = 34,
            tbl_mst_FDM_Program = 35,
            tbl_mst_FDM_Request_Log = 36,
            tbl_mst_FDM_Sub_Activity = 38,
            tbl_mst_FilmShootingFees = 39,
            tbl_mst_FixedPermissionTypes = 40,
            tbl_mst_FMDSSPermissionsTypes = 41,
            tbl_mst_Forest_Divisions = 42,
            tbl_mst_Forest_Ranges = 43,
            tbl_mst_Forest_Villages = 44,
            tbl_mst_Forest_WildLifeCircles = 45,
            tbl_mst_ForestDepot = 46,
            tbl_mst_ForestEmployees = 47,
            tbl_mst_ForestOffices = 48,
            tbl_mst_ForestRegions = 49,
            tbl_mst_GPs = 50,
            tbl_mst_Notice = 51,
            tbl_mst_Nurseries = 52,
            tbl_mst_Nurseries1 = 53,
            tbl_mst_PaymentGateWay_Status = 54,
            tbl_mst_Places = 55,
            tbl_mst_Roles = 56,
            tbl_mst_SpeciesAnimal = 57,
            tbl_mst_Status = 58,
            tbl_mst_TicketingFees = 59,
            tbl_mst_Vehicle_Equipment = 60,
            tbl_mst_Vehicle_EquipmentFee = 61,
            tbl_mst_Villages = 62,
            tbl_mst_WildLife_Divisions = 63,
            tbl_mst_WokflowActions = 64,
            tbl_mst_WokflowReasons = 65,
            tbl_OCampCrewMember = 66,
            tbl_OnlineTicketBookings = 67,
            tbl_OrganisingCamp = 68,
            tbl_PastResearchActivities = 69,
            tbl_PermissionsApprovers = 70,
            tbl_ProduceStockDetails = 71,
            tbl_Request_Log = 72,
            tbl_RequestTransactionLog = 73,
            tbl_ResearchStudyPermissions = 74,
            tbl_SafariBooking = 75,
            tbl_SMS_History = 76,
            tbl_TicketBooking = 77,
            tbl_TicketBookingMembers = 78,
            tbl_UserProfiles = 79


        }



    }
}