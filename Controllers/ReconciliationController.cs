using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FMDSS.Controllers
{
    public class ReconciliationController : Controller
    {
        //
        // GET: /Reconciliation/

        [HttpPost]
        public ActionResult Index(ReconciliationModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string request = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ReconciliationRepo repo = new ReconciliationRepo();
            try
            {
                DataSet dt = new DataSet();

                #region Request Log

                string Ipaddress = string.Empty;
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    Ipaddress = ipRange[0];
                }
                else
                {
                    Ipaddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }


                long ReconciliationLogID = repo.InsertReconciliationLog(controllerName + "/" + actionName, Ipaddress, request, UserID, string.Empty, string.Empty, 0, "Inserted");
                #endregion


                #region Emitra Log
                string EmitraRequest = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                EmitraReponse emitraresponse = repo.EmitraCancelationProcess(model.RefundModel.TokenNO, Convert.ToString(model.RefundModel.ServiceID), "SET", UserID);
                string EmitraResponse = Newtonsoft.Json.JsonConvert.SerializeObject(emitraresponse);

                #endregion

                #region Save Request And Response Log Table


                ReconciliationLogID = repo.InsertReconciliationLog(controllerName + "/" + actionName, string.Empty, string.Empty, 0, EmitraRequest, EmitraResponse, ReconciliationLogID, "UPDATED");


                #endregion
                if (emitraresponse.status.Trim().ToLower() == "success")
                {
                    #region Update Refund Tran STatus
                    dt = repo.UpdateRefundTran(model.RefundModel, UserID);

                    if (dt != null && dt.Tables != null)
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> Reconciliation Process has been Started. </div>";
                    }
                    else
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                    }
                    #endregion
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>" + emitraresponse.message + " </div>";
                }
            }
            catch (Exception ex)
            {
                repo.SendEmailFailureEmitraTransation(ex.InnerException + "_" + ex.StackTrace, request, model.RefundModel.RequestID);
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            return RedirectToAction("GetReconciliationList", "Reconciliation", new { FromDate = model.FromDate, ToDate = model.ToDate, ServiceID = model.RefundModel.ServiceID.ToString() });
        }

        public ActionResult GetReconciliationList(string FromDate, string ToDate, string ServiceID = "0")
        {
            ReconciliationModel model = new ReconciliationModel();
            model.ServiceID = ServiceID;
            model.ToDate = string.IsNullOrEmpty(ToDate) ? DateTime.Now.ToString("dd/MM/yyyy") : ToDate;
            model.FromDate = string.IsNullOrEmpty(FromDate) ? DateTime.Now.ToString("dd/MM/yyyy") : FromDate;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                #region Get Service List By Rajveer
                EmitraData OBJ = new EmitraData();
                DataSet DS1 = new DataSet();
                DS1 = OBJ.GetEmitraServiceDetailsWithParameter("GetServiceName", Convert.ToInt64(model.ServiceID), string.Empty, Convert.ToInt64(Session["UserID"]));
                if (DS1 != null && DS1.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ServiceList = new SelectList(DS1.Tables[0].AsDataView(), "Value", "Text");
                }
                else
                {
                    ViewBag.ServiceList = new List<SelectListItem>();

                }
                #endregion


                ReconciliationRepo repo = new ReconciliationRepo();
                DataSet DS = new DataSet();
                DS = repo.GetReconciliationData(model);

                if (DS != null)
                {
                    if (DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[0]);
                        model.ReconciliationMatch = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reconciliation>>(str);
                    }

                    if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        model.ReconciliationDiffrent = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reconciliation>>(str);
                    }

                    if (DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[2]);
                        model.EmritaListStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reconciliation>>(str);
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult GetReconciliationList(ReconciliationModel model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                #region Get Service List By Rajveer
                EmitraData OBJ = new EmitraData();
                DataSet DS1 = new DataSet();
                DS1 = OBJ.GetEmitraServiceDetailsWithParameter("GetServiceName", 0, string.Empty, Convert.ToInt64(Session["UserID"]));
                if (DS1 != null && DS1.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ServiceList = new SelectList(DS1.Tables[0].AsDataView(), "Value", "Text");
                }
                else
                {
                    ViewBag.ServiceList = new List<SelectListItem>();

                }
                #endregion

                ReconciliationRepo repo = new ReconciliationRepo();
                DataSet DS = new DataSet();
                DS = repo.GetReconciliationData(model);

                if (DS != null)
                {
                    if (DS.Tables[0] != null && DS.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[0]);
                        model.ReconciliationMatch = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reconciliation>>(str);
                    }

                    if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[1]);
                        model.ReconciliationDiffrent = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reconciliation>>(str);
                    }
                    if (DS.Tables[2] != null && DS.Tables[2].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DS.Tables[2]);
                        model.EmritaListStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Reconciliation>>(str);
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return View(model);
        }

        public ActionResult ReconciliationIndex()
        {
            #region Get Service List By Rajveer
            EmitraData OBJ = new EmitraData();
            DataSet DS = new DataSet();
            DS = OBJ.GetEmitraServiceDetailsWithParameter("GetServiceName", 0, string.Empty, Convert.ToInt64(Session["UserID"]));
            if (DS != null && DS.Tables[0].Rows.Count > 0)
            {
                ViewBag.ServiceList = new SelectList(DS.Tables[0].AsDataView(), "Value", "Text");
            }
            else
            {
                ViewBag.ServiceList = new List<SelectListItem>();

            }
            #endregion
            return View();
        }

        [HttpPost]
        public ActionResult ReconciliationIndex(string FromDate, string ToDate, string ServiceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            EmitraData OBJ = new EmitraData();
            try
            {
                #region Get Emitra Live Credentials Developed By Rajveer
                DataSet DS = new DataSet();

                //string ReconsilationURL = string.Empty;
                //string merchantCode = string.Empty;
                //string serviceId = string.Empty;
                //string allDetails = string.Empty;
                //string URL = ReconsilationURL + "?fromDate=" + DateTime.Now.AddDays(-1).ToShortDateString() + "&toDate=" + DateTime.Now.AddDays(-1).ToShortDateString() + "&merchantCode=" + merchantCode + "&serviceId=" + serviceId + "&allDetails=" + allDetails;
                string URL = string.Empty;


                DS = OBJ.GetEmitraServiceDetailsWithParameter("GetServiceUrl", Convert.ToInt64(ServiceID), "GET", Convert.ToInt64(Session["UserID"]));
                if (DS != null && DS.Tables[0].Rows.Count > 0)
                {
                    URL = Convert.ToString(DS.Tables[0].Rows[0]["URL"]).Replace("####", FromDate).Replace("$$$$", ToDate);
                    //ReconsilationURL = Convert.ToString(DS.Tables[0].Rows[0]["URL"]);
                    //merchantCode = Convert.ToString(DS.Tables[0].Rows[0]["MerchantCode"]);
                    //serviceId = Convert.ToString(DS.Tables[0].Rows[0]["ServiceID"]);
                    //allDetails = Convert.ToString(DS.Tables[0].Rows[0]["AllDetails"]);
                    //URL = ReconsilationURL + "?fromDate=" + FromDate + "&toDate=" + ToDate + "&merchantCode=" + merchantCode + "&serviceId=" + serviceId + "&allDetails=" + allDetails;
                }

                #endregion

                #region Insert Data  in SP_EmitraPaymentReconciliation From Emitra
                //string url = string.Format("https://emitraapp.rajasthan.gov.in/webServicesRepository/getPGDetails?fromDate=01/02/2017&toDate=28/02/2017&merchantCode=FOREST0117&serviceId=2239&allDetails=1");
                //string ReconsilationURL = System.Configuration.ConfigurationSettings.AppSettings["ReconsilationProcessInsertLIVEURL"].ToString();
                //string merchantCode = System.Configuration.ConfigurationSettings.AppSettings["merchantCode"].ToString();
                //string serviceId = System.Configuration.ConfigurationSettings.AppSettings["serviceId"].ToString();
                //string allDetails = System.Configuration.ConfigurationSettings.AppSettings["allDetails"].ToString();
                ////string URL = ReconsilationURL + "?fromDate=" + DateTime.Now.AddDays(-1).ToShortDateString() + "&toDate=" + DateTime.Now.AddDays(-1).ToShortDateString() + "&merchantCode=" + merchantCode + "&serviceId=" + serviceId + "&allDetails=" + allDetails;
                //string URL = ReconsilationURL + "?fromDate=" + FromDate + "&toDate=" + ToDate + "&merchantCode=" + merchantCode + "&serviceId=" + serviceId + "&allDetails=" + allDetails;

                #region Store URL in Text File
                //System.IO.File.AppendAllLines(Directory.GetCurrentDirectory() + "/ErrorLogFileReconciliationIndex.txt", new[] { DateTime.Now.ToString() + " URL IS :-" + URL });
               // System.IO.File.AppendAllLines(Server.MapPath("~/ErrorLogFileReconciliationIndex.txt"), new[] { DateTime.Now.ToString() + " URL IS :-" + URL });
                #endregion

                DataSet RefundDataSet = new DataSet();

                DataTable DT = new DataTable();
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(URL);
                    //EmitraData objResponse = (new JavaScriptSerializer()).Deserialize<EmitraData>(json);
                    //DT = JsoneConvertToTable.ToDataTable<EmitraResponseReconciliationProcessModel>(objResponse.data);

                    EmitraData objResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<EmitraData>(json);
                    DT = JsoneConvertToTable.ToDataTable<EmitraResponseReconciliationProcessModel>(objResponse.data);

                    DT.AcceptChanges();
                    RefundDataSet = OBJ.InsertDataRefundProcess(DT);
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> (Record form submited successfully Your Total Record  is :- " + DT.Rows.Count + ") </div>";
                }
                #endregion

                SMSandEMAILtemplate obj = new SMSandEMAILtemplate();
                if (RefundDataSet != null && RefundDataSet.Tables.Count > 0 && RefundDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < RefundDataSet.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            obj.SendMailComman("ALL", "ReconciliationInsertDataFromEmitraSuccess", DateTime.Now.ToString(), RefundDataSet.Tables[0].Rows[0]["msg"].ToString() + " Total Record Is:- " + DT.Rows.Count, string.Empty, string.Empty, string.Empty);
                        }
                        catch (Exception ex)
                        {
                            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
                        }
                    }
                }
                else
                {
                    obj.SendMailComman("ALL", "ReconciliationInsertDataFromEmitraError", DateTime.Now.ToString(), "Error Occur", string.Empty, string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return RedirectToAction("ReconciliationIndex");
        }
    }
}
