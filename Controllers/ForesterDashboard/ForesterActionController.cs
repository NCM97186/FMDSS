
//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : ManageAssetController
//  Description  : File contains calling functions from UI
//  Date Created : 18-Nov-2015
//  History      : 
//  Version      : 1.0
//  Author       : Rajkumar Singh
//  Modified By  : Rajkumar Singh
//  Modified On  : 12-Feb-2016
//  Reviewed By  : Ashok Yadav
//  Reviewed On  : 12-Feb-2016
//********************************************************************************************************


using FMDSS.Models.ForesterAction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using FMDSS.Models;
using FMDSS.Filters;
using System.Data.SqlTypes;
using System.Net;
using System.IO;
using System.Net.Mime;
using System.Configuration;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using FMDSS.Models.Home;
using FMDSS.E_SignIntegration;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForestProduction;
using IronPdf;
using FMDSS.Models.CitizenService.ProductionServices;

namespace FMDSS.Controllers.ForesterAction
{
    [MyAuthorization]
    [MyExceptionHandler]
    public class ForesterActionController : Controller
    {
        ActionRequest actionRequest = new ActionRequest();
        List<ActionRequest> actionRequestList = new List<ActionRequest>();
        List<ActionRequest> actionRequestList1 = new List<ActionRequest>();
        List<ActionRequest> actionRequestList2 = new List<ActionRequest>();
        List<SelectListItem> lstForestOfficer = new List<SelectListItem>();
        List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
        SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
        Common cm = new Common();
        private FmdssContext dbContext;
        public ForesterActionController()
        {
            dbContext = new FmdssContext();
        }
        DataTable dtforestofficer = new DataTable();
        DataTable dtOfficerDesignation = new DataTable();
        /// <summary>
        /// Returns view with details on dashboard
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ForesterDashboard()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            if (Session["CURRENT_Menus"] == null)
            {
                Home obj = new Home();
                Session["CURRENT_Menus"] = obj.GetCurrentMenus(Convert.ToInt16(Session["CURRENT_ROLE"]));
            }
            ForesterDashboardSummary OBJ = new ForesterDashboardSummary();
            try
            {
                DataSet DS = new DataSet();
                DataSet DT = new DataSet();
                DS = actionRequest.GetForestDashboardSummaryData(Convert.ToInt64(Session["UserId"]), Convert.ToString(Session["SSOid"]));
                DT = actionRequest.GetdashboardDetails(Convert.ToInt64(Session["UserId"]));
                OBJ.CitizenMyAction = Convert.ToString(DS.Tables[0].Rows.Count);
                OBJ.CitizenPendingRequests = Convert.ToString(DS.Tables[1].Rows.Count);
                OBJ.CitizenPendingAtDFOorDCF = Convert.ToString(DS.Tables[2].Rows.Count);
                OBJ.DevelopmentPendingRequests = Convert.ToString(DS.Tables[3].Rows.Count);
                OBJ.DevelopmentMyAction = Convert.ToString(DS.Tables[4].Rows.Count);
                OBJ.DevelopmentMyTransactions = Convert.ToString(DS.Tables[5].Rows.Count);
                OBJ.ProtectionMyAction = Convert.ToString(DS.Tables[6].Rows.Count);
                OBJ.ProtectionMyTransactions = Convert.ToString(DS.Tables[7].Rows.Count);
                OBJ.ProtectionPendingRequests = Convert.ToString(DS.Tables[8].Rows.Count);

                OBJ.CitizenTransitPermitPending = Convert.ToString(DT.Tables[0].Rows.Count);
                OBJ.CitizenTransitPermitApprove = Convert.ToString(DT.Tables[1].Rows.Count);
                OBJ.CitizenTransitPermitReject = Convert.ToString(DT.Tables[2].Rows.Count);


                USERDETAILS UOBJ = new USERDETAILS();
                UOBJ.Name = Convert.ToString(DS.Tables[9].Rows[0]["Name"]);
                UOBJ.Desig_Name = Convert.ToString(DS.Tables[9].Rows[0]["Desig_Name"]);
                UOBJ.Department = Convert.ToString(DS.Tables[9].Rows[0]["Department"]);
                UOBJ.OfficeName = Convert.ToString(DS.Tables[9].Rows[0]["OfficeName"]);
                UOBJ.Roles = Convert.ToString(DS.Tables[9].Rows[0]["Roles"]);
                UOBJ.Mobile = Convert.ToString(DS.Tables[9].Rows[0]["Mobile"]);
                UOBJ.Ssoid = Convert.ToString(DS.Tables[9].Rows[0]["Ssoid"]);
                OBJ.USERDETAILs = UOBJ;
                foreach (DataRow DR in DS.Tables[10].Rows)
                {
                    OBJ.CitizenModule = Convert.ToBoolean(DR["ModuleId1"]);
                    OBJ.DevelopmentModule = Convert.ToBoolean(DR["ModuleId2"]);
                    OBJ.ProductionModule = Convert.ToBoolean(DR["ModuleId3"]);
                    OBJ.ProtectionModule = Convert.ToBoolean(DR["ModuleId4"]);
                    OBJ.FinanceModule = Convert.ToBoolean(DR["ModuleId5"]);
                }
                OBJ.ProductionMyTransactions = Convert.ToString(DS.Tables[11].Rows.Count);
                if (DS.Tables.Count == 13)
                {
                    OBJ.ProductionMyAction = Convert.ToString(DS.Tables[12].Rows.Count);
                    OBJ.ProductionPendingRequests = "0";
                }
                else
                {
                }
                if (DS.Tables.Count == 14)
                {
                    OBJ.ProductionMyAction = Convert.ToString(DS.Tables[12].Rows.Count);
                    OBJ.ProductionPendingRequests = Convert.ToString(DS.Tables[13].Rows.Count);
                }
                else
                {
                    OBJ.ProductionMyAction = "0";
                    OBJ.ProductionPendingRequests = "0";
                }

                DataSet dsResearch = new DataSet();
                SqlParameter[] parameters = { new SqlParameter("@UserId", Convert.ToInt64(Session["UserId"])) };
                cm.Fill(dsResearch, "SpGetOtherNOCRequest", parameters);
                if (Globals.Util.isValidDataSet(dsResearch, 0, true))
                {
                    OBJ.ResearchPendingRequests = Convert.ToString(dsResearch.Tables[0].Rows.Count);
                }
                if (Globals.Util.isValidDataSet(dsResearch, 1, true))
                {
                    OBJ.ResearchMyAction = Convert.ToString(dsResearch.Tables[1].Rows.Count);
                }
                OBJ.ResearchModule = (Convert.ToInt32(OBJ.ResearchPendingRequests) > 0) || (Convert.ToInt32(OBJ.ResearchMyAction) > 0);


                #region Get Transation in Nursuey  Module Developed by Rajveer



                ProducePurchase obj_pp = new ProducePurchase();

                if (obj_pp.GetNurseryInchargeOrNot(Convert.ToInt64(Session["UserId"].ToString())) > 0)
                {
                    ViewData["NurseryInchargeOrNot"] = true;
                    List<ProducePurchase> result = new List<ProducePurchase>();
                    try
                    {
                        DataTable dt = new DataTable();
                        if (Session["UserId"] != null)
                        {
                            obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                            dt = obj_pp.Select_OnlinePurchaseHistoryDashboard();
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string OrderNo = string.Empty;

                            foreach (DataRow dr in dt.Rows)
                            {
                                ProducePurchase pp = new ProducePurchase();
                                pp.RequestID = Convert.ToString(dr["OrderNo"]);
                                pp.NurseryName = Convert.ToString(dr["NURSERY_NAME"].ToString());
                                pp.ProductName = dr["ProductName"].ToString();
                                pp.Quantity = Convert.ToInt64(dr["PurchaseQuantity"]);
                                pp.EmitraTransactionID = Convert.ToString(dr["EmitraTransactionID"]);
                                pp.SSOID = Convert.ToString(dr["SSOID"]);
                                result.Add(pp);
                            }
                            ViewData["NurseryDashboardDeptUserList"] = result;
                        }
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 1, DateTime.Now, Convert.ToInt64(Session["UserId"]));
            }
            return View(OBJ);
        }
        public JsonResult ForesterDashboardJson()
        {
            ForesterDashboardSummary OBJ = new ForesterDashboardSummary();
            try
            {
                DataSet DS = new DataSet();
                DS = actionRequest.GetForestDashboardSummaryData(Convert.ToInt64(Session["UserId"]), Convert.ToString(Session["SSOid"]));
                OBJ.CitizenMyAction = Convert.ToString(DS.Tables[0].Rows.Count);
                OBJ.CitizenPendingRequests = Convert.ToString(DS.Tables[1].Rows.Count);
                OBJ.CitizenPendingAtDFOorDCF = Convert.ToString(DS.Tables[2].Rows.Count);
                OBJ.DevelopmentPendingRequests = Convert.ToString(DS.Tables[3].Rows.Count);
                OBJ.DevelopmentMyAction = Convert.ToString(DS.Tables[4].Rows.Count);
                OBJ.DevelopmentMyTransactions = Convert.ToString(DS.Tables[5].Rows.Count);
                OBJ.ProtectionMyAction = Convert.ToString(DS.Tables[6].Rows.Count);
                OBJ.ProtectionMyTransactions = Convert.ToString(DS.Tables[7].Rows.Count);
                OBJ.ProtectionPendingRequests = Convert.ToString(DS.Tables[8].Rows.Count);
                //USERDETAILS UOBJ = new USERDETAILS();
                //UOBJ.Name = Convert.ToString(DS.Tables[9].Rows[0]["Name"]);
                //UOBJ.Desig_Name = Convert.ToString(DS.Tables[9].Rows[0]["Desig_Name"]);
                //UOBJ.Department = Convert.ToString(DS.Tables[9].Rows[0]["Department"]);
                //UOBJ.OfficeName = Convert.ToString(DS.Tables[9].Rows[0]["OfficeName"]);
                //UOBJ.Roles = Convert.ToString(DS.Tables[9].Rows[0]["Roles"]);
                //UOBJ.Mobile = Convert.ToString(DS.Tables[9].Rows[0]["Mobile"]);
                //UOBJ.Ssoid = Convert.ToString(DS.Tables[9].Rows[0]["Ssoid"]);
                //OBJ.USERDETAILs = UOBJ;
                foreach (DataRow DR in DS.Tables[11].Rows)
                {
                    OBJ.CitizenModule = Convert.ToBoolean(DR["ModuleId1"]);
                    OBJ.DevelopmentModule = Convert.ToBoolean(DR["ModuleId2"]);
                    OBJ.ProductionModule = Convert.ToBoolean(DR["ModuleId3"]);
                    OBJ.ProtectionModule = Convert.ToBoolean(DR["ModuleId4"]);
                    OBJ.FinanceModule = Convert.ToBoolean(DR["ModuleId5"]);
                }
                OBJ.ProductionMyTransactions = Convert.ToString(DS.Tables[11].Rows.Count);
                if (DS.Tables.Count == 13)
                {
                    OBJ.ProductionMyAction = Convert.ToString(DS.Tables[12].Rows.Count);
                    OBJ.ProductionPendingRequests = "0";
                }
                else
                {
                }
                if (DS.Tables.Count == 14)
                {
                    OBJ.ProductionMyAction = Convert.ToString(DS.Tables[12].Rows.Count);
                    OBJ.ProductionPendingRequests = Convert.ToString(DS.Tables[13].Rows.Count);
                }
                else
                {
                    OBJ.ProductionMyAction = "0";
                    OBJ.ProductionPendingRequests = "0";
                }
            }
            catch (Exception ex)
            {
            }
            return Json(OBJ, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ForesterAction(string ServiceType, string Tab = "")
        {
            try
            {
                //ViewData["ActiveTab"] = Encryption.decrypt(Tab);
                if (Session["Servicetype"] == null && Encryption.decrypt(ServiceType) == null)
                {
                    Session["Servicetype"] = "Citizen";
                }
                if (Encryption.decrypt(ServiceType) != null)
                {
                    Session["Servicetype"] = Encryption.decrypt(ServiceType);
                }
                if (Session["Servicetype"].ToString() == "Production")
                {
                    #region Bind allrecord to table
                    DataSet dtf2 = actionRequest.BindAProductionServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("1"));
                    for (int i = 0; i < dtf2.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf2.Tables[i].Rows)
                        {
                            actionRequestList.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["trn_status_Desc"].ToString()
                                });
                        }
                    }
                    DataSet dtf3 = actionRequest.BindAProductionServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("2"));
                    for (int i = 0; i < dtf3.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf3.Tables[i].Rows)
                        {
                            actionRequestList1.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["trn_status_Desc"].ToString()
                                });
                        }
                    }
                    DataSet dtTransProduction = actionRequest.BindProductionTransactionRequest(Convert.ToInt32(Session["UserId"]));
                    for (int i = 0; i < dtTransProduction.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtTransProduction.Tables[i].Rows)
                        {
                            actionRequestList2.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleName = dr["ModuleDesc"].ToString(),
                                });
                        }
                    }
                    ViewData["productionActionRequestList"] = actionRequestList;
                    ViewData["productionActionRequestList1"] = actionRequestList1;
                    ViewData["productionActionRequestList2"] = actionRequestList2;
                    #endregion
                }
                else if (Session["Servicetype"].ToString() == "Development")
                {
                    #region Bind All record To Table
                    DataSet dtDevlop = actionRequest.BindDevelopmentServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("1"));
                    for (int i = 0; i < dtDevlop.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtDevlop.Tables[i].Rows)
                        {
                            actionRequestList.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["Status"].ToString()
                                });
                        }
                    }
                    DataSet dtf3 = actionRequest.BindDevelopmentServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("2"));
                    for (int i = 0; i < dtf3.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf3.Tables[i].Rows)
                        {
                            actionRequestList1.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["Status"].ToString()
                                });
                        }
                    }
                    DataSet dtTransaction = actionRequest.BindDevelopmentTransactionRequest(Convert.ToInt32(Session["UserId"]));
                    for (int i = 0; i < dtTransaction.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtTransaction.Tables[i].Rows)
                        {
                            actionRequestList2.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleName = dr["ModuleDesc"].ToString(),
                                    //ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    //ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    //PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    //SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    //TransactionStatus = dr["Status"].ToString()
                                });
                        }
                    }
                    ViewData["DevelopmentActionRequestList"] = actionRequestList;
                    ViewData["DevelopmentActionRequestList1"] = actionRequestList1;
                    ViewData["DevelopmentActionRequestList2"] = actionRequestList2;
                    #endregion
                }
                else if (Session["Servicetype"].ToString() == "Protection")
                {
                    #region Bind All record To Table
                    DataSet dtProtection = actionRequest.BindProtectionServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("1"));
                    for (int i = 0; i < dtProtection.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtProtection.Tables[i].Rows)
                        {
                            actionRequestList.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["Status"].ToString(),
                                    OffenseCode = dr["Requestedid"].ToString()
                                });
                        }
                    }
                    DataSet dtfx = actionRequest.BindProtectionRODecision(Convert.ToInt32("2"));
                    // DataSet dtf3 = actionRequest.BindProtectionServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("2"));
                    for (int i = 0; i < dtfx.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtfx.Tables[i].Rows)
                        {
                            actionRequestList1.Add(
                                new ActionRequest()
                                {
                                    OffenseCode = dr["Offense_code"].ToString(),
                                    Place = dr["Place_of_offense"].ToString(),
                                    AppearanceDate = dr["date_of_offense"].ToString(),
                                    Time_of_offense = dr["Time_of_offense"].ToString(),
                                    InvestigatingOfficer = dr["name"].ToString(),
                                    Completiondate = dr["InvestigationCompleteDate"].ToString(),
                                    Status = dr["DisplayText"].ToString()
                                });
                        }
                    }
                    //DataSet dtf3 = actionRequest.BindProtectionServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("2"));
                    //for (int i = 0; i < dtf3.Tables.Count; i++)
                    //{
                    //    foreach (DataRow dr in dtf3.Tables[i].Rows)
                    //    {
                    //        actionRequestList1.Add(
                    //            new ActionRequest()
                    //            {
                    //                RequestId = dr["Requestedid"].ToString(),
                    //                ServiceType = dr["ServiceTypeDesc"].ToString(),
                    //                PermissionName = dr["PermissionDesc"].ToString(),
                    //                SubPermissionName = dr["SubPermissionDesc"].ToString(),
                    //                RequestedOn = dr["RequestedOn"].ToString(),
                    //                Status = dr["StatusDesc"].ToString(),
                    //                TableName = dr["TableName"].ToString(),
                    //                ModuleId = Convert.ToInt32(dr["ModuleId"]),
                    //                ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                    //                PermissionId = Convert.ToInt32(dr["PermissionId"]),
                    //                SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                    //                TransactionStatus = dr["Status"].ToString(),
                    //                OffenseCode = dr["OffenseCode"].ToString()
                    //            });
                    //    }
                    //}
                    DataSet dtf4 = actionRequest.BindProtectionServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("3"));
                    for (int i = 0; i < dtf4.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf4.Tables[i].Rows)
                        {
                            actionRequestList2.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["Status"].ToString(),
                                    OffenseCode = dr["Requestedid"].ToString()
                                });
                        }
                    }
                    ViewData["ProtectionActionRequestList"] = actionRequestList;
                    ViewData["ProtectionActionRequestList1"] = actionRequestList1;
                    ViewData["ProtectionActionRequestList2"] = actionRequestList2;
                    #endregion
                }
                else
                {
                    #region Bind All record To Table
                    DataSet dtf = actionRequest.BindAllServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("1"));
                    for (int i = 0; i < dtf.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf.Tables[i].Rows)
                        {
                            actionRequestList.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["trn_status_Desc"].ToString(),
                                    //Added by Vandana Gupta to implement SLA CR on 07-Jul-2016
                                    LastDateofAction = Convert.ToString(dr["ActionLastDate"]),
                                    PendingSinceDays = Convert.ToString(dr["PendingSinceDays"]),
                                    //Added by Rajveer Sharma to implement Hide show Inpection and survey report link in CTP on 18-Jul-2017
                                    InpectionDate = Convert.ToString(dr["InspectionDate"]),
                                    SurveyReportCount = Convert.ToInt32(dr["surveyCount"]),
                                });
                        }
                    }
                    DataSet dtf1 = actionRequest.BindAllServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("2"));
                    for (int i = 0; i < dtf1.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf1.Tables[i].Rows)
                        {
                            actionRequestList1.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["trn_status_Desc"].ToString()
                                });
                        }
                    }
                    DataSet dtf2 = actionRequest.BindAllServiceRequest(Convert.ToInt32(Session["UserId"]), Convert.ToInt32("3"));
                    for (int i = 0; i < dtf2.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf2.Tables[i].Rows)
                        {
                            actionRequestList2.Add(
                                new ActionRequest()
                                {
                                    RequestId = dr["Requestedid"].ToString(),
                                    ServiceType = dr["ServiceTypeDesc"].ToString(),
                                    PermissionName = dr["PermissionDesc"].ToString(),
                                    SubPermissionName = dr["SubPermissionDesc"].ToString(),
                                    RequestedOn = dr["RequestedOn"].ToString(),
                                    Status = dr["StatusDesc"].ToString(),
                                    TableName = dr["TableName"].ToString(),
                                    ModuleId = Convert.ToInt32(dr["ModuleId"]),
                                    ServiceTypeId = Convert.ToInt32(dr["ServiceTypeId"]),
                                    PermissionId = Convert.ToInt32(dr["PermissionId"]),
                                    SubPermissionId = Convert.ToInt32(dr["SubPermissionId"]),
                                    TransactionStatus = dr["trn_status_Desc"].ToString(),
                                    //Added by Vandana Gupta to implement SLA CR on 07-Jul-2016
                                    LastDateofAction = Convert.ToString(dr["ActionLastDate"]),
                                    PendingSinceDays = Convert.ToString(dr["PendingSinceDays"])
                                });
                        }
                    }
                    ViewData["actionRequestList"] = actionRequestList;
                    ViewData["actionRequestList1"] = actionRequestList1;
                    ViewData["actionRequestList2"] = actionRequestList2;
                    #endregion
                }
                //if (Session["DesignationId"].ToString() == "5" || Session["DesignationId"].ToString() == "6" || Session["DesignationId"].ToString() == "7")
                //{
                dtOfficerDesignation = actionRequest.GetOfficerDesignation();
                if (dtOfficerDesignation.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                    {
                        lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                    }
                    ViewBag.OfficerDesignation = lstOfficerDesignation;
                }
                else
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = "NA", Value = "0" });
                    ViewBag.OfficerDesignation = lstOfficerDesignation;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View();
        }
        public ActionResult OtherNOC()
        {
            if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) > 0)
            {
                DataSet dsResearch = new DataSet();
                SqlParameter[] parameters = { new SqlParameter("@UserId", Convert.ToInt64(Session["UserId"])) };
                cm.Fill(dsResearch, "SpGetOtherNOCRequest", parameters);
                ViewBag.NOCPending = ToNocList(dsResearch.Tables[0]);
                ViewBag.NOCMyAction = ToNocList(dsResearch.Tables[1]);
                string usertype = string.Empty;
                if (dsResearch.Tables[2] != null && dsResearch.Tables[2].Rows.Count > 0)
                {

                    DataRow[] drWildlife = dsResearch.Tables[2].Select("RuleType='GOIGOR' OR RuleType='CWLW'");
                    DataRow[] drForest = dsResearch.Tables[2].Select("RuleType='GOIGORF' OR RuleType='Forest'");

                    if (drWildlife.Count() > 0)
                        usertype = "Wildlife";
                    if (drForest.Count() > 0)
                        usertype = "Forest";
                    if (drWildlife.Count() > 0 && drForest.Count() > 0)
                        usertype = "Both";

                }
                Session["UserType"] = usertype;

                return View();
            }
            else
            {
                return Redirect(Url.Content("~/"));
            }
        }
        public ActionResult GetNocDetail(string requestId)
        {
            string origin = System.Configuration.ConfigurationManager.AppSettings["websiteUrl"].ToString() + "/PermissionDocument";
            DataSet ds = new DataSet();
            SqlParameter[] param = {
                new SqlParameter("@RequestId", requestId),
                new SqlParameter("@UserID",Convert.ToInt64(Session["UserId"]))
            };
            cm.Fill(ds, "KN_GetResearchReqDetail", param);
            DataTable tbl = ds.Tables[0];
            StringBuilder sb = new StringBuilder();
            StringBuilder sbTrail = new StringBuilder();
            StringBuilder sbCmd = new StringBuilder();
            Dictionary<string, string> cmds = new Dictionary<string, string>();
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataColumn dc in tbl.Columns)
                {
                    string colVal = Convert.ToString(tbl.Rows[0][dc.ColumnName]);
                    if (!string.IsNullOrEmpty(colVal))
                    {
                        if (dc.ColumnName.Contains(" File"))
                        {
                            var lnk = "<a href='" + origin + "/" + colVal + "' target='_blank' rel='noopener noreferrer'> Download </a>";
                            sb.Append("<tr><td>" + dc.ColumnName + "</td><td>" + lnk + "</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td>" + dc.ColumnName + "</td><td>" + colVal + "</td></tr>");
                        }
                    }
                }
                DataTable tblTrail = ds.Tables[1];
                sbTrail.Append("<thead>");
                foreach (DataColumn trailDc in tblTrail.Columns)
                {
                    sbTrail.Append("<th>" + trailDc.ColumnName + "</th>");
                }
                sbTrail.Append("</thead><tbody>");
                foreach (DataRow dr in tblTrail.Rows)
                {
                    sbTrail.Append("<tr>");
                    for (int i = 0; i < tblTrail.Columns.Count; i++)
                    {
                        sbTrail.Append("<td>" + dr[i] + "</td>");
                    }
                    sbTrail.Append("</tr>");
                }
                sbTrail.Append("</tbody>");
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    Session["UploadFile"] = null;
                    cmds.Add("cmd", ds.Tables[2].Rows[0]["Level"].ToString());
                    cmds.Add("cmdText", ds.Tables[2].Rows[0]["NextStep"].ToString());
                    cmds.Add("CurrentApprovalLevel", ds.Tables[2].Rows[0]["CurrentApprovalLevel"].ToString());
                }

            }
            else
            {
                cmds.Add("cmd", "0");
                cmds.Add("cmdText", "None");
            }
            ViewBag.Data = ds;
            return PartialView("_ResearchWorkflowDetails");
        }
        [HttpPost]
        public ActionResult ReviewReq(NOCApprovalDetails model, string OTP, string TransationID)
        {
            var attachedDoc = new List<Entity.ViewModel.CommonDocument>();

            if (Session["UploadFile"] != null)
            {
                attachedDoc = ((List<Entity.ViewModel.CommonDocument>)Session["UploadFile"]).Where(x => x.IsNew).ToList();
                new Repository.CommonRepository().SaveDocs(model.ResearchID, 4, attachedDoc);
            }

            SqlParameter[] parameters = {
                new SqlParameter("@RequestId", model.reqid),
                new SqlParameter("@UserId", Convert.ToInt64(Session["UserId"])),
                new SqlParameter("@Action",model.cmd ),
                new SqlParameter("@Comments",model.Comments ),
                new SqlParameter("@PresDate",DBNull.Value),
                new SqlParameter("@GOILetterNo",model.GOILetterNo),
                new SqlParameter("@GOIResponseNo",model.GOIResponseNo),
                new SqlParameter("@GORLetterNo",model.GORLetterNo),
                new SqlParameter("@xmlFile", new Repository.CommonRepository().GetTempDocInXML()),
            };
            if (!string.IsNullOrEmpty(model.presDate))
                parameters[4].Value = DateTime.ParseExact(model.presDate, "dd/MM/yyyy", null);
            //if (permFile != null && permFile.ContentLength > 0)
            //{
            //    string fileName = Path.GetFileName(permFile.FileName);
            //    string FileFullName = DateTime.Now.Ticks + "_" + fileName;
            //    permFile.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
            //    parameters[5].Value = FileFullName;
            //}

            //if (GOIResponseLetterPath != null && GOIResponseLetterPath.ContentLength > 0)
            //{
            //    string fileName = Path.GetFileName(GOIResponseLetterPath.FileName);
            //    string FileFullName = DateTime.Now.Ticks + "_" + fileName;
            //    GOIResponseLetterPath.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
            //    parameters[6].Value = FileFullName;
            //}
            DataTable ds = new DataTable();
            //if (model.cmdText.Trim().ToLower() == "approved" && !string.IsNullOrEmpty(OTP) && !string.IsNullOrEmpty(TransationID))
            //{
            //    cm.Fill(ds, "SPRESEARCHREVIEW", parameters);
            //}
            //else if (model.cmdText.Trim().ToLower() != "approved")
            //{
            cm.Fill(ds, "SPRESEARCHREVIEW", parameters);
            //}
            if (ds != null && ds.Rows.Count > 0)
            {


                Session["msg"] = "Request status updated successfully";

                #region E_Sign
                if (model.cmdText.Trim().ToLower() == "approved")
                {
                    //#region Call E-Sign API
                    //clsVerifyOTP request = new clsVerifyOTP();
                    //request.otp = OTP;
                    //request.transactionid = TransationID;
                    ////clsVerifyOTPResponce response = FMDSS.App_Start.cls_ESignIntegration.VerifyOTPAndGenrateTransation(request, Convert.ToString(model.reqid) , model.cmdText.Trim(), Session["TableName"].ToString());
                    //clsVerifyOTPResponce response = FMDSS.App_Start.cls_ESignIntegration.VerifyOTPAndGenrateTransation(request, Convert.ToString(model.reqid), model.cmdText.Trim(), "tbl_ResearchStudyPermissions");
                    //if (!string.IsNullOrEmpty(response.TransactionId))
                    //    TempData["Approve"] = "Request Id:" + Convert.ToString(model.reqid) + " has been Approved Sucessfully and Genrated PDF with E-Sign";
                    //else
                    //    TempData["Approve"] = "Request Id:" + Convert.ToString(model.reqid) + " has been Approved Sucessfully but not Genrated PDF with E-Sign";
                    //#endregion
                }
                #endregion


                #region "User Send Email"
                decimal Final_Amount = 0;
                string UserName = ds.Rows[0]["Name"].ToString();
                string UserEmail = ds.Rows[0]["Emailid"].ToString();
                string UserMobile = ds.Rows[0]["Mobile"].ToString();



                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                if (model.cmdText.Trim().ToLower() == "set presentation date")
                    objSMSandEMAILtemplate.SendMailComman("ALL", "ResearchTemplateSETPresentation", model.reqid, UserName, UserEmail, UserMobile, model.cmdText, model.presDate, model.Comments);
                else if (model.cmdText.Trim().ToLower() == "presentation")
                    objSMSandEMAILtemplate.SendMailComman("ALL", "ResearchTemplateAfterPresentation", model.reqid, UserName, UserEmail, UserMobile, model.cmdText, null, model.Comments);
                else
                    objSMSandEMAILtemplate.SendMailComman("ALL", "ResearchTemplate", model.reqid, UserName, UserEmail, UserMobile, model.cmdText);
                #endregion
            }
            else
            { Session["msg"] = "Error occured"; }
            return RedirectToAction("OtherNoc");
        }
        private List<NocList> ToNocList(DataTable dt)
        {
            List<NocList> lst = new List<NocList>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    NocList nobj = new NocList();
                    nobj.ResearchID = (long)(dr["ResearchID"] ?? 0);
                    nobj.RequestId = (string)(dr["RequestId"] ?? "");
                    nobj.NocType = (string)(dr["NocType"] ?? "");
                    nobj.Status = (string)(dr["Status"] ?? "");
                    nobj.ActionTakenUser = (string)(dr["ActionTakenUser"] ?? "N/A");
                    nobj.StatusId = Convert.ToInt32(dr["StatusId"]);
                    nobj.ReqDate = (string)(dr["ReqDate"] ?? "");
                    nobj.Level = Convert.ToInt32(dr["Level"]);
                    lst.Add(nobj);
                }
            }
            return lst;
        }
        [HttpPost]
        public JsonResult getForestOfficer(string designation)
        {
            List<SelectListItem> lstOfficer = new List<SelectListItem>();
            try
            {
                DAL dal = new DAL();
                DataSet dsOfficerDesig = new DataSet();
                SqlParameter[] paramBlock = { new SqlParameter("@option", "2"),
                                             new SqlParameter("@ssoid", Session["SSOid"]),
                                              new SqlParameter("@EmpDesig", designation),
                                            };
                dal.Fill(dsOfficerDesig, "Sp_Dashboard_GetFOfficerDesig", paramBlock);
                ViewBag.fname = dsOfficerDesig.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    if (@dr["SSO_ID"].ToString() != "--Select--")
                    {
                        lstOfficer.Add(new SelectListItem { Text = @dr["SSO_ID"].ToString(), Value = @dr["SSO_ID"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error " + ex);
            }
            return Json(new SelectList(lstOfficer, "Value", "Text"));
        }
        /// <summary>
        /// Action used for get details of requestid to view action result
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="TableName"></param>
        /// <param name="ModuleId"></param>
        /// <param name="ServiceTypeId"></param>
        /// <param name="PermissionId"></param>
        /// <param name="SubPermissionId"></param>
        /// <returns></returns>
        [MyAuthorization]
        public ActionResult ForesterStatus(string RequestId, string TableName, string ModuleId, string ServiceTypeId, string PermissionId, string SubPermissionId, string Reviewer, string OffenseCode = null)
        {
            ActionRequest ar = new ActionRequest();
            DataTable dt = new DataTable();
            try
            {
                #region Model binding
                RequestId = Encryption.decrypt(RequestId);
                TableName = Encryption.decrypt(TableName);
                ModuleId = Encryption.decrypt(ModuleId);
                ServiceTypeId = Encryption.decrypt(ServiceTypeId);
                PermissionId = Encryption.decrypt(PermissionId);
                SubPermissionId = Encryption.decrypt(SubPermissionId);
                Reviewer = Encryption.decrypt(Reviewer);
                // OffenseCode = Encryption.decrypt(OffenseCode);
                if (OffenseCode != null)
                {
                    Session["OffenseCode"] = OffenseCode;
                }
                else
                {
                    Session["OffenseCode"] = "";
                }
                if (RequestId != null && TableName != null && ModuleId != null && ServiceTypeId != null && PermissionId != null && SubPermissionId != null)
                {
                    ar.ModuleId = Convert.ToInt32(ModuleId);
                    ar.ServiceTypeId = Convert.ToInt32(ServiceTypeId);
                    ar.PermissionId = Convert.ToInt32(PermissionId);
                    ar.SubPermissionId = Convert.ToInt32(SubPermissionId);
                    Session["RequestId"] = RequestId;
                    Session["ModuleId"] = ar.ModuleId;
                    Session["ServiceTypeId"] = ar.ServiceTypeId;
                    Session["PermissionId"] = ar.PermissionId;
                    Session["SubPermissionId"] = ar.SubPermissionId;
                    Session["TableName"] = TableName;
                    ar.IsReviewer = ar.IdentifyApproveReview(RequestId);
                    //ar.IsReviewer = Convert.ToBoolean(Reviewer);
                    Session["IsReviewer"] = ar.IsReviewer;
                    // int actId = Convert.ToInt32(actionId);   //Only rejected reason are binded as per clint request
                    DataTable ds = ar.BindReasonList(4); // Reject Reasons
                    ViewBag.fname = ds;
                    List<SelectListItem> items = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });
                    }
                    ViewBag.ReasonType = items;
                    ds = ar.BindReasonList(3);// Re-Assign Reasons
                    items = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in ds.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });
                    }
                    ViewBag.ReasonType1 = items;
                    DataTable dtdata = new DataTable();
                    dtdata = ar.BindActionList(RequestId, TableName);
                    if (TableName.Trim() == "Tbl_Citizen_TransitPermit")
                    {
                        ar.ModuleName = dtdata.Rows[0]["Module"].ToString();
                        ar.ServiceType = dtdata.Rows[0]["Service"].ToString();
                        ar.PermissionType = dtdata.Rows[0]["Permission"].ToString();
                        ar.PermissionName = dtdata.Rows[0]["SubPermission"].ToString();
                        ar.DurationsFrom = string.IsNullOrEmpty(ar.DurationsFrom) ? DateTime.Now.ToString("dd/MM/yyyy") : ar.DurationsFrom;
                        ar.DurationsTo = string.IsNullOrEmpty(ar.DurationsTo) ? DateTime.Now.ToString("dd/MM/yyyy") : ar.DurationsTo; ;
                    }
                    else
                    {
                        ar.ModuleName = dtdata.Rows[0]["ModuleDesc"].ToString();
                        ar.ServiceType = dtdata.Rows[0]["ServiceTypeDesc"].ToString();
                        ar.PermissionType = dtdata.Rows[0]["PermissionDesc"].ToString();
                        ar.PermissionName = dtdata.Rows[0]["SubPermissionDesc"].ToString();
                    }
                    Session["PermissionDesc"] = ar.PermissionType;
                    if (dtdata.Columns.Contains("EnteredOn"))
                    {
                        DateTime _date;
                        _date = DateTime.Parse(dtdata.Rows[0]["EnteredOn"].ToString());
                        ar.RequestedOn = _date.ToString("dd-MM-yyyy");
                    }
                    else if (dtdata.Columns.Contains("Created_Date"))
                    {
                        DateTime _date;
                        _date = DateTime.Parse(dtdata.Rows[0]["Created_Date"].ToString());
                        ar.RequestedOn = _date.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        ar.RequestedOn = "";
                    }
                    if (dtdata.Columns.Contains("DurationFrom"))
                    {
                        DateTime _date1, _date2;
                        _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                        _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                        ar.DurationsFrom = _date1.ToString("dd/MM/yyyy");
                        ar.DurationsTo = _date2.ToString("dd/MM/yyyy");
                        ar.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                    }
                    else if (dtdata.Columns.Contains("StartDate"))
                    {
                        DateTime _date1, _date2;
                        _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                        _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                        ar.DurationsFrom = _date1.ToString("dd/MM/yyyy");
                        ar.DurationsTo = _date2.ToString("dd/MM/yyyy");
                        ar.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                    }
                    else if (dtdata.Columns.Contains("FinanceYear"))
                    {
                        ar.FinancialYear = dtdata.Rows[0]["FinanceYear"].ToString();
                    }
                    else
                    {
                        ar.Durations = "";
                    }
                    if (dtdata.Columns.Contains("EstimatedAmount"))
                    {
                        ar.EstimatedAmount = dtdata.Rows[0]["EstimatedAmount"].ToString();
                    }
                    if (dtdata.Columns.Contains("Name") && dtdata.Columns.Contains("Desig_Alias"))
                    {
                        ar.RequestedBy = dtdata.Rows[0]["Name"].ToString() + "(" + dtdata.Rows[0]["Desig_Alias"].ToString() + ")";
                    }
                    if (string.IsNullOrEmpty(ar.RequestedBy))
                    {
                        if (dtdata.Columns.Contains("UserName"))
                        {
                            ar.RequestedBy = dtdata.Rows[0]["UserName"].ToString();
                        }
                    }
                    if (dtdata.Columns.Contains("UserName"))
                    {
                        ar.RequestedBy = dtdata.Rows[0]["UserName"].ToString();
                    }
                    if (dtdata.Columns.Contains("trn_Status_Code"))
                    {
                        if (dtdata.Rows[0]["trn_Status_Code"].ToString() == "1")
                            ar.Payment = "True";
                    }
                    else
                    {
                        ar.PaidAmount = 0;
                    }
                    #region FilmShooting
                    if (dtdata.Columns.Contains("DepositeAmount"))
                    {
                        ar.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["DepositeAmount"]) + Convert.ToDecimal(dtdata.Rows[0]["TotalFees"]);
                    }
                    if (dtdata.Columns.Contains("Fees"))
                    {
                        ar.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["Fees"]);
                    }
                    if (dtdata.Columns.Contains("Final_Amount"))
                    {
                        ar.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["Final_Amount"]);
                    }
                    else
                    {
                        ar.PaidAmount = 0;
                    }
                    if (dtdata.Columns.Contains("NumberOfCrewMembers"))
                    {
                        ar.NumberOfPerson = dtdata.Rows[0]["NumberOfCrewMembers"].ToString();
                    }
                    if (dtdata.Columns.Contains("No_Of_Member"))
                    {
                        ar.NumberOfPerson = dtdata.Rows[0]["No_Of_Member"].ToString();
                    }
                    else
                    {
                        ar.NumberOfPerson = "1";
                    }
                    if (dtdata.Columns.Contains("ApplicantType"))
                    {
                        if (dtdata.Rows[0]["ApplicantType"].ToString() == "1")
                        {
                            ar.ApplicationType = "Individual";
                        }
                        else
                        {
                            ar.ApplicationType = "Organizational";
                        }
                    }
                    if (dtdata.Columns.Contains("Title"))
                    {
                        ar.FilmTitle = dtdata.Rows[0]["Title"].ToString();
                    }
                    if (dtdata.Columns.Contains("ShootingPurpose"))
                    {
                        ar.ShootingPurpose = dtdata.Rows[0]["ShootingPurpose"].ToString();
                    }
                    if (dtdata.Columns.Contains("IdentityProofNo"))
                    {
                        ar.IdProofNo = dtdata.Rows[0]["IdentityProofNo"].ToString();
                    }
                    if (dtdata.Columns.Contains("IdProofNo"))
                    {
                        ar.IdProofNo = dtdata.Rows[0]["IdProofNo"].ToString();
                    }
                    #endregion
                    #region Organizing Camp
                    if (dtdata.Columns.Contains("Applicant_Type"))
                    {
                        if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                        {
                            ar.ApplicationType = "Individual";
                        }
                        else
                        {
                            ar.ApplicationType = "Organizational";
                        }
                    }
                    if (dtdata.Columns.Contains("Purpose_OCM"))
                    {
                        ar.ShootingPurpose = dtdata.Rows[0]["Purpose_OCM"].ToString();
                    }
                    if (dtdata.Columns.Contains("NoOfDays"))
                    {
                        ar.NumberOfDay = dtdata.Rows[0]["NoOfDays"].ToString();
                    }
                    #endregion
                    #region Manage Notice
                    if (dtdata.Columns.Contains("Notice_Number"))
                    {
                        ar.Notice_No = dtdata.Rows[0]["Notice_Number"].ToString();
                    }
                    if (dtdata.Columns.Contains("REG_NAME"))
                    {
                        ar.Region_Name = dtdata.Rows[0]["REG_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("CIRCLE_NAME"))
                    {
                        ar.Circle_Name = dtdata.Rows[0]["CIRCLE_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        ar.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("RANGE_NAME"))
                    {
                        ar.Range_Name = dtdata.Rows[0]["RANGE_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("Depot_Name"))
                    {
                        ar.Depot_Name = dtdata.Rows[0]["Depot_Name"].ToString();
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        ar.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                    }
                    if (dtdata.Columns.Contains("ProductName"))
                    {
                        ar.Product_Name = dtdata.Rows[0]["ProductName"].ToString();
                    }
                    if (dtdata.Columns.Contains("UnitName"))
                    {
                        ar.Produce_Unit = dtdata.Rows[0]["UnitName"].ToString();
                    }
                    if (dtdata.Columns.Contains("Quantity"))
                    {
                        ar.Qty = dtdata.Rows[0]["Quantity"].ToString();
                    }
                    if (dtdata.Columns.Contains("ReservedPrice"))
                    {
                        ar.ReservedPrice = dtdata.Rows[0]["ReservedPrice"].ToString();
                    }
                    #endregion
                    if (dtdata.Columns.Contains("DIST_NAME"))
                    {
                        ar.District = dtdata.Rows[0]["DIST_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("PlaceName"))
                    {
                        ar.Place = dtdata.Rows[0]["PlaceName"].ToString();
                    }
                    if (dtdata.Columns.Contains("BLK_NAME"))
                    {
                        ar.Block = dtdata.Rows[0]["BLK_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("GP_NAME"))
                    {
                        ar.GramPanchayat = dtdata.Rows[0]["GP_NAME"].ToString();
                    }
                    if (dtdata.Columns.Contains("Qualification"))
                    {
                        ar.Qualification = dtdata.Rows[0]["Qualification"].ToString();
                    }
                    if (dtdata.Columns.Contains("College"))
                    {
                        ar.College = dtdata.Rows[0]["College"].ToString();
                    }
                    if (dtdata.Columns.Contains("R_Subject"))
                    {
                        ar.ResearchSubject = dtdata.Rows[0]["R_Subject"].ToString();
                    }
                    if (dtdata.Columns.Contains("R_Procedure"))
                    {
                        ar.ResearchProcedure = dtdata.Rows[0]["R_Procedure"].ToString();
                    }
                    if (dtdata.Columns.Contains("AnimalCategory"))
                    {
                        ar.ResearchProcedure = dtdata.Rows[0]["AnimalCategory"].ToString();
                    }
                    if (dtdata.Columns.Contains("AnimalName"))
                    {
                        ar.ResearchProcedure = dtdata.Rows[0]["AnimalName"].ToString();
                    }
                    if (dtdata.Columns.Contains("SpeciesCategory"))
                    {
                        ar.ResearchProcedure = dtdata.Rows[0]["SpeciesCategory"].ToString();
                    }
                    if (dtdata.Columns.Contains("SpeciesName"))
                    {
                        ar.ResearchProcedure = dtdata.Rows[0]["SpeciesName"].ToString();
                    }
                    if (dtdata.Columns.Contains("PermissionName"))
                    {
                        ar.FixedPermissionName = dtdata.Rows[0]["PermissionName"].ToString();
                    }
                    if (dtdata.Columns.Contains("GPSLat"))
                    {
                        ar.Latitude = dtdata.Rows[0]["GPSLat"].ToString();
                    }
                    if (dtdata.Columns.Contains("GPSLong"))
                    {
                        ar.Longitude = dtdata.Rows[0]["GPSLong"].ToString();
                    }
                }
                else
                {
                    return View("Error");
                }
                #endregion
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return View(ar);
        }
        /// <summary>
        /// Action used for bind reason dropdown
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        ///  Defect log Id-89 Done by Rajkumar 
        [HttpPost]
        public JsonResult getResons(string actionId)
        {
            ActionRequest ar = new ActionRequest();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                int actId = Convert.ToInt32(actionId);   //Only rejected reason are binded as per clint request
                DataTable ds = ar.BindReasonList(actId);
                ViewBag.fname = ds;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return Json(new SelectList(items, "Value", "Text"));
        }
        /// <summary>
        /// Button click to submit action result
        /// </summary>
        /// <param name="form"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult SubmitTransitPermit(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            ActionRequest arr = new ActionRequest();
            try
            {
                DataSet ds = new DataSet();
                arr.RequestId = form["RequestId"].ToString();
                ds = arr.SubmitDFO_Forward(form["dropForester"].ToString());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View("ForesterAction");
        }
        [HttpPost]
        public ActionResult SubmitActionResult(FormCollection form, string Command, HttpPostedFileBase Survey_Doc, string OTP, string TransationID)
        {
            ActionRequest arr = new ActionRequest();
            SMS_EMail_Services SE = new SMS_EMail_Services();
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            string UploadDoc = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
            var path = "";
            try
            {
                if (Command == "7" || Command == "6" || Command == "2")
                {
                    #region Approve
                    arr.RequestId = Session["RequestId"].ToString();
                    arr.ModuleId = Convert.ToInt32(Session["ModuleId"]);
                    arr.ServiceTypeId = Convert.ToInt32(Session["ServiceTypeId"]);
                    arr.PermissionId = Convert.ToInt32(Session["PermissionId"]);
                    arr.SubPermissionId = Convert.ToInt32(Session["SubPermissionId"]);
                    // Defect log Id-84 Resolved by Rajkumar 
                    if (Survey_Doc != null && Survey_Doc.ContentLength > 0)
                    {
                        UploadDoc = Path.GetFileName(Survey_Doc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + UploadDoc;
                        path = Path.Combine(FilePath, FileFullName);
                        arr.Survey_Doc = path;
                        Survey_Doc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        arr.Survey_Doc = "";
                    }
                    if (Session["TableName"].ToString() == "tbl_FDM_BudgetEstimation")
                    {
                        if (Session["IsReviewer"].ToString() == "True")
                        {
                            arr.ReviewedAmount = Convert.ToDecimal(form["ReviewedAmount"].ToString());
                            arr.ApprovedAmount = Convert.ToDecimal(0.0);
                        }
                        if (Session["IsReviewer"].ToString() == "False")
                        {
                            arr.ApprovedAmount = Convert.ToDecimal(form["ApprovedAmount"].ToString());
                            arr.ReviewedAmount = Convert.ToDecimal(0.0);
                        }
                        arr.DateFrom = Convert.ToDateTime(DateTime.Now);
                        arr.DateTo = Convert.ToDateTime(DateTime.Now);
                        arr.AppearanceDate = "";
                    }
                    else
                    {
                        // Defect log Id-80 Resolved by Rajkumar 
                        if (string.IsNullOrEmpty(Convert.ToString(form["Durationfrom"])))
                        {
                            arr.DateFrom = Convert.ToDateTime(DateTime.Now);
                        }
                        else
                        {
                            arr.DateFrom = DateTime.ParseExact(form["Durationfrom"].ToString(), "dd/MM/yyyy", null);
                        }
                        if (string.IsNullOrEmpty(Convert.ToString(form["Durationto"])))
                        {
                            arr.DateTo = Convert.ToDateTime(DateTime.Now);
                        }
                        else
                        {
                            arr.DateTo = DateTime.ParseExact(form["Durationto"].ToString(), "dd/MM/yyyy", null);
                        }
                        if (form["AppearanceDate"] != null && form["AppearanceDate"].ToString() != "")
                        {
                            arr.AppearanceDate = form["AppearanceDate"].ToString();
                        }
                        else
                        {
                            arr.AppearanceDate = "";
                        }
                    }
                    arr.Action = Command;
                    if (Command == "6")
                    {
                        arr.reason = form["DropReassignreason"].ToString();
                    }
                    else
                    {
                        arr.reason = "";
                    }
                    arr.ActionTakenBy = Convert.ToInt32(Session["UserId"]);
                    arr.ReviewedComment = form["txtAreaComment"].ToString();  // Defect log Id-84 Resolved by Rajkumar 
                    arr.TableName = Session["TableName"].ToString();
                    DataSet ds = new DataSet();
                    ds = arr.SubmitActionResult();
                    if (ds.Tables[0].Rows[0][0].ToString() == "DUP")
                    {
                        TempData["Approve"] = "RequestId:" + Session["RequestId"].ToString() + " has already " + arr.ActionDetail(Command) + "ed";
                    }
                    else
                    {
                        #region Commented Approve
                        //if (arr.ActionDetail(Command) == "Approve")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_GenerateApprovedBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString());
                        //            string UserSmsBody = Common.Citizen_Approved_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString());
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        //if (arr.ActionDetail(Command) == "Review")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_Reassign_Review_RejEmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            string UserSmsBody = Common.Citizen_Reasssign_Review_Rej_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //        if (ds.Tables[2].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Forester_Approve_ReviewRequest_EmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            string UserSmsBody = Common.Forester_Approve_ReviewRequest_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[2].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[2].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        //if (arr.ActionDetail(Command) == "Reassign")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_Reassign_Review_RejEmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reassign");
                        //            string UserSmsBody = Common.Citizen_Reasssign_Review_Rej_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reassign");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        //if (arr.ActionDetail(Command) == "Reject")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_Reassign_Review_RejEmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reject");
                        //            string UserSmsBody = Common.Citizen_Reasssign_Review_Rej_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reject");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //} 
                        #endregion
                        if (arr.ActionDetail(Command) == "Approve")
                        {
                            #region Call E-Sign API
                            clsVerifyOTP request = new clsVerifyOTP();
                            request.otp = OTP;
                            request.transactionid = TransationID;
                            clsVerifyOTPResponce response = FMDSS.App_Start.cls_ESignIntegration.VerifyOTPAndGenrateTransation(request, Session["RequestId"].ToString(), Command, Session["TableName"].ToString());
                            if (!string.IsNullOrEmpty(response.TransactionId))
                                TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " has been Approved Sucessfully and Genrated PDF with E-Sign";
                            else
                                TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " has been Approved Sucessfully but not Genrated PDF with E-Sign";
                            #endregion
                        }
                        if (arr.ActionDetail(Command) == "Review")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Reviewed Sucessfully";
                        }
                        if (arr.ActionDetail(Command) == "Reassign")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Reassigned Sucessfully";
                        }
                        if (arr.ActionDetail(Command) == "Reject")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Reject Sucessfully";
                        }
                    }

                    UpdateSWCS(arr.RequestId);

                    #endregion
                    #region Email and SMS
                    DataTable DT = objSMSandEMAILtemplate.GetUserDetails(arr.RequestId, "CitizenTransitPermit");
                    objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenTransitPermit", arr.RequestId, Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), arr.ActionDetail(Command));
                    #endregion
                }
                if (Command == "3")
                {
                    #region Reject
                    arr.RequestId = Session["RequestId"].ToString();
                    arr.ModuleId = Convert.ToInt32(Session["ModuleId"]);
                    arr.ServiceTypeId = Convert.ToInt32(Session["ServiceTypeId"]);
                    arr.PermissionId = Convert.ToInt32(Session["PermissionId"]);
                    arr.SubPermissionId = Convert.ToInt32(Session["SubPermissionId"]);
                    arr.Action = Command;
                    if (Survey_Doc != null && Survey_Doc.ContentLength > 0)
                    {
                        UploadDoc = Path.GetFileName(Survey_Doc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + UploadDoc;
                        path = Path.Combine(FilePath, FileFullName);
                        arr.Survey_Doc = path;
                        Survey_Doc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        arr.Survey_Doc = "";
                    }
                    if (Session["TableName"].ToString() == "tbl_FDM_BudgetEstimation")
                    {
                        if (Session["IsReviewer"].ToString() == "True")
                        {
                            arr.ReviewedAmount = Convert.ToDecimal(form["ReviewedAmount"].ToString());
                            arr.ApprovedAmount = Convert.ToDecimal(0.0);
                        }
                        if (Session["IsReviewer"].ToString() == "False")
                        {
                            arr.ApprovedAmount = Convert.ToDecimal(form["ApprovedAmount"].ToString());
                            arr.ReviewedAmount = Convert.ToDecimal(0.0);
                        }
                        arr.DateFrom = Convert.ToDateTime(DateTime.Now);
                        arr.DateTo = Convert.ToDateTime(DateTime.Now);
                        arr.AppearanceDate = "";
                    }
                    else
                    {
                        if (form["Durationfrom"].ToString() == "")
                        {
                            arr.DateFrom = Convert.ToDateTime(DateTime.Now);
                        }
                        else
                        {
                            arr.DateFrom = DateTime.ParseExact(form["Durationfrom"].ToString(), "dd/MM/yyyy", null);
                        }
                        if (form["Durationto"].ToString() == "")
                        {
                            arr.DateTo = Convert.ToDateTime(DateTime.Now);
                        }
                        else
                        {
                            arr.DateTo = DateTime.ParseExact(form["Durationto"].ToString(), "dd/MM/yyyy", null);
                        }
                        if (form["AppearanceDate"] != null && form["AppearanceDate"].ToString() != "")
                        {
                            arr.AppearanceDate = form["AppearanceDate"].ToString();
                        }
                        else
                        {
                            arr.AppearanceDate = "";
                        }
                    }
                    arr.reason = form["Dropreason"].ToString();
                    arr.ActionTakenBy = Convert.ToInt32(Session["UserId"]);
                    arr.ReviewedComment = form["txtAreaComment"].ToString();
                    arr.TableName = Session["TableName"].ToString();
                    DataSet ds = new DataSet();
                    ds = arr.SubmitActionResult();
                    if (ds.Tables[0].Rows[0][0].ToString() == "DUP")
                    {
                        TempData["Approve"] = "RequestId:" + Session["RequestId"].ToString() + " has already " + arr.ActionDetail(Command) + "ed";
                    }
                    else
                    {
                        //if (arr.ActionDetail(Command) == "Approve")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_GenerateApprovedBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString());
                        //            string UserSmsBody = Common.Citizen_Approved_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString());
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        //if (arr.ActionDetail(Command) == "Review")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_Reassign_Review_RejEmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            string UserSmsBody = Common.Citizen_Reasssign_Review_Rej_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //        if (ds.Tables[2].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Forester_Approve_ReviewRequest_EmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            string UserSmsBody = Common.Forester_Approve_ReviewRequest_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reviewed");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[2].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[2].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        //if (arr.ActionDetail(Command) == "Reassign")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_Reassign_Review_RejEmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reassign");
                        //            string UserSmsBody = Common.Citizen_Reasssign_Review_Rej_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reassign");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        //if (arr.ActionDetail(Command) == "Reject")
                        //{
                        //    if (ds != null)
                        //    {
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            string UserMailBody = Common.Citizen_Reassign_Review_RejEmailBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reject");
                        //            string UserSmsBody = Common.Citizen_Reasssign_Review_Rej_SMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), Session["PermissionDesc"].ToString(), "reject");
                        //            SE.sendEMail(arr.ActionDetail(Command), UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);
                        //            SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        //        }
                        //    }
                        //}
                        if (arr.ActionDetail(Command) == "Approve")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Approved Sucessfully";
                        }
                        if (arr.ActionDetail(Command) == "Review")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Reviewed Sucessfully";
                        }
                        if (arr.ActionDetail(Command) == "Reassign")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Reassigned Sucessfully";
                        }
                        if (arr.ActionDetail(Command) == "Reject")
                        {
                            TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " Reject Sucessfully";
                        }
                    }
                    #endregion
                    #region Email and SMS
                    DataTable DT = objSMSandEMAILtemplate.GetUserDetails(arr.RequestId, "CitizenTransitPermit");
                    objSMSandEMAILtemplate.SendMailComman("ALL", "CitizenTransitPermit", arr.RequestId, Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["EmailId"]), Convert.ToString(DT.Rows[0]["Mobile"]), arr.ActionDetail(Command));
                    #endregion
                }
                if (Command == "Forward")
                {
                    DataSet ds = new DataSet();
                    arr.RequestId = form["RequestId"].ToString();
                    ds = arr.SubmitDFO_Forward(form["dropForester"].ToString());
                }
                if (Command == "TransitPermitForward")
                {
                    string PERMIT_NO = Convert.ToString(form["PERMIT_NO"]);
                    string DATE = Convert.ToString(form["DateOfOffense"]);
                    string TIME = Convert.ToString(form["TimeOfOffense"]);
                    int i = arr.SubmitTransitPermit(PERMIT_NO, DATE, TIME, Convert.ToString(Session["UserID"]));


                }
            }
            catch (Exception ex) { Response.Write(ex); }
            return RedirectToAction("ForesterAction");
        }

        public void UpdateSWCS(string reqId)
        {
            DAL dl = new DAL();

            SqlParameter[] param = { new SqlParameter("@RequestId", reqId) };
            DataSet dsReq = new DataSet();
            dl.Fill(dsReq, "spGetFixedPermissionData", param);
            if (dsReq != null && dsReq.Tables[0] != null && dsReq.Tables[0].Rows.Count > 0)
            {
                string name = string.Empty;
                if (Session["SSoId"] != null)
                {
                    name = Session["SSoId"].ToString();
                }
                else
                {
                    name = Session["UserId"].ToString();
                }

                STATUSUPDATE.STATUSUPDATE statusupdate = new STATUSUPDATE.STATUSUPDATE();
                int AppServiceCode = Convert.ToInt32(dsReq.Tables[0].Rows[0]["P_Id"]);
                int newStatus = Convert.ToInt32(dsReq.Tables[0].Rows[0]["status"]);

                //string result = statusupdate.statusupdate(19,AppServiceCode, reqId, newStatus, "Updated by " + name, "", "", "2");
                string result = statusupdate.statusupdate(19, AppServiceCode, reqId, newStatus, "", "", "", "");
            }
        }

        /// <summary>
        /// Used for view details of particular row 
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        /// Defect log Id-89 Done by Rajkumar 
        [HttpPost]
        public JsonResult ViewDetails(string RequestId, string TableName)
        {
            try
            {
                #region Details
                ActionRequest ar = new ActionRequest();
                viewdetail vd = new viewdetail();
                DataSet dsMultDist = new DataSet();
                DataTable dtdata = new DataTable();
                dtdata = ar.BindActionList(RequestId, TableName);
                if (TableName == "Tbl_Citizen_TransitPermit" && dtdata != null && dtdata.Rows.Count > 0)
                {
                    int count = 1;
                    StringBuilder SB = new StringBuilder();
                    StringBuilder subSB = new StringBuilder();
                    SB.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                    foreach (DataRow dr in dtdata.Rows)
                    {
                        while (dtdata.Columns.Count > count)
                        {
                            if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("approved"))
                            {
                                subSB.Append("<tr><th>");
                                subSB.Append(dtdata.Columns[count].ColumnName);
                                subSB.Append("</th><td>");
                            }
                            else
                            {
                                SB.Append("<tr><th>");
                                SB.Append(dtdata.Columns[count].ColumnName);
                                SB.Append("</th><td>");
                            }
                            if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower() == "requestid")
                            {
                                SB.Append("<input type='hidden' name='PERMIT_NO' id='PERMIT_NO'  value='" + dtdata.Rows[0][count].ToString() + "' />");
                            }
                            if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("attachement"))
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0][count])))
                                    SB.Append("<a href='" + Convert.ToString(dtdata.Rows[0][count]).Replace("~", string.Empty) + "' target='_blank' rel='noopener noreferrer'><img src='../images/jpeg.png' width='30'</a>");
                            }
                            else if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("approved"))
                            {
                                subSB.Append(dtdata.Rows[0][count].ToString());
                                subSB.Append("</td></tr>");
                            }
                            else
                            {
                                SB.Append(dtdata.Rows[0][count].ToString());
                            }
                            SB.Append("</td></tr>");
                            count = Convert.ToInt16(count + 1);
                        }
                    }
                    //SB.Append("</tbody> </table> ");
                    #region Survey Report List
                    dtdata = ar.GetSurveyReportCitizen(RequestId, "Get");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        count = 1;
                        SB.Append("<h5>Surery Report</h5>");
                        SB.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                        foreach (DataRow dr in dtdata.Rows)
                        {
                            while (dtdata.Columns.Count > count)
                            {
                                SB.Append("<tr><th>");
                                SB.Append(dtdata.Columns[count].ColumnName);
                                SB.Append("</th><td>");
                                if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower() == "requestid")
                                {
                                    SB.Append("<input type='hidden' name='PERMIT_NO' id='PERMIT_NO'  value='" + dtdata.Rows[0][count].ToString() + "' />");
                                }
                                if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("attachement"))
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0][count])))
                                        SB.Append("<a href='" + Convert.ToString(dtdata.Rows[0][count]).Replace("~", string.Empty) + "' target='_blank' rel='noopener noreferrer'><img src='../images/jpeg.png' width='30'</a>");
                                }
                                else
                                {
                                    SB.Append(dtdata.Rows[0][count].ToString());
                                }
                                SB.Append("</td></tr>");
                                count = Convert.ToInt16(count + 1);
                            }
                        }
                        subSB.Append("</tbody> </table> ");
                        SB.Append(subSB);
                    }
                    #endregion
                    SB.Append("</tbody> </table> ");
                    return Json(SB.ToString(), JsonRequestBehavior.AllowGet);
                }
                vd.TableName = TableName;
                vd.RequestId = RequestId;
                vd.ModuleName = dtdata.Rows[0]["ModuleDesc"].ToString();
                vd.ServiceType = dtdata.Rows[0]["ServiceTypeDesc"].ToString();
                vd.PermissionType = dtdata.Rows[0]["PermissionDesc"].ToString();
                vd.PermissionName = dtdata.Rows[0]["SubPermissionDesc"].ToString();
                Session["SubPermissionId"] = dtdata.Rows[0]["SubPermissionId"].ToString();
                if (vd.TableName == "tbl_FixedPermissions")
                {
                    dsMultDist = ar.GetFixedDistMap(vd.RequestId);
                }
                if (dtdata.Columns.Contains("EnteredOn"))
                {
                    DateTime _date;
                    _date = DateTime.Parse(dtdata.Rows[0]["EnteredOn"].ToString());
                    vd.RequestedOn = _date.ToString("dd-MM-yyyy");
                }
                else
                {
                }
                if (dtdata.Columns.Contains("Name"))
                {
                    vd.RequestedBy = dtdata.Rows[0]["Name"].ToString();
                }
                if (string.IsNullOrEmpty(vd.RequestedBy))
                {
                    if (dtdata.Columns.Contains("UserName"))
                    {
                        vd.RequestedBy = dtdata.Rows[0]["UserName"].ToString();
                    }
                }
                if (dtdata.Columns.Contains("DurationFrom"))
                {
                    DateTime _date1, _date2;
                    _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                    _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                    vd.Duration = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                }
                else if (dtdata.Columns.Contains("StartDate"))
                {
                    DateTime _date1, _date2;
                    _date1 = DateTime.Parse(dtdata.Rows[0]["StartDate"].ToString());
                    _date2 = DateTime.Parse(dtdata.Rows[0]["EndDate"].ToString());
                    vd.Duration = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                }
                else
                {
                    if (vd.Duration == null || vd.Duration == "")
                    {
                        vd.Duration = "N/A";
                    }
                }
                if (dtdata.Columns.Contains("trn_Status_Code"))
                {
                    try
                    {
                        vd.DownloadNOC = Convert.ToString(dtdata.Rows[0]["DownloadNOC"]);
                    }
                    catch (Exception) { }
                    if (vd.PermissionType == "Fixed Land Usage" && (vd.PermissionName == "Mining Permission" || vd.PermissionName == "Swmill Permission"))
                    {

                        if (dtdata.Rows[0]["trn_Status_Code"].ToString() == "1")
                        {
                            vd.PaymentDone = "True";

                        }
                        else
                        {
                            vd.PaymentDone = "N/A";
                        }
                    }
                    else if (dtdata.Rows[0]["trn_Status_Code"].ToString() == "1" && vd.PermissionType != "Fixed Land Usage")
                    {
                        vd.PaymentDone = "True";
                    }
                    else
                    {
                        vd.PaymentDone = "N/A";
                    }
                }
                else
                {
                    vd.PaymentDone = "N/A";
                }
                if (dtdata.Columns.Contains("DepositAmount"))
                {
                    vd.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["DepositAmount"]) + Convert.ToDecimal(dtdata.Rows[0]["TotalFees"]);
                }
                else if (dtdata.Columns.Contains("Fees"))
                {
                    vd.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["Fees"]);
                }
                else if (dtdata.Columns.Contains("Final_Amount"))
                {
                    vd.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["Final_Amount"]);
                }
                else
                {
                    vd.PaidAmount = 0;
                }
                if (dtdata.Columns.Contains("NumberOfCrewMembers"))
                {
                    vd.NumberOfPerson = dtdata.Rows[0]["NumberOfCrewMembers"].ToString();
                }
                else if (dtdata.Columns.Contains("No_Of_Member"))
                {
                    vd.NumberOfPerson = dtdata.Rows[0]["No_Of_Member"].ToString();
                }
                else
                {
                    vd.NumberOfPerson = "N/A";
                }
                if (dtdata.Columns.Contains("ApplicantType"))
                {
                    if (dtdata.Rows[0]["ApplicantType"].ToString() == "1")
                    {
                        vd.ApplicationType = "Individual";
                    }
                    else
                    {
                        vd.ApplicationType = "Organizational";
                    }
                }
                else if (dtdata.Columns.Contains("Applicant_Type"))
                {
                    if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                    {
                        vd.ApplicationType = "Individual";
                    }
                    else
                    {
                        vd.ApplicationType = "Organizational";
                    }
                }
                else
                {
                    vd.ApplicationType = "N/A";
                }
                if (dtdata.Columns.Contains("Title"))
                {
                    vd.FilmTitle = dtdata.Rows[0]["Title"].ToString();
                    if (vd.FilmTitle == null || vd.FilmTitle == "")
                    {
                        vd.FilmTitle = "N/A";
                    }
                }
                else
                {
                    vd.FilmTitle = "N/A";
                }
                if (dtdata.Columns.Contains("ShootingPurpose"))
                {
                    vd.ShootingPurpose = dtdata.Rows[0]["ShootingPurpose"].ToString();
                    if (vd.ShootingPurpose == null || vd.ShootingPurpose == "")
                    {
                        vd.ShootingPurpose = "N/A";
                    }
                }
                else
                {
                    vd.ShootingPurpose = "N/A";
                }
                if (dtdata.Columns.Contains("IdentityProofNo"))
                {
                    vd.IdProofNo = dtdata.Rows[0]["IdentityProofNo"].ToString();
                }
                else if (dtdata.Columns.Contains("IdProofNo"))
                {
                    vd.IdProofNo = dtdata.Rows[0]["IdProofNo"].ToString();
                }
                else
                {
                    vd.IdProofNo = "N/A";
                }
                if (dtdata.Columns.Contains("IDProofUrl"))
                {
                    string strproof = dtdata.Rows[0]["IDProofUrl"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    vd.IdProofUrl = Server.MapPath(strproofsplit[strproofsplit.Length - 1]);
                }
                else if (dtdata.Columns.Contains("IDProof_Path"))
                {
                    string strproof = dtdata.Rows[0]["IDProof_Path"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    vd.IdProofUrl = Server.MapPath(strproofsplit[strproofsplit.Length - 1]);
                }
                else
                {
                    vd.IdProofUrl = "N/A";
                }
                if (dtdata.Columns.Contains("Purpose_OCM"))
                {
                    vd.Purpose_OCM = dtdata.Rows[0]["Purpose_OCM"].ToString();
                }
                else
                {
                    vd.Purpose_OCM = "N/A";
                }
                if (dtdata.Columns.Contains("NoOfDays"))
                {
                    vd.NumberOfDay = dtdata.Rows[0]["NoOfDays"].ToString();
                    if (vd.NumberOfDay == null || vd.NumberOfDay == "")
                    {
                        vd.NumberOfDay = "N/A";
                    }
                }
                else
                {
                    vd.NumberOfDay = "N/A";
                }
                if (dtdata.Columns.Contains("DIST_CODE"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultDiv = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultDiv.ToString().Contains(dsMultDist.Tables[0].Rows[i][6].ToString()))
                                    {
                                        sbMultDiv.Append(dsMultDist.Tables[0].Rows[i][2].ToString() + " ,");
                                    }
                                }
                                if (sbMultDiv.Length > 0)
                                {
                                    vd.Division_Name = sbMultDiv.ToString().Remove(sbMultDiv.ToString().Length - 1, 1);
                                }
                                else { vd.Division_Name = "N/A"; }
                            }
                            else { vd.Division_Name = "N/A"; }
                        }
                        else { vd.Division_Name = "N/A"; }
                    }
                    // Modified by-Rajkumar 
                    // Modified Date-28-03-2016
                    else if (vd.TableName == "tbl_FDM_Project")
                    {
                        DataTable dsTable = new DataTable();
                        StringBuilder sb = new StringBuilder();
                        dsTable = ar.MultiDistrict(dtdata.Rows[0]["DIST_CODE"].ToString());
                        if (dsTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTable.Rows.Count; i++)
                            {
                                sb.Append(dsTable.Rows[i][0].ToString() + " ,");
                            }
                            vd.District = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                        }
                        else { vd.District = "N/A"; }
                    }
                    else
                    {
                        if (dtdata.Columns.Contains("DIV_NAME"))
                        {
                            vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                            if (vd.Division_Name == null || vd.Division_Name == "")
                            {
                                vd.Division_Name = "N/A";
                            }
                        }
                        else
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                }
                else
                {
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                        if (vd.Division_Name == null || vd.Division_Name == "")
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Division_Name = "N/A";
                    }
                }
                if (dtdata.Columns.Contains("DIST_NAME"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultDist = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultDist.ToString().Contains(dsMultDist.Tables[0].Rows[i][2].ToString()))
                                    {
                                        sbMultDist.Append(dsMultDist.Tables[0].Rows[i][2].ToString() + " ,");
                                    }
                                }
                                if (sbMultDist.Length > 0)
                                {
                                    vd.District = sbMultDist.ToString().Remove(sbMultDist.ToString().Length - 1, 1);
                                }
                                else { vd.District = "N/A"; }
                            }
                            else { vd.District = "N/A"; }
                        }
                        else { vd.District = "N/A"; }
                    }
                    else
                    {
                        vd.District = dtdata.Rows[0]["DIST_NAME"].ToString();
                        if (vd.District == null || vd.District == "")
                        {
                            vd.District = "N/A";
                        }
                    }
                }
                else
                {
                    if (vd.District == null || vd.District == "")
                    {
                        vd.District = "N/A";
                    }
                }
                if (dtdata.Columns.Contains("PlaceName"))
                {
                    vd.Place = dtdata.Rows[0]["PlaceName"].ToString();
                    if (vd.Place == null || vd.Place == "")
                    {
                        vd.Place = "N/A";
                    }
                }
                else if (dtdata.Columns.Contains("Area"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultArea = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultArea.ToString().Contains(dsMultDist.Tables[0].Rows[i][0].ToString()))
                                    {
                                        sbMultArea.Append(dsMultDist.Tables[0].Rows[i][0].ToString() + " ,");
                                    }
                                    if (sbMultArea.Length > 0)
                                    {
                                        vd.Place = sbMultArea.ToString().Remove(sbMultArea.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        vd.Place = "N/A";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        vd.Place = dtdata.Rows[0]["Area"].ToString();
                        if (vd.Place == null || vd.Place == "")
                        {
                            vd.Place = "N/A";
                        }
                    }
                }
                else
                {
                    vd.Place = "N/A";
                }
                if (dtdata.Columns.Contains("BLK_NAME"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultBlk = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultBlk.ToString().Contains(dsMultDist.Tables[0].Rows[i][3].ToString()))
                                    {
                                        sbMultBlk.Append(dsMultDist.Tables[0].Rows[i][3].ToString() + " ,");
                                    }
                                }
                                if (sbMultBlk.Length > 0)
                                {
                                    vd.Block = sbMultBlk.ToString().Remove(sbMultBlk.ToString().Length - 1, 1);
                                }
                                else { vd.Block = "N/A"; }
                            }
                            else { vd.Block = "N/A"; }
                        }
                        else { vd.Block = "N/A"; }
                    }
                    else
                    {
                        vd.Block = dtdata.Rows[0]["BLK_NAME"].ToString();
                        if (vd.Block == null || vd.Block == "")
                        {
                            vd.Block = "N/A";
                        }
                    }
                }
                else
                {
                    vd.Block = "N/A";
                }
                if (dtdata.Columns.Contains("GP_NAME"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultGp = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultGp.ToString().Contains(dsMultDist.Tables[0].Rows[i][4].ToString()))
                                    {
                                        sbMultGp.Append(dsMultDist.Tables[0].Rows[i][4].ToString() + " ,");
                                    }
                                }
                                if (sbMultGp.Length > 0)
                                {
                                    vd.GramPanchayat = sbMultGp.ToString().Remove(sbMultGp.ToString().Length - 1, 1);
                                }
                                else { vd.GramPanchayat = "N/A"; }
                            }
                            else { vd.GramPanchayat = "N/A"; }
                        }
                        else { vd.GramPanchayat = "N/A"; }
                    }
                    else
                    {
                        vd.GramPanchayat = dtdata.Rows[0]["GP_NAME"].ToString();
                        if (vd.GramPanchayat == null || vd.GramPanchayat == "")
                        {
                            vd.GramPanchayat = "N/A";
                        }
                    }
                }
                else { vd.GramPanchayat = "N/A"; }
                if (dtdata.Columns.Contains("VILL_CODE"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultVill = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultVill.ToString().Contains(dsMultDist.Tables[0].Rows[i][5].ToString()))
                                    {
                                        sbMultVill.Append(dsMultDist.Tables[0].Rows[i][5].ToString() + " ,");
                                    }
                                }
                                if (sbMultVill.Length > 0)
                                {
                                    vd.Village = sbMultVill.ToString().Remove(sbMultVill.ToString().Length - 1, 1);
                                }
                                else { vd.Village = "N/A"; }
                            }
                            else { vd.Village = "N/A"; }
                        }
                        else { vd.Village = "N/A"; }
                    }
                    else
                    {
                        vd.Village = dtdata.Rows[0]["VILL_CODE"].ToString();
                        if (vd.Village == null || vd.Village == "")
                        {
                            vd.Village = "N/A";
                        }
                    }
                }
                if (dtdata.Columns.Contains("VILL_NAME"))
                {
                    vd.Village = dtdata.Rows[0]["VILL_NAME"].ToString();
                }
                else { vd.Village = "N/A"; }
                if (dtdata.Columns.Contains("KhasraNo"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultKhasra = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultKhasra.ToString().Contains(dsMultDist.Tables[0].Rows[i][1].ToString()))
                                    {
                                        sbMultKhasra.Append(dsMultDist.Tables[0].Rows[i][1].ToString() + " ,");
                                    }
                                }
                                if (sbMultKhasra.Length > 0)
                                {
                                    vd.khasraNo = sbMultKhasra.ToString().Remove(sbMultKhasra.ToString().Length - 1, 1);
                                }
                                else { vd.khasraNo = "N/A"; }
                            }
                            else { vd.khasraNo = "N/A"; }
                        }
                        else { vd.khasraNo = "N/A"; }
                    }
                    else
                    {
                        vd.khasraNo = dtdata.Rows[0]["KhasraNo"].ToString();
                        if (vd.khasraNo == null || vd.khasraNo == "")
                        {
                            vd.khasraNo = "N/A";
                        }
                    }
                }
                //Modified by Rajkumar 
                //Modified Date 28-03-2016
                if (dtdata.Columns.Contains("EstimatedAmount"))
                {
                    vd.EstimatedAmount = dtdata.Rows[0]["EstimatedAmount"].ToString();
                }
                else if (dtdata.Columns.Contains("Amount"))
                {
                    vd.EstimatedAmount = dtdata.Rows[0]["Amount"].ToString();
                }
                else
                {
                    vd.EstimatedAmount = "N/A";
                }
                if (dtdata.Columns.Contains("ReviewedAmount"))
                {
                    vd.ReviewedAmount = dtdata.Rows[0]["EstimatedAmount"].ToString();
                }
                else
                {
                    vd.ReviewedAmount = "N/A";
                }
                if (dtdata.Columns.Contains("ApprovedAmount"))
                {
                    vd.ApprovedAmount = dtdata.Rows[0]["ApprovedAmount"].ToString();
                }
                else
                {
                    vd.ApprovedAmount = "N/A";
                }
                if (dtdata.Columns.Contains("FinanceYear"))
                {
                    vd.FinanceYear = dtdata.Rows[0]["FinanceYear"].ToString();
                }
                else
                {
                    vd.FinanceYear = "N/A";
                }
                if (dtdata.Columns.Contains("WorkOrder_Name"))
                {
                    vd.WorkOrderName = dtdata.Rows[0]["WorkOrder_Name"].ToString();
                }
                else
                {
                    vd.WorkOrderName = "N/A";
                }
                if (dtdata.Columns.Contains("WorkOrderType"))
                {
                    vd.WorkOrderType = dtdata.Rows[0]["WorkOrderType"].ToString();
                }
                else
                {
                    vd.WorkOrderType = "N/A";
                }
                if (dtdata.Columns.Contains("BudgetHead"))
                {
                    vd.BudgetHead = dtdata.Rows[0]["BudgetHead"].ToString();
                }
                else
                {
                    vd.BudgetHead = "N/A";
                }
                if (dtdata.Columns.Contains("FinancialTarget"))
                {
                    vd.FinancialTarget = dtdata.Rows[0]["FinancialTarget"].ToString();
                }
                else
                {
                    vd.FinancialTarget = "N/A";
                }
                if (dtdata.Columns.Contains("AdminApprovedDate"))
                {
                    vd.AdminApprovaldate = dtdata.Rows[0]["AdminApprovedDate"].ToString();
                }
                else
                {
                    vd.AdminApprovaldate = "N/A";
                }
                if (dtdata.Columns.Contains("FinanceApprovedDate"))
                {
                    vd.FinancialApprovaldate = dtdata.Rows[0]["FinanceApprovedDate"].ToString();
                }
                else
                {
                    vd.FinancialApprovaldate = "N/A";
                }
                if (dtdata.Columns.Contains("Project_Name"))
                {
                    vd.ProjectName = dtdata.Rows[0]["Project_Name"].ToString();
                }
                else
                {
                    vd.ProjectName = "N/A";
                }
                if (dtdata.Columns.Contains("Program_Name"))
                {
                    vd.ProgramName = dtdata.Rows[0]["Program_Name"].ToString();
                }
                else
                {
                    vd.ProgramName = "N/A";
                }
                if (dtdata.Columns.Contains("Scheme_Name"))
                {
                    vd.SchemeName = dtdata.Rows[0]["Scheme_Name"].ToString();
                }
                else
                {
                    vd.SchemeName = "N/A";
                }
                if (dtdata.Columns.Contains("RefDocument"))
                {
                    string strproof = dtdata.Rows[0]["RefDocument"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    if (strproofsplit.Length > 0) { vd.ProjRefDoc = strproofsplit[strproofsplit.Length - 1]; } else { vd.ProjRefDoc = "N/A"; }
                }
                else
                {
                    vd.ProjRefDoc = "N/A";
                }
                if (dtdata.Columns.Contains("DPRDocument"))
                {
                    string strproof = dtdata.Rows[0]["DPRDocument"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    if (strproofsplit.Length > 0) { vd.ProjDprDoc = strproofsplit[strproofsplit.Length - 1]; } else { vd.ProjDprDoc = "N/A"; }
                }
                else
                {
                    vd.ProjDprDoc = "N/A";
                }
                if (dtdata.Columns.Contains("EstimatedBudget"))
                {
                    vd.ProjEstimatedBudget = dtdata.Rows[0]["EstimatedBudget"].ToString();
                }
                else
                {
                    vd.ProjEstimatedBudget = "N/A";
                }
                //End of Modification date 28-03-2016
                #region Research Study Permission
                if (vd.TableName == "tbl_ResearchStudyPermissions")
                {
                    if (dtdata.Columns.Contains("Qualification"))
                    {
                        vd.Qualification = dtdata.Rows[0]["Qualification"].ToString();
                        if (vd.Qualification == null || vd.Qualification == "")
                        {
                            vd.Qualification = "N/A";
                        }
                    }
                    else
                    {
                        vd.Qualification = "N/A";
                    }
                    if (dtdata.Columns.Contains("College"))
                    {
                        vd.College = dtdata.Rows[0]["College"].ToString();
                        if (vd.College == null || vd.College == "")
                        {
                            vd.College = "N/A";
                        }
                    }
                    else
                    {
                        vd.College = "N/A";
                    }
                    if (dtdata.Columns.Contains("R_Subject"))
                    {
                        vd.ResearchSubject = dtdata.Rows[0]["R_Subject"].ToString();
                        if (vd.ResearchSubject == null || vd.ResearchSubject == "")
                        {
                            vd.ResearchSubject = "N/A";
                        }
                    }
                    else
                    {
                        vd.ResearchSubject = "N/A";
                    }
                    if (dtdata.Columns.Contains("R_Procedure"))
                    {
                        vd.ResearchProcedure = dtdata.Rows[0]["R_Procedure"].ToString();
                        if (vd.ResearchProcedure == null || vd.ResearchProcedure == "")
                        {
                            vd.ResearchProcedure = "N/A";
                        }
                    }
                    else
                    {
                        vd.ResearchProcedure = "N/A";
                    }
                    if (dtdata.Columns.Contains("AnimalCategory"))
                    {
                        vd.AnimalCategory = dtdata.Rows[0]["AnimalCategory"].ToString();
                    }
                    else
                    {
                        vd.AnimalCategory = "N/A";
                    }
                    if (dtdata.Columns.Contains("AnimalName"))
                    {
                        vd.AnimalName = dtdata.Rows[0]["AnimalName"].ToString();
                    }
                    else
                    {
                        vd.AnimalName = "N/A";
                    }
                    if (dtdata.Columns.Contains("SpeciesCategory"))
                    {
                        vd.SpeciesCategory = dtdata.Rows[0]["SpeciesCategory"].ToString();
                        if (vd.SpeciesCategory == null || vd.SpeciesCategory == "")
                        {
                            vd.SpeciesCategory = "N/A";
                        }
                    }
                    else
                    {
                        vd.SpeciesCategory = "N/A";
                    }
                    if (dtdata.Columns.Contains("SpeciesName"))
                    {
                        vd.SpeciesName = dtdata.Rows[0]["SpeciesName"].ToString();
                        if (vd.SpeciesName == null || vd.SpeciesName == "")
                        {
                            vd.SpeciesName = "N/A";
                        }
                    }
                    else
                    {
                        vd.SpeciesName = "N/A";
                    }
                    if (dtdata.Columns.Contains("Animal_Sno"))
                    {
                        vd.Animal_SerialNo = dtdata.Rows[0]["Animal_Sno"].ToString();
                        if (vd.Animal_SerialNo == null || vd.Animal_SerialNo == "")
                        {
                            vd.Animal_SerialNo = "N/A";
                        }
                    }
                    else
                    {
                        vd.Animal_SerialNo = "N/A";
                    }
                    if (dtdata.Columns.Contains("Species_Sno"))
                    {
                        vd.Species_SerialNo = dtdata.Rows[0]["Species_Sno"].ToString();
                        if (vd.Species_SerialNo == null || vd.Species_SerialNo == "")
                        {
                            vd.Species_SerialNo = "N/A";
                        }
                    }
                    else
                    {
                        vd.Species_SerialNo = "N/A";
                    }
                    if (dtdata.Columns.Contains("Benefits"))
                    {
                        vd.Benefits = dtdata.Rows[0]["Benefits"].ToString();
                        if (vd.Benefits == null || vd.Benefits == "")
                        {
                            vd.Benefits = "N/A";
                        }
                    }
                    else
                    {
                        vd.Benefits = "N/A";
                    }
                    if (dtdata.Columns.Contains("CoordinatorId"))
                    {
                        vd.CoordinatorId = dtdata.Rows[0]["CoordinatorId"].ToString();
                        if (vd.CoordinatorId == null || vd.CoordinatorId == "")
                        {
                            vd.CoordinatorId = "N/A";
                        }
                    }
                    else
                        vd.CoordinatorId = "N/A";
                    // added more fields in Research Study for the changes came on 06-Aug-2016
                    vd.SynopsisPath = Convert.ToString(dtdata.Rows[0]["Synopsis_Name"]);
                    if (vd.SynopsisPath == "")
                        vd.SynopsisPath = "N/A";
                    vd.PresentationPath = Convert.ToString(dtdata.Rows[0]["Presentation_Name"]);
                    if (vd.PresentationPath == "")
                        vd.PresentationPath = "N/A";
                    vd.AssistName = Convert.ToString(dtdata.Rows[0]["Assist_Name"]);
                    if (vd.AssistName == "")
                        vd.AssistName = "N/A";
                    vd.AssistIdProofPath = Convert.ToString(dtdata.Rows[0]["Assist_IdProof_Name"]);
                    if (vd.AssistIdProofPath == "")
                        vd.AssistIdProofPath = "N/A";
                    vd.Vehiclecat = Convert.ToString(dtdata.Rows[0]["VehicleCat"]);
                    if (vd.Vehiclecat == "")
                        vd.Vehiclecat = "N/A";
                    vd.VehicleName = Convert.ToString(dtdata.Rows[0]["Vehiclename"]);
                    if (vd.VehicleName == "")
                        vd.VehicleName = "N/A";
                }
                #endregion
                if (dtdata.Columns.Contains("PermissionName"))
                {
                    vd.FixedPermissionName = dtdata.Rows[0]["PermissionName"].ToString();
                    if (vd.FixedPermissionName == null || vd.FixedPermissionName == "")
                    {
                        vd.FixedPermissionName = "N/A";
                    }
                }
                else
                {
                    vd.FixedPermissionName = "N/A";
                }
                if (dtdata.Columns.Contains("GPSLat"))
                {
                    vd.Latitude = dtdata.Rows[0]["GPSLat"].ToString();
                    if (vd.Latitude == null || vd.Latitude == "")
                    {
                        vd.Latitude = "N/A";
                    }
                }
                else { vd.Latitude = "N/A"; }
                if (dtdata.Columns.Contains("GPSLong"))
                {
                    vd.Longitude = dtdata.Rows[0]["GPSLong"].ToString();
                    if (vd.Longitude == null || vd.Longitude == "")
                    {
                        vd.Longitude = "N/A";
                    }
                }
                else { vd.Longitude = "N/A"; }
                if (dtdata.Columns.Contains("StatusDesc"))
                {
                    vd.StatusDesc = dtdata.Rows[0]["StatusDesc"].ToString();
                    if (vd.StatusDesc == null || vd.StatusDesc == "")
                    {
                        vd.StatusDesc = "N/A";
                    }
                }
                else { vd.StatusDesc = "N/A"; }
                if (dtdata.Columns.Contains("Remarks"))
                {
                    if (vd.StatusDesc == "Pending")
                    {
                        vd.Remarks = "";
                    }
                    else
                    {
                        vd.Remarks = dtdata.Rows[0]["Remarks"].ToString();
                    }
                    if (vd.Remarks == null || vd.Remarks == "")
                    {
                        vd.Remarks = "N/A";
                    }
                }
                else { vd.Remarks = "N/A"; }
                if (dtdata.Columns.Contains("ActionReason"))
                {
                    DataTable dsTable = new DataTable();
                    StringBuilder sb = new StringBuilder();
                    dsTable = ar.ReasonList(dtdata.Rows[0]["ActionReason"].ToString());
                    if (dsTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dsTable.Rows.Count; i++)
                        {
                            sb.Append(dsTable.Rows[i][0].ToString() + " ,");
                        }
                        vd.Reason_Desc = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                    }
                    else { vd.Reason_Desc = "N/A"; }
                }
                else { vd.Reason_Desc = "N/A"; }
                if (dtdata.Columns.Contains("KML_Path"))
                {
                    vd.KmlPath = dtdata.Rows[0]["KML_Path"].ToString();
                }
                if (dtdata.Columns.Contains("GISID"))
                {
                    vd.KmlView = dtdata.Rows[0]["GISID"].ToString();
                    vd.NOCType = ar.GISPerName(Session["SubPermissionId"].ToString());
                    //string strkml = dtdata.Rows[0]["KMLPathView"].ToString();
                    //string[] strkmlsplit = strkml.Split('/');
                    //if (strkmlsplit.Length > 0)
                    //{
                    //    string KmlFile = strkmlsplit[strkmlsplit.Length - 1];
                    //    vd.KmlView = "https://gis.rajasthan.gov.in/fmdssgis/gisview/viewongis.aspx?" + "portalid=rajcomp123&ssoid=" + Convert.ToString(Session["SSOid"]) + "&requestFor=" + ar.GISPerName(Session["SubPermissionId"].ToString()) + "&fileName=" + KmlFile;
                    //}
                    //else
                    //{
                    //    vd.KmlView = "N/A";
                    //}
                }
                //if (dtdata.Columns.Contains("KML_Path"))
                //{
                //    vd.KmlPath = vd.KmlPath = ConfigurationManager.AppSettings["KMLPath"] + Convert.ToString(dtdata.Rows[0]["KML_Path"]) + ".kml";  
                //    string strkml = dtdata.Rows[0]["KML_Path"].ToString();
                //    string[] strkmlsplit = strkml.Split('/');
                //    if (strkmlsplit.Length > 0)
                //    {
                //        string KmlFile = strkmlsplit[strkmlsplit.Length - 1];
                //        vd.KmlView = "https://gis.rajasthan.gov.in/fmdssgis/gisview/viewongis.aspx?" + "portalid=rajcomp123&ssoid=" + Convert.ToString(Session["SSOid"]) + "&requestFor=" + ar.GISPerName(Session["SubPermissionId"].ToString()) + "&fileName=" + KmlFile;
                //    }
                //    else
                //    {
                //        vd.KmlView = "N/A";
                //    }
                //}
                //else { vd.KmlPath = "N/A"; }
                if (dtdata.Columns.Contains("Revenue_Record_Path"))
                {
                    string str = dtdata.Rows[0]["Revenue_Record_Path"].ToString();
                    string[] strsplit = str.Split('/');
                    vd.Revenue_Record_Path = strsplit[strsplit.Length - 1];
                }
                else { vd.Revenue_Record_Path = "N/A"; }
                if (dtdata.Columns.Contains("Revenue_Map_Path"))
                {
                    string strmap = dtdata.Rows[0]["Revenue_Map_Path"].ToString();
                    string[] strmapsplit = strmap.Split('/');
                    vd.Revenue_Map_Path = strmapsplit[strmapsplit.Length - 1];
                }
                else { vd.Revenue_Map_Path = "N/A"; }
                if (dtdata.Columns.Contains("Citizen_Comment"))
                {
                    vd.Citizen_Comment = dtdata.Rows[0]["Citizen_Comment"].ToString();
                }
                else { vd.Citizen_Comment = "N/A"; }
                if (dtdata.Columns.Contains("Additional_Document"))
                {
                    string strdoc = dtdata.Rows[0]["Additional_Document"].ToString();
                    string[] strdocsplit = strdoc.Split('/');
                    vd.Additional_Document = strdocsplit[strdocsplit.Length - 1];
                }
                else { vd.Additional_Document = "N/A"; }
                if (dtdata.Columns.Contains("Survey_Document"))
                {
                    string strdoc = dtdata.Rows[0]["Survey_Document"].ToString();
                    string[] stsurrdocsplit = strdoc.Split('/');
                    vd.Survey_Document = stsurrdocsplit[stsurrdocsplit.Length - 1];
                }
                else { vd.Survey_Document = "N/A"; }
                if (dtdata.Columns.Contains("PlantCount"))
                {
                    vd.PlantCount = dtdata.Rows[0]["PlantCount"].ToString();
                }
                else { vd.PlantCount = "0"; }
                if (dtdata.Columns.Contains("IsGTSheetAvaliable"))
                {
                    //if (vd.PermissionName == "Mining Permission") {
                    if (dtdata.Rows[0]["IsGTSheetAvaliable"].ToString() == "True")
                    {
                        vd.IsGTSheetAvaliable = "True";
                        if (dtdata.Columns.Contains("Nearest_WaterSource"))
                        {
                            vd.Nearest_WaterSource = dtdata.Rows[0]["Nearest_WaterSource"].ToString();
                        }
                        else
                        {
                            vd.Nearest_WaterSource = "N/A";
                        }
                        if (dtdata.Columns.Contains("WaterSource_Distance"))
                        {
                            vd.WaterSource_Distance = dtdata.Rows[0]["WaterSource_Distance"].ToString();
                        }
                        else
                        {
                            vd.WaterSource_Distance = "N/A";
                        }
                        if (dtdata.Columns.Contains("Forest_Distance"))
                        {
                            vd.Forest_Distance = dtdata.Rows[0]["Forest_Distance"].ToString();
                        }
                        else
                        {
                            vd.Forest_Distance = "N/A";
                        }
                        if (dtdata.Columns.Contains("Wildlife_Distance"))
                        {
                            vd.Wildlife_Distance = dtdata.Rows[0]["Wildlife_Distance"].ToString();
                        }
                        else
                        {
                            vd.Wildlife_Distance = "N/A";
                        }
                        if (dtdata.Columns.Contains("Tree_species"))
                        {
                            vd.Tree_species = dtdata.Rows[0]["Tree_species"].ToString();
                        }
                        else
                        {
                            vd.Tree_species = "N/A";
                        }
                        if (dtdata.Columns.Contains("AravalliHills"))
                        {
                            if (dtdata.Rows[0]["AravalliHills"].ToString() == "1")
                            {
                                vd.AravalliHills = "True";
                            }
                            else
                            {
                                vd.AravalliHills = "False";
                            }
                        }
                        else
                        {
                            vd.AravalliHills = "N/A";
                        }
                        if (dtdata.Columns.Contains("ForestLand"))
                        {
                            if (dtdata.Rows[0]["ForestLand"].ToString() == "1")
                            {
                                vd.ForestLand = "True";
                            }
                            else
                            {
                                vd.ForestLand = "False";
                            }
                        }
                        else
                        {
                            vd.ForestLand = "N/A";
                        }
                        if (dtdata.Columns.Contains("Plantation_Area"))
                        {
                            if (dtdata.Rows[0]["Plantation_Area"].ToString() == "1")
                            {
                                vd.Plantation_Area = "True";
                            }
                            else
                            {
                                vd.Plantation_Area = "False";
                            }
                        }
                        else
                        {
                            vd.Plantation_Area = "N/A";
                        }
                    }
                    else
                    {
                        vd.IsGTSheetAvaliable = "False";
                        vd.Nearest_WaterSource = "N/A";
                        vd.WaterSource_Distance = "N/A";
                        vd.Forest_Distance = "N/A";
                        vd.Wildlife_Distance = "N/A";
                        vd.Tree_species = "N/A";
                        vd.AravalliHills = "N/A";
                        vd.ForestLand = "N/A";
                        vd.Plantation_Area = "N/A";
                    }
                    //}
                    //else
                    //{
                    //    vd.IsGTSheetAvaliable = "False";
                    //    vd.Nearest_WaterSource = "N/A";
                    //    vd.WaterSource_Distance = "N/A";
                    //    vd.Forest_Distance = "N/A";
                    //    vd.Wildlife_Distance = "N/A";
                    //    vd.Tree_species = "N/A";
                    //    vd.AravalliHills = "N/A";
                    //    vd.ForestLand = "N/A";
                    //    vd.Plantation_Area = "N/A";
                    //}
                }
                else
                {
                    vd.IsGTSheetAvaliable = "N/A";
                    vd.Nearest_WaterSource = "N/A";
                    vd.WaterSource_Distance = "N/A";
                    vd.Forest_Distance = "N/A";
                    vd.Wildlife_Distance = "N/A";
                    vd.Tree_species = "N/A";
                    vd.AravalliHills = "N/A";
                    vd.ForestLand = "N/A";
                    vd.Plantation_Area = "N/A";
                }
                if (dtdata.Columns.Contains("Animal_Sno"))
                {
                    vd.Animal_SerialNo = dtdata.Rows[0]["Animal_Sno"].ToString();
                    if (vd.Animal_SerialNo == null || vd.Animal_SerialNo == "")
                    {
                        vd.Animal_SerialNo = "N/A";
                    }
                }
                else
                {
                    vd.Animal_SerialNo = "N/A";
                }
                if (dtdata.Columns.Contains("Species_Sno"))
                {
                    vd.Species_SerialNo = dtdata.Rows[0]["Species_Sno"].ToString();
                    if (vd.Species_SerialNo == null || vd.Species_SerialNo == "")
                    {
                        vd.Species_SerialNo = "N/A";
                    }
                }
                else
                {
                    vd.Species_SerialNo = "N/A";
                }
                if (dtdata.Columns.Contains("Benefits"))
                {
                    vd.Benefits = dtdata.Rows[0]["Benefits"].ToString();
                    if (vd.Benefits == null || vd.Benefits == "")
                    {
                        vd.Benefits = "N/A";
                    }
                }
                else
                {
                    vd.Benefits = "N/A";
                }
                if (dtdata.Columns.Contains("CoordinatorId"))
                {
                    vd.CoordinatorId = dtdata.Rows[0]["CoordinatorId"].ToString();
                    if (vd.CoordinatorId == null || vd.CoordinatorId == "")
                    {
                        vd.CoordinatorId = "N/A";
                    }
                }
                else
                {
                    vd.CoordinatorId = "N/A";
                }
                if (dtdata.Columns.Contains("Description"))
                {
                    vd.Description = dtdata.Rows[0]["Description"].ToString();
                    if (vd.Description == null || vd.Description == "")
                    {
                        vd.Description = "N/A";
                    }
                }
                else
                {
                    vd.Description = "N/A";
                }
                if (dtdata.Columns.Contains("IndianCitizen"))
                {
                    vd.IndianCitizen = dtdata.Rows[0]["IndianCitizen"].ToString();
                    if (vd.IndianCitizen == null || vd.IndianCitizen == "")
                    {
                        vd.IndianCitizen = "N/A";
                    }
                }
                else
                {
                    vd.IndianCitizen = "N/A";
                }
                if (dtdata.Columns.Contains("NonIndianCitizen"))
                {
                    vd.NonIndianCitizen = dtdata.Rows[0]["NonIndianCitizen"].ToString();
                    if (vd.NonIndianCitizen == null || vd.NonIndianCitizen == "")
                    {
                        vd.NonIndianCitizen = "N/A";
                    }
                }
                else
                {
                    vd.NonIndianCitizen = "N/A";
                }
                if (dtdata.Columns.Contains("Students"))
                {
                    vd.Students = dtdata.Rows[0]["Students"].ToString();
                    if (vd.Students == null || vd.Students == "")
                    {
                        vd.Students = "N/A";
                    }
                }
                else
                {
                    vd.Students = "N/A";
                }
                if (dtdata.Columns.Contains("OffenderName"))
                {
                    vd.OffenderName = dtdata.Rows[0]["OffenderName"].ToString();
                    if (vd.OffenderName == null || vd.OffenderName == "")
                    {
                        vd.OffenderName = "N/A";
                    }
                }
                else
                {
                    vd.OffenderName = "N/A";
                }
                if (dtdata.Columns.Contains("FatherName"))
                {
                    vd.FatherName = dtdata.Rows[0]["FatherName"].ToString();
                    if (vd.FatherName == null || vd.FatherName == "")
                    {
                        vd.FatherName = "N/A";
                    }
                }
                else
                {
                    vd.FatherName = "N/A";
                }
                if (dtdata.Columns.Contains("Caste"))
                {
                    vd.Caste = dtdata.Rows[0]["Caste"].ToString();
                    if (vd.Caste == null || vd.Caste == "")
                    {
                        vd.Caste = "N/A";
                    }
                }
                else
                {
                    vd.Caste = "N/A";
                }
                if (dtdata.Columns.Contains("Address1"))
                {
                    vd.Address = dtdata.Rows[0]["Address1"].ToString();
                    if (vd.Address == null || vd.Address == "")
                    {
                        vd.Address = "N/A";
                    }
                }
                else
                {
                    vd.Address = "N/A";
                }
                if (dtdata.Columns.Contains("PhoneNo"))
                {
                    vd.PhoneNo = dtdata.Rows[0]["PhoneNo"].ToString();
                    if (vd.PhoneNo == null || vd.PhoneNo == "")
                    {
                        vd.PhoneNo = "N/A";
                    }
                }
                else
                {
                    vd.PhoneNo = "N/A";
                }
                if (dtdata.Columns.Contains("OffenderAge"))
                {
                    vd.OffenderAge = dtdata.Rows[0]["OffenderAge"].ToString();
                    if (vd.OffenderAge == null || vd.OffenderAge == "")
                    {
                        vd.OffenderAge = "N/A";
                    }
                }
                else
                {
                    vd.OffenderAge = "N/A";
                }
                if (dtdata.Columns.Contains("PoliceStation"))
                {
                    vd.PoliceStation = dtdata.Rows[0]["PoliceStation"].ToString();
                    if (vd.PoliceStation == null || vd.PoliceStation == "")
                    {
                        vd.PoliceStation = "N/A";
                    }
                }
                else
                {
                    vd.PoliceStation = "N/A";
                }
                if (dtdata.Columns.Contains("ClothesWorn"))
                {
                    vd.ClothesWorn = dtdata.Rows[0]["ClothesWorn"].ToString();
                    if (vd.ClothesWorn == null || vd.ClothesWorn == "")
                    {
                        vd.ClothesWorn = "N/A";
                    }
                }
                else
                {
                    vd.ClothesWorn = "N/A";
                }
                if (dtdata.Columns.Contains("ClothesColor"))
                {
                    vd.ClothesColor = dtdata.Rows[0]["ClothesColor"].ToString();
                    if (vd.ClothesColor == null || vd.ClothesColor == "")
                    {
                        vd.ClothesColor = "N/A";
                    }
                }
                else
                {
                    vd.ClothesColor = "N/A";
                }
                if (dtdata.Columns.Contains("PhysicalAppearance"))
                {
                    vd.PhysicalAppearance = dtdata.Rows[0]["PhysicalAppearance"].ToString();
                    if (vd.PhysicalAppearance == null || vd.PhysicalAppearance == "")
                    {
                        vd.PhysicalAppearance = "N/A";
                    }
                }
                else
                {
                    vd.PhysicalAppearance = "N/A";
                }
                if (dtdata.Columns.Contains("Height"))
                {
                    vd.Height = dtdata.Rows[0]["Height"].ToString();
                    if (vd.Height == null || vd.Height == "")
                    {
                        vd.Height = "N/A";
                    }
                }
                else
                {
                    vd.Height = "N/A";
                }
                if (dtdata.Columns.Contains("EvidenceDocURL"))
                {
                    string strproof = dtdata.Rows[0]["EvidenceDocURL"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    if (strproofsplit.Length > 0) { vd.EvidenceDoc = strproofsplit[strproofsplit.Length - 1]; } else { vd.EvidenceDoc = "N/A"; }
                }
                else
                {
                    vd.EvidenceDoc = "N/A";
                }
                #region MicroPlan
                if (vd.TableName == "tbl_FDM_MicroPlan")
                {
                    if (dtdata.Columns.Contains("MicroPlanName"))
                    {
                        vd.MicroPlanName = dtdata.Rows[0]["MicroPlanName"].ToString();
                        if (vd.MicroPlanName == null || vd.MicroPlanName == "")
                        {
                            vd.MicroPlanName = "N/A";
                        }
                    }
                    else
                    {
                        vd.MicroPlanName = "N/A";
                    }
                    if (dtdata.Columns.Contains("MicroPlan_Code"))
                    {
                        vd.MicroPlanCode = dtdata.Rows[0]["MicroPlan_Code"].ToString();
                        if (vd.MicroPlanCode == null || vd.MicroPlanCode == "")
                        {
                            vd.MicroPlanCode = "N/A";
                        }
                    }
                    else
                    {
                        vd.MicroPlanCode = "N/A";
                    }
                    DataSet dsMicroProj = new DataSet();
                    dsMicroProj = ar.GetMultiMicroPlanProj(vd.RequestId);
                    if (dsMicroProj.Tables[0].Columns.Contains("Project_Name"))
                    {
                        StringBuilder sbMultProj = new StringBuilder();
                        if (dsMicroProj.Tables.Count > 0)
                        {
                            if (dsMicroProj.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMicroProj.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultProj.ToString().Contains(dsMicroProj.Tables[0].Rows[i][0].ToString()))
                                    {
                                        sbMultProj.Append(dsMicroProj.Tables[0].Rows[i][0].ToString() + " ,");
                                    }
                                }
                                if (sbMultProj.Length > 0)
                                {
                                    vd.MicroProjectName = sbMultProj.ToString().Remove(sbMultProj.ToString().Length - 1, 1);
                                }
                                else { vd.MicroProjectName = "N/A"; }
                            }
                            else { vd.MicroProjectName = "N/A"; }
                        }
                        else { vd.MicroProjectName = "N/A"; }
                    }
                    else { vd.MicroProjectName = "N/A"; }
                    if (dtdata.Columns.Contains("DateOfRequest"))
                    {
                        vd.MicroDateOfRequest = dtdata.Rows[0]["DateOfRequest"].ToString();
                        if (vd.MicroDateOfRequest == null || vd.MicroDateOfRequest == "")
                        {
                            vd.MicroDateOfRequest = "N/A";
                        }
                    }
                    else
                    {
                        vd.MicroDateOfRequest = "N/A";
                    }
                    if (dtdata.Columns.Contains("NGOorSHG"))
                    {
                        vd.NGO_SHG = dtdata.Rows[0]["NGOorSHG"].ToString();
                        if (vd.NGO_SHG == null || vd.NGO_SHG == "")
                        {
                            vd.NGO_SHG = "N/A";
                        }
                    }
                    else
                    {
                        vd.NGO_SHG = "N/A";
                    }
                    if (dtdata.Columns.Contains("NGOSHGName"))
                    {
                        vd.NGO_SHGName = dtdata.Rows[0]["NGOSHGName"].ToString();
                        if (vd.NGO_SHGName == null || vd.NGO_SHGName == "")
                        {
                            vd.NGO_SHGName = "N/A";
                        }
                    }
                    else
                    {
                        vd.NGO_SHGName = "N/A";
                    }
                    if (dtdata.Columns.Contains("RevenueVillage"))
                    {
                        vd.RevenueVillage = dtdata.Rows[0]["RevenueVillage"].ToString();
                        if (vd.RevenueVillage == null || vd.RevenueVillage == "")
                        {
                            vd.RevenueVillage = "N/A";
                        }
                    }
                    else
                    {
                        vd.RevenueVillage = "N/A";
                    }
                    if (dtdata.Columns.Contains("PanchayatComittee"))
                    {
                        vd.PanchayatComittee = dtdata.Rows[0]["PanchayatComittee"].ToString();
                        if (vd.PanchayatComittee == null || vd.PanchayatComittee == "")
                        {
                            vd.PanchayatComittee = "N/A";
                        }
                    }
                    else
                    {
                        vd.PanchayatComittee = "N/A";
                    }
                    if (dtdata.Columns.Contains("ForestAdminUnit"))
                    {
                        vd.ForestAdminUnit = dtdata.Rows[0]["ForestAdminUnit"].ToString();
                        if (vd.ForestAdminUnit == null || vd.ForestAdminUnit == "")
                        {
                            vd.ForestAdminUnit = "N/A";
                        }
                    }
                    else
                    {
                        vd.ForestAdminUnit = "N/A";
                    }
                    if (dtdata.Columns.Contains("RangeOfficeUnit"))
                    {
                        vd.RangeOfficeUnit = dtdata.Rows[0]["RangeOfficeUnit"].ToString();
                        if (vd.RangeOfficeUnit == null || vd.RangeOfficeUnit == "")
                        {
                            vd.RangeOfficeUnit = "N/A";
                        }
                    }
                    else
                    {
                        vd.RangeOfficeUnit = "N/A";
                    }
                    if (dtdata.Columns.Contains("Totalarea"))
                    {
                        vd.TotalArea = dtdata.Rows[0]["Totalarea"].ToString();
                        if (vd.TotalArea == null || vd.TotalArea == "")
                        {
                            vd.TotalArea = "N/A";
                        }
                    }
                    else
                    {
                        vd.TotalArea = "N/A";
                    }
                    if (dtdata.Columns.Contains("TotalLandAreaSQKM"))
                    {
                        vd.TotalLandsqkm = dtdata.Rows[0]["TotalLandAreaSQKM"].ToString();
                        if (vd.TotalLandsqkm == null || vd.TotalLandsqkm == "")
                        {
                            vd.TotalLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.TotalLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("TotalForestAreaSQKM"))
                    {
                        vd.TotalForestsqkm = dtdata.Rows[0]["TotalForestAreaSQKM"].ToString();
                        if (vd.TotalForestsqkm == null || vd.TotalForestsqkm == "")
                        {
                            vd.TotalForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.TotalForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ReservedForestSQKM"))
                    {
                        vd.ReservedForestsqkm = dtdata.Rows[0]["ReservedForestSQKM"].ToString();
                        if (vd.ReservedForestsqkm == null || vd.ReservedForestsqkm == "")
                        {
                            vd.ReservedForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProtectedForestSQKM"))
                    {
                        vd.ProtectForestsqkm = dtdata.Rows[0]["ProtectedForestSQKM"].ToString();
                        if (vd.ProtectForestsqkm == null || vd.ProtectForestsqkm == "")
                        {
                            vd.ProtectForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ProtectForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("UnClassifiedForestSQKM"))
                    {
                        vd.UnclassifiedForestsqkm = dtdata.Rows[0]["UnClassifiedForestSQKM"].ToString();
                        if (vd.UnclassifiedForestsqkm == null || vd.UnclassifiedForestsqkm == "")
                        {
                            vd.UnclassifiedForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.UnclassifiedForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ClassifiedForestSQKM"))
                    {
                        vd.ClassifiedForestsqkm = dtdata.Rows[0]["ClassifiedForestSQKM"].ToString();
                        if (vd.ClassifiedForestsqkm == null || vd.ClassifiedForestsqkm == "")
                        {
                            vd.ClassifiedForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ClassifiedForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("FullyCoveredSQKM"))
                    {
                        vd.FullyCoversqkm = dtdata.Rows[0]["FullyCoveredSQKM"].ToString();
                        if (vd.FullyCoversqkm == null || vd.FullyCoversqkm == "")
                        {
                            vd.FullyCoversqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.FullyCoversqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("WithoutPlantSQKM"))
                    {
                        vd.WithoutPlantsqkm = dtdata.Rows[0]["WithoutPlantSQKM"].ToString();
                        if (vd.WithoutPlantsqkm == null || vd.WithoutPlantsqkm == "")
                        {
                            vd.WithoutPlantsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.WithoutPlantsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("AllocateforPlantSQKM"))
                    {
                        vd.AllocateforPlantsqkm = dtdata.Rows[0]["AllocateforPlantSQKM"].ToString();
                        if (vd.AllocateforPlantsqkm == null || vd.AllocateforPlantsqkm == "")
                        {
                            vd.AllocateforPlantsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.AllocateforPlantsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("PanchayatLandSQKM"))
                    {
                        vd.PanchayatLandsqkm = dtdata.Rows[0]["PanchayatLandSQKM"].ToString();
                        if (vd.PanchayatLandsqkm == null || vd.PanchayatLandsqkm == "")
                        {
                            vd.PanchayatLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.PanchayatLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("RevenueLandSQKM"))
                    {
                        vd.RevenueLandsqkm = dtdata.Rows[0]["PanchayatLandSQKM"].ToString();
                        if (vd.RevenueLandsqkm == null || vd.RevenueLandsqkm == "")
                        {
                            vd.RevenueLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.RevenueLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("AgricultureLand"))
                    {
                        vd.AgricultureLand = dtdata.Rows[0]["AgricultureLand"].ToString();
                        if (vd.AgricultureLand == null || vd.AgricultureLand == "")
                        {
                            vd.AgricultureLand = "N/A";
                        }
                    }
                    else
                    {
                        vd.AgricultureLand = "N/A";
                    }
                    if (dtdata.Columns.Contains("IrregatedLandSQKM"))
                    {
                        vd.IrregatedLandsqkm = dtdata.Rows[0]["IrregatedLandSQKM"].ToString();
                        if (vd.IrregatedLandsqkm == null || vd.IrregatedLandsqkm == "")
                        {
                            vd.IrregatedLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.IrregatedLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("NonIrregatedLandSQKM"))
                    {
                        vd.NonIrregatedLandsqkm = dtdata.Rows[0]["NonIrregatedLandSQKM"].ToString();
                        if (vd.NonIrregatedLandsqkm == null || vd.NonIrregatedLandsqkm == "")
                        {
                            vd.NonIrregatedLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.NonIrregatedLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ResidentialAreaSQKM"))
                    {
                        vd.ResidentialAreasqkm = dtdata.Rows[0]["ResidentialAreaSQKM"].ToString();
                        if (vd.ResidentialAreasqkm == null || vd.ResidentialAreasqkm == "")
                        {
                            vd.ResidentialAreasqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ResidentialAreasqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("RemAgricultureLandSQKM"))
                    {
                        vd.RemAgricultureLandsqkm = dtdata.Rows[0]["RemAgricultureLandSQKM"].ToString();
                        if (vd.RemAgricultureLandsqkm == null || vd.RemAgricultureLandsqkm == "")
                        {
                            vd.RemAgricultureLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.RemAgricultureLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("RemNonAgricultureLandSQKM"))
                    {
                        vd.RemNonAgricultureLandsqkm = dtdata.Rows[0]["RemAgricultureLandSQKM"].ToString();
                        if (vd.RemNonAgricultureLandsqkm == null || vd.RemNonAgricultureLandsqkm == "")
                        {
                            vd.RemNonAgricultureLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.RemNonAgricultureLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("JFMCName"))
                    {
                        vd.JFMCName = dtdata.Rows[0]["JFMCName"].ToString();
                        if (vd.JFMCName == null || vd.JFMCName == "")
                        {
                            vd.JFMCName = "N/A";
                        }
                    }
                    else
                    {
                        vd.JFMCName = "N/A";
                    }
                }
                #endregion
                #region WorkOrderInvoice
                if (dtdata.Columns.Contains("WorkOrder_Name"))
                {
                    vd.WorkOrderName = dtdata.Rows[0]["WorkOrder_Name"].ToString();
                    if (vd.WorkOrderName == null || vd.WorkOrderName == "")
                    {
                        vd.WorkOrderName = "N/A";
                    }
                }
                else
                {
                    vd.WorkOrderName = "N/A";
                }
                if (dtdata.Columns.Contains("WorkOrder_Code"))
                {
                    vd.workorderCode = dtdata.Rows[0]["WorkOrder_Code"].ToString();
                    if (vd.workorderCode == null || vd.workorderCode == "")
                    {
                        vd.workorderCode = "N/A";
                    }
                }
                else
                {
                    vd.workorderCode = "N/A";
                }
                if (dtdata.Columns.Contains("MileStoneName"))
                {
                    vd.MileStoneName = dtdata.Rows[0]["MileStoneName"].ToString();
                    if (vd.MileStoneName == null || vd.MileStoneName == "")
                    {
                        vd.MileStoneName = "N/A";
                    }
                }
                else
                {
                    vd.MileStoneName = "N/A";
                }
                if (dtdata.Columns.Contains("MileStonePaymentPercentage"))
                {
                    vd.MileStonePaymentPercentage = dtdata.Rows[0]["MileStonePaymentPercentage"].ToString();
                    if (vd.MileStonePaymentPercentage == null || vd.MileStonePaymentPercentage == "")
                    {
                        vd.MileStonePaymentPercentage = "N/A";
                    }
                }
                else
                {
                    vd.MileStonePaymentPercentage = "N/A";
                }
                if (dtdata.Columns.Contains("isBillRaised"))
                {
                    vd.BillRaised = dtdata.Rows[0]["isBillRaised"].ToString();
                }
                else
                {
                    vd.MileStonePaymentPercentage = "N/A";
                }
                if (dtdata.Columns.Contains("BillAmount"))
                {
                    vd.BillAmount = dtdata.Rows[0]["BillAmount"].ToString();
                    if (vd.BillAmount == null || vd.BillAmount == "")
                    {
                        vd.BillAmount = "N/A";
                    }
                }
                else
                {
                    vd.BillAmount = "N/A";
                }
                if (dtdata.Columns.Contains("BillDate"))
                {
                    DateTime _date;
                    _date = DateTime.Parse(dtdata.Rows[0]["BillDate"].ToString());
                    vd.BillDate = _date.ToString("dd-MM-yyyy");
                    if (vd.BillDate == null || vd.BillDate == "")
                    {
                        vd.BillDate = "N/A";
                    }
                }
                else
                {
                    vd.BillDate = "N/A";
                }
                if (dtdata.Columns.Contains("BillNo"))
                {
                    vd.BillNo = dtdata.Rows[0]["BillNo"].ToString();
                    if (vd.BillNo == null || vd.BillNo == "")
                    {
                        vd.BillNo = "N/A";
                    }
                }
                else
                {
                    vd.BillNo = "N/A";
                }
                #endregion
                #region "Add notice by Arvind"
                if (vd.TableName == "tbl_mst_Notice" || vd.TableName == "tbl_FDM_BudgetReviewApproval")
                {
                    if (dtdata.Columns.Contains("Id"))
                    {
                        vd.Notice_Id = dtdata.Rows[0]["Id"].ToString();
                        if (vd.Notice_Id == null || vd.Notice_Id == "")
                        {
                            vd.Notice_Id = "N/A";
                        }
                    }
                    else
                    {
                        vd.Notice_Id = "N/A";
                    }
                    if (dtdata.Columns.Contains("Notice_Number"))
                    {
                        vd.Notice_No = dtdata.Rows[0]["Notice_Number"].ToString();
                        if (vd.Notice_No == null || vd.Notice_No == "")
                        {
                            vd.Notice_No = "N/A";
                        }
                    }
                    else
                    {
                        vd.Notice_No = "N/A";
                    }
                    if (dtdata.Columns.Contains("REG_NAME"))
                    {
                        vd.Region_Name = dtdata.Rows[0]["REG_NAME"].ToString();
                        if (vd.Region_Name == null || vd.Region_Name == "")
                        {
                            vd.Region_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Region_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("CIRCLE_NAME"))
                    {
                        vd.Circle_Name = dtdata.Rows[0]["CIRCLE_NAME"].ToString();
                        if (vd.Circle_Name == null || vd.Circle_Name == "")
                        {
                            vd.Circle_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Circle_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                        if (vd.Division_Name == null || vd.Division_Name == "")
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Division_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("RANGE_NAME"))
                    {
                        vd.Range_Name = dtdata.Rows[0]["RANGE_NAME"].ToString();
                        if (vd.Range_Name == null || vd.Range_Name == "")
                        {
                            vd.Range_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Range_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("Depot_Name"))
                    {
                        vd.Depot_Name = dtdata.Rows[0]["Depot_Name"].ToString();
                        if (vd.Depot_Name == null || vd.Depot_Name == "")
                        {
                            vd.Depot_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Depot_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        vd.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                        if (vd.Produce_Name == null || vd.Produce_Name == "")
                        {
                            vd.Produce_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProductName"))
                    {
                        vd.Product_Name = dtdata.Rows[0]["ProductName"].ToString();
                        if (vd.Product_Name == null || vd.Product_Name == "")
                        {
                            vd.Product_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Product_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("UnitName"))
                    {
                        vd.Produce_Unit = dtdata.Rows[0]["UnitName"].ToString();
                        if (vd.Produce_Unit == null || vd.Produce_Unit == "")
                        {
                            vd.Produce_Unit = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Unit = "N/A";
                    }
                    if (dtdata.Columns.Contains("Quantity"))
                    {
                        vd.Qty = dtdata.Rows[0]["Quantity"].ToString();
                        if (vd.Qty == null || vd.Qty == "")
                        {
                            vd.Qty = "N/A";
                        }
                    }
                    else
                    {
                        vd.Qty = "N/A";
                    }
                    if (dtdata.Columns.Contains("ReservedPrice"))
                    {
                        vd.ReservedPrice = dtdata.Rows[0]["ReservedPrice"].ToString();
                        if (vd.ReservedPrice == null || vd.ReservedPrice == "")
                        {
                            vd.ReservedPrice = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedPrice = "N/A";
                    }


                }
                #endregion
                #region "Add Apply for Auction by Arvind"
                if (vd.TableName == "tbl_AuctionDetail")
                {
                    if (dtdata.Columns.Contains("Applicant_Type"))
                    {
                        if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                        {
                            vd.Applicant_Type = "Individual";
                        }
                        else
                        {
                            vd.Applicant_Type = "Organizational";
                        }
                    }
                    else
                    {
                        vd.Applicant_Type = "N/A";
                    }
                    if (dtdata.Columns.Contains("BidderName"))
                    {
                        vd.BidderName = dtdata.Rows[0]["BidderName"].ToString();
                        if (vd.BidderName == null || vd.BidderName == "")
                        {
                            vd.BidderName = "N/A";
                        }
                    }
                    else
                    {
                        vd.BidderName = "N/A";
                    }
                    if (dtdata.Columns.Contains("Notice_Number"))
                    {
                        vd.Notice_No = dtdata.Rows[0]["Notice_Number"].ToString();
                        if (vd.Notice_No == null || vd.Notice_No == "")
                        {
                            vd.Notice_No = "N/A";
                        }
                    }
                    else
                    {
                        vd.Notice_No = "N/A";
                    }
                    if (dtdata.Columns.Contains("DurationFrom"))
                    {
                        DateTime _date1, _date2;
                        _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                        _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                        vd.DurationsFrom = _date1.ToString("dd/MM/yyyy");
                        vd.DurationsTo = _date2.ToString("dd/MM/yyyy");
                        //ar.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        vd.DurationsFrom = "N/A";
                        vd.DurationsTo = "N/A";
                    }
                    if (dtdata.Columns.Contains("REG_NAME"))
                    {
                        vd.Region_Name = dtdata.Rows[0]["REG_NAME"].ToString();
                        if (vd.Region_Name == null || vd.Region_Name == "")
                        {
                            vd.Region_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Region_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("CIRCLE_NAME"))
                    {
                        vd.Circle_Name = dtdata.Rows[0]["CIRCLE_NAME"].ToString();
                        if (vd.Circle_Name == null || vd.Circle_Name == "")
                        {
                            vd.Circle_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Circle_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                        if (vd.Division_Name == null || vd.Division_Name == "")
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Division_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("RANGE_NAME"))
                    {
                        vd.Range_Name = dtdata.Rows[0]["RANGE_NAME"].ToString();
                        if (vd.Range_Name == null || vd.Range_Name == "")
                        {
                            vd.Range_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Range_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("Depot_Name"))
                    {
                        vd.Depot_Name = dtdata.Rows[0]["Depot_Name"].ToString();
                        if (vd.Depot_Name == null || vd.Depot_Name == "")
                        {
                            vd.Depot_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Depot_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        vd.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                        if (vd.Produce_Name == null || vd.Produce_Name == "")
                        {
                            vd.Produce_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        vd.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                        if (vd.Produce_Name == null || vd.Produce_Name == "")
                        {
                            vd.Produce_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("UnitName"))
                    {
                        vd.Produce_Unit = dtdata.Rows[0]["UnitName"].ToString();
                        if (vd.Produce_Unit == null || vd.Produce_Unit == "")
                        {
                            vd.Produce_Unit = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Unit = "N/A";
                    }
                    if (dtdata.Columns.Contains("Quantity"))
                    {
                        vd.Qty = dtdata.Rows[0]["Quantity"].ToString();
                        if (vd.Qty == null || vd.Qty == "")
                        {
                            vd.Qty = "N/A";
                        }
                    }
                    else
                    {
                        vd.Qty = "N/A";
                    }
                    if (dtdata.Columns.Contains("ReservedPrice"))
                    {
                        vd.ReservedPrice = dtdata.Rows[0]["ReservedPrice"].ToString();
                        if (vd.ReservedPrice == null || vd.ReservedPrice == "")
                        {
                            vd.ReservedPrice = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedPrice = "N/A";
                    }
                    if (dtdata.Columns.Contains("Payment_Mode"))
                    {
                        vd.PaymentMode = dtdata.Rows[0]["Payment_Mode"].ToString();
                        if (vd.PaymentMode == null || vd.PaymentMode == "")
                        {
                            vd.PaymentMode = "N/A";
                        }
                    }
                    else
                    {
                        vd.PaymentMode = "N/A";
                    }
                    if (dtdata.Columns.Contains("BiddingAmount"))
                    {
                        vd.BiddingAmount = dtdata.Rows[0]["BiddingAmount"].ToString();
                        if (vd.BiddingAmount == null || vd.BiddingAmount == "")
                        {
                            vd.BiddingAmount = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedPrice = "N/A";
                    }
                    if (dtdata.Columns.Contains("Emd_Amount"))
                    {
                        vd.EmdPaybleAmount = dtdata.Rows[0]["Emd_Amount"].ToString();
                        if (vd.EmdPaybleAmount == null || vd.EmdPaybleAmount == "")
                        {
                            vd.EmdPaybleAmount = "N/A";
                        }
                    }
                    else
                    {
                        vd.EmdPaybleAmount = "N/A";
                    }
                    if (dtdata.Columns.Contains("Payment_By"))
                    {
                        vd.OfflinePaymentMode = dtdata.Rows[0]["Payment_By"].ToString();
                        if (vd.OfflinePaymentMode == null || vd.OfflinePaymentMode == "")
                        {
                            vd.OfflinePaymentMode = "N/A";
                        }
                    }
                    else
                    {
                        vd.OfflinePaymentMode = "N/A";
                    }
                    if (dtdata.Columns.Contains("Bank_Name"))
                    {
                        vd.BankName = dtdata.Rows[0]["Bank_Name"].ToString();
                        if (vd.BankName == null || vd.BankName == "")
                        {
                            vd.BankName = "N/A";
                        }
                    }
                    else
                    {
                        vd.BankName = "N/A";
                    }
                    if (dtdata.Columns.Contains("Branch_Name"))
                    {
                        vd.BranchName = dtdata.Rows[0]["Branch_Name"].ToString();
                        if (vd.BranchName == null || vd.BranchName == "")
                        {
                            vd.BranchName = "N/A";
                        }
                    }
                    else
                    {
                        vd.BranchName = "N/A";
                    }
                    if (dtdata.Columns.Contains("DD_Chk_IssueDate"))
                    {
                        vd.DdchkIssuesDate = dtdata.Rows[0]["DD_Chk_IssueDate"].ToString();
                        if (vd.DdchkIssuesDate == null || vd.DdchkIssuesDate == "")
                        {
                            vd.DdchkIssuesDate = "N/A";
                        }
                    }
                    else
                    {
                        vd.DdchkIssuesDate = "N/A";
                    }
                    if (dtdata.Columns.Contains("DD_Chk_Number"))
                    {
                        vd.DdChkNumber = dtdata.Rows[0]["DD_Chk_Number"].ToString();
                        if (vd.DdChkNumber == null || vd.DdChkNumber == "")
                        {
                            vd.DdChkNumber = "N/A";
                        }
                    }
                    else
                    {
                        vd.DdChkNumber = "N/A";
                    }
                    if (dtdata.Columns.Contains("DD_Chk_Filepath"))
                    {
                        string str = dtdata.Rows[0]["DD_Chk_Filepath"].ToString();
                        string[] strsplit = str.Split('/');
                        vd.DdchkFilepth = strsplit[strsplit.Length - 1];
                        if (vd.DdchkFilepth == null || vd.DdchkFilepth == "")
                        {
                            vd.DdchkFilepth = "N/A";
                        }
                    }
                    else
                    {
                        vd.DdchkFilepth = "N/A";
                    }
                    if (dtdata.Columns.Contains("PS_Amount"))
                    {
                        vd.PsPaybleAmount = dtdata.Rows[0]["PS_Amount"].ToString();
                        if (vd.PsPaybleAmount == null || vd.PsPaybleAmount == "")
                        {
                            vd.PsPaybleAmount = "N/A";
                        }
                    }
                    else
                    {
                        vd.PsPaybleAmount = "N/A";
                    }
                    if (dtdata.Columns.Contains("Drop_Out_Reasond"))
                    {
                        vd.DropOutReason = dtdata.Rows[0]["Drop_Out_Reasond"].ToString();
                        if (vd.DropOutReason == null || vd.DropOutReason == "")
                        {
                            vd.DropOutReason = "N/A";
                        }
                    }
                    else
                    {
                        vd.DropOutReason = "N/A";
                    }
                }
                #endregion
                #region "Add Apply for Transit Permit by Arvind"
                if (vd.TableName == "tbl_TransitPermission")
                {
                    if (dtdata.Columns.Contains("Applicant_Type"))
                    {
                        if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                        {
                            vd.Applicant_Type = "Individual";
                        }
                        else
                        {
                            vd.Applicant_Type = "Organizational";
                        }
                    }
                    else
                    {
                        vd.Applicant_Type = "N/A";
                    }
                    if (dtdata.Columns.Contains("RequestID"))
                    {
                        vd.ReqID = dtdata.Rows[0]["RequestID"].ToString();
                        if (vd.ReqID == null || vd.ReqID == "")
                        {
                            vd.ReqID = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReqID = "N/A";
                    }
                    if (dtdata.Columns.Contains("ToLocation"))
                    {
                        vd.Location = dtdata.Rows[0]["ToLocation"].ToString();
                        if (vd.Location == null || vd.Location == "")
                        {
                            vd.Location = "N/A";
                        }
                    }
                    else
                    {
                        vd.Location = "N/A";
                    }
                    if (dtdata.Columns.Contains("TransportMode"))
                    {
                        vd.TransportMode = dtdata.Rows[0]["TransportMode"].ToString();
                        if (vd.TransportMode == null || vd.TransportMode == "")
                        {
                            vd.TransportMode = "N/A";
                        }
                    }
                    else
                    {
                        vd.TransportMode = "N/A";
                    }
                    if (dtdata.Columns.Contains("VehicleNo"))
                    {
                        vd.VehicleNo = dtdata.Rows[0]["VehicleNo"].ToString();
                        if (vd.VehicleNo == null || vd.VehicleNo == "")
                        {
                            vd.VehicleNo = "N/A";
                        }
                    }
                    else
                    {
                        vd.VehicleNo = "N/A";
                    }
                    if (dtdata.Columns.Contains("DriverLicense"))
                    {
                        vd.DriverLicense = dtdata.Rows[0]["DriverLicense"].ToString();
                        if (vd.DriverLicense == null || vd.DriverLicense == "")
                        {
                            vd.DriverLicense = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverLicense = "N/A";
                    }
                    if (dtdata.Columns.Contains("DriverName"))
                    {
                        vd.DriverName = dtdata.Rows[0]["DriverName"].ToString();
                        if (vd.DriverName == null || vd.DriverName == "")
                        {
                            vd.DriverName = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverName = "N/A";
                    }
                    if (dtdata.Columns.Contains("DriverMobile"))
                    {
                        vd.Depot_Name = dtdata.Rows[0]["DriverMobile"].ToString();
                        if (vd.DriverMobileno == null || vd.DriverMobileno == "")
                        {
                            vd.DriverMobileno = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverMobileno = "N/A";
                    }
                    if (dtdata.Columns.Contains("DurationFrom"))
                    {
                        DateTime _date1, _date2;
                        _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                        _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                        vd.DurationsFrom = _date1.ToString("dd/MM/yyyy");
                        vd.DurationsTo = _date2.ToString("dd/MM/yyyy");
                        vd.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        vd.DurationsFrom = "N/A";
                        vd.DurationsTo = "N/A";
                    }
                    if (dtdata.Columns.Contains("AmountPaid"))
                    {
                        vd.PaidAMT = dtdata.Rows[0]["AmountPaid"].ToString();
                        if (vd.PaidAMT == null || vd.PaidAMT == "")
                        {
                            vd.PaidAMT = "N/A";
                        }
                    }
                    else
                    {
                        vd.PaidAMT = "N/A";
                    }
                }
                #endregion
                #region "Add Apply for Education Visit Service by Arvind"
                if (vd.TableName == "tbl_EducationTourPermissions")
                {
                    if (dtdata.Columns.Contains("College_Name"))
                    {
                        vd.CollegeName = dtdata.Rows[0]["College_Name"].ToString();
                        if (vd.CollegeName == null || vd.CollegeName == "")
                        {
                            vd.CollegeName = "N/A";
                        }
                    }
                    else
                    {
                        vd.CollegeName = "N/A";
                    }
                    if (dtdata.Columns.Contains("College_Address"))
                    {
                        vd.CollegeAddress = dtdata.Rows[0]["College_Address"].ToString();
                        if (vd.CollegeAddress == null || vd.CollegeAddress == "")
                        {
                            vd.CollegeAddress = "N/A";
                        }
                    }
                    else
                    {
                        vd.CollegeAddress = "N/A";
                    }
                    if (dtdata.Columns.Contains("College_Phoneno"))
                    {
                        vd.Phonenumber = dtdata.Rows[0]["College_Phoneno"].ToString();
                        if (vd.Phonenumber == null || vd.Phonenumber == "")
                        {
                            vd.Phonenumber = "N/A";
                        }
                    }
                    else
                    {
                        vd.Phonenumber = "N/A";
                    }
                    if (dtdata.Columns.Contains("Category"))
                    {
                        vd.Category = dtdata.Rows[0]["Category"].ToString();
                        if (vd.Category == null || vd.Category == "")
                        {
                            vd.Category = "N/A";
                        }
                    }
                    else
                    {
                        vd.Category = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Name"))
                    {
                        vd.Princeple_Name = dtdata.Rows[0]["P_Name"].ToString();
                        if (vd.Princeple_Name == null || vd.Princeple_Name == "")
                        {
                            vd.Princeple_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Princeple_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Address"))
                    {
                        vd.P_Address = dtdata.Rows[0]["P_Address"].ToString();
                        if (vd.P_Address == null || vd.P_Address == "")
                        {
                            vd.P_Address = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverName = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Gender"))
                    {
                        if (dtdata.Rows[0]["P_Gender"].ToString() == "1")
                        {
                            vd.P_Gender = "Male";
                        }
                        else if (dtdata.Rows[0]["P_Gender"].ToString() == "2")
                        {
                            vd.P_Gender = "Female";
                        }
                        else
                        {
                            vd.P_Gender = "Transgender";
                        }
                        if (vd.P_Gender == null || vd.P_Gender == "")
                        {
                            vd.P_Gender = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_Gender = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Nationality"))
                    {
                        if (dtdata.Rows[0]["P_Nationality"].ToString() == "1")
                        {
                            vd.P_Natinality = "Indian";
                        }
                        else
                        {
                            vd.P_Natinality = "Foreigner";
                        }
                        if (vd.P_Natinality == null || vd.P_Natinality == "")
                        {
                            vd.P_Natinality = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_Natinality = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_MemberType"))
                    {
                        if (dtdata.Rows[0]["P_MemberType"].ToString() == "1")
                        {
                            vd.ddl_MemberType = "Child";
                        }
                        else if (dtdata.Rows[0]["P_MemberType"].ToString() == "2")
                        {
                            vd.ddl_MemberType = "Adult";
                        }
                        else
                        {
                            vd.ddl_MemberType = "Student";
                        }
                        if (vd.ddl_MemberType == null || vd.ddl_MemberType == "")
                        {
                            vd.ddl_MemberType = "N/A";
                        }
                    }
                    else
                    {
                        vd.ddl_MemberType = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_IDType"))
                    {
                        if (dtdata.Rows[0]["P_IDType"].ToString() == "1")
                        {
                            vd.P_MemberID = "Passport";
                        }
                        else if (dtdata.Rows[0]["P_IDType"].ToString() == "2")
                        {
                            vd.P_MemberID = "Aadhar";
                        }
                        else if (dtdata.Rows[0]["P_IDType"].ToString() == "3")
                        {
                            vd.P_MemberID = "Driving Licence";
                        }
                        else if (dtdata.Rows[0]["P_IDType"].ToString() == "4")
                        {
                            vd.P_MemberID = "Voter ID";
                        }
                        else
                        {
                            vd.P_MemberID = "PAN Card";
                        }
                        if (vd.P_MemberID == null || vd.P_MemberID == "")
                        {
                            vd.P_MemberID = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_MemberID = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_IDProofNo"))
                    {
                        vd.P_MemberIDProof = dtdata.Rows[0]["P_IDProofNo"].ToString();
                        if (vd.P_MemberIDProof == null || vd.P_MemberIDProof == "")
                        {
                            vd.P_MemberIDProof = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_MemberIDProof = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_MemberNo"))
                    {
                        vd.P_NumberOfMember = dtdata.Rows[0]["P_MemberNo"].ToString();
                        if (vd.P_NumberOfMember == null || vd.P_NumberOfMember == "")
                        {
                            vd.P_NumberOfMember = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_NumberOfMember = "N/A";
                    }
                    if (dtdata.Columns.Contains("DocMemberFilename"))
                    {
                        vd.MemberListPath = dtdata.Rows[0]["DocMemberFilename"].ToString();
                        if (vd.MemberListPath == null || vd.MemberListPath == "")
                        {
                            vd.MemberListPath = "N/A";
                        }
                    }
                    else
                    {
                        vd.MemberListPath = "N/A";
                    }
                    if (dtdata.Columns.Contains("DocEducationalfilename"))
                    {
                        vd.DocEducationalToueReqPath = dtdata.Rows[0]["DocEducationalfilename"].ToString();
                        if (vd.DocEducationalToueReqPath == null || vd.DocEducationalToueReqPath == "")
                        {
                            vd.DocEducationalToueReqPath = "N/A";
                        }
                    }
                    else
                    {
                        vd.DocEducationalToueReqPath = "N/A";
                    }
                    if (dtdata.Columns.Contains("CategoryName"))
                    {
                        vd.Vehiclecat = dtdata.Rows[0]["CategoryName"].ToString();
                        if (vd.Vehiclecat == null || vd.Vehiclecat == "")
                        {
                            vd.Vehiclecat = "N/A";
                        }
                    }
                    else
                    {
                        vd.Vehiclecat = "N/A";
                    }
                    if (dtdata.Columns.Contains("Vehiclename"))
                    {
                        vd.ddl_vehicle = dtdata.Rows[0]["Vehiclename"].ToString();
                        if (vd.ddl_vehicle == null || vd.ddl_vehicle == "")
                        {
                            vd.ddl_vehicle = "N/A";
                        }
                    }
                    else
                    {
                        vd.ddl_vehicle = "N/A";
                    }
                }
                #endregion
                #endregion
                return Json(vd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ViewDetailsNew(string RequestId, string TableName)
        {
            try
            {
                #region Details
                ActionRequest ar = new ActionRequest();
                viewdetail vd = new viewdetail();
                DataSet dsMultDist = new DataSet();
                DataSet dsdata = new DataSet();
                dsdata = ar.GetActionList(RequestId, TableName);
                DataTable dtdata = dsdata.Tables[0];

                if (TableName == "Tbl_Citizen_TransitPermit" && dtdata != null && dtdata.Rows.Count > 0)
                {
                    int count = 1;
                    StringBuilder SB = new StringBuilder();
                    StringBuilder subSB = new StringBuilder();
                    SB.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                    foreach (DataRow dr in dtdata.Rows)
                    {
                        while (dtdata.Columns.Count > count)
                        {
                            if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("approved"))
                            {
                                subSB.Append("<tr><th>");
                                subSB.Append(dtdata.Columns[count].ColumnName);
                                subSB.Append("</th><td>");
                            }
                            else
                            {
                                SB.Append("<tr><th>");
                                SB.Append(dtdata.Columns[count].ColumnName);
                                SB.Append("</th><td>");
                            }
                            if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower() == "requestid")
                            {
                                SB.Append("<input type='hidden' name='PERMIT_NO' id='PERMIT_NO'  value='" + dtdata.Rows[0][count].ToString() + "' />");
                            }
                            if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("attachement"))
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0][count])))
                                    SB.Append("<a href='" + Convert.ToString(dtdata.Rows[0][count]).Replace("~", string.Empty) + "' target='_blank' rel = 'noopener noreferrer'><img src='../images/jpeg.png' width='30'</a>");
                            }
                            else if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("approved"))
                            {
                                subSB.Append(dtdata.Rows[0][count].ToString());
                                subSB.Append("</td></tr>");
                            }
                            else
                            {
                                SB.Append(dtdata.Rows[0][count].ToString());
                            }
                            SB.Append("</td></tr>");
                            count = Convert.ToInt16(count + 1);
                        }
                    }
                    //SB.Append("</tbody> </table> ");
                    #region Survey Report List
                    dtdata = ar.GetSurveyReportCitizen(RequestId, "Get");
                    if (dtdata != null && dtdata.Rows.Count > 0)
                    {
                        count = 1;
                        SB.Append("<h5>Surery Report</h5>");
                        SB.Append("<table class='table table-striped table-bordered table-hover'><thead><tr>");
                        foreach (DataRow dr in dtdata.Rows)
                        {
                            while (dtdata.Columns.Count > count)
                            {
                                SB.Append("<tr><th>");
                                SB.Append(dtdata.Columns[count].ColumnName);
                                SB.Append("</th><td>");
                                if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower() == "requestid")
                                {
                                    SB.Append("<input type='hidden' name='PERMIT_NO' id='PERMIT_NO'  value='" + dtdata.Rows[0][count].ToString() + "' />");
                                }
                                if (dtdata.Columns[count].ColumnName.ToString().Trim().ToLower().Contains("attachement"))
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dtdata.Rows[0][count])))
                                        SB.Append("<a href='" + Convert.ToString(dtdata.Rows[0][count]).Replace("~", string.Empty) + "' target='_blank' rel = 'noopener noreferrer'><img src='../images/jpeg.png' width='30'</a>");
                                }
                                else
                                {
                                    SB.Append(dtdata.Rows[0][count].ToString());
                                }
                                SB.Append("</td></tr>");
                                count = Convert.ToInt16(count + 1);
                            }
                        }
                        subSB.Append("</tbody> </table> ");
                        SB.Append(subSB);
                    }
                    #endregion
                    SB.Append("</tbody> </table> ");
                    return Json(SB.ToString(), JsonRequestBehavior.AllowGet);
                }
                vd.TableName = TableName;
                vd.RequestId = RequestId;
                vd.ModuleName = dtdata.Rows[0]["ModuleDesc"].ToString();
                vd.ServiceType = dtdata.Rows[0]["ServiceTypeDesc"].ToString();
                vd.PermissionType = dtdata.Rows[0]["PermissionDesc"].ToString();
                vd.PermissionName = dtdata.Rows[0]["SubPermissionDesc"].ToString();
                Session["SubPermissionId"] = dtdata.Rows[0]["SubPermissionId"].ToString();
                if (vd.TableName == "tbl_FixedPermissions")
                {
                    dsMultDist = ar.GetFixedDistMap(vd.RequestId);
                }
                if (dtdata.Columns.Contains("EnteredOn"))
                {
                    DateTime _date;
                    _date = DateTime.Parse(dtdata.Rows[0]["EnteredOn"].ToString());
                    vd.RequestedOn = _date.ToString("dd-MM-yyyy");
                }
                else
                {
                }
                if (dtdata.Columns.Contains("Name"))
                {
                    vd.RequestedBy = dtdata.Rows[0]["Name"].ToString();
                }
                if (string.IsNullOrEmpty(vd.RequestedBy))
                {
                    if (dtdata.Columns.Contains("UserName"))
                    {
                        vd.RequestedBy = dtdata.Rows[0]["UserName"].ToString();
                    }
                }
                if (dtdata.Columns.Contains("DurationFrom"))
                {
                    DateTime _date1, _date2;
                    _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                    _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                    vd.Duration = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                }
                else if (dtdata.Columns.Contains("StartDate"))
                {
                    DateTime _date1, _date2;
                    _date1 = DateTime.Parse(dtdata.Rows[0]["StartDate"].ToString());
                    _date2 = DateTime.Parse(dtdata.Rows[0]["EndDate"].ToString());
                    vd.Duration = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                }
                else
                {
                    if (vd.Duration == null || vd.Duration == "")
                    {
                        vd.Duration = "N/A";
                    }
                }
                if (dtdata.Columns.Contains("trn_Status_Code"))
                {
                    try
                    {
                        vd.DownloadNOC = Convert.ToString(dtdata.Rows[0]["DownloadNOC"]);
                    }
                    catch (Exception) { }
                    if (vd.PermissionType == "Fixed Land Usage" && (vd.PermissionName == "Mining Permission" || vd.PermissionName == "Swmill Permission"))
                    {

                        if (dtdata.Rows[0]["trn_Status_Code"].ToString() == "1")
                        {
                            vd.PaymentDone = "True";

                        }
                        else
                        {
                            vd.PaymentDone = "N/A";
                        }
                    }
                    else if (dtdata.Rows[0]["trn_Status_Code"].ToString() == "1" && vd.PermissionType != "Fixed Land Usage")
                    {
                        vd.PaymentDone = "True";
                    }
                    else
                    {
                        vd.PaymentDone = "N/A";
                    }
                }
                else
                {
                    vd.PaymentDone = "N/A";
                }
                if (dtdata.Columns.Contains("DepositAmount"))
                {
                    vd.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["DepositAmount"]) + Convert.ToDecimal(dtdata.Rows[0]["TotalFees"]);
                }
                else if (dtdata.Columns.Contains("Fees"))
                {
                    vd.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["Fees"]);
                }
                else if (dtdata.Columns.Contains("Final_Amount"))
                {
                    vd.PaidAmount = Convert.ToDecimal(dtdata.Rows[0]["Final_Amount"]);
                }
                else
                {
                    vd.PaidAmount = 0;
                }
                if (dtdata.Columns.Contains("NumberOfCrewMembers"))
                {
                    vd.NumberOfPerson = dtdata.Rows[0]["NumberOfCrewMembers"].ToString();
                }
                else if (dtdata.Columns.Contains("No_Of_Member"))
                {
                    vd.NumberOfPerson = dtdata.Rows[0]["No_Of_Member"].ToString();
                }
                else
                {
                    vd.NumberOfPerson = "N/A";
                }
                if (dtdata.Columns.Contains("ApplicantType"))
                {
                    if (dtdata.Rows[0]["ApplicantType"].ToString() == "1")
                    {
                        vd.ApplicationType = "Individual";
                    }
                    else
                    {
                        vd.ApplicationType = "Organizational";
                    }
                }
                else if (dtdata.Columns.Contains("Applicant_Type"))
                {
                    if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                    {
                        vd.ApplicationType = "Individual";
                    }
                    else
                    {
                        vd.ApplicationType = "Organizational";
                    }
                }
                else
                {
                    vd.ApplicationType = "N/A";
                }
                if (dtdata.Columns.Contains("Title"))
                {
                    vd.FilmTitle = dtdata.Rows[0]["Title"].ToString();
                    if (vd.FilmTitle == null || vd.FilmTitle == "")
                    {
                        vd.FilmTitle = "N/A";
                    }
                }
                else
                {
                    vd.FilmTitle = "N/A";
                }
                if (dtdata.Columns.Contains("ShootingPurpose"))
                {
                    vd.ShootingPurpose = dtdata.Rows[0]["ShootingPurpose"].ToString();
                    if (vd.ShootingPurpose == null || vd.ShootingPurpose == "")
                    {
                        vd.ShootingPurpose = "N/A";
                    }
                }
                else
                {
                    vd.ShootingPurpose = "N/A";
                }
                if (dtdata.Columns.Contains("IdentityProofNo"))
                {
                    vd.IdProofNo = dtdata.Rows[0]["IdentityProofNo"].ToString();
                }
                else if (dtdata.Columns.Contains("IdProofNo"))
                {
                    vd.IdProofNo = dtdata.Rows[0]["IdProofNo"].ToString();
                }
                else
                {
                    vd.IdProofNo = "N/A";
                }
                if (dtdata.Columns.Contains("IDProofUrl"))
                {
                    string strproof = dtdata.Rows[0]["IDProofUrl"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    vd.IdProofUrl = Server.MapPath(strproofsplit[strproofsplit.Length - 1]);
                }
                else if (dtdata.Columns.Contains("IDProof_Path"))
                {
                    string strproof = dtdata.Rows[0]["IDProof_Path"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    vd.IdProofUrl = Server.MapPath(strproofsplit[strproofsplit.Length - 1]);
                }
                else
                {
                    vd.IdProofUrl = "N/A";
                }
                if (dtdata.Columns.Contains("Purpose_OCM"))
                {
                    vd.Purpose_OCM = dtdata.Rows[0]["Purpose_OCM"].ToString();
                }
                else
                {
                    vd.Purpose_OCM = "N/A";
                }
                if (dtdata.Columns.Contains("NoOfDays"))
                {
                    vd.NumberOfDay = dtdata.Rows[0]["NoOfDays"].ToString();
                    if (vd.NumberOfDay == null || vd.NumberOfDay == "")
                    {
                        vd.NumberOfDay = "N/A";
                    }
                }
                else
                {
                    vd.NumberOfDay = "N/A";
                }
                if (dtdata.Columns.Contains("DIST_CODE"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultDiv = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultDiv.ToString().Contains(dsMultDist.Tables[0].Rows[i][6].ToString()))
                                    {
                                        sbMultDiv.Append(dsMultDist.Tables[0].Rows[i][2].ToString() + " ,");
                                    }
                                }
                                if (sbMultDiv.Length > 0)
                                {
                                    vd.Division_Name = sbMultDiv.ToString().Remove(sbMultDiv.ToString().Length - 1, 1);
                                }
                                else { vd.Division_Name = "N/A"; }
                            }
                            else { vd.Division_Name = "N/A"; }
                        }
                        else { vd.Division_Name = "N/A"; }
                    }
                    // Modified by-Rajkumar 
                    // Modified Date-28-03-2016
                    else if (vd.TableName == "tbl_FDM_Project")
                    {
                        DataTable dsTable = new DataTable();
                        StringBuilder sb = new StringBuilder();
                        dsTable = ar.MultiDistrict(dtdata.Rows[0]["DIST_CODE"].ToString());
                        if (dsTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < dsTable.Rows.Count; i++)
                            {
                                sb.Append(dsTable.Rows[i][0].ToString() + " ,");
                            }
                            vd.District = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                        }
                        else { vd.District = "N/A"; }
                    }
                    else
                    {
                        if (dtdata.Columns.Contains("DIV_NAME"))
                        {
                            vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                            if (vd.Division_Name == null || vd.Division_Name == "")
                            {
                                vd.Division_Name = "N/A";
                            }
                        }
                        else
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                }
                else
                {
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                        if (vd.Division_Name == null || vd.Division_Name == "")
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Division_Name = "N/A";
                    }
                }
                if (dtdata.Columns.Contains("DIST_NAME"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultDist = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultDist.ToString().Contains(dsMultDist.Tables[0].Rows[i][2].ToString()))
                                    {
                                        sbMultDist.Append(dsMultDist.Tables[0].Rows[i][2].ToString() + " ,");
                                    }
                                }
                                if (sbMultDist.Length > 0)
                                {
                                    vd.District = sbMultDist.ToString().Remove(sbMultDist.ToString().Length - 1, 1);
                                }
                                else { vd.District = "N/A"; }
                            }
                            else { vd.District = "N/A"; }
                        }
                        else { vd.District = "N/A"; }
                    }
                    else
                    {
                        vd.District = dtdata.Rows[0]["DIST_NAME"].ToString();
                        if (vd.District == null || vd.District == "")
                        {
                            vd.District = "N/A";
                        }
                    }
                }
                else
                {
                    if (vd.District == null || vd.District == "")
                    {
                        vd.District = "N/A";
                    }
                }
                if (dtdata.Columns.Contains("PlaceName"))
                {
                    vd.Place = dtdata.Rows[0]["PlaceName"].ToString();
                    if (vd.Place == null || vd.Place == "")
                    {
                        vd.Place = "N/A";
                    }
                }
                else if (dtdata.Columns.Contains("Area"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultArea = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultArea.ToString().Contains(dsMultDist.Tables[0].Rows[i][0].ToString()))
                                    {
                                        sbMultArea.Append(dsMultDist.Tables[0].Rows[i][0].ToString() + " ,");
                                    }
                                    if (sbMultArea.Length > 0)
                                    {
                                        vd.Place = sbMultArea.ToString().Remove(sbMultArea.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        vd.Place = "N/A";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        vd.Place = dtdata.Rows[0]["Area"].ToString();
                        if (vd.Place == null || vd.Place == "")
                        {
                            vd.Place = "N/A";
                        }
                    }
                }
                else
                {
                    vd.Place = "N/A";
                }
                if (dtdata.Columns.Contains("BLK_NAME"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultBlk = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultBlk.ToString().Contains(dsMultDist.Tables[0].Rows[i][3].ToString()))
                                    {
                                        sbMultBlk.Append(dsMultDist.Tables[0].Rows[i][3].ToString() + " ,");
                                    }
                                }
                                if (sbMultBlk.Length > 0)
                                {
                                    vd.Block = sbMultBlk.ToString().Remove(sbMultBlk.ToString().Length - 1, 1);
                                }
                                else { vd.Block = "N/A"; }
                            }
                            else { vd.Block = "N/A"; }
                        }
                        else { vd.Block = "N/A"; }
                    }
                    else
                    {
                        vd.Block = dtdata.Rows[0]["BLK_NAME"].ToString();
                        if (vd.Block == null || vd.Block == "")
                        {
                            vd.Block = "N/A";
                        }
                    }
                }
                else
                {
                    vd.Block = "N/A";
                }
                if (dtdata.Columns.Contains("GP_NAME"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultGp = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultGp.ToString().Contains(dsMultDist.Tables[0].Rows[i][4].ToString()))
                                    {
                                        sbMultGp.Append(dsMultDist.Tables[0].Rows[i][4].ToString() + " ,");
                                    }
                                }
                                if (sbMultGp.Length > 0)
                                {
                                    vd.GramPanchayat = sbMultGp.ToString().Remove(sbMultGp.ToString().Length - 1, 1);
                                }
                                else { vd.GramPanchayat = "N/A"; }
                            }
                            else { vd.GramPanchayat = "N/A"; }
                        }
                        else { vd.GramPanchayat = "N/A"; }
                    }
                    else
                    {
                        vd.GramPanchayat = dtdata.Rows[0]["GP_NAME"].ToString();
                        if (vd.GramPanchayat == null || vd.GramPanchayat == "")
                        {
                            vd.GramPanchayat = "N/A";
                        }
                    }
                }
                else { vd.GramPanchayat = "N/A"; }
                if (dtdata.Columns.Contains("VILL_CODE"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultVill = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultVill.ToString().Contains(dsMultDist.Tables[0].Rows[i][5].ToString()))
                                    {
                                        sbMultVill.Append(dsMultDist.Tables[0].Rows[i][5].ToString() + " ,");
                                    }
                                }
                                if (sbMultVill.Length > 0)
                                {
                                    vd.Village = sbMultVill.ToString().Remove(sbMultVill.ToString().Length - 1, 1);
                                }
                                else { vd.Village = "N/A"; }
                            }
                            else { vd.Village = "N/A"; }
                        }
                        else { vd.Village = "N/A"; }
                    }
                    else
                    {
                        vd.Village = dtdata.Rows[0]["VILL_CODE"].ToString();
                        if (vd.Village == null || vd.Village == "")
                        {
                            vd.Village = "N/A";
                        }
                    }
                }
                if (dtdata.Columns.Contains("VILL_NAME"))
                {
                    vd.Village = dtdata.Rows[0]["VILL_NAME"].ToString();
                }
                else { vd.Village = "N/A"; }
                if (dtdata.Columns.Contains("KhasraNo"))
                {
                    if (vd.TableName == "tbl_FixedPermissions")
                    {
                        StringBuilder sbMultKhasra = new StringBuilder();
                        if (dsMultDist.Tables.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMultDist.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultKhasra.ToString().Contains(dsMultDist.Tables[0].Rows[i][1].ToString()))
                                    {
                                        sbMultKhasra.Append(dsMultDist.Tables[0].Rows[i][1].ToString() + " ,");
                                    }
                                }
                                if (sbMultKhasra.Length > 0)
                                {
                                    vd.khasraNo = sbMultKhasra.ToString().Remove(sbMultKhasra.ToString().Length - 1, 1);
                                }
                                else { vd.khasraNo = "N/A"; }
                            }
                            else { vd.khasraNo = "N/A"; }
                        }
                        else { vd.khasraNo = "N/A"; }
                    }
                    else
                    {
                        vd.khasraNo = dtdata.Rows[0]["KhasraNo"].ToString();
                        if (vd.khasraNo == null || vd.khasraNo == "")
                        {
                            vd.khasraNo = "N/A";
                        }
                    }
                }
                //Modified by Rajkumar 
                //Modified Date 28-03-2016
                if (dtdata.Columns.Contains("EstimatedAmount"))
                {
                    vd.EstimatedAmount = dtdata.Rows[0]["EstimatedAmount"].ToString();
                }
                else if (dtdata.Columns.Contains("Amount"))
                {
                    vd.EstimatedAmount = dtdata.Rows[0]["Amount"].ToString();
                }
                else
                {
                    vd.EstimatedAmount = "N/A";
                }
                if (dtdata.Columns.Contains("ReviewedAmount"))
                {
                    vd.ReviewedAmount = dtdata.Rows[0]["EstimatedAmount"].ToString();
                }
                else
                {
                    vd.ReviewedAmount = "N/A";
                }
                if (dtdata.Columns.Contains("ApprovedAmount"))
                {
                    vd.ApprovedAmount = dtdata.Rows[0]["ApprovedAmount"].ToString();
                }
                else
                {
                    vd.ApprovedAmount = "N/A";
                }
                if (dtdata.Columns.Contains("FinanceYear"))
                {
                    vd.FinanceYear = dtdata.Rows[0]["FinanceYear"].ToString();
                }
                else
                {
                    vd.FinanceYear = "N/A";
                }
                if (dtdata.Columns.Contains("WorkOrder_Name"))
                {
                    vd.WorkOrderName = dtdata.Rows[0]["WorkOrder_Name"].ToString();
                }
                else
                {
                    vd.WorkOrderName = "N/A";
                }
                if (dtdata.Columns.Contains("WorkOrderType"))
                {
                    vd.WorkOrderType = dtdata.Rows[0]["WorkOrderType"].ToString();
                }
                else
                {
                    vd.WorkOrderType = "N/A";
                }
                if (dtdata.Columns.Contains("BudgetHead"))
                {
                    vd.BudgetHead = dtdata.Rows[0]["BudgetHead"].ToString();
                }
                else
                {
                    vd.BudgetHead = "N/A";
                }
                if (dtdata.Columns.Contains("FinancialTarget"))
                {
                    vd.FinancialTarget = dtdata.Rows[0]["FinancialTarget"].ToString();
                }
                else
                {
                    vd.FinancialTarget = "N/A";
                }
                if (dtdata.Columns.Contains("AdminApprovedDate"))
                {
                    vd.AdminApprovaldate = dtdata.Rows[0]["AdminApprovedDate"].ToString();
                }
                else
                {
                    vd.AdminApprovaldate = "N/A";
                }
                if (dtdata.Columns.Contains("FinanceApprovedDate"))
                {
                    vd.FinancialApprovaldate = dtdata.Rows[0]["FinanceApprovedDate"].ToString();
                }
                else
                {
                    vd.FinancialApprovaldate = "N/A";
                }
                if (dtdata.Columns.Contains("Project_Name"))
                {
                    vd.ProjectName = dtdata.Rows[0]["Project_Name"].ToString();
                }
                else
                {
                    vd.ProjectName = "N/A";
                }
                if (dtdata.Columns.Contains("Program_Name"))
                {
                    vd.ProgramName = dtdata.Rows[0]["Program_Name"].ToString();
                }
                else
                {
                    vd.ProgramName = "N/A";
                }
                if (dtdata.Columns.Contains("Scheme_Name"))
                {
                    vd.SchemeName = dtdata.Rows[0]["Scheme_Name"].ToString();
                }
                else
                {
                    vd.SchemeName = "N/A";
                }
                if (dtdata.Columns.Contains("RefDocument"))
                {
                    string strproof = dtdata.Rows[0]["RefDocument"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    if (strproofsplit.Length > 0) { vd.ProjRefDoc = strproofsplit[strproofsplit.Length - 1]; } else { vd.ProjRefDoc = "N/A"; }
                }
                else
                {
                    vd.ProjRefDoc = "N/A";
                }
                if (dtdata.Columns.Contains("DPRDocument"))
                {
                    string strproof = dtdata.Rows[0]["DPRDocument"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    if (strproofsplit.Length > 0) { vd.ProjDprDoc = strproofsplit[strproofsplit.Length - 1]; } else { vd.ProjDprDoc = "N/A"; }
                }
                else
                {
                    vd.ProjDprDoc = "N/A";
                }
                if (dtdata.Columns.Contains("EstimatedBudget"))
                {
                    vd.ProjEstimatedBudget = dtdata.Rows[0]["EstimatedBudget"].ToString();
                }
                else
                {
                    vd.ProjEstimatedBudget = "N/A";
                }
                //End of Modification date 28-03-2016
                #region Research Study Permission
                if (vd.TableName == "tbl_ResearchStudyPermissions")
                {
                    if (dtdata.Columns.Contains("Qualification"))
                    {
                        vd.Qualification = dtdata.Rows[0]["Qualification"].ToString();
                        if (vd.Qualification == null || vd.Qualification == "")
                        {
                            vd.Qualification = "N/A";
                        }
                    }
                    else
                    {
                        vd.Qualification = "N/A";
                    }
                    if (dtdata.Columns.Contains("College"))
                    {
                        vd.College = dtdata.Rows[0]["College"].ToString();
                        if (vd.College == null || vd.College == "")
                        {
                            vd.College = "N/A";
                        }
                    }
                    else
                    {
                        vd.College = "N/A";
                    }
                    if (dtdata.Columns.Contains("R_Subject"))
                    {
                        vd.ResearchSubject = dtdata.Rows[0]["R_Subject"].ToString();
                        if (vd.ResearchSubject == null || vd.ResearchSubject == "")
                        {
                            vd.ResearchSubject = "N/A";
                        }
                    }
                    else
                    {
                        vd.ResearchSubject = "N/A";
                    }
                    if (dtdata.Columns.Contains("R_Procedure"))
                    {
                        vd.ResearchProcedure = dtdata.Rows[0]["R_Procedure"].ToString();
                        if (vd.ResearchProcedure == null || vd.ResearchProcedure == "")
                        {
                            vd.ResearchProcedure = "N/A";
                        }
                    }
                    else
                    {
                        vd.ResearchProcedure = "N/A";
                    }
                    if (dtdata.Columns.Contains("AnimalCategory"))
                    {
                        vd.AnimalCategory = dtdata.Rows[0]["AnimalCategory"].ToString();
                    }
                    else
                    {
                        vd.AnimalCategory = "N/A";
                    }
                    if (dtdata.Columns.Contains("AnimalName"))
                    {
                        vd.AnimalName = dtdata.Rows[0]["AnimalName"].ToString();
                    }
                    else
                    {
                        vd.AnimalName = "N/A";
                    }
                    if (dtdata.Columns.Contains("SpeciesCategory"))
                    {
                        vd.SpeciesCategory = dtdata.Rows[0]["SpeciesCategory"].ToString();
                        if (vd.SpeciesCategory == null || vd.SpeciesCategory == "")
                        {
                            vd.SpeciesCategory = "N/A";
                        }
                    }
                    else
                    {
                        vd.SpeciesCategory = "N/A";
                    }
                    if (dtdata.Columns.Contains("SpeciesName"))
                    {
                        vd.SpeciesName = dtdata.Rows[0]["SpeciesName"].ToString();
                        if (vd.SpeciesName == null || vd.SpeciesName == "")
                        {
                            vd.SpeciesName = "N/A";
                        }
                    }
                    else
                    {
                        vd.SpeciesName = "N/A";
                    }
                    if (dtdata.Columns.Contains("Animal_Sno"))
                    {
                        vd.Animal_SerialNo = dtdata.Rows[0]["Animal_Sno"].ToString();
                        if (vd.Animal_SerialNo == null || vd.Animal_SerialNo == "")
                        {
                            vd.Animal_SerialNo = "N/A";
                        }
                    }
                    else
                    {
                        vd.Animal_SerialNo = "N/A";
                    }
                    if (dtdata.Columns.Contains("Species_Sno"))
                    {
                        vd.Species_SerialNo = dtdata.Rows[0]["Species_Sno"].ToString();
                        if (vd.Species_SerialNo == null || vd.Species_SerialNo == "")
                        {
                            vd.Species_SerialNo = "N/A";
                        }
                    }
                    else
                    {
                        vd.Species_SerialNo = "N/A";
                    }
                    if (dtdata.Columns.Contains("Benefits"))
                    {
                        vd.Benefits = dtdata.Rows[0]["Benefits"].ToString();
                        if (vd.Benefits == null || vd.Benefits == "")
                        {
                            vd.Benefits = "N/A";
                        }
                    }
                    else
                    {
                        vd.Benefits = "N/A";
                    }
                    if (dtdata.Columns.Contains("CoordinatorId"))
                    {
                        vd.CoordinatorId = dtdata.Rows[0]["CoordinatorId"].ToString();
                        if (vd.CoordinatorId == null || vd.CoordinatorId == "")
                        {
                            vd.CoordinatorId = "N/A";
                        }
                    }
                    else
                        vd.CoordinatorId = "N/A";
                    // added more fields in Research Study for the changes came on 06-Aug-2016
                    vd.SynopsisPath = Convert.ToString(dtdata.Rows[0]["Synopsis_Name"]);
                    if (vd.SynopsisPath == "")
                        vd.SynopsisPath = "N/A";
                    vd.PresentationPath = Convert.ToString(dtdata.Rows[0]["Presentation_Name"]);
                    if (vd.PresentationPath == "")
                        vd.PresentationPath = "N/A";
                    vd.AssistName = Convert.ToString(dtdata.Rows[0]["Assist_Name"]);
                    if (vd.AssistName == "")
                        vd.AssistName = "N/A";
                    vd.AssistIdProofPath = Convert.ToString(dtdata.Rows[0]["Assist_IdProof_Name"]);
                    if (vd.AssistIdProofPath == "")
                        vd.AssistIdProofPath = "N/A";
                    vd.Vehiclecat = Convert.ToString(dtdata.Rows[0]["VehicleCat"]);
                    if (vd.Vehiclecat == "")
                        vd.Vehiclecat = "N/A";
                    vd.VehicleName = Convert.ToString(dtdata.Rows[0]["Vehiclename"]);
                    if (vd.VehicleName == "")
                        vd.VehicleName = "N/A";
                }
                #endregion
                if (dtdata.Columns.Contains("PermissionName"))
                {
                    vd.FixedPermissionName = dtdata.Rows[0]["PermissionName"].ToString();
                    if (vd.FixedPermissionName == null || vd.FixedPermissionName == "")
                    {
                        vd.FixedPermissionName = "N/A";
                    }
                }
                else
                {
                    vd.FixedPermissionName = "N/A";
                }
                if (dtdata.Columns.Contains("GPSLat"))
                {
                    vd.Latitude = dtdata.Rows[0]["GPSLat"].ToString();
                    if (vd.Latitude == null || vd.Latitude == "")
                    {
                        vd.Latitude = "N/A";
                    }
                }
                else { vd.Latitude = "N/A"; }
                if (dtdata.Columns.Contains("GPSLong"))
                {
                    vd.Longitude = dtdata.Rows[0]["GPSLong"].ToString();
                    if (vd.Longitude == null || vd.Longitude == "")
                    {
                        vd.Longitude = "N/A";
                    }
                }
                else { vd.Longitude = "N/A"; }
                if (dtdata.Columns.Contains("StatusDesc"))
                {
                    vd.StatusDesc = dtdata.Rows[0]["StatusDesc"].ToString();
                    if (vd.StatusDesc == null || vd.StatusDesc == "")
                    {
                        vd.StatusDesc = "N/A";
                    }
                }
                else { vd.StatusDesc = "N/A"; }
                if (dtdata.Columns.Contains("Remarks"))
                {
                    if (vd.StatusDesc == "Pending")
                    {
                        vd.Remarks = "";
                    }
                    else
                    {
                        vd.Remarks = dtdata.Rows[0]["Remarks"].ToString();
                    }
                    if (vd.Remarks == null || vd.Remarks == "")
                    {
                        vd.Remarks = "N/A";
                    }
                }
                else { vd.Remarks = "N/A"; }
                if (dtdata.Columns.Contains("ActionReason"))
                {
                    DataTable dsTable = new DataTable();
                    StringBuilder sb = new StringBuilder();
                    dsTable = ar.ReasonList(dtdata.Rows[0]["ActionReason"].ToString());
                    if (dsTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < dsTable.Rows.Count; i++)
                        {
                            sb.Append(dsTable.Rows[i][0].ToString() + " ,");
                        }
                        vd.Reason_Desc = sb.ToString().Remove(sb.ToString().Length - 1, 1);
                    }
                    else { vd.Reason_Desc = "N/A"; }
                }
                else { vd.Reason_Desc = "N/A"; }
                if (dtdata.Columns.Contains("KML_Path"))
                {
                    vd.KmlPath = dtdata.Rows[0]["KML_Path"].ToString();
                }
                if (dtdata.Columns.Contains("GISID"))
                {
                    vd.KmlView = dtdata.Rows[0]["GISID"].ToString();
                    vd.NOCType = ar.GISPerName(Session["SubPermissionId"].ToString());
                    //string strkml = dtdata.Rows[0]["KMLPathView"].ToString();
                    //string[] strkmlsplit = strkml.Split('/');
                    //if (strkmlsplit.Length > 0)
                    //{
                    //    string KmlFile = strkmlsplit[strkmlsplit.Length - 1];
                    //    vd.KmlView = "https://gis.rajasthan.gov.in/fmdssgis/gisview/viewongis.aspx?" + "portalid=rajcomp123&ssoid=" + Convert.ToString(Session["SSOid"]) + "&requestFor=" + ar.GISPerName(Session["SubPermissionId"].ToString()) + "&fileName=" + KmlFile;
                    //}
                    //else
                    //{
                    //    vd.KmlView = "N/A";
                    //}
                }
                //if (dtdata.Columns.Contains("KML_Path"))
                //{
                //    vd.KmlPath = vd.KmlPath = ConfigurationManager.AppSettings["KMLPath"] + Convert.ToString(dtdata.Rows[0]["KML_Path"]) + ".kml";  
                //    string strkml = dtdata.Rows[0]["KML_Path"].ToString();
                //    string[] strkmlsplit = strkml.Split('/');
                //    if (strkmlsplit.Length > 0)
                //    {
                //        string KmlFile = strkmlsplit[strkmlsplit.Length - 1];
                //        vd.KmlView = "https://gis.rajasthan.gov.in/fmdssgis/gisview/viewongis.aspx?" + "portalid=rajcomp123&ssoid=" + Convert.ToString(Session["SSOid"]) + "&requestFor=" + ar.GISPerName(Session["SubPermissionId"].ToString()) + "&fileName=" + KmlFile;
                //    }
                //    else
                //    {
                //        vd.KmlView = "N/A";
                //    }
                //}
                //else { vd.KmlPath = "N/A"; }
                if (dtdata.Columns.Contains("Revenue_Record_Path"))
                {
                    string str = dtdata.Rows[0]["Revenue_Record_Path"].ToString();
                    string[] strsplit = str.Split('/');
                    vd.Revenue_Record_Path = strsplit[strsplit.Length - 1];
                }
                else { vd.Revenue_Record_Path = "N/A"; }
                if (dtdata.Columns.Contains("Revenue_Map_Path"))
                {
                    string strmap = dtdata.Rows[0]["Revenue_Map_Path"].ToString();
                    string[] strmapsplit = strmap.Split('/');
                    vd.Revenue_Map_Path = strmapsplit[strmapsplit.Length - 1];
                }
                else { vd.Revenue_Map_Path = "N/A"; }
                if (dtdata.Columns.Contains("Citizen_Comment"))
                {
                    vd.Citizen_Comment = dtdata.Rows[0]["Citizen_Comment"].ToString();
                }
                else { vd.Citizen_Comment = "N/A"; }
                if (dtdata.Columns.Contains("Additional_Document"))
                {
                    string strdoc = dtdata.Rows[0]["Additional_Document"].ToString();
                    string[] strdocsplit = strdoc.Split('/');
                    vd.Additional_Document = strdocsplit[strdocsplit.Length - 1];
                }
                else { vd.Additional_Document = "N/A"; }
                if (dtdata.Columns.Contains("Survey_Document"))
                {
                    string strdoc = dtdata.Rows[0]["Survey_Document"].ToString();
                    string[] stsurrdocsplit = strdoc.Split('/');
                    vd.Survey_Document = stsurrdocsplit[stsurrdocsplit.Length - 1];
                }
                else { vd.Survey_Document = "N/A"; }
                if (dtdata.Columns.Contains("PlantCount"))
                {
                    vd.PlantCount = dtdata.Rows[0]["PlantCount"].ToString();
                }
                else { vd.PlantCount = "0"; }
                if (dtdata.Columns.Contains("IsGTSheetAvaliable"))
                {
                    //if (vd.PermissionName == "Mining Permission") {
                    if (dtdata.Rows[0]["IsGTSheetAvaliable"].ToString() == "True")
                    {
                        vd.IsGTSheetAvaliable = "True";
                        if (dtdata.Columns.Contains("Nearest_WaterSource"))
                        {
                            vd.Nearest_WaterSource = dtdata.Rows[0]["Nearest_WaterSource"].ToString();
                        }
                        else
                        {
                            vd.Nearest_WaterSource = "N/A";
                        }
                        if (dtdata.Columns.Contains("WaterSource_Distance"))
                        {
                            vd.WaterSource_Distance = dtdata.Rows[0]["WaterSource_Distance"].ToString();
                        }
                        else
                        {
                            vd.WaterSource_Distance = "N/A";
                        }
                        if (dtdata.Columns.Contains("Forest_Distance"))
                        {
                            vd.Forest_Distance = dtdata.Rows[0]["Forest_Distance"].ToString();
                        }
                        else
                        {
                            vd.Forest_Distance = "N/A";
                        }
                        if (dtdata.Columns.Contains("Wildlife_Distance"))
                        {
                            vd.Wildlife_Distance = dtdata.Rows[0]["Wildlife_Distance"].ToString();
                        }
                        else
                        {
                            vd.Wildlife_Distance = "N/A";
                        }
                        if (dtdata.Columns.Contains("Tree_species"))
                        {
                            vd.Tree_species = dtdata.Rows[0]["Tree_species"].ToString();
                        }
                        else
                        {
                            vd.Tree_species = "N/A";
                        }
                        if (dtdata.Columns.Contains("AravalliHills"))
                        {
                            if (dtdata.Rows[0]["AravalliHills"].ToString() == "1")
                            {
                                vd.AravalliHills = "True";
                            }
                            else
                            {
                                vd.AravalliHills = "False";
                            }
                        }
                        else
                        {
                            vd.AravalliHills = "N/A";
                        }
                        if (dtdata.Columns.Contains("ForestLand"))
                        {
                            if (dtdata.Rows[0]["ForestLand"].ToString() == "1")
                            {
                                vd.ForestLand = "True";
                            }
                            else
                            {
                                vd.ForestLand = "False";
                            }
                        }
                        else
                        {
                            vd.ForestLand = "N/A";
                        }
                        if (dtdata.Columns.Contains("Plantation_Area"))
                        {
                            if (dtdata.Rows[0]["Plantation_Area"].ToString() == "1")
                            {
                                vd.Plantation_Area = "True";
                            }
                            else
                            {
                                vd.Plantation_Area = "False";
                            }
                        }
                        else
                        {
                            vd.Plantation_Area = "N/A";
                        }
                    }
                    else
                    {
                        vd.IsGTSheetAvaliable = "False";
                        vd.Nearest_WaterSource = "N/A";
                        vd.WaterSource_Distance = "N/A";
                        vd.Forest_Distance = "N/A";
                        vd.Wildlife_Distance = "N/A";
                        vd.Tree_species = "N/A";
                        vd.AravalliHills = "N/A";
                        vd.ForestLand = "N/A";
                        vd.Plantation_Area = "N/A";
                    }
                    //}
                    //else
                    //{
                    //    vd.IsGTSheetAvaliable = "False";
                    //    vd.Nearest_WaterSource = "N/A";
                    //    vd.WaterSource_Distance = "N/A";
                    //    vd.Forest_Distance = "N/A";
                    //    vd.Wildlife_Distance = "N/A";
                    //    vd.Tree_species = "N/A";
                    //    vd.AravalliHills = "N/A";
                    //    vd.ForestLand = "N/A";
                    //    vd.Plantation_Area = "N/A";
                    //}
                }
                else
                {
                    vd.IsGTSheetAvaliable = "N/A";
                    vd.Nearest_WaterSource = "N/A";
                    vd.WaterSource_Distance = "N/A";
                    vd.Forest_Distance = "N/A";
                    vd.Wildlife_Distance = "N/A";
                    vd.Tree_species = "N/A";
                    vd.AravalliHills = "N/A";
                    vd.ForestLand = "N/A";
                    vd.Plantation_Area = "N/A";
                }
                if (dtdata.Columns.Contains("Animal_Sno"))
                {
                    vd.Animal_SerialNo = dtdata.Rows[0]["Animal_Sno"].ToString();
                    if (vd.Animal_SerialNo == null || vd.Animal_SerialNo == "")
                    {
                        vd.Animal_SerialNo = "N/A";
                    }
                }
                else
                {
                    vd.Animal_SerialNo = "N/A";
                }
                if (dtdata.Columns.Contains("Species_Sno"))
                {
                    vd.Species_SerialNo = dtdata.Rows[0]["Species_Sno"].ToString();
                    if (vd.Species_SerialNo == null || vd.Species_SerialNo == "")
                    {
                        vd.Species_SerialNo = "N/A";
                    }
                }
                else
                {
                    vd.Species_SerialNo = "N/A";
                }
                if (dtdata.Columns.Contains("Benefits"))
                {
                    vd.Benefits = dtdata.Rows[0]["Benefits"].ToString();
                    if (vd.Benefits == null || vd.Benefits == "")
                    {
                        vd.Benefits = "N/A";
                    }
                }
                else
                {
                    vd.Benefits = "N/A";
                }
                if (dtdata.Columns.Contains("CoordinatorId"))
                {
                    vd.CoordinatorId = dtdata.Rows[0]["CoordinatorId"].ToString();
                    if (vd.CoordinatorId == null || vd.CoordinatorId == "")
                    {
                        vd.CoordinatorId = "N/A";
                    }
                }
                else
                {
                    vd.CoordinatorId = "N/A";
                }
                if (dtdata.Columns.Contains("Description"))
                {
                    vd.Description = dtdata.Rows[0]["Description"].ToString();
                    if (vd.Description == null || vd.Description == "")
                    {
                        vd.Description = "N/A";
                    }
                }
                else
                {
                    vd.Description = "N/A";
                }
                if (dtdata.Columns.Contains("IndianCitizen"))
                {
                    vd.IndianCitizen = dtdata.Rows[0]["IndianCitizen"].ToString();
                    if (vd.IndianCitizen == null || vd.IndianCitizen == "")
                    {
                        vd.IndianCitizen = "N/A";
                    }
                }
                else
                {
                    vd.IndianCitizen = "N/A";
                }
                if (dtdata.Columns.Contains("NonIndianCitizen"))
                {
                    vd.NonIndianCitizen = dtdata.Rows[0]["NonIndianCitizen"].ToString();
                    if (vd.NonIndianCitizen == null || vd.NonIndianCitizen == "")
                    {
                        vd.NonIndianCitizen = "N/A";
                    }
                }
                else
                {
                    vd.NonIndianCitizen = "N/A";
                }
                if (dtdata.Columns.Contains("Students"))
                {
                    vd.Students = dtdata.Rows[0]["Students"].ToString();
                    if (vd.Students == null || vd.Students == "")
                    {
                        vd.Students = "N/A";
                    }
                }
                else
                {
                    vd.Students = "N/A";
                }
                if (dtdata.Columns.Contains("OffenderName"))
                {
                    vd.OffenderName = dtdata.Rows[0]["OffenderName"].ToString();
                    if (vd.OffenderName == null || vd.OffenderName == "")
                    {
                        vd.OffenderName = "N/A";
                    }
                }
                else
                {
                    vd.OffenderName = "N/A";
                }
                if (dtdata.Columns.Contains("FatherName"))
                {
                    vd.FatherName = dtdata.Rows[0]["FatherName"].ToString();
                    if (vd.FatherName == null || vd.FatherName == "")
                    {
                        vd.FatherName = "N/A";
                    }
                }
                else
                {
                    vd.FatherName = "N/A";
                }
                if (dtdata.Columns.Contains("Caste"))
                {
                    vd.Caste = dtdata.Rows[0]["Caste"].ToString();
                    if (vd.Caste == null || vd.Caste == "")
                    {
                        vd.Caste = "N/A";
                    }
                }
                else
                {
                    vd.Caste = "N/A";
                }
                if (dtdata.Columns.Contains("Address1"))
                {
                    vd.Address = dtdata.Rows[0]["Address1"].ToString();
                    if (vd.Address == null || vd.Address == "")
                    {
                        vd.Address = "N/A";
                    }
                }
                else
                {
                    vd.Address = "N/A";
                }
                if (dtdata.Columns.Contains("PhoneNo"))
                {
                    vd.PhoneNo = dtdata.Rows[0]["PhoneNo"].ToString();
                    if (vd.PhoneNo == null || vd.PhoneNo == "")
                    {
                        vd.PhoneNo = "N/A";
                    }
                }
                else
                {
                    vd.PhoneNo = "N/A";
                }
                if (dtdata.Columns.Contains("OffenderAge"))
                {
                    vd.OffenderAge = dtdata.Rows[0]["OffenderAge"].ToString();
                    if (vd.OffenderAge == null || vd.OffenderAge == "")
                    {
                        vd.OffenderAge = "N/A";
                    }
                }
                else
                {
                    vd.OffenderAge = "N/A";
                }
                if (dtdata.Columns.Contains("PoliceStation"))
                {
                    vd.PoliceStation = dtdata.Rows[0]["PoliceStation"].ToString();
                    if (vd.PoliceStation == null || vd.PoliceStation == "")
                    {
                        vd.PoliceStation = "N/A";
                    }
                }
                else
                {
                    vd.PoliceStation = "N/A";
                }
                if (dtdata.Columns.Contains("ClothesWorn"))
                {
                    vd.ClothesWorn = dtdata.Rows[0]["ClothesWorn"].ToString();
                    if (vd.ClothesWorn == null || vd.ClothesWorn == "")
                    {
                        vd.ClothesWorn = "N/A";
                    }
                }
                else
                {
                    vd.ClothesWorn = "N/A";
                }
                if (dtdata.Columns.Contains("ClothesColor"))
                {
                    vd.ClothesColor = dtdata.Rows[0]["ClothesColor"].ToString();
                    if (vd.ClothesColor == null || vd.ClothesColor == "")
                    {
                        vd.ClothesColor = "N/A";
                    }
                }
                else
                {
                    vd.ClothesColor = "N/A";
                }
                if (dtdata.Columns.Contains("PhysicalAppearance"))
                {
                    vd.PhysicalAppearance = dtdata.Rows[0]["PhysicalAppearance"].ToString();
                    if (vd.PhysicalAppearance == null || vd.PhysicalAppearance == "")
                    {
                        vd.PhysicalAppearance = "N/A";
                    }
                }
                else
                {
                    vd.PhysicalAppearance = "N/A";
                }
                if (dtdata.Columns.Contains("Height"))
                {
                    vd.Height = dtdata.Rows[0]["Height"].ToString();
                    if (vd.Height == null || vd.Height == "")
                    {
                        vd.Height = "N/A";
                    }
                }
                else
                {
                    vd.Height = "N/A";
                }
                if (dtdata.Columns.Contains("EvidenceDocURL"))
                {
                    string strproof = dtdata.Rows[0]["EvidenceDocURL"].ToString();
                    string[] strproofsplit = strproof.Split('/');
                    if (strproofsplit.Length > 0) { vd.EvidenceDoc = strproofsplit[strproofsplit.Length - 1]; } else { vd.EvidenceDoc = "N/A"; }
                }
                else
                {
                    vd.EvidenceDoc = "N/A";
                }
                #region MicroPlan
                if (vd.TableName == "tbl_FDM_MicroPlan")
                {
                    if (dtdata.Columns.Contains("MicroPlanName"))
                    {
                        vd.MicroPlanName = dtdata.Rows[0]["MicroPlanName"].ToString();
                        if (vd.MicroPlanName == null || vd.MicroPlanName == "")
                        {
                            vd.MicroPlanName = "N/A";
                        }
                    }
                    else
                    {
                        vd.MicroPlanName = "N/A";
                    }
                    if (dtdata.Columns.Contains("MicroPlan_Code"))
                    {
                        vd.MicroPlanCode = dtdata.Rows[0]["MicroPlan_Code"].ToString();
                        if (vd.MicroPlanCode == null || vd.MicroPlanCode == "")
                        {
                            vd.MicroPlanCode = "N/A";
                        }
                    }
                    else
                    {
                        vd.MicroPlanCode = "N/A";
                    }
                    DataSet dsMicroProj = new DataSet();
                    dsMicroProj = ar.GetMultiMicroPlanProj(vd.RequestId);
                    if (dsMicroProj.Tables[0].Columns.Contains("Project_Name"))
                    {
                        StringBuilder sbMultProj = new StringBuilder();
                        if (dsMicroProj.Tables.Count > 0)
                        {
                            if (dsMicroProj.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMicroProj.Tables[0].Rows.Count; i++)
                                {
                                    if (!sbMultProj.ToString().Contains(dsMicroProj.Tables[0].Rows[i][0].ToString()))
                                    {
                                        sbMultProj.Append(dsMicroProj.Tables[0].Rows[i][0].ToString() + " ,");
                                    }
                                }
                                if (sbMultProj.Length > 0)
                                {
                                    vd.MicroProjectName = sbMultProj.ToString().Remove(sbMultProj.ToString().Length - 1, 1);
                                }
                                else { vd.MicroProjectName = "N/A"; }
                            }
                            else { vd.MicroProjectName = "N/A"; }
                        }
                        else { vd.MicroProjectName = "N/A"; }
                    }
                    else { vd.MicroProjectName = "N/A"; }
                    if (dtdata.Columns.Contains("DateOfRequest"))
                    {
                        vd.MicroDateOfRequest = dtdata.Rows[0]["DateOfRequest"].ToString();
                        if (vd.MicroDateOfRequest == null || vd.MicroDateOfRequest == "")
                        {
                            vd.MicroDateOfRequest = "N/A";
                        }
                    }
                    else
                    {
                        vd.MicroDateOfRequest = "N/A";
                    }
                    if (dtdata.Columns.Contains("NGOorSHG"))
                    {
                        vd.NGO_SHG = dtdata.Rows[0]["NGOorSHG"].ToString();
                        if (vd.NGO_SHG == null || vd.NGO_SHG == "")
                        {
                            vd.NGO_SHG = "N/A";
                        }
                    }
                    else
                    {
                        vd.NGO_SHG = "N/A";
                    }
                    if (dtdata.Columns.Contains("NGOSHGName"))
                    {
                        vd.NGO_SHGName = dtdata.Rows[0]["NGOSHGName"].ToString();
                        if (vd.NGO_SHGName == null || vd.NGO_SHGName == "")
                        {
                            vd.NGO_SHGName = "N/A";
                        }
                    }
                    else
                    {
                        vd.NGO_SHGName = "N/A";
                    }
                    if (dtdata.Columns.Contains("RevenueVillage"))
                    {
                        vd.RevenueVillage = dtdata.Rows[0]["RevenueVillage"].ToString();
                        if (vd.RevenueVillage == null || vd.RevenueVillage == "")
                        {
                            vd.RevenueVillage = "N/A";
                        }
                    }
                    else
                    {
                        vd.RevenueVillage = "N/A";
                    }
                    if (dtdata.Columns.Contains("PanchayatComittee"))
                    {
                        vd.PanchayatComittee = dtdata.Rows[0]["PanchayatComittee"].ToString();
                        if (vd.PanchayatComittee == null || vd.PanchayatComittee == "")
                        {
                            vd.PanchayatComittee = "N/A";
                        }
                    }
                    else
                    {
                        vd.PanchayatComittee = "N/A";
                    }
                    if (dtdata.Columns.Contains("ForestAdminUnit"))
                    {
                        vd.ForestAdminUnit = dtdata.Rows[0]["ForestAdminUnit"].ToString();
                        if (vd.ForestAdminUnit == null || vd.ForestAdminUnit == "")
                        {
                            vd.ForestAdminUnit = "N/A";
                        }
                    }
                    else
                    {
                        vd.ForestAdminUnit = "N/A";
                    }
                    if (dtdata.Columns.Contains("RangeOfficeUnit"))
                    {
                        vd.RangeOfficeUnit = dtdata.Rows[0]["RangeOfficeUnit"].ToString();
                        if (vd.RangeOfficeUnit == null || vd.RangeOfficeUnit == "")
                        {
                            vd.RangeOfficeUnit = "N/A";
                        }
                    }
                    else
                    {
                        vd.RangeOfficeUnit = "N/A";
                    }
                    if (dtdata.Columns.Contains("Totalarea"))
                    {
                        vd.TotalArea = dtdata.Rows[0]["Totalarea"].ToString();
                        if (vd.TotalArea == null || vd.TotalArea == "")
                        {
                            vd.TotalArea = "N/A";
                        }
                    }
                    else
                    {
                        vd.TotalArea = "N/A";
                    }
                    if (dtdata.Columns.Contains("TotalLandAreaSQKM"))
                    {
                        vd.TotalLandsqkm = dtdata.Rows[0]["TotalLandAreaSQKM"].ToString();
                        if (vd.TotalLandsqkm == null || vd.TotalLandsqkm == "")
                        {
                            vd.TotalLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.TotalLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("TotalForestAreaSQKM"))
                    {
                        vd.TotalForestsqkm = dtdata.Rows[0]["TotalForestAreaSQKM"].ToString();
                        if (vd.TotalForestsqkm == null || vd.TotalForestsqkm == "")
                        {
                            vd.TotalForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.TotalForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ReservedForestSQKM"))
                    {
                        vd.ReservedForestsqkm = dtdata.Rows[0]["ReservedForestSQKM"].ToString();
                        if (vd.ReservedForestsqkm == null || vd.ReservedForestsqkm == "")
                        {
                            vd.ReservedForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProtectedForestSQKM"))
                    {
                        vd.ProtectForestsqkm = dtdata.Rows[0]["ProtectedForestSQKM"].ToString();
                        if (vd.ProtectForestsqkm == null || vd.ProtectForestsqkm == "")
                        {
                            vd.ProtectForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ProtectForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("UnClassifiedForestSQKM"))
                    {
                        vd.UnclassifiedForestsqkm = dtdata.Rows[0]["UnClassifiedForestSQKM"].ToString();
                        if (vd.UnclassifiedForestsqkm == null || vd.UnclassifiedForestsqkm == "")
                        {
                            vd.UnclassifiedForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.UnclassifiedForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ClassifiedForestSQKM"))
                    {
                        vd.ClassifiedForestsqkm = dtdata.Rows[0]["ClassifiedForestSQKM"].ToString();
                        if (vd.ClassifiedForestsqkm == null || vd.ClassifiedForestsqkm == "")
                        {
                            vd.ClassifiedForestsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ClassifiedForestsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("FullyCoveredSQKM"))
                    {
                        vd.FullyCoversqkm = dtdata.Rows[0]["FullyCoveredSQKM"].ToString();
                        if (vd.FullyCoversqkm == null || vd.FullyCoversqkm == "")
                        {
                            vd.FullyCoversqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.FullyCoversqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("WithoutPlantSQKM"))
                    {
                        vd.WithoutPlantsqkm = dtdata.Rows[0]["WithoutPlantSQKM"].ToString();
                        if (vd.WithoutPlantsqkm == null || vd.WithoutPlantsqkm == "")
                        {
                            vd.WithoutPlantsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.WithoutPlantsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("AllocateforPlantSQKM"))
                    {
                        vd.AllocateforPlantsqkm = dtdata.Rows[0]["AllocateforPlantSQKM"].ToString();
                        if (vd.AllocateforPlantsqkm == null || vd.AllocateforPlantsqkm == "")
                        {
                            vd.AllocateforPlantsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.AllocateforPlantsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("PanchayatLandSQKM"))
                    {
                        vd.PanchayatLandsqkm = dtdata.Rows[0]["PanchayatLandSQKM"].ToString();
                        if (vd.PanchayatLandsqkm == null || vd.PanchayatLandsqkm == "")
                        {
                            vd.PanchayatLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.PanchayatLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("RevenueLandSQKM"))
                    {
                        vd.RevenueLandsqkm = dtdata.Rows[0]["PanchayatLandSQKM"].ToString();
                        if (vd.RevenueLandsqkm == null || vd.RevenueLandsqkm == "")
                        {
                            vd.RevenueLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.RevenueLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("AgricultureLand"))
                    {
                        vd.AgricultureLand = dtdata.Rows[0]["AgricultureLand"].ToString();
                        if (vd.AgricultureLand == null || vd.AgricultureLand == "")
                        {
                            vd.AgricultureLand = "N/A";
                        }
                    }
                    else
                    {
                        vd.AgricultureLand = "N/A";
                    }
                    if (dtdata.Columns.Contains("IrregatedLandSQKM"))
                    {
                        vd.IrregatedLandsqkm = dtdata.Rows[0]["IrregatedLandSQKM"].ToString();
                        if (vd.IrregatedLandsqkm == null || vd.IrregatedLandsqkm == "")
                        {
                            vd.IrregatedLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.IrregatedLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("NonIrregatedLandSQKM"))
                    {
                        vd.NonIrregatedLandsqkm = dtdata.Rows[0]["NonIrregatedLandSQKM"].ToString();
                        if (vd.NonIrregatedLandsqkm == null || vd.NonIrregatedLandsqkm == "")
                        {
                            vd.NonIrregatedLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.NonIrregatedLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("ResidentialAreaSQKM"))
                    {
                        vd.ResidentialAreasqkm = dtdata.Rows[0]["ResidentialAreaSQKM"].ToString();
                        if (vd.ResidentialAreasqkm == null || vd.ResidentialAreasqkm == "")
                        {
                            vd.ResidentialAreasqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.ResidentialAreasqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("RemAgricultureLandSQKM"))
                    {
                        vd.RemAgricultureLandsqkm = dtdata.Rows[0]["RemAgricultureLandSQKM"].ToString();
                        if (vd.RemAgricultureLandsqkm == null || vd.RemAgricultureLandsqkm == "")
                        {
                            vd.RemAgricultureLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.RemAgricultureLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("RemNonAgricultureLandSQKM"))
                    {
                        vd.RemNonAgricultureLandsqkm = dtdata.Rows[0]["RemAgricultureLandSQKM"].ToString();
                        if (vd.RemNonAgricultureLandsqkm == null || vd.RemNonAgricultureLandsqkm == "")
                        {
                            vd.RemNonAgricultureLandsqkm = "N/A";
                        }
                    }
                    else
                    {
                        vd.RemNonAgricultureLandsqkm = "N/A";
                    }
                    if (dtdata.Columns.Contains("JFMCName"))
                    {
                        vd.JFMCName = dtdata.Rows[0]["JFMCName"].ToString();
                        if (vd.JFMCName == null || vd.JFMCName == "")
                        {
                            vd.JFMCName = "N/A";
                        }
                    }
                    else
                    {
                        vd.JFMCName = "N/A";
                    }
                }
                #endregion
                #region WorkOrderInvoice
                if (dtdata.Columns.Contains("WorkOrder_Name"))
                {
                    vd.WorkOrderName = dtdata.Rows[0]["WorkOrder_Name"].ToString();
                    if (vd.WorkOrderName == null || vd.WorkOrderName == "")
                    {
                        vd.WorkOrderName = "N/A";
                    }
                }
                else
                {
                    vd.WorkOrderName = "N/A";
                }
                if (dtdata.Columns.Contains("WorkOrder_Code"))
                {
                    vd.workorderCode = dtdata.Rows[0]["WorkOrder_Code"].ToString();
                    if (vd.workorderCode == null || vd.workorderCode == "")
                    {
                        vd.workorderCode = "N/A";
                    }
                }
                else
                {
                    vd.workorderCode = "N/A";
                }
                if (dtdata.Columns.Contains("MileStoneName"))
                {
                    vd.MileStoneName = dtdata.Rows[0]["MileStoneName"].ToString();
                    if (vd.MileStoneName == null || vd.MileStoneName == "")
                    {
                        vd.MileStoneName = "N/A";
                    }
                }
                else
                {
                    vd.MileStoneName = "N/A";
                }
                if (dtdata.Columns.Contains("MileStonePaymentPercentage"))
                {
                    vd.MileStonePaymentPercentage = dtdata.Rows[0]["MileStonePaymentPercentage"].ToString();
                    if (vd.MileStonePaymentPercentage == null || vd.MileStonePaymentPercentage == "")
                    {
                        vd.MileStonePaymentPercentage = "N/A";
                    }
                }
                else
                {
                    vd.MileStonePaymentPercentage = "N/A";
                }
                if (dtdata.Columns.Contains("isBillRaised"))
                {
                    vd.BillRaised = dtdata.Rows[0]["isBillRaised"].ToString();
                }
                else
                {
                    vd.MileStonePaymentPercentage = "N/A";
                }
                if (dtdata.Columns.Contains("BillAmount"))
                {
                    vd.BillAmount = dtdata.Rows[0]["BillAmount"].ToString();
                    if (vd.BillAmount == null || vd.BillAmount == "")
                    {
                        vd.BillAmount = "N/A";
                    }
                }
                else
                {
                    vd.BillAmount = "N/A";
                }
                if (dtdata.Columns.Contains("BillDate"))
                {
                    DateTime _date;
                    _date = DateTime.Parse(dtdata.Rows[0]["BillDate"].ToString());
                    vd.BillDate = _date.ToString("dd-MM-yyyy");
                    if (vd.BillDate == null || vd.BillDate == "")
                    {
                        vd.BillDate = "N/A";
                    }
                }
                else
                {
                    vd.BillDate = "N/A";
                }
                if (dtdata.Columns.Contains("BillNo"))
                {
                    vd.BillNo = dtdata.Rows[0]["BillNo"].ToString();
                    if (vd.BillNo == null || vd.BillNo == "")
                    {
                        vd.BillNo = "N/A";
                    }
                }
                else
                {
                    vd.BillNo = "N/A";
                }
                #endregion
                #region "Add notice by Arvind"
                //Needs work dipak
                if (vd.TableName == "tbl_mst_Notice" || vd.TableName == "tbl_FDM_BudgetReviewApproval")
                {
                    if (dtdata.Columns.Contains("Id"))
                    {
                        vd.Notice_Id = dtdata.Rows[0]["Id"].ToString();
                        if (vd.Notice_Id == null || vd.Notice_Id == "")
                        {
                            vd.Notice_Id = "N/A";
                        }
                    }
                    else
                    {
                        vd.Notice_Id = "N/A";
                    }
                    if (dtdata.Columns.Contains("Notice_Number"))
                    {
                        vd.Notice_No = dtdata.Rows[0]["Notice_Number"].ToString();
                        if (vd.Notice_No == null || vd.Notice_No == "")
                        {
                            vd.Notice_No = "N/A";
                        }
                    }
                    else
                    {
                        vd.Notice_No = "N/A";
                    }
                    if (dtdata.Columns.Contains("REG_NAME"))
                    {
                        vd.Region_Name = dtdata.Rows[0]["REG_NAME"].ToString();
                        if (vd.Region_Name == null || vd.Region_Name == "")
                        {
                            vd.Region_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Region_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("CIRCLE_NAME"))
                    {
                        vd.Circle_Name = dtdata.Rows[0]["CIRCLE_NAME"].ToString();
                        if (vd.Circle_Name == null || vd.Circle_Name == "")
                        {
                            vd.Circle_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Circle_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                        if (vd.Division_Name == null || vd.Division_Name == "")
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Division_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("RANGE_NAME"))
                    {
                        vd.Range_Name = dtdata.Rows[0]["RANGE_NAME"].ToString();
                        if (vd.Range_Name == null || vd.Range_Name == "")
                        {
                            vd.Range_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Range_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("Depot_Name"))
                    {
                        vd.Depot_Name = dtdata.Rows[0]["Depot_Name"].ToString();
                        if (vd.Depot_Name == null || vd.Depot_Name == "")
                        {
                            vd.Depot_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Depot_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        vd.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                        if (vd.Produce_Name == null || vd.Produce_Name == "")
                        {
                            vd.Produce_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProductName"))
                    {
                        vd.Product_Name = dtdata.Rows[0]["ProductName"].ToString();
                        if (vd.Product_Name == null || vd.Product_Name == "")
                        {
                            vd.Product_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Product_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("UnitName"))
                    {
                        vd.Produce_Unit = dtdata.Rows[0]["UnitName"].ToString();
                        if (vd.Produce_Unit == null || vd.Produce_Unit == "")
                        {
                            vd.Produce_Unit = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Unit = "N/A";
                    }
                    if (dtdata.Columns.Contains("Quantity"))
                    {
                        vd.Qty = dtdata.Rows[0]["Quantity"].ToString();
                        if (vd.Qty == null || vd.Qty == "")
                        {
                            vd.Qty = "N/A";
                        }
                    }
                    else
                    {
                        vd.Qty = "N/A";
                    }
                    if (dtdata.Columns.Contains("ReservedPrice"))
                    {
                        vd.ReservedPrice = dtdata.Rows[0]["ReservedPrice"].ToString();
                        if (vd.ReservedPrice == null || vd.ReservedPrice == "")
                        {
                            vd.ReservedPrice = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedPrice = "N/A";
                    }

                    //Needs work from here dipak
                    if (Globals.Util.isValidDataSet(dsdata, 1, true))
                    {
                        vd.DODProductList = Globals.Util.GetListFromTable<Models.ForestDevelopment.DODProductDetails>(dsdata, 1);
                        return PartialView("_ViewForesterAction", vd);
                    }

                }
                #endregion
                #region "Add Apply for Auction by Arvind"
                if (vd.TableName == "tbl_AuctionDetail")
                {
                    if (dtdata.Columns.Contains("Applicant_Type"))
                    {
                        if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                        {
                            vd.Applicant_Type = "Individual";
                        }
                        else
                        {
                            vd.Applicant_Type = "Organizational";
                        }
                    }
                    else
                    {
                        vd.Applicant_Type = "N/A";
                    }
                    if (dtdata.Columns.Contains("BidderName"))
                    {
                        vd.BidderName = dtdata.Rows[0]["BidderName"].ToString();
                        if (vd.BidderName == null || vd.BidderName == "")
                        {
                            vd.BidderName = "N/A";
                        }
                    }
                    else
                    {
                        vd.BidderName = "N/A";
                    }
                    if (dtdata.Columns.Contains("Notice_Number"))
                    {
                        vd.Notice_No = dtdata.Rows[0]["Notice_Number"].ToString();
                        if (vd.Notice_No == null || vd.Notice_No == "")
                        {
                            vd.Notice_No = "N/A";
                        }
                    }
                    else
                    {
                        vd.Notice_No = "N/A";
                    }
                    if (dtdata.Columns.Contains("DurationFrom"))
                    {
                        DateTime _date1, _date2;
                        _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                        _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                        vd.DurationsFrom = _date1.ToString("dd/MM/yyyy");
                        vd.DurationsTo = _date2.ToString("dd/MM/yyyy");
                        //ar.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        vd.DurationsFrom = "N/A";
                        vd.DurationsTo = "N/A";
                    }
                    if (dtdata.Columns.Contains("REG_NAME"))
                    {
                        vd.Region_Name = dtdata.Rows[0]["REG_NAME"].ToString();
                        if (vd.Region_Name == null || vd.Region_Name == "")
                        {
                            vd.Region_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Region_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("CIRCLE_NAME"))
                    {
                        vd.Circle_Name = dtdata.Rows[0]["CIRCLE_NAME"].ToString();
                        if (vd.Circle_Name == null || vd.Circle_Name == "")
                        {
                            vd.Circle_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Circle_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("DIV_NAME"))
                    {
                        vd.Division_Name = dtdata.Rows[0]["DIV_NAME"].ToString();
                        if (vd.Division_Name == null || vd.Division_Name == "")
                        {
                            vd.Division_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Division_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("RANGE_NAME"))
                    {
                        vd.Range_Name = dtdata.Rows[0]["RANGE_NAME"].ToString();
                        if (vd.Range_Name == null || vd.Range_Name == "")
                        {
                            vd.Range_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Range_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("Depot_Name"))
                    {
                        vd.Depot_Name = dtdata.Rows[0]["Depot_Name"].ToString();
                        if (vd.Depot_Name == null || vd.Depot_Name == "")
                        {
                            vd.Depot_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Depot_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        vd.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                        if (vd.Produce_Name == null || vd.Produce_Name == "")
                        {
                            vd.Produce_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("ProduceType"))
                    {
                        vd.Produce_Name = dtdata.Rows[0]["ProduceType"].ToString();
                        if (vd.Produce_Name == null || vd.Produce_Name == "")
                        {
                            vd.Produce_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("UnitName"))
                    {
                        vd.Produce_Unit = dtdata.Rows[0]["UnitName"].ToString();
                        if (vd.Produce_Unit == null || vd.Produce_Unit == "")
                        {
                            vd.Produce_Unit = "N/A";
                        }
                    }
                    else
                    {
                        vd.Produce_Unit = "N/A";
                    }
                    if (dtdata.Columns.Contains("Quantity"))
                    {
                        vd.Qty = dtdata.Rows[0]["Quantity"].ToString();
                        if (vd.Qty == null || vd.Qty == "")
                        {
                            vd.Qty = "N/A";
                        }
                    }
                    else
                    {
                        vd.Qty = "N/A";
                    }
                    if (dtdata.Columns.Contains("ReservedPrice"))
                    {
                        vd.ReservedPrice = dtdata.Rows[0]["ReservedPrice"].ToString();
                        if (vd.ReservedPrice == null || vd.ReservedPrice == "")
                        {
                            vd.ReservedPrice = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedPrice = "N/A";
                    }
                    if (dtdata.Columns.Contains("Payment_Mode"))
                    {
                        vd.PaymentMode = dtdata.Rows[0]["Payment_Mode"].ToString();
                        if (vd.PaymentMode == null || vd.PaymentMode == "")
                        {
                            vd.PaymentMode = "N/A";
                        }
                    }
                    else
                    {
                        vd.PaymentMode = "N/A";
                    }
                    if (dtdata.Columns.Contains("BiddingAmount"))
                    {
                        vd.BiddingAmount = dtdata.Rows[0]["BiddingAmount"].ToString();
                        if (vd.BiddingAmount == null || vd.BiddingAmount == "")
                        {
                            vd.BiddingAmount = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReservedPrice = "N/A";
                    }
                    if (dtdata.Columns.Contains("Emd_Amount"))
                    {
                        vd.EmdPaybleAmount = dtdata.Rows[0]["Emd_Amount"].ToString();
                        if (vd.EmdPaybleAmount == null || vd.EmdPaybleAmount == "")
                        {
                            vd.EmdPaybleAmount = "N/A";
                        }
                    }
                    else
                    {
                        vd.EmdPaybleAmount = "N/A";
                    }
                    if (dtdata.Columns.Contains("Payment_By"))
                    {
                        vd.OfflinePaymentMode = dtdata.Rows[0]["Payment_By"].ToString();
                        if (vd.OfflinePaymentMode == null || vd.OfflinePaymentMode == "")
                        {
                            vd.OfflinePaymentMode = "N/A";
                        }
                    }
                    else
                    {
                        vd.OfflinePaymentMode = "N/A";
                    }
                    if (dtdata.Columns.Contains("Bank_Name"))
                    {
                        vd.BankName = dtdata.Rows[0]["Bank_Name"].ToString();
                        if (vd.BankName == null || vd.BankName == "")
                        {
                            vd.BankName = "N/A";
                        }
                    }
                    else
                    {
                        vd.BankName = "N/A";
                    }
                    if (dtdata.Columns.Contains("Branch_Name"))
                    {
                        vd.BranchName = dtdata.Rows[0]["Branch_Name"].ToString();
                        if (vd.BranchName == null || vd.BranchName == "")
                        {
                            vd.BranchName = "N/A";
                        }
                    }
                    else
                    {
                        vd.BranchName = "N/A";
                    }
                    if (dtdata.Columns.Contains("DD_Chk_IssueDate"))
                    {
                        vd.DdchkIssuesDate = dtdata.Rows[0]["DD_Chk_IssueDate"].ToString();
                        if (vd.DdchkIssuesDate == null || vd.DdchkIssuesDate == "")
                        {
                            vd.DdchkIssuesDate = "N/A";
                        }
                    }
                    else
                    {
                        vd.DdchkIssuesDate = "N/A";
                    }
                    if (dtdata.Columns.Contains("DD_Chk_Number"))
                    {
                        vd.DdChkNumber = dtdata.Rows[0]["DD_Chk_Number"].ToString();
                        if (vd.DdChkNumber == null || vd.DdChkNumber == "")
                        {
                            vd.DdChkNumber = "N/A";
                        }
                    }
                    else
                    {
                        vd.DdChkNumber = "N/A";
                    }
                    if (dtdata.Columns.Contains("DD_Chk_Filepath"))
                    {
                        string str = dtdata.Rows[0]["DD_Chk_Filepath"].ToString();
                        string[] strsplit = str.Split('/');
                        vd.DdchkFilepth = strsplit[strsplit.Length - 1];
                        if (vd.DdchkFilepth == null || vd.DdchkFilepth == "")
                        {
                            vd.DdchkFilepth = "N/A";
                        }
                    }
                    else
                    {
                        vd.DdchkFilepth = "N/A";
                    }
                    if (dtdata.Columns.Contains("PS_Amount"))
                    {
                        vd.PsPaybleAmount = dtdata.Rows[0]["PS_Amount"].ToString();
                        if (vd.PsPaybleAmount == null || vd.PsPaybleAmount == "")
                        {
                            vd.PsPaybleAmount = "N/A";
                        }
                    }
                    else
                    {
                        vd.PsPaybleAmount = "N/A";
                    }
                    if (dtdata.Columns.Contains("Drop_Out_Reasond"))
                    {
                        vd.DropOutReason = dtdata.Rows[0]["Drop_Out_Reasond"].ToString();
                        if (vd.DropOutReason == null || vd.DropOutReason == "")
                        {
                            vd.DropOutReason = "N/A";
                        }
                    }
                    else
                    {
                        vd.DropOutReason = "N/A";
                    }
                }
                #endregion
                #region "Add Apply for Transit Permit by Arvind"
                if (vd.TableName == "tbl_TransitPermission")
                {
                    if (dtdata.Columns.Contains("Applicant_Type"))
                    {
                        if (dtdata.Rows[0]["Applicant_Type"].ToString() == "1")
                        {
                            vd.Applicant_Type = "Individual";
                        }
                        else
                        {
                            vd.Applicant_Type = "Organizational";
                        }
                    }
                    else
                    {
                        vd.Applicant_Type = "N/A";
                    }
                    if (dtdata.Columns.Contains("RequestID"))
                    {
                        vd.ReqID = dtdata.Rows[0]["RequestID"].ToString();
                        if (vd.ReqID == null || vd.ReqID == "")
                        {
                            vd.ReqID = "N/A";
                        }
                    }
                    else
                    {
                        vd.ReqID = "N/A";
                    }
                    if (dtdata.Columns.Contains("ToLocation"))
                    {
                        vd.Location = dtdata.Rows[0]["ToLocation"].ToString();
                        if (vd.Location == null || vd.Location == "")
                        {
                            vd.Location = "N/A";
                        }
                    }
                    else
                    {
                        vd.Location = "N/A";
                    }
                    if (dtdata.Columns.Contains("TransportMode"))
                    {
                        vd.TransportMode = dtdata.Rows[0]["TransportMode"].ToString();
                        if (vd.TransportMode == null || vd.TransportMode == "")
                        {
                            vd.TransportMode = "N/A";
                        }
                    }
                    else
                    {
                        vd.TransportMode = "N/A";
                    }
                    if (dtdata.Columns.Contains("VehicleNo"))
                    {
                        vd.VehicleNo = dtdata.Rows[0]["VehicleNo"].ToString();
                        if (vd.VehicleNo == null || vd.VehicleNo == "")
                        {
                            vd.VehicleNo = "N/A";
                        }
                    }
                    else
                    {
                        vd.VehicleNo = "N/A";
                    }
                    if (dtdata.Columns.Contains("DriverLicense"))
                    {
                        vd.DriverLicense = dtdata.Rows[0]["DriverLicense"].ToString();
                        if (vd.DriverLicense == null || vd.DriverLicense == "")
                        {
                            vd.DriverLicense = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverLicense = "N/A";
                    }
                    if (dtdata.Columns.Contains("DriverName"))
                    {
                        vd.DriverName = dtdata.Rows[0]["DriverName"].ToString();
                        if (vd.DriverName == null || vd.DriverName == "")
                        {
                            vd.DriverName = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverName = "N/A";
                    }
                    if (dtdata.Columns.Contains("DriverMobile"))
                    {
                        vd.Depot_Name = dtdata.Rows[0]["DriverMobile"].ToString();
                        if (vd.DriverMobileno == null || vd.DriverMobileno == "")
                        {
                            vd.DriverMobileno = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverMobileno = "N/A";
                    }
                    if (dtdata.Columns.Contains("DurationFrom"))
                    {
                        DateTime _date1, _date2;
                        _date1 = DateTime.Parse(dtdata.Rows[0]["Duration_From"].ToString());
                        _date2 = DateTime.Parse(dtdata.Rows[0]["Duration_To"].ToString());
                        vd.DurationsFrom = _date1.ToString("dd/MM/yyyy");
                        vd.DurationsTo = _date2.ToString("dd/MM/yyyy");
                        vd.Durations = _date1.ToString("dd-MM-yyyy") + " To " + _date2.ToString("dd-MM-yyyy");
                    }
                    else
                    {
                        vd.DurationsFrom = "N/A";
                        vd.DurationsTo = "N/A";
                    }
                    if (dtdata.Columns.Contains("AmountPaid"))
                    {
                        vd.PaidAMT = dtdata.Rows[0]["AmountPaid"].ToString();
                        if (vd.PaidAMT == null || vd.PaidAMT == "")
                        {
                            vd.PaidAMT = "N/A";
                        }
                    }
                    else
                    {
                        vd.PaidAMT = "N/A";
                    }
                }
                #endregion
                #region "Add Apply for Education Visit Service by Arvind"
                if (vd.TableName == "tbl_EducationTourPermissions")
                {
                    if (dtdata.Columns.Contains("College_Name"))
                    {
                        vd.CollegeName = dtdata.Rows[0]["College_Name"].ToString();
                        if (vd.CollegeName == null || vd.CollegeName == "")
                        {
                            vd.CollegeName = "N/A";
                        }
                    }
                    else
                    {
                        vd.CollegeName = "N/A";
                    }
                    if (dtdata.Columns.Contains("College_Address"))
                    {
                        vd.CollegeAddress = dtdata.Rows[0]["College_Address"].ToString();
                        if (vd.CollegeAddress == null || vd.CollegeAddress == "")
                        {
                            vd.CollegeAddress = "N/A";
                        }
                    }
                    else
                    {
                        vd.CollegeAddress = "N/A";
                    }
                    if (dtdata.Columns.Contains("College_Phoneno"))
                    {
                        vd.Phonenumber = dtdata.Rows[0]["College_Phoneno"].ToString();
                        if (vd.Phonenumber == null || vd.Phonenumber == "")
                        {
                            vd.Phonenumber = "N/A";
                        }
                    }
                    else
                    {
                        vd.Phonenumber = "N/A";
                    }
                    if (dtdata.Columns.Contains("Category"))
                    {
                        vd.Category = dtdata.Rows[0]["Category"].ToString();
                        if (vd.Category == null || vd.Category == "")
                        {
                            vd.Category = "N/A";
                        }
                    }
                    else
                    {
                        vd.Category = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Name"))
                    {
                        vd.Princeple_Name = dtdata.Rows[0]["P_Name"].ToString();
                        if (vd.Princeple_Name == null || vd.Princeple_Name == "")
                        {
                            vd.Princeple_Name = "N/A";
                        }
                    }
                    else
                    {
                        vd.Princeple_Name = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Address"))
                    {
                        vd.P_Address = dtdata.Rows[0]["P_Address"].ToString();
                        if (vd.P_Address == null || vd.P_Address == "")
                        {
                            vd.P_Address = "N/A";
                        }
                    }
                    else
                    {
                        vd.DriverName = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Gender"))
                    {
                        if (dtdata.Rows[0]["P_Gender"].ToString() == "1")
                        {
                            vd.P_Gender = "Male";
                        }
                        else if (dtdata.Rows[0]["P_Gender"].ToString() == "2")
                        {
                            vd.P_Gender = "Female";
                        }
                        else
                        {
                            vd.P_Gender = "Transgender";
                        }
                        if (vd.P_Gender == null || vd.P_Gender == "")
                        {
                            vd.P_Gender = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_Gender = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_Nationality"))
                    {
                        if (dtdata.Rows[0]["P_Nationality"].ToString() == "1")
                        {
                            vd.P_Natinality = "Indian";
                        }
                        else
                        {
                            vd.P_Natinality = "Foreigner";
                        }
                        if (vd.P_Natinality == null || vd.P_Natinality == "")
                        {
                            vd.P_Natinality = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_Natinality = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_MemberType"))
                    {
                        if (dtdata.Rows[0]["P_MemberType"].ToString() == "1")
                        {
                            vd.ddl_MemberType = "Child";
                        }
                        else if (dtdata.Rows[0]["P_MemberType"].ToString() == "2")
                        {
                            vd.ddl_MemberType = "Adult";
                        }
                        else
                        {
                            vd.ddl_MemberType = "Student";
                        }
                        if (vd.ddl_MemberType == null || vd.ddl_MemberType == "")
                        {
                            vd.ddl_MemberType = "N/A";
                        }
                    }
                    else
                    {
                        vd.ddl_MemberType = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_IDType"))
                    {
                        if (dtdata.Rows[0]["P_IDType"].ToString() == "1")
                        {
                            vd.P_MemberID = "Passport";
                        }
                        else if (dtdata.Rows[0]["P_IDType"].ToString() == "2")
                        {
                            vd.P_MemberID = "Aadhar";
                        }
                        else if (dtdata.Rows[0]["P_IDType"].ToString() == "3")
                        {
                            vd.P_MemberID = "Driving Licence";
                        }
                        else if (dtdata.Rows[0]["P_IDType"].ToString() == "4")
                        {
                            vd.P_MemberID = "Voter ID";
                        }
                        else
                        {
                            vd.P_MemberID = "PAN Card";
                        }
                        if (vd.P_MemberID == null || vd.P_MemberID == "")
                        {
                            vd.P_MemberID = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_MemberID = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_IDProofNo"))
                    {
                        vd.P_MemberIDProof = dtdata.Rows[0]["P_IDProofNo"].ToString();
                        if (vd.P_MemberIDProof == null || vd.P_MemberIDProof == "")
                        {
                            vd.P_MemberIDProof = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_MemberIDProof = "N/A";
                    }
                    if (dtdata.Columns.Contains("P_MemberNo"))
                    {
                        vd.P_NumberOfMember = dtdata.Rows[0]["P_MemberNo"].ToString();
                        if (vd.P_NumberOfMember == null || vd.P_NumberOfMember == "")
                        {
                            vd.P_NumberOfMember = "N/A";
                        }
                    }
                    else
                    {
                        vd.P_NumberOfMember = "N/A";
                    }
                    if (dtdata.Columns.Contains("DocMemberFilename"))
                    {
                        vd.MemberListPath = dtdata.Rows[0]["DocMemberFilename"].ToString();
                        if (vd.MemberListPath == null || vd.MemberListPath == "")
                        {
                            vd.MemberListPath = "N/A";
                        }
                    }
                    else
                    {
                        vd.MemberListPath = "N/A";
                    }
                    if (dtdata.Columns.Contains("DocEducationalfilename"))
                    {
                        vd.DocEducationalToueReqPath = dtdata.Rows[0]["DocEducationalfilename"].ToString();
                        if (vd.DocEducationalToueReqPath == null || vd.DocEducationalToueReqPath == "")
                        {
                            vd.DocEducationalToueReqPath = "N/A";
                        }
                    }
                    else
                    {
                        vd.DocEducationalToueReqPath = "N/A";
                    }
                    if (dtdata.Columns.Contains("CategoryName"))
                    {
                        vd.Vehiclecat = dtdata.Rows[0]["CategoryName"].ToString();
                        if (vd.Vehiclecat == null || vd.Vehiclecat == "")
                        {
                            vd.Vehiclecat = "N/A";
                        }
                    }
                    else
                    {
                        vd.Vehiclecat = "N/A";
                    }
                    if (dtdata.Columns.Contains("Vehiclename"))
                    {
                        vd.ddl_vehicle = dtdata.Rows[0]["Vehiclename"].ToString();
                        if (vd.ddl_vehicle == null || vd.ddl_vehicle == "")
                        {
                            vd.ddl_vehicle = "N/A";
                        }
                    }
                    else
                    {
                        vd.ddl_vehicle = "N/A";
                    }
                }
                #endregion
                #endregion
                return Json(vd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PdfFIR(string RequestId, string Status, string TableName)
        {
            DataTable dt = new DataTable();
            ActionRequest ar = new ActionRequest();
            DataSet DS = new DataSet();
            Status = "FIR";
            DS = ar.GetFIRDetails(RequestId, Status, TableName);
            //dt = ar.GetFIRDetails(RequestId, Status, TableName);
            string filepath = string.Empty;
            filepath = "~/ForestProtectionDocument/FIR_" + DateTime.Now.Ticks.ToString() + ".pdf";
            Document doc = new Document(PageSize.A4, 50, 50f, 50f, 50f);
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            var MyFontU = FontFactory.GetFont("Arial", 12, FontColour);
            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
            var MyFont1 = FontFactory.GetFont("Arial", 10, FontColour);
            PdfPCell cell;
            Phrase colHeading;
            PdfPTable pdfTable = null;
            doc.Open();
            doc.NewPage();
            tableheading = new Paragraph("Government of Rajasthan,Forest Department", MyFont);
            tableheading.Font.Size = 13;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            tableheading = new Paragraph("First Information Report", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            DataTable DT0 = DS.Tables[0];
            tableheading = new Paragraph("S.No. of FIR: :" + RequestId, MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of FIR: :" + DT0.Rows[0]["FIRDATE"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Place of Offense :" + DT0.Rows[0]["OffensePlace"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of Offense :" + DT0.Rows[0]["OffenseDate"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Time of Offense :" + DT0.Rows[0]["OffenseTime"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Description of Offense :" + DT0.Rows[0]["Description"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Offender Detail :-");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            DataTable DT2 = new DataTable();
            DT2 = DS.Tables[2];
            int count2 = DT2.Rows.Count;
            pdfTable = new PdfPTable(6);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count2 > 0)
            {
                string[,] arrPdfData = new string[count2, 6];
                arrPdfData[0, 0] = "Offender Name";
                arrPdfData[0, 1] = "Father Name";
                arrPdfData[0, 2] = "Address1";
                arrPdfData[0, 3] = "VillageCode";
                arrPdfData[0, 4] = "PhoneNo";
                arrPdfData[0, 5] = "Caste";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Offender Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("FatherName", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Address1", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("VillageCode", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("PhoneNo", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Caste", MyFont1)));
                for (int xid = 0; xid < count2; xid++)
                {
                    for (int yid = 0; yid < 6; yid++)
                    {
                        arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Details yields which are caught from forest :");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT3 = new DataTable();
            DT3 = DS.Tables[3];
            int count = DT3.Rows.Count;
            pdfTable = new PdfPTable(5);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count > 0)
            {
                tableheading = new Paragraph("Animal Article");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count, 5];
                arrPdfData[0, 0] = "Scientific Name";
                arrPdfData[0, 1] = "Name";
                arrPdfData[0, 2] = "Animal Article Name";
                arrPdfData[0, 3] = "Description";
                arrPdfData[0, 4] = "Quantity";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Scientific Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Article Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Description", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Quantity", MyFont1)));
                for (int xid = 0; xid < count; xid++)
                {
                    for (int yid = 0; yid < 5; yid++)
                    {
                        arrPdfData[xid, yid] = DT3.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT4 = new DataTable();
            DT4 = DS.Tables[4];
            int count4 = DT4.Rows.Count;
            pdfTable = new PdfPTable(6);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count4 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Equipment Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count4, 6];
                arrPdfData[0, 0] = "Equipment Name";
                arrPdfData[0, 1] = "Make";
                arrPdfData[0, 2] = "Model";
                arrPdfData[0, 3] = "Caliber";
                arrPdfData[0, 4] = "IdentificationNo";
                arrPdfData[0, 5] = "size";
                pdfTable.AddCell(new Paragraph("Equipment Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Make", MyFont1));
                pdfTable.AddCell(new Paragraph("Model", MyFont1));
                pdfTable.AddCell(new Paragraph("Caliber", MyFont1));
                pdfTable.AddCell(new Paragraph("IdentificationNo", MyFont1));
                pdfTable.AddCell(new Paragraph("size", MyFont1));
                for (int xid = 0; xid < count4; xid++)
                {
                    for (int yid = 0; yid < 6; yid++)
                    {
                        arrPdfData[xid, yid] = DT4.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT5 = new DataTable();
            DT5 = DS.Tables[5];
            int count5 = DT5.Rows.Count;
            pdfTable = new PdfPTable(3);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count5 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Species Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count5, 3];
                arrPdfData[0, 0] = "Species Name";
                arrPdfData[0, 1] = "Produce Type";
                arrPdfData[0, 2] = "Quantity";
                pdfTable.AddCell(new Paragraph("Species Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Produce Type", MyFont1));
                pdfTable.AddCell(new Paragraph("Quantity", MyFont1));
                for (int xid = 0; xid < count5; xid++)
                {
                    for (int yid = 0; yid < 3; yid++)
                    {
                        arrPdfData[xid, yid] = DT5.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT6 = new DataTable();
            DT6 = DS.Tables[6];
            int count6 = DT5.Rows.Count;
            pdfTable = new PdfPTable(8);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count6 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Vehicle Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count6, 8];
                arrPdfData[0, 0] = "RegistrationNo";
                arrPdfData[0, 1] = "OwnerName";
                arrPdfData[0, 2] = "VehicleMake";
                arrPdfData[0, 3] = "VehicleModel";
                arrPdfData[0, 4] = "ChassisNo";
                arrPdfData[0, 5] = "EngineNo";
                arrPdfData[0, 6] = "PastOffenses";
                arrPdfData[0, 7] = "CategoryName";
                pdfTable.AddCell(new Paragraph("Registration No", MyFont1));
                pdfTable.AddCell(new Paragraph("Owner Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Vehicle Make", MyFont1));
                pdfTable.AddCell(new Paragraph("Vehicle Model", MyFont1));
                pdfTable.AddCell(new Paragraph("Chassis No", MyFont1));
                pdfTable.AddCell(new Paragraph("Engine No", MyFont1));
                pdfTable.AddCell(new Paragraph("Past Offenses", MyFont1));
                pdfTable.AddCell(new Paragraph("Category Name", MyFont1));
                for (int xid = 0; xid < count6; xid++)
                {
                    for (int yid = 0; yid < 8; yid++)
                    {
                        arrPdfData[xid, yid] = DT6.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT7 = new DataTable();
            DT7 = DS.Tables[7];
            int count7 = DT7.Rows.Count;
            pdfTable = new PdfPTable(4);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count7 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Animal Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count7, 4];
                arrPdfData[0, 0] = "Name";
                arrPdfData[0, 1] = "Scientific Name";
                arrPdfData[0, 2] = "Description";
                arrPdfData[0, 3] = "Weight";
                pdfTable.AddCell(new Paragraph("Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Scientific Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Description", MyFont1));
                pdfTable.AddCell(new Paragraph("Weight", MyFont1));
                for (int xid = 0; xid < count7; xid++)
                {
                    for (int yid = 0; yid < 4; yid++)
                    {
                        arrPdfData[xid, yid] = DT7.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT8 = new DataTable();
            DT8 = DS.Tables[8];
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("To Whom it has been handovered");
            tableheading.Font.Size = 10;
            tableheading.Font.IsUnderlined();
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("First Officer :" + DT8.Rows[0]["Emp1"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Second Officer :" + DT8.Rows[0]["Emp2"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT1 = new DataTable();
            DT1 = DS.Tables[1];
            int count1 = DT1.Rows.Count;
            pdfTable = new PdfPTable(4);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count1 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Witness Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count1, 4];
                arrPdfData[0, 0] = "Witness Name";
                arrPdfData[0, 1] = "Father Name";
                arrPdfData[0, 2] = "Address";
                arrPdfData[0, 3] = "PhoneNo";
                pdfTable.AddCell(new Paragraph("WitnessName", MyFont1));
                pdfTable.AddCell(new Paragraph("FatherName", MyFont1));
                pdfTable.AddCell(new Paragraph("Address1", MyFont1));
                pdfTable.AddCell(new Paragraph("PhoneNo", MyFont1));
                for (int xid = 0; xid < count1; xid++)
                {
                    for (int yid = 0; yid < 4; yid++)
                    {
                        arrPdfData[xid, yid] = DT1.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Signature of officer");
            tableheading.Font.Size = 10;
            tableheading.Font.IsUnderlined();
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            doc.Close();
            if (System.IO.File.Exists(Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
            return RedirectToAction("ForesterAction", "ForesterAction");
        }
        public ActionResult PdfWarrant(string RequestId, string Status, string TableName)
        {
            #region Warrant Generate
            Status = "Warrant";
            DataTable dt = new DataTable();
            ActionRequest ar = new ActionRequest();
            DataSet DS = new DataSet();
            DS = ar.GetFIRDetails(RequestId, Status, TableName);
            //dt = ar.GetFIRDetails(RequestId, Status, TableName);
            string filepath = string.Empty;
            filepath = "~/ForestProtectionDocument/FIR_" + DateTime.Now.Ticks.ToString() + ".pdf";
            Document doc = new Document(PageSize.A4, 50, 50f, 50f, 50f);
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            var MyFontU = FontFactory.GetFont("Arial", 12, FontColour);
            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
            var MyFont1 = FontFactory.GetFont("Arial", 10, FontColour);
            PdfPCell cell;
            Phrase colHeading;
            PdfPTable pdfTable = null;
            doc.Open();
            doc.NewPage();
            tableheading = new Paragraph("Government of Rajasthan,Forest Department", MyFont);
            tableheading.Font.Size = 13;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            tableheading = new Paragraph("Appearance Notice", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            DataTable DT0 = DS.Tables[0];
            tableheading = new Paragraph("S.No. of Notice: :" + RequestId, MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of FIR: :" + DT0.Rows[0]["FIRDATE"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Place of Offense :" + DT0.Rows[0]["OffensePlace"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of Offense :" + DT0.Rows[0]["OffenseDate"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Time of Offense :" + DT0.Rows[0]["OffenseTime"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Description of Offense :" + DT0.Rows[0]["Description"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT2 = new DataTable();
            DT2 = DS.Tables[2];
            tableheading = new Paragraph("Date of Appearance :" + DT2.Rows[0]["AppreanceDate"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Offender Detail :-");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            int count2 = DT2.Rows.Count;
            pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count2 > 0)
            {
                string[,] arrPdfData = new string[count2, 7];
                arrPdfData[0, 0] = "Offender Name";
                arrPdfData[0, 1] = "Father Name";
                arrPdfData[0, 2] = "Address";
                arrPdfData[0, 3] = "Village";
                arrPdfData[0, 4] = "PhoneNo";
                arrPdfData[0, 5] = "Caste";
                arrPdfData[0, 5] = "Appreance Date";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Offender Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("FatherName", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Address1", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Village", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("PhoneNo", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Caste", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Appreance Date", MyFont1)));
                for (int xid = 0; xid < count2; xid++)
                {
                    for (int yid = 0; yid < 7; yid++)
                    {
                        arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Details Forest Produce which are caught from forest :");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT3 = new DataTable();
            DT3 = DS.Tables[3];
            int count = DT3.Rows.Count;
            pdfTable = new PdfPTable(5);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count > 0)
            {
                tableheading = new Paragraph("Animal Article");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count, 5];
                arrPdfData[0, 0] = "Scientific Name";
                arrPdfData[0, 1] = "Name";
                arrPdfData[0, 2] = "Animal Article Name";
                arrPdfData[0, 3] = "Description";
                arrPdfData[0, 4] = "Quantity";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Scientific Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Article Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Description", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Quantity", MyFont1)));
                for (int xid = 0; xid < count; xid++)
                {
                    for (int yid = 0; yid < 5; yid++)
                    {
                        arrPdfData[xid, yid] = DT3.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT4 = new DataTable();
            DT4 = DS.Tables[4];
            int count4 = DT4.Rows.Count;
            pdfTable = new PdfPTable(6);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count4 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Equipment Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count4, 6];
                arrPdfData[0, 0] = "Equipment Name";
                arrPdfData[0, 1] = "Make";
                arrPdfData[0, 2] = "Model";
                arrPdfData[0, 3] = "Caliber";
                arrPdfData[0, 4] = "IdentificationNo";
                arrPdfData[0, 5] = "size";
                pdfTable.AddCell(new Paragraph("Equipment Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Make", MyFont1));
                pdfTable.AddCell(new Paragraph("Model", MyFont1));
                pdfTable.AddCell(new Paragraph("Caliber", MyFont1));
                pdfTable.AddCell(new Paragraph("IdentificationNo", MyFont1));
                pdfTable.AddCell(new Paragraph("size", MyFont1));
                for (int xid = 0; xid < count4; xid++)
                {
                    for (int yid = 0; yid < 6; yid++)
                    {
                        arrPdfData[xid, yid] = DT4.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT5 = new DataTable();
            DT5 = DS.Tables[5];
            int count5 = DT5.Rows.Count;
            pdfTable = new PdfPTable(3);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count5 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Species Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count5, 3];
                arrPdfData[0, 0] = "Species Name";
                arrPdfData[0, 1] = "Produce Type";
                arrPdfData[0, 2] = "Quantity";
                pdfTable.AddCell(new Paragraph("Species Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Produce Type", MyFont1));
                pdfTable.AddCell(new Paragraph("Quantity", MyFont1));
                for (int xid = 0; xid < count5; xid++)
                {
                    for (int yid = 0; yid < 3; yid++)
                    {
                        arrPdfData[xid, yid] = DT5.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT6 = new DataTable();
            DT6 = DS.Tables[6];
            int count6 = DT5.Rows.Count;
            pdfTable = new PdfPTable(8);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count6 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Vehicle Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count6, 8];
                arrPdfData[0, 0] = "RegistrationNo";
                arrPdfData[0, 1] = "OwnerName";
                arrPdfData[0, 2] = "VehicleMake";
                arrPdfData[0, 3] = "VehicleModel";
                arrPdfData[0, 4] = "ChassisNo";
                arrPdfData[0, 5] = "EngineNo";
                arrPdfData[0, 6] = "PastOffenses";
                arrPdfData[0, 7] = "CategoryName";
                pdfTable.AddCell(new Paragraph("Registration No", MyFont1));
                pdfTable.AddCell(new Paragraph("Owner Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Vehicle Make", MyFont1));
                pdfTable.AddCell(new Paragraph("Vehicle Model", MyFont1));
                pdfTable.AddCell(new Paragraph("Chassis No", MyFont1));
                pdfTable.AddCell(new Paragraph("Engine No", MyFont1));
                pdfTable.AddCell(new Paragraph("Past Offenses", MyFont1));
                pdfTable.AddCell(new Paragraph("Category Name", MyFont1));
                for (int xid = 0; xid < count6; xid++)
                {
                    for (int yid = 0; yid < 8; yid++)
                    {
                        arrPdfData[xid, yid] = DT6.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT7 = new DataTable();
            DT7 = DS.Tables[7];
            int count7 = DT7.Rows.Count;
            pdfTable = new PdfPTable(4);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count7 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Animal Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count7, 4];
                arrPdfData[0, 0] = "Name";
                arrPdfData[0, 1] = "Scientific Name";
                arrPdfData[0, 2] = "Description";
                arrPdfData[0, 3] = "Weight";
                pdfTable.AddCell(new Paragraph("Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Scientific Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Description", MyFont1));
                pdfTable.AddCell(new Paragraph("Weight", MyFont1));
                for (int xid = 0; xid < count7; xid++)
                {
                    for (int yid = 0; yid < 4; yid++)
                    {
                        arrPdfData[xid, yid] = DT7.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT8 = new DataTable();
            DT8 = DS.Tables[8];
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("To Whom it has been handovered");
            tableheading.Font.Size = 10;
            tableheading.Font.IsUnderlined();
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("First Officer :" + DT8.Rows[0]["Emp1"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Second Officer :" + DT8.Rows[0]["Emp2"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT1 = new DataTable();
            DT1 = DS.Tables[1];
            int count1 = DT1.Rows.Count;
            pdfTable = new PdfPTable(4);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count1 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Witness Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count1, 4];
                arrPdfData[0, 0] = "Witness Name";
                arrPdfData[0, 1] = "Father Name";
                arrPdfData[0, 2] = "Address";
                arrPdfData[0, 3] = "PhoneNo";
                pdfTable.AddCell(new Paragraph("WitnessName", MyFont1));
                pdfTable.AddCell(new Paragraph("FatherName", MyFont1));
                pdfTable.AddCell(new Paragraph("Address1", MyFont1));
                pdfTable.AddCell(new Paragraph("PhoneNo", MyFont1));
                for (int xid = 0; xid < count1; xid++)
                {
                    for (int yid = 0; yid < 4; yid++)
                    {
                        arrPdfData[xid, yid] = DT1.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Signature of officer");
            tableheading.Font.Size = 10;
            tableheading.Font.IsUnderlined();
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            doc.Close();
            if (System.IO.File.Exists(Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
            #endregion
            return RedirectToAction("ForesterAction", "ForesterAction");
        }
        public ActionResult PdfCompounding(string RequestId, string Status, string TableName)
        {
            #region cOMPOUNDING Generate
            Status = "Compounding";
            DataTable dt = new DataTable();
            ActionRequest ar = new ActionRequest();
            DataSet DS = new DataSet();
            DS = ar.GetCompoundDetails(RequestId, Status, "tbl_FPM_ForesterParivadCategory");
            //dt = ar.GetFIRDetails(RequestId, Status, TableName);
            string filepath = string.Empty;
            filepath = "~/ForestProtectionDocument/Comp_" + DateTime.Now.Ticks.ToString() + ".pdf";
            Document doc = new Document(PageSize.A4, 50, 50f, 50f, 50f);
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            var MyFontU = FontFactory.GetFont("Arial", 12, FontColour);
            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
            var MyFont1 = FontFactory.GetFont("Arial", 10, FontColour);
            PdfPCell cell;
            Phrase colHeading;
            PdfPTable pdfTable = null;
            doc.Open();
            doc.NewPage();
            tableheading = new Paragraph("Government of Rajasthan,Forest Department", MyFont);
            tableheading.Font.Size = 13;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            tableheading = new Paragraph("Compounding Details", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            DataTable DT0 = DS.Tables[0];
            tableheading = new Paragraph("Date of FIR: :" + DT0.Rows[0]["FIRDATE"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Place of Offense :" + DT0.Rows[0]["OffensePlace"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of Offense :" + DT0.Rows[0]["OffenseDate"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Time of Offense :" + DT0.Rows[0]["OffenseTime"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Description of Offense :" + DT0.Rows[0]["Description"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT2 = new DataTable();
            DT2 = DS.Tables[2];
            tableheading = new Paragraph("Date of Appearance :" + DT2.Rows[0]["AppreanceDate"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Compounding Detail :-");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            DataTable DT1 = new DataTable();
            DT1 = DS.Tables[1];
            int count1 = DT1.Rows.Count;
            pdfTable = new PdfPTable(4);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count1 > 0)
            {
                string[,] arrPdfData = new string[count1, 4];
                arrPdfData[0, 0] = "Amount Paid";
                arrPdfData[0, 1] = "Fine Amount";
                arrPdfData[0, 2] = "Transaction Status";
                arrPdfData[0, 3] = "Emitra Transaction ID";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Amount Paid", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Fine Amount", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Transaction Status", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Emitra Transaction ID", MyFont1)));
                for (int xid = 0; xid < count1; xid++)
                {
                    for (int yid = 0; yid < 4; yid++)
                    {
                        arrPdfData[xid, yid] = DT1.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            tableheading = new Paragraph("Offender Detail :-");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            int count2 = DT2.Rows.Count;
            pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count2 > 0)
            {
                string[,] arrPdfData = new string[count2, 7];
                arrPdfData[0, 0] = "Offender Name";
                arrPdfData[0, 1] = "Father Name";
                arrPdfData[0, 2] = "Address";
                arrPdfData[0, 3] = "Village";
                arrPdfData[0, 4] = "PhoneNo";
                arrPdfData[0, 5] = "Caste";
                arrPdfData[0, 6] = "Appreance Date";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Offender Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("FatherName", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Address1", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Village", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("PhoneNo", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Caste", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Appreance Date", MyFont1)));
                for (int xid = 0; xid < count2; xid++)
                {
                    for (int yid = 0; yid < 7; yid++)
                    {
                        arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Signature of officer");
            tableheading.Font.Size = 10;
            tableheading.Font.IsUnderlined();
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            doc.Close();
            if (System.IO.File.Exists(Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
            #endregion
            return RedirectToAction("ForesterAction", "ForesterAction");
        }
        public ActionResult PdfSeizedItem(string RequestId, string Status, string TableName)
        {
            #region Seized Generate
            Status = "Seizeditem";
            DataTable dt = new DataTable();
            ActionRequest ar = new ActionRequest();
            DataSet DS = new DataSet();
            DS = ar.GetSeizedDetails(RequestId, Status, "tbl_FPM_ForesterParivadCategory");
            //dt = ar.GetFIRDetails(RequestId, Status, TableName);
            string filepath = string.Empty;
            filepath = "~/ForestProtectionDocument/FIR_" + DateTime.Now.Ticks.ToString() + ".pdf";
            Document doc = new Document(PageSize.A4, 50, 50f, 50f, 50f);
            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            var MyFontU = FontFactory.GetFont("Arial", 12, FontColour);
            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
            var MyFont1 = FontFactory.GetFont("Arial", 10, FontColour);
            PdfPCell cell;
            Phrase colHeading;
            PdfPTable pdfTable = null;
            doc.Open();
            doc.NewPage();
            tableheading = new Paragraph("Government of Rajasthan,Forest Department", MyFont);
            tableheading.Font.Size = 13;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            tableheading = new Paragraph("Release letter", MyFont);
            tableheading.Font.Size = 12;
            tableheading.Alignment = (Element.ALIGN_CENTER);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            DataTable DT0 = DS.Tables[0];
            tableheading = new Paragraph("Requested ID: :" + RequestId, MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Place of Offense :" + DT0.Rows[0]["OffensePlace"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of Offense :" + DT0.Rows[0]["OffenseDate"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Time of Offense :" + DT0.Rows[0]["OffenseTime"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Description of Offense :" + DT0.Rows[0]["Description"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Status :" + DT0.Rows[0]["StatusDesc"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            tableheading = new Paragraph("Date of Action: :" + DT0.Rows[0]["FIRDATE"].ToString(), MyFont);
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT1 = new DataTable();
            DT1 = DS.Tables[1];
            tableheading = new Paragraph("Offender Detail :-");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            doc.Add(new Paragraph(Environment.NewLine));
            int count1 = DT1.Rows.Count;
            pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count1 > 0)
            {
                string[,] arrPdfData = new string[count1, 7];
                arrPdfData[0, 0] = "Offender Name";
                arrPdfData[0, 1] = "Father Name";
                arrPdfData[0, 2] = "Address";
                arrPdfData[0, 3] = "Village";
                arrPdfData[0, 4] = "PhoneNo";
                arrPdfData[0, 5] = "Caste";
                arrPdfData[0, 5] = "Appreance Date";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Offender Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("FatherName", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Address1", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Village", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("PhoneNo", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Caste", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Appreance Date", MyFont1)));
                for (int xid = 0; xid < count1; xid++)
                {
                    for (int yid = 0; yid < 7; yid++)
                    {
                        arrPdfData[xid, yid] = DT1.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Details Forest Produce which will be released :");
            tableheading.Font.Size = 10;
            tableheading.Alignment = (Element.ALIGN_LEFT);
            doc.Add(tableheading);
            DataTable DT2 = new DataTable();
            DT2 = DS.Tables[2];
            int count = DT2.Rows.Count;
            pdfTable = new PdfPTable(5);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count > 0)
            {
                tableheading = new Paragraph("Animal Article");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count, 5];
                arrPdfData[0, 0] = "Scientific Name";
                arrPdfData[0, 1] = "Name";
                arrPdfData[0, 2] = "Animal Article Name";
                arrPdfData[0, 3] = "Description";
                arrPdfData[0, 4] = "Quantity";
                pdfTable.AddCell(new PdfPCell(new Paragraph("Scientific Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Article Name", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Description", MyFont1)));
                pdfTable.AddCell(new PdfPCell(new Paragraph("Quantity", MyFont1)));
                for (int xid = 0; xid < count; xid++)
                {
                    for (int yid = 0; yid < 5; yid++)
                    {
                        arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT3 = new DataTable();
            DT3 = DS.Tables[3];
            int count4 = DT3.Rows.Count;
            pdfTable = new PdfPTable(6);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count4 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Equipment Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count4, 6];
                arrPdfData[0, 0] = "Equipment Name";
                arrPdfData[0, 1] = "Make";
                arrPdfData[0, 2] = "Model";
                arrPdfData[0, 3] = "Caliber";
                arrPdfData[0, 4] = "IdentificationNo";
                arrPdfData[0, 5] = "size";
                pdfTable.AddCell(new Paragraph("Equipment Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Make", MyFont1));
                pdfTable.AddCell(new Paragraph("Model", MyFont1));
                pdfTable.AddCell(new Paragraph("Caliber", MyFont1));
                pdfTable.AddCell(new Paragraph("IdentificationNo", MyFont1));
                pdfTable.AddCell(new Paragraph("size", MyFont1));
                for (int xid = 0; xid < count4; xid++)
                {
                    for (int yid = 0; yid < 6; yid++)
                    {
                        arrPdfData[xid, yid] = DT3.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT4 = new DataTable();
            DT4 = DS.Tables[4];
            int count5 = DT4.Rows.Count;
            pdfTable = new PdfPTable(3);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count5 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Species Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count5, 3];
                arrPdfData[0, 0] = "Species Name";
                arrPdfData[0, 1] = "Produce Type";
                arrPdfData[0, 2] = "Quantity";
                pdfTable.AddCell(new Paragraph("Species Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Produce Type", MyFont1));
                pdfTable.AddCell(new Paragraph("Quantity", MyFont1));
                for (int xid = 0; xid < count5; xid++)
                {
                    for (int yid = 0; yid < 3; yid++)
                    {
                        arrPdfData[xid, yid] = DT4.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT5 = new DataTable();
            DT5 = DS.Tables[5];
            int count6 = DT5.Rows.Count;
            pdfTable = new PdfPTable(8);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count6 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Vehicle Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count6, 8];
                arrPdfData[0, 0] = "RegistrationNo";
                arrPdfData[0, 1] = "OwnerName";
                arrPdfData[0, 2] = "VehicleMake";
                arrPdfData[0, 3] = "VehicleModel";
                arrPdfData[0, 4] = "ChassisNo";
                arrPdfData[0, 5] = "EngineNo";
                arrPdfData[0, 6] = "PastOffenses";
                arrPdfData[0, 7] = "CategoryName";
                pdfTable.AddCell(new Paragraph("Registration No", MyFont1));
                pdfTable.AddCell(new Paragraph("Owner Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Vehicle Make", MyFont1));
                pdfTable.AddCell(new Paragraph("Vehicle Model", MyFont1));
                pdfTable.AddCell(new Paragraph("Chassis No", MyFont1));
                pdfTable.AddCell(new Paragraph("Engine No", MyFont1));
                pdfTable.AddCell(new Paragraph("Past Offenses", MyFont1));
                pdfTable.AddCell(new Paragraph("Category Name", MyFont1));
                for (int xid = 0; xid < count6; xid++)
                {
                    for (int yid = 0; yid < 8; yid++)
                    {
                        arrPdfData[xid, yid] = DT5.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            DataTable DT6 = new DataTable();
            DT6 = DS.Tables[6];
            int count7 = DT6.Rows.Count;
            pdfTable = new PdfPTable(4);
            pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 95;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            if (count7 > 0)
            {
                doc.Add(new Paragraph(Environment.NewLine));
                tableheading = new Paragraph("Animal Details");
                tableheading.Font.Size = 10;
                tableheading.Font.IsUnderlined();
                tableheading.Alignment = (Element.ALIGN_LEFT);
                doc.Add(tableheading);
                doc.Add(new Paragraph(Environment.NewLine));
                string[,] arrPdfData = new string[count7, 4];
                arrPdfData[0, 0] = "Name";
                arrPdfData[0, 1] = "Scientific Name";
                arrPdfData[0, 2] = "Description";
                arrPdfData[0, 3] = "Weight";
                pdfTable.AddCell(new Paragraph("Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Scientific Name", MyFont1));
                pdfTable.AddCell(new Paragraph("Description", MyFont1));
                pdfTable.AddCell(new Paragraph("Weight", MyFont1));
                for (int xid = 0; xid < count7; xid++)
                {
                    for (int yid = 0; yid < 4; yid++)
                    {
                        arrPdfData[xid, yid] = DT6.Rows[xid][yid].ToString();
                        colHeading = new Phrase(arrPdfData[xid, yid]);
                        colHeading.Font.Size = 9;
                        cell = new PdfPCell(new Phrase(colHeading));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfTable.AddCell(cell);
                    }
                }
                doc.Add(pdfTable);
            }
            doc.Add(new Paragraph(Environment.NewLine));
            doc.Add(new Paragraph(Environment.NewLine));
            tableheading = new Paragraph("Signature of officer");
            tableheading.Font.Size = 10;
            tableheading.Font.IsUnderlined();
            tableheading.Alignment = (Element.ALIGN_RIGHT);
            doc.Add(tableheading);
            doc.Close();
            if (System.IO.File.Exists(Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }
            #endregion
            return RedirectToAction("ForesterAction", "ForesterAction");
        }

        #region SearchReplaceDoc
        //public void SaveDocs(long objectID, int objectTypeID, List<CommonDocument> docs)
        //{
        //    string docPath = FMDSS.Globals.Util.GetAppSettings("FRADocumentPath");
        //    string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/" + docPath);

        //    foreach (var item in docs)
        //    {
        //        FileInfo f1 = new FileInfo(HttpContext.Current.Server.MapPath("~/" + item.DocumentPath));
        //        f1.CopyTo(string.Format("{0}{1}_{2}_{3}_{4}", dirPath, objectTypeID, objectID, item.DocumentTypeID, item.DocumentName), true);
        //        f1.Delete();
        //    }
        //}

        private void SearchReplaceDoc()
        {
            var rootPath = Server.MapPath("~/");
            string inputFile = string.Format("{0}Documents/Reaserch_GOI_Request_Letter_Format.docx", rootPath);
            string outputFile = string.Format("{0}PermissionDocument/{1}_ResearchLetter.docx", rootPath, DateTime.Now.Ticks);

            // Copy Word document.
            System.IO.File.Copy(inputFile, outputFile, true);

            // Open copied document.
            //using (var flatDocument = new FindAndReplace.FlatDocument(outputFile))
            //{
            //    // Search and replace document's text content.
            //    flatDocument.FindAndReplace("[TITLE]", "Lorem Ipsum");
            //    flatDocument.FindAndReplace("[SUBTITLE]", "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet...");
            //    flatDocument.FindAndReplace("[NAME]", "John Doe");
            //    flatDocument.FindAndReplace("[EMAIL]", "john.doe@email.com");
            //    flatDocument.FindAndReplace("[PHONE]", "(000)-111-222");
            //    // Save document on Dispose.

            //}
        }
        [ValidateInput(false)]
        public JsonResult CreateDocument(string Html, string requestId)
        {
            //Render any HTML fragment or document to HTML
            try
            {
                var Renderer = new IronPdf.HtmlToPdf();
                var PDF = Renderer.RenderHtmlAsPdf(Html);
                string filepath = "~/ResearchDocument/CreatedDocument/research_" + requestId + ".pdf";
                var OutputPath = Server.MapPath(filepath);
                PDF.SaveAs(OutputPath);



                SqlParameter[] parameters = {
                new SqlParameter("@RequestId", requestId),
                new SqlParameter("@CreatedBy", Convert.ToInt64(Session["UserId"])),
                new SqlParameter("@DocumentText", Html),
                new SqlParameter("@CreatedDate", DateTime.Now),
                new SqlParameter("@DocumentPath",filepath.TrimStart('~') )
            };
                int i = cm.ExecuteNonQuery("Sp_InsertResearchCreatedDocument", parameters);
                return Json(new { status_code = "200", msg = "document saved successfully.", status = "success", filepath = filepath.TrimStart('~') }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status_code = "0", msg = ex.Message, status = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDocumentTemplate(string Type)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parameters =  {
                new SqlParameter("@Type", Type),
            };
                cm.Fill(ds, "Sp_GetResearchDefaultTemplateText", parameters);
                string text = string.Empty;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    text = Convert.ToString(ds.Tables[0].Rows[0]["DefaultText"]);
                }
                return Json(new { status_code = "200", msg = text, status = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status_code = "0", msg = ex.Message, status = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        public JsonResult GetEncrypt(string text)
        {
            string result = Encryption.encrypt(text);
            return Json(new { status_code = "200", msg = result, status = "success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransitPermitdashboardDetails(int id)
        {
            CitizenTransmitpermitSummary obj = new CitizenTransmitpermitSummary();
            DataTable data = new DataTable();
            obj.UserId = Convert.ToInt64(Session["UserID"].ToString());
            string role = Convert.ToString(Session["CURRENT_ROLE"]);

            data = obj.dashboardDetails(obj.UserId, id);
            ViewBag.datalist = data;
            ViewBag.role = role;
            return View();
        }


    }
    /// <summary>
    /// Property declaration
    /// </summary>
    public class viewdetail
    {
        #region Property
        public string DownloadNOC { get; set; }
        public string RequestId { get; set; }
        public string ModuleName { get; set; }
        public string ServiceType { get; set; }
        public string PermissionType { get; set; }
        public string PermissionName { get; set; }
        public string RequestedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime RequestOn { get; set; }
        public string RequestedBy { get; set; }
        public string Duration { get; set; }
        public string PaymentDone { get; set; }
        public decimal PaidAmount { get; set; }
        public string NumberOfPerson { get; set; }
        public bool IsReviewer { get; set; }
        public int ApprovedBy { get; set; }
        public string Remarks { get; set; }
        public int ActionTakenBy { get; set; }
        public string TableName { get; set; }
        public string ApplicationType { get; set; }
        public string FilmTitle { get; set; }
        public string ShootingPurpose { get; set; }
        public string IdProof { get; set; }
        public string IdProofNo { get; set; }
        public string NumberOfDay { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string Block { get; set; }
        public string GramPanchayat { get; set; }
        public string Village { get; set; }
        public string khasraNo { get; set; }
        public string Qualification { get; set; }
        public string College { get; set; }
        public string ResearchSubject { get; set; }
        public string ResearchProcedure { get; set; }
        public string AnimalCategory { get; set; }
        public string AnimalName { get; set; }
        public string SpeciesCategory { get; set; }
        public string SpeciesName { get; set; }
        public string FixedPermissionName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StatusDesc { get; set; }
        public string Reason_Desc { get; set; }
        public string KmlPath { get; set; }
        public string Revenue_Record_Path { get; set; }
        public string Revenue_Map_Path { get; set; }
        public string IsGTSheetAvaliable { get; set; }
        public string Nearest_WaterSource { get; set; }
        public string WaterSource_Distance { get; set; }
        public string Forest_Distance { get; set; }
        public string Wildlife_Distance { get; set; }
        public string Tree_species { get; set; }
        public string AravalliHills { get; set; }
        public string ForestLand { get; set; }
        public string Plantation_Area { get; set; }
        public string Animal_SerialNo { get; set; }
        public string Species_SerialNo { get; set; }
        public string Benefits { get; set; }
        public string CoordinatorId { get; set; }
        public string IdProofUrl { get; set; }
        public string Description { get; set; }
        public string IndianCitizen { get; set; }
        public string NonIndianCitizen { get; set; }
        public string Students { get; set; }
        public string Purpose_OCM { get; set; }
        public string Additional_Document { get; set; }
        public string Citizen_Comment { get; set; }
        public string Survey_Document { get; set; }
        public string KmlView { get; set; }
        public string OffenderName { get; set; }
        public string FatherName { get; set; }
        public string Caste { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string EvidenceDoc { get; set; }
        public string OffenderAge { get; set; }
        public string PoliceStation { get; set; }
        public string ClothesWorn { get; set; }
        public string ClothesColor { get; set; }
        public string PhysicalAppearance { get; set; }
        public string Height { get; set; }
        // More fields added by Vandana Gupta in Research Study for the changes came on 06-Aug-2016
        public string SynopsisPath { get; set; }
        public string PresentationPath { get; set; }
        public string AssistName { get; set; }
        public string AssistIdProofPath { get; set; }
        public string VehicleName { get; set; }
        public string NOCType { get; set; }
        #endregion
        #region BudgetEstimation
        public string EstimatedAmount { get; set; }
        public string ReviewedAmount { get; set; }
        public string ApprovedAmount { get; set; }
        public string FinanceYear { get; set; }
        #endregion
        #region MicroPlan
        public string MicroPlanCode { get; set; }
        public string MicroPlanName { get; set; }
        public string WorkOrderName { get; set; }
        public string WorkOrderType { get; set; }
        public string BudgetHead { get; set; }
        public string FinancialTarget { get; set; }
        public string AdminApprovaldate { get; set; }
        public string FinancialApprovaldate { get; set; }
        public string MicroDateOfRequest { get; set; }
        public string NGO_SHG { get; set; }
        public string NGO_SHGName { get; set; }
        public string RevenueVillage { get; set; }
        public string PanchayatComittee { get; set; }
        public string ForestAdminUnit { get; set; }
        public string RangeOfficeUnit { get; set; }
        public string TotalArea { get; set; }
        public string TotalLandsqkm { get; set; }
        public string TotalForestsqkm { get; set; }
        public string ReservedForestsqkm { get; set; }
        public string ProtectForestsqkm { get; set; }
        public string UnclassifiedForestsqkm { get; set; }
        public string ClassifiedForestsqkm { get; set; }
        public string FullyCoversqkm { get; set; }
        public string WithoutPlantsqkm { get; set; }
        public string AllocateforPlantsqkm { get; set; }
        public string PanchayatLandsqkm { get; set; }
        public string RevenueLandsqkm { get; set; }
        public string AgricultureLand { get; set; }
        public string IrregatedLandsqkm { get; set; }
        public string NonIrregatedLandsqkm { get; set; }
        public string ResidentialAreasqkm { get; set; }
        public string RemAgricultureLandsqkm { get; set; }
        public string RemNonAgricultureLandsqkm { get; set; }
        public string JFMCName { get; set; }
        public string MicroProjectName { get; set; }
        #endregion
        #region ManageProject
        public string ProjectName { get; set; }
        public string ProgramName { get; set; }
        public string SchemeName { get; set; }
        public string ProjRefDoc { get; set; }
        public string ProjDprDoc { get; set; }
        public string ProjEstimatedBudget { get; set; }
        #endregion
        #region Manage Notice Number
        public string Notice_Id { get; set; }
        public string Notice_No { get; set; }
        public string Region_Name { get; set; }
        public string Circle_Name { get; set; }
        public string Division_Name { get; set; }
        public string Range_Name { get; set; }
        public string Depot_Name { get; set; }
        public string Produce_Name { get; set; }
        public string Product_Name { get; set; }
        public string Produce_Unit { get; set; }
        public string Qty { get; set; }
        public string ReservedPrice { get; set; }
        public virtual List<Models.ForestDevelopment.DODProductDetails> DODProductList { get; set; }
        #endregion
        #region Apply for Auction
        public string NoticeNo { get; set; }
        public string Applicant_Type { get; set; }
        public string BidderName { get; set; }
        public string PlaceofAuction { get; set; }
        public string ForestProduce { get; set; }
        public string BiddingAmount { get; set; }
        public string DropOutReason { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public int Trn_Status_Code { get; set; }
        public Int64 StockQuantity { get; set; }
        public string ProduceFor { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountToBePaid { get; set; }
        public decimal FinalAmount { get; set; }
        public string AvailStatus { get; set; }
        //Bank Details Member Data
        public string DurationsFrom { get; set; }
        public string DurationsTo { get; set; }
        public string PaymentMode { get; set; }
        public string OfflinePaymentMode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string DdchkIssuesDate { get; set; }
        public string DdChkNumber { get; set; }
        public string DdchkFile { get; set; }
        public string DdchkFilepth { get; set; }
        public string EmdPaybleAmount { get; set; }
        public string PsPaybleAmount { get; set; }
        #endregion
        #region Apply for Transit Permit
        public string ReqID { get; set; }
        public string Location { get; set; }
        public string PaidAMT { get; set; }
        public string ToLocation { get; set; }
        public string TransportMode { get; set; }
        public string VehicleNo { get; set; }
        public string DriverLicense { get; set; }
        public string DriverName { get; set; }
        public string DriverMobileno { get; set; }
        public string Durations { get; set; }
        #endregion
        #region WorkOrder Invoice
        public string workorderCode { get; set; }
        public string MileStoneName { get; set; }
        public string MileStonePaymentPercentage { get; set; }
        public string BillRaised { get; set; }
        public string BillAmount { get; set; }
        public string BillDate { get; set; }
        public string BillNo { get; set; }
        #endregion
        #region Apply for Education Tour
        public string CollegeName { get; set; }
        public string CollegeAddress { get; set; }
        public string Phonenumber { get; set; }
        public string Category { get; set; }
        public string DistrictName { get; set; }
        public string LocationName { get; set; }
        public string Princeple_Name { get; set; }
        public string P_Address { get; set; }
        public string P_Gender { get; set; }
        public string P_Natinality { get; set; }
        public string ddl_MemberType { get; set; }
        public string P_MemberID { get; set; }
        public string P_MemberIDProof { get; set; }
        public string P_NumberOfMember { get; set; }
        public string MemberListFilename { get; set; }
        public string MemberListPath { get; set; }
        public string DocEducationalToueReq { get; set; }
        public string DocEducationalToueReqPath { get; set; }
        public string Vehiclecat { get; set; }
        public string ddl_vehicle { get; set; }
        public int TotalVehicle { set; get; }
        public string PlantCount { get; set; }
        #endregion
    }



}
