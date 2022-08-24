using System;

namespace ZapFood.WinForm.Model.Ifood
{
    public class PollingModel
    {

        public string id { get; set; }
        public string code { get; set; }
        public string fullCode { get; set; }
        public SituacaoVapVuptEnum situacao => ObterSituacao();
        public string orderId { get; set; }
        public DateTime createdAt { get; set; }

        private SituacaoVapVuptEnum ObterSituacao()
        {
            if (fullCode == "PLACED") return SituacaoVapVuptEnum.INTEGRATED;

            if (fullCode == "REQUEST_DRIVER_AVAILABILITY") return SituacaoVapVuptEnum.CONFIRMED;

            try
            {
                return (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), fullCode, true);
            }
            catch
            {

                return SituacaoVapVuptEnum.OUTROS;
            }
            
        }

    }


    public class poolingRootobject
    {
        public DateTime createdAt { get; set; }
        public string fullCode { get; set; }
        public Metadata metadata { get; set; }
        public string code { get; set; }
        public string orderId { get; set; }
        public string id { get; set; }
    }

    public class Metadata
    {
        public Additionalprop1 additionalProp1 { get; set; }
        public Additionalprop2 additionalProp2 { get; set; }
        public Additionalprop3 additionalProp3 { get; set; }
    }

    public class Additionalprop1
    {
    }

    public class Additionalprop2
    {
    }

    public class Additionalprop3
    {
    }

}