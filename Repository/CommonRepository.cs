using FMDSS.Entity;
using FMDSS.Entity.ViewModel;
using FMDSS.Globals;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Repository
{
    public class CommonRepository : ICommonRepository
    {
        #region Properties & Variables
        private FMDSS.Models.DAL _db = new Models.DAL();
        #endregion

        #region Public Methods
        public EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, string parentID = null)
        {
            EnumerableRowCollection<SelectListItem> data = null;
            DataTable dtDropdownData = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", actionCode), 
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("UserID",  HttpContext.Current.Session["UserId"])};
                _db.Fill(dtDropdownData, "SP_GetCommonData", prms);
                data = GetDropdownData(actionCode, dtDropdownData);
            }
            catch (Exception ex) { }
            return data;
        }

        public DataSet GetDropdownData2(int actionCode, string parentID = null)
        {
            DataSet dsDropdownData = new DataSet();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("UserID",  HttpContext.Current.Session["UserId"])};
                _db.Fill(dsDropdownData, "SP_GetCommonData", prms);
            }
            catch (Exception ex) { }
            return dsDropdownData;
        }

        public DataTable GetDropdownData3(int actionCode, string parentID = null)
        {
            DataTable dtDropdownData = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("UserID",  HttpContext.Current.Session["UserId"])};
                _db.Fill(dtDropdownData, "SP_GetCommonData", prms);
            }
            catch (Exception ex) { }
            return dtDropdownData;
        }

        public List<CommonDocument> GetAttachedDocument(long? objectID, int objectTypeID)
        {
            var prms = new[]{ new SqlParameter("ActionCode", 1),
                              new SqlParameter("ObjectID", objectID),
                               new SqlParameter("ObjectTypeID", objectTypeID)};
            DataSet ds = new DataSet();
            _db.Fill(ds, "SP_Common_Document_Get", prms);
            return Util.GetListFromTable<CommonDocument>(ds, 0);
        }

        public DataTable GetDocumentType(int docTypeID)
        {
            DataTable dt = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("DocTypeID", docTypeID),
                            new SqlParameter("UserID",  HttpContext.Current.Session["UserId"])};
                _db.Fill(dt, "SP_Common_GetDocumentType", prms);
            }
            catch (Exception ex) { }
            return dt;
        }

        public DataTable GetServiceDetails(string serviceName)
        {
            DataTable dt = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", serviceName), 
                            new SqlParameter("UserID",  HttpContext.Current.Session["UserId"])};
                _db.Fill(dt, "SP_Common_PaymentServices_Get", prms);
            }
            catch (Exception ex) { }
            return dt;
        }

        public string GetTempDocInXML()
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
                            <IsESign>{6}</IsESign>", item.DocumentID, item.ObjectTypeID, item.ObjectID, item.DocumentTypeID, item.DocumentName, item.DocumentPath, item.IsESign));
                    sb.Append("</document>");
                }
                sb.Append("</documents>");
            }

            sb.Append("</docData>");
            return Convert.ToString(sb);
        }

        public ResponseMsg SaveFile(HttpPostedFileBase postedFile, int objectTypeID, long objectID, int documentTypeID, string targetDirectoryPath)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = false;
            try
            {
                if (postedFile != null)
                {
                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }

                    if (postedFile != null)
                    {
                        string documentName = Path.GetFileName(postedFile.FileName);
                        postedFile.SaveAs(string.Format("{0}{1}_{2}_{3}_{4}", targetDirectoryPath, objectTypeID, objectID, documentTypeID, documentName));
                        msg.ReturnMsg = Constant.SaveMsg;
                    }
                }
            }
            catch (Exception ex) { msg.IsError = true; msg.ReturnMsg = ex.Message; }
            return msg;
        }

        public ResponseMsg SaveFile(List<HttpPostedFileBase> postedFiles, int objectTypeID, long objectID, int documentTypeID, string targetDirectoryPath)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = false;
            try
            {
                if (postedFiles != null)
                {
                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.CreateDirectory(targetDirectoryPath);
                    }

                    foreach (var postedFile in postedFiles)
                    {
                        if (postedFile != null)
                        {
                            string documentName = Path.GetFileName(postedFile.FileName);
                            postedFile.SaveAs(string.Format("{0}{1}_{2}_{3}_{4}", Util.GetAppSettings("FRADocumentPath"), objectTypeID, objectID, documentTypeID, documentName));
                        }
                    }
                    msg.ReturnMsg = Constant.SaveMsg;
                }
            }
            catch (Exception ex) { msg.IsError = true; msg.ReturnMsg = ex.Message; }
            return msg;
        }

        public void SaveDocs(long objectID, int objectTypeID, List<CommonDocument> docs)
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
        #endregion

        #region Private Methods
        private EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, DataTable dtDropdownData)
        {
            EnumerableRowCollection<SelectListItem> data = null;
            switch (actionCode)
            {
                case 1://For Circle
                case 4://For Circle User Wise
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("Circle_Code"),
                        Text = x.Field<string>("CIRCLE_NAME")
                    });
                    return data;
                case 2://For Division 
                case 5://For Division User wise  

                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("DIV_CODE"),
                        Text = x.Field<string>("DIV_NAME")
                    });
                    return data;
                case 3://For Range by div
                case 6://For Range User wise by div 
                case 7://For Range User wise  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("RANGE_CODE"),
                        Text = x.Field<string>("RANGE_NAME")
                    });
                    return data;
                case 8://For Naka  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("NakaID")),
                        Text = x.Field<string>("NakaName")
                    });
                    return data;
                case 9://For Tehsil  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("TehsilID")),
                        Text = x.Field<string>("TehsilName")
                    });
                    return data;
                case 10://For block  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("BlockID")),
                        Text = x.Field<string>("BlockName")
                    });
                    return data;
                case 11://For GP  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("GPID")),
                        Text = x.Field<string>("GPName")
                    });
                    return data;
                case 12://For Village  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("VillageCode"),
                        Text = x.Field<string>("VillageName")
                    });
                    return data;
                case 13://For Financial Year  
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("FinancialYearID")),
                        Text = x.Field<string>("FinancialYearName")
                    });
                    return data;
            }
            return null;
        }
        #endregion
    }
}