using FMDSS.E_SignIntegration;
using FMDSS.Entity;
using FMDSS.Entity.DOD.ViewModel;
using FMDSS.Globals;
using FMDSS.Repository.Interface;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.Repository
{
    public class AuctionRequestRepository : IAuctionRequestRepository
    {
        #region Properties & Variables
        private FMDSS.Models.DAL _db = new Models.DAL();
        #endregion

        #region [Customer Part]
        public List<AuctionTransaction> GetAuctionDetailsForCustomer()
        {
            List<AuctionTransaction> data = null;

            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            DataTable dtAuction = new DataTable();
            _db.Fill(dtAuction, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataTable(dtAuction, true))
            {
                data = Util.GetListFromTable<AuctionTransaction>(dtAuction);
            }
            return data;
        }

        public AuctionVM GetNoticeDetails(long noticeID)
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("NoticeID", noticeID)};
            DataSet dsAuction = new DataSet();
            _db.Fill(dsAuction, "SP_DOD_GetDetailsByNoticeID", prms);

            AuctionVM data = null;

            if (Util.isValidDataSet(dsAuction, 0, true))
            {
                data = Util.GetListFromTable<AuctionVM>(dsAuction, 0).FirstOrDefault();
                if (Util.isValidDataSet(dsAuction, 1, true))
                {
                    data.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(dsAuction, 1);
                }
            }
            return data;
        }

        public List<AuctionVM> GetNoticeDetailsForAuction()
        {
            var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]) };
            DataTable dtAuction = new DataTable();
            _db.Fill(dtAuction, "SP_DOD_GetDetailsByNoticeID", prms);

            List<AuctionVM> data = null;

            if (Util.isValidDataTable(dtAuction, true))
            {
                data = Util.GetListFromTable<AuctionVM>(dtAuction);
            }
            return data;
        }

        public DataSet SaveAuctionDetails(AuctionRequest model)
        {
            var requestType = model.RequestedId.Split('/')[0];
            var actionCode = string.Empty;
            switch (requestType)
            {
                case "Auc": actionCode = "1"; break;
                case "AucPendingAmt": actionCode = "2"; break;
            }

            AuctionVM data = (AuctionVM)HttpContext.Current.Session["AuctionRequest"];

            DataSet dsAuction = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("GSTINNumber",data.GSTINNumber),
                            new SqlParameter("ID",model.RequestedId.Split('/')[1]),
                            new SqlParameter("RequestedId", model.RequestedId),
                            new SqlParameter("ParentID", model.ParentID),
                            new SqlParameter("Trn_Status_Code", model.Trn_Status_Code),
                            new SqlParameter("EmitraTransactionID", model.EmitraTransactionID),
                            new SqlParameter("PayableAmount", model.PayableAmount),
                            new SqlParameter("EmitraAmount", model.EmitraAmount),
                            new SqlParameter("Comments", model.Comments)
            };

            _db.Fill(dsAuction, "SP_DOD_Insert_AuctionDetail", prms);
            return dsAuction;
        }

        public DataSet SaveAuctionDetailsWithoutPayment(AuctionVM model)
        {
            DataSet dsAuction = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("GSTINNumber",model.GSTINNumber),
                            new SqlParameter("ID",model.RequestedId.Split('/')[1]),
                            new SqlParameter("RequestedId", model.RequestedId),
                            new SqlParameter("Trn_Status_Code", 1) 
            };

            _db.Fill(dsAuction, "SP_DOD_Insert_AuctionDetail", prms);
            return dsAuction;
        }

        public ResponseMsg ValidateUser(long noticeID)
        {
            DataTable dtAuction = new DataTable();
            ResponseMsg msg = null;

            var prms = new[]{
                            new SqlParameter("ActionCode", 3),
                            new SqlParameter("NoticeID", noticeID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtAuction, "SP_DOD_GetDetailsByNoticeID", prms);

            if (dtAuction != null)
            {
                if (dtAuction.Rows.Count > 0)
                {
                    msg = dtAuction.AsEnumerable().Select(x => new ResponseMsg
                    {
                        IsError = x.Field<bool>("IsError"),
                        ReturnMsg = x.Field<string>("ReturnMsg")
                    }).FirstOrDefault();
                }
            }

            return msg;
        }
        #endregion

        #region [Admin Part]
        public List<AuctionTransaction> GetAuctionDetails()
        {
            List<AuctionTransaction> data = null;

            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            DataTable dtAuction = new DataTable();
            _db.Fill(dtAuction, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataTable(dtAuction, true))
            {
                data = Util.GetListFromTable<AuctionTransaction>(dtAuction);
            }
            return data;
        }

        public List<AuctionClearanceDetails> GetAuctionDetailsForClearance()
        {
            List<AuctionClearanceDetails> data = null;

            var prms = new[]{
                            new SqlParameter("ActionCode", 3),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            DataTable dtAuction = new DataTable();
            _db.Fill(dtAuction, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataTable(dtAuction, true))
            {
                data = Util.GetListFromTable<AuctionClearanceDetails>(dtAuction);
            }
            return data;
        }

        public DataTable GetDetailsByInventory(string parentID, string childID)
        {
            DataTable dtData = new DataTable();
            try
            {
                var prms = new[]{
                            new SqlParameter("ActionCode", 2),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("ChildID", childID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
                _db.Fill(dtData, "SP_DOD_GetDetailsByInventory", prms);
            }
            catch (Exception) { }
            return dtData;
        }

        public AuctionVM GetNoticeDetails(Int64 auctionID, string actionCode)
        {
            AuctionVM data = null;
            try
            {
                var prms = new[]{
                            new SqlParameter("ParentID", auctionID),
                            new SqlParameter("ActionCode", actionCode)};
                DataSet dsData = new DataSet();
                _db.Fill(dsData, "SP_DOD_GetDetailsByNoticeID", prms);

                if (Util.isValidDataSet(dsData, 0, true))
                {
                    data = Util.GetListFromTable<AuctionVM>(dsData, 0).FirstOrDefault();

                    if (Util.isValidDataSet(dsData, 1, true))
                    {
                        data.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(dsData, 1);

                        if (Util.isValidDataSet(dsData, 2, true))
                        {
                            data.EmitraHeadList = Util.GetListFromTable<DropDownList2>(dsData, 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public ResponseMsg SaveAuctionClearance(AuctionClearanceVM model)
        {
            ResponseMsg msg = null;
            DataSet dsAuction = new DataSet();
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("InventoryID", model.InventoryID),
                            new SqlParameter("WinnerAuctionID", model.AuctionID), 
                            new SqlParameter("FatherName", model.FatherName),
                            new SqlParameter("DestinationAddress", model.DestinationAddress),
                            new SqlParameter("ClearanceFromDate", Util.GetDate(model.ClearanceFromDate)),
                            new SqlParameter("ClearanceToDate", Util.GetDate(model.ClearanceToDate)),
                            new SqlParameter("ModeofTransport", model.ModeofTransport),
                            new SqlParameter("VehicleNumber", model.VehicleNumber),
                            new SqlParameter("Driver_License_No", model.Driver_License_No),
                            new SqlParameter("Driver_Name", model.Driver_Name),
                            new SqlParameter("Driver_MobNo", model.Driver_MobNo),
                            new SqlParameter("Remarks", model.Remarks),
                            new SqlParameter("xmlFile", new FMDSS.Models.ForestDevelopment.TransitPermit().GetRequestInXML(model)),
            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])
            };

            _db.Fill(dsAuction, "SP_DOD_Insert_AuctionClearance", prms);

            if (Util.isValidDataSet(dsAuction, 0, true))
            {
                msg = dsAuction.Tables[0].AsEnumerable().Select(x => new ResponseMsg
                {
                    IsError = x.Field<bool>("IsError"),
                    ReturnMsg = x.Field<string>("ReturnMsg")
                }).FirstOrDefault();
            }

            return msg;
        }

        public DataSet GetNoticeDataForAuctionClearance()
        {
            DataSet dsData = new DataSet();

            try
            {
                var prms = new[]{
                    new SqlParameter("ActionCode", 8),
                    new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
                _db.Fill(dsData, "SP_DOD_GetDropdownData", prms);
            }
            catch (Exception ex)
            {
            }
            return dsData;
        }

        public DataTable GetAuctionNoticeListForAuctionClearance(string inventoryID)
        {
            DataTable dtData = new DataTable();

            try
            {
                var prms = new[]{
                    new SqlParameter("ActionCode", 9),
                    new SqlParameter("ChildID", inventoryID),
                    new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
                _db.Fill(dtData, "SP_DOD_GetDropdownData", prms);
            }
            catch (Exception ex) { }
            return dtData;
        }

        public List<AuctionPaymentDetails> GetPaymentDetails(Int64 auctionID, string actionCode)
        {
            var data = new List<AuctionPaymentDetails>();
            try
            {
                var prms = new[]{
                            new SqlParameter("ParentID", auctionID),
                            new SqlParameter("ActionCode", actionCode)};
                DataSet dsData = new DataSet();
                _db.Fill(dsData, "SP_DOD_GetDetailsByNoticeID", prms);

                if (Util.isValidDataSet(dsData, 0, true))
                {
                    data = Util.GetListFromTable<AuctionPaymentDetails>(dsData, 0);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }

        public ResponseMsg UpdateAuctionWinner(AuctionVM model)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("AuctionID", model.AuctionID),
                            new SqlParameter("PaymentMode", model.PaymentMode),
                            new SqlParameter("BankName", model.BankName),
                            new SqlParameter("IFSCCode", model.IFSCCode),
                            new SqlParameter("ChequeNumber", model.ChequeNumber),
                            new SqlParameter("ChequeDate", Util.GetDate(model.ChequeDate)),
                            new SqlParameter("BiddingAmount", model.BiddingAmount),
                            new SqlParameter("PayAmount", model.PayAmount),
                            new SqlParameter("xmlFile", GetRequestInXML(model,"AuctionWinner")),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_DOD_Insert_Notice", prms);

            if (Globals.Util.isValidDataTable(dtData, true))
            {
                msg = Globals.Util.GetListFromTable<ResponseMsg>(dtData).FirstOrDefault();
            }

            return msg;
        }

        public ResponseMsg UpdateAuctionWinnerDept(AuctionVM model)
        {
            var parentID = string.Join(",", model.DODProductList.Where(x => x.IsSelected == true).Select(x => x.AuctionWinnerID));
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            var prms = new[]{
                            new SqlParameter("ActionCode", 3),
                            new SqlParameter("AuctionID", model.AuctionID),
                            new SqlParameter("ParentID", parentID),
                            new SqlParameter("PaymentMode", model.PaymentMode),
                            new SqlParameter("BankName", model.BankName),
                            new SqlParameter("IFSCCode", model.IFSCCode),
                            new SqlParameter("ChequeNumber", model.ChequeNumber),
                            new SqlParameter("ChequeDate", Util.GetDate(model.ChequeDate)),
                            new SqlParameter("BiddingAmount", model.BiddingAmount),
                            new SqlParameter("PayAmount", model.PayAmount), 
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_DOD_Insert_Notice", prms);

            if (Globals.Util.isValidDataTable(dtData, true))
            {
                msg = Globals.Util.GetListFromTable<ResponseMsg>(dtData).FirstOrDefault();
            }

            return msg;
        }
        #endregion

        #region PDF Operation
        public ResponseMsg GenerateReport(string reqID, string reportType, string rootPath, clsVerifyOTPResponce otpResponse)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;
            if (otpResponse.Status == "0")
            {
                msg.ReturnMsg = otpResponse.ErrorMessage;
            }
            else
            {
                string docName = string.Empty;
                int documentTypeID = 0;

                switch (reportType)
                {
                    case "Clearance":
                        documentTypeID = 41;
                        msg = GenerateClearanceReport(rootPath, reqID, documentTypeID, otpResponse.TransactionId, ref docName); break;
                    case "ProduceWise":
                        documentTypeID = 42;
                        msg = GenerateProduceWiseReport(rootPath, reqID, documentTypeID, otpResponse.TransactionId, ref docName); break;
                    case "ProductWise":
                        documentTypeID = 43;
                        msg = GenerateProductWiseReport(rootPath, reqID, documentTypeID, otpResponse.TransactionId, ref docName); break;
                    case "WinnerWise":
                        documentTypeID = 44;
                        msg = GenerateWinnerWiseReport(rootPath, reqID, documentTypeID, otpResponse.TransactionId, ref docName); break;
                    case "AuctionRegister":
                        documentTypeID = 45;
                        msg = GenerateAuctionRegisterReport(rootPath, reqID, documentTypeID, otpResponse.TransactionId, ref docName); break;
                }
                if (!msg.IsError)
                {
                    var prms2 = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("ObjectID", reqID),
                            new SqlParameter("ObjectTypeID", 3),
                            new SqlParameter("DocumentTypeID", documentTypeID),
                            new SqlParameter("DocumentName", docName),
                            new SqlParameter("DocumentPath", Util.GetAppSettings("FRADocumentESignPDF") + docName),
                            new SqlParameter("IsESign", true),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]) };
                    DataSet data2 = new DataSet();
                    _db.Fill(data2, "SP_Common_Document_Save", prms2);
                    if (Globals.Util.isValidDataSet(data2, 0, true))
                    {
                        msg = Globals.Util.GetListFromTable<ResponseMsg>(data2.Tables[0]).FirstOrDefault();
                    }
                }
            }
            return msg;
        }

        private ResponseMsg GenerateClearanceReport(string rootPath, string requestID, int documentTypeID, string transactionID, ref string docName)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;

            var prms = new[]{
                            new SqlParameter("ActionCode", 4),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("ParentID", requestID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataSet(data, 0))
            {
                var cd = Util.GetListFromTable<AuctionClearanceDetails>(data, 0).FirstOrDefault();
                var lotNumber = "N/A";

                if (Util.isValidDataSet(data, 1, true))
                {
                    cd.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(data, 1);
                    lotNumber = cd.DODProductList.FirstOrDefault().DisplayLotNumber;
                }

                string filepath = string.Empty;
                string EsignPath = string.Empty;
                docName = string.Format("3_{0}_{1}_ClearanceReport.pdf", requestID, documentTypeID);
                filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
                EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

                Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
                BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 12);
                iTextSharp.text.Font hindismall = new iTextSharp.text.Font(dev, 10);
                iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);

                var Myfont = FontFactory.GetFont("Times New Roman", 16, FontColour);
                var fontTitle = FontFactory.GetFont("Times New Roman", 14, FontColour);
                var subheadfont = FontFactory.GetFont("Times New Roman", 12, FontColour);
                var smallfont = FontFactory.GetFont("Times New Roman", 10, FontColour);
                var underlineFont = FontFactory.GetFont("Times New Roman", 6, Font.UNDERLINE, FontColour);

                doc.Open();
                doc.NewPage();
                // doc.Add(new Paragraph(Environment.NewLine)); 
                #region D5

                #region header

                PdfPTable Header = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                Header.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cells = new PdfPCell() { Border = 4 };
                Header.TotalWidth = 120;
                Header.SetTotalWidth(new float[] { 20f, 0f, 0f, 0f });


                cells = new PdfPCell(new Phrase("jktLFkku ljdkj", hindibold)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("Mh -5", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("ou foHkkx", hindismall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jktdh; O;kikj  ;kstuk", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jo= fudklh ou mit", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);



                Phrase phraseL1 = new Phrase();
                phraseL1.Add(new Chunk("cqd u", hindi));
                //phraseL1.Add(new Chunk(" " + "&&&&&&&&&&&&&&&&&&&&&&&&&&" + " ", hindi));  
                phraseL1.Add(new Chunk(" " + cd.BookNumber + " ", smallfont));
                phraseL1.Add(new Chunk("dek±d", hindi));
                phraseL1.Add(new Chunk(" " + cd.ClearanceDetailsID + " ", smallfont));
                phraseL1.Add(new Chunk("Mhiks", hindi));
                phraseL1.Add(new Chunk(" " + cd.Depot_Name + " ", smallfont));
                phraseL1.Add(new Chunk("ls", hindi));
                phraseL1.Add(new Chunk(" " + cd.DestinationAddress + " ", smallfont));
                phraseL1.Add(new Chunk("ykV  u", hindi));
                phraseL1.Add(new Chunk(" " + lotNumber + " ", smallfont));
                phraseL1.Add(new Chunk("uke  [kjhnkj ", hindi));
                phraseL1.Add(new Chunk(" " + cd.WinnerName + " ", smallfont));
                phraseL1.Add(new Chunk("firk dk uke", hindi));
                phraseL1.Add(new Chunk(" " + cd.FatherName + " ", smallfont));
                phraseL1.Add(new Chunk("rkjh[k", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceFromDate, "dd/MM/yyyy") + " ", smallfont));
                phraseL1.Add(new Chunk("le;", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceFromDate, "HH:mm") + " ", smallfont));
                phraseL1.Add(new Chunk("çkr lk;  vkofèk ", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceToDate, "dd/MM/yyyy") + " ", smallfont));
                phraseL1.Add(new Chunk("ls", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceToDate, "HH:mm") + " ", smallfont));
                phraseL1.Add(new Chunk("rd", hindi));
                Header.AddCell(phraseL1);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);

                doc.Add(Header);

                #endregion

                #region body
                PdfPTable Details = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                //Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellud = new PdfPCell() { Border = 4 };
                Details.TotalWidth = 120;
                Details.SetTotalWidth(new float[] { 30f, 30f, 30f, 30f });

                cellud = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 }; //gram ka naam
                cellud.Colspan = 4;
                cellud.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("Ø l", hindiboldsmall));//{ Border = 0 }; //gram ka naam
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("fooj.k eky", hindiboldsmall));//{ Border = 0 };//khasra number
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("ux@otu@ uki ?ku ehVj", hindiboldsmall));// { Border = 0 };//khasra number ka kul shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("okgu fooj.k", hindiboldsmall));// { Border = 0 };// van bhoomi ka shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                foreach (var item in cd.DODProductList)
                {
                    cellud = new PdfPCell(new Phrase(item.SNo, smallfont));//{ Border = 0 };//krishi&aawas
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.ProductName, smallfont));//{ Border = 0 }; visheshvivran
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(Convert.ToString(item.Qty), smallfont));// { Border = 0 };
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(cd.VehicleDescription, smallfont));// { Border = 0 };yog
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);
                }

                doc.Add(Details);
                #endregion

                #region footer
                PdfPTable FooterDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellfd = new PdfPCell() { Border = 4 };
                FooterDetails.TotalWidth = 120;
                FooterDetails.SetTotalWidth(new float[] { 20f, 75f, 20f });

                cellfd = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cellfd.Colspan = 3;
                cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("gLrk{kj fudkldjrk", hindiboldsmall)) { Border = 0 };//sign patwari
                cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("psd ukdk", hindiboldsmall)) { Border = 0 }; //gram
                cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("gLrk{kj Mhiks lqijokÃtj", hindiboldsmall)) { Border = 0 };// gram
                cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterDetails.AddCell(cellfd);

                doc.Add(FooterDetails);
                #endregion

                #endregion

                doc.Close();

                #region Generate Esign
                try
                {
                    clsDocumentESign requestPdf = new clsDocumentESign();
                    byte[] bytes = File.ReadAllBytes(filepath);
                    requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                    requestPdf.transactionid = transactionID;
                    clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, requestID);

                    if (response.Status != "0")
                    {
                        using (FileStream stream = System.IO.File.Create(EsignPath))
                        {
                            byte[] byteArray = Convert.FromBase64String(response.Document);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        msg.IsError = false;
                        msg.ReturnMsg = "Report generated successfully.";
                    }
                    else
                    {
                        msg.ReturnMsg = "Report generated without e-sign.";
                    }
                }
                catch (Exception) { msg.ReturnMsg = "Some error has occurred please try again later!!!"; }
                #endregion
            }

            return msg;
        }

        private ResponseMsg GenerateProduceWiseReport(string rootPath, string requestID, int documentTypeID, string transactionID, ref string docName)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;

            var prms = new[]{
                            new SqlParameter("ActionCode", 5),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("ParentID", requestID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataSet(data, 0))
            {
                var cd = Util.GetListFromTable<AuctionClearanceDetails>(data, 0).FirstOrDefault();
                var lotNumber = "N/A";

                if (Util.isValidDataSet(data, 1, true))
                {
                    cd.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(data, 1);
                    lotNumber = cd.DODProductList.FirstOrDefault().DisplayLotNumber;
                }

                string filepath = string.Empty;
                string EsignPath = string.Empty;
                docName = string.Format("3_{0}_{1}_ProduceWiseReport.pdf", requestID, documentTypeID);
                filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
                EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

                Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
                BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 8);
                iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);

                var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
                var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
                var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                var smallfont = FontFactory.GetFont("Arial", 6, FontColour);
                var underlineFont = FontFactory.GetFont("Arial", 6, Font.UNDERLINE, FontColour);

                doc.Open();
                doc.NewPage();
                // doc.Add(new Paragraph(Environment.NewLine)); 
                #region D1
                #region header1 d1

                PdfPTable Header = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                Header.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cells = new PdfPCell() { Border = 4 };
                Header.TotalWidth = 120;
                Header.SetTotalWidth(new float[] { 20f, 0f, 0f, 0f });



                cells = new PdfPCell(new Phrase("  ou foHkkx jktLFkku", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" Mh -1", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jktdh; O;kikj  ;kstuk ", hindibold)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("çkjfHkd çkfIr eky iaftdk ", hindi)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                Phrase phraseL1 = new Phrase();

                phraseL1.Add(new Chunk("foØ; dsaæ  ", hindi));
                phraseL1.Add(new Chunk(" " + cd.Depot_Name + " ", subheadfont));   //विक्रय केंद्र
                Header.AddCell(phraseL1);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);
                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe

                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);
                doc.Add(Header);


                #endregion
                #region body
                PdfPTable Details = new PdfPTable(21) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                //Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellud = new PdfPCell() { Border = 21 };
                Details.TotalWidth = 120;
                Details.SetTotalWidth(new float[] { 20f, 20f, 40f, 40f, 40f, 40f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f });

                //Details.SetTotalWidth(new float[] { 20f, 20f, 40f, 40f, 20f, 20f });

                cellud = new PdfPCell(new Phrase("fnukd", hindi));//{ Border = 0 }; //gram ka naam
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("dek±d ", hindi));//{ Border = 0 };//khasra number
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" eky çkIr gksus dk fooj.k", hindi));// { Border = 0 };//khasra number ka kul shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 4;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("fooj.k  Nhtr  vkfn dk otu  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("fMiks ij rqyus ij otu ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("rkjh[k lQkÃ o rqykÃ eky  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase("Mhiks  ij Nukb djus ij çkIr otu  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 6;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase("vèktyh ydM+h", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("dqy otu", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("otu esa deh", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("deh otu dk iÆr'kr ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("fooj.k", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("deh otu ds [kkfjth vkns'k ds u o rkjh[k ", hindi));// { Border = 0 };yog//कमी वजन के खारिजी आदेश के न व तारीख 
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(Util.GetDateWithFormat(DateTime.Now.ToString(), "dd/MM/yyyy"), subheadfont));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(Convert.ToString(cd.ClearanceDetailsID), subheadfont));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" dgk ls vk;k", hindi));// { Border = 0 };yog// कहा से आया
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("jsyos jlhn u  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" rd iVVh çV la[; k", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("pkyku esa vafdr otu", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("dks;yk", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("pqjh", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("ehëh", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase(" rknkn cksjh ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" otu ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("rknkn cksjh  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" otu  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("rknkn cksjh    ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" otu  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);




                cellud = new PdfPCell(new Phrase(" 1", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("2 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("3 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 4", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);




                cellud = new PdfPCell(new Phrase(" 5", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("6 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("7 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 8", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase(" 9", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("10 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("11 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 12", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" 13", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("14 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("15", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 16", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("17", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 18", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" 19", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("20", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("21", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                doc.Add(Details);
                #endregion
                #endregion

                doc.Close();

                #region Generate Esign
                try
                {
                    clsDocumentESign requestPdf = new clsDocumentESign();
                    byte[] bytes = File.ReadAllBytes(filepath);
                    requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                    requestPdf.transactionid = transactionID;
                    clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, requestID);

                    if (response.Status != "0")
                    {
                        using (FileStream stream = System.IO.File.Create(EsignPath))
                        {
                            byte[] byteArray = Convert.FromBase64String(response.Document);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        msg.IsError = false;
                        msg.ReturnMsg = "Report generated successfully.";
                    }
                    else
                    {
                        msg.ReturnMsg = "Report generated without e-sign.";
                    }
                }
                catch (Exception) { msg.ReturnMsg = "Some error has occurred please try again later!!!"; }
                #endregion
            }

            return msg;
        }

        private ResponseMsg GenerateProductWiseReport(string rootPath, string requestID, int documentTypeID, string transactionID, ref string docName)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;

            var prms = new[]{
                            new SqlParameter("ActionCode", 6),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("ParentID", requestID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataSet(data, 0))
            {
                var cd = Util.GetListFromTable<AuctionClearanceDetails>(data, 0).FirstOrDefault();
                var lotNumber = "N/A";

                if (Util.isValidDataSet(data, 1, true))
                {
                    cd.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(data, 1);
                    lotNumber = cd.DODProductList.FirstOrDefault().DisplayLotNumber;
                }

                string filepath = string.Empty;
                string EsignPath = string.Empty;
                docName = string.Format("3_{0}_{1}_ProductWiseReport.pdf", requestID, documentTypeID);
                filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
                EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

                Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
                BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 8);
                iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);

                var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
                var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
                var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                var smallfont = FontFactory.GetFont("Arial", 6, FontColour);
                var underlineFont = FontFactory.GetFont("Arial", 6, Font.UNDERLINE, FontColour);

                doc.Open();
                doc.NewPage();
                // doc.Add(new Paragraph(Environment.NewLine)); 

                #region D2
                #region D2 Header
                PdfPTable Header = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                Header.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cells = new PdfPCell() { Border = 4 };
                Header.TotalWidth = 120;
                Header.SetTotalWidth(new float[] { 20f, 0f, 0f, 0f });



                cells = new PdfPCell(new Phrase("ou foHkkx jktLFkku  ", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("Mh - 2 ", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jktdh; O;kikj  ;kstuk ", hindiboldsmall)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("    pêk iaftdk", hindibold)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                Phrase phraseL1 = new Phrase();

                phraseL1.Add(new Chunk("fdLe eky  ", hindi));
                phraseL1.Add(new Chunk(" " + " &&&&&&&&&&&&&&&&&&&&&& " + " ", hindi));
                phraseL1.Add(new Chunk(" " + " &&&&&&&&&&&&&&&&&&&&&& " + " ", hindi));
                phraseL1.Add(new Chunk("{ks.kh   ", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));

                phraseL1.Add(new Chunk(" pêk ", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));

                phraseL1.Add(new Chunk(" pêk uEcj", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));

                phraseL1.Add(new Chunk("lkbt+", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));


                phraseL1.Add(new Chunk("otu", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));

                phraseL1.Add(new Chunk("uhykeh fnukd", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));

                phraseL1.Add(new Chunk("Øsrk dk uke", hindi));
                phraseL1.Add(new Chunk("  " + "&&&&&&&&&&&&&&&&&&&&&&" + "", hindi));
                phraseL1.Add(new Chunk("  " + "" + "", hindi));
                phraseL1.Add(new Chunk("  " + "" + "", hindi));
                Header.AddCell(phraseL1);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe

                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);
                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe

                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);
                doc.Add(Header);

                #endregion d2

                #region body
                PdfPTable Details = new PdfPTable(12) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellud = new PdfPCell() { Border = 12 };
                Details.TotalWidth = 320;
                Details.SetTotalWidth(new float[] { 20f, 40f, 20f, 40f, 40f, 40f, 20f, 40f, 20f, 20f, 20f, 20f });

                //Details.SetTotalWidth(new float[] { 20f, 20f, 40f, 40f, 20f, 20f });

                cellud = new PdfPCell(new Phrase("frFkh çkfIr o fuxZe", hindi));//{ Border = 0 }; //gram ka naam
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("dgk ls çkIr gqÃ ;k fdldks nh x;h ", hindi));//{ Border = 0 };//khasra number
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" jlhn@ pkyku jo=@fcy la[;k", hindi));// { Border = 0 };//khasra number ka kul shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("çkIr ek=k ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("fuxZe  ;k foØ; ek=k", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("çfr çkfIr ;k fuxZe ds ckn cdk;k lkeku  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase("gLrk{kj lqij okÃt ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase("okfLrd lR;kiu", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                cellud.Colspan = 2;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase("  cksjh  Hkkj", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("otu @ux  ?ku  ehVj  ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("cksjh  Hkkj", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("otu @ux  ?ku  ehVj ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" cksjh  Hkkj", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("otu @ux  ?ku  ehVj ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ifj.kke", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("tk¡p vfèkdkjh ds g e; rkjh[k", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" 1", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("2 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("3 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 4", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);




                cellud = new PdfPCell(new Phrase(" 5", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("6 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("7 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 8", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);



                cellud = new PdfPCell(new Phrase(" 9", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("10 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("11 ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" 12", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);


                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);
                cellud = new PdfPCell(new Phrase(" ", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("", hindi));// { Border = 0 };yog
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                doc.Add(Details);
                #endregion
                #endregion

                doc.Close();

                #region Generate Esign
                try
                {
                    clsDocumentESign requestPdf = new clsDocumentESign();
                    byte[] bytes = File.ReadAllBytes(filepath);
                    requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                    requestPdf.transactionid = transactionID;
                    clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, requestID);

                    if (response.Status != "0")
                    {
                        using (FileStream stream = System.IO.File.Create(EsignPath))
                        {
                            byte[] byteArray = Convert.FromBase64String(response.Document);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        msg.IsError = false;
                        msg.ReturnMsg = "Report generated successfully.";
                    }
                    else
                    {
                        msg.ReturnMsg = "Report generated without e-sign.";
                    }
                }
                catch (Exception) { msg.ReturnMsg = "Some error has occurred please try again later!!!"; }
                #endregion
            }

            return msg;
        }

        private ResponseMsg GenerateWinnerWiseReport(string rootPath, string requestID, int documentTypeID, string transactionID, ref string docName)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;

            var prms = new[]{
                            new SqlParameter("ActionCode", 4),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("ParentID", requestID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataSet(data, 0))
            {
                var cd = Util.GetListFromTable<AuctionClearanceDetails>(data, 0).FirstOrDefault();
                var lotNumber = "N/A";

                if (Util.isValidDataSet(data, 1, true))
                {
                    cd.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(data, 1);
                    lotNumber = cd.DODProductList.FirstOrDefault().DisplayLotNumber;
                }

                string filepath = string.Empty;
                string EsignPath = string.Empty;
                docName = string.Format("3_{0}_{1}_WinnerWiseReport.pdf", requestID, documentTypeID);
                filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
                EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

                Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
                BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 8);
                iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);

                var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
                var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
                var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                var smallfont = FontFactory.GetFont("Arial", 6, FontColour);
                var underlineFont = FontFactory.GetFont("Arial", 6, Font.UNDERLINE, FontColour);

                doc.Open();
                doc.NewPage();
                // doc.Add(new Paragraph(Environment.NewLine)); 
                #region D5

                #region header

                PdfPTable Header = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                Header.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cells = new PdfPCell() { Border = 4 };
                Header.TotalWidth = 120;
                Header.SetTotalWidth(new float[] { 20f, 0f, 0f, 0f });


                cells = new PdfPCell(new Phrase("jktLFkku ljdkj", hindibold)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("Mh -5", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("  ou foHkkx", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jktdh; O;kikj  ;kstuk", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jo= fudklh ou mit", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                Phrase phraseL1 = new Phrase();
                phraseL1.Add(new Chunk("cqd u&  ", hindi));
                //phraseL1.Add(new Chunk(" " + "&&&&&&&&&&&&&&&&&&&&&&&&&&" + " ", hindi));  
                phraseL1.Add(new Chunk(" " + cd.BookNumber + " ", subheadfont));
                phraseL1.Add(new Chunk("dek±d", hindi));
                phraseL1.Add(new Chunk(" " + cd.ClearanceDetailsID + " ", subheadfont));
                phraseL1.Add(new Chunk("Mhiks", hindi));
                phraseL1.Add(new Chunk(" " + cd.Depot_Name + " ", subheadfont));
                phraseL1.Add(new Chunk("ls", hindi));
                phraseL1.Add(new Chunk(" " + cd.DestinationAddress + " ", subheadfont));
                phraseL1.Add(new Chunk("ykV  u", hindi));
                phraseL1.Add(new Chunk(" " + lotNumber + " ", subheadfont));
                phraseL1.Add(new Chunk("uke  [kjhnkj &", hindi));
                phraseL1.Add(new Chunk(" " + cd.WinnerName + " ", subheadfont));
                phraseL1.Add(new Chunk("firk dk uke", hindi));
                phraseL1.Add(new Chunk(" " + cd.FatherName + " ", subheadfont));
                phraseL1.Add(new Chunk("rkjh[k", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceFromDate, "dd/MM/yyyy") + " ", subheadfont));
                phraseL1.Add(new Chunk("le;", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceFromDate, "HH:mm") + " ", subheadfont));
                phraseL1.Add(new Chunk("çkr lk;  vkofèk ", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceToDate, "dd/MM/yyyy") + " ", subheadfont));
                phraseL1.Add(new Chunk("ls", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceToDate, "HH:mm") + " ", subheadfont));
                phraseL1.Add(new Chunk("rd", hindi));
                Header.AddCell(phraseL1);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);

                doc.Add(Header);

                #endregion

                #region body
                PdfPTable Details = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                //Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellud = new PdfPCell() { Border = 4 };
                Details.TotalWidth = 120;
                Details.SetTotalWidth(new float[] { 30f, 30f, 30f, 30f });

                cellud = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 }; //gram ka naam
                cellud.Colspan = 4;
                cellud.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("Ø l", hindi));//{ Border = 0 }; //gram ka naam
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("fooj.k eky", hindi));//{ Border = 0 };//khasra number
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("ux@otu@ uki ?ku ehVj", hindi));// { Border = 0 };//khasra number ka kul shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("okgu fooj.k", hindi));// { Border = 0 };// van bhoomi ka shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                foreach (var item in cd.DODProductList)
                {
                    cellud = new PdfPCell(new Phrase(item.SNo, subheadfont));//{ Border = 0 };//krishi&aawas
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.ProductName, subheadfont));//{ Border = 0 }; visheshvivran
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(Convert.ToString(item.Qty), subheadfont));// { Border = 0 };
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(cd.VehicleDescription, subheadfont));// { Border = 0 };yog
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);
                }

                doc.Add(Details);
                #endregion

                #region footer
                PdfPTable FooterDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellfd = new PdfPCell() { Border = 4 };
                FooterDetails.TotalWidth = 120;
                FooterDetails.SetTotalWidth(new float[] { 20f, 75f, 20f });

                cellfd = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cellfd.Colspan = 3;
                cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("gLrk{kj fudkldjrk", hindi)) { Border = 0 };//sign patwari
                cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("psd ukdk", hindi)) { Border = 0 }; //gram
                cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("gLrk{kj Mhiks lqijokÃtj", hindi)) { Border = 0 };// gram
                cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterDetails.AddCell(cellfd);

                doc.Add(FooterDetails);
                #endregion

                #endregion

                doc.Close();

                #region Generate Esign
                try
                {
                    clsDocumentESign requestPdf = new clsDocumentESign();
                    byte[] bytes = File.ReadAllBytes(filepath);
                    requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                    requestPdf.transactionid = transactionID;
                    clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, requestID);

                    if (response.Status != "0")
                    {
                        using (FileStream stream = System.IO.File.Create(EsignPath))
                        {
                            byte[] byteArray = Convert.FromBase64String(response.Document);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        msg.IsError = false;
                        msg.ReturnMsg = "Report generated successfully.";
                    }
                    else
                    {
                        msg.ReturnMsg = "Report generated without e-sign.";
                    }
                }
                catch (Exception) { msg.ReturnMsg = "Some error has occurred please try again later!!!"; }
                #endregion
            }

            return msg;
        }

        private ResponseMsg GenerateAuctionRegisterReport(string rootPath, string requestID, int documentTypeID, string transactionID, ref string docName)
        {
            ResponseMsg msg = new ResponseMsg();
            msg.IsError = true;

            var prms = new[]{
                            new SqlParameter("ActionCode", 4),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"]),
                            new SqlParameter("ParentID", requestID)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_DOD_GetAuctionDetails", prms);

            if (Util.isValidDataSet(data, 0))
            {
                var cd = Util.GetListFromTable<AuctionClearanceDetails>(data, 0).FirstOrDefault();
                var lotNumber = "N/A";

                if (Util.isValidDataSet(data, 1, true))
                {
                    cd.DODProductList = Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(data, 1);
                    lotNumber = cd.DODProductList.FirstOrDefault().DisplayLotNumber;
                }

                string filepath = string.Empty;
                string EsignPath = string.Empty;
                docName = string.Format("3_{0}_{1}_AuctionRegisterReport.pdf", requestID, documentTypeID);
                filepath = rootPath + Util.GetAppSettings("FRADocumentAllPDF") + docName;
                EsignPath = rootPath + Util.GetAppSettings("FRADocumentESignPDF") + docName;

                Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", Util.GetAppSettings("FontFileName"));
                BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 8);
                iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);

                var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
                var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
                var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);
                var smallfont = FontFactory.GetFont("Arial", 6, FontColour);
                var underlineFont = FontFactory.GetFont("Arial", 6, Font.UNDERLINE, FontColour);

                doc.Open();
                doc.NewPage();
                // doc.Add(new Paragraph(Environment.NewLine)); 
                #region D5

                #region header

                PdfPTable Header = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                Header.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cells = new PdfPCell() { Border = 4 };
                Header.TotalWidth = 120;
                Header.SetTotalWidth(new float[] { 20f, 0f, 0f, 0f });


                cells = new PdfPCell(new Phrase("jktLFkku ljdkj", hindibold)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("Mh -5", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("  ou foHkkx", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jktdh; O;kikj  ;kstuk", hindi)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase("jo= fudklh ou mit", hindiboldsmall)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };//niyam 12 dekhe
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Header.AddCell(cells);

                Phrase phraseL1 = new Phrase();
                phraseL1.Add(new Chunk("cqd u&  ", hindi));
                //phraseL1.Add(new Chunk(" " + "&&&&&&&&&&&&&&&&&&&&&&&&&&" + " ", hindi));  
                phraseL1.Add(new Chunk(" " + cd.BookNumber + " ", subheadfont));
                phraseL1.Add(new Chunk("dek±d", hindi));
                phraseL1.Add(new Chunk(" " + cd.ClearanceDetailsID + " ", subheadfont));
                phraseL1.Add(new Chunk("Mhiks", hindi));
                phraseL1.Add(new Chunk(" " + cd.Depot_Name + " ", subheadfont));
                phraseL1.Add(new Chunk("ls", hindi));
                phraseL1.Add(new Chunk(" " + cd.DestinationAddress + " ", subheadfont));
                phraseL1.Add(new Chunk("ykV  u", hindi));
                phraseL1.Add(new Chunk(" " + lotNumber + " ", subheadfont));
                phraseL1.Add(new Chunk("uke  [kjhnkj &", hindi));
                phraseL1.Add(new Chunk(" " + cd.WinnerName + " ", subheadfont));
                phraseL1.Add(new Chunk("firk dk uke", hindi));
                phraseL1.Add(new Chunk(" " + cd.FatherName + " ", subheadfont));
                phraseL1.Add(new Chunk("rkjh[k", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceFromDate, "dd/MM/yyyy") + " ", subheadfont));
                phraseL1.Add(new Chunk("le;", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceFromDate, "HH:mm") + " ", subheadfont));
                phraseL1.Add(new Chunk("çkr lk;  vkofèk ", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceToDate, "dd/MM/yyyy") + " ", subheadfont));
                phraseL1.Add(new Chunk("ls", hindi));
                phraseL1.Add(new Chunk(" " + Util.GetDateWithFormat(cd.ClearanceToDate, "HH:mm") + " ", subheadfont));
                phraseL1.Add(new Chunk("rd", hindi));
                Header.AddCell(phraseL1);

                cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cells.Colspan = 4;
                cells.HorizontalAlignment = Element.ALIGN_LEFT;
                Header.AddCell(cells);

                doc.Add(Header);

                #endregion

                #region body
                PdfPTable Details = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                //Details.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                PdfPCell cellud = new PdfPCell() { Border = 4 };
                Details.TotalWidth = 120;
                Details.SetTotalWidth(new float[] { 30f, 30f, 30f, 30f });

                cellud = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 }; //gram ka naam
                cellud.Colspan = 4;
                cellud.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("Ø l", hindi));//{ Border = 0 }; //gram ka naam
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("fooj.k eky", hindi));//{ Border = 0 };//khasra number
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("ux@otu@ uki ?ku ehVj", hindi));// { Border = 0 };//khasra number ka kul shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                cellud = new PdfPCell(new Phrase("okgu fooj.k", hindi));// { Border = 0 };// van bhoomi ka shetrafal
                cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                Details.AddCell(cellud);

                foreach (var item in cd.DODProductList)
                {
                    cellud = new PdfPCell(new Phrase(item.SNo, subheadfont));//{ Border = 0 };//krishi&aawas
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(item.ProductName, subheadfont));//{ Border = 0 }; visheshvivran
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(Convert.ToString(item.Qty), subheadfont));// { Border = 0 };
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);

                    cellud = new PdfPCell(new Phrase(cd.VehicleDescription, subheadfont));// { Border = 0 };yog
                    cellud.HorizontalAlignment = Element.ALIGN_LEFT;
                    Details.AddCell(cellud);
                }

                doc.Add(Details);
                #endregion

                #region footer
                PdfPTable FooterDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellfd = new PdfPCell() { Border = 4 };
                FooterDetails.TotalWidth = 120;
                FooterDetails.SetTotalWidth(new float[] { 20f, 75f, 20f });

                cellfd = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cellfd.Colspan = 3;
                cellfd.HorizontalAlignment = Element.ALIGN_RIGHT;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("gLrk{kj fudkldjrk", hindi)) { Border = 0 };//sign patwari
                cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("psd ukdk", hindi)) { Border = 0 }; //gram
                cellfd.HorizontalAlignment = Element.ALIGN_CENTER;
                FooterDetails.AddCell(cellfd);

                cellfd = new PdfPCell(new Phrase("gLrk{kj Mhiks lqijokÃtj", hindi)) { Border = 0 };// gram
                cellfd.HorizontalAlignment = Element.ALIGN_LEFT;
                FooterDetails.AddCell(cellfd);

                doc.Add(FooterDetails);
                #endregion

                #endregion

                doc.Close();

                #region Generate Esign
                try
                {
                    clsDocumentESign requestPdf = new clsDocumentESign();
                    byte[] bytes = File.ReadAllBytes(filepath);
                    requestPdf.inputJson.File = Convert.ToBase64String(bytes);
                    requestPdf.transactionid = transactionID;
                    clsDocumentESignResponce response = FMDSS.App_Start.cls_ESignIntegration.GenratePDFWithSign(requestPdf, requestID);

                    if (response.Status != "0")
                    {
                        using (FileStream stream = System.IO.File.Create(EsignPath))
                        {
                            byte[] byteArray = Convert.FromBase64String(response.Document);
                            stream.Write(byteArray, 0, byteArray.Length);
                        }
                        msg.IsError = false;
                        msg.ReturnMsg = "Report generated successfully.";
                    }
                    else
                    {
                        msg.ReturnMsg = "Report generated without e-sign.";
                    }
                }
                catch (Exception) { msg.ReturnMsg = "Some error has occurred please try again later!!!"; }
                #endregion
            }

            return msg;
        }

        #endregion

        #region Private Methods
        private string GetRequestInXML(dynamic model, string type = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tpRequest>");
            if (model.DODProductList != null && model.DODProductList.Count > 0)
            {
                sb.Append("<products>");
                if (type.Equals("AuctionWinner"))
                {
                    foreach (var item in model.DODProductList)
                    {
                        if (item.IsWinner)
                        {
                            sb.Append("<product>");
                            sb.Append(string.Format(@" 
                            <InventoryID>{0}</InventoryID> 
                            <BiddingAmount>{1}</BiddingAmount>
                            <EmitraHeadCode>{2}</EmitraHeadCode>
                            ", item.InventoryID, item.BiddingAmount, item.EmitraHeadCode));
                            sb.Append("</product>");
                        }
                    }
                }
                else
                {
                    foreach (var item in model.DODProductList)
                    {
                        if (item.IsWinner)
                        {
                            sb.Append("<product>");
                            sb.Append(string.Format(@" 
                            <InventoryID>{0}</InventoryID> 
                            <BiddingAmount>{1}</BiddingAmount>
                            ", item.InventoryID, item.BiddingAmount));
                            sb.Append("</product>");
                        }
                    }
                }
                sb.Append("</products>");
            }
            sb.Append("</tpRequest>");
            return Convert.ToString(sb);
        }
        #endregion

        #region DOD Report
        public DataTable DODInventory_Report(Entity.Protection.ViewModel.OffenceReportVM model, int actionCode = 1)
        {
            DataTable dtData = new DataTable();
            var prms = new[]{
                            new SqlParameter("ActionCode", actionCode),
                            new SqlParameter("CircleCode", model.CircleCode),
                            new SqlParameter("DivisionCode", model.DivisionCode),
                            new SqlParameter("RangeCode", model.RangeCode), 
                            new SqlParameter("FromDate", model.FromDate),
                            new SqlParameter("ToDate", model.ToDate),
                            //new SqlParameter("StatusID", model.StatusID),
                            new SqlParameter("UserID", HttpContext.Current.Session["UserId"])};
            _db.Fill(dtData, "SP_DOD_rpt_InventoryData", prms);
            return dtData;
        }
        #endregion
    }
}