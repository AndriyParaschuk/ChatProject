using ChatProject.BL.Models;
using ChatProject.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatProject.Web.Controllers
{
    public class UserChatController : Controller
    {
        public ActionResult ChatPage()
        {
            return View();
        }
    }
}