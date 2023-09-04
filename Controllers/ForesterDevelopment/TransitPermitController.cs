//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible to render UI for Transit Permit
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  : Arvind Srivastava  
//  Modified On  : 26-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using FMDSS.Entity;
using FMDSS.Models;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class TransitPermitController : BaseController
    {
        #region Properties & Variables 
        int ModuleID = 3;
        private Int64 UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return Convert.ToInt64(Session["UserID"].ToString());
                else
                    return 0;
            }
        }
        Location _objLocation = new Location();
        Workorder _obj = new Workorder();
        TransitPermit objTP = new TransitPermit();
        List<TransitPermit> lstTP = new List<TransitPermit>();
        List<SelectListItem> District = new List<SelectListItem>();
        List<SelectListItem> BlockName = new List<SelectListItem>();
        List<SelectListItem> GPName = new List<SelectListItem>();
        List<SelectListItem> VillageName = new List<SelectListItem>();
        List<SelectListItem> ddlProduceType = new List<SelectListItem>();
        List<SelectListItem> ddlProduct = new List<SelectListItem>();
        List<DepotManagement> depotList = new List<DepotManagement>();
        List<SelectListItem> CircleCode = new List<SelectListItem>();
        List<SelectListItem> divisionCode = new List<SelectListItem>();
        List<SelectListItem> rangeCode = new List<SelectListItem>();
        #endregion

        #region Action Methods
        public ActionResult AddNewRowForTPProduct(int currentRowIndex, long objectID)
        {
            OpeningBalance model = new OpeningBalance();
            DODProductDetails pd = new DODProductDetails();
            List<DODProductDetails> lst = new List<DODProductDetails>();
            lst.Add(pd);
            model.DODProductList = lst;
            ViewBag.CurrentIndex = currentRowIndex;
            ViewBag.RowType = FMDSS.RowType.DOD_ProductDetails;
            var produceType = _obj.SelectProduceType().AsEnumerable().Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.Field<long>("ID")),
                Text = x.Field<string>("ProduceType")
            });
            ViewBag.ProduceType_List = produceType;

            return PartialView("_AddNewRow", model);
        }

        public ActionResult Create()
        {
            ViewBag.ExchangeMode = new SelectList(Common.GetProductExchangeType(), "Value", "Text");
            return View();
        }
         
        [HttpPost]
        public ActionResult Create(TransitPermit tp)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            { 
                ResponseMsg msg = tp.SubmitTransitPermit(tp);
                TempData["ReturnMsg"] = msg.ReturnMsg;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session["UserId"] != null)
            {
                objTP.ID = Convert.ToInt64(id);
                objTP.UpdatedBy = Convert.ToInt64(Session["UserID"]);
                Int64 i = objTP.DeleteTransitPermit();
            } 
            ViewBag.Status = "Transit Permit Deleted Successfully";
            return RedirectToAction("index", "TransitPermit"); 
        } 

        public ActionResult Index()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (TempData["ReturnMsg"] != null)
                {
                    ViewBag.IsError = TempData["IsError"];
                    ViewBag.ReturnMsg = TempData["ReturnMsg"];
                    TempData["ReturnMsg"] = null; TempData["IsError"] = null;
                }

                objTP.UserID = Convert.ToInt64(Session["UserId"]);
                DataTable dtf = objTP.Select_TransitPermit();
                 
                foreach (DataRow dr in dtf.Rows)
                {
                    lstTP.Add(new TransitPermit()
                    {
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        ToLocationType = dr["ToLocationType"].ToString(),
                        DIST_NAME = dr["DIST_NAME"].ToString(),
                        FromVillage = dr["FromVillage"].ToString(),
                        ToVillage = dr["Depot_Name"].ToString(),
                        ToDepot_NurseryCode = dr["NURSERY_NAME"].ToString(),
                        TransitPermitName = dr["TransitPermitName"].ToString(),
                        EnteredOn = dr["EnteredOn"].ToString(),
                        ModeofTransport = dr["ModeofTransport"].ToString(),
                        Permit_ValidUpto = dr["Permit_ValidUpto"].ToString(),
                        VehicleNumber = dr["VehicleNumber"].ToString(),
                        Driver_Name = dr["Driver_Name"].ToString(),
                        Driver_MobNo = dr["Driver_MobNo"].ToString(),

                    });
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return View("index", lstTP);

        }
        
        public ActionResult OpeningBalance()
        {
            OpeningBalance model = new OpeningBalance();
            var produceType = _obj.SelectProduceType().AsEnumerable().Select(x => new SelectListItem
            {
                Value = Convert.ToString(x.Field<long>("ID")),
                Text = x.Field<string>("ProduceType")
            });  
            ViewBag.ProduceType_List = produceType;
            ViewBag.FromRangeCode = new TransitPermit().SetDropdownData(6, string.Empty); 
            return View(model);
        }

        [HttpPost]
        public ActionResult OpeningBalance(OpeningBalance model)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                ResponseMsg msg = objTP.OpeningBalance_Save(model);
                TempData["ReturnMsg"] = msg.ReturnMsg;
                TempData["IsError"] = msg.IsError;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return RedirectToAction("Index");
        }

        public ActionResult LoadTransitPermitRequest(int transportModeID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TransitPermit tp = new TransitPermit();

            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                //DataTable dtprodType = _obj.SelectProduceType();
                //foreach (System.Data.DataRow dr in dtprodType.Rows)
                //{
                //    ddlProduceType.Add(new SelectListItem { Text = @dr["ProduceType"].ToString(), Value = @dr["ID"].ToString() });
                //}

                DataTable dtRange = new Common().Select_Range(Convert.ToInt64(Session["UserID"].ToString()));
                foreach (System.Data.DataRow dr in dtRange.Rows)
                {
                    items.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                }

                var produceType = _obj.SelectProduceType().AsEnumerable().Select(x => new SelectListItem
                {
                    Value = Convert.ToString(x.Field<long>("ID")),
                    Text = x.Field<string>("ProduceType")
                });
                ViewBag.ProduceType_List = produceType; 
                ViewBag.ToRangeCode = items;
                ViewBag.FromRangeCode = new TransitPermit().SetDropdownData(6, string.Empty);
                ViewBag.ToDivisionCode = new TransitPermit().SetDropdownData(1, string.Empty);
                ViewBag.TransportMode = new TransitPermit().SetDropdownData(2, string.Empty);
                //ViewBag.ddlProduceType1 = ddlProduceType;
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            if (transportModeID == 1)
            {
                return PartialView("_SiteToDepot", tp);
            }
            else
            {
                return PartialView("_DepotToDepot", tp);
            }
        }

        #endregion

        #region Json Methods 
        [HttpPost]
        public JsonResult BindWorkOrder(string villCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if (!String.IsNullOrEmpty(villCode))
                {
                    TransitPermit tpObj = new TransitPermit();
                    DataTable dt = tpObj.Select_WorkOrder(villCode);
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["WorkOrder_Code"].ToString(), Value = @dr["WorkorderID"].ToString() });
                    }
                }
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }


            return Json(new SelectList(items, "Value", "Text"));
        }
        [HttpPost]
        public JsonResult GetDepotIncharge(string depotID)
        {
            TransitPermit tp = new TransitPermit();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            string DepotIncharge = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(depotID))
                {

                    DataTable dt = tp.Select_DepotInchargeByDepotID(Convert.ToInt64(depotID));
                    if (dt != null && dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            DepotIncharge = dr["Depot_InCharge"].ToString() + "(" + dr["Desig_Alias"].ToString() + ")";
                        }
                    }
                    else
                    {
                        DepotIncharge = "Depot Incharge is Not Available";
                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(DepotIncharge, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetDepotDetails(string depotID)
        {
            TransitPermit tp = new TransitPermit();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            EnumerableRowCollection objList = null;
            EnumerableRowCollection productTypeList = null;

            string DepotIncharge = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(depotID))
                {

                    DataSet ds = tp.GetDepotDetails(Convert.ToInt64(depotID));

                    if (FMDSS.Globals.Util.isValidDataSet(ds, 0))
                    {
                        objList = ds.Tables[0].AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = Convert.ToString(x.Field<long>("InventoryID")),
                            Text = x.Field<string>("DisplayLotNumber")
                        });
                    }

                    if (FMDSS.Globals.Util.isValidDataSet(ds, 2))
                    {
                        productTypeList = ds.Tables[2].AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = Convert.ToString(x.Field<long>("ProductTypeID")),
                            Text = x.Field<string>("ProductTypeName")
                        });
                    }

                    if (FMDSS.Globals.Util.isValidDataSet(ds, 1,true))
                    {
                        DepotIncharge = ds.Tables[1].Rows[0]["Depot_InCharge"].ToString();
                    }
                    else
                    {
                        DepotIncharge = "Depot Incharge is Not Available";
                    } 
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new { ObjList = objList, ProductTypeList = productTypeList, DepotIncharge = DepotIncharge }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult getDepotData(string rangeCode)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();

            try
            {

                if (!String.IsNullOrEmpty(rangeCode))
                {
                    DataTable dt = _objLocation.BindDepottp(rangeCode);
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Depot_Name"].ToString(), Value = @dr["Depot_Id"].ToString() });
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            } 
            return Json(new SelectList(items, "Value", "Text")); 
        }
        public JsonResult GetDropdownData(string key, string parentID, string childID="")
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();


            try
            {
                EnumerableRowCollection<SelectListItem> data = null;
                switch (key)
                {
                    case "DivTo":
                        data = new TransitPermit().SetDropdownData(3, parentID);
                        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                    case "FromSite":
                        data = new TransitPermit().SetDropdownData(5, parentID);
                        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                    case "NoticeDepot":
                        data = new TransitPermit().SetDropdownData(7, parentID);
                        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                    case "InventoryLot":
                        data = new TransitPermit().SetDropdownData(10, parentID, childID);
                        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                    case "NoticeProduct":
                        data = new TransitPermit().SetDropdownData(11, parentID, childID); 
                        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                    case "NoticeLot":
                        data = new TransitPermit().SetDropdownData(12, parentID, childID);
                        return Json(new { data = data }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return null;
        }

        [HttpPost]
        public JsonResult getForesProduce(string depotId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if (!String.IsNullOrEmpty(depotId))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();
                    DataTable dt = noticeManagement.BindProducetp(Convert.ToInt64(depotId));
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ProduceType"].ToString() + ":" + @dr["Depot_InCharge"].ToString() + "(" + @dr["Desig_Alias"].ToString() + ")", Value = @dr["ID"].ToString() });
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }
         
        [HttpPost]
        public JsonResult getForesProduct(string depotId, string producetype)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {
                if (!String.IsNullOrEmpty(depotId) && !String.IsNullOrEmpty(producetype))
                {
                    NoticeManagement noticeManagement = new NoticeManagement();
                    DataTable dt = noticeManagement.BindForestProduct(Convert.ToInt64(depotId), Convert.ToInt64(producetype));
                    ViewBag.fname = dt;


                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["ProductName"].ToString(), Value = @dr["PRODUCE_ID"].ToString() });
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        [HttpPost]
        public JsonResult getForesProduceqty(string depotId, string villageCode, string producetype, string product, string exchangeMode, string workOrderID)
        {

            NoticeManagement noticeManagement = null;
            NoticeManagement notice = new NoticeManagement();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            try
            {

                if (exchangeMode == "3")
                {
                    if (!String.IsNullOrEmpty(depotId) && !String.IsNullOrEmpty(producetype) && !String.IsNullOrEmpty(product))
                    {

                        noticeManagement = new NoticeManagement();
                        DataTable dt = notice.BindProducestock(Convert.ToInt64(depotId), Convert.ToInt64(producetype), Convert.ToInt64(product));
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {

                                noticeManagement.ProduceUnit = dr["UnitName"].ToString();
                                noticeManagement.Qty = dr["PRODUCE_QTY"].ToString();
                                noticeManagement.ProductRate = Convert.ToDecimal(dr["RatePerUnit"].ToString());
                            }
                        }
                        else
                        {
                            noticeManagement.ProduceUnit = "";
                            noticeManagement.Qty = "";
                            noticeManagement.ProductRate = 0;
                        }


                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(villageCode) && !String.IsNullOrEmpty(producetype) && !String.IsNullOrEmpty(product))
                    {

                        noticeManagement = new NoticeManagement();
                        DataTable dt = _obj.GetAvailableProductbyVillage(villageCode, producetype, product, workOrderID);
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {

                                noticeManagement.ProduceUnit = dr["UnitName"].ToString();
                                noticeManagement.Qty = dr["TotalQuantity"].ToString();
                                //noticeManagement.ProductRate = Convert.ToDecimal(dr["RatePerUnit"].ToString());
                            }
                        }
                        else
                        {
                            noticeManagement.ProduceUnit = "";
                            noticeManagement.Qty = "";
                            noticeManagement.ProductRate = 0;
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(noticeManagement, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFromDepotData(string rangeCode)
        {
            return Json(new { data = new TransitPermit().SetDropdownData(4, rangeCode) }, JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult GetDTDProductDetails(string parentID)
        {
            TransitPermit model = new TransitPermit();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();  

            try
            {

                if (!string.IsNullOrEmpty(parentID))
                {
                    DataTable dt = model.GetDetailsByInventory(parentID);
                    model.DODProductList = Globals.Util.GetListFromTable<DODProductDetails>(dt);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return PartialView("_DTDProductDetails", model);
        }


        public JsonResult GetReceivedQtyByTP(Int64 tpID)
        {
            TransitPermit tp = new TransitPermit();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<SelectListItem> items = new List<SelectListItem>();
            int availableQty = 0; long productID = 0; long productTypeID = 0;
            string productName = string.Empty; string productTypeName = string.Empty;
            string permit_ValidUpto = string.Empty;
            EnumerableRowCollection lotList = null;

            try
            {

                if (tpID > 0)
                {
                    DataSet ds = tp.GetReceivedQtyByTP(tpID);
                    availableQty = Convert.ToInt32(ds.Tables[0].Rows[0]["AvailableQty"]);
                    permit_ValidUpto = Convert.ToString(ds.Tables[0].Rows[0]["Permit_ValidUpto"]);
                    productID = Convert.ToInt64(ds.Tables[0].Rows[0]["ProductID"]);
                    productName = Convert.ToString(ds.Tables[0].Rows[0]["ProductName"]);
                    productTypeID = Convert.ToInt64(ds.Tables[0].Rows[0]["ProductTypeID"]);
                    productTypeName = Convert.ToString(ds.Tables[0].Rows[0]["ProduceType"]);

                    lotList = ds.Tables[1].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<long>("StockID")),
                        Text = x.Field<string>("DisplayLotNumber")
                    });
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
            return Json(new { availableQty = availableQty, permit_ValidUpto = permit_ValidUpto, productID = productID, productName = productName, productTypeID = productTypeID, productTypeName = productTypeName, lotList = lotList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult productListByWorkorderID(string workOrderID)
        {
            var result = new List<TransitPermit>();
            List<TransitPermit> lstWOMilestone = new List<TransitPermit>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TransitPermit ts = new TransitPermit();
            try
            {
                DataTable dt = ts.Select_ProductDetail(Convert.ToInt64(workOrderID));
                TransitPermit tsobj = null;
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        tsobj = new TransitPermit()
                        {
                            ProduceType = Convert.ToInt64(dr["ProduceTypeID"].ToString()),
                            ProductType = dr["ProduceType"].ToString(),
                            Product = dr["ProductName"].ToString(),
                            Qty = dr["TotalQuantity"].ToString(),
                            ProduceUnit = dr["UnitType"].ToString(),
                            //ProductRate = convertdr["TotalQuantity"].ToString(),

                        };
                        result.Add(tsobj);

                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult ViewDetails(string transitID)
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    TransitPermit objT = new TransitPermit();

        //    try
        //    {
        //        if (!String.IsNullOrEmpty(transitID))
        //        {
        //            DataTable dt = objTP.BindTransitData(Convert.ToInt64(transitID), "VIEW");
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                DataRow dr = dt.Rows[0]; 
        //                objT = new TransitPermit();
        //                objT.TransitPermitName = dr["TransitPermitName"].ToString();
        //                objT.ExchangeMode = dr["ToLocationType"].ToString();
        //                objT.FromVillage_Code = dr["FromVillage"].ToString();
        //                objT.ToVillage_Code = dr["ToVillage"].ToString();
        //                objT.ProductType = dr["ProduceType"].ToString();
        //                objT.Product = dr["ProductName"].ToString();
        //                objT.TransferQTY = Convert.ToDecimal(dr["TransferQTY"].ToString());
        //                objT.ReceivedQty = Convert.ToDecimal(dr["ReceivedQty"].ToString());
        //                objT.RemainingQty = Convert.ToDecimal(dr["RemainingQty"].ToString());
        //                objT.ModeofTransport = dr["ModeofTransport"].ToString();
        //                objT.VehicleNumber = dr["VehicleNumber"].ToString();
        //                objT.Driver_License_No = dr["Driver_License_No"].ToString();
        //                objT.Driver_Name = dr["Driver_Name"].ToString();
        //                objT.Driver_MobNo = dr["Driver_MobNo"].ToString();
        //                objT.Permit_ValidUpto = dr["Permit_ValidUpto"].ToString();
        //                //objT.InventoryLotNumber = dr["InventoryLotNumber"].ToString();
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
        //    }

        //    return Json(objT, JsonRequestBehavior.AllowGet);

        //} 
        #endregion

    }
}
