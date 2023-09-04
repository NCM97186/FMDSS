
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.Common;
using System.Web.UI.WebControls;
using FMDSS.Models;
using System.Data;
using System.Collections;
using System.Configuration;
using FMDSS.Models.Master;
using FMDSS.Models.MIS;
using FMDSS.Models.OnlineBooking;
using FMDSS.Filters;
using System.IO;
using System.Web.UI;
using System.Text;
using System.Net;
using System.Data.SqlClient;

namespace FMDSS.Models
{
    public class SMSandEMAILtemplate : DAL
    {


        public DataSet GetUserDetailsWildlife(String RequestID, string ACTION)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GETUSERDETAILSFORSENDSMSANDEMAIL", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataSet GetWaitingCNFUsersDetails(string ACTION)
        {
            //
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_WaitingTicketsConfirmationFunctions", Conn);
                cmd.Parameters.AddWithValue("@ActionName", ACTION);               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
       
        public DataSet UpdatetWaitingCNFMessageSentInfo(string @RequestId, string ACTION)
        {
            //
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_WaitingTicketsConfirmationFunctions", Conn);
                cmd.Parameters.AddWithValue("@ActionName", ACTION);
                cmd.Parameters.AddWithValue("@CanRequestId", @RequestId);
                cmd.Parameters.AddWithValue("@WaitingCNFRequestId", @RequestId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        public DataTable GetUserDetails(String RequestID, string ACTION)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GETUSERDETAILSFORSENDSMSANDEMAIL", Conn);
                cmd.Parameters.AddWithValue("@ACTION", ACTION);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataTable GetUserDetailsForZoo(String RequestID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("SP_GETUSERDETAILSFORSENDSMSANDEMAIL", Conn);
                cmd.Parameters.AddWithValue("@ACTION", "GETUSERDETAILSFORSENDSMSANDEMAILforZOO");
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public string WildLifeTicketEmailTemplate(String UserName, String PlaceName, String RequestID, string path)
        {
            try
            {
                string html = string.Empty;
                html = System.IO.File.ReadAllText(path);
                html = html.Replace("@UserName", UserName);
                html = html.Replace("@placeID", PlaceName);
                html = html.Replace("@RequestID", RequestID);
                html = html.Replace("<html>", "");
                html = html.Replace("<head>", "");
                html = html.Replace("<title>", "");
                html = html.Replace("</title>", "");
                html = html.Replace("</head>", "");
                html = html.Replace("<body>", "");
                html = html.Replace("</body>", "");
                html = html.Replace("</html>", "");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string WildLifeTicketEmailTemplate(String UserName, String PlaceName, String RequestID,String VisitDate, string path)
        {
            try
            {
                string html = string.Empty;
                html = System.IO.File.ReadAllText(path);
                html = html.Replace("@UserName", UserName);
                html = html.Replace("@placeID", PlaceName);
                html = html.Replace("@RequestID", RequestID);
                html = html.Replace("@VisitDate", VisitDate);
                html = html.Replace("<html>", "");
                html = html.Replace("<head>", "");
                html = html.Replace("<title>", "");
                html = html.Replace("</title>", "");
                html = html.Replace("</head>", "");
                html = html.Replace("<body>", "");
                html = html.Replace("</body>", "");
                html = html.Replace("</html>", "");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string WildLifeTicketSMSTemplate(String PlaceName, String RequestID, string path)
        {
            try
            {
                string html = string.Empty;

                html = System.IO.File.ReadAllText(path);
                html = html.Replace("@placeID", PlaceName);
                html = html.Replace("@RequestID", RequestID);
                html = html.Replace("<html>", "");
                html = html.Replace("<head>", "");
                html = html.Replace("<title>", "");
                html = html.Replace("</title>", "");
                html = html.Replace("</head>", "");
                html = html.Replace("<body>", "");
                html = html.Replace("</body>", "");
                html = html.Replace("</html>", "");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string WildLifeTicketSMSTemplate(String PlaceName, String RequestID,String VisitDate, string path)
        {
            try
            {
                string html = string.Empty;

                html = System.IO.File.ReadAllText(path);
                html = html.Replace("@placeID", PlaceName);
                html = html.Replace("@RequestID", RequestID);
                html = html.Replace("@VisitDate", VisitDate);
                html = html.Replace("<html>", "");
                html = html.Replace("<head>", "");
                html = html.Replace("<title>", "");
                html = html.Replace("</title>", "");
                html = html.Replace("</head>", "");
                html = html.Replace("<body>", "");
                html = html.Replace("</body>", "");
                html = html.Replace("</html>", "");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string ZooTicketEmailTemplate(String UserName, String PlaceName, String RequestID, string path)
        {
            try
            {
                string html = string.Empty;
                html = System.IO.File.ReadAllText(path);
                html = html.Replace("@UserName", UserName);
                html = html.Replace("@placeID", PlaceName);
                html = html.Replace("@RequestID", RequestID);
                html = html.Replace("<html>", "");
                html = html.Replace("<head>", "");
                html = html.Replace("<title>", "");
                html = html.Replace("</title>", "");
                html = html.Replace("</head>", "");
                html = html.Replace("<body>", "");
                html = html.Replace("</body>", "");
                html = html.Replace("</html>", "");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string ZooTicketSMSTemplate(String PlaceName, String RequestID, string path)
        {
            try
            {
                string html = string.Empty;

                html = System.IO.File.ReadAllText(path);
                html = html.Replace("@placeID", PlaceName);
                html = html.Replace("@RequestID", RequestID);
                html = html.Replace("<html>", "");
                html = html.Replace("<head>", "");
                html = html.Replace("<title>", "");
                html = html.Replace("</title>", "");
                html = html.Replace("</head>", "");
                html = html.Replace("<body>", "");
                html = html.Replace("</body>", "");
                html = html.Replace("</html>", "");
                return html;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string OrderPurchaesEmailSMSTemplate(DataTable dt, string ActionName, string path, ref StringBuilder ItemListWithComma)
        {
            string html = string.Empty;
            StringBuilder ItmsList = new StringBuilder();
            decimal total = 0;
            string NurssaryName = string.Empty;
            string UserName = string.Empty;
            int Qty = 0;
            string requestID = string.Empty;
            try
            {
                if (ActionName == "email")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ItmsList.Append("<Table style='border-collapse: collapse; width: 50%; border:solid'><tr><th style='background-color: #4CAF50;color: white ;text-align: left;padding: 8px;'>Product Name</th><th style='background-color: #4CAF50;color: white ;text-align: left;padding: 8px;'>QTY</th><th style='background-color: #4CAF50;color: white ; text-align: left;padding: 8px;'>Amount</th></tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            ItmsList.Append("<tr>");
                            ItmsList.Append("<td style='text-align: left;padding: 8px;'>" + Convert.ToString(dr["ProductName"].ToString()) + "</td>");
                            ItmsList.Append("<td style='text-align: left;padding: 8px;'>" + Convert.ToString(dr["PurchaseQuantity"].ToString()) + "</td>");
                            ItmsList.Append("<td style='text-align: left;padding: 8px;'>" + Convert.ToString(dr["PaidAMount"].ToString()) + "</td>");
                            ItmsList.Append("</tr>");
                            total += Convert.ToDecimal(dr["PaidAMount"].ToString());
                            ItemListWithComma.Append(Convert.ToString(dr["ProductName"].ToString()) + ",");
                        }
                        ItmsList.Append("<tr>");
                        ItmsList.Append("<td style='text-align: left;padding: 8px;'></td>");
                        ItmsList.Append("<td style='text-align: left;padding: 8px;'><b>Total</b></td>");
                        ItmsList.Append("<td style='text-align: left;padding: 8px;'> <b>" + total + " </b></td>");
                        ItmsList.Append("</tr>");
                        ItmsList.Append("</Table>");
                    }


                    html = System.IO.File.ReadAllText(path);
                    html = html.Replace("@NurseryAddress", Convert.ToString(dt.Rows[0]["NURSERY_NAME"]));
                    html = html.Replace("@UserName", Convert.ToString(dt.Rows[0]["Name"]));
                    html = html.Replace("@ItemDetails", ItmsList.ToString());
                    html = html.Replace("@requestID", Convert.ToString(dt.Rows[0]["RequestedId"]));
                    html = html.Replace("@OrderDate", Convert.ToString(dt.Rows[0]["OrderDate"]));
                }

                else if (ActionName == "sms")
                {
                    html = System.IO.File.ReadAllText(path);
                    if (!string.IsNullOrEmpty(ItemListWithComma.ToString()))
                    {
                        html = html.Replace("@ItemDetails", ItemListWithComma.ToString());
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ItemListWithComma.Append(Convert.ToString(dr["ProductName"].ToString()) + ",");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return html;
        }


        public string AmritaDeviMailSMSTemplate(string MailSMS, string requestid, string Name, string IsApproveAndRejectStatus, string path)
        {
            string html = string.Empty;
            try
            {
                if (MailSMS == "Mail")
                {
                    html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    html = html.Replace("@RequestID", requestid);
                    html = html.Replace("@Name", Name);
                    html = html.Replace("@ApprovedStatus", IsApproveAndRejectStatus);
                }
                else if (MailSMS == "SMS")
                {
                    html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    html = html.Replace("@RequestID", requestid);
                    html = html.Replace("@Name", Name);
                    html = html.Replace("@ApprovedStatus", IsApproveAndRejectStatus);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return html;
        }

        public string ReconciliationMail(string MailSMS, string requestid, string Name, string Comment, string path)
        {
            string html = string.Empty;
            try
            {
                if (MailSMS == "Mail")
                {
                    html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    html = html.Replace("@RequestID", requestid);
                    html = html.Replace("@Name", Name);
                    html = html.Replace("@Comment", Comment);
                }
                else if (MailSMS == "SMS")
                {
                    html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    html = html.Replace("@RequestID", requestid);
                    html = html.Replace("@Name", Name);
                    html = html.Replace("@Comment", Comment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return html;
        }

        public void SendMailComman(string MailSMS, string ModuleName, string requestid, string name, string ClientEmail, string MobileNo, string isapproveandaejectatatus, string othercolumn = null, params string[] extracolumn)
        {
            SMSandEMAILtemplate objSMSandEMAILtemplate = new SMSandEMAILtemplate();
            SMS_EMail_Services objSMS_EMail_Services = new SMS_EMail_Services();
            string AdminTemplateEmail = string.Empty;
            string CitizenTemplateEmail = string.Empty;
            string AdminTemplateSMS = string.Empty;
            string CitizenTemplateSMS = string.Empty;
            string AdminEmail = string.Empty;
            string AdminMobileNumber = string.Empty;

            try
            {
                #region Get Email Format
                DataSet ds = GetEmailSMSFormate(ModuleName);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    AdminTemplateEmail = Convert.ToString(ds.Tables[0].Rows[0]["AdminTemplate"]);
                    CitizenTemplateEmail = Convert.ToString(ds.Tables[0].Rows[0]["CitizenTemplate"]);
                    AdminTemplateSMS = Convert.ToString(ds.Tables[0].Rows[0]["AdminTemplateSMS"]);
                    CitizenTemplateSMS = Convert.ToString(ds.Tables[0].Rows[0]["CitizenTemplateSMS"]);
                    AdminEmail = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]);
                    AdminMobileNumber = Convert.ToString(ds.Tables[0].Rows[0]["AdminMobileNumber"]);
                }

                #endregion


                if (MailSMS.Trim().ToUpper() == "ALL" || MailSMS.Trim().ToUpper() == "Mail")
                {
                    try
                    {
                        #region Admin
                        if (!string.IsNullOrEmpty(AdminTemplateEmail))
                        {
                            AdminTemplateEmail = AdminTemplateEmail.Replace("@requestid", requestid);
                            AdminTemplateEmail = AdminTemplateEmail.Replace("@name", name);
                            AdminTemplateEmail = AdminTemplateEmail.Replace("@mobile", MobileNo);
                            AdminTemplateEmail = AdminTemplateEmail.Replace("@approvedstatus", isapproveandaejectatatus);
                            AdminTemplateEmail = AdminTemplateEmail.Replace("@othercolumn", othercolumn);
                            //AdminTemplateEmail = AdminTemplateEmail.Replace("@animalcategeory", animalcategeory);
                            int i = 0;
                            foreach (var item in extracolumn)
                            {
                                AdminTemplateEmail = AdminTemplateEmail.Replace("@extracolumn[" + i + "]", item);
                                i++;
                            }

                            objSMS_EMail_Services.sendEMail(ModuleName, AdminTemplateEmail, AdminEmail, string.Empty);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.Message, "Error occured in Admin EMAIL Time and Module name is:-" + ModuleName, 0, DateTime.Now, 0);
                    }
                    try
                    {
                        #region Citizen
                        if (!string.IsNullOrEmpty(CitizenTemplateEmail) && !string.IsNullOrEmpty(ClientEmail))
                        {
                            CitizenTemplateEmail = CitizenTemplateEmail.Replace("@requestid", requestid);
                            CitizenTemplateEmail = CitizenTemplateEmail.Replace("@name", name);
                            CitizenTemplateEmail = CitizenTemplateEmail.Replace("@approvedstatus", isapproveandaejectatatus);
                            CitizenTemplateEmail = CitizenTemplateEmail.Replace("@othercolumn", othercolumn);

                            foreach (var item in extracolumn)
                            {
                                int i = 0;
                                CitizenTemplateEmail = CitizenTemplateEmail.Replace("@extracolumn[" + i + "]", item);
                                i++;
                            }
                            objSMS_EMail_Services.sendEMail(ModuleName, CitizenTemplateEmail, ClientEmail, string.Empty);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.Message, "Error occured in Citizen EMAIL Time and Module name is:-" + ModuleName, 0, DateTime.Now, 0);
                    }
                }
                if (MailSMS.Trim().ToUpper() == "ALL" || MailSMS.Trim().ToUpper() == "SMS")
                {
                    try
                    {
                        #region Admin
                        if (!string.IsNullOrEmpty(AdminTemplateSMS) && !string.IsNullOrEmpty(AdminMobileNumber))
                        {
                            AdminTemplateSMS = AdminTemplateSMS.Replace("@requestid", requestid);
                            AdminTemplateSMS = AdminTemplateSMS.Replace("@name", name);
                            AdminTemplateSMS = AdminTemplateSMS.Replace("@approvedstatus", isapproveandaejectatatus);
                            AdminTemplateSMS = AdminTemplateSMS.Replace("@othercolumn", othercolumn);
                            foreach (var item in extracolumn)
                            {
                                int i = 0;
                                AdminTemplateSMS = AdminTemplateSMS.Replace("@extracolumn[" + i + "]", item);
                                i++;
                            }
                            string[] AdminMobileNumbers = AdminMobileNumber.Split(',');
                            foreach (string number in AdminMobileNumbers)
                            {
                                SMS_EMail_Services.sendSingleSMS(number.Trim(), AdminTemplateSMS);
                            }
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.Message, "Error occured in Admin Mobile Time and Module name is:-" + ModuleName, 0, DateTime.Now, 0);
                    }
                    try
                    {
                        #region Citizen
                        if (!string.IsNullOrEmpty(CitizenTemplateSMS) && !string.IsNullOrEmpty(MobileNo))
                        {
                            CitizenTemplateSMS = CitizenTemplateSMS.Replace("@requestid", requestid);
                            CitizenTemplateSMS = CitizenTemplateSMS.Replace("@name", name);
                            CitizenTemplateSMS = CitizenTemplateSMS.Replace("@approvedstatus", isapproveandaejectatatus);
                            CitizenTemplateSMS = CitizenTemplateSMS.Replace("@othercolumn", othercolumn);
                            foreach (var item in extracolumn)
                            {
                                CitizenTemplateSMS = CitizenTemplateSMS.Replace("@extracolumn[" + 0 + "]", item);
                            }
                            SMS_EMail_Services.sendSingleSMS(MobileNo, CitizenTemplateSMS);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        new Common().ErrorLog(ex.Message, "Error occured in Citizen Mobile Time and Module name is :-" + ModuleName, 0, DateTime.Now, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetEmailSMSFormate(string ModuleName)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_GetALLEmailModules", Conn);
                cmd.Parameters.AddWithValue("@modulename", ModuleName);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public string TransitPermitInspectionSmsMail(string MailSMS, string requestid, string Name, string Comment, string path)
        {
            string html = string.Empty;
            try
            {
                if (MailSMS == "Mail")
                {
                    html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    html = html.Replace("@RequestID", requestid);
                    html = html.Replace("@Name", Name);
                    html = html.Replace("@InspectionDateTime", Comment);
                }
                else if (MailSMS == "SMS")
                {
                    html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    html = html.Replace("@RequestID", requestid);
                    html = html.Replace("@Name", Name);
                    html = html.Replace("@InspectionDateTime", Comment);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return html;
        }

    }
}