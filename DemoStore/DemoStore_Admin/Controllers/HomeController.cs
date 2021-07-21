using DemoStore_Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using System.Web.Security;
using System.Net;

namespace DemoStore_Admin.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();





        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Settings(string UserId)
        {

            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            var users = (from user in dbContext.Users
                         select new
                         {
                             UserId = user.Id,
                             Username = user.UserName,
                             Email = user.Email,
                             PhoneNumber=user.PhoneNumber,
                             RoleNames = (from userRole in user.Roles
                                          join role in dbContext.Roles on userRole.RoleId
                                          equals role.Id
                                          select role.Id).ToList()
                         }).ToList().Select(p => new ViewMainPageModel()

                         {
                             PhoneNumber=p.PhoneNumber,
                             Id = p.UserId,
                             UserName = p.Username,
                             Email = p.Email,
                             Role = string.Join(",", p.RoleNames)
                         }).FirstOrDefault(p=>p.Id==UserId);
            if (users != null)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (var Role in rm.Roles)
                {
                    list.Add(new SelectListItem()
                    {
                        Value = Role.Id,
                        Text = Role.Name
                    });

                };
                ViewBag.VbCategoryList = list;

                return View(users);
            }
            return View("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Settings(ViewMainPageModel  view)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var Role in rm.Roles)
            {
                list.Add(new SelectListItem()
                {
                    Value = Role.Id,
                    Text = Role.Name
                });

            }

            var selectedCustomerType = list.FirstOrDefault(d => d.Value == view.Role);
            if (selectedCustomerType != null)
                selectedCustomerType.Selected = true;
         



            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));
            var users = dbContext.Users.FirstOrDefault(p => p.Id == view.Id);
            if (users != null)
            {
               




                users.UserName = view.UserName;
                users.Email = view.Email;
                users.PhoneNumber = view.PhoneNumber;


                
                foreach (var item in list)
                {

                    if (await userManager.IsInRoleAsync(users.Id, item.Text))
                    {
                        await userManager.RemoveFromRoleAsync(users.Id, item.Text);
                    }
             
                }

                await userManager.AddToRoleAsync(users.Id,selectedCustomerType.Text);


                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext));

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return (ActionResult)View("NotFound");
            }

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.ToString());
                }
            }




            return RedirectToAction("Index");

        }

        // GET: Categories/Details/5

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }




        [Authorize(Roles = "Admin,Manager")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var User = (from user in dbContext.Users
                        select new
                        {
                            UserId = user.Id,
                            Username = user.UserName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            DataCreated = user.DateCreated,
                            RoleNames = (from userRole in user.Roles
                                         join role in dbContext.Roles on userRole.RoleId
                                         equals role.Id
                                         select role.Name).ToList()
                        }).ToList().Select(p => new ViewMainPageModel()

                        {
                            Id = p.UserId,
                            UserName = p.Username,
                            DateCreated = p.DataCreated,
                            PhoneNumber = p.PhoneNumber,
                            Email = p.Email,
                            Role = string.Join(",", p.RoleNames)
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
              User = User.Where(s => s.UserName.Contains(searchString)
                                       || s.Email.Contains(searchString));
            }

            //Switch action according to sortOrder
            switch (sortOrder)
            {
                case "name_desc":
                    User = User.OrderByDescending(s => s.UserName).ToList();
                    break;

                default:
                    User = User.OrderBy(s => s.UserName).ToList();
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
            return View(User.ToPagedList(pageNumber, pageSize));

        }
    }



}