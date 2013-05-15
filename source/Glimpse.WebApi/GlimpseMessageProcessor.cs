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
        private ILogger _Logger;
        public GlimpseMessageProcessor()
        {
            providerServiceLocator = new WebApiServiceLocator();

            Factory = new Factory(providerServiceLocator);
            providerServiceLocator.Logger = Factory.InstantiateLogger();
            providerServiceLocator.Store = new ApplicationPersistenceStore(_HttpServerStore);
        }

        internal IGlimpseRuntime GetRuntime(HttpRequestMessage requestMessage, IFrameworkProvider frameworkProvider)
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

            if (!runtime.IsInitialized) runtime.InitializeStateless(frameworkProvider);

            return runtime;
        }

        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken) {
         
            var frameworkProvider = new WebApiFrameworkProvider(request);
            frameworkProvider.HttpServerStore = _HttpServerStore;
            frameworkProvider.RequestMetadata = new RequestMetadata(request);
            request.Properties[Constants.FrameworkProviderKey] = frameworkProvider;

            var runtime = GetRuntime(request, frameworkProvider);

            

            runtime.BeginRequestStateless(frameworkProvider);

            return request;
        }

        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken) {
            var frameworkProvider = response.RequestMessage.Properties[Constants.FrameworkProviderKey] as WebApiFrameworkProvider;
            var runtime = GetRuntime(response.RequestMessage,frameworkProvider);
            
            frameworkProvider.Response = response;
            runtime.EndRequestStateless(frameworkProvider);

            return response;
        }


    }
}
