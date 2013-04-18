using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class GlimpseController
    {
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var runtime = request.Properties[Constants.RuntimeKey] as IGlimpseRuntime;

            if (runtime == null)
            {
                throw new HttpRequestException("Runtime not found");
            }

            var queryString = request.RequestUri.ParseQueryString();

            var resourceName = queryString["n"];


            var frameworkProvider = request.Properties[Constants.FrameworkProviderKey] as WebApiFrameworkProvider;
            frameworkProvider.Response = new HttpResponseMessage();

            if (string.IsNullOrEmpty(resourceName))
            {
                runtime.ExecuteDefaultResource();
            }
            else
            {
                runtime.ExecuteResource(resourceName, new ResourceParameters(queryString.AllKeys.Where(key => key != null).ToDictionary(key => key, key => queryString[key])), runtime.FrameworkProvider);
            }


            return frameworkProvider.Response; // need to get response out of FrameworkProvider, somehow.                                               

            
        }
    }
}
