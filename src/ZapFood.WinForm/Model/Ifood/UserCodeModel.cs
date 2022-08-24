using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapFood.WinForm.Model.Ifood
{
    public class UserCodeModel
    {
        public string userCode { get; set; }
        public string authorizationCodeVerifier { get; set; }
        public string verificationUrl { get; set; }
        public string verificationUrlComplete { get; set; }
        public int expiresIn { get; set; }
    }

}
