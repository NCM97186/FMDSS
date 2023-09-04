using FMDSS.Entity;
using FMDSS.Entity.JFMC.ViewModel;
using FMDSS.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.Repository.Interface
{
    public interface IJFMCRepository
    {
        DataSet JFMCRegistration_Get();
        DataSet JFMCRegistration_GetDetailsForUpdation(long objectID);
        ResponseMsg JFMCRegistration_Save(JFMCRegistration model);
    }
}
