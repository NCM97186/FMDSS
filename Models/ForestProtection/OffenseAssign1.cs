using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace FMDSS.Models.ForestProtection
{
    public class OffenseAssign:DAL
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
        public string AssignDescription{ get; set; }
        public string fileUpload { get; set; }
               
        public DataSet GetViewExistingRecords()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetOffenseAssignlist", Conn);
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
                SqlCommand cmd = new SqlCommand("Sp_FPM_forwardOffenseRequest", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@ssoid", ssoid);
                cmd.Parameters.AddWithValue("@AssignedFrom", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Description", AssignDescription);
                cmd.Parameters.AddWithValue("@fileUpload", fileUpload);  
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

        public DataSet  FPM_GetOffenceDetailByOffenceCode(string OffenseCode, string UserRole)
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

        public class OffenceDetal
        {
            public string OffenseCode { get; set; }
            public string CrimePhotoURL1 { get; set; }
            public string CrimePhotoURL2 { get; set; }
            public string CrimePhotoURL3 { get; set; }
            public string IsCompoundable { get; set; }
            public string  UserRole { get; set; }
            public string SettlementAmount { get; set; }
            public string AmountPaid { get; set; }
            public string ApplicantName { get; set; }
            public string Block { get; set; }
            public string CIRCLE_NAME { get; set; }
            public string Description { get; set; }
            public string OffenseDate { get; set; }
            public string OffensePlace { get; set; }
            public string OffenseTime { get; set; }
            public string DIV_NAME { get; set; }
            public string DfoDecision { get; set; }
            public string CaseStatus { get; set; }
            public string FineAmount { get; set; }
            public string OffenderPresent { get; set; }
            public string ItemSeized { get; set; }
            public string Compounding { get; set; }
            public string SSOID { get; set; }
            public string District_name { get; set; }
        }
        public class WitnessDetail
        {
            public string OffenseCode { get; set; }
            public string WitnessName { get; set; }
            public string FatherName { get; set; }
            public string Caste { get; set; }
            public string Address1 { get; set; }
            public string PhoneNo { get; set; }
            public string EmailID { get; set; }
            public string IDType { get; set; }
            public string IDProofURL { get; set; }
            public string StatementDate { get; set; }
            public string SignedStatementURL { get; set; }
            public string WitnessStatement { get; set; }
        }

        public class OffenderDetail
        {
            public string OffenderType { get; set; }
            public string OffenseCode { get; set; }
            public string OffenderName { get; set; }
            public string FatherName { get; set; }
            public string Address1 { get; set; }
            public string EvidenceDocURL { get; set; }
            public string UEvidenceDocURL { get; set; }
            public string OffenderDescription { get; set; }
            public string PoliceStation { get; set; }
            public string WarrentIssued { get; set; }
            public string AppreanceDate { get; set; }
        }

        public class CompoundingDetails
        {
            public string OffenseCode { get; set; }
            public string IsCompoundable { get; set; }
            public string SettlementAmount { get; set; }
            public string AmountPaid { get; set; }
            public string CaseStatus { get; set; }
            public string FineAmount { get; set; }
            public string DfoDecision { get; set; }
            public string StatusDesc { get; set; }
            public string Status { get; set; }
        }
        public class SeizedDetails
        {
            public string ScientificName { get; set; }
            public string CommanName { get; set; }
            public string AnimalArticleName { get; set; }
            public string AnimalArticleDescription { get; set; }
            public string Quantity1 { get; set; }
       

            public string EquipmentName { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string Caliber { get; set; }
            public string IdentificationNo { get; set; }
            public string size { get; set; }
            public string Description { get; set; }
            public string Quantity2 { get; set; }

            public string SpeciesName { get; set; }
            public string ProduceType { get; set; }
            public string Quantity { get; set; }
            public string VehicleRegistrationNo { get; set; }
            public string OwnerName { get; set; }
            public string VehicleMake { get; set; }
            public string VehicleModel { get; set; }
            public string ChassisNo { get; set; }
            public string EngineNo { get; set; }
            public string PastOffenses { get; set; }
            public string CategoryName { get; set; }



            public string Name { get; set; }
            public string AnimalScientificName { get; set; }
            public string AnimalDescription { get; set; }
            public string AnimalWeight { get; set; }



            public string FilesToBeUploaded { get; set; }
            public Int64 ID { get; set; }
            public string OffenseCode { get; set; }
            public DateTime Date_Of_Visit { get; set; }

            public string sDate_Of_Visit { get; set; }
            public string PlaceOfVisit { get; set; }
            public string Description_of_Crime { get; set; }
            public string Pictures_of_Crime1 { get; set; }
            public string Pictures_of_Crime2 { get; set; }
            public string Pictures_of_Crime3 { get; set; }
            public string DIV_Code { get; set; }
            public string Village_Code { get; set; }
            public string Rang_Code { get; set; }
            public string DIV_Name { get; set; }
            public string Village_Name { get; set; }
            public string IsComplete { get; set; }
            public string Range_Name { get; set; }
            public Int64 EnteredBy { get; set; }
            public Int64 UserID { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public int Action { get; set; }


            public string Time_Of_Visit { get; set; }
        }
    }
}