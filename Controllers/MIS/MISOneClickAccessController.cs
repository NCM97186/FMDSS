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
    public class MISOneClickAccessController : BaseController
    {
        #region Properties & Variables
        int ModuleID = 4;
        private ICommonRepository _commonRepository;
        private IOneClickAccessRepository _repository;
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
        public MISOneClickAccessController()
        {
            _commonRepository = new CommonRepository();
            _repository = new OneClickAccessRepository();
        }
        #endregion

        #region [DODReport] 

        #region [DODAuctionWinnerReport]
        public ActionResult OneClickLogReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OneClickLogReport(OffenceReportVM param)
        {
            DataTable dt = _repository.OneClickLog_Report(param);
            if (Globals.Util.isValidDataTable(dt))
            {
                var oDetails = Globals.Util.GetListFromTable<Entity.UserManagement.ViewModel.OneClickAccessLogReportVM>(dt);
                return PartialView("_OneClickLogReport", oDetails);
            }
            return null;
        }
        #endregion 

        #endregion 
    }
}
