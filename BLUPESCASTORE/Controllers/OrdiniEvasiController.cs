using BLUPESCASTORE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BLUPESCASTORE.Controllers
{
    [Authorize]
    public class OrdiniEvasiController : Controller
    {
        // GET: OrdiniEvasi
        public ActionResult OrdiniEvasi()
        {
            return View();
        }

        // GET: OrdiniEvasi/Details/5
        public JsonResult GetEvaso()
        {
            List<string> evasoRecords = new List<string>();
            try
            {
                using (SqlConnection sql = Connessioni.GetConnection())
                {
                    sql.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Evaso FROM ORDINE WHERE Evaso = 'SI' OR Evaso = 'Si'", sql))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string evaso = reader.GetString(0);
                                evasoRecords.Add(evaso);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(evasoRecords, JsonRequestBehavior.AllowGet);
        }

        // GET: OrdiniEvasi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdiniEvasi/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdiniEvasi/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrdiniEvasi/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: OrdiniEvasi/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrdiniEvasi/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}