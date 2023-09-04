using AutoMapper;
using FMDSS.APIInterface;
using FMDSS.Controllers;
using FMDSS.CustomModels.Models;
using FMDSS.Models.FmdssContext;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.DomainModel;
using FMDSS.Models.ForesterDevelopment.BudgetAllocation.ViewModel;
using FMDSS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.APIRepo
{
    public class PreSurveyRepo : IBudgerPreSurvey
    {
        private readonly FmdssContext fmdsscontext;
        public PreSurveyRepo()
        {
            if (fmdsscontext == null)
            {
                fmdsscontext = new FmdssContext();
            }
        }

        public BudgetPreSurveyResponse GetBudgetPreSurveyList(int UserID, string Action)
        {
            List<ViewBudgetPreSurveyModel> lstBudgetAllocationCircle = new List<ViewBudgetPreSurveyModel>();
            BudgetPreSurveyResponse list = new BudgetPreSurveyResponse();
            try
            {
                Repository<ViewBudgetPreSurveyModel> repo = new Repository<ViewBudgetPreSurveyModel>();
                var param1 = new SqlParameter("@Option", Action);
                var param2 = new SqlParameter("@UserId", UserID);
                var result = repo.GetWithStoredProcedure("BA_getBudgetPreSurveyList @Option,@UserId", param1, param2).ToList();
                #region Serialzed Model
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                lstBudgetAllocationCircle = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ViewBudgetPreSurveyModel>>(str);
                list.Data = lstBudgetAllocationCircle;
                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }


        public BooleanResponse DeleteBudgetPreSurvey(long Id, long UserID)
        {
            BooleanResponse status = new BooleanResponse();
            try
            {
                if (Id != 0)
                {
                    var tblAllocationCircle = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ID == Id);
                    if (tblAllocationCircle != null)
                    {
                        tblAllocationCircle.isActive = 0;
                        tblAllocationCircle.UpdatedBy = UserID;
                        fmdsscontext.Entry(tblAllocationCircle).State = System.Data.Entity.EntityState.Modified;
                        fmdsscontext.SaveChanges();
                        status.Data = true;
                    }
                    else
                    {
                        status.Data = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return status;
        }

        public DynamicResponse Select_CircleDivisionWithUserID(string Action, long UserID)
        {
            DynamicResponse Response = new DynamicResponse();
            List<dynamic> list = new List<dynamic>();
            try
            {


                var command = fmdsscontext.Database.Connection.CreateCommand();
                command.CommandText = "[dbo].[SP_GetPreBudgetMastersListAPI]";
                command.CommandType = CommandType.StoredProcedure;
                DbParameter param = command.CreateParameter();
                param.ParameterName = "@Action";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = Action;
                command.Parameters.Add(param);

                param = command.CreateParameter();
                param.ParameterName = "@UserID";
                param.DbType = DbType.Int64;
                param.Direction = ParameterDirection.Input;
                param.Value = UserID;
                command.Parameters.Add(param);



                fmdsscontext.Database.Connection.Open();
                var reader = command.ExecuteReader();
                List<SelectedListItem> _listCircle =((IObjectContextAdapter)fmdsscontext).ObjectContext.Translate<SelectedListItem>(reader).ToList();
                reader.NextResult();

                List<SelectedListItem> _listDivision = ((IObjectContextAdapter)fmdsscontext).ObjectContext.Translate<SelectedListItem>(reader).ToList();
                reader.NextResult();

                List<SelectedListItem> _listRange = ((IObjectContextAdapter)fmdsscontext).ObjectContext.Translate<SelectedListItem>(reader).ToList();
                reader.NextResult();

                List<BudgetMastersModel> _BudgetMaster = ((IObjectContextAdapter)fmdsscontext).ObjectContext.Translate<BudgetMastersModel>(reader).ToList();

                list.Add(_listCircle);
                list.Add(_listDivision);
                list.Add(_listRange);
                list.Add(_BudgetMaster);

                Response.Data = list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fmdsscontext.Database.Connection.Close();
            }
            return Response;
        }

        public DynamicResponse GetAllRangesAndSchemes(string Action, long UserID,string CircleCode,string DivisionCode)
        {
            DynamicResponse Response = new DynamicResponse();
            List<dynamic> list = new List<dynamic>();
            try
            {


                var command = fmdsscontext.Database.Connection.CreateCommand();
                command.CommandText = "[dbo].[GetBudgetMasterWithCDivisionWise]";
                command.CommandType = CommandType.StoredProcedure;
                DbParameter param = command.CreateParameter();
                param.ParameterName = "@Action";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = Action;
                command.Parameters.Add(param);

                param = command.CreateParameter();
                param.ParameterName = "@CircleCode";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = CircleCode;
                command.Parameters.Add(param);

                param = command.CreateParameter();
                param.ParameterName = "@DivisionCode";
                param.DbType = DbType.String;
                param.Direction = ParameterDirection.Input;
                param.Value = DivisionCode;
                command.Parameters.Add(param);

                param = command.CreateParameter();
                param.ParameterName = "@UserID";
                param.DbType = DbType.Int64;
                param.Direction = ParameterDirection.Input;
                param.Value = UserID;
                command.Parameters.Add(param);




                fmdsscontext.Database.Connection.Open();
                var reader = command.ExecuteReader();
                List<SelectedListItem> _listScheme = ((IObjectContextAdapter)fmdsscontext).ObjectContext.Translate<SelectedListItem>(reader).ToList();
                reader.NextResult();

                List<SelectedListItem> _listRange = ((IObjectContextAdapter)fmdsscontext).ObjectContext.Translate<SelectedListItem>(reader).ToList();
                reader.NextResult();


                list.Add(_listScheme);
                list.Add(_listRange);
                

                Response.Data = list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                fmdsscontext.Database.Connection.Close();
            }
            return Response;
        }


        public bool CheckDuplicateRecordsPreSurvey(ViewBudgetPreSurveyModel objModel, List<ViewBudgetPreSurveyModel> List)
        {
            List<ViewBudgetPreSurveyModel> lstChkDup = new List<ViewBudgetPreSurveyModel>();
            bool duplicate = false;
            #region If Site Name Null Or Empty then Set n/a
            if (string.IsNullOrEmpty(objModel.SiteName))
            {
                objModel.SiteName = "n/a";
            }
            #endregion

            #region Check In Table
            if (objModel != null && objModel.ISCircleDivision == "Range")
            {
                #region Check In Current Session
                int count = List.Where(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Range" && i.RangeCode == objModel.RangeCode && i.SiteName.ToLower().Trim() == objModel.SiteName.ToLower().Trim()).Count();
                if (count > 0)
                {
                    duplicate = true;
                }
                #endregion
                if (count == 0)
                {
                    tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Range" && i.RangeCode == objModel.RangeCode && i.isActive == 1 && i.SiteName.ToLower().Trim() == objModel.SiteName.ToLower().Trim());
                    if (tbl != null)
                    {
                        duplicate = true;
                    }
                }
            }
            #endregion




            return duplicate;
        }

        public BudgetPreSurveyResponse AddPreSurveyBudget(ViewBudgetPreSurveyModel objModel, List<ViewBudgetPreSurveyModel> BudgetAllocationLists,long USERID)
        {
            BudgetPreSurveyResponse Model = new BudgetPreSurveyResponse();
            try
            {

                objModel.ISCircleDivision = "Range"; //This is hard coded bcz that time only range level person is fill this form.In future this form open Division level or circle level ISCircleDivision paramenter pass with View Page

                #region Maintain the Pre Survey List
                if (BudgetAllocationLists == null)
                {
                    BudgetAllocationLists = new List<ViewBudgetPreSurveyModel>();
                }
               
                #endregion

                bool Duplicate = CheckDuplicateRecordsPreSurvey(objModel, BudgetAllocationLists);

                if (Duplicate == false)
                {


                    #region Circle and HQ
                    if (objModel != null && (objModel.ISCircleDivision == "HQ" || objModel.ISCircleDivision == "Circle"))
                    {
                        if (objModel.ISCircleDivision == "HQ")
                        {
                            objModel.CIRCLE_CODE = "HQ";
                            objModel.CIRCLE_NAME = "HQ";
                        }

                        tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && (i.ISCircleDivision == "Circle" || i.ISCircleDivision == "HQ") && i.isActive == 1);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>();
                            tbl_BudgetPreSurvey ObjbudgetCircle = Mapper.Map<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>(objModel);
                            ObjbudgetCircle.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            ObjbudgetCircle.CreatedBy = Convert.ToInt64(USERID);
                            ObjbudgetCircle.isActive = 1;
                            ObjbudgetCircle.UpdatedBy = Convert.ToInt32(USERID);
                            ObjbudgetCircle.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy");
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
                            Model.Message = "Duplicate Record";
                            Model.Status = 0;
                        }
                    }
                    #endregion

                    #region Division
                    if (objModel != null && objModel.ISCircleDivision == "Division")
                    {

                        tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Division" && i.isActive == 1);
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>();
                            tbl_BudgetPreSurvey ObjbudgetCircle = Mapper.Map<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>(objModel);
                            ObjbudgetCircle.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            ObjbudgetCircle.CreatedBy = Convert.ToInt64(USERID);
                            ObjbudgetCircle.isActive = 1;
                            ObjbudgetCircle.UpdatedBy = Convert.ToInt32(USERID);
                            ObjbudgetCircle.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy");
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
                            Model.Message = "Duplicate Record";
                            Model.Status = 0;

                        }
                    }
                    #endregion

                    #region Range
                    if (objModel != null && objModel.ISCircleDivision == "Range")
                    {

                        tbl_BudgetPreSurvey tbl = fmdsscontext.tbl_BudgetPreSurvey.FirstOrDefault(i => i.ActivityID == objModel.ActivityID && i.BudgetHeadID == objModel.BudgetHeadID && i.SchemeID == objModel.SchemeID && i.SubActivityID == objModel.SubActivityID && i.SubBudgetHeadID == objModel.SubBudgetHeadID && i.CIRCLE_CODE == objModel.CIRCLE_CODE && i.DivisionCode == objModel.DivisionCode && i.ISCircleDivision == "Range" && i.RangeCode == objModel.RangeCode && i.isActive == 1 && i.SiteName.ToLower().Trim() == objModel.SiteName.ToLower().Trim());
                        if (tbl == null)
                        {
                            Mapper.CreateMap<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>();
                            tbl_BudgetPreSurvey ObjbudgetCircle = Mapper.Map<ViewBudgetPreSurveyModel, tbl_BudgetPreSurvey>(objModel);
                            ObjbudgetCircle.CreatedOn = DateTime.Now.ToString("dd/MM/yyyy");
                            ObjbudgetCircle.CreatedBy = Convert.ToInt64(USERID);
                            ObjbudgetCircle.isActive = 1;
                            ObjbudgetCircle.UpdatedBy = Convert.ToInt32(USERID);
                            ObjbudgetCircle.UpdatedOn = DateTime.Now.ToString("dd/MM/yyyy");
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
                            Model.Message = "Duplicate Record";
                            Model.Status = 0;
                        }
                    }
                    #endregion
                }
                else
                {
                    objModel.rowid = "D";
                    Model.Message = "Duplicate Record";
                    Model.Status = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Model;
        }

    }
}