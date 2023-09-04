/*
 * Created By: Manoj Kumar
 * Created On: 24/09/2015
 * Description: This class will used for connect to the database and call the stored Procedure
 * 1. For save Data of Organising Camp
 * 2. For getting all data fron database
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CitizenService.PermissionService
{
    public class OrganisingCamps : DAL
    {
        #region Global Variables
        public Int64 CampId { get; set; }
        public string ApplicantType { get; set; }
        public string ApplicantCat { get; set; }
        public string RequestedId { get; set; }
        public string AgencyName { get; set; }
        public string AgencyAddress { get; set; }
        public string Contactperson { get; set; }
        public string ContactNumber { get; set; }
        public string ApplicantName { get; set; }
        public string Address { get; set; }
        public string Designation { get; set; }
        public string PostalCode { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public string DOB { get; set; }
        public string Ddlistrict { get; set; }
        public string DistrictName { get; set; }
        public string Location { get; set; }
        public string LocationName { get; set; }
        public string PurposeOfCamp { get; set; }
        public string Noofmembercamp { get; set; }
        public string IdProofType { get; set; }
        public string IdProofName { get; set; }
        public string IdProofNo { get; set; }
        public string FileNameOfIdProof { get; set; }
        public string PathofFileOfIdProof { get; set; }
        public DateTime Durationfrom { get; set; }
        public DateTime Durationto { get; set; }
        public string Noofdayscamp { get; set; }
        public string kioskuserid { get; set; }
        public string Processingfees { get; set; }
        public DateTime CreationDate { get; set; }
        public Int64 CreatedBy { get; set; }

        public Int64 IsActive { get; set; }
        public Int64 PlaceID { get; set; }
        public decimal MemberFees { get; set; }
        public string campType { get; set; }
        public Int32 TotalCamp { get; set; }
        public string Status { get; set; }
        public string Review { get; set; }

        // public string kioskuserid { get; set; }
        // public CrewMember crewMember { get; set; }


        ///change by arvind k Sharma 28/11/2016
        public decimal CampFee { get; set; }
        public decimal EmitraCharges { get; set; }
        public decimal Tax { get; set; }
        public decimal AmountTobePaid { get; set; }
       
        public decimal TentAmount { get; set; }
        public decimal IndianAdultFees_TigerProject { get; set; }
        public decimal IndianAdultFees_Surcharge { get; set; }
        public decimal ForeignerAdultFees_TigerProject { get; set; }
        public decimal ForeignerAdultFees_Surcharge { get; set; }
        public decimal StudentAdultFees_TigerProject { get; set; }
        public decimal StudentAdultFees_Surcharge { get; set; }

        public int CampAllowedPerDay { get; set; }
        public int MemberPerCamp { get; set; }

        ///change by arvind k Sharma 28/11/2016
                      
        #endregion

        /// <summary>
        /// Save data into database
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable CreateOrganizingCamp(DataTable dt)
        {
            DataTable dtR = new DataTable();
            string id = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Insert_Camp", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedId", Convert.ToInt64(HttpContext.Current.Session["RequestId"]));
                cmd.Parameters.AddWithValue("@ApplicantType", ApplicantType);
                cmd.Parameters.AddWithValue("@Ddlistrict", Ddlistrict == null ? (object)DBNull.Value : Ddlistrict);
                cmd.Parameters.AddWithValue("@Location", Location);
                cmd.Parameters.AddWithValue("@PurposeOfCamp", PurposeOfCamp);
                cmd.Parameters.AddWithValue("@Noofmembercamp", Noofmembercamp);
                cmd.Parameters.AddWithValue("@IdProofType", IdProofType);
                cmd.Parameters.AddWithValue("@IdProofNo", IdProofNo);
                cmd.Parameters.AddWithValue("@FileNameOfIdProof", FileNameOfIdProof);
                cmd.Parameters.AddWithValue("@PathofFileOfIdProof", PathofFileOfIdProof);
                cmd.Parameters.AddWithValue("@Durationfrom", Durationfrom);
                cmd.Parameters.AddWithValue("@Durationto", Durationto);
                cmd.Parameters.AddWithValue("@Noofdayscamp", Noofdayscamp);
                cmd.Parameters.AddWithValue("@Processingfees", Processingfees);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@IsActive", 1);
                cmd.Parameters.AddWithValue("@Status", 1);
                cmd.Parameters.AddWithValue("@campType", campType);
                cmd.Parameters.AddWithValue("@CampId", CampId);
                cmd.Parameters.AddWithValue("@TotalCamp", TotalCamp);
                cmd.Parameters.AddWithValue("@kioskuserid", kioskuserid);
                cmd.Parameters.AddWithValue("@CrewDetail", dt);
                cmd.Parameters.AddWithValue("@CampFee", CampFee);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtR);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Sp_Citizen_Insert_Camp" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtR;
        }

        /// <summary>
        /// Generate Request Id
        /// </summary>
        /// <returns></returns>

        private object GetRequestedId()
        {
            DateTime now = DateTime.Now;
            string id = now.Ticks.ToString();
            return id;
        }

        /// <summary>
        /// Fetch Camp Detail Data from Datbase
        /// </summary>
        /// <returns></returns>
        public DataTable BindOraganizingCamp()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_OCM", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "BindOraganizingCamp" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// For Fees of Organising Camp Permission
        /// </summary>
        /// <param name="Organizingcamp"></param>
        /// <returns></returns>
        public DataTable GetCampfeesByID(Int64 campID, string CampType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_ORGCamp_Fees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@campID", campID);
                cmd.Parameters.AddWithValue("@CampType", CampType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetCampfeesByID" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public string CheckCampAvailability()
        {
            string rp = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Check_CampAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CampID", CampId);
                cmd.Parameters.AddWithValue("@DurationFrom", Durationfrom);
                cmd.Parameters.AddWithValue("@DurationTo", Durationto);
                rp = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckCampAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return rp;
        }
        public DataTable CheckAvailableCamponSelectedDates()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetAvailableSeatsForCamping", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "CheckAvailableSeatsCamp");
                cmd.Parameters.AddWithValue("@Durationfrom", Durationfrom);
                cmd.Parameters.AddWithValue("@Durationto", Durationto);
                cmd.Parameters.AddWithValue("@CampArea", CampId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckAvailableCamponSelectedDates" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }





        /// <summary>
        /// For Fees of Organising Camp Permission
        /// </summary>
        /// <param name="Organizingcamp"></param>
        /// <returns></returns>
        public DataTable GetOrganizingCampfees(Int64 placeID, Int64 distID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_ORGCamp_Permission_Fees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@distID", distID);
                cmd.Parameters.AddWithValue("@placeID", placeID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetOrganizingCampfees" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Fetch District from Database
        /// </summary>
        /// <returns></returns>
        public DataTable getdistrct()
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_District", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "getdistrct" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataSet GetCampArea()
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Select_CampArea", Conn);
                cmd.Parameters.AddWithValue("@placeID", PlaceID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetCampArea" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }
    }
}