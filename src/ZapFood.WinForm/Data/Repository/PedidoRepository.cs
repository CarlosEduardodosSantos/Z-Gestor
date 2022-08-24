using System;
using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Repository
{
    public class PedidoRepository : BaseRepository
    {
        public int Adicionar(PedidoRootModel pedido)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var sql = new StringBuilder();
                var parns = new DynamicParameters();

                if (pedido.orderType != "DELIVERY" && Program.Sistema != SistemaEnum.Shop)
                {
                    var pedidoPendente = SeqTabela("VENDA_PENDENTE");
                    var isPagto = pedido.payments.prepaid; //TODO ponto critico
                    try
                    {
                        int seqLanc = 1;
                        foreach (var pedidoItem in pedido.items)
                        {
                            if (pedidoItem.externalCode == "999999" && pedidoItem.options != null)
                            {
                                var itemPrincipal = 0;
                                foreach (var pedidoItemSubItem in pedidoItem.options)
                                {

                                    var produtoId = pedidoItemSubItem.externalCode;
                                    if (produtoId == "999999" || produtoId == "999998") continue;


                                    itemPrincipal++;

                                    var desProduto = new ProdutoRepository().ObterPorId(produtoId);
                                    if (desProduto == null)
                                    {
                                        desProduto = new Produto();
                                        desProduto.nome = pedidoItemSubItem.name;
                                    }

                                    sql = new StringBuilder();
                                    sql.AppendLine("Exec PR_Gestor_Insert_Pendente");
                                    sql.AppendLine("@Nro, @Codigo,@Descricao,@Qtde,@Desconto,@Unit,@Total,");
                                    sql.AppendLine("@Cliente, @Hora_Abertura, @VEND, @ESTACAO, @PROD_OBS, @SEQLANC, @GPI_IMPRIMIR, @data_abertura, @Retirar, @PagamentoOnline");

                                    parns = new DynamicParameters();
                                    parns.Add("@Nro", pedidoPendente);
                                    parns.Add("@Codigo", pedidoItemSubItem.externalCode);

                                    parns.Add("@Descricao", pedidoItemSubItem.name);
                                    parns.Add("@Qtde", pedidoItemSubItem.quantity);
                                    parns.Add("@Desconto", 0);

                                    var valorUnit = pedidoItemSubItem.unitPrice > 0 ? pedidoItemSubItem.unitPrice : pedidoItem.price;
                                    parns.Add("@Unit", valorUnit);
                                    parns.Add("@Total", pedidoItemSubItem.price);
                                    parns.Add("@Cliente", pedido.customer.name);
                                    parns.Add("@Hora_Abertura", pedido.DataPedido);
                                    parns.Add("@VEND", Program.VendedorId);
                                    parns.Add("@ESTACAO", Environment.MachineName);
                                    parns.Add("@PROD_OBS", pedidoItem.observacao);
                                    parns.Add("@SEQLANC", seqLanc);
                                    parns.Add("@GPI_IMPRIMIR", 1);
                                    parns.Add("@data_abertura", pedido.DataPedido);
                                    parns.Add("@Retirar", 0);
                                    parns.Add("@PagamentoOnline", isPagto);

                                    conn.Query(sql.ToString(), parns);
                                    seqLanc++;
                                }
                            }
                            else
                            {
                                sql = new StringBuilder();
                                sql.AppendLine("Exec PR_Gestor_Insert_Pendente");
                                sql.AppendLine("@Nro, @Codigo,@Descricao,@Qtde,@Desconto,@Unit,@Total,");
                                sql.AppendLine("@Cliente, @Hora_Abertura, @VEND, @ESTACAO, @PROD_OBS, @SEQLANC, @GPI_IMPRIMIR, @data_abertura, @Retirar, @PagamentoOnline");

                                parns = new DynamicParameters();
                                parns.Add("@Nro", pedidoPendente);
                                parns.Add("@Codigo", pedidoItem.externalCode);

                                parns.Add("@Descricao", pedidoItem.NamePrincipal);
                                parns.Add("@Qtde", pedidoItem.quantity);
                                parns.Add("@Desconto", 0);

                                var valorUnit = pedidoItem.subItemsPrice > 0 ? pedidoItem.subItemsPrice : pedidoItem.price;
                                parns.Add("@Unit",
                                    pedidoItem.totalPrice > 0 ? pedidoItem.totalPrice : pedidoItem.subItemsPrice);
                                parns.Add("@Total", pedidoItem.totalItem);
                                parns.Add("@Cliente", pedido.customer.name);
                                parns.Add("@Hora_Abertura", pedido.DataPedido);
                                parns.Add("@VEND", Program.VendedorId);
                                parns.Add("@ESTACAO", Environment.MachineName);
                                parns.Add("@PROD_OBS", pedidoItem.observacao);
                                parns.Add("@SEQLANC", seqLanc);
                                parns.Add("@GPI_IMPRIMIR", 1);
                                parns.Add("@data_abertura", pedido.DataPedido);
                                parns.Add("@Retirar", 0);
                                parns.Add("@PagamentoOnline", isPagto);

                                conn.Query(sql.ToString(), parns);

                                seqLanc++;
                            }

                        }

                        ImprimirItens(pedidoPendente, 3);

                        return pedidoPendente;
                    }
                    catch (Exception e)
                    {
                        //conn.Query("Exec PR_Gestor_Rollback @vendaId", new { pedidoPendente });

                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
                else
                {

                    //Grava Cliente
                    sql = new StringBuilder();
                    sql.AppendLine("Exec PR_Gestor_Insert_Cliente");
                    sql.AppendLine("@Fone, @Nome, @Endereco, @Bairro, @Cep, @Cidade, @UF, @CPF, @Obs1, @email");

                    parns = new DynamicParameters();

                    parns.Add("@Fone", Funcoes.SoNumeros(pedido.customer.phone.number ?? ""));
                    parns.Add("@Nome", pedido.customer.name);
                    parns.Add("@Endereco", pedido.delivery.deliveryAddress?.formattedAddress.Replace("'", " ") + " " + pedido.delivery.deliveryAddress?.complement);
                    parns.Add("@Bairro", pedido.delivery.deliveryAddress?.neighborhood);
                    parns.Add("@Cep", Funcoes.SoNumeros(pedido.delivery.deliveryAddress?.postalCode));
                    parns.Add("@Cidade", pedido.delivery.deliveryAddress?.city);
                    parns.Add("@UF", pedido.delivery.deliveryAddress?.state);
                    parns.Add("@CPF", pedido.customer.documentNumber);
                    parns.Add("@Obs1", "Cliente Zfood");
                    parns.Add("@email", pedido.customer.id ?? "");


                    var idCliente = conn.Query<int>(sql.ToString(), parns).FirstOrDefault();



                    var vendaId = "99" + SeqTabela("VENDA");

                    try
                    {
                        sql = new StringBuilder();

                        sql.AppendLine("Exec PR_Gestor_Insert_Venda1");
                        sql.AppendLine("@nro,@nrocx,@data,@hora,@tipo,@vend,@Loja,@vl_compra,@Cod_cli, @xTele,");
                        sql.AppendLine("@pdv,@cpfcnpj, @Estacao, @AppId, @PagamentoOnline, @ValorVoucher, @TipoVoucher, @OBS, @Senha");

                        parns = new DynamicParameters();
                        parns.Add("@nro", vendaId);
                        parns.Add("@nrocx", Program.CaixaId);
                        parns.Add("@data", pedido.DataPedido.Date);
                        parns.Add("@hora", pedido.DataPedido.TimeOfDay.ToString().Substring(0, 4));
                        parns.Add("@tipo", "V");
                        parns.Add("@vend", Program.VendedorId);
                        parns.Add("@Loja", Program.Loja);





                        parns.Add("@vl_compra", pedido.valorTotal);
                        parns.Add("@Cod_cli", idCliente);
                        parns.Add("@xTele", pedido.orderType == "DELIVERY");
                        parns.Add("@pdv", Program.Pdv);
                        parns.Add("@cpfcnpj", pedido.customer.documentNumber);
                        parns.Add("@Estacao", Environment.MachineName);
                        parns.Add("@AppId", pedido.displayId ?? pedido.id);
                        parns.Add("@ValorVoucher", pedido.VoucherDesconto);
                        parns.Add("@TipoVoucher", pedido.tipoCupom);
                        parns.Add("@OBS", pedido.observations);

                        var isPagto = pedido.payments.prepaid; //TODO ponto critico
                        parns.Add("@PagamentoOnline", isPagto);

                        parns.Add("@Senha", pedido.senhaId);

                        conn.Query(sql.ToString(), parns);


                        //Verifica se existe um produto para o frete
                        var codTxEntrega = ObterConfiguracao("COD_TX_ENTREGA");

                        int produtoEntregaId;
                        int.TryParse(codTxEntrega, out produtoEntregaId);

                        if (produtoEntregaId > 0)
                        {
                            pedido.items.Add(new Item()
                            {
                                externalCode = produtoEntregaId.ToString(),
                                quantity = 1,
                                name = "Entrega",
                                price = decimal.Parse(pedido.total.deliveryFee.ToString()),
                                unitPrice = decimal.Parse(pedido.total.deliveryFee.ToString()),
                                totalPrice = decimal.Parse(pedido.total.deliveryFee.ToString())
                            });
                        }

                        //Varifica Taxa de serviço
                        if (pedido.valorServico > 0)
                        {
                            var codTxServico = ObterConfiguracao("COD_TX_SERVICO");
                            int produtoServicoId;
                            int.TryParse(codTxServico, out produtoServicoId);

                            if (produtoServicoId > 0)
                            {
                                var produtoTaxa = new ProdutoRepository().ObterPorId(produtoServicoId.ToString());
                                if (produtoTaxa == null)
                                {
                                    produtoTaxa = new Produto();
                                    produtoTaxa.nome = "TAXA IFOOD";
                                }

                                pedido.items.Add(new Item()
                                {
                                    externalCode = produtoServicoId.ToString(),
                                    quantity = 1,
                                    name = produtoTaxa.nome,
                                    price = pedido.valorServico,
                                    unitPrice = pedido.valorServico,
                                    totalPrice = pedido.valorServico
                                });
                            }

                        }
                        /*
                        //Verifica item options produto
                        var optionsExistes = pedido.items.Any(t => t.options != null);
                        if (optionsExistes)
                        {
                            foreach (var pedidoItem in pedido.items)
                            {
                                foreach (var option in pedidoItem.options)
                                {
                                    if (pedidoItem.externalCode == "999998") continue;

                                    if (string.IsNullOrEmpty(option.externalCode) || option.externalCode == "999999") continue;

                                    pedido.items.Add(new Item()
                                    {
                                        externalCode = option.externalCode,
                                        quantity = option.quantity,
                                        name = option.name,
                                        price = option.price,
                                        unitPrice = option.unitPrice,
                                        totalPrice = option.price
                                    });

                                    option.confirmedItem = true;
                                }
                            }
                        }
                        */
                        int seqLanc = 0;
                        foreach (var pedidoItem in pedido.items)
                        {
                            sql = new StringBuilder();
                            sql.AppendLine("Exec PR_Gestor_Insert_Venda2");
                            sql.AppendLine("@NRO,@QTDE,@COD_PROD,@UNIT,@TOTAL,@PERC,@VALOR,@LOJA,@Des_,@SEQLANC,@DATAHORA,@GPI_IMPRIMIR,@PROD_OBS,@VlCusto");

                            if (Program.TipoDescricao == TipoDescricaoEnumView.Ifood)
                            {
                                seqLanc++;

                                parns = new DynamicParameters();


                                var desProduto = new ProdutoRepository().ObterPorId(pedidoItem.externalCodeCalc);
                                if (desProduto == null)
                                {
                                    desProduto = new Produto();
                                    desProduto.nome = pedidoItem.name;
                                }

                                parns.Add("@nro", vendaId);
                                parns.Add("@QTDE", pedidoItem.quantity);
                                parns.Add("@COD_PROD", pedidoItem.externalCodeCalc);
                                parns.Add("@Des_", pedidoItem.NamePrincipal);



                                parns.Add("@UNIT", pedidoItem.unitPriceCalc);
                                parns.Add("@TOTAL", pedidoItem.totalItem);
                                parns.Add("@PERC", 0);
                                parns.Add("@VALOR", pedidoItem.unitPriceCalc);
                                parns.Add("@LOJA", Program.Loja);
                                parns.Add("@SEQLANC", seqLanc);
                                parns.Add("@DATAHORA", pedido.DataPedido);
                                parns.Add("@GPI_IMPRIMIR", 1);
                                parns.Add("@PROD_OBS", pedidoItem.observacao);
                                parns.Add("@VlCusto", 0.01); //TODO verificar custo do produto


                                conn.Query(sql.ToString(), parns);
                            }
                            else
                            {
                                if (pedidoItem.externalCode == "999999" && pedidoItem.options != null)
                                {
                                    var itemPrincipal = 0;
                                    foreach (var pedidoItemSubItem in pedidoItem.options)
                                    {

                                        var produtoId = pedidoItemSubItem.externalCode;
                                        if (produtoId == "999999" || produtoId == "999998") continue;

                                        seqLanc++;
                                        itemPrincipal++;

                                        var desProduto = new ProdutoRepository().ObterPorId(produtoId);
                                        if (desProduto == null)
                                        {
                                            desProduto = new Produto();
                                            desProduto.nome = pedidoItemSubItem.name;
                                        }


                                        parns = new DynamicParameters();

                                        parns.Add("@nro", vendaId);
                                        parns.Add("@QTDE", pedidoItemSubItem.quantity);
                                        parns.Add("@COD_PROD", produtoId);
                                        parns.Add("@Des_", $"{desProduto.nome} {pedidoItem.observacao}");

                                        var totalItem = pedidoItemSubItem.price > 0
                                            ? pedidoItem.quantity * pedidoItemSubItem.price
                                            : pedidoItem.totalItem;

                                        parns.Add("@UNIT", pedidoItemSubItem.price);
                                        parns.Add("@TOTAL", totalItem);
                                        parns.Add("@PERC", 0);
                                        parns.Add("@VALOR", totalItem);
                                        parns.Add("@LOJA", Program.Loja);
                                        parns.Add("@SEQLANC", seqLanc);
                                        parns.Add("@DATAHORA", pedido.DataPedido);
                                        parns.Add("@GPI_IMPRIMIR", 1);
                                        parns.Add("@PROD_OBS", itemPrincipal == 1 ? pedidoItem.observacao : "");
                                        parns.Add("@VlCusto", "0.01");

                                        conn.Query(sql.ToString(), parns);

                                    }
                                }
                                else
                                {
                                    seqLanc++;

                                    parns = new DynamicParameters();

                                    var desProduto = new ProdutoRepository().ObterPorId(pedidoItem.externalCode);
                                    if (desProduto == null)
                                    {
                                        desProduto = new Produto();
                                        desProduto.nome = pedidoItem.name;
                                    }

                                    parns.Add("@nro", vendaId);
                                    parns.Add("@QTDE", pedidoItem.quantity);
                                    parns.Add("@COD_PROD", pedidoItem.externalCode);
                                    //parns.Add("@Des_", desProduto.nome + " " + pedidoItem.observacao);
                                    parns.Add("@Des_", pedidoItem.NamePrincipal);

                                    var totalItem = pedidoItem.price * pedidoItem.quantity;
                                    parns.Add("@UNIT",
                                        pedidoItem.price > 0 ? pedidoItem.price : pedidoItem.subItemsPrice);
                                    parns.Add("@TOTAL", totalItem);
                                    parns.Add("@PERC", 0);
                                    parns.Add("@VALOR", totalItem);
                                    parns.Add("@LOJA", Program.Loja);
                                    parns.Add("@SEQLANC", seqLanc);
                                    parns.Add("@DATAHORA", pedido.DataPedido);
                                    parns.Add("@GPI_IMPRIMIR", 1);
                                    parns.Add("@PROD_OBS", pedidoItem.observacao);
                                    parns.Add("@VlCusto", 0); //TODO verificar custo do produto


                                    conn.Query(sql.ToString(), parns);

                                    if (pedidoItem.options?.Sum(t => t.price) > 0)
                                    {
                                        foreach (var pedidoItemSubItem in pedidoItem.options)
                                        {
                                            if (pedidoItemSubItem.price == 0) continue;

                                            seqLanc++;


                                            desProduto =
                                                new ProdutoRepository().ObterPorId(pedidoItemSubItem.externalCode);
                                            if (desProduto == null)
                                            {
                                                desProduto = new Produto();
                                                desProduto.nome = pedidoItemSubItem.name;
                                            }

                                            parns = new DynamicParameters();

                                            parns.Add("@nro", vendaId);
                                            parns.Add("@QTDE", pedidoItemSubItem.quantity);
                                            parns.Add("@COD_PROD", pedidoItemSubItem.externalCode);
                                            parns.Add("@Des_", $"{desProduto.nome}");

                                            totalItem = pedidoItemSubItem.unitPrice > 0
                                                ? pedidoItemSubItem.price
                                                : pedidoItem.totalItem;
                                            parns.Add("@UNIT",
                                                pedidoItemSubItem.price > 0
                                                    ? pedidoItemSubItem.price
                                                    : pedidoItem.price);
                                            parns.Add("@TOTAL", totalItem);
                                            parns.Add("@PERC", 0);
                                            parns.Add("@VALOR", totalItem);
                                            parns.Add("@LOJA", Program.Loja);
                                            parns.Add("@SEQLANC", seqLanc);
                                            parns.Add("@DATAHORA", pedido.DataPedido);
                                            parns.Add("@GPI_IMPRIMIR", 1);
                                            parns.Add("@PROD_OBS", "");
                                            parns.Add("@VlCusto", "0.01"); //TODO verificar custo do produto


                                            conn.Query(sql.ToString(), parns);

                                        }

                                    }
                                }
                            }

                            //Verifica se produto pode ser Meio Meio
                            if (pedidoItem.options != null && pedidoItem.price == 0 && pedidoItem.subItemsPrice > 0)
                            {

                                foreach (var pedidoItemSubItem in pedidoItem.options)
                                {
                                    sql = new StringBuilder();
                                    sql.AppendLine("Exec PR_Gestor_Insert_MeioMeio");
                                    sql.AppendLine("@Descricao, @NroOperacao, @TipoOperacao, @IdProduto, @SeqProduto,");
                                    sql.AppendLine("@ValorVenda, @Quantidade, @QtdeImpresso, @DataHora, @Observacao");

                                    parns = new DynamicParameters();
                                    parns.Add("@Descricao", pedidoItemSubItem.name);
                                    parns.Add("@NroOperacao", vendaId);
                                    parns.Add("@TipoOperacao", "V");

                                    parns.Add("@IdProduto", pedidoItemSubItem.externalCode);
                                    parns.Add("@SeqProduto", seqLanc);
                                    parns.Add("@ValorVenda", pedidoItemSubItem.price);
                                    parns.Add("@Quantidade", pedidoItemSubItem.quantity);
                                    parns.Add("@QtdeImpresso", "1/" + pedidoItem.options.Count());
                                    parns.Add("@DataHora", DateTime.Now);
                                    parns.Add("@Observacao", "");

                                    conn.Query(sql.ToString(), parns);
                                }
                            }
                            else if (pedidoItem.options != null)
                            {
                                if (pedidoItem.options.Sum(t => t.price) > 0)
                                {
                                    foreach (var pedidoItemSubItem in pedidoItem.options)
                                    {
                                        sql = new StringBuilder();
                                        sql.AppendLine("Exec PR_Gestor_Insert_Venda_4");
                                        sql.AppendLine("@NROVENDA, @NROCARTAO, @PRODCOD, @COMPCOD, @SEQLANC, @VALOR, @IDOPMESA1, @NROPENDENTE");

                                        parns = new DynamicParameters();
                                        parns.Add("@NROVENDA", vendaId);
                                        parns.Add("@NROCARTAO", "0");
                                        parns.Add("@PRODCOD", pedidoItem.externalCode);
                                        parns.Add("@COMPCOD", pedidoItemSubItem.index);
                                        parns.Add("@SEQLANC", pedidoItemSubItem.index);
                                        parns.Add("@VALOR", pedidoItemSubItem.price);
                                        parns.Add("@IDOPMESA1", "0");
                                        parns.Add("@NROPENDENTE", "0");

                                        conn.Query(sql.ToString(), parns);
                                    }
                                }

                            }
                        }

                        //Verifica se é delivery
                        if (pedido.orderType == "DELIVERY")
                        {

                            //sql = new StringBuilder();
                            //sql.AppendLine("Exec PR_Gestor_Insert_Televendas2");
                            //sql.AppendLine("@Fone, @Nome, @Endereco, @Bairro, @Cep, @Cidade, @UF, @CPF, @Obs1, @email");

                            //parns = new DynamicParameters();

                            //parns.Add("@Fone", Funcoes.SoNumeros(pedido.customer.phone));
                            //parns.Add("@Nome", pedido.customer.name);
                            //parns.Add("@Endereco", pedido.deliveryAddress.formattedAddress + " " + pedido.deliveryAddress.complement);
                            //parns.Add("@Bairro", pedido.deliveryAddress.neighborhood);
                            //parns.Add("@Cep", Funcoes.SoNumeros(pedido.deliveryAddress.postalCode));
                            //parns.Add("@Cidade", pedido.deliveryAddress.city);
                            //parns.Add("@UF", pedido.deliveryAddress.state);
                            //parns.Add("@CPF", pedido.customer.taxPayerIdentificationNumber);
                            //parns.Add("@Obs1", "Cliente IFood");
                            //parns.Add("@email", pedido.customer.email);


                            //var idCliente = conn.Query<int>(sql.ToString(), parns).FirstOrDefault();

                            sql = new StringBuilder();
                            sql.AppendLine("Exec PR_Gestor_Insert_Televendas1");
                            sql.AppendLine("@Nro_Venda, @Cod_Cliente, @Ped_Data, @Ped_Hora, @Troco, @valor, @Taxa_Adicional, @xMedicar, @travado,");
                            sql.AppendLine("@Loja, @BomPara, @Condicao, @Obs");

                            parns = new DynamicParameters();
                            parns.Add("@Nro_Venda", vendaId);
                            parns.Add("@Cod_Cliente", idCliente);
                            parns.Add("@Ped_Data", pedido.DataPedido.Date);
                            parns.Add("@Ped_Hora", pedido.DataPedido);

                            var trocoPara = pedido.payments.methods.Sum(t => t.cash?.changeFor);
                            var troco = trocoPara > 0 ? trocoPara - pedido.valorTotal : 0;


                            parns.Add("@Troco", troco < 0 ? 0 : troco);
                            parns.Add("@valor", pedido.valorTotal);
                            parns.Add("@Taxa_Adicional", produtoEntregaId == 0 ? pedido.total.deliveryFee : 0);
                            parns.Add("@xMedicar", 0);
                            parns.Add("@travado", 0);
                            parns.Add("@Loja", Program.Loja);
                            parns.Add("@BomPara", pedido.DataPedido.Date);
                            parns.Add("@Condicao", "V");



                            var observacao = !string.IsNullOrEmpty(pedido.delivery.deliveryAddress.reference)
                                ? $"Ref. {pedido.delivery.deliveryAddress.reference} "
                                : "";

                            observacao += !string.IsNullOrEmpty(pedido.customer.documentNumber)
                                ? $"CPF. {pedido.customer.documentNumber} "
                                : "";
                            foreach (var pedidoPayment in pedido.payments.methods)
                            {
                                observacao += pedidoPayment.method + " ";
                            }
                            parns.Add("@Obs", observacao);


                            conn.Query(sql.ToString(), parns);

                            try
                            {
                                conn.Query("Exec PROC_GRI_IMPRIME @vendaId, 0", new { vendaId });
                            }
                            catch
                            {
                                // ignored
                            }

                        }


                        return int.Parse(vendaId);
                    }
                    catch (Exception e)
                    {
                        conn.Query("Exec PR_Gestor_Rollback @vendaId", new { vendaId });

                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }

        private int SeqTabela(string tabela)
        {
            var sql = new StringBuilder();
            sql.AppendLine($"Exec PR_OBTER_SENQUENCIA @tabela");

            using (var conn = Connection)
            {
                conn.Open();
                var vendaId = conn.Query<int>(sql.ToString(), new { tabela }).FirstOrDefault();
                conn.Close();

                return vendaId;
            }
        }

        private string ObterConfiguracao(string variavel)
        {
            var sql = new StringBuilder();
            sql.AppendLine($"Exec PR_OBTER_CONFIGURACAO @variavel");

            using (var conn = Connection)
            {
                conn.Open();
                var valorVariavel = conn.Query<string>(sql.ToString(), new { variavel }).FirstOrDefault();
                conn.Close();

                return valorVariavel;
            }
        }

        public void ImprimirFechamentoPedido(int vendaId, int tipo)
        {
            try
            {
                var sql = "Exec PROC_GRI_IMPRIME_FECHAMENTO @NroOperacao, @TipoOperacao, @Estacao, @PgtoParc";
                using (var conn = Connection)
                {
                    conn.Open();

                    conn.Query(sql, new { NroOperacao = vendaId, TipoOperacao = tipo, Estacao = Environment.MachineName, PgtoParc = 0 });

                    conn.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao imprimir o fechamento.\n{e.Message}");
            }
        }

        public bool ImprimirItens(int vendaId, int tipoOperacao)
        {
            try
            {
                using (var conn = Connection)
                {
                    var sql = $"Exec PROC_GRI_IMPRIME @NroOperacao, @TipoOperacao";
                    conn.Open();
                    conn.Query(sql, new { NroOperacao = vendaId, TipoOperacao = tipoOperacao });
                    conn.Close();

                    return true;
                }
            }
            catch
            {
                //ignote
                return false;
            }
        }

        public string ObterSenha()
        {
            var sql = "select Cast(Isnull(valor,0) as Int) from configuracoes where variavel like 'SENHA_IMPRESSAO_ENTREGA'";

            using (var conn = Connection)
            {
                try
                {
                    conn.Open();

                    var exists = conn.Query<int>("select * from configuracoes where variavel like 'SENHA_IMPRESSAO_ENTREGA'").Any();
                    if (!exists)
                    {
                        var sqlInsert = "Insert Into configuracoes(variavel, valor, Descricao)";
                        sqlInsert += "Values('SENHA_IMPRESSAO_ENTREGA', '0', 'SENHA NA IMPRESSÃO EM TELE-VENDAS')";
                        conn.Execute(sqlInsert);
                    }

                    var senha = conn.Query<int>(sql).FirstOrDefault();

                    senha += 1;

                    //Incrementa valor
                    conn.Execute("Update configuracoes Set valor = @novaSenha where variavel like 'SENHA_IMPRESSAO_ENTREGA'", new { novaSenha = senha });

                    conn.Close();


                    return senha.ToString();

                }
                catch
                {
                    return "0";
                }
            }
        }
    }
}