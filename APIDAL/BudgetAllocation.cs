using FMDSS.APIHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.APIDAL
{
    public static class BudgetAllocation
    {
        
        public static DataSet GetBudgetPerposalCircleList(string  Option,long UserId,string Conn=null)
        {
            if(string.IsNullOrEmpty(Conn))
            {
                Conn = ConnectionString.Conn;
            }
            return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.BudgetPerposalList,
                    new SqlParameter("@Option", Option),new SqlParameter("UserId", UserId));
        }
    }
}