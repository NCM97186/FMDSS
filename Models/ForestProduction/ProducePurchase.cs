//********************************************************************************************************
//  Project      : FMDSS
//  Project Code : Project Code:	IEISLSSD-2015-16-ENV-004
//  Copyright (C): IEISL
//  File         : UI 
//  Description  : This file is responsible for data and business rule for Asset Management
//  Date Created : 14-Jan-2016
//  History      :
//  Version      : 1.0
//  Author       : Manoj Kumar
//  Modified By  : Manoj Kumar 
//  Modified On  : 06-Feb-2016
//  Reviewed By  : 
//  Reviewed On  : 
//*********************************************************************************************************@

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.Models.ForestProduction
{
    public class ProducePurchase : DAL
    {
        #region global variables
        public Int64 RowID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 CartID { get; set; }
        public Int64 StockID { get; set; }
        public string RequestID { get; set; }
        public string ProduceTypeID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductTypeName { get; set; }

        public string ReservedQty { get; set; }
        public string NurseryName { get; set; }
        public Int64 Quantity { get; set; } //8235-200-06-RFBP Head

        public Int64 Quantity1 { get; set; } //0406-01-80-05 Head
        public string NurseryDiscountDocument { get; set; }
        public string DiscountType { get; set; }

        public string QuantityByUnit { get; set; }
        public decimal RatePerUnit { get; set; }
        public decimal Discount { get; set; }
        public string UnitType { get; set; }
        public string DistrictName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string TransactionID { get; set; }
        public int Trn_Status_Code { get; set; }
        public Int64 StockQuantity { get; set; }
        public string ProduceFor { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal BeforeDiscount { get; set; }

        public decimal AmountToBePaid { get; set; }
        public decimal FinalAmount { get; set; }
        public string AvailStatus { get; set; }
        public Int16 isdiscountApplicable { get; set; }

        public decimal Amount { get; set; }

        public string SSOID { get; set; }
        public int DiscountTypeID { get; set; }

        public char IsInChargeOrCitizen { get; set; }

        public long TotalInventoryQTY { get; set; }

        public string EmitraTransactionID { get; set; }

        public string ProductThumbImage { get; set; }

        public string ProductFullImage { get; set; }

        #endregion

        #region Member Function


        public DataSet Select_order_For_Print(string RequestID)
        {
            DataSet ds = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters = 
            {    
            new SqlParameter("@RequestID", RequestID)
            };
                Fill(ds, "SP_Citizen_PurchaseProduceNurseryDetails", parameters);

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, "Select_TicketData_For_Print" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return ds;
        }










        /// <summary>
        /// Used for Select Produce Type
        /// </summary>
        /// <returns></returns>
        public DataSet Select_ProduceType(Int64 UserID, char IsInchargeOrAsCitizenUser = 'C')
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlParameter[] parameters =
            {    
                new SqlParameter("@ProduceFor", ProduceFor),
                new SqlParameter("@userid", UserID),
                 new SqlParameter("@IsInchargeOrAsCitizenUser", IsInchargeOrAsCitizenUser),
                
            };
                Fill(dt, "Sp_Comman_Select_NurseryDist", parameters);

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

        public DataSet Select_UserTotalCartItem(char IsInchargeOrCitizen = 'C', char IsCitizenOrDeptUserQTY = 'C')
        {
            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID),
            new SqlParameter("IsInchargeOrCitizen", IsInchargeOrCitizen),
            new SqlParameter("IsCitizenOrDeptUserQTY", IsCitizenOrDeptUserQTY)
            };
                Fill(dt, "sp_Fpd_Select_UserTotalCartItems", parameters);
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


        public int GetNurseryInchargeOrNot(long UserID)
        {
            int count = 0;
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "SP_GetNurseryInchargeOrNot", parameters);
                if (dt != null)
                {
                    count = Convert.ToInt32(dt.Rows[0][0]);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return count;
        }

        /// <summary>
        /// Used for Select Produce Product by TypeID
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Product(Int64 producetypeID, Int64 UserID, char IsInchargeOrCitizen = 'C')
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
                   {    
                    new SqlParameter("@ProduceTypeID", producetypeID),
                    new SqlParameter("@UserID", UserID),
                    new SqlParameter("IsInchargeOrCitizen", IsInchargeOrCitizen)
                   };
                Fill(dt, "Sp_Comman_Select_NurserysByDist", parameters);
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
        /// Used for Add data to cart table into database
        /// </summary>
        /// <returns></returns>
        public DataSet AddToCart(char IsInchargeOrAsCitizenUser = 'C')
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlParameter[] parameters =
                   {    
                    new SqlParameter("@StockID", StockID == null ? (object)DBNull.Value : StockID),
                    new SqlParameter("@Quantity", Quantity == null ? (object)DBNull.Value : Quantity),
                    new SqlParameter("@UserID", UserID == null ? (object)DBNull.Value : UserID)
                   };
                if (IsInchargeOrAsCitizenUser == 'C')
                    Fill(ds, "sp_FPD_Add_ProductToCart", parameters);
                else
                    Fill(ds, "sp_FPD_Add_ProductToCartForInchargeAsCitizen", parameters);

                return ds;
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


        #region Dept User QTY Developed by Rajveer
        public DataSet AddToCartDeptUser()
        {
            try
            {
                DALConn();
                DataSet ds = new DataSet();
                SqlParameter[] parameters =
                   {
                    new SqlParameter("@StockID", StockID == null ? (object)DBNull.Value : StockID),
                    new SqlParameter("@Quantity", Quantity == null ? (object)DBNull.Value : Quantity),
                    new SqlParameter("@UserID", UserID == null ? (object)DBNull.Value : UserID)
                   };
                Fill(ds, "sp_FPD_Add_ProductToCartForDeptUser", parameters);
                return ds;
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


        /// <summary>
        /// Select Item added to cart by user ID
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Cart_Items(char IsInChargeOrCitizen = 'C')
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID),
            new SqlParameter("@IsInChargeOrCitizen", @IsInChargeOrCitizen)
            };
                Fill(dt, "sp_Fpd_Select_CartItems", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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


        public DataTable Select_Cart_ItemsDeptUsers()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "sp_Fpd_Select_CartItemsDeptUsers", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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

        public DataTable Select_OnlinePurchaseHistory()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
                
                new SqlParameter("@FLAG", "HISTORTY"),
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "MIS_OnlinePurchaseHistory", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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


        public DataTable Select_OnlinePurchaseHistoryDashboard()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
                
                new SqlParameter("@FLAG", "HISTORTYDashboard"),
            new SqlParameter("@userID", UserID)
            };
                Fill(dt, "MIS_OnlinePurchaseHistory", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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
        /// Used to delete data from user cart
        /// </summary>
        /// <returns></returns>
        public DataTable DeleteCartItem()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
                   {    
                    new SqlParameter("@CartID", CartID == null ? (object)DBNull.Value : CartID),
                    new SqlParameter("@UserID", UserID == null ? (object)DBNull.Value : UserID)
                   };
                Fill(dt, "sp_FPD_deleteCartItem", parameters);
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
        /// Get Nursery list with the products
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Product_NurseryWise()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@ProduceTypeID", ProduceTypeID),
            new SqlParameter("@ProductID", ProductID),
            new SqlParameter("@userID", UserID),
            new SqlParameter("@ProduceFor", ProduceFor)
            };
                Fill(dt, "Sp_FPD_SelectProductNurserywise", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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


        public DataTable FatchReservedPurchaseDetails(string Action)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@Action", Action),
            new SqlParameter("@NurseryCode", NurseryName),
            new SqlParameter("@ProductID", ProductID),
            new SqlParameter("@PurchaseID", ProduceTypeID)
            };
                Fill(dt, "SP_FatchReservedPurchaseDetails", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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
        /// Update transaction status to database if payment done
        /// </summary>
        /// <param name="cartIDs"></param>
        public void UpdateTransactionStatus(string cartIDs, char IsInChargeOrCitizen = 'C', int EmitraCharges = 0)
        {
            DALConn();
            SqlParameter[] parameters =
            {              
            new SqlParameter("@cartIDs",cartIDs),  
            new SqlParameter("@TransactionID",TransactionID),
            new SqlParameter("@TransactionStatus", Trn_Status_Code),
            new SqlParameter("@RequestID", RequestID),
            new SqlParameter("@userID", UserID),
            new SqlParameter("@IsInChargeOrCitizen", IsInChargeOrCitizen),
             new SqlParameter("@EmitraCharges", EmitraCharges)
            };
            Int32 chk = Convert.ToInt32(ExecuteNonQuery("sp_FPD_UpdatePurchaseStatus", parameters));
        }

        /// <summary>
        /// Used for get nursery by village id 
        /// </summary>
        /// <returns></returns>
        public DataTable Select_Nursery(string VlgCode)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlParameter[] parameters =
            {    
            new SqlParameter("@VlgCode", VlgCode)
            };
                Fill(dt, "sp_common_Select_Nursery", parameters);

                // Fill(dt, "sp_common_Select_Nursery");
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







        public DataTable GetNurseryDiscount()
        {
            try
            {
                DALConn();
                DataTable ds = new DataTable();
                SqlParameter[] parameters =
                   {    
                    new SqlParameter("@Action", "NurseryDiscountList"),
                  
                    new SqlParameter("@UserID", UserID == null ? (object)DBNull.Value : UserID)
                   };
                Fill(ds, "SP_NurseryDiscount", parameters);
                return ds;
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

        public DataTable SubmitNurseryDiscount(DataTable DT, string PATH, char IsInChargeOrCitizen = 'C')
        {
            try
            {
                DALConn();
                DataTable ds = new DataTable();
                SqlParameter[] parameters =
                   {    
                    new SqlParameter("@Action", "UpdateNurseryDiscount"),
                    new SqlParameter("@DataTable", DT),                    
                    new SqlParameter("@UserID", UserID == null ? (object)DBNull.Value : UserID),
                    new SqlParameter("@UploadNurseryDiscountDocument", PATH),
                     new SqlParameter("@IsInChargeOrCitizen", IsInChargeOrCitizen)
                   };
                Fill(ds, "SP_NurseryDiscount", parameters);
                return ds;
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


        public string PrintOrder(string RequestID, long UserID, bool NurseryIncharge)
        {
            #region Create PDF
            StringBuilder sb = new StringBuilder();
            DataTable DT1 = new DataTable();
            DataTable DT2 = new DataTable();
            DataTable DT3 = new DataTable();
            string actionName = "PrintOrder";
            string controllerName = "ProducePurchase";

            try
            {
                DataSet ds = new DataSet();

                if (RequestID.IndexOf('/') > 0)
                {
                    ds = Select_order_For_Print(RequestID.Split('/')[3]);
                }
                else
                {
                    ds = Select_order_For_Print(RequestID);
                }
                DT1 = ds.Tables[0];
                DT2 = ds.Tables[1];
                DT3 = ds.Tables[2];

                sb.Append("<section class='print-invoice'> <div class='row border-divider'><div class='col-xs-12 col-sm-4' style='padding: 0'><img src='../images/risl-logo-small.png' alt='RISL' ></div><div class='col-xs-12 col-sm-4 centr'>Department of Forest, <br>Government of<br> Rajasthan</span></div><div class='col-xs-12 col-sm-4' style='padding: 0'> <span class='pull-right pdate'><img src='../images/e-mitra_logo.png' alt='E-Mitra' > </div>  <div class='divider'></div></div>");
                if (ds != null)
                {

                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr> <td col-lg-12 colspan='5' style='text-align:center'>ORDER CONFIRMATION SLIP</td></tr>");
                    sb.Append("<tr><td col-lg-3><b>Nursery : </b></td><td col-lg-3>" + DT1.Rows[0]["NURSERY_NAME"] + "</td><td col-lg-3><b>Order No.:</b> </td><td col-lg-3>" + DT1.Rows[0]["RequestID"] + " </td></tr>");

                    sb.Append("<tr><td col-lg-3><b>User Name : </b></td><td col-lg-3>" + DT1.Rows[0]["UserName"] + "</td><td col-lg-3><b>Mobile No.:</b> </td><td col-lg-3>" + DT1.Rows[0]["UserMobileNo"] + " </td></tr>");

                    sb.Append("<tr><td col-lg-3><b>Email Id : </b></td><td col-lg-3>" + DT1.Rows[0]["UserEmailId"] + "</td><td col-lg-3><b>Address :</b> </td><td col-lg-3>" + DT1.Rows[0]["UserAddress"] + " </td></tr>");

                    sb.Append("<tr><td col-lg-3><b>Date/Time of Order : </b></td><td col-lg-3 colspan='3' >" + DT1.Rows[0]["ORERDDATE"] + "</td> </tr>");

                    if (Convert.ToBoolean(NurseryIncharge == false) && DT1.Columns.Contains("Name"))
                    {
                        sb.Append("<tr><td col-lg-3><b>Buyer Name  : </b></td><td col-lg-3 colspan='3' >" + DT1.Rows[0]["Name"] + "</td> </tr>");
                    }

                    sb.Append("</table>");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-3><b>S.No</b></td><td col-lg-3><b>Product Type</b></td><td col-lg-3><b>Product Name </b></td><td col-lg-3><b>Unit</b></td><td col-lg-3><b>Rate / Unit</b></td><td col-lg-3><b>Quantity</b></td><td col-lg-3><b>Total Amount</b></td></td><td col-lg-3><b>Discount Amount (INR)</b></td><td col-lg-3><b>Payable Amount (INR)</b></td></tr>");


                    #region Check Product type not repected
                    List<string> ContainProductType = new List<string>();
                    string j = "0";
                    int k = 0;
                    #endregion
                    for (int i = 0; i < DT2.Rows.Count; i++)
                    {


                        #region Check Product type not repected
                        string ProductTypeName = string.Empty;
                        if (ContainProductType.Contains(Convert.ToString(DT2.Rows[i]["ProduceType"])))
                        {
                            j = string.Empty;
                        }
                        else
                        {
                            ContainProductType.Add(Convert.ToString(DT2.Rows[i]["ProduceType"]));
                            ProductTypeName = Convert.ToString(DT2.Rows[i]["ProduceType"]);
                            k = j == "0" ? 1 : k + 1;
                            j = k.ToString();
                            j = (Convert.ToInt16(j)).ToString();
                        }
                        #endregion

                        sb.Append("<tr><td col-lg-3>" + j + "</td><td col-lg-3>" + ProductTypeName + "</td><td col-lg-3>" + DT2.Rows[i]["ProductName"] + "</td><td col-lg-3>" + DT2.Rows[i]["Unit"] + "</td>  <td col-lg-3>" + DT2.Rows[i]["RatePerItem"] + "</td> <td col-lg-3>" + DT2.Rows[i]["PurchaseQuantity"] + "</td><td col-lg-3>" + DT2.Rows[i]["TotalAmount"] + "</td><td col-lg-3>" + DT2.Rows[i]["Discount"] + "</td><td col-lg-3>" + DT2.Rows[i]["PaidAmount"] + "</td></tr>");
                    }
                    sb.Append("</table>");

                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-4 style='text-align:right;' ><b>Total Amount </b></td><td col-lg-3 colspan='5' style='text-align:right;' >" + Convert.ToString(DT3.Rows[0]["TotalAmount"]) + "</td> </tr>");
                    sb.Append("<tr><td col-lg-4 style='text-align:right;'><b> RISL Charges (INR) : </b></td><td col-lg-3 colspan='5' style='text-align:right;' >" + DT3.Rows[0]["ServiceCharges"] + "</td> </tr>");
                    sb.Append("<tr><td col-lg-4 style='text-align:right;'><b> Discount Amount (INR) : </b></td><td col-lg-3 colspan='5' style='text-align:right;'>" + DT3.Rows[0]["DiscountAmount"] + "</td> </tr>");
                    sb.Append("<tr><td col-lg-4 style='text-align:right;'><b> Total Payable Amount (INR) : </b></td><td col-lg-3 colspan='5' style='text-align:right;' >" + DT3.Rows[0]["PaidAmount"] + "</td> </tr>");

                    sb.Append("</table>");
                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr> <td col-lg-3><b>Nursery Address : </b></td><td col-lg-10 colspan='4' style='text-align:center'>" + DT1.Rows[0]["NURSERY_ADRESS"] + "</td></tr>");
                    sb.Append("</table>");

                    sb.Append("<table class='table table-bordered' id='tkt'>");
                    sb.Append("<tr><td col-lg-3  colspan='2' >Terms and conditions :</td> </tr>");



                    sb.Append("<tr><td col-lg-3 >1.</td><td col-lg-3>Nursery Product should be collected within 5 days from order place. </td> </tr>");
                    sb.Append("</table>");

                    //   htmlToPdfDownloadTickets.WildlifeDownloadPdf(ds);
                }


                sb.Append("</div></section></div>");

            }
            catch (Exception ex)
            {
                new Common().ErrorLog(ex.Message, actionName + "_" + controllerName, 1, DateTime.Now, UserID);
            }
            #endregion
            return sb.ToString();
        }


        #endregion
    }
}