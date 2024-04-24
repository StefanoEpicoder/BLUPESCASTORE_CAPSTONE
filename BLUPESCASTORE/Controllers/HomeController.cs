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
                FormsAuthentication.SetAuthCookie(u.Username, false);
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