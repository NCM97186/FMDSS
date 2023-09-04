using FMDSS.Entity.Mob_BudgetVM;
using FMDSS.Models;
using FMDSS.Repository.Budget_Mobile;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using FMDSS.Globals;
using FMDSS.Repository;
using Newtonsoft.Json;
using FMDSS.Entity;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Data;
using FMDSS.CustomModels.Models;

namespace FMDSS.Controllers.API
{
    public class MobileLoginController : ApiController
    {
        private IBudget_MobileRepository _repository;
        public MobileLoginController()
        {
            _repository = new Budget_MobileRepository();
        }
        [System.Web.Http.HttpGet]
        public HttpResponseMessage CheckOTP(string SsoId,string OTP)
        {
            string requestContent = string.Empty, responseContent = string.Empty;
            string returnMsg = string.Empty;
           bool isError = false;
            cls_mobileLogin oLogin = new cls_mobileLogin();
            var oObjLoginData = oLogin.CheckOTP(SsoId, OTP);
            string sStatus = Convert.ToString(oObjLoginData.Rows[0]["Status"]);
            UserDetailsVM model = new UserDetailsVM();
            var userAdditionalInfo = _repository.GetUserDetails(SsoId);
            model.status = sStatus;           
            model.UserDetailsAdditionalInfo = Util.GetListFromTable<UserDetailsAdditionalInfo>(userAdditionalInfo, 0).FirstOrDefault();
            model.RoleList = Util.GetListFromTable<DropDownList>(userAdditionalInfo, 1);
            model.FinancialYearList = Util.GetListFromTable<DropDownList>(userAdditionalInfo, 2);
            model.BudgetHeadList = Util.GetListFromTable<DropDownList>(userAdditionalInfo, 3);
            model.SchemeList = Util.GetListFromTable<DropDownList>(userAdditionalInfo, 4);
            model.SantatuaryList = Util.GetListFromTable<CommonDropDownData2>(userAdditionalInfo, 5);
            model.ActivityList = Util.GetListFromTable<DropDownList>(userAdditionalInfo, 6);
            model.SubActivityList = Util.GetListFromTable<CommonDropDownData>(userAdditionalInfo, 7);
            model.SubBudgetHeadList = Util.GetListFromTable<CommonDropDownData>(userAdditionalInfo, 8);

            model.CircleList = Util.GetListFromTable<DropDownList2>(userAdditionalInfo, 9);
            model.DivList = Util.GetListFromTable<DropDownList2>(userAdditionalInfo, 10);
            model.SubActivityUnit = Models.Common.GetMeasurementUnits().Select(x => new DropDownList2 { Text = x.Text, Value = x.Value }).ToList();
            
            returnMsg = Constant.ServiceServeMsg;
            responseContent = JsonConvert.SerializeObject(model);

            return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg, data = model });

            //return Json(new { Status = sStatus }, JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /MobileLogin/
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetOTP(string SsoId)
        {
            cls_mobileLogin oLogin = new cls_mobileLogin();
            bool isError = false;
            cls_UserOTPResponceCode model = new cls_UserOTPResponceCode();
            string returnMsg = string.Empty;
            var oObjLoginData= oLogin.GetOTP(SsoId);
            string departmentName = "Forest";
            model.Mobile = Convert.ToString(oObjLoginData.Rows[0]["Mobile"]); //"9023108090";//
            model.Email = Convert.ToString(oObjLoginData.Rows[0]["EmailId"]);
            model.OTP = Convert.ToString(oObjLoginData.Rows[0]["OTP"]);
            model.Status = Convert.ToString(oObjLoginData.Rows[0]["Status"]);
            //model.UserSmsBody = string.Format( "{0} is the One Time Password(OTP) to process, expires in 2 mins. Verify now ", model.OTP);
            //model.UserSmsBody = string.Format("{0} is the One Time Password(OTP) to process, expires in 2 mins. Verify now.- Forest Department", model.OTP);
            model.UserSmsBody = string.Format("{0} is the One Time Password(OTP) to process, expires in 2 mins. Verify now.- {1} Department, GoR", model.OTP, departmentName);
            
            SMS_EMail_Services.sendSingleSMS_OTP(Convert.ToString(model.Mobile), model.UserSmsBody);
            //SMS_EMail_Services.sendSingleSMS(Convert.ToString(model.Mobile), model.UserSmsBody);
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();

            objSMSandEMAILtemplate.SendMailComman("ALL", "GET_OTP_FOR_MOBILE", model.UserSmsBody, model.UserSmsBody, model.Email, "", "");

            return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg, data = model });

           // return Json(new { Status= Status }, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage mobilelogin(string SsoId)
        {
           
            cls_mobileLogin oLogin = new cls_mobileLogin();
            string returnMsg = string.Empty;
            cls_LoginResponceCode model = new cls_LoginResponceCode();
           
            //DataTable oObjLoginData = oLogin.LoginMobileUser(SsoId);
            DataSet oObjLoginData = oLogin.LoginMobileUser(SsoId);
            

            //LoginDetail login=new LoginDetail { Name= Convert.ToString(oObjLoginData.Tables["Name"]), Designation="" }
            RootObject tables = new RootObject();


            //RootObject tables = new RootObject{ Status=(int)ResponseStatus.Success, Message = "Success",
            //    LoginDetail = new List<LoginDetail>(login),  oObjLoginData.Tables[0], OffenceSummery=  oObjLoginData.Tables[1], TotCountCircle=  oObjLoginData.Tables[2], TotCountDivision =  oObjLoginData.Tables[3], TotCountRange=  oObjLoginData.Tables[4], TotCountNaka =  oObjLoginData.Tables[5], NurseryStockCount=  oObjLoginData.Tables[6], WaterResourecCount =  oObjLoginData.Tables[7] };

            if (Globals.Util.isValidDataSet(oObjLoginData,0))
            {
                tables.Message = "Success";
                tables.loginDetail = Globals.Util.GetListFromTable<LoginDetail>(oObjLoginData, 0);
                tables.OffenceSummery = Globals.Util.GetListFromTable<OffenceSummery>(oObjLoginData, 1);
                tables.TotCountCircles = Globals.Util.GetListFromTable<TotCountCircle>(oObjLoginData, 2);
                tables.TotCountDivisions = Globals.Util.GetListFromTable<TotCountDivision>(oObjLoginData,3);
                tables.TotCountRanges = Globals.Util.GetListFromTable<TotCountRange>(oObjLoginData, 4);
                tables.TotCountNakas = Globals.Util.GetListFromTable<TotCountNaka>(oObjLoginData, 5);
                tables.NurseryStockCount = Globals.Util.GetListFromTable<NurseryStockCount>(oObjLoginData, 6);               
                tables.WaterResourecCount = Globals.Util.GetListFromTable<WaterResourecCount>(oObjLoginData, 7);
                tables.FireAlertCount = Globals.Util.GetListFromTable<FireAlertCount>(oObjLoginData, 8);
            }
            DataSet ds = oLogin.GetZooPlaceDetails(SsoId);

            if (ds.Tables.Count > 0)
            {
                tables.userPlaceDetails = Globals.Util.GetListFromTable<UserPlaceDetail>(ds.Tables[0]);
                tables.fB_bookingTypeList = Globals.Util.GetListFromTable<FBBookingType>(ds.Tables[1]);
            }
            bool isError = false;
            if (oObjLoginData != null)
            {
                if (oObjLoginData.Tables.Count == 0)
                {
                    isError = true;
                    tables.Message = "No Record Fount";
                    tables.Status = 0;
                }
                else
                {
                    if (oObjLoginData.Tables.Count > 0)
                    {
                        isError = true;
                        for (int i = 0; i < oObjLoginData.Tables.Count; i++)
                        {
                            if (oObjLoginData.Tables[i].Rows.Count > 0)
                                isError = false;
                        }
                        if (isError == true)
                        {
                            tables.Message = "No Record Fount";
                            tables.Status = 0;
                        }

                    }
                    else
                    {
                        tables.Message = "No Record Fount";
                        tables.Status = 0;
                        isError = true;
                    }
                }

            }
            else
            {
                tables.Message = "No Record Fount";
                tables.Status = 0;
                isError = true;
            }
            //var response = new DataTableResponse { Status = ResponseStatus.Success, Message = "Success",Data= tables };
            RootObject obj = new RootObject();
            if (tables.Message != null)
                obj = (RootObject)tables;
            else
            {
                tables.Message = "No Record Fount";
                tables.Status = 0;
                obj = (RootObject)tables;
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = obj.Message.ToString(), data = obj } );
            // return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg, data = tables });
            //return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg, data = model });

            //return Json(new {Status= sStatus }, JsonRequestBehavior.AllowGet);
        }
        //public HttpResponseMessage mobilelogin(string SsoId)
        //{
        //    cls_mobileLogin oLogin = new cls_mobileLogin();
        //    string returnMsg = string.Empty;
        //    cls_LoginResponceCode model = new cls_LoginResponceCode();
        //    var oObjLoginData = oLogin.LoginMobileUser(SsoId);
        //    bool isError = false;
        //    model.status = Convert.ToBoolean(oObjLoginData.Rows[0]["Status"]);

        //    return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg, data = model });

        //    //return Json(new {Status= sStatus }, JsonRequestBehavior.AllowGet);
        //}
    }
}
