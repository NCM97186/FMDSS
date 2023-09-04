using FMDSS.CustomModels.Models;
using FMDSS.Models;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.Master;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class NurseryAPIController : ApiController
    {
        #region [API For Finacial Year List]
        [HttpGet]
        public DataSetResponse GetFinacialYearList()
        {
            DataSetResponse response = new DataSetResponse();
            ProtectionRepository _protectionRepository = new ProtectionRepository();
            try
            {
                DataSet dsFinacialYear = new DataSet();
                //dsFinacialYear = _protectionRepository.GetFinancialYear()
                dsFinacialYear = _protectionRepository.GetFinancialYear2();

                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsFinacialYear };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion

        #region [API FOR Plant Category List]
        [HttpGet]
        public DataSetResponse GetPlantCategoryList()
        {
            DataSetResponse response = new DataSetResponse();
            NurseryFDMProduct obj = new NurseryFDMProduct();
            try
            {
                DataSet dsPlantcatList = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                dt = obj.ProductCategoryList();
                dt2 = obj.ProductLeafTypeNurseryList();
                dsPlantcatList.Tables.Add(dt);
                dsPlantcatList.Tables.Add(dt2);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsPlantcatList };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse GetProductProduceTypeList(int ProduceTypeId)
        {
            DataSetResponse response = new DataSetResponse();
            NurseryFDMProduct obj = new NurseryFDMProduct();
            try
            {
                DataSet dsPlantcatList = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                dt = obj.ProductProduceTypeList(ProduceTypeId);
                dsPlantcatList.Tables.Add(dt);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsPlantcatList };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion

        #region [API For Listing Plants]
        [HttpGet]
        public DataSetResponse GetPlantList(string ProduceTypeId, int ProductCategoryId)
        {
            DataSetResponse response = new DataSetResponse();
            NurseryFDMProduct obj = new NurseryFDMProduct();
            try
            {
                DataSet dsPlantList = new DataSet();
                DataTable dt = new DataTable();
                dt = obj.ProductList(ProduceTypeId, ProductCategoryId);
                dsPlantList.Tables.Add(dt);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsPlantList };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion

        #region [API For Nursary detail]
        [HttpGet]
        public DataSetResponse GetNurseryList(long UserID, string SSOId = "")
        {
            DataSetResponse response = new DataSetResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            System.Web.HttpContext.Current.Session["UserId"] = UserID;
            //Below Line is added on 16-08-2021, for GGAY APK
            System.Web.HttpContext.Current.Session["SSOId"] = SSOId;
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryListAPI();
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        public DataSetResponse GetNurseryList(long UserID, string CircleCode, string DivisionCode, string RangeCode, string SSOId = "")
        {
            DataSetResponse response = new DataSetResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            System.Web.HttpContext.Current.Session["UserId"] = UserID;
            //Below Line is added on 16-08-2021, for GGAY APK
            System.Web.HttpContext.Current.Session["SSOId"] = SSOId;
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryListAPI(CircleCode, DivisionCode, RangeCode);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion

        [HttpGet]
        public DataSetResponse GetNurseryListDivisionWise(long UserID, string circleCode = "", string divisionCode = "", string rangeCode = "")
        {
            DataSetResponse response = new DataSetResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            System.Web.HttpContext.Current.Session["UserId"] = UserID;
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryListDivisionWiseAPI(circleCode, divisionCode, rangeCode);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpGet]
        public DataSetResponse GetCurrentStock(string NurseryID, long UserID)
        {
            System.Web.HttpContext.Current.Session["UserId"] = UserID;
            DataSetResponse response = new DataSetResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.GetCurrentStockAPI(NurseryID);

                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
       
        [HttpGet]
        public DataSetResponse NurseryWithProductDetails(string NurseryID, long UserID, string StockName)
        {
            System.Web.HttpContext.Current.Session["UserId"] = UserID;
            DataSetResponse response = new DataSetResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.FetchLoadNurseryWithItemListAPI(NurseryID, StockName);

                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpPost]
        public DataSetResponse NurseyProductUpdate(listInventoryData InventoryObj)
        {
            DataSetResponse response = new DataSetResponse();
            System.Web.HttpContext.Current.Session["UserId"] = InventoryObj.UserId;
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet ds = new DataSet();
                //DataTable DTs = listInventoryDataDetails(InventoryObj);
                int result = objInvntry.UpdateNurseryProductAPI(InventoryObj);

                if (result > 0)
                    response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = ds };
                else
                    response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again." };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }

        public DataTableResponse GetNurseryProductType()
        {
            DataTableResponse response = new DataTableResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataTable dsProduces = new DataTable();
                dsProduces = objInvntry.NurseryProductsType();
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse NurseryProducts_OldNew(string OldNew, long UserID, string NursuryCode, int CategoryID, int ProductID, string DivCode, string RangeCode, string CircleCode, string FinacialYearDate, string PlantAge, int BaseProduceTypeId, int PRODUCE_TYPE, string SSOId = "")
        {
            DataTableResponse response = new DataTableResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataTable dsProduces = new DataTable();
                dsProduces = objInvntry.NurseryProducts_OldNew(OldNew, UserID, SSOId, NursuryCode, CategoryID, ProductID, DivCode, RangeCode, CircleCode, FinacialYearDate, PlantAge, BaseProduceTypeId, PRODUCE_TYPE);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        //[HttpGet]
        //public DataTableResponse NurseryProducts_OldNew(string OldNew, int BaseProductType, string NursuryCode)
        //{
        //    DataTableResponse response = new DataTableResponse();
        //    InventoryManagement objInvntry = new InventoryManagement();
        //    try
        //    {
        //        DataTable dsProduces = new DataTable();
        //        //dsProduces = objInvntry.NurseryProducts_OldNew(OldNew, BaseProductType, NursuryCode);
        //        response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

        //    }
        //    catch (Exception ex)
        //    {
        //        response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
        //    }
        //    return response;
        //}
        [HttpGet]
        public DataTableResponse NurseryProducts_OldNewProductList(string OldNew, int BaseProductType, string NursuryCode)
        {
            DataTableResponse response = new DataTableResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataTable dsProduces = new DataTable();
                dsProduces = objInvntry.NurseryProducts_OldNewProductList(OldNew, BaseProductType, NursuryCode);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;

        }

        //      [HttpPost] listInventoryData
        //public DataTableResponse AddUpdateProductStock(InventoryManagement.NurseryStock model)
        //      {
        //          DataTableResponse response = new DataTableResponse();
        //          InventoryManagement objInvntry = new InventoryManagement();
        //          try
        //          {
        //              DataTable dsProduces = new DataTable();
        //              dsProduces = objInvntry.AddUpdateProductStock(model);
        //              response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

        //          }
        //          catch (Exception ex)
        //          {
        //              response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
        //          }
        //          return response;
        //      }


        [HttpPost]
        public DataTableResponse AddUpdateProductStock(listInventoryData model)
        {
            DataTableResponse response = new DataTableResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataTable dsProduces = new DataTable();
                if (model.FlgActionCitizen <= 2 || model.FlgActionDept <= 2)
                    dsProduces = objInvntry.AddUpdateProductStock(model);
              

                if (model.FlgActionCitizen == 3 || model.FlgActionDept == 3)
                    dsProduces = objInvntry.EditProductStock(model);

                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        public DataTable listInventoryDataDetails(listInventoryData lstlistInventoryData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
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
                //foreach (var item in lstlistInventoryData)
                //{
                if ((lstlistInventoryData.PRODUCE_QTY_Citizen >= 0) || (lstlistInventoryData.PRODUCE_QTY_Department >= 0))
                {
                    DataRow dr = objDt2.NewRow();
                    dr["stockID"] = lstlistInventoryData.stockID;
                    dr["DEPOT_NURSERY_CODE"] = lstlistInventoryData.DEPOT_NURSERY_CODE;
                    dr["PRODUCETYPEID"] = lstlistInventoryData.PRODUCETYPEID;
                    dr["PRODUCETYPE"] = lstlistInventoryData.PRODUCETYPE;
                    dr["PRODUCTID"] = lstlistInventoryData.PRODUCTID;
                    dr["PRODUCTNAME"] = Convert.ToString(Convert.ToInt16(lstlistInventoryData.IsDiscountApplicable));
                    dr["UNITNAME"] = lstlistInventoryData.UNITNAME;
                    dr["PRICE"] = lstlistInventoryData.PRICE;
                    dr["PRODUCE_QTY_Citizen"] = lstlistInventoryData.PRODUCE_QTY_Citizen;
                    dr["PRODUCE_QTY_Citizen1"] = lstlistInventoryData.PRODUCE_QTY_Citizen1;
                    dr["PRODUCE_QTY_Citizen2"] = lstlistInventoryData.PRODUCE_QTY_Citizen2;
                    dr["RECEIVEDQTYPritory"] = lstlistInventoryData.PRODUCE_QTY_CitizenPritory;
                    dr["RECEIVEDQTYPritory1"] = lstlistInventoryData.PRODUCE_QTY_CitizenPritory1;
                    dr["RECEIVEDQTYPritory2"] = lstlistInventoryData.PRODUCE_QTY_CitizenPritory2;

                    dr["PRODUCE_HeadPrimaryStatus"] = lstlistInventoryData.PRODUCE_HeadPrimaryStatus;

                    dr["PRODUCE_QTY_Department"] = lstlistInventoryData.PRODUCE_QTY_Department;
                    dr["DiscountForCitizen"] = lstlistInventoryData.DiscountForCitizen;
                    dr["DiscountForGovt"] = lstlistInventoryData.DiscountForGovt;
                    dr["DiscountForNGO"] = Convert.ToString(Convert.ToInt16(lstlistInventoryData.IsActive));

                    objDt2.Rows.Add(dr);
                    objDt2.AcceptChanges();
                }

                //}
                #endregion
            }
            catch (Exception ex)
            {

            }
            return objDt2;
        }
        [HttpGet]
        public DataTableResponse GetNurseryDashboardCounts(string UserId,string SSOId="", string FYearId="", string NurseryID="")
        {
            DataTableResponse response = new DataTableResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.GetNurseryDashboardCounts(UserId, SSOId, FYearId, NurseryID);
                DataTable dtResponse = dsProduces.Tables[0];
                if (dtResponse.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "List fetch successfully.", Data = dtResponse };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again." };


            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetNurseryStock(string NurseryID)
        {
            DataTableResponse response = new DataTableResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.GetCurrentStock(NurseryID);
                DataTable dtResponse = dsProduces.Tables[0];
                if (dtResponse.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "List fetch successfully.", Data = dtResponse };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again." };


            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetNurseryInventory(string NurseryCode, long UserID, string StockName)
        {
            DataSet dsProduces = new DataSet();
            InventoryManagement objInvntry = new InventoryManagement();
            DataTableResponse response = new DataTableResponse();
            try
            {
                objInvntry.nurseryDepotCode = NurseryCode;
                objInvntry.StockName = StockName;
                System.Web.HttpContext.Current.Session["UserId"] = UserID;
                dsProduces = objInvntry.FetchLoadNurseryList();
                DataTable dtResponse = dsProduces.Tables[1];
                if (dtResponse.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Inventory fetch successfully.", Data = dtResponse };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again." };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;

        }

        public DataTableResponse SubmitNurseryInventory(List<listInventoryData> InvList, string StockName, long UserId)
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                InventoryManagement objInvntry = new InventoryManagement();
                DataTable DTs = listInventoryDataDetails(InvList, UserId);
                Int64 result = objInvntry.INSERT_UPDATE_LIST(DTs, StockName);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Data saved successfully." };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;

        }
        public DataTable listInventoryDataDetails(List<listInventoryData> lstlistInventoryData, long UserID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

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
        [HttpGet]
        public DataTableResponse GetNurseryStockList(string ActionName, int fYear, long UserID)
        {
            DataSet dsProduces = new DataSet();
            InventoryManagement objInvntry = new InventoryManagement();
            DataTableResponse response = new DataTableResponse();
            try
            {
                //System.Web.HttpContext.Current.Session["UserId"] = UserID;
                dsProduces = objInvntry.NurseryStockList(UserID, fYear, ActionName);
                DataTable dtResponse = dsProduces.Tables[0];
                if (dtResponse.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Inventory fetch successfully.", Data = dtResponse };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again." };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetWaterResourceList(string ActionName, long UserID)
        {
            DataSet dsProduces = new DataSet();
            InventoryManagement objInvntry = new InventoryManagement();
            DataTableResponse response = new DataTableResponse();
            try
            {
                //System.Web.HttpContext.Current.Session["UserId"] = UserID;
                dsProduces = objInvntry.WaterResourceList(UserID, ActionName);
                DataTable dtResponse = dsProduces.Tables[0];
                if (dtResponse.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Water Resource fetch successfully.", Data = dtResponse };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again." };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #region [API For Nursary detail Get]
        [HttpGet]

        public DataSetResponse GetNurseryDetailList(string NURSERY_CODE)
        {
            DataSetResponse response = new DataSetResponse();
            Nurserylist obj = new Nurserylist();
            // System.Web.HttpContext.Current.Session["UserId"] = UserId;
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = obj.FetchNurseryListAPI(NURSERY_CODE);

                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion


        #region [API For Nursary detail Update]
        [HttpPost]

        public DataSetResponse UpdateNurseryDetailList(Nurserylist obj1)
        {
            DataSetResponse response = new DataSetResponse();
            Nurserylist obj = new Nurserylist();
            System.Web.HttpContext.Current.Session["UserId"] = obj1.UserId;
            try
            {
                DataSet dsProduces = new DataSet();
              
                string host = HttpContext.Current.Request.Url.ToString().Replace("api/NurseryAPI/UpdateNurseryDetailList", "");
                dsProduces = obj.UpdateNurseryListAPI(obj1,host);
                string d = dsProduces.Tables[0].Rows[0]["Msg"].ToString();
                if (d == "Updated Successfully")
                {
                    response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };
                }
                else
                {

                    response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "", Data = dsProduces };
                }
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion
    }
}