using FMDSS.Models.BookOnlineZoo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FMDSS.Models
{
    public class SubmitZooBooking
    {
        public List<MemberInformation> lstMember { get; set; }
        public List<VehicleInformation> lstVehicle { get; set; }
        public string BookingType { get; set; }
        public string RequestId { get; set; }
        public string InstituteOrganisationName { get; set; }
        public string AddressOfInstitOrgan { get; set; }

        public string PhoneNoOfInstitOrgan { get; set; }

        public string NameOfHead { get; set; }
        public string PhoneOfHead { get; set; }
        public string DocumentForTour { get; set; }
        public string DocumentForTourImageName { get; set; }

        public string IdType { get; set; }


        public string IdNumber { get; set; }


        public string UploadId { get; set; }
        public string UploadIdImageName { get; set; }
        public string PlaceOfVisit { get; set; }
        public string DateOfVisit { get; set; }
        public string txtInput { get; set; }
        // public string DocumentForTour { get; set; }
        public bool AdultIndianMember { get; set; }
        public bool AdultNonIndianMember { get; set; }
        public bool Student { get; set; }
        public bool ChildBelowAgeFive { get; set; }

        public string IPAddress { get; set; }
        public string IPAddressAndDeviceKey { get; set; }
        public string KioskUserId { get; set; }

        public bool PrivateVehicle { get; set; }
        public string VSLNo { get; set; }
        public string TypeOfVehicle { get; set; }
        public string FeePerVehicle { get; set; }
        public string NoOfVehicle { get; set; }
        public string TotalVehicleFee { get; set; }

        public string NoOfBus { get; set; }
        public string NoOfJeepCarMotorMiniBus { get; set; }
        public string NoOfTwoWheeler { get; set; }
        public string NoOfAutoRikshaw { get; set; }

        public string TotalFeesOfBus { get; set; }
        public string TotalFeesOfJeepCarMotorMiniBus { get; set; }
        public string TotalFeesOfTwoWheeler { get; set; }
        public string TotalFeesOfAutoRikshaw { get; set; }

        public string MemberEntryFees { get; set; }
        public string CameraFees { get; set; }
        public string VehicleFees { get; set; }
        public string OnlineBookingCharges { get; set; }
        public string TotalPayableCharges { get; set; }
        public string ShiftTypeID { get; set; }
        public string ShiftTypeName { get; set; }
        public string SsoId { get; set; }

    }


    public class FinalPay
    {
        public string RequestId { get; set; }
        public Decimal MemberFee { get; set; }
        public Decimal CameraFee { get; set; }
        public Decimal EmitraCharges { get; set; }
        public Decimal TotalFinalAmount { get; set; }
        public string status { get; set; }

    }

    public class PaymentStatus
    {
        public string TRANSACTION_STATUS { get; set; }
        public string REQUEST_ID { get; set; }
        public string EMITRA_TRANSACTION_ID { get; set; }
        public string TRANSACTION_TIME { get; set; }
        public string TRANSACTION_AMOUNT { get; set; }
        public string EMITRA_AMOUNT { get; set; }
        public string USER_NAME { get; set; }
        public string TRANSACTION_BANK_DETAILS { get; set; }


    }


    public class cls_Feed
    {
        public int FeedBackId { get; set; }
        public string Heading { get; set; }
        public int Content { get; set; }
        public int Design { get; set; }
        public int EaseofUse { get; set; }
        public string Comments { get; set; }
        public string SsoId { get; set; }

    }

    public class ZooFeesDetails
    {
        public List<MemberChargeList> MemberList { get; set; }

        public List<VehicleChargeList> VehicleChargeList { get; set; }
    }
    public class MemberChargeList
    {
        public string FeeChargedOn { get; set; }
        public decimal HeadAmount { get; set; }
        public int PlaceId { get; set; }
        public decimal CameraAmount { get; set; }
    }

    public class VehicleChargeList
    {
        public string FeeChargedOn { get; set; }
        public decimal HeadAmount { get; set; }
    }

    public class cls_MemberChargeList : DAL
    {



        public DataSet ZooPlacewiseMemberVehicle(int PlaceId)
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("Sp_Zoo_MemberVehicleDetails_Mobile", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.CommandType = CommandType.StoredProcedure;

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


    public class cls_Feedback : DAL
    {
        public string SsoId { get; set; }

        public string Institutional_NameofInstitute { get; set; }

        public string Institutional_AddressofInstitute { get; set; }
        public string Institutional_PhoneofInstitute  { get; set; }
        public string Institutional_NameofHead { get; set; }       
        public string Institutional_HeadPhoneNo { get; set; }      
        public string Institutional_HeadIdType { get; set; }       
        public string Institutional_HeadIdNumber { get; set; }     
        public DateTime Institutional_DateofVisit  { get; set; }     
        public int isPrivateVehical { get; set; }
        public decimal Institutional_TotalVehicalFees { get; set; }
        public Int32 PlaceId { get; set; }
        public List<cls_Feed> GetFeedBack()
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spFeedBack", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetFeedBack");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                List<cls_Feed> feedList = new List<cls_Feed>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cls_Feed feedbackList = new cls_Feed();
                    feedbackList.FeedBackId = Convert.ToInt32(dt.Rows[i]["FeedBackId"]);
                    feedbackList.Heading = Convert.ToString(dt.Rows[i]["Heading"]);
                    feedbackList.Content = Convert.ToInt32(dt.Rows[i]["Content"]);
                    feedbackList.Design = Convert.ToInt32(dt.Rows[i]["Design"]);
                    feedbackList.EaseofUse = Convert.ToInt32(dt.Rows[i]["EaseofUse"]);
                    feedbackList.Comments = Convert.ToString(dt.Rows[i]["Comments"]);
                    feedbackList.SsoId = Convert.ToString(dt.Rows[i]["SSOId"]);
                    feedList.Add(feedbackList);
                }

                return feedList;

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


        public bool AddFeedBack(cls_Feed objFeedback)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spFeedBack", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "InsertFeedBack");
                cmd.Parameters.AddWithValue("@Content", objFeedback.Content);
                cmd.Parameters.AddWithValue("@Design", objFeedback.Design);
                cmd.Parameters.AddWithValue("@EaseofUse", objFeedback.EaseofUse);
                cmd.Parameters.AddWithValue("@Comments", objFeedback.Comments);
                cmd.Parameters.AddWithValue("@SsoId", objFeedback.SsoId);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }


       
        public DataTable GetGroupDetails(string SsoId, string Institutional_NameofInstitute, string Institutional_AddressofInstitute, string Institutional_PhoneofInstitute, string Institutional_NameofHead,
            string Institutional_HeadPhoneNo, string Institutional_HeadIdType, string Institutional_HeadIdNumber, DateTime Institutional_DateofVisit, int isPrivateVehical, decimal Institutional_TotalVehicalFees, Int32 PlaceId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
               
                SqlCommand cmd = new SqlCommand("sp_MobileZooBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GROUP");
                cmd.Parameters.AddWithValue("@SsoId", SsoId);
                cmd.Parameters.AddWithValue("@Institutional_NameofInstitute", Institutional_NameofInstitute);
                cmd.Parameters.AddWithValue("@Institutional_AddressofInstitute", Institutional_AddressofInstitute);
                cmd.Parameters.AddWithValue("@Institutional_PhoneofInstitute", Institutional_PhoneofInstitute);
                cmd.Parameters.AddWithValue("@Institutional_NameofHead", Institutional_NameofHead);
                cmd.Parameters.AddWithValue("@Institutional_HeadPhoneNo", Institutional_HeadPhoneNo);
                cmd.Parameters.AddWithValue("@Institutional_HeadIdType", Institutional_HeadIdType);
                cmd.Parameters.AddWithValue("@Institutional_HeadIdNumber", Institutional_HeadIdNumber);
                cmd.Parameters.AddWithValue("@Institutional_DateofVisit", Institutional_DateofVisit);
                cmd.Parameters.AddWithValue("@isPrivateVehical", isPrivateVehical);
                cmd.Parameters.AddWithValue("@Institutional_TotalVehicalFees", Institutional_TotalVehicalFees);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt); 
               

            }
            catch (Exception ex)
            {
                 
            }
            finally
            {
                Conn.Close();
            }

            return dt;
        }

        public DataTable GetIndivisualDetails(string SsoId, Int32 PlaceId, DateTime Institutional_DateofVisit, int isPrivateVehical, decimal Institutional_TotalVehicalFees)
        {
            DataTable dt = new DataTable();
            try
            {
               
                DALConn();
                
                SqlCommand cmd = new SqlCommand("sp_MobileZooBooking", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Indivisual");
                cmd.Parameters.AddWithValue("@SSoId", SsoId);
                cmd.Parameters.AddWithValue("@PlaceId", PlaceId);
                cmd.Parameters.AddWithValue("@Institutional_DateofVisit", Institutional_DateofVisit);
                cmd.Parameters.AddWithValue("@isPrivateVehical", isPrivateVehical);
                cmd.Parameters.AddWithValue("@Institutional_TotalVehicalFees", Institutional_TotalVehicalFees);


                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

               

            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                Conn.Close();
            }
            return dt;
            //throw new NotImplementedException();
        }
    }
}