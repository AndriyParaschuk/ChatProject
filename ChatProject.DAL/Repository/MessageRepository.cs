using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.DAL.Repository
{
    public class MessageRepository : Repository<Message, long>, IMessageRepository
    {
        public MessageRepository(ChatContext context) : base(context)
        {
        }
    }
}
