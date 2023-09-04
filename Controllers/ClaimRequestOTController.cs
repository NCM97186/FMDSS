using FMDSS.Entity;
using FMDSS.Entity.FRAViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Globals;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class ClaimRequestOTController : BaseController
    {
        #region [Properties & Variables]
        private Models.FmdssContext.FmdssContext dbContext;
        private IClaimRequestOTRepository _repository;
        #endregion

        #region [Constructor]
        public ClaimRequestOTController()
        {
            dbContext = new Models.FmdssContext.FmdssContext();
            _repository = new ClaimRequestOTRepository();
        }
        #endregion

        public ActionResult ClaimRequestDetails(Int64 ReqID = 0)
        {
            Session["UploadFile"] = null;
            ModelState.Clear();
            ClaimRequestOTVM model = new ClaimRequestOTVM();
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
            return View(model);
        }

        [HttpPost]
        public ActionResult ClaimRequestDetails(ClaimRequestOTVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.ClaimRequestDocument = (List<CommonDocument>)Session["UploadFile"];
                    string returnMsg = string.Empty; Boolean isError = false;
                    _repository.SaveClaimRequestDetails(model, ref returnMsg, ref isError);
                    TempData["ReturnMsg"] = returnMsg;
                    TempData["IsError"] = isError;
                    Session["UploadFile"] = null;
                    ModelState.Clear();
                    return RedirectToAction("ClaimRequestDetails");
                }
                catch (Exception ex) { }
            }
            else
            {
                ViewBag.ReturnMsg = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault().ErrorMessage;
                ViewBag.IsError = true;
                return View(model);
            }

            return View(model);
        }

        public ActionResult LoadClaimRequest(int claimTypeId)
        {
            ClaimRequestOTVM model = new ClaimRequestOTVM();
            model.ClaimRequestDetails = new tbl_FRA_ClaimRequestDetailsOT();
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
        public JsonResult IsValidSSO(string SSOID)
        {
            return Json(_repository.IsValidSSO(SSOID), JsonRequestBehavior.AllowGet);
        }
    }
}
