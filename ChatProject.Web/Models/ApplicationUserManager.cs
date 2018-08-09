using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatProject.Web.Models
{
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
                : base(store)
        {
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                                                IOwinContext context)
        {
            ChatContext db = context.Get<ChatContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<User>(db));
            //manager.UserValidator = new UserValidator<User>(manager)
            //{
            //    AllowOnlyAlphanumericUserNames = false,
            //    RequireUniqueEmail = true,

            //};
            manager.EmailService = new App_Start.IdentityConfig.EmailService();
            return manager;
        }
    }
}