using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class OrdineCreateViewModel
    {
        public USER User { get; set; }
        public List<DETTAGLIO> Dettagli { get; set; }
    }
}