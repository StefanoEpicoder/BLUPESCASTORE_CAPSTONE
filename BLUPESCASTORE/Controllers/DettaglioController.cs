using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLUPESCASTORE.Models;

namespace BLUPESCASTORE.Controllers
{
    [Authorize]
    public class DettaglioController : Controller
    {
        private ModelDBcontext db = new ModelDBcontext();

        // GET: Dettaglio
        //metodo per aggiungere articoli al carrello
        public ActionResult Index(int id, int quantity)
        {
            USER utente = db.USER.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            if (id > 0)
            {
                DETTAGLIO d = new DETTAGLIO();
                d.IdArticolo = id;
                d.Quantita = quantity;
                ARTICOLO p = db.ARTICOLO.Find(id);
                d.PrezzoTotale = (int)(p.Prezzo * d.Quantita);
                d.IdUser = utente.IdUser;


                db.DETTAGLIO.Add(d);
                db.SaveChanges();
            }

            return View(db.DETTAGLIO.Where(x => x.IdOrdine == null && x.IdUser == utente.IdUser).ToList());
        }

        public ActionResult Carrello()
        {
            USER utente = db.USER.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            List<DETTAGLIO> dtg = db.DETTAGLIO.Include(x => x.ARTICOLO).Where(d => d.IdOrdine == null && d.IdUser == utente.IdUser).ToList();
            return View(dtg);
        }

        // GET: Dettaglio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DETTAGLIO dETTAGLIO = db.DETTAGLIO.Find(id);
            if (dETTAGLIO == null)
            {
                return HttpNotFound();
            }
            return View(dETTAGLIO);
        }

        // GET: Dettaglio/Create
        public ActionResult Create()
        {
            ViewBag.IdOrdine = new SelectList(db.ORDINE, "IdOrdne", "Note");
            ViewBag.IdPizza = new SelectList(db.ARTICOLO, "IdArticolo", "NomeArticolo");
            return View();
        }

        // POST: Dettaglio/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDettaglio,Quantita,IdArticolo,IdOrdine")] DETTAGLIO dETTAGLIO)
        {
            if (ModelState.IsValid)
            {
                db.DETTAGLIO.Add(dETTAGLIO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdOrdine = new SelectList(db.ORDINE, "IdOrdne", "Note", dETTAGLIO.IdOrdine);
            ViewBag.IdArticolo = new SelectList(db.ARTICOLO, "IdArticolo", "NomeArticolo", dETTAGLIO.IdArticolo);
            return View(dETTAGLIO);
        }

        // GET: Dettaglio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DETTAGLIO d = db.DETTAGLIO.Find(id);


            return View(d);
        }

        // POST: Dettaglio/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DETTAGLIO d)
        {
            if (ModelState.IsValid)
            {
                USER utente = db.USER.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
                DETTAGLIO dettaglio = db.DETTAGLIO.Find(d.IdDettaglio);
                d.PrezzoTotale = dettaglio.ARTICOLO.Prezzo * d.Quantita;
                d.IdUser = utente.IdUser;
                ModelDBcontext db1 = new ModelDBcontext();
                db1.Entry(d).State = EntityState.Modified;
                db1.SaveChanges();
            }

            return RedirectToAction("Carrello");
        }

        // GET: Dettaglio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DETTAGLIO dETTAGLIO = db.DETTAGLIO.Find(id);
            if (dETTAGLIO == null)
            {
                return HttpNotFound();
            }
            return View(dETTAGLIO);
        }

        // POST: Dettaglio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DETTAGLIO dETTAGLIO = db.DETTAGLIO.Find(id);
            db.DETTAGLIO.Remove(dETTAGLIO);
            db.SaveChanges();
            return RedirectToAction("Carrello");
        }

        public int GetCount()
        {
            // Ottieni l'ID dell'utente corrente
            var userId = db.USER.Where(x => x.Username == User.Identity.Name).FirstOrDefault().IdUser;

            // Conta il numero totale di articoli nel carrello dell'utente
            int count = db.DETTAGLIO.Where(x => x.IdOrdine == null && x.IdUser == userId)
                                    .Select(x => x.Quantita)
                                    .DefaultIfEmpty(0)
                                    .Sum();

            return count;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult _DettaglioPartial()
        {
            // Recupera una lista di oggetti DETTAGLIO dal tuo database
            List<DETTAGLIO> dettagli = db.DETTAGLIO.ToList();

            // Passa la lista alla vista
            return View(dettagli);
        }

    }
}