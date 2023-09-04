using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CitizenService.PermissionService.EducationService
{


    /// <summary>
    /// Model contain public property and methods
    /// </summary>
    public class EducationTours : DAL
    {

        #region Global Variables
        public Int64 EducationTourID { get; set; }
        public string Applicant_Type { get; set; }
        public string ApplicantType { get; set; }
        public Int64 UserID { get; set; }

        public string kioskuserid { get; set; }
        public string RequestedId { get; set; }

        public string CollegeName { get; set; }
        public string CollegeAddress { get; set; }
        public string Phonenumber { get; set; }
        public string Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationTo { get; set; }
        public Int64 ddlistrict { get; set; }
        public string DistrictName { get; set; }
        public Int64 Location { get; set; }
        public string LocationName { get; set; }
        public string Princeple_Name { get; set; }
        public string P_Address { get; set; }
        public string P_Gender { get; set; }
        public string P_Natinality { get; set; }
        public string ddl_MemberType { get; set; }
        public string P_MemberID { get; set; }
        public string P_MemberIDProof { get; set; }
        public string P_NumberOfMember { get; set; }
        public string MemberListFilename { get; set; }
        public string MemberListPath { get; set; }
        public string DocEducationalToueReq { get; set; }
        public string DocEducationalToueReqPath { get; set; }
        public Int64 Vehiclecat { get; set; }
        public Int64 ddl_vehicle { get; set; }
        public int TotalVehicle { set; get; }
        public int ModuleId { get; set; }
        public int ServiceTypeId { get; set; }
        public int PermissionId { get; set; }
        public int SubPermissionId { get; set; }
        public string TransactionStatus { get; set; }
        public string RsearchType { get; set; }
        #endregion

        /// <summary>
        /// save Research Data into Database
        /// </summary>
        /// <param name="research"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string addEducationtr(EducationTours _objEt)
        {
            string Result = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_Insert_EducationTourPermissions", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_EducationTourID", _objEt.EducationTourID);
                cmd.Parameters.AddWithValue("@P_RequestedID", _objEt.RequestedId);
                cmd.Parameters.AddWithValue("@P_CollegeName", _objEt.CollegeName);
                cmd.Parameters.AddWithValue("@P_CollegeAddress", _objEt.CollegeAddress == null ? (object)DBNull.Value : _objEt.CollegeAddress);
                cmd.Parameters.AddWithValue("@P_Phonenumber", _objEt.Phonenumber);
                cmd.Parameters.AddWithValue("@P_Category", _objEt.Category);
                cmd.Parameters.AddWithValue("@P_DistrictID", 1);
                cmd.Parameters.AddWithValue("@P_Location", _objEt.Location);
                cmd.Parameters.AddWithValue("@P_DurationFrom", _objEt.DurationFrom);
                cmd.Parameters.AddWithValue("@P_DurationTo", _objEt.DurationTo);
                cmd.Parameters.AddWithValue("@P_Princeple_Name", _objEt.Princeple_Name);
                cmd.Parameters.AddWithValue("@P_Address", _objEt.P_Address);
                cmd.Parameters.AddWithValue("@P_Gender", _objEt.P_Gender == null ? (object)DBNull.Value : _objEt.P_Gender);
                cmd.Parameters.AddWithValue("@P_Natinality", _objEt.P_Natinality == null ? (object)DBNull.Value : _objEt.P_Natinality);
                cmd.Parameters.AddWithValue("@P_MemberType", _objEt.ddl_MemberType == null ? (object)DBNull.Value : _objEt.ddl_MemberType);
                cmd.Parameters.AddWithValue("@P_MemberID", _objEt.P_MemberID == null ? (object)DBNull.Value : _objEt.P_MemberID);
                cmd.Parameters.AddWithValue("@P_MemberIDProof", _objEt.P_MemberIDProof == null ? (object)DBNull.Value : _objEt.P_MemberIDProof);
                cmd.Parameters.AddWithValue("@P_NumberOfMember", _objEt.P_NumberOfMember);
                cmd.Parameters.AddWithValue("@P_MemberListFilename", _objEt.MemberListFilename);
                cmd.Parameters.AddWithValue("@P_MemberListPath", _objEt.MemberListPath);
                cmd.Parameters.AddWithValue("@P_DocEducationalToueReq", _objEt.DocEducationalToueReq);
                cmd.Parameters.AddWithValue("@P_DocEducationalToueReqPath", _objEt.DocEducationalToueReqPath);
                cmd.Parameters.AddWithValue("@Vehiclecat", _objEt.Vehiclecat == null ? (object)DBNull.Value : _objEt.Vehiclecat);
                cmd.Parameters.AddWithValue("@ddl_vehicle", _objEt.ddl_vehicle == null ? (object)DBNull.Value : _objEt.ddl_vehicle);
                cmd.Parameters.AddWithValue("@EnteredBy", _objEt.UserID);
                cmd.Parameters.AddWithValue("@ResearchType", _objEt.RsearchType);
                //cmd.Parameters.AddWithValue("@AddVehicleDetail", dt);

                //cmd.Parameters.AddWithValue("@kioskuserid", Convert.ToInt64(_objEt.kioskuserid));
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
        /// 
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="Option"></param>
        /// <returns></returns>
        public DataTable GetEmitraDivCode(Int64 LocationId, string Option)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_CommonEmitraDivCode", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationId", LocationId);
                cmd.Parameters.AddWithValue("@Option", Option);
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
    }
}