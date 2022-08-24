using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiteDB;

namespace ZapFood.WinForm.Data.LiteDbRepository
{
    public class LiteDbRepository<TEntity>  : BaseLiteDbRepository, ILiteDbRepository<TEntity> where TEntity : class
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Save(TEntity obj)
        {
            using (var conn = Connection)
            {
                conn.GetCollection<TEntity>().Insert(obj);
            }
        }

        public void Update(TEntity obj)
        {
            using (var conn = Connection)
            {
                conn.GetCollection<TEntity>().Update(obj);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var conn = Connection)
            {
                return conn.GetCollection<TEntity>().FindAll().ToList();
            }
        }

        public IEnumerable<TEntity> GetByFind(BsonExpression query)
        {
            using (var conn = Connection)
            {
                return conn.GetCollection<TEntity>().Find(query).ToList();
            }
        }
    }
}