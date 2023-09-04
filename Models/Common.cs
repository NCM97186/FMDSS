//using iTextSharp.text;
//using iTextSharp.text.html.simpleparser;
//using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq;
using ZXing;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;

namespace FMDSS.Models
{
    public class Common : DAL
    {
        public static IList<SelectListItem> GetActivityCategoryType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });

            _result.Add(new SelectListItem { Value = "Advance Soil Works", Text = "Advance Soil Works" });
            _result.Add(new SelectListItem { Value = "Nursery Operations", Text = "Nursery Operations" });
            _result.Add(new SelectListItem { Value = "Nursery Works", Text = "Nursery Works" });
            _result.Add(new SelectListItem { Value = "Maintenance Work", Text = "Maintenance Work" });
            _result.Add(new SelectListItem { Value = "Plantation Works", Text = "Plantation Works" });
            _result.Add(new SelectListItem { Value = "Wildlife Activity", Text = "Wildlife Activity" });

            return _result;
        }

        public static IList<SelectListItem> GetActivityCategoryTypeWildlife()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "Wildlife Activity", Text = "Wildlife Activity" });

            return _result;
        }
        public static IList<SelectListItem> GetCampaCategory()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "None", Text = "None" });
            _result.Add(new SelectListItem { Value = "CA", Text = "CA" });
            _result.Add(new SelectListItem { Value = "NPV + Interest", Text = "NPV + Interest" });
            return _result;
        }

        public static IList<SelectListItem> GetProductExchangeType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "1", Text = "Site to Depot" });
            // _result.Add(new SelectListItem { Value = "2", Text = "Site to Nursery" });
            //_result.Add(new SelectListItem { Value = "3", Text = "Depot to Depot" });

            return _result;
        }
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

            _result.Add(new SelectListItem { Value = "Conservation Reserve", Text = "Conservation Reserve" });
            _result.Add(new SelectListItem { Value = "National Park", Text = "National Park" });
            _result.Add(new SelectListItem { Value = "Wildlife Santuaries", Text = "Wildlife sanctuaries" });
            _result.Add(new SelectListItem { Value = "Any Other", Text = "Any Other" });

            return _result;
        }

        public static IList<SelectListItem> GetNONPACategory()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "Conservation Reserve", Text = "NON PA + Conservation Reserve" });
            _result.Add(new SelectListItem { Value = "National Park", Text = "NON PA + National Park" });
            _result.Add(new SelectListItem { Value = "Wildlife Santuaries", Text = "NON PA + Wildlife sanctuaries" });
            _result.Add(new SelectListItem { Value = "Any Other", Text = "Any Other" });

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
            _result.Add(new SelectListItem { Value = "1", Text = "Graduate" });
            _result.Add(new SelectListItem { Value = "3", Text = "M. Phill" });
            _result.Add(new SelectListItem { Value = "2", Text = "Post Graduate" });
            _result.Add(new SelectListItem { Value = "4", Text = "PhD" });



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
            _result.Add(new SelectListItem { Value = "1", Text = "Individual" });
            _result.Add(new SelectListItem { Value = "2", Text = "Organization" });
            return _result;
        }

        public static IList<SelectListItem> GetLastThreeYear()
        {
            IList<SelectListItem> revinueItem = new List<SelectListItem>();
            string currentYear = DateTime.Now.Year.ToString();

            revinueItem.Add(new SelectListItem { Text = (Convert.ToInt64(currentYear) - 1).ToString(), Value = (Convert.ToInt64(currentYear) - 1).ToString() });
            revinueItem.Add(new SelectListItem { Text = (Convert.ToInt64(currentYear) - 2).ToString(), Value = (Convert.ToInt64(currentYear) - 2).ToString() });
            revinueItem.Add(new SelectListItem { Text = (Convert.ToInt64(currentYear) - 3).ToString(), Value = (Convert.ToInt64(currentYear) - 3).ToString() });

            return revinueItem;
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
            _result.Add(new SelectListItem { Value = "2", Text = "Wood Handicraft Sawmills" });
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
            _result.Add(new SelectListItem { Value = "Numbers", Text = "Numbers" });
            _result.Add(new SelectListItem { Value = "Running KM", Text = "Running KM" });
            _result.Add(new SelectListItem { Value = "Running Meter", Text = "Running Meter" });
            _result.Add(new SelectListItem { Value = "Square Hectare", Text = "Square Hectare" });
            _result.Add(new SelectListItem { Value = "Square KM", Text = "Square KM" });
            _result.Add(new SelectListItem { Value = "Square foot", Text = "Square foot" });

            _result.Add(new SelectListItem { Value = "/Plant", Text = "/Plant" });
            _result.Add(new SelectListItem { Value = "/RM", Text = "/RM" });
            _result.Add(new SelectListItem { Value = "/M", Text = "/M" });
            _result.Add(new SelectListItem { Value = "Prorata", Text = "Prorata" });
            _result.Add(new SelectListItem { Value = "/RKM", Text = "/RKM" });
            _result.Add(new SelectListItem { Value = "/KG", Text = "/KG" });
            _result.Add(new SelectListItem { Value = "/pit", Text = "/pit" });
            _result.Add(new SelectListItem { Value = "/rmt", Text = "/rmt" });
            _result.Add(new SelectListItem { Value = "lumpsum", Text = "lumpsum" });
            _result.Add(new SelectListItem { Value = "HA", Text = "Hectare" });
            return _result;
        }


        public static IList<SelectListItem> GetActivityType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "Permanent", Text = "Permanent" });
            _result.Add(new SelectListItem { Value = "Fixed", Text = "Fixed" });



            return _result;
        }

        public static IList<SelectListItem> GetSubActivityBSRType()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "Material", Text = "Material" });
            _result.Add(new SelectListItem { Value = "Fixed", Text = "Fixed" });
            _result.Add(new SelectListItem { Value = "Labour", Text = "Labour" });


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

        public static string GenerateSMSBodyReviewerApplyedByCitizen(string TransID, string PermissionDesc)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append("An application has been received for  " + PermissionDesc + " and the  requtest ID is #" + TransID + ".");
            sb.Append("To Review the pending application please log-on FMDSS.");
            return sb.ToString();
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

        public static string Citizen_RequestApprovalEmailBody(string UserName, string TransID, string PermissionDesc)
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
                    sb.Append("<p>Thanks for applying for " + PermissionDesc + " Your request Id is <b>#" + TransID + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Please quote this ID for future communication.");
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
        public static string Citizen_RequestApproval_SMSBody(string UserName, string TransID, string PermissionDesc)
        {
            StringBuilder sb = new StringBuilder();
            string click = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
            sb.Append("Thanks for applying for " + PermissionDesc + " Your Request ID is #" + TransID + ".");
            sb.Append("To view status of your application please check your email or log-on FMDSS");
            return sb.ToString();
        }

        public Int64 SaveFavouritelink(Int64 UserID, string PageUrl, string PageName, int IsActive, int ModuleId, string StatementType)
        {
            try
            {
                DALConn();
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

        public DataSet GetAllRecords(Int64 UserID, string StatementType)
        {
            try
            {
                DALConn();
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
                DALConn();
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

        public void ForestFireApiLogs(DateTime runDateTime, int fatchRecoutCount)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_ForestFireAPILogs", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@apiRunDatetime", runDateTime);
                cmd.Parameters.AddWithValue("@fatchRecordCount", fatchRecoutCount);
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



        public void ErrorLog(string ErrorMsg, string FunctionName, int ModuleId, DateTime ErrorDate, Int64 EnteredBy)
        {
            try
            {
                DALConn();
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

        public enum Status
        {
            Default = 0,
            Success = 1,
            Error = 2

        }

        public DataTable Select_Range(Int64 UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "Select_Range_For_LoginUser", parameters);
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


        public DataTable Select_SSOIDbyRange(string Range)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@Range", Range) };
                Fill(dt, "Select_SSOIDbyRange", parameters);
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


        public DataTable Select_VillagesbyRange(string RangeCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@RangeCode", RangeCode) };
                Fill(dt, "Select_FDM_Village_For_LoginUser", parameters);
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

        public DataTable GetDDLBudgetServey(string VILL_CODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@VillageCode", VILL_CODE) };
                Fill(dt, "Sp_Select_FDM_BudgetServey", parameters);
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

        #region FRA
        public static string FRA_ClaimRequest_EmailBody(string UserName, long? reqID, string msg)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>" + msg + "</p>");
                    sb.Append("<br/>");
                    sb.Append("To view status of your application check your email or Log-on FMDSS");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");
                }
                //sb.Append("An application has been received for  " + PermissionDesc + " and the  requtest ID is #" + TransID + ".");
            }
            return sb.ToString();
        }

        public static string FRA_ClaimRequest_SMSBody(string UserName, long? reqID, string msg)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Dear " + UserName + ",");
            sb.Append(msg);
            sb.Append(" To view status of your application please log-on FMDSS.");
            return sb.ToString();
        }
        #endregion


        public static string Citizen_Parivad_EmailBody(string UserName, string TransID, string action)
        {
            StringBuilder sb = new StringBuilder();
            //   string click = "<a href=" + ConfigurationManager.AppSettings["emitraReturnUrl"].ToString() + ">click</a>";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    sb.Append("<div width='100%'>");
                    sb.Append("Dear " + UserName + ",");
                    sb.Append("<br />");
                    sb.Append("<p>Your request for the parivad vide no <b>#" + TransID + " has been " + action + ".</b></p>");
                    sb.Append("<br/>");
                    sb.Append("To view status of your application check your email or Log-on FMDSS");
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
        public static string Citizen_Parivad_SMSBody(string UserName, string TransID, string action)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Dear " + UserName + ",");
            sb.Append("Your request for the parivad vide no <b>#" + TransID + " has been " + action + ".");
            sb.Append("To view status of your application please log-on FMDSS.");
            return sb.ToString();
        }
        public static string Encrypt(string textToEncrypt, string EncryptionPassword)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(EncryptionPassword);
            pwdBytes = SHA256.Create().ComputeHash(pwdBytes);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }

        public static string Decrypt(string textToDecrypt, string EncryptionPassword)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(EncryptionPassword);
            pwdBytes = SHA256.Create().ComputeHash(pwdBytes);
            byte[] keyBytes = new byte[16];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            // rijndaelCipher.Padding = PaddingMode.PKCS7;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }



        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Function is responsible to calculate the Online ticketing fee
        /// </summary>
        /// <param name="ActualAmount"></param>
        /// <returns></returns>
        public static decimal CalculateFinalFeeForOnlineTicketing(decimal ActualAmount)
        {
            decimal FinalAmount = 0;
            try
            {
                decimal EmitraCommission = (ActualAmount * Convert.ToDecimal(2.25)) / 100;
                decimal ServiceTaxOnEmitraCommission = (EmitraCommission * Convert.ToDecimal(15)) / 100;
                FinalAmount = Math.Round(ActualAmount + EmitraCommission + ServiceTaxOnEmitraCommission, 2);
                if ((FinalAmount - Math.Truncate(FinalAmount)) > 0)
                    FinalAmount = Math.Floor(FinalAmount) + 1;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FinalAmount;
        }

        public DataTable AutoComplete(string Division, string District, string Block, string Gram, string Range, string Village, string Option)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_MasterDropdownAutofill", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Division", Division);
                cmd.Parameters.AddWithValue("@District", District);
                cmd.Parameters.AddWithValue("@Block", Block);
                cmd.Parameters.AddWithValue("@Gram", Gram);
                cmd.Parameters.AddWithValue("@Range", Range);
                cmd.Parameters.AddWithValue("@Village", Village);
                cmd.Parameters.AddWithValue("@option", Option);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "District" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

    }

    public static class Utility
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static DataTable ObjectToData(object obj)
        {
            DataTable dt = new DataTable("OutPutData");
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            obj.GetType().GetProperties().ToList().ForEach(f =>
            {
                try
                {
                    f.GetValue(obj, null);
                    dt.Columns.Add(f.Name, f.PropertyType);
                    dt.Rows[0][f.Name] = f.GetValue(obj, null);
                }
                catch { }
            });
            return dt;
        }
        public static System.Collections.IEnumerable DynamicSqlQuery(this Database database, string sql, params object[] parameters)
        {
            TypeBuilder builder = createTypeBuilder(
                    "MyDynamicAssembly", "MyDynamicModule", "MyDynamicType");

            using (System.Data.IDbCommand command = database.Connection.CreateCommand())
            {
                try
                {
                    database.Connection.Open();
                    command.CommandText = sql;
                    command.CommandTimeout = command.Connection.ConnectionTimeout;
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    using (System.Data.IDataReader reader = command.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();

                        foreach (System.Data.DataRow row in schema.Rows)
                        {
                            string name = (string)row["ColumnName"];
                            //var a=row.ItemArray.Select(d=>d.)
                            Type type = (Type)row["DataType"];
                            if (type != typeof(string) && (bool)row.ItemArray[schema.Columns.IndexOf("AllowDbNull")])
                            {
                                type = typeof(Nullable<>).MakeGenericType(type);
                            }
                            createAutoImplementedProperty(builder, name, type);
                        }
                    }
                }
                finally
                {
                    database.Connection.Close();
                    command.Parameters.Clear();
                }
            }

            Type resultType = builder.CreateType();

            return database.SqlQuery(resultType, sql, parameters);
        }

        private static TypeBuilder createTypeBuilder(
            string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void createAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
        public static string GenerateMyQCCode(string QCText, string RequestID, string FolderName)
        {
            string path = string.Empty;
            try
            {
                var QCwriter = new BarcodeWriter();
                QCwriter.Format = BarcodeFormat.QR_CODE;
                var result = QCwriter.Write(QCText);
                path = HttpContext.Current.Server.MapPath("~/" + FolderName + "/" + RequestID + ".jpg");
                var barcodeBitmap = new Bitmap(result);

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(path,
                       FileMode.Create, FileAccess.ReadWrite))
                    {
                        barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch(Exception ex)
            {
                path = "";
                new Common().ErrorLog(ex.Message, "GenerateMyQCCode" + "_" + "Comman", 0, DateTime.Now, 0);

            }
            return path;
            //imgageQRCode.Visible = true;
            //imgageQRCode.ImageUrl = "~/images/MyQRImage.jpg";
        }
    }
}