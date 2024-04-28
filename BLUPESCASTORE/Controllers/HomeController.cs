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
        private readonly ModelDBcontext db;

        public HomeController(ModelDBcontext context)
        {
            db = context;
        }


        //metodo per mostrare gli ultimi 5 articoli inseriti
        public ActionResult Index()
        {
            var ultimiArticoli = db.ARTICOLO
                .OrderByDescending(a => a.IdArticolo)
                .Take(8)
                .ToList();

            return View(ultimiArticoli);
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
                var userData = u.IdUser.ToString();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    u.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    userData,
                    FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

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