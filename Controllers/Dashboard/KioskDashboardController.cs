using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FMDSS.Models;
using FMDSS.Models.Home;

namespace FMDSS.Controllers.Dashboard
{
    public class KioskDashboardController : BaseController
    {
        #region Data Members
        Int64 UserID = 0;
        int ModuleID = 1;
        CitizenDashboard _objModel = new CitizenDashboard();
        Common _objCommon = new Common();
        List<CitizenDashboard> citizenList = new List<CitizenDashboard>();
        List<CitizenDashboard> citizenList1 = new List<CitizenDashboard>();
        List<CitizenDashboard> FavouriteList = new List<CitizenDashboard>();
        #endregion
        //
        // GET: /KioskDashboard/

        public ActionResult KioskDashboard()
        {
            #region Test
            string URL = string.Empty;
            string emitraServiceID = Convert.ToString(Session["EmitrServiceID"]);

            URL = GetSetPagePermission(emitraServiceID);

            if (string.IsNullOrWhiteSpace(URL))
            {
                URL = ByPassOTP(Convert.ToInt64(Session["UserID"]));
            }
            #endregion

            
            if (Session["CURRENT_Menus"] == null)
            {
                Home obj = new Home();
                Session["CURRENT_Menus"] = obj.GetCurrentMenus(Convert.ToInt16(Session["CURRENT_ROLE"]));

                if (!string.IsNullOrWhiteSpace(Convert.ToString(Session["DefaultKioskLayout"])))
                {
                    Session["CurrentLayout"] = Session["DefaultKioskLayout"];
                }
            } 

            if (URL != string.Empty)
            {

                #region emitra Kiosk User for Wildlife with multiple roles
                if (URL.Trim().ToLower() == "bookonlineticket/bookonlineticket")
                {
                    List<Menus> menu = new List<Menus>();
                    Home obj = new Home();
                    menu = _objModel.GetKioskUserMenuForWildlife(Convert.ToInt64(Session["KioskUserId"].ToString()), "WildLifeKioskUser", Convert.ToString(Session["SSOID"]));
                    Session["CURRENT_Menus"] = obj.GetCurrentMenusForWildLifeKiosk(Convert.ToInt16(Session["CURRENT_ROLE"]), menu);
                    Session["Menus"] = menu;
                }
                else if (URL.Trim().ToLower() == "bookonlineticket/bookonlinetickets")
                {
                    List<Menus> menu = new List<Menus>();
                    Home obj = new Home();
                    menu = _objModel.GetKioskUserMenuForWildlife(Convert.ToInt64(Session["KioskUserId"].ToString()), "WildLifeKioskUserForVIP", Convert.ToString(Session["SSOID"]));
                    Session["CURRENT_Menus"] = obj.GetCurrentMenusForWildLifeKiosk(Convert.ToInt16(Session["CURRENT_ROLE"]), menu);
                    Session["Menus"] = menu;
                    Session["CURRENT_ROLE"] = menu.LastOrDefault().RoleId;
                }
                #endregion

                Session["UserId"] = Convert.ToInt64(Session["UserID"]);
                return RedirectToAction(Convert.ToString(URL.Split('/')[1]), Convert.ToString(URL.Split('/')[0]));
                //return RedirectToAction("FixedPermission", "FixedPermission", new { aid = "NA%3D%3D" });
            }



            string StatementType = "GetRecords";
            DataSet dtf = _objModel.GetTransactionDashaboardKiosk(Convert.ToInt64(Session["KioskUserId"].ToString()), 1);
            //DataSet dtf1 = _objModel.GetTransactionDashaboard(UserID, 2);
            //DataSet dtf2 = _objCommon.GetAllRecords(Convert.ToInt64(UserID), StatementType);

            for (int i = 0; i < dtf.Tables.Count; i++)
            {
                foreach (DataRow dr in dtf.Tables[0].Rows)
                    citizenList.Add(
                        new CitizenDashboard()

                        {
                            RequstedID = dr["RequestedId"].ToString(),
                            RequestType = dr["PermissionDesc"].ToString(),
                            Date = dr["EnteredOn"].ToString(),
                            Status = dr["Status"].ToString(),
                            StatusDesc = dr["StatusDesc"].ToString(),
                            TableName = dr["Table_name"].ToString(),
                        });

            }

            //for (int i = 0; i < dtf1.Tables.Count; i++)
            //{
            //    foreach (DataRow dr in dtf1.Tables[0].Rows)
            //        citizenList1.Add(
            //            new CitizenDashboard()
            //            {
            //                RequstedID = dr["RequestedId"].ToString(),
            //                RequestType = dr["PermissionDesc"].ToString(),
            //                Date = dr["EnteredOn"].ToString(),
            //                Status = dr["Status"].ToString(),
            //                StatusDesc = dr["StatusDesc"].ToString(),
            //                TableName = dr["Table_name"].ToString(),
            //                PermissionID = dr["PermissionId"].ToString()
            //                // ActionID = dr["Action"].ToString()

            //            });

            //}

            //for (int i = 0; i < dtf2.Tables.Count; i++)
            //{
            //    foreach (DataRow dr in dtf2.Tables[i].Rows)
            //        FavouriteList.Add(
            //            new CitizenDashboard()
            //            {
            //                ModuleName = dr["ModuleDesc"].ToString(),
            //                UserName = dr["Name"].ToString(),
            //                PageURl = dr["URL"].ToString(),
            //                PageName = dr["PageName"].ToString(),
            //                FavouritelinkID = Convert.ToInt32(dr["ID"].ToString())

            //            });

            //}

            ViewData["citizenList"] = citizenList;
            ViewData["citizenList1"] = citizenList1;
            ViewData["FavouriteList"] = FavouriteList;
            ViewBag.UserName = Convert.ToString(Session["User"]);
            return View();
        } 

        //Added by dipak
        private string GetSetPagePermission(string emitraServiceID)
        {
            string Url = string.Empty;
            if (!string.IsNullOrWhiteSpace(emitraServiceID))
            {
                DataSet dsService = new UserProfile().FmdssEmitraServiceDetails(emitraServiceID, Convert.ToString(Session["SSOID"]));
                if (Globals.Util.isValidDataSet(dsService, 1, true))
                {
                    Url = Convert.ToString(dsService.Tables[1].Rows[0]["DefaultPageURL"]);
                    string serviceType = Convert.ToString(dsService.Tables[1].Rows[0]["ServiceType"]);
                    string defaultLayout = Convert.ToString(dsService.Tables[1].Rows[0]["DefaultLayout"]);

                    if (serviceType == "ServiceWise")
                    {
                        Session["CURRENT_Menus"] = null;
                        Session["DefaultKioskLayout"] = defaultLayout;
                        SetEmitraMenues(dsService);
                    }

                }
            }
            return Url;
        }

        private void SetEmitraMenues(DataSet dsService)
        {
            if (Globals.Util.isValidDataSet(dsService, 0, true))
            {
                var menuList = Globals.Util.GetListFromTable<Menus>(dsService, 0);
                Session["Menus"] = menuList;
            }
        }

        public string ByPassOTP(Int64 USERID)
        {

            string Url = string.Empty;
            try
            {

                if (USERID == 0)
                {


                }
                else
                {

                    if (new UserProfile().BypassOTPForKioskAsCounterUser(USERID))
                    {

                        //Session["UserId"] = Session["TempUserId"];
                        //Session["UserID"] = Session["TempUserId"];
                        Session["KioskCtznName"] = Session["SSOID"];//Session["TempKioskCtznName"];
                        //Session["TempUserId"] = null; Session["TempKioskCtznName"] = null;
                        if (Session["EmitrServiceId"] != null)
                        {
                            //DataTable dtTable = new UserProfile().BypassOTPForKioskAsCounterUserForList(USERID);
                            DataTable dtService = new UserProfile().FmdsServiceDetails(Convert.ToString(Session["EmitrServiceId"]));
                            if (dtService.Rows.Count > 0 && dtService.AsEnumerable().Where(s => s.Field<string>("ViewAction_Name").ToLower() == "bookonlinetickets").Count() > 0)
                            {
                                Url = Convert.ToString("BookOnlineTicket/BookOnlineTickets");
                            }
                            else
                            {

                                if (dtService.Rows.Count > 0)
                                {
                                    Url = Convert.ToString(dtService.Rows[0]["Controller_Name"].ToString() + "/" + dtService.Rows[0]["ViewAction_Name"]);

                                }
                            }
                        }

                    }


                }
                return Url;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
