using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Data.Service.Interface
{
    public interface IAccessTokenService : IDataServiceBase<AccessTokenModelView>
    {
        AccessTokenModelView ObterVariavel(string tokenSystem);
    }
}