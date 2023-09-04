using FMDSS.Entity.ViewModel;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class CommonController : Controller
    {
        #region Properties & Variables
        private ICommonRepository _repository;
        #endregion 

        #region [Constructor]
        public CommonController()
        {
            _repository = new CommonRepository();
        }
        #endregion

        /// <summary>
        /// Get Circle User wise
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCircle()
        {
            try
            {
                var data = _repository.GetDropdownData(4, string.Empty);
                return Json(new { data = data, IsError = false}, JsonRequestBehavior.AllowGet);
            }
            catch(Exception)
            {
                return Json(new { IsError = true}, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get All Circle 
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllCircle()
        {
            try
            {
                var data = _repository.GetDropdownData(1, string.Empty);
                return Json(new { data = data, IsError = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Div User wise
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDivision(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(5, parentID);
                return Json(new { data = data, IsError = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get All Division Circle Wise
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllDivision(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(2, parentID);
                return Json(new { data = data, IsError = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Range User Wise
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetRange(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(6, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get All Range Division Wise
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetAllRange(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(3, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetNaka(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(8, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAttachedEvidence(long objectID, int objectTypeID)
        {
            var model = _repository.GetAttachedDocument(objectID, objectTypeID);
            return PartialView("_AttachedEvidence", model);
        }

        [HttpPost]
        public ActionResult UploadFileSingle(int docTypeID)
        {
            var isError = false;
            var path = string.Empty;
            string FileName = string.Empty;
            string FileReplaceName = string.Empty;
            string FileFullName = string.Empty;
            string FilePath = Globals.Util.GetAppSettings("TempDocumentPath");

            List<CommonDocument> lstDoc = new List<CommonDocument>();
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    var docType = Globals.Util.GetListFromTable<CommonDocumentType>(_repository.GetDocumentType(docTypeID)).FirstOrDefault();
                    //Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    string strDoc = string.Empty;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            FileName = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            FileName = file.FileName;
                        }
                        FileFullName = DateTime.Now.Ticks + "_" + FileName;
                        path = Path.Combine(FilePath, FileFullName);
                        file.SaveAs(Server.MapPath("~/" + FilePath + FileFullName));

                        if (Session["UploadFile"] == null)
                        {
                            lstDoc.Add(new CommonDocument
                            {
                                TempID = Guid.NewGuid().ToString(),
                                IsNew = true,
                                DocumentName = FileName,
                                DocumentPath = path,
                                DocumentTypeID = docTypeID,
                                DocumentLevel = docType.DocumentLevel,
                                DocumentTypeName = docType.DocumentTypeName
                            });
                            Session["UploadFile"] = lstDoc;
                        }
                        else
                        {
                            lstDoc = ((List<CommonDocument>)Session["UploadFile"]).Where(x=>!x.DocumentTypeID.Equals(docTypeID)).ToList(); 
                            lstDoc.Add(new CommonDocument
                            {
                                TempID = Guid.NewGuid().ToString(),
                                IsNew = true,
                                DocumentName = FileName,
                                DocumentPath = path,
                                DocumentTypeID = docTypeID,
                                DocumentLevel = docType.DocumentLevel,
                                DocumentTypeName = docType.DocumentTypeName
                            });
                            Session["UploadFile"] = lstDoc;
                        }
                    }
                    if (docType.DocumentLevel > 0)
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentTypeID == docTypeID).ToList();
                    }
                    else
                    {
                        lstDoc = lstDoc.Where(x => x.DocumentLevel == 0).ToList();
                    }

                }
                catch (Exception)
                {
                    isError = true;
                }
                return Json(new { isError = isError, data = lstDoc, returnMsg = "File Uploaded Successfully!" });
            }
            else
            {
                return Json("No files selected.");
            }
        }

        #region New
        [HttpPost]
        public JsonResult GetTehsilNew(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(9, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetBlockNew(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(10, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetGPNew(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(11, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetVillageNew(string parentID)
        {
            try
            {
                var data = _repository.GetDropdownData(12, parentID);
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsError = true }, JsonRequestBehavior.AllowGet);
            }
        }  
        #endregion
    }
}
