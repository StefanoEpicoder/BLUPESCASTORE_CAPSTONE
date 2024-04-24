namespace BLUPESCASTORE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using BLUPESCASTORE.Models;

    [Table("DETTAGLIO")]
    public partial class DETTAGLIO
    {
        [Key]
        public int IdDettaglio { get; set; }

        public int Quantita { get; set; }


        [Display(Name = "Totale per articolo")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal PrezzoTotale { get; set; }


        public int IdArticolo { get; set; }

        public int? IdOrdine { get; set; }

        public int IdUser { get; set; }

        public virtual ORDINE ORDINE { get; set; }

        public virtual ARTICOLO ARTICOLO { get; set; }

        public virtual USER USER { get; set; }

    }
}
