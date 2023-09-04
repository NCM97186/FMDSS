using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FMDSS.Models.CitizenService.StakeholderService
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
        public Int64 JFMCRegistrationId { get; set; }
        [Required(ErrorMessage = "Enter Name of member")]
        public string MemberName { get; set; }
        public string Father_Husband { get; set; }
        [Required(ErrorMessage = "Enter date of birth")]
        public string DOB { get; set; }
        [Required(ErrorMessage = "Select Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Enter Designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Enter Mobile No.")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Mobile No.")]
        public string MobileNo { get; set; }

        public string Caste { get; set; }
        [Required(ErrorMessage = "Enter Address1")]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "Enter Address2")]
        public string Address2 { get; set; }

        #endregion


        #region Member Function
        /// Function for fetching  Districts from database
        /// </summary>
        /// <param name="divCode"></param>
        /// <returns></returns>
        public DataTable GetJFMC(string Village_Code)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_FDM_SELECTJFMC", Conn);
                cmd.Parameters.AddWithValue("@VillageCode", Village_Code);
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
        /// Function to insert JFMC Registration
        /// </summary>
        /// <returns></returns>
        public Int64 InsertJFMCMember(JFMCMember _objjfmc)
        {
            try
            {
                DateTime dtDob;
                dtDob = DateTime.ParseExact(_objjfmc.DOB, "dd/MM/yyyy", null);
                SqlParameter[] parameters =
                        {
                        new SqlParameter("@ID", Id ),    
                        new SqlParameter("@SSOId",HttpContext.Current.Session["SSOid"]),   
                        new SqlParameter("@Dist_Code",_objjfmc.Dist_Code),           
                        new SqlParameter("@Blk_Code", _objjfmc.Blk_Code),
                        new SqlParameter("@Gp_Code", _objjfmc.Gp_Code),      
                        new SqlParameter("@Vill_Code", _objjfmc.Vill_Code),    
                        new SqlParameter("@JFMC_Name", _objjfmc.JFMCRegistrationId),
                        new SqlParameter("@Member_Name",_objjfmc.MemberName),
                        new SqlParameter("@Father_Husband_Name",_objjfmc.Father_Husband),
                        new SqlParameter("@Date_Of_Birth",dtDob),
                        new SqlParameter("@Designation", _objjfmc.Designation),
                        new SqlParameter("@Gender", _objjfmc.Gender),
                        new SqlParameter("@Mobile_No", _objjfmc.MobileNo),
                        new SqlParameter("@Caste", _objjfmc.Caste),
                        new SqlParameter("@Address1", _objjfmc.Address1),
                        new SqlParameter("@Address2", _objjfmc.Address2),                                
                        new SqlParameter("@EnterBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),
                        };
                Int64 chk = Convert.ToInt64(ExecuteScalar("Sp_Citizen_JFMCMemberRegistration", parameters));
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


        #endregion

        #endregion
    }
}