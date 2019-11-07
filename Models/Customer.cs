using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CSharpProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [Column("FirstName", TypeName = "VARCHAR(45)")]
        public string FirstName { get; set; }

        [Required]
        [Column("LastName", TypeName = "VARCHAR(45)")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Column("Email", TypeName = "VARCHAR(45)")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string BillingAddress { get; set; }

        public string DeliveryAddress { get; set; }

        [Required]
        public long? CreditCardNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be 3 or 4 digits")]
        public int? CVV { get; set; }

        [Required(ErrorMessage = "Expiration Date is required")]
        public int? Month { get; set; }

        [Required]
        public int? Year { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Order> Orders { get; set; }

    }

}