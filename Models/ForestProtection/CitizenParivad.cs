using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace FMDSS.Models.ForestProtection
{
    public class CitizenParivad:DAL
    {


        #region "ParivadRegistration"
        public long OffenseID { get; set; }
        public string OffenseCode { get; set; }
        public string District { get; set; }
        public long UserID { get; set; }
        public string UserRole { get; set; }

        public string OffenseCategory { get; set; }

        public string DateOfOffense { get; set; }
        public string TimeOfOffense { get; set; }

        public string OffensePlace { get; set; }

        public string DistrictID { get; set; }

        public string BlockCode { get; set; }

        public string GPCode { get; set; }

        public string VillageCode { get; set; }
        public string hdnDistCode { get; set; }
        public string hdnBlockCode { get; set; }
        public string hdnGPCode { get; set; }
        public string hdnVillageCode { get; set; }

        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }

        public string Description { get; set; }

        public string UploadEvidence { get; set; }
        public int NumberOfOffender { get; set; }

        public string OffenderDescription { get; set; }
        public string OffenseSeverity { get; set; }
        public string OffenceCategory { get; set; }
        public string OffenseSubCategoryWildLife { get; set; }
        public string OffenseSubCategoryForest { get; set; }
        public string WildlifeProtectionSection { get; set; }
        public string ForestProtectionSection { get; set; }

        #endregion

        #region "Offender Details"
        public long OffenderID { get; set; }

        public string OffenderType { get; set; }
        public string kioskuserid { get; set; }
        public string OffenderName { get; set; }

        public string OFatherName { get; set; }
     

        public string OAddress1 { get; set; }

        public string OAddress2 { get; set; }
        public string OStateCode { get; set; }
        public string txtdistrict { get; set; }    

        public string OVillageCode { get; set; }
        public string txtvillage { get; set; }

        public string ODistrictCode { get; set; }    

        public string EvidenceDocURL { get; set; }
        public string UEvidenceDocURL { get; set; }       

        public int ONumberOfOffender { get; set; }
        public string OffenderStatement { get; set; }
        #endregion

        #region Vechile
     
        public string Vechilerowid { get; set; }
        public string VechileRTO { get; set; }
        public string VechileRegistrationNo { get; set; }
        public string VechileOwnerName { get; set; }
        public string VechileType { get; set; }
        public string VechileMake { get; set; }
        public string VechileModel { get; set; }
        public string VechileChassisNo { get; set; }
        public string VechileEngineNo { get; set; }
        public string PastOffensesByVechile { get; set; }
        public string VechileUploadDoc { get; set; }
        #endregion

        public string MokaPunchnama { get; set; }
        public string NagriNaka { get; set; }
        public string WitnessRecord1 { get; set; }
        public string WitnessRecord2 { get; set; }
        public string WitnessRecord3 { get; set; }
        public string FieldInspection { get; set; }
        public string Recomendation { get; set; }
        public string ListOfItemSeized { get; set; }
        public string VisitDate { get; set; }
        public string VisitPlace { get; set; }
        public string ComplainFound { get; set; }
        public string VehicleSeized { get; set; }
        public string ForestType { get; set; }
        public string EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public string UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }


        /// <summary>
        /// Return list of state
        /// </summary>
        /// <returns></returns>
        public static IList<SelectListItem> DDLState()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "1", Text = "Rajasthan" });
            _result.Add(new SelectListItem { Value = "2", Text = "Other" });

            return _result;
        }

        /// <summary>
        /// return village code and name
        /// </summary>
        /// <param name="distcode"></param>
        /// <returns></returns>
        public DataTable GetVillage(string distcode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("select VILL_CODE,VILL_NAME from tbl_mst_Villages where DIST_CODE=" + distcode, Conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetForestOfficersDesignation" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// final submission of citizen registration
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <param name="dtOffender"></param>
        /// <returns></returns>
        public Int64 SubmitDetails(CitizenParivad _objmodel, DataTable dtOffender)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertUpdate_CitizenParivadRegistration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@OffenseCategory", Convert.ToInt32(_objmodel.OffenseCategory));
                cmd.Parameters.AddWithValue("@DateOfOffense", DateTime.ParseExact(_objmodel.DateOfOffense.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@TimeOfOffense", _objmodel.TimeOfOffense);
                cmd.Parameters.AddWithValue("@OffensePlace", _objmodel.OffensePlace);
                cmd.Parameters.AddWithValue("@DistrictID", _objmodel.DistrictID);
                cmd.Parameters.AddWithValue("@BlockCode", _objmodel.BlockCode);
                cmd.Parameters.AddWithValue("@GPCode", _objmodel.GPCode);
                cmd.Parameters.AddWithValue("@VillageCode", _objmodel.VillageCode);
                cmd.Parameters.AddWithValue("@Description", _objmodel.Description);
                cmd.Parameters.AddWithValue("@UploadEvidence", _objmodel.UploadEvidence);                
                cmd.Parameters.AddWithValue("@OffenderType", _objmodel.OffenderType);                
                cmd.Parameters.AddWithValue("@NumberOfOffender", _objmodel.ONumberOfOffender);
                cmd.Parameters.AddWithValue("@OffenderDescription", _objmodel.OffenderDescription);               
                cmd.Parameters.AddWithValue("@KioskUserId", Convert.ToInt64(_objmodel.kioskuserid));
                cmd.Parameters.AddWithValue("@Offenderinfo", dtOffender);             
                cmd.Parameters.AddWithValue("@Lattitude", _objmodel.Lattitude); 
                cmd.Parameters.AddWithValue("@Longitude", _objmodel.Longitude);
                cmd.Parameters.AddWithValue("@StatementType", "Insert");
                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.Message, "SubmitDetails" + "_" + "SaveOffenderDetails", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        /// <summary>
        /// Get the records
        /// </summary>
        /// <returns></returns>
        public DataSet GetViewExistingRecords()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_ViewOffenselist", Conn);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
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

        /// <summary>
        /// use for fetch the details of Offense Category
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet GetAllRecordsForm2(string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertParivadDetails_Form2", Conn);
                //cmd.Parameters.AddWithValue("@StatementType", "Select");
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
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

        /// <summary>
        /// Fetch the citizen offender details
        /// </summary>
        /// <param name="StatementType"></param>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet GetCitizenOffenderRecords(string StatementType, string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetOffenderDetails", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
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

        /// <summary>
        /// Upadate the records of form 2 by Offense Code
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string FinalSubmission(CitizenParivad _objmodel, DataTable dtOffender, DataTable dtVechile)
        {

            try
            {              
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_New_UpdatePrivadDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@type_of_Forest", _objmodel.ForestType);
                cmd.Parameters.AddWithValue("@Offense_Severity", _objmodel.OffenseSeverity);
                cmd.Parameters.AddWithValue("@OffenceCategory", _objmodel.OffenceCategory);            
                cmd.Parameters.AddWithValue("@Wildlife_Section", _objmodel.WildlifeProtectionSection);
                cmd.Parameters.AddWithValue("@Wildlife_Sub_Section", _objmodel.OffenseSubCategoryWildLife);
                cmd.Parameters.AddWithValue("@Forest_Section", _objmodel.ForestProtectionSection);
                cmd.Parameters.AddWithValue("@Forest_Sub_Section", _objmodel.OffenseSubCategoryForest);
                cmd.Parameters.AddWithValue("@OffenderKnown", _objmodel.OffenderType);
                cmd.Parameters.AddWithValue("@NoOfOffender", _objmodel.ONumberOfOffender);
                cmd.Parameters.AddWithValue("@OffenderDescription", _objmodel.OffenderDescription);  
                cmd.Parameters.AddWithValue("@VisitDate", DateTime.ParseExact(_objmodel.VisitDate.ToString(),"dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@VisitPlace", _objmodel.VisitPlace);
                cmd.Parameters.AddWithValue("@Complaint_Found", _objmodel.ComplainFound);
                cmd.Parameters.AddWithValue("@Mokapunchnama", _objmodel.MokaPunchnama);
                cmd.Parameters.AddWithValue("@Najri_Naksha", _objmodel.NagriNaka);
                cmd.Parameters.AddWithValue("@Witness_Recorded1", _objmodel.WitnessRecord1);
                cmd.Parameters.AddWithValue("@Witness_Recorded2", _objmodel.WitnessRecord2);
                cmd.Parameters.AddWithValue("@Witness_Recorded3", _objmodel.WitnessRecord3);
                cmd.Parameters.AddWithValue("@List_of_ArticalSeized", _objmodel.ListOfItemSeized);
                cmd.Parameters.AddWithValue("@Recommendation", _objmodel.Recomendation);
                cmd.Parameters.AddWithValue("@FieldInspection", _objmodel.FieldInspection);
                cmd.Parameters.AddWithValue("@VehicleSeized", _objmodel.VehicleSeized);
                cmd.Parameters.AddWithValue("@Offense_code", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@Offenderinfo", dtOffender);
                cmd.Parameters.AddWithValue("@SeizedVechileDetails",dtVechile);
                cmd.Parameters.AddWithValue("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm2" + "_" + "SaveFPMOffenseForm2", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }

        /// <summary>
        /// Ftech the records of Forestor
        /// </summary>
        /// <returns></returns>
        public DataSet GetSeizedVehicleDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_New_FPM_GetVehicleDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", HttpContext.Current.Session["FPMOffenseCode"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetForestDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

    }
    public class CitizenParivadMappingDetails
    {
        #region "Offender Details"
        public string OOffenderrowid { get; set; }
        public string OffenderType { get; set; }
        public string Offenderrowid { get; set; }
        public string OffenderName { get; set; }
        public string FatherName { get; set; }
        public string OFatherName { get; set; }
        public string OAddress1 { get; set; }

        public string OStateCode { get; set; }
        public string ODistrictCode { get; set; }
        public string OVillageCode { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string StateCode { get; set; }     
        public string VillageCode { get; set; }

        public string DistrictCode { get; set; }

        public string OffenderStatementDoc { get; set; }
        public string OffenderStatement { get; set; }
        public string EvidenceDocURL { get; set; }
         
        #endregion
    }

    public class KnownOffender
    {
        #region "Offender Details"
        public string OOffenderrowid { get; set; }
        public string OOffenderType { get; set; }
        public string OffenderName { get; set; }    
        public string OFatherName { get; set; }
        public string OAddress1 { get; set; }

        public string OStateCode { get; set; }
        public string ODistrictCode { get; set; }
        public string OVillageCode { get; set; }        
        public string OffenderStatement { get; set; }
        public string OffenderStatementDoc { get; set; }

        #endregion
    }

    public class CaseInvestigationStatus:DAL {


        public string InvestigationDate { get; set; }
        public string DispatchNo { get; set; }
        public string OffenseCode { get; set; }
        public string OffenseDate { get; set; }        
        public string OffensePlace { get; set; }
        public string OffenseDescription { get; set; }
        public string ComplaintFound { get; set; }

        /// <summary>
        /// Get the records
        /// </summary>
        /// <returns></returns>
        public Int64 SubmitInvestigationStatus(string Option)
        {
            try
            {
                DALConn();                
                SqlCommand cmd = new SqlCommand("Sp_New_Fpm_InvestigationStatus", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@InvestigationDate", DateTime.ParseExact(InvestigationDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@DispatchNo", DispatchNo);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
                cmd.Parameters.AddWithValue("@Option", Option);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 Status = Convert.ToInt64(cmd.ExecuteScalar());
                return Status;
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
        /// Get the records
        /// </summary>
        /// <returns></returns>
        public DataSet GetViewInvestigationStatus(string Option)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_New_Fpm_InvestigationStatus", Conn);             
                cmd.Parameters.AddWithValue("@Option", Option);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
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