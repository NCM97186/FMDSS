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
    public class MISDODController : BaseController
    {
        #region Properties & Variables
        int ModuleID = 4;
        private ICommonRepository _commonRepository;
        private IAuctionRequestRepository _AuctionRequestRepository;
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
        public MISDODController()
        {
            _commonRepository = new CommonRepository();
            _AuctionRequestRepository = new AuctionRequestRepository();
        }
        #endregion

        #region [DODReport]

        #region [DODInventoryReport]
        public ActionResult DODInventoryReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult DODInventoryReport(OffenceReportVM param)
        {
            DataTable dt = _AuctionRequestRepository.DODInventory_Report(param);
            if (dt != null)
            {
                var oDetails = Globals.Util.GetListFromTable<Models.ForestDevelopment.DODProductDetailsForReport>(dt);
                return PartialView("_DODInventoryReport", oDetails);
            }
            return null;
        }
        #endregion 

        #region [DODNoticeReport]
        public ActionResult DODNoticeReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult DODNoticeReport(OffenceReportVM param)
        {
            DataTable dt = _AuctionRequestRepository.DODInventory_Report(param, 2);
            if (dt != null)
            {
                var oDetails = Globals.Util.GetListFromTable<Models.ForestDevelopment.DODNoticeDetailsForReport>(dt);
                return PartialView("_DODNoticeReport", oDetails);
            }
            return null;
        }
        #endregion

        #region [DODAuctionWinnerReport]
        public ActionResult DODAuctionWinnerReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult DODAuctionWinnerReport(OffenceReportVM param)
        {
            DataTable dt = _AuctionRequestRepository.DODInventory_Report(param, 3);
            if (dt != null)
            {
                var oDetails = Globals.Util.GetListFromTable<Models.ForestDevelopment.DODAuctionWinnerDetailsForReport>(dt);
                return PartialView("_DODAuctionWinnerReport", oDetails);
            }
            return null;
        }
        #endregion

        #region [DODAppliedAuctionReport]
        public ActionResult DODAppliedAuctionReport()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        [HttpPost]
        public ActionResult DODAppliedAuctionReport(OffenceReportVM param)
        {
            DataTable dt = _AuctionRequestRepository.DODInventory_Report(param, 4);
            if (dt != null)
            {
                var oDetails = Globals.Util.GetListFromTable<Models.ForestDevelopment.AuctionTransactionForReport>(dt);
                return PartialView("_DODAppliedAuctionReport", oDetails);
            }
            return null;
        }
        #endregion

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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }
        #endregion
    }
}
