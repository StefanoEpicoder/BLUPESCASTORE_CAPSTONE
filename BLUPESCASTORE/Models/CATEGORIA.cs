using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class CATEGORIA
    {
        [Key]
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; }

    }
}