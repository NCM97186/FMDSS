using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FMDSS.Models.BookOnlineZoo
{
    public class BookOnzooMobileApp : DAL
    {

        public string BookingType { get; set; }
        public string RequestId { get; set; }

        [Required(ErrorMessage = "Enter Institute/Organisation name")]
        public string InstituteOrganisationName { get; set; }

        [Required(ErrorMessage = "Enter address of institute/organisation")]
        public string AddressOfInstitOrgan { get; set; }

        [Required(ErrorMessage = "Enter phone no of institute/organisation")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNoOfInstitOrgan { get; set; }

        [Required(ErrorMessage = "Enter name of head")]
        public string NameOfHead { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneOfHead { get; set; }

        [Required(ErrorMessage = "Upload document of tour")]
        public string DocumentForTour { get; set; }

        [Required(ErrorMessage = "Select type of Id")]
        public string IdType { get; set; }

        [Required(ErrorMessage = "Enter Id Number")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Upload valid Id")]
        public string UploadId { get; set; }

        [Required(ErrorMessage = "Select place of visit")]
        public string PlaceOfVisit { get; set; }

        [Required(ErrorMessage = "Select date of visit")]
        public string DateOfVisit { get; set; }

        [Required(ErrorMessage = "Enter valid Captcha")]
        public string txtInput { get; set; }

        public bool AdultIndianMember { get; set; }
        public bool AdultNonIndianMember { get; set; }
        public bool Student { get; set; }
        public bool ChildBelowAgeFive { get; set; }

        public string IPAddress { get; set; }
        public string IPAddressAndDeviceKey { get; set; }

        public string KioskUserId { get; set; }


        #region printTicket
        public Int64 TicketID { get; set; }
        public decimal TotalAmount { get; set; }
        public string DateOfArrival { get; set; }

        public int TotalMember { get; set; }
        #endregion

        #region MemberDetails
        public string MSLNo { get; set; }
        public string TypeOfMember { get; set; }
        public string FeePerMember { get; set; }
        public string NoOfMember { get; set; }
        public string NoOfCamera { get; set; }
        public string FeePerCamera { get; set; }



        public string NoOfStillCamera { get; set; }
        public string FeePerStillCamera { get; set; }

        public string NoOfVideoCamera { get; set; }
        public string FeePerVideoCamera { get; set; }

        public string TotalFeesOfMember { get; set; }
        public string EmitraTransactionId { get; set; }

        public int Trn_Status_Code { get; set; }
        //   public List<MemberInformation> lstMember { get; set; }
        #endregion



        #region MemberDetails

        public bool PrivateVehicle { get; set; }
        public string VSLNo { get; set; }
        public string TypeOfVehicle { get; set; }
        public string FeePerVehicle { get; set; }
        public string NoOfVehicle { get; set; }
        public string TotalVehicleFee { get; set; }

        //  public List<VehicleInformation> lstVehicle { get; set; }

        #endregion

        #region Vehicle
        public string NoOfBus { get; set; }
        public string NoOfJeepCarMotorMiniBus { get; set; }
        public string NoOfTwoWheeler { get; set; }
        public string NoOfAutoRikshaw { get; set; }

        public string TotalFeesOfBus { get; set; }
        public string TotalFeesOfJeepCarMotorMiniBus { get; set; }
        public string TotalFeesOfTwoWheeler { get; set; }
        public string TotalFeesOfAutoRikshaw { get; set; }
        #endregion

        #region Fees details

        public string MemberEntryFees { get; set; }
        public string CameraFees { get; set; }
        public string VehicleFees { get; set; }
        public string OnlineBookingCharges { get; set; }
        public string TotalPayableCharges { get; set; }

        #endregion






        /// <summary>
        /// Function return places
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Place(Int64 UserId)
        {
            DataTable dtPlaces = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Zoo_GetPlace", Conn);
                cmd.Parameters.AddWithValue("@Option", 1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPlaces);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, UserId);
            }
            finally
            {
                Conn.Close();
            }
            return dtPlaces;
        }

        /// <summary>
        /// Function return MemberVehicleDetails
        /// </summary>
        /// <returns></returns>
        public DataSet MemberVehicleDetails(Int64 UserId)
        {
            DataSet dsMemVeh = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_MemberVehicleDetails", Conn);
                cmd.Parameters.AddWithValue("@PlaceID", Convert.ToInt32(PlaceOfVisit));
                cmd.Parameters.AddWithValue("@Option", 2);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMemVeh);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, UserId);
            }
            finally
            {
                Conn.Close();
            }
            return dsMemVeh;
        }

        public DataTable CheckTicketAvailability(int PlaceId, int ShiftType, string VisitDate, Int64 UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Zoo_ChkTicketAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.Parameters.AddWithValue("@DateofVisit", DateTime.ParseExact(VisitDate, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_ChkTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, UserID);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// final submission of form
        /// </summary>
        /// <param name="dtm"></param>
        /// <param name="finalAmount"></param>
        /// <returns></returns>
        public DataTable Submit_ZooDetails(DataTable dtMember, DataTable dtVehicle)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {              
            new SqlParameter("@BookingTypeId", BookingType),  
            new SqlParameter("@PlaceId",PlaceOfVisit),
            new SqlParameter("@Institutional_NameofInstitute", InstituteOrganisationName),
            new SqlParameter("@Institutional_AddressofInstitute", AddressOfInstitOrgan),
            new SqlParameter("@Institutional_PhoneofInstitute", PhoneNoOfInstitOrgan),
            new SqlParameter("@Institutional_NameofHead", NameOfHead),
            new SqlParameter("@Institutional_HeadPhoneNo", PhoneOfHead),
            new SqlParameter("@Institutional_DocumentofTour", DocumentForTour),          
            new SqlParameter("@HeadIdType", IdType),
            new SqlParameter("@HeadIdNumber", IdNumber),
            new SqlParameter("@Institutional_IDProfofGroupHead", UploadId),                       
            new SqlParameter("@Institutional_DateofVisit", DateTime.ParseExact(DateOfVisit, "dd/MM/yyyy", null)),
            new SqlParameter("@PrivateVehicle",PrivateVehicle),
            new SqlParameter("@MemberDetail", dtMember),
            new SqlParameter("@VehicleDetail", dtVehicle),    
            new SqlParameter("@IP_Address", IPAddress),    
            new SqlParameter("@EnteredBy",HttpContext.Current.Session["UserID"].ToString()),    
            new SqlParameter("@kioskUserId", Convert.ToInt64(KioskUserId)),

            };
                Fill(dt, "SP_Zoo_Booking", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_Booking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// function to update transaction status
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public int UpdateTransactionStatus(string option, double EmitraAmount = 0)
        {
            DataTable dt = new DataTable();
            Int32 chk = 0;
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {    
                new SqlParameter("@RequestedId", RequestId),
                new SqlParameter("@TransactionId",transId),
                new SqlParameter("@TransactionStatus", Trn_Status_Code),       
                new SqlParameter("@option", option),
                new SqlParameter("@EmitraAmount", EmitraAmount)
                };
                Fill(dt, "SP_Zoo_UpdateTransactionStatus", parameters);
                if (dt.Rows.Count > 0)
                {
                    chk = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        public DataTable Select_BookedTicket()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Zoo_PrintBookedTicket", Conn);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Ticket print
        /// </summary>
        /// <returns></returns>
        public DataSet Select_TicketData_For_Print()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {    
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "Sp_Zoo_SelecTicketDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_Print" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataTable Get_BookedTicketDetails(string RequestID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Get_ZooBookedTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookedTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

    }

    public class BookOnzooMobileAppData
    {
        public List<BookOnzooMobileApp> ticketList { get; set; }
        public List<SelectListItem> lstPlace { get; set; }
        public List<BookOnzooMobileApp> lstMember { get; set; }
        public List<BookOnzooMobileApp> lstVehicle { get; set; }
        public List<SelectListItem> lstShiftType { get; set; }

        public List<SelectListItem> lstIDType { get; set; }
    }



}