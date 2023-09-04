using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FMDSS.APIHelpers;
using System.Data.SqlClient;
namespace FMDSS.APIDAL
{
    public class OnlineBookingDAL
    {
        public static DataSet GetCurrentBookingZoneDetailsList(string ACTION, string PlaceId, long UserId, string ShiftID, string DateOfArrival, string Conn = null)
        {
            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_GetZoneWiseReportForWildlifeBooking,
                    new SqlParameter("ACTION", ACTION),
                    new SqlParameter("PlaceId", PlaceId),
                    new SqlParameter("ShiftID", ShiftID),
                    new SqlParameter("DateOfArrival", DateOfArrival),
                    new SqlParameter("USERID", UserId));
        }

        public static DataSet chkSafariAccomo(long PlaceId, string IsDepartmentalKioskUser,int IsCurrentOrAdvance, string Conn = null)
        {
            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_ChkSafariAccomoAvailability,
                    new SqlParameter("PlaceId", PlaceId),
                    new SqlParameter("DeptUser", IsDepartmentalKioskUser),
                    new SqlParameter("IsCurrentOrAdvance",@IsCurrentOrAdvance));

        }
        public static DataTable CheckOpenCloseDateCurrentAndAdvanceBooking(long PlaceId, string IsDepartmentalKioskUser, int IsCurrentOrAdvance, string Conn = null)
        {
            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataTable(Conn, CommandType.StoredProcedure, Procedures.SP_ChkSafariAccomoAvailabilityOpenCloseDate,
                    new SqlParameter("PlaceId", PlaceId),
                    new SqlParameter("DeptUser", IsDepartmentalKioskUser),
                    new SqlParameter("IsCurrentOrAdvance", @IsCurrentOrAdvance));

        }
        public static DataTable Select_vehicle(long PlaceId, long ZoneID,long VehicleCatID, string Conn = null)
        {
            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataTable(Conn, CommandType.StoredProcedure, Procedures.SP_Citizen_Select_vehicle_by_CatID,
                    new SqlParameter("VehicleCatID", VehicleCatID),
                    new SqlParameter("PlaceId", PlaceId),
                    new SqlParameter("ZoneId", ZoneID));
        }
    }
}