using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Entity.ViewModel
{
    public class CommonListVM
    {
        public static IList<SelectListItem> YesNo = new List<SelectListItem>()
        {
            new SelectListItem {Text = "Yes", Value = true.ToString()},
            new SelectListItem {Text = "No", Value = false.ToString()}
        };

        public static IList<SelectListItem> Gender = new List<SelectListItem>()
        {
            new SelectListItem { Text = "Male", Value = "Male",Selected=true },
            new SelectListItem { Text = "Female", Value = "Female" }
        };
    }
}