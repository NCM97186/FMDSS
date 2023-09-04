using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FMDSS.Globals;
using System.IO;

namespace FMDSS.Models.BookOnlineZoo
{
    public class BookOnZoo : DAL
    {

        public string BookingType { get; set; }
        public string RequestId { get; set; }

        [Required(ErrorMessage = "Enter Institute/Organisation name")]
        public string InstituteOrganisationName { get; set; }

        [Required(ErrorMessage = "Enter address of institute/organisation")]
        public string AddressOfInstitOrgan { get; set; }

        [Required(ErrorMessage = "Enter phone no of institute/organisation")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNoOfInstitOrgan { get; set; }

        [Required(ErrorMessage = "Enter name of head")]
        public string NameOfHead { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneOfHead { get; set; }

        [Required(ErrorMessage = "Upload document of tour")]
        public string DocumentForTour { get; set; }

        [Required(ErrorMessage = "Select type of Id")]
        public string IdType { get; set; }

        [Required(ErrorMessage = "Enter Id Number")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Upload valid Id")]
        public string UploadId { get; set; }

        [Required(ErrorMessage = "Select place of visit")]
        public string PlaceOfVisit { get; set; }

        [Required(ErrorMessage = "Select date of visit")]
        public string DateOfVisit { get; set; }

        public string OldRequestID { get; set; }

       // [Required(ErrorMessage = "Enter valid Captcha")]
        public string txtInput { get; set; }

        public bool AdultIndianMember { get; set; }
        public bool AdultNonIndianMember { get; set; }
        public bool Student { get; set; }
        public bool ChildBelowAgeFive { get; set; }

        public string IPAddress { get; set; }

        public string KioskUserId { get; set; }

        public string ShiftTypeID { get; set; }
        public string ShiftTypeName { get; set; }

		//[Required(ErrorMessage = "Enter Mobile Number")]
		public string MobileNumber { get; set; }
        public string ClientName { get; set; }


        #region printTicket
        public Int64 TicketID { get; set; }
        public decimal TotalAmount { get; set; }
        public string DateOfArrival { get; set; }

        public int TotalMember { get; set; }
        #endregion

        #region MemberDetails
        public string MSLNo { get; set; }
        public string TypeOfMember { get; set; }
        public string FeePerMember { get; set; }
        public string NoOfMember { get; set; }


        public string NoOfCamera { get; set; }
        public string FeePerCamera { get; set; }
        public string TotalFeesOfMember { get; set; }
        public string EmitraTransactionId { get; set; }


        public string NoOfStillCamera { get; set; }
        public string FeePerStillCamera { get; set; }

        public string NoOfVideoCamera { get; set; }
        public string FeePerVideoCamera { get; set; }



        public int Trn_Status_Code { get; set; }

        public string MemberIdType { get; set; }
        public string MemberIdNumber { get; set; }
        public string MemberMobileNo { get; set; }
        //   public List<MemberInformation> lstMember { get; set; }
        #endregion


        #region MemberDetails

        public bool PrivateVehicle { get; set; }
        public string VSLNo { get; set; }
        public string TypeOfVehicle { get; set; }
        public string FeePerVehicle { get; set; }
        public string NoOfVehicle { get; set; }
        public string VehicleNumber { get; set; }
        public string TotalVehicleFee { get; set; }

        //  public List<VehicleInformation> lstVehicle { get; set; }
		
        #endregion

        #region Vehicle
        public string NoOfBus { get; set; }
        public string NoOfJeepCarMotorMiniBus { get; set; }
        public string NoOfTwoWheeler { get; set; }
        public string NoOfAutoRikshaw { get; set; }

        public string TotalFeesOfBus { get; set; }
        public string TotalFeesOfJeepCarMotorMiniBus { get; set; }
        public string TotalFeesOfTwoWheeler { get; set; }
        public string TotalFeesOfAutoRikshaw { get; set; }
        public List<VehicleInformation> VehicleInformation { get; set; }
        #endregion

        #region Fees details

        public string MemberEntryFees { get; set; }
        public string CameraFees { get; set; }
        public string VehicleFees { get; set; }
        public string OnlineBookingCharges { get; set; }
        public string TotalPayableCharges { get; set; }

        #endregion




        /// <summary>
        /// Function return places
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Place()
        {
            DataTable dtPlaces = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Zoo_GetPlace", Conn);
                cmd.Parameters.AddWithValue("@Option", 1);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtPlaces);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtPlaces;
        }

        /// <summary>
        /// Function return MemberVehicleDetails
        /// </summary>
        /// <returns></returns>
        public DataSet MemberVehicleDetails()
        {
            DataSet dsMemVeh = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_MemberVehicleDetails", Conn);
                cmd.Parameters.AddWithValue("@PlaceID", Convert.ToInt32(PlaceOfVisit));
                cmd.Parameters.AddWithValue("@Option", 2);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMemVeh);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsMemVeh;
        }

        public DataSet MemberVehicleDetailsExtraInventory()
        {
            DataSet dsMemVeh = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_MemberVehicleDetails", Conn);
                cmd.Parameters.AddWithValue("@PlaceID", Convert.ToInt32(PlaceOfVisit));
                cmd.Parameters.AddWithValue("@Option", 4);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMemVeh);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsMemVeh;
        }

        public DataTable CheckHolidays(string PlaceOfVisit, int day)
        {
            DataTable dsMemVeh = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_CheckHolidays", Conn);
                cmd.Parameters.AddWithValue("@PlaceID", Convert.ToInt32(PlaceOfVisit));
                cmd.Parameters.AddWithValue("@day", Convert.ToInt32(day));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMemVeh);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsMemVeh;
        }

        public DataTable ZooCheckMaxTicketBooking(int PlaceId, int totalmember)
        {
            DataTable dt = new DataTable();
            bool status = false;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Zoo_CheckMaxTicketBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@totalmember", totalmember);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_CheckMaxTicketBooking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable CheckTicketAvailability(int PlaceId, int ShiftType, string VisitDate)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Zoo_ChkTicketAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.Parameters.AddWithValue("@DateofVisit", DateTime.ParseExact(VisitDate, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserId));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_ChkTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable CheckTicketExtraInventory(string Action,int PlaceId,string OldRequestID, string BookingDate,long KioskUserID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_ZooBooking_CheckExtraInventory", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceId);
                cmd.Parameters.AddWithValue("@OldRequestID", OldRequestID);
                cmd.Parameters.AddWithValue("@BookingDate", BookingDate);
                cmd.Parameters.AddWithValue("@kioskUserId", Convert.ToInt64(KioskUserID));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketExtraInventory" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        /// <summary>
        /// final submission of form
        /// </summary>
        /// <param name="dtm"></param>
        /// <param name="finalAmount"></param>
        /// <returns></returns>
        public DataTable Submit_ZooDetails(DataTable dtMember, DataTable dtVehicle, string XmlEquepmentList)
        {
            StringWriter sw = new StringWriter();
            dtVehicle.WriteXml(sw);
            string xmlVehicleList = sw.ToString();

            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {              
            new SqlParameter("@BookingTypeId", BookingType),  
            new SqlParameter("@PlaceId",PlaceOfVisit),
            new SqlParameter("@ShiftTypeID",ShiftTypeID),
            new SqlParameter("@Institutional_NameofInstitute", InstituteOrganisationName),
            new SqlParameter("@Institutional_AddressofInstitute", AddressOfInstitOrgan),
            new SqlParameter("@Institutional_PhoneofInstitute", PhoneNoOfInstitOrgan),
            new SqlParameter("@Institutional_NameofHead", NameOfHead),
            new SqlParameter("@Institutional_HeadPhoneNo", PhoneOfHead),
            new SqlParameter("@Institutional_DocumentofTour", DocumentForTour),          
            new SqlParameter("@HeadIdType", IdType),
            new SqlParameter("@HeadIdNumber", IdNumber),
            new SqlParameter("@Institutional_IDProfofGroupHead", UploadId),                       
            new SqlParameter("@Institutional_DateofVisit", DateTime.ParseExact(DateOfVisit, "dd/MM/yyyy", null)),
            new SqlParameter("@PrivateVehicle",PrivateVehicle),
            new SqlParameter("@MemberDetail", dtMember),
            new SqlParameter("@xmlVehicleDetail", xmlVehicleList),
            //new SqlParameter("@VehicleDetail", dtVehicle),    
            new SqlParameter("@IP_Address", IPAddress),    
            new SqlParameter("@EnteredBy",HttpContext.Current.Session["UserID"].ToString()),    
            new SqlParameter("@kioskUserId", Convert.ToInt64(KioskUserId)),
			new SqlParameter("@xmlEquepmentList",XmlEquepmentList),
			new SqlParameter("@MobileNo",MobileNumber),
            new SqlParameter("@ClientName",ClientName),
            };

                //shan added to save txt file 01-07-2021
                if (PlaceOfVisit == "59")
                {
                    string dtMembers = JsonConvert.SerializeObject(dtMember);
                    string dtVehicles = JsonConvert.SerializeObject(dtVehicle);
                    string SingleParam = "@BookingTypeId=" + BookingType + Environment.NewLine + "@PlaceId=" + PlaceOfVisit + Environment.NewLine + "@ShiftTypeID=" + ShiftTypeID
                    + Environment.NewLine + "@Institutional_NameofInstitute=" + InstituteOrganisationName + Environment.NewLine + "@Institutional_AddressofInstitute=" + AddressOfInstitOrgan
                    + Environment.NewLine + "@Institutional_PhoneofInstitute=" + AddressOfInstitOrgan + Environment.NewLine + "@Institutional_NameofHead=" + NameOfHead
                    + Environment.NewLine + "@Institutional_HeadPhoneNo=" + PhoneOfHead + Environment.NewLine + "@Institutional_DocumentofTour=" + DocumentForTour
                    + Environment.NewLine + "@HeadIdType=" + IdType + Environment.NewLine + "@HeadIdNumber=" + IdNumber + Environment.NewLine + "@Institutional_IDProfofGroupHead=" + UploadId
                    + Environment.NewLine + "@Institutional_DateofVisit=" + DateTime.ParseExact(DateOfVisit, "dd/MM/yyyy", null) + Environment.NewLine + "@PrivateVehicle=" + PrivateVehicle
                    + Environment.NewLine + "@IP_Address=" + IPAddress + Environment.NewLine + "@EnteredBy=" + HttpContext.Current.Session["UserID"].ToString() + Environment.NewLine
                    + "@kioskUserId=" + Convert.ToInt64(KioskUserId);
                    string combineAllString = SingleParam + Environment.NewLine + "-------MemberDetails-------" + Environment.NewLine + dtMembers + Environment.NewLine + "-------VehicleDetails-------" + Environment.NewLine + dtVehicles;
                    string filepath = HttpContext.Current.Server.MapPath("/Documents/ZooParmResult.txt");
                    SaveDataToTxtFile objFile = new SaveDataToTxtFile("txt", "", filepath, combineAllString);
                    bool result = objFile.WritetxtFile();
                }
                //end shan added to save txt file 01-07-2021

                Fill(dt, "SP_Zoo_Booking", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_Booking" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Submit_ZooDetailsOFPanduPoul()
        {
            DataTable dt = new DataTable();
            string xmlVehicleList = string.Empty;
            try
            {
                DALConn();
                if (VehicleInformation != null)
                {
                    DataTable dtVehicleList = new DataTable("ZooVehicleDetails_Pandupaul");
                    dtVehicleList.Columns.Add(new DataColumn("VehicleType", typeof(string)));
                    dtVehicleList.Columns.Add(new DataColumn("NoOfVehicle", typeof(string)));
                    dtVehicleList.Columns.Add(new DataColumn("VehicleNumber", typeof(string)));
                    dtVehicleList.Columns.Add(new DataColumn("FeePerVehicle", typeof(string)));
                    dtVehicleList.Columns.Add(new DataColumn("TotalVehicleFee", typeof(string)));
                    foreach (var obj in VehicleInformation)
                    {
                        DataRow row = dtVehicleList.NewRow();

                        row["VehicleType"] = obj.TypeOfVehicle;
                        row["NoOfVehicle"] = obj.NoOfVehicle;
                        row["VehicleNumber"] = obj.VehicleNumber;
                        row["FeePerVehicle"] = obj.FeePerVehicle;
                        row["TotalVehicleFee"] = obj.TotalVehicleFee;
                        dtVehicleList.Rows.Add(row);
                    }
                    StringWriter sw = new StringWriter();
                    dtVehicleList.WriteXml(sw);
                    xmlVehicleList = sw.ToString();
                }
                SqlParameter[] parameters =
                                      {
                                      new SqlParameter("@BookingTypeId", BookingType),
                                      new SqlParameter("@PlaceId",PlaceOfVisit),
                                      new SqlParameter("@ShiftTypeID",ShiftTypeID),
                                      new SqlParameter("@Institutional_DateofVisit", DateTime.ParseExact(DateOfVisit, "dd/MM/yyyy", null)),
                                      new SqlParameter("@PrivateVehicle",PrivateVehicle),
                                      new SqlParameter("@NoOfMember",NoOfMember),
                                      new SqlParameter("@MemberIdType",MemberIdType),
                                      new SqlParameter("@MemberIdNumber",MemberIdNumber),
                                      new SqlParameter("@MemberMobileNo",MemberMobileNo),
                                      new SqlParameter("@VehicleInformationXML",xmlVehicleList),

                                      new SqlParameter("@IP_Address", IPAddress),
                                      new SqlParameter("@EnteredBy",HttpContext.Current.Session["UserID"].ToString()),
                                      new SqlParameter("@kioskUserId", Convert.ToInt64(KioskUserId))
                                      };

                Fill(dt, "SP_Zoo_Booking_PanduPaul", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Submit_ZooDetailsOFPanduPoul" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;

        }


        public DataTable Submit_ZooDetailsExtraInventory(DataTable dtMember, DataTable dtVehicle, string OldRequestID)
        {
            StringWriter sw = new StringWriter();
            dtVehicle.WriteXml(sw);
            string xmlVehicleList = sw.ToString();
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {              
            new SqlParameter("@BookingTypeId", BookingType),  
            new SqlParameter("@PlaceId",PlaceOfVisit),
            new SqlParameter("@ShiftTypeID",ShiftTypeID),
            new SqlParameter("@Institutional_NameofInstitute", InstituteOrganisationName),
            new SqlParameter("@Institutional_AddressofInstitute", AddressOfInstitOrgan),
            new SqlParameter("@Institutional_PhoneofInstitute", PhoneNoOfInstitOrgan),
            new SqlParameter("@Institutional_NameofHead", NameOfHead),
            new SqlParameter("@Institutional_HeadPhoneNo", PhoneOfHead),
            new SqlParameter("@Institutional_DocumentofTour", DocumentForTour),          
            new SqlParameter("@HeadIdType", IdType),
            new SqlParameter("@HeadIdNumber", IdNumber),
            new SqlParameter("@Institutional_IDProfofGroupHead", UploadId),                       
            new SqlParameter("@Institutional_DateofVisit", DateTime.ParseExact(DateOfVisit, "dd/MM/yyyy", null)),
            new SqlParameter("@PrivateVehicle",PrivateVehicle),
            new SqlParameter("@MemberDetail", dtMember),
            new SqlParameter("@xmlVehicleDetail", xmlVehicleList),
            //new SqlParameter("@VehicleDetail", dtVehicle),    
            new SqlParameter("@IP_Address", IPAddress),  
            new SqlParameter("@OldRequestID", OldRequestID),
            new SqlParameter("@EnteredBy",HttpContext.Current.Session["UserID"].ToString()),    
            new SqlParameter("@kioskUserId", Convert.ToInt64(KioskUserId)),

            };
                Fill(dt, "SP_Zoo_BookingExtraInventory", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_BookingExtraInventory" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Submit_ZooDetails(SubmitZooBooking submitZooBooking, DataTable dtMember, DataTable dtVehicle)
        {
            DataTable dt = new DataTable();

            System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");


            try
            {
                DALConn();
                SqlParameter[] parameters =
            {              
            new SqlParameter("@BookingTypeId", submitZooBooking.BookingType),  
            new SqlParameter("@PlaceId",submitZooBooking.PlaceOfVisit),
            new SqlParameter("@ShiftTypeID",submitZooBooking.ShiftTypeID),
            new SqlParameter("@Institutional_NameofInstitute", submitZooBooking.InstituteOrganisationName),
            new SqlParameter("@Institutional_AddressofInstitute", submitZooBooking.AddressOfInstitOrgan),
            new SqlParameter("@Institutional_PhoneofInstitute", submitZooBooking.PhoneNoOfInstitOrgan),
            new SqlParameter("@Institutional_NameofHead", submitZooBooking.NameOfHead),
            new SqlParameter("@Institutional_HeadPhoneNo", submitZooBooking.PhoneOfHead),
            new SqlParameter("@Institutional_DocumentofTour",submitZooBooking.DocumentForTour),          
            new SqlParameter("@HeadIdType", submitZooBooking.IdType),
            new SqlParameter("@HeadIdNumber", submitZooBooking.IdNumber),
            new SqlParameter("@Institutional_IDProfofGroupHead",submitZooBooking.UploadId),//submitZooBooking.UploadId),                       
            new SqlParameter("@Institutional_DateofVisit", Convert.ToDateTime(submitZooBooking.DateOfVisit,cul)),
            new SqlParameter("@PrivateVehicle",submitZooBooking.PrivateVehicle),
            new SqlParameter("@MemberDetail", dtMember),
            new SqlParameter("@VehicleDetail", dtVehicle),    
            new SqlParameter("@IP_Address", submitZooBooking.IPAddress),    
            new SqlParameter("@EnteredBy", GetUserIDbySsoId(submitZooBooking.SsoId)),    
            new SqlParameter("@kioskUserId", Convert.ToInt64(submitZooBooking.KioskUserId)),
            new SqlParameter("@TicketType", "Mobile"),

            };
                Fill(dt, "SP_Zoo_Booking", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SP_Zoo_Booking" + "_" + "Citizen", 1, DateTime.Now, 1);
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public Int64 GetUserIDbySsoId(string ssoId)
        {
            DataTable dt = new DataTable();

            Int64 UserId = 0;
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {    
                new SqlParameter("@Action", "GetSsoId"),
                new SqlParameter("@SsoID",ssoId)                
                };

                Fill(dt, "sp_MobileZoo", parameters);
                if (dt.Rows.Count > 0)
                    UserId = Convert.ToInt64(dt.Rows[0]["UserID"]);
                else
                    UserId = 0;

            }
            catch (Exception ex)
            {

            }

            return UserId;
        }


        /// <summary>
        /// final submission FOR ZOO MOBILE APP
        /// </summary>
        /// <param name="dtm"></param>
        /// <param name="finalAmount"></param>
        /// <returns></returns>

        //public DataTable Submit_ZooDetailsForMOBILEAPP(SubmitZooBooking submitZooBooking, DataTable dtMember, DataTable dtVehicle)
        //{
        //    DataTable dt = new DataTable();

        //    System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");


        //    try
        //    {
        //        DALConn();
        //        SqlParameter[] parameters =
        //    {              
        //    new SqlParameter("@BookingTypeId", submitZooBooking.BookingType),  
        //    new SqlParameter("@PlaceId",submitZooBooking.PlaceOfVisit),
        //    new SqlParameter("@Institutional_NameofInstitute", submitZooBooking.InstituteOrganisationName),
        //    new SqlParameter("@Institutional_AddressofInstitute", submitZooBooking.AddressOfInstitOrgan),
        //    new SqlParameter("@Institutional_PhoneofInstitute", submitZooBooking.PhoneNoOfInstitOrgan),
        //    new SqlParameter("@Institutional_NameofHead", submitZooBooking.NameOfHead),
        //    new SqlParameter("@Institutional_HeadPhoneNo", submitZooBooking.PhoneOfHead),
        //    new SqlParameter("@Institutional_DocumentofTour",submitZooBooking.DocumentForTour),          
        //    new SqlParameter("@HeadIdType", submitZooBooking.IdType),
        //    new SqlParameter("@HeadIdNumber", submitZooBooking.IdNumber),
        //    new SqlParameter("@Institutional_IDProfofGroupHead",submitZooBooking.UploadId),//submitZooBooking.UploadId),                       
        //    new SqlParameter("@Institutional_DateofVisit", Convert.ToDateTime(submitZooBooking.DateOfVisit,cul)),
        //    new SqlParameter("@PrivateVehicle",submitZooBooking.PrivateVehicle),
        //    new SqlParameter("@MemberDetail", dtMember),
        //    new SqlParameter("@VehicleDetail", dtVehicle),    
        //    new SqlParameter("@IP_Address", submitZooBooking.IPAddress),    
        //    new SqlParameter("@EnteredBy", GetUserIDbySsoId(submitZooBooking.SsoId)),    
        //    new SqlParameter("@kioskUserId", Convert.ToInt64(submitZooBooking.KioskUserId)),

        //    };
        //        Fill(dt, "SP_Zoo_Booking", parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        new Common().ErrorLog(ex.Message, "SP_Zoo_Booking" + "_" + "Citizen", 1, DateTime.Now, 1);
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //    }
        //    return dt;
        //}







        /// <summary>
        /// function to update transaction status
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public int UpdateTransactionStatus(string option, double EmitraAmount = 0, double Amount = 0)
        {
            DataTable dt = new DataTable();
            Int32 chk = 0;
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {    
                new SqlParameter("@RequestedId", RequestId),
                new SqlParameter("@TransactionId",transId),
                new SqlParameter("@TransactionStatus", Trn_Status_Code),       
                new SqlParameter("@option", option),
                new SqlParameter("@EmitraAmount", EmitraAmount),
                new SqlParameter("@Amount", Amount),
                
                };
                Fill(dt, "SP_Zoo_UpdateTransactionStatus", parameters);
                if (dt.Rows.Count > 0)
                {
                    chk = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        public DataTable Select_BookedTicket()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Zoo_PrintBookedTicket", Conn);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        /// <summary>
        /// Ticket print
        /// </summary>
        /// <returns></returns>
        public DataSet Select_TicketData_For_Print()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {    
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "Sp_Zoo_SelecTicketDetail", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_Print" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public DataSet Select_TicketData_For_PrintExtraInventory()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {    
            new SqlParameter("@TicketID", TicketID)
            };
                Fill(ds, "Sp_Zoo_SelecTicketDetailExtraInventory", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_PrintExtraInventory" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataTable Get_BookedTicketDetails(string RequestID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Get_ZooBookedTicketDetails", Conn);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_BookedTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataSet BindVehiclesDetailsUsingTicket()
        {
            DataSet dsMemVeh = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_MemberVehicleDetails", Conn);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@Option", 3);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMemVeh);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsMemVeh;
        }

        public DataTable Submit_UpdateTicket(DataTable dtVehicle, string RequestId)
        {
            DataTable dsMemVeh = new DataTable();

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_UpdateTickets", Conn);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@VehicleDetail", dtVehicle);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMemVeh);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dsMemVeh;
        }

        #region Check Same IP,Shift Type,Date of Visit , Enter Date,SSOID and Place Develeoped By Rajveer

        public Boolean CheckSameIPBooking(BookOnZoo model, ref string Message)
        {
            bool flag = false;
            DataTable ds = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand("SP_CheckSameIPBooking", Conn);
                cmd.Parameters.AddWithValue("@PlaceID", model.PlaceOfVisit);
                cmd.Parameters.AddWithValue("@IPAddress", model.IPAddress);
                cmd.Parameters.AddWithValue("@ShiftType", model.ShiftTypeID);
                cmd.Parameters.AddWithValue("@EnterDate", DateTime.Now.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@DateOfVisit", model.DateOfVisit);
                cmd.Parameters.AddWithValue("@ZoneID", "");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt64(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds != null && ds.Rows.Count > 0)
                {
                    Message = ds.Rows[0]["message"].ToString();
                    flag = Convert.ToBoolean(ds.Rows[0]["status"]);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "CheckSameIPBooking" + "_" + "", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            return flag;
        }

        #endregion


        #region Zoo Booking API for Emitra Plus 31-12-2018
            public Int64 GetZooTicketId(string ZooRequestId)
        {
            DataTable dt = new DataTable();
            Int64 ZooBookingId = 0;
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {    
                new SqlParameter("@Action", "GetTicketId"),
                new SqlParameter("@RequestID",ZooRequestId)                
                };

                Fill(dt, "sp_GetUserID", parameters);
                if (dt.Rows.Count > 0)
                    ZooBookingId = Convert.ToInt64(dt.Rows[0]["ZooBookingId"]);
                else
                    ZooBookingId = 0;

            }
            catch (Exception ex)
            {

            }

            return ZooBookingId;
        }



public DataTable CheckRequestIDExist(string RequestID)
        {
            DataTable dt = new DataTable();
            Int32 chk = 0;
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(EmitraTransactionId);
                SqlParameter[] parameters =
                {    
                new SqlParameter("@RequestedId", RequestId)
                
                };
                Fill(dt, "SP_CheckRequestIDExist", parameters);
                if (dt.Rows.Count > 0)
                {
                    chk = Convert.ToInt32(dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        #endregion

    }

    public class MemberInformation
    {

        #region MemberDetails
        public string MSLNo { get; set; }
        public string TypeOfMember { get; set; }
        public string NoOfMember { get; set; }

        public string FeePerMember { get; set; }


        public string NoOfStillCamera { get; set; }
        public string FeePerStillCamera { get; set; }

        public string NoOfVideoCamera { get; set; }
        public string FeePerVideoCamera { get; set; }

        public string TotalFeesOfMember { get; set; }
        public string TotalFee { get; set; }

        #endregion
    }

    public class VehicleInformation
    {

		#region Vehicle Details
		public string VSLNo { get; set; }
        public string TypeOfVehicle { get; set; }
        public string FeePerVehicle { get; set; }
        public string NoOfVehicle { get; set; }
        public string TotalVehicleFee { get; set; }
        public string VehicleNumber { get; set; }

        #endregion
    }
	public class EquipmentInformation
	{

		#region Equipment Details
		public string EqNo { get; set; }
		public string EquipmentName { get; set; }
		public string FeePerEquipment { get; set; }
		public string NoOfEquipment { get; set; }
		public string TotalEquipmentFee { get; set; }
		public string Description { get; set; }
		#endregion
	}
}