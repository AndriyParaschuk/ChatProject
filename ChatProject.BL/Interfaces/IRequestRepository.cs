﻿using ChatProject.BL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces
{
    public interface IRequestRepository : IRepository<Request, long>
    {
    }
}