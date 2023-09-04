using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.BookOnlineTicket
{
    public class OnlineBookingSeatsModel : BaseModel
    {
        public int PlaceIDVIP { get; set; }
        public int PlaceIDHDFD { get; set; }
        public int NoOfGypsy { get; set; }
        public int ShiftType { get; set; }
        public int VehicleIDFDFD { get; set; }
        public int VehicleIDVIP { get; set; }
        public int TotalNoOfGypsy { get; set; }

        public int TotalNoOfGypsyFD { get; set; }
        public int TotalNoOfGypsyHD { get; set; }
        public int OverAllGypsy { get; set; }
        public int TotalNoOfGypsyVIPBooking { get; set; }
        public int TotalNoOfGypsyHDFD { get; set; }
        public int TotalNoOfGypsyVIP { get; set; }


        public int TotalSeatForCWLW { get; set; }
        public int TotalSeatForCCF { get; set; }
        public int TotalSeatForIncharge { get; set; }

        public string ShiftTypeVIP { get; set; }
        public string ShiftTypeFD { get; set; }
        public string ShiftTypeHD { get; set; }

        public string msg { get; set; }
        public int Status { get; set; }
    }

    public class OnlineBookingSeatsRepo : DAL
    {
        public OnlineBookingSeatsModel Select_BookedTicketSeats(OnlineBookingSeatsModel model, string Action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Get_OnlineBookingSeats", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        int PlaceID = Convert.ToInt32(dt.Rows[k]["PlaceID"]);

                        if (PlaceID == 62)
                        {
                            model.PlaceIDVIP = PlaceID;
                            model.VehicleIDVIP = Convert.ToInt32(dt.Rows[k]["VehicleID"]);
                            model.TotalSeatForCWLW = Convert.ToInt32(dt.Rows[k]["TotalSeatForCWLW"]);
                            model.TotalSeatForCCF = Convert.ToInt32(dt.Rows[k]["TotalSeatForCCF"]);
                            model.TotalSeatForIncharge = Convert.ToInt32(dt.Rows[k]["TotalSeatForIncharge"]);
                            model.ShiftTypeVIP = Convert.ToString(dt.Rows[k]["TotalSeatForIncharge"]);
                            model.TotalNoOfGypsyVIP = Convert.ToInt32(dt.Rows[k]["Gypsy"]);
                        }
                        else if (PlaceID == 65)
                        {
                            string shift = Convert.ToString(dt.Rows[k]["SHiftType"]);

                            if (shift == "3")
                            {
                                model.PlaceIDHDFD = PlaceID;
                                model.VehicleIDFDFD = Convert.ToInt32(dt.Rows[k]["VehicleID"]);
                                model.ShiftTypeFD = Convert.ToString(dt.Rows[k]["SHiftType"]);
                                model.TotalNoOfGypsyFD = Convert.ToInt32(dt.Rows[k]["Gypsy"]);
                            }
                            else
                            {
                                model.PlaceIDHDFD = PlaceID;
                                model.VehicleIDFDFD = Convert.ToInt32(dt.Rows[k]["VehicleID"]);
                                model.ShiftTypeHD = Convert.ToString(dt.Rows[k]["SHiftType"]);
                                model.TotalNoOfGypsyHD = Convert.ToInt32(dt.Rows[k]["Gypsy"]);
                            }

                        }
                        model.TotalNoOfGypsyHDFD = (model.TotalNoOfGypsyHD * 2) + model.TotalNoOfGypsyFD;
                        model.OverAllGypsy = model.TotalNoOfGypsyHDFD + (model.TotalNoOfGypsyVIP * 2);
                        model.TotalNoOfGypsyVIPBooking = model.TotalNoOfGypsyVIP * 2;

                    }


                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_BookedTicketSeats" + "_" + "OnlineBookingSeatsRepo", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return model;
        }

        public OnlineBookingSeatsModel Save_BookedTicketSeats(OnlineBookingSeatsModel model, string Action)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_SaveOnlineBookingSeats", Conn);
                cmd.Parameters.AddWithValue("@Action", Action);
                cmd.Parameters.AddWithValue("@VehicleIDFDFD", model.VehicleIDFDFD);
                cmd.Parameters.AddWithValue("@VehicleIDVIP", model.VehicleIDVIP);
                cmd.Parameters.AddWithValue("@TotalNoOfGypsyFD", model.TotalNoOfGypsyFD);
                cmd.Parameters.AddWithValue("@TotalNoOfGypsyHD", model.TotalNoOfGypsyHD);
                cmd.Parameters.AddWithValue("@TotalNoOfGypsyVIP", model.TotalNoOfGypsyVIP);
                cmd.Parameters.AddWithValue("@TotalSeatForCWLW", model.TotalSeatForCWLW);
                cmd.Parameters.AddWithValue("@TotalSeatForCCF", model.TotalSeatForCCF);
                cmd.Parameters.AddWithValue("@TotalSeatForIncharge", model.TotalSeatForIncharge);
                cmd.Parameters.AddWithValue("@UserId", Convert.ToInt64(HttpContext.Current.Session["UserID"].ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    model.msg = Convert.ToString(dt.Rows[0]["msg"]);
                    model.Status = Convert.ToInt32(dt.Rows[0]["Status"]);
                }

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Save_BookedTicketSeats" + "_" + "OnlineBookingSeatsRepo", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return model;
        }
    }
}