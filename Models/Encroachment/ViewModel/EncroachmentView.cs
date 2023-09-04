

//---------------------------------------------------------------
//  Project      : FMDSS 
//  Project Code : IEISL_SSD_2015_16_ENV_004
//  Copyright (C): IEISL 
//  File         : EncroachmentView
//  Description  : File contains function related to encroachment 
//  Date Created : 26-07-2017
//  Author       : Rajkumar
//---------------------------------------------------------------


using FMDSS.Models.Encroachment.DomainModel;
using FMDSS.Models.MIS;
using FMDSS.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMDSS.Models.Encroachment.ViewModel
{
    public class EncroachmentView : DAL
    {
        [Required(ErrorMessage = "Select Division")]
        public string DIV_CODE { get; set; }
        [Required(ErrorMessage = "Select Range")]
        public string RANGE_CODE { get; set; }
        //[Required(ErrorMessage = "Enter Lractno")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\/s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string LRACTNO { get; set; }
        [Required(ErrorMessage = "Select known/Unknown")]
        public bool IsKnown { get; set; }

        public HttpPostedFileBase Files { get; set; }

        [Required(ErrorMessage = "Enter area")]
        //[Range(0.01, 999999999, ErrorMessage = "Price must be greater than 0.00")]
        public decimal? Area { get; set; }
        [Required(ErrorMessage = "Enter valid description")]
        public string Description { get; set; }
        public long EnteredBy { get; set; }
        public string UserName { get; set; }
        public byte[] KMLFile { get; set; }
        public string KMLFileName { get; set; }       
        public HttpPostedFileBase AcfDecisionFiles { get; set; }
        public byte[] Acf_Decision_Upload { get; set; }        
        public string EncroachmentId { get; set; }
        public long InvestigationOfficers { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DOE { get; set; }
        public string SSOId { get; set; }
        public string DispatchNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> DispatchDate { get; set; }
        public string ACF_Status { get; set; }
        public string ACF_Remarks { get; set; }
        public string NoticeNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> NoticeDate { get; set; }
        public string DateOfEntry { get; set; }
        public string Dispatch_Date { get; set; }
        public string Notice_Date { get; set; }
        public string Decision_Taken { get; set; }
        public Nullable<DateTime> Decision_Date { get; set; }
        public Nullable<DateTime> Next_Date { get; set; }
        public int Year { get; set; }
        public string khasraNo { get; set; }
        public decimal TotalArea { get; set; }
        public string TypeofLand { get; set; }
        public decimal TaxPerHact { get; set; }
        public decimal Encroachment_Area { get; set; }
        public decimal Tax { get; set; }
        public int FileStatus { get; set; }
		public string Encroacher_Name { get; set; }
		public string EN_Code { get; set; }
		public decimal EncrochedArea { get; set; }
		public decimal RateOfLagan { get; set; }

		public long ReviewerOfficer { get; set; }

        public decimal Total_Area_Block { get; set; }
        public string CompartmentNo { get; set; }
        public string InformationGatheredBy { get; set; }
        public string InformationApprovedBy { get; set; }
        public string NotificationNo { get; set; }
        public DateTime NotificationDate { get; set; }
        public string Encroachment_Yield { get; set; }  //paidawar
        public string Remarks { get; set; }

        public string Near_by_area { get; set; }
        public string Block { get; set; }
        public string Special_Instruction { get; set; }



        /// <summary>
        /// This function fills designation dropdown
        /// </summary>
        /// <returns></returns>
        public List<EncroachmentView> EncroachmentList(string Option)
        {
            try
            {
                Repository<EncroachmentView> repository = new Repository<EncroachmentView>();
                List<EncroachmentView> lstEncroachment = new List<EncroachmentView>();

                object[] xparams ={                                                       
                             new SqlParameter("@UserId",Convert.ToInt64(HttpContext.Current.Session["UserId"])),
                             new SqlParameter("@option", Option)};

                var result = repository.GetWithStoredProcedure("dbo.SP_EncroachmentList @UserId,@option", xparams).ToList();
                foreach (var item in result)
                {
                    lstEncroachment.Add(new EncroachmentView
                    {

                        EncroachmentId = item.EncroachmentId,
                        DIV_CODE = item.DIV_CODE,
                        RANGE_CODE = item.RANGE_CODE,
                        UserName = item.UserName,
                        IsKnown=item.IsKnown,
                        Area=item.Area,
                        LRACTNO=item.LRACTNO,
                        DOE = item.DOE,
                        DispatchNo = item.DispatchNo,
                        DispatchDate = item.DispatchDate,
                        ACF_Status = item.ACF_Status,
                        ACF_Remarks=item.ACF_Remarks,
                        NoticeNo =  item.NoticeNo,
                        NoticeDate = item.NoticeDate,
                        Decision_Taken = item.Decision_Taken,
                        Decision_Date = item.Decision_Date,
                        Next_Date = item.Next_Date,
						Encroacher_Name = item.Encroacher_Name

					});
                }
                return lstEncroachment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<MISEncroachmentDetails> Praptra_2List(string EN_Code)
        {

            List<MISEncroachmentDetails> Praptra_2List = new List<MISEncroachmentDetails>();

            try
            {
                try
                {
                    DALConn();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("SP_praptra_3", Conn);
                    cmd.Parameters.AddWithValue("@EN_Code", EN_Code);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = 500000000;
                    da.Fill(dt);
                    foreach (DataRow item in dt.Rows)
                    {
                        Praptra_2List.Add(new MISEncroachmentDetails
                        {

                            EN_Code = Convert.ToString(item["EN_Code"]),
                            NoticeNo = Convert.ToString( item["NoticeNo"]),
                            DIV_NAME = Convert.ToString( item["DIV_NAME"]),
                            NameAddress = Convert.ToString( item["NameAddress"]),
                            Year = Convert.ToString( item["Year"]),
                            khasraNo = Convert.ToString( item["khasraNo"]),
                            EncrochArea = Convert.ToString( item["EncrochArea"]),
                            PaidawarOrKISMA = Convert.ToString( item["PaidawarOrKISMA"]),
                            TaxPerHact = Convert.ToString( item["TaxPerHact"]),
                            EncrochedArea = Convert.ToString( item["EncrochedArea"]),
                            EncrochedPaidawar = Convert.ToString( item["EncrochedPaidawar"]),
                            TotalTax = Convert.ToString( item["TotalTax"]),
                            V_V = Convert.ToString(item["V_V"]),
                        });
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

              
               
                return Praptra_2List;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// This function fills designation dropdown
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> OfficerDesignation()
        {
            try
            {
                Repository<tbl_mst_Designations> repository = new Repository<tbl_mst_Designations>();
                List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
                object[] xparams ={ 
                             //new SqlParameter("@option", "1"),
							 new SqlParameter("@option", "3"),
							 new SqlParameter("@ssoid",  HttpContext.Current.Session["SSOid"]),
                             new SqlParameter("@EmpDesig",HttpContext.Current.Session["DesignationId"])};

                var result = repository.GetWithStoredProcedure("dbo.Sp_FPM_GetFOfficerDesig @option,@ssoid,@EmpDesig", xparams).ToList();
                foreach (var item in result)
                {
                    lstOfficerDesignation.Add(new SelectListItem { Text = item.Desig_Name, Value = Convert.ToString(item.EmpDesignation) });
                }
                return lstOfficerDesignation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		/// <summary>
		/// This function fills designation dropdown
		/// </summary>
		/// <returns></returns>
		public List<SelectListItem> OfficerDesignationForReview()
		{
			try
			{
				Repository<tbl_mst_Designations> repository = new Repository<tbl_mst_Designations>();
				List<SelectListItem> lstOfficerDesignation = new List<SelectListItem>();
				object[] xparams ={ 
                            
							 new SqlParameter("@option", "4"),
							 new SqlParameter("@ssoid",  HttpContext.Current.Session["SSOid"]),
							 new SqlParameter("@EmpDesig",HttpContext.Current.Session["DesignationId"])};

				var result = repository.GetWithStoredProcedure("dbo.Sp_FPM_GetFOfficerDesig @option,@ssoid,@EmpDesig", xparams).ToList();
				foreach (var item in result)
				{
					lstOfficerDesignation.Add(new SelectListItem { Text = item.Desig_Name, Value = Convert.ToString(item.EmpDesignation) });
				}
				return lstOfficerDesignation;
			}
			catch (Exception ex)
			{
				throw;
			}
		}


		/// <summary>
		/// This function returns list of forest office based on designation
		/// </summary>
		/// <param name="designation"></param>
		/// <returns></returns>
		public List<SelectListItem> ListForestOfficer(string designation)
        {
            Repository<ForestEmployeesTemparyClass> repository = new Repository<ForestEmployeesTemparyClass>();
            List<SelectListItem> lstOfficer = new List<SelectListItem>();
            try
            {
                object[] xparams ={ 
                             new SqlParameter("@option", "2"),
                             new SqlParameter("@ssoid",  HttpContext.Current.Session["SSOid"]),
                             new SqlParameter("@EmpDesig",designation)};

                var result = repository.GetWithStoredProcedure("dbo.Sp_FPM_GetFOfficerDesig @option,@ssoid,@EmpDesig", xparams).Select(d => new { d.ROWID,d.SSO_ID }).ToList();

                foreach (var item in result)
                {
                    lstOfficer.Add(new SelectListItem { Text = item.SSO_ID, Value = Convert.ToString(item.ROWID) });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return lstOfficer;
        }        

       
    }

   
}