using Gestionale_Albergo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestionale_Albergo.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult ServiceList()
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            List<Servizi> Services = new List<Servizi>();

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetAllData", sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Servizi c = new Servizi();

                    c.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]);
                    c.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();
                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    c.Prezzo = Convert.ToDecimal(reader["TotServices"]);

                    Services.Add(c);

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(Services);
        }

        // GET: Service/Details/5
        public ActionResult Details(int id)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            List<Servizi> Services = new List<Servizi>();

            try
            {
                SqlCommand com = Connessione.GetCommand("SELECT * FROM SERVIZIO as S inner join Prenotazione as P " +
                    "on S.IdPrenotazione=P.IdPrenotazione inner join CLIENTI AS C ON P.IdCliente = C.IdCliente WHERE P.IdPrenotazione = @Id", sql);
                com.Parameters.AddWithValue("Id", id);

                SqlDataReader reader = com.ExecuteReader();



                while (reader.Read())
                {
                    Servizi c = new Servizi();
                    c.IdServizio = Convert.ToInt32(reader["IdServizio"]);
                    c.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();
                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    c.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]);
                    c.Prezzo = Convert.ToDecimal(reader["PrezzoTot"]);
                    c.Quantita = Convert.ToInt32(reader["Quantita"]);
                    c.Data = Convert.ToDateTime(reader["DataAggiunta"]);
                    c.Servizio = reader["TipoServizio"].ToString();
                    c.Tot_Service = c.TotServices(c.IdPrenotazione);
                    c.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();

                    Services.Add(c);

                    ViewBag.NrCamera = c.NrCamera;
                    ViewBag.TotServices = c.Tot_Service;
                    ViewBag.Cliente = c.Cliente;
                    ViewBag.Prenotazione = c.IdPrenotazione;
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(Services);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            ViewBag.ListaPrenotazioni = Prenotazioni.ListaPrenotazioni;
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        public ActionResult Create(Servizi s)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("CreateService", sql);


                com.Parameters.AddWithValue("TipoServizio", s.Servizio);
                com.Parameters.AddWithValue("Quantita", s.Quantita);
                com.Parameters.AddWithValue("PrezzoTot", s.Prezzo);
                com.Parameters.AddWithValue("DataAggiunta", s.Data);
                com.Parameters.AddWithValue("IdPrenotazione", s.IdPrenotazione);

                com.ExecuteNonQuery();
            }
            catch
            {

            }
            finally { sql.Close(); }
            return RedirectToAction("ServiceList");
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int id)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            Servizi c = new Servizi();
            ViewBag.ListaPrenotazioni = Prenotazioni.ListaPrenotazioni;
            try
            {
                SqlCommand com = Connessione.GetCommand("Select * from servizio where IdServizio=@id", sql);
                com.Parameters.AddWithValue("id", id);

                SqlDataReader reader = com.ExecuteReader();



                while (reader.Read())
                {

                    c.IdServizio = Convert.ToInt32(reader["IdServizio"]);
                    c.Servizio = reader["TipoServizio"].ToString();
                    c.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]);
                    c.Prezzo = Convert.ToDecimal(reader["PrezzoTot"]);
                    c.Quantita = Convert.ToInt32(reader["Quantita"]);
                    c.Data = Convert.ToDateTime(reader["DataAggiunta"]);


                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }
            return View(c);
        }

        // POST: Service/Edit/5
        [HttpPost]
        public ActionResult Edit(Servizi s)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();


            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("EditService", sql);

                com.Parameters.AddWithValue("IdServizio", s.IdServizio);
                com.Parameters.AddWithValue("TipoServizio", s.Servizio);
                com.Parameters.AddWithValue("Quantita", s.Quantita);
                com.Parameters.AddWithValue("PrezzoTot", s.Prezzo);
                com.Parameters.AddWithValue("DataAggiunta", s.Data);
                com.Parameters.AddWithValue("IdPrenotazione", s.IdPrenotazione);
                com.ExecuteNonQuery();

            }
            catch
            {

            }
            finally { sql.Close(); }
            return RedirectToAction("ServiceList");
        }

        // GET: Service/Delete/5
        public ActionResult Delete(int id)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            Servizi c = new Servizi();

            try
            {
                SqlCommand com = Connessione.GetCommand("Select * FROM SERVIZIO AS S INNER JOIN PRENOTAZIONE AS P " +
                    "ON P.IdPrenotazione = S.IdPrenotazione where IdServizio=@id", sql);
                com.Parameters.AddWithValue("id", id);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {

                    c.IdServizio = Convert.ToInt32(reader["IdServizio"]);
                    c.Servizio = reader["TipoServizio"].ToString();
                    c.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]);
                    c.Prezzo = Convert.ToDecimal(reader["PrezzoTot"]);
                    c.Quantita = Convert.ToInt32(reader["Quantita"]);
                    c.Data = Convert.ToDateTime(reader["DataAggiunta"]);
                    c.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();
                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]);

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(c);
        }

        // POST: Service/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Servizi s)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            SqlCommand com = Connessione.GetCommand("DELETE FROM servizio WHERE IdServizio=@id", sql);

            com.Parameters.AddWithValue("id", id);

            com.ExecuteNonQuery();

            sql.Close();


            return RedirectToAction("ServiceList");
        }
    }
}
