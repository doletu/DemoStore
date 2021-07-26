using DemoStore_Admin.Models.Prduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.ViewModels
{
    public class PageModel
    {

        public IEnumerable<Brand> Brands { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}