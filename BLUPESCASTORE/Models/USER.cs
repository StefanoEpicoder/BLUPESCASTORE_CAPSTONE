namespace BLUPESCASTORE.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;

    [Table("USER")]
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            DETTAGLIO = new HashSet<DETTAGLIO>();
            ORDINE = new HashSet<ORDINE>();

        }

        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string Pass { get; set; }

        [Required]
        [StringLength(50)]
        public string Ruolo { get; set; }

        public string Nome { get; set; }
        public string Cognome { get; set; }

        [Display(Name = "Codice Fiscale")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Codice Fiscale incompleto")]
        [Remote("CFEsistente", "USER", ErrorMessage = "Cliente già inserito")]
        public string Cod_Fiscale { get; set; }

        [Display(Name = "Città")]
        public string Citta { get; set; }

        // Attributi di validazione per la provincia
        [StringLength(3, MinimumLength = 2, ErrorMessage = "Inserire Sigla Provincia")]
        [Display(Name = "Provincia")]
        public string Prov { get; set; }

        [Display(Name = "Telefono")]
        public string Tel_Cell { get; set; }

        // Attributi di validazione per l'indirizzo email
        [EmailAddress(ErrorMessage = "Inserire indirizzo e-mail valido")]
        [Display(Name = "E-mail")]
        public string email { get; set; }




        //AUTENTICAZIONE
        public static bool Autenticato(string username, string password)
        {
            SqlConnection con = Connessioni.GetConnection();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [USER] WHERE Username = @username and [Pass]=@Password", con);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'autenticazione: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETTAGLIO> DETTAGLIO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDINE> ORDINE { get; set; }
        public object PasswordHash { get; internal set; }
        public string ConfirmPassword { get; internal set; }
    }
}