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

        ICategoryLeafService _categoryLeafService;

        public ExpenseController(IExpenseService expenseService, ICategoryLeafService categoryLeafService)
        {
            _expenseService = expenseService;
            _categoryLeafService = categoryLeafService;
        }

        [Route("")]
        [HttpGet]   
        public IHttpActionResult GetAll(int leaf)
        {
            var serializer =  SerializerBuilder.Create();
            var expenses =  _expenseService.GetExpenses(leaf);
            return Ok(serializer.Serialize(expenses));
        }

        [Route("{id:int}", Name = "GetExpense")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var expense = _expenseService.Get(id);
            if(expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult Add(Expense expense)
        {
            if(_categoryLeafService.Get(expense.CategoryLeafID) == null)
            {
                return BadRequest("Category leaf does not exist");
            }
           
            var created = _expenseService.Add(expense);
            return Created(Url.Link("GetExpense", new { id = created.ID }), created);
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

        [Route("{id:int}")]
        [HttpPatch]
        public IHttpActionResult Update(int id, 
        {
            if (!_expenseService.Exists(id))
            {
                return NotFound();
            }

            _expenseService.Remove(id);
            return Ok();
        }

    }
}
