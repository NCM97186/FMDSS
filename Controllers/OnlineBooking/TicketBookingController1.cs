//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : TicketBookingController
//  Description  : File contains calling functions from UI
//  Date Created : 24-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.OnlineBooking;
using FMDSS.Models.CitizenService.PermissionService;
using System.Xml;
using System.Configuration;
using System.Text;
using FMDSS.Filters;


namespace FMDSS.Controllers.OnlineBooking
{
    [MyAuthorization]
    public class TicketBookingController : Controller
    {
        //
        // GET: /TicketBooking/ 
        List<SelectListItem> Accomodation = new List<SelectListItem>();
        List<SelectListItem> vehicleCategory = new List<SelectListItem>();
        List<CS_Ticket> ticketList = new List<CS_Ticket>();
        /// <summary>
        /// Renders UI for Online Ticket Booking
        /// </summary>
        /// <returns>View for  Online Ticke and bind all booked ticket</returns>
        public ActionResult BookTicket()
        {
            CS_Ticket cst = new CS_Ticket();

            try
            {
                if (Session["UserId"] != null)
                {
                    List<CS_Ticket> feesDetails = new List<CS_Ticket>();
                    DataTable dtf = new DataTable();
                    cst.UserID = Convert.ToInt64(Session["UserId"].ToString());
                    dtf = cst.Select_BookedTicket();
                    foreach (DataRow dr in dtf.Rows)
                    {
                        ticketList.Add(new CS_Ticket()
                        {
                            // RowID = Int64.Parse(dr["Rowid"].ToString()),
                            TicketID = Convert.ToInt64(dr["TicketID"].ToString()),
                            TotalAmount = Convert.ToDecimal(dr["AmountTobePaid"].ToString()),
                            TransactionId = dr["EmitraTransactionID"].ToString(),
                            Date = dr["DateOfArrival"].ToString(),
                            TotalMember = Convert.ToInt32(dr["TotalMembers"].ToString())
                        });
                    }


                    DataTable dt = new DataTable();
                    dt = cst.GetVehicleType();
                    ViewBag.Vehiclecat = dt;
                    foreach (System.Data.DataRow dr in ViewBag.Vehiclecat.Rows)
                    {
                        vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
                    }
                    ViewBag.Vehiclecat = vehicleCategory;   

                    return View(ticketList);
                }
                else
                {
                    return RedirectToAction("login", "login");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Function to bind district on the basis of wildlife category
        /// </summary>
        /// <param name="Category"></param>
        /// <returns>Json result for District</returns>
        public JsonResult DistrictbyCategory(string Category)
        {
            List<SelectListItem> District = new List<SelectListItem>();
            try
            {
                CS_Ticket cst = new CS_Ticket();
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

        /// <summary>
        /// function to bind places on the basic of districts
        /// </summary>
        /// <param name="districtID"></param>
        /// <returns>Json resultfor places</returns>
        [HttpPost]
        public JsonResult PlaceByDistrict(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            try
            {
                CS_Ticket cst = new CS_Ticket();
                DataTable dt = new DataTable();
                cst.DistrictID = Convert.ToString(districtID);
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

        /// <summary>
        /// function to check ticket availability on the basis of given param values
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="shifttime"></param>
        /// <returns> json result return available ticket </returns>
        [HttpPost]
        public JsonResult CheckTicketAvailability(int placeID, string arrivaldate, string shifttime)
        {
            string st = string.Empty;
            try
            {
                CS_Ticket cst = new CS_Ticket();
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

        /// <summary>
        /// function to bind Shift time on given param values
        /// </summary>
        /// <param name="districtID"></param>
        /// <param name="placeID"></param>
        /// <returns> Return Shift</returns>
        [HttpPost]
        public JsonResult BindShiftByDistrictPlace(int districtID, int placeID)
        {
            List<TicketBooking> fees = new List<TicketBooking>();
            try
            {
                CS_Ticket cst = new CS_Ticket();
                DataTable dt = new DataTable();
                cst.DistrictID = Convert.ToString(districtID);
                cst.PlaceID = Convert.ToInt64(placeID);
                dt = cst.Select_Shift_ByDistrict_Places();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new TicketBooking()
                        {
                            FullDayShift = dr["FullDayShift"].ToString(),
                        });

                    }
                }
                else
                {

                    fees.Add(new TicketBooking()
                    {
                        FullDayShift = "-1",
                    });

                }

                DataTable dta = new DataTable();

                cst.PlaceID = Convert.ToInt64(placeID);
                dta = cst.GetAccomodationType();
                ViewBag.Accomo = dta;
                foreach (System.Data.DataRow dr in ViewBag.Accomo.Rows)
                {
                    Accomodation.Add(new SelectListItem { Text = @dr["RoomType"].ToString(), Value = @dr["AccommodationID"].ToString() });
                }


            }
            catch (Exception ex)
            {

            }
            return Json(new { list1 = fees, list2 = Accomodation, JsonRequestBehavior.AllowGet });


        }

        /// <summary>
        /// function to get all types of fees for ticket booking on the basis of given param values
        /// </summary>
        /// <param name="placeID"></param>
        /// <param name="districtID"></param>
        /// <param name="nationality"></param>
        /// <param name="memberType"></param>
        /// <returns>Json result for fees</returns>
        [HttpPost]
        public JsonResult SelectFee(Int64 placeID, string districtID, string nationality, string memberType)
        {
            List<CS_Ticket> fees = new List<CS_Ticket>();
            try
            {
                DataTable dt = new DataTable();
                CS_Ticket cst = new CS_Ticket();
                cst.PlaceID = placeID;
                cst.DistrictID = districtID;
                cst.Nationality = nationality;
                cst.MemberType = memberType;
                dt = cst.SelectMemberFees();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new CS_Ticket()
                        {
                            FeePerMember = Convert.ToDecimal(dr["MemberFee"].ToString()),
                            CameraFee = Convert.ToDecimal(dr["CameraFees"].ToString()),
                        });
                    }
                }
                else
                {
                    fees.Add(new CS_Ticket()
                    {
                        FeePerMember = Convert.ToDecimal(0),
                        CameraFee = Convert.ToDecimal(0),
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(fees);

        }

        /// <summary>
        /// Function to create xml file of all memeber details for whom ticket is booking
        /// </summary>
        /// <param name="member"></param>
        /// <returns>Return xml row for members details</returns>
        [HttpPost]
        public JsonResult memberData(CS_Ticket member)
        {

            if (member != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["MemberInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("CrewMember");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["MemberInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    }

                    XmlElement Crew_TYPE = xmldoc.CreateElement("Crew_TYPE");
                    XmlElement Name = xmldoc.CreateElement("Name");
                    XmlElement Gender = xmldoc.CreateElement("Gender");
                    XmlElement IDType = xmldoc.CreateElement("IDType");
                    XmlElement IDNo = xmldoc.CreateElement("IDNo");
                    XmlElement Nationality = xmldoc.CreateElement("Nationality");
                    XmlElement MemberType = xmldoc.CreateElement("MemberType");
                    XmlElement FeePerMember = xmldoc.CreateElement("FeePerMember");
                    XmlElement TotalCamera = xmldoc.CreateElement("TotalCamera");
                    XmlElement CameraFee = xmldoc.CreateElement("CameraFee");

                    Name.InnerText = member.Name;
                    Gender.InnerText = member.Gender;
                    IDType.InnerText = member.IDType;
                    IDNo.InnerText = member.IDNo;
                    Nationality.InnerText = member.Nationality;
                    MemberType.InnerText = member.MemberType;
                    FeePerMember.InnerText = Convert.ToString(member.FeePerMember);
                    TotalCamera.InnerText = Convert.ToString(member.TotalCamera);
                    CameraFee.InnerText = Convert.ToString(member.CameraFee);


                    Crew_TYPE.AppendChild(Name);
                    Crew_TYPE.AppendChild(Gender);
                    Crew_TYPE.AppendChild(IDType);
                    Crew_TYPE.AppendChild(IDNo);
                    Crew_TYPE.AppendChild(Nationality);
                    Crew_TYPE.AppendChild(MemberType);
                    Crew_TYPE.AppendChild(FeePerMember);
                    Crew_TYPE.AppendChild(TotalCamera);
                    Crew_TYPE.AppendChild(CameraFee);

                    xmldoc.DocumentElement.AppendChild(Crew_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }

            return Json(member, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// delete members data entry
        /// </summary>
        /// <param name="member"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult deleteMemberData(CS_Ticket member, string Id)
        {
            try
            {
                if (Session["MemberInfo"] != null)
                { 
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds=new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["IDNo"].ToString() == Id)
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                        Session["MemberInfo"] = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            return Json(member, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to select vehicle by there category e.g vehicle, boat, electravan
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <param name="placeID"></param>
        /// <returns>Json result as vehicle list</returns>
        [HttpPost]
        public JsonResult vehicleByCategory(int vehicleCatID, Int64 placeID)
        {
            CS_Ticket cst = new CS_Ticket();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                cst.PlaceID = placeID;
                dt = cst.Select_vehicle(Convert.ToInt64(vehicleCatID));
                ViewBag.vname = dt;
                foreach (System.Data.DataRow dr in ViewBag.vname.Rows)
                {
                    vehicle.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["VehicleID"].ToString() });
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new SelectList(vehicle, "Value", "Text"));


        }

        /// <summary>
        /// Function return vehicle fees on the basis of give param values
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <param name="vehicleID"></param>
        /// <param name="arrivaldate"></param>
        /// <param name="placeID"></param>
        /// <returns>Json result for vehicle fees</returns>
        [HttpPost]
        public JsonResult SelectVehicleFee(Int64 vehicleCatID, Int64 vehicleID, string arrivaldate, Int64 placeID)
        {
            List<CS_Ticket> vehiclefees = new List<CS_Ticket>();
            try
            {
                DataTable dt = new DataTable();
                CS_Ticket cst = new CS_Ticket();
                cst.VehicleCatID = vehicleCatID;
                cst.VehicleID = vehicleID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null); ;
                cst.PlaceID = placeID;
                dt = cst.Select_vehicle_Fees_Seat();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        vehiclefees.Add(new CS_Ticket()
                        {
                            AvailableSeat = Convert.ToInt32(dr["AvailableSeat"].ToString()),
                            FeePerVehicle = Convert.ToDecimal(dr["TotalFee"].ToString()),
                        });
                    }
                }
                else
                {
                    vehiclefees.Add(new CS_Ticket()
                    {
                        AvailableSeat = Convert.ToInt32(0),
                        FeePerVehicle = Convert.ToDecimal(0),
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(vehiclefees);

        }

        /// <summary>
        /// Function to get accomodation charges on the basis of room type
        /// </summary>
        /// <param name="AccomoID"></param>
        /// <param name="placeID"></param>
        /// <param name="arrivaldate"></param>
        /// <returns> json result return Accomodation charges.</returns>
        [HttpPost]
        public JsonResult SelectAccomoFee(Int64 AccomoID, Int64 placeID, string arrivaldate)
        {
            List<CS_Ticket> vehiclefees = new List<CS_Ticket>();
            try
            {
                DataTable dt = new DataTable();
                CS_Ticket cst = new CS_Ticket();
                cst.AccomoID = AccomoID;
                cst.ArrivalDate = DateTime.ParseExact(arrivaldate, "dd/MM/yyyy", null); ;
                cst.PlaceID = placeID;
                dt = cst.Select_Accomo_Fees_Availability();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        vehiclefees.Add(new CS_Ticket()
                        {
                            RoomAvailability = Convert.ToInt32(dr["AvailableRoom"].ToString()),
                            RoomCharge = Convert.ToDecimal(dr["RatePerRoom"].ToString()),
                        });
                    }
                }
                else
                {
                    vehiclefees.Add(new CS_Ticket()
                    {
                        AvailableSeat = Convert.ToInt32(0),
                        FeePerVehicle = Convert.ToDecimal(0),
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(vehiclefees);

        }

        /// <summary>
        /// Function to create xml file for safari ticket details
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult safariData(CS_Ticket member)
        {

            if (member != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["safariInfo"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("SafariVehicleInfo");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["safariInfo"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                    }

                    XmlElement vehicleInfo = xmldoc.CreateElement("Crew_TYPE");
                    XmlElement vahicleCatID = xmldoc.CreateElement("vahicleCatID");
                    XmlElement vahicleCat = xmldoc.CreateElement("vahicleCat");
                    XmlElement vID = xmldoc.CreateElement("vID");
                    XmlElement vName = xmldoc.CreateElement("vName");
                    XmlElement SeatForBooking = xmldoc.CreateElement("SeatForBooking");
                    XmlElement FeePerVehicle = xmldoc.CreateElement("FeePerVehicle");
                    XmlElement Totalfee = xmldoc.CreateElement("Totalfee");

                    vahicleCatID.InnerText = Convert.ToString(member.VehicleCatID);
                    vahicleCat.InnerText = member.VehicleCategory;
                    vID.InnerText = Convert.ToString(member.VehicleID);
                    vName.InnerText = member.VehicleName;
                    SeatForBooking.InnerText = Convert.ToString(member.SeatForBooking);
                    FeePerVehicle.InnerText = Convert.ToString(member.FeePerVehicle);
                    Totalfee.InnerText = Convert.ToString(member.VehicleFeeTotal);


                    vehicleInfo.AppendChild(vahicleCatID);
                    vehicleInfo.AppendChild(vahicleCat);
                    vehicleInfo.AppendChild(vID);
                    vehicleInfo.AppendChild(vName);
                    vehicleInfo.AppendChild(SeatForBooking);
                    vehicleInfo.AppendChild(FeePerVehicle);
                    vehicleInfo.AppendChild(Totalfee);

                    xmldoc.DocumentElement.AppendChild(vehicleInfo);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }

            return Json(member, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deletesafariData(CS_Ticket member, string Id)
        {
            try
            {
                if (Session["safariInfo"] != null)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    DataSet ds = new DataSet();
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (i+1 ==Convert.ToInt32(Id))
                                {
                                    ds.Tables[0].Rows[i].Delete();
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                                }
                                else
                                {
                                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml")) == true)
                                    {
                                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                                        Session["safariInfo"] = null;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            return Json(member, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to save ticket details to database with all the values
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="Command"></param>
        /// <param name="form"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public ActionResult SubmitticketDetails(CS_Ticket cs, string Command, FormCollection form, string Message)
        {
            try
            {
                if (Command == "Submit")
                {
                    if (Session["UserId"] != null)
                    {
                        cs.EnteredBy = Convert.ToInt64(Session["UserId"].ToString());
                        cs.RequestID = DateTime.Now.Ticks.ToString();
                        Session["RequestId"] = cs.RequestID;
                        cs.Category = form["Category"].ToString();
                        cs.DistrictID = form["ddl_Districts"].ToString();
                        cs.PlaceID = Convert.ToInt64(form["ddl_place"].ToString());
                        cs.ShiftTime = form["ddl_Shift"].ToString();
                        cs.ArrivalDate = DateTime.ParseExact(form["ArrivalDate"].ToString(), "dd/MM/yyyy", null);
                        if (form["TotalMember"].ToString() != "")
                        {
                        cs.TotalMember = Convert.ToInt32(form["TotalMember"].ToString());
                        }
                        else{
                         cs.TotalMember =0;
                        }
                        if (form["ddl_Accomo"].ToString() != "")
                        {
                            cs.AccomoID = Convert.ToInt64(form["ddl_Accomo"].ToString());
                        }
                        else
                        { cs.AccomoID = 0; }

                        if (form["TotalRoom"].ToString() != "")
                        {
                            cs.TotalRoom = Convert.ToInt32(form["TotalRoom"].ToString());
                        }
                        else
                        { cs.TotalRoom = 0; }
                        if (form["RoomCharge"].ToString() != "")
                        {
                            cs.RoomCharge = Convert.ToDecimal(form["RoomCharge"].ToString());
                        }
                        else
                        { cs.RoomCharge = 0; }

                        DataSet dsm = new DataSet();
                        if (Session["MemberInfo"] != null)
                        {
                            dsm.ReadXml(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                        }
                        else
                        {
                            dsm.Clear();
                            DataTable objDt2 = new DataTable("Table");
                            objDt2.Columns.Add("Name", typeof(String));
                            objDt2.Columns.Add("Gender", typeof(String));
                            objDt2.Columns.Add("IDType", typeof(String));
                            objDt2.Columns.Add("IDNo", typeof(String));
                            objDt2.Columns.Add("Nationality", typeof(String));
                            objDt2.Columns.Add("MemberType", typeof(String));
                            objDt2.Columns.Add("FeePerMember", typeof(String));
                            objDt2.Columns.Add("TotalCamera", typeof(String));
                            objDt2.Columns.Add("CameraFee", typeof(String));
                            objDt2.AcceptChanges();
                            dsm.Tables.Add(objDt2);
                            objDt2.Clear();
                            objDt2.AcceptChanges();
                        }
                        DataSet dsv = new DataSet();
                        if (Session["safariInfo"] != null)
                        {
                            {
                                dsv.ReadXml(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                            }
                        }
                        else
                        {
                            dsv.Clear();
                            DataTable objDt2 = new DataTable("Table");
                            objDt2.Columns.Add("vahicleCatID", typeof(String));
                            objDt2.Columns.Add("vahicleCat", typeof(String));
                            objDt2.Columns.Add("vID", typeof(String));
                            objDt2.Columns.Add("vName", typeof(String));
                            objDt2.Columns.Add("SeatForBooking", typeof(String));
                            objDt2.Columns.Add("FeePerVehicle", typeof(String));
                            objDt2.Columns.Add("Totalfee", typeof(String));
                            objDt2.AcceptChanges();
                            dsv.Tables.Add(objDt2);
                            objDt2.Clear();
                            objDt2.AcceptChanges();
                        }
                        DataTable dts = new DataTable();
                        if (dsm.Tables[0].Rows.Count > 0)
                        {
                            dts = cs.Submit_TicketDetails(dsm.Tables[0], dsv.Tables[0]);
                        }
                        if (Session["MemberInfo"] != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["MemberInfo"].ToString() + ".xml"));
                                Session["MemberInfo"] = null;
                            }
                        }
                        if (Session["safariInfo"] != null)
                        {
                            if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml")) == true)
                            {
                                System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["safariInfo"].ToString() + ".xml"));
                                Session["safariInfo"] = null;
                            }
                        }
                        if (dts.Rows.Count > 0)
                        {
                            Session["totalprice"] = dts.Rows[0]["TotalAmount"].ToString();
                        }

                        ViewData.Model = dts.AsEnumerable();
                        return View("TicketPayment");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("Login","Login");
        }

        /// <summary>
        /// Function to pay ticket amount
        /// </summary>
        [HttpPost]
        public void Pay()
        {
            //EM33172142@5488
            Payment pay = new Payment();
            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
            string encrypt = pay.RequestString("EM33172142@5488", Session["RequestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "TicketBooking/Payment", Session["User"].ToString(), "", "");
            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        }
        public ActionResult Payment()
        {
            if (Session["RequestId"] != null)
            {
                //TicketBooking cs = new TicketBooking();
                int status1 = 0;
                CS_Ticket cs = new CS_Ticket();
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
                string ResponseResult = pay.ProcesTranscationresponce(response);
                //string status = Request.QueryString["STATUS"].ToString();
                #region Response decryption
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
                    cs.TransactionId = "0";
                    cs.RequestID = finalreqid;
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
                    cs.TransactionId = finalemitraid;
                    dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
                    dtrow["TRANSACTION AMOUNT"] = finalamount;
                    dtrow["USER NAME"] = finalUserName;
                    dtrow["TRANSACTION BANK DETAILS"] = finalbank;
                    //dtrow["TRANSACTION BANK BID"] = finalbankid;   
                    if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                    {
                        cs.Trn_Status_Code = 1;
                        status1 = 1;
                    }
                    dt.Rows.Add(dtrow);
                }
                #endregion
                //SMS_EMail_Services SE = new SMS_EMail_Services();

                //if (Session["PaymentType"].ToString() == "FilmShooting")
                //{                 
                //    DataTable dtf = new DataTable();
                //    dtf=fs.UpdateTransactionStatus("1");
                //    if (dtf != null)
                //    {
                //        if (dt.Rows.Count > 0)
                //        {
                //            string subject = "Request for Film Shooting Permission Review";
                //            string body = Common.GenerateReviwerBody(dtf.Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), "Film Shooting Permission");
                //            SE.sendEMail(subject, body, dtf.Rows[0]["EmailId"].ToString(), "");
                //            // SMS_EMail_Services.sendBulkSMS(dt.Rows[0]["Mobile"].ToString(), "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                //            if (Session["SSODetail"] != null)
                //            {
                //                UserProfile up = (UserProfile)Session["SSODetail"];
                //                SMS_EMail_Services.sendSingleSMS(up.MobileNumber, "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                //            }
                //        }
                //    }
                //}

                cs.UpdateTransactionStatus("3");

                ViewData.Model = dt.AsEnumerable();
                return View("TransactionStatus");
            } return View();
        }

        /// <summary>
        /// Function to print ticket with all necessory details on the basis of ticket ID
        /// </summary>
        /// <param name="ticketid"></param>
        /// <returns>Json result with ticket details</returns>
        [HttpPost]
        public string PrintTicket(Int64 ticketid)
        {
            DataSet ds = new DataSet();
            CS_Ticket cs = new CS_Ticket();
            cs.TicketID = ticketid;
            ds = cs.Select_TicketData_For_Print();
            StringBuilder sb= new StringBuilder();
            if (ds != null)
            {
                sb.Append("<div class='wrapper'><section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='33%'>" +
                           "<a href='#' id='myLogo'><img src='../ima ges/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' class=''></a> </div>" +
                          "<div class='col-xs-12 col-sm-4 centr' style='33%'><span class='pdate'>" + ds.Tables[0].Rows[0]["PlaceName"].ToString() + "</span></div>" +
                          "<div class='col-xs-12 col-sm-4' style='33%'><span class='pull-right pdate'>  </span></div>" +
                          "<div class='divider'></div></div><div class='panel-body'>  <div class='col-lg-12'><label><h4>Booking Details:</h4></label>" +
                          "<div class='divider'></div>");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //sb.Append("<div class='col-lg-12'><table class='table' id='tkt'><tbody>" +
                    //          "<tr><td class='col-lg-3'><label>Booking ID:</label></td><td>" + ds.Tables[0].Rows[0]["RequestID"].ToString() + "</td>" +
                    //          "<td class='col-lg-3'><label>Date of Visit:</label></td><td>" + ds.Tables[0].Rows[0]["DateOfArrival"].ToString() + "</td></tr>" +
                    //           "<tr><td class='col-lg-3'><label>Shift:</label></td><td>" + ds.Tables[0].Rows[0]["SHIFTTIME"].ToString() + "</td>" +
                    //          "<td class='col-lg-3'><label>No. of Ticket Booked:</label></td><td>" + ds.Tables[0].Rows[0]["TOTALMEMBERS"].ToString() + "</td></tr>" +
                    //          "<tr><td class='col-lg-3'><label>Type of Safari & No. of Seats:</label></td><td>" + ds.Tables[0].Rows[0]["SAFARI"].ToString() + "</td>" +
                    //          "<td class='col-lg-3'><label></label></td><td></td></tr>");

                    sb.Append("<div><table class='table' id='tkt'><tbody>" +
                              "<tr><td class='col-lg-12'><label>Booking ID:</label></td><td>" + ds.Tables[0].Rows[0]["RequestID"].ToString() + "</td></tr>" +
                               "<tr><td class='col-lg-3'><label>Date of Visit:</label></td><td>" + ds.Tables[0].Rows[0]["DateOfArrival"].ToString() + "</td>" +
                              "<td class='col-lg-3'><label>Shift:</label></td><td>" + ds.Tables[0].Rows[0]["SHIFTTIME"].ToString() + "</td></tr>" +
                              "<tr><td class='col-lg-3'><label>No. of Ticket Booked:</label></td><td>" + ds.Tables[0].Rows[0]["TOTALMEMBERS"].ToString() + "</td>" +
                              "<td class='col-lg-3'><label>Type of Safari & No. of Seats:</label></td><td>" + ds.Tables[0].Rows[0]["SAFARI"].ToString() + "</td></tr>");

                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    sb.Append("<tr><td class='col-lg-3'><label>Number of Room Booked:</label></td><td>" + ds.Tables[2].Rows[0]["No_Of_BookedRoom"].ToString() + "</td>" +
                              "<td class='col-lg-3'><label>Room Type:</label></td><td>" + ds.Tables[2].Rows[0]["RoomType"].ToString() + "</td></tr>");
                }
                else
                {
                    sb.Append("<tr><td class='col-lg-3'><label>Number of Room Booked:</label></td><td>0</td>" +
                              "<td class='col-lg-3'><label>Room Type:</label></td><td>None</td></tr>");
                }
                sb.Append(" </tbody></table></div>");

                sb.Append("<div><label><h4>Visitor Details:</h4></label><div class='divider'></div></div>");
                if (ds.Tables[1].Rows.Count > 0)
                {
                    sb.Append("<div id='tkt-Unbold' class='table-responsive'> <table class='table table-striped table-bordered table-hover'><thead>" +
                    "<tr><th style='width: 5%'>S. No.</th><th>Visitor Name</th><th>Gender</th><th>Nationality</th><th>ID Type</th>" +
                    "<th>ID No.</th><th>No. of Camera</th></tr></thead><tbody>");
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            sb.Append("<tr><td>" + ds.Tables[1].Rows[0]["RowID"].ToString() + "</td><td>" + ds.Tables[1].Rows[0]["NAME"].ToString() + "</td>" +
                            "<td>" + ds.Tables[1].Rows[0]["GENDER"].ToString() + "</td><td>" + ds.Tables[1].Rows[0]["NATIONALITY"].ToString() + "</td>" +
                            "<td>" + ds.Tables[1].Rows[0]["IDTYPE"].ToString() + "</td><td>" + ds.Tables[1].Rows[0]["IDNO"].ToString() + "</td>" +
                            "<td>" + ds.Tables[1].Rows[0]["NOOFCAMERA"].ToString() + "</td></tr>");
                        }
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='7'>No Record Exists.</td></tr>");
                    }
                    sb.Append("</tbody></table></div>");
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<label><h4>Fare Details:</h4></label><div class='divider'></div></div> <div class='col-lg-12'><table class='table' id='tkt'><tbody>" +
                              "<tr><td class='col-lg-3'><label>Ticket Fare:</label></td><td>" + ds.Tables[0].Rows[0]["TicketFare"].ToString() + "</td>" +
                              "<td class='col-lg-3'><label>Camera Charges:</label></td><td>" + ds.Tables[0].Rows[0]["CameraFee"].ToString() + "</td></tr>" +
                               "<tr><td class='col-lg-3'><label>Safari Fee:</label></td><td>" + ds.Tables[0].Rows[0]["vehicleFee"].ToString() + "</td>" +
                              "<td class='col-lg-3'><label>Room Fare:</label></td><td>" + ds.Tables[0].Rows[0]["RoomFare"].ToString() + "</td></tr>" +
                              "<tr><td class='col-lg-3'><label>Total Charges:</label></td><td>" + ds.Tables[0].Rows[0]["FinalAmount"].ToString() + "</td>" +
                              "<td class='col-lg-3'><label></label></td><td></td></tr></tbody></table></div>");
                }
                sb.Append("<div class='col-lg-12'><table class='table' id='tkt'><tbody><tr><td class='col-lg-2'><label> Ticket booked by:</label></td>"+
                         "<td>" + ds.Tables[0].Rows[0]["Name"].ToString() + "</td>" +
                         "<td class='col-lg-2'><label> Date of booking:</label></td><td>" + ds.Tables[0].Rows[0]["BOOKINGDATE"].ToString() + "</td></tr></tbody></table></div>");

            }
            //sb.Append("<div class='wrapper'><section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='33%'>"+
            //           "<a href='#' id='myLogo'><img src='../images/logo.png' alt='Forest Department, Government of Rajasthan' title='Logo' class=''></a> </div>"+
            //          "<div class='col-xs-12 col-sm-4 centr' style='33%'><span class='pdate'>" + ds.Tables[0].Rows[0]["PlaceName"].ToString() + "</span></div>" +
            //          "<div class='col-xs-12 col-sm-4' style='33%'><span class='pull-right pdate'>Space of QR BAR Code</span></div>" +
            //          "<div class='divider'></div></div>   <div class='panel-body'>  <div class='col-lg-12'><label><h4>Booking Details:</h4></label>"+
            //          "<div class='divider'></div></div><div class='col-lg-6'><div class='form-group'>"+
            //          "<label>Booking ID:</label>" + ds.Tables[0].Rows[0]["RequestID"].ToString() + "</div> </div><div class='col-lg-6'>"+
            //          "<div class='form-group'><label>Date of Visit:</label>" + ds.Tables[0].Rows[0]["DateOfArrival"].ToString() + "</div></div>"+
            //          "<div class='col-lg-6'><div class='form-group'><label>Shift:</label>" + ds.Tables[0].Rows[0]["SHIFTTIME"].ToString() + "</div></div>" +
            //          "<div class='col-lg-6'><div class='form-group'><label>Number of Ticket Booked:</label>" + ds.Tables[0].Rows[0]["TOTALMEMBERS"].ToString() + "</div></div>" +
            //          "<div class='col-lg-6'><div class='form-group'><label>Type of Safari & Number of Seats:</label>" + ds.Tables[0].Rows[0]["SAFARI"].ToString() + "</div></div>" +
            //          "<div class='col-lg-6'><div class='form-group'><label></label></div></div>");
            //if(ds!=null)
            //{
            //    if (ds.Tables[2].Rows.Count > 0) 
            //    {
            //        sb.Append("<div class='col-lg-6'><div class='form-group'><label>Number of Room Booked:</label>" + ds.Tables[2].Rows[0]["No_Of_BookedRoom"].ToString() + "</div></div>"+
            //            "<div class='col-lg-6'><div class='form-group'><label>Room Type:</label>" + ds.Tables[2].Rows[0]["RoomType"].ToString() + "</div></div>");
            //    }
            //    else
            //    {
            //        sb.Append("<div class='col-lg-6'><div class='form-group'><label>Number of Room Booked:</label>0</div></div>" +
            //            "<div class='col-lg-6'><div class='form-group'><label>Room Type:</label></div></div>");
            //    }

            //    sb.Append("<div class='col-lg-12'><label><h4>Visitor Details:</h4></label><div class='divider'></div></div>");
            //    if (ds.Tables[1].Rows.Count > 0)
            //    {
            //        sb.Append("<div id='tbl' class='table-responsive'> <table class='table table-striped table-bordered table-hover'><thead>"+
            //        "<tr><th style='width: 5%'>S. No.</th><th>Name of visitor</th><th>Gender</th><th>Nationality</th><th>ID Type</th>"+
            //        "<th>ID No.</th><th>Carrying No. of Camera</th></tr></thead><tbody>");
            //        if (ds.Tables[1].Rows.Count > 0)
            //        {
            //            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            //            {
            //                sb.Append("<tr><td>" + ds.Tables[1].Rows[0]["RowID"].ToString() + "</td><td>" + ds.Tables[1].Rows[0]["NAME"].ToString() + "</td>" +
            //                "<td>" + ds.Tables[1].Rows[0]["GENDER"].ToString() + "</td><td>" + ds.Tables[1].Rows[0]["NATIONALITY"].ToString() + "</td>" +
            //                "<td>" + ds.Tables[1].Rows[0]["IDTYPE"].ToString() + "</td><td>" + ds.Tables[1].Rows[0]["IDNO"].ToString() + "</td>" +
            //                "<td>" + ds.Tables[1].Rows[0]["NOOFCAMERA"].ToString() + "</td></tr>");
            //            }                        
            //        }
            //        else
            //        {
            //            sb.Append("<tr><td colspan='7'>No Record Exists.</td></tr>");
            //        }
            //    }
            //}

                sb.Append("</div></section></div>");

            return sb.ToString();
        }
    }
}
