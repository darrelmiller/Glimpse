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
        private ILogger logger;

        public HttpRequestMessage InitialRequest { get; set; }

        internal ILogger Logger
        {
            get { return logger ?? (logger = new NullLogger()); }
            set { logger = value; }
        }

        internal IPersistenceStore Store { get; set; }

        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);
            if (type == typeof(IFrameworkProvider))
            {
                return new WebApiFrameworkProvider(InitialRequest) as T;
            }

            if (type == typeof(ILogger))
            {
                return Logger as T;
            }
            if (type == typeof(IPersistenceStore))
            {
                return Store as T;
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
}
