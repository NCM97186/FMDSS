using FMDSS.Models.BookOnlineTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    public class MemberDetailViewModel
    {
        public string Nationality { get; set; }
        public string NationalityId { get; set; }
        public string LeaderName { get; set; }
        public string IdType { get; set; }
        public string IdNo { get; set; }

        public string PNR_NO { get; set; }
        public string Seat_NO { get; set; }
        public string Room_No { get; set; }


        public int NoOfVideoCamera { get; set; }
        public int TotalPersons { get; set; }
        public decimal FeesPerMember { get; set; }
        public decimal CameraFees { get; set; }
        public decimal GuideFees { get; set; }
        public List<BookOnTicket> TicketDetail { get; set; }

    }
}