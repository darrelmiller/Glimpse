using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class GlimpseMessageProcessor : MessageProcessingHandler
    {
        private readonly Factory Factory;
        private readonly DictionaryDataStoreAdapter _HttpServerStore = new DictionaryDataStoreAdapter(new ConcurrentDictionary<string,object>());
        private WebApiServiceLocator providerServiceLocator;

        public GlimpseMessageProcessor()
        {
            providerServiceLocator = new WebApiServiceLocator();

            Factory = new Factory(providerServiceLocator);
            //serviceLocator.Logger = Factory.InstantiateLogger();         
        }

        internal IGlimpseRuntime GetRuntime(HttpRequestMessage requestMessage)
        {

            IGlimpseRuntime runtime;

            if (!requestMessage.Properties.ContainsKey(Constants.RuntimeKey))
            {
                requestMessage.Properties[Constants.HttpServerStoreKey] = _HttpServerStore;

                providerServiceLocator.InitialRequest = requestMessage;  // Provide requestmessage to be able to access HttpServerStore
                runtime = Factory.InstantiateRuntime();
                providerServiceLocator.InitialRequest = null;
    
                requestMessage.Properties[Constants.RuntimeKey]  = runtime;
            } else
            {
                 runtime = requestMessage.Properties[Constants.RuntimeKey] as IGlimpseRuntime;
                
            }

            return runtime;
        }

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken) {
            var runtime = GetRuntime(request);

            var frameworkProvider = new WebApiFrameworkProvider(request);
            frameworkProvider.HttpServerStore = _HttpServerStore;
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
