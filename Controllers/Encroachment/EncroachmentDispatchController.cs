

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

                var Tbl_Encroacher_DetailsWithCommaSaperate = dbContext.Tbl_Encroacher_Details
                    .GroupBy(e => e.EN_Code)
                    .ToList().Select(eg => new
                    {
                        EN_Code = eg.Key,
                        Encroacher_Name = string.Join(",", eg.Select(i => i.Encroacher_Name))
                    }).ToList();
               var Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord = Tbl_Encroacher_DetailsWithCommaSaperate.FirstOrDefault(v => v.EN_Code == EnchId);
                var EncroacherNames = Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord==null?"NA":Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord.Encroacher_Name;


                var result = (from a in dbContext.Tbl_Encroachment
                              join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                              join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                              join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                              join e in dbContext.Tbl_Encroach_InvestigationDetails on a.EN_Code equals e.EN_Code
							 // join f in Tbl_Encroacher_DetailsWithCommaSaperate.AsEnumerable() on a.EN_Code equals f.EN_Code
							  select new EncroachmentView
                              {                                  
                                  ACF_Status = a.ACF_Status,
                                  EN_Code = a.EN_Code,
                                  DIV_CODE = b.DIV_NAME,
                                  RANGE_CODE = c.RANGE_NAME,                               
                                  DispatchNo = a.DispatchNo,
                                  Encroacher_Name = EncroacherNames,                                  
                                  DOE = a.DOE,
                                  Description= a.Description,
                                  Year= e.Year,
                                  TypeofLand = e.TypeofLand,
                                  Encroachment_Area = e.Encroachment_Area,
                                  RateOfLagan= e.TaxPerHact,    
                                  Tax=e.Tax,
                                  Total_Area_Block=e.Total_Area_Block,
                                  khasraNo=e.khasraNo,
                                  CompartmentNo=e.CompartmentNo,
                                  InformationGatheredBy=e.InformationGatheredBy,
                                  InformationApprovedBy=e.InformationApprovedBy,
                                  NotificationNo=e.NotificationNo,
                                  NotificationDate=e.NotificationDate,
                                  Encroachment_Yield=e.Encroachment_Yield,
                                  Remarks=e.Remarks
                              }).ToList();
					
				//var res2 = (from a in Tbl_Encroacher_DetailsWithCommaSaperate
				//			join b in result on a.EN_Code equals b.EN_Code select new {
				//				ACF_Status = b.ACF_Status,
				//				EN_Code = b.EN_Code,
				//				DIV_CODE = b.DIV_CODE,
				//				RANGE_CODE = b.RANGE_CODE,
				//				DispatchNo = b.DispatchNo,
				//				Encroacher_Name = a.Encroacher_Name,                                  
				//				DOE = b.DOE,
				//				Description = b.Description,
				//				Year = b.Year,
				//				TypeofLand = b.TypeofLand,
				//				Encroachment_Area = b.Encroachment_Area,
				//				RateOfLagan = b.RateOfLagan,
				//				Tax =b.Tax,
    //                            Total_Area_Block=b.Total_Area_Block,
    //                            khasraNo=b.khasraNo,
    //                            CompartmentNo=b.CompartmentNo,
    //                            InformationGatheredBy=b.InformationGatheredBy,
    //                            InformationApprovedBy=b.InformationApprovedBy,
    //                            NotificationNo=b.NotificationNo,
    //                            NotificationDate=b.NotificationDate,
    //                            Encroachment_Yield=b.Encroachment_Yield,
    //                            Remarks=b.Remarks

    //                        }).ToList();
                var dispatch = result.Where(i => i.EN_Code.Equals(EnchId));               
                foreach (var item in dispatch)
                {
                    listEncroachment.Add(new EncroachmentView
                    {
                        EncroachmentId = item.EN_Code,
                        DIV_CODE = item.DIV_CODE,
                        RANGE_CODE = item.RANGE_CODE,
						Encroacher_Name = item.Encroacher_Name,
                        Description = item.Description,
                        DateOfEntry = item.DOE.ToString("dd/MM/yyyy"),
                        Year = item.Year,
                        TypeofLand = item.TypeofLand,
                        Encroachment_Area = item.Encroachment_Area,
                        TaxPerHact = item.RateOfLagan,
                        Tax = item.Tax,
                        DispatchNo = item.EN_Code.Replace("EN","D"),
                        Total_Area_Block=item.Total_Area_Block,
                        khasraNo=item.khasraNo,
                        CompartmentNo=item.CompartmentNo,
                        InformationGatheredBy=item.InformationGatheredBy,
                        InformationApprovedBy=item.InformationApprovedBy,
                        NotificationNo=item.NotificationNo,
                        NotificationDate=item.NotificationDate,
                        Encroachment_Yield=item.Encroachment_Yield,
                        Remarks=item.Remarks
                    });                   
                }
            }
            catch (Exception e) {
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
