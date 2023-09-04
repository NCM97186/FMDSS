using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Admin
{
    public class AnimalName : DAL
    {
        public DataTable BindAnimalName()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Common_Select_AnimalName", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindAnimalName" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable SelectMediAssistType()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_SelectMediAssistType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SelectMediAssistType" + "_" + "Admin", 0, DateTime.Now, 0);
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

        public DataTable SelectCasualtyType()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_SelectCasualtyType", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SelectCasualtyType" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }
}