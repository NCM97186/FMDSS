using FMDSS.APIInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.APIDAL
{
    public class BookingBoardingPass : Models.DAL, IBookingBoardingPass
    {
        public BookingBoardingPass()
        {
            DALConn();
        }
        public DataSet ValidateBoardingPass(string SsoId, string RequestId, bool IsEnter, bool IsOut = false)
        {
            DataSet ds = new DataSet();
            var prms2 = new[]{
                            new SqlParameter("@ActionName", "ValidateBoardingPass"),
                            new SqlParameter("@RequestId", RequestId),
                            new SqlParameter("@IsEnter", (IsEnter==true?1:0)),
                            new SqlParameter("@IsOut", (IsOut==true?1:0)),
                            new SqlParameter("@SSOId", SsoId),
                            };          
            Fill(ds, "Sp_ValidateBoardingPass", prms2);
            return ds;
        }
        public DataSet GetShiftTicktStatusCounts(string SsoId, string SelectedDate, int SelectedShift)
        {
            DataSet ds = new DataSet();
            var prms2 = new[]{
                            new SqlParameter("@ActionName", "GetShiftTicktStatusCounts"),
                            new SqlParameter("@SSOId", SsoId),
                            new SqlParameter("@SelectedDate", SelectedDate),
                            new SqlParameter("@SelectedShift", SelectedShift),
                            };
            Fill(ds, "Sp_ValidateBoardingPass", prms2);
            return ds;
        }
        public DataSet GetShiftList()
        {
            DataSet ds = new DataSet();
            var prms2 = new[]{
                            new SqlParameter("@ActionName", "GetShiftList"),
                            };
            Fill(ds, "Sp_ValidateBoardingPass", prms2);
            return ds;
        }
    }
}