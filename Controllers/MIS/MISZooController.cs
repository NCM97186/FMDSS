using FMDSS.Models;
using FMDSS.Models.Master;
using FMDSS.Models.MIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using FMDSS.Models.BookOnlineZoo;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.BookOnlineTicket;

namespace FMDSS.Controllers.MIS
{
    public class MISZooController : BaseController
    {
        List<MISZooCommonModel> ListZooTicketBookingDetails = new List<MISZooCommonModel>();
        List<SelectListItem> lstPlace = new List<SelectListItem>();

        public void onloadZooTicketBooking()
        {

            List<SelectListItem> Places = new List<SelectListItem>();
            List<SelectListItem> lstModeOfBooking = new List<SelectListItem>();
            List<SelectListItem> lstTRNS_Status = new List<SelectListItem>();

            List<SelectListItem> lstReportType = new List<SelectListItem>();


            List<SelectListItem> DATEtYPE = new List<SelectListItem>();

            List<SelectListItem> lstBookingID = new List<SelectListItem>();
            List<SelectListItem> lstUserlist = new List<SelectListItem>();

            List<SelectListItem> lstShiftType = new List<SelectListItem>();


            MISZooCommonModel cst = new MISZooCommonModel();

            DataTable dtPlace = new DataTable();
            dtPlace = cst.GetPlaces(Convert.ToInt64(Session["UserID"]));
            foreach (System.Data.DataRow dr in dtPlace.Rows)
            {
                Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
            }



            lstModeOfBooking.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            lstModeOfBooking.Add(new SelectListItem { Text = "Counter Booking", Value = "1" });
            lstModeOfBooking.Add(new SelectListItem { Text = "Online Booking", Value = "0" });
            lstModeOfBooking.Add(new SelectListItem { Text = "Emitra Kiosk Booking", Value = "2" });
            lstModeOfBooking.Add(new SelectListItem { Text = "Emitra-Plus", Value = "3" });

            DATEtYPE.Add(new SelectListItem { Text = "Date of Visit", Value = "DATEOFVISITE" });
            DATEtYPE.Add(new SelectListItem { Text = "Date of Booking", Value = "DATEOFBOOKING" });

            lstTRNS_Status.Add(new SelectListItem { Text = "All", Value = "-1" });
            lstTRNS_Status.Add(new SelectListItem { Text = "Success", Value = "1" });
            lstTRNS_Status.Add(new SelectListItem { Text = "Failed", Value = "0" });

            lstReportType.Add(new SelectListItem { Text = "Summary", Value = "Summary" });
            lstReportType.Add(new SelectListItem { Text = "Details", Value = "Details" });


            lstBookingID.Add(new SelectListItem { Text = "All", Value = "All" });
            lstBookingID.Add(new SelectListItem { Text = "Standard", Value = "Standard" });
            lstBookingID.Add(new SelectListItem { Text = "Institutional", Value = "Institutional" });

            lstUserlist.Add(new SelectListItem { Text = "All", Value = "All" });
            lstUserlist.Add(new SelectListItem { Text = "Self", Value = "Self" });


            lstShiftType.Add(new SelectListItem { Text = "All", Value = "All" });
            lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
            lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });
            lstShiftType.Add(new SelectListItem { Text = "Full Day", Value = "3" });



            ViewBag.ddlPlace1 = Places;
            ViewBag.ddlDATEtYPE1 = DATEtYPE;

            ViewBag.ddlTRNS_Status1 = lstTRNS_Status;

            ViewBag.ddlModeOfBooking = lstModeOfBooking;
            ViewBag.ddlReportType1 = lstReportType;

            ViewBag.lstBookingID1 = lstBookingID;
            ViewBag.lstUserlist1 = lstUserlist;
            ViewBag.lstShiftType1 = lstShiftType;

        }

        public ActionResult ZooTicketBookingDetails()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                onloadZooTicketBooking();
                //ViewData["ListZooTicketBookingDetails"] = ListZooTicketBookingDetails;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(new MISZooCommonModel { DownloadExcel = "False"});

        }


        [HttpPost]
        public ActionResult ZooTicketBookingDetails(MISZooCommonModel MIS)
        {
            DateTime startTime = DateTime.Now;
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            onloadZooTicketBooking();
            MISZooCommonModel obj = new MISZooCommonModel();
            DataTable DT;
            List<MISZooDetailReport> lstReport = new List<MISZooDetailReport>();
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                MIS.SSOIDType = "Self";

            }
            else
            {
                MIS.SSOIDType = "All";
            }
            DT = obj.GetZooTicketBookingDetails(MIS.FromDate, MIS.ToDate, MIS.Place, MIS.DATETYPE, MIS.TRNS_Status, MIS.ModeOfBooking, MIS.BookingType, MIS.SSOIDType, UserID, MIS.SHIFT_TYPE);
            if (MIS.DownloadExcel == "False")
            {
                try
                {
                    

                    int count = 1;
                    //Session["ZooTicketBookingDownload"] = DT;

                    foreach (DataRow dr in DT.Rows)
                    {
                        lstReport.Add(
                            new MISZooDetailReport()
                            {
                                Index = count,
                                RequestId = Convert.ToString(dr["RequestId"].ToString()),
                                BookingType = Convert.ToString(dr["BookingType"]),
                                DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                                DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                                NameofInstituteandOrganization = Convert.ToString(dr["NameofInstituteAndOrganization"]),

                                PLACE_NAME = Convert.ToString(dr["PlaceName"]),
                                TotalMember = Convert.ToString(dr["TotalMember"]),
                                TotalCamera = Convert.ToString(dr["TotalCamera"]),
                                VehicleType = Convert.ToString(dr["VehicleType"]),
                                SHIFT_TYPE = Convert.ToString(dr["ShiftType"]),

                                IsPrivateVehical = Convert.ToString(dr["IsPrivateVehical"]),
                                TotalVehicalFees = Convert.ToString(dr["TotalVehicalFees"]),
                                TotalMemberFees = Convert.ToString(dr["TotalMemberFees"]),
                                TotalCameraFees = Convert.ToString(dr["TotalCameraFees"]),
                                TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                                EmitraTransactionId = Convert.ToString(dr["EmitraTransactionId"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),

                                IP_Address = Convert.ToString(dr["IP_Address"]),
                                ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),
                                TicketStatus = Convert.ToString(dr["CheckStatus"])


                            });
                        count += 1;
                    }
                    string SSOID = Session["SSOid"].ToString();
                    MIS.MISZooDetailReport = lstReport;
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                }
                DateTime endTime = DateTime.Now;
                MIS.TimeDifference = (endTime - startTime).TotalSeconds.ToString();
                return View(MIS);
            }
            else {
                DownloadExcel(DT, "ZooTicketBookingDetails");
                return null;
            }
        }

        private void DownloadExcel(DataTable DT, string FileName)
        {
            GridView gv = new GridView();
            gv.DataSource = DT;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName + "_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public ActionResult ZooTicketBookingExport()
        {
            DataTable dtf = (DataTable)Session["ZooTicketBookingDownload"];
            dtf.Columns.Remove("Trn_Status_Code");
            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ZooTicketBookingDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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


        public ActionResult ZooHeadWiseDepositDetailAndSummary()
        {

            onloadZooTicketBooking();
            ViewBag.ListData = "";
            return View(new MISZooCommonModel { DownloadExcel="False" });

        }

        [HttpPost]
        public ActionResult ZooHeadWiseDepositDetailAndSummary(MISZooCommonModel MIS)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID  = Convert.ToInt64(Session["UserID"].ToString());
            onloadZooTicketBooking();
            DateTime startTime = DateTime.Now;
            StringBuilder SB = new StringBuilder();
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                MIS.SSOIDType = "Self";

            }
            else
            {
                MIS.SSOIDType = "All";
            }

            MISZooCommonModel obj = new MISZooCommonModel();
            DataTable DT;
            DT = obj.GetZooHeadWiseDepositDetailAndSummary(MIS.ReportType, MIS.FromDate, MIS.ToDate, MIS.Place, MIS.ModeOfBooking, MIS.SSOIDType, UserID, MIS.SHIFT_TYPE);

            if (MIS.DownloadExcel == "False")
            {
                try
                {

                    int count = 1;
                    //Session["ZooHeadWiseDepositDetailAndSummaryDownload"] = DT;

                    if (DT.Rows.Count > 0)
                    {
                        if (MIS.ReportType == "MIS.ReportType")
                        {
                            DT.Columns.Remove("ZooBookingId");
                            DT.Columns.Remove("RequestId1");
                        }
                        SB.Append("<table class='table table-striped table-bordered table-hover table-responsive gridtable'><thead><tr>");
                        SB.Append("<th>#</th>");
                        foreach (DataColumn DC in DT.Columns)
                        {
                            SB.Append("<th>" + DC.ColumnName + "</th>");

                        }

                        SB.Append("</tr></thead><tbody>");


                        foreach (DataRow DR in DT.Rows)
                        {
                            SB.Append("<tr>");
                            SB.Append("<td>" + count + "</td>");

                            foreach (DataColumn column in DT.Columns)
                            {

                                SB.Append("<td>" + Convert.ToString(DR[column] == null ? "" : DR[column]) + "</td>");
                            }

                            //foreach (string column in DR.ItemArray)
                            //{
                            //    SB.Append("<th>" + Convert.ToString(column == null ? "" : column) + "</th>"); 
                            //}
                            count = count + 1;
                            SB.Append("</tr>");
                        }


                        SB.Append("</tbody> </table> ");

                        ViewBag.ListData = SB.ToString();
                    }
                    else
                    {
                        ViewBag.ListData = "";
                    }
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
                }
                DateTime endTime = DateTime.Now;
                MIS.TimeDifference = (endTime - startTime).TotalSeconds.ToString();
                return View(MIS);
            }
            else
            {
                DownloadExcel(DT, "ZooTicketBookingDetails");
                return null;
            }
        }

        public ActionResult ZooHeadWiseDepositDetailAndSummaryExport()
        {
            DataTable dtf = (DataTable)Session["ZooHeadWiseDepositDetailAndSummaryDownload"];

            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ZooTicketBookingDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
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

        #region "ZooTicketVerification"

        [NonAction]

        public void getDeptplace()
        {
            //*******************For Departmental Kiosk User********************************
            if (Convert.ToString(Session["IsDepartmentalKioskUser"]) == "True")
            {
                #region DeptKioskUser
                BookOnTicket bkIsDept = new BookOnTicket();
                DataTable dtkiosk = new DataTable();
                string result = string.Empty;
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    result = ipRange[0];
                }
                else
                {
                    result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                if (dtkiosk.Rows.Count > 0)
                {
                    if (dtkiosk.Rows[0][0].ToString() == "0")
                    {
                        TempData["ValidIPAddress"] = "Invalid IP Address!!!";

                    }

                }

                FMDSS.Models.OnlineBooking.CS_BoardingDetails obj2 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                DataSet Ds = new DataSet();

                DataTable DT_BoardingDuration;


                Ds = obj2.BindDptKioskPLACES(Session["SSOid"].ToString());

                CS_BoardingDetails objB = new CS_BoardingDetails();

                DateTime DateTimes = DateTime.Now;
                int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

                DT_BoardingDuration = objB.GetBoardingDuration(Ds.Tables[0].Rows[0]["PlaceID"].ToString());
                if (DT_BoardingDuration.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsFullDayShift"]) == true)
                    {
                        // =========== FullDayShift
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                        {
                            foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                            {
                                lstPlace.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                            }
                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                    {
                        // =========== EVENING_SHIFT
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                        {
                            //lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                            foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                            {
                                lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                            }

                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                    {
                        //// =========== MORNING_SHIFT
                        //if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                        //{
                        //    lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
                        //}
                        //else 
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                        {
                           // lstShiftType.Add(new SelectListItem { Text = "Evening", Value = "2" });

                            foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                            {
                                lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                            }

                        }

                    }

                }

                ViewBag.Place = lstPlace;
                #endregion
            }
            else if (Session["IsKioskUser"] != null && Convert.ToBoolean(Session["IsKioskUser"]) && Session["KioskCtznName"] != null)
            {
              
                #region EMITRAKioskUser
                BookOnTicket bkIsDept = new BookOnTicket();
                DataTable dtkiosk = new DataTable();
                string result = string.Empty;
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    result = ipRange[0];
                }
                else
                {
                    result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                dtkiosk = bkIsDept.Chk_DeptKioskIP(result);
                if (dtkiosk.Rows.Count > 0)
                {
                    if (Convert.ToString(dtkiosk.Rows[0][0]) == "0")
                    {
                        TempData["ValidIPAddress"] = "Invalid IP Address!!!";
                    }

                }

                FMDSS.Models.OnlineBooking.CS_BoardingDetails obj2 = new FMDSS.Models.OnlineBooking.CS_BoardingDetails();

                DataSet Ds = new DataSet();

                DataTable DT_BoardingDuration;


                Ds = obj2.BindDptKioskPLACES(Convert.ToString(Session["SSOid"]));

                CS_BoardingDetails objB = new CS_BoardingDetails();

                DateTime DateTimes = DateTime.Now;
                int CurrentTime = Convert.ToInt16(DateTimes.ToString("dd/MM/yyyy HH:mm:ss").Substring(11, 2));

                DT_BoardingDuration = objB.GetBoardingDuration(Convert.ToString(Ds.Tables[0].Rows[0]["PlaceID"]));

                if (DT_BoardingDuration.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsFullDayShift"]) == true)
                    {
                        // =========== FullDayShift
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["KioskBookingMorningTimeTo"]))
                        {

                          

                            foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                            {
                                lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                            }
                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsEveningShift"]) == true)
                    {
                        // =========== EVENING_SHIFT
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayEveningTimeTo"]))
                        {
                           

                            foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                            {
                                lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                            }

                        }
                    }
                    if (Convert.ToBoolean(DT_BoardingDuration.Rows[0]["IsMorningShift"]) == true)
                    {
                        //// =========== MORNING_SHIFT
                        //if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["PreviousDayEveningTimeTo"]))
                        //{
                        //    lstShiftType.Add(new SelectListItem { Text = "Morning", Value = "1" });
                        //}
                        //else 
                        if (CurrentTime >= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeFrom"]) && CurrentTime <= Convert.ToInt16(DT_BoardingDuration.Rows[0]["CurrentDayMorningTimeTo"]))
                        {
                            

                            foreach (System.Data.DataRow dr in Ds.Tables[0].Rows)
                            {
                                lstPlace.Add(new SelectListItem { Text = Convert.ToString(@dr["PlaceName"]), Value = Convert.ToString(@dr["PlaceID"]) });
                            }

                        }

                    }

                   

                }
                ViewBag.Place = lstPlace;


                if (Convert.ToString(DateTime.Now.DayOfWeek) == "Tuesday")
                {
                    ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                }
                else
                {
                    ViewBag.CurrentDate = Convert.ToString(DateTimes.ToString("dd/MM/yyyy"));
                }


                #endregion
            }
        }


        [HttpGet]
        public ActionResult ZooTicketVerification()
        {

            getDeptplace();

            ViewData["ListZooTicketVerification"] = ListZooTicketBookingDetails;
            return View();

        }

        [HttpPost]
        public ActionResult ZooTicketVerification(MISZooCommonModel MIS)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = 0;//Convert.ToInt64(Session["UserID"].ToString());
            onloadZooTicketBooking();
            try
            {
                getDeptplace();

                MISZooCommonModel obj = new MISZooCommonModel();
                DataTable DT;
                DT = obj.GetZooTicketVerification("GetList", MIS.RequestId, Convert.ToInt32(MIS.Place));

                foreach (DataRow dr in DT.Rows)
                {
                    ListZooTicketBookingDetails.Add(
                        new MISZooCommonModel()
                        {

                            RequestId = Convert.ToString(dr["RequestId"].ToString()),
                            BookingType = Convert.ToString(dr["BookingType"]),
                            DateOfBooking = Convert.ToString(dr["DateOfBooking"]),
                            DateOfVisit = Convert.ToString(dr["DateOfVisit"]),
                            NameofInstituteandOrganization = Convert.ToString(dr["NameofInstituteAndOrganization"]),

                            PLACE_NAME = Convert.ToString(dr["PlaceName"]),
                            TotalMember = Convert.ToString(dr["TotalMember"]),
                            TotalCamera = Convert.ToString(dr["TotalCamera"]),
                            VehicleType = Convert.ToString(dr["VehicleType"]),


                            IsPrivateVehical = Convert.ToString(dr["IsPrivateVehical"]),
                            TotalVehicalFees = Convert.ToString(dr["TotalVehicalFees"]),
                            TotalMemberFees = Convert.ToString(dr["TotalMemberFees"]),
                            TotalCameraFees = Convert.ToString(dr["TotalCameraFees"]),
                            TransactionStatus = Convert.ToString(dr["TransactionStatus"]),
                            EmitraTransactionId = Convert.ToString(dr["EmitraTransactionId"]),
                            TotalAmount = Convert.ToString(dr["TotalAmount"]),

                            IP_Address = Convert.ToString(dr["IP_Address"]),
                            ModeOfBooking = Convert.ToString(dr["ModeOfBooking"]),
                            ZooTicketBoardingVerificationStatus = Convert.ToInt32(dr["ZooTicketBoardingVerificationStatus"]),
                            BoardingVerificationDateTime = Convert.ToString(dr["BoardingVerificationDateTime"]),

                        });
                    if (DT.Rows.Count > 0)
                    {
                        ViewBag.ZooTicketBoardingVerificationStatus = Convert.ToInt32(DT.Rows[0]["ZooTicketBoardingVerificationStatus"]) == 0 ? "Pending" : "Verified";
                        ViewBag.BoardingVerificationDateTime = Convert.ToString(DT.Rows[0]["BoardingVerificationDateTime"]);

                        ViewBag.Rstatus = 1;
                    }
                    else
                    {
                        ViewBag.ZooTicketBoardingVerificationStatus = "NA";
                        ViewBag.BoardingVerificationDateTime = "";
                        ViewBag.Rstatus = 0;
                    }
                    ViewData["ListZooTicketVerification"] = ListZooTicketBookingDetails;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(MIS);
        }


        [HttpPost]
        public JsonResult TicketVerification(string RequestID, int PlaceID)
        {
            DataTable DT;
            MISZooCommonModel obj = new MISZooCommonModel();
            DT = obj.GetZooTicketVerification("Verification", RequestID, PlaceID);


            string items = "";
            items = Convert.ToString(DT.Rows[0][0]) + "," + Convert.ToString(DT.Rows[0]["BoardingVerificationDateTime"]);

            //ViewBag.BoardingVerificationDateTime = Convert.ToString(DT.Rows[0]["BoardingVerificationDateTime"]);

            //obj.TransactionStatus = Convert.ToString(DT.Rows[0]["ZooTicketBoardingVerificationStatus"]);
            //obj.BoardingVerificationDateTime = Convert.ToString(DT.Rows[0]["BoardingVerificationDateTime"]);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
