using System.Data.Entity;
using System.Web.Mvc;
using Gira.Business;
using Gira.Business.Interfaces;
using Gira.Controllers;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Gira.Data.Repositories.Instances;
using Gira.Data.Repositories.Interfaces;
using Gira.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace Gira
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            //Repo's
            container.RegisterType<DbContext, GiraDbContext>();
            container.RegisterType<IGiraUoW, GiraUoW>();
            container.RegisterType<GiraDbContext>();
            container.RegisterType<GiraInitializer>();

            //services
            container.RegisterType<IStateMachine<IssueStatusCode, IssueTransition>, IssueStateMachine>();
            container.RegisterType<ITransitionService, TransitionService>();

            //identity
            container.RegisterType<DbContext, GiraDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<RoleController>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}