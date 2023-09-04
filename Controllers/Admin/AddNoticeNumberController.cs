//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Notice Number
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@

using FMDSS.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Filters;
using FMDSS.Controller;
namespace FMDSS.Controllers.Admin
{
    [MyAuthorization]
    public class AddNoticeNumberController : BaseController
    {
       
        //
        // GET: /AddNoticeNumber/

        Location location = new Location();
        List<SelectListItem> items = new List<SelectListItem>();
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public ActionResult AddNoticeNumber()
        //{
        //    #region Division
        //    DataTable dt = new DataTable();
        //    dt = location.BindDivision();
        //    ViewBag.fname = dt;
        //    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
        //    {
        //        items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
        //    }

        //    ViewBag.DivisionCode = items;
        //    // ViewBag.ToLocation = items;
        //    #endregion

        //    //Create Notice Number

        //    DateTime now = DateTime.Now;
        //    string noticenNo = now.Ticks.ToString();
        //    ViewData["Notice_No"] = noticenNo;

        //    return View();
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="divisionId"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult getRange(string divisionCode)
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    if (!String.IsNullOrEmpty(divisionCode))
        //    {

        //        DataTable dt = location.BindRange(divisionCode);
        //        ViewBag.fname = dt;
        //        foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
        //        {
        //            items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
        //        }

        //    }
            
           
        //    return Json(new SelectList(items, "Value", "Text"));
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rangeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getVillage(string divisionCode, string rangeCode)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(divisionCode) && !String.IsNullOrEmpty(rangeCode))
            {
                DataTable dt = location.BindVillage(divisionCode, rangeCode);
                ViewBag.fname = dt;
               

                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                }
            }

           
            return Json(new SelectList(items, "Value", "Text"));
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="divCode"></param>
       /// <param name="ranCode"></param>
       /// <param name="villCode"></param>
       /// <returns></returns>

        //[HttpPost]
        //public JsonResult getDepotData(string divCode, string ranCode, string villCode)
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    if (!String.IsNullOrEmpty(divCode) && !String.IsNullOrEmpty(ranCode) && !String.IsNullOrEmpty(villCode))
        //    {
        //        DataTable dt = location.BindDepot(divCode, ranCode, villCode);
        //        ViewBag.fname = dt;


        //        foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
        //        {
        //            items.Add(new SelectListItem { Text = @dr["Depot_Location"].ToString(), Value = @dr["Depot_Id"].ToString() });
        //        }
        //    }


        //    return Json(new SelectList(items, "Value", "Text"));
        //}

        [HttpPost]
        public ActionResult SubmitNoticeForm(FormCollection fm, string Command)
        {
            ActionResult actionResult = null;
            try
            {
                Notice notice = new Notice();
               
                //if (Session["SSODetail"] != null)
                //{
                //    user = (User)Session["SSODetail"];
                //    notice.CreatedBy = user.UserId;
                //}

                if (fm["NoticeNo"].ToString() == "")
                {
                    notice.NoticeNo = "";
                }
                else
                {
                    notice.NoticeNo = fm["NoticeNo"].ToString();
                }

                if (fm["DivisionCode"].ToString() == "")
                {
                    notice.DivisionCode = "";
                }
                else
                {
                    notice.DivisionCode = fm["DivisionCode"].ToString();
                }

                if (fm["RangeCode"].ToString() == "")
                {
                    notice.RangeCode = "";
                }
                else
                {
                    notice.RangeCode = fm["RangeCode"].ToString();
                }

                if (fm["VillageCode"].ToString() == "")
                {
                    notice.VillageCode = "";
                }
                else
                {
                    notice.VillageCode = fm["VillageCode"].ToString();
                }
                if (fm["DepotId"].ToString() == "")
                {
                    notice.DepotId = 0;
                }
                else
                {
                    notice.DepotId = Convert.ToInt64(fm["DepotId"].ToString());
                }
                if (fm["ForestProduce"].ToString() == "")
                {
                    notice.ForestProduce = "";
                }
                else
                {
                    notice.ForestProduce = fm["ForestProduce"].ToString();
                }
                if (fm["Qty"].ToString() == "")
                {
                    notice.Qty = "";
                }
                else
                {
                    notice.Qty = fm["Qty"].ToString();
                }
                if (fm["ReservedPrice"].ToString() == "")
                {
                    notice.ReservedPrice = 0;
                }
                else
                {
                    notice.ReservedPrice = Convert.ToDecimal(fm["ReservedPrice"].ToString());
                }
               
                if (Command == "Save")
                {
                    Int64 status = notice.CreateNotice();
                    if (!String.IsNullOrEmpty(status.ToString()))
                    {
                        Session["Status"] = "Notice No. Created Successfully";
                    }
                    else
                    {
                        Session["Status"] = "Not inserted";
                    }

                    actionResult = RedirectToAction("AddNoticeNumber", "AddNoticeNumber");
                }

                if (Command == "Cancel")
                {
                    actionResult = RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actionResult;

        }

    }
}
