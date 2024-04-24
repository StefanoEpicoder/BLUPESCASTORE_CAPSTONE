using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class OrdineCarrelloViewModel
    {
        public ORDINE Ordine { get; set; }
        public List<DETTAGLIO> DettagliCarrello { get; set; }
        public decimal TotaleImporto { get; set; }
        public string NumeroOrdine { get; set; }
        public int IdOrdine { get; set; }
  
        public string StripeEmail { get; internal set; }
        public string StripeToken { get; set; }

        public string Nome { get; internal set; }

        public string Cognome { get; internal set; }

        public string Email { get; internal set; }

        public string Indirizzo { get; internal set; }

        public string Indirizzo2 { get; internal set; }

        public string Paese { get; internal set; }

        public string Regione { get; internal set; }

        public string Citta { get; internal set; }

        public string CAP { get; internal set; }
    }
}