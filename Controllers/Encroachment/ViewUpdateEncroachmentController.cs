
//------------------------------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : EncroachmentController
//  Description  : File contains details of assigned Encroachment for further updation
//  Date Created : 24-07-2017
//  Author       : Rajkumar
//------------------------------------------------------------------------------------

namespace FMDSS.Controllers.Encroachment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using FMDSS.Models.Encroachment.ViewModel;
    using FMDSS.Models.Encroachment.DomainModel;
    using FMDSS.Repository;
    using FMDSS.Models.FmdssContext;
    using AutoMapper;
    using System.Data.SqlClient;
    using System.IO;
    public class ViewUpdateEncroachmentController : Controller
    {
        private FmdssContext dbContext;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ViewUpdateEncroachmentController()
        {

            dbContext = new FmdssContext();
        }
        /// <summary>
        /// This function returns assigned encroachment user wise
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewEncroachment()
        {
            try
            {
                /*----------------Implemented using entity framework-------------------------------------------*/
                /* List<EncroachmentView> listEncroachment = new List<EncroachmentView>();
                var ssoid = Session["SSOid"].ToString();
                var duplicateEncode = dbContext.Tbl_Encroach_InvestigationDetails.Where(x=> x.EN_Code != null).Select(e => e.EN_Code).ToList();        
                var query = (from a in dbContext.Tbl_Encroachment
                              join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                              join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                              join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                              //join e in dbContext.Tbl_Encroacher_Details on a.EN_Code equals e.EN_Code   
                              join f in dbContext.tbl_mst_ForestEmployees on a.InvestigationOfficer equals f.ROWID                             
                              select new
                              {
                                  EN_Code = a.EN_Code,
                                  DIV_CODE = b.DIV_NAME,
                                  RANGE_CODE = c.RANGE_NAME,
                                  IsKnown = a.IsKnown,                                 
                                  UserName = d.Name,
                                  Area = a.Area,
                                  DOE = a.DOE,
                                  InvestigationOfficer = a.InvestigationOfficer,
                                  SSOId=f.SSO_ID
                              }).Distinct().OrderByDescending(i=>i.DOE).ToList();

                var whereQuery = query.Where(x => Convert.ToString(x.SSOId).ToUpper().Equals(ssoid.ToUpper()));
                var result = from b in whereQuery where !duplicateEncode.Contains(b.EN_Code) select b;
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
                            UserName = item.UserName,
                            DOE = item.DOE
                        });
                }
                ViewData["AssignedEncroachment"] = listEncroachment;*/
                ViewData["AssignedEncroachment"] = new EncroachmentView().EncroachmentList("Investigation");
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        /// <summary>
        /// This function update investigation details
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public ActionResult InvestigationDeatils(string enchCode)
        {
            try
            {
                Session.Remove("EncoachmentList");
                if (enchCode != null)
                {
                    enchCode = Encryption.decrypt(enchCode);
                    TempData["EnchCode"] = enchCode;

                    EncroachInvestigationView objInvestView = new EncroachInvestigationView();
                    Tbl_Encroach_InvestigationDetails objInvest = dbContext.Tbl_Encroach_InvestigationDetails.FirstOrDefault(i => i.EN_Code == enchCode);

                    tbl_AD_District_Maping objtbl_AD_District_Maping = dbContext.tbl_AD_District_Maping.Where(x => x.RequestedID == enchCode).FirstOrDefault();

                    Tbl_Encroachment tblEnch = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == enchCode);

                    if (objInvest != null)
                    {
                        objInvestView.EN_Code = enchCode;
                        //objInvestView.AadharOrBhamasha = objInvest.AadharOrBhamasha;
                        //objInvestView.Encroacher_Aadhar = objInvest.Encroacher_Aadhar;
                        //objInvestView.Encroacher_Bhamashah = objInvest.Encroacher_Bhamashah;                     
                        objInvestView.Year = objInvest.Year;
                        objInvestView.khasraNo = objInvest.khasraNo;
                        objInvestView.TotalArea = objInvest.TotalArea;
                        objInvestView.TypeofLand = objInvest.TypeofLand;
                        objInvestView.TaxPerHact = objInvest.TaxPerHact;
                        objInvestView.Tax = objInvest.Tax;
                        objInvestView.Encroachment_Area = objInvest.Encroachment_Area;
                        objInvestView.Encroachment_Yield = objInvest.Encroachment_Yield;
                        objInvestView.Block_Name = objInvest.Block_Name;
                        objInvestView.Total_Area_Block = objInvest.Total_Area_Block;
                        objInvestView.Remarks = objInvest.Remarks;

                        objInvestView.CompartmentNo = objInvest.CompartmentNo;
                        objInvestView.InformationGatheredBy = objInvest.InformationGatheredBy;
                        objInvestView.InformationApprovedBy = objInvest.InformationApprovedBy;
                        objInvestView.NotificationNo = objInvest.NotificationNo;
                        objInvestView.NotificationDate = objInvest.NotificationDate.ToString("dd/MM/yyyy");

                        if (Convert.ToString(objInvest.DocumentUpload) != string.Empty)
                        {
                            objInvestView.IsDocumentUpload = "Y";
                        }
                        else
                        {
                            objInvestView.IsDocumentUpload = "N";
                        }

                        if (Convert.ToString(objInvest.MAPDocumentUpload) != string.Empty)
                        {
                            objInvestView.IsMAPFilesDocumentUpload = "Y";
                        }
                        else
                        {
                            objInvestView.IsMAPFilesDocumentUpload = "N";
                        }

                        if (Convert.ToString(objInvest.JamabandiDocumentUpload) != string.Empty)
                        {
                            objInvestView.IsJamabandiFilesDocumentUpload = "Y";
                        }
                        else
                        {
                            objInvestView.IsJamabandiFilesDocumentUpload = "N";
                        }


                        if (Convert.ToString(objInvest.NotificationDocumentUpload) != string.Empty)
                        {
                            objInvestView.IsNotificationFilesDocumentUpload = "Y";
                        }
                        else
                        {
                            objInvestView.IsNotificationFilesDocumentUpload = "N";
                        }

                        if (Convert.ToString(objInvest.PanchnamaDocumentUpload) != string.Empty)
                        {
                            objInvestView.IsPanchnamaFilesDocumentUpload = "Y";
                        }
                        else
                        {
                            objInvestView.IsPanchnamaFilesDocumentUpload = "N";
                        }

                        if (Convert.ToString(objInvest.NazarBandiDocumentUpload) != string.Empty)
                        {
                            objInvestView.IsNazarBandiFilesDocumentUpload = "Y";
                        }
                        else
                        {
                            objInvestView.IsNazarBandiFilesDocumentUpload = "N";
                        }

                        return View(objInvestView);
                    }
                    else
                    {
                        objInvestView.TotalArea = Convert.ToDecimal(objtbl_AD_District_Maping.AreaShapeinHactare);
                        objInvestView.Encroachment_Area = Convert.ToDecimal(tblEnch.Area);
                        objInvestView.EN_Code = enchCode;
                        objInvestView.IsDocumentUpload = "N";
                        objInvestView.IsMAPFilesDocumentUpload = "N";
                        objInvestView.IsJamabandiFilesDocumentUpload = "N";
                        objInvestView.IsNotificationFilesDocumentUpload = "N";
                        objInvestView.IsPanchnamaFilesDocumentUpload = "N";
                        objInvestView.IsNazarBandiFilesDocumentUpload = "N";
                        return View(objInvestView);
                    }
                }
                else
                {
                    return RedirectToAction("ViewEncroachment", "ViewUpdateEncroachment");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetFile(string EN_Code, string FileType)
        {
            try
            {
                Tbl_Encroach_InvestigationDetails tbl = dbContext.Tbl_Encroach_InvestigationDetails.FirstOrDefault(i => i.EN_Code == EN_Code);

                if (tbl != null)
                {
                    switch (FileType)
                    {
                        case "Files":

                            if (Convert.ToString(tbl.DocumentUpload) != string.Empty)
                            {
                                byte[] files = (byte[])tbl.DocumentUpload;
                                Response.AddHeader("Content-type", tbl.UploadFileType);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + tbl.UploadFileName);
                                Response.BinaryWrite(files);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                this.TempData["FileError"] = "Error in downloading File Information.";
                            }

                            break;
                        case "MAPFiles":

                            if (Convert.ToString(tbl.MAPDocumentUpload) != string.Empty)
                            {
                                byte[] files = (byte[])tbl.MAPDocumentUpload;
                                Response.AddHeader("Content-type", tbl.MAPUploadFileType);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + tbl.MAPUploadFileName);
                                Response.BinaryWrite(files);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                this.TempData["FileError"] = "Error in downloading File Information.";
                            }

                            break;
                        case "JamabandiFiles":

                            if (Convert.ToString(tbl.JamabandiDocumentUpload) != string.Empty)
                            {
                                byte[] files = (byte[])tbl.JamabandiDocumentUpload;
                                Response.AddHeader("Content-type", tbl.JamabandiUploadFileType);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + tbl.JamabandiUploadFileName);
                                Response.BinaryWrite(files);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                this.TempData["FileError"] = "Error in downloading File Information.";
                            }

                            break;
                        case "NotificationFiles":

                            if (Convert.ToString(tbl.DocumentUpload) != string.Empty)
                            {
                                byte[] files = (byte[])tbl.NotificationDocumentUpload;
                                Response.AddHeader("Content-type", tbl.NotificationUploadFileType);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + tbl.NotificationUploadFileName);
                                Response.BinaryWrite(files);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                this.TempData["FileError"] = "Error in downloading File Information.";
                            }

                            break;
                        case "PanchnamaFiles":

                            if (Convert.ToString(tbl.PanchnamaDocumentUpload) != string.Empty)
                            {
                                byte[] files = (byte[])tbl.PanchnamaDocumentUpload;
                                Response.AddHeader("Content-type", tbl.PanchnamaUploadFileType);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + tbl.PanchnamaUploadFileName);
                                Response.BinaryWrite(files);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                this.TempData["FileError"] = "Error in downloading File Information.";
                            }

                            break;
                        case "NazarBandiFiles":

                            if (Convert.ToString(tbl.DocumentUpload) != string.Empty)
                            {
                                byte[] files = (byte[])tbl.NazarBandiDocumentUpload;
                                Response.AddHeader("Content-type", tbl.NazarBandiUploadFileType);
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + tbl.NazarBandiUploadFileName);
                                Response.BinaryWrite(files);
                                Response.Flush();
                                Response.End();
                            }
                            else
                            {
                                this.TempData["FileError"] = "Error in downloading File Information.";
                            }

                            break;
                    }



                }
                else
                {
                    this.TempData["FileError"] = "Error in downloading File Information.";
                }
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return this.View("Error", new HandleErrorInfo(ex, "ViewUpdateEncroachment", "GetFile"));
            }
        }
        public ActionResult EncroachmentDetails()
        {
            EncroacherDetailsView tbl = new EncroacherDetailsView();
            return PartialView("EncroachmentDetails", tbl);
        }
        
        [NonAction]
        public EncroachInvestigationView UploadFiles(EncroachInvestigationView objInvest, HttpPostedFileBase Files, HttpPostedFileBase MAPFiles, HttpPostedFileBase JamabandiFiles, HttpPostedFileBase NotificationFiles, HttpPostedFileBase PanchnamaFiles, HttpPostedFileBase NazarBandiFiles)
        {
            if (Convert.ToString(objInvest.EN_Code) != string.Empty)
            {
                if (Files != null && Files.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    objInvest.UploadFileName = objInvest.EN_Code + "File" + Files.FileName;
                    objInvest.UploadFileType = Path.GetExtension(Files.FileName);
                    document = Path.GetFileName(Files.FileName);
                    string FileFullName = objInvest.EN_Code + "File" + document;
                    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, Files);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    objInvest.DocumentUpload = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objInvest.DocumentUpload = null;
                    objInvest.UploadFileName = null;
                    objInvest.UploadFileType = null;
                }

                if (MAPFiles != null && MAPFiles.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    objInvest.MAPUploadFileName = objInvest.EN_Code + "MAPFiles" + MAPFiles.FileName;
                    objInvest.MAPUploadFileType = Path.GetExtension(MAPFiles.FileName);
                    document = Path.GetFileName(MAPFiles.FileName);
                    string FileFullName = objInvest.EN_Code + "MAPFiles" + document;
                    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, MAPFiles);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    objInvest.MAPDocumentUpload = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objInvest.MAPDocumentUpload = null;
                    objInvest.MAPUploadFileName = null;
                    objInvest.MAPUploadFileType = null;
                }


                if (JamabandiFiles != null && JamabandiFiles.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    objInvest.JamabandiUploadFileName = objInvest.EN_Code + "JamabandiFiles" + JamabandiFiles.FileName;
                    objInvest.JamabandiUploadFileType = Path.GetExtension(JamabandiFiles.FileName);
                    document = Path.GetFileName(JamabandiFiles.FileName);
                    string FileFullName = objInvest.EN_Code + "JamabandiFiles" + document;
                    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, JamabandiFiles);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    objInvest.JamabandiDocumentUpload = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objInvest.JamabandiDocumentUpload = null;
                    objInvest.JamabandiUploadFileName = null;
                    objInvest.JamabandiUploadFileType = null;
                }

                if (NotificationFiles != null && NotificationFiles.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    objInvest.NotificationUploadFileName = objInvest.EN_Code + "NotificationFiles" + NotificationFiles.FileName;
                    objInvest.NotificationUploadFileType = Path.GetExtension(NotificationFiles.FileName);
                    document = Path.GetFileName(NotificationFiles.FileName);
                    string FileFullName = objInvest.EN_Code + "NotificationFiles" + document;
                    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, NotificationFiles);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    objInvest.NotificationDocumentUpload = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objInvest.NotificationDocumentUpload = null;
                    objInvest.NotificationUploadFileName = null;
                    objInvest.NotificationUploadFileType = null;
                }
                if (PanchnamaFiles != null && PanchnamaFiles.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    objInvest.PanchnamaUploadFileName = objInvest.EN_Code + "PanchnamaFiles" + PanchnamaFiles.FileName;
                    objInvest.PanchnamaUploadFileType = Path.GetExtension(PanchnamaFiles.FileName);
                    document = Path.GetFileName(PanchnamaFiles.FileName);
                    string FileFullName = objInvest.EN_Code + "PanchnamaFiles" + document;
                    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, PanchnamaFiles);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    objInvest.PanchnamaDocumentUpload = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objInvest.PanchnamaDocumentUpload = null;
                    objInvest.PanchnamaUploadFileName = null;
                    objInvest.PanchnamaUploadFileType = null;
                }
                if (NazarBandiFiles != null && NazarBandiFiles.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    objInvest.NazarBandiUploadFileName = objInvest.EN_Code + "NazarBandiFiles" + NazarBandiFiles.FileName;
                    objInvest.NazarBandiUploadFileType = Path.GetExtension(NazarBandiFiles.FileName);
                    document = Path.GetFileName(NazarBandiFiles.FileName);
                    string FileFullName = objInvest.EN_Code + "NazarBandiFiles" + document;
                    path = new EncroachInvestigationView().CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, NazarBandiFiles);
                    byte[] filedata = new EncroachInvestigationView().StreamFile(path);
                    objInvest.NazarBandiDocumentUpload = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objInvest.NazarBandiDocumentUpload = null;
                    objInvest.NazarBandiUploadFileName = null;
                    objInvest.NazarBandiUploadFileType = null;
                }
            }


            return objInvest;
        }
        
        /// <summary>
        /// Final submision of updation of investigation details
        /// </summary>
        /// <param name="objInvest"></param>
        /// <returns></returns>
        public ActionResult Submit(EncroachInvestigationView objInvest, HttpPostedFileBase Files, HttpPostedFileBase MAPFiles, HttpPostedFileBase JamabandiFiles, HttpPostedFileBase NotificationFiles, HttpPostedFileBase PanchnamaFiles, HttpPostedFileBase NazarBandiFiles)
        {
            try
            {
                Int32 dbStatus;
                Tbl_Encroach_InvestigationDetails tblEnchInvDetails  = new Tbl_Encroach_InvestigationDetails();
                if (objInvest != null)
                {
                    if (Session["EncoachmentList"] != null)
                    {
                        objInvest = UploadFiles(objInvest, Files, MAPFiles, JamabandiFiles, NotificationFiles, PanchnamaFiles, NazarBandiFiles);

                        tblEnchInvDetails = dbContext.Tbl_Encroach_InvestigationDetails.FirstOrDefault(i => i.EN_Code == objInvest.EN_Code);

                        if (tblEnchInvDetails != null)
                        {

                            tblEnchInvDetails.Year = objInvest.Year;
                            tblEnchInvDetails.khasraNo = objInvest.khasraNo;
                            tblEnchInvDetails.TotalArea = objInvest.TotalArea;
                            tblEnchInvDetails.TypeofLand = objInvest.TypeofLand;
                            tblEnchInvDetails.TaxPerHact = objInvest.TaxPerHact;
                            tblEnchInvDetails.Tax = objInvest.Tax;
                            tblEnchInvDetails.Encroachment_Area = objInvest.Encroachment_Area;
                            tblEnchInvDetails.Encroachment_Yield = objInvest.Encroachment_Yield;
                            tblEnchInvDetails.Block_Name = objInvest.Block_Name;
                            tblEnchInvDetails.Total_Area_Block = objInvest.Total_Area_Block;

                            tblEnchInvDetails.DocumentUpload = objInvest.DocumentUpload ?? tblEnchInvDetails.DocumentUpload;
                            tblEnchInvDetails.UploadFileType = objInvest.UploadFileType ?? tblEnchInvDetails.UploadFileType;
                            tblEnchInvDetails.UploadFileName = objInvest.UploadFileName ?? tblEnchInvDetails.UploadFileName;

                            tblEnchInvDetails.MAPDocumentUpload = objInvest.MAPDocumentUpload ?? tblEnchInvDetails.MAPDocumentUpload;
                            tblEnchInvDetails.MAPUploadFileType = objInvest.MAPUploadFileType ?? tblEnchInvDetails.MAPUploadFileType;
                            tblEnchInvDetails.MAPUploadFileName = objInvest.MAPUploadFileName ?? tblEnchInvDetails.MAPUploadFileName;

                            tblEnchInvDetails.JamabandiDocumentUpload = objInvest.JamabandiDocumentUpload ?? tblEnchInvDetails.JamabandiDocumentUpload;
                            tblEnchInvDetails.JamabandiUploadFileType = objInvest.JamabandiUploadFileType ?? tblEnchInvDetails.JamabandiUploadFileType;
                            tblEnchInvDetails.JamabandiUploadFileName = objInvest.JamabandiUploadFileName ?? tblEnchInvDetails.JamabandiUploadFileName;

                            tblEnchInvDetails.NotificationDocumentUpload = objInvest.NotificationDocumentUpload ?? tblEnchInvDetails.NotificationDocumentUpload;
                            tblEnchInvDetails.NotificationUploadFileType = objInvest.NotificationUploadFileType ?? tblEnchInvDetails.NotificationUploadFileType;
                            tblEnchInvDetails.NotificationUploadFileName = objInvest.NotificationUploadFileName ?? tblEnchInvDetails.NotificationUploadFileName;

                            tblEnchInvDetails.PanchnamaDocumentUpload = objInvest.PanchnamaDocumentUpload ?? tblEnchInvDetails.PanchnamaDocumentUpload;
                            tblEnchInvDetails.PanchnamaUploadFileType = objInvest.PanchnamaUploadFileType ?? tblEnchInvDetails.PanchnamaUploadFileType;
                            tblEnchInvDetails.PanchnamaUploadFileName = objInvest.PanchnamaUploadFileName ?? tblEnchInvDetails.PanchnamaUploadFileName;

                            tblEnchInvDetails.NazarBandiDocumentUpload = objInvest.NazarBandiDocumentUpload ?? tblEnchInvDetails.NazarBandiDocumentUpload;
                            tblEnchInvDetails.NazarBandiUploadFileType = objInvest.NazarBandiUploadFileType ?? tblEnchInvDetails.NazarBandiUploadFileType;
                            tblEnchInvDetails.NazarBandiUploadFileName = objInvest.NazarBandiUploadFileName ?? tblEnchInvDetails.NazarBandiUploadFileName;



                            tblEnchInvDetails.CompartmentNo = objInvest.CompartmentNo;
                            tblEnchInvDetails.InformationGatheredBy = objInvest.InformationGatheredBy;
                            tblEnchInvDetails.InformationApprovedBy = objInvest.InformationApprovedBy;
                            tblEnchInvDetails.NotificationNo = objInvest.NotificationNo;

                            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");
                            DateTime dt = Convert.ToDateTime(objInvest.NotificationDate, cul);
                            tblEnchInvDetails.NotificationDate = dt;


                            

                            tblEnchInvDetails.Remarks = objInvest.Remarks;
                            tblEnchInvDetails.Entered_By = Convert.ToInt64(Session["UserId"].ToString());
                            dbStatus = dbContext.SaveChanges();

                            //dbContext.Tbl_Encroach_InvestigationDetails.RemoveRange(dbContext.Tbl_Encroach_InvestigationDetails.Where(x => x.EN_Code == tblEnchInvDetails.EN_Code));
                            //dbContext.SaveChanges();

                            Tbl_Encroachment tblEnchroach = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == objInvest.EN_Code && i.ACF_Status == "Reassign");
                            if (tblEnchroach != null)
                            {
                                tblEnchroach.DispatchNo = "";
                                tblEnchroach.DispatchDate = null;
                                tblEnchroach.ACF_Status = "";
                                tblEnchroach.ACF_Remarks = "";
                                tblEnchroach.ACF_Date = null;
                                tblEnchroach.FileStatus = 1;
                                dbStatus = dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            Tbl_Encroach_InvestigationDetails tblEnchInvDetail = new Tbl_Encroach_InvestigationDetails();
                            tblEnchInvDetail.EN_Code = objInvest.EN_Code;
                            tblEnchInvDetail.Year = objInvest.Year;
                            tblEnchInvDetail.khasraNo = objInvest.khasraNo;
                            tblEnchInvDetail.TotalArea = objInvest.TotalArea;
                            tblEnchInvDetail.TypeofLand = objInvest.TypeofLand;
                            tblEnchInvDetail.TaxPerHact = objInvest.TaxPerHact;
                            tblEnchInvDetail.Tax = objInvest.Tax;
                            tblEnchInvDetail.Encroachment_Area = objInvest.Encroachment_Area;
                            tblEnchInvDetail.Encroachment_Yield = objInvest.Encroachment_Yield;
                            tblEnchInvDetail.Block_Name = objInvest.Block_Name;
                            tblEnchInvDetail.Total_Area_Block = objInvest.Total_Area_Block;

                            tblEnchInvDetail.DocumentUpload = objInvest.DocumentUpload == null ? null : objInvest.DocumentUpload;
                            tblEnchInvDetail.UploadFileType = objInvest.UploadFileType == null ? null : objInvest.UploadFileType;
                            tblEnchInvDetail.UploadFileName = objInvest.UploadFileName == null ? null : objInvest.UploadFileName;

                            tblEnchInvDetail.MAPDocumentUpload = objInvest.MAPDocumentUpload == null ? null : objInvest.MAPDocumentUpload;
                            tblEnchInvDetail.MAPUploadFileType = objInvest.MAPUploadFileType == null ? null : objInvest.MAPUploadFileType;
                            tblEnchInvDetail.MAPUploadFileName = objInvest.MAPUploadFileName == null ? null : objInvest.MAPUploadFileName;

                            tblEnchInvDetail.JamabandiDocumentUpload = objInvest.JamabandiDocumentUpload == null ? null : objInvest.JamabandiDocumentUpload;
                            tblEnchInvDetail.JamabandiUploadFileType = objInvest.JamabandiUploadFileType == null ? null : objInvest.JamabandiUploadFileType;
                            tblEnchInvDetail.JamabandiUploadFileName = objInvest.JamabandiUploadFileName == null ? null : objInvest.JamabandiUploadFileName;

                            tblEnchInvDetail.NotificationDocumentUpload = objInvest.NotificationDocumentUpload == null ? null : objInvest.NotificationDocumentUpload;
                            tblEnchInvDetail.NotificationUploadFileType = objInvest.NotificationUploadFileType == null ? null : objInvest.NotificationUploadFileType;
                            tblEnchInvDetail.NotificationUploadFileName = objInvest.NotificationUploadFileName == null ? null : objInvest.NotificationUploadFileName;

                            tblEnchInvDetail.PanchnamaDocumentUpload = objInvest.PanchnamaDocumentUpload == null ? null : objInvest.PanchnamaDocumentUpload;
                            tblEnchInvDetail.PanchnamaUploadFileType = objInvest.PanchnamaUploadFileType == null ? null : objInvest.PanchnamaUploadFileType;
                            tblEnchInvDetail.PanchnamaUploadFileName = objInvest.PanchnamaUploadFileName == null ? null : objInvest.PanchnamaUploadFileName;

                            tblEnchInvDetail.NazarBandiDocumentUpload = objInvest.NazarBandiDocumentUpload == null ? null : objInvest.NazarBandiDocumentUpload;
                            tblEnchInvDetail.NazarBandiUploadFileType = objInvest.NazarBandiUploadFileType == null ? null : objInvest.NazarBandiUploadFileType;
                            tblEnchInvDetail.NazarBandiUploadFileName = objInvest.NazarBandiUploadFileName == null ? null : objInvest.NazarBandiUploadFileName;

                            tblEnchInvDetail.CompartmentNo = objInvest.CompartmentNo;
                            tblEnchInvDetail.InformationGatheredBy = objInvest.InformationGatheredBy;
                            tblEnchInvDetail.InformationApprovedBy = objInvest.InformationApprovedBy;
                            tblEnchInvDetail.NotificationNo = objInvest.NotificationNo;

                            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");
                            DateTime dt = Convert.ToDateTime(objInvest.NotificationDate, cul);
                            tblEnchInvDetail.NotificationDate = dt; 

                            tblEnchInvDetail.Remarks = objInvest.Remarks;

                            tblEnchInvDetail.Entered_By = Convert.ToInt64(Session["UserId"].ToString());
                            var result = dbContext.Tbl_Encroach_InvestigationDetails.Add(tblEnchInvDetail);
                            dbStatus = dbContext.SaveChanges();
                            //Id = this.dbContext.Tbl_Encroach_InvestigationDetails.Max(p => p.ID);
                        }
                        Tbl_Encroacher_Details tblEd = dbContext.Tbl_Encroacher_Details.FirstOrDefault(x => x.EN_Code == objInvest.EN_Code);
                        if (tblEd != null && dbStatus == 1)
                        {
                            dbContext.Tbl_Encroacher_Details.RemoveRange(dbContext.Tbl_Encroacher_Details.Where(x => x.EN_Code == objInvest.EN_Code));
                            dbContext.SaveChanges();

                            List<EncroacherDetailsView> listEncroachView = (List<EncroacherDetailsView>)Session["EncoachmentList"];
                            foreach (EncroacherDetailsView objItems in listEncroachView)
                            {
                                objItems.EN_Code = objInvest.EN_Code;
                                Mapper.CreateMap<EncroacherDetailsView, Tbl_Encroacher_Details>();
                                Tbl_Encroacher_Details objModel = Mapper.Map<EncroacherDetailsView, Tbl_Encroacher_Details>(objItems);
                                dbContext.Tbl_Encroacher_Details.Add(objModel);
                                dbContext.SaveChanges();

                                Tbl_Encroachment ObjUserModel = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == objInvest.EN_Code);
                                ObjUserModel.IsKnown = true;
                                dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            if (Session["EncoachmentList"] != null)
                            {
                                List<EncroacherDetailsView> listEncroachView = (List<EncroacherDetailsView>)Session["EncoachmentList"];
                                foreach (EncroacherDetailsView objItems in listEncroachView)
                                {
                                    objItems.EN_Code = objInvest.EN_Code;
                                    Mapper.CreateMap<EncroacherDetailsView, Tbl_Encroacher_Details>();
                                    Tbl_Encroacher_Details objModel = Mapper.Map<EncroacherDetailsView, Tbl_Encroacher_Details>(objItems);
                                    dbContext.Tbl_Encroacher_Details.Add(objModel);
                                    dbContext.SaveChanges();

                                    Tbl_Encroachment ObjUserModel = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == objInvest.EN_Code);
                                    ObjUserModel.IsKnown = true;
                                    dbContext.SaveChanges();
                                }
                            }
                            else
                            {
                                TempData["InvestigationMsg"] = "Something went wrong please try after sometime";
                            }
                        }

                        Tbl_Encroachment tblEnch = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == objInvest.EN_Code);
                        if (tblEnch != null)
                        {
                            tblEnch.FileStatus = 1;
                            dbStatus = dbContext.SaveChanges();
                        }

                        TempData["InvestigationMsg"] = "Investigation details updated sucessfully for ECode:" + objInvest.EN_Code;
                    }
                    else
                    {
                        TempData["InvestigationMsg"] = "Something went wrong please try after sometime";
                    }
                }
                else
                {
                    TempData["InvestigationMsg"] = "Encoachment details must be entred!!!";
                    return RedirectToAction("InvestigationDeatils", "ViewUpdateEncroachment", new { enchCode = Encryption.encrypt(objInvest.EN_Code) });
                }
                //}
                //else {
                //    TempData["InvestigationMsg"] = "Something went wrong please try after sometime";
                //}
            }
            catch (Exception e)
            {
                throw;
            }
            return RedirectToAction("ViewEncroachment", "ViewUpdateEncroachment");
        }
        /// <summary>
        /// this function returns list of encroachment details
        /// </summary>
        /// <param name="enchCode"></param>
        /// <returns></returns>
        public JsonResult GetEncroacherDetails(string enchCode)
        {
            List<EncroacherDetailsView> objData = new List<EncroacherDetailsView>();
            List<EncroacherDetailsView> list = new List<EncroacherDetailsView>();
            try
            {
                var result = dbContext.Tbl_Encroacher_Details.Where(x => x.EN_Code.Equals(enchCode)).ToList();
                foreach (Tbl_Encroacher_Details item in result)
                {
                    Mapper.CreateMap<Tbl_Encroacher_Details, EncroacherDetailsView>();
                    EncroacherDetailsView objModel = Mapper.Map<Tbl_Encroacher_Details, EncroacherDetailsView>(item);
                    objModel.EN_Code = item.EN_Code;
                    objModel.rowid = Guid.NewGuid().ToString();
                    objData.Add(objModel);
                    Session["EncoachmentList"] = objData;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Json(objData, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Function to save offender list in grid
        /// </summary>
        /// <param name="_objModel"></param>
        /// <returns></returns>      
        public JsonResult AddEncroacherDetails(EncroacherDetailsView objModel)
        {
            List<EncroacherDetailsView> objData = new List<EncroacherDetailsView>();
            List<EncroacherDetailsView> list = new List<EncroacherDetailsView>();
            try
            {
                if (Session["EncoachmentList"] != null)
                {
                    list = (List<EncroacherDetailsView>)Session["EncoachmentList"];
                    foreach (var item in list)
                    {
                        if (item.rowid == objModel.rowid)
                        {
                            objData.Add(objModel);
                        }
                        else
                        {
                            objData.Add(item);
                        }
                    }
                    if (objModel.rowid == null)
                    {
                        objModel.rowid = Guid.NewGuid().ToString();
                        objData.Add(objModel);
                    }
                    Session["EncoachmentList"] = null;
                    Session["EncoachmentList"] = objData;
                }
                else
                {
                    objModel.rowid = Guid.NewGuid().ToString();
                    objData.Add(objModel);
                    Session["EncoachmentList"] = objData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }
    }
}
