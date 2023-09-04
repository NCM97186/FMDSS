//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : WorkOrderInvoiceController
//  Description  : Invoice generation for citizen
//  Date Created : 10-03-2016
//  History      : 
//  Version      : 1.0
//  Author       : Rajkumar Singh
//********************************************************************************************************


using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.GenericClass;
using FMDSS.Models;
using FMDSS.Models.StakeholderService;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using FMDSS.Filters;

namespace FMDSS.Controllers.StakeholderService
{
   [MyAuthorization]
    public class WorkOrderInvoiceController : BaseController
    {
        //
        // GET: /WorkOrderInvoice/
        Int64 UserID = 0;
        WorkInvoice WI = new WorkInvoice();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> WorkOrderList = new List<SelectListItem>();
       /// <summary>
       /// Function To Bind WorkOrder 
       /// </summary>
       /// <param name="ID"></param>
       /// <returns></returns>
        public ActionResult WorkOrderInvoice()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet dsworkOrder = new DataSet();
            dsworkOrder = WI.BindWorkOrder();
            foreach (System.Data.DataRow dr in dsworkOrder.Tables[0].Rows)
            {
                WorkOrderList.Add(new SelectListItem { Text = @dr["WorkOrder_Name"].ToString(), Value = @dr["WorkOrderId"].ToString() });
            }
            ViewBag.ddlWorkOrder = WorkOrderList;
            return View("WorkOrderInvoice", WI);
        }
       /// <summary>
       /// Function to bind milestone on workorder
       /// </summary>
       /// <param name="workordercode"></param>
       /// <returns></returns>
        [HttpPost]
        public JsonResult SelectMileStoneByWorkOderCode(Int64 workordercode)
        {

            DataSet dsMileStone = WI.BindMileStone(workordercode);
            return dtToViewBagJSON(dsMileStone.Tables[0], "MilestoneName", "MilestoneId");
        }
       /// <summary>
       /// Function to bind table on workorder and milestone
       /// </summary>
       /// <param name="workordercode"></param>
       /// <param name="milestonecode"></param>
       /// <returns></returns>
        [HttpPost]
        public JsonResult SelectActivityByMileStone(string workordercode, string milestonecode)
        {
            WorkInvoice workInvoice = null;
            var result = new List<WorkInvoice>();
            DataSet dsActivity = WI.BindActivitydetails(Convert.ToInt64(workordercode), Convert.ToInt64(milestonecode));
            if (dsActivity.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsActivity.Tables[0].Rows)
                {
                    workInvoice = new WorkInvoice()
                    {
                        ActivityName = dr["Activity_Name"].ToString(),
                        SubActivityName = dr["Sub_Activity_Name"].ToString(),
                        ProgressStatus = dr["ProgressStatus"].ToString(),
                        BillVoucherDate = DateTime.Now.ToString("dd/MM/yyy"),
                        BillVoucherAmount = dr["FinancialTarget"].ToString(),
                    };
                    result.Add(workInvoice);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       /// <summary>
       /// Sub function to bind dataset on column nane and ID
       /// </summary>
       /// <param name="dt"></param>
       /// <param name="TextField"></param>
       /// <param name="ValueField"></param>
       /// <returns></returns>
        public JsonResult dtToViewBagJSON(DataTable dt, string TextField, string ValueField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr[TextField].ToString(), Value = @dr[ValueField].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }

       /// <summary>
       /// Function to generate invoice in pdf
       /// </summary>
       /// <param name="fm"></param>
       /// <param name="command"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult PdfInvoice(FormCollection fm, string command)
        {
            DataSet dswo = new WorkInvoice().BindWorkOrder();
            DataSet dsms = new WorkInvoice().BindMileStone(Convert.ToInt64(Request.Form["ddlWorkOrder"]));
            WorkInvoice woi = new WorkInvoice();
            string filepath = string.Empty;

            for (int i = 0; i < dswo.Tables[0].Rows.Count; i++)
            {
                if (Request.Form["ddlWorkOrder"] == dswo.Tables[0].Rows[i]["WorkOrderId"].ToString())
                {
                    woi.WorkOrderID = dswo.Tables[0].Rows[i]["WorkOrder_Name"].ToString();
                }
            }

            for (int i = 0; i < dsms.Tables[0].Rows.Count; i++)
            {
                if (Request.Form["ddlMilestone"] == dsms.Tables[0].Rows[i]["MilestoneId"].ToString())
                {
                    woi.MilestoneName = dsms.Tables[0].Rows[i]["MilestoneName"].ToString();
                }
            }
           
            if (command == "Invoice")
            {
                filepath = "~/FixedLandDocument/research_" + DateTime.Now.Ticks.ToString() + ".pdf";
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                var FontColour = new BaseColor(0, 0, 0);
                Paragraph tableheading = null;
                var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);

                doc.Open();
                doc.NewPage();
                tableheading = new Paragraph("Government of Rajasthan,Forest Department", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_RIGHT);
                doc.Add(tableheading);

                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Work Order :" + woi.WorkOrderID, MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);

                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Mile Stone :" + woi.MilestoneName, MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);

                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Bill Voucher No :" + fm["BillVoucherNo"], MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Bill Voucher Date :" + fm["BillVoucherDate"], MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Bill Amount :" + fm["BillVoucherAmount"], MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("For any query, contact us at <> ", MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Close();
                if (System.IO.File.Exists(Server.MapPath(filepath)))
                {
                    string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(FilePath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
            }
            return RedirectToAction("WorkOrderInvoice", "WorkOrderInvoice");
        }

    }
}
