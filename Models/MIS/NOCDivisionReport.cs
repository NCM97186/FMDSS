using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.MIS
{

    public class NocStatusReportModel
    {
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string STATUS { get; set; }
        public string NOCsPermission { get; set; }
        public string EducationPermission { get; set; }
        public string CampPermission { get; set; }
        public string FilmShootingPermission { get; set; }
        public string MultipleRequestID { get; set; }
    }

    public class NocStatusReportModelbySummary
    {
        public string Permission { get; set; }
        public int Total { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Reviewed { get; set; }
    }

    public class NOCFilterModel
    {

        public string DURATION { get; set; }
        public string DATETIME_FROM { get; set; }
        public string DATETIME_TO { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string DIVISON_CODE { get; set; }
        public string Action { get; set; }
    }


    public class NOCDivisionReportModel
    {
        public NOCDivisionReportModel()
        {
            model = new NOCFilterModel();
        }

        public List<NocStatusReportModel> TotalStatus { get; set; }
        public List<NocStatusReportModel> Pending { get; set; }
        public List<NocStatusReportModel> Approved { get; set; }
        public List<NocStatusReportModel> Rejected { get; set; }
        public List<NocStatusReportModel> Reviewed { get; set; }
        public List<NocStatusReportModel> Summary { get; set; }
        public NOCFilterModel model { get; set; }
        public List <NocStatusReportModelbySummary> modelsummary { get; set; }
    }

    public class NOCDivisionRepository : DAL
    {
        public NOCDivisionReportModel GetNOCReport(NOCDivisionReportModel Parentmodel)
        {
            try
            {
                Parentmodel.model.Action = string.IsNullOrEmpty(Parentmodel.model.Action) ? "Details" : Parentmodel.model.Action;

                #region Call Store Procedure
                DALConn();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("MIS_CitizenPermissionsDivisionWiseSummaryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", Parentmodel.model.Action);
                cmd.Parameters.AddWithValue("@DURATION", string.IsNullOrEmpty(Parentmodel.model.DURATION) ? "Yearly" : Parentmodel.model.DURATION);
                cmd.Parameters.AddWithValue("@DATETIME_FROM", string.IsNullOrEmpty(Parentmodel.model.DATETIME_FROM) ? string.Empty : Parentmodel.model.DATETIME_FROM);
                cmd.Parameters.AddWithValue("@DATETIME_TO", string.IsNullOrEmpty(Parentmodel.model.DATETIME_TO) ? string.Empty : Parentmodel.model.DATETIME_TO);
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", string.IsNullOrEmpty(Parentmodel.model.CIRCLE_CODE) ? "ALL" : Parentmodel.model.CIRCLE_CODE);
                cmd.Parameters.AddWithValue("@DIVISON_CODE", string.IsNullOrEmpty(Parentmodel.model.DIVISON_CODE) ? "ALL" : Parentmodel.model.DIVISON_CODE);

                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;

                da.Fill(ds);
                #endregion

                if (Parentmodel.model.Action == "Details")
                {
                    #region Code manipulate If Action="Details"
                    
                    string str = string.Empty;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null )
                        {
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                            Parentmodel.TotalStatus = new List<NocStatusReportModel>();
                            Parentmodel.TotalStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NocStatusReportModel>>(str);
                            //Parentmodel.NOCReportList.Add(model);
                        }
                        if (ds.Tables[1] != null )
                        {
                            str = string.Empty;
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1]);
                            Parentmodel.Pending = new List<NocStatusReportModel>();
                            Parentmodel.Pending = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NocStatusReportModel>>(str);
                        }
                        if (ds.Tables[2] != null )
                        {
                            str = string.Empty;
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2]);
                            Parentmodel.Approved = new List<NocStatusReportModel>();
                            Parentmodel.Approved = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NocStatusReportModel>>(str);
                        }
                        if (ds.Tables[3] != null  )
                        {
                            str = string.Empty;
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[3]);
                            Parentmodel.Rejected = new List<NocStatusReportModel>();
                            Parentmodel.Rejected = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NocStatusReportModel>>(str);
                        }
                        if (ds.Tables[4] != null )
                        {
                            str = string.Empty;
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[4]);
                            Parentmodel.Reviewed = new List<NocStatusReportModel>();
                            Parentmodel.Reviewed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NocStatusReportModel>>(str);
                        }
                    }
                    #endregion
                    return Parentmodel;
                }
                else
                {
                    #region Code Manipulate IF Action="Summary"
                    string str = string.Empty;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0] != null )
                        {
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                            Parentmodel.modelsummary = new List<NocStatusReportModelbySummary>();
                            Parentmodel.modelsummary = Newtonsoft.Json.JsonConvert.DeserializeObject<List<NocStatusReportModelbySummary>>(str);
                            //Parentmodel.NOCReportList.Add(model);
                        }
                    }
                    #endregion
                    return Parentmodel;
                }
                
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
    }


    //public static class Extension
    //{
    //    public static bool IsValidTables(this DataTable dt)
    //    {
    //        return (dt != null && dt.Rows.Count > 0);
    //    }

    //    public static bool IsVaildDataSets(this DataSet ds)
    //    {
    //        return ds != null && ds.Tables.Count > 0;
    //    }
    //}



}



