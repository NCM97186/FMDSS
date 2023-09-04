using FMDSS.Models;
using FMDSS.Models.Master;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class ElephantController : BaseController
    {
        //
        // GET: /Elephant/


        List<FMDSS.Models.ElephantInfo> ElephantInfoLst = new List<FMDSS.Models.ElephantInfo>();
        List<SelectListItem> lstISactive = new List<SelectListItem>();

        List<SelectListItem> StateList = new List<SelectListItem>();
        List<SelectListItem> ElephantList = new List<SelectListItem>();
        List<SelectListItem> TransportList = new List<SelectListItem>();





        #region "ElephantInfo"

        public ActionResult ElephantInfo(bool? RecordStatus)
        {
            if (RecordStatus == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                if (Convert.ToBoolean(RecordStatus) == true)
                    ViewBag.RecordStatus = 1;
                else
                    ViewBag.RecordStatus = 0;
            }

            List<SelectListItem> ElephantInfo = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ElephantInfo obj = new ElephantInfo();
            try
            {

                DataTable dtf = obj.Select_ElephantInfos();

                int count = 1;
                foreach (DataRow dr in dtf.Rows)
                {
                    ElephantInfoLst.Add(
                        new ElephantInfo()
                        {
                            Index = count,
                            ElephantId = Convert.ToInt32(dr["ElephantId"].ToString()),
                            ElephantName = Convert.ToString(dr["ElephantName"].ToString()),
                            RegNumber = Convert.ToString(dr["RegNumber"].ToString()),
                            ssoid = Convert.ToString(dr["ssoid"].ToString()),
                            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                        });
                    count += 1;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return View(ElephantInfoLst);
        }
        public ActionResult ADDUpdateElephantInfo(ElephantInfo oElephantInfo,HttpPostedFileBase fileUpload, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2)
        {
            List<SelectListItem> ElephantInfo = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {
                #region Upload File
                string FileFullName = string.Empty;
                string FilePath = "~/ElephantDocument/";
                int count = 0;
                if (fileUpload != null)
                {
                    FileFullName = "PurchasedIllegalFile_"+DateTime.Now.Ticks + "_" + fileUpload.FileName;
                    Request.Files[count].SaveAs(Server.MapPath(FilePath + FileFullName));
                    oElephantInfo.PurchasedIllegalFile = FilePath.Replace('~', ' ').Trim() + FileFullName;
                    count++;

                }
                if (fileUpload1 != null)
                {
                    FileFullName = "Verterinary_" + DateTime.Now.Ticks + "_" + fileUpload1.FileName;
                    Request.Files[count].SaveAs(Server.MapPath(FilePath + FileFullName));
                    oElephantInfo.VerterinaryFile = FilePath.Replace('~', ' ').Trim() + FileFullName;
                    count++;

                }
                if (fileUpload2 != null)
                {
                    FileFullName = "OwnershipCertificate_" + DateTime.Now.Ticks + "_" + fileUpload2.FileName;
                    Request.Files[count].SaveAs(Server.MapPath(FilePath + FileFullName));
                    oElephantInfo.OwnershipCertificateFile = FilePath.Replace('~', ' ').Trim() + FileFullName;

                }
                #endregion
                ElephantInfo obj = new ElephantInfo();
                oElephantInfo.EnteredBy = Convert.ToString(UserID);
                DataTable dtf = obj.AddUpdateElephantInfo(oElephantInfo);
                status = dtf.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return RedirectToAction("ElephantInfo", new { RecordStatus = status });
        }
        public ActionResult GetElephantInfo(string ElephantId)
        {

            ElephantInfo obj = new ElephantInfo();
            List<SelectListItem> ElephantInfo = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            try
            {

                //List<SelectListItem> DistrictName = new List<SelectListItem>();
                //DataTable dt = obj.SelectAllDistricts();
                //ViewBag.fname = dt;


                //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                //{
                //    DistrictName.Add(new SelectListItem { Text = @dr["DistrictName"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                //}

                //System.Globalization.CompareInfo cul = new System.Globalization.CompareInfo("ru-RU");

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                List<SelectListItem> lstIGender = new List<SelectListItem>();
                lstIGender.Add(new SelectListItem { Text = "Male", Value = "Male" });
                lstIGender.Add(new SelectListItem { Text = "Female ", Value = "Female" });

                ViewBag.ListGender = lstIGender;

                ViewBag.ISactivelst = lstISactive;

                ViewBag.OpType = (ElephantId == "0" ? "Add Elephant Information" : "Edit Elephant Information");


                DataTable dtf = obj.Select_ElephantInfo(Convert.ToInt32(ElephantId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ElephantInfo
                    {
                        ElephantId = Convert.ToInt32(dr["ElephantId"].ToString()),
                        ElephantName = Convert.ToString(dr["ElephantName"].ToString()),
                        ssoid = Convert.ToString(dr["ssoid"].ToString()),
                        RegNumber = Convert.ToString(dr["RegNumber"].ToString()),
                        LicenceNumber = Convert.ToString(dr["MicroChipNumber"].ToString()),
                        Age = Convert.ToString(dr["Age"].ToString()),
                        Colour = Convert.ToString(dr["Colour"].ToString()),
                        ColourofEye = Convert.ToString(dr["ColourofEye"].ToString()),
                        Height = Convert.ToDouble(dr["Height"]),
                        Length = Convert.ToDouble(dr["Length"]),
                        isActive = Convert.ToInt32(dr["isActive"]),
                        NeekGirth = Convert.ToString(dr["NeekGirth"].ToString()),
                        Weight = Convert.ToDouble(dr["Weight"].ToString()),
                        NoofNail = Convert.ToInt32(dr["NoofNail"]),
                        LengthofTusk = Convert.ToDouble(dr["LengthofTusk"]),
                        IdentificationMarks = Convert.ToString(dr["IdentificationMarks"].ToString()),
                        NoofInsuranceMarks = Convert.ToDouble(dr["NoofInsuranceMarks"]),
                        //InsuranceDate = Convert.ToDateTime(dr["InsuranceDate"],cul),
                        //VerterinaryCertificatedDate = Convert.ToDateTime(dr["VerterinaryCertificatedDate"].ToString(), cul),
                        InsuranceDate = Convert.ToString(dr["InsuranceDate"].ToString()),
                        VerterinaryCertificatedDate = Convert.ToString(dr["VerterinaryCertificatedDate"].ToString()),
                        PresentMarketValue = Convert.ToString(dr["PresentMarketValue"]),
                        SourceofPurchase = Convert.ToString(dr["SourceofPurchase"]),
                        Gender = Convert.ToString(dr["Gender"]),
                        OperationType = "Edit Elephant Information",
                        PurchasedIllegalFile = Convert.ToString(dr["PurchasedIllegalFile"]),
                        OwnershipCertificateFile = Convert.ToString(dr["OwnershipCertificateFile"]),
                        VerterinaryFile = Convert.ToString(dr["VerterinaryFile"])
                    };

                }
                if(string.IsNullOrEmpty(obj.IdentificationMarks))
                {
                    obj.IdentificationMarks = "NA";
                }
                //ViewBag.DistrictLst = DistrictName;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("_partialElephantInfo", obj);
        }

        #endregion


        #region Elephant Movement
        public ActionResult ElephantIndex(string ID)
        {
            List<ElephantMovement> obj = new List<ElephantMovement>();

            if (ID == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                ViewBag.RecordStatus = 1;
                ViewBag.RecordID = ID;
            }
            ElephantMovement obj1 = new ElephantMovement();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string IDs = string.Empty;

            try
            {

                DataTable dt = obj1.DetailsElephantMovement(UserID.ToString());

                int count = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new ElephantMovement()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FromDate = Convert.ToString(dr["ElephantName"].ToString()),
                            ToDate = Convert.ToString(dr["TransportType"].ToString()),
                            ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString()),
                            ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString()),

                            MovementFrom = Convert.ToString(dr["MovementFrom"].ToString()),
                            MovementTo = Convert.ToString(dr["MovementTo"].ToString()),
                            MedicalCertificateNumber = Convert.ToString(dr["MedicalCertificateNumber"]),
                            OtherStateTPNumber = Convert.ToString(dr["OtherStateNocNumber"]),
                            MovementStatus = Convert.ToString(dr["MovementStatus"]),

                        });
                    count += 1;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(obj);
        }

        public ActionResult ElephantMovement(string ID)
        {


            List<SelectListItem> ElephantInfo = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ElephantMovement obj = new ElephantMovement();
            try
            {
                ViewBag.District = ElephantInfo;
                DataTable dts = obj.GetAllState();
                foreach (System.Data.DataRow dr in dts.Rows)
                {
                    StateList.Add(new SelectListItem { Text = @dr["StateName"].ToString(), Value = @dr["StateID"].ToString() });
                }
                ViewBag.From_place = StateList;
                ViewBag.FromStateId = StateList.Where(x => x.Value == "8");


                DataTable dtElephant = obj.GetElephants(UserID);
                foreach (System.Data.DataRow dr in dtElephant.Rows)
                {
                    ElephantList.Add(new SelectListItem { Text = @dr["ElephantName"].ToString(), Value = @dr["ElephantId"].ToString() });
                }
                ViewBag.ElephantList = ElephantList;

                DataTable dtTransport = obj.GetElephantTransport(UserID);
                foreach (System.Data.DataRow dr in dtTransport.Rows)
                {
                    TransportList.Add(new SelectListItem { Text = @dr["NAME"].ToString(), Value = @dr["ID"].ToString() });
                }
                ViewBag.TransportList = TransportList;



                //int count = 1;
                //foreach (DataRow dr in dtf.Rows)
                //{
                //    ElephantInfoLst.Add(
                //        new ElephantInfo()
                //        {
                //            Index = count,
                //            ElephantId = Convert.ToInt64(dr["ElephantId"].ToString()),
                //            ElephantName = Convert.ToString(dr["ElephantName"].ToString()),
                //            OwnerName = Convert.ToString(dr["OwnerName"].ToString()),
                //            MicroChipNumber = Convert.ToInt64(dr["MicroChipNumber"].ToString()),
                //            IsactiveView = Convert.ToBoolean(dr["isActive"]),

                //        });
                //    count += 1;
                //}
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("ElephantMovement");
        }

        public ActionResult ADDElephantInfo(ElephantMovement obj, IEnumerable<HttpPostedFileBase> MedicalCertificateDOC, HttpPostedFileBase MovementRecommendationLatter)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            DataTable dt;

            string IDs = string.Empty;

            try
            {

                string FilePath = "~/ElephantDocument/";
                string Document = "", path = "";
                if (ModelState.IsValid)
                {

                    foreach (var File in MedicalCertificateDOC)
                    {
                        if (File != null && File.ContentLength > 0)
                        {
                            Document = Path.GetFileName(File.FileName);
                            String FileFullName = DateTime.Now.Ticks + "_" + Document;
                            path = Path.Combine(FilePath, FileFullName);
                            obj.MedicalCertificateDOC = path;
                            File.SaveAs(Server.MapPath(FilePath + FileFullName));
                        }
                        else
                        {
                            obj.MedicalCertificateDOC = "";
                        }
                    }

                    Document = "";
                    path = "";

                    if (MovementRecommendationLatter != null && MovementRecommendationLatter.ContentLength > 0)
                    {
                        Document = Path.GetFileName(MovementRecommendationLatter.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + Document;
                        path = Path.Combine(FilePath, FileFullName);
                        obj.MovementRecommendationLatter = path;
                        MovementRecommendationLatter.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        obj.MovementRecommendationLatter = "";
                    }

                    obj.UserID = UserID;

                    dt = obj.SaveElephantMovement(obj);
                    IDs = Convert.ToString(dt.Rows[0][0]);

                    #region Send Mail and Message
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        SendSMSEmailForSuccessTransaction("CreateTP", IDs, "GETUSERDETAILSFORSENDSMSANDEMAILforElephantTP");
                    }
                    #endregion
                }

                return RedirectToAction("ElephantIndex", "Elephant", new { ID = IDs });
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }

        #endregion


        #region Elephant Review Approver
        public JsonResult DistrictData(int StateId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            ElephantMovement obj = new ElephantMovement();
            try
            {
                if ((!String.IsNullOrEmpty(StateId.ToString())))
                {

                    DataTable dt = obj.GetAllDistrict(StateId);
                    ViewBag.fname = dt;
                    foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
                    {
                        items.Add(new SelectListItem { Text = @dr["Dist_name"].ToString(), Value = @dr["DIST_CODE"].ToString() });
                    }
                    ViewBag.RangeCode = items;
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }

        public ActionResult ElephantReviewApprover()
        {
            List<ElephantMovement> obj = new List<ElephantMovement>();
            List<ElephantMovement> obj1 = new List<ElephantMovement>();
            List<ElephantMovement> obj2 = new List<ElephantMovement>();

            ElephantMovement objele = new ElephantMovement();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string IDs = string.Empty;

            try
            {


                DataSet DS = new DataSet();
                DS = objele.ElephantReviewApprover(UserID);

                int count = 1;
                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    obj.Add(
                        new ElephantMovement()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FromDate = Convert.ToString(dr["ElephantName"].ToString()),
                            ToDate = Convert.ToString(dr["TransportType"].ToString()),
                            ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString()),
                            ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString()),

                            MovementFrom = Convert.ToString(dr["MovementFrom"].ToString()),
                            MovementTo = Convert.ToString(dr["MovementTo"].ToString()),
                            MedicalCertificateNumber = Convert.ToString(dr["MedicalCertificateNumber"]),
                            OtherStateTPNumber = Convert.ToString(dr["OtherStateNocNumber"]),
                            MovementStatus = Convert.ToString(dr["MovementStatus"]),

                        });
                    count += 1;
                }

                ViewData["ToBeAssigned"] = obj;

                count = 1;

                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    obj1.Add(
                        new ElephantMovement()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FromDate = Convert.ToString(dr["ElephantName"].ToString()),
                            ToDate = Convert.ToString(dr["TransportType"].ToString()),
                            ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString()),
                            ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString()),

                            MovementFrom = Convert.ToString(dr["MovementFrom"].ToString()),
                            MovementTo = Convert.ToString(dr["MovementTo"].ToString()),
                            MedicalCertificateNumber = Convert.ToString(dr["MedicalCertificateNumber"]),
                            OtherStateTPNumber = Convert.ToString(dr["OtherStateNocNumber"]),
                            MovementStatus = Convert.ToString(dr["MovementStatus"]),
                            ActionTakenBy = Convert.ToString(dr["ActionTakenBy"]),
                        });
                    count += 1;
                }


                ViewData["PendingRequests"] = obj1;

                count = 1;

                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    obj2.Add(
                        new ElephantMovement()
                        {
                            Index = count,
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            FromDate = Convert.ToString(dr["ElephantName"].ToString()),
                            ToDate = Convert.ToString(dr["TransportType"].ToString()),
                            ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString()),
                            ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString()),

                            MovementFrom = Convert.ToString(dr["MovementFrom"].ToString()),
                            MovementTo = Convert.ToString(dr["MovementTo"].ToString()),
                            MedicalCertificateNumber = Convert.ToString(dr["MedicalCertificateNumber"]),
                            OtherStateTPNumber = Convert.ToString(dr["OtherStateNocNumber"]),
                            MovementStatus = Convert.ToString(dr["MovementStatus"]),
                            ActionTakenBy = Convert.ToString(dr["ActionTakenBy"]),
                            ActionTakenOn = Convert.ToString(dr["ActionTakenOn"]),

                        });
                    count += 1;
                }

                ViewData["MyActions"] = obj2;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View();

        }

        public ActionResult GetElephantRevApprvDetails(string id)
        {

            ElephantMovement objele = new ElephantMovement();



            DataSet DS = new DataSet();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string IDs = string.Empty;

            try
            {

                DS = objele.GetElephantRevApprvDetails(UserID.ToString(), id);

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    objele.RequestID = Convert.ToString(dr["RequestID"].ToString());
                    objele.FromDate = Convert.ToString(dr["ElephantName"].ToString());
                    objele.ToDate = Convert.ToString(dr["TransportType"].ToString());
                    objele.ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString());
                    objele.ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString());
                    objele.MovementFrom = Convert.ToString(dr["MovementFrom"].ToString());
                    objele.MovementTo = Convert.ToString(dr["MovementTo"].ToString());
                    objele.MedicalCertificateNumber = Convert.ToString(dr["MedicalCertificateNumber"]);
                    objele.OtherStateTPNumber = "";
                    objele.MovementStatus = Convert.ToString(dr["MovementStatus"]);
                    ViewBag.OtherStateTPNumber = Convert.ToString(dr["OtherStateNocNumber"]);
                    objele.ElephantName = Convert.ToString(dr["ElephantName"]);
                    objele.TransportType = Convert.ToString(dr["TransportType"]);



                }

                foreach (DataRow dr in DS.Tables[2].Rows)
                {
                    ViewBag.CurrentStatusID = Convert.ToString(dr["StatusID"].ToString());
                    ViewBag.CurrentStatusName = Convert.ToString(dr["StatusDesc"].ToString());
                }

                foreach (DataRow dr in DS.Tables[3].Rows)
                {
                    ElephantList.Add(new SelectListItem { Text = @dr["REASON_DESC"].ToString(), Value = @dr["REASON_ID"].ToString() });


                }

                objele.ListRejectedReason = ElephantList;

                objele.ListRajasthanAreaDCF = new SelectList(DS.Tables[4].AsDataView(), "UserID", "Ssoid").ToList();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("ElephantAppReviewApprover", objele);

        }


        public ActionResult ElephantAppReviewApprover(ElephantMovement obj, HttpPostedFileBase OtherStateNocDoc, string command)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            DataTable dt;

            string IDs = string.Empty;

            try
            {

                string FilePath = "~/ElephantDocument/";
                string Document = "", path = "";

                if (OtherStateNocDoc != null && OtherStateNocDoc.ContentLength > 0)
                {
                    Document = Path.GetFileName(OtherStateNocDoc.FileName);
                    String FileFullName = DateTime.Now.Ticks + "_" + Document;
                    path = Path.Combine(FilePath, FileFullName);
                    obj.OtherStateNocDoc = path;
                    OtherStateNocDoc.SaveAs(Server.MapPath(FilePath + FileFullName));
                }
                else
                {
                    obj.OtherStateNocDoc = "";
                }

                obj.UserID = UserID;
                obj.AssignTo = UserID;
                obj.ActionStatus = command;
                if (obj.ActionStatus == "3")
                { obj.Reasons = string.Join(",", obj.RejectedReason); }


                dt = obj.SubmitElephantReviewApprover(obj);

                #region Send Mail and Message

                SendSMSEmailForSuccessTransaction("TPApproveOrReject", obj.RequestID, "GETUSERDETAILSFORSENDSMSANDEMAILforElephantTP");

                #endregion

                return RedirectToAction("ElephantReviewApprover");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }


        public ActionResult GetElephantAppAssignerDetails(string id)
        {

            ElephantMovement objele = new ElephantMovement();


            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string IDs = string.Empty;

            try
            {

                DataSet DS = new DataSet();
                DS = objele.GetElephantRevApprvDetails(UserID.ToString(), id);

                foreach (DataRow dr in DS.Tables[0].Rows)
                {
                    objele.RequestID = Convert.ToString(dr["RequestID"].ToString());
                    objele.FromDate = Convert.ToString(dr["ElephantName"].ToString());
                    objele.ToDate = Convert.ToString(dr["TransportType"].ToString());
                    objele.ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString());
                    objele.ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString());
                    objele.MovementFrom = Convert.ToString(dr["MovementFrom"].ToString());
                    objele.MovementTo = Convert.ToString(dr["MovementTo"].ToString());
                    objele.MedicalCertificateNumber = Convert.ToString(dr["MedicalCertificateNumber"]);
                    objele.OtherStateTPNumber = "";
                    objele.MovementStatus = Convert.ToString(dr["MovementStatus"]);
                    ViewBag.OtherStateTPNumber = Convert.ToString(dr["OtherStateNocNumber"]);
                    objele.ElephantName = Convert.ToString(dr["ElephantName"]);
                    objele.TransportType = Convert.ToString(dr["TransportType"]);

                }

                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    StateList.Add(new SelectListItem { Text = @dr["Ssoid"].ToString(), Value = @dr["UserID"].ToString() });


                }

                ViewBag.ListAssignTo = StateList;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return PartialView("ElephantAppAssigner", objele);

        }

        public ActionResult ElephantAppAssigner(ElephantMovement obj)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            DataTable dt;

            try
            {
                obj.UserID = UserID;
                dt = obj.SubmitElephantAssign(obj);

                return RedirectToAction("ElephantReviewApprover");
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }

        #endregion

        #region Issue NOC Other State Movement
        [HttpGet]
        public ActionResult IssueNOCOtherStateMovement(string ID)
        {
            List<OtherStateElephantMovement> obj = new List<OtherStateElephantMovement>();

            if (ID == null)
            {
                ViewBag.RecordStatus = -1;
            }
            else
            {
                ViewBag.RecordStatus = 1;
            }
            OtherStateElephantMovement obj1 = new OtherStateElephantMovement();

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            string IDs = string.Empty;

            try
            {

                DataTable dt = obj1.DetailsOtherStateElephantMovement(UserID.ToString());

                int count = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new OtherStateElephantMovement()
                        {
                            Index = count,
                            ID = Convert.ToInt64(dr["ID"].ToString()),
                            RequestID = Convert.ToString(dr["RequestID"].ToString()),
                            ReturnFromDate = Convert.ToString(dr["TravelDuration"].ToString()),
                            ReturnToDate = Convert.ToString(dr["ReturnDuration"].ToString()),

                            MovementFrom = Convert.ToString(dr["MovementFrom"].ToString()),
                            MovementTo = Convert.ToString(dr["MovementTo"].ToString()),
                            OtherStateRequestedTPNumber = Convert.ToString(dr["OtherStateRequestedTPNumber"]),
                            OtherStateContactNo = Convert.ToString(dr["OtherStateContactNo"]),
                            SSOID = Convert.ToString(dr["Ssoid"]),
                            //MovementStatus = Convert.ToString(dr["MovementStatus"]),

                        });
                    count += 1;
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return View(obj);
        }
        public ActionResult GetIssueNOCOtherStateMovement(string ID)
        {
            List<SelectListItem> ElephantInfo = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ElephantMovement obj = new ElephantMovement();
            try
            {
                ViewBag.District = ElephantInfo;
                DataTable dts = obj.GetAllState();
                foreach (System.Data.DataRow dr in dts.Rows)
                {
                    StateList.Add(new SelectListItem { Text = @dr["StateName"].ToString(), Value = @dr["StateID"].ToString() });
                }
                ViewBag.From_place = StateList;
                ViewBag.ToStateId = StateList.Where(x => x.Value == "8");

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }

            return PartialView("ElephantMovementFromOtherState");
        }
        [HttpPost]
        public ActionResult IssueNOCOtherStateMovement(OtherStateElephantMovement obj, HttpPostedFileBase OtherStateRequestedTPDoc)
        {

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);

            DataTable dt;

            string IDs = string.Empty;

            try
            {
                string FilePath = "~/ElephantDocument/";
                string Document = "", path = "";
                if (ModelState.IsValid)
                {
                    if (OtherStateRequestedTPDoc != null && OtherStateRequestedTPDoc.ContentLength > 0)
                    {
                        Document = Path.GetFileName(OtherStateRequestedTPDoc.FileName);
                        String FileFullName = DateTime.Now.Ticks + "_" + Document;
                        path = Path.Combine(FilePath, FileFullName);
                        obj.OtherStateRequestedTPDoc = path;
                        OtherStateRequestedTPDoc.SaveAs(Server.MapPath(FilePath + FileFullName));
                    }
                    else
                    {
                        obj.OtherStateRequestedTPDoc = "";
                    }

                    obj.UserID = UserID;

                    dt = obj.SaveOtherStateElephantMovement(obj);
                    IDs = Convert.ToString(dt.Rows[0][0]);

                    #region Send Mail and Message
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        SendSMSEmailForSuccessTransaction("CreateNOC", IDs, "GETUSERDETAILSFORSENDSMSANDEMAILforElephantNOCs");
                    }
                    #endregion
                }

                return RedirectToAction("IssueNOCOtherStateMovement", new { ID = IDs });
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }

        public JsonResult GetRajasthanAreaDCF(int DistrictId)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            List<SelectListItem> items = new List<SelectListItem>();
            ElephantMovement obj = new ElephantMovement();
            try
            {
                if ((!String.IsNullOrEmpty(DistrictId.ToString())))
                {

                    DataTable dt = obj.GetAllRajasthanAreaDCF(DistrictId);
                    items = new SelectList(dt.AsDataView(), "UserID", "Ssoid").ToList();
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return Json(new SelectList(items, "Value", "Text"));
        }


        #endregion

        public void SendSMSEmailForSuccessTransaction(string ACTION, string RequestId, string DatabaseAction)
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(RequestId, DatabaseAction);
            string IsApproveAndRejectStatus = string.Empty;
            if (DT.Rows.Count > 0)
            {
                string UserMailBody = UserMailSMSBody("Mail", ACTION, RequestId, DT);
                string subject = "Regarding Elephent Movement";

                if (Convert.ToString(DT.Rows[0]["ApplicantEmailId"]) != string.Empty)
                {
                    body = string.Empty;
                    #region SMS Email
                    // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["EmailId"].ToString(), string.Empty);
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmailId"].ToString(), string.Empty);
                    #endregion
                }
                if (Convert.ToString(DT.Rows[0]["ReviewApproveEmailId"]) != string.Empty)
                {
                    body = string.Empty;
                    #region SMS Email
                    // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["EmailId"].ToString(), string.Empty);
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ReviewApproveEmailId"].ToString(), string.Empty);
                    #endregion
                }

                string UserSmsBody = UserMailSMSBody("SMS", ACTION, RequestId, DT);
                if (Convert.ToString(DT.Rows[0]["ApplicantMobile"]) != string.Empty)
                {
                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["ApplicantMobile"]), UserSmsBody);
                }
                if (Convert.ToString(DT.Rows[0]["ReviewApproveMobile"]) != string.Empty)
                {
                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["ReviewApproveMobile"]), UserSmsBody);
                }
                if (Convert.ToString(DT.Rows[0]["OtherStateContactNo"]) != string.Empty)
                {
                    SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["OtherStateContactNo"]), UserSmsBody);
                }


            }
            #endregion
        }

        public string UserMailSMSBody(string MailSMS, string ACTION, string requestid, DataTable DT)
        {
            StringBuilder sb = new StringBuilder();
            if (ACTION == "CreateTP")
            {
                if (MailSMS == "Mail")
                {
                    sb.Append("Dear User,");
                    sb.Append("<br />");
                    sb.Append("<p>Thanks for applying for Elephent Moment Your Transit Permit Number is <b>" + requestid + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Please quote this ID for future communication.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");
                }
                else if (MailSMS == "SMS")
                {
                    sb.Append("Thanks for applying for Elephent Moment Your Transit Permit Number is " + requestid + "");
                    sb.Append("Please quote this ID for future communication.");
                }

            }
            else if (ACTION == "TPApproveOrReject")
            {
                if (MailSMS == "Mail")
                {
                    sb.Append("Dear User,");
                    sb.Append("<br />");
                    sb.Append("<p>With the refrence of Transit Permit Number " + requestid + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Current Status of your application is " + DT.Rows[0]["Status"].ToString() + " .");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");

                }
                else if (MailSMS == "SMS")
                {
                    sb.Append("With the refrence of your Transit Permit Number " + requestid + " is " + DT.Rows[0]["Status"].ToString() + " by Rajasthan CWLW.");
                }
            }

            else if (ACTION == "CreateNOC")
            {
                if (MailSMS == "Mail")
                {
                    sb.Append("Dear User,");
                    sb.Append("<br />");
                    sb.Append("<p>Thanks for applying  Elephent Moment NOC with refrence of latter number " + Convert.ToString(DT.Rows[0]["LetterNumber"]) + " , Your NOC Number is <b>" + Convert.ToString(DT.Rows[0]["RequestedId"]) + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Final Status of your application is Approve by CWLW Rajasthan.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");

                }
                else if (MailSMS == "SMS")
                {
                    sb.Append("Thanks for applying Elephent Moment NOC with refrence of latter number " + Convert.ToString(DT.Rows[0]["LetterNumber"]) + " , Your NOC Number is " + Convert.ToString(DT.Rows[0]["RequestedId"]) + ". Final Status of your application is Approve by CWLW Rajasthan .");
                }
            }

            return sb.ToString();
        }

        #region Elephant Print PDF
        public ActionResult ElephantPrintDetails(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ElephantMovement obj = new ElephantMovement();
            try
            {
                DataSet dtElephant = obj.TPElephant(ID);
                if (dtElephant != null)
                {
                    Print(dtElephant);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return RedirectToAction("ElephantIndex");
        }


        public void Print(DataSet ds)
        {
            string filepath = string.Empty;

            filepath = "~/PDFFolder/Elephant_" + DateTime.Now.Ticks.ToString() + ".pdf";

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            var subheadfont = FontFactory.GetFont("Times New Roman", 10, FontColour);

            var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
            var myfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);
            //boldFont.SetStyle(FontFactory.H);


            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            /////create Table
            // PdfPTable tablehead;
            // tablehead = new PdfPTable(3);

            PdfPTable Details = new PdfPTable(3) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 120;
            Details.SetTotalWidth(new float[] { 35f, 50f, 35f });


            cells = new PdfPCell(new Phrase("Office of the Addl. Pr.Chief Conservator of Forest & Chief Wildlife Warden,", boldFont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);



            cells = new PdfPCell(new Phrase("Rajasthan, Jaipur", boldFont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);


            cells = new PdfPCell(new Phrase(ds.Tables[0].Rows[0]["TPNumber"].ToString(), subheadfont)) { Border = 0 };
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Dated " + ds.Tables[0].Rows[0]["PublishDate"].ToString(), subheadfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", boldFont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Permissition to transport specified etc., under provision of the Wildlife (Protection) Act,1972.", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("TRANSIT PERMIT", boldFont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("                       The owner of the " + ds.Tables[0].Rows[0]["Gender"].ToString() + " elephant, Shri " + ds.Tables[0].Rows[0]["Name"].ToString() + " " + ds.Tables[0].Rows[0]["Address"].ToString() + " holding the ownership Certificate" + ds.Tables[0].Rows[0]["OwnershipCertification"].ToString() + " issued by Dy. Chief Wildlife Warden, Jaipur under section 42 of the Wildlife (Protection) Act.1972 is hereby permitted to transport " + ds.Tables[0].Rows[0]["Gender"].ToString() + " elephant named " + ds.Tables[0].Rows[0]["ElephantName"].ToString() + " & T.R. No. " + ds.Tables[0].Rows[0]["MicroChipNumber"].ToString() + " from " + ds.Tables[0].Rows[0]["MovementFrom"].ToString() + ", " + "Rajasthan" + " to " + ds.Tables[0].Rows[0]["MovementTo"].ToString() + " by " + ds.Tables[0].Rows[0]["TransportType"].ToString() + " from " + ds.Tables[0].Rows[0]["FromTravelDuration"].ToString() + " and return by same before " + ds.Tables[0].Rows[0]["ReturnDuration"].ToString() + "and report to " + ds.Tables[0].Rows[0]["REPORTINGTOAREADCF"].ToString() + ".", subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            //doc.Add(new Paragraph(Environment.NewLine));

            cells = new PdfPCell(new Phrase("                      Contravention of section 11(d) of Animal Cruelty Act, 1966 should not be done. Dy. Conservator of Forest, Wildlife, Zoo, Jaipur should ensured that elephant should not get injury during transportation in vehicle and vehicle should be quite comfortable.", subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            //doc.Add(new Paragraph(Environment.NewLine));

            cells = new PdfPCell(new Phrase("                      This Perit has been issued for one " + ds.Tables[0].Rows[0]["Gender"].ToString() + " elephant only on the condition that norms and standards for transportation as enclosed and the condition imposed by Pr. Chief Conservator of Forest (Wildlise ) & Chief Wildlife Warden, " + ds.Tables[0].Rows[0]["MovementTo"].ToString() + " by his letter no." + ds.Tables[0].Rows[0]["LatterNumber"].ToString() + " dated " + ds.Tables[0].Rows[0]["LatterDate"].ToString() + " will be strictly fulfilled.", subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Enclosed: As above", subheadfont)) { Border = 0 };
            //cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("( " + ds.Tables[0].Rows[0]["CWLWNAME"].ToString() + " )", boldFont)) { Border = 0 };
            cells = new PdfPCell(new Phrase( "APCCF & " )) { Border = 0 };  ///// Change by amit singh on 20-02-2019
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("", myfont)) { Border = 0 };
            //// cells.Colspan = 2;
            //cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            //Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("Addl. Pr. Chief Conservator of Forests", subheadfont)) { Border = 0 };
            //cells.Colspan = 2;
            //cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            //Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Place: " + "Jaipur", subheadfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);


            cells = new PdfPCell(new Phrase(" Chief Wildlife Warden", subheadfont)) { Border = 0 };
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            //cells = new PdfPCell(new Phrase("", myfont)) { Border = 0 };
            //// cells.Colspan = 2;
            //cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            //Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Dated: " + ds.Tables[0].Rows[0]["PublishDate"].ToString(), subheadfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Rajasthan, Jaipur.", subheadfont)) { Border = 0 };
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("", myfont)) { Border = 0 };
            // cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(ds.Tables[0].Rows[0]["TPNumber"].ToString(), subheadfont)) { Border = 0 };
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Dated " + ds.Tables[0].Rows[0]["PublishDate"].ToString(), subheadfont)) { Border = 0 };
            //cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Copy of the following for information and necessary action :-", subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Pr. Chief Conservator of Forests (Wildlife) & Chief Wildlife Waarden, " + ds.Tables[0].Rows[0]["MovementTo"].ToString() + " with reference there letter no. " + ds.Tables[0].Rows[0]["LatterNumber"].ToString() + " dated " + ds.Tables[0].Rows[0]["LatterDate"].ToString(), subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(ds.Tables[0].Rows[0]["REPORTINGTOAREADCF"]), subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Shri " + ds.Tables[0].Rows[0]["Name"].ToString() + " " + ds.Tables[0].Rows[0]["Address"].ToString(), subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);





           
           // cells = new PdfPCell(new Phrase("( " + ds.Tables[0].Rows[0]["CWLWNAME"].ToString() + " )", boldFont)) { Border = 0 };
            cells = new PdfPCell(new Phrase("APCCF & ")) { Border = 0 };  ///// Change by amit singh on 20-02-2019
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" Chief Wildlife Warden", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Rajasthan, Jaipur.", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Dated: " + ds.Tables[0].Rows[0]["PublishDate"].ToString(), subheadfont)) { Border = 0 };
           cells.Colspan = 4;
           cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);

           





            doc.Add(Details);
            doc.Close();
            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }

        }
        #endregion

        #region Rajasthan to Other State Transfer Document
        public ActionResult ElephantRajasthanToOtherStateNOCDoc(string ID)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            ElephantMovement obj = new ElephantMovement();
            try
            {
                string filepath = filepath = "~/PDFFolder/RajasthanToOtherStateNOC/ElephantRAJ_TO_OtherState_NOC_" + ID + ".pdf"; ;
                if (System.IO.File.Exists(Server.MapPath(filepath)))
                {
                    string FilePath = Server.MapPath(filepath);
                    WebClient User = new WebClient();
                    Byte[] FileBuffer = User.DownloadData(FilePath);
                    if (FileBuffer != null)
                    {
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-length", FileBuffer.Length.ToString());
                        Response.BinaryWrite(FileBuffer);
                        Response.End();
                    }
                }
                else
                {
                    DataSet dtElephant = obj.TPElephant(ID);
                    if (dtElephant != null)
                    {
                        RajasthanToOtherStateNOC(dtElephant);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }

            return RedirectToAction("ElephantReviewApprover");
        }

        public void RajasthanToOtherStateNOC(DataSet ds)
        {
            string filepath = string.Empty;

            filepath = "~/PDFFolder/RajasthanToOtherStateNOC/ElephantRAJ_TO_OtherState_NOC_" + Convert.ToString(ds.Tables[0].Rows[0]["RequestID"]) + ".pdf";

            #region Create PDF
            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            var subheadfont = FontFactory.GetFont("Times New Roman", 10, FontColour);

            var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
            var myfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);
            //boldFont.SetStyle(FontFactory.H);


            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            /////create Table
            // PdfPTable tablehead;
            // tablehead = new PdfPTable(3);

            PdfPTable Details = new PdfPTable(4) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 140;
            Details.SetTotalWidth(new float[] { 35f, 35f, 35f, 35f });


            #region First Row Right Side
            cells = new PdfPCell(new Phrase("NO: WLF/26/B/19/2017-18", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Print Chief Conservator of forest", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Aranya Bhawan Ch-3 circle", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Opp. St.Zaveers School", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Sector 10- A. Gandhinager", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Date: " + Convert.ToString(ds.Tables[0].Rows[0]["PublishDate"]) + "", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            #endregion

            #region Secound Row Left Side
            cells = new PdfPCell(new Phrase("To,", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Chief Wildlife Warden,", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Government of " + Convert.ToString(ds.Tables[0].Rows[0]["OtherStateName"]) + ",", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            //cells = new PdfPCell(new Phrase("Jaipur", subheadfont)) { Border = 0 };
            //cells.Colspan = 4;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            //Details.AddCell(cells);
            #endregion

            #region Thrid Row Left Side
            cells = new PdfPCell(new Phrase(" ", myfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("        Sub: No Objection Certificate for transfer of captive elephant from " + Convert.ToString(ds.Tables[0].Rows[0]["MovementFrom"]) + " to " + Convert.ToString(ds.Tables[0].Rows[0]["MovementTo"]) + ".", myfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("        Ref: Your letter " + Convert.ToString(ds.Tables[0].Rows[0]["LatterNumber"]) + ".", myfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("        With reference to the above mentioned subject and letter quoted at above for issue of __ for transport of one elephant owned by " + Convert.ToString(ds.Tables[0].Rows[0]["Name"]) + " " + Convert.ToString(ds.Tables[0].Rows[0]["Address"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[0]["MovementFrom"]) + " to " + Convert.ToString(ds.Tables[0].Rows[0]["MovementTo"]) + " for religious activity. ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("The details are as under:-", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            #endregion

            #region Fourth Row Table Header
            cells = new PdfPCell(new Phrase("Sr.No", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Name Of Elephant", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Sex", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Chip Set", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            #endregion

            #region Fourth Row Table Body
            cells = new PdfPCell(new Phrase("1", myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(ds.Tables[0].Rows[0]["ElephantName"]), myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(ds.Tables[0].Rows[0]["Gender"]), myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(Convert.ToString(ds.Tables[0].Rows[0]["MicroChipNumber"]), myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            #endregion

            #region Fifth Row Left Side

            cells = new PdfPCell(new Phrase("  There is no objection if permission for transport of elephant (with " + Convert.ToString(ds.Tables[0].Rows[0]["OtherStateName"]) + ") _____ from " + Convert.ToString(ds.Tables[0].Rows[0]["MovementFrom"]) + ". to " + Convert.ToString(ds.Tables[0].Rows[0]["MovementTo"]) + ". Subject with the following condition.) ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            #endregion

            #region sixth Row Center Side ponits


            cells = new PdfPCell(new Phrase("     1.The permission is valid initially for a period of " + Convert.ToString(ds.Tables[0].Rows[0]["ReturnDuration"]) + " futher extension by this office after site verification by DCF._____", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("     2.The permission in any manner cannot be construct as trensfer of ____ " + Convert.ToString(ds.Tables[0].Rows[0]["Name"]) + " the owner of this elephant should _______ during this transport and should also be allowed to stay with this animal for day ___ needs of this elephant during the period of permission.", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("     3.Food for the animals is to be arranged from their own resources.____________ feeding the elephant. fodder/branches/leaves ect should not be ________________ standing inside Government / Panchayat owned areas.", subheadfont)) { Border = 0 };
            cells.Colspan = 4;
            Details.AddCell(cells);

            #endregion


            doc.Add(Details);
            doc.Close();
            #endregion

            if (System.IO.File.Exists(Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }

        }
        #endregion

        public void RajasthanToOtherStateCWW(DataSet ds)
        {
            string filepath = string.Empty;

            filepath = "~/PDFFolder/RajasthanToOtherStateCWW/ElephantRAJ_TO_OtherState_CWW_" + DateTime.Now.Ticks.ToString() + ".pdf";

            Document doc = new Document(PageSize.A4, 15, 15f, 15f, 15f);

            PdfWriter.GetInstance(doc, new FileStream(Server.MapPath(filepath), FileMode.Create));

            var FontColour = new BaseColor(0, 0, 0);
            Paragraph tableheading = null;
            Paragraph sideheading = null;
            Phrase colHeading;

            PdfPCell cell;
            PdfPTable pdfTable = null;

            var subheadfont = FontFactory.GetFont("Times New Roman", 10, FontColour);

            var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 12);
            var myfont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 10);
            //boldFont.SetStyle(FontFactory.H);


            doc.Open();
            doc.NewPage();

            // doc.Add(new Paragraph(Environment.NewLine));


            /////create Table
            // PdfPTable tablehead;
            // tablehead = new PdfPTable(3);

            PdfPTable Details = new PdfPTable(5) { WidthPercentage = 100, HorizontalAlignment = Element.ALIGN_LEFT };
            PdfPCell cells = new PdfPCell() { Border = 4 };
            Details.TotalWidth = 150;
            Details.SetTotalWidth(new float[] { 30f, 30f, 30f, 30f, 30f });


            #region First Row Center Heading
            cells = new PdfPCell(new Phrase("test test test test test test test test test test test test", boldFont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_CENTER;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("test test test test test test test", myfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            cells.Colspan = 2;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("__________", myfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Date:- ", myfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("__________", myfont)) { Border = 0 };
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            #endregion

            #region Secound Row Left Side
            cells = new PdfPCell(new Phrase("To,", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("       Chief Wildlife Warden,", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("       Government of Rajasthan,", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            #endregion

            #region Thrid Row Left Side

            cells = new PdfPCell(new Phrase("        Sub:- No Objection Certificate for transfer of captive elephant from jaipur Rajasthan to ValSad Umargam (Gujrat).", myfont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Dear ,", myfont)) { Border = 0 };
            cells.Colspan = 5;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("      Subject:-  With reference to the above mentioned subject and letter quoted at above for issue of __ for transport of one elephant owned by MR.Shokhat ALi S/o Shri Ikaram Ali House No.______ . Jaipur Rajasthan to _________________ for religious activity. ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            Details.AddCell(cells);
            #endregion

            #region Fourth Row Table Header
            cells = new PdfPCell(new Phrase("Sr.No", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Name Of Elephant", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Details", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Place", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Duration", myfont));
            cells.BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211);
            Details.AddCell(cells);

            #endregion

            #region Fourth Row Table Body
            cells = new PdfPCell(new Phrase("1", myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Myna", myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("Female", myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("007", myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase("1 Year", myfont));
            Details.AddCell(cells);

            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            Details.AddCell(cells);
            #endregion

            #region Fifth Row Left Side

            cells = new PdfPCell(new Phrase("  There is no objection if permission for transport of elephant (with gujrat state _____ from _______. ________ to ________. ______ Subject with the following condition.) ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            //cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase(" ", subheadfont)) { Border = 0 };
            cells.Colspan = 5;
            Details.AddCell(cells);
            #endregion

            #region sixth Row Center Side ponits


            cells = new PdfPCell(new Phrase("test:-", subheadfont)) { Border = 0 };
            cells.Colspan = 2;
            cells.HorizontalAlignment = Element.ALIGN_LEFT;
            Details.AddCell(cells);
            cells = new PdfPCell(new Phrase("Signature", subheadfont)) { Border = 0 };
            cells.Colspan = 3;
            cells.HorizontalAlignment = Element.ALIGN_RIGHT;
            Details.AddCell(cells);


            #endregion


            doc.Add(Details);
            doc.Close();
            if (System.IO.File.Exists(Server.MapPath(filepath)))
            {
                string FilePath = Server.MapPath(filepath);
                WebClient User = new WebClient();
                Byte[] FileBuffer = User.DownloadData(FilePath);
                if (FileBuffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", FileBuffer.Length.ToString());
                    Response.BinaryWrite(FileBuffer);
                    Response.End();
                }
            }

        }

    }
}
