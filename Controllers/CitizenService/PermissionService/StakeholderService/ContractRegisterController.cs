
// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 18-Nov-2015
// *****************************************************************************************
// <summary>This Page is developed for Contractor Registraton of Stake holder services </summary>
// *****************************************************************************************
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.CitizenService.PermissionService.StakeholderService;
using System.Data.SqlClient;
using System.Xml;

namespace FMDSS.Controllers.CitizenService.PermissionService.StakeholderService
{
    public class ContractRegisterController : BaseController
    {
        DAL dal = new DAL();
        DataSet dsdropdiv = new DataSet();
      //  User user = null;
       /// <summary>
       /// This Controller returns registration page
       /// </summary>
       /// <returns></returns>       
        public ActionResult ContractRegister()
        {
            try
            {
                #region Bind dropdownlist
                StakeholderServices shs = new StakeholderServices();
                List<SelectListItem> vehicleCategory = new List<SelectListItem>();
                DataTable dt = new DataTable();
                dt = shs.GetVehicleType();
                ViewBag.Vehiclecat = dt;
                foreach (System.Data.DataRow dr in ViewBag.Vehiclecat.Rows)
                {
                    vehicleCategory.Add(new SelectListItem { Text = @dr["CategoryName"].ToString(), Value = @dr["CategoryID"].ToString() });
                }
                ViewBag.Vehiclecat = vehicleCategory;


                List<SelectListItem> District = new List<SelectListItem>();
                DataSet dsDist = new DataSet();
                SqlParameter[] param1 = {new SqlParameter("@Div_Code",""), 
                                        new SqlParameter("@Range_Code",""),                      
                                        new SqlParameter("@Village_Code",""),    
                                        new SqlParameter("@Depot_Code",""), 
                                        new SqlParameter("@Dist_Code",""), 
                                        new SqlParameter("@option","5"),  
                                       };
                dal.Fill(dsDist, "Sp_getForestlocation", param1);
                ViewBag.DistrictName = dsDist.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.DistrictName.Rows)
                {
                    District.Add(new SelectListItem { Text = @dr["DIST_NAME"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                }
                ViewBag.DistrictName = District;




                List<SelectListItem> items = new List<SelectListItem>();                           
                SqlParameter[] param = {new SqlParameter("@Div_Code",""), 
                                        new SqlParameter("@Range_Code",""),                      
                                        new SqlParameter("@Village_Code",""),    
                                        new SqlParameter("@Depot_Code",""), 
                                        new SqlParameter("@Dist_Code",""), 
                                        new SqlParameter("@option","1"),  
                                       };              
                dal.Fill(dsdropdiv, "Sp_getForestlocation", param);

                ViewBag.fname = dsdropdiv.Tables[0];
                foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
                }
                ViewBag.division = items;
                return View();
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        /// <summary>
        /// Get list of vehicle category 
        /// </summary>
        /// <param name="vehicleCatID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult vehicleByCategory(string vehicleCatID)
        {          
            List<SelectListItem> vehicle = new List<SelectListItem>();
            try
            {
                StakeholderServices shs = new StakeholderServices();
                DataTable dt = new DataTable();
                dt = shs.Select_vehicle(Convert.ToInt64(vehicleCatID));
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
        /// Used to create runtime xml file and return json 
        /// </summary>
        /// <param name="Stk"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PostVehicleData(StakeholderServices Stk)
        {
            if (Stk != null)
            {
                try
                {
                    #region XmlDocument
                    XmlDocument xmldoc = new XmlDocument();

                    string filename = DateTime.Now.Ticks.ToString();

                    if (Session["filename"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("VehicleFees");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["filename"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    }


                    XmlElement Veh_TYPE = xmldoc.CreateElement("Veh_TYPE");
                    XmlElement CatID = xmldoc.CreateElement("CatID");
                    XmlElement Category = xmldoc.CreateElement("Category");
                    XmlElement VehicleID = xmldoc.CreateElement("VehicleID");
                    XmlElement Vehicle = xmldoc.CreateElement("Vehicle");
                    XmlElement VehicleNo = xmldoc.CreateElement("VehicleNo");                   
                    XmlElement VehicleSeat = xmldoc.CreateElement("VehicleSeat");

                    CatID.InnerText = Convert.ToString(Stk.VehicleCatID);
                    Category.InnerText = Stk.VehicleCategory;
                    VehicleID.InnerText = Convert.ToString(Stk.VehicleID);
                    Vehicle.InnerText = Stk.Vehicle;
                    VehicleNo.InnerText = Stk.VehicleNo;                 
                    VehicleSeat.InnerText = Stk.VehicleSeat;
                   // VehicleFees.InnerText = Convert.ToString(FS.VehicleFees);


                    Veh_TYPE.AppendChild(CatID);
                    Veh_TYPE.AppendChild(Category);
                    Veh_TYPE.AppendChild(VehicleID);
                    Veh_TYPE.AppendChild(Vehicle);
                    Veh_TYPE.AppendChild(VehicleNo);                   
                    Veh_TYPE.AppendChild(VehicleSeat);

                    xmldoc.DocumentElement.AppendChild(Veh_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    #endregion
                }
                catch (Exception ex)
                {
                    Console.Write("Error" + ex);
                }
            }
            return Json(Stk, JsonRequestBehavior.AllowGet);
        }

        public void XmldataInsert(string ReqId) {
            StakeholderServices ShS = new StakeholderServices();
            XmlDocument xmldoc = new XmlDocument();          
            DataSet dsXml = new DataSet();
            xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
            dsXml.ReadXml(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
            if (dsXml.Tables.Count > 0) {
                if (dsXml.Tables[0].Rows.Count > 0) {
                    
                 for(int i=0;i<dsXml.Tables[0].Rows.Count;i++){
                     ShS.VechileType = dsXml.Tables[0].Rows[i]["Category"].ToString();
                     ShS.Vehicle = dsXml.Tables[0].Rows[i]["Vehicle"].ToString();
                     ShS.VehicleNo = dsXml.Tables[0].Rows[i]["VehicleNo"].ToString();
                     ShS.VehicleSeat = dsXml.Tables[0].Rows[i]["VehicleSeat"].ToString();
                     ShS.InsertXmldata(ReqId);
                 }                                     
                 //   ShS.InsertXmldata();
                }                            
            }

        }
    
        /// <summary>
        /// Used to bind dropdownlist for range on selecting division
        /// </summary>
        /// <param name="division"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getRange(string division)
        {
            SqlParameter[] param = {new SqlParameter("@Div_Code",division), 
                                        new SqlParameter("@Range_Code",""),                      
                                        new SqlParameter("@Village_Code",""),    
                                        new SqlParameter("@Depot_Code",""), 
                                        new SqlParameter("@Dist_Code",""), 
                                        new SqlParameter("@option","2"),  
                                       };
            dal.Fill(dsdropdiv, "Sp_getForestlocation", param);
            ViewBag.fname = dsdropdiv.Tables[0];
            List<SelectListItem> range = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                range.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
            }
            return Json(new SelectList(range, "Value", "Text"));
        }
        /// <summary>
        ///  Used to bind dropdownlist for village on selecting range
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getVillage(string range)
        {
            SqlParameter[] param = {new SqlParameter("@Div_Code",""), 
                                        new SqlParameter("@Range_Code",range),                      
                                        new SqlParameter("@Village_Code",""),    
                                        new SqlParameter("@Depot_Code",""), 
                                        new SqlParameter("@Dist_Code",""), 
                                        new SqlParameter("@option","3"),  
                                       };
            dal.Fill(dsdropdiv, "Sp_getForestlocation", param);
            ViewBag.fname = dsdropdiv.Tables[0];
            List<SelectListItem> vill = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                vill.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
            }
            return Json(new SelectList(vill, "Value", "Text"));
        }
        /// <summary>
        ///  Used to bind dropdownlist for depot on selecting village
        /// </summary>
        /// <param name="village"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getDepot(string village)
        {
            SqlParameter[] param = {new SqlParameter("@Div_Code",""), 
                                        new SqlParameter("@Range_Code",""),                      
                                        new SqlParameter("@Village_Code",village),    
                                        new SqlParameter("@Depot_Code",""), 
                                        new SqlParameter("@Dist_Code",""), 
                                        new SqlParameter("@option","4"),  
                                       };
            dal.Fill(dsdropdiv, "Sp_getForestlocation", param);
            ViewBag.fname = dsdropdiv.Tables[0];
            List<SelectListItem> depot = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                depot.Add(new SelectListItem { Text = @dr["Depot_Location"].ToString(), Value = @dr["Depot_Location"].ToString() });
            }
            return Json(new SelectList(depot, "Value", "Text"));
        }
        /// <summary>
        /// Used to bind dropdownlist of national park on selecting district
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getNationalPark(string district)
        {
            DataSet dsNationalPark = new DataSet();
            SqlParameter[] param = {new SqlParameter("@Div_Code",""), 
                                        new SqlParameter("@Range_Code",""),                      
                                        new SqlParameter("@Village_Code",""),    
                                        new SqlParameter("@Depot_Code",""), 
                                        new SqlParameter("@Dist_Code",district), 
                                        new SqlParameter("@option","6"),  
                                       };
            dal.Fill(dsNationalPark, "Sp_getForestlocation", param);
            ViewBag.fname = dsNationalPark.Tables[0];
            List<SelectListItem> nationalpark = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                nationalpark.Add(new SelectListItem { Text = @dr["NationalPark_Sanctuary"].ToString(), Value = @dr["NationalPark_Sanctuary"].ToString() });
            }
            return Json(new SelectList(nationalpark, "Value", "Text"));
        }
        /// <summary>
        /// This Action result used submit form value to database
        /// </summary>
        /// <param name="frmDetails"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewContractor(FormCollection frmDetails, string command)
        {
            try
            {
                #region Validation Check
                StakeholderServices  objDA = new StakeholderServices();
                //ID
               
                objDA.ContractorID = 0;

                //Applicant Type
                if (frmDetails["ApplicantType"]==null)
                {
                    objDA.ApplicantType = string.Empty;
                }
                else
                {
                    objDA.ApplicantType = frmDetails["ApplicantType"].ToString();
                }

                if (frmDetails["Registration_Loc"] == null)
                {
                    objDA.Location_type = string.Empty;
                }
                else
                {
                    objDA.Location_type = frmDetails["Registration_Loc"].ToString();
                }

                if (frmDetails["Registration_type"] == null)
                {
                    objDA.RegistrationType = string.Empty;
                }
                else
                {
                    objDA.RegistrationType = frmDetails["Registration_type"].ToString();
                }  

                //District
                if (frmDetails["District"]==null)
                {
                    objDA.District = string.Empty;
                }
                else
                {
                    objDA.District = frmDetails["District"].ToString();
                }

                if (frmDetails["Dropnational"] == null)
                {
                    objDA.National_park = string.Empty;
                }
                else
                {
                    objDA.National_park = frmDetails["Dropnational"].ToString();
                }

                if (frmDetails["Dropdivision"] == null)
                {
                    objDA.Division = string.Empty;
                }
                else
                {
                    objDA.Division = frmDetails["Dropdivision"].ToString();
                }

                if (frmDetails["Droprange"] == null)
                {
                    objDA.Range = string.Empty;
                }
                else
                {
                    objDA.Range = frmDetails["Droprange"].ToString();
                }

                if (frmDetails["Dropvillage"] == null)
                {
                    objDA.village = string.Empty;
                }
                else
                {
                    objDA.village = frmDetails["Dropvillage"].ToString();
                }

                if (frmDetails["Dropdepot"] == null)
                {
                    objDA.Depot = string.Empty;
                }
                else
                {
                    objDA.Depot = frmDetails["Dropdepot"].ToString();
                }

                if (frmDetails["Registration_type"] == null)
                {
                    objDA.RegistrationType = string.Empty;
                }
                else
                {
                    objDA.RegistrationType = frmDetails["Registration_type"].ToString();
                }

                if (frmDetails["Produce_type"] == null)
                {
                    objDA.ProduceType = string.Empty;
                }
                else
                {
                    objDA.ProduceType = frmDetails["Produce_type"].ToString();
                }

                //isActive
                objDA.isActive = 1;

                //UserID
                if (Session["SSODetail"] != null)
                {
                    //user = (User)Session["SSODetail"];
                    //objDA.UserID = user.UserId;
                }

                //Commments
                if (frmDetails["Comments"]==null)
                {
                    objDA.Commments = string.Empty;
                }
                else
                {
                    objDA.Commments = frmDetails["Comments"].ToString();
                }

                objDA.RequestID = DateTime.Now.Ticks.ToString();

                if (command == "Save")
                {
                    string strContractorID = objDA.RegisterContractor();
                    XmldataInsert(objDA.RequestID);
                    if (Convert.ToInt32(strContractorID) > 0)
                        Session["Status"] = "Contractor created successfully with request ID = " + objDA.RequestID;
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ContractRegister", "ContractRegister");
        }

    }
}
