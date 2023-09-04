using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForestProtection
{
    public class CrimeSurveyDetails : DAL
    {
        public string FilesToBeUploaded { get; set; }
        public Int64 ID { get; set; }
        public string OffenseCode { get; set; }
        public DateTime Date_Of_Visit { get; set; }

        public string sDate_Of_Visit { get; set; }
        public string PlaceOfVisit { get; set; }
        public string Description_of_Crime { get; set; }
        public string Pictures_of_Crime1 { get; set; }
        public string Pictures_of_Crime2 { get; set; }
        public string Pictures_of_Crime3 { get; set; }
        public string DIV_Code { get; set; }
        public string Village_Code { get; set; }
        public string Rang_Code { get; set; }
        public string DIV_Name { get; set; }
        public string Village_Name { get; set; }
        public string IsComplete { get; set; }
        public string Range_Name { get; set; }
        public Int64 EnteredBy { get; set; }
        public Int64 UserID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Action { get; set; }

      
        public string Time_Of_Visit { get; set; }
        public DataTable Select_Survey()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();


                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_SurveyDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EnteredBy", UserID);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                //return ds;

                //SqlParameter[] parameters = { new SqlParameter("@EnteredBy", UserID) ,
                //                            new SqlParameter("@Action", Action) };
                //Fill(dt, "SP_FPM_SurveyDetails");
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Survey" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_Range()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "Select_Range_For_LoginUser", parameters);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Range" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable Select_Villages()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
             
                SqlParameter[] parameters = { new SqlParameter("@RangeCode", Rang_Code) };
                Fill(dt, "SP_FPM_getVillageByRangeCode", parameters);
       
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Villages" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Select_Offence()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters = { new SqlParameter("@EnteredBy", EnteredBy),
                                            new SqlParameter("@Action", 4) };
                Fill(dt, "SP_FPM_SurveyDetails", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_Offence" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public Int64 Insert_Crime_Survey(CrimeSurveyDetails sb)
        {Int64 chId=0;
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_SurveyDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@Date_Of_Visit", Date_Of_Visit);
                cmd.Parameters.AddWithValue("@PlaceOfVisit", PlaceOfVisit);
                cmd.Parameters.AddWithValue("@Description_of_Crime", Description_of_Crime);
                cmd.Parameters.AddWithValue("@Pictures", FilesToBeUploaded);                
                cmd.Parameters.AddWithValue("@Rang_Code", Rang_Code);
                cmd.Parameters.AddWithValue("@EnteredBy", EnteredBy);
                cmd.Parameters.AddWithValue("@Village_Code", Village_Code);
                cmd.Parameters.AddWithValue("@Latitude", Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude);
                cmd.Parameters.AddWithValue("@Action", 1);
                cmd.Parameters.AddWithValue("@Time_Of_Visit", Time_Of_Visit);
                chId = Convert.ToInt64(cmd.ExecuteNonQuery());                                              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Insert_Crime_Survey" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }




        public Int64 Update_Crime_Survey(CrimeSurveyDetails sb)
        {
            Int64 chId = 0;
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_SurveyDetails", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@Date_Of_Visit", Date_Of_Visit);
                cmd.Parameters.AddWithValue("@PlaceOfVisit", PlaceOfVisit);
                cmd.Parameters.AddWithValue("@Description_of_Crime", Description_of_Crime);
                cmd.Parameters.AddWithValue("@Pictures_of_Crime1", Pictures_of_Crime1);
                cmd.Parameters.AddWithValue("@Pictures_of_Crime2", Pictures_of_Crime2);

                cmd.Parameters.AddWithValue("@Pictures_of_Crime3", Pictures_of_Crime3);
       
                cmd.Parameters.AddWithValue("@Latitude", Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude);
                cmd.Parameters.AddWithValue("@Action", 2);
                cmd.Parameters.AddWithValue("@Time_Of_Visit", Time_Of_Visit);
                chId = Convert.ToInt64(cmd.ExecuteNonQuery());




            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Update_Crime_Survey" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }


        //public DataTable Update_Crime_Survey(CrimeSurveyDetails sb)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        DALConn();
              
        //        SqlParameter[] parameters = { 
        //                                    new SqlParameter("@Date_Of_Visit", Date_Of_Visit),
        //                                    new SqlParameter("@PlaceOfVisit", PlaceOfVisit),
        //                                    new SqlParameter("@Description_of_Crime", Description_of_Crime),
        //                                    new SqlParameter("@Pictures_of_Crime1", Pictures_of_Crime1),
        //                                    new SqlParameter("@Pictures_of_Crime2", Pictures_of_Crime2),
        //                                    new SqlParameter("@Pictures_of_Crime3", Pictures_of_Crime3),
                                     
                                     
        //                                    new SqlParameter("@Action", 1),
        //                                                 new SqlParameter("@Latitude", Rang_Code),
        //                                    new SqlParameter("@Longitude", EnteredBy),
        //                                   new SqlParameter("@Time_Of_Visit", Time_Of_Visit),
                                            
                                            
        //                                    };
        //        Fill(dt, "SP_FPM_SurveyDetails", parameters);
              
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Update_Crime_Survey" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return dt;
        //}

        public DataTable GetDetailsforEdit_Crime_Survey(CrimeSurveyDetails obj)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters = { new SqlParameter("@ID", obj.ID),
                                             new SqlParameter("@Action", 5),
                                            
                                            
                                            };
                Fill(dt, "SP_FPM_SurveyDetails", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetDetailsforEdit_Crime_Survey" + "_" + "CrimeSurveyDetails", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }
}