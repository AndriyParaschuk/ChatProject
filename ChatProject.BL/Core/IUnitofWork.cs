using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
