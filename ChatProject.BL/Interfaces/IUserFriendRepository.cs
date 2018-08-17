using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces
{
    public interface IUserFriendRepository : IRepository<UserFriend, long>
    {
        List<string> GetUserFriendsId(string userId);

        UserFriend GetUserFriend(string userId, string toWhomId);
    }
}
