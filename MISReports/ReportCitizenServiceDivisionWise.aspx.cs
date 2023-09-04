using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMDSS.Models;

namespace FMDSS
{
    public partial class ReportCitizenServiceDivisionWise : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCircle();

                BindPermissionStatus();

                // Button_submit_Click(null, null);
            }
        }

        private void BindPermissionStatus()
        {
            CitizenModel Model = new CitizenModel();
            DataTable dt = Model.GetPermissionStatus();
            DDL_PermissionStatus.DataTextField = "StatusDesc";
            DDL_PermissionStatus.DataValueField = "StatusID";
            DDL_PermissionStatus.DataSource = dt;
            DDL_PermissionStatus.DataBind();

            ListItem lst = new ListItem("--Select--", "0");
            DDL_PermissionStatus.Items.Insert(0, lst);
           

        }
        private void BindCircle()
        {
            CitizenModel Model = new CitizenModel();
            DataTable dt = Model.GetCircle();
            DDL_CIRCLE_CODE.DataTextField = "CIRCLE_NAME";
            DDL_CIRCLE_CODE.DataValueField = "Circle_Code";
            DDL_CIRCLE_CODE.DataSource = dt;
            DDL_CIRCLE_CODE.DataBind();

            ListItem lst = new ListItem("--Select--", "0");
            ListItem lst1 = new ListItem("ALL", "ALL");

            DDL_CIRCLE_CODE.Items.Insert(0, lst);
            DDL_CIRCLE_CODE.Items.Insert(1, lst1);
        }

        public void DDL_CIRCLE_CODE_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DDL_DIVISON_CODE.Items.Clear();
            CitizenModel Model = new CitizenModel();

            if (DDL_CIRCLE_CODE.SelectedIndex == 1)
            {
                ListItem lst = new ListItem("--Select--", "0");
                ListItem lst1 = new ListItem("ALL", "ALL");

                DDL_DIVISON_CODE.Items.Insert(0, lst);
                DDL_DIVISON_CODE.Items.Insert(1, lst1);
            }
            else
            {


                DataTable dt = Model.GetDivision(Convert.ToString(DDL_CIRCLE_CODE.SelectedValue));


                DDL_DIVISON_CODE.DataTextField = "DIV_NAME";
                DDL_DIVISON_CODE.DataValueField = "DIV_CODE";
                DDL_DIVISON_CODE.DataSource = dt;
                DDL_DIVISON_CODE.DataBind();

                ListItem lst = new ListItem("--Select--", "0");
                ListItem lst1 = new ListItem("ALL", "ALL");

                DDL_DIVISON_CODE.Items.Insert(0, lst);
                DDL_DIVISON_CODE.Items.Insert(1, lst1);
            }

        }

        public void DDL_DIVISON_CODE_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            CitizenModel Model = new CitizenModel();


            DDL_RANGE.Items.Clear();

            if (DDL_DIVISON_CODE.SelectedIndex == 1)
            {
                ListItem lst = new ListItem("--Select--", "0");
                ListItem lst1 = new ListItem("ALL", "ALL");
                DDL_RANGE.Items.Insert(0, lst);
                DDL_RANGE.Items.Insert(1, lst1);
            }
            else
            {
                DataTable dt = Model.GetRange(Convert.ToString(DDL_DIVISON_CODE.SelectedValue));

                DDL_RANGE.DataTextField = "RANGE_NAME";
                DDL_RANGE.DataValueField = "RANGE_CODE";
                DDL_RANGE.DataSource = dt;
                DDL_RANGE.DataBind();

                ListItem lst = new ListItem("--Select--", "0");
                ListItem lst1 = new ListItem("ALL", "ALL");

                DDL_RANGE.Items.Insert(0, lst);
                DDL_RANGE.Items.Insert(1, lst1);
            }
        }

      
        protected void Button_GetReport_Click(object sender, EventArgs e)
        {
            rptViewer.Visible = false;
            Label_error.Text = "";
            if (DDL_Duration.SelectedIndex == 0)
            {
                Label_error.Text = "Duration is Required";
                return;
            }
            if (DDL_CIRCLE_CODE.SelectedIndex == 0)
            {
                Label_error.Text = "Circle is Required";
                return;
            }
            if (DDL_DIVISON_CODE.SelectedIndex == 0)
            {
                Label_error.Text = "Division is Required";
                return;
            }
            if (DDL_RANGE.SelectedIndex == 0)
            {
                Label_error.Text = "Range is Required";
                return;
            }
            if (DDL_PermissionStatus.SelectedIndex == 0)
            {
                Label_error.Text = "Permission Status is Required";
                return;
            }

            rptViewer.Visible = true;





            string urlReportServer = "http://win-250o0fi11gj/ReportServer_FMDSSSERVER";
            rptViewer.ProcessingMode = ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
            rptViewer.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
            rptViewer.ServerReport.ReportPath = "/FMDSS_MISREPORTS/ReportCitizenServiceDivisionWise"; //Passing the Report Path                

            //Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
            ArrayList reportParam = new ArrayList();
            reportParam = ReportDefaultPatam();

            ReportParameter[] param = new ReportParameter[reportParam.Count];
            for (int k = 0; k < reportParam.Count; k++)
            {
                param[k] = (ReportParameter)reportParam[k];
            }
            // pass crendentitilas
            //rptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials("uName", "PassWORD", "doMain");

            //pass parmeters to report
            rptViewer.ServerReport.SetParameters(param); //Set Report Parameters
            rptViewer.ServerReport.Refresh();
        }

        private ArrayList ReportDefaultPatam()
        {
            
            ArrayList arrLstDefaultParam = new ArrayList();
            arrLstDefaultParam.Add(CreateReportParameter("DURATION", DDL_Duration.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("DATETIME_FROM",  TextBox_from.Text)); //DateTime.Now.Date.ToString("dd/MMM/yyyy").Substring(0, 11)));
            arrLstDefaultParam.Add(CreateReportParameter("DATETIME_TO", TextBox_to.Text));
            arrLstDefaultParam.Add(CreateReportParameter("CIRCLE_CODE",  DDL_CIRCLE_CODE.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("RANGE_CODE", DDL_RANGE.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("DIVISON_CODE", DDL_DIVISON_CODE.SelectedValue));

            arrLstDefaultParam.Add(CreateReportParameter("DIVISION_NAME", DDL_DIVISON_CODE.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("CIRCLE_NAME", DDL_CIRCLE_CODE.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("RANGE_NAME", DDL_RANGE.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("ServicesTATUS", DDL_PermissionStatus.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("SERVICE_STATUS_NAME", DDL_PermissionStatus.SelectedItem.Text));

            return arrLstDefaultParam;
        }
        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }
    }
}



