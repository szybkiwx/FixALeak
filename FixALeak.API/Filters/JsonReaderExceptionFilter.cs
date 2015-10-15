using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http.Filters;

namespace FixALeak.API.Filters
{
    public class JsonReaderExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is JsonReaderException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                base.OnException(actionExecutedContext);
            }
        }
    }
}