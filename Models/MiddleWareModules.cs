using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
	public class MiddleWareModules : DAL
	{
		public List<MiddleWareModuleGroup> GetMiddleWareModuleGroupList(string ssoid, long userid)
		{

			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("Stp_MiddleLayerModules", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GET_MainGroup");
			cmd.Parameters.AddWithValue("@UserId", UserId);
			cmd.Parameters.AddWithValue("@SSOId", ssoid);

			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			if (Conn.State == ConnectionState.Open)
			{

				Conn.Close();
			}
			List<MiddleWareModuleGroup> list = Globals.Util.GetListFromTable<MiddleWareModuleGroup>(dt);

			return list;
		}
		public List<MiddleWareModules> GetMiddleWareModulesList(string ssoid, long userid, int MainGroupId)
		{

			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("Stp_MiddleLayerModules", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GET_MainLinksWithGroup");
			cmd.Parameters.AddWithValue("@MainGroupId", MainGroupId);
			cmd.Parameters.AddWithValue("@UserId", userid);
			cmd.Parameters.AddWithValue("@SSOId", ssoid);

			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			if (Conn.State == ConnectionState.Open)
			{

				Conn.Close();
			}
			List<MiddleWareModules> list = Globals.Util.GetListFromTable<MiddleWareModules>(dt);

			return list;
		}
		public List<MiddleWareSubModulePages> GetSubModulePageList(int ModuleId)
		{
			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("Stp_MiddleLayerModules", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GET_SubModulePageList");
			cmd.Parameters.AddWithValue("@ModuleId", ModuleId);

			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			if (Conn.State == ConnectionState.Open)
			{

				Conn.Close();
			}
			List<MiddleWareSubModulePages> list = Globals.Util.GetListFromTable<MiddleWareSubModulePages>(dt);

			return list;
		}
		public List<MiddleWareSubModulePages> GetPlaceLinks(int ModuleId, string PageTitle)
		{
			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("Stp_MiddleLayerModules", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GET_PlaceLinks");
			cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
			cmd.Parameters.AddWithValue("@PageTitle", PageTitle);
			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			if (Conn.State == ConnectionState.Open)
			{

				Conn.Close();
			}
			List<MiddleWareSubModulePages> list = Globals.Util.GetListFromTable<MiddleWareSubModulePages>(dt);

			return list;
		}
		public List<BookingPermitDetail> GetPermitAvailibility(int PlaceId, string BookingDate)
		{

			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("spCitizenWLBookingTktInfo", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GetBookingStatus");
			cmd.Parameters.AddWithValue("@BookingDate", BookingDate);
			cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			if (Conn.State == ConnectionState.Open)
			{

				Conn.Close();
			}
			List<BookingPermitDetail> list = Globals.Util.GetListFromTable<BookingPermitDetail>(dt);

			return list;
		}
		public DataTable GetBookingFeeDetails(int PlaceId, int BookingType, int NoOfPerson = 1)
		{

			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("spCitizenWLBookingTktInfo", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GetBookingFee");
			cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
			cmd.Parameters.AddWithValue("@BookingType", BookingType);
			cmd.Parameters.AddWithValue("@PerPerson", NoOfPerson);

			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);
			if (Conn.State == ConnectionState.Open)
			{

				Conn.Close();
			}

			return dt;
		}
		public DataTable GetPlaceList(int ModuleId)
		{
			DALConn();
			DataTable dt = new DataTable();
			SqlCommand cmd = new SqlCommand("Stp_MiddleLayerModules", Conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@ActionName", "GET_PlaceList");
			cmd.Parameters.AddWithValue("@ModuleId", ModuleId);

			cmd.CommandType = CommandType.StoredProcedure;
			SqlDataAdapter da = new SqlDataAdapter(cmd);
			da.Fill(dt);

			return dt;
		}
        public string GetCurrentDate()
        {
            DALConn();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("Stp_MiddleLayerModules", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ActionName", "GET_CurrentDate");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            return dt.Rows[0]["CurrentDate"].ToString();
        }
        public int ModuleId { get; set; }
		public string ModuleName { get; set; }
		public string LinkUrl { get; set; }
		public bool IsCitizenUser { get; set; }
		public bool IsDepartmentUser { get; set; }
		public bool IsKioskUser { get; set; }
		public bool IsDeptKioskUser { get; set; }
		public bool IsEmitraUser { get; set; }
		public int MainModuleId { get; set; }

		public string SSOId { get; set; }
		public long UserId { get; set; }
		public string ModuleImg { get; set; }


		public List<MiddleWareModules> MWM_List { get; set; }
		public List<MiddleWareModuleGroup> MWMG_List { get; set; }
		public List<MiddleWareSubModulePages> MWSMG_List { get; set; }
		public List<BookingPermitDetail> AvailList { get; set; }
		public List<BookingPermit> PermitAvailList { get; set; }
		public List<BookingFees> bookingFeeList { get; set; }
        public List<BookingFeesOther> bookingFeeOther { get; set; }
        public List<string> ExcludeDateList { get; set; }

    }
	public class MiddleWareModuleGroup
	{
		public int MainModuleId { get; set; }
		public string MainModuleName { get; set; }
		public int DepartmentOrCitizenId { get; set; }
		public string ModuleImg { get; set; }
	}
	public class MiddleWareSubModulePages
	{
		public int ModuleId { get; set; }
		public int PlaceId { get; set; }
		public string PlaceName { get; set; }
		public string PageTitle { get; set; }
		public string PlaceType { get; set; }
		public string PageLinkUrl { get; set; }
		public string Icon { get; set; }
		public string MainLink { get; set; }
		public string BookingProcedure { get; set; }
		public int PTypeId { get; set; }
		public int BookingType { get; set; }
	}
	public class BookingPermitDetail
	{
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int ShiftType { get; set; }
        public string ShiftName { get; set; }
        public int Gypsy { get; set; }
        public int Canter { get; set; }
        public int PlaceID { get; set; }
        //Below Member Variables used for HD/FD
        public int MorningRemaing { get; set; }
        public int EveningRemaing { get; set; }
        public int FullDayRemaing { get; set; }
        public int FullDayBooking { get; set; }
        public int MorningBooking { get; set; }
        public int EveningBooking { get; set; }
        public string VehicleName { get; set; }
        //Above Member Variables used for HD/FD
    }
    public class BookingPermit
	{
		public int PlaceID { get; set; }
		public int ZoneId { get; set; }
		public string ZoneName { get; set; }
		public int ShiftType1 { get; set; }
		public int ShiftType2 { get; set; }
		public int ShiftType3 { get; set; }
		public string ShiftName1 { get; set; }
		public string ShiftName2 { get; set; }
		public string ShiftName3 { get; set; }
		public int Gypsy1 { get; set; }
		public int Canter1 { get; set; }
		public int Gypsy2 { get; set; }
		public int Canter2 { get; set; }
		public int Gypsy3 { get; set; }
		public int Canter3 { get; set; }
	}
	public class BookingFees
	{
		public int PlaceId { get; set; }
		public int BookingType { get; set; }
		public int NoOfPerson { get; set; }

		public string IndianGypsyFee { get; set; }
		public string ForeignerGypsyFee { get; set; }
		public string IndianCanterFee { get; set; }
		public string ForeignerCanterFee { get; set; }

		//public int IndianGypsyFeePerVh { get; set; }
		//public int ForeignerGypsyFeePerVh { get; set; }
		//public int IndianCanterFeePerVh { get; set; }
		//public int ForeignerCanterFeeVh { get; set; }

		//public decimal TotalIndianGypsyHDFee { get; set; }
		//public int FinalTotalIndianGypsyHDFee { get; set; }
		//public decimal TotalForeignerGypsyHDFee { get; set; }
		//public int FinalTotalForeignerGypsyHDFee { get; set; }

		//public decimal TotalIndianGypsyFDFee { get; set; }
		//public int FinalTotalIndianGypsyFDFee { get; set; }
		//public decimal TotalForeignerGypsyFDFee { get; set; }
		//public int FinalTotalForeignerGypsyFDFee { get; set; }
	}
	public class BookingFeesOther
	{
		public int PlaceId { get; set; }
		public string Indian { get; set; }
		public string Non_Indian { get; set; }
		public string Student { get; set; }
		public string Below5Years { get; set; }
		public string SpeciallyAbled { get; set; }
	}
}