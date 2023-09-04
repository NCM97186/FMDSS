using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace FMDSS.Models
{
    public class cls_RTODetails
    {
        [XmlRoot(ElementName = "VehicleDetails")]
        public class VehicleDetails
        {
            [XmlElement(ElementName = "stautsMessage")]
            public string StautsMessage { get; set; }
            [XmlElement(ElementName = "rc_regn_no")]
            public string Rc_regn_no { get; set; }
            [XmlElement(ElementName = "rc_owner_name")]
            public string Rc_owner_name { get; set; }
            [XmlElement(ElementName = "rc_vh_class_desc")]
            public string Rc_vh_class_desc { get; set; }
            [XmlElement(ElementName = "rc_chasi_no")]
            public string Rc_chasi_no { get; set; }
            [XmlElement(ElementName = "rc_eng_no")]
            public string Rc_eng_no { get; set; }
            [XmlElement(ElementName = "rc_maker_desc")]
            public string Rc_maker_desc { get; set; }
            [XmlElement(ElementName = "rc_maker_model")]
            public string Rc_maker_model { get; set; }
            [XmlElement(ElementName = "rc_status_as_on")]
            public string Rc_status_as_on { get; set; }
        }


        public static string decrypt(string Text, string secretkey)
        {
            VahanINFO.VahanInfoClient VahanInfoClients = new VahanINFO.VahanInfoClient();

            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;

            rijndaelCipher.Padding = PaddingMode.PKCS7;

            rijndaelCipher.KeySize = 128;

            rijndaelCipher.BlockSize = 128;

            byte[] encryptedData = Convert.FromBase64String(Text);

            byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(secretkey);

            byte[] keyBytes = new byte[16];

            int len = pwdBytes.Length;

            if (len > keyBytes.Length) len = keyBytes.Length;

            System.Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;

            rijndaelCipher.IV = keyBytes;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();

            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);

        }


        public static cls_RTODetails.VehicleDetails GetRTO(string VechileRegistrationNumber)
        {



            VahanINFO.VahanInfoClient VahanInfoClients = new VahanINFO.VahanInfoClient();
            string xml = VahanInfoClients.getDetails("RJFORESTDEPTT", VechileRegistrationNumber);
            xml = cls_RTODetails.decrypt(xml, "RjForEstdePtt@001 ");

            // string xml = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?><VehicleDetails><stautsMessage>OK</stautsMessage><rc_regn_no>RJ14CN1724</rc_regn_no><rc_owner_name>VINOD KHATRI</rc_owner_name><rc_vh_class_desc>L.M.V. (CAR)</rc_vh_class_desc><rc_chasi_no>MA3FHEB1S00133774</rc_chasi_no><rc_eng_no>D13A1701056</rc_eng_no><rc_maker_desc>MARUTI SUZUKI INDIA LTD(MUL)</rc_maker_desc><rc_maker_model>MARUTI SWIFT VDI</rc_maker_model><rc_status_as_on>30-Jun-2016</rc_status_as_on></VehicleDetails>";

            cls_RTODetails.VehicleDetails RTOVechileXmlData = null;
            if (xml != "Vehicle Registration No not valid")
            {

                XmlSerializer deserializer = new XmlSerializer(typeof(cls_RTODetails.VehicleDetails));

                using (var reader = new StringReader(xml))
                {
                    object obj = deserializer.Deserialize(reader);
                    RTOVechileXmlData = (cls_RTODetails.VehicleDetails)obj;
                    reader.Close();
                }
            }
            else
            {
                RTOVechileXmlData.StautsMessage = "False";
            }
            return RTOVechileXmlData;
        }

    }



}