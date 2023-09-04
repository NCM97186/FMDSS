using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.FRAViewModel
{
    public class HalkaPatwariVM
    {
        public HalkaPatwariDetailsVM HalkaPatwariDetailsVM { get; set; }
        public List<KhasraDetailsVM> KhasraDetailsVM { get; set; }
    }

    public class HalkaPatwariDetailsVM
    {
        public long? ClaimRequestDetailsID { get; set; }
        public long? WorkFlowDetailsID { get; set; }
        public string CaseNumber { get; set; }
        public string ClaimantName { get; set; }
        public string FatherName { get; set; }
        public string DistrictName { get; set; }
        public string BlockName { get; set; }
        public string VillageName { get; set; }
        public string GPName { get; set; }
        public string TehsilName { get; set; }
        public string ForestSectionNames { get; set; }
        public string MemberNames { get; set; }
        public string Address { get; set; }
        public string ScheduledTribe { get; set; }
        public string CompartmentNumbers { get; set; }
        public string KhasraNumbers { get; set; }
        public string TotalAreaAgainstOccupiedForestLands { get; set; }
        public string TotalAreaApprovedAgainstOccupiedForestLand { get; set; }
        public string Purpose { get; set; }
        public string BoundryDes { get; set; }
        public string EastLakhaRaga { get; set; }
        public string NorthLakhaRaga { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompletedDate { get; set; }
        public string ActivityData { get; set; }
    }

    public class KhasraDetailsVM
    {
        public long KhasraDetailsID { get; set; }
        public long SurveyDetailsID { get; set; }
        public string CompartmentNumber { get; set; }
        public string KhasraNumber { get; set; }
        public string TotalAreaAgainstKhasra { get; set; }
        public string TotalAreaAgainstOccupiedForestLand { get; set; }
        public string OccupancyType { get; set; }
        public string ForestSectionName { get; set; }
        public string SpecialRemarks { get; set; }
        public string VillageName { get; set; }
    }

    public class OtherLatLong
    {
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}