using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class WebApiFrameworkProvider : IFrameworkProvider
    {
        private readonly HttpRequestMessage request;


        public HttpResponseMessage Response { get; set; }

        public WebApiFrameworkProvider(HttpRequestMessage request) {
            this.request = request;
        }

        public IDataStore HttpRequestStore { 
             get { return new DictionaryDataStoreAdapter((IDictionary)request.Properties); }
        
        }
        public IDataStore HttpServerStore { get; private set; }
        public object RuntimeContext { get; private set; }
        public IRequestMetadata RequestMetadata { get; private set; }

        public void SetHttpResponseHeader(string name, string value) {
            Response.Headers.Add(name,value);
        }

        public void SetHttpResponseStatusCode(int statusCode) {
            Response.StatusCode = (HttpStatusCode)statusCode;
        }

        public void SetCookie(string name, string value) {
            var cookies = new CookieHeaderValue[1];
            cookies[0] = new CookieHeaderValue(name, value);
            Response.Headers.AddCookies(cookies);
        }

        public void InjectHttpResponseBody(string htmlSnippet) {

            Response.Content = new PreBodyTagFilter(htmlSnippet, Response.Content, new NullLogger());
        }

        public void WriteHttpResponse(byte[] content) {
            Response.Content = new ByteArrayContent(content);
        }

        public void WriteHttpResponse(string content) {
            Response.Content = new StringContent(content);
        }
    }
}
