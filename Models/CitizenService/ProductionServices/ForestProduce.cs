using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace FMDSS.Models.CitizenService.ProductionServices
{
    public class ForestProduce:DAL
    {

        public string Districts { get; set; }
        public string Villages { get; set; }
        public DataSet District()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("getDistrict", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
            
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "District" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
           
            return ds;
        }

        public DataSet Village()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("getVillage", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@district", Districts);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
     
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Village" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
         
            return ds;
        }

        public DataSet Nursery()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("getNursery", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Village", Villages);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
         
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Nursery" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
           
            return ds;
        }



    }

    public class InfoForm : DAL
    {

        public string District { get; set; }
        public string Village { get; set; }
        public string Name { get; set; }
        public string Distribution { get; set; }
        public string Department { get; set; }
        public string BigPlant { get; set; }

        public string Type { get; set; }

        public string Plant_Species { get; set; }
        public string Total_Plant { get; set; }
        public string Rate_Per_Plant { get; set; }

        public string Calulatedfees { get; set; }

        public string ExceedRequest { get; set; }

        public string NotFound { get; set; }

        public int Id { get; set; }

        public string Action { get; set; }

        public XmlDocument strxml { get; set; }

        public string SL_NO { get; set; }

        public string EnterBy { get; set; }

        public string OnlinePurchaseStatus { get; set; }

        public int Option { get; set; }

        public DataSet PlantCount()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("getPlantCount", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nursery", Name);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "PlantCount" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
           
            return ds;
        }

        public DataSet PlantType(string PType)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("getPlantType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@district", District);
                cmd.Parameters.AddWithValue("@village", Village);
                cmd.Parameters.AddWithValue("@Nursery", Name);
                cmd.Parameters.AddWithValue("@planttype", PType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
         
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "PlantType" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
          
            return ds;
        }

        public DataSet SpeciesType()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("getSpeciesType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@district", District);
                cmd.Parameters.AddWithValue("@village", Village);
                cmd.Parameters.AddWithValue("@Nursery", Name);
                cmd.Parameters.AddWithValue("@SelectType", Type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
              
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SpeciesType" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            
            return ds;
        }
        public DataSet SpeciesBigPlant()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
            SqlCommand cmd = new SqlCommand("select distinct Plant_Species from Nursery_Info where Plant_Type='Big Plant'", Conn);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
          
            da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SpeciesBigPlant" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
          
            return ds;
        }

        public void getTotalCost()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("getTotalFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@district", District);
                cmd.Parameters.AddWithValue("@village", Village);
                cmd.Parameters.AddWithValue("@Nursery", Name);
                cmd.Parameters.AddWithValue("@SelectType", Type);
                cmd.Parameters.AddWithValue("@Species", Plant_Species);
                cmd.Parameters.AddWithValue("@NoOfPlant", Total_Plant);
                cmd.Parameters.Add("@Result", SqlDbType.Int);
                cmd.Parameters.Add("@ExceedCount", SqlDbType.Int);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.Parameters["@ExceedCount"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                if (!string.IsNullOrEmpty(cmd.Parameters["@Result"].Value.ToString()))
                {
                    int Result = Convert.ToInt32(cmd.Parameters["@Result"].Value);
                    Calulatedfees = Result.ToString();
                }
                else if (!string.IsNullOrEmpty(cmd.Parameters["@ExceedCount"].Value.ToString()))
                {
                    int ExceedCount = Convert.ToInt32(cmd.Parameters["@ExceedCount"].Value);
                    ExceedRequest = ExceedCount.ToString();
                }
                else
                {
                    NotFound = "Not found";
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "getTotalCost" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }

          
        }

        public int InsertXml(string strXmldata)
        {
            int recordsInserted = 0;
            try
            {
                DALConn();
                SqlCommand sqlcmd = new SqlCommand("sp_InsertByXML", Conn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.Add("@strXML", SqlDbType.VarChar, 4000);
                sqlcmd.Parameters[0].Value = strXmldata;
                  recordsInserted = sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertXml" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
         
          
            return recordsInserted;
          

        }
        public string Insert_Wishlist(string RequestId)
        {
            string status = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_InsertWishlist", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Item_Id", Id);
                cmd.Parameters.AddWithValue("@Item", Plant_Species);
                cmd.Parameters.AddWithValue("@Quantity", Total_Plant);
                cmd.Parameters.AddWithValue("@Total_Price", Rate_Per_Plant);
                cmd.Parameters.AddWithValue("@EnterBy", EnterBy);
                cmd.Parameters.AddWithValue("@PurchaseStatus", OnlinePurchaseStatus);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 300);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                  status = Convert.ToString(cmd.Parameters["@Result"].Value);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Insert_Wishlist" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
           
            return status;
        }

        public DataSet GetWishlst()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_getwishlist", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", EnterBy);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetWishlst" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
          
            return ds;
        }

        public string RemoveWishlist()
        {
            string status = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_removeWishlist", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Item_Id", Id);
                cmd.Parameters.AddWithValue("@UserId", EnterBy);
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 300);
                cmd.Parameters["@Status"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                 status = Convert.ToString(cmd.Parameters["@Status"].Value);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "RemoveWishlist" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
          
            return status;
        }

        public DataSet GetShoppinglist(string RequestId)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_getShoppingList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", EnterBy);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetShoppinglist" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            
            return ds;
        }
        public string Insert_NurseryInfo()
        {
            DALConn();
           
                SqlCommand cmd = new SqlCommand("sp_InsertNurseryInfo", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plant_Id", Id);
                cmd.Parameters.AddWithValue("@District_Name", District);
                cmd.Parameters.AddWithValue("@Village_Name", Village);
                cmd.Parameters.AddWithValue("@Nusery_Name", Name);
                cmd.Parameters.AddWithValue("@Plant_Type", Type);
                cmd.Parameters.AddWithValue("@Plant_Species", Plant_Species);
                cmd.Parameters.AddWithValue("@Total_Plant", Total_Plant);
                cmd.Parameters.AddWithValue("@Rate_Per_Plant", Rate_Per_Plant);
                cmd.Parameters.AddWithValue("@EnterBy", EnterBy);
                cmd.Parameters.AddWithValue("@option", Option);
                cmd.Parameters.Add("@Result", SqlDbType.VarChar, 300);
                cmd.Parameters.Add("@RowInserted", SqlDbType.Int);
                cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                cmd.Parameters["@RowInserted"].Direction = ParameterDirection.Output;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Insert_NurseryInfo" + "_" + "Forest_Production", 3, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                }
                finally
                {
                    Conn.Close();
                }
                if (Option == 1)
                {
                    string status = Convert.ToString(cmd.Parameters["@Result"].Value);
                    return status;
                }
                else if (Option == 2)
                {
                    int RowInserted = Convert.ToInt32(cmd.Parameters["@RowInserted"].Value);
                    return RowInserted.ToString();
                }
                else
                {
                    string status = Convert.ToString(cmd.Parameters["@Result"].Value);
                    return status;
                }
           

        }


    }
}