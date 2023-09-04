using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FMDSS.ZooTicketReport
{
    public partial class ZooTicketView : System.Web.UI.Page
    {
        public string connection = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
        public SqlConnection Conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
            ReportViewer1.LocalReport.DataSources.Clear();
            DataSet dataSet = new DataSet();
            //SqlParameter sqlParm = new SqlParameter("@TicketID", 125.ToString());


            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand("Sp_Zoo_SelecTicketDetailKIOSK", oConnection);
            oCommand.CommandType = CommandType.StoredProcedure;
            string RequestID = Encryption.decrypt(Request.QueryString["ID"]);



            oCommand.Parameters.AddWithValue("@TicketID", RequestID);

            
            SqlDataAdapter oAdapter = new SqlDataAdapter();
            oAdapter.SelectCommand = oCommand;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {

                    oAdapter.SelectCommand.Transaction = oTransaction;

                    oAdapter.Fill(dataSet);
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    SqlConnection.ClearPool(oConnection);
                    oConnection.Dispose();
                    oAdapter.Dispose();
                }
            }


           ReportDataSource RDS1 = new Microsoft.Reporting.WebForms.ReportDataSource();


           //set Processing Mode of Report as Local  
           ReportViewer1.ProcessingMode = ProcessingMode.Local;
           //set path of the Local report  
           ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ZooTicketReport/Report1.rdlc");
          
            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("HEADERFOOTER", dataSet.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("TRNDATA", dataSet.Tables[1]));
            //ReportViewer1.LocalReport.Refresh();


            }


        }
    }
}