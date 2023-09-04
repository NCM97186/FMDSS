using System;
using System.Collections.Generic;
using System.Data;

namespace FMDSS.GenericClass
{
    /// <summary>
    /// GenericClasses will be used to convert any datatable to any model or any list of models
    /// </summary>
    /// <CreatedBy>Durgesh N Sharma</CreatedBy>
    /// <CreatedOn>01 Feb 2016</CreatedOn>

    public class GenericClasses<T>
    {
        private T _value;

        /// <summary>
        /// This function will return model from datatable - Expecting single row in datatable
        /// </summary>
        /// <param name="dt">Datatable dt</param>
        /// <returns>Model</returns>
        public T model(DataTable dt)
        {
            _value = Activator.CreateInstance<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    var obj = typeof(T).GetProperty(dt.Columns[j].ToString());
                    if (obj != null && dt.Rows[i][j] != DBNull.Value)
                    {
                        if (obj.PropertyType.FullName == "System.String")
                        {
                            //if ((obj.Name.IndexOf("Date") >= 0
                            //    || obj.Name.IndexOf("date") >= 0
                            //    || obj.Name.IndexOf("EnteredOn") >= 0
                            //    || obj.Name.IndexOf("UpdatedOn") >= 0
                            //   )
                            //    && dt.Rows[i][j].ToString() != "")
                            //{

                            //    DateTime Date = Convert.ToDateTime(dt.Rows[i][j].ToString());
                            //    obj.SetValue(_value, Date.ToString("MM/dd/yyyy"), null);
                            //}
                            //else
                            obj.SetValue(_value, dt.Rows[i][j].ToString().Trim(), null);
                        }

                        if (dt.Rows[i][j].ToString() != "")
                        {
                            if (obj.PropertyType.FullName == "System.Int32")
                                obj.SetValue(_value, Convert.ToInt32(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Int64")
                                obj.SetValue(_value, Convert.ToInt64(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Datetime")
                            {
                                DateTime Date = Convert.ToDateTime(dt.Rows[i][j].ToString());
                                obj.SetValue(_value, Date.ToString("MM/dd/yyyy"), null);
                            }
                            else if (obj.PropertyType.FullName == "System.Decimal")
                                obj.SetValue(_value, Convert.ToDecimal(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Single")
                                obj.SetValue(_value, Convert.ToSingle(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Boolean")
                                obj.SetValue(_value, Convert.ToBoolean(dt.Rows[i][j].ToString()), null);
                        }
                    }
                }
            }
            return _value;
        }

        /// <summary>
        /// This function will return list of model from datatable
        /// </summary>
        /// <param name="dt">Datatable dt</param>
        /// <returns>List of model</returns>
        public List<T> lstmodel(DataTable dt)
        {
            List<T> _lst = new List<T>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _value = Activator.CreateInstance<T>();

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    var obj = typeof(T).GetProperty(dt.Columns[j].ToString());
                    if (obj != null && dt.Rows[i][j] != DBNull.Value)
                    {

                        if (obj.PropertyType.FullName == "System.String")
                        {
                            /* if ((obj.Name.IndexOf("Date") >= 0
                                 || obj.Name.IndexOf("date") >= 0
                                 || obj.Name.IndexOf("WhenAdd") >= 0
                                 || obj.Name.IndexOf("WhenMod") >= 0
                                 || obj.Name.IndexOf("WhenReviewed") >= 0
                                 || obj.Name.IndexOf("WhenSupReviewed") >= 0)
                                 && dt.Rows[i][j].ToString() != "")
                             {

                                 DateTime Date = Convert.ToDateTime(dt.Rows[i][j].ToString());
                                 obj.SetValue(_value, Date.ToString("MM/dd/yyyy"), null);
                             }
                             else*/
                            obj.SetValue(_value, dt.Rows[i][j].ToString().Trim(), null);
                        }

                        if (dt.Rows[i][j].ToString() != "")
                        {
                            if (obj.PropertyType.FullName == "System.Int32")
                                obj.SetValue(_value, Convert.ToInt32(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Int64")
                                obj.SetValue(_value, Convert.ToInt64(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Datetime")
                                obj.SetValue(_value, Convert.ToDateTime(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Datetime")
                            {
                                DateTime Date = Convert.ToDateTime(dt.Rows[i][j].ToString());
                                obj.SetValue(_value, Date.ToString("MM/dd/yyyy"), null);
                            }
                            else if (obj.PropertyType.FullName == "System.Single")
                                obj.SetValue(_value, Convert.ToSingle(dt.Rows[i][j].ToString()), null);
                            else if (obj.PropertyType.FullName == "System.Boolean")
                                obj.SetValue(_value, Convert.ToBoolean(dt.Rows[i][j].ToString()), null);
                        }
                    }
                }
                _lst.Add(_value);
            }
            return _lst;
        }
    }
}