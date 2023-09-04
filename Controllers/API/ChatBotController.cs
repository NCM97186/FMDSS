
using FMDSS.APIInterface;
using FMDSS.APIModel;
using FMDSS.CustomModels.Models;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestFire;
using FMDSS.Models.MIS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
 

namespace FMDSS.Controllers.API
{
    public class ChatBotController : ApiController
    {
        //
        // GET: /ChatBot/

        ////private readonly IChatBot _TBooking;

        ////public ChatBotController(IChatBot TBooking)
        ////{
        ////    this._TBooking = TBooking;

        ////}
        private readonly IChatBot _TBooking;
        int OtpNUmber = 0;
        public ChatBotController()
        {
            if (_TBooking == null)
            {
                //_TBooking = new FMDSS.APIRepo.FRARepo();
                _TBooking = new FMDSS.Repository.ChatBotRepository();
            }
        }


        [HttpGet]
        public DataTableResponse Ssologin(string SsoId,string MobileNo)
        {
            
            DataTableResponse response = new DataTableResponse();
            cls_mobileLogin oLogin = new cls_mobileLogin();

            if(MobileNo!=null)
            {
                DataTable dt = oLogin.getssoid(MobileNo);
                if(dt.Rows.Count > 0)
                {
                    SsoId = dt.Rows[0]["Ssoid"].ToString();
                }
                else
                {
                    response.Status = ResponseStatus.Success;
                    response.Message = "User not valid";
                    return response;
                }
            }


            DataSet oObjLoginData = oLogin.LoginMobileUser(SsoId);
            if (Globals.Util.isValidDataSet(oObjLoginData, 0))
            {

                //tables.LoginDetail = Globals.Util.GetListFromTable<LoginDetail>(oObjLoginData, 0);
                var Status = _TBooking.GetOTP(SsoId);
                if (Status == true)
                {

                    response.Status = ResponseStatus.Success;
                    response.Message = "Otp Send Your mobile number";
                    //tables.Status = 1;
                    //tables.Ssoid = SsoId;
                    //tables.Message = "Otp Send Your mobile number";

                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = "User invalid SSoid";
                    //tables.Status = 0;
                    //tables.Ssoid = SsoId;
                    //tables.Message = "User invalid SSoid";
                    //return Json(tables, JsonRequestBehavior.AllowGet);

                }

            }
            else
            {

                response.Status = ResponseStatus.Failed;
                response.Message = "invalid SSoid";

            }
            return response;

        }

        [HttpGet]
        public DataSetResponse CheckOTP(string SsoId, int OTP, string MobileNo)
        {
            cls_mobileLogin oLogin = new cls_mobileLogin();
            if (MobileNo != null)
            {
                DataTable dt = oLogin.getssoid(MobileNo);
                if (dt != null)
                {
                    SsoId = dt.Rows[0]["Ssoid"].ToString();
                }
            }
            return _TBooking.Checkotp(SsoId, OTP); 
        }

       


        [HttpGet]
        public DataTableResponse GetPlaceDetails()
        {

            return _TBooking.GetPlace();

        }

        [HttpGet]
        public DataTableResponse DistrictList()
        {
             
            return _TBooking.District();
        }


        [HttpGet]
        public DataTableResponse ZoneList(string PlaceID)
        {

            return _TBooking.Zone(PlaceID);
        }


        [HttpGet]
        public DataTableResponse NurseryList(string Dist_CODE)
        {

            return _TBooking.DropDownNursery(Dist_CODE);

        }


        [HttpGet]
        public DataSetResponse ProductList(string DIST_CODE,string NURSERY_CODE)
        {

            return _TBooking.DropDownProduct(DIST_CODE, NURSERY_CODE);

        }




        [HttpGet]
        public DataTableResponse SiftDetails()
        {
            
            return _TBooking.GetSiftDetails();

        }

        [HttpPost]
        public DataTableResponse GetDetailsofBooking(ChatBotModel MIS)
        {

            return _TBooking.GetBookingDetails(MIS);

        }

        [HttpGet]
        public DataTableResponse GetTicketDetails(string SSOID)
        {

            return _TBooking.GetTicketdetails(SSOID);

        }


        [HttpPost]
        public DataTableResponse TicketAvailability(ChatBotModel MIS)
        {

            return _TBooking.TicketAvailability(MIS);

        }

        [HttpPost]
        public DataTableResponse TicketStatus(ChatBotModel MIS)
        {

            return _TBooking.TicketStatus(MIS);

        }

        [HttpPost]
        public DataTableResponse bookingrefund (ChatBotModel MIS)
        {

            return _TBooking.bookingrefund(MIS);

        }

        [HttpPost]
        public DataSetResponse BookingInformation(ChatBotModel MIS)
        {

            return _TBooking.BookingInformation(MIS);

        }

        [HttpPost]
        public DataTableResponse NurseryDetails(ChatBotModel MIS)
        {

            return _TBooking.NurseryDetails(MIS);

        }


        [HttpPost]
        public DataTableResponse NocStatus(ChatBotModel MIS)
        {

            return _TBooking.NocStatus(MIS);

        }

        [HttpPost]
        public DataSetResponse Nocdetails(ChatBotModel MIS)
        {

            return _TBooking.Nocdetails(MIS);

        }

        [HttpPost]
        public DataSetResponse RearchNOC(ChatBotModel MIS)
        {

            return _TBooking.RearchNOC(MIS);

        }


        [HttpPost]
        public DataTableResponse PlantationModule(ChatBotModel MIS)
        {

            return _TBooking.PlantationModule(MIS);

        }


        

        [HttpPost]
        public DataTableResponse FAQ()
        {

            return _TBooking.FAQ();

        }



    }
}
