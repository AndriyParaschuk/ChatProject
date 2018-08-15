using ChatProject.BL.Models;
using ChatProject.BL.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces.Services
{
    public interface IUserService
    {
        List<User> GetUserFriends(string userId);

        List<User> GetUserRequests(string userId);

        List<User> GetRequestsToUser(string userId);

        UserFriend GetUserFriend(string userId, string toWhomId);

        List<SearchUser> SearchUser(string userName, string userId);
    }
}
