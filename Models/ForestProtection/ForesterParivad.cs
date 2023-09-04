using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForestProtection
{
    public class ForesterParivad:DAL
    {
        #region Global Variable
        public Int64 UserID { get; set; }
        public Int64 OffenseID { get; set; }
        public string OffenseCode { get; set; }
        public int RegFormNumber { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string Tehsil { get; set; }
        public string Naka { get; set; }
        public string ForestBlock { get; set; }
        public string Compartment { get; set; }
        public string OffensePlace { get; set; }
        public string Latitude { get; set; }
        public string OffenseDate { get; set; }
        public string OffenseTime { get; set; }
        public string Longitude { get; set; }
        public string LandMark { get; set; }
        public string NakaDistance { get; set; }
        public int ForestType { get; set; }
        public string OffenceCategory { get; set; }
        public string OffenseSubCategoryWildLife { get; set; }
        public string OffenseSubCategoryForest { get; set; }
        public string WildlifeProtectionSection { get; set; }
        public string ForestProtectionSection { get; set; }
        public string OffenseSeverity { get; set; }
        public string CrimeScenePhoto1 { get; set; }
        public string CrimeScenePhoto2 { get; set; }
        public string CrimeScenePhoto3 { get; set; }
        public string ParivadiName { get; set; }
      
        public string ActionStatus { get; set; }
        public bool IsEditMode { get; set; }
        public string UserRole { get; set; }
        public int Status { get; set; }
        public string Offence_Description { get; set; }
        public bool Self { get; set; }
        public bool Name { get; set; }
        public string ApplicantName { get; set; }
        public string ComplaintFound { get; set; }
        public string OffenseStatus { get; set; }
        public string TypeoFForest { get; set; }
        public string AssignTo { get; set; }
        public string AssignDate { get; set; }
        public string Complaint_Found { get; set; }
          public string Mokapunchnama { get; set; }
          public string Najri_Naksha { get; set; }
          public string Witness_Recorded1 { get; set; }
          public string Witness_Recorded2 { get; set; }
          public string Witness_Recorded3 { get; set; }
          public string List_of_ArticalSeized { get; set; }
          public string Recommendation { get; set; }
          public string FieldInspection { get; set; }
          public string InvestigationCompleteDate { get; set; }
          public string DispatchNo { get; set; }
          public string ForestOfficer { get; set; }
          public string Vill_Name { get; set; }
          public string GP_Name { get; set; }
          public string Range_Name { get; set; }
          public string Beat { get; set; }
          public int No_of_offender { get; set; }
          public string ComplaintOnBhalfOf { get; set; }
          public string Description_of_offenders { get; set; }

          public string VisitDate { get; set; }
          public string VisitPlace { get; set; }
          public string VehicleSeized { get; set; }
          public string FilesToBeUploaded { get; set; }

          public string CompoundAmount { get; set; }
          public string CompoundReceipt { get; set; }
          public string CompoundDate { get; set; }
          public string CompoundBudgetHead { get; set; }
          public string ChallanNo { get; set; }
          public string BankName { get; set; }
        #endregion

        #region File Court Case
          public long CaseId { get; set; }
          [Required(ErrorMessage = "Enter Court Name")]
          public string CourtName { get; set; }
          [Required(ErrorMessage = "Enter Court Case Number")]
          public string CourtCaseNo { get; set; }
          [Required(ErrorMessage = "Enter Court Type")]
          public string CourtType { get; set; }
          [Required(ErrorMessage = "Enter Court Place")]
          public string CourtPlace { get; set; }
          [Required(ErrorMessage = "Enter prosecution date")]
          public string ProsecutionDate { get; set; }
          [Required(ErrorMessage = "Enter decision taken")]
          public string DecisionTaken { get; set; }
          [Required(ErrorMessage = "Enter date of decision taken")]
          public string DateOfDecisionTaken { get; set; }
          public string ConvictionReason { get; set; }
          public string ReasonOfCaseFailed { get; set; }
          public string InterimOrder { get; set; }
          public string FinalJudgmentOrder { get; set; }

          #endregion


        /// <summary>
        /// function responsible for fetching circle division district of login forester
        /// /// </summary>
        /// <returns></returns>
        public DataTable GetCircle_Div_by_Member()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "Select_Circle_Div_Dist", parameters);
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
        /// Get list of range based on userid
        /// </summary>
        /// <returns></returns>
        public DataTable Get_Range_for_LoginUser()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID)
            };
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
        /// <summary>
        /// Get offense details 
        /// </summary>
        /// <returns></returns>
        public DataTable GetOffenseDetails()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@OffenseCode", ""),
            new SqlParameter("@Option", "1")
            };
                Fill(dt, "Sp_New_Fpm_OffenseStatus", parameters);
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
        /// Get details of offense on offense code
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet ViewOffenseDetails(string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@OffenseCode", OffenseCode),
            new SqlParameter("@Option", "2")
            };
                Fill(dt, "Sp_New_Fpm_OffenseStatus", parameters);
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
        /// Save the All form details in Database
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public Int64 SubmitForm1(ForesterParivad _objmodel)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_New_FPM_InsertForesterParivad", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Circle", _objmodel.CircleCode);
                cmd.Parameters.AddWithValue("@Division", _objmodel.DivisionCode);
                cmd.Parameters.AddWithValue("@District", _objmodel.DistrictCode);
                cmd.Parameters.AddWithValue("@Range", _objmodel.RangeCode);
                cmd.Parameters.AddWithValue("@Tehsil", _objmodel.Tehsil);
                cmd.Parameters.AddWithValue("@Naka", _objmodel.Naka);
                cmd.Parameters.AddWithValue("@Block", _objmodel.ForestBlock);
                cmd.Parameters.AddWithValue("@Compartment", _objmodel.Compartment);
                cmd.Parameters.AddWithValue("@OffensePlace", _objmodel.OffensePlace);
                cmd.Parameters.AddWithValue("@Latitude", _objmodel.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", _objmodel.Longitude);               
                cmd.Parameters.AddWithValue("@Description", _objmodel.Offence_Description);
                cmd.Parameters.AddWithValue("@OffenceCategory", _objmodel.OffenceCategory);
                cmd.Parameters.AddWithValue("@ApplicantName", _objmodel.ApplicantName);
                cmd.Parameters.AddWithValue("@OffenseDate", DateTime.ParseExact(_objmodel.OffenseDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@OffenseTime", _objmodel.OffenseTime);
                cmd.Parameters.AddWithValue("@ForestType", _objmodel.ForestType);
                cmd.Parameters.AddWithValue("@FilesToBeUploaded", _objmodel.FilesToBeUploaded);
                cmd.Parameters.AddWithValue("@AssignedOfficer", _objmodel.ForestOfficer);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);              
                cmd.Parameters.AddWithValue("@StatementType", "Insert");

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm1" + "_" + "SaveFPMOffenseForm1", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }
        /// <summary>
        /// Upadate the Offense Registration details in Database
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string UpdateForm1(ForesterParivad _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OffenseCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@Circle", _objmodel.CircleCode);
                cmd.Parameters.AddWithValue("@Division", _objmodel.DivisionCode);
                cmd.Parameters.AddWithValue("@District", _objmodel.DistrictCode);
                cmd.Parameters.AddWithValue("@Range", _objmodel.RangeCode);
                cmd.Parameters.AddWithValue("@Tehsil", _objmodel.Tehsil);
                cmd.Parameters.AddWithValue("@Naka", _objmodel.Naka);
                cmd.Parameters.AddWithValue("@Block", _objmodel.ForestBlock);
                cmd.Parameters.AddWithValue("@Compartment", _objmodel.Compartment);
                cmd.Parameters.AddWithValue("@OffensePlace", _objmodel.OffensePlace);
                cmd.Parameters.AddWithValue("@Latitude", _objmodel.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", _objmodel.Longitude);
                cmd.Parameters.AddWithValue("@OffenseDate", DateTime.ParseExact(_objmodel.OffenseDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@OffenseTime", _objmodel.OffenseTime);
                cmd.Parameters.AddWithValue("@Landmark", _objmodel.LandMark);
                cmd.Parameters.AddWithValue("@DistanceFromNaka", _objmodel.NakaDistance);
                cmd.Parameters.AddWithValue("@ForestType", _objmodel.ForestType);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@StatementType", "Update");

                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateForm1" + "_" + "UpdateFPMOffenseForm1", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";                     
        }

        /// <summary>
        /// Get officer designation  
        /// </summary>
        /// <returns></returns>
        public DataTable GetOfficerDesignation()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetFOfficerDesig", Conn);
                cmd.Parameters.AddWithValue("@option", "2");
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"]);
                cmd.Parameters.AddWithValue("@EmpDesig", "6");
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
        /// Get Offense details by OffenseCode
        /// </summary>
        /// <param name="objforesterParivad"></param>
        /// <returns></returns>
        public DataSet GetParivadeDetails(ForesterParivad objforesterParivad)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@OffenseCode", objforesterParivad.OffenseCode),
 
            };
                Fill(DS, "SP_Get_OffenseDetail_By_OffenseCode", parameters);
                return DS;
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
        /// Save the All form details in Database
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public Int32 SubmitCaseAction(string CaseAction, string Remarks,string OffenseCode)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertRO_Decision", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RODecision", CaseAction);
                cmd.Parameters.AddWithValue("@Offanse_Code", OffenseCode);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                Int32 chId = Convert.ToInt32(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm1" + "_" + "SaveFPMOffenseForm1", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        /// <summary>
        /// function to get list of offense for compounding 
        /// </summary>
        /// <returns></returns>
        public DataSet GetCompoundDetails()
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlParameter[] parameters =
                {    
                new SqlParameter("@Option", "1"),
                new SqlParameter("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString())),
                };
                Fill(DS, "Sp_FPM_GetCompoundDetails", parameters);
                return DS;
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
        /// Function foe bind program details
        /// </summary>
        /// <returns></returns>
        public DataSet BindBudget()
        {
            DataSet dsBudget = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@option","6"),                   
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),            
                };
                Fill(dsBudget, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindBudget" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsBudget;
        }

        /// <summary>
        /// function for submission of compounding
        /// </summary>
        /// <param name="FP"></param>
        /// <returns></returns>
        public Int32 SubmitCompounding(ForesterParivad FP)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetCompoundDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Option", "2");
                cmd.Parameters.AddWithValue("@Offense_code", FP.OffenseCode);
                cmd.Parameters.AddWithValue("@Amount", FP.CompoundAmount);
                cmd.Parameters.AddWithValue("@RecieptNo", FP.CompoundReceipt);
                cmd.Parameters.AddWithValue("@Date_of_Compounding", DateTime.ParseExact(FP.CompoundDate, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@BudgetHead", FP.CompoundBudgetHead);
                cmd.Parameters.AddWithValue("@ChallanNo", FP.ChallanNo);
                cmd.Parameters.AddWithValue("@BankName", FP.BankName);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                Int32 chId = Convert.ToInt32(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm1" + "_" + "SaveFPMOffenseForm1", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }  
        /// <summary>
        /// Get court case list
        /// </summary>
        /// <returns></returns>
        public DataSet GetCourtCaseDetails(string option)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_New_Fpm_FileCourtCase", Conn);
                cmd.Parameters.AddWithValue("@option", option);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetCourtCaseDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// Use for insert the File court vase
        /// </summary>
        /// <returns></returns>
        public Int64 InsertFileCourtCase()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {                       
                        new SqlParameter("@option","2"),        
                        new SqlParameter("@OffenseCode",OffenseCode),                                                                                 
                        new SqlParameter("@CourtName", CourtName ), 
                        new SqlParameter("@courtCaseNo", CourtCaseNo ), 
                        new SqlParameter("@CourtType", CourtType ), 
                        new SqlParameter("@CourtPlace", CourtPlace ), 
                        new SqlParameter("@ProsecutionDate", ProsecutionDate ), 
                        new SqlParameter("@DecisionTaken", DecisionTaken ), 
                        new SqlParameter("@DateOfDecisionTaken", DateOfDecisionTaken ), 
                        new SqlParameter("@ConvictionReason", ConvictionReason ), 
                        new SqlParameter("@ReasonOfCaseFailed", ReasonOfCaseFailed ), 
                        new SqlParameter("@InterimOrder", InterimOrder ), 
                        new SqlParameter("@FinalJudgmentOrder",FinalJudgmentOrder ), 
                        new SqlParameter("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserId"])),                                          
                        };
                chk = Convert.ToInt64(ExecuteScalar("Sp_New_Fpm_FileCourtCase", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertFileCourtCase" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
    }

    public class ViewDetails {

        #region Global Variable
      
        public Int64 OffenseID { get; set; }
        public string OffenseCode { get; set; }
     
        public string Circle { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string Tehsil { get; set; }
        public string Naka { get; set; }
        public string ForestBlock { get; set; }
        public string Compartment { get; set; }
        public string OffensePlace { get; set; }
        public string Latitude { get; set; }
        public string OffenseDate { get; set; }
        public string OffenseTime { get; set; }
        public string Longitude { get; set; }
        public string LandMark { get; set; }
        public string NakaDistance { get; set; }
        public int ForestType { get; set; }
        public string OffenceCategory { get; set; }
        public string OffenseSubCategoryWildLife { get; set; }
        public string OffenseSubCategoryForest { get; set; }
        public string WildlifeProtectionSection { get; set; }
        public string ForestProtectionSection { get; set; }
        public string OffenseSeverity { get; set; }
       
    
        public int Status { get; set; }
        public string Offence_Description { get; set; }         
        public string ApplicantName { get; set; }
        public string ComplaintFound { get; set; }
        public string OffenseStatus { get; set; }
        public string TypeoFForest { get; set; }
        public string AssignTo { get; set; }
        public string AssignDate { get; set; }
        public string Complaint_Found { get; set; }
        public string Mokapunchnama { get; set; }
        public string Najri_Naksha { get; set; }
        public string Witness_Recorded1 { get; set; }
        public string Witness_Recorded2 { get; set; }
        public string Witness_Recorded3 { get; set; }
        public string List_of_ArticalSeized { get; set; }
        public string Recommendation { get; set; }
        public string FieldInspection { get; set; }
        public string InvestigationCompleteDate { get; set; }
        public string DispatchNo { get; set; }
        public string ForestOfficer { get; set; }
        public string Vill_Name { get; set; }
        public string GP_Name { get; set; }
        public string Range_Name { get; set; }
        public string Beat { get; set; }
        public int No_of_offender { get; set; }
        public string ComplaintOnBhalfOf { get; set; }
        public string Description_of_offenders { get; set; }

        public string VisitDate { get; set; }
        public string VisitPlace { get; set; }
        public string VehicleSeized { get; set; }
        public string FilesToBeUploaded { get; set; }

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
    
    }
}