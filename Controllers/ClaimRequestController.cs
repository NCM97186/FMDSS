using FMDSS.Entity;
using FMDSS.Entity.FRA.FRAViewModel;
using FMDSS.Entity.FRAViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Globals;
using FMDSS.Models;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    //********************************************************************************************************
    //  Project      : Forest Management & Decision Support System (FMDSS) 
    //  Project Code : IEISLSSD-2015-16-ENV-004
    //  Copyright (C): IEISL 
    //  File         : ClaimRequestController
    //  Description  : Performed operaion for FRA module
    //  Date Created : 16-Dec-2018
    //  History      : 
    //  Version      : 2.0
    //  Author       : Dipak Soni
    //  Modified By  : Dipak Soni
    //  Modified On  : 08-Feb-2018
    //  Reviewed By  : 
    //  Reviewed On  : 
    //********************************************************************************************************
    public class ClaimRequestController : BaseController
    {
        #region [Properties & Variables]
        private Models.FmdssContext.FmdssContext dbContext;
        private IClaimRequestRepository _repository;
        private ICommonRepository _commonRepository;
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
        public ClaimRequestController()
        {
            dbContext = new Models.FmdssContext.FmdssContext();
            _repository = new ClaimRequestRepository();
            _commonRepository = new CommonRepository();
        }
        #endregion

        #region [Appeal Request Operation]
        public ActionResult AppealRequest()
        {
            Session["UploadFile"] = null;

            SetAppealDropdownData();
            if (TempData["ReturnMsg"] != null)
            {
                ViewBag.IsError = TempData["IsError"];
                ViewBag.ReturnMsg = TempData["ReturnMsg"];
                TempData["ReturnMsg"] = null; TempData["IsError"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult AppealRequest(AppealRequestVM model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                var msg = _repository.AppealRequest_Save(model);

                if (!msg.IsError)
                {
                    Session["UploadFile"] = null;
                }

                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ReturnMsg"] = ex.Message;
                TempData["IsError"] = true;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult GetAppealDetails(string parentID, int userType)
        {
            ApproverRemarksCommonVM model = new ApproverRemarksCommonVM();
            model.ParentID = parentID;
            ViewBag.UserType = userType;
            return PartialView("_ApproveAppealRequest", model);
        }

        public JsonResult GetRequestDetailsForAppeal(string reqID)
        {
            var data = _repository.GetClaimRequestDetails(5, reqID);

            if (Util.isValidDataSet(data, 0, true))
            {
                return Json(new { data = Util.GetListFromTable<ClaimRequestForAppealVM>(data, 0), IsError = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RaiseRequestAgainstAppeal(string appealNo)
        {
            TempData["AppealNo"] = appealNo;
            return RedirectToAction("ClaimRequestDetails");
        }

        [HttpPost]
        public ActionResult UpdateAppealDetails(ApproverRemarksCommonVM model, int userType)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                var msg = _repository.UpdateAppealDetails(model);

                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;

                if (userType == Convert.ToInt32(FRAUserType.SDLCSDO))
                {
                    return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg, redirectURL = "ApprovalProcessSDLCSDO" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg, redirectURL = "ApprovalProcessDLCFinal" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                TempData["ReturnMsg"] = ex.Message;
                TempData["IsError"] = true;
            }

            return Json(new { IsError = true, ReturnMsg = "Something went wrong, please try again!", redirectURL = "" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [Citizen Request]
        [HttpGet]
        public ActionResult Index()
        {
            var model = GetDefaultWorkFlowApprover(0);
            try
            {
                model.ClaimRequestDetailsListForApproval = _repository.GetClaimRequestList();

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
            }
            catch (Exception ex) { }
            return View(model);
        }

        [HttpGet]
        public ActionResult ClaimRequestDetails(Int64 ReqID = 0)
        {
            Session["UploadFile"] = null;
            ModelState.Clear();
            ClaimRequestVM model = new ClaimRequestVM();
            model.ClaimRequestDetails = new tbl_FRA_ClaimRequestDetails();
            model = _repository.GetClaimRequestDetails(model, ReqID);

            if (ReqID > 0)
            {
                foreach (var item in model.ClaimRequestDocument)
                {
                    item.IsNew = false;
                    item.TempID = Guid.NewGuid().ToString();
                }
                Session["UploadFile"] = model.ClaimRequestDocument;
            }
            else if (TempData["AppealNo"] != null)
            {
                var appealData = Convert.ToString(TempData["AppealNo"]).Split('|');
                model.ClaimRequestDetails.AppealRequestID = appealData[0];
                model.ClaimRequestDetails.RejectedRefNumber = appealData[1];
                model.ClaimRequestDetails.ApplicationType = 1;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ClaimRequestDetails(ClaimRequestVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ClaimRequestDocument = (List<CommonDocument>)Session["UploadFile"];

                    var response = _repository.SaveClaimRequestDetails(model);
                    TempData["ReturnMsg"] = response.ReturnMsg;
                    TempData["IsError"] = response.IsError;
                    Session["UploadFile"] = null;
                    string emitraServiceID = Convert.ToString(Session["EmitrServiceID"]);

                    #region Emitra Payment
                    if (response.IsEmitraCheckRequired == true && !string.IsNullOrWhiteSpace(emitraServiceID))
                    {
                        Models.CommanModels.PaymentViewModel payModel = new Models.CommanModels.PaymentViewModel();
                        payModel.emitraserviceid = emitraServiceID;
                        payModel.requestid = response.DisplayRequestID;
                        payModel.ActionCode = "FRA";
                        var paymentResponse = FMDSS.Models.CommanModels.PaymentRepository.Pay(BookingType.EmitraKioskBooking, payModel);

                        if (!paymentResponse.Status.ToUpper().Equals("SUCCESS"))
                        {
                            _repository.UpdateClaimRequestTransaction(response.ClaimRequestDetailsID, paymentResponse);
                        }
                        return View("_KioskTransactionStatus", paymentResponse);
                    }
                    #endregion

                    //if(Convert.ToInt32(Session["CURRENT_ROLE"]) == 8)
                    //    return RedirectToAction("Index");
                    //else
                    //    return RedirectToAction("ApprovalGramSabha");

                    if (Convert.ToInt32(Session["CURRENT_ROLE"]) == 1025)
                        return RedirectToAction("ApprovalGramSabha");
                    else
                        return RedirectToAction("Index");
                }
                catch (Exception ex) { }
            }
            else
            {
                ViewBag.ReturnMsg = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;
                ViewBag.IsError = true;
                model = _repository.GetClaimRequestDetails(model, model.ClaimRequestDetails.ClaimRequestDetailsID);
            }

            return View(model);
        }

        public JsonResult GetBlock(long distID)
        {
            try
            {
                var blockList = dbContext.tbl_FRA_Block.Where(x => distID == x.DistrictID).AsQueryable().OrderBy(x => x.BlockName).Select(x => new { Text = x.BlockName, Value = x.BlockID });
                return Json(new { isError = false, data = blockList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBlockTehsil(long distID)
        {
            try
            {
                var blockList = dbContext.tbl_FRA_Block.Where(x => distID == x.DistrictID).AsQueryable().OrderBy(x => x.BlockName).Select(x => new { Text = x.BlockName, Value = x.BlockID });
                var tehsilList = dbContext.tbl_FRA_Tehsil.Where(x => distID == x.DistrictID).AsQueryable().OrderBy(x => x.TehsilName).Select(x => new { Text = x.TehsilName, Value = x.TehsilID });
                return Json(new { isError = false, blockList = blockList, tehsilList = tehsilList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetClaimRequestPupose(int claimTypeID)
        {
            try
            {
                var purposeList = dbContext.tbl_FRA_ClaimRequestPurpose.AsQueryable().Where(x => claimTypeID.Equals(x.ClaimTypeID)).OrderBy(x => x.Purpose).Select(x => new { Text = x.Purpose, Value = x.ClaimRequestPurposeID });
                return Json(new { isError = false, data = purposeList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetActionReason(int actionID)
        {
            try
            {
                var actionReasonList = dbContext.tbl_FRA_ActionReason.AsQueryable().Where(x => actionID == x.ActionID).OrderBy(x => x.ActionReason).Select(x => new { Text = x.ActionReason, Value = x.ActionReasonID });
                return Json(new { isError = false, data = actionReasonList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGramPanchayat(string blockID)
        {
            try
            {
                var blkID = blockID.Split(',');

                var gpList = dbContext.tbl_FRA_GPs.AsEnumerable().Where(x => blkID.Any(y => y.Equals(Convert.ToString(x.BlockID)))).OrderBy(x => x.GPName).Select(x => new { Text = x.GPName, Value = x.GPID });
                return Json(new { isError = false, data = gpList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVillage(string gpID)
        {
            try
            {
                var gpIDList = gpID.Split(',');
                var villageList = dbContext.tbl_FRA_Village.AsEnumerable().Where(x => gpIDList.Any(y => y.Equals(Convert.ToString(x.GPID)))).OrderBy(x => x.VillageName).Select(x => new { Text = x.VillageName, Value = x.VillageCode });

                return Json(new { isError = false, data = villageList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
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
                    if (docType.DocumentLevel == 1)
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentTypeID == docTypeID).ToList();
                    }
                    else if (docType.DocumentLevel == 4)
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentLevel == 4).ToList();
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
        public ActionResult LoadClaimRequest(int claimTypeId)
        {
            ClaimRequestVM model = new ClaimRequestVM();
            model.ClaimRequestDetails = new tbl_FRA_ClaimRequestDetails();
            model.ClaimRequestDetails.ClaimTypeID = claimTypeId;
            model = _repository.GetClaimRequestDetails(model, 0);
            if (claimTypeId == Convert.ToInt32(FRAClaimType.Individual))
            {
                return PartialView("_ForestLand", model);
            }
            else
            {
                return PartialView("_ClaimCommunityForestResource", model);
            }
        }
        #endregion

        #region [Gram Sabha Approval]
        public ActionResult AddNewRowForSurveyDetails(int currentRowIndex, long surveyDetailsID)
        {
            SurveyDetailsVM model = new SurveyDetailsVM();
            model.SurveyDetails = new tbl_FRA_SurveyDetails();
            model.SurveyDetails.SurveyDetailsID = surveyDetailsID;
            ViewBag.CurrentIndex = currentRowIndex;
            ViewBag.RowType = FMDSS.RowType.SurveyDetails;
            return PartialView("_AddNewRow", model);
        }
        public ActionResult AddSurveyDetails(long claimRequestDetailsID, int userType)
        {
            var model = GetDefaultSurveyDetails(claimRequestDetailsID);
            if (Session["FRASDData"] != null)
            {
                var _sdObj = (tbl_FRA_SurveyDetails)Session["FRASDData"];
                if (model.SurveyDetails == null)
                    model.SurveyDetails = new tbl_FRA_SurveyDetails();
                model.SurveyDetails.Latitude = _sdObj.Latitude;
                model.SurveyDetails.Longitude = _sdObj.Longitude;
                model.SurveyDetails.North = _sdObj.North;
                model.SurveyDetails.South = _sdObj.South;
                model.SurveyDetails.West = _sdObj.West;
                model.SurveyDetails.East = _sdObj.East;
                model.SurveyDetails.GISID = _sdObj.GISID;
                model.SurveyDetails.ActivityData = _sdObj.ActivityData;
                Session["FRASDData"] = null;
            }

            if (userType == Convert.ToInt32(FRAUserType.GramSabha) && model.ClaimRequestDetailsVM != null && model.ClaimRequestDetailsVM.CurrentApproverDesignationID == 19
                && ((Util.GetBoolean(model.ClaimRequestDetailsVM.IsForesterGenerated) == false) || Util.GetBoolean(model.ClaimRequestDetailsVM.IsHalkaPatwariGenerated) == false))
            {
                return PartialView("_AddSurveyDetails", model);
            }
            else if (userType == Convert.ToInt32(FRAUserType.Collector) && model.ClaimRequestDetailsVM != null && model.ClaimRequestDetailsVM.CurrentApproverDesignationID == 20
                && Util.GetBoolean(model.ClaimRequestDetailsVM.IsPattaGenerated) == false)
            {
                return PartialView("_EditSurveyDetails", model);
            }
            else
            {
                return PartialView("_ViewSurveyDetails", model);
            }
        }
        public ActionResult RequireSurveyDetailsAgain(string claimRequestDetailsID, string actionCode)
        {
            var data = _repository.UpdateClaimRequest(claimRequestDetailsID, actionCode);
            if (Util.isValidDataSet(data, 0, true))
            {
                var msg = Globals.Util.GetListFromTable<ResponseMsg>(data, 0).FirstOrDefault();
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
            }
            return RedirectToAction("ApprovalGramSabha");
        }


        [HttpPost]
        public ActionResult AddSurveyDetails(SurveyDetailsVM model, string command, string OTP, string TransationID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rootPath = Server.MapPath("~/");
                    string returnMsg = string.Empty; Boolean isError = false;
                    _repository.AddSurveyDetails(model.SurveyDetails, command, rootPath, OTP, TransationID, ref returnMsg, ref isError);

                    if (!isError)
                    {
                        Session["UploadFile"] = null;
                    }
                    return Json(new { isError = isError, msg = returnMsg }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { isError = true, msg = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateSurveyDetails(SurveyDetailsVM model)
        {
            var msg = _repository.UpdateSurveyDetails(model.SurveyDetails);
            return Json(new { isError = msg.IsError, msg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ApprovalGramSabha()
        {
            var model = new WorkFlowApproverVM();
            try
            {
                model.ClaimRequestDetailsListForApproval = _repository.GetClaimRequestForApproval();

                if (TempData["reqID"] != null)
                {
                    ViewBag.reqID = Convert.ToInt64(TempData["reqID"]);
                }

            }
            catch (Exception ex) { }
            return View(model);
        }

        public ActionResult GetGISData(GISResponseVM model)
        {
            try
            {
                tbl_FRA_SurveyDetails _objmodel = new tbl_FRA_SurveyDetails();
                if (model.centroidData != "" && model.centroidData.Contains(","))
                {
                    string[] locCentroid = model.centroidData.Split(',');
                    _objmodel.Latitude = locCentroid[1] == "NA" ? "" : locCentroid[1];
                    _objmodel.Longitude = locCentroid[0] == "NA" ? "" : locCentroid[0];
                }

                if (model.villageData != null)
                {
                    var directionList = JsonConvert.DeserializeObject<List<GISVillageData>>(model.villageData);
                    string northVillage = string.Empty;
                    string southVillage = string.Empty;
                    string eastVillage = string.Empty;
                    string westVillage = string.Empty;

                    foreach (var item in directionList)
                    {
                        if (item.Direction.Equals("North"))
                        {
                            northVillage = (string.IsNullOrEmpty(northVillage) ? northVillage : northVillage + ",") + dbContext.tbl_FRA_Village.Where(x => x.VillageCode.Equals(item.CENSUS_CD_2011)).Select(x => x.VillageName).FirstOrDefault();
                        }
                        else if (item.Direction.Equals("South"))
                        {
                            southVillage = (string.IsNullOrEmpty(southVillage) ? southVillage : southVillage + ",") + dbContext.tbl_FRA_Village.Where(x => x.VillageCode.Equals(item.CENSUS_CD_2011)).Select(x => x.VillageName).FirstOrDefault();
                        }
                        else if (item.Direction.Equals("East"))
                        {
                            eastVillage = (string.IsNullOrEmpty(eastVillage) ? eastVillage : eastVillage + ",") + dbContext.tbl_FRA_Village.Where(x => x.VillageCode.Equals(item.CENSUS_CD_2011)).Select(x => x.VillageName).FirstOrDefault();
                        }
                        else if (item.Direction.Equals("West"))
                        {
                            westVillage = (string.IsNullOrEmpty(westVillage) ? westVillage : westVillage + ",") + dbContext.tbl_FRA_Village.Where(x => x.VillageCode.Equals(item.CENSUS_CD_2011)).Select(x => x.VillageName).FirstOrDefault();
                        }
                    }

                    _objmodel.North = string.IsNullOrEmpty(northVillage) ? "N/A" : northVillage;
                    _objmodel.South = string.IsNullOrEmpty(southVillage) ? "N/A" : southVillage;
                    _objmodel.East = string.IsNullOrEmpty(eastVillage) ? "N/A" : eastVillage;
                    _objmodel.West = string.IsNullOrEmpty(westVillage) ? "N/A" : westVillage;
                    _objmodel.GISID = model.gisId;

                    if (!string.IsNullOrEmpty(model.activityData))
                        _objmodel.ActivityData = model.activityData;


                }
                TempData["reqID"] = Request.QueryString.Get("ReqID");
                Session["FRASDData"] = _objmodel;
            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        #endregion

        #region [ApprovalProcessSDLC]
        [HttpGet]
        public ActionResult ApprovalProcessSDLC()
        {
            var model = new WorkFlowApproverVM();
            try
            {
                model.ClaimRequestDetailsListForApproval = _repository.GetClaimRequestForApproval();
            }
            catch (Exception ex) { }
            return View(model);
        }
        #endregion

        #region [ApprovalProcessSDLCSDO]
        [HttpGet]
        public ActionResult ApprovalProcessSDLCSDO()
        {
            var model = new WorkFlowApproverVM();
            try
            {
                model.ClaimRequestDetailsListForApproval = _repository.GetClaimRequestForApproval();
            }
            catch (Exception ex) { }
            return View(model);
        }
        #endregion

        #region [ApprovalProcessDLC]
        [HttpGet]
        public ActionResult ApprovalProcessDLC()
        {
            var model = new WorkFlowApproverVM();
            try
            {
                model.ClaimRequestDetailsListForApproval = _repository.GetClaimRequestForApproval();
            }
            catch (Exception ex) { }
            return View(model);
        }
        #endregion

        #region [ApprovalProcessDLCFinal]
        [HttpGet]
        public ActionResult ApprovalProcessDLCFinal()
        {
            var model = new WorkFlowApproverVM();
            try
            {
                model.ClaimRequestDetailsListForApproval = _repository.GetClaimRequestForApproval();
            }
            catch (Exception ex) { }
            return View(model);
        }
        #endregion

        #region [Common Actions]
        public ActionResult GetRequesterFormDetails(long reqID, int reqTypeID)
        {
            ClaimRequestVM model = new ClaimRequestVM();
            model = _repository.GetClaimRequestDetails(model, reqID);
            Session["UploadFile"] = model.ClaimRequestDocument;

            if (reqTypeID == Convert.ToInt32(FRAClaimType.Community))
                return PartialView("_ClaimCommunityForestResource", model);
            else
                return PartialView("_ForestLand", model);
        }
        public ActionResult GetWorkFlowDetails(long claimRequestDetailsID, int userType)
        {
            Session["UploadFile"] = null;
            var model = GetDefaultWorkFlowApprover(claimRequestDetailsID);
            try
            {
                model.WorkFlowDetailsList = _repository.GetWorkFlowDetailsList(claimRequestDetailsID);
            }
            catch (Exception ex) { }
            if (userType == Convert.ToInt32(FRAUserType.Citizen))
                return PartialView("_WorkFlowForCitizen", model);
            else if (userType == Convert.ToInt32(FRAUserType.GramSabha))
                return PartialView("_WorkFlowForGramSabha", model);
            else if (userType == Convert.ToInt32(FRAUserType.SDLC))
                return PartialView("_WorkFlowForSDLCRanger", model);
            else if (userType == Convert.ToInt32(FRAUserType.DLC))
                return PartialView("_WorkFlowForDLC", model);
            else if (userType == Convert.ToInt32(FRAUserType.SDLCSDO))
                return PartialView("_WorkFlowForSDLCSDO", model);
            else
                return PartialView("_WorkFlowDetails", model);
        }
        public ActionResult ApproveAllRequest(string claimRequestDetailsID, int userType)
        {
            WorkFlowApproverMultipleVM model = new WorkFlowApproverMultipleVM();
            model.ClaimRequestDetails = _repository.GetClaimRequestDetails(claimRequestDetailsID);
            ViewBag.ClaimReqIDs = claimRequestDetailsID;
            return PartialView("_ApproveAllRequest", model);
        }

        public ActionResult PrintRaisedRequest(long claimRequestDetailsID, int userType)
        {
            Session["UploadFile"] = null;
            var model = GetDefaultWorkFlowApprover(claimRequestDetailsID);
            try
            {
                model.WorkFlowDetailsList = _repository.GetWorkFlowDetailsList(claimRequestDetailsID);
            }
            catch (Exception ex) { }

            return PartialView("_PrintRaisedRequest", model);
        }

        public ActionResult SaveAndDownloadReceipt(long claimRequestDetailsID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var model = GetDefaultWorkFlowApprover(claimRequestDetailsID);

            try
            {
                model.WorkFlowDetailsList = _repository.GetWorkFlowDetailsList(claimRequestDetailsID);
                string filepath = _repository.DownloadReceipt(model);
                if (System.IO.File.Exists(filepath))
                {
                    filepath = GetVirtualPath(filepath);
                }

                return Json(new { isError = false, pageURL = filepath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                return Json(new { isError = true, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateWorkFlowDetails(WorkFlowApproverVM model, string command)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                string returnMsg = string.Empty; Boolean isError = false;
                _repository.UpdateWorkFlowDetails(model, rootPath, ref returnMsg, ref isError);

                if (!isError)
                {
                    Session["UploadFile"] = null; //Needs work
                }
                ViewBag.IsError = isError;
                ViewBag.ReturnMsg = returnMsg;
                var citizenClaimRequestDetails = _repository.GetClaimRequestForApproval();
                return PartialView("_ClaimRequestDetails", citizenClaimRequestDetails);
            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        [HttpPost]
        public ActionResult UpdateWorkFlowDetailsForESign(WorkFlowApproverVM model, string command, string OTP, string TransationID)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                string returnMsg = string.Empty; Boolean isError = false;
                if (model.ApproverRemarksDetails.StatusID == Convert.ToInt32(FMDSS.ActionTypeForFRA.Approve))
                {
                    _repository.UpdateWorkFlowDetailsForESign(model, rootPath, command, OTP, TransationID, ref returnMsg, ref isError);
                }
                else
                {
                    _repository.UpdateWorkFlowDetails(model, rootPath, ref returnMsg, ref isError);
                }

                ViewBag.IsError = isError;
                ViewBag.ReturnMsg = returnMsg;
                var citizenClaimRequestDetails = _repository.GetClaimRequestForApproval();
                return PartialView("_ClaimRequestDetails", citizenClaimRequestDetails);
            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        [HttpPost]
        public ActionResult UpdateWorkFlowDetailsMultipleForESign(WorkFlowApproverMultipleVM model, string command, string OTP, string TransationID)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                string returnMsg = string.Empty; Boolean isError = false;
                _repository.UpdateWorkFlowDetailsMultipleForESign(model, rootPath, command, OTP, TransationID, ref returnMsg, ref isError);

                ViewBag.IsError = isError;
                ViewBag.ReturnMsg = returnMsg;
                var citizenClaimRequestDetails = _repository.GetClaimRequestForApproval();
                return PartialView("_ClaimRequestDetails", citizenClaimRequestDetails);
            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        [HttpPost]
        public ActionResult UpdateDocWitheSign(WorkFlowApproverVM model, string command, string OTP, string TransationID)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                var msg = _repository.UpdateDocWitheSign(model, rootPath, command, OTP, TransationID);

                ViewBag.IsError = msg.IsError;
                ViewBag.ReturnMsg = msg.ReturnMsg;
                var citizenClaimRequestDetails = _repository.GetClaimRequestForApproval();
                return PartialView("_ClaimRequestDetails", citizenClaimRequestDetails);
            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        [HttpPost]
        public ActionResult UpdateDocMultipleWitheSign(WorkFlowApproverMultipleVM model, string command, string OTP, string TransationID)
        {
            try
            {
                var rootPath = Server.MapPath("~/");
                var msg = _repository.UpdateDocMultipleWitheSign(model, rootPath, command, OTP, TransationID);

                ViewBag.IsError = msg.IsError;
                ViewBag.ReturnMsg = msg.ReturnMsg;
                var citizenClaimRequestDetails = _repository.GetClaimRequestForApproval();
                return PartialView("_ClaimRequestDetails", citizenClaimRequestDetails);
            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        [HttpPost]
        public ActionResult UpdateWorkFlowDetailsForCitizen(WorkFlowApproverVM model)
        {
            try
            {
                string returnMsg = string.Empty; Boolean isError = false;
                _repository.UpdateWorkFlowDetailsForCitizen(model, ref returnMsg, ref isError);

                if (!isError)
                {
                    var citizenClaimRequestDetails = _repository.GetClaimRequestForApproval();
                    return PartialView("_ClaimRequestDetails", citizenClaimRequestDetails);
                }
                else
                {
                    return Json(new { IsError = true, returnMsg = returnMsg }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex) { }
            return RedirectToAction("ApprovalGramSabha");
        }

        public JsonResult ValidateAndGetSSODetails(string ssoID)
        {
            try
            {
                RAJSSO.SSO SSO = new RAJSSO.SSO();
                var userDetails = SSO.GetUserDetailJSON(ssoID, System.Configuration.ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[0], System.Configuration.ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[1]);

                return Json(new { isError = false, data = userDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidateSSOID(string ssoID)
        {
            try
            {
                RAJSSO.SSO SSO = new RAJSSO.SSO();
                var userDetails = SSO.GetUserDetailJSON(ssoID, System.Configuration.ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[0], System.Configuration.ConfigurationManager.AppSettings["SSOServiceUser"].Split(',')[1]);
                var additionalInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserAdditionalInfo>(userDetails);
                return Json(!string.IsNullOrEmpty(additionalInfo.SSOID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewDetailsCommon(int actionCode, string parentID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionResult actionResult = null; dynamic obj = null; dynamic data = null;
            try
            {
                if (!String.IsNullOrEmpty(parentID))
                {
                    obj = _repository.AppealRequest_Get(actionCode);
                    if (Util.isValidDataSet(obj, 0))
                    {
                        data = Util.GetListFromTable<AppealRequestDetailsVM>(obj, 0);
                    }

                    switch (actionCode)
                    {
                        case 2:
                            actionResult = PartialView("_AppealDetailsForApprover", data);
                            break;
                        case 4:
                            actionResult = PartialView("_AppealDetailsPending", data);
                            break;
                        default:
                            actionResult = PartialView("_AppealDetails", data);
                            break;
                    }
                }
            }
            catch (Exception ex) { }
            return actionResult;

        }
        #endregion

        #region [Private Methods]
        private WorkFlowApproverVM GetDefaultWorkFlowApprover(long claimRequestDetailsID)
        {
            WorkFlowApproverVM model = new WorkFlowApproverVM();
            var cRViewModel = new ClaimRequestVM();
            cRViewModel.ClaimRequestDetails = new tbl_FRA_ClaimRequestDetails();
            model.ClaimRequest = cRViewModel;
            if (claimRequestDetailsID > 0)
            {
                model.CitizenClaimRequestDetails = _repository.GetWorkFlowDetails(claimRequestDetailsID);
            }

            return model;
        }

        private SurveyDetailsVM GetDefaultSurveyDetails(long claimRequestDetailsID)
        {
            SurveyDetailsVM model = new SurveyDetailsVM();

            if (claimRequestDetailsID > 0)
            {
                model.ClaimRequestDetailsVM = _repository.GetWorkFlowDetails(claimRequestDetailsID);
                model.SurveyDetails = _repository.GetSurveyDetails(claimRequestDetailsID);
                if (model.SurveyDetails != null)
                {
                    //Session["UploadFile"] = _repository.GetClaimRequestDocument(model.SurveyDetails.WorkFlowDetailsID);
                    Session["UploadFile"] = _commonRepository.GetAttachedDocument(model.SurveyDetails.WorkFlowDetailsID, 5);
                }
            }

            return model;
        }

        private void SetAppealDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var dtDropdownData = _repository.GetClaimRequestDetails(4);

                if (Util.isValidDataSet(dtDropdownData, 0))
                {
                    ViewBag.DistList = dtDropdownData.Tables[0].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("DistrictID")),
                        Text = x.Field<string>("DistrictName")
                    });
                }
                if (Util.isValidDataSet(dtDropdownData, 1))
                {
                    ViewBag.DesignationList = dtDropdownData.Tables[1].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<int>("DesignationID")),
                        Text = x.Field<string>("DesignationName")
                    });
                }
                if (Util.isValidDataSet(dtDropdownData, 2))
                {
                    ViewBag.ClaimRequestList = dtDropdownData.Tables[2].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("ClaimRequestDetailsID")),
                        Text = x.Field<string>("ClaimRequestIDWithPrefix")
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
        }

        #endregion

        #region User Registration

        [HttpPost]
        public ActionResult UserRegistrationFilterList(long DesignationID = 0, long DistictId = 0)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var rDetails = new List<RegistrationVM>();
            try
            {
                var data = _repository.UserRegistration_Get(DesignationID, DistictId);

                if (Util.isValidDataSet(data, 0))
                {
                    rDetails = Util.GetListFromTable<RegistrationVM>(data, 0);
                }

                ViewBag.DesignationList = new SelectList(data.Tables[1].AsDataView(), "Value", "Text");
                ViewBag.DistictList = new SelectList(data.Tables[2].AsDataView(), "Value", "Text");

                return View("UserRegistration", rDetails);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View("UserRegistration", rDetails);
        }


        public ActionResult UserRegistration()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var rDetails = new List<RegistrationVM>();
            try
            {
                var data = _repository.UserRegistration_Get(0, 0);

                if (Util.isValidDataSet(data, 0))
                {
                    rDetails = Util.GetListFromTable<RegistrationVM>(data, 0);
                }

                ViewBag.DesignationList = new SelectList(data.Tables[1].AsDataView(), "Value", "Text");
                ViewBag.DistictList = new SelectList(data.Tables[2].AsDataView(), "Value", "Text");
                return View(rDetails);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View("UserRegistration", rDetails);
        }

        [HttpPost]
        public ActionResult UserRegistration(UserRegistration model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                var msg = _repository.UserRegistration_Save(model);

                var distList = dbContext.tbl_FRA_District.OrderBy(x => x.DistrictName).Select(x => new DropDownListVM { Text = x.DistrictName, Value = x.DistrictID }).ToList();
                ViewBag.DistList = distList;

                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return PartialView("_partialUserProfileDetails", model);
            }
            catch (Exception ex)
            {
                TempData["ReturnMsg"] = ex.Message;
                TempData["IsError"] = true;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return PartialView("_partialUserProfileDetails", model);
        }

        public ActionResult GetCitizenUserProfileDetails(long objectID)
        {
            List<SelectListItem> lstisSSo = new List<SelectListItem>();
            List<SelectListItem> LSTDesignation = new List<SelectListItem>();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID1 = Convert.ToInt64(Session["UserID"]);
            Models.Master.UserProfileDetails obj1 = new Models.Master.UserProfileDetails();
            UserRegistration obj = new UserRegistration();
            try
            {
                ViewBag.OpType = (objectID == 0 ? "Add User" : "Edit User");

                DataSet data = _repository.UserRegistration_Get(objectID);

                if (Util.isValidDataSet(data, 0, true))
                {
                    obj = Util.GetListFromTable<UserRegistration>(data, 0).FirstOrDefault();
                }

                if (Util.isValidDataSet(data, 1))
                {
                    ViewBag.DesignationList = data.Tables[1].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<int>("DesigId")),
                        Text = x.Field<string>("Desig_Name")
                    });
                }

                if (Util.isValidDataSet(data, 2))
                {
                    ViewBag.DistList = data.Tables[2].AsEnumerable().Select(x => new DropDownListVM
                    {
                        Value = x.Field<long>("DistrictID"),
                        Text = x.Field<string>("DistrictName")
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID1);

            }
            return PartialView("_partialUserProfileDetails", obj);
        }

        public JsonResult GetBlockPermissionWise(string distID, string designationID, string selectedValue = null)
        {
            try
            {
                DataSet data = _repository.GetDropdownData(2, distID, designationID, selectedValue);
                var blockList = new List<DropDownListVM>();

                if (Util.isValidDataSet(data, 0))
                {
                    blockList = data.Tables[0].AsEnumerable().Select(x => new DropDownListVM
                    {
                        Value = x.Field<long>("BlockID"),
                        Text = x.Field<string>("BlockName")
                    }).ToList();
                }

                return Json(new { isError = false, data = blockList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBlockTehsilPermissionWise(string distID, string designationID, string selectedValue = null)
        {
            try
            {
                DataSet data = _repository.GetDropdownData(1, distID, designationID, selectedValue);
                var blockList = new List<DropDownListVM>();
                var tehsilList = new List<DropDownListVM>();

                if (Util.isValidDataSet(data, 0))
                {
                    blockList = data.Tables[0].AsEnumerable().Select(x => new DropDownListVM
                    {
                        Value = x.Field<long>("BlockID"),
                        Text = x.Field<string>("BlockName")
                    }).ToList();
                }

                if (Util.isValidDataSet(data, 1))
                {
                    tehsilList = data.Tables[1].AsEnumerable().Select(x => new DropDownListVM
                    {
                        Value = x.Field<long>("TehsilID"),
                        Text = x.Field<string>("TehsilName")
                    }).ToList();
                }
                return Json(new { isError = false, blockList = blockList, tehsilList = tehsilList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGramPanchayatPermissionWise(string blockID, string designationID, string selectedValue = null)
        {
            try
            {
                DataSet data = _repository.GetDropdownData(3, blockID, designationID, selectedValue);
                var gpList = new List<DropDownListVM>();

                if (Util.isValidDataSet(data, 0))
                {
                    gpList = data.Tables[0].AsEnumerable().Select(x => new DropDownListVM
                    {
                        Value = x.Field<long>("GPID"),
                        Text = x.Field<string>("GPName")
                    }).ToList();
                }
                return Json(new { isError = false, data = gpList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDistrictPermissionWise(string designationID, string selectedValue = null)
        {
            try
            {
                DataSet data = _repository.GetDropdownData(4, designationID, "", selectedValue);
                var distList = new List<DropDownListVM>();

                if (Util.isValidDataSet(data, 0))
                {
                    distList = data.Tables[0].AsEnumerable().Select(x => new DropDownListVM
                    {
                        Value = x.Field<long>("DistrictID"),
                        Text = x.Field<string>("DistrictName")
                    }).ToList();
                }
                return Json(new { isError = false, data = distList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { isError = true, msg = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods
        private string GetVirtualPath(string physicalPath)
        {
            string rootpath = Server.MapPath("~/");

            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");

            return physicalPath;
        }
        #endregion
    }
}
