using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Entity.FRAViewModel
{
    public class FRACommonList
    {
        public static List<SelectListItem> ClaimType = new List<SelectListItem>()
        {
            new SelectListItem { Text = "---Select---", Value = "",Selected= true },
            new SelectListItem { Text = "For Individual", Value = "1" },
            new SelectListItem { Text = "For Community", Value = "2" },
        };

        public static List<SelectListItem> DependantOption = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Yes", Value = "true" },
            new SelectListItem { Text = "No", Value = "false",Selected=true },
        };

        public static List<SelectListItem> Gender = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Male", Value = "Male",Selected=true },
            new SelectListItem { Text = "Female", Value = "Female" },
        };

        public static List<SelectListItem> TPRequestType = new List<SelectListItem>()
        {
            new SelectListItem { Text = "---Select---", Value = "",Selected= true },
            new SelectListItem { Text = "व्यक्तिगत", Value = "1" },
            new SelectListItem { Text = "संस्था", Value = "2" },
        };
    }
}