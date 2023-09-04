//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Notice Number Apply for Research
//  Date Created : 12-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using FMDSS.Models;
using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Admin
{
    public class DepotController : BaseController
    {
        //
        // GET: /Depot/
        Location location = new Location();
        List<SelectListItem> items = new List<SelectListItem>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult DepotForm()
        {
            #region Region
            DataTable dt = new DataTable();
            dt = location.BindRegion();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["REG_NAME"].ToString(), Value = @dr["REG_CODE"].ToString() });
            }

            ViewBag.RegionCode = items;
            // ViewBag.ToLocation = items;
            #endregion

            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitDepotForm(FormCollection fm, string Command)
        {
            Session["Depot_Status"] = null;

            try
            {

                Depot depot = new Depot();

                depot.DepotId = 0;

                if (Session["UserId"] != null)
                {
                    depot.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }

                if (fm["DepotType"].ToString() == "")
                {
                    depot.DepotType = "";
                }
                else
                {
                    depot.DepotType = fm["DepotType"].ToString();
                }

                if (fm["RegionCode"].ToString() == "")
                {
                    depot.RegionCode = "";
                }
                else
                {
                    depot.RegionCode = fm["RegionCode"].ToString();
                }
                if (fm["CircleCode"].ToString() == "")
                {
                    depot.CircleCode = "";
                }
                else
                {
                    depot.CircleCode = fm["CircleCode"].ToString();
                }

                if (fm["DivisionCode"].ToString() == "")
                {
                    depot.DivisionCode = "";
                }
                else
                {
                    depot.DivisionCode = fm["DivisionCode"].ToString();
                }

                if (fm["RangeCode"].ToString() == "")
                {
                    depot.RangeCode = "";
                }
                else
                {
                    depot.RangeCode = fm["RangeCode"].ToString();
                }

                if (fm["DepotName"].ToString() == "")
                {
                    depot.DepotName = "";
                }
                else
                {
                    depot.DepotName = fm["DepotName"].ToString();
                }

                if (fm["DepotIncharge"].ToString() == "")
                {
                    depot.DepotIncharge = "";
                }
                else
                {
                    depot.DepotIncharge = fm["DepotIncharge"].ToString();
                }

               
                if (Command == "Save")
                {
                    Int64 status = depot.AddDepotData();
                    if (status > 0)
                    {
                        Session["Depot_Status"] = "Depot Created Sucessfully";
                    }
                    else
                    {
                        Session["Depot_Status"] = "Not inserted";
                    }


                }
                return RedirectToAction("DepotForm", "Depot");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
