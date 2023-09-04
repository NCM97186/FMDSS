using FMDSS.Entity.JFMC.ViewModel;
using FMDSS.Models;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Globals;

namespace FMDSS.Controllers.CitizenService.PermissionService.StakeholderService
{
    public class JFMCController : BaseController
    {
        #region Properties & Variables
        private ICommonRepository _commonRepository;
        private IJFMCRepository _JFMCRepository;
        private int ModuleID = 1;
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
        public JFMCController()
        {
            _commonRepository = new CommonRepository();
            _JFMCRepository = new JFMCRepository();
        }
        #endregion

        #region Action Methods
        public ActionResult AddNewRowForMemberDetails(int currentRowIndex, long objectID)
        {
            JFMCRegistration model = new JFMCRegistration();
            Member member = new Member();
            List<Member> lstMember = new List<Member>();
            lstMember.Add(member);
            model.MemberList = lstMember;
            ViewBag.CurrentIndex = currentRowIndex;
            return PartialView("_AddNewRow", model);
        }

        public ActionResult ShowRegistrationDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var data = _JFMCRepository.JFMCRegistration_Get();
                var oDetails = data.Tables[0].AsEnumerable().Select(x => new ViewJFMCRegistration
                {
                    RowNo = x.Field<long>("RowNo"),
                    JFMCRegistrationID = x.Field<long>("RegistrationID"),
                    CommitteeName = x.Field<String>("CommitteeName"),
                    TotalSCCategory = x.Field<int?>("TotalSCCategory"),
                    TotalSTCategory = x.Field<int?>("TotalSTCategory"),
                    TotalFemaleCategory = x.Field<int?>("TotalFemaleCategory"),
                    TotalOthersCategory = x.Field<int?>("TotalOthersCategory"),
                    TotalMember = x.Field<int?>("TotalMember"),
                    ManagedAreaOrEcotourismSiteName = x.Field<String>("ManagedAreaOrEcotourismSiteName"),
                    Latitude = x.Field<String>("Latitude"),
                    Longitude = x.Field<String>("Longitude"),
                    RegistrationNumber = x.Field<String>("RegistrationNumber"),
                    RegistrationDate = x.Field<String>("RegistrationDate"),
                    BankName = x.Field<String>("BankName"),
                    BranchName = x.Field<String>("BranchName"),
                    IFSCCode = x.Field<String>("IFSCCode"),
                    AccountNo = x.Field<String>("AccountNo"),
                    AccountType = x.Field<String>("AccountType"),
                    CorpusFundDeposit = x.Field<decimal?>("CorpusFundDeposit"),
                    TotalRevenueDuringYear = x.Field<decimal?>("TotalRevenueDuringYear"),
                    IsEcotourismManagementExist = x.Field<bool?>("IsEcotourismManagementExist"),
                    Grade = x.Field<String>("Grade"),
                    Audited = x.Field<String>("Audited"),
                    LastAuditDate = x.Field<String>("LastAuditDate"),
                    LatestGeneralBodyMeetingDate = x.Field<String>("LatestGeneralBodyMeetingDate"),
                    LatestExecutiveBodyMeetingDate = x.Field<String>("LatestExecutiveBodyMeetingDate"),
                    ExecutiveCommitteeMemberNames = x.Field<String>("ExecutiveCommitteeMemberNames"),
                    NakaName = x.Field<String>("NakaName"),
                    RangeName = x.Field<String>("RANGE_NAME"),
                    DivisionName = x.Field<String>("DIV_NAME"),
                    CircleName = x.Field<String>("CIRCLE_NAME")
                }).ToList();
                return View(oDetails);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        public ActionResult AddJFMCRegistration()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            JFMCRegistration model = new JFMCRegistration();
            try
            {
                SetDropdownData();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View(model);
        }

        public ActionResult EditJFMCRegistration(long objectID)
        {
            JFMCRegistration model = new JFMCRegistration();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                SetDropdownData();

                var data = _JFMCRepository.JFMCRegistration_GetDetailsForUpdation(objectID);
                model = Util.GetListFromTable<JFMCRegistration>(data, 0).FirstOrDefault();
                model.MemberList = Util.GetListFromTable<Member>(data, 1);

                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }
                return View("AddJFMCRegistration", model);
            }
            catch (Exception ex)
            {
                ViewBag.IsError = 1;
                ViewBag.ReturnMsg = ex.Message;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("AddJFMCRegistration", model);
        }


        [HttpPost]
        public ActionResult AddJFMCRegistration(JFMCRegistration model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var msg = _JFMCRepository.JFMCRegistration_Save(model);
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
                return RedirectToAction("AddJFMCRegistration");
            }
            catch (Exception ex)
            {
                TempData["ReturnMsg"] = ex.Message;
                TempData["IsError"] = true;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("AddJFMCRegistration");
        }
        #endregion

        #region Private Methods
        private void SetDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var data = _commonRepository.GetDropdownData(1, string.Empty);
                ViewBag.CircleCode = _commonRepository.GetDropdownData(1, string.Empty);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        #endregion
    }
}
