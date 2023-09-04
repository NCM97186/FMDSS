using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace FMDSS.App_Start
{
    public enum infoFlag
    {
        PF = 0,
        P = 1,
        F = 2

    }

    public class BhamashaError
    {

        public string errorcode { get; set; }
        public string errorDescription { get; set; }

    }

    public static class cls_Bhamasha
    {


        public static BhamashaRoot GetBhamashaInfo(string BhamashaId)
        {
            using (BhamashaServices.PRDWSDLClient Client = new BhamashaServices.PRDWSDLClient())
            {
                string reqXML = string.Format("<root><Info><familyId>{0}</familyId><ackId></ackId><aadharId></aadharId></Info></root>", BhamashaId);
                string sXML = Client.bhamashahInfo(reqXML, "FMDSS", infoFlag.PF.ToString());




                XmlSerializer deserializer = new XmlSerializer(typeof(BhamashaRoot));
                using (var reader = new StringReader(sXML))
                {
                    object obj = deserializer.Deserialize(reader);
                    BhamashaRoot XmlData = (BhamashaRoot)obj;

                    reader.Close();
                    return XmlData;
                }
            }
        }


        public static BhamashaRoot GetMemberInfo(string AckId, string adhar)
        {
            using (BhamashaServices.PRDWSDLClient Client = new BhamashaServices.PRDWSDLClient())
            {
                string reqXML = string.Format("<root><Info><familyId></familyId><ackId>{0}</ackId><aadharId>{1}</aadharId></Info></root>", AckId, adhar);
                string sXML = Client.bhamashahInfo(reqXML, "FMDSS", infoFlag.P.ToString());

                XmlSerializer deserializer = new XmlSerializer(typeof(BhamashaRoot));
                using (var reader = new StringReader(sXML))
                {

                    object obj = deserializer.Deserialize(reader);
                    BhamashaRoot XmlData = (BhamashaRoot)obj;
                    HttpContext.Current.Session["BhamashahMemberDOB"] = XmlData.PersonalInfo.Member[0].Dob;
                    System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("ru-RU");
                    XmlData.PersonalInfo.Member[0].Dob = GetAge(Convert.ToDateTime(XmlData.PersonalInfo.Member[0].Dob, cul));
                    reader.Close();
                    return XmlData;
                }
            }
        }


        public static string GetAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return Convert.ToString((a - b) / 10000);
        }


    }
}