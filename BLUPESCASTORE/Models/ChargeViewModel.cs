using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class ChargeViewModel
    {
        public ORDINE Ordine { get; set; }
        public List<DETTAGLIO> Dettaglio { get; set; }
    }
}
