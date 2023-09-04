using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FMDSS.Models;
using FMDSS.Models.Master;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.MIS;

namespace FMDSS.Models
{
    public static class htmlToPdfDownloadTickets
    {
        public static void htmlToPdf(string sb, string IDORFILENAME)
        {

            ////Start PDF

            StringReader sr = new StringReader(sb.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            StyleSheet styles = new StyleSheet();
            using (FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/" + IDORFILENAME + ".pdf"), FileMode.Create))
            {
                PdfWriter.GetInstance(pdfDoc, fs);
                using (StringReader stringReader = new StringReader(sb.ToString()))
                {
                    var parsedList = HTMLWorker.ParseToList(stringReader, styles);
                    pdfDoc.Open();
                    foreach (object item in parsedList)
                    {
                        pdfDoc.Add((IElement)item);
                    }
                    pdfDoc.Close();
                }
            }

            ////End PDF

        }


        public static string WildlifeDownloadPdf(DataSet ds, DataTable dtTC)
        {
            StringBuilder sb = new StringBuilder();

            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();

            DT1 = ds.Tables[0];
            DT2 = ds.Tables[1];
            DT3 = ds.Tables[2];

            string filepath = string.Empty;

         // filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/" + Convert.ToString(DateTime.Now.Minute+DateTime.Now.Second) + ".pdf");
            filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/" + Convert.ToString(DT1.Rows[0]["RequestID"]) + ".pdf");
            if (System.IO.File.Exists(filepath))
            {
                return filepath;
            }


            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;
            var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            /////create Table
            PdfPTable table;

            table = new PdfPTable(4);

            ////Add Logo

            string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
             string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");
            #region QRCOde
            string MyQRImageLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/MyQRImage.jpg");
            #endregion

            string pdfFile = filepath;


            #region QR Code

            string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(DT1.Rows[0]["RequestID"]).Substring(0, 18), "E-m!tr@2016");
            //string decVal = FMDSS.Models.EncodingDecoding.Decrypt(encData, "E-m!tr@2016");

            string QRCodePath = Utility.GenerateMyQCCode(encData, Convert.ToString(DT1.Rows[0]["RequestID"]).Substring(0, 18), "QRCodeReader/OnlineBoardingPassQRCode");
            if (!string.IsNullOrEmpty(QRCodePath))
            {
                iTextSharp.text.Image QRImageLogo = iTextSharp.text.Image.GetInstance(QRCodePath);
                QRImageLogo.ScaleToFit(95f, 95f);
                QRImageLogo.SpacingBefore = -50f;
                QRImageLogo.SpacingAfter = -50f;
                QRImageLogo.Alignment = Element.ALIGN_LEFT;

                PdfPCell cellQREmitraLogo;
                cellQREmitraLogo = new PdfPCell(QRImageLogo);
                cellQREmitraLogo.BorderWidth = 0;
                cellQREmitraLogo.Padding = -70;
                if (ds.Tables[0].Rows[0][3].ToString().Contains("C19-"))
                {
                    cellQREmitraLogo.PaddingTop = -130;
                }
                else
                {
                    cellQREmitraLogo.PaddingTop = -120;
                }
                cellQREmitraLogo.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellQREmitraLogo);
            }
            #endregion

           

            iTextSharp.text.Image RislImageLogo = iTextSharp.text.Image.GetInstance(imageURL);
            RislImageLogo.ScaleToFit(60f, 60f);
            RislImageLogo.SpacingBefore = -60f;
            RislImageLogo.SpacingAfter = -70f;
            RislImageLogo.Alignment = Element.ALIGN_CENTER;





            PdfPCell cellRislLogo;
            cellRislLogo = new PdfPCell(RislImageLogo);
            cellRislLogo.BorderWidth = 0;
            cellRislLogo.Padding = -50;
            if (ds.Tables[0].Rows[0][3].ToString().Contains("C19-"))
            {
                cellRislLogo.PaddingTop = -110;
            }
            else
            {
                cellRislLogo.PaddingTop = -80;
            }
            cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellRislLogo);



            ////Add Heading
            tableheading = new Paragraph("Goverment of", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Rajasthan", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Department of Forest", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);

            doc.Add(tableheading);

           

            tableheading = new Paragraph(" ", MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            PdfPCell cellHeading;
            cellHeading = new PdfPCell(tableheading);
            cellHeading.BorderWidth = 0;
            cellHeading.Padding = 0;
            cellHeading.PaddingTop = -20;

            table.AddCell(cellHeading);


            

            #region Remove Emitra Logo

            iTextSharp.text.Image EmitraImageLogo = iTextSharp.text.Image.GetInstance(eMitraLogo);
            EmitraImageLogo.ScaleToFit(120f, 150f);
            //if (ds.Tables[0].Rows[0][3].ToString().Contains("C19-"))
            //{
                
                //EmitraImageLogo.SpacingBefore = -150f;
                
            //}
            //else
            //{
                
                EmitraImageLogo.SpacingBefore = -70f;
                EmitraImageLogo.SpacingAfter = -70f;
            //}
           
            EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



            PdfPCell cellEmitraLogo;
            cellEmitraLogo = new PdfPCell(EmitraImageLogo);
            cellEmitraLogo.BorderWidth = 0;
            cellEmitraLogo.Padding = 7;
            if (ds.Tables[0].Rows[0][3].ToString().Contains("C19-"))
            {
                cellEmitraLogo.PaddingTop = -105;
            }
            else
            {
                cellEmitraLogo.PaddingTop = -75;
            }
            

            table.AddCell(cellEmitraLogo);
            #endregion

            doc.Add(new Paragraph(Environment.NewLine));



            PdfPTable tabular = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell(new Phrase("Booking Confirmation Slip", subheadfont));// { Border = 4};
            //tabular.TotalWidth = 320;
            tabular.SetTotalWidth(new float[] { 60f, 100f, 60f, 100f });
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Reserve:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["PlaceName"].ToString(), subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase("Booking No:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["RequestID"].ToString(), subheadfont));
            tabular.AddCell(cells);
            
            //cells = new PdfPCell(new Phrase("Date/Time & Booking:", subheadfont));
            //tabular.AddCell(cells);
            //cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
            //tabular.AddCell(cells);
            //cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
            //tabular.AddCell(cells);
            //cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
            //tabular.AddCell(cells);

            if (ds.Tables[0].Rows[0][3].ToString().Contains("C19-"))
            {
                cells = new PdfPCell(new Phrase("New Date Of Booking:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase("New Date of Visit", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Previous Date of Booking:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(ds.Tables[3].Rows[0]["EnteredOn"].ToString(), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase("Previous Date of Visit:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(ds.Tables[3].Rows[0]["DateOfArrival"].ToString(), subheadfont));
                tabular.AddCell(cells);
            }
            else
            {
                cells = new PdfPCell(new Phrase("Date Of Booking:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
                tabular.AddCell(cells);
            }

            cells = new PdfPCell(new Phrase("Booked Seats:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["NoofTicket"].ToString(), subheadfont));

            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Shift", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["Shift"].ToString(), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Vehicle", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT2.Rows[0]["VName"].ToString(), subheadfont));
            cells.Colspan = 3;
            tabular.AddCell(cells);

            doc.Add(tabular);

            doc.Add(table);


            ///End Logo 

            ///////

            //doc.Add(new Paragraph(Environment.NewLine));

            

            //doc.Add(new Paragraph(Environment.NewLine));
            PdfPTable VisitorDetails = null;
            PdfPCell cellsv = null;
            PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellsd;
            if (DT2.Columns.Contains("tatkalbooking") && Convert.ToInt16(DT2.Rows[0]["tatkalbooking"]) == 1)
            {
                #region New Member Details
                 VisitorDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                 cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 180;
                VisitorDetails.SetTotalWidth(new float[] { 10f, 35f, 35f, 20f });


                cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                Int16 k = 0;
                foreach (DataRow DR in DT2.Rows)
                {
                    k += Convert.ToInt16(1);
                    sb.Append("<tr><td col-lg-3>" + k + "</td><td col-lg-3>" + DR["Name"] + "</td><td col-lg-3>" + DR["Nationality"] + "</td><td col-lg-3>" + DR["IDProof"] + "</td><td col-lg-3>" + DR["NoOfCamera"] + "</td><td col-lg-3>" + DR["Shift"] + "</td><td col-lg-3>" + DR["VName"] + "</td><td col-lg-3>" + DR["Amount"] + "</td></tr>");

                    cellsv = new PdfPCell(new Phrase(k.ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["Name"].ToString() + " - " + DR["Nationality"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["IDProof"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                }
                doc.Add(VisitorDetails);

                VisitorDetails = new PdfPTable(9) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 180;
                VisitorDetails.SetTotalWidth(new float[] { 10f, 20f,15f, 15f, 15f, 15f, 15f, 15f,15f });

                cellsv = new PdfPCell(new Phrase("Seats for fees Calculation", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Member Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Camera Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Vehicle Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("GST (" + Convert.ToString(DT2.Rows[0]["BoardingVehicleFeeGSTPercentage"]) + "%) Vehicle Rent Fee", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("GST (" + Convert.ToString(DT2.Rows[0]["BoardingGuideFeeGSTPercentage"]) + "%) On Guide Fee", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Total Amount(INR)", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                Decimal MemberFees = Convert.ToDecimal(DT2.Rows[0]["MemberFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[0]["SeatPerEqment"]), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["MemberFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + MemberFees), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal CemaraFees = 0;
                    CemaraFees = (Convert.ToDecimal(DT2.Rows[0]["TotalCemaraFee"]) * Convert.ToInt32(DT2.Rows[0]["TotalNoOfCemara"]));
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["TotalCemaraFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["TotalNoOfCemara"]) + " ) = " + CemaraFees), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal VehicleFee = Convert.ToDecimal(DT2.Rows[0]["VehicleFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["VehicleFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + VehicleFee), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal BoardingVehicleFee = Convert.ToDecimal(DT2.Rows[0]["BoardingVehicleFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingVehicleFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingVehicleFee), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal BoardingVehicleFeeGSTAmount = Convert.ToDecimal(DT2.Rows[0]["BoardingVehicleFeeGSTAmount"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingVehicleFeeGSTAmount"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingVehicleFeeGSTAmount), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal BoardingGuideFee = Convert.ToDecimal(DT2.Rows[0]["BoardingGuideFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingGuideFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingGuideFee), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal BoardingGuideFeeGSTAmount = Convert.ToDecimal(DT2.Rows[0]["BoardingGuideFeeGSTAmount"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingGuideFeeGSTAmount"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingGuideFeeGSTAmount), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    Decimal Totalfess = MemberFees + CemaraFees + VehicleFee + BoardingVehicleFee + BoardingVehicleFeeGSTAmount + BoardingGuideFee + BoardingGuideFeeGSTAmount;


                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Totalfess), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                doc.Add(VisitorDetails);
               


                

                cellsd = new PdfPCell(new Phrase("Tatkal Charges:", subheadfont));// { Border = 4};
                SafariDetails.TotalWidth = 130;
                SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                SafariDetails.AddCell(cellsd);

                cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT2.Rows[0]["ExtraAmountTotal"])), subheadfont));
                cellsd.Colspan = 5;
                cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                SafariDetails.AddCell(cellsd);
                #endregion
            }
            else if (DT2.Columns.Contains("HalfDayFullDay") && Convert.ToInt16(DT2.Rows[0]["HalfDayFullDay"]) == 1) // half day Full Day
            {
                VisitorDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 180;
                VisitorDetails.SetTotalWidth(new float[] { 10f, 35f, 35f, 20f });


                cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                Int16 j = 0;
                foreach (DataRow DR in DT2.Rows)
                {
                    j += Convert.ToInt16(1);
                    sb.Append("<tr><td col-lg-3>" + j + "</td><td col-lg-3>" + DR["Name"] + "</td><td col-lg-3>" + DR["Nationality"] + "</td><td col-lg-3>" + DR["IDProof"] + "</td><td col-lg-3>" + DR["NoOfCamera"] + "</td><td col-lg-3>" + DR["Shift"] + "</td><td col-lg-3>" + DR["VName"] + "</td><td col-lg-3>" + DR["Amount"] + "</td></tr>");

                    cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["Name"].ToString() + " - " + DR["Nationality"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    //cellsv = new PdfPCell(new Phrase(DR["Nationality"].ToString(), subheadfont));
                    //VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["IDProof"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                }
                doc.Add(VisitorDetails);
                // Commented by shaan 30-03-2021
                //cellsd = new PdfPCell(new Phrase("Entrance Fee :", subheadfont));// { Border = 4};
                //SafariDetails.TotalWidth = 130;
                //SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                //SafariDetails.AddCell(cellsd);

                //cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT2.Rows[0]["Fee_TigerProject"]) * (Convert.ToDecimal(DT2.Rows[0]["SeatsPerEqpt"]))), subheadfont));
                //cellsd.Colspan = 5;
                //cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                //SafariDetails.AddCell(cellsd);

                //cellsd = new PdfPCell(new Phrase("Eco-Development Surcharges:", subheadfont));// { Border = 4};
                //SafariDetails.TotalWidth = 130;
                //SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                //SafariDetails.AddCell(cellsd);

                //cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT2.Rows[0]["Fee_Surcharge"]) * (Convert.ToDecimal(DT2.Rows[0]["SeatsPerEqpt"]))), subheadfont));
                //cellsd.Colspan = 5;
                //cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                //SafariDetails.AddCell(cellsd);
                //End Comment
                //Added by shaan 30-03-2021
                VisitorDetails = new PdfPTable(9) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 180;
                VisitorDetails.SetTotalWidth(new float[] { 10f, 20f, 15f, 15f, 15f, 15f, 15f, 15f, 15f });

                cellsv = new PdfPCell(new Phrase("Seats for fees Calculation", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Member Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Camera Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Vehicle Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("GST (" + Convert.ToString(DT2.Rows[0]["BoardingVehicleFeeGSTPercentage"]) + "%) Vehicle Rent Fee", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("GST (" + Convert.ToString(DT2.Rows[0]["BoardingGuideFeeGSTPercentage"]) + "%) On Guide Fee", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Total Amount(INR)", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                //Decimal MemberFees = Convert.ToDecimal(DT2.Rows[0]["MemberFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                Decimal MemberFees = Convert.ToDecimal("0.00");
                Decimal TotalOfTRDF = Convert.ToDecimal("0.00");
                Decimal TotalOfFee_TigerProject = Convert.ToDecimal("0.00");
                Decimal TotalOfFee_Surcharge = Convert.ToDecimal("0.00");
                Decimal TotalOfVehicle_TRDF = Convert.ToDecimal("0.00");
                Decimal TotalOfGuidFee_TRDF = Convert.ToDecimal("0.00");
                Decimal TotalCameraFee= Convert.ToDecimal("0.00");

                foreach (DataRow DR in DT2.Rows)
                {
                    TotalOfTRDF += Convert.ToDecimal(DR["TRDF"].ToString());
                    TotalOfFee_TigerProject += Convert.ToDecimal(DR["Fee_TigerProject"].ToString());
                    TotalOfFee_Surcharge += Convert.ToDecimal(DR["Fee_Surcharge"].ToString());
                    TotalOfVehicle_TRDF = Convert.ToDecimal(DR["Vehicle_TRDF"].ToString());
                    TotalOfGuidFee_TRDF = Convert.ToDecimal(DR["GuidFee_TRDF"].ToString());
                    TotalCameraFee += Convert.ToDecimal(DR["CameraFee"].ToString());
                }
                MemberFees = TotalOfTRDF + TotalOfFee_TigerProject + TotalOfFee_Surcharge + TotalOfVehicle_TRDF * 5 + TotalOfGuidFee_TRDF * 5;
                cellsv = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[0]["SeatPerEqment"]), subheadfont));
                VisitorDetails.AddCell(cellsv);
                //cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["MemberFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + MemberFees), subheadfont));
                cellsv = new PdfPCell(new Phrase(Convert.ToString(MemberFees), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal CemaraFees = 0;
                //CemaraFees = (Convert.ToDecimal(DT2.Rows[0]["TotalCemaraFee"]) * Convert.ToInt32(DT2.Rows[0]["TotalNoOfCemara"]));
                CemaraFees = TotalCameraFee;
                //cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["TotalCemaraFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["TotalNoOfCemara"]) + " ) = " + CemaraFees), subheadfont));
                cellsv = new PdfPCell(new Phrase(Convert.ToString(CemaraFees), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal VehicleFee = Convert.ToDecimal(DT2.Rows[0]["VehicleFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["VehicleFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + VehicleFee), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal BoardingVehicleFee = Convert.ToDecimal(DT2.Rows[0]["BoardingVehicleFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingVehicleFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingVehicleFee), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal BoardingVehicleFeeGSTAmount = Convert.ToDecimal(DT2.Rows[0]["BoardingVehicleFeeGSTAmount"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingVehicleFeeGSTAmount"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingVehicleFeeGSTAmount), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal BoardingGuideFee = Convert.ToDecimal(DT2.Rows[0]["BoardingGuideFee"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingGuideFee"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingGuideFee), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal BoardingGuideFeeGSTAmount = Convert.ToDecimal(DT2.Rows[0]["BoardingGuideFeeGSTAmount"]) * Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]);
                cellsv = new PdfPCell(new Phrase(Convert.ToString("( " + Convert.ToString(DT2.Rows[0]["BoardingGuideFeeGSTAmount"]) + " * " + Convert.ToInt32(DT2.Rows[0]["SeatPerEqment"]) + " ) = " + BoardingGuideFeeGSTAmount), subheadfont));
                VisitorDetails.AddCell(cellsv);

                Decimal Totalfess = MemberFees + CemaraFees + VehicleFee + BoardingVehicleFee + BoardingVehicleFeeGSTAmount + BoardingGuideFee + BoardingGuideFeeGSTAmount;


                cellsv = new PdfPCell(new Phrase(Convert.ToString(Totalfess), subheadfont));
                VisitorDetails.AddCell(cellsv);
                doc.Add(VisitorDetails);



                cellsd = new PdfPCell(new Phrase("Half/Full Day Charges:", subheadfont));// { Border = 4};
                SafariDetails.TotalWidth = 130;
                SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                SafariDetails.AddCell(cellsd);

                Decimal SumOFTigerAndSurchargeFee = Convert.ToDecimal(5 * Convert.ToDecimal(DT2.Rows[0]["Fees_TigerProjectHalfDayFullDayCharge"]) + Convert.ToDecimal(5 * Convert.ToDecimal(DT2.Rows[0]["Fee_SurchargeHalfDayFullDayCharge"])));
                cellsd = new PdfPCell(new Phrase(Convert.ToString(SumOFTigerAndSurchargeFee), subheadfont));
                cellsd.Colspan = 5;
                cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                SafariDetails.AddCell(cellsd);
                //End
            }
            else
            {
                #region Old Member Details
                 VisitorDetails = new PdfPTable(12) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                 cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 180;
                VisitorDetails.SetTotalWidth(new float[] { 5f, 32f, 28f, 8f, 8f, 8f, 8f, 8f, 15f, 8f, 12f, 10f });


                cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                //cellsv = new PdfPCell(new Phrase("Nationality", tableHeaderFont));
                //cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                //VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Member Fee", tableHeaderFont)); // 
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Camera Fee", tableHeaderFont)); // Camera
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Vehicle Fee", tableHeaderFont)); //Vehicle
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont)); // Vehicle Rent
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("GST(" + Convert.ToString(DT2.Rows[0]["BoardingVehicleFeeGSTPercentage"]) + "%) Vehicle Rent Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("GST(" + Convert.ToString(DT2.Rows[0]["BoardingGuideFeeGSTPercentage"]) + "%) on guide Fee", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("Amount(INR)", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                Int16 j = 0;
                foreach (DataRow DR in DT2.Rows)
                {
                    j += Convert.ToInt16(1);
                    sb.Append("<tr><td col-lg-3>" + j + "</td><td col-lg-3>" + DR["Name"] + "</td><td col-lg-3>" + DR["Nationality"] + "</td><td col-lg-3>" + DR["IDProof"] + "</td><td col-lg-3>" + DR["NoOfCamera"] + "</td><td col-lg-3>" + DR["Shift"] + "</td><td col-lg-3>" + DR["VName"] + "</td><td col-lg-3>" + DR["Amount"] + "</td></tr>");

                    cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["Name"].ToString() + " - " + DR["Nationality"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    //cellsv = new PdfPCell(new Phrase(DR["Nationality"].ToString(), subheadfont));
                    //VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["IDProof"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(DR["MemberFee"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["CameraFee"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["VehicleFee"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["BoardingVehicleFee"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["BoardingVehicleFeeGSTAmount"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["BoardingGuideFee"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DR["BoardingGuideFeeGSTAmount"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(DR["Amount"].ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);
                }
                doc.Add(VisitorDetails);

                #endregion
            }
            cellsd = new PdfPCell(new Phrase("Service Charges(INR):", subheadfont));// { Border = 4};
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["EmitraCharges"].ToString(), subheadfont));
            cellsd.Colspan = 5;
            cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
            SafariDetails.AddCell(cellsd);


            cellsd = new PdfPCell(new Phrase("Grand Total(INR) : ", subheadfont));// { Border = 4};
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
            cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT1.Rows[0]["AmountTobePaid"])), subheadfont));
            cellsd.Colspan = 5;
            cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
            cellsd.Colspan = 5;
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase("Contact Person :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["contactperson"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase("PhoneNo :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["PhoneNo"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase("Address :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Address"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);


            doc.Add(SafariDetails);


            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
            cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            TermsCondition.TotalWidth = 430;
            TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
            cellst.Colspan = 2;
            cellst.HorizontalAlignment = Element.ALIGN_LEFT;
            TermsCondition.AddCell(cellst);


            int index = 1;
            for (int i = 0; i < dtTC.Rows.Count; i++)
            {
                string sTC = Convert.ToString(dtTC.Rows[i]["TermAndCondition_Text"]).Replace("#PlaceName#", DT1.Rows[0]["PlaceName"].ToString());


                cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                TermsCondition.AddCell(cellst);
                cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                TermsCondition.AddCell(cellst);
                index += 1;
            }


            doc.Add(TermsCondition);



            //doc.Add(new Paragraph(Environment.NewLine));
            if (DT3.Rows.Count > 0)
            {

                PdfPTable Timing = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellt = new PdfPCell();

                cellt = new PdfPCell(new Phrase("Period", subheadfont));
                cellt.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                Timing.AddCell(cellt);
                cellt = new PdfPCell(new Phrase("Morning Trip", subheadfont));
                cellt.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                Timing.AddCell(cellt);
                cellt = new PdfPCell(new Phrase("Afternoon Trip", subheadfont));
                cellt.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                Timing.AddCell(cellt);

                Int16 k = 0;
                foreach (DataRow DR in DT3.Rows)
                {
                    k += Convert.ToInt16(1);
                    sb.Append("<tr> <td col-lg-3>" + DR["Period"] + "</td><td col-lg-3>" + DR["MorningTrip"] + "</td><td col-lg-3>" + DR["AfterNoonTrip"] + "</td>");

                    cellt = new PdfPCell(new Phrase(DR["Period"].ToString(), subheadfont));
                    Timing.AddCell(cellt);
                    cellt = new PdfPCell(new Phrase(DR["MorningTrip"].ToString(), subheadfont));
                    Timing.AddCell(cellt);
                    cellt = new PdfPCell(new Phrase(DR["AfterNoonTrip"].ToString(), subheadfont));
                    Timing.AddCell(cellt);
                }




                doc.Add(Timing);
            }
            doc.Close();

            return filepath;





        }

        public static string WildlifeDownloadPdfForOfficeUse(DataSet ds, DataTable dtTC)
        {

            StringBuilder sb = new StringBuilder();

            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();

            DT1 = ds.Tables[0];
            DT2 = ds.Tables[1];
            DT3 = ds.Tables[2];

            string filepath = string.Empty;

            filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/" + Convert.ToString(DT1.Rows[0]["RequestID"]) + "_ForOfficeUse.pdf");
            //if (System.IO.File.Exists(filepath))
            //{
            //    return filepath;
            //}


            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);

            //PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
            PdfWriter docWriter = PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
            string watermark = "For Office Use Only";
            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;
            var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
            doc.Open();
            doc.NewPage();
            iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 52, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.LightGray));
            ColumnText.ShowTextAligned(docWriter.DirectContent, Element.ALIGN_CENTER, new Phrase("FOR OFFICE USE ONLY", font), 297.5f, 421, 45);



            // doc.Add(new Paragraph(Environment.NewLine));
            /////create Table
            PdfPTable table;
            table = new PdfPTable(3);

            ////Add Logo

            string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
            string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");

            string pdfFile = filepath;

            iTextSharp.text.Image RislImageLogo = iTextSharp.text.Image.GetInstance(imageURL);
            RislImageLogo.ScaleToFit(60f, 60f);
            RislImageLogo.SpacingBefore = -70f;
            RislImageLogo.SpacingAfter = -70f;
            RislImageLogo.Alignment = Element.ALIGN_CENTER;





            PdfPCell cellRislLogo;
            cellRislLogo = new PdfPCell(RislImageLogo);
            cellRislLogo.BorderWidth = 0;
            cellRislLogo.Padding = -30;
            cellRislLogo.PaddingTop = -80;
            cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellRislLogo);



            ////Add Heading
            tableheading = new Paragraph("Goverment of", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Rajasthan.", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Department of Forest,", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);

            doc.Add(tableheading);


           

            tableheading = new Paragraph(" ", MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            PdfPCell cellHeading;
            cellHeading = new PdfPCell(tableheading);
            cellHeading.BorderWidth = 0;
            cellHeading.Padding = 0;
            cellHeading.PaddingTop = -20;

            table.AddCell(cellHeading);


            iTextSharp.text.Image EmitraImageLogo = iTextSharp.text.Image.GetInstance(eMitraLogo);
            EmitraImageLogo.ScaleToFit(130f, 170f);
            EmitraImageLogo.SpacingBefore = -70f;
            EmitraImageLogo.SpacingAfter = -70f;
            EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



            PdfPCell cellEmitraLogo;
            cellEmitraLogo = new PdfPCell(EmitraImageLogo);
            cellEmitraLogo.BorderWidth = 0;
            cellEmitraLogo.Padding = 7;
            cellEmitraLogo.PaddingTop = -75;

            table.AddCell(cellEmitraLogo);
            doc.Add(new Paragraph(Environment.NewLine));

            //PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            //PdfContentByte cb = writer.DirectContent;
            // cb.Stroke();


            PdfPTable tabular = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell(new Phrase("Booking Confirmation Slip", subheadfont));// { Border = 4};
            //tabular.TotalWidth = 320;
            tabular.SetTotalWidth(new float[] { 60f, 100f, 60f, 100f });
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Reserve:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["PlaceName"].ToString(), subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase("Booking No:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["RequestID"].ToString(), subheadfont));
            tabular.AddCell(cells);
         
            //cells = new PdfPCell(new Phrase("Date/Time & Booking:", subheadfont));
            //tabular.AddCell(cells);
            //cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
            //tabular.AddCell(cells);
            //cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
            //tabular.AddCell(cells);
            //cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
            //tabular.AddCell(cells);

            if (ds.Tables[0].Rows[0][3].ToString().Contains("C19-"))
            {
                cells = new PdfPCell(new Phrase("New Date of Booking:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase("New Date of Visit", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Previous Date of Booking:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(ds.Tables[3].Rows[0]["EnteredOn"].ToString(), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase("Previous Date of Visit:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(ds.Tables[3].Rows[0]["DateOfArrival"].ToString(), subheadfont));
                tabular.AddCell(cells);
            }
            else
            {
                cells = new PdfPCell(new Phrase("Date Of Booking:", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
                tabular.AddCell(cells);
            }

            cells = new PdfPCell(new Phrase("Booked Seats:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["NoofTicket"].ToString(), subheadfont));

            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Shift", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["Shift"].ToString(), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Vehicle", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT2.Rows[0]["VName"].ToString(), subheadfont));
            cells.Colspan = 3;
            tabular.AddCell(cells);

            doc.Add(tabular);

            doc.Add(table);


            ///End Logo 

            ///////

            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable VisitorDetails = new PdfPTable(12) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellsv = new PdfPCell();// { Border = 4};
            VisitorDetails.TotalWidth = 180;
            VisitorDetails.SetTotalWidth(new float[] { 5f, 32f, 28f, 8f, 8f, 8f, 8f, 8f, 15f, 8f, 12f, 10f });


            cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            //cellsv = new PdfPCell(new Phrase("Nationality", tableHeaderFont));
            //cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Member Fee", tableHeaderFont)); // 
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Camera Fee", tableHeaderFont)); // Camera
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Vehicle Fee", tableHeaderFont)); //Vehicle
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont)); // Vehicle Rent
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);

            cellsv = new PdfPCell(new Phrase("GST(" + Convert.ToString(DT2.Rows[0]["BoardingVehicleFeeGSTPercentage"]) + "%) Vehicle Rent Fee", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);

            cellsv = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);

            cellsv = new PdfPCell(new Phrase("GST(" + Convert.ToString(DT2.Rows[0]["BoardingGuideFeeGSTPercentage"]) + "%) on guide Fee", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);

            cellsv = new PdfPCell(new Phrase("Amount(INR)", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);

            Int16 j = 0;
            foreach (DataRow DR in DT2.Rows)
            {
                j += Convert.ToInt16(1);
                sb.Append("<tr><td col-lg-3>" + j + "</td><td col-lg-3>" + DR["Name"] + "</td><td col-lg-3>" + DR["Nationality"] + "</td><td col-lg-3>" + DR["IDProof"] + "</td><td col-lg-3>" + DR["NoOfCamera"] + "</td><td col-lg-3>" + DR["Shift"] + "</td><td col-lg-3>" + DR["VName"] + "</td><td col-lg-3>" + DR["Amount"] + "</td></tr>");

                cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["Name"].ToString() + " - " + DR["Nationality"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                //cellsv = new PdfPCell(new Phrase(DR["Nationality"].ToString(), subheadfont));
                //VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["IDProof"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase(DR["MemberFee"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["CameraFee"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["VehicleFee"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["BoardingVehicleFee"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["BoardingVehicleFeeGSTAmount"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["BoardingGuideFee"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["BoardingGuideFeeGSTAmount"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase(DR["Amount"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
            }
            doc.Add(VisitorDetails);






            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };


            PdfPCell cellsd;

            cellsd = new PdfPCell(new Phrase("Service Charges(INR):", subheadfont));// { Border = 4};
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["EmitraCharges"].ToString(), subheadfont));
            cellsd.Colspan = 5;
            cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
            SafariDetails.AddCell(cellsd);


            cellsd = new PdfPCell(new Phrase("Grand Total(INR) : ", subheadfont));// { Border = 4};
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
            cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT1.Rows[0]["AmountTobePaid"])), subheadfont));
            cellsd.Colspan = 5;
            cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
            cellsd.Colspan = 5;
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase("Contact Person :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["contactperson"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase("PhoneNo :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["PhoneNo"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase("Address :", subheadfont));
            SafariDetails.AddCell(cellsd);
            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Address"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);


            doc.Add(SafariDetails);


            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
            cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            TermsCondition.TotalWidth = 430;
            TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
            cellst.Colspan = 2;
            cellst.HorizontalAlignment = Element.ALIGN_LEFT;
            TermsCondition.AddCell(cellst);


            int index = 1;
            for (int i = 0; i < dtTC.Rows.Count; i++)
            {
                string sTC = Convert.ToString(dtTC.Rows[i]["TermAndCondition_Text"]).Replace("#PlaceName#", DT1.Rows[0]["PlaceName"].ToString());


                cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                TermsCondition.AddCell(cellst);
                cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                TermsCondition.AddCell(cellst);
                index += 1;
            }


            doc.Add(TermsCondition);



            //doc.Add(new Paragraph(Environment.NewLine));
            if (DT3.Rows.Count > 0)
            {

                PdfPTable Timing = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellt = new PdfPCell();

                cellt = new PdfPCell(new Phrase("Period", subheadfont));
                cellt.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                Timing.AddCell(cellt);
                cellt = new PdfPCell(new Phrase("Morning Trip", subheadfont));
                cellt.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                Timing.AddCell(cellt);
                cellt = new PdfPCell(new Phrase("Afternoon Trip", subheadfont));
                cellt.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                Timing.AddCell(cellt);

                Int16 k = 0;
                foreach (DataRow DR in DT3.Rows)
                {
                    k += Convert.ToInt16(1);
                    sb.Append("<tr> <td col-lg-3>" + DR["Period"] + "</td><td col-lg-3>" + DR["MorningTrip"] + "</td><td col-lg-3>" + DR["AfterNoonTrip"] + "</td>");

                    cellt = new PdfPCell(new Phrase(DR["Period"].ToString(), subheadfont));
                    Timing.AddCell(cellt);
                    cellt = new PdfPCell(new Phrase(DR["MorningTrip"].ToString(), subheadfont));
                    Timing.AddCell(cellt);
                    cellt = new PdfPCell(new Phrase(DR["AfterNoonTrip"].ToString(), subheadfont));
                    Timing.AddCell(cellt);
                }




                doc.Add(Timing);
            }
            doc.Close();

            return filepath;





        }

        public static string WildlifeBoardingPassDownloadPdfForOffice(DataTable DT1, DataTable dtTC)
        {

            StringBuilder sb = new StringBuilder();

            //DataTable DT1 = new DataTable();
            //DataTable DT2 = new DataTable();
            //DataTable DT3 = new DataTable();

            //DT1 = ds;
            //DT2 = ds;
            //DT3 = ds;

            string filepath = string.Empty;

            filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/BoardingPass_" + Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18) + "_ForOfficeUse.pdf");

            //if (System.IO.File.Exists(filepath))
            //{
            //    return filepath;
            //}


            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);

            PdfWriter docWriter = PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;
            var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
            doc.Open();
            doc.NewPage();
            iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 52, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.LightGray));
            ColumnText.ShowTextAligned(docWriter.DirectContent, Element.ALIGN_CENTER, new Phrase("FOR OFFICE USE ONLY", font), 297.5f, 421, 45);

            // doc.Add(new Paragraph(Environment.NewLine));

            /////create Table

            PdfPTable table;

            table = new PdfPTable(3);

            ////Add Logo

            string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
            string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");

            string pdfFile = filepath;

            iTextSharp.text.Image RislImageLogo = iTextSharp.text.Image.GetInstance(imageURL);
            RislImageLogo.ScaleToFit(60f, 60f);
            RislImageLogo.SpacingBefore = -70f;
            RislImageLogo.SpacingAfter = -70f;
            RislImageLogo.Alignment = Element.ALIGN_CENTER;





            PdfPCell cellRislLogo;
            cellRislLogo = new PdfPCell(RislImageLogo);
            cellRislLogo.BorderWidth = 0;
            cellRislLogo.Padding = -30;
            cellRislLogo.PaddingTop = -80;
            cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellRislLogo);



            ////Add Heading
            tableheading = new Paragraph("Goverment of", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Rajasthan.", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Department of Forest,", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);

            doc.Add(tableheading);

           

            tableheading = new Paragraph(" ", MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            PdfPCell cellHeading;
            cellHeading = new PdfPCell(tableheading);
            cellHeading.BorderWidth = 0;
            cellHeading.Padding = 0;
            cellHeading.PaddingTop = -20;

            table.AddCell(cellHeading);



            iTextSharp.text.Image EmitraImageLogo = iTextSharp.text.Image.GetInstance(eMitraLogo);
            EmitraImageLogo.ScaleToFit(130f, 170f);
            EmitraImageLogo.SpacingBefore = -70f;
            EmitraImageLogo.SpacingAfter = -70f;
            EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



            PdfPCell cellEmitraLogo;
            cellEmitraLogo = new PdfPCell(EmitraImageLogo);
            cellEmitraLogo.BorderWidth = 0;
            cellEmitraLogo.Padding = 50;
            cellEmitraLogo.PaddingTop = -75;

            table.AddCell(cellEmitraLogo);
            doc.Add(new Paragraph(Environment.NewLine));

            //PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            //PdfContentByte cb = writer.DirectContent;
            // cb.Stroke();


            PdfPTable tabular = new PdfPTable(9) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell(new Phrase("Boarding Pass", subheadfont));// { Border = 4};
            tabular.TotalWidth = 320;
            tabular.SetTotalWidth(new float[] { 22f, 30f, 25f, 27f, 20f, 25f, 30f, 30f, 30f });
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cells.Colspan = 9;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            tabular.AddCell(cells);


            cells = new PdfPCell(new Phrase("Boarding Pass : " + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            tabular.AddCell(cells);


            cells = new PdfPCell(new Phrase("Booking No : " + Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18), subheadfont));
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabular.AddCell(cells);


            cells = new PdfPCell(new Phrase("Visit Date", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Reservation Date", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Route Name", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Total Members", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Shift", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Guide Name", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Ticket Amount", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Mode Of Booking", subheadfont));
            tabular.AddCell(cells);




            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["DateOfVisit"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ReservatindDate"]), subheadfont));
            tabular.AddCell(cells);



            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ZoneName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows.Count), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ShiftTime"]), subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleNumber"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["GuidName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["AmountTobePaid"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ModeOfBooking"]), subheadfont));
            tabular.AddCell(cells);


            doc.Add(tabular);
            doc.Add(table);

            ///End Logo 

            ///////

            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable VisitorDetails = new PdfPTable(5) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellsv = new PdfPCell();// { Border = 4};
            VisitorDetails.TotalWidth = 180;

            VisitorDetails.SetTotalWidth(new float[] { 15f, 35f, 20f, 30f, 20f });


            cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Nationality", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);


            Int16 j = 0;
            foreach (DataRow DR in DT1.Rows)
            {
                j += Convert.ToInt16(1);

                cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NameOfVisitor"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["Nationality"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["IdproofIdDetails"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
            }
            doc.Add(VisitorDetails);






            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };


            PdfPCell cellsd;

            cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);

            doc.Add(SafariDetails);



            //==========================================================================================================================
            PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell celltermcondition = new PdfPCell();// { Border = 4};
            TermsCondition.TotalWidth = 180;
            TermsCondition.SetTotalWidth(new float[] { 30f, 400f });


            celltermcondition = new PdfPCell(new Phrase("Terms and conditions for Visitors", tableHeaderFont));
            celltermcondition.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            celltermcondition = new PdfPCell(new Phrase("The visitor must reach the boarding point at least 15 minutes prior to the departure time.", subheadfont));
            TermsCondition.TotalWidth = 130;
            TermsCondition.SetTotalWidth(new float[] { 24f, 100f });
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            celltermcondition = new PdfPCell(new Phrase("Any violation of rules will be punishable under wildlife protection Act 1972.", subheadfont));
            TermsCondition.TotalWidth = 130;
            TermsCondition.SetTotalWidth(new float[] { 24f, 100f });
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            doc.Add(TermsCondition);

            //PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            //PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
            //cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //TermsCondition.TotalWidth = 430;
            //TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
            //cellst.Colspan = 2;
            //cellst.HorizontalAlignment = Element.ALIGN_LEFT;
            //TermsCondition.AddCell(cellst);

            //doc.Add(TermsCondition);



            PdfPTable DOsAndDont = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            PdfPCell DOsAndDontcells = new PdfPCell();// { Border = 4};
            DOsAndDont.TotalWidth = 180;
            DOsAndDont.SetTotalWidth(new float[] { 30f, 400f });


            DOsAndDontcells = new PdfPCell(new Phrase("Abide by the rules of the :" + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
            DOsAndDontcells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);


            DOsAndDontcells = new PdfPCell(new Phrase("DO's :", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("1). Enter the park with a valid ticket.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("2). Take official guide with you inside the park.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("3). maximum speed limit is 20 km/hr.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("4). Always carry drinking water.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("5). Maintain silence and discipline during excursions.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("6). Allow the animals to have the right of way.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("7). Wear colors which match with Nature.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("8). Please carry maps/Guide Book for your reference.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);


            DOsAndDontcells = new PdfPCell(new Phrase("DONT'S :", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("1). don't get down, unless told by the guide.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("2). Don't carry arms,explosives or intoxicants inside the park.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("3). Don't blow horn.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("4). Don't litter with cans,bottles, plastic bags etc.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("5). Don't try to feed the animals.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("6). Don't smoke or lit fire.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("7). Don't tease or chase the animals.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("8). Don't leave plastic/Polybags.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            doc.Add(DOsAndDont);



            //PdfPCell DOsAndDontcells = new PdfPCell(new Phrase("Abide by the rules of the :" + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));// { Border = 4};
            //DOsAndDontcells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //DOsAndDont.TotalWidth = 430;
            //DOsAndDont.SetTotalWidth(new float[] { 30f, 400f });
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDontcells.HorizontalAlignment = Element.ALIGN_LEFT;
            //DOsAndDont.AddCell(DOsAndDontcells);

            //string doss = "DO's : - 1). Enter the park with a valid ticket. 2). Take official guide with you inside the park. 3) maximum speed limit is 20 km/hr.  4). Always carry drinking water. 5). Maintain silence and discipline during excursions. 6). Allow the animals to have the right of way. 7). Wear colors which match with Nature. 8). Please carry maps/Guide Book for your reference 9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass. ";
            //DOsAndDontcells = new PdfPCell(new Phrase(doss, subheadfont));
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDont.AddCell(cellst);

            //string dontss = "DONT'S : - 1). don't get down, unless told by the guide. 2). Don't carry arms,explosives or intoxicants inside the park. 3). Don't blow horn. 4). Don't litter with cans,bottles, plastic bags etc. 5). Don't try to feed the animals. 6). Don't smoke or lit fire. 7). Don't tease or chase the animals. 8). Don't leave plastic/Polybags.";
            //DOsAndDontcells = new PdfPCell(new Phrase(dontss, subheadfont));
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDont.AddCell(cellst);

            doc.Close();

            return filepath;

        }

        public static string WildlifeBoardingPassDownloadPdf_New(DataTable DT1, DataTable dtTC)
        {
            StringBuilder sb = new StringBuilder();

            //DataTable DT1 = new DataTable();
            //DataTable DT2 = new DataTable();
            //DataTable DT3 = new DataTable();

            //DT1 = ds;
            //DT2 = ds;
            //DT3 = ds;

            string filepath = string.Empty;

            filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/BoardingPass_" + Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18) + ".pdf");

            //if (System.IO.File.Exists(filepath))
            //{
            //    return filepath;
            //}


            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;
            var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));

            /////create Table

            PdfPTable table;

            table = new PdfPTable(3);

            ////Add Logo

            string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
            string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");
            string MyQRImage = System.Web.HttpContext.Current.Server.MapPath("~/images/MyQRImage.jpg");

            string pdfFile = filepath;

            iTextSharp.text.Image RislImageLogo = iTextSharp.text.Image.GetInstance(imageURL);
            RislImageLogo.ScaleToFit(60f, 60f);
            RislImageLogo.SpacingBefore = -70f;
            RislImageLogo.SpacingAfter = -70f;
            RislImageLogo.Alignment = Element.ALIGN_CENTER;





            PdfPCell cellRislLogo;
            cellRislLogo = new PdfPCell(RislImageLogo);
            cellRislLogo.BorderWidth = 0;
            cellRislLogo.Padding = -30;
            cellRislLogo.PaddingTop = -80;
            cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellRislLogo);



            ////Add Heading
            tableheading = new Paragraph("Goverment of", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Rajasthan.", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Department of Forest,", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);

            doc.Add(tableheading);

            

            tableheading = new Paragraph(" ", MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            PdfPCell cellHeading;
            cellHeading = new PdfPCell(tableheading);
            cellHeading.BorderWidth = 0;
            cellHeading.Padding = 0;
            cellHeading.PaddingTop = -20;

            table.AddCell(cellHeading);



            iTextSharp.text.Image EmitraImageLogo = iTextSharp.text.Image.GetInstance(eMitraLogo);
            EmitraImageLogo.ScaleToFit(130f, 170f);
            EmitraImageLogo.SpacingBefore = -70f;
            EmitraImageLogo.SpacingAfter = -70f;
            EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



            PdfPCell cellEmitraLogo;
            cellEmitraLogo = new PdfPCell(EmitraImageLogo);
            cellEmitraLogo.BorderWidth = 0;
            cellEmitraLogo.Padding = 50;
            cellEmitraLogo.PaddingTop = -75;

            table.AddCell(cellEmitraLogo);
            doc.Add(new Paragraph(Environment.NewLine));

            //PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            //PdfContentByte cb = writer.DirectContent;
            // cb.Stroke();


            PdfPTable tabular = new PdfPTable(8) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell(new Phrase("Boarding Pass", subheadfont));// { Border = 4};
            tabular.TotalWidth = 320;
            tabular.SetTotalWidth(new float[] { 22f, 30f, 25f, 27f, 20f, 25f, 30f, 30f });
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cells.Colspan = 8;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            tabular.AddCell(cells);


            cells = new PdfPCell(new Phrase("Boarding Pass : " + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            tabular.AddCell(cells);


            //New Change Roster 16-09-2019 by amit
            string label = string.Empty;
            label=  (Convert.ToString(DT1.Rows[0]["SystemGeneratedVehicleNumber"])=="" ? "" : "Vehicle :");
            cells = new PdfPCell(new Phrase(label + Convert.ToString(DT1.Rows[0]["SystemGeneratedVehicleNumber"]), subheadfont));
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            tabular.AddCell(cells);
            


            cells = new PdfPCell(new Phrase("Booking No : " + Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18), subheadfont));
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Visit Date", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Reservation Date", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Route Name", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Total Members", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Shift", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Vehicle", subheadfont));
            tabular.AddCell(cells); 
             
            cells = new PdfPCell(new Phrase("Ticket Amount", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Mode Of Booking", subheadfont));
            tabular.AddCell(cells); 

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["DateOfVisit"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ReservatindDate"]), subheadfont));
            tabular.AddCell(cells); 

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ZoneName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows.Count), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ShiftTime"]), subheadfont));
            tabular.AddCell(cells); 

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleName"]), subheadfont));
            tabular.AddCell(cells);  

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["AmountTobePaid"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ModeOfBooking"]), subheadfont));
            tabular.AddCell(cells);


            doc.Add(tabular);
            doc.Add(table);

            ///End Logo 

            ///////

            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable VisitorDetails = new PdfPTable(7) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellsv = new PdfPCell();// { Border = 4};
            VisitorDetails.TotalWidth = 180;

            VisitorDetails.SetTotalWidth(new float[] { 15f, 35f, 20f, 30f, 20f,20f,20f });


            cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv); 
            cellsv = new PdfPCell(new Phrase("Guide Name", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Vehicle No", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Nationality", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);


            Int16 j = 0;
            foreach (DataRow DR in DT1.Rows)
            {
                j += Convert.ToInt16(1);

                cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NameOfVisitor"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["GuidName"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["VehicleNumber"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["Nationality"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["IdproofIdDetails"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
            }
            doc.Add(VisitorDetails);






            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };


            PdfPCell cellsd;

            cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);

            doc.Add(SafariDetails);

            //=================================================Extra Revised AMount ============================================

            SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            cellsd = new PdfPCell(new Phrase("Remaining Payable Amount :", subheadfont));
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ExtraAmountRevised"]), subheadfont));
            SafariDetails.AddCell(cellsd);

            doc.Add(SafariDetails);



            //==========================================================================================================================
            PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell celltermcondition = new PdfPCell();// { Border = 4};
            TermsCondition.TotalWidth = 180;
            TermsCondition.SetTotalWidth(new float[] { 30f, 400f });


            celltermcondition = new PdfPCell(new Phrase("Terms and conditions for Visitors", tableHeaderFont));
            celltermcondition.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            celltermcondition = new PdfPCell(new Phrase("The visitor must reach the boarding point at least 15 minutes prior to the departure time.", subheadfont));
            TermsCondition.TotalWidth = 130;
            TermsCondition.SetTotalWidth(new float[] { 24f, 100f });
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            celltermcondition = new PdfPCell(new Phrase("Any violation of rules will be punishable under wildlife protection Act 1972.", subheadfont));
            TermsCondition.TotalWidth = 130;
            TermsCondition.SetTotalWidth(new float[] { 24f, 100f });
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            doc.Add(TermsCondition);

            //PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            //PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
            //cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //TermsCondition.TotalWidth = 430;
            //TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
            //cellst.Colspan = 2;
            //cellst.HorizontalAlignment = Element.ALIGN_LEFT;
            //TermsCondition.AddCell(cellst);

            //doc.Add(TermsCondition);



            PdfPTable DOsAndDont = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            PdfPCell DOsAndDontcells = new PdfPCell();// { Border = 4};
            DOsAndDont.TotalWidth = 180;
            DOsAndDont.SetTotalWidth(new float[] { 30f, 400f });


            DOsAndDontcells = new PdfPCell(new Phrase("Abide by the rules of the :" + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
            DOsAndDontcells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);


            DOsAndDontcells = new PdfPCell(new Phrase("DO's :", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("1). Enter the park with a valid ticket.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("2). Take official guide with you inside the park.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("3). maximum speed limit is 20 km/hr.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("4). Always carry drinking water.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("5). Maintain silence and discipline during excursions.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("6). Allow the animals to have the right of way.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("7). Wear colors which match with Nature.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("8). Please carry maps/Guide Book for your reference.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);


            DOsAndDontcells = new PdfPCell(new Phrase("DONT'S :", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("1). don't get down, unless told by the guide.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("2). Don't carry arms,explosives or intoxicants inside the park.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("3). Don't blow horn.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("4). Don't litter with cans,bottles, plastic bags etc.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("5). Don't try to feed the animals.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("6). Don't smoke or lit fire.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("7). Don't tease or chase the animals.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("8). Don't leave plastic/Polybags.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            doc.Add(DOsAndDont);

            #region ADD QR CODE
            string encData = FMDSS.Models.MySecurity.SecurityCode.Encode(Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18));
            //string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18), "E-m!tr@2016");
            //string decVal = FMDSS.Models.EncodingDecoding.Decrypt(encData, "E-m!tr@2016");

            string QRCodePath = Utility.GenerateMyQCCode(encData, Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18), "QRCodeReader/OnlineBoardingPassQRCode");
            if (!string.IsNullOrEmpty(QRCodePath))
            {
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));

                PdfPTable table1 = new PdfPTable(3);

                ////Add Logo

                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);

                doc.Add(tableheading);



                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(" ", MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                cellHeading = new PdfPCell(tableheading);
                cellHeading.BorderWidth = 0;
                cellHeading.Padding = 0;
                cellHeading.PaddingBottom = -20;

                table1.AddCell(cellHeading);



                ////Add Heading
                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);

                doc.Add(tableheading);



                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(" ", MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                cellHeading = new PdfPCell(tableheading);
                cellHeading.BorderWidth = 0;
                cellHeading.Padding = 0;
                cellHeading.PaddingBottom = -20;

                table1.AddCell(cellHeading);



                EmitraImageLogo = iTextSharp.text.Image.GetInstance(QRCodePath);
                EmitraImageLogo.ScaleToFit(130f, 170f);
                EmitraImageLogo.SpacingBefore = -70f;
                EmitraImageLogo.SpacingAfter = -70f;
                EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



                cellEmitraLogo = new PdfPCell(EmitraImageLogo);
                cellEmitraLogo.BorderWidth = 0;
                cellEmitraLogo.Padding = 50;
                cellEmitraLogo.PaddingBottom = -95;

                table1.AddCell(cellEmitraLogo);
                doc.Add(new Paragraph(Environment.NewLine));

                doc.Add(table1);

            }
            #endregion

            //PdfPCell DOsAndDontcells = new PdfPCell(new Phrase("Abide by the rules of the :" + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));// { Border = 4};
            //DOsAndDontcells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //DOsAndDont.TotalWidth = 430;
            //DOsAndDont.SetTotalWidth(new float[] { 30f, 400f });
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDontcells.HorizontalAlignment = Element.ALIGN_LEFT;
            //DOsAndDont.AddCell(DOsAndDontcells);

            //string doss = "DO's : - 1). Enter the park with a valid ticket. 2). Take official guide with you inside the park. 3) maximum speed limit is 20 km/hr.  4). Always carry drinking water. 5). Maintain silence and discipline during excursions. 6). Allow the animals to have the right of way. 7). Wear colors which match with Nature. 8). Please carry maps/Guide Book for your reference 9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass. ";
            //DOsAndDontcells = new PdfPCell(new Phrase(doss, subheadfont));
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDont.AddCell(cellst);

            //string dontss = "DONT'S : - 1). don't get down, unless told by the guide. 2). Don't carry arms,explosives or intoxicants inside the park. 3). Don't blow horn. 4). Don't litter with cans,bottles, plastic bags etc. 5). Don't try to feed the animals. 6). Don't smoke or lit fire. 7). Don't tease or chase the animals. 8). Don't leave plastic/Polybags.";
            //DOsAndDontcells = new PdfPCell(new Phrase(dontss, subheadfont));
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDont.AddCell(cellst);

            doc.Close();

            return filepath;

        }
         
        public static string WildlifeBoardingPassDownloadPdf(DataTable DT1, DataTable dtTC)
        {
            StringBuilder sb = new StringBuilder();

            //DataTable DT1 = new DataTable();
            //DataTable DT2 = new DataTable();
            //DataTable DT3 = new DataTable();

            //DT1 = ds;
            //DT2 = ds;
            //DT3 = ds;

            string filepath = string.Empty;
            string BordingNo = string.Empty;
            
            if (DT1.Rows[0]["BoardingID"].ToString().Contains("C19-"))
            {
                BordingNo = Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 22);
            }
            else
            {
                BordingNo = Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18);
               
            }
            filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/BoardingPass_" + BordingNo + ".pdf");
            //if (System.IO.File.Exists(filepath))
            //{
            //    return filepath;
            //}


            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheadingupper = null;     //Added by shaan 07-04-2021
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;
            var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
            var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));

            /////create Table

            PdfPTable table;

            table = new PdfPTable(3);

            ////Add Logo

            string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
            string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");
            string MyQRImage = System.Web.HttpContext.Current.Server.MapPath("~/images/MyQRImage.jpg");

            string pdfFile = filepath;

            iTextSharp.text.Image RislImageLogo = iTextSharp.text.Image.GetInstance(imageURL);
            RislImageLogo.ScaleToFit(60f, 60f);
            RislImageLogo.SpacingBefore = -70f;
            RislImageLogo.SpacingAfter = -70f;
            RislImageLogo.Alignment = Element.ALIGN_CENTER;





            PdfPCell cellRislLogo;
            cellRislLogo = new PdfPCell(RislImageLogo);
            cellRislLogo.BorderWidth = 0;
            cellRislLogo.Padding = -30;
            cellRislLogo.PaddingTop = -80;
            cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
            table.AddCell(cellRislLogo);



            ////Add Heading
            tableheading = new Paragraph("Goverment of", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Rajasthan.", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            tableheading = new Paragraph("Department of Forest,", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);

            doc.Add(tableheading);



            tableheading = new Paragraph(" ", MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);

            PdfPCell cellHeading;
            cellHeading = new PdfPCell(tableheading);
            cellHeading.BorderWidth = 0;
            cellHeading.Padding = 0;
            cellHeading.PaddingTop = -20;

            table.AddCell(cellHeading);



            iTextSharp.text.Image EmitraImageLogo = iTextSharp.text.Image.GetInstance(eMitraLogo);
            EmitraImageLogo.ScaleToFit(130f, 170f);
            EmitraImageLogo.SpacingBefore = -70f;
            EmitraImageLogo.SpacingAfter = -70f;
            EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



            PdfPCell cellEmitraLogo;
            cellEmitraLogo = new PdfPCell(EmitraImageLogo);
            cellEmitraLogo.BorderWidth = 0;
            cellEmitraLogo.Padding = 50;
            cellEmitraLogo.PaddingTop = -75;

            table.AddCell(cellEmitraLogo);
            doc.Add(new Paragraph(Environment.NewLine));

            //PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            //PdfContentByte cb = writer.DirectContent;
            // cb.Stroke();
            //Added by shaan 07-04-2021---Genareted Bording pass date time
            string GeneratedOn = "Generated On : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            tableheadingupper = new Paragraph(GeneratedOn, MyFont);
            tableheadingupper.Font.Size = 8;
            tableheadingupper.Alignment = (Element.ALIGN_RIGHT);
            tableheadingupper.SpacingAfter = 5;
            doc.Add(tableheadingupper);
            //End shaan 07-04-2021


            PdfPTable tabular = new PdfPTable(9) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell(new Phrase("Boarding Pass", subheadfont));// { Border = 4};
            tabular.TotalWidth = 320;
            tabular.SetTotalWidth(new float[] { 22f, 30f, 25f, 27f, 20f, 25f, 30f, 30f, 30f });
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            cells.Colspan = 9;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            tabular.AddCell(cells);


            cells = new PdfPCell(new Phrase("Boarding Pass : " + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            tabular.AddCell(cells);


            //New Change Roster 16-09-2019 by amit
            string label = string.Empty;
            label = (Convert.ToString(DT1.Rows[0]["SystemGeneratedVehicleNumber"]) == "" ? "" : "Vehicle :");
            cells = new PdfPCell(new Phrase(label + Convert.ToString(DT1.Rows[0]["SystemGeneratedVehicleNumber"]), subheadfont));
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            tabular.AddCell(cells);



            cells = new PdfPCell(new Phrase("Booking No : " + BordingNo, subheadfont));
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Visit Date", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Reservation Date", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Route Name", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Total Members", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Shift", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Guide Name", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Ticket Amount", subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase("Mode Of Booking", subheadfont));
            tabular.AddCell(cells);




            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["DateOfVisit"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ReservatindDate"]), subheadfont));
            tabular.AddCell(cells);



            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ZoneName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows.Count), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ShiftTime"]), subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleNumber"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["GuidName"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["AmountTobePaid"]), subheadfont));
            tabular.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ModeOfBooking"]), subheadfont));
            tabular.AddCell(cells);


            doc.Add(tabular);
            doc.Add(table);

            ///End Logo 

            ///////

            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable VisitorDetails = new PdfPTable(5) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cellsv = new PdfPCell();// { Border = 4};
            VisitorDetails.TotalWidth = 180;

            VisitorDetails.SetTotalWidth(new float[] { 15f, 35f, 20f, 30f, 20f });


            cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Nationality", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);
            cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
            cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            VisitorDetails.AddCell(cellsv);


            Int16 j = 0;
            foreach (DataRow DR in DT1.Rows)
            {
                j += Convert.ToInt16(1);

                cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NameOfVisitor"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["Nationality"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["IdproofIdDetails"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase(DR["NoOfCamera"].ToString(), subheadfont));
                VisitorDetails.AddCell(cellsv);
            }
            doc.Add(VisitorDetails);






            //doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };


            PdfPCell cellsd;

            cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
            SafariDetails.AddCell(cellsd);

            doc.Add(SafariDetails);

            //=================================================Extra Revised AMount ============================================

            SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            cellsd = new PdfPCell(new Phrase("Remaining Payable Amount :", subheadfont));
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
            SafariDetails.AddCell(cellsd);

            cellsd = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ExtraAmountRevised"]), subheadfont));
            SafariDetails.AddCell(cellsd);

            doc.Add(SafariDetails);



            //==========================================================================================================================
            PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell celltermcondition = new PdfPCell();// { Border = 4};
            TermsCondition.TotalWidth = 180;
            TermsCondition.SetTotalWidth(new float[] { 30f, 400f });


            celltermcondition = new PdfPCell(new Phrase("Terms and conditions for Visitors", tableHeaderFont));
            celltermcondition.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            celltermcondition = new PdfPCell(new Phrase("The visitor must reach the boarding point at least 15 minutes prior to the departure time.", subheadfont));
            TermsCondition.TotalWidth = 130;
            TermsCondition.SetTotalWidth(new float[] { 24f, 100f });
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            celltermcondition = new PdfPCell(new Phrase("Any violation of rules will be punishable under wildlife protection Act 1972.", subheadfont));
            TermsCondition.TotalWidth = 130;
            TermsCondition.SetTotalWidth(new float[] { 24f, 100f });
            celltermcondition.Colspan = 2;
            TermsCondition.AddCell(celltermcondition);

            doc.Add(TermsCondition);

            //PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            //PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
            //cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //TermsCondition.TotalWidth = 430;
            //TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
            //cellst.Colspan = 2;
            //cellst.HorizontalAlignment = Element.ALIGN_LEFT;
            //TermsCondition.AddCell(cellst);

            //doc.Add(TermsCondition);



            PdfPTable DOsAndDont = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };

            PdfPCell DOsAndDontcells = new PdfPCell();// { Border = 4};
            DOsAndDont.TotalWidth = 180;
            DOsAndDont.SetTotalWidth(new float[] { 30f, 400f });


            DOsAndDontcells = new PdfPCell(new Phrase("Abide by the rules of the :" + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
            DOsAndDontcells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);


            DOsAndDontcells = new PdfPCell(new Phrase("DO's :", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("1). Enter the park with a valid ticket.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("2). Take official guide with you inside the park.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("3). maximum speed limit is 20 km/hr.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("4). Always carry drinking water.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("5). Maintain silence and discipline during excursions.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("6). Allow the animals to have the right of way.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("7). Wear colors which match with Nature.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("8). Please carry maps/Guide Book for your reference.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);


            DOsAndDontcells = new PdfPCell(new Phrase("DONT'S :", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            DOsAndDontcells = new PdfPCell(new Phrase("1). don't get down, unless told by the guide.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("2). Don't carry arms,explosives or intoxicants inside the park.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("3). Don't blow horn.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("4). Don't litter with cans,bottles, plastic bags etc.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("5). Don't try to feed the animals.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("6). Don't smoke or lit fire.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("7). Don't tease or chase the animals.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);
            DOsAndDontcells = new PdfPCell(new Phrase("8). Don't leave plastic/Polybags.", subheadfont));
            DOsAndDontcells.Colspan = 2;
            DOsAndDont.AddCell(DOsAndDontcells);

            doc.Add(DOsAndDont);

            #region ADD QR CODE
            ///string encData = FMDSS.Models.EncodingDecoding.Encrypt(Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18), "E-m!tr@2016");
            string encData = FMDSS.Models.MySecurity.SecurityCode.Encode(Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18));

            //string decVal = FMDSS.Models.EncodingDecoding.Decrypt(encData, "E-m!tr@2016");

            string QRCodePath = Utility.GenerateMyQCCode(encData, Convert.ToString(DT1.Rows[0]["BoardingID"]).Substring(0, 18), "QRCodeReader/OnlineBoardingPassQRCode");
            if (!string.IsNullOrEmpty(QRCodePath))
            {
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));
                doc.Add(new Paragraph(Environment.NewLine));

                PdfPTable table1 = new PdfPTable(3);

                ////Add Logo

                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);

                doc.Add(tableheading);



                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(" ", MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                cellHeading = new PdfPCell(tableheading);
                cellHeading.BorderWidth = 0;
                cellHeading.Padding = 0;
                cellHeading.PaddingBottom = -20;

                table1.AddCell(cellHeading);



                ////Add Heading
                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);

                doc.Add(tableheading);



                tableheading = new Paragraph("", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(" ", MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                cellHeading = new PdfPCell(tableheading);
                cellHeading.BorderWidth = 0;
                cellHeading.Padding = 0;
                cellHeading.PaddingBottom = -20;

                table1.AddCell(cellHeading);



                EmitraImageLogo = iTextSharp.text.Image.GetInstance(QRCodePath);
                EmitraImageLogo.ScaleToFit(130f, 170f);
                EmitraImageLogo.SpacingBefore = -70f;
                EmitraImageLogo.SpacingAfter = -70f;
                EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



                cellEmitraLogo = new PdfPCell(EmitraImageLogo);
                cellEmitraLogo.BorderWidth = 0;
                cellEmitraLogo.Padding = 50;
                cellEmitraLogo.PaddingBottom = -95;

                table1.AddCell(cellEmitraLogo);
                doc.Add(new Paragraph(Environment.NewLine));

                doc.Add(table1);

            }
            #endregion

            //PdfPCell DOsAndDontcells = new PdfPCell(new Phrase("Abide by the rules of the :" + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));// { Border = 4};
            //DOsAndDontcells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            //DOsAndDont.TotalWidth = 430;
            //DOsAndDont.SetTotalWidth(new float[] { 30f, 400f });
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDontcells.HorizontalAlignment = Element.ALIGN_LEFT;
            //DOsAndDont.AddCell(DOsAndDontcells);

            //string doss = "DO's : - 1). Enter the park with a valid ticket. 2). Take official guide with you inside the park. 3) maximum speed limit is 20 km/hr.  4). Always carry drinking water. 5). Maintain silence and discipline during excursions. 6). Allow the animals to have the right of way. 7). Wear colors which match with Nature. 8). Please carry maps/Guide Book for your reference 9). If driver and Guide violate then report to Department at the contacts mentioned on this Boarding Pass. ";
            //DOsAndDontcells = new PdfPCell(new Phrase(doss, subheadfont));
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDont.AddCell(cellst);

            //string dontss = "DONT'S : - 1). don't get down, unless told by the guide. 2). Don't carry arms,explosives or intoxicants inside the park. 3). Don't blow horn. 4). Don't litter with cans,bottles, plastic bags etc. 5). Don't try to feed the animals. 6). Don't smoke or lit fire. 7). Don't tease or chase the animals. 8). Don't leave plastic/Polybags.";
            //DOsAndDontcells = new PdfPCell(new Phrase(dontss, subheadfont));
            //DOsAndDontcells.Colspan = 2;
            //DOsAndDont.AddCell(cellst);

            doc.Close();

            return filepath;

        }

        public static string WildlifeBoardingPassListDownloadPdf(CS_BoardingDetails BoardingDetails, List<CS_BoardingDetails> ListBoarding)
        {

            StringBuilder sb = new StringBuilder();
            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
            string filepath = string.Empty;
            try
            {


                filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/BoardingVisitorList_" + Convert.ToString(ListBoarding[0].DisplayBookingId.Substring(0, 18)) + ".pdf");



                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                Paragraph tableheading = null;
                Paragraph sideheading = null;
                Phrase colHeading;

                PdfPCell cell;
                PdfPTable pdfTable = null;
                var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
                var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
                doc.Open();
                doc.NewPage();

                // doc.Add(new Paragraph(Environment.NewLine));

                /////create Table

                PdfPTable table;

                table = new PdfPTable(3);

                ////Add Logo

                string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
                string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");

                string pdfFile = filepath;

                iTextSharp.text.Image RislImageLogo = iTextSharp.text.Image.GetInstance(imageURL);
                RislImageLogo.ScaleToFit(60f, 60f);
                RislImageLogo.SpacingBefore = -70f;
                RislImageLogo.SpacingAfter = -70f;
                RislImageLogo.Alignment = Element.ALIGN_RIGHT;





                PdfPCell cellRislLogo;
                cellRislLogo = new PdfPCell(RislImageLogo);
                cellRislLogo.BorderWidth = 0;
                cellRislLogo.Padding = -30;
                cellRislLogo.PaddingTop = -80;
                cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cellRislLogo);



                ////Add Heading
                tableheading = new Paragraph("Goverment of", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("Rajasthan.", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph("Department of Forest,", MyFont);
                tableheading.Font.Size = 12;
                tableheading.Alignment = (Element.ALIGN_CENTER);

                doc.Add(tableheading);

               

                tableheading = new Paragraph(" ", MyFont);
                tableheading.Font.Size = 10;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                PdfPCell cellHeading;
                cellHeading = new PdfPCell(tableheading);
                cellHeading.BorderWidth = 0;
                cellHeading.Padding = 0;
                cellHeading.PaddingTop = -20;

                table.AddCell(cellHeading);



                iTextSharp.text.Image EmitraImageLogo = iTextSharp.text.Image.GetInstance(eMitraLogo);
                EmitraImageLogo.ScaleToFit(130f, 170f);
                EmitraImageLogo.SpacingBefore = -70f;
                EmitraImageLogo.SpacingAfter = -70f;
                EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



                PdfPCell cellEmitraLogo;
                cellEmitraLogo = new PdfPCell(EmitraImageLogo);
                cellEmitraLogo.BorderWidth = 0;
                cellEmitraLogo.Padding = 50;
                cellEmitraLogo.PaddingTop = -75;

                table.AddCell(cellEmitraLogo);
                doc.Add(new Paragraph(Environment.NewLine));

                //PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                //PdfContentByte cb = writer.DirectContent;
                // cb.Stroke();


                PdfPTable tabular = new PdfPTable(7) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cells = new PdfPCell(new Phrase("Boarding List", subheadfont));// { Border = 4};
                tabular.TotalWidth = 320;
                tabular.SetTotalWidth(new float[] { 30f, 30f, 30f, 30f, 30f, 30f, 30f });
                cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                cells.Colspan = 7;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                tabular.AddCell(cells);


                cells = new PdfPCell(new Phrase("Boarding List : " + Convert.ToString(BoardingDetails.PlaceName), subheadfont));
                cells.Colspan = 7;
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                tabular.AddCell(cells);


                cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Zone", subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Shift", subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Vehicle", subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Vehicle Number", subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Number Of Visitors", subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase("Guide Name", subheadfont));
                tabular.AddCell(cells);


                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.DateofVisit), subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.ZoneAtTheTimeOfBooking), subheadfont));
                tabular.AddCell(cells);



                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.Shift), subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.Vehicle), subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.VehicleNumber), subheadfont));
                tabular.AddCell(cells);
                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.VisitorCount), subheadfont));
                tabular.AddCell(cells);

                cells = new PdfPCell(new Phrase(Convert.ToString(BoardingDetails.GuidName), subheadfont));
                tabular.AddCell(cells);


                doc.Add(tabular);
                doc.Add(table);

                ///End Logo 

                ///////

                //doc.Add(new Paragraph(Environment.NewLine));


                PdfPTable VisitorDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 180;
                VisitorDetails.SetTotalWidth(new float[] { 10f, 40f, 30f, 40f, 20f, 35f });


                cellsv = new PdfPCell(new Phrase("SNO", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Booking Id", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Visitor Name ", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Id proof & Id details ", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Verify Visitors (Yes/No)", tableHeaderFont)); //
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);


                foreach (var Item in ListBoarding)
                {


                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Item.Index), subheadfont));
                    cellsv.MinimumHeight = 20f;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Item.DisplayBookingId), subheadfont));
                    cellsv.MinimumHeight = 20f;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Item.NameOfVisitor), subheadfont));
                    cellsv.MinimumHeight = 20f;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Item.IdproofIdDetails), subheadfont));
                    cellsv.MinimumHeight = 20f;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Item.Camera), subheadfont));
                    cellsv.MinimumHeight = 20f;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("", subheadfont));
                    cellsv.MinimumHeight = 20f;
                    VisitorDetails.AddCell(cellsv);


                }
                doc.Add(VisitorDetails);






                //doc.Add(new Paragraph(Environment.NewLine));


                PdfPTable SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };


                PdfPCell cellsd;

                cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
                SafariDetails.TotalWidth = 130;
                cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
                SafariDetails.AddCell(cellsd);

                cellsd = new PdfPCell(new Phrase(BoardingDetails.BoardingPointName, subheadfont));
                cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                SafariDetails.AddCell(cellsd);
                doc.Add(SafariDetails);


                doc.Close();
            }
            catch (Exception)
            {
                doc.Close();
                throw;
            }

            return filepath;

        }

        public static void ZooDownloadPdf(DataSet ds)
        {

            try
            {

                StringBuilder sb = new StringBuilder();
                DataTable DT1 = new DataTable();
                DataTable DT2 = new DataTable();
                DataTable DT3 = new DataTable();

                //DataSet ds = new DataSet();
                //BookOnZoo cs = new BookOnZoo();
                //cs.TicketID = 100;

                //ds = cs.Select_TicketData_For_Print();

                DT1 = ds.Tables[0];
                DT2 = ds.Tables[1];
                DT3 = ds.Tables[2];
                ///////////////

             

                string filepath = string.Empty;

                filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/") + Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]) + ".pdf";
                Document doc = new Document(PageSize.A8, 15, 15f, 15f, 15f);



                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                Paragraph tableheading = null;
                Paragraph sideheading = null;
                Phrase colHeading;

                PdfPCell cell;
                PdfPTable pdfTable = null;

                var subheadfont = FontFactory.GetFont("Arial", 4, FontColour);

                doc.Open();
                doc.NewPage();

                // doc.Add(new Paragraph(Environment.NewLine));

                /////create Table
                PdfPTable tablehead;
                tablehead = new PdfPTable(3);

              


                string[] splitHeader = new string[] { "<br/>" };

                string[] HeaderText = ds.Tables[0].Rows[0]["HeadeText"].ToString().Split(splitHeader, StringSplitOptions.RemoveEmptyEntries);


                tableheading = new Paragraph(Convert.ToString(HeaderText[0]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(Convert.ToString(HeaderText[1]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(Convert.ToString(HeaderText[2]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(Convert.ToString(HeaderText[3]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                //tableheading = new Paragraph(Convert.ToString(HeaderText[4]), subheadfont);
                //tableheading.Font.Size = 4;
                //tableheading.Alignment = (Element.ALIGN_CENTER);
                //doc.Add(tableheading);

                tableheading = new Paragraph(" ", subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);



                doc.Add(tablehead);




                //doc.Add(new Paragraph(Environment.NewLine));            

                PdfPTable VisitorDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsv = new PdfPCell() { Border = 2 };
                VisitorDetails.TotalWidth = 140;
                VisitorDetails.SetTotalWidth(new float[] { 32f, 10f, 15f, 33f });

                cellsv = new PdfPCell(new Phrase("Ticket No:" + ds.Tables[0].Rows[0]["DateOfVisit"].ToString(), subheadfont)) { Border = 2 };
                cellsv.Colspan = 2;
                cellsv.HorizontalAlignment = Element.ALIGN_LEFT;
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Date:" + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "Time:" + ds.Tables[0].Rows[0]["VisitTime"].ToString(), subheadfont)) { Border = 2 };
                cellsv.Colspan = 2;
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("Rate(INR)", subheadfont)) { Border = 2 };
                cellsv.Colspan = 2;
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("Qty", subheadfont)) { Border = 2 };
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Amount(INR)", subheadfont)) { Border = 2 };
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);

                for (int i = 0; i < DT2.Rows.Count; i++)
                {
                    int j = i + 1;
                    sb.Append("<tr><td>" + DT2.Rows[i]["TypeOfMember"].ToString() + "</td><td>" + DT2.Rows[i]["FeePerMember"].ToString() + "</td><td>" + DT2.Rows[i]["NoOfMember"].ToString() + "</td><td>" + DT2.Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["TypeOfMember"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_LEFT;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["FeePerMember"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["NoOfMember"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["TotalMemberFees"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    VisitorDetails.AddCell(cellsv);
                }
                cellsv = new PdfPCell(new Phrase("Grand Total: " + ds.Tables[5].Rows[0][0].ToString(), subheadfont)) { Border = 1 };
                cellsv.Colspan = 4;
                cellsv.HorizontalAlignment = Element.ALIGN_LEFT;
                VisitorDetails.AddCell(cellsv);
                doc.Add(VisitorDetails);




                PdfPTable tablefoot;

                tablefoot = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_CENTER };

                string[] split = new string[] { "<br/>" };

                string[] FooterText = ds.Tables[0].Rows[0]["FooterText"].ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);


                ////Add Footer        




                tableheading = new Paragraph(Convert.ToString(FooterText[0]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);

                doc.Add(tableheading);

                tableheading = new Paragraph(Convert.ToString(FooterText[1]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(Convert.ToString(FooterText[2]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                tableheading = new Paragraph(Convert.ToString(FooterText[3]), subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                //tableheading = new Paragraph(Convert.ToString(FooterText[4]), subheadfont);
                //tableheading.Font.Size = 4;
                //tableheading.Alignment = (Element.ALIGN_CENTER);
                //doc.Add(tableheading);

                tableheading = new Paragraph(" ", subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                doc.Add(tablefoot);
                doc.Close();


            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        #region for Zoo Booking WEB API 31-12-2018
        public static void ZooDownloadPdfForEmitraPlus(DataSet ds)
        {
            #region "HTML Ticket"

            try
            {

                StringBuilder sb = new StringBuilder();
                DataTable DT1 = new DataTable();
                DataTable DT2 = new DataTable();
                DataTable DT3 = new DataTable();

                //DataSet ds = new DataSet();
                //BookOnZoo cs = new BookOnZoo();
                //cs.TicketID = 100;

                //ds = cs.Select_TicketData_For_Print();

                DT1 = ds.Tables[0];
                DT2 = ds.Tables[1];
                DT3 = ds.Tables[2];
                ///////////////

                string filepath = string.Empty;

                //filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/") + Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]) + ".pdf";

                //Document doc = new Document(PageSize.A8, 15, 15f, 15f, 15f);

                //PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/" + Convert.ToString(ds.Tables[0].Rows[0]["RequestId"]) + "_ForEmitraPlus.pdf");

                Document doc = new Document(PageSize.A5, 25, 25f, 25f, 25f);

                //PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
                PdfWriter docWriter = PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));



                var FontColour = new BaseColor(0, 0, 0);
                Paragraph tableheading = null;
                Paragraph sideheading = null;
                Phrase colHeading;

                PdfPCell cell;
                PdfPTable pdfTable = null;

                var subheadfont = FontFactory.GetFont("Arial", 4, FontColour);

                doc.Open();
                doc.NewPage();

                // doc.Add(new Paragraph(Environment.NewLine));


                /////create Table
                PdfPTable tablehead;
                tablehead = new PdfPTable(3);



                string[] splitHeader = new string[] { "<br/>" };

                string[] HeaderText = ds.Tables[0].Rows[0]["HeadeText"].ToString().Split(splitHeader, StringSplitOptions.RemoveEmptyEntries);

                ////Add Logo
                for (int i = 0; i < HeaderText.Count(); i++)
                {
                    tableheading = new Paragraph(Convert.ToString(HeaderText[i]), subheadfont);
                    tableheading.Font.Size = 4;
                    tableheading.Alignment = (Element.ALIGN_CENTER);
                    doc.Add(tableheading);
                }
                 

                tableheading = new Paragraph(" ", subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                doc.Add(tablehead);




                //doc.Add(new Paragraph(Environment.NewLine));            

                PdfPTable VisitorDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsv = new PdfPCell() { Border = 2 };
                VisitorDetails.TotalWidth = 140;
                VisitorDetails.SetTotalWidth(new float[] { 32f, 10f, 15f, 33f });

                cellsv = new PdfPCell(new Phrase("Ticket No:" + ds.Tables[0].Rows[0]["RequestId"].ToString(), subheadfont)) { Border = 2 };
                cellsv.Colspan = 2;
                cellsv.HorizontalAlignment = Element.ALIGN_LEFT;
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Date:" + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "Time:" + ds.Tables[0].Rows[0]["VisitTime"].ToString(), subheadfont)) { Border = 2 };
                cellsv.Colspan = 2;
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("Rate(INR)", subheadfont)) { Border = 2 };
                cellsv.Colspan = 2;
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);

                cellsv = new PdfPCell(new Phrase("Qty", subheadfont)) { Border = 2 };
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Amount(INR)", subheadfont)) { Border = 2 };
                cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                VisitorDetails.AddCell(cellsv);

                for (int i = 0; i < DT2.Rows.Count; i++)
                {
                    int j = i + 1;
                    sb.Append("<tr><td>" + DT2.Rows[i]["TypeOfMember"].ToString() + "</td><td>" + DT2.Rows[i]["FeePerMember"].ToString() + "</td><td>" + DT2.Rows[i]["NoOfMember"].ToString() + "</td><td>" + DT2.Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["TypeOfMember"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_LEFT;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["FeePerMember"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["NoOfMember"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase(DT2.Rows[i]["TotalMemberFees"].ToString(), subheadfont)) { Border = 1 };
                    cellsv.HorizontalAlignment = Element.ALIGN_RIGHT;
                    VisitorDetails.AddCell(cellsv);
                }
                cellsv = new PdfPCell(new Phrase("Grand Total: " + ds.Tables[5].Rows[0][1].ToString(), subheadfont)) { Border = 1 };
                cellsv.Colspan = 4;
                cellsv.HorizontalAlignment = Element.ALIGN_LEFT;
                VisitorDetails.AddCell(cellsv);
                doc.Add(VisitorDetails);




                PdfPTable tablefoot;

                tablefoot = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_CENTER };

                string[] split = new string[] { "<br/>" };

                string[] FooterText = ds.Tables[0].Rows[0]["FooterText"].ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);


                ////Add Footer        


                for (int i = 0; i < FooterText.Count(); i++)
                {

                    tableheading = new Paragraph(Convert.ToString(FooterText[i]), subheadfont);
                    tableheading.Font.Size = 4;
                    tableheading.Alignment = (Element.ALIGN_CENTER);

                    doc.Add(tableheading);
                }
                 

                tableheading = new Paragraph(" ", subheadfont);
                tableheading.Font.Size = 4;
                tableheading.Alignment = (Element.ALIGN_CENTER);
                doc.Add(tableheading);

                doc.Add(tablefoot);
                doc.Close();

            #endregion

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion


        public static string Latter_1(string[] ArrAttachments, Tbl_Encroachment tblEnch, Tbl_Encroacher_Details tblEncroacher_Details, Tbl_Encroach_InvestigationDetails tblInvest, tbl_UserProfiles tblUserProfile, tbl_mst_Forest_Divisions tbldiv)
        {


            #region Letter 1

            string filepath = string.Empty;

            filepath = System.Web.HttpContext.Current.Server.MapPath("~/PDFFolder/Encroachment/Letter_1_" + tblEnch.EN_Code + ".pdf");

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);
            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;


            string fontpath = "C:\\Windows\\Fonts\\DevLys.ttf";
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 14);
            iTextSharp.text.Font hindiHead = new iTextSharp.text.Font(dev, 12);
            iTextSharp.text.Font hindiTitle = new iTextSharp.text.Font(dev, 10);
            iTextSharp.text.Font hindiSubFont = new iTextSharp.text.Font(dev, 8);

            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);


            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 35;
            Details.SetTotalWidth(new float[] { 35f, 0f, 0f });

            cells = new PdfPCell(new Phrase("izi= la- & 1", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("jktLFkku ljdkj", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("ou foHkkx", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            Phrase phrase = new Phrase();
            phrase.Add(new Chunk("vksj ls     ", hindi));
            phrase.Add(new Chunk(tblUserProfile.Name, fontTitle));//Aur Se
            Details.AddCell(phrase);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("          {ks=h; ou vf/kdkjh", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("                    " + tbldiv.DIV_NAME, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);


            Phrase phrase1 = new Phrase();
            phrase1.Add(new Chunk("fufer    ", hindi));
            phrase1.Add(new Chunk(" U;k;ky; lgk;d ou laj{kd]", hindi));//Nimit
            Details.AddCell(phrase1);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);


            Phrase phrase2 = new Phrase();
            phrase2.Add(new Chunk("Øekad@", hindi));
            phrase2.Add(new Chunk(tblEnch.EN_Code, fontTitle));//Kramank
            Details.AddCell(phrase2);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("          fo\"k; % & jktLFkku Hkw- jktLo vf/kfu;e 1956 dh /kkjk 91 ds vUrxZr ou foHkkx Hkwfe ij vukf/kd`r dCts dks csn[ky djus ds laca/k esaA", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("egksn;]", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            Phrase phrase3 = new Phrase();
            phrase3.Add(new Chunk("	        mijksDr fo\"k;kUrxZr fuosnu gS fd Jh ", hindi));
            phrase3.Add(new Chunk(tblEncroacher_Details.Encroacher_Name, fontTitle));//Name
            // phrase3.Add(new Chunk(" iq= Jh ", hindi));
            //  phrase3.Add(new Chunk(" FName ", fontTitle));//FName
            //   phrase3.Add(new Chunk(" tkfr", hindi));
            //   phrase3.Add(new Chunk(" Jati ", fontTitle));//Jati
            phrase3.Add(new Chunk(" fuoklh ", hindi));
            phrase3.Add(new Chunk(tblEncroacher_Details.Encroacher_Address, fontTitle));//Niwasi
            //phrase3.Add(new Chunk(" rglhy", hindi));
            //phrase3.Add(new Chunk(" Tehasil ", fontTitle));//Tehasil
            //phrase3.Add(new Chunk(" ftyk", hindi));
            //phrase3.Add(new Chunk(" Jila ", fontTitle));//Jila
            phrase3.Add(new Chunk(" us ou Hkqfe xzke ds [kljk uEcj  ", hindi));
            phrase3.Add(new Chunk(tblInvest.khasraNo, fontTitle));//Khasra Number
            phrase3.Add(new Chunk(" vkjf{kr@jf{kr ou[k.M ", hindi));
            phrase3.Add(new Chunk(tbldiv.DIV_NAME, fontTitle));//Vankhand 
            phrase3.Add(new Chunk(" dh ", hindi));
            phrase3.Add(new Chunk(Convert.ToString(tblInvest.Encroachment_Area), fontTitle));//Area in Bigha
            phrase3.Add(new Chunk(" gSDV0 ij vukf/kd`r dCtk dj j[kk gSA ;g ou[k.M jkT; ljdkj dh foKfIr la[;k ", hindi));
            phrase3.Add(new Chunk(tblInvest.NotificationNo, fontTitle));//Vigypti Sankhya
            phrase3.Add(new Chunk(" fnukad ", hindi));
            phrase3.Add(new Chunk(tblInvest.NotificationDate.ToString("dd/MM/yyyy"), fontTitle));//Dinank
            phrase3.Add(new Chunk(" ds }kjk ", hindi));
            phrase3.Add(new Chunk(tblInvest.TypeofLand, fontTitle));
            phrase3.Add(new Chunk(" ?kksf\"kr gks pqdh gSA {ks= dk ekufp= ,oa ou Hkqfe ij vukf/kd`r dCts dk fooj.k layXu gSA", hindi));
            Details.AddCell(phrase3);
         
            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            Phrase phrase4 = new Phrase();
            phrase4.Add(new Chunk("	        vr% Jh ", hindi));
            phrase4.Add(new Chunk(tblEncroacher_Details.Encroacher_Name, fontTitle));//Name
            phrase4.Add(new Chunk(" dks jktLFkku Hkw- jktLo vf/kfu;e dh /kkjk 91 ds vUrxZr vukf/kd`r dCts ls csn[ky djus dh dk;Zokgh djus dk d\"V djsaA", hindi));
            Details.AddCell(phrase4);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("layXu % &", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            // foreach (var items in ArrAttachments)
            //{
            //    cells = new PdfPCell(new Phrase(items., Myfont)) { Border = 0 };
            //    cells.Colspan = 3;
            //    cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Details.AddCell(cells);
            //}
            int j = 1;
            for (int i = 0; i < ArrAttachments.Length; i++)
            {
                if (ArrAttachments[i] == string.Empty )
                {
                }
                else
                {
                    cells = new PdfPCell(new Phrase(j.ToString()+" . " + ArrAttachments[i], Myfont)) { Border = 0 };
                    cells.Colspan = 3;
                    cells.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cells);
                    j = j +1;
                }
            }




            cells = new PdfPCell(new Phrase("Hkonh;", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(tblUserProfile.Name, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("{ks=h; ou vf/kdkjh", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(tbldiv.DIV_NAME, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);




            doc.Add(Details);



            doc.Close();

            #endregion
            return filepath;


        }

        public static string Latter_2(Tbl_Encroach_Appearance tblEnch, tbl_UserProfiles tblUserProfile, IEnumerable<Tbl_Encroacher_Details> tblEncroacher_Details, Tbl_Encroach_InvestigationDetails tblInvest, tbl_mst_Forest_Divisions tbldiv)
        {
            #region Letter 2

            string filepath = string.Empty;
            filepath = tblEnch.Decision_Taken == "Nextdate" ? tblEnch.EN_Code + tblEnch.Decision_Taken + "_" + tblEnch.Next_Date.ToString().Substring(0, 10).Replace('/', '-') : tblEnch.EN_Code + tblEnch.Decision_Taken + "_" + DateTime.Now.ToString("dd-MM-yyyy");
            filepath = HttpContext.Current.Server.MapPath("~/PDFFolder/Encroachment/Letter_2_" + filepath + ".pdf");

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);
            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));


            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            string fontpath = "C:\\Windows\\Fonts\\DevLys.ttf";
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 14);
            iTextSharp.text.Font hindiHead = new iTextSharp.text.Font(dev, 12);
            iTextSharp.text.Font hindiTitle = new iTextSharp.text.Font(dev, 10);
            iTextSharp.text.Font hindiSubFont = new iTextSharp.text.Font(dev, 8);

            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);


            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            //PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            //Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //PdfPCell cells = new PdfPCell() { Border = 4 };
            //Details.TotalWidth = 120;
            //Details.SetTotalWidth(new float[] { 35f, 50f, 35f });

            //cells = new PdfPCell(new Phrase("U;k;ky; lgk;d ou laj{kd ", hindi)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase(tbldiv.DIV_NAME, fontTitle)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("Øekad % &", hindi)) { Border = 0 };

            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            // cells = new PdfPCell(new Phrase(tblEnch.EN_Code, fontTitle)) { Border = 0 };

            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);


            //cells = new PdfPCell(new Phrase("fnukad % &", hindi)) { Border = 0 };
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("fufeRr % &", hindi)) { Border = 0 };
            // cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);


            //cells = new PdfPCell(new Phrase("               Jh", hindi)) { Border = 0 };

            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            // cells = new PdfPCell(new Phrase("               "+tblEncroacher_Details.FirstOrDefault().Encroacher_Name, fontTitle)) { Border = 0 };

            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("               "+tblEncroacher_Details.FirstOrDefault().Encroacher_Address, fontTitle)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            ////cells = new PdfPCell(new Phrase("               ---------------------------------------------------------------------", hindi)) { Border = 0 };
            ////cells.Colspan = 3;
            ////cells.HorizontalAlignment = Element.ALIGN_LEFT;
            ////Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
            //Details.AddCell(cells);
            DateTime Decision_Date = (DateTime)tblEnch.Decision_Date;

            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 35f;
            Details.SetTotalWidth(new float[] { 35f, 0f, 0f });



            cells = new PdfPCell(new Phrase("U;k;ky; lgk;d ou laj{kd ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);


            cells = new PdfPCell(new Phrase(tbldiv.DIV_NAME, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);



            cells = new PdfPCell(new Phrase("fnukad", hindi)) { Border = 0 };//Date
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Decision_Date.ToString("dd/MM/yyyy"), fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            Phrase phraseL = new Phrase();
            phraseL.Add(new Chunk(" Øekad % & ", hindi));
            phraseL.Add(new Chunk(tblEnch.EN_Code, fontTitle));//Kramank
            Details.AddCell(phraseL);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            Phrase phraseL2 = new Phrase();
            phraseL2.Add(new Chunk("fufeRr % &", hindi));
            //phraseL2.Add(new Chunk(" nimit ", fontTitle));//Nimit
            Details.AddCell(phraseL2);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);


            Phrase phraseL3 = new Phrase();
            phraseL3.Add(new Chunk("                     Jh ", hindi));
            phraseL3.Add(new Chunk(tblEncroacher_Details.FirstOrDefault().Encroacher_Name + " ", fontTitle));//Name
            phraseL3.Add(new Chunk(tblEncroacher_Details.FirstOrDefault().Encroacher_Address, fontTitle));//Name
            Details.AddCell(phraseL3);

            //cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("                      ---------------------------------------------------------------------", fontTitle)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("                      ---------------------------------------------------------------------", fontTitle)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);


            string Banam = string.Join(", ", tblEncroacher_Details.ToList().Select(d => d.Encroacher_Name));

            Phrase phraseL4 = new Phrase();
            phraseL4.Add(new Chunk("        fo\"k; % & vfrØe.k dsl la0 ", hindi));
            phraseL4.Add(new Chunk(tblEnch.EN_Code, fontTitle));//Case Number
            phraseL4.Add(new Chunk(" ljdkj ¼ou foHkkx½  cuke ", hindi));
            phraseL4.Add(new Chunk(Banam, fontTitle));//Banam
            Details.AddCell(phraseL4);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            DateTime DNext_Date = (DateTime)tblEnch.Next_Date;


            Phrase phraseL5 = new Phrase();
            phraseL5.Add(new Chunk("        fo\"k;kUrxZr izdj.k esa vkxkeh rkjh[k is’kh ", hindi));
            phraseL5.Add(new Chunk(DNext_Date.ToString("dd/MM/yyyy"), fontTitle));//Date
            phraseL5.Add(new Chunk(" eqdke dSEi dksV ", hindi));
            phraseL5.Add(new Chunk(tblEnch.Next_Decision_Place, fontTitle));//Court
            phraseL5.Add(new Chunk(" okLrs ", hindi));
            phraseL5.Add(new Chunk(tblEnch.Decision_Description, fontTitle));//Vaste
            phraseL5.Add(new Chunk(" fuf’pr dh xbZ gSA vr% vki bl fnukad dks mifLFkr gksosaA vkidh mifLFkfr ugh gksus dh lwjr esa bdrjQk dk;Zokgh djh nh tkosxhA izdj.k esa vius i{k esa izLrqr fd;s tkus okys nLrkost lfgr fu;r fnukad dks mifLFkr gksosaA vkt fnukad ", hindi));
            phraseL5.Add(new Chunk(Decision_Date.ToString("dd/MM/yyyy"), fontTitle));//current Date
            phraseL5.Add(new Chunk(" dks esjs gLrk{kj ,oa U;k;ky; dh eqnzk ls tkjh fd;k x;kA", hindi));
            Details.AddCell(phraseL5);



            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(tblUserProfile.Name, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("lgk;d ou laj{kd", hindi)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(tbldiv.DIV_NAME, fontTitle)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("Signature", fontTitle)) { Border = 0 };
            //cells.Colspan = 3;
            //cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            //Details.AddCell(cells);



            doc.Add(Details);


            doc.Close();

            return filepath;
            #endregion

        }


        public static string Praptra_2(List<MISEncroachmentDetails> Praptra2Details)
        {
            #region Praptra_2

            string filepath = string.Empty;

            filepath = System.Web.HttpContext.Current.Server.MapPath("~/PDFFolder/Encroachment/Praptra_2_" + Praptra2Details.FirstOrDefault().EN_Code + ".pdf");

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            string fontpath = "C:\\Windows\\Fonts\\DevLys.ttf";
            BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 14);
            iTextSharp.text.Font hindiHead = new iTextSharp.text.Font(dev, 12);
            iTextSharp.text.Font hindiTitle = new iTextSharp.text.Font(dev, 10);
            iTextSharp.text.Font hindiSubFont = new iTextSharp.text.Font(dev, 8);

            var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
            var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);

            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            PdfPTable Details = new PdfPTable(10) { WidthPercentage = 50, HorizontalAlignment = Element.ALIGN_RIGHT };
            PdfPCell cells = new PdfPCell() { Border = 4 };
            //Details.TotalWidth = 120;
            Details.SetTotalWidth(new float[] { 5f, 5f, 5f, 35f, 5f, 5f, 3f, 5f, 5f, 5f });


            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().DIV_NAME, fontTitle)) { Border = 0 }; // 
            
            cells.Rotation = 270;
            cells.Rowspan = 10;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("{ks=h; ou vf/kdkjh", hindi)) { Border = 0 };// shetriya van adhkari
            cells.Rotation = 270;
            cells.Rowspan = 10;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("izef.kr fd;k tkrk gS fd yxku dh nj bl {ks= ds iVokjh ls izkIr dj vafdr dh xbZ gSA", hindi)) { Border = 0 };//Line 3
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            cells.Rowspan = 10;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().NameAddress, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("  ,oa ewy fuokl LFkku  ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("O;fDr dk uke firk dk uke tkfr ", hindi));// { Border = 0 };//vakti ka naam pita ka naam
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("", fontTitle)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            cells.Rowspan = 10;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("ou Hkwfe ij vukf/kd`r dCts dk fooj.k", hindi)) { Border = 0 };// van bhumi pr kabja jmane ka vivran
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rowspan = 10;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("jktLFkku ljdkj ou foHkkx", hindi)) { Border = 0 };// rajasthan sarkar van vibahg
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rowspan = 10;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("izi= & 2", hindi)) { Border = 0 };// praptra -2
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rowspan = 10;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().Year, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase( System.Configuration.ConfigurationManager.AppSettings["samvatvarsh"].ToString(), hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().khasraNo, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);


            cells = new PdfPCell(new Phrase(" [kljk u- ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().EncrochedArea, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" dqy {ks= ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().PaidawarOrKISMA, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("  fdLe Hkwfe  ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().TaxPerHact, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Ykxku dh nj izfr ch?kk@gSDV0", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().EncrochedArea, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("  {ks=  ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("vukf/kd`r dCtk", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rowspan = 2;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().EncrochedPaidawar, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("iSnkokj", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().TotalTax, fontTitle));// { Border = 0 };
            
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("  Ykxku  ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Praptra2Details.FirstOrDefault().V_V, fontTitle));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" fo0fo0 ", hindi));// { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            cells.Rotation = 270;
            Details.AddCell(cells);

            doc.Add(Details);

            doc.Close();
            return filepath;
            #endregion
        }
    }
}