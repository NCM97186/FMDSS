using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.BookingRoaster
{
	public class BookingRoasterNew:DAL
	{
		public DataTable GetPlaceList(string SSOid,string VisitDate="")
		{
			try
			{
				DALConn();
				DataTable DS = new DataTable();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetPlaceList");
				cmd.Parameters.AddWithValue("@SSOid", SSOid);
				cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
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
		public List<CitizenVisitDetail> GetCitizenTicketList(string SSOid, string VisitDate = "")
		{
			List<CitizenVisitDetail> list = new List<CitizenVisitDetail>();
			try
			{
				DALConn();
				DataSet DS = new DataSet();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetCitizenTicket");
				cmd.Parameters.AddWithValue("@SSOid", SSOid);
				cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
				list= Globals.Util.GetListFromTable<CitizenVisitDetail>(DS, 0).ToList();
				return list;
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
		
		public DataTable GetVehicleMaxSeats(int PlaceId,  int VehicleType, int ShiftId,int ZoneId)
		{
			try
			{
				DALConn();
				DataTable DS = new DataTable();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetVehicleMaxSeats");
				cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
				cmd.Parameters.AddWithValue("@ShiftId", ShiftId);
				cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
				cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
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
		public DataSet GetSavedGuideList(int PlaceId ,string VisitDate,int VehicleId,int VehicleType,int ShiftId, int ZoneId, int MemberCount,string RequestId)
		{
			try
			{
				DALConn();
                DataSet DS = new DataSet();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetSavedGuideList");
				cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
				cmd.Parameters.AddWithValue("@VehicleId", VehicleId);
				cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
				cmd.Parameters.AddWithValue("@ShiftId", ShiftId);
				cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
				cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@MemberCount", MemberCount);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                
                cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
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
		public DataSet GetSavedVehicleList(int PlaceId, string VisitDate, int GuideId, int VehicleType, int ShiftId,int ZoneId, int MemberCount,string RequestId)
		{
			try
			{
				DALConn();
                DataSet DS = new DataSet();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetSavedVehicleList");
				cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
				cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
				cmd.Parameters.AddWithValue("@GuideId", GuideId);				
				cmd.Parameters.AddWithValue("@ShiftId", ShiftId);
				cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
				cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@MemberCount", MemberCount);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
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

		public DataTable GetGuideListAccordingBoardingPass(string VisitDate,string RequestId ,string ShiftId,string PlaceId,string VehicleType,int ZoneId)
		{
			try
			{
				DALConn();
				DataTable DS = new DataTable();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetGuideListAccordingBoardingPass");
				cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
				cmd.Parameters.AddWithValue("@RequestId", RequestId);
				cmd.Parameters.AddWithValue("@ShiftId", ShiftId);
				cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
				cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
				cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
				
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
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
		public DataTable GetVehicleListAccordingBoardingPass(string VisitDate, string VehicelType, string PlaceId,string ShiftId , string RequestId,int ZoneId)
		{
			try
			{
				DALConn();
				DataTable DS = new DataTable();
				SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
				cmd.Parameters.AddWithValue("@ActionName", "GetVehicleListAccordingBoardingPass");
				cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
				cmd.Parameters.AddWithValue("@VehicleType", VehicelType);
				cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
				cmd.Parameters.AddWithValue("@ShiftId", ShiftId);
				cmd.Parameters.AddWithValue("@RequestId", @RequestId);
				cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
				
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(DS);
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
        public DataTable SaveReplacedVehicleGuide(string RequestId,int GuideId, int VehicleId, int ShiftId, string VisitDate)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_BookingRoaster", Conn);
                cmd.Parameters.AddWithValue("@ActionName", "SaveReplacedVehicleGuide");
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@GuideId", GuideId);             
                cmd.Parameters.AddWithValue("@VehicleId", VehicleId);
                cmd.Parameters.AddWithValue("@ShiftId", ShiftId);
                cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
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
    }
	
}