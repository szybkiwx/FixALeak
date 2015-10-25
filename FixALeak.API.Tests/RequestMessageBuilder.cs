using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FixALeak.API.Tests
{
    public class RequestMessageBuilder
    {
        private static string baseUrl = "http://localhost";

        private HttpMethod method = HttpMethod.Get;

        private string url;

        private string body;

        public RequestMessageBuilder()
        {
            url = baseUrl;
        }

        public RequestMessageBuilder WithMethod(HttpMethod method)
        {
            this.method = method;
            return this;
        }

        public RequestMessageBuilder WithUrl(string url)
        {
            this.url = baseUrl + url;
            return this;
        }

        public RequestMessageBuilder WithAbsoluteUrl(string url)
        {
            this.url = url;
            return this;
        }

        public RequestMessageBuilder WithBody(string body)
        {
            this.body = body;
            return this;
        }

        public HttpRequestMessage Build()
        {
            var request = new HttpRequestMessage(method, url);
            if (body != null)
            {
                request.Content = new StringContent(body);
            }
            return request;
        }
    }
}
