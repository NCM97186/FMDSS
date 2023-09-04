using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMDSS.Controllers.OnlineBookingAPI
{
    public class ZooBookingController : ApiController
    {
        private readonly IZooBooking _requestManager;
        public ZooBookingController()
        {
            if (_requestManager == null)
            {
                _requestManager = new FMDSS.APIRepo.ZooBookingRepo();
            }
        }

        [HttpGet]
        public ZooBookingModelResponse ZooBookingPlaces()
        {
            UpdateZooTicketRequest UpdateZooBooking = new UpdateZooTicketRequest();
            string jsonstring = JsonConvert.SerializeObject(UpdateZooBooking);
            ZooBookingModelResponse response = new ZooBookingModelResponse();
            try
            {
                response = _requestManager.ZooBookingPlaces();
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<ZooBookingModelResponse>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }

        [HttpGet]
        public ShiftModelResponse GetShift()
        {
            ShiftModelResponse response = new ShiftModelResponse();
            try
            {
                response = _requestManager.GetShift();
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<ShiftModelResponse>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }

        [HttpGet]
        public AvaliableTicketModelResponse ChkTicketAvailability(string PlaceId, string ShiftType, string VisitDate, string UserId)
        {
            AvaliableTicketModelResponse response = new AvaliableTicketModelResponse();
            try
            {
                response = _requestManager.ChkTicketAvailability(Convert.ToInt32(PlaceId), ShiftType, VisitDate, UserId, null);
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<AvaliableTicketModelResponse>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }

        [HttpGet]
        public MemberVehicleDetailsResponce GetMemberAndVehicleFeelst(string PlaceId, string UserId)
        {
            MemberVehicleDetailsResponce response = new MemberVehicleDetailsResponce();
            try
            {
                //  ChkTicketAvailability(int PlaceId, string ShiftType, string DateofVisit, string KioskUserId, string Conn = null)
                response = _requestManager.MemberVehicleDetails(Convert.ToInt32(PlaceId), UserId, null);
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<MemberVehicleDetailsResponce>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }

        [HttpPost]
        public FinalSubmitResponceAfterSubmit SubmitZooBooking(FinalSubmitModel SubmitZooBooking)
        {
            FinalSubmitResponceAfterSubmit response = new FinalSubmitResponceAfterSubmit();
            try
            {
                response = _requestManager.SubmitZooBooking(SubmitZooBooking);
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<FinalSubmitResponceAfterSubmit>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }

        [HttpPost]
        public UpdateZooTicketResponce UpdateZooBooking(UpdateZooTicketRequest UpdateZooBooking)
        {
            UpdateZooTicketResponce response = new UpdateZooTicketResponce();
            try
            {
                response = _requestManager.UpdateZooBooking(UpdateZooBooking);
            }
            catch (Exception ex)
            {
                response = Response.ErrorLogs<UpdateZooTicketResponce>(response, ex.Message, ex.StackTrace);
            }
            return response;
        }
        [HttpPost]
        public UpdateZooTicketResponce UpdateZooBooking_EmitraPlus(UpdateZooTicketRequest_EmitraPlus UpdateZooBooking)
        {
            UpdateZooTicketResponce response = new UpdateZooTicketResponce();
            if (!UpdateZooBooking.Equals(null))
            {
                try
                {
                    response = _requestManager.UpdateZooBooking_Emitraplus(UpdateZooBooking);
                }
                catch (Exception ex)
                {
                    response = Response.ErrorLogs<UpdateZooTicketResponce>(response, ex.Message, ex.StackTrace);
                }
                return response;
            }
            else
            {
                return response = new UpdateZooTicketResponce { Status = ResponseStatus.Failed, Message = "Post Data have null values." };
            }

        }
    }
}