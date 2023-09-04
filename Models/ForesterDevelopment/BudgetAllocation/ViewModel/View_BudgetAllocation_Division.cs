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
    public class View_BudgetAllocation_Division:BaseModelSerializable
    {
        public long ID { get; set; }
        public long BudgetHeadID { get; set; }

        public long SubBudgetHeadID { get; set; }

        public int FY_ID { get; set; }

        public long AllocatedAmount { get; set; }
        public long AvilableAmount { get; set; }
        public string DIV_NAME { get; set; }
        public string DIV_CODE { get; set; }
        public long SchemeID { get; set; }
        public string SchemeName { get; set; }
        public long ActivityID { get; set; }
        public string ActivityName { get; set; }
        public long SubActivityID { get; set; }
        public string SubActivityName { get; set; }
        public string rowid { get; set; }
       // public DateTime EnteredOn { get; set; }                  
        public long EnteredBy { get; set; }


        public List<SelectListItem> GetDivisionList()
        {

            try
            {
                Repository<View_BudgetAllocation_Division> reposit = new Repository<View_BudgetAllocation_Division>();
                List<SelectListItem> lstDiv = new List<SelectListItem>();
                object[] xparams ={ 
                                     new SqlParameter("@SSO_Id",HttpContext.Current.Session["SSOid"]),    
                                     new SqlParameter("@Option","DIVISION"),
                                 };

                var Division = reposit.GetWithStoredProcedure("dbo.SP_GetDivisionOnSsoId @SSO_Id,@Option", xparams).ToList();

                foreach (var item in Division)
                {
                    lstDiv.Add(new SelectListItem { Text = item.DIV_NAME, Value = item.DIV_CODE });
                }
                return lstDiv;
            }
            catch (Exception)
            {
                throw;
            }

        }

      

      
    }
}