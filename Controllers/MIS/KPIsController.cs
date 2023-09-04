using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.MIS
{
    public class KPIsController : BaseController
    {
        //
        // GET: /KIPs/getKPIdata
         CitizenModel Model = new CitizenModel();

        public JsonResult ShowKPIs(string ActionType)
        {

            List<KPIs> items = new List<KPIs>();
            try
            {
                if ((!String.IsNullOrEmpty(ActionType)))
                {
                    DataTable dt = Model.FetchKPIsData(Convert.ToString(ActionType));

                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            items.Add(new KPIs { value = Convert.ToString(dr["TotalCount"]), unitCode = Convert.ToString(dr["Code"]) });
                        }
                }
            }
            catch (Exception ex)
            {
               
            }

            return Json(items);
        }


    }
}
