using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.Admin;
using System.Collections;
using System.Data;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using FMDSS.Models.CitizenService.PermissionService;
using System.Xml;
using System.Xml.Linq;
using FMDSS.Filters;
using System.Text;
using System.Diagnostics;

namespace FMDSS.Controllers.CitizenService.FixedPermission
{
    [MyAuthorization]
    public class FixedPermissionController : Controller
    {
        //
        // GET: /FixedPermission/
        FixedLandUsage _objmodel = new FixedLandUsage();
        SMS_EMail_Services _objMailSMS = new SMS_EMail_Services();
        List<SelectListItem> Division = new List<SelectListItem>();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        int ModuleID = 1;
        Int64 UserID = 0;
        double discount = 0;
        double Amount = 0;
        double Tax = 0;
        double FinalAmount = 0;
        double DiscountedAmmount = 0;
        double TaxableAmount = 0;
        Location _obj = new Location();
        DataSet dsName = new DataSet();
        public ActionResult FixedPermission(string aid, string messagetype)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

                    if (aid == null || aid == "")
                    {
                        aid = "1";

                    }
                    //Session["DivisionData"] = "";
                    _objmodel.PermissionType = _objmodel.GetPermissionTypes(Convert.ToInt32(aid)).Tables[0].Rows[0]["Name"].ToString();
                    Session["PermissionType"] = "";
                    Session["PermissionType"] = _objmodel.PermissionType;
                    DataSet Payds = new DataSet();
                    if (messagetype == "1")
                    {
                        ViewData["Message"] = "Error occur while saving records!";
                    }

                    List<FixedLandUsage> FixedLandList = new List<FixedLandUsage>();
                    DataTable dt = null;
                    dt = _objmodel.Division();

                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        Division.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                    }
                    //if (aid == "4") { dt = _objmodel.MiningDistrict(); }
                    //else { dt = _obj.District(); }

                    //ViewBag.fname = dt;
                    //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    //{
                    //    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    //}
                    //DataTable dt = _obj.District();
                    ////ViewBag.fname = dt.Select("Mining_Available=1");


                    //ViewBag.fname = dt;
                    //DataRow[] result;
                    //if (aid == "4")
                    //{
                    //    result = dt.Select("Mining_Available=1");

                    //}
                    //else { result = dt.Select("Mining_Available=1 OR Mining_Available=0");  }
                    //foreach (System.Data.DataRow dr in result)
                    //{
                    //    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    //}
                    ViewBag.ddlDivision1 = Division;
                    ViewBag.ddlDistrict1 = District;
                    ViewBag.ddlBlockName1 = BlockName;
                    ViewBag.ddlGPName1 = GPName;
                    ViewBag.ddlVillName1 = VillageName;
                    ViewBag.ApplicantType = new SelectList(Common.GetApplicantType(), "Value", "Text");
                    ViewBag.NearestWaterSource = new SelectList(Common.GetNearestWaterSource(), "Value", "Text");
                    ViewBag.Sawmill_Type = new SelectList(Common.GetSawmillType(), "Value", "Text");
                    ViewBag.Industrial_Type = new SelectList(Common.GetIndustrialType(), "Value", "Text");
                    _objmodel.ConditionRevenueMapSigned = "Both";
                    Payds = _objmodel.GetFinalAmount(1, Convert.ToInt32(aid));

                    if (Payds.Tables[0].Rows.Count > 0)
                    {
                        Amount = Convert.ToDouble(Payds.Tables[0].Rows[0]["Amount"]);
                        discount = Convert.ToDouble(Payds.Tables[0].Rows[0]["Discount"]);
                        Tax = Convert.ToDouble(Payds.Tables[0].Rows[0]["Tax"]);
                        DiscountedAmmount = (Amount * discount) / 100.0;
                        TaxableAmount = (Amount * Tax) / 100.0;
                        FinalAmount = (Amount + TaxableAmount) - DiscountedAmmount;
                        _objmodel.Amount = Convert.ToDecimal(Amount);
                        _objmodel.Discount = Convert.ToDecimal(discount);
                        _objmodel.Tax = Convert.ToDecimal(Tax);
                        _objmodel.Final_Amount = Convert.ToDecimal(FinalAmount);
                    }
                }

                _objmodel.PermissionId = Convert.ToInt32(aid);
                return View(_objmodel);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult GetDistrictName(string Division)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    List<SelectListItem> items = new List<SelectListItem>();

                    DataTable dt = _objmodel.BindDistrict(Division);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }

                    ViewBag.ddlDistrict1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

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

                    DataTable dt = _obj.BindBlockName(District);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }

                    ViewBag.ddlBlockName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpPost]
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
                    DataTable dt = _obj.BindGramPanchayatName(District, BlockName);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                    ViewBag.ddlGPName1 = new SelectList(items, "Value", "Text");

                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
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
                    DataTable dt = _obj.BindVillageName(District, BlockName, GPName);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                    ViewBag.ddlVillName1 = new SelectList(items, "Value", "Text");
                    return Json(new SelectList(items, "Value", "Text"));
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;

        }

        [HttpPost]



        public ActionResult FixedLandPermission(FormCollection FCollection, FixedLandUsage Model, string command, HttpPostedFileBase uploadFile)
        {
            var path = "";
            string FileName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

                    if (command == "")
                    {


                        //var file1 = Request.Files[0];

                        _objmodel.UserID = Convert.ToInt32(Session["UserId"]);

                        if (FCollection["hdMenuId"].ToString() == "")
                        {
                            _objmodel.PermissionId = 0;
                        }
                        else
                        {
                            _objmodel.PermissionId = Convert.ToInt32(FCollection["hdMenuId"]);
                        }
                        if (FCollection["Applicant_type"].ToString() == "")
                        {
                            _objmodel.ApplicantType = 0;
                        }
                        else
                        {
                            _objmodel.ApplicantType = Convert.ToInt32(FCollection["Applicant_type"]);
                        }

                        //if (FCollection["ddlDistrict"].ToString() == "")
                        //{
                        //    _objmodel.DIST_CODE = "";
                        //}
                        //else
                        //{
                        if(!string.IsNullOrEmpty(Session["DivisionData"].ToString()))
                        {
                        List<clsPermission> list = (List<clsPermission>)Session["DivisionData"];
                        _objmodel.DIV_CODE = list.FirstOrDefault().DIV_CODE;
                        }
                       
                             
                        //}
                        //if (FCollection["ddlBlockName"].ToString() == "")
                        //{
                        //    _objmodel.BLK_CODE = "";
                        //}
                        //else
                        //{
                        //    _objmodel.BLK_CODE = FCollection["ddlBlockName"].ToString();
                        //}
                        //if (FCollection["ddlGPName"].ToString() == "")
                        //{
                        //    _objmodel.GP_CODE = "";
                        //}
                        //else
                        //{
                        //    _objmodel.GP_CODE = FCollection["ddlGPName"].ToString();
                        //}
                        //if (FCollection["ddlVillName"].ToString() == "")
                        //{
                        //    _objmodel.VILL_CODE = "";
                        //}
                        //else
                        //{
                        //    _objmodel.VILL_CODE = FCollection["ddlVillName"].ToString();
                        //}

                        if (Model.Area_Size == null || Model.Area_Size.ToString() == "")
                        {
                            _objmodel.Area_Size = "";
                        }
                        else
                        {
                            _objmodel.Area_Size = Model.Area_Size.ToString();
                        }
                        if (Model.GPSLat == null || Model.GPSLat.ToString() == "")
                        {
                            _objmodel.GPSLat = "";
                        }
                        else
                        {
                            _objmodel.GPSLat = Model.GPSLat.ToString();
                        }
                        if (Model.GPSLong == null || Model.GPSLong.ToString() == "")
                        {
                            _objmodel.GPSLat = "";
                        }
                        else
                        {
                            _objmodel.GPSLong = Model.GPSLong.ToString();
                        }
                        if (FCollection["Industrial_Type"].ToString() == "")
                        {
                            _objmodel.Industrial_Type = "";
                        }
                        else
                        {
                            _objmodel.Industrial_Type = FCollection["Industrial_Type"].ToString();
                        }
                        if (FCollection["Revenue_Map_Signed"] == "")
                        {
                            _objmodel.Revenue_Map_Signed = "";
                        }
                        else
                        {
                            _objmodel.Revenue_Map_Signed = FCollection["Revenue_Map_Signed"].ToString();
                        }
                        
                            _objmodel.IsGTSheetAvailable = "1";
                        
                        if (Model.Duration_From == null || Model.Duration_From.ToString() == "")
                        {
                            _objmodel.Duration_From = "";
                        }
                        else
                        {
                            _objmodel.Duration_From = Model.Duration_From.ToString();//DateTime.ParseExact(Model.Duration_From.ToString(),"dd/MM/yyyy",null);
                        }
                        if (Model.Duration_To == null || Model.Duration_To.ToString() == "")
                        {
                            _objmodel.Duration_To = "";
                        }
                        else
                        {
                            _objmodel.Duration_To = Model.Duration_To.ToString();//DateTime.ParseExact(Model.Duration_To.ToString(), "dd/MM/yyyy", null); 
                        }

                        if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[0].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.KML_Path = path;
                            Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.KML_Path = ""; }

                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Record_Path = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Revenue_Record_Path = ""; }

                        if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[2].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Map_Path = path;
                            Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Revenue_Map_Path = ""; }

                        if (Request.Files[3] != null && Request.Files[3].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[3].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Additional_Document = path;
                            Request.Files[3].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Additional_Document = ""; }

                        if (Model.Citizen_Comment == null || Model.Citizen_Comment.ToString() == "")
                        {
                            _objmodel.Citizen_Comment = "";
                        }
                        else
                        {
                            _objmodel.Citizen_Comment = Model.Citizen_Comment.ToString();
                        }

                        #region MiningPermission


                        if (FCollection["Nearest_WaterSource"].ToString() == "")
                        {
                            _objmodel.Nearest_WaterSource = "";
                        }
                        else
                        {
                            _objmodel.Nearest_WaterSource = FCollection["Nearest_WaterSource"].ToString();
                        }
                        if (Model.WaterSource_Distance == null || Model.WaterSource_Distance.ToString() == "")
                        {
                            _objmodel.WaterSource_Distance = "";
                        }
                        else
                        {
                            _objmodel.WaterSource_Distance = Model.WaterSource_Distance.ToString();
                        }
                        if (Model.Forest_Distance == null || Model.Forest_Distance.ToString() == "")
                        {
                            _objmodel.Forest_Distance = "";
                        }
                        else
                        {
                            _objmodel.Forest_Distance = Model.Forest_Distance.ToString();
                        }
                        if (Model.Wildlife_Distance == null || Model.Wildlife_Distance.ToString() == "")
                        {
                            _objmodel.Wildlife_Distance = "";
                        }
                        else
                        {
                            _objmodel.Wildlife_Distance = Model.Wildlife_Distance.ToString();
                        }
                        if (Model.Tree_species == null || Model.Tree_species.ToString() == "")
                        {
                            _objmodel.Tree_species = "";
                        }
                        else
                        {
                            _objmodel.Tree_species = Model.Tree_species.ToString();
                        }

                        if (Model.AravalliHills == null || Model.AravalliHills.ToString() == "")
                        {
                            _objmodel.AravalliHills = "";
                        }
                        else
                        {
                            _objmodel.AravalliHills = Model.AravalliHills.ToString();
                        }
                        if (Model.ForestLand == null || Model.ForestLand.ToString() == "")
                        {
                            _objmodel.ForestLand = "";
                        }
                        else
                        {
                            _objmodel.ForestLand = Model.ForestLand.ToString();
                        }
                        if (Model.Plantation_Area == null || Model.Plantation_Area.ToString() == "")
                        {
                            _objmodel.Plantation_Area = "";
                        }
                        else
                        {
                            _objmodel.Plantation_Area = Model.Plantation_Area.ToString();
                        }

                        #endregion

                        #region "Sawmills"

                        if (FCollection["Sawmill_Type"].ToString() == "")
                        {
                            _objmodel.Sawmill_Type = "";
                        }
                        else
                        {
                            _objmodel.Sawmill_Type = FCollection["Sawmill_Type"].ToString();
                        }

                        if (Model.Sawmill_Size == null || Model.Sawmill_Size.ToString() == "")
                        {
                            _objmodel.Sawmill_Size = "";
                        }
                        else
                        {
                            _objmodel.Sawmill_Size = Model.Sawmill_Size.ToString();
                        }

                        #endregion
                        if (Model.OtherPermission == null || Model.OtherPermission.ToString() == "")
                        {
                            _objmodel.OtherPermission = "";
                        }
                        else
                        {
                            _objmodel.OtherPermission = Model.OtherPermission.ToString();
                        }
                        if (Model.Amount == null || Model.Amount.ToString() == "")
                        {
                            _objmodel.Amount = 0;
                        }
                        else
                        {
                            _objmodel.Amount = Convert.ToDecimal(Model.Amount);
                        }
                        if (Model.Discount == null || Model.Discount.ToString() == "")
                        {
                            _objmodel.Discount = 0;
                        }
                        else
                        {
                            _objmodel.Discount = Convert.ToDecimal(Model.Discount);
                        }
                        if (Model.Tax == null || Model.Tax.ToString() == "")
                        {
                            _objmodel.Tax = 0;
                        }
                        else
                        {
                            _objmodel.Tax = Convert.ToDecimal(Model.Tax);
                        }
                        if (Model != null)
                        {
                            _objmodel.Final_Amount = Model.Final_Amount;
                            Session["Ftotalprice"] = _objmodel.Final_Amount.ToString();

                        }
                        _objmodel.TransactionId = DateTime.Now.Ticks.ToString();
                        Session["FRequestId"] = _objmodel.TransactionId.ToString();
                        _objmodel.PayStatus = "Pending";
                        Int64 id = _objmodel.SubmitFixedLandUsage(_objmodel);
                        if (id > 0)
                        {
                            List<clsPermission> list = (List<clsPermission>)Session["DivisionData"];
                            if (list != null)
                            {
                                list.ForEach(t => t.RequestedID = Convert.ToString(id));
                                _objmodel.SaveDistrictMapping(list);
                                Session["DivisionData"] = null;
                            }
                            if (_objmodel.Amount > 0)
                            {
                                return View("FixedPermissionPayment", _objmodel);
                            }
                            else
                            {
                                DataSet ds = new DataSet();

                                ds = _objmodel.UpdatePaymentStatus(0, 1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));

                                #region "User Send Email"
                                string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                                string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                                _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                                SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                                #endregion
                                return RedirectToAction("dashboard", "dashboard", new { messagetype = "2" });
                            }

                        }
                        else
                        {
                            return View("Error");
                        }

                    }
                    else
                    {

                        
                        if (Request.Files[0] != null && Request.Files[0].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[0].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.KML_Path = path;
                            Request.Files[0].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.KML_Path = FCollection["hdKMLFile"].ToString(); }

                        if (Request.Files[1] != null && Request.Files[1].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[1].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Record_Path = path;
                            Request.Files[1].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Revenue_Record_Path = FCollection["hdRevRecPath"].ToString(); }

                        if (Request.Files[2] != null && Request.Files[2].ContentLength > 0)
                        {

                            FileName = Path.GetFileName(Request.Files[2].FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + FileName;
                            path = Path.Combine(FilePath, FileFullName);
                            _objmodel.Revenue_Map_Path = path;
                            Request.Files[2].SaveAs(Server.MapPath(FilePath + FileFullName));

                        }
                        else
                        { _objmodel.Revenue_Map_Path = FCollection["hdRevMapPath"].ToString(); }


                        _objmodel.UserID = Convert.ToInt32(Session["UserId"]);
                        _objmodel.RequestedID = command;


                        _objmodel.UpdateData(_objmodel, "reassigned");

                        return RedirectToAction("dashboard", "dashboard", new { messagetype = "3" });

                    }
                }
            }


            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return null;
        }

        #region "Pay"

        [HttpPost]
        public void Pay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();

                    //EM33172142@5488
                    Payment pay = new Payment();
                    string encrypt = pay.RequestString("EM33172142@5488", Session["FRequestId"].ToString(), Session["Ftotalprice"].ToString(), ReturnUrl + "FixedPermission/ResponsePay", Session["User"].ToString(), "", "");
                    Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }
        #endregion

        public ActionResult ResponsePay()
        {

            Payment pay = new Payment();
            int status1 = 0;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());

                    DataTable dt = new DataTable();
                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion
                    string response = Request.QueryString["trnParams"].ToString();

                    #region Response decryption
                    string ResponseResult = pay.ProcesTranscationresponce(response);
                    string str1, str2;
                    str1 = ResponseResult.Replace("<RESPONSE ", "");
                    str2 = str1.Replace("></RESPONSE>", "");
                    string[] Responsearr = str2.Split(' ');
                    string checkFail = "STATUS='FAILED'";
                    string checkSucess = "STATUS='SUCCESS'";
                    string rowstatus1 = "";
                    foreach (var item in Responsearr)
                    {
                        if (item.Equals(checkFail))
                        {
                            string[] status2 = item.Split('=');
                            rowstatus1 = status2[1].ToString();
                        }
                        if (item.Equals(checkSucess))
                        {
                            string[] status2 = item.Split('=');
                            rowstatus1 = status2[1].ToString();
                        }
                    }
                    int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
                    string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 2);
                    #endregion

                    #region Response Status
                    if (finalstatus1 == "FAILED")
                    {
                        string[] emitratransid = Responsearr[0].Split('=');
                        string[] transtime = Responsearr[1].Split('=');
                        string[] reqid = Responsearr[2].Split('=');
                        string[] reqamt = Responsearr[3].Split('=');
                        string[] username = Responsearr[4].Split('=');
                        string[] status = Responsearr[7].Split('=');


                        DataRow dtrow = dt.NewRow();
                        string rowstatus = status[1].ToString();
                        int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                        string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                        string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                        string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                        string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                        string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                        _objmodel.TransactionId = "0";
                        string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                        string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                        string rawusername = username[1].ToString();
                        int usernamelen = Convert.ToInt32(rawusername.Length);
                        string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);

                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";//transtime[1];
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUserName;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            _objmodel.Trn_Status_Code = 0;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (finalstatus1 == "SUCCESS")
                    {
                        string[] emitratransid = Responsearr[0].Split('=');
                        string[] transtime = Responsearr[1].Split('=');
                        string[] reqid = Responsearr[3].Split('=');
                        string[] reqamt = Responsearr[4].Split('=');
                        string[] username = Responsearr[5].Split('=');
                        string[] status = Responsearr[8].Split('=');
                        string[] bank = Responsearr[9].Split('=');
                        string[] bankbidno = Responsearr[13].Split('=');

                        DataRow dtrow = dt.NewRow();
                        string rowstatus = status[1].ToString();
                        int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
                        string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
                        string rowreqid = reqid[1].ToString();
                        int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
                        string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
                        string rawemitra = emitratransid[1].ToString();
                        int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
                        string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
                        _objmodel.TransactionId = finalreqid;
                        string rawtransamount = reqamt[1].ToString();
                        int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
                        string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
                        string rawusername = username[1].ToString();
                        int usernamelen = Convert.ToInt32(rawusername.Length);
                        string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);
                        string rawbank = bank[1].ToString();
                        int banklen = Convert.ToInt32(rawbank.Length);
                        string finalbank = rawbank.ToString().Substring(1, banklen - 2);
                        //string rawbankbid = bankbidno[1].ToString();
                        //int bankidlen = Convert.ToInt32(rawbankbid.Length);
                        //string finalbankid = rawbankbid.ToString().Substring(1, bankidlen - 2);
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
                        dtrow["REQUEST ID"] = finalreqid;
                        dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
                        _objmodel.TransactionId = finalemitraid;
                        dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                        dtrow["TRANSACTION AMOUNT"] = finalamount;
                        dtrow["USER NAME"] = finalUserName;
                        dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                        //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            _objmodel.Trn_Status_Code = 1;
                            status1 = 1;
                        }
                        dt.Rows.Add(dtrow);
                    }
                    #endregion
                    ViewData.Model = dt.AsEnumerable();


                    _objmodel.UserName = Session["User"].ToString();

                    if (_objmodel.Trn_Status_Code == 1)
                    {
                        DataSet ds = new DataSet();

                        ds = _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), _objmodel.Trn_Status_Code, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));

                        #region "User Send Email"
                        string UserMailBody = Common.GenerateBody(_objmodel.Final_Amount, ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                        string UserSmsBody = Common.GenerateSMSBody(ds.Tables[0].Rows[0]["Name"].ToString(), _objmodel.TransactionId, Session["PermissionType"].ToString() + " Permission");
                        _objMailSMS.sendEMail("Payment Details for " + Session["PermissionType"].ToString() + " Permission", UserMailBody, ds.Tables[0].Rows[0]["EmailId"].ToString(), string.Empty);

                        SMS_EMail_Services.sendSingleSMS(ds.Tables[0].Rows[0]["Mobile"].ToString(), UserSmsBody);
                        #endregion
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            #region "Is Reviwer Mail"
                            string ReviwerMailBody = Common.GenerateReviwerBody(ds.Tables[1].Rows[0]["Name"].ToString(), _objmodel.TransactionId, ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission");
                            _objMailSMS.sendEMail("Request for " + ds.Tables[1].Rows[0]["SubPermissionDesc"].ToString() + " Permission", ReviwerMailBody, ds.Tables[1].Rows[0]["EmailId"].ToString(), string.Empty);
                            #endregion
                        }
                        return View("TransactionStatus");
                    }
                    else
                    {
                        _objmodel.UpdatePaymentStatus(Convert.ToInt64(_objmodel.TransactionId), status1, "payUpdate", Session["FRequestId"].ToString(), Convert.ToInt64(Session["UserId"].ToString()));
                        return View("TransactionStatus");

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;


        }

        public ActionResult UpdateFixedPermission(string id)
        {
            ViewData["disablecontrols"] = true;
            DataSet _objds = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    _objds = _objmodel.GetFixedLandValues(id);

                    if (_objds.Tables[0] != null)
                    {

                        _objmodel.RequestedID = id;
                        _objmodel.PermissionId = Convert.ToInt32(_objds.Tables[0].Rows[0]["P_ID"].ToString());

                        //List<SelectListItem> items = new List<SelectListItem>();
                        //DataTable dt = _obj.District();
                        //ViewBag.fname = dt;

                        //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                        //{
                        //    items.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });

                        //}
                        //ViewBag.ddlDistrict1 = new SelectList(items, "Value", "Text", _objds.Tables[0].Rows[0]["DIST_CODE"].ToString());

                        //dt = _obj.BindBlockName(_objds.Tables[0].Rows[0]["DIST_CODE"].ToString());
                        //ViewBag.fname1 = dt;
                        //foreach (System.Data.DataRow dr in ViewBag.fname1.Rows)
                        //{
                        //    items.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                        //}

                        //ViewBag.ddlBlockName1 = new SelectList(items, "Value", "Text", _objds.Tables[0].Rows[0]["BLK_CODE"].ToString());

                        //dt = _obj.BindGramPanchayatName(_objds.Tables[0].Rows[0]["DIST_CODE"].ToString(), _objds.Tables[0].Rows[0]["BLK_CODE"].ToString());
                        //ViewBag.fname2 = dt;
                        //foreach (System.Data.DataRow dr in ViewBag.fname2.Rows)
                        //{
                        //    items.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                        //}
                        //ViewBag.ddlGPName = new SelectList(items, "Value", "Text", _objds.Tables[0].Rows[0]["GP_CODE"].ToString());

                        //dt = _obj.BindVillageName(_objds.Tables[0].Rows[0]["DIST_CODE"].ToString(), _objds.Tables[0].Rows[0]["BLK_CODE"].ToString(), _objds.Tables[0].Rows[0]["GP_CODE"].ToString());
                        //ViewBag.fname3 = dt;
                        //foreach (System.Data.DataRow dr in ViewBag.fname3.Rows)
                        //{
                        //    items.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                        //}
                        //ViewBag.ddlVillName = new SelectList(items, "Value", "Text", _objds.Tables[0].Rows[0]["VILL_CODE"].ToString());

                        DataSet dtf = _objmodel.GetFixedDistMap(id);
                        List<clsPermission> fixedlandlist = new List<clsPermission>();
                        for (int i = 0; i < dtf.Tables.Count; i++)
                        {
                            foreach (DataRow dr in dtf.Tables[0].Rows)
                                fixedlandlist.Add(
                                    new clsPermission()

                                    {
                                        DIV_CODE = dr["DIV_NAME"].ToString(),
                                        DIST_CODE = dr["DIST_NAME"].ToString(),
                                        BLK_CODE = dr["BLK_NAME"].ToString(),
                                        GP_CODE = dr["GP_NAME"].ToString(),
                                        VILL_CODE = dr["VILL_NAME"].ToString(),
                                        KhasraNo = GetkasraValue(dr["KhasraNo"].ToString()),
                                        Area = dr["Area"].ToString()
                                    });

                        }
                        ViewData["fixedlandlist"] = fixedlandlist;
                        ViewBag.ApplicantType = new SelectList(Common.GetApplicantType(), "Value", "Text", _objds.Tables[0].Rows[0]["ApplicantType"].ToString());
                        ViewBag.NearestWaterSource = new SelectList(Common.GetNearestWaterSource(), "Value", "Text", _objds.Tables[0].Rows[0]["Nearest_WaterSource"].ToString());
                        ViewBag.Sawmill_Type = new SelectList(Common.GetSawmillType(), "Value", "Text", _objds.Tables[0].Rows[0]["Sawmill_Type"].ToString());
                        ViewBag.Industrial_Type = new SelectList(Common.GetIndustrialType(), "Value", "Text", _objds.Tables[0].Rows[0]["Industrial_Type"].ToString());
                        
                        _objmodel.ConditionFileEditMode = true;
                        //_objmodel.Area = _objds.Tables[0].Rows[0]["Area"].ToString();
                        _objmodel.Area_Size = _objds.Tables[0].Rows[0]["Area_Size"].ToString();
                        _objmodel.GPSLat = _objds.Tables[0].Rows[0]["GPSLat"].ToString();
                        _objmodel.GPSLong = _objds.Tables[0].Rows[0]["GPSLong"].ToString();
                        _objmodel.Revenue_Map_Signed = _objds.Tables[0].Rows[0]["Revenue_Map_Signed"].ToString();
                        _objmodel.IsGTSheetAvailable = _objds.Tables[0].Rows[0]["IsGTSheetAvaliable"].ToString();

                        _objmodel.KML_Path = _objds.Tables[0].Rows[0]["KML_Path"].ToString();
                        _objmodel.Revenue_Map_Path = _objds.Tables[0].Rows[0]["Revenue_Map_Path"].ToString();
                        _objmodel.Revenue_Record_Path = _objds.Tables[0].Rows[0]["Revenue_Record_Path"].ToString();
                        DateTime datefrom = new DateTime();
                        datefrom = Convert.ToDateTime(_objds.Tables[0].Rows[0]["DurationFrom"].ToString());
                        DateTime dateto = new DateTime();
                        dateto = Convert.ToDateTime(_objds.Tables[0].Rows[0]["DurationTo"].ToString());
                        _objmodel.Duration_From = datefrom.ToString("dd/MM/yyyy");

                        _objmodel.Duration_To = dateto.ToString("dd/MM/yyyy");
                        _objmodel.WaterSource_Distance = _objds.Tables[0].Rows[0]["WaterSource_Distance"].ToString();
                        _objmodel.Forest_Distance = _objds.Tables[0].Rows[0]["Forest_Distance"].ToString();
                        _objmodel.Wildlife_Distance = _objds.Tables[0].Rows[0]["Wildlife_Distance"].ToString();
                        _objmodel.Tree_species = _objds.Tables[0].Rows[0]["Tree_species"].ToString();
                        _objmodel.AravalliHills = _objds.Tables[0].Rows[0]["AravalliHills"].ToString();
                        _objmodel.ForestLand = _objds.Tables[0].Rows[0]["ForestLand"].ToString();
                        _objmodel.Plantation_Area = _objds.Tables[0].Rows[0]["Plantation_Area"].ToString();
                        _objmodel.Sawmill_Size = _objds.Tables[0].Rows[0]["Sawmill_Size"].ToString();
                        _objmodel.OtherPermission = _objds.Tables[0].Rows[0]["OtherPermission"].ToString();
                        _objmodel.Amount = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Amount"]);
                        _objmodel.Discount = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Discount"]);
                        _objmodel.Tax = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Tax"]);
                        _objmodel.Final_Amount = Convert.ToDecimal(_objds.Tables[0].Rows[0]["Final_Amount"]);
                        if (Convert.ToBoolean(_objds.Tables[0].Rows[0]["Revenue_Map_Signed"])) { _objmodel.ConditionRevenueMapSigned = "Yes"; } else { _objmodel.ConditionRevenueMapSigned = "No"; }
                        if (Convert.ToBoolean(_objds.Tables[0].Rows[0]["IsGTSheetAvaliable"])) { _objmodel.ConditionIsGTSheetAvailable = "Yes"; } else { _objmodel.ConditionIsGTSheetAvailable = "No"; }
                        _objmodel.Additional_Document = _objds.Tables[0].Rows[0]["Additional_Document"].ToString();
                        _objmodel.Citizen_Comment = _objds.Tables[0].Rows[0]["Citizen_Comment"].ToString();

                    }
                    return View("FixedPermission", _objmodel);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        public ActionResult ViewFile(string filePath)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                if (Session["UserID"] != null)
                {
                    UserID = Convert.ToInt64(Session["UserID"].ToString());
                    if (System.IO.File.Exists(Server.MapPath(filePath)))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            Arguments = Server.MapPath(filePath),
                            FileName = "explorer.exe"
                        };
                        Process.Start(startInfo);
                    }
                }

                else
                {
                    Response.Redirect("~/WebForm1.aspx?val=logout", false);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public JsonResult SaveDistMapping(string Division, string District, string BlockName, string GPName, string VillName, string khasra_no, string Area)
        {
            string data = "1";
            string _listKhasrano = string.Empty;
            try
            {

                List<clsPermission> _objData = new List<clsPermission>();
                if (Session["DivisionData"] != null)
                {
                    List<clsPermission> list = (List<clsPermission>)Session["DivisionData"];

                    if (list != null)
                    {
                        if (list.FirstOrDefault().DIV_CODE == Division)
                        {
                            var result = list.Find(x => x.VILL_CODE == VillName);
                            if (result == null)
                            {
                                if (khasra_no == "")
                                {
                                    _listKhasrano = "";
                                }
                                else
                                {
                                    string lang = khasra_no.ToString().TrimEnd(',');
                                    if (lang.Contains(','))
                                    {
                                        string[] langs = lang.Split(',');

                                        XmlDocument document = new XmlDocument();
                                        XmlElement root = document.CreateElement("KhasraRoot");
                                        document.AppendChild(root);

                                        for (int i = 0; i < langs.Length; i++)
                                        {
                                            XmlElement element = document.CreateElement(string.Empty, "KhasraValue", string.Empty);
                                            XmlText text1 = document.CreateTextNode(langs[i]);
                                            element.AppendChild(text1);
                                            root.AppendChild(element);
                                        }

                                        _listKhasrano = document.InnerXml.ToString();
                                    }
                                    else
                                    {

                                        XmlDocument document = new XmlDocument();
                                        XmlElement root = document.CreateElement("KhasraRoot");
                                        document.AppendChild(root);
                                        XmlElement element = document.CreateElement(string.Empty, "KhasraValue", string.Empty);
                                        XmlText text1 = document.CreateTextNode(lang);
                                        element.AppendChild(text1);
                                        root.AppendChild(element);
                                        _listKhasrano = document.InnerXml.ToString();
                                    }
                                }


                                clsPermission obj = new clsPermission { ID = "0", DIV_CODE = Division, DIST_CODE = District, BLK_CODE = BlockName, GP_CODE = GPName, VILL_CODE = VillName, KhasraNo = _listKhasrano, Area = Area };
                                list.Add(obj);
                                Session["DivisionData"] = list;
                            }
                            else
                            {
                                data = "2";

                            }


                        }
                        else
                        {
                            var result = list.Find(x => x.VILL_CODE == VillName);
                            if (result == null)
                            {
                                if (khasra_no == "")
                                {
                                    _listKhasrano = "";
                                }
                                else
                                {
                                    string lang = khasra_no.ToString().TrimEnd(',');
                                    if (lang.Contains(','))
                                    {
                                        string[] langs = lang.Split(',');

                                        XmlDocument document = new XmlDocument();
                                        XmlElement root = document.CreateElement("KhasraRoot");
                                        document.AppendChild(root);

                                        for (int i = 0; i < langs.Length; i++)
                                        {
                                            XmlElement element = document.CreateElement(string.Empty, "KhasraValue", string.Empty);
                                            XmlText text1 = document.CreateTextNode(langs[i]);
                                            element.AppendChild(text1);
                                            root.AppendChild(element);
                                        }

                                        _listKhasrano = document.InnerXml.ToString();
                                    }
                                    else
                                    {

                                        XmlDocument document = new XmlDocument();
                                        XmlElement root = document.CreateElement("KhasraRoot");
                                        document.AppendChild(root);
                                        XmlElement element = document.CreateElement(string.Empty, "KhasraValue", string.Empty);
                                        XmlText text1 = document.CreateTextNode(lang);
                                        element.AppendChild(text1);
                                        root.AppendChild(element);
                                        _listKhasrano = document.InnerXml.ToString();
                                    }
                                }

                                clsPermission obj = new clsPermission { ID = "0", DIV_CODE = Division, DIST_CODE = District, BLK_CODE = BlockName, GP_CODE = GPName, VILL_CODE = VillName, KhasraNo = _listKhasrano, Area = Area };
                                list.Add(obj);
                                Session["DivisionData"] = list;
                            }
                            else
                            {
                                data = "2";
                            }

                        }
                    }
                }
                else
                {
                    if (khasra_no == "")
                    {
                        _listKhasrano = "";
                    }
                    else
                    {
                        string lang = khasra_no.ToString().TrimEnd(',');
                        if (lang.Contains(','))
                        {
                            string[] langs = lang.Split(',');

                            XmlDocument document = new XmlDocument();
                            XmlElement root = document.CreateElement("KhasraRoot");
                            document.AppendChild(root);

                            for (int i = 0; i < langs.Length; i++)
                            {
                                XmlElement element = document.CreateElement(string.Empty, "KhasraValue", string.Empty);
                                XmlText text1 = document.CreateTextNode(langs[i]);
                                element.AppendChild(text1);
                                root.AppendChild(element);
                            }

                            _listKhasrano = document.InnerXml.ToString();
                        }
                        else
                        {

                            XmlDocument document = new XmlDocument();
                            XmlElement root = document.CreateElement("KhasraRoot");
                            document.AppendChild(root);
                            XmlElement element = document.CreateElement(string.Empty, "KhasraValue", string.Empty);
                            XmlText text1 = document.CreateTextNode(lang);
                            element.AppendChild(text1);
                            root.AppendChild(element);
                            _listKhasrano = document.InnerXml.ToString();
                        }
                    }
                    clsPermission obj = new clsPermission {ID = "0", DIV_CODE = Division, DIST_CODE = District, BLK_CODE = BlockName, GP_CODE = GPName, VILL_CODE = VillName, KhasraNo = _listKhasrano, Area = Area };
                    _objData.Add(obj);
                    Session["DivisionData"] = _objData;
                }

            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public string GetkasraValue(string KhasraNo )
        {

            
                var doc = new XmlDocument();
                doc.LoadXml(KhasraNo.ToString());


                XmlNodeList xnList = doc.SelectNodes("/KhasraRoot/KhasraValue");
                StringBuilder sb = new StringBuilder();
                List<String> list = new List<String>();
                for (int i = 0; i < xnList.Count; i++)
                {
                    string values = xnList[i].InnerText;
                    sb.Append(values + ",");
                }

                return sb.ToString().TrimEnd(',');
            
        }



    }

}
