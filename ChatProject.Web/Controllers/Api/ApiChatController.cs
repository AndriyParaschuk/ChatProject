using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace ChatProject.Web.Controllers.Api
{
    //[Route("api/userrequest")]
    public class ApiChatController : ApiController
    {
        public IUserFriendRepository _userFriendRepository;
        public IUserRepository _userRepository;
        public IRequestRepository _requestRepository;

        public ApiChatController(IRequestRepository requestRepository, IUserFriendRepository userFriendRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userFriendRepository = userFriendRepository;
            _requestRepository = requestRepository;
        }

        //public ActionResult GetUserFriends(string userId)
        [HttpGet]
        [Route("chat/user")]
        public List<User> Get(/*string userId*/)
        {
            //List<UserFriend> currentUserFriends = _userFriendRepository.GetAll().Where(item => item.UserId == userId || item.FriendId == userId).ToList();
            //List<string> usersId = _userFriendRepository.GetAll().Where(item => item.UserId == userId || item.FriendId == userId)
            //    .Select(item => item.UserId).Where(item => item != userId).ToList();
            //List<string> friendsId = _userFriendRepository.GetAll().Where(item => item.UserId == userId || item.FriendId == userId)
            //    .Select(item => item.FriendId).Where(item => item != userId).ToList();

            //List<User> friendsOfUser = new List<User>();
            //foreach (string id in usersId)
            //{
            //    friendsOfUser.Add(_userRepository.GetById(id));
            //}
            //foreach (string id in friendsId)
            //{
            //    friendsOfUser.Add(_userRepository.GetById(id));
            //}
            return _userRepository.GetAll().ToList();

            //return Json(new { userFriends = friendsOfUser }, JsonRequestBehavior.AllowGet);
           // return friendsOfUser;
        }
    }
}
