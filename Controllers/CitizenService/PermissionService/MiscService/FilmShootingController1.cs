//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : FilmShootingController
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
using FMDSS.Models.Admin;
using System.Xml;
using FMDSS.Models.CitizenService.PermissionService;
using System.IO;
using System.Data.SqlTypes;
using FMDSS.Models;
using System.Configuration;
using FMDSS.Filters;

namespace FMDSS.Controllers.CitizenService.PermissionService.MiscService
{
      [MyAuthorization]
    public class FilmShootingController : Controller
    {
        //
        // GET: /FilmShooting/
        #region FilmShooting
        /// <summary>
        ///Call when request come for FilmShooting view Bind Vehicle category
        /// </summary>
        /// <returns>View for FilmShooting</returns>
        public ActionResult FilmShooting()
        {
            FilmShooting cs = new FilmShooting();
            List<SelectListItem> vehicleCategory = new List<SelectListItem>();
            DataTable dt = new DataTable();
            dt = cs.GetVehicleType();
            ViewBag.Vehiclecat = dt;
            foreach (System.Data.DataRow dr in ViewBag.Vehiclecat.Rows)
            {
                vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
            }
            ViewBag.Vehiclecat = vehicleCategory;
            return getDistrict();
        }

        /// <summary>
          /// Function to bind all district to dropdownlist
          /// </summary>
          /// <returns></returns>
        [HttpPost]
        public ActionResult getDistrict()
        {
            Location cs = new Location();
            List<SelectListItem> District = new List<SelectListItem>();
            DataTable dtd = new DataTable();
            dtd = cs.District();
            ViewBag.district = dtd;
            foreach (System.Data.DataRow dr in ViewBag.district.Rows)
            {
                District.Add(new SelectListItem { Text = @dr["Dist_Name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
            }
            ViewBag.Districts = District;

            return View();

        }

        /// <summary>
        /// function to bind places on the basis of districts
        /// </summary>
        /// <param name="districtID"></param>
        /// <returns>Json result with places list</returns>
        [HttpPost]
        public JsonResult PlaceByDistrict(int districtID)
        {
            List<SelectListItem> Places = new List<SelectListItem>();
            try
            {
                Location cs = new Location();
                DataTable dt = new DataTable();
                cs.DistrictID = Convert.ToInt64(districtID);
                dt = cs.Select_Places_ByDistrict();
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
        /// Function to create xml file with all members details
        /// </summary>
        /// <param name="crewMember"></param>
        /// <returns>Return members details</returns>
        [HttpPost]
        public JsonResult PostData(FilmShooting crewMember)
        {

            if (crewMember != null)
            {
                try
                {


                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["CrewMember"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("CrewMember");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["CrewMember"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml"));
                    }
                    crewMember.CrewMemberid = crewMember.CrewMemberid + 1;

                    XmlElement Crew_TYPE = xmldoc.CreateElement("Crew_TYPE");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement Name = xmldoc.CreateElement("Name");
                    XmlElement Address1 = xmldoc.CreateElement("Address1");
                    XmlElement Address2 = xmldoc.CreateElement("Address2");
                    XmlElement LandMark = xmldoc.CreateElement("LandMark");
                    XmlElement Pincode = xmldoc.CreateElement("Pincode");
                    //XmlElement typeID = xmldoc.CreateElement("TypeID");

                    XmlElement Gender = xmldoc.CreateElement("Gender");
                    XmlElement IDType = xmldoc.CreateElement("IDType");
                    XmlElement IDNo = xmldoc.CreateElement("IDNo");
                    XmlElement Nationality = xmldoc.CreateElement("Nationality");
                    XmlElement MemberType = xmldoc.CreateElement("MemberType");



                    ID.InnerText = crewMember.CrewMemberid.ToString();
                    Name.InnerText = crewMember.Name;
                    Address1.InnerText = crewMember.Address1;
                    Address2.InnerText = crewMember.Address2;
                    LandMark.InnerText = crewMember.Landmark;
                    Pincode.InnerText = crewMember.PostalCode;
                    //typeID.InnerText =Convert.ToString(crewMember.TypeID);
                    Gender.InnerText = crewMember.Gender;
                    IDType.InnerText = crewMember.CrewIDType;
                    IDNo.InnerText = crewMember.CrewIDNo;
                    Nationality.InnerText = crewMember.Nationality;
                    MemberType.InnerText = crewMember.MemberType;


                    Crew_TYPE.AppendChild(ID);
                    Crew_TYPE.AppendChild(Name);
                    Crew_TYPE.AppendChild(Address1);
                    Crew_TYPE.AppendChild(Address2);
                    Crew_TYPE.AppendChild(LandMark);
                    Crew_TYPE.AppendChild(Pincode);
                    //Crew_TYPE.AppendChild(typeID);
                    Crew_TYPE.AppendChild(Gender);
                    Crew_TYPE.AppendChild(IDType);
                    Crew_TYPE.AppendChild(IDNo);
                    Crew_TYPE.AppendChild(Nationality);
                    Crew_TYPE.AppendChild(MemberType);

                    xmldoc.DocumentElement.AppendChild(Crew_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }

            return Json(crewMember, JsonRequestBehavior.AllowGet);
        }

          /// <summary>
          /// Function to get film shooting fees on the basis of give params
          /// </summary>
          /// <param name="districtID"></param>
          /// <param name="placeID"></param>
          /// <returns></returns>
        [HttpPost]
        public JsonResult feesByDistrictPlace(int districtID, int placeID)
        {
            List<FilmShooting> fees = new List<FilmShooting>();
            try
            {
                FilmShooting cs = new FilmShooting();
                DataTable dt = new DataTable();
                cs.DistrictID = Convert.ToString(districtID);
                cs.PlaceID = Convert.ToInt64(placeID);
                dt = cs.Select_Fees_ByDistrict_Places();
                if (dt.Rows.Count>0)
                { 
                    foreach (DataRow dr in dt.Rows)
                    {
                        fees.Add(new FilmShooting()
                        {
                            //RowID = Int64.Parse(dr["Rowid"].ToString()),
                            DistrictID = dr["DIST_CODE"].ToString(),
                            PlaceID = Convert.ToInt64(dr["PlaceID"].ToString()),
                            IndianMemberFees = Convert.ToDecimal(dr["IndianMemberFees"].ToString()),
                            NonIndianMemberFees = Convert.ToDecimal(dr["NonIndianMemberFees"].ToString()),
                            StudentFees = Convert.ToDecimal(dr["StudentFees"].ToString()),
                            Discount = Convert.ToDecimal(dr["Discount"].ToString()),
                            TaxRate = Convert.ToDecimal(dr["TaxRate"].ToString()),
                        });
                    }
                }
                else {
                    fees.Add(new FilmShooting()
                    {
                        //RowID = Int64.Parse(dr["Rowid"].ToString()),
                        DistrictID = "",
                        PlaceID = Convert.ToInt64(0),
                        IndianMemberFees = Convert.ToDecimal(0),
                        NonIndianMemberFees = Convert.ToDecimal(0),
                        StudentFees = Convert.ToDecimal(0),
                        Discount = Convert.ToDecimal(0),
                        TaxRate = Convert.ToDecimal(0),
                    });
                }
               
            }
            catch (Exception ex)
            {

            }
            return Json(fees);
        }

          /// <summary>
          /// Function to get Film Category on the basis of place 
          /// </summary>
          /// <param name="placeID"></param>
          /// <returns></returns>
        [HttpPost]
        public JsonResult FilmCategoryByPlace(int placeID)
        {
            List<SelectListItem> FilmCategory = new List<SelectListItem>();
            try
            {
                FilmShooting cs = new FilmShooting();
                DataTable dt = new DataTable();
                cs.PlaceID = Convert.ToInt64(placeID);
                dt = cs.Select_FilmCategory_ByPlaces();
                ViewBag.fname = dt;
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    FilmCategory.Add(new SelectListItem { Text = @dr["FilmCategory"].ToString(), Value = @dr["id"].ToString() });
                }

            }
            catch (Exception ex)
            {

            }
            return Json(FilmCategory, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Function to bind vehicle by category like vehicle,boat,Electra Van 
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <returns> Json result for return vehicle</returns>
        [HttpPost]
        public JsonResult vehicleByCategory(int vehicleCatID)
        {
            FilmShooting cs = new FilmShooting();
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                dt = cs.Select_vehicle(Convert.ToInt64(vehicleCatID));
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
        /// Function to get fees on the basis of vehicle 
        /// </summary>
        /// <param name="VehicleCatID"></param>
        /// <param name="VehicleID"></param>
        /// <returns>Json result vehicle fees</returns>
        [HttpPost]
        public JsonResult feesByVehicle(int VehicleCatID, int VehicleID)
        {
            List<FilmShooting> fees = new List<FilmShooting>();
            try
            {
                FilmShooting cs = new FilmShooting();
                DataTable dt = new DataTable();
                cs.VehicleCatID = Convert.ToInt64(VehicleCatID);
                cs.VehicleID = Convert.ToInt64(VehicleID);
                dt = cs.Select_Fees_Per_Vehicle();

                foreach (DataRow dr in dt.Rows)
                {
                    fees.Add(new FilmShooting()
                    {
                        VehicleCatID = Int64.Parse(dr["CategoryID"].ToString()),
                        VehicleID = Convert.ToInt64(dr["VehicleID"].ToString()),
                        VehicleFees = Convert.ToDecimal(dr["Fees"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(fees);
        }
        
        /// <summary>
        /// function to get security deposite fees by film category
        /// </summary>
        /// <param name="FilmCatID"></param>
        /// <returns>Json result by film category</returns>
        [HttpPost]
        public JsonResult FeesByFilmCategory(int FilmCatID)
        {
            List<FilmShooting> depositefees = new List<FilmShooting>();
            try
            {
                FilmShooting cs = new FilmShooting();
                DataTable dt = new DataTable();
                cs.FilmCategoryID =FilmCatID;
                dt = cs.Select_Fees_By_FilmCategory();

                foreach (DataRow dr in dt.Rows)
                {
                    depositefees.Add(new FilmShooting()
                    {
                        DepositeAmount = Convert.ToDecimal(dr["Amount"].ToString()),
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(depositefees, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to create xml file of all vehicle details
        /// </summary>
        /// <param name="FS"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostVehicleData(FilmShooting FS)
        {
            if (FS != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();
                    if (Session["Vehicle"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("VehicleFees");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["Vehicle"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                    }
                    XmlElement Veh_TYPE = xmldoc.CreateElement("Veh_TYPE");
                    XmlElement CatID = xmldoc.CreateElement("CatID");
                    XmlElement Category = xmldoc.CreateElement("Category");
                    XmlElement VehicleID = xmldoc.CreateElement("VehicleID");
                    XmlElement Vehicle = xmldoc.CreateElement("Vehicle");
                    XmlElement TotalVehicle = xmldoc.CreateElement("TotalVehicle");
                    XmlElement VehicleFees = xmldoc.CreateElement("VehicleFees");



                    CatID.InnerText = Convert.ToString(FS.VehicleCatID);
                    Category.InnerText = FS.VehicleCategory;
                    VehicleID.InnerText = Convert.ToString(FS.VehicleID);
                    Vehicle.InnerText = FS.Vehicle;
                    TotalVehicle.InnerText = Convert.ToString(FS.TotalVehicle);
                    VehicleFees.InnerText = Convert.ToString(FS.VehicleFees);


                    Veh_TYPE.AppendChild(CatID);
                    Veh_TYPE.AppendChild(Category);
                    Veh_TYPE.AppendChild(VehicleID);
                    Veh_TYPE.AppendChild(Vehicle);
                    Veh_TYPE.AppendChild(TotalVehicle);
                    Veh_TYPE.AppendChild(VehicleFees);

                    xmldoc.DocumentElement.AppendChild(Veh_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));

                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }

            }

            return Json(FS, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Function to Get members details from xml file
        /// </summary>
        /// <returns>return members details from xml file</returns>
        [HttpPost]
        public JsonResult GetShootingcrewmwmber()
        {
            var result = new List<FilmShooting>();
            FilmShooting crewMember = null;
            try
            {
                if (Session["CrewMember"] != null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml"));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            crewMember = new FilmShooting()
                            {
                                CrewMemberid = Int64.Parse(dr["ID"].ToString()),
                                Name = dr["Name"].ToString(),
                                Address1 = dr["Address1"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                Landmark = dr["LandMark"].ToString(),
                                PostalCode = dr["Pincode"].ToString()



                            };
                            result.Add(crewMember);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to Get vwhicle details from xml file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEquipmentMaterial()
        {
            var result = new List<FilmShooting>();
            FilmShooting equipment = null;
            try
            {
                if (Session["Vehicle"] != null)
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            equipment = new FilmShooting()
                            {
                                VehicleCatID = Int64.Parse(dr["CatID"].ToString()),
                                VehicleCategory = dr["Category"].ToString(),
                                Vehicle = dr["Vehicle"].ToString(),
                                TotalVehicle = Convert.ToInt64(dr["TotalVehicle"].ToString()),
                                VehicleFees = Convert.ToDecimal(dr["VehicleFees"].ToString())
                            };
                            result.Add(equipment);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Function to save all data to database
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="IDProofUrl"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitFilmshootingPermission(FormCollection fm, HttpPostedFileBase IDProofUrl, string Command)
        {
            ActionResult actionResult = null;
            try
            {
                Session["PaymentType"] = "FilmShooting";
                string IDProofFile = string.Empty;
                var path = "";
                FilmShooting fs = new FilmShooting();
                if (fm["ApplicantType"].ToString() == "")
                {
                    fs.ApplicantType = Convert.ToInt32("");
                }
                else
                {
                    fs.ApplicantType = Convert.ToInt32(fm["ApplicantType"].ToString());
                    fs.ApplicantName = fm["AppLicant"].ToString();
                }
                if (fm["Title"].ToString() == "")
                {
                    fs.Title = "";
                }
                else
                {
                    fs.Title = fm["Title"].ToString();
                }
                if (fm["Description"].ToString() == "")
                {
                    fs.Description = "";
                }
                else
                {
                    fs.Description = fm["Description"].ToString();
                }
                if (fm["NoOfCrewMember"].ToString() == "")
                {
                    fs.NoOfCrewMember = "";
                }
                else
                {
                    fs.NoOfCrewMember = fm["NoOfCrewMember"].ToString();
                }
                if (fm["DurationFrom"].ToString() == "")
                {
                    fs.DurationFrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                   // fs.DurationFrom = Convert.ToDateTime(fm["DurationFrom"].ToString());
                    fs.DurationFrom = DateTime.ParseExact(fm["DurationFrom"].ToString(), "dd/MM/yyyy", null); 
                }
                if (fm["DurationTo"].ToString() == "")
                {
                    fs.DurationTo =  Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    fs.DurationTo = DateTime.ParseExact(fm["DurationTo"].ToString(), "dd/MM/yyyy", null); 
                    //fs.DurationTo = Convert.ToDateTime(fm["DurationTo"].ToString());
                }
                if (fm["ShootingPurpose"].ToString() == "")
                {
                    fs.ShootingPurpose = "";
                }
                else
                {
                    fs.ShootingPurpose = fm["ShootingPurpose"].ToString();
                }
                if (fm["IdentityProof"].ToString() == "")
                {
                    fs.IdentityProof = "";
                }
                else
                {
                    fs.IdentityProof = fm["IdentityProof"].ToString();
                }
                if (fm["IdentityProofNo"].ToString() == "")
                {
                    fs.IdentityProofNo = "";
                }
                else
                {
                    fs.IdentityProofNo = fm["IdentityProofNo"].ToString();
                }
                if (IDProofUrl != null && IDProofUrl.ContentLength > 0)
                {
                    IDProofFile = Path.GetFileName(IDProofUrl.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + IDProofFile;
                    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    fs.IDProofUrl = path;
                    IDProofUrl.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);
                }
                else
                { fs.IDProofUrl = ""; }
                if (fm["Districts"].ToString() == "")
                {
                    fs.DistrictID = "0";

                }
                else
                {
                    fs.DistrictID = fm["Districts"].ToString();
                    fs.DistrictName = fm["disName"].ToString();
                }
                if (fm["Place"].ToString() == "")
                {
                    fs.PlaceID = 0;
                }
                else
                {
                    fs.PlaceID =Convert.ToInt64(fm["Place"].ToString());
                    fs.PlaceName = fm["PlcName"].ToString();
                }
                if (fm["IndianCitizen"].ToString() == "")
                {
                    fs.IndianCitizen = 0;
                }
                else
                {
                    fs.IndianCitizen = Convert.ToInt64(fm["IndianCitizen"].ToString());
                }
                if (fm["NonIndianCitizen"].ToString() == "")
                {
                    fs.NonIndianCitizen = 0;
                }
                else
                {
                    fs.NonIndianCitizen = Convert.ToInt64(fm["NonIndianCitizen"].ToString());
                }
                if (fm["Student"].ToString() == "")
                {
                    fs.Student = 0;
                }
                else
                {
                    fs.Student = Convert.ToInt64(fm["Student"].ToString());
                }
                if (fm["TotalFees"].ToString() == "")
                {
                    fs.TotalFees = 0;
                }
                else
                {
                    fs.TotalFees = Convert.ToInt64(fm["TotalFees"].ToString());
                }
                if (fm["ddl_filmCategory"].ToString() == "")
                {
                    fs.FilmCategory = "";
                }
                else
                {
                    fs.FilmCategory = fm["ddl_filmCategory"].ToString();
                }
                if (fm["DepositeAmount"].ToString() == "") 
                {
                    fs.DepositeAmount = Convert.ToDecimal(0);
                }
                else
                {
                    fs.DepositeAmount = Convert.ToDecimal(fm["DepositeAmount"].ToString());
                }
                if (fm["VehicleFees"].ToString() == "")
                {
                    fs.VehicleFees = 0;
                }
                else
                {
                    fs.VehicleFees = Convert.ToDecimal(fm["VehicleFees"].ToString());
                }
                string RequestId = DateTime.Now.Ticks.ToString();
                Session["RequestId"] = RequestId;
                fs.TransactionID = RequestId;

                if (Command == "Proceed")
                {

                    Session["FilmShooting"] = fs;

                    return RedirectToAction("FilmShooting", "FilmShooting");
                }




                if (Command == "Payment")
                {

                    #region insertion
                    fs.EnteredBy = Convert.ToInt64(Session["UserId"]);
                    DataSet ds = new DataSet();
                    if (Session["CrewMember"] != null)
                    {
                        ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml"));
                    }
                    else
                    {
                        ds = null;
                    }
                    DataSet dsv = new DataSet();
                    if (Session["Vehicle"] != null)
                    {
                        dsv.ReadXml(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                    }
                    else
                    {
                        dsv.Clear();
                        DataTable objDt2 = new DataTable("Table");

                        objDt2.Columns.Add("CatID", typeof(String));
                        objDt2.Columns.Add("Category", typeof(String));
                        objDt2.Columns.Add("VehicleID", typeof(String));
                        objDt2.Columns.Add("Vehicle", typeof(String));
                        objDt2.Columns.Add("TotalVehicle", typeof(String));
                        objDt2.Columns.Add("VehicleFees", typeof(String));
                        objDt2.AcceptChanges();
                        dsv.Tables.Add(objDt2);
                        objDt2.Clear();
                        objDt2.AcceptChanges();
                    }


                    Int64 i = fs.Submit_FilmShootingPermission(ds.Tables[0], dsv.Tables[0]);
                    string abc = fs.Title;
                    // string status = fs.CreateOrganizingCamp(ds.Tables[0]);
                    if (Session["CrewMember"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml"));
                            Session["CrewMember"] = null;
                        }
                    }
                    if (Session["Vehicle"] != null)
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml")) == true)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                            Session["Vehicle"] = null;
                        }
                    }
                    string Message = string.Empty;
                    if (i > 0)
                    {
                        Message = "Record Saved Sucessfully";
                    }
                    else if (i == 0)
                    {
                        Message = "Record Already Exists!!!";
                    }
                    else
                    {
                        Message = "An Error occured";
                    }

                    #endregion


                    #region payment
                    DataTable dtColmn = new DataTable();
                    if (dtColmn.Rows.Count == 0)
                    {
                        dtColmn.Columns.Add("Transaction_Id");
                        dtColmn.Columns.Add("Memberfees");
                        dtColmn.Columns.Add("VechileFees");
                        dtColmn.Columns.Add("DepositFees");
                        dtColmn.Columns.Add("EnterBy");
                        dtColmn.Columns.Add("Status");
                    }
                    decimal totalPrice = 0;
                    totalPrice = fs.TotalFees + fs.DepositeAmount + fs.VehicleFees;
                    Session["totalprice"] = totalPrice;
                    DataRow dtrow = dtColmn.NewRow();
                    dtrow["Transaction_Id"] = Session["RequestId"].ToString();
                    dtrow["Memberfees"] = fs.TotalFees;
                    dtrow["VechileFees"] = fs.VehicleFees;
                    dtrow["DepositFees"] = fs.DepositeAmount;
                    dtrow["EnterBy"] = Session["User"].ToString();
                    dtrow["Status"] = "Pending";
                    dtColmn.Rows.Add(dtrow);
                    ViewData.Model = dtColmn.AsEnumerable();
                    return View("Payment");
                    #endregion
                }
                if (Command == "Submit")
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actionResult;
        }
        /// <summary>
        /// Function to delete xml files that is created for members details and vehicle details.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ClearFilmShootingData()
        {
            var result = "";
            if (Session["CrewMember"] != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml")) == true)
                {
                    System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["CrewMember"].ToString() + ".xml"));
                    Session["CrewMember"] = null;
                    result = "Data Clear";
                }
            }

            if (Session["Vehicle"] != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml")) == true)
                {
                    System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["Vehicle"].ToString() + ".xml"));
                    Session["Vehicle"] = null;
                    result = "Data Clear";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Auther : Rajkumar
        /// Description : Used for encryption of payment parameter
        /// </summary>
        [HttpPost]
        public void Pay()
        {
            string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();

            //EM33172142@5488
            Payment pay = new Payment();
            string encrypt = pay.RequestString("EM33172142@5488", Session["RequestId"].ToString(), Session["totalprice"].ToString(), ReturnUrl + "FilmShooting/Payment", Session["User"].ToString(), "", "");
            Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);           
        }
        /// <summary>
        /// Auther : Rajkumar
        /// Description : Used Getting Response from emitra 
        /// </summary>
        /// <returns></returns>
        public ActionResult Payment()
        {
            if (Session["RequestId"] != null)
            {
                FilmShooting fs = new FilmShooting();
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
                    fs.TransactionID = finalreqid;
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
                        fs.Trn_Status_Code = 0;
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
                    fs.TransactionID = finalreqid;
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
                        fs.Trn_Status_Code = 1;
                    }                   
                    dt.Rows.Add(dtrow);
                }
                #endregion

                SMS_EMail_Services SE = new SMS_EMail_Services();
                if (Session["PaymentType"].ToString() == "FilmShooting")
                {
                    DataTable dtf = new DataTable();
                    dtf = fs.UpdateTransactionStatus("1");
                    if (dtf != null)
                    {
                        if (dtf.Rows.Count > 0)
                        {
                            #region SMS Email Integration
                            string subject = "Request for Film Shooting Permission Review";
                            string body = Common.GenerateReviwerBody(dtf.Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), "Film Shooting Permission");
                            SE.sendEMail(subject, body, dtf.Rows[0]["EmailId"].ToString(), "");
                            // SMS_EMail_Services.sendBulkSMS(dtf.Rows[0]["Mobile"].ToString(), "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                            if (Session["SSODetail"] != null)
                            {
                                UserProfile up = (UserProfile)Session["SSODetail"];
                                SMS_EMail_Services.sendSingleSMS(up.MobileNumber, "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                            }
                            #endregion
                        }
                    }
                }
                if (Session["PaymentType"].ToString() == "OrganisingCamp")
                {
                    DataTable dtf = new DataTable();
                    dtf = fs.UpdateTransactionStatus("2");
                    if (dtf != null)
                    {
                        if (dtf.Rows.Count > 0)
                        {
                            #region SMS Email
                            string subject = "Request for FilmOrganisingCamp Review";
                            string body = Common.GenerateReviwerBody(dtf.Rows[0]["Name"].ToString(), Session["RequestId"].ToString(), "OrganisingCamp");
                            SE.sendEMail(subject, body, dtf.Rows[0]["EmailId"].ToString(), "");
                            // SMS_EMail_Services.sendBulkSMS(dtf.Rows[0]["Mobile"].ToString(), "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                            if (Session["SSODetail"] != null)
                            {
                                UserProfile up = (UserProfile)Session["SSODetail"];
                                SMS_EMail_Services.sendSingleSMS(up.MobileNumber, "Your request" + "(" + Session["RequestId"].ToString() + ") has been Submitted successfully");
                            }
                            #endregion
                        }
                    }
                }
                ViewData.Model = dt.AsEnumerable();
                return View("TransactionStatus");
            }
            return View();
        }
        public ActionResult InsertData()
        {
            //actionResult = View("OrcfeePayment", fs);
            //return View("FilmShooting","FilmShooting");
            return RedirectToAction("FilmShooting", "FilmShooting");
        }
        #endregion

        #region Organising Camp
        /// <summary>
        ///Call when request come for OrganisingCamp view Bind Districts
        /// </summary>
        /// <returns>View for FilmShooting</returns>
        public ActionResult OrganisingCamp()
        {
            return getDistrict();
        }
        /// <summary>
        /// Function to create xml file with all members details
        /// </summary>
        /// <param name="crewMember"></param>
        /// <returns>Return members details</returns>
        [HttpPost]
        public JsonResult PostDataCrew(FilmShooting crewMember)
        {

            if (crewMember != null)
            {
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    string filename = DateTime.Now.Ticks.ToString();
                    if (Session["orgCrewMember"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("CrewMember");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["orgCrewMember"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml"));
                    }
                    crewMember.CrewMemberid = crewMember.CrewMemberid + 1;

                    XmlElement Crew_TYPE = xmldoc.CreateElement("Crew_TYPE");
                    XmlElement ID = xmldoc.CreateElement("ID");
                    XmlElement Name = xmldoc.CreateElement("Name");
                    XmlElement Address1 = xmldoc.CreateElement("Address1");
                    XmlElement Address2 = xmldoc.CreateElement("Address2");
                    XmlElement LandMark = xmldoc.CreateElement("LandMark");
                    XmlElement Pincode = xmldoc.CreateElement("Pincode");

                    XmlElement Gender = xmldoc.CreateElement("Gender");
                    XmlElement IDType = xmldoc.CreateElement("IDType");
                    XmlElement IDNo = xmldoc.CreateElement("IDNo");
                    XmlElement Nationality = xmldoc.CreateElement("Nationality");
                    XmlElement MemberType = xmldoc.CreateElement("MemberType");

                    ID.InnerText = crewMember.CrewMemberid.ToString();
                    Name.InnerText = crewMember.Name;
                    Address1.InnerText = crewMember.Address1;
                    Address2.InnerText = crewMember.Address2;
                    LandMark.InnerText = crewMember.Landmark;
                    Pincode.InnerText = crewMember.PostalCode;

                    Gender.InnerText = crewMember.Gender;
                    IDType.InnerText = crewMember.CrewIDType;
                    IDNo.InnerText = crewMember.CrewIDNo;
                    Nationality.InnerText = crewMember.Nationality;
                    MemberType.InnerText = crewMember.MemberType;

                    Crew_TYPE.AppendChild(ID);
                    Crew_TYPE.AppendChild(Name);
                    Crew_TYPE.AppendChild(Address1);
                    Crew_TYPE.AppendChild(Address2);
                    Crew_TYPE.AppendChild(LandMark);
                    Crew_TYPE.AppendChild(Pincode);

                    Crew_TYPE.AppendChild(Gender);
                    Crew_TYPE.AppendChild(IDType);
                    Crew_TYPE.AppendChild(IDNo);
                    Crew_TYPE.AppendChild(Nationality);
                    Crew_TYPE.AppendChild(MemberType);

                    xmldoc.DocumentElement.AppendChild(Crew_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml"));
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }

            return Json(crewMember, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to get camp fees on the basis of given param values
        /// </summary>
        /// <param name="PlaceID"></param>
        /// <param name="DistrictID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetCampFees(Int64 PlaceID, Int64 DistrictID)
        {
            OrganisingCamps organisingCamp = new OrganisingCamps();
            DataTable dtc = organisingCamp.GetOrganizingCampfees(PlaceID, DistrictID);
            decimal campfee = 0;
            if (dtc != null)
            {
                if (dtc.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtc.Rows)
                    {
                        campfee = Convert.ToDecimal(dr[0].ToString());
                    }
                }
            }
            return Json(campfee, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function to get Members details fromxml file
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCrewMember()
        {
            var result = new List<FilmShooting>();
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml"));

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FilmShooting crewMember = new FilmShooting();
                            crewMember.CrewMemberid = Int64.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                            crewMember.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                            crewMember.Address1 = ds.Tables[0].Rows[i]["Address1"].ToString();
                            crewMember.Address2 = ds.Tables[0].Rows[i]["Address2"].ToString();
                            crewMember.Landmark = ds.Tables[0].Rows[i]["LandMark"].ToString();
                            crewMember.PostalCode = ds.Tables[0].Rows[i]["Pincode"].ToString();
                            result.Add(crewMember);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

      /// <summary>
        ///   Function to delete xml file after data submission to database
      /// </summary>
      /// <returns></returns>
        [HttpPost]
        public JsonResult ClearData()
        {
            var result = "";
            if (Session["orgCrewMember"] != null)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml")) == true)
                {
                    System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml"));
                    Session["orgCrewMember"] = null;
                    result = "Data Clear";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  This  ActionResult Method will get all submit form data, call the model method  for insert data in database and finally return on view
        /// </summary>
        /// <param name="fm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrganizingCamp(FormCollection fm, HttpPostedFileBase FileNameOfIdProof, string Command)
        {
            ActionResult actionResult = null;
            try
            {
                Session["PaymentType"] = "OrganisingCamp";
                OrganisingCamps oc = new OrganisingCamps();
                string IDProofFileOrg = string.Empty;
                var path = "";
                string requesteId = GetRequestedId();

                if (requesteId == "")
                {
                    oc.RequestedId = "";
                }
                else
                {
                    oc.RequestedId = requesteId;
                    Session["RequestId"] = oc.RequestedId;
                }
                if (Session["UserId"] != null)
                {
                    // user = (User)Session["SSODetail"];
                    oc.CreatedBy = Convert.ToInt64(Session["UserId"]); ; //user.UserId;
                }
                else
                {
                    oc.CreatedBy =0; 
                }

                if (fm["ApplicantType"].ToString() == "")
                {
                    oc.ApplicantType = "";
                }
                else
                {

                    oc.ApplicantType = fm["ApplicantType"].ToString();
                    oc.ApplicantCat = fm["AppLicant"].ToString();
                }

                if (fm["Districts"].ToString() == "")
                {
                    oc.Ddlistrict = "0";
                }
                else
                {
                    oc.Ddlistrict =fm["Districts"].ToString();
                    oc.DistrictName = fm["DisName"].ToString();
                }
                if (fm["Place"].ToString() == "")
                {
                    oc.Location = "";
                }
                else
                {
                    oc.Location = fm["Place"].ToString();
                    oc.LocationName = fm["Place"].ToString();
                }
                if (fm["PurposeOfCamp"].ToString() == "")
                {
                    oc.PurposeOfCamp = "";
                }
                else
                {
                    oc.PurposeOfCamp = fm["PurposeOfCamp"].ToString();
                }
                if (fm["Noofmembercamp"].ToString() == "")
                {
                    oc.Noofmembercamp = "";
                }
                else
                {
                    oc.Noofmembercamp = fm["Noofmembercamp"].ToString();
                }
                if (fm["IdProofType"].ToString() == "")
                {
                    oc.IdProofType = "";
                }
                else
                {
                    oc.IdProofType = fm["IdProofType"].ToString();
                   // oc.IdProofName = fm["IdentityProofType"].ToString();
                }
                if (fm["IdProofNo"].ToString() == "")
                {
                    oc.IdProofNo = "";
                }
                else
                {
                    oc.IdProofNo = fm["IdProofNo"].ToString();
                }
                if (FileNameOfIdProof != null && FileNameOfIdProof.ContentLength > 0)
                {
                    IDProofFileOrg = Path.GetFileName(FileNameOfIdProof.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + IDProofFileOrg;
                    path = Path.Combine(Server.MapPath("~/PermissionDocument/"), FileFullName);
                    oc.FileNameOfIdProof = FileFullName;
                   // oc.PathofFileOfIdProof = path;
                    oc.PathofFileOfIdProof = @"~/PermissionDocument/" + FileFullName.Trim();
                    FileNameOfIdProof.SaveAs(Server.MapPath("~/PermissionDocument/") + FileFullName);                   
                }
                else
                {
                    oc.FileNameOfIdProof = "";
                    oc.PathofFileOfIdProof = "";
                }
                if (fm["Durationfrom"].ToString() == "")
                {
                    oc.Durationfrom = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    // fs.DurationFrom = Convert.ToDateTime(fm["DurationFrom"].ToString());
                    oc.Durationfrom = DateTime.ParseExact(fm["DurationFrom"].ToString(), "dd/MM/yyyy", null);
                }                
                if (fm["Durationto"].ToString() == "")
                {
                    oc.Durationto = Convert.ToDateTime(SqlDateTime.Null);
                }
                else
                {
                    oc.Durationto = DateTime.ParseExact(fm["Durationto"].ToString(), "dd/MM/yyyy", null); 
                }
                if (fm["Noofdayscamp"].ToString() == "")
                {
                    oc.Noofdayscamp = "";
                }
                else
                {
                    oc.Noofdayscamp = fm["Noofdayscamp"].ToString();
                }
                if (fm["Processingfees"].ToString() == "")
                {
                    oc.Processingfees = "";
                }
                else
                {
                    oc.Processingfees = fm["Processingfees"].ToString();

                }

                if (Command == "Proceed")
                {

                    Session["OrcCamp"] = oc;

                    return RedirectToAction("OrganisingCamp", "FilmShooting");
                }



                if (Command == "Payment")
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml"));
                    string status = oc.CreateOrganizingCamp(ds.Tables[0]);
                    if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml")) == true)
                    {
                        System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["orgCrewMember"].ToString() + ".xml"));
                        Session["orgCrewMember"] = null;
                    }
                    if (status != "")
                    {
                        Session["Status"] = "Your Request has been Sucessfully Submited and  Requested Id:" + status;
                    }
                    else
                    {
                        Session["Status"] = "Not inserted";
                    }
                    actionResult = View("OrganisingCamp", oc);

                    #region payment
                    DataTable dtColmn = new DataTable();
                    if (dtColmn.Rows.Count == 0)
                    {
                        dtColmn.Columns.Add("Transaction_Id");
                        dtColmn.Columns.Add("ProcessingFees");
                        dtColmn.Columns.Add("EnterBy");
                        dtColmn.Columns.Add("Status");
                    }
                    decimal totalPrice = 0;
                    totalPrice = Convert.ToDecimal(oc.Processingfees);
                    Session["totalprice"] = totalPrice;
                    DataRow dtrow = dtColmn.NewRow();
                    dtrow["Transaction_Id"] = oc.RequestedId;
                    dtrow["ProcessingFees"] = oc.Processingfees;
                    dtrow["EnterBy"] = Session["User"].ToString();
                    dtrow["Status"] = "Pending";
                    dtColmn.Rows.Add(dtrow);
                    ViewData.Model = dtColmn.AsEnumerable();
                    return View("Payment");
                    #endregion
                }
                else if (Command == "Cancel")
                {
                    actionResult = RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actionResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetRequestedId()
        {
            DateTime now = DateTime.Now;
            string id = now.Ticks.ToString();
            return id;
        }


        #endregion
    }
}
