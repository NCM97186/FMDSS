using FMDSS.APIModel;
using FMDSS.CustomModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIInterface
{
  public  interface IChatBot
    {
        DataTableResponse GetPlace();
        DataTableResponse GetSiftDetails();
        DataTableResponse GetBookingDetails(ChatBotModel MIS);
        DataTableResponse GetTicketdetails(string SSOID);
        DataTableResponse TicketAvailability(ChatBotModel MIS);
        DataTableResponse TicketStatus(ChatBotModel MIS);
        DataTableResponse bookingrefund(ChatBotModel MIS);
        
        DataTableResponse FAQ();
        DataSetResponse BookingInformation(ChatBotModel MIS);
        DataTableResponse NurseryDetails(ChatBotModel MIS);
        DataTableResponse NocStatus(ChatBotModel MIS);
        DataSetResponse Nocdetails(ChatBotModel MIS);
        DataTableResponse PlantationModule(ChatBotModel MIS);
        DataSetResponse Checkotp(string SsoId, int OTP);
        bool GetOTP(string SsoId);
        DataTableResponse District();
        DataTableResponse DropDownNursery(string DIST_CODE);
        DataSetResponse DropDownProduct(string DIST_CODE, string NURSERY_CODE);
        DataSetResponse RearchNOC(ChatBotModel MIS);
        DataTableResponse Zone(string PlaceID);


    }
}
