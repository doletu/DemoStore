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
    
        public ActionResult Index(int page=1,int PageSize=4)
        {
            var usersWithRoles = (from user in dbContext.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      PhoneNumber = user.PhoneNumber,
                                      DataCreated=user.DateCreated,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in dbContext.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new ViewMainPageModel()

                                  {
                                      Id = p.UserId,
                                      UserName = p.Username,
                                      DateCreated = p.DataCreated,
                                      PhoneNumber=p.PhoneNumber,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  }) ;

            PagedList<ViewMainPageModel> model = new PagedList<ViewMainPageModel>(usersWithRoles, page, PageSize);
            return View(model);
        }

       
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
                                          select role.Name).ToList()
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

                }
                var selectedCustomerType = list.FirstOrDefault(d => d.Text == users.Role);
                if (selectedCustomerType != null)
                 selectedCustomerType.Selected = true;
                ViewBag.VbCategoryList = list;

                return View(users);
            }
            return View("Index");
        }

        [HttpPost]
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



        public PartialViewResult SearchUsers(string searchText, int page = 1, int PageSize = 4)
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

            var Filter = User.Where(a => a.Email.ToLower().Contains(searchText) ||
                                             a.Email.Contains(searchText)).ToList();

            PagedList<ViewMainPageModel> model = new PagedList<ViewMainPageModel>(Filter, page, PageSize);
            return PartialView("_Gridview", model);
        }
    }



}