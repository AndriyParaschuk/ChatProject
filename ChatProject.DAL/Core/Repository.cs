using ChatProject.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Text;

namespace ChatProject.DAL.Core
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public readonly UnitOfWork _unitOfWork;
        protected DbSet<TEntity> _dbSet;

        public Repository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbSet = _unitOfWork.Context.Set<TEntity>();
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(TKey id)
        {
            _dbSet.Remove(_dbSet.Find(id));
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public TEntity GetById(TKey id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public void Update(TEntity entity)
        {
            _dbSet.AddOrUpdate(entity);
        }
        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
    }
}
