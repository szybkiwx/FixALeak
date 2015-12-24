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

        private static string API_CATEGORIES = "api/categories";
        private static string API_CATEGORY_LEAVES = "api/categoryleaves";

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

            string resultString = WebApiTests.WebClient.UploadString(API_CATEGORIES, JsonConvert.SerializeObject(newCategory));
            var addedCategory = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);


            

            resultString = WebApiTests.WebClient.DownloadString(API_CATEGORIES);
            var catList = JsonConvert.DeserializeObject<GenericJsonApiCollection>(resultString);


            var update = new { 
                data = new {
                    attributes = new {
                        name = "abc"
                    }
                }
            };

            resultString = WebApiTests.WebClient.UploadString(API_CATEGORIES + "/" + addedCategory.Data.Id, "PATCH", JsonConvert.SerializeObject(update));
            resultString = WebApiTests.WebClient.DownloadString(API_CATEGORIES + "/" + addedCategory.Data.Id);

            var updatedCategory = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);

            Assert.AreEqual("abc", updatedCategory.Data.Attributes["name"]);

            resultString = WebApiTests.WebClient.UploadString(API_CATEGORIES + "/" + addedCategory.Data.Id, "DELETE", "");
            try {
                WebApiTests.WebClient.DownloadString(API_CATEGORIES + "/" + addedCategory.Data.Id);
                Assert.Fail();
            }
            catch(WebException)
            {
            }

        }


        [TestMethod]
        public void Test_AddCategoryLeaves()
        {
            var newCategory = new
            {
                data = new
                {
                    attributes = new
                    {
                        name = string.Format("test upload {0}", Guid.NewGuid())
                    },
                    type = "categories"
                }
            };

            string resultString = WebApiTests.WebClient.UploadString(API_CATEGORIES, JsonConvert.SerializeObject(newCategory));
            var addedCategory = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);

            var newCategoryLeaf = new 
            {
                data = new  
                {
                    attributes = new Dictionary<string, object>()
                    {
                        { "name",  "category leaf 1"},
                    },
                    type = "categoryleaves",
                    relationships = new {
                        category = new
                        {
                            data = new
                            {
                                type = "categories",
                                id = addedCategory.Data.Id
                            }
                        }
                    }

                },
                
            };

            resultString = WebApiTests.WebClient.UploadString(API_CATEGORY_LEAVES, JsonConvert.SerializeObject(newCategoryLeaf));
            var addedCategoryLeaf1 = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);

            newCategoryLeaf.data.attributes["name"] = "category leaf 2";

            resultString = WebApiTests.WebClient.UploadString(API_CATEGORY_LEAVES, JsonConvert.SerializeObject(newCategoryLeaf));
            var addedCategoryLeaf2 = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);
            resultString = WebApiTests.WebClient.DownloadString(API_CATEGORIES + "/" + addedCategory.Data.Id + "/relationships/categoryleaves");
            var leavesResult = JsonConvert.DeserializeObject<GenericJsonApiCollection>(resultString);

            var leafData = new List<Data>(leavesResult.Data);
            Assert.AreEqual(2, leafData.Count);


            WebApiTests.WebClient.UploadString(API_CATEGORY_LEAVES + "/" + addedCategoryLeaf1.Data.Id, "DELETE", "");
            WebApiTests.WebClient.UploadString(API_CATEGORY_LEAVES + "/" + addedCategoryLeaf2.Data.Id, "DELETE", "");

            WebApiTests.WebClient.UploadString(API_CATEGORIES + "/" + addedCategory.Data.Id, "DELETE", "");




        }
    }
}
