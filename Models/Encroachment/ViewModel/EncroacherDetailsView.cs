using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Encroachment.ViewModel
{
	public class EncroachmentDetailView
	{
		public string DIV_NAME { get; set; }
		public string RANGE_NAME { get; set; }
		public string LRACTNO { get; set; }
		public decimal Area { get; set; }
		public DateTime DOE { get; set; }
		public string Description { get; set; }
		public bool IsKnown { get; set; }
		public string GISFilePath { get; set; }
		public string CreatedBy { get; set; }
		public List<KMLDetails> KMLDetails { get; set; }
		public List<EncroacherDetails> EncroacherDetails { get; set; }

	}
	public class KMLDetails
	{
		public decimal Area { get; set; }
		public string Vill_Name { get; set; }
		public string DIST_NAME { get; set; }
		public string DIV_NAME { get; set; }
		public string BLK_NAME { get; set; }
		public string GPSLat { get; set; }
		public string GPSLong { get; set; }
		public string AreaShapeinHactare { get; set; }
	}
	public class EncroacherDetails
	{
		public string Encroacher_Name { get; set; }
		public string Encroacher_FatherName { get; set; }
		public string AadharOrBhamasha { get; set; }
		public string Encroacher_Aadhar { get; set; }
		public string Encroacher_Bhamashah { get; set; }
		public string Encroacher_Address { get; set; }
	}

	public class EncroacherDetailsView
	{

		public string rowid { get; set; }
		public string EN_Code { get; set; }
		public string CreatedBy { get; set; }

		public string AadharOrBhamasha { get; set; }
		public string Encroacher_Aadhar { get; set; }
		public string Encroacher_Bhamashah { get; set; }


		public string Encroacher_Name { get; set; }
		public string Encroacher_FatherName { get; set; }

		public string Encroacher_Address { get; set; }

		public decimal Area { get; set; }
		public string Vill_Name { get; set; }
		public string DIST_NAME { get; set; }
		public string DIV_NAME { get; set; }
		public string BLK_NAME { get; set; }
		public string GPSLat { get; set; }
		public string GPSLong { get; set; }
		public string AreaShapeinHactare { get; set; }

		public DateTime DOE { get; set; }

		public string RANGE_NAME { get; set; }
		public string LRACTNO { get; set; }
		public string Description { get; set; }
		public bool IsKnown { get; set; }
		public string GISFilePath { get; set; }




	}
}