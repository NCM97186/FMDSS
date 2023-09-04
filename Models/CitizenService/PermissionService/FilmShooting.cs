using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CitizenService.PermissionService
{
    public class FilmShooting : DAL
    {
        private string districtID;
        public string DistrictName { get; set; }
        private Int64 placeID;
        public string kioskuserid { get; set; }
        public string PlaceName { get; set; }
 
        private decimal indianMemberFees;
        private decimal nonIndianMemberFees;
        private decimal studentFees;
        private decimal discount;
        private decimal taxRate;
        private Int64 vehicleCatID;
        public string VehicleCatName { get; set; }
        private string vehicleCategory;
        private Int64 vehicleID;
        public string VehicleName { get; set; }
        private string vehicle;
        private Int64 totalVehicle;
        private decimal vehicleFees;
        private string transactionID;
        private string transactionStatus;

       
        public string RequestedId { get; set; }
        

        private int applicantType;
        public string ApplicantName { get; set; }
        private string title;
        private string description;
        private string noOfCrewMember;
        private DateTime durationFrom;
        private DateTime durationTo;
        private string shootingPurpose;
        private string identityProof;
        private string identityProofNo;
        private string iDProofUrl;
        private string districts;
        private string place;
        private Int64 indianCitizen;
        private Int64 nonIndianCitizen;
        private Int64 student;
        private decimal totalFees;
        private string filmCategory;
        private Int32 filmCategoryID;       
        private decimal depositeAmount;
        private Int64 enteredBy;

        public string identityProofType { get; set; }


        public int ApplicantType
        {
            get { return applicantType; }
            set { applicantType = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string NoOfCrewMember
        {
            get { return noOfCrewMember; }
            set { noOfCrewMember = value; }
        }
        public DateTime DurationFrom
        {
            get { return durationFrom; }
            set { durationFrom = value; }
        }
        public DateTime DurationTo
        {
            get { return durationTo; }
            set { durationTo = value; }
        }
        public string ShootingPurpose
        {
            get { return shootingPurpose; }
            set { shootingPurpose = value; }
        }
        public string IdentityProof
        {
            get { return identityProof; }
            set { identityProof = value; }
        }
        public string IdentityProofNo
        {
            get { return identityProofNo; }
            set { identityProofNo = value; }
        }
        public string IDProofUrl
        {
            get { return iDProofUrl; }
            set { iDProofUrl = value; }
        }
        public string Districts
        {
            get { return districts; }
            set { districts = value; }
        }
        public string Place
        {
            get { return place; }
            set { place = value; }
        }
        public Int64 IndianCitizen
        {
            get { return indianCitizen; }
            set { indianCitizen = value; }
        }
        public Int64 NonIndianCitizen
        {
            get { return nonIndianCitizen; }
            set { nonIndianCitizen = value; }
        }
        public Int64 Student
        {
            get { return student; }
            set { student = value; }
        }
        public decimal TotalFees
        {
            get { return totalFees; }
            set { totalFees = value; }
        }
        public string FilmCategory
        {
            get { return filmCategory; }
            set { filmCategory = value; }
        }
        public Int32 FilmCategoryID
        {
            get { return filmCategoryID; }
            set { filmCategoryID = value; }
        }
        public decimal DepositeAmount
        {
            get { return depositeAmount; }
            set { depositeAmount = value; }
        }
        public Int64 EnteredBy
        {
            get { return enteredBy; }
            set { enteredBy = value; }
        }
        public string TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }
        public string TransactionStatus
        {
            get { return transactionStatus; }
            set { transactionStatus = value; }
        }

        public string DistrictID
        {
            get { return districtID; }
            set { districtID = value; }
        }
        public Int64 PlaceID
        {
            get { return placeID; }
            set { placeID = value; }
        }
        public decimal IndianMemberFees
        {
            get { return indianMemberFees; }
            set { indianMemberFees = value; }
        }
        public decimal NonIndianMemberFees
        {
            get { return nonIndianMemberFees; }
            set { nonIndianMemberFees = value; }
        }
        public decimal StudentFees
        {
            get { return studentFees; }
            set { studentFees = value; }
        }
        public decimal Discount
        {
            get { return discount; }
            set { discount = value; }
        }
        public decimal TaxRate
        {
            get { return taxRate; }
            set { taxRate = value; }
        }
        public Int64 VehicleCatID
        {
            get { return vehicleCatID; }
            set { vehicleCatID = value; }
        }
        public string VehicleCategory
        {
            get { return vehicleCategory; }
            set { vehicleCategory = value; }
        }
        public Int64 VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }
        public string Vehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }
        public Int64 TotalVehicle
        {
            get { return totalVehicle; }
            set { totalVehicle = value; }
        }
        public decimal VehicleFees
        {
            get { return vehicleFees; }
            set { vehicleFees = value; }
        }
        public decimal MemberFee { get; set; }
        public Int64 CrewMemberid { get; set; }
        public Int64 CampId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Landmark { get; set; }
        public string PostalCode { get; set; }
        public int TypeID { get; set; }
        public string Gender { get; set; }
        public string CrewIDType { get; set; }
        public string CrewIDNo { get; set; }
        public string Nationality { get; set; }
        public string MemberType { get; set; }
        public decimal TotalDepositfees { get; set; }
        public int Trn_Status_Code { get; set; }

        public string TranscriptFile { get; set; }
        public string GOIFile { get; set; }
        public string UploadPhotoId { get; set; }



        public DataTable AutoSuggestDistrict(string Place, string Action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_District", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", Action);
                cmd.Parameters.AddWithValue("@Place", Place);
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

        /// <summary>
        /// Function to fetch ticket fees by district and place from database
        /// </summary>
        /// <returns></returns>
        public DataSet Select_Fees_ByDistrict_Places()
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_select_Shootingfees_By_District_Place", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Fees_ByDistrict_Places" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_FilmCategory_ByPlaces()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
    
                SqlCommand cmd = new SqlCommand("sp_Citizen_Select_FilmCategory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_FilmCategory_ByPlaces" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetVehicleType()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                Fill(dt, "Sp_Citizen_Get_vehicleCategory");
             
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetVehicleType" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

    
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_vehicle(Int64 VehicleCatID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VehicleCatID", VehicleCatID) 
            };
                Fill(dt, "Sp_Citizen_Select_vehicle_by_vehicleCatID", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_vehicle" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function to fetch ticket fees per vehicle from database
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Fees_Per_Vehicle()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_select_fees_per_Vehicle", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VehicleCatID", VehicleCatID);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Fees_Per_Vehicle" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_Fees_By_FilmCategory()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_Citizen_select_Film_SecurityDeposite", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FilmCategoryID", FilmCategoryID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Fees_By_FilmCategory" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Submit_FilmShootingPermission(DataTable dt, DataTable dtv)
        {
            DataTable dtR = new DataTable();
            try
            {
                DALConn();
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Insert_FilmShooting_Permission_Details", Conn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ApplicantType", ApplicantType);
                cmd.Parameters.AddWithValue("@RequestId", TransactionID);
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@NumberOfCrewMembers", NoOfCrewMember);
                cmd.Parameters.AddWithValue("@DurationFrom", DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", DurationTo);
                cmd.Parameters.AddWithValue("@TranscriptFile", TranscriptFile);
                cmd.Parameters.AddWithValue("@ShootingPurpose", ShootingPurpose);
                cmd.Parameters.AddWithValue("@IdentityProof", IdentityProof);
                cmd.Parameters.AddWithValue("@identityProofNo", IdentityProofNo);
                cmd.Parameters.AddWithValue("@IDProofUrl", IDProofUrl);
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID == null ? (object)DBNull.Value : DistrictID);

                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@IndianCitizen", IndianCitizen);
                cmd.Parameters.AddWithValue("@NonIndianCitizen", NonIndianCitizen);
                cmd.Parameters.AddWithValue("@Students", Student);
                cmd.Parameters.AddWithValue("@TotalFees", TotalFees);
                cmd.Parameters.AddWithValue("@FilmCategory", FilmCategory);
                cmd.Parameters.AddWithValue("@DepositeAmount", DepositeAmount);
                cmd.Parameters.AddWithValue("@crewMembers", dt);
                cmd.Parameters.AddWithValue("@vehicleFees", dtv);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy);
                cmd.Parameters.AddWithValue("@kioskuserid", Convert.ToInt64(kioskuserid));


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtR);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Submit_FilmShootingPermission" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dtR;
        }

        public DataTable UpdateTransactionStatus(string option)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(TransactionID);
           
                SqlCommand cmd = new SqlCommand("Sp_UpdateTransactionStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedId", TransactionID);
                cmd.Parameters.AddWithValue("@TransactionId", transId);
                cmd.Parameters.AddWithValue("@TransactionStatus", Trn_Status_Code);
                cmd.Parameters.AddWithValue("@option", option);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
         
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }
    }
}
