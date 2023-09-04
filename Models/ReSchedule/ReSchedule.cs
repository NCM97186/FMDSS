using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
namespace FMDSS.Models.ReSchedule
{
   
	public class ReSchedule : DAL
	{
		public string TicketId { get; set; }
		public string PlaceName { get; set; }
		public int PlaceId { get; set; }
		public int VehicleID { get; set; }
		public string VehicleName { get; set; }
		public string ZoneName { get; set; }
		public string ShiftName { get; set; }
		public string DateofArrival { get; set; }

		public string DateofArrival1 { get; set; }

		public string RequestID { get; set; }

		public decimal TotalAmount { get; set; }
		public string FirstDate { get; set; }
		public string SecondDate { get; set; }
		public string ThirdDate { get; set; }
		public string ApprovedVisitDate { get; set; }
		public int ShiftId { get; set; }
		public int isDFOApproved { get; set; }
		public bool isActive { get; set; }
		public string Remark { get; set; }
		public string Enteredon { get; set; }
		[Required(ErrorMessage = "Please Select Zone")]
		public int ZoneID { get; set; }
        public int ZoneID2 { get; set; }
        public int BookedQuota { get; set; }
		public string QuotaType { get; set; }

		public List<ReScheduleMemberDetails> lstMemberDetails { get; set; }

		public DataSet Select_TicketData(Int64 TicketID)
		{
			DataSet ds = new DataSet();
			try
			{
				DALConn();

				SqlParameter[] parameters =
			{
			new SqlParameter("@TicketID", TicketID)
			};
				Fill(ds, "SP_Citizen_SelecTicketDetail", parameters);

			}

			catch (Exception ex)
			{
				new Common().ErrorLog(ex.Message, "Select_TicketData_For_Covid" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

			}
			finally
			{
				Conn.Close();
			}
			return ds;
		}
		public DataTable GetZoneList(Int64 PlaceId)
		{
			DataTable ds = new DataTable();
			try
			{
				DALConn();

				SqlParameter[] parameters =
			{
			new SqlParameter("@Action", "Get_ZoneList"),
			new SqlParameter("@PlaceId", PlaceId)
			};
				Fill(ds, "spRescheduleBooking", parameters);
			}

			catch (Exception ex)
			{
				new Common().ErrorLog(ex.Message, "Select_TicketData_For_Covid" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

			}
			finally
			{
				Conn.Close();
			}
			return ds;
		}
		public DataTable SaveReScheduleBooking(ReSchedule oBooking,bool isLiveOrUAT)
		{
			DataTable dt = new DataTable();
			try
			{
				DALConn();
				SqlCommand cmd = new SqlCommand("spRescheduleBooking", Conn);
				cmd.Parameters.AddWithValue("@Action", "SaveReScheduleTicketDetail");
				cmd.Parameters.AddWithValue("@ZoneId", oBooking.ZoneID);
                cmd.Parameters.AddWithValue("@ZoneId2", oBooking.ZoneID2);
                cmd.Parameters.AddWithValue("@OldPlaceId", oBooking.PlaceId);
				cmd.Parameters.AddWithValue("@PlaceId", 2);
				cmd.Parameters.AddWithValue("@RequestId", oBooking.RequestID);
                             
                cmd.Parameters.AddWithValue("@IsLiveOrUAT", (isLiveOrUAT==true?1:0));
				
				cmd.Parameters.AddWithValue("@OLDTicketId", Encryption.decrypt(oBooking.TicketId));

				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spRescheduleBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
			}
			finally
			{
				Conn.Close();
			}
			return dt;
		}
        public DataTable CheckIsReScheduledTicketForPrint(Int64 TicketId,string OldRequestTitle)
        {
            DataTable ds = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {
            new SqlParameter("@Action", "IsTicketReScheduled"),
            new SqlParameter("@TicketId", TicketId),
             new SqlParameter("@OldRequestTitle", OldRequestTitle)
           
            };
                Fill(ds, "spRescheduleBooking", parameters);
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckIsReScheduledTicketForPrint" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet Select_ReScheduledTicket_ToPrint(Int64 TicketId)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {
            new SqlParameter("@TicketID", TicketId)
            };
                Fill(ds, "Sp_PrintReScheduleTicket", parameters);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_Print" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public string WildlifeDownloadReScheduledTicketPdf(DataSet ds, DataTable dtTC)
        {
            //HD FD TICKET Rescheduled in normal wildlife ticketing
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
                
                cellQREmitraLogo.PaddingTop = -120;
               
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
            cellRislLogo.PaddingTop = -80;
            
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
           

            EmitraImageLogo.SpacingBefore = -70f;
            EmitraImageLogo.SpacingAfter = -70f;
           
            EmitraImageLogo.Alignment = Element.ALIGN_CENTER;



            PdfPCell cellEmitraLogo;
            cellEmitraLogo = new PdfPCell(EmitraImageLogo);
            cellEmitraLogo.BorderWidth = 0;
            cellEmitraLogo.Padding = 7;
            
            cellEmitraLogo.PaddingTop = -75;
            


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

            
            cells = new PdfPCell(new Phrase("Date Of Booking:", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["EnteredOn"].ToString(), subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
            tabular.AddCell(cells);
            cells = new PdfPCell(new Phrase(DT1.Rows[0]["DateOfArrival"].ToString(), subheadfont));
            tabular.AddCell(cells);
           

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
                Decimal IndMemberFees = Convert.ToDecimal("0.00");
                Decimal ForgMemberFees = Convert.ToDecimal("0.00");
                Decimal TotalOfTRDF = Convert.ToDecimal("0.00");
                Decimal TotalOfFee_TigerProject = Convert.ToDecimal("0.00");
                Decimal TotalOfFee_Surcharge = Convert.ToDecimal("0.00");
                Decimal TotalOfVehicle_TRDF = Convert.ToDecimal("0.00");
                Decimal TotalOfGuidFee_TRDF = Convert.ToDecimal("0.00");
                Decimal TotalCameraFee = Convert.ToDecimal("0.00");
                int indianCount = 0; int foreignerCount = 0;int RemainingCount = 0;
                int totalcount = DT2.Rows.Count;
                foreach (DataRow DR in DT2.Rows)
                {
                    TotalOfTRDF += Convert.ToDecimal(DR["TRDF"].ToString());
                    TotalOfFee_TigerProject += Convert.ToDecimal(DR["Fee_TigerProject"].ToString());
                    TotalOfFee_Surcharge += Convert.ToDecimal(DR["Fee_Surcharge"].ToString());
                    TotalOfVehicle_TRDF = Convert.ToDecimal(DR["Vehicle_TRDF"].ToString());
                    TotalOfGuidFee_TRDF = Convert.ToDecimal(DR["GuidFee_TRDF"].ToString());
                    TotalCameraFee += Convert.ToDecimal(DR["CameraFee"].ToString());

                    if(DR["Nationality"].ToString()== "Indian" )
                    {
                    IndMemberFees =(IndMemberFees==0? Convert.ToDecimal(DR["MemberFee"].ToString()): IndMemberFees);
                    indianCount++;
                    }
                    if (DR["Nationality"].ToString() == "Foreigner")
                    {
                    ForgMemberFees =(ForgMemberFees==0? Convert.ToDecimal(DR["MemberFee"].ToString()): ForgMemberFees);
                    foreignerCount++;
                    }


                    
                }
                RemainingCount = 6 - totalcount;
                if (foreignerCount > 0)
                {
                    MemberFees = (IndMemberFees * indianCount) + (ForgMemberFees * (RemainingCount + foreignerCount));
                }
                else
                {
                    MemberFees = (IndMemberFees * (indianCount + RemainingCount));
                }
                

                int NoOfSeats=Convert.ToInt16( DT2.Rows[0]["SeatPerEqment"]);
               // MemberFees =  TotalOfTRDF + TotalOfFee_TigerProject + TotalOfFee_Surcharge + TotalOfVehicle_TRDF * NoOfSeats + TotalOfGuidFee_TRDF * NoOfSeats;

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



            //cellsd = new PdfPCell(new Phrase("Half/Full Day Charges:", subheadfont));// { Border = 4};
            //SafariDetails.TotalWidth = 130;
            //SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
            //SafariDetails.AddCell(cellsd);

            //Decimal SumOFTigerAndSurchargeFee = Convert.ToDecimal(5 * Convert.ToDecimal(DT2.Rows[0]["Fees_TigerProjectHalfDayFullDayCharge"]) + Convert.ToDecimal(5 * Convert.ToDecimal(DT2.Rows[0]["Fee_SurchargeHalfDayFullDayCharge"])));
            //cellsd = new PdfPCell(new Phrase(Convert.ToString(SumOFTigerAndSurchargeFee), subheadfont));
            //cellsd.Colspan = 5;
            //cellsd.HorizontalAlignment = Element.ALIGN_LEFT;
            //SafariDetails.AddCell(cellsd);
            //End
            //decimal serviceAmount =  Convert.ToDecimal(DT1.Rows[0]["AmountTobePaid"])-Totalfess;

            cellsd = new PdfPCell(new Phrase("Service Charges(INR):", subheadfont));// { Border = 4};
            SafariDetails.TotalWidth = 130;
            SafariDetails.SetTotalWidth(new float[] { 24f, 8f, 20f, 8f, 20f, 50f });
            SafariDetails.AddCell(cellsd);
            

            cellsd = new PdfPCell(new Phrase(DT1.Rows[0]["EmitraCharges"].ToString(), subheadfont));
            //cellsd = new PdfPCell(new Phrase(serviceAmount.ToString(), subheadfont));
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
    }

	public class ReScheduleMemberDetails
	{
		public string Name { get; set; }
		public string Nationality { get; set; }
		public string IdProof { get; set; }
		public string NoofCamera { get; set; }
		public string MemberFees { get; set; }
		public string CameraFees { get; set; }
		public string VehicleFees { get; set; }
		public string BoardingVehicleFee { get; set; }
		public string BoardingGuideFeeGSTAmount { get; set; }
		public string BoardingVehicleFeeGstAmount { get; set; }
		public string BoardingGuideFee { get; set; }
		public string Amount { get; set; }

	}
	public class MailReScheduleData
	{
		public string ToMail { get; set; }
		public string MailSubject { get; set; }
		public string RequestID { get; set; }
		public string PreviousVisitDate { get; set; }
		public string PreviousShift { get; set; }
		public string PreviousZone { get; set; }
		public string NoOFMembers { get; set; }
		public string RequestedShift { get; set; }
		public string RequestedPlaceName { get; set; }
		public string FirstDate { get; set; }
		public string SecondDate { get; set; }
		public string ThirdDate { get; set; }

	}

}