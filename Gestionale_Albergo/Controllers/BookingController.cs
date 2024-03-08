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
    public class BookingController : Controller
    {
        // GET: Booking
        public ActionResult ListaPren()
        {
            SqlConnection sql = Connessione.GetConnection(); // Stabilisci una connessione al database
            sql.Open(); // Apri la connessione al database
            List<Prenotazioni> Bookings = new List<Prenotazioni>(); // Crea una lista per memorizzare le prenotazioni

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetBookings", sql); // Crea un comando SQL per eseguire la stored procedure "GetBookings"
                SqlDataReader reader = com.ExecuteReader(); // Esegui il comando SQL e ottieni il data reader

                while (reader.Read()) // Loop attraverso il data reader
                {
                    Prenotazioni p = new Prenotazioni // Crea una nuova istanza della classe "Prenotazioni"
                    {
                        IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]), // Ottieni l'ID della prenotazione dal data reader e convertilo in un intero
                        IdCliente = Convert.ToInt32(reader["IdCliente"]), // Ottieni l'ID del cliente dal data reader e convertilo in un intero
                        Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(), // Ottieni il cognome e il nome del cliente dal data reader e concatenali
                        NrCamera = Convert.ToInt32(reader["NrCamera"]), // Ottieni il numero della camera dal data reader e convertilo in un intero
                        DataPren = Convert.ToDateTime(reader["DataPrenotazione"]), // Ottieni la data della prenotazione dal data reader e convertila in un oggetto DateTime
                        CheckIn = Convert.ToDateTime(reader["DataArrivo"]), // Ottieni la data di check-in dal data reader e convertila in un oggetto DateTime
                        CheckOut = Convert.ToDateTime(reader["DataUscita"]), // Ottieni la data di check-out dal data reader e convertila in un oggetto DateTime
                        Acconto = Convert.ToDecimal(reader["Acconto"]), // Ottieni l'importo dell'acconto dal data reader e convertilo in un decimale
                        Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]), // Ottieni il prezzo della camera dal data reader e convertilo in un decimale
                        Pensione = reader["TipoPensione"].ToString(), // Ottieni il tipo di pensione dal data reader come stringa
                        Tot = Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"])), // Calcola il totale dei servizi per la prenotazione utilizzando un metodo statico della classe "Prenotazioni"
                        Saldo = Prenotazioni.DaSaldare(Convert.ToDecimal(reader["PrezzoSoggiorno"]), Convert.ToDecimal(reader["Acconto"]), Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"]))) // Calcola il saldo da pagare per la prenotazione utilizzando un metodo statico della classe "Prenotazioni"
                    };
                    Bookings.Add(p); // Aggiungi la prenotazione alla lista
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message; // Memorizza il messaggio di errore nella ViewBag
            }
            finally { sql.Close(); } // Chiudi la connessione al database

            return View(Bookings); // Restituisci la lista delle prenotazioni alla vista
        }

        // GET: Booking/Details/5
        public ActionResult Details(int id)
        {
            SqlConnection sql = Connessione.GetConnection(); // Stabilisci una connessione al database
            sql.Open(); // Apri la connessione al database
            Prenotazioni b = new Prenotazioni(); // Crea una nuova istanza della classe "Prenotazioni"

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetBookingById", sql); // Crea un comando SQL per eseguire la stored procedure "GetBookingById"
                com.Parameters.AddWithValue("IdPrenotazione", id); // Imposta il valore del parametro per l'ID della prenotazione

                SqlDataReader reader = com.ExecuteReader(); // Esegui il comando SQL e ottieni il data reader

                while (reader.Read()) // Loop attraverso il data reader
                {
                    b.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]); // Ottieni l'ID della prenotazione dal data reader e convertilo in un intero
                    b.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(); // Ottieni il cognome e il nome del cliente dal data reader e concatenali
                    b.NrCamera = Convert.ToInt32(reader["NrCamera"]); // Ottieni il numero della camera dal data reader e convertilo in un intero
                    b.DataPren = Convert.ToDateTime(reader["DataPrenotazione"]); // Ottieni la data della prenotazione dal data reader e convertila in un oggetto DateTime
                    b.CheckIn = Convert.ToDateTime(reader["DataArrivo"]); // Ottieni la data di check-in dal data reader e convertila in un oggetto DateTime
                    b.CheckOut = Convert.ToDateTime(reader["DataUscita"]); // Ottieni la data di check-out dal data reader e convertila in un oggetto DateTime
                    b.Acconto = Convert.ToDecimal(reader["Acconto"]); // Ottieni l'importo dell'acconto dal data reader e convertilo in un decimale
                    b.Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]); // Ottieni il prezzo della camera dal data reader e convertilo in un decimale
                    b.Pensione = reader["TipoPensione"].ToString(); // Ottieni il tipo di pensione dal data reader come stringa
                    b.Tot = Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"])); // Calcola il totale dei servizi per la prenotazione utilizzando un metodo statico della classe "Prenotazioni"
                    b.Saldo = Prenotazioni.DaSaldare(Convert.ToDecimal(reader["PrezzoSoggiorno"]), Convert.ToDecimal(reader["Acconto"]), Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"]))); // Calcola il saldo da pagare per la prenotazione utilizzando un metodo statico della classe "Prenotazioni"
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message; // Memorizza il messaggio di errore nella ViewBag
            }
            finally { sql.Close(); } // Chiudi la connessione al database

            return View(b); // Restituisci i dettagli della prenotazione alla vista
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            ViewBag.TipoPensioni = Pensione.ListaPensione;
            ViewBag.ListaClienti = Clienti.ListaClienti;
            ViewBag.ListaCamere = Camere.ListaCamere;

            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(Prenotazioni p)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("CreateBooking", sql);


                com.Parameters.AddWithValue("NrCamera", p.NrCamera);
                com.Parameters.AddWithValue("IdPensione", p.Pensione);
                com.Parameters.AddWithValue("IdCliente", p.IdCliente);
                com.Parameters.AddWithValue("DataPrenotazione", p.DataPren);
                com.Parameters.AddWithValue("DataArrivo", p.CheckIn);
                com.Parameters.AddWithValue("DataUscita", p.CheckOut);
                com.Parameters.AddWithValue("Acconto", p.Acconto);
                com.Parameters.AddWithValue("PrezzoSoggiorno", p.Prezzo);

                com.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            SqlConnection conn = Connessione.GetConnection();
            conn.Open();

            int nuovoId = 0;

            SqlCommand command = Connessione.GetCommand("SELECT top(1) * FROM PRENOTAZIONE where IdCliente=@IdCliente ORDER BY IdPrenotazione Desc", conn);

            command.Parameters.AddWithValue("IdCliente", p.IdCliente);

            SqlDataReader read = command.ExecuteReader();

            while (read.Read())
            {
                nuovoId = Convert.ToInt32(read["IdPrenotazione"]);
            }

            conn.Close();

            SqlConnection con = Connessione.GetConnection();
            con.Open();


            SqlCommand comm = Connessione.GetCommand("UPDATE CLIENTI set IdPrenotazione=@Id where IdCliente=@IdCliente", con);

            comm.Parameters.AddWithValue("IdCliente", p.IdCliente);
            comm.Parameters.AddWithValue("Id", nuovoId);

           int row = comm.ExecuteNonQuery();

            con.Close();
            return RedirectToAction("ListaPren");
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            Prenotazioni b = new Prenotazioni();

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetBookingById", sql);
                com.Parameters.AddWithValue("IdPrenotazione", id);

                SqlDataReader reader = com.ExecuteReader();



                while (reader.Read())
                {

                    b.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]);
                    b.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();
                    b.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                    b.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    b.DataPren = Convert.ToDateTime(reader["DataPrenotazione"]);
                    b.CheckIn = Convert.ToDateTime(reader["DataArrivo"]);
                    b.CheckOut = Convert.ToDateTime(reader["DataUscita"]);
                    b.Acconto = Convert.ToDecimal(reader["Acconto"]);
                    b.Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]);
                  
                    ViewBag.TipoPensioni = Pensione.ListaPensione;
                    ViewBag.ListaClienti = Clienti.ListaClienti;
                    ViewBag.ListaCamere = Camere.ListaCamere;
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(b);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(Prenotazioni p)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("EditBooking", sql);

                com.Parameters.AddWithValue("IdPrenotazione", p.IdPrenotazione);
                com.Parameters.AddWithValue("NrCamera", p.NrCamera);
                com.Parameters.AddWithValue("IdPensione", p.Pensione);
                com.Parameters.AddWithValue("IdCliente", p.IdCliente);
                com.Parameters.AddWithValue("DataPrenotazione", p.DataPren);
                com.Parameters.AddWithValue("DataArrivo", p.CheckIn);
                com.Parameters.AddWithValue("DataUscita", p.CheckOut);
                com.Parameters.AddWithValue("Acconto", p.Acconto);
                com.Parameters.AddWithValue("PrezzoSoggiorno", p.Prezzo);

                com.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }
            return RedirectToAction("ListaPren");
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            Prenotazioni b = new Prenotazioni();

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetBookingById", sql);
                com.Parameters.AddWithValue("IdPrenotazione", id);

                SqlDataReader reader = com.ExecuteReader();



                while (reader.Read())
                {

                    b.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]);
                    b.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();
                    b.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    b.DataPren = Convert.ToDateTime(reader["DataPrenotazione"]);
                    b.CheckIn = Convert.ToDateTime(reader["DataArrivo"]);
                    b.CheckOut = Convert.ToDateTime(reader["DataUscita"]);
                    b.Acconto = Convert.ToDecimal(reader["Acconto"]);
                    b.Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]);
                    b.Pensione = reader["TipoPensione"].ToString();
                    b.Tot = Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"]));
                    b.Saldo = Prenotazioni.DaSaldare(Convert.ToDecimal(reader["PrezzoSoggiorno"]), Convert.ToDecimal(reader["Acconto"]), Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"])));

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(b);
        }

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, int idCliente)
        {

        //ELIMINA SERVIZI COLLEGATI

            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

                SqlCommand com = Connessione.GetCommand("DELETE FROM Servizio WHERE IdPrenotazione=@Id", sql);

                com.Parameters.AddWithValue("Id", id);

                com.ExecuteNonQuery();

            sql.Close();

         //ELIMINA PRENOTAZIONE DALLA TAB CLIENTI COLLEGATA
            SqlConnection conn = Connessione.GetConnection();
            conn.Open();

        
                SqlCommand command = Connessione.GetCommand("UPDATE CLIENTI set IdPrenotazione=null where IdPrenotazione=@Id", conn);

            command.Parameters.AddWithValue("Id", id);

            command.ExecuteNonQuery();

            conn.Close();

         //ELIMINA PRENOTAZIONE
            SqlConnection con = Connessione.GetConnection();
            con.Open();

            try
            {
                SqlCommand comm = Connessione.GetCommand("DELETE FROM PRENOTAZIONE WHERE IdPrenotazione=@Id", con);

                comm.Parameters.AddWithValue("Id", id);

                comm.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { con.Close(); }

        //RICERCA ULTIMA PRENOTAZIONE DI QUEL CLIENTE
            SqlConnection sq = Connessione.GetConnection();
            sq.Open();

            int nuovoId = 0;

            SqlCommand co = Connessione.GetCommand("SELECT top(1) * FROM PRENOTAZIONE where IdCliente=@IdCliente ORDER BY IdPrenotazione Desc", sq);

            co.Parameters.AddWithValue("IdCliente", idCliente);

            SqlDataReader read = co.ExecuteReader();

            if (read.HasRows)

            {
                while (read.Read())
                {
                    if (Convert.ToInt32(read["IdPrenotazione"]) != id)
                    {
                        nuovoId = Convert.ToInt32(read["IdPrenotazione"]);
                    }

                }
            }

            sq.Close();

         //AGGIORNA PRENOTAZIONE ALL'ULTIMA ESISTENTE NELLA TAB CLIENTE
            if (nuovoId > 0)
            {

                SqlConnection connec = Connessione.GetConnection();
                connec.Open();


                SqlCommand comma = Connessione.GetCommand("UPDATE CLIENTI set IdPrenotazione=@nuovoId where IdCliente=@IdCliente", connec);

                comma.Parameters.AddWithValue("IdCliente", idCliente);
                comma.Parameters.AddWithValue("nuovoID", nuovoId);

                comma.ExecuteNonQuery();

                connec.Close();
  
            }

            return RedirectToAction("ListaPren");
        }
    }
}
