﻿namespace BLUPESCASTORE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ORDINE")]
    public partial class ORDINE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ORDINE()
        {
            DETTAGLIO = new HashSet<DETTAGLIO>();
        }

        [Key]
        public int IdOrdine { get; set; }


        [StringLength(50)]
        public string Note { get; set; }

        [Required]
        [StringLength(50)]
        public string Confermato { get; set; }

        [Required]
        [Display(Name = "Totale da pagare")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotaleImporto { get; set; }

        [Required]
        [StringLength(50)]
        public string Evaso { get; set; }

        public int IdUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETTAGLIO> DETTAGLIO { get; set; }

        public virtual USER USER { get; set; }

        [Display(Name = "Numero Ordine")]
        public string NumeroOrdine { get; set; }


    }
}