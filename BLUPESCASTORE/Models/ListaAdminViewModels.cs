using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class ListaAdminViewModel
    {
        public ListaAdminViewModel()
        {
            Articoli = new List<ARTICOLO>();
            Categorie = new List<CATEGORIA>();
        }

        public List<ARTICOLO> Articoli { get; set; }
        public List<CATEGORIA> Categorie { get; set; }
    }
}
