using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Master
{
    public class CustomizedDashbaordModel
    {

        public string Name
        {
            get;
            set;
        }
        public bool Checked
        {
            get;
            set;
        }


    }

    public class CustomizedDashbaordRepo : DAL
    {
        public List<CustomizedDashbaordModel> ModuleName(string Action, long UserID, List<CustomizedDashbaordModel> List)
        {
            DataTable dt = new DataTable();
            DataTable CustomizedDashboardList = new DataTable();
            if (Action == "Save")
            {
                #region Convert Model Into Datatable
                string JSONString = JsonConvert.SerializeObject(List);
                CustomizedDashboardList = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
            }
            else
            {
                CustomizedDashboardList = null;
            }
                #endregion
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_DashboardRights", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserId", UserID);
                cmd.Parameters.AddWithValue("@CustomizedDashboardList", CustomizedDashboardList);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Action == "GET")
                        {
                            CustomizedDashbaordModel model = new CustomizedDashbaordModel();
                            model.Name = Convert.ToString(dt.Rows[i]["ModuleName"]);
                            model.Checked = Convert.ToBoolean(dt.Rows[i]["IsActive"]);
                            List.Add(model);
                        }
                        else
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetModuleName" + "_" + "CustomizedDashbaordRepo", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return List;
        }
    }
}