using FMDSS.CustomModels.Models;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIInterface
{
   public interface IBudgerPreSurvey
    {
        BudgetPreSurveyResponse GetBudgetPreSurveyList(int UserID, string Action);
        BooleanResponse DeleteBudgetPreSurvey(long Id, long UserID);

        DynamicResponse Select_CircleDivisionWithUserID(string Action, long UserID);

        DynamicResponse GetAllRangesAndSchemes(string Action, long UserID, string CircleCode, string DivisionCode);

        BudgetPreSurveyResponse AddPreSurveyBudget(ViewBudgetPreSurveyModel objModel, List<ViewBudgetPreSurveyModel> BudgetAllocationLists, long USERID);

    }
}
