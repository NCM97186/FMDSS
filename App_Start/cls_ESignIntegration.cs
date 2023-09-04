using System;
using FMDSS.E_SignIntegration;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using FMDSS.Models;
using System.Web;
using iTextSharp.text.pdf.draw;
using System.Collections.Generic;
namespace FMDSS.App_Start
{

    public class CommanClassPDF
    {
        public string ModuleName { get; set; }
        public string CreatedDate { get; set; }
        public string Aadhar_ID { get; set; }
        public string Name { get; set; }
        public string RequestID { get; set; }

    }
    public class PDFLogFile : CommanClassPDF
    {

        public string PDFwithoutSign { get; set; }
        public string PDFwithSign { get; set; }

    }
    public class DMSError : CommanClassPDF
    {
        public string Request { get; set; }
        public string Response { get; set; }

    }
    public class ReGenratedPDFModel
    {
        public string TableName { get; set; }
        public string RequestId { get; set; }
    }

    public class PDFModel
    {
        public PDFModel()
        {
            GenratePDFModel = new ReGenratedPDFModel();
            PDFLogFileModel = new List<PDFLogFile>();
            DMSErrorModel = new List<DMSError>();
        }
        public ReGenratedPDFModel GenratePDFModel { get; set; }
        public List<PDFLogFile> PDFLogFileModel { get; set; }
        public List<DMSError> DMSErrorModel { get; set; }
    }

    public static class GenratePDF
    {
        public static void GenrateSimplePDF(string RequestId, string Status, string TableName, out string filepath, out string NewFilePathWithSign)
        {
            //string filepath = string.Empty;
            string fileNames = string.Empty;
            string TransitPermitfileNames = string.Empty;
            filepath = "~/ESignUpload/ALL_PDF/";
            NewFilePathWithSign = "~/ESignUpload/ESignPDF/";
            string  NewFilePathWithSignTransitPermit = "~/ApproveTransitPermit/";
            CitizenDashboard _objModel = new CitizenDashboard();
            #region Genrate Simple PDF
            DataTable dtResult = new DataTable();
            try
            {
                DataSet ds = _objModel.GetPrintApplicationAfterApproval(RequestId, Status, TableName);
                StringBuilder sb = new StringBuilder();
                DataTable DT1 = new DataTable();
                DataTable DT2 = new DataTable();
                if (ds != null)
                {
                    #region tbl_FixedPermissions
                    if (TableName == "tbl_FixedPermissions")
                    {

                        DT1 = ds.Tables[0];
                        DT2 = ds.Tables[1];
                        fileNames = "MiningNOC_" + DateTime.Now.Ticks.ToString() + ".pdf";
                        filepath = filepath + fileNames;
                        Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create));
                        var FontColour = new BaseColor(0, 0, 0);
                        Paragraph tableheading = null;
                        Paragraph sideheading = null;
                        Phrase colHeading;
                        PdfPCell cell;
                        PdfPTable pdfTable = null;
                        var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                        var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
                        iTextSharp.text.Font fontAnchor = iTextSharp.text.FontFactory.GetFont(iTextSharp.text.FontFactory.HELVETICA, 12,
                        iTextSharp.text.Font.UNDERLINE);


                        doc.Open();
                        doc.NewPage();
                        doc.Add(new Paragraph(Environment.NewLine));
                        tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        string Div_name = "";
                        if (DT2.Rows.Count > 0)
                        {
                            Div_name = DT2.Rows[0]["DIV_NAME"].ToString();
                        }
                        else
                        {
                            Div_name = "";
                        }
                        tableheading = new Paragraph("" + DT1.Rows[0]["Designation"] + "," + Div_name, MyFont);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);

                        tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
                        tableheading.Font.Size = 10;
                        tableheading.Alignment = (Element.ALIGN_RIGHT);
                        doc.Add(tableheading);


                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Add(new Paragraph(Environment.NewLine));

                        tableheading = new Paragraph("No objection Certificate (NOC) for " + DT1.Rows[0]["Permission For"].ToString(), fontAnchor);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        string address = "";
                        if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                        {
                            address = "";
                        }
                        else
                        {
                            address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                        }
                        sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ", " + DT1.Rows[0]["Application Date"] + " Submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " for the following Area of  " + Convert.ToString(DT1.Rows[0]["PerposedArea"]) + " is not lying  in forest region.");
                        sideheading.Font.Size = 10;
                        sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                        doc.Add(sideheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        pdfTable = new PdfPTable(7);
                        pdfTable.DefaultCell.Padding = 1;
                        pdfTable.WidthPercentage = 95;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        int count = 0;
                        if (DT2.Rows.Count > 0)
                        {
                            count = DT2.Rows.Count;
                            DT2.Columns.Remove("RequestedID");
                            //   DT2.Columns.Remove("KhasraNo");
                            DT2.AcceptChanges();
                            string[,] arrPdfData = new string[count, 7];
                            arrPdfData[0, 0] = "Division Name ";
                            arrPdfData[0, 1] = "District Name";
                            arrPdfData[0, 2] = "Block Name";
                            arrPdfData[0, 3] = "GP Name";
                            arrPdfData[0, 4] = "Village Name";
                            arrPdfData[0, 5] = "Khasra No";
                            arrPdfData[0, 6] = "Area Name";

                            pdfTable.AddCell("Division Name ");
                            pdfTable.AddCell("District Name");
                            pdfTable.AddCell("Block Name");
                            pdfTable.AddCell("GP Name");
                            pdfTable.AddCell("Village Name");
                            pdfTable.AddCell("Khasra No");
                            pdfTable.AddCell("Area Name");

                            for (int xid = 0; xid < count; xid++)
                            {
                                for (int yid = 0; yid < 7; yid++)
                                {
                                    arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                                    colHeading = new Phrase(arrPdfData[xid, yid]);
                                    colHeading.Font.Size = 10;
                                    cell = new PdfPCell(new Phrase(colHeading));
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfTable.AddCell(cell);
                                }
                            }
                            doc.Add(pdfTable);
                            pdfTable = new PdfPTable(4);
                            pdfTable.DefaultCell.Padding = 1;
                            pdfTable.WidthPercentage = 95;
                            pdfTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                        }
                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Add(new Paragraph(Environment.NewLine));
                        tableheading = new Paragraph(DT1.Rows[0]["Designation"].ToString(), MyFont);

                        tableheading.Font.Size = 10;
                        tableheading.Alignment = (Element.ALIGN_RIGHT);
                        doc.Add(tableheading);
                        tableheading = new Paragraph(DT1.Rows[0]["Name"].ToString(), MyFont);
                        tableheading.Font.Size = 10;
                        tableheading.Alignment = (Element.ALIGN_RIGHT);
                        doc.Add(tableheading);

                        doc.Add(new Paragraph(Environment.NewLine));

                        sideheading = new Paragraph("Note:- This is an E-Sign Document");
                        sideheading.Font.Size = 10;
                        sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                        doc.Add(sideheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Close();
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                        {
                            //ProcessStartInfo startInfo = new ProcessStartInfo
                            //{
                            //    Arguments = Server.MapPath(filepath),
                            //    FileName = "explorer.exe"
                            //};
                            //Process.Start(startInfo);

                        }

                    }
                    #endregion
                    #region Tbl_Citizen_TransitPermit
                    if (TableName == "Tbl_Citizen_TransitPermit")
                    {
                        DT1 = ds.Tables[0];
                        DT2 = ds.Tables[1];
                        // DataTable DT3 = ds.Tables[2];
                        fileNames = "TransitPermit_" + DateTime.Now.Ticks.ToString() + ".pdf";
                        TransitPermitfileNames = "TransitPermit_" + DT1.Rows[0]["RequestID"] + ".pdf";
                        filepath = filepath + fileNames;
                        NewFilePathWithSignTransitPermit = NewFilePathWithSignTransitPermit + TransitPermitfileNames;
                        Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create));
                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(NewFilePathWithSignTransitPermit), FileMode.Create));


                        var FontColour = new BaseColor(0, 0, 0);
                        Paragraph tableheading = null;
                        Paragraph sideheading = null;
                        Phrase colHeading;

                        PdfPCell cell;
                        PdfPTable pdfTable = null;

                        var subheadfont = FontFactory.GetFont("Times New Roman", 10, FontColour);

                        var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                        var myfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);

                        //boldFont.SetStyle(FontFactory.H);


                        doc.Open();
                        doc.NewPage();
                        PdfPTable Logo = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        string imageURL = "~/images/logo.png";
                        iTextSharp.text.Image ForestLogo = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(imageURL));
                        ForestLogo.ScaleToFit(150f, 150f);
                        ForestLogo.SpacingBefore = 10f;
                        ForestLogo.SpacingAfter = 10f;
                        ForestLogo.Alignment = Element.ALIGN_CENTER;

                        PdfPCell cellForestLogo;
                        cellForestLogo = new PdfPCell(ForestLogo);
                        cellForestLogo.BorderWidth = 0;
                        cellForestLogo.Padding = 20;
                        cellForestLogo.PaddingTop = -10;
                        cellForestLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                        Logo.AddCell(cellForestLogo);

                        doc.Add(Logo);
                        #region New PDF By ProductList

                        PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        PdfPCell cells = new PdfPCell() { Border = 4 };
                        Details.TotalWidth = 120;
                        Details.SetTotalWidth(new float[] { 35f, 50f, 35f });

                        cells = new PdfPCell(new Phrase("FORM-I", boldFont)) { Border = 0 };
                        cells.Colspan = 3;
                        cells.HorizontalAlignment = Element.ALIGN_CENTER;
                        Details.AddCell(cells);

                        cells = new PdfPCell(new Phrase("(See rule 4)", boldFont)) { Border = 0 };
                        cells.Colspan = 3;
                        cells.HorizontalAlignment = Element.ALIGN_CENTER;
                        Details.AddCell(cells);


                        cells = new PdfPCell(new Phrase("Transit Pass under  rule 2 of the Rajasthan Forest (Produce Transit) Rules, 1957", subheadfont)) { Border = 0 };
                        cells.Colspan = 3;
                        cells.HorizontalAlignment = Element.ALIGN_CENTER;
                        Details.AddCell(cells);

                        cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                        cells.Colspan = 3;
                        cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                        Details.AddCell(cells);

                        doc.Add(Details);




                        PdfPTable ProductDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        PdfPCell cellproduct = new PdfPCell() { Border = 4 };
                        ProductDetails.TotalWidth = 110;
                        ProductDetails.SetTotalWidth(new float[] { 5f, 20f, 15f, 10f, 20f, 25f });

                        cellproduct = new PdfPCell(new Phrase("1", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Name of Issuing Officer", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApprovedBy"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("2", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Permit Number", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("3", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Date", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["IssueDate"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("4", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Name of the Person", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantName"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("5", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Address", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantAddress"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("6", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Village", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantVillage"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("7", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Tehsil", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["APPLICANT_TEHSIL"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("8", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("District", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["APPLICANT_DISTRICT"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("9", subheadfont));
                        cellproduct.Rowspan = DT2 != null ? DT2.Rows.Count + 2 : 2;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Quantity and description of forest produce for which the pass is valid", subheadfont));
                        cellproduct.Colspan = 5;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Produce Name", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Unit", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);


                        cellproduct = new PdfPCell(new Phrase("Quantity", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Description", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        #region Multiple Product
                        if (DT2 != null && DT2.Rows.Count > 0)
                        {
                            for (int i = 0; i <= DT2.Rows.Count - 1; i++)
                            {

                                cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductName"]), subheadfont));
                                cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                ProductDetails.AddCell(cellproduct);

                                cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductUnit"]), subheadfont));
                                cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                ProductDetails.AddCell(cellproduct);

                                cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["PRODUCE_QUANTITY"]), subheadfont));
                                cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                ProductDetails.AddCell(cellproduct);

                                cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["Desc"]), subheadfont));
                                cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellproduct.Colspan = 2;
                                ProductDetails.AddCell(cellproduct);

                            }
                        }
                        #endregion

                        cellproduct = new PdfPCell(new Phrase("10", subheadfont));
                        cellproduct.Rowspan = 6;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Description of origin of forest produce covered by the pass:", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 5;
                        ProductDetails.AddCell(cellproduct);

                        //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                        //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellproduct.Colspan = 3;
                        //ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("a. Name of land holder from whose land the raw material obtained", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderName"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("b. Name of village and khasra number where the land holding is situated", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderVillageName"]) + " / " + Convert.ToString(DT1.Rows[0]["LandHoldingKhasraNo"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("c. Area of land holding", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHoldingArea"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("d. Name of District and Tehsil of land holding", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderDistrict"]) + " / " + Convert.ToString(DT1.Rows[0]["LandHolderTehsil"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("e. Name of place where the forest produce was stored/ converted", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["PlaceNamewhereProduceGenerate"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("11", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Type and registration number of vehicle used in transportation of forest produce:", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleType"]) + " / " + Convert.ToString(DT1.Rows[0]["VehicleNo"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("12", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Fee paid for the pass", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["FeeAmount"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("13", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Fee paid on", subheadfont));
                        cellproduct.Colspan = 2;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["EmitraDate"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 3;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("14", subheadfont));
                        cellproduct.Rowspan = 2;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Place from and to which such produce is to be taken or conveyed", subheadfont));
                        cellproduct.Rowspan = 2;
                        cellproduct.Colspan = 2;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("From", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateFrom"]) + " / " + Convert.ToString(DT1.Rows[0]["MovementDistFrom"]), subheadfont));
                        cellproduct.Colspan = 2;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("To", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateTo"]) + " / " + Convert.ToString(DT1.Rows[0]["MovementDistTo"]), subheadfont));
                        cellproduct.Colspan = 2;
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("15", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Route through which such forest produce is to be conveyed", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementRoutePlan"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 4;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("16", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Period for which the pass shall be valid", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TP_Valid_ApprovalDate"]), subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 4;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("17", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Specimen Signaure of the Pass holder", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 4;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("18", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("Signature and Stamp of the Issuing Officer", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 2;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellproduct.Colspan = 6;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                        cellproduct.Colspan = 6;
                        cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                        cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellproduct.Colspan = 6;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase("( " + Convert.ToString(DT1.Rows[0]["ApprovedBy"]) + " )", subheadfont)) { Border = 0 };
                        cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellproduct.Colspan = 6;
                        ProductDetails.AddCell(cellproduct);

                        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApprovedDesignationName"]), subheadfont)) { Border = 0 };
                        cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellproduct.Colspan = 6;
                        ProductDetails.AddCell(cellproduct);
                        doc.Add(ProductDetails);
                        #endregion
                        doc.Close();
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                        {

                        }
                    }
                    #endregion
                    #region Org Camp
                    if (TableName == "tbl_OrganisingCamp")
                    {
                        DT1 = ds.Tables[0];
                        DT2 = ds.Tables[1];
                        fileNames = "orgCamp_" + DateTime.Now.Ticks.ToString() + ".pdf";
                        filepath = filepath + fileNames;
                        Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create));
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
                        tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);

                        doc.Add(tableheading);

                        tableheading = new Paragraph("No objection Certificate (NOC) for Organising Camp", MyFont);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);

                        tableheading = new Paragraph("Office of  " + DT1.Rows[0]["Place Name"] + "," + DT1.Rows[0]["District Name"], MyFont);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        tableheading = new Paragraph("Dated: " + DT1.Rows[0]["Action Taken On"] + "", MyFont);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_RIGHT);
                        doc.Add(tableheading);

                        doc.Add(new Paragraph(Environment.NewLine));
                        string address = "";
                        if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                        {
                            address = "";
                        }
                        else
                        {
                            address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                        }
                        //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["Application Date"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                        sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " for Organizing Camp has been approved. Following members is allow to visit " + DT1.Rows[0]["Place Name"] + ", " + DT1.Rows[0]["District Name"] + " from " + DT1.Rows[0]["DurationFrom"].ToString() + " to  " + DT1.Rows[0]["DurationTo"].ToString() + ".");
                        sideheading.Font.Size = 10;
                        sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                        doc.Add(sideheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        pdfTable = new PdfPTable(6);
                        pdfTable.DefaultCell.Padding = 1;
                        pdfTable.WidthPercentage = 95;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        int count = DT2.Rows.Count;

                        //   DT2.Columns.Remove("KhasraNo");
                        DT2.AcceptChanges();
                        string[,] arrPdfData = new string[count, 7];


                        PdfPCell cellName = new PdfPCell(new Phrase("Name"));
                        cellName.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellName.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellName);
                        PdfPCell cellAddress = new PdfPCell(new Phrase("Address"));
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellAddress);
                        PdfPCell cellLand = new PdfPCell(new Phrase("Land Mark"));
                        cellLand.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellLand.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellLand);
                        PdfPCell cellPostal = new PdfPCell(new Phrase("Postal Code"));
                        cellPostal.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellPostal.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellPostal);
                        PdfPCell cellGender = new PdfPCell(new Phrase("Gender"));
                        cellGender.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellGender.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellGender);
                        PdfPCell cellID = new PdfPCell(new Phrase("ID No."));
                        cellID.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellID.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellID);



                        for (int xid = 0; xid < count; xid++)
                        {
                            for (int yid = 0; yid < 6; yid++)
                            {
                                arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                                colHeading = new Phrase(arrPdfData[xid, yid]);
                                colHeading.Font.Size = 10;
                                cell = new PdfPCell(new Phrase(colHeading));
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdfTable.AddCell(cell);
                            }
                        }

                        tableheading = new Paragraph("Guest/Visitor Details");
                        tableheading.Font.Size = 14;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Add(pdfTable);


                        tableheading = new Paragraph("For any query, contact us at <> ", MyFont);
                        tableheading.Font.Size = 10;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Close();
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                        {


                        }

                    }
                    #endregion

                    #region FilmShooting
                    if (TableName == "tbl_FilmShootingPermissions")
                    {

                        DT1 = ds.Tables[0];
                        DT2 = ds.Tables[1];
                        fileNames = "Film_" + DateTime.Now.Ticks.ToString() + ".pdf";
                        filepath = filepath + fileNames;
                        Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create));
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
                        tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);

                        doc.Add(tableheading);

                        tableheading = new Paragraph("No objection Certificate (NOC) for Film Shooting", MyFont);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);

                        tableheading = new Paragraph("Office of  " + DT1.Rows[0]["PlaceName"] + "," + DT1.Rows[0]["DIST_NAME"], MyFont);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        tableheading = new Paragraph("Dated: " + DT1.Rows[0]["ActionTakenOn"] + "", MyFont);
                        tableheading.Font.Size = 11;
                        tableheading.Alignment = (Element.ALIGN_RIGHT);
                        doc.Add(tableheading);

                        doc.Add(new Paragraph(Environment.NewLine));
                        string address = "";
                        if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                        {
                            address = "";
                        }
                        else
                        {
                            address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                        }
                        //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["EnteredOn"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                        sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " for Film Shooting has been approved. Following members is allow to visit " + DT1.Rows[0]["PlaceName"] + ", " + DT1.Rows[0]["DIST_NAME"] + " from " + DT1.Rows[0]["DurationFrom"].ToString() + " to  " + DT1.Rows[0]["DurationTo"].ToString() + ".");
                        sideheading.Font.Size = 10;
                        sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                        doc.Add(sideheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        pdfTable = new PdfPTable(6);
                        pdfTable.DefaultCell.Padding = 1;
                        pdfTable.WidthPercentage = 95;
                        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                        int count = DT2.Rows.Count;

                        //   DT2.Columns.Remove("KhasraNo");
                        DT2.AcceptChanges();
                        string[,] arrPdfData = new string[count, 7];


                        PdfPCell cellName = new PdfPCell(new Phrase("Name"));
                        cellName.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellName.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellName);
                        PdfPCell cellAddress = new PdfPCell(new Phrase("Address"));
                        cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellAddress.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellAddress);
                        PdfPCell cellLand = new PdfPCell(new Phrase("Land Mark"));
                        cellLand.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellLand.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellLand);
                        PdfPCell cellPostal = new PdfPCell(new Phrase("Postal Code"));
                        cellPostal.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellPostal.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellPostal);
                        PdfPCell cellGender = new PdfPCell(new Phrase("Gender"));
                        cellGender.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellGender.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellGender);
                        PdfPCell cellID = new PdfPCell(new Phrase("ID No."));
                        cellID.HorizontalAlignment = Element.ALIGN_LEFT;
                        cellID.VerticalAlignment = Element.ALIGN_CENTER;
                        pdfTable.AddCell(cellID);



                        for (int xid = 0; xid < count; xid++)
                        {
                            for (int yid = 0; yid < 6; yid++)
                            {
                                arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                                colHeading = new Phrase(arrPdfData[xid, yid]);
                                colHeading.Font.Size = 10;
                                cell = new PdfPCell(new Phrase(colHeading));
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                pdfTable.AddCell(cell);
                            }
                        }

                        tableheading = new Paragraph("Guest/Visitor Details");
                        tableheading.Font.Size = 14;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Add(pdfTable);


                        tableheading = new Paragraph("For any query, contact us at <> ", MyFont);
                        tableheading.Font.Size = 10;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        doc.Close();
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                        {


                        }

                    }
                    #endregion

                    #region Research Study
                    if (TableName == "tbl_ResearchStudyPermissions")
                    {

                        DT1 = ds.Tables[0];
                        fileNames = "research_" + DateTime.Now.Ticks.ToString() + ".pdf";
                        filepath = filepath + fileNames;
                        Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create));
                        var FontColour = new BaseColor(0, 0, 0);
                        Paragraph tableheading = null;
                        Paragraph sideheading = null;
                        Paragraph sideheading1 = null;
                        Paragraph sideheading2 = null;
                        Phrase colHeading;
                        PdfPCell cell;
                        PdfPTable pdfTable = null;
                        var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                        var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);

                        doc.Open();
                        doc.NewPage();

                        Image image = Image.GetInstance(@"C:\PublishFMDSS\images\logo.png");

                        //image.ScaleAbsolute(50, 50);
                        doc.Add(image);
                        doc.Add(new Paragraph(Environment.NewLine));
                        tableheading = new Paragraph("Officer of the Pr.Chief Conservator of forests & Chief Wildlife Warden,jaipur,Rajasthan", MyFont);
                        //tableheading.Font.GetType(Font.UNDEFINED);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);

                        doc.Add(tableheading);


                        Chunk reqNo = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph = new Paragraph("Request No:" + DT1.Rows[0]["RequestedID"].ToString());
                        paragraph.Add(new Chunk(reqNo));
                        paragraph.Add("Dated:" + DT1.Rows[0]["Action Taken On"].ToString());
                        doc.Add(paragraph);

                        Paragraph p = new Paragraph("To");
                        doc.Add(p);

                        Paragraph pt = new Paragraph("Mrs/MS " + DT1.Rows[0]["Entered By"].ToString() + "\n" + DT1.Rows[0]["Postal_Address1"].ToString() + "\n\n" + "Sub:" + DT1.Rows[0]["Subject"].ToString() + " in " + DT1.Rows[0]["Place Name"].ToString() + "\n Ref: & Your Request dated:" + DT1.Rows[0]["Action Taken On"].ToString());
                        pt.IndentationLeft = 60f;
                        doc.Add(pt);

                        iTextSharp.text.Paragraph titolo = new iTextSharp.text.Paragraph("Sir/Madam");
                        titolo.SpacingAfter = 10;
                        doc.Add(titolo);



                        sideheading = new Paragraph("With reference to your request cited above on the subject and as per the letter of Director,Ministry of ");

                        //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["From Date"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                        //sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " has been approved for below request.");
                        sideheading.Font.Size = 10;
                        sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                        sideheading.IndentationLeft = 60f;
                        doc.Add(sideheading);
                        Paragraph sideheading4 = new Paragraph("Human Resources Development, Department of Higher Education Gov. of india, new Delhi Dated 4.5.2007, the permission to" + DT1.Rows[0]["Subject"].ToString() + " in " + DT1.Rows[0]["Place Name"].ToString() + " w.e.f " + DT1.Rows[0]["From date"] + " to" + DT1.Rows[0]["From date"] + " is hereby grantedunder the section 28(1)(c) of Wildlife (protection) Act. 1972.This permission is given subject to the conditions mentioned below:-");

                        //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["From Date"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                        //sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " has been approved for below request.");
                        sideheading4.Font.Size = 10;
                        sideheading4.Alignment = (Element.ALIGN_JUSTIFIED);

                        doc.Add(sideheading4);

                        List orderedList = new List(List.ORDERED);
                        orderedList.Add(new ListItem("The programme of survey/study is to be necessarily got approved from the DCF & Dy.Director (Buffer) karauli, officer in-charge,prior to entry in the protected area."));
                        orderedList.Add(new ListItem("The provision rules. & regulations regarding National parks. sanctuaries and protected area should be adhered strictly."));
                        orderedList.Add(new ListItem("This is ensure that no wild animal would be disturbed or caused any harm."));
                        orderedList.Add(new ListItem("For the purpose of survey work, no medice will be allowed to give to the wild animal, nor change any habit of eating would be allowed."));
                        orderedList.Add(new ListItem("The Instruction issued by the office in-charge at the time of survey work should be followed strictly."));
                        orderedList.Add(new ListItem("The movement of vehicle in protected area/National park  ot protected area, while entering and then after would be under the complete control of officer In-charge concerned."));
                        orderedList.Add(new ListItem("If any irregularity,loss,incident of poaching is senn in the protected area it should be immediately communicated to tge officer In-charge in writing or telephonically."));
                        orderedList.Add(new ListItem("This permission can be withdrawn in the intrest of wildlifeat any time without mentioning the reason therefore."));
                        orderedList.Add(new ListItem("The researcher and their assistants should have to maintain a log book regarding movement in the park and the said log book should be made available to officer In-charge regularly."));
                        orderedList.Add(new ListItem("The study work should be  based on the observation, interviews etc."));
                        orderedList.Add(new ListItem("Wildlife Institute of India will work as nodal agency for the researchers.The scholar has to provide soft and hard copy of the results,conclusions,inference of research work etc. to the agency."));
                        orderedList.Add(new ListItem("After completion of survey/study work,copy of the complete report should be provided to this office and officer In-charge of the sanctuary as well."));
                        orderedList.Add(new ListItem("The instruction mentioned in the letter dated 4.5.2007 issued by the Human Resources Department,Department of Higher Education Govt. of India, New Delhi should be adhered strictly."));
                        orderedList.Add(new ListItem("MS Alison wadmore,british National, will travel to/from this office in the jaipur and kela devi Wildlife Sanctuary to share information during the study period."));
                        orderedList.Add(new ListItem("This permission willbe extended upto 31.8.2008 after receiving the extension in visa period."));

                        doc.Add(orderedList);


                        Chunk ch1 = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph1 = new Paragraph("");
                        paragraph1.Add(new Chunk(ch1));
                        paragraph1.Add("Your faithfully.");


                        doc.Add(paragraph1);


                        Chunk ch2 = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph2 = new Paragraph("");
                        paragraph2.Add(new Chunk(ch2));
                        paragraph2.Add("Pr.Chief Conservator of forests & Chief Wildlife Warden.");
                        doc.Add(paragraph2);
                        Chunk ch3 = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph3 = new Paragraph("");
                        paragraph3.Add(new Chunk(ch3));
                        paragraph3.Add("jaipur, Rajasthan");


                        doc.Add(paragraph3);



                        Chunk ch4 = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph4 = new Paragraph("Request No:" + DT1.Rows[0]["RequestedID"].ToString());
                        paragraph4.Add(new Chunk(ch4));
                        paragraph4.Add("Dated:" + DT1.Rows[0]["Action Taken On"].ToString());


                        doc.Add(paragraph4);


                        tableheading = new Paragraph("Copy forwarded to the following for information and neccessary action:-", MyFont);
                        tableheading.Font.Size = 10;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);
                        doc.Add(new Paragraph(Environment.NewLine));
                        List orderedList1 = new List(List.ORDERED);
                        orderedList1.Add(new ListItem("Director, Ministory of Human Resources Development, Department of Higher Education Govt. of India, New Delhi."));
                        orderedList1.Add(new ListItem("Additional Secretary Forest Department, Gov. of Rajasthan Jaipur."));
                        orderedList1.Add(new ListItem("Conservator of Forests & Field Director Ranthambhore Tiger project Kota."));
                        orderedList1.Add(new ListItem("Dy Conservator of Forests(Buffer), karauli."));
                        doc.Add(orderedList1);


                        Chunk ch5 = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph5 = new Paragraph("");
                        paragraph5.Add(new Chunk(ch5));
                        paragraph5.Add("Pr.Chief Conservator of forests & Chief Wildlife Warden.");
                        doc.Add(paragraph5);
                        Chunk ch6 = new Chunk(new VerticalPositionMark());
                        Paragraph paragraph6 = new Paragraph("");
                        paragraph6.Add(new Chunk(ch6));
                        paragraph6.Add("jaipur, Rajasthan");


                        doc.Add(paragraph6);



                        doc.Close();
                        if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                        {

                        }

                    }
                    #endregion

                    #region AmritaDevi
                    if (TableName == "tbl_AD_Award")
                    {
                        fileNames = "AmritaDevi_" + DateTime.Now.Ticks.ToString() + ".pdf";
                        filepath = filepath + fileNames;

                        List<AmiritaDeviPDFModel> model1 = new List<AmiritaDeviPDFModel>();
                        //AmiritaDeviPDFModel model = new AmiritaDeviPDFModel();
                        List<AmiritaDeviYearsDetailsPDFModel> workDetails = new List<AmiritaDeviYearsDetailsPDFModel>();
                        List<AmiritaDeviGISPDFModel> GISLIST_PDF = new List<AmiritaDeviGISPDFModel>();
                        #region Insert Data Dataset to Model

                        string str = string.Empty;
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                        model1 = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmiritaDeviPDFModel>>(str);

                        str = string.Empty;
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1]);
                        GISLIST_PDF = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmiritaDeviGISPDFModel>>(str);

                        str = string.Empty;
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2]);
                        workDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AmiritaDeviYearsDetailsPDFModel>>(str);

                        #endregion

                        #region Create PDF
                        Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                        PdfWriter.GetInstance(doc, new FileStream(HttpContext.Current.Server.MapPath(filepath), FileMode.Create));

                        var FontColour = new BaseColor(0, 0, 0);
                        Paragraph tableheading = null;
                        Paragraph sideheading = null;
                        Phrase colHeading;

                        PdfPCell cell;
                        PdfPTable pdfTable = null;

                        BaseFont dev = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\mfdev016.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font Myhindi = new iTextSharp.text.Font(dev, 16);
                        iTextSharp.text.Font hindiHead = new iTextSharp.text.Font(dev, 14);
                        iTextSharp.text.Font hindiTitle = new iTextSharp.text.Font(dev, 12);
                        iTextSharp.text.Font hindiSubFont = new iTextSharp.text.Font(dev, 10);

                        BaseFont dev1 = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        iTextSharp.text.Font MyEnglish = new iTextSharp.text.Font(dev1, 14);
                        iTextSharp.text.Font Myfont = new iTextSharp.text.Font(dev1, 14);
                        iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(dev1, 12);
                        iTextSharp.text.Font subheadfont = new iTextSharp.text.Font(dev1, 8);

                        foreach (var model in model1)
                        {
                            #region Start
                            doc.Open();
                            doc.NewPage();
                            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cells = new PdfPCell() { Border = 4 };
                            Details.TotalWidth = 120;
                            Details.SetTotalWidth(new float[] { 35f, 50f, 35f });


                            cells = new PdfPCell(new Phrase("jktLFkku ljdkj", Myhindi)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);

                            cells = new PdfPCell(new Phrase("ou foHkkx", Myhindi)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);

                            cells = new PdfPCell(new Phrase("ve`rk nsoh fo’uksbZ Le`fr iqjLdkj", Myhindi)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);

                            cells = new PdfPCell(new Phrase("vkosnu & i=", Myhindi)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);

                            doc.Add(new Phrase(Environment.NewLine));

                            cells = new PdfPCell(new Phrase(" ", hindiSubFont)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);
                            doc.Add(Details);



                            PdfPTable LocationDetails = new PdfPTable(7) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellvdetails = new PdfPCell() { Border = 4 };
                            LocationDetails.TotalWidth = 254;
                            LocationDetails.SetTotalWidth(new float[] { 35f, 35f, 35f, 35f, 35f, 35f, 35f });

                            cellvdetails = new PdfPCell(new Phrase("iath;u ukekad" + " & " + model.RequestID, hindiHead)) { Border = 0 };
                            cellvdetails.Colspan = 5;
                            cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                            LocationDetails.AddCell(cellvdetails);


                            cellvdetails = new PdfPCell(new Phrase("fnukad" + " - " + model.ApplyDate, hindiHead)) { Border = 0 };
                            cellvdetails.Colspan = 2;
                            cellvdetails.HorizontalAlignment = Element.ALIGN_RIGHT;
                            LocationDetails.AddCell(cellvdetails);


                            cellvdetails = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            cellvdetails.Colspan = 7;
                            cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                            LocationDetails.AddCell(cellvdetails);

                            cellvdetails = new PdfPCell(new Phrase("Location Details", Myfont)) { Border = 0 };
                            cellvdetails.Colspan = 7;
                            cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                            LocationDetails.AddCell(cellvdetails);

                            cellvdetails = new PdfPCell(new Phrase("Division", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);
                            cellvdetails = new PdfPCell(new Phrase("District", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);
                            cellvdetails = new PdfPCell(new Phrase("Forest Division", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);
                            cellvdetails = new PdfPCell(new Phrase("Panchyat samiti", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);
                            cellvdetails = new PdfPCell(new Phrase("Gram Panchayat", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);
                            cellvdetails = new PdfPCell(new Phrase("Village", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);
                            cellvdetails = new PdfPCell(new Phrase("Name of Area", fontTitle));
                            cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            LocationDetails.AddCell(cellvdetails);

                            foreach (var itm in GISLIST_PDF)
                            {
                                cellvdetails = new PdfPCell(new Phrase(itm.Div_NM, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                                cellvdetails = new PdfPCell(new Phrase(itm.Dist_NM, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                                cellvdetails = new PdfPCell(new Phrase(itm.ForestDiv_NM, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                                cellvdetails = new PdfPCell(new Phrase(itm.Block_NM, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                                cellvdetails = new PdfPCell(new Phrase(itm.Gp_NM, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                                cellvdetails = new PdfPCell(new Phrase(itm.Village_NM, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                                cellvdetails = new PdfPCell(new Phrase(itm.areaName, subheadfont));
                                LocationDetails.AddCell(cellvdetails);
                            }
                            doc.Add(LocationDetails);


                            PdfPTable GPSDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellGPSdetails = new PdfPCell() { Border = 4 };
                            GPSDetails.TotalWidth = 100;
                            GPSDetails.SetTotalWidth(new float[] { 50f, 50f });

                            cellGPSdetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                            cellGPSdetails.Colspan = 2;
                            GPSDetails.AddCell(cellGPSdetails);

                            cellGPSdetails = new PdfPCell(new Phrase("GPS Details", Myfont)) { Border = 0 };
                            cellGPSdetails.Colspan = 2;
                            cellGPSdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                            GPSDetails.AddCell(cellGPSdetails);


                            cellGPSdetails = new PdfPCell(new Phrase("Latitude", fontTitle));
                            cellGPSdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            GPSDetails.AddCell(cellGPSdetails);
                            cellGPSdetails = new PdfPCell(new Phrase("Longitude", fontTitle));
                            cellGPSdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            GPSDetails.AddCell(cellGPSdetails);
                            cellGPSdetails = new PdfPCell(new Phrase(model.GPSLat, subheadfont));
                            GPSDetails.AddCell(cellGPSdetails);
                            cellGPSdetails = new PdfPCell(new Phrase(model.GPSLong, subheadfont));
                            GPSDetails.AddCell(cellGPSdetails);

                            cellGPSdetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                            cellGPSdetails.Colspan = 2;
                            GPSDetails.AddCell(cellGPSdetails);

                            doc.Add(GPSDetails);



                            PdfPTable PersonalDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellPdetails = new PdfPCell() { Border = 4 };

                            PersonalDetails.TotalWidth = 100;
                            PersonalDetails.SetTotalWidth(new float[] { 50f, 50f });
                            cellPdetails = new PdfPCell(new Phrase(@"vkosnu o""kZ", hindiTitle));
                            cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase("vkosnd dk izdkj", hindiTitle));
                            cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase(model.CurrentYear.ToString(), subheadfont));
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase(model.IsOrgOrPerson == false ? "O;fDr" : "laxBu", hindiSubFont));
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase("¼d½ ukfer O;fDr@laLFkk dk uke", hindiTitle));
                            cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase("¼[k½ ukfer O;fDr@laLFkk uke", hindiTitle));
                            cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase(model.FirstName1, subheadfont));
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase(model.FirstName2, subheadfont));
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase("izk;kstd dk uke ,oa irk", hindiTitle));
                            cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            cellPdetails.Colspan = 2;
                            PersonalDetails.AddCell(cellPdetails);
                            cellPdetails = new PdfPCell(new Phrase(model.Address, subheadfont));
                            cellPdetails.Colspan = 2;
                            PersonalDetails.AddCell(cellPdetails);
                            doc.Add(PersonalDetails);

                            PdfPTable AwardDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellAwarddetails = new PdfPCell() { Border = 4 };
                            AwardDetails.TotalWidth = 100;
                            AwardDetails.SetTotalWidth(new float[] { 50f, 50f });


                            cellAwarddetails = new PdfPCell(new Phrase("iq:Ldkj dh Js.kh ftlds fy, vkosnu fd;k x;k gS ", hindiTitle));
                            cellAwarddetails.Colspan = 2;
                            cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            AwardDetails.AddCell(cellAwarddetails);
                            cellAwarddetails = new PdfPCell(new Phrase(model.CategoryName, subheadfont));
                            cellAwarddetails.Colspan = 2;
                            AwardDetails.AddCell(cellAwarddetails);
                            cellAwarddetails = new PdfPCell(new Phrase("dk;Z LFky dk uke ", hindiTitle));
                            cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            cellAwarddetails.Colspan = 2;
                            AwardDetails.AddCell(cellAwarddetails);
                            cellAwarddetails = new PdfPCell(new Phrase(model.WorkStationName, subheadfont));
                            cellAwarddetails.Colspan = 2;
                            AwardDetails.AddCell(cellAwarddetails);

                            cellAwarddetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                            cellAwarddetails.Colspan = 2;
                            AwardDetails.AddCell(cellAwarddetails);

                            cellAwarddetails = new PdfPCell(new Phrase(@"O;fDr@laLFkk }kjk ftl Hkwfe ij mRd`""V dk;Z fd;k gS  mldh fdLe dks crkrs gq, okLrfod {ks=] LFkku rFkk ekfydkuk i}fr%&", hindiTitle)) { Border = 1 };
                            cellAwarddetails.Colspan = 2;
                            AwardDetails.AddCell(cellAwarddetails);


                            cellAwarddetails = new PdfPCell(new Phrase("LFkku", hindiTitle));
                            cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            AwardDetails.AddCell(cellAwarddetails);
                            cellAwarddetails = new PdfPCell(new Phrase("dqy okLrfod {ks= ¼gSDVs;j esa½", hindiTitle));
                            cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            AwardDetails.AddCell(cellAwarddetails);
                            cellAwarddetails = new PdfPCell(new Phrase(model.LandPlace, subheadfont));
                            AwardDetails.AddCell(cellAwarddetails);
                            cellAwarddetails = new PdfPCell(new Phrase(model.Landhacktor, subheadfont));
                            AwardDetails.AddCell(cellAwarddetails);


                            cellAwarddetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                            cellAwarddetails.Colspan = 2;
                            AwardDetails.AddCell(cellAwarddetails);

                            doc.Add(AwardDetails);


                            PdfPTable AreaDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellAreadetails = new PdfPCell() { Border = 4 };
                            AreaDetails.TotalWidth = 120;
                            AreaDetails.SetTotalWidth(new float[] { 35f, 50f, 50f });

                            cellAreadetails = new PdfPCell(new Phrase("LokfeRo", hindiTitle));
                            cellAreadetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase("futh Hkwfe", hindiTitle));
                            cellAreadetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                            cellAreadetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            AreaDetails.AddCell(cellAreadetails);

                            cellAreadetails = new PdfPCell(new Phrase("futh Hkwfe", hindiTitle));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.PersonalLandHactor, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.PersonalLandHactorDesc, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);

                            cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.CollectiveLandHactor, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.CollectiveLandDesc, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);

                            cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.RevenueLandHactor, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.RevenueLandDesc, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);

                            cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.ForestLandHactor, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);
                            cellAreadetails = new PdfPCell(new Phrase(model.ForestLandDesc, subheadfont));
                            AreaDetails.AddCell(cellAreadetails);

                            cellAreadetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                            cellAreadetails.Colspan = 3;
                            AreaDetails.AddCell(cellAreadetails);

                            cellAreadetails = new PdfPCell(new Phrase(@"vFkkZr pV~Vkuh] nynyh] cqybZ feV~Vh] yo.kh;@{kkjh;] igkMh ,oa chgM+ {ks= vU; dksbZ fo’ks""k fdLe", hindiTitle)) { Border = 0 };
                            cellAreadetails.Colspan = 3;
                            AreaDetails.AddCell(cellAreadetails);
                            doc.Add(AreaDetails);


                            PdfPTable WorkDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellWorkdetails = new PdfPCell() { Border = 4 };
                            WorkDetails.TotalWidth = 120;
                            WorkDetails.SetTotalWidth(new float[] { 35f, 30f, 30f, 30f });

                            cellWorkdetails = new PdfPCell(new Phrase("Details", subheadfont));
                            cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            WorkDetails.AddCell(cellWorkdetails);

                            cellWorkdetails = new PdfPCell(new Phrase(Convert.ToString(Convert.ToInt16(model.CurrentYear) - 1), subheadfont));
                            cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            WorkDetails.AddCell(cellWorkdetails);

                            cellWorkdetails = new PdfPCell(new Phrase(Convert.ToString(Convert.ToInt16(model.CurrentYear) - 2), subheadfont));
                            cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            WorkDetails.AddCell(cellWorkdetails);

                            cellWorkdetails = new PdfPCell(new Phrase(Convert.ToString(Convert.ToInt16(model.CurrentYear) - 3), subheadfont));
                            cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            WorkDetails.AddCell(cellWorkdetails);

                            foreach (var itm in workDetails)
                            {
                                cellWorkdetails = new PdfPCell(new Phrase(itm.WorkDescription, subheadfont));
                                WorkDetails.AddCell(cellWorkdetails);

                                cellWorkdetails = new PdfPCell(new Phrase(itm.CURRENT, subheadfont));
                                WorkDetails.AddCell(cellWorkdetails);

                                cellWorkdetails = new PdfPCell(new Phrase(itm.PREV, subheadfont));
                                WorkDetails.AddCell(cellWorkdetails);

                                cellWorkdetails = new PdfPCell(new Phrase(itm.END, subheadfont));
                                WorkDetails.AddCell(cellWorkdetails);
                            }

                            doc.Add(WorkDetails);
                            #endregion
                            doc.Close();

                        #endregion

                        }
                    #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                NewFilePathWithSign = string.Empty;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GenrateSimplePDF" + "_" + "E-SignIntegartion Genrate PDF time", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }

            #endregion

            NewFilePathWithSign = NewFilePathWithSign + fileNames;
            NewFilePathWithSignTransitPermit = NewFilePathWithSignTransitPermit + TransitPermitfileNames;
            #region Insert PDF information
            try
            {
              //  DataSet PDFInfo = _objModel.PDFInformation("Insert", RequestId, TableName, filepath, NewFilePathWithSign);
                //if (PDFInfo != null && PDFInfo.Tables.Count > 0 && PDFInfo.Tables[0].Rows.Count > 0 && Convert.ToString(PDFInfo.Tables[0].Rows[0]["Status"]) == "True")
                //{

                //}
                //else
                //{
                //    NewFilePathWithSign = string.Empty;
                //    throw new Exception("Not Insert PDF information on Table");
                //}

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GenrateSimplePDF" + "_" + "SignIntegartion Insert PDF Information time", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            #endregion


        }

        public static void UpdateGenrateSimplePDF(string RequestId, string NewFilePathWithSign)
        {
            CitizenDashboard _objModel = new CitizenDashboard();
            #region Update New PDF with sign file path

            try
            {
                DataSet PDFInfo = _objModel.PDFInformation("Update", RequestId, string.Empty, string.Empty, NewFilePathWithSign);
                if (PDFInfo != null && PDFInfo.Tables.Count > 0 && PDFInfo.Tables[0].Rows.Count > 0 && Convert.ToString(PDFInfo.Tables[0].Rows[0]["Status"]) == "True")
                {

                }
                else
                {
                    throw new Exception("Not Update New PDF file Path information on Table");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GenrateSimplePDF" + "_" + "SignIntegartion Insert PDF Information time", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            #endregion

        }

        public static PDFModel GetWithoutSignPdfList()
        {
            CitizenDashboard _objModel = new CitizenDashboard();
            DataSet ds = new DataSet();
            PDFModel model = new PDFModel();
            #region Update New PDF with sign file path
            try
            {
                ds = _objModel.PDFInformation("List", string.Empty, string.Empty, string.Empty, string.Empty);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    #region Serialize and DeSerialize Datatable to Model
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    model.PDFLogFileModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PDFLogFile>>(str);
                    #endregion
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    #region Serialize and DeSerialize Datatable to Model
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1]);
                    model.DMSErrorModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DMSError>>(str);
                    #endregion
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetWithoutSignPdfList" + "_" + "GenratePDF GetWithoutSignPdfList", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            return model;
            #endregion
        }
    }


    public static class cls_ESignIntegration
    {
        public static OtpResponce SendOTPbyESign(string AAdharId, string RequestId)
        {
            //AAdharId = "909731239310";
            CitizenDashboard _objModel = new CitizenDashboard();
            OtpResponce OtpResponce = new OtpResponce();
            #region Step 1 Send OTP
            try
            {
                clsOTP oSign = new clsOTP();
                oSign.aadharid = AAdharId;
                oSign.departmentname = System.Configuration.ConfigurationSettings.AppSettings["departmentname"].ToString();
                cls_eSign eSignRequest = new cls_eSign();
                OtpResponce = eSignRequest.GetOTP(oSign, RequestId);
            }
            catch (Exception ex)
            {
                OtpResponce = new OtpResponce();
            }

            #endregion
            return OtpResponce;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VerifyOTP"> OTP Number get from mobile</param>
        /// <param name="RequestID">Unique Number</param>
        /// <param name="Status">Please Pass 2 because 2 means Approve</param>
        /// <param name="TableName">This is particular table name to get data and create particular PDF</param>
        /// <returns></returns>

        public static clsVerifyOTPResponce VerifyOTPAndGenrateTransation(clsVerifyOTP VerifyOTP, string RequestID, string Status, string TableName)
        {
            clsVerifyOTPResponce VerifyResponce = new clsVerifyOTPResponce();

            /*
              #region Insert PDF information 
              try
              {
                  DataSet PDFInfo = _objModel _objModel.PDFInformation("Insert", RequestId, TableName, filepath, NewFilePathWithSign);
                  if (PDFInfo != null && PDFInfo.Tables.Count > 0 && PDFInfo.Tables[0].Rows.Count > 0 && Convert.ToString(PDFInfo.Tables[0].Rows[0]["[Status]"]) == "True")
                  {

                  }
                  else
                  {
                      NewFilePathWithSign = string.Empty;
                      throw new Exception("Not Insert PDF information on Table");
                  }

              }
              catch (Exception ex)
              {
                  new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GenrateSimplePDF" + "_" + "SignIntegartion Insert PDF Information time", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
              }
              #endregion

      */
            try
            {
                #region Step 2 Verify OTP
                VerifyOTP.otp = VerifyOTP.otp;
                VerifyOTP.transactionid = VerifyOTP.transactionid;

                cls_eSign eSignRequest = new cls_eSign();
                VerifyResponce = eSignRequest.VerifyOTP(VerifyOTP, RequestID);
                #endregion

                string FilePath = string.Empty;
                string FilePathWithSign = string.Empty;
                string NewFilePathWithSignTransitPermit = string.Empty; 
                
                if (!string.IsNullOrEmpty(VerifyResponce.TransactionId))
                {
                    #region Step 3 Genrate PDF
                    clsDocumentESign requestPdf = new clsDocumentESign();
                    clsDocumentESign transitrequestPdf = new clsDocumentESign();
                    requestPdf.transactionid = VerifyResponce.TransactionId;
                    transitrequestPdf.transactionid = VerifyResponce.TransactionId;
                    NewFilePathWithSignTransitPermit = "~/ApproveTransitPermit/TransitPermit_" + RequestID + ".pdf";

                    GenratePDF.GenrateSimplePDF(RequestID, Status, TableName, out FilePath, out FilePathWithSign);
                    byte[] bytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(FilePath));
                   
                    requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                    clsDocumentESignResponce response = GenratePDFWithSign(requestPdf, RequestID);
                    using (FileStream stream = System.IO.File.Create(HttpContext.Current.Server.MapPath(FilePathWithSign)))
                    {
                        byte[] byteArray = Convert.FromBase64String(response.Document);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    #region Update New File Path Information
                    GenratePDF.UpdateGenrateSimplePDF(RequestID, FilePathWithSign);
                    #endregion
                    if(TableName== "Tbl_Citizen_TransitPermit")
                    {
                        byte[] transitbytes = File.ReadAllBytes(HttpContext.Current.Server.MapPath(NewFilePathWithSignTransitPermit));

                        transitrequestPdf.inputJson.File = Convert.ToBase64String(transitbytes);
                        clsDocumentESignResponce transitresponse = GenratePDFWithSign(transitrequestPdf, RequestID);
                        using (FileStream stream = System.IO.File.Create(HttpContext.Current.Server.MapPath(NewFilePathWithSignTransitPermit)))
                        {
                            byte[] byteArray = Convert.FromBase64String(transitresponse.Document);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        #region Update New File Path Information
                        GenratePDF.UpdateGenrateSimplePDF(RequestID, NewFilePathWithSignTransitPermit);
                        #endregion
                    }



                    #endregion
                }
                else
                {
                    #region If OTP is Wrong then Genrate Simple PDF
                    GenratePDF.GenrateSimplePDF(RequestID, Status, TableName, out FilePath, out FilePathWithSign);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                VerifyResponce = new clsVerifyOTPResponce();
                VerifyResponce.ErrorMessage = ex.Message.ToString();
            }


            return VerifyResponce;
        }

        public static clsDocumentESignResponce GenratePDFWithSign(clsDocumentESign clsDocumentESignRequest, string RequestID)
        {
            clsDocumentESignResponce Responce = new clsDocumentESignResponce();
            #region Step 3 Genrate PDF with Sign
            try
            {
                cls_eSign eSignRequest = new cls_eSign();
                Responce = eSignRequest.GetDocumentESign(clsDocumentESignRequest, RequestID);
            }
            catch (Exception ex)
            {
                Responce = new clsDocumentESignResponce();
            }

            #endregion
            return Responce;
        }

        public static clsDocumentESignResponce GenratePDFWithSign(clsDocumentESign clsDocumentESignRequest, string RequestID, string type)
        {
            clsDocumentESignResponce Responce = new clsDocumentESignResponce();
            #region Step 3 Genrate PDF with Sign
            try
            {
                cls_eSign eSignRequest = new cls_eSign();
                if (string.IsNullOrWhiteSpace(type))
                {
                    Responce = eSignRequest.GetDocumentESign(clsDocumentESignRequest, RequestID);
                }
                else
                {
                    Responce = eSignRequest.GetDocumentESign(clsDocumentESignRequest, RequestID, type);
                }
            }
            catch (Exception ex)
            {
                Responce = new clsDocumentESignResponce();
            }

            #endregion
            return Responce;
        }

    }


    public static class cls_ESignIntegrationbyEmitra
    {
        public static OtpResponce SendOTPbyEmitra(string AAdharId = "937329232440")//909731239310 AMit Sir  937329232440 Karni sir
        {
            OtpResponce OtpResponce = new OtpResponce();
            #region Step 1 Send OTP
            try
            {
                clsOTPbyEmitra oSign = new clsOTPbyEmitra();
                oSign.aadharid = AAdharId;
                cls_eSign eSignRequest = new cls_eSign();
                OtpResponce = eSignRequest.GetOTPbyEmitra(oSign);
            }
            catch (Exception ex)
            {
                OtpResponce = new OtpResponce();
            }

            #endregion
            return OtpResponce;
        }

        public static clsVerifyOTPResponce VerifyOTPAndGenrateTransationbyEmitra(clsVerifyOTP VerifyOTP)
        {
            clsVerifyOTPResponce VerifyResponce = new clsVerifyOTPResponce();
            #region Step 2 Verify OTP
            try
            {

                VerifyOTP.otp = VerifyOTP.otp;
                VerifyOTP.transactionid = VerifyOTP.transactionid;

                cls_eSign eSignRequest = new cls_eSign();
                VerifyResponce = eSignRequest.VerifyOTPbyEmitra(VerifyOTP);
            }
            catch (Exception ex)
            {
                VerifyResponce = new clsVerifyOTPResponce();
            }

            #endregion
            return VerifyResponce;
        }

        public static clsDocumentESignByEmitraResponse GetTextFileEncrypted(clsDocumentESignByEmitra VerifyOTP)
        {
            clsDocumentESignByEmitraResponse VerifyResponce = new clsDocumentESignByEmitraResponse();
            #region Step 3 Get Text FIle
            try
            {
                VerifyOTP.filecontant = VerifyOTP.filecontant;
                VerifyOTP.transactionid = VerifyOTP.transactionid;

                cls_eSign eSignRequest = new cls_eSign();
                VerifyResponce = eSignRequest.GetDocumentESignByEmitra(VerifyOTP);
            }
            catch (Exception ex)
            {
                VerifyResponce = new clsDocumentESignByEmitraResponse();
            }

            #endregion
            return VerifyResponce;
        }


        public static clsUploadTextFileResponse UploadTextFile(clsUploadTextFile TextFileDetails, string SSOId)
        {
            clsUploadTextFileResponse VerifyResponce = new clsUploadTextFileResponse();
            #region Step 3 Get Text FIle
            try
            {
                cls_eSign eSignRequest = new cls_eSign();
                VerifyResponce = eSignRequest.UploadTextFile(TextFileDetails, SSOId);
            }
            catch (Exception ex)
            {
                VerifyResponce = new clsUploadTextFileResponse();
            }

            #endregion
            return VerifyResponce;
        }

    }

    public static class cls_ESignIntegrationByFRA
    {
        public static clsVerifyOTPResponce VerifyOTPData(clsVerifyOTP VerifyOTP, string requestID)
        {
            clsVerifyOTPResponce VerifyResponce = new clsVerifyOTPResponce();
            try
            {
                #region Step 2 Verify OTP
                VerifyResponce = new cls_eSign().VerifyOTP(VerifyOTP, requestID);
                #endregion
            }
            catch (Exception ex)
            {
                VerifyResponce = new clsVerifyOTPResponce();
            }
            return VerifyResponce;
        }
    }
}