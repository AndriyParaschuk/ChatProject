using ChatProject.BL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Models
{
    public class User : IdentityUser, IEntity<string>
    {
        //public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public bool MarkedAsDeleted { get; set; }
        public bool MarkedAsLoggedIn { get; set; }
        //public long? FriendId { get; set; }
        //public Friend Friend { get; set; }

        //public ICollection<Chat> Chats { get; set; }
        //public User()
        //{
        //    this.Chats = new List<Chat>();
        //}
    }
}
