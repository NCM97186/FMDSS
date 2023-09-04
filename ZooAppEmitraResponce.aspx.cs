using FMDSS.Models;
using FMDSS.Models.BookOnlineTicket;
using FMDSS.Models.BookOnlineZoo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS
{
    public partial class ZooAppEmitraResponce : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

          PaymentStatus objResponce=  GetPayment();

            if(objResponce.TRANSACTION_STATUS=="SUCCESS")
            {
                Response.Redirect("MobileSuccess.html?TRANSACTION_STATUS=" + objResponce.TRANSACTION_STATUS + "&EMITRA_AMOUNT=" + objResponce.EMITRA_AMOUNT + "&EMITRA_TRANSACTION_ID=" + objResponce.EMITRA_TRANSACTION_ID + "&REQUEST_ID=" + objResponce.REQUEST_ID);
            }
            else
            {
                Response.Redirect("MobileFail.html?TRANSACTION_STATUS=" + objResponce.TRANSACTION_STATUS + "&EMITRA_AMOUNT=" + objResponce.EMITRA_AMOUNT + "&EMITRA_TRANSACTION_ID=" + objResponce.EMITRA_TRANSACTION_ID + "&REQUEST_ID=" + objResponce.REQUEST_ID);
            }

        }

        public PaymentStatus GetPayment() // return emitra code 
        {
            FMDSS.Controllers.FMDSSJsonController obj = new Controllers.FMDSSJsonController();

            string MERCHANTCODE = ""; string PRN = ""; string STATUS = ""; string ENCDATA = "";

            if (Request.Form["MERCHANTCODE"] != null)
                MERCHANTCODE = Request.Form["MERCHANTCODE"].ToString();
            if (Request.Form["PRN"] != null)
                PRN = Request.Form["PRN"].ToString();
            if (Request.Form["STATUS"] != null)
                STATUS = Request.Form["STATUS"].ToString();
            if (Request.Form["ENCDATA"] != null)
                ENCDATA = Request.Form["ENCDATA"].ToString();
            BookOnTicket OBJ = new BookOnTicket();
            DataTable DT = OBJ.Get_HeadWiseAmountOfWildLifeTickets("ZooTickets", PRN).Tables[0];

            PaymentStatus objPaymentStatus = new PaymentStatus();

            if (DT.Rows.Count > 1)
            {

                objPaymentStatus= obj.PaymentUpdate(MERCHANTCODE, PRN, STATUS, ENCDATA, PRN, Convert.ToString(DT.Rows[0]["HeadAmount"]), "0");
            }
            return objPaymentStatus;
        }
    }
}