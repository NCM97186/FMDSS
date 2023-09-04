using FMDSS.APIDAL;
using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using FMDSS.Models.FmdssContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FMDSS.APIRepo
{
    public class ZooBookingRepo : IZooBooking
    {
        private readonly FmdssContext fmdsscontext;
        public ZooBookingRepo()
        {
            if (fmdsscontext == null)
            {
                fmdsscontext = new FmdssContext();
            }
        }

        public ZooBookingModelResponse ZooBookingPlaces()
        {
            ZooBookingModelResponse response = new ZooBookingModelResponse();
            try
            {
                DataSet ds = new DataSet();
                ds = FMDSS.APIDAL.ZooBookingDAL.ZooBookingPlaces();
                response.Data = Util.GetListFromTable<ZooBookingModel>(ds, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public ShiftModelResponse GetShift()
        {
            ShiftModelResponse response = new ShiftModelResponse();
            try
            {
                var objShift = FMDSS.APIDAL.ZooBookingDAL.GetShift();
                response.Data = objShift; //Util.GetListFromTable<ShiftModel>(objShift, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        public AvaliableTicketModelResponse ChkTicketAvailability(int PlaceId, string ShiftType, string DateofVisit, string KioskUserId, string Conn = null)
        {
            AvaliableTicketModelResponse response = new AvaliableTicketModelResponse();
            try
            {
                DataSet ds = new DataSet();
                ds = FMDSS.APIDAL.ZooBookingDAL.ChkTicketAvailability(PlaceId, ShiftType, DateofVisit, KioskUserId);
                response.Data = Util.GetListFromTable<AvaliableTicketModel>(ds, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }


        public MemberVehicleDetailsResponce MemberVehicleDetails(int PlaceId, string KioskUserId, string Conn = null)
        {
            MemberVehicleDetailsResponce response = new MemberVehicleDetailsResponce();
            try
            {
                DataSet ds = new DataSet();
                ds = FMDSS.APIDAL.ZooBookingDAL.MemberAndVehicleFeelst(PlaceId, KioskUserId, null);
                var memberDetails = Util.GetListFromTable<GetMemberFeelstModel>(ds.Tables[0]);   ///Member details
                var VehicalDetails = Util.GetListFromTable<GetVehicleFeelstModel>(ds.Tables[1]);   ///Vehical details
                var ShiftDetails = Util.GetListFromTable<ShiftModel>(ds.Tables[2]);   ///Vehical details

                MemberVehicallistModel obj = new MemberVehicallistModel();
                obj.GetMemberFeelstModel = memberDetails;
                obj.GetVehicleFeelstModel = VehicalDetails;
                obj.GetShiftModel = ShiftDetails;

                response.Data = obj; ////Util.GetListFromTable<MemberVehicallistModel>(ds.Tables[0]);   ///Member details
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }

        [HttpPost]
        public FinalSubmitResponceAfterSubmit SubmitZooBooking(FinalSubmitModel ZooBookingSubmit)
        {

            FinalSubmitResponceAfterSubmit response = new FinalSubmitResponceAfterSubmit();
            try
            {
                DataSet ds = new DataSet();
                ds = FMDSS.APIDAL.ZooBookingDAL.SubmitZooBooking(ZooBookingSubmit);
                response.Data = Util.GetListFromTable<AfterSubmitModel>(ds, 0);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }


        public UpdateZooTicketResponce UpdateZooBooking(UpdateZooTicketRequest UpdateZooBooking)
        {

            UpdateZooTicketResponce response = new UpdateZooTicketResponce();
            try
            {
                DataTable ds = new DataTable();
                ds = ZooBookingDAL.UpdateZooBooking(UpdateZooBooking);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }


        public UpdateZooTicketResponce UpdateZooBooking_Emitraplus(UpdateZooTicketRequest_EmitraPlus UpdateZooBooking)
        {

            try
            {
                UpdateZooTicketResponce response = new UpdateZooTicketResponce();
                DataTable ds = new DataTable();
                ds = ZooBookingDAL.UpdateZooBooking_EmitraPlus(UpdateZooBooking);

                response.Data = Util.GetListFromTable<GetZooTicketResponceSubmitModel>(ds);
                if (response.Data.Count <= 0)
                {
                    response.Message = "Transaction Failed";
                    response.Status = 0;
                }
                else
                {
                    if (Convert.ToString(ds.Rows[0]["Message"]) != "")
                    {
                        if (Convert.ToInt32(ds.Rows[0]["Status"]) == 0)
                        {
                            response.Status = 0;
                        }
                        response.Message = Convert.ToString(ds.Rows[0]["Message"]);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
