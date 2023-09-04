using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Entity 
{
    public class DropDownList
    {
        public long Value { get; set; }
        public string Text { get; set; }
    }

    public class DropDownList2 
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class CommonDropDownData
    {
        public long ParentID { get; set; }
        public long Value { get; set; }
        public string Text { get; set; }
    }

    public class CommonDropDownData2
    {
        public string ParentID { get; set; }
        public long Value { get; set; }
        public string Text { get; set; }
    }
    //public class CommonDropDownData3
    //{
    //    public string ParentID { get; set; }
    //    public string Value { get; set; }
    //    public string Text { get; set; }
    //}
}