using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace FixALeak.API.IntegrationTests
{
    [TestClass]
    public class CategoryCRUD
    {

        private static string API_BASE = "api/categories";

        [TestMethod]
        public void Test_AddCategory()
        {
            var newCategory = new GenericJsonApiObject()
            {
                Data = new Data()
                {
                    Attributes = new Dictionary<string, object>()
                    {
                        { "name",  string.Format("test upload {0}", Guid.NewGuid()) },
                    },
                    Type = "categories"
                }
            };

            string resultString = WebApiTests.WebClient.UploadString(API_BASE, JsonConvert.SerializeObject(newCategory));
            var addedCategory = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);


            

            resultString = WebApiTests.WebClient.DownloadString(API_BASE);
            var catList = JsonConvert.DeserializeObject<GenericJsonApiCollection>(resultString);


            string update = @"{ 
                ""data"": {
                    ""attributes"" : {
                        ""name"" : ""abc""
                    }
                }
            }";

            resultString = WebApiTests.WebClient.UploadString(API_BASE + "/" + addedCategory.Data.Id, "PATCH", update);
            resultString = WebApiTests.WebClient.DownloadString(API_BASE + "/" + addedCategory.Data.Id);

            var updatedCategory = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);

            Assert.AreEqual("abc", updatedCategory.Data.Attributes["name"]);

            resultString = WebApiTests.WebClient.UploadString(API_BASE + "/" + addedCategory.Data.Id, "DELETE", "");
            try {
                WebApiTests.WebClient.DownloadString(API_BASE + "/" + addedCategory.Data.Id);
                Assert.Fail();
            }
            catch(WebException e)
            {

            }

        }
    }
}
