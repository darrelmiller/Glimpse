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
        private HttpResponseMessage response;
        private RequestMetadata requestMetadata;

        public HttpResponseMessage Response
        {
            get { return response; }
            set { response = value;
                requestMetadata.ResponseMessage = response;
            }
        }

        public WebApiFrameworkProvider(HttpRequestMessage request) {
            this.request = request;
        }

        public IDataStore HttpRequestStore { 
             get { return new DictionaryDataStoreAdapter((IDictionary)request.Properties); }
        
        }
        public IDataStore HttpServerStore { get; set; }
        public object RuntimeContext { get { return request.Properties; } 
        }
        public IRequestMetadata RequestMetadata
        {
            get { return requestMetadata; }
            set { requestMetadata = value as RequestMetadata; }
        }

        public void SetHttpResponseHeader(string name, string value) {
            if (name == "Content-Type")
            {
                //Response.Content.Headers.ContentType = new MediaTypeHeaderValue(value);
            }
            else
            {
                Response.Headers.TryAddWithoutValidation(name, value);
            }
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
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        }

        public void WriteHttpResponse(byte[] content) {
            Response.Content = new ByteArrayContent(content);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        }

        public void WriteHttpResponse(string content) {
            Response.Content = new StringContent(content);
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
        }
    }
}
