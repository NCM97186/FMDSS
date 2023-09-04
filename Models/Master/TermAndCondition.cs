using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class TermAndCondition : DAL
    {
        public int Index { get; set; }
        public int ID { get; set; }
      
        public long TermAndConditionID { get; set; }
        public int PlaceID { get; set; }
        public string TermAndCondition_Text { get; set; }
        public bool IsactiveView { get; set; }
        public List<ListTermAndCondition> LstTermAndCondition { get; set; }

        public int IsActive { get; set; }
        public string OperationType { get; set; }
        public int SetDisplayOrder { get; set; }
        
       
        public DataTable GetPlaces()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_MappingTermAndCondition", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlaces");
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

        public DataTable GetTermAndConditionDataOnPlaces(int PlaceID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_MappingTermAndCondition", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetTermAndConditionDataOnPlaces");
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
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


        public string AddUpdateTandC(TermAndCondition OBJ)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_MappingTermAndCondition", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateTandC");
                cmd.Parameters.AddWithValue("@PlaceID", OBJ.PlaceID);
                cmd.Parameters.AddWithValue("@ID", OBJ.ID);
                cmd.Parameters.AddWithValue("@TermAndConditionID", OBJ.TermAndConditionID);
                cmd.Parameters.AddWithValue("@IsActive", OBJ.IsactiveView);
                cmd.Parameters.AddWithValue("@SetDisplayOrder", OBJ.SetDisplayOrder); 
                
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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







        public DataTable Select_TermsConditions()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TermsAndCondition_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllTermsCondition");
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

        public DataTable Select_TermsCondition(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TermsAndCondition_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneTermsCondition");
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@TermAndCondition_Text", TermAndCondition_Text);
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

        public DataTable AddUpdateTermsCondition(TermAndCondition oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_TermsAndCondition_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateTermsCondition");
                cmd.Parameters.AddWithValue("@ID", oPlace.ID);
                cmd.Parameters.AddWithValue("@TermAndCondition_Text", oPlace.TermAndCondition_Text);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsActive);
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

    public class ListTermAndCondition
    {
        public int Index { get; set; }
        public int ID { get; set; }
        public long TermAndConditionID { get; set; }
        public int PlaceID { get; set; }
        public string TermAndCondition_Text { get; set; }
        public int SetDisplayOrder { get; set; }       
        public bool IsActive { get; set; }
    }
}