using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Naka_Master
{
    public class Naka_Master_Model
    {
        public long NakaID { get; set; }
        public string NakaName { get; set; }
        public string Range_Name { get; set; }
        public string RangeCode { get; set; }
        public string ActiveStatus { get; set; }

        public string SNo { get; set; }
    }
}