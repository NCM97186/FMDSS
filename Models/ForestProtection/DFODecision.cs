using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace FMDSS.Models.ForestProtection
{
    public class DFODecision:DAL
    {
        #region Variable declaration
        public string option { get; set; }
        public string OffenseCode { get; set; }
        public string OffenseCategory { get; set; }
        public string OffenseSubCategory { get; set; }
        public string WildlifeCategory { get; set; }
        public string ForestCategory { get; set; }
        public string OffenceDescription { get; set; }
        public string SectionWildlife { get; set; }
        public string SectionForest { get; set; }
        public string OffenseSeverity { get; set; }
        public string SettlementAmount { get; set; }
        public string AmountPaid { get; set; }
        public string District { get; set; }
        public string OffenseName { get; set; }
        public string OffenseStatement { get; set; }
        public string OffenseDate { get; set; }
        public string OffenseTime { get; set; }
        public string OffensePlace { get; set; }
        public string Landmark { get; set; }
        public string DistanceFromNaka { get; set; }
        public string SeizedItem { get; set; }
        public string RegistrationNo { get; set; }
        public string Name { get; set; }
        public string FirstOfficer { get; set; }
        public string SecondOfficer { get; set; }
        public string WitnessName { get; set; }
        public string WitnessPhone { get; set; }
        public string WitnessAddress { get; set; }
        public string WitnessVillage { get; set; }
        public string WitnessStatement { get; set; }
        public string DFODecisionTaken { get; set; }
        public string CaseStatus { get; set; }
        public string FineAmount { get; set; }
        public string OffenderPresent { get; set; }
        public string OffenderDfoStatement { get; set; }
        public string UploadOffenderStatement { get; set; }
        public string OffenderAgreeCompoundingAmount { get; set; }
        public string ItemSeized { get; set; }
        public string Compounding { get; set; }
        public string Category { get; set; }
        public string Details { get; set; }
        public string Punishment { get; set; }
        public string Cognition { get; set; }
        public string Bailable { get; set; }

        #endregion


        public DataSet GetPunishment(DFODecision dfo)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Get_Punishment_Details", Conn);

                cmd.Parameters.AddWithValue("@OffenceCode", dfo.OffenseCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetPunishment" + "_" + "DFODecision", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// Get offense category
        /// </summary>
        /// <returns></returns>
        public DataSet GetOffenseCategory()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetFortectionDropdown", Conn);
                cmd.Parameters.AddWithValue("@option", option);
                cmd.Parameters.AddWithValue("@OffenseCategory", OffenseCategory);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                da.Fill(ds);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetOffenseCategory" + "_" + "DFODecision", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Final submision of dfo decision
        /// </summary>
        /// <returns></returns>
        public Int16 InsertDfoDecision()
        {
            Int16 chk=0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@OffenseCode", OffenseCode),     
                new SqlParameter("@DfoDecision",DFODecisionTaken),    
                new SqlParameter("@CaseStatus", CaseStatus),     
                new SqlParameter("@FineAmount", FineAmount), 
                new SqlParameter("@OffenderPresent", OffenderPresent), 
                new SqlParameter("@OffenderStatement", OffenderDfoStatement), 
                new SqlParameter("@OffenderStatementDoc", UploadOffenderStatement), 
                new SqlParameter("@PaymentOfCompoundingAmount", OffenderAgreeCompoundingAmount), 
                new SqlParameter("@ItemSeized", ItemSeized), 
                new SqlParameter("@Compounding", Compounding ),     
                new SqlParameter("@EnterBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),              
                };
                chk = Convert.ToInt16(ExecuteScalar("Sp_FPM_InsertDFODecision", parameters));               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertDfoDecision" + "_" + "DFODecision", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }                
            finally
            {
                Conn.Close();
            }
            return chk;
        }
    }
    public class CourtHearingDetail: DAL
    {
        public string OffenderName { get; set; }
        public string FatherName { get; set; }
        public string Index { get; set; }

        public string DIST_NAME { get; set; }
        public string AppreanceDate { get; set; }
        public string OffenseCode { get; set; }

        /// <summary>
        /// Get offense Hearing Details
        /// </summary>
        /// <returns></returns>
        public DataSet GetHearingDetails(Int64 UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_GET_HearingDetails", Conn);
                cmd.Parameters.AddWithValue("@AssignedOfficer", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetHearingDetails" + "_" + "DFODecision", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        
    }


}