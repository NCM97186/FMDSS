using FMDSS.Models;
using FMDSS.Models.ForestFire;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.ForestFire
{
    public class ForestFireController : BaseController
    {
        //
        // GET: /ForestFire/


        private ICommonRepository _commonRepository;
        private IProtectionRepository _ProtectionRepository;
        int ModuleID = 4;
        private Int64 UserID
        {
            get
            {
                if (Session["UserID"] != null)
                    return Convert.ToInt64(Session["UserID"].ToString());
                else
                    return 0;
            }
        }
        #region [Constructor]
        public ForestFireController()
        {
            _commonRepository = new CommonRepository();
            _ProtectionRepository = new ProtectionRepository();
        }
        #endregion
        public ActionResult ForestFire_Report()
        {
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            SetDropdownData();
            return View();
        }

        private void SetDropdownData()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            try
            {
                var dsData = _commonRepository.GetDropdownData2(14, string.Empty);
                ViewBag.FinacialYearList = _commonRepository.GetDropdownData(13, string.Empty);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    ViewBag.CircleList = dsData.Tables[0].AsEnumerable().Select(x => new SelectListItem
                    {
                        Value = x.Field<string>("CIRCLE_CODE"),
                        Text = x.Field<string>("CIRCLE_NAME")
                    });

                    if (Globals.Util.isValidDataSet(dsData, 1, true))
                    {
                        ViewBag.DivList = dsData.Tables[1].AsEnumerable().Select(x => new SelectListItem
                        {
                            Value = x.Field<string>("DIV_CODE"),
                            Text = x.Field<string>("DIV_NAME")
                        });

                        if (Globals.Util.isValidDataSet(dsData, 2, true))
                        {
                            ViewBag.RangeList = dsData.Tables[2].AsEnumerable().Select(x => new SelectListItem
                            {
                                Value = x.Field<string>("RANGE_CODE"),
                                Text = x.Field<string>("RANGE_NAME")
                            });

                            if (Globals.Util.isValidDataSet(dsData, 3, true))
                            {
                                ViewBag.NakaList = dsData.Tables[3].AsEnumerable().Select(x => new SelectListItem
                                {
                                    Value = Convert.ToString(x.Field<long>("NakaID")),
                                    Text = x.Field<string>("NakaName")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, ModuleID, DateTime.Now, UserID);
            }
        }

        [HttpPost]
        public ActionResult ForestFire_Report(ForestFire_AddDetailsReport param)
        {
            ForestFire_AddDetailsVM_Total model = new ForestFire_AddDetailsVM_Total();
            DataSet ds = _ProtectionRepository.ForestFire_AddDetailsReport(param);
            if (Globals.Util.isValidDataSet(ds))
            {
                var oDetails = Globals.Util.GetListFromTable<ForestFire_AddDetailsVM>(ds, 0);
                model = Globals.Util.GetListFromTable<ForestFire_AddDetailsVM_Total>(ds, 1).FirstOrDefault();
                model.ForestFire_AddDetailsVMReportList = oDetails;
                //model.ForestFire_AddDetailsVMReportList = Globals.Util.GetListFromTable<ForestFire_AddDetailsVM>(ds, 0);
                return PartialView("_ForestFireSummaryReport", model);
            }
            return null;
        }



        public ActionResult ForestFire()
        {
            TempData["msg"] = TempData["msg1"];
            TempData["isError1"] = TempData["isError"];
            ViewBag.ReturnMsg = TempData["msg"];
            ViewBag.IsError = TempData["isError1"];
            List<ForestFireModel> oList = GetForestFireData();
            return View("ForestFire", oList);
        }
        [HttpPost]
        public ActionResult ForestFire(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                if (postedFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(postedFile.FileName);
                    string withouex = Path.GetFileNameWithoutExtension(postedFile.FileName);
                    string excelfile = withouex + "_" + DateTime.Now.ToString("MM/dd/yyyy_HH:mm:ss") + extension;
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        string path = Server.MapPath("~/ExcelSheetsForestFire/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        filePath = path + Path.GetFileName(postedFile.FileName);
                        //filePath = path + excelfile;
                        postedFile.SaveAs(filePath);

                        string conString = string.Empty;

                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        //connection String for xls file format.
                        if (extension == ".xls")
                        {
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
                        }
                        //connection String for xlsx file format.
                        else if (extension == ".xlsx")
                        {
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                        }
                        DataTable dt = new DataTable();
                        SqlDataReader reader;
                        conString = string.Format(conString, filePath);

                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);

                                    connExcel.Close();
                                }
                            }
                        }
                        SqlConnection cnn = new SqlConnection();
                        cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                        cnn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_ForestFireExcelData";
                        cmd.Connection = cnn;
                        cmd.Parameters.AddWithValue("@ActionCode", 1);
                        cmd.Parameters.Add("@Typ_ForestFireExcelData", SqlDbType.Structured).SqlValue = dt;
                        //string result = cmd.ExecuteNonQuery().ToString();
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            bool isError = Convert.ToBoolean(reader["IsError"]);
                            string ReturnMessage = Convert.ToString(reader["ReturnMessage"]);
                            TempData["msg1"] = ReturnMessage;
                            TempData["isError"] = isError;
                        }
                        reader.Close();
                    }
                    else
                    {
                        TempData["msg1"] = "The uploaded file is not Excel file.";
                        TempData["isError"] = 1;
                        return RedirectToAction("ForestFire");
                    }
                }
                else
                {
                    TempData["msg1"] = "The Excel have no data.";
                    TempData["isError"] = 1;
                    return RedirectToAction("ForestFire");
                }
            }
            else
            {
                TempData["msg1"] = "The file is not attached.";
                TempData["isError"] = 1;
                return RedirectToAction("ForestFire");
            }
            return RedirectToAction("ForestFire");
        }

        public List<ForestFireModel> GetForestFireData()
        {
            List<ForestFireModel> oObj = new List<ForestFireModel>();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            DataTable dt = new DataTable();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionCode", 2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ForestFireModel Content = new ForestFireModel();
                    Content.SNo = Convert.ToString(dt.Rows[i]["SNo"]);
                    Content.Fire_Date = Convert.ToString(dt.Rows[i]["Fire_Date"]);
                    Content.Fire_Time = Convert.ToString(dt.Rows[i]["Fire_Time"]);
                    Content.Source = Convert.ToString(dt.Rows[i]["Source"]);
                    Content.Latitude = Convert.ToString(dt.Rows[i]["Latitude"]);
                    Content.Longitude = Convert.ToString(dt.Rows[i]["Longitude"]);
                    Content.State = Convert.ToString(dt.Rows[i]["State"]);
                    Content.District = Convert.ToString(dt.Rows[i]["District"]);
                    Content.Circle = Convert.ToString(dt.Rows[i]["Circle"]);
                    Content.Division = Convert.ToString(dt.Rows[i]["Division"]);
                    Content.Range = Convert.ToString(dt.Rows[i]["Range"]);
                    Content.Block = Convert.ToString(dt.Rows[i]["Block"]);
                    Content.Beat = Convert.ToString(dt.Rows[i]["Beat"]);
                    oObj.Add(Content);
                }
                return oObj;


            }
            catch (Exception ex)
            {
            }
            finally
            {
                cnn.Close();
            }
            return oObj;
        }

        //public ActionResult ForestFire_Report()
        //{
        //    List<ForestFire_Report> lst = new List<ForestFire_Report>();
        //    SqlConnection cnn = new SqlConnection();
        //    cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        cnn.Open();
        //        SqlCommand cmd = new SqlCommand("SP_ForestFireExcelData", cnn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ActionCode", 3);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            ForestFire_Report Content = new ForestFire_Report();
        //            Content.SNo = Convert.ToString(i+1);
        //            Content.District = Convert.ToString(dt.Rows[i]["District"]);
        //            Content.NumberOfIncidents = Convert.ToString(dt.Rows[i]["NumberOfIncidents"]);
        //            Content.State = Convert.ToString(dt.Rows[i]["State"]);
        //            Content.Financial_Year = Convert.ToString(dt.Rows[i]["Financial_Year"]);
        //            Content.TotalAreaAffected = Convert.ToString(dt.Rows[i]["TotalAreaAffected"]);
        //            Content.CauseofFire = Convert.ToString(dt.Rows[i]["CauseofFire"]);
        //            Content.QuantificationOfLoss = Convert.ToString(dt.Rows[i]["QuantificationOfLoss"]);
        //            lst.Add(Content);
        //            ViewBag.State = Convert.ToString(dt.Rows[i]["State"]);
        //            ViewBag.Financial_Year = Convert.ToString(dt.Rows[i]["Financial_Year"]);
        //        }

        //        return View ("ForestFire_Report", lst);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        cnn.Close();
        //    }
        //    return View("ForestFire_Report", lst);
        //}
    }
}
