using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using ZapFood.WinForm.Data.LiteDbRepository;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Data.Service.Interface;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Data.Service
{
    public class AccessTokenService : IAccessTokenService
    {
        private TipoDatabaseEnum _tipoDatabase;
        private AccessTokenRepository _accessTokenRepository;
        private ILiteDbRepository<AccessTokenModelView> _liteDbRepository;

        public AccessTokenService(TipoDatabaseEnum tipoDatabase)
        {
            _tipoDatabase = tipoDatabase;
            _liteDbRepository = new LiteDbRepository<AccessTokenModelView>();
            _accessTokenRepository = new AccessTokenRepository();
        }

        public void Adicionar(AccessTokenModelView objEntity)
        {
            if (_tipoDatabase == TipoDatabaseEnum.SQLSever)
                _accessTokenRepository.Atualiza(objEntity);
            else
                _liteDbRepository.Save(objEntity);
        }

        public void Alterar(AccessTokenModelView objEntity)
        {
            if (_tipoDatabase == TipoDatabaseEnum.SQLSever)
                _accessTokenRepository.Atualiza(objEntity);
            else
                _liteDbRepository.Update(objEntity);
        }

        public void Excluir(AccessTokenModelView objEntity)
        {
            throw new NotImplementedException();
        }

        public AccessTokenModelView ObterById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AccessTokenModelView> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public AccessTokenModelView ObterVariavel(string tokenSystem)
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _accessTokenRepository.ObterVariavel(tokenSystem)
                : _liteDbRepository.GetByFind(Query.Contains("token_system", tokenSystem)).FirstOrDefault();
        }
    }
}