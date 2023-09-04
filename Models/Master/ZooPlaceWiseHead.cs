using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class ZooPlaces
    {
        public int Index { get; set; }
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
    public class ZooPlaceWiseHead : DAL
    {

        public Int32 ZooPlaceWiseHeadId { get; set; }
        public int Index { get; set; }
        public Int64 PlaceID{ get; set; }
       
       
        public string PlaceName { get; set; }
        public int HeadId { get; set; }
        public string HeadName { get; set; }
        public decimal HeadAmount { get; set; }
        public string FeeChargedOn { get; set; }
        public string ParentFeeChangeON { get; set; }
        public string Type { get; set; }
        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
        public string OperationType { get; set; }


        public DataTable SelectPlacesForZoo()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectPlaces");
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

        public DataTable Select_ZooPlaceWiseHeads()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZooPlaceWiseHead");
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

        public DataTable Select_ZooPlaceWiseHead(int ZooPlaceWiseHeadId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneZooPlaceWiseHead");
                cmd.Parameters.AddWithValue("@ZooPlaceWiseHeadId", ZooPlaceWiseHeadId);
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


        public DataTable SelectHeadDetailsPlaceWise(string PlaceId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectHeadDetailsPlaceWise");
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        public DataTable AddUpdateZooPlaceWiseHead(ZooPlaceWiseHead oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateZooPlaceWiseHead");
                cmd.Parameters.AddWithValue("@ZooPlaceWiseHeadId", oPlace.ZooPlaceWiseHeadId);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@HeadId", oPlace.HeadId);
                cmd.Parameters.AddWithValue("@FeeChargedOn", oPlace.FeeChargedOn);
                cmd.Parameters.AddWithValue("@HeadAmount", oPlace.HeadAmount);
                cmd.Parameters.AddWithValue("@ParentFeeChangeON", oPlace.ParentFeeChangeON);
                cmd.Parameters.AddWithValue("@Type", oPlace.Type);
                cmd.Parameters.AddWithValue("@isActive", oPlace.isActive);
                

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

        public DataTable ZooHeadName()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllZooHeadName");

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


        public DataTable PlaceName1()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ZooPlaceWiseHead", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetAllPlaceName");

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