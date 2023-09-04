using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.APIInterface
{
    public interface IBookingBoardingPass
    {
        DataSet ValidateBoardingPass(string SsoId, string RequestId, bool IsEnter, bool IsOut = false);
        DataSet GetShiftTicktStatusCounts(string SsoId, string SelectedDate, int SelectedShift);
        DataSet GetShiftList();
    }
}
