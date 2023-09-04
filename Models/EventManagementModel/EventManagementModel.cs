using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.EventManagementModel
{
    public class EventDetails
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string EmailTemplate { get; set; }
        public string SMSTemplate { get; set; }
        public string EventName { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string ActiveStatus { get; set; }
    }

    public class AddEventDetails
    {
         public int ID { get; set; }
        [AllowHtml]
        public string EmialTemplate { get; set; }

        [Display(Name = "Event Start Date Time")]
        [Required]
        public string EventSDateTime { get; set; }

        [Display(Name = "Event End Date Time")]
        [Required]
        public string EventEDateTime { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        [StringLength(500, ErrorMessage = "SMS lenght of text shoould be less then 500.")]
        public string SMSTemplate { get; set; }
        public string ActiveStatus { get; set; }

    }
}