using ChatProject.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Models
{
    public class UserFriend : IEntity<long>
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public virtual User User { get; set; }
        public virtual User Friend { get; set; }
    }
}
