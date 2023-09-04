using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class FRA_APIController : ApiController
    {
        private readonly IFRASummaryReport _requestManager;
        public FRA_APIController()
        {
            if (_requestManager == null)
            {
                _requestManager = new FMDSS.APIRepo.FRARepo();
            }
        }
        [HttpGet]
        public DataSetResponse FRASummary_DistrictList()
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.FRASummary_DistrictList();
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpGet]
        public DataSetResponse FRASummary_GPList(int DistrictCode)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.FRASummary_GPList(DistrictCode);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse FRASummary_VillageList(long GPID)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.FRASummary_VillageList(GPID);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse FRASummary_ClaimRequestDetailsID(long VillageID)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.FRASummary_ClaimRequestDetailsID(VillageID);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }

        #region Needs Work
        [HttpPost]
        public DataSetResponse FRA_GetApplicationStatus([FromBody] APIRepo.FRAStatus data)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.FRA_GetApplicationStatus(data);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpPost]
        public DataSetResponse FRA_GetDistrict()
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.GetDropdownData(21);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpPost]
        public DataSetResponse FRA_GetBlock([FromBody] APIRepo.CommonParam param)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.GetDropdownData(10, param.ParentID);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpPost]
        public DataSetResponse FRA_GetGP([FromBody]APIRepo.CommonParam param)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                response = _requestManager.GetDropdownData(11, param.ParentID);
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpPost]
        public DataSetResponse ClaimRequestSummaryReport([FromBody]Entity.FRAViewModel.ClaimRequestParamVM param)
        {
            return _requestManager.ClaimRequestSummaryReport(param);
        }

        [HttpPost]
        public DataSetResponse GetClaimRequestSummarySub_Rpt([FromBody]Entity.FRAViewModel.ClaimRequestSubParamVM param)
        {
            return _requestManager.GetClaimRequestSummarySub_Rpt(param);
        }
        #endregion

    }
}