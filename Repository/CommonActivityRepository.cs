using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Data.Common;  
using System.Reflection;
using FMDSS.Entity.VM;
using FMDSS.Models.FmdssContext;
using System.Data.SqlClient;

namespace FMDSS.Repository
{
    public class CommonActivity
    {
        #region Properties & Variables
        private FmdssContext dbContext;
        private FMDSS.Models.DAL _db = new Models.DAL(); 
        #endregion

        public void SaveActivity(ServiceActivity model)
        { 
            var prms = new[]{
                            new SqlParameter("ModuleName", model.ModuleName),
                            new SqlParameter("ServiceName", model.ServiceName),
                            new SqlParameter("Request", model.Request),
                            new SqlParameter("Response", model.Response)};
            DataSet data = new DataSet();
            _db.Fill(data, "SP_ServiceActivity_Save", prms);
        }
    }
}
