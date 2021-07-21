using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.Prduct
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string CoverImage { get; set; }

        public string Description { get; set; }


        public ICollection<Product> Products { get; set; }
    }



}