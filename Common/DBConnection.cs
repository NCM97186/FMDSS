using System.Data.SqlClient;
using System.Web.Configuration;

namespace FMDSS.Globals
{
    public class DBConnection
    {
        public SqlConnection getConnection()
        {
            return new SqlConnection(WebConfigurationManager.ConnectionStrings["FMDSSVER2"].ToString());
        }
    }
}