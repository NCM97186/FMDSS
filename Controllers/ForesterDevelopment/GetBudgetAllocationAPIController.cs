using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using FMDSS.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc; 

namespace FMDSS.Controllers.ForesterDevelopment
{
    public class GetBudgetAllocationAPIController : ApiController
    {
        FmdssContext dbContext=new FmdssContext();
        private long status = 0;
        private string response = "";
        // GET api/<controller>
        [System.Web.Http.HttpGet]
        public List<View_BudgetAllocation_Circle> GetBudgetAllocationCircleList(int UserID)
        {
            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = new List<View_BudgetAllocation_Circle>();
            try
            {
                Repository<View_BudgetAllocation_Circle> repo = new Repository<View_BudgetAllocation_Circle>();
                var param1 = new SqlParameter("@Option", "CIRCLE");
                var param2 = new SqlParameter("@UserId", UserID);
                var result = repo.GetWithStoredProcedure("BA_getBudgetAllocationList @Option,@UserId", param1, param2).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                #endregion
                return lstBudgetAllocationCircle;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [System.Web.Http.HttpGet]
        public List<View_BudgetAllocation_Circle> GetBudgetAllocationBudgetID(int UserID, int ID)
        {
            List<View_BudgetAllocation_Circle> lstBudgetAllocationCircle = new List<View_BudgetAllocation_Circle>();
            try
            {
                Repository<View_BudgetAllocation_Circle> repo = new Repository<View_BudgetAllocation_Circle>();
                var param1 = new SqlParameter("@Option", "CIRCLE");
                var param2 = new SqlParameter("@UserId", UserID);
                var param3 = new SqlParameter("@ID", ID);
                var result = repo.GetWithStoredProcedure("BA_getBudgetAllocationListBudgetID @Option,@UserId,@ID", param1, param2, param3).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<View_BudgetAllocation_Circle>>(str);
                #endregion
                return lstBudgetAllocationCircle;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [System.Web.Http.HttpGet]
        public List<BugdetExpenditure_RemainingMonthsOnID> GetBudgetAllocationBudgetID_Month(int ID)
        {
            List<BugdetExpenditure_RemainingMonthsOnID> lst = new List<BugdetExpenditure_RemainingMonthsOnID>();
            try
            {
                Repository<BugdetExpenditure_RemainingMonthsOnID> repo = new Repository<BugdetExpenditure_RemainingMonthsOnID>();
                var param1 = new SqlParameter("@ID", ID);
                var result = repo.GetWithStoredProcedure("BugdetExpenditure_RemainingMonthsOnID @ID", param1).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BugdetExpenditure_RemainingMonthsOnID>>(str);
                #endregion
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage BudgetExpenditureAllocationSave(List<View_Budget_Expenditure> BudgetExpenditureList)
        {
            int EditAad = 0;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            tbl_Budget_Expenditure tblBudgExpUpdate = new tbl_Budget_Expenditure();
            tbl_Budget_Expenditure tblBudgExpUpdate1 = new tbl_Budget_Expenditure();
            try
            {
                try
                {
                    foreach (var items in BudgetExpenditureList)
                    {
                        var id = Convert.ToInt64(items.BudgetHeadAllocationID);
                        tblBudgExpUpdate = dbContext.tbl_Budget_Expenditure.FirstOrDefault(i=>i.BudgetAllocation_CircleID==id && i.ExpenditureMonths==items.ExpenditureMonths);
                        if (tblBudgExpUpdate != null)
                        {
                            EditAad = 1;
                            tblBudgExpUpdate.FY_ID =items.FY_ID;
                            tblBudgExpUpdate.SchemeID = items.SchemeID;
                            tblBudgExpUpdate.ActivityID = items.ActivityID;
                            tblBudgExpUpdate.SubActivityID = items.SubActivityID;
                            tblBudgExpUpdate.BudgetHeadID = items.BudgetHeadID;
                            tblBudgExpUpdate.SubBudgetHeadID = items.SubBudgetHeadID;
                            tblBudgExpUpdate.AllocatedAmount = items.AllocatedAmount;
                            tblBudgExpUpdate.ExpenditureTilldate = items.ExpenditureTilldate;
                            tblBudgExpUpdate.ExpenditureMonths = items.ExpenditureMonths;
                            tblBudgExpUpdate.CIRCLE_CODE = items.CIRCLE_CODE;
                            tblBudgExpUpdate.Division = items.Division;
                            tblBudgExpUpdate.EnteredOn = DateTime.Now;
                            tblBudgExpUpdate.EnteredBy = items.EnteredBy;
                            tblBudgExpUpdate.IsActive = true;
                            tblBudgExpUpdate.ISCircleDivision = items.ISCircleDivision;
                            tblBudgExpUpdate.SanctuaryCode = items.SanctuaryCode;
                            tblBudgExpUpdate.WorkProgressDetails = items.WorkProgressDetails;
                            tblBudgExpUpdate.SiteName = items.SiteName;
                            tblBudgExpUpdate.BudgetAllocation_CircleID = id;
                            tblBudgExpUpdate.SiteNameExpenditure = items.SiteNameExpenditure;
                            tblBudgExpUpdate.Remarks = items.Remarks;
                            tblBudgExpUpdate.IsCoreOrBuffer = items.IsCoreOrBuffer;
                            tblBudgExpUpdate.Mobile_Web = "M";
                            tblBudgExpUpdate.Latitude = items.Latitude;
                            tblBudgExpUpdate.Longitude = items.Longitude;
                            this.dbContext.tbl_Budget_Expenditure.Add(tblBudgExpUpdate);
                            dbContext.Entry(tblBudgExpUpdate).State = System.Data.Entity.EntityState.Modified;
                            var result_id = (from t in dbContext.tbl_Budget_Expenditure where t.EnteredBy == items.EnteredBy orderby t.EnteredOn descending select t.Id).First();
                            for (int i=0; i < items.Image.Count();i++)
                            {
                                if (!string.IsNullOrWhiteSpace(items.Image[i]) && !string.IsNullOrEmpty(items.Image[i]))

                                {
                                    //tblBudgExpUpdate.Image = items.Image;
                                    string fileName = id + "_BudgetExpenditure_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(items.Image[i]) + "." + GetFileExtensionFromBase64(items.Image[i]);
                                    string _FileName = HttpContext.Current.Server.MapPath("~/BudgetExpenditure/") + fileName;
                                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(items.Image[i]));
                                    string Images = "/BudgetExpenditure/" + fileName;
                                    tbl_BudgetExpenditure_Image Imageinsert = new tbl_BudgetExpenditure_Image();
                                    Imageinsert.BudgetAllocation_CircleID = id;
                                    Imageinsert.Image_Path = "../"+ Images;
                                    Imageinsert.Expenditure_Month=items.ExpenditureMonths;
                                    Imageinsert.tbl_BudgetExpenditure_ID = result_id;
                                    this.dbContext.tbl_BudgetExpenditure_Image.Add(Imageinsert);
                                    dbContext.Entry(Imageinsert).State = System.Data.Entity.EntityState.Added;
                                }
                                else
                                {
                                    items.Image[i] = string.Empty;
                                }
                            }
                            
                        }
                        else
                        {
                            EditAad = 2;
                            tblBudgExpUpdate1.FY_ID = items.FY_ID;
                            tblBudgExpUpdate1.SchemeID = items.SchemeID;
                            tblBudgExpUpdate1.ActivityID = items.ActivityID;
                            tblBudgExpUpdate1.SubActivityID = items.SubActivityID;
                            tblBudgExpUpdate1.BudgetHeadID = items.BudgetHeadID;
                            tblBudgExpUpdate1.SubBudgetHeadID = items.SubBudgetHeadID;
                            tblBudgExpUpdate1.AllocatedAmount = items.AllocatedAmount;
                            tblBudgExpUpdate1.ExpenditureTilldate = items.ExpenditureTilldate;
                            tblBudgExpUpdate1.ExpenditureMonths = items.ExpenditureMonths;
                            tblBudgExpUpdate1.CIRCLE_CODE = items.CIRCLE_CODE;
                            tblBudgExpUpdate1.Division = items.Division;
                            tblBudgExpUpdate1.EnteredOn = DateTime.Now;
                            tblBudgExpUpdate1.EnteredBy = items.EnteredBy;
                            tblBudgExpUpdate1.IsActive = true;
                            tblBudgExpUpdate1.ISCircleDivision = items.ISCircleDivision;
                            tblBudgExpUpdate1.SanctuaryCode = items.SanctuaryCode;
                            tblBudgExpUpdate1.WorkProgressDetails = items.WorkProgressDetails;
                            tblBudgExpUpdate1.SiteName = items.SiteName;
                            tblBudgExpUpdate1.BudgetAllocation_CircleID = id;
                            tblBudgExpUpdate1.SiteNameExpenditure = items.SiteNameExpenditure;
                            tblBudgExpUpdate1.Remarks = items.Remarks;
                            tblBudgExpUpdate1.IsCoreOrBuffer = items.IsCoreOrBuffer;
                            tblBudgExpUpdate1.Mobile_Web = "M";
                            tblBudgExpUpdate1.Latitude = items.Latitude;
                            tblBudgExpUpdate1.Longitude = items.Longitude;
                            this.dbContext.tbl_Budget_Expenditure.Add(tblBudgExpUpdate1);
                            dbContext.Entry(tblBudgExpUpdate1).State = System.Data.Entity.EntityState.Added;
                            var result_id = (from t in dbContext.tbl_Budget_Expenditure where t.EnteredBy == items.EnteredBy orderby t.EnteredOn descending select t.Id).First();
                            for (int i = 0; i < items.Image.Count(); i++)
                            {
                                if (!string.IsNullOrWhiteSpace(items.Image[i]) && !string.IsNullOrEmpty(items.Image[i]))

                                {
                                    //tblBudgExpUpdate.Image = items.Image;
                                    string fileName = id + "_BudgetExpenditure_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(items.Image[i]) + "." + GetFileExtensionFromBase64(items.Image[i]);
                                    string _FileName = HttpContext.Current.Server.MapPath("~/BudgetExpenditure/") + fileName;
                                    string Image = SaveByteArrayAsImage(_FileName, Convert.ToString(items.Image[i]));
                                    string Images = "/BudgetExpenditure/" + fileName;
                                    tbl_BudgetExpenditure_Image Imageinsert = new tbl_BudgetExpenditure_Image();
                                    Imageinsert.BudgetAllocation_CircleID = id;
                                    Imageinsert.Image_Path = "../" + Images;
                                    Imageinsert.Expenditure_Month = items.ExpenditureMonths;
                                    Imageinsert.tbl_BudgetExpenditure_ID = result_id;
                                    this.dbContext.tbl_BudgetExpenditure_Image.Add(Imageinsert);
                                    dbContext.Entry(Imageinsert).State = System.Data.Entity.EntityState.Added;
                                }
                                else
                                {
                                    items.Image[i] = string.Empty;
                                }
                            }
                        }
                    }
                    status = dbContext.SaveChanges();

                }
                catch (Exception)
                {
                    throw;
                }
                
            }
            catch (Exception ex)
            {

            }
            if (status == 0)
            {
                StatusCode = 0;
                Message = "Failed to Upload.";
            }
            else if (status >= 1 && EditAad == 1)
            {
                StatusCode = 1;
                Message = "Updated Successfully.";
            }
            else if (status >= 1 && EditAad == 2)
            {
                StatusCode = 1;
                Message = "Successfully Uploaded.";
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message });

        }
        
        public static string GetFileExtensionFromBase64(string base64String)
        {
            var data = base64String.Substring(0, 30);


            if (data.ToUpper() == "IVBOR" || data.ToUpper().Contains("IVBOR"))
                return "png";
            else if (data.ToUpper() == "/9J/4" || data.ToUpper().Contains("/9J/4"))
            {
                return "jpg";
            }

            else
            {
                return string.Empty;
            }
         
        }
        
        private string SaveByteArrayAsImage(string fullOutputPath, string base64String)
        {
            try
            {
                if (base64String.IndexOf(',') > 0)
                {
                    string[] arr = base64String.Split(',');
                    base64String = arr[1];
                }

                byte[] bytes = Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(fullOutputPath, bytes);
                return fullOutputPath;
            }
            catch (Exception ex)
            {

            }
            return fullOutputPath;
        }
    }
}