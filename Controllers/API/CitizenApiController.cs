using FMDSS.CustomModels.Models;
using FMDSS.Models;
using FMDSS.Models.ForestProduction;
using FMDSS.Models.Master;
using FMDSS.Models.MIS;
using FMDSS.Repository;
using FMDSS.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace FMDSS.Controllers.API
{

    public class CitizenApiController : ApiController
    {
        [HttpGet]
        public DataSetResponse GetPermitAvailibility(int PlaceId, string BookingDate)
        {
            DataSetResponse response = new DataSetResponse();
            string bookingDate = BookingDate;
            try
            {
                MiddleWareModules middleWareModules = new MiddleWareModules();
                middleWareModules.AvailList = middleWareModules.GetPermitAvailibility(PlaceId, bookingDate);
                middleWareModules.AvailList.ToList().ForEach(s => s.ShiftName = (s.ShiftType == 1 ? "Morning" : (s.ShiftType == 2 ? "Evening" : "Full Day")));
                middleWareModules.PermitAvailList = new List<BookingPermit>();
                BookingPermit bp;

                int zoneid = 0; string zoneName = "";
                int ShiftType1 = 0; string ShiftName1 = ""; int Gypsy1 = 0; int Canter1 = 0;
                int ShiftType2 = 0; string ShiftName2 = ""; int Gypsy2 = 0; int Canter2 = 0;
                int ShiftType3 = 0; string ShiftName3 = ""; int Gypsy3 = 0; int Canter3 = 0;
                int PlaceID = 0;
                if (PlaceId != 65)
                {
                    foreach (BookingPermitDetail bpd in middleWareModules.AvailList)
                    {

                        if (zoneid != bpd.ZoneId && zoneid == 0)
                        {
                            zoneid = bpd.ZoneId;
                            zoneName = bpd.ZoneName;
                            ShiftType1 = bpd.ShiftType;
                            ShiftName1 = bpd.ShiftName;
                            Gypsy1 = bpd.Gypsy;
                            Canter1 = bpd.Canter;
                            PlaceID = bpd.PlaceID;
                        }
                        else if (zoneid == bpd.ZoneId)
                        {
                            zoneName = bpd.ZoneName;
                            PlaceID = bpd.PlaceID;
                            if (ShiftType2 == 0)
                            {
                                ShiftType2 = bpd.ShiftType;
                                ShiftName2 = bpd.ShiftName;
                                Gypsy2 = bpd.Gypsy;
                                Canter2 = bpd.Canter;
                            }
                            else
                            if (ShiftType3 == 0)
                            {
                                ShiftType3 = bpd.ShiftType;
                                ShiftName3 = bpd.ShiftName;
                                Gypsy3 = bpd.Gypsy;
                                Canter3 = bpd.Canter;
                            }
                        }
                        else
                        {
                            bp = new BookingPermit();

                            bp.PlaceID = PlaceId;
                            bp.ZoneId = zoneid;
                            bp.ZoneName = zoneName;
                            bp.ShiftType1 = ShiftType1;
                            bp.ShiftName1 = ShiftName1;
                            bp.Gypsy1 = Gypsy1;
                            bp.Canter1 = Canter1;

                            bp.ShiftType2 = ShiftType2;
                            bp.ShiftName2 = ShiftName2;
                            bp.Gypsy2 = Gypsy2;
                            bp.Canter2 = Canter2;

                            bp.ShiftType3 = ShiftType3;
                            bp.ShiftName3 = ShiftName3;
                            bp.Gypsy3 = Gypsy3;
                            bp.Canter3 = Canter3;


                            middleWareModules.PermitAvailList.Add(bp);
                            zoneName = ""; PlaceID = 0;
                            ShiftType1 = 0; ShiftName1 = ""; Gypsy1 = 0; Canter1 = 0;
                            ShiftType2 = 0; ShiftName2 = ""; Gypsy2 = 0; Canter2 = 0;
                            ShiftType3 = 0; ShiftName3 = ""; Gypsy3 = 0; Canter3 = 0;

                            zoneid = bpd.ZoneId;
                            zoneName = bpd.ZoneName;
                            ShiftType1 = bpd.ShiftType;
                            ShiftName1 = bpd.ShiftName;
                            Gypsy1 = bpd.Gypsy;
                            Canter1 = bpd.Canter;
                            PlaceID = bpd.PlaceID;
                        }

                    }
                    if (zoneName != "")
                    {
                        bp = new BookingPermit();

                        bp.ZoneId = zoneid;
                        bp.ZoneName = zoneName;
                        bp.ShiftType1 = ShiftType1;
                        bp.ShiftName1 = ShiftName1;
                        bp.Gypsy1 = Gypsy1;
                        bp.Canter1 = Canter1;

                        bp.ShiftType2 = ShiftType2;
                        bp.ShiftName2 = ShiftName2;
                        bp.Gypsy2 = Gypsy2;
                        bp.Canter2 = Canter2;

                        bp.ShiftType3 = ShiftType3;
                        bp.ShiftName3 = ShiftName3;
                        bp.Gypsy3 = Gypsy3;
                        bp.Canter3 = Canter3;
                        bp.PlaceID = PlaceID;
                        middleWareModules.PermitAvailList.Add(bp);
                    }
                }


                DataSet dataSet = new DataSet();

                string jsonAvailList = JsonConvert.SerializeObject(middleWareModules.AvailList);
                string jsonPermitAvailList = JsonConvert.SerializeObject(middleWareModules.PermitAvailList);
                DataTable dtAvailList = (DataTable)JsonConvert.DeserializeObject(jsonAvailList, (typeof(DataTable)));
                DataTable dtPermitAvailList = (DataTable)JsonConvert.DeserializeObject(jsonPermitAvailList, (typeof(DataTable)));
                dataSet.Tables.Add(dtAvailList);
                dataSet.Tables.Add(dtPermitAvailList);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dataSet };
                //response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dataSet };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse GetBookingFee(int PlaceId, int BookingType, int NoOfPersons = 1)
        {
            DataSetResponse response = new DataSetResponse();
            MiddleWareModules middleWareModules = new MiddleWareModules();
            try
            {
                DataTable dt = middleWareModules.GetBookingFeeDetails(PlaceId, BookingType, NoOfPersons);
                string strPlaceList = "1,21,22,30,58,59,60,61,66,67,72";
                string[] spl = strPlaceList.Split(',');
                bool isExist = Array.Exists(spl, x => x.Equals(PlaceId.ToString()));
                if (isExist == true)
                {
                    middleWareModules.bookingFeeOther = Globals.Util.GetListFromTable<BookingFeesOther>(dt);
                }
                else
                {
                    if (dt.Rows[0]["PlaceId"].ToString() == "65")
                    {
                        int i = -1;
                        List<BookingFees> list = Globals.Util.GetListFromTable<BookingFees>(dt);
                        string IndianGypsyFee = "0"; string ForeignerGypsyFee = "0";
                        middleWareModules.bookingFeeList = new List<BookingFees>();
                        foreach (var itm in list)
                        {
                            i++;
                            //< td class="inth">@item.IndianGypsyFee</td>
                            //                 <td class="inth">@item.ForeignerGypsyFee</td>
                            //                 <td class="inth">@item.IndianCanterFee</td>
                            //                 <td class="inth">@item.ForeignerCanterFee</td>
                            if (i % 2 == 0)
                            {
                                IndianGypsyFee = itm.IndianGypsyFee;
                                ForeignerGypsyFee = itm.ForeignerGypsyFee;
                            }
                            else
                            {
                                BookingFees bookingFees = new BookingFees();
                                bookingFees.PlaceId = itm.PlaceId;
                                bookingFees.IndianGypsyFee = IndianGypsyFee;
                                bookingFees.ForeignerGypsyFee = ForeignerGypsyFee;
                                bookingFees.IndianCanterFee = itm.IndianGypsyFee;
                                bookingFees.ForeignerCanterFee = itm.ForeignerGypsyFee;

                                middleWareModules.bookingFeeList.Add(bookingFees);
                            }
                        }
                    }
                    else
                        middleWareModules.bookingFeeList = Globals.Util.GetListFromTable<BookingFees>(dt);
                }





                DataSet dataSet = new DataSet();

                string jsonBookingFeeList = JsonConvert.SerializeObject(middleWareModules.bookingFeeList);
                string jsonBookingFeeOther = JsonConvert.SerializeObject(middleWareModules.bookingFeeOther);
                DataTable dtBookingFeeList = (DataTable)JsonConvert.DeserializeObject(jsonBookingFeeList, (typeof(DataTable)));
                DataTable dtBookingFeeOther = (DataTable)JsonConvert.DeserializeObject(jsonBookingFeeOther, (typeof(DataTable)));
                if (dtBookingFeeList != null)
                    dataSet.Tables.Add(dtBookingFeeList);
                if (dtBookingFeeOther != null)
                    dataSet.Tables.Add(dtBookingFeeOther);
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dataSet };
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse CheckTicketInfo(string RequestId)
        {
            DataSetResponse response = new DataSetResponse();
            Ticker obj = new Ticker();

            try
            {

                DataTable dtf = obj.Check_Ticket(RequestId);
                if (dtf.Rows.Count > 0)
                {

                    DataSet dataSet = new DataSet();

                    if (dtf != null)
                        dataSet.Tables.Add(dtf);

                    response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dataSet };
                }
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        [HttpGet]
        public DataSetResponse Failedtransaction(string VisitDate, string ssoId)
        {
            DataSetResponse response = new DataSetResponse();
            DataSet dataSet = new DataSet();

            if (VisitDate == null)
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Invalid Visit Date", Data = dataSet };
            else if (VisitDate.Trim() == "")
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Invalid Visit Date", Data = dataSet };
            if (ssoId == null)
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Invalid SSO ID", Data = dataSet };
            else if (ssoId.Trim() == "")
                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Invalid SSO ID", Data = dataSet };

            FMDSS.Models.cls_UserSession.clsFailReport fail = new FMDSS.Models.cls_UserSession.clsFailReport();
            fail.ssoId = ssoId; fail.VisitDate = VisitDate;

            try
            {

                cls_UserSession oSession = new cls_UserSession();
                List<FMDSS.Models.cls_UserSession.listFail> oFailList = new List<FMDSS.Models.cls_UserSession.listFail>();

                List<ViewTicketRemaningDT1> List = new List<ViewTicketRemaningDT1>();

                DataTable oDt = oSession.GetFailRecord(fail);
                if (oDt.Rows.Count > 0)
                {
                   
                    if (oDt != null)
                        dataSet.Tables.Add(oDt);
                    response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dataSet };
                }
            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        ////// GET api/citizenapi
        ////public IEnumerable<string> Get()
        ////{
        ////    return new string[] { "value1", "value2" };
        ////}

        ////// GET api/citizenapi/5
        ////public string Get(int id)
        ////{
        ////    return "value";
        ////}

        ////// POST api/citizenapi
        ////public void Post([FromBody]string value)
        ////{
        ////}

        ////// PUT api/citizenapi/5
        ////public void Put(int id, [FromBody]string value)
        ////{
        ////}

        ////// DELETE api/citizenapi/5
        ////public void Delete(int id)
        ////{
        ////}
    }
}
