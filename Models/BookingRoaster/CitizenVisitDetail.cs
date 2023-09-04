using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.BookingRoaster
{
	public class CitizenVisitDetail
	{
		public int SNo { get; set; }
		public long TicketId { get; set; }
		public long UserId { get; set; }
		public string UserName { get; set; }
		public int PlaceID { get; set; }
		public string PlaceName { get; set; }
		public int ZoneId { get; set; }
		public string ZoneName { get; set; }
		public int ShiftId { get; set; }
		public string ShiftName { get; set; }
		public int MemberCount { get; set; }
		public string DateOfVisit { get; set; }
		public string GuideId { get; set; }
		public string GuideName { get; set; }
		public string VehicleNumber { get; set; }
		public string VehicleType { get; set; }
		
		public List<CitizenVisitDetail> citizenVisitDetails { get; set; }
	}
}