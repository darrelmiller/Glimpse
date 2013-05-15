using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using Glimpse.Core.Framework;

namespace Glimpse.WebApi
{
    public class RequestMetadata : IRequestMetadata
    {
        public  HttpRequestMessage RequestMessage { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }

        public RequestMetadata(HttpRequestMessage requestMessage)
        {
            
            if (requestMessage == null)
            {
                throw new ArgumentNullException("requestMessage");
            }
            RequestMessage = requestMessage;
        }

        public string RequestHttpMethod
        {
            get { return RequestMessage.Method.ToString(); }
        }

        public int ResponseStatusCode
        {
            get { return (int)ResponseMessage.StatusCode; }
        }

        public string ResponseContentType
        {
            get { return RequestMessage.Content != null && RequestMessage.Content.Headers.ContentType != null ? RequestMessage.Content.Headers.ContentType.ToString() : string.Empty; }
        }

        public string IpAddress
        {
            get
            {
                throw new NotImplementedException("Need to implement this IP logic");
            }
        }

        public bool RequestIsAjax
        {
            get
            {
                var request = RequestMessage.Headers;

                //if (request.GetValues("X-Requested-With"). == "XMLHttpRequest")
                //{
                //    return true;
                //}

                //if (request.Headers != null)
                //{
                //    return request.Headers["X-Requested-With"] == "XMLHttpRequest";
                //}

                return false;
            }
        }

        public string ClientId
        {
            get
            {
            //    string user = Context.User.Identity.Name;

            //    if (!string.IsNullOrEmpty(user))
            //    {
            //        return user;
            //    }

            //    var browser = Context.Request.Browser;

            //    if (browser != null)
            //    {
            //        return string.Format("{0} {1}", browser.Browser, browser.Version);
            //    }

                return Guid.NewGuid().ToString("N");
            }
        }

        public string RequestUri
        {
            get { return RequestMessage.RequestUri.AbsoluteUri; }
        }



        public string GetCookie(string name)
        {
            return null;
            //var cookie = Context.Request.Cookies.Get(name);

            //return cookie == null ? null : cookie.Value;
        }

        public string GetHttpHeader(string name)
        {
            return RequestMessage.Headers.GetValues(name).FirstOrDefault();
        }
    }
}
