
//---------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : EncroachmentController
//  Description  : File contains details of Encroachment registration
//  Date Created : 19-07-2017
//  Author       : Rajkumar
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
    using Ionic.Zip;
    using FMDSS.Models.CitizenService.PermissionServices;
    using FMDSS.Models;
    using System.Data;
    using FMDSS.Models.Master;
    using Models.MIS;
    using log4net;
    using Newtonsoft.Json;

    public class EncroachmentController : Controller
    {
        ILog ErrorLog = LogManager.GetLogger("DBLogger");
        private FmdssContext dbContext;
        /// <summary>
        /// This function is default Constructor
        /// </summary>
        public EncroachmentController()
        {
            this.dbContext = new FmdssContext();
        }
        /// <summary>
        /// This function return registration view
        /// </summary>
        /// <returns></returns>
        public ActionResult Registration(string docId = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(docId))
                {
                    string[] docids = docId.Split('-');
                    string path1 = Server.MapPath("~/Documents/EnchroachmentGISData/" + docids[0]);
                    string path2 = Server.MapPath("~/Documents/EnchroachmentGISData/" + docids[1]);
                    string fixedlandJSONList = System.IO.File.ReadAllText(path1);
                    string GISInformationJSONList = System.IO.File.ReadAllText(path2);
                    List<clsPermission> fixedLandlist = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(fixedlandJSONList, typeof(List<clsPermission>));
                    if (System.IO.File.Exists(path1) == true)
                    {
                        System.IO.File.Delete(path1);
                    }
                    List<GISDataBaseModel> GISInformationList = (List<GISDataBaseModel>)Newtonsoft.Json.JsonConvert.DeserializeObject(GISInformationJSONList, typeof(List<GISDataBaseModel>));
                    if (System.IO.File.Exists(path2) == true)
                    {
                        System.IO.File.Delete(path2);
                    }
                    TempData["GPSLat"] = GISInformationList.FirstOrDefault().GPSLat;
                    TempData["GPSLong"] = GISInformationList.FirstOrDefault().GPSLong;
                    TempData["shapeArea"] = GISInformationList.FirstOrDefault().AreaShapeInHactor;

                    TempData["GISModelLIst"] = fixedLandlist;
                    TempData["GISModel"] = GISInformationList;
                }

                Int64 UID = Convert.ToInt64(Session["UserID"]);
                Session.Remove("EncoachmentList");
                List<SelectListItem> lstDivision = new List<SelectListItem>();
                List<EncroachmentView> lstEncroach = new List<EncroachmentView>();
                var encrochStatus = (from a in dbContext.Tbl_Encroachment
                                     join b in dbContext.tbl_mst_Forest_Divisions on a.DIV_CODE equals b.DIV_CODE
                                     join c in dbContext.tbl_mst_Forest_Ranges on a.RANGE_CODE equals c.RANGE_CODE
                                     join d in dbContext.tbl_UserProfiles on a.EnteredBy equals d.UserID
                                     select new
                                     {
                                         a.EN_Code,
                                         b.DIV_NAME,
                                         c.RANGE_NAME,
                                         d.Name,
                                         a.DOE,
                                         a.ACF_Status,
                                         a.ACF_Remarks,
                                         a.LRACTNO,
                                         a.Final_Decision_Taken,
                                         a.Final_Decision_Date,
                                         a.EnteredBy,
                                         a.DispatchNo,
                                         a.FileStatus
                                     }).OrderByDescending(a => a.DOE).ToList().Where(t => t.EnteredBy == UID);

                foreach (var item in encrochStatus)
                {
                    lstEncroach.Add(new EncroachmentView
                    {
                        EncroachmentId = item.EN_Code,
                        DIV_CODE = item.DIV_NAME,
                        RANGE_CODE = item.RANGE_NAME,
                        UserName = item.Name,
                        ACF_Status = item.ACF_Status,
                        ACF_Remarks = item.ACF_Remarks,
                        LRACTNO = item.LRACTNO,
                        Decision_Taken = item.Final_Decision_Taken,
                        Decision_Date = item.Final_Decision_Date,
                        FileStatus = item.FileStatus

                    });
                }
                // isDownload= IsDownloadFile(item.EN_Code)

                var CurrentDivAndRange = (from a in dbContext.tbl_UserProfiles
                                          join b in dbContext.tbl_mst_ForestEmployees on a.Ssoid equals b.SSO_ID
                                          join c in dbContext.tbl_mst_ForestOffices on b.Office_Id equals c.Office_ID
                                          where a.UserID == UID
                                          select new { c.DIV_CODE, c.OfficeName }).FirstOrDefault();

                if (CurrentDivAndRange.DIV_CODE.Contains("DIV"))
                {
                    ViewBag.Division = new SelectList(this.dbContext.tbl_mst_Forest_Divisions.ToList(), "DIV_CODE", "DIV_NAME", CurrentDivAndRange.DIV_CODE);
                    ViewBag.RANGE = new SelectList(this.dbContext.tbl_mst_Forest_Ranges.Where(x => x.DIV_CODE == CurrentDivAndRange.DIV_CODE).ToList(), "RANGE_CODE", "RANGE_NAME");

                }
                else if (CurrentDivAndRange.DIV_CODE.Contains("RNG"))
                {
                    string divCode = this.dbContext.tbl_mst_Forest_Ranges.Where(x => x.RANGE_CODE == CurrentDivAndRange.DIV_CODE).Select(x => x.DIV_CODE).FirstOrDefault();
                    ViewBag.Division = new SelectList(this.dbContext.tbl_mst_Forest_Divisions.ToList(), "DIV_CODE", "DIV_NAME", divCode);

                    ViewBag.RANGE = new SelectList(this.dbContext.tbl_mst_Forest_Ranges.Where(x => x.DIV_CODE == divCode).ToList(), "RANGE_CODE", "RANGE_NAME", CurrentDivAndRange.DIV_CODE);
                }
              
                // string path = Server.MapPath("~/Documents/TempData/" + gisdata);
                //string fixedlandJSONList = System.IO.File.ReadAllText(path);
                //List<clsPermission> fixedLandlist = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(fixedlandJSONList, typeof(List<clsPermission>));
                //if (System.IO.File.Exists(path) == true)
                //{
                //    System.IO.File.Delete(path);
                //}

                 
                
                
                //var result = this.dbContext.tbl_mst_Forest_Divisions.ToList();
                //foreach (var item in result)
                //{
                //    lstDivision.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                //}
                //ViewBag.Division = lstDivision;
                #region Fill GIS Information

                if (TempData["GISModelLIst"] == null)
                    TempData["GISModelLIst"] = new List<clsPermission>();
                #endregion

                ViewBag.EncroachStatus = lstEncroach;
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }


        /// <summary>
        /// This function return range based on division code
        /// </summary>
        /// <param name="DIV_CODE"></param>
        /// <returns></returns>
        public JsonResult GetRange(string DIV_CODE)
        {
            try
            {
                List<SelectListItem> lstRange = new List<SelectListItem>();
                var result = this.dbContext.tbl_mst_Forest_Ranges.Where(e => DIV_CODE.Contains(e.DIV_CODE)).ToList();
                foreach (var item in result)
                {
                    lstRange.Add(new SelectListItem { Text = item.RANGE_NAME, Value = item.RANGE_CODE });
                }
                return Json(lstRange, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Function to save offender list in grid
        /// </summary>
        /// <param name="_objModel"></param>
        /// <returns></returns>
        [HttpPost]
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
            catch (Exception)
            {
                throw;
            }
            return Json(objModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete the vechile details based on ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult DeleteEncroachment(string Id)
        {
            List<EncroacherDetailsView> lstEnch = new List<EncroacherDetailsView>();
            try
            {
                if (Session["EncoachmentList"] != null)
                {
                    lstEnch = (List<EncroacherDetailsView>)Session["EncoachmentList"];
                    if (Id != "0" && Id.Length > 0)
                    {
                        EncroacherDetailsView ench = lstEnch.Single(a => a.rowid == Id);
                        lstEnch.Remove(ench);
                    }
                    Session["EncoachmentList"] = lstEnch;
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(lstEnch, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Final submission of registration of encroachment
        /// </summary>
        /// <param name="objEncroch"></param>
        /// <param name="Files"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(EncroachmentView objEncroch, HttpPostedFileBase Files)
        {
            try
            {
                if (Files != null && Files.ContentLength > 0)
                {
                    string document = string.Empty;
                    var path = string.Empty;
                    Guid fileGenName = Guid.NewGuid();
                    document = Path.GetFileName(Files.FileName);
                    objEncroch.KMLFileName = Files.FileName;
                    string FileFullName = DateTime.Now.Ticks + "_" + document;
                    path = this.CopyFileToSharedLocation(fileGenName.ToString(), FileFullName, Files);
                    byte[] filedata = this.StreamFile(path);
                    objEncroch.KMLFile = filedata;
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    objEncroch.KMLFileName = null;
                    objEncroch.KMLFile = null;
                }
                objEncroch.EnteredBy = Convert.ToInt64(Session["UserId"]);
                objEncroch.FileStatus = 0;
                Mapper.CreateMap<EncroachmentView, Tbl_Encroachment>();
                Tbl_Encroachment ObjUserModel = Mapper.Map<EncroachmentView, Tbl_Encroachment>(objEncroch);

                this.dbContext.Tbl_Encroachment.Add(ObjUserModel);
                this.dbContext.SaveChanges();

                long Id = this.dbContext.Tbl_Encroachment.Max(p => p.ID);
                var result = this.dbContext.Tbl_Encroachment.FirstOrDefault(a => a.ID == Id);
                if (Convert.ToString(result.EN_Code) != string.Empty)
                {
                    if (Session["EncoachmentList"] != null)
                    {
                        List<EncroacherDetailsView> listEncroachView = (List<EncroacherDetailsView>)Session["EncoachmentList"];
                        foreach (EncroacherDetailsView objItems in listEncroachView)
                        {
                            objItems.EN_Code = result.EN_Code;
                            Mapper.CreateMap<EncroacherDetailsView, Tbl_Encroacher_Details>();
                            Tbl_Encroacher_Details ObjEnrochView = Mapper.Map<EncroacherDetailsView, Tbl_Encroacher_Details>(objItems);
                            this.dbContext.Tbl_Encroacher_Details.Add(ObjEnrochView);
                            this.dbContext.SaveChanges();
                        }
                    }
                    #region Insert GIS Information
                    if (TempData["GISModel"] != null)
                    {
                        List<GISDataBaseModel> GISInformationList = TempData["GISModel"] as List<GISDataBaseModel>;

                        #region Convert Model Into Datatable
                        string JSONString = Newtonsoft.Json.JsonConvert.SerializeObject(GISInformationList);
                        DataTable GISTable = (DataTable)Newtonsoft.Json.JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
                        GISInformationRepo GIS = new GISInformationRepo();
                        DataSet ds = GIS.InsertGISInformation(GISTable, Convert.ToString(result.EN_Code), Convert.ToInt64(Session["UserID"]));
                        #endregion
                    }
                    #endregion
                    TempData["RegistrationMsg"] = "Encroachment registred with id:" + result.EN_Code;
                }
                else
                {
                    TempData["RegistrationMsg"] = "Something went wrong please try after some time!!!";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Registration", "Encroachment");
        }

        /// <summary>
        /// Function to save file to physical location
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="fileName"></param>
        /// <param name="uploadPath"></param>
        /// <returns></returns>
        public string CopyFileToSharedLocation(string fileID, string fileName, HttpPostedFileBase uploadPath)
        {
            string fullPath = string.Empty;
            string fileFullPathLocal = string.Empty;
            try
            {
                ////Save file to local directory
                string filePathLocal = ConfigurationManager.AppSettings["FolderPathEncroachment"].ToString();
                string strDate = DateTime.Now.ToShortDateString().Replace(".", string.Empty);
                strDate = strDate.Replace(":", string.Empty);
                strDate = strDate.Replace("/", "_");
                fullPath = filePathLocal + strDate + "\\" + fileID;

                if (!Directory.Exists(@fullPath))
                    Directory.CreateDirectory(@fullPath);

                filePathLocal = @fullPath;
                fileFullPathLocal = Path.Combine(filePathLocal, fileName);
                uploadPath.SaveAs(fileFullPathLocal);

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return fileFullPathLocal;
        }
        /// <summary>
        /// function convert file into byte
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private byte[] StreamFile(string filename)
        {
            byte[] imageData;
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                imageData = new byte[fs.Length];
                fs.Read(imageData, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return imageData;
        }

        /// <summary>
        /// function to download into zip file 
        /// </summary>
        /// <param name="En_Code"></param>
        /// <returns></returns>
        public ActionResult ZipDownload(string En_Code)
        {
            try
            {
                tbl_UserProfiles tblUserProfile = new tbl_UserProfiles();
                En_Code = Encryption.decrypt(En_Code);
                List<FileZip> lstZip = new List<FileZip>();
                Tbl_Encroachment tblEnch = dbContext.Tbl_Encroachment.FirstOrDefault(i => i.EN_Code == En_Code);
                tbl_mst_ForestEmployees Objtbl_mst_ForestEmployees = dbContext.tbl_mst_ForestEmployees.FirstOrDefault(i => i.ROWID == tblEnch.InvestigationOfficer);
                if (Objtbl_mst_ForestEmployees != null)
                {
                    tblUserProfile = dbContext.tbl_UserProfiles.FirstOrDefault(i => i.Ssoid == Objtbl_mst_ForestEmployees.SSO_ID);
                }

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
                        return RedirectToAction("Registration", "Encroachment");
                    }
                }
                else
                {

                    TempData["ZipMsg"] = "File not exists";
                    return RedirectToAction("Registration", "Encroachment");
                }

            }
            catch (Exception ex)
            {
                ErrorLog.Error(ex);
                throw;
            }
            return null;
        }
        public ActionResult getGISDataOld(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<GISDataBaseModel> GISInformationList = new List<GISDataBaseModel>();
            List<clsPermission> fixedlandlist = new List<clsPermission>();
            string aid = string.Empty;
            string  StrGISInformationList = string.Empty;
            try
            {

            //    if (Convert.ToString(Session["PermissionTypeID"]) != "") { aid = Convert.ToString(Session["PermissionTypeID"]); }
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {

                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));
                    #region "Muliple List"
                    string LAT = string.Empty, Long = string.Empty;
                    foreach (var dr in myDeserializedObj)
                    {
                        #region "KML and Lat-Long"
                        if (form["locCentroid"].ToString() != "")
                        {
                            if (form["locCentroid"].ToString().Contains(","))
                            {
                                string[] locCentroid = form["locCentroid"].ToString().Split(',');
                                LAT = locCentroid[1] == "NA" ? "" : locCentroid[1];
                                Long = locCentroid[0] == "NA" ? "" : locCentroid[0];
                            }
                        }

                        #endregion

                        clsPermission cls = new clsPermission();
                        cls.Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        cls.Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        cls.Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        cls.Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        cls.Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        cls.Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM;
                        cls.Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM;
                        cls.Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM;
                        cls.Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM;
                        cls.Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM;
                        cls.areaName = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        cls.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE;
                        cls.Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd;
                        cls.Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM;
                        fixedlandlist.Add(cls);

                        GISDataBaseModel gisModel = new GISDataBaseModel();
                        gisModel.DIV_CODE = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        gisModel.DIST_CODE = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        gisModel.BLK_CODE = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        gisModel.GP_CODE = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        gisModel.VILL_CODE = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        gisModel.Area = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        gisModel.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : string.IsNullOrEmpty(dr.FOREST_DIVCODE) ? "N/A" : dr.FOREST_DIVCODE;
                        gisModel.GPSLat = LAT;
                        gisModel.GPSLong = Long;
                        gisModel.GISFilePath = Convert.ToString(form["filePath"]);
                        gisModel.GISOrignalFilePath = Convert.ToString(form["originalFileName"]);
                        gisModel.GISID = form["gisid"].ToString();
                        gisModel.AreaShapeInHactor = Convert.ToDecimal(form["shapeArea"].ToString());

                   //     TempData["GPSLat"] = LAT;
                   //     TempData["GPSLong"] = Long;
                  //      TempData["shapeArea"] = gisModel.AreaShapeInHactor;
                        //objmodel.GPSLat = LAT; // ak
                        //objmodel.GPSLong = Long; //ak
                        GISInformationList.Add(gisModel);

                    }

                  
                    #endregion
                    //   TempData["GISModelLIst"] = fixedlandlist;
                    //   TempData["GISModel"] = GISInformationList;
                    //objmodel.ConditionGISMode = true;
                }
                else
                {
                    GISInformationList = new List<GISDataBaseModel>();
                 //   TempData["GISModel"] = GISInformationList;

                    fixedlandlist = new List<clsPermission>();
                 //   ViewData["GISModelLIst"] = fixedlandlist;



                }

                  string ster = Newtonsoft.Json.JsonConvert.SerializeObject(fixedlandlist);
                  StrGISInformationList = Newtonsoft.Json.JsonConvert.SerializeObject(GISInformationList);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);
            }

            string strfilepath = DateTime.Now.Ticks + "fixedlandJSONList" + GISInformationList[0].GISID + ".txt";         
            string path = Server.MapPath("~/Documents/TempData/" + strfilepath);
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                sw.WriteLine(StrGISInformationList);
            }

            return RedirectToAction("Registration", "Encroachment", strfilepath);
        }

        public ActionResult getGISData(FormCollection form)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            List<GISDataBaseModel> GISInformationList = new List<GISDataBaseModel>();
            List<clsPermission> fixedlandlist = new List<clsPermission>();
            string filepath1 = string.Empty;
            string filepath2 = string.Empty;
            try
            {
                if (Convert.ToString(form["successFlag"]).ToLower() == "true")
                {

                    List<clsPermission> myDeserializedObj = (List<clsPermission>)Newtonsoft.Json.JsonConvert.DeserializeObject(form["ids"], typeof(List<clsPermission>));
                    #region "Muliple List"
                    string LAT = string.Empty, Long = string.Empty;
                    foreach (var dr in myDeserializedObj)
                    {
                        #region "KML and Lat-Long"
                        if (form["locCentroid"].ToString() != "")
                        {
                            if (form["locCentroid"].ToString().Contains(","))
                            {
                                string[] locCentroid = form["locCentroid"].ToString().Split(',');
                                LAT = locCentroid[1] == "NA" ? "" : locCentroid[1];
                                Long = locCentroid[0] == "NA" ? "" : locCentroid[0];
                            }
                        }

                        #endregion

                        clsPermission cls = new clsPermission();
                        cls.Div_Cd = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        cls.Dist_Cd = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        cls.Blk_Cd = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        cls.Gp_Cd = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        cls.Vlg_Cd = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        cls.Div_NM = dr.Div_NM == "NA" ? "N/A" : dr.Div_NM;
                        cls.Dist_NM = dr.Dist_NM == "NA" ? "N/A" : dr.Dist_NM;
                        cls.Block_NM = dr.Block_NM == "NA" ? "N/A" : dr.Block_NM;
                        cls.Gp_NM = dr.Gp_NM == "NA" ? "N/A" : dr.Gp_NM;
                        cls.Village_NM = dr.Village_NM == "NA" ? "N/A" : dr.Village_NM;
                        cls.areaName = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        cls.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : dr.FOREST_DIVCODE;
                        cls.Tehsil_Cd = dr.Tehsil_Cd == "NA" ? "" : dr.Tehsil_Cd;
                        cls.Tehsil_NM = dr.Tehsil_NM == "NA" ? "" : dr.Tehsil_NM;
                        fixedlandlist.Add(cls);

                        GISDataBaseModel gisModel = new GISDataBaseModel();
                        gisModel.DIV_CODE = dr.Div_Cd == "NA" ? "" : dr.Div_Cd;
                        gisModel.DIST_CODE = dr.Dist_Cd == "NA" ? "" : dr.Dist_Cd;
                        gisModel.BLK_CODE = dr.Blk_Cd == "NA" ? "" : dr.Blk_Cd;
                        gisModel.GP_CODE = dr.Gp_Cd == "NA" ? "" : dr.Gp_Cd;
                        gisModel.VILL_CODE = dr.Vlg_Cd == "NA" ? "" : dr.Vlg_Cd;
                        gisModel.Area = dr.areaName == "NA" ? "N/A" : dr.areaName;
                        gisModel.FOREST_DIVCODE = dr.FOREST_DIVCODE == "NA" ? "N/A" : string.IsNullOrEmpty(dr.FOREST_DIVCODE) ? "N/A" : dr.FOREST_DIVCODE;
                        gisModel.GPSLat = LAT;
                        gisModel.GPSLong = Long;
                        gisModel.GISFilePath = Convert.ToString(form["filePath"]);
                        gisModel.GISOrignalFilePath = Convert.ToString(form["originalFileName"]);
                        gisModel.GISID = form["gisid"].ToString();
                        gisModel.AreaShapeInHactor = Convert.ToDecimal(form["shapeArea"].ToString());

                        GISInformationList.Add(gisModel);
                    }
                    string ster = Newtonsoft.Json.JsonConvert.SerializeObject(fixedlandlist);
                    #endregion
                    string str1 = JsonConvert.SerializeObject(fixedlandlist);
                    string str2 = JsonConvert.SerializeObject(GISInformationList);
                    Random r = new Random();
                    Int64 number = r.Next(1, 100);
                    filepath1 = DateTime.Now.Ticks + "FixedlandlistJSONList" + number + ".txt";
                    filepath2 = DateTime.Now.Ticks + "GISInformationListJSONList" + number + ".txt";
                    string path1 = Server.MapPath("~/Documents/EnchroachmentGISData/" + filepath1);
                    string path2 = Server.MapPath("~/Documents/EnchroachmentGISData/" + filepath2);
                    using (StreamWriter sw1 = System.IO.File.CreateText(path1))
                    {
                        sw1.WriteLine(str1);
                    }
                    using (StreamWriter sw2 = System.IO.File.CreateText(path2))
                    {
                        sw2.WriteLine(str2);
                    }
                   
                }
                else
                {
                    //GISInformationList = new List<GISDataBaseModel>();
                    //TempData["GISModel"] = GISInformationList;

                    //fixedlandlist = new List<clsPermission>();
                    //ViewData["GISModelLIst"] = fixedlandlist;

                }
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, 0);
            }
            return RedirectToAction("Registration", "Encroachment", new { docId = filepath1 + "-" + filepath2 });
            // return RedirectToAction("~/Encroachment/Registration");
        }
       


        #region Get Encroachment Details Developed by Rajveer
        public ActionResult GetEncroachmentDetails(string EnchId)
        {
            List<EncroacherDetailsView> model = new List<EncroacherDetailsView>();
            try
            {
                var param1 = new SqlParameter("@EN_Code", EnchId);
                var param2 = new SqlParameter("@Action", "Details");
                var result = FMDSS.Models.Utility.DynamicSqlQuery(dbContext.Database, "SP_EncrocherDetailById @EN_Code,@Action", param1, param2);

                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EncroacherDetailsView>>(str).ToList();

                if (model == null)
                {
                    model = new List<EncroacherDetailsView>();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetEncroachmentDetails" + "_" + "Encroachment", 0, DateTime.Now, Convert.ToInt64(Session["USERID"]));

            }

            return PartialView("_GetEnchroachmentDetails", model);
        }
        #endregion

    }
}
