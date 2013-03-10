using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Glimpse.Core.Extensibility;

namespace Glimpse.WebApi {
    public class PreBodyTagFilter : HttpContent {

        private readonly HttpContent _OutputContent;

        public PreBodyTagFilter(string htmlSnippet, HttpContent inputContent, ILogger logger)
        {
#if NET35
            HtmlSnippet = Glimpse.AspNet.Net35.Backport.Net35Backport.IsNullOrWhiteSpace(htmlSnippet) ? string.Empty : htmlSnippet + "</body>";
#else
            HtmlSnippet = string.IsNullOrWhiteSpace(htmlSnippet) ? string.Empty : htmlSnippet + "</body>";
#endif


            BodyEnd = new Regex("</body>", RegexOptions.Compiled | RegexOptions.Multiline);
            Logger = logger;


            string contentInBuffer = inputContent.ReadAsStringAsync().Result;  // This is probably a really bad idea!

            if (BodyEnd.IsMatch(contentInBuffer))
            {
                string bodyCloseWithScript = BodyEnd.Replace(contentInBuffer,
                                                             HtmlSnippet);

                _OutputContent = new StringContent(bodyCloseWithScript,Encoding.GetEncoding(inputContent.Headers.ContentEncoding.FirstOrDefault()));
            }
            else
            {
                var contentEncoding = String.Join(",", inputContent.Headers.ContentEncoding);
                Logger.Warn("Unable to locate '</body>' with content encoding '{0}'. Response may be compressed.",
                            contentEncoding);
                _OutputContent = inputContent;
            }
        }


        private ILogger Logger { get; set; }
        private string HtmlSnippet { get; set; }
        private Regex BodyEnd { get; set; }


        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) {
      
            return _OutputContent.CopyToAsync(stream);

        }
        protected override bool TryComputeLength(out long length) {
            var nullableLength = _OutputContent.Headers.ContentLength;
        
            if (nullableLength != null) {
                length =   (long)nullableLength;
                return true;    
            }

            length = -1;
            return false;
        }
    }
}