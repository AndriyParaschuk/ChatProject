using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatProject.Web.Controllers
{
    public class UserProfileController : Controller
    {
        [HttpPost]
        public ActionResult UserProfile()
        {
            return View();
        }
    }
}