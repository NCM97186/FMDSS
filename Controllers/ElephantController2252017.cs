using FMDSS.Models;
using FMDSS.Models.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers
{
    public class ElephantController : Controller
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
                            RegNumber = Convert.ToInt64(dr["RegNumber"].ToString()),
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
        public ActionResult ADDUpdateElephantInfo(ElephantInfo oElephantInfo)
        {
            List<SelectListItem> ElephantInfo = new List<SelectListItem>();
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            Int64 UserID = Convert.ToInt64(Session["UserID"]);
            string status = null;
            try
            {

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

                ViewBag.OpType = (ElephantId == "0" ? "Add Elephant Information" : "Edit Elephant Information");


                DataTable dtf = obj.Select_ElephantInfo(Convert.ToInt32(ElephantId));

                foreach (DataRow dr in dtf.Rows)
                {
                    obj = new ElephantInfo
                    {
                        ElephantId = Convert.ToInt32(dr["ElephantId"].ToString()),
                        ElephantName = Convert.ToString(dr["ElephantName"].ToString()),
                        ssoid = Convert.ToString(dr["ssoid"].ToString()),
                        RegNumber = Convert.ToInt64(dr["RegNumber"].ToString()),
                        LicenceNumber = Convert.ToString(dr["LicenceNumber"].ToString()),
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

                        OperationType = "Edit Elephant Information"
                    };

                }

                lstISactive.Add(new SelectListItem { Text = "Active", Value = "1" });
                lstISactive.Add(new SelectListItem { Text = "Deactive ", Value = "0" });

                ViewBag.ISactivelst = lstISactive;
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
                }

                return RedirectToAction("ElephantIndex", new { ID = IDs });
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
                }

                return RedirectToAction("IssueNOCOtherStateMovement", new { ID = IDs });
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, actionName + "_" + controllerName, 0, DateTime.Now, UserID);

            }
            return null;

        }

        #endregion



        public void SendSMSEmailForSuccessTransaction(string ACTION, string RequestId)
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(RequestId, ACTION);
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                {
                    body = string.Empty;

                    #region SMS Email
                    string UserMailBody = UserMailSMSBody("Mail", ACTION,RequestId);
                    string subject = "Regarding Elephent Movement";
                   // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["EmailId"].ToString(), string.Empty);
                    objSMS_EMail_Services.sendEMail(subject, UserMailBody, "avi.bhatra87@gmail.com", string.Empty);
                    #endregion

                }

                if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                {
                    string UserSmsBody =  UserMailSMSBody("Mail", ACTION,RequestId);
                    SMS_EMail_Services.sendSingleSMS("9166046444", UserSmsBody);
                }

            }
            #endregion
        }

        public string UserMailSMSBody(string MailSMS, string type,string requestid)
        {

            StringBuilder sb = new StringBuilder();
            if (type == "CreateTP")
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
                    sb.Append("<p>Thanks for applying for Elephent Moment Your Transit Permit Number is <b>" + requestid + "</b></p>");
                    sb.Append("Please quote this ID for future communication.");
                }
                
            }
            else if (type == "TPApproveOrReject")
            {
                if (MailSMS == "Mail")
                {
                    sb.Append("Dear User,");
                    sb.Append("<br />");
                    sb.Append("<p>Thanks for applying for Elephent Moment Your Transit Permit Number is <b>" + requestid + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Final Status of your application is Approve .");
                    sb.Append("<br/>");
                    sb.Append("Get your application hard copy with in 7 days.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");

                }
                else if (MailSMS == "SMS")
                {
                    sb.Append("With the refrence of your Transit Permit Number " + requestid + " is Approve by Rajasthan CWLW.");
                }
            }
            else if (type == "CreateNOC")
            {
                if (MailSMS == "Mail")
                {
                    sb.Append("Dear User,");
                    sb.Append("<br />");
                    sb.Append("<p>Thanks for applying for Elephent Moment NOC Your NOC Number is <b>" + requestid + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Final Status of your application is Approve by Rajasthan CWLW.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");

                }
                else if (MailSMS == "SMS")
                {
                    sb.Append("With the refrence of Requested NOC number NOC_45657456 your application for Elephent Moment NOC is Approve by Rajasthan CWLW.");
                }
            }

            return sb.ToString();
        }


    }
}
