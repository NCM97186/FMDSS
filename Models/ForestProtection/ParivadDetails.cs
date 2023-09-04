using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.WebPages.Html;

namespace FMDSS.Models.ForestProtection
{
    public class ParivadDetails : DAL
    {


    }

    public class FPMParivadRegistrations : DAL
    {
        #region "ParivadRegistration"
        public long OffenseID { get; set; }
        public long UserID { get; set; }
        public string UserRole { get; set; }

        public string OffenseCategory { get; set; }

        public string DateOfOffense { get; set; }
        public string TimeOfOffense { get; set; }

        public string OffensePlace { get; set; }

        public string DistrictID { get; set; }

        public string BlockCode { get; set; }

        public string GPCode { get; set; }

        public string VillageCode { get; set; }

        public decimal Lattitude { get; set; }
        public decimal Longitude { get; set; }

        public string Description { get; set; }
        public int NumberOfOffender { get; set; }

        public string OffenderDescription { get; set; }

        #endregion

        #region "Offender Details"
        public long OffenderID { get; set; }

        public string OffenderType { get; set; }
        public string kioskuserid { get; set; }
        public string OffenderName { get; set; }

        public string OFatherName { get; set; }

        public string OSpouseName { get; set; }

        public string OCategory { get; set; }
        public string OCasteName { get; set; }
        public string OClothesWorn { get; set; }
        public string OColorOfClothes { get; set; }
        public string OPhysicalAppearance { get; set; }
        public string OHeight { get; set; }
        public string OOtherSpecialDetails { get; set; }

        public string OAddress1 { get; set; }

        public string OAddress2 { get; set; }
        public string OStateCode { get; set; }
        public string txtdistrict { get; set; }

        public string OPincode { get; set; }

        public string OVillageCode { get; set; }
        public string txtvillage { get; set; }

        public string ODistrictCode { get; set; }

        public string OPhoneNo { get; set; }

        public string OEmailID { get; set; }

        public string EvidenceDocURL { get; set; }
        public string UEvidenceDocURL { get; set; }

        public string OffenderAge { get; set; }

        public string OPoliceStation { get; set; }

        public int ONumberOfOffender { get; set; }

        #endregion

        public string EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public string UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }


        public static IList<SelectListItem> DDLState()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();

            _result.Add(new SelectListItem { Value = "0", Text = "---Select---" });
            _result.Add(new SelectListItem { Value = "1", Text = "Rajasthan" });
            _result.Add(new SelectListItem { Value = "2", Text = "Other" });

            return _result;
        }

        public Int64 SubmitDetails(FPMParivadRegistrations _objmodel, DataTable dtOffender)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertUpdate_Citizen_OffenseRegistration", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCategory", Convert.ToInt32(_objmodel.OffenseCategory));              
                cmd.Parameters.AddWithValue("@DateOfOffense", DateTime.ParseExact(_objmodel.DateOfOffense.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@TimeOfOffense", _objmodel.TimeOfOffense);
                cmd.Parameters.AddWithValue("@OffensePlace", _objmodel.OffensePlace);
                cmd.Parameters.AddWithValue("@DistrictID", _objmodel.DistrictID);
                cmd.Parameters.AddWithValue("@BlockCode", _objmodel.BlockCode);
                cmd.Parameters.AddWithValue("@GPCode", _objmodel.GPCode);
                cmd.Parameters.AddWithValue("@VillageCode", _objmodel.VillageCode);
                cmd.Parameters.AddWithValue("@Description", _objmodel.Description);
                cmd.Parameters.AddWithValue("@UserRole", _objmodel.UserRole);            
                cmd.Parameters.AddWithValue("@NumberOfOffender", _objmodel.ONumberOfOffender);
                cmd.Parameters.AddWithValue("@OffenderDescription", _objmodel.OffenderDescription);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@KioskUserId",Convert.ToInt64( _objmodel.kioskuserid));
                cmd.Parameters.AddWithValue("@Offenderinfo", dtOffender);
                cmd.Parameters.AddWithValue("@StatementType", "Insert");

                cmd.Parameters.AddWithValue("@Lattitude", _objmodel.Lattitude); // Added By Arvind Kumar Sharma
                cmd.Parameters.AddWithValue("@Longitude", _objmodel.Longitude); // Added By Arvind Kumar Sharma

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.Message, "SubmitDetails" + "_" + "SaveOffenderDetails", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        /// <summary>
        /// Get the records
        /// </summary>
        /// <returns></returns>
        public DataSet GetViewExistingRecords()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_Select_OffenseRegistration", Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
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

    public class OffenderMappingDetails
    {
        #region "Offender Details"
        public string OffenderType { get; set; }
        public string Offenderrowid { get; set; }
        public string OffenderName { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string Category { get; set; }       
        public string Caste { get; set; }      
        public string ClothesWorn { get; set; }
        public string ColorOfClothes { get; set; }
        public string PhysicalAppearance { get; set; }
        public string Height { get; set; }
        public string OtherSpecialDetails { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string StateCode { get; set; }

        public string Pincode { get; set; }

        public string VillageCode { get; set; }

        public string DistrictCode { get; set; }

        public string PhoneNo { get; set; }

        public string EmailID { get; set; }

        public string EvidenceDocURL { get; set; }

        public string OffenderAge { get; set; }

        public string PoliceStation { get; set; }



        #endregion
    }

    public class OffenderGISReturnDetails
    {
        public string OffenseCategoryID { get; set; }
        public string DistrictID { get; set; }
        public string BlocknameID { get; set; }
        public string GPNameID { get; set; }
        public string VillageID { get; set; }

    }
}