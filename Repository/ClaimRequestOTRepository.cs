using FMDSS.Entity;
using FMDSS.Entity.FRAViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Globals;
using FMDSS.Models;
using FMDSS.Models.Encroachment.DomainModel;
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
using System.Xml.Linq;

namespace FMDSS.Repository
{
    public class ClaimRequestOTRepository : IClaimRequestOTRepository
    {
        #region Properties & Variables
        private FmdssContext dbContext;
        private FMDSS.Models.DAL _db = new Models.DAL();
        private long userID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserId"]);
        #endregion

        #region Constructor
        public ClaimRequestOTRepository()
        {
            dbContext = new Models.FmdssContext.FmdssContext();
        }
        #endregion

        #region Claim Request Operation 
        public void SaveClaimRequestDetails(ClaimRequestOTVM model, ref string returnMsg, ref Boolean isError)
        {
            try
            {
                Int64 claimRequestDetailsID = model.ClaimRequestDetails.ClaimRequestDetailsID;
                var xmlData = GetRequestInXML(model);
                var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", userID),
                            new SqlParameter("xmlFile", xmlData)};
                DataSet data = new DataSet();
                _db.Fill(data, "SP_FRA_ClaimRequestDetailsOTSave", prms);

                var workFlowModel = Util.GetListFromTable<WorkFlowVM>(data, 0).FirstOrDefault();
                returnMsg = workFlowModel.ReturnMsg;
                isError = workFlowModel.IsError;
                if (!isError)
                {
                    if (model.ClaimRequestDocument != null)
                    {
                        SaveClaimReqDocs(workFlowModel.ClaimRequestDetailsID, workFlowModel.WorkFlowDetailsID, model.ClaimRequestDocument.Where(x => x.IsNew).ToList());
                    }  
                }
            }
            catch (Exception ex) { throw ex; }
        }
        public ClaimRequestOTVM GetClaimRequestDetails(ClaimRequestOTVM model, long claimReqID)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("ClaimRequestDetailsID", claimReqID),
                            new SqlParameter("ClaimTypeID", model.ClaimRequestDetails==null? 0: model.ClaimRequestDetails.ClaimTypeID)};
            DataSet data = new DataSet(); 
            _db.Fill(data, "SP_FRA_GetClaimRequestDetails", prms);

            if (Util.isValidDataSet(data))
            {
                Parallel.Invoke(
                () => model.DistrictList = Util.GetListFromTable<tbl_FRA_District>(data, 0).Select(t => new DropDownListVM { Text = t.DistrictName, Value = t.DistrictID }).ToList(),
                () => model.DocumentTypeList = Util.GetListFromTable<CommonDocumentType>(data, 1).Select(t => new DropDownListVM { Text = t.DocumentTypeName, Value = t.DocumentTypeID }).ToList(),
                () => model.ClaimTypeList = Util.GetListFromTable<tbl_FRA_ClaimType>(data, 2).Select(t => new DropDownListVM { Text = t.ClaimTypeName, Value = t.ClaimTypeID }).ToList(),
                () => model.WorkFlowRuleList = Util.GetListFromTable<tbl_FRA_WorkFlowRule>(data, 3)
                );

            }
            return model;
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
                //obj.DocumentPath = string.Format("{0}{1}_{2}_{3}", docPath, reqID, item.DocumentTypeID, item.DocumentName);
                //obj.DocumentTypeID = item.DocumentTypeID;
                //obj.TempID = item.TempID;
                //obj.ActiveStatus = true;
                //dbContext.Entry(obj).State = System.Data.Entity.EntityState.Added;
            }
        }

        //private void SaveClaimReqDocsForCitizen(long? reqID, long? workFlowDetailsID, List<CommonDocument> docs)
        //{
        //    string docPath = Util.GetAppSettings("FRADocumentPath");
        //    string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

        //    foreach (var item in docs)
        //    {
        //        FileInfo f1 = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/" + item.DocumentPath));
        //        f1.CopyTo(string.Format("{0}{1}_{2}_{3}", dirPath, reqID, item.DocumentTypeID, item.DocumentName), true);
        //        f1.Delete(); 
        //    }
        //}
        private string GetRequestInXML(ClaimRequestOTVM model)
        {
            var data = model.ClaimRequestDetails;
            StringBuilder sb = new StringBuilder();
            sb.Append("<claimRequest>");
            sb.Append(string.Format(@"<SSOID>{0}</SSOID>
                                      <RequestDate>{1}</RequestDate>", 
                model.ClaimRequestDetails.SSOID, Util.GetDateWithFormat(model.ClaimRequestDetails.EnteredOn,"MM/dd/yyyy")));
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
                            <IsForesterGenerated>{12}</IsForesterGenerated>", data.ClaimRequestDetailsID, data.ClaimTypeID, data.ClaimRequestPurposeID, data.DistrictID,
                            data.TehsilID, data.VillageCode, data.GPID, data.BlockID, data.CompartmentNumber, data.KhasraNumber, data.IsPattaGenerated, data.IsHalkaPatwariGenerated, data.IsForesterGenerated));
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
                      data.Individual_DisputedLands, data.Individual_PLGrants, data.Individual_LFISROAlternativeLand,data.Individual_LFWDisplacedWLCompensation, data.Individual_EOLIFVillages, data.Individual_AOTRight));
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
                        </ClaimantDetail>", item.ClaimantDetailsID,item.BhamashahID,item.ClaimantName,item.FatherName,item.SpouseName,item.Email,item.Mobile,item.Gender));
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
                        </BorderingVillageDetail>",item.BorderingVillageDetailsID,item.VillageCode));
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
                        </MemberDetail>", item.MemberDetailsID,item.BhamashahID,item.MemberName,item.FatherName,item.SpouseName,item.Age,item.Email,item.Mobile,item.IsDependant,item.Gender));
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
            if (model.ApproverRemarksVM != null)
            {
                sb.Append("<ApproverActions>");

                foreach (var item in model.ApproverRemarksVM)
                {
                    sb.Append(string.Format(@" 
                    <ApproverAction>
                      <StatusID>{0}</StatusID>
                      <SSOID>{1}</SSOID>  
                      <ApproverComment>{2}</ApproverComment>
                      <ApproverComment1>{3}</ApproverComment1>
                      <ApprovedDate>{4}</ApprovedDate>
                      <CaseNumber>{5}</CaseNumber>
                    </ApproverAction>", item.StatusID,item.SSOID, item.ApproverComment,item.ApproverComment1,Util.GetDateWithFormat(item.EnteredOn,"MM/dd/yyyy"),item.CaseNumber));
                }
                sb.Append("</ApproverActions>");
            }
            sb.Append("</claimRequest>");
            return Convert.ToString(sb);
        }

        #endregion
          
        #region ClaimRequestOT
        public bool IsValidSSO(string ssoID)
        {
            var isexist = dbContext.tbl_UserProfiles.AsQueryable().Where(x => x.Ssoid.Trim().ToLower().Equals(ssoID.ToLower())).Count();
            return isexist > 0 ? true : false;
        }
        #endregion 
    }
}