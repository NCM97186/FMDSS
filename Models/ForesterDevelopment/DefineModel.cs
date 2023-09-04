using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.ForesterDevelopment
{

    public class ActvityMap
    {
        public string ID { get; set; }

        public string ActivityID { get; set; }
        public int Activity_Year { get; set; }
        public string Activity_Year_Name { get; set; }
        public string Activity_Name { get; set; }

        public string Activity_Type { get; set; }
        public string Activity_Desc { get; set; }

        public string BitStaus { get; set; }

    }
    public class DefineModel : DAL
    {
        public Int64 UserID { get; set; }
        public long ID { get; set; }
        public long ActivityID { get; set; }
        public long Index { get; set; }
        public string TableName { get; set; }
        [Display(Name = "Model Name:")]
        public string Model_Name { get; set; }
        public string Model_DocumentPath { get; set; }
        public bool ConditionFileEditMode { get; set; }
        public string Model_RefNo { get; set; }

        public string Model_FromDate { get; set; }

        public string Model_ToDate { get; set; }

        public string CreatedDate { get; set; }
        public DateTime EnteredOn { get; set; }

        public long EnteredBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public long UpdatedBy { get; set; }

        public bool IsActive { get; set; }



        public Int64 SubmitDefineModel(DefineModel _objmodel)
        {

            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_MODEL", Conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Model_Name", _objmodel.Model_Name);
                //cmd.Parameters.AddWithValue("@Model_StartDate", DateTime.ParseExact(_objmodel.Model_FromDate.ToString(), "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@Model_ToDate", DateTime.ParseExact(_objmodel.Model_ToDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@Model_StartDate", "");
                cmd.Parameters.AddWithValue("@Model_ToDate", "");
                cmd.Parameters.AddWithValue("@Model_DocumentPath", _objmodel.Model_DocumentPath);
                cmd.Parameters.AddWithValue("@Model_RefNo", _objmodel.Model_RefNo);
                cmd.Parameters.AddWithValue("@EnteredBy", _objmodel.UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SubmitDefineModel" + "_" + "Development", 4, DateTime.Now, UserID);
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 UpdateDefineModel(DefineModel _objmodel)
        {

            try
            {
                DALConn();

                SqlCommand cmd = new SqlCommand("SP_FDM_ADD_UPDATE_MODEL", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", _objmodel.ID);
                cmd.Parameters.AddWithValue("@Model_Name", _objmodel.Model_Name);
                //cmd.Parameters.AddWithValue("@Model_StartDate", DateTime.ParseExact(_objmodel.Model_FromDate.ToString(), "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@Model_ToDate", DateTime.ParseExact(_objmodel.Model_ToDate.ToString(), "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@Model_StartDate", "");
                cmd.Parameters.AddWithValue("@Model_ToDate", "");
                cmd.Parameters.AddWithValue("@Model_DocumentPath", _objmodel.Model_DocumentPath);
                cmd.Parameters.AddWithValue("@Model_RefNo", _objmodel.Model_RefNo);
                cmd.Parameters.AddWithValue("@UpdatedBy", _objmodel.UserID);

                Int64 chId = Convert.ToInt64(cmd.ExecuteScalar());
                return chId;
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "UpdateDefineModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public Int64 DeleteDefineModel(DefineModel _objmodel)
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
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DeleteDefineModel" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return 0;
        }

        public DataSet GetAllRecords()
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_Model", Conn);
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
                SqlCommand cmd = new SqlCommand("Sp_FDM_Select_Model", Conn);
                cmd.Parameters.AddWithValue("@ModelID", ID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                
            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "GetAllRecords" + "_" + "ByID_Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public void SaveActModelMapping(List<Activity> list, Int64 UserID)
        {
            try
            {
                DataSet ds = new DataSet();
                DALConn();


                for (int i = 0; i < list.Count; i++)
                {

                    SqlCommand cmd = new SqlCommand("[Sp_Insert_ModelActivityMap]", Conn);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActivityID", list[i].ID);
                    cmd.Parameters.AddWithValue("@ModelID", list[i].Model_ID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Action", "INSERT");

                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "SaveActModelMapping" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }

        }

        public void DeleteActSubActMapping(Int64 ActivityID, Int64 ModelID)
        {
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("[Sp_Insert_ModelActivityMap]", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.Parameters.AddWithValue("@ModelID", ModelID);
                cmd.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "DeleteActSubActMapping" + "_" + "Development", 4, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));
      
            }
            finally
            {
                Conn.Close();
            }

        }
    }
}