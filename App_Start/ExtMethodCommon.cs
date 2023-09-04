using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.App_Start
{
    public static class ExtMethodCommon
    {

        public static DataTable AsDataTable<T>(this IEnumerable<T> enumerable)
        {
            DataTable table = new DataTable();

            T first = enumerable.FirstOrDefault();
            if (first == null)
                return table;

            PropertyInfo[] properties = first.GetType().GetProperties();
            foreach (PropertyInfo pi in properties)
                table.Columns.Add(pi.Name, pi.PropertyType);

            foreach (T t in enumerable)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo pi in properties)
                    row[pi.Name] = t.GetType().InvokeMember(pi.Name, BindingFlags.GetProperty, null, t, null);
                table.Rows.Add(row);
            }

            return table;
        } 

        public static SelectList ToSelectListExt(this DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }
    }
}