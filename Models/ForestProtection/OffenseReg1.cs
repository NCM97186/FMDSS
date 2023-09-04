using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace FMDSS.Models.ForestProtection
{

    public class OffenseReg : DAL
    {
        #region Global Variable
        public Int64 UserID { get; set; }
        public Int64 OffenseID { get; set; }
        public string OffenseCode { get; set; }
        public int RegFormNumber { get; set; }
        public string Circle { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string CircleCode { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string RangeCode { get; set; }
        public string Tehsil { get; set; }
        public string Naka { get; set; }
        public string ForestBlock { get; set; }
        public string Compartment { get; set; }
        public string OffensePlace { get; set; }
        public string Latitude { get; set; }
        public string OffenseDate { get; set; }
        public string OffenseTime { get; set; }
        public string Longitude { get; set; }
        public string LandMark { get; set; }
        public string NakaDistance { get; set; }
        public int ForestType { get; set; }
        public string OffenceCategory { get; set; }
        public string OffenseSubCategoryWildLife { get; set; }
        public string OffenseSubCategoryForest { get; set; }
        public string WildlifeProtectionSection { get; set; }
        public string ForestProtectionSection { get; set; }
        public string OffenseSeverity { get; set; }
        public string CrimeScenePhoto1 { get; set; }
        public string CrimeScenePhoto2 { get; set; }
        public string CrimeScenePhoto3 { get; set; }
        public string ParivadiName { get; set; }
        //public string IsCompoundable { get; set; }
        //public decimal SettlementAmount { get; set; }
        //public string IsAmountPaid { get; set; }
        public string ActionStatus { get; set; }
        public bool IsEditMode { get; set; }
        public string UserRole { get; set; }
        public int Status { get; set; }
        public string Offence_Description { get; set; }
        public bool Self { get; set; }
        public bool Name { get; set; }
        public string ApplicantName { get; set; }   
        #endregion
        #region "Offender Details"
        public long OffenderID { get; set; }
        public string OOffenderrowid { get; set; }
        public string OffenderType { get; set; }
        public string OffenderName { get; set; }
        public string OFatherName { get; set; }
        public string OSpouseName { get; set; }

        public string OCategory { get; set; }
        public string OCaste { get; set; }
        public string OClothesWorn { get; set; }
        public string OColorOfClothes { get; set; }
        public string OPhysicalAppearance { get; set; }
        public string OHeight { get; set; }
        public string OOtherSpecialDetails { get; set; }
        //public string ClothesWorn { get; set; }
        //public string ColorOfClothes { get; set; }
        //public string PhysicalAppearance { get; set; }
        //public string Height { get; set; }
        //public string OtherSpecialDetails { get; set; }
        public string OAddress1 { get; set; }
        public string OAddress2 { get; set; }
        public string OStateCode { get; set; }
        public string OPincode { get; set; }
        public string OVillageCode { get; set; }
        public string txtdistrict { get; set; }
        public string txtvillage { get; set; }
        public string ODistrictCode { get; set; }
        public string OPhoneNo { get; set; }
        public string OEmailID { get; set; }
        public string OEvidenceDocURL { get; set; }
        public string OPhotoIDType { get; set; }
        public string OPhotoIDURL { get; set; }
        public string OffenderStatementDoc { get; set; }
        public string ComplainantStatementDoc { get; set; }
        public string OffenderAge { get; set; }
        public string ArrestedOrdetained { get; set; }
        public string InformToOffenderRelative { get; set; }
        public string CommunicationMode { get; set; }
        public string CommunicationDate { get; set; }

        public string FardGriftri { get; set; }
        public string GriftariPunchnama { get; set; }
        public string NagriNaka { get; set; }
        public string JamaTalashi { get; set; }
        public string MedicalReport { get; set; }
        public string OPoliceStation { get; set; }
        public int ONumberOfOffender { get; set; }
        public string OffenseStatementDate { get; set; }
        public string OffenderDescription { get; set; }
        public string OffenderStatement { get; set; }
        public string OffenderComplainant { get; set; }
        public string FilesToBeUploaded { get; set; }


        #endregion
        #region Member Functions

        /// <summary>
        /// function responsible for fetching circle division district of login forester
        /// /// </summary>
        /// <returns></returns>
        public DataTable GetCircle_Div_by_Member()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "Select_Circle_Div_Dist", parameters);
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
        /// Use for Ftech the Range Details of Login User
        /// </summary>
        /// <returns></returns>
        public DataTable Get_Range_for_LoginUser()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "Select_Range_For_LoginUser", parameters);
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
        /// Fetch the data for Offense Place drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_OffensePlace()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "OffensePlace"), 
                                              new SqlParameter("@Id", "")
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Forest Type drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_ForestType()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "ForestType"), 
                                              new SqlParameter("@Id", "")
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Wildlife Protection Act drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_WildlifeProtectionAct()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "WProtectionAct"), 
                                              new SqlParameter("@Id", "")
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Forest Protection Act drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_ForestProtectionAct()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "FProtectionAct"),
                                              new SqlParameter("@Id", "")
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Offense Severity drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_OffenseSeverity()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "OffenseSeverity"),
                                              new SqlParameter("@Id", "")
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Offense Photo ID types drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_OPhotoIDType()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "OPhotoIDType"), 
                                              new SqlParameter("@Id", "")  
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Wildlife Sub-Category drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_WildLifeSubCategory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "WildlifeSubCat"), 
                                              new SqlParameter("@Id", "")    
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        /// Fetch the data for Forest Sub-Category drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_ForestSubCategory(string FProtectionId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "forestSubCat"),
                                              new SqlParameter("@Id", FProtectionId)      
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        ///  Fetch the data for Forest Category drop down
        /// </summary>
        /// <returns></returns>
        public DataTable Get_OffenseCategory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@DropDownType", "forestCategory"), 
                                              new SqlParameter("@Id", "")     
                                            };
                Fill(dt, "Sp_FPM_Common_SelectDropDown", parameters);
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
        public DataTable GetVillage(string distcode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("select VILL_CODE,VILL_NAME from tbl_mst_Villages where DIST_CODE=" + distcode, Conn);
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

        /// <summary>
        /// Save the All form details in Database
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string SubmitForm1(OffenseReg _objmodel)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Circle", _objmodel.CircleCode);
                cmd.Parameters.AddWithValue("@Division", _objmodel.DivisionCode);
                cmd.Parameters.AddWithValue("@District", _objmodel.DistrictCode);
                cmd.Parameters.AddWithValue("@Range", _objmodel.RangeCode);
                cmd.Parameters.AddWithValue("@Tehsil", _objmodel.Tehsil);
                cmd.Parameters.AddWithValue("@Naka", _objmodel.Naka);
                cmd.Parameters.AddWithValue("@Block", _objmodel.ForestBlock);
                cmd.Parameters.AddWithValue("@Compartment", _objmodel.Compartment);
                cmd.Parameters.AddWithValue("@OffensePlace", _objmodel.OffensePlace);
                cmd.Parameters.AddWithValue("@Latitude", _objmodel.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", _objmodel.Longitude);
                cmd.Parameters.AddWithValue("@Landmark", _objmodel.LandMark);
                cmd.Parameters.AddWithValue("@DistanceFromNaka", _objmodel.NakaDistance);
                cmd.Parameters.AddWithValue("@Description", _objmodel.Offence_Description);
                cmd.Parameters.AddWithValue("@OffenceCategory", _objmodel.OffenceCategory);
                cmd.Parameters.AddWithValue("@ApplicantName", _objmodel.ApplicantName);
                cmd.Parameters.AddWithValue("@OffenseDate", DateTime.ParseExact(_objmodel.OffenseDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@OffenseTime", _objmodel.OffenseTime);
                cmd.Parameters.AddWithValue("@ForestType", _objmodel.ForestType);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@UserRole", _objmodel.UserRole);
                cmd.Parameters.AddWithValue("@StatementType", "Insert");

                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm1" + "_" + "SaveFPMOffenseForm1", 2, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }
        /// <summary>
        /// Upadate the Offense Registration details in Database
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string UpdateForm1(OffenseReg _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form1", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OffenseCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@Circle", _objmodel.CircleCode);
                cmd.Parameters.AddWithValue("@Division", _objmodel.DivisionCode);
                cmd.Parameters.AddWithValue("@District", _objmodel.DistrictCode);
                cmd.Parameters.AddWithValue("@Range", _objmodel.RangeCode);
                cmd.Parameters.AddWithValue("@Tehsil", _objmodel.Tehsil);
                cmd.Parameters.AddWithValue("@Naka", _objmodel.Naka);
                cmd.Parameters.AddWithValue("@Block", _objmodel.ForestBlock);
                cmd.Parameters.AddWithValue("@Compartment", _objmodel.Compartment);
                cmd.Parameters.AddWithValue("@OffensePlace", _objmodel.OffensePlace);
                cmd.Parameters.AddWithValue("@Latitude", _objmodel.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", _objmodel.Longitude);
                cmd.Parameters.AddWithValue("@OffenseDate", DateTime.ParseExact(_objmodel.OffenseDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@OffenseTime", _objmodel.OffenseTime);
                cmd.Parameters.AddWithValue("@Landmark", _objmodel.LandMark);
                cmd.Parameters.AddWithValue("@DistanceFromNaka", _objmodel.NakaDistance);
                cmd.Parameters.AddWithValue("@ForestType", _objmodel.ForestType);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@StatementType", "Update");

                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateForm1" + "_" + "UpdateFPMOffenseForm1", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }
        /// <summary>
        /// fetch the details of Offense registration
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet GetAllRecordsForm1(string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form1", Conn);
                cmd.Parameters.AddWithValue("@StatementType", "Select");
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
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
        /// <summary>
        /// Save the details of Offense Category
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string SubmitForm2(OffenseReg _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form2", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseID", _objmodel.OffenseID);
                cmd.Parameters.AddWithValue("@OffenceCategory", Convert.ToInt32(_objmodel.OffenceCategory));
                cmd.Parameters.AddWithValue("@OffenseSubCategoryWildLife", _objmodel.OffenseSubCategoryWildLife);
                cmd.Parameters.AddWithValue("@OffenseSubCategoryForest", _objmodel.OffenseSubCategoryForest);
                cmd.Parameters.AddWithValue("@SectionWildlife", _objmodel.WildlifeProtectionSection);
                cmd.Parameters.AddWithValue("@SectionForest", _objmodel.ForestProtectionSection);
                cmd.Parameters.AddWithValue("@OffenseSeverity", Convert.ToInt32(_objmodel.OffenseSeverity));
                cmd.Parameters.AddWithValue("@PoliceStation", _objmodel.OPoliceStation);                            
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@UserRole", _objmodel.UserRole);
                cmd.Parameters.AddWithValue("@StatementType", "Insert");

                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm2" + "_" + "SaveFPMOffenseForm2", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }
        /// <summary>
        /// use for fetch the details of Offense Category
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet GetAllRecordsForm2(string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form2", Conn);
                cmd.Parameters.AddWithValue("@StatementType", "Select");
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
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
        /// <summary>
        /// Use for Fetch all the Form steps based on Parameter
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <param name="StatementType"></param>
        /// <returns></returns>
        public DataSet GetAllRecordsForm1(string OffenseCode, string StatementType)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetDetails_All_FormSteps", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
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
        /// <summary>
        /// Use for fetching the records of Form 2
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="StatementType"></param>
        /// <returns></returns>
        public DataSet GetAllRecordsForm2(string ID, string StatementType)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetDetails_All_FormSteps", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@OffenseCode", ID);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
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
        /// <summary>
        /// Upadate the records of form 2 by Offense Code
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <returns></returns>
        public string UpdateForm2(OffenseReg _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form2", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseID", _objmodel.OffenseID);
                cmd.Parameters.AddWithValue("@OffenseCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@OffenceCategory", Convert.ToInt32(_objmodel.OffenceCategory));
                cmd.Parameters.AddWithValue("@OffenseSubCategoryWildLife", _objmodel.OffenseSubCategoryWildLife);
                cmd.Parameters.AddWithValue("@OffenseSubCategoryForest", _objmodel.OffenseSubCategoryForest);
                cmd.Parameters.AddWithValue("@SectionWildlife", _objmodel.WildlifeProtectionSection);
                cmd.Parameters.AddWithValue("@SectionForest", _objmodel.ForestProtectionSection);
                cmd.Parameters.AddWithValue("@OffenseSeverity", Convert.ToInt32(_objmodel.OffenseSeverity));
                cmd.Parameters.AddWithValue("@PoliceStation", _objmodel.OPoliceStation);                               
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@StatementType", "Update");
                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm2" + "_" + "SaveFPMOffenseForm2", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }
        /// <summary>
        /// Save the Offender details
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <param name="dtable"></param>
        /// <returns></returns>
        public string SubmitForm3(OffenseReg _objmodel, DataTable dtable)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form3", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@OffenderType", _objmodel.OffenderType);
                cmd.Parameters.AddWithValue("@Offenderinfo", dtable);
                cmd.Parameters.AddWithValue("@NumberOfOffender", _objmodel.ONumberOfOffender);
                cmd.Parameters.AddWithValue("@OffenderDescription", _objmodel.OffenderDescription);
                //cmd.Parameters.AddWithValue("@PoliceStation", _objmodel.OPoliceStation);
                cmd.Parameters.AddWithValue("@OffenseStatementDate", _objmodel.OffenseStatementDate.ToString());
               // cmd.Parameters.AddWithValue("@OffenderStatement", _objmodel.OffenderStatement);
                cmd.Parameters.AddWithValue("@OffenderComplainant", _objmodel.OffenderComplainant);
                cmd.Parameters.AddWithValue("@ComplainantStatementDoc", _objmodel.ComplainantStatementDoc);
               // cmd.Parameters.AddWithValue("@OffenderStatementDoc", _objmodel.OffenderStatementDoc);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@StatementType", "Insert");
                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm3" + "_" + "SaveFPMOffenseForm3", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }

        /// <summary>
        /// Upadete the Form details
        /// </summary>
        /// <param name="_objmodel"></param>
        /// <param name="dtable"></param>
        /// <returns></returns>
        public string UpdateForm3(OffenseReg _objmodel, DataTable dtable)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form3", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OffenseCode", _objmodel.OffenseCode);
                cmd.Parameters.AddWithValue("@OffenderType", _objmodel.OffenderType);
                cmd.Parameters.AddWithValue("@Offenderinfo", dtable);
                cmd.Parameters.AddWithValue("@NumberOfOffender", _objmodel.ONumberOfOffender);
                cmd.Parameters.AddWithValue("@OffenderDescription", _objmodel.OffenderDescription);
                //cmd.Parameters.AddWithValue("@PoliceStation", _objmodel.OPoliceStation);
                cmd.Parameters.AddWithValue("@OffenseStatementDate", _objmodel.OffenseStatementDate.ToString());
               // cmd.Parameters.AddWithValue("@OffenderStatement", _objmodel.OffenderStatement);
                cmd.Parameters.AddWithValue("@OffenderComplainant", _objmodel.OffenderComplainant);
                cmd.Parameters.AddWithValue("@ComplainantStatementDoc", _objmodel.ComplainantStatementDoc);
               // cmd.Parameters.AddWithValue("@OffenderStatementDoc", _objmodel.OffenderStatementDoc);
                cmd.Parameters.AddWithValue("@UserID", _objmodel.UserID);
                cmd.Parameters.AddWithValue("@StatementType", "Update");

                string chId = cmd.ExecuteScalar().ToString();
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SubmitForm3" + "_" + "SaveFPMOffenseForm3", 5, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return "0";
        }
        /// <summary>
        /// Ftech the Existing records of Form 3
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet GetAllRecordsForm3(string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_InsertOffenseRegistration_Form3", Conn);
                cmd.Parameters.AddWithValue("@StatementType", "Select");
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
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
        /// <summary>
        /// Check the records is exist or not
        /// </summary>
        /// <returns></returns>
        public DataSet GetViewExistingRecords()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_Select_ExistingRecords", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
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
        /// <summary>
        /// Edit details
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public DataSet EditDetails(string OffenseCode, string UserRole)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_FPM_EditDetails", Conn);
                cmd.Parameters.AddWithValue("@Option", "1");
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);
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
        /// <summary>
        /// Fetch the citizen parivad details
        /// </summary>
        /// <param name="OffenseCode"></param>
        /// <param name="StatementType"></param>
        /// <returns></returns>
        public DataSet GetCitizenRecords(string OffenseCode, string StatementType)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetDetails_All_FormSteps", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
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
        /// <summary>
        /// Fetch the citizen offender details
        /// </summary>
        /// <param name="StatementType"></param>
        /// <param name="OffenseCode"></param>
        /// <returns></returns>
        public DataSet GetCitizenOffenderRecords(string StatementType, string OffenseCode)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetDetails_All_FormSteps", Conn);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.Parameters.AddWithValue("@StatementType", StatementType);
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
        /// <summary>
        /// Get the Applicant (Citizen/Forestor) details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserRole"></param>
        /// <returns></returns>
        public DataSet GetApplicantDeatils(string id, string UserRole)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetDetails_Applicant", Conn);
                cmd.Parameters.AddWithValue("@UserRole", UserRole);
                cmd.Parameters.AddWithValue("@OffenseCode", id);
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
        public DataTable GetClothes()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("select Id,Clothes from  tbl_FPM_mst_Clothes", Conn);
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
    }


    #region "Known/Unknown mapping Details"
    public class KnownOffenderDetails
    {   
   
        public string OOffenderrowid { get; set; }
        public string OffenderName { get; set; }
        public string OFatherName { get; set; }
        public string OSpouseName { get; set; }
        public string OCategory { get; set; }    
        public string OCaste { get; set; }      
        public string ClothesWorn { get; set; }
        public string ColorOfClothes { get; set; }
        public string PhysicalAppearance { get; set; }
        public string Height { get; set; }
        public string OtherSpecialDetails { get; set; }                                             
        public string OAddress1 { get; set; }
        public string OAddress2 { get; set; }
        public string OStateCode { get; set; }
        public string ODistrictCode { get; set; }
        public string OVillageCode { get; set; }
        public string OPincode { get; set; }
        public string OPhoneNo { get; set; }
        public string OEmailID { get; set; }
        public string OPhotoIDURL { get; set; }
        public int OPhotoIDType { get; set; }
        public string OffenderAge { get; set; }
        public string ArrestedOrdetained { get; set; }
        public string InformToOffenderRelative { get; set; }
        public string CommunicationMode { get; set; }
        public string CommunicationDate { get; set; }
        public string OffenderStatementDoc { get; set; }
        public string OffenderStatement { get; set; }

        public string FardGriftri { get; set; }
        public string GriftariPunchnama { get; set; }
        public string NagriNaka { get; set; }
        public string JamaTalashi { get; set; }
        public string MedicalReport { get; set; }
    }
    #endregion

}