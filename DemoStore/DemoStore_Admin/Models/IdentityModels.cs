using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoStore_Admin.Models.Prduct;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DemoStore_Admin.Models
{
   
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Product> Products  { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories  { get; set; }
        public DbSet<Order> Orders  { get; set; }
        public DbSet<OrderDetail> Details  { get; set; }



    public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(String roleName) : base(roleName)
        {

        }
    }
}