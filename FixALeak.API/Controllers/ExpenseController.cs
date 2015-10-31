using FixALeak.Data.Entities;
using FixALeak.JsonApiSerializer;
using FixALeak.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace FixALeak.API.Controllers
{
    [RoutePrefix("api/expenses")]
    [Authorize]
    public class ExpenseController : ApiController
    {
        private IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [Route("")]
        [HttpGet]   
        public IHttpActionResult GetAll(int leaf)
        {
            var serializer =  SerializerBuilder.Create();
            var expenses =  _expenseService.GetExpenses(leaf);
            return Ok(serializer.Serialize(expenses));
        }

        [Route("{id:int}")]
        [HttpGet]
        public IHttpActionResult Get(int id, int leaf)
        {
            var serializer = SerializerBuilder.Create();
            var expense = _expenseService.Get(id);
            if(expense == null)
            {
                return NotFound();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(serializer.Serialize(expense).ToString());
            return ResponseMessage(response);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Add(HttpRequestMessage request, int leaf)
        {
            var serializer = SerializerBuilder.Create();
            var content = request.Content.ReadAsStringAsync().Result;
            var expense = serializer.Deserialize<Expense>(content);

            expense.CategoryLeafID = leaf;
           
            var created = _expenseService.Add(expense);
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new StringContent(serializer.Serialize(created).ToString());
            return ResponseMessage(response);
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id, int leaf)
        {
            if(!_expenseService.Exists(id))
            {
                return NotFound();
            }

            _expenseService.Remove(id);
            return Ok();
        }

    }
}
