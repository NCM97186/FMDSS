using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS
{
    public partial class SendDataToEmitra : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if((Convert.ToString(Request.QueryString["IsMobileApp"])!="") && (Convert.ToString(Request.QueryString["Ssoid"])!="") && (Convert.ToString(Request.QueryString["RequestId"])!=""))
                Pay(true, Convert.ToString(Request.QueryString["Ssoid"]), Convert.ToString(Request.QueryString["RequestId"]), "http://fmdss.forest.rajasthan.gov.in/ZooAppEmitraResponce.aspx", "http://fmdss.forest.rajasthan.gov.in/ZooAppEmitraResponce.aspx");
            else
            {
                Response.Write("Please Pass all parameter!");
                Response.End();
            }
        }


        public string Pay(bool IsMobileApp, string Ssoid, string RequestId, string SUCCESSEmitraReturnURL, string FAILEDEmitraReturnURL)
        {
            string actionName = "Pay";
            string controllerName = "FMDSSJsonController";
            string EmitraPaymentGatwayString = "";

            try
            {
                // get different heads amount from DB
                
                BookOnTicket OBJ = new BookOnTicket();
                DataSet DS = new DataSet();
                DS = OBJ.Get_HeadWiseAmountOfWildLifeTickets("ZooTickets", RequestId);
                // 0 Head for ecodevelopment surcharge - 0406-02-800-01
                // 1 Head for entry fees- 0406-01-800-05
                // 2 Grand Total
                // 3 Office

                string REVENUEHEAD = Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEntryFeeCode"]) + "|" + Convert.ToString(DS.Tables[1].Rows[0]["RevenueHeadEcoDevelopmentSurchargeCode"]);

                EmitraPayRequest ObjEmitraPayRequest = new EmitraPayRequest();


                EmitraPaymentGatwayString = ObjEmitraPayRequest.PayRequest(true, RequestId,
                Convert.ToString(DS.Tables[1].Rows[0]["MerchantCode"]),
                Convert.ToString(DS.Tables[1].Rows[0]["ChecksumKey"]),
                Convert.ToString(DS.Tables[1].Rows[0]["EncryptionKey"]),
                 SUCCESSEmitraReturnURL, FAILEDEmitraReturnURL,
                Convert.ToString(DS.Tables[0].Rows[1]["HeadAmount"]), Convert.ToString(DS.Tables[1].Rows[0]["ServiceID"]),
                Convert.ToString(DS.Tables[0].Rows[0]["HeadAmount"]), REVENUEHEAD, Ssoid);




                 Response.Redirect(EmitraPaymentGatwayString,false);
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 2, DateTime.Now, 0);
            }
            return EmitraPaymentGatwayString;

        }
    }
}