using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using FMDSS.Filters;

namespace FMDSS.Models.ForestProtection
{
    [MyExceptionHandler]
    public class OffenseRegistrationfinal : DAL
    {
        #region Crimescenedescription

        public string Crimerowid { get; set; }
        public long CrimeSceneID { get; set; }     
        public string VisitDate { get; set; }       
        public string VisitPlace { get; set; }       
        public string VisitTime { get; set; }       
        public string Description { get; set; }       
        public string Range { get; set; }
        public string PhotoURL { get; set; }
        public string TabShow1 { get; set; }
        public string TabShow2 { get; set; }
        public string OffenseCode { get; set; }
        public string OffenseDate { get; set; }
        public string OffensePlace { get; set; }
        public string DIST_NAME { get; set; }      
        public string WarrantTab { get; set; }
        public string UserRole { get; set; }
        public string Iscomplete { get; set; }
        public string ApproveStatus { get; set; }
        public string DfoApproveStatus { get; set; }
        public string ApplicantName { get; set; }   
        #endregion

        #region Description of Seized Item
        public long SeizedItemId { get; set; }
        public long TeamId { get; set; }
        public string FirstOfficer { get; set; }
        public string FirstOfficerDesig { get; set; }
        public string SecondOfficer { get; set; }
        public string SecondOfficerDesig { get; set; }
        public string ThirdOfficer { get; set; }
        public string ThirdOfficerDesig { get; set; }
        public string FourthOfficer { get; set; }
        public string FourthOfficerDesig { get; set; }
        public bool Vechile { get; set; }
        public bool ForestProduce { get; set; }
        public bool Equipment { get; set; }
        public bool Animal { get; set; }
        public bool AnimalArticle { get; set; }
        #endregion

        #region Vechile
        public string Vechilerowid { get; set; }
        public string VechileRTO { get; set; }
        public string VechileRegistrationNo { get; set; }
        public string VechileOwnerName { get; set; }
        public string VechileType { get; set; }
        public string VechileMake { get; set; }
        public string VechileModel { get; set; }
        public string VechileChassisNo { get; set; }
        public string VechileEngineNo { get; set; }
        public string PastOffensesByVechile { get; set; }
        public string VechileUploadDoc { get; set; }
        #endregion

        #region ProduceType
        public string Producerowid { get; set; }
        public string ProduceType { get; set; }
        public string SpeciesName { get; set; }
        public string QuantityOfProduce { get; set; }
        public string ProduceUploadDoc { get; set; }
        #endregion

        #region Equipment
        public string Equipmentrowid { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentMake { get; set; }
        public string EquipmentModel { get; set; }
        public string EquipmentCaliber { get; set; }
        public string EquipmentIdentificationNo { get; set; }
        public string EquipmentSize { get; set; }
        public string EquipmentUploadDoc { get; set; }
        #endregion

        #region Animal Details
        public string Animalrowid { get; set; }
        public string AnimalScientificName { get; set; }
        public string AnimalCommanName { get; set; }
        public string AnimalDescription { get; set; }
        public string AnimalWeight { get; set; }
        public string AnimalUploadDoc { get; set; }

        #endregion

        #region Animal Article Details
        public string ArticleAnimalrowid { get; set; }
        public string ArticleAnimalScientificName { get; set; }
        public string ArticleAnimalCommanName { get; set; }
        public string NameOfAnimalArticle { get; set; }
        public string DescriptionOfAnimalArticle { get; set; }
        public string ArticleQuantity { get; set; }
        public string AnimalArticleUploadDoc { get; set; }

        #endregion

        #region witness details
        public string Witnessrowid { get; set; }
        public long WitnessId { get; set; }
        public string Bhamasha { get; set; }
        public string WitnessName { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string Category { get; set; }
        public string Caste { get; set; }
        public string ResidentialAddress1 { get; set; }
        public string ResidentialAddress2 { get; set; }
        public string Pincode { get; set; }
        public string Village { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string PhotoId { get; set; }
        public string UploadId { get; set; }
        public string WitnessAge { get; set; }
        public string StatementDate { get; set; }
        public string WitnessStatement { get; set; }
        public string UploadSignedStatement { get; set; }

        #endregion

        #region IssueWarrant

       // public long WarrantId { get; set; }
        public string Warrantrowid { get; set; }
        public string NameOfOffender { get; set; }
        public string ClothesWorn { get; set; }
        public string ColorOfClothes { get; set; }
        public string PhysicalAppearance { get; set; }
        public string Height { get; set; }
        public string OtherSpecialDetails { get; set; }
        public string Appearancedate { get; set; }
        public string Appearancetime { get; set; }
        public string AppearancePlace { get; set; }

        #endregion

        #region File Court Case
        public long CaseId { get; set; }
        [Required(ErrorMessage = "Enter Court Name")]
        public string CourtName { get; set; }
        [Required(ErrorMessage = "Enter Court Case Number")]
        public string CourtCaseNo { get; set; }
        [Required(ErrorMessage = "Enter Court Type")]
        public string CourtType { get; set; }
        [Required(ErrorMessage = "Enter Court Place")]
        public string CourtPlace { get; set; }
        [Required(ErrorMessage = "Enter prosecution date")]
        public string ProsecutionDate { get; set; }
        [Required(ErrorMessage = "Enter decision taken")]
        public string DecisionTaken { get; set; }
        [Required(ErrorMessage = "Enter date of decision taken")]
        public string DateOfDecisionTaken { get; set; }
        public string ConvictionReason { get; set; }
        public string ReasonOfCaseFailed { get; set; }
        public string InterimOrder { get; set; }
        public string FinalJudgmentOrder { get; set; }

        #endregion

        #region IssueJamanat
        public long JamanatId { get; set; }
        [Required(ErrorMessage = "Enter guarantor name")]
        public string GuarantorName { get; set; }
        [Required(ErrorMessage = "Enter guarantor's father name")]
        public string GuarantorFatherName { get; set; }
        [Required(ErrorMessage = "Select id proof type")]
        public string GuarantorIdProofType { get; set; }
        public string IdProof { get; set; }
        [Required(ErrorMessage = "Select category")]
        public string GuarantorCategory { get; set; }
        public string GuarantorCaste { get; set; }
        [Required(ErrorMessage = "Enter address")]
        public string GuarantorAddress { get; set; }
        public string GuarantorResidentialAddress1 { get; set; }
        public string GuarantorResidentialAddress2 { get; set; }
        public string GuarantorPincode { get; set; }
        [Required(ErrorMessage = "Select village")]
        public string GuarantorVillageOrCity { get; set; }
        public string GuarantorNearestTehsil { get; set; }
        public string GuarantorNearestDistrict { get; set; }
        public string OffenderNearestPoliceStation { get; set; }
        public string OffenderNearestTehsil { get; set; }
        public string OffenderNearestDistrict { get; set; }
        [Required(ErrorMessage = "Enter amount of bail")]
        public Int32 AmountOfBail { get; set; }
        [Required(ErrorMessage = "Enter guaranteer date")]
        public string GuaranteerDate { get; set; }
        [Required(ErrorMessage = "Enter submit date")]
        public string MuchalkaSubmitDate { get; set; }
        [Required(ErrorMessage = "Enter name of witness1")]
        public string WitnessName1 { get; set; }
        [Required(ErrorMessage = "Enter name of witness2")]
        public string WitnessName2 { get; set; }
        public string FileView { get; set; }
        #endregion

        #region WarrantDelivery
         public string WarrantApproveStatus { get; set; }
         public string DeliveryMode { get; set; }
         public string DeliveryDate { get; set; }
        #endregion

        public DataTable Select_MailMobileNoByUserID(Int64 UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters = { new SqlParameter("@UserID", UserID) };
                Fill(dt, "SP_Get_EmailMobile", parameters);
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
        /// Get the offense details
        /// </summary>
        /// <returns></returns>
        public DataSet GetOffenseDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetOffenseList", Conn);
                cmd.Parameters.AddWithValue("@option", "1");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                
                da.Fill(ds);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetOffenseDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Get court case list
        /// </summary>
        /// <returns></returns>
        public DataSet GetCourtCaseDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetOffenseList", Conn);
                cmd.Parameters.AddWithValue("@option", "2");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);              
                da.Fill(ds);               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetCourtCaseDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Get issue jammant list
        /// </summary>
        /// <returns></returns>
        public DataSet GetIssueJammantDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetOffenseList", Conn);
                cmd.Parameters.AddWithValue("@option", "3");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                da.Fill(ds);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetIssueJammantDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Get applicant details
        /// </summary>
        /// <returns></returns>
        public DataSet GetApplicantDeatils(string OffenseCode)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetApplicantDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetApplicantDeatils" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public Int64 InsertSeizedItemTeam()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {
                                           
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),           
                        new SqlParameter("@FirstOfficerId", FirstOfficer),
                        new SqlParameter("@DesigFirstOfficer", FirstOfficerDesig),      
                        new SqlParameter("@SecondOfficerId",SecondOfficer),  
                        new SqlParameter("@DesigSecondOfficer",SecondOfficerDesig),    
                        new SqlParameter("@ThirdOfficerId",ThirdOfficer),  
                        new SqlParameter("@DesigThirdOfficer",ThirdOfficerDesig),        
                        new SqlParameter("@FourthOfficerId",FourthOfficer),  
                        new SqlParameter("@DesigFourthOfficer",FourthOfficerDesig),                             
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),                       
                        };
                chk = Convert.ToInt64(ExecuteScalar("SP_FPM_InsertSeizedItemTeam", parameters));

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertSeizedItemTeam" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;

        }
        /// <summary>
        /// Save the records of Seized item
        /// </summary>
        /// <param name="dtVechile"></param>
        /// <param name="dtProduce"></param>
        /// <param name="dtEquipment"></param>
        /// <param name="dtAnimal"></param>
        /// <param name="dtAnimalDescription"></param>
        /// <returns></returns>
        public Int64 InsertSeizedItems(DataTable dtVechile, DataTable dtProduce, DataTable dtEquipment, DataTable dtAnimal, DataTable dtAnimalDescription)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {                                            
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])), 
                        new SqlParameter("@SeizedVechileDetails",dtVechile), 
                        new SqlParameter("@SeizedProduceDetails",dtProduce), 
                        new SqlParameter("@SeizedEquipmentDetails",dtEquipment), 
                        new SqlParameter("@SeizedAnimalDetails",dtAnimal), 
                        new SqlParameter("@SeizedAnimalArticleDetails",dtAnimalDescription), 
                        };
                chk = Convert.ToInt64(ExecuteScalar("SP_FPM_InsertSeizedItems", parameters));
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertSeizedItemTeam" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;

        }


        /// <summary>
        /// Save the Witness details
        /// </summary>
        /// <param name="dtWitness"></param>
        /// <returns></returns>
        public Int64 InsertWitnessDetails(DataTable dtWitness)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {
                        new SqlParameter("@ID", WitnessId ),                              
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])), 
                        new SqlParameter("@WitnessDetails",dtWitness),                     
                        };
                 chk = Convert.ToInt64(ExecuteScalar("Sp_FPM_InsertWitnessDetails", parameters));
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertWitnessDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Save the warrant details
        /// </summary>
        /// <param name="dtWarrant"></param>
        /// <returns></returns>
        public Int64 InsertWarrantDetails(DataTable dtWarrant)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {                                             
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])), 
                        new SqlParameter("@WarrantDetails",dtWarrant),                     
                        };
                 chk = Convert.ToInt64(ExecuteScalar("Sp_FPM_InsertWarrantDetails", parameters));
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertWitnessDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
       
        public string InsertWarrantDelivery()
        {
            string chk = string.Empty;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {                                             
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@DeliveryMode",DeliveryMode ), 
                        new SqlParameter("@Deliverydate",DeliveryDate),                     
                        };
                chk = Convert.ToString(ExecuteScalar("Sp_FPM_InsertWarrantDelivery", parameters));

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertWitnessDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Use for insert the File court vase
        /// </summary>
        /// <returns></returns>
        public Int64 InsertFileCourtCase()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {
                        new SqlParameter("@ID", CaseId ),                        
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@CourtName", CourtName ), 
                        new SqlParameter("@courtCaseNo", CourtCaseNo ), 
                        new SqlParameter("@CourtType", CourtType ), 
                        new SqlParameter("@CourtPlace", CourtPlace ), 
                        new SqlParameter("@ProsecutionDate", ProsecutionDate ), 
                        new SqlParameter("@DecisionTaken", DecisionTaken ), 
                        new SqlParameter("@DateOfDecisionTaken", DateOfDecisionTaken ), 
                        new SqlParameter("@ConvictionReason", ConvictionReason ), 
                        new SqlParameter("@ReasonOfCaseFailed", ReasonOfCaseFailed ), 
                        new SqlParameter("@InterimOrder", InterimOrder ), 
                        new SqlParameter("@FinalJudgmentOrder",FinalJudgmentOrder ), 
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])), 
                                         
                        };
                chk = Convert.ToInt64(ExecuteScalar("Sp_FPM_InsertFileCourtCase", parameters));              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertFileCourtCase" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Insert the data of Issue Jamant
        /// </summary>
        /// <returns></returns>
        public Int64 InsertIssueJamanat()
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {
                        new SqlParameter("@ID", JamanatId ),                        
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@GuarantorName", GuarantorName ), 
                        new SqlParameter("@GuarantorIdProofType", GuarantorIdProofType ), 
                        new SqlParameter("@IdProof", IdProof ), 
                        new SqlParameter("@GuarantorCategory", GuarantorCategory ), 
                        new SqlParameter("@GuarantorCaste", GuarantorCaste ), 
                        new SqlParameter("@GuarantorAddress", GuarantorAddress ), 
                        new SqlParameter("@GuarantorResidentialAddress1", GuarantorResidentialAddress1 ), 
                        new SqlParameter("@GuarantorResidentialAddress2", GuarantorResidentialAddress2 ), 
                        new SqlParameter("@GuarantorPincode", Convert.ToInt32(GuarantorPincode)), 
                        new SqlParameter("@GuarantorVillageOrCity", GuarantorVillageOrCity ), 
                        new SqlParameter("@GuarantorNearestTehsil", GuarantorNearestTehsil ), 
                        new SqlParameter("@GuarantorNearestDistrict",GuarantorNearestDistrict ), 
                        new SqlParameter("@OffenderNearestPoliceStation",OffenderNearestPoliceStation ),
                        new SqlParameter("@OffenderNearestTehsil",OffenderNearestTehsil ),
                        new SqlParameter("@OffenderNearestDistrict",OffenderNearestDistrict ),
                        new SqlParameter("@AmountOfBail",AmountOfBail ),
                        new SqlParameter("@GuaranteerDate",GuaranteerDate ),
                        new SqlParameter("@MuchalkaSubmitDate",MuchalkaSubmitDate ),
                        new SqlParameter("@WitnessName1",WitnessName1 ),
                        new SqlParameter("@WitnessName2",WitnessName2 ),
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])), 
                                         
                        };
                 chk = Convert.ToInt64(ExecuteScalar("Sp_InsertForesterIssueJamanat", parameters));
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertIssueJamanat" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Upadate the Final status of Form steps
        /// </summary>
        /// <returns></returns>
        public string InsertFinalDetails()
        {
            string chk = string.Empty;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {                                             
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                 
                        new SqlParameter("@EnteredBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),                         
                        };
                 chk = Convert.ToString(ExecuteScalar("Sp_FPM_OffenseRegistFinalSubmit", parameters));
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertFinalDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Use for show the Warrant, Seized item and Witness details
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public string TabShowInfo(string option)
        {
            string chk = string.Empty;
            try
            {
                DALConn();
                SqlParameter[] parameters =
                        {  
                        new SqlParameter("@Option",option),                              
                        new SqlParameter("@OffenseCode",HttpContext.Current.Session["FPMOffenseID"]),                                                                                                                
                        };
                 chk = Convert.ToString(ExecuteScalar("Sp_FPM_TabShowInfo", parameters));
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "TabShowInfo" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }
        /// <summary>
        /// Ftech the records of Forestor
        /// </summary>
        /// <returns></returns>
        public DataSet GetForestDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_FrosterGetDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", HttpContext.Current.Session["FPMOffenseID"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                da.Fill(ds);              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetForestDetails" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// Use for fetch the officer details
        /// </summary>
        /// <returns></returns>

        public DataTable GetForestOfficers(string OffenseCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetForestOfficer", Conn);
                cmd.Parameters.AddWithValue("@option", "1");
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"]);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetForestOfficers" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// Get the Officers designation
        /// </summary>
        /// <param name="ssoid"></param>
        /// <returns></returns>
        public DataTable GetForestOfficersDesignation(string ssoid)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetForestOfficer", Conn);
                cmd.Parameters.AddWithValue("@option", "2");
                cmd.Parameters.AddWithValue("@ssoid", ssoid);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable GetFirstForestOfficer(string OffenseCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetForestOfficer", Conn);
                cmd.Parameters.AddWithValue("@option", "3");
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"]);
                cmd.Parameters.AddWithValue("@OffenseCode", OffenseCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetForestOfficers" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetAnimalScientificName(string AnimalId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("select Sno_Species_Animal from tbl_mst_SpeciesAnimal where ID="+AnimalId, Conn);              
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
        /// Fetch the drop down values based on parameter
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public DataTable GetDropdown(string option)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_OffenseRegistrationGetDropdown", Conn);
                cmd.Parameters.AddWithValue("@option", option);
                cmd.Parameters.AddWithValue("@DistCode", District);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                da.Fill(dt);              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetDropdown" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetWarrantOfficePlace()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetOfficePlace", Conn);
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"].ToString());               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetDropdown" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetSurveyDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetSurveyDetails", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", HttpContext.Current.Session["FPMOffenseID"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetOffenderName" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// get the Applicant name who is apply for Offense
        /// </summary>
        /// <returns></returns>
        public DataTable GetOffenderName()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetOffenderName", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", HttpContext.Current.Session["FPMOffenseID"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);                
                da.Fill(dt);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetOffenderName" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetOffenderWarrantDetail(string OffenderName)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FPM_GetWarrantOffenderdetail", Conn);
                cmd.Parameters.AddWithValue("@OffenseCode", HttpContext.Current.Session["FPMOffenseID"]);
                cmd.Parameters.AddWithValue("@OffenderName", OffenderName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetOffenderName" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetCourtCaseDetailByOffenceCode()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FPM_GetCourtCaseDetailsByOffenceCode", Conn);
                cmd.Parameters.AddWithValue("@Offence_Code", HttpContext.Current.Session["FPMOffenseID"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetCourtCaseDetail" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable GetFileInfo(string OffenseCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("select OffenseCode,FilePath from tbl_FPM_SurveyFile where OffenseCode='"+OffenseCode+"'",Conn);
                cmd.CommandType = CommandType.Text;
                System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);               
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "GetCourtCaseDetail" + "_" + "OffenseRegistrationfinal", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

    }
}