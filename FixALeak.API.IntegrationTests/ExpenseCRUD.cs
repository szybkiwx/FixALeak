using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FixALeak.API.IntegrationTests
{
    [TestClass]
    public class ExpenseCRUD
    {

        /*private static string API_BASE = "api/expenses";

        [TestMethod]
        public void Test_AddExpense()
        {
            var newCategory = new GenericJsonApiObject()
            {
                Data = new Data()
                {
                    Attributes = new Dictionary<string, object>()
                    {
                        { "name",  string.Format("test upload {0}", Guid.NewGuid()) },
                    },
                    Type = "expenses"
                }
            };

            string resultString = WebApiTests.WebClient.UploadString(API_BASE, JsonConvert.SerializeObject(newCategory));
            var addedCategory = JsonConvert.DeserializeObject<GenericJsonApiObject>(resultString);

        }*/
    }
}
