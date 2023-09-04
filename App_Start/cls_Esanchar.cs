using FMDSS.eSanchar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FMDSS.App_Start
{
    public static class cls_Esanchar
    {
        public static bool SendeSancharMessage(string ServiceName,string MobileNumber, string Message)
        {

            string eSancharUserName= WebConfigurationManager.AppSettings["eSancharUserName"];
            string eSancharPassword= WebConfigurationManager.AppSettings["eSancharPassword"];
            using (eSancharServiceSoapClient Client = new eSancharServiceSoapClient())
            {
                if (Client.PingService())
                {
                    AuthHeader header = new AuthHeader();
                    header.Username = eSancharUserName;
                    header.Password = eSancharPassword;
                    return Client.PostMessage(header, ServiceName, "FMDSS"+DateTime.Now.ToString("dd.MM.yyyy"), MobileNumber, Message);
                }
                else
                    return false;
            }
        }
    }
}