using System;
using System.Collections.Generic;
using LiteDB;

namespace ZapFood.WinForm.Data.LiteDbRepository
{
    public interface ILiteDbRepository<TEntity> : IDisposable where TEntity : class
    {
        void Save(TEntity obj);
        void Update(TEntity obj);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetByFind(BsonExpression query);
    }
}