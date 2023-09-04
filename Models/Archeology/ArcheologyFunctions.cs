
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace FMDSS.Models.Archeology
{

    public class ArcheologyFunctions : DAL
    {
        string SP_GetDistrictMaster = "SpGetArcheologyDsitrict";
        string SP_GetTypeVisitor = "SpGetTypeOfVisitor";
        string SP_GetPalaceMaster = "GetArcheologyAreabyDistrictName";
        string SP_GetPlaceWiseRateList = "SP_GetPlaceWiseRateList";
        string SP_InsertArcheologyRequest = "SP_InsertArcheologyRequest";
        string SP_GetEmitraDetails = "SP_GetEmitraDetailsArcheology";
        string SP_InsertArcheologyResponse = "SP_InsertArcheologyResponse";
        string SP_PrintArcheologyTicket = "SP_PrintArcheologyTicket";
        string SP_GetPlaceListbyPRNNumber = "SP_GetPlaceListbyPRNNumber";
        string SP_GetArcheologyareaByDistrict = "GetArcheologyareaByDistrict";
        string SP_GetArcheologyMISData = "SP_GetArcheologyMISData";

        public DataTable GetDistrictMaster()
        {
            DataTable dtCovid = new DataTable();
            try
            {

                DALConn();
                SqlCommand cmd = new SqlCommand(SP_GetDistrictMaster, Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }

        public DataTable GetTypeVisitor()
        {
            DataTable dtCovid = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand(SP_GetTypeVisitor, Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }

        public DataTable GetPalaceMaster()
        {
            DataTable dtCovid = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand(SP_GetTypeVisitor, Conn);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCovid);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "spGetCovidTicketDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return dtCovid;
        }

        public DataTable GetArcheologyAreabyDistrictName(string DistrictName)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand(SP_GetPalaceMaster, Conn);
                cmd.Parameters.AddWithValue("@DistrictName", DistrictName);
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


        public DataTable GetRateList()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_GetPlaceWiseRateList", Conn);

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
        public DataTable GetTermsandCondition(int placeId)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("GetTermsandCondtionbyPlaceId", Conn);
                cmd.Parameters.AddWithValue("@Placeid", placeId);


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

        public DataTable GetRateListbyPlaceId(int placeId, int visitorType)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_GetRateListbyPlaceId", Conn);
                cmd.Parameters.AddWithValue("@Placeid", placeId);
                cmd.Parameters.AddWithValue("@VistorType", visitorType);

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

        public DataTable InsertArcheologyRequest(ArcheologyModel obj)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand(SP_InsertArcheologyRequest, Conn);
                cmd.Parameters.AddWithValue("@ConsumerKey", obj.ConsumerKey);
                cmd.Parameters.AddWithValue("@SelectedPlaces", obj.selectedPlaces);
                cmd.Parameters.AddWithValue("@DateOfVisit", obj.DateOfVisit);
                cmd.Parameters.AddWithValue("@NumberofCitizen", obj.NumberofCitizen);
                cmd.Parameters.AddWithValue("@VisitorName", obj.VisitorName);
                cmd.Parameters.AddWithValue("@VisitorEmail", obj.Visitoremail);
                cmd.Parameters.AddWithValue("@VisitorMobile", obj.Visitormobile);
                cmd.Parameters.AddWithValue("@VisitorIdType", obj.VisitorIdType);
                cmd.Parameters.AddWithValue("@VisitorIdNumber", obj.VisitorIdNumber);
                cmd.Parameters.AddWithValue("@CreatedBy", obj.createdby);
                cmd.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);
                cmd.Parameters.AddWithValue("@VisitorType", Convert.ToInt32(obj.VistorType));
                cmd.Parameters.AddWithValue("@DistrictId", Convert.ToInt32(obj.PlaceOfVisit));

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

        public DataTable GetEmtiraDetailsforPayment()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand(SP_GetEmitraDetails, Conn);
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

        public DataTable InsertResponseFromEmitra(PGResponse obj)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand(SP_InsertArcheologyResponse, Conn);

                cmd.Parameters.AddWithValue("@REQTIMESTAMP", obj.REQTIMESTAMP);
                cmd.Parameters.AddWithValue("@PRN", obj.PRN);
                cmd.Parameters.AddWithValue("@MERCHANTCODE", obj.MERCHANTCODE);
                cmd.Parameters.AddWithValue("@AMOUNT", obj.AMOUNT);
                cmd.Parameters.AddWithValue("@PAIDAMOUNT", obj.PAIDAMOUNT);
                cmd.Parameters.AddWithValue("@SERVICEID", obj.SERVICEID);
                cmd.Parameters.AddWithValue("@TRANSACTIONID", obj.TRANSACTIONID);
                cmd.Parameters.AddWithValue("@RECEIPTNO", obj.RECEIPTNO);
                cmd.Parameters.AddWithValue("@EMITRATIMESTAMP", obj.EMITRATIMESTAMP);
                cmd.Parameters.AddWithValue("@STATUS", obj.STATUS);
                cmd.Parameters.AddWithValue("@PAYMENTMODE", obj.PAYMENTMODE);
                cmd.Parameters.AddWithValue("@PAYMENTMODEBID", obj.PAYMENTMODEBID);
                cmd.Parameters.AddWithValue("@PAYMENTMODETIMESTAMP", obj.PAYMENTMODETIMESTAMP);
                cmd.Parameters.AddWithValue("@RESPONSECODE", obj.RESPONSECODE);
                cmd.Parameters.AddWithValue("@RESPONSEMESSAGE", obj.RESPONSEMESSAGE);
                cmd.Parameters.AddWithValue("@UDF1", obj.UDF1);
                cmd.Parameters.AddWithValue("@UDF2", obj.UDF2);
                cmd.Parameters.AddWithValue("@CHECKSUM", obj.CHECKSUM);




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

        public DataSet GetEmtiraDataForPrintTicket(string PRN)
        {
            var ds = new DataSet();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand(SP_PrintArcheologyTicket, Conn);
                cmd.Parameters.AddWithValue("@PRN", PRN);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataTable GetArcheologyareaByDistrict(int districtId)
        {
            var ds = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand(SP_GetArcheologyareaByDistrict, Conn);
                cmd.Parameters.AddWithValue("@DistrictId", districtId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataTable GetArcheologyMISData(ArcheologyMIS obj)
        {
            var ds = new DataTable();
            try
            {
                DALConn();
                var check = Convert.ToDateTime(obj.dateofvisitfrom);
                SqlCommand cmd = new SqlCommand(SP_GetArcheologyMISData, Conn);
                cmd.Parameters.AddWithValue("@dateofvisitfrom", Convert.ToDateTime(obj.dateofvisitfrom));
                cmd.Parameters.AddWithValue("@dateofvisitto", Convert.ToDateTime(obj.dateofvisitto));
                cmd.Parameters.AddWithValue("@District", Convert.ToInt32(obj.District));
                cmd.Parameters.AddWithValue("@Place", obj.Place);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicket" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }





    }




}