using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.DriverGuideAPIModel
{
    public class DriverGuideAPIModel
    {
        public long USERID { get; set; }
        public int IsActive { get; set; }
        public long ID { get; set; }
        public string PersonType { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string PersonName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public List<TotalVehicle> TotalVehicle { get; set; }
    }
    
public class TotalVehicle
{
    public string VehicleNo { get; set; }
    public string VehicleType { get; set; }
}
    public class tbl_DriverGuideProfile
    {
        public long USERID { get; set; }
        public int IsActive { get; set; }
        public long ID { get; set; }
        public string PersonType { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string PersonName { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
    }
    public class DriverGuideAttendance
    {
        public long ID { get; set; }
        public int PlaceId { get; set; }
        public string PersonType { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string GuideName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public long UserId { get; set; }
        public int IsActive { get; set; }

    }

    public class tbl_OnlineBooking_VehicleDetailsAttendance
    {
        public long ID { get; set; }
        public int PlaceId { get; set; }
        public string PersonType { get; set; }
        public string VehicleNo { get; set; }
        public string GuideName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public long UserId { get; set; }
        public int IsActive { get; set; }
    }
    public class GetShiftTime
    {
        public int KioskBookingMorningTimeFrom { get; set; }
        public int KioskBookingMorningTimeTo { get; set; }
        public int KioskBookingEveningTimeFrom { get; set; }
        public int KioskBookingEveningTimeTo { get; set; }
        public int placeid { get; set; }

        public string ShiftName { get; set; }

        public int ShiftSatus { get; set; }

        public string NextShiftName { get; set; }

        public int NextShiftTime { get; set; }


    }
    public class GetDataRequestID
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string IDType { get; set; }
        public string IDNo { get; set; }
        public string Nationality { get; set; }
        public string MemberType { get; set; }
        public string EnteredOn { get; set; }

        public string GuidName { get; set; }

        public string VehicleNumber { get; set; }

        public string RequestID { get; set; }

        public int PlaceID { get; set; }

        public string PlaceName { get; set; }

        public int ZoneID { get; set; }
        public string ZoneName { get; set; }

        public int ShiftTime { get; set; }
        public string ShiftTimeName { get; set; }
        public string DateOfArrival { get; set; }

        public decimal AmountTobePaid { get; set; }

        public int AmountWithServiceCharges { get; set; }

    }


    public class GetRequestID
    {
        public string RequestID { get; set; }
        public string DateOfArrival { get; set; }
        public string ShiftTimeName { get; set; }
    }
    public class WildLifeFeedBackMaster
    {
        public int ID { get; set; }
        public int SNO { get; set; }
        public string FeedBack { get; set; }
        public string ControlType { get; set; }
        public string PersonType { get; set; }
        public int IsActive { get; set; }
        public int Rating { get; set; }
        public List<string> Options { get; set; }
    }

  public class Options
    {
        public string OptionsYes { get; set; }
        public string OptionsNo { get; set; }
    }
    public class WildLifeFeedBackTransaction
    {
        public int ID { get; set; }
        public int FeedBackID { get; set; }
        public string RequestID { get; set; }
        public int Rating { get; set; }
        public string Remarks { get; set; }
        public int FeedBackYes_No { get; set; }
        public string PersonType { get; set; }
        public string PersonName { get; set; }
        public string SSOID { get; set; }
    }
    public class DriverGuideAPIModel_Owner
    {
        public long USERID { get; set; }
        public int IsActive { get; set; }
        public long ID { get; set; }
        public string PersonType { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public List<TotalVehicle_Owner> TotalVehicle_Owner { get; set; }
    }
    public class TotalVehicle_Owner
    {
        public int SNO { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleType { get; set; }
        public string PersonName { get; set; }
    }
    public class DriverGuideAttendance_Owner
    {
        public long ID { get; set; }
        public int PlaceId { get; set; }
        public string PersonType { get; set; }
        //public string VehicleType { get; set; }
        //public string VehicleNo { get; set; }
        //public string GuideName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public long UserId { get; set; }
        public int IsActive { get; set; }
        public List<TotalVehicle_Owner> TotalVehicle_Owner { get; set; }

    }
}