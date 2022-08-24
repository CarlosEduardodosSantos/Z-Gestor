
IF NOT EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'PedidoVapVupt' ) 
BEGIN
	CREATE TABLE [dbo].[PedidoVapVupt](
		[VapVuptId] [uniqueidentifier] NOT NULL,
		[PedidoId] [varchar](100) NULL,
		[VendaId] [varchar](50) NULL,
		[DataHora] [datetime] NULL,
		[Situacao] [int] NULL,
		[FileJson] [varchar](max) NULL,
		[Aplicacao] [int] NULL,
		[TipoPedido] [varchar](25) NULL
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_VapVuptProdutos' ) 
   DROP VIEW Vw_VapVuptProdutos
GO

Create View [dbo].[Vw_VapVuptProdutos]
as
	Select 
		Prod.Codigo as produtoId,
		Prod.Codigo as referenciaId, 
		Prod.DES_ as nome,
		Prod.VLVENDA as valorVenda,
		Grupo.Grupo as categoriaId,
		Grupo.DES_ as categoriaNome
	From Prod
	Left Join Grupo on Grupo.Grupo = Prod.Grupo
GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'VW_VapVuptUsuario' ) 
   DROP VIEW VW_VapVuptUsuario
GO

Create View VW_VapVuptUsuario
as
	Select 
		Codigo as VendedorId,
		Nome,
		Senha
	from RECEP Where Ativo = 'SIM'


GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'VW_VapVuptCaixa' ) 
   DROP VIEW VW_VapVuptCaixa
GO

Create View VW_VapVuptCaixa
as
	Select 
		NROCX as CaixaId,
		DATA,
		fim_data,
		loja,
		Pdv
	from Caixa_1


GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosEntregando' ) 
   DROP VIEW Vw_GestorPedidosEntregando
GO

Create View [dbo].[Vw_GestorPedidosEntregando]
as
	Select * From PedidoVapVupt Where Situacao = 2 And Isnull(TipoPedido, 'Entregar') = 'Entregar' 
		And Isnull(VendaId,0) In (Select Nro_Venda From Televenda_1 Where Sai_Data Is not null And Ret_Data is null)
GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosEntreges' ) 
   DROP VIEW Vw_GestorPedidosEntreges
GO

Create View [dbo].[Vw_GestorPedidosEntreges]
as
	Select * From PedidoVapVupt Where Situacao = 5 And Isnull(TipoPedido, 'Entregar') = 'Entregar'
		And Isnull(VendaId,0) In (Select Nro_Venda From Televenda_1 Where Ret_Data is Not null)
GO


IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosRetirar' ) 
   DROP VIEW Vw_GestorPedidosRetirar
GO

Create View [dbo].[Vw_GestorPedidosRetirar]
as
	Select * From PedidoVapVupt Where Situacao = 2 And Isnull(TipoPedido, 'Entregar') = 'Retirar'
		And Isnull(VendaId,0) In (Select Nro From VENDAS_PENDENTES Where Isnull(Retirar, 0) = 1)
GO


IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Venda1' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Venda1
GO
Create procedure PR_Gestor_Insert_Venda1
    @nro Int,
    @nrocx Int,
    @data DateTime,
    @hora DateTime,
    @tipo Varchar(2),
    @vend Int,
    @Loja Int,
    @vl_compra Decimal(18,2),
    @Cod_cli Int,
    @xTele Int,
    @pdv Int,
    @cpfcnpj Varchar(25), 
    @Estacao Varchar(100), 
    @AppId Varchar(50), 
    @PagamentoOnline Int, 
    @ValorVoucher Decimal(18,2), 
	@TipoVoucher Varchar(25) = '', 
    @OBS Varchar(800),
    @Senha Varchar(25)
as
Begin 


    declare @horaStr Varchar(8)
    set @horaStr = (SELECT Cast(Right(CONVERT(VARCHAR, @hora, 113),12) as Varchar(8))  AS timeToStr)

	declare @desconto decimal(10,2) = @ValorVoucher
	if(@TipoVoucher = 'IFOOD')
		set @desconto = 0


    Insert Into Venda_1(
        nro,
        nrocx,
        [data],
        hora,
        tipo,
        vend,
        Loja,
        vl_compra,
        Cod_cli,
        Loja_transf,
        xFrete,
        xTele,
        pdv,
        Pgto,
        senha,
        cpfcnpj, 
        IdopVendas_Pendentes, 
        Estacao, 
        AppId, 
        PagamentoOnline, 
        ValorVoucher, 
        OBS,
        ARREDONDAMENTO) 
        values (
        @nro,
        @nrocx,
        Cast(@data as date),
        @horaStr,
        @tipo,
        @vend,
        @Loja,
        @vl_compra,
        0,--@Cod_cli,
        0, --Loja_transf
        0, --xFrete
        @xTele,
        @pdv,
        @data,
        @Senha, --senha,
        @cpfcnpj, 
        0, --@IdopVendas_Pendentes, 
        @Estacao, 
        @AppId, 
        @PagamentoOnline, 
        @ValorVoucher, 
        @OBS,
        @desconto)


        --Fidelidade


        Select @nro as VendaId
End

GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Venda2' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Venda2
GO
Create procedure PR_Gestor_Insert_Venda2
	@NRO Int,
	@QTDE Decimal(18,4),
	@COD_PROD Int,
	@UNIT Decimal(18,4),
	@TOTAL Decimal(18,4),
	@PERC Decimal(14,4),
	@VALOR Decimal(18,4),
	@LOJA Int,
	@Des_ Varchar(8000),
	@SEQLANC Int, 
	@DATAHORA DateTime, 
	@GPI_IMPRIMIR Int, 
	@PROD_OBS  Varchar(8000),
	@VlCusto Decimal(18,4)
As
Begin

	--Baixa de estoque
	declare @sqlEstoque NVarchar(800);
	Set @sqlEstoque = 'Update Prod Set Qtde'+Ltrim(@LOJA)+'= Isnull(Qtde'+Ltrim(@LOJA)+ ',0) -' + Cast(@QTDE as varchar(30)) + 'where codigo = ' + Ltrim(@COD_PROD)
	Execute sp_executesql @sqlEstoque

	Exec PR_INSERT_KARDEX 
		@kad_loja = @LOJA, 
		@kad_op = 'V', 
		@kad_nroop = @NRO, 
		@kad_prod = @COD_PROD, 
		@kad_qtde = @QTDE, 
		@kad_obs = 'VENDA'

	Insert Into Venda_2 (
		
		NRO,
		QTDE,
		COD_PROD,
		UNIT,
		TOTAL,
		PERC,
		VALOR,
		LOJA,
		Des_,
		SEQLANC, 
		DATAHORA, 
		GPI_IMPRIMIR, 
		PROD_OBS, 
		VlCusto
		) Values (
		@NRO,
		@QTDE,
		@COD_PROD,
		@UNIT,
		@TOTAL,
		@PERC,
		@VALOR,
		@LOJA,
		@Des_,
		@SEQLANC, 
		@DATAHORA, 
		@GPI_IMPRIMIR, 
		@PROD_OBS, 
		@VlCusto
		)
End

GO

IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Televendas1' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Televendas1
GO
Create procedure PR_Gestor_Insert_Televendas1
	@Nro_Venda Int, 
	@Cod_Cliente Int, 
	@Ped_Data DateTime, 
	@Ped_Hora DateTime, 
	@Troco Decimal(18,2), 
	@valor Decimal(18,2), 
	@Taxa_Adicional Decimal(14,2),  
	@xMedicar Int, 
	@travado Int,
	@Loja Int, 
	@BomPara DateTime, 
	@Condicao Varchar(1), 
	@Obs Varchar(800)
As 
Begin
	Insert Into Televenda_1(
		Nro_Venda, 
		Cod_Cliente, 
		Ped_Data, 
		Ped_Hora, 
		Troco, 
		valor, 
		Taxa_Adicional, 
		xMedicar, 
		travado,
		Loja, 
		BomPara, 
		Condicao, 
		Obs) values(
		@Nro_Venda, 
		@Cod_Cliente, 
		@Ped_Data, 
		@Ped_Hora, 
		@Troco, 
		@valor, 
		@Taxa_Adicional, 
		@xMedicar, 
		@travado,
		@Loja, 
		@BomPara, 
		@Condicao, 
		@Obs)
		
End


GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Cliente' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Cliente
GO
Create procedure PR_Gestor_Insert_Cliente
	@Fone Varchar(30), 
	@Nome Varchar(100), 
	@Endereco Varchar(255), 
	@Bairro Varchar(255), 
	@Cep Varchar(30),  
	@Cidade Varchar(255), 
	@UF Varchar(2), 
	@CPF Varchar(30), 
	@Obs1 Varchar(800),
	@email Varchar(100)
As 
Begin
	declare @clienteId Int 
	Set @clienteId = Isnull((Select Top 1 Codigo From Televenda_2 Where Endereco like @Endereco),0)

	If(@clienteId = 0)
	Begin
		Insert Into Televenda_2(Fone, Nome, Endereco, Bairro, Cep, Cidade, UF, CPF, Obs1) Values (
			@Fone, @Nome, DBO.FN_REMOVEACENTUACAO(@Endereco), @Bairro, @Cep, @Cidade, @UF, @CPF, @Obs1)

		Set @clienteId = (Select @@Identity)
	End
	Else
	Begin
		Update Televenda_2 Set 
			Endereco = DBO.FN_REMOVEACENTUACAO(@Endereco),
			Bairro = @Bairro,
			Cep = @Cep,
			Cidade = @Cidade,
			UF = @UF,
			CPF = @CPF
		Where Codigo = @clienteId
	End

	Select @clienteId as ClienteId

End
GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Rollback' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Rollback
GO
Create procedure PR_Gestor_Rollback
	@vendaId Int
As
Begin

	Delete Venda_1 Where Nro = @vendaId
	Delete Venda_2 Where Nro = @vendaId
	Delete Televenda_1 Where Nro_Venda = @vendaId
	Delete MeioMeio Where NroOperacao = @vendaId
	Delete VENDA_4 Where NROVENDA = @vendaId

End

GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_MeioMeio' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_MeioMeio
GO
Create procedure PR_Gestor_Insert_MeioMeio
	@Descricao Varchar(255), 
	@NroOperacao Int, 
	@TipoOperacao Varchar(10), 
	@IdProduto Int, 
	@SeqProduto Int,
	@ValorVenda Decimal(18,2), 
	@Quantidade Decimal(18,3), 
	@QtdeImpresso Varchar(25), 
	@DataHora DateTime, 
	@Observacao Varchar(800)
As
Begin
	Insert Into MeioMeio(
		Descricao, 
		NroOperacao, 
		TipoOperacao, 
		IdProduto, 
		SeqProduto,
		ValorVenda, 
		Quantidade, 
		QtdeImpresso, 
		DataHora, 
		Observacao) Values (
		@Descricao, 
		@NroOperacao, 
		@TipoOperacao, 
		@IdProduto, 
		@SeqProduto,
		@ValorVenda, 
		@Quantidade, 
		@QtdeImpresso, 
		@DataHora, 
		@Observacao
		)

End

GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Venda_4' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Venda_4
GO
Create procedure PR_Gestor_Insert_Venda_4
	@NROVENDA Int, 
	@NROCARTAO Varchar(25), 
	@PRODCOD Int, 
	@COMPCOD Int, 
	@SEQLANC int, 
	@VALOR Decimal(14,2), 
	@IDOPMESA1 Int, 
	@NROPENDENTE Int
As
Begin
	Insert Into VENDA_4(NROVENDA, NROCARTAO, PRODCOD, COMPCOD, SEQLANC, VALOR, IDOPMESA1, NROPENDENTE)
	Values(@NROVENDA, @NROCARTAO, @PRODCOD, @COMPCOD, @SEQLANC, @VALOR, @IDOPMESA1, @NROPENDENTE)

End


GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Pendente' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Pendente
GO
Create procedure PR_Gestor_Insert_Pendente
	@Nro Int, 
	@Codigo Int,
	@Descricao Varchar(8000),
	@Qtde decimal(14,3),
	@Desconto decimal(14,2),
	@Unit decimal(14,3),
	@Total decimal(18,2),
	@Cliente Varchar(100),
	@Hora_Abertura DateTime, 
	@VEND Int, 
	@ESTACAO Varchar(50), 
	@PROD_OBS Varchar(800), 
	@SEQLANC Int, 
	@GPI_IMPRIMIR Int, 
	@data_abertura DateTime, 
	@Retirar Int,
	@PagamentoOnline bit
As
Begin
	declare @horaStr Varchar(8)
	set @horaStr = (SELECT Cast(Right(CONVERT(VARCHAR, @Hora_Abertura, 113),12) as Varchar(8))  AS timeToStr)



	Insert Into VENDAS_PENDENTES (
		Nro, 
		Codigo,
		Descricao,
		Qtde,
		Desconto,
		Unit,
		Total,
		Cliente, 
		Hora_Abertura, 
		VEND, 
		ESTACAO, 
		PROD_OBS, 
		SEQLANC, 
		GPI_IMPRIMIR, 
		data_abertura, 
		Retirar,
		PagamentoOnline) Values (
		@Nro, 
		@Codigo,
		@Descricao,
		@Qtde,
		@Desconto,
		@Unit,
		@Total,
		@Cliente, 
		@horaStr, 
		@VEND, 
		@ESTACAO, 
		@PROD_OBS, 
		@SEQLANC, 
		@GPI_IMPRIMIR, 
		@data_abertura, 
		@Retirar,
		@PagamentoOnline)	
End
GO

IF EXISTS (SELECT NAME FROM   SYSOBJECTS WHERE  NAME = N'PR_OBTER_SENQUENCIA')
    DROP PROCEDURE PR_OBTER_SENQUENCIA
GO
CREATE PROCEDURE PR_OBTER_SENQUENCIA 
	@TABELA  VARCHAR(50)

AS
BEGIN
	DECLARE @RESULT INT

	UPDATE SEQ_TABELA SET SEQUENCIA = (ISNULL(SEQUENCIA,0) + 1) WHERE TABELA = @TABELA AND COLUNA = 'NRO'

	SET @RESULT = isnull((select top 1 SEQUENCIA from seq_tabela where TABELA = @TABELA AND COLUNA = 'NRO'),0)

	SELECT @RESULT;
END

GO

IF EXISTS (SELECT NAME FROM   SYSOBJECTS WHERE  NAME = N'PR_OBTER_CONFIGURACAO')
    DROP PROCEDURE PR_OBTER_CONFIGURACAO
GO
CREATE PROCEDURE PR_OBTER_CONFIGURACAO 
	@VARIAVEL  VARCHAR(50)

AS
BEGIN
	DECLARE @RESULT VARCHAR(100)

	SET @RESULT = ISNULL((select TOP 1 valor from configuracoes where variavel like @VARIAVEL),'')

	SELECT @RESULT;
END


Insert Into configuracoes(variavel, valor, Descricao)
Values('COD_TX_SERVICO', '0', 'VARIAVEL QUE DEFINE O CODIGO DO PRODUTO PARA TAXA IFOOD')

