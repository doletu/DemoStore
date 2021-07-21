using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.Prduct
{
    public class ProductImages
    {
        [Key]
        public int Id { get; set; }

        public string img { get; set; }



         [ForeignKey("Product")]
         public int ProductId { get; set; }
         public Product  Product { get; set; }
       


    }
}