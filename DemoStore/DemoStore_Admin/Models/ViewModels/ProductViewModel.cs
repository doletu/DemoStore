using DemoStore_Admin.Models.Prduct;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.ViewModels
{
    public class ProductViewModel
    {

        public int Id { get; set; }


        [Required]
        [StringLength(255)]
        public string Name { get; set; }



        [Required]
        public decimal Price { get; set; }

        public int Discount { get; set; }




        [StringLength(255)]
        public string Description { get; set; }



        public IEnumerable<HttpPostedFileBase> DetailImages { get; set; }


        public HttpPostedFileBase CoverImg { get; set; }
        
        public string CoverImage { get; set; }
        public IEnumerable<ProductImages> Detail { get; set; }


        [Required]
        public int Stock { get; set; }

        public int ViewCount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

      

        public string Brand { get; set; }
        public IEnumerable<Brand> Brands { get; set; }

        public string Category { get; set; }
        public IEnumerable<Category>  Categories { get; set; }




    }
}