using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models.GVChoice.IGVChoice;
namespace FMDSS.Models.GVChoice
{
    public class GVChoice
    {
        public long TicketId { get; set; }
        public string RequestId { get; set; }
        public string ChoiceRequestId { get; set; }
        public long PlaceId { get; set; }
        public string PlaceName { get; set; }
        public long ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VisitDate { get; set; }
        public string BookingDate { get; set; }
        public int TotalMember { get; set; }
        public int VehicleOrBoatId { get; set; }
        public string VehicleNumber { get; set; }
        public decimal VehileChoiceAmt { get; set; }
        public int GuideId { get; set; }
        public string GuideName { get; set; }
        public decimal GuideChoiceAmt { get; set; }
        public bool IsGuideChoice { get; set; }
        public bool IsVehicleChoice { get; set; }
        public decimal GuideChoiceGSTAmt { get; set; }
        public decimal VehileChoiceGSTAmt { get; set; }
        public Int16 status { get; set; }
        public string respone { get; set; }
        public int ChoiceType { get; set; }
        public decimal TotalChoiceAmt { get; set; }
        public decimal TotalChoiceGSTAmt { get; set; }
        public string TransDate { get; set; }
        public string ItemName { get; set; }

        public decimal OnlineCitizenEmitraCharge { get; set; }
    }
    public class GV_ChoiceView
    {
        public List<GVChoice> gvChoiceList { get; set; }
        public GVChoice gvChoice { get; set; }
        public List<BoatProp> BoatSelectList { get; set; }
        public List<VehicleProp> VehicleSelectList { get; set; }
        public List<SelectListItem> GuideSelectList { get; set; }
    }
    
    public class BoatProp
    {
        public int BoatId { get; set; }
        public string BoatNumber { get; set; }
        public int TotalSeats { get; set; }
        public int BoatOwnerId { get; set; }
        public int PlaceId { get; set; }
    }
    public class VehicleProp
    {
        public int VehicleId { get; set; }
        public string VehicleNumber { get; set; }
        public int TotalSeats { get; set; }
        public int BoatOwnerId { get; set; }
        public int PlaceId { get; set; }
    }
    public class GuideProp
    {
        public int GuideId { get; set; }
        public string GuideCode { get; set; }
        public string GuideName { get; set; }
        public string GuideMobileNo { get; set; }
        public string GuideRegNo { get; set; }
        public string RegValidUpTo { get; set; }
        public int PlaceId { get; set; }
        public long UserId { get; set; }
    }
    public class GV_PayStatus
    {
        public long UserId { get; set; }
        public bool isValidResponse { get; set; }
        public string ResponseMsg { get; set; }
        public string RequestId { get; set; }
        public string ChoiceRequestId { get; set; }
        public int ResponseStaus { get; set; }
        public string EmitraResponse { get; set; }
        public string TicketId { get; set; }
        public string TransactionId { get; set; }
        public int PaymentMode { get; set; }
        public decimal EmitraAmount { get; set; }


    }
    public class GVChoiceService : DAL, INTF_GVChoice
    {
        public DataSet GetChoiceDetails(string RequestId,long UserId,int BookingType)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetChoiceGuideVehicle"),
                                        new SqlParameter("@RequestId",RequestId),
                                        new SqlParameter("@UserId",UserId),
                                        new SqlParameter("@BookingType",BookingType),                                        
                                   };
            DataSet ds = new DataSet();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            return ds;
        }
        public List<BoatProp> GetBoatNumberList(long PlaceId)
        {
            throw new NotImplementedException();
        }
        public List<GVChoice> GetGVChoiceForReceiptList(long UserId,int bookingType , string RequestId = "", string ChoiceRequestId = "")
        {
            List<GVChoice> GetGvChoiceTransList = new List<GVChoice>();
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetChoiceForReceiptList"),
                                        new SqlParameter("@RequestId",RequestId),
                                        new SqlParameter("@ChoiceRequestId",ChoiceRequestId),
                                        new SqlParameter("@UserId",UserId),
                                        new SqlParameter("@BookingType",bookingType),

                                   };
            DataTable ds = new DataTable();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            GetGvChoiceTransList = Globals.Util.GetListFromTable<GVChoice>(ds);
            return GetGvChoiceTransList;
        }

        public List<GVChoice> GetGVChoiceTransactionList(long UserId,ref string ReturnId, int bookingType , string RequestId = "", string ChoiceRequestId = "")
        {
            List<GVChoice> GetGVChoiceTransList = new List<GVChoice>();
            SqlParameter[] param = {
                                            new SqlParameter("@ActionCode","GetSavedChoiceList"),
                                            new SqlParameter("@RequestId",RequestId),
                                            new SqlParameter("@ChoiceRequestId",ChoiceRequestId),
                                            new SqlParameter("@UserId",UserId),
                                            new SqlParameter("@SentTicketId",ReturnId),
                                            new SqlParameter("@BookingType",bookingType),

                                       };
            DataSet ds = new DataSet();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            GetGVChoiceTransList = Globals.Util.GetListFromTable<GVChoice>(ds.Tables[0]);
            if(ds.Tables[1].Rows.Count>0)
                ReturnId = ds.Tables[1].Rows[0]["RequestID"].ToString();

            return GetGVChoiceTransList;
        }

        public List<SelectListItem> GetGvGuideList(long PlaceId)
        {
            List<SelectListItem> GuideSelectList = new List<SelectListItem>();
            SqlParameter[] param = {
                                            new SqlParameter("@ActionCode","GetGVGuideList"),
                                            new SqlParameter("@PlaceId",PlaceId),
                                       };
            DataTable ds = new DataTable();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            GuideSelectList = GetSelectList(ds, "GuideId", "GuideName");
            return GuideSelectList; 
        }

        public List<VehicleProp> GetVehicleNumberList(long PlaceId,string VehicleType)
        {
           
            SqlParameter[] param = {
                                            new SqlParameter("@ActionCode","GetVehicleNumberList"),
                                            new SqlParameter("@PlaceId",PlaceId),
                                            new SqlParameter("@VehicleType",VehicleType),
                                       };
            DataTable ds = new DataTable();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            List<VehicleProp> VehicleList = Globals.Util.GetListFromTable<VehicleProp>(ds);
            return VehicleList;
        }

        
        public DataSet UpdateChoiceDetails(GVChoice gvChoice, string ChoiceRequestId, string EmitraResponse, int ResponseStatus, bool IsValidResponse,int BookingType,int IsLiveOrUAT)
        {
            string CommonNames =(gvChoice.VehicleNumber==null?"":(gvChoice.VehicleNumber.Length > 0 ? gvChoice.VehicleName : ""));
            CommonNames += (CommonNames.Length > 0 ? "," + (gvChoice.GuideId > 0 ? "GUIDE" : "") : (gvChoice.GuideId > 0 ? "GUIDE" : ""));

            SqlParameter[] param = {
                                    new SqlParameter("@RequestId",gvChoice.RequestId),
                                    new SqlParameter("@ChoiceRequestId",ChoiceRequestId),
                                    new SqlParameter("@GuideId",gvChoice.GuideId),
                                    new SqlParameter("@GuideChoiceAmt",gvChoice.GuideChoiceAmt),
                                    new SqlParameter("@VehicleOrBoatId",gvChoice.VehicleOrBoatId),
                                    new SqlParameter("@VehicleNumber",gvChoice.VehicleNumber),
                                    new SqlParameter("@VehileChoiceAmt",gvChoice.VehileChoiceAmt),
                                    new SqlParameter("@ChoiceType",gvChoice.ChoiceType),
                                    new SqlParameter("@IsGuideChoice",(gvChoice.ChoiceType==1 || gvChoice.ChoiceType==3?1:0)),
                                    new SqlParameter("@IsVehicleChoice",(gvChoice.ChoiceType==2 || gvChoice.ChoiceType==3?1:0)),
                                    new SqlParameter("@EmitraResponse",EmitraResponse),
                                    new SqlParameter("@ResponseStatus",ResponseStatus),
                                    new SqlParameter("@IsValidResponse",(IsValidResponse==true?1:0)),
                                    new SqlParameter("@VehicelCommonName",gvChoice.VehicleName),
                                    new SqlParameter("@BookingType",BookingType),
                                    new SqlParameter("@IsLiveOrUAT",(IsLiveOrUAT==0?0:1)),
                                    new SqlParameter("@CommonNameStr",CommonNames),
                                                                  
                                };
            DataSet ds = new DataSet();
            Fill(ds, "spGV_UpdateChoiceGuideVehicle", param);
            return ds;
        }
       
        public DataTable SaveAndGetPGChoiceRequestId(string RequestId) {
            SqlParameter[] param = {
                                             new SqlParameter("@RequestId",RequestId),
                                       };
            DataTable ds = new DataTable();
            Fill(ds, "sp_ChoiceGVSavePGRequest", param);
            return ds;
        }
        private List<SelectListItem> GetSelectList(DataTable dataTable, string ValueNameField, string TextNameField)
        {
            List<SelectListItem> selectlist = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in dataTable.Rows)
            {
                selectlist.Add(new SelectListItem { Text = @dr[TextNameField].ToString(), Value = @dr[ValueNameField].ToString() });
            }
            return selectlist;
        }
        public DataTable CheckGuideBookedStatus(int GuideId, int ShiftId, string VisitDate)
        {
            SqlParameter[] param = {
                                             new SqlParameter("@ActionCode","CheckGuideBookedStatus"),
                                             new SqlParameter("@ShiftId",ShiftId),
                                             new SqlParameter("@VisitDate",VisitDate),
                                             new SqlParameter("@GuideId",GuideId),
                                       };
            DataTable ds = new DataTable();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            return ds;
        }
         public DataTable CheckVehicleBookedStatus(int VehicleId, int ShiftId, string VisitDate)
        {
            SqlParameter[] param = {        new SqlParameter("@ActionCode","CheckVehicleBookedStatus"),
                                            new SqlParameter("@VehicleOrBoatId",VehicleId),
                                            new SqlParameter("@ShiftId",ShiftId),
                                            new SqlParameter("@VisitDate",VisitDate),
                                           
                                       };
            DataTable ds = new DataTable();
            Fill(ds, "spGV_ChoiceGuideVehicle", param);
            return ds;
        }
    }
}