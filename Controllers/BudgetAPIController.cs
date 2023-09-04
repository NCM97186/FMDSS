using FMDSS.Entity;
using FMDSS.Entity.Mob_BudgetVM;
using FMDSS.Entity.VM;
using FMDSS.Globals;
using FMDSS.Repository;
using FMDSS.Repository.Budget_Mobile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace FMDSS.Controllers
{
    public class BudgetAPIController : ApiController
    {
        #region [Properties & Variables]
        private IBudget_MobileRepository _repository;
        #endregion

        #region [Constructor]
        public BudgetAPIController()
        {
            _repository = new Budget_MobileRepository();
        }
        #endregion

        #region Login Operation
        #region Login
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request)
        {
            string requestContent = string.Empty, responseContent = string.Empty;
            string returnMsg = string.Empty;
            bool isError = false;
            //LoginResponse loginResponse;
            UserDetails userDetailsResponse;
            UserDetailsVM model = new UserDetailsVM();

            var response = new HttpResponseMessage();
            try
            {
                requestContent = request.Content.ReadAsStringAsync().Result;
                var loginReqModel = JsonConvert.DeserializeObject<LoginReqParam>(requestContent);
                SSO.SSOWSSoapClient client = new SSO.SSOWSSoapClient();

                #region Authenticate operation
                responseContent = client.SSOAuthenticationMobileJSON(Util.GetAppSettings("departmentname"), loginReqModel.UserName, loginReqModel.Password);
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                isError = !loginResponse.valid;
                #endregion

                if (!isError)
                {
                    #region User Details operation
                    var ssoServiceUser = Util.GetAppSettings("SSOServiceUser").Split(',');
                    responseContent = client.GetUserDetailJSON(loginReqModel.UserName, ssoServiceUser[0], ssoServiceUser[1]);
                    model.UserDetails = JsonConvert.DeserializeObject<UserDetails>(responseContent);

                    var userAdditionalInfo = _repository.GetUserDetails(loginReqModel.UserName);


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

                    #endregion
                }
                else
                {
                    returnMsg = loginResponse.message;
                    responseContent = JsonConvert.SerializeObject(model);
                }

                #region Save Activity
                var _objActivity = new ServiceActivity()
                {
                    ModuleName = ModuleName.Budget,
                    Request = requestContent,
                    Response = responseContent,
                    ServiceName = "SSOAuthenticationMobileJSON"
                };
                new CommonActivity().SaveActivity(_objActivity);
                #endregion

                return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg, data = model }); 
            }
            catch (Exception ex)
            {
                #region Save Activity
                var _objActivity = new ServiceActivity()
                {
                    ModuleName = ModuleName.Budget,
                    Request = requestContent,
                    Response = responseContent,
                    ServiceName = "SSOAuthenticationMobileJSON"
                };
                new CommonActivity().SaveActivity(_objActivity);
                #endregion
                return Request.CreateResponse(HttpStatusCode.OK, new { isError = true, msg = ex.Message });
            }
        }
        #endregion
        #endregion 

        #region Budget Allocation Operation
        [HttpPost]
        //Needs work on this
        public HttpResponseMessage SaveBudgetProposal(HttpRequestMessage request)
        {
            string requestContent = string.Empty, responseContent = string.Empty;
            string returnMsg = string.Empty;
            bool isError = false;

            var response = new HttpResponseMessage();
            try
            {

                returnMsg = "Data saved successfully.";
                requestContent = request.Content.ReadAsStringAsync().Result;
                var _objActivity = new ServiceActivity()
                {
                    ModuleName = ModuleName.Budget,
                    Request = requestContent,
                    Response = responseContent,
                    ServiceName = "SaveBudgetProposal"
                };
                new CommonActivity().SaveActivity(_objActivity);

                return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg });
            }
            catch (Exception ex)
            {
                #region Save Activity
                var _objActivity = new ServiceActivity()
                {
                    ModuleName = ModuleName.Budget,
                    Request = requestContent,
                    Response = responseContent,
                    ServiceName = "SaveBudgetProposal"
                };
                new CommonActivity().SaveActivity(_objActivity);
                #endregion
                return Request.CreateResponse(HttpStatusCode.OK, new { isError = true, msg = ex.Message });
            }
        }
        #endregion
        #region  "Proposal List"
        [HttpGet]
        //Needs work on this
        public HttpResponseMessage GetBudgetProposal(HttpRequestMessage request)
        {
            string requestContent = string.Empty, responseContent = string.Empty;
            string returnMsg = string.Empty;
            bool isError = false;

            var response = new HttpResponseMessage();
            try
            {

                var UserId = JsonConvert.DeserializeObject<string>(requestContent);



                returnMsg = "Data saved successfully.";

                requestContent = request.Content.ReadAsStringAsync().Result;
                
                var _objActivity = new ServiceActivity()
                {
                    ModuleName = ModuleName.Budget,
                    Request = requestContent,
                    Response = responseContent,
                    ServiceName = "GetBudgetProposal"
                };
                new CommonActivity().SaveActivity(_objActivity);

                return Request.CreateResponse(HttpStatusCode.OK, new { isError = isError, msg = returnMsg });
            }
            catch (Exception ex)
            {
                #region Save Activity
                var _objActivity = new ServiceActivity()
                {
                    ModuleName = ModuleName.Budget,
                    Request = requestContent,
                    Response = responseContent,
                    ServiceName = "SaveBudgetProposal"
                };
                new CommonActivity().SaveActivity(_objActivity);
                #endregion
                return Request.CreateResponse(HttpStatusCode.OK, new { isError = true, msg = "Invalid Request!" });
            }
        }
            
        #endregion



        #region Get Budget Perposal List Developed by Rajveer


        #endregion
    }
}