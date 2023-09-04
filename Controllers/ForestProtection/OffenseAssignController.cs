
//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : OffenseAssignController
//  Description  : File contains calling functions from UI
//  Date Created : 29-Mar-2016
//  History      : Add the Details of 3 tabs (Warrant, Seized item and Crime scene )
//  Version      : 1.0
//  Author       : Raj kumar
//  Modified By  : Raj kumar
//  History      : Code change for add the new form File court case and edit functionlity for all tabs
//  Modified On  : 19-Apr-2016
//  Reviewed By  : 
//  Reviewed On  : 

///Bug No-465,468,469,473




using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.ForestProtection;
using System.Data.SqlClient;
using System.IO;


namespace FMDSS.Controllers.ForestProtection
{
    public class OffenseAssignController : BaseController
    {
        List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails> SeizedIteam1 = new List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails>();
        List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails> SeizedIteam5 = new List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails>();
        List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails> SeizedIteam6 = new List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails>();
        List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails> SeizedIteam7 = new List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails>();
        List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails> SeizedIteam8 = new List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails>();
        List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails> SeizedIteam9 = new List<FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails>();
 
        List<FMDSS.Models.ForestProtection.OffenseAssign.CompoundingDetails> CompoundIteam1 = new List<FMDSS.Models.ForestProtection.OffenseAssign.CompoundingDetails>();//
        List<FMDSS.Models.ForestProtection.OffenseAssign.WitnessDetail> WitnessDetails = new List<FMDSS.Models.ForestProtection.OffenseAssign.WitnessDetail>();
        List<FMDSS.Models.ForestProtection.OffenseAssign.OffenderDetail> OffenderDetails = new List<FMDSS.Models.ForestProtection.OffenseAssign.OffenderDetail>();
        // GET: /OffenseAssign/
        int ModuleID = 4;
        public ActionResult OffenseAssign()
        {
            
          
            DataTable dtOfficerDesignation = new DataTable();
            List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
            OffenseAssign _objModel = new OffenseAssign();
            List<OffenseAssign> Offenderdata = new List<OffenseAssign>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {                                
                    DataSet dtf = _objModel.GetViewExistingRecords();
                   
                            foreach (DataRow dr in dtf.Tables[0].Rows)
                                Offenderdata.Add(
                                    new OffenseAssign()
                                    {
                                        District = dr["District"].ToString(),
                                        UserRole = dr["UserRole"].ToString(),
                                        OffenseCode = dr["OffenseCode"].ToString(),
                                        OffensePlace = dr["OffensePlace"].ToString(),
                                        OffenseDate = dr["OffenseDate"].ToString(),
                                        OffenseTime = dr["OffenseTime"].ToString(),
                                        Status = Convert.ToInt32(dr["Status"].ToString()),
                                        OffenseDescription=dr["Description"].ToString()
                                    });                     
                        ViewData["OffenderList"] = Offenderdata;                  

                    dtOfficerDesignation = _objModel.GetOfficerDesignation();
                    foreach (System.Data.DataRow dr in dtOfficerDesignation.Rows)
                    {
                        lstOfficerDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["EmpDesignation"].ToString() });
                    }
                    ViewBag.OfficerDesignation = lstOfficerDesignation;
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, Convert.ToInt64(Session["UserID"].ToString()));
            }
            return View();
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
                dal.Fill(dsOfficerDesig, "Sp_FPM_GetFOfficerDesig", paramBlock);
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

        [HttpPost]
        public ActionResult Submit(FormCollection form, string Command, HttpPostedFileBase fileUpload)
        {
            OffenseAssign _objModel = new OffenseAssign();
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = "~/ForestProtectionDocument/";
            try {
                if (Command == "Forward")
                {
                    DataSet ds = new DataSet();
                    _objModel.OffenseCode = form["hdnOffenseCode"].ToString();
                    if (form["AssignDescription"] != null)
                    {
                        _objModel.AssignDescription = form["AssignDescription"].ToString();
                    }
                    else {
                        _objModel.AssignDescription = "";
                    }
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        FileName = Path.GetFileName(fileUpload.FileName);
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        _objModel.fileUpload = path;
                        fileUpload.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        _objModel.fileUpload = "";
                    }                    
                    ds = _objModel.SubmitDFO_Forward(form["dropForester"].ToString());
                }                  
              }
            catch (Exception ex) { }
            return RedirectToAction("OffenseAssign");
        }

        public ActionResult ViewDetails()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ViewDetails(string OffenseCode,string UserRole)
        {
            try
            {
                #region Details
                OffenseAssign OFA = new OffenseAssign();
                OffenseAssign OFA1 = new OffenseAssign();
                DataSet ds = new DataSet();
                ds = OFA1.GetViewDetails(OffenseCode, UserRole);
                if (ds.Tables.Count > 0) {

                    if (ds.Tables[0].Columns.Contains("Circle"))
                    {
                        OFA.Circle = ds.Tables[0].Rows[0]["Circle"].ToString();
                        OFA.Circle = OFA.Circle == null ? "N/A" : OFA.Circle;
                    }
                    else {
                        OFA.Circle = OFA.Circle == null ? "N/A" : OFA.Circle;
                    }
                    if (ds.Tables[0].Columns.Contains("Division"))
                    {
                        OFA.Division = ds.Tables[0].Rows[0]["Division"].ToString();
                        OFA.Division = OFA.Division == null ? "N/A" : OFA.Division;
                    }
                    else {
                        OFA.Division = OFA.Division == null ? "N/A" : OFA.Division;
                    }
                    if (ds.Tables[0].Columns.Contains("District"))
                    {
                        OFA.District = ds.Tables[0].Rows[0]["District"].ToString();
                        OFA.District = OFA.District == null ? "N/A" : OFA.District;
                    }
                    else {
                        OFA.District = OFA.District == null ? "N/A" : OFA.District;
                    }
                    if (ds.Tables[0].Columns.Contains("Block"))
                    {
                        OFA.Block = ds.Tables[0].Rows[0]["Block"].ToString();
                        OFA.Block = OFA.Block == null ? "N/A" : OFA.Block;
                    }
                    else {
                        OFA.Block = OFA.Block == null ? "N/A" : OFA.Block;
                    }
                    if (ds.Tables[0].Columns.Contains("GP_NAME"))
                    {
                        OFA.GPName = ds.Tables[0].Rows[0]["GP_NAME"].ToString();
                        OFA.GPName = OFA.GPName == null ? "N/A" : OFA.GPName;
                    }
                    else {
                        OFA.GPName = OFA.GPName == null ? "N/A" : OFA.GPName;
                    }
                    if (ds.Tables[0].Columns.Contains("VILL_NAME"))
                    {
                        OFA.Village = ds.Tables[0].Rows[0]["VILL_NAME"].ToString();
                        OFA.Village = OFA.Village == null ? "N/A" : OFA.Village;
                    }
                    else {
                        OFA.Village = OFA.Village == null ? "N/A" : OFA.Village;
                    }
                    if (ds.Tables[0].Columns.Contains("RANGE"))
                    {
                        OFA.Range = ds.Tables[0].Rows[0]["RANGE"].ToString();
                        OFA.Range = OFA.Range == null ? "N/A" : OFA.Range;
                    }
                    else {
                        OFA.Range = OFA.Range == null ? "N/A" : OFA.Range;
                    }
                    if (ds.Tables[0].Columns.Contains("Tehsil"))
                    {
                        OFA.Tehsil = ds.Tables[0].Rows[0]["Tehsil"].ToString();
                        OFA.Tehsil = OFA.Tehsil == null ? "N/A" : OFA.Tehsil;
                    }
                    else {
                        OFA.Tehsil = OFA.Tehsil == null ? "N/A" : OFA.Tehsil;
                    }
                    if (ds.Tables[0].Columns.Contains("Naka"))
                    {
                        OFA.Naka = ds.Tables[0].Rows[0]["Naka"].ToString();
                        OFA.Naka = OFA.Naka == null ? "N/A" : OFA.Naka;
                    }
                    else {
                        OFA.Naka = OFA.Naka == null ? "N/A" : OFA.Naka;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenseDate"))
                    {
                        OFA.OffenseDate = ds.Tables[0].Rows[0]["OffenseDate"].ToString();
                        OFA.OffenseDate = OFA.OffenseDate == null ? "N/A" : OFA.OffenseDate;
                    }
                    else {
                        OFA.OffenseDate = OFA.OffenseDate == null ? "N/A" : OFA.OffenseDate;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenseTime"))
                    {
                        OFA.OffenseTime = ds.Tables[0].Rows[0]["OffenseTime"].ToString();
                        OFA.OffenseTime = OFA.OffenseTime == null ? "N/A" : OFA.OffenseTime;
                    }
                    else
                    {
                        OFA.OffenseTime = OFA.OffenseTime == null ? "N/A" : OFA.OffenseTime;
                    }
                  
                    if (ds.Tables[0].Columns.Contains("Latitude"))
                    {
                        OFA.Latitude = ds.Tables[0].Rows[0]["Latitude"].ToString();
                        OFA.Latitude = OFA.Latitude == null ? "N/A" : OFA.Latitude;
                    }
                    else
                    {
                        OFA.Latitude = OFA.Latitude == null ? "N/A" : OFA.Latitude;
                    }
                    if (ds.Tables[0].Columns.Contains("Longitude"))
                    {
                        OFA.Longitude = ds.Tables[0].Rows[0]["Longitude"].ToString();
                        OFA.Longitude = OFA.Longitude == null ? "N/A" : OFA.Longitude;
                    }
                    else
                    {
                        OFA.Longitude = OFA.Longitude == null ? "N/A" : OFA.Longitude;
                    }
                    if (ds.Tables[0].Columns.Contains("DistanceFromNaka"))
                    {
                        OFA.DistanceFrmNaka = ds.Tables[0].Rows[0]["DistanceFromNaka"].ToString();
                        OFA.DistanceFrmNaka = OFA.DistanceFrmNaka == null ? "N/A" : OFA.DistanceFrmNaka;
                    }
                    else {
                        OFA.DistanceFrmNaka = OFA.DistanceFrmNaka == null ? "N/A" : OFA.DistanceFrmNaka;
                    }
                    if (ds.Tables[0].Columns.Contains("OffensePlace"))
                    {
                        OFA.OffensePlace = ds.Tables[0].Rows[0]["OffensePlace"].ToString();
                        OFA.OffensePlace = OFA.OffensePlace == null ? "N/A" : OFA.OffensePlace;
                    }
                    else {
                        OFA.OffensePlace = OFA.OffensePlace == null ? "N/A" : OFA.OffensePlace;
                    }
                    if (ds.Tables[0].Columns.Contains("Description"))
                    {
                        OFA.OffenseDescription = ds.Tables[0].Rows[0]["Description"].ToString();
                        OFA.OffenseDescription = OFA.OffenseDescription == null ? "N/A" : OFA.OffenseDescription;
                    }
                    else
                    {
                        OFA.OffenseDescription = OFA.OffenseDescription == null ? "N/A" : OFA.OffenseDescription;
                    }
                    if (ds.Tables[0].Columns.Contains("FOCategory"))
                    {
                        OFA.OffenseCategory = ds.Tables[0].Rows[0]["FOCategory"].ToString();
                        OFA.OffenseCategory = OFA.OffenseCategory == null ? "N/A" : OFA.OffenseCategory;
                    }
                    else
                    {
                        OFA.OffenseCategory = OFA.OffenseCategory == null ? "N/A" : OFA.OffenseCategory;
                    }
                    if (ds.Tables[0].Columns.Contains("Wildlife_Protection_Act"))
                    {
                        OFA.WildlifeProtection = ds.Tables[0].Rows[0]["Wildlife_Protection_Act"].ToString();
                        OFA.WildlifeProtection = OFA.WildlifeProtection == null ? "N/A" : OFA.WildlifeProtection;
                    }
                    else
                    {
                        OFA.WildlifeProtection = OFA.WildlifeProtection == null ? "N/A" : OFA.WildlifeProtection;
                    }
                    if (ds.Tables[0].Columns.Contains("Forest_Protection_Act"))
                    {
                        OFA.ForestProtection = ds.Tables[0].Rows[0]["Forest_Protection_Act"].ToString();
                        OFA.ForestProtection = OFA.ForestProtection == null ? "N/A" : OFA.ForestProtection;
                    }
                    else
                    {
                        OFA.ForestProtection = OFA.ForestProtection == null ? "N/A" : OFA.ForestProtection;
                    }
                    if (ds.Tables[0].Columns.Contains("Offense_Severity"))
                    {
                        OFA.OffenseSeverity = ds.Tables[0].Rows[0]["Offense_Severity"].ToString();
                        OFA.OffenseSeverity = OFA.OffenseSeverity == null ? "N/A" : OFA.OffenseSeverity;
                    }
                    else
                    {
                        OFA.OffenseSeverity = OFA.OffenseSeverity == null ? "N/A" : OFA.OffenseSeverity;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderType"))
                    {
                        OFA.OffenderType = ds.Tables[0].Rows[0]["OffenderType"].ToString();
                        OFA.OffenderType = OFA.OffenderType == null ? "N/A" : OFA.OffenderType;
                    }
                    else
                    {
                        OFA.OffenderType = OFA.OffenderType == null ? "N/A" : OFA.OffenderType;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderName"))
                    {
                        OFA.OffenderName = ds.Tables[0].Rows[0]["OffenderName"].ToString();
                        OFA.OffenderName = OFA.OffenderName == null ? "N/A" : OFA.OffenderName;
                    }
                    else
                    {
                        OFA.OffenderName = OFA.OffenderName == null ? "N/A" : OFA.OffenderName;
                    }
                    if (ds.Tables[0].Columns.Contains("FatherName"))
                    {
                        OFA.OffenderFatherName = ds.Tables[0].Rows[0]["FatherName"].ToString();
                        OFA.OffenderFatherName = OFA.OffenderName == null ? "N/A" : OFA.OffenderName;
                    }
                    else
                    {
                        OFA.OffenderFatherName = OFA.OffenderFatherName == null ? "N/A" : OFA.OffenderFatherName;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderCaste"))
                    {
                        OFA.OffenderCaste = ds.Tables[0].Rows[0]["OffenderCaste"].ToString();
                        OFA.OffenderCaste = OFA.OffenderCaste == null ? "N/A" : OFA.OffenderCaste;
                    }
                    else
                    {
                        OFA.OffenderCaste = OFA.OffenderCaste == null ? "N/A" : OFA.OffenderCaste;
                    }
                    if (ds.Tables[0].Columns.Contains("ClothesWorn"))
                    {
                        OFA.OffenderClothesWorn = ds.Tables[0].Rows[0]["ClothesWorn"].ToString();
                        OFA.OffenderClothesWorn = OFA.OffenderClothesWorn == null ? "N/A" : OFA.OffenderClothesWorn;
                    }
                    else
                    {
                        OFA.OffenderClothesWorn = OFA.OffenderClothesWorn == null ? "N/A" : OFA.OffenderClothesWorn;
                    }
                    if (ds.Tables[0].Columns.Contains("ClothesColor"))
                    {
                        OFA.OffenderClothesColor = ds.Tables[0].Rows[0]["ClothesColor"].ToString();
                        OFA.OffenderClothesColor = OFA.OffenderClothesColor == null ? "N/A" : OFA.OffenderClothesColor;
                    }
                    else
                    {
                        OFA.OffenderClothesColor = OFA.OffenderClothesColor == null ? "N/A" : OFA.OffenderClothesColor;
                    }
                    if (ds.Tables[0].Columns.Contains("PhysicalAppearance"))
                    {
                        OFA.OffenderPhysicalAppearance = ds.Tables[0].Rows[0]["PhysicalAppearance"].ToString();
                        OFA.OffenderPhysicalAppearance = OFA.OffenderPhysicalAppearance == null ? "N/A" : OFA.OffenderPhysicalAppearance;
                    }
                    else
                    {
                        OFA.OffenderPhysicalAppearance = OFA.OffenderPhysicalAppearance == null ? "N/A" : OFA.OffenderPhysicalAppearance;
                    }
                    if (ds.Tables[0].Columns.Contains("Height"))
                    {
                        OFA.OffenderHeight = ds.Tables[0].Rows[0]["Height"].ToString();
                        OFA.OffenderHeight = OFA.OffenderHeight == null ? "N/A" : OFA.OffenderHeight;
                    }
                    else
                    {
                        OFA.OffenderHeight = OFA.OffenderHeight == null ? "N/A" : OFA.OffenderHeight;
                    }
                    if (ds.Tables[0].Columns.Contains("OtherSpecialDetails"))
                    {
                        OFA.OffenderOtherSpecialDetail = ds.Tables[0].Rows[0]["OtherSpecialDetails"].ToString();
                        OFA.OffenderOtherSpecialDetail = OFA.OffenderOtherSpecialDetail == null ? "N/A" : OFA.OffenderOtherSpecialDetail;
                    }
                    else
                    {
                        OFA.OffenderOtherSpecialDetail = OFA.OffenderOtherSpecialDetail == null ? "N/A" : OFA.OffenderOtherSpecialDetail;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderPincode"))
                    {
                        OFA.OffenderPincode = ds.Tables[0].Rows[0]["OffenderPincode"].ToString();
                        OFA.OffenderPincode = OFA.OffenderPincode == null ? "N/A" : OFA.OffenderPincode;
                    }
                    else
                    {
                        OFA.OffenderPincode = OFA.OffenderPincode == null ? "N/A" : OFA.OffenderPincode;
                    }
                    if (ds.Tables[0].Columns.Contains("VillageCode"))
                    {
                        OFA.OffenderVillage = ds.Tables[0].Rows[0]["VillageCode"].ToString();
                        OFA.OffenderVillage = OFA.OffenderVillage == null ? "N/A" : OFA.OffenderVillage;
                    }
                    else
                    {
                        OFA.OffenderVillage = OFA.OffenderVillage == null ? "N/A" : OFA.OffenderVillage;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderDistrict"))
                    {
                        OFA.OffenderDistrict = ds.Tables[0].Rows[0]["OffenderDistrict"].ToString();
                        OFA.OffenderDistrict = OFA.OffenderDistrict == null ? "N/A" : OFA.OffenderDistrict;
                    }
                    else
                    {
                        OFA.OffenderDistrict = OFA.OffenderDistrict == null ? "N/A" : OFA.OffenderDistrict;
                    }
                    if (ds.Tables[0].Columns.Contains("EmailID"))
                    {
                        OFA.OffenderEmailId = ds.Tables[0].Rows[0]["EmailID"].ToString();
                        OFA.OffenderEmailId = OFA.OffenderDistrict == null ? "N/A" : OFA.OffenderDistrict;
                    }
                    else
                    {
                        OFA.OffenderEmailId = OFA.OffenderEmailId == null ? "N/A" : OFA.OffenderEmailId;
                    }                    
                    if (ds.Tables[0].Columns.Contains("OffenderAddress"))
                    {
                        OFA.OffenderAddress = ds.Tables[0].Rows[0]["OffenderAddress"].ToString();
                        OFA.OffenderAddress = OFA.OffenderAddress == null ? "N/A" : OFA.OffenderAddress;
                    }
                    else
                    {
                        OFA.OffenderAddress = OFA.OffenderAddress == null ? "N/A" : OFA.OffenderAddress;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderPhoneNo"))
                    {
                        OFA.OffenderPhoneNo = ds.Tables[0].Rows[0]["OffenderPhoneNo"].ToString();
                        OFA.OffenderPhoneNo = OFA.OffenderPhoneNo == null ? "N/A" : OFA.OffenderPhoneNo;
                    }
                    else
                    {
                        OFA.OffenderPhoneNo = OFA.OffenderPhoneNo == null ? "N/A" : OFA.OffenderPhoneNo;
                    }
                    if (ds.Tables[0].Columns.Contains("PoliceStation"))
                    {
                        OFA.PoliceStation = ds.Tables[0].Rows[0]["PoliceStation"].ToString();
                        OFA.PoliceStation = OFA.PoliceStation == null ? "N/A" : OFA.PoliceStation;
                    }
                    else
                    {
                        OFA.PoliceStation = OFA.PoliceStation == null ? "N/A" : OFA.PoliceStation;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderStatementDate"))
                    {
                        OFA.OffenderStatementDate = ds.Tables[0].Rows[0]["OffenderStatementDate"].ToString();
                        OFA.OffenderStatementDate = OFA.OffenderStatementDate == null ? "N/A" : OFA.OffenderStatementDate;
                    }
                    else
                    {
                        OFA.OffenderStatementDate = OFA.OffenderStatementDate == null ? "N/A" : OFA.OffenderStatementDate;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderStatement"))
                    {
                        OFA.OffenderStatement = ds.Tables[0].Rows[0]["OffenderStatement"].ToString();
                        OFA.OffenderStatement = OFA.OffenderStatement == null ? "N/A" : OFA.OffenderStatement;
                    }
                    else
                    {
                        OFA.OffenderStatement = OFA.OffenderStatement == null ? "N/A" : OFA.OffenderStatement;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderStatementDoc"))
                    {
                        string strDoc = ds.Tables[0].Rows[0]["OffenderStatementDoc"].ToString();
                        string[] strDocsplit = strDoc.Split('/');
                        OFA.OffenderStatementDoc = strDocsplit[strDocsplit.Length - 1];                     
                        OFA.OffenderStatementDoc = OFA.OffenderStatementDoc == null ? "N/A" : OFA.OffenderStatementDoc;
                    }
                    else
                    {
                        OFA.OffenderStatementDoc = OFA.OffenderStatementDoc == null ? "N/A" : OFA.OffenderStatementDoc;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessName"))
                    {
                        OFA.WitnessName = ds.Tables[0].Rows[0]["WitnessName"].ToString();
                        OFA.WitnessName = OFA.WitnessName == null ? "N/A" : OFA.WitnessName;
                    }
                    else
                    {
                        OFA.WitnessName = OFA.WitnessName == null ? "N/A" : OFA.WitnessName;
                    }
                    if (ds.Tables[0].Columns.Contains("FatherName"))
                    {
                        OFA.WitnessFatherName = ds.Tables[0].Rows[0]["FatherName"].ToString();
                        OFA.WitnessFatherName = OFA.WitnessFatherName == null ? "N/A" : OFA.WitnessFatherName;
                    }
                    else
                    {
                        OFA.WitnessFatherName = OFA.WitnessFatherName == null ? "N/A" : OFA.WitnessFatherName;
                    }
                    if (ds.Tables[0].Columns.Contains("Caste"))
                    {
                        OFA.WitnessCaste = ds.Tables[0].Rows[0]["Caste"].ToString();
                        OFA.WitnessCaste = OFA.WitnessCaste == null ? "N/A" : OFA.WitnessCaste;
                    }
                    else
                    {
                        OFA.WitnessCaste = OFA.WitnessCaste == null ? "N/A" : OFA.WitnessCaste;
                    }
                    if (ds.Tables[0].Columns.Contains("Caste"))
                    {
                        OFA.WitnessCaste = ds.Tables[0].Rows[0]["Caste"].ToString();
                        OFA.WitnessCaste = OFA.WitnessCaste == null ? "N/A" : OFA.WitnessCaste;
                    }
                    else
                    {
                        OFA.WitnessCaste = OFA.WitnessCaste == null ? "N/A" : OFA.WitnessCaste;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessAddress"))
                    {
                        OFA.WitnessAddress = ds.Tables[0].Rows[0]["WitnessAddress"].ToString();
                        OFA.WitnessAddress = OFA.WitnessAddress == null ? "N/A" : OFA.WitnessAddress;
                    }
                    else
                    {
                        OFA.WitnessAddress = OFA.WitnessAddress == null ? "N/A" : OFA.WitnessAddress;
                    }
                    if (ds.Tables[0].Columns.Contains("Pincode"))
                    {
                        OFA.WitnessPincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                        OFA.WitnessPincode = OFA.WitnessPincode == null ? "N/A" : OFA.WitnessPincode;
                    }
                    else
                    {
                        OFA.WitnessPincode = OFA.WitnessPincode == null ? "N/A" : OFA.WitnessPincode;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessVillage"))
                    {
                        OFA.WitnessVillage = ds.Tables[0].Rows[0]["WitnessVillage"].ToString();
                        OFA.WitnessVillage = OFA.WitnessVillage == null ? "N/A" : OFA.WitnessVillage;
                    }
                    else
                    {
                        OFA.WitnessVillage = OFA.WitnessVillage == null ? "N/A" : OFA.WitnessVillage;
                    }
                    if (ds.Tables[0].Columns.Contains("witnessDistrict"))
                    {
                        OFA.WitnessDistrict = ds.Tables[0].Rows[0]["witnessDistrict"].ToString();
                        OFA.WitnessDistrict = OFA.WitnessDistrict == null ? "N/A" : OFA.WitnessDistrict;
                    }
                    else
                    {
                        OFA.WitnessDistrict = OFA.WitnessDistrict == null ? "N/A" : OFA.WitnessDistrict;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessPhoneNo"))
                    {
                        OFA.WitnessPhoneNo = ds.Tables[0].Rows[0]["WitnessPhoneNo"].ToString();
                        OFA.WitnessPhoneNo = OFA.WitnessPhoneNo == null ? "N/A" : OFA.WitnessPhoneNo;
                    }
                    else
                    {
                        OFA.WitnessPhoneNo = OFA.WitnessPhoneNo == null ? "N/A" : OFA.WitnessPhoneNo;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessIdType"))
                    {
                        OFA.WitnessIDType = ds.Tables[0].Rows[0]["WitnessIdType"].ToString();
                        OFA.WitnessIDType = OFA.WitnessIDType == null ? "N/A" : OFA.WitnessIDType;
                    }
                    else
                    {
                        OFA.WitnessIDType = OFA.WitnessIDType == null ? "N/A" : OFA.WitnessIDType;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessIdProofUrl"))
                    {
                        string strDoc = ds.Tables[0].Rows[0]["WitnessIdProofUrl"].ToString();
                        string[] strDocsplit = strDoc.Split('/');
                        OFA.WitnessIDProofURL = strDocsplit[strDocsplit.Length - 1];
                        OFA.WitnessIDProofURL = OFA.WitnessIDProofURL == null ? "N/A" : OFA.WitnessIDProofURL;
                    }
                    else
                    {
                        OFA.WitnessIDProofURL = OFA.WitnessIDProofURL == null ? "N/A" : OFA.WitnessIDProofURL;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessAge"))
                    {
                        OFA.WitnessAge = ds.Tables[0].Rows[0]["WitnessAge"].ToString();
                        OFA.WitnessAge = OFA.WitnessAge == null ? "N/A" : OFA.WitnessAge;
                    }
                    else
                    {
                        OFA.WitnessAge = OFA.WitnessAge == null ? "N/A" : OFA.WitnessAge;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessStatementDate"))
                    {
                        OFA.WitnessStatementDate = ds.Tables[0].Rows[0]["WitnessStatementDate"].ToString();
                        OFA.WitnessStatementDate = OFA.WitnessStatementDate == null ? "N/A" : OFA.WitnessStatementDate;
                    }
                    else
                    {
                        OFA.WitnessStatementDate = OFA.WitnessStatementDate == null ? "N/A" : OFA.WitnessStatementDate;
                    }
                    if (ds.Tables[0].Columns.Contains("WitnessStatement"))
                    {
                        OFA.WitnessStatement = ds.Tables[0].Rows[0]["WitnessStatement"].ToString();
                        OFA.WitnessStatement = OFA.WitnessStatement == null ? "N/A" : OFA.WitnessStatement;
                    }
                    else
                    {
                        OFA.WitnessStatement = OFA.WitnessStatement == null ? "N/A" : OFA.WitnessStatement;
                    }
                    if (ds.Tables[0].Columns.Contains("SignedStatementURL"))
                    {
                        string strDoc = ds.Tables[0].Rows[0]["SignedStatementURL"].ToString();
                        string[] strDocsplit = strDoc.Split('/');
                        OFA.SignedStatementURL = strDocsplit[strDocsplit.Length - 1];                              
                        OFA.SignedStatementURL = OFA.SignedStatementURL == null ? "N/A" : OFA.SignedStatementURL;
                    }
                    else
                    {
                        OFA.SignedStatementURL = OFA.SignedStatementURL == null ? "N/A" : OFA.SignedStatementURL;
                    }                                                                                          
                }
                return Json(OFA, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ViewDetailsOnClick(string OffenseCode, string UserRole)
        {
            try
            {
                FMDSS.Models.ForestProtection.OffenseAssign.OffenceDetal offencedetail = null;
                FMDSS.Models.ForestProtection.OffenseAssign.WitnessDetail objWitness = null;
                FMDSS.Models.ForestProtection.OffenseAssign.OffenderDetail objOffender = null;
                FMDSS.Models.ForestProtection.OffenseAssign.CompoundingDetails objCompound = null;
                FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails objSeized = null;
                #region Details
                //OffenseAssign OFA = new OffenseAssign();
                OffenseAssign OFA1 = new OffenseAssign();
                DataSet ds = new DataSet();
                ds = OFA1.FPM_GetOffenceDetailByOffenceCode(OffenseCode, UserRole);
                if (ds.Tables.Count > 0)
                {
                    offencedetail = new Models.ForestProtection.OffenseAssign.OffenceDetal();

                    if (ds.Tables[0].Columns.Contains("CrimePhotoURL1"))
                    {
                        string str1 = ds.Tables[0].Rows[0]["CrimePhotoURL1"].ToString();
                        if (str1 != "")
                        {
                            int length = str1.Length;
                            str1 = str1.Substring(2, length - 2);
                            offencedetail.CrimePhotoURL1 = str1;
                            offencedetail.CrimePhotoURL1 = offencedetail.CrimePhotoURL1 == null ? "N/A" : offencedetail.CrimePhotoURL1;
                        }

                    }
                    else
                    {
                        offencedetail.CrimePhotoURL1 = offencedetail.CrimePhotoURL1 == null ? "N/A" : offencedetail.CrimePhotoURL1;
                    }
                    if (ds.Tables[0].Columns.Contains("CrimePhotoURL2"))
                    {
                        string str1 = ds.Tables[0].Rows[0]["CrimePhotoURL2"].ToString();
                        if (str1 != "")
                        {
                            int length = str1.Length;
                            str1 = str1.Substring(2, length - 2);
                            offencedetail.CrimePhotoURL2 = str1;
                            offencedetail.CrimePhotoURL2 = offencedetail.CrimePhotoURL2 == null ? "N/A" : offencedetail.CrimePhotoURL2;
                        }
                        // offencedetail.CrimePhotoURL2 = ds.Tables[0].Rows[0]["CrimePhotoURL2"].ToString();

                    }
                    else
                    {
                        offencedetail.CrimePhotoURL2 = offencedetail.CrimePhotoURL2 == null ? "N/A" : offencedetail.CrimePhotoURL2;
                    }
                    if (ds.Tables[0].Columns.Contains("CrimePhotoURL3"))
                    {
                        string str1 = ds.Tables[0].Rows[0]["CrimePhotoURL3"].ToString();
                        if (str1 != "")
                        {
                            int length = str1.Length;
                            str1 = str1.Substring(2, length - 2);
                            offencedetail.CrimePhotoURL3 = str1;
                            offencedetail.CrimePhotoURL3 = offencedetail.CrimePhotoURL3 == null ? "N/A" : offencedetail.CrimePhotoURL3;
                        }
                        // offencedetail.CrimePhotoURL3 = ds.Tables[0].Rows[0]["CrimePhotoURL3"].ToString();

                    }
                    else
                    {
                        offencedetail.CrimePhotoURL3 = offencedetail.CrimePhotoURL3 == null ? "N/A" : offencedetail.CrimePhotoURL3;
                    }
                    if (ds.Tables[0].Columns.Contains("IsCompoundable"))
                    {
                        offencedetail.IsCompoundable = ds.Tables[0].Rows[0]["IsCompoundable"].ToString();
                        offencedetail.IsCompoundable = offencedetail.IsCompoundable == null ? "N/A" : offencedetail.IsCompoundable;
                    }
                    else
                    {
                        offencedetail.IsCompoundable = offencedetail.IsCompoundable == null ? "N/A" : offencedetail.IsCompoundable;
                    }
                    if (ds.Tables[0].Columns.Contains("UserRole"))
                    {
                        offencedetail.UserRole = ds.Tables[0].Rows[0]["UserRole"].ToString();
                        offencedetail.UserRole = offencedetail.UserRole == null ? "N/A" : offencedetail.UserRole;
                    }
                    else
                    {
                        offencedetail.UserRole = offencedetail.UserRole == null ? "N/A" : offencedetail.UserRole;
                    }
                    if (ds.Tables[0].Columns.Contains("SettlementAmount"))
                    {
                        offencedetail.SettlementAmount = ds.Tables[0].Rows[0]["SettlementAmount"].ToString();
                        offencedetail.SettlementAmount = offencedetail.SettlementAmount == null ? "N/A" : offencedetail.SettlementAmount;
                    }
                    else
                    {
                        offencedetail.SettlementAmount = offencedetail.SettlementAmount == null ? "N/A" : offencedetail.SettlementAmount;
                    }
                    if (ds.Tables[0].Columns.Contains("AmountPaid"))
                    {
                        offencedetail.AmountPaid = ds.Tables[0].Rows[0]["AmountPaid"].ToString();
                        offencedetail.AmountPaid = offencedetail.AmountPaid == null ? "N/A" : offencedetail.AmountPaid;
                    }
                    else
                    {
                        offencedetail.AmountPaid = offencedetail.AmountPaid == null ? "N/A" : offencedetail.AmountPaid;
                    }
                    if (ds.Tables[0].Columns.Contains("ApplicantName"))
                    {
                        offencedetail.ApplicantName = ds.Tables[0].Rows[0]["ApplicantName"].ToString();
                        offencedetail.ApplicantName = offencedetail.ApplicantName == null ? "N/A" : offencedetail.ApplicantName;
                    }
                    else
                    {
                        offencedetail.ApplicantName = offencedetail.ApplicantName == null ? "N/A" : offencedetail.ApplicantName;
                    }
                    if (ds.Tables[0].Columns.Contains("Block"))
                    {
                        offencedetail.Block = ds.Tables[0].Rows[0]["Block"].ToString();
                        offencedetail.Block = offencedetail.Block == null ? "N/A" : offencedetail.Block;
                    }
                    else
                    {
                        offencedetail.Block = offencedetail.Block == null ? "N/A" : offencedetail.Block;
                    }
                    if (ds.Tables[0].Columns.Contains("CIRCLE_NAME"))
                    {
                        offencedetail.CIRCLE_NAME = ds.Tables[0].Rows[0]["CIRCLE_NAME"].ToString();
                        offencedetail.CIRCLE_NAME = offencedetail.CIRCLE_NAME == null ? "N/A" : offencedetail.CIRCLE_NAME;
                    }
                    else
                    {
                        offencedetail.CIRCLE_NAME = offencedetail.CIRCLE_NAME == null ? "N/A" : offencedetail.CIRCLE_NAME;
                    }

                    if (ds.Tables[0].Columns.Contains("Description"))
                    {
                        offencedetail.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        offencedetail.Description = offencedetail.Description == null ? "N/A" : offencedetail.Description;
                    }
                    else
                    {
                        offencedetail.Description = offencedetail.Description == null ? "N/A" : offencedetail.Description;
                    }

                    if (ds.Tables[0].Columns.Contains("OffenseDate"))
                    {
                        offencedetail.OffenseDate = ds.Tables[0].Rows[0]["OffenseDate"].ToString();
                        offencedetail.OffenseDate = offencedetail.OffenseDate == null ? "N/A" : offencedetail.OffenseDate;
                    }
                    else
                    {
                        offencedetail.OffenseDate = offencedetail.OffenseDate == null ? "N/A" : offencedetail.OffenseDate;
                    }
                    if (ds.Tables[0].Columns.Contains("OffensePlace"))
                    {
                        offencedetail.OffensePlace = ds.Tables[0].Rows[0]["OffensePlace"].ToString();
                        offencedetail.OffensePlace = offencedetail.OffensePlace == null ? "N/A" : offencedetail.OffensePlace;
                    }
                    else
                    {
                        offencedetail.OffensePlace = offencedetail.OffensePlace == null ? "N/A" : offencedetail.OffensePlace;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenseTime"))
                    {
                        offencedetail.OffenseTime = ds.Tables[0].Rows[0]["OffenseTime"].ToString();
                        offencedetail.OffenseTime = offencedetail.OffenseTime == null ? "N/A" : offencedetail.OffenseTime;
                    }
                    else
                    {
                        offencedetail.OffenseTime = offencedetail.OffenseTime == null ? "N/A" : offencedetail.OffenseTime;
                    }
                    if (ds.Tables[0].Columns.Contains("DIV_NAME"))
                    {
                        offencedetail.DIV_NAME = ds.Tables[0].Rows[0]["DIV_NAME"].ToString();
                        offencedetail.DIV_NAME = offencedetail.DIV_NAME == null ? "N/A" : offencedetail.DIV_NAME;
                    }
                    else
                    {
                        offencedetail.DIV_NAME = offencedetail.DIV_NAME == null ? "N/A" : offencedetail.DIV_NAME;
                    }
                    if (ds.Tables[0].Columns.Contains("DfoDecision"))
                    {
                        offencedetail.DfoDecision = ds.Tables[0].Rows[0]["DfoDecision"].ToString();
                        offencedetail.DfoDecision = offencedetail.DfoDecision == null ? "N/A" : offencedetail.DfoDecision;
                    }
                    else
                    {
                        offencedetail.DfoDecision = offencedetail.DfoDecision == null ? "N/A" : offencedetail.DfoDecision;
                    }
                    if (ds.Tables[0].Columns.Contains("CaseStatus"))
                    {
                        offencedetail.CaseStatus = ds.Tables[0].Rows[0]["CaseStatus"].ToString();
                        offencedetail.CaseStatus = offencedetail.CaseStatus == null ? "N/A" : offencedetail.CaseStatus;
                    }
                    else
                    {
                        offencedetail.CaseStatus = offencedetail.CaseStatus == null ? "N/A" : offencedetail.CaseStatus;
                    }
                    if (ds.Tables[0].Columns.Contains("FineAmount"))
                    {
                        offencedetail.FineAmount = ds.Tables[0].Rows[0]["FineAmount"].ToString();
                        offencedetail.FineAmount = offencedetail.FineAmount == null ? "N/A" : offencedetail.FineAmount;
                    }
                    else
                    {
                        offencedetail.FineAmount = offencedetail.FineAmount == null ? "N/A" : offencedetail.FineAmount;
                    }
                    if (ds.Tables[0].Columns.Contains("OffenderPresent"))
                    {
                        offencedetail.OffenderPresent = ds.Tables[0].Rows[0]["OffenderPresent"].ToString();
                        offencedetail.OffenderPresent = offencedetail.OffenderPresent == null ? "N/A" : offencedetail.OffenderPresent;
                    }
                    else
                    {
                        offencedetail.OffenderPresent = offencedetail.OffenderPresent == null ? "N/A" : offencedetail.OffenderPresent;
                    }
                    if (ds.Tables[0].Columns.Contains("ItemSeized"))
                    {
                        offencedetail.ItemSeized = ds.Tables[0].Rows[0]["ItemSeized"].ToString();
                        offencedetail.ItemSeized = offencedetail.ItemSeized == null ? "N/A" : offencedetail.ItemSeized;
                    }
                    else
                    {
                        offencedetail.ItemSeized = offencedetail.ItemSeized == null ? "N/A" : offencedetail.ItemSeized;
                    }
                    if (ds.Tables[0].Columns.Contains("Compounding"))
                    {
                        offencedetail.Compounding = ds.Tables[0].Rows[0]["Compounding"].ToString();
                        offencedetail.Compounding = offencedetail.Compounding == null ? "N/A" : offencedetail.Compounding;
                    }
                    else
                    {
                        offencedetail.Compounding = offencedetail.Compounding == null ? "N/A" : offencedetail.Compounding;
                    }

                    foreach (DataRow dr in ds.Tables[9].Rows)
                    {
                        SeizedIteam9.Add(new FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails()
                        {
                            //ID = Convert.ToInt64(dr["ID"].ToString()),
                            OffenseCode = dr["OffenseCode"].ToString(),
                            sDate_Of_Visit = dr["Date_Of_Visit"].ToString(),
                            PlaceOfVisit = dr["PlaceOfVisit"].ToString(),
                            Description_of_Crime = dr["Description_of_Crime"].ToString(),
                            //Pictures_of_Crime1 = dr["Pictures_of_Crime1"].ToString(),
                            //Pictures_of_Crime2 = dr["Pictures_of_Crime2"].ToString(),
                            //Pictures_of_Crime3 = dr["Pictures_of_Crime3"].ToString(),
                            Village_Name = dr["VILL_NAME"].ToString(),
                            Range_Name = dr["RANGE_NAME"].ToString(),
                            IsComplete = dr["IsComplete"].ToString(),
                            //   EnteredBy = dr["EnteredBy"].ToString(),

                        });
                    }



                    foreach (DataRow dr in ds.Tables[8].Rows)
                    {
                        string DOC = dr["UploadDoc"].ToString();
                        if (DOC != "")
                        {
                            int length = DOC.Length;
                            DOC = DOC.Substring(2, length - 2);
                        }
                        SeizedIteam8.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails()
                          {
                              Name = dr["Name"].ToString(),
                              AnimalScientificName = dr["AnimalScientificName"].ToString(),
                              AnimalDescription = dr["AnimalDescription"].ToString(),

                              AnimalWeight = dr["AnimalWeight"].ToString(),
                              UploadedDoc = DOC,

                          });

                    }


                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        string DOC = dr["UploadDoc"].ToString();
                        if (DOC != "")
                        {
                            int length = DOC.Length;
                            DOC = DOC.Substring(2, length - 2);
                        }
                        SeizedIteam7.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails()
                          {
                              SpeciesName = dr["VehicleRegistrationNo"].ToString(),
                              ProduceType = dr["OwnerName"].ToString(),
                              Quantity = dr["VehicleMake"].ToString(),
                              VehicleModel = dr["VehicleModel"].ToString(),
                              ChassisNo = dr["ChassisNo"].ToString(),
                              EngineNo = dr["EngineNo"].ToString(),
                              PastOffenses = dr["PastOffenses"].ToString(),
                              CategoryName = dr["CategoryName"].ToString(),
                              UploadedDoc = DOC,
                          });

                    }


                    foreach (DataRow dr in ds.Tables[6].Rows)
                    {
                        string DOC = dr["UploadDoc"].ToString();
                        if (DOC != "")
                        {
                            int length = DOC.Length;
                            DOC = DOC.Substring(2, length - 2);
                        }
                        SeizedIteam6.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails()
                          {
                              SpeciesName = dr["SpeciesName"].ToString(),
                              ProduceType = dr["ProduceType"].ToString(),
                              Quantity = dr["Quantity"].ToString(),
                              UploadedDoc = DOC,
                          });

                    }

                    foreach (DataRow dr in ds.Tables[5].Rows)
                    {
                        string DOC = dr["UploadDoc"].ToString();
                        if (DOC != "")
                        {
                            int length = DOC.Length;
                            DOC = DOC.Substring(2, length - 2);

                        }
                        SeizedIteam5.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails()
                          {
                              EquipmentName = dr["EquipmentName"].ToString(),
                              Make = dr["Make"].ToString(),
                              Model = dr["Model"].ToString(),
                              Caliber = dr["Caliber"].ToString(),
                              IdentificationNo = dr["IdentificationNo"].ToString(),
                              size = dr["size"].ToString(),
                              UploadedDoc = DOC,
                          });

                    }
                    foreach (DataRow dr in ds.Tables[4].Rows)
                    {
                        string DOC = dr["UploadDoc"].ToString();
                        if (DOC != "")
                        {
                            int length = DOC.Length;
                            DOC = DOC.Substring(2, length - 2);

                        }
                        SeizedIteam1.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.SeizedDetails()
                          {
                              ScientificName = dr["ScientificName"].ToString(),
                              CommanName = dr["CommanName"].ToString(),
                              AnimalArticleName = dr["AnimalArticleName"].ToString(),
                              AnimalArticleDescription = dr["AnimalArticleDescription"].ToString(),
                              Quantity1 = dr["Quantity"].ToString(),
                              UploadedDoc = DOC,

                          });

                    }

                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        CompoundIteam1.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.CompoundingDetails()
                          {
                              IsCompoundable = dr["IsCompoundable"].ToString(),
                              SettlementAmount = dr["SettlementAmount"].ToString(),
                              AmountPaid = dr["AmountPaid"].ToString(),
                              CaseStatus = dr["CaseStatus"].ToString(),
                              FineAmount = dr["FineAmount"].ToString(),
                              DfoDecision = dr["DfoDecision"].ToString(),
                              StatusDesc = dr["StatusDesc"].ToString(),
                              Status = dr["Status"].ToString(),

                          });

                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        string str1 = dr["IDProofURL"].ToString();
                        if (str1 != "")
                        {
                            int length = str1.Length;
                            str1 = str1.Substring(2, length - 2);

                        }
                        string str2 = dr["SignedStatementURL"].ToString();
                        if (str2 != "")
                        {
                            int length = str2.Length;
                            str2 = str2.Substring(2, length - 2);

                        }
                        WitnessDetails.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.WitnessDetail()
                          {
                              WitnessName = dr["WitnessName"].ToString(),
                              FatherName = dr["FatherName"].ToString(),
                              Caste = dr["Caste"].ToString(),
                              Address1 = dr["Address1"].ToString(),
                              PhoneNo = dr["PhoneNo"].ToString(),
                              EmailID = dr["EmailID"].ToString(),
                              IDType = dr["IDType"].ToString(),
                              IDProofURL = str1,
                              StatementDate = dr["StatementDate"].ToString(),
                              SignedStatementURL = str2,
                              WitnessStatement = dr["WitnessStatement"].ToString(),


                          });

                    }
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        string str1 = dr["UEvidenceDocURL"].ToString();
                        if (str1 != "")
                        {
                            int length = str1.Length;
                            str1 = str1.Substring(2, length - 2);

                        }
                        string str2 = dr["EvidenceDocURL"].ToString();
                        if (str2 != "")
                        {
                            int length = str2.Length;
                            str2 = str2.Substring(2, length - 2);

                        }

                        string str3 = dr["FardGriftri"].ToString();
                        if (str3 != "")
                        {
                            int length = str3.Length;
                            str3 = str3.Substring(2, length - 2);

                        }
                        string str4 = dr["GriftariPunchnama"].ToString();
                        if (str4 != "")
                        {
                            int length = str4.Length;
                            str4 = str4.Substring(2, length - 2);

                        }
                        string str5 = dr["NagriNaka"].ToString();
                        if (str5 != "")
                        {
                            int length = str5.Length;
                            str5 = str5.Substring(2, length - 2);

                        }
                        string str6 = dr["JamaTalashi"].ToString();
                        if (str6 != "")
                        {
                            int length = str6.Length;
                            str5 = str6.Substring(2, length - 2);

                        }
                        string str7 = dr["JamaTalashi"].ToString();
                        if (str7 != "")
                        {
                            int length = str7.Length;
                            str7 = str7.Substring(2, length - 2);

                        }
                        OffenderDetails.Add(
                          new FMDSS.Models.ForestProtection.OffenseAssign.OffenderDetail()
                          {
                              OffenderType = dr["OffenderType"].ToString(),
                              OffenderName = dr["OffenderName"].ToString(),
                              FatherName = dr["FatherName"].ToString(),
                              Address1 = dr["Address1"].ToString(),

                              EvidenceDocURL = str2,
                              UEvidenceDocURL = str1,
                              FardGriftri = str3,
                              GriftariPunchnama = str4,
                              NagriNaka = str5,
                              JamaTalashi = str6,
                              MedicalReport = str7,
                              OffenderDescription = dr["OffenderDescription"].ToString(),
                              PoliceStation = dr["PoliceStation"].ToString(),
                              WarrentIssued = dr["WarrentIssued"].ToString(),
                              AppreanceDate = dr["AppreanceDate"].ToString(),



                          });

                    }
                    //return Json(new { list2 = SeizedIteam1 }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { list1 = offencedetail, list2 = OffenderDetails, list3 = WitnessDetails, list4 = CompoundIteam1, list5 = SeizedIteam1, list6 = SeizedIteam5, list7 = SeizedIteam6, list8 = SeizedIteam7, list9 = SeizedIteam8, list10 = SeizedIteam9 }, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
