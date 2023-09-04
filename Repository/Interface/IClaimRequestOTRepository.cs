using FMDSS.Entity;
using FMDSS.Entity.FRAViewModel;
using FMDSS.Models.Encroachment.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.Repository
{
    public interface IClaimRequestOTRepository 
    { 
        void SaveClaimRequestDetails(ClaimRequestOTVM model, ref string returnMsg, ref Boolean isError);
        ClaimRequestOTVM GetClaimRequestDetails(ClaimRequestOTVM model, long claimReqID); 
        bool IsValidSSO(string ssoID);
    }
}