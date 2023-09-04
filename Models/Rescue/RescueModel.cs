using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDSS.Models;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FMDSS.Models.Rescue
{

    public class AssistanceTypeFirstAidModel
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }


    public class RescueData
    {
        public string Action { get; set; }
        public string Index { get; set; }
        public int RescueId { get; set; }
        public string RescueDateTime { get; set; }
        public string AnimalName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string VillageId { get; set; }
        public string VillageName { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
        public string Status { get; set; }
        public string Processing { get; set; }
        public string FactualReportPath { get; set; }
        public string PostMortemReportPath { get; set; }
        public string Remarks { get; set; }

    }

    public class WildlifeScheduleModel
    {
        public int IsCWLW { get; set; }
        public bool Selected { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class RescuePhoto
    {
        public int Id { get; set; }
        public int ActiveStatus { get; set; }
        public string FileName { get; set; }
        public string Base64string { get; set; }
        public string Ext { get; set; }
    }
    public class RescueModel : DAL
    {
        public RescueModel()
        {

            AssistanceTypeFirstAidModel = new List<AssistanceTypeFirstAidModel>
            { new AssistanceTypeFirstAidModel { Name = "Energy Drink/Glucose", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "PainKiller", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "Antiseptic Spray", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "Bandage (if possible on site)", IsChecked = false },
            new AssistanceTypeFirstAidModel { Name = "Other", IsChecked = false }};

            ImageDataRegistration = new List<RescuePhoto>{
                new RescuePhoto{FileName="test",Base64string="test",Ext=".jpg"}
            };
        }

        public int? SendToNGOOrSelf { get; set; }
        public int IsCaptured { get; set; }
        public int IsReleased { get; set; }
        public string WLScheduleListCWLWApproval { get; set; }
        public string SendToOfficerSSOID { get; set; }
        public string CitizenRole { get; set; }
        public int UserID { get; set; }

        public int RegistrationID { get; set; }
        public string CitizenName { get; set; }
        public string CitizenEmailID { get; set; }
        public string CitizenMobileNo { get; set; }
        public bool Rural { get; set; }
        public string DistrictID { get; set; }
        public string District { get; set; }
        public string BlockID { get; set; }
        public string Block { get; set; }
        public string CityID { get; set; }
        public string City { get; set; }
        public string WardID { get; set; }
        public string Ward { get; set; }
        public string GPID { get; set; }
        public string GP { get; set; }
        public string VillageID { get; set; }
        public string Village { get; set; }
        public string Location { get; set; }
        public string CWLWRemarks { get; set; }
        public string AnimalID { get; set; }
        public string AnimalName { get; set; }
        public string ChildAnimalId { get; set; }
        public string ChildAnimalName { get; set; }
        public string OtherAnimalName { get; set; }
        public string RegistrationDescription { get; set; }
        public string ForestStaffMobile { get; set; }

        public List<RescuePhoto> ImageDataRegistration { get; set; }

        public List<RescuePhoto> ImageDataCapturesubmit { get; set; }

        public List<RescuePhoto> ImageDataCapture { get; set; }

        public List<RescuePhoto> ImageDataCaptureStaff { get; set; }

        public List<RescuePhoto> ImageDataReleasesubmit { get; set; }

        public List<RescuePhoto> ImageDataRelease { get; set; }

        public List<RescuePhoto> ImageDataReleaseStaff { get; set; }

        public string RegistrationPhotoPath { get; set; }
        public string WildlifeScheduleName { get; set; }
        public string WildlifeScheduleRemarks { get; set; }
        public string SendTONGORemarks { get; set; }

        public bool RegistrationApproved { get; set; }
        public bool Casualty { get; set; }
        public string CasualtyType { get; set; }
        public bool MediAssistRequired { get; set; }
        public string MediAssistType { get; set; }
        public string NoOfPersonInjured { get; set; }
        public string CasualtyDescription { get; set; }
        public bool AnimalNeedTreatment { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string RescuePhotoPath { get; set; }
        public string RescueRemarks { get; set; }
        public string ReleasePhotoPath { get; set; }
        public string RescueImage { get; set; }

        public string ReleaseRemarks { get; set; }
        public string RescueOfficerDesig { get; set; }
        public string RescueOfficerDesigName { get; set; }
        public string RescueOfficerName { get; set; }
        public string SpecialInstruction { get; set; }
        public string RescueStatus { get; set; }

        public string SendTOCWLWStatus { get; set; }
        public string SendTODCFafterAPPROVALStatus { get; set; }
        public string SendTONGOStatus { get; set; }

        public string ReportedBy { get; set; }
        public Nullable<DateTime> ReportingTime { get; set; }
        public string WhomReported { get; set; }
        public string ModeOfCommunication { get; set; }
        public string Latitude { get; set; }

        public string ForestStaffSSOID { get; set; }
        public string Longitude { get; set; }
        public DateTime? ActionTakenTime { get; set; }
        public DateTime? AnimalRescueTime { get; set; }
        public string[] ScheduleWildlifeAct { get; set; }

        public bool AnimalRescuedSuccessfully { get; set; }
        public string AnimalReleasedInto { get; set; }
        public string GPSLocation { get; set; }
        public bool AnimalDead { get; set; }
        public string PostmortemStatus { get; set; }
        public string InjuriesAdded { get; set; }
        public DataTable Images { get; set; }

        //Added by Dipak for additional columns on release form
        public string Gender { get; set; }
        public string Weight { get; set; }
        public string Age { get; set; }
        public string HealthStatus { get; set; }
        public bool IsNGO { get; set; }
        //End

        public string AssistanceTypeFirstAid { get; set; }
        public List<AssistanceTypeFirstAidModel> AssistanceTypeFirstAidModel { get; set; }


        public List<RescueModel> getAllRegistrations(string Action, long UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Select", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "getAllRegistrations" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            List<RescueModel> res = new List<RescueModel>();
            if (Globals.Util.isValidDataTable(dt))
            {
                res = Globals.Util.GetListFromTable<RescueModel>(dt);
            }
            return res;
        }



        public List<RescueModel> getAllRegistrationsBySearch ( long UserID,int StartRow,int FetchRowsNext,string Search)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_indexbySearch", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd); 
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@StartRow", StartRow);
                cmd.Parameters.AddWithValue("@FetchRowsNext", FetchRowsNext);
                cmd.Parameters.AddWithValue("@Search", Search); 
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "getAllRegistrations" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            List<RescueModel> res = new List<RescueModel>();
            if (Globals.Util.isValidDataTable(dt))
            {
                res = Globals.Util.GetListFromTable<RescueModel>(dt);
            }
            return res;
        }


        public List<RescueModel> GetRescueDetails_Mobile_BySerach(string ActionCode, string Action, long UserID, int startRec, int NextFetch, string Search)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Mobile_BySearch", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ActionCode", ActionCode);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@StartRow", startRec);
                cmd.Parameters.AddWithValue("@FetchRowsNext", NextFetch);
                cmd.Parameters.AddWithValue("@Search", Search);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Rescue_Registration_Mobile" + "_" + "RescueModel", 1, DateTime.Now, UserID);

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            List<RescueModel> res = new List<RescueModel>();
            if (Globals.Util.isValidDataTable(dt))
            {
                res = Globals.Util.GetListFromTable<RescueModel>(dt);
            }
            return res;
        }

        public List<RescueModel> GetRescueDetails_Mobile(string ActionCode, string Action, long UserID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Mobile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ActionCode", ActionCode);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Rescue_Registration_Mobile" + "_" + "RescueModel", 1, DateTime.Now, UserID);

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            List<RescueModel> res = new List<RescueModel>();
            if (Globals.Util.isValidDataTable(dt))
            {
                res = Globals.Util.GetListFromTable<RescueModel>(dt);
            }
            return res;
        }

        public RescueModel getRegistrationByID(int RegistrationID)
        {
            RescueModel res = new RescueModel();
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_SelectByRegistrationID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RegistrationID", RegistrationID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SendToNGOOrSelf"] != null && dr["SendToNGOOrSelf"].ToString() != "")
                        res.SendToNGOOrSelf = Convert.ToInt32(dr["SendToNGOOrSelf"]);
                    res.RegistrationID = Convert.ToInt32(dr["RegistrationID"]);
                    res.CitizenName = Convert.ToString(dr["CitizenName"]);
                    res.CitizenEmailID = Convert.ToString(dr["CitizenEmailID"]);
                    res.CitizenMobileNo = Convert.ToString(dr["CitizenMobileNo"]);
                    if (dr["Rural"] != null && dr["Rural"].ToString() != "")
                        res.Rural = Convert.ToBoolean(dr["Rural"]);
                    res.DistrictID = Convert.ToString(dr["DistrictID"]);
                    res.District = Convert.ToString(dr["District"]);
                    res.CityID = Convert.ToString(dr["CityID"]);
                    res.City = Convert.ToString(dr["City"]);
                    res.WardID = Convert.ToString(dr["WardID"]);
                    res.Ward = Convert.ToString(dr["Ward"]);
                    res.BlockID = Convert.ToString(dr["BlockID"]);
                    res.Block = Convert.ToString(dr["Block"]);
                    res.GPID = Convert.ToString(dr["GPID"]);
                    res.GP = Convert.ToString(dr["GP"]);
                    res.VillageID = Convert.ToString(dr["VillageID"]);
                    res.Village = Convert.ToString(dr["Village"]);
                    res.Location = Convert.ToString(dr["Location"]);
                    res.AnimalID = Convert.ToString(dr["AnimalID"]);
                    res.AnimalName = Convert.ToString(dr["AnimalName"]);
                    res.ChildAnimalId = Convert.ToString(dr["ChildAnimalId"]);
                    res.ChildAnimalName = Convert.ToString(dr["ChildAnimalName"]);


                    if (dr["Casualty"] != null && dr["Casualty"].ToString() != "")
                        res.Casualty = Convert.ToBoolean(dr["Casualty"]);
                    res.CasualtyType = Convert.ToString(dr["CasualtyType"]);
                    if (dr["MediAssistRequired"] != null && dr["MediAssistRequired"].ToString() != "")
                        res.MediAssistRequired = Convert.ToBoolean(dr["MediAssistRequired"]);
                    res.MediAssistType = Convert.ToString(dr["MediAssistType"]);
                    res.AssistanceTypeFirstAid = Convert.ToString(dr["AssistanceTypeFirstAid"]);
                    res.NoOfPersonInjured = Convert.ToString(dr["NoOfPersonInjured"]);
                    res.CasualtyDescription = Convert.ToString(dr["CasualtyDescription"]);
                    res.RegistrationDescription = Convert.ToString(dr["RegistrationDescription"]);
                    res.RescuePhotoPath = Convert.ToString(dr["RescuePhotoPath"]);
                    res.RescueRemarks = Convert.ToString(dr["RescueRemarks"]);
                    if (dr["AnimalNeedTreatment"] != null && dr["AnimalNeedTreatment"].ToString() != "")
                        res.AnimalNeedTreatment = Convert.ToBoolean(dr["AnimalNeedTreatment"]);
                    res.HospitalAddress = Convert.ToString(dr["HospitalAddress"]);
                    res.HospitalName = Convert.ToString(dr["HospitalName"]);
                    res.ReleasePhotoPath = Convert.ToString(dr["ReleasePhotoPath"]);
                    res.ReleaseRemarks = Convert.ToString(dr["ReleaseRemarks"]);
                    res.RescueOfficerDesig = Convert.ToString(dr["RescueOfficerDesig"]);
                    res.RescueOfficerDesigName = Convert.ToString(dr["RescueOfficerDesigName"]);
                    res.WildlifeScheduleName = Convert.ToString(dr["WildlifeScheduleName"]);

                    res.RescueOfficerName = Convert.ToString(dr["RescueOfficerName"]);
                    res.SpecialInstruction = Convert.ToString(dr["SpecialInstruction"]);
                    res.RescueStatus = Convert.ToString(dr["RescueStatus"]);
                    res.RegistrationPhotoPath = Convert.ToString(dr["RegistrationPhotoPath"]);

                    res.SendTONGOStatus = Convert.ToString(dr["SendTONGOStatus"]);
                    res.SendTODCFafterAPPROVALStatus = Convert.ToString(dr["SendTODCFafterAPPROVALStatus"]);
                    res.SendTOCWLWStatus = Convert.ToString(dr["SendTOCWLWStatus"]);

                    res.ReportedBy = Convert.ToString(dr["ReportedBy"]);
                    if (dr["ReportingTime"] == DBNull.Value)
                        res.ReportingTime = null;
                    else
                        res.ReportingTime = Convert.ToDateTime(dr["ReportingTime"]);


                    res.WhomReported = Convert.ToString(dr["WhomReported"]);
                    res.ModeOfCommunication = Convert.ToString(dr["ModeOfCommunication"]);

                    res.Latitude = Convert.ToString(dr["Latitude"]);
                    res.Longitude = Convert.ToString(dr["Longitude"]);
                    if (dr["ActionTakenTime"] == DBNull.Value)
                        res.ActionTakenTime = null;
                    else
                        res.ActionTakenTime = Convert.ToDateTime(dr["ActionTakenTime"]);

                    if (dr["AnimalRescueTime"] == DBNull.Value)
                        res.AnimalRescueTime = null;
                    else
                        res.AnimalRescueTime = Convert.ToDateTime(dr["AnimalRescueTime"]);

                    res.InjuriesAdded = Convert.ToString(dr["InjuriesAdded"]);


                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "getRegistrationByID" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return res;

        }

        //public int createRegistration(RescueModel model)
        //{
        //    int registrationID = 0;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        DALConn();
        //        SqlCommand cmd = new SqlCommand("Rescue_Registration_Insert", Conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@CitizenName", model.CitizenName);
        //        cmd.Parameters.AddWithValue("@CitizenEmailID", model.CitizenEmailID);
        //        cmd.Parameters.AddWithValue("@CitizenMobileNo", model.CitizenMobileNo);
        //        cmd.Parameters.AddWithValue("@Rural", model.Rural);
        //        cmd.Parameters.AddWithValue("@DistrictID", model.DistrictID);
        //        cmd.Parameters.AddWithValue("@CityID", model.CityID);
        //        cmd.Parameters.AddWithValue("@WardID", model.WardID);
        //        cmd.Parameters.AddWithValue("@BlockID", model.BlockID);
        //        cmd.Parameters.AddWithValue("@GPID", model.GPID);
        //        cmd.Parameters.AddWithValue("@VillageID", model.VillageID);
        //        cmd.Parameters.AddWithValue("@Location", model.Location);
        //        cmd.Parameters.AddWithValue("@AnimalID", model.AnimalID);
        //        cmd.Parameters.AddWithValue("@RegistrationDescription", model.RegistrationDescription);
        //        cmd.Parameters.AddWithValue("@RegistrationPhotoPath", model.RegistrationPhotoPath);
        //        cmd.Parameters.AddWithValue("@Casualty", model.Casualty);
        //        cmd.Parameters.AddWithValue("@CasualtyType", model.CasualtyType);
        //        cmd.Parameters.AddWithValue("@MediAssistRequired", model.MediAssistRequired);
        //        cmd.Parameters.AddWithValue("@MediAssistType", model.MediAssistType);
        //        cmd.Parameters.AddWithValue("@NoOfPersonInjured", model.NoOfPersonInjured);
        //        cmd.Parameters.AddWithValue("@CasualtyDescription", model.CasualtyDescription);
        //        cmd.Parameters.AddWithValue("@UserID", model.UserID);
        //        cmd.Parameters.AddWithValue("@RescuePhotoPath", model.RescueImage);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if(dt.Rows.Count > 0)
        //        {
        //            registrationID = Convert.ToInt32(dt.Rows[0]["RegistrationID"]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        registrationID = 0;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return registrationID;
        //}

        public Entity.ResponseMsg createRegistration(RescueModel model)
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();

            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Insert", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CitizenName", model.CitizenName);
                cmd.Parameters.AddWithValue("@CitizenEmailID", model.CitizenEmailID);
                cmd.Parameters.AddWithValue("@CitizenMobileNo", model.CitizenMobileNo);
                cmd.Parameters.AddWithValue("@Rural", model.Rural);
                cmd.Parameters.AddWithValue("@DistrictID", model.DistrictID);
                cmd.Parameters.AddWithValue("@CityID", model.CityID);
                cmd.Parameters.AddWithValue("@WardID", model.WardID);
                cmd.Parameters.AddWithValue("@BlockID", model.BlockID);
                cmd.Parameters.AddWithValue("@GPID", model.GPID);
                cmd.Parameters.AddWithValue("@VillageID", model.VillageID);
                cmd.Parameters.AddWithValue("@Location", model.Location);
                cmd.Parameters.AddWithValue("@AnimalID", model.AnimalID);
                cmd.Parameters.AddWithValue("@ChildAnimalId", model.ChildAnimalId);
                cmd.Parameters.AddWithValue("@RegistrationDescription", model.RegistrationDescription);
                cmd.Parameters.AddWithValue("@RegistrationPhotoPath", model.RegistrationPhotoPath);
                cmd.Parameters.AddWithValue("@Casualty", model.Casualty);
                cmd.Parameters.AddWithValue("@CasualtyType", model.CasualtyType);
                cmd.Parameters.AddWithValue("@MediAssistRequired", model.MediAssistRequired);
                cmd.Parameters.AddWithValue("@MediAssistType", model.MediAssistType);
                cmd.Parameters.AddWithValue("@NoOfPersonInjured", model.NoOfPersonInjured);
                cmd.Parameters.AddWithValue("@CasualtyDescription", model.CasualtyDescription);
                cmd.Parameters.AddWithValue("@RescueStatus", model.RescueStatus);
                cmd.Parameters.AddWithValue("@RescuePhotoPath", model.RescuePhotoPath);
                cmd.Parameters.AddWithValue("@ReleasePhotoPath", model.ReleasePhotoPath);
                cmd.Parameters.AddWithValue("@UserID", model.UserID);
                cmd.Parameters.AddWithValue("@OtherAnimalName", model.OtherAnimalName);
                cmd.Parameters.AddWithValue("@ReportedBy", model.ReportedBy);
                cmd.Parameters.AddWithValue("@ReportingTime", model.ReportingTime);
                cmd.Parameters.AddWithValue("@WhomReported", model.WhomReported);
                cmd.Parameters.AddWithValue("@ModeOfCommunication", model.ModeOfCommunication);
                cmd.Parameters.AddWithValue("@InjuriesAdded", model.InjuriesAdded);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@AssistanceTypeFirstAid", model.AssistanceTypeFirstAid);
                cmd.Parameters.AddWithValue("@ImageList", Images);


                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "createRegistration" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public Entity.ResponseMsg updateRegistrationApprove(RescueModel model)
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Update_Approval", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RegistrationID", model.RegistrationID);
                cmd.Parameters.AddWithValue("@RegistrationApproved", model.RegistrationApproved);
                cmd.Parameters.AddWithValue("@RescueStatus", model.RescueStatus);
                cmd.Parameters.AddWithValue("@RescueOfficerDesig", model.RescueOfficerDesig);
                cmd.Parameters.AddWithValue("@RescueOfficerName", model.RescueOfficerName);
                cmd.Parameters.AddWithValue("@CWLWRemarks", model.CWLWRemarks);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationApprove" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public Entity.ResponseMsg updateRegistrationCapture(RescueModel model)
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Update_Capture", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RegistrationID", model.RegistrationID);
                cmd.Parameters.AddWithValue("@RescueRemarks", model.RescueRemarks);
                cmd.Parameters.AddWithValue("@RescuePhotoPath", model.RescuePhotoPath);
                cmd.Parameters.AddWithValue("@RescueStatus", model.RescueStatus);
                cmd.Parameters.AddWithValue("@RescueOfficerDesig", model.RescueOfficerDesig);
                cmd.Parameters.AddWithValue("@RescueOfficerName", model.RescueOfficerName);

                cmd.Parameters.AddWithValue("@SendToNGOOrSelf", model.SendToNGOOrSelf ?? 0);
                cmd.Parameters.AddWithValue("@SendToOfficerSSOID", model.SendToOfficerSSOID);

                cmd.Parameters.AddWithValue("@Latitude", model.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", model.Longitude);
                cmd.Parameters.AddWithValue("@ActionTakenTime", model.ActionTakenTime);
                cmd.Parameters.AddWithValue("@AnimalRescueTime", model.AnimalRescueTime);
                cmd.Parameters.AddWithValue("@SendTONGORemarks", model.SendTONGORemarks);
                cmd.Parameters.AddWithValue("@WildlifeScheduleRemarks", model.WildlifeScheduleRemarks);

                cmd.Parameters.AddWithValue("@Casualty", model.Casualty);
                cmd.Parameters.AddWithValue("@CasualtyType", model.CasualtyType);
                cmd.Parameters.AddWithValue("@MediAssistRequired", model.MediAssistRequired);
                cmd.Parameters.AddWithValue("@MediAssistType", model.MediAssistType);
                cmd.Parameters.AddWithValue("@NoOfPersonInjured", model.NoOfPersonInjured);
                cmd.Parameters.AddWithValue("@CasualtyDescription", model.CasualtyDescription);
                cmd.Parameters.AddWithValue("@AssistanceTypeFirstAid", model.AssistanceTypeFirstAid);
                cmd.Parameters.AddWithValue("@ImageList", Images);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationCapture" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public Entity.ResponseMsg UpdateRegistrationAssigned(RescueModel model)
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Update_Assign", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ActionCode", 1);
                cmd.Parameters.AddWithValue("@RegistrationID", model.RegistrationID);
                cmd.Parameters.AddWithValue("@RescueRemarks", model.RescueRemarks);
                cmd.Parameters.AddWithValue("@RescueStatus", model.RescueStatus);
                cmd.Parameters.AddWithValue("@RescueOfficerDesig", model.RescueOfficerDesig);
                cmd.Parameters.AddWithValue("@RescueOfficerName", model.RescueOfficerName);
                cmd.Parameters.AddWithValue("@SendToNGOOrSelf", model.SendToNGOOrSelf ?? 0);
                cmd.Parameters.AddWithValue("@SendToOfficerSSOID", model.SendToOfficerSSOID);
                cmd.Parameters.AddWithValue("@ForestStaffMobile", model.ForestStaffMobile);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationCapture" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public Entity.ResponseMsg UpdateRegistrationByNGO(string actionCode, string parentID, string userID = "")
        {
            //Needs work
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Update_Assign", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("ActionCode", actionCode);
                cmd.Parameters.AddWithValue("ParentID", parentID);
                cmd.Parameters.AddWithValue("UserID", userID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationCapture" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public Entity.ResponseMsg updateRegistrationRelease(RescueModel model)
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Update_Release", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RegistrationID", model.RegistrationID);
                cmd.Parameters.AddWithValue("@AnimalNeedTreatment", model.AnimalNeedTreatment);
                cmd.Parameters.AddWithValue("@HospitalName", model.HospitalName);
                cmd.Parameters.AddWithValue("@HospitalAddress", model.HospitalAddress);
                cmd.Parameters.AddWithValue("@ReleaseRemarks", model.ReleaseRemarks);
                cmd.Parameters.AddWithValue("@ReleasePhotoPath", model.ReleasePhotoPath);
                cmd.Parameters.AddWithValue("@RescueStatus", model.RescueStatus);
                cmd.Parameters.AddWithValue("@ScheduleWildlifeAct", string.Join(",", model.ScheduleWildlifeAct));
                cmd.Parameters.AddWithValue("@AnimalRescuedSuccessfully", model.AnimalRescuedSuccessfully);
                cmd.Parameters.AddWithValue("@AnimalReleasedInto", model.AnimalReleasedInto);
                cmd.Parameters.AddWithValue("@GPSLocation", model.GPSLocation);
                cmd.Parameters.AddWithValue("@AnimalDead", model.AnimalDead);
                cmd.Parameters.AddWithValue("@PostmortemStatus", model.PostmortemStatus);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                cmd.Parameters.AddWithValue("@Weight", model.Weight);
                cmd.Parameters.AddWithValue("@Age", model.Age);
                cmd.Parameters.AddWithValue("@HealthStatus", model.HealthStatus);
                cmd.Parameters.AddWithValue("@ImageList", Images);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationRelease" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public Entity.ResponseMsg updateRegistrationOfficer(RescueModel model)
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Rescue_Registration_Update_Officer", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RegistrationID", model.RegistrationID);
                cmd.Parameters.AddWithValue("@RescueOfficerDesig", model.RescueOfficerDesig);
                cmd.Parameters.AddWithValue("@RescueOfficerName", model.RescueOfficerName);
                cmd.Parameters.AddWithValue("@SpecialInstruction", model.SpecialInstruction);
                cmd.Parameters.AddWithValue("@RescueStatus", model.RescueStatus);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationOfficer" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetOfficerDesignationRescue(string SSOID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetDesignatationForResuceModule", Conn);
                cmd.Parameters.AddWithValue("@option", "1");
                cmd.Parameters.AddWithValue("@ssoid", SSOID);
                cmd.Parameters.AddWithValue("@EmpDesig", "");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetOfficerDesignationRescue" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
        }


        public DataTable GetOfficerDesignationRescue()
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetDesignatationForResuceModule", Conn);
                cmd.Parameters.AddWithValue("@option", "1");
                cmd.Parameters.AddWithValue("@ssoid", HttpContext.Current.Session["SSOid"]);
                cmd.Parameters.AddWithValue("@EmpDesig", "");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetOfficerDesignationRescue" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet GetDropdownData(int actionCode, string parentID = "")
        {
            DataSet dsDropdownData = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Rescue_GetDropdownData", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ActionCode", actionCode);
                cmd.Parameters.AddWithValue("@ParentID", parentID);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsDropdownData);
            }
            catch (Exception ex) { }
            return dsDropdownData;
        }
    }

    public class NGOModel
    {
        public string WaterHoleVendorDetailsID { get; set; }
        public string RegNumber { get; set; }
        public string NameofVendor { get; set; }
        public string District { get; set; }
        public string PinCode { get; set; }
        public string RepresentativeName { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please Enter Valid Phone Number")]
        public string MobileNumber { get; set; }
        public string VendorSSOId { get; set; }
        public string PurposeforRegistration { get; set; }
        public string Circle_Code { get; set; }
        public string Division_Code { get; set; }
        public string Range_Code { get; set; }
        public string Village_Code { get; set; }
        public string AnimalID { get; set; }
        public string AnimalName { get; set; }
        public string ChildAnimalID { get; set; }
        public string ChildAnimalName { get; set; }
        public string OtherAnimalName { get; set; }
        public string WaterSource_Code { get; set; }
        public string WaterHoles_Code { get; set; }
        public string InsertDate { get; set; }
        public string InsertedBy { get; set; }
        public Boolean isActive { get; set; }
        public string UsedFor { get; set; }

        public string RANGE_NAME { get; set; }
        public string DIV_NAME { get; set; }
        public string CIRCLE_NAME { get; set; }
        public DataTable Images { get; set; }
        public virtual List<StaffModel> StaffList { get; set; }
        public virtual List<RescuePhoto> UploadNGODocuments { get; set; }
        public virtual List<RescuePhoto> UploadNGOAffidavit { get; set; }
        public virtual List<CIRCLEList> CIRCLELists { get; set; }
        public virtual List<DISTList> DISTLists { get; set; }





        public class CIRCLEList
        {
            public string Id { get; set; }
            public string RANGE_NAME { get; set; }

        }
        public class DISTList
        {
            public string RANGE_NAME { get; set; }


        }




        public List<NGOModel> GetNGOList()
        {
            DAL dal = new DAL();
            int rows;
            DataTable dt = new DataTable();
            List<NGOModel> res = new List<NGOModel>();
            try
            {
                dal.DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", dal.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetDetails");

                //cmd.Parameters.AddWithValue("@ImageList", Images);
                //cmd.Parameters.AddWithValue("@ActionFlag", "1");
                cmd.CommandType = CommandType.StoredProcedure;
                rows = cmd.ExecuteNonQuery();
                da.Fill(dt);

                if (Globals.Util.isValidDataTable(dt))
                {
                    res = Globals.Util.GetListFromTable<NGOModel>(dt);
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return res;

        }


        public void SaveImagebase64toImage(string base64, string path)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true))
                {
                    image.Save(path);
                }
            }
        }


        public Entity.ResponseMsg InsertUpdateNGODetails(NGOModel model, string actionPerform = "")
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();
            DAL dal = new DAL();
            try
            {
                dal.DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", dal.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@NameofVendor", model.NameofVendor);
                cmd.Parameters.AddWithValue("@District", model.District);
                cmd.Parameters.AddWithValue("@PinCode", model.PinCode);
                cmd.Parameters.AddWithValue("@RepresentativeName", model.RepresentativeName);
                cmd.Parameters.AddWithValue("@MobileNumber", model.MobileNumber);
                cmd.Parameters.AddWithValue("@VendorSSOId", model.VendorSSOId);

                cmd.Parameters.AddWithValue("@PurposeforRegistration", model.PurposeforRegistration);
                cmd.Parameters.AddWithValue("@Circle_Code", model.Circle_Code);

                cmd.Parameters.AddWithValue("@Division_Code", model.Division_Code);
                cmd.Parameters.AddWithValue("@Range_Code", model.Range_Code);

                cmd.Parameters.AddWithValue("@Village_Code", string.IsNullOrEmpty(model.Village_Code) ? "1" : model.Village_Code);
                cmd.Parameters.AddWithValue("@WaterSource_Code", "0");
                cmd.Parameters.AddWithValue("@WaterHoles_Code", "0");
                cmd.Parameters.AddWithValue("@AnimalID", model.AnimalID);
                cmd.Parameters.AddWithValue("@ChildAnimalId", model.ChildAnimalID);
                cmd.Parameters.AddWithValue("@OtherAnimalName", model.OtherAnimalName);

                cmd.Parameters.AddWithValue("@InsertedBy", model.InsertedBy);
                cmd.Parameters.AddWithValue("@isActive", model.isActive);
                cmd.Parameters.AddWithValue("@UsedFor", model.UsedFor);

                //cmd.Parameters.AddWithValue("@ImageList", Images);

                string xmlResult;
                using (StringWriter sw = new StringWriter())
                {
                    model.Images.WriteXml(sw);
                    xmlResult = sw.ToString();
                }
                cmd.Parameters.AddWithValue("@ImageXmlList", xmlResult);

                //if (Globals.Util.isValidDataTable(Images, true))
                //{
                //    cmd.Parameters.AddWithValue("@ImageXmlList", Images);
                //}

                if (actionPerform != "")
                {
                    if (model.StaffList != null && model.StaffList.Count() > 0)
                    {
                        DataTable dtStaff = new DataTable();
                        dtStaff.Columns.Add("ID");
                        dtStaff.Columns.Add("Name");
                        dtStaff.Columns.Add("Mobile");
                        dtStaff.Columns.Add("Age");
                        dtStaff.Columns.Add("PhotoURL");

                        try
                        {
                            foreach (var item in model.StaffList)
                            {
                                DataRow dr = dtStaff.NewRow();
                                dr["ID"] = item.ID;
                                dr["Name"] = item.Name;
                                dr["Mobile"] = item.Mobile;
                                dr["Age"] = item.Age;
                                if (item.PhotoURL != null)
                                {

                                    //  string fileName = "Registration_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(item.PhotoURL.FileName);
                                    // string _FileName = HttpContext.Current.Server.MapPath("~/RescueDocument/") + fileName;
                                    //  item.PhotoURL.SaveAs(_FileName);
                                    // dr["PhotoURL"] = "~/RescueDocument/" + fileName;
                                    string base64 = string.Empty;
                                    string fileName = "Established_" + DateTime.Now.ToFileTime().ToString() + "_" + Guid.NewGuid().ToString() + getFileExtension(item.PhotoURL, ref base64);  //Path.GetFileName(item.PhotoURL.FileName);
                                    string _FileName = HttpContext.Current.Server.MapPath("~/RescueDocument/") + fileName;
                                    SaveImagebase64toImage(base64, _FileName);
                                    //SaveByteArrayAsImage(_FileName, item.PhotoURL);
                                    //item.PhotoURL.SaveAs(_FileName);
                                    dr["PhotoURL"] = "~/RescueDocument/" + fileName;
                                }
                                dtStaff.Rows.Add(dr);
                            }

                            cmd.Parameters.AddWithValue("@StaffList", dtStaff);
                        }
                        catch (Exception ex) { }

                    }
                    cmd.Parameters.AddWithValue("@REGNUMBERIN", model.RegNumber);
                    cmd.Parameters.AddWithValue("@Action", "UPDATEVENDORDETAILS");
                }
                else
                {
                    if (model.StaffList != null && model.StaffList.Count() > 0)
                    {
                        DataTable dtStaff = new DataTable();
                        dtStaff.Columns.Add("ID");
                        dtStaff.Columns.Add("Name");
                        dtStaff.Columns.Add("Mobile");
                        dtStaff.Columns.Add("Age");
                        dtStaff.Columns.Add("PhotoURL");

                        try
                        {
                            foreach (var item in model.StaffList)
                            {
                                DataRow dr = dtStaff.NewRow();
                                dr["ID"] = "0";
                                dr["Name"] = item.Name;
                                dr["Mobile"] = item.Mobile;
                                dr["Age"] = item.Age;
                                if (item.PhotoURL != null)
                                {

                                    //  string fileName = "Registration_" + DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(item.PhotoURL.FileName);
                                    // string _FileName = HttpContext.Current.Server.MapPath("~/RescueDocument/") + fileName;
                                    //  item.PhotoURL.SaveAs(_FileName);
                                    // dr["PhotoURL"] = "~/RescueDocument/" + fileName;
                                    string base64 = string.Empty;
                                    string fileName = "Established_" + DateTime.Now.ToFileTime().ToString() + "_" + Guid.NewGuid().ToString() + getFileExtension(item.PhotoURL, ref base64);  //Path.GetFileName(item.PhotoURL.FileName);
                                    string _FileName = HttpContext.Current.Server.MapPath("~/RescueDocument/") + fileName;
                                    SaveImagebase64toImage(base64, _FileName);
                                    //SaveByteArrayAsImage(_FileName, item.PhotoURL);
                                    //item.PhotoURL.SaveAs(_FileName);
                                    dr["PhotoURL"] = "~/RescueDocument/" + fileName;
                                }
                                dtStaff.Rows.Add(dr);
                            }
                            cmd.Parameters.AddWithValue("@StaffList", dtStaff);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    cmd.Parameters.AddWithValue("@Action", "INSERTVENDERDETAILS");
                }

                cmd.CommandType = CommandType.StoredProcedure;

                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }

                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationCapture" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                dal.Conn.Close();
            }
        }

        private string getFileExtension(string base64String, ref string base64Data)
        {
            String[] strings = base64String.Split(',');
            base64Data = strings[1];
            String extension;
            switch (strings[0])
            {
                case "data:image/jpeg;base64":
                    extension = ".jpeg";
                    break;
                case "data:image/png;base64":
                    extension = ".png";
                    break;
                default:
                    extension = ".jpg";
                    break;
            }
            return extension;
        }


        public Entity.ResponseMsg UpdateVendorDetails(string parentID, string action = "", string userID = "")
        {
            Entity.ResponseMsg msg = null;
            DataSet dsData = new DataSet();
            DAL dal = new DAL();
            try
            {
                dal.DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", dal.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@ParentID", parentID);
                cmd.Parameters.AddWithValue("@Action", "DeActivateNGO");
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    msg = Globals.Util.GetListFromTable<Entity.ResponseMsg>(dsData, 0).FirstOrDefault();
                }
                return msg;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateVendorDetails" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(userID));

                throw ex;
            }
            finally
            {
                dal.Conn.Close();
            }
        }



        public NGOModel GetNGODetails_RangCode(string RangCode)
        {
            DataSet dsData = new DataSet();
            DAL dal = new DAL();
            NGOModel returnModel = new NGOModel();
            try
            {
                dal.DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", dal.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RANGE_CODE", RangCode);
                cmd.Parameters.AddWithValue("@Action", "GETDETAILS");
                cmd.Parameters.AddWithValue("@ImageXmlList", "");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    returnModel = Globals.Util.GetListFromTable<NGOModel>(dsData, 0).FirstOrDefault();
                    if (Globals.Util.isValidDataSet(dsData, 1))
                    {
                        returnModel.StaffList = Globals.Util.GetListFromTable<StaffModel>(dsData, 1);
                    }

                    if (Globals.Util.isValidDataSet(dsData, 2))
                    {
                        returnModel.Images = dsData.Tables[2];
                    }
                    if (Globals.Util.isValidDataSet(dsData, 3))
                    {
                        returnModel.UploadNGODocuments = Globals.Util.GetListFromTable<RescuePhoto>(dsData, 3);
                    }
                    if (Globals.Util.isValidDataSet(dsData, 4))
                    {
                        returnModel.UploadNGOAffidavit = Globals.Util.GetListFromTable<RescuePhoto>(dsData, 4);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationCapture" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                dal.Conn.Close();
            }
            return returnModel;
        }

        public NGOModel GetNGODetails(string regNumber)
        {
            DataSet dsData = new DataSet();
            DAL dal = new DAL();
            NGOModel returnModel = new NGOModel();
            try
            {
                dal.DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", dal.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@RegNumberIn", regNumber);
                cmd.Parameters.AddWithValue("@Action", "GetSingleRecord");
                cmd.Parameters.AddWithValue("@ImageXmlList", "");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dsData);

                if (Globals.Util.isValidDataSet(dsData, 0, true))
                {
                    returnModel = Globals.Util.GetListFromTable<NGOModel>(dsData, 0).FirstOrDefault();
                    if (Globals.Util.isValidDataSet(dsData, 1))
                    {
                        returnModel.StaffList = Globals.Util.GetListFromTable<StaffModel>(dsData, 1);
                    }

                    if (Globals.Util.isValidDataSet(dsData, 2))
                    {
                        returnModel.Images = dsData.Tables[2];
                    }
                    if (Globals.Util.isValidDataSet(dsData, 3))
                    {
                        returnModel.UploadNGODocuments = Globals.Util.GetListFromTable<RescuePhoto>(dsData, 3);
                    }
                    if (Globals.Util.isValidDataSet(dsData, 4))
                    {
                        returnModel.UploadNGOAffidavit = Globals.Util.GetListFromTable<RescuePhoto>(dsData, 4);
                    }
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "updateRegistrationCapture" + "_" + "RescueModel", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

                throw ex;
            }
            finally
            {
                dal.Conn.Close();
            }
            return returnModel;
        }

        public DataTable GetNGODetails1(string regNumber)
        {
            DataTable dt = new DataTable();
            DAL dal = new DAL();
            try
            {
                //DALConn();
                SqlCommand cmd = new SqlCommand("SP_WaterHoleVender", dal.Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RegNumberIn", regNumber);
                cmd.Parameters.AddWithValue("@Action", "GetSingleRecord");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw e;
            }
            return dt;
        }


    }

    public class StaffModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Age { get; set; }
        public string DisplayPhotoURL { get; set; }
        public string PhotoURL { get; set; }
    }

}