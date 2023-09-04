//*********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS)
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UserApproval
//  Description  : File contains calling functions from System Admin Controller
//  Date Created : 24-Dec-2015
//  History      :
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  :
//  Modified On  :
//  Reviewed By  :
//  Reviewed On  :
//*********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.Admin
{
    public class UserApproval : DAL
    {
        #region Data Members

        private string eMPNAME;
        public string EMPNAME
        {
            get { return eMPNAME; }
            set { eMPNAME = value; }
        }

        private string eMPSSOId;
        public string EMPSSOID
        {
            get { return eMPSSOId; }
            set { eMPSSOId = value; }
        }
        private string eMPDESIGNATION;
        public string EMPDESIGNATION
        {
            get { return eMPDESIGNATION; }
            set { eMPDESIGNATION = value; }
        }

        private string moduleId;
        public string ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private string serviceTypeId;
        public string ServiceTypeId
        {
            get { return serviceTypeId; }
            set { serviceTypeId = value; }
        }

        private string permissionId;
        public string PermissionId
        {
            get { return permissionId; }
            set { permissionId = value; }
        }

        private string subPermissionId;
        public string SubPermissionId
        {
            get { return subPermissionId; }
            set { subPermissionId = value; }
        }

        private string userId;
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string roleId;
        public string RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        private string designation;
        public string Designation
        {
            get { return designation; }
            set { designation = value; }
        }

        private string office;
        public string Office
        {
            get { return office; }
            set { office = value; }
        }

        private string department;
        public string Department
        {
            get { return department; }
            set { department = value; }
        }

        private string jurisdiction;
        public string Jurisdiction
        {
            get { return jurisdiction; }
            set { jurisdiction = value; }
        }

        private string pecuniaryLimit;
        public string PecuniaryLimit
        {
            get { return pecuniaryLimit; }
            set { pecuniaryLimit = value; }
        }

        private bool isReviewer;
        public bool IsReviewer
        {
            get { return isReviewer; }
            set { isReviewer = value; }
        }

        private bool isApprover;
        public bool IsApprover
        {
            get { return isApprover; }
            set { isApprover = value; }
        }

        private string updatedBy;
        public string UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }

        #endregion

        #region Member Functions

        /// <summary>
        /// Method is responsible for updating Reviewers/Approvers for specific permission
        /// </summary>
        /// <param name="uA"></param>
        /// <returns></returns>
        public Int64 SaveUserApproval(UserApproval uA)
        {
            Int64 chk = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Edit_Add_PermissionsApprovers", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModuleId", uA.ModuleId);
                cmd.Parameters.AddWithValue("@ServiceTypeId", uA.ServiceTypeId);
                cmd.Parameters.AddWithValue("@PermissionId", uA.PermissionId);
                cmd.Parameters.AddWithValue("@SubPermissionId", uA.SubPermissionId);
                cmd.Parameters.AddWithValue("@UserId", uA.UserId);
                cmd.Parameters.AddWithValue("@RoleId", uA.RoleId);
                cmd.Parameters.AddWithValue("@designation", uA.Designation);
                cmd.Parameters.AddWithValue("@Office", uA.Office);
                cmd.Parameters.AddWithValue("@department", uA.Department);
                cmd.Parameters.AddWithValue("@Jurisdiction", uA.Jurisdiction);
                cmd.Parameters.AddWithValue("@PecuniaryLimit", uA.PecuniaryLimit);
                cmd.Parameters.AddWithValue("@IsReviewer", uA.isReviewer);
                cmd.Parameters.AddWithValue("@IsApprover", uA.IsApprover);
                cmd.Parameters.AddWithValue("@UpdatedBy", Convert.ToString(HttpContext.Current.Session["User"]));//uA.UpdatedBy);
                  chk = Convert.ToInt64(cmd.ExecuteScalar());
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveUserApproval" + "_" + "Admin", 0, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return chk;
        }

        #endregion
    }
}