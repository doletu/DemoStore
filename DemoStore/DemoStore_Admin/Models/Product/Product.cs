using DemoStore_Admin.Models.Prduct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }



        [Required]
        public decimal Price { get; set; }

        public int Discount { get; set; }




        [StringLength(255)]
        public string Description { get; set; }


        [Required]
        public string CoverImage { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int ViewCount { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }


        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category  Category { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }



    }
    


    

  


}