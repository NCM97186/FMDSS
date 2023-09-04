using FMDSS.APIInterface;
using FMDSS.APIModel;
using FMDSS.CustomModels.Models;
using FMDSS.Models;
using FMDSS.Models.MIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Repository
{
    public class ChatBotRepository : DAL, IChatBot
    {
        private DAL _db = new DAL();
        #region Get Details of Place 
        public DataTableResponse GetPlace()
        {
            DataTableResponse response = new DataTableResponse();

            try
            {


                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ChatBot", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetPlace");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                if (dt != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion


        public DataTableResponse District()
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_getDistrictsbystatecode", Conn); 
                cmd.CommandType = CommandType.StoredProcedure; 
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                if (dt != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }



        public DataTableResponse Zone(string PlaceID)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_GetzonebypalceId", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceID);
                cmd.CommandType = CommandType.StoredProcedure; 
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                if (dt != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }

        #region Get Details of Sift 
        public DataTableResponse GetSiftDetails()
        {
            DataTableResponse response = new DataTableResponse();

            try
            {


                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ChatBot", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetSiftdetails");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                if (dt != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion

        public DataTableResponse DropDownNursery(string DIST_CODE)
        {
            DataTableResponse response = new DataTableResponse();
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetNursery");
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt != null)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;
        }


        public DataSetResponse DropDownProduct(string DIST_CODE, string NURSERY_CODE)
        {
            DataSetResponse response = new DataSetResponse();
          
            try
            {
                DataSet ds = new DataSet();
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_ProduceListbyNurseryCode", Conn); 
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                cmd.Parameters.AddWithValue("@NURSERY_CODE", NURSERY_CODE); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);


                int rowCount = ds.Tables.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = ds;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = ds.Tables[0].Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;
        }


        #region Get Details of Sift 

        public DataTableResponse GetBookingDetails(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {


                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ChatBot", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "RemainingTicketForCitzn");
                cmd.Parameters.AddWithValue("@PlaceId", MIS.Place);
                cmd.Parameters.AddWithValue("@ShiftID", MIS.SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DateOfArrival", MIS.FromDate);
                //cmd.Parameters.AddWithValue("@USERID", USERID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion



        #region Get Details of Sift 
        public DataTableResponse GetTicketdetails(string SSOID)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {


                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ChatBot", Conn);
                cmd.Parameters.AddWithValue("@Action", "bookingdetails");
                cmd.Parameters.AddWithValue("@ssoid", SSOID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion


        #region Get TicketAvailability   
        //tej singh
        public DataTableResponse TicketAvailability(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            { 
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_ChatBot_Api", Conn);
                cmd.Parameters.AddWithValue("@Action", "RemainingTicketForCitzn");
                cmd.Parameters.AddWithValue("@PlaceId", MIS.Place);
                cmd.Parameters.AddWithValue("@ShiftID", MIS.SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@DateOfArrival", MIS.FromDate);
                cmd.Parameters.AddWithValue("@ZoneID", MIS.zone);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    if(rowCount==1)
                    {
                        response.CountOfVehicleType = 1;

                    }
                    else
                    {
                        response.CountOfVehicleType = 2;

                    }
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion


        #region Get TicketStatus   
        //tej singh
        public DataTableResponse TicketStatus(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_TicketStatus", Conn); 
                cmd.Parameters.AddWithValue("@RequestID", MIS.RequestID); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion
        #region Get bookingrefund   
        //tej singh
        public DataTableResponse bookingrefund(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_Bookingrefund", Conn);
                cmd.Parameters.AddWithValue("@RequestID", MIS.RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion

        

        #region Get BookingInformation   
        //tej singh
        public DataSetResponse BookingInformation(ChatBotModel MIS)
        {
            DataSetResponse response = new DataSetResponse();

            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_BookingInformation", Conn);
                cmd.Parameters.AddWithValue("@RequestID", MIS.RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                int rowCount = ds.Tables.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = ds;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = ds.Tables[0].Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion



        #region Get NurseryDetails   
        //tej singh
        public DataTableResponse NurseryDetails(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetDetailsNursery");
                cmd.Parameters.AddWithValue("@DIST_CODE", MIS.DIST_CODE);
                cmd.Parameters.AddWithValue("@NURSERY_CODE", MIS.NURSERY_CODE);
                cmd.Parameters.AddWithValue("@Id", MIS.NURSERY_Id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion

        #region Get NocStatus   
        //tej singh
        public DataTableResponse NocStatus(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_NocStatus", Conn); 
                cmd.Parameters.AddWithValue("@RequestedID", MIS.RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion

        #region Get NocStatus   
        //tej singh
        public DataSetResponse Nocdetails(ChatBotModel MIS)
        {
            DataSetResponse response = new DataSetResponse();

            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_GetNOCApplicationDetails", Conn);
                cmd.Parameters.AddWithValue("@RequestID", MIS.RequestID);
                cmd.Parameters.AddWithValue("@Status", MIS.Noc_Status);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                int rowCount = ds.Tables.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = ds;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = ds.Tables[0].Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion


        #region Get RearchNOC   
        //tej singh
        public DataSetResponse RearchNOC(ChatBotModel MIS)
        {
            DataSetResponse response = new DataSetResponse();

            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_RearchNOC", Conn);
                cmd.Parameters.AddWithValue("@RequestId", MIS.RequestID); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                int rowCount = ds.Tables.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = ds;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = ds.Tables[0].Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion



        #region Get PlantationModule   
        //tej singh
        public DataTableResponse PlantationModule(ChatBotModel MIS)
        {
            DataTableResponse response = new DataTableResponse();

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_GETPlantationmodule", Conn);
                cmd.Parameters.AddWithValue("@divname", MIS.divname);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion

        
        #region Get FAQ   
        //tej singh
        public DataTableResponse FAQ()
        {
            DataTableResponse response = new DataTableResponse();

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_Bookingfaq", Conn); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                int rowCount = dt.Rows.Count;
                if (rowCount > 0)
                {
                    response.Status = ResponseStatus.Success;
                    response.Data = dt;
                }
                else
                {
                    response.Status = ResponseStatus.Failed;
                    response.Message = dt.Rows[0]["Message"].ToString();

                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            return response;

        }
        #endregion

        #region  Checkotp 
        public DataSetResponse Checkotp(string SsoId, int OTP)
        {
            DataSetResponse response = new DataSetResponse();

            try
            {
                if (HttpContext.Current.Session["otp"] !=null)
                {
                    int userotp = Convert.ToInt32(HttpContext.Current.Session["otp"].ToString());
                    if (userotp == OTP)
                    {

                        cls_mobileLogin oLogin = new cls_mobileLogin();
                        DataSet oObjLoginData = oLogin.LoginMobileUser(SsoId);
                        if (Globals.Util.isValidDataSet(oObjLoginData, 0))
                        {
                            response.Status = ResponseStatus.Success;
                            response.Message = "Otp Check successfully";
                            response.Data = oObjLoginData;
                            

                            response.listItems = new List<Weboption> {
                            new Weboption { Id = 1, Name = "Booking Related" },
                            new Weboption { Id = 2, Name = "Plant purchase from Nursery" },
                            new Weboption { Id = 3, Name = "Forest Clearance and NOC" },
                            new Weboption { Id = 4, Name = "Plantation Monitoring" }
                            };

                            }
                    }
                    else
                    {
                        response.Status = ResponseStatus.Failed;
                        response.Message = "Otp not valid";

                        //var status = "Otp not valid";
                        //return Json(status, JsonRequestBehavior.AllowGet);

                    }

                }
                    else
                    {
                    response.Status = ResponseStatus.Failed;
                    response.Message = "Otp not valid";

                }

                   



            }
            catch (Exception ex)
            {
                response.Status = ResponseStatus.Failed;
                response.Message = ex.Message;
            }
             
            return response;

        }
        #endregion

        public bool GetOTP(string SsoId)
        {
            cls_mobileLogin oLogin = new cls_mobileLogin();
            bool isError = false;
            cls_UserOTPResponceCode model = new cls_UserOTPResponceCode();
            string returnMsg = string.Empty;
            var oObjLoginData = oLogin.GetOTP(SsoId);

            if (oObjLoginData.Rows.Count > 0)
            {
                model.Mobile = "8130223324";// Convert.ToString(oObjLoginData.Rows[0]["Mobile"]);
                model.Email = Convert.ToString(oObjLoginData.Rows[0]["EmailId"]);
                model.OTP = Convert.ToString(oObjLoginData.Rows[0]["OTP"]);
                model.Status = Convert.ToString(oObjLoginData.Rows[0]["Status"]);
                model.UserSmsBody = string.Format("{0} is the One Time Password(OTP) to process, expires in 2 mins. Verify now ", model.OTP);

                SMS_EMail_Services.sendSingleSMS(Convert.ToString(model.Mobile), model.UserSmsBody);
                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();

                objSMSandEMAILtemplate.SendMailComman("ALL", "GET_OTP_FOR_MOBILE", model.UserSmsBody, model.UserSmsBody, model.Email, "", "");
               HttpContext.Current.Session["otp"] = Convert.ToInt32(model.OTP);
                var STATUS = true;
                return STATUS;
            }
            else
            {

                return isError;

            }

            // return Json(new { Status= Status }, JsonRequestBehavior.AllowGet);
        } 


    }
}
