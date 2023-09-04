using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FMDSS.Models.GVChoice.IGVChoice
{
    public interface INTF_GVChoice
    {
        DataSet GetChoiceDetails(string RequestId, long UserId,int BookingType);
        DataSet UpdateChoiceDetails(GVChoice gvChoice, string ChoiceRequestId, string EmitraResponse, int ResponseStatus, bool IsValidResponse, int BookingType, int IsLiveOrUAT);

        List<SelectListItem> GetGvGuideList(long PlaceId);
        List<BoatProp> GetBoatNumberList(long PlaceId);        
        List<VehicleProp> GetVehicleNumberList(long PlaceId,string VehicleType);
        List<GVChoice> GetGVChoiceTransactionList(long UserId, ref string ReturnId, int bookingType, string RequestId = "", string ChoiceRequestId = "");
        List<GVChoice> GetGVChoiceForReceiptList(long UserId,int bookingType, string RequestId = "", string ChoiceRequestId = "");
        DataTable SaveAndGetPGChoiceRequestId(string RequestId);
        DataTable CheckGuideBookedStatus(int GuideId, int ShiftId, string VisitDate);
        DataTable CheckVehicleBookedStatus(int VehicleId, int ShiftId, string VisitDate);
        
    }
}
