using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{

    public class EqptSanctuariesFee : DAL
    {

        public Int64 ID { get; set; }
        public int Index { get; set; }
        public Int64 PlaceID { get; set; }
        public Int64 ZoneID { get; set; }
        public string ZoneName { get; set; }
        public string PlaceName { get; set; }
        public Int64 CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string ShiftType { get; set; }
        public int TotalEqptAvailability { get; set; }
        public int SeatsPerEqpt { get; set; }
        public decimal Fee_TigerProject { get; set; }
        public decimal Fee_Surcharge { get; set; }
        public decimal TotalFee { get; set; }
        public int IsActive { get; set; }
        public int TotalSeats { get; set; }
        public int seatsForCitizen { get; set; }
        public bool IsactiveView { get; set; }
        public string EnteredOn { get; set; }
        public string EnteredBy { get; set; }
        public string OperationType { get; set; }


        public DataTable Select_EqptSanctuariesFeesVip()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFeeVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllEqptSanctuariesFee");
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

        public DataTable AddUpdateEqptSanctuariesFeeVip(EqptSanctuariesFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFeeVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateEqptSanctuariesFee");
                cmd.Parameters.AddWithValue("@ID", oPlace.ID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@CategoryID", oPlace.CategoryID);
                cmd.Parameters.AddWithValue("@Name", oPlace.Name);
                cmd.Parameters.AddWithValue("@TotalEqptAvailability", oPlace.TotalEqptAvailability);
                cmd.Parameters.AddWithValue("@SeatsPerEqpt", oPlace.SeatsPerEqpt);
                cmd.Parameters.AddWithValue("@Fee_TigerProject", oPlace.Fee_TigerProject);
                cmd.Parameters.AddWithValue("@Fee_Surcharge", oPlace.Fee_Surcharge);
                cmd.Parameters.AddWithValue("@TotalFee", oPlace.TotalFee);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.IsActive);
                cmd.Parameters.AddWithValue("@TotalSeats", oPlace.TotalSeats);
                cmd.Parameters.AddWithValue("@seatsForCitizen", oPlace.seatsForCitizen);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@ZoneID", oPlace.ZoneID);
                cmd.Parameters.AddWithValue("@ShiftType", oPlace.ShiftType);

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

        public DataTable SelectAllVehicleEquipmentCategoryVip()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFeeVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllVehicleEquipmentCategory");
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

        public Int64 DeleteEqptSanctuariesFeeVip()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.Parameters.AddWithValue("@Action", "DeleteEqptSanctuariesFee");
                cmd.Parameters.AddWithValue("@ID", ID);
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

        public bool Check_DuplicateRecordVip()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFeeVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateEqptSanctuariesFee");
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@Name", Name);

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


        public DataTable Select_EqptSanctuariesFeeVip(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFeeVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneEqptSanctuariesFee");
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

        public DataTable Select_EqptSanctuariesFees()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllEqptSanctuariesFee");
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

        public DataTable Select_EqptSanctuariesFee(int ID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneEqptSanctuariesFee");
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

        public DataTable AddUpdateEqptSanctuariesFee(EqptSanctuariesFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateEqptSanctuariesFee");
                cmd.Parameters.AddWithValue("@ID", oPlace.ID);
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@CategoryID", oPlace.CategoryID);
                cmd.Parameters.AddWithValue("@Name", oPlace.Name);
                cmd.Parameters.AddWithValue("@TotalEqptAvailability", oPlace.TotalEqptAvailability);
                cmd.Parameters.AddWithValue("@SeatsPerEqpt", oPlace.SeatsPerEqpt);
                cmd.Parameters.AddWithValue("@Fee_TigerProject", oPlace.Fee_TigerProject);
                cmd.Parameters.AddWithValue("@Fee_Surcharge", oPlace.Fee_Surcharge);
                cmd.Parameters.AddWithValue("@TotalFee", oPlace.TotalFee);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.IsActive);
                cmd.Parameters.AddWithValue("@TotalSeats", oPlace.TotalSeats);
                cmd.Parameters.AddWithValue("@seatsForCitizen", oPlace.seatsForCitizen);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@ZoneID", oPlace.ZoneID);
                cmd.Parameters.AddWithValue("@ShiftType", oPlace.ShiftType);

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

        public Int64 DeleteEqptSanctuariesFee()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.Parameters.AddWithValue("@Action", "DeleteEqptSanctuariesFee");
                cmd.Parameters.AddWithValue("@ID", ID);
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

        public DataTable SelectAllVehicleEquipmentCategory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllVehicleEquipmentCategory");
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

        public DataTable SelectAllPlaces()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlaces");
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



        public DataTable SelectAllZone(int PlaceID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllZone");
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



        public bool Check_DuplicateRecord()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_EqptSanctuariesFee", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicateEqptSanctuariesFee");
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                cmd.Parameters.AddWithValue("@Name", Name);

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