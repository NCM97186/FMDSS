using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.OnlineBooking
{
    public class CS_Ticket : DAL
    {
        private Int64 userID;
        private Int64 ticketID;
        private string requestID;
        private string category;
        private string districtID;
        private Int64 placeID;
        private DateTime arrivalDate;
        private string shiftTime;
        private string nationality;
        private int totalMember;
        private decimal feePerMember;
        private decimal cameraFee;
        private string memberType;
        private string name;
        private string gender;
        private string iDType;
        private string iDNo;
        private int totalCamera;
        private Int64 vehicleCatID;
        private string vehicleCategory;
        private Int64 vehicleID;
        private string vehicleName;
        private int availableSeat;
        private int seatForBooking;
        private decimal feePerVehicle;
        private decimal vehicleFeeTotal;
        private Int64 accomoID;
        private decimal roomCharge;
        private int totalRoom;
        private decimal roomAvailability;
        private Int64 enteredBy;
        private string transactionId;
        private int trn_Status_Code;
        private decimal totalAmount;
        private string date;

        

        public Int64 UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public Int64 TicketID
        {
            get { return ticketID; }
            set { ticketID = value; }
        }
        public string RequestID
        {
            get { return requestID; }
            set { requestID = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
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
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }
        public string ShiftTime
        {
            get { return shiftTime; }
            set { shiftTime = value; }
        }
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }
        public int TotalMember
        {
            get { return totalMember; }
            set { totalMember = value; }
        }
        public decimal FeePerMember
        {
            get { return feePerMember; }
            set { feePerMember = value; }
        }
        public decimal CameraFee
        {
            get { return cameraFee; }
            set { cameraFee = value; }
        }
        public string MemberType
        {
            get { return memberType; }
            set { memberType = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public string IDType
        {
            get { return iDType; }
            set { iDType = value; }
        }
        public string IDNo
        {
            get { return iDNo; }
            set { iDNo = value; }
        }
        public int TotalCamera
        {
            get { return totalCamera; }
            set { totalCamera = value; }
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
        public string VehicleName
        {
            get { return vehicleName; }
            set { vehicleName = value; }
        }
        public int AvailableSeat
        {
            get { return availableSeat; }
            set { availableSeat = value; }
        }
        public int SeatForBooking
        {
            get { return seatForBooking; }
            set { seatForBooking = value; }
        }
        public decimal FeePerVehicle
        {
            get { return feePerVehicle; }
            set { feePerVehicle = value; }
        }
        public decimal VehicleFeeTotal
        {
            get { return vehicleFeeTotal; }
            set { vehicleFeeTotal = value; }
        }
        public Int64 AccomoID
        {
            get { return accomoID; }
            set { accomoID = value; }
        }
        public decimal RoomCharge
        {
            get { return roomCharge; }
            set { roomCharge = value; }
        }
        public int TotalRoom
        {
            get { return totalRoom; }
            set { totalRoom = value; }
        }
        public decimal RoomAvailability
        {
            get { return roomAvailability; }
            set { roomAvailability = value; }
        }
        public Int64 EnteredBy
        {
            get { return enteredBy; }
            set { enteredBy = value; }
        }
        public decimal TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }
        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        public int Trn_Status_Code
        {
            get { return trn_Status_Code; }
            set { trn_Status_Code = value; }
        }
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        public DataTable Select_BookedTicket()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_BookedTicket", Conn);
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
        public DataTable GetVehicleType()
        {
            try
            {
                DataTable dt = new DataTable();
                Fill(dt, "Sp_Citizen_Get_vehicleCategory");
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
        public DataTable GetAccomodationType()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@placeID", PlaceID)
            };
                Fill(dt, "Sp_Citizen_SelectAccomodationType", parameters);
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
        public string CheckTicketAvailability()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Citizen_ChkTicketAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate);
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
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

        public DataTable Select_Shift_ByDistrict_Places()
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

        public DataTable SelectMemberFees()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Citizen_select_Ticket_Camera_Fees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                cmd.Parameters.AddWithValue("@Nationality", Nationality);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
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

        public DataTable Select_vehicle(Int64 VehicleCatID)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@PlaceID", PlaceID) 
            };
                Fill(dt, "Sp_Citizen_Select_vehicle_by_CatID", parameters);
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

        public DataTable Select_vehicle_Fees_Seat()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@VehicleID", VehicleID),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@PlaceID", PlaceID)
            };
                Fill(dt, "Sp_Citizen_Vehicle_Fees_Seat", parameters);
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

        public DataTable Select_Accomo_Fees_Availability()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@PlaceID", PlaceID)
            };
                Fill(dt, "Sp_Citizen_Accomo_Availability", parameters);
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

        public DataTable Submit_TicketDetails(DataTable dtm, DataTable dts)
        {
            try
            {
                DataTable dt = new DataTable();

                SqlParameter[] parameters =
            {    
            new SqlParameter("@RequestID", RequestID),
            new SqlParameter("@Category", Category),
            new SqlParameter("@DistrictID", DistrictID),
            new SqlParameter("@PlaceID", PlaceID),
            new SqlParameter("@ShiftTime", ShiftTime),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@TotalMembers", TotalMember),
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@TotalRoom", TotalRoom),
            new SqlParameter("@RoomCharge", RoomCharge),

            new SqlParameter("@MemberDetail", dtm),
            new SqlParameter("@SafariDetail", dts),
            new SqlParameter("@EnteredBy", EnteredBy)
            };
                // Int64 chk = Convert.ToInt64(ExecuteNonQuery("Sp_Citizen_BookTicket", parameters));
                Fill(dt, "Sp_Citizen_BookTicket", parameters);
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
        public void UpdateTransactionStatus(string option)
        {
            Int64 transId = Convert.ToInt64(TransactionId);
            SqlParameter[] parameters =
            {    
            new SqlParameter("@RequestedId", RequestID),
            new SqlParameter("@TransactionId",transId),
            new SqlParameter("@TransactionStatus", Trn_Status_Code),       
            new SqlParameter("@option", option)     
            };
            Int32 chk = Convert.ToInt32(ExecuteNonQuery("Sp_Common_UpdateTransactionStatus", parameters));
        }

        public DataSet Select_TicketData_For_Print()
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "Sp_Citizen_SelecTicketDetail", parameters);
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