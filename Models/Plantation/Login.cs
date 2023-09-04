using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Plantation
{
    public class Login
    {
        
        public string SSOIdOrMobileNo { get; set; }
        
        public string Password { get; set; }

        public bool isCattleGuard { get; set; }

        public bool Remember { get; set; }
    }
    public class UserPlantLocation
    {
        public long UserId { get; set; }
        public int PlantLocationId { get; set; }
        public string SiteNameEng { get; set; }
        public string SiteNameHindi { get; set; }
    }


    public class UserTripMobileDataFormat
    {

        public List<UserPlantationTrip> PlantTripList { get; set; }
        public List<UserPlantationTripDetail> TripDetailList { get; set; }
        public List<UserPlantationTripImg> PlantImgList { get; set; }
        public List<CattleGuardSalfy> Selfy { get; set; }
    }

    public class UserPlantationTrip
    {

        public long PTripId { get; set; }
        public int PlantLocationId { get; set; }
        public long UserId { get; set; }
        public string SSOId { get; set; }
        public int PSiteActionId { get; set; }
        public string StartTripDate { get; set; }
        public string EndTripDate { get; set; }
        public double PTripStartLat { get; set; }
        public double PTripStartLong { get; set; }
        public double PTripEndLat { get; set; }
        public double PTripEndLong { get; set; }
        public bool TripStatus { get; set; }
        public string Description1 { get; set; }
        public double PhotoLat { get; set; }
        public double PhotoLong { get; set; }
        public string Photo { get; set; }
        public string PhotoDateTime { get; set; }
        public List<UserPlantationTripDetail> TripDetailList { get; set; }
        public List<UserPlantationTripImg> PlantImgList { get; set; }
    }
    public class UserPlantationTripImg
    {
        public long Id { get; set; }
        public long PTripId { get; set; }
        public int ActionId { get; set; }
        public double Latitute { get; set; }
        public double Longitute { get; set; }
        public string Image1 { get; set; }
        public string AudioFile { get; set; }
        public string AudioExt { get; set; }
        public string ImgDateTime { get; set; }
        public string Remarks { get; set; }
    }
    public class UserPlantationTripDetail
    {
        public long Id { get; set; }
        public long PTripId { get; set; }
        public double Latitute { get; set; }
        public double Longitute { get; set; }
        public int PSiteActionId { get; set; }
        public string Description1 { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string TripDateTime { get; set; }

    }
    public class CheckUserLatLong
    {
        public int PlantLocationId { get; set; }
        public double PTripLat { get; set; }
        public double PTripLong { get; set; }
    }
    public class CattleGuardActions
    {
        public int PlantLocationId { get; set; }
        public double PTripLat { get; set; }
        public double PTripLong { get; set; }
    }
    public class CattleGuardSalfy
    {
        public int PlantLocationId { get; set; }
        public double PhotoLat { get; set; }
        public double PhotoLong { get; set; }
        public string Photo { get; set; }
        public string PhotoDateTime { get; set; }
    }
}