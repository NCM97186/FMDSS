//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : ManageAssetController
//  Description  : File contains calling functions from UI
//  Date Created : 24-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  : Ashok Yadav
//  Modified On  : 12-Feb-2016
//  Reviewed By  : Amar swain
//  Reviewed On  : 12-Feb-2016
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using System.Data;
using FMDSS.Filters;
using System.Text;
using FMDSS.Models.ForesterAction;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Configuration;
using iTextSharp.text.pdf.draw;
using FMDSS.Models.Home;
using AutoMapper;
using FMDSS.Models.ForestProduction;

namespace FMDSS.Controllers.Dashboard
{
    [MyAuthorization]
    public class dashboardController : BaseController
    {
        #region Data Members
        Int64 UserID = 0;
        int ModuleID = 1;
        CitizenDashboard _objModel = new CitizenDashboard();
        Common _objCommon = new Common();
        List<CitizenDashboard> citizenList = new List<CitizenDashboard>();
        List<CitizenDashboard> citizenList1 = new List<CitizenDashboard>();
        List<CitizenDashboard> FavouriteList = new List<CitizenDashboard>();
        #endregion
        /// <summary>
        ///  
        /// </summary>
        public class viewdetails
        {
            #region Property
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
            public string ApprovedFile { get; set; }
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
            public string Area_Size { get; set; }
            public string Division_Name { get; set; }
            public string Survey_Document { get; set; }
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
            public string Vehiclecat { get; set; }
            public string VehicleName { get; set; }
            public string PlantCount { get; set; }
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
            // public string Vehiclecat { get; set; }
            public string ddl_vehicle { get; set; }
            public int TotalVehicle { set; get; }
            #endregion
            #endregion
        }
        #region Member Functions
        /// <summary>
        /// Get all request on the  dashboard which is entered by citizen
        /// </summary>
        /// <param name="ActionID"></param>
        /// <param name="messagetype"></param>
        /// <returns></returns>
        public ActionResult dashboard(int ActionID = 2, string messagetype = "")
        {
            if (Session["CURRENT_Menus"] == null)
            {
                Home obj = new Home();
                Session["CURRENT_Menus"] = obj.GetCurrentMenus(Convert.ToInt16(Session["CURRENT_ROLE"]));
            }
            string StatementType = "GetRecords";
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (messagetype == "1")
                    {
                        ViewData["Message"] = "1";
                        ViewData["Messagetext"] = "Successfully Deleted.";
                    }
                    if (messagetype == "2")
                    {
                        ViewData["Message"] = "2";
                        ViewData["Messagetext"] = "Record Saved Successfully.Your requested ID is :" + Session["FRequestId"].ToString();
                        Session["FRequestId"] = "";
                    }
                    if (messagetype == "3")
                    {
                        ViewData["Message"] = "3";
                        ViewData["Messagetext"] = "Successfully Updated.";
                    }
                    if (messagetype == "4")
                    {
                        ViewData["Message"] = "4";
                        ViewData["Messagetext"] = Session["ResearchStatuss"];
                        Session["ResearchStatuss"] = "";
                    }
                    DataSet dtf = _objModel.GetTransactionDashaboard(UserID, 1);
                    DataSet dtf1 = _objModel.GetTransactionDashaboard(UserID, 2);
                    DataSet dtf2 = _objCommon.GetAllRecords(Convert.ToInt64(UserID), StatementType);
                    #region New
                    citizenList = new List<CitizenDashboard>();
                    if (dtf != null && dtf.Tables.Count > 0 && dtf.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dtf.Tables[0].Rows.Count; i++)
                        {
                            CitizenDashboard model = new CitizenDashboard();
                            model.RequstedID = Convert.ToString(dtf.Tables[0].Rows[i]["RequestedId"]);
                            model.RequestType = Convert.ToString(dtf.Tables[0].Rows[i]["PermissionDesc"]);
                            model.Date = Convert.ToString(dtf.Tables[0].Rows[i]["EnteredOn"]);
                            model.Status = Convert.ToString(dtf.Tables[0].Rows[i]["Status"]);
                            model.ForestStatus = Convert.ToString(dtf.Tables[0].Rows[i]["ForestStatus"]);
                            model.StatusDesc = Convert.ToString(dtf.Tables[0].Rows[i]["StatusDesc"]);
                            model.TableName = Convert.ToString(dtf.Tables[0].Rows[i]["Table_name"]);
                            model.PageURl = string.Empty;

                            if (Convert.ToString(dtf.Tables[0].Rows[i]["PermissionDesc"]) == "Online Ticketing")
                            {
                                #region Online Booking Print Ticket
                                model.PageURl = "/DownloadTickets/" + Convert.ToString(dtf.Tables[0].Rows[i]["RequestedId"]) + ".pdf";
                                #endregion
                            }
                            else
                            {

                                #region Call E-Sign PDF
                                DataSet PFDDataset = _objModel.PDFInformation("Select", model.RequstedID, model.TableName, string.Empty, string.Empty);
                                if (PFDDataset != null && PFDDataset.Tables.Count > 0 && PFDDataset.Tables[0].Rows.Count > 0)
                                {
                                    if (PFDDataset.Tables[0].Columns.Contains("PDFwithSign") && PFDDataset.Tables[0].Columns.Contains("PDFwithoutSign"))
                                        model.PageURl = !string.IsNullOrEmpty(Convert.ToString(PFDDataset.Tables[0].Rows[0]["PDFwithSign"])) ? Convert.ToString(PFDDataset.Tables[0].Rows[0]["PDFwithSign"]) : Convert.ToString(PFDDataset.Tables[0].Rows[0]["PDFwithoutSign"]);
                                    if (!string.IsNullOrEmpty(model.PageURl))
                                        model.PageURl = model.PageURl.Substring(1, model.PageURl.Length - 1);
                                }
                                #endregion
                            }
                            citizenList.Add(model);
                        }
                    }
                    #endregion
                    #region OLD
                    //for (int i = 0; i < dtf.Tables.Count; i++)
                    //{
                    //    foreach (DataRow dr in dtf.Tables[0].Rows)
                    //        citizenList.Add(
                    //            new CitizenDashboard()
                    //            {
                    //                RequstedID = dr["RequestedId"].ToString(),
                    //                RequestType = dr["PermissionDesc"].ToString(),
                    //                Date = dr["EnteredOn"].ToString(),
                    //                Status = dr["Status"].ToString(),
                    //                StatusDesc = dr["StatusDesc"].ToString(),
                    //                TableName = dr["Table_name"].ToString(),
                    //            });
                    //}
                    #endregion
                    //for (int i = 0; i < dtf.Tables.Count; i++)
                    //{
                    //    foreach (DataRow dr in dtf.Tables[0].Rows)
                    //        citizenList.Add(
                    //            new CitizenDashboard()
                    //            {
                    //                RequstedID = dr["RequestedId"].ToString(),
                    //                RequestType = dr["PermissionDesc"].ToString(),
                    //                Date = dr["EnteredOn"].ToString(),
                    //                Status = dr["Status"].ToString(),
                    //                StatusDesc = dr["StatusDesc"].ToString(),
                    //                TableName = dr["Table_name"].ToString(),
                    //                PageURl= _objModel.PDFInformation("Select", dr["RequestedId"].ToString(), dr["Table_name"].ToString())
                    //            });
                    //}
                    for (int i = 0; i < dtf1.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf1.Tables[0].Rows)
                            citizenList1.Add(
                                new CitizenDashboard()
                                {
                                    RequstedID = dr["RequestedId"].ToString(),
                                    RequestType = dr["PermissionDesc"].ToString(),
                                    Date = dr["EnteredOn"].ToString(),
                                    Status = dr["Status"].ToString(),
                                    
                                    StatusDesc = dr["StatusDesc"].ToString(),
                                    TableName = dr["Table_name"].ToString(),
                                    PermissionID = dr["PermissionId"].ToString()
                                    // ActionID = dr["Action"].ToString()
                                });
                    }
                    for (int i = 0; i < dtf2.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf2.Tables[i].Rows)
                            FavouriteList.Add(
                                new CitizenDashboard()
                                {
                                    ModuleName = dr["ModuleDesc"].ToString(),
                                    UserName = dr["Name"].ToString(),
                                    PageURl = dr["URL"].ToString(),
                                    PageName = dr["PageName"].ToString(),
                                    FavouritelinkID = Convert.ToInt32(dr["ID"].ToString())
                                });
                    }
                    ViewData["citizenList"] = citizenList;
                    ViewData["citizenList1"] = citizenList1;
                    ViewData["FavouriteList"] = FavouriteList;
                    if (Session["KioskCtznName"] == null)
                        ViewBag.UserName = Convert.ToString(Session["User"]);
                    else
                        ViewBag.UserName = Convert.ToString(Session["KioskCtznName"]);

                    if (Session["returnURL"] != null)
                    {
                        try
                        {
                            string strURL = Session["returnURL"].ToString();
                            if (strURL == "" || strURL == null)
                            {
                                return View();
                            }
                            else
                            {
                                Session["returnURL"] = null;
                                Response.Redirect(strURL, false);
                            }
                        }
                        catch
                        {
                            HttpResponse Response = System.Web.HttpContext.Current.Response; // HttpContext.Current.Response;
                            Response.StatusCode = 301;
                            Response.StatusDescription = "Moved Permanently";
                            Response.RedirectLocation = "dashboard.cshtml";
                            Response.Flush();
                            return View();
                        }
                    }

                    return View();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        /// <summary>
        /// By this function we get all the details of a request.
        /// </summary>
        /// <param name="RequestID"></param>
        /// <returns></returns>
        [HttpPost]
         
        public JsonResult GetTransDetails(string RequestID)
        {
            DataSet ds = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    ds = _objModel.GetPrintDetails(RequestID);
                    if (ds != null)
                    {
                        _objModel.Final_Amount = ds.Tables[0].Rows[0]["Final_Amount"].ToString();
                        _objModel.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                        _objModel.Date = ds.Tables[0].Rows[0]["Date"].ToString();
                        _objModel.ApprovedDate = ds.Tables[0].Rows[0]["Approved Date"].ToString();
                        _objModel.ApprovedStatus = ds.Tables[0].Rows[0]["Approved Status"].ToString();
                        _objModel.ApprovedID = ds.Tables[0].Rows[0]["Approved ID"].ToString();
                        _objModel.ReviewedDate = ds.Tables[0].Rows[0]["Reviewed Date"].ToString();
                        _objModel.ReviewedStatus = ds.Tables[0].Rows[0]["Reviewed Status"].ToString();
                        _objModel.ReviewedID = ds.Tables[0].Rows[0]["Reviewed ID"].ToString();
                    }
                    return Json(new
                    {
                        Final_Amount = _objModel.Final_Amount,
                        Status = _objModel.Status,
                        Date = _objModel.Date,
                        ApprovedDate = _objModel.ApprovedDate,
                        ApprovedStatus = _objModel.ApprovedStatus,
                        ApprovedID = _objModel.ApprovedID,
                        ReviewedDate = _objModel.ReviewedDate,
                        ReviewedStatus = _objModel.ReviewedStatus,
                        ReviewedID = _objModel.ReviewedID
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        /// <summary>
        /// description: For Show view on requestid
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpPost]
         
        public ActionResult ViewDetails(string RequestId, string TableName)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    #region Details
                    ActionRequest ar = new ActionRequest();
                    CitizenDashboard cd = new CitizenDashboard();
                    viewdetails vd = new viewdetails();
                    DataSet dsMultDist = new DataSet();
                    DataTable dtdata = new DataTable();
                    dtdata = cd.BindActionList(RequestId, TableName);
                    vd.TableName = TableName;
                    vd.RequestId = RequestId;
                    vd.ModuleName = dtdata.Rows[0]["ModuleDesc"].ToString();
                    vd.ServiceType = dtdata.Rows[0]["ServiceTypeDesc"].ToString();
                    vd.PermissionType = dtdata.Rows[0]["PermissionDesc"].ToString();
                    vd.PermissionName = dtdata.Rows[0]["SubPermissionDesc"].ToString();
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
                    if (dtdata.Columns.Contains("trn_Status_Code"))
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
                        vd.IdProofUrl = strproofsplit[strproofsplit.Length - 1];
                    }
                    else if (dtdata.Columns.Contains("IDProof_Path"))
                    {
                        string strproof = dtdata.Rows[0]["IDProof_Path"].ToString();
                        string[] strproofsplit = strproof.Split('/');
                        vd.IdProofUrl = strproofsplit[strproofsplit.Length - 1];
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
                    if (dsMultDist.Tables.Count > 0)
                    {
                        if (dsMultDist.Tables[0].Rows.Count > 0)
                        {
                            if (dsMultDist.Tables[0].Columns.Contains("DIV_NAME"))
                            {
                                if (dsMultDist.Tables.Count > 0)
                                {
                                    vd.Division_Name = dsMultDist.Tables[0].Rows[0][6].ToString();
                                }
                                else { vd.Division_Name = "N/A"; }
                            }
                            else { vd.Division_Name = "N/A"; }
                        }
                        else { vd.Division_Name = "N/A"; }
                    }
                    else { vd.Division_Name = "N/A"; }
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
                                    if (sbMultDist.ToString().Length > 0)
                                    {
                                        vd.District = sbMultDist.ToString().Remove(sbMultDist.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        vd.District = "N/A";
                                    }
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
                        vd.District = "N/A";
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
                                        if (sbMultArea.ToString().Length > 0)
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
                                    if (sbMultBlk.ToString().Length > 0)
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
                                    if (sbMultGp.ToString().Length > 0)
                                    {
                                        vd.GramPanchayat = sbMultGp.ToString().Remove(sbMultGp.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        vd.GramPanchayat = "N/A";
                                    }
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
                                    if (sbMultVill.ToString().Length > 0)
                                    {
                                        vd.Village = sbMultVill.ToString().Remove(sbMultVill.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        vd.Village = "N/A";
                                    }
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
                                    if (sbMultKhasra.ToString().Length > 0)
                                    {
                                        vd.khasraNo = sbMultKhasra.ToString().Remove(sbMultKhasra.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        vd.khasraNo = "N/A";
                                    }
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
                    if (dtdata.Columns.Contains("Remarks"))
                    {
                        vd.Remarks = dtdata.Rows[0]["Remarks"].ToString();
                        if (vd.Remarks == null || vd.Remarks == "")
                        {
                            vd.Remarks = "N/A";
                        }
                        //if (dtdata.Rows[0]["Desig_Alias"].ToString().ToUpper() == "CITIZEN")
                        //{
                        //    vd.Remarks = "N/A";
                        //}
                    }
                    else { vd.Remarks = "N/A"; }
                    if (dtdata.Columns.Contains("StatusDesc"))
                    {
                        vd.StatusDesc = dtdata.Rows[0]["StatusDesc"].ToString();
                        if (vd.StatusDesc == null || vd.StatusDesc == "")
                        {
                            vd.StatusDesc = "N/A";
                        }
                    }
                    else { vd.StatusDesc = "N/A"; }
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
                        //string strkml = dtdata.Rows[0]["KML_Path"].ToString();
                        //string[] strkmlsplit = strkml.Split('/');
                        //vd.KmlPath =  "http://10.68.107.144/fmdssintegration/shapeFile/"+strkmlsplit[strkmlsplit.Length - 1];
                        vd.KmlPath = "http://" + dtdata.Rows[0]["KML_Path"].ToString();
                        Session["KmlPath"] = vd.KmlPath;
                    }
                    else { vd.KmlPath = "N/A"; }
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
                    if (dtdata.Columns.Contains("Area_Size"))
                    {
                        vd.Area_Size = dtdata.Rows[0]["Area_Size"].ToString();
                        if (vd.Area_Size == null || vd.Area_Size == "")
                        {
                            vd.Area_Size = "N/A";
                        }
                    }
                    else { vd.Area_Size = "N/A"; }
                    if (dtdata.Columns.Contains("Survey_Document"))
                    {
                        string strSurvey_Doc = dtdata.Rows[0]["Survey_Document"].ToString();
                        string[] strdocsplit = strSurvey_Doc.Split('/');
                        vd.Survey_Document = strdocsplit[strdocsplit.Length - 1];
                    }
                    if (dtdata.Columns.Contains("PlantCount"))
                    {
                        vd.PlantCount = dtdata.Rows[0]["PlantCount"].ToString();
                    }
                    else { vd.PlantCount = "0"; }
                    if (dtdata.Columns.Contains("IsGTSheetAvaliable"))
                    {
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
                            vd.P_Address = "N/A";
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
                    #region Research Study Permission
                    if (vd.TableName == "tbl_ResearchStudyPermissions")
                    {
                        Models.CitizenService.ProductionServices.EducationService.Research research = new Models.CitizenService.ProductionServices.EducationService.Research();
                        var dsReq = research.GetResearchStudyData(RequestId);
                        if (dsReq != null && dsReq.Tables[0].Rows.Count > 0)
                        {
                            research = Globals.Util.GetListFromTable<Models.CitizenService.ProductionServices.EducationService.Research>(dsReq, 0).FirstOrDefault();
                            research.SpecimenDetailsList = Globals.Util.GetListFromTable<Models.CitizenService.ProductionServices.EducationService.SpecimenDetailsModel>(dsReq, 1);
                            research.SampleDetailsList = Globals.Util.GetListFromTable<Models.CitizenService.ProductionServices.EducationService.SampleDetailsModel>(dsReq, 2);
                            Mapper.CreateMap<viewdetails, Entity.ViewModel.CommonRequestData>();
                            research.CommonRequestData = Mapper.Map<viewdetails, Entity.ViewModel.CommonRequestData>(vd);
                        }
                        //return Json(new {commonData=vd, research = research }, JsonRequestBehavior.AllowGet);
                        return PartialView("_ResearchStudyDetails", research);//Needs work
                        #region Commented by dipak
                        //if (dtdata.Columns.Contains("Qualification"))
                        //{
                        //    vd.Qualification = dtdata.Rows[0]["Qualification"].ToString();
                        //    if (vd.Qualification == null || vd.Qualification == "")
                        //    {
                        //        vd.Qualification = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.Qualification = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("College"))
                        //{
                        //    vd.College = dtdata.Rows[0]["College"].ToString();
                        //    if (vd.College == null || vd.College == "")
                        //    {
                        //        vd.College = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.College = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("R_Subject"))
                        //{
                        //    vd.ResearchSubject = dtdata.Rows[0]["R_Subject"].ToString();
                        //    if (vd.ResearchSubject == null || vd.ResearchSubject == "")
                        //    {
                        //        vd.ResearchSubject = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.ResearchSubject = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("R_Procedure"))
                        //{
                        //    vd.ResearchProcedure = dtdata.Rows[0]["R_Procedure"].ToString();
                        //    if (vd.ResearchProcedure == null || vd.ResearchProcedure == "")
                        //    {
                        //        vd.ResearchProcedure = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.ResearchProcedure = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("AnimalCategory"))
                        //{
                        //    vd.AnimalCategory = dtdata.Rows[0]["AnimalCategory"].ToString();
                        //}
                        //else
                        //{
                        //    vd.AnimalCategory = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("AnimalName"))
                        //{
                        //    vd.AnimalName = dtdata.Rows[0]["AnimalName"].ToString();
                        //}
                        //else
                        //{
                        //    vd.AnimalName = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("SpeciesCategory"))
                        //{
                        //    vd.SpeciesCategory = dtdata.Rows[0]["SpeciesCategory"].ToString();
                        //    if (vd.SpeciesCategory == null || vd.SpeciesCategory == "")
                        //    {
                        //        vd.SpeciesCategory = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.SpeciesCategory = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("SpeciesName"))
                        //{
                        //    vd.SpeciesName = dtdata.Rows[0]["SpeciesName"].ToString();
                        //    if (vd.SpeciesName == null || vd.SpeciesName == "")
                        //    {
                        //        vd.SpeciesName = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.SpeciesName = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("Animal_Sno"))
                        //{
                        //    vd.Animal_SerialNo = dtdata.Rows[0]["Animal_Sno"].ToString();
                        //    if (vd.Animal_SerialNo == null || vd.Animal_SerialNo == "")
                        //    {
                        //        vd.Animal_SerialNo = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.Animal_SerialNo = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("Species_Sno"))
                        //{
                        //    vd.Species_SerialNo = dtdata.Rows[0]["Species_Sno"].ToString();
                        //    if (vd.Species_SerialNo == null || vd.Species_SerialNo == "")
                        //    {
                        //        vd.Species_SerialNo = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.Species_SerialNo = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("Benefits"))
                        //{
                        //    vd.Benefits = dtdata.Rows[0]["Benefits"].ToString();
                        //    if (vd.Benefits == null || vd.Benefits == "")
                        //    {
                        //        vd.Benefits = "N/A";
                        //    }
                        //}
                        //else
                        //{
                        //    vd.Benefits = "N/A";
                        //}
                        //if (dtdata.Columns.Contains("CoordinatorId"))
                        //{
                        //    vd.CoordinatorId = dtdata.Rows[0]["CoordinatorId"].ToString();
                        //    if (vd.CoordinatorId == null || vd.CoordinatorId == "")
                        //    {
                        //        vd.CoordinatorId = "N/A";
                        //    }
                        //}
                        //else
                        //    vd.CoordinatorId = "N/A";
                        //// added more fields in Research Study for the changes came on 06-Aug-2016
                        //vd.SynopsisPath = Convert.ToString(dtdata.Rows[0]["Synopsis_Name"]);
                        //if (vd.SynopsisPath == "")
                        //    vd.SynopsisPath = "N/A";
                        //vd.PresentationPath = Convert.ToString(dtdata.Rows[0]["Presentation_Name"]);
                        //if (vd.PresentationPath == "")
                        //    vd.PresentationPath = "N/A";
                        //vd.AssistName = Convert.ToString(dtdata.Rows[0]["Assist_Name"]);
                        //if (vd.AssistName == "")
                        //    vd.AssistName = "N/A";
                        //vd.AssistIdProofPath = Convert.ToString(dtdata.Rows[0]["Assist_IdProof_Name"]);
                        //if (vd.AssistIdProofPath == "")
                        //    vd.AssistIdProofPath = "N/A";
                        //vd.Vehiclecat = Convert.ToString(dtdata.Rows[0]["VehicleCat"]);
                        //if (vd.Vehiclecat == "")
                        //    vd.Vehiclecat = "N/A";
                        //vd.VehicleName = Convert.ToString(dtdata.Rows[0]["Vehiclename"]);
                        //if (vd.VehicleName == "")
                        //    vd.VehicleName = "N/A";
                        #endregion
                    }
                    #endregion
                    return Json(vd, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                return null;
            }
            return null;
        }
        /// <summary>
        /// To download KML files
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadKMLFile()
        {
            try
            {
                CitizenDashboard CD = new CitizenDashboard();
                string FileName = Session["KmlPath"].ToString();
                bool forceDownload = true;
                //string FileName1 = "~//FixedLandDocument//" + FileName;
                string ReturnUrl = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
                string FileName1 = ReturnUrl + FileName;
                WebRequest serverRequest = WebRequest.Create(FileName1);
                WebResponse serverResponse;
                try //Try to get response from server  
                {
                    serverResponse = serverRequest.GetResponse();
                }
                catch //If could not obtain any response  
                {
                    return null;
                }
                serverResponse.Close();
                //FileInfo fi = new FileInfo(Server.MapPath(FileName1));
                //if (System.IO.File.Exists(FileName1))
                //{
                //    string path = Server.MapPath(FileName1);
                //    string name = Path.GetFileName(path);
                //    string ext = Path.GetExtension(path);
                //    string type = "";
                //    if (ext != null)
                //    {
                //        switch (ext.ToLower())
                //        {
                //            case ".htm":
                //            case ".html":
                //                type = "text/HTML";
                //                break;
                //            case ".txt":
                //                type = "text/plain";
                //                break;
                //            case ".kml":
                //                type = "Application/kml";
                //                break;
                //            case ".doc":
                //            case ".rtf":
                //                type = "Application/msword";
                //                break;
                //        }
                //    }
                //    if (forceDownload)
                //    {
                //        Response.AppendHeader("content-disposition",
                //            "attachment; filename=" + name);
                //    }
                //    if (type != "")
                //        Response.ContentType = type;
                //    Response.WriteFile(path);
                //    Response.End();
                return RedirectToAction("dashboard");
                //}
                //else
                //{
                //    TempData["kml"] = "File not exists";
                //    return RedirectToAction("dashboard", "dashboard");
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, null + "_" + null, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }
        /// <summary>
        /// To add Favourite in database
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ServiceDesc"></param>
        /// <returns></returns>
        [HttpPost]

        public JsonResult AddFavourite(string ServiceId, string ServiceDesc)
        {
            try
            {
                CitizenDashboard CD = new CitizenDashboard();
                DataTable dt = new DataTable();
                List<SelectListItem> lst = new List<SelectListItem>();
                dt = CD.FavouriteList(ServiceId, ServiceDesc);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lst.Add(new SelectListItem { Text = dt.Rows[i][0].ToString(), Value = dt.Rows[i][0].ToString() });
                    }
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, null + "_" + null, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }
        /// <summary>
        /// Get added Favourite services in Grid
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult GetAddFavourite()
        {
            try
            {
                CitizenDashboard CD = new CitizenDashboard();
                DataTable dt = new DataTable();
                List<FavouritList> lst = new List<FavouritList>();
                dt = CD.GetFavouriteList();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        lst.Add(new FavouritList { FavItem = dt.Rows[i][0].ToString(), Controller = dt.Rows[i][1].ToString(), Action = dt.Rows[i][2].ToString(), ServiceId = dt.Rows[i][3].ToString() });
                    }
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, null + "_" + null, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }
        /// <summary>
        /// To delete Favourite services
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <returns></returns>
        [HttpPost]
        
        public JsonResult DelFavourite(string ServiceId)
        {
            try
            {
                CitizenDashboard CD = new CitizenDashboard();
                DataTable dt = new DataTable();
                dt = CD.DelFavouriteList(ServiceId);
                if (dt.Rows.Count > 0)
                {
                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NF", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, null + "_" + null, ModuleID, DateTime.Now, UserID);
                return null;
            }
        }
        /// <summary>
        /// Action of citizen is used to get all transcation which is approved or reviewed
        /// </summary>
        /// <returns></returns>
        public ActionResult CitizenAction()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    DataSet dtf1 = _objModel.GetTransactionDashaboard(UserID, 2);
                    for (int i = 0; i < dtf1.Tables.Count; i++)
                    {
                        foreach (DataRow dr in dtf1.Tables[i].Rows)
                            citizenList1.Add(
                                new CitizenDashboard()
                                {
                                    RequstedID = dr["RequestedId"].ToString(),
                                    RequestType = dr["PermissionDesc"].ToString(),
                                    Date = dr["EnteredOn"].ToString(),
                                    StatusDesc = dr["StatusDesc"].ToString(),
                                    TableName = dr["Table_name"].ToString(),
                                    PermissionID = dr["PermissionId"].ToString()
                                });
                    }
                    ViewData["citizenList1"] = citizenList1;
                    return View();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        /// <summary>
        /// To save Favourite links
        /// </summary>
        /// <param name="PageUrl"></param>
        /// <param name="PageName"></param>
        /// <returns></returns>
        public JsonResult SaveFavouritelink(string PageUrl, string PageName)
        {
            Common _objcommon = new Common();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    #region Details
                    string StatementType = "Insert";
                    Int64 UserId = Convert.ToInt64(Session["UserId"]);
                    int IsActive = 1;
                    int ModuleId = 1;
                    bool status = false;
                    Int64 id = _objcommon.SaveFavouritelink(UserId, PageUrl, PageName, IsActive, ModuleId, StatementType);
                    if (id > 0)
                    {
                        status = true;
                    }
                    #endregion
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        /// <summary>
        /// To dlete Favourite links
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult DeleteFavouritelink(string ID)
        {
            Common _objcommon = new Common();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    string StatementType = "Delete";
                    int IsActive = 0;
                    int id = _objcommon.DeleteFavouritelink(Convert.ToInt32(ID), StatementType, IsActive);
                    if (id > 0)
                    {
                        return RedirectToAction("dashboard", "dashboard", new { messagetype = "1" });
                    }
                    else
                    {
                        return null; ;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }
        /// <summary>
        /// Converting   Kasra in xml
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public string GetKasraNo(string lang)
        {
            var doc = new XmlDocument();
            doc.LoadXml(lang.ToString());
            XmlNodeList xnList = doc.SelectNodes("/KhasraRoot/KhasraValue");
            StringBuilder sb = new StringBuilder();
            List<String> list = new List<String>();
            try
            {
                for (int i = 0; i < xnList.Count; i++)
                {
                    string values = xnList[i].InnerText;
                    sb.Append(values + ",");
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, null + "_" + null, ModuleID, DateTime.Now, UserID);
            }
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// user to print the request details
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Status"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        [HttpPost]
         
        public string PrintAfterApproval(string RequestId, string Status, string TableName)
        {
            DataSet ds = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            StringBuilder sb = new StringBuilder();
            try
            {
                if (Session["UserID"] != null)
                {
                    ds = _objModel.GetPrintApplicationAfterApproval(RequestId, Status, TableName);
                    DataTable DT1 = new DataTable();
                    DataTable DT2 = new DataTable();
                    sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4'></div> <div class='col-xs-12 col-sm-4' style='padding: 0'><img src='../images/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' >  </div></div>");
                    if (ds != null)
                    {
                        if (TableName == "tbl_FPD_OnlinePurchaseDetails")
                        {
                            ProducePurchase obj_pp = new ProducePurchase();
                            sb.Append(obj_pp.PrintOrder(RequestId, UserID, Convert.ToBoolean(Session["NurseryIncharge"])));
                        }

                        else if (TableName == "tbl_FixedPermissions")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b>Division Name</b></td><td col-lg-3><b>District Name</b></td><td col-lg-3><b>Block Name</b></td><td col-lg-3><b>GP Name</b></td><td col-lg-3><b> Village Name</b></td><td col-lg-3><b>Kasra No</b></td><td col-lg-3><b>Area</b></td></tr>");
                            string KasraNo = "";
                            for (int i = 0; i < DT2.Rows.Count; i++)
                            {
                                if (DT2.Rows[i]["KhasraNo"] != "")
                                {
                                    KasraNo = DT2.Rows[i]["KhasraNo"].ToString();
                                }
                                else
                                {
                                    KasraNo = "";
                                }
                                sb.Append("<tr><td col-lg-3>" + DT2.Rows[i]["DIV_NAME"] + "</td><td col-lg-3>" + DT2.Rows[i]["DIST_NAME"] + "</td><td col-lg-3>" + DT2.Rows[i]["BLK_NAME"] + "</td><td col-lg-3>" + DT2.Rows[i]["GP_NAME"] + "</td><td col-lg-3>" + DT2.Rows[i]["VILL_NAME"] + "</td><td col-lg-3>" + KasraNo + "</td><td col-lg-3>" + DT2.Rows[i]["Area"] + "</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("<BR/>");
                            sb.Append("<BR/>");
                            string ApplicantType = "";
                            if (DT1.Rows[0]["ApplicantType"].ToString() == "1")
                            {
                                ApplicantType = "Individual";
                            }
                            else
                            {
                                ApplicantType = "Organization";
                            }
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b>Requested ID: </b></td><td col-lg-3>" + DT1.Rows[0]["RequestedID"] + "</td><td col-lg-3><b>Applicant Type:</b> </td><td col-lg-3>" + ApplicantType + " </td></tr>");
                            sb.Append("<tr><td col-lg-12 colspan='4'><label><h5><b>Application Details:</b></h5></label><div class='divider'></div></td></tr>");
                            sb.Append("<tr><td col-lg-3><b>From Date: </b></td><td col-lg-3>" + DT1.Rows[0]["From Date"] + "</td><td col-lg-3><b>To Date:</b> </td><td col-lg-3>" + DT1.Rows[0]["To Date"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Permission For: </b></td><td col-lg-3>" + DT1.Rows[0]["Permission For"] + "</td><td col-lg-3></td><td col-lg-3></td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Amount: </b></td><td col-lg-3>" + DT1.Rows[0]["Amount"] + "</td><td col-lg-3><b>Discount:</b> </td><td col-lg-3>" + DT1.Rows[0]["Discount"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Final Amount: </b></td><td col-lg-3>" + DT1.Rows[0]["Final Amount"] + "</td><td col-lg-3><b> </b> </td><td col-lg-3> </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Latitude: </b></td><td col-lg-3>" + DT1.Rows[0]["Latitude"] + "</td><td col-lg-3><b>Longitude:</b> </td><td col-lg-3>" + DT1.Rows[0]["Longitude"] + " </td></tr>");
                            if (DT1.Rows[0]["P_ID"].ToString() == "9")
                            {
                                sb.Append("<tr><td col-lg-3><b>Sawmill Type: </b></td><td col-lg-3>" + DT1.Rows[0]["Sawmill Type"] + "</td><td col-lg-3><b>Sawmill Size:</b></td><td col-lg-3>" + DT1.Rows[0]["Sawmill Size"] + " </td></tr>");
                            }
                            if (DT1.Rows[0]["P_ID"].ToString() == "4")
                            {
                                sb.Append("<tr><td col-lg-3><b>Nearest Water Source: </b></td><td col-lg-3>" + DT1.Rows[0]["Nearest Water Source"] + "</td><td col-lg-3> </td><td col-lg-3> </td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Water Source Distance: </b></td><td col-lg-3>" + DT1.Rows[0]["Water Source Distance"] + "</td><td col-lg-3><b>Distance From Nearest Forest boundary(KM)</b>: </td><td col-lg-3>" + DT1.Rows[0]["Distance From Nearest Forest boundary(KM)"] + " </td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Distance From" + "<br/>" + " Wildlife Area(KM.):</b> </td><td col-lg-3>" + DT1.Rows[0]["Distance From Wildlife Area(KM.)"] + "</td><td col-lg-3><b>Number of Trees:</b> </td><td col-lg-3>" + DT1.Rows[0]["Number of Trees"] + " </td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Mining Area Falls in Aravali Hills  </b></td><td col-lg-3>" + DT1.Rows[0]["Mining Area Falls in Aravali Hills"] + "</td><td col-lg-3><b>Mining area is a Part of Forest Land:</b> </td><td col-lg-3>" + DT1.Rows[0]["Mining area is a Part of Forest Land"] + " </td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Mining Area Falls in Plantation Area:</b> </td><td col-lg-3>" + DT1.Rows[0]["Mining Area Falls in Plantation Area"] + "</td><td col-lg-3>  </td><td col-lg-3>  </td></tr>");
                            }
                            sb.Append("<tr><td col-lg-3><b>Area(In sqm.): </b></td><td col-lg-3>" + DT1.Rows[0]["Area(In sqm.)"] + "</td><td col-lg-3><b></b> </td><td col-lg-3> </td></tr>");
                            sb.Append("<tr><td col-lg-3> <b>Application Date: </b></td><td col-lg-3>" + DT1.Rows[0]["Application Date"] + "</td><td col-lg-3> Status:  </b> </td><td col-lg-3> " + DT1.Rows[0]["Status"] + " </td></tr>");
                            if (Status != "1")
                            {
                                sb.Append("<tr><td col-lg-3><b>Action Taken By:</b> </td><td col-lg-3>" + DT1.Rows[0]["Name"] + "</td><td col-lg-3><b>Action Taken On: </b></td><td col-lg-3>" + DT1.Rows[0]["Action Taken On"] + " </td></tr>");
                                if (Status != "2")
                                {
                                    sb.Append("<tr><td col-lg-3><b>Remarks:</b> </td><td col-lg-3>" + DT1.Rows[0]["Remarks"] + "</td><td col-lg-3><b> ActionReason: </b></td><td col-lg-3>" + DT1.Rows[0]["ActionReason"] + " </td></tr>");
                                }
                            }
                            sb.Append("</table> </section>");
                        }
                        else if (TableName == "tbl_FPM_Parivad_Details")
                        {
                            DT1 = ds.Tables[0];
                            if (DT1.Rows.Count > 0)
                            {
                                sb.Append("<table class='table table-bordered' id='tkt'>");
                                sb.Append("<BR/>");
                                sb.Append("<BR/>");
                                sb.Append("<tr><td col-lg-3><b> Offense ID: </b></td><td col-lg-3>" + DT1.Rows[0]["RequestedID"] + "</td><td col-lg-3> <b>  Service Name: </b></td><td col-lg-3>Parivad </td></tr>");
                                sb.Append("</table>");
                                sb.Append("<table>");
                                sb.Append("<BR/>");
                                sb.Append("<BR/>");
                                sb.Append("<table class='table table-bordered' id='tkt'>");
                                sb.Append("<tr><td col-lg-12 colspan='4'><label><h6><b>Offense Details: </b></h6></label><div class='divider'></div></td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Category: </b></td><td col-lg-3>" + DT1.Rows[0]["FOCategory"] + "</td><td col-lg-3><b>Date of Offense :</b> </td><td col-lg-3>" + DT1.Rows[0]["DateOfOffense"] + " </td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Time of Offense :</b></td><td col-lg-3>" + DT1.Rows[0]["TimeOfOffense"] + "</td><td col-lg-3><b>Offense Place: </b></td><td col-lg-3>" + DT1.Rows[0]["OffensePlace"] + "</td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Description: </b></td><td col-lg-3>" + DT1.Rows[0]["description"] + "</td><td col-lg-3><b>District Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["DIST_NAME"] + "</td></tr>");
                                sb.Append("<tr><td col-lg-3><b>Application Date:</b> </td><td col-lg-3>" + DT1.Rows[0]["EnteredOn"] + "</td><td col-lg-3><b>Status:</b>   </td><td col-lg-3> " + DT1.Rows[0]["StatusDesc"] + " </td></tr>");
                                sb.Append("</table>");
                            }
                        }
                        else if (TableName == "tbl_FilmShootingPermissions")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3>Requested ID: </td><td col-lg-3>" + DT1.Rows[0]["RequestedID"] + "</td><td col-lg-3>   Permission For: </td><td col-lg-3>Film Shooting </td></tr>");
                            sb.Append("</table>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-12 colspan='4'><label><h6><b>Application Details:</b></h6></label><div class='divider'></div></td></tr>");
                            sb.Append("<tr><td col-lg-3><b>From Date:</b> </td><td col-lg-3>" + DT1.Rows[0]["DurationFrom"] + "</td><td col-lg-3><b>To Date:</b> </td><td col-lg-3>" + DT1.Rows[0]["DurationTo"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Film Title:</b></td><td col-lg-3>" + DT1.Rows[0]["Title"] + "</td><td col-lg-3><b>Description: </b></td><td col-lg-3>" + DT1.Rows[0]["Description"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Number Of Guest/Visitor:</b> </td><td col-lg-3>" + DT1.Rows[0]["NumberOfCrewMembers"] + "</td><td col-lg-3><b>Shooting Purpose: </b></td><td col-lg-3> " + DT1.Rows[0]["ShootingPurpose"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b>No of Indian Citizen:</b> </td><td col-lg-3>" + DT1.Rows[0]["IndianCitizen"] + "</td><td col-lg-3><b>No of non Indian Citizen</b></td><td col-lg-3>" + DT1.Rows[0]["NonIndianCitizen"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>No of Students:</b> </td><td col-lg-3>" + DT1.Rows[0]["Students"] + "</td><td col-lg-3><b>Total Fees: </b></td><td col-lg-3>" + DT1.Rows[0]["TotalFees"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Emitra Transaction ID:</b> </td><td col-lg-3>" + DT1.Rows[0]["EmitraTransactionID"] + "</td><td col-lg-3></b></td><td col-lg-3> </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Total Fees:</b> </td><td col-lg-3>" + DT1.Rows[0]["TotalFees"] + "</td><td col-lg-3><b>Deposite Amount:</b></td><td col-lg-3>" + DT1.Rows[0]["DepositeAmount"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>District Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["DIST_NAME"] + "</td><td col-lg-3><b>Place Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["PlaceName"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Application Date:</b> </td><td col-lg-3>" + DT1.Rows[0]["EnteredOn"] + "</td><td col-lg-3><b>Status:</b></td><td col-lg-3> " + DT1.Rows[0]["StatusDesc"] + " </td></tr>");
                            if (Status != "1")
                            {
                                sb.Append("<tr><td col-lg-3><b>Action Taken By:</b> </td><td col-lg-3>" + DT1.Rows[0]["Name"] + "</td><td col-lg-3><b>Action Taken On:</b> </td><td col-lg-3>" + DT1.Rows[0]["ActionTakenOn"] + " </td></tr>");
                                if (Status != "2")
                                {
                                    sb.Append("<tr><td col-lg-3><b>Remarks:</b></td><td col-lg-3>" + DT1.Rows[0]["Remarks"] + "</td><td col-lg-3><b>ActionReason:</b></td><td col-lg-3>" + DT1.Rows[0]["ActionReason"] + " </td></tr>");
                                }
                            }
                            sb.Append("</table>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b>Name</b></td><td col-lg-3><b>Address</b></td><td col-lg-3><b>Pin Code</b></td><td col-lg-3> <b>Gender</b></td><td col-lg-3> <b> Document</b></td><td col-lg-3> <b> Document No</b></td><td col-lg-3><b>Nationality</b></td><td col-lg-3><b>Member Type</b></td></tr>");
                            for (int i = 0; i < DT2.Rows.Count; i++)
                            {
                                sb.Append("<tr><td col-lg-3>" + DT2.Rows[i]["Name"] + "</td><td col-lg-3>" + DT2.Rows[i]["Address1"] + "</td><td col-lg-3>" + DT2.Rows[i]["Postal_Code"] + "</td><td col-lg-3>" + DT2.Rows[i]["Gender"] + "</td><td col-lg-3>" + DT2.Rows[i]["IDType"] + "</td><td col-lg-3>" + DT2.Rows[i]["IDNo"] + "</td><td col-lg-3>" + DT2.Rows[i]["Nationality"] + "</td><td col-lg-3>" + DT2.Rows[i]["MemberType"] + "</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("<b style='fontsize:14px' >Instruction</b>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td>1. Bookings  are to be cross checked by  the Sanctuary/ Park authorities  at the time of entry.</td></tr>");
                            sb.Append("<tr><td>2. The visitor must reach to the forest booking center along with valid original photo ID mentioned at the time of booking, at least 45 minutes prior to the entrance time. In case, same Id proof is not  produced at the time entry in the park,   the permission shall be deemed cancelled or fake.</td></tr>");
                            sb.Append("<tr><td>3. The park/sanctuary authority  reserves the right to  cancel the  booking at any time due to unavoidable circumstances/ emergency.</td></tr>");
                            sb.Append("<tr><td>4. Damage to the  property or plants or tree or animal of park/sanctuary  booking will be cancelled by park/sanctuary authority without refund and penalty  will be imposed as per rules.</td></tr>");
                            sb.Append("<tr><td>5. Additional equipment/ video camera/ vehicle other than indicated in the  booking will not be allowed.</td></tr>");
                            sb.Append("<tr><td>6. Visitors should carry   two copies of this voucher for entire duration of camping/ Film Shooting.</td></tr>");
                            sb.Append("</table>");
                        }
                        else if (TableName == "tbl_OrganisingCamp")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b> Requested ID: </b></td><td col-lg-3>" + DT1.Rows[0]["RequestedID"] + "</td><td col-lg-3> <b>  Permission For: </b></td><td col-lg-3>Organizing Camp </td></tr>");
                            sb.Append("</table>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-12 colspan='4'><label><h6><b></b></h6></label><div class='divider'></div></td></tr>");
                            sb.Append("<tr><td col-lg-3><b>From Date: </b></td><td col-lg-3>" + DT1.Rows[0]["From Date"] + "</td><td col-lg-3><b>To Date :</b> </td><td col-lg-3>" + DT1.Rows[0]["To Date"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Purpose of Camp:</b></td><td col-lg-3>" + DT1.Rows[0]["Purpose of Camp"] + "</td><td col-lg-3><b>ID Proof No: </b></td><td col-lg-3>" + DT1.Rows[0]["ID Proof No"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b>No of Days: </b></td><td col-lg-3>" + DT1.Rows[0]["No of Days"] + "</td><td col-lg-3> </td><td col-lg-3> </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Deposite Amount:</b> </td><td col-lg-3>" + DT1.Rows[0]["Fees"] + "</td><td col-lg-3><b>Emitra Transaction ID:</b> </td><td col-lg-3>" + DT1.Rows[0]["Emitra Transaction ID"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>District Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["District Name"] + "</td><td col-lg-3><b>Place Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["Place Name"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Application Date:</b> </td><td col-lg-3>" + DT1.Rows[0]["Application Date"] + "</td><td col-lg-3><b>Status:</b>   </td><td col-lg-3> " + DT1.Rows[0]["status1"] + " </td></tr>");
                            if (Status != "1")
                            {
                                sb.Append("<tr><td col-lg-3><b>Action Taken By:</b> </td><td col-lg-3>" + DT1.Rows[0]["Name"] + "</td><td col-lg-3><b>Action Taken On:</b> </td><td col-lg-3>" + DT1.Rows[0]["Action Taken On"] + " </td></tr>");
                                if (Status != "2")
                                {
                                    sb.Append("<tr><td col-lg-3><b>Remarks:</b> </td><td col-lg-3>" + DT1.Rows[0]["Remarks"] + "</td><td col-lg-3><b>ActionReason:</b> </td><td col-lg-3>" + DT1.Rows[0]["Action Reason"] + " </td></tr>");
                                }
                            }
                            sb.Append("</table>");
                            sb.Append("<table>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b>Name</b></td><td col-lg-3><b>Address</b></td><td col-lg-3><b>Pin Code</b></td><td col-lg-3><b> Gender</b></td><td col-lg-3><b>ID Document</b></td><td col-lg-3><b>ID Document No</b></td><td col-lg-3><b>Nationality</b></td><td col-lg-3><b>Member Type</b></td></tr>");
                            for (int i = 0; i < DT2.Rows.Count; i++)
                            {
                                sb.Append("<tr><td col-lg-3>" + DT2.Rows[i]["Name"] + "</td><td col-lg-3>" + DT2.Rows[i]["Address"] + "</td><td col-lg-3>" + DT2.Rows[i]["Postal Code"] + "</td><td col-lg-3>" + DT2.Rows[i]["Gender"] + "</td><td col-lg-3>" + DT2.Rows[i]["IDType"] + "</td><td col-lg-3>" + DT2.Rows[i]["IDNo"] + "</td><td col-lg-3>" + DT2.Rows[i]["Nationality"] + "</td><td col-lg-3>" + DT2.Rows[i]["Member Type"] + "</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("<b style='fontsize:14px' >Instruction</b>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td>1. Bookings  are to be cross checked by  the Sanctuary/ Park authorities  at the time of entry.</td></tr>");
                            sb.Append("<tr><td>2. The visitor must reach to the forest booking center along with valid original photo ID mentioned at the time of booking, at least 45 minutes prior to the entrance time. In case, same Id proof is not  produced at the time entry in the park,   the permission shall be deemed cancelled or fake.</td></tr>");
                            sb.Append("<tr><td>3. The park/sanctuary authority  reserves the right to  cancel the  booking at any time due to unavoidable circumstances/ emergency.</td></tr>");
                            sb.Append("<tr><td>4. Damage to the  property or plants or tree or animal of park/sanctuary  booking will be cancelled by park/sanctuary authority without refund and penalty  will be imposed as per rules.</td></tr>");
                            sb.Append("<tr><td>5. Additional equipment/ video camera/ vehicle other than indicated in the  booking will not be allowed.</td></tr>");
                            sb.Append("<tr><td>6. Visitors should carry   two copies of this voucher for entire duration of camping/ Film Shooting.</td></tr>");
                            sb.Append("</table>");
                        }
                        else if (TableName == "tbl_EducationTourPermissions")
                        {
                            DT1 = ds.Tables[0];
                            string gender = "";
                            string nation = "";
                            string memType = "";
                            string IDType = "";
                            if (DT1.Columns.Contains("P_Gender"))
                            {
                                if (DT1.Rows[0]["P_Gender"].ToString() == "1")
                                {
                                    gender = "Male";
                                }
                                else if (DT1.Rows[0]["P_Gender"].ToString() == "2")
                                {
                                    gender = "Female";
                                }
                                else
                                {
                                    gender = "Transgender";
                                }
                            }
                            if (DT1.Rows[0]["P_Nationality"].ToString() == "1")
                            {
                                nation = "Indian";
                            }
                            else
                            {
                                nation = "Foreigner";
                            }
                            if (DT1.Columns.Contains("P_MemberType"))
                            {
                                if (DT1.Rows[0]["P_MemberType"].ToString() == "1")
                                {
                                    memType = "Child";
                                }
                                else if (DT1.Rows[0]["P_MemberType"].ToString() == "2")
                                {
                                    memType = "Adult";
                                }
                                else
                                {
                                    memType = "Student";
                                }
                            }
                            if (DT1.Columns.Contains("P_IDType"))
                            {
                                if (DT1.Rows[0]["P_IDType"].ToString() == "1")
                                {
                                    IDType = "Passport";
                                }
                                else if (DT1.Rows[0]["P_IDType"].ToString() == "2")
                                {
                                    IDType = "Aadhar";
                                }
                                else if (DT1.Rows[0]["P_IDType"].ToString() == "3")
                                {
                                    IDType = "Driving Licence";
                                }
                                else if (DT1.Rows[0]["P_IDType"].ToString() == "4")
                                {
                                    IDType = "Voter ID";
                                }
                                else
                                {
                                    IDType = "PAN Card";
                                }
                            }
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b> Requested ID: </b></td><td col-lg-3>" + DT1.Rows[0]["RequestedID"] + "</td><td col-lg-3>  <b> Permission For:</b> </td><td col-lg-3>Visit Services </td></tr>");
                            sb.Append("</table>");
                            sb.Append("<BR/>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b>Institute Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["CollegeName"] + "</td><td col-lg-3> <b>Institute Address</b></td><td col-lg-3> " + DT1.Rows[0]["CollegeAddress"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Institute Phone Number:</b> </td><td col-lg-3>" + DT1.Rows[0]["CollegePhone"] + "</td><td col-lg-3> <b>Category:</b></td><td col-lg-3> " + DT1.Rows[0]["Category"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-12 colspan='4'><label><h6><b></b></h6></label><div class='divider'></div></td></tr>");
                            sb.Append("<tr><td col-lg-3><b> From Date: </b></td><td col-lg-3>" + DT1.Rows[0]["From date"] + "</td><td col-lg-3><b>To Date: </b></td><td col-lg-3>" + DT1.Rows[0]["To Date"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b>Head of Institute Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["P_Name"] + "</td><td col-lg-3><b>Phone Number: </b></td><td col-lg-3> " + DT1.Rows[0]["P_Address"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b> District Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["District Name"] + "</td><td col-lg-3><b>Place Name : </b></td><td col-lg-3>" + DT1.Rows[0]["Place Name"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b> Application Date: </b></td><td col-lg-3>" + DT1.Rows[0]["Action Taken On"] + "</td><td col-lg-3><b>Status:</b>   </td><td col-lg-3> " + DT1.Rows[0]["Status"] + " </td></tr>");
                            if (Status != "1")
                            {
                                sb.Append("<tr><td col-lg-3><b> Action Taken By: </b></td><td col-lg-3>" + DT1.Rows[0]["Name"] + "</td><td col-lg-3><b>Action Taken On:</b> </td><td col-lg-3>" + DT1.Rows[0]["Action Taken On"] + " </td></tr>");
                                if (Status != "2")
                                {
                                    sb.Append("<tr><td col-lg-3><b> Remarks: </b></td><td col-lg-3>" + DT1.Rows[0]["Remarks"] + "</td><td col-lg-3><b> </b> </td><td col-lg-3>  </td></tr>");
                                }
                            }
                            sb.Append("</table>");
                        }
                        else
                        {
                            DT1 = ds.Tables[0];
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b> Requested ID: </b></td><td col-lg-3>" + DT1.Rows[0]["RequestedID"] + "</td><td col-lg-3>  <b> Permission For:</b> </td><td col-lg-3>Research Study </td></tr>");
                            sb.Append("</table>");
                            sb.Append("<BR/>");
                            sb.Append("<BR/>");
                            sb.Append("<table class='table table-bordered' id='tkt'>");
                            sb.Append("<tr><td col-lg-3><b> Subject:</b> </td><td col-lg-3>" + DT1.Rows[0]["Subject"] + "</td><td col-lg-3> <b>Procedure</b></td><td col-lg-3> " + DT1.Rows[0]["Procedure"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-12 colspan='4'><label><h6><b></b></h6></label><div class='divider'></div></td></tr>");
                            sb.Append("<tr><td col-lg-3><b> From Date: </b></td><td col-lg-3>" + DT1.Rows[0]["From date"] + "</td><td col-lg-3><b>To Date: </b></td><td col-lg-3>" + DT1.Rows[0]["To Date"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b> District Name:</b></td><td col-lg-3>" + DT1.Rows[0]["District Name"] + "</td><td col-lg-3><b>Place Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["Place Name"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b> Research Type:</b> </td><td col-lg-3>" + DT1.Rows[0]["Research Type"] + "</td><td col-lg-3><b>Animal Category: </b></td><td col-lg-3> " + DT1.Rows[0]["Animal Category"] + "</td></tr>");
                            sb.Append("<tr><td col-lg-3><b> Species Category: </b></td><td col-lg-3>" + DT1.Rows[0]["Species Category"] + "</td><td col-lg-3><b>Species Name</b></td><td col-lg-3>" + DT1.Rows[0]["Species Name"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b> Benefits: </b></td><td col-lg-3>" + DT1.Rows[0]["Benefits"] + "</td><td col-lg-3>  </td><td col-lg-3> </td></tr>");
                            sb.Append("<tr><td col-lg-3><b> Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["Name"] + "</td><td col-lg-3><b>Address </b></td><td col-lg-3>" + DT1.Rows[0]["Address"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b> District Name:</b> </td><td col-lg-3>" + DT1.Rows[0]["District Name"] + "</td><td col-lg-3><b>Place Name : </b></td><td col-lg-3>" + DT1.Rows[0]["Place Name"] + " </td></tr>");
                            sb.Append("<tr><td col-lg-3><b> Application Date: </b></td><td col-lg-3>" + DT1.Rows[0]["Action Taken On"] + "</td><td col-lg-3><b>Status:</b>   </td><td col-lg-3> " + DT1.Rows[0]["Status"] + " </td></tr>");
                            if (Status != "1")
                            {
                                sb.Append("<tr><td col-lg-3><b> Action Taken By: </b></td><td col-lg-3>" + DT1.Rows[0]["Action Taken By"] + "</td><td col-lg-3><b>Action Taken On:</b> </td><td col-lg-3>" + DT1.Rows[0]["Action Taken On"] + " </td></tr>");
                                if (Status != "2")
                                {
                                    sb.Append("<tr><td col-lg-3><b> Remarks: </b></td><td col-lg-3>" + DT1.Rows[0]["Remarks"] + "</td><td col-lg-3><b> </b> </td><td col-lg-3>  </td></tr>");
                                }
                            }
                            sb.Append("</table>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return sb.ToString();
        }
        /// <summary>
        /// Pdf genrate for approved request
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Status"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public ActionResult PDFAfterApproval(string RequestId, string Status, string TableName)
        {
            DataTable dtResult = new DataTable();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // PDFFormat pdfData = new Models.PDFFormat();
            string filepath = string.Empty;
            try
            {
                if (Session["UserID"] != null)
                {
                    DataSet ds = _objModel.GetPrintApplicationAfterApproval(RequestId, Status, TableName);
                    StringBuilder sb = new StringBuilder();
                    DataTable DT1 = new DataTable();
                    DataTable DT2 = new DataTable();
                    if (ds != null)
                    {
                        #region tbl_FixedPermissions
                        if (TableName == "tbl_FixedPermissions")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            filepath = "~/FixedLandDocument/MiningNOC_" + DateTime.Now.Ticks.ToString() + ".pdf";
                            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                            var FontColour = new BaseColor(0, 0, 0);
                            Paragraph tableheading = null;
                            Paragraph sideheading = null;
                            Phrase colHeading;
                            PdfPCell cell;
                            PdfPTable pdfTable = null;
                            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
                            doc.Open();
                            doc.NewPage();
                            doc.Add(new Paragraph(Environment.NewLine));
                            tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                            tableheading.Font.Size = 12;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            string Div_name = "";
                            if (DT2.Rows.Count > 0)
                            {
                                Div_name = DT2.Rows[0]["DIV_NAME"].ToString();
                            }
                            else
                            {
                                Div_name = "";
                            }
                            tableheading = new Paragraph("" + DT1.Rows[0]["Designation"] + "," + Div_name, MyFont);
                            tableheading.Font.Size = 12;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
                            tableheading.Font.Size = 10;
                            tableheading.Alignment = (Element.ALIGN_RIGHT);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            doc.Add(new Paragraph(Environment.NewLine));
                            tableheading = new Paragraph("No objection Certificate (NOC) for " + DT1.Rows[0]["Permission For"].ToString(), MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            string address = "";
                            if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                            {
                                address = "";
                            }
                            else
                            {
                                address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                            }
                            sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ", " + DT1.Rows[0]["Application Date"] + " Submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " for the following area is not lying  in forest region.");
                            sideheading.Font.Size = 10;
                            sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                            doc.Add(sideheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            pdfTable = new PdfPTable(7);
                            pdfTable.DefaultCell.Padding = 1;
                            pdfTable.WidthPercentage = 95;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            int count = 0;
                            if (DT2.Rows.Count > 0)
                            {
                                count = DT2.Rows.Count;
                                DT2.Columns.Remove("RequestedID");
                                //   DT2.Columns.Remove("KhasraNo");
                                DT2.AcceptChanges();
                                string[,] arrPdfData = new string[count, 7];
                                arrPdfData[0, 0] = "Division Name ";
                                arrPdfData[0, 1] = "District Name";
                                arrPdfData[0, 2] = "Block Name";
                                arrPdfData[0, 3] = "GP Name";
                                arrPdfData[0, 4] = "Village Name";
                                arrPdfData[0, 5] = "Khasra No";
                                arrPdfData[0, 6] = "Area ";
                                pdfTable.AddCell("Division Name ");
                                pdfTable.AddCell("District Name");
                                pdfTable.AddCell("Block Name");
                                pdfTable.AddCell("GP Name");
                                pdfTable.AddCell("Village Name");
                                pdfTable.AddCell("Khasra No");
                                pdfTable.AddCell("Area");
                                //pdfTable.HorizontalAlignment = 0;
                                //pdfTable.SpacingAfter = 10;
                                //pdfTable.DefaultCell.Border = 0;
                                //pdfTable.SetWidths(new int[] { 2, 2, 6, 3, 3, 3, 3 });
                                //pdfTable.WidthPercentage = 100;
                                //pdfTable.DefaultCell.Border = Rectangle.BOX;
                                //arrPdfData[0, 3] = dtResult.Rows[0]["GPSLat"].ToString() + "," + dtResult.Rows[0]["GPSLong"].ToString();
                                // pdfTable.GetHeader();
                                for (int xid = 0; xid < count; xid++)
                                {
                                    for (int yid = 0; yid < 7; yid++)
                                    {
                                        arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                                        colHeading = new Phrase(arrPdfData[xid, yid]);
                                        colHeading.Font.Size = 10;
                                        cell = new PdfPCell(new Phrase(colHeading));
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        pdfTable.AddCell(cell);
                                    }
                                }
                                doc.Add(pdfTable);
                                pdfTable = new PdfPTable(4);
                                pdfTable.DefaultCell.Padding = 1;
                                pdfTable.WidthPercentage = 95;
                                pdfTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            }
                            doc.Add(new Paragraph(Environment.NewLine));
                            doc.Add(new Paragraph(Environment.NewLine));
                            tableheading = new Paragraph(DT1.Rows[0]["Designation"].ToString(), MyFont);
                            tableheading.Font.Size = 10;
                            tableheading.Alignment = (Element.ALIGN_RIGHT);
                            doc.Add(tableheading);
                            tableheading = new Paragraph(DT1.Rows[0]["Name"].ToString(), MyFont);
                            tableheading.Font.Size = 10;
                            tableheading.Alignment = (Element.ALIGN_RIGHT);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            doc.Close();
                            if (System.IO.File.Exists(Server.MapPath(filepath)))
                            {
                                //ProcessStartInfo startInfo = new ProcessStartInfo
                                //{
                                //    Arguments = Server.MapPath(filepath),
                                //    FileName = "explorer.exe"
                                //};
                                //Process.Start(startInfo);
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
                        }
                        #endregion
                        #region Tbl_Citizen_TransitPermit
                        if (TableName == "Tbl_Citizen_TransitPermit")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            // DataTable DT3 = ds.Tables[2];
                            filepath = string.Empty;
                            filepath = "~/PDFFolder/CitizenTP/TransitPermit_" + DateTime.Now.Ticks.ToString() + ".pdf";
                            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);
                            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                            var FontColour = new BaseColor(0, 0, 0);
                            Paragraph tableheading = null;
                            Paragraph sideheading = null;
                            Phrase colHeading;
                            PdfPCell cell;
                            PdfPTable pdfTable = null;
                            var subheadfont = FontFactory.GetFont("Times New Roman", 10, FontColour);
                            var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
                            var myfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);
                            //boldFont.SetStyle(FontFactory.H);
                            doc.Open();
                            doc.NewPage();
                            // doc.Add(new Paragraph(Environment.NewLine));
                            /////create Table
                            // PdfPTable tablehead;
                            // tablehead = new PdfPTable(3);
                            PdfPTable Logo = new PdfPTable(1) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            string imageURL = "~/images/logo.png";
                            iTextSharp.text.Image ForestLogo = iTextSharp.text.Image.GetInstance(Server.MapPath(imageURL));
                            ForestLogo.ScaleToFit(150f, 150f);
                            ForestLogo.SpacingBefore = 10f;
                            ForestLogo.SpacingAfter = 10f;
                            ForestLogo.Alignment = Element.ALIGN_CENTER;
                            PdfPCell cellForestLogo;
                            cellForestLogo = new PdfPCell(ForestLogo);
                            cellForestLogo.BorderWidth = 0;
                            cellForestLogo.Padding = 20;
                            cellForestLogo.PaddingTop = -10;
                            cellForestLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                            Logo.AddCell(cellForestLogo);
                            doc.Add(Logo);
                            #region New PDF By ProductList
                            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cells = new PdfPCell() { Border = 4 };
                            Details.TotalWidth = 120;
                            Details.SetTotalWidth(new float[] { 35f, 50f, 35f });
                            cells = new PdfPCell(new Phrase("FORM-I", boldFont)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);
                            cells = new PdfPCell(new Phrase("(See rule 4)", boldFont)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);
                            cells = new PdfPCell(new Phrase("Transit Pass under  rule 2 of the Rajasthan Forest (Produce Transit) Rules, 1957", subheadfont)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            Details.AddCell(cells);
                            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            cells.Colspan = 3;
                            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Details.AddCell(cells);
                            doc.Add(Details);
                            PdfPTable ProductDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            PdfPCell cellproduct = new PdfPCell() { Border = 4 };
                            ProductDetails.TotalWidth = 110;
                            ProductDetails.SetTotalWidth(new float[] { 5f, 20f, 15f, 10f, 20f, 25f });
                            cellproduct = new PdfPCell(new Phrase("1", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Name of Issuing Officer", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApprovedBy"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("2", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Permit Number", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("3", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Date", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["IssueDate"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("4", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Name of the Person", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantName"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("5", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Address", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantAddress"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("6", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Village", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantVillage"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("7", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Tehsil", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["APPLICANT_TEHSIL"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("8", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("District", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["APPLICANT_DISTRICT"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("9", subheadfont));
                            cellproduct.Rowspan = DT2 != null ? DT2.Rows.Count + 2 : 2;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Quantity and description of forest produce for which the pass is valid", subheadfont));
                            cellproduct.Colspan = 5;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Produce Name", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Unit", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Quantity", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Description", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            #region Multiple Product
                            if (DT2 != null && DT2.Rows.Count > 0)
                            {
                                for (int i = 0; i <= DT2.Rows.Count - 1; i++)
                                {
                                    cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductName"]), subheadfont));
                                    cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ProductDetails.AddCell(cellproduct);
                                    cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductUnit"]), subheadfont));
                                    cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ProductDetails.AddCell(cellproduct);
                                    cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["PRODUCE_QUANTITY"]), subheadfont));
                                    cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ProductDetails.AddCell(cellproduct);
                                    cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["Desc"]), subheadfont));
                                    cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cellproduct.Colspan = 2;
                                    ProductDetails.AddCell(cellproduct);
                                }
                            }
                            #endregion
                            cellproduct = new PdfPCell(new Phrase("10", subheadfont));
                            cellproduct.Rowspan = 6;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Description of origin of forest produce covered by the pass:", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 5;
                            ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 3;
                            //ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("a. Name of land holder from whose land the raw material obtained", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderName"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("b. Name of village and khasra number where the land holding is situated", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderVillageName"]) + " / " + Convert.ToString(DT1.Rows[0]["LandHoldingKhasraNo"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("c. Area of land holding", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHoldingArea"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("d. Name of District and Tehsil of land holding", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderDistrict"]) + " / " + Convert.ToString(DT1.Rows[0]["LandHolderTehsil"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("e. Name of place where the forest produce was stored/ converted", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["PlaceNamewhereProduceGenerate"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("11", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Type and registration number of vehicle used in transportation of forest produce:", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleType"]) + " / " + Convert.ToString(DT1.Rows[0]["VehicleNo"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("12", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Fee paid for the pass", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["FeeAmount"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("13", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Fee paid on", subheadfont));
                            cellproduct.Colspan = 2;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["EmitraDate"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 3;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("14", subheadfont));
                            cellproduct.Rowspan = 2;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Place from and to which such produce is to be taken or conveyed", subheadfont));
                            cellproduct.Rowspan = 2;
                            cellproduct.Colspan = 2;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("From", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateFrom"]) + " / " + Convert.ToString(DT1.Rows[0]["MovementDistFrom"]), subheadfont));
                            cellproduct.Colspan = 2;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("To", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateTo"]) + " / " + Convert.ToString(DT1.Rows[0]["MovementDistTo"]), subheadfont));
                            cellproduct.Colspan = 2;
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("15", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Route through which such forest produce is to be conveyed", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementRoutePlan"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 4;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("16", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Period for which the pass shall be valid", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TP_Valid_ApprovalDate"]), subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 4;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("17", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Specimen Signaure of the Pass holder", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 4;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("18", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("Signature and Stamp of the Issuing Officer", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 2;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellproduct.Colspan = 6;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            cellproduct.Colspan = 6;
                            cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellproduct.Colspan = 6;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase("( " + Convert.ToString(DT1.Rows[0]["ApprovedBy"]) + " )", subheadfont)) { Border = 0 };
                            cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellproduct.Colspan = 6;
                            ProductDetails.AddCell(cellproduct);
                            cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApprovedDesignationName"]), subheadfont)) { Border = 0 };
                            cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            cellproduct.Colspan = 6;
                            ProductDetails.AddCell(cellproduct);
                            doc.Add(ProductDetails);
                            #endregion
                            //#region New PDF
                            //PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell cells = new PdfPCell() { Border = 4 };
                            //Details.TotalWidth = 120;
                            //Details.SetTotalWidth(new float[] { 35f, 50f, 35f });
                            //cells = new PdfPCell(new Phrase("FORM-I", boldFont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase("(See rule 4)", boldFont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase("Transit Pass under  rule 2 of the Rajasthan Forest (Produce Transit) Rules, 1957", subheadfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //Details.AddCell(cells);
                            //doc.Add(Details);
                            //PdfPTable ProductDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell cellproduct = new PdfPCell() { Border = 4 };
                            //ProductDetails.TotalWidth = 90;
                            //ProductDetails.SetTotalWidth(new float[] { 5f, 45f, 10f, 10f, 10f, 10f });
                            //cellproduct = new PdfPCell(new Phrase("1", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Name of Issuing Officer", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("2", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Permit Number", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("3", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Date", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["IssueDate"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("4", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Name of the Person", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantName"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("5", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Address", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantAddress"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("6", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Village", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["ApplicantVillage"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("7", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Tehsil", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["APPLICANT_TEHSIL"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("8", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("District", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["APPLICANT_DISTRICT"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("9", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Quantity and description of forest produce for which the pass is valid", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("10", subheadfont));
                            //cellproduct.Rowspan = 6;
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Description of origin of forest produce covered by the pass:", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("a. Name of land holder from whose land the raw material obtained", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderName"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("b. Name of village and khasra number where the land holding is situated", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderVillageName"])+" / "+ Convert.ToString(DT1.Rows[0]["LandHoldingKhasraNo"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("c. Area of land holding", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHoldingArea"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("d. Name of District and Tehsil of land holding", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["LandHolderDistrict"]) + " / " + Convert.ToString(DT1.Rows[0]["LandHolderTehsil"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("e. Name of place where the forest produce was stored/ converted", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["PlaceNamewhereProduceGenerate"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("11", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Type and registration number of vehicle used in transportation of forest produce:", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleType"])+" / "+ Convert.ToString(DT1.Rows[0]["VehicleNo"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("12", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Fee paid for the pass", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["FeeAmount"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("13", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Fee paid on", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("14", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Place from and to which such produce is to be taken or conveyed", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("From", subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateFrom"])+" / "+ Convert.ToString(DT1.Rows[0]["MovementDistFrom"]), subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("To", subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateTo"]) + " / " + Convert.ToString(DT1.Rows[0]["MovementDistTo"]), subheadfont)) { Border = 3 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("15", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Route through which such forest produce is to be conveyed", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementRoutePlan"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("16", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Period for which the pass shall be valid", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["TP_Valid_ApprovalDate"]), subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("17", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Specimen Signaure of the Pass holder", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 4;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("18", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Signature and Stamp of the Issuing Officer", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.Colspan = 6;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("By the Order of the Governor", subheadfont)) { Border = 0 };
                            //cellproduct.Colspan = 6;
                            //cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //cellproduct.Colspan = 6;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("(Yogendra Kumar Dak)", subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //cellproduct.Colspan = 6;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Secretary to the Government", subheadfont)) { Border = 0 };
                            //cellproduct.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //cellproduct.Colspan = 6;
                            //ProductDetails.AddCell(cellproduct);
                            //doc.Add(ProductDetails);
                            //#endregion
                            #region Old Pdf
                            //PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell cells = new PdfPCell() { Border = 4 };
                            //Details.TotalWidth = 120;
                            //Details.SetTotalWidth(new float[] { 35f, 50f, 35f });
                            //cells = new PdfPCell(new Phrase("Citizen Transit Permit", boldFont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase("Permit Number:" + Convert.ToString(DT1.Rows[0]["RequestID"]), subheadfont)) { Border = 0 };
                            //cells.Colspan = 2;
                            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase("Date:-" + Convert.ToString(DT1.Rows[0]["IssueDate"]), subheadfont)) { Border = 0 };
                            //cells.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase("TP Valid Date:-" + Convert.ToString(DT1.Rows[0]["TP_Valid_ApprovalDate"]), subheadfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase("        This transit permit issued to Mr./Ms. " + Convert.ToString(DT1.Rows[0]["ApplicantName"]) + " address " + Convert.ToString(DT1.Rows[0]["ApplicantAddress"]) + " for product whose details are mentioned below and this Transit Permit valid till " + Convert.ToString(DT1.Rows[0]["TP_VALIDITY_DATE"]), myfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                            //Details.AddCell(cells);
                            //cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
                            //cells.Colspan = 3;
                            //cells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //Details.AddCell(cells);
                            //doc.Add(Details);
                            //PdfPTable ProductDetails = new PdfPTable(6) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell cellproduct = new PdfPCell() { Border = 6 };
                            //ProductDetails.TotalWidth = 150;
                            //ProductDetails.SetTotalWidth(new float[] { 10f, 20f, 20f, 15f, 15f, 70f });
                            //cellproduct = new PdfPCell(new Phrase("Product Details:", boldFont)) { Border = 0 };
                            //cellproduct.Colspan = 6;
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
                            //cellproduct.Colspan = 6;
                            //cellproduct.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("S.No.", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Product Type", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Product Name", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Unit", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Quantity", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //ProductDetails.AddCell(cellproduct);
                            //cellproduct = new PdfPCell(new Phrase("Description", subheadfont));
                            //cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellproduct.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //ProductDetails.AddCell(cellproduct);
                            //#region Multiple Product
                            //if (DT2 != null && DT2.Rows.Count > 0)
                            //{
                            //    for (int i = 0; i <= DT2.Rows.Count - 1; i++)
                            //    {
                            //        cellproduct = new PdfPCell(new Phrase((i + 1).ToString(), subheadfont));
                            //        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        ProductDetails.AddCell(cellproduct);
                            //        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductType"]), subheadfont));
                            //        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        ProductDetails.AddCell(cellproduct);
                            //        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductName"]), subheadfont));
                            //        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        ProductDetails.AddCell(cellproduct);
                            //        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["ProductUnit"]), subheadfont));
                            //        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        ProductDetails.AddCell(cellproduct);
                            //        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["PRODUCE_QUANTITY"]), subheadfont));
                            //        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        ProductDetails.AddCell(cellproduct);
                            //        cellproduct = new PdfPCell(new Phrase(Convert.ToString(DT2.Rows[i]["Desc"]), subheadfont));
                            //        cellproduct.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        ProductDetails.AddCell(cellproduct);
                            //    }
                            //}
                            //#endregion
                            //doc.Add(ProductDetails);
                            //#region Movement Details
                            //PdfPTable ExtraDetails = new PdfPTable(5) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell cellExtra = new PdfPCell();
                            //ExtraDetails.TotalWidth = 100;
                            //ExtraDetails.SetTotalWidth(new float[] { 25f, 25f, 25f, 25f, 25f });
                            //cellExtra = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
                            //cellExtra.Colspan = 5;
                            //cellExtra.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("Extra Details", boldFont)) { Border = 0 };
                            //cellExtra.Colspan = 5;
                            //cellExtra.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
                            //cellExtra.Colspan = 5;
                            //cellExtra.HorizontalAlignment = Element.ALIGN_LEFT;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("From", boldFont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.Colspan = 2;
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("To", boldFont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.Colspan = 2;
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("Route", boldFont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.Rowspan = 2;
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("State", subheadfont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("Distict", subheadfont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("State", subheadfont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase("Distict", subheadfont));
                            //cellExtra.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateFrom"]), subheadfont));
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementDistFrom"]), subheadfont));
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementStateTo"]), subheadfont));
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementDistTo"]), subheadfont));
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //cellExtra = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["MovementRoutePlan"]), subheadfont));
                            //cellExtra.HorizontalAlignment = Element.ALIGN_CENTER;
                            //ExtraDetails.AddCell(cellExtra);
                            //doc.Add(ExtraDetails);
                            //#endregion
                            //#region Vehicle Details
                            //PdfPTable VehicleDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell cellvehicle = new PdfPCell();
                            //VehicleDetails.TotalWidth = 75;
                            //VehicleDetails.SetTotalWidth(new float[] { 25f, 25f, 25f });
                            //cellvehicle = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
                            //cellvehicle.Colspan = 3;
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_CENTER;
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase("Vehicle Details", boldFont)) { Border = 0 };
                            //cellvehicle.Colspan = 3;
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
                            //cellvehicle.Colspan = 3;
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_CENTER;
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase("Vehicle Number", subheadfont));
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellvehicle.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase("Type", subheadfont));
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellvehicle.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase("Payble Amount", subheadfont));
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cellvehicle.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleNo"]), subheadfont));
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["VehicleType"]), subheadfont));
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //VehicleDetails.AddCell(cellvehicle);
                            //cellvehicle = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["FeeAmount"]), subheadfont));
                            //cellvehicle.HorizontalAlignment = Element.ALIGN_LEFT;
                            //VehicleDetails.AddCell(cellvehicle);
                            //doc.Add(VehicleDetails);
                            //PdfPTable btmDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            //PdfPCell btmcells = new PdfPCell() { Border = 4 };
                            //btmDetails.TotalWidth = 120;
                            //btmDetails.SetTotalWidth(new float[] { 35f, 50f, 35f });
                            //btmcells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
                            //btmcells.Colspan = 3;
                            //btmcells.HorizontalAlignment = Element.ALIGN_CENTER;
                            //btmDetails.AddCell(btmcells);
                            //btmcells = new PdfPCell(new Phrase("Issued By Division office", subheadfont)) { Border = 0 };
                            //btmcells.Colspan = 3;
                            //btmcells.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //btmDetails.AddCell(btmcells);
                            //btmcells = new PdfPCell(new Phrase(Convert.ToString(DT1.Rows[0]["Division"]), subheadfont)) { Border = 0 };
                            //btmcells.Colspan = 3;
                            //btmcells.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //btmDetails.AddCell(btmcells);
                            //doc.Add(btmDetails);
                            //#endregion
                            //#region Survey Report
                            ////if (DT3 != null && DT3.Rows.Count > 0)
                            ////{
                            ////    PdfPTable SurveyDetails = new PdfPTable(9) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                            ////    PdfPCell cellSurvey = new PdfPCell();
                            ////    SurveyDetails.TotalWidth = 225;
                            ////    SurveyDetails.SetTotalWidth(new float[] { 25f, 25f, 25f, 25f, 25f, 25f, 25f, 25f, 25f });
                            ////    cellSurvey = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
                            ////    cellSurvey.Colspan = 9;
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Survey Details", boldFont)) { Border = 0 };
                            ////    cellSurvey.Colspan = 9;
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(" ")) { Border = 0 };
                            ////    cellSurvey.Colspan = 9;
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_CENTER;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Division Office", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Village", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Tehsil", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Survey Date", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Area Name", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Area in KM", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Latitude", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Longitude", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase("Description", subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    cellSurvey.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["DIVISION_OFFICE"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["District_OFFICE"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["Village"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["SurveyDate"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["AreaName"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["AreaInKm"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["Latitude"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["Longitude"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    cellSurvey = new PdfPCell(new Phrase(Convert.ToString(DT3.Rows[0]["Description"]), subheadfont));
                            ////    cellSurvey.HorizontalAlignment = Element.ALIGN_LEFT;
                            ////    SurveyDetails.AddCell(cellSurvey);
                            ////    doc.Add(SurveyDetails);
                            //// }
                            //#endregion
                            #endregion
                            doc.Close();
                            if (System.IO.File.Exists(Server.MapPath(filepath)))
                            {
                                //ProcessStartInfo startInfo = new ProcessStartInfo
                                //{
                                //    Arguments = Server.MapPath(filepath),
                                //    FileName = "explorer.exe"
                                //};
                                //Process.Start(startInfo);
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
                        }
                        #endregion
                        #region Org Camp
                        if (TableName == "tbl_OrganisingCamp")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            filepath = "~/FixedLandDocument/orgCamp_" + DateTime.Now.Ticks.ToString() + ".pdf";
                            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                            var FontColour = new BaseColor(0, 0, 0);
                            Paragraph tableheading = null;
                            Paragraph sideheading = null;
                            Phrase colHeading;
                            PdfPCell cell;
                            PdfPTable pdfTable = null;
                            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
                            doc.Open();
                            doc.NewPage();
                            tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                            tableheading.Font.Size = 12;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            tableheading = new Paragraph("No objection Certificate (NOC) for Organising Camp", MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            tableheading = new Paragraph("Office of  " + DT1.Rows[0]["Place Name"] + "," + DT1.Rows[0]["District Name"], MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            tableheading = new Paragraph("Dated: " + DT1.Rows[0]["Action Taken On"] + "", MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_RIGHT);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            string address = "";
                            if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                            {
                                address = "";
                            }
                            else
                            {
                                address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                            }
                            //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["Application Date"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                            sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " for Organizing Camp has been approved. Following members is allow to visit " + DT1.Rows[0]["Place Name"] + ", " + DT1.Rows[0]["District Name"] + " from " + DT1.Rows[0]["DurationFrom"].ToString() + " to  " + DT1.Rows[0]["DurationTo"].ToString() + ".");
                            sideheading.Font.Size = 10;
                            sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                            doc.Add(sideheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            pdfTable = new PdfPTable(6);
                            pdfTable.DefaultCell.Padding = 1;
                            pdfTable.WidthPercentage = 95;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            int count = DT2.Rows.Count;
                            //   DT2.Columns.Remove("KhasraNo");
                            DT2.AcceptChanges();
                            string[,] arrPdfData = new string[count, 7];
                            PdfPCell cellName = new PdfPCell(new Phrase("Name"));
                            cellName.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellName.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellName);
                            PdfPCell cellAddress = new PdfPCell(new Phrase("Address"));
                            cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAddress.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellAddress);
                            PdfPCell cellLand = new PdfPCell(new Phrase("Land Mark"));
                            cellLand.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellLand.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellLand);
                            PdfPCell cellPostal = new PdfPCell(new Phrase("Postal Code"));
                            cellPostal.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellPostal.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellPostal);
                            PdfPCell cellGender = new PdfPCell(new Phrase("Gender"));
                            cellGender.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGender.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellGender);
                            PdfPCell cellID = new PdfPCell(new Phrase("ID No."));
                            cellID.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellID.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellID);
                            for (int xid = 0; xid < count; xid++)
                            {
                                for (int yid = 0; yid < 6; yid++)
                                {
                                    arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                                    colHeading = new Phrase(arrPdfData[xid, yid]);
                                    colHeading.Font.Size = 10;
                                    cell = new PdfPCell(new Phrase(colHeading));
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfTable.AddCell(cell);
                                }
                            }
                            tableheading = new Paragraph("Guest/Visitor Details");
                            tableheading.Font.Size = 14;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            doc.Add(pdfTable);
                            tableheading = new Paragraph("For any query, contact us at <> ", MyFont);
                            tableheading.Font.Size = 10;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
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
                        }
                        #endregion
                        #region FilmShooting
                        if (TableName == "tbl_FilmShootingPermissions")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            filepath = "~/FixedLandDocument/Film_" + DateTime.Now.Ticks.ToString() + ".pdf";
                            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                            var FontColour = new BaseColor(0, 0, 0);
                            Paragraph tableheading = null;
                            Paragraph sideheading = null;
                            Phrase colHeading;
                            PdfPCell cell;
                            PdfPTable pdfTable = null;
                            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
                            doc.Open();
                            doc.NewPage();
                            tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                            tableheading.Font.Size = 12;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            tableheading = new Paragraph("No objection Certificate (NOC) for Film Shooting", MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            tableheading = new Paragraph("Office of  " + DT1.Rows[0]["PlaceName"] + "," + DT1.Rows[0]["DIST_NAME"], MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            tableheading = new Paragraph("Dated: " + DT1.Rows[0]["ActionTakenOn"] + "", MyFont);
                            tableheading.Font.Size = 11;
                            tableheading.Alignment = (Element.ALIGN_RIGHT);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            string address = "";
                            if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                            {
                                address = "";
                            }
                            else
                            {
                                address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                            }
                            //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["EnteredOn"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                            sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " for Film Shooting has been approved. Following members is allow to visit " + DT1.Rows[0]["PlaceName"] + ", " + DT1.Rows[0]["DIST_NAME"] + " from " + DT1.Rows[0]["DurationFrom"].ToString() + " to  " + DT1.Rows[0]["DurationTo"].ToString() + ".");
                            sideheading.Font.Size = 10;
                            sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                            doc.Add(sideheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            pdfTable = new PdfPTable(6);
                            pdfTable.DefaultCell.Padding = 1;
                            pdfTable.WidthPercentage = 95;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            int count = DT2.Rows.Count;
                            //   DT2.Columns.Remove("KhasraNo");
                            DT2.AcceptChanges();
                            string[,] arrPdfData = new string[count, 7];
                            PdfPCell cellName = new PdfPCell(new Phrase("Name"));
                            cellName.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellName.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellName);
                            PdfPCell cellAddress = new PdfPCell(new Phrase("Address"));
                            cellAddress.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellAddress.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellAddress);
                            PdfPCell cellLand = new PdfPCell(new Phrase("Land Mark"));
                            cellLand.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellLand.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellLand);
                            PdfPCell cellPostal = new PdfPCell(new Phrase("Postal Code"));
                            cellPostal.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellPostal.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellPostal);
                            PdfPCell cellGender = new PdfPCell(new Phrase("Gender"));
                            cellGender.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellGender.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellGender);
                            PdfPCell cellID = new PdfPCell(new Phrase("ID No."));
                            cellID.HorizontalAlignment = Element.ALIGN_LEFT;
                            cellID.VerticalAlignment = Element.ALIGN_CENTER;
                            pdfTable.AddCell(cellID);
                            for (int xid = 0; xid < count; xid++)
                            {
                                for (int yid = 0; yid < 6; yid++)
                                {
                                    arrPdfData[xid, yid] = DT2.Rows[xid][yid].ToString();
                                    colHeading = new Phrase(arrPdfData[xid, yid]);
                                    colHeading.Font.Size = 10;
                                    cell = new PdfPCell(new Phrase(colHeading));
                                    cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    pdfTable.AddCell(cell);
                                }
                            }
                            tableheading = new Paragraph("Guest/Visitor Details");
                            tableheading.Font.Size = 14;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            doc.Add(pdfTable);
                            tableheading = new Paragraph("For any query, contact us at <> ", MyFont);
                            tableheading.Font.Size = 10;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
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
                        }
                        #endregion
                        #region Research Study
                        if (TableName == "tbl_ResearchStudyPermissions")
                        {
                            DT1 = ds.Tables[0];
                            filepath = "~/FixedLandDocument/research_" + DateTime.Now.Ticks.ToString() + ".pdf";
                            Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                            var FontColour = new BaseColor(0, 0, 0);
                            Paragraph tableheading = null;
                            Paragraph sideheading = null;
                            Paragraph sideheading1 = null;
                            Paragraph sideheading2 = null;
                            Phrase colHeading;
                            PdfPCell cell;
                            PdfPTable pdfTable = null;
                            var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                            var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
                            doc.Open();
                            doc.NewPage();
                            Image image = Image.GetInstance(@"C:\PublishFMDSS\images\logo.png");
                            //image.ScaleAbsolute(50, 50);
                            doc.Add(image);
                            doc.Add(new Paragraph(Environment.NewLine));
                            tableheading = new Paragraph("Officer of the Pr.Chief Conservator of forests & Chief Wildlife Warden,jaipur,Rajasthan", MyFont);
                            //tableheading.Font.GetType(Font.UNDEFINED);
                            tableheading.Font.Size = 12;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            Chunk reqNo = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph = new Paragraph("Request No:" + DT1.Rows[0]["RequestedID"].ToString());
                            paragraph.Add(new Chunk(reqNo));
                            paragraph.Add("Dated:" + DT1.Rows[0]["Action Taken On"].ToString());
                            doc.Add(paragraph);
                            Paragraph p = new Paragraph("To");
                            doc.Add(p);
                            Paragraph pt = new Paragraph("Mrs/MS " + DT1.Rows[0]["Entered By"].ToString() + "\n" + DT1.Rows[0]["Postal_Address1"].ToString() + "\n\n" + "Sub:" + DT1.Rows[0]["Subject"].ToString() + " in " + DT1.Rows[0]["Place Name"].ToString() + "\n Ref: & Your Request dated:" + DT1.Rows[0]["Action Taken On"].ToString());
                            pt.IndentationLeft = 60f;
                            doc.Add(pt);
                            iTextSharp.text.Paragraph titolo = new iTextSharp.text.Paragraph("Sir/Madam");
                            titolo.SpacingAfter = 10;
                            doc.Add(titolo);
                            sideheading = new Paragraph("With reference to your request cited above on the subject and as per the letter of Director,Ministry of ");
                            //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["From Date"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                            //sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " has been approved for below request.");
                            sideheading.Font.Size = 10;
                            sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                            sideheading.IndentationLeft = 60f;
                            doc.Add(sideheading);
                            Paragraph sideheading4 = new Paragraph("Human Resources Development, Department of Higher Education Gov. of india, new Delhi Dated 4.5.2007, the permission to" + DT1.Rows[0]["Subject"].ToString() + " in " + DT1.Rows[0]["Place Name"].ToString() + " w.e.f " + DT1.Rows[0]["From date"] + " to" + DT1.Rows[0]["From date"] + " is hereby grantedunder the section 28(1)(c) of Wildlife (protection) Act. 1972.This permission is given subject to the conditions mentioned below:-");
                            //sideheading = new Paragraph("This is to certify that the  application no " + DT1.Rows[0]["RequestedID"] + ",  " + DT1.Rows[0]["From Date"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + ", " + DT1.Rows[0]["Postal_Address1"].ToString() + " for the following area is not lying  in forest region.");
                            //sideheading = new Paragraph("This is to certify that the application no. " + DT1.Rows[0]["RequestedID"] + " submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " has been approved for below request.");
                            sideheading4.Font.Size = 10;
                            sideheading4.Alignment = (Element.ALIGN_JUSTIFIED);
                            doc.Add(sideheading4);
                            List orderedList = new List(List.ORDERED);
                            orderedList.Add(new ListItem("The programme of survey/study is to be necessarily got approved from the DCF & Dy.Director (Buffer) karauli, officer in-charge,prior to entry in the protected area."));
                            orderedList.Add(new ListItem("The provision rules. & regulations regarding National parks. sanctuaries and protected area should be adhered strictly."));
                            orderedList.Add(new ListItem("This is ensure that no wild animal would be disturbed or caused any harm."));
                            orderedList.Add(new ListItem("For the purpose of survey work, no medice will be allowed to give to the wild animal, nor change any habit of eating would be allowed."));
                            orderedList.Add(new ListItem("The Instruction issued by the office in-charge at the time of survey work should be followed strictly."));
                            orderedList.Add(new ListItem("The movement of vehicle in protected area/National park  ot protected area, while entering and then after would be under the complete control of officer In-charge concerned."));
                            orderedList.Add(new ListItem("If any irregularity,loss,incident of poaching is senn in the protected area it should be immediately communicated to tge officer In-charge in writing or telephonically."));
                            orderedList.Add(new ListItem("This permission can be withdrawn in the intrest of wildlifeat any time without mentioning the reason therefore."));
                            orderedList.Add(new ListItem("The researcher and their assistants should have to maintain a log book regarding movement in the park and the said log book should be made available to officer In-charge regularly."));
                            orderedList.Add(new ListItem("The study work should be  based on the observation, interviews etc."));
                            orderedList.Add(new ListItem("Wildlife Institute of India will work as nodal agency for the researchers.The scholar has to provide soft and hard copy of the results,conclusions,inference of research work etc. to the agency."));
                            orderedList.Add(new ListItem("After completion of survey/study work,copy of the complete report should be provided to this office and officer In-charge of the sanctuary as well."));
                            orderedList.Add(new ListItem("The instruction mentioned in the letter dated 4.5.2007 issued by the Human Resources Department,Department of Higher Education Govt. of India, New Delhi should be adhered strictly."));
                            orderedList.Add(new ListItem("MS Alison wadmore,british National, will travel to/from this office in the jaipur and kela devi Wildlife Sanctuary to share information during the study period."));
                            orderedList.Add(new ListItem("This permission willbe extended upto 31.8.2008 after receiving the extension in visa period."));
                            doc.Add(orderedList);
                            Chunk ch1 = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph1 = new Paragraph("");
                            paragraph1.Add(new Chunk(ch1));
                            paragraph1.Add("Your faithfully.");
                            doc.Add(paragraph1);
                            Chunk ch2 = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph2 = new Paragraph("");
                            paragraph2.Add(new Chunk(ch2));
                            paragraph2.Add("Pr.Chief Conservator of forests & Chief Wildlife Warden.");
                            doc.Add(paragraph2);
                            Chunk ch3 = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph3 = new Paragraph("");
                            paragraph3.Add(new Chunk(ch3));
                            paragraph3.Add("jaipur, Rajasthan");
                            doc.Add(paragraph3);
                            Chunk ch4 = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph4 = new Paragraph("Request No:" + DT1.Rows[0]["RequestedID"].ToString());
                            paragraph4.Add(new Chunk(ch4));
                            paragraph4.Add("Dated:" + DT1.Rows[0]["Action Taken On"].ToString());
                            doc.Add(paragraph4);
                            tableheading = new Paragraph("Copy forwarded to the following for information and neccessary action:-", MyFont);
                            tableheading.Font.Size = 10;
                            tableheading.Alignment = (Element.ALIGN_CENTER);
                            doc.Add(tableheading);
                            doc.Add(new Paragraph(Environment.NewLine));
                            List orderedList1 = new List(List.ORDERED);
                            orderedList1.Add(new ListItem("Director, Ministory of Human Resources Development, Department of Higher Education Govt. of India, New Delhi."));
                            orderedList1.Add(new ListItem("Additional Secretary Forest Department, Gov. of Rajasthan Jaipur."));
                            orderedList1.Add(new ListItem("Conservator of Forests & Field Director Ranthambhore Tiger project Kota."));
                            orderedList1.Add(new ListItem("Dy Conservator of Forests(Buffer), karauli."));
                            doc.Add(orderedList1);
                            Chunk ch5 = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph5 = new Paragraph("");
                            paragraph5.Add(new Chunk(ch5));
                            paragraph5.Add("Pr.Chief Conservator of forests & Chief Wildlife Warden.");
                            doc.Add(paragraph5);
                            Chunk ch6 = new Chunk(new VerticalPositionMark());
                            Paragraph paragraph6 = new Paragraph("");
                            paragraph6.Add(new Chunk(ch6));
                            paragraph6.Add("jaipur, Rajasthan");
                            doc.Add(paragraph6);
                            doc.Close();
                            if (System.IO.File.Exists(Server.MapPath(filepath)))
                            {
                                //ProcessStartInfo startInfo = new ProcessStartInfo
                                //{
                                //    Arguments = Server.MapPath(filepath),
                                //    FileName = "explorer.exe"
                                //};
                                //Process.Start(startInfo);
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
                        }
                        #endregion
                    }
                }
                return RedirectToAction("dashboard", "dashboard");
                // return new RazorPDF.PdfResult(pdfData, "PDFAfterApproval");
            }
            catch (Exception ex)
            { throw ex; }
        }
        /// <summary>
        /// Genrate pdf for reassigned request
        /// </summary>
        /// <param name="RequestId"></param>
        /// <param name="Status"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public ActionResult PDFForReassigned(string RequestId, string Status, string TableName)
        {
            DataTable dtResult = new DataTable();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // PDFFormat pdfData = new Models.PDFFormat();
            string filepath = string.Empty;
            try
            {
                if (Session["UserID"] != null)
                {
                    DataSet ds = _objModel.GetRequestforreassignedPdf(RequestId, TableName);
                    StringBuilder sb = new StringBuilder();
                    DataTable DT1 = new DataTable();
                    DataTable DT2 = new DataTable();
                    DataTable DT3 = new DataTable();
                    if (ds != null)
                    {
                        if (TableName == "tbl_FixedPermissions")
                        {
                            DT1 = ds.Tables[0];
                            DT2 = ds.Tables[1];
                            DT3 = ds.Tables[2];
                            if (DT1.Rows.Count > 0 && DT2.Rows.Count > 0 && DT3.Rows.Count > 0)
                            {
                                filepath = "~/FixedLandDocument/Reassignedletter_" + DateTime.Now.Ticks.ToString() + ".pdf";
                                Document doc = new Document(PageSize.A4, 25, 25f, 25f, 25f);
                                PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));
                                var FontColour = new BaseColor(0, 0, 0);
                                Paragraph tableheading = null;
                                Paragraph sideheading = null;
                                Phrase colHeading;
                                PdfPCell cell;
                                PdfPTable pdfTable = null;
                                var MyFont = FontFactory.GetFont("Arial", 12, FontColour);
                                var subheadfont = FontFactory.GetFont("Arial", 10, FontColour);
                                doc.Open();
                                doc.NewPage();
                                doc.Add(new Paragraph(Environment.NewLine));
                                tableheading = new Paragraph("Government of Rajasthan,Forest Department,", MyFont);
                                tableheading.Font.Size = 12;
                                tableheading.Alignment = (Element.ALIGN_CENTER);
                                doc.Add(tableheading);
                                tableheading = new Paragraph("" + DT1.Rows[0]["Designation"] + "," + DT2.Rows[0]["DIV_NAME"], MyFont);
                                tableheading.Font.Size = 12;
                                tableheading.Alignment = (Element.ALIGN_CENTER);
                                doc.Add(tableheading);
                                tableheading = new Paragraph("Date : " + System.DateTime.Now.ToString("dd-MMM-yyyy"), MyFont);
                                tableheading.Font.Size = 10;
                                tableheading.Alignment = (Element.ALIGN_RIGHT);
                                doc.Add(tableheading);
                                doc.Add(new Paragraph(Environment.NewLine));
                                doc.Add(new Paragraph(Environment.NewLine));
                                tableheading = new Paragraph("Letter for reassigned for " + DT1.Rows[0]["Permission For"].ToString(), MyFont);
                                tableheading.Font.Size = 11;
                                tableheading.Alignment = (Element.ALIGN_CENTER);
                                doc.Add(tableheading);
                                doc.Add(new Paragraph(Environment.NewLine));
                                string address = "";
                                if (DT1.Rows[0]["Postal_Address1"].ToString() == "")
                                {
                                    address = "";
                                }
                                else
                                {
                                    address = ", " + DT1.Rows[0]["Postal_Address1"].ToString();
                                }
                                sideheading = new Paragraph("The  application no " + DT1.Rows[0]["RequestedID"] + ", " + DT1.Rows[0]["Application Date"] + " Submitted  by " + DT1.Rows[0]["Entered By"].ToString() + "" + address + " has been reassigned due to following reason.");
                                sideheading.Font.Size = 10;
                                sideheading.Alignment = (Element.ALIGN_JUSTIFIED);
                                doc.Add(sideheading);
                                doc.Add(new Paragraph(Environment.NewLine));
                                pdfTable = new PdfPTable(1);
                                pdfTable.DefaultCell.Padding = 1;
                                pdfTable.WidthPercentage = 95;
                                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                                int count = DT3.Rows.Count;
                                DT3.Columns.Remove("RequestedID");
                                //   DT2.Columns.Remove("KhasraNo");
                                DT3.AcceptChanges();
                                string[,] arrPdfData = new string[count, 2];
                                // arrPdfData[0, 0] = "S.no ";
                                arrPdfData[0, 1] = "Reasons:-";
                                //  pdfTable.AddCell("S.no ");
                                colHeading = new Phrase("Reasons:-");
                                colHeading.Font.Size = 11;
                                cell = new PdfPCell(new Phrase(colHeading));
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                cell.Border = 0;
                                pdfTable.AddCell(cell);
                                for (int xid = 0; xid < count; xid++)
                                {
                                    for (int yid = 0; yid < 1; yid++)
                                    {
                                        arrPdfData[xid, yid] = DT3.Rows[xid][yid].ToString();
                                        colHeading = new Phrase(arrPdfData[xid, yid]);
                                        colHeading.Font.Size = 10;
                                        cell = new PdfPCell(new Phrase(colHeading));
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        cell.Border = 0;
                                        pdfTable.AddCell(cell);
                                    }
                                }
                                doc.Add(pdfTable);
                                pdfTable = new PdfPTable(4);
                                pdfTable.DefaultCell.Padding = 1;
                                pdfTable.WidthPercentage = 95;
                                pdfTable.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
                                doc.Add(new Paragraph(Environment.NewLine));
                                doc.Add(new Paragraph(Environment.NewLine));
                                tableheading = new Paragraph(DT1.Rows[0]["Designation"].ToString(), MyFont);
                                tableheading.Font.Size = 10;
                                tableheading.Alignment = (Element.ALIGN_RIGHT);
                                doc.Add(tableheading);
                                tableheading = new Paragraph(DT1.Rows[0]["Name"].ToString(), MyFont);
                                tableheading.Font.Size = 10;
                                tableheading.Alignment = (Element.ALIGN_RIGHT);
                                doc.Add(tableheading);
                                doc.Add(new Paragraph(Environment.NewLine));
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
                            }
                        }
                    }
                }
                return RedirectToAction("dashboard", "dashboard");
                // return new RazorPDF.PdfResult(pdfData, "PDFAfterApproval");
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
    /// <summary>
    /// this class is used to save Favourit list
    /// </summary>
    class FavouritList
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string FavItem { get; set; }
        public string ServiceId { get; set; }
    }
}
