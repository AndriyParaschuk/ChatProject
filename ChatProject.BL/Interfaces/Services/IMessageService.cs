using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces.Services
{
    public interface IMessageService
    {
        List<Message> GetMessage(string userId, string withWhomId);

        Message PostMessage(string message, string userId, string toWhomId);
    }
}
