
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
		Produtos.Codigo as produtoId,
		Produtos.Codigo as referenciaId, 
		Produtos.Descricao as nome,
		Produtos.VlVenda2 as valorVenda,
		Grupos.codigo as categoriaId,
		Grupos.descricao as categoriaNome
	From Produtos
	Left Join Grupos on Grupos.codigo = Produtos.Grupo
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
	from usuarios Where Ativo = 0

GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'VW_VapVuptCaixa' ) 
   DROP VIEW VW_VapVuptCaixa
GO

Create View VW_VapVuptCaixa
as
	Select 
		nro as CaixaId,
		dataini,
		datafim as fim_data,
		loja,
		Pdv = 1
	from Caixa_1


GO


IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosEntregando' ) 
   DROP VIEW Vw_GestorPedidosEntregando
GO

Create View [dbo].[Vw_GestorPedidosEntregando]
as
	Select * From PedidoVapVupt Where Situacao = 2 And Isnull(TipoPedido, 'Entregar') = 'Entregar' 
GO

IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosEntreges' ) 
   DROP VIEW Vw_GestorPedidosEntreges
GO

Create View [dbo].[Vw_GestorPedidosEntreges]
as
	Select * From PedidoVapVupt Where Situacao = 5 And Isnull(TipoPedido, 'Entregar') = 'Entregar'
GO


IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosRetirar' ) 
   DROP VIEW Vw_GestorPedidosRetirar
GO

Create View [dbo].[Vw_GestorPedidosRetirar]
as
	Select * From PedidoVapVupt Where Situacao = 2 And Isnull(TipoPedido, 'Entregar') = 'Retirar'
GO



IF NOT EXISTS(SELECT NAME FROM SYSCOLUMNS WHERE NAME = 'Estacao' AND ID IN
     (SELECT ID FROM SYSOBJECTS WHERE NAME = 'Venda_1'))
      Alter table Venda_1 Add Estacao Varchar(100)
GO

IF NOT EXISTS(SELECT NAME FROM SYSCOLUMNS WHERE NAME = 'PagamentoOnline' AND ID IN
     (SELECT ID FROM SYSOBJECTS WHERE NAME = 'Venda_1'))
      Alter table Venda_1 Add PagamentoOnline Bit
GO
IF NOT EXISTS(SELECT NAME FROM SYSCOLUMNS WHERE NAME = 'ValorVoucher' AND ID IN
     (SELECT ID FROM SYSOBJECTS WHERE NAME = 'Venda_1'))
      Alter table Venda_1 Add ValorVoucher Decimal(18,2)
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
		[data],
		hora,
		tipo,
		vendedor,
		Loja,
		Codigo,
		cpfCliente, 
		Estacao, 
		AppId, 
		PagamentoOnline, 
		ValorVoucher, 
		Observacao,
		COTACAO,
		CONDICAO_PGTO,
		tabela_venda_cod,
		REFERENCIAL,
		NROTRANSP
		) 
		values (
		@nro,
		@data,
		@horaStr,
		@tipo,
		@vend,
		@Loja,
		@Cod_cli,
		@cpfcnpj, 
		@Estacao, 
		@AppId, 
		@PagamentoOnline, 
		@ValorVoucher, 
		@OBS,
		0, --COTACAO
		1, -- CONDICAO_PGTO
		0, --tabela_venda_cod
		1, --REFERENCIAL
		0  --NROTRANSP
		)


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

	Insert Into Venda_2 (
		
		NRO,
		QTDE,
		codigo,
		unitario,
		TOTAL,
		desconto,
		OBSERVACAO, 
		CardexVLCusto
		) Values (
		@NRO,
		@QTDE,
		@COD_PROD,
		@UNIT,
		@TOTAL,
		@PERC,
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
	update venda_1 Set Valor_Frete = @Taxa_Adicional Where Nro = @Nro_Venda;
		
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

	declare @ddd Varchar(2), @celular varchar(9), @BairroId Int

	If(Len(@Fone) = 10)
	begin
		Set @ddd = Substring(@Fone, 1,2)
		Set @Fone = Substring(@Fone, 3,8)

	end
	else if(Len(@Fone) = 11)
	Begin
		Set @ddd = Substring(@Fone, 1,2)
		Set @Fone = Substring(@Fone, 3,9)
	End

	Set @BairroId = (Select top 1 CODIGO from BAIRROS Where BAIRRO = @Bairro)

	if(@BairroId = null Or @BairroId = 0)
	Begin
		Insert Into BAIRROS (BAIRRO, CODIGO_ROTA ) values (@Bairro, 1)
		Set @BairroId = @@identity
	End

	declare @clienteId Int 
	Set @clienteId = Isnull((Select Codigo From Clientes Where Fone1 = @fone),0)

	If(@clienteId = 0)
	Begin
		Insert Into Clientes(Fone1, ddd, Nome, Endereço, Bairro, COD_BAIRRO, Cep, Cidade, Estado, CNPJ, observacao) Values (
			@Fone, @ddd, @Nome, DBO.FN_REMOVEACENTUACAO(@Endereco), @Bairro, @BairroId, @Cep, @Cidade, @UF, @CPF, @Obs1)

		Set @clienteId = (Select @@Identity)
	End
	Else
	Begin
		Update Clientes Set 
			Fone1 = @Fone,
			ddd = @ddd,
			Endereço = DBO.FN_REMOVEACENTUACAO(@Endereco),
			Bairro = @Bairro,
			COD_BAIRRO = @BairroId,
			Cep = @Cep,
			Cidade = @Cidade,
			Estado = @UF,
			CNPJ = @CPF
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

End

GO
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_MeioMeio' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_MeioMeio
GO
Create procedure PR_Gestor_Insert_MeioMeio
	@Descricao Varchar(255), 
	@NroOperacao Int, 
	@TipoOperacao Int, 
	@IdProduto Int, 
	@SeqProduto Int,
	@ValorVenda Decimal(18,2), 
	@Quantidade Decimal(18,3), 
	@QtdeImpresso Int, 
	@DataHora DateTime, 
	@Observacao Varchar(800)
As
Begin
	Set @Observacao = 'Não tem meio meio'
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
	Set @NROCARTAO = 'Não tem complementos'
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
	@Retirar Int
As
Begin
		Select 'Não existe este recurso'
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

	UPDATE SEQ_TABELA SET SEQUENCIA = (ISNULL(SEQUENCIA,0) + 1) WHERE TABELA = 'VENDA_1' AND Campo = 'NRO'

	SET @RESULT = isnull((select top 1 SEQUENCIA from seq_tabela where TABELA = 'VENDA_1' AND Campo = 'NRO'),0)

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

	SET @RESULT = ISNULL((select TOP 1 valor from Config where variavel like @VARIAVEL),'')

	SELECT @RESULT;
END
