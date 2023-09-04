using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FMDSS.Models.CitizenService.ProductionServices
{
    public class BuyerObject:DAL
    {

        #region data members
        public Int64 RowID { get; set; }
        public Int64 BuyerID { get; set; }
        public string RequestedId { get; set; }
        public string IsTendupatta { get; set; }
        public string DivisionCode { get; set; }
        public string RangeCode { get; set; }
        public Int64 DepotId { get; set; }
        public string DepotName { get; set; }
        public Int64 ForestProduceID { get; set; }
        public string ForestProducename { get; set; }
        public Int64 ForestProductID { get; set; }
        public string ForestProductName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DurationFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime DurationTo { get; set; }

        public int IsActive { get; set; }
        public Int64 EnteredBy { get; set; }

        public string IDProof { get; set; }
        public string IDProofPath { get; set; }
  

        #endregion

        #region Member Functions

        /// <summary>
        /// function responsible for add/edit nursery details
        /// </summary>
        /// <param name="actionFlag"></param>
        /// <returns>scope identity returned from DB</returns>
        public Int64 InsertBuyerDetails()
        {

            Int64 result = 0;
            try
            {
                DALConn();
                SqlCommand cmd = new SqlCommand("SP_Citizen_AddBuyers", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BuyerID", BuyerID);
                cmd.Parameters.AddWithValue("@RequestedId", RequestedId);
                cmd.Parameters.AddWithValue("@RequestedFor", IsTendupatta);
                cmd.Parameters.AddWithValue("@DivisionCode", DivisionCode);
                cmd.Parameters.AddWithValue("@RangeCode", RangeCode);
                cmd.Parameters.AddWithValue("@DepotId", DepotId);
                cmd.Parameters.AddWithValue("@DurationFrom", DurationFrom);
                cmd.Parameters.AddWithValue("@DurationTo", DurationTo);
                cmd.Parameters.AddWithValue("@IDProof", IDProof);
                cmd.Parameters.AddWithValue("@IDProofPath", IDProofPath);
                cmd.Parameters.AddWithValue("@EnteredBy", Convert.ToString(HttpContext.Current.Session["UserId"]));
                cmd.CommandType = CommandType.StoredProcedure;
                result =Convert.ToInt64(cmd.ExecuteScalar().ToString());

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "InsertBuyerDetails" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));


            }
            finally
            {
                Conn.Close();
            }
            return result;
        }


        #endregion


  
    }
}