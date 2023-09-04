using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FMDSS.Entity
{ 
    public class tbl_FRA_ClaimRequestDetailsOT:CommonEntity
    { 
        [Key]
        public long ClaimRequestDetailsID { get; set; } 
        [Required]
        public Nullable<int> ClaimTypeID { get; set; }
        public Nullable<int> ClaimRequestPurposeID { get; set; }
        [Required]
        [Display(Name ="District")]
        public Nullable<long> DistrictID { get; set; }
        [Required]
        [Display(Name = "Tehsil")]
        public Nullable<long> TehsilID { get; set; }
        [Required]
        [Display(Name = "Village")]
        public string VillageCode { get; set; }
        [Required]
        [Display(Name = "Gram Panchayat")]
        public Nullable<long> GPID { get; set; }
        [Required]
        [Display(Name = "Block")]
        public Nullable<long> BlockID { get; set; }
        public string CompartmentNumber { get; set; }
        public string KhasraNumber { get; set; }
        public Nullable<bool> IsPattaGenerated { get; set; }
        public Nullable<bool> IsHalkaPatwariGenerated { get; set; }
        public Nullable<bool> IsForesterGenerated { get; set; }

        #region Community Field
        [Display(Name = "FDST community")]
        public Nullable<bool> Comminity_FDSTCommunity { get; set; }
        [Display(Name = "OTFD community")]
        public Nullable<bool> Comminity_OTFDCommunity { get; set; }
        [Display(Name = "Community rights such as nistar, if any")]
        public string Community_CRSANistar { get; set; }
        [Display(Name = "Rights over minor forest produce, if any")]
        public string Community_ROMFProduce { get; set; }
        [Display(Name = "Uses or entitlements (fish, water bodies), if any")]
        public string Community_UsesOrEntitlement { get; set; }
        [Display(Name = "Grazing, if any")]
        public string Community_Grazing { get; set; }
        [Display(Name = "Traditional resource access for nomadic and pastoralist, if any")]
        public string Community_TRAFNAPastroralist { get; set; }
        [Display(Name = "Community tenures of habitat and habitation for PTGs and pre-agricultural communities, if any")]
        public string Community_CTOHAHabitation { get; set; }
        [Display(Name = "Right to access biodiversity, intellectual property and traditional knowledge, if any")]
        public string Community_RTABiodiversity { get; set; }
        [Display(Name = "Other traditional right, if any")]
        public string Community_OTRight { get; set; }
        #endregion

        #region Individual Field

        [Display(Name = "Is Claim Before 13 Dec 2005")]
        public Nullable<bool> Individual_IsClaimBefore { get; set; }
        [Display(Name = "Schedule Tribe")]
        public Nullable<bool> Individual_STribe { get; set; }
        [Display(Name = "Other Traditional Forest Dweller")]
        public Nullable<bool> Individual_OTFDweller { get; set; }
        [Display(Name = "for habitation")]
        public string Individual_FHabitation { get; set; }
        [Display(Name = "for self-cultivation, if any")]
        public string Individual_FSCultivation { get; set; }
        [Display(Name = "disputed lands if any")]
        public string Individual_DisputedLands { get; set; }
        [Display(Name = "Pattas/leases/grants, if any")]
        public string Individual_PLGrants { get; set; }
        [Display(Name = "Land for in situ rehabilitation or alternative land, if any")]
        public string Individual_LFISROAlternativeLand { get; set; }
        [Display(Name = "Land from where displaced without land compensation")]
        public string Individual_LFWDisplacedWLCompensation { get; set; }
        [Display(Name = "Extent of land in forest villages, if any")]
        public string Individual_EOLIFVillages { get; set; }
        [Display(Name = "Any other traditional right, if any")]
        public string Individual_AOTRight { get; set; }
        #endregion 
        public Nullable<int> StatusID { get; set; }
        public Nullable<int> PendingAt { get; set; }
        public string RequesterComment { get; set; }
        [Required]
        [Display(Name = "On Behalf Of")] 
        public string SSOID { get; set; } 
         
        [Display(Name = "On Behalf Of")]
        public Nullable<long> OnBehalfOf { get; set; }
        [NotMapped]
        public Nullable<int> DocumentTypeID { get; set; } 
        public virtual List<tbl_FRA_ClaimantDetails> ClaimantDetailsList { get; set; }
        public virtual List<tbl_FRA_MemberDetails> MemberDetailsList { get; set; }
        public virtual List<tbl_FRA_BorderingVillageDetails> BorderingVillageDetails { get; set; } 
    } 
}
