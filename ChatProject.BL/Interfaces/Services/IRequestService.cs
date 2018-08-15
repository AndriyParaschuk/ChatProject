using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces.Services
{
    public interface IRequestService
    {
        void AcceptRequest(string userId, string withWhomId);
        void DeclineRequest(string userId, string withWhomId);
        void CreateRequest(string userId, string toWhomId);
    }
}
