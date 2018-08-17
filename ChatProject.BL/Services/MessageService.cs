using ChatProject.BL.Interfaces;
using ChatProject.BL.Interfaces.Services;
using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatProject.BL.Services
{
    public class MessageService : IMessageService
    {
        IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public List<Message> GetMessage(string userId, string withWhomId)
        {
            List<Message> messages = _messageRepository.GetAll().Where(item => (item.ToId == userId && item.FromId == withWhomId) ||
                (item.ToId == withWhomId && item.FromId == userId)).OrderBy(item => item.Date).ToList();
            return messages;
        }

        public Message PostMessage(string message, string userId, string toWhomId)
        {
            Message currentMessage = new Message() { FromId = userId, ToId = toWhomId, TextMessage = message };
            _messageRepository.Create(currentMessage);
            //_messageRepository.SaveChanges(); !!!!!
            return currentMessage;
        }
    }
}
