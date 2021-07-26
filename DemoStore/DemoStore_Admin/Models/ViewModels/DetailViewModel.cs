using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.ViewModels
{
    public class DetailViewModel
    {

        public  int Id { get; set; }
        public string Name { get; set; }
        public string CoverImage { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}