using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models.Prduct
{
    public class UserValidate : ValidationAttribute
    {
        public readonly ApplicationDbContext dbContext = new ApplicationDbContext();
        public override bool IsValid(object value)
        {
            var item = dbContext.Users.FirstOrDefault(p=>p.Name==value.ToString());
            if (item==null)
            {
                return false;
            }

            return true;
        }


    }
}