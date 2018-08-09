using ChatProject.BL;
using ChatProject.BL.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace ChatProject.DAL.Core
{
    public class ChatContext : IdentityDbContext<User>
    {
        public ChatContext()
            : base("ChatContext")
        {
        }

        public static ChatContext Create()
        {
            return new ChatContext();
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserFriend> Friends { get; set; }
    }
}
