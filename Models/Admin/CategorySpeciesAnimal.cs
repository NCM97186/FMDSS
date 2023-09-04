using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Admin
{
    public class CategorySpeciesAnimal:DAL
    {
        public Int64 CategoryId { get; set; }
        public string Category { get; set; }
        public Int64 CategoryspanimalId { get; set; }
        public string Name { get; set; }
        public string SnoSpanimal { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public Int64 CreatedBy { get; set; }
        public int IsActive { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 AddCategorySpeciesAnimal()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Category_sp_animal", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", CategoryId);
                cmd.Parameters.AddWithValue("@P_Category", Category);
                cmd.Parameters.AddWithValue("@P_Name", Name);
                cmd.Parameters.AddWithValue("@P_Description", Description);
                //cmd.Parameters.AddWithValue("@CreationDate", CreationDate);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@P_Status", "Pending");
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AddCategorySpeciesAnimal" + "_" + "AddSpeciesAnimal", 1, DateTime.Now, 0);
                 
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Int64 AddSpeciesAnimal()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp__Citizen_species_animal", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", CategoryId);
                cmd.Parameters.AddWithValue("@P_Category", Category);
                cmd.Parameters.AddWithValue("@P_CategoryId", CategoryspanimalId);
                cmd.Parameters.AddWithValue("@P_Name", Name);
                cmd.Parameters.AddWithValue("@P_SnoSpanimal", SnoSpanimal);
                cmd.Parameters.AddWithValue("@P_Description", Description);
                //cmd.Parameters.AddWithValue("@CreationDate", CreationDate);
                cmd.Parameters.AddWithValue("@P_CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@P_IsActive", 1);
                cmd.Parameters.AddWithValue("@P_Status", "Pending");
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
   
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "AddSpeciesAnimal" + "_" + "Citizen", 1, DateTime.Now, 0);
              
                
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public DataTable BindCategorySpanimal(string category)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                SqlCommand cmd = new SqlCommand("Sp__Citizen_Select_Category_Special_Animal", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_category", category);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
          
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "CategorySpanimal" + "_" + "Citizen", 1, DateTime.Now, 0);
            
                
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public DataTable BindSpanimal(string category, Int64 CategoryId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_Species_Animal", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_category", category);
                cmd.Parameters.AddWithValue("@P_CategoryId", CategoryId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindSpanimal" + "_" + "Citizen", 1, DateTime.Now, 0);
            
           //     throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetSpeciesanimalsno(Int64 spanimalId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_SnoSpeciesAnimal", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_spanimalId", spanimalId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
           
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetSpeciesanimalsno" + "_" + "Citizen", 1, DateTime.Now, 0);
               // throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



    }

}