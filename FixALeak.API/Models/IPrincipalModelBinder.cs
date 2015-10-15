using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
namespace FixALeak.API.Models
{
    public class IPrincipalModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {

            bindingContext.Model = actionContext.ControllerContext.RequestContext.Principal;
            return true;
        }
    }
}