using FMDSS.CustomModels.Models;
using FMDSS.Entity;
using FMDSS.Entity.Protection.ViewModel;
using FMDSS.Entity.ViewModel;
using FMDSS.Models;
using FMDSS.Models.ForestFire;
using FMDSS.Models.Rescue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class WaterRFFLineController : ApiController
    {
        [HttpGet]
        public DataTableResponse GetCircel(string SSOId)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetCircel(SSOId);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }



        [HttpGet]
        public DataTableResponse GetCircelAll()
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetCircelAll();
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetMasterDataListCircleDivisionRangeWise(string Action, string ChildAction, string CircleCode, string DivisionCode, string RangeCode, string NakaID, string DistictCode, long UserID, int FYearId)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetMasterDataListCircleDivisionRangeWise(Action, ChildAction, CircleCode, DivisionCode, RangeCode, NakaID, DistictCode, UserID, FYearId);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetDistrict(string SSOId)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetDistrict(SSOId);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetDivision(string SSOId, string ParentID)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetDivisionOnUser(SSOId, ParentID);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetRange(string ParentID)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetRange(ParentID);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetNaka(string ParentID)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetNaka(ParentID);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }


        #region "Fire Alert"

        [HttpGet]
        public DataTableResponse GetFireAlertCircle(string ActionType, string SSOId = "")
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetFireAlertCircle(ActionType, SSOId);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [System.Web.Mvc.HttpGet]
        public DataTableResponse GetFireAlertDivision(string ActionType, string Circle_Code, string SSOId = "")
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetFireAlertDivision(ActionType, Circle_Code, SSOId);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetFireAlertRange(string ActionType, string Division_Code, string SSOId = "")
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetFireAlertRange(ActionType, Division_Code, SSOId);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [HttpGet]
        public DataTableResponse GetGISAlertData(string ActionType, string Circle_Code, string Division_Code, string Range_Code, string SSOID)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetGISAlertData(ActionType, Circle_Code, Division_Code, Range_Code, SSOID);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        #endregion

        [HttpPost]
        public ResponseMsg Upload_DocumentAPI(WaterFireModel model)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = Globals.Util.GetAppSettings("WaterResourceFireLine");
            List<CommonDocumentAPI> lstDoc = new List<CommonDocumentAPI>();
            ResponseMsg response = new ResponseMsg();
            var docType = "";
            switch (model.Type)
            {
                case 1: docType = "UploadKMLFile"; break;
                case 2: docType = "UploadSourceKMLFile"; break;
                case 3: docType = "UpKMLFile_ForestFireLine"; break;
                case 4: docType = "UpSourceKMLFile_ForestFireLine"; break;

            }
            if (!string.IsNullOrWhiteSpace(model.Image) && !string.IsNullOrEmpty(model.Image) && model.Type != 0)
            {
                try
                {
                    string src = model.Image;
                    docType = Convert.ToString(model.Type);
                    FileFullName = DateTime.Now.Ticks + "_" + model.Type + "." + GetFileExtensionFromBase64(model.Image);
                    string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                    path = FilePath + FileFullName;
                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(model.Image));
                    if (model.Type == 1)
                    {
                        System.Web.HttpContext.Current.Session["WaterPointImagePath"] = path;
                    }
                    else if (model.Type == 2)
                    {
                        System.Web.HttpContext.Current.Session["WaterSourceImagePath"] = path;
                    }
                    else if (model.Type == 3)
                    {
                        System.Web.HttpContext.Current.Session["FFLinePointImagePath"] = path;
                    }
                    else if (model.Type == 4)
                    {
                        System.Web.HttpContext.Current.Session["FFLineSourceImagePath"] = path;
                    }
                    response = new ResponseMsg { IsError = false, ReturnMsg = "Sucessfully Upload", ReturnIDs = "0" };
                }
                catch (Exception ex)
                {
                    response = new ResponseMsg { IsError = true, ReturnMsg = ex.Message, ReturnIDs = "0" };
                }
            }
            else
            {
                response = new ResponseMsg { IsError = true, ReturnMsg = "Documnet not attached or DocType is 0.", ReturnIDs = "0" };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse WaterManagerPoints(string circleCode = "", string divisionCode = "", string rangeCode = "", string nakaCode = "", long UserId = 0)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                ds = objWF.WaterResFFLineDetails(circleCode, divisionCode, rangeCode, nakaCode);
                dt = ds.Tables[0];
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", ErrorDescription = ex.Message, Data = dt };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse WaterPointsList(string WaterPoint_LatLong, string fromDate, string toDate)
        {

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DataTableResponse response = new DataTableResponse();
            try
            {
                if (!string.IsNullOrEmpty(fromDate))
                {
                    FromDate = Convert.ToDateTime(fromDate);
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    ToDate = Convert.ToDateTime(toDate);
                }
                DataSet ds = new DataSet();
                WaterFireModel objWF = new WaterFireModel();
                List<WaterFireModel> WRList = new List<WaterFireModel>();
                ds = objWF.WaterResFFLineDetails(WaterPoint_LatLong, fromDate, toDate);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = ds.Tables[0] };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed" };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", ErrorDescription = ex.Message };
            }
            return response;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        WaterFireModel w = new WaterFireModel();
            //        w.SNO = Convert.ToInt32(ds.Tables[0].Rows[i]["SNO"]);
            //        w.DivisionName = Convert.ToString(ds.Tables[0].Rows[i]["DIV_NAME"]);
            //        w.RangeName = Convert.ToString(ds.Tables[0].Rows[i]["RANGE_NAME"]);
            //        w.NakaName = Convert.ToString(ds.Tables[0].Rows[i]["NakaName"]);
            //        w.BlockName = Convert.ToString(ds.Tables[0].Rows[i]["BlockName"]);
            //        w.WaterPoint_LatLong = Convert.ToString(ds.Tables[0].Rows[i]["WaterPoint_LatLong"]);
            //        w.WaterSource_LatLong = Convert.ToString(ds.Tables[0].Rows[i]["WaterSource_LatLong"]);
            //        w.Distance = Convert.ToString(ds.Tables[0].Rows[i]["Distance"]);
            //        w.EnteredOn = Convert.ToString(ds.Tables[0].Rows[i]["EnteredOn"]);
            //        w.Enteredby = Convert.ToString(ds.Tables[0].Rows[i]["EnteredBy"]);
            //        w.WaterPointImagePath = Convert.ToString(ds.Tables[0].Rows[i]["WaterPointImagePath"]);
            //        w.WaterSourceImagePath = Convert.ToString(ds.Tables[0].Rows[i]["WaterSourceImagePath"]);
            //        WRList.Add(w);
            //    }
            //}
            //ViewBag.WRList = WRList;
            //ViewBag.WRList = WRList;
            //return PartialView("_WRDetials");

        }
        [HttpPost]
        public DataTableResponse SaveWaterRFFLine(WaterFireModel model)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                model.Type = model.Type1;
                model.Image = model.WaterPointImagePath;
                Upload_DocumentAPI(model);

                model.Type = model.Type2;
                model.Image = model.WaterSourceImagePath;
                Upload_DocumentAPI(model);
                dt = objWF.SaveWaterResouceData(model);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }



        public List<DocumentList> Upload_ImageForestFire(string FileNameUpload, int UType, List<string> ImageFileslist)
        {
            List<DocumentList> DocList = new List<DocumentList>();
            foreach (string ImageFiles in ImageFileslist)
            {
                DocumentList view = new DocumentList();
                var path = string.Empty;
                string FileName = string.Empty;
                string FileFullName = string.Empty;
                string FilePath = Globals.Util.GetAppSettings("WaterResourceFireLine");
                List<CommonDocumentAPI> lstDoc = new List<CommonDocumentAPI>();
                ResponseMsg response = new ResponseMsg();
                var docType = "";
                switch (UType)
                {
                    case 1: docType = "UploadForestFireImage"; break;
                }

                if (!string.IsNullOrWhiteSpace(ImageFiles) && !string.IsNullOrEmpty(ImageFiles) && UType != 0)
                {
                    try
                    {
                        string ext = GetFileExtensionFromBase64(ImageFiles);
                        if (ext == "" || ext == null)
                        {
                            path = ImageFiles;

                            if (System.Web.HttpContext.Current.Session["FFimage"] == null)
                            {
                                System.Web.HttpContext.Current.Session["FFimage"] = path;
                            }
                            string[] spl = null;
                            spl = path.Split('/');
                            view.FileName = spl[spl.Length - 1];// path;
                            view.FilePath = path;
                        }
                        else
                        {
                            FileFullName = DateTime.Now.Ticks + FileNameUpload + "." + GetFileExtensionFromBase64(ImageFiles);//"_ForestFire"
                            string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                            path = FilePath + FileFullName;
                            string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(ImageFiles));

                            view.FileName = FileFullName;
                            view.FilePath = path;
                        }

                        //string src = ImageFiles;
                        //docType = Convert.ToString(UType);
                        //FileFullName = DateTime.Now.Ticks + FileNameUpload + "." + GetFileExtensionFromBase64(ImageFiles);//"_ForestFire"
                        //string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                        //path = FilePath + FileFullName;
                        //string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(ImageFiles));

                        //view.FileName = FileFullName;
                        //view.FilePath = path;
                        view.FileType = 1;//Means Fire Alert
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Upload_ImageForestFire" + "_" + "WaterRFFLineController", 0, DateTime.Now, 0);
                    }
                    DocList.Add(view);

                }
            }
            return DocList;
        }
        public static string GetFileExtensionFromBase64(string base64String)
        {
            var data = base64String.Substring(0, 39);


            if (data.ToUpper() == "IVBOR" || data.ToUpper().Contains("IVBOR"))
            {
                return "png";
            }
            else if (data.ToUpper() == "/9J/4" || data.ToUpper().Contains("/9J/4"))
            {
                return "jpg";
            }
            else if (data.ToUpper() == "JVBER" || data.ToUpper().Contains("JVBER"))
            {
                return "pdf";
            }
            else
            {
                return string.Empty;
            }

        }
        private string SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            try
            {
                if (base64String.IndexOf(',') > 0)
                {
                    string[] arr = base64String.Split(',');
                    base64String = arr[1];
                }

                byte[] bytes = Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(fullOutputPath, bytes);
                return fullOutputPath;
            }
            catch (Exception ex)
            {

            }
            return fullOutputPath;
        }
        #region new Forest Fire Alert

        [HttpGet]
        public DataTableResponse GetSQHECTList()
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetSQHECTList();
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        //http://103.203.138.101/api/WaterRFFLine/GetFireAlertDataDistrictWsie?SSOId=sanjaysingh.jadon&districtName=&CircleCode=0&DivisionCode=0&RangeCode=0&action=New&Month=0&Year=0&asOnDate=&PageSize=10&PageStart=1
        [HttpGet]
        public DataTableResponse GetFireAlertDataDistrictWsie(string SSOId, string districtName = "", string CircleCode = "", string DivisionCode = "", string RangeCode = "", string action = "", int Month = 0, int Year = 0, string asOnDate = "", int PageSize = 1, int PageStart = 10)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("AlertId");
            dt2.Columns.Add("ID1");
            dt2.Columns.Add("District");
            dt2.Columns.Add("Fire_Date");
            dt2.Columns.Add("Fire_Time");
            dt2.Columns.Add("Latitude");
            dt2.Columns.Add("Longitude");
            dt2.Columns.Add("GIS_Lat");
            dt2.Columns.Add("GIS_Long");
            dt2.Columns.Add("SpeciesList");
            dt2.Columns.Add("UploadFileList");
            dt2.Columns.Add("UploadFileListAfter");

            dt2.Columns.Add("ID");
            dt2.Columns.Add("TotalAreaAffected");
            dt2.Columns.Add("QuantificationOfLoss");
            dt2.Columns.Add("CauseOfFire");
            dt2.Columns.Add("Remarks");
            dt2.Columns.Add("Source");
            dt2.Columns.Add("Created_date");
            dt2.Columns.Add("Modified_date");
            dt2.Columns.Add("GISStatus");
            dt2.Columns.Add("Entered_by");
            dt2.Columns.Add("ForestProduceLoss");
            dt2.Columns.Add("WildlifeLoss");
            dt2.Columns.Add("AnyotherLoss");
            dt2.Columns.Add("NoPepoleInvolved");
            dt2.Columns.Add("OtherDeptHelped");
            dt2.Columns.Add("FireDateTime");
            dt2.Columns.Add("PuttOffTime");
            dt2.Columns.Add("FireActionId");
            dt2.Columns.Add("MonetoryLoss");
            dt2.Columns.Add("CurrentDate");
            dt2.Columns.Add("CurrentTime");
            dt2.Columns.Add("CurrentLat");
            dt2.Columns.Add("CurrentLong");
            dt2.Columns.Add("FireIncidentAreaId");
            dt2.Columns.Add("CauseOfFireId");
            dt2.Columns.Add("ResponseInitTime");
            dt2.Columns.Add("PutOffDate");
            dt2.Columns.Add("ImagePath");
            dt2.Columns.Add("ImagePathAfter");
            //-------------------------------------------------
            dt2.Columns.Add("Circle");
            dt2.Columns.Add("CircleCode");
            dt2.Columns.Add("ForestDivision");
            dt2.Columns.Add("ForestDivisionCode");
            dt2.Columns.Add("ForestRange");
            dt2.Columns.Add("ForestRangeCode");
            dt2.Columns.Add("PutOffDate1");
            dt2.Columns.Add("DiffTime");

            dt2.TableName = "ForestFireAlerts";
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetDataFireAlertDistrictWise(SSOId, districtName, CircleCode, DivisionCode, RangeCode, action, Month, Year, asOnDate, PageSize, PageStart);
                DataRow rw;
                foreach (DataRow row in dt.Rows)
                {
                    string[] splImg;
                    string[] splImgAfter;
                    rw = dt2.NewRow();
                    rw["AlertID"] = row["AlertID"].ToString();
                    rw["ID1"] = Convert.ToInt64(row["ID1"].ToString());
                    rw["District"] = row["District"].ToString();
                    rw["Fire_Date"] = row["Fire_Date"].ToString();
                    rw["Fire_Time"] =  (row["Fire_Time"].ToString().Length>10? (row["Fire_Time"].ToString().Contains(".")? Convert.ToDateTime(row["Fire_Time"].ToString()).ToString("hh:MM:ss") : row["Fire_Time"].ToString().Split(' ')[4]) : row["Fire_Time"].ToString());//row["Fire_Time"].ToString();//
                    rw["Latitude"] = row["Latitude"].ToString();
                    rw["Longitude"] = row["Longitude"].ToString();
                    rw["GIS_Lat"] = row["GIS_Lat"].ToString();
                    rw["GIS_Long"] = row["GIS_Long"].ToString();
                    rw["SpeciesList"] = row["SpeciesList"].ToString();


                    splImg = row["ImagePath"].ToString().Split(',');
                    splImgAfter = row["ImagePathAfter"].ToString().Split(',');
                    string jsonFirImg = Newtonsoft.Json.JsonConvert.SerializeObject(splImg);
                    rw["UploadFileList"] = jsonFirImg;
                    string jsonFirImgAfter = Newtonsoft.Json.JsonConvert.SerializeObject(splImgAfter);
                    rw["UploadFileListAfter"] = jsonFirImgAfter;
                    rw["ID"] = row["ID"].ToString();
                    rw["TotalAreaAffected"] = row["TotalAreaAffected"].ToString();
                    rw["QuantificationOfLoss"] = row["QuantificationOfLoss"].ToString();
                    rw["CauseOfFire"] = row["CauseOfFire"].ToString();
                    rw["Remarks"] = row["Remarks"].ToString();
                    rw["Source"] = row["Source"].ToString();
                    rw["Created_date"] = row["Created_date"].ToString();
                    rw["Modified_date"] = row["Modified_date"].ToString();
                    rw["GISStatus"] = row["EnteredBy"].ToString();
                    rw["Entered_by"] = row["Entered_by"].ToString();
                    rw["ForestProduceLoss"] = row["ForestProduceLoss"].ToString();
                    rw["WildlifeLoss"] = row["WildlifeLoss"].ToString();
                    rw["AnyotherLoss"] = row["AnyotherLoss"].ToString();
                    rw["NoPepoleInvolved"] = (row["NoPepoleInvolved"].ToString() == "0" ? "" : row["NoPepoleInvolved"].ToString());
                    rw["OtherDeptHelped"] = row["OtherDeptHelped"].ToString();
                    rw["FireDateTime"] = row["FireDateTime"].ToString();
                    rw["PuttOffTime"] = row["PuttOffTime"].ToString();
                    rw["FireActionId"] = row["FireActionId"].ToString();
                    rw["MonetoryLoss"] = (row["MonetoryLoss"].ToString() == "0" ? "" : row["MonetoryLoss"].ToString());
                    rw["CurrentDate"] = row["CurrentDate"].ToString();
                    rw["CurrentTime"] = row["CurrentTime"].ToString();
                    rw["CurrentLat"] = row["CurrentLat"].ToString();
                    rw["CurrentLong"] = row["CurrentLong"].ToString();
                    rw["FireIncidentAreaId"] = row["FireIncidentAreaId"].ToString();
                    rw["CauseOfFireId"] = row["CauseOfFireId"].ToString();
                    rw["ResponseInitTime"] = row["ResponseInitTime"].ToString();
                    rw["PutOffDate"] = (row["PutOffDate"].ToString() == "1900-01-01" ? "" : row["PutOffDate"].ToString());
                    rw["ImagePath"] = row["ImagePath"].ToString();
                    rw["ImagePathAfter"] = row["ImagePathAfter"].ToString();
                    //-------------------------------------------------
                    rw["Circle"] = row["CIRCLE_NAME"].ToString();
                    rw["CircleCode"] = row["Circle"].ToString();
                    rw["ForestDivision"] = row["DIV_NAME"].ToString();
                    rw["ForestDivisionCode"] = row["ForestDivision_Code"].ToString();
                    rw["ForestRange"] = row["RANGE_NAME"].ToString();
                    rw["ForestRangeCode"] = row["FOREST_RANGECODE"].ToString();
                    rw["PutOffDate1"] = (row["PutOffDate1"].ToString() == "1900-01-01" ? "" : row["PutOffDate1"].ToString());
                    if (row["PutOffDate"].ToString() != null && row["PutOffDate"].ToString() != "" && row["PutOffDate1"].ToString() != "" && row["PutOffDate1"].ToString() != null)//Convert.ToDateTime(rw["PutOffDate1"]).ToString("dd-MM-yyyy")
                        rw["DiffTime"] = GetDeffTime(Convert.ToDateTime(row["PutOffDate"]).ToString("dd-MM-yyyy"), row["PuttOffTime"].ToString(), Convert.ToDateTime(row["PutOffDate1"]).ToString("dd-MM-yyyy"), row["ResponseInitTime"].ToString());
                    else
                        rw["DiffTime"] = "";

                    dt2.Rows.Add(rw);
                }

                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt2 };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt2 };
            }
            return response;
        }


        /// <summary>
        /// Wildlife Species Details
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public DataTableResponse GetWildlife_Species()
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetWildlifeSpecies();
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        /// <summary>
        /// Water Hole Vender Details
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public DataTableResponse GetWaterHoleVenderDetails(string RangeCode = "")
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetWaterHoleVenderDetail(RangeCode);

                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        /// <summary>
        ///Single NGO Details
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public DataTableResponse GetNGODetails(string regNumber = "")
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            // WaterFireModel objWF = new WaterFireModel();
            NGOModel ngoModel = new NGOModel();
            try
            {
                dt = ngoModel.GetNGODetails1(regNumber);

                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }



        [HttpPost]
        public DataTableResponse SaveWaterHoleVenderDetails(WaterHoleVendor water)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.SaveWaterHoleVenderDetails(water);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }



        [HttpPost]
        public DataTableResponse UpdateWaterHoleVenderDetails(WaterHoleVendor water)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.UpdateWaterHoleVenderDetails(water);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }



        [HttpPost]
        public DataTableResponse SaveForestFireAlert(ForestFire_AddDetails model)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                string host = HttpContext.Current.Request.Url.ToString().Replace("api/WaterRFFLine/SaveForestFireAlert", "");
                if (model.UploadForestFireImage != null && model.UploadForestFireImage.Count > 0)
                {
                    model.ImageForestFireImage = Upload_ImageForestFire("_ForestFire", 1, model.UploadForestFireImage);
                }
                if (model.UploadForestFireImageAfter != null && model.UploadForestFireImageAfter.Count > 0)
                {
                    model.ImageForestFireImageAfter = Upload_ImageForestFire("_ForestFireAfter", 1, model.UploadForestFireImageAfter);
                }

                dt = objWF.SaveForestFireAlert(model, "SaveForestFireAlert", "WaterRFFLine", host);
                response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }
        [HttpPost]
        public ResponseMsg Upload_ImageForestFire(WaterFireModel model)
        {
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = Globals.Util.GetAppSettings("WaterResourceFireLine");
            List<CommonDocumentAPI> lstDoc = new List<CommonDocumentAPI>();
            ResponseMsg response = new ResponseMsg();
            var docType = "";
            switch (model.Type)
            {
                case 1: docType = "UploadForestFireImage"; break;
            }

            if (!string.IsNullOrWhiteSpace(model.Image) && !string.IsNullOrEmpty(model.Image) && model.Type != 0)
            {
                try
                {
                    string src = model.Image;
                    docType = Convert.ToString(model.Type);
                    FileFullName = DateTime.Now.Ticks + "_ForestFire" + "." + GetFileExtensionFromBase64(model.Image);
                    string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                    path = FilePath + FileFullName;
                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(model.Image));
                    string imagepath = string.Empty;

                    if (System.Web.HttpContext.Current.Session["FFimage"] == null)
                    {
                        System.Web.HttpContext.Current.Session["FFimage"] = path;
                        imagepath = path;
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["FFimage"] = path; //+ "," + imagepath;
                    }
                    response = new ResponseMsg { IsError = false, ReturnMsg = "Sucessfully Upload", ReturnIDs = "0" };
                }
                catch (Exception ex)
                {
                    response = new ResponseMsg { IsError = true, ReturnMsg = ex.Message, ReturnIDs = "0" };
                }
            }
            else
            {
                response = new ResponseMsg { IsError = true, ReturnMsg = "Document not attached or DocType is 0.", ReturnIDs = "0" };
            }
            return response;
        }
        [HttpGet]
        public DataSet GetAllFireStatusLists()
        {
            DataSet ds = new DataSet();
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                ds = objWF.GetAllFireStatusList();
                //var json = JsonConvert.SerializeObject(objCircleWise);
                //DataTable dataTable = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                //dataTable.TableName = "CircleWiseList";

                //ds.Tables.Add(dataTable);                
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return ds;
        }
        #endregion
        [HttpGet]
        public DataTableResponse GetWaterSourceList(string ssoid)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                ds = objWF.GetWaterSourceList(ssoid, "GetSourceName");
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again.", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }


        [HttpGet]
        public WaterSourceDetailResponse WaterPointsDetailsList(string sourceLatLong, string destinationLatLong)
        {


            WaterSourceDetailResponse response = new WaterSourceDetailResponse();
            try
            {
                DataTable ds = new DataTable();
                WaterFireModel objWF = new WaterFireModel();
                ds = objWF.GetWaterRefillList("GetWaterRefillList", sourceLatLong, destinationLatLong);

                if (ds != null && ds.Rows.Count > 0)
                {
                    response.Data = Globals.Util.GetListFromTable<WaterSourceDetail>(ds);
                    response = new WaterSourceDetailResponse { Status = ResponseStatus.Success, Message = "Success", Data = response.Data };
                }
                else
                {
                    response = new WaterSourceDetailResponse { Status = ResponseStatus.Failed, Message = "No record found." };
                }
            }
            catch (Exception ex)
            {
                response = new WaterSourceDetailResponse { Status = ResponseStatus.Failed, Message = "Failed", ErrorDescription = ex.Message };
            }
            return response;

        }


        [HttpGet]
        public DataTableResponse GetWaterDestinationList(string ssoid)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                ds = objWF.GetWaterSourceList(ssoid, "GetDestinationName");
                dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again.", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }


        [HttpGet]
        public DataTableResponse CheckUserauthenticity(string ssoid)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.Checkauthenticity(ssoid);
                if (dt != null && dt.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again.", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }

        [HttpPost]
        public DataTableResponse SaveWaterDetails(WaterSourceDetail model)
        {
            DataTableResponse response = new DataTableResponse();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                string FileFullName = string.Empty;
                var path = string.Empty;
                string FilePath = Globals.Util.GetAppSettings("WaterResourceFireLine");
                #region Save Image

                if (!string.IsNullOrEmpty(model.SourceImagePath))
                {
                    FileFullName = DateTime.Now.Ticks + "_" + "." + GetFileExtensionFromBase64(model.SourceImagePath);
                    string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                    path = FilePath + FileFullName;
                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(model.SourceImagePath));
                    model.SourceImagePath = path;
                }
                if (!string.IsNullOrEmpty(model.DestinationImagePath))
                {
                    FileFullName = "";
                    path = "";
                    FileFullName = DateTime.Now.Ticks + "_" + "." + GetFileExtensionFromBase64(model.DestinationImagePath);
                    string _FileName = HttpContext.Current.Server.MapPath("~/" + FilePath) + FileFullName;
                    path = FilePath + FileFullName;

                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(model.DestinationImagePath));
                    model.DestinationImagePath = path;
                }
                #endregion
                int result = objWF.SaveWaterSourceDetails(model, "Save");
                if (result > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Data Saved Successfully." };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Data Not Saved.Please try again." };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataTableResponse GetFireAlertList(string ActionName, string DistrictName, int Month = 0, int Year = 0, long UserID = 0)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            WaterFireModel objWF = new WaterFireModel();
            try
            {
                dt = objWF.GetFireAlertList(ActionName, DistrictName, Month, Year, UserID);

                if (dt != null && dt.Rows.Count > 0)
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success", Data = dt };
                else
                    response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Something went wrong.Please try again.", Data = dt };
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetYearMonthList()
        {


            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();

            DataTable dtMnt = new DataTable();
            dtMnt.Columns.Add("MonthId");
            dtMnt.Columns.Add("MonthName");

            List<DataTable> ObjList = new List<DataTable>();
            DataRow rw;
            DataRow rwM;
            dt.Columns.Add("YearId");
            dt.Columns.Add("YearName");
            int CurrentYear = DateTime.Now.Year;
            int yr = 1970;
            try
            {
                while (CurrentYear >= yr)
                {
                    rw = dt.NewRow();
                    rw["YearId"] = "" + CurrentYear;
                    rw["YearName"] = "" + CurrentYear;
                    dt.Rows.Add(rw);
                    CurrentYear--;
                }
                dt.TableName = "YearList";
                ObjList.Add(dt);


                int mnt = 1;
                while (mnt <= 12)
                {
                    rwM = dtMnt.NewRow();
                    rwM["MonthId"] = "" + mnt;
                    rwM["MonthName"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mnt);
                    dtMnt.Rows.Add(rwM);
                    mnt++;
                }
                dtMnt.TableName = "MonthList";
                ObjList.Add(dtMnt);
                return Request.CreateResponse(HttpStatusCode.OK, new { isError = "", msg = "success", data = ObjList });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { isError = ex.Message.ToString(), msg = ResponseStatus.Failed, data = ObjList });
                //  return  response = new DataTableResponse { Status = ResponseStatus.Failed, Message = "Failed", Data = dt };
            }
        }

        #region Creaded By Saunesh
        [HttpGet]
        public HttpResponseMessage GetOverallTime(string PostDate, string RespTime, string PutDate, string ActionTime)
        {
            DataTableResponse response = new DataTableResponse();
            string result = string.Empty;
            try
            {
                result = Convert.ToDateTime(PutDate + " " + ActionTime).Subtract(Convert.ToDateTime(PostDate + " " + RespTime)).ToString();
                //Math.Round(result = (Convert.ToDecimal(result)/60).ToString() ;//Math.Round(Convert.ToDecimal(result), 2).ToString();
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { isError = ex.Message.ToString(), msg = ResponseStatus.Failed, data = result });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { isError = "", msg = "success", data = result });

        }

        public string GetDeffTime(string ActionDate, string ActionTime, string ResponseDate, string ResponseTime)
        {
            string result = string.Empty;
            try
            {
                result = Convert.ToDateTime(ActionDate + " " + ActionTime).Subtract(Convert.ToDateTime(ResponseDate + " " + ResponseTime)).ToString();
                //Math.Round(result = (Convert.ToDecimal(result)/60).ToString() ;//Math.Round(Convert.ToDecimal(result), 2).ToString();
            }
            catch (Exception ex)
            {
                return result = ex.Message.ToString();
            }
            return result;

        } 

        #endregion

    }

}