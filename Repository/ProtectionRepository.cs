using FMDSS.Entity;
using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FMDSS.Entity.ViewModel;
using System.IO;
using System.Text;
using FMDSS.Globals;
using FMDSS.Models.ForestFire;
using System.Globalization;
using Newtonsoft.Json;
using System.Net;
using FMDSS.Models;
using System.Web.Mvc;

namespace FMDSS.Repository
{
    public class ProtectionRepository : IProtectionRepository
    {
        #region Properties & Variables
        private FMDSS.Models.DAL _db = new Models.DAL();
        #endregion

        #region Offence operations
        public ResponseMsg OffenceDetails_Save(OffenceDetailsModel model,int isMobile)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var attachedDoc = new List<CommonDocument>();
            //var attachedDoc2 = new List<CommonDocument>();

            if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
            {
                attachedDoc = ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.IsNew).ToList();
                //attachedDoc2= ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).ToList();
            }

            Int32 actionCode = model.ID == 0 ? 1 : 2;
            var xmlData = GetRequestInXML(model);

            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ID", model.ID),
                            new SqlParameter("RequestType", model.RequestType),
                            new SqlParameter("RangeCode", model.RangeCode),
                            new SqlParameter("NakaID", model.NakaID),
                            new SqlParameter("FIRNumber", model.FIRNumber),
                            new SqlParameter("FIRDate",Util.GetDate(model.FIRDate)),
                            new SqlParameter("OffenderName", model.OffenderName),
                            new SqlParameter("OffenderAddress", model.OffenderAddress),
                            new SqlParameter("OffenceDescription", model.OffenceDescription),
                            new SqlParameter("OffenceCategory", model.OffenceCategory),
                            new SqlParameter("NoOfTree", model.NoOfTree),
                            new SqlParameter("VolumeInCubicMetre", model.VolumeInCubicMetre),
                            new SqlParameter("IsWPA", model.IsWPA),
                            new SqlParameter("WPADescription", model.WPADescription),
                            new SqlParameter("IsFA", model.IsFA),
                            new SqlParameter("FADescription", model.FADescription),
                            new SqlParameter("InvestigatorOfficer", model.InvestigatorOfficer),
                            new SqlParameter("Latitude", model.Latitude),
                            new SqlParameter("Longitude", model.Longitude),
                            new SqlParameter("CompoundStatus", model.CompoundStatus),
                            new SqlParameter("CompoundAmount", model.CompoundAmount),
                            new SqlParameter("IsMaterialReleased", model.IsMaterialReleased),
                            new SqlParameter("IsVehicleReleased", model.IsVehicleReleased),
                            new SqlParameter("NotCompoundedStatus", model.NotCompoundedStatus),
                            new SqlParameter("CourtName", model.CourtName),
                            new SqlParameter("FileDate", Util.GetDate(model.FileDate)),
                            new SqlParameter("CourtCaseNumber", model.CourtCaseNumber),
                            new SqlParameter("NextHearingDate", Util.GetDate(model.NextHearingDate)),
                            new SqlParameter("DateOfFinalReport", Util.GetDate(model.DateOfFinalReport)),
                            new SqlParameter("DateOfApprovalByDFO", Util.GetDate(model.DateOfApprovalByDFO)),
                            new SqlParameter("SpecialRemarks", model.SpecialRemarks),
                            new SqlParameter("xmlFile", xmlData),
                            new SqlParameter("StatusID", model.Status),                            
                            new SqlParameter("isMobile", isMobile),                          
                            new SqlParameter("CompoundingDate", model.CompoundingDate),

                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_PM_OffenceDetails_Save", prms);

            if (dtData != null)
            {
                msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                {
                    IsError = x.Field<bool>("IsError"),
                    ReturnMsg = x.Field<string>("ReturnMsg"),
                    ReturnIDs = x.Field<string>("ReturnIDs")
                }).FirstOrDefault();

                if (attachedDoc.Count > 0)
                {
                    SaveDocs(msg.ReturnIDs, 1, attachedDoc);
                }
            }

            return msg;
        }

        public ResponseMsg OffenceDetails_Update(ApproverRemarks model)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("OffenceDetailsID", model.RequestID),
                            new SqlParameter("StatusID", model.StatusID),
                             new SqlParameter("SpecialRemarks", model.ApproverComment),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_PM_OffenceDetails_Update", prms);

            if (dtData != null)
            {
                if (dtData.Rows.Count > 0)
                {
                    msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                    {
                        IsError = x.Field<bool>("IsError"),
                        ReturnMsg = x.Field<string>("ReturnMsg")
                    }).FirstOrDefault();
                }
            }

            return msg;
        }

        public ResponseMsg OffenceDetails_Delete(long offenceDetailsID)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("OffenceDetailsID", offenceDetailsID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_PM_OffenceDetails_Update", prms);

            if (dtData != null)
            {
                if (dtData.Rows.Count > 0)
                {
                    msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                    {
                        IsError = x.Field<bool>("IsError"),
                        ReturnMsg = x.Field<string>("ReturnMsg")
                    }).FirstOrDefault();
                }
            }

            return msg;
        }

        public ResponseMsg FileUpload(DataSet ds, UploadOffenceDetailsModel model)
        {
            ResponseMsg msg = null;
            var xmlData = ds.GetXml().Replace("'", @"''");
            DataTable dtData = new DataTable();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("RangeCode", model.RangeCode),
                            new SqlParameter("xmlFile", xmlData),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_PM_OffenceDetails_Upload", prms);

            if (dtData != null)
            {
                if (dtData.Rows.Count > 0)
                {
                    msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                    {
                        IsError = x.Field<bool>("IsError"),
                        ReturnMsg = x.Field<string>("ReturnMsg")
                    }).FirstOrDefault();
                }
            }
            return msg;
        }

        public DataSet OffenceDetails_Get(int pageNumber, int pageSize, string FDate, string TDate)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                             new SqlParameter("PageNumber", pageNumber),
                             new SqlParameter("PageSize", pageSize),
                              new SqlParameter("FromDate", FDate),
                               new SqlParameter("ToDate", TDate),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }

        public DataSet OffenceDetails_GetPermission()
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 6),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }
        public DataSet OffenceDetails_EditHistory(long RequestId)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 11),
                            new SqlParameter("RequestId", RequestId) };
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            
            return dsData;
        }
        public DataSet OffenceDetails_History(long logID)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 12),
                            new SqlParameter("@LogId", logID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }
        public DataSet OffenceDetails_GetDropdownData()
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 5),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }

        public DataSet DivisionList_GetDropdownData()
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 9),
                            new SqlParameter("OffenceDetailsID", 0),
                            new SqlParameter("PageNumber", 0),
                            new SqlParameter("PageSize", 0),
                            new SqlParameter("CircelCode", ""),
                            new SqlParameter("DivCode", ""),
                            new SqlParameter("RangeCode", ""),
                            new SqlParameter("NakaId", 0),
                            new SqlParameter("UserID",Convert.ToInt64(HttpContext.Current.Session["UserId"]))};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }

        public DataSet OffenceDetails_GetDetailsForUpdation(long offenceDetailsID)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 4),
                            new SqlParameter("OffenceDetailsID", offenceDetailsID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }
        
        public DataSet OffenceDetailsItemWise_Get()
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            return dsData;
        }

        public OffenceDetailsWithLog GetLogDetails(long? offenceDetailsID)
        {
            OffenceDetailsWithLog log = new OffenceDetailsWithLog();
            var prms = new[]{ new SqlParameter("ActionCode", 3),
                              new SqlParameter("OffenceDetailsID", offenceDetailsID),
                              new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            DataSet ds = new DataSet();
            _db.Fill(ds, "SP_PM_OffenceDetails_Get", prms);
            log.ViewOffenceDetails = FMDSS.Globals.Util.GetListFromTable<ViewOffenceDetails>(ds, 0).FirstOrDefault();
            log.OffenceLogList = FMDSS.Globals.Util.GetListFromTable<OffenceLog>(ds, 1);
            return log;
        }

        private void SaveDocs(string objectID, int objectTypeID, List<CommonDocument> docs)
        {
            string docPath = FMDSS.Globals.Util.GetAppSettings("FRADocumentPath");
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

            foreach (var item in docs)
            {
                FileInfo f1 = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/" + item.DocumentPath));
                f1.CopyTo(string.Format("{0}{1}_{2}_{3}_{4}", dirPath, objectTypeID, objectID, item.DocumentTypeID, item.DocumentName), true);
                f1.Delete();
            }
        }

        private string GetRequestInXML(OffenceDetailsModel model)
        {
            var attachedDataDoc = new List<CommonDocument>();
            var attachedDataDoc2 = new List<CommonDocument>();
            if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
            {
                attachedDataDoc = ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.IsNew).ToList();
                attachedDataDoc2 = ((List<CommonDocument>)System.Web.HttpContext.Current.Session["UploadFile"]).ToList();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<offenceRequest>");
            if (model.SeizedItemsList != null && model.SeizedItemsList.Count > 0)
            {
                sb.Append("<seizedItems>");
                foreach (var item in model.SeizedItemsList)
                {
                    sb.Append("<seizedItem>");
                    sb.Append(string.Format(@"
                            <ID>{0}</ID>
                            <OffenceDetailsID>{1}</OffenceDetailsID>
                            <ItemTypeID>{2}</ItemTypeID>
                            <ItemName>{3}</ItemName>
                            <Qty>{4}</Qty>
                            <VehicleNumber>{5}</VehicleNumber>
                            ", item.ID,
                            item.OffenceDetailsID,
                            item.ItemTypeID,
                            item.ItemName, 
                            item.Qty,
                            item.VehicleNumber
                            ));
                    sb.Append("</seizedItem>");
                }
                sb.Append("</seizedItems>");
            }
            //int atch = attachedDataDoc2.Count;
            //if (attachedDataDoc.Count > 0)
            if (attachedDataDoc2.Count > 0)
            {
                sb.Append("<documents>");
                //foreach (var item in attachedDataDoc)
                foreach (var item in attachedDataDoc2)
                {
                    sb.Append("<document>");
                    sb.Append(string.Format(@"
                    <DocumentID>{0}</DocumentID>
                            <ObjectTypeID>{1}</ObjectTypeID>
                            <ObjectID>{2}</ObjectID> 
                            <DocumentTypeID>{3}</DocumentTypeID>
                            <DocumentName>{4}</DocumentName> 
                            <DocumentPath>{5}</DocumentPath>
                            <IsESign>{6}</IsESign>", item.DocumentID, item.ObjectTypeID, item.ObjectID, item.DocumentTypeID, item.DocumentName, item.DocumentPath, item.IsESign));
                    sb.Append("</document>");
                }
                sb.Append("</documents>");
            }

            sb.Append("</offenceRequest>");
            return Convert.ToString(sb);
        }

        #endregion

        #region Forest Fire
        public DataSet ForestFire_AddDetailsReport(ForestFire_AddDetailsReport model)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("@ActionCode", 3),
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_ForestFireExcelData", prms);
            return dsData;
        }
        #endregion

        #region Report
        public DataTable OffenceDetails_Report(OffenceReportVM model)
        {
            DataTable dtData = new DataTable();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("CircleCode", model.CircleCode),
                            new SqlParameter("DivisionCode", model.DivisionCode),
                            new SqlParameter("RangeCode", model.RangeCode),
                            new SqlParameter("NakaID", model.NakaID),
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate),
                            new SqlParameter("StatusID", model.StatusID),
                            new SqlParameter("OffenceCategory", model.OffenceCategory==null?"":string.Join(",",model.OffenceCategory)),
                            new SqlParameter("SearchBy", model.Searchby),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_PM_RPT_OffenceDetails", prms);
            return dtData;
        }

        public DataSet OffenceSummary_Report(OffenceReportVM model)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_RPT_OffenceDetails", prms);
            return dsData;
        }

        public DataSet OffenceSummarySub_Report(OffenceSubParamVM param)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("ActionCode",param.ActionCode),
                                        new SqlParameter("DIST_CODE",param.DIST_CODE),
                                        new SqlParameter("FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("ToDate",Util.GetDate(param.ToDate))
                                   };
            _db.Fill(dsData, "SP_PM_RPT_OffenceSummarySubDetails", prms);
            return dsData;
        }

        public DataSet OffenceSummaryQtr_Report(OffenceReportVM model)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode",4),
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate),
                               new SqlParameter("CircleCode", model.CircleCode),
                            new SqlParameter("DivisionCode", model.DivisionCode),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_RPT_OffenceDetails", prms);
            return dsData;
        }

        public DataSet OffenceSummaryQtrSub_Report(OffenceSubParamVM param)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("ActionCode",param.ActionCode),
                                        new SqlParameter("DIV_CODE",param.DIV_CODE),
                                        new SqlParameter("FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("ToDate",Util.GetDate(param.ToDate))
                                   };
            _db.Fill(dsData, "SP_PM_RPT_OffenceSummarySubDetails", prms);
            return dsData;
        }

        public DataSet EncroachmentSummary_Report(OffenceReportVM model)
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 3),
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_RPT_OffenceDetails", prms);
            return dsData;
        }

        public DataSet EncroachmentSummarySub_Report(OffenceSubParamVM param)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] prms = {
                                        new SqlParameter("ActionCode",param.ActionCode),
                                        new SqlParameter("DIST_CODE",param.DIST_CODE),
                                        new SqlParameter("FromDate",Util.GetDate(param.FromDate)),
                                        new SqlParameter("ToDate",Util.GetDate(param.ToDate))
                                   };
            _db.Fill(dsData, "SP_PM_RPT_EncroachmentSummarySubDetails", prms);
            return dsData;
        }
        #endregion

        #region Add by sunny for OffenceAPI
        public ResponseMsg OffenceDetails_SaveAPI(OffenceDetailsModel_API model,int isMobile)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var attachedDoc = new List<CommonDocumentAPI>();

            //if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
            //{
                //attachedDoc = ((List<CommonDocumentAPI>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.IsNew).ToList();
                attachedDoc = model.CommonDocApiList.Where(x => x.IsNew).ToList(); 
            //}

            Int32 actionCode = model.ID == 0 ? 1 : 2;
            var xmlData = GetRequestInXML(model);
            string xmlFileList="";
            DataTable dtImg = new DataTable("Tbl_CommonDocApiList");

            dtImg.Columns.Add("DocumentID", typeof(long));
            dtImg.Columns.Add("ObjectTypeID", typeof(int));
            dtImg.Columns.Add("ObjectID", typeof(long));
            dtImg.Columns.Add("DocumentName", typeof(string));
            dtImg.Columns.Add("DocumentTypeID", typeof(int));
            dtImg.Columns.Add("DocumentPath", typeof(string));
            dtImg.Columns.Add("DocumentTypeName", typeof(string));
            dtImg.Columns.Add("IsESign", typeof(bool));
            dtImg.Columns.Add("ActiveStatus", typeof(bool));
            dtImg.Columns.Add("TempID", typeof(string));
            dtImg.Columns.Add("IsNew", typeof(bool));
            dtImg.Columns.Add("DocumentLevel", typeof(int));
            if (model.CommonDocApiList.Count > 0)
            {
                foreach (var itm in model.CommonDocApiList)
                {
                    DataRow dr = dtImg.NewRow();
                    dr["DocumentID"] = itm.DocumentID;
                    dr["ObjectTypeID"] = itm.ObjectTypeID;
                    dr["ObjectID"] = itm.ObjectID;
                    dr["DocumentName"] = itm.DocumentName;
                    dr["DocumentTypeID"] = itm.DocumentTypeID;
                    dr["DocumentPath"] = itm.DocumentPath;
                    dr["DocumentTypeName"] = itm.DocumentTypeName;
                    dr["IsESign"] = itm.IsESign;
                    dr["ActiveStatus"] = itm.ActiveStatus;
                    dr["TempID"] = itm.TempID;
                    dr["IsNew"] = itm.IsNew;
                    dr["DocumentLevel"] = itm.IsNew;
                    dtImg.Rows.Add(dr);
                }
                using (StringWriter sw = new StringWriter())
                {
                    dtImg.WriteXml(sw);
                    xmlFileList = sw.ToString();
                }
            }            
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ID", model.ID),
                            new SqlParameter("RequestType", model.RequestType),
                            new SqlParameter("RangeCode", model.RangeCode),
                            new SqlParameter("NakaID", model.NakaID),
                            new SqlParameter("FIRNumber", model.FIRNumber),
                            new SqlParameter("FIRDate",Util.GetDate(model.FIRDate)),
                            new SqlParameter("OffenderName", model.OffenderName),
                            new SqlParameter("OffenderAddress", model.OffenderAddress),
                            new SqlParameter("OffenceDescription", model.OffenceDescription),
                            new SqlParameter("OffenceCategory", model.OffenceCategory),
                            new SqlParameter("NoOfTree", model.NoOfTree),
                            new SqlParameter("VolumeInCubicMetre", model.VolumeInCubicMetre),
                            new SqlParameter("IsWPA", model.IsWPA),
                            new SqlParameter("WPADescription", model.WPADescription),
                            new SqlParameter("IsFA", model.IsFA),
                            new SqlParameter("FADescription", model.FADescription),
                            new SqlParameter("InvestigatorOfficer", model.InvestigatorOfficer),
                            new SqlParameter("Latitude", model.Latitude),
                            new SqlParameter("Longitude", model.Longitude),
                            new SqlParameter("CompoundStatus", model.CompoundStatus),
                            new SqlParameter("CompoundAmount", model.CompoundAmount),
                            new SqlParameter("IsMaterialReleased", model.IsMaterialReleased),
                            new SqlParameter("IsVehicleReleased", model.IsVehicleReleased),
                            new SqlParameter("NotCompoundedStatus", model.NotCompoundedStatus),
                            new SqlParameter("CourtName", model.CourtName),
                            new SqlParameter("FileDate", Util.GetDate(model.FileDate)),
                            new SqlParameter("CourtCaseNumber", model.CourtCaseNumber),
                            new SqlParameter("NextHearingDate", Util.GetDate(model.NextHearingDate)),
                            new SqlParameter("DateOfFinalReport", Util.GetDate(model.DateOfFinalReport)),
                            new SqlParameter("DateOfApprovalByDFO", Util.GetDate(model.DateOfApprovalByDFO)),
                            new SqlParameter("SpecialRemarks", model.SpecialRemarks),
                            new SqlParameter("xmlFile", xmlData),
                            new SqlParameter("StatusID", model.StatusID),
                            new SqlParameter("UserID", model.UserID),
                            new SqlParameter("EnteredFrom", model.MobileDeviceName),  /////Change by Amit on 15-07-2019
                            new SqlParameter("VersionNo", model.MobileVersionNo),  /////Change by mukesh jangid on 07-02-2020
                            new SqlParameter("xmlFileList",xmlFileList),
                            new SqlParameter("isMobile",isMobile),                           
                            new SqlParameter("CompoundingDate", model.CompoundingDate),

            };
            _db.Fill(dtData, "SP_PM_OffenceDetails_Save", prms);

            if (dtData != null)
            {
                msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                {
                    IsError = x.Field<bool>("IsError"),
                    ReturnMsg = x.Field<string>("ReturnMsg"),
                    ReturnIDs = x.Field<string>("ReturnIDs")
                }).FirstOrDefault();

                if (attachedDoc.Count > 0)
                {
                    SaveDocs(msg.ReturnIDs, 1, attachedDoc);
                }
            }

            return msg;
        }
        private void SaveDocs(string objectID, int objectTypeID, List<CommonDocumentAPI> docs)
        {
            string docPath = FMDSS.Globals.Util.GetAppSettings("FRADocumentPath");
            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

            foreach (var item in docs)
            {
                FileInfo f1 = new FileInfo(System.Web.HttpContext.Current.Server.MapPath("~/" + item.DocumentPath));
                f1.CopyTo(string.Format("{0}{1}_{2}_{3}_{4}", dirPath, objectTypeID, objectID, item.DocumentTypeID, item.DocumentName), true);
                f1.Delete();
            }
        }
        private string GetRequestInXML(OffenceDetailsModel_API model)
        {
            var attachedDataDoc = new List<CommonDocumentAPI>();
            if (System.Web.HttpContext.Current.Session["UploadFile"] != null)
            {
                attachedDataDoc = ((List<CommonDocumentAPI>)System.Web.HttpContext.Current.Session["UploadFile"]).Where(x => x.IsNew).ToList();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("<offenceRequest>");
            if (model.SeizedItemsList != null && model.SeizedItemsList.Count > 0)
            {
                sb.Append("<seizedItems>");
                foreach (var item in model.SeizedItemsList)
                {
                    sb.Append("<seizedItem>");
                    sb.Append(string.Format(@"
                            <ID>{0}</ID>
                            <OffenceDetailsID>{1}</OffenceDetailsID>
                            <ItemTypeID>{2}</ItemTypeID>
                            <ItemName>{3}</ItemName>
                            <Qty>{4}</Qty>
                            <VehicleNumber>{5}</VehicleNumber>", item.ID, item.OffenceDetailsID, item.ItemTypeID, item.ItemName, item.Qty,item.VehicleNumber));
                    sb.Append("</seizedItem>");
                }
                sb.Append("</seizedItems>");
            }

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
                            <IsESign>{6}</IsESign>", item.DocumentID, item.ObjectTypeID, item.ObjectID, item.DocumentTypeID, item.DocumentName, item.DocumentPath, item.IsESign));
                    sb.Append("</document>");
                }
                sb.Append("</documents>");
            }

            sb.Append("</offenceRequest>");
            return Convert.ToString(sb);
        }
        public DataSet OffenceDetails_GetDropdownDataAPI()
        {
            DataSet dsData = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 7),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dsData, "SP_PM_OffenceDetails_Get", prms);
            dsData.Tables[0].TableName = "Range";
            dsData.Tables[1].TableName = "OffenceCategory";
            dsData.Tables[2].TableName = "Status";
            dsData.Tables[3].TableName = "ItemType";
            return dsData;
        }
        public DataTable GetCircle()
        {
            DataTable data = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", "Circle") };
                _db.Fill(data, "SP_GetCommonDataForOffence", prms);
            }
            catch (Exception ex) { }
            data.TableName = "Circle";
            return data;
        }
        public DataSet GetFinancialYear()
        {
            DataSet dsData = new DataSet();
            _db.Fill(dsData, "spYearList");
             dsData.Tables[0].TableName = "YearList";
            return dsData;            
        }
        public DataSet GetFinancialYear2()
        {
            DataSet dsData = new DataSet();
            _db.Fill(dsData, "spNurseryYearList");
            dsData.Tables[0].TableName = "YearList";
            return dsData;
        }
        #endregion
        #region Set Duplicate FIR
        public ResponseMsg SetDuplicateOffenceFIR(long CurrentRequestId, long RefRequestCaseId, string Remarks)
        {
            //Alter table[tbl_PM_OffenceDetails] ADD RefRequestFIRId bigint default null,FIRDupStats varchar(20) default null
            ResponseMsg msg = new ResponseMsg();
            DataTable data = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("@ActionCode", 5) ,
                            new SqlParameter("@CurrentRequestId",CurrentRequestId),
                            new SqlParameter("@RefRequestCaseId", RefRequestCaseId),
                            new SqlParameter("@Remarks", Remarks),
                            new SqlParameter("@UserID", HttpContext.Current.Session["UserId"]),                            
                };
                _db.Fill(data, "SP_PM_RPT_OffenceDetails", prms);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }
            data.TableName = "Result";
            List<ResponseMsg> Lst = Globals.Util.GetListFromTable<ResponseMsg>(data);
            msg = Lst.FirstOrDefault();
            return msg;
        }
        #endregion
        
        #region Update Forest Fire Data From GIS
        public async System.Threading.Tasks.Task<string> UpdateForesFireDataFromGISAsync()
        {
            int TotalRecordUpdated = 0;
            string longLat = "";
            string URL = "https://gistest1.rajasthan.gov.in/webapi/rajdharaa/getadminmaster";
            List<DBExeclDataModel> Lst = GetDataList();
            foreach (DBExeclDataModel obj in Lst)
            {
                if (obj.Latitude == null || obj.Longitude == null)
                    continue;
                if (obj.Latitude == "" || obj.Longitude == "")
                    continue;

                decimal degLat; decimal minutLat; decimal secondLat;
                decimal degLng; decimal minutLng; decimal secondLng;
                //////Decimal Degrees = degrees + (minutes / 60) + (seconds / 3600)
                if (obj.Latitude.Contains('N') == true)
                {
                    string strLat = "";
                    string strLng = "";
                    if (obj.Latitude.Contains("ï¿½") == true)
                    {
                        strLat = obj.Latitude.Substring(0, obj.Latitude.Length - 2);
                        strLat = strLat.Replace("ï¿½", "°");
                        strLng = obj.Longitude.Substring(0, obj.Longitude.Length - 2);
                        strLng = strLng.Replace("ï¿½", "°");
                    }
                    else if (obj.Latitude.Contains("Â") == true)
                    {
                        strLat = obj.Latitude.Substring(0, obj.Latitude.Length - 2);
                        strLat = strLat.Replace("Â", "");
                        strLng = obj.Longitude.Substring(0, obj.Longitude.Length - 2);
                        strLng = strLng.Replace("Â", "");
                    }
                    else
                    {
                        strLat = obj.Latitude.Substring(0, obj.Latitude.Length - 2);
                        strLng = obj.Longitude.Substring(0, obj.Longitude.Length - 2);
                    }


                    string[] strSplLat = null; string[] strSplLng = null;
                    strSplLat = strLat.Split(' ');
                    strSplLng = strLng.Split(' ');
                    strSplLat[0] = strSplLat[0].Substring(0, strSplLat[0].Length - 1);
                    strSplLat[1] = strSplLat[1].Substring(0, strSplLat[1].Length - 1);
                    degLat = Convert.ToDecimal(strSplLat[0]); minutLat = Convert.ToDecimal(strSplLat[1]); secondLat = Convert.ToDecimal(strSplLat[2]);
                    degLat = degLat + (minutLat / 60) + (secondLat / 3600);
                    strSplLng[0] = strSplLng[0].Substring(0, strSplLng[0].Length - 1);
                    strSplLng[1] = strSplLng[1].Substring(0, strSplLng[1].Length - 1);

                    degLng = Convert.ToDecimal(strSplLng[0]); minutLng = Convert.ToDecimal(strSplLng[1]); secondLng = Convert.ToDecimal(strSplLng[2]);

                    degLng = degLng + (minutLng / 60) + (secondLng / 3600);

                    string strLn = degLng.ToString("#.#####", CultureInfo.InvariantCulture); //This is OK
                    string strLt = degLat.ToString("#.#####", CultureInfo.InvariantCulture); //This is OK
                    longLat = "" + strLn + "," + strLt;
                }
                else
                {
                    longLat = "" + obj.Longitude + "," + obj.Latitude;
                }



                //////string longLat = @"""76.8976,24.87765""";

                ////// string sLatLong = string.Format("""Cordinates""":'{0},{1}'", "76.8976", "24.87765");


                ApplicationJson jsono = new ApplicationJson { Cordinates = longLat };


                //////string longLat = @""""+ strLn + "," + strLt;
                //////string DATA = @"{""Cordinates"":""76.8976,24.87765""}";
                //////string DATA = sLatLong; //@"{""Cordinates"":"+ longLat + "}";

                ////jsono = new ApplicationJson { Cordinates = "76.8976,24.87765" }; //Correct Format working
                string jsonData = JsonConvert.SerializeObject(jsono);


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = jsonData.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(jsonData);
                requestWriter.Close();
                try
                {
                    WebResponse webResponse = request.GetResponse();
                    Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webStream);
                    string response = responseReader.ReadToEnd();
                    List<GISDataModel> objList = new List<GISDataModel>();
                    objList = JsonConvert.DeserializeObject<List<GISDataModel>>(response);
                    GISDataModel GisObj = new GISDataModel();
                    GisObj = objList.ToList().FirstOrDefault();
                    TotalRecordUpdated += UpdateData(GisObj, obj.ID);
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                    Console.WriteLine("Process Started....");
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Process Completed");
            return "Process Complete and Total Record Updated : " + TotalRecordUpdated;
        }
        private int UpdateData(GISDataModel GisObj, int ID)
        {
            int res = 0;
            try
            {
                if (GisObj != null && ID > 0)
                {
                    DataTable dt = new DataTable();
                    var prms = new[]{
                            new SqlParameter("@Action_Name", "Update_ForestFireExcelData"),
                            new SqlParameter("@ID", ID),
                            new SqlParameter("@District", GisObj.DISTRICT_NAME_EN),
                            new SqlParameter("@DivisionCode", GisObj.DIVISION_CODE),
                            new SqlParameter("@ForestDivision_Code", GisObj.FOREST_DIVCODE),
                            new SqlParameter("@ForestRange_Code", GisObj.FOREST_RANGECODE),
                            new SqlParameter("@BLOCK_NAME", GisObj.BLOCK_NAME),
                            new SqlParameter("@Block", GisObj.BLOCK_CODE),
                            new SqlParameter("@GP_FINAL", GisObj.GP_FINAL),
                            new SqlParameter("@GP_FINAL_CODE", GisObj.GP_FINAL_CODE),
                            new SqlParameter("@CENSUS_NM",  GisObj.CENSUS_NM_2011),
                            new SqlParameter("@CENSUS_CD", (GisObj.LCENSUS_CD_2011 == null ? "" : GisObj.LCENSUS_CD_2011)),

                    };
                    _db.Fill(dt, "spUpdateForestFireExcelData", prms);
                    if (dt.Rows[0][0].ToString().Contains("Successfull") == true)
                        res = 1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }
        private List<DBExeclDataModel> GetDataList()
        {
            DataTable dsResult = new DataTable();
            List<DBExeclDataModel> Lst = new List<DBExeclDataModel>();
            DataTable dt = new DataTable();

            try
            {
                Console.WriteLine("Data List Fatching Started...");
                var prms = new[]{
                            new SqlParameter("@Action_Name", "Get_ForestFireExcelData"),
                    };
                _db.Fill(dsResult, "spUpdateForestFireExcelData", prms);


                for (int i = 0; i < dsResult.Rows.Count; i++)
                {
                    //ID Latitude    Longitude

                    DBExeclDataModel obj = new DBExeclDataModel();
                    obj.ID = Convert.ToInt32(dsResult.Rows[i]["ID"]);
                    obj.Latitude = Convert.ToString(dsResult.Rows[i]["Latitude"]);
                    obj.Longitude = Convert.ToString(dsResult.Rows[i]["Longitude"]);
                    Lst.Add(obj);
                }
                Console.WriteLine("Data List Fatching Completed.");
                return Lst;

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
            }
            return Lst;
        }

        private class DBExeclDataModel
        {
            public int ID { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string District { get; set; }
            public string Circle { get; set; }
            public string Division_Code { get; set; }

            public string Range_Code { get; set; }
            public string BLOCK_NAME { get; set; }
            public string Block { get; set; }
            public string GP_FINAL { get; set; }
            public string GP_FINAL_CODE { get; set; }
            public string CENSUS_NM { get; set; }
            public string CENSUS_CD { get; set; }
            public bool GIS_StatusUpdate { get; set; }
            public string GIS_Date { get; set; }
        }
        private class GISDataModel
        {
            public string DIVISION_NAME { get; set; }
            public string DIVISION_CODE { get; set; }
            public string DISTRICT_NAME_EN { get; set; }
            public string DISTRICT_CODE { get; set; }
            public string BLOCK_NAME { get; set; }
            public string BLOCK_CODE { get; set; }
            public string GP_FINAL { get; set; }
            public string GP_FINAL_CODE { get; set; }
            public string CENSUS_NM_2011 { get; set; }
            public string LCENSUS_CD_2011 { get; set; }
            public string FOREST_DIVCODE { get; set; }
            public string FOREST_RANGECODE { get; set; }
            public string Cordinates { get; set; }
            public string MSG { get; set; }
        }
        private class ApplicationJson
        {
            public string Cordinates { get; set; }

        }
        #endregion

        #region Update Forest fire data from API
        // create by amrit barotia on 14-06-2021
        public async System.Threading.Tasks.Task<string> UpdateForestFireDataFromAPI()
        {
            string Url = "http://117.239.115.41/smsalerts/ws/get-fire-points.php";
            var bodyParams = new
            {
                fromdate = DateTime.Now.ToString("yyyy-MM-dd"),
                todate = DateTime.Now.ToString("yyyy-MM-dd"),
                //fromdate = "2022-12-07",
                //todate = "2022-12-14",
                sourcetype = "",
                filters = new { district = "", circle = "", division = "", rangename = "", block = "", beat = "" }
            };
            string returnMsg = string.Empty;
            int fatchRecods;
            try
            {
                string jsonData = JsonConvert.SerializeObject(bodyParams);
                JsonResult jr = new JsonResult();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Headers.Add("Apikey", "92b5211f7771d628afb5b3adff77783b34e4af1e81ce2f270d5933ec5380146d");
                request.Headers.Add("hashKey", "OU0mekZTaVNITzIwMjEvUkFKQVNUSEFO");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = jsonData.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(jsonData);
                requestWriter.Close();

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                responseReader.Close();
                try
                {
                    ForestFireHeadDataAPI forestFireHeadDataAPI = JsonConvert.DeserializeObject<ForestFireHeadDataAPI>(response);
                    if (forestFireHeadDataAPI.error == "0")
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ApiExecDate", typeof(DateTime));
                        dt.Columns.Add("Sno", typeof(int));
                        dt.Columns.Add("FireDate", typeof(DateTime));
                        dt.Columns.Add("FireTime", typeof(DateTime));
                        dt.Columns.Add("SourceType", typeof(string));
                        dt.Columns.Add("Longitude", typeof(string));
                        dt.Columns.Add("Latitude", typeof(string));
                        dt.Columns.Add("LongDeg", typeof(string));
                        dt.Columns.Add("LatDeg", typeof(string));
                        dt.Columns.Add("State", typeof(string));
                        dt.Columns.Add("District", typeof(string));
                        dt.Columns.Add("Circle", typeof(string));
                        dt.Columns.Add("Division", typeof(string));
                        dt.Columns.Add("RangeName", typeof(string));
                        dt.Columns.Add("Block", typeof(string));
                        dt.Columns.Add("Beat", typeof(string));
                        dt.Columns.Add("ForestBlock", typeof(string));
                        dt.Columns.Add("CompartmentNo", typeof(string));
                        dt.Columns.Add("TimeStamp", typeof(DateTime));
                        foreach (var rValues in forestFireHeadDataAPI.data.Values)
                        {
                            DataRow dr = dt.NewRow();
                            dr["ApiExecDate"] = DateTime.Now.Date;
                            dr["Sno"] = Convert.ToInt32(rValues.sno);
                            dr["FireDate"] = Convert.ToDateTime(rValues.firedate);
                            dr["FireTime"] = Convert.ToDateTime(rValues.firedate).ToString("yyyy-MM-dd " + rValues.firetime);
                            dr["SourceType"] = rValues.sourcetype;
                            dr["Latitude"] = rValues.latitude;
                            dr["Longitude"] = rValues.longitude;
                            dr["LongDeg"] = rValues.longdeg;
                            dr["LatDeg"] = rValues.latdeg;
                            dr["State"] = rValues.state;
                            dr["District"] = rValues.district;
                            dr["Circle"] = rValues.circle;
                            dr["Division"] = rValues.division;
                            dr["RangeName"] = rValues.rangename;
                            dr["Block"] = rValues.block;
                            dr["Beat"] = rValues.beat;
                            dr["ForestBlock"] = rValues.forestBlock;
                            dr["CompartmentNo"] = rValues.compartmentNo;
                            dr["TimeStamp"] = Convert.ToDateTime(rValues.timestamp);
                            dt.Rows.Add(dr);
                        }
                        dt.AcceptChanges();
                        //DataTable distinctDT = dt.DefaultView.ToTable(true, "Fire_date", "Fire_Time", "Latitude", "Longitude");

                        SqlConnection cnn = new SqlConnection();
                        SqlDataReader reader;
                        cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                        cnn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_ForestFireExcelData";
                        cmd.Connection = cnn;
                        cmd.Parameters.AddWithValue("@ActionCode", 7);
                        cmd.Parameters.Add("@Typ_ForestFireAPIRawData", SqlDbType.Structured).SqlValue = dt;
                        //string result = cmd.ExecuteNonQuery().ToString();
                        bool isError = false;
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            isError = Convert.ToBoolean(reader["IsError"]);
                            string ReturnMessage = Convert.ToString(reader["ReturnMessage"]);
                            //TempData["msg1"] = ReturnMessage;
                            //TempData["isError"] = isError;
                        }
                        reader.Close();
                        if (isError == false)
                        {
                            var msg = UpdateForesFireDataFromGISAsync();
                        }
                    }
                    fatchRecods = Convert.ToInt32(forestFireHeadDataAPI.totalRecords);
                    returnMsg = "Record Updated";
                }
                catch (Exception ex)
                {

                    returnMsg = "Record not found";
                    fatchRecods = 0;
                }
                new Common().ForestFireApiLogs(DateTime.Now, fatchRecods);

            }
            catch (Exception ex)
            {
                returnMsg = "Error";
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "ForestFireData_Update_FromAPI", 0, DateTime.Now, 0);
            }


            return returnMsg;
        }


        #endregion
    }
}