using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using FMDSS.Models;
namespace FMDSS.Controllers.Services
{
    public class RTIController : BaseController
    {
        //
        // GET: /RTI/

        public ActionResult My_RTI()
        {
            RTI_Service obj=new RTI_Service();
            DataTable DT = new DataTable();

            DT = obj.Get_All_RTI();
            if (DT.Rows.Count > 0)
            {
                DT.Columns.Remove("Citizen_Id");
                DT.Columns.Remove("Total_Application");
                DT.Columns.Remove("Information_Given");
                DT.Columns.Remove("Rejected_Application");
                DT.Columns.Remove("InProcess_Application");
                DT.Columns.Remove("From_Date");
                DT.Columns.Remove("Today_Date");
                DT.Columns.Remove("ActionType");
                DT.Columns.Remove("Total_Processed");
                DT.Columns.Remove("Applicant_Name_Hi");
                DT.Columns.Remove("Life_Liberty");
                DT.Columns.Remove("Department_Name_Hi");
                DT.Columns.Remove("Department_ShortName_En");
                DT.Columns.Remove("Department_ShortName_Hi");
                DT.Columns.Remove("Office_Name_Hi");

                DT.Columns.Remove("Office_Address_En");
                DT.Columns.Remove("Office_Address_Hi");
                DT.Columns.Remove("PIO_MobileNo");
                DT.Columns.Remove("PIO_PhoneNo");
                DT.Columns.Remove("Login_Officer_Name_Hi");



                DT.Columns.Remove("App_Received_By_En");
                DT.Columns.Remove("App_Received_By_Hi");
                DT.Columns.Remove("Info_Subject");
                DT.Columns.Remove("Info_Details");
                DT.Columns.Remove("Info_From_Date");
                DT.Columns.Remove("Info_To_Date");
                DT.Columns.Remove("Status_Hi");
                DT.Columns.Remove("Fee_Pages");
                DT.Columns.Remove("Fee_Publication");
                DT.Columns.Remove("Fee_Model_Sample");
                DT.Columns.Remove("Total_Fees");
                DT.Columns.Remove("Larger_Paper_Size_Cost");
                DT.Columns.Remove("Fee_Intemation_Other_Details");
                DT.Columns.Remove("Office_Type");



                DT.Columns.Remove("Officer_Id");
                DT.Columns.Remove("Inward_Outward_No");
                DT.Columns.Remove("Application_Form_Fee");
                DT.Columns.Remove("Total_Requested_Fee");
                DT.Columns.Remove("Received_Fee");
                DT.Columns.Remove("Total_Fee");

                DT.Columns.Remove("Received_Reply_Date");



                DT.Columns.Remove("IsBPL");
                DT.Columns.Remove("InformationGiven_FileName");
                DT.Columns.Remove("Reason_For_MoreInfo");
                DT.Columns.Remove("IsApplicationInMI");
                DT.Columns.Remove("IsApplicationInFI");
                DT.Columns.Remove("IsApplicationInINFoGiven");
                DT.Columns.Remove("IsApp_En");
                DT.Columns.Remove("IsCanTakeOtherFileUploadActino");


                DT.Columns.Remove("OutwardActionType");
                DT.Columns.Remove("Office_Id");
                DT.Columns.Remove("Fee_Disks");
                DT.Columns.Remove("District_Name_En");
                DT.Columns.Remove("District_Name_Hi");

                DT.Columns.Remove("Request_Date");
                DT.Columns.Remove("Mode_Of_Information");
                DT.Columns.Remove("Login_Officer_Name_En");
                DT.Columns["Application_No"].ColumnName = "Application No";
                DT.Columns["Submission_Date"].ColumnName = "Submission Date";
                DT.Columns["Applicant_Name_En"].ColumnName = "Applicant Name";
                DT.Columns["Department_Name_En"].ColumnName = "Department";
                DT.Columns["Office_Name_En"].ColumnName = "Office";
                DT.Columns["Status"].ColumnName = "Status";
                DT.Columns["InwardActionType"].ColumnName = "Action";
                DT.AcceptChanges();
            }
      
            
            return View(DT);
        }

        //
        // GET: /RTI/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RTI/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RTI/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RTI/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RTI/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /RTI/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RTI/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
