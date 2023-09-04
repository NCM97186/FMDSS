using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.Master
{
    public class TicketingFee : DAL
    {
        private Int64 districtID;
        public int Index { get; set; }
        public long PlaceID { get; set; }

        public long FeesID { get; set; }


        public string OperationType { get; set; }

        public string ShiftType { get; set; }
        public string DIST_CODE { get; set; }
        public Int64 DistrictID
        {
            get { return districtID; }
            set { districtID = value; }
        }

        public string DisNAme { get; set; }

        [Required(ErrorMessage = "Please enter place name")]
        public string PlaceName { get; set; }

        public decimal IndianAdultFees_Surcharge { get; set; }

        public decimal Foreigner_TRDF { get; set; }
        public decimal Indian_TRDF { get; set; }

        public decimal Indian_TRDF_Gypsy { get; set; }

        public decimal Foreigner_TRDF_Gypsy { get; set; }


        public decimal IndianAdultFees_TigerProject { get; set; }

        public decimal Foreigner_Surcharge { get; set; }

        public decimal Foreigner_TigerProject { get; set; }



        public decimal StudentFees { get; set; }

        public decimal CameraFeesForeigner_Surcharge { get; set; }
        public decimal CameraFeesForeigner_TigerProject { get; set; }
        public decimal CameraFeesIndian_Surcharge { get; set; }
        public decimal CameraFeesIndian_TigerProject { get; set; }

        public decimal GuideFees { get; set; }

        public decimal Indian_GypsyVehicleRentFees { get; set; }
        public decimal Indian_CanterVehicleRentFees { get; set; }
        public decimal Indian_GypsyGuideFee { get; set; }
        public decimal Indian_CanterGuideFee { get; set; }

        public decimal Foreigner_GypsyVehicleRentFees { get; set; }
        public decimal Foreigner_CanterVehicleRentFees { get; set; }
        public decimal Foreigner_GypsyGuideFee { get; set; }
        public decimal Foreigner_CanterGuideFee { get; set; }

        public decimal GSTonGuideFee { get; set; }
        public decimal GSTonVehicleRentFee { get; set; }


        public decimal Vehicle_TRDF_Gypsy { get; set; }
        public decimal Vehicle_TRDF_Canter { get; set; }
        

        public decimal GuidFee_TRDF_Indian_Gypsy { get; set; }
        public decimal GuidFee_TRDF_NonIndian_Gypsy { get; set; }
        public decimal GuidFee_TRDF_Indian_Canter { get; set; }
        public decimal GuidFee_TRDF_NonIndian_Canter { get; set; }



        public decimal SingleOccupancyFees { get; set; }
        public decimal DoubleOccupancyFees { get; set; }
        public int Isactive { get; set; }
        public bool IsactiveView { get; set; }
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }


        public void Ticketauto()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_reset_autoticket", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteScalar();
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

        public DataTable Select_Places()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_Places", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlaceTicketing");
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

        public DataTable Select_Place(int FeeID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_Places", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOnePlaceTicketing");
                cmd.Parameters.AddWithValue("@FeesID", FeeID);
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

        public DataTable GETDivision()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable AddUpdatePlace(TicketingFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_Places", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdatePlaceTicketing");
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@FeesID", oPlace.FeesID);
                cmd.Parameters.AddWithValue("@DIST_CODE", oPlace.DIST_CODE);
                cmd.Parameters.AddWithValue("@IndianAdultFees_Surcharge", oPlace.IndianAdultFees_Surcharge);
                cmd.Parameters.AddWithValue("@IndianAdultFees_TigerProject", oPlace.IndianAdultFees_TigerProject);
                cmd.Parameters.AddWithValue("@Foreigner_Surcharge", oPlace.Foreigner_Surcharge);
                cmd.Parameters.AddWithValue("@Foreigner_TigerProject", oPlace.Foreigner_TigerProject);
                cmd.Parameters.AddWithValue("@Indian_TRDF", oPlace.Indian_TRDF);
                cmd.Parameters.AddWithValue("@Foreigner_TRDF", oPlace.Foreigner_TRDF);
                cmd.Parameters.AddWithValue("@GuideFees", oPlace.GuideFees);
                cmd.Parameters.AddWithValue("@CameraFeesForeigner_TigerProject", oPlace.CameraFeesForeigner_TigerProject);
                cmd.Parameters.AddWithValue("@CameraFeesForeigner_Surcharge", oPlace.CameraFeesForeigner_Surcharge);
                cmd.Parameters.AddWithValue("@CameraFeesIndian_TigerProject", oPlace.CameraFeesIndian_TigerProject);
                cmd.Parameters.AddWithValue("@CameraFeesIndian_Surcharge", oPlace.CameraFeesIndian_Surcharge);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@StudentFees", oPlace.StudentFees);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);

                cmd.Parameters.AddWithValue("@SingleOccupancyFees", oPlace.SingleOccupancyFees);
                cmd.Parameters.AddWithValue("@DoubleOccupancyFees", oPlace.DoubleOccupancyFees);

                cmd.Parameters.AddWithValue("@Indian_TRDF_Gypsy", oPlace.Indian_TRDF_Gypsy);
                cmd.Parameters.AddWithValue("@Foreigner_TRDF_Gypsy", oPlace.Foreigner_TRDF_Gypsy);


                //Arvind 31-07-2017

                cmd.Parameters.AddWithValue("@Indian_GypsyVehicleRentFees", oPlace.Indian_GypsyVehicleRentFees);
                cmd.Parameters.AddWithValue("@Indian_CanterVehicleRentFees", oPlace.Indian_CanterVehicleRentFees);
                cmd.Parameters.AddWithValue("@Indian_GypsyGuideFee", oPlace.Indian_GypsyGuideFee);
                cmd.Parameters.AddWithValue("@Indian_CanterGuideFee", oPlace.Indian_CanterGuideFee);

                cmd.Parameters.AddWithValue("@Foreigner_GypsyVehicleRentFees", oPlace.Foreigner_GypsyVehicleRentFees);
                cmd.Parameters.AddWithValue("@Foreigner_CanterVehicleRentFees", oPlace.Foreigner_CanterVehicleRentFees);
                cmd.Parameters.AddWithValue("@Foreigner_GypsyGuideFee", oPlace.Foreigner_GypsyGuideFee);
                cmd.Parameters.AddWithValue("@Foreigner_CanterGuideFee", oPlace.Foreigner_CanterGuideFee);

                cmd.Parameters.AddWithValue("@GSTonGuideFee", oPlace.GSTonGuideFee);
                cmd.Parameters.AddWithValue("@GSTonVehicleRentFee", oPlace.GSTonVehicleRentFee);
                cmd.Parameters.AddWithValue("@Vehicle_TRDF_Gypsy", oPlace.Vehicle_TRDF_Gypsy);
                cmd.Parameters.AddWithValue("@Vehicle_TRDF_Canter", oPlace.Vehicle_TRDF_Canter);

                cmd.Parameters.AddWithValue("@GuidFee_TRDF_Indian_Gypsy", oPlace.GuidFee_TRDF_Indian_Gypsy);
                cmd.Parameters.AddWithValue("@GuidFee_TRDF_NonIndian_Gypsy", oPlace.GuidFee_TRDF_NonIndian_Gypsy);
                cmd.Parameters.AddWithValue("@GuidFee_TRDF_Indian_Canter", oPlace.GuidFee_TRDF_Indian_Canter);
                cmd.Parameters.AddWithValue("@GuidFee_TRDF_NonIndian_Canter", oPlace.GuidFee_TRDF_NonIndian_Canter);

                //End

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

        public Int64 DeletePlace()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Delete_TicketingPlace", Conn);
                cmd.Parameters.AddWithValue("@FeeId", FeesID);
                // cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
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


        public DataTable SelectPlaceWithCategory(string DIST_CODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_Places", Conn);

                cmd.Parameters.AddWithValue("@Action", "SelectPlaceLIST");
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
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
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
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_Places", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicatePlaceTicketing");
                cmd.Parameters.AddWithValue("@FeesID", FeesID);
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

        public bool Check_DuplicateRecordVip()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_PlacesVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "CheckDuplicatePlaceTicketing");
                cmd.Parameters.AddWithValue("@FeesID", FeesID);
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

        public DataTable Select_PlacesVip()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_PlacesVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllPlaceTicketing");
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

        public DataTable AddUpdatePlaceVip(TicketingFee oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_PlacesVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdatePlaceTicketing");
                cmd.Parameters.AddWithValue("@PlaceID", oPlace.PlaceID);
                cmd.Parameters.AddWithValue("@FeesID", oPlace.FeesID);
                cmd.Parameters.AddWithValue("@DIST_CODE", oPlace.DIST_CODE);
                cmd.Parameters.AddWithValue("@IndianAdultFees_Surcharge", oPlace.IndianAdultFees_Surcharge);
                cmd.Parameters.AddWithValue("@IndianAdultFees_TigerProject", oPlace.IndianAdultFees_TigerProject);
                cmd.Parameters.AddWithValue("@Foreigner_Surcharge", oPlace.Foreigner_Surcharge);
                cmd.Parameters.AddWithValue("@Foreigner_TigerProject", oPlace.Foreigner_TigerProject);
                cmd.Parameters.AddWithValue("@Indian_TRDF", oPlace.Indian_TRDF);
                cmd.Parameters.AddWithValue("@Foreigner_TRDF", oPlace.Foreigner_TRDF);
                cmd.Parameters.AddWithValue("@GuideFees", oPlace.GuideFees);
                cmd.Parameters.AddWithValue("@CameraFeesForeigner_TigerProject", oPlace.CameraFeesForeigner_TigerProject);
                cmd.Parameters.AddWithValue("@CameraFeesForeigner_Surcharge", oPlace.CameraFeesForeigner_Surcharge);
                cmd.Parameters.AddWithValue("@CameraFeesIndian_TigerProject", oPlace.CameraFeesIndian_TigerProject);
                cmd.Parameters.AddWithValue("@CameraFeesIndian_Surcharge", oPlace.CameraFeesIndian_Surcharge);
                cmd.Parameters.AddWithValue("@Isactive", oPlace.Isactive);
                cmd.Parameters.AddWithValue("@StudentFees", oPlace.StudentFees);
                cmd.Parameters.AddWithValue("@EnteredBy", oPlace.EnteredBy);
                cmd.Parameters.AddWithValue("@UpdatedBy", oPlace.UpdatedBy);

                cmd.Parameters.AddWithValue("@SingleOccupancyFees", oPlace.SingleOccupancyFees);
                cmd.Parameters.AddWithValue("@DoubleOccupancyFees", oPlace.DoubleOccupancyFees);

                cmd.Parameters.AddWithValue("@Indian_TRDF_Gypsy", oPlace.Indian_TRDF_Gypsy);
                cmd.Parameters.AddWithValue("@Foreigner_TRDF_Gypsy", oPlace.Foreigner_TRDF_Gypsy);


                //Arvind 31-07-2017

                cmd.Parameters.AddWithValue("@Indian_GypsyVehicleRentFees", oPlace.Indian_GypsyVehicleRentFees);
                cmd.Parameters.AddWithValue("@Indian_CanterVehicleRentFees", oPlace.Indian_CanterVehicleRentFees);
                cmd.Parameters.AddWithValue("@Indian_GypsyGuideFee", oPlace.Indian_GypsyGuideFee);
                cmd.Parameters.AddWithValue("@Indian_CanterGuideFee", oPlace.Indian_CanterGuideFee);

                cmd.Parameters.AddWithValue("@Foreigner_GypsyVehicleRentFees", oPlace.Foreigner_GypsyVehicleRentFees);
                cmd.Parameters.AddWithValue("@Foreigner_CanterVehicleRentFees", oPlace.Foreigner_CanterVehicleRentFees);
                cmd.Parameters.AddWithValue("@Foreigner_GypsyGuideFee", oPlace.Foreigner_GypsyGuideFee);
                cmd.Parameters.AddWithValue("@Foreigner_CanterGuideFee", oPlace.Foreigner_CanterGuideFee);

                cmd.Parameters.AddWithValue("@GSTonGuideFee", oPlace.GSTonGuideFee);
                cmd.Parameters.AddWithValue("@GSTonVehicleRentFee", oPlace.GSTonVehicleRentFee);
                cmd.Parameters.AddWithValue("@Vehicle_TRDF_Gypsy", oPlace.Vehicle_TRDF_Gypsy);
                cmd.Parameters.AddWithValue("@Vehicle_TRDF_Canter", oPlace.Vehicle_TRDF_Canter);

                cmd.Parameters.AddWithValue("@GuidFee_TRDF_Indian_Gypsy", oPlace.GuidFee_TRDF_Indian_Gypsy);
                cmd.Parameters.AddWithValue("@GuidFee_TRDF_NonIndian_Gypsy", oPlace.GuidFee_TRDF_NonIndian_Gypsy);
                cmd.Parameters.AddWithValue("@GuidFee_TRDF_Indian_Canter", oPlace.GuidFee_TRDF_Indian_Canter);
                cmd.Parameters.AddWithValue("@GuidFee_TRDF_NonIndian_Canter", oPlace.GuidFee_TRDF_NonIndian_Canter);
                cmd.Parameters.AddWithValue("@ShiftType", oPlace.ShiftType);


                //End

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

        public DataTable Select_PlaceVip(int FeeID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_PlacesVip", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOnePlaceTicketing");
                cmd.Parameters.AddWithValue("@FeesID", FeeID);
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

        public DataTable SelectPlaceWithCategoryVip(string DIST_CODE)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Ticketing_PlacesVip", Conn);

                cmd.Parameters.AddWithValue("@Action", "SelectPlaceLIST");
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

    }

}