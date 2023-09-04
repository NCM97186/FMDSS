// *****************************************************************************************
// Author : Rajkumar Singh
// Created : 18-Nov-2015
// *****************************************************************************************
// <summary>This Class is developed for Stake holder services of FMDSS </summary>
// *****************************************************************************************
using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace FMDSS.Models.CitizenService.PermissionService.StakeholderService
{
    public class StakeholderServices:DAL
    {

        #region Contractor registration

        #region Propertydefine
        public Int64 ContractorID { get; set; }
        
        public string ApplicantType { get; set; }
        public string ProduceType { get; set; }
        public int isActive { get; set; }
        public Int64 UserID { get; set; }
      
        public string Commments { get; set; }
        public string Status { get; set; }
        public string RequestID { get; set; }
        public string RegistrationType { get; set; }

        public string Location_type { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }
        public string village { get; set; }
        public string Depot { get; set; }
        public string District { get; set; }
        public string National_park { get; set; }
        public string VechileType { get; set; }
        public string VehicleNo { get; set; }
        public string VehicleSeat { get; set; }

        public Int64 VehicleCatID {get; set; }
        public string VehicleCategory{ get;set; }
        public Int64 VehicleID{get;set;}
        public string Vehicle{ get;set; }
        #endregion
        public DataTable GetVehicleType()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                Fill(dt, "Get_vehicleCategory");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="VehicleCatID"></param>
        /// <returns></returns>
        public DataTable Select_vehicle(Int64 VehicleCatID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VehicleCatID", VehicleCatID) 
            };
                Fill(dt, "Select_vehicle_by_vehicleCatID", parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string RegisterContractor()
        {
            try
            {
                DALConn();
                UserID = 1;
                Status = "Pending";
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_RegisterContractor", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", ContractorID);
                cmd.Parameters.AddWithValue("@P_ApplicantType", ApplicantType);
                cmd.Parameters.AddWithValue("@P_Registration_Type", RegistrationType);
                cmd.Parameters.AddWithValue("@P_Location_Type", Location_type);
                cmd.Parameters.AddWithValue("@P_Produce_Type", ProduceType);
                cmd.Parameters.AddWithValue("@P_Division", Division);
                cmd.Parameters.AddWithValue("@P_Range", Range);
                cmd.Parameters.AddWithValue("@P_Village", village);
                cmd.Parameters.AddWithValue("@P_Depot", Depot);
                cmd.Parameters.AddWithValue("@P_District", District);
                cmd.Parameters.AddWithValue("@P_NationalPark", National_park);
                cmd.Parameters.AddWithValue("@P_isActive", isActive);
                cmd.Parameters.AddWithValue("@P_UserID", UserID);
                cmd.Parameters.AddWithValue("@P_Commments", Commments);
                cmd.Parameters.AddWithValue("@P_Status", Status);
                cmd.Parameters.AddWithValue("@P_RequestedID", RequestID);                            
                string id = cmd.ExecuteScalar().ToString();
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="RequestId"></param>
        /// <returns></returns>
        public int InsertXmldata(string RequestId)
        {
            SqlCommand sqlcmd = new SqlCommand("Sp_InsertVechileDetails", Conn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@ContractorRegistration_ID", RequestId);
            sqlcmd.Parameters.AddWithValue("@Vehicle_Type", VechileType);
            sqlcmd.Parameters.AddWithValue("@Vechile_Name", Vehicle);
            sqlcmd.Parameters.AddWithValue("@Vechile_Number", VehicleNo);
            sqlcmd.Parameters.AddWithValue("@Number_of_Seat", VehicleSeat);
            sqlcmd.Parameters.AddWithValue("@Created_By", "Rajkumar");
            int recordsInserted = sqlcmd.ExecuteNonQuery();
            return recordsInserted;
        }
        #endregion

        #region JFMC Registration

        #region property define
        public long Id { get; set; }

                [Required(ErrorMessage = "Select district")]
                public string Dist_Code { get; set; }
                [Required(ErrorMessage = "Select block")]
                public string Blk_Code { get; set; }
                [Required(ErrorMessage = "Select Gram panchayat")]
                public string Gp_Code { get; set; }
                [Required(ErrorMessage = "Select village")]       
                public string Vill_Code { get; set; }
                [Required(ErrorMessage = "Select MOM")]       
                public string Minutes_of_Meeting { get; set; }
                public string Upload_MOM { get; set; }
                [Required(ErrorMessage = "Enter Committee Name")]       
                public string Committee_Name { get; set; }
                [Required(ErrorMessage = "Enter registration id")]
                public string Registration_Id { get; set; }
                [Required(ErrorMessage = "Enter secretary name")]
                public string Secretary_Name { get; set; }
                [Required(ErrorMessage = "Enter valid ssoid")]
                public string Ssoid { get; set; }              
                public decimal Lattitude { get; set; }                           
                public decimal Longitude { get; set; }
                [Required(ErrorMessage = "Enter plantation area")]
              
                public decimal Plantation_Area { get; set; }

                [Required(ErrorMessage = "Enter total area manage")]
           
                public decimal Total_Area_Managed { get; set; }
                [Required(ErrorMessage = "Select bank a/c operated")]
              
                public string Bank_Account_Operated { get; set; }
                public string Bank_Name { get; set; }
                public string Branch_Name { get; set; }
                public string Branch_Id { get; set; }
                public string Account_No { get; set; }
                public string Account_Type { get; set; }
                [Required(ErrorMessage = "Enter membership fees")]            
                public long MembershipFees { get; set; }
                [Required(ErrorMessage = "Enter Income")]              
                public long Income_Deposit_ForestOffice { get; set; }
                [Required(ErrorMessage = "Enter Income on forest produce sale")]              
                public long Income_Sale_ForestProduce { get; set; }
                public long Income_Generated_Activity { get; set; }           
                public string Other_Sources { get; set; }
                public long Total_Income { get; set; }
                [Required(ErrorMessage = "Enter plant grass quantity")]
               
                public long Benefits_Plant_Grass_Quantity { get; set; }
                [Required(ErrorMessage = "Enter plant grass amount")]
              
                public long Benefits_Plant_Grass_Amount { get; set; }
                [Required(ErrorMessage = "Enter forest produce quantity")]
              
                public long Minor_Forest_Produce_Quantity { get; set; }
                [Required(ErrorMessage = "Enter minor forest produce amount")]
              
                public long Minor_Forest_Produce_Amount { get; set; }
                [Required(ErrorMessage = "Enter other forest produce quantity")]
              
                public long Other_ForestProduce_Quantity { get; set; }
                [Required(ErrorMessage = "Enter other forest produce amount")]
            
                public long Other_ForestProduce_Amount { get; set; }
                [Required(ErrorMessage = "Enter No. of beneficiaries last three year")]
              
                public long No_Of_Beneficiaries_LastThreeYear { get; set; }
                [Required(ErrorMessage = "Enter No. of beneficiaries current year")]
            
                public long No_Of_Beneficiaries_CurrentYear { get; set; }
                [Required(ErrorMessage = "Enter grades")]
                public string Efficient_Grades { get; set; }               
                public string No_Of_NGO_Working { get; set; }
                [Required(ErrorMessage = "Enter self help group mem")]
             
                public int Self_Help_Group_Men { get; set; }
                [Required(ErrorMessage = "Enter self help group womem")]
             
                public int Self_Help_Group_Women { get; set; }               
                public string Self_Help_Group_Others { get; set; }                
                public int Self_Help_Group_Total { get; set; }
                [Required(ErrorMessage = "Enter remarks")]
                public string Overall_Remarks { get; set; }     
                public long EnterBy { get; set; }     
                public long UpdatedBy { get; set; }
                public string RefGisID { get; set; }  
        #endregion

        #region Insert Function
                /// <summary>
                /// Function to insert JFMC Registration
                /// </summary>
                /// <returns></returns>
                public Int64 InsertJFMC(StakeholderServices stk)
                {
                    try
                    {
                        DALConn();
                        SqlParameter[] parameters =
                        {  
                        new SqlParameter("@ID",stk.Id),   
                        new SqlParameter("@Dist_Code",stk.Dist_Code),           
                        new SqlParameter("@Blk_Code", stk.Blk_Code),
                        new SqlParameter("@Gp_Code", stk.Gp_Code),      
                        new SqlParameter("@Vill_Code", stk.Vill_Code),    
                        new SqlParameter("@Minutes_of_Meeting", stk.Minutes_of_Meeting),
                        new SqlParameter("@Upload_MOM",stk.Upload_MOM),
                        new SqlParameter("@Committee_Name",stk.Committee_Name),
                        new SqlParameter("@Registration_Id", stk.Registration_Id),
                        new SqlParameter("@Secretary_Name", stk.Secretary_Name),
                           new SqlParameter("@Secretary_ssoid", stk.Ssoid),
                        new SqlParameter("@Lattitude", stk.Lattitude),
                        new SqlParameter("@Longitude", stk.Longitude),
                        new SqlParameter("@Plantation_Area", stk.Plantation_Area),
                        new SqlParameter("@Total_Area_Managed", stk.Total_Area_Managed),
                        new SqlParameter("@Bank_Account_Operated", stk.Bank_Account_Operated),
                        new SqlParameter("@Bank_Name", stk.Bank_Name),
                        new SqlParameter("@Branch_Name", stk.Branch_Name),
                        new SqlParameter("@Branch_Id", stk.Branch_Id),
                        new SqlParameter("@Account_No", Convert.ToInt64(stk.Account_No)),
                        new SqlParameter("@Account_Type", stk.Account_Type),
                        new SqlParameter("@MembershipFees", stk.MembershipFees),
                        new SqlParameter("@Income_Deposit_ForestOffice", stk.Income_Deposit_ForestOffice),
                        new SqlParameter("@Income_Sale_ForestProduce", stk.Income_Sale_ForestProduce),
                          new SqlParameter("@Income_Generated_Activity", stk.Income_Generated_Activity),
                        new SqlParameter("@Other_Sources", Convert.ToInt64(stk.Other_Sources)),
                        new SqlParameter("@Total_Income", stk.Total_Income),
                        new SqlParameter("@Benefits_Plant_Grass_Quantity", stk.Benefits_Plant_Grass_Quantity),
                        new SqlParameter("@Benefits_Plant_Grass_Amount", stk.Benefits_Plant_Grass_Amount),
                        new SqlParameter("@Minor_Forest_Produce_Quantity", stk.Minor_Forest_Produce_Quantity),
                        new SqlParameter("@Minor_Forest_Produce_Amount",stk. Minor_Forest_Produce_Amount),
                        new SqlParameter("@Other_ForestProduce_Quantity", stk.Other_ForestProduce_Quantity),
                        new SqlParameter("@Other_ForestProduce_Amount", stk.Other_ForestProduce_Amount),
                        new SqlParameter("@No_Of_Beneficiaries_LastThreeYear", stk.No_Of_Beneficiaries_LastThreeYear),
                        new SqlParameter("@No_Of_Beneficiaries_CurrentYear", stk.No_Of_Beneficiaries_CurrentYear),
                        new SqlParameter("@Efficient_Grades", stk.Efficient_Grades),
                        new SqlParameter("@No_Of_NGO_Working",stk.No_Of_NGO_Working),
                        new SqlParameter("@Self_Help_Group_Men",stk.Self_Help_Group_Men),
                        new SqlParameter("@Self_Help_Group_Women", stk.Self_Help_Group_Women),
                        new SqlParameter("@Self_Help_Group_Others", Convert.ToInt32(stk.Self_Help_Group_Others)),
                        new SqlParameter("@Self_Help_Group_Total", stk.Self_Help_Group_Total),
                        new SqlParameter("@Overall_Remarks", stk.Overall_Remarks),
                        new SqlParameter("@EnterBy", Convert.ToInt64(HttpContext.Current.Session["UserId"])),
                          new SqlParameter("@RefGisID", Convert.ToInt64(stk.RefGisID)), // Added By Arvind Kumar Sharam for save GIS Ref ID
                        };
                        Int64 chk = Convert.ToInt64(ExecuteScalar("Sp_InsertJfmcRegist", parameters));
                        return chk;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        Conn.Close();
                    }

                }
                #endregion

        #endregion

    }

    public class GISactivityData
    {
        public string village_NM { get; set; }
        public string DrawArea { get; set; }
        public string DrawLength { get; set; }
        public string PlantationArea { get; set; }
        public string PlantationLength { get; set; }
        public string Cordinates { get; set; }
        public string refGisId { get; set; }

    }


    public class GISpostbackData
    {
        public string DistrictID { get; set; }
        public string BlocknameID { get; set; }
        public string GPNameID { get; set; }
        public string VillageID { get; set; }
    }


}