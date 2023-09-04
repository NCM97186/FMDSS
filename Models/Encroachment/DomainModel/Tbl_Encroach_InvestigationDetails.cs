using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class Tbl_Encroach_InvestigationDetails
    {
        [Key]        
        public long ID { get; set; }
        public string EN_Code { get; set; }
        public string AadharOrBhamasha { get; set; }
        public string Encroacher_Aadhar { get; set; }
        public string Encroacher_Bhamashah { get; set; }        
        public int Year { get; set; }
        public string khasraNo { get; set; }
        public decimal TotalArea { get; set; }
        public string TypeofLand { get; set; }
        public decimal TaxPerHact { get; set; }
        public decimal Encroachment_Area { get; set; }
        public string Encroachment_Yield { get; set; }
        public decimal Tax { get; set; }
        public string Block_Name { get; set; }      
        public decimal Total_Area_Block { get; set; }

        //== uploads
        public byte[] DocumentUpload { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFileType { get; set; }

        public byte[] MAPDocumentUpload { get; set; }
        public string MAPUploadFileName { get; set; }
        public string MAPUploadFileType { get; set; }

        public byte[] JamabandiDocumentUpload { get; set; }
        public string JamabandiUploadFileName { get; set; }
        public string JamabandiUploadFileType { get; set; }

        public byte[] NotificationDocumentUpload { get; set; }
        public string NotificationUploadFileName { get; set; }
        public string NotificationUploadFileType { get; set; }

        public byte[] PanchnamaDocumentUpload { get; set; }
        public string PanchnamaUploadFileName { get; set; }
        public string PanchnamaUploadFileType { get; set; }

        public byte[] NazarBandiDocumentUpload { get; set; }
        public string NazarBandiUploadFileName { get; set; }
        public string NazarBandiUploadFileType { get; set; }


        
        public string CompartmentNo { get; set; }       
        public string InformationGatheredBy { get; set; }       
        public string InformationApprovedBy { get; set; }      
        public string NotificationNo { get; set; }
        public DateTime NotificationDate { get; set; }

        //== uploads
        public string Remarks { get; set; }       
        public long Entered_By { get; set; }
    }
}