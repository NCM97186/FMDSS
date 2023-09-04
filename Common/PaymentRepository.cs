using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CommanModels;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CommanModels
{
    public class PaymentRepository
    {

        DataSet ds = new DataSet();
        public static PaymentResponse Pay(BookingType bookingType, PaymentViewModel paymentModel)
        {
            PaymentResponse resp = new PaymentResponse();
            try
            {
                if (bookingType == BookingType.OnlineCitizenBooking)
                {
                    resp = RedirectCitizenToEmitra(paymentModel);
                }
                else if (bookingType == BookingType.DepartmentBooking)
                {

                }
                else if (bookingType == BookingType.EmitraKioskBooking)
                {
                    resp = ProcessEmitraKioskPayment(paymentModel);
                }
            }
            catch (Exception ex)
            {
                resp = new PaymentResponse { Status_Code = "0", Status = "error", Message = ex.Message };
            }
            return resp;
        }


        #region Online Payment
        private static PaymentResponse RedirectCitizenToEmitra(PaymentViewModel paymentModel)
        {
            PaymentResponse resp = new PaymentResponse();
            try
            {
                if (string.IsNullOrEmpty(paymentModel.emitraserviceid))
                {
                    resp = new PaymentResponse { Status_Code = "0", Status = "error", Message = "emitra service id can't be null." };
                }
                else
                {
                    PaymentRequestViewModel request = GetPaymentServiceDetail(paymentModel, BookingType.OnlineCitizenBooking.ToString());
                    //Commented below for test by dipak, need to uncomment it
                    EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();
                    string response = ObjEmitraPayRequest.PayRequest(false, paymentModel.requestid, request.emitrarequest.MERCHANTCODE, request.emitrarequest.CHECKSUM, request.emitrarequest.ENCRYPTIONKEY, request.emitrarequest.SUCCESSURL, request.emitrarequest.FAILUREURL, request.emitrarequest.OFFICECODE, request.emitrarequest.SERVICEID, request.emitrarequest.AMOUNT, request.emitrarequest.REVENUEHEAD, string.Empty);
                    resp = new PaymentResponse { OnlinePaymentResponse = response, };
                }
            }
            catch (Exception ex)
            {
                resp = new PaymentResponse { Status_Code = "0", Status = "error", Message = ex.Message };
            }
            return resp;
        }
        #endregion

        #region Kiosk Payment
        private static PaymentResponse ProcessEmitraKioskPayment(PaymentViewModel paymentModel)
        {
            PaymentResponse resp = new PaymentResponse();
            try
            {

                if (string.IsNullOrEmpty(paymentModel.emitraserviceid))
                {
                    resp = new PaymentResponse { Status_Code = "0", Status = "error", Message = "emitra service id can't be null." };
                }
                else
                {
                    PaymentRequestViewModel request = GetPaymentServiceDetail(paymentModel, BookingType.EmitraKioskBooking.ToString());
                    if (string.IsNullOrEmpty(paymentModel.requestid))
                    {
                        resp = new PaymentResponse { Status_Code = "0", Status = "error", Message = "request id can't be null." };
                    }
                    //else if (string.IsNullOrEmpty(paymentModel.officecode))
                    //{
                    //    resp = new PaymentResponse { Status_Code = "0", Status = "error", Message = "office code can't be null." };
                    //}
                    else
                    {
                        request.emitrarequest.REQUESTID = paymentModel.requestid;
                        request.emitrarequest.CONSUMERKEY = paymentModel.requestid;

                        string ssoid = Convert.ToString(HttpContext.Current.Session["KioskSSOId"]).ToUpper();
                        string UserName = string.Empty;
                        DataTable dtSSoDetail = GetSSODetail(ssoid);
                        if (dtSSoDetail != null && dtSSoDetail.Rows.Count > 0)
                        {
                            UserName = string.IsNullOrEmpty(Convert.ToString(dtSSoDetail.Rows[0]["Name"])) ? "NA" : Convert.ToString(dtSSoDetail.Rows[0]["Name"]);
                        }
                        request.emitrarequest.CONSUMERNAME = UserName;

                        request.emitrarequest.REQTIMESTAMP = DateTime.Now.Ticks.ToString();
                        request.emitrarequest.SSOID = Convert.ToString(HttpContext.Current.Session["KioskSSOId"]).ToUpper();
                        request.emitrarequest.BASEURL = request.Links.BaseUrl;
                        request.emitrarequest.SUBSERVICEID = "";
                        request.emitrarequest.VERIFICAION_URL = request.Links.VerificationUrl;
                        request.emitrarequest.SERVICERESPONSETIME = Convert.ToInt16(request.Links.MaxResponseTime);
                        request.emitrarequest.OFFICECODE = string.IsNullOrWhiteSpace(paymentModel.officecode) ? request.emitrarequest.OFFICECODE : paymentModel.officecode;
                        request.emitrarequest.SSOTOKEN = Convert.ToString(HttpContext.Current.Session["SSOTOKEN"]);
                        EmitraKisokPayment emitraKisokPayment = new EmitraKisokPayment();
                        EmitraKioskResponse emitraKisokResponse = emitraKisokPayment.ProcessPayment(request.emitrarequest);
                        if (emitraKisokResponse.TRANSACTIONSTATUS == "SUCCESS")
                        {
                            emitraKisokResponse.COMMTYPE = request.emitrarequest.COMMTYPE;
                            emitraKisokResponse.USERNAME = request.emitrarequest.CONSUMERNAME;
                            resp = new PaymentResponse { Message = emitraKisokResponse.MSG, EmitraKioskResponse = emitraKisokResponse };
                        }
                        else
                        {
                            emitraKisokResponse.COMMTYPE = request.emitrarequest.COMMTYPE;
                            emitraKisokResponse.USERNAME = request.emitrarequest.CONSUMERNAME;
                            resp = new PaymentResponse { Status = "error", Status_Code = "0", Message = emitraKisokResponse.MSG, EmitraKioskResponse = emitraKisokResponse, };
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                resp = new PaymentResponse { Status = "error", Status_Code = "0", Message = ex.Message };
            }
            return resp;
        }
        #endregion

        private static PaymentRequestViewModel GetPaymentServiceDetail(PaymentViewModel model, string userType)
        {
            PaymentRequestViewModel lstRequest = new PaymentRequestViewModel();
            try
            {
                SqlParameter[] param = {
                                            new SqlParameter("ActionCode",model.ActionCode),
                                            new SqlParameter("EmitraServiceCode",model.emitraserviceid),
                                            new SqlParameter("EmitraHeadCode",model.EmitraHeadCode),
                                            new SqlParameter("UserType",userType),
                                            new SqlParameter("ParentID",model.parentid),
                                            new SqlParameter("RequestID",model.requestid),
                                            new SqlParameter("PayAmt",model.PayAmt)
                                        };

                FMDSS.Models.DAL dl = new Models.DAL();
                DataSet DS = new DataSet();
                dl.Fill(DS, "Sp_GetPaymentServiceDetail", param);
                if (Globals.Util.isValidDataSet(DS, 0, true))
                {
                    lstRequest.emitrarequest = Globals.Util.GetListFromTable<EmitraKioskRequest>(DS, 0).FirstOrDefault();
                }
                if (Globals.Util.isValidDataSet(DS, 1, true))
                {
                    lstRequest.Links = Globals.Util.GetListFromTable<EmitraLinks>(DS, 1).FirstOrDefault();
                }
                return lstRequest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DataTable GetSSODetail(string ssoid)
        {
            DataTable dtUserInfo = new DataTable();
            Designations objDesignation = new Designations();
            dtUserInfo = objDesignation.Select_SSODETAILS(ssoid);
            return dtUserInfo;
        }
    }
}