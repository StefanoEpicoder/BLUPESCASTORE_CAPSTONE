using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class MenuArticoloIndexViewModel
    {
        public IEnumerable<ARTICOLO> Articoli { get; set; }
        public IEnumerable<CATEGORIA> Categorie { get; set; }
        public Dictionary<CATEGORIA, List<ARTICOLO>> ArticoliPerCategoria { get; set; }

    }
}