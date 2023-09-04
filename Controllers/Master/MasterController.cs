// Arvind Kumar Sharma
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.Master;
using System.Data.OleDb;
using System.Data;
using FMDSS.Models.Admin;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using FMDSS.Models;
using FMDSS.Filters;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.ForestFire;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.BookOnlineTicket;

namespace FMDSS.Controllers.Master
{
    [MyAuthorization]
    public class MasterController : BaseController
    {


        List<TransportMaster> lstTransportMaster = new List<TransportMaster>();
        List<Districts> lstDistricts = new List<Districts>();

        List<ResearchPlantsAnimals> lstResearchPlantsAnimals = new List<ResearchPlantsAnimals>();
        List<CoordinatorRegistration> lstCoordinatorRegistration = new List<CoordinatorRegistration>();

        List<Places> lstPlace = new List<Places>();
        List<Region> region = new List<Region>();
        List<wcircles> circle = new List<wcircles>();
        List<TicketingFee> ticket = new List<TicketingFee>();
        List<VehicleEquipment> VehicleEquipmentLst = new List<VehicleEquipment>();
        List<VehicleEquipmentFee> VehicleEquipmentFeeLst = new List<VehicleEquipmentFee>();
        List<EmitraKioskLinkDirectOrBypass> EmitraKioskLinkDirectOrBypassLst = new List<EmitraKioskLinkDirectOrBypass>();

        List<EqptSanctuariesFee> EqptSanctuariesFeeLst = new List<EqptSanctuariesFee>();
        List<AccomodationFee> AccomodationFeeLst = new List<AccomodationFee>();
        List<CampFees> LstCampFees = new List<CampFees>();
        List<FilmShootingFees> LstFilmShootingFees = new List<FilmShootingFees>();
        List<FilmShootingSecurityDeposit> LstFilmShootingSecurityDeposit = new List<FilmShootingSecurityDeposit>();

        List<Zone> LstZone = new List<Zone>();
        List<PlaceBookingDuration> LstPlaceBookingDuration = new List<PlaceBookingDuration>();
        List<ZooSeatInventory> ZooSeatInventoryLst = new List<ZooSeatInventory>();
        List<SelectListItem> LSTPlaceName = new List<SelectListItem>();


        Location _objLocation = new Location();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> Place1 = new List<SelectListItem>();
        List<SelectListItem> Category = new List<SelectListItem>();
        List<SelectListItem> lstShiftType = new List<SelectListItem>();
        List<SelectListItem> lstISactive = new List<SelectListItem>();
        List<SelectListItem> lstSafariAvailable = new List<SelectListItem>();
        RowStatus objRowStatus = new RowStatus();
        List<NurseryFDMProduct> NurseryFDMProductLst = new List<NurseryFDMProduct>();

        List<SelectListItem> LSTNurseryFDMProduct = new List<SelectListItem>();

        List<ZooVehicle> ZVehicleLst = new List<ZooVehicle>();
        List<ZooEqptFee> ZooEqptFeeLst = new List<ZooEqptFee>();
        List<ZooPlaceWiseHead> ZooPlaceWiseHeadLst = new List<ZooPlaceWiseHead>();
        List<ZooHeadMaster> ZooHeadMasterLst = new List<ZooHeadMaster>();
        List<SelectListItem> LSTVehicalEqptName = new List<SelectListItem>();
        List<SelectListItem> LSTZooPlaceWiseHead = new List<SelectListItem>();

        List<SelectListItem> LSTVehicalType = new List<SelectListItem>();
        List<SelectListItem> lstFeeChargeon = new List<SelectListItem>();
        List<SelectListItem> lstType = new List<SelectListItem>();
        List<SelectListItem> lstParentFeeChargeon = new List<SelectListItem>();
        List<ZooPlaces> ZooPlaceLst = new List<ZooPlaces>();
        List<SelectListItem> lstisAvailable = new List<SelectListItem>();
        List<Designations> DesignationLst = new List<Designations>();


        List<Users> lstUsers = new List<Users>();
        Users _objRole = new Users();

        List<Ticker> TickerLst = new List<Ticker>();


        List<SelectListItem> lstisReviewer = new List<SelectListItem>();
        List<SelectListItem> lstisApprover = new List<SelectListItem>();


        List<ContactUs> ContactLst = new List<ContactUs>();
        List<Navigation> NavigationLst = new List<Navigation>();
        List<ImportLink> ImportLinkLst = new List<ImportLink>();

        List<ZooHeaderFooter> ZooHeaderFooterLst = new List<ZooHeaderFooter>();

        List<FixedPermissionTypes> lstFixedPermissionTypes = new List<FixedPermissionTypes>();
        List<SelectListItem> lstStatus = new List<SelectListItem>();
        List<EducationQualification> lstEducationQualification = new List<EducationQualification>();
        List<IndustryType> lstIndustryType = new List<IndustryType>();
        List<SawmillType> lstSawmillType = new List<SawmillType>();

        List<UnitNameMaster> UnitNameMasterLst = new List<UnitNameMaster>();
        List<ProduceTypes> ProduceTypeLst = new List<ProduceTypes>();
        List<NurseryHeadMaster> NurseryHeadMasterLst = new List<NurseryHeadMaster>();

        List<ValueTypeName> lstValueTypeName = new List<ValueTypeName>();
        List<BaseProduceType> lstBaseProduceType = new List<BaseProduceType>();
        List<NurseryDiscountType> lstNurseryDiscountType = new List<NurseryDiscountType>();

        List<ADWorkDescription> lstAD_WorkDescription = new List<ADWorkDescription>();
        List<ADCategory> lstAD_Category = new List<ADCategory>();
        List<SelectListItem> TreeAgeList = new List<SelectListItem>();

        #region Protection Module

        List<ForestVillageType> lstForestVillageType = new List<ForestVillageType>();
        List<ForestProtectionAct> lstForestProtectionAct = new List<ForestProtectionAct>();
        List<WildlifeProtectionAct> lstWildlifeProtectionAct = new List<WildlifeProtectionAct>();
        List<ForestCategory> lstForestCategory = new List<ForestCategory>();
        List<DecisionTaken> lstDecisionTaken = new List<DecisionTaken>();
        List<ReasonCaseFailed> lstReasonCaseFailed = new List<ReasonCaseFailed>();

        #endregion


        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ExecuteQuery()
        {
            if (Convert.ToString(Session["CURRENT_ROLE"]).Equals("1"))
            {
                ViewBag.Status = "";
                return View();
            }
            else
                return RedirectToAction("dashboard", "dashboard");
        }
        public ActionResult SubmitQuery(FormCollection frm)
        {
            SuperAdminOperations sao = new SuperAdminOperations();
            DataSet ds = new DataSet();
            string Qry = string.Empty;
            if (frm["Query"].ToString() != null && frm["Query"].ToString() != "")
            {

                if (!CheckKeyWord(frm["Query"].ToString()))
                {
                    ViewBag.Status = "";
                    Qry = frm["Query"].ToString();
                    ds = sao.ExecuteQuery(Qry);

                    if (ds.Tables.Count > 0)
                    {
                        MakeHtmlTable(ds);
                    }
                }
                else
                {
                    ViewBag.Status = "Only ' SELECT ' statements are allowed ";
                }

            }



            return View("ExecuteQuery");

        }

        public void MakeHtmlTable(DataSet ds)
        {

            HttpResponse Response = System.Web.HttpContext.Current.Response;
            string attachment = "attachment; filename=Report.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                tab = "";
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }


        public bool CheckKeyWord(string Query)
        {
            bool status = true;
            ///string[] arrValues = new string[] { "DELETE", "UPDATE", "INSERT", "MERGE", "ALTER", "CREATE", "DISABLE", "TRIGGER", "DROP", "ENABLE", "TRUNCATE", "INTO", "SAVE", "GRANT", "REVOKE" };
            string[] arrValues = new string[] { "MERGE", "DISABLE", "TRIGGER", "DROP", "ENABLE", "SAVE", "GRANT", "REVOKE" };

            foreach (string val in arrValues)
            {

                status = Query.ToUpper().Contains(val);
                if (status == true)
                {
                    status = true;
                    break;
                }
            }

            return status;
        }

        #region "Districts"
        [HttpGet]
        public ActionResult Districts()
        {


            Districts obj = new Districts();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                DataTable dtf = obj.Select_District();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstDistricts.Add(
                        new Districts()
                        {
                            Index = count,
                            ROWID = Convert.ToInt64(dr["ROWID"].ToString()),
                            STATE_CODE = Convert.ToString(dr["STATE_CODE"]),
                            DIST_CODE = Convert.ToString(dr["DIST_CODE"]),
                            DIST_NAME = Convert.ToString(dr["DIST_NAME"]),
                            ISOrganisingCamp = Convert.ToBoolean(dr["ISOrganisingCamp"]),
                            IsActive = Convert.ToBoolean(Convert.ToString(dr["IsActive"])),
                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstDistricts);
        }


        public JsonResult UpdateDistISOrganisingCamp(Int64 ID, bool STATUS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = "0";
            try
            {
                Districts District = new Districts();
                District.ROWID = ID;
                District.STATUS = Convert.ToInt16(STATUS);
                District.CurrentAction = "DistrictsUPDATEISOrganisingCamp";
                District.UpdateDistrict(District);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region "Places"
        public ActionResult Places(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                Places obj = new Places();
                DataTable dtf = obj.Select_Places();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstPlace.Add(
                        new Places()
                        {
                            Index = count,
                            PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                            DisNAme = Convert.ToString(dr["DIST_NAME"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            Category = Convert.ToString(dr["Category"]),
                            FullDayShift = Convert.ToString(dr["FullDayShift"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                            TicketAllocatedPerShift = Convert.ToInt32(Convert.ToString(dr["TicketAllocatedPerShift"])),


                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstPlace);
        }

        public ActionResult ADDUpdatePlace(Places oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Places obj = new Places();
                //  obj.Placeauto();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdatePlace(oPlace);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Places", new { RecordStatus = status });
        }
        public ActionResult GetPlace(string PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Places obj = new Places();
            try
            {
                List<SelectListItem> District = new List<SelectListItem>();
                List<SelectListItem> items = new List<SelectListItem>();

                DataTable dt = _objLocation.District();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.OpType = (PlaceID == "0" ? "Add Place" : "Edit Place");

                dt = obj.SelectPlaceCategory();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Category.Add(new SelectListItem { Text = @dr["Category"].ToString(), Value = @dr["Category"].ToString() });
                }
                DataTable dtf = obj.Select_Place(Convert.ToInt32(PlaceID));
                lstShiftType.Add(new SelectListItem { Text = "Morning / Evening", Value = "0" });
                lstShiftType.Add(new SelectListItem { Text = "Full Day", Value = "1" });


                ViewBag.ShiftType = lstShiftType;

                lstSafariAvailable.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstSafariAvailable.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.SafariAvailablelst = lstSafariAvailable;


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                lstisAvailable.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisAvailable.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.ISavilablelst = lstisAvailable;


                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Places
                    {
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        PlaceName = Convert.ToString(dr["PlaceName"]),
                        Category = Convert.ToString(dr["Category"]),
                        MorningShiftFrom = Convert.ToString(dr["MorningShiftFrom"]),
                        MorningShiftTo = Convert.ToString(dr["MorningShiftTo"]),
                        EveningShiftFrom = Convert.ToString(dr["EveningShiftFrom"]),
                        EveningShiftTo = Convert.ToString(dr["EveningShiftTo"]),
                        FullDayShift = Convert.ToString(dr["FullDayShift"]),
                        Code = Convert.ToString(dr["Code"]),
                        isZooAvailable = Convert.ToInt16(dr["isZooAvailable"]),
                        DIST_CODE = Convert.ToString(dr["DIST_CODE"]),
                        TicketAllocatedPerShift = Convert.ToInt32(Convert.ToString(dr["TicketAllocatedPerShift"])),

                        IsAccommodation = Convert.ToString(dr["IsAccommodation"]),

                        IsSafari = Convert.ToInt32(dr["IsSafari"]),

                        Isactive = Convert.ToInt32(dr["Isactive"]),
                        //SafariAvailability = Convert.ToString(dr["SafariAvailability"]),

                        IsZone = Convert.ToInt32(dr["IsZone"]),
                        ContactPerson = Convert.ToString(dr["ContactPerson"]),
                        Address = Convert.ToString(dr["Address"]),
                        PhoneNo = Convert.ToString(dr["PhoneNo"]),
                        Boarding_Point = Convert.ToString(dr["Boarding_Point"]),

                        IsOnlineBooking = Convert.ToInt32(dr["IsOnlineBooking"]),
                        IsCamping = Convert.ToInt32(dr["IsCamping"]),
                        IsResearch = Convert.ToInt32(dr["IsResearch"]),
                        MaxBookingDuration = Convert.ToInt32(dr["MaxBookingDuration"]),

                        isMorning = Convert.ToInt32(dr["isMorning"]),
                        isEvening = Convert.ToInt32(dr["isEvening"]),
                        isFullDay = Convert.ToInt32(dr["isFullDay"]),

                        isDptKiosk = Convert.ToInt32(dr["isDptKiosk"]),
                        isCitizen = Convert.ToInt32(dr["isCitizen"]),
                        Tax = Convert.ToDecimal(dr["Tax"]),
                        EmitraCharges = Convert.ToDecimal(dr["EmitraCharges"]),


                        OperationType = "Edit Place"
                    };

                }

                ViewBag.ddlDistrict1 = District;
                ViewBag.Category1 = Category;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialplace", obj);
        }


        [HttpPost]
        public JsonResult CheckDuplicateForPlace(int PlaceID, string DIST_CODE, string PlaceName, string Category)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = "0";
            try
            {
                Places Placess = new Places();

                Placess.PlaceID = PlaceID;
                Placess.DIST_CODE = DIST_CODE;
                Placess.PlaceName = PlaceName;
                Placess.Category = Category;
                status = Placess.Check_DuplicateRecord();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region "TicketingFees"
        public ActionResult TicketingFees(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                TicketingFee obj = new TicketingFee();
                DataTable dtf = obj.Select_Places();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ticket.Add(
                        new TicketingFee()
                        {
                            Index = count,
                            FeesID = Convert.ToInt64(dr["FeesID"].ToString()),
                            DIST_CODE = Convert.ToString(dr["DIST_CODE"]),
                            DisNAme = Convert.ToString(dr["DIST_NAME"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ticket);
        }
        public ActionResult ADDUpdateTicket(TicketingFee oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string status = null;
            try
            {
                TicketingFee obj = new TicketingFee();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdatePlace(oPlace);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("TicketingFees", new { RecordStatus = status });
        }
        public ActionResult GetTicket(string FeesID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            TicketingFee obj = new TicketingFee();

            try
            {
                List<SelectListItem> District = new List<SelectListItem>();

                List<SelectListItem> items = new List<SelectListItem>();

                List<SelectListItem> Places = new List<SelectListItem>();

                DataTable dt = _objLocation.District();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }

                ViewBag.OpType = (FeesID == "0" ? "Add Ticket" : "Edit Ticket");


                DataTable dtf = obj.Select_Place(Convert.ToInt32(FeesID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new TicketingFee
                    {
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        FeesID = Convert.ToInt64(dr["FeesID"].ToString()),
                        DIST_CODE = Convert.ToString(dr["DIST_CODE"]),

                        IndianAdultFees_Surcharge = Convert.ToDecimal(dr["IndianAdultFees_Surcharge"]),
                        IndianAdultFees_TigerProject = Convert.ToDecimal(dr["IndianAdultFees_TigerProject"]),
                        Foreigner_Surcharge = Convert.ToDecimal(dr["Foreigner_Surcharge"]),
                        Foreigner_TigerProject = Convert.ToDecimal(dr["Foreigner_TigerProject"]),
                        Indian_TRDF = Convert.ToDecimal(dr["Indian_TRDF"]), // canter TRDF
                        Foreigner_TRDF = Convert.ToDecimal(dr["Foreigner_TRDF"]), // canter TRDF

                        Indian_TRDF_Gypsy = Convert.ToDecimal(dr["Indian_TRDF_Gypsy"]),
                        Foreigner_TRDF_Gypsy = Convert.ToDecimal(dr["Foreigner_TRDF_Gypsy"]),


                        GuideFees = Convert.ToDecimal(dr["GuideFees"]),
                        CameraFeesForeigner_TigerProject = Convert.ToDecimal(dr["CameraFeesForeigner_TigerProject"]),
                        CameraFeesForeigner_Surcharge = Convert.ToDecimal(dr["CameraFeesForeigner_Surcharge"]),
                        CameraFeesIndian_TigerProject = Convert.ToDecimal(dr["CameraFeesIndian_TigerProject"]),
                        CameraFeesIndian_Surcharge = Convert.ToDecimal(dr["CameraFeesIndian_Surcharge"]),
                        Isactive = Convert.ToInt32(dr["Isactive"]),
                        StudentFees = Convert.ToDecimal(dr["StudentFees"]),


                        SingleOccupancyFees = Convert.ToDecimal(dr["SingleOccupancyFees"]),
                        DoubleOccupancyFees = Convert.ToDecimal(dr["DoubleOccupancyFees"]),

                        //Rajveer
                        Indian_GypsyVehicleRentFees = Convert.ToDecimal(dr["Indian_GypsyVehicleRentFees"]),
                        Indian_CanterVehicleRentFees = Convert.ToDecimal(dr["Indian_CanterVehicleRentFees"]),
                        Indian_GypsyGuideFee = Convert.ToDecimal(dr["Indian_GypsyGuideFee"]),
                        Indian_CanterGuideFee = Convert.ToDecimal(dr["Indian_CanterGuideFee"]),

                        Foreigner_GypsyVehicleRentFees = Convert.ToDecimal(dr["Foreigner_GypsyVehicleRentFees"]),
                        Foreigner_CanterVehicleRentFees = Convert.ToDecimal(dr["Foreigner_CanterVehicleRentFees"]),
                        Foreigner_GypsyGuideFee = Convert.ToDecimal(dr["Foreigner_GypsyGuideFee"]),
                        Foreigner_CanterGuideFee = Convert.ToDecimal(dr["Foreigner_CanterGuideFee"]),

                        GSTonGuideFee = Convert.ToDecimal(dr["GSTonGuideFee"]),
                        GSTonVehicleRentFee = Convert.ToDecimal(dr["GSTonVehicleRentFee"]),

                        Vehicle_TRDF_Gypsy = Convert.ToDecimal(dr["Vehicle_TRDF_Gypsy"]),
                        Vehicle_TRDF_Canter = Convert.ToDecimal(dr["Vehicle_TRDF_Canter"]),


                        GuidFee_TRDF_Indian_Gypsy = Convert.ToDecimal(dr["GuidFee_TRDF_Indian_Gypsy"]),
                        GuidFee_TRDF_NonIndian_Gypsy = Convert.ToDecimal(dr["GuidFee_TRDF_NonIndian_Gypsy"]),
                        GuidFee_TRDF_Indian_Canter = Convert.ToDecimal(dr["GuidFee_TRDF_Indian_Canter"]),
                        GuidFee_TRDF_NonIndian_Canter = Convert.ToDecimal(dr["GuidFee_TRDF_NonIndian_Canter"]),


                        //End
                        OperationType = "Edit Ticket"
                    };


                }

                if (ViewBag.OpType == "Edit Ticket")
                {
                    dt = obj.SelectPlaceWithCategory(obj.DIST_CODE);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                    {
                        Places.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                    }
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlPlace1 = Places;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialticket", obj);
        }
        public ActionResult DeleteTicket(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                TicketingFee obj = new TicketingFee();
                obj.FeesID = id;
                obj.DeletePlace();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("TicketingFees");
        }
        public JsonResult PlaceByDistrict(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                TicketingFee cst = new TicketingFee();
                DataTable dt = new DataTable();

                dt = cst.SelectPlaceWithCategory(Convert.ToString(districtID));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(Places, "Value", "Text"));
        }

        public JsonResult CheckDuplicateForTicket(int FeesID, int PlaceID, string DIST_CODE)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                TicketingFee TicketingFees = new TicketingFee();
                TicketingFees.FeesID = FeesID;
                TicketingFees.PlaceID = PlaceID;
                TicketingFees.DIST_CODE = DIST_CODE;


                status = TicketingFees.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }


        #endregion

        #region "VehicleEquipment"
        public ActionResult VehicleEquipment(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            VehicleEquipment obj = new VehicleEquipment();
            try
            {

                DataTable dtf = obj.Select_VehicleEquipments();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    VehicleEquipmentLst.Add(
                        new VehicleEquipment()
                        {
                            Index = count,
                            CategoryID = Convert.ToInt64(dr["CategoryID"].ToString()),
                            CategoryName = Convert.ToString(dr["CategoryName"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(VehicleEquipmentLst);
        }
        public ActionResult ADDUpdateVehicleEquipment(VehicleEquipment oVehicleEquipment)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                VehicleEquipment obj = new VehicleEquipment();
                oVehicleEquipment.EnteredBy = Convert.ToString(UserID);
                oVehicleEquipment.UpdatedBy = Convert.ToString(UserID);
                DataTable dtf = obj.AddUpdateVehicleEquipment(oVehicleEquipment);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("VehicleEquipment", new { RecordStatus = status });
        }
        public ActionResult GetVehicleEquipment(string CategoryID)
        {

            VehicleEquipment obj = new VehicleEquipment();
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                List<SelectListItem> District = new List<SelectListItem>();

                List<SelectListItem> items = new List<SelectListItem>();

                ViewBag.OpType = (CategoryID == "0" ? "Add Vehicle Equipment" : "Edit Vehicle Equipment");


                DataTable dtf = obj.Select_VehicleEquipment(Convert.ToInt32(CategoryID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new VehicleEquipment
                    {
                        CategoryID = Convert.ToInt64(dr["CategoryID"].ToString()),
                        CategoryName = Convert.ToString(dr["CategoryName"].ToString()),
                        Isactive = Convert.ToInt32(dr["Isactive"]),



                        OperationType = "Edit Vehicle Equipment"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialVehicleEquipment", obj);
        }
        public ActionResult DeleteVehicleEquipment(int id)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                VehicleEquipment obj = new VehicleEquipment();
                obj.CategoryID = id;
                obj.DeleteVehicleEquipment();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("VehicleEquipment");
        }
        public JsonResult CheckDuplicateForVehicleEquipment(int CategoryID, string CategoryName)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                VehicleEquipment VehicleEquipments = new VehicleEquipment();
                VehicleEquipments.CategoryID = CategoryID;
                VehicleEquipments.CategoryName = CategoryName;
                status = VehicleEquipments.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }



        #endregion

        #region "Roster Details Master"
         
        public ActionResult RosterDetails()
        {

            RosterDetails oObjeRoster = new RosterDetails();
            DataTable odt= oObjeRoster.Select_RosterPlace();

            ViewBag.PlaceList = odt.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.Field<string>("PLACEID"),
                Text = x.Field<string>("PLACENAME")
            });
            return View();
        }

       
        public JsonResult GetRosterZone(string PlaceId)
        {
            RosterDetails oObjeRoster = new RosterDetails();
            DataTable odt = oObjeRoster.Select_RosterZone(PlaceId);
            var oZone = odt.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.Field<string>("zoneID"),
                Text = x.Field<string>("ZoneName")
            });            
            return Json(new { data= oZone }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRosterShift(string PlaceId,string ZoneId)
        {
            string DATEOFVISITE = string.Empty;
            string SHIFT_TYPE = string.Empty;
            string strSHIFT = string.Empty;
            List<BookOnTicket> fees = new List<BookOnTicket>();
            RosterDetails oObjeRoster = new RosterDetails();
            DataTable odt = oObjeRoster.Select_RosterShift(PlaceId, ZoneId);
            DateTime DateTimes = DateTime.Now;
            int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

            if (Convert.ToBoolean(odt.Rows[0]["IsEveningShift"]) == true)
            {
                // =========== EVENING_SHIFT
                if (CurrentTime >= Convert.ToInt16(odt.Rows[0]["KioskBookingEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(odt.Rows[0]["KioskBookingEveningTimeTo"]))
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "2";
                    strSHIFT = "EVENING";
                    fees.Add(new BookOnTicket()
                    {
                        isMorning = "False",
                        isEvening = "True",
                        isFullDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["isFullDayShift"])),
                        isHalfDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["IsHalfDayShift"])),
                    });
                }
            }
            if (Convert.ToBoolean(odt.Rows[0]["IsMorningShift"]) == true)
            {
                // =========== MORNING_SHIFT                       
                if (CurrentTime >= Convert.ToInt16(odt.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(odt.Rows[0]["KioskBookingMorningTimeTo"]))
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    SHIFT_TYPE = "1";
                    strSHIFT = "MORNING";
                    fees.Add(new BookOnTicket()
                    {
                        isMorning = "True",
                        isEvening = "False",
                        isFullDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["isFullDayShift"])),
                        isHalfDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["IsHalfDayShift"])),
                    });
                }
            }

            if (Convert.ToBoolean(odt.Rows[0]["IsHalfDayShift"]) == true)
            {
                // =========== MORNING_SHIFT                       
                if (CurrentTime >= Convert.ToInt16(odt.Rows[0]["KioskBookingHalfDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(odt.Rows[0]["KioskBookingHalfDayEveningTimeTo"]))
                {
                    DATEOFVISITE = DateTime.Now.ToString("dd/MM/yyyy");
                    if (fees.Count > 0 && (fees.FirstOrDefault().isMorning == "True" || fees.FirstOrDefault().isEvening == "True"))
                    {
                        fees.FirstOrDefault().isHalfDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["IsHalfDayShift"]));
                    }
                    else
                    {
                        SHIFT_TYPE = "4";
                        strSHIFT = "MORNING";
                        fees.Add(new BookOnTicket()
                        {
                            isMorning = "False",
                            isEvening = "False",
                            isFullDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["isFullDayShift"])),
                            isHalfDay = Convert.ToString(Convert.ToBoolean(odt.Rows[0]["IsHalfDayShift"])),
                        });
                    }
                }
            } 
            //var oZone = odt.AsEnumerable().Select(x => new SelectListItem
            //{
            //    Value = x.Field<string>("zoneID"),
            //    Text = x.Field<string>("ZoneName")
            //});
            return Json(new { data = strSHIFT }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LoadRosterData(string PlaceId,string ShiftId,string ZoneId,string DateofVisit)
        {
            RosterDetails oObjeRoster = new RosterDetails();
            var oModal = oObjeRoster.LoadRosterData(PlaceId, ZoneId, ShiftId, DateofVisit);
            return PartialView("_PartialRosterDetails",oModal);
        }

        [HttpPost]
        public ActionResult SaveRosterDetails(List<RosterVehicleDetails> model)
        {
            RosterDetails oObjeRoster = new RosterDetails();
           // var oModal = oObjeRoster.SaveRosterDetails(model);
            return View();
        }



        #endregion

        #region "VehicleEquipmentFee"
        public ActionResult VehicleEquipmentFee(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                VehicleEquipmentFee obj = new VehicleEquipmentFee();
                DataTable dtf = obj.Select_VehicleEquipmentFee();
                TempData["RecordStatus"] = dtf.Rows[0][0].ToString();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    VehicleEquipmentFeeLst.Add(
                        new VehicleEquipmentFee()
                        {
                            Index = count,
                            VehicleID = Convert.ToInt64(dr["VehicleID"]),
                            CategoryID = Convert.ToInt64(dr["CategoryID"]),
                            CategoryName = Convert.ToString(dr["CategoryName"]),
                            Name = Convert.ToString(dr["Name"]),
                            Fees = Convert.ToInt64(dr["Fees"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(VehicleEquipmentFeeLst);
        }
        public ActionResult ADDUpdateVehicleEquipmentFee(VehicleEquipmentFee oVehicleEquipment)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                VehicleEquipmentFee obj = new VehicleEquipmentFee();
                oVehicleEquipment.EnteredBy = Convert.ToString(UserID);
                oVehicleEquipment.UpdatedBy = Convert.ToString(UserID);
                DataTable dtf = obj.AddUpdateVehicleEquipmentFee(oVehicleEquipment);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("VehicleEquipmentFee", new { RecordStatus = status });
        }

        public ActionResult GetVehicleEquipmentFee(string VehicleID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();

            VehicleEquipmentFee obj = new VehicleEquipmentFee();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                List<SelectListItem> CategoryIDs = new List<SelectListItem>();

                DataTable dt = obj.SelectAllVehicleEquipmentCategory();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    CategoryIDs.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }


                ViewBag.OpType = (VehicleID == "0" ? "Add Vehicle Equipment Fee" : "Edit Vehicle Equipment Fee");

                DataTable dtf = obj.Select_VehicleEquipmentFee(Convert.ToInt32(VehicleID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new VehicleEquipmentFee
                    {
                        VehicleID = Convert.ToInt64(dr["VehicleID"]),
                        CategoryID = Convert.ToInt64(dr["CategoryID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Fees = Convert.ToInt64(dr["Fees"]),
                        Isactive = Convert.ToInt16(dr["Isactive"]),



                        OperationType = "Edit Vehicle Equipment"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                ViewBag.CategoryIDLst = CategoryIDs;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialVehicleEquipmentFee", obj);
        }
        public ActionResult DeleteVehicleEquipmentFee(int id)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                VehicleEquipmentFee obj = new VehicleEquipmentFee();
                obj.VehicleID = id;
                obj.DeleteVehicleEquipmentFee();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("VehicleEquipmentFee");
        }

        public JsonResult CheckDuplicateForVehicleEquipmentFee(int VehicleID, int CategoryID, string NAME)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                VehicleEquipmentFee VehicleEquipments = new VehicleEquipmentFee();
                VehicleEquipments.VehicleID = VehicleID;
                VehicleEquipments.CategoryID = CategoryID;
                VehicleEquipments.Name = NAME;
                status = VehicleEquipments.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }


        #endregion

        #region "EqptSanctuariesFee"
        public ActionResult EqptSanctuariesFee(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            EqptSanctuariesFee obj = new EqptSanctuariesFee();
            try
            {
                DataTable dtf = obj.Select_EqptSanctuariesFees();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    EqptSanctuariesFeeLst.Add(
                           new EqptSanctuariesFee()
                           {
                               Index = count,
                               ID = Convert.ToInt64(dr["ID"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               ZoneName = Convert.ToString(dr["ZoneName"]),
                               CategoryName = Convert.ToString(dr["CategoryName"]),
                               Name = Convert.ToString(dr["Name"]),
                               ShiftType = Convert.ToString(dr["ShiftType"]),
                               TotalFee = Convert.ToDecimal(dr["TotalFee"]),
                               IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(EqptSanctuariesFeeLst);
        }
        public ActionResult ADDUpdateEqptSanctuariesFee(EqptSanctuariesFee oVehicleEquipment)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                EqptSanctuariesFee obj = new EqptSanctuariesFee();
                oVehicleEquipment.EnteredBy = Convert.ToString(UserID);

                DataTable dtf = obj.AddUpdateEqptSanctuariesFee(oVehicleEquipment);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("EqptSanctuariesFee", new { RecordStatus = status });
        }

        public ActionResult GetEqptSanctuariesFee(string ID)
        {

            EqptSanctuariesFee obj = new EqptSanctuariesFee();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                List<SelectListItem> CategoryIDs = new List<SelectListItem>();
                List<SelectListItem> PlaceIDs = new List<SelectListItem>();
                List<SelectListItem> ZoneIDs = new List<SelectListItem>();
                List<SelectListItem> ShiftTypes = new List<SelectListItem>();


                DataTable dt = obj.SelectAllVehicleEquipmentCategory();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    CategoryIDs.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                dt = obj.SelectAllPlaces();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    PlaceIDs.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                ViewBag.OpType = (ID == "0" ? "Add Equipment Sanctuaries Fee" : "Edit Equipment Sanctuaries Fee");

                DataTable dtf = obj.Select_EqptSanctuariesFee(Convert.ToInt32(ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new EqptSanctuariesFee
                    {
                        ID = Convert.ToInt64(dr["ID"]),
                        PlaceID = Convert.ToInt64(dr["PlaceID"]),
                        ZoneID = Convert.ToInt64(dr["ZoneID"]),
                        CategoryID = Convert.ToInt64(dr["CategoryID"]),
                        Name = Convert.ToString(dr["Name"]),
                        ShiftType = Convert.ToString(dr["ShiftType"]),
                        TotalEqptAvailability = Convert.ToInt32(dr["TotalEqptAvailability"]),
                        SeatsPerEqpt = Convert.ToInt32(dr["SeatsPerEqpt"]),
                        seatsForCitizen = Convert.ToInt32(dr["seatsForCitizen"]),
                        TotalSeats = Convert.ToInt32(dr["TotalSeats"]),
                        Fee_TigerProject = Convert.ToInt32(dr["Fee_TigerProject"]),
                        Fee_Surcharge = Convert.ToDecimal(dr["Fee_Surcharge"]),
                        TotalFee = Convert.ToDecimal(dr["TotalFee"]),
                        IsActive = Convert.ToInt16(dr["IsActive"]),


                        OperationType = "Edit Vehicle Equipment"

                    };

                }

                if (ID != "0")
                {
                    dt = obj.SelectAllZone(Convert.ToInt32(obj.PlaceID));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        ZoneIDs.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }
                }


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });


                ShiftTypes.Add(new SelectListItem { Text = "Morning", Value = "1" });
                ShiftTypes.Add(new SelectListItem { Text = "Evening ", Value = "2" });
                ShiftTypes.Add(new SelectListItem { Text = "Full Day ", Value = "3" });


                ViewBag.ISactivelst = lstISactive;
                ViewBag.CategoryIDLst = CategoryIDs;
                ViewBag.PlaceIDLst = PlaceIDs;
                ViewBag.ZoneIDLst = ZoneIDs;

                ViewBag.ShiftTypesLst = ShiftTypes;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialEqptSanctuariesFee", obj);
        }
        public ActionResult DeleteEqptSanctuariesFee(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                EqptSanctuariesFee obj = new EqptSanctuariesFee();
                obj.ID = id;
                obj.DeleteEqptSanctuariesFee();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("EqptSanctuariesFee");
        }
        public JsonResult ZoneByPlace(int PlaceID)
        {
            EqptSanctuariesFee obj = new EqptSanctuariesFee();
            List<SelectListItem> ZoneS = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                DataTable dt = new DataTable();
                dt = obj.SelectAllZone(PlaceID);
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    ZoneS.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(ZoneS, "Value", "Text"));
        }

        public JsonResult CheckDuplicateForEqptSanctuariesFee(int ID, int PlaceID, int CategoryID, string Name)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                EqptSanctuariesFee VehicleEquipments = new EqptSanctuariesFee();
                VehicleEquipments.ID = ID;
                VehicleEquipments.PlaceID = PlaceID;
                VehicleEquipments.CategoryID = CategoryID;
                VehicleEquipments.Name = Name;
                status = VehicleEquipments.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }

        #endregion

        #region "AccomodationFee"
        public ActionResult AccomodationFee(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                AccomodationFee obj = new AccomodationFee();
                DataTable dtf = obj.Select_AccomodationFees();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    AccomodationFeeLst.Add(
                      new AccomodationFee()
                      {
                          Index = count,
                          AccommodationID = Convert.ToInt64(dr["AccommodationID"]),
                          PlaceID = Convert.ToInt64(dr["PlaceID"]),
                          PlaceName = Convert.ToString(dr["PlaceName"]),
                          RoomType = Convert.ToString(dr["RoomType"]),
                          TotalRooms = Convert.ToInt32(dr["TotalRooms"]),
                          RatePerRoom = Convert.ToDecimal(dr["RatePerRoom"]),
                          IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                      });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(AccomodationFeeLst);
        }
        public ActionResult ADDUpdateAccomodationFee(AccomodationFee oVehicleEquipment)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                AccomodationFee obj = new AccomodationFee();
                oVehicleEquipment.EnteredBy = Convert.ToString(UserID);
                oVehicleEquipment.UpdatedBy = Convert.ToString(UserID);
                DataTable dtf = obj.AddUpdateAccomodationFee(oVehicleEquipment);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("AccomodationFee", new { RecordStatus = status });
        }
        public ActionResult GetAccomodationFee(string AccommodationID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            AccomodationFee obj = new AccomodationFee();


            try
            {

                List<SelectListItem> PlaceIDs = new List<SelectListItem>();



                DataTable dt = obj.SelectAllPlaces();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    PlaceIDs.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }


                ViewBag.OpType = (AccommodationID == "0" ? "Add Accommodation Fee" : "Edit Accommodation Fee");

                DataTable dtf = obj.Select_AccomodationFee(Convert.ToInt32(AccommodationID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new AccomodationFee
                    {
                        AccommodationID = Convert.ToInt64(dr["AccommodationID"]),
                        PlaceID = Convert.ToInt64(dr["PlaceID"]),

                        RoomType = Convert.ToString(dr["RoomType"]),
                        TotalRooms = Convert.ToInt32(dr["TotalRooms"]),
                        RatePerRoom = Convert.ToDecimal(dr["RatePerRoom"]),
                        IsActive = Convert.ToInt32(dr["IsActive"]),
                        OperationType = "Edit Accommodation"

                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                ViewBag.PlaceIDLst = PlaceIDs;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_partialAccomodation", obj);

        }
        public ActionResult DeleteAccomodationFee(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                EqptSanctuariesFee obj = new EqptSanctuariesFee();
                obj.ID = id;
                obj.DeleteEqptSanctuariesFee();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("AccomodationFee");
        }
        public JsonResult CheckDuplicateForAccomodationFee(int AccommodationID, int PlaceID, string RoomType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                AccomodationFee VehicleEquipments = new AccomodationFee();
                VehicleEquipments.AccommodationID = AccommodationID;
                VehicleEquipments.PlaceID = PlaceID;
                VehicleEquipments.RoomType = RoomType;

                status = VehicleEquipments.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }

        #endregion

        #region "CampFees"
        public ActionResult CampFees(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                CampFees obj = new CampFees();
                DataTable dtf = obj.Select_CampFees();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    LstCampFees.Add(
                        new CampFees()
                        {
                            Index = count,
                            CampFeesID = Convert.ToInt64(dr["CampFeesID"].ToString()),

                            DIST_NAME = Convert.ToString(dr["DIST_NAME"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            Name = Convert.ToString(dr["Name"]),
                            Amount = Convert.ToInt64(dr["Amount"]),
                            TentAmount = Convert.ToInt64(dr["TentAmount"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(LstCampFees);
        }
        public ActionResult ADDUpdateCampFees(CampFees oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string status = null;
            try
            {
                CampFees obj = new CampFees();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateCampFees(oPlace);
                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("CampFees", new { RecordStatus = status });
        }
        public ActionResult GetCampFees(string CampFeesID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            CampFees obj = new CampFees();

            try
            {
                List<SelectListItem> District = new List<SelectListItem>();
                List<SelectListItem> Places = new List<SelectListItem>();

                DataTable dt = _objLocation.District();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }

                ViewBag.OpType = (CampFeesID == "0" ? "Add Camp Fees" : "Edit Camp Fees");


                DataTable dtf = obj.Select_CampFee(Convert.ToInt32(CampFeesID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new CampFees
                    {
                        CampFeesID = Convert.ToInt64(dr["CampFeesID"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        DIST_CODE = Convert.ToInt64(dr["DIST_CODE"]),

                        Name = Convert.ToString(dr["Name"]),
                        Amount = Convert.ToInt64(dr["Amount"]),
                        TentAmount = Convert.ToInt64(dr["TentAmount"]),
                        Discount = Convert.ToInt64(dr["Discount"]),
                        TaxRate = Convert.ToInt64(dr["TaxRate"]),
                        CampAllowedPerDay = Convert.ToInt32(dr["CampAllowedPerDay"]),
                        MemberPerCamp = Convert.ToInt32(dr["MemberPerCamp"]),
                        Isactive = Convert.ToInt16(dr["Isactive"]),
                        OperationType = "Edit Camp Fees"
                    };


                }

                if (ViewBag.OpType == "Edit Camp Fees")
                {
                    dt = obj.SelectPlaceCategory(Convert.ToString(obj.DIST_CODE));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                    {
                        Places.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                    }
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlPlace1 = Places;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialCampFees", obj);
        }

        //public ActionResult DeleteCampFees(int id)
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    try
        //    {

        //        CampFees obj = new CampFees();
        //        obj.CampFeesID = id;
        //        obj.DeletePlace();
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return RedirectToAction("TicketingFees");
        //}
        public JsonResult PlaceByDistrictForCampFees(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                CampFees cst = new CampFees();
                DataTable dt = new DataTable();
                cst.DIST_CODE = Convert.ToInt64(districtID);
                dt = cst.Select_Places_ByDistrict();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(Places, "Value", "Text"));
        }

        public JsonResult CheckDuplicateForCampFees(int CampFeesID, int DIST_CODE, int PlaceID, string Name)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                CampFees VehicleEquipments = new CampFees();
                VehicleEquipments.CampFeesID = CampFeesID;
                VehicleEquipments.DIST_CODE = DIST_CODE;
                VehicleEquipments.PlaceID = PlaceID;
                VehicleEquipments.Name = Name;

                status = VehicleEquipments.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }



        #endregion

        #region "FilmShootingFees"
        public ActionResult FilmShootingFees(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                FilmShootingFees obj = new FilmShootingFees();
                DataTable dtf = obj.Select_FilmShootingFees();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    LstFilmShootingFees.Add(
                        new FilmShootingFees()
                        {
                            Index = count,
                            FilmShootingFeesID = Convert.ToInt64(dr["FilmShootingFeesID"].ToString()),
                            DIST_NAME = Convert.ToString(dr["DIST_NAME"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            IndianMemberFees = Convert.ToDecimal(dr["IndianMemberFees"]),
                            NonIndianMemberFees = Convert.ToDecimal(dr["NonIndianMemberFees"]),
                            StudentFees = Convert.ToDecimal(dr["StudentFees"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),

                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(LstFilmShootingFees);
        }
        public ActionResult ADDUpdateFilmShootingFees(FilmShootingFees oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;

            try
            {
                FilmShootingFees obj = new FilmShootingFees();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateFilmShootingFee(oPlace);
                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("FilmShootingFees", new { RecordStatus = status });
        }
        public ActionResult GetFilmShootingFees(string FilmShootingFeesID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            FilmShootingFees obj = new FilmShootingFees();

            try
            {
                List<SelectListItem> District = new List<SelectListItem>();
                List<SelectListItem> Places = new List<SelectListItem>();

                DataTable dt = _objLocation.District();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }

                ViewBag.OpType = (FilmShootingFeesID == "0" ? "Add Film Shooting Fees" : "Edit Film Shooting Fees");

                DataTable dtf = obj.Select_FilmShootingFee(Convert.ToInt32(FilmShootingFeesID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new FilmShootingFees
                    {
                        FilmShootingFeesID = Convert.ToInt64(dr["FilmShootingFeesID"].ToString()),
                        DIST_CODE = Convert.ToInt64(dr["DIST_CODE"]),
                        PlaceID = Convert.ToInt64(dr["PlaceID"]),
                        IndianMemberFees = Convert.ToDecimal(dr["IndianMemberFees"]),
                        NonIndianMemberFees = Convert.ToDecimal(dr["NonIndianMemberFees"]),
                        StudentFees = Convert.ToDecimal(dr["StudentFees"]),
                        Discount = Convert.ToDecimal(dr["Discount"]),
                        TaxRate = Convert.ToDecimal(dr["TaxRate"]),
                        Isactive = Convert.ToInt16(dr["Isactive"]),
                        OperationType = "Edit Camp Fees"

                    };


                }

                if (ViewBag.OpType == "Edit Film Shooting Fees")
                {
                    dt = obj.SelectPlaceCategory(Convert.ToString(obj.DIST_CODE));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                    {
                        Places.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                    }
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlPlace1 = Places;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialFilmShootingFees", obj);
        }

        public JsonResult PlaceByDistrictForFilmShootingFees(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                FilmShootingFees cst = new FilmShootingFees();
                DataTable dt = new DataTable();
                cst.DIST_CODE = Convert.ToInt64(districtID);
                dt = cst.Select_Places_ByDistrict();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(Places, "Value", "Text"));
        }

        public JsonResult CheckDuplicateForFilmShootingFees(int FilmShootingFeesID, int DIST_CODE, int PlaceID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                FilmShootingFees FilmShootingFeess = new FilmShootingFees();
                FilmShootingFeess.FilmShootingFeesID = FilmShootingFeesID;
                FilmShootingFeess.DIST_CODE = DIST_CODE;
                FilmShootingFeess.PlaceID = PlaceID;

                status = FilmShootingFeess.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }


        #endregion

        #region "FilmShootingSecurityDeposit"
        public ActionResult FilmShootingSecurityDeposit(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            try
            {
                FilmShootingSecurityDeposit obj = new FilmShootingSecurityDeposit();
                DataTable dtf = obj.Select_FilmShootingSecurityDepositS();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    LstFilmShootingSecurityDeposit.Add(
                        new FilmShootingSecurityDeposit()
                        {
                            Index = count,
                            FilmShootingSDID = Convert.ToInt64(dr["FilmShootingSDID"].ToString()),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            FilmCategory = Convert.ToString(dr["FilmCategory"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(LstFilmShootingSecurityDeposit);
        }
        public ActionResult ADDUpdateFilmShootingSecurityDeposit(FilmShootingSecurityDeposit oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                FilmShootingSecurityDeposit obj = new FilmShootingSecurityDeposit();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateFilmShootingSecurityDeposit(oPlace);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("FilmShootingSecurityDeposit", new { RecordStatus = status });
        }
        public ActionResult GetFilmShootingSecurityDeposit(string FilmShootingSDID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            FilmShootingSecurityDeposit obj = new FilmShootingSecurityDeposit();

            try
            {
                List<SelectListItem> Places = new List<SelectListItem>();
                ViewBag.OpType = (FilmShootingSDID == "0" ? "Add Film Shooting Security Deposit" : "Edit Film Shooting Security Deposit");

                DataTable dtf = obj.Select_FilmShootingSecurityDeposit(Convert.ToInt32(FilmShootingSDID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new FilmShootingSecurityDeposit
                    {
                        FilmShootingSDID = Convert.ToInt64(dr["FilmShootingSDID"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"]),
                        FilmCategory = Convert.ToString(dr["FilmCategory"]),
                        Amount = Convert.ToDecimal(dr["Amount"]),
                        Isactive = Convert.ToInt16(dr["Isactive"]),
                        OperationType = "Edit Film Shooting Security Deposit"

                    };

                }

                DataTable dts = obj.SelectPlaceCategory();
                ViewBag.fname = dts;
                foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                }


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                ViewBag.ddlPlace1 = Places;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialFilmShootingSecurityDeposit", obj);
        }


        public ActionResult CheckDuplicateForFilmShootingSecurityDeposit(int FilmShootingSDID, int PlaceID, string FilmCategory)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool Status = false;
            try
            {
                FilmShootingSecurityDeposit FilmShootingFeess = new FilmShootingSecurityDeposit();
                FilmShootingFeess.FilmShootingSDID = FilmShootingSDID;
                FilmShootingFeess.PlaceID = PlaceID;
                FilmShootingFeess.FilmCategory = FilmCategory;

                Status = FilmShootingFeess.Check_DuplicateRecord();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(Status, JsonRequestBehavior.AllowGet);
        }


        #endregion




        #region "ZONE"
        public ActionResult Zone(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            try
            {
                Zone obj = new Zone();
                DataTable dtf = obj.Select_ZoneS();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    LstZone.Add(
                        new Zone()
                        {
                            Index = count,
                            zoneID = Convert.ToInt32(dr["zoneID"].ToString()),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            ZoneName = Convert.ToString(dr["ZoneName"]),
                            TicketAllocatedPerShift = Convert.ToInt32(dr["TicketAllocatedPerShift"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                            isMorningView = Convert.ToBoolean(dr["isMorning"]),
                            isEveningView = Convert.ToBoolean(dr["isEvening"]),
                            isFullDayView = Convert.ToBoolean(dr["isFullDay"]),

                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(LstZone);
        }
        public ActionResult ADDUpdateZone(Zone oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                Zone obj = new Zone();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateZone(oPlace);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Zone", new { RecordStatus = status });
        }
        public ActionResult GetZone(string zoneID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Zone obj = new Zone();

            try
            {
                List<SelectListItem> Places = new List<SelectListItem>();
                ViewBag.OpType = (zoneID == "0" ? "Add Zone" : "Edit Zone");

                DataTable dtf = obj.Select_Zone(Convert.ToInt32(zoneID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Zone
                    {

                        zoneID = Convert.ToInt32(dr["zoneID"].ToString()),
                        PlaceID = Convert.ToInt32(dr["PlaceID"]),
                        ZoneName = Convert.ToString(dr["ZoneName"]),
                        TicketAllocatedPerShift = Convert.ToInt32(dr["TicketAllocatedPerShift"]),
                        Isactive = Convert.ToInt32(dr["Isactive"]),
                        isMorning = Convert.ToInt32(dr["isMorning"]),
                        isEvening = Convert.ToInt32(dr["isEvening"]),
                        isFullDay = Convert.ToInt32(dr["isFullDay"]),

                        isDptKiosk = Convert.ToInt32(dr["isDptKiosk"]),
                        isCitizen = Convert.ToInt32(dr["isCitizen"]),

                        OperationType = "Edit Zone"

                    };

                }

                DataTable dts = obj.SelectPlaces();
                ViewBag.fname = dts;
                foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                }


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                lstShiftType.Add(new SelectListItem { Text = "Morning / Evening", Value = "0" });
                lstShiftType.Add(new SelectListItem { Text = "Full Day", Value = "1" });


                lstSafariAvailable.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstSafariAvailable.Add(new SelectListItem { Text = "No", Value = "0" });


                ViewBag.SafariAvailablelst = lstSafariAvailable;
                ViewBag.ShiftType = lstShiftType;
                ViewBag.ISactivelst = lstISactive;
                ViewBag.ddlPlace1 = Places;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialZone", obj);
        }


        #endregion

        #region "PlaceBookingDuration"
        public ActionResult PlaceBookingDurations(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                PlaceBookingDuration obj = new PlaceBookingDuration();
                DataTable dtf = obj.Select_PlaceBookingDurations();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    LstPlaceBookingDuration.Add(
                        new PlaceBookingDuration()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"].ToString()),
                            PlaceID = Convert.ToInt32(dr["PlaceID"]),
                            ZoneID = Convert.ToInt32(dr["ZoneID"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            //  ZoneName = Convert.ToString(dr["ZoneName"]),
                            BookingTypeName = Convert.ToString(dr["BookingTypeName"]),
                            DurationFromDate = Convert.ToString(dr["DurationFromDate"]),
                            DurationToDate = Convert.ToString(dr["DurationToDate"]),
                            TicketDurationFromDate = Convert.ToString(dr["TicketDurationFromDate"]),
                            TicketDurationToDate = Convert.ToString(dr["TicketDurationToDate"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                            Isactive = Convert.ToInt32(dr["Isactive"]),
                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(LstPlaceBookingDuration);
        }
        public ActionResult ADDUpdatePlaceBookingDuration(PlaceBookingDuration oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string status = null;
            try
            {
                PlaceBookingDuration obj = new PlaceBookingDuration();
                DataTable dtf = obj.AddUpdatePlaceBookingDuration(oPlace);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("PlaceBookingDurations", new { RecordStatus = status });
        }
        public ActionResult GetPlaceBookingDuration(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            PlaceBookingDuration obj = new PlaceBookingDuration();

            try
            {
                List<SelectListItem> Places = new List<SelectListItem>();

                List<SelectListItem> LstBookingTypeName = new List<SelectListItem>();

                List<SelectListItem> Zones = new List<SelectListItem>();

                DataTable dt = obj.GETPlace();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

                ViewBag.OpType = (ID == "0" ? "Add Place Booking Duration" : "Edit Place Booking Duration");


                DataTable dtf = obj.Select_PlaceBookingDuration(Convert.ToInt32(ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new PlaceBookingDuration
                    {
                        ID = Convert.ToInt32(dr["ID"].ToString()),
                        PlaceID = Convert.ToInt32(dr["PlaceID"]),
                        ZoneID = Convert.ToInt32(dr["ZoneID"]),
                        PlaceName = Convert.ToString(dr["PlaceName"]),
                        // ZoneName = Convert.ToString(dr["ZoneName"]),
                        BookingTypeName = Convert.ToString(dr["BookingTypeName"]),
                        DurationFromDate = Convert.ToString(dr["DurationFromDate"]),
                        DurationToDate = Convert.ToString(dr["DurationToDate"]),
                        TicketDurationFromDate = Convert.ToString(dr["TicketDurationFromDate"]),
                        TicketDurationToDate = Convert.ToString(dr["TicketDurationToDate"]),
                        IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        Isactive = Convert.ToInt32(dr["Isactive"]),
                        OperationType = "Edit"
                    };


                }

                if (ViewBag.OpType == "Edit Place Booking Duration")
                {
                    dt = obj.GETZone(obj.PlaceID.ToString());
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                    {
                        Zones.Add(new SelectListItem { Text = @dr1["ZoneName"].ToString(), Value = @dr1["ZoneID"].ToString() });
                    }
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                // LstBookingTypeName.Add(new SelectListItem { Text = "Both", Value = "Both" });
                LstBookingTypeName.Add(new SelectListItem { Text = "Online", Value = "Online" });
                LstBookingTypeName.Add(new SelectListItem { Text = "Current ", Value = "Current" });


                ViewBag.ISactivelst = lstISactive;
                ViewBag.ddlPlace1 = Places;
                ViewBag.ddlZones1 = Zones;
                ViewBag.ddlLstBookingTypeName1 = LstBookingTypeName;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_PlaceBookingDurations", obj);
        }
        public JsonResult ZoneByPlaceBookingDuration(int PlaceID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                PlaceBookingDuration cst = new PlaceBookingDuration();
                DataTable dt = new DataTable();

                dt = cst.GETZone(Convert.ToString(PlaceID));

                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr1["ZoneName"].ToString(), Value = @dr1["ZoneID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(Places, "Value", "Text"));
        }

        #endregion

        #region "ResearchPlantsAnimals"
        public ActionResult ResearchPlantsAnimals(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ResearchPlantsAnimals obj = new ResearchPlantsAnimals();
                DataTable dtf = obj.Select_ResearchPlantsAnimalsS();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstResearchPlantsAnimals.Add(
                        new ResearchPlantsAnimals()
                        {
                            Index = count,
                            SpecieId = Convert.ToInt64(dr["SpecieId"].ToString()),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            PlaceCat = Convert.ToString(dr["PlaceCat"]),
                            SpecieName = Convert.ToString(dr["SpecieName"]),
                            SpecieCat = Convert.ToString(dr["SpecieCat"]),
                            SpecieType = Convert.ToString(dr["SpecieType"]),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),
                        });
                    count += 1;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(lstResearchPlantsAnimals);
        }
        public ActionResult ADDUpdateResearchPlantsAnimals(ResearchPlantsAnimals oResearchPlantsAnimals)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                ResearchPlantsAnimals obj = new ResearchPlantsAnimals();
                oResearchPlantsAnimals.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateResearchPlantsAnimals(oResearchPlantsAnimals);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ResearchPlantsAnimals", new { RecordStatus = status });
        }
        public ActionResult GetResearchPlantsAnimals(string SpecieId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ResearchPlantsAnimals obj = new ResearchPlantsAnimals();
            List<SelectListItem> Places = new List<SelectListItem>();
            try
            {

                DataTable dt = obj.GETPlaceForResearchPlantsAnimals();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

                ViewBag.OpType = (SpecieId == "0" ? "Add Research Plants And Animals" : "Edit Research Plants And Animals");

                Places objs = new Places();

                dt = objs.SelectPlaceCategory();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Category.Add(new SelectListItem { Text = @dr["Category"].ToString(), Value = @dr["Category"].ToString() });
                }


                DataTable dtf = obj.Select_ResearchPlantsAnimals(Convert.ToInt64(SpecieId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ResearchPlantsAnimals
                    {
                        SpecieId = Convert.ToInt64(dr["SpecieId"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),

                        PlaceCat = Convert.ToString(dr["PlaceCat"]),
                        SpecieName = Convert.ToString(dr["SpecieName"]),
                        SpecieCat = Convert.ToString(dr["SpecieCat"]),
                        SpecieType = Convert.ToString(dr["SpecieType"]),
                        Isactive = Convert.ToInt16(dr["isActive"]),

                        OperationType = "Edit Research Plants And Animals"
                    };
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                ViewBag.ddlPlace1 = Places;
                ViewBag.Category1 = Category;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_ResearchPlantsAnimals", obj);
        }

        #endregion

        #region "CoordinatorRegistration"
        public ActionResult CoordinatorRegistration(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                CoordinatorRegistration obj = new CoordinatorRegistration();
                DataTable dtf = obj.Select_CoordinatorRegistrationS();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstCoordinatorRegistration.Add(
                        new CoordinatorRegistration()
                        {
                            Index = count,
                            ID = Convert.ToInt64(dr["ID"].ToString()),
                            DIST_NAME = Convert.ToString(dr["DIST_NAME"]),
                            Name = Convert.ToString(dr["Name"]),
                            SSOID = Convert.ToString(dr["SSOID"]),
                            Address = Convert.ToString(dr["Address"]),
                            Pincode = Convert.ToString(dr["Pincode"]),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(lstCoordinatorRegistration);
        }
        public ActionResult ADDUpdateCoordinatorRegistration(CoordinatorRegistration oCoordinatorRegistration)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                CoordinatorRegistration obj = new CoordinatorRegistration();
                oCoordinatorRegistration.UpdatedBy = UserID;
                oCoordinatorRegistration.EnteredBy = UserID;
                DataTable dtf = obj.AddUpdateCoordinatorRegistration(oCoordinatorRegistration);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("CoordinatorRegistration", new { RecordStatus = status });
        }
        public ActionResult GetCoordinatorRegistration(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            CoordinatorRegistration obj = new CoordinatorRegistration();
            List<SelectListItem> District = new List<SelectListItem>();

            try
            {

                DataTable dt = obj.GETDistrictsForCoordinatorRegistration();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }

                ViewBag.OpType = (ID == "0" ? "Add Coordinator Registration" : "Edit Coordinator Registration");


                DataTable dtf = obj.Select_CoordinatorRegistration(Convert.ToInt64(ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new CoordinatorRegistration
                    {

                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        DIST_CODE = Convert.ToInt32(dr["DIST_CODE"]),
                        Name = Convert.ToString(dr["Name"]),
                        SSOID = Convert.ToString(dr["SSOID"]),
                        Address = Convert.ToString(dr["Address"]),
                        Pincode = Convert.ToString(dr["Pincode"]),
                        Isactive = Convert.ToInt16(dr["isActive"]),
                        OperationType = "Edit Coordinator Registration"

                    };
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                ViewBag.ddlDistrict1 = District;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_CoordinatorReistration", obj);
        }

        #endregion

        #region "ContactUs"
        public ActionResult ContactUs(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> ContactUs = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ContactUs obj = new ContactUs();
            try
            {

                DataTable dtf = obj.Select_ContactUs();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ContactLst.Add(
                        new ContactUs()
                        {
                            Index = count,
                            ContactUsId = Convert.ToInt32(dr["ContactUsId"]),
                            Heading = Convert.ToString(dr["Heading"].ToString()),
                            TextString = Convert.ToString(dr["TextString"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ContactLst);
        }
        public ActionResult AddUpdateContactUs(ContactUs oContactUs)
        {
            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ContactUs obj = new ContactUs();

                DataTable dtf = obj.AddUpdateContactUs(oContactUs);
                //oTicker.LastUpdatedBy = UserID;
                //status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ContactUs", new { RecordStatus = status });
        }
        public ActionResult GetContactUs(string ContactUsId)
        {

            ContactUs obj = new ContactUs();
            List<SelectListItem> ContactUs = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (ContactUsId == "0" ? "Add ContactUs" : "Edit ContactUs");


                DataTable dtf = obj.Select_Contact(Convert.ToInt32(ContactUsId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ContactUs
                    {
                        ContactUsId = Convert.ToInt64(dr["ContactUsId"].ToString()),
                        Heading = Convert.ToString(dr["Heading"].ToString()),
                        TextString = Convert.ToString(dr["TextString"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"]),



                        OperationType = "Edit ContactUs"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialContactUs", obj);
        }


        #endregion

        #region "Navigation"
        public ActionResult Navigation(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Navigation = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Navigation obj = new Navigation();
            try
            {

                DataTable dtf = obj.Select_Navigations();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    NavigationLst.Add(
                        new Navigation()
                        {
                            Index = count,
                            NavigationID = Convert.ToInt32(dr["NavigationID"]),
                            Link = Convert.ToString(dr["Link"].ToString()),
                            TextString = Convert.ToString(dr["TextString"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(NavigationLst);
        }
        public ActionResult ADDUpdateNavigation(Navigation oNavigation)
        {
            List<SelectListItem> Navigation = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Navigation obj = new Navigation();

                DataTable dtf = obj.AddUpdateNavigation(oNavigation);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Navigation", new { RecordStatus = status });
        }
        public ActionResult GetNavigation(string NavigationID)
        {

            Navigation obj = new Navigation();
            List<SelectListItem> Navigation = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (NavigationID == "0" ? "Add Navigation" : "Edit Navigation");


                DataTable dtf = obj.Select_Navigation(Convert.ToInt32(NavigationID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Navigation
                    {
                        NavigationID = Convert.ToInt32(dr["NavigationID"].ToString()),
                        Link = Convert.ToString(dr["Link"].ToString()),
                        TextString = Convert.ToString(dr["TextString"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),


                        OperationType = "Edit Navigation"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialNavigation", obj);
        }


        #endregion

        #region "ImportLink"
        public ActionResult ImportLink(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> ImportLink = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ImportLink obj = new ImportLink();
            try
            {

                DataTable dtf = obj.Select_ImportLinks();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ImportLinkLst.Add(
                        new ImportLink()
                        {
                            Index = count,
                            ImportID = Convert.ToInt32(dr["ImportID"]),
                            Link = Convert.ToString(dr["Link"].ToString()),
                            TextString = Convert.ToString(dr["TextString"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ImportLinkLst);
        }
        public ActionResult ADDUpdateImportLink(ImportLink oImportLink)
        {
            List<SelectListItem> ImportLink = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ImportLink obj = new ImportLink();

                DataTable dtf = obj.ADDUpdateImportLink(oImportLink);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ImportLink", new { RecordStatus = status });
        }
        public ActionResult GetImportLink(string ImportID)
        {

            ImportLink obj = new ImportLink();
            List<SelectListItem> ImportLink = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (ImportID == "0" ? "Add Import" : "Edit Import");


                DataTable dtf = obj.Select_ImportLink(Convert.ToInt32(ImportID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ImportLink
                    {
                        ImportID = Convert.ToInt32(dr["ImportID"].ToString()),
                        Link = Convert.ToString(dr["Link"].ToString()),
                        TextString = Convert.ToString(dr["TextString"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"]),


                        OperationType = "Edit ImportLink"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialImportLink", obj);
        }


        #endregion

        #region "ForestVillageType"
        public ActionResult ForestVillageType(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ForestVillageType obj = new ForestVillageType();
                DataTable dtf = obj.Select_ForestVillageTypes();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstForestVillageType.Add(
                        new ForestVillageType()
                        {
                            Index = count,
                            FTypeID = Convert.ToInt32(dr["FTypeID"].ToString()),
                            Forest_Type = Convert.ToString(dr["Forest_Type"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstForestVillageType);
        }

        public ActionResult ADDUpdateForestVillageType(ForestVillageType oForestVillageType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ForestVillageType obj = new ForestVillageType();


                oForestVillageType.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateForestVillageType(oForestVillageType);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ForestVillageType", new { RecordStatus = status });
        }
        public ActionResult GetForestVillageType(string FTypeID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ForestVillageType obj = new ForestVillageType();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_ForestVillageType(Convert.ToInt16(FTypeID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ForestVillageType
                    {
                        FTypeID = Convert.ToInt32(dr["FTypeID"].ToString()),
                        Forest_Type = Convert.ToString(dr["Forest_Type"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit ForestVillageType"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialForestVillageType", obj);
        }





        #endregion

        #region "ForestProtectionAct"
        public ActionResult ForestProtectionAct(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ForestProtectionAct obj = new ForestProtectionAct();
                DataTable dtf = obj.Select_ForestProtectionActs();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstForestProtectionAct.Add(
                        new ForestProtectionAct()
                        {
                            Index = count,
                            FProtectionActID = Convert.ToInt32(dr["FProtectionActID"].ToString()),
                            Forest_Protection_Act = Convert.ToString(dr["Forest_Protection_Act"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstForestProtectionAct);
        }

        public ActionResult ADDUpdateForestProtectionAct(ForestProtectionAct oForestProtectionAct)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ForestProtectionAct obj = new ForestProtectionAct();


                oForestProtectionAct.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateForestProtectionAct(oForestProtectionAct);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ForestProtectionAct", new { RecordStatus = status });
        }
        public ActionResult GetForestProtectionAct(string FProtectionActID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ForestProtectionAct obj = new ForestProtectionAct();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_ForestProtectionAct(Convert.ToInt16(FProtectionActID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ForestProtectionAct
                    {
                        FProtectionActID = Convert.ToInt32(dr["FProtectionActID"].ToString()),
                        Forest_Protection_Act = Convert.ToString(dr["Forest_Protection_Act"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit ForestProtectionAct"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialForestProtectionAct", obj);
        }





        #endregion

        #region "WildlifeProtectionAct"
        public ActionResult WildlifeProtectionAct(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                WildlifeProtectionAct obj = new WildlifeProtectionAct();
                DataTable dtf = obj.Select_WildlifeProtectionActs();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstWildlifeProtectionAct.Add(
                        new WildlifeProtectionAct()
                        {
                            Index = count,
                            WProtectionActID = Convert.ToInt32(dr["WProtectionActID"].ToString()),
                            Wildlife_Protection_Act = Convert.ToString(dr["Wildlife_Protection_Act"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstWildlifeProtectionAct);
        }

        public ActionResult ADDUpdateWildlifeProtectionAct(WildlifeProtectionAct oWildlifeProtectionAct)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                WildlifeProtectionAct obj = new WildlifeProtectionAct();


                oWildlifeProtectionAct.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateWildlifeProtectionAct(oWildlifeProtectionAct);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("WildlifeProtectionAct", new { RecordStatus = status });
        }
        public ActionResult GetWildlifeProtectionAct(string WProtectionActID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            WildlifeProtectionAct obj = new WildlifeProtectionAct();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_WildlifeProtectionAct(Convert.ToInt16(WProtectionActID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new WildlifeProtectionAct
                    {
                        WProtectionActID = Convert.ToInt32(dr["WProtectionActID"].ToString()),
                        Wildlife_Protection_Act = Convert.ToString(dr["Wildlife_Protection_Act"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit WildlifeProtectionAct"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialWildlifeProtectionAct", obj);
        }





        #endregion

        #region "ForestCategory"
        public ActionResult ForestCategory(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ForestCategory obj = new ForestCategory();
                DataTable dtf = obj.Select_ForestCategorys();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstForestCategory.Add(
                        new ForestCategory()
                        {
                            Index = count,
                            FOCatID = Convert.ToInt32(dr["FOCatID"].ToString()),
                            FOCategory = Convert.ToString(dr["FOCategory"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstForestCategory);
        }

        public ActionResult ADDUpdateForestCategory(ForestCategory oForestCategory)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ForestCategory obj = new ForestCategory();


                oForestCategory.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateForestCategory(oForestCategory);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ForestCategory", new { RecordStatus = status });
        }
        public ActionResult GetForestCategory(string FOCatID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ForestCategory obj = new ForestCategory();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_ForestCategory(Convert.ToInt16(FOCatID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ForestCategory
                    {
                        FOCatID = Convert.ToInt32(dr["FOCatID"].ToString()),
                        FOCategory = Convert.ToString(dr["FOCategory"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit ForestCategory"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialForestCategory", obj);
        }





        #endregion

        #region "DecisionTaken"
        public ActionResult DecisionTaken(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                DecisionTaken obj = new DecisionTaken();
                DataTable dtf = obj.Select_DecisionTakens();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstDecisionTaken.Add(
                        new DecisionTaken()
                        {
                            Index = count,
                            DID = Convert.ToInt32(dr["DID"].ToString()),
                            DName = Convert.ToString(dr["DName"]),
                            DecisionDefinition = Convert.ToString(dr["DecisionDefinition"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstDecisionTaken);
        }

        public ActionResult ADDUpdateDecisionTaken(DecisionTaken oDecisionTaken)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                DecisionTaken obj = new DecisionTaken();


                oDecisionTaken.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateDecisionTaken(oDecisionTaken);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("DecisionTaken", new { RecordStatus = status });
        }
        public ActionResult GetDecisionTaken(string DID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            DecisionTaken obj = new DecisionTaken();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_DecisionTaken(Convert.ToInt16(DID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new DecisionTaken
                    {
                        DID = Convert.ToInt32(dr["DID"].ToString()),
                        DName = Convert.ToString(dr["DName"]),
                        DecisionDefinition = Convert.ToString(dr["DecisionDefinition"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit DecisionTaken"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialDecisionTaken", obj);
        }





        #endregion

        #region "ReasonCaseFailed"
        public ActionResult ReasonCaseFailed(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ReasonCaseFailed obj = new ReasonCaseFailed();
                DataTable dtf = obj.Select_ReasonCaseFaileds();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstReasonCaseFailed.Add(
                        new ReasonCaseFailed()
                        {
                            Index = count,
                            RID = Convert.ToInt32(dr["RID"].ToString()),
                            RName = Convert.ToString(dr["RName"]),
                            ReasonDefinition = Convert.ToString(dr["ReasonDefinition"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstReasonCaseFailed);
        }

        public ActionResult ADDUpdateReasonCaseFailed(ReasonCaseFailed oReasonCaseFailed)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ReasonCaseFailed obj = new ReasonCaseFailed();


                oReasonCaseFailed.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateReasonCaseFailed(oReasonCaseFailed);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ReasonCaseFailed", new { RecordStatus = status });
        }
        public ActionResult GetReasonCaseFailed(string RID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ReasonCaseFailed obj = new ReasonCaseFailed();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_ReasonCaseFailed(Convert.ToInt16(RID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ReasonCaseFailed
                    {
                        RID = Convert.ToInt32(dr["RID"].ToString()),
                        RName = Convert.ToString(dr["RName"]),
                        ReasonDefinition = Convert.ToString(dr["ReasonDefinition"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit ReasonCaseFailed"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialReasonCaseFailed", obj);
        }





        #endregion






        #region "ZooHeaderFooter"
        public ActionResult ZooHeaderFooter(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooHeaderFooter obj = new ZooHeaderFooter();
            try
            {
                DataTable dtf = obj.Select_ZooHeaderFooters();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZooHeaderFooterLst.Add(
                           new ZooHeaderFooter()
                           {
                               Index = count,
                               Id = Convert.ToInt64(dr["Id"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               HeadeText = Convert.ToString(dr["HeadeText"]),
                               FooterText = Convert.ToString(dr["FooterText"]),
                               IsactiveView = Convert.ToBoolean(dr["isActive"]),
                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ZooHeaderFooterLst);
        }
        public ActionResult ADDUpdateZooHeaderFooter(ZooHeaderFooter oZooHeaderFooter)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                ZooHeaderFooter obj = new ZooHeaderFooter();


                DataTable dtf = obj.AddUpdateZooHeaderFooter(oZooHeaderFooter);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ZooHeaderFooter", new { RecordStatus = status });
        }

        public ActionResult GetZooHeaderFooter(string Id)
        {
            ZooHeaderFooter obj = new ZooHeaderFooter();
            List<SelectListItem> ZooHeaderFooter = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (Id == "0" ? "Add Header Footer" : "Edit Header Footer");


                DataTable dtf = obj.Select_ZooHeaderFooter(Convert.ToInt16(Id));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ZooHeaderFooter
                    {
                        Id = Convert.ToInt64(dr["Id"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        HeadeText = Convert.ToString(dr["HeadeText"].ToString()),
                        FooterText = Convert.ToString(dr["FooterText"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit ZooHeaderFooter"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                DataTable dtf3 = obj.PlaceName1();

                foreach (DataRow dr1 in dtf3.Rows)
                {
                    LSTPlaceName.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                }

                ViewBag.ddlLSTPlaceName = LSTPlaceName;



            }


            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_partialZooHeaderFooter", obj);
        }



        #endregion

        public JsonResult UpdateMasterRecordStatus(Int64 ID, bool STATUS, string MasterType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = "0";
            try
            {
                Places OBJ = new Places();

                OBJ.UpdateMasterRecordStatus(ID, STATUS, MasterType);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ImportData(HttpPostedFileBase file, FormCollection fc, string name)
        {
            string str = "", tablename = "";
            string table = name;
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {

                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create fConnection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    //  int t = 0;
                    //excel data saves in temp file here.
                    /*  foreach (DataRow row in dt.Rows)
                      {
                          excelSheets[t] = row["TABLE_NAME"].ToString();
                          t++;
                      }*/
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [Sheet1$]");//, excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }
                if (name == "Place")
                {
                    Places pl = new Places();
                    pl.Placeauto();
                }
                else if (name == "TicketingFee")
                {
                    TicketingFee pl1 = new TicketingFee();
                    pl1.Ticketauto();
                }

                else if (name == "Region")
                {
                    Region pl2 = new Region();
                    pl2.regionauto();
                }


                MasterData ob = new MasterData();
                DataTable did = new DataTable();
                did = ob.GetTableID(name);
                string id = did.Rows[0][0].ToString();

                //remove
                /*if (Convert.ToInt64(count) > ds.Tables[0].Rows.Count)
                 {
                     ViewBag.OpType = "Current data is less than previous data";
                 }
                 else
                 {*/
                ob = new MasterData();
                // ob.TruncateMasterData(id);
                ob.ImportMasterData(id, ds.Tables[0]);
                ViewBag.OpType = "Data Imported Successfully";
                //}
                if (name == "Place")
                {
                    str = "Places";
                    tablename = "tbl_mst_Places";
                }
                else if (name == "TicketingFee")
                {
                    str = "TicketingFee";
                    tablename = "tbl_mst_TicketingFees";
                }
                else if (name == "Region")
                {
                    str = "Region";
                    tablename = "tbl_mst_ForestRegions";
                }
                else if (name == "wcircles")
                {
                    str = "wcircles";
                    tablename = "tbl_mst_Forest_WildLifeCircles";
                }

                did = ob.getcount(tablename);
                string count = did.Rows[0][0].ToString();
                ob.updatecount(id, count);
            }


            return RedirectToAction(str, new { iid = ViewBag.OpType });
        }

        private void AddMasterData(DataSet ds)
        { }
        private void AddMasterPlaceData(DataSet ds)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Places objPlace = new Places
                {
                    PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                    PlaceName = Convert.ToString(dr["PlaceName"]),
                    Category = Convert.ToString(dr["Category"]),
                    MorningShiftFrom = Convert.ToString(dr["MorningShiftFrom"]),
                    MorningShiftTo = Convert.ToString(dr["MorningShiftTo"]),
                    EveningShiftFrom = Convert.ToString(dr["EveningShiftFrom"]),
                    EveningShiftTo = Convert.ToString(dr["EveningShiftTo"]),
                    FullDayShift = Convert.ToString(dr["FullDayShift"]),
                    DIST_CODE = Convert.ToString(dr["DIST_CODE"]),
                    TicketAllocatedPerShift = Convert.ToInt32(Convert.ToString(dr["TicketAllocatedPerShift"])),

                    IsAccommodation = Convert.ToString(dr["IsAccommodation"]),

                    SingleOccupancy = Convert.ToString(dr["SingleOccupancy"]),

                    DoubleOccupancy = Convert.ToString(dr["DoubleOccupancy"]),

                    IsSafari = Convert.ToInt32(dr["IsSafari"]),

                    SafariAvailability = Convert.ToString(dr["SafariAvailability"]),
                    EnteredBy = 2,
                    UpdatedBy = 2
                };
                objPlace.AddUpdatePlace(objPlace);
            }
        }

        private void AddMasterSpecialAnimalData(DataSet ds)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SpeciesAnimal objSA = new SpeciesAnimal
                {
                    ID = Convert.ToInt64(dr["ID"].ToString()),
                    Category = Convert.ToString(dr["Category"]),
                    AnimalCatId = Convert.ToInt64(dr["AnimalCatId"]),
                    Name = Convert.ToString(dr["Name"]),
                    Sno_Species_Animal = Convert.ToString(dr["Sno_Species_Animal"]),
                    Cat_Description = Convert.ToString(dr["Cat_Description"]),
                    Status = Convert.ToString(dr["Status"]),
                    IsActive = Convert.ToBoolean(dr["IsActive"]),

                    EnteredBy = 2,
                    UpdatedBy = 2
                };
                objSA.AddUpdateSpeciesAnimals(objSA);
            }
        }

        public ActionResult ExportData(string name)
        {
            MasterData ob = new MasterData();
            DataTable dtf = new DataTable();
            dtf = ob.GetTableID(name);
            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + name + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;
        }


        public ActionResult Region()
        {

            Region obj = new Region();
            DataTable dtf = obj.Select_Regions();

            int count = 1;
            foreach (DataRow dr in dtf.Rows)
            {
                region.Add(
                     new Region()
                     {
                         Index = count,
                         ROWID = Convert.ToInt32(dr["ROWID"].ToString()),
                         REG_CODE = Convert.ToString(dr["REG_CODE"]),
                         REG_NAME = Convert.ToString(dr["REG_NAME"]),
                         REG_HNAME = Convert.ToString(dr["REG_HNAME"]),
                         AREA_SQKM = Convert.ToDecimal(dr["AREA_SQKM"])
                     });
                count += 1;
            }
            return View("Region", region);
        }

        public ActionResult ADDUpdateRegion(Region oPlace)
        {
            Region obj = new Region();
            obj.regionauto();
            DataTable dtf = obj.AddUpdateRegion(oPlace);
            return RedirectToAction("Region");
        }

        public ActionResult GetRegion(string ROWID)
        {
            // List<SelectListItem> District = new List<SelectListItem>();

            List<SelectListItem> items = new List<SelectListItem>();


            ViewBag.OpType = (ROWID == "0" ? "Add Region" : "Edit Region");
            Region obj = new Region();
            DataTable dtf = obj.Select_Region(Convert.ToInt32(ROWID));

            foreach (DataRow dr in dtf.Rows)
            {
                obj = new Region
                {
                    ROWID = Convert.ToInt32(dr["ROWID"].ToString()),
                    REG_CODE = Convert.ToString(dr["REG_CODE"]),
                    REG_NAME = Convert.ToString(dr["REG_NAME"]),
                    REG_HNAME = Convert.ToString(dr["REG_HNAME"]),
                    AREA_SQKM = Convert.ToDecimal(Convert.ToString(dr["AREA_SQKM"]))
                };

            }


            return PartialView("_partialregion", obj);
        }

        public ActionResult Deleteregion(int id)
        {
            Region obj = new Region();
            obj.ROWID = id;
            //obj.Deleteregion();
            return RedirectToAction("Region");
        }
        public ActionResult wcircles()
        {
            wcircles obj = new wcircles();
            DataTable dtf = obj.Select_Circle();
            int count = 1;
            foreach (DataRow dr in dtf.Rows)
            {
                circle.Add(
                    new wcircles()
                    {
                        Index = count,
                        ROWID = Convert.ToInt64(dr["ROWID"].ToString()),
                        REG_NAME = Convert.ToString(dr["REG_NAME"]),
                        CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]),
                        CIRCLE_NAME = Convert.ToString(dr["CIRCLE_NAME"]),
                        CIRCLE_HNAME = Convert.ToString(dr["CIRCLE_HNAME"]),
                        AREA_SQKM = Convert.ToDecimal(dr["AREA_SQKM"])
                    });
                count += 1;
            }
            return View("wcircles", circle);
        }
        public ActionResult ADDUpdatewcircles(wcircles oPlace)
        {
            wcircles obj = new wcircles();
            //obj.Placeauto();
            DataTable dtf = obj.AddUpdatePlace(oPlace);
            return RedirectToAction("wcircles");
        }

        public ActionResult Getwcircles(string ROWID)
        {
            List<SelectListItem> Region = new List<SelectListItem>();

            List<SelectListItem> items = new List<SelectListItem>();

            DataTable dt = _objLocation.BindRegion();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                Region.Add(new SelectListItem { Text = @dr["REG_NAME"].ToString(), Value = @dr["REG_CODE"].ToString() });
            }
            ViewBag.OpType = (ROWID == "0" ? "Add Circle" : "Edit Circle");


            wcircles obj = new wcircles();

            DataTable dtf = obj.Select_Place(Convert.ToInt32(ROWID));

            foreach (DataRow dr in dtf.Rows)
            {
                obj = new wcircles
                {
                    ROWID = Convert.ToInt64(dr["ROWID"].ToString()),
                    REG_CODE = Convert.ToString(dr["REG_CODE"]),
                    CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]),
                    CIRCLE_NAME = Convert.ToString(dr["CIRCLE_NAME"]),
                    CIRCLE_HNAME = Convert.ToString(dr["CIRCLE_HNAME"]),
                    AREA_SQKM = Convert.ToDecimal(dr["AREA_SQKM"]),
                    ISWILDLIFECIRCLE = Convert.ToInt16(dr["ISWILDLIFECIRCLE"]),

                    OperationType = "Edit Place"
                };

            }

            ViewBag.ddlDistrict1 = Region;


            return PartialView("_partialwcircles", obj);
        }

        /* public ActionResult DeletePlaces(int id)
         {
             Places obj = new Places();
             obj.PlaceID = id;
             obj.DeletePlace();
             return RedirectToAction("Places");
         }*/

        /* public ActionResult MasterData(string iid)
        {
            DataTable dt = _obj.GetImportExportTables();
            ViewBag.fname = dt;
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                lstImportExport.Add(new SelectListItem { Text = @dr["TableName"].ToString(), Value = @dr["ID"].ToString() + "#" + @dr["TableName"].ToString() + "#" + @dr["CurrentRecordCount"].ToString() });
            }

            ViewBag.ddlTableName1 = lstImportExport;
            ViewBag.OpType = iid;
            return View();
        }*/

        #region "NurseryFDMProduct"
        public ActionResult NurseryFDMProduct(bool? RecordStatus, string type)
        {
            ViewBag.ProduceFor = type;
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            NurseryFDMProduct obj = new NurseryFDMProduct();
            try
            {
                DataTable dtf = obj.Select_NurseryFDMProducts(Encryption.decrypt(type));
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    NurseryFDMProductLst.Add(
                           new NurseryFDMProduct()
                           {
                               Index = count,
                               ID = Convert.ToInt64(dr["ID"]),
                              // ProductFullImage = Convert.ToString(dr["ProductFullImage"]),
                              // ProductThumbImage = Convert.ToString(dr["ProductThumbImage"]),
                               ProductName = Convert.ToString(dr["ProductName"]),
                               ProduceType = Convert.ToString(dr["ProduceType"]),
                               Unit = Convert.ToString(dr["Unit"]),
                               IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                               DiscountNGO = Convert.ToDecimal(dr["Price"]),

                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(NurseryFDMProductLst);
        }
        public ActionResult ADDUpdateNurseryFDMProduct(List<MapNurserieHeadPrice> LstHeadDetails, NurseryFDMProduct oNurseryFDMProduct, List<HttpPostedFileBase> thumbImage, List<HttpPostedFileBase> fullImage)
        {
            oNurseryFDMProduct.ProducTCategoryID = "1";
            oNurseryFDMProduct.PlantAge = 1;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            string RedirectType = string.Empty;
            try
            {
                NurseryFDMProduct obj = new NurseryFDMProduct();

                DataTable DT = getDTofMapNurserieHeadPrice(LstHeadDetails);
                oNurseryFDMProduct.EnteredBy = UserID;
                oNurseryFDMProduct.UpdatedBy = UserID;

                if (oNurseryFDMProduct.ID == 0 || oNurseryFDMProduct.ID == -1)
                {
                    oNurseryFDMProduct.BaseProduceTypeID = string.Join(",", oNurseryFDMProduct.BaseProduceTypeIDs);
                }

                if (oNurseryFDMProduct.ID == -1)
                {
                    RedirectType = "other";
                    oNurseryFDMProduct.ID = 0;
                }
                else
                {
                    RedirectType = "Same";
                }

                #region Add Image

                if (thumbImage != null)
                {
                    string FileFullName = string.Empty;
                    string FilePath = "~/NurseryImages/Thumb/";
                    string path;
                    int i = 0;
                    foreach (var itm in thumbImage)
                    {
                        if (itm != null)
                        {
                            var fileExt = System.IO.Path.GetExtension(itm.FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + oNurseryFDMProduct.ProductName + fileExt;
                            path = Path.Combine(FilePath, FileFullName);
                            Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                            i++;
                            oNurseryFDMProduct.ProductThumbImage = path;
                        }
                    }
                }


                if (fullImage != null)
                {
                    string FileFullName = string.Empty;
                    string FilePath = "~/NurseryImages/Full/";
                    string path;
                    int i = 0;
                    foreach (var itm in fullImage)
                    {
                        if (itm != null)
                        {
                            var fileExt = System.IO.Path.GetExtension(itm.FileName);
                            FileFullName = DateTime.Now.Ticks + "_" + oNurseryFDMProduct.ProductName + fileExt;
                            path = Path.Combine(FilePath, FileFullName);
                            Request.Files[i].SaveAs(Server.MapPath(FilePath + FileFullName));
                            i++;

                            oNurseryFDMProduct.ProductFullImage = path;
                        }
                    }
                }
                #endregion

                DataTable dtf = obj.AddUpdateNurseryFDMProduct(oNurseryFDMProduct, DT);
                status = dtf.Rows[0][0].ToString();
				if (status == "True")
				{
					TempData["msg"] = "Product Successfully Added";
				}
				else
				{
					TempData["msg"] = "Product already Exist";
					//return RedirectToAction("GetNurseryFDMProduct", "ManageInventory", new { ID = "0", ProduceFor = oNurseryFDMProduct.ProduceFor });
				}
					
				
			}
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            //if (RedirectType == "Same")
            //{
            //    return RedirectToAction("NurseryFDMProduct", new { RecordStatus = status, type = oNurseryFDMProduct.ProduceFor });
            //}
            //else
            //{
                return RedirectToAction("ManageNurseryInventory", "ManageInventory", new { type = oNurseryFDMProduct.ProduceFor });
           // }
        }
        public ActionResult GetNurseryFDMProduct(string ID, string ProduceFor)
        {
            NurseryFDMProduct obj = new NurseryFDMProduct();
            List<SelectListItem> NurseryFDMProduct = new List<SelectListItem>();
            DataSet dtf = new DataSet();
			DataSet dtf1 = new DataSet();
			List<MapNurserieHeadPrice> itemsMapNurserieHeadPrice = new List<MapNurserieHeadPrice>();


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ViewBag.OpType = (ID == "0" ? "Add Plant Details" : "Edit Plant Details");

                dtf = obj.Select_NurseryFDMProduct(Convert.ToInt16(ID));
			
                if (dtf.Tables != null && dtf.Tables.Count > 1)
                {


                    foreach (DataRow dr in dtf.Tables[1].Rows)
                    {
                        itemsMapNurserieHeadPrice.Add(new MapNurserieHeadPrice()
                        {
                            NurseriesHeadID = Convert.ToInt32(dr["NurseriesHeadID"]),
                            NurserieHeadName = dr["NurserieHeadName"].ToString(),
                            Price = Convert.ToDecimal(dr["Price"]),
                        });
                    }

                    foreach (DataRow dr in dtf.Tables[0].Rows)
                    {
                        obj.ID = Convert.ToInt64(dr["ID"].ToString());
                        //VehicleName = Convert.ToString(dr["VehicleName"].ToString()),
                        obj.ProduceTypeID = Convert.ToInt64(dr["ProduceTypeID"].ToString());
                        obj.ProductName = Convert.ToString(dr["ProductName"].ToString());

                        obj.ProductThumbImage = Convert.ToString(dr["ProductThumbImage"]);
                        obj.ProductFullImage = Convert.ToString(dr["ProductFullImage"]);

                        obj.ProduceType = Convert.ToString(dr["ProduceType"].ToString());
                        obj.Unit = Convert.ToString(dr["Unit"].ToString());
                        obj.IsActive = Convert.ToInt32(dr["IsActive"]);
                        obj.OperationType = "Edit Product Name";
                        obj.HeadPriceList = itemsMapNurserieHeadPrice;
                        obj.DiscountCitizen = Convert.ToDecimal(dr["DiscountCitizen"]);
                        obj.DiscountDepartment = Convert.ToDecimal(dr["DiscountDepartment"]);
                        obj.DiscountNGO = Convert.ToDecimal(dr["DiscountNGO"]);
                        obj.BaseProduceTypeID = Convert.ToString(dr["BaseProduceTypeID"]);
                    }


                    if (dtf.Tables[0].Rows.Count == 0)
                    {
                        ViewBag.ProduceTypeID = 0;
                    }
                    else
                    {
                        ViewBag.ProduceTypeID = Convert.ToInt64(dtf.Tables[0].Rows[0]["ProduceTypeID"].ToString());
                    }
                    TempData["MapNurserieHeadPrice"] = dtf.Tables[1];
                }


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                DataTable dtf2 = obj.ProductType(Encryption.decrypt(ProduceFor));

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTNurseryFDMProduct.Add(new SelectListItem { Text = @dr1["ProduceType"].ToString(), Value = @dr1["ID"].ToString() });
                }

                List<SelectListItem> BaseProductType = new List<SelectListItem>();
                DataTable dtf3 = obj.BaseProductType(obj.ProduceTypeID.ToString());
                foreach (DataRow dr1 in dtf3.Rows)
                {
                    BaseProductType.Add(new SelectListItem { Text = @dr1["Name"].ToString(), Value = @dr1["ID"].ToString() });
                }
                TempData["BaseProductType"] = BaseProductType;

                ViewBag.ddlNurseryFDMProduct = LSTNurseryFDMProduct;

                obj.ProduceFor = ProduceFor;
				List<SelectListItem> ProductCategoryList = new List<SelectListItem>();
				DataTable dtf4 = obj.ProductCategoryList();
				foreach (DataRow dr1 in dtf4.Rows)
				{
					ProductCategoryList.Add(new SelectListItem { Text = @dr1["ProductCategory"].ToString(), Value = @dr1["ID"].ToString() });
				}
				ViewBag.ddlProductCategory = ProductCategoryList;

				List<SelectListItem> ProductNamesList = new List<SelectListItem>();
                //DataTable dtf5 = obj.ProductList();
                //foreach (DataRow dr1 in dtf5.Rows)
                //{
                //    ProductNamesList.Add(new SelectListItem { Text = @dr1["CommanEngName"].ToString() + "/" + @dr1["CommanHindiName"].ToString(), Value = @dr1["ID"].ToString() });
                //}
                ViewBag.ddlProductName = ProductNamesList;
                
                TreeAgeList.Add(new SelectListItem { Text = "Less Than One Year", Value = "0" });
                TreeAgeList.Add(new SelectListItem { Text = "Greater Than One Year", Value = "1" });

                ViewBag.TreeAgeList = TreeAgeList;

            }


            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }


            if (ID == "0" || ID == "-1")
            {
                return PartialView("_partialNurseryFDMProduct", obj);
            }
            else
            {
                return PartialView("_partialNurseryFDMProductEdit", obj);
            }

        }

        [HttpPost]
        public JsonResult getHeadDetails(string objectType, string ID)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {



                    List<MapNurserieHeadPrice> itemsMapNurserieHeadPrice = new List<MapNurserieHeadPrice>();

                    InventoryManagement _objProduce = new InventoryManagement();
                    DataTable dsMemberVehle = new DataTable();


                    if (objectType != "in")
                    {
                        if (TempData["MapNurserieHeadPrice"] != null)
                        {
                            dsMemberVehle = (DataTable)TempData["MapNurserieHeadPrice"];
                            TempData["MapNurserieHeadPrice"] = null;
                        }


                    }
                    else
                    {
                        dsMemberVehle = _objProduce.GetHeadDetails(ID);
                    }

                    //  dsMemberVehle = _objProduce.GetHeadDetails();
                    foreach (DataRow dr in dsMemberVehle.Rows)
                    {
                        itemsMapNurserieHeadPrice.Add(new MapNurserieHeadPrice()
                        {
                            NurseriesHeadID = Convert.ToInt32(dr["NurseriesHeadID"]),
                            NurserieHeadName = dr["NurserieHeadName"].ToString(),
                            Price = Convert.ToDecimal(dr["Price"]),
                        });
                    }

                    var VehiclePartialView = RenderRazorViewToString(this.ControllerContext, "MapNurserieHeadPrice", itemsMapNurserieHeadPrice);
                    var VehicleStatus = "FLASE";
                    if (dsMemberVehle.Rows.Count > 0)
                    {
                        VehicleStatus = "TRUE";
                    }

                    string TicketStatus = Convert.ToString(dsMemberVehle.Rows[0][0]);

                    var json = Json(new { VehiclePartialView, VehicleStatus });
                    return json;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 3, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return null;

        }


        public JsonResult getBaseProductType(string ProduceTypeID,int ProductCategoryId)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                NurseryFDMProduct obj = new NurseryFDMProduct();
                List<SelectListItem> BaseProductType = new List<SelectListItem>();
                DataTable dtf3 = obj.BaseProductType(ProduceTypeID);
                foreach (DataRow dr1 in dtf3.Rows)
                {
                    BaseProductType.Add(new SelectListItem { Text = @dr1["Name"].ToString(), Value = @dr1["ID"].ToString() });
                }
                SelectList baseProductList = new SelectList(BaseProductType, "Value", "Text");
                DataTable dtf5 = obj.ProductList(ProduceTypeID, ProductCategoryId);
                List<SelectListItem> ProductNamesList = new List<SelectListItem>();
                foreach (DataRow dr1 in dtf5.Rows)
                {
                    ProductNamesList.Add(new SelectListItem { Text = @dr1["CommanEngName"].ToString() + "/" + @dr1["CommanHindiName"].ToString(), Value = @dr1["ID"].ToString() });
                }
                SelectList productNameList = new SelectList(ProductNamesList, "Value", "Text");
                var data = new { BaseProductList = baseProductList, ProductNameList = productNameList };
                return Json(data,JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 3, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return null;

        }

		public JsonResult getProduct(string ProduceTypeID,int ProductCategoryId)
		{

			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

			try
			{
				NurseryFDMProduct obj = new NurseryFDMProduct();
				List<SelectListItem> Product = new List<SelectListItem>();
				DataTable dtf3 = obj.ProductList(ProduceTypeID,ProductCategoryId);
				foreach (DataRow dr1 in dtf3.Rows)
				{
					Product.Add(new SelectListItem { Text = @dr1["CommanEngName"].ToString()+"/"+ @dr1["CommanHindiName"].ToString(), Value = @dr1["ID"].ToString() });
					//Product.Add(new SelectListItem { Text = @dr1["ProductName"].ToString(), Value = @dr1["ID"].ToString() });
				}

				return Json(new SelectList(Product, "Value", "Text"));

			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 3, DateTime.Now, Convert.ToInt64(Session["UserID"]));
			}
			return null;

		}

		public JsonResult getProductCategory()
		{

			string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
			string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

			try
			{
				NurseryFDMProduct obj = new NurseryFDMProduct();
				List<SelectListItem> Product = new List<SelectListItem>();
				DataTable dtf3 = obj.ProductCategoryList();
				foreach (DataRow dr1 in dtf3.Rows)
				{
					Product.Add(new SelectListItem { Text = @dr1["ProductCategory"].ToString(), Value = @dr1["ID"].ToString() });
				}

				return Json(new SelectList(Product, "Value", "Text"));

			}
			catch (Exception ex)
			{
				new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 3, DateTime.Now, Convert.ToInt64(Session["UserID"]));
			}
			return null;

		}

		public DataTable getDTofMapNurserieHeadPrice(List<MapNurserieHeadPrice> lstMapNurserieHeadPrice)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region Vehicle Info
                objDt2.Columns.Add("NurseriesHeadID", typeof(int));
                objDt2.Columns.Add("NurserieHeadName", typeof(String));
                objDt2.Columns.Add("Price", typeof(decimal));

                objDt2.AcceptChanges();
                foreach (var item in lstMapNurserieHeadPrice)
                {
                    DataRow dr = objDt2.NewRow();

                    dr["NurseriesHeadID"] = item.NurseriesHeadID;
                    dr["NurserieHeadName"] = item.NurserieHeadName;
                    dr["Price"] = item.Price;

                    objDt2.Rows.Add(dr);
                    objDt2.AcceptChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }
        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model)
        {
            controllerContext.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

        #region "ZooVehicle"
        public ActionResult ZooVehicle(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> ZooVehicle = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooVehicle obj = new ZooVehicle();
            try
            {

                DataTable dtf = obj.Select_ZooVehicles();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZVehicleLst.Add(
                        new ZooVehicle()
                        {
                            Index = count,
                            ZooVehicleID = Convert.ToInt32(dr["ZooVehicleID"]),
                            VehicleName = Convert.ToString(dr["VehicleName"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ZVehicleLst);
        }
        public ActionResult ADDUpdateZooVehicle(ZooVehicle oZooVehicle)
        {
            List<SelectListItem> ZooVehicle = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ZooVehicle obj = new ZooVehicle();

                DataTable dtf = obj.AddUpdateZooVehicle(oZooVehicle);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ZooVehicle", new { RecordStatus = status });
        }
        public ActionResult GetZooVehicle(string ZooVehicleID)
        {

            ZooVehicle obj = new ZooVehicle();
            List<SelectListItem> ZooVehicles = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (ZooVehicleID == "0" ? "Add Zoo Vehicle" : "Edit Zoo Vehicle");


                DataTable dtf = obj.Select_ZooVehicle(Convert.ToInt32(ZooVehicleID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ZooVehicle
                    {
                        ZooVehicleID = Convert.ToInt32(dr["ZooVehicleID"].ToString()),
                        VehicleName = Convert.ToString(dr["VehicleName"].ToString()),
                        CategoryID = Convert.ToInt64(dr["CategoryID"].ToString()),
                        CategoryName = Convert.ToString(dr["CategoryName"].ToString()),
                        Isactive = Convert.ToInt32(dr["Isactive"]),



                        OperationType = "Edit ZooVehicle"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;


                DataTable dtf2 = obj.VehicleType();
                // Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTVehicalType.Add(new SelectListItem { Text = @dr1["CategoryName"].ToString(), Value = @dr1["CategoryId"].ToString() });
                }

                ViewBag.ddlVehicalEqptName1 = LSTVehicalType;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialZooVehicle", obj);
        }


        #endregion

        #region "ZooEqptFee"
        public ActionResult ZooEqptFee(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooEqptFee obj = new ZooEqptFee();
            try
            {
                DataTable dtf = obj.Select_ZooEqptFees();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZooEqptFeeLst.Add(
                           new ZooEqptFee()
                           {
                               Index = count,
                               FeeId = Convert.ToInt64(dr["FeeId"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               VehicleName = Convert.ToString(dr["VehicleName"]),
                               FeePerVehicle = Convert.ToDecimal(dr["FeePerVehicle"]),
                               NumberofVehicle = Convert.ToInt16(dr["NumberofVehicle"]),
                               IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ZooEqptFeeLst);
        }
        public ActionResult ADDUpdateZooEqptFee(ZooEqptFee oVehicleEquipment)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                ZooEqptFee obj = new ZooEqptFee();


                DataTable dtf = obj.AddUpdateZooEqptFee(oVehicleEquipment);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ZooEqptFee", new { RecordStatus = status });
        }

        public ActionResult GetZooEqptFee(string FeeId)
        {
            ZooEqptFee obj = new ZooEqptFee();
            List<SelectListItem> ZooEqptFees = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (FeeId == "0" ? "Add Vehicle Fee" : "Edit Vehicle Fee");


                DataTable dtf = obj.Select_ZooEqptFee(Convert.ToInt16(FeeId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ZooEqptFee
                    {
                        FeeId = Convert.ToInt64(dr["FeeId"].ToString()),
                        //VehicleName = Convert.ToString(dr["VehicleName"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        ZooVehicleID = Convert.ToInt32(dr["ZooVehicleID"]),
                        //PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        NumberofVehicle = Convert.ToInt32(dr["NumberofVehicle"]),
                        FeePerVehicle = Convert.ToDecimal(dr["FeePerVehicle"]),

                        IsActive = Convert.ToInt32(dr["IsActive"]),



                        OperationType = "Edit ZooEqptFee"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                DataTable dtf2 = obj.VehicalEqpt();

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTVehicalEqptName.Add(new SelectListItem { Text = @dr1["VehicleName"].ToString(), Value = @dr1["ZooVehicleID"].ToString() });
                }

                ViewBag.ddlVehicalEqptName1 = LSTVehicalEqptName;



                DataTable dtf3 = obj.PlaceName1();

                foreach (DataRow dr1 in dtf3.Rows)
                {
                    LSTPlaceName.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                }

                ViewBag.ddlLSTPlaceName = LSTPlaceName;



            }


            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_partialZooEqptFee", obj);
        }



        #endregion

        #region "ZooHeadMaster"
        public ActionResult ZooHeadMaster(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> ZooHeadMasters = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooHeadMaster obj = new ZooHeadMaster();
            try
            {

                DataTable dtf = obj.Select_ZooHeadMasters();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZooHeadMasterLst.Add(
                        new ZooHeadMaster()
                        {
                            Index = count,
                            HeadId = Convert.ToInt32(dr["HeadId"]),
                            HeadName = Convert.ToString(dr["HeadName"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ZooHeadMasterLst);
        }
        public ActionResult ADDUpdateZooHeadMaster(ZooHeadMaster oZooHeadMaster)
        {
            List<SelectListItem> ZooHeadMaster = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ZooHeadMaster obj = new ZooHeadMaster();

                DataTable dtf = obj.AddUpdateZooHeadMaster(oZooHeadMaster);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ZooHeadMaster", new { RecordStatus = status });
        }
        public ActionResult GetZooHeadMaster(string HeadId)
        {

            ZooHeadMaster obj = new ZooHeadMaster();
            List<SelectListItem> ZooHeadMasters = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (HeadId == "0" ? "Add Zoo Head Master" : "Edit Zoo Head Master");


                DataTable dtf = obj.Select_ZooHeadMaster(Convert.ToInt32(HeadId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ZooHeadMaster
                    {
                        HeadId = Convert.ToInt32(dr["HeadId"]),
                        HeadName = Convert.ToString(dr["HeadName"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit Zoo Head Master"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialZooHeadMaster", obj);
        }



        #endregion


        #region "ZooPlaceWiseHead"
        public ActionResult ZooPlaceWiseHead(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooPlaceWiseHead obj = new ZooPlaceWiseHead();
            try
            {
                //DataTable dtf = obj.Select_ZooPlaceWiseHeads();
                DataTable dtf = obj.SelectPlacesForZoo();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZooPlaceLst.Add(
                           new ZooPlaces()
                           {
                               Index = count,
                               PlaceId = Convert.ToInt32(dr["PlaceId"]),
                               PlaceName = Convert.ToString(dr["PlaceName"])

                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ZooPlaceLst);
        }

        public JsonResult GetHeadDetails(string PlaceId)
        {


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooPlaceWiseHead obj = new ZooPlaceWiseHead();

            try
            {
                //DataTable dtf = obj.Select_ZooPlaceWiseHeads();
                DataTable dtf = obj.SelectHeadDetailsPlaceWise(PlaceId);
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZooPlaceWiseHeadLst.Add(
                           new ZooPlaceWiseHead()
                           {
                               Index = count,
                               ZooPlaceWiseHeadId = Convert.ToInt16(dr["ZooPlaceWiseHeadId"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               HeadId = Convert.ToInt16(dr["HeadId"]),
                               HeadName = Convert.ToString(dr["HeadName"]),
                               HeadAmount = Convert.ToDecimal(dr["HeadAmount"]),
                               FeeChargedOn = Convert.ToString(dr["FeeChargedOn"]),
                               Type = Convert.ToString(dr["Type"]),
                               ParentFeeChangeON = Convert.ToString(dr["ParentFeeChangeON"])


                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(ZooPlaceWiseHeadLst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ADDUpdateZooPlaceWiseHead(ZooPlaceWiseHead oZooPlaceWiseHead)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                ZooPlaceWiseHead obj = new ZooPlaceWiseHead();


                DataTable dtf = obj.AddUpdateZooPlaceWiseHead(oZooPlaceWiseHead);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ZooPlaceWiseHead", new { RecordStatus = status });
        }

        public ActionResult GetZooPlaceWiseHead(string ZooPlaceWiseHeadId)
        {
            ZooPlaceWiseHead obj = new ZooPlaceWiseHead();
            List<SelectListItem> ZooPlaceWiseHeads = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (ZooPlaceWiseHeadId == "0" ? "Add Zoo Place Wise Head" : "Edit Zoo Place Wise Head");


                DataTable dtf = obj.Select_ZooPlaceWiseHead(Convert.ToInt32(ZooPlaceWiseHeadId));

                lstFeeChargeon.Add(new SelectListItem { Text = "Indian Visitors", Value = "Indian Visitors" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Non-Indian Visitors", Value = "Non-Indian Visitors" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Auto Rikshaw", Value = "Auto Rikshaw" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Bus", Value = "Bus" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Video Camera Amount", Value = "Video Camera Amount" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Still Camera Amount", Value = "Still Camera Amount" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Child Below Age of 5 Years", Value = "Child Below Age of 5 Years" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Jeep/Car/Motor/Mini Bus", Value = "Jeep/Car/Motor/Mini Bus" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Students", Value = "Students" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Two Wheeler", Value = "Two Wheeler" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Gypsy Entry Fee", Value = "Gypsy Entry Fee" });
                lstFeeChargeon.Add(new SelectListItem { Text = "Gypsy Vehicle Rent", Value = "Gypsy Vehicle Rent" });

                ViewBag.FeeChargeonlst = lstFeeChargeon;


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;

                lstType.Add(new SelectListItem { Text = "M", Value = "M" });
                lstType.Add(new SelectListItem { Text = "V", Value = "V" });

                ViewBag.Typelst = lstType;

                lstParentFeeChargeon.Add(new SelectListItem { Text = "Indian Visitors", Value = "Indian Visitors" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Non-Indian Visitors", Value = "Non-Indian Visitors" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Auto Rikshaw", Value = "Auto Rikshaw" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Bus", Value = "Bus" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Video Camera Amount", Value = "Video Camera Amount" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Still Camera Amount", Value = "Still Camera Amount" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Child Below Age of 5 Years", Value = "Child Below Age of 5 Years" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Jeep/Car/Motor/Mini Bus", Value = "Jeep/Car/Motor/Mini Bus" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Students", Value = "Students" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Two Wheeler", Value = "Two Wheeler" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Gypsy Entry Fee", Value = "Gypsy Entry Fee" });
                lstParentFeeChargeon.Add(new SelectListItem { Text = "Gypsy Vehicle Rent", Value = "Gypsy Vehicle Rent" });


                ViewBag.ParentFeeChargeonlst = lstParentFeeChargeon;


                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ZooPlaceWiseHead
                    {
                        ZooPlaceWiseHeadId = Convert.ToInt32(dr["ZooPlaceWiseHeadId"]),
                        //VehicleName = Convert.ToString(dr["VehicleName"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        HeadId = Convert.ToInt32(dr["HeadId"]),
                        //PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        FeeChargedOn = Convert.ToString(dr["FeeChargedOn"]),
                        ParentFeeChangeON = Convert.ToString(dr["ParentFeeChangeON"]),
                        Type = Convert.ToString(dr["Type"]),
                        HeadAmount = Convert.ToDecimal(dr["HeadAmount"]),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit ZooPlaceWiseHead"
                    };

                }





                DataTable dtf2 = obj.ZooHeadName();

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTZooPlaceWiseHead.Add(new SelectListItem { Text = @dr1["HeadName"].ToString(), Value = @dr1["HeadId"].ToString() });
                }

                ViewBag.ddlZooPlaceWiseHead = LSTZooPlaceWiseHead;

                DataTable dtf3 = obj.PlaceName1();

                foreach (DataRow dr1 in dtf3.Rows)
                {
                    LSTPlaceName.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                }

                ViewBag.ddlLSTPlaceName = LSTPlaceName;





            }


            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_partialZooPlaceWiseHead", obj);
        }


        #endregion

        #region "ZooSeatInventory"
        public ActionResult ZooSeatInventory(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ZooSeatInventory obj = new ZooSeatInventory();
            try
            {
                DataTable dtf = obj.Select_ZooSeatInventorys();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ZooSeatInventoryLst.Add(
                           new ZooSeatInventory()
                           {
                               Index = count,
                               ZooSeatInventoryId = Convert.ToInt32(dr["ZooSeatInventoryId"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               OnlineZooSeats = Convert.ToInt32(dr["OnlineZooSeats"]),
                               OffLineZooSeats = Convert.ToInt32(dr["OffLineZooSeats"]),
                               IsactiveView = Convert.ToBoolean(dr["isActive"]),
                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ZooSeatInventoryLst);
        }
        public ActionResult ADDUpdateZooSeatInventory(ZooSeatInventory oZooSeatInventory)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                ZooSeatInventory obj = new ZooSeatInventory();


                DataTable dtf = obj.AddUpdateZooSeatInventory(oZooSeatInventory);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ZooSeatInventory", new { RecordStatus = status });
        }

        public ActionResult GetZooSeatInventory(string ZooSeatInventoryId)
        {
            ZooSeatInventory obj = new ZooSeatInventory();
            List<SelectListItem> ZooSeatInventorys = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (ZooSeatInventoryId == "0" ? "Add Zoo Seat Inventory" : "Edit Zoo Seat Inventory");


                DataTable dtf = obj.Select_ZooSeatInventory(Convert.ToInt16(ZooSeatInventoryId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ZooSeatInventory
                    {
                        ZooSeatInventoryId = Convert.ToInt32(dr["ZooSeatInventoryId"]),
                        //VehicleName = Convert.ToString(dr["VehicleName"].ToString()),
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),

                        //PlaceName = Convert.ToString(dr["PlaceName"].ToString()),
                        OnlineZooSeats = Convert.ToInt32(dr["OnlineZooSeats"]),
                        OffLineZooSeats = Convert.ToInt32(dr["OffLineZooSeats"]),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit ZooSeatInventory"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;




                DataTable dtf2 = obj.PlaceName1();

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTPlaceName.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                }

                ViewBag.ddlLSTPlaceName = LSTPlaceName;




            }


            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("_partialZooSeatInventory", obj);
        }



        #endregion

        #region "Users"
        public ActionResult Users(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            Users obj = new Users();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                DataTable dtf = obj.Select_UserProfiles();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstUsers.Add(
                        new Users()
                        {
                            Index = count,
                            UserID = Convert.ToString(dr["UserID"]),
                            Ssoid = Convert.ToString(dr["Ssoid"]),
                            Name = Convert.ToString(dr["Name"]),
                            RoleId = Convert.ToString(dr["RoleId"]),
                            EmailId = Convert.ToString(dr["EmailId"]),
                            Designation = Convert.ToString(dr["Designation"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstUsers);
        }

        public ActionResult AddUpdateUserProfile(Users user)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            string status = null;
            try
            {

                Users obj = new Users();

                DataTable dt = obj.AddUpdateUserProfile(user);

                status = dt.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                //  new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Users", new { RecordStatus = status });
        }
        public ActionResult GetUser(string UserID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Users obj = new Users();
            try
            {
                List<SelectListItem> Role = new List<SelectListItem>();
                // List<SelectListItem> items = new List<SelectListItem>();
                DataTable dt = _objRole.BindROLE();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Role.Add(new SelectListItem { Text = @dr["RoleId"].ToString(), Value = @dr["RoleId"].ToString() });
                }

                ViewBag.Roles = Role;

                ViewBag.OpType = (UserID == "0" ? "Add User" : "Edit Assing Role");


                DataTable dtf = obj.Select_UserProfile(Convert.ToInt32(UserID));


                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Users
                    {
                        Ssoid = Convert.ToString(dr["SsoId"]),
                        Name = Convert.ToString(dr["Name"]),
                        RoleId = Convert.ToString(dr["RoleId"]),
                        EmailId = Convert.ToString(dr["EmailId"]),
                        Designation = Convert.ToString(dr["Designation"]),


                        OperationType = "Edit User"
                    };

                }



            }
            catch (Exception ex)
            {
                //  new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialusers", obj);
        }





        #endregion


        #region "Home Page Ticker"
        public ActionResult Ticker(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Ticker obj = new Ticker();
            try
            {

                DataTable dtf = obj.Select_Tickers();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    TickerLst.Add(
                        new Ticker()
                        {
                            Index = count,
                            TickerId = Convert.ToInt32(dr["TickerId"]),
                            TickerMessage = Convert.ToString(dr["TickerMessage"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(TickerLst);
        }
        public ActionResult AddUpdateTicker(Ticker oTicker)
        {
            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Ticker obj = new Ticker();

                DataTable dtf = obj.AddUpdateTicker(oTicker);
                //oTicker.LastUpdatedBy = UserID;
                //status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Ticker", new { RecordStatus = status });
        }
        public ActionResult GetTicker(string TickerId)
        {

            Ticker obj = new Ticker();
            List<SelectListItem> Ticker = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (TickerId == "0" ? "Add Ticker" : "Edit Ticker");


                DataTable dtf = obj.Select_Ticker(Convert.ToInt32(TickerId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Ticker
                    {
                        TickerId = Convert.ToInt64(dr["TickerId"].ToString()),
                        TickerMessage = Convert.ToString(dr["TickerMessage"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit Ticker"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialTicker", obj);
        }


        #endregion


        #region "Designations"
        public ActionResult Designations(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            Designations obj = new Designations();
            try
            {

                DataTable dtf = obj.Select_Designations();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    DesignationLst.Add(
                        new Designations()
                        {
                            Index = count,
                            DesigId = Convert.ToInt32(dr["DesigId"]),
                            Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                            Desig_Alias = Convert.ToString(dr["Desig_Alias"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(DesignationLst);
        }
        public ActionResult AddUpdateDesignation(Designations oDesignations)
        {
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                Designations obj = new Designations();

                DataTable dtf = obj.AddUpdateDesignation(oDesignations);
                oDesignations.LastUpdatedBy = UserID;
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("Designations", new { RecordStatus = status });
        }
        public ActionResult GetDesignations(string DesigId)
        {

            Designations obj = new Designations();
            List<SelectListItem> Designations = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (DesigId == "0" ? "Add Designation" : "Edit Designations");


                DataTable dtf = obj.Select_Designation(Convert.ToInt32(DesigId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new Designations
                    {
                        DesigId = Convert.ToInt32(dr["DesigId"].ToString()),
                        Desig_Name = Convert.ToString(dr["Desig_Name"].ToString()),
                        Desig_Alias = Convert.ToString(dr["Desig_Alias"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"]),
                        IsReviewer = Convert.ToInt32(dr["IsReviewer"]),
                        IsApprover = Convert.ToInt32(dr["IsApprover"]),


                        OperationType = "Edit Designations"
                    };

                }
                lstisReviewer.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisReviewer.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.ISReviewerlst = lstisReviewer;

                lstisApprover.Add(new SelectListItem { Text = "Yes", Value = "1" });
                lstisApprover.Add(new SelectListItem { Text = "No", Value = "0" });

                ViewBag.ISApproverlst = lstisApprover;


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialDesignation", obj);
        }


        #endregion


        #region "EducationQualification"
        public ActionResult EducationQualification(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                EducationQualification obj = new EducationQualification();
                DataTable dtf = obj.Select_EducationQualifications();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstEducationQualification.Add(
                        new EducationQualification()
                        {
                            Index = count,
                            Edu_ID = Convert.ToInt32(dr["Edu_ID"].ToString()),
                            EName = Convert.ToString(dr["EName"]),

                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstEducationQualification);
        }

        public ActionResult ADDUpdateEducationQualification(EducationQualification oEducationQualification)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                EducationQualification obj = new EducationQualification();


                oEducationQualification.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateEducationQualification(oEducationQualification);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("EducationQualification", new { RecordStatus = status });
        }
        public ActionResult GetEducationQualification(string Edu_ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            EducationQualification obj = new EducationQualification();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_EducationQualification(Convert.ToInt16(Edu_ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new EducationQualification
                    {
                        Edu_ID = Convert.ToInt32(dr["Edu_ID"].ToString()),
                        EName = Convert.ToString(dr["EName"]),

                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit EducationQualification"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialEducationQualification", obj);
        }





        #endregion

        #region "IndustryType"
        public ActionResult IndustryType(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                IndustryType obj = new IndustryType();
                DataTable dtf = obj.Select_IndustryTypes();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstIndustryType.Add(
                        new IndustryType()
                        {
                            Index = count,
                            IID = Convert.ToInt32(dr["IID"].ToString()),
                            IName = Convert.ToString(dr["IName"]),
                            IndustDefinition = Convert.ToString(dr["IndustDefinition"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstIndustryType);
        }

        public ActionResult ADDUpdateIndustryType(IndustryType oIndustryType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                IndustryType obj = new IndustryType();


                oIndustryType.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateIndustryType(oIndustryType);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("IndustryType", new { RecordStatus = status });
        }
        public ActionResult GetIndustryType(string IID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            IndustryType obj = new IndustryType();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_IndustryType(Convert.ToInt16(IID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new IndustryType
                    {
                        IID = Convert.ToInt32(dr["IID"].ToString()),
                        IName = Convert.ToString(dr["IName"]),
                        IndustDefinition = Convert.ToString(dr["IndustDefinition"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit IndustryType"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialIndustryType", obj);
        }


        #endregion

        #region "SawmillType"
        public ActionResult SawmillType(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                SawmillType obj = new SawmillType();
                DataTable dtf = obj.Select_SawmillTypes();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstSawmillType.Add(
                        new SawmillType()
                        {
                            Index = count,
                            SID = Convert.ToInt32(dr["SID"].ToString()),
                            SName = Convert.ToString(dr["SName"]),
                            SawmillDefinition = Convert.ToString(dr["SawmillDefinition"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstSawmillType);
        }
        public ActionResult ADDUpdateSawmillType(SawmillType oSawmillType)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                SawmillType obj = new SawmillType();


                oSawmillType.LastUpdatedBy = UserID;
                DataTable dtf = obj.AddUpdateSawmillType(oSawmillType);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("SawmillType", new { RecordStatus = status });
        }
        public ActionResult GetSawmillType(string SID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            SawmillType obj = new SawmillType();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_SawmillType(Convert.ToInt16(SID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new SawmillType
                    {
                        SID = Convert.ToInt32(dr["SID"].ToString()),
                        SName = Convert.ToString(dr["SName"]),
                        SawmillDefinition = Convert.ToString(dr["SawmillDefinition"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit SawmillType"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialSawmillType", obj);
        }





        #endregion


        #region "FixedPermissionTypes"
        public ActionResult FixedPermissionTypes(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                FixedPermissionTypes obj = new FixedPermissionTypes();
                DataTable dtf = obj.Select_FixedPermissionTypess();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstFixedPermissionTypes.Add(
                        new FixedPermissionTypes()
                        {
                            Index = count,
                            P_ID = Convert.ToInt32(dr["P_ID"].ToString()),
                            Name = Convert.ToString(dr["Name"]),
                            Description = Convert.ToString(dr["Description"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),



                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstFixedPermissionTypes);
        }

        public ActionResult ADDUpdateFixedPermissionTypes(FixedPermissionTypes oFixedPermissionTypes)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                FixedPermissionTypes obj = new FixedPermissionTypes();

                oFixedPermissionTypes.EnteredBy = UserID;

                DataTable dtf = obj.AddUpdateFixedPermissionTypes(oFixedPermissionTypes);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("FixedPermissionTypes", new { RecordStatus = status });
        }
        public ActionResult GetFixedPermissionTypes(string P_ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            FixedPermissionTypes obj = new FixedPermissionTypes();
            try
            {
                lstStatus.Add(new SelectListItem { Text = "Active", Value = "Active" });
                lstStatus.Add(new SelectListItem { Text = "Deactive", Value = "Deactive" });

                ViewBag.Statustype = lstStatus;

                ViewBag.OpType = (P_ID == "0" ? "Add FixedPermissionTypes" : "Edit FixedPermissionTypes");

                DataTable dtf = obj.Select_FixedPermissionTypes(Convert.ToInt32(P_ID));



                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new FixedPermissionTypes
                    {
                        P_ID = Convert.ToInt32(dr["P_ID"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Description = Convert.ToString(dr["Description"]),
                        Status = Convert.ToString(dr["Status"]),
                        Amount = Convert.ToDecimal(dr["Amount"]),
                        Tax = Convert.ToDecimal(dr["Tax"]),
                        EmitraServiceId = Convert.ToInt32(dr["EmitraServiceId"].ToString()),
                        Discount = Convert.ToInt32(dr["Discount"].ToString()),

                        OperationType = "Edit FixedPermissionTypes"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialFixedPermissionTypes", obj);
        }



        #endregion



        #region "ProduceType"
        public ActionResult ProduceType(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ProduceTypes obj = new ProduceTypes();
            try
            {

                DataTable dtf = obj.Select_ProduceTypes();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ProduceTypeLst.Add(
                        new ProduceTypes()
                        {
                            Index = count,
                            ID = Convert.ToInt64(dr["ID"].ToString()),
                            ProduceType = Convert.ToString(dr["ProduceType"].ToString()),
                            UnitName = Convert.ToString(dr["UnitName"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                            ProduceFor = Convert.ToString(dr["ProduceFor"].ToString()),
                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ProduceTypeLst);
        }
        public ActionResult ADDUpdateProduceType(ProduceTypes oProduceType)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ProduceTypes obj = new ProduceTypes();
                oProduceType.EnteredBy = Convert.ToString(UserID);
                oProduceType.UpdatedBy = Convert.ToString(UserID);
                DataTable dtf = obj.AddUpdateProduceType(oProduceType);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return RedirectToAction("ProduceType", new { RecordStatus = status });
        }
        public ActionResult GetProduceType(string ID)
        {

            ProduceTypes obj = new ProduceTypes();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {


                List<SelectListItem> items = new List<SelectListItem>();

                ViewBag.OpType = (ID == "0" ? "Add Produce Type" : "Edit Produce Type");


                DataSet dtf = new DataSet();
                dtf = obj.Select_ProduceType(Convert.ToInt32(ID));

                foreach (DataRow dr in dtf.Tables[0].Rows)
                {
                    obj = new ProduceTypes
                    {
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        ProduceType = Convert.ToString(dr["ProduceType"].ToString()),
                        UnitName = Convert.ToString(dr["UnitName"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"]),
                        ProduceFor = Convert.ToString(dr["ProduceFor"].ToString()),
                        OperationType = "Edit Produce Type"
                    };

                }

                foreach (System.Data.DataRow dr in dtf.Tables[1].Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["UnitName"].ToString(), Value = @dr["UnitName"].ToString() });
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                List<SelectListItem> ProduceFor = new List<SelectListItem>();

                ProduceFor.Add(new SelectListItem { Text = "Forest Produce", Value = "Forest Produce" });
                ProduceFor.Add(new SelectListItem { Text = "Nursery Produce", Value = "Nursery Produce" });


                ViewBag.ProduceFor = ProduceFor;

                ViewBag.ISactivelst = lstISactive;
                ViewBag.LstUnit = items;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialProduceType", obj);
        }

        #endregion

        #region "NurseryHeadMaster"
        public ActionResult NurseryHeadMaster(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            NurseryHeadMaster obj = new NurseryHeadMaster();
            try
            {

                DataTable dtf = obj.Select_NurseryHeadMasters();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    NurseryHeadMasterLst.Add(
                        new NurseryHeadMaster()
                        {
                            Index = count,
                            NurseriesHeadID = Convert.ToInt64(dr["NurseriesHeadID"].ToString()),
                            NurserieHeadName = Convert.ToString(dr["NurserieHeadName"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(NurseryHeadMasterLst);
        }
        public ActionResult ADDUpdateNurseryHeadMaster(NurseryHeadMaster oNurseryHeadMaster)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                NurseryHeadMaster obj = new NurseryHeadMaster();

                DataTable dtf = obj.AddUpdateNurseryHeadMaster(oNurseryHeadMaster);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("NurseryHeadMaster", new { RecordStatus = status });
        }
        public ActionResult GetNurseryHeadMaster(string NurseriesHeadID)
        {

            NurseryHeadMaster obj = new NurseryHeadMaster();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {


                List<SelectListItem> items = new List<SelectListItem>();

                ViewBag.OpType = (NurseriesHeadID == "0" ? "Add Nursery Head Master" : "Edit Nursery Head Master");


                DataTable dtf = obj.Select_NurseryHeadMaster(Convert.ToInt32(NurseriesHeadID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new NurseryHeadMaster
                    {
                        NurseriesHeadID = Convert.ToInt64(dr["NurseriesHeadID"].ToString()),
                        NurserieHeadName = Convert.ToString(dr["NurserieHeadName"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit Nursery Head Master"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialNurseryHeadMaster", obj);
        }

        #endregion
        #region "UnitNameMaster"
        public ActionResult UnitNameMaster(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            UnitNameMaster obj = new UnitNameMaster();
            try
            {

                DataTable dtf = obj.Select_UnitNameMasters();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    UnitNameMasterLst.Add(
                        new UnitNameMaster()
                        {
                            Index = count,
                            UnitId = Convert.ToInt64(dr["UnitId"].ToString()),
                            ShortName = Convert.ToString(dr["ShortName"].ToString()),
                            UnitName = Convert.ToString(dr["UnitName"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(UnitNameMasterLst);
        }
        public ActionResult ADDUpdateUnitNameMaster(UnitNameMaster oUnitNameMaster)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                UnitNameMaster obj = new UnitNameMaster();

                DataTable dtf = obj.AddUpdateUnitNameMaster(oUnitNameMaster);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("UnitNameMaster", new { RecordStatus = status });
        }
        public ActionResult GetUnitNameMaster(string UnitId)
        {

            UnitNameMaster obj = new UnitNameMaster();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {


                List<SelectListItem> items = new List<SelectListItem>();

                ViewBag.OpType = (UnitId == "0" ? "Add Unit Name" : "Edit Unit Name");


                DataTable dtf = obj.Select_UnitNameMaster(Convert.ToInt32(UnitId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new UnitNameMaster
                    {
                        UnitId = Convert.ToInt64(dr["UnitId"].ToString()),
                        ShortName = Convert.ToString(dr["ShortName"].ToString()),
                        UnitName = Convert.ToString(dr["UnitName"].ToString()),
                        isActive = Convert.ToInt32(dr["isActive"]),



                        OperationType = "Edit Unit Name"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialUnitNameMaster", obj);
        }




        #endregion

        #region "TransportMaster"
        public ActionResult TransportMaster(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                TransportMaster obj = new TransportMaster();
                DataTable dtf = obj.Select_TransportMasters();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstTransportMaster.Add(
                        new TransportMaster()
                        {
                            Index = count,
                            TransportId = Convert.ToInt32(dr["TransportId"].ToString()),
                            Name = Convert.ToString(dr["Name"]),
                            Details = Convert.ToString(dr["Details"]),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstTransportMaster);
        }
        public ActionResult ADDUpdateTransportMaster(TransportMaster oTransportMaster)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                TransportMaster obj = new TransportMaster();



                DataTable dtf = obj.AddUpdateTransportMaster(oTransportMaster);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("TransportMaster", new { RecordStatus = status });
        }
        public ActionResult GetTransportMaster(string TransportId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            TransportMaster obj = new TransportMaster();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_TransportMaster(Convert.ToInt16(TransportId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new TransportMaster
                    {
                        TransportId = Convert.ToInt32(dr["TransportId"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Details = Convert.ToString(dr["Details"]),
                        isActive = Convert.ToInt32(dr["isActive"] == "" ? 0 : dr["isActive"]),


                        OperationType = "Edit TransportMaster"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialTransportMaster", obj);
        }

        #endregion




        #region "ValueTypeName"
        public ActionResult ValueTypeName(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ValueTypeName obj = new ValueTypeName();
                DataTable dtf = obj.Select_ValueType_Names();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstValueTypeName.Add(
                        new ValueTypeName()
                        {
                            Index = count,
                            VId = Convert.ToInt32(dr["VId"].ToString()),
                            ValueType_Name = Convert.ToString(dr["ValueType_Name"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstValueTypeName);
        }
        public ActionResult ADDUpdate_ValueType_Name(ValueTypeName oVTN)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ValueTypeName obj = new ValueTypeName();



                DataTable dtf = obj.AddUpdate_ValueType_Name(oVTN);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ValueTypeName", new { RecordStatus = status });
        }
        public ActionResult Get_ValueType_Name(string VId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ValueTypeName obj = new ValueTypeName();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_ValueType_Name(Convert.ToInt16(VId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ValueTypeName
                    {
                        VId = Convert.ToInt32(dr["VId"].ToString()),
                        ValueType_Name = Convert.ToString(dr["ValueType_Name"]),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit ValueTypeName"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialValueTypeName", obj);
        }





        #endregion

        #region "BaseProduceType"
        public ActionResult BaseProduceType(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                NurseryFDMProduct OBJ1 = new Models.Master.NurseryFDMProduct();
                BaseProduceType obj = new BaseProduceType();
                DataTable dtf = obj.Select_BaseProduceTypes();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstBaseProduceType.Add(
                        new BaseProduceType()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"].ToString()),
                            Name = Convert.ToString(dr["Name"]),
                            Price = Convert.ToDouble(dr["Price"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstBaseProduceType);
        }
        public ActionResult ADDUpdate_BaseProduceType(BaseProduceType oBPT)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                BaseProduceType obj = new BaseProduceType();



                DataTable dtf = obj.AddUpdate_BaseProduceType(oBPT);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("BaseProduceType", new { RecordStatus = status });
        }
        public ActionResult Get_BaseProduceType(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            BaseProduceType obj = new BaseProduceType();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_BaseProduceType(Convert.ToInt16(ID));
                NurseryFDMProduct OBJ1 = new NurseryFDMProduct();
                DataTable dtf2 = OBJ1.ProductType("");

                foreach (DataRow dr1 in dtf2.Rows)
                {
                    LSTNurseryFDMProduct.Add(new SelectListItem { Text = @dr1["ProduceType"].ToString(), Value = @dr1["ID"].ToString() });
                }

                TempData["ddlNurseryFDMProduct1"] = LSTNurseryFDMProduct;

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new BaseProduceType
                    {
                        ID = Convert.ToInt32(dr["ID"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Price = Convert.ToDouble(dr["Price"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),
                        ProductTypeID = Convert.ToString(Convert.ToInt32(dr["ProduceTypeID"].ToString())),

                        OperationType = "Edit BaseProduceType"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialBaseProduceType", obj);
        }





        #endregion

        #region "NurseryDiscountType"
        public ActionResult NurseryDiscountType(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {


                NurseryDiscountType obj = new NurseryDiscountType();
                DataTable dtf = obj.Select_NurseryDiscountTypes();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstNurseryDiscountType.Add(
                        new NurseryDiscountType()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["ID"].ToString()),
                            Name = Convert.ToString(dr["Name"]),
                            Value = Convert.ToDouble(dr["Value"].ToString()),
                            ValueType_Name = Convert.ToString(dr["ValueType_Name"]),
                            IsactiveView = Convert.ToBoolean(dr["IsActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstNurseryDiscountType);
        }
        public ActionResult ADDUpdate_NurseryDiscountType(NurseryDiscountType oBPT)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                NurseryDiscountType obj = new NurseryDiscountType();



                DataTable dtf = obj.AddUpdate_NurseryDiscountType(oBPT);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("NurseryDiscountType", new { RecordStatus = status });
        }
        public ActionResult Get_NurseryDiscountType(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            NurseryDiscountType obj = new NurseryDiscountType();
            try
            {

                List<SelectListItem> Valuetype = new List<SelectListItem>();

                DataTable dt = obj.SelectAllValueTypeName();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Valuetype.Add(new SelectListItem { Text = @dr["ValueType_Name"].ToString(), Value = @dr["VID"].ToString() });
                }


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                ViewBag.ValuetypeLst = Valuetype;
                DataTable dtf = obj.Select_NurseryDiscountType(Convert.ToInt16(ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new NurseryDiscountType
                    {
                        ID = Convert.ToInt32(dr["ID"].ToString()),
                        Name = Convert.ToString(dr["Name"]),
                        Value = Convert.ToDouble(dr["Value"].ToString()),
                        VID = Convert.ToInt32(dr["VID"].ToString()),
                        IsActive = Convert.ToInt32(dr["IsActive"] == "" ? 0 : dr["IsActive"]),


                        OperationType = "Edit NurseryDiscountType"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialNurseryDiscountType", obj);
        }





        #endregion

        #region "ADWorkDescription"
        public ActionResult ADWorkDescription(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ADWorkDescription obj = new ADWorkDescription();
                DataTable dtf = obj.Select_AD_WorkDescriptions();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstAD_WorkDescription.Add(
                        new ADWorkDescription()
                        {
                            Index = count,
                            WorkDescriptionId = Convert.ToInt32(dr["WorkDescriptionId"].ToString()),
                            WorkDescription = Convert.ToString(dr["WorkDescription"]),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstAD_WorkDescription);
        }
        public ActionResult ADDUpdateAD_WorkDescription(ADWorkDescription oAD_WorkDescription)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ADWorkDescription obj = new ADWorkDescription();



                DataTable dtf = obj.AddUpdateAD_WorkDescription(oAD_WorkDescription);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ADWorkDescription", new { RecordStatus = status });
        }
        public ActionResult GetAD_WorkDescription(string WorkDescriptionId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ADWorkDescription obj = new ADWorkDescription();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_AD_WorkDescription(Convert.ToInt16(WorkDescriptionId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ADWorkDescription
                    {
                        WorkDescriptionId = Convert.ToInt32(dr["WorkDescriptionId"].ToString()),
                        WorkDescription = Convert.ToString(dr["WorkDescription"]),
                        isActive = Convert.ToInt32(dr["isActive"] == "" ? 0 : dr["isActive"]),


                        OperationType = "Edit AD_WorkDescription"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialADWorkDescription", obj);
        }





        #endregion

        #region "ADCategory"
        public ActionResult ADCategory(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                ADCategory obj = new ADCategory();
                DataTable dtf = obj.Select_AD_Categorys();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    lstAD_Category.Add(
                        new ADCategory()
                        {
                            Index = count,
                            AwardCategoryId = Convert.ToInt32(dr["AwardCategoryId"].ToString()),
                            CategoryName = Convert.ToString(dr["CategoryName"]),
                            Amount = Convert.ToDecimal(dr["Amount"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(lstAD_Category);
        }
        public ActionResult ADDUpdateAD_Category(ADCategory oAD_Category)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                ADCategory obj = new ADCategory();



                DataTable dtf = obj.AddUpdateAD_Category(oAD_Category);

                status = dtf.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ADCategory", new { RecordStatus = status });
        }
        public ActionResult GetAD_Category(string AwardCategoryId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ADCategory obj = new ADCategory();
            try
            {

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
                DataTable dtf = obj.Select_AD_Category(Convert.ToInt16(AwardCategoryId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ADCategory
                    {
                        AwardCategoryId = Convert.ToInt32(dr["AwardCategoryId"].ToString()),
                        CategoryName = Convert.ToString(dr["CategoryName"]),
                        Amount = Convert.ToDecimal(dr["Amount"]),


                        OperationType = "Edit AD_Category"
                    };

                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialADCategory", obj);
        }





        #endregion
        #region All Email Module List

        public ActionResult AllEmailModuleList()
        {
            AllEmailModuleDetails model = new AllEmailModuleDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                AllEmailModuleRepository repo = new AllEmailModuleRepository();
                DataSet ds = new DataSet();
                #region Get List
                ds = repo.GetAllEmailList("List", model.AddEmailmodel);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    model.AddEmailList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AllEmailModule>>(str);
                }
                #endregion

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddEmailModuleList(AllEmailModuleDetails model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                AllEmailModuleRepository repo = new AllEmailModuleRepository();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(model.AddEmailmodel.ID) && model.AddEmailmodel.ID != "0")
                {
                    ds = repo.GetAllEmailList("UPDATE", model.AddEmailmodel);
                }
                else
                {
                    ds = repo.GetAllEmailList("INSERT", model.AddEmailmodel);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> Your Email Module has been " + ds.Tables[0].Rows[0]["Status"] + "  </div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return RedirectToAction("AllEmailModuleList");
        }

        public JsonResult GetAllEmailDetails(string ID)
        {
            AllEmailModuleDetails model = new AllEmailModuleDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                AllEmailModuleRepository repo = new AllEmailModuleRepository();
                DataSet ds = new DataSet();
                #region Get Particular Detail
                model.AddEmailmodel.ID = ID;
                ds = repo.GetAllEmailList("Details", model.AddEmailmodel);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model.AddEmailmodel.ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                    model.AddEmailmodel.ModuleName = Convert.ToString(ds.Tables[0].Rows[0]["ModuleName"]);
                    model.AddEmailmodel.EmailID = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]);
                    model.AddEmailmodel.SSOId = Convert.ToString(ds.Tables[0].Rows[0]["SSOId"]);
                    model.AddEmailmodel.Status = Convert.ToBoolean(ds.Tables[0].Rows[0]["Status"]);
                    model.AddEmailmodel.AdminMobileNumber = Convert.ToString(ds.Tables[0].Rows[0]["AdminMobileNumber"]);
                    model.AddEmailmodel.IsSendMailStatusCitizen = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSendMailStatusCitizen"]);
                    model.AddEmailmodel.AdminTemplate = Convert.ToString(ds.Tables[0].Rows[0]["AdminTemplate"]);
                    model.AddEmailmodel.CitizenTemplate = Convert.ToString(ds.Tables[0].Rows[0]["CitizenTemplate"]);
                    model.AddEmailmodel.AdminTemplateSMS = Convert.ToString(ds.Tables[0].Rows[0]["AdminTemplateSMS"]);
                    model.AddEmailmodel.CitizenTemplateSMS = Convert.ToString(ds.Tables[0].Rows[0]["CitizenTemplateSMS"]);
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(model.AddEmailmodel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Online Booking Pop Up by Rajveer
        public ActionResult AddOnlineBookingPopUp()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            OnlineBookingPopUpDetails obj = new OnlineBookingPopUpDetails();
            try
            {
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                DataSet ds = new DataSet();
                #region Get List
                ds = repo.GetAllOnlineBookingPopUpList("List", obj.model);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    obj.ModelList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OnlineBookingPopUp>>(str);
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOnlineBookingPopUp(OnlineBookingPopUpDetails data)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(data.model.ID) && data.model.ID != "0")
                {
                    ds = repo.GetAllOnlineBookingPopUpList("UPDATE", data.model);
                }
                else
                {
                    ds = repo.GetAllOnlineBookingPopUpList("INSERT", data.model);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> " + ds.Tables[0].Rows[0]["Status"] + "  </div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return RedirectToAction("AddOnlineBookingPopUp");
        }


        public JsonResult GetOnlineBookingPopUpDetails(string ID)
        {
            OnlineBookingPopUpDetails data = new OnlineBookingPopUpDetails();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                OnlineBookingPopUpRepository repo = new OnlineBookingPopUpRepository();
                DataSet ds = new DataSet();
                #region Get Particular Detail
                data.model.ID = ID;
                ds = repo.GetAllOnlineBookingPopUpList("Details", data.model);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    data.model.ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]);
                    data.model.ModuleName = Convert.ToString(ds.Tables[0].Rows[0]["ModuleName"]);
                    data.model.Content = Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                    data.model.Status = Convert.ToBoolean(ds.Tables[0].Rows[0]["Status"]);
                    data.model.Title = Convert.ToString(ds.Tables[0].Rows[0]["Title"]);
                }
                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return Json(data.model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Emitra Response Kiosk User
        public ActionResult EmitraTransactionLogsKioskUser()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            EmitraResponseKioskUserDetails model = new EmitraResponseKioskUserDetails();
            model.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            model.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            try
            {

                EmitraResponseKioskUserRepository obj = new EmitraResponseKioskUserRepository();

                DataSet DT = new DataSet();
                DT = obj.GetEmitraResponseKioskUserList("List", model);
                if (DT != null && DT.Tables.Count > 0)
                {
                    if (DT.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT.Tables[0]);
                        model.EmitraResponseList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmitraResponseKioskUser>>(str);
                    }
                    if (DT.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.ddlKioskUser = new SelectList(DT.Tables[1].AsDataView(), "UserID", "Name");
                    }
                    else
                    {
                        ViewBag.ddlKioskUser = null;
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
        public ActionResult EmitraTransactionLogsKioskUser(EmitraResponseKioskUserDetails model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                EmitraResponseKioskUserRepository obj = new EmitraResponseKioskUserRepository();
                DataSet DT = new DataSet();
                DT = obj.GetEmitraResponseKioskUserList("List", model);
                if (DT != null && DT.Tables.Count > 0)
                {
                    if (DT.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT.Tables[0]);
                        model.EmitraResponseList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmitraResponseKioskUser>>(str);
                    }
                    if (DT.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.ddlKioskUser = new SelectList(DT.Tables[1].AsDataView(), "UserID", "Name");
                    }
                    else
                    {
                        ViewBag.ddlKioskUser = null;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(model);

        }


        #endregion

        #region Restrict Online Zoo Booking Days
        public ActionResult RestrictOnlineZooBookingDays()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            RestrictOnlineZooBookingDetails model = new RestrictOnlineZooBookingDetails();
            try
            {
                RestrictOnlineZooBookingRepository obj = new RestrictOnlineZooBookingRepository();
                DataSet DT = new DataSet();
                DT = obj.RestrictOnlineZooBookingList("List", model.ModelData);
                if (DT != null && DT.Tables.Count > 0)
                {
                    if (DT.Tables[0].Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT.Tables[0]);
                        model.RestrictOnlineZooBookingModelList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RestrictOnlineZooBooking>>(str);
                    }
                    if (DT.Tables[1].Rows.Count > 0)
                    {
                        ViewBag.PlaceName = new SelectList(DT.Tables[1].AsDataView(), "PlaceID", "PlaceName");
                    }
                    else
                    {
                        ViewBag.PlaceName = null;
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
        public ActionResult RestrictOnlineZooBookingDays(RestrictOnlineZooBookingDetails model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                RestrictOnlineZooBookingRepository obj = new RestrictOnlineZooBookingRepository();
                DataSet ds = new DataSet();
                ds = obj.RestrictOnlineZooBookingList("Insert", model.ModelData);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> " + ds.Tables[0].Rows[0]["Status"] + "  </div>";

                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return RedirectToAction("RestrictOnlineZooBookingDays");

        }


        #endregion


        #region "EmitraKioskLinkDirectOrBypass Master"
        public ActionResult EmitraKioskLinkDirectOrBypass(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            EmitraKioskLinkDirectOrBypass obj = new EmitraKioskLinkDirectOrBypass();
            try
            {

                DataTable dtf = obj.Select_EmitraKioskLinkDirectOrBypass();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    EmitraKioskLinkDirectOrBypassLst.Add(
                        new EmitraKioskLinkDirectOrBypass()
                        {
                            Index = count,
                            ID = Convert.ToInt32(dr["Id"]),
                            SERVICENAME = Convert.ToString(dr["SERVICENAME"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }






            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(EmitraKioskLinkDirectOrBypassLst);
        }
        public ActionResult AddUpdateEmitraKioskLinkDirectOrBypass(EmitraKioskLinkDirectOrBypass ObjEmitraKioskLinkDirectOrBypass)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

                EmitraKioskLinkDirectOrBypass obj = new EmitraKioskLinkDirectOrBypass();

                DataTable dtf = obj.AddUpdateTicker(ObjEmitraKioskLinkDirectOrBypass);

                #region Email and SMS
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                string name = string.Empty;
                if (ObjEmitraKioskLinkDirectOrBypass.DIRECT_LINK_IsActive == 1)
                {
                    name = "[ " + ObjEmitraKioskLinkDirectOrBypass.SERVICENAME + " ] Direct Emitra Service";
                }
                else if (ObjEmitraKioskLinkDirectOrBypass.RAJSEVADWAR_LINK_IsActive == 1)
                {
                    name = "[ " + ObjEmitraKioskLinkDirectOrBypass.SERVICENAME + " ] Raj Seva Dwar Emitra Service";
                }

                objSMSandEMAILtemplate.SendMailComman("ALL", "EmitraKioskServiceBypass", "", name, "", "", ""); //Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])

                #endregion


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("EmitraKioskLinkDirectOrBypass", new { RecordStatus = status });
        }
        public ActionResult GetEmitraKioskLinkDirectOrBypass(string Id)
        {

            EmitraKioskLinkDirectOrBypass obj = new EmitraKioskLinkDirectOrBypass();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                ViewBag.OpType = (Id == "0" ? "Add" : "Edit");

                DataTable dtf = obj.Select_EmitraKioskLinkDirectOrBypass(Convert.ToInt32(Id));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new EmitraKioskLinkDirectOrBypass
                    {

                        ID = Convert.ToInt32(dr["ID"]),
                        SERVICENAME = Convert.ToString(dr["SERVICENAME"].ToString()),
                        DIRECT_LINK = Convert.ToString(dr["DIRECT_LINK"].ToString()),
                        DIRECT_LINK_IsActive = Convert.ToInt16(dr["DIRECT_LINK_IsActive"]),
                        RAJSEVADWAR_LINK = Convert.ToString(dr["RAJSEVADWAR_LINK"].ToString()),
                        RAJSEVADWAR_LINK_IsActive = Convert.ToInt16(dr["RAJSEVADWAR_LINK_IsActive"]),
                        MAX_RESPONSE_TIME_SEC = Convert.ToInt16(dr["MAX_RESPONSE_TIME_SEC"]),
                        IsActive = Convert.ToInt16(dr["IsActive"]),

                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.lstISactive = lstISactive;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialEmitraKioskLinkDirectOrBypass", obj);
        }

        #endregion

        #region "VIP BOOKING"


        public ActionResult EqptSanctuariesFeeVip(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            EqptSanctuariesFee obj = new EqptSanctuariesFee();
            try
            {
                DataTable dtf = obj.Select_EqptSanctuariesFeesVip();
                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    EqptSanctuariesFeeLst.Add(
                           new EqptSanctuariesFee()
                           {
                               Index = count,
                               ID = Convert.ToInt64(dr["ID"]),
                               PlaceName = Convert.ToString(dr["PlaceName"]),
                               ZoneName = Convert.ToString(dr["ZoneName"]),
                               CategoryName = Convert.ToString(dr["CategoryName"]),
                               Name = Convert.ToString(dr["Name"]),
                               ShiftType = Convert.ToString(dr["ShiftType"]),
                               TotalFee = Convert.ToDecimal(dr["TotalFee"]),
                               IsactiveView = Convert.ToBoolean(dr["IsActive"]),
                           });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(EqptSanctuariesFeeLst);
        }
        public ActionResult ADDUpdateEqptSanctuariesFeeVip(EqptSanctuariesFee oVehicleEquipment)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                EqptSanctuariesFee obj = new EqptSanctuariesFee();
                oVehicleEquipment.EnteredBy = Convert.ToString(UserID);

                DataTable dtf = obj.AddUpdateEqptSanctuariesFeeVip(oVehicleEquipment);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("EqptSanctuariesFeeVip", new { RecordStatus = status });
        }

        public ActionResult GetEqptSanctuariesFeeVip(string ID)
        {

            EqptSanctuariesFee obj = new EqptSanctuariesFee();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                List<SelectListItem> CategoryIDs = new List<SelectListItem>();
                List<SelectListItem> PlaceIDs = new List<SelectListItem>();
                List<SelectListItem> ZoneIDs = new List<SelectListItem>();
                List<SelectListItem> ShiftTypes = new List<SelectListItem>();


                DataTable dt = obj.SelectAllVehicleEquipmentCategoryVip();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    CategoryIDs.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                dt = obj.SelectAllPlaces();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    PlaceIDs.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                }

                ViewBag.OpType = (ID == "0" ? "Add Equipment Sanctuaries Fee" : "Edit Equipment Sanctuaries Fee");

                DataTable dtf = obj.Select_EqptSanctuariesFeeVip(Convert.ToInt32(ID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new EqptSanctuariesFee
                    {
                        ID = Convert.ToInt64(dr["ID"]),
                        PlaceID = Convert.ToInt64(dr["PlaceID"]),
                        ZoneID = Convert.ToInt64(dr["ZoneID"]),
                        CategoryID = Convert.ToInt64(dr["CategoryID"]),
                        Name = Convert.ToString(dr["Name"]),
                        ShiftType = Convert.ToString(dr["ShiftType"]),
                        TotalEqptAvailability = Convert.ToInt32(dr["TotalEqptAvailability"]),
                        SeatsPerEqpt = Convert.ToInt32(dr["SeatsPerEqpt"]),
                        seatsForCitizen = Convert.ToInt32(dr["seatsForCitizen"]),
                        TotalSeats = Convert.ToInt32(dr["TotalSeats"]),
                        Fee_TigerProject = Convert.ToInt32(dr["Fee_TigerProject"]),
                        Fee_Surcharge = Convert.ToDecimal(dr["Fee_Surcharge"]),
                        TotalFee = Convert.ToDecimal(dr["TotalFee"]),
                        IsActive = Convert.ToInt16(dr["IsActive"]),


                        OperationType = "Edit Vehicle Equipment"

                    };

                }

                if (ID != "0")
                {
                    dt = obj.SelectAllZone(Convert.ToInt32(obj.PlaceID));
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        ZoneIDs.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                    }
                }

                // ShiftTypes.Add(new SelectListItem { Text = "Morning", Value = "1" });
                //ShiftTypes.Add(new SelectListItem { Text = "Evening", Value = "2" });
                //ShiftTypes.Add(new SelectListItem { Text = "Fullday", Value = "3" });
                //ShiftTypes.Add(new SelectListItem { Text = "HalfDay", Value = "4" });

                //ViewBag.ShiftList = ShiftTypes;


                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });


                ShiftTypes.Add(new SelectListItem { Text = "Morning", Value = "1" });
                ShiftTypes.Add(new SelectListItem { Text = "Evening ", Value = "2" });
                ShiftTypes.Add(new SelectListItem { Text = "Full Day ", Value = "3" });
                ShiftTypes.Add(new SelectListItem { Text = "Half Day ", Value = "4" });


                ViewBag.ISactivelst = lstISactive;
                ViewBag.CategoryIDLst = CategoryIDs;
                ViewBag.PlaceIDLst = PlaceIDs;
                ViewBag.ZoneIDLst = ZoneIDs;

                ViewBag.ShiftTypesLst = ShiftTypes;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialEqptSanctuariesFeeVip", obj);
        }
        public ActionResult DeleteEqptSanctuariesFeeVip(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                EqptSanctuariesFee obj = new EqptSanctuariesFee();
                obj.ID = id;
                obj.DeleteEqptSanctuariesFeeVip();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("EqptSanctuariesFeeVip");
        }
        public JsonResult ZoneByPlaceVip(int PlaceID)
        {
            EqptSanctuariesFee obj = new EqptSanctuariesFee();
            List<SelectListItem> ZoneS = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                DataTable dt = new DataTable();
                dt = obj.SelectAllZone(PlaceID);
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    ZoneS.Add(new SelectListItem { Text = @dr["ZoneName"].ToString(), Value = @dr["ZoneID"].ToString() });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(ZoneS, "Value", "Text"));
        }

        public JsonResult CheckDuplicateForEqptSanctuariesFeeVip(int ID, int PlaceID, int CategoryID, string Name)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                EqptSanctuariesFee VehicleEquipments = new EqptSanctuariesFee();
                VehicleEquipments.ID = ID;
                VehicleEquipments.PlaceID = PlaceID;
                VehicleEquipments.CategoryID = CategoryID;
                VehicleEquipments.Name = Name;
                status = VehicleEquipments.Check_DuplicateRecordVip();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }

        #endregion

        #region "Vehicle Master"
        public ActionResult VehicleMaster()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            VehicleMasterModel model = new VehicleMasterModel();
            VehicleMasterRepo repo = new VehicleMasterRepo();
            try
            {
                DataTable dt = repo.Select_VehicleList(UserID);
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehicleMaster>>(str);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult VehicleMaster(VehicleMaster model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                VehicleMasterRepo obj = new VehicleMasterRepo();
                DataTable dtf = obj.AddUpdate_Vehicle(model, UserID);
                status = dtf.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(status))
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> " + status + "</div>";
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("VehicleMaster");
        }

        public ActionResult VehicleMasterDetails(long ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            VehicleMasterModel model = new VehicleMasterModel();
            VehicleMasterRepo repo = new VehicleMasterRepo();
            try
            {
                DataSet dt = repo.Select_VehicleDetails(Convert.ToInt64(ID), UserID);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VehicleMaster>>(str);
                    model.Model = model.List.LastOrDefault();
                }
                string stringplace = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[1]);
                ViewBag.PlaceList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(stringplace);


                stringplace = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[2]);
                ViewBag.VehicleTypeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(stringplace);

                stringplace = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[3]);
                ViewBag.ISactivelst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(stringplace);
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialVehicleMasterDetails", model.Model);
        }




        #endregion

        #region Forest Fire Excel Data Upload

        public ActionResult ForestFire()
        {
            TempData["msg"] = TempData["msg1"];
            TempData["isError1"] = TempData["isError"];
            ViewBag.ReturnMsg = TempData["msg"];
            ViewBag.IsError = TempData["isError1"];
            List<ForestFireModel> oList = GetForestFireData();
            return View("ForestFire", oList);
        }
        [HttpPost]
        public ActionResult ForestFire(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                if (postedFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(postedFile.FileName);
                    string withouex = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string excelfile = withouex + "_" + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss") + extension;
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        string path = Server.MapPath("~/ExcelSheetsForestFire/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(postedFile.FileName);
                        //filePath = path + excelfile;
                        postedFile.SaveAs(filePath);

                        string conString = string.Empty;

                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (extension == ".xls")
                        {
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
                        }
                        //connection String for xlsx file format.
                        else if (extension == ".xlsx")
                        {
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                        }
                        DataTable dt = new DataTable();
                        SqlDataReader reader;
                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);

                                    connExcel.Close();
                                }
                            }
                        }
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                        cnn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_ForestFireExcelData";
                        cmd.Connection = cnn;
                        cmd.Parameters.AddWithValue("@ActionCode", 1);
                        cmd.Parameters.Add("@Typ_ForestFireExcelData", SqlDbType.Structured).SqlValue = dt;
                        //string result = cmd.ExecuteNonQuery().ToString();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            bool isError = Convert.ToBoolean(reader["IsError"]);
                            string ReturnMessage = Convert.ToString(reader["ReturnMessage"]);
                            TempData["msg1"] = ReturnMessage;
                            TempData["isError"] = isError;
                        }
                        reader.Close();
                    }
                    else
                    {
                        TempData["msg1"] = "The uploaded file is not Excel file.";
                        TempData["isError"] = 1;
                        return RedirectToAction("ForestFire");
                    }
                }
                else
                {
                    TempData["msg1"] = "The Excel have no data.";
                    TempData["isError"] = 1;
                    return RedirectToAction("ForestFire");
                }
            }
            else
            {
                TempData["msg1"] = "The file is not attached.";
                TempData["isError"] = 1;
                return RedirectToAction("ForestFire");
            }
            return RedirectToAction("ForestFire");
        }

        public List<ForestFireModel> GetForestFireData()
        {
            List<ForestFireModel> oObj = new List<ForestFireModel>();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            DataTable dt = new DataTable();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ForestFireModel Content = new ForestFireModel();
                    Content.SNo = Convert.ToString(dt.Rows[i]["SNo"]);
                    Content.Fire_Date = Convert.ToString(dt.Rows[i]["Fire_Date"]);
                    Content.Fire_Time = Convert.ToString(dt.Rows[i]["Fire_Time"]);
                    Content.Source = Convert.ToString(dt.Rows[i]["Source"]);
                    Content.Latitude = Convert.ToString(dt.Rows[i]["Latitude"]);
                    Content.Longitude = Convert.ToString(dt.Rows[i]["Longitude"]);
                    Content.State = Convert.ToString(dt.Rows[i]["State"]);
                    Content.District = Convert.ToString(dt.Rows[i]["District"]);
                    Content.Circle = Convert.ToString(dt.Rows[i]["Circle"]);
                    Content.Division = Convert.ToString(dt.Rows[i]["Division"]);
                    Content.Range = Convert.ToString(dt.Rows[i]["Range"]);
                    Content.Block = Convert.ToString(dt.Rows[i]["Block"]);
                    Content.Beat = Convert.ToString(dt.Rows[i]["Beat"]);
                    oObj.Add(Content);
                }
                return oObj;


            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
            return oObj;
        }
        #endregion

        #region "TicketingFeesVip"
        public ActionResult TicketingFeesVip(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                TicketingFee obj = new TicketingFee();
                DataTable dtf = obj.Select_PlacesVip();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ticket.Add(
                        new TicketingFee()
                        {
                            Index = count,
                            FeesID = Convert.ToInt64(dr["FeesID"].ToString()),
                            DIST_CODE = Convert.ToString(dr["DIST_CODE"]),
                            DisNAme = Convert.ToString(dr["DIST_NAME"]),
                            PlaceName = Convert.ToString(dr["PlaceName"]),
                            IsactiveView = Convert.ToBoolean(dr["Isactive"]),
                        });
                    count += 1;
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ticket);
        }
        public ActionResult ADDUpdateTicketVip(TicketingFee oPlace)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string status = null;
            try
            {
                TicketingFee obj = new TicketingFee();
                oPlace.EnteredBy = UserID;
                oPlace.UpdatedBy = UserID;
                DataTable dtf = obj.AddUpdatePlaceVip(oPlace);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("TicketingFeesVip", new { RecordStatus = status });
        }
        public ActionResult GetTicketVip(string FeesID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            TicketingFee obj = new TicketingFee();

            try
            {
                List<SelectListItem> District = new List<SelectListItem>();

                List<SelectListItem> items = new List<SelectListItem>();

                List<SelectListItem> Places = new List<SelectListItem>();

                List<SelectListItem> ShiftTypes = new List<SelectListItem>();


                DataTable dt = _objLocation.District();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }

                ViewBag.OpType = (FeesID == "0" ? "Add Ticket" : "Edit Ticket");


                DataTable dtf = obj.Select_PlaceVip(Convert.ToInt32(FeesID));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new TicketingFee
                    {
                        PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                        FeesID = Convert.ToInt64(dr["FeesID"].ToString()),
                        DIST_CODE = Convert.ToString(dr["DIST_CODE"]),

                        IndianAdultFees_Surcharge = Convert.ToDecimal(dr["IndianAdultFees_Surcharge"]),
                        IndianAdultFees_TigerProject = Convert.ToDecimal(dr["IndianAdultFees_TigerProject"]),
                        Foreigner_Surcharge = Convert.ToDecimal(dr["Foreigner_Surcharge"]),
                        Foreigner_TigerProject = Convert.ToDecimal(dr["Foreigner_TigerProject"]),
                        Indian_TRDF = Convert.ToDecimal(dr["Indian_TRDF"]), // canter TRDF
                        Foreigner_TRDF = Convert.ToDecimal(dr["Foreigner_TRDF"]), // canter TRDF

                        Indian_TRDF_Gypsy = Convert.ToDecimal(dr["Indian_TRDF_Gypsy"]),
                        Foreigner_TRDF_Gypsy = Convert.ToDecimal(dr["Foreigner_TRDF_Gypsy"]),


                        GuideFees = Convert.ToDecimal(dr["GuideFees"]),
                        CameraFeesForeigner_TigerProject = Convert.ToDecimal(dr["CameraFeesForeigner_TigerProject"]),
                        CameraFeesForeigner_Surcharge = Convert.ToDecimal(dr["CameraFeesForeigner_Surcharge"]),
                        CameraFeesIndian_TigerProject = Convert.ToDecimal(dr["CameraFeesIndian_TigerProject"]),
                        CameraFeesIndian_Surcharge = Convert.ToDecimal(dr["CameraFeesIndian_Surcharge"]),
                        Isactive = Convert.ToInt32(dr["Isactive"]),
                        StudentFees = Convert.ToDecimal(dr["StudentFees"]),


                        SingleOccupancyFees = Convert.ToDecimal(dr["SingleOccupancyFees"]),
                        DoubleOccupancyFees = Convert.ToDecimal(dr["DoubleOccupancyFees"]),

                        //Rajveer
                        Indian_GypsyVehicleRentFees = Convert.ToDecimal(dr["Indian_GypsyVehicleRentFees"]),
                        Indian_CanterVehicleRentFees = Convert.ToDecimal(dr["Indian_CanterVehicleRentFees"]),
                        Indian_GypsyGuideFee = Convert.ToDecimal(dr["Indian_GypsyGuideFee"]),
                        Indian_CanterGuideFee = Convert.ToDecimal(dr["Indian_CanterGuideFee"]),

                        Foreigner_GypsyVehicleRentFees = Convert.ToDecimal(dr["Foreigner_GypsyVehicleRentFees"]),
                        Foreigner_CanterVehicleRentFees = Convert.ToDecimal(dr["Foreigner_CanterVehicleRentFees"]),
                        Foreigner_GypsyGuideFee = Convert.ToDecimal(dr["Foreigner_GypsyGuideFee"]),
                        Foreigner_CanterGuideFee = Convert.ToDecimal(dr["Foreigner_CanterGuideFee"]),

                        GSTonGuideFee = Convert.ToDecimal(dr["GSTonGuideFee"]),
                        GSTonVehicleRentFee = Convert.ToDecimal(dr["GSTonVehicleRentFee"]),

                        Vehicle_TRDF_Gypsy = Convert.ToDecimal(dr["Vehicle_TRDF_Gypsy"]),
                        Vehicle_TRDF_Canter = Convert.ToDecimal(dr["Vehicle_TRDF_Canter"]),


                        GuidFee_TRDF_Indian_Gypsy = Convert.ToDecimal(dr["GuidFee_TRDF_Indian_Gypsy"]),
                        GuidFee_TRDF_NonIndian_Gypsy = Convert.ToDecimal(dr["GuidFee_TRDF_NonIndian_Gypsy"]),
                        GuidFee_TRDF_Indian_Canter = Convert.ToDecimal(dr["GuidFee_TRDF_Indian_Canter"]),
                        GuidFee_TRDF_NonIndian_Canter = Convert.ToDecimal(dr["GuidFee_TRDF_NonIndian_Canter"]),


                        //End
                        OperationType = "Edit Ticket",
                        ShiftType = Convert.ToString(dr["ShiftType"]),
                    };


                }

                if (ViewBag.OpType == "Edit Ticket")
                {
                    dt = obj.SelectPlaceWithCategory(obj.DIST_CODE);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr1 in ViewBag.fname.Rows)
                    {
                        Places.Add(new SelectListItem { Text = @dr1["PlaceName"].ToString(), Value = @dr1["PlaceID"].ToString() });
                    }
                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ShiftTypes.Add(new SelectListItem { Text = "Morning", Value = "1" });
                ShiftTypes.Add(new SelectListItem { Text = "Evening ", Value = "2" });
                ShiftTypes.Add(new SelectListItem { Text = "Full Day ", Value = "3" });
                ShiftTypes.Add(new SelectListItem { Text = "Half Day ", Value = "4" });
                ViewBag.ShiftTypesLst = ShiftTypes;

                ViewBag.ISactivelst = lstISactive;

                ViewBag.ddlDistrict1 = District;
                ViewBag.ddlPlace1 = Places;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialticketVip", obj);
        }
        public ActionResult DeleteTicketVip(int id)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                TicketingFee obj = new TicketingFee();
                obj.FeesID = id;
                obj.DeletePlace();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("TicketingFees");
        }
        public JsonResult PlaceByDistrictVip(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {
                TicketingFee cst = new TicketingFee();
                DataTable dt = new DataTable();

                dt = cst.SelectPlaceWithCategoryVip(Convert.ToString(districtID));
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(new SelectList(Places, "Value", "Text"));
        }

        public JsonResult CheckDuplicateForTicketVip(int FeesID, int PlaceID, string DIST_CODE)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            bool status = false;
            try
            {
                TicketingFee TicketingFees = new TicketingFee();
                TicketingFees.FeesID = FeesID;
                TicketingFees.PlaceID = PlaceID;
                TicketingFees.DIST_CODE = DIST_CODE;


                status = TicketingFees.Check_DuplicateRecordVip();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return Json(status);
        }


        #endregion


        #region "Resuce Vendor List"
        public ActionResult VendorList(string Type = "Rescue")
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            WaterFireModelData model = new WaterFireModelData();
            DataTable DT = new DataTable();
            WaterFireModel repo = new WaterFireModel();

            try
            {

                DT = repo.GETVendorList("GETRescueVendorList", Type, UserID);
                if (DT != null && DT.Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(DT);
                    model.List= Newtonsoft.Json.JsonConvert.DeserializeObject<List<WaterFireModel>>(str);
                }

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(model);
        }
        #endregion


        #region "App Setting"

        public ActionResult APPMaster()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            cls_AppSetting model = new cls_AppSetting();
            DataTable dt = new DataTable();
            try
            {
                dt = model.Select_AppSettingList();

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cls_AppSetting>>(str);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult APPMaster(cls_AppSetting model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                cls_AppSetting obj = new cls_AppSetting();
                DataTable dtf = obj.AddUpdate_AppDetails(model);
                status = dtf.Rows[0][0].ToString();
                if (string.IsNullOrEmpty(status))
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
                }
                else
                {
                    TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> " + status + "</div>";
                    #region Email and SMS
                    SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                    string sAppSetting = string.Empty;

                    sAppSetting = "<table style='background-repeat:no-repeat; width:450px;margin:0;' cellpadding='0' cellspacing='0' border='0'><thead><tr>";
                    sAppSetting += "<th>SettingType</th><th>AppKey</th><th>AppValue<th><th>Description</th><th>ActiveStatus</th></thead></tr>";
                    sAppSetting += "<tbody><tr class='odd'><td>" + model.SettingType + "</td><td>" + model.AppKey + "</td><td>" + model.AppValue + "</td><td>" + model.Description + "</td><td>" + (model.ActiveStatus == true ? "Active" : "DeActive") + "</td></tbody></tr>";
                    sAppSetting += "</table>";

                    objSMSandEMAILtemplate.SendMailComman("ALL", "App Setting", sAppSetting, sAppSetting, "", "", "");//Convert.ToString(DT.Rows[0]["ApplicantEmail"])  Convert.ToString(DT.Rows[0]["ApplicantMobile"])
                    #endregion
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("APPMaster");
        }

        public ActionResult AppMasterDetails(long ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            cls_AppSetting model = new cls_AppSetting();
            try
            {
                DataSet dt = model.Select_AppDetails(ID);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
                    model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cls_AppSetting>>(str);
                    model.Model = model.List.LastOrDefault();
                }

                List<SelectListItem> list = new List<SelectListItem>() { new SelectListItem { Text = "Active", Value = "true" }, new SelectListItem { Text = "DeActive", Value = "false" } };


                ViewBag.ISactivelst = list;
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialAppSettingMaster", model.Model);
        }

        #endregion


        //#region "Site Master"

        //public ActionResult SiteMaster()
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    cls_SiteName model = new cls_SiteName();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = model.Select_SiteList();

        //        string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        //        model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cls_SiteName>>(str);

        //    }

        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult SiteMaster(cls_SiteName model)
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    string status = null;
        //    try
        //    {
        //        cls_SiteName obj = new cls_SiteName();
        //        DataTable dtf = obj.AddUpdate_Site(model);
        //        status = dtf.Rows[0][0].ToString();
        //        if (string.IsNullOrEmpty(status))
        //        {
        //            TempData["Message"] = "<div id='divSuccess' class='alert alert-danger'><i class='fa fa-thumbs-o-up fa-fw'></i>कुछ त्रुटि हुई है कृपया बाद में फिर से प्रयास करें ! <br/> (Some Error Occur Please try again later)</div>";
        //        }
        //        else
        //        {
        //            TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i> " + status + "</div>";

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return RedirectToAction("SiteMaster");
        //}



        //public ActionResult SiteDetails(long ID)
        //{

        //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"]);
        //    cls_SiteName model = new cls_SiteName();
        //    model.Model = new cls_SiteName();
        //    try
        //    {
        //        DataSet dt = model.Select_SiteDetails(ID);
        //        if (dt != null && dt.Tables[0].Rows.Count > 0)
        //        {
        //            string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
        //            model.List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<cls_SiteName>>(str);
        //            model.Model = model.List.LastOrDefault();
        //        }

        //        FmdssContext fmdsscontext = new FmdssContext();
        //        List<SelectListItem> lstCircle = new List<SelectListItem>();
        //        List<SelectListItem> lstDivision = new List<SelectListItem>();
        //        List<SelectListItem> Range = new List<SelectListItem>();
        //        var Circle = fmdsscontext.tbl_mst_Forest_WildLifeCircles.Where(s => s.isBOTH == 1).Select(i => new { i.CIRCLE_NAME, i.CIRCLE_CODE }).OrderBy(s => s.CIRCLE_NAME);
        //        foreach (var item in Circle)
        //        {
        //            lstCircle.Add(new SelectListItem { Text = item.CIRCLE_NAME, Value = Convert.ToString(item.CIRCLE_CODE) });
        //        }

        //        ViewBag.Circle = lstCircle;
        //        ViewBag.Div = lstDivision;
        //        ViewBag.Range = Range;

        //        List<SelectListItem> list = new List<SelectListItem>() { new SelectListItem { Text = "Active", Value = "true" }, new SelectListItem { Text = "DeActive", Value = "false" } };
        //        ViewBag.ISactivelst = list;
        //    }

        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

        //    }
        //    return PartialView("_partialSite", model);
        //}



        //#endregion

       
    }

    public class RowStatus
    {
        public string RStatus { get; set; }
    }
}
