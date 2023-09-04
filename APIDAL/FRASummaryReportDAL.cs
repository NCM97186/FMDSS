using FMDSS.APIHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.APIDAL
{
    public class FRASummaryReportDAL
    {
        public static DataSet FRASummary_DistrictList(string Conn = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Conn))
                {
                    Conn = ConnectionString.Conn;
                }
                return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_FRA_API_ClaimRequestDetails, new SqlParameter("ActionCode", "FRASummaryWithDistrictList"));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataSet FRASummary_GPList(int DistrictCode, string Conn = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Conn))
                {
                    Conn = ConnectionString.Conn;
                }
                return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_FRA_API_ClaimRequestDetails,
                    new SqlParameter("ActionCode", "FRASummaryWithGPList"), new SqlParameter("ParentID", DistrictCode));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataSet FRASummary_VillageList(long GPID, string Conn = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Conn))
                {
                    Conn = ConnectionString.Conn;
                }
                return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_FRA_API_ClaimRequestDetails,
                    new SqlParameter("ActionCode", "FRASummaryWithVillageList"), new SqlParameter("ParentID", GPID));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataSet FRASummary_ClaimRequestDetailsID(long VillageID, string Conn = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Conn))
                {
                    Conn = ConnectionString.Conn;
                }
                return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_FRA_API_ClaimRequestDetails,
                    new SqlParameter("ActionCode", "FRASummaryWithClaimRequestDetailsID"), new SqlParameter("ParentID", VillageID));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static DataSet FRA_GetApplicationStatus(APIRepo.FRAStatus model, string Conn = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Conn))
                {
                    Conn = ConnectionString.Conn;
                }
                return SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, Procedures.SP_FRA_API_ClaimRequestDetails,
                    new SqlParameter("ActionCode", "GetFRARequestDetails"),
                    new SqlParameter("ParentID", model.RefNo),
                    new SqlParameter("ChildID", model.Name),
                    new SqlParameter("ClaimTypeID", model.ClaimTypeID),
                    new SqlParameter("DistrictID", model.DistrictID),
                    new SqlParameter("BlockID", model.BlockID),
                    new SqlParameter("GPID", model.GPID)

                    );
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class FRADAL
    {
        public static DataSet GetDropdownData(int actionCode, string parentID = null)
        {
            return new Repository.CommonRepository().GetDropdownData2(actionCode, parentID);
        }
        public static DataSet ClaimRequestSummaryReport(Entity.FRAViewModel.ClaimRequestParamVM param)
        {
            return new Repository.ClaimRequestRepository().GetClaimRequestSummary_Rpt(param);
        }

        public static DataSet GetClaimRequestSummarySub_Rpt(Entity.FRAViewModel.ClaimRequestSubParamVM param)
        {
            return new Repository.ClaimRequestRepository().GetClaimRequestSummarySub_Rpt(param);
        }
    }
}