using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace FMDSS.LIBS
{
    public static class TicketPDFGenerate
    {
        public static string NP_GenerateTicket(DataSet DS)
        {
            if (DS.Tables.Count > 2)
            {
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    DataTable DT1 = DS.Tables[0];
                    DataTable DT2 = DS.Tables[1];
                    DataTable DT3 = DS.Tables[2];
                    DataTable DT4 = DS.Tables[3];
                    string filepath = string.Empty;

                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_Ticket_" + Convert.ToString(DT1.Rows[0]["RequestId"]) + ".pdf");

                    PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                    var FontColour = new BaseColor(0, 0, 0);
                    Paragraph tableheading = null;

                    var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
                    var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                    var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
                    doc.Open();
                    doc.NewPage();

                    PdfPTable table;

                    table = new PdfPTable(3);

                    string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
                    string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");


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

                    tableheading = new Paragraph("Department of Forest,", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);

                    doc.Add(tableheading);

                    tableheading = new Paragraph("Goverment of", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);
                    doc.Add(tableheading);

                    tableheading = new Paragraph("Rajasthan.", MyFont);
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
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase("Booking No:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Date/Time & Booking:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["BookingDateTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt"), subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["VisitingDate"]).ToString("dd-MMM-yyyy"), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Booked Seats:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TotalMember"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Zone/Track", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ZoneName"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Shift", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Shift"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Vehicle", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Vehicle"]), subheadfont));
                    tabular.AddCell(cells);



                    doc.Add(tabular);

                    doc.Add(table);

                    //Visitor Details.
                    PdfPTable VisitorDetails = new PdfPTable(12) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsv = new PdfPCell();// { Border = 4};
                    VisitorDetails.TotalWidth = 180;
                    VisitorDetails.SetTotalWidth(new float[] { 5f, 20f, 20f, 10f, 10f, 10f, 10f, 10f, 10f, 15f, 18f, 12f });

                    cellsv = new PdfPCell(new Phrase("#", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Still Camera", tableHeaderFont)); //
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Member Fee", tableHeaderFont)); // 
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Vehicle Entry Fee", tableHeaderFont)); //Vehicle
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont)); //Vehicle
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    //cellsv = new PdfPCell(new Phrase("Total Vehicle Fee", tableHeaderFont)); //Vehicle
                    //cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    //VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Camera Fee", tableHeaderFont)); // Camera
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("GST Amount on Vehicle Rent and Guide Fee)", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Total Amount (INR)", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);

                    Int16 j = 0;
                    foreach (DataRow DR in DT2.Rows)
                    {
                        j += Convert.ToInt16(1);
                        cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["Name"]) + " - " + Convert.ToString(DR["VisitorType"]), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["IDName"]) + " / " + Convert.ToString(DR["IDNo"]), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["NoOfStillCamera"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["NofVideoCamera"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["MemberFees"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        //cellsv = new PdfPCell(new Phrase(DR["VechileEntryFee"].ToString(), subheadfont));
                        //VisitorDetails.AddCell(cellsv);

                        //cellsv = new PdfPCell(new Phrase(DR["VehicleRentFee"].ToString(), subheadfont));
                        //VisitorDetails.AddCell(cellsv);
                        string vehfeeEntryFee = Convert.ToInt32(DR["VechileEntryFee"]).ToString();
                        cellsv = new PdfPCell(new Phrase(vehfeeEntryFee, subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        string vehicleRentFee = Convert.ToInt32(DR["VehicleRentFee"]).ToString();
                        cellsv = new PdfPCell(new Phrase(vehicleRentFee, subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["GuideFees"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["CameraFees"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["GSTAmount"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["TotalAmount"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                    }
                    doc.Add(VisitorDetails);

                    PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsd;

                    cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);

                    cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["EmitraAmount"].ToString(), subheadfont));
                    cellsd.Colspan = 5;
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);


                    cellsd = new PdfPCell(new Phrase("Grand Total (INR) : ", subheadfont));// { Border = 4};
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    SafariDetails.AddCell(cellsd);


                    cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT1.Rows[0]["TotalAmountBePay"]) + Convert.ToDecimal(DT1.Rows[0]["EmitraAmount"])), subheadfont));
                    cellsd.Colspan = 5;
                    cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);


                    //cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
                    //cellsd.Colspan = 5;
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase("Contact Person :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["contactperson"].ToString(), subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase("PhoneNo :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["PhoneNo"].ToString(), subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase("Address :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Address"].ToString(), subheadfont));
                    //SafariDetails.AddCell(cellsd);


                    doc.Add(SafariDetails);

                    if (DT3.Rows.Count > 0)
                    {
                        PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
                        cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                        TermsCondition.TotalWidth = 430;
                        TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
                        cellst.Colspan = 2;
                        cellst.HorizontalAlignment = Element.ALIGN_LEFT;
                        TermsCondition.AddCell(cellst);

                        int index = 1;
                        for (int i = 0; i < DT3.Rows.Count; i++)
                        {
                            string sTC = Convert.ToString(DT3.Rows[i]["TermAndCondition_Text"]).Replace("#PlaceName#", DT1.Rows[0]["PlaceName"].ToString());
                            cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                            TermsCondition.AddCell(cellst);
                            cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                            TermsCondition.AddCell(cellst);
                            index += 1;
                        }
                        doc.Add(TermsCondition);
                    }
                    if (DT4.Rows.Count > 0)
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
                        foreach (DataRow DR in DT4.Rows)
                        {
                            k += Convert.ToInt16(1);

                            cellt = new PdfPCell(new Phrase(DR["Period"].ToString(), subheadfont));
                            Timing.AddCell(cellt);
                            cellt = new PdfPCell(new Phrase(DR["MorningTrip"].ToString(), subheadfont));
                            Timing.AddCell(cellt);
                            cellt = new PdfPCell(new Phrase(DR["AfterNoonTrip"].ToString(), subheadfont));
                            Timing.AddCell(cellt);
                        }
                        doc.Add(Timing);
                    }
                    return filepath;
                }
                catch (Exception e)
                { }
                finally
                {
                    doc.Close();
                }

            }
            return "";
        }
        public static string NP_GenerateTicket_New(DataSet DS)
        {
            if (DS.Tables.Count > 2)
            {
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    DataTable DT1 = DS.Tables[0];
                    DataTable DT2 = DS.Tables[1];
                    DataTable DT3 = DS.Tables[2];
                    DataTable DT4 = DS.Tables[3];
                    string filepath = string.Empty;
                    decimal OdhiCharge = 0;
                    decimal GrandTotal = 0;
                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_Ticket_" + Convert.ToString(DT1.Rows[0]["RequestId"]) + ".pdf");

                    PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                    var FontColour = new BaseColor(0, 0, 0);
                    Paragraph tableheading = null;

                    var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
                    var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                    var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
                    doc.Open();
                    doc.NewPage();

                    PdfPTable table;

                    table = new PdfPTable(3);

                    string imageURL = System.Web.HttpContext.Current.Server.MapPath("~/images/risl-logo-small.png");
                    string eMitraLogo = System.Web.HttpContext.Current.Server.MapPath("~/images/e-mitra_logo.png");


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

                    tableheading = new Paragraph("Department of Forest,", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);

                    doc.Add(tableheading);

                    tableheading = new Paragraph("Goverment of", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);
                    doc.Add(tableheading);

                    tableheading = new Paragraph("Rajasthan.", MyFont);
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
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase("Booking No:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Date/Time & Booking:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["BookingDateTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt"), subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["VisitingDate"]).ToString("dd-MMM-yyyy"), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Booked Seats:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TotalMember"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Zone/Track", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ZoneName"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Shift", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Shift"]), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Vehicle", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Vehicle"]), subheadfont));
                    tabular.AddCell(cells);



                    doc.Add(tabular);

                    doc.Add(table);

                    //Visitor Details.
                    bool IsOdhiCharge = false;

                    foreach (DataRow DR in DT2.Rows)
                    {
                        if (Convert.ToDecimal(DR["VechileOdhiCharge"].ToString()) > 0)
                        {
                            IsOdhiCharge = true;
                            OdhiCharge = Convert.ToDecimal(DR["VechileOdhiCharge"].ToString()) * 6;
                        }

                    }

                    PdfPTable VisitorDetails = new PdfPTable(14) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsv = new PdfPCell();// { Border = 4};
                    VisitorDetails.TotalWidth = 180;


                    VisitorDetails.SetTotalWidth(new float[] { 5f, 20f, 20f, 10f, 10f, 12f, 12f, 10f, 10f, 10f, 10f, 15f, 18f, 12f });

                    cellsv = new PdfPCell(new Phrase("#", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Still Camera", tableHeaderFont)); //
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Member Entry Fee", tableHeaderFont)); // 
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Member Entry VFPMC Charge", tableHeaderFont)); // 
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Vehicle Entry Fee", tableHeaderFont)); //Vehicle
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Vehicle Entry VFPMC Charge", tableHeaderFont)); //Vehicle
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont)); //Vehicle
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);


                    cellsv = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Camera Fee", tableHeaderFont)); // Camera
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("GST Amount on Vehicle Rent and Guide Fee)", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Total Amount (INR)", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);

                    Int16 j = 0;
                    foreach (DataRow DR in DT2.Rows)
                    {
                        j += Convert.ToInt16(1);
                        cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["Name"]) + " - " + Convert.ToString(DR["VisitorType"]), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["IDName"]) + " / " + Convert.ToString(DR["IDNo"]), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["NoOfStillCamera"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["NofVideoCamera"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["MemberEntryFee"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["MemberVFPMC"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        string vehfeeEntryFee = Convert.ToInt32(DR["VechileEntryFee"]).ToString();
                        cellsv = new PdfPCell(new Phrase(vehfeeEntryFee, subheadfont));
                        VisitorDetails.AddCell(cellsv);


                        cellsv = new PdfPCell(new Phrase(DR["VechileVFPMC"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        string vehicleRentFee = Convert.ToInt32(DR["VehicleRentFee"]).ToString();
                        cellsv = new PdfPCell(new Phrase(vehicleRentFee, subheadfont));
                        VisitorDetails.AddCell(cellsv);


                        cellsv = new PdfPCell(new Phrase(DR["GuideFees"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["CameraFees"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["GSTAmount"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(DR["TotalAmount"].ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        GrandTotal = GrandTotal + Convert.ToDecimal(DR["TotalAmount"].ToString());

                    }
                    doc.Add(VisitorDetails);

                    PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsd;

                    cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);

                    cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["EmitraAmount"].ToString(), subheadfont));
                    cellsd.Colspan = 5;
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);

                    if (IsOdhiCharge == true)
                    {
                        cellsd = new PdfPCell(new Phrase("Odhi Charge (INR):", subheadfont)); //Vehicle						
                        SafariDetails.TotalWidth = 130;
                        SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                        SafariDetails.AddCell(cellsd);

                        cellsd = new PdfPCell(new Phrase(Convert.ToString(OdhiCharge), subheadfont));
                        cellsd.Colspan = 5;
                        cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                        SafariDetails.AddCell(cellsd);
                    }

                    cellsd = new PdfPCell(new Phrase("Grand Total (INR) : ", subheadfont));// { Border = 4};
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    SafariDetails.AddCell(cellsd);

                    
                    //cellsd = new PdfPCell(new Phrase(Convert.ToString(Convert.ToDecimal(DT1.Rows[0]["TotalAmountBePay"]) + Convert.ToDecimal(DT1.Rows[0]["EmitraAmount"]) + OdhiCharge), subheadfont));
                    cellsd = new PdfPCell(new Phrase(Convert.ToString(GrandTotal + Convert.ToDecimal(DT1.Rows[0]["EmitraAmount"]) + OdhiCharge), subheadfont));
                    cellsd.Colspan = 5;
                    cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);


                    //cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
                    //cellsd.Colspan = 5;
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase("Contact Person :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["contactperson"].ToString(), subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase("PhoneNo :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["PhoneNo"].ToString(), subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase("Address :", subheadfont));
                    //SafariDetails.AddCell(cellsd);
                    //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Address"].ToString(), subheadfont));
                    //SafariDetails.AddCell(cellsd);


                    doc.Add(SafariDetails);

                    if (DT3.Rows.Count > 0)
                    {
                        PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
                        cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                        TermsCondition.TotalWidth = 430;
                        TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
                        cellst.Colspan = 2;
                        cellst.HorizontalAlignment = Element.ALIGN_LEFT;
                        TermsCondition.AddCell(cellst);

                        int index = 1;
                        for (int i = 0; i < DT3.Rows.Count; i++)
                        {
                            string sTC = Convert.ToString(DT3.Rows[i]["TermAndCondition_Text"]).Replace("#PlaceName#", DT1.Rows[0]["PlaceName"].ToString());
                            cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                            TermsCondition.AddCell(cellst);
                            cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                            TermsCondition.AddCell(cellst);
                            index += 1;
                        }
                        doc.Add(TermsCondition);
                    }
                    if (DT4.Rows.Count > 0)
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
                        foreach (DataRow DR in DT4.Rows)
                        {
                            k += Convert.ToInt16(1);

                            cellt = new PdfPCell(new Phrase(DR["Period"].ToString(), subheadfont));
                            Timing.AddCell(cellt);
                            cellt = new PdfPCell(new Phrase(DR["MorningTrip"].ToString(), subheadfont));
                            Timing.AddCell(cellt);
                            cellt = new PdfPCell(new Phrase(DR["AfterNoonTrip"].ToString(), subheadfont));
                            Timing.AddCell(cellt);
                        }
                        doc.Add(Timing);
                    }
                    return filepath;
                }
                catch (Exception e)
                { }
                finally
                {
                    doc.Close();
                }

            }
            return "";
        }

        public static string NP_GenerateBoardingPaas(DataSet DS)
        {
            if (DS.Tables.Count > 3)
            {
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    DataTable DT1 = DS.Tables[0];
                    DataTable DT2 = DS.Tables[1];
                    DataTable DT3 = DS.Tables[2];
                    DataTable DT4 = DS.Tables[3];
                    string filepath = string.Empty;
                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_BoardingPass_" + Convert.ToString(DT1.Rows[0]["RequestId"]) + ".pdf");

                    if (!File.Exists(filepath))
                    {
                        #region Write data to PDF
                        PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));
                        var FontColour = new BaseColor(0, 0, 0);
                        Paragraph tableheading = null;

                        var MyFont = FontFactory.GetFont("Arial", 8, FontColour);
                        var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                        var tableHeaderFont = FontFactory.GetFont("Arial", 8, FontColour);
                        doc.Open();
                        doc.NewPage();

                        PdfPTable table;

                        table = new PdfPTable(3);

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


                        tableheading = new Paragraph("Department of Forest,", MyFont);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);

                        doc.Add(tableheading);

                        tableheading = new Paragraph("Goverment of", MyFont);
                        tableheading.Font.Size = 12;
                        tableheading.Alignment = (Element.ALIGN_CENTER);
                        doc.Add(tableheading);

                        tableheading = new Paragraph("Rajasthan.", MyFont);
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


                        cells = new PdfPCell(new Phrase("Booking No : " + Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont));
                        cells.Colspan = 4;
                        cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                        tabular.AddCell(cells);


                        cells = new PdfPCell(new Phrase("Visit Date", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Booking Date", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Route Name", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Total Members", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Shift", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Vehicle"]), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Guide Name", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Ticket Amount", subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase("Mode Of Booking", subheadfont));
                        tabular.AddCell(cells);


                        cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["VisitingDate"]).ToString("dd-MMM-yyyy"), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["BookingDateTime"]).ToString("dd-MMM-yyyy hh:mm:ss tt"), subheadfont));
                        tabular.AddCell(cells);



                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ZoneName"]), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TotalMember"]), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Shift"]), subheadfont));
                        tabular.AddCell(cells);
                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleNumber"]), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["GuideName"]), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TotalPaidAmount"]), subheadfont));
                        tabular.AddCell(cells);

                        cells = new PdfPCell(new Phrase(((BookingType)Convert.ToByte(DT1.Rows[0]["BookingType"])).ToString(), subheadfont));
                        tabular.AddCell(cells);


                        doc.Add(tabular);
                        doc.Add(table);


                        PdfPTable VisitorDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        PdfPCell cellsv = new PdfPCell();// { Border = 4};
                        VisitorDetails.TotalWidth = 180;

                        VisitorDetails.SetTotalWidth(new float[] { 15f, 35f, 20f, 30f, 20f, 20f });


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
                        cellsv = new PdfPCell(new Phrase("Still Camera", tableHeaderFont)); //
                        cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                        VisitorDetails.AddCell(cellsv);
                        cellsv = new PdfPCell(new Phrase("Video Camera", tableHeaderFont)); //
                        cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                        VisitorDetails.AddCell(cellsv);


                        Int16 j = 0;
                        foreach (DataRow DR in DT2.Rows)
                        {
                            j += Convert.ToInt16(1);

                            cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                            VisitorDetails.AddCell(cellsv);
                            cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["Name"]), subheadfont));
                            VisitorDetails.AddCell(cellsv);
                            cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["VisitorType"]), subheadfont));
                            VisitorDetails.AddCell(cellsv);
                            cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["IDName"]) + '/' + Convert.ToString(DR["IDNo"]), subheadfont));
                            VisitorDetails.AddCell(cellsv);
                            cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["NoOfStillCamera"]), subheadfont));
                            VisitorDetails.AddCell(cellsv);
                            cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["NofVideoCamera"]), subheadfont));
                            VisitorDetails.AddCell(cellsv);
                        }
                        doc.Add(VisitorDetails);

                        //PdfPTable SafariDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                        //PdfPCell cellsd;
                        //cellsd = new PdfPCell(new Phrase("Boarding Point :", subheadfont));
                        //SafariDetails.TotalWidth = 130;
                        //SafariDetails.SetTotalWidth(new float[] { 24f, 100f });
                        //SafariDetails.AddCell(cellsd);
                        //cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["Boarding_Point"].ToString(), subheadfont));
                        //SafariDetails.AddCell(cellsd);
                        //doc.Add(SafariDetails);

                        if (DT3.Rows.Count > 0)
                        {
                            PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
                            cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            TermsCondition.TotalWidth = 430;
                            TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
                            cellst.Colspan = 2;
                            cellst.HorizontalAlignment = Element.ALIGN_LEFT;
                            TermsCondition.AddCell(cellst);

                            int index = 1;
                            for (int i = 0; i < DT3.Rows.Count; i++)
                            {
                                string sTC = Convert.ToString(DT3.Rows[i]["TermAndCondition_Text"]).Replace("#PlaceName#", DT1.Rows[0]["PlaceName"].ToString());
                                cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                                TermsCondition.AddCell(cellst);
                                cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                                TermsCondition.AddCell(cellst);
                                index += 1;
                            }
                            doc.Add(TermsCondition);
                        }
                        if (DT4.Rows.Count > 0)
                        {

                            PdfPTable DoS = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellst = new PdfPCell(new Phrase("Abide by the rules of the " + Convert.ToString(DT1.Rows[0]["PlaceName"]), subheadfont));// { Border = 4};
                            cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            DoS.TotalWidth = 430;
                            DoS.SetTotalWidth(new float[] { 30f, 400f });
                            cellst.Colspan = 2;
                            cellst.HorizontalAlignment = Element.ALIGN_LEFT;
                            DoS.AddCell(cellst);

                            if (DT4.Select("Type=1").Length > 0)
                            {
                                PdfPCell cellst1 = new PdfPCell(new Phrase("DO's:", subheadfont));// { Border = 4};                            
                                DoS.TotalWidth = 430;
                                DoS.SetTotalWidth(new float[] { 30f, 400f });
                                cellst1.Colspan = 2;
                                cellst1.HorizontalAlignment = Element.ALIGN_LEFT;
                                DoS.AddCell(cellst1);

                                DataRow[] rows = DT4.Select("Type=1");
                                int index = 1;
                                foreach (DataRow dr in rows)
                                {
                                    string sTC = Convert.ToString(dr["Title"]);
                                    cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                                    DoS.AddCell(cellst);
                                    cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                                    DoS.AddCell(cellst);
                                    index += 1;
                                }
                            }

                            if (DT4.Select("Type=2").Length > 0)
                            {
                                PdfPCell cellst1 = new PdfPCell(new Phrase("DONT's:", subheadfont));// { Border = 4};                            
                                DoS.TotalWidth = 430;
                                DoS.SetTotalWidth(new float[] { 30f, 400f });
                                cellst1.Colspan = 2;
                                cellst1.HorizontalAlignment = Element.ALIGN_LEFT;
                                DoS.AddCell(cellst1);

                                DataRow[] rows = DT4.Select("Type=1");
                                int index = 1;
                                foreach (DataRow dr in rows)
                                {
                                    string sTC = Convert.ToString(dr["Title"]);
                                    cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                                    DoS.AddCell(cellst);
                                    cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                                    DoS.AddCell(cellst);
                                    index += 1;
                                }
                            }
                            doc.Add(DoS);
                        }
                        #endregion
                    }
                    return filepath;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    doc.Close();
                }

            }
            return "";
        }
    }
}