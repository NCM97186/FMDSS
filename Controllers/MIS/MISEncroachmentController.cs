using FMDSS.Models;
using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.MIS;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using AutoMapper;
using System.Configuration;
using System.Data.SqlClient;
using FMDSS.Models.Encroachment.ViewModel;
using FMDSS.Repository;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.CitizenService.PermissionServices;
using FMDSS.Models.Master;
using log4net;

namespace FMDSS.Controllers.MIS
{
    public class MISEncroachmentController : Controller
    {
        ILog ErrorLog = LogManager.GetLogger("DBLogger");
        CitizenModel Model = new CitizenModel();
        private FmdssContext dbContext;
        public MISEncroachmentController()
        {
            this.dbContext = new FmdssContext();
        }

        List<SelectListItem> lstCircle = new List<SelectListItem>();
        List<SelectListItem> itemsRange = new List<SelectListItem>();
        List<SelectListItem> itemsDivision = new List<SelectListItem>();

        List<SelectListItem> lstStatus = new List<SelectListItem>();

        MISPurchaseHistory ObjMISPurchaseHistory = new MISPurchaseHistory();

        MISEncroachmentDetails ObjMISEncroachmentDetail = new MISEncroachmentDetails();

        // GET: /MISEncroachment/
        string status = string.Empty;

        public void statusDDL()
        {
            lstStatus.Add(new SelectListItem { Text = "ALL", Value = "ALL" });
            lstStatus.Add(new SelectListItem { Text = "Approve", Value = "Approve" });
            lstStatus.Add(new SelectListItem { Text = "Pending", Value = "Pending" });

            ViewBag.AppStatus = lstStatus;
        }
        public ActionResult MISEncroachmentDetails()
        {

            Session.Remove("DownloadMISEncroachmentDetailsExport");

            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                statusDDL();
                status = LoadData(UserID, "LOAD", "", "", "");

                ObjMISEncroachmentDetail.CircleStatus = Convert.ToString(status.Split(',')[0]);
                ObjMISEncroachmentDetail.DivisionStatus = Convert.ToString(status.Split(',')[1]);
                ObjMISEncroachmentDetail.RangeStatus = Convert.ToString(status.Split(',')[2]);


                // ObjMISProducationInventory.HIERARCHY_LEVEL_CODE = FPM_HIERARCHY_LEVEL(UserID);
                List<MISEncroachmentDetails> Obj = new List<MISEncroachmentDetails>();
                ViewData["ListMISEncroachmentDetailsInventory"] = Obj;



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, this.ControllerContext.RouteData.Values["action"].ToString() + "_" + this.ControllerContext.RouteData.Values["controller"].ToString(), 0, DateTime.Now, UserID);
            }

            return View(ObjMISEncroachmentDetail);

        }
        [HttpPost]
        public ActionResult MISEncroachmentDetails(MISEncroachmentDetails obj)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<MISEncroachmentDetails> ObjlistMISEncroachment = new List<MISEncroachmentDetails>();

            try
            {

                statusDDL();

                status = LoadData(UserID, "SUBMIT", obj.Circle, obj.Division, obj.Range);

                DataSet DT = new DataSet();
                DT = obj.GET_EncroachmentDetails(obj.Range, obj.Status, "List");
                int count = 1;

                Session["DownloadMISEncroachmentDetailsExport"] = DT.Tables[0];

                foreach (DataRow dr in DT.Tables[0].Rows)
                {
                    ObjlistMISEncroachment.Add(
                        new MISEncroachmentDetails()
                        {
                            Index = count,

                            EN_Code = Convert.ToString(dr["EN_Code"].ToString()),
                            NoticeNo = Convert.ToString(dr["NoticeNo"].ToString()),
                            NameAddress = Convert.ToString(dr["NameAddress"]),
                            Year = Convert.ToString(dr["Year"]),
                            EncrochedArea = Convert.ToString(dr["EncrochedArea"]),
                            DIV_NAME = Convert.ToString(dr["DIV_NAME"]),
                            Block_Name = Convert.ToString(dr["Block_Name"]),

                            CompartmentNo = Convert.ToString(dr["CompartmentNo"]),

                            RANGE_NAME = Convert.ToString(dr["RANGE_NAME"]),
                            LRACTNO = Convert.ToString(dr["LRACTNO"].ToString()),
                            KMLFile = Convert.ToString(dr["KMLFile"].ToString()),
                            DateOfKMLFile = Convert.ToString(dr["DateOfKMLFile"].ToString()),
                            InformationGatheredBy = Convert.ToString(dr["InformationGatheredBy"].ToString()),
                            InformationApprovedBy = Convert.ToString(dr["InformationApprovedBy"].ToString()),
                            //khasraNo = Convert.ToString(dr["khasraNo"]),
                            //EncrochArea = Convert.ToString(dr["EncrochArea"]),
                            //PaidawarOrKISMA = Convert.ToString(dr["PaidawarOrKISMA"]),
                            //TaxPerHact = Convert.ToString(dr["TaxPerHact"]),

                            //EncrochedPaidawar = Convert.ToString(dr["EncrochedPaidawar"]),
                            //TotalTax = Convert.ToString(dr["TotalTax"]),

                        });
                    count += 1;
                }

                ViewData["ListMISEncroachmentDetailsInventory"] = ObjlistMISEncroachment;

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return View(obj);

        }

        public ActionResult MISEncroachmentDetailsExport()
        {
            DataTable dtf = (DataTable)Session["DownloadMISEncroachmentDetailsExport"];



            if (dtf != null)
            {
                GridView gv = new GridView();
                gv.DataSource = dtf;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=MISEncroachmentDetails_" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return null;

        }

        public string LoadData(Int64 UserID, string TYPE, string Circle, string Division, string Range)
        {

            DataSet DS = new DataSet();
            DS = ObjMISPurchaseHistory.GET_USER_LEVEL(UserID);


            string CIRCLEAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[0].Rows)
            {
                if (Convert.ToString(dr["CIRCLE_CODE"]).ToLower() != "select")
                {
                    CIRCLEAllDataListCommanSprated += Convert.ToString(dr["CIRCLE_CODE"]) + ",";
                }
                lstCircle.Add(new SelectListItem { Text = dr["CIRCLE_NAME"].ToString(), Value = dr["CIRCLE_CODE"].ToString() });
            }
            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(CIRCLEAllDataListCommanSprated) && DS.Tables[0].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                lstCircle.Insert(1, itm);
                lstCircle[1].Value = CIRCLEAllDataListCommanSprated.Remove(CIRCLEAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion
            ViewBag.CIRCLE = lstCircle;


            string DivisionAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[1].Rows)
            {
                if (Convert.ToString(dr["DIV_NAME"]).ToLower() != "select" && Convert.ToString(dr["DIV_NAME"]).ToLower() != "all")
                {
                    DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                }
                itemsDivision.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
            }
            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && DS.Tables[1].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                itemsDivision.Insert(1, itm);
                itemsDivision[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion
            ViewBag.Division = itemsDivision;

            string RangeAllDataListCommanSprated = string.Empty;
            foreach (System.Data.DataRow dr in DS.Tables[2].Rows)
            {
                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                itemsRange.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
            }

            #region Set Comman Sprated Value in ALL field
            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && DS.Tables[2].Rows.Count > 1)
            {
                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                itemsRange.Insert(1, itm);
                itemsRange[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
            }
            #endregion
            ViewBag.Range = itemsRange;

            string ReturnData = string.Empty;
            ReturnData = DS.Tables[0].Rows.Count > 1 || Convert.ToString(DS.Tables[1].Rows[0][0]) == "SELECT" ? "False" : "True";
            ReturnData = ReturnData + "," + (DS.Tables[1].Rows.Count > 1 || Convert.ToString(DS.Tables[1].Rows[0][0]) == "SELECT" ? "False" : "True");
            ReturnData = ReturnData + "," + (DS.Tables[2].Rows.Count > 1 || Convert.ToString(DS.Tables[2].Rows[0][0]) == "SELECT" ? "False" : "True");

            string Code = Convert.ToString(DS.Tables[3].Rows[0][0]);

            if (TYPE == "SUBMIT")
            {

                if (Code == "CIR")
                {

                    List<SelectListItem> items = new List<SelectListItem>();

                    if ((!String.IsNullOrEmpty(Circle)))
                    {

                        if (Circle == "SELECT")
                        {
                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                            DataTable dt = Model.GetDivision(Convert.ToString(Circle));
                            DivisionAllDataListCommanSprated = string.Empty;

                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                                items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 2)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items.Insert(1, itm);
                                items[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                        ViewBag.Division = items;

                    }

                    List<SelectListItem> items1 = new List<SelectListItem>();
                    if ((!String.IsNullOrEmpty(Division)))
                    {
                        if (Division == "SELECT")
                        {
                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                            DataTable dt = Model.GetRange(Convert.ToString(Division));
                            RangeAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                                items1.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 2)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items1.Insert(1, itm);
                                items1[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                    }

                    ViewBag.Range = items1;
                }
                else if (Code == "DIV")
                {
                    List<SelectListItem> items1 = new List<SelectListItem>();
                    if ((!String.IsNullOrEmpty(Division)))
                    {
                        if (Division == "SELECT")
                        {
                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });

                            DataTable dt = Model.GetRange(Convert.ToString(Division));

                            RangeAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                                items1.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 2)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items1.Insert(1, itm);
                                items1[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                    }

                    ViewBag.Range = items1;

                }
                else if (Code == "RNG")
                {

                }
                else if (Code == "ST0")
                {
                    List<SelectListItem> items = new List<SelectListItem>();

                    if ((!String.IsNullOrEmpty(Circle)))
                    {

                        if (Circle == "SELECT")
                        {
                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                            DataTable dt = Model.GetDivision(Convert.ToString(Circle));
                            DivisionAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                DivisionAllDataListCommanSprated += Convert.ToString(dr["DIV_CODE"]) + ",";
                                items.Add(new SelectListItem { Text = dr["DIV_NAME"].ToString(), Value = dr["DIV_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(DivisionAllDataListCommanSprated) && dt.Rows.Count > 2)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items.Insert(1, itm);
                                items[1].Value = DivisionAllDataListCommanSprated.Remove(DivisionAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                        ViewBag.Division = items;

                    }

                    List<SelectListItem> items1 = new List<SelectListItem>();
                    if ((!String.IsNullOrEmpty(Division)))
                    {
                        if (Division == "SELECT")
                        {
                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });
                        }
                        else
                        {

                            items1.Add(new SelectListItem { Text = "SELECT", Value = "SELECT" });

                            DataTable dt = Model.GetRange(Convert.ToString(Division));

                            RangeAllDataListCommanSprated = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                RangeAllDataListCommanSprated += Convert.ToString(dr["RANGE_CODE"]) + ",";
                                items1.Add(new SelectListItem { Text = dr["RANGE_NAME"].ToString(), Value = dr["RANGE_CODE"].ToString() });
                            }
                            #region Set Comman Sprated Value in ALL field
                            if (!string.IsNullOrEmpty(RangeAllDataListCommanSprated) && dt.Rows.Count > 2)
                            {
                                SelectListItem itm = new SelectListItem() { Text = "ALL", Value = "All" };
                                items1.Insert(1, itm);
                                items1[1].Value = RangeAllDataListCommanSprated.Remove(RangeAllDataListCommanSprated.Length - 1, 1);
                            }
                            #endregion
                        }
                    }

                    ViewBag.Range = items1;
                }
            }

            return ReturnData;
        }

        public ActionResult GetApplicationDetails(string ids)
        {
            MISEncroachmentDetails obj = new MISEncroachmentDetails();

            StringBuilder SB = new StringBuilder();

            DataSet DS = new DataSet();
            DS = obj.GET_EncroachmentDetails(ids, "", "Details");

            if (DS.Tables.Count == 2)
            {
                Int16 index = 0;
                foreach (DataRow dr in DS.Tables[0].Rows)
                {

                    while (DS.Tables[0].Columns.Count > index)
                    {
                        if (DS.Tables[0].Columns[index].ColumnName.ToLower() == "kmlfile")
                        {
                            SB.Append("<tr><td>");
                            SB.Append(DS.Tables[0].Columns[index].ColumnName);
                            SB.Append("</td><td>");
                            SB.Append("<a href='" + DS.Tables[0].Rows[0][index].ToString() + "' target='_blank' rel = 'noopener noreferrer'>Download KML File</a>");
                            SB.Append("</td></tr>");
                        }
                        else
                        {

                            SB.Append("<tr><td>");
                            SB.Append(DS.Tables[0].Columns[index].ColumnName);
                            SB.Append("</td><td>");
                            SB.Append(DS.Tables[0].Rows[0][index].ToString());
                            SB.Append("</td></tr>");
                        }
                        index = Convert.ToInt16(index + 1);
                    }
                }
                SB.Append("<td colspan='2' style='font-size:14px;font-weight:bold' > ACF Decision </td>");


                SB.Append("<tr><td colspan='2'>");
                SB.Append("<table cellpadding='0'class='table table-striped table-bordered table-hover table-responsive'>");
                SB.Append("<thead><tr><th>Decision</th><th>Decision Date</th><th>Next Or Closed Date</th><th>Next Decision Place</th><th>Remarks</th><th>SSO ID</th></tr></thead>");
                SB.Append("<tbody>");
                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    SB.Append("<tr><td>" + Convert.ToString(dr["Decision"]) + "</td><td>" + Convert.ToString(dr["DecisionDate"]) + "</td><td>" + Convert.ToString(dr["NextOrClosedDate"]) + "</td><td>" + Convert.ToString(dr["Next_Decision_Place"]) + "</td><td>" + Convert.ToString(dr["Remarks"]) + "</td><td>" + Convert.ToString(dr["UserSSOID"]) + "</td></tr>");
                }
                SB.Append("</tbody></table>");
                SB.Append("</td></tr>");
            }


            ViewBag.List = SB.ToString();

            return PartialView("ApplicationNoCurrentDetails");
        }


        public ActionResult ZipDownload(string En_Code)
        {
            try
            {

                En_Code = Encryption.decrypt(En_Code);
                List<FileZip> lstZip = new List<FileZip>();


                Tbl_Encroachment tblEnch =  dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == En_Code);

                if (tblEnch != null && tblEnch.KMLFile != null && tblEnch.KMLFile.Length > 0)
                {
                    lstZip.Add(new FileZip
                    {
                        File = tblEnch.KMLFile,
                        FileName = tblEnch.KMLFileName,
                    });

                }

                if (lstZip.Count > 0)
                {
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + En_Code + ".zip");
                    Response.ContentType = "application/zip";

                    using (var zipStream = new ZipOutputStream(Response.OutputStream))
                    {
                        foreach (var item in lstZip)
                        {
                            byte[] fileBytes = item.File;
                            zipStream.PutNextEntry(item.FileName);
                            zipStream.Write(fileBytes, 0, fileBytes.Length);
                        }
                        zipStream.Flush();
                        zipStream.Close();

                    }
                    return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    TempData["ZipMsg"] = "File not exists";
                    return RedirectToAction("MISEncroachmentDetails", "MISEncroachment");
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Error(ex);
            }
            return null;
        } 
    }
}
