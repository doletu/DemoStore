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




        [Authorize(Roles = "Admin,Manager")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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
            //  the paging links in order to keep the sort order the same while paging
            ViewBag.CurrentSort = sortOrder;
            //Add ViewBag to save SortOrder of table
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;





            //Added this area to, Search and match data, if search string is not null or empty
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Name.Contains(searchString));
            }

            //Switch action according to sortOrder
            switch (sortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.Name).ToList();
                    break;

                default:
                    list = list.OrderBy(s => s.Name).ToList();
                    break;
            }




            //ViewBag.CurrentFilter, provides the view with the current filter string.
            //he search string is changed when a value is entered in the text box and the submit button is pressed. In that case, the searchString parameter is not null.
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            //indicates the size of list
            int pageSize = 3;
            //set page to one is there is no value, ??  is called the null-coalescing operator.
            int pageNumber = (page ?? 1);
            //return the Model data with paged
            return View(list.ToPagedList(pageNumber, pageSize));

        }







        // GET: Products/Details/5
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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

    


    }
}
