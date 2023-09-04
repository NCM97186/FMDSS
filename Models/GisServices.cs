using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models
{

    public class DepoList
    {
        public string REG_CODE { get; set; }
        public string CIRCLE_CODE { get; set; }
        public string DIV_CODE { get; set; }
        public string RANGE_CODE { get; set; }
        public int Depot_Id { get; set; }

        public string Depot_Name { get; set; }
        public string Depot_InCharge { get; set; }
    }

    public class NURSERYList
    {
        public string Dist_CODE { get; set; }
        public string Block_CODE { get; set; }
        public string GP_CODE { get; set; }
        public string Village_CODE { get; set; }


        public string Nursery_Code { get; set; }
        public string Nursery_Name { get; set; }

        public string Nursery_Type { get; set; }

        public string DIST_NAME { get; set; }
        public  string NURSERY_CODE { get; set; }
        public string NURSERY_NAME { get; set; }
        public int Id { get; set; }
        public string CommanEngName { get; set; }
    }


    public class NurseryDetails
    {
        public int DIST_CODE { get; set; }
    }



    public class NurseryWiseAvailableForestProduce
    {
        public string NURSERY_CODE { get; set; }
        public string NURSERY_NAME { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeID { get; set; }
        public string ProduceType { get; set; }

        public string UnitName { get; set; }
        public decimal PRODUCE_QTY { get; set; }
        public string GIS_ID { get; set; }


    }


    public class ProductList
    {
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeID { get; set; }
        public string ProduceType { get; set; }
        public string UnitName { get; set; }

    }

    public class GetDepotWiseAvailableForestProduce
    {
        public int Depot_Id { get; set; }
        public string Depot_Name { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeID { get; set; }
        public string ProduceType { get; set; }

        public string UnitName { get; set; }
        public int PRODUCE_QTY { get; set; }

        public string GIS_ID { get; set; }
    }

    public class ProgressEntry
    {
        public string ProjectName { get; set; }
        public string Scheme_Name { get; set; }

        public string SurveyDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string AreaName { get; set; }

        public string Area { get; set; }
        public string Activity_Name { get; set; }
        public DateTime Activity_StartDate { get; set; }
        public DateTime Activity_EndDate { get; set; }
        public string ActivityPercentage { get; set; }
        public string VillageCode { get; set; }
        public string MicroPlan_Code { get; set; }
        public string MicroPlanName { get; set; }
        public string PanchayatComittee { get; set; }

        public decimal AreaCoveredinSQKM { get; set; }

        public decimal ProgressStatus { get; set; }

    }


    public class ProgressEntryDistDivRange
    {
       
        public string DIV_CODE { get; set; }
        public string DIST_CODE { get; set; }
        public string VILL_CODE { get; set; }

        public string ProjectName { get; set; }
        public string Scheme_Name { get; set; }

        public decimal TotalAmount { get; set; }

        public string Activity_Name { get; set; }

        public string ActivityPercentage { get; set; }

        public decimal AreaCoveredinSQKM { get; set; }

        public decimal ProgressStatus { get; set; }

    }


    public class permissionWithKML
    {
        public string PermissionType { get; set; }
        public string PermissionTypeCatrgory { get; set; }

        public string Status { get; set; }

        public string RequestedId { get; set; }

        public string KML { get; set; }

        public string Ssoid { get; set; }

    }

    public class RegistrationOfOffense
    {
        public string CIRCLE_CODE { get; set; }
        public string DIV_CODE { get; set; }

        public string REG_CODE { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string OffenseCode { get; set; }

    }

    public class GisServices : DAL
    {
        public List<DepoList> GetDepoList()
        {
            List<DepoList> oDepoList = new List<DepoList>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "DepoServices");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);



                foreach (DataRow dr in dt.Rows)
                {
                    DepoList CD = new DepoList();


                    CD.REG_CODE = Convert.ToString(dr["REG_CODE"]);
                    CD.CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]);
                    CD.DIV_CODE = Convert.ToString(dr["DIV_CODE"]);
                    CD.RANGE_CODE = Convert.ToString(dr["RANGE_CODE"]);
                    CD.Depot_Id = Convert.ToInt32(dr["Depot_Id"]);
                    CD.Depot_Name = Convert.ToString(dr["Depot_Name"]);
                    CD.Depot_InCharge = Convert.ToString(dr["Depot_InCharge"]);

                    oDepoList.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oDepoList;
        }

        public List<NURSERYList> NurseriesList()
        {
            List<NURSERYList> oNURSERYList = new List<NURSERYList>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "NurseriesServices");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    NURSERYList CD = new NURSERYList();

                    CD.Dist_CODE = Convert.ToString(dr["Dist_CODE"]);
                    CD.Block_CODE = Convert.ToString(dr["BLK_CODE"]);
                    CD.GP_CODE = Convert.ToString(dr["GP_CODE"]);
                    CD.Village_CODE = Convert.ToString(dr["VILL_CODE"]);
                    CD.Nursery_Code = Convert.ToString(dr["NURSERY_CODE"]);
                    CD.Nursery_Name = Convert.ToString(dr["NURSERY_NAME"]);
                    CD.Nursery_Type = Convert.ToString(dr["NURSERY_TYPE"]);

                    oNURSERYList.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oNURSERYList;
        }



        public DataTable DropDownDistrict()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetDistrict");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataTable DropDownNursery(string DIST_CODE)
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetNursery");
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }



        public DataTable DropDownProduct()
        {
            DataTable dt = new DataTable();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetProduct");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }


        public DataSet GetDetailsNursery(string DIST_CODE, string NURSERY_CODE, int Id)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetDetailsNursery");
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                cmd.Parameters.AddWithValue("@NURSERY_CODE", NURSERY_CODE);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }

        public DataSet GetNurseryProductDetails(string DIST_CODE, string NURSERY_CODE, int Id)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("sp_NurseryDetails", Conn);
                cmd.Parameters.AddWithValue("@Action", "GetDetailsProductType");
                cmd.Parameters.AddWithValue("@DIST_CODE", DIST_CODE);
                cmd.Parameters.AddWithValue("@NURSERY_CODE", NURSERY_CODE);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, "BindRangeBydivisionCode" + "_" + "Admin", 0, DateTime.Now, 0);

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }



        public List<ProductList> ProductList()
        {
            List<ProductList> oProductList = new List<ProductList>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "ProductList");
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProductList CD = new ProductList();

                    CD.Product_Id = Convert.ToInt16(dr["ProductId"]);
                    CD.ProductTypeID = Convert.ToInt16(dr["ProductTypeID"]);
                    CD.ProductName = Convert.ToString(dr["ProductName"]);
                    CD.ProduceType = Convert.ToString(dr["ProduceType"]);
                    CD.UnitName = Convert.ToString(dr["UnitName"]);
                    oProductList.Add(CD);

                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oProductList;
        }


        public List<GetDepotWiseAvailableForestProduce> GetDepotListbySSOID(string SSOID)
        {

            List<GetDepotWiseAvailableForestProduce> oGetDepotWiseAvailableForestProduce = new List<GetDepotWiseAvailableForestProduce>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetDepotWiseAvailableForestProduce");
                //cmd.Parameters.AddWithValue("@REG_CODE", REG_CODE);
                //cmd.Parameters.AddWithValue("@Circle_Code", Circle_Code);
                //cmd.Parameters.AddWithValue("@DIV_Code", DIV_Code);
                //cmd.Parameters.AddWithValue("@RANGE_CODE", RANGE_CODE);
                //cmd.Parameters.AddWithValue("@Depo_id", Depo_id);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    GetDepotWiseAvailableForestProduce CD = new GetDepotWiseAvailableForestProduce();

                    CD.Depot_Id = Convert.ToInt16(dr["Depot_Id"]);
                    CD.Depot_Name = Convert.ToString(dr["Depot_Name"]);
                    CD.ProductID = Convert.ToInt16(dr["ProductID"]);
                    CD.ProductName = Convert.ToString(dr["ProductName"]);
                    CD.ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]);
                    CD.ProduceType = Convert.ToString(dr["ProduceType"]);
                    CD.UnitName = Convert.ToString(dr["UnitName"]);
                    CD.PRODUCE_QTY = Convert.ToInt16(dr["PRODUCE_QTY"]);                  
                    oGetDepotWiseAvailableForestProduce.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oGetDepotWiseAvailableForestProduce;


        }

        public List<GetDepotWiseAvailableForestProduce> GetDepotWiseAvailableForestProduce(string REG_CODE, string Circle_Code, string DIV_Code, string RANGE_CODE, string Depo_id)
        {

            List<GetDepotWiseAvailableForestProduce> oGetDepotWiseAvailableForestProduce = new List<GetDepotWiseAvailableForestProduce>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetDepotWiseAvailableForestProduce");
                cmd.Parameters.AddWithValue("@REG_CODE", REG_CODE);
                cmd.Parameters.AddWithValue("@Circle_Code", Circle_Code);
                cmd.Parameters.AddWithValue("@DIV_Code", DIV_Code);
                cmd.Parameters.AddWithValue("@RANGE_CODE", RANGE_CODE);
                cmd.Parameters.AddWithValue("@Depo_id", Depo_id);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    GetDepotWiseAvailableForestProduce CD = new GetDepotWiseAvailableForestProduce();

                    CD.Depot_Id = Convert.ToInt16(dr["Depot_Id"]);
                    CD.Depot_Name = Convert.ToString(dr["Depot_Name"]);
                    CD.ProductID = Convert.ToInt16(dr["ProductID"]);
                    CD.ProductName = Convert.ToString(dr["ProductName"]);
                    CD.ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]);
                    CD.ProduceType = Convert.ToString(dr["ProduceType"]);
                    CD.UnitName = Convert.ToString(dr["UnitName"]);
                    CD.PRODUCE_QTY = Convert.ToInt16(dr["PRODUCE_QTY"]);

                    oGetDepotWiseAvailableForestProduce.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oGetDepotWiseAvailableForestProduce;


        }

        public List<GetDepotWiseAvailableForestProduce> GetNurseryWiseAvailableForestProduce(string Dist_CODE, string Block_Code, string GP_Code, string Vill_Code, string Nursery_id)
        {
            List<GetDepotWiseAvailableForestProduce> oGetDepotWiseAvailableForestProduce = new List<GetDepotWiseAvailableForestProduce>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetNurseryWiseAvailableForestProduce");
                cmd.Parameters.AddWithValue("@Dist_CODE", Dist_CODE);
                cmd.Parameters.AddWithValue("@Block_Code", Block_Code);
                cmd.Parameters.AddWithValue("@GP_Code", GP_Code);
                cmd.Parameters.AddWithValue("@Vill_Code", Vill_Code);
                cmd.Parameters.AddWithValue("@Nursery_id", Nursery_id);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    GetDepotWiseAvailableForestProduce CD = new GetDepotWiseAvailableForestProduce();

                    CD.Depot_Id = Convert.ToInt16(dr["Depot_Id"]);
                    CD.Depot_Name = Convert.ToString(dr["Depot_Name"]);
                    CD.ProductID = Convert.ToInt16(dr["ProductID"]);
                    CD.ProductName = Convert.ToString(dr["ProductName"]);
                    CD.ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]);
                    CD.ProduceType = Convert.ToString(dr["ProduceType"]);
                    CD.UnitName = Convert.ToString(dr["UnitName"]);
                    CD.PRODUCE_QTY = Convert.ToInt16(dr["PRODUCE_QTY"]);

                    oGetDepotWiseAvailableForestProduce.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oGetDepotWiseAvailableForestProduce;
        }

        public List<ProgressEntry> GetProgress(string MicroPlan_Code, string Vill_Code, string Dist_Id)
        {
            List<ProgressEntry> oProgressEntry = new List<ProgressEntry>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetProgress");
                cmd.Parameters.AddWithValue("@MicroPlan_Code", MicroPlan_Code);
                cmd.Parameters.AddWithValue("@Vill_Code", Vill_Code);
                cmd.Parameters.AddWithValue("@Dist_CODE", Dist_Id);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProgressEntry CD = new ProgressEntry();

                    CD.ProjectName = Convert.ToString(dr["Project_Name"]);
                    CD.Scheme_Name = Convert.ToString(dr["Scheme_Name"]);
                    CD.MicroPlan_Code = Convert.ToString(dr["MicroPlan_Code"]);
                    CD.MicroPlanName = Convert.ToString(dr["MicroPlanName"]);

                    CD.SurveyDate = Convert.ToString(dr["SurveyDate"]);
                    CD.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                    CD.AreaName = Convert.ToString(dr["AreaName"]);
                    CD.Area = Convert.ToString(dr["Area"]);

                    CD.Activity_Name = Convert.ToString(dr["Activity_Name"]);
                    CD.Activity_StartDate = Convert.ToDateTime(dr["Activity_StartDate"]);
                    CD.Activity_EndDate = Convert.ToDateTime(dr["Activity_EndDate"]);
                    CD.ActivityPercentage = Convert.ToString(dr["ActivityPercentage"]);
                    CD.VillageCode = Convert.ToString(dr["VillageCode"]);

                    CD.PanchayatComittee = Convert.ToString(dr["PanchayatComittee"]);

                    CD.AreaCoveredinSQKM = Convert.ToDecimal(dr["AreaCoveredinSQKM"]);
                    //CD.ProgressStatus = Convert.ToDecimal(dr["ProgressStatus"]);

                    oProgressEntry.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oProgressEntry;
        }

        public List<ProgressEntryDistDivRange> GetProgressAdminDistDivVillageWise(string Dist_Code, string Div_Code, string Vill_Code)
        {
            List<ProgressEntryDistDivRange> oProgressEntry = new List<ProgressEntryDistDivRange>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetProgressAdminDistDivVillage");
                cmd.Parameters.AddWithValue("@Dist_CODE", Dist_Code);
                cmd.Parameters.AddWithValue("@div_CODE", Div_Code);
                cmd.Parameters.AddWithValue("@Vill_Code", Vill_Code);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProgressEntryDistDivRange CD = new ProgressEntryDistDivRange();


                    CD.DIV_CODE = Convert.ToString(dr["DIV_CODE"]);
                    CD.DIST_CODE = Convert.ToString(dr["DIST_CODE"]);
                    CD.VILL_CODE = Convert.ToString(dr["VILL_CODE"]);


                    CD.ProjectName = Convert.ToString(dr["Project_Name"]);
                    CD.Scheme_Name = Convert.ToString(dr["Scheme_Name"]);

                    CD.Activity_Name = Convert.ToString(dr["Activity_Name"]);
                    CD.ActivityPercentage = Convert.ToString(dr["ActivityPercentage"]);

                    CD.AreaCoveredinSQKM = Convert.ToDecimal(dr["AreaCoveredinSQKM"]);
                    //CD.ProgressStatus = Convert.ToDecimal(dr["ProgressStatus"]);

                    oProgressEntry.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oProgressEntry;
        }

        public List<ProgressEntryDistDivRange> GetProgressForestCirDivRangeWise(string CIRCLE_CODE, string Div_Code, string Range_Code)
        {
            List<ProgressEntryDistDivRange> oProgressEntry = new List<ProgressEntryDistDivRange>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetProgressForestCirDivRange");
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", CIRCLE_CODE);
                cmd.Parameters.AddWithValue("@DIV_Code", Div_Code);
                cmd.Parameters.AddWithValue("@RANGE_CODE", Range_Code);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    ProgressEntryDistDivRange CD = new ProgressEntryDistDivRange();


                    CD.DIV_CODE = Convert.ToString(dr["DIV_CODE"]);
                    CD.DIST_CODE = Convert.ToString(dr["DIST_CODE"]);
                    CD.VILL_CODE = Convert.ToString(dr["VILL_CODE"]);


                    CD.ProjectName = Convert.ToString(dr["Project_Name"]);
                    CD.Scheme_Name = Convert.ToString(dr["Scheme_Name"]);

                    CD.Activity_Name = Convert.ToString(dr["Activity_Name"]);
                    CD.ActivityPercentage = Convert.ToString(dr["ActivityPercentage"]);

                    CD.AreaCoveredinSQKM = Convert.ToDecimal(dr["AreaCoveredinSQKM"]);
                    //CD.ProgressStatus = Convert.ToDecimal(dr["ProgressStatus"]);

                    oProgressEntry.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oProgressEntry;
        }

        public List<permissionWithKML> GetpermissionWithKMLs(string SSOID, string KML)
        {
            List<permissionWithKML> oProgressEntry = new List<permissionWithKML>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetReceivedAppPermission");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@KML_Path", KML);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    permissionWithKML CD = new permissionWithKML();

                    CD.PermissionType = Convert.ToString(dr["PermissionType"]);
                    CD.PermissionTypeCatrgory = Convert.ToString(dr["PermissionTypeCatrgory"]);
                    CD.Status = Convert.ToString(dr["Status"]);
                    CD.RequestedId = Convert.ToString(dr["RequestedId"]);

                    CD.KML = Convert.ToString(dr["KML"]);
                    CD.Ssoid = Convert.ToString(dr["Ssoid"]);


                    oProgressEntry.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oProgressEntry;
        }

        public List<RegistrationOfOffense> GetRegistrationOfOffenses(string CIRCLE_CODE, string DIV_CODE, string RANGE_CODE)
        {
            List<RegistrationOfOffense> oProgressEntry = new List<RegistrationOfOffense>();
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetRegistrationOfOffense");
                cmd.Parameters.AddWithValue("@CIRCLE_CODE", CIRCLE_CODE);
                cmd.Parameters.AddWithValue("@DIV_CODE", DIV_CODE);
                cmd.Parameters.AddWithValue("@RANGE_CODE", RANGE_CODE);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    RegistrationOfOffense CD = new RegistrationOfOffense();

                    CD.CIRCLE_CODE = Convert.ToString(dr["CIRCLE_CODE"]);
                    CD.DIV_CODE = Convert.ToString(dr["DIV_CODE"]);
                    CD.REG_CODE = Convert.ToString(dr["REG_CODE"]);
                    CD.Latitude = Convert.ToString(dr["Latitude"]);

                    CD.Longitude = Convert.ToString(dr["Longitude"]);
                    CD.OffenseCode = Convert.ToString(dr["OffenseCode"]);


                    oProgressEntry.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oProgressEntry;
        }





        public List<GetDepotWiseAvailableForestProduce> GetDepotWiseAvailableForestProducebySSOID(string SSOID, string reqID)
        {
            DALConn();
            List<GetDepotWiseAvailableForestProduce> oGetDepotWiseAvailableForestProduce = new List<GetDepotWiseAvailableForestProduce>();
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetDepobySSoID");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@Req_ID", reqID);

                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    GetDepotWiseAvailableForestProduce CD = new GetDepotWiseAvailableForestProduce();
                    CD.Depot_Id = Convert.ToInt16(dr["Depot_Id"]);
                    CD.Depot_Name = Convert.ToString(dr["Depot_Name"]);
                    CD.ProductID = Convert.ToInt16(dr["ProductID"]);
                    CD.ProductName = Convert.ToString(dr["ProductName"]);
                    CD.ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]);
                    CD.ProduceType = Convert.ToString(dr["ProduceType"]);
                    CD.UnitName = Convert.ToString(dr["UnitName"]);
                    CD.PRODUCE_QTY = Convert.ToInt16(dr["PRODUCE_QTY"]);
                    CD.GIS_ID = Convert.ToString(dr["Gis_Id"]);                    
                    oGetDepotWiseAvailableForestProduce.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oGetDepotWiseAvailableForestProduce;


        }

        public List<NurseryWiseAvailableForestProduce> GetNurseryWiseAvailableForestProducebySSOID(string SSOID, string reqID)
        {
            DALConn();
            List<NurseryWiseAvailableForestProduce> oGetNurseryWiseAvailableForestProduce = new List<NurseryWiseAvailableForestProduce>();
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("spGIS_Services", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "GetNurserySSoID");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@Req_ID", reqID);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    NurseryWiseAvailableForestProduce CD = new NurseryWiseAvailableForestProduce();

                    CD.NURSERY_CODE = Convert.ToString(dr["NURSERY_CODE"]);
                    CD.NURSERY_NAME = Convert.ToString(dr["NURSERY_NAME"]);
                    CD.ProductID = Convert.ToInt16(dr["ProductID"]);
                    CD.ProductName = Convert.ToString(dr["ProductName"]);
                    CD.ProductTypeID = Convert.ToInt32(dr["ProductTypeID"]);
                    CD.ProduceType = Convert.ToString(dr["ProduceType"]);
                    CD.UnitName = Convert.ToString(dr["UnitName"]);
                    CD.PRODUCE_QTY = Convert.ToInt16(dr["PRODUCE_QTY"]);
                    CD.GIS_ID = Convert.ToString(dr["Gis_Id"]);
                    
                   oGetNurseryWiseAvailableForestProduce.Add(CD);
                }

            }
            catch
            {

            }
            finally
            {
                Conn.Close();
            }
            return oGetNurseryWiseAvailableForestProduce;
        }


       
            

        
    }
}