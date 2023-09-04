using FMDSS.Models;
using FMDSS.Models.ForestFire;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForestFire
{
    public class ForestFireAddDetailsController : BaseController
    {
        //
        // GET: /ForestFireAddDetails/
        SqlConnection cnn = new SqlConnection();

        public ActionResult ForestFireAddDetails()
        {
            List<ForestFire_AddDetails> lst = new List<ForestFire_AddDetails>();
            try
            {

                DataTable dt = new DataTable();
                WaterFireModel objWF = new WaterFireModel();
                dt = objWF.GetDistrict(Convert.ToString(Session["SSOID"]));
                ViewBag.District = GetDropdownData(1, dt);

                //string dis = GetDistrict_User(Convert.ToString(Session["SSOID"]));
                ////if (dis != "" || dis != null)
                ////{

                //cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                //cnn.Open();
                //SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", cnn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ActionCode", 4);
                //cmd.Parameters.AddWithValue("@District", dis);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    ForestFire_AddDetails Content = new ForestFire_AddDetails();
                //    Content.SNo = Convert.ToString(i + 1);
                //    Content.ID = Convert.ToInt64(dt.Rows[i]["ID"]);
                //    Content.Division = Convert.ToString(dt.Rows[i]["Division"]);
                //    Content.District = Convert.ToString(dt.Rows[i]["District"]);
                //    Content.Fire_Date = Convert.ToString(dt.Rows[i]["Fire_Date"]);
                //    Content.Fire_Time = Convert.ToString(dt.Rows[i]["Fire_Time"]);
                //    Content.Latitude = Convert.ToString(dt.Rows[i]["Latitude"]);
                //    Content.Longitude = Convert.ToString(dt.Rows[i]["Longitude"]);
                //    Content.CauseofFire = Convert.ToString(dt.Rows[i]["CauseOfFire"]);
                //    Content.TotalAreaAffected = Convert.ToString(dt.Rows[i]["TotalAreaAffected"]);
                //    Content.Remarks = Convert.ToString(dt.Rows[i]["Remarks"]);
                //    Content.QuantificationOfLoss = Convert.ToString(dt.Rows[i]["QuantificationOfLoss"]);
                //    lst.Add(Content);
                //}
                //}
                return View("ForestFireAddDetails", lst);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
            return View("ForestFireAddDetails", lst);
        }


        public ActionResult _ForestFireList(string districtName)
        {
            DataTable dt = new DataTable();
            List<ForestFire_AddDetails> lst = new List<ForestFire_AddDetails>();
            if (!string.IsNullOrEmpty(districtName))
            {
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 4);
                cmd.Parameters.AddWithValue("@District", districtName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ForestFire_AddDetails Content = new ForestFire_AddDetails();
                    Content.SNo = Convert.ToString(i + 1);
                    Content.ID = Convert.ToInt64(dt.Rows[i]["ID"]);
                    Content.Division = Convert.ToString(dt.Rows[i]["Division"]);
                    Content.District = Convert.ToString(dt.Rows[i]["District"]);
                    Content.Fire_Date = Convert.ToString(dt.Rows[i]["Fire_Date"]);
                    Content.Fire_Time = Convert.ToString(dt.Rows[i]["Fire_Time"]);
                    Content.Latitude = Convert.ToString(dt.Rows[i]["Latitude"]);
                    Content.Longitude = Convert.ToString(dt.Rows[i]["Longitude"]);
                    Content.CauseofFire = Convert.ToString(dt.Rows[i]["CauseOfFire"]);
                    Content.TotalAreaAffected = Convert.ToString(dt.Rows[i]["TotalAreaAffected"]);
                    Content.Remarks = Convert.ToString(dt.Rows[i]["Remarks"]);
                    Content.QuantificationOfLoss = Convert.ToString(dt.Rows[i]["QuantificationOfLoss"]);
                    lst.Add(Content);
                }
            }

            return PartialView(lst);

        }

        public ActionResult AddDataFireAlert(int ID, string TotalAreaAffected, string QU, string CF, string Remarks)
        {
            List<ForestFire_AddDetails> lst = new List<ForestFire_AddDetails>();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            DataTable dt = new DataTable();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ForestFireAddData", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@TotalAreaAffected", TotalAreaAffected);
                cmd.Parameters.AddWithValue("@QU", QU);
                cmd.Parameters.AddWithValue("@CF", CF);
                cmd.Parameters.AddWithValue("@Remarks", Remarks);
                cmd.Parameters.AddWithValue("@SSO_ID", Convert.ToString(Session["SSOID"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ForestFire_AddDetails Content = new ForestFire_AddDetails();
                    Content.SNo = Convert.ToString(i + 1);
                    Content.ID = Convert.ToInt64(dt.Rows[i]["ID"]);
                    Content.Division = Convert.ToString(dt.Rows[i]["Division"]);
                    Content.District = Convert.ToString(dt.Rows[i]["District"]);
                    Content.Fire_Date = Convert.ToString(dt.Rows[i]["Fire_Date"]);
                    Content.Fire_Time = Convert.ToString(dt.Rows[i]["Fire_Time"]);
                    Content.Latitude = Convert.ToString(dt.Rows[i]["Latitude"]);
                    Content.Longitude = Convert.ToString(dt.Rows[i]["Longitude"]);
                    lst.Add(Content);
                }
                return View("ForestFireAddDetails", lst);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
            return View("ForestFireAddDetails", lst);
        }

        public string GetDistrict_User(string SSO_Id)
        {
            string dist = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 5);
                cmd.Parameters.AddWithValue("@SSO_Id", SSO_Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dist = Convert.ToString(dt.Rows[0]["District"]);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
            return dist;
        }

        public ActionResult _FireAlertDetail(int Id)
        {
            ForestFire_AddDetails lst = new ForestFire_AddDetails();
            try
            {
                string dis = GetDistrict_User(Convert.ToString(Session["SSOID"]));
                if (dis != "" || dis != null)
                {
                    DataTable dt = new DataTable();
                    cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand("SP_ForestFireAddData", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionCode", 3);
                    cmd.Parameters.AddWithValue("@ID", Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    lst = Globals.Util.GetListFromTable<ForestFire_AddDetails>(dt).FirstOrDefault();
                    if (lst == null)
                        lst = new ForestFire_AddDetails();

                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
            return PartialView(lst);
        }
        private EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, DataTable dtDropdownData)
        {
            EnumerableRowCollection<SelectListItem> data = null;
            switch (actionCode)
            {
                case 1:
                    data = dtDropdownData.AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("Id"),
                        Text = x.Field<string>("DistName")
                    });
                    return data;
            }
            return null;
        }

    }
}


