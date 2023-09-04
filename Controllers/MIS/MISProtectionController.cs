using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Models;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace FMDSS.Controllers.MIS
{
    public class MISProtectionController : BaseController
    {
        #region Properties & Variables
        int ModuleID = 4;
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
        public MISProtectionController()
        {
            _commonRepository = new CommonRepository();
            _ProtectionRepository = new ProtectionRepository();
        }
        #endregion

        #region [OffenceReport]
        public ActionResult OffenceReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult OffenceReport(OffenceReportVM param)
        {
            DataTable dt = _ProtectionRepository.OffenceDetails_Report(param);
            if (dt != null)
            {
                var oDetails = Globals.Util.GetListFromTable<ViewOffenceDetails>(dt);
                return PartialView("_OffenceReport", oDetails);
            }
            return null;
        }
        #endregion

        #region [OffenceSummaryReport]
        public ActionResult OffenceSummaryReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult OffenceSummaryReport(OffenceReportVM param)
        {
            OffenceSummaryReportVM model = new OffenceSummaryReportVM();
            DataSet ds = _ProtectionRepository.OffenceSummary_Report(param);
            if (Globals.Util.isValidDataSet(ds))
            {
                var oDetails = Globals.Util.GetListFromTable<OffenceSummaryDetailsReportVM>(ds, 0);
                model = Globals.Util.GetListFromTable<OffenceSummaryReportVM>(ds, 1).FirstOrDefault();
                model.OffenceSummaryDetailsReportList = oDetails;
                return PartialView("_OffenceSummaryReport", model);
            }
            return null;
        }

        public ActionResult OffenceSummarySubReport(OffenceSubParamVM param)
        {
            DataSet ds = _ProtectionRepository.OffenceSummarySub_Report(param);
            if (ds.Tables.Count > 0)
            {
                var oDetails = Globals.Util.GetListFromTable<ViewOffenceDetails>(ds, 0);
                return PartialView("_OffenceReportSummarySubReport", oDetails);
            }
            return null;
        }

        #endregion

        #region [OffenceSummaryQuarterReport]
        public ActionResult OffenceSummaryQuarterReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult OffenceSummaryQuarterReport(OffenceReportVM param)
        {
            DataSet ds = _ProtectionRepository.OffenceSummaryQtr_Report(param);
            if (Globals.Util.isValidDataSet(ds))
            {
                var oDetails = Globals.Util.GetListFromTable<OffenceSummaryQtrReportVM>(ds, 0);
                return PartialView("_OffenceSummaryQuarterReport", oDetails);
            }
            return null;
        }

        public ActionResult OffenceSummaryQuarterSubReport(OffenceSubParamVM param)
        {
            DataSet ds = _ProtectionRepository.OffenceSummaryQtrSub_Report(param);
            if (ds.Tables.Count > 0)
            {
                var oDetails = Globals.Util.GetListFromTable<ViewOffenceDetails>(ds, 0);
                return PartialView("_OffenceSummaryQuarterSubReport", oDetails);
            }
            return null;
        }

        #endregion

        #region [EncroachmentSummaryReport]
        public ActionResult EncroachmentSummaryReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult EncroachmentSummaryReport(OffenceReportVM param)
        {
            EncroachmentSummaryReportVM model = new EncroachmentSummaryReportVM();
            DataSet ds = _ProtectionRepository.EncroachmentSummary_Report(param);
            if (Globals.Util.isValidDataSet(ds))
            {
                var oDetails = Globals.Util.GetListFromTable<EncroachmentSummaryDetailsReportVM>(ds, 0);
                model = Globals.Util.GetListFromTable<EncroachmentSummaryReportVM>(ds, 1).FirstOrDefault();
                model.EncroachmentSummaryDetailsReportList = oDetails;
                return PartialView("_EncroachmentSummaryReport", model);
            }
            return null;
        }

        public ActionResult EncroachmentSummarySubReport(OffenceSubParamVM param)
        {
            DataSet ds = _ProtectionRepository.EncroachmentSummarySub_Report(param);
            if (ds.Tables.Count > 0)
            {
                var oDetails = Globals.Util.GetListFromTable<ViewEncroachmentDetails>(ds, 0);
                return PartialView("_EncroachmentReportSummarySubReport", oDetails);
            }
            return null;
        }
        #endregion

        #region [Private Methods]
        private void SetDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var dsData = _commonRepository.GetDropdownData2(14, string.Empty);
                ViewBag.FinacialYearList = _commonRepository.GetDropdownData(13, string.Empty);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    ViewBag.CircleList = dsData.Tables[0].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("CIRCLE_CODE"),
                        Text = x.Field<string>("CIRCLE_NAME")
                    });

                    if (Globals.Util.isValidDataSet(dsData, 1, true))
                    {
                        ViewBag.DivList = dsData.Tables[1].AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<string>("DIV_CODE"),
                            Text = x.Field<string>("DIV_NAME")
                        });

                        if (Globals.Util.isValidDataSet(dsData, 2, true))
                        {
                            ViewBag.RangeList = dsData.Tables[2].AsEnumerable().Select(x => new SelectListItem
                            {
                                Value = x.Field<string>("RANGE_CODE"),
                                Text = x.Field<string>("RANGE_NAME")
                            });

                            if (Globals.Util.isValidDataSet(dsData, 3, true))
                            {
                                ViewBag.NakaList = dsData.Tables[3].AsEnumerable().Select(x => new SelectListItem
                                {
                                    Value = Convert.ToString(x.Field<long>("NakaID")),
                                    Text = x.Field<string>("NakaName")
                                });
                            }
                        }
                    }
                }
                var data = _ProtectionRepository.OffenceDetails_GetDropdownData();
                ViewBag.OffenceCategory = GetDropdownData(2, data.Tables[1]);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
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
        #endregion
    }
}
