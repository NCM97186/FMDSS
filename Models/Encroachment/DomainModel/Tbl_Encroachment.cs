using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Encroachment.DomainModel
{
    public class Tbl_Encroachment
    {
        [Key]        
        public long ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string EN_Code {get;set;}        
        public string DIV_CODE { get; set; }      
        public string RANGE_CODE { get; set; }       
        public string LRACTNO { get; set; }       
        public bool IsKnown { get; set; }       
        public byte[] KMLFile { get; set; }
        public string KMLFileName { get; set; }      
        public decimal? Area { get; set; }
        public string Description { get; set; }
        public long InvestigationOfficer { get; set; }
        public string Special_Instruction { get; set; }
        public long EnteredBy { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]    
        public DateTime  DOE { get; set; }
        public string DispatchNo { get; set; }

       // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]    
        public Nullable<DateTime> DispatchDate { get; set; }
        public string ACF_Status { get; set; }
        public string ACF_Remarks { get; set; }
        public Nullable<DateTime> ACF_Date { get; set; }
        public string NoticeNo { get; set; }
        public Nullable<DateTime> NoticeDate { get; set; }
        public byte[] Acf_Decision_Upload { get; set; }
        public string Final_Decision_Taken { get; set; }
        public long Final_Decision_OfficerId { get; set; }
        public string Final_Decision_Remarks { get; set; }
        public Nullable<DateTime> Final_Decision_Date { get; set; }
        public Nullable<DateTime> Next_Decision_Date { get; set; }
        public string Next_Decision_Place { get; set; }

        public int FileStatus { get; set; }

		public long ReviewerOfficer { get; set; }
		public string Special_InstructionForReview { get; set; }
		public long AssignerOfficer { get; set; }


	}
  
  
}