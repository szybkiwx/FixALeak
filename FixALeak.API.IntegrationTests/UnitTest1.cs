using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FixALeak.API.IntegrationTests
{
    [TestClass]
    public class WebApiTests
    {
        private static IDisposable _webApp;

        private static readonly string baseUrl = "http://*:9000";

        private static WebClient _webClient;

        public static WebClient WebClient {
            get
            {
                _webClient.Headers[HttpRequestHeader.ContentType] = "application/vnd.api+json";
                _webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + _token;
                return _webClient;
            }
            private set
            {
                _webClient = value;
            }
        }

        private static string _token;


        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            _webApp = WebApp.Start<Startup>(baseUrl);

            WebClient = new WebClient()
            {
                BaseAddress = "http://localhost:9000",
            };

            Authorize("Andrzej", "dupaZbita!");
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            WebClient.Dispose();
            _webApp.Dispose();

        }

        public static void Authorize(string username, string password)
        {
            _webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            var authBody = new NameValueCollection();
            authBody.Add("grant_type", "password");
            authBody.Add("username", username);
            authBody.Add("password", password);

            byte[] responseBytes = _webClient.UploadValues("/token", authBody);
            string responseBody = Encoding.UTF8.GetString(responseBytes);

            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);
            _token = result["access_token"];
        }
    }
}
