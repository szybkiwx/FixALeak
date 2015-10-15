using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using FixALeak.API.Models.Auth;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using FixALeak.Service;
using FixALeak.Data;
using FixALeak.JsonApiSerializer;
using FixALeak.API.Models;
using System.Security.Principal;

namespace FixALeak.API
{
    public static class UnityHelpers
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<Startup>();
            container.RegisterType<IUser>(new InjectionFactory(c => c.Resolve<IUser>()));
            container.RegisterType(typeof(IUserStore<>), typeof(UserStore<>));
            container.RegisterType(typeof(UserManager<>), new InjectionConstructor(typeof(IUserStore<>)));
            container.RegisterType<DbContext, AuthContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<IExpenseContext, ExpenseContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<ICategoryLeafService, CategoryLeafService>();
            container.RegisterType<ISingleObjectSerializer, SingleObjectSerializer>();

        }

    }
}