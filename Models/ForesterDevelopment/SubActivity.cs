using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{
    public class SubActivity : DAL
    {
        public long ID { get; set; }

        public Int64 UserID { get; set; }
        public long Activity_ID { get; set; }
        public long Model_ID { get; set; }
        public string Model_Name { get; set; }
        public string Activity_Name { get; set; }
        public string Sub_Activity_Name { get; set; }
        public long Index { get; set; }
        public string Sub_Activity_Unit { get; set; }
        public string Sub_Activity_UnitName { get; set; }
        public decimal Sub_Activity_BSR_Material_Cost { get; set; }
        public decimal Sub_Activity_BSR_Labour_Cost { get; set; }
        public string TableName { get; set; }
        public decimal Sub_Activity_RatePerUnit { get; set; }
        public string Sub_Activity_DocumentPath { get; set; }
        public bool ConditionFileEditMode { get; set; }
        public string Sub_Activity_RefNo { get; set; }
        public string Sub_Activity_BSRType { get; set; }
        public decimal Sub_Activity_LaborCost { get; set; }
        public bool IsSubActivity { get; set; }

        public decimal Sub_Activity_MaterialCost { get; set; }

        public decimal Sub_Activity_totalCost { get; set; }

        public DateTime Sub_Activity_StartDate { get; set; }

        public DateTime Sub_Activity_EndDate { get; set; }

        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }


        public DataTable BindDDlActivity(Int64 Model)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_Select_ActivityByModel", Conn);
                cmd.Parameters.AddWithValue("@ModelID", Model);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
              
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDDlActivity" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public Int64 SubmitSubActivity(SubActivity _objmodel)
        {
            Int64 chId = 0;

            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_SubActivity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sub_Activity_Name", _objmodel.Sub_Activity_Name);
                cmd.Parameters.AddWithValue("@Sub_Activity_RatePerUnit", _objmodel.Sub_Activity_RatePerUnit);
                cmd.Parameters.AddWithValue("@Sub_Activity_Unit", _objmodel.Sub_Activity_Unit);
                cmd.Parameters.AddWithValue("@Sub_Activity_BSR_Material_Cost", _objmodel.Sub_Activity_BSR_Material_Cost);
                cmd.Parameters.AddWithValue("@Sub_Activity_BSR_Labour_Cost", _objmodel.Sub_Activity_BSR_Labour_Cost);
                //cmd.Parameters.AddWithValue("@Sub_Activity_BSRType", _objmodel.Sub_Activity_BSRType);
                cmd.Parameters.AddWithValue("@Sub_Activity_DocumentPath", _objmodel.Sub_Activity_DocumentPath);
                cmd.Parameters.AddWithValue("@Sub_Activity_RefNo", _objmodel.Sub_Activity_RefNo);
                cmd.Parameters.AddWithValue("@Sub_Activity_TotalCost", _objmodel.Sub_Activity_totalCost);
                cmd.Parameters.AddWithValue("@EnteredBy", _objmodel.UserID);

                  chId = Convert.ToInt64(cmd.ExecuteScalar());
          
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitSubActivity" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return chId;
        }

        public DataSet GetAllRecords()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_SubActivity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllRecords" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetAllRecords(Int64 ID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
               
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_SubActivity", Conn);
                cmd.Parameters.AddWithValue("@SID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllRecords" + "_" + "BYID_Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetMapRecords(Int64 ID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
              
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_ActivityWithMap", Conn);
                cmd.Parameters.AddWithValue("@ActivityID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
               
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetMapRecords" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public Int64 DeleteSubActivity(SubActivity _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_DeleteByTablenameandID", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@Tablename", _objmodel.TableName);


                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DeleteSubActivity" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 UpdateSubActivity(SubActivity _objmodel)
        {

            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_SubActivity", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@Sub_Activity_Name", _objmodel.Sub_Activity_Name);
                cmd.Parameters.AddWithValue("@Sub_Activity_RatePerUnit", _objmodel.Sub_Activity_RatePerUnit);
                cmd.Parameters.AddWithValue("@Sub_Activity_Unit", _objmodel.Sub_Activity_Unit);
                cmd.Parameters.AddWithValue("@Sub_Activity_BSR_Material_Cost", _objmodel.Sub_Activity_BSR_Material_Cost);
                cmd.Parameters.AddWithValue("@Sub_Activity_BSR_Labour_Cost", _objmodel.Sub_Activity_BSR_Labour_Cost);
                //cmd.Parameters.AddWithValue("@Sub_Activity_BSRType", _objmodel.Sub_Activity_BSRType);
                cmd.Parameters.AddWithValue("@Sub_Activity_TotalCost", _objmodel.Sub_Activity_totalCost);
                cmd.Parameters.AddWithValue("@Sub_Activity_DocumentPath", _objmodel.Sub_Activity_DocumentPath);
                cmd.Parameters.AddWithValue("@Sub_Activity_RefNo", _objmodel.Sub_Activity_RefNo);
                cmd.Parameters.AddWithValue("@UpdatedBy", _objmodel.UserID);


                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateSubActivity" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
       
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }
    }
    public class SubActivityModels
    {
        public string ID { get; set; }
        [Required]
        public string ActivityID { get; set; }
        [Required]
        public string SUBActivity_Name { get; set; }
        [Required]
        public string SUBActivity_FullName { get; set; }
        [Required]
        public string Unit { get; set; }

        public decimal RatePerUnit { get; set; }

        public decimal NumberPerUnit { get; set; }
        public string ReferenceNo { get; set; }

      
        public string ReferenceDoc { get; set; }
        public int IsActive { get; set; }
        public int Index { get; set; }
        public string Activity_Name { get; set; }
    }

    public class SubActivityModel : SubActivityModels
    {
        public SubActivityModel()
        {
            List = new List<SubActivityModels>();
            RatePerUnit = 0;
            NumberPerUnit = 0;
            Unit = "none";
        }
        public List<SubActivityModels> List { get; set; }
    }

    public class SubActivityRepo : DAL
    {
        public DataTable BindActivityYear()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_ActivityYear", Conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindActivityYear" + "_" + "ActivityRepo", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }
        public DataTable BindDDlBudgetMasterModel(string ActionName, int ID)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("GetBudgetMaster", Conn);
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDDlBudgetMasterModel" + "_" + "SubActivityRepo", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

        public DataSet BindDDlBudgetMasterWithSchemeWise(int SchemeID,int BudgetHeadId,int ActivityID,string ActionName)
        {
            DataSet dt = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("GetBudgetMasterWithSchemeWise", Conn);
                cmd.Parameters.AddWithValue("@Action", ActionName);
                cmd.Parameters.AddWithValue("@SchemeID", SchemeID);
                cmd.Parameters.AddWithValue("@BudgetHeadID", BudgetHeadId);
                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindDDlBudgetMasterWithSchemeWise" + "_" + "SubActivityRepo", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataSet InsertSubActivity(SubActivityModels model, string ActionName, long UserID)
        {
            DataSet dsScheme = new DataSet();
            try
            {
                DALConn();
                SqlParameter[] parameters =
                {  
                new SqlParameter("@Action",ActionName),                   
                new SqlParameter("@ID",Convert.ToInt64(model.ID)),   
                 new SqlParameter("@ActivityID",model.ActivityID),
                 new SqlParameter("@SUBActivity_FullName",model.SUBActivity_FullName),
                  new SqlParameter("@SUBActivity_Name",model.SUBActivity_Name),  
                   new SqlParameter("@Unit",model.Unit),  
                    new SqlParameter("@RatePerUnit",model.RatePerUnit),  
                     new SqlParameter("@NumberPerUnit",model.NumberPerUnit),  
                  new SqlParameter("@ReferenceNo",model.ReferenceNo),  
                   new SqlParameter("@ReferenceDoc",model.ReferenceDoc),
                    new SqlParameter("@IsActive",1),  
                    new SqlParameter("@CreatedBy",UserID),  
                };
                Fill(dsScheme, "sp_NewSUBActivityForWideLife", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "InsertSubActivity" + "_" + "SubActivityRepo", 4, DateTime.Now, UserID);

            }
            finally
            {
                Conn.Close();
            }
            return dsScheme;
        }
    }
}