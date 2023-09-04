using FMDSS.CustomModels.Models;
using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIInterface
{
   public interface IZooBooking
    {

        ZooBookingModelResponse ZooBookingPlaces();
        ShiftModelResponse GetShift();

        AvaliableTicketModelResponse ChkTicketAvailability(int PlaceId, string ShiftType, string DateofVisit, string KioskUserId, string Conn = null);
        MemberVehicleDetailsResponce MemberVehicleDetails(int PlaceId,string KioskUserId, string Conn = null);

        //FinalSubmitResponce SubmitZooBooking(FinalSubmitModel ZooBookingSubmit);

        FinalSubmitResponceAfterSubmit SubmitZooBooking(FinalSubmitModel ZooBookingSubmit);

        UpdateZooTicketResponce UpdateZooBooking(UpdateZooTicketRequest UpdateZooBooking);


        UpdateZooTicketResponce UpdateZooBooking_Emitraplus(UpdateZooTicketRequest_EmitraPlus UpdateZooBooking);
    }
}
