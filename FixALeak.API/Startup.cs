using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using FixALeak.JsonApiSerializer;
using System.Security.Principal;
using FixALeak.API.Models;
using FixALeak.API.Filters;

[assembly: OwinStartup(typeof(FixALeak.API.Startup))]
namespace FixALeak.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var jsonApiFormatter = new JsonApiMediaTypeFormatter();
            jsonApiFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/vnd.api+json") );
            config.Formatters.Add(jsonApiFormatter);

            config.DependencyResolver = new UnityDependencyResolver.Lib.UnityWebApiDependencyResolver(UnityHelpers.GetConfiguredContainer());
            config.BindParameter(typeof(IPrincipal), new IPrincipalModelBinder());
            config.Filters.Add(new JsonReaderExceptionFilter());

            

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureOAuth(app);
            ConfigureSerializer();

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }

        private void ConfigureSerializer()
        {
            SerializerConfiguration.Prefix = "api";
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            
            var serverOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new FixALeakAuthServerProvider(UnityHelpers.GetConfiguredContainer().Resolve<UserManager<IdentityUser>>())
            };

            app.UseOAuthAuthorizationServer(serverOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}