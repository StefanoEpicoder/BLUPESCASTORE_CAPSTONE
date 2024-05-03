using BLUPESCASTORE.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace BLUPESCASTORE.Controllers
{
    [Authorize(Roles = "Amministratore")]
    public class MenuArticoloController : Controller
    {
        private ModelDBcontext db = new ModelDBcontext();

        // GET: MenuArticolo
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new MenuArticoloIndexViewModel();
            model.Articoli = db.ARTICOLO.ToList();
            model.Categorie = db.CATEGORIA.ToList();
            model.ArticoliPerCategoria = db.CATEGORIA.ToDictionary(
                categoria => categoria,
                categoria => db.ARTICOLO.Where(articolo => articolo.CATEGORIA.IdCategoria == categoria.IdCategoria).ToList()
            );
            return View(model);
        }


        public ActionResult ListaAdmin()
        {
            return View(db.ARTICOLO.ToList());
        }

  
        // GET: MenuArticolo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICOLO aRTICOLO = db.ARTICOLO.Include(a => a.CATEGORIA).SingleOrDefault(a => a.IdArticolo == id);
            if (aRTICOLO == null)
            {
                return HttpNotFound();
            }
            return View(aRTICOLO);
        }

        // GET: MenuArticolo/Create
        public ActionResult Create()
        {
            // Recupera le categorie dal database
            var categorie = db.CATEGORIA.ToList();

            // Crea una SelectList con le categorie
            var listaCategorie = new SelectList(categorie, "IdCategoria", "NomeCategoria");

            // Passa la lista alla vista tramite ViewBag
            ViewBag.IdCategoria = listaCategorie;

            return View();
        }

        // POST: MenuArticolo/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ARTICOLO aRTICOLO, HttpPostedFileBase File, string Prezzo)
        {
            if (ModelState.IsValid)
            {
                if (aRTICOLO.Esaurito == null)
                {
                    aRTICOLO.Esaurito = false;
                }

                if (File != null)
                {
                    string Path = Server.MapPath("/Content/img/" + File.FileName);
                    File.SaveAs(Path);
                    aRTICOLO.Foto = File.FileName;
                }

                decimal prezzo;
                if (Decimal.TryParse(Prezzo.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out prezzo))
                {
                    prezzo = Math.Truncate(100 * prezzo) / 100;
                    aRTICOLO.Prezzo = prezzo;
                }
                else
                {
                    ModelState.AddModelError("Prezzo", "Il prezzo non è in un formato valido.");
                    return View(aRTICOLO);
                }

                if (aRTICOLO.InMagazzino == 0)
                {
                    aRTICOLO.Esaurito = true;
                }
                else
                {
                    aRTICOLO.Esaurito = false;
                }

                db.ARTICOLO.Add(aRTICOLO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var categorie = db.CATEGORIA.ToList();
            if (aRTICOLO.CATEGORIA != null)
            {
                ViewBag.IdCategoria = new SelectList(categorie, "IdCategoria", "NomeCategoria", aRTICOLO.CATEGORIA.IdCategoria);
            }
            else
            {
                ViewBag.IdCategoria = new SelectList(categorie, "IdCategoria", "NomeCategoria");
            }

            return View(aRTICOLO);
        }





        // GET: MenuArticolo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Include la categoria quando recuperi l'articolo dal database
            ARTICOLO aRTICOLO = db.ARTICOLO.Include(a => a.CATEGORIA).SingleOrDefault(a => a.IdArticolo == id);
            if (aRTICOLO == null)
            {
                return HttpNotFound();
            }

            // Recupera le categorie dal database
            var categorie = db.CATEGORIA.ToList();

            // Crea una SelectList con le categorie, impostando la categoria dell'articolo come selezionata
            var listaCategorie = new SelectList(categorie, "IdCategoria", "NomeCategoria", aRTICOLO.CATEGORIA.IdCategoria);

            // Passa la lista alla vista tramite ViewBag
            ViewBag.IdCategoria = listaCategorie;

            return View(aRTICOLO);
        }



        // POST: MenuArticolo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "Prezzo")] ARTICOLO aRTICOLO, HttpPostedFileBase File, string Prezzo)
        {
            if (ModelState.IsValid)
            {
                var existingArticolo = db.ARTICOLO.Find(aRTICOLO.IdArticolo);

                if (File != null)
                {
                    string Path = Server.MapPath("/Content/img/" + File.FileName);
                    File.SaveAs(Path);
                    existingArticolo.Foto = File.FileName;
                }

                existingArticolo.NomeArticolo = aRTICOLO.NomeArticolo;
                existingArticolo.Descrizione = aRTICOLO.Descrizione;

                // Converti il prezzo in un decimal
                decimal prezzo;
                if (Decimal.TryParse(Prezzo.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out prezzo))
                {
                    prezzo = Math.Truncate(100 * prezzo) / 100; // Tronca a due cifre decimali
                    existingArticolo.Prezzo = prezzo;
                }
                else
                {
                    ModelState.AddModelError("Prezzo", "Il prezzo non è in un formato valido.");
                    return View(aRTICOLO);
                }

                existingArticolo.InMagazzino = aRTICOLO.InMagazzino; // Aggiorna la quantità in magazzino

                // Controlla se la quantità in magazzino è 0
                if (existingArticolo.InMagazzino == 0)
                {
                    // Se è 0, imposta Esaurito su true
                    existingArticolo.Esaurito = true;
                }
                else
                {
                    // Se non è 0, imposta Esaurito su false
                    existingArticolo.Esaurito = false;
                }

                db.Entry(existingArticolo).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ListaAdmin");
            }

            // Se il modello non è valido, ricarica le categorie e restituisci la vista
            var categorie = db.CATEGORIA.ToList();
            ViewBag.IdCategoria = new SelectList(categorie, "IdCategoria", "NomeCategoria", aRTICOLO.CATEGORIA.IdCategoria);

            return View(aRTICOLO);
        }








        // GET: MenuArticolo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICOLO aRTICOLO = db.ARTICOLO.Find(id);
            if (aRTICOLO == null)
            {
                return HttpNotFound();
            }
            return View(aRTICOLO);
        }

        // POST: MenuArticolo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ARTICOLO aRTICOLO = db.ARTICOLO.Find(id);
            var preferiti = db.PREFERITI.Where(p => p.IdArticolo == id);
            db.PREFERITI.RemoveRange(preferiti);
            db.ARTICOLO.Remove(aRTICOLO);
            db.SaveChanges();
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
        // GET: MenuArticolo/Search
        [AllowAnonymous]
        public ActionResult Cerca(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var searchResults = db.ARTICOLO
                .Include(a => a.CATEGORIA)
                .Where(a => a.NomeArticolo.Contains(searchTerm) || a.CATEGORIA.NomeCategoria.Contains(searchTerm))
                .ToList();

            return View(searchResults);
        }

        [AllowAnonymous]
        public ActionResult PerCategoria(string categoria)
        {
            var articoli = db.ARTICOLO.Where(a => a.CATEGORIA.NomeCategoria == categoria).ToList();
            return View(articoli);
        }


        // GET: MenuArticolo/Articolo/5
        [AllowAnonymous]
        public ActionResult Articolo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICOLO articolo = db.ARTICOLO.Include(a => a.CATEGORIA).SingleOrDefault(a => a.IdArticolo == id);
            if (articolo == null)
            {
                return HttpNotFound();
            }
            return View(articolo);
        }



        public ActionResult CercaDaAmministratore(string searchTerm)
        {
            var model = new ListaAdminViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.Articoli = db.ARTICOLO.Where(a => a.NomeArticolo.Contains(searchTerm) || a.CATEGORIA.NomeCategoria.Contains(searchTerm)).ToList();
                model.Categorie = db.CATEGORIA.Where(c => c.NomeCategoria.Contains(searchTerm)).ToList();
            }
            else
            {
                model.Articoli = db.ARTICOLO.ToList();
                model.Categorie = db.CATEGORIA.ToList();
            }

            return View(model);
        }





    }

    public class MenuArticolo
    {
        public ARTICOLO Articolo { get; set; }
    }

}
