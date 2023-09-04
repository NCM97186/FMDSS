using FMDSS.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FMDSS.Repository.Interface
{
    public interface ICommonRepository
    {
        List<CommonDocument> GetAttachedDocument(long? objectID, int objectTypeID);
        DataTable GetDocumentType(int docTypeID);
        DataTable GetServiceDetails(string serviceName);
        EnumerableRowCollection<SelectListItem> GetDropdownData(int actionCode, string parentID);
        DataSet GetDropdownData2(int actionCode, string parentID);
        string GetTempDocInXML();
        void SaveDocs(long objectID, int objectTypeID, List<CommonDocument> docs);
    }
}
