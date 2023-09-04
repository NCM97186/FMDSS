using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.BookOnlineTicket
{
    public class WildLifeOnlineBooking : DAL
    {
        public WildlifeOnlineExtraAmountMasterViewModel GetOnlineBookingSetting(int placeId)
        {
            WildlifeOnlineExtraAmountMasterViewModel objModel = null;
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_MaintainMemberDetail_OnlineBooking", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objModel = Globals.Util.GetListFromTable<WildlifeOnlineExtraAmountMasterViewModel>(dt).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckOpenForDepartmentUser" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return objModel;
        }
        public DataTable MemberInformationWildLifeOnlineBooking(List<MemberInfo> lstMemberInfo, Int64 placeID, int vehicleID, int ShiftType)
        {
            DataTable objDt2 = new DataTable("Table");
            int seatsPerEqpt = 0;
            List<BookOnTicket> lstTicketInfo = CalculateFeesOnlineBooking(placeID, "2", vehicleID, ShiftType, out seatsPerEqpt);
            try
            {

                #region MemberInfo
                objDt2.Columns.Add("Name", typeof(String));
                objDt2.Columns.Add("Gender", typeof(String));
                objDt2.Columns.Add("Nationality", typeof(String));
                objDt2.Columns.Add("IDType", typeof(String));
                objDt2.Columns.Add("IDNo", typeof(String));
                objDt2.Columns.Add("MemberType", typeof(int));
                objDt2.Columns.Add("FeeTigerProject", typeof(decimal));
                objDt2.Columns.Add("FeeSurcharge", typeof(decimal));
                objDt2.Columns.Add("CameraFeeTigerProject", typeof(decimal));
                objDt2.Columns.Add("CameraFeeSurcharge", typeof(decimal));
                objDt2.Columns.Add("TRDF", typeof(decimal));

                objDt2.Columns.Add("TotalFeePerMember", typeof(decimal));
                objDt2.Columns.Add("TotalFeePerCamera", typeof(decimal));
                objDt2.Columns.Add("TotalCamera", typeof(string));

                objDt2.Columns.Add("BoardingVehicleFee", typeof(decimal));
                objDt2.Columns.Add("BoardingGuideFee", typeof(decimal));
                objDt2.Columns.Add("TotalBoardingFee", typeof(decimal));

                objDt2.Columns.Add("BoardingVehicleFeeGSTPercentage", typeof(decimal));
                objDt2.Columns.Add("BoardingGuideFeeGSTPercentage", typeof(decimal));
                objDt2.Columns.Add("BoardingVehicleFeeGSTAmount", typeof(decimal));
                objDt2.Columns.Add("BoardingGuideFeeGSTAmount", typeof(decimal));

                objDt2.Columns.Add("FinalAmountTobePaid", typeof(decimal));

                objDt2.Columns.Add("Vehicle_TRDF", typeof(decimal));
                objDt2.Columns.Add("GuidFee_TRDF", typeof(decimal));
                objDt2.Columns.Add("Isactive", typeof(int));

                objDt2.Columns.Add("PNR_NO", typeof(String));
                objDt2.Columns.Add("Seat_NO", typeof(String));
                objDt2.Columns.Add("Room_No", typeof(String));
                //Added by shaan 31-03-2021
                objDt2.Columns.Add("Fees_TigerProjectHalfDayFullDayCharge", typeof(decimal));
                objDt2.Columns.Add("Fee_SurchargeHalfDayFullDayCharge", typeof(decimal));
                //END

                string nationality = "";
                var nonIndianCount = lstMemberInfo.Where(x => x.MemberNationality == "2" && !string.IsNullOrEmpty(x.MemberName)).FirstOrDefault();

                nationality = "1";
                if (nonIndianCount != null)
                    nationality = "2";
                else
                    nationality = "1";

                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                    {

                        dr["Name"] = item.MemberName;
                        dr["Gender"] = item.MemberGender;
                        dr["Nationality"] = item.MemberNationality;
                        dr["IDType"] = item.MemberIdType;
                        dr["IDNo"] = item.MemberIdNo;
                        dr["MemberType"] = 2;
                        // dr["FeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberFees_TigerProject;
                        dr["FeeTigerProject"] = item.MemberFees_TigerProject;
                        //dr["FeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberFees_Surcharge;
                        dr["FeeSurcharge"] = item.MemberFees_Surcharge;
                        dr["CameraFeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().CameraFees_TigerProject;
                        dr["CameraFeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().CameraFees_Surcharge;
                        //dr["TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TRDF;
                        dr["TRDF"] = item.TRDF;

                        dr["BoardingVehicleFee"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFee;
                        dr["BoardingGuideFee"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFee;

                        dr["BoardingVehicleFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTPercentage;
                        dr["BoardingGuideFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTPercentage;
                        dr["BoardingVehicleFeeGSTAmount"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount;
                        dr["BoardingGuideFeeGSTAmount"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount;
                        //if (ShiftType == 3)
                        //{
                        //    //dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount);
                        //    dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount);
                        //    dr["BoardingVehicleFeeGSTAmount"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount;
                        //    //dr["BoardingGuideFeeGSTAmount"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount;
                        //}
                        //else
                        //{
                        //    //dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount);
                        //    dr["BoardingGuideFeeGSTAmount"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount;
                        //    dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount);
                        //    //dr["BoardingGuideFeeGSTAmount"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount;
                        //}
                        //item.TotalBoardingFee = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount);
                        item.TotalBoardingFee = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFee) + lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFee) + lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount;

                        //dr["TotalFeePerMember"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TotalPerMemberFees;
                        dr["TotalFeePerMember"] = item.TotalPerMemberFees;
                        if (!string.IsNullOrEmpty(item.MemberTotalCamera) && item.MemberTotalCamera != "0")
                        {
                            dr["TotalFeePerCamera"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TotalPerMemberCameraFees * Convert.ToDecimal(item.MemberTotalCamera);
                        }
                        else
                        {
                            dr["TotalFeePerCamera"] = 0;
                        }
                        dr["TotalCamera"] = item.MemberTotalCamera; //lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberTotalCamera;
                        dr["TotalBoardingFee"] = item.TotalBoardingFee;

                        //dr["FinalAmountTobePaid"] = (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TotalPerMemberFees) +
                        //    (Convert.ToDecimal(dr["TotalFeePerCamera"])) +
                        //    (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));

                        dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) +
                              (Convert.ToDecimal(dr["TotalFeePerCamera"])) + Convert.ToDecimal(item.TotalBoardingFee));


                        //if (ShiftType == 3)
                        //{
                        //    dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) +
                        //       (Convert.ToDecimal(dr["TotalFeePerCamera"])) + Math.Ceiling(Convert.ToDecimal(item.TotalBoardingFee)));
                        //}
                        //else
                        //{
                        //    dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) +
                        //       (Convert.ToDecimal(dr["TotalFeePerCamera"])) + Convert.ToDecimal(item.TotalBoardingFee));
                        //    //(Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));
                        //}
                        dr["Vehicle_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().Vehicle_TRDF;
                        dr["GuidFee_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().GuidFee_TRDF;
                        dr["Isactive"] = item.Isactive;

                        dr["PNR_NO"] = string.IsNullOrEmpty(item.PNR_NO) ? "" : item.PNR_NO;
                        dr["Seat_NO"] = string.IsNullOrEmpty(item.Seat_NO) ? "" : item.Seat_NO;
                        dr["Room_No"] = string.IsNullOrEmpty(item.Room_No) ? "" : item.Room_No;
                        //Added by shaan 30-03-2021
                        dr["Fees_TigerProjectHalfDayFullDayCharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().Fees_TigerProjectHalfDayFullDayCharge;
                        dr["Fee_SurchargeHalfDayFullDayCharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().Fee_SurchargeHalfDayFullDayCharge;
                        //End




                        //dr["Name"] = item.MemberName;
                        //dr["Gender"] = item.MemberGender;
                        //dr["Nationality"] = item.MemberNationality;
                        //dr["IDType"] = item.MemberIdType;
                        //dr["IDNo"] = item.MemberIdNo;
                        //dr["MemberType"] = 2;
                        //dr["FeeTigerProject"] = item.MemberFees_TigerProject;
                        //dr["FeeSurcharge"] = item.MemberFees_Surcharge;
                        //dr["CameraFeeTigerProject"] = item.CameraFees_TigerProject;
                        //dr["CameraFeeSurcharge"] = item.CameraFees_Surcharge;
                        //dr["TRDF"] = item.TRDF;

                        //dr["BoardingVehicleFee"] = item.BoardingVehicleFee;
                        //dr["BoardingGuideFee"] = item.BoardingGuideFee;

                        //dr["BoardingVehicleFeeGSTPercentage"] = item.BoardingVehicleFeeGSTPercentage;
                        //dr["BoardingGuideFeeGSTPercentage"] = item.BoardingGuideFeeGSTPercentage;
                        //dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(item.BoardingVehicleFeeGSTAmount);
                        //dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(item.BoardingGuideFeeGSTAmount);

                        //item.TotalBoardingFee = Convert.ToDecimal(item.BoardingVehicleFee) + Math.Ceiling(item.BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(item.BoardingGuideFee) + Math.Ceiling(item.BoardingGuideFeeGSTAmount);

                        //dr["TotalFeePerMember"] = item.TotalPerMemberFees;
                        //dr["TotalFeePerCamera"] = item.TotalPerMemberCameraFees;
                        //dr["TotalCamera"] = item.MemberTotalCamera;
                        //dr["TotalBoardingFee"] = item.TotalBoardingFee;

                        //dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) + (Convert.ToDecimal(item.TotalPerMemberCameraFees)) + (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));

                        //dr["Vehicle_TRDF"] = item.Vehicle_TRDF;
                        //dr["GuidFee_TRDF"] = item.GuidFee_TRDF;
                        //dr["Isactive"] = item.Isactive;
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return objDt2;
        }
        public DataTable MemberInformationWildLifeOnlineBookingIndianAndForeign(List<MemberInfo> lstMemberInfo, Int64 placeID, int vehicleID, int ShiftType)
        {
            DataTable objDt2 = new DataTable("Table");
            int seatsPerEqpt = 0;
            List<BookOnTicket> lstTicketInfo = CalculateFeesOnlineBooking(placeID, "2", vehicleID, ShiftType, out seatsPerEqpt);
            try
            {

                #region MemberInfo
                objDt2.Columns.Add("Name", typeof(String));
                objDt2.Columns.Add("Gender", typeof(String));
                objDt2.Columns.Add("Nationality", typeof(String));
                objDt2.Columns.Add("IDType", typeof(String));
                objDt2.Columns.Add("IDNo", typeof(String));
                objDt2.Columns.Add("MemberType", typeof(String));
                objDt2.Columns.Add("FeeTigerProject", typeof(String));
                objDt2.Columns.Add("FeeSurcharge", typeof(String));
                objDt2.Columns.Add("CameraFeeTigerProject", typeof(String));
                objDt2.Columns.Add("CameraFeeSurcharge", typeof(String));
                objDt2.Columns.Add("TRDF", typeof(String));

                objDt2.Columns.Add("TotalFeePerMember", typeof(String));
                objDt2.Columns.Add("TotalFeePerCamera", typeof(String));
                objDt2.Columns.Add("TotalCamera", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFee", typeof(String));
                objDt2.Columns.Add("BoardingGuideFee", typeof(String));
                objDt2.Columns.Add("TotalBoardingFee", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingVehicleFeeGSTAmount", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTAmount", typeof(String));

                objDt2.Columns.Add("FinalAmountTobePaid", typeof(String));

                objDt2.Columns.Add("Vehicle_TRDF", typeof(String));
                objDt2.Columns.Add("GuidFee_TRDF", typeof(String));
                objDt2.Columns.Add("Isactive", typeof(int));
                objDt2.Columns.Add("PNR_NO", typeof(String));
                objDt2.Columns.Add("Seat_NO", typeof(String));
                objDt2.Columns.Add("Room_No", typeof(String));
                //Added by shaan 31-03-2021
                objDt2.Columns.Add("Fees_TigerProjectHalfDayFullDayCharge", typeof(String));
                objDt2.Columns.Add("Fee_SurchargeHalfDayFullDayCharge", typeof(String));
                //end

                //var nonIndianCount = lstMemberInfo.Where(x => x.MemberNationality == "2" && !string.IsNullOrEmpty(x.MemberName)).FirstOrDefault();
                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                    {

                        dr["Name"] = item.MemberName;
                        dr["Gender"] = item.MemberGender;
                        dr["Nationality"] = item.MemberNationality;
                        dr["IDType"] = item.MemberIdType;
                        dr["IDNo"] = item.MemberIdNo;
                        dr["MemberType"] = 2;
                        dr["FeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().MemberFees_TigerProject;
                        dr["FeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().MemberFees_Surcharge;
                        dr["CameraFeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().CameraFees_TigerProject;
                        dr["CameraFeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().CameraFees_Surcharge;
                        dr["TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().TRDF;

                        dr["BoardingVehicleFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingVehicleFee;
                        dr["BoardingGuideFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingGuideFee;

                        dr["BoardingVehicleFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingVehicleFeeGSTPercentage;
                        dr["BoardingGuideFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingGuideFeeGSTPercentage;
                        dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingVehicleFeeGSTAmount);
                        dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingGuideFeeGSTAmount);

                        item.TotalBoardingFee = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingVehicleFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingGuideFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().BoardingGuideFeeGSTAmount);

                        dr["TotalFeePerMember"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().TotalPerMemberFees;
                        if (!string.IsNullOrEmpty(item.MemberTotalCamera) && item.MemberTotalCamera != "0")
                        {
                            dr["TotalFeePerCamera"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().TotalPerMemberCameraFees * Convert.ToDecimal(item.MemberTotalCamera);
                        }
                        else
                        {
                            dr["TotalFeePerCamera"] = 0;
                        }
                        dr["TotalCamera"] = item.MemberTotalCamera; //lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberTotalCamera;
                        dr["TotalBoardingFee"] = item.TotalBoardingFee;

                        dr["FinalAmountTobePaid"] = (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().TotalPerMemberFees) +
                            (Convert.ToDecimal(dr["TotalFeePerCamera"])) +
                            (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));

                        dr["Vehicle_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().Vehicle_TRDF;
                        dr["GuidFee_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().GuidFee_TRDF;
                        dr["Isactive"] = 1;
                        dr["PNR_NO"] = string.IsNullOrEmpty(item.PNR_NO) ? "" : item.PNR_NO;
                        dr["Seat_NO"] = string.IsNullOrEmpty(item.Seat_NO) ? "" : item.Seat_NO;
                        dr["Room_No"] = string.IsNullOrEmpty(item.Room_No) ? "" : item.Room_No;
                        //Added by shaan 30-03-2021
                        dr["Fees_TigerProjectHalfDayFullDayCharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().Fees_TigerProjectHalfDayFullDayCharge;
                        dr["Fee_SurchargeHalfDayFullDayCharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.MemberNationality).FirstOrDefault().Fee_SurchargeHalfDayFullDayCharge;
                        //End
                        //dr["Name"] = item.MemberName;
                        //dr["Gender"] = item.MemberGender;
                        //dr["Nationality"] = item.MemberNationality;
                        //dr["IDType"] = item.MemberIdType;
                        //dr["IDNo"] = item.MemberIdNo;
                        //dr["MemberType"] = 2;
                        //dr["FeeTigerProject"] = item.MemberFees_TigerProject;
                        //dr["FeeSurcharge"] = item.MemberFees_Surcharge;
                        //dr["CameraFeeTigerProject"] = item.CameraFees_TigerProject;
                        //dr["CameraFeeSurcharge"] = item.CameraFees_Surcharge;
                        //dr["TRDF"] = item.TRDF;

                        //dr["BoardingVehicleFee"] = item.BoardingVehicleFee;
                        //dr["BoardingGuideFee"] = item.BoardingGuideFee;

                        //dr["BoardingVehicleFeeGSTPercentage"] = item.BoardingVehicleFeeGSTPercentage;
                        //dr["BoardingGuideFeeGSTPercentage"] = item.BoardingGuideFeeGSTPercentage;
                        //dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(item.BoardingVehicleFeeGSTAmount);
                        //dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(item.BoardingGuideFeeGSTAmount);

                        //item.TotalBoardingFee = Convert.ToDecimal(item.BoardingVehicleFee) + Math.Ceiling(item.BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(item.BoardingGuideFee) + Math.Ceiling(item.BoardingGuideFeeGSTAmount);

                        //dr["TotalFeePerMember"] = item.TotalPerMemberFees;
                        //dr["TotalFeePerCamera"] = item.TotalPerMemberCameraFees;
                        //dr["TotalCamera"] = item.MemberTotalCamera;
                        //dr["TotalBoardingFee"] = item.TotalBoardingFee;

                        //dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) + (Convert.ToDecimal(item.TotalPerMemberCameraFees)) + (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));

                        //dr["Vehicle_TRDF"] = item.Vehicle_TRDF;
                        //dr["GuidFee_TRDF"] = item.GuidFee_TRDF;
                        //dr["Isactive"] = item.Isactive;
                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return objDt2;
        }
        public DataTable MemberInformationWildLifeOnlineBookingForeignWise(List<MemberInfo> lstMemberInfo, Int64 placeID, int vehicleID, int ShiftType)
        {
            DataTable objDt2 = new DataTable("Table");
            int seatsPerEqpt = 0;
            List<BookOnTicket> lstTicketInfo = CalculateFeesOnlineBooking(placeID, "2", vehicleID, ShiftType, out seatsPerEqpt);
            try
            {
                #region MemberInfo
                objDt2.Columns.Add("Name", typeof(String));
                objDt2.Columns.Add("Gender", typeof(String));
                objDt2.Columns.Add("Nationality", typeof(String));
                objDt2.Columns.Add("IDType", typeof(String));
                objDt2.Columns.Add("IDNo", typeof(String));
                objDt2.Columns.Add("MemberType", typeof(String));
                objDt2.Columns.Add("FeeTigerProject", typeof(String));
                objDt2.Columns.Add("FeeSurcharge", typeof(String));
                objDt2.Columns.Add("CameraFeeTigerProject", typeof(String));
                objDt2.Columns.Add("CameraFeeSurcharge", typeof(String));
                objDt2.Columns.Add("TRDF", typeof(String));

                objDt2.Columns.Add("TotalFeePerMember", typeof(String));
                objDt2.Columns.Add("TotalFeePerCamera", typeof(String));
                objDt2.Columns.Add("TotalCamera", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFee", typeof(String));
                objDt2.Columns.Add("BoardingGuideFee", typeof(String));
                objDt2.Columns.Add("TotalBoardingFee", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingVehicleFeeGSTAmount", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTAmount", typeof(String));

                objDt2.Columns.Add("FinalAmountTobePaid", typeof(String));

                objDt2.Columns.Add("Vehicle_TRDF", typeof(String));
                objDt2.Columns.Add("GuidFee_TRDF", typeof(String));
                objDt2.Columns.Add("Isactive", typeof(int));

                objDt2.Columns.Add("PNR_NO", typeof(String));
                objDt2.Columns.Add("Seat_NO", typeof(String));
                objDt2.Columns.Add("Room_No", typeof(String));

                //Added by shaan 31-03-2021
                objDt2.Columns.Add("Fees_TigerProjectHalfDayFullDayCharge", typeof(String));
                objDt2.Columns.Add("Fee_SurchargeHalfDayFullDayCharge", typeof(String));

                //end

                string nationality = "";
                var nonIndianCount = lstMemberInfo.Where(x => x.MemberNationality == "2" && !string.IsNullOrEmpty(x.MemberName)).FirstOrDefault();

                nationality = "1";
                if (nonIndianCount != null)
                    nationality = "2";
                else
                    nationality = "1";




                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();
                    if (item.MemberName != string.Empty && item.MemberName != null && item.MemberGender != "0" && item.MemberIdType != "0" && item.MemberIdNo != string.Empty && item.MemberIdNo != null && item.MemberNationality != "0")
                    {

                        dr["Name"] = item.MemberName;
                        dr["Gender"] = item.MemberGender;
                        dr["Nationality"] = item.MemberNationality;
                        dr["IDType"] = item.MemberIdType;
                        dr["IDNo"] = item.MemberIdNo;
                        dr["MemberType"] = 2;
                        dr["FeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberFees_TigerProject;
                        dr["FeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberFees_Surcharge;
                        dr["CameraFeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().CameraFees_TigerProject;
                        dr["CameraFeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().CameraFees_Surcharge;
                        dr["TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TRDF;

                        dr["BoardingVehicleFee"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFee;
                        dr["BoardingGuideFee"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFee;

                        dr["BoardingVehicleFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTPercentage;
                        dr["BoardingGuideFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTPercentage;
                        dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount);
                        dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount);

                        item.TotalBoardingFee = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().BoardingGuideFeeGSTAmount);

                        dr["TotalFeePerMember"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TotalPerMemberFees;
                        if (!string.IsNullOrEmpty(item.MemberTotalCamera) && item.MemberTotalCamera != "0")
                        {
                            dr["TotalFeePerCamera"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TotalPerMemberCameraFees * Convert.ToDecimal(item.MemberTotalCamera);
                        }
                        else
                        {
                            dr["TotalFeePerCamera"] = 0;
                        }
                        dr["TotalCamera"] = item.MemberTotalCamera; //lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().MemberTotalCamera;
                        dr["TotalBoardingFee"] = item.TotalBoardingFee;

                        dr["FinalAmountTobePaid"] = (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().TotalPerMemberFees) +
                            (Convert.ToDecimal(dr["TotalFeePerCamera"])) +
                            (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));

                        dr["Vehicle_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().Vehicle_TRDF;
                        dr["GuidFee_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().GuidFee_TRDF;
                        dr["Isactive"] = item.Isactive;

                        dr["PNR_NO"] = string.IsNullOrEmpty(item.PNR_NO) ? "" : item.PNR_NO;
                        dr["Seat_NO"] = string.IsNullOrEmpty(item.Seat_NO) ? "" : item.Seat_NO;
                        dr["Room_No"] = string.IsNullOrEmpty(item.Room_No) ? "" : item.Room_No;
                        //Added by shaan 30-03-2021
                        dr["Fees_TigerProjectHalfDayFullDayCharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().Fees_TigerProjectHalfDayFullDayCharge;
                        dr["Fee_SurchargeHalfDayFullDayCharge"] = lstTicketInfo.Where(x => x.MemberNationality == nationality).FirstOrDefault().Fee_SurchargeHalfDayFullDayCharge;
                        //End

                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return objDt2;
        }




        public List<MemberInfo> ReGenrateMemberInfoVechileWiseForOnline(List<MemberInfo> lstMemberInfo)
        {
            List<MemberInfo> newMemberList = new List<MemberInfo>();
            IEnumerator<MemberInfo> newMemberList1 = lstMemberInfo.GetEnumerator();
            MemberInfo data = null;
            string str = string.Empty;
            MemberInfo Record = lstMemberInfo.Where(x => x.MemberNationality == "2" && !string.IsNullOrEmpty(x.MemberName)).FirstOrDefault();
            if (Record == null)
            {
                Record = lstMemberInfo.Where(x => x.MemberNationality == "1").FirstOrDefault();

            }
            string strRecordDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(Record);
            while (newMemberList1.MoveNext())
            {
                var x = newMemberList1.Current;

                if (!string.IsNullOrEmpty(x.MemberName) && x.MemberNationality != "0")
                {
                    data = new MemberInfo();
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberInfo>(strRecordDataJson);
                    data.Isactive = 1;
                    data.MemberName = x.MemberName;
                    data.MemberNationality = x.MemberNationality;
                    data.MemberIdType = x.MemberIdType;
                    data.MemberIdNo = x.MemberIdNo;
                    data.MemberTotalCamera = x.MemberTotalCamera;
                    str += Newtonsoft.Json.JsonConvert.SerializeObject(data) + ",";
                }
                else
                {
                    data = new MemberInfo();
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberInfo>(strRecordDataJson);
                    data.Isactive = 20;
                    data.MemberTotalCamera = "0";
                    data.TotalPerMemberCameraFees = "0";
                    //Added by shaan 30-03-2021
                    data.TRDF = 0;
                    data.MemberFees_TigerProject = 0;
                    data.MemberFees_Surcharge = 0;
                    data.TotalPerMemberFees = Convert.ToString(Convert.ToDecimal(data.TRDF) + Convert.ToDecimal(data.MemberFees_TigerProject) + Convert.ToDecimal(data.MemberFees_Surcharge) + Convert.ToDecimal(data.Vehicle_TRDF) + Convert.ToDecimal(data.GuidFee_TRDF));
                    //END
                    str += Newtonsoft.Json.JsonConvert.SerializeObject(data) + ",";

                }

            }

            str = "[" + str.TrimEnd(',') + "]";
            newMemberList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MemberInfo>>(str);

            return newMemberList;
        }

        public List<MemberInfo> ReGenrateMemberInfoVechileWiseForDepartment(List<MemberDetailViewModel> lstMembers, Int64 placeID, int vehicleID, int ShiftType)
        {
            MemberDetailViewModel Indianrecord = lstMembers.Where(x => x.NationalityId == "1" && !string.IsNullOrEmpty(x.LeaderName)).FirstOrDefault();
            MemberDetailViewModel nonIndianrecord = lstMembers.Where(x => x.NationalityId == "2" && !string.IsNullOrEmpty(x.LeaderName)).FirstOrDefault();
            MemberDetailViewModel IndianStudentrecord = lstMembers.Where(x => x.NationalityId == "3" && !string.IsNullOrEmpty(x.LeaderName)).FirstOrDefault();

            List<MemberInfo> objMember = new List<MemberInfo>();
            int seatspereqpt = 0;
            List<BookOnTicket> lstTicketInfo = CalculateFeesOnlineBooking(placeID, "2", vehicleID, ShiftType, out seatspereqpt);
            string str = string.Empty;
            for (int i = 1; i <= nonIndianrecord.TotalPersons; i++)
            {
                str += Newtonsoft.Json.JsonConvert.SerializeObject(GetMemberInfo(nonIndianrecord, lstTicketInfo, nonIndianrecord, i)) + ",";
            }
            for (int i = 1; i <= Indianrecord.TotalPersons; i++)
            {
                if (nonIndianrecord.TotalPersons > 0)
                {
                    str += Newtonsoft.Json.JsonConvert.SerializeObject(GetMemberInfo(nonIndianrecord, lstTicketInfo, Indianrecord, i)) + ",";
                }
                else
                {
                    str += Newtonsoft.Json.JsonConvert.SerializeObject(GetMemberInfo(Indianrecord, lstTicketInfo, Indianrecord, i)) + ",";
                }
            }
            int totalcount = (Indianrecord.TotalPersons + nonIndianrecord.TotalPersons);
            int othercount = seatspereqpt - totalcount;

            if (othercount > 0)
            {
                if (nonIndianrecord != null)
                {
                    for (int i = 0; i < othercount; i++)
                    {
                        var data = GetMemberInfo(nonIndianrecord, lstTicketInfo, nonIndianrecord, i); //objMember.Where(x => x.MemberNationality == "2").FirstOrDefault();
                        data.Isactive = 20;
                        data.MemberTotalCamera = "0";
                        data.TotalPerMemberCameraFees = "0";
                        str += Newtonsoft.Json.JsonConvert.SerializeObject(data) + ",";
                    }
                }
                else
                {
                    for (int i = 0; i < othercount; i++)
                    {
                        var data = GetMemberInfo(Indianrecord, lstTicketInfo, Indianrecord, i); //objMember.Where(x => x.MemberNationality == "1").FirstOrDefault();
                        data.Isactive = 20;
                        data.MemberTotalCamera = "0";
                        data.TotalPerMemberCameraFees = "0";
                        str += Newtonsoft.Json.JsonConvert.SerializeObject(data) + ",";

                    }

                }
            }
            str = "[" + str.TrimEnd(',') + "]";
            List<MemberInfo> d = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MemberInfo>>(str);
            objMember.AddRange(d);
            return objMember;
        }
        public MemberInfo GetMemberInfo(MemberDetailViewModel memberDetail, List<BookOnTicket> lstTicketInfo, MemberDetailViewModel memberDetailIDProof, int Count = 0)
        {
            MemberInfo objMember = new MemberInfo();
            objMember.BoardingGuideFee = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().BoardingGuideFee;
            objMember.BoardingGuideFeeGSTAmount = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().BoardingGuideFee;
            objMember.BoardingGuideFeeGSTPercentage = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().BoardingGuideFeeGSTPercentage;
            objMember.BoardingVehicleFee = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().BoardingVehicleFee;
            objMember.BoardingVehicleFeeGSTAmount = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTAmount;
            objMember.BoardingVehicleFeeGSTPercentage = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTPercentage;
            objMember.CameraFees_Surcharge = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().CameraFees_Surcharge;
            objMember.CameraFees_TigerProject = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().CameraFees_TigerProject;
            objMember.GuidFee_TRDF = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().GuidFee_TRDF;

            if (Count == 1)
            {

                objMember.Isactive = 1;
            }
            else
            {
                objMember.Isactive = 20;
            }

            objMember.MemberFees_Surcharge = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().MemberFees_Surcharge;
            objMember.MemberFees_TigerProject = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().MemberFees_TigerProject;
            objMember.MemberGender = "1";
            objMember.MemberIdNo = memberDetailIDProof.IdNo;
            objMember.MemberIdType = memberDetailIDProof.IdType;
            objMember.MemberName = memberDetailIDProof.LeaderName;
            objMember.MemberNationality = memberDetailIDProof.NationalityId;

            if (Count == 1)
            {
                objMember.MemberTotalCamera = !string.IsNullOrEmpty(Convert.ToString(memberDetailIDProof.NoOfVideoCamera)) ? Convert.ToString(memberDetailIDProof.NoOfVideoCamera) : "0";
            }
            else
            {
                objMember.MemberTotalCamera = "0";
            }

            objMember.MemberType = "2";
            objMember.TotalBoardingFee = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().TotalBoardingFee;
            if (Count == 1)
            {
                objMember.TotalPerMemberCameraFees = Convert.ToString(lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().TotalPerMemberCameraFees);
            }
            else
            {
                objMember.TotalPerMemberCameraFees = "0";

            }

            objMember.TotalPerMemberFees = Convert.ToString(lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().TotalPerMemberFees);
            objMember.TRDF = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().TRDF;
            objMember.Vehicle_TRDF = lstTicketInfo.Where(x => x.MemberNationality == memberDetail.NationalityId).FirstOrDefault().Vehicle_TRDF;
            return objMember;
        }
        public bool IsOpenForDepartmentUser(int placeId)
        {
            WildlifeOnlineExtraAmountMasterViewModel objModel = new WildlifeOnlineExtraAmountMasterViewModel();
            objModel = GetOnlineBookingSetting(placeId);
            bool result = false;
            if (objModel != null)
            {
                result = objModel.OpenForDepartmentUser;
            }
            return result;
        }

        public string CheckBookingSeatWiseOrVehicleWise(int placeId)
        {
            WildlifeOnlineExtraAmountMasterViewModel objModel = new WildlifeOnlineExtraAmountMasterViewModel();
            objModel = GetOnlineBookingSetting(placeId);
            string result = string.Empty;
            if (objModel != null)
            {
                result = Convert.ToString(objModel.CalculationSeatWiseOrVehicleWise);
            }
            return result;
        }
        public DataTable MemberInformationsOnlineBooking(List<MemberDetailViewModel> lstMemberInfo, Int64 placeID, int vehicleID, int ShiftType)
        {
            DataTable objDt2 = new DataTable("Table");
            int seatsPerEqpt = 0;
            List<BookOnTicket> lstTicketInfo = CalculateFeesOnlineBooking(placeID, "2", vehicleID, ShiftType, out seatsPerEqpt);
            try
            {

                #region MemberInfo
                objDt2.Columns.Add("Name", typeof(String));
                objDt2.Columns.Add("Gender", typeof(String));
                objDt2.Columns.Add("Nationality", typeof(String));
                objDt2.Columns.Add("IDType", typeof(String));
                objDt2.Columns.Add("IDNo", typeof(String));
                objDt2.Columns.Add("MemberType", typeof(String));
                objDt2.Columns.Add("FeeTigerProject", typeof(String));
                objDt2.Columns.Add("FeeSurcharge", typeof(String));
                objDt2.Columns.Add("CameraFeeTigerProject", typeof(String));
                objDt2.Columns.Add("CameraFeeSurcharge", typeof(String));
                objDt2.Columns.Add("TRDF", typeof(String));

                objDt2.Columns.Add("TotalFeePerMember", typeof(String));
                objDt2.Columns.Add("TotalFeePerCamera", typeof(String));
                objDt2.Columns.Add("TotalCamera", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFee", typeof(String));
                objDt2.Columns.Add("BoardingGuideFee", typeof(String));
                objDt2.Columns.Add("TotalBoardingFee", typeof(String));

                objDt2.Columns.Add("BoardingVehicleFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTPercentage", typeof(String));
                objDt2.Columns.Add("BoardingVehicleFeeGSTAmount", typeof(String));
                objDt2.Columns.Add("BoardingGuideFeeGSTAmount", typeof(String));

                objDt2.Columns.Add("FinalAmountTobePaid", typeof(String));

                objDt2.Columns.Add("Vehicle_TRDF", typeof(String));
                objDt2.Columns.Add("GuidFee_TRDF", typeof(String));



                objDt2.AcceptChanges();
                foreach (var item in lstMemberInfo)
                {
                    DataRow dr = objDt2.NewRow();

                    if (item.LeaderName != string.Empty && item.LeaderName != null && item.IdType != "0" && item.IdNo != string.Empty && item.IdNo != null && item.NationalityId != "0")
                    {

                        dr["Name"] = item.LeaderName;
                        dr["Gender"] = "1";
                        dr["Nationality"] = item.NationalityId;
                        dr["IDType"] = item.IdType;
                        dr["IDNo"] = item.IdNo;
                        dr["MemberType"] = 2;
                        dr["FeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().MemberFees_TigerProject * item.TotalPersons;
                        dr["FeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().MemberFees_Surcharge * item.TotalPersons;
                        dr["CameraFeeTigerProject"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().CameraFees_TigerProject * item.TotalPersons;
                        dr["CameraFeeSurcharge"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().CameraFees_Surcharge * item.TotalPersons;
                        dr["TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TRDF * item.TotalPersons;

                        dr["BoardingVehicleFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFee * item.TotalPersons;
                        dr["BoardingGuideFee"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFee * item.TotalPersons;

                        dr["BoardingVehicleFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTPercentage * item.TotalPersons;
                        dr["BoardingGuideFeeGSTPercentage"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTPercentage * item.TotalPersons;

                        dr["BoardingVehicleFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTAmount * item.TotalPersons);
                        dr["BoardingGuideFeeGSTAmount"] = Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTAmount * item.TotalPersons);

                        //lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTAmount);




                        dr["TotalFeePerMember"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberFees * item.TotalPersons;
                        decimal CemaraFess = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees;
                        dr["TotalFeePerCamera"] = CemaraFess * item.NoOfVideoCamera;
                        // calculate camera fees
                        //decimal percamerafees = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees;
                        int totalCamera = item.NoOfVideoCamera;
                        dr["TotalCamera"] = totalCamera;

                        // END


                        decimal boardingfees = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingVehicleFeeGSTAmount) + Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFee) + Math.Ceiling(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().BoardingGuideFeeGSTAmount);
                        dr["TotalBoardingFee"] = boardingfees; //lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee * item.TotalPersons;
                        // calculate total fees


                        decimal totalMemberFees = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberFees) * item.TotalPersons;
                        decimal totalcamrafees = Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees) * totalCamera;
                        decimal totalbordingfees = boardingfees * item.TotalPersons; //Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee) * item.TotalPersons;

                        //decimal totalAmount = (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberFees * item.TotalPersons) + (Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalPerMemberCameraFees * totalCamera)) + (Math.Ceiling(Convert.ToDecimal(lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().TotalBoardingFee * item.TotalPersons))));
                        //dr["FinalAmountTobePaid"] = (Convert.ToDecimal(item.TotalPerMemberFees) + (Convert.ToDecimal(item.TotalPerMemberCameraFees)) + (Math.Round(Convert.ToDecimal(item.TotalBoardingFee))));
                        dr["FinalAmountTobePaid"] = totalMemberFees + totalcamrafees + totalbordingfees;
                        // END
                        dr["Vehicle_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().Vehicle_TRDF * item.TotalPersons;
                        dr["GuidFee_TRDF"] = lstTicketInfo.Where(x => x.MemberNationality == item.NationalityId).FirstOrDefault().GuidFee_TRDF * item.TotalPersons;

                        objDt2.Rows.Add(dr);
                        objDt2.AcceptChanges();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDt2;
        }
        public List<BookOnTicket> CalculateFeesOnlineBooking(Int64 placeID, string memberType, int vehicleID, int ShiftType, out int seatspereqpt)
        {
            List<BookOnTicket> fees = new List<BookOnTicket>();
            DataSet ds = new DataSet();
            BookOnTicket cst = new BookOnTicket();
            cst.PlaceId = placeID;
            cst.MemberType = memberType;
            cst.vehicleID = vehicleID;
            ds = cst.SelectFeesForOnlineBooking(ShiftType);
            for (int i = 0; i < 3; i++)
            {
                if (ds.Tables[i].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[i].Rows)
                    {
                        string nationality = "";
                        if (i == 0)
                            nationality = "1";
                        else if (i == 1)
                            nationality = "2";
                        else if (i == 2)
                            nationality = "3";
                        fees.Add(new BookOnTicket()
                        {

                            MemberFees_TigerProject = Convert.ToDecimal(dr["MFees_TigerProject"].ToString()),
                            MemberFees_Surcharge = Convert.ToDecimal(dr["MFees_Surcharge"].ToString()),
                            TRDF = Convert.ToDecimal(dr["TRDF"].ToString()),
                            CameraFees_TigerProject = Convert.ToDecimal(dr["CFees_TigerProject"].ToString()),
                            CameraFees_Surcharge = Convert.ToDecimal(dr["CFees_Surcharge"].ToString()),

                            TotalPerMemberFees = (Convert.ToDecimal(dr["MFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["MFees_Surcharge"].ToString()) + Convert.ToDecimal(dr["TRDF"].ToString()) + Convert.ToDecimal(dr["Vehicle_TRDF"]) + Convert.ToDecimal(dr["GuidFee_TRDF"])),

                            TotalPerMemberCameraFees = (Convert.ToDecimal(dr["CFees_TigerProject"].ToString()) + Convert.ToDecimal(dr["CFees_Surcharge"].ToString())),

                            BoardingVehicleFee = Convert.ToDecimal(dr["BoardingVehicleFee"]),
                            BoardingVehicleFeeGSTPercentage = Convert.ToDecimal(dr["BoardingVehicleFeeGSTPercentage"]),
                            BoardingVehicleFeeGSTAmount = Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]),


                            BoardingGuideFee = Convert.ToDecimal(dr["BoardingGuideFee"]),
                            BoardingGuideFeeGSTPercentage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]),
                            BoardingGuideFeeGSTAmount = Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),

                            TotalBoardingFee = Convert.ToDecimal(dr["BoardingVehicleFee"]) + Convert.ToDecimal(dr["BoardingVehicleFeeGSTAmount"]) + Convert.ToDecimal(dr["BoardingGuideFee"]) + Convert.ToDecimal(dr["BoardingGuideFeeGSTAmount"]),


                            GSTMessage = Convert.ToDecimal(dr["BoardingGuideFeeGSTPercentage"]) == 0 ? "" : Convert.ToString(dr["BoardingGuideFeeGSTPercentage"]) + " % GST  applicable on guide fees And " + Convert.ToString(dr["BoardingVehicleFeeGSTPercentage"]) + "% applicable on vehicle rent",

                            Vehicle_TRDF = Convert.ToDecimal(dr["Vehicle_TRDF"]),

                            GuidFee_TRDF = Convert.ToDecimal(dr["GuidFee_TRDF"]),

                            MemberNationality = nationality,
                            //Added by shaan 30-03-2021
                            Fees_TigerProjectHalfDayFullDayCharge = string.IsNullOrEmpty(dr["Fees_TigerProjectHalfDayFullDayCharge"].ToString()) ? 0 : Convert.ToDecimal(dr["Fees_TigerProjectHalfDayFullDayCharge"].ToString()),
                            Fee_SurchargeHalfDayFullDayCharge = string.IsNullOrEmpty(dr["Fee_SurchargeHalfDayFullDayCharge"].ToString()) ? 0 : Convert.ToDecimal(dr["Fee_SurchargeHalfDayFullDayCharge"].ToString())
                            //End
                        });
                    }
                }
                else
                {
                    fees.Add(new BookOnTicket()
                    {
                        MemberFees_TigerProject = Convert.ToDecimal(0),
                        MemberFees_Surcharge = Convert.ToDecimal(0),
                        TRDF = Convert.ToDecimal(0),
                        CameraFees_TigerProject = Convert.ToDecimal(0),
                        CameraFees_Surcharge = Convert.ToDecimal(0),
                        TotalPerMemberFees = Convert.ToDecimal(0),
                        TotalPerMemberCameraFees = Convert.ToDecimal(0),
                    });
                }
            }
            seatspereqpt = ds.Tables[3].Rows[0][0] == null ? 0 : Convert.ToInt32(ds.Tables[3].Rows[0][0]);
            return fees;
        }
        public DataTable GetQuotaSeats(int placeId, int VehicleID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingQuotaSeats", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                cmd.Parameters.AddWithValue("@BookingDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@VehicleId", VehicleID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckOpenForDepartmentUser" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetQuotaSeatsForCovid(int placeId, int VehicleID,string arrivaldate,string shifttime,string CovidType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetOnlineBookingQuotaSeatsForCovid", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                cmd.Parameters.AddWithValue("@BookingDate",Convert.ToDateTime(arrivaldate));
                cmd.Parameters.AddWithValue("@ShiftTime", shifttime);
                cmd.Parameters.AddWithValue("@VehicleId", VehicleID);
                cmd.Parameters.AddWithValue("@CovidType", CovidType);
				cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckOpenForDepartmentUser" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public int SaveOnlineQuotaSeats(string requestId, int placeId, int CWLWBookedSeat, int CCFBookedSeat, int InchargeBookedSeat, DateTime DateofBooking, DateTime CreatedDate, int CreatedBy, int VehicleId)
        {
            int result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_SaveOnlineQuotaSeat", Conn);
                cmd.Parameters.AddWithValue("@RequestId", requestId);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                cmd.Parameters.AddWithValue("@CWLWBookedSeat", CWLWBookedSeat);
                cmd.Parameters.AddWithValue("@CCFBookedSeat", CCFBookedSeat);
                cmd.Parameters.AddWithValue("@InchargeBookedSeat", InchargeBookedSeat);
                cmd.Parameters.AddWithValue("@DateofBooking", DateofBooking);
                cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
                cmd.Parameters.AddWithValue("@VehicleId", VehicleId);
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SaveOnlineQuotaSeats" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        public int SaveOnlineFileUploder(string Action, string requestId, int placeId, long UserID, int Trn_Status, List<DocumentList> document)
        {
            int result = 0;
            try
            {

                string JSONString = JsonConvert.SerializeObject(document);
                DataTable documentsListTable = (DataTable)JsonConvert.DeserializeObject(JSONString, (typeof(DataTable)));

                DALConn();
                SqlCommand cmd = new SqlCommand("SP_SaveOnlineBookingFileUploader", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@PlaceID", placeId);
                cmd.Parameters.AddWithValue("@RequestID", requestId);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@Trn_Status", Trn_Status);
                cmd.Parameters.AddWithValue("@Documents", documentsListTable);
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "SaveOnlineFileUploder" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return result;
        }

        public DataTable CheckQuotaSeatsAvalibality(int placeId, DateTime dateofbooking, int VehicleId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_CheckQuotaAvalibality", Conn);
                cmd.Parameters.AddWithValue("@PlaceId", placeId);
                cmd.Parameters.AddWithValue("@BookingDate", dateofbooking);
                cmd.Parameters.AddWithValue("@VehicleId", VehicleId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "CheckOpenForDepartmentUser" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetVehicleList(string vehicleType, string prefix)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetVehicleList", Conn);
                cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
                cmd.Parameters.AddWithValue("@Perfix", prefix);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "GetVehicleList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable GetGuideList(string vehicleType, string prefix)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_GetGuideList", Conn);
                cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
                cmd.Parameters.AddWithValue("@Perfix", prefix);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Sp_GetGuideList" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

    }
}