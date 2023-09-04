using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace FMDSS.Models.OnlineBooking
{

    public class CS_BoardingDetails : DAL
    {

        public int Index { get; set; }
        public string VehicleName { get; set; }  /// change by amit for new roster change on 13-09-2019        
        public string DisplayBookingId { get; set; }
        public string HDNBookingId { get; set; }
        public string RequestID { get; set; }
        public string Colors { get; set; }
        public string PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string NameOfVisitor { get; set; }
        public string DateofVisit { get; set; }
        public string DateofBooking { get; set; }
        public string Shift { get; set; }
        public string Vehicle { get; set; }
        public string ZoneAtTheTimeOfBooking { get; set; }
        public string ZoneAtTheTimeOfBoarding { get; set; }
        public string ZoneID { get; set; }
        public string Nationality { get; set; }
        public string IdproofIdDetails { get; set; }
        public string Camera { get; set; }
        public string Amount { get; set; }
        public string FeesPerPerson { get; set; }
        public string BoardingPointName { get; set; }
        public int BoardingIssueStatus { get; set; }
        public int ZoneUpdateStatus { get; set; }
        public string PrintID { get; set; }
        public string EmailIDDeptUser { get; set; }
        public string ContactNoDeptUser { get; set; }
        public string GuidName { get; set; }
        public string VehicleNumber { get; set; }
        public string VisitorCount { get; set; }
        public string ShiftID { get; set; }
        public string Trn_Status_Code { get; set; }
        //public List<CS_MemberDetails> MemberList { get; set; }
        public string AmountTobePaid { get; set; }
        public Int16 IsDepartmentalKioskUser { get; set; }
        public string DisplayShiftName { get; set; }
        public bool IsNotArrival { get; set; }
        public string TotalMembers { get; set; }
        public string ModeOfBooking { get; set; }
        public string IsNoShowOpenOrNot { get; set; }

        public string RequestId2 { get; set; }
        public int GuideId { get; set; }
        public int VehicleId { get; set; }
        public decimal ChoiceGuideAmt { get; set; }
        public decimal ChoiceVehicleAmt { get; set; }
        public int ChoiceGuideReplaceId { get; set; }
        public int ChoiceVehicleReplaceId { get; set; }
        // public decimal ExtraAmountRevised { get; set; }       
        public List<SelectListItem> GuideList = new List<SelectListItem>();
        public List<SelectListItem> VehicleList = new List<SelectListItem>();

        public DataTable GetAutoCompleteRequestId(string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, string RequestId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_AutoCompleteRequestId", Conn);
                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@DateOfArrival", DateOfArrival);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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



        public DataTable GetLoadDataIfKioskUser(string RequestID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetLoadDataIfKioskUser");
                if (RequestID.Contains("C19-"))
                {
                    cmd.Parameters.AddWithValue("@REQUESTIDForCovid", RequestID);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@REQUESTID", RequestID);
                }
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable GetLoadDataIfKioskUserNew(string SystemVehicleName, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, string UserID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetLoadDataIfKioskUserNew"); 

                cmd.Parameters.AddWithValue("@SystemGeneratedVehicleNumber", SystemVehicleName);   ///// Change by Amit for New system Generated Vehicle Number on 16-09-2019

                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@DateOfVisite", DateOfArrival);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftTime);

                cmd.Parameters.AddWithValue("@UpdatedBy", UserID);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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




        public DataTable GetVehicleByZoneAndPlace(int PlaceID, int ZoneID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "VehicleByZoneAndPlace");
                cmd.Parameters.AddWithValue("@RecordID", PlaceID);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable GetVehicleByZoneAndPlaceForVIPSeats(int PlaceID, int ZoneID,int ShiftType)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "VehicleByZoneAndPlaceForVIPSeats");
                cmd.Parameters.AddWithValue("@RecordID", PlaceID);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable GetDptUserEmail(int UserID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetDptUserEmail");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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


        ///Old Boarding Pass
        ///              
        public DataTable UpdateGuidNameAndVehicleNumber(string ID, string GuidName, string VehicleNumber, string UserID) 
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "REQUESTIDwiseUpdateGuidNameAndVehicleNumber");
                if (ID.Contains("C19-"))
                {
                    cmd.Parameters.AddWithValue("@REQUESTIDForCovid", ID);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@REQUESTID", ID);
                }
                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@GuideId", 0);
                cmd.Parameters.AddWithValue("@VehicleId", 0);
                cmd.Parameters.AddWithValue("@UpdatedBy", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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
        public DataTable UpdateGuidNameAndVehicleNumber(string ID, string GuidName, string VehicleNumber, string UserID, int GuideId, int VehicleId)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "REQUESTIDwiseUpdateGuidNameAndVehicleNumber");
                if (ID.Contains("C19-"))
                {
                    cmd.Parameters.AddWithValue("@REQUESTIDForCovid", ID);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@REQUESTID", ID);
                }
                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@GuideId", GuideId);
                cmd.Parameters.AddWithValue("@VehicleId", VehicleId);
                cmd.Parameters.AddWithValue("@UpdatedBy", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable IsBlockedRequest(string ID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "IsBlockedRequest");
          
                cmd.Parameters.AddWithValue("@REQUESTID", ID);
                
              
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        
        //New Boarding Pass develo by amit on 23-09-2019
        public DataTable UpdateGuidNameAndVehicleNumber(string ID,string SystemVehicleName, string GuidName, string VehicleNumber, string UserID,string DateOfArrival, string ShiftTime)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "REQUESTIDwiseUpdateGuidNameAndVehicleNumber");
                cmd.Parameters.AddWithValue("@REQUESTID", ID);
                cmd.Parameters.AddWithValue("@SystemGeneratedVehicleNumber", SystemVehicleName);   ///// Change by Amit for New system Generated Vehicle Number on 16-09-2019
                
                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);

                cmd.Parameters.AddWithValue("@DateOfVisite", DateOfArrival);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftTime);

                cmd.Parameters.AddWithValue("@UpdatedBy", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable UpdateGuidNameAndVehicleNumber_New(string SystemVehicleName, string GuidName, string VehicleNumber, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, string UserID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "UpdateGuidNameAndVehicleNumberNew"); 
                cmd.Parameters.AddWithValue("@SystemGeneratedVehicleNumber", SystemVehicleName);   ///// Change by Amit for New system Generated Vehicle Number on 16-09-2019

                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);

                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@DateOfVisite", DateOfArrival);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftTime); 

                cmd.Parameters.AddWithValue("@UpdatedBy", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        //begin Arvind Change for pass log 10/11/2016
        public DataTable UpdateForNotArrived(string REQUESTID, string MemberID, string UserID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "TicketUpdateForNotArrivedOnBoarding");
                cmd.Parameters.AddWithValue("@REQUESTID", REQUESTID);
                cmd.Parameters.AddWithValue("@MemberID", MemberID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UserID);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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
        //end Arvind Change for pass log 10/11/2016
        
        public DataSet ValidationForGuidNameAndVehicleNumber(string GuidName, string VehicleNumber, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime, string ID,string UserId)////Add User Id for boarding pass issue(by Amit)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "REQUESTIDwiseValidationGuidNameAndVehicleNumber");
                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);

                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@DateOfVisite", DateOfArrival);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftTime);
                cmd.Parameters.AddWithValue("@RequestID", ID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UserId);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable ValidateVehicleAndGuide(string RequestId, string DateOfArrival, string ShiftTime, int GuideId, int VehicleId, int VehicelTypeId)
        {
            DataTable dataTable = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_ValidateVehicleGuide", Conn);
                cmd.Parameters.AddWithValue("@Action", "ValidateVehicleGuide");

                cmd.Parameters.AddWithValue("@RequestId", RequestId);
                cmd.Parameters.AddWithValue("@DateOfVisite", DateOfArrival);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftTime);
                cmd.Parameters.AddWithValue("@GuideId", GuideId);
                cmd.Parameters.AddWithValue("@VehicleId", VehicleId);
                cmd.Parameters.AddWithValue("@VehicleTypeId", VehicelTypeId);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return dataTable;
        }
        public DataSet BindDptKioskPLACES(string SSOid)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GETLOADDATA");
                cmd.Parameters.AddWithValue("@SSOid", SSOid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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


        public DataSet BindDptKioskPLACESVIPSeats(string SSOid)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GETLOADDATAFORVIP");
                cmd.Parameters.AddWithValue("@SSOid", SSOid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataSet BindDptKioskPLACESVIPSeatsForPlaceWise(string SSOid, string Action = "GETLOADDATAFORVIP")
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@SSOid", SSOid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable GetBoardingDuration(string PlaceID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetBoardingDuration");
                cmd.Parameters.AddWithValue("@Place", PlaceID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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


        public DataTable GetBoardingDurationForVIPSeats(string PlaceID,string ZONEID,string Action)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@Place", PlaceID);
                cmd.Parameters.AddWithValue("@ZoneID", ZONEID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

       
        public DataTable GetZoneByPlace(string Place)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "ZoneByPlace");
                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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

        public DataTable GetZoneByPlaceForVIPSeat(string Place, string shiftType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "ZoneByPlace");
                cmd.Parameters.AddWithValue("@ShiftType", shiftType);
                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
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

        public DataTable UpdateBoardingZone(string ZoneID, string RecordID, string UpdatedBy)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "UPDATEBoardingZone");
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@RecordID", RecordID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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


        public void UPDATEBoardingIssueStatus(string RecordID, string UpdatedBy)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "UPDATEBoardingIssueStatus");
                cmd.Parameters.AddWithValue("@RecordID", RecordID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);

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



        public DataTable GetCurrentDateBooking(string DATE, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE,string Zone,string VehicleTypeID,string RequestID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                //SqlCommand cmd = new SqlCommand("sp_TicketBookingBoardingPassOnline", Conn);
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DATE);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DATE);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", BOOKING_TYPE);

                cmd.Parameters.AddWithValue("@Zone", Zone);
                cmd.Parameters.AddWithValue("@VehicleTypeID", VehicleTypeID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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


        public DataSet GetMappingTicketswithGuideAndVehicle(string DATE, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string Zone, string VehicleTypeID, string RequestID)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DATE);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DATE);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", BOOKING_TYPE);

                cmd.Parameters.AddWithValue("@Zone", Zone);
                cmd.Parameters.AddWithValue("@VehicleTypeID", VehicleTypeID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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



        public DataTable BoardingPassForOne(string RequestID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", "BoardingPassForOneRequestID");
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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


        public DataSet GetIssueBoardingListPrint(string DATE, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string VehicleNumber,string ZONE,string GuideName)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DATE);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DATE);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", BOOKING_TYPE);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@Zone", ZONE);
                cmd.Parameters.AddWithValue("@GuideName", GuideName);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public string GetSystemGeneratedNumber(string RequestID)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetSystemGeneratedNumber");
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                if (DS.Tables[0].Rows.Count > 0)
                    return Convert.ToString(DS.Tables[0].Rows[0]["SystemGeneratedVehicleNumber"]);
                else
                    return "0";


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

        public DataSet GetUserEmailbyRequestId(string RequestID)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetEmailAddress");
                cmd.Parameters.AddWithValue("@RequestID", RequestID);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable GetBookingDetailsVehicleWise(string SystemVehicleName, string BOOKING_TYPE, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@SystemGeneratedVehicleNumber", SystemVehicleName);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DateOfArrival);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DateOfArrival);
                cmd.Parameters.AddWithValue("@PlaceID", Place);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", ShiftTime);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", BOOKING_TYPE); 
                cmd.Parameters.AddWithValue("@Zone", ZoneID); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
    }

    public class CS_MemberDetails
    {
        public string VisiterName { get; set; }
        public string Nationality { get; set; }
        public string IdproofIdDetails { get; set; }

        public string Camera { get; set; }

        public string Amount { get; set; }

        public string BoardingPointName { get; set; }

    }


    public class CS_Ticket : DAL
    {
        private Int64 userID;
        private Int64 ticketID;
        private string requestID;
        private string category;
        private string districtID;
        private Int64 placeID;
        private DateTime arrivalDate;
        private string shiftTime;
        private string nationality;
        private int totalMember;
        private decimal feePerMember;
        private decimal cameraFee;
        private string memberType;
        private string name;
        private string gender;
        private string iDType;
        private string iDNo;
        private int totalCamera;
        private Int64 vehicleCatID;
        private string vehicleCategory;
        private Int64 vehicleID;
        private string vehicleName;
        private int availableSeat;
        private int seatForBooking;
        private decimal feePerVehicle;
        private decimal vehicleFeeTotal;
        private Int64 accomoID;
        private decimal roomCharge;
        private int totalRoom;
        private decimal roomAvailability;
        private Int64 enteredBy;
        private string transactionId;
        private int trn_Status_Code;
        private decimal totalAmount;
        private string date;
        private string kioskUserId;

        public string KioskUserId
        {
            get { return kioskUserId; }
            set { kioskUserId = value; }
        }



        public Int64 UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        public Int64 TicketID
        {
            get { return ticketID; }
            set { ticketID = value; }
        }
        public string RequestID
        {
            get { return requestID; }
            set { requestID = value; }
        }
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
        public string DistrictID
        {
            get { return districtID; }
            set { districtID = value; }
        }
        public Int64 PlaceID
        {
            get { return placeID; }
            set { placeID = value; }
        }
        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }
        public string ShiftTime
        {
            get { return shiftTime; }
            set { shiftTime = value; }
        }
        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }
        public int TotalMember
        {
            get { return totalMember; }
            set { totalMember = value; }
        }
        public decimal FeePerMember
        {
            get { return feePerMember; }
            set { feePerMember = value; }
        }
        public decimal CameraFee
        {
            get { return cameraFee; }
            set { cameraFee = value; }
        }
        public string MemberType
        {
            get { return memberType; }
            set { memberType = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public string IDType
        {
            get { return iDType; }
            set { iDType = value; }
        }
        public string IDNo
        {
            get { return iDNo; }
            set { iDNo = value; }
        }
        public int TotalCamera
        {
            get { return totalCamera; }
            set { totalCamera = value; }
        }
        public Int64 VehicleCatID
        {
            get { return vehicleCatID; }
            set { vehicleCatID = value; }
        }
        public string VehicleCategory
        {
            get { return vehicleCategory; }
            set { vehicleCategory = value; }
        }
        public Int64 VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }
        public string VehicleName
        {
            get { return vehicleName; }
            set { vehicleName = value; }
        }
        public int AvailableSeat
        {
            get { return availableSeat; }
            set { availableSeat = value; }
        }
        public int SeatForBooking
        {
            get { return seatForBooking; }
            set { seatForBooking = value; }
        }
        public decimal FeePerVehicle
        {
            get { return feePerVehicle; }
            set { feePerVehicle = value; }
        }
        public decimal VehicleFeeTotal
        {
            get { return vehicleFeeTotal; }
            set { vehicleFeeTotal = value; }
        }
        public Int64 AccomoID
        {
            get { return accomoID; }
            set { accomoID = value; }
        }
        public decimal RoomCharge
        {
            get { return roomCharge; }
            set { roomCharge = value; }
        }
        public int TotalRoom
        {
            get { return totalRoom; }
            set { totalRoom = value; }
        }
        public decimal RoomAvailability
        {
            get { return roomAvailability; }
            set { roomAvailability = value; }
        }
        public Int64 EnteredBy
        {
            get { return enteredBy; }
            set { enteredBy = value; }
        }
        public decimal TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }
        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        public int Trn_Status_Code
        {
            get { return trn_Status_Code; }
            set { trn_Status_Code = value; }
        }
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        public string isSafari { get; set; }
        public string isAccomo { get; set; }
        public string UploadId { get; set; }
        public Int64 ZoneId { get; set; }
        public string IPAddress { get; set; }

        public string DurationFrom { get; set; }
        public string DurationTo { get; set; }

        public int ShiftType { get; set; }



        public DataTable Select_Place()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_Get_citizen_Place", Conn);
                cmd.Parameters.AddWithValue("@Option", 1);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_PlaceDeptUser()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_Get_citizen_Place_DeptUser", Conn);
                cmd.Parameters.AddWithValue("@Option", 1);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@SSOid", HttpContext.Current.Session["SSOID"].ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SP_Get_citizen_Place" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_BookedTicket()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_Citizen_Select_BookedTicket", Conn);
                cmd.Parameters.AddWithValue("@UserId", UserID);
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
        public DataTable GetVehicleType()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                Fill(dt, "Sp_Citizen_Get_vehicleCategory");

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetVehicleType" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetAccomodationType()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@placeID", PlaceID)
            };
                Fill(dt, "Sp_Citizen_SelectAccomodationType", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetAccomodationType" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Get_CategorywiseDistrict()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_Citizen_GetCategorywiseDistrict", Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Category", Category);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Get_CategorywiseDistrict" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable Select_Zone_ByPlace()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Get_citizen_Place", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Option", 2);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Places_ByDistrict" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataSet GetVisitDate()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_Citizen_VisitDate", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "sp_Citizen_VisitDate" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet chkSafariAccomo()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("chkSafariAccomoAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "chkSafariAccomo" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }
        public string CheckTicketAvailability()
        {
            string Str = "";
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Citizen_ChkTicketAvailability", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DateOfArrival", ArrivalDate);
                cmd.Parameters.AddWithValue("@ShiftTime", ShiftTime);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                Str = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return Str;
        }

        public DataTable Select_Shift_ByDistrict_Places()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_Get_citizen_Place", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Option", 2);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ZoneId", ZoneId);
                cmd.Parameters.AddWithValue("@DeptUser", HttpContext.Current.Session["IsDepartmentalKioskUser"].ToString());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckTicketAvailability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable SelectMemberFees()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_Citizen_select_Ticket_Camera_Fees", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@Nationality", Nationality);
                cmd.Parameters.AddWithValue("@MemberType", MemberType);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SelectMemberFees" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_vehicle(Int64 VehicleCatID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@PlaceID", PlaceID),
            new SqlParameter("@ZoneId", ZoneId)
            };
                Fill(dt, "Sp_Citizen_Select_vehicle_by_CatID", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_vehicle" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_vehicle_Fees_Seat()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VehicleCatID", VehicleCatID),
            new SqlParameter("@VehicleID", VehicleID),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@PlaceID", PlaceID),
            new SqlParameter("@ZoneId", ZoneId),
            new SqlParameter("@ShiftType", ShiftType)
            };
                Fill(dt, "Sp_Citizen_Vehicle_Fees_Seat", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_vehicle_Fees_Seat" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Select_Accomo_Fees_Availability()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {    
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@PlaceID", PlaceID)
            };
                Fill(dt, "Sp_Citizen_Accomo_Availability", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_Accomo_Fees_Availability" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataTable Submit_TicketDetails(DataTable dtm, DataTable dts)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@RequestID", RequestID),
            new SqlParameter("@Category", Category),
            new SqlParameter("@DistrictID", DistrictID),
            new SqlParameter("@PlaceID", PlaceID),
            new SqlParameter("@ZoneId", ZoneId),
            new SqlParameter("@ShiftTime", ShiftTime),
            new SqlParameter("@ArrivalDate", ArrivalDate),
            new SqlParameter("@TotalMembers", TotalMember),
            new SqlParameter("@AccomoID", AccomoID),
            new SqlParameter("@TotalRoom", TotalRoom),
            new SqlParameter("@RoomCharge", RoomCharge),
            new SqlParameter("@MemberDetail", dtm),
            new SqlParameter("@SafariDetail", dts),
            new SqlParameter("@EnteredBy", EnteredBy),
            new SqlParameter("@IPAddress", IPAddress),
            new SqlParameter("@kioskUserId",Convert.ToInt64( kioskUserId))
            };
                // Int64 chk = Convert.ToInt64(ExecuteNonQuery("Sp_Citizen_BookTicket", parameters));
                Fill(dt, "Sp_Citizen_BookTicket", parameters);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Submit_TicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public void UpdateTransactionStatus(string option)
        {
            Int32 chk = 0;
            try
            {
                DALConn();
                Int64 transId = Convert.ToInt64(TransactionId);
                SqlParameter[] parameters =
            {    
            new SqlParameter("@RequestedId", RequestID),
            new SqlParameter("@TransactionId",transId),
            new SqlParameter("@TransactionStatus", Trn_Status_Code),       
            new SqlParameter("@option", option)     
            };
                chk = Convert.ToInt32(ExecuteNonQuery("Sp_Common_UpdateTransactionStatus", parameters));
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "UpdateTransactionStatus" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }

        }

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
                Fill(ds, "Sp_Citizen_SelecTicketDetail", parameters);

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
    }




    public class AssignGuideAndVehicleName : DAL
    {

        public int Index { get; set; }
        public string ID { get; set; }
        public string GVID { get; set; }
        public string PlaceID { get; set; }

        public string ZoneID { get; set; }

        public string PlaceName { get; set; }
        public string DateOfVisit { get; set; }
        public string ShiftType { get; set; }
        public string ShiftTypeName { get; set; }
        public string VehicalEqptName { get; set; }
        public string GuideName { get; set; }
        public string VehicleNumber { get; set; }
        public bool IsactiveForMapping { get; set; }

        public string EnteredBy { get; set; }
        public string UpdatedBy { get; set; }


        // ========= mapping with request ids

        public string RequestID { get; set; }
        public int TotalMembers { get; set; }


        public DataTable INSERTGuideAndVehicleForAssign( string GVID, string PlaceID, string ShiftType, string DateOfVisit, string VehicalEqptName, string GuideName, string VehicleNumber, string IsactiveForMapping)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AssignGuideAndVehicleForDateOfVisite", Conn);
                cmd.Parameters.AddWithValue("@Action", "INSERTGuideAndVehicleForAssign");
                cmd.Parameters.AddWithValue("@GVID", GVID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.Parameters.AddWithValue("@DateOfVisit", DateOfVisit);
                cmd.Parameters.AddWithValue("@VehicalEqptName", VehicalEqptName);
                cmd.Parameters.AddWithValue("@GuideName", GuideName);
                cmd.Parameters.AddWithValue("@VehicalNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@IsactiveForMapping", IsactiveForMapping.ToUpper() == "TRUE" ? 1 : 0 );                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable ValidationAndMappingGVforTickets(string Action, string GVID, string RequestID, string PlaceID, string ZoneID, string DateOfVisit, string VehicleID, string ShiftType)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AssignGuideAndVehicleForDateOfVisite", Conn);
                cmd.Parameters.AddWithValue("@Action", Action );
                cmd.Parameters.AddWithValue("@GVID", GVID);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftType);
                cmd.Parameters.AddWithValue("@DateOfVisit", DateOfVisit);
                cmd.Parameters.AddWithValue("@VehicalEqptName", VehicleID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);        
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        
        public DataTable GetVehicleByZoneAndDateOfVisit(int PlaceID, string DateOfVisit)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AssignGuideAndVehicleForDateOfVisite", Conn);
                cmd.Parameters.AddWithValue("@Action", "VehicleByZoneAndDateOfVisit");
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@DateOfVisit", DateOfVisit);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable GetDptUserEmail(int UserID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetDptUserEmail");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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





        public DataTable UpdateGuidNameAndVehicleNumber(string ID, string GuidName, string VehicleNumber, string BoardingID)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "UpdateGuidNameAndVehicleNumber");
                cmd.Parameters.AddWithValue("@RecordID", ID);
                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@BoardingID", BoardingID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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



        public DataTable ValidationForGuidNameAndVehicleNumber(string GuidName, string VehicleNumber, string Place, string ZoneID, string DateOfArrival, string VehicleID, string ShiftTime)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "ValidationForGuidNameAndVehicleNumberUpdate");
                cmd.Parameters.AddWithValue("@GuidName", GuidName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);

                cmd.Parameters.AddWithValue("@Place", Place);
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@DateOfVisite", DateOfArrival);
                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                cmd.Parameters.AddWithValue("@ShiftType", ShiftTime);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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


        public DataSet BindDptKioskPLACES(string SSOid)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "GETLOADDATA");
                cmd.Parameters.AddWithValue("@SSOid", SSOid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

        public DataTable UpdateBoardingZone(string ZoneID, string RecordID, string UpdatedBy)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "UPDATEBoardingZone");
                cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                cmd.Parameters.AddWithValue("@RecordID", RecordID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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


        public void UPDATEBoardingIssueStatus(string RecordID, string UpdatedBy)
        {
            try
            {
                DALConn();
                DataTable DS = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_Get_BoardingPassPlaceForDeptKUser", Conn);
                cmd.Parameters.AddWithValue("@Action", "UPDATEBoardingIssueStatus");
                cmd.Parameters.AddWithValue("@RecordID", RecordID);
                cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);

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



        public DataTable GetAssignGuideAndVehicleName(string DATE, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string VehicleTypeID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_AssignGuideAndVehicleForDateOfVisite", Conn);
                cmd.Parameters.AddWithValue("@Action", "SelectAllGuideAndVehicleForAssign");
                cmd.Parameters.AddWithValue("@DateOfVisit", DATE);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@ShiftType", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@VehicalEqptName", VehicleTypeID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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





        public DataTable BoardingPassForOne(string MemberID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", "BoardingPassForOne");
                cmd.Parameters.AddWithValue("@MemberID", MemberID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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


        public DataSet GetIssueBoardingListPrint(string DATE, string PlaceID, string SHIFT_TYPE, string BOOKING_TYPE, string VehicleNumber,string ZONE)
        {
            try
            {
                DALConn();
                DataSet DS = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_TicketBookingReport", Conn);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", DATE);
                cmd.Parameters.AddWithValue("@DATETIME_TO", DATE);
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);
                cmd.Parameters.AddWithValue("@SHIFT_TYPE", SHIFT_TYPE);
                cmd.Parameters.AddWithValue("@BOOKING_TYPE", BOOKING_TYPE);
                cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                cmd.Parameters.AddWithValue("@Zone", ZONE);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DS);
                return DS;
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

    
}