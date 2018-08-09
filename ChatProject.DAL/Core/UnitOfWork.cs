using ChatProject.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace ChatProject.DAL.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; }
        public UnitOfWork(ChatContext dbContext)
        {
            Context = dbContext;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }
    }
}
