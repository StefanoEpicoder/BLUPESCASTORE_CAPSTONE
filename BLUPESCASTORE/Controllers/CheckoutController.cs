using BLUPESCASTORE.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BLUPESCASTORE.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult ListaCk()
        {
            SqlConnection sql = new SqlConnection("Data Source=.;Initial Catalog=BLUPESCASTORE;Integrated Security=True");
            sql.Open();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            try
            {
                SqlCommand com = new SqlCommand("SELECT * FROM USER INNER JOIN DETTAGLIO ON USER.Id = DETTAGLIO.UserId", sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectListItem p = new SelectListItem
                    {
                        Value = Convert.ToInt32(reader["IdDettaglio"]).ToString(),
                        Text = reader["Nome"].ToString(),
                    };
                    selectListItems.Add(p);
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally
            {
                sql.Close();
            }
            return View(selectListItems);
        }
        public ActionResult Details(int id)
        {
            SqlConnection sql = new SqlConnection("Data Source=.;Initial Catalog=BLUPESCASTORE;Integrated Security=True");
            sql.Open();
            SelectListItem p = new SelectListItem();
            try
            {
                SqlCommand com = new SqlCommand("SELECT * FROM USER INNER JOIN DETTAGLIO ON USER.Id = DETTAGLIO.UserId WHERE IdDettaglio = " + id, sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    p.Value = Convert.ToInt32(reader["IdDettaglio"]).ToString();
                    p.Text = reader["Nome"].ToString();
                    p.Text = reader["Cognome"].ToString();
                    p.Text = reader["Cod_Fiscale"].ToString();
                    p.Text = reader["Citta"].ToString();
                    p.Text = reader["Prov"].ToString();
                    p.Text = reader["Tel_Cell"].ToString();
                    p.Text = reader["Email"].ToString();
                    p.Text = reader["IdOrdine"].ToString();
                    p.Text = reader["IdDettaglio"].ToString();

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally
            {
                sql.Close();
            }
            return View(p);
        }
    }
}