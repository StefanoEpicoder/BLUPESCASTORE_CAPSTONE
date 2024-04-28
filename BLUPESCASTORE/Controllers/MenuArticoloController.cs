using BLUPESCASTORE.Models;
using System.Data.Entity;
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
        public ActionResult Create(ARTICOLO aRTICOLO, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                // Imposta un valore predefinito per Esaurito se è null
                if (aRTICOLO.Esaurito == null)
                {
                    aRTICOLO.Esaurito = false; // o qualsiasi valore predefinito che desideri
                }

                File.SaveAs(Server.MapPath("/Content/img/" + File.FileName));
                aRTICOLO.Foto = File.FileName;

                db.ARTICOLO.Add(aRTICOLO);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            ARTICOLO aRTICOLO = db.ARTICOLO.Find(id);
            if (aRTICOLO == null)
            {
                return HttpNotFound();
            }

            // Recupera le categorie dal database
            var categorie = db.CATEGORIA.ToList();

            // Crea una SelectList con le categorie
            var listaCategorie = new SelectList(categorie, "IdCategoria", "NomeCategoria");

            // Passa la lista alla vista tramite ViewBag
            ViewBag.IdCategoria = listaCategorie;

            return View(aRTICOLO);
        }

        // POST: MenuArticolo/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ARTICOLO aRTICOLO, HttpPostedFileBase File)
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

                existingArticolo.Esaurito = aRTICOLO.Esaurito; // Aggiorna lo stato Esaurito

                existingArticolo.NomeArticolo = aRTICOLO.NomeArticolo;
                existingArticolo.Descrizione = aRTICOLO.Descrizione;
                existingArticolo.Prezzo = aRTICOLO.Prezzo;
                // Aggiungi qui eventuali altre proprietà che desideri aggiornare

                db.Entry(existingArticolo).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ListaAdmin");
            }

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

    }

    public class MenuArticolo
    {
        public ARTICOLO Articolo { get; set; }
    }

}
