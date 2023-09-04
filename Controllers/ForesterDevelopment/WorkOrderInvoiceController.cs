using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.GenericClass;
using FMDSS.Models;
using FMDSS.Models.ForesterDevelopment;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net;
using FMDSS.Models.StakeholderService;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class WorkOrderInvoiceController : BaseController
    {
        //
        // GET: /WorkOrderInvoice/
        Int64 UserID = 0;

        WorkInvoice WI = new WorkInvoice();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> WorkOrderList = new List<SelectListItem>();
        List<WorkInvoice> ddlInvoiceList = new List<WorkInvoice>();
        public ActionResult WorkOrderInvoice(string ID)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataSet dsworkOrder = new DataSet();
            dsworkOrder = WI.BindWorkOrder();
            foreach (System.Data.DataRow dr in dsworkOrder.Tables[0].Rows)
            {
                WorkOrderList.Add(new SelectListItem { Text = @dr["WorkOrder_Name"].ToString(), Value = @dr["WorkOrderId"].ToString() });
            }
            ViewBag.ddlWorkOrder = WorkOrderList;

            DataSet dtf2 = WI.GetInvoiceList(Session["SSOID"].ToString());

            for (int i = 0; i < dtf2.Tables.Count; i++)
            {
                foreach (DataRow dr in dtf2.Tables[0].Rows)
                    ddlInvoiceList.Add(
                        new WorkInvoice()

                        {
                            MilestoneID = dr["ID"].ToString(),
                            WorkOrderID = dr["WorkOrder_Code"].ToString(),
                            WorkOrder_Desc = dr["WorkOrder_Name"].ToString(),
                            MilestoneName = dr["MilestoneName"].ToString(),
                            MilestonePaymentPercent = dr["MilestonePaymentPercentage"].ToString(),
                            BillVoucherAmount = dr["BillAmount"].ToString(),
                            BillVoucherDate = dr["BillDate"].ToString(),
                            ProgressStatus = dr["StatusDesc"].ToString(),
                            BillVoucherNo = dr["BillNo"].ToString(),
                        });

            }


            ViewData["ddlInvoiceList"] = ddlInvoiceList;


            return View("WorkOrderInvoice", WI);
        }





        [HttpPost]
        public JsonResult SelectMileStoneByWorkOderCode(Int64 workordercode)
        {

            DataSet dsMileStone = WI.BindMileStone(workordercode);
            return dtToViewBagJSON(dsMileStone.Tables[0], "MilestoneName", "MilestoneId");
        }

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


        [HttpPost]
        public ActionResult PdfInvoice(FormCollection fm, string command)
        {
            //DataSet dswo = new WorkInvoice().BindWorkOrder();
            //DataSet dsms = new WorkInvoice().BindMileStone(Convert.ToInt64(Request.Form["ddlWorkOrder"]));
            //WorkInvoice woi = new WorkInvoice();
            //string filepath = string.Empty;


            WorkInvoice workInvoice = new WorkInvoice();
            workInvoice.MilestoneID = fm["ddlMilestone"].ToString();
            workInvoice.BillVoucherAmount = fm["BillVoucherAmount"].ToString();
            workInvoice.BillVoucherDate = fm["BillVoucherDate"].ToString();
            workInvoice.UserID = Session["UserId"].ToString();

            int k = workInvoice.SaveInvoiceDetails(Convert.ToDouble(workInvoice.BillVoucherAmount), workInvoice.BillVoucherDate, Convert.ToInt64(workInvoice.MilestoneID), Convert.ToInt64(workInvoice.UserID));
            TempData["InvoiceMsg"] = k;
            if (k > 0)
            {
                TempData["InvoiceMsgDesc"] = "Data saved successfully";
            }
            else
            {
                TempData["InvoiceMsgDesc"] = "Data not save.";
            }
            return RedirectToAction("WorkOrderInvoice", "WorkOrderInvoice");
        }

        public ActionResult PrintInvoice(string workOrderID, string MileStoneID)
        {
               WorkInvoice workInvoice = new WorkInvoice();
               workInvoice.MilestoneID = MileStoneID;       
               DataSet DS = workInvoice.GetPDFdetail(Convert.ToInt64(workInvoice.MilestoneID));
               DataTable DT1 = new DataTable();
               DT1 = DS.Tables[0];


              string filepath = "~/FixedLandDocument/InvoiceBill" + DateTime.Now.Ticks.ToString() + ".pdf";
               Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
               PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
               var FontColour = new BaseColor(0, 0, 0);
               Paragraph tableheading = null;
               Paragraph sideheading = null;
               Phrase colHeading;
               PdfPCell cell;
               PdfPTable pdfTable = null;
               var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
               var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);

               doc.Open();
               doc.NewPage();

               tableheading = new Paragraph("Bill voucher details", MyFont);
               tableheading.Font.Size = 11;
               tableheading.Alignment = (Element.ALIGN_CENTER);
               doc.Add(tableheading);
               doc.Add(new Paragraph(Environment.NewLine));
               pdfTable = new PdfPTable(8);
               pdfTable.DefaultCell.Padding = 1;
               pdfTable.WidthPercentage = 95;
               pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
               int count = DT1.Rows.Count;
               DT1.Columns.Remove("ID");
               //   DT2.Columns.Remove("KhasraNo");
               DT1.AcceptChanges();
               string[,] arrPdfData = new string[count, 8];
               arrPdfData[0, 0] = "Workorder ID";
               arrPdfData[0, 1] = "WorkOrder Desc";
               arrPdfData[0, 2] = "Milestone";
               arrPdfData[0, 3] = "PaymentPercentage";
               arrPdfData[0, 4] = "Amount";
               arrPdfData[0, 5] = "Date";
               arrPdfData[0, 6] = "Status ";
               arrPdfData[0, 7] = "BillNo ";

               pdfTable.AddCell("Workorder ID");
               pdfTable.AddCell("WorkOrder Desc");
               pdfTable.AddCell("Milestone");
               pdfTable.AddCell("PaymentPercentage");
               pdfTable.AddCell("Amount");
               pdfTable.AddCell("Date");
               pdfTable.AddCell("Status");
               pdfTable.AddCell("BillNo");
               for (int xid = 0; xid < count; xid++)
               {
                   for (int yid = 0; yid < 8; yid++)
                   {
                       arrPdfData[xid, yid] = DT1.Rows[xid][yid].ToString();
                       colHeading = new Phrase(arrPdfData[xid, yid]);
                       colHeading.Font.Size = 10;
                       cell = new PdfPCell(new Phrase(colHeading));
                       cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                       cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                       pdfTable.AddCell(cell);
                   }
               }
               doc.Add(pdfTable);
               doc.Close();
               if (System.IO.File.Exists(Server.MapPath(filepath)))
               {
                   //ProcessStartInfo startInfo = new ProcessStartInfo
                   //{
                   //    Arguments = Server.MapPath(filepath),
                   //    FileName = "explorer.exe"
                   //};
                   //Process.Start(startInfo);
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
               return RedirectToAction("WorkOrderInvoice", "WorkOrderInvoice");
          //  return View("WorkOrderInvoice", "WorkOrderInvoice");
        }

    }
}
