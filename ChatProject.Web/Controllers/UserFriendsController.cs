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
                if(isFriend == null && isFromUserRequest == null && isToUserRequest == null)
                {
                    isFromUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.FromId == userId && x.ToId == item.Id);
                    isToUserRequest = _requestRepository.GetAll().FirstOrDefault(x => x.ToId == userId && x.FromId == item.Id);
                    if (isFromUserRequest == null && isToUserRequest == null)
                    {
                        searchUser.Add(new SearchUser() { User = item, Status = "new" });
                    }
                }
            }

            return Json(new { findedUsers = searchUser }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public ActionResult GetMessage(string userId, string withWhomId)
        //public List<Message> GetMessage(string userName)
        {
            List<Message> messages = _messageRepository.GetAll().Where(item => (item.ToId == userId && item.FromId == withWhomId) ||
                (item.ToId == withWhomId && item.FromId == userId)).OrderBy(item=> item.Date).ToList();
            return Json(new { chatMessages = messages }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public ActionResult PostMessage(string message, string userId, string toWhomId)
        //public List<User> PostMessage(string userName)
        {
            UserFriend userFriend = _userFriendRepository.GetAll().FirstOrDefault(item => (item.UserId == userId && item.FriendId == toWhomId) ||
                (item.UserId == toWhomId && item.FriendId == userId));
            Message currentMessage = new Message() { FromId = userId, ToId = toWhomId, TextMessage = message };
            if (userFriend != null)
            {
                _messageRepository.Create(currentMessage);
                _messageRepository.SaveChanges();
            }
 
            return Json(new { oneMessage = currentMessage }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public ActionResult AcceptRequest(string userId, string withWhomId)
        //public List<User> AcceptRequest(string userName)
        {
            Request currentRequest = _requestRepository.GetAll().FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
            currentRequest.Status = RequestStatus.Accept;
            _requestRepository.Update(currentRequest);
            _requestRepository.SaveChanges();

            User currentUser = _userRepository.GetById(userId);
            User friend = _userRepository.GetById(withWhomId);
            UserFriend currentUserFriend = new UserFriend() { FriendId = withWhomId, UserId = userId };
            _userFriendRepository.Create(currentUserFriend);
            _userFriendRepository.SaveChanges();

            return Json(new { user = currentUser, userFriend = friend }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public ActionResult DeclineRequest(string userId, string withWhomId)
        //public List<User> DeclineRequest(string userName)
        {
            Request request = _requestRepository.GetAll().FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
            request.Status = RequestStatus.Decline;
            _requestRepository.Update(request);
            _requestRepository.SaveChanges();

            User currentUser = _userRepository.GetById(userId);
            User friend = _userRepository.GetById(withWhomId);

            return Json(new { user = currentUser, userFriend = friend }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }

        public ActionResult CreateRequest(string userId, string toWhomId)
        //public List<User> DeclineRequest(string userName)
        {
            _requestRepository.Create(new Request() { FromId = userId, ToId = toWhomId });
            _requestRepository.SaveChanges();

            User currentUser = _userRepository.GetById(userId);
            User userToWhomSend = _userRepository.GetById(toWhomId);
            return Json(new { user = currentUser, userToWhomSendRequest = userToWhomSend }, JsonRequestBehavior.AllowGet);
            //return findedUsers;
        }
    }
}