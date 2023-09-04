using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FMDSS.Models.NP_ChoiceGuideVehicleBoat
{
    public interface INpChoice
    {
        
        DataSet GetChoiceDetails(string RequestId);
        DataSet UpdateChoiceDetails(NPChoice npChoice, string ChoiceRequestId, string EmitraResponse, int ResponseStatus, bool IsValidResponse);

        List<SelectListItem> GetNpGuideList(long PlaceId);
        List<NpBoatProp> GetNpBoatNumberList(long PlaceId);
        List<NPChoice> GetNpChoiceTransactionList(long UserId, string RequestId = "", string ChoiceRequestId = "");
        List<NPChoice> GetNpChoiceForReceiptList(long UserId, string RequestId = "", string ChoiceRequestId = "");

    }
}
