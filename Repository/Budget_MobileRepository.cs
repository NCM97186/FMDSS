using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FMDSS.Globals;
using FMDSS.Entity.Mob_BudgetVM;

namespace FMDSS.Repository.Budget_Mobile
{
    public class Budget_MobileRepository:IBudget_MobileRepository
    {
        #region Properties & Variables 
        private FMDSS.Models.DAL _db = new Models.DAL(); 
        #endregion

        public DataSet GetUserDetails(string ssoID)
        {
            try
            { 
                var prms = new[]{
                            new SqlParameter("ActionCode", 1),
                            new SqlParameter("SSOID", ssoID)};
                DataSet data = new DataSet();
                _db.Fill(data, "SP_Mob_UserDetails", prms); 
                return data;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}