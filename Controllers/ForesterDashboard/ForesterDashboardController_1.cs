using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForesterAction;
using FMDSS.Filters;


namespace FMDSS.Controllers.Dashboard
{
      [MyAuthorization]
    public class ForesterDashboardController : BaseController
    {
        ActionRequest actionRequest = new ActionRequest();
        List<ActionRequest> actionRequestList2 = new List<ActionRequest>();
        // GET: /ForestDashboard/

        public ActionResult ForesterDashboard()
        {           
            DataSet dtf = actionRequest.BindAllServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("1"));
            for (int i = 0; i < dtf.Tables.Count; i++)
            {
                foreach (DataRow dr in dtf.Tables[i].Rows)
                {
                    actionRequestList2.Add(
                        new ActionRequest()
                        {
                            RequestId = dr["Requestedid"].ToString(),
                            ServiceType = dr["ServiceTypeDesc"].ToString(),
                            PermissionName = dr["PermissionDesc"].ToString(),
                            SubPermissionName = dr["SubPermissionDesc"].ToString(),
                            RequestedOn = dr["DurationFrom"].ToString(),
                            Status = dr["StatusDesc"].ToString(),
                            TableName = dr["TableName"].ToString(),
                            ModuleId = Convert.ToInt32(dr["ModuleId"]),
                            ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                            PermissionId = Convert.ToInt32(dr["PermissionId"]),
                            SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                            TransactionStatus = dr["trn_status_Desc"].ToString()
                        });
                }
            }
            ViewData["actionRequestList2"] = actionRequestList2;

            return View();
        }

    }
}
