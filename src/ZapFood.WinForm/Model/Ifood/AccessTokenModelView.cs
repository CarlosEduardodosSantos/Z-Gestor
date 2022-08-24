using System;

namespace ZapFood.WinForm.Model.Ifood
{
    public class AccessTokenModelView
    {
        public string accesstoken { get; set; }
        public string refreshToken { get; set; }
        public string tokentype { get; set; }
        public int expiresin { get; set; }
        public string scope { get; set; }
        public string tokensystem { get; set; }
        public DateTime datacreate { get; set; }
        public DateTime dataRefresh => datacreate.AddSeconds(expiresin);
    }
}