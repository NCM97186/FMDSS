using FMDSS.E_SignIntegration;
using FMDSS.Entity;
using FMDSS.Entity.DOD.ViewModel;
using FMDSS.Globals;
using FMDSS.Repository.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.Repository
{
    public class OneClickAccessRepository : IOneClickAccessRepository
    {
        #region Properties & Variables 
        private FMDSS.Models.DAL _db = new Models.DAL();
        #endregion

        #region OneClickAccess Report
        public DataTable OneClickLog_Report(Entity.Protection.ViewModel.OffenceReportVM model, int actionCode=1)
        {
            DataTable dtData = new DataTable();
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode), 
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate), 
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_UM_rpt_OneClickAccess", prms);
            return dtData;
        }
        #endregion
    }
}