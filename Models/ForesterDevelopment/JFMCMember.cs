using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{
    public class JFMCMember:DAL
    {

        #region JFMC Member Registration

        #region Member Variable

        public long Id { get; set; }
        [Required(ErrorMessage = "Select district")]
        public string Dist_Code { get; set; }
        [Required(ErrorMessage = "Select block")]
        public string Blk_Code { get; set; }
        [Required(ErrorMessage = "Select Gram panchayat")]
        public string Gp_Code { get; set; }
        [Required(ErrorMessage = "Select village")]

        public string Vill_Code { get; set; }

        [Required(ErrorMessage = "Select JFMC")]
        public string JFMCRegistrationId { get; set; }
        public string VFMCName { get; set; }

        [Required(ErrorMessage = "Enter ssoid")]
        public string SSOId { get; set; }

     //   [Required(ErrorMessage = "Enter member name")]
        public string MemberName { get; set; }
        public string Father_Husband { get; set; }
      //  [Required(ErrorMessage = "Enter date of birth")]
        public string DOB { get; set; }
      //  [Required(ErrorMessage = "Select Gender")]
        public string modelGender { get; set; }
     //   [Required(ErrorMessage = "Enter Designation")]
        public string modelDesignation { get; set; }

    //    [Required(ErrorMessage = "Enter Mobile No.")]
    //    [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Mobile No.")]
        public string MobileNo { get; set; }

        public string Caste { get; set; }
    //    [Required(ErrorMessage = "Enter Address")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "Enter Address2")]
        public string Address2 { get; set; }
        public string rowid { get; set; }    
        public string FullName{get;set;}
 //       [Required(ErrorMessage = "Enter aadharId")]
        public string AadharId{get;set;}
        public string BhamashahId{get;set;}

  //      [Required(ErrorMessage = "Enter date of birth")]
        public string DatOFBirth { get; set; }

   //     [Required(ErrorMessage = "Select Gender")]
        public string Gender{get;set;}
        public string EmailId{get;set;}
        public string MobileNumber{get;set;}
        public string Designation{get;set;}
        public string Roles{get;set;}       
        #endregion


        #region Member Function
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
                new Common().ErrorLog(ex.Message, "GetJFMC" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Function to insert JFMC Registration
        /// </summary>
        /// <returns></returns>
        public Int64 InsertJFMCMember(JFMCMember _objjfmc,DataTable dtMember)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
              
                SqlParameter[] parameters =
                        {
                        new SqlParameter("@ID", Id ),    
                        new SqlParameter("@SSOId",HttpContext.Current.Session["SSOid"]),                          
                        new SqlParameter("@JFMC_Name", _objjfmc.JFMCRegistrationId),                    
                        new SqlParameter("@EnterBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),
                        new SqlParameter("@MemberDetails",dtMember), 
                        };
                  chk = Convert.ToInt64(ExecuteScalar("Sp_Citizen_JFMCMemberRegistration", parameters));
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertJFMCMember" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        /// <summary>
        /// return village code and name
        /// </summary>
        /// <param name="distcode"></param>
        /// <returns></returns>
        public DataTable GetJFMCMember(string JFMCRegistration)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("select SSOId,FullName,AadharId,BhamashahId,convert(varchar,DateOfBirth,103) DateOfBirth,Gender,EmailId,MobileNo,Address1,Designation,Roles from tbl_JFMCMemberdetails where JFMCMemberRegistrationId=" + JFMCRegistration, Conn);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetForestOfficersDesignation" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



        #endregion

        #endregion
    }
}