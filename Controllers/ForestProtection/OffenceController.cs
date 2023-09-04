using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Globals;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForestProtection
{
    public class OffenceController : BaseController
    {
        #region Properties & Variables
        int ModuleID = 4;
        OffenceDetails _objModel = new OffenceDetails();
        private ICommonRepository _commonRepository;
        private IProtectionRepository _ProtectionRepository;
        private Int64 UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return Convert.ToInt64(Session["UserID"].ToString());
                else
                    return 0;
            }
        }
        #endregion

        #region [Constructor]
        public OffenceController()
        {
            _commonRepository = new CommonRepository();
            _ProtectionRepository = new ProtectionRepository();
        }
        #endregion

        #region Action Methods

        public ActionResult ShowOffenceDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var data = _ProtectionRepository.OffenceDetails_GetPermission();
                ViewBag.IsEditReq = data.Tables[0].Rows[0]["IsEditReqInOffencePage"];
                ViewBag.IsEditForNE = data.Tables[0].Rows[0]["IsEditForNE"];                
                ViewBag.IsUploadRequired = data.Tables[0].Rows[0]["IsUploadRequired"];
                ViewBag.IsDeleteRequired = data.Tables[0].Rows[0]["IsDeleteRequired"];
                return View(new List<ViewOffenceDetails>());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return View();
        }
       
        public string ShowOffenceDetails_Pager(string sEcho, int iDisplayStart, int iDisplayLength, string sSearch,string FDate, string TDate)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            { 
                var data = _ProtectionRepository.OffenceDetails_Get((iDisplayStart / iDisplayLength) + 1, iDisplayLength, FDate, TDate);
                var oDetails = Util.GetListFromTable<ViewOffenceDetails>(data, 0);
                var totalRecord = oDetails.Count > 0 ? oDetails.FirstOrDefault().TotalRows : 0;

                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("{");
                sb.Append("\"sEcho\": ");
                sb.Append(sEcho);
                sb.Append(",");
                sb.Append("\"iTotalRecords\": ");
                sb.Append(totalRecord);
                sb.Append(",");
                sb.Append("\"iTotalDisplayRecords\": ");
                sb.Append(totalRecord);
                sb.Append(",");
                sb.Append("\"aaData\": ");
                sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(oDetails));
                sb.Append("}");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return string.Empty;
        }

        public ActionResult AddOffenceDetails()
        {
            OffenceDetailsModel model = new OffenceDetailsModel();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            try
            {
                Session["UploadFile"] = null; 
                SetDropdownData();

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddOffenceDetails(OffenceDetailsModel model)
        {
            //long idBack = model.ID;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var msg = _ProtectionRepository.OffenceDetails_Save(model, 0);

                if (!msg.IsError)
                {
                    Session["UploadFile"] = null;
                }

                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                //if (idBack == 0)
                //    return RedirectToAction("AddOffenceDetails");
                //else
                    return RedirectToAction("ShowOffenceDetails");
            }
            catch (Exception ex)
            {
                TempData["ReturnMsg"] = ex.Message;
                TempData["IsError"] = true;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return RedirectToAction("AddOffenceDetails");
        }

        public ActionResult EditOffenceDetails(long offenceID)
        {
            OffenceDetailsModel model = new OffenceDetailsModel();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                Session["UploadFile"] = null;
                SetDropdownData();

                var data = _ProtectionRepository.OffenceDetails_GetDetailsForUpdation(offenceID); 
                model=Util.GetListFromTable<OffenceDetailsModel>(data, 0).FirstOrDefault();
                model.SeizedItemsList = Util.GetListFromTable<SeizedItemsModel>(data, 1);

                model.FIRDate = Convert.ToDateTime(model.FIRDate).ToString("dd/MM/yyyy");
                model.CompoundingDate = Convert.ToDateTime(model.CompoundingDate).ToString("dd/MM/yyyy");
                model.FileDate = Convert.ToDateTime(model.FileDate).ToString("dd/MM/yyyy");
                model.NextHearingDate = Convert.ToDateTime(model.NextHearingDate).ToString("dd/MM/yyyy");
                model.DateOfFinalReport = Convert.ToDateTime(model.DateOfFinalReport).ToString("dd/MM/yyyy");
                var cDocuments = Util.GetListFromTable<CommonDocument>(data, 2);

                foreach (var item in cDocuments)
                {
                    item.IsNew = false;
                    item.TempID = Guid.NewGuid().ToString();
                }

                Session["UploadFile"]= cDocuments;

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
                return View("AddOffenceDetails", model);
            }
            catch (Exception ex)
            {
                ViewBag.IsError = 1;
                ViewBag.ReturnMsg = ex.Message; 
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("AddOffenceDetails", model); 
        }

        [HttpPost]
        public ActionResult UpdateOffence(ApproverRemarks model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString(); 
            try
            {
                var msg = _ProtectionRepository.OffenceDetails_Update(model); 
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return Json(new { IsError = msg.IsError, returnMsg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        public ActionResult DeleteOffence(long offenceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var msg = _ProtectionRepository.OffenceDetails_Delete(offenceID);
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return RedirectToAction("ShowOffenceDetails");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
            return null;
        }

        public ActionResult UploadOffenceDetails()
        {
            UploadOffenceDetailsModel model = new UploadOffenceDetailsModel();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                Session["UploadFile"] = null;
                SetDropdownData();

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public ActionResult UploadOffenceDetails(UploadOffenceDetailsModel model)
        {
            Entity.ResponseMsg msg = new Entity.ResponseMsg();
            msg.IsError = false;

            try
            {
                if (model.FileUpload != null && model.FileUpload.ContentLength > 0)
                { 
                    var fileName = Path.GetFileName(model.FileUpload.FileName);
                    var folderLoc = Server.MapPath("~/App_Data/offence");
                    if (!Directory.Exists(folderLoc))
                    {
                        Directory.CreateDirectory(folderLoc);
                    }
                    var filepath = Path.Combine(folderLoc, fileName);
                    model.FileUpload.SaveAs(filepath);
                    DataSet dsExcel = new DataSet("offencedetails");
                    var OffenceDetails = Util.ImportDataFromExcel(filepath, "OffenceDetail", ref msg);

                    if (!msg.IsError && Util.isValidDataTable(OffenceDetails, true))
                    {
                        var tmpData = OffenceDetails.Copy();
                        dsExcel.Tables.Add(tmpData); tmpData = null;
                        var SeizedItemDetails = Util.ImportDataFromExcel(filepath, "SeizedItemDetail", ref msg);
                        if (Util.isValidDataTable(SeizedItemDetails, true))
                        {
                            tmpData = SeizedItemDetails.Copy();
                            dsExcel.Tables.Add(tmpData);
                        }

                        if (!msg.IsError)
                        {
                            msg = _ProtectionRepository.FileUpload(dsExcel, model);
                        } 
                    }  
                } 
            }
            catch (Exception ex)
            {
                msg.IsError = true;
                msg.ReturnMsg = "Uploaded file is not valid, please verify your file!"; 
            }
            TempData["ReturnMsg"] = msg.ReturnMsg; TempData["IsError"] = msg.IsError;

            if (msg.IsError)
            {
                return RedirectToAction("UploadOffenceDetails");
            }

            return RedirectToAction("ShowOffenceDetails");

        }

        public ActionResult AddNewRowForItemSeized(int currentRowIndex, long offenceDetailsID)
        {
            OffenceDetailsModel model = new OffenceDetailsModel();
            SeizedItemsModel sim = new SeizedItemsModel();
            List<SeizedItemsModel> lst = new List<SeizedItemsModel>();
            lst.Add(sim);
            model.SeizedItemsList = lst;
            ViewBag.CurrentIndex = currentRowIndex;
            ViewBag.RowType = FMDSS.RowType.ItemSeizedDetails;
            return PartialView("_AddNewRow", model);
        }
         
        public ActionResult GetLogDetails(long odID)
        {
            var model = _ProtectionRepository.GetLogDetails(odID);
            return PartialView("_LogDetails", model);
        }

        [HttpPost]
        public ActionResult UploadFiles(int docTypeID)
        {
            var isError = false;
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = Util.GetAppSettings("TempDocumentPath");

            List<CommonDocument> lstDoc = new List<CommonDocument>();
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    var docType = Util.GetListFromTable<CommonDocumentType>(_commonRepository.GetDocumentType(docTypeID)).FirstOrDefault();
                    //Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    string strDoc = string.Empty;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        file.SaveAs(Server.MapPath("~/" + FilePath + FileFullName));

                        if (Session["UploadFile"] == null)
                        {
                            lstDoc.Add(new CommonDocument
                            {
                                TempID = Guid.NewGuid().ToString(),
                                IsNew = true,
                                DocumentName = FileName,
                                DocumentPath = path,
                                DocumentTypeID = docTypeID,
                                DocumentLevel = docType.DocumentLevel,
                                DocumentTypeName = docType.DocumentTypeName
                            });
                            Session["UploadFile"] = lstDoc;
                        }
                        else
                        {
                            lstDoc = (List<CommonDocument>)Session["UploadFile"];
                            lstDoc.Add(new CommonDocument
                            {
                                TempID = Guid.NewGuid().ToString(),
                                IsNew = true,
                                DocumentName = FileName,
                                DocumentPath = path,
                                DocumentTypeID = docTypeID,
                                DocumentLevel = docType.DocumentLevel,
                                DocumentTypeName = docType.DocumentTypeName
                            });
                            Session["UploadFile"] = lstDoc;
                        }
                    }
                    if (docType.DocumentLevel > 0)
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentTypeID == docTypeID).ToList();
                    }
                    else
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentLevel == 0).ToList();
                    }

                }
                catch (Exception)
                {
                    isError = true;
                }
                return Json(new { isError = isError, data = lstDoc, returnMsg = "File Uploaded Successfully!" });
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public JsonResult RemoveUplodedFile(string TempID)
        {
            Boolean isError = false;
            try
            {
                List<CommonDocument> lstUploadFile = (List<CommonDocument>)Session["UploadFile"];
                if (lstUploadFile != null)
                {
                    CommonDocument result = lstUploadFile.SingleOrDefault(i => i.TempID == TempID);
                    if (result != null)
                    {
                        lstUploadFile.Remove(result);
                    }
                    Session["UploadFile"] = lstUploadFile;
                }
                return Json(new { isError = isError, returnMsg = "File removed Successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, returnMsg = ex.Message });
            }
        }

        #endregion

        #region Private Methods
        private void SetDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Location cs = new Location();
            try
            {
                _objModel.UserID = UserID; 
                var data = _ProtectionRepository.OffenceDetails_GetDropdownData();
                ViewBag.RangeCode = GetDropdownData(1, data.Tables[0]);
                ViewBag.OffenceCategory = GetDropdownData(2, data.Tables[1]);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, _objModel.UserID);
            }
        }

        private EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, DataTable dtDropdownData)
        {
            EnumerableRowCollection<SelectListItem> data = null;
            switch (actionCode)
            {
                case 1: 
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("RANGE_CODE"),
                        Text = x.Field<string>("RANGE_NAME")
                    }); 
                    return data;
                case 2:
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<int>("OffenceCategoryID")),
                        Text = x.Field<string>("OffenceCategoryName")
                    });
                    return data;
            }
            return null;
        }
        [HttpPost]
        public JsonResult SetDuplicateFIR(long CurrentRequestId, long RefRequestCaseId, string Remarks)
        {           
            Entity.ResponseMsg msg = _ProtectionRepository.SetDuplicateOffenceFIR(CurrentRequestId, RefRequestCaseId, Remarks);
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Offence History
        [HttpGet]
        public JsonResult ShowOffenceEidtHistory(long RequestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                List<ViewOffenceDetails> lst = new List<ViewOffenceDetails>();
                DataSet dsData = _ProtectionRepository.OffenceDetails_EditHistory(RequestId);

                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    ViewOffenceDetails obj = new ViewOffenceDetails();
                    obj.RowID = Convert.ToInt64(dr["RowID"].ToString());
                    obj.Id = Convert.ToInt64(dr["ID"].ToString());
                    obj.OffenceDetailsID = Convert.ToInt64(dr["OffenceDetailsId"].ToString());
                    obj.OffenderName = dr["OffenderName"].ToString();
                    obj.OffenceDescription = dr["OffenceDescription"].ToString();
                    obj.FIRNumber = dr["FIRNumber"].ToString();
                    obj.FIRDate = Convert.ToDateTime(dr["FIRDate"].ToString()).ToString("dd/MM/yyyy");
                    obj.InvestigatorOfficer = dr["InvestigatorOfficer"].ToString();
                    obj.TotalItemSeized = Convert.ToDecimal(dr["TotalItemSeized"].ToString());
                    obj.CompoundAmount = Convert.ToDecimal(dr["CompoundAmount"].ToString());
                    obj.CourtName = dr["CourtName"].ToString();
                    obj.CourtCaseNumber = dr["CourtCaseNumber"].ToString();
                    obj.NextHearingDate = dr["NextHearingDate"].ToString();
                    obj.DateOfFinalReport = dr["DateOfFinalReport"].ToString();
                    obj.SpecialRemarks = dr["SpecialRemarks"].ToString();
                    obj.logDate =Convert.ToDateTime(dr["logDateTime"]).ToString("dd/MM/yyyy");
                    obj.logTime = Convert.ToDateTime(dr["logDateTime"]).ToString("HH:MM");
                    lst.Add(obj);
                }


                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult ShowOffenceHistory()
        {
            OffenceDetailsModel model = new OffenceDetailsModel();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                Session["UploadFile"] = null;
                SetDropdownData();

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        public ActionResult GetOffenceHistory(long logID)
        {
            OffenceDetailsModel model = new OffenceDetailsModel();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                Session["UploadFile"] = null;
                SetDropdownData();

                var data = _ProtectionRepository.OffenceDetails_History(logID);
                model = Util.GetListFromTable<OffenceDetailsModel>(data, 0).FirstOrDefault();
                model.SeizedItemsList = Util.GetListFromTable<SeizedItemsModel>(data, 1);
                model.FIRDate = Convert.ToDateTime(model.FIRDate).ToString("dd/MM/yyyy");
                model.CompoundingDate = Convert.ToDateTime(model.CompoundingDate).ToString("dd/MM/yyyy");
                model.FileDate = Convert.ToDateTime(model.FileDate).ToString("dd/MM/yyyy");
                model.NextHearingDate = Convert.ToDateTime(model.NextHearingDate).ToString("dd/MM/yyyy");
                model.DateOfFinalReport = Convert.ToDateTime(model.DateOfFinalReport).ToString("dd/MM/yyyy");
                var cDocuments = Util.GetListFromTable<CommonDocument>(data, 2);

                foreach (var item in cDocuments)
                {
                    item.IsNew = false;
                    item.TempID = Guid.NewGuid().ToString();
                }

                Session["UploadFile"] = cDocuments;

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
                return View("ShowOffenceHistory", model);
            }
            catch (Exception ex)
            {
                ViewBag.IsError = 1;
                ViewBag.ReturnMsg = ex.Message;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("ShowOffenceHistory", model);
        }
        #endregion 
    }
}
