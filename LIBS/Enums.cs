using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS
{
    #region NP Booking
    public enum VehicleType
    {
        [Display(Name = "Govt. Specified")]
        GovtSpecified = 1,
        [Display(Name = "Private Vehicle")]
        Private = 2
    }

    public enum FeesApplicable
    {
        [Display(Name = "Member Wise")]
        MemberWise = 1,
        [Display(Name = "Booking Wise")]
        BookingWise = 2
    }

    public enum ItemParent
    {
        [Display(Name = "Booking Fees")]
        BookingFee = 1,
        [Display(Name = "Vehicle Fee")]
        VehicleFee = 2,
        [Display(Name = "Camera Fee")]
        CamersFee = 3,
        [Display(Name = "Guide Fee")]
        GuideFee = 4,
        [Display(Name = "Emitra Fee")]
        EmitraFee = 5,
		[Display(Name = "Odhi Fee")]
		OdhiFee = 6,
		[Display(Name = "Half Day Fee")]
		HalfDayFee = 7,
        [Display(Name = "Facility Fee")]
        FacilityFee = 8,
        [Display(Name = "Maintenance Fee")]
        MaintenanceFee = 9,
    }
    public enum PaymentMode
    {
        Online = 0,
        Cash = 1,
        Cheque = 2,
        DemandDraft = 3
    }
    public enum TransactionStatus
    {
        Pending = 0,
        Paid = 1,
        Failed = 2
    }
    public enum BookingStatus
    {
        Pending = 0,
        Booked = 1,
        Cancelled = 2
    }
    public enum BookingStatusForReport
    {
        Failed = 0,
        Booked = 1,
        Cancelled = 2
    }
    public enum BookingType
    {
        OnlineCitizenBooking = 1,
        DepartmentBooking = 2,
        EmitraKioskBooking = 3
    }

    public enum NPDropDownActionCode
    {
        Place = 1
    }
    #endregion

    #region FRA
    public enum ActionTypeForFRA
    {
        Approve = 2,
        Reject = 3,
        [Display(Name = "Re-Assign")]
        ReAssign = 6,
        [Display(Name = "Re-submit")]
        ReSubmit = 8,
        Forward = 10,
        Appeal = 13,
        Recommended = 21,
        ESigned = 22,
        Considered = 23
    }
    public enum DocumentType
    {
        CompartmentNo = 1,
        GramSabhaMember = 2,
        BorderingVillage = 3,
        ScheduledTribe = 21,
        ForestDweller = 22,
        FamilyMember = 23,
        Other = 24,
        SurveyEvidence = 25,
        HalkaPatwariReport = 26,
        ForesterReport = 27,
        PattaReport = 28,
        ApprovalEvidence = 29,
        FIREvidence = 30,
        CaseDetailsEvidence = 31,
        SeizureReport = 32,
        FRCCommitteeReport = 37,
        GramSabhaSankalpDocument = 38,
        MOMDocument = 39
    }
    public enum DocumentLevel
    {
        Common = 0,
        Specific = 1,
        Approve = 2
    }

    public enum FRAUserType
    {
        Citizen = 1,
        GramSabha = 2,
        SDLC = 3,
        SDLCSDO = 7,
        DLC = 4,
        Collector = 5,
        Other = 6
    }
    public enum FRAClaimType
    {
        Individual = 1,
        Community = 2
    }
    public enum RowType
    {
        SurveyDetails = 1,
        ItemSeizedDetails = 2,
        Research_SpecimenDetails = 3,
        Research_SampleDetails = 4,
        DOD_ProductDetails = 5,
        Rescue_Staff = 6
    }

    public enum ReportType
    {
        HalkaPatwari = 1,
        Forester = 2,
        Patta = 3
    }

    #endregion

    #region DOD
    public enum ExchangeMode
    {
        SiteToDepot = 1,
        Nursury = 2,
        DepotToDepot = 3
    }
    #endregion

    #region Tendupatta
    public enum TPRequestType
    {
        Individual = 1,
        Organization = 2
    }
    #endregion

}