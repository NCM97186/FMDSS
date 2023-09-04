//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : DM-Model 
//  Description  : This file is responsible for all data and business rule for Work order, milestone and work order progress
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Durgesh N Sharma
//  Modified By  : Durgesh N Sharma  
//  Modified On  : 10-Mar-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@



using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForestDevelopment
{
    public class Workorder : DAL
    {
        #region Class common property
        public Int64 UserID 
        {  get
        {
                return  Convert.ToInt64(HttpContext.Current.Session["UserId"]);
        }
            set { value = this.UserID; }
        }
        public long ID { get; set; }

        public string EnteredOn { get; set; }
        public string SurveyID{get;set;}
        public long EnteredBy { get; set; }

        public string UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public int Status { get; set; }
        #endregion Class common property

        #region Workorder Definition

        public string refno { get; set; }
        public string refDocument { get; set; }
        public string WorkOrder_Code { get; set; }
        [Required(ErrorMessage = "Descriptiom Required")]
        public string WorkOrder_Name { get; set; }

        public string IFMC_WorkOrder_Code { get; set; }

        public string DIV_CODE { get; set; }

        public string Vill_Code { get; set; }
        public string hdnVillageCode { get; set; }
        public long MicroPlanID { get; set; }

        public long ProjectID { get; set; }

        public string WorkOrderType { get; set; }

        public string ModelIDs { get; set; }

        public string ActivityIDs { get; set; }

        public string Placeofwork{get;set;}
         [Required(ErrorMessage = "Start Date Required")]
        public string StartDate { get; set; }
         [Required(ErrorMessage = "End Date Required")]
        public string EndDate { get; set; }
        public string ContractAgencyType { get; set; }
        public string JFMCorContractAgency { get; set; }

        public string AdminApprovedOrderNo { get; set; }
        public string AdminApprovedDate { get; set; }
        public string FinanceApprovedOrderNo { get; set; }
        public string FinanceApprovedDate { get; set; }
        public string BudgetHead { get; set; }
         [Required(ErrorMessage = "Activity cost per unit required")]
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        //[Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal FinancialTarget { get; set; }
         [Required(ErrorMessage = "Total project cost Required")]
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        //[Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal TotalProjectCost { get; set; }
         [Required(ErrorMessage = "Total area covered Required")]
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        //[Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal PhysicalTarget { get; set; }

        public bool AdminApproved { get; set; }

        public bool FinanceApproved { get; set; }

        public string StatusDesc { get; set; }
        #endregion Workorder Definition


        #region WorkOrderProgress

        public long WorkOrderID { get; set; }

        public long Model { get; set; }

        public long Activity { get; set; }

        public long SubActivity { get; set; }

        public long MileStoneID { get; set; }

        public string ProgressStatus { get; set; }

 public string BillVoucherNo { get; set; } 
        public string BillVoucherDate { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal BillVoucherAmount { get; set; }
        public string ProgressImage { get; set; }

        public string FieldOfficerName { get; set; }

        public string FieldOfficerDesignation { get; set; }
[Required(AllowEmptyStrings = false, ErrorMessage = "Area of Operation is required.")]
        public string FieldName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 99.00, ErrorMessage = "Value must be between 0 - 99.00")]
        public string GPSLatitute { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 99.00, ErrorMessage = "Value must be between 0 - 99.00")]
        public string GPSLongitute { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal BSRPerUnit { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal BSRPerUnitMat { get; set; }
        //[RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        //[Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal noofunitMat { get; set; }
        ////[RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        //[Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal TotalCost { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        //[Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]       
        public decimal AreaCoveredinSQKM { get; set; }
       
        [RegularExpression(@"[-+]?[0-9]*", ErrorMessage = "Number required.")]
        [Range(0, 9999, ErrorMessage = "Value must be between 0 - 9,999")]
        public decimal MenDaysWorked { get; set; }

        public decimal TotalCostlabour { get; set; }
        public decimal TotalCostMaterial { get; set; }
        public string SupportedFile { get; set; }
        public string ProduceType { get; set; }
        public long ProduceTypeID { get; set; }

        public long ProductID { get; set; }
        public long AssetCategoryID { get; set; }

        public long AssetID { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public long TotalQuantity { get; set; }
        public long CompletedMilestone { get; set; }
        public string CompletedMilestoneActivity { get; set; }
        public string AuctionFromSite { get; set; }
        #endregion

        #region Extra Parameter

        public string DIST_NAME{get;set;}
        public string Model_Name { get; set; }
        public string Activity_Name { get; set; }
        public string Sub_Activity_Name { get; set; }
        public string BLK_NAME{get;set;}
        public string RANGE_CODE { get; set; }
    public string  GP_NAME{get;set;}
        public string VILL_NAME{get;set;}
        public string MicroPlanName {get;set;}
        public string RowID { get; set; }

        #endregion Extra Parameter

        /// <summary>
        /// SubmitWorkOrder
        /// </summary>
        /// <param name="_objWO"></param>
        /// <param name="womp"></param>
        /// <param name="WOMilestone"></param>
        /// <returns></returns>
        public Int64 SubmitWorkOrder(Workorder _objWO, DataTable womp, DataTable WOMilestone)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_WORKORDER", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objWO.ID);
                //cmd.Parameters.AddWithValue("@WorkOrder_Code", _objWO.WorkOrder_Code);
                cmd.Parameters.AddWithValue("@refno", _objWO.refno);
                cmd.Parameters.AddWithValue("@refDocument", _objWO.refDocument);
                cmd.Parameters.AddWithValue("@WorkOrder_Name", _objWO.WorkOrder_Name);
                cmd.Parameters.AddWithValue("@IFMC_WorkOrder_Code", _objWO.IFMC_WorkOrder_Code);
                cmd.Parameters.AddWithValue("@Div_Code", _objWO.DIV_CODE);              
                cmd.Parameters.AddWithValue("@WorkOrderType", _objWO.WorkOrderType);                
                cmd.Parameters.AddWithValue("@StartDate",DateTime.ParseExact( _objWO.StartDate,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@EndDate",DateTime.ParseExact( _objWO.EndDate,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@ContractAgencyType", _objWO.ContractAgencyType);
                cmd.Parameters.AddWithValue("@JFMCorContractAgency", _objWO.JFMCorContractAgency);
                cmd.Parameters.AddWithValue("@BudgetHead", _objWO.BudgetHead);
                cmd.Parameters.AddWithValue("@FinancialTarget", _objWO.FinancialTarget);
                cmd.Parameters.AddWithValue("@TotalProjectCost", _objWO.TotalProjectCost);
                cmd.Parameters.AddWithValue("@Placeofwork", _objWO.Placeofwork);
                cmd.Parameters.AddWithValue("@PhysicalTarget", _objWO.PhysicalTarget);
                cmd.Parameters.AddWithValue("@AdminApprovedOrderNo", _objWO.AdminApprovedOrderNo);
                cmd.Parameters.AddWithValue("@AdminApprovedDate",DateTime.ParseExact( _objWO.AdminApprovedDate,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@FinanceApprovedOrderNo", _objWO.FinanceApprovedOrderNo);
                cmd.Parameters.AddWithValue("@FinanceApprovedDate",DateTime.ParseExact( _objWO.FinanceApprovedDate,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@WOMP", womp);
               // cmd.Parameters.AddWithValue("@Milestone", WOMilestone);
                cmd.Parameters.AddWithValue("@EnteredBy", _objWO.UserID);
                cmd.Parameters.AddWithValue("@UpdatedBy", _objWO.UserID);
                cmd.Parameters.AddWithValue("@IsActive", _objWO.IsActive);
                cmd.Parameters.AddWithValue("@Status", _objWO.Status);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitWorkOrder" + "_" + "WorkOrder", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        /// <summary>
        /// SubmitWorkOrderProgress
        /// </summary>
        /// <param name="_objWO"></param>
        /// <returns></returns>
        public Int64 SubmitWorkOrderProgress(Workorder _objWO)
        {
            DALConn();
            SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_WORKORDERProgress", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", _objWO.ID);
         
            cmd.Parameters.AddWithValue("@WorkOrderID", _objWO.WorkOrderID);
            cmd.Parameters.AddWithValue("@Model", _objWO.Model);
            cmd.Parameters.AddWithValue("@SurveyID", _objWO.SurveyID);
            cmd.Parameters.AddWithValue("@Activity", _objWO.Activity);
            cmd.Parameters.AddWithValue("@SubActivity", _objWO.SubActivity);
            cmd.Parameters.AddWithValue("@MileStoneId", _objWO.MileStoneID);
            cmd.Parameters.AddWithValue("@ProgressStatus", _objWO.ProgressStatus);
            cmd.Parameters.AddWithValue("@ProgressImage", _objWO.ProgressImage);
            cmd.Parameters.AddWithValue("@JFMCorContractAgency", _objWO.JFMCorContractAgency);
            cmd.Parameters.AddWithValue("@FieldOfficerName", _objWO.FieldOfficerName);
            cmd.Parameters.AddWithValue("@FieldOfficerDesignation", _objWO.FieldOfficerDesignation);
            cmd.Parameters.AddWithValue("@FieldName", _objWO.FieldName);
            cmd.Parameters.AddWithValue("@Description", _objWO.Description);
            cmd.Parameters.AddWithValue("@GPSLatitute", _objWO.GPSLatitute);
            cmd.Parameters.AddWithValue("@GPSLongitute", _objWO.GPSLongitute);
            cmd.Parameters.AddWithValue("@BSRPerUnit", _objWO.BSRPerUnit);
            cmd.Parameters.AddWithValue("@MenDaysWorked", _objWO.MenDaysWorked);
            cmd.Parameters.AddWithValue("@BSRPerUnitMat", _objWO.BSRPerUnitMat);
            cmd.Parameters.AddWithValue("@noofunitMat", _objWO.noofunitMat);
            cmd.Parameters.AddWithValue("@TotalCost", _objWO.TotalCost);
            cmd.Parameters.AddWithValue("@BillVoucherNo", _objWO.BillVoucherNo);
            cmd.Parameters.AddWithValue("@BillVoucherDate",DateTime.ParseExact(_objWO.BillVoucherDate,"dd/MM/yyyy",null));
            cmd.Parameters.AddWithValue("@BillVoucherAmount", _objWO.BillVoucherAmount);
            cmd.Parameters.AddWithValue("@SupportedFile", _objWO.SupportedFile);
            cmd.Parameters.AddWithValue("@ProduceTypeID", _objWO.ProduceTypeID);
            cmd.Parameters.AddWithValue("@ProductID", _objWO.ProductID);
            cmd.Parameters.AddWithValue("@ProduceType", _objWO.ProduceType);
            cmd.Parameters.AddWithValue("@AssetCategoryID", _objWO.AssetCategoryID);
            cmd.Parameters.AddWithValue("@AssetID", _objWO.AssetID);
            cmd.Parameters.AddWithValue("@TotalQuantity", _objWO.TotalQuantity);
            cmd.Parameters.AddWithValue("@AuctionFromSite", _objWO.AuctionFromSite);
            cmd.Parameters.AddWithValue("@EnteredBy", _objWO.UserID);
            cmd.Parameters.AddWithValue("@UpdatedBy", _objWO.UserID);
            cmd.Parameters.AddWithValue("@IsActive", _objWO.IsActive);
            cmd.Parameters.AddWithValue("@Status", _objWO.Status);
            cmd.Parameters.AddWithValue("@CompletedMilestoneActivity", _objWO.CompletedMilestoneActivity);
            cmd.Parameters.AddWithValue("@TotalMaterialCost", _objWO.TotalCostMaterial);
            cmd.Parameters.AddWithValue("@TotalLabourCost", _objWO.TotalCostlabour);

            Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
            return chId;
        }

        /// <summary>
        /// SubmitMilestone
        /// </summary>
        /// <param name="WorkorderID"></param>
        /// <param name="dtActivity"></param>
        /// <returns></returns>
        public Int64 SubmitMilestone(string WorkorderID, DataTable dtActivity)
        {
            DALConn();
            SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_MilestoneActivity", Conn);
            cmd.CommandType = CommandType.StoredProcedure;
           // cmd.Parameters.AddWithValue("@ID",0);

            cmd.Parameters.AddWithValue("@WorkOrderID", WorkorderID);

            cmd.Parameters.AddWithValue("@MilestoneActivity", dtActivity);

            Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
            return chId;
        }
        /// <summary>
        /// Function for fetching  Projects applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectMicroPlanByVilageCode(string Village_Code)
        {
            try
            {

                DALConn(); SqlCommand cmd = new SqlCommand("SP_FDM_Select_MicroPlanByVilageCode", Conn);
                cmd.Parameters.AddWithValue("@Village_Code", Village_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching  All workorders for any micro plan
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectWorkOrderByMicroPlanID(string MicroPlanID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_WorkOrderByMicroPlanID", Conn);
                cmd.Parameters.AddWithValue("@MicroPlanID", MicroPlanID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// SelectSurvey
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns></returns>
        public DataTable SelectSurvey(string SurveyId)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_selectSurveyDetails", Conn);
                cmd.Parameters.AddWithValue("@SurveyId", SurveyId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// SelectMilestoneByActivitySubActivityID
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <param name="ActivityID"></param>
        /// <param name="SubActivityID"></param>
        /// <returns></returns>
        public DataTable SelectMilestoneByActivitySubActivityID(string WorkOrderID, string ActivityID, string SubActivityID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SelectMilestoneByActivitySubActivityID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.Parameters.AddWithValue("@SubActivityID", SubActivityID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// Function for fetching  All workorders for any micro plan
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectWorkOrderByDivisionCode(string Div_Code)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_WorkOrderByDivCode", Conn);
                cmd.Parameters.AddWithValue("@Div_Code", Div_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }


        /// <summary>
        /// Function for fetching  All workorders for any micro plan
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectWorkOrderByVillCode(string Vill_Code)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_WorkOrderByVillCode", Conn);
                cmd.Parameters.AddWithValue("@Vill_Code", Vill_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching  Projects applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectProjectByMicroplan(string MicroplanID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ProjectByMicroplan", Conn);
                cmd.Parameters.AddWithValue("@MicroplanID", MicroplanID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }  
       
        
        /// <summary>
        /// Function for fetching  Projects applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectBudgetHeadByProject(string ProjectIDs)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_BudgetHeadByProjectID", Conn);
                cmd.Parameters.AddWithValue("@ID", ProjectIDs);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching  Projects applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectBudgetHeadByScheme(string SchemeIDs)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_BudgetHeadBySchemeID", Conn);
                cmd.Parameters.AddWithValue("@ID", SchemeIDs);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching Model under a Project
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable SelectModelBySchemeID(string SchemeID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ModelBySchemeID", Conn);
                cmd.Parameters.AddWithValue("@ID", SchemeID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching Model under a WorkOrder
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable SelectModelByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ModelByWorkOrderID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        } 

        /// <summary>
        /// Function for fetching Model under a WorkOrder
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable SelectSurveyByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_SurveyByWorkOrderID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching Model under a WorkOrder
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable SelectActivityByWorkOrderID(string WorkOrderID,string ModelID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ActivityByWorkOrderID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.Parameters.AddWithValue("@ModelID", ModelID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }  
        
        /// <summary>
        /// Function for fetching Model under a WorkOrder
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <returns></returns>
        public DataTable SelectActivityBySurveyID(string WorkOrderID, string SurveyID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ActivityBySurveyID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.Parameters.AddWithValue("@SurveyID", SurveyID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching  Activity under a model
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>
        public DataTable SelectActivityByModelID(string ModelID, string DIV_Code)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ActivitybyModelID", Conn);
                cmd.Parameters.AddWithValue("@ID", ModelID);
                cmd.Parameters.AddWithValue("@DIV_Code", DIV_Code);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }
       
        /// <summary>
        /// Function for fetching  Activity under a model
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>
        public DataTable SelectActivityByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ActivityByWorkOrderID", Conn);
                if (WorkOrderID == "0")
                    WorkOrderID = HttpContext.Current.Session["WorkOrderID"].ToString();
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Function for fetching subActivity under Activity
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectSubActivityByActivityID(string ActivityID, string WorkOrder)
        {
            try
            {
                DALConn();
                //SqlCommand cmd = new SqlCommand("SP_FDM_Select_SubActivitybyActivityID", Conn);
                SqlCommand cmd = new SqlCommand("SP_Select_SubActivityByWorkOrderID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrder);
                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }
        /// <summary>
        /// Function for fetching subActivity under Activity
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectSubActivityByActivityIDWorkOrder(string ActivityID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_SubActivitybyActivityID", Conn);
                cmd.Parameters.AddWithValue("@ID", ActivityID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }
        /// <summary>
        /// Function for fetching subActivity under Activity
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectSubActivityByWorkOrderID(string WorkOrderID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_SubActivitybyWorkOrderID", Conn);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching ProduceType
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectProduceType()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ProduceType", Conn);
                // cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        } 
        /// <summary>
        /// Function for fetching ProduceType
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectAssetCategoryType()
        {
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("SELECT distinct AssetCategoryID,CategoryName from tbl_mst_FDM_AssetCategory where Isactive=1 order by CategoryName", Conn);
                // cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        } /// <summary>
        /// Function for fetching ProduceType
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectAssetbyCategoryType(string AssetCategoryID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SELECT ASSETID,ASSETNAME FROM tbl_mst_FDM_Assets where Isactive=1 and AssetCategoryID = " + AssetCategoryID + " order by ASSETNAME", Conn);
                // cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }
        /// <summary>
        /// Function for fetching ProduceType
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable GetActiveIFMSWorkOrder()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_IFMS_Select_ActiveWorkOrder", Conn);
                // cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Function for fetching Product with Type, Give produceType = 0 for all Product
        /// </summary>
        /// <param name="ActivityID"></param>
        /// <returns></returns>
        public DataTable SelectProductByProduceType(string ProduceTypeID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ProductByProduceType", Conn);
                cmd.Parameters.AddWithValue("@ProduceTypeID", ProduceTypeID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        public DataTable SelectUnitByProduceType(string ProduceTypeID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_UnitByProduct", Conn);
                cmd.Parameters.AddWithValue("@ProduceTypeId", ProduceTypeID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }


        /// <summary>
        /// SelectNurseryByVilageCode
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectNurseryByVilageCode(string Village_Code)         {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SelectNurseryByVilageCode", Conn);
                cmd.Parameters.AddWithValue("@VILL_CODE", Village_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Select_WorkOrder
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        public DataTable Select_WorkOrder(string WorkOrderID="0")
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_WorkOrder", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@WorkOrderID", WorkOrderID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Select_WorkOrderProgress
        /// </summary>
        /// <param name="WorkOrderProgressID"></param>
        /// <returns></returns>
        public DataTable Select_WorkOrderProgress(string WorkOrderProgressID = "0")
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_WorkOrderProgress", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@WorkOrderProgressID", WorkOrderProgressID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// GetAvailableProductbyVillage
        /// </summary>
        /// <param name="VillageCode"></param>
        /// <param name="produceTypeId"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public DataTable GetAvailableProductbyVillage(string VillageCode, string produceTypeId, string ProductID, string workOrderID)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FDM_GetAvailableProductbyVillagew", Conn);
                cmd.Parameters.AddWithValue("@workOrderID", workOrderID);
                cmd.Parameters.AddWithValue("@VillageCode", VillageCode);
                cmd.Parameters.AddWithValue("@ProduceTypeId", produceTypeId);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;

            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// getWorkOrderMicroplan
        /// </summary>
        /// <param name="WordOrderID"></param>
        /// <returns></returns>
        public DataTable getWorkOrderMicroplan(string WordOrderID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FDM_getWorkOrderMicroplan", Conn);
                cmd.Parameters.AddWithValue("@WordOrderID", WordOrderID);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// saveWorkOrderMilestone
        /// </summary>
        /// <param name="WOMil"></param>
        /// <returns></returns>
        public DataTable saveWorkOrderMilestone(WorkOrderMilestone WOMil)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FDM_Add_Update_WorkOrderMilestone", Conn);
                cmd.Parameters.AddWithValue("@WorkorderID", WOMil.WorkOrderID);
                cmd.Parameters.AddWithValue("@MilestoneName", WOMil.MilestoneName);
                cmd.Parameters.AddWithValue("@MilestonePaymentPercentage", WOMil.MilestonePaymentPercentage);
                cmd.Parameters.AddWithValue("@ActivitycompletionPercentage", WOMil.ActivitycompletionPercentage); //cmd.Parameters.AddWithValue("@ID", WOMil.ID);                

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// DeleteWorkOrderMilestone
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        public DataTable DeleteWorkOrderMilestone(string ID, string WorkOrderID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("update tbl_FDM_Workorder_Milestone set isActive=0 where ID="+ID+" AND ID not in(Select MilestoneID from tbl_fdm_MilestoneActivity)", Conn);
               // cmd.Parameters.AddWithValue("@WordOrderID", WordOrderID);                
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                return getWorkOrderMilestone(WorkOrderID);
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// getWorkOrderMilestone
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        public DataTable getWorkOrderMilestone(string WorkOrderID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_FDM_getWorkOrderMilestone", Conn);
                cmd.Parameters.AddWithValue("@WordOrderID", WorkOrderID);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// GetMilestoneActivity
        /// </summary>
        /// <param name="WorkOrderID"></param>
        /// <returns></returns>
        public DataTable GetMilestoneActivity(string WorkOrderID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Select ma.ID as rowID, ActivityID,A.Activity_Name as ActivityName,SubActivityID,sa.Sub_Activity_Name as SubActivityName,ma.PercentageActivitycompletion,wm.ID as MilestoneID,wm.WorkOrderID, wm.MilestoneName from tbl_FDM_MilestoneActivity ma left join tbl_FDM_Workorder_Milestone wm on ma.MilestoneID=wm.ID left join tbl_mst_FDM_Activity A on ma.ActivityID = A.ID left join tbl_mst_FDM_Sub_Activity sa on ma.SubActivityID = sa.ID where ma.WorkOrderID = " + WorkOrderID, Conn);
                cmd.Parameters.AddWithValue("@WordOrderID", WorkOrderID);                
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// DeleteWorkOrder
        /// </summary>
        /// <returns></returns>
        public Int64 DeleteWorkOrder()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_delete_WorkOrder", Conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// DeleteWorkOrderProgress
        /// </summary>
        /// <returns></returns>
        public Int64 DeleteWorkOrderProgress()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_delete_WorkOrderProgress", Conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                Int64 chk = Convert.ToInt64(cmd.ExecuteScalar());
                return chk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable Select_Activity_BSRAmount(string ActivityID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@ActivityID", ActivityID) };                
                Fill(dt, "select_Activity_BSR_Amount", parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataTable Select_Activity_BSR(string ActivityID, string SubActivityId, string Division)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@ActivityID", ActivityID), 
                                              new SqlParameter("@subActivityID", SubActivityId), 
                                             new SqlParameter("@Div_Code", Division) 
                                            };
                Fill(dt, "Select_Activity_BSR", parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable Select_Activity_CircleBSRAmount(string ActivityID, string Range_Code)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@ActivityID", ActivityID), new SqlParameter("@Range_Code", Range_Code) };
                Fill(dt, "select_Activity_CircleBSR_Amount", parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable Select_SubActivity_BSRAmount(string ActivityID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@SubActivityID", ActivityID) };
                Fill(dt, "select_SubActivity_BSR_Amount", parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataTable Select_SubActivity_CircleBSRAmount(string ActivityID, string Range_Code)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@SubActivityID", ActivityID), new SqlParameter("@Range_Code", Range_Code) };
                Fill(dt, "select_SubActivity_CircleBSR_Amount", parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public string Select_ProjectArea(string ProjectID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("select AreaofRolloutinSQKM from tbl_FDM_Scheme where ID= " + ProjectID, Conn);
               // cmd.Parameters.AddWithValue("@WordOrderID", WorkOrderID);
                cmd.CommandType = CommandType.Text;                
                return Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

    }


    public class WorkOrderMicroplan : DAL
    {
        public string rowID { get; set; }
        public string WorkorderID { get; set; }
        public string Village_Code { get; set; }
        public string Village_Name { get; set; }
        public string MicroPlanID { get; set; }
        public string MicroPlanName { get; set; }
        public string ProjectID { get; set; }
        public string SchemeID { get; set; }
        public string SchemeName { get; set; }
        public string ProjectName { get; set; }
        public string UnitName { get; set; }
        public string ModelIDs { get; set; }
        public string ModelName { get; set; }
        public string ActivityIDs { get; set; }
        public string ActivityName { get; set; }
        public string SubActivityId { get; set; }
        public string SubActivityName { get; set; }
        public double projectArea { get; set; }
        public double ActivityCost { get; set; }
        public double NoofActivities { get; set; }
        public string Quantity { get; set; }
        public string Division { get; set; }

        
    }

    public class WorkOrderMilestone
    {
        public string rowID { get; set; }
        public string WorkOrderID
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["WorkOrderID"]);
            }
            set { value = this.WorkOrderID; }
        }
     //    [Required(ErrorMessage = "Milestone name Required")]
        public string MilestoneName { get; set; }
      //   [Required(ErrorMessage = "Percentage Required")]
        public int MilestonePaymentPercentage { get; set; }
        public int ActivitycompletionPercentage { get; set; }
        public bool isCompleted { get; set; }
        public bool isBillRaised { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }  
    } 
    public class MilestoneActivity
    {
        public string rowID { get; set; }
        public string WorkOrderID
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["WorkOrderID"]);
            }
            set { value = this.WorkOrderID; }
        }
        public string MilestoneID {get;set;}
        public string MilestoneName { get; set; }
      public string  ActivityID{get;set;}
      public string ActivityName { get; set; }
      public string SubActivityID { get; set; }
      public string SubActivityName { get;set; }
      public string PercentageActivitycompletion { get; set; }
        public bool isCompleted { get; set; }
        public bool isBillRaised { get; set; }
             
    }
    public class WorkOrderSurvey
    {
        public long SurveyID { get; set; }
        public string SurveyDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PhotoURL { get; set; }
        public string ComplitionYear { get; set; }
        public string ActivityID { get; set; }
        public string Activity_Name { get; set; }
        public string ActivityPercentage { get; set; }
        public string AreaName { get; set; }
        public string Area { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Description { get; set; }
        public int Isactive { get; set; }
        public string EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public string UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public string Quantity { get; set; }
        public string Activity_Unit { get; set; }
        public string AllotedWork { get; set; }
    }

    public class IFMSVendor 
    {
        public string VendorCode { get; set; }
        public string name { get; set; }
        public string Address { get; set; }
    }
    public class IFMSWorkOrder
    {
        public string WorkOrderId{get;set;}
        public string WorkOrderNo { get; set; }
        public string WorkOrderName { get; set; }
        public string VendorCode { get; set; }
        public string WorkOrderAmount { get; set; }
    }
}
