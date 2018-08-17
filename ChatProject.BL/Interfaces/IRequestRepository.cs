using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatProject.BL.Interfaces
{
    public interface IRequestRepository : IRepository<Request, long>
    {
        Request GetRequestFromUserToUser(string userId, string withWhomId);

        List<string> GetNewUsersRequests(string userId);
        List<string> GetNewRequestsToUser(string userId);

        Request GetOneNewRequest(string userId, string withWhomId);
        Request GetOneNewRequestsToUser(string userId, string withWhomId);

        Request GetNewRequests(string userId, string withWhomId);
        Request GetNewRequestsToUser(string userId, string withWhomId);
    }
}
