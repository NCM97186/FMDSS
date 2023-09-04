using FMDSS.Models.ForestFire;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Admin
{
    public class widget
    {
        public int ID { get; set; }
        public string DashboardCount { get; set; }
        public string DashboardText { get; set; }
        public string IconName { get; set; }
    }

    #region Budget Properties
    public class BudgetCircle
    {
        public long SNo { get; set; }
        public long FinancialYearID { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalCount { get; set; }
    }

    public class BudgetDivision
    {
        public long SNo { get; set; }
        public long FinancialYearID { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalCount { get; set; }
    }
    public class BudgetSanctuary
    {
        public long SNo { get; set; }
        public long FinancialYearID { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string DIV_CODE { get; set; }
        public string Place_ID { get; set; }
        public string Place_Name { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalCount { get; set; }
    }
    public class DashboardBudgetFinancialYearWise
    {
        public long FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }
        public decimal AllocatedAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalCount { get; set; }
    }
	#endregion



		#region [Offence Properties]

		public class OffenceCategoryList
    {
        public long Value { get; set; }
        public string Text { get; set; }
    }
    public class CircleWise
    {
        public long SNo { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string CIRCLE_NAME { get; set; }
        public int TotalCount { get; set; }
        public int PendingCount { get; set; }
        public int InCourtCount { get; set; }
        public int ClosedCount { get; set; }
        public int TotalPending { get; set; }
        public int OffenceCategoryId { get; set; }

        public string OffenceCategoryName { get; set; }
        public virtual IEnumerable<SelectListItem> ICATEGORY { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
		public string RANG_CODE { get; set; }
		public string RANG_NAME { get; set; }
		public string NURSERY_CODE { get; set; }
		public string NURSERY_NAME { get; set; }
		public string ProductID { get; set; }
		public string ProductName { get; set; }
		public string Citizen_StockTotal { get; set; }
		public string Dept_StockTotal { get; set; }
		public string Total_StockTotal { get; set; }
		public string Citizen_StockOut { get; set; }
		public string Dept_StockOut { get; set; }
		public string Total_StockOut { get; set; }
		public string Citizen_RemainingQTY { get; set; }
		public string Dept_RemainingQTY { get; set; }
		public string Total_RemainingQty { get; set; }
		public string Nursery_Count { get; set; }
										   //public string FinacialYear { get; set; }



		public static explicit operator CircleWise(List<CircleWise> v)
        {
            throw new NotImplementedException();
        }
    }
	public class FinacialYearList
	{
		public string Years { get; set; }
		public string YearId { get; set; }
	}
	public class NurseryStockSummary
	{
		public string fromDate { get; set; }
		public string toDate { get; set; }
		public string FinacialYear { get; set; }
		public string Citizen_Stock_GrandTotal { get; set; }
		public string Dept_Stock_GrandTotal { get; set; }
		public string Citizen_StockOut_GrandTotal { get; set; }
		public string Dept_StockOut_GrandTotal { get; set; }
		public string GrandTotalStock { get; set; }
		public string GrandTotalStockOut { get; set; }
		public string Citizen_RemainingQTY_GrandTotal { get; set; }
		public string Dept_RemainingQTY_GrandTotal { get; set; }
		public string GrandTotalRemainingQTY { get; set; }
		public string Nursery_Count { get; set; }
		public List<CircleWise> CircleWise { get; set; }
		public List<CircleWise> DivWise { get; set; }
		public List<CircleWise> RangWise { get; set; }
		public List<CircleWise> NurseryWise { get; set; }
		public List<CircleWise> ProductWise { get; set; }
		public List<FinacialYearList> finacialYearList { get; set; }
	}

	public class CircleWiseXLS
	{
		public long SNo { get; set; }
		//public string CIRCLE_CODE { get; set; }
		public string CIRCLE_NAME { get; set; }
		public string Nursery_Count { get; set; }
		public string Citizen_StockTotal { get; set; }
		public string Dept_StockTotal { get; set; }
		public string Total_StockTotal { get; set; }
		public string Citizen_StockOut { get; set; }
		public string Dept_StockOut { get; set; }
		public string Total_StockOut { get; set; }
		//public string Citizen_RemainingQTY { get; set; }
		//public string Dept_RemainingQTY { get; set; }
		//public string Total_RemainingQty { get; set; }
		
	}
	public class DivisionWiseXLS
	{
		public long SNo { get; set; }
		public string DIV_NAME { get; set; }
		public string Nursery_Count { get; set; }
		public string Citizen_StockTotal { get; set; }
		public string Dept_StockTotal { get; set; }
		public string Total_StockTotal { get; set; }
		public string Citizen_StockOut { get; set; }
		public string Dept_StockOut { get; set; }
		public string Total_StockOut { get; set; }
		//public string Citizen_RemainingQTY { get; set; }
		//public string Dept_RemainingQTY { get; set; }
		//public string Total_RemainingQty { get; set; }
		
	}
	public class RangeWiseXLS
	{
		public long SNo { get; set; }
		public string RANG_NAME { get; set; }
		public string Nursery_Count { get; set; }
		public string Citizen_StockTotal { get; set; }
		public string Dept_StockTotal { get; set; }
		public string Total_StockTotal { get; set; }
		public string Citizen_StockOut { get; set; }
		public string Dept_StockOut { get; set; }
		public string Total_StockOut { get; set; }
		//public string Citizen_RemainingQTY { get; set; }
		//public string Dept_RemainingQTY { get; set; }
		//public string Total_RemainingQty { get; set; }
	
	}

	public class NurseryWiseXLS
	{
		public long SNo { get; set; }
		public string NURSERY_NAME { get; set; }
		public string Citizen_StockTotal { get; set; }
		public string Dept_StockTotal { get; set; }
		public string Total_StockTotal { get; set; }
		public string Citizen_StockOut { get; set; }
		public string Dept_StockOut { get; set; }
		public string Total_StockOut { get; set; }
		//public string Citizen_RemainingQTY { get; set; }
		//public string Dept_RemainingQTY { get; set; }
		//public string Total_RemainingQty { get; set; }
	}
	public class ProductWiseXLS
	{
		public long SNo { get; set; }
		//public string ProductID { get; set; }
		public string ProductName { get; set; }
		public string Citizen_StockTotal { get; set; }
		public string Dept_StockTotal { get; set; }
		public string Total_StockTotal { get; set; }
		public string Citizen_StockOut { get; set; }
		public string Dept_StockOut { get; set; }
		public string Total_StockOut { get; set; }
		//public string Citizen_RemainingQTY { get; set; }
		//public string Dept_RemainingQTY { get; set; }
		//public string Total_RemainingQty { get; set; }
	}



	#region Division Wise Offence Details
	public class DivisionWiseOffence
    {
        public long SNo { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public int TotalCount { get; set; }
        public int PendingCount { get; set; }
        public int InCourtCount { get; set; }
        public int ClosedCount { get; set; }
        public int TotalPending { get; set; }
    }

    public class DivisionOffenceWiseReportParameters
    {
        public string Fromdate { get; set; }

        public string Todate { get; set; }

        public string DIV_CODE { get; set; }

        public string OffenceCategory { get; set; }

        public int flag { get; set; }
    }
    #endregion

    public class DashboardDivision
    {
        public long SNo { get; set; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public int TotalCount { get; set; }

        //add by vinod
        public int Pending { get; set; }
        public int CaseInCourt { get; set; }
        public int Closed { get; set; }

        public int TotalPending { get; set; }


    }
    public class DashboardRange
    {
        public long SNo { get; set; }
        public string RANGE_NAME { get; set; }
        public string RANGE_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public int TotalCount { get; set; }
        //add by vinod
        public int Pending { get; set; }
        public int CaseInCourt { get; set; }
        public int Closed { get; set; }

        public int TotalPending { get; set; }
    }
    public class DashboardNaka
    {
        public long SNo { get; set; }
        public int NakaID { get; set; }
        public string NakaName { get; set; }
        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public int TotalCount { get; set; }

        //add by vinod
        public int Pending { get; set; }
        public int CaseInCourt { get; set; }
        public int Closed { get; set; }
    }

    
    public class DashboardOffence
    {
        public long SNo { get; set; }
        public string RANGE_NAME { get; set; }
        public string NakaName { get; set; }
        public string Offense_code { get; set; }
        public string FIRNumber { get; set; }
        public string DIV_NAME { get; set; }
        public string OffenderName { get; set; }
        public string FIRDate { get; set; }
        public string FStatus { get; set; }
        public string OfCategroy { get; set; }

    }
    public class DashboardOffenceDetails
    {
        public long OffenceID { get; set; }
        public string RequestType { get; set; }
        public string Offense_code { get; set; }
        public string RangeName { get; set; }
        public string NakaName { get; set; }

        public string FIRNumber { get; set; }
        public string FIRDate { get; set; }
        public string OffenderName { get; set; }
        public string OffenderAddress { get; set; }
        public string OffenceDescription { get; set; }
        public string IsWPA { get; set; }
        public string WPADescription { get; set; }
        public string IsFA { get; set; }
        public string FADescription { get; set; }
        public string InvestigatorOfficer { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string CompoundStatus { get; set; }
        public string CompoundAmount { get; set; }
        public string IsMaterialReleased { get; set; }
        public string TotalItemSeized { get; set; }
        public string CourtName { get; set; }
        public string FileDate { get; set; }

        public string CourtCaseNumber { get; set; }
        public string NextHearingDate { get; set; }
        public string DateOfFinalReport { get; set; }
        public string DateOfApprovalByDFO { get; set; }

        public string SpecialRemarks { get; set; }
        public string Status { get; set; }
        public string AddedOn { get; set; }
        public string AddedBy { get; set; }

        public string ActiveStatus { get; set; }
        public string OffenceCategory { get; set; }
        public string IsVehicleReleased { get; set; }
        public string NotCompoundedStatus { get; set; }

    }

   
    #endregion

    #region Research Properties
    public class ResearchByResearchType
    {
        public string ResearchType { get; set; }
        public int TotalCount { get; set; }
    }

    public class DashboardResearch
    {
        public long SNo { get; set; }
        public string RequestedId { get; set; }
        public string TitleOfResearch { get; set; }
    }

    public class ResearchPlace
    {
        public long SNo { get; set; }
        public long PlaceID { get; set; }
        public string PlaceName { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region Rescue Properties
    public class DashboardRescue
    {
        public long SNo { get; set; }
        public long RegistrationID { get; set; }
        public string CasualtyType { get; set; }
        public string RescueStatus { get; set; }
        public string CitizenMobileNo { get; set; }
        public string Animal_NAME { get; set; }
    }

    public class DashboardRescueDetails
    {
        public long RegistrationID { get; set; }
        public string CitizenName { get; set; }
        public string CitizenEmailID { get; set; }
        public string CitizenMobileNo { get; set; }
        public string DIST_NAME { get; set; }
        public string BLK_NAME { get; set; }
        public string GP_NAME { get; set; }
        public string VILL_NAME { get; set; }
        public string Location { get; set; }
        public string RegistrationDescription { get; set; }
        public string RegistrationPhotoPath { get; set; }
        public string AnimalNeedTreatment { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string RescuePhotoPath { get; set; }
        public string RescueRemarks { get; set; }
        public string ReleasePhotoPath { get; set; }
        public string ReleaseRemarks { get; set; }
        public string RescueOfficerDesig { get; set; }
        public string RescueOfficerName { get; set; }
        public string SpecialInstruction { get; set; }
        public string RescueStatus { get; set; }
        public string CasualtyType { get; set; }
        public string NoOfPersonInjured { get; set; }
        public string CasualtyDescription { get; set; }
        public string RegistrationApproved { get; set; }
    }

    public class RescueByDistrict
    {
        public long SNo { get; set; }
        public string DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public int TotalCount { get; set; }
    }
    #endregion

    #region for Enchorsment by Sunny
    public class EnchorsmentReport
    {
        public long SNo { get; set; }
        public string Enchorsment_Code { get; set; }
        public string DOE { get; set; }
        public string Range_Name { get; set; }
        public string Division_Name { get; set; }
        public string Area { get; set; }
        public string InvestigationOfficer { get; set; }
        public string Final_Decision_Taken { get; set; }
        public string TotalCount { get; set; }
        public string Division_ID { get; set; }
        public string Range_ID { get; set; }
        public string Encroacher_Name { get; set; }
        public string Encroacher_Address { get; set; }
        public string Decision_Taken { get; set; }
        public string Decision_Date { get; set; }
        public string Next_Date { get; set; }
        public string Next_Decision_Place { get; set; }
        public string Acf_Decision_UploadFileName { get; set; }
    }
    public class EnchorsmentReportByID
    {
        public long SNo { get; set; }
        public string Enchorsment_Code { get; set; }
        public string DOE { get; set; }
        public string Range_Name { get; set; }
        public string Division_Name { get; set; }
        public string Area { get; set; }
        public string InvestigationOfficer { get; set; }
        public string Final_Decision_Taken { get; set; }
        public string TotalCount { get; set; }
        public string DIV_CODE { get; set; }
        public string RANGE_CODE { get; set; }
        public string LRACTNO { get; set; }
        public string IsKnown { get; set; }
        public string Description { get; set; }
        public string Special_Instruction { get; set; }
        public string DispatchNo { get; set; }
        public string DispatchDate { get; set; }
        public string ACF_Status { get; set; }
        public string ACF_Remarks { get; set; }
        public string ACF_Date { get; set; }
        public string NoticeNo { get; set; }
        public string NoticeDate { get; set; }
        public string Final_Decision_OfficerId { get; set; }
        public string Final_Decision_Remarks { get; set; }
        public string Final_Decision_Date { get; set; }
        public string Next_Decision_Date { get; set; }
        public string Next_Decision_Place { get; set; }
        public string KMLFile { get; set; }
        public string KMLFileName { get; set; }
        public string EnteredBy { get; set; }
        public string Acf_Decision_Upload { get; set; }

    }
    public class FileDetailsModel
    {
        public int Id { get; set; }
        public byte[] FileContent { get; set; }


    }
    #endregion

    #region for Year Wise Offence Report
    public class YearWiseOffenceReport
    {
        public int RowNo { get; set; }
        public string CommonName { get; set; }
        public string CIRCLE_NAME { get; set; }
        public int PendingInDept_LastQtr { get; set; }
        public int PendingInCourt_LastQtr { get; set; }
        public int Total_LastQtr { get; set; }
        public int CaseRegistration_Department_CurrentQtr { get; set; }
        public int CaseRegistration_Court_CurrentQtr { get; set; }
        public int Total_CaseRegistration_CurrentQTR { get; set; }
        public int CaseRegistration_Department_Total { get; set; }
        public int CaseRegistration_Court_Total { get; set; }
        public int CaseRegistration_Total { get; set; }
        public int Close_Depart_ThisYear { get; set; }

        public int Close_Court_ThisYear  { get; set; }

        public int Close_Total_ThisYear { get; set; }
        public int CurrentYearCompoundingAmount { get; set; }
        public int TotalSeizedItem { get; set; }

        public int Pending_Dpt_at_the_end_of_this_Qtr { get; set; }
        public int Pending_Court_at_the_end_of_this_Qtr { get; set; }
        public int Pending_Total_at_the_end_of_this_Qtr { get; set; }
        public int PendingInDept_LessThanOneYrs { get; set; }
        public int PendingInDept_btwnOneAndThreeYrs { get; set; }
        public int PendingInDept_GtrThanThreeYrs { get; set; }
        public int TotalPendingInCourt { get; set; }
        public int TotalPendingInDept { get; set; }
        public int OffenceCategory { get; set; }
        public string StartDateQtr { get; set; }
        public string EndDateQtr { get; set; }
        public string StrOffenceCat { get; set; }
    }

    public class YearWiseOffenceReportParameters
    {
        public string Year { get; set; }
        
        public string Quarter { get; set; }
       
        public string OffenceCategory { get; set; }

        public string PrintNames { get; set; }
        public int ReportType { get; set; }
        
    }
	#endregion
	
	public class clsForestDashboard : DAL
    {
        public List<widget> Forestwidgets()
        {
            DataTable dt = new DataTable();
            List<widget> objwidget = new List<Admin.widget>();
            try
            {
                DALConn();
                using (SqlCommand cmd = new SqlCommand("spForestDashboard", Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DashboardCount");
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    objwidget = Globals.Util.GetListFromTable<widget>(dt).ToList();
                }                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spForestDashboard" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return objwidget;
        }
        public dynamic GetDataForDashboard(string moduleID,string offence_category,string FinacialYear, DateTime? fromDate = null, DateTime? toDate = null,long ApiUser=0)
        {
		    DataTable dt = new DataTable();
			DataSet ds = new DataSet();
            List<CircleWise> objCircleWise = new List<CircleWise>();
			NurseryStockSummary nurseryStockSummary = new NurseryStockSummary();
			//Edit by Sunny
			List<EnchorsmentReport> objEnchorsmentReport = new List<EnchorsmentReport>();
            List<ForestFireAlertDashBoard> objForestFire = new List<ForestFireAlertDashBoard>();
            try
            {
                DALConn();
                long UserId = 0;
                if (ApiUser>0)
                    UserId = ApiUser;
                else
                    UserId =Convert.ToInt64(HttpContext.Current.Session["UserId"]);
                
                using (SqlCommand cmd = new SqlCommand("spForestDashboard", Conn))
                { 
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        dynamic data = null;

                        switch (moduleID)
                        {
                            case "1":
                                cmd.Parameters.AddWithValue("Action", "CircleWiseOffence");
                                cmd.Parameters.AddWithValue("Fromdate", fromDate);
                                cmd.Parameters.AddWithValue("Todate", toDate);
                                cmd.Parameters.AddWithValue("@UserId", UserId);
                                cmd.Parameters.AddWithValue("OffenceCategory", offence_category);
                                
                                da.Fill(dt);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    CircleWise CW = new CircleWise();
                                    CW.SNo = Convert.ToInt64(dr["SNo"]);
                                    CW.CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]);
                                    CW.CIRCLE_NAME = Convert.ToString(dr["CIRCLE_NAME"]);
                                    CW.TotalCount = Convert.ToInt32(dr["TotalCount"]);
                                    CW.PendingCount = Convert.ToInt32(dr["Pending"]);
                                    CW.InCourtCount = Convert.ToInt32(dr["InCourt"]);
                                    CW.ClosedCount = Convert.ToInt32(dr["Closed"]);
                                    CW.TotalPending = Convert.ToInt32(dr["TotalPending"]);
                                    objCircleWise.Add(CW);
                                }

                                return objCircleWise;
                            case "2":
                                cmd.Parameters.AddWithValue("Action", "ResearchByResearchType");
                                da.Fill(dt);
                                data = dt.AsEnumerable().Select(x => new ResearchByResearchType
                                {
                                    ResearchType = x.Field<string>("ResearchType"),
                                    TotalCount = x.Field<int>("TotalCount")
                                });
                                return data;
                            case "3":
                                cmd.Parameters.AddWithValue("Action", "BudgetFinancialYear");
                                da.Fill(dt);
                                data = Globals.Util.GetListFromTable<DashboardBudgetFinancialYearWise>(dt);
                                return data;
                            case "4":
                                cmd.Parameters.AddWithValue("Action", "RescueDist");
                                da.Fill(dt);
                                data = dt.AsEnumerable().Select(x => new RescueByDistrict
                                {
                                    DIST_CODE = x.Field<string>("DIST_CODE"),
                                    DIST_NAME = x.Field<string>("DIST_NAME"),
                                    TotalCount = x.Field<int>("TotalCount")
                                });
                                return data;
                            //Edit by Sunny for EventManagement
                            case "6":
                                cmd.Parameters.AddWithValue("Action", "EncroachmentReport");
                                cmd.Parameters.AddWithValue("UserId", UserId);
                                da.Fill(dt);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    EnchorsmentReport EN = new EnchorsmentReport();
                                    EN.Division_Name = Convert.ToString(dr["DIV_NAME"]);
                                    EN.Range_Name = Convert.ToString(dr["RANGE_NAME"]);
                                    EN.TotalCount = Convert.ToString(dr["TotalCount"]);
                                    objEnchorsmentReport.Add(EN);
                                }
                                return objEnchorsmentReport;
                            //Edit by Sunny for ForestFireAlert
                            case "7":                                 
                                cmd.Parameters.AddWithValue("Action", "ForestFireAlert");
                                cmd.Parameters.AddWithValue("@UserId", UserId);
                                da.Fill(dt);
                                foreach (DataRow dr in dt.Rows)
                                {
                                    ForestFireAlertDashBoard EN = new ForestFireAlertDashBoard();
                                    EN.District = Convert.ToString(dr["District"]);
                                    EN.TotalCount = Convert.ToString(dr["TotalCount"]);
                                    objForestFire.Add(EN);
                                }
                                return objForestFire;


							case "8":
									cmd.Parameters.AddWithValue("Action", "CircleWiseNurseryStock");
								    cmd.Parameters.AddWithValue("Fromdate", fromDate);
								    cmd.Parameters.AddWithValue("Todate", toDate);
					                cmd.Parameters.AddWithValue("@UserId", UserId);
									cmd.Parameters.AddWithValue("@FinacialYear", FinacialYear);
									da.Fill(ds);
									nurseryStockSummary.finacialYearList = Globals.Util.GetListFromTable<FinacialYearList>(ds.Tables[6]);
								
								//if (fromDate != null && toDate != null)
								//{
									nurseryStockSummary.fromDate = fromDate.ToString();
									nurseryStockSummary.toDate = toDate.ToString();
								    nurseryStockSummary.FinacialYear = FinacialYear;
									nurseryStockSummary.Citizen_Stock_GrandTotal = ds.Tables[0].Rows[0][0].ToString();
									nurseryStockSummary.Dept_Stock_GrandTotal = ds.Tables[0].Rows[0][1].ToString();
									nurseryStockSummary.GrandTotalStock= ds.Tables[0].Rows[0][2].ToString();
									nurseryStockSummary.Citizen_StockOut_GrandTotal = ds.Tables[0].Rows[0][3].ToString();
									nurseryStockSummary.Dept_StockOut_GrandTotal = ds.Tables[0].Rows[0][4].ToString();
								    nurseryStockSummary.GrandTotalStockOut= ds.Tables[0].Rows[0][5].ToString();
								    nurseryStockSummary.Citizen_RemainingQTY_GrandTotal = ds.Tables[0].Rows[0][6].ToString();
								    nurseryStockSummary.Dept_RemainingQTY_GrandTotal = ds.Tables[0].Rows[0][7].ToString();
								    nurseryStockSummary.GrandTotalRemainingQTY = ds.Tables[0].Rows[0][8].ToString();
								    nurseryStockSummary.Nursery_Count= ds.Tables[0].Rows[0][9].ToString();
									nurseryStockSummary.CircleWise = new List<CircleWise>();
									nurseryStockSummary.DivWise = new List<CircleWise>();
									nurseryStockSummary.RangWise = new List<CircleWise>();
									nurseryStockSummary.NurseryWise = new List<CircleWise>();
								    nurseryStockSummary.ProductWise = new List<CircleWise>();
								//}
								foreach (DataRow dr in ds.Tables[1].Rows)
								{
								
									
									nurseryStockSummary.CircleWise.Add(new CircleWise()
									{
										SNo = Convert.ToInt64(dr["SNo"]),
										CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]),
										CIRCLE_NAME = Convert.ToString(dr["CIRCLE_NAME"]),
										Citizen_StockTotal = dr["Citizen_StockTotal"].ToString(),
										Dept_StockTotal = dr["Dept_StockTotal"].ToString(),
										Total_StockTotal= dr["Total_StockTotal"].ToString(),
										Citizen_StockOut = dr["Citizen_StockOut"].ToString(),
										Dept_StockOut = dr["Dept_StockOut"].ToString(),
										Total_StockOut= dr["Total_StockOut"].ToString(),
										Citizen_RemainingQTY= dr["Citizen_RemainingQTY"].ToString(),
										Dept_RemainingQTY= dr["Dept_RemainingQTY"].ToString(),
										Total_RemainingQty= dr["Dept_RemainingQTY"].ToString(),
										Nursery_Count = dr["Nursery_Count"].ToString()

									});
								}
								
								foreach (DataRow dr in ds.Tables[2].Rows)
								{
									
									
									nurseryStockSummary.DivWise.Add(new CircleWise()
									{
										SNo = Convert.ToInt64(dr["SNo"]),
										CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]),
										CIRCLE_NAME = Convert.ToString(dr["CIRCLE_NAME"]),
										DIV_CODE = Convert.ToString(dr["DIV_CODE"]),
										DIV_NAME = Convert.ToString(dr["DIV_NAME"]),
										Citizen_StockTotal = dr["Citizen_StockTotal"].ToString(),
										Dept_StockTotal = dr["Dept_StockTotal"].ToString(),
										Total_StockTotal = dr["Total_StockTotal"].ToString(),
										Citizen_StockOut = dr["Citizen_StockOut"].ToString(),
										Dept_StockOut = dr["Dept_StockOut"].ToString(),
										Total_StockOut = dr["Total_StockOut"].ToString(),
										Citizen_RemainingQTY = dr["Citizen_RemainingQTY"].ToString(),
										Dept_RemainingQTY = dr["Dept_RemainingQTY"].ToString(),
										Total_RemainingQty = dr["Dept_RemainingQTY"].ToString(),
										Nursery_Count = dr["Nursery_Count"].ToString()
									});
								}
								foreach (DataRow dr in ds.Tables[3].Rows)
								{	
									nurseryStockSummary.RangWise.Add(new CircleWise()
									{
										SNo = Convert.ToInt64(dr["SNo"]),
										DIV_CODE = Convert.ToString(dr["DIV_CODE"]),
										DIV_NAME = Convert.ToString(dr["DIV_NAME"]),
										RANG_CODE = Convert.ToString(dr["RANGE_CODE"]),
										RANG_NAME = Convert.ToString(dr["RANGE_NAME"]),
										Citizen_StockTotal = dr["Citizen_StockTotal"].ToString(),
										Dept_StockTotal = dr["Dept_StockTotal"].ToString(),
										Total_StockTotal = dr["Total_StockTotal"].ToString(),
										Citizen_StockOut = dr["Citizen_StockOut"].ToString(),
										Dept_StockOut = dr["Dept_StockOut"].ToString(),
										Total_StockOut = dr["Total_StockOut"].ToString(),
										Citizen_RemainingQTY = dr["Citizen_RemainingQTY"].ToString(),
										Dept_RemainingQTY = dr["Dept_RemainingQTY"].ToString(),
										Total_RemainingQty = dr["Dept_RemainingQTY"].ToString(),
										Nursery_Count = dr["Nursery_Count"].ToString()
									});
								}
								foreach (DataRow dr in ds.Tables[4].Rows)
								{
									nurseryStockSummary.NurseryWise.Add(new CircleWise()
									{
										SNo = Convert.ToInt64(dr["SNo"]),
										RANG_CODE = Convert.ToString(dr["RANGE_CODE"]),
										RANG_NAME = Convert.ToString(dr["RANGE_NAME"]),
										NURSERY_CODE = Convert.ToString(dr["NURSERY_CODE"]),
										NURSERY_NAME = Convert.ToString(dr["NURSERY_NAME"]),
										Citizen_StockTotal = dr["Citizen_StockTotal"].ToString(),
										Dept_StockTotal = dr["Dept_StockTotal"].ToString(),
										Total_StockTotal = dr["Total_StockTotal"].ToString(),
										Citizen_StockOut = dr["Citizen_StockOut"].ToString(),
										Dept_StockOut = dr["Dept_StockOut"].ToString(),
										Total_StockOut = dr["Total_StockOut"].ToString(),
										Citizen_RemainingQTY = dr["Citizen_RemainingQTY"].ToString(),
										Dept_RemainingQTY = dr["Dept_RemainingQTY"].ToString(),
										Total_RemainingQty = dr["Dept_RemainingQTY"].ToString()
									});
								}

								foreach (DataRow dr in ds.Tables[5].Rows)
								{
									nurseryStockSummary.ProductWise.Add(new CircleWise()
									{
										SNo = Convert.ToInt64(dr["SNo"]),
										NURSERY_CODE = Convert.ToString(dr["NURSERY_CODE"]),
										NURSERY_NAME = Convert.ToString(dr["NURSERY_NAME"]),
										ProductID = Convert.ToString(dr["ProductID"]),
										ProductName = Convert.ToString(dr["ProductName"]),
										Citizen_StockTotal = dr["Citizen_StockTotal"].ToString(),
										Dept_StockTotal = dr["Dept_StockTotal"].ToString(),
										Total_StockTotal = dr["Total_StockTotal"].ToString(),
										Citizen_StockOut = dr["Citizen_StockOut"].ToString(),
										Dept_StockOut = dr["Dept_StockOut"].ToString(),
										Total_StockOut = dr["Total_StockOut"].ToString(),
										Citizen_RemainingQTY = dr["Citizen_RemainingQTY"].ToString(),
										Dept_RemainingQTY = dr["Dept_RemainingQTY"].ToString(),
										Total_RemainingQty = dr["Dept_RemainingQTY"].ToString()
									});
								}

								return nurseryStockSummary;
						}




                    }
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spForestDashboard" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return objCircleWise;
        }


        

        //Edit by Sunny
        public FileDetailsModel GetFileList(string id)
        {
            DataTable dt = new DataTable();
            FileDetailsModel WFList = new FileDetailsModel();
            dynamic data = null;
            try
            {
                DALConn();
                using (SqlCommand cmd = new SqlCommand("spForestDashboard", Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("Action", "GetFileEnchorsment");
                    cmd.Parameters.AddWithValue("ParentID", id);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        WFList.FileContent = (byte[])(dt.Rows[0]["FileContent"]);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spForestDashboard" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return WFList;
        
        }
        //public dynamic GetDataForDashboard(string moduleName, string parentID, string type)
        //{
        //    DataTable dt = new DataTable();
        //    dynamic data = null;

        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("spForestDashboard", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        switch (moduleName)
        //        {
        //            case "Budget":
        //                if (type == "BudgetCircle")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "BudgetCircle");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<BudgetCircle>(dt);
        //                }
        //                else if (type == "BudgetDivision")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "BudgetDivision");
        //                    cmd.Parameters.AddWithValue("CIRCLE_CODE", parentID.Split('#')[0]);
        //                    cmd.Parameters.AddWithValue("ParentID", parentID.Split('#')[1]);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<BudgetDivision>(dt);
        //                }
        //                else if (type == "BudgetSanctuary")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "BudgetSanctuary");
        //                    cmd.Parameters.AddWithValue("CIRCLE_CODE", parentID.Split('#')[0]);
        //                    cmd.Parameters.AddWithValue("DIV_CODE", parentID.Split('#')[1]);
        //                    cmd.Parameters.AddWithValue("ParentID", parentID.Split('#')[2]);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<BudgetSanctuary>(dt);
        //                }
        //                break;
        //            case "Offence":
        //                if (type == "OffenceDivisionList")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "OffenceDivision");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<DashboardDivision>(dt);
        //                }
        //                else if (type == "OffenceListByDivision")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "OffenceListByDivision");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<DashboardOffence>(dt);
        //                }
        //                else if (type == "OffenceDetailsByOffenceCode")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "OffenceDetailsByOffenceCode");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<DashboardOffenceDetails>(dt).FirstOrDefault();
        //                }
        //                break;
        //            case "Rescue":
        //                if (type == "RescueListByDist")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "RescueListByDist");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<DashboardRescue>(dt);
        //                }
        //                else if (type == "RescueDetailsByID")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "RescueDetailsByID");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<DashboardRescueDetails>(dt).FirstOrDefault();
        //                }
        //                break;
        //            case "Research":
        //                if (type == "Place")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "ResearchPlace");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<ResearchPlace>(dt);
        //                }
        //                else if (type == "ResearchListByPlace")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "ResearchListByPlace");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<DashboardResearch>(dt);
        //                }
        //                break;
        //            //Edit by Sunny
        //            case "Enchorsment":
        //                if (type == "Division")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "Enchorsment_Division");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<EnchorsmentReport>(dt);
        //                }
        //                else if (type == "EnchorsmentDetailsByID")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "EnchorsmentDetailsByID");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<EnchorsmentReportByID>(dt);
        //                }
        //                else if (type == "Flow")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "EnchorsmentDetailsByIDFlow");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<EnchorsmentReport>(dt);
        //                }
        //                break;
        //            //Edit by Sunny
        //            case "ForestFireAlert":
        //                if (type == "District")
        //                {
        //                    cmd.Parameters.AddWithValue("Action", "ForestFireAlertDistrict");
        //                    cmd.Parameters.AddWithValue("ParentID", parentID);
        //                    da.Fill(dt);
        //                    data = Globals.Util.GetListFromTable<ForestFireAlertDistrict>(dt);
        //                }
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spForestDashboard" + "_" + "Admin", 0, DateTime.Now, 0);
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return data;
        //}
        public dynamic GetDataForDashboard(string moduleName, string parentID, string type, string status, DateTime? fromDate, DateTime? toDate, int OffenceId,long ApiUser)
        {
            long UserId = 0;
            if (ApiUser > 0)
                UserId = ApiUser;
            else
                UserId = Convert.ToInt64(HttpContext.Current.Session["UserId"]);

            DataTable dt = new DataTable();
            dynamic data = null;

            try
            {
                DALConn();
                using (SqlCommand cmd = new SqlCommand("spForestDashboard", Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        switch (moduleName)
                        {
                            case "Budget":
                                if (type == "BudgetCircle")
                                {
                                    cmd.Parameters.AddWithValue("Action", "BudgetCircle");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<BudgetCircle>(dt);
                                }
                                else if (type == "BudgetDivision")
                                {
                                    cmd.Parameters.AddWithValue("Action", "BudgetDivision");
                                    cmd.Parameters.AddWithValue("CIRCLE_CODE", parentID.Split('#')[0]);
                                    cmd.Parameters.AddWithValue("ParentID", parentID.Split('#')[1]);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<BudgetDivision>(dt);
                                }
                                else if (type == "BudgetSanctuary")
                                {
                                    cmd.Parameters.AddWithValue("Action", "BudgetSanctuary");
                                    cmd.Parameters.AddWithValue("CIRCLE_CODE", parentID.Split('#')[0]);
                                    cmd.Parameters.AddWithValue("DIV_CODE", parentID.Split('#')[1]);
                                    cmd.Parameters.AddWithValue("ParentID", parentID.Split('#')[2]);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<BudgetSanctuary>(dt);
                                }
                                break;
                            case "Offence":
                                if (type == "OffenceDivisionList")
                                {
                                    cmd.Parameters.AddWithValue("Action", "OffenceDivision");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("Status", status);
                                    cmd.Parameters.AddWithValue("Fromdate", fromDate);
                                    cmd.Parameters.AddWithValue("Todate", toDate);
                                    cmd.Parameters.AddWithValue("OffenceCategory", OffenceId);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardDivision>(dt);
                                }
                                else if (type == "OffenceListByDivision")
                                {
                                    cmd.Parameters.AddWithValue("Action", "OffenceListByDivision");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("Status", status);
                                    cmd.Parameters.AddWithValue("Fromdate", fromDate);
                                    cmd.Parameters.AddWithValue("Todate", toDate);
                                    cmd.Parameters.AddWithValue("OffenceCategory", OffenceId);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardOffence>(dt);
                                }
                                else if (type == "OffenceRange")
                                {
                                    cmd.Parameters.AddWithValue("Action", "OffenceRange");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("Status", status);
                                    cmd.Parameters.AddWithValue("Fromdate", fromDate);
                                    cmd.Parameters.AddWithValue("Todate", toDate);
                                    cmd.Parameters.AddWithValue("OffenceCategory", OffenceId);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardRange>(dt);
                                }
                                else if (type == "OffenceNaka")
                                {
                                    cmd.Parameters.AddWithValue("Action", "OffenceNaka");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("Status", status);
                                    cmd.Parameters.AddWithValue("Fromdate", fromDate);
                                    cmd.Parameters.AddWithValue("Todate", toDate);
                                    cmd.Parameters.AddWithValue("OffenceCategory", OffenceId);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardNaka>(dt);
                                }

                                else if (type == "OffenceDetailsByOffenceCode")
                                {
                                    cmd.Parameters.AddWithValue("Action", "OffenceDetailsByOffenceCode");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardOffenceDetails>(dt).FirstOrDefault();
                                }

                                break;
                            case "Rescue":
                                if (type == "RescueListByDist")
                                {
                                    cmd.Parameters.AddWithValue("Action", "RescueListByDist");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardRescue>(dt);
                                }
                                else if (type == "RescueDetailsByID")
                                {
                                    cmd.Parameters.AddWithValue("Action", "RescueDetailsByID");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardRescueDetails>(dt).FirstOrDefault();
                                }
                                break;
                            case "Research":
                                if (type == "Place")
                                {
                                    cmd.Parameters.AddWithValue("Action", "ResearchPlace");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<ResearchPlace>(dt);
                                }
                                else if (type == "ResearchListByPlace")
                                {
                                    cmd.Parameters.AddWithValue("Action", "ResearchListByPlace");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<DashboardResearch>(dt);
                                }
                                break;
                            //Edit by Sunny
                            case "Enchorsment":
                                if (type == "Division")
                                {
                                    cmd.Parameters.AddWithValue("Action", "Enchorsment_Division");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<EnchorsmentReport>(dt);
                                }
                                else if (type == "EnchorsmentDetailsByID")
                                {
                                    cmd.Parameters.AddWithValue("Action", "EnchorsmentDetailsByID");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<EnchorsmentReportByID>(dt);
                                }
                                else if (type == "Flow")
                                {
                                    cmd.Parameters.AddWithValue("Action", "EnchorsmentDetailsByIDFlow");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<EnchorsmentReport>(dt);
                                }
                                break;
                            //Edit by Sunny
                            case "ForestFireAlert":
                                if (type == "District")
                                {
                                    cmd.Parameters.AddWithValue("Action", "ForestFireAlertDistrict");
                                    cmd.Parameters.AddWithValue("ParentID", parentID);
                                    cmd.Parameters.AddWithValue("UserID", UserId);
                                    da.Fill(dt);
                                    data = Globals.Util.GetListFromTable<ForestFireAlertDistrict>(dt);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spForestDashboard" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return data;
        }


        public dynamic GetYearWiseOffenceReport(YearWiseOffenceReportParameters param)
        {
            DataTable dt = new DataTable();
            List<YearWiseOffenceReport> objYearWise = new List<YearWiseOffenceReport>();
            try
            {

                DALConn();
                
                using (SqlCommand cmd = new SqlCommand("spYearWiseOffenceReport", Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FinancialYear", param.Year);
                    cmd.Parameters.AddWithValue("@MonthStartQtr", param.Quarter);
                    cmd.Parameters.AddWithValue("@OffenceCategory", param.OffenceCategory);
                    cmd.Parameters.AddWithValue("@ReportType", param.ReportType);
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        foreach (DataRow dr in dt.Rows)
                        {
                            YearWiseOffenceReport YW = new YearWiseOffenceReport();
                            YW.RowNo = Convert.ToInt32(dr["RowNo"]);
                            YW.CommonName = Convert.ToString(dr["CommonName"]);
                            YW.PendingInDept_LastQtr = Convert.ToInt32(dr["PendingInDept_LastQtr"]);
                            YW.PendingInCourt_LastQtr = Convert.ToInt32(dr["PendingInCourt_LastQtr"]);
                            YW.Total_LastQtr = Convert.ToInt32(dr["Total_LastQtr"]);
                            YW.CaseRegistration_Department_CurrentQtr = Convert.ToInt32(dr["CaseRegistration_Department_CurrentQtr"]);
                            YW.CaseRegistration_Court_CurrentQtr = Convert.ToInt32(dr["CaseRegistration_Court_CurrentQtr"]);
                            YW.Total_CaseRegistration_CurrentQTR = Convert.ToInt32(dr["Total_CaseRegistration_CurrentQTR"]);
                            YW.CaseRegistration_Department_Total = Convert.ToInt32(dr["CaseRegistration_Department_Total"]);
                            YW.CaseRegistration_Court_Total = Convert.ToInt32(dr["CaseRegistration_Court_Total"]);
                            YW.CaseRegistration_Total = Convert.ToInt32(dr["CaseRegistration_Total"]);
                            YW.Close_Depart_ThisYear = Convert.ToInt32(dr["Close_Depart_ThisYear"]);
                            YW.Close_Court_ThisYear = Convert.ToInt32(dr["Close_Court_ThisYear"]);
                            YW.Close_Total_ThisYear = Convert.ToInt32(dr["Close_Total_ThisYear"]);
                            YW.CurrentYearCompoundingAmount = Convert.ToInt32(dr["CurrentYearCompoundingAmount"]);
                            YW.TotalSeizedItem = Convert.ToInt32(dr["TotalSeizedItem"]);
                            YW.Pending_Dpt_at_the_end_of_this_Qtr = Convert.ToInt32(dr["Pending_Dpt_at_the_end_of_this_Qtr"]);
                            YW.Pending_Court_at_the_end_of_this_Qtr = Convert.ToInt32(dr["Pending_Court_at_the_end_of_this_Qtr"]);
                            YW.Pending_Total_at_the_end_of_this_Qtr = Convert.ToInt32(dr["Pending_Total_at_the_end_of_this_Qtr"]);
                            YW.PendingInDept_LessThanOneYrs = Convert.ToInt32(dr["PendingInDept_LessThanOneYrs"]);
                            YW.PendingInDept_btwnOneAndThreeYrs = Convert.ToInt32(dr["PendingInDept_btwnOneAndThreeYrs"]);
                            YW.PendingInDept_GtrThanThreeYrs = Convert.ToInt32(dr["PendingInDept_GtrThanThreeYrs"]);
                            YW.TotalPendingInCourt = Convert.ToInt32(dr["TotalPendingInCourt"]);
                            YW.TotalPendingInDept = Convert.ToInt32(dr["TotalPendingInDept"]);
                            YW.StartDateQtr = Convert.ToString(dr["StartDateQtr"]);
                            YW.EndDateQtr = Convert.ToString(dr["EndDateQtr"]);
                            YW.StrOffenceCat = Convert.ToString(dr["StrOffenceCat"]);
                            objYearWise.Add(YW);
                        }
                        return objYearWise;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spForestDashboard" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return objYearWise;
        }
        public dynamic GetOffenceDashboardReportDivisionOffenceWise(string fromDate, string toDate, string DIV_CODE, string OffenceCategory, int flag)
        {
            DataTable dt = new DataTable();
            List<CircleWise> objDivWise = new List<CircleWise>();
            try
            {
                long UserId = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
                DALConn();
                using (SqlCommand cmd = new SqlCommand("spOffenceDashboardReportDivisionOffenceWise", Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Fromdate", fromDate);
                    cmd.Parameters.AddWithValue("@Todate", toDate);
                    cmd.Parameters.AddWithValue("@DIV_CODE", DIV_CODE);
                    cmd.Parameters.AddWithValue("@OffenceCategory", OffenceCategory);
                    cmd.Parameters.AddWithValue("@flag", flag);
                    cmd.Parameters.AddWithValue("@UserID", UserId);
                    
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        foreach (DataRow dr in dt.Rows)
                        {
                            CircleWise DW = new CircleWise();
                            DW.SNo = Convert.ToInt32(dr["SNo"]);
                            DW.DIV_CODE = Convert.ToString(dr["DIV_CODE"]);
                            DW.DIV_NAME = Convert.ToString(dr["DIV_NAME"]);
                            DW.TotalCount = Convert.ToInt32(dr["TotalCount"]);
                            DW.PendingCount = Convert.ToInt32(dr["Pending"]);
                            DW.InCourtCount = Convert.ToInt32(dr["CaseInCourt"]);
                            DW.ClosedCount = Convert.ToInt32(dr["Closed"]);
                            DW.TotalPending = Convert.ToInt32(dr["TotalPending"]);

                            objDivWise.Add(DW);
                        }
                        return objDivWise;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spOffenceDashboardReportDivisionOffenceWise" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return objDivWise;
        }        
        public dynamic GetOffenceDashboardReportOffenceWise(string fromDate, string toDate, string DIV_CODE, string OffenceCategory, int flag)
        {
            DataTable dt = new DataTable();
            List<CircleWise> objDivWise = new List<CircleWise>();
            try
            {
                long UserId = Convert.ToInt64(HttpContext.Current.Session["UserId"]);
                DALConn();
                using (SqlCommand cmd = new SqlCommand("spOffenceDashboardReportDivisionOffenceWise", Conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Fromdate", fromDate);
                    cmd.Parameters.AddWithValue("@Todate", toDate);
                    cmd.Parameters.AddWithValue("@DIV_CODE", DIV_CODE);
                    cmd.Parameters.AddWithValue("@OffenceCategory", OffenceCategory);
                    cmd.Parameters.AddWithValue("@flag", flag);
                    cmd.Parameters.AddWithValue("@UserID", UserId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        foreach (DataRow dr in dt.Rows)
                        {
                            CircleWise DW = new CircleWise();
                            DW.SNo = Convert.ToInt32(dr["SNo"]);
                            DW.OffenceCategoryName = Convert.ToString(dr["OffenceCategoryName"]);
                            //DW.DIV_CODE = Convert.ToString(dr["DIV_CODE"]);
                            DW.TotalCount = Convert.ToInt32(dr["TotalCount"]);
                            DW.PendingCount = Convert.ToInt32(dr["Pending"]);
                            DW.InCourtCount = Convert.ToInt32(dr["CaseInCourt"]);
                            DW.ClosedCount = Convert.ToInt32(dr["Closed"]);
                            DW.TotalPending = Convert.ToInt32(dr["TotalPending"]);

                            objDivWise.Add(DW);
                        }
                        return objDivWise;
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spOffenceDashboardReportDivisionOffenceWise" + "_" + "Admin", 0, DateTime.Now, 0);
            }
            finally
            {
                Conn.Close();
            }
            return objDivWise;
        }
    }
}