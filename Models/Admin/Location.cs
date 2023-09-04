//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Location
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@

using FMDSS.Models.Rescue;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Admin
{
    public class Location : DAL
    {
        #region global variable
        private Int64 districtID;

        public Int64 DistrictID
        {
            get { return districtID; }
            set { districtID = value; }
        }

        #endregion
        /// <summary>
        /// Function for fetching  Range from database
        /// </summary>
        /// <returns></returns>


        public DataTable BindCircle()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Circle", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCircle" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }







        public DataTable BindDivision(string circleCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division_By_CircleCode", Conn);
                cmd.Parameters.AddWithValue("@P_circleCode", circleCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDivision" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }






        public DataTable BindRangeBydivisionCode(string divisionCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Range_By_DivisionCode", Conn);
                cmd.Parameters.AddWithValue("@P_divisionCode", divisionCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Region from database
        /// </summary>
        /// <returns></returns>
        public DataTable BindRegion()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Region", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRegion" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// Function for fetching  Region from database
        /// </summary>
        /// <returns></returns>
        public DataTable BindDivision()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRegion" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Region from database
        /// </summary>
        /// <returns></returns>
        public DataTable BindRange()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Range", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRange" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Region from database
        /// </summary>
        /// <returns></returns>
        //public DataTable BindRangeBydivisionCode(string divisionCode)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Sp_Common_Select_RangeBydivisionCode", Conn);
        //        cmd.Parameters.AddWithValue("@divisionCode", divisionCode);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return dt;
        //}





        /// <summary>
        /// Function for fetching  Region from database
        /// </summary>
        /// <returns></returns>
        public DataTable BindRegionByUserID(Int64 userID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GETCIRCLECODE_FORLOGGEDINUSER", Conn);
                cmd.Parameters.AddWithValue("@USERID", userID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRegion" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Region from database
        /// </summary>
        /// <returns></returns>
        public DataTable BindRangeByUserID(Int64 userID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GETRANGEBYUSERID", Conn);
                cmd.Parameters.AddWithValue("@USERID", userID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRegion" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        ///  Function for fetching  Depot from database
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindDepotbyRangeCode(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Depot_ByRange", Conn);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDepot" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindSchedular(string Period)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_SelectSchedular_ByPeriod", Conn);
                cmd.Parameters.AddWithValue("@Period", Period);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindSchedular" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Circle using Range code from database
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindCircle(string regionCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Circle_By_RegionCode", Conn);
                cmd.Parameters.AddWithValue("@P_regionCode", regionCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCircle" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Circle using Range code from database
        /// </summary>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
       
        
        
        //public DataTable BindCircle()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Sp_Common_Select_Circle", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCircle" + "_" + "Admin", 0, DateTime.Now, 0);
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return dt;
        //}





        /// <summary>
        /// Function for fetching  Division using region and circle code from database
        /// </summary>
        /// <returns></returns>

        public DataTable BindDivision(string regionCode, string circleCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Division_By_RegionCircleCode", Conn);
                cmd.Parameters.AddWithValue("@P_regionCode", regionCode);
                cmd.Parameters.AddWithValue("@P_circleCode", circleCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDivision" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Range from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindRange(string regionCode, string circleCode, string divisionCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Range_By_RegionCircleCodeDivisionCode", Conn);
                cmd.Parameters.AddWithValue("@P_regionCode", regionCode);
                cmd.Parameters.AddWithValue("@P_circleCode", circleCode);
                cmd.Parameters.AddWithValue("@P_divisionCode", divisionCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRange" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }        /// <summary>
        /// Function for fetching  Village from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindVillage(string divCode, string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Village", Conn);
                cmd.Parameters.AddWithValue("@P_divCode", divCode);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindVillage" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable District()
        {
            DataTable dt = new DataTable();
            try
            {
                //var District = 8;
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_District", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "District");
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "District" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        public DataTable DistrictBystatecode()
        {
            DataTable dt = new DataTable();
            try
            {
                var District = 8;
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_getDistrictsbystatecode", Conn);
                cmd.CommandType = CommandType.StoredProcedure; 
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "District" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        public DataTable District(string Action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_District", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", Action);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "District" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        /// <summary>
        /// Function for fetching  Places by Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable Select_Places_ByDistrict()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("0", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictID", DistrictID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Places_ByDistrict" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        ///  Function for fetching  Depot from database
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindDepot(string regionCode, string circleCode, string divisionCode, string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Depot", Conn);
                cmd.Parameters.AddWithValue("@P_regionCode", regionCode);
                cmd.Parameters.AddWithValue("@P_circleCode", circleCode);
                cmd.Parameters.AddWithValue("@P_divisionCode", divisionCode);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDepot" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        ///  Function for fetching  Depot from database
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindDepotnotice(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Depot_Notice", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDepot" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        ///  Function for fetching  Depot from database
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindDepot(string divisionCode, string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Depot_ByDivRange", Conn);
                cmd.Parameters.AddWithValue("@P_divisionCode", divisionCode);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDepot" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindChildAnimalName(string parentId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_PM_GetAnimalDetails", Conn);

                cmd.Parameters.AddWithValue("@ParentID", parentId);
                cmd.Parameters.AddWithValue("@ActionCode", 1);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindChildAnimalName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindBlockName(string District)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Block", Conn);
                cmd.Parameters.AddWithValue("@distid", District);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindBlockName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindCityName(string District)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_City", Conn);
                cmd.Parameters.AddWithValue("@distid", District);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindCityName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindWardName(string city)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Wards", Conn);
                cmd.Parameters.AddWithValue("@cityid", city);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindWardName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindGramPanchayatName(string District, string BlockName)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_GramPanchayat", Conn);
                cmd.Parameters.AddWithValue("@DISID", District);
                cmd.Parameters.AddWithValue("@BLKID", BlockName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindGramPanchayatName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable BindVillageName(string District, string BlockName, string GPName)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Village", Conn);
                cmd.Parameters.AddWithValue("@DISID", District);
                cmd.Parameters.AddWithValue("@BLKID", BlockName);
                cmd.Parameters.AddWithValue("@GPID", GPName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindGramPanchayatName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindVillageName(string DistrictId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_Village_By_DistricID", Conn);
                cmd.Parameters.AddWithValue("@DISID", DistrictId);              
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindVillageName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable CrudOperationRescueData(RescueData model)
        {
            DataTable dt = new DataTable();
            try
            {
                System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("RU-ru");
                DALConn();
                SqlCommand cmd = new SqlCommand("spRescueData", Conn);
                cmd.Parameters.AddWithValue("@Action", model.Action);
                cmd.Parameters.AddWithValue("@RescueDate", Convert.ToDateTime(model.RescueDateTime == null ? DateTime.Now.ToString() : model.RescueDateTime, cul));
                cmd.Parameters.AddWithValue("@AnimalName", model.AnimalName);
                cmd.Parameters.AddWithValue("@DistrictId", model.DistrictId);
                cmd.Parameters.AddWithValue("@VillageID", model.VillageId);
                cmd.Parameters.AddWithValue("@Lat", model.Lat);
                cmd.Parameters.AddWithValue("@Long", model.Long);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@ProcessingStatus", model.Processing);
                cmd.Parameters.AddWithValue("@isActive", 1);
                cmd.Parameters.AddWithValue("@EnteredBy", HttpContext.Current.Session["UserId"]);
                cmd.Parameters.AddWithValue("@RescueId", model.RescueId);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
                cmd.Parameters.AddWithValue("@PostMortemReportPath", model.PostMortemReportPath);
                cmd.Parameters.AddWithValue("@FactualReportPath", model.FactualReportPath);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindVillageName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



        public DataTable BindJFMC(string District, string Block, string GP, string village)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_JFMC", Conn);
                cmd.Parameters.AddWithValue("@DISID", District);
                cmd.Parameters.AddWithValue("@BLKID", Block);
                cmd.Parameters.AddWithValue("@GPID", GP);
                cmd.Parameters.AddWithValue("@VillageID", village);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindJFMC" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindMicroPlan(string jfmcID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_MicroPlan", Conn);
                cmd.Parameters.AddWithValue("@jfmcID", jfmcID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindMicroPlan" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindFinancialYear()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_FinancialYear", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindFinancialYear" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }
        /// <summary>
        ///  Function for fetching  Depot from database
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="circleCode"></param>
        /// <param name="divisionCode"></param>
        /// <param name="rangeCode"></param>
        /// <returns></returns>
        public DataTable BindDepottp(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Depot_TP", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDepot" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable BindFromDepottp(string rangeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Select_Depot_TP", Conn);
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                cmd.Parameters.AddWithValue("@P_rangeCode", rangeCode);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDepot" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

    }
}