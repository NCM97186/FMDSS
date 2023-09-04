

//---------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : EncroachmentDispatchController
//  Description  : File contains details of Encroachment dispatch
//  Date Created : 21-07-2017
//  Author       : Ashok 
//---------------------------------------------------------------

  
namespace FMDSS.Controllers.Encroachment
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.IO;
    using AutoMapper;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using FMDSS.Models.Encroachment.DomainModel;
    using FMDSS.Models.Encroachment.ViewModel;
    using FMDSS.Repository;
    using FMDSS.Models.FmdssContext;
    using System.Globalization;

    public class EncroachmentDispatchController : Controller
    {      
        private FmdssContext dbContext;        
        /// <summary>
        /// Default constructor
        /// </summary>
        public EncroachmentDispatchController()
        {
            dbContext = new FmdssContext();        
        }
    
        /// <summary>
        /// This function returns list for dispatch
        /// </summary>
        /// <returns></returns>
        public ActionResult DispatchCaseList()
        {
            List<EncroachmentView> listEncroachment = new List<EncroachmentView>();
            var ssoid = Session["SSOid"].ToString();
            try
            {
                /*----------------Implemented using entity framework-------------------------------------------*/               
                /* var encodeList = dbContext.Tbl_Encroach_InvestigationDetails.Where(x => x.EN_Code != null).Select(e => e.EN_Code).ToList();
                var query = (from a in dbContext.Tbl_Encroachment
                             join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                             join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                             join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                             join e in dbContext.tbl_mst_ForestEmployees on a.InvestigationOfficer equals e.ROWID
                             select new
                             {
                                 InvestigationOfficer = a.InvestigationOfficer,
                                 ACF_Status = a.ACF_Status,
                                 EN_Code = a.EN_Code,
                                 DIV_CODE = b.DIV_NAME,
                                 RANGE_CODE = c.RANGE_NAME,
                                 IsKnown = a.IsKnown,
                                 LRACTNO = a.LRACTNO,
                                 DispatchNo = a.DispatchNo,
                                 UserName = d.Name,
                                 Area = a.Area,
                                 DOE = a.DOE,
                                 SSOId = e.SSO_ID
                             }).ToList();

                var whereQuery = query.Where(x => x.DispatchNo == null && Convert.ToString(x.SSOId).ToUpper().Equals(ssoid.ToUpper()));
                var result = from b in whereQuery where encodeList.Contains(b.EN_Code) select b;
                foreach (var item in result)
                {
                    listEncroachment.Add(
                        new EncroachmentView
                        {
                            EncroachmentId = item.EN_Code,
                            DIV_CODE = item.DIV_CODE,
                            RANGE_CODE = item.RANGE_CODE,
                            IsKnown = item.IsKnown,
                            Area = item.Area,
                            LRACTNO = item.LRACTNO,
                            DOE = item.DOE

                        });
                }
                ViewData["SubmittedCase"] = listEncroachment;*/              
                ViewData["SubmittedCase"] = new EncroachmentView().EncroachmentList("Dispatch");
            }
            catch (Exception) {

                throw;
            }
            return View();
        }
       
       /// <summary>
       /// This function returns list of details in json format
       /// </summary>
       /// <param name="EnchId"></param>
       /// <returns></returns>
        public ActionResult DispatchDetails(string EnchId)
        {
            List<EncroachmentView> listEncroachment = new List<EncroachmentView>();            
            try
            {
                var userid = Session["UserId"].ToString();
                var result = (from a in dbContext.Tbl_Encroachment
                              join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                              join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                              join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                              join e in dbContext.Tbl_Encroach_InvestigationDetails on a.EN_Code equals e.EN_Code
                              select new
                              {                                  
                                  ACF_Status = a.ACF_Status,
                                  EN_Code = a.EN_Code,
                                  DIV_CODE = b.DIV_NAME,
                                  RANGE_CODE = c.RANGE_NAME,                               
                                  DispatchNo = a.DispatchNo,
                                  UserName = d.Name,                                  
                                  DOE = a.DOE,
                                  Description= a.Description,
                                  Year= e.Year,
                                  TypeofLand = e.TypeofLand,
                                  EncrochedArea = e.Encroachment_Area,
                                  RateOfLagan= e.TaxPerHact,    
                                  Tax=e.Tax
                              }).ToList();
              
                var dispatch = result.Where(i => i.EN_Code.Equals(EnchId));               
                foreach (var item in dispatch)
                {
                    listEncroachment.Add(new EncroachmentView
                    {
                        EncroachmentId = item.EN_Code,
                        DIV_CODE = item.DIV_CODE,
                        RANGE_CODE = item.RANGE_CODE,
                        UserName = item.UserName,
                        Description = item.Description,
                        DateOfEntry = item.DOE.ToString("dd/MM/yyyy"),
                        Year = item.Year,
                        TypeofLand = item.TypeofLand,
                        Encroachment_Area = item.EncrochedArea,
                        TaxPerHact = item.RateOfLagan,
                        Tax = item.Tax,
                        DispatchNo = item.EN_Code.Replace("EN","D")
                    });                   
                }
            }
            catch (Exception) {
                throw;
            }
            return Json(listEncroachment, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Final updation for dispatch
        /// </summary>
        /// <param name="encroachmentView"></param>
        /// <returns></returns>
        public ActionResult UpdateforDispatch(EncroachmentView encroachmentView)
        {
            try
            {
                if (encroachmentView != null)
                {
                    Tbl_Encroachment t = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == encroachmentView.EncroachmentId);
                    t.DispatchNo = encroachmentView.DispatchNo;
                    t.DispatchDate = DateTime.Now;
                    dbContext.SaveChanges();
                    TempData["DispatchMsg"] = "Sucessfully dispatch for Encroachment: " + encroachmentView.EncroachmentId;
                }
                else {
                    TempData["DispatchMsg"] = "Something went wrong please try after some time!!!";                
                }
            }
            catch (Exception) {
                throw;
            }
            return this.RedirectToAction("DispatchCaseList", "EncroachmentDispatch");
        }
       
    }
}
