using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoStore_Admin.Models;
using DemoStore_Admin.Models.Prduct;
using DemoStore_Admin.Models.ViewModels;
using PagedList;

namespace DemoStore_Admin.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Orders/Details/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create([Bind(Include = "Id,OrderDate,UserId,ShipName,ShipAddress,ShipPhoneNumber,status")] Order order)
        {
            if (ModelState.IsValid)
            {
               
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Create", "OrderDetails", new { area = "" });
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit([Bind(Include = "Id,OrderDate,UserId,ShipName,ShipAddress,ShipPhoneNumber,status")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        [Authorize(Roles = "Admin,Manager")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var list = db.Orders.ToList().Select(p => new OrderViewModel()

            {
                Id = p.Id,
                OrderDate = p.OrderDate,
                ShipAddress = p.ShipAddress,

                ShipName = p.ShipName,
                ShipPhoneNumber = p.ShipPhoneNumber,
                status = p.status,
                UserId = p.UserId,

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
                list = list.Where(s => s.ShipName.Contains(searchString)
                                         || s.UserId.Contains(searchString));
            }

            //Switch action according to sortOrder
            switch (sortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.ShipName).ToList();
                    break;

                default:
                    list = list.OrderBy(s => s.UserId).ToList();
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
