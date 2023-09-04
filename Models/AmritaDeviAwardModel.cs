using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FMDSS.Models
{
    [Serializable]
    public class AmiritaDeviYearsDetailsPDFModel:BaseModelSerializable
    {

        public string CURRENT { get; set; }
        public string PREV { get; set; }
        public string END { get; set; }
        public string WorkDescription { get; set; }
        public int WorkDescriptionIdS { get; set; }
    }
    [Serializable]
    public class AmiritaDeviPDFModel:BaseModelSerializable
    {
        public string ApplyDate { get; set; }
        public string RequestID { get; set; }
        public string CategoryName { get; set; }
        public string WorkStationName { get; set; }
        public string LandPlace { get; set; }
        public string Landhacktor { get; set; }
        public string PersonalLandHactor { get; set; }
        public string PersonalLandHactorDesc { get; set; }
        public string CollectiveLandHactor { get; set; }
        public string CollectiveLandDesc { get; set; }
        public string RevenueLandHactor { get; set; }
        public string RevenueLandDesc { get; set; }
        public string ForestLandHactor { get; set; }
        public string ForestLandDesc { get; set; }
        public string CurrentYear { get; set; }
        public bool IsOrgOrPerson { get; set; }
        public string FirstName1 { get; set; }
        public string FirstName2 { get; set; }
        public string Address { get; set; }

        public string GPSLat { get; set; }
        public string GPSLong { get; set; }
    }


    public class PrintAmritaDeviAwardModel
    {
        public string RequestID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Land Place")]
        public string LandPlace { get; set; }
        [Display(Name = "Status")]
        public string StatusName { get; set; }

    }

    [Serializable]
    public class AmiritaDeviGISPDFModel:BaseModelSerializable
    {
        public string Div_NM { get; set; }
        public string Dist_NM { get; set; }
        public string ForestDiv_NM { get; set; }
        public string Block_NM { get; set; }
        public string Gp_NM { get; set; }
        public string Village_NM { get; set; }
        public string areaName { get; set; }

    }
    [Serializable]
    public class GISDataBaseModel:BaseModelSerializable
    {
        public string DIV_CODE { get; set; }
        public string DIST_CODE { get; set; }
        public string BLK_CODE { get; set; }
        public string GP_CODE { get; set; }
        public string VILL_CODE { get; set; }
        public string Area { get; set; }
        public string GPSLat { get; set; }
        public string GPSLong { get; set; }
        public string GISID { get; set; }
        public string GISOrignalFilePath { get; set; }
        public string GISFilePath { get; set; }
        public string FOREST_DIVCODE { get; set; }
        public decimal AreaShapeInHactor { get; set; } 
    }

    [Serializable]
    public class AmritaDeviAwardModel : GISModel
    {
        public AmritaDeviAwardModel()
        {
            DetailList = new DataSet();
            DocumentsViews = new List<AttachmentsViews>();
            GetWorkList = new List<YearsDetailsList>();
            GISInformationList = new List<GISDataBaseModel>();
        }
        public List<GISDataBaseModel> GISInformationList { get; set; }
        //  GISModel Gis
        public string Document1 { get; set; }
        public string Document2 { get; set; }
        public string ApplicationPDFName { get; set; }
        public bool IsButtonShow { get; set; }

        public bool IsCCFLevel { get; set; }
        public List<SelectListItem> ListRejectedReason { get; set; }
        public int[] RejectedReason { get; set; }
        public string Comment { get; set; }
        public string Reason { get; set; }
        public string ActionStatus { get; set; }
        public string ReviewApprovalDocument { get; set; }
        public int ApprovalId { get; set; }
        public string ApprovalName { get; set; }
        public int RejectId { get; set; }
        public string RejectName { get; set; }



        public List<YearsDetailsList> GetWorkList { get; set; }
        public decimal AwardAmount { get; set; }
        public string ActionTakenBy { get; set; }
        public string StatusName { get; set; }
        public int Index { get; set; }
        public DataSet DetailList { get; set; }
        public string RequestID { get; set; }
        public int UserId { get; set; }
        public int AssignTo { get; set; }

        public bool IsOrgOrPerson { get; set; }
        public int CurrentYear { get; set; }

        [Required(ErrorMessage = "नामित व्यक्ति/संस्था का नाम फ़ील्ड आवश्यक है।")]
        [Display(Name = "(क) नामित व्यक्ति/संस्था का नाम")]
        public string FirstName1 { get; set; }

        [Display(Name = "(ख) नामित व्यक्ति/संस्था नाम")]
        public string FirstName2 { get; set; }

        [Required(ErrorMessage = "प्रायोजक का नाम एवं पता फ़ील्ड आवश्यक है।")]
        [Display(Name = "प्रायोजक का नाम एवं पता")]
        public string Address { get; set; }


        [Required(ErrorMessage = "पुरूस्कार की श्रेणी फ़ील्ड आवश्यक है।")]
        [Display(Name = "पुरूस्कार की श्रेणी जिसके लिए आवेदन किया गया है ")]
        public string AwardCategory { get; set; }


        [Required(ErrorMessage = "कार्य स्थल का नाम फ़ील्ड आवश्यक है।")]
        [Display(Name = "कार्य स्थल का नाम")]
        public string WorkStationName { get; set; }


        [Required(ErrorMessage = "स्थान फ़ील्ड आवश्यक है।")]
        [Display(Name = "(1) स्थान")]
        public string LandPlace { get; set; }


        [Required(ErrorMessage = "कुल वास्तविक क्षेत्र फ़ील्ड आवश्यक है।")]
        [Display(Name = "(2) कुल वास्तविक क्षेत्र (हैक्टेयर में)")]
        public string Landhacktor { get; set; }

        [Display(Name = "निजी भूमि क्षेत्र (हैक्टेयर में)")]
        public string PersonalLandHactor { get; set; }
        [Display(Name = "निजी भूमि वास्तविक क्षेत्र विवरण")]
        public string PersonalLandHactorDesc { get; set; }
        [Display(Name = "सामूदायिक भूमि क्षेत्र (हैक्टेयर में)")]
        public string CollectiveLandHactor { get; set; }
        [Display(Name = "सामूदायिक भूमि वास्तविक क्षेत्र विवरण")]
        public string CollectiveLandDesc { get; set; }
        [Display(Name = "राजस्व भूमि क्षेत्र (हैक्टेयर में)")]
        public string RevenueLandHactor { get; set; }
        [Display(Name = "राजस्व भूमि वास्तविक क्षेत्र विवरण")]
        public string RevenueLandDesc { get; set; }
        [Display(Name = "वन भूमि क्षेत्र (हैक्टेयर में)")]
        public string ForestLandHactor { get; set; }
        [Display(Name = "वन भूमि वास्तविक क्षेत्र विवरण")]
        public string ForestLandDesc { get; set; }

        public string ReferedBy { get; set; }

        [Display(Name = "स्थान")]
        public string Place { get; set; }
        [Display(Name = "दिनांक:-")]
        public string CurrentDate { get; set; }

        public YearsDetails[] WorkDetail { get; set; }

        public List<AttachmentsViews> DocumentsViews { get; set; }
        public List<DocumentList> DocumentList { get; set; }
        public List<CommandList> UserCommantList { get; set; }
    }
    [Serializable]
    public class DocumentList:BaseModelSerializable
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileType { get; set; }
    }
    [Serializable]
    public class AttachmentsViews:BaseModelSerializable
    {
        public string Action { get; set; }
        public string Users { get; set; }
        public string Documents { get; set; }
        public string Remarks { get; set; }
    }
    [Serializable]
    public class YearsDetails:BaseModelSerializable
    {

        public string Current { get; set; }
        public string Prev { get; set; }
        public string End { get; set; }
        [JsonIgnore]
        public string WorkDesc { get; set; }
        public int WorkDescID { get; set; }
    }
    [Serializable]
    public class YearsDetailsList:BaseModelSerializable
    {

        public string Current { get; set; }
        public string Prev { get; set; }
        public string End { get; set; }
        public string WorkDesc { get; set; }
    }
    [Serializable]
    public class CommandList:BaseModelSerializable
    {
        public string Commants { get; set; }
        public string Name { get; set; }
        public string StatusDesc { get; set; }
    }

    [Serializable]
    public class GISModel:BaseModelSerializable
    {
        public int NOCFor { get; set; }
        public int NOCPurpose { get; set; }
        public string Division { get; set; }
        public string District { get; set; }

        public string Tehsil { get; set; }
        public string PanchayatSamiti { get; set; }

        public string GramPanchayat { get; set; }
        public string Village { get; set; }
        public string NameofArea { get; set; }

        public string hdSSOID { get; set; }
        public string hdGISID { get; set; }
        public string hdfileEditModeId { get; set; }

        public string hdMenuId { get; set; }
        public string FOREST_DIVCODE { get; set; }
        [Required(ErrorMessage = "The GPS Latitude field is required.")]
        public string GPSLat { get; set; }
        [Required(ErrorMessage = "The GPS Longitude field is required.")]
        public string GPSLong { get; set; }


        public int PermissionId { get; set; }
        public bool ConditionFileEditMode { get; set; }
        public string GISID { get; set; }
        public string SSOID { get; set; }
        public string GISOrignalFilePath { get; set; }
        public string GISFilePath { get; set; }
        public decimal AreaShape { get; set; }
        public string ReferedBy { get; set; }

    }

    public class AmritaDeviInfo : DAL
    {

        public DataTable Select_AmritaDeviAward()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AD_Category", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_AD_Category");
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

        public DataSet Select_AmritaDeviDetailList()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetAmritaAwardDetailsList", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                da.TableMappings.Add("Table", "Years");
                da.TableMappings.Add("Table1", "WorkDetails");
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


        public DataTable ConvertToDatatableAward(YearsDetails[] lstlistInventoryData)
        {
            DataTable objDt2 = new DataTable("Table");
            try
            {

                #region Vehicle Info
                objDt2.Columns.Add("Current", typeof(String));
                objDt2.Columns.Add("Prev", typeof(String));
                objDt2.Columns.Add("End", typeof(String));
                objDt2.Columns.Add("WorkDescId", typeof(Int32));
                objDt2.AcceptChanges();

                foreach (var item in lstlistInventoryData.ToList())
                {
                    DataRow dr = objDt2.NewRow();
                    dr["Current"] = item.Current;
                    dr["Prev"] = item.Prev;
                    dr["End"] = item.End;
                    dr["WorkDescId"] = item.WorkDescID;
                    objDt2.Rows.Add(dr);
                    objDt2.AcceptChanges();
                }
                #endregion
            }
            catch (Exception ex)
            {
                // new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }

        public string SubmitApplication(AmritaDeviAwardModel _objmodel)
        {
           
            try
            {
                #region Convert Model Into Datatable

                string JSONString = JsonConvert.SerializeObject(_objmodel.WorkDetail);
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));

                JSONString = JsonConvert.SerializeObject(_objmodel.DocumentList);
                DataTable documentsListTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));

                JSONString = JsonConvert.SerializeObject(_objmodel.GISInformationList);
                DataTable GISTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));
                #endregion

                DALConn();
                
                SqlCommand cmd = new SqlCommand("[SP_Insert_AmritaDeviAwardDetails]", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NameOrganizationTYPE", _objmodel.IsOrgOrPerson);
                cmd.Parameters.AddWithValue("@Name", _objmodel.FirstName1);
                cmd.Parameters.AddWithValue("@SponcerName", _objmodel.FirstName2);
                cmd.Parameters.AddWithValue("@AwardCategoryId", _objmodel.AwardCategory);
                cmd.Parameters.AddWithValue("@Document1URL", _objmodel.Document1);
                cmd.Parameters.AddWithValue("@Document2URL", _objmodel.Document2);
                cmd.Parameters.AddWithValue("@ApplicationStatus", 8);
                cmd.Parameters.AddWithValue("@isActive", true);
                cmd.Parameters.AddWithValue("@CreatedBy", _objmodel.UserId);
                cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);

                cmd.Parameters.AddWithValue("@WorkPlace", _objmodel.WorkStationName);
                cmd.Parameters.AddWithValue("@LandPlace", _objmodel.LandPlace);
                cmd.Parameters.AddWithValue("@LandRealArea", _objmodel.Landhacktor);
                cmd.Parameters.AddWithValue("@PersonalLandHectare", _objmodel.PersonalLandHactor);
                cmd.Parameters.AddWithValue("@PersonalLandDETAIL", _objmodel.PersonalLandHactorDesc);
                cmd.Parameters.AddWithValue("@CommunityLandHectare", _objmodel.CollectiveLandHactor);
                cmd.Parameters.AddWithValue("@CommunityLandDETAIL", _objmodel.CollectiveLandDesc);
                cmd.Parameters.AddWithValue("@RevenueLandHectare", _objmodel.RevenueLandHactor);
                cmd.Parameters.AddWithValue("@RevenueLandDETAIL", _objmodel.RevenueLandDesc);
                cmd.Parameters.AddWithValue("@ForestLandHectare", _objmodel.ForestLandHactor);
                cmd.Parameters.AddWithValue("@ForestLandDetail", _objmodel.ForestLandDesc);
                cmd.Parameters.AddWithValue("@RequestedId", _objmodel.RequestID);
                cmd.Parameters.AddWithValue("@ApplyYear", _objmodel.CurrentYear);
                cmd.Parameters.AddWithValue("@ApplicationPDF", _objmodel.ApplicationPDFName);
                cmd.Parameters.AddWithValue("@ReferedBySSOID", _objmodel.ReferedBy);
                cmd.Parameters.AddWithValue("@AwardWorkDescriptions", dt);
                cmd.Parameters.AddWithValue("@GISModelList", GISTable);
                cmd.Parameters.AddWithValue("@Documents", documentsListTable);
                cmd.Parameters.AddWithValue("@result", System.Data.SqlDbType.Int);
                cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int awardID = Convert.ToInt32(cmd.Parameters["@result"].Value);

                #region Send Main and Email
                if (awardID > 0)
                {
                    try
                    {
                        SendSMSEmailForSuccessTransaction("AmritaDeviSubmit", _objmodel.RequestID, "AmritaDeviSubmit");
                    }
                    catch(Exception ex)
                    {
                        new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "SubmitApplication" + "_" + "AmritaDeviAwardMail", 1, DateTime.Now, _objmodel.UserId);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _objmodel.RequestID = string.Empty;
                new Common().ErrorLog(ex.Message + "_" + ex.StackTrace, "SubmitApplication" + "_" + "AmritaDeviAward", 1, DateTime.Now, _objmodel.UserId);
            }
            finally
            {
                Conn.Close();
            }
            return _objmodel.RequestID;
        }

        public DataSet ADReviewApprover(Int64 UserID)
        {

            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "ADReviewApproverLists");
                cmd.Parameters.AddWithValue("@UserID", UserID);
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

        public DataSet GetADApprvDetails(string UserID, string RequestID)
        {

            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "ADAssignerlist");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
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

        public DataTable SubmitADAssign(AmritaDeviAwardModel ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SubmitADAssign");
                cmd.Parameters.AddWithValue("@RequestID", ele.RequestID);


                cmd.Parameters.AddWithValue("@UserID", ele.UserId);
                cmd.Parameters.AddWithValue("@AssignTo", ele.AssignTo);
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

        public DataSet GetADRevApprvDetails(string UserID, string RequestID)
        {

            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "ADReviewApproverlist");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
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
        public DataSet GetADRevApprvDetailsView(string UserID, string RequestID, string ActionNames)
        {

            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", ActionNames);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
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
        public DataTable SubmitADReviewApprover(AmritaDeviAwardModel ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SubmitADReviewApprover");
                cmd.Parameters.AddWithValue("@RequestID", ele.RequestID);

                cmd.Parameters.AddWithValue("@ReviewApprovalDocument", ele.ReviewApprovalDocument);

                cmd.Parameters.AddWithValue("@UserID", ele.UserId);
                cmd.Parameters.AddWithValue("@AssignTo", ele.AssignTo);
                cmd.Parameters.AddWithValue("@ActionStatus", ele.ActionStatus);
                cmd.Parameters.AddWithValue("@Comment", ele.Comment);
                cmd.Parameters.AddWithValue("@Reasons", ele.Reason);

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dt);
                if (string.IsNullOrEmpty(ele.Reason))
                {
                    SendSMSEmailForSuccessTransaction("AmritaDeviRejectAndApproval", ele.RequestID, "AmritadeviReviewApprove");
                }
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

        public DataTable SubmitCommantbyTechandStateLeval(AmritaDeviAwardModel ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_AD_ReviewApprove", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SubmitCommantbyTechandStateLeval");
                cmd.Parameters.AddWithValue("@RequestID", ele.RequestID);
                cmd.Parameters.AddWithValue("@UserID", ele.UserId);
                cmd.Parameters.AddWithValue("@ActionStatus", ele.ActionStatus);
                cmd.Parameters.AddWithValue("@Comment", ele.Comment);

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

        public void SendSMSEmailForSuccessTransaction(string ACTION, string RequestId, string DatabaseAction)
        {
            #region  after SUCCESS flag send SMS and Email to the user // code by Rajveer Sharma

            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

            string body = string.Empty;
            DataTable DT = objSMSandEMAILtemplate.GetUserDetails(RequestId, DatabaseAction);
            string IsApproveAndRejectStatus = string.Empty;
            if (DT.Rows.Count > 0)
            {
                if (Convert.ToString(DT.Rows[0]["ApplicantEmail"]) != string.Empty)
                {
                    body = string.Empty;

                    if (DT.Columns.Contains("Status") && Convert.ToString(DT.Rows[0]["Status"]) != string.Empty)
                    {
                        IsApproveAndRejectStatus = Convert.ToString(DT.Rows[0]["Status"]);
                    }

                    if (ACTION == "AmritaDeviSubmit")
                    {
                        #region Send Email Submit Application
                        //string UserMailBody = UserMailSMSBody("Mail", ACTION, RequestId, IsApproveAndRejectStatus);
                        string UserMailBody = objSMSandEMAILtemplate.AmritaDeviMailSMSTemplate("Mail", RequestId, Convert.ToString(DT.Rows[0]["ApplicantName"]), IsApproveAndRejectStatus, WebConfigurationManager.AppSettings["AmritaDeviMailTemplate"].ToString());
                        string subject = "Amrita Devi Award";
                        // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);
                        objSMS_EMail_Services.sendEMail(subject, UserMailBody,DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);
                        #endregion
                    }
                    else if (ACTION == "AmritaDeviRejectAndApproval")
                    {
                        #region Send Email approval Application
                        //string UserMailBody = UserMailSMSBody("Mail", ACTION, RequestId, IsApproveAndRejectStatus);
                        string UserMailBody = objSMSandEMAILtemplate.AmritaDeviMailSMSTemplate("Mail", RequestId, Convert.ToString(DT.Rows[0]["ApplicantName"]), IsApproveAndRejectStatus, WebConfigurationManager.AppSettings["AmritaDeviApprovalMailTemplate"].ToString());
                        string subject = "Amrita Devi Award";
                        // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);
                        objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ApplicantEmail"].ToString(), string.Empty);
                        #endregion

                        #region Send Email approval TechnicalOfficer
                        if (Convert.ToString(DT.Rows[0]["ReviewApproveEmailId"]) != string.Empty)
                        {
                            UserMailBody = string.Empty;
                            //string UserMailBody = UserMailSMSBody("Mail", ACTION, RequestId, IsApproveAndRejectStatus);
                            UserMailBody = objSMSandEMAILtemplate.AmritaDeviMailSMSTemplate("Mail", RequestId, Convert.ToString(DT.Rows[0]["ApplicantName"]), IsApproveAndRejectStatus, WebConfigurationManager.AppSettings["AmritaDeviApprovalTechnicalOfficerEmailTemplate"].ToString());
                            subject = "Amrita Devi Award";
                            // objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ReviewApproveEmailId"].ToString(), string.Empty);
                            objSMS_EMail_Services.sendEMail(subject, UserMailBody, DT.Rows[0]["ReviewApproveEmailId"].ToString(), string.Empty);
                        }
                        #endregion
                    }
                }

                if (Convert.ToString(DT.Rows[0]["ApplicantMobile"]) != string.Empty)
                {
                    if (ACTION == "AmritaDeviSubmit")
                    {
                        #region Send SMS Submit Application
                        // string UserSmsBody = UserMailSMSBody("SMS", ACTION, RequestId, IsApproveAndRejectStatus);
                        string UserSmsBody = objSMSandEMAILtemplate.AmritaDeviMailSMSTemplate("SMS", RequestId, Convert.ToString(DT.Rows[0]["ApplicantName"]), IsApproveAndRejectStatus, WebConfigurationManager.AppSettings["AmritaDeviSMSTemplate"].ToString());
                        string mobile = DT.Rows[0]["ApplicantMobile"].ToString();
                        SMS_EMail_Services.sendSingleSMS(mobile, UserSmsBody);
                        #endregion
                    }
                    else if (ACTION == "AmritaDeviRejectAndApproval")
                    {
                        #region Send SMS Approval Application
                        // string UserSmsBody = UserMailSMSBody("SMS", ACTION, RequestId, IsApproveAndRejectStatus);
                        string UserSmsBody = objSMSandEMAILtemplate.AmritaDeviMailSMSTemplate("SMS", RequestId, Convert.ToString(DT.Rows[0]["ApplicantName"]), IsApproveAndRejectStatus, WebConfigurationManager.AppSettings["AmritaDeviApprovalSMSTemplate"].ToString());
                        string mobile = DT.Rows[0]["ApplicantMobile"].ToString();
                        SMS_EMail_Services.sendSingleSMS(mobile, UserSmsBody);
                        #endregion

                        if (Convert.ToString(DT.Rows[0]["ReviewApproveMobile"]) != string.Empty)
                        {
                            UserSmsBody = string.Empty;
                            #region Send SMS Approval Technical Officer
                            // string UserSmsBody = UserMailSMSBody("SMS", ACTION, RequestId, IsApproveAndRejectStatus);
                            UserSmsBody = objSMSandEMAILtemplate.AmritaDeviMailSMSTemplate("SMS", RequestId, Convert.ToString(DT.Rows[0]["ReviewApproveName"]), IsApproveAndRejectStatus, WebConfigurationManager.AppSettings["AmritaDeviApprovalTechnicalOfficerSMSTemplate"].ToString());
                            mobile = DT.Rows[0]["ReviewApproveMobile"].ToString();
                            SMS_EMail_Services.sendSingleSMS(mobile, UserSmsBody);
                            #endregion
                        }
                    }


                }

            }
            #endregion
        }

        public string UserMailSMSBody(string MailSMS, string ACTION, string requestid, string IsApproveAndRejectStatus)
        {

            StringBuilder sb = new StringBuilder();
            if (ACTION == "AmritaDeviSubmit")
            {
                if (MailSMS == "Mail")
                {
                    sb.Append("Dear User,");
                    sb.Append("<br />");
                    sb.Append("<p>Thanks for applying for <b>Amrita Devi Award</b> Your Transit Permit Number is <b>" + requestid + "</b></p>");
                    sb.Append("<br/>");
                    sb.Append("Please quote this ID for future communication.");
                    sb.Append("<br/><br/>");
                    sb.Append("Thanks");
                    sb.Append("<br/>");
                    sb.Append("FMDSS Team");
                    sb.Append("<br/><br/>");
                    sb.Append("<b><p>*** This is an auto generated email, please do not reply ***</b></p>");
                }
                else if (MailSMS == "SMS")
                {
                    sb.Append("Thanks for applying for Amrita Devi Award Your Transit Permit Number is " + requestid + "");
                    sb.Append("Please quote this ID for future communication.");
                }

            }


            return sb.ToString();
        }



    }
}