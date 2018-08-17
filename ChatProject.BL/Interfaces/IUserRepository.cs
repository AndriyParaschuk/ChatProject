using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces
{
    public interface IUserRepository : IRepository<User, string>
    {
        List<User> SearchUser(string userName, string userId);
    }
}
