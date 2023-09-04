
//---------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : ACFDecisionController
//  Description  : File contains details of ACF Decision
//  Date Created : 26-07-2017
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
using FMDSS.Models;
using System.IO;
using Ionic.Zip;
using FMDSS.Models.MIS;


namespace FMDSS.Controllers.Encroachment
{
    public class ACFDecisionController : Controller
    {
        private FmdssContext dbContext;
        /// <summary>
        /// This function is default Constructor
        /// </summary>
        public ACFDecisionController()
        {

            dbContext = new FmdssContext();
        }
        /// <summary>
        /// This function returns list of acf decision pending 
        /// </summary>
        /// <returns></returns>
        public ActionResult ACFDecision()
        {
            List<EncroachmentView> listEnDecision = new List<EncroachmentView>();
            try
            {
                /*----------------Implemented using entity framework-------------------------------------------*/
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
                                   ACF_Status=a.ACF_Status,
                                   ACF_Remarks=a.ACF_Remarks,                                 
                               }).OrderByDescending(i=>i.DOE).ToList();
                 //var result = query.Where(i => (i.DispatchNo != null && i.DispatchNo != string.Empty) && (i.ACF_Status==null || i.ACF_Status==string.Empty)).ToList();
                 var result = query.Where(i =>i.DispatchNo != null && i.DispatchNo != string.Empty).ToList();                
                 foreach (var item in result)
                 {
                     listEnDecision.Add(
                                 new EncroachmentView
                                 {
                                     EncroachmentId = item.EN_Code,
                                     DIV_CODE = item.DIV_CODE,
                                     RANGE_CODE = item.RANGE_CODE,
                                     UserName = item.UserName,
                                     DOE = item.DOE,
                                     DispatchNo = item.DispatchNo,
                                     DispatchDate = item.DispatchDate,
                                     ACF_Status=item.ACF_Status
                                 });
                 }
                 ViewData["DecisionList"] = listEnDecision; */
                ViewData["DecisionList"] = new EncroachmentView().EncroachmentList("Decision");
            }
            catch (Exception)
            {
                throw;
            }
            return View();

        }

        /// <summary>
        /// Final submission for ACF decision
        /// </summary>
        /// <param name="objDecision"></param>
        /// <returns></returns>
        public ActionResult Submit(EncroachmentView objDecision)
        {
            try
            {
                if (objDecision != null)
                {
                    //if (AcfDecisionFiles != null && AcfDecisionFiles.ContentLength > 0)
                    //{
                    //    string document = string.Empty;
                    //    var path = string.Empty;
                    //    Guid fileGenName = Guid.NewGuid();
                    //    document = Path.GetFileName(AcfDecisionFiles.FileName);
                    //    string FileFullName = DateTime.Now.Ticks + "_" + document;
                    //    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, AcfDecisionFiles);
                    //    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    //    objDecision.Acf_Decision_Upload = filedata;
                    //    FileInfo file = new FileInfo(path);
                    //    if (file.Exists)
                    //    {
                    //        file.Delete();
                    //    }
                    //}
                    //else
                    //{
                    objDecision.Acf_Decision_Upload = null;
                    //}
                    Tbl_Encroachment tbl = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == objDecision.EncroachmentId);
                    tbl.ACF_Status = objDecision.ACF_Status;
                    tbl.ACF_Remarks = objDecision.ACF_Remarks;
                    tbl.ACF_Date = DateTime.Now;
                    tbl.Acf_Decision_Upload = objDecision.Acf_Decision_Upload;
                    if (Convert.ToString(tbl.ACF_Status) == "Approve")
                    {
                        tbl.NoticeNo = objDecision.NoticeNo;
                        tbl.NoticeDate = DateTime.Now;
                    }
                    dbContext.SaveChanges();
                    TempData["DecisionMsg"] = "Record updated successfully for Encroachment Id:" + objDecision.EncroachmentId;
                }
                else
                {
                    TempData["DecisionMsg"] = "Something went wrong please try after sometime!!!";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("ACFDecision", "ACFDecision");
        }

        /// <summary>
        /// This function shows details based on encroachment id
        /// </summary>
        /// <param name="EnchId"></param>
        /// <returns></returns>
        public ActionResult ViewDetails(string EnchId)
        {
            List<EncroachmentView> listView = new List<EncroachmentView>();
            var userid = Session["UserId"].ToString();
            var Tbl_Encroacher_DetailsWithCommaSaperate = dbContext.Tbl_Encroacher_Details.Where(x => x.EN_Code == EnchId)
                   .GroupBy(e => e.EN_Code)
                   .ToList().Select(eg => new
                   {
                       EN_Code = eg.Key,
                       Encroacher_Name = string.Join(",", eg.Select(i => i.Encroacher_Name))
                   }).FirstOrDefault();
            var Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord = Tbl_Encroacher_DetailsWithCommaSaperate == null ? "NA" : Tbl_Encroacher_DetailsWithCommaSaperate.Encroacher_Name;
            var query = (from a in dbContext.Tbl_Encroachment
                         join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                         join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                         join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                         join e in dbContext.Tbl_Encroach_InvestigationDetails on a.EN_Code equals e.EN_Code
                         select new
                         {
                             //b.DIV_CODE
                             //e.Encroacher_Aadhar
                             //e.Encroacher_Bhamashah
                             //e.Encroachment_Area
                             //e.AadharOrBhamasha
                             //e.khasraNo

                             InvestigationOfficer = a.InvestigationOfficer,
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
                             Description = a.Description,
                             Year = e.Year,
                             TypeofLand = e.TypeofLand,
                             EncrochedArea = e.Encroachment_Area,
                             RateOfLagan = e.TaxPerHact,
                             Tax = e.Tax,
                             NoticeNo = a.NoticeNo
                             
                         }).ToList();
            var result = query.Where(i => i.EN_Code.Equals(EnchId));
            foreach (var item in result)
            {
                listView.Add(new EncroachmentView
                {
                    EncroachmentId = item.EN_Code,
                    DIV_CODE = item.DIV_CODE,
                    RANGE_CODE = item.RANGE_CODE,
                    UserName = item.UserName,
                    DateOfEntry = item.DOE.ToString("dd/MM/yyyy"),
                    Year = item.Year,
                    TypeofLand = item.TypeofLand,
                    Encroachment_Area = item.EncrochedArea,
                    TaxPerHact = item.RateOfLagan,
                    Tax = item.Tax,
                    DispatchNo = item.DispatchNo,
                    Dispatch_Date = Convert.ToString(item.DispatchDate),
                    NoticeNo = item.EN_Code.Replace("EN", "N"),
                    Encroacher_Name= Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord
                });
            }
            return Json(listView, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult RegDetails(string EnchId)
        {
            var Tbl_Encroacher_DetailsWithCommaSaperate = dbContext.Tbl_Encroacher_Details.Where(x=>x.EN_Code==EnchId)
					.GroupBy(e => e.EN_Code)
					.ToList().Select(eg => new
					{
						EN_Code = eg.Key,					
						Encroacher_Name = string.Join(",", eg.Select(i => i.Encroacher_Name))
					}).FirstOrDefault();
            var Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord = Tbl_Encroacher_DetailsWithCommaSaperate == null ? "NA" : Tbl_Encroacher_DetailsWithCommaSaperate.Encroacher_Name;
            var query = (from a in dbContext.Tbl_Encroachment
                         join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                         join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                         //join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                         join e in dbContext.Tbl_Encroach_InvestigationDetails on a.EN_Code equals e.EN_Code
                         join f in dbContext.tbl_AD_District_Maping on a.EN_Code equals f.RequestedID
                         where a.EN_Code == EnchId
                         select new EncroachmentView
                         {
                             EN_Code=a.EN_Code,
                             DIV_CODE=b.DIV_NAME,
                             RANGE_CODE=c.RANGE_NAME,
                             Near_by_area=f.Area,
                             Block=e.Block_Name,
                             khasraNo=e.khasraNo,
                             TypeofLand= e.TypeofLand,
                             TaxPerHact = e.TaxPerHact,
                             TotalArea=e.TotalArea,
                             Encroachment_Area=e.Encroachment_Area,
                             DOE=a.DOE,
                             Encroacher_Name= Tbl_Encroacher_DetailsWithCommaSaperateSingleRecord,
                             LRACTNO=a.LRACTNO,
                             Year=e.Year,
                             DispatchNo=a.DispatchNo,
                             DispatchDate=a.DispatchDate,
                             Description=a.Description,
                             Special_Instruction = a.Special_Instruction,
                             NoticeNo = a.NoticeNo,
                             NoticeDate=a.NoticeDate
                         }).FirstOrDefault();


            //var query = (from a in dbContext.Tbl_Encroachment
            //             join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
            //             join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
            //             join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
            //             join e in dbContext.Tbl_Encroach_InvestigationDetails on a.EN_Code equals e.EN_Code
            //             select new
            //             {
            //                 InvestigationOfficer = a.InvestigationOfficer,
            //                 EN_Code = a.EN_Code,
            //                 DIV_CODE = b.DIV_NAME,
            //                 RANGE_CODE = c.RANGE_NAME,
            //                 IsKnown = a.IsKnown,
            //                 LRACTNO = a.LRACTNO,
            //                 DispatchNo = a.DispatchNo,
            //                 DispatchDate = a.DispatchDate,
            //                 UserName = d.Name,
            //                 Area = a.Area,
            //                 DOE = a.DOE,
            //                 Description = a.Description,
            //                 Year = e.Year,
            //                 TypeofLand = e.TypeofLand,
            //                 EncrochedArea = e.Encroachment_Area,
            //                 RateOfLagan = e.TaxPerHact,
            //                 Tax = e.Tax,
            //                 NoticeNo = a.NoticeNo
            //             }).ToList();
            //var result = query.Where(i => i.EN_Code.Equals(EnchId));

            //var vi = result.Select(x => new
            //{
            //    EncroachmentId = x.EN_Code,
            //    DIV_CODE = x.DIV_CODE,
            //    RANGE_CODE = x.RANGE_CODE,
            //    UserName = x.UserName,
            //    DateOfEntry = x.DOE.ToString("dd/MM/yyyy"),
            //    Year = x.Year,
            //    TypeofLand = x.TypeofLand,
            //    Encroachment_Area = x.EncrochedArea,
            //    TaxPerHact = x.RateOfLagan,
            //    Tax = x.Tax,
            //    DispatchNo = x.DispatchNo,
            //    Dispatch_Date = Convert.ToString(x.DispatchDate),
            //    NoticeNo = x.EN_Code.Replace("EN", "N"),

            //    InvestigationOfficer = x.InvestigationOfficer,
            //    Description = x.Description,
            //    IsKnown = x.IsKnown,
            //    LRACTNO = x.LRACTNO,
            //    Area = x.Area
            //}).FirstOrDefault();

            //commented by shaan 19-04-2021
            //var param1 = new SqlParameter("@EN_Code", EnchId);
            //var result =  FMDSS.Models.Utility.DynamicSqlQuery(dbContext.Database, "spEncrocherDetailById @EN_Code", param1);
            //var result = dbContext.Database.SqlQuery<object>("spEncrocherDetailById @EN_Code", param1).ToList();
            return Json(query, JsonRequestBehavior.AllowGet); 
        }

        public ActionResult GetNotice(string encode)
        {

            try
            {
                List<EncroacherDetailsView> listView = new List<EncroacherDetailsView>();
                object[] xparams ={                            
                             new SqlParameter("@EN_Code", encode)};

                var result = dbContext.Database.SqlQuery<string>("dbo.SP_EncroachmentNotice @EN_Code", xparams);

                var query = (from a in dbContext.Tbl_Encroachment
                             join b in dbContext.Tbl_Encroacher_Details on a.EN_Code equals b.EN_Code
                             select new
                             {
                                 a.EN_Code,
                                 b.Encroacher_Name,
                                 b.Encroacher_Address
                             }).ToList();
                var eResult = query.Where(i => i.EN_Code.Equals(encode));
                foreach (var item in eResult)
                {

                    listView.Add(new EncroacherDetailsView
                    {
                        Encroacher_Name = item.Encroacher_Name,
                        Encroacher_Address = item.Encroacher_Address
                    });
                }
                return Json(new { list1 = Convert.ToString(result.FirstOrDefault()), list2 = listView }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

            return null;
        }

        public ActionResult ZipDownload(string En_Code)
        {
            try
            {
                En_Code = Encryption.decrypt(En_Code);
                List<FileZip> lstZip = new List<FileZip>();

                Tbl_Encroachment tblEnch = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == En_Code);
                tbl_mst_ForestEmployees Objtbl_mst_ForestEmployees = dbContext.tbl_mst_ForestEmployees.FirstOrDefault(i => i.ROWID == tblEnch.InvestigationOfficer);
                tbl_UserProfiles tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.Ssoid == Objtbl_mst_ForestEmployees.SSO_ID);
                Tbl_Encroach_InvestigationDetails tblInvest = dbContext.Tbl_Encroach_InvestigationDetails.FirstOrDefault(i => i.EN_Code == En_Code);

                IEnumerable<Tbl_Encroacher_Details> tblEncroacher_Details = dbContext.Tbl_Encroacher_Details.Where(i => i.EN_Code == En_Code).ToList();

                tbl_mst_Forest_Divisions tbldiv = dbContext.tbl_mst_Forest_Divisions.FirstOrDefault(i => i.DIV_CODE == tblEnch.DIV_CODE);

                string[] ArrAttachments = new string[7];



                if (tblEnch != null && tblEnch.KMLFile != null && tblEnch.KMLFile.Length > 0)
                {
                    lstZip.Add(new FileZip
                    {
                        File = tblEnch.KMLFile,
                        FileName = tblEnch.KMLFileName,
                    });
                    ArrAttachments[0] = "Kml File";
                }
                else
                {
                    ArrAttachments[0] = string.Empty;
                }

                if (tblInvest != null)
                {
                    if (tblInvest.DocumentUpload != null && tblInvest.DocumentUpload.Length > 0)
                    {
                        lstZip.Add(new FileZip
                        {
                            File = tblInvest.DocumentUpload,
                            FileName = tblInvest.UploadFileName,
                        });
                        ArrAttachments[1] = "Adoc Document File";
                    }
                    else
                    {
                        ArrAttachments[1] = string.Empty;
                    }

                    if (tblInvest.MAPDocumentUpload != null && tblInvest.MAPDocumentUpload.Length > 0)
                    {
                        lstZip.Add(new FileZip
                        {
                            File = tblInvest.MAPDocumentUpload,
                            FileName = tblInvest.MAPUploadFileName,
                        });
                        ArrAttachments[2] = "MAP Document File";
                    }
                    else
                    {
                        ArrAttachments[2] = string.Empty;
                    }
                    if (tblInvest.JamabandiDocumentUpload != null && tblInvest.JamabandiDocumentUpload.Length > 0)
                    {
                        lstZip.Add(new FileZip
                        {
                            File = tblInvest.JamabandiDocumentUpload,
                            FileName = tblInvest.JamabandiUploadFileName,
                        });
                        ArrAttachments[3] = "Jamabandi Document File";
                    }
                    else
                    {
                        ArrAttachments[3] = string.Empty;
                    }

                    if (tblInvest.NotificationDocumentUpload != null && tblInvest.NotificationDocumentUpload.Length > 0)
                    {
                        lstZip.Add(new FileZip
                        {
                            File = tblInvest.NotificationDocumentUpload,
                            FileName = tblInvest.NotificationUploadFileName,
                        });
                        ArrAttachments[4] = "Notification Document File";
                    }
                    else
                    {
                        ArrAttachments[4] = string.Empty;
                    }


                    if (tblInvest.PanchnamaDocumentUpload != null && tblInvest.PanchnamaDocumentUpload.Length > 0)
                    {
                        lstZip.Add(new FileZip
                        {
                            File = tblInvest.PanchnamaDocumentUpload,
                            FileName = tblInvest.PanchnamaUploadFileName,
                        });
                        ArrAttachments[5] = "Panchnama Document File";
                    }
                    else
                    {
                        ArrAttachments[5] = string.Empty;
                    }
                    if (tblInvest.NazarBandiDocumentUpload != null && tblInvest.NazarBandiDocumentUpload.Length > 0)
                    {
                        lstZip.Add(new FileZip
                        {
                            File = tblInvest.NazarBandiDocumentUpload,
                            FileName = tblInvest.NazarBandiUploadFileName,
                        });
                        ArrAttachments[6] = "Nazri Naksha Document File";
                    }
                    else
                    {
                        ArrAttachments[6] = string.Empty;
                    }

                }






                if (tblUserProfile != null)
                {
                    string filepath = htmlToPdfDownloadTickets.Latter_1(ArrAttachments, tblEnch, tblEncroacher_Details.FirstOrDefault(), tblInvest, tblUserProfile, tbldiv);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(filepath);

                    lstZip.Add(new FileZip
                    {
                        File = filedata,
                        FileName = Path.GetFileName(filepath),
                    });
                }


                List<Tbl_Encroach_Appearance> tblEncroachAppearance = new List<Tbl_Encroach_Appearance>();

                tblEncroachAppearance = dbContext.Tbl_Encroach_Appearance.Where(i => i.EN_Code == En_Code).ToList();

                if (tblEncroachAppearance != null && tblUserProfile != null)
                {
                    foreach (var item in tblEncroachAppearance)
                    {
                        if (item.Decision_Taken != "Closed")
                        {
                            tbl_UserProfiles tblUserProfile2 = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.UserID == item.Entered_By);

                            string filepath2 = htmlToPdfDownloadTickets.Latter_2(item, tblUserProfile2, tblEncroacher_Details, tblInvest, tbldiv);
                            byte[] filedata2 = new EncroachInvestigationView().StreamFile(filepath2);

                            lstZip.Add(new FileZip
                            {
                                File = filedata2,
                                FileName = Path.GetFileName(filepath2),
                            });
                        }

                        if (item.Acf_Decision_Upload != null && item.Acf_Decision_Upload.Length > 0)
                        {
                            lstZip.Add(new FileZip
                            {

                                File = item.Acf_Decision_Upload,
                                FileName = item.Acf_Decision_UploadFileName,
                            });
                        }

                    }

                }
                List<MISEncroachmentDetails> ListPraptra_2 = new EncroachmentView().Praptra_2List(En_Code);

                if (ListPraptra_2 != null)
                {
                    string filepath = htmlToPdfDownloadTickets.Praptra_2(ListPraptra_2);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(filepath);

                    lstZip.Add(new FileZip
                    {
                        File = filedata,
                        FileName = Path.GetFileName(filepath),
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
                    return RedirectToAction("ACFDecision", "ACFDecision");
                }
            }
            catch (Exception ex)
            {
                TempData["ZipMsg"] = "File not exists";
                return RedirectToAction("ACFDecision", "ACFDecision");
            }

        } 
    } 
}
