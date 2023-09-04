using FMDSS.APIHelpers;
using FMDSS.CustomModels.Models;
using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.BookOnlineZoo;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.APIDAL
{
    public class ZooBookingDAL
    {
        public static DataSet ZooBookingPlaces(string Conn = null)
        {
            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.GetZooPlaces,
                new SqlParameter("@Option", 1));
        }

        public static List<ShiftModel> GetShift(string Conn = null)
        {
            List<ShiftModel> shiftlst = new List<CustomModels.Models.ShiftModel>();
            shiftlst.AddRange(new List<ShiftModel> { new ShiftModel { Text = "--Select--", Value = 0 }, new ShiftModel { Text = "Full Day", Value = 3 } });
            return shiftlst;
        }

        public static DataSet ChkTicketAvailability(int PlaceId, string ShiftType, string DateofVisit, string KioskUserId, string Conn = null)
        {
            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.CheckTicketAvailability,
                new SqlParameter("@PlaceId", PlaceId),
                new SqlParameter("@ShiftType", ShiftType),
                new SqlParameter("DateofVisit", DateTime.ParseExact(DateofVisit, "dd/MM/yyyy", null)),
                new SqlParameter("@KioskUserId", Convert.ToInt64(KioskUserId))
                );
        }

        public static DataSet MemberAndVehicleFeelst(int PlaceId, string KioskUserId, string Conn = null)
        {
            ////TEST
            FinalSubmitModel model = new FinalSubmitModel();
            // model.RequestId = "1";
            // model.AddressOfInstitOrgan = "test";
            // model.AdultIndianMember = false;
            // model.AdultNonIndianMember = false;
            model.BookingType = "Emitra-Plus";
            // model.CameraFees = "20";
            // model.ChildBelowAgeFive = false;
            model.DateOfVisit = "14/11/2018";
            model.DocumentForTour = "";
            model.DocumentForTourImageName = "No-Image-JPG";
            // model.FeePerVehicle = "20";
            model.IdNumber = "1";
            model.IdType = "Passport";
            model.UploadId = "";
            model.InstituteOrganisationName = "InstituteOrganisationName";
            model.IPAddress = "10.68.128.101";
            model.IPAddressAndDeviceKey = "123456";

            // model.KioskUserId = "amit17rajput";
            // model.MemberEntryFees = "200";
            model.NameOfHead = "Test Head";
            // model.NoOfAutoRikshaw = "0";
            // model.NoOfBus = "0";
            // model.NoOfJeepCarMotorMiniBus = "0";
            // model.NoOfTwoWheeler = "0";
            // model.NoOfVehicle = "0";
            // model.OnlineBookingCharges = "0";
            // model.PhoneNoOfInstitOrgan = "0";
            // model.PhoneOfHead = "0";
            model.PlaceOfVisit = "58";
            model.PrivateVehicle = true;
            //model.RequestId = "--";
            model.ShiftTypeID = "3";
            model.ShiftTypeName = "FULL DAY";
            model.SsoId = "amit17rajput";
            //model.Student = false;
            //model.TotalFeesOfAutoRikshaw = "0";
            //model.TotalFeesOfBus = "0";
            //model.TotalFeesOfJeepCarMotorMiniBus = "0";
            //model.TotalFeesOfTwoWheeler = "0";
            //model.TotalPayableCharges = "200";
            //model.TotalVehicleFee = "200";


            model.lstMember = new List<GetMemberFeelstModel> { new GetMemberFeelstModel { FeeChargedOn = "Indian Visitors", HeadAmount = 50, NoOfMember = 1, StillCameraAmount = 200, NoOfStillCamera = 1, VideoCameraAmount = 500, NoOfVideoCamera = 1, TotalFeesOfMember = 750 } };
            model.lstVehicle = new List<GetVehicleFeelstModel> { new GetVehicleFeelstModel { FeeChargedOn = "Auto Rikshaw", HeadAmount = 60, NoOfVehicle = 1, TotalVehicleFee = 60 } };

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            ////End Test




            if (string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.GetMemberAndVehicleFeelst,
                new SqlParameter("@PlaceId", PlaceId),
                new SqlParameter("@option", 2)
                );
        }

        public static DataSet SubmitZooBooking(FinalSubmitModel submitZooBooking, string Conn = null)
        {
            DataSet ds = new DataSet();
            try
            {
                if (string.IsNullOrEmpty(Conn))
                {
                    Conn = ConnectionString.Conn;
                }

                DataTable dtMember = MemberInformation(submitZooBooking.lstMember);
                DataTable dtVehicle = VehicleInformation(submitZooBooking.lstVehicle);
                System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");
                DataTable dt = new DataTable();

                ds = SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.FinalSubmit,
                 new SqlParameter("@BookingTypeId", submitZooBooking.BookingType),
                 new SqlParameter("@PlaceId", submitZooBooking.PlaceOfVisit),
                 new SqlParameter("@ShiftTypeID", submitZooBooking.ShiftTypeID),
                 new SqlParameter("@Institutional_NameofInstitute", submitZooBooking.InstituteOrganisationName),
                 new SqlParameter("@Institutional_AddressofInstitute", submitZooBooking.AddressOfInstitOrgan),
                 new SqlParameter("@Institutional_PhoneofInstitute", submitZooBooking.PhoneNoOfInstitOrgan),
                 new SqlParameter("@Institutional_NameofHead", submitZooBooking.NameOfHead),
                 new SqlParameter("@Institutional_HeadPhoneNo", submitZooBooking.PhoneOfHead),
                 new SqlParameter("@Institutional_DocumentofTour", submitZooBooking.DocumentForTour),
                 new SqlParameter("@HeadIdType", submitZooBooking.IdType),
                 new SqlParameter("@HeadIdNumber", submitZooBooking.IdNumber),
                 new SqlParameter("@Institutional_IDProfofGroupHead", submitZooBooking.UploadId),//submitZooBooking.UploadId),                       
                 new SqlParameter("@Institutional_DateofVisit", Convert.ToDateTime(submitZooBooking.DateOfVisit, cul)),
                 new SqlParameter("@PrivateVehicle", submitZooBooking.PrivateVehicle),
                 new SqlParameter("@MemberDetail", dtMember),
                 new SqlParameter("@VehicleDetail", dtVehicle),
                 new SqlParameter("@IP_Address", submitZooBooking.IPAddress),
                 new SqlParameter("@EnteredBy", GetUserIDbySsoId(submitZooBooking.SsoId)),
                 new SqlParameter("@kioskUserId", Convert.ToInt64(submitZooBooking.KioskUserId)),
                 new SqlParameter("@TicketType", "Emitra-Plus")
                 );



            }
            catch (Exception ex)
            {


            }
            return ds;
            //return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.GetMemberAndVehicleFeelst,
            //    new SqlParameter("@PlaceId", PlaceId),
            //    new SqlParameter("@option", 2)
            //    );

        }



        private static DataTable MemberInformation(List<GetMemberFeelstModel> lstMemberInfo)
        {

            Int64 UserID = 0;
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region MemberInfo
                objDt2.Columns.Add("TypeOfMember", typeof(String));
                objDt2.Columns.Add("NoOfMember", typeof(String));
                objDt2.Columns.Add("FeePerMember", typeof(String));
                objDt2.Columns.Add("NoOfStillCamera", typeof(String));
                objDt2.Columns.Add("FeePerStillCamera", typeof(String));
                objDt2.Columns.Add("NoOfVideoCamera", typeof(String));
                objDt2.Columns.Add("FeePerVideoCamera", typeof(String));

                objDt2.Columns.Add("TotalFees", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfMember != 0 && item.NoOfMember != 0 && item.TotalFeesOfMember != 0 && item.TotalFeesOfMember != 0 && item.TotalFeesOfMember != 0)
                    {
                        dr["TypeOfMember"] = item.TypeOfMember;
                        dr["NoOfMember"] = item.NoOfMember;
                        dr["FeePerMember"] = Convert.ToInt32(item.HeadAmount);
                        dr["NoOfStillCamera"] = Convert.ToInt32(item.NoOfStillCamera);
                        dr["FeePerStillCamera"] = Convert.ToInt32(item.StillCameraAmount);
                        dr["NoOfVideoCamera"] = Convert.ToInt32(item.NoOfVideoCamera);
                        dr["FeePerVideoCamera"] = Convert.ToInt32(item.VideoCameraAmount);

                        dr["TotalFees"] = ((Convert.ToDecimal(item.HeadAmount) * Convert.ToDecimal(item.NoOfMember))
                            + (Convert.ToDecimal(item.StillCameraAmount) * Convert.ToDecimal(item.NoOfStillCamera))
                            + (Convert.ToDecimal(item.VideoCameraAmount) * Convert.ToDecimal(item.NoOfVideoCamera)));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                    else
                    {
                        if (item.TypeOfMember == "Child Below Age of 5 Years" && item.NoOfMember != 0)
                        {
                            dr["TypeOfMember"] = item.TypeOfMember;
                            dr["NoOfMember"] = item.NoOfMember;
                            dr["FeePerMember"] = item.HeadAmount;
                            dr["NoOfStillCamera"] = item.NoOfStillCamera;
                            dr["FeePerStillCamera"] = item.StillCameraAmount;
                            dr["NoOfVideoCamera"] = item.NoOfVideoCamera;
                            dr["FeePerVideoCamera"] = item.VideoCameraAmount;

                            dr["TotalFees"] = ((Convert.ToDecimal(item.HeadAmount) * Convert.ToDecimal(item.NoOfMember))
                                + (Convert.ToDecimal(item.StillCameraAmount) * Convert.ToDecimal(item.NoOfStillCamera))
                                + (Convert.ToDecimal(item.VideoCameraAmount) * Convert.ToDecimal(item.NoOfVideoCamera)));
                            objDt2.Rows.Add(dr);
                            objDt2.AcceptChanges();
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }

        private static DataTable VehicleInformation(List<GetVehicleFeelstModel> lstVehicleInfo)
        {
            DataTable objDt2 = new DataTable("Table");
            try
            {
                #region Vehicle Info
                objDt2.Columns.Add("VehicleType", typeof(String));
                objDt2.Columns.Add("FeePerVehicle", typeof(String));
                objDt2.Columns.Add("NoOfVehicle", typeof(String));
                objDt2.Columns.Add("TotalVehicleFee", typeof(String));
                objDt2.AcceptChanges();
                foreach (var item in lstVehicleInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.NoOfVehicle != 0 && item.TotalVehicleFee != 0)
                    {
                        dr["VehicleType"] = item.FeeChargedOn;
                        dr["FeePerVehicle"] = Convert.ToInt32(item.HeadAmount);
                        dr["NoOfVehicle"] = Convert.ToInt32(item.NoOfVehicle);
                        dr["TotalVehicleFee"] = (Convert.ToDecimal(item.NoOfVehicle) * Convert.ToDecimal(item.HeadAmount));
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 0, DateTime.Now, UserID);
            }
            return objDt2;
        }


        private static long GetUserIDbySsoId(string sSoId)
        {
            BookOnZoo Zoo = new BookOnZoo();
            long UserId = Zoo.GetUserIDbySsoId(sSoId);
            return UserId;
        }

        public static DataTable UpdateZooBooking(UpdateZooTicketRequest UpdateZooBooking) //string MERCHANTCODEs, string PRNs, string STATUSs, string ENCDATAs, string RequestId, string TotalPrice, string Ssoid)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            GetZooTicketResponceSubmitModel ObjPaymentStatus = new GetZooTicketResponceSubmitModel();
            long UserId = GetUserIDbySsoId(UpdateZooBooking.Model.Ssoid);
            //  Session["UserID"] = GetUserIDbySsoId(UpdateZooBooking.Model.Ssoid);


            string TotalPrice = UpdateZooBooking.Model.TotalPrice;
            string RequestId = UpdateZooBooking.Model.RequestId;
            string MERCHANTCODEs = UpdateZooBooking.Model.MERCHANTCODEs;
            string PRNs = UpdateZooBooking.Model.PRNs;
            string STATUSs = UpdateZooBooking.Model.STATUSs;
            string ENCDATAs = UpdateZooBooking.Model.ENCDATAs;
            string ssoId = UpdateZooBooking.Model.Ssoid;
            string EmitraCharge = UpdateZooBooking.Model.EmitraCharge;
            string PaymentMode = UpdateZooBooking.Model.PaymentMode;

            if (RequestId != null && RequestId != string.Empty)
            {
                try
                {
                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";
                    if (MERCHANTCODEs != null)
                        MERCHANTCODE = MERCHANTCODEs.ToString();
                    if (PRNs != null)
                        PRN = PRNs.ToString();
                    if (STATUSs != null)
                        STATUS = STATUSs.ToString();
                    if (ENCDATAs != null)
                        ENCDATA = ENCDATAs.ToString();

                    //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");
                    //string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);

                    ENCData ObjPGResponse = JsonConvert.DeserializeObject<ENCData>(ENCDATA);
                    BookOnTicket cs1 = new BookOnTicket();
                    cs1.UpdateEmitraResponseForApi(RequestId.ToString(), "ZooTicketBooking", ENCDATA, UserId);
                    int status1 = 0;
                    BookOnZoo cs = new BookOnZoo();
                    FMDSS.Models.CitizenService.PermissionService.Payment pay = new Models.CitizenService.PermissionService.Payment();

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION STATUS");
                        dt.Columns.Add("REQUEST ID");
                        dt.Columns.Add("EMITRA TRANSACTION ID");
                        dt.Columns.Add("TRANSACTION TIME");
                        dt.Columns.Add("TRANSACTION AMOUNT");
                        dt.Columns.Add("EMITRA AMOUNT");
                        dt.Columns.Add("USER NAME");
                        dt.Columns.Add("TRANSACTION BANK DETAILS");
                        dt.Columns.Add("ZooTicketHTML");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion
                    #region Response Status
                    if (STATUS == "FAILED")
                    {
                        DataRow dtrow = dt.NewRow();
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = PRN;
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(STATUS);
                        dtrow["REQUEST ID"] = PRN;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.TRANSAMT;
                        dtrow["EMITRA AMOUNT"] = EmitraCharge;
                        dtrow["USER NAME"] = ssoId;
                        if (dtrow["TRANSACTION STATUS"].ToString() == "FAILED")
                        {
                            cs.Trn_Status_Code = 0;
                            //cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));
                            cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.TRANSAMT), Convert.ToDouble(EmitraCharge));
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (ObjPGResponse.TRANSACTIONSTATUS == "SUCCESS")
                    {

                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = ObjPGResponse.TRANSAMT;
                        cs.RequestId = RequestId.ToString();
                        dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(ObjPGResponse.TRANSACTIONSTATUS);
                        dtrow["REQUEST ID"] = ObjPGResponse.REQUESTID;
                        dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.TRANSAMT;
                        dtrow["EMITRA AMOUNT"] = Convert.ToDecimal(ObjPGResponse.TRANSAMT) - Convert.ToDecimal(EmitraCharge);
                        dtrow["USER NAME"] = ssoId; // use as user name
                        dtrow["TRANSACTION BANK DETAILS"] = PaymentMode;

                        if (dtrow["TRANSACTION STATUS"].ToString() == "SUCCESS")
                        {
                            if (Convert.ToString(RequestId).Equals(ObjPGResponse.REQUESTID) && (TotalPrice != null && Convert.ToDecimal(TotalPrice) == Convert.ToDecimal(ObjPGResponse.TRANSAMT)))
                            {
                                cs.Trn_Status_Code = 1;
                                status1 = 1;
                                cs.UpdateTransactionStatus("1", Convert.ToDouble(EmitraCharge), Convert.ToDouble(ObjPGResponse.TRANSAMT));
                                Int64 ZooTicketId = cs.GetZooTicketId(ObjPGResponse.REQUESTID);

                                string PrintHTML = PrintTicket(ZooTicketId);

                                SendZooBookingEmailSMS(RequestId);
                            }
                            else // Added to save mismatch in payment
                            {
                                cs.Trn_Status_Code = 0;
                                status1 = 0;
                                cs.UpdateTransactionStatus("1", Convert.ToDouble(EmitraCharge), Convert.ToDouble(ObjPGResponse.TRANSAMT));
                            }
                        }

                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                }
                catch (Exception ex)
                {

                }
            }
            return dt;
        }

        public static DataTable UpdateZooBooking_EmitraPlus(UpdateZooTicketRequest_EmitraPlus UpdateZooBooking) //string MERCHANTCODEs, string PRNs, string STATUSs, string ENCDATAs, string RequestId, string TotalPrice, string Ssoid)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            UpdateZooTicketResponce response = new UpdateZooTicketResponce();

            GetZooTicketResponceSubmitModel ObjPaymentStatus = new GetZooTicketResponceSubmitModel();
            long UserId = GetUserIDbySsoId(UpdateZooBooking.Ssoid);
            //  Session["UserID"] = GetUserIDbySsoId(UpdateZooBooking.Model.Ssoid);

            string RequestId = "";
            string PRNs = "";
            string TotalPrice = UpdateZooBooking.TotalPrice;
            //string RequestId = UpdateZooBooking.Model.RequestId;
            string MERCHANTCODEs = UpdateZooBooking.MERCHANTCODEs;
            //string PRNs = UpdateZooBooking.Model.PRNs;
            string STATUSs = UpdateZooBooking.STATUSs;
            //string ENCDATAs = UpdateZooBooking.Model.ENCDATAs;
            string ssoId = UpdateZooBooking.Ssoid;
            string EmitraCharge = UpdateZooBooking.EmitraCharge;
            string PaymentMode = UpdateZooBooking.PaymentMode;
            RequestId = UpdateZooBooking.ENCDATAs.REQUESTID;
            PRNs = UpdateZooBooking.ENCDATAs.REQUESTID;


            if (RequestId != null && RequestId != string.Empty)
            {
                try
                {
                    string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";
                    if (MERCHANTCODEs != null)
                        MERCHANTCODE = MERCHANTCODEs.ToString();
                    if (PRNs != null)
                        PRN = PRNs.ToString();
                    if (STATUSs != null)
                        STATUS = STATUSs.ToString();
                    if (UpdateZooBooking.ENCDATAs != null)
                        ENCDATA = Convert.ToString(UpdateZooBooking.ENCDATAs);

                    //EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("N@FOREST#4*23");
                    //string DecryptedData = objEncryptDecrypt.decrypt(ENCDATA);

                    //ENCData ObjPGResponse = JsonConvert.DeserializeObject<ENCData>(ENCDATA);

                    ENCDATAs ObjPGResponse = new ENCDATAs();
                    ObjPGResponse.REQUESTID = UpdateZooBooking.ENCDATAs.REQUESTID;
                    ObjPGResponse.TRANSACTIONID = UpdateZooBooking.ENCDATAs.TRANSACTIONID;
                    ObjPGResponse.TRANSACTIONSTATUS = UpdateZooBooking.ENCDATAs.TRANSACTIONSTATUS;
                    ObjPGResponse.TRANSACTIONSTATUSCODE = UpdateZooBooking.ENCDATAs.TRANSACTIONSTATUSCODE;
                    ObjPGResponse.EMITRATIMESTAMP = UpdateZooBooking.ENCDATAs.EMITRATIMESTAMP;
                    ObjPGResponse.TRANSAMT = UpdateZooBooking.ENCDATAs.TRANSAMT;

                    BookOnTicket cs1 = new BookOnTicket();

                    cs1.UpdateEmitraResponseForApi(RequestId.ToString(), "ZooTicketBooking", ENCDATA, UserId);

                    int status1 = 0;
                    BookOnZoo cs = new BookOnZoo();
                    DataTable cs_exist = new DataTable();

                    FMDSS.Models.CitizenService.PermissionService.Payment pay = new Models.CitizenService.PermissionService.Payment();

                    // string res = pay.RequestString(MERCHANTCODEs, RequestId, TotalPrice, "", Convert.ToString(UserId), PRN, PRN);

                    #region Datarow defination
                    if (dt.Rows.Count == 0)
                    {
                        dt.Columns.Add("TRANSACTION_STATUS");
                        dt.Columns.Add("REQUEST_ID");
                        dt.Columns.Add("EMITRA_TRANSACTION_ID");
                        dt.Columns.Add("TRANSACTION_TIME");
                        dt.Columns.Add("TRANSACTION_AMOUNT");
                        dt.Columns.Add("EMITRA_AMOUNT");
                        dt.Columns.Add("USER_NAME");
                        dt.Columns.Add("TRANSACTION_BANK_DETAILS");
                        dt.Columns.Add("ZooTicketHTML");
                        dt.Columns.Add("Message");
                        dt.Columns.Add("Status");
                        dt.Columns.Add("HeadeText");
                        dt.Columns.Add("FooterText");
                        //dt.Columns.Add("TRANSACTION BANK BID");                    
                    }
                    #endregion
                    #region Response Status
                    if (STATUS == "FAILED")
                    {
                        DataRow dtrow = dt.NewRow();
                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = PRN;
                        //dtrow["TRANSACTION STATUS"] = pay.TransactionStatus(STATUS);
                        //dtrow["REQUEST ID"] = PRN;
                        //dtrow["EMITRA TRANSACTION ID"] = ObjPGResponse.TRANSACTIONID;
                        //dtrow["TRANSACTION TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        //dtrow["TRANSACTION AMOUNT"] = ObjPGResponse.TRANSAMT;
                        //dtrow["EMITRA AMOUNT"] = EmitraCharge;
                        //dtrow["USER NAME"] = ssoId;

                        dtrow["TRANSACTION_STATUS"] = "Transaction Failed";
                        dtrow["REQUEST_ID"] = PRN;
                        dtrow["EMITRA_TRANSACTION_ID"] = "";
                        dtrow["TRANSACTION_TIME"] = "";
                        dtrow["TRANSACTION_AMOUNT"] = "";
                        dtrow["EMITRA_AMOUNT"] = "";
                        dtrow["USER_NAME"] = ssoId;
                        dtrow["Message"] = "Transaction Failed";
                        dtrow["Status"] = 0;
                        dtrow["HeadeText"] = "";
                        dtrow["FooterText"] = "";
                        if (dtrow["TRANSACTION_STATUS"].ToString() == "FAILED")
                        {
                            dtrow["Message"] = "Transaction Failed";
                            dtrow["Status"] = 0;
                            response.Message = "Transaction Failed";
                            response.Status = 0;
                            cs.Trn_Status_Code = 0;
                            //cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.PAIDAMOUNT), Convert.ToDouble(ObjPGResponse.AMOUNT));
                            cs.UpdateTransactionStatus("1", Convert.ToDouble(ObjPGResponse.TRANSAMT), Convert.ToDouble(EmitraCharge));
                        }
                        dt.Rows.Add(dtrow);
                    }
                    else if (STATUS == "SUCCESS")
                    {

                        DataRow dtrow = dt.NewRow();

                        cs.EmitraTransactionId = ObjPGResponse.TRANSACTIONID;
                        cs.RequestId = RequestId.ToString();

                        dtrow["TRANSACTION_STATUS"] = pay.TransactionStatus(ObjPGResponse.TRANSACTIONSTATUS);
                        dtrow["REQUEST_ID"] = ObjPGResponse.REQUESTID;
                        dtrow["EMITRA_TRANSACTION_ID"] = ObjPGResponse.TRANSACTIONID;
                        dtrow["TRANSACTION_TIME"] = ObjPGResponse.EMITRATIMESTAMP;
                        dtrow["TRANSACTION_AMOUNT"] = ObjPGResponse.TRANSAMT;
                        // dtrow["EMITRA_AMOUNT"] = Convert.ToDecimal(ObjPGResponse.TRANSAMT) - Convert.ToDecimal(EmitraCharge);
                        decimal AmountTobepayForEmitra = Convert.ToDecimal(ObjPGResponse.TRANSAMT) + Convert.ToDecimal(EmitraCharge);
                        dtrow["EMITRA_AMOUNT"] = Convert.ToDecimal(EmitraCharge);
                        dtrow["USER_NAME"] = ssoId;
                        dtrow["TRANSACTION_BANK_DETAILS"] = PaymentMode;
                        dtrow["Message"] = "";
                        dtrow["ZooTicketHTML"] = "";

                        if (Convert.ToString(dtrow["TRANSACTION_STATUS"]) == "SUCCESS")
                        {
                            if (Convert.ToString(RequestId).Equals(ObjPGResponse.REQUESTID) && (TotalPrice != null && Convert.ToDecimal(TotalPrice) == Convert.ToDecimal(ObjPGResponse.TRANSAMT)))
                            {
                                cs.Trn_Status_Code = 1;
                                status1 = 1;
                                cs_exist = cs.CheckRequestIDExist(Convert.ToString(RequestId));

                                if (cs_exist.Rows.Count > 0)
                                {

                                    dtrow["HeadeText"] = Convert.ToString(cs_exist.Rows[0]["HeadeText"]);
                                    dtrow["FooterText"] = Convert.ToString(cs_exist.Rows[0]["FooterText"]);

                                    cs.UpdateTransactionStatus("1", Convert.ToDouble(AmountTobepayForEmitra), Convert.ToDouble(EmitraCharge));

                                    Int64 ZooTicketId = cs.GetZooTicketId(ObjPGResponse.REQUESTID);

                                    if (ZooTicketId != 0)
                                    {
                                        string PrintHTML = PrintTicket(ZooTicketId);
                                        SendZooBookingEmailSMS(RequestId);
                                        dtrow["ZooTicketHTML"] = PrintHTML;
                                        dtrow["Message"] = "Success";
                                        dtrow["Status"] = 1;
                                    }
                                    else
                                    {
                                        dtrow["Message"] = "Zoo Ticket ID not found.";
                                        dtrow["Status"] = 0;
                                        response.Status = 0;
                                        cs.Trn_Status_Code = 0;
                                        status1 = 0;
                                    }
                                }
                                else
                                {
                                    dtrow["Message"] = "Request ID is already exists.";
                                    dtrow["Status"] = 0;
                                    response.Status = 0;
                                    cs.Trn_Status_Code = 0;
                                    status1 = 0;
                                }

                            }
                            else // Added to save mismatch in payment
                            {

                                dtrow["Message"] = "Mismatch in Payment";
                                dtrow["Status"] = 0;
                                response.Status = 0;
                                cs.Trn_Status_Code = 0;
                                status1 = 0;
                                cs.UpdateTransactionStatus("1", Convert.ToDouble(AmountTobepayForEmitra), Convert.ToDouble(EmitraCharge));
                            }
                        }

                        dt.Rows.Add(dtrow);
                    }
                    #endregion

                }
                catch (Exception ex)
                {

                }
            }
            return dt;
        }

        private static string PrintTicket(Int64 ticketid)
        {
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //Int64 UserID = Convert.ToInt64(Session["UserID"].ToString());
            try
            {
                DataSet ds = new DataSet();
                BookOnZoo cs = new BookOnZoo();
                cs.TicketID = ticketid;
                ds = cs.Select_TicketData_For_Print();

                if (ds.Tables.Count > 0)
                {
                    sb.Append("<section class=print-invoice>");
                    sb.Append("<div class=panel panel-default> <div class=panel-body> <div id=tbl_unbold class=table-responsive> ");

                    sb.Append("<table class=table>  <thead><tr style=text-align:center> <th style=text-align:center>" + ds.Tables[0].Rows[0]["HeadeText"].ToString() + "</th></tr></thead></table></div>");

                    sb.Append("<div id=tbl_unbold class=table-responsive><table class=table table-striped><thead>");
                    sb.Append("<tr> <th>Ticket No:" + ds.Tables[0].Rows[0]["RequestId"].ToString() + "</th>");
                    sb.Append("<th style=text-align:right>Date Of Visit : " + ds.Tables[0].Rows[0]["DateOfVisit"].ToString() + "</th></tr>");

                    sb.Append("<tr> <th>Shift : " + ds.Tables[0].Rows[0]["ShiftType"].ToString() + "</th>");
                    sb.Append("<th style=text-align:right> </th></tr>");

                    if (ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() != "")
                    {
                        sb.Append("<tr><th>Issued in Favor of:" + ds.Tables[0].Rows[0]["Institutional_NameofHead"].ToString() + "</th><th style=text-align:right> ID Number:" + ds.Tables[0].Rows[0]["Institutional_HeadIdNumber"].ToString() + "</th></tr>");
                    }
                    sb.Append("</thead><tbody></tbody></table>");
                    sb.Append("</div>  " +
                     "                               <div id=tbl_unbold class=table-responsive>  " +
                     "                                   <table class=table table-striped table-bordered table-hover>  " +
                     "                                       <thead>  " +
                     "                                           <tr>  " +
                     "                                               <th></th>  " +
                     "                                               <th>Rate(INR)</th>  " +
                     "                                               <th>Qty</th>  " +
                     "                                               <th>Amount(INR)</th>  " +
                     "                                           </tr>  " +
                     "                                       </thead>  " +
                     "                                       <tbody>  ");

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        int j = i + 1;
                        sb.Append("<tr><td>" + ds.Tables[1].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["FeePerMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["NoOfMember"].ToString() + "</td><td>" + ds.Tables[1].Rows[i]["TotalMemberFees"].ToString() + "</td></tr>");
                    }
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        int j = i + 1;
                        sb.Append("<tr><td>" + ds.Tables[2].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["FeePerStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["NoOfStillCamera"].ToString() + "</td><td>" + ds.Tables[2].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                    }

                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        int j = i + 1;
                        sb.Append("<tr><td>" + ds.Tables[3].Rows[i]["TypeOfMember"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["FeePerVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["NoOfVideoCamera"].ToString() + "</td><td>" + ds.Tables[3].Rows[i]["TotalCameraFees"].ToString() + "</td></tr>");
                    }

                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        int j = i + 1;
                        sb.Append("<tr><td>" + ds.Tables[4].Rows[i]["VehicleType"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["FeePerVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["NoOfVehicle"].ToString() + "</td><td>" + ds.Tables[4].Rows[i]["TotalVehicleFees"].ToString() + "</td></tr>");
                    }

                    //sb.Append("<tr><td>Emitra Amount</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td><td>-</td><td>" + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</td></tr>");

                    sb.Append("</tbody>  " +
                    "                                   </table>  " +
                    "                               </div>  " +
                    "                               <!-- /.Table3 -->  " +
                    "     " +
                    "                               <!-- Table4 -->  " +
                    "                               <div id=tbl_unbold class=table-responsive>  " +
                    "                                   <table class=table style=text-align: right>  " +
                    "                                       <thead>  " +
                    "                                           <tr>  " +
                    //  "                                               <th style=text-align: right>Grand Total: " + ds.Tables[5].Rows[0][0].ToString() + "</th>  " +
                      "                                               <th style=text-align: right>Grand Total: " + ds.Tables[5].Rows[0]["EmitraCharges"].ToString() + "</th>  " +
                    "                                           </tr>  " +
                    "                                       </thead>  " +
                    "                                   </table>  " +
                    "                               </div>  ");

                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["PlaceID"].ToString()) == 60)
                    {
                        sb.Append(
             "                               <div id=tbl_unbold class=table-responsive>  " +
                        "                                   <p><b>Terms & Conditions</b> <ul>" +

//  "Terms and conditions for Visitors : "+
"<li> The visitor or his / her representative must reach to the Forest Jungle Safari booking centre to collect the boarding pass, at least 45 minutes prior to the entry time. </li> " +
"<li> The Id proof of visitor produced at the time of collecting boarding pass should be the Id used while booking online ticket, failing which, the ticket will be deemed fake and liable to be cancelled.The visitor must also carry copy of such Id while visiting the Park. </li>" +
"<li> The charges deposited during online booking include tourist entry fee, vehicle entry fee, vehicle rent, and online booking charges and applicable taxes. </li>" +
"<li> In Case of Online booking all the 6 seats are required to be filled or else the costumer has to pay difference amount for going inside the park. </li>" +
"<li> The visitor must bring two copies of confirmation slip at the time of collecting boarding pass.One copy will be deposited in the office and the other copy will be carried by the visitor. </li>" +
"<li> Seats remaining vacant due to non - turn up of any visitors would be filled by the park management at the booking window. </li>" +
"<li> Boarding pass shall be collected from the booking office Jhalana Safari Park during 5.00 PM to 6.00 PM of previous evening for the next day morning safari. </li>" +
"<li> In case of group booking, park authorities will try to adjust the group together in vehicles subject to space availability at the time of entry.  </li>" +
"<li> Boarding Pass will be issued at: Jhalana Safari Booking Office, Jaipur </li>" +
"<li> For cancellations made 30 days or more in advance from the date of safari, fifty percent amount would be refunded through the channel the booking was made. </li>" +
"<li> In case of any changes in applicable Fees &Tax Rates, the difference amount shall be collected at the time of Boarding / entry of the park </li>" +
                        "                                          </ul> </p></div>  ");

                    }
                    sb.Append(
                   "                               <div class=print-bg-tkt>  " +
                    "                                   <div class=centr>  " + ds.Tables[0].Rows[0]["FooterText"].ToString() +
                    //                                           "Visit Timing </br>"+
                    //                                           "Winter (15 oct to 14 March 9:00 AM to  5:00 PM) </br>"+
                    //                                           "Summer(15 March to 14 Oct 8:30 AM to  5:30 PM) </br>"+
                    //"                                       This ticket is valid for bird section also.<br />  " +
                    //"                                      Do not tease the animals.<br />  " +
                    //"                                       Thanks visit again.  " +
                    "                                   </div>  " +
                    "                               </div>  " +
                    "                   <!-- /.Footer -->     " +
                    "                   " +
                    "                         </div>  " +
                    "                           <!-- /.panel-body -->  " +
                    "                             " +
                    "                  </div>  " +
                    "              <!-- /.panel -->  " +
                    "          " +
                    "          </section>");


                    htmlToPdfDownloadTickets.ZooDownloadPdfForEmitraPlus(ds);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Emitra Plus", 1, DateTime.Now, 1);
            }
            return sb.ToString();
        }


        private static void SendZooBookingEmailSMS(string ZooBookingId)
        {
            //Send_ZooEmailSMS obj = new Send_ZooEmailSMS();
            try
            {
                #region  after SUCCESS flag send SMS and Email to the user // code by Arvind Kumar Sharma

                SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
                SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();

                string body = string.Empty;
                DataTable DT = objSMSandEMAILtemplate.GetUserDetailsForZoo(ZooBookingId);
                if (DT.Rows.Count > 0)
                {
                    if (Convert.ToString(DT.Rows[0]["EmailId"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ZooTicketEmailTemplate"].ToString()));
                        objSMS_EMail_Services.sendEMail("Zoo Ticket Booking for RequestID : " + Convert.ToString(DT.Rows[0]["RequestID"]), body, Convert.ToString(DT.Rows[0]["EmailId"]), ConfigurationManager.AppSettings["ZooTicketEmail_CC"].ToString());
                        body = string.Empty;

                    }

                    if (Convert.ToString(DT.Rows[0]["Mobile"]) != string.Empty)
                    {
                        body = objSMSandEMAILtemplate.WildLifeTicketEmailTemplate(Convert.ToString(DT.Rows[0]["NAME"]), Convert.ToString(DT.Rows[0]["PlaceName"]), Convert.ToString(DT.Rows[0]["RequestID"]), System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ZooTicketSMSTemplate"].ToString()));

                        SMS_EMail_Services.sendSingleSMS(Convert.ToString(DT.Rows[0]["Mobile"]), body);

                        body = string.Empty;
                    }

                }


                #endregion


            }



            catch (Exception ex)
            {
                //return false;
            }

        }



    }
}


