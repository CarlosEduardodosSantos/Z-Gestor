-- =================================== integração APP | PHARM =====================================================


-- verificar seq_tabela se tem nro pra vendas pendentes 

-- insert into SEQ_TABELA (tabela,coluna,sequencia) values ('venda_pendente','nro','1')




-- Cria Tabela | Prod
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

-- Cria Tabela | Usuários
IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'VW_VapVuptUsuario' ) 
   DROP VIEW VW_VapVuptUsuario
GO

Create View VW_VapVuptUsuario
as
	Select 
		Codigo as VendedorId,
		Nome,
		Senha
	from RECEP Where Ativo = 1


GO

-- Cria Tabela | Caixa
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

-- Cria Tabela | Entregando
IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosEntregando' ) 
   DROP VIEW Vw_GestorPedidosEntregando
GO

Create View [dbo].[Vw_GestorPedidosEntregando]
as
	Select * From PedidoVapVupt Where Situacao = 2 And Isnull(TipoPedido, 'Entregar') = 'Entregar' 
		And Isnull(VendaId,0) In (Select Nro_Venda From Televenda_1 Where Sai_Data Is not null And Ret_Data is null)
GO

-- Cria Tabela | Entregues
IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosEntreges' ) 
   DROP VIEW Vw_GestorPedidosEntreges
GO

Create View [dbo].[Vw_GestorPedidosEntreges]
as
	Select * From PedidoVapVupt Where Situacao = 5 And Isnull(TipoPedido, 'Entregar') = 'Entregar'
		And Isnull(VendaId,0) In (Select Nro_Venda From Televenda_1 Where Ret_Data is Not null)
GO


-- Cria Tabela | Retirar
IF EXISTS( SELECT NAME FROM SYSOBJECTS WHERE NAME = 'Vw_GestorPedidosRetirar' ) 
   DROP VIEW Vw_GestorPedidosRetirar
GO

Create View [dbo].[Vw_GestorPedidosRetirar]
as
	Select * From PedidoVapVupt Where Situacao = 2 And Isnull(TipoPedido, 'Entregar') = 'Retirar'
GO

-- Cria Variáveis | Tabela Venda_1
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

IF NOT EXISTS(SELECT NAME FROM SYSCOLUMNS WHERE NAME = 'AppId' AND ID IN
     (SELECT ID FROM SYSOBJECTS WHERE NAME = 'Venda_1'))
      Alter table Venda_1 Add AppId Varchar(50)
GO



-- ========================================== Venda_1 ================================================
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
		cpfcnpj, 
		Estacao, 
		AppId, 
		PagamentoOnline, 
		ValorVoucher, 
		OBS) 
		values (
		@nro,
		@nrocx,
		@data,
		@horaStr,
		@tipo,
		@vend,
		@Loja,
		@vl_compra,
		@Cod_cli,
		0, --Loja_transf
		0, --xFrete
		@xTele,
		@pdv,
		@data,
		@cpfcnpj, 
		@Estacao, 
		@AppId, 
		@PagamentoOnline, 
		@ValorVoucher, 
		@OBS)


		--Fidelidade


		Select @nro as VendaId
End

GO

-- ========================================== Venda_2 Itens ============================================
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
		COD_PROD,
		UNIT,
		TOTAL,
		PERC,
		VALOR,
		LOJA,
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
		@VlCusto
		)
End

GO


-- ========================================== TeleVenda_1 | Cabeçalho =========================================
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
		@travado,
		@Loja, 
		@BomPara, 
		@Condicao, 
		@Obs)
		
End


GO


-- ========================================== TeleVenda_2 | Cliente =========================================
IF EXISTS (SELECT NAME FROM SYSOBJECTS WHERE NAME = N'PR_Gestor_Insert_Cliente' AND TYPE = 'P')
   DROP PROCEDURE PR_Gestor_Insert_Cliente
GO

CREATE procedure [dbo].[PR_Gestor_Insert_Cliente]
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
	Set @clienteId = Isnull((Select Codigo From Televenda_2 Where Fone = @fone),0)

	If(@clienteId = 0)
	Begin

		Declare @codigo int
		set @codigo = Isnull((select Top 1 Isnull(SEQUENCIA,0) from seq_tabela Where tabela = 'Televenda_2'),0)+1
		Update seq_tabela Set SEQUENCIA = @codigo from seq_tabela Where tabela = 'Televenda_2'


		Insert Into Televenda_2(Codigo, Fone, Nome, Endereco, Bairro, Cep, Cidade, UF, CPF, ObsFixa) Values (
			@codigo, @Fone, @Nome, DBO.FN_REMOVEACENTUACAO(@Endereco), @Bairro, @Cep, @Cidade, @UF, @CPF, @Obs1)

		Set @clienteId = @codigo
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

End

GO




-- ========================================== Pendente | Venda_4 Pharm =======================================
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

	Insert Into Venda_4 
	       (
		     inc_venda4 
		   	,Nome
			,Hora
	        ,Forma
	        ,Condicao
         	,Loja
			,Loja_dest
			,Estoque
        	,Vendedor
	        ,codigo
         	,descricao
	        ,qtde
         	,unit
	        ,sub
         	,total
			,OBS_DESCONTO
			,desconto
	       )
	       Values 
		   ( 
		     @Nro,
			 @Cliente,
			 Cast (@data_abertura as date ),
			 'retira',
			 'a vista',
			 1,
			 1,
			 1,
			 1,
			 @Codigo,
			 @Descricao,
			 @Qtde,
			 @Unit,
			 @Total,
			 @Total,
			 '',
			 @Desconto
		   )
 
End
GO

-- verificar seq_tabela se tem nro pra vendas pendentes 

-- insert into SEQ_TABELA (tabela,coluna,sequencia) values ('vendas_pendentes','nro','1')

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



-- =========================================================================================================
-- =========================================================================================================
-- =========================================================================================================

-- ======================================================== meio a meio | não tem no PHARM =================
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
	Set @Observacao = 'Não tem meio meio'
End

GO

-- ========================================== Venda_4 | Complementos  =========================================
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