//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : CitizenRegistration
//  Description  : File contains registration for Amrita Devi Module
//  Date Created : 25-05-2017
//  History      :
//  Version      : 1.0
//  Author       : Rajveer Sharma
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using FMDSS.E_SignIntegration;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS.Controllers
{
    public class AmritaDeviAwardController : BaseController
    {
        AmritaDeviAwardModel objmodel = new AmritaDeviAwardModel();
        //
        // GET: /AmritaDeviAward/
        public ActionResult AmritaDeviAwardReferedByDept()
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {


                ViewData["fixedlandlist"] = new List<clsPermission>();

                AmritaDeviInfo model = new AmritaDeviInfo();
                BindMasterData _ObjMaster = new BindMasterData();
                DataTable dtAward = new DataTable();
                dtAward = model.Select_AmritaDeviAward();
                //DataTable dtNOCPurpose = new DataTable();
                //dtNOCPurpose = _ObjMaster.GetFixedLandNOCPurpose();
                //ViewBag.NOCPurpose = new SelectList(dtNOCPurpose.AsDataView(), "NOCTypeId", "NOCTypeName");
                objmodel.DetailList = model.Select_AmritaDeviDetailList();
                ViewBag.AwardCategory = new SelectList(dtAward.AsDataView(), "AwardCategoryId", "CategoryName");
                ViewBag.ApplicantYear = new SelectList(objmodel.DetailList.Tables[2].AsDataView(), "ApplicantYears", "ApplicantYears");
                TempData["AwardCategory"] = dtAward;
                objmodel.CurrentYear = DateTime.Now.Year;

                objmodel.ReferedBy = Convert.ToString(Session["SSOID"]);
                TempData["ReferedBy"] = Convert.ToString(Session["SSOID"]);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(objmodel);
        }

        public ActionResult AmritaDeviAwardIndex()
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {


                ViewData["fixedlandlist"] = new List<clsPermission>();

                AmritaDeviInfo model = new AmritaDeviInfo();
                BindMasterData _ObjMaster = new BindMasterData();
                DataTable dtAward = new DataTable();
                dtAward = model.Select_AmritaDeviAward();
                //DataTable dtNOCPurpose = new DataTable();
                //dtNOCPurpose = _ObjMaster.GetFixedLandNOCPurpose();
                //ViewBag.NOCPurpose = new SelectList(dtNOCPurpose.AsDataView(), "NOCTypeId", "NOCTypeName");
                objmodel.DetailList = model.Select_AmritaDeviDetailList();
                ViewBag.AwardCategory = new SelectList(dtAward.AsDataView(), "AwardCategoryId", "CategoryName");
                ViewBag.ApplicantYear = new SelectList(objmodel.DetailList.Tables[2].AsDataView(), "ApplicantYears", "ApplicantYears");
                TempData["AwardCategory"] = dtAward;
                objmodel.CurrentYear = DateTime.Now.Year;

                objmodel.ReferedBy = "Self";
                TempData["ReferedBy"] = "Self";
				


			}
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return View(objmodel);
        }

        [HttpPost]
        public ActionResult AmritaDeviAwardIndex(AmritaDeviAwardModel model, List<HttpPostedFileBase> fileUpload, List<HttpPostedFileBase> fileUpload1)
        {
            if (ModelState.IsValid && Session["UserID"] != null)
            {
                model.RequestID = Convert.ToString(DateTime.Now.Ticks);
                bool IsPDFCreated = false;
                if (TempData["GISModelLIst"] != null)
                {
                    model.GISInformationList = TempData["GISModelLIst"] as List<GISDataBaseModel>;
                    YearsDetails[] WorkDetails_PDF = model.WorkDetail;
                    List<clsPermission> GISLIST_PDF = TempData["GISlistShowPDF"] as List<clsPermission>;
                    string CategoryName = Convert.ToString((TempData["AwardCategory"] as DataTable).AsEnumerable().Where(s => s.Field<int>("AwardCategoryId") == 1).Select(d => d.Field<string>("CategoryName")).FirstOrDefault());
                    model.ApplicationPDFName = Print(model, WorkDetails_PDF, GISLIST_PDF, CategoryName);
                }


                if (!string.IsNullOrEmpty(model.ApplicationPDFName))
                {
                    string FileFullName = string.Empty;
					string FilePath = string.Empty;
					string host = HttpContext.Request.Url.Host.ToLower();
					if (host == "localhost")
					{
						FilePath = "~/AmritaDeviDocument/";
					}
					else
					{
						FilePath = "/AmritaDeviDocument/";
					}
					string path;
                    model.DocumentList = new List<DocumentList>();
					int i = 0;
					if (fileUpload != null)
                    {
						foreach(var itm in fileUpload)
						{
							DocumentList view = new DocumentList();
							FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
							path = Path.Combine(FilePath, FileFullName);
							view.FileName = itm.FileName;
							view.FilePath = path;
							view.FileType = 1;//Means DSR1 
							path = Server.MapPath(FilePath + FileFullName);
							itm.SaveAs(path);
							model.DocumentList.Add(view);

						}
						

						//foreach (var itm in fileUpload)
						//{
						//    if (itm != null)
						//    {

						//        DocumentList view = new DocumentList();
						//        FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
						//        path = Path.Combine(FilePath, FileFullName);
						//        view.FileName = itm.FileName;
						//        view.FilePath = Path.Combine(FilePath, FileFullName);
						//        view.FileType = 1;//Means DSR1 
						//        Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
						//        model.DocumentList.Add(view);
						//        i++;
						//    }
						//}
						
					}

					if (fileUpload1 != null)
					{
						foreach (var itm in fileUpload1)
						{
							DocumentList view = new DocumentList();
							FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
							path = Path.Combine(FilePath, FileFullName);
							view.FileName = itm.FileName;
							view.FilePath = path;
							view.FileType = 2;//Means DSR2 
							path = Server.MapPath(FilePath + FileFullName);
							itm.SaveAs(path);
							model.DocumentList.Add(view);

						}

						//    foreach (var itm in fileUpload1)
						//    {
						//        if (itm != null)
						//        {
						//            DocumentList view = new DocumentList();
						//            FileFullName = DateTime.Now.Ticks + "_" + itm.FileName;
						//            path = Path.Combine(FilePath, FileFullName);
						//            view.FileName = itm.FileName;
						//            view.FilePath = Path.Combine(FilePath, FileFullName);
						//            view.FileType = 2;//Means DSR2 
						//            Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
						//            model.DocumentList.Add(view);

						//        }
						//    }
					}

                    model.UserId = Convert.ToInt32(Session["UserID"]);
                    AmritaDeviInfo Repository = new AmritaDeviInfo();
                    model.RequestID = Convert.ToString(Repository.SubmitApplication(model));
                    if (string.IsNullOrEmpty(model.RequestID) || model.RequestID == "0")
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                    }
                    else
                    {
                        TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> आवेदक का फॉर्म सफलतापूर्वक सबमिट हो गया है आपका आवेदन क्रमांक " + model.RequestID + " है! <br/>(Applicant form submited successfully Your Request ID is :- " + model.RequestID + ") </div>";
                    }
                }
            }
            else
            {
                TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
            }
            if (model.ReferedBy == "Self")
            {
                return RedirectToAction("AmritaDeviAwardIndex");
            }
            else
            {
                return RedirectToAction("AmritaDeviAwardReferedByDept");
            }
        }

        /// <summary>
        /// Use to bind the data to page controls which come from GIS Service
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult getGISData(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string aid = string.Empty;
            try
            {
                AmritaDeviInfo model = new AmritaDeviInfo();
                BindMasterData _ObjMaster = new BindMasterData();
                DataTable dtAward = new DataTable();
                dtAward = model.Select_AmritaDeviAward();
                DataTable dtNOCPurpose = new DataTable();
                dtNOCPurpose = _ObjMaster.GetFixedLandNOCPurpose();
                ViewBag.NOCPurpose = new SelectList(dtNOCPurpose.AsDataView(), "NOCTypeId", "NOCTypeName");
                objmodel.DetailList = model.Select_AmritaDeviDetailList();
                ViewBag.AwardCategory = new SelectList(dtAward.AsDataView(), "AwardCategoryId", "CategoryName");
                ViewBag.ApplicantYear = new SelectList(objmodel.DetailList.Tables[2].AsDataView(), "ApplicantYears", "ApplicantYears");
                if (Convert.ToString(Session["PermissionTypeID"]) != "") { aid = Convert.ToString(Session["PermissionTypeID"]); }
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {


                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));
                    objmodel.GISID = form["gisid"].ToString();
                    objmodel.AreaShape = Convert.ToDecimal(form["shapeArea"].ToString());
                    #region "Muliple List"
                    objmodel.GISInformationList = new List<GISDataBaseModel>();
                    List<clsPermission> fixedlandlist = new List<clsPermission>();
                    string LAT = string.Empty, Long = string.Empty;
                    foreach (var dr in myDeserializedObj)
                    {
                        #region "KML and Lat-Long"
                        if (form["locCentroid"].ToString() != "")
                        {
                            if (form["locCentroid"].ToString().Contains(","))
                            {
                                string[] locCentroid = form["locCentroid"].ToString().Split(',');
                                LAT = locCentroid[1] == "NA" ? "" : locCentroid[1];
                                Long = locCentroid[0] == "NA" ? "" : locCentroid[0];
                            }
                        }

                        #endregion

                        clsPermission cls = new clsPermission();
                        cls.Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        cls.Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        cls.Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        cls.Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        cls.Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        cls.Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM;
                        cls.Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM;
                        cls.Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM;
                        cls.Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM;
                        cls.Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM;
                        cls.areaName = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        cls.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE;
                        cls.Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd;
                        cls.Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM;
                        fixedlandlist.Add(cls);

                        GISDataBaseModel gisModel = new GISDataBaseModel();
                        gisModel.DIV_CODE = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        gisModel.DIST_CODE = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        gisModel.BLK_CODE = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        gisModel.GP_CODE = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        gisModel.VILL_CODE = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        gisModel.Area = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        gisModel.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : string.IsNullOrEmpty(dr.FOREST_DIVCODE) ? "N/A" : dr.FOREST_DIVCODE;
                        gisModel.GPSLat = LAT;
                        gisModel.GPSLong = Long;
                        gisModel.GISFilePath = Convert.ToString(form["filePath"]);
                        gisModel.GISOrignalFilePath = Convert.ToString(form["originalFileName"]);
                        gisModel.GISID = objmodel.GISID;
                        gisModel.AreaShapeInHactor = objmodel.AreaShape;

                        objmodel.GPSLat = LAT;
                        objmodel.GPSLong = Long;
                        objmodel.GISInformationList.Add(gisModel);

                    }

                    string ster = Newtonsoft.Json.JsonConvert.SerializeObject(fixedlandlist);
                    #endregion
                    TempData["GISModelLIst"] = objmodel.GISInformationList;
                    //objmodel.ConditionGISMode = true;
                    ViewData["fixedlandlist"] = fixedlandlist;
                    TempData["GISlistShowPDF"] = fixedlandlist;
                    objmodel.ReferedBy = Convert.ToString(TempData["ReferedBy"]);
                }
                else
                {
                    List<clsPermission> fixedlandlist = new List<clsPermission>();
                    ViewData["fixedlandlist"] = fixedlandlist;

                    objmodel.GISInformationList = new List<GISDataBaseModel>();
                    ViewData["GISModelLIst"] = objmodel.GISInformationList;
                    // objmodel = BindPageData(aid);
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.InnerException + "_" + ex.StackTrace);
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);
            }

			new Common().ErrorLog("test","test",2,DateTime.Now,1);
			return View("AmritaDeviAwardIndex", objmodel);
        }

	

        public ActionResult AmritaDeviAwardReviewApprover()
        {
            List<AmritaDeviAwardModel> obj = new List<AmritaDeviAwardModel>();
            List<AmritaDeviAwardModel> obj1 = new List<AmritaDeviAwardModel>();
            List<AmritaDeviAwardModel> obj2 = new List<AmritaDeviAwardModel>();
			//Export();
            AmritaDeviInfo repo = new AmritaDeviInfo();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string IDs = string.Empty;

            try
            {


                DataSet DS = new DataSet();
                DS = repo.ADReviewApprover(UserID);

                int count = 1;
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    obj.Add(
                        new AmritaDeviAwardModel()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FirstName1 = Convert.ToString(dr["Name"].ToString()),
                            AwardCategory = Convert.ToString(dr["CategoryName"].ToString()),
                            LandPlace = Convert.ToString(dr["LandPlace"].ToString()),
                            StatusName = Convert.ToString(dr["Statusdesc"].ToString()),
                        });
                    count += 1;
                }

                ViewData["ToBeAssigned"] = obj;

                count = 1;

                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    obj1.Add(
                        new AmritaDeviAwardModel()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FirstName1 = Convert.ToString(dr["Name"].ToString()),
                            AwardCategory = Convert.ToString(dr["CategoryName"].ToString()),
                            LandPlace = Convert.ToString(dr["LandPlace"].ToString()),
                            StatusName = Convert.ToString(dr["Statusdesc"].ToString()),
                        });
                    count += 1;
                }


                ViewData["PendingRequests"] = obj1;

                count = 1;

                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    obj2.Add(
                        new AmritaDeviAwardModel()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FirstName1 = Convert.ToString(dr["Name"].ToString()),
                            AwardCategory = Convert.ToString(dr["CategoryName"].ToString()),
                            LandPlace = Convert.ToString(dr["LandPlace"].ToString()),
                            StatusName = Convert.ToString(dr["Statusdesc"].ToString()),

                        });
                    count += 1;
                }

                ViewData["MyActions"] = obj2;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View();

        }

        public ActionResult GetADAssignerDetails(string id)
        {
            AmritaDeviInfo repo = new AmritaDeviInfo();
            AmritaDeviAwardModel model = new AmritaDeviAwardModel();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                DataSet DS = new DataSet();
                DS = repo.GetADApprvDetails(UserID.ToString(), id);
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    model.RequestID = Convert.ToString(dr["RequestID"].ToString());
                    model.FirstName1 = Convert.ToString(dr["Name"].ToString());
                    model.ReferedBy = Convert.ToString(dr["ReferedBYSSOID"].ToString());
                    model.StatusName = Convert.ToString(dr["StatusDesc"].ToString());
                    model.AwardCategory = Convert.ToString(dr["CategoryName"].ToString());
                    model.AwardAmount = Convert.ToDecimal(dr["AwardAmount"].ToString());
                    model.PersonalLandHactorDesc = Convert.ToString(dr["PersonalLandDetail"].ToString());
                    model.PersonalLandHactor = Convert.ToString(dr["PersonalLandHectare"].ToString());
                    model.CollectiveLandDesc = Convert.ToString(dr["CommunityLandDETAIL"]);
                    model.CollectiveLandHactor = Convert.ToString(dr["CommunityLandHectare"]);
                    model.RevenueLandDesc = Convert.ToString(dr["RevenueLandDETAIL"]);
                    model.RevenueLandHactor = Convert.ToString(dr["RevenueLandHectare"]);
                    model.Landhacktor = Convert.ToString(dr["LandRealArea"]);
                    model.LandPlace = Convert.ToString(dr["LandPlace"]);
                    model.ForestLandDesc = Convert.ToString(dr["ForestLandDetail"]);
                    model.ForestLandHactor = Convert.ToString(dr["ForestLandHectare"]);
                    model.GISID = Convert.ToString(dr["GISID"]);
                    model.Village = Convert.ToString(dr["VILL_Name"]);
                    model.GramPanchayat = Convert.ToString(dr["GP_Name"]);
                    model.District = Convert.ToString(dr["Dist"]);
                    model.NameofArea = Convert.ToString(dr["Area"]);
                    string filePdfFileName = DS.Tables[0].Columns.Contains("ApplicationPDF") ? Convert.ToString(dr["ApplicationPDF"]) : "";
                    if (!string.IsNullOrEmpty(filePdfFileName))
                        model.ApplicationPDFName = filePdfFileName.Substring(1, filePdfFileName.Length - 1);
                }


                ViewBag.ListAssignTo = new SelectList(DS.Tables[2].AsDataView(), "UserID", "Ssoid");
                if (DS.Tables[1] != null && DS.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[1].Rows)
                    {
                        YearsDetailsList view = new YearsDetailsList();
                        view.Current = Convert.ToString(dr["CURRENT"]);
                        view.Prev = Convert.ToString(dr["PREV"]);
                        view.End = Convert.ToString(dr["END"]);
                        view.WorkDesc = Convert.ToString(dr["Workdescription"]);
                        model.GetWorkList.Add(view);
                    }
                }
                if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                {
                    ViewBag.CurrentYears = Convert.ToInt32(DS.Tables[3].Rows[0][0]);
                    ViewBag.PrevYears = Convert.ToInt32(DS.Tables[3].Rows[1][0]);
                    ViewBag.EndYears = Convert.ToInt32(DS.Tables[3].Rows[2][0]);
                }
                model.DocumentList = new List<DocumentList>();
                if (DS.Tables[4] != null && DS.Tables.Contains("Table4") && DS.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[4].Rows)
                    {
                        DocumentList list = new DocumentList();
                        string filepath = string.Empty;
                        list.FileName = Convert.ToString(dr["FileName"]);
                        filepath = Convert.ToString(dr["FilePath"]);
                        list.FilePath = filepath.Substring(1, filepath.Length - 1);
                        list.FileType = Convert.ToInt32(dr["FileType"]);
                        model.DocumentList.Add(list);
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("ADAppAssigner", model);

        }


        public ActionResult ADAppAssigner(AmritaDeviAwardModel model)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            int UserID = Convert.ToInt32(Session["UserID"]);
            AmritaDeviInfo repo = new AmritaDeviInfo();

            DataTable dt;

            try
            {
                model.UserId = UserID;
                dt = repo.SubmitADAssign(model);

                return RedirectToAction("AmritaDeviAwardReviewApprover");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }

        public ActionResult GetADRevApprvDetails(string id)
        {

            DataSet DS = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            AmritaDeviInfo repo = new AmritaDeviInfo();
            AmritaDeviAwardModel model = new AmritaDeviAwardModel();

            try
            {

                DS = repo.GetADRevApprvDetails(UserID.ToString(), id);

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    model.RequestID = Convert.ToString(dr["RequestID"].ToString());
                    model.FirstName1 = Convert.ToString(dr["Name"].ToString());
                    model.ReferedBy = Convert.ToString(dr["ReferedBYSSOID"].ToString());
                    model.StatusName = Convert.ToString(dr["StatusDesc"].ToString());
                    model.AwardCategory = Convert.ToString(dr["CategoryName"].ToString());
                    model.AwardAmount = Convert.ToDecimal(dr["AwardAmount"].ToString());
                    model.PersonalLandHactorDesc = Convert.ToString(dr["PersonalLandDetail"].ToString());
                    model.PersonalLandHactor = Convert.ToString(dr["PersonalLandHectare"].ToString());
                    model.CollectiveLandDesc = Convert.ToString(dr["CommunityLandDETAIL"]);
                    model.CollectiveLandHactor = Convert.ToString(dr["CommunityLandHectare"]);
                    model.RevenueLandDesc = Convert.ToString(dr["RevenueLandDETAIL"]);
                    model.RevenueLandHactor = Convert.ToString(dr["RevenueLandHectare"]);
                    model.Landhacktor = Convert.ToString(dr["LandRealArea"]);
                    model.LandPlace = Convert.ToString(dr["LandPlace"]);
                    model.ForestLandDesc = Convert.ToString(dr["ForestLandDetail"]);
                    model.ForestLandHactor = Convert.ToString(dr["ForestLandHectare"]);
                    model.Document1 = Convert.ToString(dr["DPRDocument1URL"]);
                    model.Document2 = Convert.ToString(dr["DPRDocument2URL"]);

                    model.Village = Convert.ToString(dr["VILL_Name"]);
                    model.GramPanchayat = Convert.ToString(dr["GP_Name"]);
                    model.District = Convert.ToString(dr["Dist"]);
                    model.NameofArea = Convert.ToString(dr["Area"]);
					model.ActionStatus = Convert.ToString(dr["ApplicationStatus"]);
					string filePdfFileName = DS.Tables[0].Columns.Contains("ApplicationPDF") ? Convert.ToString(dr["ApplicationPDF"]) : "";
                    if (!string.IsNullOrEmpty(filePdfFileName))
                        model.ApplicationPDFName = filePdfFileName.Substring(1, filePdfFileName.Length - 1);
                }

				if (DS != null && DS.Tables[1].Rows.Count > 0)
				{
					model.ApprovalId = Convert.ToInt32(DS.Tables[1].Rows[0]["StatusID"].ToString());
					model.ApprovalName = Convert.ToString(DS.Tables[1].Rows[0]["StatusDesc"].ToString());
					model.RejectId = Convert.ToInt32(DS.Tables[1].Rows[1]["StatusID"].ToString());
					model.RejectName = Convert.ToString(DS.Tables[1].Rows[1]["StatusDesc"].ToString());
					model.IsButtonShow = Convert.ToBoolean(DS.Tables[1].Rows[2]["StatusDesc"].ToString());
					model.IsCCFLevel = Convert.ToBoolean(DS.Tables[1].Rows[2]["isccf"].ToString());
				}

				List<SelectListItem> List = new List<SelectListItem>();
                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    List.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });
                }
                model.ListRejectedReason = List;
				string host = HttpContext.Request.Url.Host.ToLower();
				if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[3].Rows)
                    {
                        AttachmentsViews view = new AttachmentsViews();
                        view.Action = Convert.ToString(dr["Action"]);
                        view.Users = Convert.ToString(dr["USER"]);
						if (host == "localhost")
						{
							view.Documents = Convert.ToString(dr["Attachment"]).Replace("~", string.Empty);
						}
						else
						{
							//view.Documents = Convert.ToString(dr["Attachment"]).Replace("~", "FMDSS");
                            view.Documents = Convert.ToString(dr["Attachment"]).Replace("~", string.Empty);
						}
						view.Remarks = Convert.ToString(dr["remarks"]);
                        model.DocumentsViews.Add(view);
                    }
                }
                if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[4].Rows)
                    {
                        YearsDetailsList view = new YearsDetailsList();
                        view.Current = Convert.ToString(dr["CURRENT"]);
                        view.Prev = Convert.ToString(dr["PREV"]);
                        view.End = Convert.ToString(dr["END"]);
                        view.WorkDesc = Convert.ToString(dr["Workdescription"]);
                        model.GetWorkList.Add(view);
                    }
                }

                if (DS.Tables[5] != null && DS.Tables[5].Rows.Count > 0)
                {
                    ViewBag.CurrentYear = Convert.ToInt32(DS.Tables[5].Rows[0][0]);
                    ViewBag.PrevYear = Convert.ToInt32(DS.Tables[5].Rows[1][0]);
                    ViewBag.EndYear = Convert.ToInt32(DS.Tables[5].Rows[2][0]);
                }

                model.DocumentList = new List<DocumentList>();
                if (DS.Tables.Contains("Table6") && DS.Tables[6].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[6].Rows)
                    {
                        DocumentList list = new DocumentList();
                        string filepath = string.Empty;
                        list.FileName = Convert.ToString(dr["FileName"]);
                        filepath = Convert.ToString(dr["FilePath"]);
                        list.FilePath = filepath.Substring(1, filepath.Length - 1);

                        list.FileType = Convert.ToInt32(dr["FileType"]);
                        model.DocumentList.Add(list);
                    }
                }

                model.UserCommantList = new List<CommandList>();
                if (DS.Tables.Contains("Table7") && DS.Tables[7].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[7].Rows)
                    {
                        CommandList list = new CommandList();
                        list.Name = Convert.ToString(dr["Name"]);
                        list.Commants = Convert.ToString(dr["Commnets"]);
                        list.StatusDesc = Convert.ToString(dr["StatusDesc"]);
                        model.UserCommantList.Add(list);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
			//Export(model);
            return PartialView("ADAppReviewApprover", model);

        }


        public ActionResult ADAppReviewApprover(AmritaDeviAwardModel obj, HttpPostedFileBase ReviewApprovalDocument, string command, int TechnicalPersonID = 0, string OTP = "", string TransationID = "")
        {
			//if (command.Equals("printpdf"))
			//{
			//	return RedirectToAction("GetADRevApprvDetails", new { id = obj.RequestID });
			//}
			//else
			//{
				AmritaDeviInfo repo = new AmritaDeviInfo();
				string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
				string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
				Int32 UserID = Convert.ToInt32(Session["UserID"]);

				DataTable dt;

				string IDs = string.Empty;

				try
				{
				string FilePath = string.Empty;
				string host = HttpContext.Request.Url.Host.ToLower();
				if (host == "localhost")
				{
					FilePath= "~/AmritaDeviDocument/";
				}
				else
				{
					FilePath = "~/AmritaDeviDocument/";
				}
					
					string Document = "", path = "";

					if (ReviewApprovalDocument != null && ReviewApprovalDocument.ContentLength > 0)
					{
						Document = Path.GetFileName(ReviewApprovalDocument.FileName);
						String FileFullName = DateTime.Now.Ticks + "_" + Document;
						path = Path.Combine(FilePath, FileFullName);
						obj.ReviewApprovalDocument = path;
						ReviewApprovalDocument.SaveAs(Server.MapPath(FilePath + FileFullName));
					}
					else
					{
						obj.ReviewApprovalDocument = "";
					}

					if (TechnicalPersonID > 0)
					{
						obj.UserId = UserID;
						obj.AssignTo = UserID;
						obj.ActionStatus = TechnicalPersonID.ToString();
						dt = repo.SubmitCommantbyTechandStateLeval(obj);
						if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(OTP) && !string.IsNullOrEmpty(TransationID))
						{
							#region Call E-Sign API
							clsVerifyOTP request = new clsVerifyOTP();
							request.otp = OTP;
							request.transactionid = TransationID;
							clsVerifyOTPResponce response = FMDSS.App_Start.cls_ESignIntegration.VerifyOTPAndGenrateTransation(request, obj.RequestID, "2", "tbl_AD_Award");

							if (!string.IsNullOrEmpty(response.TransactionId))
								TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " has been Approved Sucessfully and Genrated PDF with E-Sign";
							else
								TempData["Approve"] = "Request Id:" + Session["RequestId"].ToString() + " has been Approved Sucessfully but not Genrated PDF with E-Sign";
							#endregion
						}
					}
					else
					{
						obj.UserId = UserID;
						obj.AssignTo = UserID;


						obj.ActionStatus = command;
						//obj.ActionStatus = "5";
						if (obj.RejectedReason != null)
						{ obj.Reason = string.Join(",", obj.RejectedReason); }


						dt = repo.SubmitADReviewApprover(obj);
					
						if (command == "6")
						{
							TempData["msg"] = "Requested id “" + obj.RequestID + "” is successfully reviewed. ";
						}
						else if (command == "5")
						{
							TempData["msg"] = "Requested id “" + obj.RequestID + "” is successfully approved. ";
						}
						else if (command == "9")
						{
							TempData["msg"] = "Requested id “" + obj.RequestID + "” is successfully reviewed and approved. ";
						}
						else if (command == "3")
						{
							TempData["msg"] = "Requested id “" + obj.RequestID + "” is successfully approved. ";
						}
						else if (command == "7")
						{
							TempData["msg"] = "Requested id “" + obj.RequestID + "” is successfully rejected. ";
						}
						else if (command == "10")
						{
							TempData["msg"] = "Requested id “" + obj.RequestID + "” is successfully rejected. ";
						}
						else
						{
							TempData["msg"] = null;
						}
					
				}
					return RedirectToAction("AmritaDeviAwardReviewApprover");
				}
				catch (Exception ex)
				{
					new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

				}
			//}
            return null;

        }

		[HttpPost]
		public ActionResult ExportToExcel(string hdn_myaction, string hdn_pending,string hdn_tobeassign)
		{
			List<AmritaDeviAwardModel> obj = new List<AmritaDeviAwardModel>();
			List<AmritaDeviAwardModel> obj1 = new List<AmritaDeviAwardModel>();
			List<AmritaDeviAwardModel> obj2 = new List<AmritaDeviAwardModel>();

			AmritaDeviInfo repo = new AmritaDeviInfo();
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			Int64 UserID = Convert.ToInt64(Session["UserID"]);
			DataSet DS = new DataSet();
			try
			{
				DS = repo.ADReviewApprover(UserID);
				int count = 1;
				foreach (DataRow dr in DS.Tables[0].Rows)
				{
					obj.Add(
						new AmritaDeviAwardModel()
						{
							Index = count,
							RequestID = Convert.ToString(dr["RequestID"].ToString()),
							FirstName1 = Convert.ToString(dr["Name"].ToString()),
							AwardCategory = Convert.ToString(dr["CategoryName"].ToString()),
							LandPlace = Convert.ToString(dr["LandPlace"].ToString()),
							StatusName = Convert.ToString(dr["Statusdesc"].ToString()),
						});
					count += 1;
				}

				ViewData["ToBeAssigned"] = obj;

				count = 1;

				foreach (DataRow dr in DS.Tables[1].Rows)
				{
					obj1.Add(
						new AmritaDeviAwardModel()
						{
							Index = count,
							RequestID = Convert.ToString(dr["RequestID"].ToString()),
							FirstName1 = Convert.ToString(dr["Name"].ToString()),
							AwardCategory = Convert.ToString(dr["CategoryName"].ToString()),
							LandPlace = Convert.ToString(dr["LandPlace"].ToString()),
							StatusName = Convert.ToString(dr["Statusdesc"].ToString()),
						});
					count += 1;
				}


				ViewData["PendingRequests"] = obj1;

				count = 1;

				foreach (DataRow dr in DS.Tables[2].Rows)
				{
					obj2.Add(
						new AmritaDeviAwardModel()
						{
							Index = count,
							RequestID = Convert.ToString(dr["RequestID"].ToString()),
							FirstName1 = Convert.ToString(dr["Name"].ToString()),
							AwardCategory = Convert.ToString(dr["CategoryName"].ToString()),
							LandPlace = Convert.ToString(dr["LandPlace"].ToString()),
							StatusName = Convert.ToString(dr["Statusdesc"].ToString()),

						});
					count += 1;
				}

				ViewData["MyActions"] = obj2;

                 
				var gv = new GridView();
				List<PrintAmritaDeviAwardModel> ObjNew1 = new List<PrintAmritaDeviAwardModel>();
				string xlsFileName = string.Empty;
				if (hdn_myaction == "myaction")
				{
				
					 obj2.ForEach(x=>ObjNew1.Add(new PrintAmritaDeviAwardModel() {
						RequestID =x.RequestID,
						Name =x.FirstName1,
						CategoryName =x.AwardCategory,
						LandPlace =x.LandPlace,
						StatusName =x.StatusName
					}));
					gv.DataSource = ObjNew1;
					xlsFileName = "MyAction" + "_" + DateTime.Now.ToString("yyyy-MM-dd");
				}
				if (hdn_pending == "pending")
				{
					obj1.ForEach(x => ObjNew1.Add(new PrintAmritaDeviAwardModel()
					{
						RequestID = x.RequestID,
						Name = x.FirstName1,
						CategoryName = x.AwardCategory,
						LandPlace = x.LandPlace,
						StatusName = x.StatusName
					}));
					gv.DataSource = ObjNew1;
					xlsFileName = "Pending" + "_" + DateTime.Now.ToString("yyyy-MM-dd");
				}
				if (hdn_tobeassign == "tobeassign")
				{
					obj.ForEach(x => ObjNew1.Add(new PrintAmritaDeviAwardModel()
					{
						RequestID = x.RequestID,
						Name = x.FirstName1,
						CategoryName = x.AwardCategory,
						LandPlace = x.LandPlace,
						StatusName = x.StatusName
					}));
					gv.DataSource = ObjNew1;
					xlsFileName = "ToBeAssign" + "_" + DateTime.Now.ToString("yyyy-MM-dd");
				}
				
				gv.DataBind();

				Response.ClearContent();
				Response.Buffer = true;
				Response.AddHeader("content-disposition", "attachment; filename="+ xlsFileName+".xls");
				Response.ContentType = "application/ms-excel";

				Response.Charset = "";
				StringWriter objStringWriter = new StringWriter();
				HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
				for (int i = 0; i < gv.Rows.Count; i++)
				{
					GridViewRow row = gv.Rows[i];
					//ApplytextstyletoeachRow
					row.Cells[0].Attributes.Add("class", "textmode");
				}
				gv.RenderControl(objHtmlTextWriter);
				string style = @"<style>.textmode{mso-number-format:\@;}.textmode1{mso-number-format:'0';}</style>";
				Response.Write(style);
				Response.Output.Write(objStringWriter.ToString());
				Response.Flush();
				Response.End();
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

			}

			return View("AmritaDeviAwardReviewApprover");

		}

		[HttpPost]
		public ActionResult Export(string hdn_rqid)
		{
			DataSet DS = new DataSet();
			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
			Int64 UserID = Convert.ToInt64(Session["UserID"]);

			AmritaDeviInfo repo = new AmritaDeviInfo();
			AmritaDeviAwardModel model = new AmritaDeviAwardModel();

			try
			{

				DS = repo.GetADRevApprvDetailsView(UserID.ToString(), hdn_rqid,"ADReviewApproverlistView");

				foreach (DataRow dr in DS.Tables[0].Rows)
				{
					model.RequestID = Convert.ToString(dr["RequestID"].ToString());
					model.FirstName1 = Convert.ToString(dr["Name"].ToString());
					model.ReferedBy = Convert.ToString(dr["ReferedBYSSOID"].ToString());
					model.StatusName = Convert.ToString(dr["StatusDesc"].ToString());
					model.AwardCategory = Convert.ToString(dr["CategoryName"].ToString());
					model.AwardAmount = Convert.ToDecimal(dr["AwardAmount"].ToString());
					model.PersonalLandHactorDesc = Convert.ToString(dr["PersonalLandDetail"].ToString());
					model.PersonalLandHactor = Convert.ToString(dr["PersonalLandHectare"].ToString());
					model.CollectiveLandDesc = Convert.ToString(dr["CommunityLandDETAIL"]);
					model.CollectiveLandHactor = Convert.ToString(dr["CommunityLandHectare"]);
					model.RevenueLandDesc = Convert.ToString(dr["RevenueLandDETAIL"]);
					model.RevenueLandHactor = Convert.ToString(dr["RevenueLandHectare"]);
					model.Landhacktor = Convert.ToString(dr["LandRealArea"]);
					model.LandPlace = Convert.ToString(dr["LandPlace"]);
					model.ForestLandDesc = Convert.ToString(dr["ForestLandDetail"]);
					model.ForestLandHactor = Convert.ToString(dr["ForestLandHectare"]);
					model.Document1 = Convert.ToString(dr["DPRDocument1URL"]);
					model.Document2 = Convert.ToString(dr["DPRDocument2URL"]);

					model.Village = Convert.ToString(dr["VILL_Name"]);
					model.GramPanchayat = Convert.ToString(dr["GP_Name"]);
					model.District = Convert.ToString(dr["Dist"]);
					model.NameofArea = Convert.ToString(dr["Area"]);
					//model.ActionStatus = Convert.ToString(dr["ApplicationStatus"]);
					string filePdfFileName = DS.Tables[0].Columns.Contains("ApplicationPDF") ? Convert.ToString(dr["ApplicationPDF"]) : "";
					if (!string.IsNullOrEmpty(filePdfFileName))
						model.ApplicationPDFName = filePdfFileName.Substring(1, filePdfFileName.Length - 1);
				}

				//if (DS != null && DS.Tables[1].Rows.Count > 0)
				//{
				//	model.ApprovalId = Convert.ToInt32(DS.Tables[1].Rows[0]["StatusID"].ToString());
				//	model.ApprovalName = Convert.ToString(DS.Tables[1].Rows[0]["StatusDesc"].ToString());
				//	model.RejectId = Convert.ToInt32(DS.Tables[1].Rows[1]["StatusID"].ToString());
				//	model.RejectName = Convert.ToString(DS.Tables[1].Rows[1]["StatusDesc"].ToString());
				//	model.IsButtonShow = Convert.ToBoolean(DS.Tables[1].Rows[2]["StatusDesc"].ToString());
				//	model.IsCCFLevel = Convert.ToBoolean(DS.Tables[1].Rows[2]["isccf"].ToString());
				//}

				List<SelectListItem> List = new List<SelectListItem>();
				foreach (DataRow dr in DS.Tables[2].Rows)
				{
					List.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });
				}
				model.ListRejectedReason = List;

				if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
				{
					foreach (DataRow dr in DS.Tables[3].Rows)
					{
						AttachmentsViews view = new AttachmentsViews();
						view.Action = Convert.ToString(dr["Action"]);
						view.Users = Convert.ToString(dr["USER"]);
						view.Documents = Convert.ToString(dr["Attachment"]).Replace("~", string.Empty);
						view.Remarks = Convert.ToString(dr["remarks"]);
						model.DocumentsViews.Add(view);
					}
				}
				if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
				{
					foreach (DataRow dr in DS.Tables[4].Rows)
					{
						YearsDetailsList view = new YearsDetailsList();
						view.Current = Convert.ToString(dr["CURRENT"]);
						view.Prev = Convert.ToString(dr["PREV"]);
						view.End = Convert.ToString(dr["END"]);
						view.WorkDesc = Convert.ToString(dr["Workdescription"]);
						model.GetWorkList.Add(view);
					}
				}

				if (DS.Tables[5] != null && DS.Tables[5].Rows.Count > 0)
				{
					ViewBag.CurrentYear = Convert.ToInt32(DS.Tables[5].Rows[0][0]);
					ViewBag.PrevYear = Convert.ToInt32(DS.Tables[5].Rows[1][0]);
					ViewBag.EndYear = Convert.ToInt32(DS.Tables[5].Rows[2][0]);
				}

				model.DocumentList = new List<DocumentList>();
				if (DS.Tables.Contains("Table6") && DS.Tables[6].Rows.Count > 0)
				{
					foreach (DataRow dr in DS.Tables[6].Rows)
					{
						DocumentList list = new DocumentList();
						string filepath = string.Empty;
						list.FileName = Convert.ToString(dr["FileName"]);
						filepath = Convert.ToString(dr["FilePath"]);
						list.FilePath = filepath.Substring(2, filepath.Length - 2);

						list.FileType = Convert.ToInt32(dr["FileType"]);
						model.DocumentList.Add(list);
					}
				}

				model.UserCommantList = new List<CommandList>();
				if (DS.Tables.Contains("Table7") && DS.Tables[7].Rows.Count > 0)
				{
					foreach (DataRow dr in DS.Tables[7].Rows)
					{
						CommandList list = new CommandList();
						list.Name = Convert.ToString(dr["Name"]);
						list.Commants = Convert.ToString(dr["Commnets"]);
						list.StatusDesc = Convert.ToString(dr["StatusDesc"]);
						model.UserCommantList.Add(list);
					}
				}
			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

			}






			Document doc = new Document();
			//string filepath = string.Empty;
			//Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

			//var FontColour = new BaseColor(0, 0, 0); string _FileName = Server.MapPath("~/RescueDocument/") + fileName;
			string filePath = Server.MapPath("~/AmritaDeviDocument/ApplicationPDF/"); 
			string fileName = "File_"+DateTime.Now.Ticks.ToString() + ".pdf";
			string fullFilePath = filePath + fileName;
			PdfWriter.GetInstance(doc, new FileStream(fullFilePath, FileMode.Create));

			string fontpaths1 = string.Format("C:\\Windows\\Fonts\\{0}", "Mangal.ttf");
			BaseFont dev1 = BaseFont.CreateFont(fontpaths1, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			iTextSharp.text.Font hindi2 = new iTextSharp.text.Font(dev1, 14, iTextSharp.text.Font.BOLD);


			//string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", "Devanagari.ttf");
			//BaseFont mang = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			//iTextSharp.text.Font hindi = new iTextSharp.text.Font(mang, 8);


			string fontpath = string.Format("C:\\Windows\\Fonts\\{0}", "k010_1.ttf");
			BaseFont dev = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			iTextSharp.text.Font hindi = new iTextSharp.text.Font(dev, 8);
			iTextSharp.text.Font hindi1 = new iTextSharp.text.Font(dev, 8);
			//iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 12);


			//string fontpaths = string.Format("C:\\Windows\\Fonts\\{0}", "Krutidev.ttf");
			//BaseFont devbold = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			////iTextSharp.text.Font verdana = FontFactory.GetFont("Krutidev", 16);
			//iTextSharp.text.Font hindibold = new iTextSharp.text.Font(dev, 18, iTextSharp.text.Font.BOLD);
			//iTextSharp.text.Font hindiboldsmall = new iTextSharp.text.Font(dev, 12, iTextSharp.text.Font.BOLD);


			var subheadfont = FontFactory.GetFont("Times New Roman", 10);
			var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 6);
			var Normalfont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 6);
			//var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
			//var myfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);
			//document.Open();

			doc.Open();

			PdfPTable blacnkTable = new PdfPTable(4);
			PdfPCell blankCell = new PdfPCell(new Phrase("")) { Border = 0 };
			blankCell.Colspan = 4;
			blankCell.Padding = 10;
			blacnkTable.AddCell(blankCell);

			PdfPTable HeaderTable = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			HeaderTable.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
			PdfPCell cells = new PdfPCell() { Border = 4 };
			HeaderTable.TotalWidth = 120;
			HeaderTable.SetTotalWidth(new float[] { 20f, 0f, 0f, 0f });
			
			cells = new PdfPCell(new Phrase("  AMRITA DEVI AWARD", subheadfont)) { Border = 2 };
			cells.Colspan = 4;
			cells.HorizontalAlignment = Element.ALIGN_CENTER;
			HeaderTable.AddCell(cells);
			doc.Add(HeaderTable);
			doc.Add(blacnkTable);
			//----------------header end
			PdfPTable table1=new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell cell11 = new PdfPCell(new Phrase("Reffered By", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE ,BorderColor=BaseColor.BLACK};
			cell11.Colspan = 2;
			cell11.Padding = 4;
			cell11.BorderWidthLeft= 1;
			cell11.BorderWidthRight = 1;
			cell11.BorderWidthTop = 1;
			cell11.BorderWidthBottom = 1;
			cell11.HorizontalAlignment = Element.ALIGN_CENTER;
			
			table1.AddCell(cell11);

			PdfPCell cell12 = new PdfPCell(new Phrase(model.ReferedBy, boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell12.Colspan = 2;
			cell12.Padding = 4;
			cell12.BorderWidthLeft = 0;
			cell12.BorderWidthRight = 1;
			cell12.BorderWidthTop = 1;
			cell12.BorderWidthBottom = 1;
			cell12.HorizontalAlignment = Element.ALIGN_CENTER;
			table1.AddCell(cell12);
		
			doc.Add(table1);
			doc.Add(blacnkTable);
			//--------------------first table end
			PdfPTable table2 = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell cell21 = new PdfPCell(new Phrase("RequestID", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell21.Padding = 4;
			cell21.BorderWidthLeft = 1;
			cell21.BorderWidthRight = 1;
			cell21.BorderWidthTop = 1;
			cell21.BorderWidthBottom = 0;
			cell21.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell21);
			PdfPCell cell22 = new PdfPCell(new Phrase("Name", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell22.Padding = 4;
			cell22.BorderWidthLeft = 0;
			cell22.BorderWidthRight = 1;
			cell22.BorderWidthTop = 1;
			cell22.BorderWidthBottom = 0;
			cell22.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell22);
			PdfPCell cell23 = new PdfPCell(new Phrase("Status", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell23.Colspan = 2;
			cell23.Padding = 4;
			cell23.BorderWidthLeft = 0;
			cell23.BorderWidthRight = 1;
			cell23.BorderWidthTop = 1;
			cell23.BorderWidthBottom = 0;
			cell23.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell23);
			PdfPCell cell24 = new PdfPCell(new Phrase(model.RequestID, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell24.Padding = 4;
			cell24.BorderWidthLeft = 1;
			cell24.BorderWidthRight = 1;
			cell24.BorderWidthTop = 1;
			cell24.BorderWidthBottom = 0;
			cell24.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell24);
			PdfPCell cell25 = new PdfPCell(new Phrase(model.FirstName1, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell25.Padding = 4;
			cell25.BorderWidthLeft = 0;
			cell25.BorderWidthRight = 1;
			cell25.BorderWidthTop = 1;
			cell25.BorderWidthBottom = 0;
			cell25.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell25);
			PdfPCell cell26 = new PdfPCell(new Phrase(model.StatusName, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell26.Colspan = 2;
			cell26.Padding = 4;
			cell26.BorderWidthLeft = 0;
			cell26.BorderWidthRight = 1;
			cell26.BorderWidthTop = 1;
			cell26.BorderWidthBottom = 0;
			cell26.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell26);


			PdfPCell cell27 = new PdfPCell(new Phrase("District", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell27.Colspan = 2;
			cell27.Padding = 4;
			cell27.BorderWidthLeft = 1;
			cell27.BorderWidthRight = 1;
			cell27.BorderWidthTop = 1;
			cell27.BorderWidthBottom = 0;
			cell27.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell27);

			PdfPCell cell28 = new PdfPCell(new Phrase("Gram Panchayadt", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell28.Colspan = 2;
			cell28.Padding = 4;
			cell28.BorderWidthLeft = 0;
			cell28.BorderWidthRight = 1;
			cell28.BorderWidthTop = 1;
			cell28.BorderWidthBottom = 0;
			cell28.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell28);

			PdfPCell cell29 = new PdfPCell(new Phrase("Village", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell29.Colspan = 2;
			cell29.Padding = 4;
			cell29.BorderWidthLeft = 0;
			cell29.BorderWidthRight = 1;
			cell29.BorderWidthTop = 1;
			cell29.BorderWidthBottom = 0;
			cell29.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell29);

			PdfPCell cell30 = new PdfPCell(new Phrase(model.District, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell30.Colspan = 2;
			cell30.Padding = 4;
			cell30.BorderWidthLeft = 1;
			cell30.BorderWidthRight = 1;
			cell30.BorderWidthTop = 1;
			cell30.BorderWidthBottom = 0;
			cell30.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell30);

			PdfPCell cell31 = new PdfPCell(new Phrase(model.GramPanchayat, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell31.Colspan = 2;
			cell31.Padding = 4;
			cell31.BorderWidthLeft = 0;
			cell31.BorderWidthRight = 1;
			cell31.BorderWidthTop = 1;
			cell31.BorderWidthBottom = 0;
			cell31.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell31);

			PdfPCell cell32 = new PdfPCell(new Phrase(model.Village, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell32.Colspan = 2;
			cell32.Padding = 4;
			cell32.BorderWidthLeft = 0;
			cell32.BorderWidthRight = 1;
			cell32.BorderWidthTop = 1;
			cell32.BorderWidthBottom = 0;
			cell32.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell32);

			PdfPCell cell33 = new PdfPCell(new Phrase("Area", boldFont)) { Border = 0, BackgroundColor = BaseColor.GRAY,BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell33.Colspan = 4;
			cell33.Padding = 4;
			cell33.BorderWidthLeft = 1;
			cell33.BorderWidthRight = 1;
			cell33.BorderWidthTop = 1;
			cell33.BorderWidthBottom = 0;
			cell33.HorizontalAlignment = Element.ALIGN_LEFT;
			table2.AddCell(cell33);

			PdfPCell cell34 = new PdfPCell(new Phrase(model.NameofArea, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell34.Colspan = 4;
			cell34.Padding = 4;
			cell34.BorderWidthLeft = 1;
			cell34.BorderWidthRight = 1;
			cell34.BorderWidthTop = 1;
			cell34.BorderWidthBottom = 0;
			cell34.HorizontalAlignment = Element.ALIGN_LEFT;
			table2.AddCell(cell34);

			PdfPCell cell35 = new PdfPCell(new Phrase("Award Category", boldFont)) { Border = 0, BackgroundColor = BaseColor.GRAY, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell35.Colspan = 4;
			cell35.Padding = 4;
			cell35.BorderWidthLeft = 1;
			cell35.BorderWidthRight = 1;
			cell35.BorderWidthTop = 1;
			cell35.BorderWidthBottom = 0;
			cell35.HorizontalAlignment = Element.ALIGN_LEFT;
			table2.AddCell(cell35);

			PdfPCell cell36 = new PdfPCell(new Phrase(model.AwardCategory, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell36.Colspan = 4;
			cell36.Padding = 4;
			cell36.BorderWidthLeft = 1;
			cell36.BorderWidthRight = 1;
			cell36.BorderWidthTop = 1;
			cell36.BorderWidthBottom = 0;
			cell36.HorizontalAlignment = Element.ALIGN_LEFT;
			table2.AddCell(cell36);

			PdfPCell cell37 = new PdfPCell(new Phrase("Award Amount", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell37.Padding = 4;
			cell37.BorderWidthLeft = 1;
			cell37.BorderWidthRight = 1;
			cell37.BorderWidthTop = 1;
			cell37.BorderWidthBottom = 0;
			cell37.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell37);

			PdfPCell cell38 = new PdfPCell(new Phrase("Personal Land Details", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell38.Padding = 4;
			cell38.BorderWidthLeft = 0;
			cell38.BorderWidthRight = 1;
			cell38.BorderWidthTop = 1;
			cell38.BorderWidthBottom = 0;
			cell38.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell38);

			PdfPCell cell39 = new PdfPCell(new Phrase("Personal Land Hactare", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell39.Colspan = 2;
			cell39.Padding = 4;
			cell39.BorderWidthLeft = 0;
			cell39.BorderWidthRight = 1;
			cell39.BorderWidthTop = 1;
			cell39.BorderWidthBottom = 0;
			cell39.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell39);

			PdfPCell cell40 = new PdfPCell(new Phrase(model.AwardAmount.ToString(), boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell40.Padding = 4;
			cell40.BorderWidthLeft = 1;
			cell40.BorderWidthRight = 1;
			cell40.BorderWidthTop = 1;
			cell40.BorderWidthBottom = 0;
			cell40.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell40);

			PdfPCell cell41 = new PdfPCell(new Phrase(model.PersonalLandHactorDesc, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell41.Padding = 4;
			cell41.BorderWidthLeft = 0;
			cell41.BorderWidthRight = 1;
			cell41.BorderWidthTop = 1;
			cell41.BorderWidthBottom = 0;
			cell41.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell41);
			PdfPCell cell42 = new PdfPCell(new Phrase(model.PersonalLandHactor, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell42.Colspan = 2;
			cell42.Padding = 4;
			cell42.BorderWidthLeft = 0;
			cell42.BorderWidthRight = 1;
			cell42.BorderWidthTop = 1;
			cell42.BorderWidthBottom = 0;
			cell42.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell42);

			PdfPCell cell43 = new PdfPCell(new Phrase("Collective Land Details", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell43.Padding = 4;
			cell43.BorderWidthLeft = 1;
			cell43.BorderWidthRight = 1;
			cell43.BorderWidthTop = 1;
			cell43.BorderWidthBottom = 0;
			cell43.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell43);
			PdfPCell cell44 = new PdfPCell(new Phrase("Collective Land Hactare", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell44.Padding = 4;
			cell44.BorderWidthLeft = 0;
			cell44.BorderWidthRight = 1;
			cell44.BorderWidthTop = 1;
			cell44.BorderWidthBottom = 0;
			cell44.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell44);
			PdfPCell cell45 = new PdfPCell(new Phrase("Revenue Land Details", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell45.Colspan = 2;
			cell45.Padding = 4;
			cell45.BorderWidthLeft = 0;
			cell45.BorderWidthRight = 1;
			cell45.BorderWidthTop = 1;
			cell45.BorderWidthBottom = 0;
			cell45.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell45);

			PdfPCell cell46 = new PdfPCell(new Phrase(model.CollectiveLandDesc, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell46.Padding = 4;
			cell46.BorderWidthLeft = 1;
			cell46.BorderWidthRight = 1;
			cell46.BorderWidthTop = 1;
			cell46.BorderWidthBottom = 0;
			cell46.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell46);
			PdfPCell cell47 = new PdfPCell(new Phrase(model.CollectiveLandHactor, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell47.Padding = 4;
			cell47.BorderWidthLeft = 0;
			cell47.BorderWidthRight = 1;
			cell47.BorderWidthTop = 1;
			cell47.BorderWidthBottom = 0;
			cell47.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell47);
			PdfPCell cell48 = new PdfPCell(new Phrase(model.RevenueLandDesc, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell48.Colspan = 2;
			cell48.Padding = 4;
			cell48.BorderWidthLeft = 0;
			cell48.BorderWidthRight = 1;
			cell48.BorderWidthTop = 1;
			cell48.BorderWidthBottom = 0;
			cell48.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell48);

			PdfPCell cell49 = new PdfPCell(new Phrase("Revenue Land Hactare", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell49.Padding = 4;
			cell49.BorderWidthLeft = 1;
			cell49.BorderWidthRight = 1;
			cell49.BorderWidthTop = 1;
			cell49.BorderWidthBottom = 0;
			cell49.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell49);

			PdfPCell cell50 = new PdfPCell(new Phrase("Forest Land Details", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell50.Padding = 4;
			cell50.BorderWidthLeft = 0;
			cell50.BorderWidthRight = 1;
			cell50.BorderWidthTop = 1;
			cell50.BorderWidthBottom = 0;
			cell50.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell50);
			PdfPCell cell51 = new PdfPCell(new Phrase("Forest Land Hactare", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell51.Colspan = 2;
			cell51.Padding = 4;
			cell51.BorderWidthLeft = 0;
			cell51.BorderWidthRight = 1;
			cell51.BorderWidthTop = 1;
			cell51.BorderWidthBottom = 0;
			cell51.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell51);

			PdfPCell cell52 = new PdfPCell(new Phrase(model.RevenueLandHactor, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell52.Padding = 4;
			cell52.BorderWidthLeft = 1;
			cell52.BorderWidthRight = 1;
			cell52.BorderWidthTop = 1;
			cell52.BorderWidthBottom = 0;
			cell52.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell52);
			PdfPCell cell53 = new PdfPCell(new Phrase(model.ForestLandDesc, boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell53.Padding = 4;
			cell53.BorderWidthLeft = 0;
			cell53.BorderWidthRight = 1;
			cell53.BorderWidthTop = 1;
			cell53.BorderWidthBottom = 0;
			cell53.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell53);
			PdfPCell cell54 = new PdfPCell(new Phrase(model.ForestLandHactor, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell54.Colspan = 2;
			cell54.Padding = 4;
			cell54.BorderWidthLeft = 0;
			cell54.BorderWidthRight = 1;
			cell54.BorderWidthTop = 1;
			cell54.BorderWidthBottom = 0;
			cell54.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell54);

			PdfPCell cell55 = new PdfPCell(new Phrase("Land Palace", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell55.Padding = 4;
			cell55.BorderWidthLeft = 1;
			cell55.BorderWidthRight = 1;
			cell55.BorderWidthTop = 1;
			cell55.BorderWidthBottom = 0;
			cell55.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell55);
			PdfPCell cell56 = new PdfPCell(new Phrase("Land Hactare", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell56.Padding = 4;
			cell56.BorderWidthLeft = 0;
			cell56.BorderWidthRight = 1;
			cell56.BorderWidthTop = 1;
			cell56.BorderWidthBottom = 0;
			cell56.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell56);
			PdfPCell cell57 = new PdfPCell(new Phrase("KML", boldFont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell57.Colspan = 2;
			cell57.Padding = 4;
			cell57.BorderWidthLeft = 0;
			cell57.BorderWidthRight = 1;
			cell57.BorderWidthTop = 1;
			cell57.BorderWidthBottom = 0;
			cell57.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell57);

			PdfPCell cell58 = new PdfPCell(new Phrase(model.LandPlace, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell58.Padding = 4;
			cell58.BorderWidthLeft = 1;
			cell58.BorderWidthRight = 1;
			cell58.BorderWidthTop = 1;
			cell58.BorderWidthBottom = 1;
			cell58.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell58);
			PdfPCell cell59 = new PdfPCell(new Phrase(model.Landhacktor, Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			//cell21.Colspan = 2;
			cell59.Padding = 4;
			cell59.BorderWidthLeft = 0;
			cell59.BorderWidthRight = 1;
			cell59.BorderWidthTop = 1;
			cell59.BorderWidthBottom = 1;
			cell59.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell59);
			PdfPCell cell60 = new PdfPCell(new Phrase("Image Pending", Normalfont)) { Border = 0, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK, BorderWidth = 1 };
			cell60.Colspan = 2;
			cell60.Padding = 4;
			cell60.BorderWidthLeft = 0;
			cell60.BorderWidthRight = 1;
			cell60.BorderWidthTop = 1;
			cell60.BorderWidthBottom = 1;
			cell60.HorizontalAlignment = Element.ALIGN_CENTER;
			table2.AddCell(cell60);

			doc.Add(table2);

			PdfPTable subHedderTable = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell cell61 = new PdfPCell(new Phrase("Application Review/Approval:-", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell61.Colspan = 4;
			cell61.Padding = 2;
			cell61.HorizontalAlignment = Element.ALIGN_LEFT;

			subHedderTable.AddCell(cell61);
			doc.Add(subHedderTable);
			PdfPTable table3 = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell[] pdfCell;
			pdfCell = new PdfPCell[model.DocumentsViews.Count*2];
			int j = 0;
			foreach (var itm in model.DocumentsViews)
			{
				if (j != pdfCell.Length - 2)
				{
					pdfCell[j] = new PdfPCell(new Phrase(itm.Action + "(" + itm.Users + ")", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCell[j].Colspan = 2;
					pdfCell[j].Padding = 4;
					pdfCell[j].BorderWidthLeft = 1;
					pdfCell[j].BorderWidthRight = 1;
					pdfCell[j].BorderWidthTop = 1;
					pdfCell[j].BorderWidthBottom = 0;
					pdfCell[j].HorizontalAlignment = Element.ALIGN_CENTER;
					table3.AddCell(pdfCell[j]);
					j++;
					pdfCell[j] = new PdfPCell(new Phrase((String.IsNullOrEmpty(itm.Remarks) ? "" : itm.Remarks), boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCell[j].Colspan = 2;
					pdfCell[j].Padding = 4;
					pdfCell[j].BorderWidthLeft = 0;
					pdfCell[j].BorderWidthRight = 1;
					pdfCell[j].BorderWidthTop = 1;
					pdfCell[j].BorderWidthBottom = 0;
					pdfCell[j].HorizontalAlignment = Element.ALIGN_CENTER;
					table3.AddCell(pdfCell[j]);
					j++;
				}
				else
				{
					pdfCell[j] = new PdfPCell(new Phrase(itm.Action + "(" + itm.Users + ")", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCell[j].Colspan = 2;
					pdfCell[j].Padding = 4;
					pdfCell[j].BorderWidthLeft = 1;
					pdfCell[j].BorderWidthRight = 1;
					pdfCell[j].BorderWidthTop = 1;
					pdfCell[j].BorderWidthBottom = 1;
					pdfCell[j].HorizontalAlignment = Element.ALIGN_CENTER;
					table3.AddCell(pdfCell[j]);
					j++;
					pdfCell[j] = new PdfPCell(new Phrase((String.IsNullOrEmpty(itm.Remarks) ? "" : itm.Remarks), boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCell[j].Colspan = 2;
					pdfCell[j].Padding = 4;
					pdfCell[j].BorderWidthLeft = 0;
					pdfCell[j].BorderWidthRight = 1;
					pdfCell[j].BorderWidthTop = 1;
					pdfCell[j].BorderWidthBottom = 1;
					pdfCell[j].HorizontalAlignment = Element.ALIGN_CENTER;
					table3.AddCell(pdfCell[j]);
					j++;
				}
			}
            doc.Add(table3);

			PdfPTable subHedderTable1 = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell cell62 = new PdfPCell(new Phrase("Comment Of CCF Level:-", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell62.Colspan = 4;
			cell62.Padding = 2;
			cell62.HorizontalAlignment = Element.ALIGN_LEFT;
            subHedderTable1.AddCell(cell62);

			doc.Add(subHedderTable1);
			PdfPTable table4 = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell[] pdfCells;
			PdfPCell cell63 = new PdfPCell(new Phrase("Name", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			//cell63.Colspan = 4;
			cell63.Padding = 2;
			cell63.BorderWidthLeft = 1;
			cell63.BorderWidthRight = 1;
			cell63.BorderWidthTop = 1;
			cell63.BorderWidthBottom = 0;
			cell63.HorizontalAlignment = Element.ALIGN_CENTER;
			table4.AddCell(cell63);
			PdfPCell cell64 = new PdfPCell(new Phrase("Status", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			//cell63.Colspan = 4;
			cell64.Padding = 2;
			cell64.BorderWidthLeft = 0;
			cell64.BorderWidthRight = 0;
			cell64.BorderWidthTop = 1;
			cell64.BorderWidthBottom = 0;
			cell64.HorizontalAlignment = Element.ALIGN_CENTER;
			table4.AddCell(cell64);
			PdfPCell cell65 = new PdfPCell(new Phrase("Comments", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell65.Colspan = 2;
			cell65.Padding = 2;
			cell65.BorderWidthLeft = 1;
			cell65.BorderWidthRight = 1;
			cell65.BorderWidthTop = 1;
			cell65.BorderWidthBottom = 0;
			cell65.HorizontalAlignment = Element.ALIGN_CENTER;
			table4.AddCell(cell65);

			pdfCells = new PdfPCell[model.UserCommantList.Count * 3];
			int k = 0;
			foreach (var itm in model.UserCommantList)
			{
				if (k != pdfCells.Length - 3)
				{
					pdfCells[k] = new PdfPCell(new Phrase(itm.Name, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					//pdfCell[k].Colspan = 2;
					pdfCells[k].Padding = 4;
					pdfCells[k].BorderWidthLeft = 1;
					pdfCells[k].BorderWidthRight = 1;
					pdfCells[k].BorderWidthTop = 1;
					pdfCells[k].BorderWidthBottom = 0;
					pdfCells[k].HorizontalAlignment = Element.ALIGN_CENTER;
					table4.AddCell(pdfCells[k]);
					k++;
					pdfCells[k] = new PdfPCell(new Phrase(itm.StatusDesc, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					//pdfCell[k].Colspan = 2;
					pdfCells[k].Padding = 4;
					pdfCells[k].BorderWidthLeft = 0;
					pdfCells[k].BorderWidthRight = 1;
					pdfCells[k].BorderWidthTop = 1;
					pdfCells[k].BorderWidthBottom = 0;
					pdfCells[k].HorizontalAlignment = Element.ALIGN_CENTER;
					table4.AddCell(pdfCells[k]);
					k++;
					pdfCells[k] = new PdfPCell(new Phrase(itm.Commants, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCells[k].Colspan = 2;
					pdfCells[k].Padding = 4;
					pdfCells[k].BorderWidthLeft = 0;
					pdfCells[k].BorderWidthRight = 1;
					pdfCells[k].BorderWidthTop = 1;
					pdfCells[k].BorderWidthBottom = 0;
					pdfCells[k].HorizontalAlignment = Element.ALIGN_CENTER;
					table4.AddCell(pdfCells[k]);
					k++;
				}
				else
				{
					pdfCells[k] = new PdfPCell(new Phrase(itm.Name, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					//pdfCell[k].Colspan = 2;
					pdfCells[k].Padding = 4;
					pdfCells[k].BorderWidthLeft = 1;
					pdfCells[k].BorderWidthRight = 1;
					pdfCells[k].BorderWidthTop = 1;
					pdfCells[k].BorderWidthBottom = 1;
					pdfCells[k].HorizontalAlignment = Element.ALIGN_CENTER;
					table4.AddCell(pdfCells[k]);
					k++;
					pdfCells[k] = new PdfPCell(new Phrase(itm.StatusDesc, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					//pdfCell[k].Colspan = 2;
					pdfCells[k].Padding = 4;
					pdfCells[k].BorderWidthLeft = 0;
					pdfCells[k].BorderWidthRight = 0;
					pdfCells[k].BorderWidthTop = 1;
					pdfCells[k].BorderWidthBottom = 1;
					pdfCells[k].HorizontalAlignment = Element.ALIGN_CENTER;
					table4.AddCell(pdfCells[k]);
					k++;
					pdfCells[k] = new PdfPCell(new Phrase(itm.Commants, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCells[k].Colspan = 2;
					pdfCells[k].Padding = 4;
					pdfCells[k].BorderWidthLeft = 1;
					pdfCells[k].BorderWidthRight = 1;
					pdfCells[k].BorderWidthTop = 1;
					pdfCells[k].BorderWidthBottom = 1;
					pdfCells[k].HorizontalAlignment = Element.ALIGN_CENTER;
					table4.AddCell(pdfCells[k]);
					k++;
				}
			}
			doc.Add(table4);

			PdfPTable subHedderTable11 = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell cell66 = new PdfPCell(new Phrase("Work Descriptions:-", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell66.Colspan = 4;
			cell66.Padding = 2;
			cell66.HorizontalAlignment = Element.ALIGN_LEFT;
			subHedderTable11.AddCell(cell66);

			doc.Add(subHedderTable11);

			PdfPTable table5 = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
			PdfPCell[] pdfCellss;
			PdfPCell cell67 = new PdfPCell(new Phrase("Work Description", boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell67.Padding = 2;
			cell67.BorderWidthLeft = 1;
			cell67.BorderWidthRight = 1;
			cell67.BorderWidthTop = 1;
			cell67.BorderWidthBottom = 0;
			cell67.HorizontalAlignment = Element.ALIGN_CENTER;
			table5.AddCell(cell67);

			PdfPCell cell68 = new PdfPCell(new Phrase(ViewBag.CurrentYear as string, boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell68.Padding = 2;
			cell68.BorderWidthLeft = 1;
			cell68.BorderWidthRight = 1;
			cell68.BorderWidthTop = 1;
			cell68.BorderWidthBottom = 0;
			cell68.HorizontalAlignment = Element.ALIGN_CENTER;
			table5.AddCell(cell68);

			PdfPCell cell69 = new PdfPCell(new Phrase(ViewBag.PrevYear as string, boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell69.Padding = 2;
			cell69.BorderWidthLeft = 1;
			cell69.BorderWidthRight = 1;
			cell69.BorderWidthTop = 1;
			cell69.BorderWidthBottom = 0;
			cell69.HorizontalAlignment = Element.ALIGN_CENTER;
			table5.AddCell(cell69);

			PdfPCell cell70 = new PdfPCell(new Phrase(ViewBag.EndYear as string, boldFont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
			cell70.Padding = 2;
			cell70.BorderWidthLeft = 1;
			cell70.BorderWidthRight = 1;
			cell70.BorderWidthTop = 1;
			cell70.BorderWidthBottom = 0;
			cell70.HorizontalAlignment = Element.ALIGN_CENTER;
			table5.AddCell(cell70);

			pdfCellss = new PdfPCell[model.GetWorkList.Count * 4];
			int l = 0;
			foreach (var itm in model.GetWorkList)
			{
				if (l != pdfCells.Length - 4)
				{
					pdfCellss[l] = new PdfPCell(new Phrase(itm.WorkDesc, hindi)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 1;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 0;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;

					pdfCellss[l] = new PdfPCell(new Phrase(itm.Current, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 0;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 0;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;

					pdfCellss[l] = new PdfPCell(new Phrase(itm.Prev, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 0;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 0;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;

					pdfCellss[l] = new PdfPCell(new Phrase(itm.End, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 1;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 0;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;
				}
				else
				{
					pdfCellss[l] = new PdfPCell(new Phrase(itm.WorkDesc, hindi)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 1;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 1;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;

					pdfCellss[l] = new PdfPCell(new Phrase(itm.Current, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 0;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 1;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;

					pdfCellss[l] = new PdfPCell(new Phrase(itm.Prev, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 0;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 1;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;

					pdfCellss[l] = new PdfPCell(new Phrase(itm.End, Normalfont)) { Border = 1, BackgroundColor = BaseColor.WHITE, BorderColor = BaseColor.BLACK };
					pdfCellss[l].Padding = 2;
					pdfCellss[l].BorderWidthLeft = 1;
					pdfCellss[l].BorderWidthRight = 1;
					pdfCellss[l].BorderWidthTop = 1;
					pdfCellss[l].BorderWidthBottom = 1;
					pdfCellss[l].HorizontalAlignment = Element.ALIGN_CENTER;
					table5.AddCell(pdfCellss[l]);
					l++;
				}
			}
			doc.Add(table5);
					doc.Close();

			System.Diagnostics.Process.Start(fullFilePath);
			return RedirectToAction("AmritaDeviAwardReviewApprover");
		}


		



		public ActionResult GetMyActionApprvDetails(string id, string ActionNames)
        {

            DataSet DS = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            AmritaDeviInfo repo = new AmritaDeviInfo();
            AmritaDeviAwardModel model = new AmritaDeviAwardModel();

            try
            {

                DS = repo.GetADRevApprvDetailsView(UserID.ToString(), id, ActionNames);

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    model.ReferedBy = Convert.ToString(dr["ReferedBYSSOID"]);
                    model.RequestID = Convert.ToString(dr["RequestID"].ToString());
                    model.FirstName1 = Convert.ToString(dr["Name"].ToString());
                    model.StatusName = Convert.ToString(dr["StatusDesc"].ToString());
                    model.AwardCategory = Convert.ToString(dr["CategoryName"].ToString());
                    model.AwardAmount = Convert.ToDecimal(dr["AwardAmount"].ToString());
                    model.GISID = Convert.ToString(dr["GISID"]);
                    model.PersonalLandHactorDesc = Convert.ToString(dr["PersonalLandDetail"].ToString());
                    model.PersonalLandHactor = Convert.ToString(dr["PersonalLandHectare"].ToString());
                    model.CollectiveLandDesc = Convert.ToString(dr["CommunityLandDETAIL"]);
                    model.CollectiveLandHactor = Convert.ToString(dr["CommunityLandHectare"]);
                    model.RevenueLandDesc = Convert.ToString(dr["RevenueLandDETAIL"]);
                    model.RevenueLandHactor = Convert.ToString(dr["RevenueLandHectare"]);
                    model.Landhacktor = Convert.ToString(dr["LandRealArea"]);
                    model.LandPlace = Convert.ToString(dr["LandPlace"]);
                    model.ForestLandDesc = Convert.ToString(dr["ForestLandDetail"]);
                    model.ForestLandHactor = Convert.ToString(dr["ForestLandHectare"]);
                    model.Document1 = Convert.ToString(dr["DPRDocument1URL"]);
                    model.Document2 = Convert.ToString(dr["DPRDocument2URL"]);
                    model.Village = Convert.ToString(dr["VILL_Name"]);
                    model.GramPanchayat = Convert.ToString(dr["GP_Name"]);
                    model.District = Convert.ToString(dr["Dist"]);
                    model.NameofArea = Convert.ToString(dr["Area"]);
                    string filePdfFileName = DS.Tables[0].Columns.Contains("ApplicationPDF") ? Convert.ToString(dr["ApplicationPDF"]) : "";
                    if (!string.IsNullOrEmpty(filePdfFileName))
                        model.ApplicationPDFName = filePdfFileName.Substring(1, filePdfFileName.Length - 1);
                }

				//if (DS != null && DS.Tables[1].Rows.Count > 0)
				//{
				//	model.ApprovalId = Convert.ToInt32(DS.Tables[1].Rows[0]["StatusID"].ToString());
				//	model.ApprovalName = Convert.ToString(DS.Tables[1].Rows[0]["StatusDesc"].ToString());
				//	model.RejectId = Convert.ToInt32(DS.Tables[1].Rows[1]["StatusID"].ToString());
				//	model.RejectName = Convert.ToString(DS.Tables[1].Rows[1]["StatusDesc"].ToString());
				//	model.IsButtonShow = Convert.ToBoolean(DS.Tables[1].Rows[2]["StatusDesc"].ToString());
				//}

				List<SelectListItem> List = new List<SelectListItem>();
                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    List.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });
                }
                model.ListRejectedReason = List;
				string host = HttpContext.Request.Url.Host.ToLower();
				if (DS.Tables[3] != null && DS.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[3].Rows)
                    {
                        AttachmentsViews view = new AttachmentsViews();
                        view.Action = Convert.ToString(dr["Action"]);
                        view.Users = Convert.ToString(dr["USER"]);
						if (host == "localhost")
						{
							view.Documents = Convert.ToString(dr["Attachment"]).Replace("~", string.Empty);
						}
						else
						{
							view.Documents = Convert.ToString(dr["Attachment"]).Replace("~", string.Empty);
						}
						view.Remarks = Convert.ToString(dr["remarks"]);
                        model.DocumentsViews.Add(view);
                    }
                }
                if (DS.Tables[4] != null && DS.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[4].Rows)
                    {
                        YearsDetailsList view = new YearsDetailsList();
                        view.Current = Convert.ToString(dr["CURRENT"]);
                        view.Prev = Convert.ToString(dr["PREV"]);
                        view.End = Convert.ToString(dr["END"]);
                        view.WorkDesc = Convert.ToString(dr["Workdescription"]);
                        model.GetWorkList.Add(view);
                    }
                }

                if (DS.Tables[5] != null && DS.Tables[5].Rows.Count > 0)
                {
                    ViewBag.CurrentYear = Convert.ToInt32(DS.Tables[5].Rows[0][0]);
                    ViewBag.PrevYear = Convert.ToInt32(DS.Tables[5].Rows[1][0]);
                    ViewBag.EndYear = Convert.ToInt32(DS.Tables[5].Rows[2][0]);
                }

                model.DocumentList = new List<DocumentList>();
                if (DS.Tables.Contains("Table6") && DS.Tables[6].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[6].Rows)
                    {
                        string filepath = string.Empty;
                        DocumentList list = new DocumentList();
                        list.FileName = Convert.ToString(dr["FileName"]);
                        list.FileType = Convert.ToInt32(dr["FileType"]);
                        filepath = Convert.ToString(dr["FilePath"]);
                        list.FilePath = filepath.Substring(1, filepath.Length - 1);
                        model.DocumentList.Add(list);
                    }
                }

                model.UserCommantList = new List<CommandList>();
                if (DS.Tables.Contains("Table7") && DS.Tables[7].Rows.Count > 0)
                {
                    foreach (DataRow dr in DS.Tables[7].Rows)
                    {
                        CommandList list = new CommandList();
                        list.Name = Convert.ToString(dr["Name"]);
                        list.Commants = Convert.ToString(dr["Commnets"]);
                        list.StatusDesc = Convert.ToString(dr["StatusDesc"]);
                        model.UserCommantList.Add(list);
                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("ADActionReviewApprover", model);

        }


        public ActionResult ADActionReviewApprover(AmritaDeviAwardModel obj, HttpPostedFileBase ReviewApprovalDocument, string command, int TechnicalPersonID)
        {
            AmritaDeviInfo repo = new AmritaDeviInfo();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int32 UserID = Convert.ToInt32(Session["UserID"]);

            DataTable dt;

            string IDs = string.Empty;

            try
            {

                string FilePath = "~/AmritaDeviDocument/";
                string Document = "", path = "";

                if (ReviewApprovalDocument != null && ReviewApprovalDocument.ContentLength > 0)
                {
                    Document = Path.GetFileName(ReviewApprovalDocument.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    obj.ReviewApprovalDocument = path;
                    ReviewApprovalDocument.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    obj.ReviewApprovalDocument = "";
                }

                if (TechnicalPersonID > 0)
                {
                    obj.UserId = UserID;
                    obj.AssignTo = UserID;
                    obj.ActionStatus = TechnicalPersonID.ToString();
                    dt = repo.SubmitCommantbyTechandStateLeval(obj);
                }
                else
                {
                    obj.UserId = UserID;
                    obj.AssignTo = UserID;
                    obj.ActionStatus = command;
                    if (obj.RejectedReason != null)
                    { obj.Reason = string.Join(",", obj.RejectedReason); }


                    dt = repo.SubmitADReviewApprover(obj);
                }
                return RedirectToAction("AmritaDeviAwardReviewApprover");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }


        #region Amrita Devi Print PDF



        public string Print(AmritaDeviAwardModel model, YearsDetails[] workDetails, List<clsPermission> GISLIST_PDF, string CategoryName)
        {
            string filepath = string.Empty;
            try
            {
				string host = HttpContext.Request.Url.Host.ToLower();
				if (host == "localhost")
				{
					filepath = "~/AmritaDeviDocument/ApplicantPDF/AD(" + model.RequestID + ")" + DateTime.Now.Ticks.ToString() + ".pdf";
				}
				else
				{
					filepath = "~/AmritaDeviDocument/ApplicantPDF/AD(" + model.RequestID + ")" + DateTime.Now.Ticks.ToString() + ".pdf";
				}
                Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

                PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));

                var FontColour = new BaseColor(0, 0, 0);
                Paragraph tableheading = null;
                Paragraph sideheading = null;
                Phrase colHeading;

                PdfPCell cell;
                PdfPTable pdfTable = null;

                //BaseFont dev = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\mfdev016.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
				BaseFont dev = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\Kruti_Dev.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
				iTextSharp.text.Font Myhindi = new iTextSharp.text.Font(dev, 16);
                iTextSharp.text.Font hindiHead = new iTextSharp.text.Font(dev, 14);
                iTextSharp.text.Font hindiTitle = new iTextSharp.text.Font(dev, 12);
                iTextSharp.text.Font hindiSubFont = new iTextSharp.text.Font(dev, 10);

                BaseFont dev1 = BaseFont.CreateFont(Environment.GetEnvironmentVariable("windir") + @"\fonts\ARIALUNI.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font MyEnglish = new iTextSharp.text.Font(dev1, 14);
                iTextSharp.text.Font Myfont = new iTextSharp.text.Font(dev1, 14);
                iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(dev1, 12);
                iTextSharp.text.Font subheadfont = new iTextSharp.text.Font(dev1, 8);

                //var Myfont = FontFactory.GetFont("Arial", 12, FontColour);
                //var fontTitle = FontFactory.GetFont("Arial", 10, FontColour);
                //var subheadfont = FontFactory.GetFont("Arial", 8, FontColour);



                doc.Open();
                doc.NewPage();

                // doc.Add(new Paragraph(Environment.NewLine));


                PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cells = new PdfPCell() { Border = 4 };
                Details.TotalWidth = 120;
                Details.SetTotalWidth(new float[] { 35f, 50f, 35f });


                cells = new PdfPCell(new Phrase("jktLFkku ljdkj", Myhindi)) { Border = 0 };
                cells.Colspan = 3;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cells);

                cells = new PdfPCell(new Phrase("ou foHkkx", Myhindi)) { Border = 0 };
                cells.Colspan = 3;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cells);

                cells = new PdfPCell(new Phrase("ve`rk nsoh fo’uksbZ Le`fr iqjLdkj", Myhindi)) { Border = 0 };
                cells.Colspan = 3;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cells);

                cells = new PdfPCell(new Phrase("vkosnu & i=", Myhindi)) { Border = 0 };
                cells.Colspan = 3;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cells);

                doc.Add(new Phrase(Environment.NewLine));

                cells = new PdfPCell(new Phrase(" ", hindiSubFont)) { Border = 0 };
                cells.Colspan = 3;
                cells.HorizontalAlignment = Element.ALIGN_CENTER;
                Details.AddCell(cells);
                doc.Add(Details);



                PdfPTable LocationDetails = new PdfPTable(7) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellvdetails = new PdfPCell() { Border = 4 };
                LocationDetails.TotalWidth = 254;
                LocationDetails.SetTotalWidth(new float[] { 35f, 35f, 35f, 35f, 35f, 35f, 35f });

                cellvdetails = new PdfPCell(new Phrase("iath;u ukekad" + " & " + model.RequestID, hindiHead)) { Border = 0 };
                cellvdetails.Colspan = 5;
                cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                LocationDetails.AddCell(cellvdetails);


                cellvdetails = new PdfPCell(new Phrase("fnukad" + " - " + DateTime.Now.ToString("dd@MM@yyyy"), hindiHead)) { Border = 0 };
                cellvdetails.Colspan = 2;
                cellvdetails.HorizontalAlignment = Element.ALIGN_RIGHT;
                LocationDetails.AddCell(cellvdetails);


                cellvdetails = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
                cellvdetails.Colspan = 7;
                cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                LocationDetails.AddCell(cellvdetails);

                cellvdetails = new PdfPCell(new Phrase("Referred By :-", fontTitle));
                cellvdetails.Colspan = 2;
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                LocationDetails.AddCell(cellvdetails);

                cellvdetails = new PdfPCell(new Phrase(model.ReferedBy, fontTitle));
                cellvdetails.Colspan = 5;
                cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);


                cellvdetails = new PdfPCell(new Phrase("Location Details", Myfont)) { Border = 0 };
                cellvdetails.Colspan = 7;
                cellvdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                LocationDetails.AddCell(cellvdetails);

                cellvdetails = new PdfPCell(new Phrase("Division", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);
                cellvdetails = new PdfPCell(new Phrase("District", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);
                cellvdetails = new PdfPCell(new Phrase("Tehsil", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);
                cellvdetails = new PdfPCell(new Phrase("Panchyat samiti", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);
                cellvdetails = new PdfPCell(new Phrase("Gram Panchayat", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);
                cellvdetails = new PdfPCell(new Phrase("Village", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);
                cellvdetails = new PdfPCell(new Phrase("Name of Area", fontTitle));
                cellvdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                LocationDetails.AddCell(cellvdetails);

                foreach (var itm in GISLIST_PDF)
                {
                    cellvdetails = new PdfPCell(new Phrase(itm.Div_NM, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                    cellvdetails = new PdfPCell(new Phrase(itm.Dist_NM, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                    cellvdetails = new PdfPCell(new Phrase(itm.Tehsil_NM, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                    cellvdetails = new PdfPCell(new Phrase(itm.Block_NM, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                    cellvdetails = new PdfPCell(new Phrase(itm.Gp_NM, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                    cellvdetails = new PdfPCell(new Phrase(itm.Village_NM, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                    cellvdetails = new PdfPCell(new Phrase(itm.areaName, subheadfont));
                    LocationDetails.AddCell(cellvdetails);
                }
                doc.Add(LocationDetails);


                PdfPTable GPSDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellGPSdetails = new PdfPCell() { Border = 4 };
                GPSDetails.TotalWidth = 100;
                GPSDetails.SetTotalWidth(new float[] { 50f, 50f });

                cellGPSdetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                cellGPSdetails.Colspan = 2;
                GPSDetails.AddCell(cellGPSdetails);

                cellGPSdetails = new PdfPCell(new Phrase("GPS Details", Myfont)) { Border = 0 };
                cellGPSdetails.Colspan = 2;
                cellGPSdetails.HorizontalAlignment = Element.ALIGN_LEFT;
                GPSDetails.AddCell(cellGPSdetails);


                cellGPSdetails = new PdfPCell(new Phrase("Latitude", fontTitle));
                cellGPSdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                GPSDetails.AddCell(cellGPSdetails);
                cellGPSdetails = new PdfPCell(new Phrase("Longitude", fontTitle));
                cellGPSdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                GPSDetails.AddCell(cellGPSdetails);
                cellGPSdetails = new PdfPCell(new Phrase(model.GISInformationList[0].GPSLat, subheadfont));
                GPSDetails.AddCell(cellGPSdetails);
                cellGPSdetails = new PdfPCell(new Phrase(model.GISInformationList[0].GPSLong, subheadfont));
                GPSDetails.AddCell(cellGPSdetails);

                cellGPSdetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                cellGPSdetails.Colspan = 2;
                GPSDetails.AddCell(cellGPSdetails);

                doc.Add(GPSDetails);



                PdfPTable PersonalDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellPdetails = new PdfPCell() { Border = 4 };

                PersonalDetails.TotalWidth = 100;
                PersonalDetails.SetTotalWidth(new float[] { 50f, 50f });
                cellPdetails = new PdfPCell(new Phrase(@"vkosnu o""kZ", hindiTitle));
                cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase("vkosnd dk izdkj", hindiTitle));
                cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase(model.CurrentYear.ToString(), subheadfont));
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase(model.IsOrgOrPerson == false ? "O;fDr" : "laxBu", hindiSubFont));
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase("¼d½ ukfer O;fDr@laLFkk dk uke", hindiTitle));
                cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase("¼[k½ ukfer O;fDr@laLFkk uke", hindiTitle));
                cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase(model.FirstName1, subheadfont));
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase(model.FirstName2, subheadfont));
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase("izk;kstd dk uke ,oa irk", hindiTitle));
                cellPdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                cellPdetails.Colspan = 2;
                PersonalDetails.AddCell(cellPdetails);
                cellPdetails = new PdfPCell(new Phrase(model.Address, subheadfont));
                cellPdetails.Colspan = 2;
                PersonalDetails.AddCell(cellPdetails);
                doc.Add(PersonalDetails);

                PdfPTable AwardDetails = new PdfPTable(2) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellAwarddetails = new PdfPCell() { Border = 4 };
                AwardDetails.TotalWidth = 100;
                AwardDetails.SetTotalWidth(new float[] { 50f, 50f });


                cellAwarddetails = new PdfPCell(new Phrase("iq:Ldkj dh Js.kh ftlds fy, vkosnu fd;k x;k gS ", hindiTitle));
                cellAwarddetails.Colspan = 2;
                cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                AwardDetails.AddCell(cellAwarddetails);
                cellAwarddetails = new PdfPCell(new Phrase(CategoryName, subheadfont));
                cellAwarddetails.Colspan = 2;
                AwardDetails.AddCell(cellAwarddetails);
                cellAwarddetails = new PdfPCell(new Phrase("dk;Z LFky dk uke ", hindiTitle));
                cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                cellAwarddetails.Colspan = 2;
                AwardDetails.AddCell(cellAwarddetails);
                cellAwarddetails = new PdfPCell(new Phrase(model.WorkStationName, subheadfont));
                cellAwarddetails.Colspan = 2;
                AwardDetails.AddCell(cellAwarddetails);

                cellAwarddetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                cellAwarddetails.Colspan = 2;
                AwardDetails.AddCell(cellAwarddetails);

                cellAwarddetails = new PdfPCell(new Phrase(@"O;fDr@laLFkk }kjk ftl Hkwfe ij mRd`""V dk;Z fd;k gS  mldh fdLe dks crkrs gq, okLrfod {ks=] LFkku rFkk ekfydkuk i}fr%&", hindiTitle)) { Border = 1 };
                cellAwarddetails.Colspan = 2;
                AwardDetails.AddCell(cellAwarddetails);


                cellAwarddetails = new PdfPCell(new Phrase("LFkku", hindiTitle));
                cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                AwardDetails.AddCell(cellAwarddetails);
                cellAwarddetails = new PdfPCell(new Phrase("dqy okLrfod {ks= ¼gSDVs;j esa½", hindiTitle));
                cellAwarddetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                AwardDetails.AddCell(cellAwarddetails);
                cellAwarddetails = new PdfPCell(new Phrase(model.LandPlace, subheadfont));
                AwardDetails.AddCell(cellAwarddetails);
                cellAwarddetails = new PdfPCell(new Phrase(model.Landhacktor, subheadfont));
                AwardDetails.AddCell(cellAwarddetails);


                cellAwarddetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                cellAwarddetails.Colspan = 2;
                AwardDetails.AddCell(cellAwarddetails);

                doc.Add(AwardDetails);


                PdfPTable AreaDetails = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellAreadetails = new PdfPCell() { Border = 4 };
                AreaDetails.TotalWidth = 120;
                AreaDetails.SetTotalWidth(new float[] { 35f, 50f, 50f });

                cellAreadetails = new PdfPCell(new Phrase("LokfeRo", hindiTitle));
                cellAreadetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase("futh Hkwfe", hindiTitle));
                cellAreadetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                cellAreadetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                AreaDetails.AddCell(cellAreadetails);

                cellAreadetails = new PdfPCell(new Phrase("futh Hkwfe", hindiTitle));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.PersonalLandHactor, subheadfont));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.PersonalLandHactorDesc, subheadfont));
                AreaDetails.AddCell(cellAreadetails);

                cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.CollectiveLandHactor, subheadfont));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.CollectiveLandDesc, subheadfont));
                AreaDetails.AddCell(cellAreadetails);

                cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.RevenueLandHactor, subheadfont));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.RevenueLandDesc, subheadfont));
                AreaDetails.AddCell(cellAreadetails);

                cellAreadetails = new PdfPCell(new Phrase("{ks= dk fooj.k", hindiTitle));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.ForestLandHactor, subheadfont));
                AreaDetails.AddCell(cellAreadetails);
                cellAreadetails = new PdfPCell(new Phrase(model.ForestLandDesc, subheadfont));
                AreaDetails.AddCell(cellAreadetails);

                cellAreadetails = new PdfPCell(new Phrase("  ", hindiSubFont)) { Border = 0 };
                cellAreadetails.Colspan = 3;
                AreaDetails.AddCell(cellAreadetails);

                cellAreadetails = new PdfPCell(new Phrase(@"vFkkZr pV~Vkuh] nynyh] cqybZ feV~Vh] yo.kh;@{kkjh;] igkMh ,oa chgM+ {ks= vU; dksbZ fo’ks""k fdLe", hindiTitle)) { Border = 0 };
                cellAreadetails.Colspan = 3;
                AreaDetails.AddCell(cellAreadetails);
                doc.Add(AreaDetails);


                PdfPTable WorkDetails = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
                PdfPCell cellWorkdetails = new PdfPCell() { Border = 4 };
                WorkDetails.TotalWidth = 120;
                WorkDetails.SetTotalWidth(new float[] { 35f, 30f, 30f, 30f });

                cellWorkdetails = new PdfPCell(new Phrase("Details", subheadfont));
                cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                WorkDetails.AddCell(cellWorkdetails);

                cellWorkdetails = new PdfPCell(new Phrase(Convert.ToString(model.CurrentYear - 1), subheadfont));
                cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                WorkDetails.AddCell(cellWorkdetails);

                cellWorkdetails = new PdfPCell(new Phrase(Convert.ToString(model.CurrentYear - 2), subheadfont));
                cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                WorkDetails.AddCell(cellWorkdetails);

                cellWorkdetails = new PdfPCell(new Phrase(Convert.ToString(model.CurrentYear - 3), subheadfont));
                cellWorkdetails.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
                WorkDetails.AddCell(cellWorkdetails);

                foreach (var itm in workDetails)
                {
                    cellWorkdetails = new PdfPCell(new Phrase(itm.WorkDesc, subheadfont));
                    WorkDetails.AddCell(cellWorkdetails);

                    cellWorkdetails = new PdfPCell(new Phrase(itm.Current, subheadfont));
                    WorkDetails.AddCell(cellWorkdetails);

                    cellWorkdetails = new PdfPCell(new Phrase(itm.Prev, subheadfont));
                    WorkDetails.AddCell(cellWorkdetails);

                    cellWorkdetails = new PdfPCell(new Phrase(itm.End, subheadfont));
                    WorkDetails.AddCell(cellWorkdetails);
                }

                doc.Add(WorkDetails);

                doc.Close();
                //if (System.IO.File.Exists(Server.MapPath(filepath)))
                //{
                //    string FilePath = Server.MapPath(filepath);
                //    WebClient User = new WebClient();
                //    Byte[] FileBuffer = User.DownloadData(FilePath);
                //    if (FileBuffer != null)
                //    {
                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                //        Response.BinaryWrite(FileBuffer);
                //        Response.End();
                //    }
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AmritaDeviPDFonSubmit", 0, DateTime.Now, model.UserId);
                return null;
            }
            return filepath;
        }
        #endregion
    }
}
