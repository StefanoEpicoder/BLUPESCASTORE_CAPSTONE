using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLUPESCASTORE.Models;
using Stripe;

namespace BLUPESCASTORE.Controllers
{
    [Authorize]
    public class OrdineController : Controller
    {
        private ModelDBcontext db = new ModelDBcontext();

        // GET: Ordine
        public ActionResult Index()
        {
            // Recupera tutti gli ordini
            var ordini = db.ORDINE.Include(o => o.USER).Include(o => o.DETTAGLIO).ToList();

            // Crea una lista di modelli di vista
            var viewModel = new List<OrdinePagamentoViewModel>();

            // Popola la lista di modelli di vista
            foreach (var ordine in ordini)
            {
                // Recupera il pagamento corrispondente all'ordine, se esiste
                var pagamento = db.PAGAMENTI.FirstOrDefault(p => p.IdOrdine == ordine.IdOrdine);

                // Recupera i dettagli dell'ordine
                var dettagli = db.DETTAGLIO.Where(d => d.IdOrdine == ordine.IdOrdine).ToList();

                viewModel.Add(new OrdinePagamentoViewModel
                {
                    Ordine = ordine,
                    Pagamento = pagamento,
                    Dettagli = dettagli
                });
            }

            // Passa il modello di vista alla vista
            return View(viewModel);
        }



        public ActionResult OrdineConfermato()
        {
            // Recupera l'utente corrente
            var currentUser = User.Identity.Name;

            // Recupera solo gli ordini dell'utente corrente
            var ordini = db.ORDINE.Where(o => o.USER.Username == currentUser).ToList();

            // Crea una lista di modelli di vista
            var viewModel = new List<OrdinePagamentoViewModel>();

            // Popola la lista di modelli di vista
            foreach (var ordine in ordini)
            {
                // Recupera il pagamento corrispondente all'ordine, se esiste
                var pagamento = db.PAGAMENTI.FirstOrDefault(p => p.IdOrdine == ordine.IdOrdine);

                // Recupera i dettagli dell'ordine
                var dettagli = db.DETTAGLIO.Where(d => d.IdOrdine == ordine.IdOrdine).ToList();

                viewModel.Add(new OrdinePagamentoViewModel
                {
                    Ordine = ordine,
                    Pagamento = pagamento,
                    Dettagli = dettagli
                });
            }

            // Passa il modello di vista alla vista
            return View(viewModel);
        }





        // GET: Ordine/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ORDINE ordine = db.ORDINE.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }

            // Recupera il pagamento corrispondente all'ordine, se esiste
            var pagamento = db.PAGAMENTI.FirstOrDefault(p => p.IdOrdine == ordine.IdOrdine);

            // Recupera i dettagli dell'ordine
            var dettagli = db.DETTAGLIO.Where(d => d.IdOrdine == ordine.IdOrdine).ToList();

            // Crea un modello di vista
            var viewModel = new OrdinePagamentoViewModel
            {
                Ordine = ordine,
                Pagamento = pagamento,
                Dettagli = dettagli
            };

            return View(viewModel);
        }


        // GET: Ordine/Create
        public ActionResult Create()
        {
            ORDINE o = new ORDINE();
            USER u = db.USER.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
            List<DETTAGLIO> d = db.DETTAGLIO.Where(x => x.IdOrdine == null && x.IdUser == u.IdUser).ToList();
            o.TotaleImporto = d.Sum(x => x.PrezzoTotale);
            o.IdUser = u.IdUser;
            o.DETTAGLIO = d;
            o.USER = u;

            // Genera un numero di ordine casuale
            Random random = new Random();
            o.NumeroOrdine = random.Next(10000, 99999).ToString();

            return View(o);
        }

        // POST: Ordine/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ORDINE o)
        {
            o.Confermato = "No";
            o.Evaso = "No";
            db.ORDINE.Add(o); // Aggiungi l'ordine al contesto del database
            db.SaveChanges(); // Salva tutte le modifiche nel database

            // Recupera i dettagli dell'ordine
            var dettagli = db.DETTAGLIO.Include(d => d.ARTICOLO).Where(x => x.IdOrdine == null && x.IdUser == o.IdUser).ToList();

            // Salva i dettagli nell' TempData
            TempData["Dettagli"] = dettagli;

            // Reindirizza alla View Charge
            return RedirectToAction("Charge", new { id = o.IdOrdine });
        }




        // GET: Ordine/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDINE oRDINE = db.ORDINE.Find(id);
            if (oRDINE == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUser = new SelectList(db.USER, "IdUser", "Username", oRDINE.IdUser);
            return View(oRDINE);
        }

        // POST: Ordine/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrdine,Note,Confermato,Importo,TotaleImporto,Evaso,IdUser")] ORDINE oRDINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oRDINE).State = EntityState.Modified;
                db.SaveChanges();

                // Add toast notification
                TempData["ToastMessage"] = "L'ordine è stato modificato con successo.";

                return RedirectToAction("Index");
            }
            ViewBag.IdUser = new SelectList(db.USER, "IdUser", "Username", oRDINE.IdUser);
            return View(oRDINE);
        }

        // GET: Ordine/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDINE oRDINE = db.ORDINE.Find(id);
            if (oRDINE == null)
            {
                return HttpNotFound();
            }
            return View(oRDINE);
        }

        // POST: Ordine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Trova e rimuovi prima i record DETTAGLIO correlati
            var dettagli = db.DETTAGLIO.Where(d => d.IdOrdine == id);
            db.DETTAGLIO.RemoveRange(dettagli);
            db.SaveChanges();

            // Poi trova e rimuovi i record PAGAMENTI correlati
            var pagamenti = db.PAGAMENTI.Where(p => p.IdOrdine == id);
            db.PAGAMENTI.RemoveRange(pagamenti);
            db.SaveChanges();

            // Ora trova e rimuovi il record ORDINE
            ORDINE oRDINE = db.ORDINE.Find(id);
            db.ORDINE.Remove(oRDINE);
            db.SaveChanges();

            // Aggiungi notifica toast
            TempData["ToastMessage"] = "L'ordine è stato eliminato con successo.";

            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        //metodo per effettuare i pagamenti
        [HttpPost]
        public ActionResult Charge(string stripeEmail, string stripeToken, int idOrdine, string nome, string cognome, string email, string indirizzo, string indirizzo2, string paese, string regione, string citta, string cap)
        {
            ORDINE o = db.ORDINE.Find(idOrdine);
            if (o == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            StripeConfiguration.ApiKey = "sk_test_51P6CweEooIqhcjUCAgQqptq1wYYDszK9rmadX5KJwRtllqqwv3s56bnZpdRhTtpsx3X0Zr2ky4MLnfO34Dpkcg4P004SeoWwmo";

            var customers = new Stripe.CustomerService();
            var charges = new Stripe.ChargeService();

            var customer = customers.Create(new Stripe.CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            decimal importoArrotondato = Math.Round(o.TotaleImporto, 2);
            var charge = charges.Create(new Stripe.ChargeCreateOptions
            {
                Amount = (int)(importoArrotondato * 100),
                Description = "Pagamento per ordine " + o.IdOrdine,
                Currency = "eur",
                Customer = customer.Id
            });

            // Se il pagamento è stato effettuato con successo, salva l'ordine e il pagamento nel database
            if (charge.Paid)
            {
                USER u = db.USER.Where(x => x.Username == User.Identity.Name).FirstOrDefault();
                var pagamento = new PAGAMENTI
                {
                    IdUser = u.IdUser,
                    IdOrdine = o.IdOrdine,
                    Importo = o.TotaleImporto,
                    DataPagamento = DateTime.Now,
                    StripeChargeId = charge.Id,
                    Nome = nome,
                    Cognome = cognome,
                    Email = email,
                    Indirizzo = indirizzo,
                    Indirizzo2 = indirizzo2,
                    Paese = paese,
                    Regione = regione,
                    Citta = citta,
                    CAP = cap
                };
                db.PAGAMENTI.Add(pagamento);

                // Conferma l'ordine
                o.Confermato = "Si";
                db.Entry(o).State = EntityState.Modified;

                db.SaveChanges();

                List<DETTAGLIO> d = db.DETTAGLIO.Where(x => x.IdOrdine == null && x.IdUser == u.IdUser).ToList();
                foreach (var dettaglio in d)
                {
                    db.DETTAGLIO.Remove(dettaglio);
                }
                db.SaveChanges();
            }

            return RedirectToAction("OrdineConfermato");
        }








        public ActionResult Charge(int id, List<DETTAGLIO> dettagli)
        {
            ORDINE o = db.ORDINE.Find(id);
            if (o == null)
            {
                // Gestisci l'errore, ad esempio reindirizzando a una pagina di errore o restituendo un errore HTTP
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Passa l'ordine e i dettagli dell'ordine alla vista
            ViewBag.Dettagli = dettagli;
            return View(o);
        }


    }
}
