using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CSharpProject.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Column("TotalCost", TypeName = "DECIMAL(30,2)")]
        public decimal TotalCost { get; set; }

        public string Notes { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<OrderedItem> Products { get; set; }

    }

}