
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace FMDSS.Models.MIS
{
    public class MISPurchaseHistory : DAL
    {
        public int Index { get; set; }
        public string OrderNo { get; set; }
        public string NurseryName { get; set; }
        public string ProduceType { get; set; }
        public string ProductName { get; set; }
        public string PurchaseQuantity { get; set; }
        public string RatePerItem { get; set; }
        public string PaidAmount { get; set; }
        public int Citizen_StockTotal { get; set; }
        public int Dept_StockTotal { get; set; }
        public int Citizen_StockOut { get; set; }
        public int Dept_StockOut { get; set; }
        public int TotalStock { get; set; }
        public int RemaingStock { get; set; }
        public string Ssoid { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string HIERARCHY_CODE { get; set; }
        public string Discount { get; set; }


        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public string CircleStatus { get; set; }
        public string DivisionStatus { get; set; }
        public string RangeStatus { get; set; }
        public string SaleDistributedStatus { get; set; }
        
        public List<SelectListItem> HIERARCHY_LEVEL_CODE { get; set; }


        public DataTable GET_NURSERY_INVENTORY()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_OnlinePurchaseHistory", Conn);
                cmd.Parameters.AddWithValue("@FLAG", "GETLIST");
                cmd.Parameters.AddWithValue("@userid", "");
                cmd.Parameters.AddWithValue("@FROM_DATE", FROM_DATE);
                cmd.Parameters.AddWithValue("@TO_DATE", TO_DATE);
                cmd.Parameters.AddWithValue("@RANGE_CODE", NurseryName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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


        public DataTable GET_NURSERY_INVENTORYCitizen(long UserID, char IsInchargeOrAsCitizenUser)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_OnlinePurchaseHistory", Conn);
                cmd.Parameters.AddWithValue("@FLAG", "HISTORTYCITIZEN");
                cmd.Parameters.AddWithValue("@userid", UserID);
                cmd.Parameters.AddWithValue("@FROM_DATE", FROM_DATE);
                cmd.Parameters.AddWithValue("@TO_DATE", TO_DATE);
                cmd.Parameters.AddWithValue("@RANGE_CODE", NurseryName);
                cmd.Parameters.AddWithValue("@IsInchargeOrAsCitizenUser", IsInchargeOrAsCitizenUser);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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

        public DataTable Select_NURSERYSBYRANGE(string RANGE_CODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_OnlinePurchaseHistory", Conn);
                cmd.Parameters.AddWithValue("@FLAG", "GETNURSERY");
                cmd.Parameters.AddWithValue("@RANGE_CODE", RANGE_CODE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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

        public DataSet GET_USER_LEVEL(Int64 UserID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_GET_USER_LEVEL", Conn);
                cmd.Parameters.AddWithValue("@FLAG", "LOAD");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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



        public DataTable GET_NURSERY_Stock()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_NurseryInventoryDetails", Conn);
                cmd.Parameters.AddWithValue("@FLAG", "GETLIST");
                cmd.Parameters.AddWithValue("@FROM_DATE", FROM_DATE);
                cmd.Parameters.AddWithValue("@TO_DATE", TO_DATE);
                cmd.Parameters.AddWithValue("@RANGE_CODE", NurseryName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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
    }

    public class MISNursueryDetailsModel
    {
        public MISNursueryDetailsModel()
        {
            NurseryInfo = new List<MisNurseryModelDetails>();
            OrderInfo = new List<MisNurseryModelDetails>();
            Model = new MisNurseryModelDetails();
        }

        public List<MisNurseryModelDetails> NurseryInfo { get; set; }
        public List<MisNurseryModelDetails> OrderInfo { get; set; }
        public MisNurseryModelDetails Model { get; set; }
    }

    public class MisNurseryModelDetails
    {
        public string NursuryType { get; set; }
        public string DEPOT_NURSERY_CODE { get; set; }
        public string NurseryName { get; set; }
        public string ProduceType { get; set; }
        public string ProductName { get; set; }
        public int PurchaseQuantity { get; set; }
        public decimal RatePerItem { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
        public string Ssoid { get; set; }
        public string ROWID { get; set; }
        public string NURSERY_CODE { get; set; }

        public string IsInchargeOrAsCitizenUser { get; set; }
        public string OrderNo { get; set; }
        public string EnteredOn { get; set; }
        public string RANGE_CODE { get; set; }
        public string NurseryInchargeSSOID { get; set; }
        public long UserID { get; set; }

        public long CitizenQty { get; set; }
        public long DeptQty { get; set; }
    }


    public class MISNursueryModel
    {

        public string ProductName { get; set; }
        public string ProductNameWithType { get; set; }
        public long CitizenQTY { get; set; }
        public long DeptQTY { get; set; }
        public string NursuryType { get; set; }
        public long Total { get; set; }
        public long UserID { get; set; }

        public string Dist_Name { get; set; }
        public string FROM_DATE { get; set; }
        public string TO_DATE { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }
        public string NurseryName { get; set; }

        public string StockName { get; set; }

        public int Status { get; set; }

        public int INDEXS { get; set; }
        public string MakePlantInNursery { get; set; }

    }
    public class MISNursueryRepo : DAL
    {


        public List<MISNursueryModel> GetNursuryReport1(MISNursueryModel model, string Action)
        {
            List<MISNursueryModel> List = new List<MISNursueryModel>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetNurseriesReport1", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.Parameters.AddWithValue("@NurserisId", model.NurseryName);
                cmd.Parameters.AddWithValue("@RangeCode", model.Range);
                cmd.Parameters.AddWithValue("@NURSERY_TYPE", model.NursuryType);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MISNursueryModel>>(str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return List;
        }

        public List<SelectListItem> GetStockNameList( string Action,long USERID)
        {
            List<SelectListItem> List = new List<SelectListItem>();
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetNurseryInventoryLogReport", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", USERID);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", string.Empty);
                cmd.Parameters.AddWithValue("@Status", 0);
                cmd.Parameters.AddWithValue("@FromDate", string.Empty);
                cmd.Parameters.AddWithValue("@ToDate", string.Empty);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                foreach (System.Data.DataRow dr in dt.Tables[0].Rows)
                {
                    List.Add(new SelectListItem { Text = @dr["StockName"].ToString(), Value = @dr["StockName"].ToString() });
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return List;
        }

        public string  GetNursuryReport2(MISNursueryModel model, string Action)
        {
            
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetNurseriesReport1", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.Parameters.AddWithValue("@NurserisId", model.NurseryName);
                cmd.Parameters.AddWithValue("@RangeCode", model.Range);
                cmd.Parameters.AddWithValue("@NURSERY_TYPE", model.NursuryType);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);


                if (dt != null && dt.Tables.Count > 0)
                {
                    #region Create Column
                    List<string> ColumnList = new List<string>();
                    stringBuilder.Append("<thead><tr>");
                    for (int j = 0; j < dt.Tables[0].Columns.Count; j++)
                    {
                        stringBuilder.Append("<th>" + dt.Tables[0].Columns[j].ColumnName.ToString() + "</th>");
                        ColumnList.Add(dt.Tables[0].Columns[j].ColumnName.ToString());
                    }
                    stringBuilder.Append("</tr></thead>");
                    #endregion

                    #region Create Row
                    stringBuilder.Append("<tbody>");
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        stringBuilder.Append("<tr>");
                        foreach (string col in ColumnList)
                        {
                            string RowValue = Convert.ToString(dt.Tables[0].Rows[i][col]);
                            stringBuilder.Append("<td>" + Convert.ToString(RowValue == null || RowValue == string.Empty ? "0" : RowValue) + "</td>");

                        }
                        stringBuilder.Append("</tr>");

                    }

                    stringBuilder.Append("</tbody>");
                    #endregion
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return stringBuilder.ToString();
        }


        public MISNursueryDetailsModel GetNursuryReport3(MISNursueryDetailsModel obj, string Action)
        {
            
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetNuseryReportsDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", obj.Model.UserID);
                cmd.Parameters.AddWithValue("@NurserisId", obj.Model.NurseryName);
                cmd.Parameters.AddWithValue("@RangeCode", obj.Model.RANGE_CODE);
                cmd.Parameters.AddWithValue("@NURSERY_TYPE", obj.Model.NursuryType);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                if (dt != null && dt.Tables.Count > 0)
                {
                    #region
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[0]);
                    obj.OrderInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MisNurseryModelDetails>>(str);

                    str = Newtonsoft.Json.JsonConvert.SerializeObject(dt.Tables[1]);
                    obj.NurseryInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MisNurseryModelDetails>>(str);
                    #endregion
                }

              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return obj;
        }
		
		 public string GetNursuryInventoryLogsReport(MISNursueryModel model, string Action)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetNurseryInventoryLogReport", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.Parameters.AddWithValue("@StockName", model.StockName);
                cmd.Parameters.AddWithValue("@DEPOT_NURSERY_CODE", model.NurseryName);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@FromDate", model.FROM_DATE);
                cmd.Parameters.AddWithValue("@ToDate", model.TO_DATE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);


                if (dt != null && dt.Tables.Count > 0)
                {
                    #region Create Column
                    List<string> ColumnList = new List<string>();
                    stringBuilder.Append("<thead><tr>");
                    for (int j = 0; j < dt.Tables[0].Columns.Count; j++)
                    {
                        stringBuilder.Append("<th>" + dt.Tables[0].Columns[j].ColumnName.ToString() + "</th>");
                        ColumnList.Add(dt.Tables[0].Columns[j].ColumnName.ToString());
                    }
                    stringBuilder.Append("</tr></thead>");
                    #endregion

                    #region Create Row
                    stringBuilder.Append("<tbody>");
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        stringBuilder.Append("<tr>");
                        foreach (string col in ColumnList)
                        {
                            string RowValue = Convert.ToString(dt.Tables[0].Rows[i][col]);
                            stringBuilder.Append("<td>" + Convert.ToString(RowValue == null || RowValue == string.Empty ? "0" : RowValue) + "</td>");

                        }
                        stringBuilder.Append("</tr>");

                    }

                    stringBuilder.Append("</tbody>");
                    #endregion
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return stringBuilder.ToString();
        }
    }


}


