using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using FMDSS.Models.FmdssContext;
using System.Data;
using FMDSS.Models.BookOnlineTicket;
using System.Threading.Tasks;

namespace FMDSS.APIRepo
{
    public class OnlineBookingRepo : IOnlineBooking
    {
        private readonly FmdssContext fmdsscontext;
        private BookOnTicket repo = new BookOnTicket();
        public OnlineBookingRepo()
        {
            if (fmdsscontext == null)
            {
                fmdsscontext = new FmdssContext();
            }
        }

        public OnlineBookingZoneWiseModelResponse GetOnlineBookingZoneDetails(long UserID, string PlaceID, string ShiftID, string DateOfArrival)
        {
            OnlineBookingZoneWiseModelResponse response = new OnlineBookingZoneWiseModelResponse();
            try
            {
                DataSet ds = new DataSet();
                ds = FMDSS.APIDAL.OnlineBookingDAL.GetCurrentBookingZoneDetailsList("GETZONEWISEGUIDENAMEAPI", PlaceID, UserID, ShiftID, DateOfArrival);
                //  if (ds.isValidDataSet())
                // {
                response.Data = Util.GetListFromTable<OnlineBookingZoneWiseModel>(ds, 0);
                //  }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }


        #region Online Booking Morning Evening Shift in Ranthambore

        public DataTableResponse GetPlaceDetails(long UserID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                response.Data = repo.Select_Place(UserID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public DataTableResponse GetPlaceDetailsAPI(long UserID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                response.Data = repo.Select_PlaceAPI(UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
        public DataSetResponse chkSafariAccomo(long UserID, string IsDepartmentalKioskUser, long PlaceID, int IsCurrentOrAdvance)
        {
            DataSetResponse response = new DataSetResponse();
            DataTable DT = new DataTable();
            try
            {

                response.Data = FMDSS.APIDAL.OnlineBookingDAL.chkSafariAccomo(PlaceID, IsDepartmentalKioskUser, IsCurrentOrAdvance);
                DT = FMDSS.APIDAL.OnlineBookingDAL.CheckOpenCloseDateCurrentAndAdvanceBooking(PlaceID, IsDepartmentalKioskUser, IsCurrentOrAdvance);
                response.Data.Tables.Add(DT);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse() { Message = ex.Message, Status = ResponseStatus.Failed, Data = null };
            }
            return response;
        }

        public DataTableResponse Select_vehicle(long UserID, long PlaceID, long ZoneID, long VehicleCatID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                response.Data = FMDSS.APIDAL.OnlineBookingDAL.Select_vehicle(PlaceID, ZoneID, VehicleCatID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }

        #endregion

        public async Task<DataTableResponse> chkTicketAvailabiltyandSeatsEqp(long placeid, string arrivaldate, string shifttime, int zoneid, int vehicleid)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                repo.PlaceId = placeid;
                repo.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
                repo.ShiftTime = shifttime;
                repo.ZoneId = zoneid;
                repo.vehicleID = vehicleid;
                repo.KioskUserId = "0";
                //response.Data = repo.CheckTicketAvailability();
                response.Data = await repo.CheckTicketAvailabilityWityPalaceOfWheel();
                //response.Data =  repo.CheckTicketAvailabilityWityPalaceOfWheel();
                //response.Data = FMDSS.APIDAL.OnlineBookingDAL.chkTicketAvailabiltyandSeatsEqp(PlaceId, DateOfArrival, ShiftID, ZoneID, VehicleID, kioskUserId);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }

        public DataTableResponse BindShiftByPlaceZoneOnlineBooking(int placeID, int Zone, string ArrivalDate, int UserID)
        {
            BookOnTicket bkt = new BookOnTicket();
            DataTableResponse response = new DataTableResponse();
            try
            {
                #region Citizen User
                bkt.PlaceId = Convert.ToInt64(placeID);
                bkt.ZoneId = Convert.ToInt64(Zone);
                bkt.DateOfArrival = ArrivalDate;
                response.Data = bkt.Select_Shift_By_PlacesZonesOnlineBooking();
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }
    }
}