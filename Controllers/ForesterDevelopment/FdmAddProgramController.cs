
// *************************************************************************************
// Author : Rajkumar Singh
// Created : 31-Dec-2015
// *************************************************************************************
// <summary>This Controller is Created for Add Program in Forest Development</summary>
// *************************************************************************************
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.ForestDevelopment;
using FMDSS.Filters;
using System.Configuration;
using System.IO;

namespace FMDSS.Controllers.ForestDevelopment
{
    [MyAuthorization]
    public class FdmAddProgramController : BaseController
    {       
        /// <summary>
        /// Return view with previous entered program 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddProgram()
        {
            List<FdmProgram> Prgmlst = new List<FdmProgram>();
            try
            {
                DataSet dsPrgm = new DataSet();
                FdmProgram prgm = new FdmProgram();
                DataSet dsFund = new DataSet();
                List<SelectListItem> fundlst = new List<SelectListItem>();
                dsPrgm = prgm.GetProgram();
                foreach (System.Data.DataRow dr in dsPrgm.Tables[0].Rows)
                {
                    Prgmlst.Add(new FdmProgram
                    {
                        ID = Convert.ToInt64(dr["ID"].ToString()),
                        ProgramName = dr["Program_Name"].ToString(),
                        ProgramDesc = dr["Program_Desc"].ToString(),
                        StartDate1 = dr["StartDate"].ToString(),
                        EndDate1 = dr["Enddate"].ToString(),
                        Extended_ToDate1 = dr["Extended_Date"].ToString(),
                        FundingAgency = dr["Funding_Agency"].ToString(),
                        Terms_Ref_Doc = dr["Terms_Ref_Document"].ToString(),
                        Revised_Ref_Doc = dr["Revised_Ref_Document"].ToString(),
                    });
                }
                dsFund = prgm.BindFAgency();
                foreach (System.Data.DataRow dr in dsFund.Tables[0].Rows)
                {
                    fundlst.Add(new SelectListItem { Text = dr["AgencyName"].ToString(), Value = dr["ID"].ToString() });
                }
                ViewBag.Funding = fundlst;
              //  ViewData["Programlist"] = Prgmlst;
            }
            catch (Exception ex) {
                Console.Write("Error " + ex);
            }
            return View(Prgmlst);
        }
        /// <summary>
        /// Button click to submit data
        /// </summary>
        /// <param name="frm"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult btnSave(FormCollection frm, string Command, HttpPostedFileBase Term_Ref_Doc, HttpPostedFileBase Revised_ref_doc)
        {
            try
            {
                string UploadDoc = string.Empty;
                string FilePath = ConfigurationManager.AppSettings["KMLfileUploadPath"].ToString();
                var path = "";
                if (Command == "save") {
                    FdmProgram prgm = new FdmProgram();
                    prgm.ID = Convert.ToInt64(frm["hdn_id"].ToString());
                    prgm.ProgramName = frm["PrgmName"].ToString();
                    prgm.ProgramDesc = frm["PrgmDesc"].ToString();
                    prgm.FundingAgency = Request.Form["dropFAgency"].ToString();
                    if (Term_Ref_Doc != null && Term_Ref_Doc.ContentLength > 0)
                    {
                        UploadDoc = System.IO.Path.GetFileName(Term_Ref_Doc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + UploadDoc;
                        path = Path.Combine(FilePath, FileFullName);
                        prgm.Terms_Ref_Doc = path;
                        Term_Ref_Doc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        prgm.Revised_Ref_Doc = "";
                    }
                    prgm.StartDate = DateTime.ParseExact(frm["Startdate"].ToString(), "dd/MM/yyyy", null);
                    prgm.EndDate = DateTime.ParseExact(frm["Enddate"].ToString(), "dd/MM/yyyy", null);                   
                  
                    Int64 status = prgm.InsertProgram();
                }
                if (Command == "update") {
                    FdmProgram prgm = new FdmProgram();
                    prgm.ID = Convert.ToInt64(frm["hdn_id"].ToString());
                    if (Revised_ref_doc != null && Revised_ref_doc.ContentLength > 0)
                    {
                        UploadDoc = System.IO.Path.GetFileName(Revised_ref_doc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + UploadDoc;
                        path = Path.Combine(FilePath, FileFullName);
                        prgm.Revised_Ref_Doc = path;
                        Revised_ref_doc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        prgm.Revised_Ref_Doc = "";
                    }
                    if (!string.IsNullOrWhiteSpace(frm["ExtendedDate"].ToString()))
                    {
                        prgm.Extended_ToDate = DateTime.ParseExact(frm["ExtendedDate"].ToString(), "dd/MM/yyyy", null);
                    }
                    else
                    {
                        prgm.Extended_ToDate = null;
                    }
                     Int64 status =prgm.UpdateProgram();
                }
            }
            catch (Exception ex) {
                Console.Write("Error " + ex);
            }
            return RedirectToAction("AddProgram", "FdmAddProgram");
        }
        [HttpPost]
        public JsonResult Edit(string ProgramId)
        {

            FdmProgram FP = null;
            FdmProgram FPobj = new FdmProgram();
            DataSet ds = new DataSet();
            FPobj.ID = Convert.ToInt64(ProgramId);
            ds = FPobj.EditProgram();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FP = new FdmProgram();
                    FP.ProgramName = ds.Tables[0].Rows[0][0].ToString();
                    FP.ProgramDesc = ds.Tables[0].Rows[0][1].ToString();
                    FP.FundingAgency = ds.Tables[0].Rows[0][2].ToString();
                    FP.Terms_Ref_Doc = ds.Tables[0].Rows[0][3].ToString();
                    DateTime _date1 = DateTime.Parse(ds.Tables[0].Rows[0][4].ToString());
                    DateTime _date2 = DateTime.Parse(ds.Tables[0].Rows[0][5].ToString());
                    FP.StartDate1 = _date1.ToString("dd/MM/yyyy");
                    FP.EndDate1 = _date2.ToString("dd/MM/yyyy");
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0][6].ToString()))
                    {
                        DateTime _date3 = DateTime.Parse(ds.Tables[0].Rows[0][6].ToString());
                        FP.Extended_ToDate1 = _date3.ToString("dd/MM/yyyy");
                    }
                    else {
                        FP.Extended_ToDate1 = "";
                    }                                                            
                    FP.Revised_Ref_Doc = ds.Tables[0].Rows[0][7].ToString();                  
                }
            }
            return Json(FP, JsonRequestBehavior.AllowGet);

        }

    }
}
