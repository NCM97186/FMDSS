using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.MIS
{
  

    public class MISEncroachmentDetails : DAL
    {
        public int Index { get; set; }

        public string RANGE_NAME { get; set; }
        public string HIERARCHY_CODE { get; set; }
        public string RANGE_CODE { get; set; }
        public string EN_Code { get; set; }
        public string NoticeNo { get; set; }
        public string DIV_NAME { get; set; }
        public string NameAddress { get; set; }
        public string Year { get; set; }
        public string Block_Name { get; set; }
        public string khasraNo { get; set; }
        public string EncrochArea { get; set; }
        public string PaidawarOrKISMA { get; set; }

        public string TaxPerHact { get; set; }
        public string EncrochedArea { get; set; }
        public string EncrochedPaidawar { get; set; }
        public string TotalTax { get; set; }
        public string V_V { get; set; }

        public string Circle { get; set; }
        public string Division { get; set; }
        public string Range { get; set; }

        public string CircleStatus { get; set; }
        public string DivisionStatus { get; set; }
        public string RangeStatus { get; set; }

        public string Status { get; set; }

        public string CompartmentNo { get; set; }
       
        public string LRACTNO { get; set; }
        public string KMLFile { get; set; }
        public string DateOfKMLFile { get; set; }
        public string InformationGatheredBy { get; set; }
        public string InformationApprovedBy { get; set; }

        
        public List<SelectListItem> HIERARCHY_LEVEL_CODE { get; set; }


        public DataSet GET_EncroachmentDetails(string CODE, string Status, string Action)
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_MIS_GetEncroachmentDetails", Conn);
                cmd.Parameters.AddWithValue("@CODE", CODE);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Action", Action);
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 500000000;
                da.Fill(dt);
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



    }

}