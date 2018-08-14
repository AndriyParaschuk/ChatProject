using ChatProject.BL;
using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;

namespace ChatProject.Web.Controllers
{
    public class UserFriendsController : Controller
    {
        public IUserFriendRepository _userFriendRepository;
        public IUserRepository _userRepository;
        public IRequestRepository _requestRepository;
        public IMessageRepository _messageRepository;

        public UserFriendsController(IRequestRepository requestRepository, IUserFriendRepository userFriendRepository, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _userFriendRepository = userFriendRepository;
            _requestRepository = requestRepository;
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public ActionResult GetUserFriends(string userId)
        //public List<User> GetUserFriends(string userId)
        {
            //List<UserFriend> currentUserFriends = _userFriendRepository.GetAll().Where(item => item.UserId == userId || item.FriendId == userId).ToList();
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

            return Json(new { userFriends = friendsOfUser }, JsonRequestBehavior.AllowGet);
            //return friendsOfUser;
        }

        [HttpGet]
        public ActionResult GetUserRequests(string userId)
        //public List<User> GetUserRequests(string userId)
        {
            List<string> userRequests = _requestRepository.GetAll().Where(item => item.FromId == userId && item.Status == RequestStatus.New)
                .Select(item => item.ToId).ToList();

            List<User> requestsOfUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsOfUser.Add(_userRepository.GetById(id));
            }

            return Json(new { userRequests = requestsOfUser }, JsonRequestBehavior.AllowGet);
            //return requestsOfUser;
        }

        [HttpGet]
        public ActionResult GetRequestsToUser(string userId)
        //public List<User> GetRequestsToUser(string userId)
        {
            List<string> userRequests = _requestRepository.GetAll().Where(item => item.ToId == userId && item.Status == RequestStatus.New)
                .Select(item => item.FromId).ToList();

            List<User> requestsToUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsToUser.Add(_userRepository.GetById(id));
            }

            return Json(new { toUserRequests = requestsToUser }, JsonRequestBehavior.AllowGet);
            //return requestsToUser;
        }

        public ActionResult SearchUser(string userName, string userId)
        //public List<User> SearchUser(string userName)
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
                Request isFromUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.FromId == userId && x.ToId == item.Id);
                if (isFromUserRequest != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "fromUser" });
                }
                Request isToUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.ToId == userId && x.FromId == item.Id);
                if (isToUserRequest != null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "toUser" });
                }
                if(isFriend == null && isFromUserRequest == null && isToUserRequest == null)
                {
                    searchUser.Add(new SearchUser() { User = item, Status = "new" });
                }
            }

            return Json(new { findedUsers = searchUser }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public ActionResult GetMessage(string userId, string withWhomId)
        //public List<User> SearchUser(string userName)
        {
            List<Message> messages = _messageRepository.GetAll().Where(item => (item.ToId == userId && item.FromId == withWhomId) ||
                (item.ToId == withWhomId && item.FromId == userId)).OrderBy(item=> item.Date).ToList();
            return Json(new { chatMessages = messages }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public void PostMessage(string message, string userId, string toWhomId)
        //public List<User> SearchUser(string userName)
        {
            UserFriend userFriend = _userFriendRepository.GetAll().FirstOrDefault(item => (item.UserId == userId && item.FriendId == toWhomId) ||
                (item.UserId == toWhomId && item.FriendId == userId));
            if (userFriend != null)
            {
                _messageRepository.Create(new Message() { FromId = userId, ToId = toWhomId, TextMessage = message });
                _messageRepository.SaveChanges();
            }
            //return Json(new { chatMessages = messages }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public void AcceptRequest(string userId, string withWhomId)
        //public List<User> SearchUser(string userName)
        {
            Request request = _requestRepository.GetAll().FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
            request.Status = RequestStatus.Accept;
            _requestRepository.Update(request);
            _requestRepository.SaveChanges();

            _userFriendRepository.Create(new UserFriend() { UserId = userId, FriendId = withWhomId });
            _userFriendRepository.SaveChanges();
            //return Json(new { chatMessages = messages }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public void DeclineRequest(string userId, string withWhomId)
        //public List<User> SearchUser(string userName)
        {
            Request request = _requestRepository.GetAll().FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
            request.Status = RequestStatus.Decline;
            _requestRepository.Update(request);
            _requestRepository.SaveChanges();
            //return Json(new { chatMessages = messages }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }
    }
}