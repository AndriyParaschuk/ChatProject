using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChatProject.DAL.Repository
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(ChatContext context) : base(context)
        {
        }

        public List<User> SearchUser(string userName, string userId)
        {
            return _dbSet.Where(item => item.UserName.ToUpper().Contains(userName.ToUpper()) && item.Id != userId).ToList();
        }
    }
}
