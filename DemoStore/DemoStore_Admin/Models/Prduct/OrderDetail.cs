using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.Prduct
{
    public class OrderDetail
    {


        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Key]
        [Column(Order = 1)]
        public int OrderId { get; set; }
        public Order Order { get; set; }


        [Key]
        [Column(Order = 2)]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }

}