using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FMDSS.Repository;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Data.Entity;



namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_Multiple_Report:BaseModelSerializable
    {     
        Repository<View_Multiple_Report> repository;
             
        public View_Multiple_Report() {

            repository = new Repository<View_Multiple_Report>();
          
        }

        public string ReportType { get; set; }

        public long FinancialYearId { get; set; }
        public string FinancialYear { get; set; }
        public long AllocatedAmount { get; set; }
        public string Circle_Name { get; set; }
        public string Circle_Code { get; set; }

        public string Division_Name { get; set; }
        public string Division_Code { get; set; }

        public string Range_Name { get; set; }
        public string Range_Code { get; set; }

        public string Village_Name { get; set; }
        public string Village_Code { get; set; }

        public string Activity_Name { get; set; }
        public long ActivityID { get; set; }

        public string SubActivity_Name { get; set; }
        public long SubActivityID { get; set; }

        public string Scheme_Name { get; set; }
        public string Scheme_Code { get; set; }
   
        public List<View_Multiple_Report> GetMultipleReport(View_Multiple_Report obj)
        {
            try
            {
                List<View_Multiple_Report> lstCircleDetails = new List<View_Multiple_Report>();               
                object[] xparams ={                                                       
                          
                             new SqlParameter("@CIRCLE_CODE",obj.Circle_Code),
                             new SqlParameter("@DIV_CODE",obj.Division_Code),
                             new SqlParameter("@RANGE_CODE",obj.Range_Code),
                             new SqlParameter("@VILL_CODE",obj.Village_Code),
                             new SqlParameter("@FY_ID",obj.FinancialYearId),
                             new SqlParameter("@Option", obj.ReportType)};

                var result = repository.GetWithStoredProcedure("dbo.SP_BudgetMultiple_Report @CIRCLE_CODE,@DIV_CODE,@RANGE_CODE,@VILL_CODE, @FY_ID,@Option", xparams).ToList();

                foreach (var item in result)
                {
                    if (obj.ReportType == "CIRCLE")
                    {
                        lstCircleDetails.Add(new View_Multiple_Report()
                        {
                            FinancialYear=item.FinancialYear,
                            Circle_Code = item.Circle_Code,
                            Circle_Name = item.Circle_Name,
                            ActivityID = item.ActivityID,
                            Activity_Name = item.Activity_Name,
                            SubActivityID=item.SubActivityID,
                            SubActivity_Name=item.SubActivity_Name,
                            AllocatedAmount = item.AllocatedAmount,
                        });
                    }
                    if (obj.ReportType == "DIVISION")
                    {
                        lstCircleDetails.Add(new View_Multiple_Report()
                        {
                            FinancialYear = item.FinancialYear,
                            Circle_Code = item.Circle_Code,
                            Circle_Name = item.Circle_Name,
                            Division_Code = item.Division_Code,
                            Division_Name = item.Division_Name,
                            ActivityID = item.ActivityID,
                            Activity_Name = item.Activity_Name,
                            SubActivityID = item.SubActivityID,
                            SubActivity_Name = item.SubActivity_Name,
                            AllocatedAmount = item.AllocatedAmount,
                        });
                    }
                    if (obj.ReportType == "RANGE")
                    {
                        lstCircleDetails.Add(new View_Multiple_Report()
                        {
                            FinancialYear = item.FinancialYear,
                            Circle_Code = item.Circle_Code,
                            Circle_Name = item.Circle_Name,
                            Division_Code = item.Division_Code,
                            Division_Name = item.Division_Name,
                            Range_Code = item.Range_Code,
                            Range_Name = item.Range_Name,
                            ActivityID = item.ActivityID,
                            Activity_Name = item.Activity_Name,
                            SubActivityID = item.SubActivityID,
                            SubActivity_Name = item.SubActivity_Name,
                            AllocatedAmount = item.AllocatedAmount,
                        });
                    }
                    if (obj.ReportType == "VILLAGE" || obj.ReportType == "FINANCIALYEAR")
                    {
                        lstCircleDetails.Add(new View_Multiple_Report()
                        {

                            FinancialYear=item.FinancialYear,
                            Circle_Code = item.Circle_Code,
                            Circle_Name = item.Circle_Name,
                            Division_Code = item.Division_Code,
                            Division_Name = item.Division_Name,
                            Range_Code = item.Range_Code,
                            Range_Name = item.Range_Name,
                            Village_Code = item.Village_Code,
                            Village_Name = item.Village_Name,
                            ActivityID = item.ActivityID,
                            Activity_Name = item.Activity_Name,
                            SubActivityID = item.SubActivityID,
                            SubActivity_Name = item.SubActivity_Name,
                            AllocatedAmount = item.AllocatedAmount,
                        });
                    }  
                                     
                }
                return lstCircleDetails;
            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}