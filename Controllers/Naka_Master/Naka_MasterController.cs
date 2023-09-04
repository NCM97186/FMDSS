using FMDSS.Models.FmdssContext;
using FMDSS.Models.Naka_Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Naka_Master
{
    public class Naka_MasterController : BaseController
    {
        SqlConnection cnn = new SqlConnection();
        public ActionResult Naka_Master()
        {

            List<Naka_Master_Model> lst = new List<Naka_Master_Model>();
            try
            {
                TempData["msg"] = TempData["msg1"];
                TempData["isError1"] = TempData["isError"];
                ViewBag.ReturnMsg = TempData["msg"];
                ViewBag.IsError = TempData["isError1"];
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_Mst_Naka", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Naka_Master_Model Content = new Naka_Master_Model();
                    Content.SNo = Convert.ToString(i + 1);
                    Content.NakaID = Convert.ToInt64(dt.Rows[i]["NakaID"]);
                    Content.NakaName = Convert.ToString(dt.Rows[i]["NakaName"]);
                    Content.RangeCode = Convert.ToString(dt.Rows[i]["RangeCode"]);
                    Content.Range_Name = Convert.ToString(dt.Rows[i]["RANGE_NAME"]);
                    Content.ActiveStatus = Convert.ToString(dt.Rows[i]["ActiveStatus"]);
                    lst.Add(Content);
                }
                return View("Naka_Master", lst);
            }
            catch (Exception ex)
            {
            }
            return View("Naka_Master", lst);
        }

        public ActionResult GetNakaWithID(string ID)
        {
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();
            List<SelectListItem> items = new List<SelectListItem>();
            ViewBag.OpType = (ID == "0" ? "Add WorkFlow" : "Edit WorkFlow");
            TempData["ID"] = Convert.ToString(ID);
            Naka_Master_Model obj = new Naka_Master_Model();

            DataTable dtf = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_Mst_Naka", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@ActionCode", 3);
            cmd.Parameters.AddWithValue("@Naka_ID", ID);
            cmd.CommandType = CommandType.StoredProcedure;
            // db.Conn.Open();
            da.Fill(dtf);


            foreach (DataRow dr in dtf.Rows)
            {
                obj = new Naka_Master_Model
                {
                    NakaID = Convert.ToInt32(dr["NakaID"].ToString()),
                    NakaName = Convert.ToString(dr["NakaName"].ToString()),
                    RangeCode = Convert.ToString(dr["RangeCode"]),
                    Range_Name = Convert.ToString(dr["RANGE_NAME"]),
                    ActiveStatus = Convert.ToString(dr["ActiveStatus"])
                };

            }

            Range_Bind();
            return PartialView("_partialNakaMaster", obj);
        }

        public void Range_Bind()
        {
            List<SelectListItem> RangeList = new List<SelectListItem>();
            DataTable dtf = new DataTable();
            SqlCommand cmd = new SqlCommand("SP_Mst_Naka", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.Parameters.AddWithValue("@ActionCode", 5);
            cmd.CommandType = CommandType.StoredProcedure;
            da.Fill(dtf);
            RangeList.Add(new SelectListItem { Text = "---Select---", Value = "0" });
            foreach (DataRow dr in dtf.Rows)
            {
                RangeList.Add(new SelectListItem { Text = dr["RANGENAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
            }

            ViewBag.RangeList = RangeList;

        }

        [HttpPost]
        public ActionResult AddEditNakaMaster(Naka_Master_Model obj)
        {
            try
            {
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand("SP_Mst_Naka", cnn);
                string temprowid = Convert.ToString(TempData["ID"]);
                cmd.Parameters.AddWithValue("@Naka_Name", obj.NakaName);
                cmd.Parameters.AddWithValue("@Range_code", obj.RangeCode);
                cmd.Parameters.AddWithValue("@ActiveStatus", Convert.ToBoolean(obj.ActiveStatus));
                if (temprowid == "0" || temprowid == "")
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ActionCode", 7);

                }
                else
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@ActionCode", 6);
                    cmd.Parameters.AddWithValue("@ID", temprowid);
                    TempData.Remove("ID");
                }
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bool isError = Convert.ToBoolean(reader["iserror"]);
                    string ReturnMessage = Convert.ToString(reader["returnmessage"]);
                    TempData["msg1"] = ReturnMessage;
                    TempData["isError"] = isError;
                }
                reader.Close();
            }

            catch (Exception ex)
            {

            }
            return RedirectToAction("Naka_Master");
        }
    }
}
