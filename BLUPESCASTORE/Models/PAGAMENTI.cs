using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class PAGAMENTI
    {
        [Key]
        public int IdPagamento { get; set; }

        [ForeignKey("User")]
        public int IdUser { get; set; }

        [ForeignKey("Ordine")]
        public int IdOrdine { get; set; }

        public decimal Importo { get; set; }

        public DateTime DataPagamento { get; set; }

        public string StripeChargeId { get; set; }

        // Navigation properties
        public virtual USER User { get; set; }
        public virtual ORDINE Ordine { get; set; }


        public virtual DETTAGLIO TotaleImporto { get; internal set; }
        public string Nome { get; internal set; }
        public string Indirizzo2 { get; internal set; }
        public string Cognome { get; internal set; }
        public string Email { get; internal set; }
        public string Indirizzo { get; internal set; }
        public string Paese { get; internal set; }
        public string Regione { get; internal set; }
        public string Citta { get; internal set; }
        public string CAP { get; internal set; }
    }
}