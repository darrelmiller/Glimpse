using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Dependencies;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class WebApiServiceLocator : IServiceLocator
    {

        public HttpRequestMessage InitialRequest { get; set; }

        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);
            if (type == typeof(IFrameworkProvider))
            {
                return new WebApiFrameworkProvider(InitialRequest) as T;
            }

            if (type == typeof(ResourceEndpointConfiguration))
            {
                return new WebApiEndpointConfiguration() as T;
            }

            return null;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }
    }

    public class WebApiEndpointConfiguration : ResourceEndpointConfiguration
    {
        protected override string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            return "";  // I have no idea what this does
        }
    }
}
