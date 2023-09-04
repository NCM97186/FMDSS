using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FMDSS.Models.CommanModels
{
    public class UserRolesModel
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }

    }

    public class UserRolesModelDetailsByRoles
    {
        public string UserID { get; set; }
        public string RoleID { get; set; }
        public List<string> RoleName { get; set; }

    }
        public class CommanRepo : DAL
        {

            public UserRolesModelDetailsByRoles GetUserRoleBySSOID(string SSOID, string Action)
            {
                DataTable dt = new DataTable();
                List<UserRolesModel> List = new List<UserRolesModel>();
                UserRolesModelDetailsByRoles Model = new UserRolesModelDetailsByRoles();
                try
                {
                    DALConn();
                    SqlCommand cmd = new SqlCommand("SP_GetRoleByUserID", Conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@Action", Action);
                    cmd.Parameters.AddWithValue("@SSOID", SSOID);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(dt);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string str = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                        List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserRolesModel>>(str);

                        Model.UserID = List.FirstOrDefault().UserID;
                        Model.RoleID = List.FirstOrDefault().RoleID;
                        Model.RoleName = List.Select(s => s.RoleName).ToList();
                    }
                }
                catch (Exception ex)
                {
                    Model = new UserRolesModelDetailsByRoles();
                    throw ex;
                }
                finally
                {
                    Conn.Close();
                }
                return Model;
            }
            private static List<T> ConvertDataTable<T>(DataTable dt)
            {
                List<T> data = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;
            }
            private static T GetItem<T>(DataRow dr)
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                }
                return obj;
            }
        }
  
}