using FMDSS.Globals;
using FMDSS.Models.Plantation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class plantationController : ApiController
    {

        string Url = Util.GetAppSettings("FMDSS2_API");

        public HttpResponseMessage LoginUser(Login request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url+"api/UserAccountApi/");
                //HTTP POST
                var postTask = client.PostAsJsonAsync<Login>("LoginUser", request);
                postTask.Wait();

                var response = postTask.Result;

                // Info.  
                return response;

            }
        }


        public HttpResponseMessage Getconditiondetail()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url+"api/useraccountapi/");

                //HTTP POST
                var postTask = client.GetAsync("getconditiondetail");
                postTask.Wait();

                var response = postTask.Result;

                // Info.  
                return response;

            }
        }

        public HttpResponseMessage GetUserSitesDetail(string UserId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url+"api/UserAccountApi/");

                //HTTP POST
                var postTask = client.GetAsync("GetUserSitesDetail?UserId=" + UserId);
                postTask.Wait();

                var response = postTask.Result;

                // Info.  
                return response;

            }
        }

        public HttpResponseMessage StartEndPlantTrip(UserTripMobileDataFormat plantTripData)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Url+"api/UserAccountApi/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<UserTripMobileDataFormat>("StartEndPlantTrip", plantTripData);
                postTask.Wait();

                var response = postTask.Result;

                // Info.  
                return response;

            }
        }

       
    }
}
