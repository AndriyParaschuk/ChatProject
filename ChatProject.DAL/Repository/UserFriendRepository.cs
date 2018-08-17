using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ChatProject.DAL.Repository
{
    public class UserFriendRepository : Repository<UserFriend, long>, IUserFriendRepository
    {
        public UserFriendRepository(ChatContext context) : base(context)
        {
        }

        public List<string> GetUserFriendsId(string userId)
        {
            List<string> usersId = _dbSet.Where(item => item.UserId == userId || item.FriendId == userId)
                .Select(item => item.UserId).Where(item => item != userId).ToList();
            List<string> friendsId = _dbSet.Where(item => item.UserId == userId || item.FriendId == userId)
                .Select(item => item.FriendId).Where(item => item != userId).ToList();

            usersId.AddRange(friendsId);
            return usersId;
        }

        public UserFriend GetUserFriend(string userId, string toWhomId)
        {
            return _dbSet.FirstOrDefault(item => (item.UserId == userId && item.FriendId == toWhomId) ||
                (item.UserId == toWhomId && item.FriendId == userId));
        }
    }
}
