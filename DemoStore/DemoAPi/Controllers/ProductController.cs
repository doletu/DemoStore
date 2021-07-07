using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoAPi.Controllers
{
    public class ProductController : ApiController
    {
        public IHttpActionResult getTemp()
        {
            DbModel model = new DbModel();
            var result =model.StoreModel
        }
    }
}
