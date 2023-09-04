using FMDSS.CustomModels.Models;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIInterface
{
   public interface IBudgetAllocation
    {
        BudgetAllocationPerposalResponse GetBudgetAllocationList(long UserID);
        bool CheckDuplicateRecordsMasterPerposal(ViewBudgetAllocationPerposalModel objModel, List<ViewBudgetAllocationPerposalModel> List);

        ViewBudgetAllocationPerposalViewResponse AddPerposalMaster(ViewBudgetAllocationPerposalModel objModel, List<ViewBudgetAllocationPerposalModel> List,long UserID);

        BooleanResponse DeleteBudgetAllocatedEntryForPerposal(long Id);
    }
}
