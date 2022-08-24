using System;
using System.Collections.Generic;

namespace ZapFood.WinForm.Data.Service.Interface
{
    public interface IDataServiceBase<TEntity> where TEntity : class
    {
        void Adicionar(TEntity objEntity);
        void Alterar(TEntity objEntity);
        void Excluir(TEntity objEntity);
        TEntity ObterById(Guid id);
        IEnumerable<TEntity> ObterTodos();
    }
}