//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Asset Management
//  Date Created : 14-Jan-2016
//  History      :
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : Manoj Kumar 
//  Modified On  : 06-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//Bug No-394 


//*********************************************************************************************************@


using FMDSS.Filters;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.CitizenService.PermissionService;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.Master;
using FMDSS.Models.OnlineBooking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForestProduction
{
    [MyAuthorization]
    public class PurchaseProduceController : BaseController
    {

        ProducePurchase obj_pp = new ProducePurchase();
        int ModuleID = 3;
        #region Member functions

        /// <summary>
        /// Renders UI for Purchase Produce
        /// </summary>
        /// <returns>Bind all Produce Type to table and items in cart</returns>
        public ActionResult PurchaseProduce()
        {


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ProducePurchase pp = new ProducePurchase();

            List<SelectListItem> District = new List<SelectListItem>();
            List<SelectListItem> ProduceType = new List<SelectListItem>();

            try
            {
                ProduceType.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                ViewBag.Produce = ProduceType;
                Int64 cartItem;

                #region CHeck Role


                pp.IsInChargeOrCitizen = 'C';
                if (Convert.ToInt32(Session["CurrentRoleID"]) != 8 && pp.GetNurseryInchargeOrNot(Convert.ToInt64(Session["UserId"].ToString())) > 0)
                {
                    pp.IsInChargeOrCitizen = 'I';
                }
                #endregion

                if (Session["UserId"] != null)
                {

                    pp.UserID = Convert.ToInt64(Session["UserId"].ToString());

                    DataSet dtItem = new DataSet();
                    dtItem = pp.Select_UserTotalCartItem(pp.IsInChargeOrCitizen, 'C');
                    cartItem = Convert.ToInt64(dtItem.Tables[0].Rows[0][0].ToString());
                    ViewBag.ItemCount = cartItem;
                    ViewBag.PurchaseHistory = Convert.ToInt64(dtItem.Tables[1].Rows[0][0].ToString());
                }
                else { ViewBag.ItemCount = 0; }


                DataSet dtd = new DataSet();

                DataTable dtp = new DataTable();
                //ProducePurchase pp = new ProducePurchase();
                pp.ProduceFor = "Nursery";




                dtd = pp.Select_ProduceType(pp.UserID, pp.IsInChargeOrCitizen);

                ProduceType.Clear();
                foreach (DataRow dr in dtd.Tables[0].Rows)
                {
                    ProduceType.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.Produce = ProduceType;

                //if (Convert.ToBoolean(dtd.Tables[1].Rows[0][0]) == true)
                //{
                //    Session["NurseryIncharge"] = true;
                //}
                //else
                //{
                //    Session["NurseryIncharge"] = false;
                //}


                //Check Nursery Incharge comeing as a dept or citizen developed by rajveer  
                //CurrentRoleID = 8 means Citizen Role
                if (Convert.ToBoolean(dtd.Tables[1].Rows[0][0]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                {
                    Session["NurseryIncharge"] = true;
                }
                else
                {
                    Session["NurseryIncharge"] = false;
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        /// <summary>
        /// Get all produce type from database to bind dropdown list
        /// </summary>
        /// <param name="ProduceFor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BindProduceType(string ProduceFor)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ProducePurchase pp = new ProducePurchase();
            List<SelectListItem> ProduceType = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(ProduceFor))
                {
                    //Int64 typeID = Convert.ToInt64(produceTypeID);
                    pp.ProduceFor = ProduceFor;
                    DataSet dt = new DataSet();
                    dt = pp.Select_ProduceType(Convert.ToInt64(Session["UserId"].ToString()));
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        ProduceType.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    ViewBag.Produce = ProduceType;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(ProduceType, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind product dropdownlist
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <returns>json result with product name and ID</returns>
        [HttpPost]
        public JsonResult BindProduct(string produceTypeID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ProducePurchase pp = new ProducePurchase();
            List<SelectListItem> Product = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(produceTypeID))
                {
                    #region CHeck Role
                    pp.IsInChargeOrCitizen = 'C';
                    if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                    {
                        pp.IsInChargeOrCitizen = 'I';
                    }
                    #endregion

                    Int64 typeID = Convert.ToInt64(produceTypeID);
                    DataTable dt = pp.Select_Product(typeID, UserID, pp.IsInChargeOrCitizen);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Product.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["id"].ToString() });
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Product, "Value", "Text"));
        }

        /// <summary>
        /// Used to get products details of nursery / depot on the basis of given param values
        /// </summary>
        /// <param name="ProduceTypeID"></param>
        /// <param name="ProductID"></param>
        /// <param name="ProduceFor"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProducts(string ProduceTypeID, string ProductID, string ProduceFor)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();

            try
            {
                if (!String.IsNullOrEmpty(ProduceTypeID))
                {
                    obj_pp.ProduceTypeID = ProduceTypeID;
                    obj_pp.ProductID = ProductID;
                    if (Session["UserId"] != null)
                    {
                        obj_pp.UserID = Convert.ToInt64(Session["UserId"].ToString());
                    }
                    obj_pp.ProduceFor = ProduceFor;
                    DataTable dt = obj_pp.Select_Product_NurseryWise();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ProducePurchase pp = new ProducePurchase();
                            pp.RowID = Convert.ToInt64(dr["RowID"].ToString());
                            pp.StockID = Convert.ToInt64(dr["STOCKID"].ToString());
                            pp.DistrictName = dr["DIST_NAME"].ToString();
                            pp.VillageName = dr["VILL_NAME"].ToString();
                            pp.NurseryName = dr["NURSERY_NAME"].ToString();
                            pp.ProductTypeName = dr["ProduceType"].ToString();
                            pp.ProductName = dr["ProductName"].ToString();
                            pp.UnitType = dr["UnitName"].ToString();
                            pp.Quantity = Convert.ToInt64(dr["PRODUCE_QTY"].ToString());
                            pp.RatePerUnit = Convert.ToDecimal(dr["RatePerUnit"].ToString());
                            pp.Discount = Convert.ToDecimal(dr["DiscountForCitizen"].ToString());
                            pp.StockQuantity = Convert.ToInt64(dr["IteminCart"].ToString());
                            pp.QuantityByUnit = dr["PRODUCE_QTY"].ToString() + ' ' + dr["UnitName"].ToString();
                            pp.BeforeDiscount = Convert.ToDecimal(dr["PRODUCE_QTY"]);
                            pp.ReservedQty = Convert.ToString(dr["ReservedQty"]);
                            pp.ProductID = Convert.ToString(dr["PRODUCE_ID"]);
                            pp.TotalInventoryQTY = Convert.ToInt64(dr["TotalInventoryQTY"]);

                            pp.ProductThumbImage = Convert.ToString(dr["ProductThumbImage"]);
                            pp.ProductFullImage = Convert.ToString(dr["ProductFullImage"]);
                            result.Add(pp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetViewReservedDetails(string ProductID, string NurseryCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();

            try
            {
                if (!String.IsNullOrEmpty(ProductID))
                {
                    obj_pp.NurseryName = NurseryCode;
                    obj_pp.ProductID = ProductID;
                    if (Session["UserId"] != null)
                    {
                        obj_pp.UserID = Convert.ToInt64(Session["UserId"].ToString());
                    }

                    DataTable dt = obj_pp.FatchReservedPurchaseDetails("LIST");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ProducePurchase pp = new ProducePurchase();
                            pp.RowID = Convert.ToInt64(dr["PurchaseID"].ToString());
                            pp.RequestID = Convert.ToString(dr["RequestID"].ToString());
                            pp.StockID = Convert.ToInt64(dr["STOCKID"].ToString());
                            pp.NurseryName = dr["DEPOT_NURSERY_CODE"].ToString();
                            pp.ProductName = dr["ProductName"].ToString();
                            pp.ProductTypeName = dr["UnitName"].ToString();
                            pp.Quantity = Convert.ToInt64(dr["PurchaseQuantity"].ToString());
                            pp.NurseryDiscountDocument = Convert.ToString(dr["NurseryDiscountDocument"]).Replace("~", string.Empty);
                            pp.DiscountType = Convert.ToString(dr["DiscountType"]);
                            result.Add(pp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return PartialView("PartialReservedDetails", result);
        }

        public JsonResult UPDATEReservedQTY(string PurchaseID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            string result = string.Empty;

            try
            {
                if (!String.IsNullOrEmpty(PurchaseID))
                {
                    obj_pp.ProduceTypeID = PurchaseID;
                    if (Session["UserId"] != null)
                    {
                        obj_pp.UserID = Convert.ToInt64(Session["UserId"].ToString());
                    }

                    DataTable dt = obj_pp.FatchReservedPurchaseDetails("UPDATEReservedQTY");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = "TRUE";
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Add purchase item to database in cart table
        /// </summary>
        /// <param name="StockID"></param>
        /// <param name="Quantity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddToCart(string StockID, string Quantity)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();
            try
            {
                if (Session["UserId"] != null)
                {
                    obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                }
                obj_pp.StockID = Convert.ToInt64(StockID);
                obj_pp.Quantity = Convert.ToInt64(Quantity);

                #region CHeck Role
                obj_pp.IsInChargeOrCitizen = 'C';
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                {
                    obj_pp.IsInChargeOrCitizen = 'I';
                }
                #endregion

                DataSet ds = new DataSet();
                ds = obj_pp.AddToCart(obj_pp.IsInChargeOrCitizen);
                ProducePurchase pp = new ProducePurchase();
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        pp.VillageName = Convert.ToString(dr["TotalItem"].ToString());
                        pp.NurseryName = Convert.ToString(dr["VALIDATIONMSG"].ToString());
                        result.Add(pp);
                    }
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// used to get all saved cart item detail on the basis of user id
        /// </summary>
        /// <param name="ProduceTypeID"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get_UserShopingCartItems(string ProduceTypeID, string ProductID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();
            try
            {
                DataTable dt = new DataTable();
                if (Session["UserId"] != null)
                {
                    #region CHeck Role
                    obj_pp.IsInChargeOrCitizen = 'C';
                    if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                    {
                        obj_pp.IsInChargeOrCitizen = 'I';
                    }
                    #endregion

                    obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                    dt = obj_pp.Select_Cart_Items(obj_pp.IsInChargeOrCitizen);
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    object sumObject;
                    sumObject = dt.Compute("Sum(AmountToBePaid)", "AvailStatus = 'In Stock'");
                    if (sumObject.ToString() == "")
                    { sumObject = 0; }
                    foreach (DataRow dr in dt.Rows)
                    {
                        ProducePurchase pp = new ProducePurchase();
                        pp.RowID = Convert.ToInt64(dr["RowID"].ToString());
                        pp.StockID = Convert.ToInt64(dr["CartID"].ToString());
                        pp.ProductName = dr["ProductName"].ToString();
                        pp.Quantity = Convert.ToInt64(dr["PurchaseQty"].ToString());
                        pp.TotalAmount = Convert.ToDecimal(dr["Amount"].ToString());
                        pp.BeforeDiscount = Convert.ToDecimal(dr["BeforeDiscount"].ToString());
                        pp.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                        pp.AmountToBePaid = Convert.ToDecimal(dr["AmountToBePaid"].ToString());
                        pp.AvailStatus = dr["AvailStatus"].ToString();
                        pp.FinalAmount = Convert.ToDecimal(sumObject);
                        pp.QuantityByUnit = dr["PurchaseQty"].ToString() + ' ' + dr["UnitName"].ToString();
                        result.Add(pp);
                    }
                }
                // object sumObject;
                //  sumObject = dt.Compute("Sum(AmountToBePaid)", "");
                //  Session["FinalAmount"] = sumObject;       
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult UserPurchaseHistory()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<ProducePurchase> result = new List<ProducePurchase>();
            try
            {
                DataTable dt = new DataTable();
                if (Session["UserId"] != null)
                {
                    obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                    dt = obj_pp.Select_OnlinePurchaseHistory();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    string OrderNo = string.Empty;

                    foreach (DataRow dr in dt.Rows)
                    {
                        ProducePurchase pp = new ProducePurchase();
                        if (OrderNo == string.Empty)
                        {
                            OrderNo = dr["OrderNo"].ToString();
                        }
                        else if (OrderNo == Convert.ToString(dr["OrderNo"]))
                        {

                            OrderNo = "";
                        }
                        else
                        {
                            OrderNo = dr["OrderNo"].ToString();
                        }

                        pp.ProduceFor = OrderNo;

                        pp.NurseryName = Convert.ToString(dr["NURSERY_NAME"].ToString());
                        pp.ProductTypeName = Convert.ToString(dr["ProduceType"].ToString());
                        pp.ProductName = dr["ProductName"].ToString();
                        pp.AmountToBePaid = Convert.ToDecimal(dr["PaidAmount"].ToString());
                        pp.VillageName = dr["OrderNo"].ToString() + "AB";
                        pp.DiscountType = Convert.ToString(dr["DiscountType"]);
                        pp.ReservedQty = Convert.ToString(dr["ReservedStatus"]);
                        result.Add(pp);

                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View(result);
        }



        /// <summary>
        /// Delete users cart item by id
        /// </summary>
        /// <param name="CartID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteItemFromCart(string CartID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();
            try
            {
                DataTable dt = new DataTable();
                if (Session["UserId"] != null)
                {
                    obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                    obj_pp.CartID = Convert.ToInt64(CartID);
                    dt = obj_pp.DeleteCartItem();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        object sumObject;
                        sumObject = dt.Compute("Sum(AmountToBePaid)", "AvailStatus = 'In Stock'");

                        if (string.IsNullOrEmpty(Convert.ToString(sumObject)))
                            sumObject = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            //ProducePurchase pp = new ProducePurchase();
                            //pp.RowID = Convert.ToInt64(dr["RowID"].ToString());
                            //pp.StockID = Convert.ToInt64(dr["CartID"].ToString());
                            //pp.ProductName = dr["ProductName"].ToString();
                            //pp.Quantity = Convert.ToInt64(dr["PurchaseQty"].ToString());
                            //pp.TotalAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            //pp.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                            //pp.AmountToBePaid = Convert.ToDecimal(dr["AmountToBePaid"].ToString());
                            //pp.AvailStatus = dr["AvailStatus"].ToString();
                            //pp.FinalAmount = Convert.ToDecimal(sumObject);
                            //pp.QuantityByUnit = dr["PurchaseQty"].ToString() + ' ' + dr["UnitName"].ToString();
                            //pp.BeforeDiscount = Convert.ToDecimal(dr["BeforeDiscount"].ToString());
                            //result.Add(pp);

                            ProducePurchase pp = new ProducePurchase();
                            pp.RowID = Convert.ToInt64(dr["RowID"].ToString());
                            pp.StockID = Convert.ToInt64(dr["CartID"].ToString());
                            pp.ProductName = dr["ProductName"].ToString();
                            pp.Quantity = Convert.ToInt64(dr["PurchaseQty"].ToString());
                            pp.TotalAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            pp.BeforeDiscount = Convert.ToDecimal(dr["BeforeDiscount"].ToString());
                            pp.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                            pp.AmountToBePaid = Convert.ToDecimal(dr["AmountToBePaid"].ToString());
                            pp.AvailStatus = dr["AvailStatus"].ToString();
                            pp.FinalAmount = Convert.ToDecimal(sumObject);
                            pp.QuantityByUnit = dr["PurchaseQty"].ToString() + ' ' + dr["UnitName"].ToString();
                            result.Add(pp);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// function used to pay purchased items amount using emitra.
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="form"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        /// 

        #region payment process
        [HttpPost]

        public ActionResult ApplyNurseryDiscount(string Command, FormCollection form, string Message)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ProducePurchase pp = new ProducePurchase();
            List<ProducePurchase> ListPP = new List<ProducePurchase>();
            List<SelectListItem> Lists = new List<SelectListItem>();
            List<SelectListItem> Lists2 = new List<SelectListItem>();
            try
            {
                #region CHeck Role
                obj_pp.IsInChargeOrCitizen = 'C';
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                {
                    obj_pp.IsInChargeOrCitizen = 'I';
                }
                #endregion
                if (Command == "Submit")
                {
                    DataTable dt = new DataTable();
                    if (Session["UserId"] != null)
                    {
                        pp.UserID = Convert.ToInt64(Session["UserID"]);
                        dt = pp.Select_Cart_Items(obj_pp.IsInChargeOrCitizen);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        object sumObject;
                        sumObject = dt.Compute("Sum(AmountToBePaid)", "AvailStatus = 'In Stock'");
                        Session["FinalAmount"] = sumObject;
                        string IDs = string.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["AvailStatus"].ToString() == "In Stock")
                            {
                                IDs += dt.Rows[i]["CartID"].ToString() + ",";
                            }
                            else
                            {
                                dt.Rows.Remove(dt.Rows[i]);
                                dt.AcceptChanges();
                            }
                        }

                        string cartIDs = IDs.TrimEnd(',');
                        Session["CartIds"] = cartIDs;
                        //ViewData.Model = dt.Select("AvailStatus = 'In Stock'").AsEnumerable();

                        foreach (DataRow dr in dt.Rows)
                        {
                            ListPP.Add(
                                new ProducePurchase()
                                {
                                    RowID = Convert.ToInt64(dr["RowID"]),
                                    CartID = Convert.ToInt64(dr["CartID"]),
                                    ProductName = Convert.ToString(dr["ProductName"]),
                                    Quantity = Convert.ToInt64(dr["PurchaseQty"]),
                                    Amount = Convert.ToDecimal(dr["Amount"].ToString()),
                                    BeforeDiscount = Convert.ToInt64(dr["BeforeDiscount"]),
                                    DiscountTypeID = Convert.ToInt32(dr["Discount"]),
                                    AmountToBePaid = Convert.ToInt64(dr["AmountToBePaid"]),
                                    SSOID = Convert.ToString(Session["SSOID"]),
                                    isdiscountApplicable = Convert.ToInt16(dr["isdiscountApplicable"]),
                                });
                        }


                        DataTable dt1 = pp.GetNurseryDiscount();
                        ViewBag.blanklist = Lists2;
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Lists.Add(new SelectListItem { Text = @dr["Name"].ToString(), Value = @dr["ID"].ToString() });
                        }

                        ViewBag.NurseryDiscountLists = Lists;


                        return View("ApplyNurseryDiscount", ListPP);
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("PurchaseProduce");
        }

        public ActionResult SubmitApplyNurseryDiscount(List<ProducePurchase> InventoryObj, HttpPostedFileBase UploadNurseryDiscountDocument)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ProducePurchase pp = new ProducePurchase();

            string FilePath = "~/NurseryDocument/";
            string Document = "", path = "";

            try
            {
                #region CHeck Role
                obj_pp.IsInChargeOrCitizen = 'C';
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                {
                    obj_pp.IsInChargeOrCitizen = 'I';
                }
                #endregion

                Document = "";
                path = "";

                if (UploadNurseryDiscountDocument != null && UploadNurseryDiscountDocument.ContentLength > 0)
                {
                    Document = Path.GetFileName(UploadNurseryDiscountDocument.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    UploadNurseryDiscountDocument.SaveAs(Server.MapPath(FilePath + FileFullName));
                }


                DataTable DT = listToDT(InventoryObj);
                DataTable dt1 = pp.SubmitNurseryDiscount(DT, path, obj_pp.IsInChargeOrCitizen);
                return RedirectToAction("ProceedToPay", "PurchaseProduce", new { Command = "Submit", Message = "" });

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View("PurchaseProduce");
        }

        public DataTable listToDT(List<ProducePurchase> lstlistInventoryData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");
            try
            {

                #region Vehicle Info
                objDt2.Columns.Add("CartID", typeof(Int64));
                objDt2.Columns.Add("DiscountTypeID", typeof(String));
                objDt2.AcceptChanges();

                foreach (var item in lstlistInventoryData)
                {
                    DataRow dr = objDt2.NewRow();
                    dr["CartID"] = item.CartID;
                    dr["DiscountTypeID"] = item.DiscountTypeID;
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


        public ActionResult ProceedToPay(string Command, FormCollection form, string Message)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                ProducePurchase pp = new ProducePurchase();

                #region CHeck Role
                pp.IsInChargeOrCitizen = 'C';
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                {
                    pp.IsInChargeOrCitizen = 'I';
                }
                #endregion
                if (Command == "Submit")
                {
                    DataTable dt = new DataTable();
                    if (Session["UserId"] != null)
                    {
                        pp.UserID = Convert.ToInt64(Session["UserID"]);
                        dt = pp.Select_Cart_Items(pp.IsInChargeOrCitizen);
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        object sumObject;
                        sumObject = dt.Compute("Sum(AmountToBePaid)", "AvailStatus = 'In Stock'");
                        Session["FinalAmount"] = sumObject;
                        string IDs = string.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["AvailStatus"].ToString() == "In Stock")
                            {
                                IDs += dt.Rows[i]["CartID"].ToString() + ",";
                            }
                        }
                        string cartIDs = IDs.TrimEnd(',');
                        Session["CartIds"] = cartIDs;
                        ViewData.Model = dt.Select("AvailStatus = 'In Stock'").AsEnumerable();


                        return View("PurchasePayment");
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View("PurchaseProduce");


        }



        //[HttpPost]
        //public void Pay()
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //    try
        //    {
        //        Payment pay = new Payment();
        //        Session["RequestId"] = DateTime.Now.Ticks.ToString();
        //        string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();


        //        string encrypt = pay.RequestString("EM33172142@5488", Session["RequestId"].ToString(), Session["FinalAmount"].ToString(), ReturnUrl + "PurchaseProduce/Payment", Session["User"].ToString(), "", "");
        //        Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }
        //}


        [HttpPost]
        public ActionResult DeptKioskUserPayNursery()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true)
                {
                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();

                    _obj.RequestedIdEn = Session["CartIds"].ToString();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 3;
                    _obj.PermissionId = 2;
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = "Order ID Generated After Payment";
                    //===== ADDED BY ARVIND
                    //===== ADDED BY ARVIND K SHARMA

                    _obj.PaidBy = Convert.ToString(Session["UserId"]);
                    _obj.PaidForCitizen = Convert.ToInt64(Session["UserId"]);
                    _obj.PaidAmount = Convert.ToDecimal(Session["FinalAmount"]);
                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                    return PartialView("PaymentByDepartmentalKioskUserForNursery", _obj);

                }
                else
                {
                    return RedirectToAction("PurchaseProduce", "PurchaseProduce");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }



        [HttpPost]
        public void Pay()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {

                // get different heads amount from DB 
                BookOnTicket OBJ = new BookOnTicket();

                DataSet DS = new DataSet();
                Session["RequestNurseryPurchaesId"] = DateTime.Now.Ticks.ToString();

                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("PurchaseNurseryProduce", Convert.ToString(Session["CartIds"]));

                //string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadPurchaseNurseryProduceRevenueIncome"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Revenue"]);
                // +"|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadPurchaseNurseryProduceOthers"]) + "-" + Convert.ToString(DS.Tables[0].Rows[0]["Others"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadFOUNDATIONCode"]) + "-" + Convert.ToString("0");
                string REVENUEHEAD = Convert.ToString(DS.Tables[0].Rows[0]["RevenueHeadPurchaseNurseryProduceRevenueIncome"]);

                string ReturnUrl = ConfigurationManager.AppSettings["emitraReturnUrl"].ToString();
                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();


                string forms = ObjEmitraPayRequest.PayRequest(false, Convert.ToString(Session["RequestNurseryPurchaesId"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                    Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                    ReturnUrl + "PurchaseProduce/Payment", ReturnUrl + "PurchaseProduce/Payment",
                    Convert.ToString(DS.Tables[0].Rows[0]["DIST_CODE"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                    Convert.ToString(DS.Tables[0].Rows[0]["TotalFees"]), REVENUEHEAD, Session["User"].ToString());

                Response.Write(forms);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        public ActionResult Payment()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            int fmdssStatus = 0;
            if (Session["RequestNurseryPurchaesId"] != null)
            {
                try
                {

                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";


                    if (Request.Form["MERCHANTCODE"] != null)
                        MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
                    if (Request.Form["PRN"] != null)
                        PRN = Request.Form["PRN"].ToString();
                    if (Request.Form["STATUS"] != null)
                        STATUS = Request.Form["STATUS"].ToString();
                    if (Request.Form["ENCDATA"] != null)
                        ENCDATA = Request.Form["ENCDATA"].ToString();

                    EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");//Live
                    // EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EmitraNew@2016");//UAT

                    string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);
                    PGResponse ObjPGResponse = JsonConvert.DeserializeObject<PGResponse>(DecryptedData);

                    int status1 = 0;
                    ProducePurchase cs = new ProducePurchase();
                    Payment pay = new Payment();
                    DataTable dt = new DataTable();

                    BookOnTicket cs1 = new BookOnTicket();
                    cs1.UpdateEmitraResponse(ObjPGResponse.PRN, "NurseryModuleOnline", DecryptedData);

                    #region Datarow defination

                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        //dt.Columns.Add("TRANSACTION BANK BID");


                    }
                    #endregion
                    // ObjPGResponse.STATUS = "SUCCESS";
                    #region Response Status
                    if (ObjPGResponse.STATUS != "SUCCESS")
                    {

                        DataRow dtrow = dt.NewRow();


                        cs.RequestID = ObjPGResponse.PRN;
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = "";
                        dtrow["TRANSACTION TIME"] = "";
                        dtrow["TRANSACTION AMOUNT"] = "0";
                        dtrow["EMITRA AMOUNT"] = "0";
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name

                        if (dtrow["TRANSACTION STATUS"].ToString() != "SUCCESS")
                        {
                            cs.Trn_Status_Code = 0;
                        }

                        dt.Rows.Add(dtrow);
                    }
                    else if (ObjPGResponse.STATUS == "SUCCESS")
                    {
                        // ObjPGResponse.STATUS = "SUCCESS";
                        // ObjPGResponse.EMITRATIMESTAMP = "6484818454564564";

                        DataRow dtrow = dt.NewRow();
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.STATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.RequestID = ObjPGResponse.PRN;
                        cs.TransactionID = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.AMOUNT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT);
                        dtrow["USER NAME"] = ObjPGResponse.UDF1; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = ObjPGResponse.PAYMENTMODE;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            cs.Trn_Status_Code = 1;
                            status1 = 1;
                        }
                        dt.Rows.Add(dtrow);
                        if (Session["CartIds"] != null)
                        {
                            if (Session["UserId"] != null)
                            {
                                cs.UserID = Convert.ToInt64(Session["UserID"]);


                                cs.RequestID = Session["RequestNurseryPurchaesId"].ToString();
                                string cartIDs = Session["CartIds"].ToString();

                                #region CHeck Role
                                char IsInChargeOrCitizen = 'C';
                                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true && Convert.ToInt32(Session["CurrentRoleID"]) != 8)
                                {
                                    IsInChargeOrCitizen = 'I';
                                }
                                #endregion
                                int EmitraCharges = Convert.ToInt32(Convert.ToDecimal(ObjPGResponse.PAIDAMOUNT) - Convert.ToDecimal(ObjPGResponse.AMOUNT));
                                cs.UpdateTransactionStatus(cartIDs, IsInChargeOrCitizen, EmitraCharges);
                                SendSMSEmailForSuccessTransaction();
                                ViewBag.REQUESTID = cs.RequestID;
                            }
                        }
                    }

                    #endregion

                    List<CS_BoardingDetails> List = new List<CS_BoardingDetails>();

                    ViewBag.TicketStatus = dt.Rows[0]["TRANSACTION STATUS"].ToString();


                    //if (ViewBag.TicketStatus == "SUCCESS")
                    //{
                    //    DataTable DTdetails = cs.Get_BookedTicketDetails(Session["RequestId"].ToString());

                    //    foreach (DataRow dr in DTdetails.Rows)
                    //    {
                    //        List.Add(
                    //               new CS_BoardingDetails()
                    //               {
                    //                   PrintID = Convert.ToString(dr["PrintID"]),
                    //                   RequestID = Convert.ToString(dr["RequestID"]),
                    //                   PlaceName = Convert.ToString(dr["PlaceName"]),
                    //                   Vehicle = Convert.ToString(dr["Vehicle"]),
                    //                   TotalMembers = Convert.ToString(dr["TotalMembers"]),
                    //                   DateofBooking = Convert.ToString(dr["DateofBooking"]),
                    //                   DateofVisit = Convert.ToString(dr["DateofVisit"]),
                    //                   AmountTobePaid = Convert.ToString(dr["AmountTobePaid"]),
                    //                   BoardingPointName = Convert.ToString(dr["BoardingPointName"]),

                    //               });

                    //    }
                    //}

                    //ViewData["TicketSummary"] = List;

                    ViewData.Model = dt.AsEnumerable();
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
                }
                return View("NurseryPurchaseProduceTransactionStatus");
            }
            return View();
        }

        //public ActionResult Payment()
        //{
        //    if (Session["RequestId"] != null)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
        //        try
        //        {
        //            //TicketBooking cs = new TicketBooking();
        //            int status1 = 0;
        //            ProducePurchase cs = new ProducePurchase();
        //            Payment pay = new Payment();
        //            DataTable dt = new DataTable();
        //            #region Datarow defination
        //            if (dt.Rows.Count == 0)
        //            {
        //                dt.Columns.Add("TRANSACTION STATUS");
        //                dt.Columns.Add("REQUEST ID");
        //                dt.Columns.Add("EMITRA TRANSACTION ID");
        //                dt.Columns.Add("TRANSACTION TIME");
        //                dt.Columns.Add("TRANSACTION AMOUNT");
        //                dt.Columns.Add("USER NAME");
        //                dt.Columns.Add("TRANSACTION BANK DETAILS");
        //            }
        //            #endregion

        //            string response = Request.QueryString["trnParams"].ToString();
        //            string ResponseResult = pay.ProcesTranscationresponce(response);

        //            #region Response decryption

        //            string str1, str2;
        //            str1 = ResponseResult.Replace("<RESPONSE ", "");
        //            str2 = str1.Replace("></RESPONSE>", "");
        //            string[] Responsearr = str2.Split(' ');
        //            string checkFail = "STATUS='FAILED'";
        //            string checkSucess = "STATUS='SUCCESS'";
        //            string rowstatus1 = "";
        //            foreach (var item in Responsearr)
        //            {
        //                if (item.Equals(checkFail))
        //                {
        //                    string[] status2 = item.Split('=');
        //                    rowstatus1 = status2[1].ToString();
        //                }
        //                if (item.Equals(checkSucess))
        //                {
        //                    string[] status2 = item.Split('=');
        //                    rowstatus1 = status2[1].ToString();
        //                }
        //            }
        //            int status1len = Convert.ToInt32(rowstatus1.ToString().Length);
        //            string finalstatus1 = rowstatus1.ToString().Substring(1, status1len - 2);
        //            #endregion
        //            #region Response Status
        //            if (finalstatus1 == "FAILED")
        //            {
        //                string[] emitratransid = Responsearr[0].Split('=');
        //                string[] transtime = Responsearr[1].Split('=');
        //                string[] reqid = Responsearr[2].Split('=');
        //                string[] reqamt = Responsearr[3].Split('=');
        //                string[] username = Responsearr[4].Split('=');
        //                string[] status = Responsearr[7].Split('=');


        //                DataRow dtrow = dt.NewRow();
        //                string rowstatus = status[1].ToString();
        //                int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
        //                string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
        //                string rowreqid = reqid[1].ToString();
        //                int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
        //                string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
        //                string rawemitra = emitratransid[1].ToString();
        //                int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
        //                string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
        //                cs.TransactionID = "0";
        //                cs.RequestID = finalreqid;
        //                string rawtransamount = reqamt[1].ToString();
        //                int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
        //                string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
        //                string rawusername = username[1].ToString();
        //                int usernamelen = Convert.ToInt32(rawusername.Length);
        //                string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);

        //                dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
        //                dtrow["REQUEST ID"] = finalreqid;
        //                dtrow["EMITRA TRANSACTION ID"] = "";
        //                dtrow["TRANSACTION TIME"] = "";//transtime[1];
        //                dtrow["TRANSACTION AMOUNT"] = finalamount;
        //                dtrow["USER NAME"] = finalUserName;

        //                if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
        //                {
        //                    cs.Trn_Status_Code = 0;
        //                }
        //                dt.Rows.Add(dtrow);
        //            }
        //            else if (finalstatus1 == "SUCCESS")
        //            {
        //                string[] emitratransid = Responsearr[0].Split('=');
        //                string[] transtime = Responsearr[1].Split('=');
        //                string[] reqid = Responsearr[3].Split('=');
        //                string[] reqamt = Responsearr[4].Split('=');
        //                string[] username = Responsearr[5].Split('=');
        //                string[] status = Responsearr[8].Split('=');
        //                string[] bank = Responsearr[9].Split('=');
        //                string[] bankbidno = Responsearr[13].Split('=');

        //                DataRow dtrow = dt.NewRow();
        //                string rowstatus = status[1].ToString();
        //                int statuslen = Convert.ToInt32(rowstatus.ToString().Length);
        //                string finalstatus = rowstatus.ToString().Substring(1, statuslen - 2);
        //                string rowreqid = reqid[1].ToString();
        //                int reqidlen = Convert.ToInt32(rowreqid.ToString().Length);
        //                string finalreqid = rowreqid.ToString().Substring(1, reqidlen - 2);
        //                string rawemitra = emitratransid[1].ToString();
        //                int emitralen = Convert.ToInt32(rawemitra.ToString().Length);
        //                string finalemitraid = rawemitra.ToString().Substring(1, emitralen - 2);
        //                cs.TransactionID = finalreqid;
        //                string rawtransamount = reqamt[1].ToString();
        //                int amountlen = Convert.ToInt32(rawtransamount.ToString().Length);
        //                string finalamount = rawtransamount.ToString().Substring(1, amountlen - 2);
        //                string rawusername = username[1].ToString();
        //                int usernamelen = Convert.ToInt32(rawusername.Length);
        //                string finalUserName = rawusername.ToString().Substring(1, usernamelen - 2);
        //                string rawbank = bank[1].ToString();
        //                int banklen = Convert.ToInt32(rawbank.Length);
        //                string finalbank = rawbank.ToString().Substring(1, banklen - 2);
        //                dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(finalstatus);
        //                dtrow["REQUEST ID"] = finalreqid;
        //                dtrow["EMITRA TRANSACTION ID"] = finalemitraid;
        //                cs.TransactionID = finalemitraid;
        //                dtrow["TRANSACTION TIME"] = transtime[1] + Responsearr[2];
        //                dtrow["TRANSACTION AMOUNT"] = finalamount;
        //                dtrow["USER NAME"] = finalUserName;
        //                dtrow["TRANSACTION BANK DETAILS"] = finalbank;
        //                if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
        //                {
        //                    cs.Trn_Status_Code = 1;
        //                    status1 = 1;
        //                }
        //                dt.Rows.Add(dtrow);
        //                if (Session["CartIds"] != null)
        //                {
        //                    if (Session["UserId"] != null)
        //                    {
        //                        cs.UserID = Convert.ToInt64(Session["UserID"]);
        //                    }

        //                    cs.RequestID = Session["RequestId"].ToString();
        //                    string cartIDs = Session["CartIds"].ToString();
        //                    cs.UpdateTransactionStatus(cartIDs);
        //                }
        //            }
        //            #endregion


        //            ViewData.Model = dt.AsEnumerable();
        //            return View("NurseryPurchaseProduceTransactionStatus");
        //        }
        //        catch (Exception ex)
        //        {
        //            new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //        }
        //    }
        //    return View();
        //}
        #endregion



        /// <summary>
        /// Function is used to bind block dropdownlist
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <returns>json result with block name and code</returns>        
        [HttpPost]
        public JsonResult BindBlock(string DistrictID)
        {
            Location loc = new Location();
            List<SelectListItem> Block = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (!String.IsNullOrEmpty(DistrictID))
                {
                    DataTable dt = loc.BindBlockName(DistrictID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Block.Add(new SelectListItem { Text = @dr["BLK_NAME"].ToString(), Value = @dr["BLK_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Block, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind GramPanchayat dropdownlist on the basis of given param
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <param name="BlockID"></param>
        /// <returns>json result with Gram Panchayat name and code</returns>
        [HttpPost]
        public JsonResult BindGramPanchayat(string DistrictID, String BlockID)
        {
            Location loc = new Location();
            List<SelectListItem> GP = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if ((!String.IsNullOrEmpty(DistrictID)) && !String.IsNullOrEmpty(BlockID))
                {
                    DataTable dt = loc.BindGramPanchayatName(DistrictID, BlockID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        GP.Add(new SelectListItem { Text = @dr["GP_NAME"].ToString(), Value = @dr["GP_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(GP, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind village dropdownlist on the basis of given param
        /// </summary>
        /// <param name="DistrictID"></param>
        /// <param name="BlockID"></param>
        /// /// <param name="GpID"></param>
        /// <returns>json result with Village name and code</returns>
        [HttpPost]
        public JsonResult BindVillages(string DistrictID, string BlockID, string GpID)
        {
            Location loc = new Location();
            List<SelectListItem> Village = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if ((!String.IsNullOrEmpty(DistrictID)) && !String.IsNullOrEmpty(BlockID))
                {
                    DataTable dt = loc.BindVillageName(DistrictID, BlockID, GpID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Village.Add(new SelectListItem { Text = @dr["VILL_NAME"].ToString(), Value = @dr["VILL_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Village, "Value", "Text"));
        }

        /// <summary>
        /// Function is used to bind Nursery dropdownlist on the basis of given param
        /// </summary>       
        /// <param name="GpID"></param>
        /// <returns>json result with Nursery name and code</returns>
        [HttpPost]
        public JsonResult BindNursery(string villageID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            ProducePurchase pp = new ProducePurchase();
            List<SelectListItem> Village = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(villageID))
                {
                    DataTable dt = pp.Select_Nursery(villageID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Village.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new SelectList(Village, "Value", "Text"));
        }

        #endregion

        public void SendSMSEmailForSuccessTransaction()
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(Convert.ToString(Session["RequestNurseryPurchaesId"]), "GETUSERDETAILSFORSENDSMSANDEMAILforNurseryPurchaseProduce");

            if (DT.Rows.Count > 0)
            {
                StringBuilder ItemListWithComma = new StringBuilder();
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    #region Old MAIL/SMS Send
                    //body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));

                    //objSMS_EMail_Services.sendEMail("Zoo Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());

                    //body = string.Empty;
                    #endregion

                    #region NEW MAIL/SMS Send
                    body = objSMSandEMAILtemplate.OrderPurchaesEmailSMSTemplate(DT, "email", Server.MapPath(ConfigurationManager.AppSettings["PurchaesOrderEmailTemplate"].ToString()), ref ItemListWithComma);

                    objSMS_EMail_Services.sendEMail("Purchaese Order", body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["OrderPurchasesTicketEmail_CC"].ToString());
                    body = string.Empty;
                    #endregion
                }

                if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                {
                    #region Old MAIL/SMS Send

                    body = objSMSandEMAILtemplate.OrderPurchaesEmailSMSTemplate(DT, "sms", Server.MapPath(ConfigurationManager.AppSettings["PurchaesOrderSMSTemplate"].ToString()), ref ItemListWithComma);
                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);
                    body = string.Empty;
                    #endregion
                }

            }


        }

            #endregion


        [HttpPost]
        public string PrintOrder(string RequestID)
        {
            #region Create PDF
            string sb = string.Empty;
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());

            try
            {
                sb = obj_pp.PrintOrder(RequestID, UserID, Convert.ToBoolean(Session["NurseryIncharge"]));

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            #endregion
            return sb;
        }




        #region Purchaes Dept User Developed by Rajveer
        public ActionResult PurchaseProduceDeptUser()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            ProducePurchase pp = new ProducePurchase();

            List<SelectListItem> District = new List<SelectListItem>();
            List<SelectListItem> ProduceType = new List<SelectListItem>();

            try
            {
                ProduceType.Add(new SelectListItem { Text = "--Select--", Value = "0" });
                ViewBag.Produce = ProduceType;
                Int64 cartItem;
                #region CHeck Role
                char IsInChargeOrCitizen = 'I';
                #endregion
                if (Session["UserId"] != null)
                {

                    pp.UserID = Convert.ToInt64(Session["UserId"].ToString());


                    DataSet dtItem = new DataSet();
                    dtItem = pp.Select_UserTotalCartItem('C', 'D');
                    cartItem = Convert.ToInt64(dtItem.Tables[0].Rows[0][0].ToString());
                    ViewBag.ItemCount = cartItem;
                    ViewBag.PurchaseHistory = Convert.ToInt64(dtItem.Tables[1].Rows[0][0].ToString());
                }
                else { ViewBag.ItemCount = 0; }


                DataSet dtd = new DataSet();

                DataTable dtp = new DataTable();
                //ProducePurchase pp = new ProducePurchase();
                pp.ProduceFor = "NurseryDeptUser";
                dtd = pp.Select_ProduceType(pp.UserID, IsInChargeOrCitizen);

                ProduceType.Clear();
                foreach (DataRow dr in dtd.Tables[0].Rows)
                {
                    ProduceType.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.Produce = ProduceType;

                if (Convert.ToBoolean(dtd.Tables[1].Rows[0][0]) == true)
                {
                    Session["NurseryIncharge"] = true;
                }
                else
                {
                    Session["NurseryIncharge"] = false;
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetProductsDeptUser(string ProduceTypeID, string ProductID, string ProduceFor)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();

            try
            {
                if (!String.IsNullOrEmpty(ProduceTypeID))
                {
                    obj_pp.ProduceTypeID = ProduceTypeID;
                    obj_pp.ProductID = ProductID;
                    if (Session["UserId"] != null)
                    {
                        obj_pp.UserID = Convert.ToInt64(Session["UserId"].ToString());
                    }
                    obj_pp.ProduceFor = ProduceFor;
                    DataTable dt = obj_pp.Select_Product_NurseryWise();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ProducePurchase pp = new ProducePurchase();
                            pp.RowID = Convert.ToInt64(dr["RowID"].ToString());
                            pp.StockID = Convert.ToInt64(dr["STOCKID"].ToString());
                            pp.DistrictName = dr["DIST_NAME"].ToString();
                            pp.VillageName = dr["VILL_NAME"].ToString();
                            pp.NurseryName = dr["NURSERY_NAME"].ToString();
                            pp.ProductTypeName = dr["ProduceType"].ToString();
                            pp.ProductName = dr["ProductName"].ToString();
                            pp.UnitType = dr["UnitName"].ToString();
                            pp.Quantity = Convert.ToInt64(dr["PRODUCE_QTY"].ToString());

                            pp.RatePerUnit = Convert.ToDecimal(dr["RatePerUnit"].ToString());
                            pp.Discount = Convert.ToDecimal(dr["DiscountForCitizen"].ToString());
                            pp.StockQuantity = Convert.ToInt64(dr["IteminCart"].ToString());
                            pp.QuantityByUnit = dr["PRODUCE_QTY"].ToString() + ' ' + dr["UnitName"].ToString();
                            pp.BeforeDiscount = Convert.ToDecimal(dr["PRODUCE_QTY"]);
                            pp.ReservedQty = Convert.ToString(dr["ReservedQty"]);
                            pp.ProductID = Convert.ToString(dr["PRODUCE_ID"]);
                            result.Add(pp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddToCartDeptUser(string StockID, string Quantity)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();
            try
            {
                if (Session["UserId"] != null)
                {
                    obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                }
                obj_pp.StockID = Convert.ToInt64(StockID);
                obj_pp.Quantity = Convert.ToInt64(Quantity);
                DataSet ds = new DataSet();
                ds = obj_pp.AddToCartDeptUser();
                ProducePurchase pp = new ProducePurchase();
                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        pp.VillageName = Convert.ToString(dr["TotalItem"].ToString());
                        pp.NurseryName = Convert.ToString(dr["VALIDATIONMSG"].ToString());
                        result.Add(pp);
                    }
                }



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProceedToPayDeptUser(string Command, FormCollection form, string Message)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                ProducePurchase pp = new ProducePurchase();
                if (Command == "Submit")
                {
                    DataTable dt = new DataTable();
                    if (Session["UserId"] != null)
                    {
                        pp.UserID = Convert.ToInt64(Session["UserID"]);
                        dt = pp.Select_Cart_ItemsDeptUsers();
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        object sumObject;
                        sumObject = dt.Compute("Sum(AmountToBePaid)", "AvailStatus = 'In Stock'");
                        Session["FinalAmount"] = sumObject;
                        string IDs = string.Empty;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["AvailStatus"].ToString() == "In Stock")
                            {
                                IDs += dt.Rows[i]["CartID"].ToString() + ",";
                            }
                        }
                        string cartIDs = IDs.TrimEnd(',');
                        Session["CartIds"] = cartIDs;
                        ViewData.Model = dt.Select("AvailStatus = 'In Stock'").AsEnumerable();


                        return View("PurchasePaymentDeptUser");
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View("PurchaseProduceDeptUser");


        }


        [HttpPost]
        public ActionResult DeptUserPayNursery()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true)
                {
                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();

                    _obj.RequestedIdEn = Session["CartIds"].ToString();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 3;
                    _obj.PermissionId = 2;
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = "Order ID Generated After Payment";
                    //===== ADDED BY ARVIND
                    //===== ADDED BY ARVIND K SHARMA

                    _obj.PaidBy = Convert.ToString(Session["UserId"]);
                    _obj.PaidForCitizen = Convert.ToInt64(Session["UserId"]);
                    _obj.PaidAmount = Convert.ToDecimal(Session["FinalAmount"]);
                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                    return PartialView("PaymentByDepartmentalKioskUserForNursery", _obj);

                }
                else
                {
                    return RedirectToAction("PurchaseProduce", "PurchaseProduce");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public ActionResult DeptUserPayNurseryAmount()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                if (Convert.ToBoolean(Session["NurseryIncharge"]) == true)
                {
                    PaymentByDepartmentalKioskUserDetails _obj = new PaymentByDepartmentalKioskUserDetails();

                    _obj.RequestedIdEn = Session["CartIds"].ToString();
                    _obj.ModuleId = 1;
                    _obj.ServiceTypeId = 3;
                    _obj.PermissionId = 2;
                    _obj.SubPermissionId = 1;
                    _obj.RequestedId = "Order ID Generated After Payment";
                    //===== ADDED BY ARVIND
                    //===== ADDED BY ARVIND K SHARMA

                    _obj.PaidBy = Convert.ToString(Session["UserId"]);
                    _obj.PaidForCitizen = Convert.ToInt64(Session["UserId"]);
                    _obj.PaidAmount = Convert.ToDecimal(Session["FinalAmount"]);
                    _obj.PaidOn = Convert.ToString(DateTime.Now);
                    return PartialView("PaymentByDepartmentalUserForNurseryDeptUser", _obj);

                }
                else
                {
                    return RedirectToAction("PurchaseProduceDeptUser", "PurchaseProduce");
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }


        public JsonResult GetSSOIDDetails(string SSOID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 LOGINSSOID = Convert.ToInt64(Session["UserID"]);

            string ssoid = "0";
            string DesigId = "0";
            string Designation = "0";
            string OfficeName = "0";
            string Mobile = "0";

            try
            {
                if (!string.IsNullOrEmpty(SSOID))
                {
                    Designations obj1 = new Designations();
                    DataTable dtf = obj1.Select_SSODETAILS(SSOID);
                    if (dtf.Rows.Count > 0)
                    {
                        ssoid = Convert.ToString(dtf.Rows[0]["ssoid"]);
                        DesigId = Convert.ToString(dtf.Rows[0]["DesigId"]);
                        Designation = Convert.ToString(dtf.Rows[0]["Desig_Name"]);
                        OfficeName = Convert.ToString(dtf.Rows[0]["OfficeName"]);
                        Mobile = Convert.ToString(dtf.Rows[0]["Mobile"]);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, LOGINSSOID);

            }
            return Json(new { ssoid = ssoid, Designation = Designation, OfficeName = OfficeName, Mobile = Mobile, DesigId = DesigId }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get_UserShopingCartItemsDeptUser(string ProduceTypeID, string ProductID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            var result = new List<ProducePurchase>();
            try
            {
                DataTable dt = new DataTable();
                if (Session["UserId"] != null)
                {
                    obj_pp.UserID = Convert.ToInt64(Session["UserID"]);
                    dt = obj_pp.Select_Cart_ItemsDeptUsers();
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    object sumObject;
                    sumObject = dt.Compute("Sum(AmountToBePaid)", "AvailStatus = 'In Stock'");
                    if (sumObject.ToString() == "")
                    { sumObject = 0; }
                    foreach (DataRow dr in dt.Rows)
                    {
                        ProducePurchase pp = new ProducePurchase();
                        pp.RowID = Convert.ToInt64(dr["RowID"].ToString());
                        pp.StockID = Convert.ToInt64(dr["CartID"].ToString());
                        pp.ProductName = dr["ProductName"].ToString();
                        pp.Quantity = Convert.ToInt64(dr["PurchaseQty"].ToString());
                        pp.TotalAmount = Convert.ToDecimal(dr["Amount"].ToString());
                        pp.BeforeDiscount = Convert.ToDecimal(dr["BeforeDiscount"].ToString());
                        pp.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                        pp.AmountToBePaid = Convert.ToDecimal(dr["AmountToBePaid"].ToString());
                        pp.AvailStatus = dr["AvailStatus"].ToString();
                        pp.FinalAmount = Convert.ToDecimal(sumObject);
                        pp.QuantityByUnit = dr["PurchaseQty"].ToString() + ' ' + dr["UnitName"].ToString();
                        result.Add(pp);
                    }
                }
                // object sumObject;
                //  sumObject = dt.Compute("Sum(AmountToBePaid)", "");
                //  Session["FinalAmount"] = sumObject;       
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion


    }
}