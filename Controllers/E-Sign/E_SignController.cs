using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.App_Start;
using FMDSS.E_SignIntegration;
using FMDSS.Models;
using System.IO;

namespace FMDSS.Controllers.E_Sign
{
    public class E_SignController : Controller
    {
        //
        // GET: /E_Sign/

        #region Send OTP and Upload .PAY file Emitra for zoo booking refund Process
        public JsonResult SendOTPEsignIntegration()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OtpResponce OtpResponce = new OtpResponce();
            try
            {
                #region Step 1 Send OTP
                OtpResponce = cls_ESignIntegrationbyEmitra.SendOTPbyEmitra();
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(OtpResponce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckOTP(string OTPNumber, string TransactionId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            clsVerifyOTPResponce OtpResponce = new clsVerifyOTPResponce();
            try
            {
                #region Step 2 Check OTP
                clsVerifyOTP VerifyOTP = new clsVerifyOTP();
                VerifyOTP.otp = OTPNumber;
                VerifyOTP.transactionid = TransactionId;
                OtpResponce = cls_ESignIntegrationbyEmitra.VerifyOTPAndGenrateTransationbyEmitra(VerifyOTP);
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(OtpResponce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenratePAYFile(string TransactionId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            clsDocumentESignByEmitraResponse OtpResponce = new clsDocumentESignByEmitraResponse();
            try
            {
                #region Step 3 Genrated E-sign .PAY File 

                clsDocumentESignByEmitra DocumentESignRequest = new clsDocumentESignByEmitra();
                DocumentESignRequest.transactionid = TransactionId;
                cls_eSign eSignRequest = new cls_eSign();
                string GetFilePath = "~/ESignUpload/TestFile";
                byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(GetFilePath + ".txt"));
                DocumentESignRequest.filecontant = Convert.ToBase64String(bytes);

                OtpResponce = cls_ESignIntegrationbyEmitra.GetTextFileEncrypted(DocumentESignRequest);

                using (FileStream stream = System.IO.File.Create(Server.MapPath(GetFilePath + ".PAY")))
                {
                    byte[] byteArray = Convert.FromBase64String(OtpResponce.file);
                    stream.Write(byteArray, 0, byteArray.Length);
                }
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(OtpResponce, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadPAYFile()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            clsUploadTextFileResponse FileUploadResponce = new clsUploadTextFileResponse();
            try
            {
                #region Step 4 Upload .PAY file on emitra

                clsUploadTextFile FileUploadRequest = new clsUploadTextFile();
                FileUploadRequest.merchantCode = "FOREST0716";
                FileUploadRequest.bankCode = "HDFC";
                FileUploadRequest.fileName = "TestFile";
                FileUploadRequest.remitterAccountNumber = "1245465646";
                FileUploadRequest.totalTransactionCount = "1";
                string GetFilePath = "~/ESignUpload/TestFile";
                byte[] bytes = System.IO.File.ReadAllBytes(Server.MapPath(GetFilePath + ".PAY"));
                FileUploadRequest.fileContent = Convert.ToBase64String(bytes);
                FileUploadResponce = cls_ESignIntegrationbyEmitra.UploadTextFile(FileUploadRequest, "6c4f9996-cebc-45d3-9fd7-babd69fab94e");
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(FileUploadResponce, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Send OTP for Genrate E-Sign 
        public JsonResult SendOTPESign(string RequestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OtpResponce OtpResponce = new OtpResponce();
            try
            {
                #region Step 1 Send OTP
                //OtpResponce = cls_ESignIntegration.SendOTPbyESign("937329232440", RequestId);
                //Session["AadharID"] = "937329232440";//"909731239310";//amit sir aadhar
                Session["AadharID"] = "523500041991";//"909731239310";//dinesh sir aadhar
                if (!string.IsNullOrEmpty(Convert.ToString(Session["AadharID"])))
                    OtpResponce = cls_ESignIntegration.SendOTPbyESign(Convert.ToString(Session["AadharID"]), RequestId);
                else
                {
                    OtpResponce.TransactionId = "";
                    OtpResponce.Status = "0";
                    OtpResponce.ErrorMessage = "Please Link Your Aadhar Card TO SSO";
                }
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return Json(OtpResponce, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region If PDF Not Genrate With E-Sign 

        public ActionResult PDFGenrateWithSign()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OtpResponce OtpResponce = new OtpResponce();
            PDFModel model = new PDFModel();
            try
            {
                model = GenratePDF.GetWithoutSignPdfList();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult PDFGenrateWithSign(PDFModel model, string OTP, string TransationID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            OtpResponce OtpResponce = new OtpResponce();
            try
            {
                #region Call E-Sign API
                clsVerifyOTP request = new clsVerifyOTP();
                request.otp = OTP;
                request.transactionid = TransationID;
                clsVerifyOTPResponce response = FMDSS.App_Start.cls_ESignIntegration.VerifyOTPAndGenrateTransation(request, model.GenratePDFModel.RequestId, "2", model.GenratePDFModel.TableName);

                if (!string.IsNullOrEmpty(response.TransactionId))
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> Genrate PDF with E-Sign </div>";
                else
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later " + response.ErrorMessage + ")</div>";
                #endregion
                //model = GenratePDF.GetWithoutSignPdfList();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("PDFGenrateWithSign");
        }
        #endregion

    }
}
