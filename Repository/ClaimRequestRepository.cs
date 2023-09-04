using FMDSS.E_SignIntegration;
using FMDSS.Entity;
using FMDSS.Entity.FRA.FRAViewModel;
using FMDSS.Entity.FRAViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Globals;
using FMDSS.Models;
using FMDSS.Models.FmdssContext;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace FMDSS.Repository
{
    public class ClaimRequestRepository : IClaimRequestRepository
    {
        #region Properties & Variables
        private FmdssContext dbContext;
        private FMDSS.Models.DAL _db = new Models.DAL();
        private long userID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserId"]);
        #endregion

        #region Constructor
        public ClaimRequestRepository()
        {
            dbContext = new Models.FmdssContext.FmdssContext();
        }
        #endregion

        #region [Appeal Request Operation]
        public DataSet AppealRequest_Get(int ActionCode)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", ActionCode),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_FRA_GetAppealRequest", prms);
            return data;
        }

        public ResponseMsg AppealRequest_Save(AppealRequestVM model)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var reqLen = string.IsNullOrEmpty(model.ClaimRequestID) ? 0 : model.ClaimRequestID.Split('/').Length;
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("EntryType", model.EntryType),
                             new SqlParameter("ClaimRequestDetailsID", reqLen>0?model.ClaimRequestID.Split('/')[reqLen-1]:null),
                             new SqlParameter("AppealReason", model.AppealReason),
                             new SqlParameter("ClaimTypeID", model.ClaimTypeID),
                             new SqlParameter("ClaimRequestDate",Util.GetDateWithFormat(model.ClaimRequestDate, "MM/dd/yyyy")),
                             new SqlParameter("RejectionDate", Util.GetDateWithFormat(model.RejectionDate, "MM/dd/yyyy")),
                             new SqlParameter("RejectionReason", model.RejectionReason),
                             new SqlParameter("RejectedAt", model.RejectedAt),
                             new SqlParameter("ClaimantName", model.ClaimantName),
                             new SqlParameter("FatherName", model.FatherName),
                             new SqlParameter("Mobile", model.Mobile),
                             new SqlParameter("Individual_STribe", model.Individual_STribe),
                             new SqlParameter("DistrictID", model.DistrictID),
                             new SqlParameter("TehsilID", model.TehsilID),
                             new SqlParameter("VillageCode", model.VillageCode),
                             new SqlParameter("GPID", model.GPID),
                             new SqlParameter("BlockID", model.BlockID),
                             new SqlParameter("KhasraNumber", model.KhasraNumber),
                             new SqlParameter("CompartmentNumber", model.CompartmentNumber),
                             new SqlParameter("TotalAreaAgainstOccupiedForestLand", model.TotalAreaAgainstOccupiedForestLand),
                             new SqlParameter("OccupancyType", model.OccupancyType),
                             new SqlParameter("ForestSectionName", model.ForestSectionName),
                             new SqlParameter("Remarks", model.Remarks),
                             new SqlParameter("Latitude", model.Latitude),
                             new SqlParameter("Longitude", model.Longitude),
                            new SqlParameter("xmlFile", GetRequestInXML(model)),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_FRA_AppealRequest_Save", prms);

            if (dtData != null)
            {
                msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                {
                    IsError = x.Field<bool>("IsError"),
                    ReturnMsg = x.Field<string>("ReturnMsg"),
                    ReturnIDs = x.Field<string>("ReturnIDs")
                }).FirstOrDefault();

                SaveAppealDoc(model, msg.ReturnIDs);
            }

            return msg;
        }
        #endregion

        #region Claim Request Operation
        public ClaimRequestVM GetClaimRequestDetails(ClaimRequestVM model, long claimReqID)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("ClaimTypeID", model.ClaimRequestDetails==null? 0: model.ClaimRequestDetails.ClaimTypeID),
                            new SqlParameter("ClaimRequestDetailsID", claimReqID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_FRA_GetClaimRequestDetails", prms);

            if (Util.isValidDataSet(data))
            {
                Parallel.Invoke(
                () => model.DistrictList = Util.GetListFromTable<tbl_FRA_District>(data, 0).Select(t => new DropDownListVM { Text = t.DistrictName, Value = t.DistrictID }).ToList(),
                () => model.DocumentTypeList = Util.GetListFromTable<CommonDocumentType>(data, 1).Select(t => new DropDownListVM { Text = t.DocumentTypeName, Value = t.DocumentTypeID }).ToList(),
                () => model.ClaimTypeList = Util.GetListFromTable<tbl_FRA_ClaimType>(data, 2).Select(t => new DropDownListVM { Text = t.ClaimTypeName, Value = t.ClaimTypeID }).ToList());

                if (claimReqID > 0)
                {
                    Parallel.Invoke(
                    () => model.ClaimRequestDetails = Util.GetListFromTable<tbl_FRA_ClaimRequestDetails>(data, 3).FirstOrDefault(),
                    () => model.ClaimantDetailsList = Util.GetListFromTable<tbl_FRA_ClaimantDetails>(data, 4),
                    () => model.ClaimRequestDocument = Util.GetListFromTable<CommonDocument>(data, 5),
                    () => model.MemberDetailsList = Util.GetListFromTable<tbl_FRA_MemberDetails>(data, 6),
                    () => model.BorderingVillageDetails = Util.GetListFromTable<tbl_FRA_BorderingVillageDetails>(data, 7),
                    () => model.TehsilList = Util.GetListFromTable<tbl_FRA_Tehsil>(data, 8).Select(t => new DropDownListVM { Text = t.TehsilName, Value = t.TehsilID }).ToList(),
                    () => model.BlockList = Util.GetListFromTable<tbl_FRA_Block>(data, 9).Select(t => new DropDownListVM { Text = t.BlockName, Value = t.BlockID }).ToList(),
                    () => model.GPList = Util.GetListFromTable<tbl_FRA_GPs>(data, 10).Select(t => new DropDownListVM { Text = t.GPName, Value = t.GPID }).ToList(),
                    () => model.VillageList = Util.GetListFromTable<tbl_FRA_Village>(data, 11).Select(t => new DropDownList2VM { Text = t.VillageName, Value = t.VillageCode }).ToList(),
                    () => model.ClaimRequestPurposeList = Util.GetListFromTable<tbl_FRA_ClaimRequestPurpose>(data, 12).Select(t => new DropDownListVM { Text = t.Purpose, Value = t.ClaimRequestPurposeID }).ToList());
                }
            }
            return model;
        }

        public DataSet GetClaimRequestDetails(int actionCode, string parentID = null)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("UserID", userID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_FRA_GetClaimRequestDetails", prms);
            return data;
        }

        public List<ClaimRequestDetailsVM> GetClaimRequestList()
        {
            var prms = new[]{ new SqlParameter("ActionCode", 3),
                              new SqlParameter("UserID", userID)};
            var cmdText = "EXEC [SP_FRA_ClaimRequestDetails] @ActionCode = @ActionCode, @UserID = @UserID";
            return dbContext.Database.SqlQuery<ClaimRequestDetailsVM>(cmdText, prms).ToList();
        }
        private void SaveClaimReqDocs(long? reqID, long? workFlowDetailsID, List<CommonDocument> docs)
        {
            string docPath = Util.GetAppSettings("FRADocumentPath");
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

            foreach (var item in docs)
            {
                FileInfo f1 = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/" + item.DocumentPath));
                f1.CopyTo(string.Format("{0}5_{1}_{2}_{3}", dirPath, reqID, item.DocumentTypeID, item.DocumentName), true);
                f1.Delete();

                //tbl_FRA_ClaimRequestDocument obj = new tbl_FRA_ClaimRequestDocument();
                //obj.WorkFlowDetailsID = workFlowDetailsID;
                //obj.DocumentName = item.DocumentName;
                //obj.DocumentPath = string.Format("{0}5_{1}_{2}_{3}", docPath, reqID, item.DocumentTypeID, item.DocumentName);
                //obj.DocumentTypeID = item.DocumentTypeID;
                //obj.TempID = item.TempID;
                //obj.ActiveStatus = true;
                //dbContext.Entry(obj).State = System.Data.Entity.EntityState.Added;
            }
        }

        //private void SaveClaimReqDocsWithDBChange(long? reqID, long? workFlowDetailsID, List<CommonDocument> docs)
        //{
        //    string docPath = Util.GetAppSettings("FRADocumentPath");
        //    string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

        //    foreach (var item in docs)
        //    {
        //        var fullPath = string.Format("{0}5_{1}_{2}_{3}", dirPath, reqID, item.DocumentTypeID, item.DocumentName);
        //        FileInfo f1 = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/" + item.DocumentPath));
        //        f1.CopyTo(fullPath, true);
        //        f1.Delete();

        //        var prms = new[]{
        //                    new SqlParameter("ActionCode", 1),
        //                    new SqlParameter("ObjectID", reqID),
        //                    new SqlParameter("ObjectTypeID", 5),
        //                    new SqlParameter("DocumentTypeID", item.DocumentTypeID),
        //                    new SqlParameter("DocumentName", item.DocumentName),
        //                    new SqlParameter("DocumentPath", fullPath),
        //                    new SqlParameter("IsESign", 0),
        //                    new SqlParameter("UserID", HttpContext.Current.Session["UserId"]) };
        //        DataSet data2 = new DataSet();
        //        _db.Fill(data2, "SP_Common_Document_Save", prms);
        //    }
        //}

        public WorkFlowVM SaveClaimRequestDetails(ClaimRequestVM model)
        {
            try
            {
                Int64 claimRequestDetailsID = model.ClaimRequestDetails.ClaimRequestDetailsID;
                var xmlData = GetRequestInXML(model);
                var prms = new[]{
                            new SqlParameter("ActionCode", claimRequestDetailsID > 0 ? 2:1),
                            new SqlParameter("UserID", userID),
                            new SqlParameter("xmlFile", xmlData)};
                DataSet data = new DataSet();
                _db.Fill(data, "SP_FRA_ClaimRequestDetailsSave", prms);

                var workFlowModel = Util.GetListFromTable<WorkFlowVM>(data, 0).FirstOrDefault();

                if (!workFlowModel.IsError)
                {
                    if (model.ClaimRequestDocument != null)
                    {
                        SaveClaimReqDocs(workFlowModel.ClaimRequestDetailsID, workFlowModel.WorkFlowDetailsID, model.ClaimRequestDocument.Where(x => x.IsNew).ToList());
                    }
                    try
                    {
                        SendSMSEmailForSuccessTransaction(workFlowModel.ClaimRequestDetailsID, workFlowModel.ReturnMsg);
                    }
                    catch (Exception ex) { }
                }
                return workFlowModel;
            }
            catch (Exception ex) { throw ex; }
        }

        public DataSet UpdateClaimRequest(string parentID = null, string actionCode = null)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("UserID", userID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_FRA_UpdateClaimRequestDetails", prms);
            return data;
        }

        public void UpdateClaimRequestTransaction(long? reqID, Models.CommanModels.PaymentResponse model = null)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("ClaimRequestDetailsID", reqID),
                            new SqlParameter("UserID", userID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_FRA_UpdateClaimRequestDetails", prms);
        }

        private string GetRequestInXML(ClaimRequestVM model)
        {
            var data = model.ClaimRequestDetails;
            StringBuilder sb = new StringBuilder();
            sb.Append("<claimRequest>");
            sb.Append(string.Format(@"
                            <ClaimRequestDetailsID>{0}</ClaimRequestDetailsID>
                            <ClaimTypeID>{1}</ClaimTypeID>
                            <ClaimRequestPurposeID>{2}</ClaimRequestPurposeID>
                            <DistrictID>{3}</DistrictID>
                            <TehsilID>{4}</TehsilID>
                            <VillageCode>{5}</VillageCode>
                            <GPID>{6}</GPID>
                            <BlockID>{7}</BlockID>
                            <CompartmentNumber>{8}</CompartmentNumber>
                            <KhasraNumber>{9}</KhasraNumber>
                            <IsPattaGenerated>{10}</IsPattaGenerated>
                            <IsHalkaPatwariGenerated>{11}</IsHalkaPatwariGenerated>
                            <IsForesterGenerated>{12}</IsForesterGenerated>
                            <SSOID>{13}</SSOID>
                            <RequestDate>{14}</RequestDate>
                            <ApplicationType>{15}</ApplicationType>
                            <RejectedRefNumber>{16}</RejectedRefNumber>
                            <RejectedRequestDate>{17}</RejectedRequestDate>
                            <RejectedDate>{18}</RejectedDate> 
                            <RejectedReason>{19}</RejectedReason>
                            <AppealRequestID>{20}</AppealRequestID>", data.ClaimRequestDetailsID, data.ClaimTypeID, data.ClaimRequestPurposeID, data.DistrictID,
                            data.TehsilID, data.VillageCode, data.GPID, data.BlockID, data.CompartmentNumber, data.KhasraNumber, data.IsPattaGenerated, data.IsHalkaPatwariGenerated, data.IsForesterGenerated, data.SSOID, Util.GetDateWithFormat(data.EnteredOn, "MM/dd/yyyy"), data.ApplicationType,
                            data.RejectedRefNumber, Util.GetDateWithFormat(data.RejectedRequestDate, "MM/dd/yyyy"), Util.GetDateWithFormat(data.RejectedDate, "MM/dd/yyyy"), data.RejectedReason, data.AppealRequestID));
            if (data.ClaimTypeID == Convert.ToInt32(FMDSS.FRAClaimType.Community))
            {
                sb.Append(string.Format(@"
              <Comminity_FDSTCommunity>{0}</Comminity_FDSTCommunity>
              <Comminity_OTFDCommunity>{1}</Comminity_OTFDCommunity>
              <Community_CRSANistar>{2}</Community_CRSANistar>
              <Community_ROMFProduce>{3}</Community_ROMFProduce>
              <Community_UsesOrEntitlement>{4}</Community_UsesOrEntitlement>
              <Community_Grazing>{5}</Community_Grazing>
              <Community_TRAFNAPastroralist>{6}</Community_TRAFNAPastroralist>
              <Community_CTOHAHabitation>{7}</Community_CTOHAHabitation>
              <Community_RTABiodiversity>{8}</Community_RTABiodiversity>
              <Community_OTRight>{9}</Community_OTRight>", data.Comminity_FDSTCommunity, data.Comminity_OTFDCommunity, data.Community_CRSANistar, data.Community_ROMFProduce, data.Community_UsesOrEntitlement,
                  data.Community_Grazing, data.Community_TRAFNAPastroralist, data.Community_CTOHAHabitation, data.Community_RTABiodiversity, data.Community_OTRight));
            }
            else
            {
                sb.Append(string.Format(@"<Individual_IsClaimBefore>{0}</Individual_IsClaimBefore>
              <Individual_STribe>{1}</Individual_STribe>
              <Individual_OTFDweller>{2}</Individual_OTFDweller>
              <Individual_FHabitation>{3}</Individual_FHabitation>
              <Individual_FSCultivation>{4}</Individual_FSCultivation>
              <Individual_DisputedLands>{5}</Individual_DisputedLands>
              <Individual_PLGrants>{6}</Individual_PLGrants>
              <Individual_LFISROAlternativeLand>{7}</Individual_LFISROAlternativeLand>
              <Individual_LFWDisplacedWLCompensation>{8}</Individual_LFWDisplacedWLCompensation>
              <Individual_EOLIFVillages>{9}</Individual_EOLIFVillages>
              <Individual_AOTRight>{10}</Individual_AOTRight>", data.Individual_IsClaimBefore, data.Individual_STribe, data.Individual_OTFDweller, data.Individual_FHabitation, data.Individual_FSCultivation,
                      data.Individual_DisputedLands, data.Individual_PLGrants, data.Individual_LFISROAlternativeLand, data.Individual_LFWDisplacedWLCompensation, data.Individual_EOLIFVillages, data.Individual_AOTRight));
            }
            sb.Append(string.Format("<RequesterComment>{0}</RequesterComment>", data.RequesterComment));
            if (model.ClaimantDetailsList != null)
            {
                sb.Append("<ClaimantDetails>");
                foreach (var item in model.ClaimantDetailsList)
                {
                    sb.Append(string.Format(@"  
                        <ClaimantDetail>
                        <ClaimantDetailsID>{0}</ClaimantDetailsID>
                        <BhamashahID>{1}</BhamashahID>
                        <ClaimantName>{2}</ClaimantName>
                        <FatherName>{3}</FatherName>
                        <SpouseName>{4}</SpouseName>
                        <Email>{5}</Email>
                        <Mobile>{6}</Mobile>
                        <Gender>{7}</Gender>
                        </ClaimantDetail>", item.ClaimantDetailsID, item.BhamashahID, item.ClaimantName, item.FatherName, item.SpouseName, item.Email, item.Mobile, item.Gender));
                }
                sb.Append("</ClaimantDetails>");
            }
            if (model.BorderingVillageDetails != null)
            {
                sb.Append("<BorderingVillageDetails>");
                foreach (var item in model.BorderingVillageDetails)
                {
                    sb.Append(string.Format(@"
                          <BorderingVillageDetail>
                          <BorderingVillageDetailsID>{0}</BorderingVillageDetailsID>
                          <VillageCode>{1}</VillageCode>
                        </BorderingVillageDetail>", item.BorderingVillageDetailsID, item.VillageCode));
                }
                sb.Append("</BorderingVillageDetails>");
            }
            if (model.MemberDetailsList != null)
            {
                sb.Append("<MemberDetails>");
                foreach (var item in model.MemberDetailsList)
                {
                    sb.Append(string.Format(@" 
                        <MemberDetail>
                          <MemberDetailsID>{0}</MemberDetailsID>
                          <BhamashahID>{1}</BhamashahID>
                          <MemberName>{2}</MemberName> 
                          <FatherName>{3}</FatherName>
                          <SpouseName>{4}</SpouseName>
                          <Age>{5}</Age>
                          <Email>{6}</Email>
                          <Mobile>{7}</Mobile>
                          <IsDependant>{8}</IsDependant>
                          <Gender>{9}</Gender>
                        </MemberDetail>", item.MemberDetailsID, item.BhamashahID, item.MemberName, item.FatherName, item.SpouseName, item.Age, item.Email, item.Mobile, item.IsDependant, item.Gender));
                }
                sb.Append("</MemberDetails>");
            }
            if (model.ClaimRequestDocument != null)
            {
                sb.Append("<ClaimRequestDocuments>");

                foreach (var item in model.ClaimRequestDocument.Where(x => x.IsNew).ToList())
                {
                    sb.Append(string.Format(@" 
                    <ClaimRequestDocument>
                      <DocumentName>{0}</DocumentName>
                      <DocumentPath>{1}</DocumentPath>
                      <DocumentTypeID>{2}</DocumentTypeID>
                    </ClaimRequestDocument>", item.DocumentName, Util.GetAppSettings("FRADocumentPath"), item.DocumentTypeID));
                }

                sb.Append("</ClaimRequestDocuments>");
            }
            sb.Append("</claimRequest>");
            return Convert.ToString(sb);
        }

        private string GetDocInXML()
        {
            var attachedDataDoc = new List<CommonDocument>();
            if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
            {
                attachedDataDoc = ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.IsNew).ToList();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<docData>");

            if (attachedDataDoc.Count > 0)
            {
                sb.Append("<documents>");
                foreach (var item in attachedDataDoc)
                {
                    sb.Append("<document>");
                    sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName> 
                            <DocumentPath>{5}</DocumentPath>
                            <IsESign>{6}</IsESign>", item.DocumentID, item.ObjectTypeID, item.ObjectID, item.DocumentTypeID, item.DocumentName, Util.GetAppSettings("FRADocumentPath"), item.IsESign));
                    sb.Append("</document>");
                }
                sb.Append("</documents>");
            }

            sb.Append("</docData>");
            return Convert.ToString(sb);
        }

        #endregion

        #region Gramsabha Operation
        public void AddSurveyDetails(tbl_FRA_SurveyDetails model, string reportType, string rootPath, string OTP, string TransationID, ref string returnMsg, ref Boolean isError)
        {
            E_SignIntegration.clsVerifyOTP request = new E_SignIntegration.clsVerifyOTP();
            request.otp = OTP;
            request.transactionid = TransationID;

            var otpResponse = FMDSS.App_Start.cls_ESignIntegrationByFRA.VerifyOTPData(request, Convert.ToString(model.ClaimRequestDetailsID));

            if (!string.IsNullOrEmpty(otpResponse.TransactionId) || Util.GetAppSettings("FRACanContinueWithWrongOTP").Equals("1"))
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.SurveyDetailsID.Equals(0))
                        {
                            model.AddedBy = userID;
                            model.AddedOn = Util.GetISTDateTime();
                            dbContext.Entry(model).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            model.ModifiedBy = userID;
                            model.ModifiedOn = Util.GetISTDateTime();
                            dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            foreach (var item in model.KhasraDetailsList)
                            {
                                tbl_FRA_KhasraDetails khasraDetails = dbContext.tbl_FRA_KhasraDetails.Where(d => d.KhasraDetailsID == item.KhasraDetailsID).FirstOrDefault();
                                dbContext.Entry(item).State = khasraDetails == null ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                            }
                        }

                        if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
                        {
                            var sAttachment = ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.DocumentTypeID == Convert.ToInt32(FMDSS.DocumentType.SurveyEvidence) && x.IsNew).ToList();


                            if (sAttachment != null)
                            {
                                SaveClaimReqDocs(model.ClaimRequestDetailsID, model.WorkFlowDetailsID, sAttachment);
                            }
                        }
                        //Need to uncomment all lines
                        dbContext.SaveChanges();
                        GenerateReport(model.ClaimRequestDetailsID, model.WorkFlowDetailsID, reportType, rootPath, otpResponse);
                        dbContext.SaveChanges();
                        transaction.Commit();
                        returnMsg = Constant.SaveMsg;
                        isError = false;
                    }
                    catch (Exception ex)
                    {
                        isError = true;
                        returnMsg = ex.Message;
                    }
                }
            }
            else
            {
                returnMsg = otpResponse.ErrorMessage;
                isError = true;
            }
        }

        public ResponseMsg UpdateSurveyDetails(tbl_FRA_SurveyDetails model)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = false;
            try
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    model.ModifiedBy = userID;
                    model.ModifiedOn = Util.GetISTDateTime();
                    foreach (var item in model.KhasraDetailsList)
                    {
                        dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    //Need to uncomment all lines
                    dbContext.SaveChanges();
                    transaction.Commit();
                    msg.ReturnMsg = Constant.UpdateMsg;
                }
            }
            catch (Exception ex)
            {
                msg.IsError = true;
            }
            return msg;
        }

        public void GenerateReport(long? reqID, long? wfdID, string reportType, string rootPath, clsVerifyOTPResponce otpResponse)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", userID),
                            new SqlParameter("ClaimRequestDetailsID", reqID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_FRA_GetDataForApprovalReport", prms);

            if (Util.isValidDataSet(data, 1))
            {
                string docName = string.Empty;
                bool isEsignGenerated = false;

                HalkaPatwariVM hpVM = new HalkaPatwariVM();
                hpVM.HalkaPatwariDetailsVM = Util.GetListFromTable<HalkaPatwariDetailsVM>(data, 0).FirstOrDefault();
                hpVM.KhasraDetailsVM = Util.GetListFromTable<KhasraDetailsVM>(data, 1);

                var _objCR = dbContext.tbl_FRA_ClaimRequestDetails.AsQueryable().Where(x => x.ClaimRequestDetailsID == reqID).FirstOrDefault();
                var _objCRDoc = new CommonDocument();

                if (reportType.Equals(Convert.ToString(FMDSS.ReportType.HalkaPatwari)))
                {
                    GenerateHalkaPatwariReport(rootPath, hpVM, otpResponse.TransactionId, ref docName, ref isEsignGenerated);
                    _objCRDoc.DocumentTypeID = Convert.ToInt32(FMDSS.DocumentType.HalkaPatwariReport);
                    _objCR.IsHalkaPatwariGenerated = true;
                }
                else if (reportType.Equals(Convert.ToString(FMDSS.ReportType.Forester)))
                {
                    GenerateForesterReport(rootPath, hpVM, otpResponse.TransactionId, ref docName, ref isEsignGenerated);
                    _objCRDoc.DocumentTypeID = Convert.ToInt32(FMDSS.DocumentType.ForesterReport);
                    _objCR.IsForesterGenerated = true;
                }
                else if (reportType.Equals(Convert.ToString(FMDSS.ReportType.Patta)))
                {
                    GeneratePattaReport(rootPath, hpVM, otpResponse.TransactionId, ref docName, ref isEsignGenerated);
                    _objCRDoc.DocumentTypeID = Convert.ToInt32(FMDSS.DocumentType.PattaReport);
                    _objCR.IsPattaGenerated = true;
                }

                if (isEsignGenerated)
                {
                    _objCRDoc.DocumentPath = Util.GetAppSettings("FRADocumentESignPDF") + docName;
                }
                else
                {
                    _objCRDoc.DocumentPath = Util.GetAppSettings("FRADocumentAllPDF") + docName;
                }

                var prms2 = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("ObjectID", wfdID),
                            new SqlParameter("ObjectTypeID", 5),
                            new SqlParameter("DocumentTypeID", _objCRDoc.DocumentTypeID),
                            new SqlParameter("DocumentName", docName),
                            new SqlParameter("DocumentPath", _objCRDoc.DocumentPath),
                            new SqlParameter("IsESign", isEsignGenerated),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]) };
                DataSet data2 = new DataSet();
                _db.Fill(data2, "SP_Common_Document_Save", prms2);
                dbContext.Entry(_objCR).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public tbl_FRA_SurveyDetails GetSurveyDetails(long claimRequestDetailsID)
        {
            var wfID = dbContext.tbl_FRA_WorkFlowDetails.AsQueryable().Where(x => x.ClaimRequestDetailsID == claimRequestDetailsID).OrderByDescending(x => x.WorkFlowDetailsID).FirstOrDefault().WorkFlowDetailsID;
            var obj = dbContext.tbl_FRA_SurveyDetails.AsQueryable().Where(x => x.ClaimRequestDetailsID == claimRequestDetailsID).FirstOrDefault();
            if (obj == null)
            {
                obj = new tbl_FRA_SurveyDetails();
            }
            obj.WorkFlowDetailsID = wfID;
            return obj;
        }
        #endregion

        #region Common Operation for Approval Workflow
        public List<ClaimRequestDetailsVM> GetClaimRequestForApproval()
        {
            var prms = new[]{ new SqlParameter("ActionCode", 1),
                              new SqlParameter("UserID", userID)};
            var cmdText = "EXEC [SP_FRA_ClaimRequestDetails] @ActionCode = @ActionCode, @UserID = @UserID";

            return dbContext.Database.SqlQuery<ClaimRequestDetailsVM>(cmdText, prms).ToList();
        }

        public List<ClaimRequestDetailsVM> GetClaimRequestDetails(string parentID)
        {
            var prms = new[]{ new SqlParameter("ActionCode", 3),
                    new SqlParameter("ParentID", parentID),
                    new SqlParameter("UserID", userID)};
            var cmdText = "EXEC [SP_FRA_GetClaimRequestDetails] @ActionCode = @ActionCode, @ParentID = @ParentID, @UserID = @UserID";

            return dbContext.Database.SqlQuery<ClaimRequestDetailsVM>(cmdText, prms).ToList();
        }
        public ClaimRequestDetailsVM GetWorkFlowDetails(long claimRequestDetailsID)
        {
            var prms = new[]{ new SqlParameter("ActionCode", 3),
                              new SqlParameter("ClaimRequestDetailsID", claimRequestDetailsID),
                              new SqlParameter("UserID", userID)};
            var cmdText = "EXEC [SP_FRA_ClaimRequestDetails] @ActionCode = @ActionCode, @ClaimRequestDetailsID=@ClaimRequestDetailsID, @UserID = @UserID";

            return dbContext.Database.SqlQuery<ClaimRequestDetailsVM>(cmdText, prms).FirstOrDefault();
        }
        public List<WorkFlowDetailsVM> GetWorkFlowDetailsList(long claimRequestDetailsID)
        {
            var prms = new[]{ new SqlParameter("ActionCode", 2),
                              new SqlParameter("ClaimRequestDetailsID", claimRequestDetailsID),
                              new SqlParameter("UserID", userID)};
            var cmdText = "EXEC [SP_FRA_ClaimRequestDetails] @ActionCode = @ActionCode, @ClaimRequestDetailsID = @ClaimRequestDetailsID";

            return dbContext.Database.SqlQuery<WorkFlowDetailsVM>(cmdText, prms).ToList();
        }
        public ResponseMsg UpdateAppealDetails(ApproverRemarksCommonVM model)
        {
            ResponseMsg msg = null;
            try
            {
                var prms = new[]{
                        new SqlParameter("ActionCode", "1"),
                        new SqlParameter("ParentID", model.ParentID),
                        new SqlParameter("StatusID", model.StatusID),
                        new SqlParameter("ApproverComment", model.ApproverComment), 
                        new SqlParameter("UserID", userID)};

                DataSet ds = new DataSet();
                _db.Fill(ds, "SP_FRA_UpdateAppealRequest", prms);

                if (Globals.Util.isValidDataSet(ds, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<ResponseMsg>(ds, 0).FirstOrDefault();
                }
            }
            catch (Exception ex) { }
            return msg;
        }
        public void UpdateWorkFlowDetails(WorkFlowApproverVM model, string rootPath, ref string returnMsg, ref Boolean isError)
        {
            //using (var transaction = dbContext.Database.BeginTransaction())
            //{
            try
            {
                var remarkModel = model.ApproverRemarksDetails;
                var prms = new[]{
                        new SqlParameter("ActionCode", "Approver"),
                        new SqlParameter("ClaimRequestDetailsID", remarkModel.ClaimRequestDetailsID),
                        new SqlParameter("ApprovalStatus", remarkModel.StatusID),
                        new SqlParameter("ApproverComment", remarkModel.ApproverComment),
                        new SqlParameter("ApproverComment1", string.IsNullOrWhiteSpace(remarkModel.ApproverComment1)?string.Empty:remarkModel.ApproverComment1),
                        new SqlParameter("xmlFile", GetDocInXML()),
                        new SqlParameter("UserID", userID)};

                DataSet ds = new DataSet();
                _db.Fill(ds, "SP_FRA_UpdateWorkFlowDetails", prms);

                var workFlowModel = Util.GetListFromTable<WorkFlowVM>(ds, 0).FirstOrDefault();

                if (!workFlowModel.IsError)
                {
                    var requireDbChange = false;

                    Parallel.Invoke(() =>
                    {
                        if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
                        {
                            var approverAttachment = ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.IsNew).ToList();

                            if (approverAttachment != null)
                            {
                                SaveClaimReqDocs(remarkModel.ClaimRequestDetailsID, workFlowModel.WorkFlowDetailsID, approverAttachment);
                                requireDbChange = true;
                            }
                        }
                    },
                    () =>
                    {
                        if (remarkModel.StatusID == Convert.ToInt32(FMDSS.ActionTypeForFRA.Approve))
                        {
                            GenerateReport(remarkModel.ClaimRequestDetailsID, workFlowModel.WorkFlowDetailsID, Convert.ToString(FMDSS.ReportType.Patta), rootPath, null);
                            requireDbChange = true;
                        }
                    });

                    if (requireDbChange)
                    {
                        dbContext.SaveChanges();
                    }
                    try
                    {
                        SendSMSEmailForSuccessTransaction(remarkModel.ClaimRequestDetailsID, workFlowModel.ReturnMsg);
                    }
                    catch (Exception ex) { }
                    //transaction.Commit();

                }
                //else
                //{
                //    transaction.Rollback();
                //}
                returnMsg = workFlowModel.ReturnMsg;
                isError = workFlowModel.IsError;
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                returnMsg = ex.Message;
                isError = true;
            }
            //}
        }
        public void UpdateWorkFlowDetailsForESign(WorkFlowApproverVM model, string rootPath, string command, string OTP, string TransationID, ref string returnMsg, ref Boolean isError)
        {
            //using (var transaction = dbContext.Database.BeginTransaction())
            //{
            try
            {
                var remarkModel = model.ApproverRemarksDetails;
                E_SignIntegration.clsVerifyOTP request = new E_SignIntegration.clsVerifyOTP();
                request.otp = OTP;
                request.transactionid = TransationID;

                var otpResponse = FMDSS.App_Start.cls_ESignIntegrationByFRA.VerifyOTPData(request, Convert.ToString(remarkModel.ClaimRequestDetailsID));
                if (!string.IsNullOrEmpty(otpResponse.TransactionId) || Util.GetAppSettings("FRACanContinueWithWrongOTP").Equals("1"))
                {
                    var prms = new[]{
                        new SqlParameter("ActionCode", "Approver"),
                        new SqlParameter("ClaimRequestDetailsID", remarkModel.ClaimRequestDetailsID),
                        new SqlParameter("ApprovalStatus", remarkModel.StatusID),
                        new SqlParameter("ApproverComment", remarkModel.ApproverComment),
                        new SqlParameter("ApproverComment1", string.IsNullOrWhiteSpace(remarkModel.ApproverComment1)?string.Empty:remarkModel.ApproverComment1),
                        new SqlParameter("CaseNumber", string.IsNullOrWhiteSpace(remarkModel.CaseNumber)?string.Empty:remarkModel.CaseNumber),
                        new SqlParameter("UserID", userID)};

                    DataSet ds = new DataSet();
                    _db.Fill(ds, "SP_FRA_UpdateWorkFlowDetails", prms);

                    var workFlowModel = Util.GetListFromTable<WorkFlowVM>(ds, 0).FirstOrDefault();

                    if (!workFlowModel.IsError)
                    {
                        if (remarkModel.StatusID == Convert.ToInt32(FMDSS.ActionTypeForFRA.Approve))
                        {
                            GenerateReport(remarkModel.ClaimRequestDetailsID, workFlowModel.WorkFlowDetailsID, Convert.ToString(FMDSS.ReportType.Patta), rootPath, otpResponse);
                            dbContext.SaveChanges();

                        }
                        try
                        {
                            SendSMSEmailForSuccessTransaction(remarkModel.ClaimRequestDetailsID, workFlowModel.ReturnMsg);
                        }
                        catch (Exception ex) { }
                        ////transaction.Commit(); 
                    }
                    else
                    {
                        //transaction.Rollback();
                    }
                    returnMsg = workFlowModel.ReturnMsg;
                    isError = workFlowModel.IsError;
                }
                else
                {
                    returnMsg = otpResponse.ErrorMessage;
                    isError = true;
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                returnMsg = ex.Message;
                isError = true;
            }
            //}
        }
        public void UpdateWorkFlowDetailsMultipleForESign(WorkFlowApproverMultipleVM model, string rootPath, string command, string OTP, string TransationID, ref string returnMsg, ref Boolean isError)
        {
            //using (var transaction = dbContext.Database.BeginTransaction())
            //{
            try
            {
                var remarkModel = model.ApproverRemarksDetails;
                clsVerifyOTP request = new clsVerifyOTP();
                request.otp = OTP;
                request.transactionid = TransationID;

                var otpResponse = App_Start.cls_ESignIntegrationByFRA.VerifyOTPData(request, Convert.ToString(remarkModel.ClaimRequestDetailsID));
                if (!string.IsNullOrEmpty(otpResponse.TransactionId) || Util.GetAppSettings("FRACanContinueWithWrongOTP").Equals("1"))
                {
                    bool isApproved = false; bool isAnyError = false;

                    foreach (var item in remarkModel.ClaimRequestDetailsID.Split(','))
                    {
                        var prms = new[]{
                        new SqlParameter("ActionCode", "Approver"),
                        new SqlParameter("ClaimRequestDetailsID", item),
                        new SqlParameter("ApprovalStatus", remarkModel.StatusID),
                        new SqlParameter("ApproverComment", remarkModel.ApproverComment),
                        new SqlParameter("UserID", userID)};

                        DataSet ds = new DataSet();
                        _db.Fill(ds, "SP_FRA_UpdateWorkFlowDetails", prms);

                        var workFlowModel = Util.GetListFromTable<WorkFlowVM>(ds, 0).FirstOrDefault();

                        if (!workFlowModel.IsError)
                        {
                            if (remarkModel.StatusID == Convert.ToInt32(FMDSS.ActionTypeForFRA.Approve).ToString())
                            {
                                GenerateReport(Convert.ToInt64(item), workFlowModel.WorkFlowDetailsID, Convert.ToString(FMDSS.ReportType.Patta), rootPath, otpResponse);
                                dbContext.SaveChanges();

                            }
                            //try
                            //{
                            //    SendSMSEmailForSuccessTransaction(remarkModel.ClaimRequestDetailsID, workFlowModel.ReturnMsg);
                            //}
                            //catch (Exception ex) { }
                            ////transaction.Commit();

                        }

                        if (!isApproved)
                        {
                            isApproved = !workFlowModel.IsError;
                        }

                        if (!isAnyError)
                        {
                            isAnyError = workFlowModel.IsError;
                        }
                    }

                    if (isApproved && isAnyError)
                    {
                        returnMsg = "Selected Request Approved partially."; isError = true;
                    }
                    else
                    {
                        returnMsg = "Selected Request Approved successfully."; isError = false;
                    }
                }
                else
                {
                    returnMsg = otpResponse.ErrorMessage;
                    isError = true;
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                returnMsg = ex.Message;
                isError = true;
            }
            //}
        }

        //UpdateDocWitheSign
        public ResponseMsg UpdateDocWitheSign(WorkFlowApproverVM model, string rootPath, string command, string OTP, string TransationID)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;
            try
            {
                var remarkModel = model.ApproverRemarksDetails;
                E_SignIntegration.clsVerifyOTP request = new E_SignIntegration.clsVerifyOTP();
                request.otp = OTP;
                request.transactionid = TransationID;

                var otpResponse = App_Start.cls_ESignIntegrationByFRA.VerifyOTPData(request, Convert.ToString(remarkModel.ClaimRequestDetailsID));
                if (!string.IsNullOrEmpty(otpResponse.TransactionId) || Util.GetAppSettings("FRACanContinueWithWrongOTP").Equals("1"))
                {
                    var prms = new[]{
                        new SqlParameter("ActionCode", 1),
                        new SqlParameter("ObjectID", remarkModel.ClaimRequestDetailsID),
                        new SqlParameter("UserID", userID)};

                    DataSet dSDoc = new DataSet();
                    _db.Fill(dSDoc, "SP_FRA_GetClaimRequestDocument", prms);

                    var esignPath = string.Empty;

                    if (Util.isValidDataSet(dSDoc, true))
                    {
                        var docPath = rootPath + Convert.ToString(dSDoc.Tables[0].Rows[0]["DocumentPath"]);
                        var isEsignSuccess = false;

                        if (command.Equals("DLC"))
                        {
                            isEsignSuccess = GenerateESignPDF(docPath, docPath, otpResponse, Convert.ToString(remarkModel.ClaimRequestDetailsID), "", "", "400", "20");
                        }
                        else
                        {
                            isEsignSuccess = GenerateESignPDF(docPath, docPath, otpResponse, Convert.ToString(remarkModel.ClaimRequestDetailsID), "", "", "20", "20");
                        }

                        if (isEsignSuccess)
                        {
                            #region Save Workflow data
                            var prms2 = new[]{
                            new SqlParameter("ActionCode", "Approver"),
                            new SqlParameter("ClaimRequestDetailsID", remarkModel.ClaimRequestDetailsID),
                            new SqlParameter("ApprovalStatus", remarkModel.StatusID),
                            new SqlParameter("UserID", userID)};
                            DataSet ds = new DataSet();
                            _db.Fill(ds, "SP_FRA_UpdateWorkFlowDetails", prms2);

                            var workFlowModel = Util.GetListFromTable<WorkFlowVM>(ds, 0).FirstOrDefault();
                            #endregion

                            msg.ReturnMsg = "E-Sign generated successfully."; msg.IsError = false;
                        }
                        else
                        {
                            msg.ReturnMsg = "Oops some error on digital sign, please co-ordinate with support team!"; msg.IsError = true;
                        }
                    }
                }
                else
                {
                    msg.ReturnMsg = otpResponse.ErrorMessage;
                    msg.IsError = true;
                }
            }
            catch (Exception ex)
            {
                msg.ReturnMsg = ex.Message;
                msg.IsError = true;
            }
            return msg;
        }

        public ResponseMsg UpdateDocMultipleWitheSign(WorkFlowApproverMultipleVM model, string rootPath, string command, string OTP, string TransationID)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;
            try
            {
                var remarkModel = model.ApproverRemarksDetails;
                E_SignIntegration.clsVerifyOTP request = new E_SignIntegration.clsVerifyOTP();
                request.otp = OTP;
                request.transactionid = TransationID;

                var otpResponse = App_Start.cls_ESignIntegrationByFRA.VerifyOTPData(request, Convert.ToString(remarkModel.ClaimRequestDetailsID));
                if (!string.IsNullOrEmpty(otpResponse.TransactionId) || Util.GetAppSettings("FRACanContinueWithWrongOTP").Equals("1"))
                {
                    bool isApproved = false; bool isAnyError = false;

                    foreach (var reqID in remarkModel.ClaimRequestDetailsID.Split(','))
                    {
                        var prms = new[]{
                        new SqlParameter("ActionCode", 1),
                        new SqlParameter("ObjectID", reqID),
                        new SqlParameter("UserID", userID)};

                        DataSet dSDoc = new DataSet();
                        _db.Fill(dSDoc, "SP_FRA_GetClaimRequestDocument", prms);

                        var esignPath = string.Empty;

                        if (Util.isValidDataSet(dSDoc, true))
                        {
                            var docPath = rootPath + Convert.ToString(dSDoc.Tables[0].Rows[0]["DocumentPath"]);
                            var isEsignSuccess = false;

                            if (command.Equals("DLC"))
                            {
                                isEsignSuccess = GenerateESignPDF(docPath, docPath, otpResponse, reqID, "", "", "400", "20");
                            }
                            else
                            {
                                isEsignSuccess = GenerateESignPDF(docPath, docPath, otpResponse, reqID, "", "", "20", "20");
                            }

                            if (isEsignSuccess)
                            {
                                #region Save Workflow data
                                var prms2 = new[]{
                            new SqlParameter("ActionCode", "Approver"),
                            new SqlParameter("ClaimRequestDetailsID", reqID),
                            new SqlParameter("ApprovalStatus", remarkModel.StatusID),
                            new SqlParameter("UserID", userID)};
                                DataSet ds = new DataSet();
                                _db.Fill(ds, "SP_FRA_UpdateWorkFlowDetails", prms2);

                                var workFlowModel = Util.GetListFromTable<WorkFlowVM>(ds, 0).FirstOrDefault();
                                #endregion

                                msg.ReturnMsg = "E-Sign generated successfully."; msg.IsError = false;
                            }
                            else
                            {
                                msg.ReturnMsg = "Oops some error on digital sign, please co-ordinate with support team!"; msg.IsError = true;
                            }

                            if (!isApproved)
                            {
                                isApproved = isEsignSuccess;
                            }

                            if (!isAnyError)
                            {
                                isAnyError = !isEsignSuccess;
                            }
                        }
                    }

                    if (isApproved && isAnyError)
                    {
                        msg.ReturnMsg = "E-Sign generated partially for selected request."; msg.IsError = true;
                    }
                    else
                    {
                        msg.ReturnMsg = "E-Sign generated successfully for selected request."; msg.IsError = false;
                    }
                }
                else
                {
                    msg.ReturnMsg = otpResponse.ErrorMessage;
                    msg.IsError = true;
                }
            }
            catch (Exception ex)
            {
                msg.ReturnMsg = ex.Message;
                msg.IsError = true;
            }
            return msg;
        }
        public void UpdateWorkFlowDetailsForCitizen(WorkFlowApproverVM model, ref string returnMsg, ref Boolean isError)
        {
            var remarkModel = model.ApproverRemarksDetails;
            var prms = new[]{
                        new SqlParameter("ActionCode", "Citizen"),
                        new SqlParameter("ClaimRequestDetailsID", remarkModel.ClaimRequestDetailsID),
                        new SqlParameter("ApprovalStatus", remarkModel.StatusID),
                        new SqlParameter("ApproverComment", remarkModel.ApproverComment),
                        new SqlParameter("UserID", userID)};

            var cmdText = @"EXEC [SP_FRA_UpdateWorkFlowDetails] 
                            @ActionCode=@ActionCode, 
                            @ClaimRequestDetailsID = @ClaimRequestDetailsID,  
                            @ApprovalStatus=@ApprovalStatus, 
                            @ApproverComment=@ApproverComment,
                            @UserID = @UserID";

            var workFlowModel = dbContext.Database.SqlQuery<WorkFlowVM>(cmdText, prms).FirstOrDefault();
            returnMsg = workFlowModel.ReturnMsg;
            isError = workFlowModel.IsError;
        }

        public string DownloadReceipt(WorkFlowApproverVM model)
        {
            if (model != null)
            {
                var claimRequestDetailsID = model.WorkFlowDetailsList.LastOrDefault().ClaimRequestDetailsID;
                var citizenRequest = model.CitizenClaimRequestDetails;

                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    string filepath = Util.GetAppSettings("FRADocumentPath");
                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/" + filepath + "ClaimRequestReceipt.pdf");

                    #region Write data to PDF
                    PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
                    var FontColour = new BaseColor(0, 0, 0);
                    Paragraph tableheading = null;

                    var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                    var subheadfont = FontFactory.GetFont("Arial", 12, FontColour);
                    var tableHeaderFont = FontFactory.GetFont("Arial", 12, FontColour);
                    doc.Open();
                    doc.NewPage();


                    #region TEST

                    PdfPTable Logo = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    string imageURLs = "~/images/logo.png";

                    iTextSharp.text.Image ForestLogo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(imageURLs));
                    ForestLogo.ScaleToFit(150f, 150f);
                    ForestLogo.SpacingBefore = 10f;
                    ForestLogo.SpacingAfter = 10f;
                    ForestLogo.Alignment = Element.ALIGN_CENTER;

                    PdfPCell cellForestLogo;
                    cellForestLogo = new PdfPCell(ForestLogo);
                    cellForestLogo.BorderWidth = 0;
                    cellForestLogo.Padding = 0;
                    cellForestLogo.PaddingTop = -20;
                    cellForestLogo.HorizontalAlignment = Element.ALIGN_LEFT;
                    Logo.AddCell(cellForestLogo);

                    tableheading = new Paragraph("Department of Forest,", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);
                    doc.Add(tableheading);

                    tableheading = new Paragraph("Goverment of", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);
                    doc.Add(tableheading);

                    tableheading = new Paragraph("Rajasthan.", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);
                    //doc.Add(tableheading);

                    PdfPCell cellHeadings;
                    cellHeadings = new PdfPCell(tableheading);
                    cellHeadings.BorderWidth = 0;
                    //cellHeadings.Padding = 20;
                    cellHeadings.Colspan = 2;
                    cellHeadings.PaddingLeft = 65;
                    //cellHeadings.HorizontalAlignment = Element.ALIGN_CENTER;
                    Logo.AddCell(cellHeadings);
                    doc.Add(Logo);
                    #endregion

                    PdfPTable table;

                    table = new PdfPTable(2);
                    doc.Add(new Paragraph(Environment.NewLine));


                    PdfPTable tabular = new PdfPTable(9) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cells = new PdfPCell(new Phrase("REQUEST CONFIRMATION SLIP", subheadfont));// { Border = 4};
                    tabular.TotalWidth = 320;
                    tabular.SetTotalWidth(new float[] { 22f, 30f, 25f, 27f, 20f, 25f, 30f, 30f, 30f });
                    cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cells.Colspan = 9;
                    cells.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Request Type : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.ClaimTypeName, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Request No : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.ClaimRequestIDWithPrefix, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Applicant Name : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.RaisedBy, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Block : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.BlockName, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Village : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.VillageName, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Gram Panchayat : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.GPName, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Current Status : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.CurrentStatus, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Applicant Comment : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(citizenRequest.RequesterComment, subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Raised On : ", subheadfont));
                    cells.Colspan = 5;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(citizenRequest.RaisedOn).ToString("dd-MMM-yyyy"), subheadfont));
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabular.AddCell(cells);

                    doc.Add(tabular);
                    doc.Add(table);
                    #endregion

                    return filepath;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    doc.Close();
                }

            }
            return "";
        }

        #endregion

        #region PDF Operation
        public void GenerateHalkaPatwariReport(string rootPath, HalkaPatwariVM hpVM, string transactionID, ref string docName, ref bool isEsignGenerated)
        {
            #region Doc
            string filepath = string.Empty;
            string EsignPath = string.Empty;
            docName = string.Format("5_{0}_{1}_HalkaPatwariReport.pdf", hpVM.HalkaPatwariDetailsVM.ClaimRequestDetailsID, hpVM.HalkaPatwariDetailsVM.WorkFlowDetailsID);
            filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
            EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 14);

            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            #endregion

            #region Doc Body
            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));

            #region First Page

            #region top

            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 120;
            Details.SetTotalWidth(new float[] { 20f, 0f, 0f });


            cells = new PdfPCell(new Phrase("gYdk iVokjh dh fjiksVZ	", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("izi= & 4", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("fu;e 12 ¼4½ ns[ksa", hindi)) { Border = 0 };//niyam 12 dekhe
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };//niyam 12 dekhe
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            Phrase phraseL1 = new Phrase();
            phraseL1.Add(new Chunk("xzke", hindi));
            phraseL1.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.VillageName + " ", fontTitle));
            phraseL1.Add(new Chunk("xzke iapk;r", hindi));
            phraseL1.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.GPName + " ", fontTitle));
            phraseL1.Add(new Chunk("rglhy", hindi));
            phraseL1.Add(new Chunk(" N/A ", fontTitle));
            phraseL1.Add(new Chunk("ftyk", hindi));
            phraseL1.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.DistrictName + " ", fontTitle));
            phraseL1.Add(new Chunk("esa ou vf/kdkj lfefr }kjk fnukad", hindi));
            phraseL1.Add(new Chunk(" " + Util.GetISTDateTime().ToString("dd MMM yyyy") + " ", fontTitle));
            phraseL1.Add(new Chunk("dks Jh", hindi));
            phraseL1.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.ClaimantName + " ", fontTitle));
            phraseL1.Add(new Chunk("firk@ifr", hindi));
            phraseL1.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.FatherName + " ", fontTitle));
            phraseL1.Add(new Chunk("ds nkos ls lacaf/kr LFky ¼ou {ks=½ dk fujh{k.k jktLo fd;k x;k mldk jktLo vfHkys[kksa ds vuqlkj fooj.k fuEukuqlkj gS % &", hindi));
            Details.AddCell(phraseL1);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };//niyam 12 dekhe
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            doc.Add(Details);

            #endregion

            #region table

            PdfPTable UserDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            //Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cellud = new PdfPCell() { Border = 4 };
            UserDetails.TotalWidth = 120;
            UserDetails.SetTotalWidth(new float[] { 30f, 30f, 30f, 30f, 30f, 30f });

            cellud = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 }; //gram ka naam
            cellud.Colspan = 6;
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("xzke dk uke", hindi));//{ Border = 0 }; //gram ka naam
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("[kljk uEcj", hindi));//{ Border = 0 };//khasra number
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("[kljk ua- dk dqy {ks=Qy ", hindi));// { Border = 0 };//khasra number ka kul shetrafal
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("vf/kHkksx dh ou Hkwfe dk {ks=Qy", hindi));// { Border = 0 };// van bhoomi ka shetrafal
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("vf/kHkksx dk izdkj d`f\"k@vkokl", hindi));//{ Border = 0 };//krishi & aawas
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("fo'ks\"k fooj.k", hindi));//{ Border = 0 }; vishesh vivran
            cellud.HorizontalAlignment = Element.ALIGN_CENTER;
            UserDetails.AddCell(cellud);

            if (hpVM.KhasraDetailsVM != null)
            {
                foreach (var item in hpVM.KhasraDetailsVM)
                {
                    cellud = new PdfPCell(new Phrase(item.VillageName, fontTitle));// { Border = 0 }; 
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    UserDetails.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.KhasraNumber, fontTitle));// { Border = 0 }; 
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    UserDetails.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.TotalAreaAgainstKhasra, fontTitle));// { Border = 0 }; 
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    UserDetails.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.TotalAreaAgainstOccupiedForestLand, fontTitle));// { Border = 0 }; 
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    UserDetails.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.OccupancyType, fontTitle));// { Border = 0 }; 
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    UserDetails.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.SpecialRemarks, fontTitle));// { Border = 0 }; 
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    UserDetails.AddCell(cellud);
                }
            }

            cellud = new PdfPCell(new Phrase(";ksx", fontTitle));// { Border = 0 };
            cellud.HorizontalAlignment = Element.ALIGN_LEFT;
            UserDetails.AddCell(cellud);

            cellud = new PdfPCell(new Phrase("gLrk{kj iVokjh", hindi));// { Border = 0 };yog
            cellud.HorizontalAlignment = Element.ALIGN_RIGHT;
            UserDetails.AddCell(cellud);

            doc.Add(UserDetails);

            #endregion

            #region footer

            PdfPTable FooterDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellfd = new PdfPCell() { Border = 4 };
            FooterDetails.TotalWidth = 120;
            FooterDetails.SetTotalWidth(new float[] { 75f, 20f });

            cellfd = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cellfd.Colspan = 2;
            cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("gLrk{kj iVokjh", hindi)) { Border = 0 };//sign patwari
            cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };//sign patwari
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("xzke", hindi)) { Border = 0 }; //gram
            cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase(hpVM.HalkaPatwariDetailsVM.VillageName, fontTitle)) { Border = 0 };// gram
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("rglhy", hindi)) { Border = 0 };// gram
            cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.TehsilName + " ", fontTitle)) { Border = 0 };// Tehsil
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("ftyk", hindi)) { Border = 0 }; //gram
            cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase(hpVM.HalkaPatwariDetailsVM.DistrictName, fontTitle)) { Border = 0 }; //gram
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("uksV % vfHkHkksx dh ou Hkwfe dks uD’kk Vªsl ij yky L;kgh ls n’kkZrs gq, layXu djsaA", hindi)) { Border = 0 }; //gram
            cellfd.Colspan = 2;
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            doc.Add(FooterDetails);

            #endregion
            #endregion

            doc.NewPage();

            #region second page

            PdfPTable DetailSecondPage = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            DetailSecondPage.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cellsp = new PdfPCell() { Border = 4 };
            DetailSecondPage.TotalWidth = 35f;
            DetailSecondPage.SetTotalWidth(new float[] { 35f, 0f, 0f });

            cellsp = new PdfPCell(new Phrase("uD’kk Vªsl", hindi)) { Border = 0 };
            cellsp.Colspan = 3;
            cellsp.HorizontalAlignment = Element.ALIGN_CENTER;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase("izi=& 5", hindi)) { Border = 0 };
            cellsp.Colspan = 3;
            cellsp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase("¼uD’kk Vªsl bl ist ij fpidk,sa½", hindi)) { Border = 0 };//niyam 12 dekhe
            cellsp.Colspan = 3;
            cellsp.HorizontalAlignment = Element.ALIGN_CENTER;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase("fu;e 12 ¼4½ ns[ksa", hindi)) { Border = 0 };
            cellsp.Colspan = 3;
            cellsp.HorizontalAlignment = Element.ALIGN_CENTER;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cellsp.Colspan = 3;
            cellsp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailSecondPage.AddCell(cellsp);

            Phrase phraseL = new Phrase();
            phraseL.Add(new Chunk("vkaf'kd uD’kk Vªsl xzke", hindi));
            phraseL.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.VillageName + " ", fontTitle));
            phraseL.Add(new Chunk("ftlesa Jh ", hindi));
            phraseL.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.ClaimantName + " ", fontTitle));
            phraseL.Add(new Chunk("firk@ ifr Jh", hindi));
            phraseL.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.FatherName + " ", fontTitle));
            phraseL.Add(new Chunk("}kjk vf/kHkksx esa yh xbZ ou Hkwfe dks yky L;kgh ls n’kkZ;k x;k gSA", hindi));
            DetailSecondPage.AddCell(phraseL);

            cellsp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cellsp.Colspan = 3;
            cellsp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase("gLrk{kj iVokjh", hindi)) { Border = 0 };
            cellsp.Colspan = 2;
            cellsp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cellsp.HorizontalAlignment = Element.ALIGN_LEFT;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase("xzke", hindi)) { Border = 0 };
            cellsp.Colspan = 2;
            cellsp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailSecondPage.AddCell(cellsp);

            cellsp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cellsp.HorizontalAlignment = Element.ALIGN_LEFT;
            DetailSecondPage.AddCell(cellsp);

            doc.Add(DetailSecondPage);

            #endregion

            doc.Close();
            #endregion

            #region Generate Esign
            try
            {
                clsDocumentESign requestPdf = new clsDocumentESign();
                byte[] bytes = File.ReadAllBytes(filepath);
                requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                requestPdf.transactionid = transactionID;
                clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, Convert.ToString(hpVM.HalkaPatwariDetailsVM.ClaimRequestDetailsID), "MultipleLocation");

                if (response.Status != "0")
                {
                    using (FileStream stream = System.IO.File.Create(EsignPath))
                    {
                        byte[] byteArray = Convert.FromBase64String(response.Document);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    isEsignGenerated = true;
                }
            }
            catch (Exception ex) { }
            #endregion
        }

        public void GenerateForesterReport(string rootPath, HalkaPatwariVM hpVM, string transactionID, ref string docName, ref bool isEsignGenerated)
        {
            #region Doc
            string filepath = string.Empty;
            string EsignPath = string.Empty;
            docName = string.Format("5_{0}_{1}_ForesterReport.pdf", hpVM.HalkaPatwariDetailsVM.ClaimRequestDetailsID, hpVM.HalkaPatwariDetailsVM.WorkFlowDetailsID);
            filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
            EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);
            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 14);

            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            #endregion

            #region Doc Body
            doc.Open();
            doc.NewPage();

            #region third page

            PdfPTable DetailThirdPage = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            DetailThirdPage.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell celltp = new PdfPCell() { Border = 4 };
            DetailThirdPage.TotalWidth = 35f;
            DetailThirdPage.SetTotalWidth(new float[] { 35f, 0f, 0f });

            celltp = new PdfPCell(new Phrase("ou foHkkx izfrfuf/k dh fjiksVZ", hindi)) { Border = 0 };
            celltp.Colspan = 3;
            celltp.HorizontalAlignment = Element.ALIGN_CENTER;
            DetailThirdPage.AddCell(celltp);

            celltp = new PdfPCell(new Phrase("izi= & 6", hindi)) { Border = 0 };
            celltp.Colspan = 3;
            celltp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailThirdPage.AddCell(celltp);

            celltp = new PdfPCell(new Phrase("fu;e 12 ¼4½ ns[ksa ", hindi)) { Border = 0 };//niyam 12 dekhe
            celltp.Colspan = 3;
            celltp.HorizontalAlignment = Element.ALIGN_CENTER;
            DetailThirdPage.AddCell(celltp);

            celltp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            celltp.Colspan = 3;
            celltp.HorizontalAlignment = Element.ALIGN_CENTER;
            DetailThirdPage.AddCell(celltp);

            celltp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            celltp.Colspan = 3;
            celltp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailThirdPage.AddCell(celltp);

            Phrase phraseS = new Phrase();
            phraseS.Add(new Chunk("Jh", hindi));
            phraseS.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.ClaimantName + " ", fontTitle));
            phraseS.Add(new Chunk("firk@ifr ", hindi));
            phraseS.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.FatherName + " ", fontTitle));
            phraseS.Add(new Chunk("xzke ", hindi));
            phraseS.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.VillageName + " ", fontTitle));
            phraseS.Add(new Chunk("xzke iapk;r ", hindi));
            phraseS.Add(new Chunk("rglhy", hindi));
            phraseS.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.TehsilName + " ", fontTitle));
            phraseS.Add(new Chunk("ftyk  ", hindi));
            phraseS.Add(new Chunk(" " + hpVM.HalkaPatwariDetailsVM.DistrictName + " ", fontTitle));
            phraseS.Add(new Chunk("}kjk vf/kHkksx esa yh tk jgh ou Hkwfe dk fooj.k fuEu izdkj gS %&", hindi));
            DetailThirdPage.AddCell(phraseS);

            celltp = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            celltp.Colspan = 3;
            celltp.HorizontalAlignment = Element.ALIGN_RIGHT;
            DetailThirdPage.AddCell(celltp);

            doc.Add(DetailThirdPage);

            #region Content table

            PdfPTable ContentTable = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            ContentTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cellct = new PdfPCell() { Border = 4 };
            ContentTable.TotalWidth = 35f;
            ContentTable.SetTotalWidth(new float[] { 30f, 20f });


            cellct = new PdfPCell(new Phrase("            1-	ou [k.M dk uke	%", hindi)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.ForestSectionNames + " ", fontTitle)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase("            2-	dEikVZesaV la-		%", hindi)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.CompartmentNumbers + " ", fontTitle)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase("            3-	vf/kHkksx esa yh xbZ ou Hkwfe dk {ks=Qy % ", hindi)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.TotalAreaAgainstOccupiedForestLands + " ", fontTitle)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase("            4-	utjh uD’kk", hindi)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase("N/A", fontTitle)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase("            5-	uD’kks esa n’kkZ;s x;s fcUnwvksa ds funZs’kkad", hindi)) { Border = 0 };
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            cellct = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cellct.Colspan = 2;
            cellct.HorizontalAlignment = Element.ALIGN_LEFT;
            ContentTable.AddCell(cellct);

            doc.Add(ContentTable);

            #endregion
            #region table
            if (!string.IsNullOrEmpty(hpVM.HalkaPatwariDetailsVM.ActivityData))
            {


                PdfPTable InnerTable = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                InnerTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellit = new PdfPCell() { Border = 4 };
                InnerTable.TotalWidth = 35f;
                InnerTable.SetTotalWidth(new float[] { 30f, 30f, 30f });

                cellit = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
                cellit.Colspan = 3;
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                cellit = new PdfPCell(new Phrase("fcUnq", hindi));// { Border = 0 };
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                cellit = new PdfPCell(new Phrase("v{kka’k ¼mRrj½", hindi));//{ Border = 0 };
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                cellit = new PdfPCell(new Phrase("ns'kkUrj ¼iwoZ½", hindi));// { Border = 0 };
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                List<OtherLatLong> lstOtherLatLongs = GetLatLong(hpVM.HalkaPatwariDetailsVM.ActivityData);
                int count = 1;
                foreach (var item in lstOtherLatLongs)
                {
                    cellit = new PdfPCell(new Phrase(" " + count + " ", fontTitle));// { Border = 0 };
                    cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                    InnerTable.AddCell(cellit);

                    cellit = new PdfPCell(new Phrase(" " + item.Lat + " ", fontTitle));// { Border = 0 };
                    cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                    InnerTable.AddCell(cellit);

                    cellit = new PdfPCell(new Phrase(" " + item.Long + " ", fontTitle));// { Border = 0 };
                    cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                    InnerTable.AddCell(cellit);
                    count++;
                }
                doc.Add(InnerTable);
            }

            #endregion
            //#region table

            //PdfPTable InnerTable = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            //InnerTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //PdfPCell cellit = new PdfPCell() { Border = 4 };
            //InnerTable.TotalWidth = 35f;
            //InnerTable.SetTotalWidth(new float[] { 30f, 30f, 30f });

            //cellit = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            //cellit.Colspan = 3;
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            //cellit = new PdfPCell(new Phrase("fcUnq", hindi));// { Border = 0 };
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            //cellit = new PdfPCell(new Phrase("v{kka’k ¼mRrj½", hindi));//{ Border = 0 };
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            //cellit = new PdfPCell(new Phrase("ns'kkUrj ¼iwoZ½", hindi));// { Border = 0 };
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("v", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("m-", hindi));// { Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("iw-", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            //cellit = new PdfPCell(new Phrase(" 1 ", fontTitle));// { Border = 0 };
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            //cellit = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.Latitude + " ", fontTitle));// { Border = 0 };
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            //cellit = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.Longitude + " ", fontTitle));// { Border = 0 };
            //cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            //InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("c", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("m-", hindi));// { Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("iw-", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("l", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("m-", hindi));// { Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("iw-", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("n", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("m-", hindi));// { Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("iw-", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase(";", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("m-", hindi));// { Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            ////cellit = new PdfPCell(new Phrase("iw-", hindi));//{ Border = 0 };
            ////cellit.HorizontalAlignment = Element.ALIGN_CENTER;
            ////InnerTable.AddCell(cellit);

            //doc.Add(InnerTable);

            //#endregion

            #endregion

            doc.Close();
            #endregion

            #region Generate Esign
            try
            {
                clsDocumentESign requestPdf = new clsDocumentESign();
                byte[] bytes = File.ReadAllBytes(filepath);
                requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                requestPdf.transactionid = transactionID;
                clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, Convert.ToString(hpVM.HalkaPatwariDetailsVM.ClaimRequestDetailsID), "MultipleLocation");

                if (response.Status != "0")
                {
                    using (FileStream stream = System.IO.File.Create(EsignPath))
                    {
                        byte[] byteArray = Convert.FromBase64String(response.Document);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    isEsignGenerated = true;
                }
            }
            catch (Exception ex) { }
            #endregion
        }

        public void GeneratePattaReport(string rootPath, HalkaPatwariVM hpVM, string transactionID, ref string docName, ref bool isEsignGenerated)
        {
            #region Doc
            string filepath = string.Empty;
            string EsignPath = string.Empty;
            docName = string.Format("5_{0}_{1}_PattaReport.pdf", hpVM.HalkaPatwariDetailsVM.ClaimRequestDetailsID, hpVM.HalkaPatwariDetailsVM.WorkFlowDetailsID);
            filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
            EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);
            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 14);
            iTextSharp.text.Font hindiBold = new iTextSharp.text.Font(dev, 14, iTextSharp.text.Font.BOLD);
            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            #endregion

            #region Doc Body
            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine)); 


            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 120;
            Details.SetTotalWidth(new float[] { 5f, 50f, 35f });


            cells = new PdfPCell(new Phrase("mikca/k & 2", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("izdj.k la[;k ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(hpVM.HalkaPatwariDetailsVM.CaseNumber, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Hkkjr ljdkj", hindiBold)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("tutkrh; dk;Z ea=kYk;", hindiBold)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("¼vuwlwfpr tutkfr vkSj vU; ijEijkxr ou fuoklh ¼ou vf/kdkjksa dh ekU;rk½", hindiBold)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("fu;e 2008 dk ¼fu;e 8 ¼t½ ns[ks", hindiBold)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("vf/kHkksx ds v/khu ou Hkwfe ds fy, gd ", hindiBold)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            //add by sunny
            cells = new PdfPCell(new Phrase("fnukad ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(hpVM.HalkaPatwariDetailsVM.CompletedDate, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);
            //end


            cells = new PdfPCell(new Phrase("1-", hindi));// { Border = 0 };
            cells.Rowspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("ou vf/kdkjksa ds /kkjd ¼dks½ dk@ds uke", hindi));//{ Border = 0 }; van adhikari ke naam
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.ClaimantName + " ", fontTitle));// { Border = 0 }; pati ya patni sahit
            cells.Rowspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("¼ifr ;k ifRu lfgr½", hindi));// { Border = 0 }; pati ya patni sahit
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);


            cells = new PdfPCell(new Phrase("2-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("firk@ekrk dk uke", hindi));// { Border = 0 }; pita / mata ka naam
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.FatherName + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("3-", hindi));// { Border = 0 }; aashrito ka naam
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("vkfJrksa dk uke", hindi));// { Border = 0 }; aashrito ka naam
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.MemberNames + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("4-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("irk", hindi));// { Border = 0 }; pata
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.Address + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("5-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("xzke", hindi));// { Border = 0 }; gram
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.VillageName + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("6-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("xzke iapk;r", hindi));// { Border = 0 }; gram panchayat
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.GPName + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("7-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("rglhy@rkywdk", hindi));// { Border = 0 }; thsil/ taluka
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.TehsilName + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("8-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("ftyk", hindi));// { Border = 0 }; jila
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.DistrictName + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("9-", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("vuwlwfpr tutkfr@vU; ijEijkxr ou fuoklh", hindi));// { Border = 0 }; anusuchit janjati niwasi
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.ScheduledTribe + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("10-", hindi));// { Border = 0 };
            cells.Rowspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("{kS=Qy gsDVs;j ", hindi));// { Border = 0 };shektfal
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.TotalAreaApprovedAgainstOccupiedForestLand + " " + "Hectare", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("iz;kstu", hindi));// { Border = 0 }; prayojan
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.Purpose + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("11-", hindi));// { Border = 0 };
            cells.Rowspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("[kljk la[;k ", hindi));// { Border = 0 }; khasra sankhya
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.KhasraNumbers + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("dEikVZesaV la[;k@th-ih-,l-", hindi));// { Border = 0 }; kampartment sankya gps
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.CompartmentNumbers + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("izeq[k lhekfpUg }kjk lhekvksa dk fooj.k", hindi));// { Border = 0 }; pramukh something
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.BoundryDes + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("iwoZ & y[kk@jkxk", hindi));// { Border = 0 }; purva
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.EastLakhaRaga + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("mRRkj & jxk@ouk", hindi));// { Border = 0 }; uttar
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" " + hpVM.HalkaPatwariDetailsVM.NorthLakhaRaga + " ", fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(";g gd nk; ;ksX; gS fdUrq vf/kfu;e dh /kkjk 4 dh mi/kkjk ¼4½ ds v/khu vU; laØkE; ;k varj.kh; ugh gSAge] v/kksgLrk{kjh jktLFkku ¼jkT; dk uke½ ljdkj ds fy, vkSj mldh vksj ls mijksDr ou vf/kdkjksa dh iqf\"V djus ds fy, gLrk{kj djrs gSA", hindi)) { Border = 0 }; //long text
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            doc.Add(Details);

            #region table
            if (!string.IsNullOrEmpty(hpVM.HalkaPatwariDetailsVM.ActivityData))
            {


                PdfPTable InnerTable = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                InnerTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellit = new PdfPCell() { Border = 4 };
                InnerTable.TotalWidth = 35f;
                InnerTable.SetTotalWidth(new float[] { 30f, 30f, 30f });

                cellit = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
                cellit.Colspan = 3;
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                cellit = new PdfPCell(new Phrase("fcUnq", hindi));// { Border = 0 };
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                cellit = new PdfPCell(new Phrase("v{kka’k ¼mRrj½", hindi));//{ Border = 0 };
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                cellit = new PdfPCell(new Phrase("ns'kkUrj ¼iwoZ½", hindi));// { Border = 0 };
                cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                InnerTable.AddCell(cellit);

                List<OtherLatLong> lstOtherLatLongs = GetLatLong(hpVM.HalkaPatwariDetailsVM.ActivityData);
                int count = 1;
                foreach (var item in lstOtherLatLongs)
                {
                    cellit = new PdfPCell(new Phrase(" " + count + " ", fontTitle));// { Border = 0 };
                    cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                    InnerTable.AddCell(cellit);

                    cellit = new PdfPCell(new Phrase(" " + item.Lat + " ", fontTitle));// { Border = 0 };
                    cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                    InnerTable.AddCell(cellit);

                    cellit = new PdfPCell(new Phrase(" " + item.Long + " ", fontTitle));// { Border = 0 };
                    cellit.HorizontalAlignment = Element.ALIGN_CENTER;
                    InnerTable.AddCell(cellit);
                    count++;
                }
                doc.Add(InnerTable);
            }

            #endregion

            PdfPTable FooterDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellfd = new PdfPCell() { Border = 4 };
            FooterDetails.TotalWidth = 120;
            FooterDetails.SetTotalWidth(new float[] { 35f, 50f, 35f });

            cellfd = new PdfPCell(new Phrase("  ", hindi)) { Border = 0 }; //For Newline
            cellfd.Colspan = 3;
            cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
            FooterDetails.AddCell(cellfd);
            FooterDetails.AddCell(cellfd);
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("eaMyh; ou vf/kdkjh", hindi)) { Border = 0 };//mandaliya van adhikari
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("ftyk dyDVj", hindi)) { Border = 0 };// jila collector
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("ftyk tutkrh;", hindi)) { Border = 0 }; //jila jan jatiya
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("mi ou laj{kd", hindi)) { Border = 0 };// up van sanraksh
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("mik;qDRk", hindi)) { Border = 0 }; //upayukth
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);

            cellfd = new PdfPCell(new Phrase("dY;k.k vf/kdkjh", hindi)) { Border = 0 }; //kalyan adhikari
            cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
            FooterDetails.AddCell(cellfd);
            doc.Add(FooterDetails);
            doc.Close();
            #endregion

            #region Generate Esign
            try
            {
                clsDocumentESign requestPdf = new clsDocumentESign();
                byte[] bytes = File.ReadAllBytes(filepath);
                requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                requestPdf.transactionid = transactionID;
                requestPdf.positionX = ""; requestPdf.llx = "200";
                requestPdf.positionY = ""; requestPdf.lly = "20";

                clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, Convert.ToString(hpVM.HalkaPatwariDetailsVM.ClaimRequestDetailsID), "MultipleLocation");

                if (response.Status != "0")
                {
                    using (FileStream stream = System.IO.File.Create(EsignPath))
                    {
                        byte[] byteArray = Convert.FromBase64String(response.Document);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    isEsignGenerated = true;
                }
            }
            catch (Exception ex) { }
            #endregion
        }
        public List<OtherLatLong> GetLatLong(string commaSepratedString)
        {
            List<OtherLatLong> latlongs = new List<OtherLatLong>();
            try
            {
                char[] seprator = { ']' };
                char[] secseprator = { ',' };
                string[] a = commaSepratedString.Split(seprator, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < a.Length; i++)
                {
                    string[] aa = a[i].Split(secseprator, StringSplitOptions.RemoveEmptyEntries);
                    latlongs.Add(new OtherLatLong
                    {
                        Lat = Convert.ToString(aa.GetValue(0)).Replace("[", ""),
                        Long = Convert.ToString(aa.GetValue(1)).Replace("[", "")
                    });
                }

                //[[8499243.122066436,3044979.6671028645],[8498129.927158505,3043737.2387811244],[8497016.732250571,3042494.9179087323]]
                //8499243.122066436,3044979.6671028645]
                //,[8498129.927158505,3043737.2387811244],[8497016.732250571,3042494.9179087323
            }
            catch (Exception ex) { }

            return latlongs;
        }
        #endregion

        #region [Send email and SMS]
        public void SendSMSEmailForSuccessTransaction(long? reqID, string msg)
        {
            #region  after SUCCESS flag send SMS and Email to the user

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable dtUserDetails = objSMSandEMAILtemplate.GetUserDetails(Convert.ToString(reqID), "FRA_ClaimRequest");
            if (dtUserDetails.Rows.Count > 0)
            {
                if (Convert.ToString(dtUserDetails.Rows[0]["ApplicantEmailId"]) != string.Empty)
                {
                    body = string.Empty;

                    #region SMS Email
                    string UserMailBody = Common.FRA_ClaimRequest_EmailBody(dtUserDetails.Rows[0]["ApplicantName"].ToString(), reqID, msg);
                    string subject = "Regarding " + dtUserDetails.Rows[0]["SubjectHeading"];
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, dtUserDetails.Rows[0]["ApplicantEmailId"].ToString(), string.Empty);
                    #endregion

                }

                if (Convert.ToString(dtUserDetails.Rows[0]["ApplicantMobile"]) != string.Empty)
                {
                    string UserSmsBody = Common.FRA_ClaimRequest_SMSBody(dtUserDetails.Rows[0]["ApplicantName"].ToString(), reqID, msg);
                    SMS_EMail_Services.sendSingleSMS(dtUserDetails.Rows[0]["ApplicantMobile"].ToString(), UserSmsBody);
                }

            }
            #endregion
        }
        #endregion

        #region User Registration
        public DataSet UserRegistration_Get(long DesignationID, long DistictId)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("DesignationsId", DesignationID),
                            new SqlParameter("DistictId", DistictId),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_FRA_UserRegistration_Get", prms);
            return dsData;
        }

        public DataSet UserRegistration_Get(long userRegistrationID)
        {
            DataSet dtData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("RegistrationID", userRegistrationID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_FRA_UserRegistration_Get", prms);
            return dtData;
        }

        public ResponseMsg UserRegistration_Save(UserRegistration model)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;

            Int32 actionCode = model.RegistrationID == 0 ? 1 : 2;

            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("RegistrationID", model.RegistrationID),
                            new SqlParameter("SSOID", model.SSOID),
                            new SqlParameter("DistID", model.DistrictID),
                            new SqlParameter("TehsilID", model.TehsilID),
                            new SqlParameter("BlockID", model.BlockID),
                            new SqlParameter("GPID",model.GPID),
                            new SqlParameter("VillageCode", model.VillageCode),
                            new SqlParameter("Designation", model.Designation),
                            new SqlParameter("ActiveStatus", model.ActiveStatus),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_FRA_UserRegistration_Save", prms);

            if (dtData != null)
            {
                msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                {
                    IsError = x.Field<bool>("IsError"),
                    ReturnMsg = x.Field<string>("ReturnMsg"),
                    ReturnIDs = x.Field<string>("ReturnIDs")
                }).FirstOrDefault();
            }

            return msg;
        }

        public DataSet GetDropdownData(int actionCode, string parentID = null, string childID = null, string selectedValue = null)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("ChildID", childID),
                            new SqlParameter("SelectedValue", selectedValue),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_FRA_GetDropdownData", prms);
            return dsData;
        }

        #endregion

        #region Report
        public DataSet GetClaimRequestDetails_Rpt(ClaimRequestParamVM param)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("@ActionCode",1),
                                        new SqlParameter("@FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("@ToDate",Util.GetDate(param.ToDate)),
                                        new SqlParameter("@ClaimTypeID",param.ClaimTypeID),
                                        new SqlParameter("DistrictID",param.DistrictID),
                                        new SqlParameter("BlkID",param.BlockID),
                                        new SqlParameter("GPID",param.GPID),
                                        new SqlParameter("@SSOID",HttpContext.Current.Session["SSOID"])

                                   };
            _db.Fill(dsData, "SP_FRA_rpt_ClaimRequestDetails", prms);
            return dsData;
        }

        public DataSet GetClaimRequestSummary_Rpt(ClaimRequestParamVM param)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("ActionCode",1),
                                        new SqlParameter("FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("ToDate",Util.GetDate(param.ToDate)),
                                        new SqlParameter("ClaimTypeID",param.ClaimTypeID),
                                        new SqlParameter("DistrictID",param.DistrictID),
                                        new SqlParameter("BlkID",param.BlockID),
                                        new SqlParameter("GPID",param.GPID),
                                        new SqlParameter("SSOID",HttpContext.Current.Session["SSOID"]),
                                        new SqlParameter("UserID", HttpContext.Current.Session["UserId"])
                                   };
            _db.Fill(dsData, "SP_FRA_rpt_ClaimRequestSummary", prms);
            return dsData;
        }

        public DataSet GetClaimRequestSummarySub_Rpt(ClaimRequestSubParamVM param)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("ActionCode",param.ActionCode),
                                        new SqlParameter("ClaimTypeID",param.ClaimTypeID),
                                        new SqlParameter("DistrictID",param.DistrictID),
                                        new SqlParameter("BlkID",param.BlockID),
                                        new SqlParameter("GPID",param.GPID),
                                        new SqlParameter("FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("ToDate",Util.GetDate(param.ToDate)),
                                        new SqlParameter("UserID", HttpContext.Current.Session["UserId"])
        };
            _db.Fill(dsData, "SP_FRA_rpt_ClaimRequestSummarySubDetails", prms);
            return dsData;
        }
        public DataSet GetClaimRequestDetails_RptAdmin(ClaimRequestParamVM param)
        {
            string SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"]);
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("@ActionCode",2),
                                        new SqlParameter("@FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("@ToDate",Util.GetDate(param.ToDate)),
                                        new SqlParameter("@ClaimTypeID",param.ClaimTypeID),
                                        new SqlParameter("@SSOID",SSOID)
                                   };
            _db.Fill(dsData, "SP_FRA_rpt_ClaimRequestDetails", prms);
            return dsData;
        }
        public DataSet GetClaimRequestSummary_RptAdmin(ClaimRequestParamVM param)
        {
            string SSOID = Convert.ToString(System.Web.HttpContext.Current.Session["SSOID"]);
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("ActionCode",2),
                                        new SqlParameter("FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("ToDate",Util.GetDate(param.ToDate)),
                                        new SqlParameter("ClaimTypeID",param.ClaimTypeID),
                                        new SqlParameter("@SSOID",SSOID)
                                   };
            _db.Fill(dsData, "SP_FRA_rpt_ClaimRequestSummary", prms);
            return dsData;
        }

        private bool GenerateESignPDF(string filepath, string eSignPath, clsVerifyOTPResponce otpResponse, string reqID, string posX, string posY, string llX, string llY)
        {
            bool isEsignGenerated = false;
            #region Generate Esign
            try
            {
                clsDocumentESign requestPdf = new clsDocumentESign();
                byte[] bytes = File.ReadAllBytes(filepath);
                requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                requestPdf.transactionid = otpResponse.TransactionId;
                requestPdf.positionX = posX; requestPdf.llx = llX;
                requestPdf.positionY = posY; requestPdf.lly = llY;

                clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, reqID, "MultipleLocation");

                if (response.Status != "0")
                {
                    using (FileStream stream = System.IO.File.Create(eSignPath))
                    {
                        byte[] byteArray = Convert.FromBase64String(response.Document);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    isEsignGenerated = true;
                }
            }
            catch (Exception ex) { }
            #endregion
            return isEsignGenerated;
        }
        #endregion

        private string GetRequestInXML(AppealRequestVM model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<AppealRequest>");
            sb.Append("<documents>");

            if (model.UploadRejectionNoticeOrPatta != null)
            {
                sb.Append("<document>");
                sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName>", 0, 6, 0, 46, model.UploadRejectionNoticeOrPatta.FileName));
                sb.Append("</document>");
            }

            if (model.UploadOtherEvidenceOrdocuments != null)
            {
                sb.Append("<document>");
                sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName>", 0, 6, 0, 47, model.UploadOtherEvidenceOrdocuments.FileName));
                sb.Append("</document>");
            }

            sb.Append("</documents>");

            sb.Append("</AppealRequest>");
            return Convert.ToString(sb);
        }

        private void SaveAppealDoc(AppealRequestVM model, string objectID)
        {
            CommonRepository repository = new CommonRepository();
            string docPath = FMDSS.Globals.Util.GetAppSettings("FRADocumentPath");
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

            if (!string.IsNullOrWhiteSpace(objectID) && objectID.Split('|').Count() == 2)
            {
                if (model.UploadRejectionNoticeOrPatta != null)
                {
                    repository.SaveFile(model.UploadRejectionNoticeOrPatta, Convert.ToInt32(objectID.Split('|')[0]), Convert.ToInt64(objectID.Split('|')[1]), 46, dirPath);
                }
                if (model.UploadOtherEvidenceOrdocuments != null)
                {
                    repository.SaveFile(model.UploadOtherEvidenceOrdocuments, Convert.ToInt32(objectID.Split('|')[0]), Convert.ToInt64(objectID.Split('|')[1]), 47, dirPath);
                }
            }

        }
    }
}