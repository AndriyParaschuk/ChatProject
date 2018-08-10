using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatProject.Web.Controllers
{
    public class UserFriendsController : Controller
    {
        public IUserFriendRepository _userFriendRepository;
        public IUserRepository _userRepository;
        public IRequestRepository _requestRepository;

        public UserFriendsController(IRequestRepository requestRepository, IUserFriendRepository userFriendRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userFriendRepository = userFriendRepository;
            _requestRepository = requestRepository; 
        }

        public ActionResult GetUserFriends(string userId)
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
        }

        public ActionResult GetUserRequests(string userId)
        {
            List<string> userRequests = _requestRepository.GetAll().Where(item => item.FromId == userId && item.Status == RequestStatus.New)
                .Select(item => item.ToId).ToList();

            List<User> requestsOfUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsOfUser.Add(_userRepository.GetById(id));
            }

            return Json(new { userRequests = requestsOfUser }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRequestsToUser(string userId)
        {
            List<string> userRequests = _requestRepository.GetAll().Where(item => item.ToId == userId && item.Status == RequestStatus.New)
                .Select(item => item.FromId).ToList();

            List<User> requestsToUser = new List<User>();
            foreach (string id in userRequests)
            {
                requestsToUser.Add(_userRepository.GetById(id));
            }

            return Json(new { toUserRequests = requestsToUser }, JsonRequestBehavior.AllowGet);
        }
    }
}