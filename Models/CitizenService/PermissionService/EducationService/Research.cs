//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Apply Research Permission
//  Date Created : 18-Nov-2015
//  History      :
//  Version      : 1.0
//  Author       : Arvind Srivastava
//  Modified By  :
//  Modified On  :
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@


using FMDSS.Entity;
using FMDSS.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.Models.CitizenService.ProductionServices.EducationService
{

    /// <summary>
    /// Model contain public property and methods
    /// </summary>
    public class Research : DAL
    {
        #region Global Variables
        public long ResearchID { get; set; }
        public string RequestedId { get; set; }
        public string ResearchType { get; set; }
        [Required]
        public int ApplicationType { get; set; }
        public bool IsFundingAvailable { get; set; }
        public string SourceOfFunding { get; set; }
        public string FatherName { get; set; }
        [Required]
        public int QualificationID { get; set; }
        [Required]
        public string CollegeName { get; set; }
        [Required]
        public string TitleOfResearch { get; set; }
        [Required]
        public string Procedure { get; set; }
        [Required]
        public string DurationFrom { get; set; }
        [Required]
        public string DurationTo { get; set; }
        public string ResearchCategories { get; set; }
        [Required]
        [Display(Name = "IUCN Categories")]
        public int IUCNCategoryID { get; set; }
        [Display(Name = "Schedule As per wildlife protection act 1972")]
        public string ScheduleCategories { get; set; }
        public long ScheduleSpeciesID { get; set; }
        public string Category { get; set; }
        [Required]
        public string AreaCategory { get; set; }
        public long PlaceID { get; set; }
        [Required]
        public long PlaceForResearch { get; set; }
        [Required]
        public string PlaceForResearch_Other { get; set; }
        public string DeliverablesToWildlifeDepartment { get; set; }
        public int trn_status_code { get; set; }
        public string TransactionId { get; set; }
        public string KioskUserId { get; set; }
        [Required]
        public string Synopsis_Name { get; set; }
        public string Synopsis_Path { get; set; }
        [Required]
        public string Presentation_Name { get; set; }
        public string Presentation_Path { get; set; }
        [Required]
        public string CoordinatorName { get; set; }
        [Required]
        public string CoordinatorAddress { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectDescriptionName { get; set; }
        public string ProjectDescriptionPath { get; set; }
        public string Area_Category { get; set; }
        public string PresentationDate { get; set; }
        public string PresentationStatus { get; set; }
        public string PermissionFile { get; set; }
        public bool IsUseOfVehicle { get; set; }
        public bool IsSurvey { get; set; }
        public bool IsSampleCollection { get; set; }
        public bool IsFilmAndShooting { get; set; }
        public bool IsHandyCamera { get; set; }
        public bool IsDrone { get; set; }
        public bool IsSpecimenToBeTakenOutOfIndia { get; set; }
        public long UserID { get; set; }
        public int Status { get; set; }
        public string SpecifyGroup { get; set; }
        public string ApplicationFilePath { get; set; }
        [Required]
        public string Duration { get; set; }
        public virtual List<SpecimenDetailsModel> SpecimenDetailsList { get; set; }
        public virtual List<SampleDetailsModel> SampleDetailsList { get; set; }
        public virtual CommonRequestData CommonRequestData { get; set; }
        #endregion


        public DataSet Get_Animal_Plant_Category(string RCategory, string Category, string LocationId, string dataType)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_FetchResearchSpecies", Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpecieType", RCategory);
                cmd.Parameters.AddWithValue("@PlaceCat", Category);
                cmd.Parameters.AddWithValue("@PlaceId", Convert.ToInt64(LocationId));
                cmd.Parameters.AddWithValue("@DataType", dataType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_Animal_Plant_Category" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        private string GetRequestInXML(Research model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ResearchRequest>");

            if (model.SpecimenDetailsList != null)
            {
                sb.Append("<Specimens>");
                foreach (var item in model.SpecimenDetailsList)
                {
                    sb.Append("<Specimen>");
                    sb.Append(string.Format(@" 
                            <SpecimenDetailsID>{0}</SpecimenDetailsID>
                            <MemberType>{1}</MemberType>
                            <MemberName>{2}</MemberName>
                            <Gender>{3}</Gender>", item.SpecimenDetailsID, item.MemberType, item.MemberName, item.Gender));
                    sb.Append("</Specimen>");
                }
                sb.Append("</Specimens>");
            }

            if (model.SampleDetailsList != null)
            {
                sb.Append("<Samples>");
                foreach (var item in model.SampleDetailsList)
                {
                    sb.Append("<Sample>");
                    sb.Append(string.Format(@" 
                            <SampleDetailsID>{0}</SampleDetailsID>
                            <Location>{1}</Location>
                            <MaterialName>{2}</MaterialName>
                            <Qty>{3}</Qty>", item.SampleDetailsID, item.Location, item.MaterialName, item.Qty));
                    sb.Append("</Sample>");
                }
                sb.Append("</Samples>");
            }

            sb.Append("</ResearchRequest>");
            return Convert.ToString(sb);
        }

        /// <summary>
        /// save Research Data into Database
        /// </summary>
        /// <param name="research"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public ResponseMsg addResearch(Research research)
        {
            DataTable dtData = new DataTable();
            ResponseMsg msg = null;
            try
            {
                var actionCode = research.ResearchID == 0 ? 1 : 2;
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_AddResearchStudy", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("ActionCode", actionCode);
                cmd.Parameters.AddWithValue("ResearchID", research.ResearchID);
                cmd.Parameters.AddWithValue("RequestedId", research.RequestedId);
                cmd.Parameters.AddWithValue("ResearchType", research.ResearchType);
                cmd.Parameters.AddWithValue("ApplicationType", research.ApplicationType);
                cmd.Parameters.AddWithValue("IsFundingAvailable", research.IsFundingAvailable);
                cmd.Parameters.AddWithValue("SourceOfFunding", research.SourceOfFunding);
                cmd.Parameters.AddWithValue("FatherName", research.FatherName);
                cmd.Parameters.AddWithValue("QualificationID", research.QualificationID);
                cmd.Parameters.AddWithValue("CollegeName", research.CollegeName);
                cmd.Parameters.AddWithValue("TitleOfResearch", research.TitleOfResearch);
                cmd.Parameters.AddWithValue("Procedure", research.Procedure);
                cmd.Parameters.AddWithValue("DurationFrom", Globals.Util.GetDate(research.DurationFrom));
                cmd.Parameters.AddWithValue("DurationTo", Globals.Util.GetDate(research.DurationTo));
                cmd.Parameters.AddWithValue("ResearchCategories", research.ResearchCategories);
                cmd.Parameters.AddWithValue("IUCNCategoryID", research.IUCNCategoryID);
                cmd.Parameters.AddWithValue("ScheduleCategories", research.ScheduleCategories);
                cmd.Parameters.AddWithValue("Category", research.Category);
                cmd.Parameters.AddWithValue("AreaCategory", research.AreaCategory);
                cmd.Parameters.AddWithValue("PlaceID", research.PlaceID);
                cmd.Parameters.AddWithValue("PlaceForResearch", research.PlaceForResearch);
                cmd.Parameters.AddWithValue("PlaceForResearch_Other", research.PlaceForResearch_Other);
                cmd.Parameters.AddWithValue("DeliverablesToWildlifeDepartment", research.DeliverablesToWildlifeDepartment);
                cmd.Parameters.AddWithValue("KioskUserId", research.KioskUserId);
                cmd.Parameters.AddWithValue("Synopsis_Name", research.Synopsis_Name);
                cmd.Parameters.AddWithValue("Synopsis_Path", research.Synopsis_Path);
                cmd.Parameters.AddWithValue("Presentation_Name", research.Presentation_Name);
                cmd.Parameters.AddWithValue("Presentation_Path", research.Presentation_Path);
                cmd.Parameters.AddWithValue("CoordinatorName", research.CoordinatorName);
                cmd.Parameters.AddWithValue("CoordinatorAddress", research.CoordinatorAddress);
                cmd.Parameters.AddWithValue("ProjectDescription", research.ProjectDescription);
                cmd.Parameters.AddWithValue("ProjectDescriptionPath", research.ProjectDescriptionName);
                cmd.Parameters.AddWithValue("IsUseOfVehicle", research.IsUseOfVehicle);
                cmd.Parameters.AddWithValue("IsSurvey", research.IsSurvey);
                cmd.Parameters.AddWithValue("IsSampleCollection", research.IsSampleCollection);
                cmd.Parameters.AddWithValue("IsFilmAndShooting", research.IsFilmAndShooting);
                cmd.Parameters.AddWithValue("IsHandyCamera", research.IsHandyCamera);
                cmd.Parameters.AddWithValue("IsDrone", research.IsDrone);
                cmd.Parameters.AddWithValue("Status", research.Status);
                cmd.Parameters.AddWithValue("xmlFile", GetRequestInXML(research));
                cmd.Parameters.AddWithValue("UserID", research.UserID);
                cmd.Parameters.AddWithValue("Duration", research.Duration);
                cmd.Parameters.AddWithValue("ApplicationFilePath", research.ApplicationFilePath);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtData);

                if (dtData != null)
                {
                    msg = dtData.AsEnumerable().Select(x => new ResponseMsg
                    {
                        IsError = x.Field<bool>("IsError"),
                        ReturnMsg = x.Field<string>("ReturnMsg"),
                        ReturnIDs = x.Field<string>("ReturnIDs")
                    }).FirstOrDefault();


                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "addResearch" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return msg;
        }
        public string EditResearch(Research research)
        {
            string Result = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SpEdit_ResearchStudy", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Applicant_Type", research.ApplicantType);
                //cmd.Parameters.AddWithValue("@RequestedId", research.RequestedId);
                //cmd.Parameters.AddWithValue("@FathersName", research.FatherName);
                //cmd.Parameters.AddWithValue("@Qualification", research.Qualification);
                //cmd.Parameters.AddWithValue("@College", research.College);
                //cmd.Parameters.AddWithValue("@R_Subject", research.Subject);
                //cmd.Parameters.AddWithValue("@R_Procedure", research.Procedure);
                //cmd.Parameters.AddWithValue("@DurationFrom", research.DurationFrom);
                //cmd.Parameters.AddWithValue("@DurationTo", research.DurationTo);
                //cmd.Parameters.AddWithValue("@Location", research.Location);
                //cmd.Parameters.AddWithValue("@Research_Type", research.ResearchType);
                //cmd.Parameters.AddWithValue("@Animal_cat", research.Animal_cat);
                //cmd.Parameters.AddWithValue("@Animal_Name", research.AnimalName);
                //cmd.Parameters.AddWithValue("@Animal_Sno", research.Animal_Sno);

                //cmd.Parameters.AddWithValue("@Species_cat", research.Species_cat);
                //cmd.Parameters.AddWithValue("@Species_Name", research.Species_Name);
                ////cmd.Parameters.AddWithValue("@Species_Sno", research.Species_Sno);
                //cmd.Parameters.AddWithValue("@R_Benefits", research.R_Benefits);
                //cmd.Parameters.AddWithValue("@C_ID", research.C_ID);
                //cmd.Parameters.AddWithValue("@UserID", research.UserID);
                //cmd.Parameters.AddWithValue("@Research_Status", research.Research_Status);

                //cmd.Parameters.AddWithValue("@Synopsis_Name", research.ResSynopsisName);
                //cmd.Parameters.AddWithValue("@Presentation_Name", research.ResPresentationName);
                //cmd.Parameters.AddWithValue("@ProjectDescription", research.ProjectDescription);

                //cmd.Parameters.AddWithValue("@Assist_Name", research.txt_AssistName);
                //cmd.Parameters.AddWithValue("@Assist_IdType", research.ddl_AssistIDType);
                //cmd.Parameters.AddWithValue("@Assist_IdNo", research.txt_AssistIDno);
                //cmd.Parameters.AddWithValue("@Assist_IdProof_Name", research.AssistIDProofName);

                //cmd.Parameters.AddWithValue("@Vehicle_TypeId", research.Vehiclecat);
                //cmd.Parameters.AddWithValue("@Vehicle_Id", research.ddl_vehicle);
                //cmd.Parameters.AddWithValue("@ResearchType", research.ResearchType);
                //cmd.Parameters.AddWithValue("@Area_Category", research.AreaCategory);

                Result = cmd.ExecuteScalar().ToString();


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "addResearch" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return Result;
        }

        /// <summary>
        /// used for bind Research Detail
        /// </summary>
        /// <returns></returns>
        public DataTable BindResearchStudy()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_GetResearchInfo", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "BindResearchStudy" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataSet GetResearchStudyData(string requestID)
        {
            DataSet dsReq = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SPGetResearchReqForEdit", Conn);
                cmd.Parameters.AddWithValue("ActionCode", 1);
                cmd.Parameters.AddWithValue("RequestId", requestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsReq);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetResearchStudyData" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dsReq;
        }

        /// <summary>
        /// used for get Past Research Activities
        /// </summary>
        /// <param name="researchId"></param>
        /// <returns></returns>
        public DataTable GetResearchDetail(string researchId)
        {
            DataTable dt = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen__Select_ResearchInfo_ByReserchid", Conn);
                cmd.Parameters.AddWithValue("@researchId", researchId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetResearchDetail" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetResearchValues(string RequestedID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_GetRecord_ResearchStudy", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedID", RequestedID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetResearchValues" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetCoordinatorName()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_CoordinatorName", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.Message, "GetCoordinatorName" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// user for Coordinator details using by coordinatorId
        /// </summary>
        /// <param name="coordinatorId"></param>
        /// <returns></returns>
        public DataTable GetCoordinatorDetail(string coordinatorId)
        {
            DataTable dt = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_Coordinator_ById", Conn);
                cmd.Parameters.AddWithValue("@P_coordinatorId", coordinatorId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

                new Common().ErrorLog(ex.Message, "GetCoordinatorDetail" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Used for send the message to reviewer
        /// </summary>
        /// <returns></returns>
        public DataSet FindReviewer()
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Citizen_FindReviewer", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@ModuleId", ModuleId);
                //cmd.Parameters.AddWithValue("@ServiceTypeId", ServiceTypeId);
                //cmd.Parameters.AddWithValue("@PermissionId", PermissionId);
                //cmd.Parameters.AddWithValue("@SubPermissionId", SubPermissionId);
                //cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(HttpContext.Current.Session["UserId"]));
                //cmd.Parameters.AddWithValue("@Role", PermissionId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "FindReviewer" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable Get_CategorywiseDistrict(string category, string researchType)
        {
            DataTable dt = new DataTable();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_GetCategorywiseDistrict", Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@researchType", researchType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_CategorywiseDistrict" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }


        //public DataTable Get_Animal_Plant_Category(string RCategory, string Category, string LocationId, string dataType)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Sp_Citizen_FetchResearchSpecies", Conn);

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpecieType", RCategory);
        //        cmd.Parameters.AddWithValue("@PlaceCat", Category);
        //        cmd.Parameters.AddWithValue("@PlaceId", Convert.ToInt64(LocationId));
        //        cmd.Parameters.AddWithValue("@DataType", dataType);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);

        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, "Get_Animal_Plant_Category" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return dt;
        //}

        /// <summary>
        /// error log :Get_Animal_Plant_Category_2
        /// </summary>
        /// <param name="RCategory"></param>
        /// <param name="Category"></param>
        /// <param name="District"></param>
        /// <param name="LocationId"></param>
        /// <param name="spCategory"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public DataTable Get_Animal_Plant_Category(string RCategory, string Category, string LocationId, string spCategory, string dataType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_FetchResearchSpecies", Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpecieType", RCategory);
                cmd.Parameters.AddWithValue("@PlaceCat", Category);
                cmd.Parameters.AddWithValue("@PlaceId", Convert.ToInt64(LocationId));
                cmd.Parameters.AddWithValue("@SpecieCat", spCategory);
                cmd.Parameters.AddWithValue("@DataType", dataType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_Animal_Plant_Category_2" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetResearchDropDownData(string parentID, int actionCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Research_GetDropdownData", Conn);
                cmd.Parameters.AddWithValue("ActionCode", actionCode);
                cmd.Parameters.AddWithValue("ParentID", parentID);
                cmd.Parameters.AddWithValue("UserID", HttpContext.Current.Session["UserId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetCoordinatorDetail" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
    }

    public class SpecimenDetailsModel
    {
        public long SpecimenDetailsID { get; set; }
        public long ResearchID { get; set; }
        public string MemberType { get; set; }
        public string MemberName { get; set; }
        public string Gender { get; set; }
      
    }

    public class SampleDetailsModel
    {
        public long SampleDetailsID { get; set; }
        public long ResearchID { get; set; }
        public string Location { get; set; }
        public string MaterialName { get; set; }
        public decimal Qty { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }

    public class ResearchDetail : DAL
    {
        public Int64 Id { get; set; }
        public string ResearchId { get; set; }
        public string Activity_TakenBy { get; set; }
        public string Subjectforresearch { get; set; }
        public string Durationfrom { get; set; }
        public string Durationto { get; set; }
        public string ResearchLocation { get; set; }
        public string ResearchBenefits { get; set; }
        public string ResearchPurpose { get; set; }
        public string ResearchWildlife { get; set; }
    }




    public class Coordinator : DAL
    {
        public Int64 CoordinatorId { get; set; }
        public string CoordinatorName { get; set; }
        public string ContactNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
    }

}