using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace FMDSS.Models.Master
{
    public class SpeciesAnimal:DAL
    {

        #region Class Property

        public long UserID = 2;
        public long ID { get; set; }

    public string Category { get; set; }

    public long AnimalCatId { get; set; }

    public string Name { get; set; }

    public string Sno_Species_Animal { get; set; }

    public string Cat_Description { get; set; }

    public bool IsActive { get; set; }

    public long EnteredBy { get; set; }

    public DateTime EnteredOn { get; set; }

    public long UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public string Status { get; set; }
        #endregion Class Property



    public DataTable Select_SpeciesAnimals()
    {
        try
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_SpeciesAnimals", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            cmd.Parameters.AddWithValue("@Action", "SelectAllSpeciesAnimals");
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
    public DataTable AddUpdateSpeciesAnimals(SpeciesAnimal SA)
    {
        try
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_SpeciesAnimals", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            cmd.Parameters.AddWithValue("@Action", "AddUpdateSpeciesAnimals");
            cmd.Parameters.AddWithValue("@ID", SA.ID);
            cmd.Parameters.AddWithValue("@Category", SA.Category);
            cmd.Parameters.AddWithValue("@AnimalCatId", SA.AnimalCatId);
            cmd.Parameters.AddWithValue("@Name", SA.Name);
            cmd.Parameters.AddWithValue("@Sno_Species_Animal", SA.Sno_Species_Animal);
            cmd.Parameters.AddWithValue("@Cat_Description", SA.Cat_Description);
            cmd.Parameters.AddWithValue("@Status", SA.Status);           
            cmd.Parameters.AddWithValue("@EnteredBy", UserID);
            cmd.Parameters.AddWithValue("@UpdatedBy", UserID);
            cmd.Parameters.AddWithValue("@Isactive", SA.IsActive);
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