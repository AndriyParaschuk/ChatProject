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
        public IUserRepository _userRepository;
        public HomeController(IRequestRepository requestRepository, IUserRepository userRepository)
        {
            _requestRepository = requestRepository;
            _userRepository = userRepository;
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
            //_requestRepository.SaveChanges();
            return View();
        }
    }
}