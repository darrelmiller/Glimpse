﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Glimpse.WebApi.Sample.Controllers
{
    public class TestController : ApiController
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return new HttpResponseMessage()
                {
                    Content = new StringContent("<html><body>Hello World <a href='test2'>Test2</a></body></html>", Encoding.UTF8, "text/html")
                };
        } 
    }
}