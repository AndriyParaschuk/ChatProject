using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.DAL.Repository
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(UnitOfWork uow) : base(uow)
        {
        }
    }
}
