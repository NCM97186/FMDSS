using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace FMDSS.Globals
{
    public class Util
    {

        /// <summary>
        /// To Get Random Number 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRandomNumber()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            //if (rbType.SelectedItem.Value == "1")
            //{
            //	characters += alphabets + small_alphabets + numbers;
            //}
            //int length = int.Parse(ddlLength.SelectedItem.Value);
            string otp = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        /// <summary>
        /// To Get Connection settings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionsettings(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

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

        /// <summary>
        /// Get Date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime? GetDate(string dateTime)
        {
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB");
                return Convert.ToDateTime(dateTime, culture);
            }
            return null;
        }

        /// <summary>
        /// GetDateWithFormat
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetDateWithFormat(string dateTime, string format)
        {
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB");
                return Convert.ToDateTime(dateTime, culture).ToString(format);
            }
            return null;
        }

        /// <summary>
        /// Get Date
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public static DateTime? GetDate(string dateTime, string cultureName)
        {
            if (!string.IsNullOrWhiteSpace(dateTime))
            {
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(cultureName);
                return Convert.ToDateTime(dateTime, culture);
            }
            return null;
        }

        /// <summary>
        /// To import data from excel to dataset
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static DataTable ImportDataFromExcel(string FilePath, string sheetName, ref Entity.ResponseMsg msg)
        {
            try
            {
                string conStrExcel = "";
                string sName = sheetName.ToLower();

                if (FilePath.EndsWith(".xls"))
                {
                    conStrExcel = GetConnectionsettings("Excel03Connection");
                }
                else if (FilePath.EndsWith(".xlsx"))
                {
                    conStrExcel = GetConnectionsettings("Excel07Connection");
                }

                conStrExcel = String.Format(conStrExcel, FilePath);
                OleDbConnection connExcel = new OleDbConnection(conStrExcel);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataSet dsExcelData = new DataSet(sName + "s");
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet 
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                bool isSheetExist = false;

                foreach (DataRow item in dtExcelSchema.Rows)
                {
                    if (Convert.ToString(item["TABLE_NAME"]).ToLower().Equals(sheetName.ToLower() + "$"))
                    {
                        sheetName = Convert.ToString(item["TABLE_NAME"]);
                        isSheetExist = true;
                        break;
                    }
                }
                connExcel.Close();

                if (isSheetExist)
                {
                    //Read Data from First Sheet 
                    connExcel.Open();
                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                    oda.SelectCommand = cmdExcel;
                    oda.Fill(dsExcelData, sName);
                    connExcel.Close();
                    msg.ReturnMsg = "Data imported successfully.";
                    if (isValidDataSet(dsExcelData, 0))
                    {
                        var dt = dsExcelData.Tables[0];
                        dt.TableName = sName;
                        return dt;
                    }
                }
                msg.IsError = true;
                msg.ReturnMsg = "Excel sheet is not valid!";
            }
            catch (Exception ex)
            {
                msg.IsError = true;
                msg.ReturnMsg = ex.Message;
            }

            return null;
        }

        public static bool isValidDataSet(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0)
                return true;
            return false;
        }

        public static bool isValidDataSet(DataSet ds, bool isRowCheckRequired)
        {
            if (isRowCheckRequired)
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    return true;
            }
            else
            {
                if (ds != null && ds.Tables.Count > 0)
                    return true;
            }
            return false;
        }

        public static bool isValidDataTable(DataTable dt)
        {
            if (dt != null)
                return true;
            return false;
        }

        public static bool isValidDataTable(DataTable dt, bool isRowCheckRequired)
        {
            if (isRowCheckRequired)
            {
                if (dt != null && dt.Rows.Count > 0)
                    return true;
            }
            else
            {
                if (dt != null)
                    return true;
            }
            return false;
        }

        public static bool GetBoolean(bool? value)
        {
            if (value == null)
                return false;
            else
                return Convert.ToBoolean(value);
        }

        public static bool isValidDataSet(DataSet ds, int tableIndex)
        {
            if (ds != null && ds.Tables.Count > tableIndex)
                return true;
            return false;
        }

        public static bool isValidDataSet(DataSet ds, int tableIndex, bool isRowCheckRequired)
        {
            if (isRowCheckRequired)
            {
                if (ds != null && ds.Tables.Count > tableIndex && ds.Tables[tableIndex].Rows.Count > 0)
                    return true;
            }
            else
            {
                if (ds != null && ds.Tables.Count > tableIndex)
                    return true;
            }
            return false;
        }

        public static string GetAppSettings(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static DataTable GetDataTableFromObjects(object[] objects)
        {

            if (objects != null && objects.Length > 0)
            {

                Type t = objects[0].GetType();

                DataTable dt = new DataTable(t.Name);

                foreach (PropertyInfo pi in t.GetProperties())
                {
                    dt.Columns.Add(new DataColumn(pi.Name));
                }

                foreach (var o in objects)
                {

                    DataRow dr = dt.NewRow();

                    foreach (DataColumn dc in dt.Columns)
                    {

                        dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);

                    }

                    dt.Rows.Add(dr);

                }

                return dt;

            }

            return null;

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

        //added by shaan for particular work 05-02-2021
        public static List<T> GetListFromTableInCitizenList<T>(DataTable tbl) where T : new()
        {
            List<T> lst = new List<T>();
            if (isValidDataTable(tbl))
            {
                foreach (DataRow r in tbl.Rows)
                {
                    //string act = r.Table.Columns["Action"].ToString();
                    string TicketID = r.ItemArray[15].ToString();
                    string ACTION = r.ItemArray[21].ToString();
                    ACTION = ACTION.Replace(TicketID, "'" + Encryption.encrypt(TicketID) + "'");
                    if (r.Table.Columns[21].ToString() == "Actions")
                    {
                        r.SetField("Actions", ACTION);
                        lst.Add(GetItemFromRow<T>(r));
                    }
                    else
                    {
                        lst.Add(GetItemFromRow<T>(r));
                    }

                }
                tbl.AcceptChanges();
            }
            return lst;
        }
        //end  shaan
      
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

        public static DataTable ToDataTable<T>(List<T> items) where T : new()
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
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
                    try
                    {
                        var targetType = IsNullableType(p.PropertyType) ? Nullable.GetUnderlyingType(p.PropertyType) : p.PropertyType;
                        p.SetValue(item, Convert.ChangeType(row[c], targetType), null);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }

    public static class UtilExtn
    {
        /// <summary>
        /// TrimEndIfNotNull
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string TrimEndIfNotNull(this string value, char separator = ',')
        {
            if (value != null)
            {
                return value.TrimEnd(separator);
            }
            return null;
        }
    }
}