using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;

namespace FMDSS.Models.Encroachment.ViewModel
{
    public class EncroachInvestigationView
    {
        public string EN_Code { get; set; }



        [Required(ErrorMessage = "Select year")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Enter khasra No.")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string khasraNo { get; set; }

        [Required(ErrorMessage = "Enter total area")]
        [Range(-1, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [RegularExpression(@"^\d+.?\d{0,4}$", ErrorMessage = "Invalid land area; Maximum four Decimal Points.")]
        public decimal TotalArea { get; set; }

        [Required(ErrorMessage = "Select land type")]
        public string TypeofLand { get; set; }

        [Required(ErrorMessage = "Enter tax per hectare")]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [RegularExpression(@"^\d+.?\d{0,5}$", ErrorMessage = "Invalid land area; Maximum five Decimal Points.")]
        public decimal TaxPerHact { get; set; }

        [Required(ErrorMessage = "Enter encroachment area")]
        [Range(0, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [RegularExpression(@"^\d+.?\d{0,5}$", ErrorMessage = "Invalid land area; Maximum five Decimal Points.")]
        public decimal Encroachment_Area { get; set; }

        [Required(ErrorMessage = "Enter Encroachment Paidawar")]

        public string Encroachment_Yield { get; set; }

        [Required(ErrorMessage = "Enter tax")]
        [Range(1, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid land area; Maximum Two Decimal Points.")]
        public decimal Tax { get; set; }

        [Required]
        [Display(Name = "Compartment No")]
        public string CompartmentNo { get; set; }

        [Required]
        [Display(Name = "Information Gathered By")]
        public string InformationGatheredBy { get; set; }

        [Required]
        [Display(Name = "Information Approved By")]
        public string InformationApprovedBy { get; set; }

        [Required]
        [Display(Name = "Notification No")]
        public string NotificationNo { get; set; }

        [Required]
        [Display(Name = "Notification Date")]
        public string NotificationDate { get; set; }




        public HttpPostedFileBase Files { get; set; }


        public HttpPostedFileBase MAPFiles { get; set; }
        public HttpPostedFileBase JamabandiFiles { get; set; }
        public HttpPostedFileBase NotificationFiles { get; set; }
        public HttpPostedFileBase PanchnamaFiles { get; set; }
        public HttpPostedFileBase NazarBandiFiles { get; set; }

        //== uploads
        public byte[] DocumentUpload { get; set; }
        public string UploadFileName { get; set; }
        public string UploadFileType { get; set; }

        public byte[] MAPDocumentUpload { get; set; }
        public string MAPUploadFileName { get; set; }
        public string MAPUploadFileType { get; set; }

        public byte[] JamabandiDocumentUpload { get; set; }
        public string JamabandiUploadFileName { get; set; }
        public string JamabandiUploadFileType { get; set; }

        public byte[] NotificationDocumentUpload { get; set; }
        public string NotificationUploadFileName { get; set; }
        public string NotificationUploadFileType { get; set; }

        public byte[] PanchnamaDocumentUpload { get; set; }
        public string PanchnamaUploadFileName { get; set; }
        public string PanchnamaUploadFileType { get; set; }

        public byte[] NazarBandiDocumentUpload { get; set; }
        public string NazarBandiUploadFileName { get; set; }
        public string NazarBandiUploadFileType { get; set; }


        [Required(ErrorMessage = "Enter block")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "special characters are not  allowed.")]
        public string Block_Name { get; set; }

        [Required(ErrorMessage = "Enter total area of block")]
        [Range(1, double.MaxValue, ErrorMessage = "The value must be greater than 0")]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid land area; Maximum Two Decimal Points.")]
        public decimal Total_Area_Block { get; set; }
        
        public string IsDocumentUpload { get; set; }

       

        public string IsMAPFilesDocumentUpload { get; set; }
        public string IsJamabandiFilesDocumentUpload { get; set; }
        public string IsNotificationFilesDocumentUpload { get; set; }
        public string IsPanchnamaFilesDocumentUpload { get; set; }
        public string IsNazarBandiFilesDocumentUpload { get; set; }


        public string Remarks { get; set; }
        public long Entered_By { get; set; }



        /// <summary>
        /// Function to save file to physical location
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="fileName"></param>
        /// <param name="uploadPath"></param>
        /// <returns></returns>
        public string CopyFileToSharedLocation(string fileID, string fileName, HttpPostedFileBase uploadPath)
        {
            string fullPath = string.Empty;
            string fileFullPathLocal = string.Empty;
            try
            {
                ////Save file to local directory
                string filePathLocal = ConfigurationManager.AppSettings["FolderPathEncroachment"].ToString();
                string strDate = DateTime.Now.ToShortDateString().Replace(".", string.Empty);
                strDate = strDate.Replace(":", string.Empty);
                strDate = strDate.Replace("/", "_");
                fullPath = filePathLocal + strDate + "\\" + fileID;

                if (!Directory.Exists(@fullPath))
                    Directory.CreateDirectory(@fullPath);

                filePathLocal = @fullPath;
                fileFullPathLocal = Path.Combine(filePathLocal, fileName);
                uploadPath.SaveAs(fileFullPathLocal);

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            return fileFullPathLocal;
        }
        /// <summary>
        /// function convert file into byte
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public byte[] StreamFile(string filename)
        {
            byte[] imageData;
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                imageData = new byte[fs.Length];
                fs.Read(imageData, 0, System.Convert.ToInt32(fs.Length));
                fs.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return imageData;
        }

    }
}