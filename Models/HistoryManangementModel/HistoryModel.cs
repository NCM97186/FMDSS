using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.HistoryManangementModel
{
    public class HistoryModel : BaseModel
    {
        public string ModuleName { get; set; }
        public string ModuleDesc { get; set; }
        public string TODate { get; set; }
        public string FromDate { get; set; }
        public string FileUploader { get; set; }
        public long Index { get; set; }
        public string CreatedDate { get; set; }
    }


    public class HistoryDetailsModel
    {

        public HistoryDetailsModel()
        {
            model = new HistoryModel();
            List = new List<HistoryModel>();
        }
        public HistoryModel model { get; set; }
        public List<HistoryModel> List { get; set; }
    }



    public class HistoryRepo : DAL
    {
        public DataTable SelectHistoryManagement(HistoryModel model,string Action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_GETHistoryManagement", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@ModuleName", model.ModuleName);
                cmd.Parameters.AddWithValue("@ModuleDesc", model.ModuleDesc);
                cmd.Parameters.AddWithValue("@TODate", model.TODate);
                cmd.Parameters.AddWithValue("@FromDate", model.FromDate);
                cmd.Parameters.AddWithValue("@FileUploader", model.FileUploader);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectHistoryManagement" + "_" + "HistoryRepo", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }
}