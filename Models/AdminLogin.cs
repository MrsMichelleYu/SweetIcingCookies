using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CSharpProject.Models
{
    public class AdminLogin
    {
        [Required]
        [CorrectAdminName]
        public string AdminName { get; set; }

        [Required]
        [CorrectPassword]
        public string Password { get; set; }

    }
}