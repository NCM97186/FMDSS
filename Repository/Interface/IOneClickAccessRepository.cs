using FMDSS.Entity;
using FMDSS.Entity.DOD.ViewModel;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FMDSS.Repository.Interface
{
    public interface IOneClickAccessRepository
    {
        #region OneClickAccess Report
        DataTable OneClickLog_Report(Entity.Protection.ViewModel.OffenceReportVM model, int actionCode = 1);
        #endregion
    }
}