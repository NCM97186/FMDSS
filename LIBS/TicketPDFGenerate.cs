﻿using FMDSS.Models.GVChoice;
using FMDSS.Models.NP_ChoiceGuideVehicleBoat;
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

					tableheading = new Paragraph("Government of", MyFont);
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
            decimal FacilityCharge = 0;
            bool IsFacilityCharge = false;
            decimal MaintenanceCharge = 0;
            bool IsMaintenanceCharge = false;
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
					bool IsOdhiCharge = false;
					//foreach (DataRow DR in DT2.Rows)
					//{
					
					filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_Ticket_" + Convert.ToString(DT1.Rows[0]["RequestId"]) + ".pdf");
					if (Convert.ToBoolean(DT1.Rows[0]["IsOdhiExist"].ToString()) == true)
					{
						IsOdhiCharge = true;
						OdhiCharge = Convert.ToDecimal(DT1.Rows[0]["OdhiCharge"].ToString());
					}
                    DataColumnCollection columns = DT1.Columns;
                    if (columns.Contains("IsFacilityCharge"))
                    {
                        if (string.IsNullOrEmpty(DT1.Rows[0]["IsFacilityCharge"].ToString()) == false)
                        {
                            if (Convert.ToBoolean(DT1.Rows[0]["IsFacilityCharge"].ToString()) == true)
                            {
                                IsFacilityCharge = true;
                                FacilityCharge = Convert.ToDecimal(DT1.Rows[0]["FacilityCharge"].ToString());
                            }
                        }
                    }
                    if (columns.Contains("MaintenanceCharge"))
                    {
                        if (string.IsNullOrEmpty(DT1.Rows[0]["IsMaintenanceCharge"].ToString()) == false)
                        {
                            if (Convert.ToBoolean(DT1.Rows[0]["IsMaintenanceCharge"].ToString()) == true)
                            {
                                IsMaintenanceCharge = true;
                                MaintenanceCharge = Convert.ToDecimal(DT1.Rows[0]["MaintenanceCharge"].ToString());
                            }
                        }
                    }

                    if ((IsOdhiCharge == true || DT1.Rows[0]["ZoneName"].ToString() == "Amagarh") && Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 60 && (Convert.ToInt16(DT1.Rows[0]["ShiftId"].ToString()) == 16 || Convert.ToInt16(DT1.Rows[0]["ShiftId"].ToString()) == 17))// halfday morning or halfday evening
					{

						int totMember = Convert.ToInt16(DT1.Rows[0]["TotalMember"].ToString());
						//nNumberOfSeats = (6 / totMember);
					    PrintHalfDayTicket(DS, doc, IsOdhiCharge, OdhiCharge, totMember,filepath);
					}
                    else if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 19)// 
                    {

                        int totMember = Convert.ToInt16(DT1.Rows[0]["TotalMember"].ToString());
                        //nNumberOfSeats = (6 / totMember);
                        PrintTicketKumbhalGarh(DS, doc, IsFacilityCharge, FacilityCharge, totMember, filepath);
                    }
                    else if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 12 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 36 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 49 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 74 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 75 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 77 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 78)// Line Number 440
                    {

                        int totMember = Convert.ToInt16(DT1.Rows[0]["TotalMember"].ToString());
                        //nNumberOfSeats = (6 / totMember);
                        PrintTicketPalighat(DS, doc, IsFacilityCharge, FacilityCharge, IsMaintenanceCharge, MaintenanceCharge, totMember, filepath);
                    }
                    else
					{
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

						tableheading = new Paragraph("Government of", MyFont);
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

								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["NoOfStillCamera"]), subheadfont));
								VisitorDetails.AddCell(cellsv);

								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["NofVideoCamera"]), subheadfont));
								VisitorDetails.AddCell(cellsv);

								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["MemberEntryFee"]), subheadfont));
								VisitorDetails.AddCell(cellsv);
                                
                                cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["MemberVFPMC"]), subheadfont));
                                VisitorDetails.AddCell(cellsv);
                                
								string vehfeeEntryFee = Convert.ToString(DR["VechileEntryFee"]);
								cellsv = new PdfPCell(new Phrase(vehfeeEntryFee, subheadfont));
								VisitorDetails.AddCell(cellsv);

                                
                                cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["VechileVFPMC"]), subheadfont));
                                VisitorDetails.AddCell(cellsv);
                                 
                           

								string vehicleRentFee = Convert.ToString(DR["VehicleRentFee"]);
								cellsv = new PdfPCell(new Phrase(vehicleRentFee, subheadfont));
								VisitorDetails.AddCell(cellsv);


								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["GuideFees"]), subheadfont));
								VisitorDetails.AddCell(cellsv);

								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["CameraFees"]), subheadfont));
								VisitorDetails.AddCell(cellsv);

								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["GSTAmount"]), subheadfont));
								VisitorDetails.AddCell(cellsv);

								cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["TotalAmount"]), subheadfont));
								VisitorDetails.AddCell(cellsv);

								GrandTotal = GrandTotal + Convert.ToInt32(DR["TotalAmount"].ToString());

							}
							doc.Add(VisitorDetails);

							PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
							PdfPCell cellsd;

							cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
							SafariDetails.TotalWidth = 130;
							SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
							SafariDetails.AddCell(cellsd);

							cellsd = new PdfPCell(new Phrase((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString()), subheadfont));
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
							cellsd = new PdfPCell(new Phrase(Convert.ToString(GrandTotal + Convert.ToDecimal((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString())) + OdhiCharge), subheadfont));
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

	    static void PrintHalfDayTicket(DataSet DS,Document doc,bool IsOdhiCharge,decimal OdhiCharge,int TotalMember , string filepath)
		{
			
			try
			{
				DataTable DT1 = DS.Tables[0];
				DataTable DT2 = DS.Tables[1];
				DataTable DT3 = DS.Tables[2];
				DataTable DT4 = DS.Tables[3];

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

				tableheading = new Paragraph("Government of", MyFont);
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

				PdfPTable VisitorDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
				PdfPCell cellsv = new PdfPCell();// { Border = 4};
				VisitorDetails.TotalWidth = 150;


				VisitorDetails.SetTotalWidth(new float[] { 15f, 50f, 50f });

				cellsv = new PdfPCell(new Phrase("S No.", tableHeaderFont));
				cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				VisitorDetails.AddCell(cellsv);
				cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
				cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				VisitorDetails.AddCell(cellsv);
				cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
				cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				VisitorDetails.AddCell(cellsv);
			

				Int16 j = 0;
				decimal GrandTotal = 0;
				decimal TotalMemberEntryFee = 0;
				decimal TotalMemberVFPMC = 0;
				decimal TotalVechileEntryFee = 0; decimal TotalVechileVFPMC = 0;
				decimal TotalVehicleRentFee = 0; decimal TotalGSTAmount = 0;
				decimal TotalAmount = 0; 
				foreach (DataRow DR in DT2.Rows)
				{
					j += Convert.ToInt16(1);
					cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
					VisitorDetails.AddCell(cellsv);

					cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["Name"]) + " - " + Convert.ToString(DR["VisitorType"]), subheadfont));
					VisitorDetails.AddCell(cellsv);

					cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["IDName"]) + " / " + Convert.ToString(DR["IDNo"]), subheadfont));
					VisitorDetails.AddCell(cellsv);

					TotalMemberEntryFee+=Convert.ToInt32(DR["MemberEntryFee"]);
					TotalMemberVFPMC+=Convert.ToInt32(DR["MemberVFPMC"]);
					TotalVechileEntryFee=Convert.ToInt32(DR["VechileEntryFee"]);
					TotalVechileVFPMC=Convert.ToInt32(DR["VechileVFPMC"]);
					TotalVehicleRentFee=Convert.ToInt32(DR["VehicleRentFee"]);
					TotalGSTAmount=Convert.ToDecimal(DR["GSTAmountHDFd"]);
					TotalAmount=Convert.ToInt32(DR["TotalAmount"]);
					

				}
				

				PdfPTable FeeDetails = new PdfPTable(8) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
				PdfPCell cellFee = new PdfPCell();// { Border = 4};
				FeeDetails.TotalWidth = 180;


				FeeDetails.SetTotalWidth(new float[] { 5f, 15f, 15f, 15f, 15f, 15f, 10f, 10f });

				cellFee = new PdfPCell(new Phrase("Seats For Calculation", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("Member Entry Fee", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("Member Entry VFPMC Charge", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("Vehicle Entry Fee", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("Vehicle Entry VFPMC Charge", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("Vehicle Rent", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("GST Amount on Vehicle Rent", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);
				cellFee = new PdfPCell(new Phrase("Total Amount(INR)", tableHeaderFont));
				cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
				FeeDetails.AddCell(cellFee);

				doc.Add(VisitorDetails);




				// GrandTotal ,
				// TotalMemberEntryFee 
				//, TotalMemberVFPMC
				//, TotalVechileEntryFee  TotalVechileVFPMC 
				//, TotalVehicleRentFee   TotalGSTAmount 
				//, TotalAmount 

				cellFee = new PdfPCell(new Phrase("6", subheadfont));
				FeeDetails.AddCell(cellFee);

				cellFee = new PdfPCell(new Phrase(Convert.ToString(TotalMemberEntryFee), subheadfont));
				FeeDetails.AddCell(cellFee);

				cellFee = new PdfPCell(new Phrase( TotalMemberVFPMC.ToString(), subheadfont));
				FeeDetails.AddCell(cellFee);

				cellFee = new PdfPCell(new Phrase(TotalVechileEntryFee.ToString() +" * 6 =" + (TotalVechileEntryFee * 6), subheadfont));
				FeeDetails.AddCell(cellFee);

				cellFee = new PdfPCell(new Phrase(TotalVechileVFPMC.ToString() + " * 6 =" + (TotalVechileVFPMC * 6), subheadfont));
				FeeDetails.AddCell(cellFee);

				cellFee = new PdfPCell(new Phrase(TotalVehicleRentFee.ToString() + " * 6 =" + (TotalVehicleRentFee * 6), subheadfont));
				FeeDetails.AddCell(cellFee);

				cellFee = new PdfPCell(new Phrase(TotalGSTAmount.ToString() + " * 6 =" + Math.Ceiling((TotalGSTAmount * 6)), subheadfont));
				FeeDetails.AddCell(cellFee);
				GrandTotal = TotalMemberEntryFee + TotalMemberVFPMC + Math.Ceiling((6 * (TotalVechileEntryFee + TotalVechileVFPMC + TotalVehicleRentFee + TotalGSTAmount)));
				cellFee = new PdfPCell(new Phrase(Convert.ToString(GrandTotal) , subheadfont));
				FeeDetails.AddCell(cellFee);



				doc.Add(FeeDetails);

				PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
				PdfPCell cellsd;

				cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
				SafariDetails.TotalWidth = 130;
				SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
				SafariDetails.AddCell(cellsd);

				cellsd = new PdfPCell(new Phrase((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString()), subheadfont));
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
				cellsd = new PdfPCell(new Phrase(Convert.ToString(GrandTotal + Convert.ToDecimal((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString())) + OdhiCharge), subheadfont));
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
			}
			catch (Exception e)
			{ }
			finally
			{
				doc.Close();
			}
		}
        static void PrintTicketKumbhalGarh(DataSet DS, Document doc, bool IsFacilityCharge, decimal FacilityCharge, int TotalMember, string filepath)
        {

            try
            {
                DataTable DT1 = DS.Tables[0];
                DataTable DT2 = DS.Tables[1];
                DataTable DT3 = DS.Tables[2];
                DataTable DT4 = DS.Tables[3];

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

                tableheading = new Paragraph("Government of", MyFont);
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

                PdfPTable VisitorDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 150;


                VisitorDetails.SetTotalWidth(new float[] { 15f, 50f, 50f, 50f });

                cellsv = new PdfPCell(new Phrase("S No.", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Camera Count", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);



                Int16 j = 0;
                decimal GrandTotal = 0;
                decimal TotalMemberEntryFee = 0;
                decimal TotalVechile_EntryFee = 0;
                decimal TotalVehicleRentFee = 0; decimal TotalVechile_ECO = 0;
                decimal TotalGuide_Fee = 0; decimal TotalMember_Fee_ECO = 0;
                decimal TotalVideoCameraAmount = 0; decimal TotalVehicleAmt = 0;
                decimal TotalVehGSTAmount = 0; decimal TotalGuideGSTAmount = 0;
                decimal TotalAmount = 0;

                decimal GSTGuidePrcnt = 0;
                decimal GSTVehiclePrcnt = 0;

                foreach (DataRow DR in DT2.Rows)
                {
                    j += Convert.ToInt16(1);
                    cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["Name"]) + " - " + Convert.ToString(DR["VisitorType"]), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["IDName"]) + " / " + Convert.ToString(DR["IDNo"]), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Convert.ToInt16(DR["NofVideoCamera"]) + Convert.ToInt16(DR["NoOfStillCamera"])), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    TotalMemberEntryFee += Convert.ToDecimal(DR["MemberEntryFee"]);
                    TotalMember_Fee_ECO += Convert.ToDecimal(DR["Member_Fee_ECO"]);

                    TotalVideoCameraAmount += Convert.ToDecimal(DR["CameraFees"]);

                    TotalGuide_Fee = Convert.ToDecimal(DR["Guide_Fee"]);
                    //TotalGuideGSTAmount += Convert.ToInt32(DR["GuideGSTAmount"]);


                    TotalVechile_EntryFee = Convert.ToDecimal(DR["Vechile_EntryFee"]);
                    TotalVechile_ECO = Convert.ToDecimal(DR["Vechile_ECO"]);
                    TotalVehicleRentFee = Convert.ToDecimal(DR["VehicleRentFee"]);
                    //TotalVehGSTAmount += Convert.ToInt32(DR["VehGSTAmount"]);

                    TotalVehicleAmt = Convert.ToDecimal(DR["VehicleRentFee"]) + Convert.ToDecimal(DR["Vechile_ECO"]) + Convert.ToDecimal(DR["Vechile_EntryFee"]);

                    TotalAmount += Convert.ToDecimal(DR["TotalAmount"]);

                    GSTGuidePrcnt = Convert.ToDecimal(DR["GuideFeesGST"]);

                    GSTVehiclePrcnt = Convert.ToDecimal(DR["VechileFeesGST"]);
                }
                TotalGuide_Fee = TotalGuide_Fee * 6;
                TotalVechile_EntryFee = TotalVechile_EntryFee * 6;
                TotalVechile_ECO = TotalVechile_ECO * 6;
                TotalVehicleRentFee = TotalVehicleRentFee * 6;
                TotalVehicleAmt = TotalVehicleAmt * 6;

                TotalGuideGSTAmount = Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 2);
                TotalVehGSTAmount = Math.Round(TotalVehicleRentFee * GSTVehiclePrcnt / 100, 2);
                doc.Add(VisitorDetails);

                PdfPTable FeeDetails = new PdfPTable(11) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellFee = new PdfPCell();// { Border = 4};
                FeeDetails.TotalWidth = 180;


                FeeDetails.SetTotalWidth(new float[] { 5f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f });

                cellFee = new PdfPCell(new Phrase("Seats For Calculation", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Member Entry Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Member ECO Development Charge", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Video Camera Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("GST Amount On Guide Fee (" + Math.Round(GSTGuidePrcnt, 2) + "%)", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Vehicle Entry Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Vehicle ECO Development Charge", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Vehicle Rent Charge", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);


                cellFee = new PdfPCell(new Phrase("GST Amount on Vehicle Rent (" + Math.Round(GSTVehiclePrcnt, 2) + "%)", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Total Amount(INR)", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                //doc.Add(VisitorDetails);




                // GrandTotal ,
                // TotalMemberEntryFee 
                //, TotalMemberVFPMC
                //, TotalVechileEntryFee  TotalVechileVFPMC 
                //, TotalVehicleRentFee   TotalGSTAmount 
                //, TotalAmount 

                cellFee = new PdfPCell(new Phrase(TotalMember.ToString(), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalMemberEntryFee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalMember_Fee_ECO, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVideoCameraAmount, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalGuide_Fee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVechile_EntryFee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVechile_ECO, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVehicleRentFee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round((TotalVehicleRentFee * GSTVehiclePrcnt) / 100, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                GrandTotal = Math.Round(TotalMemberEntryFee, 0) + Math.Round(TotalMember_Fee_ECO, 0) + Math.Round(TotalVideoCameraAmount, 0) + Math.Round(TotalGuide_Fee, 0) + Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 0) + Math.Round(TotalVechile_EntryFee, 0) + Math.Round(TotalVechile_ECO, 0) + Math.Round(TotalVehicleRentFee, 0) + Math.Round((TotalVehicleRentFee * GSTVehiclePrcnt) / 100, 0);
                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(GrandTotal, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);



                doc.Add(FeeDetails);

                PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsd;

                cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
                SafariDetails.TotalWidth = 130;
                SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                SafariDetails.AddCell(cellsd);

                cellsd = new PdfPCell(new Phrase((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString()), subheadfont));
                cellsd.Colspan = 5;
                cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                SafariDetails.AddCell(cellsd);

                if (IsFacilityCharge == true)
                {
                    cellsd = new PdfPCell(new Phrase("Facility Charge (INR):", subheadfont)); //Vehicle						
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);

                    cellsd = new PdfPCell(new Phrase(Convert.ToString(Math.Round(FacilityCharge, 0)), subheadfont));
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
                cellsd = new PdfPCell(new Phrase(Convert.ToString(Math.Round(GrandTotal, 0) + Convert.ToDecimal((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString())) + Math.Round(FacilityCharge, 0)), subheadfont));
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
            }
            catch (Exception e)
            { }
            finally
            {
                doc.Close();
            }
        }
        static void PrintTicketPalighat(DataSet DS, Document doc, bool IsFacilityCharge, decimal FacilityCharge, bool IsMaintenanceCharge, decimal MaintenanceCharge, int TotalMember, string filepath)
        {

            try
            {
                DataTable DT1 = DS.Tables[0];
                DataTable DT2 = DS.Tables[1];
                DataTable DT3 = DS.Tables[2];
                DataTable DT4 = DS.Tables[3];

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

                tableheading = new Paragraph("Government of", MyFont);
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

                PdfPTable VisitorDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsv = new PdfPCell();// { Border = 4};
                VisitorDetails.TotalWidth = 150;


                VisitorDetails.SetTotalWidth(new float[] { 15f, 50f, 50f, 50f });

                cellsv = new PdfPCell(new Phrase("S No.", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Visitor Name", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Id proof & Id details", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);
                cellsv = new PdfPCell(new Phrase("Camera Count", tableHeaderFont));
                cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                VisitorDetails.AddCell(cellsv);



                Int16 j = 0;
                decimal GrandTotal = 0;
                decimal TotalMemberEntryFee = 0;
                decimal TotalVechile_EntryFee = 0;
                decimal TotalVehicleRentFee = 0; decimal TotalVechile_ECO = 0;
                decimal TotalGuide_Fee = 0; decimal TotalMember_Fee_ECO = 0;
                decimal TotalVideoCameraAmount = 0; decimal TotalVehicleAmt = 0;
                decimal TotalVehGSTAmount = 0; decimal TotalGuideGSTAmount = 0;
                decimal TotalMaintenanceCharge = 0;
                decimal TotalAmount = 0;

                decimal GSTGuidePrcnt = 0;
                decimal GSTVehiclePrcnt = 0;

                foreach (DataRow DR in DT2.Rows)
                {
                    j += Convert.ToInt16(1);
                    cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["Name"]) + " - " + Convert.ToString(DR["VisitorType"]), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(Convert.ToString(DR["IDName"]) + " / " + Convert.ToString(DR["IDNo"]), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    cellsv = new PdfPCell(new Phrase(Convert.ToString(Convert.ToInt16(DR["NofVideoCamera"]) + Convert.ToInt16(DR["NoOfStillCamera"])), subheadfont));
                    VisitorDetails.AddCell(cellsv);

                    TotalMemberEntryFee += Convert.ToDecimal(DR["MemberEntryFee"]);
                    TotalMember_Fee_ECO += Convert.ToDecimal(DR["Member_Fee_ECO"]);

                    TotalVideoCameraAmount += Convert.ToDecimal(DR["CameraFees"]);

                    TotalGuide_Fee += Convert.ToDecimal(DR["Guide_Fee"]);
                    //TotalGuideGSTAmount += Convert.ToInt32(DR["GuideGSTAmount"]);


                    TotalVechile_EntryFee += Convert.ToDecimal(DR["Vechile_EntryFee"]);
                    TotalVechile_ECO += Convert.ToDecimal(DR["Vechile_ECO"]);
                    TotalVehicleRentFee += Convert.ToDecimal(DR["VehicleRentFee"]);
                    //TotalVehGSTAmount += Convert.ToInt32(DR["VehGSTAmount"]);

                    TotalVehicleAmt += Convert.ToDecimal(DR["VehicleRentFee"]) + Convert.ToDecimal(DR["Vechile_ECO"]) + Convert.ToDecimal(DR["Vechile_EntryFee"]);

                    TotalAmount += Convert.ToDecimal(DR["TotalAmount"]);

                    TotalMaintenanceCharge += Convert.ToDecimal(DR["TRDF_Maintenance"]);

                    GSTGuidePrcnt = Convert.ToDecimal(DR["GuideFeesGST"]);

                    GSTVehiclePrcnt = Convert.ToDecimal(DR["VechileFeesGST"]);
                }
                //TotalGuide_Fee = TotalGuide_Fee * TotalMember;
                //TotalVechile_EntryFee = TotalVechile_EntryFee * TotalMember;
                //TotalVechile_ECO = TotalVechile_ECO * TotalMember;
                //TotalVehicleRentFee = TotalVehicleRentFee * TotalMember;
                //TotalVehicleAmt = TotalVehicleAmt * TotalMember;
                //TotalMaintenanceCharge = TotalMaintenanceCharge * TotalMember;

                TotalGuideGSTAmount = Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 2);
                TotalVehGSTAmount = Math.Round(TotalVehicleRentFee * GSTVehiclePrcnt / 100, 2);
                doc.Add(VisitorDetails);

                PdfPTable FeeDetails = new PdfPTable(11) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellFee = new PdfPCell();// { Border = 4};
                FeeDetails.TotalWidth = 180;


                FeeDetails.SetTotalWidth(new float[] { 5f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f });

                cellFee = new PdfPCell(new Phrase("Seats For Calculation", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Member Entry Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Member ECO Development Charge", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Video Camera Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("Guide Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);
                cellFee = new PdfPCell(new Phrase("GST Amount On Guide Fee (" + Math.Round(GSTGuidePrcnt, 2) + "%)", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Vehicle Entry Fee", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Vehicle ECO Development Charge", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Vehicle Rent Charge", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);


                cellFee = new PdfPCell(new Phrase("GST Amount on Vehicle Rent (" + Math.Round(GSTVehiclePrcnt, 2) + "%)", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase("Total Amount(INR)", tableHeaderFont));
                cellFee.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                FeeDetails.AddCell(cellFee);

                //doc.Add(VisitorDetails);




                // GrandTotal ,
                // TotalMemberEntryFee 
                //, TotalMemberVFPMC
                //, TotalVechileEntryFee  TotalVechileVFPMC 
                //, TotalVehicleRentFee   TotalGSTAmount 
                //, TotalAmount 

                cellFee = new PdfPCell(new Phrase(TotalMember.ToString(), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalMemberEntryFee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalMember_Fee_ECO, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVideoCameraAmount, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalGuide_Fee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVechile_EntryFee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVechile_ECO, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);

                cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(TotalVehicleRentFee, 0)), subheadfont));
                FeeDetails.AddCell(cellFee);


                if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 36 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 12 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 49)
                {
                    cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round((TotalVehicleRentFee * GSTVehiclePrcnt) / 100, 2)), subheadfont));
                    decimal vehGST = Math.Round((TotalVehicleRentFee * GSTVehiclePrcnt) / 100, 2);
                    FeeDetails.AddCell(cellFee);
                    GrandTotal = Math.Round(TotalMemberEntryFee, 0) + Math.Round(TotalMember_Fee_ECO, 0) + Math.Round(TotalVideoCameraAmount, 0) + Math.Round(TotalGuide_Fee, 0) + Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 0) + Math.Round(TotalVechile_EntryFee, 0) + Math.Round(TotalVechile_ECO, 0) + Math.Round(TotalVehicleRentFee, 2) + vehGST;

                    cellFee = new PdfPCell(new Phrase(Convert.ToString(GrandTotal), subheadfont));
                    FeeDetails.AddCell(cellFee);
                }
                else
                {
                    cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round((TotalVehicleRentFee * GSTVehiclePrcnt) / 100, 2)), subheadfont));
                    FeeDetails.AddCell(cellFee);
                    GrandTotal = Math.Round(TotalMemberEntryFee, 0) + Math.Round(TotalMember_Fee_ECO, 0) + Math.Round(TotalVideoCameraAmount, 0) + Math.Round(TotalGuide_Fee, 0) + Math.Round((TotalGuide_Fee * GSTGuidePrcnt) / 100, 0) + Math.Round(TotalVechile_EntryFee, 0) + Math.Round(TotalVechile_ECO, 0) + Math.Round(TotalVehicleRentFee, 0) + Math.Round((TotalVehicleRentFee * GSTVehiclePrcnt) / 100, 0);

                    cellFee = new PdfPCell(new Phrase(Convert.ToString(Math.Round(GrandTotal, 0)), subheadfont));
                    FeeDetails.AddCell(cellFee);
                }





                doc.Add(FeeDetails);

                PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellsd;

                cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
                SafariDetails.TotalWidth = 130;
                SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                SafariDetails.AddCell(cellsd);

                cellsd = new PdfPCell(new Phrase((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString()), subheadfont));
                cellsd.Colspan = 5;
                cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                SafariDetails.AddCell(cellsd);

                if (IsFacilityCharge == true)
                {
                    cellsd = new PdfPCell(new Phrase("Facility Charge (INR):", subheadfont)); //Vehicle						
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);
                    if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 49)
                    {
                        FacilityCharge = Math.Round(FacilityCharge, 2);
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(FacilityCharge), subheadfont));
                    }
                    else
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(FacilityCharge), subheadfont));

                    cellsd.Colspan = 5;
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);
                }

                if (IsMaintenanceCharge == true)
                {
                    cellsd = new PdfPCell(new Phrase("Maintenance Charge (INR):", subheadfont)); //Vehicle						
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);
                    if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 36)
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(MaintenanceCharge), subheadfont));
                    else
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(Math.Round(MaintenanceCharge, 0)), subheadfont));

                    cellsd.Colspan = 5;
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);
                }


                cellsd = new PdfPCell(new Phrase("Grand Total (INR) : ", subheadfont));// { Border = 4};
                SafariDetails.TotalWidth = 130;
                SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                SafariDetails.AddCell(cellsd);


                if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 36 || Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 49)
                    cellsd = new PdfPCell(new Phrase(Convert.ToString(GrandTotal + Convert.ToDecimal((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString())) + FacilityCharge + MaintenanceCharge), subheadfont));
                else if (Convert.ToInt32(DT1.Rows[0]["PlaceId"].ToString()) == 12)
                    cellsd = new PdfPCell(new Phrase(Convert.ToString(Math.Ceiling(GrandTotal) + Convert.ToDecimal((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString())) + Math.Round(FacilityCharge, 0) + Math.Round(MaintenanceCharge, 0)), subheadfont));
                else
                    cellsd = new PdfPCell(new Phrase(Convert.ToString(Math.Round(GrandTotal, 0) + Convert.ToDecimal((DT1.Rows[0]["EmitraAmount"].ToString() == "" || DT1.Rows[0]["EmitraAmount"] == null ? "0" : DT1.Rows[0]["EmitraAmount"].ToString())) + Math.Round(FacilityCharge, 0) + Math.Round(MaintenanceCharge, 0)), subheadfont));
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
            }
            catch (Exception e)
            { }
            finally
            {
                doc.Close();
            }
        }
        public static string NP_GenerateGuideOrBoatReceipt(List<NPChoice> npReceiptList)
        {
            decimal TotalAmount = 0;

            if (npReceiptList.Count > 0)
            {
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    string filepath = string.Empty;

                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_BoatReceipt_" + Convert.ToString(npReceiptList[1].ChoiceRequestId) + ".pdf");

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
                    cellRislLogo.PaddingTop = -150;
                    cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cellRislLogo);

                    ////Add Heading

                    tableheading = new Paragraph("Department of Forest,", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);

                    doc.Add(tableheading);

                    tableheading = new Paragraph("Government of", MyFont);
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
                    cellEmitraLogo.PaddingTop = -145;

                    table.AddCell(cellEmitraLogo);
                    //doc.Add(new Paragraph(Environment.NewLine));

                    PdfPTable tbl0 = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellGenerationDate = new PdfPCell(new Phrase("Generated On : " + npReceiptList[0].TransDate, subheadfont));// { Border = 4};
                                                                                                                                        //tabular.TotalWidth = 320;
                    tbl0.SetTotalWidth(new float[] { 60f });
                    // cellGenerationDate.BackgroundColor = new iTextSharp.text.BaseColor(0, 0, 0);
                    cellGenerationDate.Colspan = 1;
                    cellGenerationDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tbl0.AddCell(cellGenerationDate);
                    doc.Add(tbl0);



                    doc.Add(new Paragraph(Environment.NewLine));

                    PdfPTable tabular = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cells = new PdfPCell(new Phrase("Choice Vehicle/Boat/Guide Paymet Slip", subheadfont));// { Border = 4};
                                                                                                                    //tabular.TotalWidth = 320;
                    tabular.SetTotalWidth(new float[] { 60f, 100f, 60f, 100f });
                    cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Place Name:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].PlaceName), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Confirmation No.:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].ChoiceRequestId), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Date Of Visit:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].VisitDate), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Shift Name", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].ShiftName), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Zone Name:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].ZoneName), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Booking Reference No.:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].RequestId), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Visitor Count", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].TotalMember), subheadfont));
                    tabular.AddCell(cells);

                    NPChoice objGuide = npReceiptList.ToList().Where(c => c.ItemName == "Guide Fee").ToList().FirstOrDefault();
                    NPChoice objVeh = npReceiptList.ToList().Where(c => c.ItemName == "VehicleChoiceFee").ToList().FirstOrDefault();

                    cells = new PdfPCell(new Phrase("Choice Guide Name", subheadfont));
                    tabular.AddCell(cells);
                    if (objGuide != null)
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].GuideName), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }


                    cells = new PdfPCell(new Phrase("Choice Vehicle", subheadfont));
                    tabular.AddCell(cells);
                    if (objVeh != null)
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].VehicleName), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("Vehicle Number", subheadfont));
                    tabular.AddCell(cells);
                    if (objVeh != null)
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(npReceiptList[0].VehicleNumber), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("Boat Charge", subheadfont));
                    tabular.AddCell(cells);

                    if (objVeh != null)
                    {
                        TotalAmount += objVeh.TotalChoiceAmt;
                        cells = new PdfPCell(new Phrase(Convert.ToString(objVeh.TotalChoiceAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("Guide Charge", subheadfont));
                    tabular.AddCell(cells);

                    if (objGuide != null)
                    {
                        TotalAmount += objGuide.TotalChoiceAmt;
                        cells = new PdfPCell(new Phrase(Convert.ToString(objGuide.TotalChoiceAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("GST(5%) Vehicle/ Boat RentFee", subheadfont));
                    tabular.AddCell(cells);
                    if (objVeh != null)
                    {
                        TotalAmount += objVeh.TotalChoiceGSTAmt;
                        cells = new PdfPCell(new Phrase(Convert.ToString(objVeh.TotalChoiceGSTAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("GST(18%) on Guide Fee", subheadfont));
                    tabular.AddCell(cells);
                    if (objGuide != null)
                    {
                        TotalAmount += objGuide.TotalChoiceGSTAmt;
                        cells = new PdfPCell(new Phrase(Convert.ToString(objGuide.TotalChoiceGSTAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }
                    NPChoice objEmitra = npReceiptList.ToList().Where(c => c.ItemName == "Emitra Fees").ToList().FirstOrDefault();

                    doc.Add(tabular);

                    doc.Add(table);



                    PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsd;

                    cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);
                    if (objEmitra != null)
                    {
                        TotalAmount += objEmitra.TotalChoiceAmt;
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(objEmitra.TotalChoiceAmt), subheadfont));
                        cellsd.Colspan = 5;
                        cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                        SafariDetails.AddCell(cellsd);
                    }
                    else
                    {
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
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
                    cellsd = new PdfPCell(new Phrase(Convert.ToString(TotalAmount), subheadfont));
                    cellsd.Colspan = 5;
                    cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);


                    doc.Add(SafariDetails);

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

                        tableheading = new Paragraph("Government of", MyFont);
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

                                DataRow[] rows2 = DT4.Select("Type=2");
                                int index = 1;
                                foreach (DataRow dr2 in rows2)
                                {
                                    string sTC = Convert.ToString(dr2["Title"]);
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
        public static string GV_GenerateGuideOrVehicleReceipt(List<GVChoice> gvReceiptList)
        {
            decimal TotalAmount = 0;

            if (gvReceiptList.Count > 0)
            {
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    string filepath = string.Empty;

                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/GV_Receipt_" + Convert.ToString(gvReceiptList[0].ChoiceRequestId) + ".pdf");

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
                    cellRislLogo.PaddingTop = -150;
                    cellRislLogo.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cellRislLogo);

                    ////Add Heading

                    tableheading = new Paragraph("Department of Forest,", MyFont);
                    tableheading.Font.Size = 12;
                    tableheading.Alignment = (Element.ALIGN_CENTER);

                    doc.Add(tableheading);

                    tableheading = new Paragraph("Government of", MyFont);
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
                    cellEmitraLogo.PaddingTop = -145;

                    table.AddCell(cellEmitraLogo);
                    //doc.Add(new Paragraph(Environment.NewLine));

                    PdfPTable tbl0 = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellGenerationDate = new PdfPCell(new Phrase("Generated On : " + gvReceiptList[0].TransDate, subheadfont));// { Border = 4};
                                                                                                                                        //tabular.TotalWidth = 320;
                    tbl0.SetTotalWidth(new float[] { 60f });
                    // cellGenerationDate.BackgroundColor = new iTextSharp.text.BaseColor(0, 0, 0);
                    cellGenerationDate.Colspan = 1;
                    cellGenerationDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tbl0.AddCell(cellGenerationDate);
                    doc.Add(tbl0);



                    doc.Add(new Paragraph(Environment.NewLine));

                    PdfPTable tabular = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cells = new PdfPCell(new Phrase("Choice Vehicle/Boat/Guide Paymet Slip", subheadfont));// { Border = 4};
                                                                                                                    //tabular.TotalWidth = 320;
                    tabular.SetTotalWidth(new float[] { 60f, 100f, 60f, 100f });
                    cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cells.Colspan = 4;
                    cells.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Place Name:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].PlaceName), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Confirmation No.:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].ChoiceRequestId), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Date Of Visit:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].VisitDate), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Shift Name", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].ShiftName), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Zone Name:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].ZoneName), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Booking Reference No.:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].RequestId), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Visitor Count", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].TotalMember), subheadfont));
                    tabular.AddCell(cells);

                    //GVChoice objGuide = gvReceiptList.ToList().Where(c => c.ItemName == "Guide Fee").ToList().FirstOrDefault();
                    //GVChoice objVeh = gvReceiptList.ToList().Where(c => c.ItemName == "VehicleChoiceFee").ToList().FirstOrDefault();

                    cells = new PdfPCell(new Phrase("Choice Guide Name", subheadfont));
                    tabular.AddCell(cells);
                    if (gvReceiptList[0].GuideName != null)
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].GuideName), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }


                    cells = new PdfPCell(new Phrase("Choice Vehicle", subheadfont));
                    tabular.AddCell(cells);
                    if (gvReceiptList[0].VehicleName != null)
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].VehicleName), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("Vehicle Number", subheadfont));
                    tabular.AddCell(cells);
                    if (gvReceiptList[0].VehicleName != null)
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].VehicleNumber), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("Vehicle Charge", subheadfont));
                    tabular.AddCell(cells);
                    TotalAmount = gvReceiptList[0].TotalChoiceAmt+ gvReceiptList[0].TotalChoiceGSTAmt;
                    if (gvReceiptList[0].VehicleName != null)
                    {
                        
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].VehileChoiceAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("Guide Charge", subheadfont));
                    tabular.AddCell(cells);

                    if (gvReceiptList[0].GuideName != null)
                    {
                        
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].GuideChoiceAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("GST(5%) Vehicle/ Boat RentFee", subheadfont));
                    tabular.AddCell(cells);
                    if (gvReceiptList[0].VehicleName != null)
                    {
                       
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].VehileChoiceGSTAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }

                    cells = new PdfPCell(new Phrase("GST(18%) on Guide Fee", subheadfont));
                    tabular.AddCell(cells);
                    if (gvReceiptList[0].GuideName != null)
                    {
                       
                        cells = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].GuideChoiceGSTAmt), subheadfont));
                        tabular.AddCell(cells);
                    }
                    else
                    {
                        cells = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                        tabular.AddCell(cells);
                    }
                    //GVChoice objEmitra = gvReceiptList.ToList().Where(c => c.ItemName == "Emitra Fees").ToList().FirstOrDefault();

                    doc.Add(tabular);

                    doc.Add(table);



                    PdfPTable SafariDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsd;

                    cellsd = new PdfPCell(new Phrase("Service Charges (INR):", subheadfont));// { Border = 4};
                    SafariDetails.TotalWidth = 130;
                    SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
                    SafariDetails.AddCell(cellsd);
                    if (gvReceiptList[0].OnlineCitizenEmitraCharge >0)
                    {
                        TotalAmount += gvReceiptList[0].OnlineCitizenEmitraCharge;
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(gvReceiptList[0].OnlineCitizenEmitraCharge), subheadfont));
                        cellsd.Colspan = 5;
                        cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                        SafariDetails.AddCell(cellsd);
                    }
                    else
                    {
                        cellsd = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
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
                    cellsd = new PdfPCell(new Phrase(Convert.ToString(TotalAmount), subheadfont));
                    cellsd.Colspan = 5;
                    cellsd.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
                    SafariDetails.AddCell(cellsd);


                    doc.Add(SafariDetails);

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
    }
}