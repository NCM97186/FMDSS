using FMDSS.Entity;
using FMDSS.Entity.JFMC.ViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FMDSS.Globals;
using System.Text;

namespace FMDSS.Repository
{
    public class JFMCRepository : IJFMCRepository
    {
        #region Properties & Variables
        private FMDSS.Models.DAL _db = new Models.DAL();
        #endregion

        public DataSet JFMCRegistration_Get()
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_JFMC_Registration_Get", prms);
            return dsData;
        }

        public DataSet JFMCRegistration_GetDetailsForUpdation(long objectID)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("RegistrationID", objectID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_JFMC_Registration_Get", prms);
            return dsData;
        }

        public ResponseMsg JFMCRegistration_Save(JFMCRegistration model)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            Int32 actionCode = model.JFMCRegistrationID == 0 ? 1 : 2;

            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ObjectID", model.JFMCRegistrationID),
                            new SqlParameter("CircleCode", model.CircleCode),
                            new SqlParameter("DivisionCode", model.DivisionCode),
                            new SqlParameter("RangeCode", model.RangeCode),
                            new SqlParameter("NakaID", model.NakaID),
                            new SqlParameter("Latitude", model.Latitude),
                            new SqlParameter("Longitude", model.Longitude),
                            new SqlParameter("CommitteeName", model.CommitteeName),
                            new SqlParameter("RegistrationNumber", model.RegistrationNumber),
                            new SqlParameter("RegistrationDate", Util.GetDate(model.RegistrationDate)),
                            new SqlParameter("BankName", model.BankName),
                            new SqlParameter("BranchName", model.BranchName),
                            new SqlParameter("IFSCCode", model.IFSCCode),
                            new SqlParameter("AccountNo", model.AccountNo),
                            new SqlParameter("AccountType", model.AccountType),
                            new SqlParameter("CorpusFundDeposit", model.CorpusFundDeposit),
                            new SqlParameter("TotalRevenueDuringYear", model.TotalRevenueDuringYear),
                            new SqlParameter("IsEcotourismManagementExist", model.IsEcotourismManagementExist), 
                            new SqlParameter("Grade", model.Grade),
                            new SqlParameter("Audited", model.Audited),
                            new SqlParameter("LastAuditDate", Util.GetDate(model.LastAuditDate)),
                            new SqlParameter("LatestGeneralBodyMeetingDate", Util.GetDate(model.LatestGeneralBodyMeetingDate)),
                            new SqlParameter("LatestExecutiveBodyMeetingDate", Util.GetDate(model.LatestExecutiveBodyMeetingDate)),
                            new SqlParameter("Remarks", model.Remarks),
                            new SqlParameter("TotalSCCategory", model.TotalSCCategory),
                            new SqlParameter("TotalSTCategory", model.TotalSTCategory),
                            new SqlParameter("TotalFemaleCategory", model.TotalFemaleCategory),
                            new SqlParameter("TotalOthersCategory", model.TotalOthersCategory),
                            new SqlParameter("ManagedAreaOrEcotourismSiteName", model.ManagedAreaOrEcotourismSiteName),
                            new SqlParameter("xmlFile", GetRequestInXML(model)),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_JFMC_Registration_Save", prms);

            if (dtData != null)
            {
                msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                {
                    IsError = x.Field<bool>("IsError"),
                    ReturnMsg = x.Field<string>("ReturnMsg"),
                    ReturnIDs = x.Field<string>("ReturnIDs")
                }).FirstOrDefault();

                SaveJFMCDoc(model, msg.ReturnIDs);
            }

            return msg;
        }

        private string GetRequestInXML(JFMCRegistration model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<JFMCRequest>");
            sb.Append("<documents>");

            if (model.UploadPlanEvidence != null)
            {
                sb.Append("<document>");
                sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName>", 0, 2, 0, 33, model.UploadPlanEvidence.FileName));
                sb.Append("</document>");
            }

            if (model.UploadAuditEvidence != null)
            {
                sb.Append("<document>");
                sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName>", 0, 2, 0, 34, model.UploadAuditEvidence.FileName));
                sb.Append("</document>");
            }

            if (model.UploadGeneralBodyMOMEvidence != null)
            {
                sb.Append("<document>");
                sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName>", 0, 2, 0, 35, model.UploadGeneralBodyMOMEvidence.FileName));
                sb.Append("</document>");
            }

            if (model.UploadCMemberMOMEvidence != null)
            {
                sb.Append("<document>");
                sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName>", 0, 2, 0, 36, model.UploadCMemberMOMEvidence.FileName));
                sb.Append("</document>");
            }

            sb.Append("</documents>");

            if (model.MemberList != null)
            {
                sb.Append("<members>");
                foreach (var item in model.MemberList)
                {
                    sb.Append("<member>");
                    sb.Append(string.Format(@" 
                            <MemberID>{0}</MemberID>
                            <MemberName>{1}</MemberName>", item.MemberID, item.MemberName));
                    sb.Append("</member>");
                }
                sb.Append("</members>");
            }

            sb.Append("</JFMCRequest>");
            return Convert.ToString(sb);
        }

        private void SaveJFMCDoc(JFMCRegistration model, string objectID)
        {
            CommonRepository repository = new CommonRepository();
            string docPath = FMDSS.Globals.Util.GetAppSettings("FRADocumentPath");
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

            if (model.UploadPlanEvidence != null)
            {
                repository.SaveFile(model.UploadPlanEvidence, 2, Convert.ToInt64(objectID), 33, dirPath);
            }
            if (model.UploadAuditEvidence != null)
            {
                repository.SaveFile(model.UploadAuditEvidence, 2, Convert.ToInt64(objectID), 34, dirPath);
            }
            if (model.UploadGeneralBodyMOMEvidence != null)
            {
                repository.SaveFile(model.UploadGeneralBodyMOMEvidence, 2, Convert.ToInt64(objectID), 35, dirPath);
            }
            if (model.UploadCMemberMOMEvidence != null)
            {
                repository.SaveFile(model.UploadCMemberMOMEvidence, 2, Convert.ToInt64(objectID), 36, dirPath);
            }
        }
    }
}