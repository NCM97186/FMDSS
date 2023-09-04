using FMDSS.Models;
using FMDSS.Models.Archeology;
using FMDSS.Models.BookOnlineZoo;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace FMDSS.Controllers.Archeology
{
    public class BookArcheologyTicketController : Controller
    {
        //
        // GET: /BookArcheologyTicket/

        public ActionResult Index()
        {
            //PrintTicket(202207081025076512);
            GetDistrict();
            GetVisitorType();
            GetVisitorTypeId();
            return View();
        }

        /// <summary>
        /// This will get the archeology area
        /// </summary>
        /// <param name="districtName"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public JsonResult CreateArcheologyArea(string districtName)
        {
            DataTable dt = new DataTable();
            try
            {
                var obj = new ArcheologyFunctions();
                dt = obj.GetArcheologyAreabyDistrictName(districtName);
            }
            catch (Exception ex)
            {
                return null;
            }
            string value = JsonConvert.SerializeObject(dt);
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// This will get the list of district
        /// </summary>
        /// <returns></returns>

        public void GetDistrict()
        {
            var obj = new ArcheologyFunctions(); DataTable dt = new DataTable(); List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                dt = obj.GetDistrictMaster();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lst.Add(new SelectListItem { Text = Convert.ToString(@dr["DistrictName"]), Value = Convert.ToString(@dr["PK_Id"]) });
                }
                ViewBag.Place1 = lst;
            }
            catch (Exception ex)
            {

            }

        }


        /// <summary>
        /// This will get the list of type of visitors
        /// </summary>
        /// <returns></returns>

        public void GetVisitorType()
        {
            var obj = new ArcheologyFunctions(); DataTable dt = new DataTable(); List<SelectListItem> lst = new List<SelectListItem>();
            try
            {

                dt = obj.GetTypeVisitor();
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    lst.Add(new SelectListItem { Text = Convert.ToString(@dr["VisitorType"]), Value = Convert.ToString(@dr["PK_Id"]) });
                }
                ViewBag.VistorType = lst;
            }
            catch (Exception ex)
            {

            }

        }

        public void GetVisitorTypeId()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem { Text = "Photo PAN Card", Value = "PANCard" });
            lst.Add(new SelectListItem { Text = "Valid Passport", Value = "Passport" });
            lst.Add(new SelectListItem { Text = "Voter Id Card", Value = "VoterIdCard" });
            lst.Add(new SelectListItem { Text = "Driving License", Value = "DrivingLicense" });
            lst.Add(new SelectListItem { Text = "Aadhar Card", Value = "AadharCard" });
            lst.Add(new SelectListItem { Text = "Student Identity Card", Value = "StudentIdentityCard" });
            ViewBag.VistorTypeId = lst;
        }

        [System.Web.Http.HttpPost]
        public ActionResult FinalSubmit([FromBody] ArcheologyModel input)
        {
            var data = input;

            calculateTransactionFees(input);


            return View();
        }

        // This will calculate the fees for the archeology department.
        private void calculateTransactionFees(ArcheologyModel obj)
        {
            try
            {
                var objarch = new ArcheologyFunctions();
                char[] sperator = new char[1] { ',' };
                obj.selectedPlaces = obj.selectedPlaces.Remove(0, 1);
                var splitplaces = obj.selectedPlaces.Split(sperator);
                var PlaceWiseRate = new List<PrintingTicketOfArcheology>();

                #region Calculating the amount to be paid for booking
                int totalamount = 0;
                foreach (var item in splitplaces)
                {
                    var dt = objarch.GetRateListbyPlaceId(Convert.ToInt32(item), Convert.ToInt32(obj.VistorType));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        //RateList.Add(new RateWiseList { fees = Convert.ToInt32(@dr["fees"]) });
                        totalamount += Convert.ToInt32(@dr["fees"]) * obj.NumberofCitizen;
                        PlaceWiseRate.Add(new PrintingTicketOfArcheology { Amount = totalamount, Place = Convert.ToString(@dr["AreaName"]), Qty = obj.NumberofCitizen, Rate = Convert.ToInt32(@dr["fees"]) });
                    }

                }

                Session["PlaceWiseRate"] = PlaceWiseRate;
                //foreach (var item in RateList)
                //{
                //    totalamount += item.fees * obj.NumberofCitizen;

                //}
                #endregion

                #region Saving data into database
                obj.TotalAmount = totalamount;
                obj.ConsumerKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                obj.createdby = Convert.ToInt32(Session["UserID"]);
                var dttable = objarch.InsertArcheologyRequest(obj);

                #endregion

                #region Executing emitra payment system
                GetemitraDetails(obj.ConsumerKey);
                #endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private void GetemitraDetails(string requestId)
        {
            try
            {
                var objarch = new ArcheologyFunctions();

                var emtiradata = objarch.GetEmtiraDetailsforPayment();
                // Success and failure url are same.
                var ReturnUrl = @"http://localhost:17105/BookArcheologyTicket/PaymentResponse";// Convert.ToString(emtiradata.Rows[0]["ReturnUrl"]) + "BookOnlineTicket/Payment";
                var REVENUEHEAD = Convert.ToString(emtiradata.Rows[0]["RevenueHead"]);

                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();
                string forms = ObjEmitraPayRequest.PayRequestLive(false, requestId,
                                    Convert.ToString(emtiradata.Rows[0]["MerchantCode"]),
                                    Convert.ToString(emtiradata.Rows[0]["ChecksumKey"]),
                                    Convert.ToString(emtiradata.Rows[0]["EncryptionKey"]), ReturnUrl, ReturnUrl,
                                    Convert.ToString(emtiradata.Rows[0]["DIST_CODE"]), Convert.ToString(emtiradata.Rows[0]["ServiceID"]),
                                    Convert.ToString(emtiradata.Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());
                Response.Write(forms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PaymentResponse()
        {
            PGResponse ObjPGResponse = new PGResponse();
            try
            {
                string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";


                if (Request.Form["MERCHANTCODE"] != null)
                    MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                if (Request.Form["PRN"] != null)
                    PRN = Request.Form["PRN"].ToString();
                if (Request.Form["STATUS"] != null)
                    STATUS = Request.Form["STATUS"].ToString();
                if (Request.Form["ENCDATA"] != null)
                    ENCDATA = Request.Form["ENCDATA"].ToString();

                var objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016");
                var DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);


                #region save response in database
                var objarch = new ArcheologyFunctions();
                objarch.InsertResponseFromEmitra(ObjPGResponse);
                var filepath = PrintTicket(Convert.ToInt64(PRN));

                ObjPGResponse.FilePath = PRN;

                //if (System.IO.File.Exists(filepath))
                //{
                //    // string FilePath = Server.MapPath(filepath);
                //    WebClient User = new WebClient();
                //    Byte[] FileBuffer = User.DownloadData(filepath);
                //    if (FileBuffer != null)
                //    {
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                //        Response.BinaryWrite(FileBuffer);
                //        Response.End();
                //    }
                //}

                #endregion

            }
            catch (Exception)
            {

            }

            return View(ObjPGResponse);
        }


        public string PrintTicket(Int64 ticketid)
        {
            var objarch = new ArcheologyFunctions();
            var PlaceWiseRate = new List<PrintingTicketOfArcheology>();
            var lstTermsandcondition = new List<TermsandConditionArhceology>();
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            string filepath = null;
            try
            {
                DataSet ds = new DataSet();
                BookOnZoo cs = new BookOnZoo();
                cs.TicketID = ticketid;
                ds = objarch.GetEmtiraDataForPrintTicket(Convert.ToString(ticketid));

                #region for calculating the amount for the ticket
                DT1 = ds.Tables[2]; // for calculating the ticket amount;
                char[] sperator = new char[1] { ',' };
                var dtterms = new DataTable();
                var splitplaces = Convert.ToString(DT1.Rows[0]["SelectedPlaces"]).Split(sperator);
                var totalamount = 0;
                foreach (var item in splitplaces)
                {
                    // getting rate list
                    var dt = objarch.GetRateListbyPlaceId(Convert.ToInt32(item), Convert.ToInt32(DT1.Rows[0]["VisitorType"]));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        totalamount += Convert.ToInt32(@dr["fees"]) * Convert.ToInt32(DT1.Rows[0]["NumberOfCitizen"]);
                        PlaceWiseRate.Add(new PrintingTicketOfArcheology { Amount = totalamount, Place = Convert.ToString(@dr["AreaName"]), Qty = Convert.ToInt32(DT1.Rows[0]["NumberOfCitizen"]), Rate = Convert.ToInt32(@dr["fees"]) });
                    }

                    // getting terms and condition data
                    dtterms = objarch.GetTermsandCondition(Convert.ToInt32(item));
                    foreach (System.Data.DataRow dr in dtterms.Rows)
                    {
                        lstTermsandcondition.Add(new TermsandConditionArhceology { Place = Convert.ToString(@dr["Place"]), TermandCondtionText = Convert.ToString(@dr["TermsandCondition"]) });
                    }
                    // here we need to merge the rows.


                }

                filepath = NP_GenerateTicket_New(ds, PlaceWiseRate, lstTermsandcondition);

                #endregion

            }
            catch (Exception ex)
            {
                //  new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return filepath;
        }




        public static string NP_GenerateTicket_New(DataSet DS, List<PrintingTicketOfArcheology> lst, List<TermsandConditionArhceology> lsttermsandcondition)
        {
            if (DS.Tables.Count > 2)
            {
                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                try
                {
                    DataTable DT1 = DS.Tables[0];
                    DataTable DT2 = DS.Tables[1];

                    string filepath = string.Empty;


                    filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_Ticket_" + Convert.ToString(DT1.Rows[0]["PRN"]) + ".pdf");

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
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["DistrictName"]), subheadfont)); // district name 
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase("Booking No:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["PRN"]), subheadfont)); //Booking Number
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Date/Time & Booking:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["CreatedOn"]).ToString("dd-MMM-yyyy hh:mm:ss tt"), subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase("Date of Visit", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToDateTime(DT1.Rows[0]["DateofVisit"]).ToString("dd-MMM-yyyy"), subheadfont));
                    tabular.AddCell(cells);

                    cells = new PdfPCell(new Phrase("Booked Seats:", subheadfont));
                    tabular.AddCell(cells);
                    cells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["NumberOfCitizen"]), subheadfont));
                    tabular.AddCell(cells);

                    doc.Add(tabular);
                    doc.Add(new Paragraph(Environment.NewLine));
                    doc.Add(table);
                    doc.Add(new Paragraph(Environment.NewLine));
                    //Visitor Details.

                    PdfPTable VisitorDetails = new PdfPTable(5) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsv = new PdfPCell();// { Border = 4};
                    VisitorDetails.TotalWidth = 180;


                    cellsv = new PdfPCell(new Phrase("#", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Place", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Rate(INR)", tableHeaderFont));
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("QTY", tableHeaderFont)); //
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);
                    cellsv = new PdfPCell(new Phrase("Amount", tableHeaderFont)); //
                    cellsv.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    VisitorDetails.AddCell(cellsv);


                    Int16 j = 0;
                    foreach (var item in lst)
                    {
                        j += Convert.ToInt16(1);
                        cellsv = new PdfPCell(new Phrase(j.ToString(), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(item.Place), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(item.Rate), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(item.Qty), subheadfont));
                        VisitorDetails.AddCell(cellsv);

                        cellsv = new PdfPCell(new Phrase(Convert.ToString(item.Amount), subheadfont));
                        VisitorDetails.AddCell(cellsv);
                    }
                    doc.Add(VisitorDetails);
                    doc.Add(new Paragraph(Environment.NewLine));
                    #region AddingVisitorDetails
                    PdfPTable VisitorInfo = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellsinfo = new PdfPCell(new Phrase("Visitor Personal Information", subheadfont));// { Border = 4};
                                                                                                               //tabular.TotalWidth = 320;
                    VisitorInfo.SetTotalWidth(new float[] { 60f, 100f, 60f, 100f });
                    cellsinfo.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    cellsinfo.Colspan = 4;
                    cellsinfo.HorizontalAlignment = Element.ALIGN_CENTER;
                    VisitorInfo.AddCell(cellsinfo);

                    cellsinfo = new PdfPCell(new Phrase("Visitor Name:", subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VistorName"]), subheadfont)); // district name 
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase("Visitor Email:", subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VisitorEmail"]), subheadfont)); //Booking Number
                    VisitorInfo.AddCell(cellsinfo);

                    cellsinfo = new PdfPCell(new Phrase("Visitor Mobile:", subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VisitorMobile"]), subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase("Visitor Id Type:", subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VisitorIdType"]), subheadfont));
                    VisitorInfo.AddCell(cellsinfo);

                    cellsinfo = new PdfPCell(new Phrase("Visitor Id Number:", subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VisitorIdNumber"]), subheadfont));
                    VisitorInfo.AddCell(cellsinfo);

                    cellsinfo = new PdfPCell(new Phrase("", subheadfont));
                    VisitorInfo.AddCell(cellsinfo);
                    cellsinfo = new PdfPCell(new Phrase(Convert.ToString(""), subheadfont));
                    VisitorInfo.AddCell(cellsinfo);

                    doc.Add(VisitorInfo);
                    doc.Add(new Paragraph(Environment.NewLine));
                    //doc.Add(table);

                    #endregion

                    #region Terms and Condition

                    PdfPTable TermsCondition = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                    PdfPCell cellst = new PdfPCell(new Phrase("Terms and conditions for Visitors :", subheadfont));// { Border = 4};
                    cellst.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                    TermsCondition.TotalWidth = 430;
                    TermsCondition.SetTotalWidth(new float[] { 30f, 400f });
                    cellst.Colspan = 2;
                    cellst.HorizontalAlignment = Element.ALIGN_LEFT;
                    TermsCondition.AddCell(cellst);

                    int index = 1;
                    foreach (var item in lsttermsandcondition)
                    {
                        string sTC = Convert.ToString(item.TermandCondtionText + "," + item.Place);
                        cellst = new PdfPCell(new Phrase(index + ".", subheadfont));
                        TermsCondition.AddCell(cellst);

                        cellst = new PdfPCell(new Phrase(sTC, subheadfont));
                        TermsCondition.AddCell(cellst);
                        index += 1;

                    }

                    doc.Add(TermsCondition);

                    #endregion




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

        //[System.Web.Http.HttpGet]
        public FileResult GetTicketForPrint(string PRNNumber)
        {
            var filepath = System.Web.HttpContext.Current.Server.MapPath("~/DownloadTickets/NP_Ticket_" + PRNNumber + ".pdf");
            if (System.IO.File.Exists(filepath))
            {
                // string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(filepath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
            return null;
        }


    }
}
