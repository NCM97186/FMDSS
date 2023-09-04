using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Master
{

    public class RosterVehicleDetails
    {
        public int Index { get; set; }
        public string PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string ShiftName { get; set; }
        public string ZoneId { get; set; }
        public string ShiftId { get; set; }
        public string ZoneName { get; set; }
        public string VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string DriverName { get; set; }
        public string GuideName { get; set; }
        public string DriverMobileNumber { get; set; }
        public DateTime DateofVisit { get; set; }
        public string Colour { get; set; }

    }

    public class RosterDetails : DAL
    {
        public Int64 RosterDetailId { get; set; }
        public string PlaceId { get; set; }
        public string CurrentDate { get; set; }
        public int ZoneId { get; set; }
        public int ShiftId { get; set; }
        public string VehicleId { get; set; }
        public string AllocatedVehicleNameNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileNumber { get; set; }
        public string isActive { get; set; }
        public string Enteredby { get; set; }
        public string EnteredOn { get; set; }
        public string IPAddredd { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public string Updated_IPAddredd { get; set; }
        public DateTime DateofVisit { get; set; }

        public DataTable Select_RosterPlace()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spRosterDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectPlace");
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

        public DataTable Select_RosterZone(string PlaceId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spRosterDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectZone");
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
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

        public DataTable Select_RosterShift(string PlaceId, string ZoneId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spRosterDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectShift");
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
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

        public List<RosterVehicleDetails> LoadRosterData(string PlaceId, string ZoneId, string ShiftId, string DateofVisit)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spRosterDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetVehicleZonePlaceWise");
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                List<RosterVehicleDetails> RDlist = new List<RosterVehicleDetails>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //int Count = Convert.ToInt32(dt.Rows[i]["TotalSeats"]);
                    int Count = Convert.ToInt32(dt.Rows[i]["TotalEqptAvailability"]);
                    string ZoneCode = GetZoneNumber(Convert.ToString(dt.Rows[i]["ZoneName"]));
                    string ZoneName = Convert.ToString(dt.Rows[i]["ZoneName"]);
                    string NewZoneId = Convert.ToString(dt.Rows[i]["ZoneID"]);
                    string Shift = GetShiftName(ShiftId);
                    string NewPlaceId = Convert.ToString(dt.Rows[i]["PlaceId"]);
                    string PlaceName = Convert.ToString(dt.Rows[i]["PlaceName"]);
                    string PlaceCode = GetPlaceCode(Convert.ToString(dt.Rows[i]["PlaceName"]));
                    string Vehicle = Convert.ToString(dt.Rows[i]["Name"]);
                    string VehicleId = Convert.ToString(dt.Rows[i]["VehicleId"]);
                    int index = 1;
                    for (int x = 0; x < Count; x++)
                    {



                        RosterVehicleDetails RVDlist = new RosterVehicleDetails();
                        RVDlist.Index = index;
                        RVDlist.PlaceId = NewPlaceId;
                        RVDlist.PlaceName = PlaceName;
                        RVDlist.ZoneId = NewZoneId;
                        RVDlist.ZoneName = ZoneName;
                        RVDlist.ShiftId = ShiftId;
                        RVDlist.ShiftName = Shift;
                        RVDlist.VehicleId = VehicleId;
                        RVDlist.VehicleName = PlaceCode + "-" + ZoneCode + "-" + Shift + "-" + Vehicle.ToString() + index;
                        RVDlist.VehicleRegistrationNumber = "";
                        RVDlist.DateofVisit = Convert.ToDateTime(DateofVisit);
                        RVDlist.DriverName = "";
                        RVDlist.DriverMobileNumber = "";

                        if (Vehicle.ToString() == "Gypsy")
                            RVDlist.Colour = "#00FFFF";
                        else
                            RVDlist.Colour = "#00FF00";
                        RDlist.Add(RVDlist);
                        index += 1;
                    }

                }



                return RDlist;
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

        private string GetPlaceCode(string PlaceName)
        {
            switch (PlaceName)
            {
                case "Ranthambore National Park": // statement sequence
                    return "RTR";
                case "Ranthambore National Park Current Booking": // statement sequence
                    return "RTRC";
                case "Ranthambore National Park(HD/FD)": // statement sequence
                    return "RTRHF";
                case "Sariska WL Sanctuary (Sariska Gate)": // statement sequence
                    return "STR";
                case "Sariska WL Sanctuary (Tehla Gate)": // statement sequence
                    return "STR";
                case "Tatkal Booking for Ranthambore National Park": // statement sequence
                    return "RTRT";
                default:
                    return "";
            }
        }

        private string GetShiftName(string shiftId)
        {
            switch (shiftId)
            {
                case "1": // statement sequence
                    return "M";
                case "2": // statement sequence
                    return "E";
                case "3": // statement sequence
                    return "F";
                case "4": // statement sequence
                    return "H";
                default:
                    return "";
            }
        }

        private string GetZoneNumber(string ZoneName)
        {
            switch (ZoneName)
            {
                case "Zone 1": // statement sequence
                    return "1";
                case "Zone 2": // statement sequence
                    return "2";
                case "Zone 3": // statement sequence
                    return "3";
                case "Zone 4": // statement sequence
                    return "4";
                case "Zone 5": // statement sequence
                    return "5";
                case "Zone 6": // statement sequence
                    return "6";
                case "Zone 7": // statement sequence
                    return "7";
                case "Zone 8": // statement sequence
                    return "8";
                case "Zone 9": // statement sequence
                    return "9";
                case "Zone 10": // statement sequence
                    return "10";
                case "Route 1": // statement sequence
                    return "1";
                case "Route 2": // statement sequence
                    return "2";
                case "Route 3": // statement sequence
                    return "3";
                case "All Zone": // statement sequence
                    return "A";
                case "Modified Route": // statement sequence
                    return "MR";
                default:
                    return "";
            }
        }

        //public List<RosterVehicleDetails> SaveRosterDetails(List<RosterVehicleDetails> model)
        //{
        //    foreach (var Item in model)
        //    {

        //        DALConn();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("spRosterDetails", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        cmd.Parameters.AddWithValue("@Action", "GetVehicleZonePlaceWise");
        //        cmd.Parameters.AddWithValue("@PlaceId", Item.PlaceId);
        //        cmd.Parameters.AddWithValue("@ZoneId", Item.ZoneId);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da.Fill(dt);


        //        List<RosterVehicleDetails> RDlist = new List<RosterVehicleDetails>();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int Count = Convert.ToInt32(dt.Rows[i]["TotalSeats"]);
        //            //int Count = Convert.ToInt32(dt.Rows[i]["TotalEqptAvailability"]);
        //            string ZoneCode = GetZoneNumber(Convert.ToString(dt.Rows[i]["ZoneName"]));
        //            string ZoneName = Convert.ToString(dt.Rows[i]["ZoneName"]);
        //            string NewZoneId = Convert.ToString(dt.Rows[i]["ZoneID"]);
        //            string Shift = GetShiftName(Item.ShiftId);
        //            string NewPlaceId = Convert.ToString(dt.Rows[i]["PlaceId"]);
        //            string PlaceName = Convert.ToString(dt.Rows[i]["PlaceName"]);
        //            string PlaceCode = GetPlaceCode(Convert.ToString(dt.Rows[i]["PlaceName"]));
        //            string Vehicle = Convert.ToString(dt.Rows[i]["Name"]);
        //            string VehicleId = Convert.ToString(dt.Rows[i]["VehicleId"]);
        //            int index = 1;
        //            for (int x = 0; x < Count; x++)
        //            {
        //                RosterVehicleDetails RVDlist = new RosterVehicleDetails();
        //                RVDlist.Index = index;
        //                RVDlist.PlaceId = NewPlaceId;
        //                RVDlist.PlaceName = PlaceName;
        //                RVDlist.ZoneId = NewZoneId;
        //                RVDlist.ZoneName = ZoneName;
        //                RVDlist.ShiftId = Item.ShiftId;
        //                RVDlist.ShiftName = Shift;
        //                RVDlist.VehicleId = VehicleId;
        //                RVDlist.VehicleName = PlaceCode + "-" + ZoneCode + "-" + Shift + "-" + Vehicle.ToString() + index;
        //                RVDlist.VehicleRegistrationNumber = "";
        //                RVDlist.DateofVisit = Convert.ToDateTime(Item.DateofVisit);
        //                RVDlist.DriverName = Item.DriverName;
        //                RVDlist.DriverMobileNumber = Item.DriverMobileNumber;
        //                RDlist.Add(RVDlist);
        //            }

        //        }

        //        ///for(int i=0;i<)

        //        //DataTable dt1 = new DataTable();
        //        //SqlCommand cmd1 = new SqlCommand("spRosterDetails", Conn);
        //        //cmd.CommandType = CommandType.StoredProcedure;
        //        //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        //        //cmd.Parameters.AddWithValue("@Action", "SaveRosterDetails");
        //        //cmd.Parameters.AddWithValue("@PlaceId",Item.PlaceId);
        //        //cmd.Parameters.AddWithValue("@VisitDate", Item.DateofVisit);
        //        //cmd.Parameters.AddWithValue("@ZoneId", Item.ZoneId);
        //        //cmd.Parameters.AddWithValue("@ShiftId", Item.ShiftId);
        //        //cmd.Parameters.AddWithValue("@VehicleId", Item.VehicleId);
        //        //cmd.Parameters.AddWithValue("@AllocatedVehicleNameNumber", Item.VehicleName);
        //        //cmd.Parameters.AddWithValue("@VehicleRegistrationNumber", Item.VehicleRegistrationNumber);
        //        //cmd.Parameters.AddWithValue("@DriverName", Item.DriverName);
        //        //cmd.Parameters.AddWithValue("@DriverMobileNumber", Item.DriverMobileNumber);
        //        //cmd.Parameters.AddWithValue("@isActive", 1);
        //        //cmd.Parameters.AddWithValue("@Enteredby",Convert.ToInt32(HttpContext.Current.Session["UserID"]));
        //        //cmd.Parameters.AddWithValue("@EnteredOn",DateTime.Now);
        //        //cmd.Parameters.AddWithValue("@IPAddredd", System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
        //        //cmd.Parameters.AddWithValue("@UpdatedBy", 0);
        //        //cmd.Parameters.AddWithValue("@UpdatedOn", null);
        //        //cmd.Parameters.AddWithValue("@GuideName", Item.GuideName);  
        //        //cmd1.CommandType = CommandType.StoredProcedure;
        //        //da1.Fill(dt1); 
        //        return null;
        //    }
        //}
    }
}
