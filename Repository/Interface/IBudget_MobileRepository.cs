using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FMDSS.Repository.Budget_Mobile
{
    public interface IBudget_MobileRepository
    {
        DataSet GetUserDetails(string ssoID);
    }
}