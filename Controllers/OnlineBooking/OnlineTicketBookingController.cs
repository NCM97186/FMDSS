using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.CitizenService.PermissionService;
using System.Configuration;
using FMDSS.Filters;

namespace FMDSS.Controllers.OnlineBooking
{
    [MyAuthorization]
    public class OnlineTicketBookingController : BaseController
    { //
        // GET: /OnlineTicketBooking/
        TicketBooking cst = new TicketBooking();
        List<TicketBooking> PlaceList = new List<TicketBooking>();
        List<TicketBooking> FeesList = new List<TicketBooking>();
        List<TicketBooking> ticketList = new List<TicketBooking>();
        public ActionResult OnlineTicketBooking()
        {
            /*
            List<SelectListItem> District = new List<SelectListItem>();
            DataTable dtd = new DataTable();
            dtd = cst.District();
            ViewBag.district = dtd;
            foreach (System.Data.DataRow dr in ViewBag.district.Rows)
            {
                District.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = District;
            */
            if (Session["UserId"] != null)
            {
                List<TicketBooking> feesDetails = new List<TicketBooking>();
                DataTable dtf = new DataTable();
                cst.UserID = Convert.ToInt64(Session["UserId"].ToString());
                dtf = cst.Select_BookedTicket();
                foreach (DataRow dr in dtf.Rows)
                {
                    ticketList.Add(new TicketBooking()
                    {
                        // RowID = Int64.Parse(dr["Rowid"].ToString()),
                        Amount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                        TransactionId = dr["EmitraTransactionID"].ToString(),
                        ArrivalDate = Convert.ToDateTime(dr["DateOfArrival"].ToString()),
                        TotalTickets = Convert.ToInt64(dr["TotalMembers"].ToString())
                    });
                }
                return View(ticketList);
            }
            else
            {
                return RedirectToAction("login", "login");
            }

            //try
            //{
            //    DataTable dt = new DataTable();
            //    cst.EnteredBy = 1;
            //    dt = cst.Select_BookindTicketsDetails();

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        feesDetails.Add(new TicketBooking()
            //        {
            //            DistrictID = Int64.Parse(dr["DistrictID"].ToString()),
            //            DistrictName = dr["DIST_NAME"].ToString(),
            //            PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
            //            Place = dr["PlaceName"].ToString(),
            //            TotalPerson = dr["TotalMembers"].ToString(),
            //            ArrivalDate = dr["DateOfArrival"].ToString(),
            //            Amount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
            //        });


            //    }

            //}
            //catch (Exception ex)
            //{

            //}


        }

        public JsonResult DistrictbyCategory(string Category)
        {
            List<SelectListItem> District = new List<SelectListItem>();
            try
            {
                DataTable dtd = new DataTable();
                cst.Category = Category;
                dtd = cst.Get_CategorywiseDistrict();
                ViewBag.district = dtd;
                foreach (System.Data.DataRow dr in ViewBag.district.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.Districts = District;
            }
            catch { }
            return Json(new SelectList(District, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult PlaceByDistrict(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                cst.DistrictID = Convert.ToInt64(districtID);
                dt = cst.Select_Places_ByDistrict();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    Places.Add(new SelectListItem { Text = @dr["PlaceName"].ToString(), Value = @dr["PlaceID"].ToString() });
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new SelectList(Places, "Value", "Text"));


        }

        [HttpPost]
        public JsonResult CheckTicketAvailability(int placeID, string arrivaldate, string shifttime)
        {
            string st = string.Empty;
            try
            {

                cst.PlaceID = placeID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null);
                cst.ShiftTime = shifttime;
                st = cst.CheckTicketAvailability();
                ViewBag.fname = st;


            }
            catch (Exception ex)
            {

            }
            return Json(st);

        }

        [HttpPost]
        public JsonResult feesByDistrictPlace(int districtID, int placeID)
        {
            List<TicketBooking> fees = new List<TicketBooking>();
            try
            {
                DataTable dt = new DataTable();
                cst.DistrictID = Convert.ToInt64(districtID);
                cst.PlaceID = Convert.ToInt64(placeID);
                dt = cst.Select_Fees_ByDistrict_Places();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new TicketBooking()
                        {
                            //RowID = Int64.Parse(dr["Rowid"].ToString()),
                            DistrictID = Int64.Parse(dr["DIST_CODE"].ToString()),
                            PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                            IndianAdultFees = Convert.ToDecimal(dr["IndianAdultFees"].ToString()),
                            IndianChildFees = Convert.ToDecimal(dr["IndianChildFees"].ToString()),
                            StudentFees = Convert.ToDecimal(dr["StudentFees"].ToString()),
                            ForeignerAdultFees = Convert.ToDecimal(dr["ForeignerAdultFees"].ToString()),
                            ForeignerChildFees = Convert.ToDecimal(dr["ForeignerChildFees"].ToString()),
                            GuideFees = Convert.ToDecimal(dr["GuideFees"].ToString()),
                            CameraFees = Convert.ToDecimal(dr["CameraFees"].ToString()),
                            SingleOccupancyFees = Convert.ToDecimal(dr["SingleOccupancyFees"].ToString()),
                            DoubleOccupancyFees = Convert.ToDecimal(dr["DoubleOccupancyFees"].ToString()),
                            SafariFees = Convert.ToDecimal(dr["SafariFees"].ToString()),
                            DiscountPercentage = Convert.ToDecimal(dr["Discount"].ToString()),
                            TaxRate = Convert.ToDecimal(dr["TaxRate"].ToString()),
                            FullDayShift = dr["FullDayShift"].ToString(),
                        });
                    }

                }
                else {

                    fees.Add(new TicketBooking()
                    {
                        //RowID = Int64.Parse(dr["Rowid"].ToString()),
                        DistrictID = 0,
                        PlaceID = 0,
                        IndianAdultFees = Convert.ToDecimal(0),
                        IndianChildFees = Convert.ToDecimal(0),
                        StudentFees = Convert.ToDecimal(0),
                        ForeignerAdultFees = Convert.ToDecimal(0),
                        ForeignerChildFees = Convert.ToDecimal(0),
                        GuideFees = Convert.ToDecimal(0),
                        CameraFees = Convert.ToDecimal(0),
                        SingleOccupancyFees = Convert.ToDecimal(0),
                        DoubleOccupancyFees = Convert.ToDecimal(0),
                        SafariFees = Convert.ToDecimal(0),
                        DiscountPercentage = Convert.ToDecimal(0),
                        TaxRate = Convert.ToDecimal(0),
                        FullDayShift = "-1",
                    });
                
                }

            }
            catch (Exception ex)
            {

            }
            return Json(fees);


        }

        [HttpPost]
        public ActionResult SubmitticketDetails(TicketBooking cs, string Command, FormCollection form, string Message)
        {
            try
            {
                if (Command == "Payment")
                {
                    if (Session["UserId"] != null)
                    {
                        cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                        cs.TransactionId = DateTime.Now.Ticks.ToString();
                        Session["RequestId"] = cs.TransactionId;
                        cs.DistrictID = Convert.ToInt64(form["ddl_Districts"].ToString());
                        cs.PlaceID = Convert.ToInt64(form["ddl_place"].ToString());
                        cs.ShiftTime = form["ddl_Shift"].ToString();
                        cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                        cs.TotalPerson = form["TotalPerson"].ToString();
                        if (form["txt_IndAdult"].ToString() != "")
                        {
                            cs.IndianAdult = Convert.ToInt32(form["txt_IndAdult"].ToString());
                        }
                        else
                        {
                            cs.IndianAdult = 0;
                        }
                        if (form["txt_IndChild"].ToString() != "")
                        {
                            cs.IndianChild = Convert.ToInt32(form["txt_IndChild"].ToString());
                        }
                        else
                        {
                            cs.IndianChild = 0;
                        }
                        if (form["txt_Student"].ToString() != "")
                        {
                            cs.Student = Convert.ToInt32(form["txt_Student"]);
                        }
                        else
                        {
                            cs.Student = 0;
                        }
                        if (form["txt_forAdult"].ToString() != "")
                        {
                            cs.ForeignerAdult = Convert.ToInt32(form["txt_forAdult"].ToString());
                        }
                        else
                        {
                            cs.ForeignerAdult = 0;
                        }
                        if (form["txt_forChild"].ToString() != "")
                        {
                            cs.ForeignerChild = Convert.ToInt32(form["txt_forChild"].ToString());
                        }
                        else
                        {
                            cs.ForeignerChild = 0;
                        }
                        if (form["txt_guideFee"].ToString() != "")
                        {
                            cs.Guide = Convert.ToInt32(form["txt_guideFee"].ToString());
                        }
                        else
                        {
                            cs.Guide = 0;
                        }
                        if (form["txt_cameraFees"].ToString() != "")
                        {
                            cs.Camera = Convert.ToInt32(form["txt_cameraFees"].ToString());
                        }
                        else
                        {
                            cs.Camera = 0;
                        }
                        if (form["txt_singleOccupancy"].ToString() != "")
                        {
                            cs.SingleRoom = Convert.ToInt32(form["txt_singleOccupancy"].ToString());
                        }
                        else
                        {
                            cs.SingleRoom = 0;
                        }
                        if (form["txt_doubleOccupancy"].ToString() != "")
                        {
                            cs.DoubleRoom = Convert.ToInt32(form["txt_doubleOccupancy"].ToString());
                        }
                        else
                        {
                            cs.DoubleRoom = 0;
                        }
                        if (form["txt_safariMember"].ToString() != "")
                        {
                            cs.Safari = Convert.ToInt32(form["txt_safariMember"].ToString());
                        }
                        else
                        {
                            cs.Safari = 0;
                        }
                        if (form["txt_finalfees"].ToString() != "")
                        {
                            cs.TotalFees = Convert.ToDecimal(form["txt_finalfees"].ToString());
                        }
                        else
                        {
                            cs.TotalFees = 0;
                        }
                        if (form["DiscountPercentage"].ToString() != "")
                        {
                            cs.DiscountPercentage = Convert.ToDecimal(form["DiscountPercentage"].ToString());
                        }
                        else
                        {
                            cs.DiscountPercentage = 0;
                        }
                        if (form["TaxRate"].ToString() != "")
                        {
                            cs.TaxRate = Convert.ToDecimal(form["TaxRate"].ToString());
                        }
                        else
                        {
                            cs.TaxRate = 0;
                        }
                        if (form["FinalAmount"].ToString() != "")
                        {
                            cs.Amount = Convert.ToDecimal(form["FinalAmount"].ToString());
                        }
                        else
                        {
                            cs.Amount = 0;
                        }
                        if (Session["KioskUserId"] != null)
                        {
                            cs.kioskuserid = Session["KioskUserId"].ToString();
                        }
                        else
                        {
                            cs.kioskuserid = "0";
                        }
                        Int64 i = cs.SubmitTicketDetails();
                        if (i > 0)
                        {
                            Message = "Record Saved Sucessfully";
                            #region payment
                            DataTable dtColmn = new DataTable();
                            if (dtColmn.Rows.Count == 0)
                            {
                                dtColmn.Columns.Add("Transaction_Id");
                                dtColmn.Columns.Add("TotalFees");
                                dtColmn.Columns.Add("TaxRate");
                                dtColmn.Columns.Add("Discount");
                                dtColmn.Columns.Add("EnterBy");
                                dtColmn.Columns.Add("Status");
                            }
                            decimal totalPrice = 0;
                            totalPrice = Convert.ToDecimal(form["FinalAmount"].ToString());
                            Session["totalprice"] = totalPrice;
                            DataRow dtrow = dtColmn.NewRow();
                            dtrow["Transaction_Id"] = Session["RequestId"].ToString();
                            dtrow["TotalFees"] = totalPrice;
                            dtrow["TaxRate"] = form["TaxRate"].ToString();
                            dtrow["Discount"] = form["DiscountPercentage"].ToString();
                            dtrow["EnterBy"] = Session["User"].ToString();
                            dtrow["Status"] = "Pending";
                            dtColmn.Rows.Add(dtrow);
                            ViewData.Model = dtColmn.AsEnumerable();
                            return View("OnlineTicketPayment");
                            #endregion
                        }
                        else if (i == 0)
                        {
                            Message = "No Ticket Available!!!";
                        }
                        else
                        {
                            Message = "An Error occured";
                        }
                    }
                }
                else if (Command == "Update")
                {
                    cs.UpdatedBy = 1;
                    Int64 k = cs.UpdatePlaces();
                    if (k > 0)
                    {
                        Message = "Record Updated Sucessfully";
                    }
                    else
                    {
                        Message = "An Error occured";
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }


        [HttpPost]
        public void Pay()
        {
            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();

            //EM33172142@5488
            Payment pay = new Payment();
            string encrypt = pay.RequestString("EM33172142@5488", Session["RequestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "OnlineTicketBooking/Payment", Session["User"].ToString(), "", "");
            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        }
        public ActionResult Payment()
        {
            if (Session["RequestId"] != null)
            {
                TicketBooking cs = new TicketBooking();
                Payment pay = new Payment();
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
                //string response = "yXBdEkS6dSGTCohZ8VmsDuZkk9apDX2AJkACzuSmD6gc601RZn4UOpK823hjVERWp0SxTl3EiVeqJ/PBwCCcRrOTU1CJrl1VLe5W4zZXUq6aEkCERKPRhxQu9jGYORuckUzNnE1 l6flcwuTPVXDh6Ra6hFUfj43OQ48bwDba8v2RnhgJasajFfnNThsUelcDjbrJTR CT3yeIOdMM/z5Q9xZc3bGMqkZg4u0n3hoePCMH7rFETTQIspUsQsr9OL3Cf25W 9pz43p6skE8VWuKyW1HdTdKbHurgJkfurNjnsnOzOHDWAWfEQG1KRXhSr4cQGsegHV0zuuG/5q1t3gwaWq177ViRxU69x sJTSx9rGNNnN523ingz4R6DkPNaj7rpAAqzDHQQnByajyAWCLeHpEnurFI9 TOUk04tFHoR/JTzB92DGwfQZQ7PBZ0mrqyNfb yzVo2lor3vzwbdaAV90Iuk/QbxZJ4yoU5lYw=";
                // string response = "yXBdEkS6dSGTCohZ8VmsDqTIY/En9It4EuxQN7SaK 6M7TphG8JfNYLekyyQRP0zJDPONZJEIdxuYynpBL3kujGN nWxjWjkL2LhMPCc 74x7AbjOTkBB11cwtW8PjgcdxKScsjusU TvlqepR2uBGd3ft9DKPTiKgVeOvzEcVGizEpCAgGOBTkydLz7mZ8UkSbVeFTdYLnC8t/wXvLMbaCStEZvC9Fw5RPgloli2isefscmdhEX8mG8c50vldCCxUD7hUe/XaDEeNyeP7fOz6siPEKRP5n7lronn047kGJGw9ZM QEj3tlbPvywYvYY6KG9Hh wpoM=";
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
                        string[] status1 = item.Split('=');
                        rowstatus1 = status1[1].ToString();
                    }
                    if (item.Equals(checkSucess))
                    {
                        string[] status1 = item.Split('=');
                        rowstatus1 = status1[1].ToString();
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
                    cs.TransactionId = finalreqid;
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
                        cs.Trn_Status_Code = 0;
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
                    cs.TransactionId = finalreqid;
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
                    dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;
                    dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                    //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                    if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                    {
                        cs.Trn_Status_Code = 1;
                    }
                    dt.Rows.Add(dtrow);
                }
                #endregion

                cs.UpdateTransactionStatus("3");
                ViewData.Model = dt.AsEnumerable();
                
            }
            return View("TransactionStatus");
        }
    }
}
