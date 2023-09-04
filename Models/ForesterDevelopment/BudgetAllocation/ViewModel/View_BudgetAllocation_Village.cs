using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FMDSS.Repository;
using System.Data.SqlClient;

namespace FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel
{
    [Serializable]
    public class View_BudgetAllocation_Village:BaseModelSerializable
    {
        public long ID { get; set; }

        public long BudgetHeadID { get; set; }

        public long SubBudgetHeadID { get; set; }

        public int FY_ID { get; set; }

        public long AllocatedAmount { get; set; }

        public string RANGE_CODE { get; set; }

        public string RANGE_NAME { get; set; }

        public string VILL_CODE { get; set; }

        public long Activity_ID { get; set; }

        public long SubActivity { get; set; }

        public string TotalAmount { get; set; }

        public long SchemeID { get; set; }
        public string SchemeName { get; set; }

        public long ActivityID { get; set; }
        public string ActivityName { get; set; }

        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        public long AvilableAmount { get; set; }
        public long EnteredBy { get; set; }

        public string rowid { get; set; }


        public List<SelectListItem> GetRangeList()
        {

            try
            {
                Repository<View_BudgetAllocation_Village> reposit = new Repository<View_BudgetAllocation_Village>();
                List<SelectListItem> lstRange = new List<SelectListItem>();
                object[] xparams ={ 
                                     new SqlParameter("@SSO_Id",HttpContext.Current.Session["SSOid"]),    
                                     new SqlParameter("@Option","RANGE"),
                                 };

                var Range = reposit.GetWithStoredProcedure("dbo.SP_GetDivisionOnSsoId @SSO_Id,@Option", xparams).ToList();

                foreach (var item in Range)
                {
                    lstRange.Add(new SelectListItem { Text = item.RANGE_NAME, Value = item.RANGE_CODE });
                }
                return lstRange;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}