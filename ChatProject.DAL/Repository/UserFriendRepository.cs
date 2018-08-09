using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.DAL.Repository
{
    public class UserFriendRepository : Repository<UserFriend, long>, IUserFriendRepository
    {
        public UserFriendRepository(UnitOfWork uow) : base(uow)
        {
        }
    }
}
