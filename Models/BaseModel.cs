using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{
    [Serializable]
    public class BaseModelSerializable
    {

    }
    public class BaseModel : DAL
    {

        /// <summary>
        /// Used for identify reviewer or approver
        /// </summary>
        /// <returns></returns>
        public bool GetCurrentURLStatus(string URL)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GetCurrentURLStatus", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@URL", URL);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return Convert.ToBoolean(dt.Rows[0][0]);

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