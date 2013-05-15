using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Glimpse.Core.Framework;
using Microsoft.Practices.Unity;
using WebApiContrib.IoC.Unity;

namespace Glimpse.WebApi.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = "http://localhost:8080/";
            var config = new HttpSelfHostConfiguration(baseAddress);

            config.Routes.MapHttpRoute("glimpse", "glimpse.axd",new {controller="Glimpse"});
            config.Routes.MapHttpRoute("default", "{controller}");
            
            config.MessageHandlers.Add(new GlimpseMessageProcessor());

            var server = new HttpSelfHostServer(config);

            server.OpenAsync().Wait();

            Console.WriteLine("Browse to " + baseAddress + "test");
            Console.WriteLine("or " + baseAddress + "glimpse.axd");
            Console.Read();

            server.CloseAsync().Wait();

        }

    }
}
