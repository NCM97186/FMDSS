using FMDSS.APIRepo;
using FMDSS.CustomModels.Models;
using FMDSS.Entity;
using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Models.Admin;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class Offence_APIController : ApiController
    {
        OffenceDetails _objModel = new OffenceDetails();
        private FMDSS.Models.DAL _db = new Models.DAL();
        private ICommonRepository _commonRepository;
        private IProtectionRepository _ProtectionRepository;
        public Offence_APIController()
        {
            _commonRepository = new CommonRepository();
            _ProtectionRepository = new ProtectionRepository();
        }

        [HttpGet]
        public DataTableResponse GetCircle()
        {
            DataTableResponse response = new DataTableResponse();
            Location cs = new Location();
            try
            {
                var data = _ProtectionRepository.GetCircle();
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = data };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpGet]
        public DataSetResponse GetRange_OffenceCategoryData(long UserID)
        {
            DataSetResponse response = new DataSetResponse();
            Location cs = new Location();
            try
            {
                System.Web.HttpContext.Current.Session["UserID"] = UserID;
                _objModel.UserID = UserID;
                var data = _ProtectionRepository.OffenceDetails_GetDropdownDataAPI();
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = data };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetNakaData(string parentID, long UserID)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                System.Web.HttpContext.Current.Session["UserId"] = UserID;
                DataTable dtDropdownData = new DataTable();
                DataTable Naka_Data = new DataTable();
                Naka_Data = GetNaka(8, parentID, UserID);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = Naka_Data };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        public DataTable GetNaka(int actionCode, string parentID, long UserID)
        {
            DataTable data = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("UserID",  UserID)};
                _db.Fill(data, "SP_GetCommonData", prms);
            }
            catch (Exception ex) { }
            data.TableName = "Naka";
            return data;
        }

        [HttpPost]
        public ResponseMsg SubmitOffenceDetails(OffenceDetailsModel_API model)
        {
            ResponseMsg response = new ResponseMsg();
            string docSaveResponseMsg="";
            if (model != null)
            {
                try
                {
                    //if (System.Web.HttpContext.Current.Session["UploadFile"] == null)
                    //{
                    //    response = new ResponseMsg { IsError = true, ReturnMsg = "Please Upload Documents.", ReturnIDs = "0" };
                    //}
                    //else
                    //{
                        bool isError= SaveDocs(model,out docSaveResponseMsg);
                        var msg = _ProtectionRepository.OffenceDetails_SaveAPI(model,1);

                    //if (msg.IsError==true)
                    //{
                    //    System.Web.HttpContext.Current.Session["UploadFile"] = null;
                    //}

                    msg.ReturnMsg = msg.ReturnMsg; // +" "+ docSaveResponseMsg;
                    response = new ResponseMsg { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg, ReturnIDs = "0" };

                    //}
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Session["UploadFile"] = null;
                    response = new ResponseMsg { IsError = true, ReturnMsg = ex.Message, ReturnIDs = "0" };
                }
            }
            else
            {
                response = new ResponseMsg { IsError = true, ReturnMsg = "Model have some data missing.", ReturnIDs = "0" };
            }
            return response;
        }
        private bool SaveDocs(OffenceDetailsModel_API MainModel,out string ResponseMsg)
        {
            ResponseMsg = "";
            bool IsError = false;
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
           // string FilePath = Util.GetAppSettings("TempDocumentPath");
            string FilePath = Util.GetAppSettings("FRADocuments");
            
            List<CommonDocumentAPI> lstDoc = new List<CommonDocumentAPI>();
            ResponseMsg response = new ResponseMsg();
            foreach (var model in MainModel.DocList)
            {
                if (!string.IsNullOrWhiteSpace(model.UploadDocumnet) && !string.IsNullOrEmpty(model.UploadDocumnet) && model.docTypeID != 0)
                {
                    try
                    {
                        string src = model.UploadDocumnet;
                        var docType = Util.GetListFromTable<CommonDocumentTypeAPI>(_commonRepository.GetDocumentType(model.docTypeID)).FirstOrDefault();
                        FileFullName = DateTime.Now.Ticks + "_" + model.docTypeID + "_OffenceDoc" + "." + GetFileExtensionFromBase64(model.UploadDocumnet);
                        string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                        path = FilePath + FileFullName;
                        string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(model.UploadDocumnet));
                        //if (System.Web.HttpContext.Current.Session["UploadFile"] == null)
                        //{
                        lstDoc.Add(new CommonDocumentAPI
                        {
                            TempID = Guid.NewGuid().ToString(),
                            IsNew = true,
                            DocumentName = FileFullName,
                            DocumentPath = path,
                            DocumentTypeID = model.docTypeID,
                            DocumentLevel = docType.DocumentLevel,
                            DocumentTypeName = docType.DocumentTypeName
                        });
                        //    ///  System.Web.HttpContext.Current.Session["UploadFile"] = lstDoc;

                        //}
                        //else
                        //{
                        //    lstDoc = (List<CommonDocumentAPI>)System.Web.HttpContext.Current.Session["UploadFile"];
                        //    lstDoc.Add(new CommonDocumentAPI
                        //    {
                        //        TempID = Guid.NewGuid().ToString(),
                        //        IsNew = true,
                        //        DocumentName = FileFullName,
                        //        DocumentPath = path,
                        //        DocumentTypeID = model.docTypeID,
                        //        DocumentLevel = docType.DocumentLevel,
                        //        DocumentTypeName = docType.DocumentTypeName
                        //    });
                        //    System.Web.HttpContext.Current.Session["UploadFile"] = lstDoc;
                        //}

                        //if (docType.DocumentLevel > 0)
                        //{
                        //    lstDoc = lstDoc.Where(x => x.DocumentTypeID == model.docTypeID).ToList();
                        //}
                        //else
                        //{
                        //    lstDoc = lstDoc.Where(x => x.DocumentLevel == 0).ToList();
                        //}
                        ResponseMsg = "Doument Sucessfully Uploaded";
                        //response = new ResponseMsg { IsError = false, ReturnMsg = "Sucessfully Upload", ReturnIDs = "0" };
                    }
                    catch (Exception ex)
                    {
                        ResponseMsg = ex.Message;
                        IsError = true;
                        //response = new ResponseMsg { IsError = true, ReturnMsg = ex.Message, ReturnIDs = "0" };
                    }
                }

                else
                {
                    ResponseMsg = "Documnet not attached or DocType is 0.";
                    IsError = true;
                    //response = new ResponseMsg { IsError = true, ReturnMsg = "Documnet not attached or DocType is 0.", ReturnIDs = "0" };
                }
            }
            MainModel.CommonDocApiList = lstDoc;
            return IsError;
        }
        [HttpPost]
        public ResponseMsg Upload_DocumentAPI(OffenceDocumentUploadAPI model)
        {
            
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = Util.GetAppSettings("TempDocumentPath");
            List<CommonDocumentAPI> lstDoc = new List<CommonDocumentAPI>();
            ResponseMsg response = new ResponseMsg();
            if (!string.IsNullOrWhiteSpace(model.UploadDocumnet) && !string.IsNullOrEmpty(model.UploadDocumnet) && model.docTypeID != 0)
            {
                try
                {
                    string src = model.UploadDocumnet;
                    var docType = Util.GetListFromTable<CommonDocumentTypeAPI>(_commonRepository.GetDocumentType(model.docTypeID)).FirstOrDefault();
                    FileFullName = DateTime.Now.Ticks + "_" + model.docTypeID + "_OffenceDoc" + "." + GetFileExtensionFromBase64(model.UploadDocumnet);
                    string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                    path = FilePath + FileFullName;
                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(model.UploadDocumnet));
                    if (System.Web.HttpContext.Current.Session["UploadFile"] == null)
                    {
                        lstDoc.Add(new CommonDocumentAPI
                        {
                            TempID = Guid.NewGuid().ToString(),
                            IsNew = true,
                            DocumentName = FileFullName,
                            DocumentPath = path,
                            DocumentTypeID = model.docTypeID,
                            DocumentLevel = docType.DocumentLevel,
                            DocumentTypeName = docType.DocumentTypeName
                        });
                        System.Web.HttpContext.Current.Session["UploadFile"] = lstDoc;
                        
                    }
                    else
                    {
                        lstDoc = (List<CommonDocumentAPI>)System.Web.HttpContext.Current.Session["UploadFile"];
                        lstDoc.Add(new CommonDocumentAPI
                        {
                            TempID = Guid.NewGuid().ToString(),
                            IsNew = true,
                            DocumentName = FileFullName,
                            DocumentPath = path,
                            DocumentTypeID = model.docTypeID,
                            DocumentLevel = docType.DocumentLevel,
                            DocumentTypeName = docType.DocumentTypeName
                        });
                        System.Web.HttpContext.Current.Session["UploadFile"] = lstDoc;
                    }

                    if (docType.DocumentLevel > 0)
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentTypeID == model.docTypeID).ToList();
                    }
                    else
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentLevel == 0).ToList();
                    }
                    response = new ResponseMsg { IsError = false, ReturnMsg = "Sucessfully Upload", ReturnIDs = "0" };
                }
                catch (Exception ex)
                {
                    response = new ResponseMsg { IsError = true, ReturnMsg = ex.Message, ReturnIDs = "0" };
                }
            }
            else
            {
                response = new ResponseMsg { IsError = true, ReturnMsg = "Documnet not attached or DocType is 0.", ReturnIDs = "0" };
            }
            return response;
        }

        [HttpGet]
        public DataSetResponse GetAllOffenceDetails(long UserID, int PageNumber, int PageSize, string circelcode, string divcode, string rangecode, long nakaid)
            {
            DataSetResponse response = new DataSetResponse();
            try
            {
                System.Web.HttpContext.Current.Session["UserId"] = UserID;
                var data = GetOffenceDetails(UserID, PageNumber, PageSize, circelcode, divcode, rangecode, nakaid);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = data };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse GetOffenceSummery(long UserID, int PageNumber, int PageSize, string circelcode, string divcode, string rangecode, long nakaid,int FYearId)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                System.Web.HttpContext.Current.Session["UserId"] = UserID;
                var data = GetOffenceSummerys(UserID, PageNumber, PageSize, circelcode, divcode, rangecode, nakaid,FYearId);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = data };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSet GetAllOffenceDetails(int PageNumber, int PageSize)
        {
            DataSet ds = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", System.Web.HttpContext.Current.Session["UserId"]),
                            new SqlParameter("PageNumber", PageNumber),
            new SqlParameter("PageSize", PageSize)
            };
            _db.Fill(ds, "SP_PM_OffenceDetails_Get", prms);
            ds.Tables[0].TableName = "OffenceDetails";
            ds.Tables[1].TableName = "EditReqInOffencePage";
            return ds;
        }

        public DataSet GetOffenceDetails(long UserId, int PageNumber, int PageSize, string circelcode, string divcode, string rangecode, long nakaid)
        {
            DataSet ds = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 8),
                            new SqlParameter("UserID", UserId),
                            new SqlParameter("PageNumber", PageNumber),
                            new SqlParameter("PageSize", PageSize),
                            new SqlParameter("CircelCode", circelcode),
                            new SqlParameter("DivCode", divcode),
                            new SqlParameter("RangeCode", rangecode),
                            new SqlParameter("NakaId", nakaid),            
            };
            _db.Fill(ds, "SP_PM_OffenceDetails_Get", prms);
            ds.Tables[0].TableName = "OffenceDetails";
            ds.Tables[1].TableName = "EditReqInOffencePage";
           // ds.Tables[2].TableName = "GetOffenceCount";
            return ds;
        }
        public DataSet GetOffenceSummerys(long UserId, int PageNumber, int PageSize, string circelcode, string divcode, string rangecode, long nakaid,int FYearId)
        {
            DataSet ds = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 10),
                            new SqlParameter("UserID", UserId),
                            new SqlParameter("PageNumber", PageNumber),
                            new SqlParameter("PageSize", PageSize),
                            new SqlParameter("CircelCode", circelcode),
                            new SqlParameter("DivCode", divcode),
                            new SqlParameter("RangeCode", rangecode),
                            new SqlParameter("NakaId", nakaid),
                            new SqlParameter("FYearId ", FYearId),
            };
            _db.Fill(ds, "SP_PM_OffenceDetails_Get", prms);
            ds.Tables[0].TableName = "OffenceDetails";
            ds.Tables[1].TableName = "EditReqInOffencePage";
            //ds.Tables[1].TableName = "EditReqInOffencePage";
            // ds.Tables[2].TableName = "GetOffenceCount";
            return ds;
        }
        public static string GetFileExtensionFromBase64(string base64String)
        {
            var data = base64String.Substring(0, 39);


            if (data.ToUpper() == "IVBOR" || data.ToUpper().Contains("IVBOR"))
            {
                return "png";
            }
            else if (data.ToUpper() == "/9J/4" || data.ToUpper().Contains("/9J/4"))
            {
                return "jpg";
            }
            else if (data.ToUpper() == "JVBER" || data.ToUpper().Contains("JVBER"))
            {
                return "pdf";
            }
            //switch (data.ToUpper())
            //{
            //    case "IVBOR":
            //        return "png";
            //    case "/9J/4":
            //        return "jpg";
            //    case "AAAAF":
            //        return "mp4";
            //    case "JVBER":
            //        return "pdf";
            //    case "AAABA":
            //        return "ico";
            //    case "UMFYI":
            //        return "rar";
            //    case "E1XYD":
            //        return "rtf";
            //    case "U1PKC":
            //        return "txt";
            //    case "MQOWM":
            //    case "77U/M":
            //        return "srt";
            //    default:
            //        return string.Empty;
            //}
            else
            {
                return string.Empty;
            }

        }
        private string SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            try
            {
                if (base64String.IndexOf(',') > 0)
                {
                    string[] arr = base64String.Split(',');
                    base64String = arr[1];
                }

                byte[] bytes = Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(fullOutputPath, bytes);
                return fullOutputPath;
            }
            catch (Exception ex)
            {

            }
            return fullOutputPath;
        }
        [HttpGet]
        public DataSetResponse GetAllOffenceDetailsCount(long UserID, string circelcode, string divcode, string rangecode, long nakaid)
        {
            DataSetResponse response = new DataSetResponse();
            try
            {
                System.Web.HttpContext.Current.Session["UserId"] = UserID;
                var data = GetOffenceDetailsCount(circelcode, divcode, rangecode, nakaid);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = data };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSet GetOffenceDetailsCount(string circelcode, string divcode, string rangecode, long nakaid)
        {
            DataSet ds = new DataSet();
            var prms = new[]{
                            new SqlParameter("CircelCode", circelcode),
                            new SqlParameter("DivCode", divcode),
                            new SqlParameter("RangeCode", rangecode),
                            new SqlParameter("NakaId", nakaid),

            };
            _db.Fill(ds, "SP_PM_OffenceDetails_GetCount", prms);
            ds.Tables[0].TableName = "OffenceDetailsCount";
            return ds;
        }
        [HttpGet]
        public DataSet GetOffenceAllCount(long UserID,string CircleCode, string DivisionCode, string RangeCode,long NakaId,int FYearId, string OffenceId="0")
        {
           
            
            DataSet ds = new DataSet();
            DataSet dsFiDate = new DataSet();
            var prms = new[]{
                            new SqlParameter("UserID", UserID),
                            new SqlParameter("CircleCode", CircleCode),
                            new SqlParameter("DivisionCode", DivisionCode),
                            new SqlParameter("RangeCode", RangeCode),
                            new SqlParameter("NakaId", NakaId),
                            new SqlParameter("FYearId", FYearId),
                            new SqlParameter("StrOffenceCat", OffenceId)                          
            };
            _db.Fill(ds, "SP_PM_OffenceAllType_GetCount", prms);
            ds.Tables[0].TableName = "OffenceAllCount";
          
            var prms2 = new[] { new SqlParameter("IsCurrent", false), new SqlParameter("Year", FYearId) };
            _db.Fill(dsFiDate, "spGetFinancialYearStartEndDate", prms2);
            dsFiDate.Tables[0].TableName = "FYearDate";
            string fromDate = ""; string toDate = "";
           
            if (FYearId>0)
            {               
                fromDate = dsFiDate.Tables[0].Rows[0][0].ToString();
                toDate = dsFiDate.Tables[0].Rows[0][1].ToString();
            }
            else
            {
                fromDate = "1969/04/01";
                toDate =   DateTime.Now.Date.Year+ "/03/31";
            }

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            if (!string.IsNullOrEmpty(fromDate))
            {
                FromDate = Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                ToDate = Convert.ToDateTime(toDate);
            }

            string ModuleId = "";  string type = ""; string parentID = ""; string status = "0,1,16,17";
            if (CircleCode.Trim() == "0" && DivisionCode.Trim() == "0" && RangeCode.Trim() == "0" && NakaId == 0)
                ModuleId = "1";
            else if (CircleCode.Trim() != "0" && DivisionCode.Trim() == "0" && RangeCode.Trim() == "0" && NakaId == 0)
            {
                ModuleId = "Offence";
                parentID = CircleCode;
                type = "OffenceDivisionList";
            }                
            else if (CircleCode.Trim() != "0" && DivisionCode.Trim() != "0" && RangeCode.Trim() == "0" && NakaId == 0)
            {
                ModuleId = "Offence";
                parentID = DivisionCode;
                type = "OffenceRange";
            }
            
            else if (CircleCode.Trim() != "0" && DivisionCode.Trim() != "0" && RangeCode.Trim() != "0" && NakaId == 0)
            {
                ModuleId = "Offence";
                parentID = RangeCode;
                type = "OffenceNaka";
            }
            else if (CircleCode.Trim() != "0" && DivisionCode.Trim() != "0" && RangeCode.Trim() != "0" && NakaId > 0)
            {
                ModuleId = "Offence";
                parentID = ""+NakaId;
                type = "OffenceListByDivision";
            }
            if (ModuleId == "1")
            {
                clsForestDashboard FD = new Models.Admin.clsForestDashboard();
                List<CircleWise> objCircleWise = FD.GetDataForDashboard(ModuleId, OffenceId == "0" ? "" : OffenceId,null, FromDate, ToDate, UserID);

                //DataTable dataTable = Globals.Util.ToDataTable(objCircleWise);
                var json = JsonConvert.SerializeObject(objCircleWise);
                DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));               
                dataTable.TableName = "CircleWiseList";

                ds.Tables.Add(dataTable);                             
            }
            else
            {
                List<DashboardRange> listRange = new List<DashboardRange>();
                var  dlist = new clsForestDashboard().GetDataForDashboard(ModuleId, parentID, type, status, FromDate, ToDate, Convert.ToInt32(OffenceId), UserID);
                //DataTable dataTable = Globals.Util.ToDataTable(data);
                //dataTable.TableName = type;
                //List<dynamic> dlist = new List<dynamic>
                var json = JsonConvert.SerializeObject(dlist);
                DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                dataTable.TableName = type;
                ds.Tables.Add(dataTable);
            }

            //dynamic offenceList = _ProtectionRepository.OffenceDetails_GetDropdownData();
            //DataTable offenceDTLst = Globals.Util.ToDataTable(offenceList);
            //offenceDTLst.TableName = "OffenceList";
            //ds.Tables.Add(offenceDTLst);

            return ds;
        }

        [HttpGet]  /////amit change
        public DataSet GetCircleDivRangNakaWiseList(string ActionnName,string CircleCode,string DivCode,string RangCode, long UserID)
        {
            DataSet ds = new DataSet();
            var prms = new[]{
                            new SqlParameter("Action", ActionnName),
                            new SqlParameter("CircleCode", CircleCode),                            
                            new SqlParameter("DivCode", DivCode),
                            new SqlParameter("RangCode", RangCode),
                            new SqlParameter("UserID", UserID)                            
            };
            _db.Fill(ds, "spCircleDivRangNakaWiseCount", prms);
            ds.Tables[0].TableName = "DetailList";
            ds.Tables[1].TableName = "TotalCount";            
            return ds;
        }              
    }
}