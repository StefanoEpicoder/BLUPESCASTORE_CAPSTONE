using BLUPESCASTORE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace BLUPESCASTORE.Controllers
{
    public class RegistrazioneController : Controller
    {
        private ModelDBcontext _context;

        public RegistrazioneController()
        {
            _context = new ModelDBcontext();
        }

        // Metodo GET per visualizzare il form di registrazione
        public ActionResult RegistraUtente()
        {
            return View();
        }

        // Metodo POST per gestire la sottomissione del form di registrazione
        [HttpPost]
        public ActionResult RegistraUtente(REGISTRAZIONE model)
        {
            if (ModelState.IsValid)
            {
                // Verifica se l'username esiste già nel database
                if (_context.USER.Any(u => u.Username == model.Username))
                {
                    TempData["ErrorMessage"] = "Username esistente.";
                    return View(model);
                }

                var user = new USER
                {
                    Username = model.Username,
                    Pass = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    Nome = model.Nome,
                    Cognome = model.Cognome,
                    Cod_Fiscale = model.Cod_Fiscale,
                    Citta = model.Citta,
                    Prov = model.Prov,
                    Tel_Cell = model.Tel_Cell,
                    email = model.Email,
                    Ruolo = "Utente" // Assegna il ruolo "Utente"
                };

                _context.USER.Add(user);

                var errors = _context.GetValidationErrors();
                if (!errors.Any())
                {
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Home"); // Reindirizza alla pagina di login dopo la registrazione
                }
                else
                {
                    foreach (var error in errors)
                    {
                        foreach (var validationError in error.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Proprietà: " + validationError.PropertyName + " Errore: " + validationError.ErrorMessage);
                        }
                    }
                    ViewBag.ErrorMessage = "Si è verificato un errore durante la registrazione.";
                    return View(model); // Ritorna alla vista con il modello corrente in caso di errore
                }
            }

            return View(model);
        }



    }
}


/* [HttpPost]
 public ActionResult CompletaRegistrazione(string username, string nome, string cognome, string codFiscale, string citta, string prov, string telCell, string email)
 {
     var user = _context.USER.FirstOrDefault(u => u.Username == username);
     if (user != null)
     {
         user.Nome = nome;
         user.Cognome = cognome;
         user.Cod_Fiscale = codFiscale;
         user.Citta = citta;
         user.Prov = prov;
         user.Tel_Cell = telCell;
         user.email = email;

         _context.SaveChanges();

         return RedirectToAction("Index", "Home");
     }

     return View();
 }*/



