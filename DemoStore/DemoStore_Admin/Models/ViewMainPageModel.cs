using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoStore_Admin.Models
{
    public class ViewMainPageModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public string Role { get; set; }
     

    }
}