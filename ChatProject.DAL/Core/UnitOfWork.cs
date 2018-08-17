using ChatProject.BL.Interfaces;
using System.Data.Entity;
using ChatProject.BL.Models;
using ChatProject.BL;
using ChatProject.DAL.Repository;
using Ninject;
using System.Reflection;
using Ninject.Parameters;

namespace ChatProject.DAL.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _context;

        public UnitOfWork(ChatContext dbContext)
        {
            _context = dbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public T GetRepository<T>() /*where T : class*/
        {
            using (var kernel = new StandardKernel())
            {
                //kernel.Load(Assembly.GetExecutingAssembly());
                kernel.Bind<IRequestRepository>().To<RequestRepository>();
                kernel.Bind<IMessageRepository>().To<MessageRepository>();
                kernel.Bind<IUserRepository>().To<UserRepository>();
                kernel.Bind<IUserFriendRepository>().To<UserFriendRepository>();
                var result = kernel.Get<T>(new ConstructorArgument("context", _context));
                return result;
            }
        }
    }
}
