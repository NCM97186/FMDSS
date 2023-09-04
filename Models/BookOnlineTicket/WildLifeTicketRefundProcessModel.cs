using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace FMDSS.Models.BookOnlineTicket
{
    public class WildLifeTicketRefundProcessModel
    {
        public string  ApplicationLevel { get; set; }
        public string RequestID { get; set; }
        [JsonIgnore]
        public string SSOId { get; set; }
        [JsonIgnore]
        public string TicketAmount { get; set; }
        [JsonIgnore]
        public string ServiceCharge { get; set; }
        [JsonIgnore]
        public string RefundAmount { get; set; }
        public string Reason { get; set; }
        [JsonIgnore]
        public bool Checked { get; set; }

    }

    public class WildLifeTicketRefundProcessListModel
    {
        public WildLifeTicketRefundProcessListModel()
        {
            List = new List<WildLifeTicketRefundProcessModel>();
        }
       public List<WildLifeTicketRefundProcessModel> List { get; set; }
        public string ButtonName { get; set; }

    }

    public class WildLifeTicketRefundProcessRepository:DAL
    {
        public DataSet GETWildLifeTicketRefundProcess(WildLifeTicketRefundProcessListModel model,string Action)
        {
            DataSet DS = new DataSet();
            try
            {
                #region Convert Model Into Datatable
                if(model.List.Count==0)
                {
                    model.List = new List<WildLifeTicketRefundProcessModel>() { new WildLifeTicketRefundProcessModel { RequestID=string.Empty, Reason=string.Empty,ApplicationLevel="0"} };
                }
                string JSONString = JsonConvert.SerializeObject(model.List);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
                #endregion



                DALConn();
                SqlCommand cmd = new SqlCommand("TB_BookTicket_RefundDept_ReviewApprove", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@WildLifeCancellation_Type", dt);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "WildLifeTicketRefundProcessRepository" + "_" + "GETWildLifeTicketRefundProcess", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return DS;
        }


    }

}