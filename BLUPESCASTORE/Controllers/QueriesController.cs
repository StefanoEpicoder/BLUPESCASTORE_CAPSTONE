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
    public class QueriesController : Controller
    {
        // GET: Queries
        public ActionResult QueriesPage()
        {
            return View();
        }

        //JSON RESULT GET 

        public JsonResult GetOrdersTotalAmount()
        {
            decimal totalAmount = 0;
            try
            {
                using (SqlConnection sql = Connessioni.GetConnection())
                {
                    sql.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(TotaleImporto) AS TotalAmount FROM ORDINE", sql))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                totalAmount = reader.GetDecimal(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(totalAmount, JsonRequestBehavior.AllowGet);
        }

        //JSON RESULT GET TOTAL ORDINI EVASI
        /*public JsonResult GetEvaso()
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
        }*/


        // GET: Queries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Queries/Create
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

        // GET: Queries/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Queries/Edit/5
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

        // GET: Queries/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Queries/Delete/5
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