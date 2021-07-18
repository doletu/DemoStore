using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoStore_Admin.Models;
using DemoStore_Admin.Models.Prduct;
using DemoStore_Admin.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using PagedList;

namespace DemoStore_Admin.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
         public ActionResult Index(int page = 1, int PageSize = 4)
         {
             var listbrand = db.Products.Include("Brand").Include("Category").ToList().Select(p => new ProductViewModel()

             {
                 Id = p.Id,
                 Name = p.Name,
                 Brand=p.Brand.Name,
                 Category=p.Category.Name,
                 Price=p.Price,
                 Discount=p.Discount,
                 ViewCount=p.ViewCount,
                 Stock=p.Stock,
                 DateCreated=p.DateCreated,
                 CoverImage = p.CoverImage,
                 Description = p.Description
             });


             PagedList<ProductViewModel> model = new PagedList<ProductViewModel>(listbrand, page, PageSize);
             return View(model);
         }
        
      


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include("Brand")
                .Include("Category")
                .FirstOrDefault(p=>p.Id==id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var viewmodel = new ProductViewModel()
            {

                Categories = db.Categories.ToList(),
                Brands = db.Brands.ToList()


            };
            return View(viewmodel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var item = viewModel.CoverImg;
                if (item != null)
                {
                    var myfile = UploadImage(item, viewModel.Name);
                    Product product = new Product()
                    {
                        BrandId = int.Parse(viewModel.Brand),
                        CategoryId = int.Parse(viewModel.Category),
                        Name = viewModel.Name,
                        Price = viewModel.Price,
                        DateCreated = DateTime.Now,
                        CoverImage = myfile,
                        ViewCount=0,
                        Stock=viewModel.Stock,
                         Description= viewModel.Description,
                          Discount=viewModel.Discount
                         
                    };
                    db.Products.Add(product);
                    if (viewModel.DetailImages!=null)
                    {
                        int index = 1;
                        foreach (var file in viewModel.DetailImages)
                        {

                            myfile= UploadImage(file, viewModel.Name + index.ToString());
                            index++;

                            var image = new ProductImages()
                            {
                                img = myfile,
                                Product = product

                            };
                            //  product.ProductImages = images;
                            db.ProductImages.Add(image);
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
             
            }
            var viewmodel = new ProductViewModel()
            {

                Categories = db.Categories.ToList(),
                Brands = db.Brands.ToList()


            };
            return View(viewmodel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Include("Brand").Include("Category").FirstOrDefault(p=>p.Id==id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var item = new ProductViewModel()

            {
                Brand = product.BrandId.ToString(),
                Categories = db.Categories.ToList(),
                Brands = db.Brands.ToList(),
                Detail = db.ProductImages.Where(a=>a.ProductId==id).ToList(),

                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Discount = product.Discount,
                Stock = product.Stock,
                CoverImage = product.CoverImage,
                Description = product.Description
            };

            return View(item);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var item = db.Products.Find(product.Id);
                if (item!=null)
                {

                    item.BrandId = int.Parse(product.Brand);
                    item.CategoryId = int.Parse(product.Category);
                    item.Name = product.Name;
                    item.Price = product.Price;
                    item.Stock = product.Stock;
                    item.Description = product.Description;
                    item.Discount = product.Discount;
                    item.DateCreated = DateTime.Now;
                    if (product.CoverImage != null)  item.CoverImage=UploadImage(product.CoverImg, product.Name);
                    if (product.DetailImages!=null)
                    {
                        int index = 1;

                        foreach (var file in product.DetailImages)
                        {
                            var myfile = UploadImage(file, product.Name + index.ToString());
                            index++;

                            var image = new ProductImages()
                            {
                                img = myfile,
                                Product = item

                            };
                            //  product.ProductImages = images;
                            db.ProductImages.Add(image);

                        }
                    }


                }



                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

       

        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {

            var item = db.Products.Find(id);

            if (item == null)
            {
                ViewBag.ErrorMessage = "Cannot be found";
                return (ActionResult)View("NotFound");
            }
            var result = db.Products.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Index");

        }





        public String UploadImage(HttpPostedFileBase item, String ItemName)
        {

            var url = item.ToString(); //getting complete url  

            var fileName = Path.GetFileName(item.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(item.FileName); //getting the extension(ex-.jpg)  

            string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  

            string myfile = name + "_" + ItemName + ext; //appending the name with id  

            // store the file inside ~/project folder(Img)  
            var path = Path.Combine(Server.MapPath("~/Img"), myfile);
            item.SaveAs(path);
            return myfile;
        }

        public PartialViewResult SearchProducts(string searchText, int page = 1, int PageSize = 4)
        {
            var list = db.Products.Include("Brand").Include("Category").ToList().Select(p => new ProductViewModel()

            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand.Name,
                Category = p.Category.Name,
                Price = p.Price,
                Discount = p.Discount,
                ViewCount = p.ViewCount,
                Stock = p.Stock,
                DateCreated = p.DateCreated,
                CoverImage = p.CoverImage,
                Description = p.Description
            });


          
            var Filter = list.Where(a => a.Name.ToLower().Contains(searchText)).ToList();
            PagedList<ProductViewModel> model = new PagedList<ProductViewModel>(Filter, page, PageSize);

            return PartialView("_GridViewProduct", model);
        }




    }
}
