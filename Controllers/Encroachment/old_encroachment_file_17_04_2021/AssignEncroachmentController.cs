
//---------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : AssignEncroachmentController
//  Description  : File contains details of Encroachment assignment
//  Date Created : 21-07-2017
//  Author       : Rajkumar
//---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.Encroachment.ViewModel;
using System.Data.SqlClient;
using FMDSS.Repository;
using Ionic.Zip;

namespace FMDSS.Controllers.Encroachment
{
    public class AssignEncroachmentController : Controller
    {
        private FmdssContext dbContext;       
        public AssignEncroachmentController() {

            dbContext = new FmdssContext();          
        }
        /// <summary>
        /// This function return list of unassigned encroachment 
        /// </summary>
        /// <returns></returns>
        public ActionResult AssignEncroachment()
        {
            try
            {
                Int64 UID = Convert.ToInt64(Session["UserID"]);
                List<EncroachmentView> listEncroachment = new List<EncroachmentView>();
                var designationId = Convert.ToString(Session["DesignationId"]);
                var query = (from a in dbContext.Tbl_Encroachment
                              join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                              join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                              join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID                                                   
                              select new
                              {                                 
                                   a.EN_Code,
                                   b.DIV_NAME,
                                   c.RANGE_NAME,
                                   a.LRACTNO,
                                   a.IsKnown,
                                   a.Area,
                                   d.Name,
                                   a.InvestigationOfficer, 
                                   a.DOE,
                                   a.EnteredBy
                              }).OrderByDescending(a => a.DOE).ToList().Where(t => t.EnteredBy == UID);

                //var result = query.Where(i => i.InvestigationOfficer == 0 && (designationId == "5" || designationId == "6"));
                foreach (var item in query)
                {
                    listEncroachment.Add(
                        new EncroachmentView
                        {
                            EncroachmentId = item.EN_Code,
                            DIV_CODE = item.DIV_NAME,
                            RANGE_CODE = item.RANGE_NAME,
                            LRACTNO = item.LRACTNO,
                            IsKnown = item.IsKnown,
                            Area = item.Area,
                            UserName = item.Name,
                            InvestigationOfficers = item.InvestigationOfficer,
                            DOE=item.DOE
                        });
                }              
                ViewData["AssignedEncroachment"] = listEncroachment;
                ViewBag.OfficerDesignation = new EncroachmentView().OfficerDesignation();
            }
            catch (Exception) {

                throw;
            }          
            return View();
        }
       
        /// <summary>
        /// This function returns list of forest office based on designation
        /// </summary>
        /// <param name="designation"></param>
        /// <returns></returns>
        public JsonResult GetForestOfficer(string designation)
        {           
            List<SelectListItem> lstOfficer = new List<SelectListItem>();
            try
             {
                 lstOfficer = new EncroachmentView().ListForestOfficer(designation);
             }
            catch (Exception)
            {
                throw;
            }
            return Json(new SelectList(lstOfficer, "Value", "Text"));
        }        
        /// <summary>
        /// function to forward offense to forester
        /// </summary>
        /// <param name="form"></param>
        /// <param name="Command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(Tbl_Encroachment objEnch, string Command)
        {          
            try
            {
                if (Command == "Forward")
                {                                     
                    if (Convert.ToString(objEnch.EN_Code) != string.Empty)
                    {
                        Tbl_Encroachment tbl_Ench = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == objEnch.EN_Code);
                        tbl_Ench.InvestigationOfficer = objEnch.InvestigationOfficer;
                        tbl_Ench.Special_Instruction = objEnch.Special_Instruction;
                        dbContext.SaveChanges();
                        TempData["UpdateMsg"] = "Encroachment assigned sucessfully";
                    }
                    else {
                        TempData["UpdateMsg"] = "Something went wrong please try after some time!!!";                    
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AssignEncroachment", "AssignEncroachment");
        }

        public ActionResult ZipDownload(string En_Code)
        {
            try
            {
                En_Code = Encryption.decrypt(En_Code);
                List<FileZip> lstZip = new List<FileZip>();
                Tbl_Encroachment tblEnch = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == En_Code);
                if (tblEnch != null && tblEnch.Acf_Decision_Upload != null && tblEnch.Acf_Decision_Upload.Length>0)
                {
                    lstZip.Add(new FileZip
                    {
                        File = tblEnch.Acf_Decision_Upload,
                        FileName = En_Code + ".pdf",
                    });

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
                        return RedirectToAction("Registration", "Encroachment");
                    }
                }
                else
                {

                    TempData["ZipMsg"] = "File not exists";
                    return RedirectToAction("AssignEncroachment", "AssignEncroachment");
                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }    
    }
}
