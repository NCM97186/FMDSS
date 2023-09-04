using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace FMDSS.Models.CitizenService.PermissionService
{
    public class Payment
    {

        public string RequestString(string MerchantID, string Request_Id, string TransactionAmount, string ReturnUrl, string UserId, string otherparam2, string otherparm3)
        {
            string request = "<REQUEST MERCHANT_ID='" + MerchantID + "' REQUEST_ID='" + Request_Id + "' AMOUNT='" + TransactionAmount + "' RU='" + ReturnUrl + "' OTHER_PARAM1='" + UserId + "' OTHER_PARAM2='" + otherparam2 + "' OTHER_PARAM3='" + otherparm3 + "'/>";
            return ProcesTranscationrequest(request);
            //  return request;
        }
        // protected void Button1_Click(object sender, EventArgs e)
        //  {
        // TextBox1.Text = ProcesTranscationresponce("yXBdEkS6dSHD4W6JRDqLXaoMbBHXJHE6NCtT6dPWP4emXjs71IGqrjqhKSreIIGQZ2FfBB3/GmsyK8yLgS3IGHqaLTrDSbXyMUq7RstTeUDB78nd+dHWeiuRysO70XgiCbh6AtsHCN8VotHgMLGk7RXDTiVaOmaefWj9QtgawKB7goyknnAUqDHxGg48eOV+VcvcJTr/mxprIOe+m4eTMp1pySDgQIhs6oPmykeWG51BcHgGY/d20siIMEOtjve6JL4r30q0yfrD3ap2VA3yAoZIEJ+drbFHE0JzepXAPbvaM8i/Bi4xPMW2Vk8Uh0zMSqaERnDBv2wymJ3GXFoaf8RAw/O2dbK8k18CfbtzdfYXMRSAcANEdczGCSHUIj75UpbTOMs0Veo=");
        // TextBox2.Text = ProcesTranscationrequest("<RESPONSE MERCHANT_ID='EM38862143@5488' REQUEST_ID='RHT100080' AMOUNT='7499.00' RU='http://10.68.106.77/pages/transactionresponse.html' OTHER_PARAM1='100080' OTHER_PARAM2='1212121212' OTHER_PARAM3='abc@gmail.com'></RESPONSE>");
        //   }

        public string ProcesTranscationrequest(string Request)
        {
            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EM!TR@TPS");
            return objEncryptDecrypt.encrypt(Request + "|" + GetChecksum(Request));
        }

        public string ProcesTranscationresponce(string Responce)
        {
            EncryptDecrypt3DES objEncryptDecrypt = new EncryptDecrypt3DES("EM!TR@TPS");
            string originalchecksum = string.Empty, calculatedChecksum = string.Empty, result = "Mismatch checksum", decryptString = string.Empty;
            decryptString = objEncryptDecrypt.decrypt(Responce.Replace(' ', '+'));
            originalchecksum = decryptString.Substring(decryptString.LastIndexOf('|') + 1);
            string response = decryptString.Substring(0, decryptString.LastIndexOf('|'));
            calculatedChecksum = GetChecksum(response);
            if (originalchecksum.ToUpper() == calculatedChecksum.ToUpper())
            {
                result = response;
            }
            return result;
        }

        public string GetChecksum(string requeststring)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(requeststring);
            MemoryStream stream = new MemoryStream(byteArray);
            System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed();
            byte[] bytes = sha.ComputeHash(stream);
            return BitConverter.ToString(bytes).Replace("-", String.Empty);
        }

        public string TransactionStatus(string num)
        {
            string status = "";
            switch (num)
            {
                case "SUCCESS":
                    status = "SUCCESS";
                    break;
                case "FAILED":
                    status = "FAILED";
                    break;
                case "PENDING":
                    status = "FAILED";
                    break;
                case "-1":
                    status = "Fatal Error";
                    break;
                case "-2":
                    status = "Invalid Merchant ID";
                    break;
                case "-3":
                    status = "Invalid User Details";
                    break;
                case "-4":
                    status = "Invalid Data. Encryption/Decryption Issue";
                    break;
                case "-5":
                    status = "Invalid Checksum";
                    break;
                case "-6":
                    status = "Invalid XML";
                    break;
                case "-7":
                    status = "Unexpected Error";
                    break;
                case "-8":
                    status = "Invalid Request Resource";
                    break;

            }
            return status;
        }
    }
}