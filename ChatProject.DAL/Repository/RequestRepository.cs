using ChatProject.BL;
using ChatProject.BL.Interfaces;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using ChatProject.BL.Models;

namespace ChatProject.DAL.Repository
{
    public class RequestRepository : Repository<Request, long>, IRequestRepository
    {
        public RequestRepository(ChatContext context) : base(context)
        {
        }

        public Request GetRequestFromUserToUser(string userId, string withWhomId)
        {
            return _dbSet.FirstOrDefault(item => item.FromId == withWhomId && item.ToId == userId);
        }

        public List<string> GetNewUsersRequests(string userId)
        {
            return _dbSet.Where(item => item.FromId == userId && item.Status == RequestStatus.New)
                .Select(item => item.ToId).ToList();
        }

        public List<string> GetNewRequestsToUser(string userId)
        {
            return _dbSet.Where(item => item.ToId == userId && item.Status == RequestStatus.New)
                .Select(item => item.FromId).ToList(); ;
        }

        public Request GetOneNewRequest(string userId, string withWhomId)
        {
            return _dbSet.FirstOrDefault(item => item.FromId == userId && item.ToId == withWhomId && item.Status == RequestStatus.New);
        }

        public Request GetOneNewRequestsToUser(string userId, string withWhomId)
        {
            return _dbSet.FirstOrDefault(item => item.ToId == userId && item.FromId == withWhomId && item.Status == RequestStatus.New);
        }

        public Request GetNewRequests(string userId, string withWhomId)
        {
            return _dbSet.FirstOrDefault(item => item.FromId == userId && item.ToId == withWhomId);
        }

        public Request GetNewRequestsToUser(string userId, string withWhomId)
        {
            return _dbSet.FirstOrDefault(item => item.ToId == userId && item.FromId == withWhomId);
        }
    }
}
