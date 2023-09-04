using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xml2CSharp;

namespace ReadXML
{
    public static class Program
    {
        static void Main(string[] args)
        {
            WebClient Client = new WebClient();
            Client.Credentials = new NetworkCredential("administrator", "P@ssw0rd@321");

            string filename = "AZ_2017-02-24-17_00_05_8.xml";

            byte[] filedata = Client.DownloadData("http://10.68.128.101/fmdsstest/XMLFILES/sourcePath/" + filename);

            FileStream file = File.Create("C:\\PublishFMDSS\\XMLFILES\\targetPath\\" + filename);
            file.Write(filedata, 0, filedata.Length);
            file.Close();

            string xmlObject = System.IO.File.ReadAllText(@"C:\\PublishFMDSS\\XMLFILES\\targetPath\\" + filename);

            ShipmentInformation result;

            XmlSerializer serializer = new XmlSerializer(typeof(ShipmentInformation));
            using (TextReader reader = new StringReader(xmlObject))
            {
                result = (ShipmentInformation)serializer.Deserialize(reader);
            }


            DataTable DTShipmentPackageInfoItemEvent = new DataTable("ShipmentPackageInfoItemEvent");
            DTShipmentPackageInfoItemEvent = ShipmentPackageInfoItemEventDT(result);

            int STATUS = INSERTDATA(filename, result.ShipmentStatusHeader.ReceiverID,
                       result.ShipmentStatusHeader.SenderID,
                       result.ShipmentStatusHeader.TransmissionDate,
                       result.ShipmentStatusHeader.TransmissionTime,
                       DTShipmentPackageInfoItemEvent);
            if (STATUS == 0)
            {
                Console.Write("Already Exist");
            }
            else
            {
                Console.Write("Process complete For File :" + filename);
            }

        }

        public static int INSERTDATA(string filename, string ReceiverID, string SenderID, string TransmissionDate, string TransmissionTime, DataTable DTShipmentPackageInfoItemEvent)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = new SqlConnection("Data Source=VM-FMDSS2-STAG\\SQLEXPRESS; Initial Catalog=FMDSS_WR;Integrated Security=false;User ID=sa;Password=P@ssw0rd@321;");
            int STATUS = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT_ShipmentDetails", Conn);
                cmd.Parameters.AddWithValue("@filename", filename);
                cmd.Parameters.AddWithValue("@ReceiverID", ReceiverID);
                cmd.Parameters.AddWithValue("@SenderID", SenderID);
                cmd.Parameters.AddWithValue("@TransmissionDate", TransmissionDate);
                cmd.Parameters.AddWithValue("@TransmissionTime", TransmissionTime);
                cmd.Parameters.AddWithValue("@ShipmentPackageInfoItemEvent", DTShipmentPackageInfoItemEvent);
                cmd.CommandType = CommandType.StoredProcedure;
                Conn.Open();
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                STATUS = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Conn.Close();
            }
            return STATUS;
        }

        public static DataTable ShipmentPackageInfoItemEventDT(ShipmentInformation result)
        {

            DataTable DTItemEvent = new DataTable("ItemEvent");

            DTItemEvent.Columns.Add("ArticleNumber", typeof(string));
            DTItemEvent.Columns.Add("ReferenceNumber", typeof(string));

            DTItemEvent.Columns.Add("ActivityCode", typeof(string));
            DTItemEvent.Columns.Add("ActivityDateTime", typeof(string));
            DTItemEvent.Columns.Add("OfficeName", typeof(string));
            DTItemEvent.Columns.Add("ReasonCode", typeof(string));
            DTItemEvent.Columns.Add("ActivityCity", typeof(string));
            DTItemEvent.Columns.Add("ActivityState", typeof(string));
            DTItemEvent.Columns.Add("ItemEventSID", typeof(Int64));

            Int64 ID = 1;
            foreach (var i in result.ShipmentStatusDetail.ItemEvents)
            {
                DataRow dr = DTItemEvent.NewRow();

                dr["ArticleNumber"] = i.ShipmentPackageInfo.ArticleNumber;
                dr["ReferenceNumber"] = i.ShipmentPackageInfo.ReferenceNumber;

                dr["ActivityCode"] = i.ItemEvent.ActivityCode;
                dr["ActivityDateTime"] = i.ItemEvent.ActivityDateTime;
                dr["OfficeName"] = i.ItemEvent.OfficeName;
                dr["ReasonCode"] = i.ItemEvent.ReasonCode;
                dr["ActivityCity"] = i.ItemEvent.ActivityCity;
                dr["ActivityState"] = i.ItemEvent.ActivityState;
                dr["ItemEventSID"] = ID;
                ID = ID + 1;
                DTItemEvent.Rows.Add(dr);

            }
            return DTItemEvent;
        }


    }
}
