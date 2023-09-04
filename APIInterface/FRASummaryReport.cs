using FMDSS.CustomModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.APIInterface
{
    public interface IFRASummaryReport
    {
        DataSetResponse FRASummary_DistrictList();
        DataSetResponse FRASummary_GPList(int DistrictCode);
        DataSetResponse FRASummary_VillageList(long GPID);
        DataSetResponse FRASummary_ClaimRequestDetailsID(long VillageID);
        #region FRA API
        DataSetResponse GetDropdownData(int actionCode, string parentID = null);
        DataSetResponse FRA_GetApplicationStatus(APIRepo.FRAStatus model);
        DataSetResponse ClaimRequestSummaryReport(Entity.FRAViewModel.ClaimRequestParamVM param);
        DataSetResponse GetClaimRequestSummarySub_Rpt(Entity.FRAViewModel.ClaimRequestSubParamVM param);
        #endregion
    }
}