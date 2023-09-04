using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class Places:DAL
    {
        public int Index { get; set; }
        public long PlaceID { get; set; }

        public string OperationType { get; set; } 
           
        public string DIST_CODE { get; set; }

        public string DisNAme { get; set; }

        [Required(ErrorMessage="Please enter place name")]
        public string PlaceName { get; set; }

        public string Category { get; set; }
        public string Code { get; set; }
        public int isZooAvailable { get; set; }

        public string MorningShiftFrom { get; set; }

        public string MorningShiftTo { get; set; }

        public string EveningShiftFrom { get; set; }

        public string EveningShiftTo { get; set; }

        public string FullDayShift { get; set; }

         
        
        public int TicketAllocatedPerShift { get; set; }
     

        public string IsAccommodation { get; set; }

        public string SingleOccupancy { get; set; }

        public string DoubleOccupancy { get; set; }

        public int IsSafari { get; set; }

        public string SafariAvailability { get; set; }

        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }


        public int IsZone { get; set; }

        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Boarding_Point { get; set; }

         public int IsOnlineBooking { get; set; }
         public int IsCamping { get; set; }
         public int IsResearch { get; set; }


         public bool IsOnlineBookingView { get; set; }
         public bool IsCampingView { get; set; }
         public bool IsResearchView { get; set; }
        


         public int MaxBookingDuration { get; set; }


         public int isMorning { get; set; }
         public int isEvening { get; set; }
         public int isFullDay { get; set; }

         public bool isMorningView { get; set; }
         public bool isEveningView { get; set; }
         public bool isFullDayView { get; set; }


         public int isDptKiosk { get; set; }
         public int isCitizen { get; set; }

        public decimal Tax { get; set; }
        public decimal EmitraCharges { get; set; }

        public void Placeauto()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_reset_autoincrement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteScalar();                
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

        public DataTable BindDistrict(string Division)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Select_Districts", Conn);
                cmd.Parameters.AddWithValue("@divCode", Division);
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


        public DataTable Select_Places()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AllPlaces", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlace");
                cmd.CommandType = CommandType.StoredProcedure;
               
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

        public DataTable Select_Place(int PlaceID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AllPlaces", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOnePlace");
                cmd.Parameters.AddWithValue("@PlaceId", PlaceID);
                cmd.CommandType = CommandType.StoredProcedure;
               
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

        public DataTable GETDivision()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
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

        public DataTable AddUpdatePlace(Places oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AllPlaces", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdatePlace");
                cmd.Parameters.AddWithValue("@DistrictID", oPlace.DIST_CODE);
                cmd.Parameters.AddWithValue("@PlaceId", oPlace.PlaceID);               
                cmd.Parameters.AddWithValue("@PlaceName", oPlace.PlaceName);
                cmd.Parameters.AddWithValue("@Category", oPlace.Category);
                cmd.Parameters.AddWithValue("@FullDayShift", oPlace.FullDayShift);

                cmd.Parameters.AddWithValue("@MorningShiftFrom", oPlace.MorningShiftFrom);
                cmd.Parameters.AddWithValue("@MorningShiftTo", oPlace.MorningShiftTo);
                cmd.Parameters.AddWithValue("@EveningShiftFrom", oPlace.EveningShiftFrom);
                cmd.Parameters.AddWithValue("@EveningShiftTo", oPlace.EveningShiftTo);
               
                cmd.Parameters.AddWithValue("@TicketAllocatedPerShift", oPlace.TicketAllocatedPerShift);
                
                cmd.Parameters.AddWithValue("@SafariAvailability", oPlace.SafariAvailability);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);
                
                cmd.Parameters.AddWithValue("@IsZone", oPlace.IsZone);
                cmd.Parameters.AddWithValue("@ContactPerson", oPlace.ContactPerson);
                cmd.Parameters.AddWithValue("@Address", oPlace.Address);
                cmd.Parameters.AddWithValue("@PhoneNo", oPlace.PhoneNo);
                cmd.Parameters.AddWithValue("@Boarding_Point", oPlace.Boarding_Point);

                cmd.Parameters.AddWithValue("@IsOnlineBooking", oPlace.IsOnlineBooking);
                cmd.Parameters.AddWithValue("@IsCamping", oPlace.IsCamping);
                cmd.Parameters.AddWithValue("@IsResearch", oPlace.IsResearch);
                cmd.Parameters.AddWithValue("@MaxBookingDuration", oPlace.MaxBookingDuration);

                cmd.Parameters.AddWithValue("@isMorning", oPlace.isMorning);
                cmd.Parameters.AddWithValue("@isEvening", oPlace.isEvening);
                cmd.Parameters.AddWithValue("@isFullDay", oPlace.isFullDay);

                cmd.Parameters.AddWithValue("@isDptKiosk", oPlace.isDptKiosk);
                cmd.Parameters.AddWithValue("@isCitizen", oPlace.isCitizen);
                cmd.Parameters.AddWithValue("@Tax", oPlace.Tax);
                cmd.Parameters.AddWithValue("@EmitraCharges", oPlace.EmitraCharges);
                cmd.Parameters.AddWithValue("@Code", oPlace.Code);
                cmd.Parameters.AddWithValue("@isZooAvailable", oPlace.isZooAvailable);

                cmd.CommandType = CommandType.StoredProcedure;

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

        public Int64 DeletePlace()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDP_delete_Place", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceID);
               // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
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
        public DataTable SelectPlaceCategory()
        {
            try
            {
                DALConn();
                DataTable dt=new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_PlaceCategory", Conn);
               // cmd.Parameters.AddWithValue("@PlaceId", PlaceID);
               // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
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

        public string Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_AllPlaces", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicatePlace");
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DistrictID", DIST_CODE);
                cmd.Parameters.AddWithValue("@PlaceName", PlaceName);
                cmd.Parameters.AddWithValue("@Category", Category);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                    return "0";
                else
                    return "1";
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

        public void UpdateMasterRecordStatus(Int64 ID, bool STATUS, string MasterType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UpdateMastersRecordStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tablename", MasterType);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@STATUS", STATUS);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

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