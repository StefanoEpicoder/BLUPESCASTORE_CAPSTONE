namespace BLUPESCASTORE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARTICOLO")]
    public partial class ARTICOLO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ARTICOLO()
        {
            DETTAGLIO = new HashSet<DETTAGLIO>();
        }

        [Key]
        public int IdArticolo { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Articolo")]
        public string NomeArticolo { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Prezzo unitario")]
        public decimal Prezzo { get; set; }

        [StringLength(50)]
        public string Foto { get; set; }

        public int IdCategoria { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETTAGLIO> DETTAGLIO { get; set; }

        public virtual CATEGORIA CATEGORIA { get; set; }
        public bool? Esaurito { get; set; }


    }
}