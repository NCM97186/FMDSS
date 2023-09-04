using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class NurseryFDMProduct : DAL
    {

        public Int64 ID{ get; set; }
        public int Index { get; set; }
        public Int64 ProduceTypeID { get; set; }

        public string ProduceFor { get; set; }

        public string ProductThumbImage { get; set; }
        public string ProductFullImage { get; set; }
        public string ProduceType { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }     
        public int IsActive { get; set; }
        [Required (ErrorMessage ="Plant age Requied")]
        public int PlantAge { get; set; } //0 for Less than one year and  1 for greater than year
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }
        public string BaseProduceTypeID { get; set; }
        public int[] BaseProduceTypeIDs { get; set; }
        public decimal DiscountCitizen { get; set; }
        public decimal DiscountDepartment { get; set; }
        public decimal DiscountNGO { get; set; }
		public string ProductNamesID { get; set; }
		public string ProducTCategoryID { get; set; }
		public string ProductCategory { get; set; }


		public IEnumerable<MapNurserieHeadPrice> HeadPriceList { get; set; }


        public DataTable Select_NurseryFDMProducts(string ProduceFor)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllNurseryFDMProduct");
                cmd.Parameters.AddWithValue("@ProduceFor", ProduceFor);
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

        public DataSet Select_NurseryFDMProduct(int ID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneNurseryFDMProduct");
                cmd.Parameters.AddWithValue("@ID", ID);
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

        public DataTable AddUpdateNurseryFDMProduct(NurseryFDMProduct oPlace,DataTable DT)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateNurseryFDMProduct");
                cmd.Parameters.AddWithValue("@ID", oPlace.ID);
                cmd.Parameters.AddWithValue("@ProduceTypeID", oPlace.ProduceTypeID);
                cmd.Parameters.AddWithValue("@ProductName", oPlace.ProductName);
                cmd.Parameters.AddWithValue("@Unit", oPlace.Unit);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);
                cmd.Parameters.AddWithValue("@ProduceType", oPlace.ProduceType);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsActive);
                cmd.Parameters.AddWithValue("@DiscountCitizen", oPlace.DiscountCitizen);
                cmd.Parameters.AddWithValue("@DiscountDepartment", oPlace.DiscountDepartment);
                cmd.Parameters.AddWithValue("@DiscountNGO", oPlace.DiscountNGO);
                cmd.Parameters.AddWithValue("@BaseProduceTypeID", oPlace.BaseProduceTypeID);
				//cmd.Parameters.AddWithValue("@ProductThumbImage", oPlace.ProductThumbImage);
				//cmd.Parameters.AddWithValue("@ProductFullImage", oPlace.ProductFullImage);
				cmd.Parameters.AddWithValue("@HeadDetails", DT);
				cmd.Parameters.AddWithValue("@ProduceFor", oPlace.ProduceFor);
				cmd.Parameters.AddWithValue("@ProduceCategoryID", oPlace.ProducTCategoryID);
				cmd.Parameters.AddWithValue("@ProduceNamesID", oPlace.ProductNamesID);
                cmd.Parameters.AddWithValue("@PlantAge", oPlace.PlantAge);

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

        public DataTable ProductType(string ProduceFor)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllProducttype");
                cmd.Parameters.AddWithValue("@ProduceFor", ProduceFor);
                
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


        public DataTable BaseProductType(string ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllBaseProductType");
                cmd.Parameters.AddWithValue("@ProduceTypeID", ID);
                
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

		public DataTable ProductList(string ProduceTypeId,int ProductCategoryId)
		{
			try
			{
				DALConn();
				DataTable dt = new DataTable();
				SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);

				cmd.Parameters.AddWithValue("@Action", "GetAllProduct");
                cmd.Parameters.AddWithValue("@ProduceCategoryID", ProductCategoryId);
                cmd.Parameters.AddWithValue("@ProduceTypeID", ProduceTypeId);
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

		public DataTable ProductCategoryList()
		{
			try
			{
				DALConn();
				DataTable dt = new DataTable();
				SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter da = new SqlDataAdapter(cmd);

				cmd.Parameters.AddWithValue("@Action", "GetProductCategry");
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
        public DataTable ProductProduceTypeList(int ProduceTypeID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetBaseProduceType");
                cmd.Parameters.AddWithValue("@ProduceTypeID", ProduceTypeID);
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
        public DataTable ProductLeafTypeNurseryList()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_NurseryFDMProduct", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetProductLeafTypeNursery");                
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
    }
}