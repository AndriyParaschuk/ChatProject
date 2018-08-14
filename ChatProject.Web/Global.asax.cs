using ChatProject.BL.Interfaces;
using ChatProject.DAL.Core;
using ChatProject.DAL.Repository;
using Ninject;
using Ninject.Web.Mvc;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApiContrib.IoC.Ninject;


namespace ChatProject.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var ninjectKernel = new StandardKernel();

            ConfigureDependencies(ninjectKernel);

            DependencyResolver.SetResolver(new NinjectDependencyResolver(ninjectKernel));
        }

        private void ConfigureDependencies(StandardKernel ninjectKernel)
        {
            ninjectKernel.Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(@"Data Source=CH001;Initial Catalog=Chat;Integrated Security=True"/*FilePaths.connectionString*/);
            ninjectKernel.Bind<IUserFriendRepository>().To<UserFriendRepository>();
            ninjectKernel.Bind<IMessageRepository>().To<MessageRepository>();
            ninjectKernel.Bind<IRequestRepository>().To<RequestRepository>();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(ninjectKernel);
        }
    }
}
