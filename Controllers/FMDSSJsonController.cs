using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.BookOnlineZoo;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.OnlineBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Drawing;

namespace FMDSS.Controllers
{

    public class SelectedListItem
    {


        public string Text { get; set; }
        public string Value { get; set; }

    }
   
    public class FMDSSJsonController : Controller
    {
        //
        // GET: /FMDSSJson/

        public JsonResult GetFeedBack()
        {
            cls_Feedback Feedback = new cls_Feedback();

            var oObj = Feedback.GetFeedBack();
            return Json(oObj, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveFeedBack(string Content, string Design, string EaseofUse, string Comments, string SSOId)
        {
            // cls_Feed myDeserializedObj = (cls_Feed)Newtonsoft.Json.JsonConvert.DeserializeObject(Feed, typeof(cls_Feed));
            cls_Feed Feed = new cls_Feed { Content = Convert.ToInt16(Content), Design = Convert.ToInt16(Design), EaseofUse = Convert.ToInt16(EaseofUse), Comments = Comments, SsoId = SSOId };

            cls_Feedback Feedback = new cls_Feedback();
            bool result = Feedback.AddFeedBack(Feed);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGroupBook(string SSoId, string Institutional_NameofInstitute, string Institutional_AddressofInstitute, string Institutional_PhoneofInstitute, string Institutional_NameofHead,
            string Institutional_HeadPhoneNo, string Institutional_HeadIdType, string Institutional_HeadIdNumber, DateTime Institutional_DateofVisit, int isPrivateVehical, decimal Institutional_TotalVehicalFees, Int32 PlaceId)
        {

            List<cls_Feedback> FeedList = new List<cls_Feedback>();

            cls_Feedback Feedback = new cls_Feedback();
            DataTable Dtresult = Feedback.GetGroupDetails(SSoId, Institutional_NameofInstitute, Institutional_AddressofInstitute, Institutional_PhoneofInstitute, Institutional_NameofHead,
            Institutional_HeadPhoneNo, Institutional_HeadIdType, Institutional_HeadIdNumber, Institutional_DateofVisit, isPrivateVehical, Institutional_TotalVehicalFees, PlaceId);



            foreach (DataRow dr in Dtresult.Rows)
            {

                Feedback = new cls_Feedback
                {
                    SsoId = Convert.ToString(dr["SsoId"].ToString()),
                    Institutional_NameofInstitute = Convert.ToString(dr["Institutional_NameofInstitute"].ToString()),
                    Institutional_AddressofInstitute = Convert.ToString(dr["Institutional_AddressofInstitute"].ToString()),
                    Institutional_PhoneofInstitute = Convert.ToString(dr["Institutional_PhoneofInstitute"].ToString()),
                    Institutional_NameofHead = Convert.ToString(dr["Institutional_NameofHead"].ToString()),
                    Institutional_HeadPhoneNo = Convert.ToString(dr["Institutional_HeadPhoneNo"].ToString()),
                    Institutional_HeadIdType = Convert.ToString(dr["Institutional_HeadIdType"].ToString()),
                    Institutional_HeadIdNumber = Convert.ToString(dr["Institutional_HeadIdNumber"].ToString()),
                    Institutional_DateofVisit = Convert.ToDateTime(dr["Institutional_DateofVisit"]),
                    isPrivateVehical = Convert.ToInt32(dr["isPrivateVehical"]),
                    Institutional_TotalVehicalFees = Convert.ToDecimal(dr["Institutional_TotalVehicalFees"]),
                    PlaceId = Convert.ToInt16(dr["PlaceId"]),
                };
                FeedList.Add(Feedback);

            }


            return Json(FeedList, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetIndivisualDetails(string SSoId, Int32 PlaceId, DateTime Institutional_DateofVisit, int isPrivateVehical, decimal Institutional_TotalVehicalFees)
        {



            List<cls_Feedback> FeedList = new List<cls_Feedback>();

            cls_Feedback Feedback = new cls_Feedback();
            DataTable Dtresult1 = Feedback.GetIndivisualDetails(SSoId, PlaceId, Institutional_DateofVisit, isPrivateVehical, Institutional_TotalVehicalFees);

            foreach (DataRow dr in Dtresult1.Rows)
            {

                Feedback = new cls_Feedback
                {
                    SsoId = Convert.ToString(dr["SsoId"].ToString()),
                    Institutional_DateofVisit = Convert.ToDateTime(dr["Institutional_DateofVisit"]),
                    isPrivateVehical = Convert.ToInt32(dr["isPrivateVehical"]),
                    Institutional_TotalVehicalFees = Convert.ToDecimal(dr["Institutional_TotalVehicalFees"]),
                    PlaceId = Convert.ToInt16(dr["PlaceId"]),
                };
                FeedList.Add(Feedback);

            }


            return Json(FeedList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BookOnlineZooOnloadDetails(Int64 UserID)
        {
            BookOnzooMobileApp obj = new BookOnzooMobileApp();
            List<BookOnzooMobileApp> ticketList = new List<BookOnzooMobileApp>();
            BookOnzooMobileAppData objBookOnzooMobileAppData = new BookOnzooMobileAppData();
            string actionName = "CheckTicketAvailability";
            string controllerName = "BookOnlineZooOnloadDetails";

            List<SelectListItem> lstPlace = new List<SelectListItem>();
            DataTable dtPlace = new DataTable();
            try
            {

                dtPlace = obj.Select_Place(UserID);
                foreach (DataRow dr in dtPlace.Rows)
                {
                    lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }
                ViewBag.Place = lstPlace;

                DataTable dtf = new DataTable();
                dtf = obj.Select_BookedTicket();
                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new BookOnzooMobileApp()
                    {
                        TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                        TotalAmount = Convert.ToDecimal(dr["EmitraAmount"].ToString()),
                        EmitraTransactionId = dr["EmitraTransactionID"].ToString(),
                        DateOfArrival = dr["DateOfArrival"].ToString(),
                        TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString())
                    });
                }

                objBookOnzooMobileAppData.lstPlace = lstPlace;
                objBookOnzooMobileAppData.ticketList = ticketList;

                objBookOnzooMobileAppData.lstIDType = new List<SelectListItem>
                                            {                                               
                                                new SelectListItem { Text = "Passport", Value = "1"},
                                                new SelectListItem { Text = "Aadhar", Value = "2"},
                                                new SelectListItem { Text = "Driving Licence", Value = "3"},
                                                new SelectListItem { Text = "Voter ID", Value = "4"},
                                                new SelectListItem { Text = "PAN Card", Value = "5"},
                                                new SelectListItem { Text = "Office ID", Value = "6"},
                                               
                                            };

                return Json(objBookOnzooMobileAppData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
               
            }
            return null;
        }

        public JsonResult CheckTicketAvailability(int PlaceId, int ShiftType, string VisitDate, Int64 UserID)
        {
            string strStatus = string.Empty;
            string actionName = "CheckTicketAvailability";
            string controllerName = "ZooBookingMobileAppController";

            DataTable dtTicketdetails = new DataTable();
            try
            {
                BookOnzooMobileApp boz = new BookOnzooMobileApp();
                boz.KioskUserId = "0";

                dtTicketdetails = boz.CheckTicketAvailability(PlaceId, ShiftType, VisitDate, UserID);
                if (dtTicketdetails.Rows.Count > 0)
                {
                    if (dtTicketdetails.Rows[0][0].ToString() != "")
                    {

                        strStatus = dtTicketdetails.Rows[0][0].ToString();
                    }
                    else
                    {

                        strStatus = "0";
                    }
                }
                else
                {

                    strStatus = "0";
                }
            }
            catch (Exception ex)
            {
               
            }
            return Json(strStatus, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetMemberAndVehicleFeelst(string PlaceId, Int64 UserID)
        {
            actionName = "GetMemberAndVehicleFeelst";
            controllerName = "ZooBookingMobileAppController";

            var lstVehicle = new List<BookOnzooMobileApp>();
            BookOnzooMobileApp obj = new BookOnzooMobileApp();
            List<BookOnzooMobileApp> lstMember = new List<BookOnzooMobileApp>();
            BookOnzooMobileAppData objBookOnzooMobileAppData = new BookOnzooMobileAppData();
            DataSet dsMemberVehle = new DataSet();
            obj.PlaceOfVisit = PlaceId;
            try
            {


                dsMemberVehle = obj.MemberVehicleDetails(UserID);
                foreach (DataRow dr in dsMemberVehle.Tables[0].Rows)
                {
                    lstMember.Add(new BookOnzooMobileApp()
                    {
                        TypeOfMember = dr["FeeChargedOn"].ToString(),
                        NoOfMember = "0",
                        FeePerMember = dr["HeadAmount"].ToString(),
                        NoOfCamera = "0",

                        NoOfStillCamera = "",
                        FeePerStillCamera = dr["StillCameraAmount"].ToString(),

                        NoOfVideoCamera = "",
                        FeePerVideoCamera = dr["VideoCameraAmount"].ToString(),

                        //FeePerCamera = dr["CameraAmount"].ToString(),
                        TotalFeesOfMember = "0"
                    });
                }
                foreach (DataRow dr in dsMemberVehle.Tables[1].Rows)
                {
                    lstVehicle.Add(new BookOnzooMobileApp()
                    {
                        TypeOfVehicle = dr["FeeChargedOn"].ToString(),
                        FeePerVehicle = dr["HeadAmount"].ToString(),
                        NoOfVehicle = "0",
                        TotalVehicleFee = "0"
                    });
                }

                List<BookOnZoo> LstShiftType = new List<BookOnZoo>();


                objBookOnzooMobileAppData.lstMember = lstMember;
                objBookOnzooMobileAppData.lstVehicle = lstVehicle;
                objBookOnzooMobileAppData.lstShiftType = new SelectList(dsMemberVehle.Tables[2].AsDataView(), "ID", "Name").ToList();
            }
            catch (Exception ex)
            {
                
            }
            return Json(objBookOnzooMobileAppData, JsonRequestBehavior.AllowGet);
        }


        string actionName = string.Empty;
        string controllerName = string.Empty;

        public JsonResult ZooBookinigIDtype()
        {


            try
            {


                var lstIdType = new List<SelectedListItem>
                                            {                                               
                                                new SelectedListItem { Text = "Passport", Value = "1"},
                                                new SelectedListItem { Text = "Aadhar", Value = "2"},
                                                new SelectedListItem { Text = "Driving Licence", Value = "3"},
                                                new SelectedListItem { Text = "Voter ID", Value = "4"},
                                                new SelectedListItem { Text = "PAN Card", Value = "5"},
                                                new SelectedListItem { Text = "Office ID", Value = "6"},
                                               
                                            };

                return Json(lstIdType, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public JsonResult ZooBookingPlaces(Int64 UserID)
        {
            BookOnzooMobileApp obj = new BookOnzooMobileApp();

            BookOnzooMobileAppData objBookOnzooMobileAppData = new BookOnzooMobileAppData();

            List<SelectedListItem> lstPlace = new List<SelectedListItem>();
            DataTable dtPlace = new DataTable();
            try
            {

                dtPlace = obj.Select_Place(UserID);
                foreach (DataRow dr in dtPlace.Rows)
                {
                    lstPlace.Add(new SelectedListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }
                ViewBag.Place = lstPlace;






                return Json(lstPlace, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        public JsonResult ZooPlacewiseMemberVehicle(int PlaceId)
        {

            ZooFeesDetails obj = new ZooFeesDetails();


            cls_MemberChargeList objYewala = new cls_MemberChargeList();

            DataSet dtPlace = new DataSet();
            try
            {

                dtPlace = objYewala.ZooPlacewiseMemberVehicle(PlaceId);

                ZooFeesDetails zoodetail = new ZooFeesDetails();

                // zoodetail.MemberList memberlist = new List<MemberChargeList>();
                //  zoodetail.VehicleChargeList vehiclelist = new List<VehicleChargeList>();

                zoodetail.MemberList = new List<MemberChargeList>();
                zoodetail.VehicleChargeList = new List<VehicleChargeList>();
                foreach (DataRow dr in dtPlace.Tables[0].Rows)
                {
                    zoodetail.MemberList.Add(new MemberChargeList { FeeChargedOn = @dr["FeeChargedOn"].ToString(), HeadAmount = Convert.ToDecimal(@dr["HeadAmount"]), CameraAmount = Convert.ToDecimal(@dr["CameraAmount"]) });
                    //lstPlace.Add(new cls_ChargeListItem { FeeChargedOn = @dr["PlaceName"].ToString(), HeadAmount = @dr["PlaceID"].ToString(), CameraAmount = @dr["PlaceID"].ToString() });
                }


                foreach (DataRow dr in dtPlace.Tables[1].Rows)
                {
                    zoodetail.VehicleChargeList.Add(new VehicleChargeList { FeeChargedOn = @dr["FeeChargedOn"].ToString(), HeadAmount = Convert.ToDecimal(@dr["HeadAmount"]) });
                    //lstPlace.Add(new cls_ChargeListItem { FeeChargedOn = @dr["PlaceName"].ToString(), HeadAmount = @dr["PlaceID"].ToString(), CameraAmount = @dr["PlaceID"].ToString() });
                }
                ;





                return Json(zoodetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }


        /// <summary>
        /// /, string Base64Formatting_DocumentForTour, string Base64Formatting_UploadId
        /// </summary>
        /// <param name="SubmitZooBooking"></param>
        /// <returns></returns>

        private string GetExt(string Ext)
        {
            if (Ext == "data:image/png;base64")
                return ".png";
            else if (Ext == "data:image/jpeg;base64")
                return ".jpg";
            else
                return ".bmp";
        }

        private string SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);


            // byte[] sPDFDecoded = Convert.FromBase64String(base64BinaryStr);


            System.IO.File.WriteAllBytes(fullOutputPath, bytes);


            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    image = Image.FromStream(ms);
            //}
            //image.Save(fullOutputPath);
            //image.Dispose();
            return fullOutputPath;
        }





        [HttpPost]
        public JsonResult SubmitZooBooking(SubmitZooBooking SubmitZooBooking)
        {

            if (SubmitZooBooking.BookingType != "Institutional")
            {

                string DocumentForTourImageName = Path.GetFileNameWithoutExtension(SubmitZooBooking.DocumentForTourImageName);
                string DocumentForTourExt = Path.GetExtension(SubmitZooBooking.DocumentForTourImageName);

                string FilePath = Server.MapPath("~/ZoologicalDocument/" + DocumentForTourImageName + Guid.NewGuid().ToString().Substring(0, 6) + DocumentForTourExt);// +SubmitZooBooking.DocumentForTourImageName);

                string DocumentForTour = SaveByteArrayAsImage(FilePath, SubmitZooBooking.DocumentForTour.Split(',')[1]);

                SubmitZooBooking.DocumentForTour = DocumentForTour;



                string UploadIdImageName = Path.GetFileNameWithoutExtension(SubmitZooBooking.UploadIdImageName);
                string UploadIdExt = Path.GetExtension(SubmitZooBooking.UploadIdImageName);

                FilePath = Server.MapPath("~/ZoologicalDocument/" + UploadIdImageName + Guid.NewGuid().ToString().Substring(0, 6) + UploadIdExt);// +SubmitZooBooking.DocumentForTourImageName);

                string UploadId = SaveByteArrayAsImage(FilePath, SubmitZooBooking.UploadId.Split(',')[1]);


                SubmitZooBooking.UploadId = UploadId;
            }
            DataTable dtsubmit = new DataTable();
            BookOnZoo bz = new BookOnZoo();
            actionName = "ZooBookingMobileApp";
            controllerName = "ZooBookingMobileAppController";
            Int64 UserID = 0;
            string Document = string.Empty;
            string IdUpload = string.Empty;
            var path = "";

            int totalmember = 0;
            try
            {
                DataTable dsMember = MemberInformation(SubmitZooBooking.lstMember);
                DataTable dsVehicle = VehicleInformation(SubmitZooBooking.lstVehicle);

                if (dsMember.Rows.Count == 0)
                {


                }
                else
                {
                    for (int i = 0; i < dsMember.Rows.Count; i++)
                    {
                        totalmember += Convert.ToInt32(dsMember.Rows[i]["NoOfMember"]);
                    }
                }




                if (dsVehicle.Rows.Count == 0 && SubmitZooBooking.PrivateVehicle == true)
                {

                }

                //if (DocumentForTourfile.ContentLength > 0)
                //{




                // Document = Path.GetFileName(DocumentForTourfile.FileName);
                //    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                //    path = Path.Combine(FilePath, FileFullName);
                //   // SubmitZooBooking.DocumentForTour = path;
                //    SubmitZooBooking.DocumentForTour.SaveAs(Server.MapPath(FilePath + FileFullName));


                //    //var decodedFileBytes = Convert.FromBase64String(SubmitZooBooking.DocumentForTour.ContentType);

                //     bz.DocumentForTour = "pending Base64Formatting_DocumentForTour";
                //   // File.WriteAllBytes(FilePath + "/" + FileUpload[j].FileName, decodedFileBytes);

                //}
                //else
                //{
                //   // DocumentForTour = "";
                //}
                //if (SubmitZooBooking.UploadId.ContentLength  > 0)
                //{
                //    //IdUpload = Path.GetFileName(DocumentForTour.FileName);
                //    //String FileFullName = DateTime.Now.Ticks + "_" + IdUpload;
                //    //path = Path.Combine(FilePath, FileFullName);
                //    //bz.UploadId = path;
                //    //UploadId.SaveAs(Server.MapPath(FilePath + FileFullName));
                //   // DocumentForTour = "pending Base64Formatting_UploadId";
                //}
                //else
                //{
                //   //  UploadId = "";
                //}

                // IPAddress = bz.IPAddress + "[ " + bz.IPAddressAndDeviceKey + " ]";

                bz.KioskUserId = "0";

                dtsubmit = bz.Submit_ZooDetails(SubmitZooBooking, dsMember, dsVehicle);
                if (dtsubmit.Rows.Count > 0)
                {
                    decimal finalAmnt = Convert.ToDecimal(dtsubmit.Rows[0]["TotalFinalAmount"].ToString());
                    //Session["totalprice"] = finalAmnt;
                    //Session["ZooRequestId"] = dtsubmit.Rows[0]["RequestID"].ToString();
                }

                //  return Json(dtsubmit.AsEnumerable(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            FinalPay pay = new FinalPay();
            if (dtsubmit.Rows.Count > 0)
            {
                pay.RequestId = Convert.ToString(dtsubmit.Rows[0]["RequestId"]);
                pay.MemberFee = Convert.ToDecimal(dtsubmit.Rows[0]["MemberFee"]);
                pay.CameraFee = Convert.ToDecimal(dtsubmit.Rows[0]["CameraFee"]);
                pay.EmitraCharges = Convert.ToDecimal(dtsubmit.Rows[0]["EmitraCharges"]);
                pay.TotalFinalAmount = Convert.ToDecimal(dtsubmit.Rows[0]["TotalFinalAmount"]);
                pay.status = Convert.ToString(dtsubmit.Rows[0]["status"]);
            }

            return Json(pay, JsonRequestBehavior.AllowGet);
            // return  Pay(false, SubmitZooBooking.SsoId, Convert.ToString(dtsubmit.Rows[0]["RequestId"]), "http://10.68.128.101/fmdsstest/ZooAppEmitraResponce.aspx", "http://10.68.128.101/fmdsstest/ZooAppEmitraResponce.aspx");

        }


        public string Pay(bool IsMobileApp, string Ssoid, string RequestId, string SUCCESSEmitraReturnURL, string FAILEDEmitraReturnURL)
        {
            string actionName = "Pay";
            string controllerName = "FMDSSJsonController";
            string EmitraPaymentGatwayString = "";

            try
            {
                // get different heads amount from DB
                BookOnTicket OBJ = new BookOnTicket();
                DataSet DS = new DataSet();
                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("ZooTickets", RequestId);
                // 0 Head for ecodevelopment surcharge - 0406-02-800-01
                // 1 Head for entry fees- 0406-01-800-05
                // 2 Grand Total
                // 3 Office

                string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]);

                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();


                EmitraPaymentGatwayString = ObjEmitraPayRequest.PayRequest(true, RequestId,
                Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                 SUCCESSEmitraReturnURL, FAILEDEmitraReturnURL,
                Convert.ToString(DS.Tables[0].Rows[1]["HeadAmount"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                Convert.ToString(DS.Tables[0].Rows[0]["HeadAmount"]), REVENUEHEAD, Ssoid);

                //Response.Redirect(EmitraPaymentGatwayString);
            }
            catch (Exception ex)
            {
                
            }
            return EmitraPaymentGatwayString;

        }
        /// <summary>
        /// function to display response from emitra
        /// </summary>
        /// <returns></returns>
        /// 


        public PaymentStatus PaymentUpdate(string MERCHANTCODEs, string PRNs, string STATUSs, string ENCDATAs, string RequestId, string TotalPrice, string Ssoid)
        {
            PaymentStatus ObjPaymentStatus = new PaymentStatus();
            string actionName = "PaymentUpdate";
            string controllerName = "FMDSSJsonController";

            TotalPrice = "1";

            if (RequestId != null && RequestId != string.Empty)
            {
                try
                {
                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";



                    if (MERCHANTCODEs != null)
                        MERCHANTCODE = MERCHANTCODEs.ToString();
                    if (PRNs != null)
                        PRN = PRNs.ToString();
                    if (STATUSs != null)
                        STATUS = STATUSs.ToString();
                    if (ENCDATAs != null)
                        ENCDATA = ENCDATAs.ToString();



                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    BookOnTicket cs1 = new BookOnTicket();
                    cs1.UpdateEmitraResponse(RequestId.ToString(), "ZooTicketBooking", DecryptedData);

                    int status1 = 0;
                    BookOnZoo cs = new BookOnZoo();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion



                    #region Response Status
                    if (ObjPGResponse.STATUS == "FAILED")
                    {
                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = "0";
                        cs.RequestId = ObjPGResponse.PRN;

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = "";

                        dtrow["USER NAME"] = ObjPGResponse.UDF1;// use as user name

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            cs.Trn_Status_Code = 0;
                            cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {

                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = ObjPGResponse.AMOUNT;
                        cs.RequestId = RequestId.ToString();


                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            if (Convert.ToString(RequestId).Equals(ObjPGResponse.PRN) && (TotalPrice != null && Convert.ToDecimal(TotalPrice) == Convert.ToDecimal(ObjPGResponse.AMOUNT)))
                            {
                                cs.Trn_Status_Code = 1;
                                status1 = 1;
                                cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));

                                SendZooBookingEmailSMS(RequestId);
                            }
                            else // Added to save mismatch in payment
                            {
                                cs.Trn_Status_Code = 0;
                                status1 = 0;
                                cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));
                            }
                        }

                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                    //DataTable DTdetails = cs.Get_BookedTicketDetails(RequestId.ToString());
                    //List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();
                    //foreach (DataRow dr in DTdetails.Rows)
                    //{
                    //    List.Add(
                    //           new CS_BoardingDetails()
                    //           {
                    //               PrintID = Convert.ToString(dr["ZooBookingId"]),
                    //               RequestID = Convert.ToString(dr["RequestId"]),
                    //               PlaceName = Convert.ToString(dr["PlaceName"]),
                    //               Vehicle = Convert.ToString(dr["VehicleType"]),
                    //               TotalMembers = Convert.ToString(dr["NoOfMember"]),
                    //               DateofBooking = Convert.ToString(dr["DateofBooking"]),
                    //               DateofVisit = Convert.ToString(dr["DateOfVisit"]),
                    //               AmountTobePaid = Convert.ToString(dr["TotalAmountToBePaid"]),
                    //           });
                    //}
                    //ViewData["TicketSummary"] = List;
                    // ViewBag.TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();


                    //             public string TRANSACTION_STATUS { get; set; }
                    //public string REQUEST_ID { get; set; }
                    //public string EMITRA_TRANSACTION_ID { get; set; }
                    //public string TRANSACTION_TIME { get; set; }
                    //public string TRANSACTION_AMOUNT { get; set; }
                    //public string EMITRA_AMOUNT { get; set; }
                    //public string USER_NAME { get; set; }
                    //public string TRANSACTION_BANK_DETAILS { get; set; }


                    ObjPaymentStatus.TRANSACTION_STATUS = Convert.ToString(dt.Rows[0]["TRANSACTION STATUS"]);
                    ObjPaymentStatus.REQUEST_ID = Convert.ToString(dt.Rows[0]["REQUEST ID"]);
                    ObjPaymentStatus.EMITRA_TRANSACTION_ID = Convert.ToString(dt.Rows[0]["EMITRA TRANSACTION ID"]);
                    ObjPaymentStatus.TRANSACTION_TIME = Convert.ToString(dt.Rows[0]["TRANSACTION TIME"]);
                    ObjPaymentStatus.TRANSACTION_AMOUNT = Convert.ToString(dt.Rows[0]["TRANSACTION AMOUNT"]);
                    ObjPaymentStatus.EMITRA_AMOUNT = Convert.ToString(dt.Rows[0]["EMITRA AMOUNT"]);
                    ObjPaymentStatus.USER_NAME = Convert.ToString(dt.Rows[0]["USER NAME"]);
                    ObjPaymentStatus.TRANSACTION_BANK_DETAILS = Convert.ToString(dt.Rows[0]["TRANSACTION BANK DETAILS"]);

                    return ObjPaymentStatus;
                }
                catch (Exception ex)
                {
                    //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, 0);
                }

                return ObjPaymentStatus;
            }
            return ObjPaymentStatus;

        }

        public void SendZooBookingEmailSMS(string ZooBookingId)
        {
            //Send_ZooEmailSMS obj = new Send_ZooEmailSMS();
            try
            {
                #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

                string body = string.Empty;
                DataTable DT = objSMSandEMAILtemplate.GetUserDetailsForZoo(ZooBookingId);
                if (DT.Rows.Count > 0)
                {
                    if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));

                        objSMS_EMail_Services.sendEMail("Zoo Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());

                        body = string.Empty;

                    }

                    if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ZooTicketSMSTemplate"].ToString()));

                        SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

                        body = string.Empty;
                    }

                }


                #endregion


            }



            catch (Exception ex)
            {
                //return false;
            }

        }


        /// <summary>
        /// function to return member information in datatable
        /// </summary>
        /// <param name="lstMemberInfo"></param>
        /// <returns></returns>
        [NonAction]
        public DataTable MemberInformation(List<MemberInformation> lstMemberInfo)
        {
            string actionName = "MemberInformation";
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region MemberInfo
                objDt2.Columns.Add("TypeOfMember", typeof(String));
                objDt2.Columns.Add("NoOfMember", typeof(String));
                objDt2.Columns.Add("FeePerMember", typeof(String));
                objDt2.Columns.Add("NoOfStillCamera", typeof(String));
                objDt2.Columns.Add("FeePerStillCamera", typeof(String));
                objDt2.Columns.Add("NoOfVideoCamera", typeof(String));
                objDt2.Columns.Add("FeePerVideoCamera", typeof(String));

                objDt2.Columns.Add("TotalFees", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0" && item.TotalFeesOfMember != null && item.TotalFeesOfMember != "0" && item.TotalFeesOfMember != string.Empty)
                    {
                        dr["TypeOfMember"] = item.TypeOfMember;
                        dr["NoOfMember"] = item.NoOfMember;
                        dr["FeePerMember"] = item.FeePerMember;
                        dr["NoOfStillCamera"] = string.IsNullOrEmpty(item.NoOfStillCamera) ? "0" : item.NoOfStillCamera;
                        dr["FeePerStillCamera"] = string.IsNullOrEmpty(item.FeePerStillCamera) ? "0" : item.FeePerStillCamera;
                        dr["NoOfVideoCamera"] = string.IsNullOrEmpty(item.NoOfVideoCamera) ? "0" : item.NoOfVideoCamera;
                        dr["FeePerVideoCamera"] = string.IsNullOrEmpty(item.FeePerVideoCamera) ? "0" : item.FeePerVideoCamera;

                        dr["TotalFees"] = ((Convert.ToDecimal(item.FeePerMember) * Convert.ToDecimal(item.NoOfMember))
                            + (Convert.ToDecimal(item.FeePerStillCamera) * Convert.ToDecimal(item.NoOfStillCamera))
                            + (Convert.ToDecimal(item.FeePerVideoCamera) * Convert.ToDecimal(item.NoOfVideoCamera)));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                    else
                    {
                        if (item.TypeOfMember == "Child Below Age of 5 Years" && item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0")
                        {
                            dr["TypeOfMember"] = item.TypeOfMember;
                            dr["NoOfMember"] = item.NoOfMember;
                            dr["FeePerMember"] = item.FeePerMember;
                            dr["NoOfStillCamera"] = string.IsNullOrEmpty(item.NoOfStillCamera) ? "0" : item.NoOfStillCamera;
                            dr["FeePerStillCamera"] = string.IsNullOrEmpty(item.FeePerStillCamera) ? "0" : item.FeePerStillCamera;
                            dr["NoOfVideoCamera"] = string.IsNullOrEmpty(item.NoOfVideoCamera) ? "0" : item.NoOfVideoCamera;
                            dr["FeePerVideoCamera"] = string.IsNullOrEmpty(item.FeePerVideoCamera) ? "0" : item.FeePerVideoCamera;

                            dr["TotalFees"] = ((Convert.ToDecimal(item.FeePerMember) * Convert.ToDecimal(item.NoOfMember))
                                + (Convert.ToDecimal(item.FeePerStillCamera) * Convert.ToDecimal(item.NoOfStillCamera))
                                + (Convert.ToDecimal(item.FeePerVideoCamera) * Convert.ToDecimal(item.NoOfVideoCamera)));
                            objDt2.Rows.Add(dr);
                            objDt2.AcceptChanges();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }


        [HttpPost]
        public JsonResult MemberList(List<MemberInformation> lstMemberInfo)
        {
            //string MSLNo, string TypeOfMember, string NoOfMember, string FeePerMember, string NoOfCamera, string FeePerCamera, string TotalFeesOfMember, string TotalFee
            string actionName = "MemberInformation";
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;


            MemberInformation MemberInfo = new MemberInformation();
            List<MemberInformation> lstMember = new List<MemberInformation>();
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region MemberInfo
                objDt2.Columns.Add("TypeOfMember", typeof(String));
                objDt2.Columns.Add("NoOfMember", typeof(String));
                objDt2.Columns.Add("FeePerMember", typeof(String));
                objDt2.Columns.Add("NoOfStillCamera", typeof(String));
                objDt2.Columns.Add("FeePerStillCamera", typeof(String));
                objDt2.Columns.Add("NoOfVideoCamera", typeof(String));
                objDt2.Columns.Add("FeePerVideoCamera", typeof(String));

                objDt2.Columns.Add("TotalFees", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0" && item.TotalFeesOfMember != null && item.TotalFeesOfMember != "0" && item.TotalFeesOfMember != string.Empty)
                    {
                        dr["TypeOfMember"] = item.TypeOfMember;
                        dr["NoOfMember"] = item.NoOfMember;
                        dr["FeePerMember"] = item.FeePerMember;
                        dr["NoOfStillCamera"] = string.IsNullOrEmpty(item.NoOfStillCamera) ? "0" : item.NoOfStillCamera;
                        dr["FeePerStillCamera"] = string.IsNullOrEmpty(item.FeePerStillCamera) ? "0" : item.FeePerStillCamera;
                        dr["NoOfVideoCamera"] = string.IsNullOrEmpty(item.NoOfVideoCamera) ? "0" : item.NoOfVideoCamera;
                        dr["FeePerVideoCamera"] = string.IsNullOrEmpty(item.FeePerVideoCamera) ? "0" : item.FeePerVideoCamera;

                        dr["TotalFees"] = ((Convert.ToDecimal(item.FeePerMember) * Convert.ToDecimal(item.NoOfMember))
                            + (Convert.ToDecimal(item.FeePerStillCamera) * Convert.ToDecimal(item.NoOfStillCamera))
                            + (Convert.ToDecimal(item.FeePerVideoCamera) * Convert.ToDecimal(item.NoOfVideoCamera)));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                    else
                    {
                        if (item.TypeOfMember == "Child Below Age of 5 Years" && item.NoOfMember != string.Empty && item.NoOfMember != null && item.NoOfMember != "0")
                        {
                            dr["TypeOfMember"] = item.TypeOfMember;
                            dr["NoOfMember"] = item.NoOfMember;
                            dr["FeePerMember"] = item.FeePerMember;
                            dr["NoOfStillCamera"] = string.IsNullOrEmpty(item.NoOfStillCamera) ? "0" : item.NoOfStillCamera;
                            dr["FeePerStillCamera"] = string.IsNullOrEmpty(item.FeePerStillCamera) ? "0" : item.FeePerStillCamera;
                            dr["NoOfVideoCamera"] = string.IsNullOrEmpty(item.NoOfVideoCamera) ? "0" : item.NoOfVideoCamera;
                            dr["FeePerVideoCamera"] = string.IsNullOrEmpty(item.FeePerVideoCamera) ? "0" : item.FeePerVideoCamera;

                            dr["TotalFees"] = ((Convert.ToDecimal(item.FeePerMember) * Convert.ToDecimal(item.NoOfMember))
                                + (Convert.ToDecimal(item.FeePerStillCamera) * Convert.ToDecimal(item.NoOfStillCamera))
                                + (Convert.ToDecimal(item.FeePerVideoCamera) * Convert.ToDecimal(item.NoOfVideoCamera)));
                            objDt2.Rows.Add(dr);
                            objDt2.AcceptChanges();
                        }
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            //lstMember.Add(objDt2);
            return Json(objDt2, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Function to return member information in datatable
        /// </summary>
        /// <param name="lstMemberInfo"></param>
        /// <returns></returns>
        [NonAction]
        public DataTable VehicleInformation(List<VehicleInformation> lstVehicleInfo)
        {
            string actionName = "VehicleInformation";
            string controllerName = "FMDSSJsonController";
            Int64 UserID = 0;
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region Vehicle Info
                objDt2.Columns.Add("VehicleType", typeof(String));
                objDt2.Columns.Add("FeePerVehicle", typeof(String));
                objDt2.Columns.Add("NoOfVehicle", typeof(String));
                objDt2.Columns.Add("TotalVehicleFee", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstVehicleInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfVehicle != "0" && item.NoOfVehicle != null && item.NoOfVehicle != string.Empty && item.TotalVehicleFee != "0" && item.TotalVehicleFee != string.Empty && item.TotalVehicleFee != null)
                    {
                        dr["VehicleType"] = item.TypeOfVehicle;
                        dr["FeePerVehicle"] = item.FeePerVehicle;
                        dr["NoOfVehicle"] = item.NoOfVehicle;
                        dr["TotalVehicleFee"] = (Convert.ToDecimal(item.NoOfVehicle) * Convert.ToDecimal(item.FeePerVehicle));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }
        public JsonResult VehicleList(List<VehicleInformation> lstVehicleInfo)
        {
            string actionName = "VehicleInformation";
            string controllerName = "FMDSSJsonController";
            Int64 UserID = 0;
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region Vehicle Info
                objDt2.Columns.Add("VehicleType", typeof(String));
                objDt2.Columns.Add("FeePerVehicle", typeof(String));
                objDt2.Columns.Add("NoOfVehicle", typeof(String));
                objDt2.Columns.Add("TotalVehicleFee", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstVehicleInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfVehicle != "0" && item.NoOfVehicle != null && item.NoOfVehicle != string.Empty && item.TotalVehicleFee != "0" && item.TotalVehicleFee != string.Empty && item.TotalVehicleFee != null)
                    {
                        dr["VehicleType"] = item.TypeOfVehicle;
                        dr["FeePerVehicle"] = item.FeePerVehicle;
                        dr["NoOfVehicle"] = item.NoOfVehicle;
                        dr["TotalVehicleFee"] = (Convert.ToDecimal(item.NoOfVehicle) * Convert.ToDecimal(item.FeePerVehicle));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
               // new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(objDt2, JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /FMDSSJson/Create

        public JsonResult GetShift()
        {
            var lstShiftType = new List<SelectedListItem>
                                            {
                                                new SelectedListItem { Text = "--Select--", Value = "0"},
                                                new SelectedListItem { Text = "FULL Day", Value = "3"}
                                            };
            return Json(lstShiftType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FMDSSJson/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /FMDSSJson/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /FMDSSJson/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /FMDSSJson/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /FMDSSJson/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
