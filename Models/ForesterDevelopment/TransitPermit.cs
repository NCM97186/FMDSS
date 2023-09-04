using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FMDSS.Entity;
using System.Web.Mvc;

namespace FMDSS.Models.ForestDevelopment
{
    public class TransitPermit : DAL
    {

        #region Class Property
        public Int64 UserID { get; set; }
        public long ID { get; set; }

        public string TransitPermitName { get; set; }

        public string ExchangeMode { get; set; }
        public string ddlDistrictfrom { get; set; }
        public string ddlBlockNamefrom { get; set; }
        public string ddlGPNamefrom { get; set; }
        public string FromVillage_Code { get; set; }
        public string RegionCodeFrom { get; set; }
        public string CircleCodeFrom { get; set; }
        public string DivisionCodeFrom { get; set; }
        public string RangeCodeFrom { get; set; }
        public Int64 DepotCodeFrom { get; set; }
        public string ToLocationType { get; set; }
        public string depotInchargefrom { get; set; }
        public string ddlDistrictto { get; set; }
        public string ddlBlockNameto { get; set; }
        public string ddlGPNameto { get; set; }
        public string ToVillage_Code { get; set; }
        public string ToDepot_NurseryCode { get; set; }
        public string ToRegionCode { get; set; }
        public string ToCircleCode { get; set; }
        public string ToDivisionCode { get; set; }
        public string ToRangeCode { get; set; }

        public Int64 ToDepotCode { get; set; }
        public Int64 WorkOrderQty { get; set; }
        public Int64 WorkOrder { get; set; }
        public string FromGPS_Long { get; set; }

        public Int64 ProduceType { get; set; }
        public string ProduceUnit { get; set; }
        public Int64 ProductID { get; set; }

        public string ProductType { get; set; }
        public string Product { get; set; }
        public string Qty { get; set; }
        public decimal TransferQTY { get; set; }
        public decimal ReceivedQty { get; set; }
        public decimal RemainingQty { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal LostQty { get; set; }
        public decimal TransferQtyToOtherDepo { get; set; }

        public DateTime Receivedon { get; set; }

        public long ReceivedBy { get; set; }

        public string ReceiverComment { get; set; }

        public string ModeofTransport { get; set; }

        public string VehicleNumber { get; set; }

        public string Driver_License_No { get; set; }

        public string OtherSiteName { get; set; }

        public string Driver_Name { get; set; }
        [RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "PhoneNumber should contain only numbers")]
        public string Driver_MobNo { get; set; }

        public string Permit_ValidUpto { get; set; }

        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "Ammount is not in correct format")]
        public decimal Amount_ToBePaid { get; set; }

        public string EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public string UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }
        public int Status { get; set; }

        public string DIST_NAME { get; set; }
        public string FromVillage { get; set; }
        public string ToVillage { get; set; }
        public string ProductName { get; set; }
        public string RowID { get; set; }
        public string InventoryID { get; set; }
        public string Comment { get; set; }
        //public string InventoryLotNumber { get; set; }
        public virtual List<DODProductDetails> DODProductList { get; set; }
        #endregion Class Property

        #region Public Methods
        public ResponseMsg SubmitTransitPermit(TransitPermit model)
        {
            UserID = Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString());
            if (model.ExchangeMode == "1" || model.ExchangeMode == "3")
                model.ExchangeMode = "Depot";
            else
                model.ExchangeMode = "Nursery";

            if (model.ExchangeMode != "Nursery")
                model.ToDepot_NurseryCode = Convert.ToString(model.ToDepotCode);

            ResponseMsg msg = new ResponseMsg();
            DataTable dtResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_Insert_TransitPermit", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ActionCode", 1);
                cmd.Parameters.AddWithValue("ID", model.ID);
                cmd.Parameters.AddWithValue("ToLocationType", model.ExchangeMode);
                cmd.Parameters.AddWithValue("FromVillage_Code", model.FromVillage_Code == null ? (object)DBNull.Value : model.FromVillage_Code);
                cmd.Parameters.AddWithValue("RangeCodeFrom", model.RangeCodeFrom == null ? (object)DBNull.Value : model.RangeCodeFrom);
                cmd.Parameters.AddWithValue("DepotCodeFrom", model.DepotCodeFrom == null ? (object)DBNull.Value : model.DepotCodeFrom);
                cmd.Parameters.AddWithValue("ToVillage_Code", model.ToVillage_Code == null ? (object)DBNull.Value : model.ToVillage_Code);
                cmd.Parameters.AddWithValue("ToDepot_NurseryCode", model.ToDepot_NurseryCode == null ? (object)DBNull.Value : model.ToDepot_NurseryCode);
                cmd.Parameters.AddWithValue("ToRangeCode", model.ToRangeCode == null ? (object)DBNull.Value : model.ToRangeCode);
                cmd.Parameters.AddWithValue("ToDepotCode", model.ToDepotCode == null ? (object)DBNull.Value : model.ToDepotCode);
                cmd.Parameters.AddWithValue("ModeofTransport", model.ModeofTransport);
                cmd.Parameters.AddWithValue("VehicleNumber", model.VehicleNumber);
                cmd.Parameters.AddWithValue("Driver_License_No", model.Driver_License_No);
                cmd.Parameters.AddWithValue("Driver_Name", model.Driver_Name);
                cmd.Parameters.AddWithValue("Driver_MobNo", model.Driver_MobNo);
                cmd.Parameters.AddWithValue("Permit_ValidUpto", DateTime.ParseExact(model.Permit_ValidUpto, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("Amount_ToBePaid", model.Amount_ToBePaid);
                cmd.Parameters.AddWithValue("InventoryID", model.InventoryID);
                cmd.Parameters.AddWithValue("OtherSiteName", model.OtherSiteName);
                cmd.Parameters.AddWithValue("Status", 1);
                cmd.Parameters.AddWithValue("Comment", model.Comment);
                cmd.Parameters.AddWithValue("xmlFile", GetRequestInXML(model));
                cmd.Parameters.AddWithValue("UserID", UserID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                msg = FMDSS.Globals.Util.GetListFromTable<Entity.ResponseMsg>(dtResult).FirstOrDefault();
            }
            catch (Exception ex)
            {
                msg.IsError = true;
                msg.ReturnMsg = ex.Message;
                new Common().ErrorLog(ex.Message, "SubmitTransitPermit" + "_" + "SaveModelData", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return msg;
        }

        public DataTable BindTransitData(Int64 transID, string action)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Select_TransitDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_transID", transID);
                cmd.Parameters.AddWithValue("@P_ActionFlag", action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable Select_TransitPermit()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByDepotID", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public Int64 DeleteTransitPermit()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_delete_TransitPermit", Conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
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

        public DataTable Select_WorkOrder(string villCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Select_WorkOrder", Conn);
                cmd.Parameters.AddWithValue("@VillCode", villCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable Select_ProductDetail(Int64 workOrderID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FPD_Select_ProductByworkorderID", Conn);
                cmd.Parameters.AddWithValue("@workOrderID", workOrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataTable Select_DepotInchargeByDepotID(Int64 DepotID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FPM_Select_DepotIncharge", Conn);
                cmd.Parameters.AddWithValue("@DepotID", DepotID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public DataSet GetDepotDetails(Int64 DepotID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByDepotID", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@DepotID", DepotID);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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

        public DataSet GetReceivedQtyByTP(Int64 tpID)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByTPID", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@TPID", tpID);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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

        public DataTable GetDetailsByInventory(string parentID, int actionCode = 1)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_DOD_GetDetailsByInventory", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", actionCode);
                cmd.Parameters.AddWithValue("@ParentID", parentID);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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

        public ResponseMsg OpeningBalance_Save(OpeningBalance model)
        {
            ResponseMsg msg = new ResponseMsg();
            DataTable dtResult = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DOD_OpeningBalance_Save", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ActionCode", 1);
                cmd.Parameters.AddWithValue("RangeCode", model.RangeCode);
                cmd.Parameters.AddWithValue("DepotID", model.DepotID);
                cmd.Parameters.AddWithValue("Comment", model.Comment);
                cmd.Parameters.AddWithValue("UserID", HttpContext.Current.Session["UserId"]);
                cmd.Parameters.AddWithValue("xmlFile", GetRequestInXML(model));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                msg = FMDSS.Globals.Util.GetListFromTable<Entity.ResponseMsg>(dtResult).FirstOrDefault();
            }
            catch (Exception ex)
            {
                msg.IsError = true;
                msg.ReturnMsg = ex.Message;
                new Common().ErrorLog(ex.Message, "OpeningBalance_Save" + "_" + "SaveModelData", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return msg;
        }

        public EnumerableRowCollection<SelectListItem> SetDropdownData(int actionCode, string parentID, string childID = "")
        {
            DataTable dtDropdownData = GetDropdownData(actionCode, parentID, childID);
            EnumerableRowCollection<SelectListItem> data = null;

            switch (actionCode)
            {
                case 1://DivList
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("DIV_CODE"),
                        Text = x.Field<string>("DIV_NAME")
                    });
                    break;
                case 2://Tranport list
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<int>("TransportModeID")),
                        Text = x.Field<string>("Name")
                    });
                    break;
                case 3://Range list by div
                case 6://Range by depot Incharge
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("RANGE_CODE"),
                        Text = x.Field<string>("RANGE_NAME")
                    });
                    break;
                case 4:// "From Depot" options for depot to depot
                case 7:// Get Depot for Notice by Inventory availability

                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<Int64>("Depot_Id")),
                        Text = x.Field<string>("Depot_Name")
                    });
                    break;
                case 5:// GetVillageNamebyRange 
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("VILL_CODE"),
                        Text = x.Field<string>("VILL_NAME")
                    });
                    break;
                case 10:// Get Lot Number for Site To Depot for existing lot 
                case 12:// Get Lot Number for Site To Depot for existing lot 
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<Int64>("InventoryID")),
                        Text = x.Field<string>("DisplayLotNumber")
                    });
                    break;
                case 11:// Get Notice Product 
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = Convert.ToString(x.Field<Int64>("ProductID")),
                        Text = x.Field<string>("ProductName")
                    });
                    break;
            }
            return data;
        }
        #endregion

        #region Public Methods
        public string GetRequestInXML(dynamic model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tpRequest>");
            if (model.DODProductList != null && model.DODProductList.Count > 0)
            {
                sb.Append("<products>");
                foreach (var item in model.DODProductList)
                {
                    sb.Append("<product>");
                    sb.Append(string.Format(@"
                            <ID>{0}</ID>
                            <InventoryID>{1}</InventoryID>
                            <ProductID>{2}</ProductID>
                            <Qty>{3}</Qty>
                            <TargetInventoryID>{4}</TargetInventoryID>
                            <NoOfLot>{5}</NoOfLot>
                            <BiddingAmount>{6}</BiddingAmount>
                            ", item.ID, item.InventoryID, item.ProductID, item.Qty, item.TargetInventoryID, item.NoOfLot, item.BiddingAmount));
                    sb.Append("</product>");
                }
                sb.Append("</products>");
            }
            sb.Append("</tpRequest>");
            return Convert.ToString(sb);
        }
        #endregion

        #region Private Methods
        private DataTable GetDropdownData(int actionCode, string parentID, string childID = "")
        {
            DataTable dtDropdownData = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_DOD_GetDropdownData", Conn);
                cmd.Parameters.AddWithValue("ActionCode", actionCode);
                cmd.Parameters.AddWithValue("ParentID", parentID);
                cmd.Parameters.AddWithValue("ChildID", childID);
                cmd.Parameters.AddWithValue("UserID", HttpContext.Current.Session["UserId"]);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtDropdownData);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetDropdownData" + "_" + "Sp_DOD_GetDropdownData", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtDropdownData;
        }

        #endregion
    }

    #region [OpeningBalance]
    public class OpeningBalance
    {
        [Required]
        public string RangeCode { get; set; }
        [Required]
        public string DepotID { get; set; }
        [Display(Name = "Validity of Transit Permit")]
        public string PermitValidUpto { get; set; }
        [Required]
        public string Comment { get; set; }
        public virtual List<DODProductDetails> DODProductList { get; set; }
    }

    public class DODProductDetails
    {
        public string SNo { get; set; }
        public string ID { get; set; }
        public string InventoryID { get; set; }
        public string TargetInventoryID { get; set; }
        public string TPID { get; set; }
        public string ProductTypeID { get; set; }
        public string ProductType { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal NoOfLot { get; set; }
        public decimal Qty { get; set; }
        public decimal AvailableQty { get; set; }
        public string DisplayLotNumber { get; set; }
        public string AuctionWinnerID { get; set; }
        public string WinnerName { get; set; }
        public string AuctionStatus { get; set; }
        public decimal? BiddingAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PendingAmount { get; set; }
        public bool IsWinner { get; set; }
        public bool IsSelected { get; set; }
        public string EmitraHeadCode { get; set; }
    }
    #endregion

    #region DOD Report
    public class DODProductDetailsForReport
    {
        public string SNo { get; set; }
        public string Depot_Name { get; set; }
        public long? InventoryID { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public decimal Qty { get; set; }
        public decimal NoticeReservedQty { get; set; }
        public decimal WriteOffQty { get; set; }
        public decimal AvailableQty { get; set; }
        public string DisplayLotNumber { get; set; }
        public string AddedOn { get; set; }
    }
    public class DODNoticeDetailsForReport
    {
        public string SNo { get; set; }
        public string Depot_Name { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string Notice_Number { get; set; }
        public string Notice_Status { get; set; }
        public string ApproverStatus { get; set; }
        public string DurationTo { get; set; }
    }
    public class DODAuctionWinnerDetailsForReport
    {
        public string SNo { get; set; }
        public long? InventoryID { get; set; }
        public string Depot_Name { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string Notice_Number { get; set; }
        public string DisplayLotNumber { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string UnitName { get; set; }
        public string WinnerName { get; set; }
        public string DurationTo { get; set; }
        public string BiddingAmount { get; set; }
        public string Emitra_Amount { get; set; }
        public string PaidAmt { get; set; }
        public string TotalPaidAmt { get; set; }
        public string PendingAmount { get; set; }
        public string Qty { get; set; }
    }
    public class AuctionTransactionForReport
    {
        public string SNo { get; set; }
        public string Depot_Name { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_NAME { get; set; }
        public string RequestedId { get; set; }
        public string Notice_Number { get; set; }
        public string BidClosingDate { get; set; }
        public string EMD_Amount { get; set; }
        public string BiddingAmount { get; set; }
        public string Emitra_Amount { get; set; }
        public string PaidAmt { get; set; }
        public string TotalPaidAmt { get; set; }
        public string PendingAmount { get; set; }
        public string ApplicantName { get; set; }
        public string Comments { get; set; }
        public string TransactionStatus { get; set; }
        public string RequestedOn { get; set; }
    }
    #endregion
}