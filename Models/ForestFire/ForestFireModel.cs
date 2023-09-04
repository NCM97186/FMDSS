using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForestFire
{
    public class ForestFireModel
    {
        public string SNo { get; set; }
        public string Fire_Date { get; set; }
        public string Fire_Time { get; set; }
        public string Source { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }
        public string Block { get; set; }
        public string Beat { get; set; }
    }

    public class ForestFire_Report
    {
        public string SNo { get; set; }
        public string State { get; set; }

        public string Financial_Year { get; set; }

        public string Quarter { get; set; }

        public string District { get; set; }

        public string NumberOfIncidents { get; set; }

        public string TotalAreaAffected { get; set; }

        public string QuantificationOfLoss { get; set; }
        public string CauseofFire { get; set; }
    }

    public class ForestFire_AddDetails
    {
        public string SpeciesList { get; set; }

        public string SNo { get; set; }

        public long ID { get; set; }
        public string State { get; set; }

        public string Financial_Year { get; set; }

        public string Quarter { get; set; }

        public string District { get; set; }

        public string NumberOfIncidents { get; set; }

        public string QuantificationOfLoss { get; set; }
        public string TotalAreaAffected { get; set; }
        public string CauseofFire { get; set; }
        public string Fire_Date { get; set; }
        public string Fire_Time { get; set; }
        public string Division { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Remarks { get; set; }
        public string AlertID { get; set; }
        public string ForestProduceLoss { get; set; }
        public string WildlifeLoss { get; set; }
        public string AnyotherLoss { get; set; }
        public string NoPepoleInvolved { get; set; }
        public string OtherDeptHelped { get; set; }
        public string FireDateTime { get; set; }
        public string PuttOffTime { get; set; }
        public string SSOID { get; set; }
        public string ImagePath { get; set; }
        
       
        //----------------------Added On 22-04-2020
        /// /////////////////////////////////////////////////
        public int FireActionStatusId { get; set; }
        public int MonetaryLoss { get; set; }
        public DateTime CurrentDate { get; set; }
        public TimeSpan CurrentTime { get; set; }
        public string CurrentLat { get; set; }
        public string CurrentLong { get; set; }
        public int CauseOfFireId { get; set; }
        public int FireIncidentAreaId { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string RangeCode { get; set; }
        public int NakaId { get; set; }
        public long User_Id { get; set; }
        public string Source { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string Block_Name { get; set; }
        public string Beat { get; set; }
        public TimeSpan ResponseInitTime { get; set; }
        /// /////////////////////////////////////////////////

        public string PutOffDate { get; set; }
        public string PutOffDate1 { get; set; } //Add for Saunesh (response date)
        public string ImagePathAfter { get; set; }
        

        public List<DocumentList> ImageForestFireImage { get; set; }
        public List<DocumentList> ImageForestFireImageAfter { get; set; }

        public List<string> UploadForestFireImage { get; set; }
        public List<string> UploadForestFireImageAfter { get; set; }

         


    }

    public class ForestFire_AddDetailsReport
    {
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        [Display(Name = "Circle")]
        public string CircleCode { get; set; }
        [Display(Name = "Division")]
        public string DivisionCode { get; set; }
        [Display(Name = "Range")]
        public string RangeCode { get; set; }
        [Display(Name = "Naka")]
        public string NakaID { get; set; }
        public Int16 StatusID { get; set; }
        public string Total_NumberOfIncidents { get; set; }
        public string Total_QuantificationOfLoss { get; set; }
        public string Total_TotalAreaAffected { get; set; }
    }
    public class ForestFire_AddDetailsVM
    {
        public string SNo { get; set; }

        public long ID { get; set; }
        public string State { get; set; }

        public string Financial_Year { get; set; }

        public string Quarter { get; set; }

        public string District { get; set; }

        public string NumberOfIncidents { get; set; }

        public string QuantificationOfLoss { get; set; }
        public string TotalAreaAffected { get; set; }
        public string CauseofFire { get; set; }
        public string Fire_Date { get; set; }
        public string Fire_Time { get; set; }
        public string Division { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Remarks { get; set; }

    }

    public class ForestFire_AddDetailsVM_Total
    {
        public string Total_NumberOfIncidents { get; set; }
        public string Total_QuantificationOfLoss { get; set; }
        public string Total_TotalAreaAffected { get; set; }
        public virtual List<ForestFire_AddDetailsVM> ForestFire_AddDetailsVMReportList { get; set; }
    }

    public class ForestFireAlertDashBoard
    {
        public string SNo { get; set; }
        public string District { get; set; }
        public string TotalCount { get; set; }
    }
    public class ForestFireAlertDistrict
    {
        public string SNo { get; set; }
        public string Fire_Date { get; set; }
        public string Fire_Time { get; set; }
        public string Source { get; set; }
        [JsonIgnore]
        public string Latitude { get; set; }
        [JsonIgnore]
        public string Longitude { get; set; }

        public string GIS_Latitude { get; set; }
        public string GIS_Longitude { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }
        public string Block { get; set; }
        public string Beat { get; set; }
        public string SSOID { get; set; }
        public string Coordinates { get; set; }



        public string TotalAreaAffected { get; set; }
        public string QuantificationOfLoss { get; set; }
        public string CauseOfFire { get; set; }
        public string Remarks { get; set; }
        public string Entered_by { get; set; }
        public string ForestProduceLoss { get; set; }
        public string WildlifeLoss { get; set; }
        public string AnyotherLoss { get; set; }
        public string NoPepoleInvolved { get; set; }

        public string OtherDeptHelped { get; set; }
        public string PuttOffTime { get; set; }
    }
    #region Forest fire API data
    //add by amrit barotia on 14-06-2021
    public class ForestFireHeadDataAPI
    {
        public string error { get; set; }
        public string message { get; set; }
        public string totalRecords { get; set; }
        public Dictionary<int, ForestFireDetailDataAPI> data { get; set; }

    }

    public class ForestFireDetailDataAPI
    {

        public string sno { get; set; }
        public string firedate { get; set; }
        public string firetime { get; set; }
        public string sourcetype { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string longdeg { get; set; }
        public string latdeg { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string circle { get; set; }
        public string division { get; set; }
        public string rangename { get; set; }
        public string block { get; set; }
        public string beat { get; set; }
        public string forestBlock { get; set; }
        public string compartmentNo { get; set; }
        public string timestamp { get; set; }
    }
    #endregion
}