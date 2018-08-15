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
            List<string> usersId = _userFriendRepository.GetAll().Where(item => item.UserId == userId || item.FriendId == userId)
                .Select(item => item.UserId).Where(item => item != userId).ToList();
            List<string> friendsId = _userFriendRepository.GetAll().Where(item => item.UserId == userId || item.FriendId == userId)
                .Select(item => item.FriendId).Where(item => item != userId).ToList();

            List<User> friendsOfUser = new List<User>();
            foreach (string id in usersId)
            {
                friendsOfUser.Add(_userRepository.GetById(id));
            }
            foreach (string id in friendsId)
            {
                friendsOfUser.Add(_userRepository.GetById(id));
            }

            return friendsOfUser;
        }

        public List<User> GetUserRequests(string userId)
        {
            List<string> userRequests = _requestRepository.GetAll().Where(item => item.FromId == userId && item.Status == RequestStatus.New)
                .Select(item => item.ToId).ToList();

            List<User> requestsOfUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsOfUser.Add(_userRepository.GetById(id));
            }

            return requestsOfUser;
        }

        public List<User> GetRequestsToUser(string userId)
        {
            List<string> userRequests = _requestRepository.GetAll().Where(item => item.ToId == userId && item.Status == RequestStatus.New)
                .Select(item => item.FromId).ToList();

            List<User> requestsToUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsToUser.Add(_userRepository.GetById(id));
            }

            return requestsToUser;
        }

        public UserFriend GetUserFriend(string userId, string toWhomId)
        {
            UserFriend userFriend = _userFriendRepository.GetAll().FirstOrDefault(item => (item.UserId == userId && item.FriendId == toWhomId) ||
                (item.UserId == toWhomId && item.FriendId == userId));
            return userFriend;
        }

        public List<SearchUser> SearchUser(string userName, string userId)
        {
            List<User> users = _userRepository.GetAll().Where(item => item.UserName.ToUpper().Contains(userName.ToUpper()) && item.Id != userId).ToList();

            List<SearchUser> searchUser = new List<SearchUser>();

            foreach (User item in users)
            {
                UserFriend isFriend = _userFriendRepository.GetAll().FirstOrDefault(x => (x.FriendId == userId && x.UserId == item.Id) || (x.UserId == userId && x.FriendId == item.Id));
                if (isFriend != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "friend" });
                }
                Request isFromUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.FromId == userId && x.ToId == item.Id && x.Status == RequestStatus.New);
                if (isFromUserRequest != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "fromUser" });
                }
                Request isToUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.ToId == userId && x.FromId == item.Id && x.Status == RequestStatus.New);
                if (isToUserRequest != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "toUser" });
                }
                if (isFriend == null && isFromUserRequest == null && isToUserRequest == null)
                {
                    isFromUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.FromId == userId && x.ToId == item.Id);
                    isToUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.ToId == userId && x.FromId == item.Id);
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
