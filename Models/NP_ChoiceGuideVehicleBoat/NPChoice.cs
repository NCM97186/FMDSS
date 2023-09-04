using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace FMDSS.Models.NP_ChoiceGuideVehicleBoat
{
    public class NP_ChoiceView
    {
        public List<NPChoice> NpChoiceList { get; set; }
        public  NPChoice NpChoice { get; set; }
        public List<NpBoatProp> NpBoatSelectList { get; set; }
        public List<SelectListItem> NpGuideSelectList { get; set; }       
    }
    public class NPChoice
    {
        public long TicketId { get; set; }
        public string RequestId { get; set; }
        public string ChoiceRequestId { get; set; }
        public long PlaceId { get; set; }
        public string PlaceName{ get; set; }
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
        public decimal  GuideChoiceGSTAmt { get; set; }
        public decimal VehileChoiceGSTAmt { get; set; }
        public Int16 status { get; set; }
        public string respone { get; set; }
        public int ChoiceType { get; set; }
        public decimal TotalChoiceAmt { get; set; }
        public decimal TotalChoiceGSTAmt { get; set; }
        public string TransDate { get; set; }
        public string ItemName { get; set; }
        
    }
    public class NpBoatProp
    {
        public int BoatId { get; set; }
        public string BoatNumber { get; set; }
        public int TotalSeats { get; set; }
        public int BoatOwnerId { get; set; }
        public int PlaceId { get; set; }        
    }
    public class NpGuideProp
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
    public class Np_PayStatus
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
    public class NPChoiceService:DAL, INpChoice
    {
       
        public DataSet GetChoiceDetails(string RequestId)
        {
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetChoiceGuideVehicle"),
                                        new SqlParameter("@RequestId",RequestId),                        
                                   };
            DataSet ds = new DataSet();
            Fill(ds, "spNP_ChoiceGuideVehicle", param);
            return ds;
        }
        public DataSet UpdateChoiceDetails(NPChoice npChoice,string ChoiceRequestId,string EmitraResponse,int ResponseStatus,bool IsValidResponse)
        {
            SqlParameter[] param = {                                        
                                        new SqlParameter("@RequestId",npChoice.RequestId),
                                        new SqlParameter("@ChoiceRequestId",ChoiceRequestId),
                                        new SqlParameter("@GuideId",npChoice.GuideId),
                                        new SqlParameter("@GuideChoiceAmt",npChoice.GuideChoiceAmt),
                                        new SqlParameter("@VehicleOrBoatId",npChoice.VehicleOrBoatId),
                                        new SqlParameter("@VehicleNumber",npChoice.VehicleNumber),
                                        new SqlParameter("@VehileChoiceAmt",npChoice.VehileChoiceAmt),
                                        new SqlParameter("@ChoiceType",npChoice.ChoiceType),
                                        new SqlParameter("@IsGuideChoice",(npChoice.ChoiceType==1 || npChoice.ChoiceType==3?1:0)),
                                        new SqlParameter("@IsVehicleChoice",(npChoice.ChoiceType==2 || npChoice.ChoiceType==3?1:0)),
                                        new SqlParameter("@EmitraResponse",EmitraResponse),
                                        new SqlParameter("@ResponseStatus",ResponseStatus),
                                        new SqlParameter("@IsValidResponse",(IsValidResponse==true?1:0)),
                                   };
            DataSet ds = new DataSet();
            Fill(ds, "spNP_UpdateChoiceGuideVehicle", param);
            return ds;
        }


        public List<SelectListItem> GetNpGuideList(long PlaceId)
        {
            List<SelectListItem> NpGuideSelectList = new List<SelectListItem>();
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetNPGuideList"),
                                        new SqlParameter("@PlaceId",PlaceId),
                                   };
            DataTable ds = new DataTable();
            Fill(ds, "spNP_ChoiceGuideVehicle", param);
            NpGuideSelectList = GetSelectList(ds , "GuideId", "GuideName");
            return NpGuideSelectList;
        }
        
        public List<NpBoatProp> GetNpBoatNumberList(long PlaceId)
        {
            List<SelectListItem> NpBoatSelectList = new List<SelectListItem>();
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetBoatNumberList"),
                                        new SqlParameter("@PlaceId",PlaceId),
                                   };
            DataTable ds = new DataTable();
            Fill(ds, "spNP_ChoiceGuideVehicle", param);
            List<NpBoatProp> NPBoatList = Globals.Util.GetListFromTable<NpBoatProp>(ds);
            return NPBoatList;
        }
        public List<NPChoice> GetNpChoiceTransactionList(long UserId, string RequestId="", string ChoiceRequestId = "")
        {
            List<NPChoice> GetNpChoiceTransList = new List<NPChoice>();
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetSavedChoiceList"),
                                        new SqlParameter("@RequestId",RequestId),
                                        new SqlParameter("@ChoiceRequestId",ChoiceRequestId),
                                        new SqlParameter("@UserId",UserId),

                                   };
            DataTable ds = new DataTable();
            Fill(ds, "spNP_ChoiceGuideVehicle", param);
            GetNpChoiceTransList = Globals.Util.GetListFromTable<NPChoice>(ds);
            return GetNpChoiceTransList;
        }
        public List<NPChoice> GetNpChoiceForReceiptList(long UserId, string RequestId = "", string ChoiceRequestId = "")
        {
            List<NPChoice> GetNpChoiceTransList = new List<NPChoice>();
            SqlParameter[] param = {
                                        new SqlParameter("@ActionCode","GetChoiceForReceiptList"),
                                        new SqlParameter("@RequestId",RequestId),
                                        new SqlParameter("@ChoiceRequestId",ChoiceRequestId),
                                        new SqlParameter("@UserId",UserId),

                                   };
            DataTable ds = new DataTable();
            Fill(ds, "spNP_ChoiceGuideVehicle", param);
            GetNpChoiceTransList = Globals.Util.GetListFromTable<NPChoice>(ds);
            return GetNpChoiceTransList;
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
    }
}