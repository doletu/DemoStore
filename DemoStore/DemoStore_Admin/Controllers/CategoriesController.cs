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



        // GET: Categories/Details/5
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin,Manager")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var list = db.Categories.ToList().Select(p => new CategoryViewModel()

            {
                Id = p.Id,
                Name = p.Name,
                CoverImage = p.CoverImage
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


    }


}
