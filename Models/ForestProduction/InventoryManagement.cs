using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System.Drawing.Imaging;

namespace FMDSS.Models.ForestProduction
{
    public class InventoryManagement : DAL
    {
        public long rowID { get; set; }
        public string TPID { get; set; }
        public long stockID { get; set; }
        public string produceFor { get; set; }
        public string nurseryDepotCode { get; set; }
        public string StockName { get; set; }
        public string nurseryDepotName { get; set; }
        public long produceTypeID { get; set; }
        public string produceType { get; set; }
        public long produceID { get; set; }
        public string produce { get; set; }
        public string produceUnit { get; set; }
        public double produceQty { get; set; }

        public double d_produceQty { get; set; }
        public string transportMode { get; set; }
        public string vehicleNo { get; set; }
        public string driverName { get; set; }
        public string driverNumber { get; set; }
        public bool isItemActive { get; set; }
        public string recComments { get; set; }
        public string lotCount { get; set; }
        public string lotNumber { get; set; }
        public double deductibleQty { get; set; }
        public string VillageCode { get; set; }

        public string InventoryRange { get; set; }
        public string InventoryVillage { get; set; }
        public string InventorySpeciesType { get; set; }
        public string InventorySpecies { get; set; }
        public string InventoryNursery { get; set; }

        public string InventoryCitizenDiscount { get; set; }
        public string InventoryNGODiscount { get; set; }
        public string InventoryGovDiscount { get; set; }
        public decimal InventoryproduceQty { get; set; }
        public decimal Inventoryd_produceQty { get; set; }

        public string speciesFor { get; set; }
        public string NurseryQtyproduceQty { get; set; }
        public string CitizenDiscount { get; set; }
        public string NGODiscount { get; set; }
        public string GovDiscount { get; set; }
        public string UnitType { get; set; }
        public string CircleName { get; set; }
        public string DivName { get; set; }
        public string Date { get; set; }
        public string FinacialYear { get; set; }
        public int AddNurseryPlantBtn { get; set; }

        public List<SelectListItem> FinacialYearList { get; set; }

        public List<SelectListItem> NurseryList { get; set; }

        public List<listInventoryData> ListInventory { get; set; }


        public DataSet FetchLoadNurseryList()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "LOAD");
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@FinacialYear", FinacialYear);
                cmd.Parameters.AddWithValue("@UpdatedDate", Date);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
                da.Dispose();
            }
        }

        public DataSet GetCurrentStock(string NurseryCode)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", NurseryCode);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "GETSTOCK");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
                da.Dispose();
            }
        }

        public DataSet GetNurseryDashboardCounts(string UserId, string SSOId = "", string FYearId="", string NurseryCode="")
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", NurseryCode);
                cmd.Parameters.AddWithValue("@FinacialYear", FYearId);
                cmd.Parameters.AddWithValue("@UserID", UserId);
                cmd.Parameters.AddWithValue("@SSOId", SSOId);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "GetNurseryDashboardCounts");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);
                da.Dispose();
            }
        }
        public int UpdateNurseryProductAPI(listInventoryData data)
        {
            DataSet dsResult = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "Update");
                cmd.Parameters.AddWithValue("@STOCKID", data.stockID);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", data.PRODUCE_QTY_Citizen);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY1", data.PRODUCE_QTY_Citizen1);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY2", data.PRODUCE_QTY_Citizen2);
                cmd.Parameters.AddWithValue("@RECEIVEDQTYPritory", data.PRODUCE_QTY_CitizenPritory);
                cmd.Parameters.AddWithValue("@RECEIVEDQTYPritory1", data.PRODUCE_QTY_CitizenPritory1);
                cmd.Parameters.AddWithValue("@RECEIVEDQTYPritory2", data.PRODUCE_QTY_CitizenPritory2);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));

                cmd.CommandType = CommandType.StoredProcedure;
                int i = cmd.ExecuteNonQuery();
                return i;
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dsResult);
                //return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public Int64 INSERT_UPDATE_LIST(DataTable DT, string StockName)
        {
            DataTable dtResult = new DataTable();
            Int64 result = 0;
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PRODUCE_FOR", "Nursery");
                cmd.Parameters.AddWithValue("@ActionFlag", "INSERT_UPDATE_LIST");
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@TypeListNursaryInventoryData", DT);
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        

        public DataSet FetchDODInventoryDetails(int actionCode = 1)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDataForinventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", actionCode);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet FetchNurseryProduces()
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WORKORDER", TPID);
                cmd.Parameters.AddWithValue("@STOCKID", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", DBNull.Value);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", DBNull.Value);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataSet FetchForestProduceDetails(string tpID, string stockID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_INVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TPID", tpID);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", DBNull.Value);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionFlag", "VIEW");
                cmd.Parameters.AddWithValue("@LOTNUMBER", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataSet FetchLotDetails(string lotID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDataForLot", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@LotID", lotID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataSet GetInventoryReceiverLog(long tpID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByTPID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 4);
                cmd.Parameters.AddWithValue("@TPID", tpID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataSet FetchItemInventoryDetails(string tpID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDataForinventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@TPID", tpID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataTable GetProductByProductType(string ProductTypeID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ProductByProduceType", Conn);
                cmd.Parameters.AddWithValue("@ProduceTypeID", ProductTypeID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataSet FetchNurseryProduceDetails(string stockID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WORKORDER", TPID);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", DBNull.Value);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionFlag", "VIEW");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public Int64 UpdateProduceStock(string stockID, double produceQty, string actionFlag)
        {
            DataTable dtResult = new DataTable();
            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_INVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TPID", DBNull.Value);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", DBNull.Value);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", produceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.Parameters.AddWithValue("@LOTNUMBER", lotNumber);
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public Int64 UpdateNurseryProduceStock(string stockID, double produceQty, double receivedDQty, string actionFlag)
        {
            DataTable dtResult = new DataTable();
            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WORKORDER", TPID);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", DBNull.Value);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", DBNull.Value);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", DBNull.Value);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", produceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY_DEPT", receivedDQty);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public Int64 AddItemsToInventory(string actionFlag)
        {
            DataTable dtResult = new DataTable();
            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_INVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TPID", TPID);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", produceFor);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", produceTypeID);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", produceID);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", produceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", recComments);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.Parameters.AddWithValue("@LOTNUMBER", lotNumber);
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public Int64 AddItemsToNursery(string actionFlag, DataTable DT)
        {
            DataTable dtResult = new DataTable();
            Int64 result = 0;
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WORKORDER", TPID);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", "Nursery");
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", produceTypeID);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", produceID);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", produceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY_DEPT", d_produceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", recComments);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.Parameters.AddWithValue("@HeadDetails", DT);
                cmd.Parameters.AddWithValue("@CitizenDiscount", CitizenDiscount);
                cmd.Parameters.AddWithValue("@NGODiscount", NGODiscount);
                cmd.Parameters.AddWithValue("@GovDiscount", GovDiscount);
                cmd.Parameters.AddWithValue("@UnitType", UnitType);

                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public Int64 UpdateItemsToNursery(string actionFlag, DataTable DT)
        {
            DataTable dtResult = new DataTable();
            Int64 result = 0;
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WORKORDER", TPID);
                cmd.Parameters.AddWithValue("@STOCKID", stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_FOR", "Nursery");
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", produceTypeID);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", produceID);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY", InventoryproduceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY_DEPT", Inventoryd_produceQty);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@RECEIVERCOMMENT", recComments);
                cmd.Parameters.AddWithValue("@ActionFlag", actionFlag);
                cmd.Parameters.AddWithValue("@HeadDetails", DT);
                cmd.Parameters.AddWithValue("@CitizenDiscount", InventoryCitizenDiscount);
                cmd.Parameters.AddWithValue("@NGODiscount", InventoryNGODiscount);
                cmd.Parameters.AddWithValue("@GovDiscount", InventoryGovDiscount);
                cmd.Parameters.AddWithValue("@UnitType", UnitType);

                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt64(cmd.ExecuteNonQuery());
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataSet getObjectsForFurseryOperation(string objectType, string objectCode, bool IsWorkOrder, string workOrder = null)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GETOBJECTSFORNURSERYOPERATION", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OBJTYPE", objectType);
                cmd.Parameters.AddWithValue("@OBJCODE", objectCode);
                cmd.Parameters.AddWithValue("@WORKORDER", workOrder);
                cmd.Parameters.AddWithValue("@IFWORKORDER", IsWorkOrder);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataTable GetHeadDetails(string ID)
        {
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_FPD_MapNurserieHeadPrice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@Stock_ID", ID);
                cmd.Parameters.AddWithValue("@userID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataSet GetDataForLot()
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDataForLot", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public DataTable GetUnitDetails(string ProductTypeID)
        {
            DataTable dtResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetUnitDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@ProductTypeID", ProductTypeID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                return dtResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataTable AddUpdateProductStock(listInventoryData model)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("ACTIONFLAG", "AddUpdateProductStock");
                cmd.Parameters.AddWithValue("@NurseryCode", model.DEPOT_NURSERY_CODE);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", model.PRODUCETYPEID);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", model.PRODUCEID);
                //cmd.Parameters.AddWithValue("@PRODUCE_TYPE", model.PRODUCETYPEID);

                cmd.Parameters.AddWithValue("@RECEIVEDQTY", model.PRODUCE_QTY_Citizen);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY_DEPT", model.PRODUCE_QTY_Department);
                //cmd.Parameters.AddWithValue(@"NurseryCode", model.DEPOT_NURSERY_CODE);
                cmd.Parameters.AddWithValue("@ProductID", model.PRODUCTID);
                cmd.Parameters.AddWithValue("@STOCKID", model.stockID);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@SSOId", model.SSOId);
                cmd.Parameters.AddWithValue("@BaseProductType", Convert.ToInt32(model.BASEPRODUCETYPE));
                cmd.Parameters.AddWithValue("@FlgActionCitizen", Convert.ToInt32(model.FlgActionCitizen));
                cmd.Parameters.AddWithValue("@FlgActionDept", Convert.ToInt32(model.FlgActionDept));
                cmd.Parameters.AddWithValue("@FinacialYear", Convert.ToInt32(model.FinancialYear));
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataTable EditProductStock(listInventoryData model)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("ACTIONFLAG", "EditNurseryProductStock");
                cmd.Parameters.AddWithValue("@NurseryCode", model.DEPOT_NURSERY_CODE);
                cmd.Parameters.AddWithValue("@STOCKID", model.stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", model.PRODUCETYPEID);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", model.PRODUCEID);
                //cmd.Parameters.AddWithValue("@PRODUCE_TYPE", model.PRODUCETYPEID);

                cmd.Parameters.AddWithValue("@RECEIVEDQTY", model.PRODUCE_QTY_Citizen);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY_DEPT", model.PRODUCE_QTY_Department);
                //cmd.Parameters.AddWithValue(@"NurseryCode", model.DEPOT_NURSERY_CODE);
                cmd.Parameters.AddWithValue("@ProductID", model.PRODUCTID);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@SSOId", model.SSOId);
                cmd.Parameters.AddWithValue("@FinacialYear", model.FinancialYear);
                cmd.Parameters.AddWithValue("@BaseProductType", Convert.ToInt32(model.BASEPRODUCETYPE));
                cmd.Parameters.AddWithValue("@FlgActionCitizen", Convert.ToInt32(model.FlgActionCitizen));
                cmd.Parameters.AddWithValue("@FlgActionDept", Convert.ToInt32(model.FlgActionDept));
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataTable EditWebProductStock(listInventoryData model)
        {
            try
            {

                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("ACTIONFLAG", "EditNurseryProductStock");
                cmd.Parameters.AddWithValue("@NurseryCode", model.DEPOT_NURSERY_CODE);
                cmd.Parameters.AddWithValue("@STOCKID", model.stockID);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", model.PRODUCETYPEID);
                cmd.Parameters.AddWithValue("@PRODUCE_ID", model.PRODUCEID);
                //cmd.Parameters.AddWithValue("@PRODUCE_TYPE", model.PRODUCETYPEID);

                cmd.Parameters.AddWithValue("@RECEIVEDQTY", model.PRODUCE_QTY_Citizen);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY1", model.PRODUCE_QTY_Citizen1);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY2", model.PRODUCE_QTY_Citizen2);
                cmd.Parameters.AddWithValue("@RECEIVEDQTY_DEPT", model.PRODUCE_QTY_Department);
                //cmd.Parameters.AddWithValue(@"NurseryCode", model.DEPOT_NURSERY_CODE);
                cmd.Parameters.AddWithValue("@ProductID", model.PRODUCTID);
                cmd.Parameters.AddWithValue("@UserId", model.UserId);
                cmd.Parameters.AddWithValue("@FinacialYear", model.FinancialYear);
                cmd.Parameters.AddWithValue("@BaseProductType", Convert.ToInt32(model.BASEPRODUCETYPE));
                cmd.Parameters.AddWithValue("@FlgActionCitizen", Convert.ToInt32(model.FlgActionCitizen));
                cmd.Parameters.AddWithValue("@FlgActionDept", Convert.ToInt32(model.FlgActionDept));
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        //public DataTable AddUpdateProductStock(NurseryStock model)
        //{
        //    try
        //    {
        //        DALConn();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        cmd.Parameters.AddWithValue("ACTIONFLAG", "AddUpdateProductStock");
        //        cmd.Parameters.AddWithValue("NurseryCode", model.NurseryCode);
        //        cmd.Parameters.AddWithValue("ProductID", model.ProductID);
        //        cmd.Parameters.AddWithValue("Stock", model.Stock);
        //        cmd.Parameters.AddWithValue("SellOut", model.SellOut);
        //        cmd.Parameters.AddWithValue("Remaining", model.Remaining);
        //        cmd.Parameters.AddWithValue("IsOldNew", model.IsOldNew);
        //        cmd.Parameters.AddWithValue("UserID", model.EnteredBy);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //}
        public DataSet FetchLoadNurseryListAPI()
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "LOAD");
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@SSOId", Convert.ToString(HttpContext.Current.Session["SSOId"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet FetchLoadNurseryListAPI(string CircleCode, string DivisionCode, string RangeCode)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "LOAD");
                cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@DivCode", DivisionCode);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@SSOId", Convert.ToString(HttpContext.Current.Session["SSOId"]));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet FetchLoadNurseryListDivisionWiseAPI(string CircelCode, string DivCode, string RangeCode)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", nurseryDepotCode);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "LOADNurseryDivisionWise");
                cmd.Parameters.AddWithValue("@CircelCode", CircelCode);
                cmd.Parameters.AddWithValue("@DivCode", DivCode);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet FetchLoadNurseryWithItemListAPI(string NurseryID, string StockName)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", NurseryID);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "LOADWithProducts");
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataTable NurseryProductsType()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@ACTIONFLAG", "ProductType");
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        // public class  MISFilter
        //{
        //	public int CategoryID { get; set; }
        //	public int ProductID { get; set; }
        //	public string DivCode { get; set; }
        //	public string RangCode { get; set; }
        //	public string CircleCode { get; set; }
        //	public string FinacialYearDate { get; set; }
        //}

        public DataTable NurseryProducts_OldNew(string OldNew, long UserID, string SSOId , string NursuryCode, int CategoryID, int ProductID, string DivCode, string RangeCode, string CircleCode, string FinacialYearDate, string PlantAge, int BaseProduceTypeId, int PRODUCE_TYPE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@ACTIONFLAG", "ProductONOldNew");
                cmd.Parameters.AddWithValue("@OldNew", OldNew);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@SSOId", SSOId);
                cmd.Parameters.AddWithValue("@NurseryCode", NursuryCode);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                cmd.Parameters.AddWithValue("@DivCode", DivCode);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
                cmd.Parameters.AddWithValue("@FinacialYearDate", FinacialYearDate);
                cmd.Parameters.AddWithValue("@PlantAge", PlantAge);
                cmd.Parameters.AddWithValue("@PRODUCE_TYPE", PRODUCE_TYPE);                
                cmd.Parameters.AddWithValue("@BaseProductType", BaseProduceTypeId);
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        #region GGAY Procedure or Functions
        
        public DataSet GetNurseryMasterGGAY(string Nursery_Code, string DIV_CODE)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetNurseryMasterGGAY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NURSERY_CODE", Nursery_Code);
                cmd.Parameters.AddWithValue("@DIV_CODE", DIV_CODE);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                cmd.Dispose();
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet GetNurseryStockGGAY(string Nursery_Code)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetNurseryStockGGAY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NURSERY_CODE", Nursery_Code);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                cmd.Dispose();
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet UpdateNurseryStockGGAY(string Nursery_Code, String SSO_ID, List<GGAYProducts> ProductOutList)
        {
            DataTable dt = new DataTable("GGAY_OutStock");
            dt.Columns.Add(new DataColumn("RowId", typeof(int)));
            dt.Columns.Add(new DataColumn("Product_NamesId", typeof(int)));
            dt.Columns.Add(new DataColumn("Product_Names", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(int)));
            int i = 1;
            foreach (var obj in ProductOutList)
            {
                DataRow row = dt.NewRow();
                row["RowId"] = i;
                row["Product_NamesId"] = obj.Product_NamesId;
                row["Product_Names"] = obj.Product_Names;
                row["Quantity"] = obj.Quantity;
                dt.Rows.Add(row);
                i++;
            }

            StringWriter sw = new StringWriter();
            dt.WriteXml(sw);
            string xmlList = sw.ToString();
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_UpdateNurseryStockGGAY", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NURSERY_CODE", Nursery_Code);
                cmd.Parameters.AddWithValue("@SSO_ID", SSO_ID);
                cmd.Parameters.AddWithValue("@xmlList", xmlList);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                cmd.Dispose();
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();

                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        #endregion
        //public DataTable NurseryProducts_OldNew(string OldNew, long UserID,string NursuryCode, int CategoryID, int ProductID, string DivCode, string RangeCode, string CircleCode, string FinacialYearDate)
        //{
        //	try
        //	{
        //		DALConn();
        //		DataTable dt = new DataTable();
        //		SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
        //		cmd.CommandType = CommandType.StoredProcedure;
        //		SqlDataAdapter da = new SqlDataAdapter(cmd);

        //		cmd.Parameters.AddWithValue("@ACTIONFLAG", "ProductONOldNew");
        //		cmd.Parameters.AddWithValue("@OldNew", OldNew);
        //		cmd.Parameters.AddWithValue("@UserID",  UserID);
        //		cmd.Parameters.AddWithValue("@NurseryCode", NursuryCode);
        //		cmd.Parameters.AddWithValue("@CategoryID",CategoryID);
        //		cmd.Parameters.AddWithValue("@ProductID", ProductID);
        //		cmd.Parameters.AddWithValue("@DivCode", DivCode);
        //		cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
        //		cmd.Parameters.AddWithValue("@CircleCode", CircleCode);
        //		cmd.Parameters.AddWithValue("@FinacialYearDate",FinacialYearDate);
        //		cmd.CommandType = CommandType.StoredProcedure;

        //		da.Fill(dt);
        //		return dt;
        //	}
        //	catch (Exception ex)
        //	{
        //		throw ex;
        //	}
        //	finally
        //	{
        //		Conn.Close();
        //	}
        //}

        //public DataTable NurseryProducts_OldNew(string OldNew, int BaseProductType, string NursuryCode)
        //{
        //    try
        //    {
        //        DALConn();
        //        DataTable dt = new DataTable();
        //        SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        cmd.Parameters.AddWithValue("@ACTIONFLAG", "ProductONOldNew");
        //        cmd.Parameters.AddWithValue("@OldNew", OldNew);
        //        cmd.Parameters.AddWithValue("@NurseryCode", NursuryCode);
        //        cmd.Parameters.AddWithValue("@BaseProductType", BaseProductType);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //}

        public DataTable NurseryProducts_OldNewProductList(string OldNew, int BaseProductType, string NursuryCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@ACTIONFLAG", "GETProductList");
                cmd.Parameters.AddWithValue("@OldNew", OldNew);
                cmd.Parameters.AddWithValue("@NurseryCode", NursuryCode);
                cmd.Parameters.AddWithValue("@BaseProductType", BaseProductType);
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet GetCurrentStockAPI(string NurseryCode)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", NurseryCode);
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "GETSTOCK");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        
        public DataSet UpdateNurseryProductAPI(DataTable DT, long userID, string StockName)
        {
            DataSet dsResult = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPD_UPDATE_NURSERYINVENTORY_API", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTIONFLAG", "Update");
                cmd.Parameters.AddWithValue("@StockName", StockName);
                cmd.Parameters.AddWithValue("@RECEIVEDBY", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@TypeListNursaryInventoryData", DT);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        //public Entity.ResponseMsg UpdateLot(LotMaster lotMaster, string actionName)
        //{ 
        //    Entity.ResponseMsg msg = new Entity.ResponseMsg(); 
        //    DataTable dtResult = new DataTable();
        //    try
        //    {
        //        Int16 actionCode = 0;
        //        if (actionName == "SaveLot")
        //        {
        //            actionCode = 1;
        //        }
        //        else
        //        {
        //            actionCode = 2;
        //        }
        //            DALConn(); 
        //        SqlCommand cmd = new SqlCommand("SP_DOD_Insert_LotMaster", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ActionCode", actionCode);
        //        cmd.Parameters.AddWithValue("@DepotID", lotMaster.DepotID);
        //        cmd.Parameters.AddWithValue("@LotID", lotMaster.LotID);
        //        cmd.Parameters.AddWithValue("@ProductID", lotMaster.ProductID);
        //        cmd.Parameters.AddWithValue("@MaxQty", lotMaster.LotQty); 
        //        cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dtResult);
        //        msg = FMDSS.Globals.Util.GetListFromTable<Entity.ResponseMsg>(dtResult).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return msg;
        //}

        //public Entity.ResponseMsg DeleteLot(long lotID)
        //{
        //    Entity.ResponseMsg msg = new Entity.ResponseMsg();
        //    DataTable dtResult = new DataTable();
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("SP_DOD_Delete_LotMaster", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure; 
        //        cmd.Parameters.AddWithValue("@lotID", lotID); 
        //        cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dtResult);
        //        msg = FMDSS.Globals.Util.GetListFromTable<Entity.ResponseMsg>(dtResult).FirstOrDefault();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    return msg;
        //}

        public Entity.ResponseMsg UpdateDODInventory(DODItemAddedToInventory model, string actionName)
        {
            Entity.ResponseMsg msg = new Entity.ResponseMsg();
            DataTable dtResult = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_DOD_Insert_Depot_ForestProduceInventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@TPID", model.TPID);
                cmd.Parameters.AddWithValue("@Comment", model.Comment);
                cmd.Parameters.AddWithValue("@xmlFile", GetRequestInXML(model));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                msg = FMDSS.Globals.Util.GetListFromTable<Entity.ResponseMsg>(dtResult).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
            return msg;
        }

        public Entity.ResponseMsg UpdateWriteOff(InventoryWriteOff model)
        {
            Entity.ResponseMsg msg = new Entity.ResponseMsg();
            DataTable dtResult = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_DOD_Insert_Inventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@ParentID", model.InventoryID);
                cmd.Parameters.AddWithValue("@WriteOffQty", model.WriteOffQty);
                cmd.Parameters.AddWithValue("@Comment", model.Comment);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                msg = FMDSS.Globals.Util.GetListFromTable<Entity.ResponseMsg>(dtResult).FirstOrDefault();
            }
            catch (Exception e)
            {
                throw e;
            }
            return msg;
        }

        public DataSet UpdateInventory(DODInventory model, int actionCode = 0)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_DOD_Insert_Inventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", actionCode);
                cmd.Parameters.AddWithValue("@ParentID", model.InventoryID);
                cmd.Parameters.AddWithValue("@OldQty", model.OldQty);
                cmd.Parameters.AddWithValue("@Qty", model.Qty);
                cmd.Parameters.AddWithValue("@Comment", model.ReceiverComment);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
            return dsResult;
        }

        public DataSet DeActivateInventory(string parentID)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_DOD_Insert_Inventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 3);
                cmd.Parameters.AddWithValue("@ParentID", parentID);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
            return dsResult;
        }

        public string ReleaseAllProduct()
        {
            string message = string.Empty;
            DataTable dsResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Scheduling_NurseryPurchaseDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);

                message = Convert.ToString(dsResult.Rows[0]["status"]);
                return message;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }

        public class LotNumbers
        {
            public int lotId { get; set; }
            public string lotNumber { get; set; }
            public string lotQuantity { get; set; }
        }
        public class NurseryStock
        {
            public string NurseryCode { get; set; }
            public long ProductID { get; set; }
            public long Stock { get; set; }
            public long SellOut { get; set; }
            public long Remaining { get; set; }
            public string IsOldNew { get; set; }
            public long EnteredBy { get; set; }
        }
        private string GetRequestInXML(DODItemAddedToInventory model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tpRequest>");
            if (model.TPProductList != null && model.TPProductList.Count > 0)
            {
                sb.Append("<products>");
                foreach (var item in model.TPProductList)
                {
                    sb.Append("<product>");
                    sb.Append(string.Format(@"
                            <TargetInventoryID>{0}</TargetInventoryID>
                            <ProductID>{1}</ProductID>
                            <TransferQty>{2}</TransferQty>
                            <GoodQty>{3}</GoodQty>
                            <DamagedQty>{4}</DamagedQty>", item.SecondaryObjectID, item.ProductID, item.TransferQty, item.GoodQty, item.DamagedQty));
                    sb.Append("</product>");
                }
                sb.Append("</products>");
            }
            sb.Append("</tpRequest>");
            return Convert.ToString(sb);
        }
        public DataSet NurseryStockList(long UserId, int Year, string ActionName)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spNurseryStockYearWise", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                cmd.Parameters.AddWithValue("@Year", Year);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public DataSet WaterResourceList(long UserId, string ActionName)
        {
            DataSet dsResult = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("spWaterResourceCount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@UserId", UserId);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }



    }
    public class Nurserylist : DAL
    {
        public string UserId { get; set; }
        public string NURSERY_CODE { get; set; }
        public string NURSERY_NAME { get; set; }
        public string NURSERY_ADRESS { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string NURSERY_IMAGE { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string NURSERY_LANDMARK { get; set; }
        public string NURSERY_PINCODE { get; set; }
        internal DataSet FetchNurseryListAPI(string nURSERY_CODE)
        {
            DataSet dsResult = new DataSet();
            try
            {
                string flag = "select";
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetNurseryDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", flag);
                cmd.Parameters.AddWithValue("@NURSERY_CODE", nURSERY_CODE);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }



        //internal DataSet UpdateNurseryListAPI(Nurserylist obj1)
        //{
        //    DataSet dsResult = new DataSet();
        //    try
        //    {

        //        DALConn();

        //        var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/SiteImage/");
        //        string str = SaveImage(Convert.ToString(obj1.NURSERY_IMAGE), path);
        //        SqlCommand cmd = new SqlCommand("sp_GetNurseryDetails", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Flag", "Update");
        //        cmd.Parameters.AddWithValue("@NURSERY_CODE", obj1.NURSERY_CODE);
        //        cmd.Parameters.AddWithValue("@NURSERY_NAME", obj1.NURSERY_NAME);
        //        cmd.Parameters.AddWithValue("@NURSERY_ADRESS", obj1.NURSERY_ADRESS);
        //        cmd.Parameters.AddWithValue("@LATITUDE", Convert.ToDecimal(obj1.LATITUDE));
        //        cmd.Parameters.AddWithValue("@LONGITUDE", Convert.ToDecimal(obj1.LONGITUDE));
        //        cmd.Parameters.AddWithValue("@NURSERY_IMAGE", str);
        //        cmd.Parameters.AddWithValue("@Name", obj1.Name);
        //        cmd.Parameters.AddWithValue("@Mobile", obj1.Mobile);
        //        cmd.Parameters.AddWithValue("@UserId", obj1.UserId);
        //        cmd.Parameters.AddWithValue("@NURSERY_LANDMARK", obj1.NURSERY_LANDMARK);
        //        cmd.Parameters.AddWithValue("@path", path);

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dsResult);
        //        return dsResult;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        if (Conn.State == ConnectionState.Open)
        //            Conn.Close();
        //        Conn.Dispose();
        //        SqlConnection.ClearPool(Conn);

        //    }
        //}
        internal DataSet UpdateNurseryListAPI(Nurserylist obj1, string host)
        {
            DataSet dsResult = new DataSet();
            try
            {

                DALConn();

                var path1 = host+"Content/Images/SiteImage/";
                // "http://103.203.138.101/fmdssnewtest/Content/Images/SiteImage/";
                var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/SiteImage/");
               

                SqlCommand cmd = new SqlCommand("sp_GetNurseryDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "Update");
                cmd.Parameters.AddWithValue("@NURSERY_CODE", obj1.NURSERY_CODE);
                cmd.Parameters.AddWithValue("@NURSERY_NAME", obj1.NURSERY_NAME);
                cmd.Parameters.AddWithValue("@NURSERY_ADRESS", obj1.NURSERY_ADRESS);
                cmd.Parameters.AddWithValue("@LATITUDE", Convert.ToDecimal(obj1.LATITUDE));
                cmd.Parameters.AddWithValue("@LONGITUDE", Convert.ToDecimal(obj1.LONGITUDE));
                if (obj1.NURSERY_IMAGE != "")
                {
                    string str = SaveImage(Convert.ToString(obj1.NURSERY_IMAGE), path);
                    cmd.Parameters.AddWithValue("@NURSERY_IMAGE", str);
                }
                //cmd.Parameters.AddWithValue("@NURSERY_IMAGE", str);
                cmd.Parameters.AddWithValue("@Name", obj1.Name);
                cmd.Parameters.AddWithValue("@Mobile", obj1.Mobile);
                cmd.Parameters.AddWithValue("@UserId", obj1.UserId);
                cmd.Parameters.AddWithValue("@NURSERY_LANDMARK", obj1.NURSERY_LANDMARK);
                cmd.Parameters.AddWithValue("@NURSERY_PINCODE", obj1.NURSERY_PINCODE);
                cmd.Parameters.AddWithValue("@path", path1);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsResult);
                return dsResult;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
                Conn.Dispose();
                SqlConnection.ClearPool(Conn);

            }
        }
        public string SaveImage(string base64ShopImg, string filePath)
        {
            string retVal = "";
            string fileName = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + ".png";

            try
            {
                // string fileName = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + ".png";
                if (base64ShopImg.Length > 0)
                {
                    string base64 = base64ShopImg.Split(',')[1];
                    byte[] bytes = Convert.FromBase64String(base64);
                    filePath = Path.Combine(filePath, fileName);
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(bytes)))
                    {
                        image.Save(filePath, ImageFormat.Png);
                    }
                    // System.Configuration.ConfigurationManager.AppSettings["SaveImagePath"].ToString();
                    filePath = filePath + fileName;
                    retVal = filePath;

                }
                else
                {
                    retVal = "";
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return fileName;
        }


    }
    //public class LotMaster
    //{
    //    public long? SNo { get; set; }
    //    public long? DepotID { get; set; }
    //    public string Depot_Name { get; set; }
    //    public long? LotID { get; set; }
    //    public int? CurrentLotNumber { get; set; }
    //    public decimal? LotQty { get; set; }
    //    public Int64 ProductID { get; set; }
    //    public string ProductName { get; set; }
    //    public string ProductType { get; set; }
    //    public string UnitName { get; set; }
    //    public string DisplayLotNumber { get; set; }
    //}



    public class DODInventory
    {
        public long? SNo { get; set; }
        public long? InventoryID { get; set; }
        public string Depot_Name { get; set; }
        public decimal? Qty { get; set; }
        public decimal? OldQty { get; set; }
        public decimal? TransferQty { get; set; }
        public decimal? NoticeReservedQty { get; set; }
        public decimal? WriteOffQty { get; set; }
        public decimal? AvailableQty { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string UnitName { get; set; }
        public string DisplayLotNumber { get; set; }
        public long? TPID { get; set; }
        public string InventoryStatus { get; set; }
        public string ReceiverComment { get; set; }
    }

    public class DODItemAddedToInventory
    {
        public long? SNo { get; set; }
        public long? TPID { get; set; }
        public string TransitPermitName { get; set; }
        public long? DepotID { get; set; }
        public string Depot_Name { get; set; }
        public int? InventoryStatus { get; set; }
        public bool? IsActive { get; set; }
        public string Comment { get; set; }
        public virtual List<DODTPProductDetails> TPProductList { get; set; }
    }

    public class DODTPProductDetails
    {
        public string ID { get; set; }
        public string TPID { get; set; }
        public string SecondaryObjectID { get; set; }
        public string ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal TransferQty { get; set; }
        public decimal GoodQty { get; set; }
        public decimal DamagedQty { get; set; }
        public decimal LostQty { get; set; }
    }

    public class InventoryReceiverLog
    {
        public long? SNo { get; set; }
        public string Depot_Name { get; set; }
        public string ProductName { get; set; }
        public decimal? ReceivedQty { get; set; }
        public string ReceiverComment { get; set; }
        public string UpdatedBy { get; set; }
        public string InventoryStatus { get; set; }
        public string DisplayLotNumber { get; set; }
    }

    public class InventoryLotDetails
    {
        public Int64 ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public string UnitName { get; set; }
        public string RatePerUnit { get; set; }
        public decimal? AvailableQty { get; set; }
    }

    public class listInventoryData
    {
        public long UserId { get; set; }
        public string SSOId { get; set; }
        public string StockName { get; set; }
        public Int64 stockID { get; set; }

        public string DEPOT_NURSERY_CODE { get; set; }
        public string ProductFullImage { get; set; }

        public string ProductThumbImage { get; set; }
        public string PRODUCETYPEID { get; set; }
        public string PRODUCETYPE { get; set; }
        public Int64 PRODUCEID { get; set; }
        public Int64 PRODUCTID { get; set; }
        public string BASEPRODUCETYPE { get; set; }
        public int BASEPRODUCETYPE_ID { get; set; }
        public string PRODUCTNAME { get; set; }
        public string PRODUCTCATEGORY { get; set; }
        public string UNITNAME { get; set; }
        public string PRICE { get; set; }
        public Int64 PRODUCE_QTY_Citizen { get; set; }//Head Is 8235-200-06-RFBP(NP)
        public Int64 PRODUCE_QTY_Citizen1 { get; set; }//Head Is0406-01-80-05
        public Int64 PRODUCE_QTY_Citizen2 { get; set; }//Head Campa
        public int PRODUCE_QTY_CitizenPritory { get; set; }//Head Is 8235-200-06-RFBP(NP)
        public int PRODUCE_QTY_CitizenPritory1 { get; set; }//Head Is0406-01-80-05
        public int PRODUCE_QTY_CitizenPritory2 { get; set; }//Head Campa
        public int PRODUCE_HeadPrimaryStatus { get; set; }
        public Int64 PRODUCE_QTY_Department { get; set; }
        public string DiscountForCitizen { get; set; }
        public string DiscountForGovt { get; set; }
        public string DiscountForNGO { get; set; }
        public bool IsDiscountApplicable { get; set; }
        public bool IsActive { get; set; }
        public string FinancialYear { get; set; }
        public string TransDate { get; set; }
        public string PlantAgeStr { get; set; }
        public int FlgActionCitizen { get; set; }//0 add stock,1 out stock,3  Edit qty
        public int FlgActionDept { get; set; }//0 add stock,1 out stock,3  Edit qty       
        public long Citizen_StockOut { get; set; }
        public long Dept_StockOut { get; set; }
    }
    public class MapNurserieHeadPrice
    {
        public int NurseriesHeadID { get; set; }
        public string NurserieHeadName { get; set; }
        public decimal Price { get; set; }

    }

    public class InventoryWriteOff
    {
        public long? InventoryID { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal WriteOffQty { get; set; }
        public string Comment { get; set; }
    }
    public class GGAYProducts
    {
        public int Product_NamesId { get; set; } // tbl_mst_FDM_ProductNames id belongs to Product_NamesId
        public string Product_Names { get; set; }//  tbl_mst_FDM_ProductNames Product_Names belongs to CommanEngName
        public int Quantity { get; set; }
    }
    public class GGAY_Details
    {
        public string Nursery_Code { get; set; }
        public string SSO_ID { get; set; }
        public List<GGAYProducts> ProductOutList { get; set; }
    }
}