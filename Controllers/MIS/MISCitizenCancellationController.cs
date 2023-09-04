#region Namespaces
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FMDSS.Models.MIS;
using Newtonsoft.Json;
using FMDSS.Models;
#endregion


namespace FMDSS.Controllers.MIS
{
    public class MISCitizenCancellationController : BaseController
    {
        #region [CitizenCancellationDetails]
        [HttpGet]
        public ActionResult CitizenCancellationDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                CitizenWildLifeCancellationViewModel model = new CitizenWildLifeCancellationViewModel();
                MISCWLTicketBookingCancellationRepository obj = new MISCWLTicketBookingCancellationRepository();

                var dsCitezenData = obj.GetCitizenCancellationDetails(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), "", "");

                if (dsCitezenData != null && dsCitezenData.Tables.Count > 0)
                {
                    string strCitizenData = JsonConvert.SerializeObject(dsCitezenData.Tables[0]);
                    string strEmitraStatusList = JsonConvert.SerializeObject(dsCitezenData.Tables[1]);
                    model.TicketBookingCancellationList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MISCWLTicketBookingCancellationModel>>(strCitizenData);
                    model.EmitraStatusList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmitraStatus>>(strEmitraStatusList);
                }

                return View(model); 
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View(); 
        } 

        [HttpPost]
        public ActionResult CitizenCancellationDetails(CitizenWildLifeCancellationViewModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var param = model.CWLTicketBookingCancellationModel;
                MISCWLTicketBookingCancellationRepository obj = new MISCWLTicketBookingCancellationRepository();
                var dsCitezenData = obj.GetCitizenCancellationDetails(param.FromDate, param.ToDate, param.RequestID, param.EmitraStatus);

                if (dsCitezenData != null && dsCitezenData.Tables.Count > 0)
                {
                    string strCitizenData = JsonConvert.SerializeObject(dsCitezenData.Tables[0]);
                    string strEmitraStatusList = JsonConvert.SerializeObject(dsCitezenData.Tables[1]);
                    model.TicketBookingCancellationList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MISCWLTicketBookingCancellationModel>>(strCitizenData);
                    model.EmitraStatusList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmitraStatus>>(strEmitraStatusList);
                }

                return View(model);
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View(); 
        }
        #endregion

        #region [GetCitizenWildLifeTicketbookingCancellation]
        public ActionResult GetCitizenWildLifeTicketbookingCancellation(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            { 
                CitizenWildLifeCancellationViewModel model = new CitizenWildLifeCancellationViewModel();
                MISCWLTicketBookingCancellationRepository repo = new MISCWLTicketBookingCancellationRepository();

                var dsCitezenData = repo.GetCitizenCancellationDetailsByID(id);

                if (dsCitezenData != null && dsCitezenData.Tables.Count > 0)
                {
                    string strCitizenData = JsonConvert.SerializeObject(dsCitezenData.Tables[0]);
                    model.TicketBookingCancellationList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MISCWLTicketBookingCancellationModel>>(strCitizenData);

                }

                return PartialView("_PartialCitizenWildLifeCancelDetails", model);
            }
            catch (Exception ex) {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return null;
        }
        #endregion
    }
}
