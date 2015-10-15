using FixALeak.JsonApiSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace FixALeak.API.Models
{
    public class JsonApiModelBInder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            //SerializerBuilder.Create().
            throw new NotImplementedException();
        }
    }
}