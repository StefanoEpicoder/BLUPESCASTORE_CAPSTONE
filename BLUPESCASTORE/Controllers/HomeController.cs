using BLUPESCASTORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BLUPESCASTORE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

[HttpPost]
public ActionResult Login(USER u)
{
    if (USER.Autenticato(u.Username, u.Pass))
    {
        var userData = u.IdUser.ToString(); // Usa 'IdUser' invece di 'Id'
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
            1,                             // versione del ticket
            u.Username,                    // nome utente associato al ticket
            DateTime.Now,                  // ora di creazione del ticket
            DateTime.Now.AddMinutes(30),   // ora di scadenza del ticket
            false,                         // se persistere il ticket nel cookie
            userData,                      // dati dell'utente da memorizzare nel ticket
            FormsAuthentication.FormsCookiePath); // percorso del cookie

        // Cripta il ticket
        string encTicket = FormsAuthentication.Encrypt(ticket);

        // Crea il cookie con il ticket criptato
        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

        TempData["ToastType"] = "success";
        TempData["ToastMessage"] = "Benvenuto, " + u.Username + "!";
        return Redirect(FormsAuthentication.DefaultUrl);
    }
    else
    {
        TempData["ToastType"] = "error";
        TempData["ToastMessage"] = "Username o password non corretti";
        return View();
    }
}


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Logout effettuato con successo!";
            return Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}