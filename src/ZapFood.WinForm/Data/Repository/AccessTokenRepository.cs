using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Data.Repository
{
    public class AccessTokenRepository : BaseRepository
    {
        public void VerificaTabela()
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].AccessToken') and 
                                       OBJECTPROPERTY(id, N'IsUserTable') = 1)
                                    BEGIN
	                                    Create Table AccessToken(
		                                    access_token Varchar(8000),
                                            refreshToken Varchar(8000),
		                                    token_type	Varchar(25),
		                                    expires_in	int,
		                                    scope Varchar(50),
                                            token_system Varchar(25),
                                            data_create DateTime
	                                    )
                                    END;");
     
            sql.AppendLine(@"IF NOT EXISTS(SELECT NAME FROM SYSCOLUMNS WHERE NAME = 'refreshToken' AND ID IN
                                 (SELECT ID FROM SYSOBJECTS WHERE NAME = 'AccessToken'))
	                            Alter Table AccessToken Add refreshToken Varchar(8000)");

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql.ToString());
                conn.Close();
            }
        }

        public void Salvar(AccessTokenModelView accessToken)
        {
            var sql = $"Insert Into AccessToken(access_token, token_type, expires_in, scope, token_system, data_create, refreshToken) " +
                      $"values(@access_token, @token_type, @expires_in, @scope, @token_system, @data_create, @refreshToken)";

            var parms = new DynamicParameters();
            parms.Add("@access_token", accessToken.accesstoken);
            parms.Add("@token_type", accessToken.tokentype);
            parms.Add("@expires_in", accessToken.expiresin);
            parms.Add("@scope", accessToken.scope);
            parms.Add("@token_system", accessToken.tokensystem);
            parms.Add("@data_create", accessToken.datacreate);
            parms.Add("@refreshToken", accessToken.refreshToken);



            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql, parms);
                conn.Close();
            }
        }

        public void Atualiza(AccessTokenModelView accessToken)
        {

            var verifica = ObterVariavel(accessToken.tokensystem);
            if (verifica == null)
            {
                Salvar(accessToken);
                return;
            }


            var sql = new StringBuilder();
            sql.AppendLine("Update AccessToken Set");
            sql.AppendLine("access_token = @access_token,");
            sql.AppendLine("token_type = @token_type,");
            sql.AppendLine("expires_in = @expires_in,");
            sql.AppendLine("scope = @scope,");
            sql.AppendLine("data_create = @data_create,");
            sql.AppendLine("refreshToken = @refreshToken");
            sql.AppendLine("Where token_system = @token_system");

            var parms = new DynamicParameters();
            parms.Add("@access_token", accessToken.accesstoken);
            parms.Add("@token_type", accessToken.tokentype);
            parms.Add("@expires_in", accessToken.expiresin);
            parms.Add("@scope", accessToken.scope);
            parms.Add("@token_system", accessToken.tokensystem);
            parms.Add("@data_create", accessToken.datacreate);
            parms.Add("@refreshToken", accessToken.refreshToken);

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }
        }

        public AccessTokenModelView ObterVariavel(string tokenSystem)
        {
            VerificaTabela();
            var sql = $"Select access_token as accesstoken, token_type as tokentype, expires_in as expiresin, scope, token_system as tokensystem, data_create as datacreate, refreshToken From AccessToken Where token_system like '{tokenSystem}'";
            using (var conn = Connection)
            {
                conn.Open();
                var access = conn.Query<AccessTokenModelView>(sql).FirstOrDefault();
                conn.Close();

                return access;
            }
        }
    }
}