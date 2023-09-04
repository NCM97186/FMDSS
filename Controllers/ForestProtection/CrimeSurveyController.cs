//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible cirme survey
//  Date Created : 02-06-2016
//  History      :error handle, bind plant, animal dropdown
//  Version      : 1.0
//  Author       : Ashok Yadav
//  Modified By  :   
//  Modified On  : 02-06-2016
//  Reviewed By  : 
//  Reviewed On  :  
//  Bug No-462,470


using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForestProtection;
using System.Data;
using System.Data.SqlTypes;
namespace FMDSS.Controllers.ForestProtection
{
    public class CrimeSurveyController : BaseController
    {
        List<SelectListItem> RangeName = new List<SelectListItem>();
        List<SelectListItem> OffenceList = new List<SelectListItem>();
        List<CrimeSurveyDetails> CrimSurveyList = new List<CrimeSurveyDetails>();
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /CrimeSurvey/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
        //
        // GET: /CrimeSurvey/Create
        [HttpPost]
        public JsonResult Bind_Village(string RangeCode1)
        {            
            string    RangeCode = RangeCode1;                        
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(RangeCode))
                {
                    CrimeSurveyDetails CSD = new CrimeSurveyDetails();
                    CSD.Rang_Code = RangeCode;
                    DataTable dt = CSD.Select_Villages();
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.VillageList = items;
                }
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

           // return Json(new SelectList(items, "Value", "Text"));
            return Json(new { list1 = items }, JsonRequestBehavior.AllowGet);     
        }

        public ActionResult Create(string OffenseCode)        
        {
            CrimeSurveyDetails CSD = new CrimeSurveyDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();           
            try
            {
               
                if (Encryption.decrypt(OffenseCode) != null) {
                    CSD.OffenseCode = Encryption.decrypt(OffenseCode);
                    Session["OffenseCode"] = CSD.OffenseCode;
                    }                    
                    ViewBag.UserName = Session["User"].ToString();
                    CSD.UserID = Convert.ToInt64(Session["UserID"].ToString());
                    CSD.EnteredBy = Convert.ToInt64(Session["UserID"].ToString());
                                        
                    DataTable dt = CSD.Select_Range();
                    RangeName.Add(new SelectListItem { Text = "--Select--", Value = "0" }); 
                    foreach (DataRow dr in dt.Rows)
                    {
                        RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                    }                  
                    ViewBag.Range = RangeName;
                    CSD.Action = 3;
                    DataTable dtf = new DataTable();
                    dtf = CSD.Select_Survey();
                   
                    foreach (DataRow dr in dtf.Rows)
                    {
                        CrimSurveyList.Add(new CrimeSurveyDetails()
                        {
                            ID = Convert.ToInt64(dr["ID"].ToString()),
                            OffenseCode = dr["OffenseCode"].ToString(),
                            sDate_Of_Visit = dr["Date_Of_Visit"].ToString(),
                            PlaceOfVisit = dr["PlaceOfVisit"].ToString(),
                            Description_of_Crime = dr["Description_of_Crime"].ToString(),
                            Pictures_of_Crime1 = dr["Pictures_of_Crime1"].ToString(),
                            Pictures_of_Crime2 = dr["Pictures_of_Crime2"].ToString(),
                            Pictures_of_Crime3 = dr["Pictures_of_Crime3"].ToString(),
                            Village_Name = dr["VILL_NAME"].ToString(),
                            Range_Name = dr["RANGE_NAME"].ToString(),
                            IsComplete = dr["IsComplete"].ToString(),                       
                        });
                    }                                       
                    ViewData["CrimeLst"] = CrimSurveyList;
                    //ViewBag.CrimeLst = CrimSurveyList;              
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        //
        // POST: /CrimeSurvey/Create

        [HttpPost]
        public ActionResult Create(CrimeSurveyDetails obj, FormCollection FM, string Command, List<HttpPostedFileBase> fileUpload)
        {
            try
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
                CrimeSurveyDetails sb = new CrimeSurveyDetails();

                if (Command == "Cancel")
                {
                    return RedirectToAction("OffenseRegistrationfinal", "OffenseRegistrationfinal");
                }
                if (FM["hdSurveyID"].ToString() == "")
                {
                    sb.ID = 0;
                }
                else
                {
                    sb.ID = Convert.ToInt64(FM["hdSurveyID"].ToString());
                }

                if (Session["UserId"] != null)
                {
                    sb.EnteredBy = Convert.ToInt64(Session["UserID"]);
                }

                if (FM["RangeCode"].ToString() == "")
                {
                    sb.Rang_Code = "0";
                }
                else
                {
                    sb.Rang_Code = FM["RangeCode"].ToString();
                }

                if (FM["VillageCode"].ToString() == "")
                {
                    sb.Village_Code = "0";
                }
                else
                {
                    sb.Village_Code = FM["VillageCode"].ToString();
                }               
                if (FM["txt_DOV"].ToString() == "")
                {
                    sb.Date_Of_Visit = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    sb.Date_Of_Visit = DateTime.ParseExact(FM["txt_DOV"].ToString(), "dd/MM/yyyy", null);

                }

                if (FM["txt_TimeVisit"].ToString() == "")
                {
                    sb.Time_Of_Visit = "";
                }
                else
                {
                    sb.Time_Of_Visit = FM["txt_TimeVisit"].ToString();
                }

                if (FM["txt_desc"].ToString() == "")
                {
                    sb.Description_of_Crime = "";
                }
                else
                {
                    sb.Description_of_Crime = FM["txt_desc"].ToString();
                }


                if (FM["txt_Place"].ToString() == "")
                {
                    sb.PlaceOfVisit = "";
                }
                else
                {
                    sb.PlaceOfVisit = FM["txt_Place"].ToString();
                }
               
                if (FM["txt_latitude"].ToString() == "")
                {
                    sb.Latitude = 0;
                }
                else
                {
                    sb.Latitude = Convert.ToDecimal(FM["txt_latitude"].ToString());
                }
                if (FM["txt_longitude"].ToString() == "")
                {
                    sb.Longitude = 0;
                }
                else
                {
                    sb.Longitude = Convert.ToDecimal(FM["txt_longitude"].ToString());
                }
                sb.OffenseCode = Session["OffenseCode"].ToString(); 
                var path = "";
                string FileName = string.Empty;
                string FileFullName = string.Empty;
                string FilePath =  "~/ForestProtectionDocument/CrimeSceneDetails/";
              
                System.Text.StringBuilder strbldr = new System.Text.StringBuilder();
                int count = 0;
                foreach (HttpPostedFileBase item in fileUpload)
                {

                    if (item != null)
                    {
                        if (Array.Exists(obj.FilesToBeUploaded.Split(','), s => s.Equals(item.FileName)))
                        {
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = item.FileName.Split(new char[] { '\\' });
                                FileName = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                FileName = item.FileName;
                            }
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = System.IO.Path.Combine(FilePath, FileFullName);
                           
                            strbldr.Append(path+",");
                            item.SaveAs(Server.MapPath(path));
                            count++;
                        }
                    }
                }
                if (Command == "Submit")
                {
                    DataSet ds = new DataSet();                    
                    sb.FilesToBeUploaded = strbldr.ToString().TrimEnd(',');
                    Int64 status = sb.Insert_Crime_Survey(sb);                               
                    if (status != 0)
                    {
                        TempData["BGS_Status"] = "Survey Created Successfully";
                        //return RedirectToAction("SurveyBudgetEstimation", "SurveyBudgetEstimation");
                    }
                    else
                    {
                        TempData["BGS_Status"] = "No inserted";
                    }
                }
                if (Command == "Update")
                {
                    DataSet ds = new DataSet();
                    Int64 status = 0;
                    if (sb.ID != 0)
                    {
                        status = sb.Update_Crime_Survey(sb);
                    }
                    if (status != 0)
                    {
                        TempData["BGS_Status"] = "Survey Updated Successfully";
                        //return RedirectToAction("SurveyBudgetEstimation", "SurveyBudgetEstimation");
                    }
                    else
                    {
                        TempData["BGS_Status"] = "Survey not Updated";
                    }
                }
              //return RedirectToAction("Create");
                DataTable dtf = new DataTable();
                dtf = sb.Select_Survey();

                foreach (DataRow dr in dtf.Rows)
                {
                    CrimSurveyList.Add(new CrimeSurveyDetails()
                    {
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        OffenseCode = dr["OffenseCode"].ToString(),
                        sDate_Of_Visit = dr["Date_Of_Visit"].ToString(),
                        PlaceOfVisit = dr["PlaceOfVisit"].ToString(),
                        Description_of_Crime = dr["Description_of_Crime"].ToString(),
                        Pictures_of_Crime1 = dr["Pictures_of_Crime1"].ToString(),
                        Pictures_of_Crime2 = dr["Pictures_of_Crime2"].ToString(),
                        Pictures_of_Crime3 = dr["Pictures_of_Crime3"].ToString(),
                        Village_Name = dr["VILL_NAME"].ToString(),
                        Range_Name = dr["RANGE_NAME"].ToString(),
                        IsComplete = dr["IsComplete"].ToString(),
                    });
                }
                ViewData["CrimeLst"] = CrimSurveyList;
                return RedirectToAction("OffenseRegistrationfinal", "OffenseRegistrationfinal");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /CrimeSurvey/Edit/5

        public JsonResult Edits(int id)
        { CrimeSurveyDetails CSD =null;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            CrimeSurveyDetails obj = new CrimeSurveyDetails();


            DataTable dt = obj.Select_Range();
            RangeName.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            foreach (DataRow dr in dt.Rows)
            {
                RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
            }

            ViewBag.Range = RangeName;
             
               
            obj.ID = id;
            DataTable DT = obj.GetDetailsforEdit_Crime_Survey(obj);
            if (DT.Rows.Count > 0)
            {
                OffenceList.Add(new SelectListItem { Text = DT.Rows[0]["OffenseCode"].ToString(), Value = DT.Rows[0]["OffenseCode"].ToString() });
                ViewBag.Offencelist = OffenceList;
                CSD = new CrimeSurveyDetails();
                 CSD.ID = Convert.ToInt64(DT.Rows[0]["ID"].ToString());
                 CSD.OffenseCode = DT.Rows[0]["OffenseCode"].ToString();
                 CSD.sDate_Of_Visit = DT.Rows[0]["Date_Of_Visits"].ToString();
                 CSD.Time_Of_Visit = DT.Rows[0]["Time_Of_Visit"].ToString();
                 CSD.PlaceOfVisit = DT.Rows[0]["PlaceOfVisit"].ToString();
                 CSD.Village_Name = DT.Rows[0]["VILL_NAME"].ToString();
                 CSD.Range_Name = DT.Rows[0]["RANGE_NAME"].ToString();  
                 CSD.Description_of_Crime = DT.Rows[0]["Description_of_Crime"].ToString();
                 CSD.Pictures_of_Crime1 = DT.Rows[0]["Pictures_of_Crime1"].ToString();
                 CSD.Pictures_of_Crime2 = DT.Rows[0]["Pictures_of_Crime2"].ToString();
                 CSD.Pictures_of_Crime3 = DT.Rows[0]["Pictures_of_Crime3"].ToString();
                 CSD.Rang_Code = DT.Rows[0]["Rang_Code"].ToString();
                 CSD.Village_Code = DT.Rows[0]["Village_Code"].ToString();
                 if (DT.Rows[0]["Latitude"].ToString()!="")
                 {
                     CSD.Latitude = Convert.ToDecimal(DT.Rows[0]["Latitude"].ToString());
                 }
                 if (DT.Rows[0]["Longitude"].ToString() != "")
                 {
                     CSD.Longitude = Convert.ToDecimal(DT.Rows[0]["Longitude"].ToString());
                 }


                 
            }
            
            return Json(CSD, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /CrimeSurvey/Edit/5

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
        // GET: /CrimeSurvey/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CrimeSurvey/Delete/5

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
