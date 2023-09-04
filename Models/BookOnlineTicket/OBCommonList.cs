using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models
{
    public class OBCommonList
    {
        public static List<SelectListItem> DateType = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Visiting Date", Value = "1" },
            new SelectListItem { Text = "Booking Date", Value = "2" },
        };

        public static List<SelectListItem> BookingStatus = new List<SelectListItem>()
        {
              new SelectListItem { Text = "All", Value = "-1" },
            new SelectListItem { Text = "Pending", Value = "0" },
            new SelectListItem { Text = "Booked", Value = "1" },
            new SelectListItem { Text = "Cancelled", Value = "2" },
        };

        public static List<SelectListItem> BookingType = new List<SelectListItem>()
        {
            new SelectListItem { Text = "OnlineCitizenBooking", Value = "1" },
            new SelectListItem { Text = "DepartmentBooking", Value = "2" },
            new SelectListItem { Text = "EmitraKioskBooking", Value = "3" } 
        };  
    }
}