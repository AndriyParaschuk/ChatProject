using ChatProject.BL.Interfaces;
using ChatProject.DAL.Core;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatProject.Web.UserIdProvider
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            using (ChatContext db = new ChatContext())
            {
                var userId = "";
                if (request.User.Identity.Name != "")
                {
                    userId = db.Users.FirstOrDefault(item => item.UserName == request.User.Identity.Name).Id;
                }
                return userId.ToString();
            }
        }
    }
}