using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data;
namespace FMDSS.Models
{


    public class RTI_Details
    {
        public string Application_ID { get; set; }
        public string Submission_Date { get; set; }
        public string Applicant_Name_En { get; set; }
        public string Department_Name_En { get; set; }
        public string Office_Name_En { get; set; }
        public string Total_Fees { get; set; }
        public string Status { get; set; }
    }
    public class RTI_Service
    {


        public DataTable Get_All_RTI()
        {
            RTI_APP_Services.RTISoapClient obj=new RTI_APP_Services.RTISoapClient();
            string SSOID = System.Web.HttpContext.Current.Session["SSOID"].ToString();
          // string SSOID = "ASHA.SHARMA";
            string json = obj.GetApplicationAllTransactionBySSOIDJSON(SSOID);
            
            if (json != null)
            {
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                return dt;
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Clear();
                dt.AcceptChanges();
                return dt;
            }
            //ui_grdVw_EmployeeDetail2.DataSource = dt;
            //ui_grdVw_EmployeeDetail2.DataBind();
        }

    }
}