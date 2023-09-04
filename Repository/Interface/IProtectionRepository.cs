using FMDSS.Entity;
using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Models.ForestFire;
using System.Data;

namespace FMDSS.Repository.Interface
{
    interface IProtectionRepository
    {
        ResponseMsg OffenceDetails_Save(OffenceDetailsModel model,int isMobile);
        ResponseMsg OffenceDetails_Update(ApproverRemarks model);
        ResponseMsg OffenceDetails_Delete(long offenceDetailsID);
        ResponseMsg FileUpload(DataSet ds, UploadOffenceDetailsModel model);
        DataSet OffenceDetails_Get(int pageNumber, int pageSize,string FDate,string TDate);
        DataSet OffenceDetails_GetPermission();
        DataSet OffenceDetails_EditHistory(long RequestId);
        DataSet OffenceDetails_History(long logID);
        DataSet OffenceDetails_GetDropdownData();
        DataSet DivisionList_GetDropdownData();
        DataSet OffenceDetails_GetDetailsForUpdation(long offenceDetailsID);
        DataSet OffenceDetailsItemWise_Get();
        OffenceDetailsWithLog GetLogDetails(long? offenceDetailsID);
        DataSet ForestFire_AddDetailsReport(ForestFire_AddDetailsReport model);

        #region Reports
        DataTable OffenceDetails_Report(OffenceReportVM model);
        DataSet OffenceSummary_Report(OffenceReportVM model);
        DataSet OffenceSummarySub_Report(OffenceSubParamVM param);
        DataSet OffenceSummaryQtr_Report(OffenceReportVM model);
        DataSet OffenceSummaryQtrSub_Report(OffenceSubParamVM param);
        DataSet EncroachmentSummary_Report(OffenceReportVM model);
        DataSet EncroachmentSummarySub_Report(OffenceSubParamVM param);
        #endregion
        #region Add by sunny for OffenceAPI
        DataSet OffenceDetails_GetDropdownDataAPI();
        DataTable GetCircle();
        DataSet GetFinancialYear();
        ResponseMsg OffenceDetails_SaveAPI(OffenceDetailsModel_API model,int isMobile);
        ResponseMsg SetDuplicateOffenceFIR(long CurrentCaseRequestId, long RefRequestCaseId, string Remarks);
        #endregion

        #region Added by Mukesh Jangid on 12-01-2021
        System.Threading.Tasks.Task<string> UpdateForesFireDataFromGISAsync();
        #endregion

        #region Added by Amrit Barotia on 09-06-2021
        System.Threading.Tasks.Task<string> UpdateForestFireDataFromAPI();
        #endregion
    }
}
