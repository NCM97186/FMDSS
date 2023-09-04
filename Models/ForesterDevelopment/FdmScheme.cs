

// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 28-Dec-2015
// *****************************************************************************************
// <summary>This Model is Created for Add Program in Forest Development</summary>
// *****************************************************************************************

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FMDSS.Models.ForestDevelopment
{
    public class FdmScheme : DAL
    {
        /// <summary>
        /// Property declare
        /// </summary>
        public long ID { get; set; }
        public long SCHEME_ID { get; set; }
        public string Scheme_Name { get; set; }
        public Int64 Programme { get; set; }

        public string ProgramName { get; set; }

        public decimal AreaofRolloutinSQKM { get; set; }
        public string RefNoRelatedDocument { get; set; }
        public string RefDocument { get; set; }
        public string District { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime StartDate1 { get; set; }
        public DateTime EndDate1 { get; set; }
        public DateTime EnteredOn { get; set; }
        public long EnteredBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public int Status { get; set; }
        public string ImpAgency { get; set; }
        public string Keybeneficial { get; set; }
        public string Keyactivity { get; set; }
        public string Model_Code { get; set; }
        public string Budget_Head { get; set; }
        public string Administrative_Approval { get; set; }
        public string Administrative_Approval_Date { get; set; }
        public string Financial_Approval { get; set; }
        public string Financial_Approval_Date { get; set; }
        public string Administrative_Approval_Date1 { get; set; }
        public string Administrative_Approval_Document { get; set; }
        public string Financial_Approval_Date1 { get; set; }
        public string Financial_Approval_Document { get; set; }
        public decimal Budget { get; set; }
        public string StatusDesc { get; set; }

        /// <summary>
        /// Function to bind district
        /// </summary>
        /// <returns></returns>
        public DataSet BindDistrict()
        {
            DataSet dsDistrict = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                    {
                new SqlParameter("@option","1"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsDistrict, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDistrict" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsDistrict;
        }
        /// <summary>
        /// Used to bind model
        /// </summary>
        /// <returns></returns>
        public DataSet BindModel()
        {
            DataSet dsModel = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@option","2"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsModel, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsModel;
        }
        /// <summary>
        /// Function for final submission of data
        /// </summary>
        /// <returns></returns>
        public Int64 InsertScheme()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
            new SqlParameter("@ID",ID),
            new SqlParameter("@Scheme_Name", Scheme_Name),
            new SqlParameter("@AreaofRolloutinSQKM", AreaofRolloutinSQKM),
            new SqlParameter("@RefDocument", RefDocument),
            new SqlParameter("@RefNoDocument", RefNoRelatedDocument),
            new SqlParameter("@StartDate", StartDate1),
            new SqlParameter("@EndDate", EndDate1),
            new SqlParameter("@IAgency", ImpAgency),
            new SqlParameter("@Budget_Head", Budget_Head),
            new SqlParameter("@Budget", Budget),
            new SqlParameter("@Admin_Approval", Administrative_Approval),
            new SqlParameter("@Admin_Approval_Date", Administrative_Approval_Date),
            new SqlParameter("@Admin_Approval_Doc", Administrative_Approval_Document),
            new SqlParameter("@Financial_Approval", Financial_Approval),
            new SqlParameter("@Financial_Approval_Date",Financial_Approval_Date),
            new SqlParameter("@Financial_Approval_Doc",Financial_Approval_Document),
            new SqlParameter("@ProgramId", Programme),
            new SqlParameter("@Model_Code", Model_Code),
            new SqlParameter("@Dist_Code", District),
            new SqlParameter("@EnteredBy", EnteredBy),
            new SqlParameter("@UpdatedBy", EnteredBy),
            new SqlParameter("@IsActive", IsActive),
            new SqlParameter("@Status", Status),
            };
                chk = Convert.ToInt64(ExecuteScalar("SP_FDM_ADD_UPDATE_SCHEME", parameters));

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertScheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Insert Comma Seperated Scheme into another table 
        /// </summary>
        /// <returns></returns>
        public Int64 InsertSchemeDistrict()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {
            new SqlParameter("@SCHEME_ID",SCHEME_ID),
            new SqlParameter("@Values", District),
            };
                chk = Convert.ToInt64(ExecuteScalar("SP_InsertSchemeDistrict", parameters));

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertSchemeDistrict" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Function for bind Scheme
        /// </summary>
        /// <returns></returns>
        public DataSet BindScheme()
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@option","3"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsScheme, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindScheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }
        public DataSet BindImplementAgency()
        {
            DataSet dsIAgency = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@option","4"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsIAgency, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindProgram" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsIAgency;
        }
        /// <summary>
        /// Function foe bind program details
        /// </summary>
        /// <returns></returns>
        public DataSet BindProgram()
        {
            DataSet dsPrgm = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@option","5"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsPrgm, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindProgram" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsPrgm;
        }

        /// <summary>
        /// Function foe bind program details
        /// </summary>
        /// <returns></returns>
        public DataSet BindBudget()
        {
            DataSet dsBudget = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@option","6"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsBudget, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindBudget" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsBudget;
        }

        public DataSet EditScheme()
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
                {
                new SqlParameter("@option","7"),
                new SqlParameter("@SchemeId",ID),
                };
                Fill(dsScheme, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "EditScheme" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }


    }


    public class SchemeMapping
    {

        public int MappingId { get; set; }

        [Required]
        public string SchemeName { get; set; }
        public string ID { get; set; }
        [Required]
        public int BudgetHead { get; set; }
        public string BudgetHeadName { get; set; }
        [Required]
        public int SubBudgetHead { get; set; }
        public string SubBudgetHeadName { get; set; }
        [Required]
        public int Activity { get; set; }
        public string ActivityName { get; set; }
        [Required]
        public int SubActivity { get; set; }
        public string SubActivityName { get; set; }

        public string ISCircleDivision { get; set; }

        public string CIRCLE_CODE { get; set; }
        public string Division { get; set; }
        public string SanctuaryCode { get; set; }

        public string CIRCLEName { get; set; }
        public string DivisionName { get; set; }
        public string SanctuaryName { get; set; }

    }
    public class Scheme : SchemeMapping
    {

        public string Status { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
    }

    public class SchemeModel : Scheme
    {
        public SchemeModel()
        {
            List = new List<Scheme>();
            MappingList = new List<Scheme>();
        }
        public List<Scheme> List { get; set; }
        public List<Scheme> MappingList { get; set; }
    }

    public class SchemeRepo : DAL
    {
        public DataSet BindDistrict()
        {
            DataSet dsDistrict = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@option","1"),
                new SqlParameter("@SchemeId",Convert.ToInt64("0")),
                };
                Fill(dsDistrict, "SP_FDM_GetSchemeDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDistrict" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsDistrict;
        }

        public DataSet InsertScheme(SchemeModel model, string ActionName, long UserID)
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@Action",ActionName),
                new SqlParameter("@ID",Convert.ToInt64(model.ID)),
                 new SqlParameter("@Scheme_Name",model.SchemeName),
                    new SqlParameter("@StartDate",model.StartDate),
                   new SqlParameter("@EndDate",model.EndDate),
                    new SqlParameter("@IsActive",1),
                    new SqlParameter("@CreatedBy",UserID),
                    new SqlParameter("@District",model.DistrictCode)
                };
                Fill(dsScheme, "sp_NewSchemeForWideLife", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindScheme" + "_" + "SchemeRepo", 4, DateTime.Now, UserID);

            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }

        public DataSet InsertSchemeMapping(SchemeMapping model, string ActionName, long UserID, string ApprovedIds)
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {
                new SqlParameter("@Action",ActionName),
                new SqlParameter("@MappingID",Convert.ToInt64(model.MappingId)),
                 new SqlParameter("@SchemeID",model.ID),
                    new SqlParameter("@BudgetHeadID",model.BudgetHead),
                   new SqlParameter("@SubBudgetHeadID",model.SubBudgetHead),
                    new SqlParameter("@ActivityID",model.Activity),
                    new SqlParameter("@SubActivityID",model.SubActivity),

                     new SqlParameter("@ISCircleDivision",model.ISCircleDivision),
                   new SqlParameter("@CIRCLE_CODE",model.CIRCLE_CODE),
                    new SqlParameter("@Division",model.Division),
                    new SqlParameter("@SanctuaryCode",model.SanctuaryCode),

                    new SqlParameter("@Status",0),
                    new SqlParameter("@CreatedBy",UserID),
                    new SqlParameter("@ApprovedIds",ApprovedIds)
                };
                Fill(dsScheme, "SP_FMD_SchemeForWidelifeMapping", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindScheme" + "_" + "SchemeRepo", 4, DateTime.Now, UserID);

            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }
    }
}