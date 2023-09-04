using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{    
    public class ResearchPlantsAnimals:DAL
    {
      
        public int Index { get; set; }
        public long PlaceID { get; set; }
        public string PlaceName { get; set; }
        public string PlaceCat { get; set; }

        public Int64 SpecieId { get; set; }

        public string SpecieName { get; set; }
        public string SpecieCat { get; set; }
        public string SpecieType { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public string OperationType { get; set; }
        

        public DataTable Select_ResearchPlantsAnimalsS()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ResearchPlantsAnimals", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectAllResearchPlantsAnimals");
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

        public DataTable Select_ResearchPlantsAnimals(Int64 SpecieId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ResearchPlantsAnimals", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneResearchPlantsAnimals");
                cmd.Parameters.AddWithValue("@SpecieId", SpecieId);
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

        public DataTable GETPlaceForResearchPlantsAnimals()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_ResearchPlantsAnimals", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SelectAllPlaceForResearchPlantsAnimals");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
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

        public DataTable AddUpdateResearchPlantsAnimals(ResearchPlantsAnimals oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_ResearchPlantsAnimals", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateResearchPlantsAnimals");

                cmd.Parameters.AddWithValue("@SpecieId", oPlace.SpecieId);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@PlaceCat", oPlace.PlaceCat);
                cmd.Parameters.AddWithValue("@SpecieName", oPlace.SpecieName);
                cmd.Parameters.AddWithValue("@SpecieCat", oPlace.SpecieCat);
                cmd.Parameters.AddWithValue("@SpecieType", oPlace.SpecieType);
                cmd.Parameters.AddWithValue("@isActive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);
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