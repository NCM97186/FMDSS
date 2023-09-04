using FMDSS.App_Start;
using FMDSS.Entity;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.NationalPark;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Controllers.Master
{
    public class NationalParkMasterController : BaseController
    {
        FMDSS.Models.DAL dl = new Models.DAL();
        private FmdssContext dbContext;
        public NationalParkMasterController()
        {
            dbContext = new FmdssContext();
        }

        #region Shif Master
        public ActionResult Shift()
        {
            try
            {
                var model = dbContext.NP_ShiftMaster.Select(s => new ShiftModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Duration = s.Duration,
                    IsActive = s.IsActive
                }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult CreateEditShift(int id = 0)
        {
            ShiftModel model;
            if (id == 0)
                model = new ShiftModel { Id = 0 };
            else
            {
                var shiftEntity = dbContext.NP_ShiftMaster.FirstOrDefault(p => p.Id == id);
                if (shiftEntity == null)
                    model = new ShiftModel { Id = 0 };
                else
                {
                    model = new ShiftModel { Id = shiftEntity.Id, Duration = shiftEntity.Duration, IsActive = shiftEntity.IsActive, Name = shiftEntity.Name };
                }
            }
            return PartialView("_CreateEditShift", model);
        }
        [HttpPost]
        public ActionResult CreateEditShift(ShiftModel model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SaveShift(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("Shift");
        }
        private void SaveShift(ShiftModel model, Int64 userId)
        {
            NP_ShiftMaster shiftEntity;
            if (model.Id > 0)
            {
                shiftEntity = dbContext.NP_ShiftMaster.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                shiftEntity = new NP_ShiftMaster();
                shiftEntity.Id = 0;
            }
            shiftEntity.Name = model.Name;
            shiftEntity.Duration = model.Duration;
            shiftEntity.IsActive = model.IsActive;
            shiftEntity.AddDate = DateTime.Now;
            shiftEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_ShiftMaster.Add(shiftEntity);
            }
            dbContext.SaveChanges();
        }
        #endregion

        #region Visitor Type Master
        public ActionResult VisitorType()
        {
            try
            {
                var model = dbContext.NP_VisitorTypeMaster.Select(s => new CommonModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsActive = s.IsActive
                }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult CreateEditVisitor(int id = 0)
        {
            CommonModel model;
            if (id == 0)
                model = new CommonModel { Id = 0 };
            else
            {
                var visitorEntity = dbContext.NP_VisitorTypeMaster.FirstOrDefault(p => p.Id == id);
                if (visitorEntity == null)
                    model = new CommonModel { Id = 0 };
                else
                {
                    model = new CommonModel { Id = visitorEntity.Id, IsActive = visitorEntity.IsActive, Name = visitorEntity.Name };
                }
            }
            return PartialView("_CreateEditVisitor", model);
        }
        [HttpPost]
        public ActionResult CreateEditVisitor(CommonModel model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SaveVisitor(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("VisitorType");
        }
        private void SaveVisitor(CommonModel model, Int64 userId)
        {
            NP_VisitorTypeMaster visitorEntity;
            if (model.Id > 0)
            {
                visitorEntity = dbContext.NP_VisitorTypeMaster.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                visitorEntity = new NP_VisitorTypeMaster();
                visitorEntity.Id = 0;
            }
            visitorEntity.Name = model.Name;
            visitorEntity.IsActive = model.IsActive;
            visitorEntity.AddDate = DateTime.Now;
            visitorEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_VisitorTypeMaster.Add(visitorEntity);
            }
            dbContext.SaveChanges();
        }
        #endregion

        #region Vehicle Master
        public ActionResult Vehicle()
        {
            try
            {
                var model = dbContext.NP_VehicleMaster.Select(s => new VehicleModule
                {
                    Id = s.Id,
                    Name = s.Name,
                    SeatAlloted = s.SeatAlloted,
                    GovtSpecifiedOrPrivate = (VehicleType)s.GovtSpecifiedOrPrivate,
                    FeesApplicable = (FeesApplicable)s.FeesApplicable,
                    IsActive = s.IsActive
                }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult CreateEditVehicle(int id = 0)
        {
            VehicleModule model;
            if (id == 0)
                model = new VehicleModule { Id = 0 };
            else
            {
                var vehicleEntity = dbContext.NP_VehicleMaster.FirstOrDefault(p => p.Id == id);
                if (vehicleEntity == null)
                    model = new VehicleModule { Id = 0 };
                else
                {
                    model = new VehicleModule { Id = vehicleEntity.Id, IsActive = vehicleEntity.IsActive, Name = vehicleEntity.Name, SeatAlloted = vehicleEntity.SeatAlloted, FeesApplicable = (FeesApplicable)vehicleEntity.FeesApplicable, GovtSpecifiedOrPrivate = (VehicleType)vehicleEntity.GovtSpecifiedOrPrivate };
                }
            }
            return PartialView("_CreateEditVehicle", model);
        }
        [HttpPost]
        public ActionResult CreateEditVehicle(VehicleModule model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SaveVehicle(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("Vehicle");
        }
        private void SaveVehicle(VehicleModule model, Int64 userId)
        {
            NP_VehicleMaster vehicleEntity;
            if (model.Id > 0)
            {
                vehicleEntity = dbContext.NP_VehicleMaster.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                vehicleEntity = new NP_VehicleMaster();
                vehicleEntity.Id = 0;
            }
            vehicleEntity.Name = model.Name;
            vehicleEntity.IsActive = model.IsActive;
            vehicleEntity.SeatAlloted = model.SeatAlloted;
            vehicleEntity.GovtSpecifiedOrPrivate = (byte)model.GovtSpecifiedOrPrivate;
            vehicleEntity.FeesApplicable = (byte)model.FeesApplicable;
            vehicleEntity.AddDate = DateTime.Now;
            vehicleEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_VehicleMaster.Add(vehicleEntity);
            }
            dbContext.SaveChanges();
        }
        #endregion

        #region Head Master
        public ActionResult Head()
        {
            try
            {
                var model = dbContext.NP_HeadMaster.Select(s => new HeadModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    EmitraHeadCode = s.EmitraHeadCode,
                    EmitraKioskHeadCode = s.EmitraKioskHeadCode,
                    IsActive = s.IsActive
                }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult CreateEditHead(int id = 0)
        {
            HeadModel model;
            if (id == 0)
                model = new HeadModel { Id = 0 };
            else
            {
                var headEntity = dbContext.NP_HeadMaster.FirstOrDefault(p => p.Id == id);
                if (headEntity == null)
                    model = new HeadModel { Id = 0 };
                else
                {
                    model = new HeadModel
                    {
                        Id = headEntity.Id,
                        EmitraHeadCode = headEntity.EmitraHeadCode,
                        EmitraKioskHeadCode = headEntity.EmitraKioskHeadCode,
                        IsActive = headEntity.IsActive,
                        Name = headEntity.Name
                    };
                }
            }
            return PartialView("_CreateEditHead", model);
        }
        [HttpPost]
        public ActionResult CreateEditHead(HeadModel model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SaveHead(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("Head");
        }
        private void SaveHead(HeadModel model, Int64 userId)
        {
            NP_HeadMaster headEntity;
            if (model.Id > 0)
            {
                headEntity = dbContext.NP_HeadMaster.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                headEntity = new NP_HeadMaster();
                headEntity.Id = 0;
            }
            headEntity.Name = model.Name;
            headEntity.EmitraHeadCode = model.EmitraHeadCode;
            headEntity.EmitraKioskHeadCode = model.EmitraKioskHeadCode;
            headEntity.IsActive = model.IsActive;
            headEntity.AddDate = DateTime.Now;
            headEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_HeadMaster.Add(headEntity);
            }
            dbContext.SaveChanges();
        }
        #endregion

        #region Head Required
        public ActionResult HeadRequired(int id = 0)
        {
            DataTable Ds = new DataTable();
            dl.Fill(Ds, "SpGetNPData");

            if (Ds != null && Ds.Rows.Count > 0)
            {
                ViewBag.PlaceList = Ds.ToSelectListExt("Id", "Name");
            }
            return View();
        }

        [HttpPost]
        public PartialViewResult GetHeadDetail(int id = 0)
        {
            List<HeadRequired> lst = new List<HeadRequired>();
            DataTable Ds = new DataTable();
            SqlParameter[] prms = { new SqlParameter("@PlaceId", id) };
            dl.Fill(Ds, "SpHeadRequiredIsRequired", prms);
            if (Ds != null && Ds.Rows.Count > 0)
            {
                foreach (DataRow dr in Ds.Rows)
                {
                    HeadRequired req = new HeadRequired();
                    req.Id = Convert.ToInt32(dr["Id"]);
                    req.Name = Convert.ToString(dr["Name"]);
                    req.IsRequired = Convert.ToBoolean(dr["IsRequired"]);
                    lst.Add(req);
                }
            }
            return PartialView("_HeadDetail", lst);
        }

        [HttpPost]
        public ActionResult HeadRequired(int PlaceId, string hid)
        {
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SqlParameter[] prms = {
                        new SqlParameter("@PlaceId", PlaceId),
                        new SqlParameter("@Ids", hid),
                        new SqlParameter("@UserId", UserID)
                    };
                    if (dl.ExecuteNonQuery("SpInsertHeadRequired", prms) > 0)
                    {
                        TempData["RecordStatus"] = 1;
                    }
                }
            }
            return RedirectToAction("HeadRequired");
        }

        public ActionResult PlaceInventory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PlaceInventory(int hid, int tbOnlineSeat, int tbOfflineSeat)
        {
            
             if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SqlParameter[] prms = {
                        new SqlParameter("@Id", hid ),
                        new SqlParameter("@OnlineSeat", tbOnlineSeat),
                        new SqlParameter("@OfflineSeat", tbOfflineSeat)
                    };
                    if (dl.ExecuteNonQuery("SpUpdateNPPlaceInventory", prms) > 0)
                    {
                        TempData["RecordStatus"] = 1;
                    }
                }
            }
            return RedirectToAction("PlaceInventory");
        }
        [HttpPost]
        public JsonResult GetData(string option, int? placeid, int? zoneid, int? shiftid, int? vehicleid)
        {
            DataTable Ds = new DataTable();
            SqlParameter[] param = {new SqlParameter("@Option",option),
                                        new SqlParameter("@PlaceId",placeid),
                                        new SqlParameter("@ZoneId",zoneid),
                                        new SqlParameter("@ShiftId",shiftid),
                                        new SqlParameter("@VehicleId",vehicleid)
                                   };

            dl.Fill(Ds, "SpGetNPData", param);
            if (Ds != null && Ds.Rows.Count > 0)
            {
                if (option.ToLower() == "seat")
                {
                    var str = new
                    {
                        id = Ds.Rows[0]["Id"],
                        OnlineSeats = Ds.Rows[0]["OnlineSeats"],
                        OfflineSeats = Ds.Rows[0]["OfflineSeats"]
                    };
                    return Json(str, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(Ds.ToSelectListExt("Id", "Name"), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Head Item Master
        public ActionResult HeadItem()
        {
            try
            {
                var model = dbContext.NP_FeesItemMaster.Select(s => new CommonModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsActive = s.IsActive
                }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult CreateEditHeadItem(int id = 0)
        {
            CommonModel model;
            if (id == 0)
                model = new CommonModel { Id = 0 };
            else
            {
                var itemEntity = dbContext.NP_FeesItemMaster.FirstOrDefault(p => p.Id == id);
                if (itemEntity == null)
                    model = new CommonModel { Id = 0 };
                else
                {
                    model = new CommonModel { Id = itemEntity.Id, IsActive = itemEntity.IsActive, Name = itemEntity.Name };
                }
            }
            return PartialView("_CreateEditHeadItem", model);
        }
        [HttpPost]
        public ActionResult CreateEditHeadItem(CommonModel model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SaveHeadItem(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("HeadItem");
        }
        private void SaveHeadItem(CommonModel model, Int64 userId)
        {
            NP_FeesItemMaster itemEntity;
            if (model.Id > 0)
            {
                itemEntity = dbContext.NP_FeesItemMaster.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                itemEntity = new NP_FeesItemMaster();
                itemEntity.Id = 0;
            }
            itemEntity.Name = model.Name;
            itemEntity.IsActive = model.IsActive;
            itemEntity.AddDate = DateTime.Now;
            itemEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_FeesItemMaster.Add(itemEntity);
            }
            dbContext.SaveChanges();
        }
        #endregion

        #region Place Mapping
        public ActionResult PlaceMapping()
        {
            try
            {
                var model = dbContext.NP_PlaceRelation.OrderBy(p => p.ZoneId).ThenBy(p => p.ZoneId).ThenBy(p => p.ShiftId).Select(s => new PlaceMappingModel
                {
                    Id = s.Id,
                    PlaceName = s.tbl_mst_Places.PlaceName,
                    ZoneName = s.ZoneId.HasValue ? s.tbl_mst_Zone.ZoneName : "-",
                    ShiftName = s.ShiftId.HasValue ? s.NP_ShiftMaster.Name : "-",
                    VehicleName = s.VehicleId.HasValue ? s.NP_VehicleMaster.Name : "-",
                    IsActive = s.IsActive
                }).ToList();
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult CreateEditPlaceMapping(int id = 0)
        {
            PlaceMappingModel model;
            if (id == 0)
                model = new PlaceMappingModel { Id = 0 };
            else
            {
                var placeMappingEntity = dbContext.NP_PlaceRelation.FirstOrDefault(p => p.Id == id);
                if (placeMappingEntity == null)
                    model = new PlaceMappingModel { Id = 0 };
                else
                {
                    model = new PlaceMappingModel { Id = placeMappingEntity.Id, PlaceId = placeMappingEntity.PlaceId, ZoneId = placeMappingEntity.ZoneId, VehicleId = placeMappingEntity.VehicleId, ShiftId = placeMappingEntity.ShiftId, IsActive = placeMappingEntity.IsActive };
                }
            }
            FillViewBag();
            return PartialView("_CreateEditPlaceMapping", model);
        }
        [HttpPost]
        public ActionResult CreateEditPlaceMapping(PlaceMappingModel model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SavePlaceMapping(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("PlaceMapping");
        }

        private void FillViewBag()
        {
            ViewBag.Shifts = dbContext.NP_ShiftMaster.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            ViewBag.Vehicles = dbContext.NP_VehicleMaster.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            ViewBag.Places = dbContext.tbl_mst_Places.Select(p => new SelectListItem { Text = p.PlaceName, Value = p.PlaceID.ToString() });
            ViewBag.Zones = dbContext.tbl_mst_Zone.Select(p => new SelectListItem { Text = p.ZoneName, Value = p.ZoneID.ToString() });
        }

        private void SavePlaceMapping(PlaceMappingModel model, Int64 userId)
        {
            NP_PlaceRelation placeMappingEntity;
            if (model.Id > 0)
            {
                placeMappingEntity = dbContext.NP_PlaceRelation.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                placeMappingEntity = new NP_PlaceRelation();
                placeMappingEntity.Id = 0;
            }
            placeMappingEntity.PlaceId = model.PlaceId;
            placeMappingEntity.ZoneId = model.ZoneId;
            placeMappingEntity.ShiftId = model.ShiftId;
            placeMappingEntity.VehicleId = model.VehicleId;
            placeMappingEntity.IsActive = model.IsActive;
            placeMappingEntity.AddDate = DateTime.Now;
            placeMappingEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_PlaceRelation.Add(placeMappingEntity);
            }
            dbContext.SaveChanges();
        }

        #endregion

        #region Head Wise Fees
        public ActionResult HeadWiseFees()
        {

            var placeList = dbContext.NP_HeadWiseItemFee.Select(p => new NPPlace { PlaceId = p.PlaceId, PlaceName = p.tbl_mst_Places.PlaceName }).Distinct().ToList();
            return View(placeList);

        }
        public ActionResult GetFeedDetails(int id)
        {
            var feesList = dbContext.NP_HeadWiseItemFee.Where(p => p.PlaceId == id)
                .OrderBy(h => h.HeadId).ThenBy(v => v.VisitorTypeId).ThenBy(v => v.ItemParent).ThenBy(v => v.VehicleId).ThenBy(i => i.ItemId)
                .Select(f => new HeadWiseItemFeeModel
                {
                    Id = f.Id,
                    HeadName = f.NP_HeadMaster.Name,
                    VehicleName = f.VehicleId.HasValue ? f.NP_VehicleMaster.Name : "-",
                    ItemName = f.NP_FeesItemMaster.Name,
                    VisitorType = f.VisitorTypeId.HasValue ? f.NP_VisitorTypeMaster.Name : "-",
                    Amount = f.Amount,
                    IsActive = f.IsActive,
                    ItemParentName = ((ItemParent)f.ItemParent).ToString()
                }).ToList();
            return PartialView("_FeesGrid", feesList);
        }
        public ActionResult CreateEditHeadWiseFee(int id = 0)
        {
            HeadWiseItemFeeModel model;
            if (id == 0)
                model = new HeadWiseItemFeeModel { Id = 0 };
            else
            {
                var feesEntity = dbContext.NP_HeadWiseItemFee.FirstOrDefault(p => p.Id == id);
                if (feesEntity == null)
                    model = new HeadWiseItemFeeModel { Id = 0 };
                else
                {
                    model = new HeadWiseItemFeeModel { Id = feesEntity.Id, PlaceId = feesEntity.PlaceId, VisitorTypeId = feesEntity.VisitorTypeId, VehicleId = feesEntity.VehicleId, HeadId = feesEntity.HeadId, IsActive = feesEntity.IsActive, ItemId = feesEntity.ItemId, ItemParent = (ItemParent)feesEntity.ItemParent, Amount = feesEntity.Amount, GSTApplicable = feesEntity.GSTApplicable };
                }
            }
            FillFeesViewBag();
            return PartialView("_CreateEditHeadWiseFee", model);
        }
        [HttpPost]
        public ActionResult CreateEditHeadWiseFee(HeadWiseItemFeeModel model)
        {
            TempData["RecordStatus"] = 0;
            if (ModelState.IsValid)
            {
                Int64 UserID = Convert.ToInt64(Session["UserID"]);
                if (UserID > 0)
                {
                    SaveItemFees(model, UserID);
                    TempData["RecordStatus"] = 1;
                }
            }
            return RedirectToAction("HeadWiseFees");
        }

        private void FillFeesViewBag()
        {
            ViewBag.Places = dbContext.NP_PlaceRelation.Select(p => new SelectListItem { Text = p.tbl_mst_Places.PlaceName, Value = p.tbl_mst_Places.PlaceID.ToString() }).Distinct().ToList();
            ViewBag.Heads = dbContext.NP_HeadMaster.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            ViewBag.Visitors = dbContext.NP_VisitorTypeMaster.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            ViewBag.Vehicles = dbContext.NP_VehicleMaster.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
            ViewBag.Items = dbContext.NP_FeesItemMaster.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
        }

        private void SaveItemFees(HeadWiseItemFeeModel model, Int64 userId)
        {
            NP_HeadWiseItemFee feesEntity;
            if (model.Id > 0)
            {
                feesEntity = dbContext.NP_HeadWiseItemFee.FirstOrDefault(p => p.Id == model.Id);
            }
            else
            {
                feesEntity = new NP_HeadWiseItemFee();
                feesEntity.Id = 0;
            }
            feesEntity.PlaceId = model.PlaceId;
            feesEntity.HeadId = model.HeadId;
            feesEntity.VisitorTypeId = model.VisitorTypeId;
            feesEntity.VehicleId = model.VehicleId;
            feesEntity.ItemId = model.ItemId;
            feesEntity.ItemParent = (byte)model.ItemParent;
            feesEntity.Amount = model.Amount;
            feesEntity.GSTApplicable = model.GSTApplicable;
            feesEntity.IsActive = model.IsActive;
            feesEntity.AddDate = DateTime.Now;
            feesEntity.UserId = userId;
            if (model.Id == 0)
            {
                dbContext.NP_HeadWiseItemFee.Add(feesEntity);
            }
            dbContext.SaveChanges();
        }

        #endregion


    }
}
