using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.Encroachment.ViewModel;
using FMDSS.Repository;
using System.Data.SqlClient;
using AutoMapper;
using System.IO;

namespace FMDSS.Controllers.Encroachment
{
    public class ACFAppearanceController : Controller
    {
         private FmdssContext dbContext;
        /// <summary>
        /// This function is default Constructor
        /// </summary>
         public ACFAppearanceController()
         {
            dbContext = new FmdssContext();
         }
        /// <summary>
        /// This function returns list of acf decision pending 
        /// </summary>
        /// <returns></returns>
        public ActionResult ACFAppearance()
        {            

            List<EncroachmentView> listEnAppearance = new List<EncroachmentView>();
            try
            {
                /*----------------Implemented using entity framework-------------------------------------------*/

               // var duplicateEncode = dbContext.Tbl_Encroach_Appearance.Where(x => x.Decision_Taken != null && x.Decision_Taken == "Closed").Select(e => e.EN_Code).ToList();
                /* var query = (from a in dbContext.Tbl_Encroachment
                              join c in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals c.DIV_CODE
                              join d in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals d.RANGE_CODE
                              join e in dbContext.tbl_UserProfiles on a.EnteredBy equals e.UserID
                              select new
                              {
                                  EN_Code = a.EN_Code,
                                  DIV_CODE = c.DIV_NAME,
                                  RANGE_CODE = d.RANGE_NAME,
                                  UserName = e.Name,
                                  DOE = a.DOE,
                                  DispatchNo = a.DispatchNo,
                                  DispatchDate = a.DispatchDate,
                                  ACF_Status = a.ACF_Status,
                                  ACF_Remarks = a.ACF_Remarks,
                                  NoticeNo = a.NoticeNo,
                                  NoticeDate = a.NoticeDate,
                                  Final_Decision_Taken= a.Final_Decision_Taken,
                                  Final_Decision_Date=a.Final_Decision_Date,
                                  Next_Decision_Date=  a.Next_Decision_Date
                              }).ToList();

                 var result = query.Where(i => (i.ACF_Status != null && i.ACF_Status == "Approve") 
                              && (i.Final_Decision_Taken == null || i.Final_Decision_Taken != "Closed")).ToList();                
                 //var result = from b in whereQuery
                 //             join d in dbContext.Tbl_Encroach_Appearance on b.EN_Code equals d.EN_Code
                 //             where !duplicateEncode.Contains(b.EN_Code)
                 //             select b;
                 foreach (var item in result)
                 {
                     listEnAppearance.Add(
                                 new EncroachmentView
                                 {
                                     EncroachmentId = item.EN_Code,
                                     DIV_CODE = item.DIV_CODE,
                                     RANGE_CODE = item.RANGE_CODE,
                                     UserName = item.UserName,
                                     DOE = item.DOE,
                                     DispatchNo = item.DispatchNo,
                                     DispatchDate = item.DispatchDate,
                                     ACF_Status=item.ACF_Status,
                                     NoticeNo = item.NoticeNo,
                                     NoticeDate = item.NoticeDate,
                                     Decision_Taken = item.Final_Decision_Taken,
                                     Decision_Date=item.Final_Decision_Date,
                                     Next_Date = item.Next_Decision_Date
                                 });
                 }

                // ViewData["AppearanceList"] = listEnAppearance;*/                
                ViewData["AppearanceList"] = new EncroachmentView().EncroachmentList("Appearance");
            }
            catch (Exception) { throw; }
            return View();
        }      
        /// <summary>
        /// Final submission for ACF decision
        /// </summary>
        /// <param name="objDecision"></param>
        /// <returns></returns>
        public ActionResult Submit(EncroachAppearanceView objAppearance, HttpPostedFileBase AcfDecisionFiles)
        {
            try
            {
                if (objAppearance != null)
                {
                    if (AcfDecisionFiles != null && AcfDecisionFiles.ContentLength > 0)
                    {
                        string document = string.Empty;
                        var path = string.Empty;
                        Guid fileGenName = Guid.NewGuid();
                        document = Path.GetFileName(AcfDecisionFiles.FileName);
                        string FileFullName = objAppearance.Decision_Taken == "Nextdate" ? objAppearance.EN_Code + objAppearance.Decision_Taken + "_" + objAppearance.Next_Date.ToString().Substring(0, 10).Replace('/', '-') + "_" + document : objAppearance.EN_Code + objAppearance.Decision_Taken + "_" + DateTime.Now.ToString("dd-MM-yyyy") + "_" + document;
                        path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, AcfDecisionFiles);
                        byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                        objAppearance.Acf_Decision_Upload = filedata;
                        objAppearance.Acf_Decision_UploadFileName = FileFullName;
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    else
                    {
                        objAppearance.Acf_Decision_Upload = null;
                    }

                    objAppearance.Entered_By = Convert.ToInt64(Session["UserId"]);


                     //objAppearance.Next_Date = objAppearance.Next_Date  08/10/2017

                    System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");
                    if (objAppearance.Decision_Taken == "Nextdate")
                    { 

                    DateTime dt = Convert.ToDateTime(objAppearance.Next_Date, cul);
                    objAppearance.Next_Date = dt.ToString();

                    }
                    objAppearance.Decision_Date = DateTime.Today;
                    Mapper.CreateMap<EncroachAppearanceView, Tbl_Encroach_Appearance>();
                    Tbl_Encroach_Appearance ObjUserModel = Mapper.Map<EncroachAppearanceView, Tbl_Encroach_Appearance>(objAppearance);
                    dbContext.Tbl_Encroach_Appearance.Add(ObjUserModel);
                    dbContext.SaveChanges();

                    Tbl_Encroachment t = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == ObjUserModel.EN_Code);
                    t.Final_Decision_Taken = ObjUserModel.Decision_Taken;
                    t.Final_Decision_OfficerId = ObjUserModel.Entered_By;
                    t.Final_Decision_Remarks = ObjUserModel.Decision_Description;
                    t.Acf_Decision_Upload = ObjUserModel.Acf_Decision_Upload;
                  
                    t.Final_Decision_Date = DateTime.Today;

                    if (ObjUserModel.Decision_Taken=="Nextdate")
                    {
                        t.Next_Decision_Date = ObjUserModel.Next_Date;
                        t.Next_Decision_Place = ObjUserModel.Next_Decision_Place;
                    }                   
                    dbContext.SaveChanges();

                    TempData["AppearanceMsg"] = "Record updated successfully for Encroachment Id:" + objAppearance.EN_Code;

                }
                else {

                    TempData["AppearanceMsg"] = "Something went wrong please try after sometime!!!";

                }
            }
            catch (Exception) {

                throw;
            }
            return RedirectToAction("ACFAppearance", "ACFAppearance");
        }

        /// <summary>
        /// This function shows details based on encroachment id
        /// </summary>
        /// <param name="EnchId"></param>
        /// <returns></returns>
        public ActionResult ViewDetails(string EnchId)
        {
            List<EncroachmentView> listView = new List<EncroachmentView>();           
            var query = (from a in dbContext.Tbl_Encroachment
                         join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                         join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                         join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID                        
                         select new
                         {                            
                             EN_Code = a.EN_Code,
                             DIV_CODE = b.DIV_NAME,
                             RANGE_CODE = c.RANGE_NAME,
                             IsKnown = a.IsKnown,
                             LRACTNO = a.LRACTNO,
                             DispatchNo = a.DispatchNo,
                             DispatchDate = a.DispatchDate,
                             UserName = d.Name,
                             Area = a.Area,
                             DOE = a.DOE,
                             ACF_Status=a.ACF_Status,
                             ACF_Remarks=a.ACF_Remarks,
                             NoticeNo=a.NoticeNo,
                             NoticeDate=a.NoticeDate
                         }).ToList();
            var result = query.Where(i => i.EN_Code.Equals(EnchId));
            foreach (var item in result)
            {
                listView.Add(new EncroachmentView
                {
                    EncroachmentId = item.EN_Code,
                    DIV_CODE = item.DIV_CODE,
                    RANGE_CODE = item.RANGE_CODE,
                    IsKnown = item.IsKnown,
                    Area = item.Area,
                    LRACTNO = item.LRACTNO,                 
                    DispatchNo = item.DispatchNo,
                    Dispatch_Date = item.DispatchDate.ToString(),
                    ACF_Status=item.ACF_Status,
                    ACF_Remarks=item.ACF_Remarks,
                    NoticeNo=item.NoticeNo,
                    Notice_Date =item.NoticeDate.ToString(),
                    DateOfEntry = item.DOE.ToString("dd/MM/yyyy"),
                });
            }
            return Json(listView, JsonRequestBehavior.AllowGet);
        }       
    }
}
