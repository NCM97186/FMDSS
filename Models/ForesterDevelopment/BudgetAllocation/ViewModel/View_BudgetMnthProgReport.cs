using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDSS.Repository;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.Text;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_BudgetMnthProgReport:BaseModelSerializable
    {
        Repository<View_BudgetMnthProgReport> repository;
        public View_BudgetMnthProgReport()
        {
            repository = new Repository<View_BudgetMnthProgReport>();
        }

        public string ReportType { get; set; }
        public int FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public string BudgetHead { get; set; }
        public long BudgetHeadId { get; set; }
        public string SubBudgetHead { get; set; }
        public long SubBudgetHeadId { get; set; }
        public string Activity_Name { get; set; }
        public long ActivityID { get; set; }
        public string SubActivity_Name { get; set; }
        public long SubActivityID { get; set; }
        public string Scheme_Name { get; set; }
        public string Scheme_Code { get; set; }
        public string Circle_Name { get; set; }
        public string Circle_Code { get; set; }
        public string ISCircleDivision { get; set; }
        public string Division_Name { get; set; }
        public string Division_Code { get; set; }

        public string Range_Name { get; set; }
        public string Range_Code { get; set; }
        public string Village_Name { get; set; }
        public string Village_Code { get; set; }
        public string HQ { get; set; }
        public decimal CumulativeExpenditure { get; set; }

        public decimal RemaningAmount { get; set; }
        public long AllocationId { get; set; }
        public string AllocatedLevel { get; set; }
        public decimal AllocatedAmount { get; set; }

        public decimal Expenditure { get; set; }
        public decimal TotalAllocationAmount { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string Option { get; set; }

        public long BudgetHeadAllocationID { get; set; }
        public List<View_BudgetMnthProgReport> GetMontlyProgressReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<View_BudgetMnthProgReport> lstDetails = new List<View_BudgetMnthProgReport>();
                object[] xparams ={
                             new SqlParameter("@FY_ID",obj.FinancialYearId),
                             new SqlParameter("@BudgetHeadId",obj.BudgetHeadId),
                             new SqlParameter("@SubBudgetHeadId",obj.SubBudgetHeadId),
                             new SqlParameter("@ActivityId",obj.ActivityID),
                             new SqlParameter("@SubActivityId",obj.SubActivityID),
                             new SqlParameter("@Circle_Code", obj.Circle_Code),
                             new SqlParameter("@Div_Code", obj.Division_Code),
                             new SqlParameter("@FromDate", (object)obj.FromDate??DBNull.Value),
                             new SqlParameter("@ToDate", (object)obj.ToDate??DBNull.Value),
                             new SqlParameter("@Option", "ALL")};
                var result = repository.GetWithStoredProcedure("dbo.Sp_GetMonthlyProgressReport @FY_ID,@BudgetHeadId,@SubBudgetHeadId,@ActivityId, @SubActivityId,@Circle_Code,@Div_Code,@FromDate,@ToDate,@Option", xparams).ToList();

                foreach (var item in result)
                {

                    lstDetails.Add(new View_BudgetMnthProgReport()
                    {
                        AllocationId = item.AllocationId,
                        BudgetHeadId = item.BudgetHeadId,
                        BudgetHead = item.BudgetHead,
                        SubBudgetHeadId = item.SubBudgetHeadId,
                        SubBudgetHead = item.SubBudgetHead,
                        ActivityID = item.ActivityID,
                        Activity_Name = item.Activity_Name,
                        SubActivityID = item.SubActivityID,
                        SubActivity_Name = item.SubActivity_Name,
                        HQ = item.HQ,
                        Circle_Name = item.Circle_Name,
                        Division_Name = item.Division_Name,
                        ISCircleDivision = item.ISCircleDivision,
                        FinancialYearId = item.FinancialYearId,
                        AllocatedAmount = item.AllocatedAmount,
                        TotalAllocationAmount = item.TotalAllocationAmount,
                        CumulativeExpenditure = item.CumulativeExpenditure
                    });
                }
                return lstDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    [Serializable]
    public class MonthlyProgressReport:BaseModelSerializable
    {

        Repository<MonthlyProgressReport> repository;
        public MonthlyProgressReport()
        {
            repository = new Repository<MonthlyProgressReport>();
        }

        public string Year { get; set; }
        public Nullable<decimal> January { get; set; }
        public Nullable<decimal> February { get; set; }
        public Nullable<decimal> March { get; set; }
        public Nullable<decimal> April { get; set; }
        public Nullable<decimal> May { get; set; }
        public Nullable<decimal> June { get; set; }
        public Nullable<decimal> July { get; set; }
        public Nullable<decimal> August { get; set; }
        public Nullable<decimal> September { get; set; }
        public Nullable<decimal> October { get; set; }
        public Nullable<decimal> November { get; set; }
        public Nullable<decimal> December { get; set; }

        public List<MonthlyProgressReport> GetMontlyProgressReport(View_BudgetMnthProgReport obj)
        {
            try
            {
                List<MonthlyProgressReport> lstDetails = new List<MonthlyProgressReport>();
                object[] xparams ={
                             new SqlParameter("@FY_ID",obj.FinancialYearId),
                             new SqlParameter("@BudgetHeadId",obj.BudgetHeadId),
                             new SqlParameter("@SubBudgetHeadId",obj.SubBudgetHeadId),
                             new SqlParameter("@ActivityId",obj.ActivityID),
                             new SqlParameter("@SubActivityId",obj.SubActivityID),
                             new SqlParameter("@Circle_Code", obj.Circle_Code),
                             new SqlParameter("@Div_Code", obj.Division_Code),
                             new SqlParameter("@FromDate", (object)obj.FromDate??DBNull.Value),
                             new SqlParameter("@ToDate", (object)obj.ToDate??DBNull.Value),
                             new SqlParameter("@Option", "MONTHLY")};
                var result = repository.GetWithStoredProcedure("dbo.Sp_GetMonthlyProgressReport @FY_ID,@BudgetHeadId,@SubBudgetHeadId,@ActivityId, @SubActivityId,@Circle_Code,@Div_Code,@FromDate,@ToDate,@Option", xparams).ToList();

                foreach (var item in result)
                {

                    lstDetails.Add(new MonthlyProgressReport()
                    {
                        Year = item.Year,
                        January = item.January,
                        February = item.February,
                        March = item.March,
                        April = item.April,
                        May = item.May,
                        June = item.June,
                        July = item.July,
                        August = item.August,
                        September = item.September,
                        October = item.October,
                        November = item.November,
                        December = item.December
                    });
                }
                return lstDetails;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    #region MonthlyProgressReport Developed By Rajveer
    
    [Serializable]
    public class MonthlyProgressReportModel:BaseModelSerializable
    {
        public int FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public string BudgetHead { get; set; }
        public long BudgetHeadId { get; set; }
        public string SubBudgetHead { get; set; }
        public long SubBudgetHeadId { get; set; }
        public string Activity_Name { get; set; }
        public long ActivityID { get; set; }
        public string SubActivity_Name { get; set; }
        public long SubActivityID { get; set; }
        public string Scheme_Name { get; set; }
        public long SchemeID { get; set; }
        public string Scheme_Code { get; set; }
        public string Circle_Name { get; set; }
        public string Circle_Code { get; set; }
        public string Division_Name { get; set; }
        public string Division_Code { get; set; }
        public decimal CumulativeExpenditure { get; set; }
        public decimal RemaningAmount { get; set; }
        public long AllocationId { get; set; }
        public decimal AllocatedAmount { get; set; }
        public string ISCircleDivision { get; set; }
        public decimal Expenditure { get; set; }
        public string Option { get; set; }

        public decimal ExpenditurePercentage { get; set; }
        public string IsRecurringName { get; set; }

        public decimal TotalAmount { get; set; }

        public string SantuaryCode { get; set; }

        public string SantuaryName { get; set; }
        public long BudgetHeadAllocationID { get; set; }

        public MonthlyStatusModel MonthlyReport { get; set; }

        public decimal Expenditureduringthemonth { get; set; }
        public decimal ExpenditureLastmonth { get; set; }

        public string GISID { get; set; }
        public string GISFilePath { get; set; }

        public string IsWildlifeOrForest { get; set; }
        public string SiteName { get; set; }
        public string Unit { get; set; }
        public decimal NumberPerUnit { get; set; }

        public string WorkProgressDetails { get; set; }

        public string SelectColumnName { get; set; }

        public string IsCoreOrBuffer { get; set; }
    }

    public class MonthlyProgressReportExpenditureModel
    {
        public MonthlyProgressReportExpenditureModel()
        {
            model = new MonthlyProgressReportModel();
            List = new List<MonthlyProgressReportModel>();
        }
        public MonthlyProgressReportModel model { get; set; }
        public List<MonthlyProgressReportModel> List { get; set; }
    }

    [Serializable]
    public class MonthlyStatusModel:BaseModelSerializable
    {
        public string Year { get; set; }
        public Nullable<decimal> January { get; set; }
        public Nullable<decimal> February { get; set; }
        public Nullable<decimal> March { get; set; }
        public Nullable<decimal> April { get; set; }
        public Nullable<decimal> May { get; set; }
        public Nullable<decimal> June { get; set; }
        public Nullable<decimal> July { get; set; }
        public Nullable<decimal> August { get; set; }
        public Nullable<decimal> September { get; set; }
        public Nullable<decimal> October { get; set; }
        public Nullable<decimal> November { get; set; }
        public Nullable<decimal> December { get; set; }
    }
    public class MonthlyProgressReportExpenditure
    {
        Repository<MonthlyProgressReportModel> repository;
        Repository<MonthlyStatusModel> Monthlyrepository;
        public MonthlyProgressReportExpenditure()
        {
            repository = new Repository<MonthlyProgressReportModel>();
            Monthlyrepository = new Repository<MonthlyStatusModel>();
        }
        public List<MonthlyProgressReportModel> GetMontlyProgressReport(MonthlyProgressReportModel obj, string Option)
        {
            List<MonthlyProgressReportModel> lstDetails = new List<MonthlyProgressReportModel>();

            try
            {
                if (obj.SantuaryCode == "0")
                {
                    obj.SantuaryCode = string.Empty;
                }

                object[] xparams ={
                             new SqlParameter("@FYID",!string.IsNullOrEmpty(obj.FinancialYear)?Convert.ToString(obj.FinancialYear):string.Empty),
                             new SqlParameter("@SchemeIds",!string.IsNullOrEmpty(obj.Scheme_Name)?Convert.ToString(obj.Scheme_Name):string.Empty),
                             new SqlParameter("@BudgetHeadIds",!string.IsNullOrEmpty(obj.BudgetHead)?Convert.ToString(obj.BudgetHead):string.Empty),
                             new SqlParameter("@SubBudgetHeadIds",!string.IsNullOrEmpty(obj.SubBudgetHead)?Convert.ToString(obj.SubBudgetHead):string.Empty),
                             new SqlParameter("@ActivityIds",!string.IsNullOrEmpty(obj.Activity_Name)?Convert.ToString(obj.Activity_Name):string.Empty),
                             new SqlParameter("@SubActivityIds",!string.IsNullOrEmpty(obj.SubActivity_Name)?Convert.ToString(obj.SubActivity_Name):string.Empty),
                             new SqlParameter("@Circle_Codes",!string.IsNullOrEmpty(obj.Circle_Code)?Convert.ToString(obj.Circle_Code):string.Empty),
                             new SqlParameter("@Div_Codes",!string.IsNullOrEmpty(obj.Division_Code)?Convert.ToString(obj.Division_Code):string.Empty),
                              new SqlParameter("@SantuaryCode",!string.IsNullOrEmpty(obj.SantuaryCode)?Convert.ToString(obj.SantuaryCode):string.Empty),
                             new SqlParameter("@IsCircleOrDivision",!string.IsNullOrEmpty(obj.ISCircleDivision)?Convert.ToString(obj.ISCircleDivision):string.Empty),
                             new SqlParameter("@IsRecurringOrNonRecurring",!string.IsNullOrEmpty(obj.IsRecurringName)?Convert.ToString(obj.IsRecurringName):string.Empty),
                             new SqlParameter("@SiteName",!string.IsNullOrEmpty(obj.SiteName)?Convert.ToString(obj.SiteName):string.Empty),
                              new SqlParameter("@IsCoreOrBuffer",!string.IsNullOrEmpty(obj.IsCoreOrBuffer)?Convert.ToString(obj.IsCoreOrBuffer):string.Empty),
                             new SqlParameter("@UserID",Convert.ToInt32(HttpContext.Current.Session["UserId"])),
                             new SqlParameter("@Option",Option)};

                //new SqlParameter("@Option", "ALL")};

                // var result = repository.GetWithStoredProcedure("SP_BudgetMnthProgReport @Option", xparams).ToList();

                if (Option.Contains("MONTHLY"))
                {
                    var result = Monthlyrepository.GetWithStoredProcedure("dbo.SP_BudgetMnthProgReport @FYID,@SchemeIds,@BudgetHeadIds,@SubBudgetHeadIds,@ActivityIds,@SubActivityIds,@Circle_Codes,@Div_Codes,@SantuaryCode,@IsCircleOrDivision,@IsRecurringOrNonRecurring,@SiteName,@IsCoreOrBuffer,@UserID,@Option", xparams).ToList();
                    #region Serialzed Object
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    MonthlyProgressReportModel model = new MonthlyProgressReportModel();
                    model.MonthlyReport = new MonthlyStatusModel();
                    List<MonthlyStatusModel> List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MonthlyStatusModel>>(str);
                    if (List.Count > 0)
                    {
                        model.MonthlyReport = List.FirstOrDefault();
                    }
                    else
                    {
                        model.MonthlyReport = new MonthlyStatusModel();
                    }
                    lstDetails.Add(model);
                    #endregion
                }
                else
                {
                    var result = repository.GetWithStoredProcedure("dbo.SP_BudgetMnthProgReport @FYID,@SchemeIds,@BudgetHeadIds,@SubBudgetHeadIds,@ActivityIds,@SubActivityIds,@Circle_Codes,@Div_Codes,@SantuaryCode,@IsCircleOrDivision,@IsRecurringOrNonRecurring,@SiteName,@IsCoreOrBuffer,@UserID,@Option", xparams).ToList();
                    #region Serialzed Object
                    string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    lstDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MonthlyProgressReportModel>>(str);
                    #endregion
                }



            }
            catch (Exception ex)
            {

                throw;
            }
            return lstDetails;
        }



    }
    #endregion

    public class MonthlyProgressAnalystReport : DAL
    {
        public string GetMontlyProgressAnalystReport(MonthlyProgressReportModel obj, string Option)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                if (obj.SantuaryCode == "0")
                {
                    obj.SantuaryCode = string.Empty;
                }
                if (string.IsNullOrEmpty(obj.SelectColumnName))
                {
                    obj.SelectColumnName = "";
                }

                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SPBudgetMnthProgAnaylistReport", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@FYID", !string.IsNullOrEmpty(obj.FinancialYear) ? Convert.ToString(obj.FinancialYear) : string.Empty);
                cmd.Parameters.AddWithValue("@SchemeIds", !string.IsNullOrEmpty(obj.Scheme_Name) ? Convert.ToString(obj.Scheme_Name) : string.Empty);
                cmd.Parameters.AddWithValue("@BudgetHeadIds", !string.IsNullOrEmpty(obj.BudgetHead) ? Convert.ToString(obj.BudgetHead) : string.Empty);
                cmd.Parameters.AddWithValue("@SubBudgetHeadIds", !string.IsNullOrEmpty(obj.SubBudgetHead) ? Convert.ToString(obj.SubBudgetHead) : string.Empty);
                cmd.Parameters.AddWithValue("@ActivityIds", !string.IsNullOrEmpty(obj.Activity_Name) ? Convert.ToString(obj.Activity_Name) : string.Empty);
                cmd.Parameters.AddWithValue("@SubActivityIds", !string.IsNullOrEmpty(obj.SubActivity_Name) ? Convert.ToString(obj.SubActivity_Name) : string.Empty);
                cmd.Parameters.AddWithValue("@Circle_Codes", !string.IsNullOrEmpty(obj.Circle_Code) ? Convert.ToString(obj.Circle_Code) : string.Empty);
                cmd.Parameters.AddWithValue("@Div_Codes", !string.IsNullOrEmpty(obj.Division_Code) ? Convert.ToString(obj.Division_Code) : string.Empty);
                cmd.Parameters.AddWithValue("@SantuaryCode", !string.IsNullOrEmpty(obj.SantuaryCode) ? Convert.ToString(obj.SantuaryCode) : string.Empty);
                cmd.Parameters.AddWithValue("@IsCircleOrDivision", !string.IsNullOrEmpty(obj.ISCircleDivision) ? Convert.ToString(obj.ISCircleDivision) : string.Empty);
                cmd.Parameters.AddWithValue("@IsRecurringOrNonRecurring", !string.IsNullOrEmpty(obj.IsRecurringName) ? Convert.ToString(obj.IsRecurringName) : string.Empty);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Option", Option);
                //cmd.Parameters.AddWithValue("@cols", "Sch.Scheme_Name, S.SubBudgetHead,  Cir.IsRecurring, Act.Activity_Name,  Cir.SiteName, Cir.Unit, Cir.NumberPerUnit");
                cmd.Parameters.AddWithValue("@cols", obj.SelectColumnName);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                if (dt != null && dt.Tables.Count > 0)
                {
                    #region Create Column
                    List<string> ColumnList = new List<string>();
                    stringBuilder.Append("<thead><tr>");
                    for (int j = 0; j < dt.Tables[0].Columns.Count; j++)
                    {
                        stringBuilder.Append("<th>" + dt.Tables[0].Columns[j].ColumnName.ToString() + "<input type='checkbox' onclick='fnShowHide(" + j + ")' /></th>");
                        ColumnList.Add(dt.Tables[0].Columns[j].ColumnName.ToString());
                    }
                    stringBuilder.Append("</tr></thead>");
                    #endregion

                    #region Create Row
                    stringBuilder.Append("<tbody>");
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        stringBuilder.Append("<tr>");
                        foreach (string col in ColumnList)
                        {
                            string RowValue = Convert.ToString(dt.Tables[0].Rows[i][col]);
                            if (col.ToLower().Trim() == "totalamount")
                            {
                                stringBuilder.Append("<td class='TAmount'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "allocatedamount")
                            {
                                stringBuilder.Append("<td class='AAmount'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "expenditure_till_last_month")
                            {
                                stringBuilder.Append("<td class='EAmount EAmountEachTotal_" + i + "'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "expenditure_during_the_month")
                            {
                                stringBuilder.Append("<td class='CAmount CAmountEachTotal_" + i + "'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "expenditure_till_date")
                            {
                                stringBuilder.Append("<td class='RAmount RAmountEachTotal_" + i + "'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "remainingamount")
                            {
                                stringBuilder.Append("<td class='RemainingAmount RemainingAmountEachTotal_" + i + "'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "expendituretilldate")
                            {
                                stringBuilder.Append("<td class='ExpenditureTilldate ExpenditureTilldateEachTotal_" + i + "'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else if (col.ToLower().Trim() == "expendituretilllastmonth")
                            {
                                stringBuilder.Append("<td class='ExpenditureLastmonth ExpenditureLastmonthEachTotal_" + i + "'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }
                            else
                            {
                                stringBuilder.Append("<td>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }

                        }
                        stringBuilder.Append("</tr>");

                    }

                    stringBuilder.Append("</tbody>");
                    #endregion

                    #region Create Footer
                    stringBuilder.Append("<tfoot><tr>");
                    foreach (string col in ColumnList)
                    {
                        if (col.ToLower().Trim() == "totalamount")
                        {
                            stringBuilder.Append("<td id='TotalAmount' class='Bold'></td>");
                        }
                        else if (col.ToLower().Trim() == "allocatedamount")
                        {
                            stringBuilder.Append("<td id='AllocatedAmount' class='Bold'></td>");
                        }
                        else if (col.ToLower().Trim() == "expenditure_till_last_month")
                        {
                            stringBuilder.Append("<td id='ExpenditureAmount' class='Bold'></td>");
                        }
                        else if (col.ToLower().Trim() == "expenditure_during_the_month")
                        {
                            stringBuilder.Append("<td id='CurrentAmount' class='Bold'></td>");
                        }
                        else if (col.ToLower().Trim() == "expenditure_till_date")
                        {
                            stringBuilder.Append("<td id='RemaningAmount' class='Bold'></td>");
                        }

                        else if (col.ToLower().Trim() == "remainingamount")
                        {
                            stringBuilder.Append("<td id='RemainingAmountTotal' class='Bold'></td>");
                        }
                        else if (col.ToLower().Trim() == "expendituretilldate")
                        {
                            stringBuilder.Append("<td id='ExpenditureTilldateTotal' class='Bold'></td>");
                        }
                        else if (col.ToLower().Trim() == "expendituretilllastmonth")
                        {
                            stringBuilder.Append("<td id='ExpenditureLastmonthTotal' class='Bold'></td>");
                        }
                        else
                        {
                            stringBuilder.Append("<td></td>");
                        }
                    }
                    stringBuilder.Append("</tr></tfoot>");
                    #endregion
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return stringBuilder.ToString();
        }

        public string GetMontlyProgressSummaryeport(MonthlyProgressReportModel obj, string Option, ref DataSet ReportList)
        {
            StringBuilder stringBuilder = new StringBuilder();

            try
            {
                if (obj.SantuaryCode == "0")
                {
                    obj.SantuaryCode = string.Empty;
                }
                if (string.IsNullOrEmpty(obj.SelectColumnName))
                {
                    obj.SelectColumnName = "";
                }

                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_BudgetMnthProgReportWithSummaryWise", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@FYID", !string.IsNullOrEmpty(obj.FinancialYear) ? Convert.ToString(obj.FinancialYear) : string.Empty);
                cmd.Parameters.AddWithValue("@SchemeIds", !string.IsNullOrEmpty(obj.Scheme_Name) ? Convert.ToString(obj.Scheme_Name) : string.Empty);
                cmd.Parameters.AddWithValue("@BudgetHeadIds", !string.IsNullOrEmpty(obj.BudgetHead) ? Convert.ToString(obj.BudgetHead) : string.Empty);
                cmd.Parameters.AddWithValue("@SubBudgetHeadIds", !string.IsNullOrEmpty(obj.SubBudgetHead) ? Convert.ToString(obj.SubBudgetHead) : string.Empty);
                cmd.Parameters.AddWithValue("@ActivityIds", !string.IsNullOrEmpty(obj.Activity_Name) ? Convert.ToString(obj.Activity_Name) : string.Empty);
                cmd.Parameters.AddWithValue("@SubActivityIds", !string.IsNullOrEmpty(obj.SubActivity_Name) ? Convert.ToString(obj.SubActivity_Name) : string.Empty);
                cmd.Parameters.AddWithValue("@Circle_Codes", !string.IsNullOrEmpty(obj.Circle_Code) ? Convert.ToString(obj.Circle_Code) : string.Empty);
                cmd.Parameters.AddWithValue("@Div_Codes", !string.IsNullOrEmpty(obj.Division_Code) ? Convert.ToString(obj.Division_Code) : string.Empty);
                cmd.Parameters.AddWithValue("@SantuaryCode", !string.IsNullOrEmpty(obj.SantuaryCode) ? Convert.ToString(obj.SantuaryCode) : string.Empty);
                cmd.Parameters.AddWithValue("@IsCircleOrDivision", !string.IsNullOrEmpty(obj.ISCircleDivision) ? Convert.ToString(obj.ISCircleDivision) : string.Empty);
                cmd.Parameters.AddWithValue("@IsRecurringOrNonRecurring", !string.IsNullOrEmpty(obj.IsRecurringName) ? Convert.ToString(obj.IsRecurringName) : string.Empty);
                cmd.Parameters.AddWithValue("@IsCoreOrBuffer", !string.IsNullOrEmpty(obj.IsCoreOrBuffer) ? Convert.ToString(obj.IsCoreOrBuffer) : string.Empty);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                cmd.Parameters.AddWithValue("@Option", Option);
                //cmd.Parameters.AddWithValue("@cols", "Sch.Scheme_Name, S.SubBudgetHead,  Cir.IsRecurring, Act.Activity_Name,  Cir.SiteName, Cir.Unit, Cir.NumberPerUnit");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                ReportList = dt;
                if (dt != null && dt.Tables.Count > 0)
                {
                    #region Avoid IDs 
                    string TagString = "schemeid,activityid,subactivityid,budgetheadid,subbudgetheadid,division_code,circle_code,sanctuarycode,iscircledivision";
                    List<String> AvoidColumnNameArray = new List<string>(TagString.ToLower().Split(',')).ToList();


                    #endregion

                    #region Create Column
                    List<string> ColumnList = new List<string>();
                    stringBuilder.Append("<thead><tr>");
                    for (int j = 0; j < dt.Tables[0].Columns.Count; j++)
                    {
                        int count = AvoidColumnNameArray.Where(s => s == dt.Tables[0].Columns[j].ColumnName.ToString().Trim().ToLower()).Count();
                        if (dt.Tables[0].Columns[j].ColumnName.ToString().Trim().ToLower() != "flag" && count == 0)
                        {
                            stringBuilder.Append("<th>" + dt.Tables[0].Columns[j].ColumnName.ToString() + "</th>");
                        }
                        ColumnList.Add(dt.Tables[0].Columns[j].ColumnName.ToString());
                    }
                    stringBuilder.Append("</tr></thead>");
                    #endregion

                    #region Create Row
                    stringBuilder.Append("<tbody>");
                    int IsRowAndGrandTotal = 1;
                    for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                    {
                        stringBuilder.Append("<tr>");
                        IsRowAndGrandTotal = 1;
                        foreach (string col in ColumnList)
                        {
                            string RowValue = Convert.ToString(dt.Tables[0].Rows[i][col]);
                            if (col.ToLower().Trim() == "flag" && RowValue.Trim().ToLower() == "0")
                            {
                                IsRowAndGrandTotal = 0;
                            }
                            int count = AvoidColumnNameArray.Where(s => s == col.ToString().Trim().ToLower()).Count();
                            if (IsRowAndGrandTotal == 0 && count == 0)
                            {
                                if (col.ToLower().Trim() == "unit" && RowValue.Trim().ToLower() == "total")
                                {
                                    stringBuilder.Append("<td class='totalamt" + i + " counttotal redBold'>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                                }
                                else if (col.ToLower().Trim() == "flag")
                                {
                                }
                                else if (col.ToLower().Trim() == "budgethead" && RowValue.Trim().ToLower() == "zzzz")
                                {
                                    stringBuilder.Append("<td class='totalamt" + i + " redBold'>  </td>");
                                }
                                else if (col.ToLower().Trim() == "totalamount")
                                {
                                    stringBuilder.Append("<td class='counttotalamount redBold'> " + Convert.ToString(RowValue == null ? "0" : RowValue) + " </td>");
                                }
                                else if (col.ToLower().Trim() == "allocatedamount")
                                {
                                    stringBuilder.Append("<td class='countallocatedamount redBold'> " + Convert.ToString(RowValue == null ? "0" : RowValue) + " </td>");
                                }
                                else if (col.ToLower().Trim() == "expendituretilldate")
                                {
                                    stringBuilder.Append("<td class='countexpendituretilldate redBold'> " + Convert.ToString(RowValue == null ? "0" : RowValue) + " </td>");
                                }
                                else if (col.ToLower().Trim() == "expenditureduringthemonth")
                                {
                                    stringBuilder.Append("<td class='countexpenditureduringthemonth redBold'>  " + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                                }
                                else if (col.ToLower().Trim() == "remaningamount")
                                {
                                    stringBuilder.Append("<td class='countremaningamount redBold'> " + Convert.ToString(RowValue == null ? "0" : RowValue) + " </td>");
                                }
                                else if (col.ToLower().Trim() == "expenditurelastmonth")
                                {
                                    stringBuilder.Append("<td class='countexpenditurelastmonth redBold'> " + Convert.ToString(RowValue == null ? "0" : RowValue) + " </td>");
                                }
                                else
                                {
                                    stringBuilder.Append("<td>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                                }
                            }

                            else if (IsRowAndGrandTotal == 1 && (col.ToLower().Trim() != "flag") && count == 0)
                            {
                                stringBuilder.Append("<td>" + Convert.ToString(RowValue == null ? "0" : RowValue) + "</td>");
                            }



                        }
                        stringBuilder.Append("</tr>");

                    }

                    stringBuilder.Append("</tbody>");
                    #endregion

                    #region Create Footer
                    stringBuilder.Append("<tfoot><tr>");
                    foreach (string col in ColumnList)
                    {
                        int count = AvoidColumnNameArray.Where(s => s == col.ToString().Trim().ToLower()).Count();
                        if (col.Trim().ToLower() == "flag" || count > 0)
                        {
                        }
                        else if (col.ToLower().Trim() == "totalamount")
                        {
                            stringBuilder.Append("<td id='totalamount' class='Bold totalamount'></td>");
                        }
                        else if (col.ToLower().Trim() == "allocatedamount")
                        {
                            stringBuilder.Append("<td id='allocatedamount' class='Bold allocatedamount'></td>");
                        }
                        else if (col.ToLower().Trim() == "expendituretilldate")
                        {
                            stringBuilder.Append("<td id='expendituretilldate' class='Bold expendituretilldate'></td>");
                        }
                        else if (col.ToLower().Trim() == "expenditureduringthemonth")
                        {
                            stringBuilder.Append("<td id='expenditureduringthemonth' class='Bold expenditureduringthemonth'></td>");
                        }
                        else if (col.ToLower().Trim() == "remaningamount")
                        {
                            stringBuilder.Append("<td id='remaningamount' class='Bold remaningamount'></td>");
                        }
                        else if (col.ToLower().Trim() == "expenditurelastmonth")
                        {
                            stringBuilder.Append("<td id='expenditurelastmonth' class='Bold expenditurelastmonth'></td>");
                        }
                        else if (col.ToLower().Trim() == "unit")
                        {
                            stringBuilder.Append("<td id='grandTotal' class='Bold'> Grand Total</td>");
                        }
                        else
                        {
                            stringBuilder.Append("<td></td>");
                        }
                    }
                    stringBuilder.Append("</tr></tfoot>");
                    #endregion
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return stringBuilder.ToString();
        }


    }
}