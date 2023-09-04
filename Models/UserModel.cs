//********************************************************************************************************
//  Project      : Forest Management & Decision Support System (FMDSS) 
//  Project Code : FMDSS 2.0
//  Copyright (C): 
//  File         : User Model
//  Description  : File contains functions For Business Rules and DB for User Details Update
//  Date Created : 15-06-2020
//  History      : 
//  Version      : 2.0
//  Author       : Rajan Kumar Bahrati
//  Modified By  : 
//  Modified On  : 
//  Reviewed By  : 
//  Reviewed On  : 
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMDSS.Models
{
  public  class UserModel
    {

        public int UserId { get; set; }


        [Required(ErrorMessage = "Please Enter Ssoid")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        public string Ssoid { get; set; }
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [Required]
        public string Name { get; set; }

       [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string EmailId { get; set; }

        [MaxLength(10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile must be numeric")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "{0}  is Required")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "DOB  is Required")]
        public string DOB { get; set; }
        [Required(ErrorMessage = "{0}  is Required")]
        public string Gender { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [MaxLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postal Code must be numeric")]
        public string Postal_Code { get; set; }
        public string District { get; set; }
        public string Postal_Address { get; set; }
       
        public string City2 { get; set; }
        public string Bhamashah_Id { get; set; }
        [MaxLength(12)]
       [RegularExpression("^[0-9]*$", ErrorMessage = "Aadhar must be numeric")]
        public string Aadhar_ID { get; set; }
        public bool IsActive { get; set; }

        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
    }

    public class User
    {
        public string ssoId { get; set; }
        //public string Password { get; set; }
    }
}
