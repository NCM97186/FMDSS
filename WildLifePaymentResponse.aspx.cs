using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.OnlineBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS.Views.BookOnlineTicket
{
    public partial class WildLifePaymentResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentStatus objResponce = GetPayment();

            if (objResponce.TRANSACTION_STATUS == "SUCCESS")
            {
                Response.Redirect("MobileSuccess.html?TRANSACTION_STATUS=" + objResponce.TRANSACTION_STATUS + "&EMITRA_AMOUNT=" + objResponce.EMITRA_AMOUNT + "&EMITRA_TRANSACTION_ID=" + objResponce.EMITRA_TRANSACTION_ID + "&REQUEST_ID=" + objResponce.REQUEST_ID);
            }
            else
            {
                Response.Redirect("MobileFail.html?TRANSACTION_STATUS=" + objResponce.TRANSACTION_STATUS + "&EMITRA_AMOUNT=" + objResponce.EMITRA_AMOUNT + "&EMITRA_TRANSACTION_ID=" + objResponce.EMITRA_TRANSACTION_ID + "&REQUEST_ID=" + objResponce.REQUEST_ID);
            }
        }
        public PaymentStatus GetPayment() // return emitra code 
        {
            int fmdssStatus = 0;
            string resultMsg = "";
            DataTable resDt = new DataTable();
            FMDSS.Controllers.FMDSSJsonController obj = new Controllers.FMDSSJsonController();
            PaymentStatus objPaymentStatus = new PaymentStatus();
            string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";

            if (Request.Form["MERCHANTCODE"] != null)
                MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
            if (Request.Form["PRN"] != null)
                PRN = Request.Form["PRN"].ToString();
            if (Request.Form["STATUS"] != null)
                STATUS = Request.Form["STATUS"].ToString();
            if (Request.Form["ENCDATA"] != null)
                ENCDATA = Request.Form["ENCDATA"].ToString();

            System.Web.HttpContext.Current.Session["UserID"] = "0";
            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");
            //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016"); //UAT UAT CHANGES

            string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
            // string DecryptedData = "{'MERCHANTCODE':'FOREST0117','REQTIMESTAMP':'20170810001655000','PRN':'636379209711021628','AMOUNT':'10044.00','PAIDAMOUNT':'10050.00','SERVICEID':'2239','TRANSACTIONID':'170055011924','RECEIPTNO':'17053223840','EMITRATIMESTAMP':'20170810001853257','STATUS':'SUCCESS','PAYMENTMODE':'Kotak Bank Payment Gateway(All Banks)','PAYMENTMODEBID':'CTX170809184817579982','PAYMENTMODETIMESTAMP':'20170810001715000','RESPONSECODE':'200','RESPONSEMESSAGE':'Transaction Successfully Done At Emitra.','UDF1':'CKNDR HASHIM','UDF2':'udf2','CHECKSUM':'03263f854d49bcdbf966ce9ffe494d26'}";
            PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

            int status1 = 0;
            BookOnTicket cs = new BookOnTicket();
            Payment pay = new Payment();
            DataTable dt = new DataTable();

            cs.UpdateEmitraResponse(Convert.ToString(ObjPGResponse.PRN), "WildLifeTicketBooking", DecryptedData);

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

            string steps = string.Empty;
            #region Response Status
            // if (ObjPGResponse.STATUS == "FAILED") Arvind Sir
            if (ObjPGResponse.STATUS != "SUCCESS") //Rajveer
            {
                //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 0: Enter Success " });
                DataRow dtrow = dt.NewRow();

                cs.EmitraTransactionId = "0";
                cs.RequestId = ObjPGResponse.PRN;

                cs.KioskUserId = "0";


                dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                dtrow["EMITRA TRANSACTION ID"] = "";
                dtrow["TRANSACTION TIME"] = "";
                dtrow["TRANSACTION AMOUNT"] = "0";
                dtrow["EMITRA AMOUNT"] = "0";
                dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                try
                {
                    cs.Trn_Status_Code = 0;
                    fmdssStatus = 0;
                    resultMsg = "";
                    resDt = cs.UpdateTransactionStatus("3", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                    if (resDt.Rows.Count > 0)
                    {

                        fmdssStatus = Convert.ToInt32(resDt.Rows[0][0].ToString());
                        resultMsg = resDt.Rows[0][3].ToString();
                    }
                }
                catch (Exception ex)
                {
                    //System.IO.File.AppendAllLines(Server.MapPath("~/Errorlog.txt"), new[] { ObjPGResponse.PRN + " [ " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + " ] " + " Step FAILED 2: catch in Thread " + ex.Message });
                }
                if (fmdssStatus == 1)
                {
                    dtrow["TRANSACTION STATUS"] = "SUCCESS";
                }
                else
                {
                    dtrow["TRANSACTION STATUS"] = "FAILED";
                }
                dt.Rows.Add(dtrow);
               
            }
            else if (ObjPGResponse.STATUS == "SUCCESS")
            {
                DataRow dtrow = dt.NewRow();
                dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                cs.RequestId = ObjPGResponse.PRN;
                cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;
                //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                objPaymentStatus.TRANSACTION_STATUS = ObjPGResponse.STATUS;
                objPaymentStatus.EMITRA_AMOUNT = ObjPGResponse.AMOUNT;
                objPaymentStatus.EMITRA_TRANSACTION_ID = ObjPGResponse.EMITRATIMESTAMP;
                objPaymentStatus.REQUEST_ID = ObjPGResponse.PRN;
                if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                {
                    try
                    {
                        if (Convert.ToString(ObjPGResponse.PRN).Equals(ObjPGResponse.PRN))
                        {
                            cs.Trn_Status_Code = 1;
                            status1 = 1;
                            fmdssStatus = 0;
                            resultMsg = "";
                            resDt = cs.UpdateTransactionStatus("3", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                            if (resDt.Rows.Count > 0)
                            {
                                fmdssStatus = Convert.ToInt32(resDt.Rows[0][0].ToString());
                                resultMsg = resDt.Rows[0][3].ToString();
                            }
                        }
                        else // Added to save mismatch in payment
                        {
                            cs.Trn_Status_Code = 0;
                            status1 = 0;
                            fmdssStatus = 0;
                            resultMsg = "";
                            resDt = cs.UpdateTransactionStatus("3", Convert.ToDouble(ObjPGResponse.AMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT), ObjPGResponse.EMITRATIMESTAMP);
                            if (resDt.Rows.Count > 0)
                            {
                                fmdssStatus = Convert.ToInt32(resDt.Rows[0][0].ToString());
                                resultMsg = resDt.Rows[0][3].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                    }
                }
                if (fmdssStatus == 1)
                {
                    dtrow["TRANSACTION STATUS"] = "SUCCESS";
                    objPaymentStatus.TRANSACTION_STATUS = "SUCCESS";
                }
                else
                {
                    dtrow["TRANSACTION STATUS"] = "FAILED";
                    objPaymentStatus.TRANSACTION_STATUS = "FAILED";
                }
                dt.Rows.Add(dtrow);
                
            }
            #endregion
            List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();

            string TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();

            if (TicketStatus == "SUCCESS")
            {
                DataTable DTdetails = cs.Get_BookedTicketDetails(Convert.ToString(ObjPGResponse.PRN));

                foreach (DataRow dr in DTdetails.Rows)
                {
                    List.Add(
                           new CS_BoardingDetails()
                           {
                               PrintID = Convert.ToString(dr["PrintID"]),
                               RequestID = Convert.ToString(dr["RequestID"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               Vehicle = Convert.ToString(dr["Vehicle"]),
                               TotalMembers = Convert.ToString(dr["TotalMembers"]),
                               DateofBooking = Convert.ToString(dr["DateofBooking"]),
                               DateofVisit = Convert.ToString(dr["DateofVisit"]),
                               AmountTobePaid = Convert.ToString(dr["AmountTobePaid"]),
                               BoardingPointName = Convert.ToString(dr["BoardingPointName"]),
                           });

                }
                string PrintID = "";
                if (DTdetails.Rows.Count > 0)
                {
                    PrintID = Convert.ToString(DTdetails.Rows[0]["PrintID"]);
                }
                else
                {
                    PrintID = "";
                }
            }

            return objPaymentStatus;
        }
    }
}