using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.OnlineBooking 
{
    [Serializable]//add by rajveer load balanceing
    public class TicketBooking :DAL
    {

        #region Global Variables

        private Int64 rowID;
        private Int64 userID;
        private Int64 feeID;
        private Int64 districtID;
        private string districtName;
        private Int64 placeID;
        private string place;
        private string category;       
        private string action;
        private Int64 preferenceID;
        private string morningShift;
        private string morningShiftTo;
        private string eveningShift;
        private string eveningShiftTo;
        private string fullDayShift;
        private string ticketPerDay;
        private string isAccomodation;
        private string singleOccupancy;
        private string doubleOccupancy;
        private string isSafari;
        private string safariAvailability;
        private string preference;
        private Int64 memberTypeID;
        private string memberType;
        private Int64 preferenceCatID;
        public string totalPerson;
        private decimal amount;
        private decimal discountPercentage;
        private decimal totalFees;
        private Int64 totalTickets;
        private decimal finalAmount;
        private DateTime arrivalDate;

        private string shiftTime;
        private Int32 indianAdult;
        private decimal indianAdultFees;
        private Int32 indianChild;
        private decimal indianChildFees;
        private Int32 student;
        private decimal studentFees;      
        private Int32 foreignerAdult;
        private decimal foreignerAdultFees;
        private Int32 foreignerChild;
        private decimal foreignerChildFees;
        private Int32 guide;
        private decimal guideFees;
        private Int32 camera;
        private decimal cameraFees;
        private Int32 singleRoom;
        private decimal singleOccupancyFees;
        private Int32 doubleRoom;
        private decimal doubleOccupancyFees;
        private Int32 safari;
        private decimal safariFees;
        private decimal taxRate;

        private Int64 enteredBy;
        private Int64 updatedBy;
        public string kioskuserid { get; set; }
        #region Global Variables
        public Int64 RowID
        {
            get { return rowID; }
            set { rowID = value; }
        }
        public Int64 UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public Int64 FeeID
        {
            get { return feeID; }
            set { feeID = value; }
        }
        public Int64 DistrictID
        {
            get { return districtID; }
            set { districtID = value; }
        }
        public string DistrictName
        {
            get { return districtName; }
            set { districtName = value; }
        }
        public Int64 PlaceID
        {
            get { return placeID; }
            set { placeID = value; }
        }
        public string Place
        {
            get { return place; }
            set { place = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        public string Action
        {
            get { return action; }
            set { action = value; }
        }
        public Int64 PreferenceID
        {
            get { return preferenceID; }
            set { preferenceID = value; }
        }
        public Int64 MemberTypeID
        {
            get { return memberTypeID; }
            set { memberTypeID = value; }
        }
        public string MemberType
        {
            get { return memberType; }
            set { memberType = value; }
        }
        public Int64 PreferenceCatID
        {
            get { return preferenceCatID; }
            set { preferenceCatID = value; }
        }
        public string MorningShift
        {
            get { return morningShift; }
            set { morningShift = value; }
        }
        public string MorningShiftTo
        {
            get { return morningShiftTo; }
            set { morningShiftTo = value; }
        }
        public string EveningShift
        {
            get { return eveningShift; }
            set { eveningShift = value; }
        }
        public string EveningShiftTo
        {
            get { return eveningShiftTo; }
            set { eveningShiftTo = value; }
        }
        public string FullDayShift
        {
            get { return fullDayShift; }
            set { fullDayShift = value; }
        }
        public string TicketPerDay
        {
            get { return ticketPerDay; }
            set { ticketPerDay = value; }
        }
        public string IsAccomodation
        {
            get { return isAccomodation; }
            set { isAccomodation = value; }
        }
        public string SingleOccupancy
        {
            get { return singleOccupancy; }
            set { singleOccupancy = value; }
        }
        public string DoubleOccupancy
        {
            get { return doubleOccupancy; }
            set { doubleOccupancy = value; }
        }
        public string IsSafari
        {
            get { return isSafari; }
            set { isSafari = value; }
        }
        public string SafariAvailability
        {
            get { return safariAvailability; }
            set { safariAvailability = value; }
        }
        public string Preference
        {
            get { return preference; }
            set { preference = value; }
        }
        public string TotalPerson
        {
            get { return totalPerson; }
            set { totalPerson = value; }
        }
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public decimal DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }
        public decimal TotalFees
        {
            get { return totalFees; }
            set { totalFees = value; }
        }
        public decimal FinalAmount
        {
            get { return finalAmount; }
            set { finalAmount = value; }
        }
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }
        public Int64 TotalTickets
        {
            get { return totalTickets; }
            set { totalTickets = value; }
        }
        #endregion
        public string ShiftTime
        {
            get { return shiftTime; }
            set { shiftTime = value; }
        }
        public Int32 IndianAdult
        {
            get { return indianAdult; }
            set { indianAdult = value; }
        }
        public decimal IndianAdultFees
        {
            get { return indianAdultFees; }
            set { indianAdultFees = value; }
        }
        public Int32 IndianChild
        {
            get { return indianChild; }
            set { indianChild = value; }
        }
        public decimal IndianChildFees
        {
            get { return indianChildFees; }
            set { indianChildFees = value; }
        }
        public Int32 Student
        {
            get { return student; }
            set { student = value; }
        }
        public decimal StudentFees
        {
            get { return studentFees; }
            set { studentFees = value; }
        }
        public Int32 ForeignerAdult
        {
            get { return foreignerAdult; }
            set { foreignerAdult = value; }
        }
        public decimal ForeignerAdultFees
        {
            get { return foreignerAdultFees; }
            set { foreignerAdultFees = value; }
        }
        public Int32 ForeignerChild
        {
            get { return foreignerChild; }
            set { foreignerChild = value; }
        }
        public decimal ForeignerChildFees
        {
            get { return foreignerChildFees; }
            set { foreignerChildFees = value; }
        }
        public Int32 Guide
        {
            get { return guide; }
            set { guide = value; }
        }
        public decimal GuideFees
        {
            get { return guideFees; }
            set { guideFees = value; }
        }
        public Int32 Camera
        {
            get { return camera; }
            set { camera = value; }
        }
        public decimal CameraFees
        {
            get { return cameraFees; }
            set { cameraFees = value; }
        }
        public Int32 SingleRoom
        {
            get { return singleRoom; }
            set { singleRoom = value; }
        }
        public Int32 DoubleRoom
        {
            get { return doubleRoom; }
            set { doubleRoom = value; }
        }
        public decimal SingleOccupancyFees
        {
            get { return singleOccupancyFees; }
            set { singleOccupancyFees = value; }
        }
        public decimal DoubleOccupancyFees
        {
            get { return doubleOccupancyFees; }
            set { doubleOccupancyFees = value; }
        }
        public Int32 Safari
        {
            get { return safari; }
            set { safari = value; }
        }
        public decimal SafariFees
        {
            get { return safariFees; }
            set { safariFees = value; }
        }
        public decimal TaxRate
        {
            get { return taxRate; }
            set { taxRate = value; }
        }

        public Int64 EnteredBy
        {
            get { return enteredBy; }
            set { enteredBy = value; }
        }
        public Int64 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public string TransactionId { get; set; }
        public int Trn_Status_Code { get; set; }

        public string isMorning { get; set; }
        public string isEvening { get; set; }
        public string isFullDay { get; set; }

        #endregion

        public DataTable District()
        {
            SqlCommand cmd = new SqlCommand("Sp_Common_Select_District", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public Int64 SubmitPlaces()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Insert_Place_With_Shift", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@MorningShift", MorningShift == null ? (object)DBNull.Value : MorningShift);
                cmd.Parameters.AddWithValue("@MorningShiftTo", MorningShiftTo == null ? (object)DBNull.Value : MorningShiftTo);
                cmd.Parameters.AddWithValue("@EveningShift", EveningShift == null ? (object)DBNull.Value : EveningShift);
                cmd.Parameters.AddWithValue("@EveningShiftTo", EveningShiftTo == null ? (object)DBNull.Value : EveningShiftTo);
                cmd.Parameters.AddWithValue("@FullDayShift", FullDayShift);
                cmd.Parameters.AddWithValue("@TicketPerDay", TicketPerDay);
                cmd.Parameters.AddWithValue("@IsAccomodation", IsAccomodation == null ? (object)DBNull.Value : IsAccomodation);
                cmd.Parameters.AddWithValue("@SingleOccupancy", SingleOccupancy == null ? (object)DBNull.Value : SingleOccupancy);
                cmd.Parameters.AddWithValue("@DoubleOccupancy", DoubleOccupancy == null ? (object)DBNull.Value : DoubleOccupancy);
                cmd.Parameters.AddWithValue("@IsSafari", IsSafari == null ? (object)DBNull.Value : IsSafari);
                cmd.Parameters.AddWithValue("@SafariAvailability", SafariAvailability == null ? (object)DBNull.Value : SafariAvailability);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy);
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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

        public Int64 UpdatePlaces()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Update_Places_With_Shift", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@MorningShift", MorningShift == null ? (object)DBNull.Value : MorningShift);
                cmd.Parameters.AddWithValue("@MorningShiftTo", MorningShiftTo == null ? (object)DBNull.Value : MorningShiftTo);
                cmd.Parameters.AddWithValue("@EveningShift", EveningShift == null ? (object)DBNull.Value : EveningShift);
                cmd.Parameters.AddWithValue("@EveningShiftTo", EveningShiftTo == null ? (object)DBNull.Value : EveningShiftTo);
                cmd.Parameters.AddWithValue("@FullDayShift", FullDayShift);
                cmd.Parameters.AddWithValue("@TicketPerDay", TicketPerDay);
                cmd.Parameters.AddWithValue("@IsAccomodation", IsAccomodation == null ? (object)DBNull.Value : IsAccomodation);
                cmd.Parameters.AddWithValue("@SingleOccupancy", SingleOccupancy == null ? (object)DBNull.Value : SingleOccupancy);
                cmd.Parameters.AddWithValue("@DoubleOccupancy", DoubleOccupancy == null ? (object)DBNull.Value : DoubleOccupancy);
                cmd.Parameters.AddWithValue("@IsSafari", IsSafari == null ? (object)DBNull.Value : IsSafari);
                cmd.Parameters.AddWithValue("@SafariAvailability", SafariAvailability == null ? (object)DBNull.Value : SafariAvailability);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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

        public DataTable Select_Places()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Select_Places", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable Select_Places_ByDistrict()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_Places_by_DistrictID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public Int64 DeletePlaces()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Delete_Places", Conn);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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


        public Int64 SubmitFees()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Inset_Ticket_Fees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@IndianAdultFees", IndianAdultFees == null ? (object)DBNull.Value : IndianAdultFees);
                cmd.Parameters.AddWithValue("@IndianChildFees", IndianChildFees == null ? (object)DBNull.Value : IndianChildFees);
                cmd.Parameters.AddWithValue("@ForeignerAdultFees", ForeignerAdultFees == null ? (object)DBNull.Value : ForeignerAdultFees);
                cmd.Parameters.AddWithValue("@ForeignerChildFees", ForeignerChildFees == null ? (object)DBNull.Value : ForeignerChildFees);
                cmd.Parameters.AddWithValue("@GuideFees", GuideFees == null ? (object)DBNull.Value : GuideFees);
                cmd.Parameters.AddWithValue("@CameraFees", CameraFees == null ? (object)DBNull.Value : CameraFees);
                cmd.Parameters.AddWithValue("@SingleOccupancyFees", SingleOccupancyFees == null ? (object)DBNull.Value : SingleOccupancyFees);
                cmd.Parameters.AddWithValue("@DoubleOccupancyFees", DoubleOccupancyFees == null ? (object)DBNull.Value : DoubleOccupancyFees);
                cmd.Parameters.AddWithValue("@SafariFees", SafariFees == null ? (object)DBNull.Value : SafariFees);
                cmd.Parameters.AddWithValue("@Discount", DiscountPercentage == null ? (object)DBNull.Value : DiscountPercentage);
                cmd.Parameters.AddWithValue("@TaxRate", TaxRate == null ? (object)DBNull.Value : TaxRate);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy);
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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

        public DataTable Select_Fees()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Select_TicketFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable Select_Fees_ByDistrict_Places()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Citizen_select_fees_By_District_Place", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable Select_BookedTicket()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_BookedTicked", Conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public DataTable Get_CategorywiseDistrict()
        {
            SqlCommand cmd = new SqlCommand("Sp_Citizen_GetCategorywiseDistrict", Conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Category", Category);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        } 
        public string CheckTicketAvailability()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Citizen_CheckTicketAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate);
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                return Convert.ToString(cmd.ExecuteScalar());
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
        public Int64 SubmitTicketDetails()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Citizen_Insert_TicketBooking_Details", Conn);   
                cmd.CommandType = CommandType.StoredProcedure;                           
                cmd.Parameters.AddWithValue("@RequestedId", TransactionId);              
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime == null ? (object)DBNull.Value : ShiftTime);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate == null ? (object)DBNull.Value : ArrivalDate);
                cmd.Parameters.AddWithValue("@TotalMembers", TotalPerson == null ? (object)DBNull.Value : TotalPerson);
                cmd.Parameters.AddWithValue("@IndianAdult", IndianAdult == null ? (object)DBNull.Value : IndianAdult);
                cmd.Parameters.AddWithValue("@IndianChild", IndianChild == null ? (object)DBNull.Value : IndianChild);
                cmd.Parameters.AddWithValue("@Student", Student == null ? (object)DBNull.Value : Student);
                cmd.Parameters.AddWithValue("@ForeignerAdult", ForeignerAdult == null ? (object)DBNull.Value : ForeignerAdult);
                cmd.Parameters.AddWithValue("@ForeignerChild", ForeignerChild == null ? (object)DBNull.Value : ForeignerChild);
                cmd.Parameters.AddWithValue("@NumberOfGuide", Guide == null ? (object)DBNull.Value : Guide);
                cmd.Parameters.AddWithValue("@NumberOfCamera", Camera == null ? (object)DBNull.Value : Camera);
                cmd.Parameters.AddWithValue("@SingleOccupancy", SingleRoom == null ? (object)DBNull.Value : SingleRoom);
                cmd.Parameters.AddWithValue("@DoubleOccupancy", DoubleRoom == null ? (object)DBNull.Value : DoubleRoom);
                cmd.Parameters.AddWithValue("@SafariMembers", Safari == null ? (object)DBNull.Value : Safari);
                cmd.Parameters.AddWithValue("@TotalFees", TotalFees == null ? (object)DBNull.Value : TotalFees);
                cmd.Parameters.AddWithValue("@Discount", DiscountPercentage == null ? (object)DBNull.Value : DiscountPercentage);
                cmd.Parameters.AddWithValue("@TaxRate", TaxRate == null ? (object)DBNull.Value : TaxRate);
                cmd.Parameters.AddWithValue("@AmountTobePaid", Amount == null ? (object)DBNull.Value : Amount);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy);
                cmd.Parameters.AddWithValue("@kioskuserid",Convert.ToInt64(kioskuserid));
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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


        public void UpdateTransactionStatus(string option)
        {
            Int64 transId = Convert.ToInt64(TransactionId);
            SqlParameter[] parameters =
            {    
            new SqlParameter("@RequestedId", TransactionId),
            new SqlParameter("@TransactionId",transId),
            new SqlParameter("@TransactionStatus", Trn_Status_Code),       
            new SqlParameter("@option", option)     
            };
            Int32 chk = Convert.ToInt32(ExecuteNonQuery("Sp_Common_UpdateTransactionStatus", parameters));
        }


        public DataTable Select_BookindTicketsDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Slect_ticketBooking_Details", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", EnteredBy);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
    }
}