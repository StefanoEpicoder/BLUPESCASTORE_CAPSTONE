using System;

namespace BLUPESCASTORE.Controllers
{
    internal class Pagamento
    {
        public string IdUser { get; set; }
        public int IdOrdne { get; set; }
        public decimal Importo { get; set; }
        public DateTime DataPagamento { get; set; }
        public object StripeChargeId { get; set; }
    }
}