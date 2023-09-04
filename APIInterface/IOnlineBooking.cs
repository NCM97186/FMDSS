using FMDSS.CustomModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIInterface
{
    public interface IOnlineBooking
    {
        OnlineBookingZoneWiseModelResponse GetOnlineBookingZoneDetails(long UserID, string PlaceID, string ShiftID, string DateOfArrival);

        #region Online Booking Morning Evening Shift in Ranthambore
        DataTableResponse GetPlaceDetails(long UserID);
        DataSetResponse chkSafariAccomo(long UserID, string IsDepartmentalKioskUser, long PlaceID, int IsCurrentOrAdvance);
        DataTableResponse Select_vehicle(long UserID, long PlaceID, long ZoneID, long VehicleCatID);
        DataTableResponse GetPlaceDetailsAPI(long UserID);
        //DataTableResponse chkTicketAvailabiltyandSeatsEqp(long placeid, string arrivaldate, string shifttime, int zoneid, int vehicleid);
        Task<DataTableResponse>  chkTicketAvailabiltyandSeatsEqp(long placeid, string arrivaldate, string shifttime, int zoneid, int vehicleid);
        DataTableResponse BindShiftByPlaceZoneOnlineBooking(int placeID, int Zone, string ArrivalDate, int UserID);
        #endregion
    }
}
