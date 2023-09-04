using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class tbl_mst_ForestEmployees
    {
        [Key]
        public int ROWID { get; set; }      
        public string SSO_ID { get; set; }
        [JsonIgnore]
        public string Office_Id { get; set; }

    }

    public class ForestEmployeesTemparyClass
    {
        [Key]
        public int ROWID { get; set; }
        public string SSO_ID { get; set; }
       

    }
}