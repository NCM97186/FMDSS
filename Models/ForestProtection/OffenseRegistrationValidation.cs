using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FMDSS.Models.ForestProtection
{
    public class OffenseRegistrationValidation:ValidationAttribute
    {

        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)value;
            string error = string.Empty;
            if (dt!=null)
            {
               
            }
            else {
                error = "Add atleast one record to submit";
                return new ValidationResult(error);
            }
            return ValidationResult.Success;  
        }

    }
}