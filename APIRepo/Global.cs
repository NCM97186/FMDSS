using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FMDSS.APIRepo
{
    public static class Util
    {
        /// <summary>
        /// Get IST Date
        /// </summary>
        /// <returns></returns>
        public static DateTime GetISTDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        /// <summary>
        /// Get IST Date
        /// </summary>
        /// <returns></returns>
        public static string GetISTDateTimeInString()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString();
        }


        public static bool isValidDataSet(this DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
                return true;
            return false;
        }

        public static bool isValidDataTable(this DataTable dt)
        {
            if (dt != null)
                return true;
            return false;
        }

        public static bool GetBoolean(this bool? value)
        {
            if (value == null)
                return false;
            else
                return Convert.ToBoolean(value);
        }

        public static bool isValidDataSet(this DataSet ds,int tableIndex)
        {
            if (ds != null && ds.Tables.Count > tableIndex)
                return true;
            return false;
        }

        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static List<T> GetListFromTable<T>(DataTable tbl) where T : new()
        {
            List<T> lst = new List<T>();
            if (isValidDataTable(tbl))
            {
                foreach (DataRow r in tbl.Rows)
                {
                    lst.Add(GetItemFromRow<T>(r));
                }
            }
            return lst;
        }

        public static List<T> GetListFromTable<T>(DataSet ds, int tableIndex) where T : new()
        {
            List<T> lst = new List<T>();

            if (isValidDataSet(ds, tableIndex))
            {
                foreach (DataRow r in ds.Tables[tableIndex].Rows)
                {
                    lst.Add(GetItemFromRow<T>(r));
                }
            }

            return lst;
        }

        private static T GetItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();
            SetItemFromRow(item, row);
            return item;
        }

        private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

        #region Developed by Rajveer
        public  static List<T> ConvertDataTable<T>( DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public  static T GetItem<T>(DataRow dr)
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
        #endregion
    }
}