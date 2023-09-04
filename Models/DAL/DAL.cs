//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL 
//  File         : Data Connection Class
//  Description  : File contains functions for communicating with DB
//  Date Created : 24-Dec-2015
//  History      : 
//  Version      : 1.0
//  Author       : Vandana Gupta
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.Runtime.Serialization;
//using System.Runtime.Serialization;

namespace FMDSS.Models
{
       [Serializable] 
    public class DAL : IDeserializationCallback
    {
        void IDeserializationCallback.OnDeserialization(object sender)
        {
            Conn = new SqlConnection(connection);
            //recreate your connection here

        }
        #region Data Members
        [JsonIgnore]
        private string connection = ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString.ToString();
        [NonSerialized]
        public SqlConnection Conn;

        #endregion

        #region Member Functions
        public void DALConn()
        { 
           
           Conn = new SqlConnection(connection);
            if (Conn.State == ConnectionState.Closed)
            {
            
                Conn.Open();
            }

 
        
        }

        //public  DAL()
        //{

        //    Conn = new SqlConnection(connection);
        //    if (Conn.State == ConnectionState.Closed)
        //    {

        //        Conn.Open();
        //    }


        //    //    Conn.Close();
        //    ////    SqlConnection.ClearPool(Conn);
        //    //    Conn.Open();

        //}
        
        #region "FILL DATA TABLE"

        public void Fill(DataTable dataTable, String procedureName)
        {
           SqlConnection oConnection = new SqlConnection(connection);
     
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
            oCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter oAdapter = new SqlDataAdapter();

            oAdapter.SelectCommand = oCommand;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    //DALConn();
                    oAdapter.SelectCommand.Transaction = oTransaction;
                    oAdapter.Fill(dataTable);
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
                    oConnection.Dispose();
                    SqlConnection.ClearPool(Conn);
                    oAdapter.Dispose();
                }
            }
        }

        public void Fill(DataTable dataTable, String procedureName, SqlParameter[] parameters)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
            oCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                oCommand.Parameters.AddRange(parameters);

            SqlDataAdapter oAdapter = new SqlDataAdapter();

            oAdapter.SelectCommand = oCommand;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
                    oAdapter.SelectCommand.Transaction = oTransaction;
                    oAdapter.Fill(dataTable);
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
        }
        public void Fill_WithoutCommit(DataTable dataTable, String procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection oConnection = new SqlConnection(connection))
            {
                using (SqlCommand oCommand = new SqlCommand(procedureName, oConnection))
                {
                    oCommand.CommandType = CommandType.StoredProcedure;                  
                    if (parameters != null)
                        oCommand.Parameters.AddRange(parameters);

                    SqlDataAdapter oAdapter = new SqlDataAdapter();

                    oAdapter.SelectCommand = oCommand;
                    oConnection.Open();
                    try
                    {
                        oAdapter.Fill(dataTable);
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine(se.Message.ToString());
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());                       
                        throw;
                    }
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message.ToString());
                    //    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, procedureName + "_" + ex.Message.ToString(), 0, DateTime.Now, 0);
                    //    throw;
                    //}
                    finally
                    {
                        if (oConnection.State == ConnectionState.Open)
                            oConnection.Close();
                        SqlConnection.ClearPool(oConnection);
                        oConnection.Dispose();
                        oAdapter.Dispose();

                    }
                }
            }
        }
        public void Fill_WithoutCommit2(DataSet dataTable, String procedureName, SqlParameter[] parameters)
        {
            using (SqlConnection oConnection = new SqlConnection(connection))
            {
                using (SqlCommand oCommand = new SqlCommand(procedureName, oConnection))
                {
                    oCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                        oCommand.Parameters.AddRange(parameters);

                    SqlDataAdapter oAdapter = new SqlDataAdapter();

                    oAdapter.SelectCommand = oCommand;
                    oConnection.Open();
                    try
                    {
                        oAdapter.Fill(dataTable);
                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine(se.Message.ToString());
                        throw;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        throw;
                    }
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message.ToString());
                    //    new Common().ErrorLog(ex.InnerException + "_" + ex.StackTrace, procedureName + "_" + ex.Message.ToString(), 0, DateTime.Now, 0);
                    //    throw;
                    //}
                    finally
                    {
                        if (oConnection.State == ConnectionState.Open)
                            oConnection.Close();
                        SqlConnection.ClearPool(oConnection);
                        oConnection.Dispose();
                        oAdapter.Dispose();

                    }
                }
            }
        }
        #endregion

        #region "FILL DATASET"

        public void Fill(DataSet dataSet, String procedureName)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
            oCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter oAdapter = new SqlDataAdapter();

            oAdapter.SelectCommand = oCommand;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
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
        }

        public void Fill(DataSet dataSet, String procedureName, SqlParameter[] parameters)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);
            oCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                oCommand.Parameters.AddRange(parameters);

            SqlDataAdapter oAdapter = new SqlDataAdapter();

            oAdapter.SelectCommand = oCommand;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
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
                    oConnection.Dispose();
                    oAdapter.Dispose();
                }
            }
        }

        #endregion

        #region "EXECUTE SCALAR"

        public object ExecuteScalar(String procedureName)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);

            oCommand.CommandType = CommandType.StoredProcedure;
            object oReturnValue;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
                    oCommand.Transaction = oTransaction;
                    oReturnValue = oCommand.ExecuteScalar();
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
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return oReturnValue;
        }

        public object ExecuteScalar(String procedureName, SqlParameter[] parameters)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);

            oCommand.CommandType = CommandType.StoredProcedure;
            object oReturnValue;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
                    if (parameters != null)
                        oCommand.Parameters.AddRange(parameters);

                    oCommand.Transaction = oTransaction;
                    oReturnValue = oCommand.ExecuteScalar();
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
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return oReturnValue;
        }

        #endregion

        #region "EXECUTE NON QUERY"

        public int ExecuteNonQuery(string procedureName)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);

            oCommand.CommandType = CommandType.StoredProcedure;
            int iReturnValue;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
                    oCommand.Transaction = oTransaction;
                    iReturnValue = oCommand.ExecuteNonQuery();
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
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return iReturnValue;
        }

        public int ExecuteNonQuery(string procedureName, SqlParameter[] parameters)
        {
            SqlConnection oConnection = new SqlConnection(connection);
            SqlCommand oCommand = new SqlCommand(procedureName, oConnection);

            oCommand.CommandType = CommandType.StoredProcedure;
            int iReturnValue;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    DALConn();
                    if (parameters != null)
                        oCommand.Parameters.AddRange(parameters);

                    oCommand.Transaction = oTransaction;
                    iReturnValue = oCommand.ExecuteNonQuery();
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
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return iReturnValue;
        }

        #endregion

        #endregion

       // public string password = emailId.tos
    }
}
