using FMDSS.Models;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using FMDSS.Models.Home;
using FMDSS.Models.MIS;
using CaptchaMvc.HtmlHelpers;

namespace FMDSS.Controllers
{
    public class cls_userDetails
    {
        public string UserName { get; set; }
        public string mobileNumber { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
    }
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        GisServices oModel = new GisServices();


       [HttpPost]
        public ActionResult UserData(FormCollection fc)
        {

            string oUser = fc["UserName"].ToString();
            return View();
        }

        public ActionResult Home()
        {

            SMS_EMail_Services.sendSingleSMS("", "");


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            Ticker obj = new Ticker();
            try
            {



                DataTable dtf = obj.Select_Ticker_homepage();
                int count = 1;



                ViewData["TicketMessage"] = Convert.ToString(dtf.Rows[0]["TickerMessage"]);
                //DataTable dt = obj.OnlineBookingPopUp();
                //ViewData["PopUpContent"] = Convert.ToString(dt.Rows[0]["Content"]);
                //ViewData["PopUpContentStatus"] = Convert.ToString(dt.Rows[0]["Status"]);

                DataSet ds = new DataSet();
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                OnlineBookingPopUp model = new OnlineBookingPopUp();
                model.ModuleName = "LandingPage";
                ds = repo.GetAllOnlineBookingPopUpList("ShowPopUp", model);
                //Ticker obj1 = new Ticker();
                // DataTable dt = obj1.OnlineBookingPopUp();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewData["PopUpContent"] = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    ViewData["PopUpContentStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["Status"]);
                    ViewData["Title"] = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);
                }
                else
                {
                    ViewData["PopUpContent"] = string.Empty;
                    ViewData["PopUpContentStatus"] = string.Empty;
                    ViewData["Title"] = string.Empty;

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);


            }

            return View();
        }

        [HttpPost]
        public ActionResult CheckTicketInfo(string RequestId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Ticker obj = new Ticker();
            bool ErrorMessageFlag = true;
            #region Check Captcha Validation by Rajveer
            if (Convert.ToString(this.IsCaptchaValid("Validate your captcha")) == "False")
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            if (ErrorMessageFlag)
            {
                try
                {
                    DataTable dtf = obj.Check_Ticket(RequestId);
                    if (dtf.Rows.Count > 0)
                    {
                        obj.RequestId = dtf.Rows[0]["RequestId"].ToString();
                        obj.DateOfBooking = Convert.ToDateTime(dtf.Rows[0]["DateOfBooking"]).ToString("dd/MM/yyyy");
                        obj.Place = dtf.Rows[0]["Place"].ToString();
                        obj.Zone = dtf.Rows[0]["Zone"].ToString();
                        obj.Shift = dtf.Rows[0]["Shift"].ToString();
                        obj.Vehicle = dtf.Rows[0]["Vehicle"].ToString();
                        obj.VisitingDate = Convert.ToDateTime(dtf.Rows[0]["VisitingDate"]).ToString("dd/MM/yyyy");
                        obj.TotalMembers = dtf.Rows[0]["TotalMembers"].ToString();
                        obj.TotalAmount = dtf.Rows[0]["TotalAmount"].ToString();
                        obj.TicketStatus = dtf.Rows[0]["TicketStatus"].ToString();
                        obj.BookingType = dtf.Rows[0]["BookingType"].ToString();
                        obj.MemberNames = dtf.Rows[0]["MemberName"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);
                }
            }
            else
            {
                return View("Home");
            }
            return View(obj);
        }

        public ActionResult SwitchRoles(Int16 CurrentRole = 0)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                List<ROLEGROUPS> OBJListROLE = new List<ROLEGROUPS>();
                OBJListROLE = (List<ROLEGROUPS>)Session["ROLEGROUPS"];
                var Record = OBJListROLE.Where(item => item.RoleId == CurrentRole).FirstOrDefault();
                StringBuilder SB = new StringBuilder();
                Home obj = new Home();
                Session["CURRENT_Menus"] = obj.GetCurrentMenus(CurrentRole);

                Response.Redirect(Convert.ToString(Record.DefaultPage), false);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);

            }

            return View();
        }



        public ActionResult Nurserydetails()
        {
            ProductListVM productListVM = new ProductListVM();
            return View(productListVM);
        }

        //public class DropDownsMember
        //{

        //    public EnumerableRowCollection<SelectListItem> DistrictList { get; set; }


        //}
        [HttpGet]
        public JsonResult DropDownDistrict()
        {
            // DropDownsMember DesignationMasterDropDowns = new DropDownsMember();
            List<SelectListItem> items = new List<SelectListItem>();
            DataTable dataTable = oModel.DropDownDistrict();
            ViewBag.dt = dataTable;
            foreach (System.Data.DataRow dr in ViewBag.dt.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
            }


            return Json(items, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult DropDownNursery(string Dist_CODE)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataTable dataTable = oModel.DropDownNursery(Dist_CODE);
            ViewBag.dt = dataTable;
            foreach (System.Data.DataRow dr in ViewBag.dt.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult DropDownProduct()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DataTable dataTable = oModel.DropDownProduct();
            ViewBag.dt = dataTable;
            foreach (System.Data.DataRow dr in ViewBag.dt.Rows)
            {
                items.Add(new SelectListItem { Text = @dr["CommanEngName"].ToString(), Value = @dr["Id"].ToString() });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NurseryPlantdetails(string DIST_CODE, string NURSERY_CODE, int Id)
        {
            ProductListVM productListVM = new ProductListVM();
            DataSet ds = oModel.GetDetailsNursery(DIST_CODE, NURSERY_CODE, Id);
            productListVM.districtWiseNurseryProductList = Globals.Util.GetListFromTable<FMDSS.Models.Home.DistrictWiseNurseryProductList>(ds.Tables[1]);
            return View(productListVM);
        }


        [HttpPost]
        public ActionResult Nurserydetails(string DIST_CODE, string NURSERY_CODE, int Id)
        {
            ProductListVM productListVM = new ProductListVM();
            productListVM.DIST_CODE = DIST_CODE;
            productListVM.NURSERY_CODE = NURSERY_CODE;
            productListVM.Id = Id;
            bool ErrorMessageFlag = true;
            if (productListVM.DIST_CODE!=null)
            {
                #region Check Captcha Validation by Shahnawaz
                if (!this.IsCaptchaValid("Validate your captcha"))
                {
                    ErrorMessageFlag = false;
                    TempData["ErrorMessage"] = "Invalid captcha";
                }
                #endregion

                if (ErrorMessageFlag == true)
                {
                    
                    productListVM.ErrorMessageFlag = ErrorMessageFlag;
                    DataSet ds = oModel.GetDetailsNursery(DIST_CODE, NURSERY_CODE, Id);
                    productListVM.nurseryInformationList = ds.Tables[0].AsEnumerable().Select(x => new NurseryInformation()
                    {
                        NurseryName = x.Field<string>("NurseryName"),
                        NURSERY_CODE = x.Field<string>("NURSERY_CODE"),
                        NurseryAddress = x.Field<string>("NurseryAddress"),
                        NurseryImageUrl = x.Field<string>("NurseryImageUrl"),
                        NurseryIncharge = x.Field<string>("NurseryIncharge"),
                        Mobile = x.Field<string>("Mobile"),
                        NURSERY_LANDMARK = x.Field<string>("NURSERY_LANDMARK"),
                        LATITUDE = x.Field<decimal>("LATITUDE").ToString(),
                        LONGITUDE = x.Field<decimal>("LONGITUDE").ToString()
                    }).ToList();

                    return View(productListVM);
                }
            }
            return View(productListVM);
        }

        public JsonResult GetNurseryProductDetails(string DIST_CODE, string NURSERY_CODE, int Id)
        {
            List<ProductTypeInformation> productTypeInformationList = new List<ProductTypeInformation>();
            DataSet ds = oModel.GetDetailsNursery(DIST_CODE, NURSERY_CODE, Id);
            productTypeInformationList = Globals.Util.GetListFromTable<FMDSS.Models.Home.ProductTypeInformation>(ds.Tables[2]);
            return Json(productTypeInformationList,JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Failedtransaction()
        {
            List<FMDSS.Models.cls_UserSession.listFail> oFailList = new List<FMDSS.Models.cls_UserSession.listFail>();

            TempData["FailReport"] = oFailList;

            cls_UserSession.clsFailReport failObj = new cls_UserSession.clsFailReport();
            failObj.VisitDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View(failObj);
        }


        #region "Fail TRANgaction"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Failedtransaction(FMDSS.Models.cls_UserSession.clsFailReport fail)
        {
            cls_UserSession oSession = new cls_UserSession();
            List<FMDSS.Models.cls_UserSession.listFail> oFailList = new List<FMDSS.Models.cls_UserSession.listFail>();
            bool ErrorMessageFlag = true;
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();
            onloadTicketBooking();
            #region Check Captcha Validation by AMIT
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag)
            {
                try
                {

                    DataTable oDt = oSession.GetFailRecord(fail);
                    int count = 1;
                    for (int i = 0; i < oDt.Rows.Count - 1; i++)
                    {
                        FMDSS.Models.cls_UserSession.listFail oFail = new FMDSS.Models.cls_UserSession.listFail();
                        oFail.INDEX = count;
                        oFail.RequestID = Convert.ToString(oDt.Rows[i]["RequestID"]);
                        oFail.SSOID = Convert.ToString(oDt.Rows[i]["SSOID"]);
                        oFail.NAME = Convert.ToString(oDt.Rows[i]["NAME"]);
                        oFail.PlaceName = Convert.ToString(oDt.Rows[i]["PlaceName"]);
                        oFail.ZoneName = Convert.ToString(oDt.Rows[i]["ZoneName"]);
                        oFail.ShiftName = Convert.ToString(oDt.Rows[i]["ShiftName"]);
                        oFail.TotalMembers = Convert.ToString(oDt.Rows[i]["TotalMembers"]);
                        oFail.DAteofArrival = Convert.ToString(oDt.Rows[i]["DAteofArrival"]);
                        oFail.IP_Address = Convert.ToString(oDt.Rows[i]["IP_Address"]);
                        oFail.FAIL = Convert.ToString(oDt.Rows[i]["FAIL"]);
                        oFailList.Add(oFail);
                        count += 1;
                    }
                    TempData["FailReport"] = oFailList;
                }



                //}
                catch (Exception ex)
                {

                    throw ex.InnerException;
                }
            }
            return View();
        }

        #endregion




         #region "Seat Inventory"
        [HttpGet]
       // [OutputCache(CacheProfile = "Cache10Min")]
        public ActionResult InventoryReport()
        {
            Session["CurrentBookingRemainingDetailsDownload"] = null;
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();
            onloadTicketBooking();
            TempData["ListCurrentBookingRemainingDetails"] = List;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         //[OutputCache(Duration = 600, VaryByParam = "none",Location= OutputCacheLocation.Client)]  
        public ActionResult InventoryReport(MISCommonModel MIS)
        {

            bool ErrorMessageFlag = true;
            List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();
            onloadTicketBooking();
            #region Check Captcha Validation by AMIT
            if (!this.IsCaptchaValid("Validate your captcha"))
            {
                ErrorMessageFlag = false;
                TempData["ErrorMessage"] = "Invalid captcha";
            }
            #endregion

            else if (ErrorMessageFlag)
            {

                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());

                string remoteIp = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                try
                {
                  
                        MISCommonModel obj = new MISCommonModel();
                        DataTable DT;
                        DT = obj.GetCitizenCurrentBookingRemainingSeatsDetails("GETREMAININGSEAT", MIS.FromDate, MIS.Place, MIS.SHIFT_TYPE, UserID);
                        int count = 1;
                        foreach (DataRow dr in DT.Rows)
                        {
                            List.Add(
                                new ViewTicketRemaningDT1()
                                {
                                    Index = count,
                                    VehicleName = Convert.ToString(dr["VehicleName"]),
                                    ZoneName = Convert.ToString(dr["ZoneName"]),
                                    PlaceName = Convert.ToString(dr["PlaceName"]),
                                    seatsForCitizen = Convert.ToInt64(dr["SeatForOnline"]),
                                    CurrentBookingSeat = Convert.ToInt64(dr["SeatForCurrent"]),
                                    TotalSeats = Convert.ToInt64(dr["TotalSeats"]),
                                    CitizenbookedSeats = Convert.ToInt64(dr["OnlineBooked"]),
                                    CurrentbookedSeats = Convert.ToInt64(dr["CurrentBooked"]),
                                    TotalBookedSeat = Convert.ToInt64(dr["TotalBookedSeat"]),
                                    CitizenRemainingSeat = Convert.ToInt64(dr["OnlineRemaining"]),
                                    CurrentRemainingSeat = Convert.ToInt64(dr["CurrentRemaining"]),
                                    RemainingSeat = Convert.ToInt64(dr["RemainingSeat"]),
                                    UpComming = Convert.ToInt64(dr["UpComming"]),
                                    UnderProcess = Convert.ToInt64(dr["UnderProcess"]),

                                });
                            count += 1;
                        }
                        //HttpContext.Cache.Insert(remoteIp, List, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                    

                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 1, DateTime.Now, UserID);
                }
            }
            TempData["ListCurrentBookingRemainingDetails"] = List;
            return View(MIS);
            

        }
        
        public void onloadTicketBooking()
        {
            ResearchPlantsAnimals obj = new ResearchPlantsAnimals();
            List<SelectListItem> Places = new List<SelectListItem>();
            List<SelectListItem> BookingType = new List<SelectListItem>();
            List<SelectListItem> ShiftType = new List<SelectListItem>();
            List<SelectListItem> ShiftTypeRemaingSeats = new List<SelectListItem>();
            List<SelectListItem> DATEtYPE = new List<SelectListItem>();
            List<SelectListItem> RemarksList = new List<SelectListItem>();

            List<SelectListItem> lstTRNS_Status = new List<SelectListItem>();

            List<SelectListItem> lstModeOfBooking = new List<SelectListItem>();


            Zone cst = new Zone();


            DataTable dtPlace = new DataTable();

            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                FMDSS.Models.OnlineBooking.CS_BoardingDetails obj1 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();
                DataSet Ds = new DataSet();
                Ds = obj1.BindDptKioskPLACES(Session["SSOid"].ToString());

                foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

                lstModeOfBooking.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Kiosk Booking", Value = "2" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Counter Booking", Value = "1" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Online Booking", Value = "0" });
            }
            else
            {
                dtPlace = cst.SelectPlaces();
                foreach (System.Data.DataRow dr in dtPlace.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

                lstModeOfBooking.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Kiosk Booking", Value = "2" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Counter Booking", Value = "1" });
                lstModeOfBooking.Add(new SelectListItem { Text = "Online Booking", Value = "0" });
            }

            DataTable dtRemarks = new DataTable();
            dtRemarks = cst.SelectOnlineBookingRefundRemarks();
            foreach (System.Data.DataRow dr in dtRemarks.Rows)
            {
                RemarksList.Add(new SelectListItem { Text = @dr["RemarksName"].ToString(), Value = @dr["RemarksName"].ToString() });
            }
            ViewBag.RemarksList = RemarksList;

            BookingType.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            BookingType.Add(new SelectListItem { Text = "Online", Value = "Online" });
            BookingType.Add(new SelectListItem { Text = "Kiosk User", Value = "Kiosk User" });
            BookingType.Add(new SelectListItem { Text = "Departmental Kiosk User", Value = "Departmental Kiosk User" });

            ShiftType.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
           // ShiftType.Add(new SelectListItem { Text = "Full Day", Value = "3" });
            ShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
            ShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

          //  ShiftTypeRemaingSeats.Add(new SelectListItem { Text = "Full Day", Value = "3" });
            ShiftTypeRemaingSeats.Add(new SelectListItem { Text = "Morning", Value = "1" });
            ShiftTypeRemaingSeats.Add(new SelectListItem { Text = "Evening", Value = "2" });


            DATEtYPE.Add(new SelectListItem { Text = "Date of Visit", Value = "DATEOFVISITE" });
            DATEtYPE.Add(new SelectListItem { Text = "Date of Booking", Value = "DATEOFBOOKING" });

            lstTRNS_Status.Add(new SelectListItem { Text = "All", Value = "-1" });
            lstTRNS_Status.Add(new SelectListItem { Text = "Success", Value = "1" });
            lstTRNS_Status.Add(new SelectListItem { Text = "Failed", Value = "0" });

            ViewBag.ddlShiftType1 = ShiftType;
            ViewBag.ddlShiftTypeRemaingSeats = ShiftTypeRemaingSeats;
            ViewBag.ddlBookingType1 = BookingType;
            ViewBag.ddlPlace1 = Places;
            ViewBag.ddlDATEtYPE1 = DATEtYPE;
            ViewBag.ddlTRNS_Status1 = lstTRNS_Status;

            ViewBag.ddlModeOfBooking = lstModeOfBooking;
        }
        #endregion
    }
}
