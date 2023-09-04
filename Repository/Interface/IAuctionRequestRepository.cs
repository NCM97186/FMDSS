using FMDSS.Entity;
using FMDSS.Entity.DOD.ViewModel;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FMDSS.Repository.Interface
{
    public interface IAuctionRequestRepository
    {
        #region [Customer Part]
        List<AuctionTransaction> GetAuctionDetailsForCustomer();
        AuctionVM GetNoticeDetails(long noticeID);
        List<AuctionVM> GetNoticeDetailsForAuction();
        DataSet SaveAuctionDetails(AuctionRequest model);
        DataSet SaveAuctionDetailsWithoutPayment(AuctionVM model);
        #endregion

        #region [Admin Part]
        List<AuctionTransaction> GetAuctionDetails();
        List<AuctionClearanceDetails> GetAuctionDetailsForClearance();
        DataTable GetDetailsByInventory(string parentID, string childID);
        ResponseMsg ValidateUser(long noticeID);
        AuctionVM GetNoticeDetails(Int64 auctionID, string actionCode);
        ResponseMsg SaveAuctionClearance(AuctionClearanceVM model);
        DataSet GetNoticeDataForAuctionClearance();
        DataTable GetAuctionNoticeListForAuctionClearance(string inventoryID);
        List<AuctionPaymentDetails> GetPaymentDetails(Int64 auctionID, string actionCode);
        ResponseMsg UpdateAuctionWinner(AuctionVM model);
        ResponseMsg UpdateAuctionWinnerDept(AuctionVM model);
        ResponseMsg GenerateReport(string reqID, string reportType, string rootPath, E_SignIntegration.clsVerifyOTPResponce otpResponse);
        #endregion

        #region DOD Report
        DataTable DODInventory_Report(Entity.Protection.ViewModel.OffenceReportVM model, int actionCode = 1);
        #endregion
    }
}