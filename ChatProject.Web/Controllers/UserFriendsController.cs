using ChatProject.BL.Interfaces;
using ChatProject.BL.Interfaces.Services;
using ChatProject.BL.Models;
//using System.Web.Http;
using System.Web.Mvc;

namespace ChatProject.Web.Controllers
{
    public class UserFriendsController : Controller
    {
        public IUserRepository _userRepository;

        public IUserService _userService;
        public IRequestService _requestService;
        public IMessageService _messageService;

        public UserFriendsController(IUserRepository userRepository, IUserService userService, IRequestService requestService, IMessageService messageService)
        {
            _userRepository = userRepository;

            _userService = userService;
            _requestService = requestService;
            _messageService = messageService;
        }

        [HttpGet]
        public ActionResult GetUserFriends(string userId)
        //public List<User> GetUserFriends(string userId)
        {
            return Json(new { userFriends = _userService.GetUserFriends(userId) }, JsonRequestBehavior.AllowGet);
            //return friendsOfUser;
        }

        [HttpGet]
        public ActionResult GetUserRequests(string userId)
        {
            return Json(new { userRequests = _userService.GetUserRequests(userId) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRequestsToUser(string userId)
        {
            return Json(new { toUserRequests = _userService.GetRequestsToUser(userId) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchUser(string userName, string userId)
        {
            return Json(new { findedUsers = _userService.SearchUser(userName, userId) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMessage(string userId, string withWhomId)
        {
            return Json(new { chatMessages = _messageService.GetMessage(userId, withWhomId) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostMessage(string message, string userId, string toWhomId)
        {
            if (_userService.GetUserFriend(userId, toWhomId) != null)
            {
                return Json(new { oneMessage = _messageService.PostMessage(message, userId, toWhomId) }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public ActionResult AcceptRequest(string userId, string withWhomId)
        {
            _requestService.AcceptRequest(userId, withWhomId);

            User currentUser = _userRepository.GetById(userId);
            User friend = _userRepository.GetById(withWhomId);

            return Json(new { user = currentUser, userFriend = friend }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeclineRequest(string userId, string withWhomId)
        {
            _requestService.DeclineRequest(userId, withWhomId);

            User currentUser = _userRepository.GetById(userId);
            User friend = _userRepository.GetById(withWhomId);

            return Json(new { user = currentUser, userFriend = friend }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateRequest(string userId, string toWhomId)
        {
            _requestService.CreateRequest(userId, toWhomId);

            User currentUser = _userRepository.GetById(userId);
            User userToWhomSend = _userRepository.GetById(toWhomId);

            return Json(new { user = currentUser, userToWhomSendRequest = userToWhomSend }, JsonRequestBehavior.AllowGet);
        }
    }
}