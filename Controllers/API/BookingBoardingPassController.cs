using FMDSS.APIDAL;
using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class BookingBoardingPassController : ApiController
    {
        IBookingBoardingPass booking;
        public BookingBoardingPassController()
        {
            booking = new BookingBoardingPass();
        }
        [System.Web.Http.HttpGet]
        public DataTableResponse ValidateBoardingPass(string SsoId,bool IsQRCode, string RequestId,bool IsEnter,bool IsOut=false)
        {
            string requestId = "";
            if (IsQRCode == true && RequestId!=null)
            {
                if (RequestId.Length > 0)
                    requestId = FMDSS.Models.MySecurity.SecurityCode.Decode(RequestId);
                else
                    requestId = "na";
            }
            else
            {
                requestId = RequestId;
            }
            
            //string requestId = FMDSS.Models.EncodingDecoding.Decrypt("3XUAdp0GkXqNlEXAuf4+1Jwa30Als5+Zx0ZCLpT+7Kk=", "E-m!tr@2016");
            ///string str = "3XUAdp0GkXqNlEXAuf4+1Jwa30Als5+Zx0ZCLpT+7Kk=";
            DataTableResponse response = null;
            DataSet ds=   booking.ValidateBoardingPass(SsoId, requestId, IsEnter, IsOut);
            DataTable dt = null;
            if (ds.Tables.Count > 1)
            {
                dt= ds.Tables[1];
            }
                
            if (Convert.ToString(ds.Tables[0].Rows[0]["MsgStatus"]) == "1")
            {
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = Convert.ToString(ds.Tables[0].Rows[0]["Msg"]), data3 = dt };
            }
            else if (Convert.ToString(ds.Tables[0].Rows[0]["MsgStatus"]) == "2")
            {
                response = new DataTableResponse { Status = ResponseStatus.Info, Message = Convert.ToString(ds.Tables[0].Rows[0]["Msg"]), data3 = dt };
            }
            else
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = Convert.ToString(ds.Tables[0].Rows[0]["Msg"]) };
            }
            return response;
        }
        public DataTableResponse GetShiftTicktStatusCounts(string SsoId, string SelectedDate, int SelectedShift)
        {
            DataTableResponse response = null;
            DataSet ds = booking.GetShiftTicktStatusCounts(SsoId, SelectedDate, SelectedShift);
            DataTable dt = null;
            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables[1];
            }

            if (Convert.ToString(ds.Tables[0].Rows[0]["MsgStatus"]) == "1")
            {
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = Convert.ToString(ds.Tables[0].Rows[0]["Msg"]), data3 = dt };
            }
            else if (Convert.ToString(ds.Tables[0].Rows[0]["MsgStatus"]) == "2")
            {
                response = new DataTableResponse { Status = ResponseStatus.Info, Message = Convert.ToString(ds.Tables[0].Rows[0]["Msg"]), data3 = dt };
            }
            else
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = Convert.ToString(ds.Tables[0].Rows[0]["Msg"]) };
            }
            return response;
        }
        public DataTableResponse GetShiftList()
        {
            DataTableResponse response = null;
            DataSet ds = booking.GetShiftList();
            DataTable dt = null;
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Shift List Data Get Successfully", data3 = dt };
            }
            else
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Record Not Found" };

            return response;
        }
    }
}
