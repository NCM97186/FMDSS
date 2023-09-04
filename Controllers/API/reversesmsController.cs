using FMDSS.CustomModels.Models;
using FMDSS.Models;
using FMDSS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class reversesmsController : ApiController
    {
        //
        // GET: /reversesms/
        [HttpPost]
        public DataTableResponse RTR_OB(string mobilenumber, string messagetext) 
        {
            DataTableResponse response = new DataTableResponse();
            try
            {
                AddRuleDetails oObj = new AddRuleDetails();
                DataTable dt = oObj.CheckApproverValidate(mobilenumber);

                if (dt.Rows.Count > 0)
                {
                    string ApproverUserId = Convert.ToString(dt.Rows[0]["ApproverUserId"]);
                    DataTable oDt = oObj.UpdateHolidayDetails(mobilenumber, (messagetext.ToLower().Contains("yes") ? "1" : "0"), ApproverUserId, messagetext);
                    response = new DataTableResponse { Status = ResponseStatus.Success, Message = MessageText.Success.ToString(), Data = oDt };
                    cls_HolidayDetails oHoliday = new cls_HolidayDetails();
                    oHoliday.SendHolidayEmailSMS((int)SMSTEmplate.HolidaySeat_response);
                }
                else ////unauthorized user
                {
                    DataTable oDt = oObj.UPdateunauthorizedLog(mobilenumber, messagetext);
                }
            }
            catch (Exception ex)
            {
                response = new DataTableResponse { Status = ResponseStatus.Failed, Message = MessageText.Fail.ToString(), ErrorDescription = ex.Message };
            }
            return response;
        }

        private DataTable GetUserbyMobileNumber(string mobilenumber)
        {
            AddRuleDetails oObj = new AddRuleDetails();
            DataTable odt= oObj.GetUserByMobileNumber(mobilenumber);
            return odt;

        }
    }

    enum MessageText
    {
        Success=1,
        Fail=0
    
    }
}
