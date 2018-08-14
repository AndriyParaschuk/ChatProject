using ChatProject.BL;
using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using ChatProject.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatProject.Web.Controllers
{
    public class HomeController : Controller
    {
        public IRequestRepository _requestRepository;
        public IMessageRepository _messageRepository;
        public IUserRepository _userRepository;
        public IUserFriendRepository _userFriendRepository;
        public HomeController(IRequestRepository requestRepository, IUserRepository userRepository, IUserFriendRepository userFriendRepository, IMessageRepository messageRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _userFriendRepository = userFriendRepository;
            _messageRepository = messageRepository;
        }

        public ActionResult Index()
        {
            //using (ChatContext db = new ChatContext())
            //{
            //    List<User> users = db.Users.ToList();
            //    Request request = new Request() { Id = 1, Date = DateTime.Now, From = users[0], To = users[1] };
            //    db.Requests.Add(request);
            //    db.SaveChanges();
            //}

            //_requestRepository.Create(new Request { });
            //_requestRepository.Create(new Request { });
            //_requestRepository.Create(new Request { });
            //_requestRepository.SaveChanges();

            //List<User> users = _userRepository.GetAll().ToList();
            //_requestRepository.Create(new Request { FromId = users[0].Id, ToId = users[1].Id });
            //_requestRepository.Create(new Request { FromId = users[3].Id, ToId = users[2].Id });
            //_requestRepository.SaveChanges();
            //_userFriendRepository.Create(new UserFriend { UserId = users[0].Id, FriendId = users[1].Id });
            //_userFriendRepository.Create(new UserFriend { UserId = users[2].Id, FriendId = users[0].Id });
            //_userFriendRepository.SaveChanges();
            //_messageRepository.Create(new Message() { FromId = users[3].Id, ToId = users[0].Id, TextMessage = "hello" });
            //_messageRepository.Create(new Message() { FromId = users[0].Id, ToId = users[3].Id, TextMessage = "hello)" });
            //_messageRepository.SaveChanges();
            return View();
        }
    }
}