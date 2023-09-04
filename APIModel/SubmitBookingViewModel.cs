using FMDSS.Models.BookOnlineTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.APIModel
{
    public class SubmitBookingViewModel
    {
        public int placeid { get; set; }
        public string arrivaldate { get; set; }
        public string shifttime { get; set; }
        public int zoneid { get; set; }
        public int vehicleid { get; set; }
        public long UserID { get; set; }
        public string DeviceUniqueID { get; set; }
        public string DeviceType { get; set; }
        public string RequestId { get; set; }
        public string AppVersion { get; set; }
        public System.Net.Http.HttpRequestMessage Request { get; set; }
        public List<MemberInfo> memberinfo { get; set; }
    }
    public class EmitraRequestResponse
    {
        public string MerchantCode { get; set; }
        public string PRN { get; set; }
        public string STATUS { get; set; }
        public string ENCDATA { get; set; }

    }
}