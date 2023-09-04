using FMDSS.Models.EventManagementModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.EventManagement
{
    public class EventManagementController : Controller
    {
        public ActionResult EventManagement()
        {
            return View();
        }
        public ActionResult GetEventCalendar()
        {
            JsonResult result = new JsonResult();
            try
            {
                List<EventDetails> data = this.LoadData();
                result = this.Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }

        public ActionResult GetEventCalendarWithID(string ID)
        {
            JsonResult result = new JsonResult();
            try
            {
                List<EventDetails> data = this.LoadData(ID);
                result = this.Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }
        private List<EventDetails> LoadData(string ID)
        {
            TempData["ID"] = Convert.ToString(ID);
            DataTable dtf = GetAllData(ID);
            List<EventDetails> lst = new List<EventDetails>();

            foreach (DataRow dr in dtf.Rows)
            {
                lst.Add(
                     new EventDetails()
                     {

                         ID = Convert.ToInt32(dr["EventID"].ToString()),
                         EventName = Convert.ToString(dr["EventName"].ToString()),
                         Title = Convert.ToString(dr["EventTitle"].ToString()),
                         SMSTemplate = Convert.ToString(dr["SMSTemplate"].ToString()),
                         EmailTemplate = Convert.ToString(dr["EmailTemplate"].ToString()),
                         Desc = Convert.ToString(dr["Description"]),
                         Start_Date = Convert.ToDateTime(dr["EventStartDate"]),
                         End_Date = Convert.ToDateTime(dr["EventEndDate"]),
                         ActiveStatus= Convert.ToString(dr["ActiveStatus"])
                     });
            }
            return (lst);
        }
        private List<EventDetails> LoadData()
        {

            DataTable dtf = GetAllData();
            List<EventDetails> lst = new List<EventDetails>();

            foreach (DataRow dr in dtf.Rows)
            {
                lst.Add(
                     new EventDetails()
                     {
                         ID = Convert.ToInt32(dr["EventID"].ToString()),
                         EventName = Convert.ToString(dr["EventName"].ToString()),
                         Title = Convert.ToString(dr["EventTitle"].ToString()),
                         SMSTemplate = Convert.ToString(dr["SMSTemplate"].ToString()),
                         EmailTemplate = Convert.ToString(dr["EmailTemplate"].ToString()),
                         Desc = Convert.ToString(dr["Description"]),
                         Start_Date = Convert.ToDateTime(dr["EventStartDate"]),
                         End_Date = Convert.ToDateTime(dr["EventEndDate"]),
                         ActiveStatus = Convert.ToString(dr["ActiveStatus"])
                     });
            }
            return (lst);
        }
        public DataTable GetAllData(string ID)
        {

            string connection = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
            SqlConnection Conn = new SqlConnection(connection);
            Conn.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SPGETDATAFOREventDetails", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@ActionCode", 4);
            cmd.Parameters.AddWithValue("@EventID", Convert.ToInt32(ID));
            cmd.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
            Conn.Close();
            return dt;
        }
        public DataTable GetAllData()
        {

            string connection = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
            SqlConnection Conn = new SqlConnection(connection);
            Conn.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SPGETDATAFOREventDetails", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@ActionCode", 1);
            cmd.Parameters.AddWithValue("@EnteredBy", Convert.ToInt32(Session["UserID"]));
            cmd.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);
            Conn.Close();
            return dt;
        }

        public ActionResult AddEventDetails(AddEventDetails AddEvent, string command)
        {

            if (command == "Save")
            {
                AddData(AddEvent);
                return RedirectToAction("EventManagement");
            }
            else
            {
                return RedirectToAction("EventManagement");
            }

        }
        public DataTable AddData(AddEventDetails AddEvent)
        {
            int ID = Convert.ToInt32(TempData["ID"]);
            string connection = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
            SqlConnection Conn = new SqlConnection(connection);
            Conn.Open();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SPGETDATAFOREventDetails", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ActionCode", 2);
            cmd.Parameters.AddWithValue("EventID", ID);
            cmd.Parameters.AddWithValue("EventName", AddEvent.EventName);
            cmd.Parameters.AddWithValue("EventTitle", AddEvent.EventTitle);
            cmd.Parameters.AddWithValue("Description", AddEvent.EventDescription);
            cmd.Parameters.AddWithValue("EventStartDate", Globals.Util.GetDate(AddEvent.EventSDateTime));
            cmd.Parameters.AddWithValue("EventEndDate", Globals.Util.GetDate(AddEvent.EventEDateTime));
            cmd.Parameters.AddWithValue("EmailTemplate", AddEvent.EmialTemplate);
            cmd.Parameters.AddWithValue("SMSTemplate", AddEvent.SMSTemplate);
            cmd.Parameters.AddWithValue("EnteredBy", Convert.ToInt32(Session["UserID"]));
            cmd.Parameters.AddWithValue("ActiveStatus", Convert.ToBoolean(AddEvent.ActiveStatus));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if(Convert.ToInt32(TempData["ID"])!=0)
            {
                TempData.Remove("ID");
            }
            return dt;

        }
    }
}
