using DemoAPi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace DemoAPi.Controllers
{
    public class UserController : ApiController
    {
        private DemoStoreEntities2 db = new DemoStoreEntities2();


        public IEnumerable<User> Get()
        {
            return db.Users.ToList();
        }

        [ResponseType(typeof(User))]
        public IHttpActionResult Get(int Id)
        {

            User user = db.Users.Find(Id);
            if (user!=null)
            {
                return NotFound();
            }
            return Ok(user);


        }
    }
}
