using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace FMDSS.Models.ForestProtection
{
    public class AssignOffence : DAL
    {


        public string District { get; set; }
        public string UserRole { get; set; }
        public string OffenseCode { get; set; }
        public string OffensePlace { get; set; }
        public string OffenseDate { get; set; }
        public string OffenseTime { get; set; }
        public string OffenseDescription { get; set; }
        public int Status { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }
        public string Tehsil { get; set; }
        public string Naka { get; set; }
        public string Block { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DistanceFrmNaka { get; set; }
        public string OffenseCategory { get; set; }
        public string WildlifeProtection { get; set; }
        public string ForestProtection { get; set; }
        public string OffenseSeverity { get; set; }
        public string GPName { get; set; }
        public string Village { get; set; }
        public string OffenderType { get; set; }
        public string OffenderName { get; set; }
        public string OffenderFatherName { get; set; }
        public string OffenderCaste { get; set; }
        public string OffenderClothesWorn { get; set; }
        public string OffenderClothesColor { get; set; }
        public string OffenderPhysicalAppearance { get; set; }
        public string OffenderHeight { get; set; }
        public string OffenderOtherSpecialDetail { get; set; }
        public string OffenderPincode { get; set; }
        public string OffenderVillage { get; set; }
        public string OffenderDistrict { get; set; }
        public string OffenderEmailId { get; set; }
        public string OffenderAddress { get; set; }
        public string OffenderPhoneNo { get; set; }
        public string PoliceStation { get; set; }
        public string OffenderStatementDate { get; set; }
        public string OffenderStatement { get; set; }
        public string OffenderStatementDoc { get; set; }
        public string WitnessName { get; set; }
        public string WitnessFatherName { get; set; }
        public string WitnessCaste { get; set; }
        public string WitnessAddress { get; set; }
        public string WitnessVillage { get; set; }
        public string WitnessDistrict { get; set; }
        public string WitnessPincode { get; set; }
        public string WitnessPhoneNo { get; set; }
        public string WitnessIDType { get; set; }
        public string WitnessIDProofURL { get; set; }
        public string WitnessAge { get; set; }
        public string WitnessStatementDate { get; set; }
        public string WitnessStatement { get; set; }
        public string SignedStatementURL { get; set; }
        public string AssignDescription { get; set; }

        public string AssignTo { get; set; }
        public string AssignDate { get; set; }

        public DataSet GetViewExistingRecords()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_AssignOffenselist", Conn);
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
        public DataTable GetOfficerDesignation()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetFOfficerDesig", Conn);
                cmd.Parameters.AddWithValue("@option", "1");
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"]);
                cmd.Parameters.AddWithValue("@EmpDesig", "");
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

        public DataSet SubmitDFO_Forward(string ssoid)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_forwardOffenseAssignRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@ssoid", ssoid);
                cmd.Parameters.AddWithValue("@AssignedFrom", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Description", AssignDescription);
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

        public DataSet GetViewDetails(string OffenseCode, string UserRole)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetViewDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);
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

        public DataSet FPM_GetOffenceDetailByOffenceCode(string OffenseCode, string UserRole)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetOffenceDetailByOffenceCode", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                // cmd.Parameters.AddWithValue("@UserRole", UserRole);
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