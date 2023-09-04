using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
namespace FMDSS.Models
{
    public static class DMS_Service
    {

        #region Push Document

        public static UploadResponce DMSPushDocument(string sPath, String FileFullName, DocumentTypeClass documenttype, DMSAttribute dMSAttribute, string TableName, string Filefor = null)
        {

            string RepoDocId = Guid.NewGuid().ToString();

            var baseAddress = System.Configuration.ConfigurationManager.AppSettings["DMSAddDocument"];
            string DMSFilePath = System.Configuration.ConfigurationManager.AppSettings["DMSFixedLandUsagesDocumentPath"];


            string FileName = System.IO.Path.GetFileNameWithoutExtension(sPath);
            string FileExt = System.IO.Path.GetExtension(sPath);





            Byte[] bytes = System.IO.File.ReadAllBytes(sPath);
            String file = Convert.ToBase64String(bytes);
            Attribute attDocumentTitle = new Attribute { SymbolicName = "DocumentTitle", Value = FileName };  /////Here we assign file name
            Attribute attRequest = new Attribute { SymbolicName = "RequestedID", Value = dMSAttribute.RequestedID };
            Attribute attServices = new Attribute { SymbolicName = "ServiceTypeId", Value = dMSAttribute.ServiceTypeId };
            Attribute attPermission = new Attribute { SymbolicName = "PermissionId", Value = dMSAttribute.PermissionId };
            Attribute attSubPermission = new Attribute { SymbolicName = "SubPermissionId", Value = dMSAttribute.SubPermissionId };
            Attribute attModule = new Attribute { SymbolicName = "ModuleId", Value = dMSAttribute.ModuleId };

            List<Attribute> Attobj = new List<Attribute>();
            Attobj.Add(attDocumentTitle);
            Attobj.Add(attRequest);
            Attobj.Add(attServices);
            Attobj.Add(attPermission);
            Attobj.Add(attSubPermission);
            Attobj.Add(attModule);

            Metadata metadata = new Metadata { Attributes = Attobj, File = file, FileExtension = FileExt, MimeType = GetMIMEType(FileExt) };

            PushDMSObject RootJson = new PushDMSObject { classname = documenttype.ToString(), deptkey = "ECC643D6B4E6E6F9", deptname = "fmdss", metadata = metadata, repodocid = RepoDocId, path = DMSFilePath, os = "FMDSS" };


            //InputJson json = new InputJson { FileExtension = ".pdf", ClassName = "FMDSSGenDoc", MimeType = "application/pdf", File = file, Attributes = Attobj };
            //RootObject RootJson = new RootObject { inputJson = json, path = "/fmdss/test/test1/test2/test3", os = "fmdss" };




            var jsonData = new JavaScriptSerializer().Serialize(RootJson);

            //



            var request = (HttpWebRequest)WebRequest.Create(baseAddress);
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";
            request.Headers.Add("X-IBM-Client-Id", "6c4f9996-cebc-45d3-9fd7-babd69fab94e");
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
            }
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors SslPolicyErrors)
            {

                return true;

            };
            var response = (HttpWebResponse)request.GetResponse();
            UploadResponce responceData = new UploadResponce();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = streamReader.ReadToEnd();
                    responceData = new JavaScriptSerializer().Deserialize<UploadResponce>(result);
                    responceData.RepoDocID = RepoDocId;
                    //if (System.IO.File.Exists(sPath))
                    //{
                    //    System.IO.File.Delete(sPath);
                    //}
                }
            }

            UpdateFOrDMS UpdateFOrDMS = new Models.UpdateFOrDMS();

            string FIleNamewithExtn = FileName + FileExt;
            Int64 i = UpdateFOrDMS.UpdateTable_For_DMS(dMSAttribute.RequestedID, FIleNamewithExtn, responceData.Status, responceData.RepoDocID, TableName, Filefor);

            return responceData;

        }

        private static string GetMIMEType(string FileExt)
        {
            switch (FileExt)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                    return "application/jpg";
                case ".jpeg":
                    return "application/jpeg";
                case ".png":
                    return "application/png";
                case ".doc":
                    return "application/doc";
                default:
                    return string.Empty;


            }
        }


        public enum DocumentTypeClass
        {
            ProofOfIdentity,
            NOCCertificates,
            PermissionDocuments,
            AdditionalDocuments,
            NoticeDocuments,
            FinancialDocuments,
            RevenueRecordsAndMaps

        }

        #endregion End PushDocument



        # region Get Document

        public static FetchDMSResponce DMSGetDocument(String RepoDocId, DocumentTypeClass documenttype)
        {

            var baseAddress = System.Configuration.ConfigurationManager.AppSettings["DMSGetDocument"];

            FetchDMSObject RootJson = new FetchDMSObject
            {
                repodocid = RepoDocId,
                os = "FMDSS",
                classname = documenttype.ToString(),
                deptkey = "ECC643D6B4E6E6F9",
                version = "1",
                datareq = "all"
            };

            var jsonData = new JavaScriptSerializer().Serialize(RootJson);

            var request = (HttpWebRequest)WebRequest.Create(baseAddress);
            request.ContentType = "application/json";
            request.MediaType = "application/json";
            request.Accept = "application/json";
            request.Method = "POST";
            request.Headers.Add("X-IBM-Client-Id", "6c4f9996-cebc-45d3-9fd7-babd69fab94e");
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonData);
            }
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors SslPolicyErrors)
            {

                return true;

            };

            var response = (HttpWebResponse)request.GetResponse();
            FetchDMSResponce responceData = new FetchDMSResponce();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var result = streamReader.ReadToEnd();
                    responceData = new JavaScriptSerializer().Deserialize<FetchDMSResponce>(result);
                    responceData.DocID = RepoDocId;
                }
            }

            return responceData;

        }

        #endregion


    }
    public class Attribute
    {
        public string Value { get; set; }
        public string SymbolicName { get; set; }
    }

    public class Metadata
    {
        public string FileExtension { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string File { get; set; }
        public string MimeType { get; set; }
    }

    public class PushDMSObject
    {
        public Metadata metadata { get; set; }
        public string classname { get; set; }
        public string path { get; set; }
        public string os { get; set; }
        public string deptname { get; set; }
        public string deptkey { get; set; }
        public string repodocid { get; set; }
    }

    public class FetchDMSObject
    {

        public string repodocid { get; set; }
        public string os { get; set; }
        public string classname { get; set; }
        public string deptkey { get; set; }
        public string version { get; set; }
        public string datareq { get; set; }
    }
    public class FetchDMSResponce
    {
        public string Status { get; set; }
        public string DocID { get; set; }
        public string DocExists { get; set; }
        public string File { get; set; }
        public string IncidentID { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class DMSAttribute
    {
        public string RequestedID { get; set; }
        public string ServiceTypeId { get; set; }
        public string PermissionId { get; set; }
        public string SubPermissionId { get; set; }
        public string ModuleId { get; set; }
    }

    public class UploadResponce
    {
        public string Status { get; set; }
        public string DocID { get; set; }
        public string DocExists { get; set; }
        public string IncidentID { get; set; }
        public string ResponseCode { get; set; }
        public string ErrorMessage { get; set; }
        public string RepoDocID { get; set; }
    }

    public class UpdateFOrDMS : DAL
    {
        public Int64 UpdateTable_For_DMS(string RequestedID, string FileName, string DocStatus, string RepoDocID, string TableName, string Filefor = null)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Update_For_DMS", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestedID", RequestedID);
                cmd.Parameters.AddWithValue("@FileName", FileName);
                cmd.Parameters.AddWithValue("@DocStatus", Convert.ToInt32(DocStatus));
                cmd.Parameters.AddWithValue("@RepoDocID", RepoDocID);
                cmd.Parameters.AddWithValue("@TableName", TableName);
                cmd.Parameters.AddWithValue("@Filefor", Filefor);
                 chk = Convert.ToInt64(cmd.ExecuteNonQuery());
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateTable_For_DMS" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
 
            }
            finally
            {
                Conn.Close();
            } 
            return chk;
        }
    }

}