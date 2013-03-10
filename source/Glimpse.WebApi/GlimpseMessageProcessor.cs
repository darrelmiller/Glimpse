using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class GlimpseMessageProcessor : MessageProcessingHandler
    {
        private readonly Factory Factory;

        public GlimpseMessageProcessor(HttpConfiguration config)
        {
            
            //var serviceLocator = new WebApiServiceLocator();
            Factory = new Factory((IServiceLocator)config.DependencyResolver);
            //serviceLocator.Logger = Factory.InstantiateLogger();         
        }

        internal IGlimpseRuntime GetRuntime(HttpRequestMessage requestMessage)
        {
            var runtime = requestMessage.Properties[Constants.RuntimeKey] as IGlimpseRuntime;

            if (runtime == null)
            {
                    runtime = Factory.InstantiateRuntime();
                    requestMessage.Properties[Constants.RuntimeKey]  = runtime;
            }

            return runtime;
        }

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken) {
            var runtime = GetRuntime(request);

            var frameworkProvider = new WebApiFrameworkProvider(request);
            request.Properties[Constants.FrameworkProviderKey] = frameworkProvider;
            runtime.BeginRequest(frameworkProvider);

            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken) {
            var runtime = GetRuntime(response.RequestMessage);
            var frameworkProvider = response.RequestMessage.Properties[Constants.FrameworkProviderKey] as WebApiFrameworkProvider;
            frameworkProvider.Response = response;
            runtime.EndRequest(frameworkProvider);

            return response;
        }


    }
}
