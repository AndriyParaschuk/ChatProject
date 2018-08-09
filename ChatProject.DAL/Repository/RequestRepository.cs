using ChatProject.BL;
using ChatProject.BL.Interfaces;
using ChatProject.DAL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.DAL.Repository
{
    public class RequestRepository : Repository<Request, long>, IRequestRepository
    {
        public RequestRepository(UnitOfWork uow) : base(uow)
        {
        }
    }
}
