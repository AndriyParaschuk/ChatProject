using ChatProject.BL.Interfaces;
using ChatProject.BL.Interfaces.Services;
using ChatProject.BL.Models;
using ChatProject.BL.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatProject.BL.Services
{
    public class UserService : IUserService
    {
        IUserFriendRepository _userFriendRepository;
        IUserRepository _userRepository;
        IRequestRepository _requestRepository;

        public UserService(IUserFriendRepository userFriendRepository, IUserRepository userRepository, IRequestRepository requestRepository)
        {
            _userFriendRepository = userFriendRepository;
            _userRepository = userRepository;
            _requestRepository = requestRepository;
        }

        public List<User> GetUserFriends(string userId)
        {
            List<string> usersId = _userFriendRepository.GetUserFriendsId(userId);

            List<User> friendsOfUser = new List<User>();
            foreach (string id in usersId)
            {
                friendsOfUser.Add(_userRepository.GetById(id));
            }

            return friendsOfUser;
        }

        public List<User> GetUserRequests(string userId)
        {
            List<string> userRequests = _requestRepository.GetNewUsersRequests(userId);

            List<User> requestsOfUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsOfUser.Add(_userRepository.GetById(id));
            }

            return requestsOfUser;
        }

        public List<User> GetRequestsToUser(string userId)
        {
            List<string> userRequests = _requestRepository.GetNewRequestsToUser(userId);

            List<User> requestsToUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsToUser.Add(_userRepository.GetById(id));
            }

            return requestsToUser;
        }

        public UserFriend GetUserFriend(string userId, string toWhomId)
        {
            UserFriend userFriend = _userFriendRepository.GetUserFriend(userId, toWhomId);
            return userFriend;
        }

        public List<SearchUser> SearchUser(string userName, string userId)
        {
            List<User> users = _userRepository.SearchUser(userName, userId);

            List<SearchUser> searchUser = new List<SearchUser>();

            foreach (User item in users)
            {
                UserFriend isFriend = _userFriendRepository.GetUserFriend(userId, item.Id);
                if (isFriend != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "friend" });
                }
                Request isFromUserRequest = _requestRepository.GetOneNewRequest(userId, item.Id);
                if (isFromUserRequest != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "fromUser" });
                }
                Request isToUserRequest = _requestRepository.GetOneNewRequestsToUser(userId, item.Id);
                if (isToUserRequest != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "toUser" });
                }
                if (isFriend == null && isFromUserRequest == null && isToUserRequest == null)
                {
                    isFromUserRequest = _requestRepository.GetNewRequests(userId, item.Id);
                    isToUserRequest = _requestRepository.GetNewRequestsToUser(userId, item.Id);
                    if (isFromUserRequest == null && isToUserRequest == null)
                    {
                        searchUser.Add(new SearchUser() { User = item, Status = "new" });
                    }
                }
            }

            return searchUser;
        }
    }
}
