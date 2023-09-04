using AutoMapper;
using FMDSS.APIInterface;
using FMDSS.CustomModels.Models;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace FMDSS.APIRepo
{
    public class BudgetAllocation : IBudgetAllocation
    {
        private readonly FmdssContext fmdsscontext;
        public BudgetAllocation()
        {
            if (fmdsscontext == null)
            {
                fmdsscontext = new FmdssContext();
            }
        }
        public BudgetAllocationPerposalResponse GetBudgetAllocationList(long UserID)
        {
            BudgetAllocationPerposalResponse response = new BudgetAllocationPerposalResponse();
            try
            {
                DataSet ds = new DataSet();
                ds = FMDSS.APIDAL.BudgetAllocation.GetBudgetPerposalCircleList("CIRCLE", UserID);
              //  if (ds.isValidDataSet())
               // {
                    response.Data = Util.GetListFromTable<ViewBudgetAllocationPerposalModel>(ds, 0);
              //  }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //response = Response.Failed(response, ex, "", System.Reflection.MethodBase.GetCurrentMethod().Name, _companyinfo.PrivateConnectionString, _companyinfo.CompanyID);
            }
            return response;
        }

        public bool CheckDuplicateRecordsMasterPerposal(ViewBudgetAllocationPerposalModel objModel, List<ViewBudgetAllocationPerposalModel> List)
        {

            List<ViewBudgetAllocationPerposalModel> lstChkDup = new List<ViewBudgetAllocationPerposalModel>();
            bool duplicate = false;
            try
            {
                #region Check In Table
                if (objModel != null && objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle")
                {
                    #region Check In Current Session
                    int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ")).Count();
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                    #endregion
                    if (count == 0)
                    {
                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                        if (tbl != null)
                        {
                            duplicate = true;
                        }
                    }

                }
                else if (objModel != null && objModel.ISCircleDivision == "Division")
                {
                    #region Check In Current Session
                    int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division").Count();
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                    #endregion
                    if (count == 0)
                    {
                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                        if (tbl != null)
                        {
                            duplicate = true;
                        }
                    }
                }

                else if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
                {
                    #region Check In Current Session
                    int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode).Count();
                    if (count > 0)
                    {
                        duplicate = true;
                    }
                    #endregion
                    if (count == 0)
                    {
                        tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true);
                        if (tbl != null)
                        {
                            duplicate = true;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return duplicate;
        }

        public ViewBudgetAllocationPerposalViewResponse AddPerposalMaster(ViewBudgetAllocationPerposalModel objModel, List<ViewBudgetAllocationPerposalModel> BudgetAllocationLists, long UserID)
        {
            ViewBudgetAllocationPerposalViewResponse Model = new ViewBudgetAllocationPerposalViewResponse();
            try
            {
                #region Circle and HQ
                if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                {
                    if (objModel.ISCircleDivision == "HQ")
                    {
                        objModel.CIRCLE_CODE = "HQ";
                        objModel.CIRCLE_NAME = "HQ";
                    }

                    tbl_BudgetAllocation_Circle tbl = fmdsscontext.tbl_BudgetAllocation_Circle.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == true);
                    if (tbl == null)
                    {
                        Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                        tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                        ObjbudgetCircle.EnteredOn = DateTime.Now;
                        ObjbudgetCircle.EnteredBy = Convert.ToInt64(UserID);
                        ObjbudgetCircle.isActive = true;
                        fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                        fmdsscontext.SaveChanges();
                        long Id = ObjbudgetCircle.ID;
                        objModel.rowid = Convert.ToString(Id);

                        #region Add Data in List
                        BudgetAllocationLists.Add(objModel);
                        #endregion
                    }
                    else
                    {
                        objModel.rowid = "D";
                    }
                }
                #endregion

                #region Division
                if (objModel != null && objModel.ISCircleDivision == "Division")
                {

                    tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Division" && i.isActive == true);
                    if (tbl == null)
                    {
                        Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                        tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                        ObjbudgetCircle.EnteredOn = DateTime.Now;
                        ObjbudgetCircle.EnteredBy = Convert.ToInt64(UserID);
                        ObjbudgetCircle.isActive = true;
                        fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                        fmdsscontext.SaveChanges();
                        long Id = ObjbudgetCircle.ID;
                        objModel.rowid = Convert.ToString(Id);

                        #region Add Data in List
                        BudgetAllocationLists.Add(objModel);
                        #endregion
                    }
                    else
                    {
                        objModel.rowid = "D";
                    }
                }
                #endregion

                #region Sanctuary
                if (objModel != null && objModel.ISCircleDivision == "Sanctuary")
                {

                    tbl_BudgetAllocationPerposal tbl = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.Division == objModel.Division && i.ISCircleDivision == "Sanctuary" && i.SanctuaryCode == objModel.SanctuaryCode && i.isActive == true);
                    if (tbl == null)
                    {
                        Mapper.CreateMap<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>();
                        tbl_BudgetAllocationPerposal ObjbudgetCircle = Mapper.Map<ViewBudgetAllocationPerposalModel, tbl_BudgetAllocationPerposal>(objModel);
                        ObjbudgetCircle.EnteredOn = DateTime.Now;
                        ObjbudgetCircle.EnteredBy = Convert.ToInt64(UserID);
                        ObjbudgetCircle.isActive = true;
                        fmdsscontext.Entry(ObjbudgetCircle).State = System.Data.Entity.EntityState.Added;
                        fmdsscontext.SaveChanges();
                        long Id = ObjbudgetCircle.ID;
                        objModel.rowid = Convert.ToString(Id);

                        #region Add Data in List
                        BudgetAllocationLists.Add(objModel);
                        #endregion
                    }
                    else
                    {
                        objModel.rowid = "D";
                    }
                }
                #endregion

                Model.Model = objModel;
                Model.List = BudgetAllocationLists;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Model;
        }

        public BooleanResponse DeleteBudgetAllocatedEntryForPerposal(long Id)
        {
            BooleanResponse response = new BooleanResponse();
            response.Data = false;
            long status = 0;
            try
            {
                if (Id != 0)
                {
                    tbl_BudgetAllocationPerposal tblAllocationCircle = fmdsscontext.tbl_BudgetAllocationPerposal.FirstOrDefault(i => i.ID == Id);

                    if (tblAllocationCircle != null)
                    {
                        tblAllocationCircle.isActive = false;
                        fmdsscontext.Entry(tblAllocationCircle).State = System.Data.Entity.EntityState.Modified;
                        status = fmdsscontext.SaveChanges();
                        response.Data = true;
                    }
                    else
                    {
                        status = 0;
                        response.Data = false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return response;
        }
    }
}