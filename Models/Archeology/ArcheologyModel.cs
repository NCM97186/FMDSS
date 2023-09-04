using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Archeology
{
    public class ArcheologyModel
    {
        //
        // GET: /ArcheologyModel/

        [Required(ErrorMessage = "Select place of visit")]
        public string PlaceOfVisit { get; set; }

        [Required(ErrorMessage = "Select Visitor Type")]
        public string VistorType { get; set; }

        [Required(ErrorMessage = "Select date of visit")]
        public string DateOfVisit { get; set; }

        [Required(ErrorMessage = "Please Enter Number of Citizen")]

        public int NumberofCitizen { get; set; }

        public string selectedPlaces { get; set; }



        [Required(ErrorMessage = "Enter Visitor Name")]
        public string VisitorName { get; set; }

        [Required(ErrorMessage = "Enter Visitor email")]
        public string Visitoremail { get; set; }

        [Required(ErrorMessage = "Enter Visitor mobile")]
        public string Visitormobile { get; set; }

        [Required(ErrorMessage = "select VisitorId Type")]
        public string VisitorIdType { get; set; }

        [Required(ErrorMessage = "Enter VisitorId Number")]
        public string VisitorIdNumber { get; set; }


        public int IndianVisitor { get; set; }
        public int IndianStudent { get; set; }
        public int ForeignerVisitor { get; set; }
        public int ForeignerStudent { get; set; }

        public string ConsumerKey { get; set; }

        public int createdby { get; set; }

        public float TotalAmount { get; set; }

    }

    public class RateWiseList
    {
        public int PK_ID { get; set; }
        public string PlaceType { get; set; }
        public int fees { get; set; }
    }

    public class PrintingTicketOfArcheology
    {
        public string Place { get; set; }
        public int Rate { get; set; }
        public int Qty { get; set; }
        public int Amount { get; set; }
    }

    public class TermsandConditionArhceology
    {
        public string Place { get; set; }
        public string TermandCondtionText { get; set; }

    }

    public class ArcheologyMIS
    {
        public string DateType { get; set; }
        public string Place { get; set; }
        public string District { get; set; }
        public string MobileNumber { get; set; }
        public string dateofvisitfrom { get; set; }
        public string dateofvisitto { get; set; }

    }
}
