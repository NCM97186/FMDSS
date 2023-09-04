using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMDSS.APIRepo
{
    public class FRARepo : IFRASummaryReport
    {
        public DataSetResponse FRASummary_DistrictList()
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = FMDSS.APIDAL.FRASummaryReportDAL.FRASummary_DistrictList();

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }
        public DataSetResponse FRASummary_GPList(int DistrictCode)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = FMDSS.APIDAL.FRASummaryReportDAL.FRASummary_GPList(DistrictCode);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }
        public DataSetResponse FRASummary_VillageList(long GPID)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = FMDSS.APIDAL.FRASummaryReportDAL.FRASummary_VillageList(GPID);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }
        public DataSetResponse FRASummary_ClaimRequestDetailsID(long VillageID)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = FMDSS.APIDAL.FRASummaryReportDAL.FRASummary_ClaimRequestDetailsID(VillageID);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }

        #region FRA API Work
        public DataSetResponse GetDropdownData(int actionCode, string parentID = null)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = APIDAL.FRADAL.GetDropdownData(actionCode, parentID);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }

        public DataSetResponse ClaimRequestSummaryReport(Entity.FRAViewModel.ClaimRequestParamVM param)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = APIDAL.FRADAL.ClaimRequestSummaryReport(param);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }

        public DataSetResponse GetClaimRequestSummarySub_Rpt(Entity.FRAViewModel.ClaimRequestSubParamVM param)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = APIDAL.FRADAL.GetClaimRequestSummarySub_Rpt(param);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }

        public DataSetResponse FRA_GetApplicationStatus(APIRepo.FRAStatus model)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response.Data = FMDSS.APIDAL.FRASummaryReportDAL.FRA_GetApplicationStatus(model);

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "An exception has been occurs.", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion
    }

    public class FRAStatus
    {
        public string RefNo { get; set; }
        public string Name { get; set; }
        public string ClaimTypeID { get; set; }
        public string DistrictID { get; set; }
        public string BlockID { get; set; }
        public string GPID { get; set; }
    }

    public class CommonParam
    {
        public string ParentID { get; set; }
    }
}