using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DemoStore_Admin.Models;
using DemoStore_Admin.Models.Prduct;
using DemoStore_Admin.Models.ViewModels;
using PagedList;

namespace DemoStore_Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index(int page = 1, int PageSize = 4)
        {

            var listcate= db.Categories.ToList().Select(p => new CategoryViewModel()

                {
                    Id = p.Id,
                    Name=p.Name,
                    CoverImage=p.CoverImage
                });

            PagedList<CategoryViewModel> model = new PagedList<CategoryViewModel>(listcate, page, PageSize);
            return View(model);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CoverImage")] Category category, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {


                var url = file.ToString(); //getting complete url  

                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  

                string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  

                string myfile = name + "_" + category.Name + ext; //appending the name with id  

                // store the file inside ~/project folder(Img)  
                var path = Path.Combine(Server.MapPath("~/Img"), myfile);

                category.CoverImage = myfile;







                db.Categories.Add(category);
                db.SaveChanges();
                file.SaveAs(path);

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CoverImage")] Category category, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var cate = db.Categories.Find(category.Id);
                if (cate!=null)
                {
                    cate.Name = category.Name;
                };
                if (file!=null)
                {
                    var url = file.ToString(); //getting complete url  

                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var item = db.Categories.Find(category.Id).CoverImage;
                    if (!fileName.Contains(item))
                    {
                        var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  

                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  

                        string myfile = name + "_" + category.Name + ext; //appending the name with id  

                        // store the file inside ~/project folder(Img)  
                        var path = Path.Combine(Server.MapPath("~/Img"), myfile);

                        cate.CoverImage = myfile;


                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        [HttpPost]
        public ActionResult DeleteCate(int id)
        {

            var item = db.Categories.Find(id);

            if (item == null)
            {
                ViewBag.ErrorMessage = "Cannot be found";
                return (ActionResult)View("NotFound");
            }
            var result = db.Categories.Remove(item);
            db.SaveChanges();

            return RedirectToAction("Index");

        }



        public PartialViewResult SearchCategories(string searchText, int page = 1, int PageSize = 4)
        {
            var listcate = db.Categories.ToList().Select(p => new CategoryViewModel()

            {
                Id = p.Id,
                Name = p.Name,
                CoverImage = p.CoverImage
            });

            var Filter = listcate.Where(a => a.Name.ToLower().Contains(searchText)).ToList();

            PagedList<CategoryViewModel> model = new PagedList<CategoryViewModel>(Filter, page, PageSize);
            return PartialView("_GridViewCategory", model);
        }
    }


}
