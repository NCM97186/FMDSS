//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : MIS Reports Code file
//  Description  : File contains calling functions RDS from this application
//  Date Created : 15-Mar-2015
//  History      : 
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using Microsoft.Reporting.WebForms;
using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using FMDSS.Models;

namespace FMDSS.MIS_Reports
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCircle();

                DDL_ServiceCategory.Items.Add(new ListItem("SELECT", "0"));
                DDL_ServiceCategory.Items.Add(new ListItem("ALL", "ALL"));
                DDL_ServiceCategory.Items.Add(new ListItem("FIXED", "FIXED"));
                DDL_ServiceCategory.Items.Add(new ListItem("EDUCATION", "EDUCATION"));
                DDL_ServiceCategory.Items.Add(new ListItem("MISC", "MISC"));
                DDL_ServiceCategory.Items.Add(new ListItem("ONLINEBOOKING", "ONLINEBOOKING"));


                // Button_submit_Click(null, null);
            }
            // ShowReport();
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

        protected void DDL_ServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            DDL_ServiceName.Enabled = true;
            DDL_ServiceName.Items.Clear();
            if (DDL_ServiceCategory.SelectedItem.Text == "ALL")
            {
                DDL_ServiceName.Enabled = false;
            }
            else if (DDL_ServiceCategory.SelectedItem.Text == "FIXED")
            {
                DDL_ServiceName.Items.Add(new ListItem("SELECT", "0"));
                DDL_ServiceName.Items.Add(new ListItem("ALL", "ALL"));
                DDL_ServiceName.Items.Add(new ListItem("Cable Lines", "Cable Lines"));
                DDL_ServiceName.Items.Add(new ListItem("Electricity lines", "Electricity lines"));
                DDL_ServiceName.Items.Add(new ListItem("Industry Set-Up", "Industry Set-Up"));
                DDL_ServiceName.Items.Add(new ListItem("Mining Permission", "Mining Permission"));
                DDL_ServiceName.Items.Add(new ListItem("Hospital", "Hospital"));
                DDL_ServiceName.Items.Add(new ListItem("Power Plant", "Power Plant"));
                DDL_ServiceName.Items.Add(new ListItem("School Permission", "School Permission"));
                DDL_ServiceName.Items.Add(new ListItem("Road/Highway", "Road/Highway"));
                DDL_ServiceName.Items.Add(new ListItem("Sawmill Permission", "Sawmill Permission"));
                DDL_ServiceName.Items.Add(new ListItem("Telephone Lines", "Telephone Lines"));
                DDL_ServiceName.Items.Add(new ListItem("Other Permission", "Other Permission"));
                DDL_ServiceName.Items.Add(new ListItem("Apply For Forest Rights", "Apply For Forest Rights"));
            }
            else if (DDL_ServiceCategory.SelectedItem.Text == "EDUCATION")
            {

                DDL_ServiceName.Items.Add(new ListItem("ALL", "ALL"));
            }
            else if (DDL_ServiceCategory.SelectedItem.Text == "MISC")
            {
                DDL_ServiceName.Items.Add(new ListItem("SELECT", "0"));
                DDL_ServiceName.Items.Add(new ListItem("ALL", "ALL"));
                DDL_ServiceName.Items.Add(new ListItem("CAMP", "CAMP"));
                DDL_ServiceName.Items.Add(new ListItem("SHOOTING", "SHOOTING"));
            }
            else if (DDL_ServiceCategory.SelectedItem.Text == "ONLINEBOOKING")
            {

                DDL_ServiceName.Items.Add(new ListItem("ALL", "ALL"));
            }

        }

        protected void Button_submit_Click(object sender, EventArgs e)
        {
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
            if (DDL_ServiceCategory.SelectedIndex == 0)
            {
                Label_error.Text = "Service Category is Required";
                return;
            }
          



            rdView.Visible = true;



            string urlReportServer = "http://win-250o0fi11gj/ReportServer_FMDSSSERVER";
            rdView.ProcessingMode = ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
            rdView.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
            rdView.ServerReport.ReportPath = "/FMDSS_MISREPORTS/MIS_ReportCitizenServiceGroupWiseDrillDown"; //Passing the Report Path                

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
            rdView.ServerReport.SetParameters(param); //Set Report Parameters
            rdView.ServerReport.Refresh();








        }
        private ArrayList ReportDefaultPatam()
        {

            ArrayList arrLstDefaultParam = new ArrayList();
            arrLstDefaultParam.Add(CreateReportParameter("DURATION", DDL_Duration.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("DATETIME_FROM", TextBox_from.Text)); //DateTime.Now.Date.ToString("dd/MMM/yyyy").Substring(0, 11)));
            arrLstDefaultParam.Add(CreateReportParameter("DATETIME_TO", TextBox_to.Text));
            arrLstDefaultParam.Add(CreateReportParameter("CIRCLE_CODE", DDL_CIRCLE_CODE.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("RANGE_CODE", DDL_RANGE.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("DIVISON_CODE", DDL_DIVISON_CODE.SelectedValue));
            arrLstDefaultParam.Add(CreateReportParameter("ServiceCategory", DDL_ServiceCategory.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("ServiceName", DDL_ServiceName.SelectedItem.Text));


            arrLstDefaultParam.Add(CreateReportParameter("DIVISION_NAME", DDL_DIVISON_CODE.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("CIRCLE_NAME", DDL_CIRCLE_CODE.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("RANGE_NAME", DDL_RANGE.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("ServiceCategoryName", DDL_ServiceCategory.SelectedItem.Text));
            arrLstDefaultParam.Add(CreateReportParameter("ServiceNameView", DDL_ServiceName.SelectedItem.Text));

            return arrLstDefaultParam;
        }
        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }
    }
}