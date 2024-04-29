using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BLUPESCASTORE.Models
{
    public class REGISTRAZIONE
    {
        internal string email;

        public REGISTRAZIONE() { }

        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La {0} deve essere almeno {2} caratteri di lunghezza.", MinimumLength = 4)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Conferma password")]
        [Compare("Password", ErrorMessage = "La password e la password di conferma non corrispondono.")]
        public string ConfirmPassword { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Il Codice Fiscale deve essere di 16 caratteri.", MinimumLength = 16)]
        public string Cod_Fiscale { get; set; }


        public string Citta { get; set; }
        public string Prov { get; set; }
        public string Tel_Cell { get; set; }
        public string Email { get; set; }
    }
}