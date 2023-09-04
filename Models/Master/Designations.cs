using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Master
{

    public class Designations : DAL
    {

        public int DesigId { get; set; }
        public int Index { get; set; }
        public int ID { get; set; }
        public int FMDSSPermissionsTypesID { get; set; }

        public string ModuleDesc { get; set; }
        public string ServiceTypeDesc { get; set; }
        public string PermissionDesc { get; set; }
        public string SubPermissionDesc { get; set; }

       
        public string Desig_Name { get; set; }
       
        public string Desig_Alias { get; set; }
       


        public int IsActive { get; set; }
        public int IsReviewer { get; set; }
        public int IsApprover { get; set; }
        public int ApplicationAssigner { get; set; }

        public bool IsactiveView { get; set; }
       
        public string OperationType { get; set; }
        public DateTime LastUpdatedOn { get; set; }

        public long LastUpdatedBy { get; set; }

        public List<UnMappedUserDetails> UnMappedUserLIST { get; set; }

        public List<MappedUserDetails> MappedUserLIST { get; set; }

        public Int64 USERID { get; set; }
        public string SSOID { get; set; }
        public string OfficeID { get; set; }
        public string PLCAES { get; set; }
        public List<SelectListItem> PLCAEIDs { get; set; }
        public int[] PLCAEIDss { get; set; }
        public string OffcLevel { get; set; }
        public string ForestBoundaries { get; set; }

        
        public DataTable Select_Designations()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Designation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllDesignation");
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable Select_Designation(int DesigId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Designation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneDesignation");
                cmd.Parameters.AddWithValue("@DesigId", DesigId);
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable AddUpdateDesignation(Designations oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Designation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateDesignation");
                cmd.Parameters.AddWithValue("@DesigId", oPlace.DesigId);
                cmd.Parameters.AddWithValue("@Desig_Name", oPlace.Desig_Name);
                cmd.Parameters.AddWithValue("@Desig_Alias", oPlace.Desig_Alias);
                cmd.Parameters.AddWithValue("@LastUpdatedBy", oPlace.LastUpdatedBy);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsActive);
                cmd.Parameters.AddWithValue("@IsApprover", oPlace.IsApprover);
                cmd.Parameters.AddWithValue("@IsReviewer", oPlace.IsReviewer);
                cmd.Parameters.AddWithValue("@IsApplicationAssigner", oPlace.ApplicationAssigner);
                
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable SelectDesignationWithObjectLinking(int DesigId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Designation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectDesignationWithObjectLinking");
                cmd.Parameters.AddWithValue("@DesigId", DesigId);
                cmd.CommandType = CommandType.StoredProcedure;

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
       
        public string AddUpdateDesignationWithObjectLinking(Designations oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Designation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "ADDUpdateDesignationWithObjectLinking");
                cmd.Parameters.AddWithValue("@ID", oPlace.@ID);
                cmd.Parameters.AddWithValue("@DesigId", oPlace.DesigId);
                cmd.Parameters.AddWithValue("@FMDSSPermissionsTypesID", oPlace.@FMDSSPermissionsTypesID);
                cmd.Parameters.AddWithValue("@IsActive", oPlace.IsactiveView);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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


        public DataSet GetMapUnmapDesignationsForPA(int DesigId)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Designation_master", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetMapUnmapDesignationsForPA");
                cmd.Parameters.AddWithValue("@DesigId", DesigId);
                cmd.CommandType = CommandType.StoredProcedure;

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

        public string MappingForPA(Int64 USERIDs, int DESIGNATIONIDs, bool STATUS, Int64 LOGINSSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MappingDesignationsWithReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "CompleteMappingWithReviewerApprover");
                cmd.Parameters.AddWithValue("@USERID", USERIDs);
                cmd.Parameters.AddWithValue("@DESIGNATIONID", DESIGNATIONIDs);
                cmd.Parameters.AddWithValue("@ISACTIVE", STATUS);
                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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


        public string MappingForPA(Int64 USERIDs, int DESIGNATIONIDs, bool STATUS, Int64 LOGINSSOID,int FMDSSPermissionsTypesID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_MappingDesignationsWithReviewerApprover", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "ManualMappingWithReviewerApprover");
                cmd.Parameters.AddWithValue("@USERID", USERIDs);
                cmd.Parameters.AddWithValue("@DESIGNATIONID", DESIGNATIONIDs);
                cmd.Parameters.AddWithValue("@FMDSSPermissionsTypesID", FMDSSPermissionsTypesID);
                cmd.Parameters.AddWithValue("@ISACTIVE", STATUS);

                cmd.Parameters.AddWithValue("@LOGINSSOID", LOGINSSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                return dt.Rows[0][0].ToString();
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


        public DataTable Select_AllOfficeEmps()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UserProfilesDesignations", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllUser");
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataSet Select_AllOfficeEmps(string SSOID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_UserProfilesDesignations", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneUser");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                

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

        public DataTable AddUpdateUserDesignation(Designations oPlace)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_UserProfilesDesignations", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "ADDUpdateUserDesignation");
                cmd.Parameters.AddWithValue("@SSOID", oPlace.SSOID);
                cmd.Parameters.AddWithValue("@DesigId", oPlace.DesigId);
                cmd.CommandType = CommandType.StoredProcedure;
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



        public DataTable Select_SSODETAILS(string SSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_SSODETAILS", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GETSSODETAILS");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;

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


        public DataSet Select_Place(string SSOID,string OFFICE)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_MappingUserForOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetPlacewiseEmpAndOfficeMapping");
                cmd.Parameters.AddWithValue("@USERID", SSOID);
                cmd.Parameters.AddWithValue("@OfficeID", OFFICE);

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

        public DataSet AddPlacewiseEmpAndOfficeMapping(Int64 USERID, string OFFICE, string PLACEIDs)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_MappingUserForOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddPlacewiseEmpAndOfficeMapping");
                cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.Parameters.AddWithValue("@OfficeID", OFFICE);
                cmd.Parameters.AddWithValue("@PLACEID", PLACEIDs);
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

        #region add by sunny for multiple office mapping
        public DataTable Select_SSODETAILS_MultiOffice(string SSOID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SSODetails_MultipleOffice", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GETSSODETAILSForMultipleOffice");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;

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
        #endregion
    }


    public class UnMappedUserDetails
    {
        public int Index { get; set; }
        public Int64 USERID { get; set; }
        public string SSOID { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }
    
    }

    public class MappedUserDetails
    {
        public int Index { get; set; }
        public Int64 USERID { get; set; }
        public string SSOID { get; set; }
        public bool IsactiveView { get; set; }
        public int IsActive { get; set; }

    }
}