using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CSharpProject.Models
{
    public class CorrectAdminNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((string)value != "admin")
                return new ValidationResult("Admin Name is incorrect, please try again");
            return ValidationResult.Success;
        }

    }
    public class CorrectPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((string)value != "a1234")
                return new ValidationResult("Admin Password is incorrect, please try again");
            return ValidationResult.Success;
        }

    }
}