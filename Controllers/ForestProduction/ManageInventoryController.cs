using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using FMDSS.Models.Admin;
using FMDSS.Models.ForestProduction;
using FMDSS.Models;
using System.IO;
using FMDSS.Globals;
using FMDSS.Entity;
using System.Configuration;
using System.Data.SqlClient;
using FMDSS.Repository.Interface;
using FMDSS.Repository;

namespace FMDSS.Controllers.ForestProduction
{

    public class ManageInventoryController : BaseController
    {
        Location _obj = new Location();
        List<DODInventory> objInventory = new List<DODInventory>();
        List<DODItemAddedToInventory> objItemAddedToInventory = new List<DODItemAddedToInventory>();
        List<SelectListItem> Range = new List<SelectListItem>();
        List<SelectListItem> YearRange = new List<SelectListItem>();
        List<SelectListItem> TreeYearList = new List<SelectListItem>();
        private IProtectionRepository _protectionRepository;
        #region [Cunstructor]
        public ManageInventoryController()
        {
            _protectionRepository = new ProtectionRepository();
        }
        #endregion

        public ActionResult ManageInventory()
        {
            try
            {
                DODInventory objInvntry = new DODInventory();
                DataSet dsProduces = new InventoryManagement().FetchDODInventoryDetails();

                objInventory = Util.GetListFromTable<DODInventory>(dsProduces, 0);
                ViewData["lstInventory"] = objInventory;

                objItemAddedToInventory = Util.GetListFromTable<DODItemAddedToInventory>(dsProduces, 1);
                ViewData["lstInventoryProduces"] = objItemAddedToInventory;

                //var objLotMaster = new List<LotMaster>();
                //objLotMaster = Util.GetListFromTable<LotMaster>(dsProduces, 2); 
                //ViewData["lstAssignedLot"] = objLotMaster;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateInventory()
        {
            try
            {
                DODInventory objInvntry = new DODInventory();
                DataSet dsProduces = new InventoryManagement().FetchDODInventoryDetails(3);

                objInventory = Util.GetListFromTable<DODInventory>(dsProduces, 0);
                ViewData["lstInventory"] = objInventory;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageNurseryInventory()
        {

            InventoryManagement objInvntry = new InventoryManagement();
            List<listInventoryData> InvList = new List<listInventoryData>();
            try
            {

                DataSet dsFinacialYear = _protectionRepository.GetFinancialYear();
                foreach (System.Data.DataRow dr in dsFinacialYear.Tables[0].Rows)
                {
                    YearRange.Add(new SelectListItem { Text = @dr["Years"].ToString(), Value = @dr["YearId"].ToString() });
                }
                objInvntry.FinacialYearList = YearRange.Take(2).ToList();


                //ViewBag.FinacialYear=new SelectList(_protectionRepository.GetFinancialYear(), "Value", "Text", "Partner Codes");
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryList();

                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                }

                objInvntry.NurseryList = Range;

                //code added by pooran on 04-07-2019
                foreach (DataRow dr in dsProduces.Tables[1].Rows)
                {
                    InvList.Add(
                        new listInventoryData()
                        {
                            stockID = Convert.ToInt64(dr["STOCKID"]),
                            DEPOT_NURSERY_CODE = Convert.ToString(dr["DEPOT_NURSERY_CODE"]),
                            PRODUCETYPEID = dr["PRODUCETYPEID"].ToString(),
                            PRODUCETYPE = dr["PRODUCETYPE"].ToString(),
                            PRODUCTID = Convert.ToInt64(dr["PRODUCTID"]),
                            PRODUCTNAME = dr["PRODUCTNAME"].ToString(),
                            UNITNAME = dr["UNITNAME"].ToString(),
                            PRICE = Convert.ToString(dr["PRICE"]),
                            PRODUCE_QTY_Citizen = Convert.ToInt64(dr["PRODUCE_QTY_Citizen"]),
                            PRODUCE_QTY_Citizen1 = Convert.ToInt64(dr["PRODUCE_QTY_Citizen1"]),
                            PRODUCE_QTY_Citizen2 = Convert.ToInt64(dr["PRODUCE_QTY_Citizen2"]),

                            PRODUCE_QTY_CitizenPritory = Convert.ToInt32(dr["EmitraHeadPritory"]),
                            PRODUCE_QTY_CitizenPritory1 = Convert.ToInt32(dr["EmitraHeadPritory1"]),
                            PRODUCE_QTY_CitizenPritory2 = Convert.ToInt32(dr["EmitraHeadPritory2"]),

                            PRODUCE_HeadPrimaryStatus = Convert.ToInt32(dr["PRODUCE_HeadPrimaryStatus"]),

                            PRODUCE_QTY_Department = Convert.ToInt64(dr["PRODUCE_QTY_Department"]),
                            DiscountForCitizen = Convert.ToString(dr["DiscountForCitizen"]),
                            DiscountForGovt = Convert.ToString(dr["DiscountForGovt"]),
                            DiscountForNGO = Convert.ToString(dr["DiscountForNGO"]),
                            IsDiscountApplicable = Convert.ToBoolean(dr["IsDiscountApplicable"]),
                            IsActive = Convert.ToBoolean(dr["IsActive"]),
                            Citizen_StockOut = Convert.ToInt64(dr["Citizen_StockOut"]),
                            Dept_StockOut = Convert.ToInt64(dr["Dept_StockOut"])
                        });
                }
                foreach (System.Data.DataRow dr in dsProduces.Tables[3].Rows)
                {
                    objInvntry.AddNurseryPlantBtn = Convert.ToInt16(@dr["AddNurseryPlantBtn"].ToString());
                }

                objInvntry.ListInventory = InvList;
                List<SelectListItem> StockList = new List<SelectListItem>();
                //foreach (System.Data.DataRow dr in dsProduces.Tables[2].Rows)
                //{
                //    StockList.Add(new SelectListItem { Text = @dr["StockName"].ToString(), Value = @dr["StockName"].ToString() });
                //}
                ViewBag.StockList = StockList;

                //DataTable dt = new Common().Select_Range(Convert.ToInt64(Session["UserID"].ToString()));
                //ViewBag.fname = dt;
                //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //{
                //    Range.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                //}
                //ViewBag.RangeCode = Range;


                //DataSet dsProduces = objInvntry.FetchNurseryProduces();

                //foreach (DataRow dr in dsProduces.Tables[0].Rows)
                //{
                //    objInventory.Add(
                //        new InventoryManagement()
                //        {
                //            rowID = Convert.ToInt64(dr["ROWID"]),
                //            stockID = Convert.ToInt64(dr["STOCKID"]),
                //            VillageCode = dr["VILL_NAME"].ToString(),
                //            nurseryDepotCode = dr["NURSERY_NAME"].ToString(),
                //            produceType = dr["PRODUCETYPE"].ToString(),
                //            produce = dr["PRODUCTNAME"].ToString(),
                //            produceUnit = dr["UNITNAME"].ToString(),
                //            produceQty = Convert.ToDouble(dr["PRODUCE_QTY"]),
                //            d_produceQty = Convert.ToDouble(dr["PRODUCE_QTY_Department"]),
                //            isItemActive = Convert.ToBoolean(dr["ISACTIVE"])
                //        });
                //}
                //ViewData["lstInventory"] = objInventory;
                // objInvntry.NurseryList






                //DataTable dt = new Common().Select_Range(Convert.ToInt64(Session["UserID"].ToString()));
                //ViewBag.fname = dt;
                //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //{
                //    Range.Add(new SelectListItem { Text = @dr["RANGE_NAME"].ToString(), Value = @dr["RANGE_CODE"].ToString() });
                //}
                //ViewBag.RangeCode = Range;

                //InventoryManagement objInvntry = new InventoryManagement();
                //DataSet dsProduces = objInvntry.FetchNurseryProduces();

                //foreach (DataRow dr in dsProduces.Tables[0].Rows)
                //{
                //    objInventory.Add(
                //        new InventoryManagement()
                //        {
                //            rowID = Convert.ToInt64(dr["ROWID"]),
                //            stockID = Convert.ToInt64(dr["STOCKID"]),
                //            VillageCode = dr["VILL_NAME"].ToString(),
                //            nurseryDepotCode = dr["NURSERY_NAME"].ToString(),
                //            produceType = dr["PRODUCETYPE"].ToString(),
                //            produce = dr["PRODUCTNAME"].ToString(),
                //            produceUnit = dr["UNITNAME"].ToString(),
                //            produceQty = Convert.ToDouble(dr["PRODUCE_QTY"]),
                //            d_produceQty = Convert.ToDouble(dr["PRODUCE_QTY_Department"]),
                //            isItemActive = Convert.ToBoolean(dr["ISACTIVE"])
                //        });
                //}
                //ViewData["lstInventory"] = objInventory;

                return View(objInvntry);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "ManageNurseryInventory" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ManageNurseryInventory(InventoryManagement InventoryObj, string Command)
        {
            InventoryManagement objInvntry = new InventoryManagement();
            List<listInventoryData> InvList = new List<listInventoryData>();
            DataSet dsProduces = new DataSet();
            try
            {
                objInvntry.nurseryDepotCode = InventoryObj.nurseryDepotCode;
                //objInvntry.StockName = InventoryObj.StockName;
                objInvntry.FinacialYear = InventoryObj.FinacialYear;
                objInvntry.Date = InventoryObj.Date;


                if (Command.Equals("GetList"))
                {

                    dsProduces = objInvntry.FetchLoadNurseryList();
                    foreach (System.Data.DataRow dr in dsProduces.Tables[3].Rows)
                    {
                        objInvntry.AddNurseryPlantBtn = Convert.ToInt16(@dr["AddNurseryPlantBtn"].ToString());
                    }
                    //foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                    //{
                    //    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                    //}

                    //objInvntry.NurseryList =  Range;

                    objInvntry.NurseryList = new SelectList(dsProduces.Tables[0].AsDataView(), "NURSERY_CODE", "NURSERY_NAME").ToList();
                    List<SelectListItem> StockList = new List<SelectListItem>();
                    foreach (System.Data.DataRow dr in dsProduces.Tables[2].Rows)
                    {
                        StockList.Add(new SelectListItem { Text = @dr["StockName"].ToString(), Value = @dr["StockName"].ToString() });
                    }
                    ViewBag.StockList = StockList;

                    DataSet dsFinacialYear = _protectionRepository.GetFinancialYear();
                    foreach (System.Data.DataRow dr in dsFinacialYear.Tables[0].Rows)
                    {
                        YearRange.Add(new SelectListItem { Text = @dr["Years"].ToString(), Value = @dr["YearId"].ToString() });
                    }
                    //objInvntry.FinacialYearList = YearRange.Take(6).ToList();
                    objInvntry.FinacialYearList = YearRange.Take(2).ToList();

                    foreach (DataRow dr in dsProduces.Tables[1].Rows)
                    {                        
                        InvList.Add(
                            new listInventoryData()
                            {
                                stockID = Convert.ToInt64(dr["STOCKID"]),
                                DEPOT_NURSERY_CODE = Convert.ToString(dr["DEPOT_NURSERY_CODE"]),
                                ProductFullImage = Convert.ToString(dr["ProductFullImage"]),
                                ProductThumbImage = Convert.ToString(dr["ProductThumbImage"]),
                                PRODUCETYPEID = dr["PRODUCETYPEID"].ToString(),
                                PRODUCETYPE = dr["PRODUCETYPE"].ToString(),
                                PRODUCTID = Convert.ToInt64(dr["PRODUCTID"]),
                                BASEPRODUCETYPE = dr["BASEPRODUCETYPE"].ToString(),
                                BASEPRODUCETYPE_ID = Convert.ToInt32((DBNull.Value.Equals(dr["BASEPRODUCETYPEID"]) == false ? dr["BASEPRODUCETYPEID"] : 0)),
                                PRODUCTNAME = dr["PRODUCTNAME"].ToString(),
                                PRODUCTCATEGORY = dr["ProductCategory"].ToString(),
                                UNITNAME = dr["UNITNAME"].ToString(),
                                PRICE = Convert.ToString(dr["PRICE"]),
                                PRODUCE_QTY_Citizen = Convert.ToInt64(dr["PRODUCE_QTY_Citizen"]),
                                PRODUCE_QTY_Citizen1 = Convert.ToInt64(dr["PRODUCE_QTY_Citizen1"]),
                                PRODUCE_QTY_Citizen2 = Convert.ToInt64(dr["PRODUCE_QTY_Citizen2"]),

                                PRODUCE_QTY_CitizenPritory = Convert.ToInt32(dr["EmitraHeadPritory"]),
                                PRODUCE_QTY_CitizenPritory1 = Convert.ToInt32(dr["EmitraHeadPritory1"]),
                                PRODUCE_QTY_CitizenPritory2 = Convert.ToInt32(dr["EmitraHeadPritory2"]),

                                PRODUCE_HeadPrimaryStatus = Convert.ToInt32(dr["PRODUCE_HeadPrimaryStatus"]),

                                PRODUCE_QTY_Department = Convert.ToInt64(dr["PRODUCE_QTY_Department"]),
                                DiscountForCitizen = Convert.ToString(dr["DiscountForCitizen"]),
                                DiscountForGovt = Convert.ToString(dr["DiscountForGovt"]),
                                DiscountForNGO = Convert.ToString(dr["DiscountForNGO"]),
                                IsDiscountApplicable = Convert.ToBoolean(dr["IsDiscountApplicable"]),
                                IsActive = Convert.ToBoolean(dr["IsActive"]),
                                Citizen_StockOut= Convert.ToInt64(dr["Citizen_StockOut"]),
                                Dept_StockOut= Convert.ToInt64(dr["Dept_StockOut"])
                                //PlantAgeStr = "" + dr["PlantAge"]
                            });
                    }
                    objInvntry.ListInventory = InvList;
                    TempData["NurInvList"] = InvList;
                    return View(objInvntry);


                }
                else if (Command.Equals("SubmitList"))
                {
                    DataTable DTs = listInventoryDataDetails(InventoryObj.ListInventory);
                    Int64 result = objInvntry.INSERT_UPDATE_LIST(DTs, objInvntry.StockName);

                    if (result > 0)
                    {
                        TempData["StockStatus"] = "Item(s) has been updated into the Inventory successfully!";

                        dsProduces = objInvntry.FetchLoadNurseryList();

                        //foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                        //{
                        //    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                        //}

                        objInvntry.NurseryList = new SelectList(dsProduces.Tables[0].AsDataView(), "NURSERY_CODE", "NURSERY_NAME").ToList();


                        foreach (DataRow dr in dsProduces.Tables[1].Rows)
                        {
                            InvList.Add(
                                new listInventoryData()
                                {
                                    stockID = Convert.ToInt64(dr["stockID"]),
                                    DEPOT_NURSERY_CODE = Convert.ToString(dr["DEPOT_NURSERY_CODE"]),
                                    PRODUCETYPEID = dr["PRODUCETYPEID"].ToString(),
                                    PRODUCETYPE = dr["PRODUCETYPE"].ToString(),
                                    PRODUCTID = Convert.ToInt64(dr["PRODUCTID"]),
                                    PRODUCTNAME = dr["PRODUCTNAME"].ToString(),
                                    UNITNAME = dr["UNITNAME"].ToString(),
                                    PRICE = Convert.ToString(dr["PRICE"]),
                                    PRODUCE_QTY_Citizen = Convert.ToInt64(dr["PRODUCE_QTY_Citizen"]),
                                    PRODUCE_QTY_Department = Convert.ToInt64(dr["PRODUCE_QTY_Department"]),
                                    PRODUCE_QTY_CitizenPritory = Convert.ToInt32(dr["EmitraHeadPritory"]),
                                    PRODUCE_QTY_CitizenPritory1 = Convert.ToInt32(dr["EmitraHeadPritory1"]),
                                    PRODUCE_QTY_CitizenPritory2 = Convert.ToInt32(dr["EmitraHeadPritory2"]),
                                    DiscountForCitizen = Convert.ToString(dr["DiscountForCitizen"]),
                                    DiscountForGovt = Convert.ToString(dr["DiscountForGovt"]),
                                    DiscountForNGO = Convert.ToString(dr["DiscountForNGO"]),
                                    IsDiscountApplicable = Convert.ToBoolean(dr["IsDiscountApplicable"]),
                                    IsActive = Convert.ToBoolean(dr["IsActive"])

                                });
                        }

                        objInvntry.ListInventory = InvList;

                        return RedirectToAction("ManageNurseryInventory");

                    }
                    else
                    {
                        dsProduces = objInvntry.FetchLoadNurseryList();

                        //foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                        //{
                        //    Range.Add(new SelectListItem { Text = @dr["NURSERY_NAME"].ToString(), Value = @dr["NURSERY_CODE"].ToString() });
                        //}

                        objInvntry.NurseryList = new SelectList(dsProduces.Tables[0].AsDataView(), "NURSERY_CODE", "NURSERY_NAME").ToList();


                        objInvntry.NurseryList = Range;
                        objInvntry.ListInventory = InvList;

                        TempData["StockStatus"] = "Failed";
                        return RedirectToAction("ManageNurseryInventory");
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return View();
        }
        /// <summary>
        /// added by pooran on 05-07-2019 to save individual product detail through ajax call
        /// </summary>
        /// <param name="selectedIndex">Represents Changed Record Index</param>
        /// <param name="inventoryManagement">Modle For Inventory Management</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSingleProductRecord(int selectedIndex, [System.Web.Http.FromBody] listInventoryData inventoryData)
        {
            string returntext = string.Empty;
            InventoryManagement objInvntry = new InventoryManagement();

            try
            {
                objInvntry.nurseryDepotCode = inventoryData.DEPOT_NURSERY_CODE;
                objInvntry.StockName = inventoryData.StockName;

                DataTable DTs = listInventoryDataDetailsForSingleRecord(inventoryData);

                Int64 result = objInvntry.INSERT_UPDATE_LIST(DTs, objInvntry.StockName);

                if (result > 0)
                {
                    
                    TempData["StockStatus"] = "";
                    returntext = "Product Info Saved Successfully.";
                }
                else
                {
                    returntext = "Error While Saving Product Info.";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                returntext = exception.Message;
            }
            //return new Json(,JsonRequestBehavior.AllowGet);
            return Json(returntext, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateSingleProductRecord(int selectedIndex, [System.Web.Http.FromBody] listInventoryData inventoryData)
        {
            string returntext = string.Empty;
            InventoryManagement objInvntry = new InventoryManagement();
            
            try
            {


                inventoryData.FlgActionCitizen = 3;
                inventoryData.FlgActionDept = 3;
                
                DataTable result = objInvntry.EditWebProductStock(inventoryData);


                if (result.Rows.Count > 0)
                {
                    foreach (DataRow dr in result.Rows)
                    {
                        returntext = "" + dr["Msg"].ToString();
                    }
                       
                }
                else
                {
                    returntext = "Error While Saving Product Info.";
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                returntext = exception.Message;
            }
            //return new Json(,JsonRequestBehavior.AllowGet);
            return Json(returntext, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCurrentStock(string NurseryID)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.GetCurrentStock(NurseryID);

                foreach (System.Data.DataRow dr in dsProduces.Tables[0].Rows)
                {
                    list.Add(new SelectListItem { Text = @dr["StockName"].ToString(), Value = @dr["StockName"].ToString() });
                }

            }
            catch (Exception ex)
            {
                list = new List<SelectListItem>();
                new Common().ErrorLog(ex.Message, "GetCurrentStock" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Function searches out for Nursery(s) on the basis on Nurrsery Code
        /// </summary>
        /// <returns>Json Result for Nurseries</returns>
        [HttpPost]
        public JsonResult FetchDetails(string tpID, string stockID)
        {
            try
            {
                InventoryManagement _objProduce = new InventoryManagement();
                List<InventoryManagement.LotNumbers> items = new List<InventoryManagement.LotNumbers>();
                InventoryManagement.LotNumbers _objLots = new InventoryManagement.LotNumbers();
                DataSet dsResult = _objProduce.FetchForestProduceDetails(tpID, stockID);
                DataTable dt = dsResult.Tables[0];
                if (tpID != "")
                {
                    _objProduce = new InventoryManagement
                    {
                        TPID = dt.Rows[0]["TRANSITPERMITNAME"].ToString(),
                        nurseryDepotCode = dt.Rows[0]["TODEPOT_NURSERYCODE"].ToString(),
                        nurseryDepotName = dt.Rows[0]["DEPOT_NAME"].ToString(),
                        produceTypeID = Convert.ToInt64(dt.Rows[0]["PRODUCETYPEID"].ToString()),
                        produceType = dt.Rows[0]["PRODUCETYPE"].ToString(),
                        produceID = Convert.ToInt64(dt.Rows[0]["PRODUCTID"].ToString()),
                        produce = dt.Rows[0]["PRODUCTNAME"].ToString(),
                        produceUnit = dt.Rows[0]["UNITNAME"].ToString(),
                        produceQty = Convert.ToDouble(dt.Rows[0]["TRANSFERQTY"].ToString()),
                        transportMode = dt.Rows[0]["MODEOFTRANSPORT"].ToString(),
                        vehicleNo = dt.Rows[0]["VEHICLENUMBER"].ToString(),
                        driverName = dt.Rows[0]["DRIVER_NAME"].ToString(),
                        driverNumber = dt.Rows[0]["DRIVER_MOBNO"].ToString(),
                        DivName = dt.Rows[0]["Div_Name"].ToString(),
                        CircleName = dt.Rows[0]["Circle_Name"].ToString()
                    };
                }
                else
                {
                    _objProduce = new InventoryManagement
                    {
                        stockID = Convert.ToInt64(dt.Rows[0]["STOCKID"]),
                        nurseryDepotCode = dt.Rows[0]["DEPOT_NURSERY_CODE"].ToString(),
                        nurseryDepotName = dt.Rows[0]["DEPOT_NAME"].ToString(),
                        produceType = dt.Rows[0]["PRODUCETYPE"].ToString(),
                        produce = dt.Rows[0]["PRODUCTNAME"].ToString(),
                        produceUnit = dt.Rows[0]["UNITNAME"].ToString(),
                        produceQty = Convert.ToDouble(dt.Rows[0]["PRODUCE_QTY"].ToString()),
                        DivName = dt.Rows[0]["Div_Name"].ToString(),
                        CircleName = dt.Rows[0]["Circle_Name"].ToString()
                    };

                    if (dsResult.Tables[1] != null)
                    {
                        foreach (DataRow dr in dsResult.Tables[1].Rows)
                        {
                            _objLots = new InventoryManagement.LotNumbers
                            {
                                lotId = Convert.ToInt16(dr["LotId"].ToString()),
                                lotNumber = dr["LotNumber"].ToString(),
                                lotQuantity = dr["Quantity"].ToString()
                            };
                            items.Add(_objLots);
                        }
                    }
                }
                return Json(new { list = _objProduce, list1 = items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "FetchDetails" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                throw ex;
            }
        }

        /// <summary>
        /// Function searches out for Nursery(s) on the basis on Nurrsery Code
        /// </summary>
        /// <returns>Json Result for Nurseries</returns>
        [HttpPost]
        public JsonResult GetInventoryItemDetails(string objID)
        {
            try
            {
                DODItemAddedToInventory _objIATInventory = new DODItemAddedToInventory();
                DataSet dsResult = new InventoryManagement().FetchItemInventoryDetails(objID);
                _objIATInventory = Util.GetListFromTable<DODItemAddedToInventory>(dsResult, 0).FirstOrDefault();
                return Json(new { list = _objIATInventory }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetProductByProductTypeID(string productTypeID)
        {
            try
            {
                var _objProduct = new InventoryManagement().GetProductByProductType(productTypeID).AsEnumerable().Select(a => new DropDownList
                {
                    Value = a.Field<Int64>("ID"),
                    Text = a.Field<string>("ProductName")
                }).ToList();

                var _objUnitDetails = new InventoryManagement().GetUnitDetails(productTypeID).AsEnumerable().Select(a => new
                {
                    UnitName = a.Field<string>("UnitName")
                }).FirstOrDefault();

                return Json(new { productList = _objProduct, unitDetails = _objUnitDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetProductByProductTypeID" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
            }
            return null;
        }

        [HttpPost]
        public JsonResult FetchNurseryProduceDetails(string stockID)
        {
            try
            {
                InventoryManagement _objProduce = new InventoryManagement();
                List<InventoryManagement.LotNumbers> items = new List<InventoryManagement.LotNumbers>();
                InventoryManagement.LotNumbers _objLots = new InventoryManagement.LotNumbers();
                DataSet dsResult = _objProduce.FetchNurseryProduceDetails(stockID);
                DataTable dt = dsResult.Tables[0];
                _objProduce = new InventoryManagement
                {
                    stockID = Convert.ToInt64(dt.Rows[0]["STOCKID"]),
                    VillageCode = dt.Rows[0]["VILL_NAME"].ToString(),
                    InventoryRange = dt.Rows[0]["RANGE_NAME"].ToString(),
                    nurseryDepotCode = dt.Rows[0]["DEPOT_NURSERY_CODE"].ToString(),
                    nurseryDepotName = dt.Rows[0]["NURSERY_NAME"].ToString(),
                    produceType = dt.Rows[0]["PRODUCETYPE"].ToString(),
                    produce = dt.Rows[0]["PRODUCTNAME"].ToString(),
                    produceUnit = dt.Rows[0]["UNITNAME"].ToString(),
                    InventoryproduceQty = Convert.ToDecimal(dt.Rows[0]["PRODUCE_QTY"].ToString()),
                    Inventoryd_produceQty = Convert.ToDecimal(dt.Rows[0]["PRODUCE_QTY_Department"]),
                    InventoryCitizenDiscount = Convert.ToString(dt.Rows[0]["DiscountForCitizen"]),
                    InventoryGovDiscount = Convert.ToString(dt.Rows[0]["DiscountForGovt"]),
                    InventoryNGODiscount = Convert.ToString(dt.Rows[0]["DiscountForNGO"]),

                };

                List<MapNurserieHeadPrice> itemsMapNurserieHeadPrice = new List<MapNurserieHeadPrice>();

                DataTable dsMemberVehle = dsResult.Tables[1];

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

                var json = Json(new { _objProduce, VehiclePartialView, VehicleStatus }, JsonRequestBehavior.AllowGet);
                return json;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "FetchNurseryProduceDetails" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                throw ex;
            }



        }

        /// <summary>
        /// Function searches out for Nursery(s) on the basis on Nurrsery Code
        /// </summary>
        /// <returns>Json Result for Nurseries</returns>
        [HttpPost]
        public ActionResult UpdateProduceStock(string stockID, double produceQty, string actionFlag)
        {
            try
            {
                InventoryManagement _objProduce = new InventoryManagement();
                if (_objProduce.UpdateProduceStock(stockID, produceQty, actionFlag) > 0)
                {
                    if (actionFlag.Equals("DELETE"))
                        Session["StockStatus"] = " Selected item has been de-activated from the system!";
                    if (actionFlag.Equals("ACTIVATE"))
                        Session["StockStatus"] = " Selected item has been activated into the system!";
                    if (actionFlag.Equals("UPDATE"))
                        Session["StockStatus"] = " Stock has been updated successfully!";
                }
                else
                    Session["StockStatus"] = "Failed";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateProduceStock" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                throw ex;
            }
            return Json(Session["StockStatus"].ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateNurseryProduceStock(string stockID, double produceQty, double produceQtydpt, string actionFlag)
        {
            try
            {
                InventoryManagement _objProduce = new InventoryManagement();
                if (_objProduce.UpdateNurseryProduceStock(stockID, produceQty, produceQtydpt, actionFlag) > 0)
                {
                    if (actionFlag.Equals("DELETE"))
                        Session["StockStatus"] = " Selected item has been de-activated from the system!";
                    if (actionFlag.Equals("ACTIVATE"))
                        Session["StockStatus"] = " Selected item has been activated into the system!";
                    if (actionFlag.Equals("UPDATESAVE"))
                        Session["StockStatus"] = " Stock has been updated successfully!";
                }
                else
                    Session["StockStatus"] = "Failed";
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateNurseryProduceStock" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
            }
            return Json(Session["StockStatus"].ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddItemsToInventory(InventoryManagement InventoryObj, FormCollection formUser, string Command)
        {
            try
            {
                if (Command.Equals("UPDATE"))
                {
                    InventoryObj.lotNumber = Convert.ToString(formUser["ddllotNumber"]);
                    InventoryObj.TPID = Convert.ToString(formUser["TPID"]);
                    InventoryObj.produceQty = Convert.ToDouble(formUser["deductibleQty"]);
                }

                Int64 result = InventoryObj.AddItemsToInventory(Command);
                if (result > 0)
                {
                    if (Command.Equals("INSERT"))
                        Session["StockStatus"] = "Item(s) has been updated into the Inventory successfully!";
                    if (Command.Equals("UPDATE"))
                        Session["StockStatus"] = "Item(s) has been deducted from Inventory successfully!";
                    return RedirectToAction("ManageInventory", "ManageInventory", false);
                }
                else
                {
                    Session["StockStatus"] = "Failed";
                    return null;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "AddItemsToInventory" + "_" + "ManageInventoryController", 0, DateTime.Now, Convert.ToInt64(Session["UserId"]));
                throw ex;
            }
        }

        public ActionResult UpdateWriteOff(InventoryWriteOff model)
        {
            ResponseMsg res = new ResponseMsg();
            res = new InventoryManagement().UpdateWriteOff(model);
            DataSet dsProduces = new InventoryManagement().FetchDODInventoryDetails();
            var objInventory = Util.GetListFromTable<DODInventory>(dsProduces, 0);
            ViewData["lstInventory"] = objInventory;
            ViewBag.ResponseMsg = res;
            return PartialView("_ItemAvailableList");
        }

        public ActionResult UpdateInventoryQty(DODInventory model)
        {
            ResponseMsg msg = new ResponseMsg();
            var dsResult = new InventoryManagement().UpdateInventory(model, 2);
            if (Util.isValidDataSet(dsResult, 0))
            {
                msg = Util.GetListFromTable<Entity.ResponseMsg>(dsResult, 0).FirstOrDefault();
            }
            DataSet dsProduces = new InventoryManagement().FetchDODInventoryDetails(3);
            var objInventory = Util.GetListFromTable<DODInventory>(dsProduces, 0);
            ViewData["lstInventory"] = objInventory;
            ViewBag.ResponseMsg = msg;

            #region Email and SMS 
            try
            {
                if (Util.isValidDataSet(dsResult, 1))
                {
                    DataRow dRow = dsResult.Tables[1].Rows[0];
                    new SMSandEMAILtemplate().SendMailComman("ALL", "DOD_InventoryQtyChange", Convert.ToString(model.InventoryID), "", Convert.ToString(dRow["EmailId"]), Convert.ToString(dRow["Mobile"]), "", "", new string[] { Convert.ToString(model.OldQty), Convert.ToString(model.Qty), Convert.ToString(dRow["DisplayLotNumber"]) });
                }
            }
            catch { }
            #endregion
            return PartialView("_UpdateInventoryList");
        }

        public ActionResult DeActivateInventory(string parentID)
        {
            ResponseMsg msg = new ResponseMsg();
            var dsResult = new InventoryManagement().DeActivateInventory(parentID);
            if (Util.isValidDataSet(dsResult, 0))
            {
                msg = Util.GetListFromTable<Entity.ResponseMsg>(dsResult, 0).FirstOrDefault();

                #region Email and SMS 
                try
                {
                    if (Util.isValidDataSet(dsResult, 1))
                    {
                        DataRow dRow = dsResult.Tables[1].Rows[0];
                        new SMSandEMAILtemplate().SendMailComman("ALL", "DOD_InventoryDeActivate", parentID, "", Convert.ToString(dRow["EmailId"]), Convert.ToString(dRow["Mobile"]), "", "", new string[] { Convert.ToString(dRow["DisplayLotNumber"]) });
                    }
                }
                catch { }
                #endregion
            }
            TempData["ReturnMsg"] = msg.ReturnMsg;
            TempData["IsError"] = msg.IsError;
            return Json(new { IsError = msg.IsError, ReturnMsg = msg.ReturnMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateItemToInventory(DODItemAddedToInventory model, string Command)
        {
            ResponseMsg res = new ResponseMsg();
            res = new InventoryManagement().UpdateDODInventory(model, Command);
            DataSet dsProduces = new InventoryManagement().FetchDODInventoryDetails();
            var objInventory = Util.GetListFromTable<DODItemAddedToInventory>(dsProduces, 1);
            ViewData["lstInventoryProduces"] = objInventory;
            ViewBag.ResponseMsg = res;
            return PartialView("_ItemInventorylist");
        }

        public ActionResult LoadPartial(string value, long objID)
        {
            dynamic data = null;
            if (objID == 0)
            {
                DataSet ds = new InventoryManagement().GetDataForLot();
                var _objData = new InventoryManagement().GetDataForLot().Tables[0].AsEnumerable().Select(a => new SelectListItem
                {
                    Value = Convert.ToString(a.Field<long>("ID")),
                    Text = a.Field<string>("ProduceType")
                }).ToList();

                ViewBag.ProduceType = _objData;

                _objData = new InventoryManagement().GetDataForLot().Tables[1].AsEnumerable().Select(a => new SelectListItem
                {
                    Value = Convert.ToString(a.Field<long>("Depot_Id")),
                    Text = a.Field<string>("Depot_Name")
                }).ToList();

                ViewBag.DepotFromList = _objData;

                _objData = new InventoryManagement().GetDataForLot().Tables[2].AsEnumerable().Select(a => new SelectListItem
                {
                    Value = Convert.ToString(a.Field<long>("Depot_Id")),
                    Text = a.Field<string>("Depot_Name")
                }).ToList();

                ViewBag.DepotToList = _objData;
            }
            else
            {
                DataSet dsResult = new InventoryManagement().FetchItemInventoryDetails(objID.ToString());
                data = Util.GetListFromTable<DODItemAddedToInventory>(dsResult, 0).FirstOrDefault();
                data.TPProductList = Util.GetListFromTable<DODTPProductDetails>(dsResult, 1);
                ViewBag.IATInventory = data;
            }

            return PartialView(value, data);
        }



        public ActionResult LoadPartialCommon(string partialViewName, long objID, string type)
        {
            dynamic _obj = null;
            dynamic data = null;
            DataSet dsResult = null;
            DataTable dt = null;

            switch (type)
            {
                case "ReceiverLog":
                    _obj = new InventoryReceiverLog();
                    dsResult = new InventoryManagement().GetInventoryReceiverLog(objID);
                    data = Util.GetListFromTable<InventoryReceiverLog>(dsResult, 0);
                    ViewData["ReceiverLog"] = data;
                    break;
                case "WriteOff":
                    dt = new FMDSS.Models.ForestDevelopment.TransitPermit().GetDetailsByInventory(Convert.ToString(objID));
                    _obj = Util.GetListFromTable<InventoryWriteOff>(dt).FirstOrDefault();
                    break;
                case "InventoryData":
                    dt = new FMDSS.Models.ForestDevelopment.TransitPermit().GetDetailsByInventory(Convert.ToString(objID), 3);
                    _obj = Util.GetListFromTable<DODInventory>(dt).FirstOrDefault();
                    break;
                case "TPDetails":
                    dsResult = new InventoryManagement().FetchItemInventoryDetails(objID.ToString());
                    _obj = Util.GetListFromTable<DODItemAddedToInventory>(dsResult, 0).FirstOrDefault();
                    _obj.TPProductList = Util.GetListFromTable<DODTPProductDetails>(dsResult, 1);
                    break;
            }
            return PartialView(partialViewName, _obj);
        }

        [HttpPost]
        public ActionResult AddItemsToNursery(List<MapNurserieHeadPrice> LstHeadDetails, InventoryManagement InventoryObj, FormCollection formUser, string Command)
        {

            try
            {
                InventoryManagement _objProduce = new InventoryManagement();
                if (Command.Equals("UPDATE"))
                {
                    //if (Convert.ToString(formUser["speciesFor"]) == "Sale")
                    //    InventoryObj.produceQty = Convert.ToDouble(formUser["NurseryQtyproduceQty"]);
                    //else
                    //    InventoryObj.d_produceQty = Convert.ToDouble(formUser["NurseryQtyproduceQty"]);

                    InventoryObj.stockID = Convert.ToInt64(formUser["InventorystockID"]);

                    // if (_objProduce.UpdateNurseryProduceStock(Convert.ToString(formUser["InventorystockID"]), InventoryObj.produceQty, InventoryObj.d_produceQty, Command) > 0)
                    //{
                    //    Session["StockStatus"] = "Item(s) has been updated into the Inventory successfully!";
                    //    return RedirectToAction("ManageNurseryInventory", "ManageInventory", false);
                    //}

                    DataTable DT = getDTofMapNurserieHeadPrice(LstHeadDetails);

                    Int64 result = InventoryObj.UpdateItemsToNursery(Command, DT);

                    if (result > 0)
                    {
                        Session["StockStatus"] = "Item(s) has been updated into the Inventory successfully!";
                        return RedirectToAction("ManageNurseryInventory", "ManageInventory", false);
                    }
                    else
                    {
                        Session["StockStatus"] = "Failed";
                        return null;
                    }

                }
                else
                {

                    InventoryObj.TPID = Convert.ToString(formUser["ddlWorkOrder"]);
                    InventoryObj.nurseryDepotCode = Convert.ToString(formUser["ddlNursery"]);
                    InventoryObj.produceTypeID = Convert.ToInt64(formUser["ddlProduceType"]);
                    InventoryObj.produceID = Convert.ToInt64(formUser["ddlProduce"]);

                    DataTable DT = getDTofMapNurserieHeadPrice(LstHeadDetails);

                    Int64 result = InventoryObj.AddItemsToNursery(Command, DT);
                    if (result > 0)
                    {
                        if (Command.Equals("INSERT"))
                            Session["StockStatus"] = "Item(s) has been added into the Inventory successfully!";
                        else
                            Session["StockStatus"] = "Item(s) has been updated into the Inventory successfully!";
                        return RedirectToAction("ManageNurseryInventory", "ManageInventory", false);
                    }
                    else
                    {
                        Session["StockStatus"] = "Failed";
                        return null;
                    }

                }
                return RedirectToAction("ManageNurseryInventory", "ManageInventory", false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Function to bind all Village to dropdownlist
        /// </summary>
        /// <param name="RangeCode"></param>
        /// <returns></returns>

        [HttpPost]
        public JsonResult getObjectsForFurseryOperation(string objectType, string objectCode, bool IsWorkOrder, string workOrder = null)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    List<SelectListItem> items1 = new List<SelectListItem>();
                    InventoryManagement _objProduce = new InventoryManagement();
                    DataSet ds = _objProduce.getObjectsForFurseryOperation(objectType, objectCode, IsWorkOrder, workOrder);

                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
                    }
                    if (ds.Tables.Count == 2)
                    {
                        foreach (System.Data.DataRow dr in ds.Tables[1].Rows)
                        {
                            items1.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
                        }
                    }
                    return Json(new { list = items, list1 = items1 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 3, DateTime.Now, Convert.ToInt64(Session["UserID"]));
            }
            return null;

        }

        [HttpPost]
        public JsonResult getHeadDetails(string objectType)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            try
            {
                if (Session["UserID"] != null)
                {
                    List<MapNurserieHeadPrice> itemsMapNurserieHeadPrice = new List<MapNurserieHeadPrice>();

                    InventoryManagement _objProduce = new InventoryManagement();
                    DataTable dsMemberVehle = _objProduce.GetHeadDetails("0");

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

        [NonAction]
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

        public DataTable listInventoryDataDetails(List<listInventoryData> lstlistInventoryData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");

            try
            {

                #region Vehicle Info
                objDt2.Columns.Add("stockID", typeof(Int64));
                objDt2.Columns.Add("DEPOT_NURSERY_CODE", typeof(String));
                objDt2.Columns.Add("PRODUCETYPEID", typeof(String));
                objDt2.Columns.Add("PRODUCETYPE", typeof(String));
                objDt2.Columns.Add("PRODUCTID", typeof(String));
                objDt2.Columns.Add("PRODUCTNAME", typeof(String));
                objDt2.Columns.Add("UNITNAME", typeof(String));
                objDt2.Columns.Add("PRICE", typeof(String));

                objDt2.Columns.Add("PRODUCE_QTY_Citizen", typeof(String));//8235-200-06-RFBP(NP) Head
                objDt2.Columns.Add("PRODUCE_QTY_Citizen1", typeof(String));//0406-01-80-05 Head
                objDt2.Columns.Add("PRODUCE_QTY_Citizen2", typeof(String));//Campa

                objDt2.Columns.Add("RECEIVEDQTYPritory", typeof(String));//8235-200-06-RFBP(NP) Head
                objDt2.Columns.Add("RECEIVEDQTYPritory1", typeof(String));//0406-01-80-05 Head
                objDt2.Columns.Add("RECEIVEDQTYPritory2", typeof(String));//Campa

                objDt2.Columns.Add("PRODUCE_HeadPrimaryStatus", typeof(String));

                objDt2.Columns.Add("PRODUCE_QTY_Department", typeof(String));
                objDt2.Columns.Add("DiscountForCitizen", typeof(String));
                objDt2.Columns.Add("DiscountForGovt", typeof(String));
                objDt2.Columns.Add("DiscountForNGO", typeof(String));

                objDt2.AcceptChanges();




                foreach (var item in lstlistInventoryData)
                {
                    if ((item.PRODUCE_QTY_Citizen >= 0) || (item.PRODUCE_QTY_Department >= 0))  /// if citizen or department qty is exist change by amit on 01-05-2018
                    {
                        DataRow dr = objDt2.NewRow();
                        dr["stockID"] = item.stockID;
                        dr["DEPOT_NURSERY_CODE"] = item.DEPOT_NURSERY_CODE;
                        dr["PRODUCETYPEID"] = item.PRODUCETYPEID;
                        dr["PRODUCETYPE"] = item.PRODUCETYPE;
                        dr["PRODUCTID"] = item.PRODUCTID;
                        dr["PRODUCTNAME"] = Convert.ToString(Convert.ToInt16(item.IsDiscountApplicable));// dr PRODUCTNAME use as IsDiscountApplicable;
                        dr["UNITNAME"] = item.UNITNAME;
                        dr["PRICE"] = item.PRICE;
                        dr["PRODUCE_QTY_Citizen"] = item.PRODUCE_QTY_Citizen;
                        dr["PRODUCE_QTY_Citizen1"] = item.PRODUCE_QTY_Citizen1;
                        dr["PRODUCE_QTY_Citizen2"] = item.PRODUCE_QTY_Citizen2;
                        dr["RECEIVEDQTYPritory"] = item.PRODUCE_QTY_CitizenPritory;
                        dr["RECEIVEDQTYPritory1"] = item.PRODUCE_QTY_CitizenPritory1;
                        dr["RECEIVEDQTYPritory2"] = item.PRODUCE_QTY_CitizenPritory2;

                        dr["PRODUCE_HeadPrimaryStatus"] = item.PRODUCE_HeadPrimaryStatus;

                        dr["PRODUCE_QTY_Department"] = item.PRODUCE_QTY_Department;
                        dr["DiscountForCitizen"] = item.DiscountForCitizen;
                        dr["DiscountForGovt"] = item.DiscountForGovt;
                        dr["DiscountForNGO"] = Convert.ToString(Convert.ToInt16(item.IsActive));

                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }
        /// <summary>
        /// To update single record changed by user : added by pooran on 04-07-2019
        /// </summary>
        /// <param name="inventoryData">Record Which Contains Changes Done By Admin</param>
        /// <returns></returns>
        public DataTable listInventoryDataDetailsForSingleRecord(listInventoryData inventoryData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            DataTable objDt2 = new DataTable("Table");

            try
            {

                objDt2.Columns.Add("stockID", typeof(Int64));
                objDt2.Columns.Add("DEPOT_NURSERY_CODE", typeof(String));
                objDt2.Columns.Add("PRODUCETYPEID", typeof(String));
                objDt2.Columns.Add("PRODUCETYPE", typeof(String));
                objDt2.Columns.Add("PRODUCTID", typeof(String));
                objDt2.Columns.Add("PRODUCTNAME", typeof(String));
                objDt2.Columns.Add("UNITNAME", typeof(String));
                objDt2.Columns.Add("PRICE", typeof(String));

                objDt2.Columns.Add("PRODUCE_QTY_Citizen", typeof(String));//8235-200-06-RFBP(NP) Head
                objDt2.Columns.Add("PRODUCE_QTY_Citizen1", typeof(String));//0406-01-80-05 Head
                objDt2.Columns.Add("PRODUCE_QTY_Citizen2", typeof(String));//Campa

                objDt2.Columns.Add("RECEIVEDQTYPritory", typeof(String));//8235-200-06-RFBP(NP) Head
                objDt2.Columns.Add("RECEIVEDQTYPritory1", typeof(String));//0406-01-80-05 Head
                objDt2.Columns.Add("RECEIVEDQTYPritory2", typeof(String));//Campa

                objDt2.Columns.Add("PRODUCE_HeadPrimaryStatus", typeof(String));

                objDt2.Columns.Add("PRODUCE_QTY_Department", typeof(String));
                objDt2.Columns.Add("DiscountForCitizen", typeof(String));
                objDt2.Columns.Add("DiscountForGovt", typeof(String));
                objDt2.Columns.Add("DiscountForNGO", typeof(String));
                objDt2.Columns.Add("BASEPRODUCETYPEID", typeof(int));
                objDt2.AcceptChanges();

                if ((inventoryData.PRODUCE_QTY_Citizen >= 0) || (inventoryData.PRODUCE_QTY_Department >= 0))  /// if citizen or department qty is exist change by amit on 01-05-2018
                {
                    DataRow dr = objDt2.NewRow();
                    dr["stockID"] = inventoryData.stockID;
                    dr["DEPOT_NURSERY_CODE"] = inventoryData.DEPOT_NURSERY_CODE;
                    dr["PRODUCETYPEID"] = inventoryData.PRODUCETYPEID;
                    dr["PRODUCETYPE"] = inventoryData.PRODUCETYPE;
                    dr["PRODUCTID"] = inventoryData.PRODUCTID;
                    dr["PRODUCTNAME"] = Convert.ToString(Convert.ToInt16(inventoryData.IsDiscountApplicable));// dr PRODUCTNAME use as IsDiscountApplicable;
                    dr["UNITNAME"] = inventoryData.UNITNAME;
                    dr["PRICE"] = inventoryData.PRICE;
                    dr["PRODUCE_QTY_Citizen"] = inventoryData.PRODUCE_QTY_Citizen;
                    dr["PRODUCE_QTY_Citizen1"] = inventoryData.PRODUCE_QTY_Citizen1;
                    dr["PRODUCE_QTY_Citizen2"] = inventoryData.PRODUCE_QTY_Citizen2;
                    dr["RECEIVEDQTYPritory"] = inventoryData.PRODUCE_QTY_CitizenPritory;
                    dr["RECEIVEDQTYPritory1"] = inventoryData.PRODUCE_QTY_CitizenPritory1;
                    dr["RECEIVEDQTYPritory2"] = inventoryData.PRODUCE_QTY_CitizenPritory2;

                    dr["PRODUCE_HeadPrimaryStatus"] = inventoryData.PRODUCE_HeadPrimaryStatus;

                    dr["PRODUCE_QTY_Department"] = inventoryData.PRODUCE_QTY_Department;
                    dr["DiscountForCitizen"] = inventoryData.DiscountForCitizen;
                    dr["DiscountForGovt"] = inventoryData.DiscountForGovt;
                    dr["DiscountForNGO"] = Convert.ToString(Convert.ToInt16(inventoryData.IsActive));
                    dr["BASEPRODUCETYPEID"] = inventoryData.BASEPRODUCETYPE_ID;


                    objDt2.Rows.Add(dr);
                    objDt2.AcceptChanges();
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }

        public ActionResult ReleaseAllItems()
        {

            InventoryManagement objInvntry = new InventoryManagement();
            TempData["Message"] = "<div id='divSuccess' class='alert alert-success'><i class='fa fa-thumbs-o-up fa-fw'></i>" + objInvntry.ReleaseAllProduct() + "</div>";
            return RedirectToAction("ManageNurseryInventory");
        }
    }
}
