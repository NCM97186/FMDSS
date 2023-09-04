using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class FilmShootingFees : DAL
    {
        public Int64 FilmShootingFeesID { get; set; }
        public int Index { get; set; }
        public long DIST_CODE { get; set; }

        public long PlaceID { get; set; }

        public string OperationType { get; set; }

        public string DIST_NAME { get; set; }

        public string PlaceName { get; set; }

        public decimal IndianMemberFees { get; set; }

        public decimal NonIndianMemberFees { get; set; }

        public decimal StudentFees { get; set; }

        public decimal Discount { get; set; }

        public decimal TaxRate { get; set; }

        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }


        public DataTable BindDistrict(string Division)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_District", Conn);
                cmd.Parameters.AddWithValue("@divCode", Division);
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

        public DataTable Select_FilmShootingFees()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllShootingFees");
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

        public DataTable Select_FilmShootingFee(int FilmShootingFeesID)
        {
            try
            { 
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneShootingFees");
                cmd.Parameters.AddWithValue("@FilmShootingFeesID", FilmShootingFeesID);
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

        //public DataTable GETDivision()
        //{
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
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

        public DataTable AddUpdateFilmShootingFee(FilmShootingFees oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateShootingFees");
                cmd.Parameters.AddWithValue("@FilmShootingFeesID", oPlace.FilmShootingFeesID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@DIST_CODE", oPlace.DIST_CODE);
                cmd.Parameters.AddWithValue("@IndianMemberFees", oPlace.IndianMemberFees);
                cmd.Parameters.AddWithValue("@NonIndianMemberFees", oPlace.NonIndianMemberFees);
                cmd.Parameters.AddWithValue("@StudentFees", oPlace.StudentFees);
                cmd.Parameters.AddWithValue("@Discount", oPlace.Discount);
                cmd.Parameters.AddWithValue("@TaxRate", oPlace.TaxRate);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
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

        //public Int64 DeleteCampFees()
        //{
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("SP_Delete_TicketingPlace", Conn);
        //        cmd.Parameters.AddWithValue("@FeeId", CampFeesID);
        //        // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
        //        return chk;
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

        public DataTable SelectPlaceCategory(string DIST_CODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_PlaceName", Conn);
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
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
        public DataTable Select_Places_ByDistrict()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_Places_by_DistrictID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DIST_CODE);
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



        public bool Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FilmShootingFees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateShootingFees");
                cmd.Parameters.AddWithValue("@FilmShootingFeesID", FilmShootingFeesID);
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);                
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return false;
                else
                    return true;
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