//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Microplan
//  Date Created : 12-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Durgesh N Sharma
//  Modified By  : Durgesh N Sharma  
//  Modified On  : 10-Mar-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@



using System;
using System.Collections.Generic;
using System.Linq;
using FMDSS.GenericClass;
using FMDSS.Models;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.ForesterDevelopment;
using FMDSS.Models.CitizenService.PermissionServices;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using FMDSS.Models.Admin;
using System.Data;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Configuration;
using FMDSS.Filters;
using Newtonsoft.Json;


namespace FMDSS.Controllers.ForesterDevelopment
{
     [MyAuthorization]
    public class MicroPlanController : BaseController
    {
//private RequestMethod requestMethod = new RequestMethod();
       // ISerialization _serializer;



        Int64 UserID;//= 0;
        Location _objLocation = new Location();
        MicroPlan _obj = new MicroPlan();
        List<MicroPlan> lstMP = new List<MicroPlan>();
        List<SelectListItem> cast = new List<SelectListItem>();
        List<SelectListItem> Education = new List<SelectListItem>();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> ForestOfficers = new List<SelectListItem>(); 
        List<SelectListItem> ForestDesignation = new List<SelectListItem>();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> Scheme = new List<SelectListItem>();
        List<SelectListItem> ddlJFMCorContractAgency = new List<SelectListItem>();
        List<SelectListItem> ddlNGOSHG = new List<SelectListItem>();
        List<SelectListItem> RangeName = new List<SelectListItem>();
        //
        // GET: /MicroPlan/

        public ActionResult Index()
        {
            try
            {
                _obj.UserID = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = _obj.Select_MicroPlan();

                foreach (DataRow dr in dtf.Rows)
                {
                    lstMP.Add(new MicroPlan()
                    {
                        RowID = dr["RowID"].ToString(),
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        DIST_NAME = dr["DIST_NAME"].ToString(),
                        BLK_NAME = dr["BLK_NAME"].ToString(),
                        GP_NAME = dr["GP_NAME"].ToString(),
                        VILL_NAME = dr["VILL_NAME"].ToString(),
                        MicroPlanName = dr["MicroPlanName"].ToString(),
                        StartDate = dr["StartDate"].ToString(),
                        EndDate = dr["EndDate"].ToString(),
                        EnteredOn = dr["EnteredOn"].ToString(),
                        StatusDesc = dr["StatusDesc"].ToString()
                    });
                }                
                //return dtf;
                return View("index", lstMP);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        /// <summary>
        /// This function is used to get social economic and literacy data.
        /// </summary>
        /// <param name="Dist_Code"></param>
        /// <param name="block_Code"></param>
        /// <param name="GP_Code"></param>
        /// <param name="ddlVillage"></param>
        /// <returns></returns>
        public JsonResult GetBhamashahData(string Dist_Code,string block_Code,string GP_Code,string ddlVillage)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                Stream resStream = null;
                string responseFromServer = "";
                // string json;
                //String json = serializeProduct(product);
                // requestMethod.getRequest("PUT", "application/json", "http://localhost:18950/api/products", json).GetResponse();
               // string ReturnUrl = ConfigurationManager.AppSettings["bhamashahserviceurl"].ToString();

                string url =  ConfigurationManager.AppSettings["bhamashahserviceurl"].ToString()+"getVillageData/" + Dist_Code + "/" + block_Code + "/" + GP_Code + "/" + ddlVillage;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                resStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(resStream);
                responseFromServer = reader.ReadToEnd();
                bhamashahdata bm = new JavaScriptSerializer().Deserialize<bhamashahdata>(responseFromServer);
                return Json(bm, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex){
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, UserID); 
                return null;
            }
           // return responseFromServer;
        }

        public ActionResult MicroPlan(string MPID, FormCollection form)//,string name)
        {
            ViewData["disablecontrols"] = false;
            string ID = Encryption.decrypt(MPID);
            try { 
            UserID = Convert.ToInt64(Session["UserID"]);
            List<SelectListItem> items = new List<SelectListItem>();

            DataTable dt = new FixedLandUsage().Division();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
            }

            //dt = _objLocation.District();
            //ViewBag.fname = dt;
            //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            //{
            //    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
            //}

            dt = _obj.GetFORESTDesignation();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                ForestDesignation.Add(new SelectListItem { Text = @dr["Desig_Name"].ToString(), Value = @dr["DesigId"].ToString() });
            }
            // Defect id 229 incorporated for ranger login only range and village will be shown automatically reported on 19 feb 2016.
            DataTable dtRange = new Common().Select_Range(_obj.UserID);
            foreach (DataRow dr in dtRange.Rows)
            {
                RangeName.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
            }
            ViewBag.ddlRange = RangeName;
            //dt = _obj.GetEducation();
            //ViewBag.fname = dt;
            //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            //{
            //    Education.Add(new SelectListItem { Text = @dr["Education"].ToString(), Value = @dr["ID"].ToString() });
            //}
            //dt = _obj.GetSocialCast();
            //ViewBag.fname = dt;
            //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            //{
            //    cast.Add(new SelectListItem { Text = @dr["Cast"].ToString(), Value = @dr["ID"].ToString() });
            //}        
            MicroPlan objMP = new MicroPlan();
            if (Convert.ToInt32(ID) > 0)
            {
                DataTable dtf = _obj.Select_MicroPlan(ID);

                GenericClasses<MicroPlan> genMP = new GenericClasses<MicroPlan>();
                FixedLandUsage ff = new FixedLandUsage();
              // dtf.Rows[0]["DIST_CODE"])); 
                dt =  ff.BindDistrict((Convert.ToString(dtf.Rows[0]["Div_Code"])));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                dt = _objLocation.BindBlockName(Convert.ToString(dtf.Rows[0]["DIST_CODE"]));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    BlockName.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                }
                dt = _objLocation.BindGramPanchayatName(Convert.ToString(dtf.Rows[0]["DIST_CODE"]), Convert.ToString(dtf.Rows[0]["BLK_CODE"]));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    GPName.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                }
                if (Session["DesignationId"].ToString() == "8")
                                        dt = new Common().Select_VillagesbyRange(Convert.ToString(dtf.Rows[0]["RANGE_CODE"]));
                else
                    dt = _objLocation.BindVillageName(Convert.ToString(dtf.Rows[0]["DIST_CODE"]), Convert.ToString(dtf.Rows[0]["BLK_CODE"]), Convert.ToString(dtf.Rows[0]["GP_CODE"]));                
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    VillageName.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                }
                //if ((Convert.ToString(dtf.Rows[0]["Div_Code"])).ToUpper() == "JFMC")
                //{ 
                //  dt= _obj.GetJFMCbyDivCode((Convert.ToString(dtf.Rows[0]["Div_Code"])));
                //    ViewBag.fname = dt;
                //    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //    {
                //        ddlJFMCorContractAgency.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                //    }
                //}
                //else
                //{
                //  dt= _obj.GetContratorbyDivCode((Convert.ToString(dtf.Rows[0]["Div_Code"])));
                //    ViewBag.fname = dt;
                //    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //    {
                //        ddlJFMCorContractAgency.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                //    }
                //}
                dt = _obj.GetJFMC(Convert.ToString(dtf.Rows[0]["VILL_CODE"]));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    ddlJFMCorContractAgency.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                }
               dt = _obj.GetNONGOVTOFFICERS((Convert.ToString(dtf.Rows[0]["NGOorSHO"])),Convert.ToString(dtf.Rows[0]["VILL_CODE"]));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    ddlNGOSHG.Add(new SelectListItem { Text = @dr["NGOSHGName"].ToString(), Value = @dr["ID"].ToString() });
                }
                dt = _obj.SelectSchemeByDist_Code(Convert.ToString(dtf.Rows[0]["Dist_Code"]));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Scheme.Add(new SelectListItem { Text = @dr["Scheme_Name"].ToString(), Value = @dr["ID"].ToString(), Selected = true });
                }
                if(!(dtf.Rows[0]["Status"].ToString()=="1" || dtf.Rows[0]["Status"].ToString()=="6") )
                    ViewData["disablecontrols"] = true;

               // objMP.get
                objMP = genMP.model(dtf);
                ViewBag.ProjectIDs = objMP.ProjectIDs;
                ViewBag.SchemeIDs = objMP.SchemeIDs;
                ViewBag.ForestEmployees = objMP.ForestOfficerEmpId;
            }
            else
            {
                objMP.DateofRequest = DateTime.Now.ToString("dd/MM/yyyy");
                objMP.StartDate = DateTime.Now.ToString("dd/MM/yyyy");
                objMP.EndDate = DateTime.Now.ToString("dd/MM/yyyy");
            }

            ViewBag.Scheme = Scheme;
            ViewBag.ddlDivision1 = Division;
            ViewBag.ddlDistrict1 = District;
            ViewBag.ddlBlockName1 = BlockName;
            ViewBag.ddlGPName1 = GPName;
            ViewBag.ddlVillName1 = VillageName;
            ViewBag.ddlEducation1 = Education;
            ViewBag.ddlcast1 = cast;
            ViewBag.ForestOfficers1 = ForestOfficers;
            ViewBag.ForestDesignation1 = ForestDesignation;
            ViewBag.ddlJFMCorContractAgency1 = ddlJFMCorContractAgency;
            ViewBag.ddlNGOSHG1 = ddlNGOSHG;


            // added by arvind kumar sharma [ for maintain state of dropdowns ]
            //START
            ViewBag.VillageIDGIS = "";
            ViewBag.GisRefID = "";
            string retData = Convert.ToString(form["activityData"]);
            if (retData != null)
            {
                //{"village_NM":"31 Ssw (Fatehgarh)","JFMCArea":0.478,"JFMCLength":2.95,"PlantationArea":0.169,"PlantationLength":1.68,"Cordinates":"8263526.6951298285,3440398.965724529","refGisId":52}
                GISactivityData Obj = (GISactivityData)Newtonsoft.Json.JsonConvert.DeserializeObject(form["activityData"], typeof(GISactivityData));


                var oObjpostbackData = JsonConvert.DeserializeObject<GISpostbackData>(form["postbackData"]);

                string[] LatLong = Obj.Cordinates.Split(',');

                // below return parameter are not available here
                //StkhldRet.Lattitude = Convert.ToDecimal(LatLong[0]);
                //StkhldRet.Longitude = Convert.ToDecimal(LatLong[1]);
                //StkhldRet.Plantation_Area = Convert.ToDecimal(Obj.PlantationArea);
                objMP.Totalarea = Convert.ToDecimal(Obj.DrawArea);
                objMP.RefGisID = Obj.refGisId;

                string DistrictGIS = string.Empty;
                string BlocknameGIS = string.Empty;
                string GPNameGIS = string.Empty;
                string RangeORForestDisvision = string.Empty;
                string VillageGIS = string.Empty;

                DistrictGIS = oObjpostbackData.DistrictID;
                BlocknameGIS = oObjpostbackData.BlocknameID;
                GPNameGIS = oObjpostbackData.GPNameID;
                VillageGIS = oObjpostbackData.VillageID;
                RangeORForestDisvision = oObjpostbackData.RangeID;

                if (Session["DesignationId"].ToString() == "8")
                {

                    ViewBag.ddlRange = new SelectList(RangeName, "Value", "Text", RangeORForestDisvision); //  Range

                    DataTable dtRangeORForestDisvision = new Common().Select_VillagesbyRange(RangeORForestDisvision);
                    foreach (System.Data.DataRow dr in dtRangeORForestDisvision.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }

                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text", VillageGIS);
                    ViewBag.VillageIDGIS = VillageGIS;
                }
                else if (Session["DesignationId"].ToString() == "6")
                {

                    ViewBag.ddlDivision1 = new SelectList(Division, "Value", "Text", RangeORForestDisvision);//  ForestDisvision


                    FixedLandUsage ffixedand = new FixedLandUsage();
                    dt = ffixedand.BindDistrict((Convert.ToString(RangeORForestDisvision)));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }
                    ViewBag.ddlDistrict1 = new SelectList(District, "Value", "Text", DistrictGIS);


                    dt = _objLocation.BindBlockName(Convert.ToString(DistrictGIS));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        BlockName.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }
                    ViewBag.ddlBlockName1 = new SelectList(BlockName, "Value", "Text", BlocknameGIS);




                    dt = _objLocation.BindGramPanchayatName(Convert.ToString(DistrictGIS), Convert.ToString(BlocknameGIS));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        GPName.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    ViewBag.ddlGPName1 = new SelectList(GPName, "Value", "Text", GPNameGIS);



                    dt = _objLocation.BindGramPanchayatName(Convert.ToString(DistrictGIS), Convert.ToString(BlocknameGIS));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        GPName.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    ViewBag.ddlGPName1 = new SelectList(GPName, "Value", "Text", GPNameGIS);


                    dt = _objLocation.BindVillageName(Convert.ToString(DistrictGIS), Convert.ToString(BlocknameGIS), Convert.ToString(GPNameGIS));
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        VillageName.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(VillageName, "Value", "Text", VillageGIS);
                    ViewBag.VillageIDGIS = VillageGIS;


                }

            }

            //END
            // added by arvind kumar sharma [ for maintain state of dropdowns ]
               






            return View("Microplan",objMP);
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /MicroPlan/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MicroPlan/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MicroPlan/Create

        [HttpPost]
        public ActionResult Create(MicroPlan objMP,FormCollection fc)
        {
            try
            {
              /*  objMP.Village_Code = Convert.ToString(fc["ddlVillName"]);
                objMP.AgricultureLand = "";
                objMP.AllocateforPlantSQKM = "";
                objMP.ClassifiedForestSQKM = "";
                objMP.DateofRequest = "";
                objMP.EndDate = "";
                objMP.EnteredBy = "";
                objMP.ForestAdminUnit = "";
                objMP.ForestOfficerEmpId = "";
                objMP.FullyCoveredSQKM = "";
                objMP.GramPanchayat = "";
                objMP.ID = "";
                objMP.IrregatedLandSQKM = "";
                objMP.NGOOfficerEmpId = "";
                objMP.IsActive = "";
                objMP.JFMCorContractAgency = "";
                objMP.MicroPlanName = "";
                objMP.NonIrregatedLandSQKM = "";
                objMP.Other = "";
                objMP.PanchayatComittee = "";
                objMP.PanchayatLandSQKM = "";
                objMP.ProtectedForestSQKM = "";
                objMP.RangeOfficeUnit = "";
                objMP.*/
                
              //  objMP.Village_Code = Convert.ToString(fc["ddlVillName"]);

                objMP.hdnVillageCode =  objMP.VILL_CODE;

                if (Convert.ToString(fc["ddlForestOfficer"]) != null)
                    objMP.ForestOfficerEmpId = Convert.ToString(fc["ddlForestOfficer"]).Split('#')[0];
                else
                    objMP.ForestOfficerEmpId = "0";
              //  objMP.NGOorSHO = Convert.ToString(fc[""]);
               // objMP.ForestOfficerEmpId = Convert.ToString(fc["ddlForestOfficer"]);
                objMP.StartDate = Convert.ToString(fc["StartDate"]);
                objMP.EndDate = Convert.ToString(fc["EndDate"]);
                objMP.DateofRequest = Convert.ToString(fc["DateofRequest"]);
                int totalEduCount = Convert.ToInt32(fc["totalEducation"]);
                int totalCattleCount = Convert.ToInt32(fc["totalCattle"]);
                int totalCastCount = Convert.ToInt32(fc["totalCast"]);
                try
                {
                  //  objMP.ProjectIDs = Convert.ToString(fc.GetValue("ProjectIDs").AttemptedValue);//	"17,18"	string
                    objMP.SchemeIDs = Convert.ToString(fc.GetValue("SchemeIDs").AttemptedValue);//	"17,18"	string
                }
                catch
                {
                    //objMP.ProjectIDs = "";
                    objMP.SchemeIDs = "";
                }
               //objMP.NGOorSHOOfficerEmpId = Convert.ToString(fc["ddlNgoShg"]);
                DataTable dtPopulation = new DataTable("Table");
                dtPopulation.Columns.Add("SocialClassCategory", typeof(String));
                dtPopulation.Columns.Add("NoofFamily", typeof(String));
                dtPopulation.Columns.Add("AdultMale", typeof(String));
                dtPopulation.Columns.Add("AdultFemale", typeof(String));
                dtPopulation.Columns.Add("ChildMale", typeof(String));
                dtPopulation.Columns.Add("ChildFemale", typeof(String));
                dtPopulation.AcceptChanges();
              
                                
                DataTable dtEducation = new DataTable("Table");
                dtEducation.Columns.Add("EducationLevel", typeof(String));                
                dtEducation.Columns.Add("Male", typeof(String));
                dtEducation.Columns.Add("Female", typeof(String));                
                dtEducation.AcceptChanges();

                DataTable dtCattle = new DataTable("Table");
                dtCattle.Columns.Add("CattleType", typeof(String));
                dtCattle.Columns.Add("CattleName", typeof(String));
                dtCattle.Columns.Add("CattleCount", typeof(String));
                dtCattle.AcceptChanges();

                for (int i = 1; i <= totalCastCount; i++)
                {
                    DataRow dr = dtPopulation.NewRow();
                   dr["SocialClassCategory"] = fc["castID_"+i.ToString()];
                   dr["NoofFamily"] = fc["familycount_" + i.ToString()];
                   dr["AdultMale"] = fc["adultM_" + i.ToString()];
                   dr["AdultFemale"] = fc["AdultF_" + i.ToString()];
                   dr["ChildMale"] = fc["ChildM_" + i.ToString()];
                   dr["ChildFemale"] = fc["ChildF_" + i.ToString()];
                   dtPopulation.Rows.Add(dr);
                   dtPopulation.AcceptChanges();
                }
                for (int i = 0; i < totalCattleCount; i++)
                {
                    DataRow dr = dtCattle.NewRow();
              
                        dr["CattleType"] = fc["CattleType_" + i.ToString()];
                        dr["CattleName"] = fc["CattleName_" + i.ToString()];
                        string abc = "";
                        abc = fc["CattleCount_" + i.ToString()] ;
                        if (abc != null)
                        {
                            if (abc.Contains(',') == true)
                            {
                                abc = abc.TrimEnd(',');
                            }
                        }
                        dr["CattleCount"] = abc;
                             
                    if (!string.IsNullOrEmpty(Convert.ToString(dr["CattleName"])))
                    {
                        dtCattle.Rows.Add(dr);
                        dtCattle.AcceptChanges();
                    }
                }
                    for (int i = 1; i <= totalEduCount; i++)
                    {
                        DataRow dr = dtEducation.NewRow();
                        dr["EducationLevel"] = fc["eduID_" + i.ToString()];
                        dr["Male"] = fc["femaleEdu_" + i.ToString()];
                        dr["Female"] = fc["maleEdu_" + i.ToString()];
                        dtEducation.Rows.Add(dr);
                        dtEducation.AcceptChanges();
                    }
                    objMP.ForestOfficerEmpId = "";              
                   int totalemp = Convert.ToInt32(fc["totalemp"]);
                   for (int i = 1; i <= totalemp; i++)
                   {
                       if (!string.IsNullOrEmpty(fc["Addthis_" + i.ToString()]))
                           objMP.ForestOfficerEmpId = objMP.ForestOfficerEmpId+ Convert.ToString(fc["Addthis_" + i.ToString()])+",";
                   }
                 objMP.ForestOfficerEmpId.TrimEnd(',');
                 objMP.IsActive = true;
                 objMP.Status = 1;
                    // TODO: Add insert logic here
                 new MicroPlan().SubmitMicroPlan(objMP, dtEducation, dtPopulation, dtCattle);

                 return RedirectToAction("Index");
            
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /MicroPlan/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MicroPlan/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("MicroPlan");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // GET: /MicroPlan/Delete/5

        public ActionResult Delete(int id)
        {
            try { 
            if (Session["UserId"] != null)
            {
                _obj.ID = Convert.ToInt64(id);
                _obj.UpdatedBy = Convert.ToInt64(Session["UserID"]);
                Int64 i = _obj.DeleteMicroPlan();
            }
            //Int64 i = cst.DeleteMasterFeesEntry();
            return RedirectToAction("index", "MicroPlan");
          //  return View();
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }

        //
        // POST: /MicroPlan/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("MicroPlan");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetBlockName(string District)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _objLocation.BindBlockName(District);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }

                    ViewBag.ddlBlockName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }

         [HttpPost]
        public JsonResult GetFORESTOFFICERS(string Village_Code,string FORESTDesignationID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.GetFORESTOFFICERS(Village_Code,FORESTDesignationID);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["EmpName"].ToString(), Value = @dr["EmpId"].ToString() + "#" + @dr["EmpDesignation"].ToString() });
                    }

                    ViewBag.ForestOfficers1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }
        
                 [HttpPost]
        public JsonResult GetFORESTOFFICERSbyDivCode(string div_code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();
                    GenericClasses<ForestEmployees> ff = new GenericClasses<ForestEmployees>();
                    DataTable dt = _obj.GetFORESTOFFICERS(div_code);
                    ViewBag.fname = dt;
                    
                    return Json(ff.lstmodel(dt), JsonRequestBehavior.AllowGet); 
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }   
        [HttpPost]
        public JsonResult GetGetHierarchybyVillageCode(string Vill_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    GenericClasses<MicroPlan> ff = new GenericClasses<MicroPlan>();
                    DataTable dt = _obj.GetGetHierarchybyVillageCode(Vill_Code);
                    ViewBag.fname = dt;
                    
                    return Json(ff.model(dt), JsonRequestBehavior.AllowGet); 
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }  
         [HttpPost]
         public JsonResult GetCattleDetailbyMicroplan(string MicroplanID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    GenericClasses<MicroplanCattleDetail> ff = new GenericClasses<MicroplanCattleDetail>();
                    DataTable dt = _obj.SelectCattleByMicroplan(MicroplanID);
                    ViewBag.fname = dt;
                    
                    return Json(ff.lstmodel(dt), JsonRequestBehavior.AllowGet); 
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }
        
[HttpPost]
         public JsonResult GetNONGOVTOFFICERS(string Type,string Village_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.GetNONGOVTOFFICERS(Type,Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["NGOSHGName"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ForestOfficers1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        } 
        [HttpPost]
        public JsonResult GetJFMC(string Village_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            ViewData["ee"]="Hello";
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.GetJFMC( Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ForestOfficers1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }
        public JsonResult GetJFMCbyDivCode(string div_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.GetJFMCbyDivCode(div_Code );
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ForestOfficers1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }
        public JsonResult GetContrator(string Village_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.GetContrator(Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ForestOfficers1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        } 
        public JsonResult GetContratorbyDivCode(string div_Code)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _obj.GetContratorbyDivCode(div_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Committee_Name"].ToString(), Value = @dr["ID"].ToString() });
                    }

                    ViewBag.ForestOfficers1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        } [HttpPost]
        public JsonResult GetGramPName(string District, string BlockName)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _objLocation.BindGramPanchayatName(District, BlockName);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    ViewBag.ddlGPName1 = new SelectList(items, "Value", "Text");

                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }
        [HttpPost]
        public JsonResult GetVillageName(string District, string BlockName, string GPName)
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _objLocation.BindVillageName(District, BlockName, GPName);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        } 
        
        [HttpPost]
        public JsonResult GetEducation()
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _obj.GetEducation();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Education"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }
        public JsonResult SelectSchemeByDist_Code(string Dist_Code)
        {
            try {
                DataTable dt = _obj.SelectSchemeByDist_Code(Dist_Code);
                return dtToViewBagJSON(dt, "Scheme_Name", "ID");
            }
            catch (Exception ex) { Console.Write("Error " + ex); return null; }
        }
        
        [HttpPost]
        public JsonResult GetSocialCast()
        {

            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    DataTable dt = _obj.GetSocialCast();
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Cast"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;

        }

        public JsonResult dtToViewBagJSON(DataTable dt, string TextField, string ValueField)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    //DataTable dt = _obj.SelectMicroPlanByVilageCode(Village_Code);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr[TextField].ToString(), Value = @dr[ValueField].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }

        public JsonResult GetTotalLandUseAndLandCoverOfVillage(string Village)
        {
            
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 2, DateTime.Now, UserID);
            }
            return null;
        }

    }
}
