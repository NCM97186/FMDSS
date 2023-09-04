using FMDSS.Models.DriverGuideAPIModel;
using FMDSS.Models.FmdssContext;
using FMDSS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMDSS.Controllers.DriverGuideAPI
{
    public class DriverGuideAPIController : ApiController
    {
        FmdssContext dbContext = new FmdssContext();
        private long status = 0;
        private string response = "";

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDriverGuideListfromcontext(int UserID)
        {
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            DriverGuideAPIModel lstDriverGuide = new DriverGuideAPIModel();
            try
            {
                Repository<DriverGuideAPIModel> repo = new Repository<DriverGuideAPIModel>();
                var param1 = new SqlParameter("@Action", 1);
                var param2 = new SqlParameter("@UserId", UserID);
                var result = repo.GetWithStoredProcedure("SP_DriverGuide @Action,@UserId", param1, param2);
                DriverGuideAPIModel tbl_DriverGuideProfile = new DriverGuideAPIModel();
                if (result != null)
                {
                    foreach (var re in result)
                    {
                        tbl_DriverGuideProfile.USERID = re.USERID;
                        tbl_DriverGuideProfile.ID = re.ID;
                        tbl_DriverGuideProfile.PersonType = re.PersonType;
                        tbl_DriverGuideProfile.VehicleNo = re.VehicleNo;
                        tbl_DriverGuideProfile.VehicleType = re.VehicleType;
                        tbl_DriverGuideProfile.IsActive = re.IsActive;
                        tbl_DriverGuideProfile.StatusCode = re.StatusCode;
                        tbl_DriverGuideProfile.Message = re.Message;
                        tbl_DriverGuideProfile.PersonName = re.PersonName;
                        tbl_DriverGuideProfile.ContactNo = re.ContactNo;
                        tbl_DriverGuideProfile.Email = re.Email;
                    }
                    StatusCode = 1;
                    Message = "USERID exists.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile });
                }
                else
                {
                    StatusCode = 0;
                    Message = "USERID does not exists.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDriverGuideList(int UserID)
        {
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", 1);
            cmd.Parameters.AddWithValue("@UserId", UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<DriverGuideAPIModel> lst = new List<DriverGuideAPIModel>();
            List<TotalVehicle> TotalVehicle_lst = new List<TotalVehicle>();
            DriverGuideAPIModel lst1 = new DriverGuideAPIModel();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["StatusCode"]) == "0")
                    {
                        StatusCode = 0;
                        Message = "USERID does not exists.";
                        return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = lst1 });
                    }
                    else
                    {
                        lst1.USERID = Convert.ToInt32(dt.Rows[0]["USERID"]);
                        lst1.StatusCode = Convert.ToInt32(dt.Rows[0]["StatusCode"]);
                        lst1.Message = Convert.ToString(dt.Rows[0]["Message"]);
                        lst1.ID = Convert.ToInt64(dt.Rows[0]["ID"]);
                        lst1.PersonType = Convert.ToString(dt.Rows[0]["PersonType"]);
                        lst1.PersonName = Convert.ToString(dt.Rows[0]["PersonName"]);
                        lst1.ContactNo = Convert.ToString(dt.Rows[0]["ContactNo"]);
                        lst1.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        lst1.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TotalVehicle TotalVehicle_lst1 = new TotalVehicle();
                            TotalVehicle_lst1.VehicleNo = Convert.ToString(dt.Rows[i]["VehicleNo"]);
                            TotalVehicle_lst1.VehicleType = Convert.ToString(dt.Rows[i]["VehicleType"]);
                            TotalVehicle_lst.Add(TotalVehicle_lst1);
                        }
                        lst1.TotalVehicle = TotalVehicle_lst;
                        StatusCode = 1;
                        Message = "USERID exists.";
                        return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = lst1 });
                    }
                }
                else
                {
                    StatusCode = 0;
                    Message = "USERID does not exists.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = lst1 });
                }
            }

            catch (Exception ex)
            {
                throw;
            }



        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage DriverGuideAttendanceSave(DriverGuideAttendance DriverGuideAttendanceList)
        {
            int EditAad = 0;
            long StatusCode = 0;
            string Message = "";
            string result = "";
            var response = new HttpResponseMessage();
            SqlConnection cnn = new SqlConnection();
            List<DriverGuideAPIModel> lst = new List<DriverGuideAPIModel>();
            DataTable dt = new DataTable();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();

            try
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", 4);
                    cmd.Parameters.AddWithValue("@PlaceId", DriverGuideAttendanceList.PlaceId);
                    cmd.Parameters.AddWithValue("@PersonType", DriverGuideAttendanceList.PersonType);
                    cmd.Parameters.AddWithValue("@VehicleType", DriverGuideAttendanceList.VehicleType);
                    cmd.Parameters.AddWithValue("@VehicleNo", DriverGuideAttendanceList.VehicleNo);
                    cmd.Parameters.AddWithValue("@GuideName", DriverGuideAttendanceList.GuideName);
                    cmd.Parameters.AddWithValue("@UserId", DriverGuideAttendanceList.UserId);
                    cmd.Parameters.AddWithValue("@IsActive", DriverGuideAttendanceList.IsActive);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    StatusCode = 1;
                    Message = "Inserted Successfully.";

                }
                catch (Exception)
                {
                    StatusCode = 0;
                    Message = "User does not have profile data.";
                    throw;
                }

            }
            catch (Exception ex)
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = dt });
        }

         #region comment code
        //[System.Web.Http.HttpPost]
        //public HttpResponseMessage DriverGuideAttendanceSave(DriverGuideAttendance DriverGuideAttendanceList)
        //{
        //    int EditAad = 0;
        //    long StatusCode = 0;
        //    string Message = "";
        //    string result = "";
        //    var response = new HttpResponseMessage();
        //    List<DriverGuideAttendance> lst = new List<DriverGuideAttendance>();
        //    tbl_OnlineBooking_VehicleDetailsAttendance DriverGuideAttendance = new tbl_OnlineBooking_VehicleDetailsAttendance();
        //    tbl_OnlineBooking_VehicleDetailsAttendance DriverGuideAttendance1 = new tbl_OnlineBooking_VehicleDetailsAttendance();
        //    tbl_DriverGuideProfile tbl_DriverGuideProfile = new tbl_DriverGuideProfile();
        //    try
        //    {
        //        try
        //        {
        //            //foreach (var items in DriverGuideAttendanceList)
        //            //{
        //            var userid = Convert.ToInt32(DriverGuideAttendanceList.UserId);
        //            tbl_DriverGuideProfile = dbContext.tbl_DriverGuideProfile.FirstOrDefault(i => i.USERID == userid);
        //            if (tbl_DriverGuideProfile != null)
        //            {
        //                DriverGuideAttendance.PlaceId = DriverGuideAttendanceList.PlaceId;
        //                DriverGuideAttendance.PersonType = DriverGuideAttendanceList.PersonType;
        //                DriverGuideAttendance.VehicleNo = DriverGuideAttendanceList.VehicleNo;
        //                DriverGuideAttendance.GuideName = DriverGuideAttendanceList.GuideName;
        //                DriverGuideAttendance.AttendanceDate = DateTime.Now;
        //                DriverGuideAttendance.UserId = DriverGuideAttendanceList.UserId;
        //                DriverGuideAttendance.IsActive = DriverGuideAttendanceList.IsActive;
        //                this.dbContext.tbl_OnlineBooking_VehicleDetailsAttendance.Add(DriverGuideAttendance);
        //                dbContext.Entry(DriverGuideAttendance).State = System.Data.Entity.EntityState.Added;
        //                status = dbContext.SaveChanges();
        //                var response_result = (from t in dbContext.tbl_OnlineBooking_VehicleDetailsAttendance
        //                                       where t.UserId == DriverGuideAttendanceList.UserId
        //                                       orderby t.AttendanceDate descending
        //                                       select new DriverGuideAttendance
        //                                       {
        //                                           ID = t.ID,
        //                                           UserId = t.UserId,
        //                                           PlaceId = t.PlaceId,
        //                                           PersonType = t.PersonType,
        //                                           VehicleNo = t.VehicleNo,
        //                                           GuideName = t.GuideName,
        //                                           AttendanceDate = t.AttendanceDate,
        //                                           IsActive = t.IsActive
        //                                       }).ToList().Take(1);
        //                foreach (var re in response_result)
        //                {
        //                    DriverGuideAttendance1.ID = re.ID;
        //                    DriverGuideAttendance1.UserId = re.UserId;
        //                    DriverGuideAttendance1.PlaceId = re.PlaceId;
        //                    DriverGuideAttendance1.PersonType = re.PersonType;
        //                    DriverGuideAttendance1.VehicleNo = re.VehicleNo;
        //                    DriverGuideAttendance1.GuideName = re.GuideName;
        //                    DriverGuideAttendance1.AttendanceDate = re.AttendanceDate;
        //                    DriverGuideAttendance1.IsActive = re.IsActive;
        //                }
        //                result = Newtonsoft.Json.JsonConvert.SerializeObject(response_result);
        //                lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DriverGuideAttendance>>(result);

        //                StatusCode = 1;
        //                Message = "Inserted Successfully.";
        //            }
        //            else
        //            {
        //                StatusCode = 0;
        //                Message = "User does not have profile data.";
        //            }
        //            //}
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = DriverGuideAttendance1 });

        //}
        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage GetDriverGuideList(int UserID)
        //{
        //    long StatusCode = 0;
        //    string Message = "";
        //    var response = new HttpResponseMessage();
        //    tbl_DriverGuideProfile tbl_DriverGuideProfile = new tbl_DriverGuideProfile();
        //    try
        //    {
        //        var userid = Convert.ToInt32(UserID);
        //        tbl_DriverGuideProfile = dbContext.tbl_DriverGuideProfile.FirstOrDefault(i => i.USERID == userid);
        //        if (tbl_DriverGuideProfile != null)
        //        {
        //            StatusCode = 1;
        //            Message = "USERID exists.";
        //            //return Request.CreateResponse<tbl_DriverGuideProfile>(HttpStatusCode.OK, tbl_DriverGuideProfile);
        //            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile });
        //        }
        //        else
        //        {
        //            StatusCode = 0;
        //            Message = "USERID does not exists.";
        //            //return Request.CreateResponse(HttpStatusCode.OK, tbl_DriverGuideProfile);
        //            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing Get Driver Guide Profile.");
        //    }
        //}
        //[System.Web.Http.HttpPost]

        //public HttpResponseMessage DriverGuideProfileSave(DriverGuideAPIModel DriverGuideProfile)
        //{
        //    int EditAad = 0;
        //    long StatusCode = 0;
        //    string Message = "";
        //    string result = "";
        //    var response = new HttpResponseMessage();
        //    tbl_DriverGuideProfile tbl_DriverGuideProfile = new tbl_DriverGuideProfile();
        //    tbl_DriverGuideProfile tbl_DriverGuideProfileSave = new tbl_DriverGuideProfile();
        //    tbl_DriverGuideProfile tbl_DriverGuideProfile1 = new tbl_DriverGuideProfile();
        //    List<DriverGuideAPIModel> lst = new List<DriverGuideAPIModel>();
        //    try
        //    {
        //        try
        //        {
        //            //foreach (var items in DriverGuideAttendanceList)
        //            //{
        //            var userid = Convert.ToInt32(DriverGuideProfile.USERID);
        //            tbl_DriverGuideProfile = dbContext.tbl_DriverGuideProfile.FirstOrDefault(i => i.USERID == userid);
        //            if (tbl_DriverGuideProfile != null)
        //            {
        //                StatusCode = 0;
        //                Message = "User already created profile.";
        //            }
        //            else
        //            {
        //                tbl_DriverGuideProfileSave.USERID = DriverGuideProfile.USERID;
        //                tbl_DriverGuideProfileSave.IsActive = DriverGuideProfile.IsActive;
        //                tbl_DriverGuideProfileSave.ID = DriverGuideProfile.ID;
        //                tbl_DriverGuideProfileSave.PersonType = DriverGuideProfile.PersonType;
        //                tbl_DriverGuideProfileSave.VehicleType = DriverGuideProfile.VehicleType;
        //                tbl_DriverGuideProfileSave.VehicleNo = DriverGuideProfile.VehicleNo;
        //                tbl_DriverGuideProfileSave.IsActive = DriverGuideProfile.IsActive;
        //                tbl_DriverGuideProfileSave.PersonName = DriverGuideProfile.PersonName;
        //                tbl_DriverGuideProfileSave.ContactNo = DriverGuideProfile.ContactNo;
        //                tbl_DriverGuideProfileSave.Email = DriverGuideProfile.Email;
        //                this.dbContext.tbl_DriverGuideProfile.Add(tbl_DriverGuideProfileSave);
        //                dbContext.Entry(tbl_DriverGuideProfileSave).State = System.Data.Entity.EntityState.Added;
        //                status = dbContext.SaveChanges();
        //                var response_result = (from t in dbContext.tbl_DriverGuideProfile
        //                                       where t.USERID == DriverGuideProfile.USERID
        //                                       orderby t.ID descending
        //                                       select new DriverGuideAPIModel
        //                                       {
        //                                           ID = t.ID,
        //                                           USERID = t.USERID,
        //                                           PersonType = t.PersonType,
        //                                           VehicleNo = t.VehicleNo,
        //                                           IsActive = t.IsActive,
        //                                           PersonName = t.PersonName,
        //                                           VehicleType = t.VehicleType,
        //                                           ContactNo = t.ContactNo,
        //                                           Email = t.Email,
        //                                       });
        //                foreach (var re in response_result)
        //                {
        //                    tbl_DriverGuideProfile1.USERID = re.USERID;
        //                    tbl_DriverGuideProfile1.ID = re.ID;
        //                    tbl_DriverGuideProfile1.PersonType = re.PersonType;
        //                    tbl_DriverGuideProfile1.VehicleNo = re.VehicleNo;
        //                    tbl_DriverGuideProfile1.VehicleType = re.VehicleType;
        //                    tbl_DriverGuideProfile1.IsActive = re.IsActive;
        //                    tbl_DriverGuideProfile1.PersonName = re.PersonName;
        //                    tbl_DriverGuideProfile1.ContactNo = re.ContactNo;
        //                    tbl_DriverGuideProfile1.Email = re.Email;
        //                }
        //                result = Newtonsoft.Json.JsonConvert.SerializeObject(response_result);
        //                lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DriverGuideAPIModel>>(result);
        //                StatusCode = 1;
        //                Message = "Inserted Successfully.";
        //                return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile1 });
        //            }
        //            //}
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile1 });
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = tbl_DriverGuideProfile1 });
        //}

        //[System.Web.Http.HttpGet]
        //public HttpResponseMessage GetShift(int UserID)
        //{
        //    string responseContent = string.Empty;
        //    long StatusCode = 0;
        //    string Message = "";
        //    var response = new HttpResponseMessage();
        //    GetShiftTime lstDriverGuide = new GetShiftTime();
        //    try
        //    {
        //        Repository<GetShiftTime> repo = new Repository<GetShiftTime>();
        //        var param1 = new SqlParameter("@Action", 2);
        //        var param2 = new SqlParameter("@PlaceId", 2);
        //        var result = repo.GetWithStoredProcedure("SP_DriverGuide @Action,@PlaceId", param1, param2).ToList();
        //        GetShiftTime ShiftTime = new GetShiftTime();
        //        if (result.Count > 0)
        //        {
        //            foreach (var re in result)
        //            {
        //                ShiftTime.KioskBookingMorningTimeFrom = re.KioskBookingMorningTimeFrom;
        //                ShiftTime.KioskBookingMorningTimeTo = re.KioskBookingMorningTimeTo;
        //                ShiftTime.KioskBookingEveningTimeFrom = re.KioskBookingEveningTimeFrom;
        //                ShiftTime.KioskBookingEveningTimeTo = re.KioskBookingEveningTimeTo;
        //                ShiftTime.PlaceID = re.PlaceID;
        //                ShiftTime.ShiftName = re.ShiftName;
        //                ShiftTime.ShiftSatus = re.ShiftSatus;
        //                ShiftTime.NextShiftName = re.NextShiftName;
        //                ShiftTime.NextShiftTime = re.NextShiftTime;
        //            }
        //            StatusCode = 1;
        //            Message = "Successfully Get";
        //            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = ShiftTime });
        //        }
        //        else
        //        {
        //            StatusCode = 0;
        //            Message = "An error occurs.";
        //            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = ShiftTime });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        #endregion

        [System.Web.Http.HttpPost]
        public HttpResponseMessage DriverGuideProfileSave(DriverGuideAPIModel DriverGuideProfile)
        {
            int EditAad = 0;
            long StatusCode = 0;
            string Message = "";
            string result = "";
            var response = new HttpResponseMessage();
            SqlConnection cnn = new SqlConnection();
            List<DriverGuideAPIModel> lst = new List<DriverGuideAPIModel>();
            DataTable dt = new DataTable();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();
            try
            {
                try
                {
                    foreach (var items in DriverGuideProfile.TotalVehicle)
                    {
                        SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", 3);
                        cmd.Parameters.AddWithValue("@UserId", DriverGuideProfile.USERID);
                        cmd.Parameters.AddWithValue("@PersonType", DriverGuideProfile.PersonType);
                        cmd.Parameters.AddWithValue("@VehicleNo", items.VehicleNo);
                        cmd.Parameters.AddWithValue("@PersonName", DriverGuideProfile.PersonName);
                        cmd.Parameters.AddWithValue("@VehicleType", items.VehicleType);
                        cmd.Parameters.AddWithValue("@ContactNo", DriverGuideProfile.ContactNo);
                        cmd.Parameters.AddWithValue("@Email", DriverGuideProfile.Email);
                        cmd.Parameters.AddWithValue("@IsActive", DriverGuideProfile.IsActive);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        StatusCode = 1;
                        Message = "Inserted Successfully.";
                    }
                }
                catch (Exception)
                {
                    StatusCode = 0;
                    Message = "Error Occurs.";
                    throw;
                }

            }
            catch (Exception ex)
            {
                StatusCode = 0;
                Message = "Error Occurs.";

            }
            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = dt });

        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetShift(long UserID)
        {
            SqlConnection cnn = new SqlConnection();
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            GetShiftTime lstDriverGuide = new GetShiftTime();
            FMDSS.Models.DAL dl = new Models.DAL();
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", 2);
                cmd.Parameters.AddWithValue("@PlaceId", 2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                GetShiftTime ShiftTime = new GetShiftTime();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ShiftTime.KioskBookingMorningTimeFrom = Convert.ToInt32(dt.Rows[i]["KioskBookingMorningTimeFrom"]);
                        ShiftTime.KioskBookingMorningTimeTo = Convert.ToInt32(dt.Rows[i]["KioskBookingMorningTimeTo"]);
                        ShiftTime.KioskBookingEveningTimeFrom = Convert.ToInt32(dt.Rows[i]["KioskBookingEveningTimeFrom"]);
                        ShiftTime.KioskBookingEveningTimeTo = Convert.ToInt32(dt.Rows[i]["KioskBookingEveningTimeTo"]);
                        ShiftTime.placeid = Convert.ToInt32(dt.Rows[i]["placeid"]);
                        ShiftTime.ShiftName = Convert.ToString(dt.Rows[i]["ShiftName"]);
                        ShiftTime.ShiftSatus = Convert.ToInt32(dt.Rows[i]["ShiftSatus"]);
                        ShiftTime.NextShiftName = Convert.ToString(dt.Rows[i]["NextShiftName"]);
                        ShiftTime.NextShiftTime = Convert.ToInt32(dt.Rows[i]["NextShiftTime"]);
                    }
                    StatusCode = 1;
                    Message = "Successfully Get";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = ShiftTime });
                }

                else
                {
                    StatusCode = 0;
                    Message = "An error occurs.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = ShiftTime });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRequestIDs_GuideName(string GuideName)
        {
            SqlConnection cnn = new SqlConnection();
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            GetRequestID lstRequestIDs = new GetRequestID();
            FMDSS.Models.DAL dl = new Models.DAL();
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", 5);
                cmd.Parameters.AddWithValue("@GuideName", GuideName);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                List<GetRequestID> RequestIDs = new List<GetRequestID>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetRequestID RequestIDs1 = new GetRequestID();
                        RequestIDs1.RequestID = Convert.ToString(dt.Rows[i]["RequestID"]);
                        RequestIDs1.DateOfArrival = Convert.ToString(dt.Rows[i]["DateOfArrival"]);
                        RequestIDs1.ShiftTimeName = Convert.ToString(dt.Rows[i]["ShiftTimeName"]);
                        RequestIDs.Add(RequestIDs1);
                    }

                    StatusCode = 1;
                    Message = "Successfully Get";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = RequestIDs });
                }
                else
                {
                    StatusCode = 0;
                    Message = "An error occurs.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = RequestIDs });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDataRequestID(string RequestID)
        {
            SqlConnection cnn = new SqlConnection();
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            GetDataRequestID lstRequestIDData = new GetDataRequestID();
            FMDSS.Models.DAL dl = new Models.DAL();
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", 7);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                List<GetDataRequestID> RequestIDs = new List<GetDataRequestID>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetDataRequestID RequestIDs1 = new GetDataRequestID();
                        RequestIDs1.RequestID = Convert.ToString(dt.Rows[i]["RequestID"]);
                        RequestIDs1.Name = Convert.ToString(dt.Rows[i]["Name"]);
                        RequestIDs1.Gender = Convert.ToString(dt.Rows[i]["Gender"]);
                        RequestIDs1.IDType = Convert.ToString(dt.Rows[i]["IDType"]);
                        RequestIDs1.IDNo = Convert.ToString(dt.Rows[i]["IDNo"]);
                        RequestIDs1.Nationality = Convert.ToString(dt.Rows[i]["Nationality"]);
                        RequestIDs1.MemberType = Convert.ToString(dt.Rows[i]["MemberType"]);
                        RequestIDs1.EnteredOn = Convert.ToString(dt.Rows[i]["EnteredOn"]);
                        RequestIDs1.GuidName = Convert.ToString(dt.Rows[i]["GuidName"]);
                        RequestIDs1.VehicleNumber = Convert.ToString(dt.Rows[i]["VehicleNumber"]);
                        RequestIDs1.PlaceID = Convert.ToInt32(dt.Rows[i]["PlaceID"]);
                        RequestIDs1.ZoneID = Convert.ToInt32(dt.Rows[i]["ZoneID"]);
                        RequestIDs1.ZoneName = Convert.ToString(dt.Rows[i]["ZoneName"]);
                        RequestIDs1.PlaceName = Convert.ToString(dt.Rows[i]["PlaceName"]);
                        RequestIDs1.ShiftTime = Convert.ToInt32(dt.Rows[i]["ShiftTime"]);
                        RequestIDs1.ShiftTimeName = Convert.ToString(dt.Rows[i]["ShiftTimeName"]);
                        RequestIDs1.DateOfArrival = Convert.ToString(dt.Rows[i]["DateOfArrival"]);
                        RequestIDs1.AmountTobePaid = Convert.ToDecimal(dt.Rows[i]["AmountTobePaid"]);
                        RequestIDs1.AmountWithServiceCharges = Convert.ToInt32(dt.Rows[i]["AmountWithServiceCharges"]);
                        RequestIDs.Add(RequestIDs1);
                    }
                    StatusCode = 1;
                    Message = "Successfully Get";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = RequestIDs });
                }
                else
                {
                    StatusCode = 0;
                    Message = "An error occurs.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = RequestIDs });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [System.Web.Http.HttpGet]
        //public HttpResponseMessage GetDataWildLifeFeedBackMaster(string PersonType)
        //{
        //    SqlConnection cnn = new SqlConnection();
        //    string responseContent = string.Empty;
        //    long StatusCode = 0;
        //    string Message = "";
        //    var response = new HttpResponseMessage();
        //    WildLifeFeedBackMaster lstRequestIDData = new WildLifeFeedBackMaster();
        //    FMDSS.Models.DAL dl = new Models.DAL();
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
        //        cnn.Open();
        //        SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Action", 8);
        //        cmd.Parameters.AddWithValue("@PersonType", PersonType);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        List<WildLifeFeedBackMaster> FeedBackMaster = new List<WildLifeFeedBackMaster>();
        //        if (dt.Rows.Count > 0)
        //        {

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                WildLifeFeedBackMaster FeedBackMaster1 = new WildLifeFeedBackMaster();
        //                FeedBackMaster1.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
        //                FeedBackMaster1.FeedBack = Convert.ToString(dt.Rows[i]["FeedBack"]);
        //                FeedBackMaster1.ControlType = Convert.ToString(dt.Rows[i]["ControlType"]);
        //                FeedBackMaster1.PersonType = Convert.ToString(dt.Rows[i]["PersonType"]);
        //                FeedBackMaster1.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
        //                FeedBackMaster.Add(FeedBackMaster1);
        //            }
        //            StatusCode = 1;
        //            Message = "Successfully Get";
        //            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = FeedBackMaster });
        //        }
        //        else
        //        {
        //            StatusCode = 0;
        //            Message = "An error occurs.";
        //            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = FeedBackMaster });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public HttpResponseMessage GetDataWildLifeFeedBackMaster(string PersonType)
        {
            SqlConnection cnn = new SqlConnection();
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            WildLifeFeedBackMaster lstRequestIDData = new WildLifeFeedBackMaster();
            FMDSS.Models.DAL dl = new Models.DAL();
            DataSet ds = new DataSet();
            try
            {
       
               
              
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", 8);
                cmd.Parameters.AddWithValue("@PersonType", PersonType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                List<WildLifeFeedBackMaster> FeedBackMaster = new List<WildLifeFeedBackMaster>();
                Options OptionsList = new Options();
                List<string> opt = new List<string>();
                opt.Add("Yes");
                opt.Add("No");
                if (dt.Rows.Count > 0)
                {
                    int count = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        WildLifeFeedBackMaster FeedBackMaster1 = new WildLifeFeedBackMaster();
                        FeedBackMaster1.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                        FeedBackMaster1.SNO = count;
                        FeedBackMaster1.FeedBack = Convert.ToString(dt.Rows[i]["FeedBack"]);
                        FeedBackMaster1.ControlType = Convert.ToString(dt.Rows[i]["ControlType"]);
                        FeedBackMaster1.PersonType = Convert.ToString(dt.Rows[i]["PersonType"]);
                        FeedBackMaster1.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                        FeedBackMaster1.Rating = Convert.ToInt32(dt.Rows[i]["Rating"]);
                        if (Convert.ToString(dt.Rows[i]["ControlType"]) == "Radio" || Convert.ToString(dt.Rows[i]["ControlType"]) == "RadioText")
                        {
                            FeedBackMaster1.Options = opt;
                        }


                        FeedBackMaster.Add(FeedBackMaster1);
                        count++;
                    }
                    StatusCode = 1;
                    Message = "Successfully Get";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = FeedBackMaster });
                }
                else
                {
                    StatusCode = 0;
                    Message = "An error occurs.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = FeedBackMaster });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Save_WildLifeFeedBackTransaction(List<WildLifeFeedBackTransaction> WildLifeFeedBackTransactionList)
        {
            int EditAad = 0;
            long StatusCode = 0;
            string Message = "";
            string result = "";
            var response = new HttpResponseMessage();
            SqlConnection cnn = new SqlConnection();
            List<DriverGuideAPIModel> lst = new List<DriverGuideAPIModel>();
            DataTable dt = new DataTable();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();

            try
            {
                try
                {
                    foreach (var items in WildLifeFeedBackTransactionList)
                    {
                        SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", 9);
                        cmd.Parameters.AddWithValue("@FeedBackID", items.FeedBackID);
                        cmd.Parameters.AddWithValue("@RequestID", items.RequestID);
                        cmd.Parameters.AddWithValue("@Rating", items.Rating);
                        cmd.Parameters.AddWithValue("@Remarks", items.Remarks);
                        cmd.Parameters.AddWithValue("@FeedBackYes_No", items.FeedBackYes_No);
                        cmd.Parameters.AddWithValue("@PersonType", items.PersonType);
                        cmd.Parameters.AddWithValue("@PersonName", items.PersonName);
                        cmd.Parameters.AddWithValue("@SSOID", items.SSOID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }
                    StatusCode = 1;
                    Message = "Inserted Successfully.";

                }
                catch (Exception)
                {
                    StatusCode = 0;
                    Message = "Some error occurs.";
                    throw;
                }

            }
            catch (Exception ex)
            {
                StatusCode = 0;
                Message = Convert.ToString(ex);
                throw;
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = dt });
        }


        [System.Web.Http.HttpPost]
        public HttpResponseMessage DriverGuideProfileSave_Owner(DriverGuideAPIModel_Owner DriverGuideProfile)
        {
            int EditAad = 0;
            long StatusCode = 0;
            string Message = "";
            string result = "";
            var response = new HttpResponseMessage();
            SqlConnection cnn = new SqlConnection();
            List<DriverGuideAPIModel_Owner> lst = new List<DriverGuideAPIModel_Owner>();
            DataTable dt = new DataTable();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();
            try
            {
                try
                {
                    foreach (var items in DriverGuideProfile.TotalVehicle_Owner)
                    {
                        SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", 3);
                        cmd.Parameters.AddWithValue("@UserId", DriverGuideProfile.USERID);
                        cmd.Parameters.AddWithValue("@PersonType", DriverGuideProfile.PersonType);
                        cmd.Parameters.AddWithValue("@VehicleNo", items.VehicleNo);
                        cmd.Parameters.AddWithValue("@PersonName", items.PersonName);
                        cmd.Parameters.AddWithValue("@VehicleType", items.VehicleType);
                        cmd.Parameters.AddWithValue("@ContactNo", DriverGuideProfile.ContactNo);
                        cmd.Parameters.AddWithValue("@Email", DriverGuideProfile.Email);
                        cmd.Parameters.AddWithValue("@IsActive", DriverGuideProfile.IsActive);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        StatusCode = 1;
                        Message = "Inserted Successfully.";
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = dt });

        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetDriverGuideList_Owner(int UserID)
        {
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", 1);
            cmd.Parameters.AddWithValue("@UserId", UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<DriverGuideAPIModel_Owner> lst = new List<DriverGuideAPIModel_Owner>();
            List<TotalVehicle_Owner> TotalVehicle_lst = new List<TotalVehicle_Owner>();
            DriverGuideAPIModel_Owner lst1 = new DriverGuideAPIModel_Owner();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["StatusCode"]) == "0")
                    {
                        StatusCode = 0;
                        Message = "USERID does not exists.";
                        return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = lst1 });
                    }
                    else
                    {
                        lst1.USERID = Convert.ToInt32(dt.Rows[0]["USERID"]);
                        lst1.StatusCode = Convert.ToInt32(dt.Rows[0]["StatusCode"]);
                        lst1.Message = Convert.ToString(dt.Rows[0]["Message"]);
                        lst1.ID = Convert.ToInt64(dt.Rows[0]["ID"]);
                        lst1.PersonType = Convert.ToString(dt.Rows[0]["PersonType"]);
                        // lst1.PersonName = Convert.ToString(dt.Rows[0]["PersonName"]);
                        lst1.ContactNo = Convert.ToString(dt.Rows[0]["ContactNo"]);
                        lst1.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        lst1.IsActive = Convert.ToInt32(dt.Rows[0]["IsActive"]);
                        int Count=1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            TotalVehicle_Owner TotalVehicle_lst1 = new TotalVehicle_Owner();
                            TotalVehicle_lst1.SNO = Count;
                            TotalVehicle_lst1.PersonName = Convert.ToString(dt.Rows[i]["PersonName"]);
                            TotalVehicle_lst1.VehicleNo = Convert.ToString(dt.Rows[i]["VehicleNo"]);
                            TotalVehicle_lst1.VehicleType = Convert.ToString(dt.Rows[i]["VehicleType"]);
                            TotalVehicle_lst.Add(TotalVehicle_lst1);
                            Count++;
                        }
                        lst1.TotalVehicle_Owner = TotalVehicle_lst;
                        StatusCode = 1;
                        Message = "USERID exists.";
                        return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = lst1 });
                    }
                }
                else
                {
                    StatusCode = 0;
                    Message = "USERID does not exists.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = lst1 });
                }
            }

            catch (Exception ex)
            {
                throw;
            }



        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage DriverGuideAttendanceSave_Owner(DriverGuideAttendance_Owner DriverGuideAttendanceList)
        {
            int EditAad = 0;
            long StatusCode = 0;
            string Message = "";
            string result = "";
            var response = new HttpResponseMessage();
            SqlConnection cnn = new SqlConnection();
            List<DriverGuideAPIModel> lst = new List<DriverGuideAPIModel>();
            DataTable dt = new DataTable();
            cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            cnn.Open();

            try
            {
                try
                {
                    foreach (var items in DriverGuideAttendanceList.TotalVehicle_Owner)
                    {
                        SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", 4);
                        cmd.Parameters.AddWithValue("@PlaceId", DriverGuideAttendanceList.PlaceId);
                        cmd.Parameters.AddWithValue("@PersonType", DriverGuideAttendanceList.PersonType);
                        cmd.Parameters.AddWithValue("@VehicleType", items.VehicleType);
                        cmd.Parameters.AddWithValue("@VehicleNo", items.VehicleNo);
                        cmd.Parameters.AddWithValue("@GuideName", items.PersonName);
                        cmd.Parameters.AddWithValue("@UserId", DriverGuideAttendanceList.UserId);
                        cmd.Parameters.AddWithValue("@IsActive", DriverGuideAttendanceList.IsActive);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        StatusCode = 1;
                        Message = "Inserted Successfully.";
                    }
                }
                catch (Exception)
                {
                    StatusCode = 0;
                    Message = "User does not have profile data.";
                    throw;
                }

            }
            catch (Exception ex)
            {

            }
            return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = dt });
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetRequestIDs_UserID(string UserId)
        {
            SqlConnection cnn = new SqlConnection();
            string responseContent = string.Empty;
            long StatusCode = 0;
            string Message = "";
            var response = new HttpResponseMessage();
            GetRequestID lstRequestIDs = new GetRequestID();
            FMDSS.Models.DAL dl = new Models.DAL();
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                cnn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
                cnn.Open();
                SqlCommand cmd = new SqlCommand("SP_DriverGuide", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", 10);
                cmd.Parameters.AddWithValue("@UserId", UserId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                List<GetRequestID> RequestIDs = new List<GetRequestID>();
                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GetRequestID RequestIDs1 = new GetRequestID();
                        RequestIDs1.RequestID = Convert.ToString(dt.Rows[i]["RequestID"]);
                        RequestIDs1.DateOfArrival = Convert.ToString(dt.Rows[i]["DateOfArrival"]);
                        RequestIDs1.ShiftTimeName = Convert.ToString(dt.Rows[i]["ShiftTimeName"]);
                        RequestIDs.Add(RequestIDs1);
                    }

                    StatusCode = 1;
                    Message = "Successfully Get";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = RequestIDs });
                }
                else
                {
                    StatusCode = 0;
                    Message = "An error occurs.";
                    return Request.CreateResponse(HttpStatusCode.OK, new { StatusCode = StatusCode, msg = Message, result = RequestIDs });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}