using DemoStore_Admin.Models;
using DemoStore_Admin.Models.Prduct;
using DemoStore_Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoStore_Admin.Controllers
{
    public class PageController : Controller
    {

        public ApplicationDbContext dbContext = new ApplicationDbContext();


        // GET: Page
        public ActionResult Index()
        {
            IList<Brand> brands = dbContext.Brands.SqlQuery("Select top 3 * from Brands").ToList();
            IList<Product> products = dbContext.Products.SqlQuery("Select top 8 * from Products").ToList();
            ViewData["Brand"] = brands;
            ViewData["Product"] = products;
            return View();
        }


        public ActionResult Category()
        {

            IList<Category> cate = dbContext.Categories.SqlQuery("Select top 3 * from Categories").ToList();
            ViewData["Cate"] = cate;


            return View();
        }



        public ActionResult Brand()
        {
            IList<Brand> brands = dbContext.Brands.SqlQuery("Select * from Brands").ToList();
            ViewData["Brand"] = brands;

            return View();
        }

        public ActionResult Product()
        {
            IList<Product> products = dbContext.Products.SqlQuery("Select * from Products").ToList();
            ViewData["Product"] = products;

            return View();
        }



        public ActionResult DetailsProduct(int Id)
        {
            var item = dbContext.Products.FirstOrDefault(p => p.Id == Id);
            
            if (item!=null)
            {
                IList<ProductImages> images = dbContext.ProductImages.Where(p=>p.ProductId==Id).ToList();
                ViewData["Images"] = images;
                return View(item);
            }
            return HttpNotFound();
        }


        public ActionResult DetailsBrand(int Id)
        {
            var list = dbContext.Products.Include("Brand").Where(p => p.BrandId == Id).ToList();
            if (list != null)
            {
                return View(list);
            }
            return HttpNotFound();
        }


        public ActionResult DetailsCategory(int Id)
        {
            var list = dbContext.Products.Include("Category").Where(p => p.CategoryId == Id).ToList();
            if (list != null)
            {
                return View(list);
            }
            return HttpNotFound();
        }


    }
}