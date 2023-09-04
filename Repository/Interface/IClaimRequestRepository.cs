using FMDSS.Entity;
using FMDSS.Entity.FRA.FRAViewModel;
using FMDSS.Entity.FRAViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FMDSS.Repository
{
    public interface IClaimRequestRepository
    {
        #region [Appeal Request Operation]
        DataSet AppealRequest_Get(int ActionCode);
        ResponseMsg AppealRequest_Save(AppealRequestVM model);
        #endregion

        #region Citizen
        ClaimRequestVM GetClaimRequestDetails(ClaimRequestVM model, long claimReqID);
        DataSet GetClaimRequestDetails(int actionCode, string parentID = null);
        List<ClaimRequestDetailsVM> GetClaimRequestList();
        WorkFlowVM SaveClaimRequestDetails(ClaimRequestVM model);
        DataSet UpdateClaimRequest(string parentID = null, string actionCode = null);
        void UpdateClaimRequestTransaction(long? reqID, Models.CommanModels.PaymentResponse model = null);
        void UpdateWorkFlowDetailsForCitizen(WorkFlowApproverVM model, ref string returnMsg, ref Boolean isError);
        #endregion

        #region Gramsabha
        void AddSurveyDetails(tbl_FRA_SurveyDetails model, string reportType, string rootPath, string OTP, string TransationID, ref string returnMsg, ref Boolean isError);
        tbl_FRA_SurveyDetails GetSurveyDetails(long claimRequestDetailsID);
        ResponseMsg UpdateSurveyDetails(tbl_FRA_SurveyDetails model);
        #endregion

        #region Collector
        void UpdateWorkFlowDetailsForESign(WorkFlowApproverVM model, string rootPath, string command, string OTP, string TransationID, ref string returnMsg, ref Boolean isError);
        void UpdateWorkFlowDetailsMultipleForESign(WorkFlowApproverMultipleVM model, string rootPath, string command, string OTP, string TransationID, ref string returnMsg, ref Boolean isError);
        ResponseMsg UpdateDocWitheSign(WorkFlowApproverVM model, string rootPath, string command, string OTP, string TransationID);
        ResponseMsg UpdateDocMultipleWitheSign(WorkFlowApproverMultipleVM model, string rootPath, string command, string OTP, string TransationID);
        #endregion

        #region Common
        List<ClaimRequestDetailsVM> GetClaimRequestForApproval();
        List<ClaimRequestDetailsVM> GetClaimRequestDetails(string parentID);
        List<WorkFlowDetailsVM> GetWorkFlowDetailsList(long claimRequestDetailsID);
        ResponseMsg UpdateAppealDetails(ApproverRemarksCommonVM model);
        //List<tbl_FRA_ClaimRequestDocument> GetClaimRequestDocument(long? workFlowID);
        void UpdateWorkFlowDetails(WorkFlowApproverVM model, string rootPath, ref string returnMsg, ref Boolean isError);
        ClaimRequestDetailsVM GetWorkFlowDetails(long claimRequestDetailsID);
        string DownloadReceipt(WorkFlowApproverVM model);
        #endregion

        #region User Registration
        DataSet UserRegistration_Get(long DesignationID, long DistictId);
        DataSet UserRegistration_Get(long userRegistrationID);
        ResponseMsg UserRegistration_Save(UserRegistration model);
        DataSet GetDropdownData(int actionCode, string parentID = null, string childID = null, string selectedValue = null);
        #endregion


        //#region "Fra Entry Form"
        //DataSet GetFraPoints();
        //#endregion


        #region Report
        DataSet GetClaimRequestDetails_Rpt(ClaimRequestParamVM param);
        DataSet GetClaimRequestSummary_Rpt(ClaimRequestParamVM param);
        DataSet GetClaimRequestSummarySub_Rpt(ClaimRequestSubParamVM param);
        DataSet GetClaimRequestSummary_RptAdmin(ClaimRequestParamVM param);
        DataSet GetClaimRequestDetails_RptAdmin(ClaimRequestParamVM param);
        #endregion
    }
}