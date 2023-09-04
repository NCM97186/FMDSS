using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models
{

    public class ElephantMovement : DAL
    {
        public Int64? ElephantMovementID { get; set; }
        public string RequestID { get; set; }

        [Required]
        [Display(Name = "From State")]
        public int FromStateId { get; set; }

        [Required]
        [Display(Name = "From District")]
        public int FromDistrictId { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "From Date")]
        public string FromDate { get; set; }

        [Required]
        [Display(Name = "To State")]
        public int ToStateId { get; set; }

        [Required]
        [Display(Name = "To District")]
        public int ToDistrictId { get; set; }
        [StringLength(10)]
        [Required]
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "Return To Date")]
        public string ReturnToDate { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "Return From Date")]
        public string ReturnFromDate { get; set; }


        [Required]
        [Display(Name = "Select Elephant")]
        public int ElephantId { get; set; }

        [Required]
        [Display(Name = "Select Transport")]
        public int TransportId { get; set; }

        [StringLength(40)]
        [Required]
        [Display(Name = "Medical Certificate Number")]
        public string MedicalCertificateNumber { get; set; }

        [Required]
        [Display(Name = "Medical Certificate DOC")]
        public string MedicalCertificateDOC { get; set; }

        [Required]
        [Display(Name = "Movement Recommendation Latter Of CWLW or DCF")]
        public string MovementRecommendationLatter { get; set; }


        [StringLength(50)]
        [Display(Name = "Other State Transit Permit Number")]
        public string OtherStateTPNumber { get; set; }


        [Display(Name = "Other State Noc Doc")]
        public string OtherStateNocDoc { get; set; }


        [StringLength(10)]
        [Display(Name = "Transit Permit Date")]
        public string TPDate { get; set; }

        [StringLength(10)]
        [Display(Name = "Other State Contact No")]
        public string OtherStateContactNo { get; set; }


        [Display(Name = "Area DCF Officer")]
        public string RajasthanAreaDCF { get; set; }
        public List<SelectListItem> ListRajasthanAreaDCF { get; set; }


        public Int64? UserID { get; set; }

        public string MovementFrom { get; set; }
        public string MovementTo { get; set; }
        public string MovementStatus { get; set; }

        public Int64? AssignTo { get; set; }
        public string ActionStatus { get; set; }
        public string Comment { get; set; }

        public string ActionTakenBy { get; set; }
        public string ActionTakenOn { get; set; }
        public Int64? Index { get; set; }
        public List<SelectListItem> ListRejectedReason { get; set; }
        public int[] RejectedReason { get; set; }
        public string Reasons { get; set; }
        public string ElephantName { get; set; }
        public string TransportType { get; set; }



        public DataTable GetAllState()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Select_State");
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable GetAllDistrict(int StateId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "Selec_District");
                cmd.Parameters.AddWithValue("@FromStateId", StateId);
                cmd.CommandType = CommandType.StoredProcedure;
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


        public DataTable GetElephants(long UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectUserWiseElephantInfo");
                cmd.Parameters.AddWithValue("@EnteredBy", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetElephantTransport(long UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectElephantTransport");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
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


        public DataTable SaveElephantMovement(ElephantMovement ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Insert_Elephant_Movement");
                cmd.Parameters.AddWithValue("@FromStateId", ele.FromStateId);
                cmd.Parameters.AddWithValue("@FromDistrictId", ele.FromDistrictId);
                cmd.Parameters.AddWithValue("@FromDate", ele.FromDate);
                cmd.Parameters.AddWithValue("@ToStateId", ele.ToStateId);
                cmd.Parameters.AddWithValue("@ToDistrictId", ele.ToDistrictId);

                cmd.Parameters.AddWithValue("@ToDate", ele.ToDate);
                cmd.Parameters.AddWithValue("@ReturnFromDate", ele.ReturnFromDate);
                cmd.Parameters.AddWithValue("@ReturnToDate", ele.ReturnToDate);
                cmd.Parameters.AddWithValue("@ElephantId", ele.ElephantId);
                cmd.Parameters.AddWithValue("@TransportId", ele.TransportId);

                cmd.Parameters.AddWithValue("@OtherStateTPNumber", ele.OtherStateTPNumber);
                cmd.Parameters.AddWithValue("@OtherStateNocDoc", ele.OtherStateNocDoc);
                cmd.Parameters.AddWithValue("@TPDate", ele.TPDate);
                cmd.Parameters.AddWithValue("@MedicalCertificateNumber", ele.MedicalCertificateNumber);
                cmd.Parameters.AddWithValue("@MedicalCertificateDOC", ele.MedicalCertificateDOC);
                cmd.Parameters.AddWithValue("@MovementRecommendationLatter", ele.MovementRecommendationLatter);
                cmd.Parameters.AddWithValue("@UserID", ele.UserID);

                cmd.CommandType = CommandType.StoredProcedure;

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




            //try
            //{
            //    DALConn();
            //    DataTable dt = new DataTable();
            //    SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    cmd.Parameters.AddWithValue("@Action", "Insert_Elephant_Movement");
            //    cmd.Parameters.AddWithValue("@From_Place", ele.From_place);
            //    cmd.Parameters.AddWithValue("@To_Place", ele.To_Place);
            //    cmd.Parameters.AddWithValue("@FromDistrict_Code", ele.FromDistrict_Code);
            //    cmd.Parameters.AddWithValue("@ToDistrict_Code", ele.ToDistrict_Code);
            //    cmd.Parameters.AddWithValue("@ElephantId", ele.ElephantId);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    da.Fill(dt);
            //    return dt;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    Conn.Close();
            //}
        }

        public DataTable DetailsElephantMovement(string UserID)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Details_Elephant_Movement");

                cmd.Parameters.AddWithValue("@UserID", UserID);

                cmd.CommandType = CommandType.StoredProcedure;

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



        public DataSet ElephantReviewApprover(Int64 UserID)
        {

            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "ElephantReviewApprover");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataSet GetElephantRevApprvDetails(string UserID, string RequestID)
        {

            try
            {
                DALConn();
                DataSet dt = new DataSet();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "AssignElephantAppAndReviewApprover");
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@RequestID", RequestID);
                cmd.CommandType = CommandType.StoredProcedure;
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


        public DataTable SubmitElephantReviewApprover(ElephantMovement ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SubmitElephantReviewApprover");
                cmd.Parameters.AddWithValue("@RequestID", ele.RequestID);

                cmd.Parameters.AddWithValue("@OtherStateTPNumber", ele.OtherStateTPNumber);
                cmd.Parameters.AddWithValue("@OtherStateNocDoc", ele.OtherStateNocDoc);
                cmd.Parameters.AddWithValue("@TPDate", ele.TPDate);
                cmd.Parameters.AddWithValue("@OtherStateContactNo", ele.OtherStateContactNo);

                cmd.Parameters.AddWithValue("@UserID", ele.UserID);
                cmd.Parameters.AddWithValue("@AssignTo", ele.AssignTo);
                cmd.Parameters.AddWithValue("@ActionStatus", ele.ActionStatus);
                cmd.Parameters.AddWithValue("@Comment", ele.Comment);
                cmd.Parameters.AddWithValue("@Reasons", ele.Reasons);
                cmd.Parameters.AddWithValue("@RajasthanAreaDCF", ele.RajasthanAreaDCF);

                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable SubmitElephantAssign(ElephantMovement ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SubmitElephantAssign");
                cmd.Parameters.AddWithValue("@RequestID", ele.RequestID);


                cmd.Parameters.AddWithValue("@UserID", ele.UserID);
                cmd.Parameters.AddWithValue("@AssignTo", ele.AssignTo);
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable AutoSuggestSSO(string SSOID, string SearchType)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_ElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "SelectSSOID");
                cmd.Parameters.AddWithValue("@SSOID", SSOID);
                cmd.Parameters.AddWithValue("@Comment", SearchType);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable GetAllRajasthanAreaDCF(long UserID)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_IssueNOCForOtherStateElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("@Action", "GetDivisionofficeSSOIDbyDist");
                cmd.Parameters.AddWithValue("@ToDistrictId", UserID);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataSet TPElephant(string ElephantId)
        {

            DataSet dt = new DataSet();
            try
            {
                DALConn();

                SqlParameter[] parameters =
            {    
            new SqlParameter("@Action", "ElephentTPPDF"),
             new SqlParameter("@RequestID", ElephantId)
            };
                Fill(dt, "sp_ElephantPDFDoc", parameters);

            }
            catch (Exception ex)
            {
                //new Common().ErrorLog(ex.Message, "Select_TicketData_For_Print" + "_" + "Citizen", 1, DateTime.Now, Convert.ToInt64(HttpContext.Current.Session["UserId"]));

            }
            finally
            {
                Conn.Close();
            }
            return dt;
        }

    }


    //public class ElephantInfo : DAL
    //{
    //    public int ElephantId { get; set; }
    //    public int Index { get; set; }
    //    public string ElephantName { get; set; }
    //    public string Age { get; set; }
    //    public string ssoid { get; set; }
    //    public Int64 RegNumber { get; set; }
    //    public string LicenceNumber { get; set; }
    //    public string Colour { get; set; }
    //    public int isActive { get; set; }
    //    public bool IsactiveView { get; set; }
    //    public string ColourofEye { get; set; }
    //    public string EnteredBy { get; set; }
    //    public string NeekGirth { get; set; }
    //    public double Height { get; set; }
    //    public double Length { get; set; }
    //    public double Weight { get; set; }
    //    public int NoofNail { get; set; }
    //    public double LengthofTusk { get; set; }
    //    public string IdentificationMarks { get; set; }
    //    public double NoofInsuranceMarks { get; set; }
    //    public string InsuranceDate { get; set; }
    //    public string VerterinaryCertificatedDate { get; set; }
    //    public string PresentMarketValue { get; set; }
    //    public string SourceofPurchase { get; set; }
    //    public string OperationType { get; set; }

    //    public DataTable Select_ElephantInfos()
    //    {
    //        try
    //        {
    //            DALConn();
    //            DataTable dt = new DataTable();
    //            SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "SelectAllElephantInfo");
    //            cmd.CommandType = CommandType.StoredProcedure;

    //            da.Fill(dt);
    //            return dt;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //    }

    //    public DataTable Select_ElephantInfo(int ElephantId)
    //    {
    //        try
    //        {
    //            DALConn();
    //            DataTable dt = new DataTable();
    //            SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "SelectOneElephantInfo");
    //            cmd.Parameters.AddWithValue("@ElephantId", ElephantId);
    //            cmd.CommandType = CommandType.StoredProcedure;

    //            da.Fill(dt);
    //            return dt;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }

    //    }

    //    public DataTable AddUpdateElephantInfo(ElephantInfo oElephantInfo)
    //    {
    //        try
    //        {
    //            DALConn();
    //            DataTable dt = new DataTable();
    //            SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            SqlDataAdapter da = new SqlDataAdapter(cmd);

    //            cmd.Parameters.AddWithValue("@Action", "AddUpdateElephantInfo");
    //            cmd.Parameters.AddWithValue("@ElephantId", oElephantInfo.ElephantId);
    //            cmd.Parameters.AddWithValue("@ElephantName", oElephantInfo.ElephantName);
    //            cmd.Parameters.AddWithValue("@ssoid", oElephantInfo.ssoid);
    //            cmd.Parameters.AddWithValue("@RegNumber", oElephantInfo.RegNumber);
    //            cmd.Parameters.AddWithValue("@LicenceNumber", oElephantInfo.LicenceNumber);
    //            cmd.Parameters.AddWithValue("@Age", oElephantInfo.Age);
    //            cmd.Parameters.AddWithValue("@isActive", oElephantInfo.isActive);
    //            cmd.Parameters.AddWithValue("@EnteredBy", oElephantInfo.EnteredBy);
    //            cmd.Parameters.AddWithValue("@Colour", oElephantInfo.Colour);
    //            cmd.Parameters.AddWithValue("@ColourofEye", oElephantInfo.ColourofEye);
    //            cmd.Parameters.AddWithValue("@Height", oElephantInfo.Height);
    //            cmd.Parameters.AddWithValue("@Length", oElephantInfo.Length);
    //            cmd.Parameters.AddWithValue("@NeekGirth", oElephantInfo.NeekGirth);
    //            cmd.Parameters.AddWithValue("@Weight", oElephantInfo.Weight);
    //            cmd.Parameters.AddWithValue("@NoofNail", oElephantInfo.NoofNail);
    //            cmd.Parameters.AddWithValue("@LengthofTusk", oElephantInfo.LengthofTusk);
    //            cmd.Parameters.AddWithValue("@IdentificationMarks", oElephantInfo.IdentificationMarks);
    //            cmd.Parameters.AddWithValue("@NoofInsuranceMarks", oElephantInfo.NoofInsuranceMarks);
    //            cmd.Parameters.AddWithValue("@InsuranceDate", oElephantInfo.InsuranceDate);
    //            cmd.Parameters.AddWithValue("@VerterinaryCertificatedDate", oElephantInfo.VerterinaryCertificatedDate);
    //            cmd.Parameters.AddWithValue("@PresentMarketValue", oElephantInfo.PresentMarketValue);
    //            cmd.Parameters.AddWithValue("@SourceofPurchase", oElephantInfo.SourceofPurchase);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            da.Fill(dt);
    //            return dt;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        finally
    //        {
    //            Conn.Close();
    //        }
    //    }

    //    //public DataTable SelectAllDistricts()
    //    //{
    //    //    try
    //    //    {
    //    //        DALConn();
    //    //        DataTable dt = new DataTable();
    //    //        SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
    //    //        cmd.CommandType = CommandType.StoredProcedure;
    //    //        SqlDataAdapter da = new SqlDataAdapter(cmd);

    //    //        cmd.Parameters.AddWithValue("@Action", "SelectAllDistrict");
    //    //        cmd.CommandType = CommandType.StoredProcedure;
    //    //        da.Fill(dt);
    //    //        return dt;
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        throw ex;
    //    //    }
    //    //    finally
    //    //    {
    //    //        Conn.Close();
    //    //    }
    //    //}

    //}


    public class ElephantInfo : DAL
    {
        public int ElephantId { get; set; }
        public int Index { get; set; }
        [Required]
        [Display(Name = "ElephantName")]
        public string ElephantName { get; set; }
        [Required]
        [Display(Name = "Age")]
        public string Age { get; set; }
        [Required]
        [Display(Name = "ssoid")]
        public string ssoid { get; set; }
        [Required]
        [Display(Name = "RegNumber")]
        public string  RegNumber { get; set; }

       
        [Required]
        [Display(Name = "Micro Chip Number")]
        public string LicenceNumber { get; set; }
        [Required]
        [Display(Name = "Colour")]
        public string Colour { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }


        public int isActive { get; set; }
        public bool IsactiveView { get; set; }
        [Required]
        [Display(Name = "ColourofEye")]
        public string ColourofEye { get; set; }

        public string EnteredBy { get; set; }
        [Required]
        [Display(Name = "NeekGirth")]
        public string NeekGirth { get; set; }
        [Required]
        [Display(Name = "Height (feet)")]
        public double Height { get; set; }
        [Required]
        [Display(Name = "Length (feet)")]
        public double Length { get; set; }
        [Required]
        [Display(Name = "Weight (KG)")]
        public double Weight { get; set; }
        [Required]
        [Range(1,20)]
        [Display(Name = "NoofNail")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only insert numbers")]
        public int NoofNail { get; set; }
        [Required]
        [Display(Name = "LengthofTusk (In Inch)")]
        public double LengthofTusk { get; set; }
        [Required]
        [Display(Name = "IdentificationMarks")]
        public string IdentificationMarks { get; set; }
        //[Required]
        [Display(Name = "NoofInsuranceMarks")]
        public double NoofInsuranceMarks { get; set; }

        public string InsuranceDate { get; set; }
        public string VerterinaryCertificatedDate { get; set; }
        [Required]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only insert numbers")]
        [Display(Name = "PresentMarketValue")]
        public string PresentMarketValue { get; set; }
        [Required]
        [Display(Name = "Source")]
        public string SourceofPurchase { get; set; }
        public string OperationType { get; set; }

        public string PurchasedIllegalFile { get; set; }
        public string VerterinaryFile { get; set; }
        public string OwnershipCertificateFile { get; set; }

        public DataTable Select_ElephantInfos()
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectAllElephantInfo");
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable Select_ElephantInfo(int ElephantId)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "SelectOneElephantInfo");
                cmd.Parameters.AddWithValue("@ElephantId", ElephantId);
                cmd.CommandType = CommandType.StoredProcedure;

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

        public DataTable AddUpdateElephantInfo(ElephantInfo oElephantInfo)
        {
            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Elephant_Info", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "AddUpdateElephantInfo");
                cmd.Parameters.AddWithValue("@ElephantId", oElephantInfo.ElephantId);
                cmd.Parameters.AddWithValue("@ElephantName", oElephantInfo.ElephantName);
                cmd.Parameters.AddWithValue("@ssoid", oElephantInfo.ssoid);
                cmd.Parameters.AddWithValue("@RegNumber", oElephantInfo.RegNumber);
                cmd.Parameters.AddWithValue("@LicenceNumber", oElephantInfo.LicenceNumber);
                cmd.Parameters.AddWithValue("@Age", oElephantInfo.Age);
                cmd.Parameters.AddWithValue("@isActive", oElephantInfo.isActive);
                cmd.Parameters.AddWithValue("@EnteredBy", oElephantInfo.EnteredBy);
                cmd.Parameters.AddWithValue("@Colour", oElephantInfo.Colour);
                cmd.Parameters.AddWithValue("@ColourofEye", oElephantInfo.ColourofEye);
                cmd.Parameters.AddWithValue("@Height", oElephantInfo.Height);
                cmd.Parameters.AddWithValue("@Length", oElephantInfo.Length);
                cmd.Parameters.AddWithValue("@NeekGirth", oElephantInfo.NeekGirth);
                cmd.Parameters.AddWithValue("@Weight", oElephantInfo.Weight);
                cmd.Parameters.AddWithValue("@NoofNail", oElephantInfo.NoofNail);
                cmd.Parameters.AddWithValue("@LengthofTusk", oElephantInfo.LengthofTusk);
                cmd.Parameters.AddWithValue("@IdentificationMarks", oElephantInfo.IdentificationMarks);
                cmd.Parameters.AddWithValue("@NoofInsuranceMarks", oElephantInfo.NoofInsuranceMarks);
                cmd.Parameters.AddWithValue("@InsuranceDate", oElephantInfo.InsuranceDate);
                cmd.Parameters.AddWithValue("@VerterinaryCertificatedDate", oElephantInfo.VerterinaryCertificatedDate);
                cmd.Parameters.AddWithValue("@PresentMarketValue", oElephantInfo.PresentMarketValue);
                cmd.Parameters.AddWithValue("@SourceofPurchase", oElephantInfo.SourceofPurchase);
                cmd.Parameters.AddWithValue("@Gender", oElephantInfo.Gender);
                cmd.Parameters.AddWithValue("@PurchasedIllegalFile", oElephantInfo.PurchasedIllegalFile);
                cmd.Parameters.AddWithValue("@OwnershipCertificateFile", oElephantInfo.OwnershipCertificateFile);
                cmd.Parameters.AddWithValue("@VerterinaryFile", oElephantInfo.VerterinaryFile);

                cmd.CommandType = CommandType.StoredProcedure;
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


    public class OtherStateElephantMovement : DAL
    {
        public Int64? ID { get; set; }
        public Int64? Index { get; set; }
        public string RequestID { get; set; }

        [Required]
        [Display(Name = "From State")]
        public int FromStateId { get; set; }

        [Required]
        [Display(Name = "From District")]
        public int FromDistrictId { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "From Date")]
        public string FromDate { get; set; }

        [Required]
        [Display(Name = "To State")]
        public int ToStateId { get; set; }

        [Required]
        [Display(Name = "To District")]
        public int ToDistrictId { get; set; }
        [StringLength(10)]
        [Required]
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "Return To Date")]
        public string ReturnToDate { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "Return From Date")]
        public string ReturnFromDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Other State Requested NOC Number")]
        public string OtherStateRequestedTPNumber { get; set; }

        [Required]
        [Display(Name = "Other State Requested NOC Doc")]
        public string OtherStateRequestedTPDoc { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Other State Requested NOC Date")]
        public string OtherStateRequestedTPDate { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Other State Contact No")]
        public string OtherStateContactNo { get; set; }
        public Int64? UserID { get; set; }

        public string MovementFrom { get; set; }
        public string MovementTo { get; set; }
        public string MovementStatus { get; set; }

        public string SSOID { get; set; }


        [Display(Name = "Rajastan Area DCF Officer")]
        public string RajasthanAreaDCF { get; set; }
        public List<SelectListItem> ListRajasthanAreaDCF { get; set; }

        public DataTable SaveOtherStateElephantMovement(OtherStateElephantMovement ele)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_IssueNOCForOtherStateElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@FromStateId", ele.FromStateId);
                cmd.Parameters.AddWithValue("@FromDistrictId", ele.FromDistrictId);
                cmd.Parameters.AddWithValue("@FromDate", ele.FromDate);

                cmd.Parameters.AddWithValue("@ToStateId", ele.ToStateId);
                cmd.Parameters.AddWithValue("@ToDistrictId", ele.ToDistrictId);
                cmd.Parameters.AddWithValue("@ToDate", ele.ToDate);

                cmd.Parameters.AddWithValue("@ReturnFromDate", ele.ReturnFromDate);
                cmd.Parameters.AddWithValue("@ReturnToDate", ele.ReturnToDate);

                cmd.Parameters.AddWithValue("@OtherStateRequestedTPNumber", ele.OtherStateRequestedTPNumber);
                cmd.Parameters.AddWithValue("@OtherStateRequestedTPDate", ele.OtherStateRequestedTPDate);
                cmd.Parameters.AddWithValue("@OtherStateRequestedTPDoc", ele.OtherStateRequestedTPDoc);
                cmd.Parameters.AddWithValue("@OtherStateContactNo", ele.OtherStateContactNo);
                cmd.Parameters.AddWithValue("@UserID", ele.UserID);
                cmd.Parameters.AddWithValue("@RajasthanAreaDCF", ele.RajasthanAreaDCF);
                cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable DetailsOtherStateElephantMovement(string UserID)
        {

            try
            {
                DALConn();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("sp_IssueNOCForOtherStateElephantMovement", Conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.AddWithValue("@Action", "List");

                cmd.Parameters.AddWithValue("@UserID", UserID);

                cmd.CommandType = CommandType.StoredProcedure;

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
