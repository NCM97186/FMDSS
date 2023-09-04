using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIHelpers
{

    public enum Status
    {
        Active = 1,
        DeActive = 0
    }


    public static class ConnectionString
    {
        public static string Conn = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
    }
    public class Procedures
    {
        internal static string BudgetPerposalList = "dbo.BA_getBudgetPerposalList";
        internal static string SP_GetZoneWiseReportForWildlifeBooking = "dbo.SP_GetZoneWiseReportForWildlifeBooking";
        internal static string GetZooPlaces = "dbo.GetZooPlaces";


        #region Online Booking Evening Morning API
        internal static string SP_ChkSafariAccomoAvailability = "dbo.TB_ChkSafariAccomoAvailability";
        internal static string SP_Citizen_Select_vehicle_by_CatID = "dbo.TB_Citizen_Select_vehicle_by_CatID";

        internal static string CheckTicketAvailability = "dbo.Sp_Zoo_ChkTicketAvailability";
        internal static string SP_ChkSafariAccomoAvailabilityOpenCloseDate = "dbo.TB_ChkSafariAccomoAvailabilityForMobileAPI";
        internal static string GetMemberAndVehicleFeelst = "dbo.Sp_Zoo_MemberVehicleDetails";
        internal static string FinalSubmit = "dbo.SP_Zoo_Booking";
        #endregion
        #region FRA Summary Report SPs
        internal static string FRASummary_DistrictList = "dbo.SP_FRA_API_ClaimRequestDetails";
        internal static string SP_FRA_API_ClaimRequestDetails = "dbo.SP_FRA_API_ClaimRequestDetails";
        #endregion
    }
}
