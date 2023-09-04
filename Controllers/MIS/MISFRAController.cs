using FMDSS.Entity.FRAViewModel;
using FMDSS.Entity.NPVM;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS.Controllers.MIS
{
    public class MISFRAController : BaseController
    {
        #region [Properties & Variables]
        private IClaimRequestRepository _repository;
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
        public MISFRAController()
        {
            _repository = new ClaimRequestRepository();
        }
        #endregion

        //FMDSS.Models.DAL dl = new Models.DAL();

        #region Reports
        #region Claim Request Details Report
        public ActionResult ClaimRequestReport()
        {
            Session.Remove("ClaimRequestReport");
            DataSet data = _repository.GetDropdownData(5);
            if (Globals.Util.isValidDataSet(data, 0))
            {
                ViewBag.DistList = data.Tables[0].AsEnumerable().Select(x => new DropDownListVM
                {
                    Value = x.Field<long>("DistrictID"),
                    Text = x.Field<string>("DistrictName")
                }).ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult ClaimRequestReport(ClaimRequestParamVM param)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DataSet ds = new DataSet();
            if (Convert.ToString(Session["ClaimRequestReportAdmin"]) == "ClaimRequestReportAdmin")
            {
                ds = _repository.GetClaimRequestDetails_RptAdmin(param);
                Session["ClaimRequestReport"] = ds.Tables[0];
            }
            else
            {
                ds = _repository.GetClaimRequestDetails_Rpt(param);
                Session["ClaimRequestReport"] = ds.Tables[0];
            }

            if (Globals.Util.isValidDataSet(ds, 0))
            {
                var claimRequestDetails = Globals.Util.GetListFromTable<ClaimRequestDetailsVM>(ds, 0);
                return PartialView("_ClaimRequestReport", claimRequestDetails);
            }

            return null;
        }

        public ActionResult ClaimRequestReportAdmin()
        {
            Session["ClaimRequestReportAdmin"] = "ClaimRequestReportAdmin";
            return View("ClaimRequestReport");
        }


        #endregion

        #region Claim Request Summary Report
        public ActionResult ClaimRequestSummaryReport()
        {
            DataSet data = _repository.GetDropdownData(5);
            if (Globals.Util.isValidDataSet(data, 0))
            {
                ViewBag.DistList = data.Tables[0].AsEnumerable().Select(x => new DropDownListVM
                {
                    Value = x.Field<long>("DistrictID"),
                    Text = x.Field<string>("DistrictName")
                }).ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult ClaimRequestSummaryReport(ClaimRequestParamVM param)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            DataSet ds = new DataSet();
            if (Convert.ToString(Session["ClaimRequestSummaryReport"]) == "ClaimRequestSummaryReport")
            {
                ds = _repository.GetClaimRequestSummary_RptAdmin(param);
            }
            else
            {
                ds = _repository.GetClaimRequestSummary_Rpt(param);
            }

            if (Globals.Util.isValidDataSet(ds, 0))
            {
                var claimRequestSummaryDetails = Globals.Util.GetListFromTable<ClaimRequestSummaryVM>(ds, 0);

                Session["ClaimRequestSummaryReportExport"] = claimRequestSummaryDetails;
                return PartialView("_ClaimRequestSummaryReport", claimRequestSummaryDetails);
            }
            return null;
        }

        public ActionResult ClaimRequestSummarySubReport(ClaimRequestSubParamVM param)
        {
            DataSet ds = _repository.GetClaimRequestSummarySub_Rpt(param);

            if (Globals.Util.isValidDataSet(ds, 0))
            {
                var claimRequestDetails = Globals.Util.GetListFromTable<ClaimRequestDetailsVM>(ds, 0);

                return PartialView("_ClaimRequestSummarySubReport", claimRequestDetails);
            }
            return null;
        }

        public ActionResult ClaimRequestSummaryReportExport()
        {
            List<ClaimRequestSummaryVM> dtf = (List<ClaimRequestSummaryVM>)Session["ClaimRequestSummaryReportExport"];
            if (dtf != null)
            {

                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("dd/MM/yyyy") + "_ClaimRequestReport.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult exportRequestSummaryReportPDF(string gridHTML)
        {
            StringReader reader = null;
            Document pdfDoc = null;
            PdfWriter write = null;
            try
            {
                using (MemoryStream memorystream = new MemoryStream())
                {
                    reader = new StringReader(gridHTML);
                    pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 5f);
                    write = PdfWriter.GetInstance(pdfDoc, memorystream);
                    pdfDoc.Open();
                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(write, pdfDoc, reader);
                    pdfDoc.Close();
                    return File(memorystream.ToArray(), "application/pdf", Convert.ToString("RequestSummaryReport_" + DateTime.Now.ToShortDateString() + ".pdf"));
                }

            }
            finally
            {
                reader.Dispose();
                pdfDoc.Dispose();
                write.Dispose();
            }
        }

        #endregion
        #region for excel and PDF generate
        public FileResult GetPDFReportClaimRequest()
        {
            string rootPath = Server.MapPath("~/");
            DataTable dtf = (DataTable)Session["ClaimRequestReport"];
            string filpath = GetPDFSample(rootPath, dtf);
            string ReportURL = filpath;
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }

        public string GetPDFSample(string rootPath, DataTable dt)
        {

            string docName = string.Empty;
            docName = "CliamRequestReport.pdf";
            string filepath = string.Empty;

            filepath = System.Web.HttpContext.Current.Server.MapPath("~/Documents/" + docName);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.FileInfo fi_del = new System.IO.FileInfo(filepath);
                try
                {
                    fi_del.Delete();
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
            }


            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);

            string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Globals.Util.GetAppSettings("FontFileName"));
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);
            // iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 14, iTextSharp.text.Font.BOLD);

            iTextSharp.text.Font Myfont = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            var subheadfontdata = FontFactory.GetFont("Arial", 7, FontColour);
            doc.Open();
            doc.NewPage();

            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cells = new PdfPCell() { Border = 0 };
            Details.TotalWidth = 120;
            Details.SetTotalWidth(new float[] { 20f, 0f, 0f });



            cells = new PdfPCell(new Phrase("Tribal Area Development Department Udaipur, Rajasthan", Myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("tutkfr {ks=h; fodkl foÒkx mn;iqj] jktLFkku", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Date:" + DateTime.Now.ToString("dd/MM/yyyy"), fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            doc.Add(Details);

            PdfPTable TableData = new PdfPTable(14) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellD = new PdfPCell() { Border = 12 };
            TableData.TotalWidth = 120;
            TableData.SetTotalWidth(new float[] { 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f });

            cellD = new PdfPCell(new Phrase("S.No.", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Claim Request ID", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Date On Which Claim Was Raised", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Claimant Name", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Spouse Name", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Father Name", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Claim Type", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Mobile Number", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("District", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Gram Panchayat", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Village", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Khasra No", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Status", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            cellD = new PdfPCell(new Phrase("Pending At", subheadfont));
            cellD.HorizontalAlignment = Element.ALIGN_LEFT;
            TableData.AddCell(cellD);
            int k = 0;
            foreach (DataRow DR in dt.Rows)
            {
                k += Convert.ToInt16(1);
                cellD = new PdfPCell(new Phrase(Convert.ToString(k), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["ClaimRequestIDWithPrefix"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["RaisedOn"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["ClaimantName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["SpouseName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["FatherName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["ClaimTypeName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["Mobile"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["DistrictName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["GPName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["VillageName"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["KhasraNumber"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["CurrentStatus"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);
                cellD = new PdfPCell(new Phrase(Convert.ToString(DR["PendingAt"]), subheadfontdata));
                cellD.HorizontalAlignment = Element.ALIGN_LEFT;
                TableData.AddCell(cellD);

            }
            doc.Add(TableData);

            doc.Close();
            return filepath;
            // System.Diagnostics.Process.Start(filepath);
        }

        public ActionResult ClaimReuestReportExport()
        {
            DataTable dtf = (DataTable)Session["ClaimRequestReport"];
            //#region Test
            //DataRow newRow = dtf.NewRow();
            //newRow.c
            //newRow[0] = "Hi";
            //#endregion
            if (dtf != null)
            {
                try
                {
                    dtf.Columns.Remove("ClaimRequestDetailsID");
                    dtf.Columns.Remove("IsHalkaPatwariGenerated");
                    dtf.Columns.Remove("IsForesterGenerated");
                    dtf.Columns.Remove("ClaimTypeID");
                    dtf.Columns.Remove("BlockName");
                    dtf.Columns.Remove("VillageCode");
                    dtf.Columns.Remove("RequesterComment");
                    dtf.Columns.Remove("CurrentApproverDesignationID");
                }
                catch { }
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("dd/MM/yyyy") + "_ClaimReuestReport.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }
        #endregion
        #endregion
    }
}
