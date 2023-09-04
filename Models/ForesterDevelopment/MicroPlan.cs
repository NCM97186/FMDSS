//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : DM-Model 
//  Description  : This file is responsible for all data and business rule for to Microplan
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
    public class MicroPlan: DAL
    {
        #region Class Parameter
        private Int64 _UserID;
              
        public Int64 UserID {
            get{
                return  Convert.ToInt64(HttpContext.Current.Session["UserId"]);} 
            set { _UserID = value; }
        }
        public long ID { get; set; }
        [Required(ErrorMessage = "Please Enter Start Date")]
        public string StartDate { get; set; }
                [Required(ErrorMessage = "Please Enter End Date")]
        public string EndDate { get; set; }
                [Required(ErrorMessage = "Please Enter Date of submission")]
        public string DateofRequest { get; set; }
        [Required(ErrorMessage="Please Enter Micro Plan Name")]
       // [RegularExpression("^([a-zA-Z]+[a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Micro Plan name")]
        public string MicroPlanName { get; set; }

        public string Village_Code { get; set; }
        public string ForestOfficerEmpId { get; set; }

        public string MPCode { get; set; }
        public string NGOorSHO { get; set; }
        public string NGOorSHOOfficerEmpId { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Gram Panchayat name")]
        [Required(ErrorMessage = "Please Enter Panchayat name")]
        public string GramPanchayat { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Revenue Village name")]
        [Required(ErrorMessage = "Please Enter Revenue Village name")]
        public string RevenueVillage { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Panchayat Comittee name")]
        [Required(ErrorMessage = "Please Enter Panchayat Comittee name")]
        public string PanchayatComittee { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Forest Admin Unit name")]
        //[Required(ErrorMessage = "Please Enter Admin Unit name")]       
        public string ForestAdminUnit { get; set; }
        //[RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        public decimal Totalarea{get;set;}
        public string ProjectIDs { get; set; }
        public string SchemeIDs { get; set; }
          [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Forest Range Office name")]
          [Required(ErrorMessage = "Please Enter Range Office name")]
        public string RangeOfficeUnit { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal TotalLandAreaSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal TotalForestAreaSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal ReservedForestSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal ProtectedForestSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal UnclassifiedForestSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal ClassifiedForestSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal FullyCoveredSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal WithoutPlantSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal AllocateforPlantSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal PanchayatLandSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal RevenueLandSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal AgricultureLand { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal IrregatedLandSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal NonIrregatedLandSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal ResidentialAreaSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal RemAgricultureLandSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public decimal RemNonAgricultureLandSQKM { get; set; }
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]?[0-9]?[0-9]", ErrorMessage = "Number required.")]
        [Range(0, 9999.99, ErrorMessage = "Value must be between 0 - 9,999.99")]
        public string Other { get; set; }
       
        public string JFMCorContractAgency { get; set; }
        public string JFMCorContractAgencyName {get; set;}
        public string EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public string UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public int Status { get; set; }
        public string RowID { get; set; }
       
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }       
        public string RANGE_CODE { get; set; }
        public string RANGE_NAME { get; set; }      
        public string DIST_CODE { get; set; }
        public string DIST_NAME { get; set; }
        public string BLK_CODE { get; set; }
        public string BLK_NAME { get; set; }
        public string GP_CODE { get; set; }   
        public string GP_NAME { get; set; }
        public string VILL_CODE { get; set; }
        public string hdnVillageCode { get; set; }
        public string VILL_NAME { get; set; }      
        public string StatusDesc { get; set; }


        public string RefGisID { get; set; }


        #endregion Class Parameter
        /// <summary>
        /// to Save Microplan
        /// </summary>
        /// <param name="_objMP"></param>
        /// <param name="dtEducation"></param>
        /// <param name="dtCast"></param>
        /// <returns></returns>
        public Int64 SubmitMicroPlan(MicroPlan _objMP, DataTable dtEducation, DataTable dtCast, DataTable dtCattle)
        {
            Int64 chId = 0;

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_MICROPLAN", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objMP.ID);
                cmd.Parameters.AddWithValue("@Div_Code", _objMP.DIV_CODE);
                cmd.Parameters.AddWithValue("@MicroPlanName", _objMP.MicroPlanName);
                cmd.Parameters.AddWithValue("@StartDate",DateTime.ParseExact( _objMP.StartDate,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@EndDate",DateTime.ParseExact( _objMP.EndDate,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@DateofRequest",DateTime.ParseExact( _objMP.DateofRequest,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@Village_Code", _objMP.hdnVillageCode);
                cmd.Parameters.AddWithValue("@ForestOfficerEmpId", _objMP.ForestOfficerEmpId);
                cmd.Parameters.AddWithValue("@NGOorSHO", _objMP.NGOorSHO);
                cmd.Parameters.AddWithValue("@NGOorSHOOfficerEmpId", _objMP.NGOorSHOOfficerEmpId);
                cmd.Parameters.AddWithValue("@GramPanchayat", _objMP.GramPanchayat);
                cmd.Parameters.AddWithValue("@RevenueVillage", _objMP.RevenueVillage);
                cmd.Parameters.AddWithValue("@PanchayatComittee", _objMP.PanchayatComittee);
                cmd.Parameters.AddWithValue("@ForestAdminUnit", _objMP.ForestAdminUnit);
                cmd.Parameters.AddWithValue("@RangeOfficeUnit", _objMP.RangeOfficeUnit);
                cmd.Parameters.AddWithValue("@Totalarea", _objMP.Totalarea);
                cmd.Parameters.AddWithValue("@TotalLandAreaSQKM", _objMP.TotalLandAreaSQKM);
                cmd.Parameters.AddWithValue("@TotalForestAreaSQKM", _objMP.TotalForestAreaSQKM);
                cmd.Parameters.AddWithValue("@ReservedForestSQKM", _objMP.ReservedForestSQKM);
                cmd.Parameters.AddWithValue("@ProtectedForestSQKM", _objMP.ProtectedForestSQKM);
                cmd.Parameters.AddWithValue("@UnclassifiedForestSQKM", _objMP.UnclassifiedForestSQKM);
                cmd.Parameters.AddWithValue("@ClassifiedForestSQKM", _objMP.ClassifiedForestSQKM);
                cmd.Parameters.AddWithValue("@FullyCoveredSQKM", _objMP.FullyCoveredSQKM);
                cmd.Parameters.AddWithValue("@WithoutPlantSQKM", _objMP.WithoutPlantSQKM);
                cmd.Parameters.AddWithValue("@AllocateforPlantSQKM", _objMP.AllocateforPlantSQKM);
                cmd.Parameters.AddWithValue("@PanchayatLandSQKM", _objMP.PanchayatLandSQKM);
                cmd.Parameters.AddWithValue("@RevenueLandSQKM", _objMP.RevenueLandSQKM);
                cmd.Parameters.AddWithValue("@AgricultureLand", _objMP.AgricultureLand);
                cmd.Parameters.AddWithValue("@IrregatedLandSQKM", _objMP.IrregatedLandSQKM);
                cmd.Parameters.AddWithValue("@NonIrregatedLandSQKM", _objMP.NonIrregatedLandSQKM);
                cmd.Parameters.AddWithValue("@ResidentialAreaSQKM", _objMP.ResidentialAreaSQKM);
                cmd.Parameters.AddWithValue("@RemAgricultureLandSQKM", _objMP.RemAgricultureLandSQKM);
                cmd.Parameters.AddWithValue("@RemNonAgricultureLandSQKM", _objMP.RemNonAgricultureLandSQKM);
                cmd.Parameters.AddWithValue("@Other", _objMP.Other);
                cmd.Parameters.AddWithValue("@SchemeIDs", _objMP.SchemeIDs);
                cmd.Parameters.AddWithValue("@JFMCorContractAgency", _objMP.JFMCorContractAgency);
                cmd.Parameters.AddWithValue("@MicroPlanEducationDetail", dtEducation);
                cmd.Parameters.AddWithValue("@MicroPlanCattleDetail", dtCattle);
                cmd.Parameters.AddWithValue("@MicroPlanPopulationDetail", dtCast);
                cmd.Parameters.AddWithValue("@EnteredBy", _objMP.UserID);
                cmd.Parameters.AddWithValue("@UpdatedBy", _objMP.UserID);
                cmd.Parameters.AddWithValue("@IsActive", _objMP.IsActive);
                cmd.Parameters.AddWithValue("@Status", _objMP.Status);
                cmd.Parameters.AddWithValue("@RefGisID", _objMP.RefGisID);   // Added By Arvind Kumar Sharam for save GIS Ref ID 
                  chId = Convert.ToInt64(cmd.ExecuteScalar());
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitMicroPlan" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }
        /// <summary>
        /// Selecting microplan
        /// </summary>
        /// <returns></returns>
        public DataTable Select_MicroPlan()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_MicroPlan", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_MicroPlan" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Select single microplan
        /// </summary>
        /// <param name="MicroplanID"></param>
        /// <returns></returns>
        public DataTable Select_MicroPlan(string MicroplanID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_MicroPlan", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@MicroplanID", MicroplanID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "Select_MicroPlan" + "_" + "BYID_Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Get Education
        /// </summary>
        /// <returns></returns>
        public DataTable GetEducation()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_Education", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
            
                da.Fill(dt);
               
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetEducation" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// GetSocialCast
        /// </summary>
        /// <returns></returns>
        public DataTable GetSocialCast()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_Cast", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
              
                da.Fill(dt);
              
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetSocialCast" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetFORESTOFFICERS(string Village_Code, string FORESTDesignationID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTFORESTOFFICERS", Conn);
                cmd.Parameters.AddWithValue("@VillageCode", Village_Code);
                cmd.Parameters.AddWithValue("FORESTDesignationID", FORESTDesignationID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                da.Fill(dt);
              
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFORESTOFFICERS" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        } 
        /// <summary>
        /// GetFORESTOFFICERS
        /// </summary>
        /// <param name="div_code"></param>
        /// <returns></returns>
        public DataTable GetFORESTOFFICERS(string div_code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTFORESTOFFICERSbyDivCode", Conn);
                cmd.Parameters.AddWithValue("@div_code", div_code);
               // cmd.Parameters.AddWithValue("FORESTDesignationID", FORESTDesignationID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                da.Fill(dt);
               
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFORESTOFFICERS" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
          
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Function for fetching  Projects applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectProjectByDist_Code(string Dist_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ProjectByDist_Code", Conn);
                cmd.Parameters.AddWithValue("@Dist_Code", Dist_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
             
                da.Fill(dt);
               
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SelectProjectByDist_Code" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Scheme applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectSchemeByDist_Code(string Dist_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_SchemeByDist_Code", Conn);
                cmd.Parameters.AddWithValue("@Dist_Code", Dist_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_FDM_Select_SchemeByDist_Code" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function for fetching  Projects applied on a vilage from database
        /// </summary>
        /// <param name="Village_Code"></param>
        /// <returns></returns>
        public DataTable SelectCattleByMicroplan(string MicroplanID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_CattleByMicroPlanCode", Conn);
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
        /// Function for fetching  Designation from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetFORESTDesignation()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTFORESTDesignation", Conn);
               // cmd.Parameters.AddWithValue("@VillageCode", Village_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(dt);
              
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetFORESTDesignation" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetNONGOVTOFFICERS(string Type,string Village_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTNONGOVTOFFICERS", Conn);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@VillageCode", Village_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(dt);
              
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetNONGOVTOFFICERS" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        } /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetJFMC(string Village_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTJFMC", Conn);                
                cmd.Parameters.AddWithValue("@VillageCode", Village_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(dt);
              
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetJFMC" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetJFMCbyDivCode(string Div_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTJFMCbyDiv_Code", Conn);
                cmd.Parameters.AddWithValue("@Div_Code", Div_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
            
                da.Fill(dt);
               
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetJFMCbyDivCode" + "_" + "BYDIVID_Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetGetHierarchybyVillageCode(string Vill_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_GetHierarchybyVillageCode", Conn);
                cmd.Parameters.AddWithValue("@Vill_Code", Vill_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
              
                da.Fill(dt);
               
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetGetHierarchybyVillageCode" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }/// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        /// <summary>
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetContrator(string Village_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTContractor", Conn);                
                cmd.Parameters.AddWithValue("@VillageCode", Village_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
              
                da.Fill(dt);
              
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetContrator" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        } 
        /// <summary>
        /// GetContratorbyDivCode
        /// </summary>
        /// <param name="Div_Code"></param>
        /// <returns></returns>
        public DataTable GetContratorbyDivCode(string Div_Code)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTContractorbyDiv_Code", Conn);
                cmd.Parameters.AddWithValue("@Div_Code", Div_Code);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
               
                da.Fill(dt);
               
            }

            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetContratorbyDivCode" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// DeleteMicroPlan
        /// </summary>
        /// <returns></returns>
        public Int64 DeleteMicroPlan()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_delete_MicroPlan", Conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
         
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DeleteMicroPlan" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
    }
    public class ForestEmployees
    {
        public string ROWID { get; set; }
       public string EmpId{get;set;}
          public string EmpDesignation{get;set;}

public string EmpName{get;set;}
public string Desig_Name{get;set;}
public string SSO_ID { get; set; }
public string OfficeName { get; set; }
    }

    public class literacy_level
    { 
        public string FEMALE{get;set;}
                 public string EDUCATION_DESC_ENG { get; set; }
           public string MALE{get;set;}
        public string COUNT
        {
            get
            {
                return Convert.ToString(Convert.ToInt64(FEMALE)+Convert.ToInt64(MALE));
            }
            set { _COUNT = value; }
        }
        private string _COUNT;
    }
    public class cast_category
    {
        public string FAMILIES {get;set;}       
        public string FEMALE { get; set; }      
        public string CHILD_FEMALE { get; set; }
       
        public string CATEGORY_DESC_ENG { get; set; }
        public string MALE { get; set; }
        public string CHILD_MALE { get; set; }
        public string COUNT
        {
            get
            {
                return Convert.ToString(Convert.ToInt64(FEMALE) + Convert.ToInt64(MALE));
            }
            set { _COUNT = value; }
        }
        public string CHILD_COUNT
        {
            get
            {
                return Convert.ToString(Convert.ToInt64(CHILD_FEMALE) + Convert.ToInt64(CHILD_MALE));
            }
            set { _CHILD_COUNT = value; }
        }

        private string _COUNT;
        private string _CHILD_COUNT;
    }
    public class lncome_level
    { 
        public string COUNT {get;set;}
        public string INCOME_DESC_ENG { get; set; }
    }
    public class bhamashahdata
    {
       public string FAMILIES {get;set;}
        public string NO_OF_HOUSEHOLD {get; set;}
        public string FEMALE {get;set;}
        public string CHILD_FEMALE {get; set;}

        public string CHILD_MALE {get; set;}
        public string MALE {get; set;}
        public List<literacy_level> literacy_level {get;set;}
        public List<cast_category> cast_category {get;set;}

        public List<lncome_level> lncome_level {get;set;}
    }



    public class GISactivityData
    {
        public string village_NM { get; set; }
        public double DrawArea { get; set; }
        public double DrawLength { get; set; }
        public string Cordinates { get; set; }
        public string refGisId { get; set; }

    }


    public class GISpostbackData
    {
        public string DistrictID { get; set; }
        public string BlocknameID { get; set; }
        public string GPNameID { get; set; }
        public string RangeID { get; set; }
        public string VillageID { get; set; }

    }

    public class MicroplanCattleDetail
    {
        public string CattleType { get; set; }
        public string CattleName { get; set; }
        public string CattleCount { get; set; }
     

    }


}