using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Entity.ViewModel
{
    [Serializable]
    public class CommonDocument
    {
        public long DocumentID { get; set; }
        public int ObjectTypeID { get; set; }
        public long ObjectID { get; set; }
        public string DocumentName { get; set; }
        public int DocumentTypeID { get; set; }
        public string DocumentPath { get; set; }
        public String DocumentTypeName { get; set; }
        public bool IsESign { get; set; }
        public bool ActiveStatus { get; set; }
        public string TempID { get; set; }
        public bool IsNew { get; set; }
        public int DocumentLevel { get; set; }
    }

    [Serializable]
    public class CommonDocumentType
    {
        public int DocumentTypeID { get; set; }
        public string DocumentTypeName { get; set; }
        public int DocumentLevel { get; set; }
        public bool ActiveStatus { get; set; }
    }

    public class CommonRequestData
    {
        public string ModuleName { get; set; }
        public string ServiceType { get; set; }
        public string PermissionType { get; set; }
        public string PermissionName { get; set; }
        public string RequestId { get; set; }
        public string RequestedOn { get; set; }
        public string RequestedBy { get; set; }
        public string Remarks { get; set; }
        public string StatusDesc { get; set; }
        public string Reason_Desc { get; set; }
    }
}