using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using FixALeak.TestApi.Resources;
using Newtonsoft.Json;

namespace FixALeak.TestApi
{
    public class Client
    {
        WebClient _webClient;
        string _token;
        public Client(string baseUrl = "http://localhost:25366")
        {
            _webClient = new WebClient()
            {
                BaseAddress = baseUrl,
            };
        }

       

        public void Authorize(string username, string password) {
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

        public void Register(string username, string password)
        {
            _webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            string data = JsonConvert.SerializeObject(
                new UserModel() 
                { 
                    UserName = username,
                    Password = password, 
                    ConfirmPassword = password 
                });
            _webClient.UploadString("/api/Account", data);
        }

        public List<Category> GetCategories()
        {
            _webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            _webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + _token;
            string data = _webClient.DownloadString("/api/categories?include=subcategories");
            return null;
        }



        public void AddCategory()
        {
            _webClient.Headers[HttpRequestHeader.ContentType] = "application/vnd.api+json";
            _webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + _token;

            string newCategoryJson = @"
               {
                    ""data"": {
                        ""type"": ""categories"",
                        ""attributes"": {
                            ""name"": ""test upload""
                        }
                    }
                }
            ";

            try {
                _webClient.UploadString("/api/categories", newCategoryJson);
            }
            catch (Exception e)
            {

            } 
        }
    }
}
