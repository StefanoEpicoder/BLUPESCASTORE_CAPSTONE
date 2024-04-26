using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class PREFERITI
    {
        [Key]
        public int IdPreferito { get; set; }
        public int IdUser { get; set; }
        public int IdArticolo { get; set; }

        [ForeignKey("IdUser")]
        public virtual USER User { get; set; }

        [ForeignKey("IdArticolo")]
        public virtual ARTICOLO Articolo { get; set; }
    }

}

