using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DemoAPi
{
    public class Global
    {

        public static HttpClient api = new HttpClient();



        static Global()
        {
            api.BaseAddress = new Uri("https://localhost:44337/api");
            api.DefaultRequestHeaders.Clear();
            api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("aplication/json"));

        }

    }
}