using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CSharpProject.Models
{
    public class OrderedItem
    {
        [Key]
        public int OrderedItemId { get; set; }

        public int Quantity { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public Order OrderInfo { get; set; }

        public Product ProductInfo { get; set; }
    }

}