using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.Entity.VM
{
    public class ServiceActivity
    {
        public string ModuleName { get; set; } 
        public string ServiceName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }
}
