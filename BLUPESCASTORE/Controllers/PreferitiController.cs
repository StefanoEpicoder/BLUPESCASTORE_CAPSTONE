using BLUPESCASTORE.Models;
using System.Web.Mvc;
using System.Web.Security;
using System;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using BLUPESCASTORE;


public class PreferitiController : Controller
{
    private readonly ModelDBcontext db;

    public PreferitiController()
    {
        db = new ModelDBcontext();
    }

    [HttpPost]
    public ActionResult AddToPreferitiPost(int idArticolo)
    {
        string username = User.Identity.Name; // Ottieni l'username dell'utente corrente
        bool success = AddToPreferiti(username, idArticolo);

        if (!success)
        {
            // Gestisci l'errore: non è stato possibile aggiungere l'articolo ai preferiti
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        // Restituisci un codice di stato 200
        return new HttpStatusCodeResult(HttpStatusCode.OK);
    }





    public bool AddToPreferiti(string username, int idArticolo)
    {
        // Trova l'IdUser corrispondente all'Username
        var user = db.USER.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            // Gestisci l'errore: non esiste un utente con questo username
            return false;
        }

        var preferito = new PREFERITI
        {
            IdUser = user.IdUser,
            IdArticolo = idArticolo
        };

        db.PREFERITI.Add(preferito);
        db.SaveChanges();

        return true;
    }


    [HttpGet]
    public ActionResult GetPreferiti()
    {
        string username = User.Identity.Name; // Ottieni l'username dell'utente corrente

        // Trova l'IdUser corrispondente all'Username
        var user = db.USER.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            // Gestisci l'errore: non esiste un utente con questo username
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        // Ottieni gli ID degli articoli preferiti dell'utente
        var preferiti = db.PREFERITI.Where(p => p.IdUser == user.IdUser).Select(p => p.IdArticolo).ToList();

        return Json(preferiti, JsonRequestBehavior.AllowGet);
    }




    [HttpPost]
    public ActionResult RemoveFromPreferitiPost(int idArticolo)
    {
        string username = User.Identity.Name; // Ottieni l'username dell'utente corrente
        bool success = RemoveFromPreferiti(username, idArticolo);

        if (!success)
        {
            // Gestisci l'errore: non è stato possibile rimuovere l'articolo dai preferiti
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        // Restituisci un codice di stato 200
        return new HttpStatusCodeResult(HttpStatusCode.OK);
    }

    public bool RemoveFromPreferiti(string username, int idArticolo)
    {
        // Trova l'IdUser corrispondente all'Username
        var user = db.USER.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            // Gestisci l'errore: non esiste un utente con questo username
            return false;
        }

        // Trova l'articolo preferito corrispondente all'IdUser e all'IdArticolo
        var preferito = db.PREFERITI.FirstOrDefault(p => p.IdUser == user.IdUser && p.IdArticolo == idArticolo);
        if (preferito == null)
        {
            // Gestisci l'errore: l'articolo non è nei preferiti dell'utente
            return false;
        }

        // Rimuovi l'articolo dai preferiti
        db.PREFERITI.Remove(preferito);
        db.SaveChanges();

        return true;

    }

    [HttpGet]
    public bool IsInPreferiti(int idArticolo)
    {
        string username = User.Identity.Name; // Ottieni l'username dell'utente corrente

        // Trova l'IdUser corrispondente all'Username
        var user = db.USER.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            // Gestisci l'errore: non esiste un utente con questo username
            return false;
        }

        // Verifica se esiste un record nei preferiti per l'utente corrente e l'ID dell'articolo specificato
        var preferito = db.PREFERITI.FirstOrDefault(p => p.IdUser == user.IdUser && p.IdArticolo == idArticolo);

        // Se il record esiste, restituisci true, altrimenti restituisci false
        return preferito != null;
    }


    //metodo per visualizzare gli articoli preferiti
    public ActionResult Preferiti()
    {
        string username = User.Identity.Name; // Ottieni l'username dell'utente corrente

        // Trova l'IdUser corrispondente all'Username
        var user = db.USER.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            // Gestisci l'errore: non esiste un utente con questo username
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        // Ottieni gli IdArticolo dei preferiti dell'utente
        var idArticoliPreferiti = db.PREFERITI
            .Where(p => p.IdUser == user.IdUser)
            .Select(p => p.IdArticolo)
            .ToList();

        // Ottieni gli articoli corrispondenti agli IdArticolo
        var preferiti = db.ARTICOLO
            .Where(a => idArticoliPreferiti.Contains(a.IdArticolo))
            .ToList();

        return View(preferiti);
    }





}


