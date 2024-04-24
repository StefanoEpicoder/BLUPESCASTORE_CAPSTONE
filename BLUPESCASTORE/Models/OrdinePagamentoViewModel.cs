using BLUPESCASTORE.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class OrdinePagamentoViewModel
    {
        public ORDINE Ordine { get; set; }
        public PAGAMENTI Pagamento { get; set; }
        public List<DETTAGLIO> Dettagli { get; internal set; }

        public ARTICOLO Articolo { get; set; }
    }

}