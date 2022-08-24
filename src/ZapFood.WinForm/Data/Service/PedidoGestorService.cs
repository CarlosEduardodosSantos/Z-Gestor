using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.LiteDbRepository;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Data.Service.Interface;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Service
{
    public class PedidoGestorService : IPedidoGestorService
    {
        private PedidoZapFoodRepository _pedidoZapFoodRepository;
        private ILiteDbRepository<PedidoVapVupt> _liteDbRepository;
        private TipoDatabaseEnum _tipoDatabase;

        public PedidoGestorService(TipoDatabaseEnum tipoDatabase)
        {
            _tipoDatabase = tipoDatabase;
            _pedidoZapFoodRepository = new PedidoZapFoodRepository();
            _liteDbRepository = new LiteDbRepository<PedidoVapVupt>();
        }
        public void Adicionar(PedidoVapVupt objEntity)
        {
            if (_tipoDatabase == TipoDatabaseEnum.SQLSever)
                _pedidoZapFoodRepository.AddOrUpdate(objEntity);
            else
                _liteDbRepository.Save(objEntity);
        }

        public void Alterar(PedidoVapVupt objEntity)
        {
            if (_tipoDatabase == TipoDatabaseEnum.SQLSever)
                _pedidoZapFoodRepository.AddOrUpdate(objEntity);
            else
                _liteDbRepository.Update(objEntity);
        }

        public void Excluir(PedidoVapVupt objEntity)
        {
            throw new NotImplementedException();
        }

        public PedidoVapVupt ObterById(Guid id)
        {
            return _pedidoZapFoodRepository.ObterPorPedidoId(id.ToString());
        }

        public IEnumerable<PedidoVapVupt> ObterTodos()
        {
            if (_tipoDatabase == TipoDatabaseEnum.SQLSever)
                return _pedidoZapFoodRepository.ObterNovos();
            else
                return _liteDbRepository.GetAll();
        }

        public IEnumerable<PedidoVapVupt> ObterPorData(DateTime dataInicio, DateTime dataFinal)
        {

            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPorData(dataInicio, dataFinal)
                : _liteDbRepository.GetByFind(Query.Between("DataHora", dataInicio, dataFinal));
        }

        public IEnumerable<PedidoVapVupt> ObterPorAppId(string vendaId)
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPorAppId(vendaId)
                : _liteDbRepository.GetByFind(Query.Contains("VendaId", vendaId));
        }

        public IEnumerable<PedidoVapVupt> ObterNovos()
        {

            //return _liteDbRepository.GetAll();
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterNovos()
                : _liteDbRepository.GetByFind(Query.In("Situacao", "PLACED", "INTEGRATED"));
        }

        public IEnumerable<PedidoVapVupt> ObterPorSituacao(SituacaoVapVuptEnum situacao)
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPorSituacao(situacao)
                : _liteDbRepository.GetByFind(Query.EQ("Situacao", situacao.ToString()));
        }

        public IEnumerable<PedidoVapVupt> ObterPedidosEntregando()
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPedidosEntregando()
                : _liteDbRepository.GetByFind(Query.EQ("Situacao", SituacaoVapVuptEnum.DISPATCHED.ToString()));
        }

        public IEnumerable<PedidoVapVupt> ObterPedidosEntreges()
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPedidosEntreges()
                : _liteDbRepository.GetByFind(Query.EQ("Situacao", SituacaoVapVuptEnum.DELIVERED.ToString()));
        }

        public IEnumerable<PedidoVapVupt> ObterPedidosRetirar()
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPedidosRetirar()
                : _liteDbRepository.GetByFind(Query.EQ("Situacao", SituacaoVapVuptEnum.DELIVERED.ToString()));
        }

        public PedidoVapVupt ObterPorPedidoId(string pedidoId)
        {
            return _tipoDatabase == TipoDatabaseEnum.SQLSever
                ? _pedidoZapFoodRepository.ObterPorPedidoId(pedidoId)
                : _liteDbRepository.GetByFind(Query.Contains("PedidoId", pedidoId)).FirstOrDefault();
        }

        public void Dispose()
        {
            _liteDbRepository.Dispose();
        }
    }
}